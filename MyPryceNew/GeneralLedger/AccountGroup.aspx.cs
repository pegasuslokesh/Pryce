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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;


public partial class GeneralLedger_AccountGroup : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Ac_Nature_Accounts objNOA = null;
    Ac_Groups objAccGroup = null;
    Ac_ChartOfAccount objCOA = null;
    SystemParameter ObjSysParam = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = "admin";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        cmn = new Common(Session["DBConnection"].ToString());
        objNOA = new Ac_Nature_Accounts(Session["DBConnection"].ToString());
        objAccGroup = new Ac_Groups(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../GeneralLedger/AccountGroup.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            //FillParentGroup();
            FillGridBin();
            FillGrid();
            txtAccGroupId.Text = GetDocumentNumber();
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnAccGroupSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    public string GetDocumentNumber()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(StrCompId, "36", "192");

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += StrCompId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += StrBrandId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += strLocationId;
            }

            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["SessionDepId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {
                DataTable Dtuser = ObjUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }

            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(objAccGroup.GetGroupsAll(StrCompId.ToString(), StrBrandId).Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(objAccGroup.GetGroupsAll(StrCompId.ToString(), StrBrandId).Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(objAccGroup.GetGroupsAll(StrCompId.ToString(), StrBrandId).Rows.Count) + 1).ToString();
        }
        return DocumentNo;
    }


    #region System defined Function
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dtAccGroupEdit = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, e.CommandArgument.ToString());

        if (dtAccGroupEdit.Rows.Count > 0)
        {
            hdnAccGroupId.Value = e.CommandArgument.ToString();

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtAccGroupId.Text = dtAccGroupEdit.Rows[0]["Ac_Group_Id"].ToString();
            txtGroupName.Text = dtAccGroupEdit.Rows[0]["Ac_GroupName"].ToString();
            txtGroupNameL.Text = dtAccGroupEdit.Rows[0]["Ac_GroupNameL"].ToString();

            string strNGroupId = dtAccGroupEdit.Rows[0]["N_Group_ID"].ToString();
            if (strNGroupId != "" && strNGroupId != "0")
            {
                txtNatureOfAccount.Text = GetNatureOfAccount(strNGroupId) + "/" + strNGroupId;
            }
            else
            {
                txtNatureOfAccount.Text = "";
            }

            string strParentId = dtAccGroupEdit.Rows[0]["Parant_Ac_Group_Id"].ToString();
            if (strParentId != "" && strParentId != "0")
            {
                txtParentAccGroup.Text = GetAccountGroupName(strParentId) + "/" + strParentId;
            }
            else
            {
                txtParentAccGroup.Text = "";
            }

            txtSystem.Text = dtAccGroupEdit.Rows[0]["System"].ToString();
        }
        btnNew_Click(null, null);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccGroupId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected string GetAccountGroupName(string strAccountGroupId)
    {
        string strAccountGroupName = string.Empty;
        if (strAccountGroupId != "0" && strAccountGroupId != "")
        {
            DataTable dtAccGroup = objAccGroup.GetGroupsByAcGroupId(StrCompId, StrBrandId, strAccountGroupId);
            if (dtAccGroup.Rows.Count > 0)
            {
                strAccountGroupName = dtAccGroup.Rows[0]["Ac_GroupName"].ToString();
            }
        }
        else
        {
            strAccountGroupName = "";
        }
        return strAccountGroupName;
    }
    protected void GvAccGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvAccGroup.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DT_Filter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAccGroup, dt, "", "");
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

            DataTable dtAdd = (DataTable)Session["dtCurrency"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAccGroup, view.ToTable(), "", "");
            Session["DT_Filter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        txtValue.Focus();
    }
    protected void GvAccGroup_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["DT_Filter"];
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
        Session["DT_Filter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAccGroup, dt, "", "");
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dtAccLedgerDelete = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, e.CommandArgument.ToString());

        if (dtAccLedgerDelete.Rows.Count > 0)
        {

        }
        hdnAccGroupId.Value = e.CommandArgument.ToString();
        b = objAccGroup.DeleteGroups(StrCompId, StrBrandId, hdnAccGroupId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin(); //Update grid view in bin tab
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
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
    }
    protected void btnAccGroupCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        FillGridBin();
        FillGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccGroupId);
    }
    protected void btnAccGroupSave_Click(object sender, EventArgs e)
    {
        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strNatureId = string.Empty;
        string strParentId = string.Empty;

        if (txtAccGroupId.Text == "")
        {
            DisplayMessage("Enter Account Group Id");
            txtAccGroupId.Focus();
            return;
        }
        else
        {
            if (hdnAccGroupId.Value == "0")
            {
                DataTable dtVoucherNo = objAccGroup.GetGroupsByAcGroupId(StrCompId, StrBrandId, txtAccGroupId.Text);
                if (dtVoucherNo.Rows.Count > 0)
                {
                    DisplayMessage("Account Group Id Already Exits");
                    txtAccGroupId.Text = "";
                    return;
                }
            }
        }

        if (txtGroupName.Text == "")
        {
            DisplayMessage("Fill Group Name");
            txtGroupName.Focus();
            return;
        }

        if (txtNatureOfAccount.Text != "")
        {
            strNatureId = GetNatureId(txtNatureOfAccount.Text);
            if (strNatureId != "" && strNatureId != "0")
            {

            }
            else
            {
                DisplayMessage("Choose in Suggestion Only");
                txtNatureOfAccount.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Fill Nature Of Account");
            txtNatureOfAccount.Focus();
            return;
        }

        if (txtParentAccGroup.Text == "")
        {
            strParentId = "0";
        }
        else if (txtParentAccGroup.Text != "")
        {
            strParentId = txtParentAccGroup.Text.Split('/')[1].ToString();
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            int b = 0;
            if (hdnAccGroupId.Value != "0")
            {
                b = objAccGroup.UpdateGroups(StrCompId, StrBrandId, hdnAccGroupId.Value, txtAccGroupId.Text, txtGroupName.Text, txtGroupNameL.Text, strNatureId, strParentId, txtSystem.Text, "0", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    btnList_Click(null, null);
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
                b = objAccGroup.InsertGroups(StrCompId, StrBrandId, txtAccGroupId.Text, txtGroupName.Text, txtGroupNameL.Text, strNatureId, strParentId, txtSystem.Text, "0", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    DisplayMessage("Record Saved", "green");
                }
                else
                {
                    DisplayMessage("Record  Not Saved");
                }
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            BindTreeView();
            FillGrid();
            Reset();
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Replace("'", " ") + " Line Number : " + Common.GetLineNumber(ex));
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
        hdnAccGroupId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objAccGroup.DeleteGroups(StrCompId, StrBrandId, hdnAccGroupId.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
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

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }
    protected void GvAccGroupBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvAccGroupBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAccGroupBin, dt, "", "");

        string temp = string.Empty;
        for (int i = 0; i < GvAccGroupBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvAccGroupBin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvAccGroupBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvAccGroupBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objAccGroup.GetGroupsAllFalse(StrCompId, StrBrandId);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAccGroupBin, dt, "", "");
        lblSelectedRecord.Text = "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objAccGroup.GetGroupsAllFalse(StrCompId, StrBrandId);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvAccGroupBin, dt, "", "");
        Session["dtPBrandBin"] = dt;
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
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = true;
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


            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAccGroupBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objAccGroup.GetGroupsAllFalse(StrCompId, StrBrandId);

        if (GvAccGroupBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objAccGroup.DeleteGroups(StrCompId, StrBrandId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvAccGroupBin.Rows)
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvAccGroupBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvAccGroupBin.Rows.Count; i++)
        {
            ((CheckBox)GvAccGroupBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvAccGroupBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvAccGroupBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvAccGroupBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvAccGroupBin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvAccGroupBin.Rows[index].FindControl("chkSelect")).Checked)
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
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvAccGroupBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvAccGroupBin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvAccGroupBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAccGroupBin, dtUnit1, "", "");
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
                    b = objAccGroup.DeleteGroups(StrCompId, StrBrandId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvAccGroupBin.Rows)
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
    #endregion

    #endregion

    #region User defined Function
    private void FillGrid()
    {
        DataTable dtBrand = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        Session["DT_Filter"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAccGroup, dtBrand, "", "");
        }
        else
        {
            GvAccGroup.DataSource = null;
            GvAccGroup.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
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
        FillGrid();
        txtAccGroupId.Text = GetDocumentNumber();
        txtGroupName.Text = "";
        txtGroupNameL.Text = "";
        txtNatureOfAccount.Text = "";
        txtParentAccGroup.Text = "";
        navTreeAccontGroup.Visible = false;
        txtSystem.Text = "";
        PnlNewContant.Enabled = true;
        hdnAccGroupId.Value = "0";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    protected string GetNatureOfAccount(string strNatureId)
    {
        string strName = string.Empty;
        if (strNatureId != "0" && strNatureId != "")
        {
            DataTable dtNature = objNOA.GetNatureAccountsByTransId(strNatureId);
            if (dtNature.Rows.Count > 0)
            {
                strName = dtNature.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strName = "";
        }
        return strName;
    }
    protected string GetAccountPGroupName(string strGroupId)
    {
        string strPGroupName = string.Empty;
        if (strGroupId != "0" && strGroupId != "")
        {
            DataTable dtAcGroup = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, strGroupId);
            if (dtAcGroup.Rows.Count > 0)
            {
                strPGroupName = dtAcGroup.Rows[0]["Ac_GroupName"].ToString();
            }
        }
        else
        {
            strPGroupName = "";
        }
        return strPGroupName;
    }
    #endregion

    #region
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListNatureOfAccount(string prefixText, int count, string contextKey)
    {
        Ac_Nature_Accounts objNature = new Ac_Nature_Accounts(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objNature.GetNatureAccountsAllTrue();

        dt = new DataView(dt, "Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
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
                dt = objNature.GetNatureAccountsAllTrue();
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    protected void txtNatureOfAccount_TextChanged(object sender, EventArgs e)
    {
        string strTransId = string.Empty;
        if (txtNatureOfAccount.Text != "")
        {
            strTransId = GetNatureId(txtNatureOfAccount.Text);
            if (strTransId != "" && strTransId != "0")
            {
                txtParentAccGroup.Text = "";
            }
            else
            {
                DisplayMessage("Select Nature of Account In Suggestions Only");
                txtNatureOfAccount.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNatureOfAccount);
            }
        }
    }
    private string GetNatureId(string strNatureGroupId)
    {
        string retval = string.Empty;
        if (strNatureGroupId != "")
        {
            DataTable dtNatureOfAccount = objNOA.GetNatureAccountsByName(strNatureGroupId.Split('/')[0].ToString());
            if (dtNatureOfAccount.Rows.Count > 0)
            {
                retval = (strNatureGroupId.Split('/'))[strNatureGroupId.Split('/').Length - 1];

                DataTable dtNature = objNOA.GetNatureAccountsByTransId(retval);
                if (dtNature.Rows.Count > 0)
                {
                    FillParentGroup(Convert.ToInt32(retval));
                    BindTreeView_AcGroup(retval);
                    navTreeAccontGroup.Visible = true;
                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    private void FillParentGroup(int retval)
    {
        DataTable dsParentGroup = null;
        dsParentGroup = objAccGroup.GetAccountsGroupByNOA(StrCompId, StrBrandId, retval);
        if (dsParentGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            /////////cmn.FillData((object)ddlParentAccGroup, dsParentGroup, "Ac_GroupName", "Trans_Id");
            //updated by jitendra upadhyay on 04/02/2014
            //replace the add select text with insert select text at 0 index for resolve the error in the click on tree node)
        }
        else
        {
            ////ddlParentAccGroup.Items.Insert(0, "--Select--");
            ////ddlParentAccGroup.SelectedIndex = 0;
        }
    }
    #endregion

    #region TreeView
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewAccGroup.Visible == true)//To show grid view
        {
            TreeViewAccGroup.Visible = false;
            GvAccGroup.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else //To show tree view
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            GvAccGroup.Visible = false;
            TreeViewAccGroup.Visible = true;
            //trdel2.Visible = false;
            //trdel.Visible = false;
            //trgv.Visible = false;
            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }
    protected void btnTreeView_Click(object sender, ImageClickEventArgs e)
    {
        if (TreeViewAccGroup.Visible == true)
        {
            TreeViewAccGroup.Visible = false;
            GvAccGroup.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            GvAccGroup.Visible = false;
            TreeViewAccGroup.Visible = true;
            //trdel2.Visible = false;
            //trdel.Visible = false;
            //trgv.Visible = false;
        }
        btnTreeView.Focus();
    }
    protected void TreeViewCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewAccGroup.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }
    //private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    //{
    //    TreeViewAccGroup.Nodes.Clear();
    //    DataTable dt = new DataTable();
    //    string x = "Parant_Ac_Group_Id=" + "'0'" + "";
    //    dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId, strLocationId);
    //    dt = new DataView(dt, x, "", DataViewRowState.OriginalRows).ToTable();
    //    int i = 0;
    //    while (i < dt.Rows.Count)
    //    {
    //        TreeNode tn = new TreeNode();
    //        tn.Text = dt.Rows[i]["Ac_GroupName"].ToString();
    //        tn.Value = dt.Rows[i]["Trans_Id"].ToString();
    //        TreeViewAccGroup.Nodes.Add(tn);
    //        FillChild((dt.Rows[i]["Trans_Id"].ToString()), tn);
    //        i++;
    //    }
    //    TreeViewAccGroup.DataBind();
    //}
    //private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    //{
    //    DataTable dt = new DataTable();
    //    dt = GetProductCategoryByParentId(index);

    //    int i = 0;
    //    while (i < dt.Rows.Count)
    //    {
    //        TreeNode tn1 = new TreeNode();
    //        tn1.Text = dt.Rows[i]["Ac_GroupName"].ToString();
    //        tn1.Value = dt.Rows[i]["Trans_Id"].ToString();
    //        tn.ChildNodes.Add(tn1);
    //        FillChild((dt.Rows[i]["Trans_Id"].ToString()), tn1);
    //        i++;
    //    }
    //    TreeViewAccGroup.DataBind();
    //}
    //public DataTable GetProductCategoryByParentId(string ParentId) //Function to get entries of same ProductId
    //{
    //    //dt = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, strLocationId, "0");
    //    string query = "Parant_Ac_Group_Id='" + ParentId + "'";
    //    DataTable dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId, strLocationId);
    //    dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
    //    return dt;
    //}

    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewAccGroup.Nodes.Clear();
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsAllTrue();

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Name"].ToString();
            tn.Value = dt.Rows[i]["Trans_Id"].ToString();
            tn.SelectAction = TreeNodeSelectAction.None;
            TreeViewAccGroup.Nodes.Add(tn);

            FillChild((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn);
            i++;
        }
        TreeViewAccGroup.DataBind();

        TreeViewAccGroup.CollapseAll();
    }

    private void FillChild(int s, TreeNode tn1)
    {

        DataTable dt = new DataTable();
        string x = "N_Group_ID=" + s + " and Parant_Ac_Group_Id=" + "'0'" + "";
        dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        dt = new DataView(dt, x, "", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn2 = new TreeNode();
            tn2.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn2.Value = dt.Rows[i]["Trans_Id"].ToString();
            //tn2.SelectAction = TreeNodeSelectAction.None;
            tn1.ChildNodes.Add(tn2);

            FillChild1((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn2);
            i++;
        }
        TreeViewAccGroup.DataBind();
    }
    private void FillChild1(int s, TreeNode tn2)
    {
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        dt = GetAccountGroupByParentId(s);

        int i = 0;
        int j = 0;

        while (i < dt.Rows.Count)
        {
            TreeNode tn3 = new TreeNode();
            tn3.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn3.Value = dt.Rows[i]["Trans_Id"].ToString();
            //tn3.SelectAction = TreeNodeSelectAction.None;
            tn2.ChildNodes.Add(tn3);

            dt2 = GetAllCOA(Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString()), tn3);
            while (j < dt2.Rows.Count)
            {
                TreeNode tn4 = new TreeNode();
                tn4.Text = dt2.Rows[j]["AccountName"].ToString();
                tn4.Value = dt2.Rows[j]["Trans_Id"].ToString();
                tn4.SelectAction = TreeNodeSelectAction.None;
                tn3.ChildNodes.Add(tn4);

                j++;
            }

            FillChild1((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn3);
            i++;
        }
        TreeViewAccGroup.DataBind();
    }
    public DataTable GetAccountGroupByParentId(int ParentId) //Function to get entries of same ProductId
    {
        //dt = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, strLocationId, "0");
        string query = "Parant_Ac_Group_Id='" + ParentId + "'";
        DataTable dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
        return dt;
    }
    public DataTable GetAllCOA(int s, TreeNode tn)
    {
        string query = "Acc_Group_Id='" + s + "'";
        DataTable dt = objCOA.GetCOAAllTrue(StrCompId);
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
        return dt;
    }


    #region AccountGroup_TreeView

    protected void navTreeAccontGroup_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)navTreeAccontGroup.SelectedValue.ToString());
        DataTable dtAccGroupEdit = objAccGroup.GetGroupsByTransId(StrCompId, StrBrandId, navTreeAccontGroup.SelectedValue.ToString());
        {
            txtParentAccGroup.Text = dtAccGroupEdit.Rows[0]["Ac_GroupName"].ToString() + "/" + dtAccGroupEdit.Rows[0]["Ac_Group_Id"].ToString();
            navTreeAccontGroup.Visible = false;
        }
        txtParentAccGroup_TextChanged(sender, e);
    }
    private void BindTreeView_AcGroup(string strNOAId)//fucntion to fill up TreeView according to parent child nodes
    {
        navTreeAccontGroup.Nodes.Clear();
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsByTransId(strNOAId);

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Name"].ToString();
            tn.Value = dt.Rows[i]["Trans_Id"].ToString();
            tn.SelectAction = TreeNodeSelectAction.None;
            navTreeAccontGroup.Nodes.Add(tn);

            FillChildACGroup((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn);
            i++;
        }
        navTreeAccontGroup.DataBind();

        navTreeAccontGroup.CollapseAll();
    }
    private void FillChildACGroup(int s, TreeNode tn1)
    {
        DataTable dt = new DataTable();
        string x = "N_Group_ID=" + s + " and Parant_Ac_Group_Id=" + "'0'" + "";
        dt = objAccGroup.GetGroupsAllTrue(StrCompId, StrBrandId);
        dt = new DataView(dt, x, "", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn2 = new TreeNode();
            tn2.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn2.Value = dt.Rows[i]["Trans_Id"].ToString();
            //tn2.SelectAction = TreeNodeSelectAction.Select;
            tn1.ChildNodes.Add(tn2);

            FillChild1_AcGroup((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn2);
            i++;
        }
        navTreeAccontGroup.DataBind();
    }
    private void FillChild1_AcGroup(int s, TreeNode tn2)
    {
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        dt = GetAccountGroupByParentId(s);

        int i = 0;
        int j = 0;

        while (i < dt.Rows.Count)
        {
            TreeNode tn3 = new TreeNode();
            tn3.Text = dt.Rows[i]["Ac_GroupName"].ToString();
            tn3.Value = dt.Rows[i]["Trans_Id"].ToString();
            //tn3.SelectAction = TreeNodeSelectAction.Select;
            tn2.ChildNodes.Add(tn3);

            //dt2 = GetAllCOA(Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString()), tn3);
            //while (j < dt2.Rows.Count)
            //{
            //    TreeNode tn4 = new TreeNode();
            //    tn4.Text = dt2.Rows[j]["AccountName"].ToString();
            //    tn4.Value = dt2.Rows[j]["Trans_Id"].ToString();
            //    //tn4.SelectAction = TreeNodeSelectAction.None;
            //    tn3.ChildNodes.Add(tn4);

            //    j++;
            //}

            FillChild1((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn3);
            i++;
        }
        navTreeAccontGroup.DataBind();
    }
    protected void txtParentAccGroup_TextChanged(object sender, EventArgs e)
    {
        //string strAddValue = "1";
        string strAccountNo = string.Empty;
        string strAccGroupId = string.Empty;

        if (txtParentAccGroup.Text != "")
        {
            strAccGroupId = GetAccGroupId(txtParentAccGroup.Text);
            if (strAccGroupId != "" && strAccGroupId != "0")
            {
                DataTable dtNOA = objAccGroup.GetGroupsByAcGroupId(StrCompId, StrBrandId, strAccGroupId);
                if (dtNOA.Rows.Count > 0)
                {
                    string strNOAId = dtNOA.Rows[0]["N_Group_ID"].ToString();
                    if (strNOAId != "0" && strNOAId != "")
                    {
                        DataTable dtNatureOfAccount = objNOA.GetNatureAccountsByTransId(strNOAId);
                        if (dtNatureOfAccount.Rows.Count > 0)
                        {
                            txtNatureOfAccount.Text = dtNatureOfAccount.Rows[0]["Name"].ToString() + "/" + dtNatureOfAccount.Rows[0]["Trans_Id"].ToString();
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group In Suggestions Only");
                txtParentAccGroup.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtParentAccGroup);
            }
        }
    }
    private string GetAccGroupId(string strAccountGroupName)
    {
        string retval = string.Empty;
        if (strAccountGroupName != "")
        {
            DataTable dtGroup = objAccGroup.GetGroupsByGroupName(StrCompId, StrBrandId, strAccountGroupName.Split('/')[0].ToString());
            if (dtGroup.Rows.Count > 0)
            {
                retval = (strAccountGroupName.Split('/'))[strAccountGroupName.Split('/').Length - 1];

                DataTable dtGroupById = objAccGroup.GetGroupsByAcGroupId(StrCompId, StrBrandId, retval);
                if (dtGroupById.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    #endregion
    #endregion

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccGroupName(string prefixText, int count, string contextKey)
    {
        Ac_Groups ObjAccountGroup = new Ac_Groups(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjAccountGroup.GetGroupsAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());

        DataTable dt = new DataView(dt1, "Ac_GroupName like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Ac_GroupName"].ToString() + "/" + dt.Rows[i]["Ac_Group_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txt[i] = dt1.Rows[i]["Ac_GroupName"].ToString() + "/" + dt1.Rows[i]["Ac_Group_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }
}
