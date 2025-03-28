using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using PegasusDataAccess;

public partial class SystemSetUp_TaxMaster : System.Web.UI.Page
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    TaxMaster objTaxMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    Inv_ProductCategory_Tax objProCategoryTax = null;
    DataAccessClass objda = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ErpLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objProCategoryTax = new Inv_ProductCategory_Tax(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetUp/TaxMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            Session["CHECKED_ITEMS"] = null;
            ddlOption.SelectedIndex = 2;
            btnNew_Click(null, null);
            FillGridBin();
            FillGrid();
            FillTaxCategory();
            FillTransactionType();
            ddlFieldName.SelectedIndex = 0;
            txtTaxName.Focus();
        }

        
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    public void FillTaxCategory()
    {
        string CategorySql = "Select Trans_Id, Category_Name from Sys_TaxCategoryMaster where IsActive = 'true'";
        DataTable dt = objda.return_DataTable(CategorySql);
        if (dt != null && dt.Rows.Count > 0)
        {
            ddlTaxCategory.DataTextField = "Category_Name";
            ddlTaxCategory.DataValueField = "Trans_Id";
            ddlTaxCategory.DataSource = dt;
            ddlTaxCategory.DataBind();
            ddlTaxCategory.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }

    public void FillTransactionType()
    {
        chklist.DataSource = Enum
        .GetValues(typeof(Common.TransactionType))
        .Cast<Common.TransactionType>()
        .Select(s => new KeyValuePair<int, string>((int)s, s.ToString()))
        .ToList();

        chklist.DataValueField = "Key";
        chklist.DataTextField = "Value";
        chklist.DataBind();
        
    }

    public void CheckUncheckAll()
    {
        foreach (ListItem item in chklist.Items)
        {
            item.Selected = false;
        }
    }

    public void SetCheckBoxList(string param)
    {
        foreach (ListItem item in chklist.Items)
        {
            if (item.Value == param)
                item.Selected = true;
        }
    }

   
    #region User Defined Funcation
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objTaxMaster.GetTaxMasterFalseAll();

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaxMasterBin, dt, "", "");
        Session["dtBinTax"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";
        
    }
    private void FillGrid()
    {
        DataTable dtBrand = objTaxMaster.GetTaxMasterTrueAll();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtTax"] = dtBrand;
        Session["dtFilter_Tax_Mstr"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvTaxMaster, dtBrand, "", "");
         
        }
        else
        {
            GvTaxMaster.DataSource = null;
            GvTaxMaster.DataBind();
        }
    }
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
        DataTable dtres = (DataTable)Session["MessageDt"];
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
    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        txtTaxName.Text = "";
        txtTaxName_L.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtTaxName_L.Text = "";
        ddlTaxCategory.SelectedIndex = 0;
        CheckUncheckAll();
        txtAccountNo.Text = string.Empty;
    }
    #endregion

    #region System Defined Funcation
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlList.Visible = true;
        //pnlBin.Visible = false;
        txtTaxName.Focus();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlList.Visible = false;
        //pnlBin.Visible = true;
        FillGridBin();
        txtValueBin.Focus();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        string qa = txtTaxName.Text.Trim();
        string AccountId = string.Empty;
        if (txtTaxName.Text.Trim() == "" || txtTaxName.Text.Trim() == null)
        {
            DisplayMessage("Please Enter Tax Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
            return;
        }

        if (String.IsNullOrEmpty(txtAccountNo.Text))
        {
            DisplayMessage("Please Enter Account No");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
            return;
        }
        else
        {
            AccountId = txtAccountNo.Text.Split('/')[1].ToString();
        }

        string TaxCategoryId = string.Empty;
        if (ddlTaxCategory.SelectedIndex > 0)
            TaxCategoryId = ddlTaxCategory.SelectedValue;
        else
            TaxCategoryId = "0";

        string TransactionType = string.Empty;
        foreach(ListItem item in chklist.Items)
        {
            if (item.Selected)
            {
                if (String.IsNullOrEmpty(TransactionType))
                    TransactionType = item.Value;
                else
                    TransactionType = TransactionType + "," + item.Value;
            }
        }

        int b = 0;
        if (editid.Value != "")
        {
            DataTable dtCate = objTaxMaster.GetTaxMaster_ByTaxName(txtTaxName.Text);
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Trans_Id"].ToString() != editid.Value)
                {
                    txtTaxName.Text = "";
                    DisplayMessage("Tax Name is Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
                    return;
                }
            }

            b = objTaxMaster.UpdateTaxMaster(editid.Value, txtTaxName.Text.Trim(), txtTaxName_L.Text.Trim().ToString(), TaxCategoryId, TransactionType, AccountId, "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                btnNew_Click(null, null);
                DisplayMessage("Record Updated", "green");
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objTaxMaster.GetTaxMasterAll();
            dtPro = new DataView(dtPro, "Tax_Name='" + txtTaxName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPro.Rows.Count > 0)
            {
                txtTaxName.Text = "";
                DisplayMessage("Tax Name is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
                return;
            }

            b = objTaxMaster.InsertTaxMaster(txtTaxName.Text.Trim(), txtTaxName_L.Text.Trim(), TaxCategoryId, TransactionType,AccountId, "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                btnNew_Click(null, null);
                DisplayMessage("Record Saved","green");
                Reset();
                FillGrid();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
    }
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString();
        String UserId = Session["UserId"].ToString();
        b = objTaxMaster.TaxMaster_UpdateStatus(editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvTaxMasterBin.Rows)
        {
            index = (int)GvTaxMasterBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;

            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }

        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvTaxMasterBin.Rows)
            {
                int index = (int)GvTaxMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void GvTaxMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvTaxMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaxMasterBin, dt, "", "");
     
        PopulateCheckedValuesemplog();
    }
    protected void GvTaxMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objTaxMaster.GetTaxMasterFalseAll();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaxMasterBin, dt, "", "");
        lblSelectedRecord.Text = "";
       
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        editid.Value = e.CommandArgument.ToString();
        DataTable dtTax = objTaxMaster.GetTaxMasterById(editid.Value);
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        txtTaxName.Text = dtTax.Rows[0]["Tax_Name"].ToString();
        txtTaxName_L.Text = dtTax.Rows[0]["TaxName_L"].ToString();
        string TaxCategory = dtTax.Rows[0]["Field1"].ToString();
        if (!String.IsNullOrEmpty(TaxCategory))
            ddlTaxCategory.SelectedValue = TaxCategory;

        string AccountId = dtTax.Rows[0]["Field3"].ToString();
        if (!String.IsNullOrEmpty(AccountId))
        {
            DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), AccountId);
            if (dtAccount.Rows.Count > 0)
                txtAccountNo.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + AccountId;
        }

        string StrField2= dtTax.Rows[0]["Field2"].ToString();
        if (!String.IsNullOrEmpty(StrField2))
        {
            string[] StrType = StrField2.Split(',');
            foreach(string value in StrType)
            {
                SetCheckBoxList(value);
            }
        }

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
    }
    protected void GvTaxMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTaxMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Tax_Mstr"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaxMaster, dt, "", "");
      
        try
        {
            GvTaxMaster.HeaderRow.Focus();
        }
        catch
        {
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
            DataTable dtCurrency = (DataTable)Session["dtTax"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvTaxMaster, view.ToTable(), "", "");
            Session["dtFilter_Tax_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
          
        }
        txtValue.Focus();
    }
    protected void GvTaxMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Tax_Mstr"];
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
        Session["dtFilter_Tax_Mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaxMaster, dt, "", "");
       
        try
        {
            GvTaxMaster.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;

        DataTable dt = new DataTable();
        dt = objProCategoryTax.GetRecord_ByTaxId(editid.Value);

        if (dt.Rows.Count > 0)
        {
            DisplayMessage("Record is in Use ,you cannot delete this");
            return;
        }

        String UserId = Session["UserId"].ToString().ToString();
        b = objTaxMaster.TaxMaster_UpdateStatus(editid.Value, "false", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
        FillGridBin();
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount COA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "AccountName Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
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

            DataTable dtCust = (DataTable)Session["dtBinTax"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvTaxMasterBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        ArrayList userdetail = new ArrayList();
        if (GvTaxMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            Msg = objTaxMaster.TaxMaster_UpdateStatus(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }

                if (Msg != 0)
                {
                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in GvTaxMasterBin.Rows)
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
            else
            {
                DisplayMessage("Please Select Record");
                try
                {
                    GvTaxMasterBin.Focus();
                }
                catch
                {
                }
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Trans_ID"]))
                {
                    userdetails.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in GvTaxMasterBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvTaxMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvTaxMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvTaxMasterBin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        txtValueBin.Focus();
    }
    protected void txtTaxName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objTaxMaster.GetTaxMasterTrueAll();
            dt = new DataView(dt, "Tax_Name='" + txtTaxName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                txtTaxName.Text = "";
                DisplayMessage("Tax Name is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
                return;
            }
            DataTable dt1 = objTaxMaster.GetTaxMasterFalseAll();
            dt1 = new DataView(dt1, "Tax_Name='" + txtTaxName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtTaxName.Text = "";
                DisplayMessage("Tax Name is Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
                return;
            }
            txtTaxName_L.Focus();
        }
        else
        {
            DataTable dtTemp = objTaxMaster.GetTaxMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Tax_Name"].ToString() != txtTaxName.Text)
                {
                    DataTable dt = objTaxMaster.GetTaxMasterTrueAll();
                    dt = new DataView(dt, "Tax_Name='" + txtTaxName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dt.Rows.Count > 0)
                    {
                        txtTaxName.Text = "";
                        DisplayMessage("Tax Name is Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
                        return;
                    }
                    DataTable dt1 = objTaxMaster.GetTaxMasterFalseAll();
                    dt1 = new DataView(dt1, "Tax_Name='" + txtTaxName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtTaxName.Text = "";
                        DisplayMessage("Tax Name is Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxName);
                        return;
                    }
                }
            }
            txtTaxName_L.Focus();
        }
    }

    protected void txtAccountNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAccountNo.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(Session["CompId"].ToString());
            dtAccount = new DataView(dtAccount, "IsActive='True' and Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
            string retval = string.Empty;
            if (txtAccountNo.Text != "")
            {
                string strAccountName = txtAccountNo.Text.Trim().Split('/')[0].ToString();
                dtAccount = new DataView(dtAccount, "AccountName='" + strAccountName + "' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtAccount.Rows.Count > 0)
                {
                    retval = (txtAccountNo.Text.Split('/'))[txtAccountNo.Text.Split('/').Length - 1];
                }
                else
                {
                    txtAccountNo.Text = "";
                    DisplayMessage("No Account Found");
                    txtAccountNo.Focus();
                    return;
                }
            }            
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAccountNo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
        }
    }

    #endregion
}