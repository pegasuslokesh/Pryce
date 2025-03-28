using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.IO;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Configuration;

public partial class Inventory_ProductCategoryMaster : BasePage
{
    #region defined Class Object
    DataAccessClass daClass = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_ProductCategoryDetail ObjProductCategoryDetail = null;
    SystemParameter ObjSysPeram = null;
    Common cmn = null;
    ArrayList arr = new ArrayList();
    TaxMaster objTaxMaster = null;
    Inv_ProductCategory_Tax objprotax = null;
    PageControlCommon objPageCmn = null;
    string SortExpression = "";
    Set_ApplicationParameter objAppParam = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        daClass = new DataAccessClass(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjProductCategoryDetail = new Inv_ProductCategoryDetail(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        objprotax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ProductCategoryMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            ddlOption.SelectedIndex = 2;
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            {
                btnupload.Visible = false;
            }
            FillProductCategory();
            //FillGridBin();
            FillGrid();
            Session["Image"] = null;
            ViewState["dtTax"] = null;
           
            try
            {
                RootFolder();
                string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }
            catch(Exception ex)
            {

            }
            // LoadStores();
        }
        //AllPageCode();
        //    BindTreeView();
    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        btnRestoreSelected.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnExportExcel.Visible = clsPagePermission.bDownload;
    }
    #endregion

    #region System defined Function

