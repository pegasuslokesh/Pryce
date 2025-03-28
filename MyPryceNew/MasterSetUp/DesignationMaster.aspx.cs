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

public partial class MasterSetUp_DesignationMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
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
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/DesignationMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtDesignationName.Focus();
            Session["CHECKED_ITEMS"] = null;
            ddlOption.SelectedIndex = 2;
            btnNew_Click(null, null);
            FillGridBin();
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtDesignationName.Focus();
        }
        txtDesignationName.Focus();

    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    #region User Defined Funcation
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objDesg.GetDesignationMasterInactive();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvDesignationBin, dt, "", "");
        Session["dtBinDesignation"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";

    }
    private void FillGrid()
    {
        DataTable dtDesignation = objDesg.GetDesignationMaster();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtDesignation.Rows.Count + "";
        Session["dtDesignation"] = dtDesignation;
        Session["dtFilter_EmpDsgn_Mstr"] = dtDesignation;
        if (dtDesignation != null && dtDesignation.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvDesignation, dtDesignation, "", "");

        }
        else
        {
            GvDesignation.DataSource = null;
            GvDesignation.DataBind();
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
        txtDesignationName.Text = "";
        txtDesignationNameL.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtDesignationNameL.Text = "";
    }
    #endregion


    #region System Defined Funcation

    protected void btnNew_Click(object sender, EventArgs e)
    {
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        txtDesignationName.Focus();

        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //pnlList.Visible = true;

        //pnlBin.Visible = false;

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtValueBin.Focus();
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //pnlList.Visible = false;
        //pnlBin.Visible = true;

        FillGridBin();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtDesignationName.Text == "" || txtDesignationName.Text == null)
        {
            DisplayMessage("Enter Designation Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {

            DataTable dtCate = objDesg.GetDesignationMaster();
            dtCate = new DataView(dtCate, "Designation='" + txtDesignationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Designation_ID"].ToString() != editid.Value)
                {
                    txtDesignationName.Text = "";
                    DisplayMessage("Designation Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
                    return;
                }
            }


            b = objDesg.UpdateDesignationMaster(editid.Value, txtDesignationName.Text, txtDesignationNameL.Text.Trim().ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
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
            DataTable dtPro = objDesg.GetDesignationMaster();
            dtPro = new DataView(dtPro, "Designation='" + txtDesignationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtDesignationName.Text = "";
                DisplayMessage("Designation Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
                return;
            }

            b = objDesg.InsertDesignationMaster(txtDesignationName.Text, txtDesignationNameL.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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

        String UserId = Session["UserId"].ToString();
        b = objDesg.DeleteDesignationMaster(editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Restored");
        }
        else
        {
            DisplayMessage("Record  Restore Fail");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvDesignationBin.Rows)
        {
            index = (int)GvDesignationBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in GvDesignationBin.Rows)
            {
                int index = (int)GvDesignationBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void GvDesignationBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvDesignationBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvDesignationBin, dt, "", "");

        PopulateCheckedValuesemplog();
    }
    protected void GvDesignationBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objDesg.GetDesignationMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvDesignationBin, dt, "", "");

        lblSelectedRecord.Text = "";
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtEdit = objDesg.GetDesignationMasterById(editid.Value);
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        txtDesignationName.Text = dtEdit.Rows[0]["Designation"].ToString();
        txtDesignationNameL.Text = dtEdit.Rows[0]["Designation_L"].ToString();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
    }
    protected void GvDesignation_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvDesignation.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_EmpDsgn_Mstr"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvDesignation, dt, "", "");

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
            DataTable dtCurrency = (DataTable)Session["dtDesignation"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvDesignation, view.ToTable(), "", "");
            Session["dtFilter_EmpDsgn_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

        }
        txtValue.Focus();
    }
    protected void GvDesignation_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_EmpDsgn_Mstr"];
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
        dt = (new DataView(dt, "", "" + e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtFilter_EmpDsgn_Mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvDesignation, dt, "", "");

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;

        DataTable dt = new DataTable();
        dt = cmn.GetCheckEsistenceId(editid.Value, "6");

        if (dt.Rows.Count > 0)
        {
            DisplayMessage(" You Can Not Delete  This Record Is Currently Used");
            return;
        }


        String UserId = Session["UserId"].ToString();
        b = objDesg.DeleteDesignationMaster(editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin();
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtDesignationName.Focus();
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        ddlFieldName.SelectedIndex = 0;
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

            DataTable dtCust = (DataTable)Session["dtBinDesignation"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvDesignationBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (GvDesignationBin.Rows.Count > 0)
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
                            b = objDesg.DeleteDesignationMaster(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
                    foreach (GridViewRow Gvr in GvDesignationBin.Rows)
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
                GvDesignationBin.Focus();
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

                if (!userdetails.Contains(dr["Designation_ID"]))
                {
                    userdetails.Add(dr["Designation_ID"]);
                }
            }
            foreach (GridViewRow GR in GvDesignationBin.Rows)
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
            objPageCmn.FillData((object)GvDesignationBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)GvDesignationBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvDesignationBin.Rows)
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
        txtValueBin.Focus();
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void txtDesignationName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objDesg.GetDesignationMasterByDesignationName(txtDesignationName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtDesignationName.Text = "";
                DisplayMessage("Designation Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
                return;
            }
            DataTable dt1 = objDesg.GetDesignationMasterInactive();
            dt1 = new DataView(dt1, "Designation='" + txtDesignationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtDesignationName.Text = "";
                DisplayMessage("Designation Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
                return;
            }
            txtDesignationNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objDesg.GetDesignationMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Designation"].ToString() != txtDesignationName.Text)
                {
                    DataTable dt = objDesg.GetDesignationMaster();
                    dt = new DataView(dt, "Designation='" + txtDesignationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtDesignationName.Text = "";
                        DisplayMessage("Designation Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
                        return;
                    }
                    DataTable dt1 = objDesg.GetDesignationMasterInactive();
                    dt1 = new DataView(dt1, "Designation='" + txtDesignationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtDesignationName.Text = "";
                        DisplayMessage("Designation Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDesignationName);
                        return;
                    }
                }
            }
            txtDesignationNameL.Focus();
        }

    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesg(string prefixText, int count, string contextKey)
    {
        DesignationMaster objquali = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objquali.GetDesignationMaster(), "Designation like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Designation"].ToString();
        }
        return txt;
    }
    #endregion
}
