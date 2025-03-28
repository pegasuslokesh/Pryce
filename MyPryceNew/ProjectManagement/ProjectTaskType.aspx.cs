using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProjectManagement_ProjectTaskType : System.Web.UI.Page
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    Prj_Project_TaskType ObjTaskType = null;
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
        ObjTaskType = new Prj_Project_TaskType(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ProjectManagement/ProjectTaskType.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtTaskTypeName.Focus();
            Session["CHECKED_ITEMS"] = null;
            ddlOption.SelectedIndex = 2;
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtTaskTypeName.Focus();
        }
        txtTaskTypeName.Focus();
        //AllPageCode();
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
        dt = ObjTaskType.GetAllFalserecord();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaskTypeBin, dt, "", "");
        Session["dtBinDesignation"] = dt;
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
    private void FillGrid()
    {
        DataTable dtDesignation = ObjTaskType.GetAllTrueRecord();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtDesignation.Rows.Count + "";
        Session["dtDesignation"] = dtDesignation;
        Session["dtFilter_Project_Task_type"] = dtDesignation;
        if (dtDesignation != null && dtDesignation.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvTaskType, dtDesignation, "", "");
            //AllPageCode();
        }
        else
        {
            GvTaskType.DataSource = null;
            GvTaskType.DataBind();
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
        txtTaskTypeName.Text = "";
        txttaskTypeLocalName.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txttaskTypeLocalName.Text = "";
        chkIsBug.Checked = false;
    }
    #endregion


    #region System Defined Funcation

    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtValueBin.Focus();
        FillGridBin();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtTaskTypeName.Text.Trim() == "" || txtTaskTypeName.Text.Trim() == null)
        {
            DisplayMessage("Enter Task Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {

            DataTable dtCate = ObjTaskType.GetAllRecord();
            dtCate = new DataView(dtCate, "TaskType_Name='" + txtTaskTypeName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Trans_Id"].ToString() != editid.Value)
                {
                    txtTaskTypeName.Text = "";
                    DisplayMessage("Task Type Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
                    return;
                }
            }


            b = ObjTaskType.UpdateRecord(editid.Value, txtTaskTypeName.Text.Trim(), txttaskTypeLocalName.Text.Trim().ToString(), chkIsBug.Checked.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            DataTable dtPro = ObjTaskType.GetAllRecord();
            dtPro = new DataView(dtPro, "TaskType_Name='" + txtTaskTypeName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtTaskTypeName.Text = "";
                DisplayMessage("Task Type Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
                return;
            }

            b = ObjTaskType.Insertrecord(txtTaskTypeName.Text.Trim(), txttaskTypeLocalName.Text.Trim(), chkIsBug.Checked.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        b = ObjTaskType.DeleteRecord(editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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

        foreach (GridViewRow gvrow in GvTaskTypeBin.Rows)
        {
            index = (int)GvTaskTypeBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in GvTaskTypeBin.Rows)
            {
                int index = (int)GvTaskTypeBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void GvTaskTypeBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvTaskTypeBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaskTypeBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }

    protected void GvTaskTypeBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjTaskType.GetAllFalserecord();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaskTypeBin, dt, "", "");
        //AllPageCode();
        lblSelectedRecord.Text = "";
    }


    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtEdit = ObjTaskType.GetRecordByTransId(editid.Value);
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        txtTaskTypeName.Text = dtEdit.Rows[0]["TaskType_Name"].ToString();
        txttaskTypeLocalName.Text = dtEdit.Rows[0]["TaskType_Local_Name"].ToString();
        chkIsBug.Checked = Convert.ToBoolean(dtEdit.Rows[0]["Is_Bug"].ToString());
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
    }
    protected void GvTaskType_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTaskType.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Project_Task_type"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaskType, dt, "", "");
        //AllPageCode();
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
            objPageCmn.FillData((object)GvTaskType, view.ToTable(), "", "");
            Session["dtFilter_Project_Task_type"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvTaskType_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Project_Task_type"];
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
        Session["dtFilter_Project_Task_type"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvTaskType, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        DataTable dt = new DataTable();

        dt = cmn.GetCheckEsistenceId(e.CommandArgument.ToString(), "8");


        if (dt.Rows.Count > 0)
        {
            DisplayMessage(" You Can Not Delete  This Record Is Currently Used");
            return;
        }


        dt.Dispose();

        editid.Value = e.CommandArgument.ToString();
        String UserId = Session["UserId"].ToString();
        b = ObjTaskType.DeleteRecord(editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
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
        txtTaskTypeName.Focus();
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
            objPageCmn.FillData((object)GvTaskTypeBin, view.ToTable(), "", "");
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
        if (GvTaskTypeBin.Rows.Count > 0)
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
                            b = ObjTaskType.DeleteRecord(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
                    foreach (GridViewRow Gvr in GvTaskTypeBin.Rows)
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
                GvTaskTypeBin.Focus();
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

                if (!userdetails.Contains(dr["Trans_Id"]))
                {
                    userdetails.Add(dr["Trans_Id"]);
                }
            }
            foreach (GridViewRow GR in GvTaskTypeBin.Rows)
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
            objPageCmn.FillData((object)GvTaskTypeBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvTaskTypeBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvTaskTypeBin.Rows)
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
    protected void txtTaskTypeName_TextChanged(object sender, EventArgs e)
    {
        if (txtTaskTypeName.Text.Trim() != "")
        {

            DataTable dt = ObjTaskType.GetAllTrueRecord();


            if (editid.Value == "")
            {
                dt = new DataView(dt, "TaskType_Name='" + txtTaskTypeName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dt = new DataView(dt, "TaskType_Name='" + txtTaskTypeName.Text.Trim() + "' and Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    txtTaskTypeName.Text = "";
                    DisplayMessage("Task Type Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
                    return;
                }
                else
                {
                    txtTaskTypeName.Text = "";
                    DisplayMessage("Task Type Already Exists - Go to Bin Tab");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaskTypeName);
                    return;
                }


            }


        }





    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTaskType(string prefixText, int count, string contextKey)
    {
        Prj_Project_TaskType ObjTask = new Prj_Project_TaskType(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjTask.GetAllTrueRecord(), "TaskType_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["TaskType_Name"].ToString();
        }
        return txt;
    }
    #endregion
}