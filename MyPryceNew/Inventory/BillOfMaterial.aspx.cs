using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Linq;

public partial class Inventory_BillOfMaterial : BasePage
{
    #region Defined Class Object
    set_PrinterMaster objprinter = null;
    BillOfMaterial ObjInvBOM = null;
    Inv_ProductMaster ObjInvProductMaster = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_OptionCategoryMaster ObjOpCate = null;
    Common cmn = null;
    SystemParameter ObjSysPeram = null;
    CurrencyMaster objCurrency = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();
    DataAccessClass objDa = null;
    ProductOptionCategoryDetail objProOpCatedetail = null;

    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objprinter = new set_PrinterMaster(Session["DBConnection"].ToString());
        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        ObjInvProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjOpCate = new Inv_OptionCategoryMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objProOpCatedetail = new ProductOptionCategoryDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/BillOfMaterial.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            Session["dtFilter_BOM"] = null;
            Session["DtModel"] = null;
            Session["Modelid"] = null;
            ViewState["ExchangeRate"] = null;
            Reset_Child();
            FillModelGrid();
            txtDate.Text = DateTime.Now.ToString(ObjSysPeram.SetDateFormat());
            CalendarExtender_date.Format = ObjSysPeram.SetDateFormat();
            ViewState["CurrencyId"] = null;
            FillPrinter();
        }
    }

    public void FillPrinter()
    {

        DataTable dtprinter = objprinter.GetPrinterMaster();
        chkPrinter.DataSource = dtprinter;
        chkPrinter.DataTextField = "Printer_Name";
        chkPrinter.DataValueField = "Printer_id";
        chkPrinter.DataBind();


    }


    #region System Defined Function
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strprinter = "";
        btnSave.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        foreach (ListItem li in chkPrinter.Items)
        {
            if (li.Selected)
            {
                if (strprinter == "")
                {
                    strprinter = li.Value;
                }
                else
                {
                    strprinter = strprinter + "," + li.Value;
                }
            }
        }


        //if (ddlPrinter.SelectedIndex > 0)
        //    strprinter = ddlPrinter.SelectedValue;

        if (txtDate.Text == "")
        {
            DisplayMessage("Enter Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
            btnSave.Enabled = true;
            return;
        }
        else
        {

            try
            {
                ObjSysPeram.getDateForInput(txtDate.Text);

            }
            catch
            {
                DisplayMessage("Enter Date in Format " + ObjSysPeram.SetDateFormat() + "");
                txtDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDate);
                btnSave.Enabled = true;
                return;
            }

        }

        string SubProductId = "0";
        string ModelId;
        try
        {
            ModelId = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), "True"), "Model_No='" + txtModelNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
        }
        catch
        {
            ModelId = "0";

        }
        if (txtSubProduct.Text != "")
        {
            if (txtSubProduct.Text != "0")
            {
                SubProductId = new DataView(ObjInvProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName like '" + txtSubProduct.Text.Trim() + "%'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["ProductId"].ToString();
            }
        }

        if (SubProductId == "")
        {
            SubProductId = "0";
        }

        if (txtOption.Text == "")
        {
            DisplayMessage("Enter Option");

            txtOption.Text = "";
            txtOption.Focus();
            btnSave.Enabled = true;
            return;
        }
        else
        {
            if (txtOption.Text.Trim() == "0")
            {
                DisplayMessage("This Option  is already in use");

                txtOption.Focus();
                btnSave.Enabled = true;
                return;
            }
        }
        if (txtOptCatId.Text == "")
        {
            DisplayMessage("Enter Option Category");

            txtOptCatId.Text = "";
            txtOptCatId.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtShortDesc.Text == "")
        {
            DisplayMessage("Enter Option Short Description");

            txtShortDesc.Text = "";
            txtShortDesc.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtUnitPrice.Text == "")
        {
            DisplayMessage("Enter Unit Price");

            txtUnitPrice.Text = "";
            txtUnitPrice.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtQty.Text == "")
        {
            DisplayMessage("Enter Quantity");

            txtQty.Text = "";
            txtQty.Focus();
            btnSave.Enabled = true;
            return;
        }
        string str = "False";
        if (chkDefault.Checked)
        {
            str = "True";
        }
        string OptionCateId = string.Empty;
        if (txtOptCatId.Text != "")
        {
            OptionCateId = new DataView(ObjOpCate.GetOptionCategoryTrueAll(Session["CompId"].ToString().ToString()), "EName='" + txtOptCatId.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["OptionCategoryId"].ToString();


        }



        //this code is created by jitendra upadhyay to save unti price in local or company currency in field1 column in inv_bom table
        //code start

        string LocalUnitPrice = string.Empty;

        if (txtUnitPrice.Text != "" && txtUnitPrice.Text != "0")
        {
            LocalUnitPrice = ObjSysPeram.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), (float.Parse(ViewState["ExchangeRate"].ToString()) * float.Parse(txtUnitPrice.Text)).ToString());
        }
        else
        {
            LocalUnitPrice = "0";
        }



        //code end

        string StrOptionCategorySNo = string.Empty;

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (hdnDetailEdit.Value == "")
            {
                DataTable dtPS = new DataView(ObjInvBOM.BOM_All(Session["CompId"].ToString().ToString(), ref trns), "ModelId='" + ModelId.Trim() + "'", "Field1 desc", DataViewRowState.CurrentRows).ToTable();

                if (dtPS.Rows.Count != 0)
                {

                    DataTable dtTemp = new DataView(dtPS, "OptionCategoryId='" + OptionCateId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtTemp.Rows.Count > 0)
                    {

                        StrOptionCategorySNo = dtTemp.Rows[0]["Field1"].ToString();


                        if (checkOptionId(ModelId, OptionCateId, txtOption.Text.Trim(), ref trns).Rows.Count != 0)
                        {

                            txtOption.Text = "";
                            DisplayMessage("Option Id Already Exists");
                            btnSave.Enabled = true;
                            return;
                        }
                    }
                    else
                    {
                        StrOptionCategorySNo = (Convert.ToInt32(dtPS.Rows[0]["Field1"].ToString()) + 1).ToString();

                    }
                }
                else
                {
                    StrOptionCategorySNo = "1";
                }


                int b = ObjInvBOM.Insert_BOM(Session["CompId"].ToString().ToString(), ModelId.Trim(), ObjSysPeram.getDateForInput(txtDate.Text).ToString(), ddlTransType.SelectedValue.ToString().Trim(), SubProductId.ToString().Trim(), txtModelNo.Text.ToString().Trim(), txtOption.Text.ToString().Trim(), txtOptionDesc.Text.ToString().Trim(), txtShortDesc.Text.ToString().Trim(), OptionCateId.ToString().Trim(), txtUnitPrice.Text.ToString().Trim(), txtQty.Text.ToString().Trim(), str.ToString(), StrOptionCategorySNo.ToString(), LocalUnitPrice, strprinter, "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                //insert in inv_label_filter table 
                foreach (ListItem li in chkSelectedItems.Items)
                {
                    if (li.Selected)
                    {
                        string strsql = "insert into Inv_Label_Filter values(" + li.Value + "," + b.ToString() + ")";
                        objDa.execute_Command(strsql, ref trns);
                    }
                }

            }
            else
            {
                DataTable dtPSID = checkOptionId(ModelId, OptionCateId, txtOption.Text.Trim(), ref trns);
                //DataTable dtPSID = new DataView(ObjInvBOM.BOM_All(Session["CompId"].ToString().ToString()), "ModelId='" + ModelId.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                //dtPSID = new DataView(dtPSID, "OptionId like '%[" + txtOption.Text.Trim() + "]%' and OptionCategoryId='" + OptionCateId.ToString() + "' collate Latin1_General_CS_AS ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtPSID.Rows.Count != 0)
                {
                    if (dtPSID.Rows[0]["TransId"].ToString() != hdnDetailEdit.Value.ToString())
                    {
                        DisplayMessage("Option Id Already Exists");


                        txtOption.Focus();
                        btnSave.Enabled = true;
                        return;
                    }

                }
                ObjInvBOM.Update_BOM(hdnDetailEdit.Value.ToString(), Session["CompId"].ToString().ToString().Trim(), ModelId.ToString().Trim(), ObjSysPeram.getDateForInput(txtDate.Text).ToString(), ddlTransType.SelectedValue.ToString().Trim(), SubProductId.ToString().Trim(), txtModelNo.Text.ToString().Trim(), txtOption.Text.ToString().Trim(), txtOptionDesc.Text.ToString().Trim(), txtShortDesc.Text.ToString().Trim(), OptionCateId.ToString().Trim(), txtUnitPrice.Text.ToString().Trim(), txtQty.Text.ToString().Trim(), str.ToString(), "", LocalUnitPrice, strprinter, "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                //insert in inv_label_filter table on update mode
                string strsql = string.Empty;
                //first delete of exisiting record
                strsql = "delete from Inv_Label_Filter where Child_Id=" + hdnDetailEdit.Value.ToString() + "";
                objDa.execute_Command(strsql, ref trns);

                //insert
                foreach (ListItem li in chkSelectedItems.Items)
                {
                    if (li.Selected)
                    {
                        strsql = "insert into Inv_Label_Filter values(" + li.Value + "," + hdnDetailEdit.Value.ToString() + ")";
                        objDa.execute_Command(strsql, ref trns);
                    }
                }

            }


            //here we commit transaction when all data insert and update proper 
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            ModelResesequence();
            ddlTransType.Enabled = false;
            txtDate.Enabled = false;
            txtModelNo.Enabled = false;
            txtModelName.Enabled = false;
            Reset_Child();
            pnlChlidGrid.Visible = true;
            rdoOption.Focus();
            chkSelectedItems.Items.Clear();
            fillGrid();
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
            btnSave.Enabled = true;
            return;
        }
        btnSave.Enabled = true;
    }

    public DataTable checkOptionId(string ModelId, string optionCategoryId, string OptionId)
    {
        DataTable dt = new DataTable();
        string sql = string.Empty;
        sql = " select * from Inv_BOM where Company_id=" + Session["CompId"].ToString() + " and ModelId=" + ModelId + " and OptionCategoryID=" + optionCategoryId + " and OptionID like '%" + OptionId + "%' collate Latin1_General_CS_AS";
        return objDa.return_DataTable(sql);

    }
    public DataTable checkOptionId(string ModelId, string optionCategoryId, string OptionId, ref SqlTransaction trns)
    {
        DataTable dt = new DataTable();
        string sql = string.Empty;
        sql = " select * from Inv_BOM where Company_id=" + Session["CompId"].ToString() + " and ModelId=" + ModelId + " and OptionCategoryID=" + optionCategoryId + " and OptionID like '%" + OptionId + "%' collate Latin1_General_CS_AS";
        return objDa.return_DataTable(sql, ref trns);

    }
    protected void btnFinalSave_Click(object sender, EventArgs e)
    {
        ddlTransType.Enabled = true;
        txtDate.Enabled = true;
        txtModelNo.Enabled = true;
        txtModelName.Enabled = true;
        Reset_Child();
        txtModelNo.Text = "";
        ddlTransType.SelectedValue = "A";
        CalendarExtender_date.Format = ObjSysPeram.SetDateFormat();
        txtDate.Text = DateTime.Now.ToString(ObjSysPeram.SetDateFormat());

        txtModelName.Text = "";
        gvProductSpecsChild.DataSource = null;
        gvProductSpecsChild.DataBind();
        pnlChlidGrid.Visible = false;
        txtValue.Focus();
        btnRefresh_Click(null, null);
        rdoOption.Enabled = true;
        rdoStock.Enabled = true;
        try
        {
            gvProductSpecsChild.Columns[0].Visible = true;
            gvProductSpecsChild.Columns[1].Visible = true;
        }
        catch
        {
        }
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void rdoStockOption_CheckedChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {
            if (rdoOption.Checked)
            {
                trStock.Visible = false;
                txtSubProduct.Text = "";
                txtSubProduct.Focus();
                txtSubProduct.ValidationGroup = "";
            }
            else
            {
                if (rdoStock.Checked)
                {
                    trStock.Visible = true;
                    txtSubProduct.ValidationGroup = "Save";

                }

            }
            pnlChlidGrid.Visible = false;
            pnlOptStock.Visible = true;

        }
        else
        {
            DisplayMessage("Enter Model No Or Model Name");
            txtModelNo.Focus();
            rdoOption.Checked = false;
            rdoStock.Checked = false;
        }
        lblFiltertext.Text = "";
        txtOption.Enabled = true;
        txtOptCatId.Enabled = true;

    }
    protected void btnDetailEdit_Command(object sender, CommandEventArgs e)
    {

        hdnDetailEdit.Value = e.CommandArgument.ToString();

        DataTable dt = ObjInvBOM.BOM_ById(Session["CompId"].ToString().ToString(), hdnDetailEdit.Value);
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["SubProductId"].ToString() == "0")
            {
                rdoStock.Checked = false;
                rdoOption.Checked = true;
            }
            else
            {
                rdoOption.Checked = false;
                rdoStock.Checked = true;

            }
            rdoStockOption_CheckedChanged(null, null);
            txtSubProduct.Text = getProductName(dt.Rows[0]["SubProductId"].ToString());
            txtOption.Text = dt.Rows[0]["OptionId"].ToString();
            txtOptCatId.Text = GetOpCateName(dt.Rows[0]["OptionCategoryId"].ToString()).ToString();
            txtOptCatId_TextChanged(null, null);
            string strField3 = dt.Rows[0]["Field3"].ToString();
            foreach (ListItem li in chkPrinter.Items)
            {

                if (strField3.Split(',').Contains(li.Value))
                {
                    li.Selected = true;
                }
                else
                {
                    li.Selected = false;
                }
            }


            try
            {
                ddlPrinter.SelectedValue = dt.Rows[0]["Field3"].ToString();
            }
            catch
            {
                ddlPrinter.SelectedIndex = 0;
            }
            txtOptionDesc.Text = dt.Rows[0]["OptionDescription"].ToString();
            txtShortDesc.Text = dt.Rows[0]["ShortDescription"].ToString();
            txtUnitPrice.Text = SetDecimal(dt.Rows[0]["UnitPrice"].ToString());
            txtQty.Text = SetDecimal(dt.Rows[0]["Quantity"].ToString());
            chkDefault.Checked = Convert.ToBoolean(dt.Rows[0]["PDefault"].ToString());
            rdoOption.Focus();
            string sql = "select ModelNo from Inv_ProductMaster where ModelNo=" + Session["Modelid"].ToString() + "";
            if (objDa.return_DataTable(sql).Rows.Count > 0)
            {
                txtOption.Enabled = false;
                txtOptCatId.Enabled = false;
            }
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset_Child();
        pnlChlidGrid.Visible = true;
        ddlTransType.Focus();
        chkSelectedItems.Items.Clear();
        txtOption.Enabled = true;
        txtOptCatId.Enabled = true;
        lblprinter.Visible = false;
        ddlPrinter.Visible = false;
        chkPrinter.Visible = false;

    }

    protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTransType.SelectedValue == "A")
        {
            Session["IT"] = ddlTransType.SelectedValue;
        }
        else
        {
            if (ddlTransType.SelectedValue == "K")
            {
                Session["IT"] = ddlTransType.SelectedValue;
            }


        }
        ddlTransType.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string sql = "select * from Inv_ProductMaster where ModelNo=" + Session["Modelid"].ToString() + "";
        if (objDa.return_DataTable(sql).Rows.Count > 0)
        {
            DisplayMessage("This record is in use, you can not delete !");
            return;
        }

        ObjInvBOM.DeleteOrRestore_BOM(e.CommandArgument.ToString(), Session["CompId"].ToString().ToString(), false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        fillGrid();
        try
        {
            ((LinkButton)((GridViewRow)((LinkButton)sender).Parent.Parent).FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            rdoOption.Focus();
        }

        //delete in label filter table
        //delete filter criteria

        string strsql = string.Empty;
        //first delete of exisiting record
        strsql = "delete from Inv_Label_Filter where Child_Id=" + e.CommandArgument.ToString() + "";
        objDa.execute_Command(strsql);
        ModelResesequence();

    }

    public void ModelResesequence()
    {
        DataTable dtModel = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), "True"), "Model_No='" + txtModelNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string ModelId = dtModel.Rows[0]["Trans_Id"].ToString();
        DataTable dt = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId);
        if (dt.Rows.Count > 0)
        {
            try
            {
                dt = new DataView(dt, "", "Field1 asc", DataViewRowState.CurrentRows).ToTable(true, "Field1");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sql = "update Inv_BOM set Field1='" + (i + 1).ToString() + "' where ModelId=" + ModelId + " and Field1 ='" + dt.Rows[i]["Field1"].ToString() + "'";
                    objDa.execute_Command(sql);
                }
            }
            catch
            {

            }
        }
    }
    protected void gvProductSpecsChild_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductSpecsChild.PageIndex = e.NewPageIndex;
        fillGrid();
        gvProductSpecsChild.BottomPagerRow.Focus();
    }


    protected void gvModelMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_BOM"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");

        //AllPageCode();
        //gvModelMaster.BottomPagerRow.Focus();

    }
    protected void gvModelMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_BOM"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_BOM"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");
        //AllPageCode();
        //gvModelMaster.HeaderRow.Focus();

    }

    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {

            DataTable dt = new DataView(ObjInvModelMaster.GetModelMasterByModelNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True", txtModelNo.Text.ToString().Trim()), "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                txtModelName.Text = dt.Rows[0]["Model_Name"].ToString();
                lblUnitPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Unit_Price, Session["DBConnection"].ToString());
                ViewState["CurrencyId"] = dt.Rows[0]["Field4"].ToString();
                Session["Modelid"] = dt.Rows[0]["Trans_Id"].ToString();
                if (ViewState["CurrencyId"].ToString() == Session["CurrencyId"].ToString())
                {
                    ViewState["ExchangeRate"] = "1";
                }
                else
                {

                    //try
                    //{
                    ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(ViewState["CurrencyId"].ToString(), Session["CurrencyId"].ToString(), Session["DBConnection"].ToString());
                    //    ViewState["ExchangeRate"] = (obj.ConversionRate((Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(ViewState["CurrencyId"].ToString()).Rows[0]["Currency_Code"].ToString()), (Currency)System.Enum.Parse(Currency.GetType(), objCurrency.GetCurrencyMasterById(Session["CurrencyId"].ToString()).Rows[0]["Currency_Code"].ToString())).ToString());
                    //}
                    //catch
                    //{
                    //    if (ViewState["CurrencyId"].ToString() != "")
                    //    {
                    //        DataTable dtcurr = new DataView(objCurrency.GetCurrencyMaster(), "Currency_ID='" + ViewState["CurrencyId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //        if (dtcurr.Rows.Count != 0)
                    //        {
                    //            ViewState["ExchangeRate"] = dtcurr.Rows[0]["Currency_Value"].ToString();
                    //        }
                    //        else
                    //        {
                    //            ViewState["ExchangeRate"] = "1";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        ViewState["ExchangeRate"] = "1";
                    //    }
                    //}
                }

                fillGrid();
                try
                {
                    gvProductSpecsChild.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Unit_Price, Session["DBConnection"].ToString());
                }
                catch
                {
                }
                pnlChlidGrid.Visible = true;
                rdoOption.Focus();
            }
            else
            {
                txtModelNo.Text = "";
                DisplayMessage("Select Model No");
                txtModelNo.Focus();

            }
        }


    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, (CommandEventArgs)e);
        rdoOption.Enabled = false;
        rdoStock.Enabled = false;
        try
        {
            gvProductSpecsChild.Columns[0].Visible = false;
            gvProductSpecsChild.Columns[1].Visible = false;
        }
        catch
        {
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        //GridViewRow Row = (GridViewRow)((ImageButton)sender).Parent.Parent; ;



        DataTable dtModel = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString(), "True");
        txtModelName.Text = dtModel.Rows[0]["Model_Name"].ToString();
        txtModelNo.Text = dtModel.Rows[0]["Model_No"].ToString();

        lblUnitPrice.Text = SystemParameter.GetCurrencySmbol(dtModel.Rows[0]["Field4"].ToString(), Resources.Attendance.Unit_Price, Session["DBConnection"].ToString());
        txtModelNo_TextChanged(null, null);
        ddlTransType.Enabled = false;
        txtDate.Enabled = false;
        txtModelNo.Enabled = false;
        txtModelName.Enabled = false;
        btnReset_Click(null, null);
        try
        {
            gvProductSpecsChild.Columns[0].Visible = true;
            gvProductSpecsChild.Columns[1].Visible = true;
        }
        catch
        {
        }
        if (((LinkButton)sender).ID == "btnEdit")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
            DataView view = new DataView((DataTable)Session["DtModel"], condition, "", DataViewRowState.CurrentRows);

            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvModelMaster, view.ToTable(), "", "");

            Session["dtFilter_BOM"] = view.ToTable();
            //AllPageCode();
        }
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
        FillModelGrid();
        txtValue.Focus();
    }

    protected void txtOptCatId_TextChanged(object sender, EventArgs e)
    {
        lblprinter.Visible = false;
        ddlPrinter.Visible = false;
        chkPrinter.Visible = false;


        if (txtOptCatId.Text != "")
        {
            DataTable dt = new DataView(ObjOpCate.GetOptionCategoryTrueAll(Session["CompId"].ToString().ToString()), "EName='" + txtOptCatId.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Option Category ");
                txtOptCatId.Text = "";
                txtOptCatId.Focus();
                return;
            }

            else
            {
                txtShortDesc.Focus();
            }


            string strsql = string.Empty;
            //for get categorr id in option category detail table
            strsql = "select distinct Field2 as CoreCategoryId,QtyCategoryId,Field1 as PackingCategoryId,QtyCategoryId,Field3 as PerforationCategoryId from dbo.Inv_ProductOptionCategoryDetail";
            DataTable dtDetail = objDa.return_DataTable(strsql);
            if (dtDetail.Rows.Count > 0)
            {
                chkSelectedItems.Items.Clear();
                //if selected category is core then we showing label size for inv_model_labelsize table
                if (dt.Rows[0]["OptionCategoryID"].ToString() == dtDetail.Rows[0]["CoreCategoryId"].ToString() || dt.Rows[0]["OptionCategoryID"].ToString() == dtDetail.Rows[0]["PerforationCategoryId"].ToString())
                {
                    lblprinter.Visible = true;
                    chkPrinter.Visible = true;
                    //here we select record from label model

                    //code start
                    string sql = "select * from Inv_Model_LabelSize where model_id=" + Session["Modelid"].ToString() + "";
                    DataTable dtlabel = objDa.return_DataTable(sql);
                    for (int i = 0; i < dtlabel.Rows.Count; i++)
                    {
                        ListItem Li = new ListItem();
                        if (dtlabel.Rows[i]["Field1"].ToString().Trim() != "")
                        {
                            Li.Text = dtlabel.Rows[i]["Field3"].ToString().Trim() + dtlabel.Rows[i]["Width"].ToString() + "X" + dtlabel.Rows[i]["Height"].ToString() + " " + "(mm) - " + dtlabel.Rows[i]["gap"].ToString() + " (gap) - " + dtlabel.Rows[i]["Field1"].ToString().Trim();
                        }
                        else
                        {
                            Li.Text = dtlabel.Rows[i]["Field3"].ToString().Trim() + dtlabel.Rows[i]["Width"].ToString() + "X" + dtlabel.Rows[i]["Height"].ToString() + " " + "(mm) - " + dtlabel.Rows[i]["gap"].ToString() + " (gap)";

                        }



                        Li.Value = dtlabel.Rows[i]["Trans_Id"].ToString();
                        if (hdnDetailEdit.Value != "")
                        {
                            strsql = "select * from dbo.Inv_Label_Filter where Child_Id=" + hdnDetailEdit.Value + " and Parent_Id=" + dtlabel.Rows[i]["Trans_Id"].ToString() + "";
                            if (objDa.return_DataTable(strsql).Rows.Count > 0)
                            {
                                Li.Selected = true;
                            }
                        }

                        chkSelectedItems.Items.Add(Li);
                    }
                    //code end
                    lblFiltertext.Text = Resources.Attendance.Label_Size;
                }

                //if slected option category is quantity
                if (dt.Rows[0]["OptionCategoryID"].ToString() == dtDetail.Rows[0]["QtyCategoryId"].ToString())
                {
                    DataTable dtbom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), Session["Modelid"].ToString());
                    try
                    {
                        dtbom = new DataView(dtbom, "OptionCategoryID=" + dtDetail.Rows[0]["CoreCategoryId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    foreach (DataRow dr in dtbom.Rows)
                    {
                        ListItem Li = new ListItem();
                        Li.Text = dr["ShortDescription"].ToString();
                        Li.Value = dr["TransID"].ToString();

                        if (hdnDetailEdit.Value != "")
                        {
                            strsql = "select * from dbo.Inv_Label_Filter where Child_Id=" + hdnDetailEdit.Value + " and Parent_Id=" + dr["TransID"].ToString() + "";
                            if (objDa.return_DataTable(strsql).Rows.Count > 0)
                            {
                                Li.Selected = true;
                            }
                        }

                        chkSelectedItems.Items.Add(Li);
                    }
                    lblFiltertext.Text = "Core";

                }
                //if slected option category is packing list

                if (dt.Rows[0]["OptionCategoryID"].ToString() == dtDetail.Rows[0]["PackingCategoryId"].ToString())
                {
                    DataTable dtbom = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), Session["Modelid"].ToString());
                    try
                    {
                        dtbom = new DataView(dtbom, "OptionCategoryID=" + dtDetail.Rows[0]["QtyCategoryId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    foreach (DataRow dr in dtbom.Rows)
                    {
                        ListItem Li = new ListItem();
                        Li.Text = dr["ShortDescription"].ToString();
                        Li.Value = dr["TransID"].ToString();
                        if (hdnDetailEdit.Value != "")
                        {
                            strsql = "select * from dbo.Inv_Label_Filter where Child_Id=" + hdnDetailEdit.Value + " and Parent_Id=" + dr["TransID"].ToString() + "";
                            if (objDa.return_DataTable(strsql).Rows.Count > 0)
                            {
                                Li.Selected = true;
                            }
                        }

                        chkSelectedItems.Items.Add(Li);
                    }
                    lblFiltertext.Text = "Label Quantity";
                }
            }
        }
    }
    protected void txtModelName_TextChanged(object sender, EventArgs e)
    {
        if (txtModelName.Text != "")
        {

            DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True"), "Model_Name='" + txtModelName.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
                Session["Modelid"] = dt.Rows[0]["Trans_Id"].ToString();
                lblUnitPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Unit_Price, Session["DBConnection"].ToString());
                ViewState["CurrencyId"] = dt.Rows[0]["Field4"].ToString();

                if (ViewState["CurrencyId"].ToString() == Session["CurrencyId"].ToString())
                {
                    ViewState["ExchangeRate"] = "1";
                }
                else
                {
                    ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(ViewState["CurrencyId"].ToString(), Session["CurrencyId"].ToString(), Session["DBConnection"].ToString());
                }

                fillGrid();

                try
                {
                    gvProductSpecsChild.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Unit_Price, Session["DBConnection"].ToString());
                }
                catch
                {
                }
                pnlChlidGrid.Visible = true;
                rdoOption.Focus();
            }
            else
            {
                txtModelNo.Text = "";
                DisplayMessage("Select Model Name");
                txtModelNo.Focus();

            }
        }


    }
    //
    #endregion

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        BtnPrint.Visible = clsPagePermission.bPrint;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
    }
    #endregion

    #region Auto Complete Method/Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelName(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_Name like '%" + prefixText.ToString() + "%'", "Model_Name Asc", DataViewRowState.CurrentRows).ToTable();


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_Name"].ToString();
        }


        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_No like '%" + prefixText.ToString() + "%'", "Model_No Asc", DataViewRowState.CurrentRows).ToTable();


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }


        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSubProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();

            }
        }

        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_OptionCategoryMaster ObjOptCat = new Inv_OptionCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjOptCat.GetOptionCategoryTrueAll(HttpContext.Current.Session["CompId"].ToString());
        DataTable dtOp = dt.Copy();
        dt = new DataView(dt, "EName Like '%" + prefixText.ToString() + "%'", "Ename Asc", DataViewRowState.CurrentRows).ToTable();

        string[] text = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            text[i] = dt.Rows[i]["EName"].ToString();

        }
        return text;

    }
    #endregion

    #region User Defined Function
    public string GetItemType(string IT)
    {
        string retval = string.Empty;
        if (IT == "A")
        {
            retval = "Assemble  (Search KeyWord as A)";
        }
        if (IT == "K")
        {
            retval = "KIT  (Search KeyWord as K)";
        }

        return retval;

    }

    public void Reset_Child()
    {
        rdoStock.Checked = false;
        rdoOption.Checked = false;
        pnlOptStock.Visible = false;
        txtSubProduct.Text = "";
        txtUnitPrice.Text = "";
        txtQty.Text = "";
        txtShortDesc.Text = "";
        txtOptionDesc.Text = "";
        txtOption.Text = "";
        txtOptCatId.Text = "";
        hdnDetailEdit.Value = "";
        chkDefault.Checked = false;
        lblFiltertext.Text = "";
    }
    public void fillGrid()
    {
        DataTable dtModel = new DataView(ObjInvModelMaster.GetModelMasterByModelNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), "True", txtModelNo.Text.Trim()), "", "", DataViewRowState.CurrentRows).ToTable();
        string ModelId = dtModel.Rows[0]["Trans_Id"].ToString();
        DataTable dt = ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString().ToString(), ModelId);
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductSpecsChild, dt, "", "");

        try
        {
            gvProductSpecsChild.HeaderRow.Cells[4].Text = SystemParameter.GetCurrencySmbol(dtModel.Rows[0]["Field4"].ToString(), Resources.Attendance.Unit_Price, HttpContext.Current.Session["DBConnection"].ToString());
        }
        catch
        {
        }
        //AllPageCode();
    }
    private void FillModelGrid()
    {
        DataTable dtModel = ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), "True");
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        gvModelMaster.DataSource = dtModel;
        gvModelMaster.DataBind();
        Session["dtFilter_BOM"] = dtModel;
        Session["dtModel"] = dtModel;
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

    public string getProductName(string ProductId)
    {
        string ProductName = string.Empty;
        try
        {
            if (ProductId != "0" && ProductId != "")
            {
                ProductName = new DataView(ObjInvProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0"), "ProductId='" + ProductId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["EProductName"].ToString();
            }
            else
            {
                ProductName = "0";
            }    
        }
        catch
        {
            ProductName = "0";
        }

        return ProductName;
    }
    public string SetDecimal(string Amount)
    {
        if (ViewState["CurrencyId"] == null)
        {
            ViewState["CurrencyId"] = "0";
        }
        return ObjSysPeram.GetCurencyConversionForInv(ViewState["CurrencyId"].ToString(), Amount).ToString();
    }
    #endregion
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/BillOfMaterial_Report.aspx?ModelId=" + e.CommandArgument.ToString() + "','window','width=1024');", true);

    }
    protected void btnPrintReport_Click(object sender, EventArgs e)
    {


        string ModelId = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), "True"), "Model_No='" + txtModelNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/BillOfMaterial_Report.aspx?ModelId=" + ModelId.ToString() + "','window','width=1024');", true);

    }
    #endregion
    public void FillFilterOption()
    {
        //hdnDetailEdit.Value

    }
    //get unit price for inventory item

    protected void txtSubProduct_OnTextChanged(object sender, EventArgs e)
    {
        string SubProductId = string.Empty;
        if (txtSubProduct.Text != "")
        {
            try
            {
                txtUnitPrice.Text = new DataView(ObjInvProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "EProductName='" + txtSubProduct.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["SalesPrice1"].ToString();

            }
            catch
            {
                txtUnitPrice.Text = "0";
            }
        }

    }

    //#region pageindexchangeevent
    //protected void gvModelMaster_OnPageIndexChanged(object sender, EventArgs e)
    //    {
    //        int pageIndex = (sender as ASPxGridView).PageIndex;

    //        gvModelMaster.PageIndex = pageIndex;
    //        FillModelGrid();
    //    }
    //#endregion
}
