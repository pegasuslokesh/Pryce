using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Data.SqlClient;

public partial class SystemSetUp_CurrencyMaster : BasePage
{
    #region defind Class Object
    Common cmn = null;
    CurrencyMaster objCurr = null;
    SystemParameter objSys = null;
    CountryMaster objCountry = null;
    Country_Currency objCountryCurrency = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    string StrUserId = string.Empty;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objCurr = new CurrencyMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objCountry = new CountryMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        StrUserId = Session["UserId"].ToString();

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetup/CurrencyMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            Session["CHECKED_ITEMS"] = null;
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGridBin();
            FillGrid();
            FillCountryDDL();
            txtValue.Focus();
            DataTable dt = objCurr.Get_ActiveCurrencyMaster();
            if (dt.Rows.Count > 0)
            {
                try
                {
                    dt = new DataView(dt, "Is_BaseCurrency='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtBaseCurrency.Text = dt.Rows[0]["Currency_Name"].ToString();
                    }
                }
                catch
                {
                }
            }
            else
            {
                txtBaseCurrency.Text = "";
            }
        }
        Page.Title = objSys.GetSysTitle();

    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    #region System defind Funcation
    protected void btnList_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //PnlList.Visible = true;
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = false;
        txtValue.Focus();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //PnlList.Visible = false;
        //PnlNewEdit.Visible = true;
        //PnlBin.Visible = false;
        ddlCountry.Focus();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dtCurrEdit = new DataTable();
        try
        {
            editid.Value = e.CommandArgument.ToString();
            ViewState["Currency_Id"] = e.CommandName.ToString();
            dtCurrEdit = objCountryCurrency.GetCurrencyByCountryId(editid.Value, "1");

            if (editid.Value == string.Empty || editid.Value == null)
            {
                editid.Value = e.CommandName.ToString();
                dtCurrEdit = objCountryCurrency.GetCurrencyByCountryId(editid.Value, "2");
            }
        }
        catch
        {
            editid.Value = e.CommandName.ToString();
        }

        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        if (dtCurrEdit.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtCurrEdit.Rows[0]["Is_BaseCurrency"].ToString()) == true)
            {
                txtCurrencyValue.ReadOnly = true;
            }
            else
            {
                txtCurrencyValue.ReadOnly = false;
            }

            txtCurrencyCode.Text = dtCurrEdit.Rows[0]["Currency_Code"].ToString();
            txtCurrencyName.Text = dtCurrEdit.Rows[0]["Currency_Name"].ToString();
            ViewState["Currency_Name"] = dtCurrEdit.Rows[0]["Currency_Name"].ToString();
            txtLCurrencyName.Text = dtCurrEdit.Rows[0]["Currency_Name_L"].ToString();
            txtCurrencySymbol.Text = dtCurrEdit.Rows[0]["Currency_Symbol"].ToString();
            txtCurrencyValue.Text = dtCurrEdit.Rows[0]["Currency_Value"].ToString();
            txtCurrencyDecimalCount.Text = dtCurrEdit.Rows[0]["Field1"].ToString();
            txtCreditStatThreshold.Text= dtCurrEdit.Rows[0]["Field4"].ToString();
            if (txtCurrencyDecimalCount.Text != "")
                Txt_Small_Denomination.MaxLength = Convert.ToInt32(txtCurrencyDecimalCount.Text) + 2;
            else
                Txt_Small_Denomination.MaxLength = 0;
            Txt_Small_Denomination.Text = dtCurrEdit.Rows[0]["Field2"].ToString();
            string strCountry = dtCurrEdit.Rows[0]["Country_Id"].ToString();
            if (strCountry != "" && strCountry != "0")
            {
                ddlCountry.SelectedValue = strCountry;
            }

            txtAfterDecimal.Text = dtCurrEdit.Rows[0]["AfterDecimal"].ToString();
            btnNew_Click(null, null);
            ddlCountry.Enabled = false;
            txtCurrencyCode.Focus();
        }
        else
        {
            DisplayMessage("Country is not Assigned for this Currency");
        }

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyCode);
    }
    protected void GvCurrency_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCurrency.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Currency"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCurrency, dt, "", "");

        try
        {
            GvCurrency.HeaderRow.Focus();
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

            DataTable dtCurrency = (DataTable)Session["dtCurrency"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCurrency, view.ToTable(), "", "");
            Session["dtFilter_Currency"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }

        //btnRefresh.Focus();
        txtValue.Focus();
    }
    protected void GvCurrency_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Currency"];
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
        Session["dtFilter_Currency"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCurrency, dt, "", "");

        try
        {
            GvCurrency.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();
        string CountryId = e.CommandName.ToString();
        DataTable dt = new DataTable();
        dt = cmn.GetCheckEsistenceId(editid.Value, "7");

        DataTable dtbasecurrency = objCurr.GetCurrencyMasterById(editid.Value);
        if (dtbasecurrency.Rows.Count > 0)
        {
            try
            {
                dtbasecurrency = new DataView(dtbasecurrency, "Is_BaseCurrency='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (dtbasecurrency.Rows.Count > 0)
        {
            DisplayMessage("You Can not delete the Base Currency");
            editid.Value = "";
            return;
        }

        int b = 0;
        b = objCurr.DeleteCurrencyMaster(editid.Value, CountryId, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
        try
        {
            GvCurrency.Rows[gvRow.RowIndex].Cells[1].Focus();
        }
        catch
        {

        }
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 2;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";

        txtValue.Focus();
    }
    protected void BtnCCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        FillGridBin();
        FillGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyCode);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());

        string decimalFormat = string.Empty;
        //Condition added to check Currency name entered before save
        if (ddlCountry.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Country From List");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlCountry);
            return;
        }
        if (txtCurrencyName.Text.Trim() == "" || txtCurrencyName.Text.Trim() == null)
        {
            DisplayMessage("Enter Currency Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
            return;
        }

        if (txtCurrencyValue.Text.Trim() == "" || txtCurrencyValue.Text.Trim() == null)
        {
            DisplayMessage("Enter Currency Value");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyValue);
            return;
        }
        else
        {
            float flTemp = 0;
            if (float.TryParse(txtCurrencyValue.Text, out flTemp))
            {

            }
            else
            {
                DisplayMessage("Enter Numeric Value Only");
                txtCurrencyValue.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyValue);
                return;
            }
        }

        if (txtCurrencyDecimalCount.Text.Trim() == "" || txtCurrencyDecimalCount.Text.Trim() == "0")
        {
            decimalFormat = "0";
            txtCurrencyDecimalCount.Text = "0";
        }
        else
        {

            for (int j = 0; j < Convert.ToInt32(txtCurrencyDecimalCount.Text); j++)
            {
                if (j == 0)
                {
                    decimalFormat = "0.0";
                }
                else
                {
                    decimalFormat = decimalFormat + "0";
                }
            }

        }

        //added rollback transaction by nawaz ahmed
        //added on 11-08-2016

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {


            int b = 0;
            if (editid.Value != "")
            {
                string Currency_Id = ViewState["Currency_Id"].ToString();
                //Code to check whether the new name after edit does not exists
                if (txtCurrencyCode.Text.Trim() != "")
                {
                    DataTable dtCurr1 = objCurr.Get_ActiveCurrencyMaster(ref trns);
                    dtCurr1 = new DataView(dtCurr1, "Currency_Code='" + txtCurrencyCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCurr1.Rows.Count > 0)
                    {
                        if (dtCurr1.Rows[0]["Currency_Id"].ToString().Trim() != Currency_Id)
                        {
                            txtCurrencyCode.Text = "";
                            DisplayMessage("Currency Code is Already Exists");
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyCode);
                            return;
                        }
                    }
                }

                DataTable dtPBrand = objCurr.GetCurrencyMasterByCurrencyName(txtCurrencyName.Text.Trim(), editid.Value, ref trns);
                if (dtPBrand.Rows.Count > 0)
                {
                    if (dtPBrand.Rows[0]["Currency_Id"].ToString().Trim() != Currency_Id)
                    {
                        txtCurrencyName.Text = "";
                        DisplayMessage("Currency Name is Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
                        return;
                    }
                }
                DataTable dtCurrency = new DataTable();
                dtCurrency = objCurr.GetCurrencyMasterById(Currency_Id, ref trns);

                bool BaseCurrencyStatus = false;
                if (Convert.ToBoolean(dtCurrency.Rows[0]["Is_BaseCurrency"].ToString()) == true)
                    BaseCurrencyStatus = true;

                b = objCurr.UpdateCurrencyMaster(Currency_Id, txtCurrencyName.Text.Trim(), txtLCurrencyName.Text.Trim(), txtCurrencyCode.Text, txtCurrencySymbol.Text.Trim(), txtCurrencyValue.Text, BaseCurrencyStatus.ToString(), txtAfterDecimal.Text, Txt_Small_Denomination.Text.Trim(), "", txtCreditStatThreshold.Text, "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

                if (b != 0)
                {
                    int Result = objCountryCurrency.DeleteCountryCurrency(editid.Value);
                    int output = objCountryCurrency.InsertCountry_Currency(ddlCountry.SelectedValue, b.ToString(), txtCurrencyDecimalCount.Text.Trim(), decimalFormat, "", "", "", "True", System.DateTime.Now.ToString(), "True", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                    if (output > 0)
                    {

                    }
                    else
                    {

                    }
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    btnList_Click(null, null);
                    editid.Value = "";
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {
                if (txtCurrencyCode.Text.Trim() != "")
                {
                    DataTable dtCurr1 = objCurr.Get_ActiveCurrencyMaster(ref trns);
                    dtCurr1 = new DataView(dtCurr1, "Currency_Code='" + txtCurrencyCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtCurr1.Rows.Count > 0)
                    {
                        txtCurrencyCode.Text = "";
                        DisplayMessage("Currency Code is Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyCode);
                        return;
                    }
                }

                DataTable dtCurr = objCurr.GetCurrencyMasterByCurrencyName(txtCurrencyName.Text.Trim(), editid.Value, ref trns);
                if (dtCurr.Rows.Count > 0)
                {
                    txtCurrencyName.Text = "";
                    DisplayMessage("Currency Name is Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
                    return;
                }
                DataTable dt = objCountryCurrency.GetCurrencyByCountryId(ddlCountry.SelectedValue, "1", ref trns);
                if (dt.Rows.Count > 0)
                {
                    DisplayMessage("Record Already Exist for Selected Country");

                }
                else
                {
                    b = objCurr.InsertCurrencyMaster(txtCurrencyName.Text.Trim(), txtLCurrencyName.Text.Trim(), txtCurrencyCode.Text.Trim(), txtCurrencySymbol.Text.Trim(), txtCurrencyValue.Text.Trim(), "False", txtAfterDecimal.Text, Txt_Small_Denomination.Text.Trim(),  "", txtCreditStatThreshold.Text, "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    if (b != 0)
                    {
                        int output = objCountryCurrency.InsertCountry_Currency(ddlCountry.SelectedValue, b.ToString(), txtCurrencyDecimalCount.Text.Trim(), decimalFormat, "", "", "", "True", "1/1/1899", "True", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                        if (output > 0)
                        {

                        }
                        else
                        {

                        }
                        DisplayMessage("Record Saved","green");
                        Lbl_Tab_New.Text = Resources.Attendance.New;
                    }
                    else
                    {
                        DisplayMessage("Record Not Saved");
                    }
                }
                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                Reset();
                FillGrid();

            }
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


    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string CountryId = e.CommandName.ToString();
        int b = 0;
        b = objCurr.DeleteCurrencyMaster(editid.Value, CountryId, true.ToString(), StrUserId, DateTime.Now.ToString());
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
    protected void txtCurrencyName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objCurr.GetCurrencyMasterByCurrencyNames(txtCurrencyName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtCurrencyName.Text = "";
                DisplayMessage("Currency Name is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
                return;
            }
            DataTable dt1 = objCurr.GetCurrencyMasterInactive();
            dt1 = new DataView(dt1, "Currency_Name='" + txtCurrencyName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtCurrencyName.Text = "";
                DisplayMessage("Currency Name Already Exists in Bin Section");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
                return;
            }
        }
        else
        {
            DataTable dtTemp = objCurr.GetCurrencyMasterByCurrencyNames(txtCurrencyName.Text.Trim());
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Currency_Name"].ToString() == txtCurrencyName.Text)
                {
                    if (ViewState["Currency_Name"].ToString() == txtCurrencyName.Text)
                    {
                    }
                    else
                    {
                        DataTable dt = objCurr.GetCurrencyMasterByCurrencyName(txtCurrencyName.Text.Trim(), editid.Value);
                        if (dt.Rows.Count > 0)
                        {

                            txtCurrencyName.Text = "";
                            DisplayMessage("Currency Name is Already Exists");
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
                            return;
                        }
                    }

                    DataTable dt1 = objCurr.GetCurrencyMasterInactive();
                    dt1 = new DataView(dt1, "Currency_Name='" + txtCurrencyName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtCurrencyName.Text = "";
                        DisplayMessage("Currency Name Already Exists in Bin Section");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCurrencyName);
                        return;
                    }
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLCurrencyName);
    }

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        if (GvCurrencyBin.Rows.Count > 0)
        {
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            ImgbtnSelectAll.Visible = false;

        }
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = true;
        //PnlList.Visible = false;
        FillGridBin();
        txtValueBin.Focus();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvCurrencyBin.Rows)
        {
            index = (int)GvCurrencyBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;

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
            foreach (GridViewRow gvrow in GvCurrencyBin.Rows)
            {
                int index = (int)GvCurrencyBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void GvCurrencyBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvCurrencyBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPBrandBin"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCurrencyBin, dt, "", "");

        PopulateCheckedValuesemplog();
    }
    protected void GvCurrencyBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objCurr.GetCurrencyMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCurrencyBin, dt, "", "");
        lblSelectedRecord.Text = "";

        try
        {
            GvCurrencyBin.HeaderRow.Focus();
        }
        catch
        {

        }
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objCurr.GetCurrencyMasterInactive();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCurrencyBin, dt, "", "");

        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
       

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
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }

            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCurrencyBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            //if (view.ToTable().Rows.Count == 0)
            //{
            //    FillGridBin();
            //}
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }

        //btnRefreshBin.Focus();
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        ArrayList userdetail = new ArrayList();
        Session["CHECKED_ITEMS"] = null;
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
                        b = objCurr.DeleteCurrencyMaster(userdetail[j].ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillGridBin();
                lblSelectedRecord.Text = "";
                ViewState["Select"] = null;
                DisplayMessage("Record Activated");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvCurrencyBin.Rows)
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
        else
        {
            DisplayMessage("Please Select Record");
            GvCurrencyBin.Focus();
            return;
        }
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)GvCurrencyBin.HeaderRow.FindControl("chkCurrent"));
        foreach (GridViewRow gr in GvCurrencyBin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkSelect")).Checked = false;
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
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

        txtValueBin.Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtInactive"];
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

                if (!userdetails.Contains(dr["Currency_ID"]))
                {
                    userdetails.Add(dr["Currency_ID"]);
                }
            }
            foreach (GridViewRow GR in GvCurrencyBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCurrencyBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (GvCurrency.Rows.Count > 0)
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
                            b = objCurr.DeleteCurrencyMaster(userdetail[j].ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }

                if (b != 0)
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
                    foreach (GridViewRow Gvr in GvCurrencyBin.Rows)
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
            else
            {
                DisplayMessage("Please Select Record");
                GvCurrencyBin.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("No Record To Activate");
        }
    }
    #endregion

    #endregion

    #region User defind Funcation
    public void FillCountryDDL()
    {
        DataTable dt = objCountry.GetCountryMaster();

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlCountry, dt, "Country_Name", "Country_Id");
        }
        else
        {
            try
            {
                ddlCountry.Items.Insert(0, "--Select--");
                ddlCountry.SelectedIndex = 0;
            }
            catch
            {
                ddlCountry.Items.Insert(0, "--Select--");
                ddlCountry.SelectedIndex = 0;
            }
        }
    }
    private void FillGrid()
    {
        DataTable dtBrand = objCurr.GetCurrencyMaster();

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        Session["dtFilter_Currency"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCurrency, dtBrand, "", "");
        }
        else
        {
            GvCurrency.DataSource = null;
            GvCurrency.DataBind();
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
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        Txt_Small_Denomination.Text = "";
        Session["CHECKED_ITEMS"] = null;
        txtCurrencyDecimalCount.Text = "";
        txtCurrencyCode.Text = "";
        txtCurrencyName.Text = "";
        txtLCurrencyName.Text = "";
        txtCurrencySymbol.Text = "";
        txtCurrencyValue.Text = "";
        ddlCountry.SelectedValue = "--Select--";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 2;
        ddlOption.SelectedIndex = 1;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        ddlCountry.Enabled = true;
        ddlCountry.Focus();
        txtCurrencyValue.ReadOnly = false;
        txtAfterDecimal.Text = "";
        txtCreditStatThreshold.Text = "";
    }
    #endregion

    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        CurrencyMaster Curr = new CurrencyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Curr.GetDistinctCurrencyName(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Currency_Name"].ToString();
        }
        return str;
    }
    #endregion

    protected void Txt_Small_Denomination_TextChanged(object sender, EventArgs e)
    {
        string Denomination_Format = string.Empty;
        if (txtCurrencyDecimalCount.Text == "0")
        {
            Denomination_Format = "0";
        }
        else if (txtCurrencyDecimalCount.Text == "1")
        {
            Denomination_Format = "0.0";
        }
        else if (txtCurrencyDecimalCount.Text == "2")
        {
            Denomination_Format = "0.00";
        }
        else if (txtCurrencyDecimalCount.Text == "3")
        {
            Denomination_Format = "0.000";
        }
        else if (txtCurrencyDecimalCount.Text == "4")
        {
            Denomination_Format = "0.0000";
        }
        else if (txtCurrencyDecimalCount.Text == "5")
        {
            Denomination_Format = "0.00000";
        }
        else if (txtCurrencyDecimalCount.Text == "6")
        {
            Denomination_Format = "0.000000";
        }
        else if (txtCurrencyDecimalCount.Text == "7")
        {
            Denomination_Format = "0.0000000";
        }
        else if (txtCurrencyDecimalCount.Text == "8")
        {
            Denomination_Format = "0.00000000";
        }
        else if (txtCurrencyDecimalCount.Text == "9")
        {
            Denomination_Format = "0.000000000";
        }
        if (Denomination_Format != "")
        {
            if (Txt_Small_Denomination.Text == "")
            {
                DisplayMessage("Please Enter Smallest Denomination");
                return;
            }
            if (Txt_Small_Denomination.Text.Length != Denomination_Format.Length)
            {
                DisplayMessage("Smallest Denomination Length Not Set According To Decimal Count");
                return;
            }
        }
    }

    protected void txtCurrencyDecimalCount_TextChanged(object sender, EventArgs e)
    {
        if (txtCurrencyDecimalCount.Text != "")
            Txt_Small_Denomination.MaxLength = Convert.ToInt32(txtCurrencyDecimalCount.Text) + 2;
        else
            Txt_Small_Denomination.MaxLength = 1;

        Txt_Small_Denomination.Text = "";
    }
}
