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

public partial class AccountSetup_NatureOfAccounts : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Ac_Nature_Accounts objNOA = null;
    Ac_Groups objAccGroup = null;
    SystemParameter ObjSysParam = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    Ac_ChartOfAccount objCOA = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;

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
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());

            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "189", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../AccountSetup/NatureOfAccounts.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGridBin();
            FillGrid();
            txtNGroupId.Text = GetDocumentNumber();
            BindTreeView();
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    public string GetDocumentNumber()
    {
        DataTable dtNOA = objNOA.GetNatureAccountsAll();
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(StrCompId, "36", "189");

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
                DocumentNo += "-" + (Convert.ToInt32(dtNOA.Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(dtNOA.Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(dtNOA.Rows.Count) + 1).ToString();
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
        DataTable dtNOAEdit = objNOA.GetNatureAccountsByTransId(e.CommandArgument.ToString());

        if (dtNOAEdit.Rows.Count > 0)
        {
            hdnNOAId.Value = e.CommandArgument.ToString();

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            txtNGroupId.Text = dtNOAEdit.Rows[0]["N_Group_Id"].ToString();
            txtName.Text = dtNOAEdit.Rows[0]["Name"].ToString();
            txtNameL.Text = dtNOAEdit.Rows[0]["Name_L"].ToString();
        }
        btnNew_Click(null, null);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNGroupId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void GvNOA_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvNOA.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Nature_Account"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvNOA, dt, "", "");
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
            objPageCmn.FillData((object)GvNOA, view.ToTable(), "", "");
            Session["dtFilter_Nature_Account"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        txtValue.Focus();
    }
    protected void GvNOA_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Nature_Account"];
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
        Session["dtFilter_Nature_Account"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvNOA, dt, "", "");
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dtNOADelete = objNOA.GetNatureAccountsByTransId(e.CommandArgument.ToString());

        if (dtNOADelete.Rows.Count > 0)
        {

        }
        hdnNOAId.Value = e.CommandArgument.ToString();
        b = objNOA.DeleteNatureAccounts(hdnNOAId.Value, "false", StrUserId, DateTime.Now.ToString());
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
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
    }
    protected void btnNOACancel_Click(object sender, EventArgs e)
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNGroupId);
    }
    protected void btnNOASave_Click(object sender, EventArgs e)
    {
        //Add for RollBack On 26-02-2016
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strCurrencyId = string.Empty;
        string strEmployeeId = string.Empty;

        if (txtNGroupId.Text == "")
        {
            DisplayMessage("Enter Nature Group Id");
            txtNGroupId.Focus();
            return;
        }
        else
        {
            if (hdnNOAId.Value == "0")
            {
                DataTable dtNGroupId = objNOA.GetNatureAccountsAll();
                dtNGroupId = new DataView(dtNGroupId, "N_Group_Id='" + txtNGroupId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtNGroupId.Rows.Count > 0)
                {
                    if (dtNGroupId.Rows[0]["IsActive"].ToString() == "True")
                    {
                        DisplayMessage("Nature Group Id Already Exits");
                        txtNGroupId.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNGroupId);
                        return;
                    }
                    else if (dtNGroupId.Rows[0]["IsActive"].ToString() == "False")
                    {
                        DisplayMessage("Nature Group Id Already Exits Go To Bin Tab and Restore That Same");
                        txtNGroupId.Text = "";
                        return;
                    }
                }
            }
        }

        if (txtName.Text == "")
        {
            DisplayMessage("Fill Name");
            txtName.Focus();
            return;
        }
        else
        {
            if (hdnNOAId.Value == "0")
            {
                if (txtName.Text != "")
                {
                    DataTable dtName = objNOA.GetNatureAccountsAll();
                    dtName = new DataView(dtName, "Name='" + txtName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtName.Rows.Count > 0)
                    {
                        if (dtName.Rows[0]["IsActive"].ToString() == "True")
                        {
                            DisplayMessage("Nature Group Name Already Exits");
                            txtName.Text = "";
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtName);
                            return;
                        }
                        else if (dtName.Rows[0]["IsActive"].ToString() == "False")
                        {
                            DisplayMessage("Nature Group Name Already Exits Go To Bin Tab and Restore That Same");
                            txtName.Text = "";
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtName);
                            return;
                        }
                    }
                }
            }
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (hdnNOAId.Value != "0")
            {
                b = objNOA.UpdateNatureAccounts(hdnNOAId.Value, txtNGroupId.Text, txtName.Text, txtNameL.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {
                    DisplayMessage("Record Updated Successfully !", "green");
                    btnList_Click(null, null);
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
            }
            else
            {
                b = objNOA.InsertNatureAccounts(txtNGroupId.Text, txtName.Text, txtNameL.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {
                    DisplayMessage("Record Saved Successfully !", "green");
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
        hdnNOAId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objNOA.DeleteNatureAccounts(hdnNOAId.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
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
    protected void GvNOABin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvNOABin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvNOABin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvNOABin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvNOABin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvNOABin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvNOABin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsAllFalse();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvNOABin, dt, "", "");
        lblSelectedRecord.Text = "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsAllFalse();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvNOABin, dt, "", "");
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
            objPageCmn.FillData((object)GvNOABin, view.ToTable(), "", "");
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
        DataTable dt = objNOA.GetNatureAccountsAllFalse();

        if (GvNOABin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = objNOA.DeleteNatureAccounts(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                foreach (GridViewRow Gvr in GvNOABin.Rows)
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
        CheckBox chkSelAll = ((CheckBox)GvNOABin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvNOABin.Rows.Count; i++)
        {
            ((CheckBox)GvNOABin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvNOABin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvNOABin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvNOABin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvNOABin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvNOABin.Rows[index].FindControl("chkSelect")).Checked)
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
            for (int i = 0; i < GvNOABin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvNOABin.Rows[i].FindControl("lblgvTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvNOABin.Rows[i].FindControl("chkSelect")).Checked = true;
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
            objPageCmn.FillData((object)GvNOABin, dtUnit1, "", "");
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
                    b = objNOA.DeleteNatureAccounts(lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            foreach (GridViewRow Gvr in GvNOABin.Rows)
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
        DataTable dtBrand = objNOA.GetNatureAccountsAllTrue();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        Session["dtFilter_Nature_Account"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvNOA, dtBrand, "", "");
        }
        else
        {
            GvNOA.DataSource = null;
            GvNOA.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
    }
    # endregion

    #region TreeView

    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewNOA.Visible == true)//To show grid view
        {
            TreeViewNOA.Visible = false;
            GvNOA.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else //To show tree view
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            GvNOA.Visible = false;
            TreeViewNOA.Visible = true;

            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }
    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewNOA.Visible == true)
        {
            TreeViewNOA.Visible = false;
            GvNOA.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            GvNOA.Visible = false;
            TreeViewNOA.Visible = true;
        }
        btnTreeView.Focus();
    }
    protected void TreeViewNOA_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewNOA.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }


    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewNOA.Nodes.Clear();
        DataTable dt = new DataTable();
        dt = objNOA.GetNatureAccountsAllTrue();

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Name"].ToString();
            tn.Value = dt.Rows[i]["Trans_Id"].ToString();
            TreeViewNOA.Nodes.Add(tn);

            FillChild((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn);
            i++;
        }
        TreeViewNOA.DataBind();


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
            tn2.SelectAction = TreeNodeSelectAction.None;
            tn1.ChildNodes.Add(tn2);

            FillChild1((Convert.ToInt32(dt.Rows[i]["Trans_Id"].ToString())), tn2);
            i++;
        }
        TreeViewNOA.DataBind();
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
            tn3.SelectAction = TreeNodeSelectAction.None;
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
        TreeViewNOA.DataBind();
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

    #endregion

    #region display
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
        txtNGroupId.Text = GetDocumentNumber();
        txtName.Text = "";
        txtNameL.Text = "";

        PnlNewContant.Enabled = true;
        hdnNOAId.Value = "0";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    #endregion
}