    protected void btnNew_Click(object sender, EventArgs e)
    {
        FillProductCategory();
        txtCategoryName.Focus();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Category_Id"]))
                {
                    lblSelectedRecord.Text += dr["Category_Id"] + ",";
                }
            }
            for (int i = 0; i < gvProductCategoryBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvProductCategoryBin.Rows[i].FindControl("lblCategoryId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvProductCategoryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtProductCategory1 = (DataTable)Session["dtInactive"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductCategoryBin, dtProductCategory1, "", "");

            //AllPageCode();
            ViewState["Select"] = null;
        }
        ImgbtnSelectAll.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        if (txtCategoryName.Text == "")
        {
            DisplayMessage("Enter Category Name");
            txtCategoryName.Focus();
            btnSave.Enabled = true;
            return;
        }

        if (Session["Image"] == null)
        {
            Session["Image"] = "";
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {








            int b = 0;
            if (editid.Value == "")
            {
                string Ref_Id = string.Empty;
                b = ObjProductCateMaster.InsertProductCategoryMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtCategoryName.Text, txtLCategoryName.Text, "", txtDescription.Text, Session["Image"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    Ref_Id = b.ToString();
                    foreach (ListItem li in ChkProductCategory.Items)
                    {
                        if (li.Selected)
                        {
                            ObjProductCategoryDetail.ProductCategoryDetail_Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), Ref_Id.Trim(), li.Value.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //for save in tax table

                    if (ViewState["dtTax"] != null)
                    {
                        DataTable dtTax = (DataTable)ViewState["dtTax"];

                        foreach (DataRow dr in dtTax.Rows)
                        {
                            objprotax.InsertRecord(Ref_Id.Trim(), dr["Tax_Id"].ToString(), dr["Tax_Value"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }


                    DisplayMessage("Record Saved", "green");
                    txtCategoryName.Focus();
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }

            }
            else
            {

                b = ObjProductCateMaster.UpdateProductCategoryMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), editid.Value.ToString(), txtCategoryName.Text, txtLCategoryName.Text, "", txtDescription.Text, Session["Image"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {

                    ObjProductCategoryDetail.ProductCategoryDetail_Delete(Session["CompId"].ToString(), Session["BrandId"].ToString(), editid.Value.Trim(), ref trns);
                    foreach (ListItem li in ChkProductCategory.Items)
                    {
                        if (li.Selected)
                        {
                            ObjProductCategoryDetail.ProductCategoryDetail_Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), editid.Value.Trim(), li.Value.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }


                    //for save in tax table

                    objprotax.DeleteRecord_ByProductcategoryId(editid.Value);

                    if (ViewState["dtTax"] != null)
                    {
                        DataTable dtTax = (DataTable)ViewState["dtTax"];

                        foreach (DataRow dr in dtTax.Rows)
                        {
                            objprotax.InsertRecord(editid.Value, dr["Tax_Id"].ToString(), dr["Tax_Value"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    DisplayMessage("Record Updated", "green");
                    //btnList_Click(null, null);
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Updated");
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
            FillGrid();
            Reset(1);
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
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = ObjProductCateMaster.GetProductCategoryByCategoryId(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count != 0)
        {

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

            GetCategoryChild(editid.Value);

          
                txtCategoryName.Text = dt.Rows[0]["Category_Name"].ToString();
                txtLCategoryName.Text = dt.Rows[0]["Category_Name_L"].ToString();
            try
            {
                string RegistrationCode = Common.Decrypt(daClass.get_SingleValue("Select registration_code from Application_Lic_Main"));
           
                imgProduct.ImageUrl = "~/CompanyResource/" + RegistrationCode + "/" + Session["CompId"].ToString() + "/ProductCategory/" + dt.Rows[0]["Field1"].ToString().Trim();
                Session["Image"] = dt.Rows[0]["Field1"].ToString();
            }
            catch(Exception ex)
            {
                imgProduct.ImageUrl = "~/CompanyResource/"+ Session["CompId"].ToString() + "/ProductCategory/" + dt.Rows[0]["Field1"].ToString().Trim();
                Session["Image"] = dt.Rows[0]["Field1"].ToString();

            }
            txtDescription.Text = dt.Rows[0]["Description"].ToString();

            DataTable dtProtax = objprotax.GetRecord_ByProductcategoryId(editid.Value);

            try
            {
                dtProtax = dtProtax.DefaultView.ToTable(true, "Tax_Id", "Tax_Name", "Tax_Value");
            }
            catch
            {
            }
            ViewState["dtTax"] = dtProtax;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductCategoryTax, dtProtax, "", "");
            //AllPageCode();

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {

        btnDeleteChild.Visible = true;
        btnBack.Visible = false;
        btnMoveChild.Visible = true;
        pnlMoveChild.Visible = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset(0);
        txtCategoryName.Focus();
    }


    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset(1);
        FillGrid();
        txtValue.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }



    protected void gvCategoryProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCategoryProduct.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvCategoryProduct, dt, "", "");

        //AllPageCode();
        gvCategoryProduct.BottomPagerRow.Focus();
    }



    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOption.SelectedIndex != 0)
        {

            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
            }


            TreeViewCategory.Visible = false;
            gvCategoryProduct.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View; //Show grid if tree view is current shown

            DataTable dtCust = (DataTable)Session["Category"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvCategoryProduct, view.ToTable(), "", "");

            //AllPageCode();            
        }
        txtValue.Focus();
    }

    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        TreeViewCategory.Visible = false;
        gvCategoryProduct.Visible = true;
        btnGridView.ToolTip = Resources.Attendance.Tree_View;
        BindTreeView();
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 1;
        txtValue.Text = "";
        txtValue.Focus();
    }


    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewCategory.Visible == true)
        {
            TreeViewCategory.Visible = false;
            gvCategoryProduct.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvCategoryProduct.Visible = false;
            TreeViewCategory.Visible = true;
            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }

    protected void TreeViewCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewCategory.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }


    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewCategory.Visible == true)
        {
            TreeViewCategory.Visible = false;
            gvCategoryProduct.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvCategoryProduct.Visible = false;
            TreeViewCategory.Visible = true;

        }
        btnTreeView.Focus();
    }

    //Delete  Section
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtChildCategory = ObjProductCategoryDetail.GetProductCategoryDetailByParentId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dtChildCategory.Rows.Count == 0)
        {
            ObjProductCateMaster.DeleteProductCategoryMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());
            ObjProductCategoryDetail.ProductCategoryDetail_Delete(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());

            DisplayMessage("Record Delete");
            FillGrid();
            FillGridBin();

        }
        else
        {
            //panelOverlay.Visible = true;
            //panelPopUpPanel.Visible = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Category_Modal_Popup()", true);
            editid.Value = e.CommandArgument.ToString();
            btnBack.Visible = false;
            btnDeleteChild.Visible = true;
            pnlMoveChild.Visible = false;
            btnMoveChild.Visible = true;

        }

    }
    protected void btnDeleteChild_Click(object sender, EventArgs e)
    {
        ObjProductCategoryDetail.ProductCategoryDetail_DeleteByParentId(Session["CompId"].ToString(), Session["BrandId"].ToString(), editid.Value);

        ObjProductCateMaster.DeleteProductCategoryMaster(Session["CompId"].ToString(), editid.Value.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Delete");
        FillGrid();
        FillGridBin();
        //panelOverlay.Visible = false;
        //panelPopUpPanel.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Category_Modal_Popup()", true);
        editid.Value = "";

    }
    protected void btnMoveChild_Click(object sender, EventArgs e)
    {
        btnDeleteChild.Visible = false;
        btnBack.Visible = true;
        btnMoveChild.Visible = false;
        pnlMoveChild.Visible = true;
        FillddlProductCategory();
    }
    protected void btnUpdateParent_Click(object sender, EventArgs e)
    {
        if (ddlMoveCategory.SelectedItem.Text != "No Category Available Here")
        {
            ObjProductCateMaster.DeleteProductCategoryMaster(Session["CompId"].ToString(), editid.Value.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());

            ObjProductCategoryDetail.ProductCategoryDetail_Update(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlMoveCategory.SelectedValue, editid.Value.Trim());
            DisplayMessage("Record Delete and Move Child");
            //panelOverlay.Visible = false;
            //panelPopUpPanel.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Category_Modal_Popup()", true);
            FillGrid();
            editid.Value = "";
        }
    }


    protected void gvCategoryProductBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductCategoryBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductCategoryBin, dt, "", "");

        //AllPageCode();
        string temp = string.Empty;

        for (int i = 0; i < gvProductCategoryBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvProductCategoryBin.Rows[i].FindControl("lblCategoryId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvProductCategoryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        gvProductCategoryBin.BottomPagerRow.Focus();
    }
    protected void gvProductCategoryBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        SortExpression = e.SortExpression;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtInactive"];
        DataView dv = new DataView(dt);
        string Query = "" + SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductCategoryBin, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();
        gvProductCategoryBin.HeaderRow.Focus();
    }
    protected void gvProductCategory_OnSorting(object sender, GridViewSortEventArgs e)
    {
        SortExpression = e.SortExpression;
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvCategoryProduct, dt, "", "");

        //AllPageCode();
        gvCategoryProduct.HeaderRow.Focus();

    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        BindTreeView();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueBin.Focus();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {


            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text + "%'";
            }


            DataTable dtCust = (DataTable)Session["CategoryBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductCategoryBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                ImgbtnSelectAll.Visible = false;
                btnRestoreSelected.Visible = false;
            }
            else
            {
                //AllPageCode();
            }

        }
        txtValueBin.Focus();
    }
    protected void txtCategoryName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = ObjProductCateMaster.GetProductCategoryByCategoryName(Session["CompId"].ToString(), txtCategoryName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtCategoryName.Text = "";
                DisplayMessage("Category Name Already Exists");
                txtCategoryName.Focus();

            }
            else
            {
                DataTable dt1 = ObjProductCateMaster.GetProductCategoryFalseAllData(Session["CompId"].ToString());
                dt1 = new DataView(dt1, "Category_Name='" + txtCategoryName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    txtCategoryName.Text = "";
                    DisplayMessage("Category Name Already Exists - Go to Bin Tab");
                    txtCategoryName.Focus();

                }
            }
        }
        else
        {
            DataTable dtTemp = ObjProductCateMaster.GetProductCategoryByCategoryId(Session["CompId"].ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Category_Name"].ToString() != txtCategoryName.Text.Trim())
                {
                    DataTable dt = ObjProductCateMaster.GetProductCategoryByCategoryName(Session["CompId"].ToString(), txtCategoryName.Text.Trim());
                    if (dt.Rows.Count > 0)
                    {
                        txtCategoryName.Text = "";
                        DisplayMessage("Category Name Already Exists");
                        txtCategoryName.Focus();
                    }
                    else
                    {
                        DataTable dt1 = ObjProductCateMaster.GetProductCategoryFalseAllData(Session["CompId"].ToString());
                        dt1 = new DataView(dt1, "Category_Name='" + txtCategoryName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            txtCategoryName.Text = "";
                            DisplayMessage("Category Name Already Exists - Go to Bin Tab");
                            txtCategoryName.Focus();

                        }
                    }
                }
            }
        }
        txtLCategoryName.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        DataTable dt = ObjProductCateMaster.GetProductCategoryFalseAllData(Session["CompId"].ToString());
        if (gvProductCategoryBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        Msg = ObjProductCateMaster.DeleteProductCategoryMaster(Session["CompId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                }
            }

            if (Msg != 0)
            {
                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
                btnRefreshBin_Click(null, null);

            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in gvProductCategoryBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                    if (chk.Checked)
                    {
                        fleg = 1;
                    }
                    else
                    {
                        fleg = 0;
                    }
                }
                if (fleg == 0)
                {
                    DisplayMessage("Please Select Record");
                }
                else
                {
                    DisplayMessage("Record Not Activated");
                }
            }
        }
        txtValueBin.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvProductCategoryBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvProductCategoryBin.Rows.Count; i++)
        {
            ((CheckBox)gvProductCategoryBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvProductCategoryBin.Rows[i].FindControl("lblCategoryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvProductCategoryBin.Rows[i].FindControl("lblCategoryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvProductCategoryBin.Rows[i].FindControl("lblCategoryId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
        chkSelAll.Focus();
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvProductCategoryBin.Rows[index].FindControl("lblCategoryId");
        if (((CheckBox)gvProductCategoryBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;

        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
        ((CheckBox)gvProductCategoryBin.Rows[index].FindControl("chkSelect")).Focus();
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        Reset(1);
        //panelOverlay.Visible = false;
        //panelPopUpPanel.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Category_Modal_Popup()", true);
        txtValue.Focus();
    }
    #endregion

    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_ProductCategoryMaster ObjProductCateMaster = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjProductCateMaster.GetDistinctCategoryName(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Category_Name"].ToString() + "";
        }
        return txt;
    }
    #endregion

    #region User defined Function
    public void FillGrid()
    {
        DataTable dt = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "", "Category_Id desc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvCategoryProduct, dt, "", "");

        }
        else
        {
            gvCategoryProduct.DataSource = null;
            gvCategoryProduct.DataBind();
        }

        Session["Category"] = dt;
        ViewState["dtFilter"] = dt;
        //AllPageCode();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
    }

    public void Reset(int RC)
    {
        txtCategoryName.Text = "";
        txtLCategoryName.Text = "";
        txtDescription.Text = "";

        Lbl_Tab_New.Text = Resources.Attendance.New;
        editid.Value = "";
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        ddlFieldNameBin.SelectedIndex = 1;
        ddlOptionBin.SelectedIndex = 2;
        txtValueBin.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        Session["Image"] = null;
        imgProduct.ImageUrl = null;
        if (RC == 1)
        {

            txtValue.Focus();
        }

        else
        {
            txtCategoryName.Focus();
        }

        Session["DeleteNodeValue"] = "";
        arr.Clear();
        TreeViewCategory.Visible = false;
        gvCategoryProduct.Visible = true;
        FillGrid();
        FillProductCategory();
        btnGridView.ToolTip = Resources.Attendance.Tree_View;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["dtTax"] = null;
        gvProductCategoryTax.DataSource = null;
        gvProductCategoryTax.DataBind();
        txtTaxname.Text = "";
        txtTaxValue.Text = "";
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

    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = ObjProductCateMaster.GetProductCategoryFalseAllData(Session["CompId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductCategoryBin, dt, "", "");

        Session["CategoryBin"] = dt;
        Session["dtInactive"] = dt;
        if (dt != null)
        {
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        }
        else
        {
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + "0";
        }
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            ImgbtnSelectAll.Visible = false;
            btnRestoreSelected.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }

    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString().ToString());

        if (editid.Value != "")
        {
            dsCategory = new DataView(dsCategory, "Category_Id<>'" + editid.Value + "'", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();
        }

        if (dsCategory.Rows.Count > 0)
        {
            ChkProductCategory.Items.Clear();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ChkProductCategory, dsCategory, "Category_Name", "Category_Id");
        }
        else
        {
            ChkProductCategory.Items.Clear();
            ChkProductCategory.DataSource = null;
            ChkProductCategory.DataBind();
        }
    }
    private void FillddlProductCategory()
    {
        DataTable dsCategory = new DataTable();

        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        DataTable dtParent = dsCategory.Clone();
        if (dsCategory.Rows.Count != 0)
        {
            DataTable dtParnt = ObjProductCategoryDetail.GetProductCategoryDetailByParentId(Session["CompId"].ToString(), editid.Value.ToString());
            foreach (DataRow drChild in dtParnt.Rows)
            {

                dsCategory = new DataView(dsCategory, "Category_Id<>'" + drChild["Ref_Id"].ToString() + "'", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();
            }
            foreach (DataRow dr in dsCategory.Rows)
            {
                if (ObjProductCategoryDetail.GetProductCategoryDetailByRefId(Session["CompId"].ToString(), Session["BrandId"].ToString(), dr["Category_Id"].ToString()).Rows.Count == 0)
                {

                    if (dr["Category_Id"].ToString() != editid.Value.ToString())
                    {
                        dtParent.ImportRow(dr);

                    }
                }



            }
        }



        if (dtParent.Rows.Count > 0)
        {

            ddlMoveCategory.Items.Clear();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015


            objPageCmn.FillData((object)ddlMoveCategory, dtParent, "Category_Name", "Category_Id");


        }
        else
        {
            ddlMoveCategory.Items.Add("No Category Available Here");
        }
    }
    public void GetCategoryChild(string Ref_id)
    {
        DataTable dtCategoryChild = null;
        dtCategoryChild = ObjProductCategoryDetail.GetProductCategoryDetailByRefId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Ref_id);
        if (dtCategoryChild.Rows.Count != 0)
        {
            foreach (ListItem li in ChkProductCategory.Items)
            {
                if ((new DataView(dtCategoryChild, "Parent_Id='" + li.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) != 0)
                {

                    li.Selected = true;
                }
            }
        }

    }
    #endregion

    #region Tax
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTax(string prefixText, int count, string contextKey)
    {
        TaxMaster objtaxMaster = new TaxMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objtaxMaster.GetTaxMaster_ByTaxName(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Tax_Name"].ToString();
        }
        return txt;
    }


    protected void IbtnAddProductcategoryTax_Click(object sender, ImageClickEventArgs e)
    {
        if (txtTaxname.Text == "")
        {
            DisplayMessage("Select Tax");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxname);
            return;

        }
        if (txtTaxValue.Text == "")
        {
            DisplayMessage("Enter Tax value");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxValue);
            return;
        }

        DataTable dt = new DataTable();

        if (ViewState["dtTax"] == null)
        {
            dt.Columns.Add("Tax_Id", typeof(int));
            dt.Columns.Add("Tax_Name", typeof(string));
            dt.Columns.Add("Tax_Value", typeof(float));
            DataRow dr = dt.NewRow();
            dr[0] = hdhtaxId.Value;
            dr[1] = txtTaxname.Text;
            dr[2] = txtTaxValue.Text;
            dt.Rows.Add(dr);
        }
        else
        {


            dt = (DataTable)ViewState["dtTax"];

            DataRow dr = dt.NewRow();
            dr[0] = hdhtaxId.Value;
            dr[1] = txtTaxname.Text;
            dr[2] = txtTaxValue.Text;
            dt.Rows.Add(dr);
        }

        ViewState["dtTax"] = dt;

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductCategoryTax, dt, "", "");


        txtTaxname.Text = "";
        txtTaxValue.Text = "";
        txtTaxname.Focus();

    }

    protected void txtTaxName_TextChanged(object sender, EventArgs e)
    {
        if (txtTaxname.Text.Trim() != "")
        {
            DataTable dt = objTaxMaster.GetTaxMasterTrueAll();
            dt = new DataView(dt, "Tax_Name='" + txtTaxname.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count == 0)
            {

                DisplayMessage("Choose Tax In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxname);
                txtTaxname.Text = "";
                hdhtaxId.Value = "0";
                //return;
            }
            else
            {

                hdhtaxId.Value = dt.Rows[0]["Trans_Id"].ToString();
                txtTaxValue.Focus();
                if (ViewState["dtTax"] != null)
                {


                    DataTable dtCheckExistTax = new DataTable();
                    dtCheckExistTax = (DataTable)ViewState["dtTax"];
                    try
                    {
                        dtCheckExistTax = new DataView(dtCheckExistTax, "Tax_Id=" + hdhtaxId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtCheckExistTax.Rows.Count > 0)
                    {
                        DisplayMessage("Tax is Already Exists");
                        txtTaxname.Text = "";
                        txtTaxname.Focus();
                        hdhtaxId.Value = "0";
                        //return;
                    }
                }



            }
        }
    }




    protected void IbtnDeleteTax_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtTax"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                var rows = dt.Select("Tax_Id ='" + e.CommandArgument.ToString() + "'");
                foreach (var row in rows)
                    row.Delete();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvProductCategoryTax, dt, "", "");

                ViewState["dtTax"] = dt;
            }
        }

    }

    protected void gvProductCategoryTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductCategoryTax.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtTax"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductCategoryTax, dt, "", "");

    }









    //protected void txtTaxFooter_TextChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

    //    DataTable dt = objTaxMaster.GetTaxMasterTrueAll();
    //    dt = new DataView(dt, "Tax_Name='" + ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();

    //    if (dt.Rows.Count == 0)
    //    {

    //        DisplayMessage("Choose Tax In Suggestion Only");
    //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")));
    //        ((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text = "";
    //        ((HiddenField)gvrow.FindControl("hdnTaxId")).Value = "";
    //        return;
    //    }
    //    else
    //    {
    //        ((HiddenField)gvrow.FindControl("hdnTaxId")).Value = dt.Rows[0]["Trans_Id"].ToString();
    //    }


    //}
    //public void LoadStores()
    //{
    //    DataTable dt = new DataTable();
    //    if (ViewState["dtTax"] != null)
    //    {
    //         dt = new DataTable();

    //        dt = (DataTable)ViewState["dtTax"];

    //        gridView.DataSource = dt;
    //        gridView.DataBind();

    //    }
    //    else
    //    {
    //        gridView.DataSource = dt;
    //        gridView.DataBind();
    //    }
    //}


    //protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    gridView.EditIndex = e.NewEditIndex;
    //    LoadStores();
    //}




    //protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    if(((TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxName")).Text=="")
    //    {
    //        DisplayMessage("Enter Tax name");
    //        return;
    //    }

    //    if (((TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxValue")).Text == "")
    //    {
    //        DisplayMessage("Enter Tax value");
    //        return;
    //    }




    //    DataTable dt = new DataTable();
    //    if (ViewState["dtTax"] != null)
    //    {

    //      dt = (DataTable)ViewState["dtTax"];

    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            if (dt.Rows[i]["Tax_Id"].ToString() == ((HiddenField)gridView.Rows[e.RowIndex].FindControl("hdnTaxId")).Value)
    //            {
    //                dt.Rows[i]["Tax_Name"] = ((TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxName")).Text;
    //                dt.Rows[i]["Tax_Value"] = ((TextBox)gridView.Rows[e.RowIndex].FindControl("txtTaxValue")).Text;
    //                break;
    //            }
    //        }

    //    }
    //    ViewState["dtTax"] = dt;

    //DisplayMessage("Updated Successfully");
    // LoadStores();
    //    gridView.EditIndex = -1;

    //}
    //protected void gridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{

    //    gridView.EditIndex = -1;
    //    LoadStores();

    //}
    //protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    DataTable dt = new DataTable();
    //    if (ViewState["dtTax"] != null)
    //    {

    //        dt = (DataTable)ViewState["dtTax"];

    //        dt = new DataView(dt, "Tax_Id<>" + ((HiddenField)gridView.Rows[e.RowIndex].FindControl("hdnTaxId")).Value + "", "", DataViewRowState.CurrentRows).ToTable();
    //        ViewState["dtTax"] = dt;
    //        LoadStores();
    //    }


    //}
    //protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
    //{

    //       DataTable dt = new DataTable();
    //    string TaxId = "";
    //    if (e.CommandName.Equals("AddNew"))
    //    {

    //        if (((TextBox)gridView.FooterRow.FindControl("txtTaxFooter")).Text == "")
    //        {
    //            DisplayMessage("Enter Tax name");
    //            return;
    //        }

    //        if (((TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter")).Text == "")
    //        {
    //            DisplayMessage("Enter Tax value");
    //            return;
    //        }

    //        TextBox txtTaxName = (TextBox)gridView.FooterRow.FindControl("txtTaxName");
    //        TextBox txtTaxValue = (TextBox)gridView.FooterRow.FindControl("txtTaxValueFooter");



    //         DataTable dtTax = objTaxMaster.GetTaxMasterTrueAll();
    //         dtTax = new DataView(dtTax, "Tax_Name='" + txtTaxName + "'", "", DataViewRowState.CurrentRows).ToTable();

    //         if (dtTax.Rows.Count> 0)
    //    {

    //        TaxId = dtTax.Rows[0]["Trans_Id"].ToString();

    //    }


    //     if(ViewState["dtTax"]==null)
    //        {
    //        dt.Columns.Add("Tax_Id", typeof(int));
    //        dt.Columns.Add("Tax_Name", typeof(string));
    //        dt.Columns.Add("Tax_Value", typeof(float));
    //        DataRow dr = dt.NewRow();
    //        dr[0] = TaxId;
    //        dr[1] = txtTaxName.Text;
    //        dr[2] = txtTaxValue.Text;
    //        dt.Rows.Add(dr);
    //        }
    //        else
    //        {
    //            dt = (DataTable)ViewState["dtTax"];
    //            DataRow dr = dt.NewRow();
    //            dr[0] = TaxId;
    //            dr[1] = txtTaxName.Text;
    //            dr[2] = txtTaxValue.Text;
    //            dt.Rows.Add(dr);
    //        }

    //        ViewState["dtTax"] = dt;
    //    }



    //}


    #endregion


    #region Filetobytearray
    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }
    #endregion
    #region Upload
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)
    {
        try
        {
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            CompanyMaster objComp = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string compid = (int.Parse(objComp.GetMaxCompanyId())).ToString();           
            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                fullname = fullname.Replace("Product_", "Product//Product_");
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath ="~/CompanyResource/"+RegistrationCode+"/"+HttpContext.Current.Session["CompId"].ToString()+"/ProductCategory";
                string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                if (Directory.Exists(MapfullPath))
                {
                    if (RegistrationCode != "" && RegistrationCode != null)
                    {
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(MapfullPath);
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch(Exception ex)
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/"+ HttpContext.Current.Session["CompId"].ToString() + "/ProductCategory";
                string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                if (Directory.Exists(MapfullPath))
                {                    
                  File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);                   
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(MapfullPath);
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    catch (Exception exp)
                    {

                    }
                }
            }
            HttpContext.Current.Session["Image"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
        }
    }
    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            try
            {
                string RegistrationCode = Common.Decrypt(daClass.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch
            {
                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }
        }

    }
    #endregion
    #endregion



    #region UploadImage

    protected void btnloadimg_Click(object sender, EventArgs e)
    {
        if (fugProduct.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/"));
            }
            imgProduct.ImageUrl = null;

            fugProduct.SaveAs(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/" + fugProduct.FileName));
            imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/" + fugProduct.FileName;

            Session["Image"] = fugProduct.FileName;
        }
    }

    #region filemanager
    protected void btnClosePanel1_Click(object sender, EventArgs e)
    {
        //pnlAddress1.Visible = false;
        //pnlAddress2.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Modal_Popup()", true);
    }

    protected void btnupload_OnClick(object sender, EventArgs e)
    {
        //pnlAddress1.Visible = true;
        //pnlAddress2.Visible = true;
        //string strCmd = string.Format("window.open('../Production/FileManager.aspx','window','width=1024, ');");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
    }

    protected void btnRemove_OnClick(object sender, EventArgs e)
    {
        if (Session["Image"] != null)
        {
            Session["Image"] = "";
        }


        try
        {

            imgProduct.ImageUrl = "";
        }
        catch
        {

        }

    }


    #endregion
    #endregion

    #region TreeViewConcept
    private void BindTreeView()
    {
        TreeViewCategory.Nodes.Clear();
        DataTable dt = new DataTable();

        dt = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        DataTable dtParent = dt.Clone();
        if (dt.Rows.Count != 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (ObjProductCategoryDetail.GetProductCategoryDetailByRefId(Session["CompId"].ToString(), Session["BrandId"].ToString(), dr["Category_Id"].ToString()).Rows.Count == 0)
                {
                    dtParent.ImportRow(dr);
                }
            }

        }

        int i = 0;
        while (i < dtParent.Rows.Count)
        {

            TreeNode tn = new TreeNode();
            tn.Text = dtParent.Rows[i]["Category_Name"].ToString();
            tn.Value = dtParent.Rows[i]["Category_Id"].ToString();
            TreeViewCategory.Nodes.Add(tn);
            FillChild((dtParent.Rows[i]["Category_Id"].ToString()), tn);
            i++;
        }
        TreeViewCategory.DataBind();
        TreeViewCategory.CollapseAll();
    }
    private void FillChild(string index, TreeNode tn)
    {
        DataTable dt = new DataTable();
        dt = ObjProductCategoryDetail.GetProductCategoryDetailByParentId(Session["CompId"].ToString(), index);

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Category_Name"].ToString();
            tn1.Value = dt.Rows[i]["Category_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Category_Id"].ToString()), tn1);
            i++;
        }
        TreeViewCategory.DataBind();
    }
    #endregion

    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (fugProduct.HasFile)
        {
            string ext = fugProduct.FileName.Substring(fugProduct.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/"));
                }
                imgProduct.ImageUrl = null;

                fugProduct.SaveAs(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/" + fugProduct.FileName));
                imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/ProductCategory/" + fugProduct.FileName;

                Session["Image"] = fugProduct.FileName;
            }
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["dtFilter"] != null)
        {
            DataTable dt = ViewState["dtFilter"] as DataTable;
            if (dt.Rows.Count > 0)
            {

                dt.Columns["Category_Id"].SetOrdinal(0);
                dt.Columns["Category_Name"].SetOrdinal(1);
                dt.Columns["Created_User"].SetOrdinal(2);
                dt.Columns["Modified_User"].SetOrdinal(3);

                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);
                dt.Columns.RemoveAt(4);

                dt.Columns["Category_Id"].ColumnName = "CategoryId";
                dt.Columns["Category_Name"].ColumnName = "CategoryName";
                dt.Columns["Created_User"].ColumnName = "CreatedBy";
                dt.Columns["Modified_User"].ColumnName = "ModifiedBy";

                ExportTableData(dt);
            }
        }
    }

    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "ProductCategoryMaster";
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
