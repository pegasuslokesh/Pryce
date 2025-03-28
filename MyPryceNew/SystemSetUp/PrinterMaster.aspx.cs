using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class SystemSetUp_PrinterMaster : System.Web.UI.Page
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    set_PrinterMaster objPrinter = null;
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
        objPrinter = new set_PrinterMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        //selected_tab.Value = Request.Form[selected_tab.UniqueID];
        //AllPageCode();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/ReligionMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            Session["CHECKED_ITEMS"] = null;
            ddlOption.SelectedIndex = 2;
            //btnNew_Click(null, null);
            FillGridBin();
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtPrinterName.Focus();
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #region User Defined Funcation
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objPrinter.GetPrinterMasterInactive();
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvPrinterBin, dt, "", "");
        Session["dtBinReligion"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            // imgBtnRestore.Visible = false;
            // ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }

    }
    private void FillGrid()
    {
        DataTable dtReligion = objPrinter.GetPrinterMaster();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtReligion.Rows.Count + "";
        Session["dtReligion"] = dtReligion;
        Session["dtFilter_Religion_Mstr"] = dtReligion;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvPrinter, dtReligion, "", "");
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
        Session["CHECKED_ITEMS"] = null;
        txtPrinterName.Text = "";
        txtPrinterNameL.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtPrinterNameL.Text = "";
    }
    #endregion


    #region System Defined Funcation
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtPrinterName.Text.Trim() == "" || txtPrinterName.Text.Trim() == null)
        {
            DisplayMessage("Please Enter Printer Name");
            txtPrinterName.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {
            DataTable dtCate = objPrinter.GetPrinterMaster();
            dtCate = new DataView(dtCate, "Printer_Name='" + txtPrinterName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Printer_id"].ToString() != editid.Value)
                {
                    txtPrinterName.Text = "";
                    DisplayMessage("Printer Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
                    return;
                }
            }

            b = objPrinter.UpdatePrinterMaster(editid.Value, txtPrinterName.Text.Trim(), txtPrinterNameL.Text.Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            editid.Value = "";
            if (b != 0)
            {
                Reset();
                FillGrid();
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objPrinter.GetPrinterMaster();
            dtPro = new DataView(dtPro, "Printer_Name='" + txtPrinterName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtPrinterName.Text = "";
                DisplayMessage("Printer Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
                return;
            }

            b = objPrinter.InsertPrinterMaster(txtPrinterName.Text.Trim(), txtPrinterNameL.Text.Trim(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
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
        b = objPrinter.DeletePrinterMaster(editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        foreach (GridViewRow gvrow in GvPrinterBin.Rows)
        {
            index = (int)GvPrinterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in GvPrinterBin.Rows)
            {
                int index = (int)GvPrinterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void GvPrinterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvPrinterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];

        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvPrinter, (DataTable)Session["dtbinFilter"], "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void GvPrinterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objPrinter.GetPrinterMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;

        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvPrinterBin, dt, "", "");
        //AllPageCode();
        lblSelectedRecord.Text = "";
        GvPrinterBin.HeaderRow.Focus();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dtTax = objPrinter.GetPrinterMasterById(editid.Value);

        Lbl_Tab_New.Text = Resources.Attendance.Edit;

        txtPrinterName.Text = dtTax.Rows[0]["Printer_Name"].ToString().Trim();
        txtPrinterNameL.Text = dtTax.Rows[0]["Description"].ToString().Trim();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
    }
    protected void GvPrinter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPrinter.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Religion_Mstr"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvPrinter, dt, "", "");
        //AllPageCode();
        GvPrinter.HeaderRow.Focus();
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
            DataTable dtCurrency = (DataTable)Session["dtReligion"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);

            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)GvPrinter, view.ToTable(), "", "");
            Session["dtFilter_Religion_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvPrinter_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Religion_Mstr"];
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
        Session["dtFilter_Religion_Mstr"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvPrinter, dt, "", "");
        //AllPageCode();
        GvPrinter.HeaderRow.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;


        String CompanyId = Session["CompId"].ToString().ToString();
        String UserId = Session["UserId"].ToString().ToString();
        b = objPrinter.DeletePrinterMaster(editid.Value, "false", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
        //FillGridBin();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListReligion(string prefixText, int count, string contextKey)
    {
        set_PrinterMaster ObjPrinter = new set_PrinterMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjPrinter.GetPrinterMaster(), "Printer_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Printer_Name"].ToString();
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

            DataTable dtCust = (DataTable)Session["dtBinReligion"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)GvPrinterBin, view.ToTable(), "", "");

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
        if (GvPrinterBin.Rows.Count > 0)
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
                            b = objPrinter.DeletePrinterMaster(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
                    foreach (GridViewRow Gvr in GvPrinterBin.Rows)
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
                GvPrinterBin.Focus();
                return;

            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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

                if (!userdetails.Contains(dr["Religion_ID"]))
                {
                    userdetails.Add(dr["Religion_ID"]);
                }
            }
            foreach (GridViewRow GR in GvPrinterBin.Rows)
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

            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)GvPrinterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvPrinterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvPrinterBin.Rows)
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
        //FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void txtPrinterName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objPrinter.GetPrinterMaster();
            dt = new DataView(dt, "Printer_Name='" + txtPrinterName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                txtPrinterName.Text = "";
                DisplayMessage("Printer Name is Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
                return;
            }
            DataTable dt1 = objPrinter.GetPrinterMasterInactive();
            dt1 = new DataView(dt1, "Printer_Name='" + txtPrinterName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtPrinterName.Text = "";
                DisplayMessage("Printer Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
                return;
            }
            txtPrinterNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objPrinter.GetPrinterMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Printer_Name"].ToString() != txtPrinterName.Text)
                {
                    DataTable dt = objPrinter.GetPrinterMaster();
                    dt = new DataView(dt, "Printer_Name='" + txtPrinterName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtPrinterName.Text = "";
                        DisplayMessage("Printer Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
                        return;
                    }
                    DataTable dt1 = objPrinter.GetPrinterMasterInactive();
                    dt1 = new DataView(dt1, "Printer_Name='" + txtPrinterName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtPrinterName.Text = "";
                        DisplayMessage("Printer Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPrinterName);
                        return;
                    }
                }
            }
            txtPrinterNameL.Focus();
        }
    }
    #endregion
}