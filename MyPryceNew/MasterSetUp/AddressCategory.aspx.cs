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

public partial class MasterSetUp_AddressCategory : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    Set_AddressCategory objAddCat = null;
    SystemParameter ObjSysParam = null;
    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objAddCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/AddressCategory.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();

            ddlOption.SelectedIndex = 2;
            btnNew_Click(null, null);
            FillGridBin();
            FillGrid();
            Session["CHECKED_ITEMS"] = null;
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
    #region User Defined Funcation
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objAddCat.GetAddressCategoryFalseAll(StrCompId.ToString());
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressCategoryBin, dt, "", "");
        Session["dtBinAddressCategory"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        Session["CHECKED_ITEMS"] = null;

       
    }
    private void FillGrid()
    {
        DataTable dtBrand = objAddCat.GetAddressCategoryTrueAll(StrCompId.ToString());
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtAddressCategory"] = dtBrand;
        Session["dtFilter_Addr_Cat"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressCategory, dtBrand, "", "");
        }
        else
        {
            GvAddressCategory.DataSource = null;
            GvAddressCategory.DataBind();
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
        txtAddressName.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        Session["CHECKED_ITEMS"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtAddressNameL.Text = "";
        Session["CHECKED_ITEMS"] = null;
    }
    #endregion
    #region Auto Complete Method/Funcation
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Set_AddressCategory obj = new Set_AddressCategory(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = obj.GetDistinctAddressName(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] str = new string[dt.Rows.Count];

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i][0].ToString();
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
                dt = obj.GetAddressCategoryTrueAll("1");
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Address_Name"].ToString();
                    }
                }
            }
        }
        return str;
    }
    #endregion
    #region System Defined Funcation

    protected void btnNew_Click(object sender, EventArgs e)
    {
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlList.Visible = true;
        //pnlBin.Visible = false;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlList.Visible = false;
        //pnlBin.Visible = true;
        FillGridBin();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtAddressName.Text == "" || txtAddressName.Text == null)
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {

            DataTable dtCate = objAddCat.GetAddressCategoryTrueAll(StrCompId.ToString());
            dtCate = new DataView(dtCate, "Address_Name='" + txtAddressName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Address_Category_ID"].ToString() != editid.Value)
                {
                    txtAddressName.Text = "";
                    DisplayMessage("Address Category Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    return;
                }
            }


            b = objAddCat.UpdateAddressCategory(editid.Value, txtAddressName.Text, txtAddressNameL.Text.Trim().ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                DisplayMessage("Record Updated", "green");

            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objAddCat.GetAddressCategoryTrueAll(StrCompId.ToString());
            dtPro = new DataView(dtPro, "Address_Name='" + txtAddressName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtAddressName.Text = "";
                DisplayMessage("Address Category Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                return;
            }

            b = objAddCat.InsertAddressCategory(txtAddressName.Text, txtAddressNameL.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved","green");
                Reset();
                FillGrid();
            }
            else
            {
                DisplayMessage("Record  Not Saved");
            }
        }
    }
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString();
        String UserId = Session["UserId"].ToString();
        b = objAddCat.DeleteAddressCategory(editid.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
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
    protected void GvAddressCategoryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvAddressCategoryBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinAddressCategory"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressCategoryBin, dt, "", "");
        PopulateCheckedValues();
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvAddressCategoryBin.Rows)
            {
                int index = (int)GvAddressCategoryBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvAddressCategoryBin.Rows)
        {
            index = (int)GvAddressCategoryBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void GvAddressCategoryBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objAddCat.GetAddressCategoryFalseAll(StrCompId.ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressCategoryBin, dt, "", "");
        Session["CHECKED_ITEMS"] = null;
  
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtTax = objAddCat.GetAddressCategoryTruebyId(editid.Value);
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        txtAddressName.Text = dtTax.Rows[0]["Address_Name"].ToString();
        txtAddressNameL.Text = dtTax.Rows[0]["Address_Name_L"].ToString();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
    }
    protected void GvAddressCategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvAddressCategory.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Addr_Cat"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressCategory, dt, "", "");
      
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
            DataTable dtCurrency = (DataTable)Session["dtAddressCategory"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressCategory, view.ToTable(), "", "");
            Session["dtFilter_Addr_Cat"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
          
        }
        txtValue.Focus();
    }
    protected void GvAddressCategory_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Addr_Cat"];
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
        Session["dtFilter_Addr_Cat"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressCategory, dt, "", "");
     
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = StrCompId.ToString();
        String UserId = StrUserId.ToString();
        DataTable dtCount = objAddCat.CheckAddressExistance(editid.Value, "1");
        if (dtCount.Rows.Count > 0)
        {
            DisplayMessage("Record Used in other page, So it can not be deleted");
        }
        else
        {
            b = objAddCat.DeleteAddressCategory(editid.Value, "false", StrUserId.ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Deleted");
            }
            else
            {
                DisplayMessage("Record  Not Deleted");
            }
        }
        FillGridBin();
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
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

            DataTable dtCust = (DataTable)Session["dtBinAddressCategory"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressCategoryBin, view.ToTable(), "", "");

            Session["CHECKED_ITEMS"] = null;
           
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        SaveCheckedValues();
        int Msg = 0;
        if (GvAddressCategoryBin.Rows.Count != 0)
        {
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = objAddCat.DeleteAddressCategory(userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }

                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        Session["CHECKED_ITEMS"] = null;
                        DisplayMessage("Record Activated");
                        Session["CHECKED_ITEMS"] = null;
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
                return;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {

        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtDEPARTMENT.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Address_Category_ID"])))
                    userdetails.Add(Convert.ToInt32(dr["Address_Category_ID"]));

            }
            foreach (GridViewRow gvrow in GvAddressCategoryBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressCategoryBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)GvAddressCategoryBin.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvAddressCategoryBin.Rows)
        {
            index = (int)GvAddressCategoryBin.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        Session["CHECKED_ITEMS"] = null;
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void txtAddressName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objAddCat.GetAddressCategoryByAddressName(txtAddressName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtAddressName.Text = "";
                DisplayMessage("Address Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                return;
            }
            DataTable dt1 = objAddCat.GetAddressCategoryFalseAll(StrCompId.ToString());
            dt1 = new DataView(dt1, "Address_Name='" + txtAddressName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtAddressName.Text = "";
                DisplayMessage("Address Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                return;
            }
        }
        else
        {
            DataTable dtTemp = objAddCat.GetAddressCategoryTruebyId(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Address_Name"].ToString() != txtAddressName.Text)
                {
                    DataTable dt = objAddCat.GetAddressCategoryTrueAll(StrCompId.ToString());
                    dt = new DataView(dt, "Address_Name='" + txtAddressName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtAddressName.Text = "";
                        DisplayMessage("Address Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                        return;
                    }
                    DataTable dt1 = objAddCat.GetAddressCategoryFalseAll(StrCompId.ToString());
                    dt1 = new DataView(dt1, "Address_Name='" + txtAddressName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtAddressName.Text = "";
                        DisplayMessage("Address Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                        return;
                    }
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnCSave);
    }
    #endregion
}
