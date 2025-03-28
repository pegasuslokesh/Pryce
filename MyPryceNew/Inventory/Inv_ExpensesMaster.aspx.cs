using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_Inv_ExpensesMaster : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Inv_ShipExpMaster ObjShipExpMaster = null;
    SystemParameter ObjSysPeram = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        ObjShipExpMaster = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/Inv_ExpensesMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            ddlOption.SelectedIndex = 2;
            FillGrid();

        }
        //AllPageCode();
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


    #region System defined Function

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = ObjShipExpMaster.GetShipExpMasterById(Session["CompId"].ToString(), editid.Value);

        Lbl_Tab_New.Text = Resources.Attendance.Edit;

        txtExpName.Text = dt.Rows[0]["Exp_Name"].ToString();
        txtLExpName.Text = dt.Rows[0]["Exp_Name_L"].ToString();

        string strAccountId = dt.Rows[0]["Account_No"].ToString();
        DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
        if (dtAccount.Rows.Count > 0)
        {
            txtAccountName.Text = dtAccount.Rows[0]["AccountName"].ToString() + "/" + strAccountId;
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void gvexpenses_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvexpenses.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvexpenses, dt, "", "");
        //AllPageCode();
        gvexpenses.BottomPagerRow.Focus();
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
            DataTable dtShipExp = (DataTable)Session["dtShipExp"];
            DataView view = new DataView(dtShipExp, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvexpenses, view.ToTable(), "", "");

            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void gvexpenses_Sorting(object sender, GridViewSortEventArgs e)
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
        objPageCmn.FillData((object)gvexpenses, dt, "", "");

        //AllPageCode();
        gvexpenses.HeaderRow.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = ObjShipExpMaster.DeleteShipExpMaster(Session["CompId"].ToString(), editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }

        FillGrid();
        Reset();
        try
        {
            int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            ((LinkButton)gvexpenses.Rows[i].FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }
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
    protected void BtnCCancel_Click(object sender, EventArgs e)
    {
        Reset();
        FillGridBin();
        FillGrid();
        txtValue.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtExpName.Focus();
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {


        string strAccountId = string.Empty;
        if (txtExpName.Text == "")
        {
            DisplayMessage("Enter Expenses Name");
            txtExpName.Focus();
            return;
        }

        if (txtAccountName.Text == "")
        {
            DisplayMessage("Fill Account Name");
            txtAccountName.Text = "";
            txtAccountName.Focus();
            return;
        }
        else
        {
            if (GetAccountId() == "")
            {
                DisplayMessage("Please Choose Account Name In Suggestions Only");
                txtAccountName.Text = "";
                txtAccountName.Focus();
                return;
            }
            else
            {

                strAccountId = GetAccountId();
            }
        }

        int b = 0;
        if (editid.Value != "")
        {
            //Code to check whether the new name after edit does not exists
            DataTable dtShipExp = ObjShipExpMaster.GetShipExpMasterByExpName(Session["CompId"].ToString(), txtExpName.Text);
            if (dtShipExp.Rows.Count > 0)
            {
                if (dtShipExp.Rows[0]["Expense_Id"].ToString() != editid.Value)
                {
                    txtExpName.Text = "";
                    DisplayMessage("Expenses Name Already Exists");
                    txtExpName.Focus();
                    return;
                }
            }

            b = ObjShipExpMaster.UpdateShipExpMaster(Session["CompId"].ToString(), editid.Value, txtExpName.Text, txtLExpName.Text, strAccountId, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

            editid.Value = "";
            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            DataTable dtShip = ObjShipExpMaster.GetShipExpMasterByExpName(Session["CompId"].ToString(), txtExpName.Text);
            if (dtShip.Rows.Count > 0)
            {
                txtExpName.Text = "";
                DisplayMessage("Expenses Name Already Exists");
                txtExpName.Focus();
                return;
            }

            b = ObjShipExpMaster.InsertShipExpMaster(Session["CompId"].ToString(), txtExpName.Text, txtLExpName.Text, strAccountId, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved","green");

                Reset();
                FillGrid();
                txtExpName.Focus();
            }
            else
            {
                DisplayMessage("Record  Not Saved");
            }
        }
    }

    protected void txtExpName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dtShip = ObjShipExpMaster.GetShipExpMasterByExpName(Session["CompId"].ToString(), txtExpName.Text);
            if (dtShip.Rows.Count > 0)
            {
                txtExpName.Text = "";
                DisplayMessage("Expenses Name Already Exists");
                txtExpName.Focus();
                return;
            }
            DataTable dt1 = ObjShipExpMaster.GetShipExpMasterInactive(Session["CompId"].ToString());
            dt1 = new DataView(dt1, "Exp_Name='" + txtExpName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtExpName.Text = "";
                DisplayMessage("Expenses Name Already Exists - Go to Bin Tab");
                txtExpName.Focus();
                return;
            }
        }
        else
        {
            DataTable dtTemp = ObjShipExpMaster.GetShipExpMasterById(Session["CompId"].ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Exp_Name"].ToString() != txtExpName.Text)
                {
                    DataTable dt = ObjShipExpMaster.GetShipExpMasterByExpName(Session["CompId"].ToString(), txtExpName.Text);
                    if (dt.Rows.Count > 0)
                    {
                        txtExpName.Text = "";
                        DisplayMessage("Expenses Name Already Exists");
                        txtExpName.Focus();
                        return;
                    }
                    DataTable dt1 = ObjShipExpMaster.GetShipExpMasterInactive(Session["CompId"].ToString());
                    dt1 = new DataView(dt1, "Exp_Name='" + txtExpName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtExpName.Text = "";
                        DisplayMessage("Expenses Name Already Exists - Go to Bin Tab");
                        txtExpName.Focus();
                        return;
                    }
                }
            }
        }
        txtLExpName.Focus();
    }

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {

        FillGridBin();
        txtValueBin.Focus();
    }
    protected void gvexpensesBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvexpensesBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvexpensesBin, dt, "", "");

        //AllPageCode();
        string temp = string.Empty;

        for (int i = 0; i < gvexpensesBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvexpensesBin.Rows[i].FindControl("lblExpId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvexpensesBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        gvexpensesBin.BottomPagerRow.Focus();
    }
    protected void gvexpensesBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjShipExpMaster.GetShipExpMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvexpensesBin, dt, "", "");

        //AllPageCode();
        lblSelectedRecord.Text = "";
        gvexpensesBin.HeaderRow.Focus();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = ObjShipExpMaster.GetShipExpMasterInactive(Session["CompId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvexpensesBin, dt, "", "");

        Session["dtShipExpBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
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

            DataTable dtCust = (DataTable)Session["dtShipExpBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvexpensesBin, view.ToTable(), "", "");

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

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvexpensesBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvexpensesBin.Rows.Count; i++)
        {
            ((CheckBox)gvexpensesBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvexpensesBin.Rows[i].FindControl("lblExpId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvexpensesBin.Rows[i].FindControl("lblExpId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvexpensesBin.Rows[i].FindControl("lblExpId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvexpensesBin.Rows[index].FindControl("lblExpId");
        if (((CheckBox)gvexpensesBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ((CheckBox)gvexpensesBin.Rows[index].FindControl("chkSelect")).Focus();
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Expense_Id"]))
                {
                    lblSelectedRecord.Text += dr["Expense_Id"] + ",";
                }
            }
            for (int i = 0; i < gvexpensesBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvexpensesBin.Rows[i].FindControl("lblExpId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvexpensesBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvexpensesBin, dtUnit1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjShipExpMaster.DeleteShipExpMaster(Session["CompId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            btnRefreshBin_Click(null, null);
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvexpensesBin.Rows)
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

        txtValueBin.Focus();

    }
    #endregion

    #endregion

    #region User defined Function
    private void FillGrid()
    {
        DataTable dtShipExp = ObjShipExpMaster.GetShipExpMaster(Session["CompId"].ToString());
        dtShipExp = new DataView(dtShipExp, "", "Expense_Id desc", DataViewRowState.CurrentRows).ToTable();


        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtShipExp.Rows.Count + "";
        Session["dtShipExp"] = dtShipExp;
        ViewState["dtFilter"] = dtShipExp;
        if (dtShipExp != null && dtShipExp.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvexpenses, dtShipExp, "", "");

        }
        else
        {
            gvexpenses.DataSource = null;
            gvexpenses.DataBind();
        }
        //AllPageCode();
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
    public void Reset()
    {

        txtExpName.Text = "";
        txtLExpName.Text = "";
        txtAccountName.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    #endregion

    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_ShipExpMaster Obj = new Inv_ShipExpMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(Obj.GetShipExpMaster(HttpContext.Current.Session["CompId"].ToString()), "Exp_Name like '%" + prefixText.ToString() + "%'", "Exp_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Exp_Name");

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Exp_Name"].ToString();
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "AccountName asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                txt = null;
            }
            else
            {
                dtCOA = objCOA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dtCOA.Rows.Count > 0)
                {
                    txt = new string[dtCOA.Rows.Count];
                    for (int i = 0; i < dtCOA.Rows.Count; i++)
                    {
                        txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return txt;
    }
    #endregion
    protected void txtAccountName_TextChanged(object sender, EventArgs e)
    {
        if (txtAccountName.Text != "")
        {
            string strTransId = GetAccountId();
            if (strTransId != "")
            {
                DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strTransId);
                if (dtAccount.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnCSave);
                }
                else
                {
                    txtAccountName.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Account Name");
                txtAccountName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountName);
        }
    }
    public string GetAccountNameByAccountId(string strAccountId)
    {
        string strAccountName = string.Empty;
        if (strAccountId != "")
        {
            DataTable dtAccName = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
            if (dtAccName.Rows.Count > 0)
            {
                try
                {
                    strAccountName = dtAccName.Rows[0]["AccountName"].ToString();
                }
                catch
                {

                }
            }
            return strAccountName;
        }
        else
        {
            strAccountName = "";
            return strAccountName;
        }

    }
    private string GetAccountId()
    {
        string retval = string.Empty;

        if (txtAccountName.Text != "")
        {

            retval = (txtAccountName.Text.Split('/'))[txtAccountName.Text.Split('/').Length - 1];
            try
            {
                DataTable dtCOA = objCOA.GetCOAByTransId(Session["CompId"].ToString(), retval);

                if (dtCOA.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            catch
            {
                retval = "";
            }
            return retval;

        }
        else
        {
            retval = "";

        }
        return retval;
    }

}

