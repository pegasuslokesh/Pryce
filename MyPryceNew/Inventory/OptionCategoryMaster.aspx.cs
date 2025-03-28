using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Inventory_OptionCategoryMaster : BasePage
{
    #region Object Defined
    Common cmn = null;
    Inv_OptionCategoryMaster objOptionCategory = null;
    ProductOptionCategoryDetail objOptioncategoryDetail = null;    
    SystemParameter ObjSysPeram = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objOptionCategory = new Inv_OptionCategoryMaster(Session["DBConnection"].ToString());
        objOptioncategoryDetail = new ProductOptionCategoryDetail(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/OptionCategoryMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillGrid();
        }
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    #endregion


    #region System Defined Function:-Events

    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Focus();
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        btnCSave.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (txtOpCateName.Text == "")
        {
            DisplayMessage("Enter Option Category Name");
            Focus();
            btnCSave.Enabled = true;
            return;
        }
        if (chkPeramter.Checked)
        {
            if (txtMeterialCategory.Text == "")
            {
                DisplayMessage("Enter Meterial Category Name");
                Focus();
                btnCSave.Enabled = true;
                return;
            }
            if (txtColorCategory.Text == "")
            {
                DisplayMessage("Enter Color Category Name");
                Focus();
                btnCSave.Enabled = true;
                return;
            }
            if (txtQuantityCategory.Text == "")
            {
                DisplayMessage("Enter Quantity Category Name");
                Focus();
                btnCSave.Enabled = true;
                return;
            }
            if (txtDefaultValue.Text == "")
            {
                DisplayMessage("Enter Default Value");
                Focus();
                btnCSave.Enabled = true;
                return;
            }

        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        int b = 0;
        try
        {
            if (editid.Value == "")
            {
                b = objOptionCategory.InsertOptionCategory(Session["CompId"].ToString().ToString(), txtOpCateName.Text.Trim(), txtlblOpCateNameLocal.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DisplayMessage("Record Saved","green");

                }
            }
            else
            {
                b = objOptionCategory.UpdateOptionCategory(Session["CompId"].ToString().ToString(), editid.Value.ToString(), txtOpCateName.Text.Trim(), txtlblOpCateNameLocal.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");

                }

            }
            string CateId = string.Empty;
            if (editid.Value != "")
            {
                CateId = editid.Value;
            }
            else
            {
                CateId = b.ToString();
            }
            objOptioncategoryDetail.ProductOpCateDelete(Session["CompId"].ToString().ToString(), CateId.ToString(), ref trns);
            if (chkPeramter.Checked)
            {

                objOptioncategoryDetail.ProductOpCateDelete(Session["CompId"].ToString().ToString(), ref trns);
                objOptioncategoryDetail.ProductOpCateInsert(Session["CompId"].ToString(), CateId, "0", "0", "0", "0", txtDefaultValue.Text.Trim(), txtMeterialCategory.Text.Split('/')[1].ToString(), txtColorCategory.Text.Split('/')[1].ToString(), txtQuantityCategory.Text.Split('/')[1].ToString(), txtPackingCategory.Text.Split('/')[1].ToString(), txtCore.Text.Split('/')[1].ToString(), txtPerforation.Text.Split('/')[1].ToString(), "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

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
            Reset();
            txtOpCateName.Focus();
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
            btnCSave.Enabled = true;
            return;
        }
        btnCSave.Enabled = true;
    }
    protected void GvOptionCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvOptionCategory.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvOptionCategory, dt, "", "");


        //AllPageCode();
        GvOptionCategory.BottomPagerRow.Focus();
    }
    protected void GvOptionCategory_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtFilter"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";

            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }





        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvOptionCategory, dt, "", "");



        //AllPageCode();
        GvOptionCategory.HeaderRow.Focus();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objOptionCategory.GetOptionCategoryTruebyId(Session["CompId"].ToString().ToString(), editid.Value);
        if (dt.Rows.Count != 0)
        {
            txtOpCateName.Text = dt.Rows[0]["EName"].ToString();
            txtlblOpCateNameLocal.Text = dt.Rows[0]["LName"].ToString();
            DataTable dtdetail = objOptioncategoryDetail.GetDetailByOpCateId(Session["CompId"].ToString(), editid.Value);
            if (dtdetail.Rows.Count != 0)
            {
                chkPeramter.Checked = true;
                pnlPerameter.Visible = true;
                txtMeterialCategory.Text = dtdetail.Rows[0]["MaterialCategoryName"].ToString() + "/" + dtdetail.Rows[0]["MaterialCategoryId"].ToString();
                txtColorCategory.Text = dtdetail.Rows[0]["ColorCategoryName"].ToString() + "/" + dtdetail.Rows[0]["ColorCategoryId"].ToString();
                txtQuantityCategory.Text = dtdetail.Rows[0]["QtyCategoryName"].ToString() + "/" + dtdetail.Rows[0]["QtyCategoryId"].ToString();
                txtPackingCategory.Text = dtdetail.Rows[0]["PackingCategoryName"].ToString() + "/" + dtdetail.Rows[0]["Field1"].ToString();
                txtCore.Text = dtdetail.Rows[0]["CoreCategoryName"].ToString() + "/" + dtdetail.Rows[0]["Field2"].ToString();
                txtPerforation.Text = dtdetail.Rows[0]["PerforationCategoryName"].ToString() + "/" + dtdetail.Rows[0]["Field3"].ToString();
                txtDefaultValue.Text = dtdetail.Rows[0]["DefaultValue"].ToString();
                for (int i = 0; i < dtdetail.Rows.Count; i++)
                {
                    string ItemTaxt = dtdetail.Rows[i]["Width"].ToString() + "X" + dtdetail.Rows[i]["Height"].ToString() + " " + "(MM) - " + dtdetail.Rows[i]["gap"].ToString() + " (gap)";
                    string ItemValue = dtdetail.Rows[i]["MMSize"].ToString();

                    chkSelectedItems.Items.Add(new ListItem(ItemTaxt, ItemValue));
                }
            }
            txtOpCateName.Focus();
        }
        Lbl_Tab_New.Text = Resources.Attendance.Edit;

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objOptionCategory.DeleteOptionCategory(Session["CompId"].ToString().ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            BtnReset_Click(null, null);
        }
        try
        {
            int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            ((LinkButton)GvOptionCategory.Rows[i].FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
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
            DataTable dtCurrency = (DataTable)Session["dtoptionCate"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvOptionCategory, view.ToTable(), "", "");


            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            //AllPageCode();            
        }
        txtValue.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        FillGrid();
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtOpCateName.Focus();
    }

    protected void txtOpCateName_TextChanged(object sender, EventArgs e)
    {
        if (txtOpCateName.Text != "")
        {
            DataTable dt = new DataView(objOptionCategory.GetOptionCategoryAll(Session["CompId"].ToString().ToString()), "EName='" + txtOpCateName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0]["OptionCategoryId"].ToString() != editid.Value.Trim())
                {
                    if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Option Category Already Exists");
                        txtOpCateName.Text = "";
                        txtOpCateName.Focus();
                    }
                    else
                    {
                        DisplayMessage("Option Category Already Exists :- Go to Bin Tab");
                        txtOpCateName.Text = "";
                        txtOpCateName.Focus();

                    }
                }

            }

            else
            {
                txtlblOpCateNameLocal.Focus();
            }


        }
    }


    #region Bin

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

            DataTable dtCust = (DataTable)Session["dtBinOpCate"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvOptionCategoryBin, view.ToTable(), "", "");


            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtValueBin.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        txtValueBin.Focus();

    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        DataTable dt = objOptionCategory.GetAddressCategoryFalseAll(Session["CompId"].ToString().ToString());

        if (GvOptionCategoryBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {

                        Msg = objOptionCategory.DeleteOptionCategory(Session["CompId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (Msg != 0)
            {
                FillGrid();
                FillGridBin();
                ViewState["Select"] = null;
                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
                btnRefreshBin_Click(null, null);

            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvOptionCategoryBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
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
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtOptionCategory = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtOptionCategory.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["OptionCategoryID"]))
                {
                    lblSelectedRecord.Text += dr["OptionCategoryID"] + ",";
                }
            }
            for (int i = 0; i < GvOptionCategoryBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvOptionCategoryBin.Rows[i].FindControl("lblgvOptionCategoryId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvOptionCategoryBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtOptionCategory1 = (DataTable)Session["dtBinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvOptionCategoryBin, dtOptionCategory1, "", "");

            //AllPageCode();
            ViewState["Select"] = null;
        }

        ImgbtnSelectAll.Focus();


    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string OptCateidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvOptionCategoryBin.Rows[index].FindControl("lblgvOptionCategoryId");
        if (((CheckBox)GvOptionCategoryBin.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            OptCateidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += OptCateidlist;
        }
        else
        {
            OptCateidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += OptCateidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != OptCateidlist)
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
        ((CheckBox)GvOptionCategoryBin.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvOptionCategoryBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < GvOptionCategoryBin.Rows.Count; i++)
        {
            ((CheckBox)GvOptionCategoryBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvOptionCategoryBin.Rows[i].FindControl("lblgvOptionCategoryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvOptionCategoryBin.Rows[i].FindControl("lblgvOptionCategoryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvOptionCategoryBin.Rows[i].FindControl("lblgvOptionCategoryId"))).Text.Trim().ToString())
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
    protected void GvOptionCategoryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvOptionCategoryBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtBinFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvOptionCategoryBin, dt, "", "");


        //AllPageCode();
        string temp = string.Empty;


        for (int i = 0; i < GvOptionCategoryBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvOptionCategoryBin.Rows[i].FindControl("lblgvOptionCategoryId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvOptionCategoryBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        GvOptionCategoryBin.BottomPagerRow.Focus();
    }
    protected void GvOptionCategoryBin_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtBinFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvOptionCategoryBin, dt, "", "");


        lblSelectedRecord.Text = "";
        //AllPageCode();
        GvOptionCategoryBin.HeaderRow.Focus();
    }
    #endregion
    #region Auto Complete Method

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_OptionCategoryMaster ObjOptCat = new Inv_OptionCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjOptCat.GetOptionCategoryTrueAll(HttpContext.Current.Session["CompId"].ToString()), "EName like '%" + prefixText.ToString() + "%'", "EName asc", DataViewRowState.CurrentRows).ToTable();
        string[] text = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            text[i] = dt.Rows[i]["EName"].ToString();

        }
        return text;

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList1(string prefixText, int count, string contextKey)
    {
        Inv_OptionCategoryMaster ObjOptCat = new Inv_OptionCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjOptCat.GetOptionCategoryTrueAll(HttpContext.Current.Session["CompId"].ToString()), "EName like '%" + prefixText.ToString() + "%'", "EName asc", DataViewRowState.CurrentRows).ToTable();
        string[] text = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            text[i] = dt.Rows[i]["EName"].ToString() + "/" + dt.Rows[i]["OptionCategoryId"].ToString();

        }
        return text;

    }


    #endregion

    #endregion


    #region User Defined Function

    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
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
    public void FillGrid()
    {
        DataTable dt = objOptionCategory.GetOptionCategoryTrueAll(Session["CompId"].ToString().ToString());
        Session["dtoptionCate"] = dt;
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvOptionCategory, dt, "", "");


        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString();
        //AllPageCode();
    }
    public void Reset()
    {
        txtOpCateName.Text = "";
        txtlblOpCateNameLocal.Text = "";
        editid.Value = "";
        btnRefreshReport_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtMeterialCategory.Text = "";
        chkPeramter.Checked = false;
        pnlPerameter.Visible = false;
        txtColorCategory.Text = "";
        txtQuantityCategory.Text = "";
        txtDefaultValue.Text = "";
        txtwidth.Text = "";
        txtHeight.Text = "";
        txtgap.Text = "";
        chkSelectedItems.Items.Clear();
        txtPackingCategory.Text = "";
        txtPerforation.Text = "";
        txtCore.Text = "";
    }

    #region//Bin
    public void FillGridBin()
    {

        DataTable dt = objOptionCategory.GetAddressCategoryFalseAll(Session["CompId"].ToString().ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvOptionCategoryBin, dt, "", "");


        Session["dtBinOpCate"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";

        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    #endregion
    #endregion

    //Add by akshay discuss with neeraj sir For Label
    protected void chkPeramter_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPeramter.Checked)
        {
            pnlPerameter.Visible = true;
        }
        else
        {
            pnlPerameter.Visible = false;
        }
    }
    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (txtwidth.Text == "")
        {
            DisplayMessage("Enter Width");

            Focus();
            return;
        }
        if (txtHeight.Text == "")
        {
            DisplayMessage("Enter Heigth");

            Focus();
            return;
        }
        if (txtgap.Text == "")
        {
            DisplayMessage("Enter Gap");

            Focus();
            return;
        }
        string ItemTaxt = txtwidth.Text.Trim() + "X" + txtHeight.Text.Trim() + " " + "(MM) - " + txtgap.Text.Trim() + " (gap)";
        string ItemValue = ((float.Parse(txtwidth.Text.Trim()) + float.Parse(txtgap.Text.Trim())) * (float.Parse(txtHeight.Text.Trim()) + float.Parse(txtgap.Text.Trim()))).ToString();
        if (chkSelectedItems.Items.FindByText(ItemTaxt) == null && chkSelectedItems.Items.FindByValue(ItemValue) == null)
        {
            chkSelectedItems.Items.Add(new ListItem(ItemTaxt, ItemValue));
        }

        chkSelectedItems.SelectedValue = ItemValue;

        txtHeight.Text = "";
        txtwidth.Text = "";
        txtgap.Text = "";
    }
    protected void imgDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (chkSelectedItems.SelectedItem != null)
        {
            chkSelectedItems.Items.RemoveAt(chkSelectedItems.SelectedIndex);
        }
        else
        {
            DisplayMessage("Select Item");
        }
    }


    protected void txtComman_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        if (txt.Text != "")
        {
            DataTable dt = new DataView(objOptionCategory.GetOptionCategoryTrueAll(Session["CompId"].ToString().ToString()), "EName='" + txt.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {

                DisplayMessage("Select In Suggestion list");
                txt.Text = "";
                txt.Focus();

            }

            else
            {
                txt.Focus();
            }


        }
    }


}
