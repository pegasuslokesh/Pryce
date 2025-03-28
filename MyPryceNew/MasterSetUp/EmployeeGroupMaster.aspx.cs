using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_EmployeeGroupMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    EmployeeMaster objEmp = null;
    Set_Group_Employee objGroupEmp = null;
    LocationMaster ObjLocationMaster = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //AllPageCode();

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/EmployeeGroupMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            ddlOption.SelectedIndex = 2;
            FillGridBin();
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtGroupName.Focus();
            txtbinVal.Text = "";
            FillddlLocation();
            FillddlDeaprtment();
            //FillddlGroup_Emp_Department();
        }
    }


    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmp.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvEmp, (DataTable)Session["dtEmp"], "", "");

        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvEmp.Rows.Count; i++)
        {
            Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
            string[] split = lblEmp.Text.Split(',');

            for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
            {
                if (lblEmp.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
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
        dt = objEmpGroup.GetEmployeeGroup_MasterInactive(Session["CompId"].ToString().ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvGroupBin, dt, "", "");

        Session["dtBinGroup"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;

        }
        else
        {
            //AllPageCode();
        }

    }
    private void FillGrid()
    {
        DataTable dtBrand = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString().ToString());

        dtBrand = new DataView(dtBrand, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //// Modified By Nitin Jain On 25-09-2014
        if (Session["RoleId"] != null)
        {
            dtBrand = new DataView(dtBrand, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        ////---------------------
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtGroup"] = dtBrand;
        Session["dtFilter_Empgroup_Mstr"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvGroup, dtBrand, "", "");
            //AllPageCode();
        }
        else
        {
            GvGroup.DataSource = null;
            GvGroup.DataBind();
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

        txtGroupName.Text = "";
        txtGroupNameL.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        lblEmp.Text = "";

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtGroupNameL.Text = "";
        rbtnAllEmp.Checked = false;
        rbtnEmpInGroup.Checked = false;
        rbtnEmpNotInGroup.Checked = false;
        Session["dtEmp"] = null;
        txtbinVal.Text = "";
    }
    #endregion


    #region System Defined Funcation


    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        //AllPageCode();
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {

        if (txtGroupName.Text == "" || txtGroupName.Text == null)
        {
            DisplayMessage("Enter Group Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
            return;
        }
        int b = 0;


        string strRoleId = Session["RoleId"].ToString();

        if (Session["RoleId"].ToString().Contains(','))
        {
            strRoleId = Session["RoleId"].ToString().Split(',')[0].ToString();
        }


        if (editid.Value != "")
        {

            DataTable dtCate = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString().ToString());
            dtCate = new DataView(dtCate, "Group_Name='" + txtGroupName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Group_ID"].ToString() != editid.Value)
                {
                    txtGroupName.Text = "";
                    DisplayMessage("Group Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
                    return;
                }
            }


            b = objEmpGroup.UpdateEmployeeGroup_Master(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, "0", strRoleId, txtGroupName.Text, txtGroupNameL.Text.Trim().ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
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
            DataTable dtPro = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString().ToString());
            dtPro = new DataView(dtPro, "Group_Name='" + txtGroupName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtGroupName.Text = "";
                DisplayMessage("Group Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
                return;
            }

            b = objEmpGroup.InsertEmployeeGroup_Master(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtGroupName.Text, txtGroupNameL.Text.Trim(), "0", strRoleId, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        b = objEmpGroup.DeleteEmployeeGroup_Master(Session["CompId"].ToString().ToString(), editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        foreach (GridViewRow gvrow in GvGroupBin.Rows)
        {
            index = (int)GvGroupBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in GvGroupBin.Rows)
            {
                int index = (int)GvGroupBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void GvGroupBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvGroupBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvGroupBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void GvGroupBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objEmpGroup.GetEmployeeGroup_MasterInactive(Session["CompId"].ToString().ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvGroupBin, dt, "", "");
        //AllPageCode();
        lblSelectedRecord.Text = "";
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        txtGroupNameG.Text = "";
        txtGroupNameLG.Text = "";

        editid.Value = e.CommandArgument.ToString();

        DataTable dtTax = objEmpGroup.GetEmployeeGroup_MasterById(Session["CompId"].ToString().ToString(), editid.Value);

        // Lbl_Tab_New.Text = Resources.Attendance.Edit;

        txtGroupNameG.Text = dtTax.Rows[0]["Group_Name"].ToString();
        txtGroupNameLG.Text = dtTax.Rows[0]["Group_Name_L"].ToString();
        //rbtnAllEmp.Checked = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Show()", true);

        //pnl2.Visible = true;
        rbtnAllEmp.Checked = true;
        EmpGroup_CheckedChanged(null, null);


        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
    }




    protected void GvGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvGroup.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Empgroup_Mstr"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvGroup, dt, "", "");
        //AllPageCode();
    }
    protected void chkgvSelect_CheckedChanged1(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmp.Rows[index].FindControl("lblEmpId");
        if (((CheckBox)gvEmp.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";

            if (!lblEmp.Text.Split(',').Contains(lb.Text))
            {
                lblEmp.Text += empidlist;
            }
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblEmp.Text += empidlist;
            string[] split = lblEmp.Text.Split(',');
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
            lblEmp.Text = temp;
        }
    }

    protected void chkgvSelectAll_CheckedChanged1(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmp.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvEmp.Rows.Count; i++)
        {
            ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblEmp.Text.Split(',').Contains(((Label)(gvEmp.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
                {
                    lblEmp.Text += ((Label)(gvEmp.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblEmp.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvEmp.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblEmp.Text = temp;
            }
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
            DataTable dtCurrency = (DataTable)Session["dtGroup"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvGroup, view.ToTable(), "", "");
            Session["dtFilter_Empgroup_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvGroup_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Empgroup_Mstr"];
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
        Session["dtFilter_Empgroup_Mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvGroup, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        String UserId = Session["UserId"].ToString().ToString();
        b = objEmpGroup.DeleteEmployeeGroup_Master(Session["CompId"].ToString().ToString(), editid.Value, "false", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListGroup(string prefixText, int count, string contextKey)
    {
        Set_EmployeeGroup_Master objGroup = new Set_EmployeeGroup_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objGroup.GetEmployeeGroup_Master(HttpContext.Current.Session["CompId"].ToString()), "Group_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Group_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListGroupG(string prefixText, int count, string contextKey)
    {
        Set_EmployeeGroup_Master objGroup = new Set_EmployeeGroup_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objGroup.GetEmployeeGroup_Master(HttpContext.Current.Session["CompId"].ToString()), "Group_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Group_Name"].ToString();
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

            DataTable dtCust = (DataTable)Session["dtBinGroup"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvGroupBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
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
        if (GvGroupBin.Rows.Count > 0)
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
                            b = objEmpGroup.DeleteEmployeeGroup_Master(Session["CompId"].ToString().ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
                    foreach (GridViewRow Gvr in GvGroupBin.Rows)
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
                GvGroupBin.Focus();
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

                if (!userdetails.Contains(dr["Group_ID"]))
                {
                    userdetails.Add(dr["Group_ID"]);
                }
            }
            foreach (GridViewRow GR in GvGroupBin.Rows)
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
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvGroupBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvGroupBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvGroupBin.Rows)
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
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void txtGroupName_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).ID != "txtGroupNameG")
        {
            editid.Value = "";
        }

        if (editid.Value == "")
        {
            DataTable dt = objEmpGroup.GetEmployeeGroup_MasterByGroupName(Session["CompId"].ToString().ToString(), txtGroupName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtGroupName.Text = "";
                DisplayMessage("Group Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
                return;
            }
            DataTable dt1 = objEmpGroup.GetEmployeeGroup_MasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Group_Name='" + txtGroupName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtGroupName.Text = "";
                DisplayMessage("Group Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
                return;
            }
            txtGroupNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objEmpGroup.GetEmployeeGroup_MasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Group_Name"].ToString() != txtGroupName.Text)
                {
                    DataTable dt = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Group_Name='" + txtGroupName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtGroupName.Text = "";
                        DisplayMessage("Group Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
                        return;
                    }
                    DataTable dt1 = objEmpGroup.GetEmployeeGroup_MasterInactive(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Group_Name='" + txtGroupName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtGroupName.Text = "";
                        DisplayMessage("Group Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupName);
                        return;
                    }
                }
            }
            txtGroupNameL.Focus();
        }

    }

    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinVal.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinVal.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinVal.Text.Trim() + "%'";
            }

            DataTable dt = (DataTable)Session["dtEmp"];

            DataView view = new DataView(dt, condition, "", DataViewRowState.CurrentRows);

            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";

            dt = view.ToTable();

            if (dt.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)gvEmp, dt, "", "");

                DataTable dtEmpInGroup = objGroupEmp.GetGroup_Employee(editid.Value, Session["CompId"].ToString());
                if (dtEmpInGroup.Rows.Count > 0)
                {
                    string EmpIds = string.Empty;

                    for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";

                    }
                    for (int i = 0; i < gvEmp.Rows.Count; i++)
                    {
                        Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
                        string[] split = lblEmp.Text.Split(',');

                        for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
                        {
                            if (lblEmp.Text.Split(',')[j] != "")
                            {
                                if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                                {
                                    ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                                }
                            }
                        }
                    }

                }
            }
            else
            {
                gvEmp.DataSource = null;
                gvEmp.DataBind();
            }
        }


    }


    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        txtbinVal.Text = "";
        ddlbinFieldName.SelectedIndex = 1;
        ddlbinOption.SelectedIndex = 2;



        EmpGroup_CheckedChanged(null, null);
    }




    protected void ImgbtnSelectAll_Click1(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (rbtnAllEmp.Checked == true)
        {
            DataTable dtGroup = (DataTable)Session["dtEmp"];

            if (ViewState["Select"] == null)
            {
                ViewState["Select"] = 1;
                foreach (DataRow dr in dtGroup.Rows)
                {
                    if (!lblEmp.Text.Split(',').Contains(dr["Emp_Id"]))
                    {
                        lblEmp.Text += dr["Emp_Id"] + ",";
                    }
                }
                for (int i = 0; i < gvEmp.Rows.Count; i++)
                {
                    string[] split = lblEmp.Text.Split(',');
                    Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
                    for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
                    {
                        if (lblEmp.Text.Split(',')[j] != "")
                        {
                            if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                            {
                                ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                            }
                        }
                    }
                }
            }
            else
            {
                lblEmp.Text = "";
                DataTable dtGroup1 = (DataTable)Session["dtEmp"];
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)gvEmp, dtGroup1, "", "");
                ViewState["Select"] = null;
            }
        }
    }
    protected void btnSaveGroup_Click(object sender, EventArgs e)
    {
        int b = 0;

        DataTable dtCate = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString().ToString());
        dtCate = new DataView(dtCate, "Group_Name='" + txtGroupNameG.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCate.Rows.Count > 0)
        {
            if (dtCate.Rows[0]["Group_ID"].ToString() != editid.Value)
            {
                txtGroupNameG.Text = "";
                DisplayMessage("Group Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGroupNameG);
                return;
            }
        }

        string strRoleId = Session["RoleId"].ToString();

        if (Session["RoleId"].ToString().Contains(','))
        {
            strRoleId = Session["RoleId"].ToString().Split(',')[0].ToString();
        }


        b = objEmpGroup.UpdateEmployeeGroup_Master(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, "0", strRoleId, txtGroupNameG.Text, txtGroupNameLG.Text.Trim().ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {

            //objGroupEmp.DeleteGroup_Employee(Session["CompId"].ToString(), editid.Value, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            foreach (string str in lblEmp.Text.Split(','))
            {
                if (str != "")
                {
                    b = objGroupEmp.InsertGroup_Employee(Session["CompId"].ToString(), str, editid.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }






            DisplayMessage("Record Updated", "green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
        }
        else
        {
            DisplayMessage("Record  Not Updated");
        }


    }
    protected void btnCancelGroup_Click(object sender, EventArgs e)
    {




        //pnl2.Visible = false;
        txtGroupNameG.Text = "";
        txtGroupNameLG.Text = "";
        rbtnAllEmp.Checked = false;
        rbtnEmpInGroup.Checked = false;
        rbtnEmpNotInGroup.Checked = false;
        Reset();
        FillGrid();
        txtbinVal.Text = "";
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

        //pnl2.Visible = false;
        txtGroupNameG.Text = "";
        txtGroupNameLG.Text = "";
        txtbinVal.Text = "";
    }



    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        //txtGroupNameG.Text = "";
        //txtGroupNameLG.Text = "";
        lblEmp.Text = "";
        if (rbtnAllEmp.Checked)
        {
            imgbtnSelectAll1.Visible = true;
            dpLocation_SelectedIndexChanged(null, null);

        }
        else if (rbtnEmpInGroup.Checked)
        {
            imgbtnSelectAll1.Visible = false;
            dpLocation_SelectedIndexChanged(null, null);

        }
        else if (rbtnEmpNotInGroup.Checked)
        {
            imgbtnSelectAll1.Visible = false;
            dpLocation_SelectedIndexChanged(null, null);

        }
        txtbinVal.Text = "";
        //dpLocation_SelectedIndexChanged(null, null);
    }
    protected void ImgClose_Click(object sender, ImageClickEventArgs e)
    {
        btnCancelGroup_Click(sender, e);
    }
    #endregion

    // code added by Ghanshyam Suthar on 16/08/2017
    protected void IbtnDelete_Command1(object sender, CommandEventArgs e)
    {
        string strDeptId = string.Empty;
        foreach (ListItem li in listEmpDept.Items)
        {
            if (li.Selected)
            {
                strDeptId += li.Value.ToString() + ",";
            }
        }

        int b = 0;
        b = objGroupEmp.DeleteGroup_EmployeeByEmpId(e.CommandArgument.ToString(), editid.Value, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            rbtnEmpInGroup.Checked = true;
            EmpGroup_CheckedChanged(null, null);
            lblEmp.Text = "";
        }
        for (int j = 0; j < strDeptId.Split(',').Length; j++)
        {
            if (strDeptId.Split(',')[j] != "")
            {
                string Split_S = strDeptId.Split(',')[j].Trim().ToString();
                if (listEmpDept.Items.FindByValue(Split_S) != null)
                    listEmpDept.Items.FindByValue(Split_S).Selected = true;
            }
        }
        listEmpDept_SelectedIndexChanged(null, null);
    }
    protected void listEmpDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        Fill_Grid_Department();
    }
    protected void Fill_Grid_Department()
    {

        gvEmp.DataSource = null;
        gvEmp.DataBind();
        if (rbtnAllEmp.Checked == true)
        {
            string strStatus = "False";
            foreach (ListItem li in listEmpDept.Items)
            {
                if (li.Selected)
                {
                    strStatus = "True";
                }
            }

            DataTable dt = new DataTable();
            try
            {
                if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
                {

                    dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


                    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    //}
                    if (strStatus == "True")
                    {
                        string strDeptId = string.Empty;
                        foreach (ListItem li in listEmpDept.Items)
                        {
                            if (li.Selected)
                            {
                                strDeptId += li.Value.ToString() + ",";
                            }
                        }
                        try
                        {
                            dt = new DataView(dt, "Department_Id in(" + strDeptId.Substring(0, strDeptId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                            Session["gv_emp_sorting_dt"] = dt;
                        }
                        catch
                        {

                        }


                        if (dt.Rows.Count > 0)
                        {
                            //Common Function add By Lokesh on 22-05-2015
                            objPageCmn.FillData((object)gvEmp, dt, "", "");
                            Session["dtEmp"] = dt;
                            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                            gvEmp.Columns[0].Visible = true;
                            gvEmp.Columns[1].Visible = false;
                        }
                        else
                        {
                            gvEmp.DataSource = null;
                            gvEmp.DataBind();
                            Session["dtEmp"] = null;
                            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                        }
                    }
                }
                for (int i = 0; i < gvEmp.Rows.Count; i++)
                {
                    Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
                    string[] split = lblEmp.Text.Split(',');

                    for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
                    {
                        if (lblEmp.Text.Split(',')[j] != "")
                        {
                            if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                            {
                                ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //DisplayMessage("Select Company");
            }
        }

        else if (rbtnEmpInGroup.Checked == true)
        {
            string strStatus = "False";
            foreach (ListItem li in listEmpDept.Items)
            {
                if (li.Selected)
                {
                    strStatus = "True";
                }
            }

            DataTable dt = new DataTable();
            try
            {
                if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
                {

                    dt = objEmp.Get_Group_EmployeeOrDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, "0");
                    //dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                    ////if (ddlLocation.SelectedIndex == 0)
                    ////{
                    ////    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    ////}
                    ////else
                    ////{
                    //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    //}
                    if (strStatus == "True")
                    {
                        string strDeptId = string.Empty;
                        foreach (ListItem li in listEmpDept.Items)
                        {
                            if (li.Selected)
                            {
                                strDeptId += li.Value.ToString() + ",";
                            }
                        }
                        try
                        {
                            dt = new DataView(dt, "group_id=" + editid.Value + " and  Department_Id in(" + strDeptId.Substring(0, strDeptId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                            Session["gv_emp_sorting_dt"] = dt;
                        }
                        catch
                        {

                        }


                        if (dt.Rows.Count > 0)
                        {
                            //Common Function add By Lokesh on 22-05-2015
                            objPageCmn.FillData((object)gvEmp, dt, "", "");
                            Session["dtEmp"] = dt;
                            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                            gvEmp.Columns[0].Visible = false;
                            gvEmp.Columns[1].Visible = true;
                        }
                        else
                        {
                            gvEmp.DataSource = null;
                            gvEmp.DataBind();
                            Session["dtEmp"] = null;
                            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                        }
                    }
                }
                for (int i = 0; i < gvEmp.Rows.Count; i++)
                {
                    Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
                    string[] split = lblEmp.Text.Split(',');

                    for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
                    {
                        if (lblEmp.Text.Split(',')[j] != "")
                        {
                            if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                            {
                                ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //DisplayMessage("Select Company");

            }
        }
        else if (rbtnEmpNotInGroup.Checked == true)
        {
            string strStatus = "False";
            foreach (ListItem li in listEmpDept.Items)
            {
                if (li.Selected)
                {
                    strStatus = "True";
                }
            }

            DataTable dt = new DataTable();
            try
            {
                if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
                {

                    //dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                    ////if (ddlLocation.SelectedIndex == 0)
                    ////{
                    ////    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    ////}
                    ////else
                    ////{
                    //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    dt = objEmp.Get_Group_EmployeeOrDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, "0");

                    //}
                    if (strStatus == "True")
                    {
                        string strDeptId = string.Empty;
                        foreach (ListItem li in listEmpDept.Items)
                        {
                            if (li.Selected)
                            {
                                strDeptId += li.Value.ToString() + ",";
                            }
                        }
                        try
                        {
                            dt = new DataView(dt, "group_id is null and  Department_Id in(" + strDeptId.Substring(0, strDeptId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                            Session["gv_emp_sorting_dt"] = dt;
                        }
                        catch
                        {

                        }


                        if (dt.Rows.Count > 0)
                        {
                            //Common Function add By Lokesh on 22-05-2015
                            objPageCmn.FillData((object)gvEmp, dt, "", "");
                            Session["dtEmp"] = dt;
                            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                            gvEmp.Columns[0].Visible = false;
                            gvEmp.Columns[1].Visible = false;
                        }
                        else
                        {
                            gvEmp.DataSource = null;
                            gvEmp.DataBind();
                            Session["dtEmp"] = null;
                            lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                        }
                    }
                }
                for (int i = 0; i < gvEmp.Rows.Count; i++)
                {
                    Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
                    string[] split = lblEmp.Text.Split(',');

                    for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
                    {
                        if (lblEmp.Text.Split(',')[j] != "")
                        {
                            if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                            {
                                ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                //DisplayMessage("Select Company");

            }
        }
    }

    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (rbtnAllEmp.Checked == true)
        {
            //if (ddlLocation.SelectedIndex == 0)
            //{
            //    listEmpDept.Items.Clear();
            //    listEmpDept.DataSource = null;
            //    listEmpDept.DataBind();
            //    Fill_Grid_Department();
            //}
            //else
            //{
            DataTable dt_depart = new DataView((DataTable)Session["Location_Emp_Group"], "Location_Id='" + ddlLocation.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)listEmpDept, dt_depart, "DeptName", "Dep_Id");
            foreach (ListItem li in listEmpDept.Items)
            {
                li.Selected = true;
            }
            Fill_Grid_Department();
            //}
        }
        else
        {
            //if (ddlLocation.SelectedIndex == 0)
            //{
            //    listEmpDept.Items.Clear();
            //    listEmpDept.DataSource = null;
            //    listEmpDept.DataBind();
            //    Fill_Grid_Department();
            //}
            //else
            //{
            if (editid.Value != "")
            {
                DataTable dt = objEmp.Get_Group_EmployeeOrDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, "0");
                DataTable dt_depart = new DataView(dt, "Group_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable distinctValues = new DataView(dt_depart).ToTable(true, "DeptName", "Dep_Id");
                objPageCmn.FillData((object)listEmpDept, distinctValues, "DeptName", "Dep_Id");

                foreach (ListItem li in listEmpDept.Items)
                {
                    li.Selected = true;
                }
                Fill_Grid_Department();
            }
            //}
        }
    }
    //protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    //{
    //    foreach (ListItem li in listEmpDept.Items)
    //    {
    //        if (li.Selected)
    //        {
    //            li.Selected = false;
    //        }
    //    }
    //    ddlLocation.SelectedIndex = 0;
    //    listEmpDept.SelectedIndex = -1;
    //    EmpGroup_CheckedChanged(null, null);
    //}
    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            //Common Function add By Lokesh on 22-05-2015

            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();

            //objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }
    private void FillddlDeaprtment()
    {
        //Bind Department According to location on 18-04-2015
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        string FLocIds = string.Empty;

        for (int i = 0; i < dtLoc.Rows.Count; i++)
        {
            FLocIds += dtLoc.Rows[i]["Location_Id"].ToString() + ",";
        }
        //End New Code



        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        dt = new DataView(dt, "Location_Id in(" + FLocIds.Substring(0, FLocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }

        if (dt.Rows.Count > 0)
        {
            dt = dt.DefaultView.ToTable();
            Session["Location_Emp_Group"] = dt;
            //Common Function add By Lokesh on 22-05-2015
            //objPageCmn.FillData((object)listEmpDept, dt, "DeptName", "Dep_Id");
        }
        else
        {
            try
            {
                listEmpDept.DataSource = null;
                listEmpDept.DataBind();
            }
            catch
            {

            }
        }
    }
    //private void FillddlGroup_Emp_Department()
    //{
    //    DataTable dtLoc = objEmp.Get_Group_EmployeeOrDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),"0");
    //    Session["Group_Depart_Master"] = dtLoc;
    //}

    protected void gvEmp_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["gv_emp_sorting_dt"];
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
        Session["gv_emp_sorting_dt"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
        //AllPageCode();
    }
}
