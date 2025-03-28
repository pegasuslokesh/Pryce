using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class SystemSetUp_MerchantMaster : System.Web.UI.Page
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    MerchantMaster objMerchantMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
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
        objMerchantMaster = new MerchantMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
         
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetup/MerchantMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
            ddlFieldName.SelectedIndex = 0;
            txtMerchantName.Focus();
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
        dt = objMerchantMaster.GetMerchantMasterFalseAll();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvMerchantMasterBin, dt, "", "");
        Session["dtBinMerchant"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";
        
    }
    private void FillGrid()
    {
        DataTable dtBrand = objMerchantMaster.GetMerchantMasterTrueAll();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtMerchant"] = dtBrand;
        Session["dtFilter_Merchant"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvMerchantMaster, dtBrand, "", "");
         
        }
        else
        {
            GvMerchantMaster.DataSource = null;
            GvMerchantMaster.DataBind();
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
        txtMerchantName.Text = "";
        txtMerchantName_L.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtMerchantName_L.Text = "";
        txtContactList.Text = "";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = "0";
        DataTable dt = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Filtertext"].ToString();
            }
            dt = null;
        }
        return filterlist;
    }
    #endregion

    #region System Defined Funcation
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlList.Visible = true;
        //pnlBin.Visible = false;
        txtMerchantName.Focus();
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        string qa = txtMerchantName.Text.Trim();

        if (txtMerchantName.Text.Trim() == "" || txtMerchantName.Text.Trim() == null)
        {
            DisplayMessage("Enter Merchant Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
            return;
        }

        string strContactId = string.Empty;
        if (!string.IsNullOrEmpty(txtContactList.Text))
        {
            strContactId = txtContactList.Text.Split('/')[1].ToString();
        }

        int b = 0;
        if (editid.Value != "")
        {
            DataTable dtCate = objMerchantMaster.GetMerchantMaster_ByMerchantName(txtMerchantName.Text);
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Trans_Id"].ToString() != editid.Value)
                {
                    txtMerchantName.Text = "";
                    DisplayMessage("Merchant Name is Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
                    return;
                }
            }

            b = objMerchantMaster.UpdateMerchantMaster(editid.Value, txtMerchantName.Text.Trim(), txtMerchantName_L.Text.Trim().ToString(), strContactId, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                DisplayMessage("Record Updated", "green");
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objMerchantMaster.GetMerchantMasterAll();
            dtPro = new DataView(dtPro, "Merchant_Name='" + txtMerchantName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPro.Rows.Count > 0)
            {
                txtMerchantName.Text = "";
                DisplayMessage("Merchant Name is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
                return;
            }

            b = objMerchantMaster.InsertMerchantMaster(txtMerchantName.Text.Trim(), txtMerchantName_L.Text.Trim(), strContactId, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
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
        b = objMerchantMaster.MerchantMaster_UpdateStatus(editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        foreach (GridViewRow gvrow in GvMerchantMasterBin.Rows)
        {
            index = (int)GvMerchantMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in GvMerchantMasterBin.Rows)
            {
                int index = (int)GvMerchantMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void GvMerchantMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvMerchantMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvMerchantMasterBin, dt, "", "");
 
        PopulateCheckedValuesemplog();
    }
    protected void GvMerchantMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objMerchantMaster.GetMerchantMasterFalseAll();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvMerchantMasterBin, dt, "", "");
        lblSelectedRecord.Text = "";
       
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        try
        {
            editid.Value = e.CommandArgument.ToString();
            DataTable dtMerchant = objMerchantMaster.GetMerchantMasterById(editid.Value);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtMerchantName.Text = dtMerchant.Rows[0]["Merchant_Name"].ToString();
            txtMerchantName_L.Text = dtMerchant.Rows[0]["Merchant_Name_L"].ToString();
            if (!string.IsNullOrEmpty(dtMerchant.Rows[0]["field1"].ToString()) && dtMerchant.Rows[0]["field1"].ToString() != "0")
            {
                string contactName = new Ems_ContactMaster(Session["DBConnection"].ToString()).GetContactNameByContactiD(dtMerchant.Rows[0]["field1"].ToString());
                if (!string.IsNullOrEmpty(contactName))
                {
                    txtContactList.Text = contactName + "/" + dtMerchant.Rows[0]["field1"].ToString();
                }

            }

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
        }
        catch(Exception ex)
        {
            DisplayMessage(ex.Message);
        }
    }
    protected void GvMerchantMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvMerchantMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Merchant"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvMerchantMaster, dt, "", "");
        
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
            DataTable dtCurrency = (DataTable)Session["dtMerchant"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvMerchantMaster, view.ToTable(), "", "");
            Session["dtFilter_Merchant"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
          
        }
        txtValue.Focus();
    }
    protected void GvMerchantMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Merchant"];
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
        Session["dtFilter_Merchant"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvMerchantMaster, dt, "", "");
        
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;
        //here we check that merchant name is in use or not 


        //this page is created by jitnedr aupadhyay on 30-04-2015



        DataTable dt = objSInvHeader.GetSInvHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        try
        {
            dt = new DataView(dt, "Invoice_Merchant_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            DisplayMessage("Merchant is in use ,you can not delete");
            return;
        }



        editid.Value = e.CommandArgument.ToString();

        String UserId = Session["UserId"].ToString().ToString();
        b = objMerchantMaster.MerchantMaster_UpdateStatus(editid.Value, "false", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
    public static string[] GetCompletionListMerchant(string prefixText, int count, string contextKey)
    {
        MerchantMaster objMerchantMaster = new MerchantMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objMerchantMaster.GetMerchantMaster_ByMerchantName(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Merchant_Name"].ToString();
        }
        return txt;
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

            DataTable dtCust = (DataTable)Session["dtBinMerchant"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvMerchantMasterBin, view.ToTable(), "", "");
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
        if (GvMerchantMasterBin.Rows.Count > 0)
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
                            Msg = objMerchantMaster.MerchantMaster_UpdateStatus(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                    foreach (GridViewRow Gvr in GvMerchantMasterBin.Rows)
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
                    GvMerchantMasterBin.Focus();
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
            foreach (GridViewRow GR in GvMerchantMasterBin.Rows)
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
            objPageCmn.FillData((object)GvMerchantMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)GvMerchantMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvMerchantMasterBin.Rows)
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
    protected void txtMerchantName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objMerchantMaster.GetMerchantMasterTrueAll();
            dt = new DataView(dt, "Merchant_Name='" + txtMerchantName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                txtMerchantName.Text = "";
                DisplayMessage("Merchant Name is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
                return;
            }
            DataTable dt1 = objMerchantMaster.GetMerchantMasterFalseAll();
            dt1 = new DataView(dt1, "Merchant_Name='" + txtMerchantName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtMerchantName.Text = "";
                DisplayMessage("Merchant Name is Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
                return;
            }
            txtMerchantName_L.Focus();
        }
        else
        {
            DataTable dtTemp = objMerchantMaster.GetMerchantMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Merchant_Name"].ToString() != txtMerchantName.Text)
                {
                    DataTable dt = objMerchantMaster.GetMerchantMasterTrueAll();
                    dt = new DataView(dt, "Merchant_Name='" + txtMerchantName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dt.Rows.Count > 0)
                    {
                        txtMerchantName.Text = "";
                        DisplayMessage("Merchant Name is Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
                        return;
                    }
                    DataTable dt1 = objMerchantMaster.GetMerchantMasterFalseAll();
                    dt1 = new DataView(dt1, "Merchant_Name='" + txtMerchantName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtMerchantName.Text = "";
                        DisplayMessage("Merchant Name is Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtMerchantName);
                        return;
                    }
                }
            }
            txtMerchantName_L.Focus();
        }
    }
    #endregion

}