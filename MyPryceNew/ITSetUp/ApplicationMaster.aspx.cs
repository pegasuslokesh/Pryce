using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ITSetUp_ApplicationMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    IT_ApplicationMaster objDesg = null;
    ObjectMaster objObject = null;
    IT_ModuleMaster objModule = null;
    ModuleMaster objModule1 = null;
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
        objDesg = new IT_ApplicationMaster(Session["DBConnection"].ToString());
        objObject = new ObjectMaster(Session["DBConnection"].ToString());
        objModule = new IT_ModuleMaster(Session["DBConnection"].ToString());
        objModule1 = new ModuleMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ITSetUp/ApplicationMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            ddlOption.SelectedIndex = 2;
            btnNew_Click(null, null);
            //pnlApplication.Visible = false;

            Div_Tree.Style.Add("display", "none");
            Div_New.Style.Add("display", "");

            FillGridBin();
            FillGrid();
            ddlFieldName.SelectedIndex = 0;
            txtApplicationName.Focus();
        }
        else
        {


            try
            {
                if (navTree.SelectedNode.Checked == true)
                {
                    UnSelectChild(navTree.SelectedNode);
                }
                else
                {
                    SelectChild(navTree.SelectedNode);
                }
            }
            catch (Exception)
            {

            }



            //try
            //{
            //    if (navTree.SelectedNode.Checked == true)
            //    {
            //        UnSelectChild(navTree.SelectedNode);
            //    }
            //    else
            //    {
            //        SelectChild(navTree.SelectedNode);
            //    }
            //}
            //catch (Exception)
            //{

            //}
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


    //protected void navTree_OnSelectedNodeChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (navTree.SelectedNode.Checked == true)
    //        {
    //            UnSelectChild(navTree.SelectedNode);
    //        }
    //        else
    //        {
    //            SelectChild(navTree.SelectedNode);
    //        }
    //    }
    //    catch (Exception)
    //    {

    //    }
    //}
    //protected void navTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    //{

    //    if (e != null && e.Node != null)
    //    {
    //        try
    //        {
    //            if (e.Node.Checked == true)
    //            {
    //                try
    //                {
    //                    e.Node.Parent.Checked = true;
    //                }
    //                catch
    //                {

    //                }
    //                try
    //                {
    //                    e.Node.Parent.Parent.Checked = true;
    //                }
    //                catch
    //                {
    //                }

    //                CheckTreeNodeRecursive(e.Node, true);
    //            }
    //            else
    //            {
    //                CheckTreeNodeRecursive(e.Node, false);

    //            }
    //        }
    //        catch (Exception)
    //        {

    //        }


    //    }


    //}
    private void UnSelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = false;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = false;
            UnSelectChild(treeNode.ChildNodes[i]);
            i++;
        }

        navTree.DataBind();
    }

    private void SelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = true;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = true;
            SelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        try
        {
            treeNode.Parent.Checked = true;
            treeNode.Parent.Parent.Checked = true;
        }
        catch { }

        navTree.DataBind();

    }




    protected void btnResetApp_Click(object sender, EventArgs e)
    {
        chkSelectAll.Checked = false;
        ChkSelectAll_OnCheckedChanged(null, null);
    }

    protected void btnCancelApp_Click(object sender, EventArgs e)
    {
        FillGrid();
        Reset();
        btnNew_Click(null, null);
        //pnlApplication.Visible = false;
        Div_Tree.Style.Add("display", "none");
        Div_New.Style.Add("display", "");
        navTree.DataSource = null;
        navTree.DataBind();
    }

    protected void navTree_SelectedNodeChanged(object sender, EventArgs e)
    {

    }
    protected void ChkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        BindTree();
        if (chkSelectAll.Checked)
        {
            foreach (TreeNode Tn in navTree.Nodes)
            {
                SelectChild(Tn);



            }

        }
        else
        {
            foreach (TreeNode Tn in navTree.Nodes)
            {
                UnSelectChild(Tn);

            }

        }
        try
        {
            if (chkSelectAll.Checked == false)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch
        {

        }

    }

    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {








        foreach (TreeNode child in parent.ChildNodes)
        {


            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;

            }



            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }

        }


    }

    public void BindTree()
    {
        string AppId = string.Empty;
        DataTable dtApp = objSys.GetSysParameterByParamName("Application_Id");
        if (dtApp.Rows.Count > 0)
        {
            AppId = dtApp.Rows[0]["Param_Value"].ToString().Trim();

        }
        else
        {
            return;
        }

        AppId = "0";

        string RoleId = string.Empty;
        string moduleids = string.Empty;


        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();

        DataTable dtRoleP = new DataTable();
        dtRoleP = objModule1.GetModuleObjectByApplicatonId(editid.Value);

        if (dtRoleP.Rows.Count > 0)
        {


        }


        DataTable dtModule = objModule.GetModuleMaster();

        DataTable DtModuleApp = new DataTable();

        DtModuleApp = objModule1.GetModuleObjectByApplicatonId(AppId);


        for (int i = 0; i < DtModuleApp.Rows.Count; i++)
        {
            moduleids += DtModuleApp.Rows[i]["Module_Id"].ToString() + ",";
        }

        dtModule = new DataView(dtModule, "Module_Id in (" + moduleids.Substring(0, moduleids.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        DataTable DtOpType = objObject.GetOpType();

        foreach (DataRow datarow in dtModule.Rows)
        {

            TreeNode tn = new TreeNode();


            tn = new TreeNode(datarow["Module_Name"].ToString(), datarow["Module_Id"].ToString());

            DataTable dtModuleSaved = new DataTable();
            if (dtRoleP.Rows.Count > 0)
            {
                dtModuleSaved = new DataView(dtRoleP, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (dtModuleSaved.Rows.Count > 0)
            {
                tn.Checked = true;

            }



            DataTable dtAllChild = objObject.GetObjectMasterByModuleId_ApplicationId(datarow["Module_Id"].ToString(), AppId);




            dtAllChild = new DataView(dtAllChild, "", "Order_Appear", DataViewRowState.CurrentRows).ToTable();
            foreach (DataRow childrow in dtAllChild.Rows)
            {
                string GetUrl = string.Empty;
                GetUrl = childrow[0].ToString();



                TreeNode tnChild = new TreeNode(childrow[1].ToString(), GetUrl);
                DataTable dtObj = new DataTable();

                if (dtRoleP.Rows.Count > 0)
                {
                    dtObj = new DataView(dtRoleP, "Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                if (dtObj.Rows.Count > 0)
                {
                    tnChild.Checked = true;
                }
                tn.ChildNodes.Add(tnChild);


            }

            navTree.Nodes.Add(tn);
        }




        navTree.DataBind();
        navTree.CollapseAll();


        return;







    }

    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = objDesg.GetApplicationMasterInactive();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvApplicationBin, dt, "", "");
        Session["dtBinApplication"] = dt;
        Session["dtBinFilter"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    private void FillGrid()
    {
        DataTable dtBrand = objDesg.GetApplicationMaster();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtApplication"] = dtBrand;
        Session["dtFilter_Ap_Mast"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvApplication, dtBrand, "", "");
            //AllPageCode();
        }
        else
        {
            GvApplication.DataSource = null;
            GvApplication.DataBind();
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

        txtApplicationName.Text = "";
        txtApplicationNameL.Text = "";
        editid.Value = "";
        Lbl_New_tab.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtApplicationNameL.Text = "";
    }
    #endregion


    #region System Defined Funcation

    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");


        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlList.Visible = true;

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_Bin_Hide_tab()", true);

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlList.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_Bin_tab_position()", true);

        FillGridBin();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
    }
    protected void btnSaveApp_Click(object sender, EventArgs e)
    {

        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
        {

        }


        int b = 0;

        IT_ApplicationMaster objApp = new IT_ApplicationMaster(Session["DBConnection"].ToString());
        objApp.DeleteApplicationModuleObjectByModuleId(editid.Value);
        DataTable dtModule = objModule1.GetModuleObjectByApplicatonId("0");
        foreach (TreeNode ModuleNode in navTree.Nodes)
        {
            //here save one row for module
            if (ModuleNode.Checked)
            {
                DataTable dt = new DataView(dtModule, "Module_Id='" + ModuleNode.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                    //     b=objModule.InsertModuleMaster(ModuleNode.Text, ModuleNode.Text, dt.Rows[0]["Sequence_No"].ToString(), dt.Rows[0]["Module_Image"].ToString(), dt.Rows[0]["Module_Banner"].ToString(), editid.Value, dt.Rows[0]["DashBoard_Url"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }

                else
                {
                    return;

                }

                foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
                {
                    if (ObjNode.Checked)
                    {




                        b = objObject.InsertObjectModuleMaster(editid.Value, ModuleNode.Value, ObjNode.Value);



                    }

                }




            }

        }


        if (b != 0)
        {
            DisplayMessage("Record Updated", "green");
            FillGrid();
            Reset();
            btnNew_Click(null, null);
            //pnlApplication.Visible = false;
            Div_Tree.Style.Add("display", "none");
            Div_New.Style.Add("display", "");
            navTree.DataSource = null;
            navTree.DataBind();
        }
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {

        if (txtApplicationName.Text == "" || txtApplicationName.Text == null)
        {
            DisplayMessage("Enter Application Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {

            DataTable dtCate = objDesg.GetApplicationMaster();
            dtCate = new DataView(dtCate, "Application_Name='" + txtApplicationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {
                if (dtCate.Rows[0]["Application_ID"].ToString() != editid.Value)
                {
                    txtApplicationName.Text = "";
                    DisplayMessage("Application Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
                    return;
                }
            }


            b = objDesg.UpdateApplicationMaster(editid.Value, txtApplicationName.Text, txtApplicationNameL.Text.Trim().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {

                FillGrid();
                DisplayMessage("Record Updated", "green");
                pnlList.Visible = false;
                //pnlApplication.Visible = true;
                Div_Tree.Style.Add("display", "");
                Div_New.Style.Add("display", "none");
                BindTree();
                lblAppId.Text = editid.Value;
                lblAppName.Text = txtApplicationName.Text;

            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objDesg.GetApplicationMaster();
            dtPro = new DataView(dtPro, "Application_Name='" + txtApplicationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtApplicationName.Text = "";
                DisplayMessage("Application Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
                return;
            }

            b = objDesg.InsertApplicationMaster(txtApplicationName.Text, txtApplicationNameL.Text.Trim(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                editid.Value = b.ToString();
                DisplayMessage("Record Saved","green");

                FillGrid();

                pnlList.Visible = false;
                Div_Tree.Style.Add("display", "");
                Div_New.Style.Add("display", "none");

                BindTree();

                lblAppId.Text = editid.Value;
                lblAppName.Text = txtApplicationName.Text;

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
        b = objDesg.DeleteApplicationMaster(editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
    protected void GvApplicationBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvApplicationBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvApplicationBin, dt, "", "");

        //AllPageCode();
        string temp = string.Empty;

        for (int i = 0; i < GvApplicationBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvApplicationBin.Rows[i].FindControl("lblApplicationId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvApplicationBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void GvApplicationBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objDesg.GetApplicationMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvApplicationBin, dt, "", "");

        //AllPageCode();
        lblSelectedRecord.Text = "";
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dtTax = objDesg.GetApplicationMasterById(editid.Value);

        Lbl_New_tab.Text = Resources.Attendance.Edit.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_View_tab_position()", true);

        txtApplicationName.Text = dtTax.Rows[0]["Application_Name"].ToString();
        txtApplicationNameL.Text = dtTax.Rows[0]["Application_Name_L"].ToString();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);


    }
    protected void GvApplication_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvApplication.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Ap_Mast"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvApplication, dt, "", "");
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
            DataTable dtCurrency = (DataTable)Session["dtApplication"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvApplication, view.ToTable(), "", "");
            Session["dtFilter_Ap_Mast"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void GvApplication_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Ap_Mast"];
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
        Session["dtFilter_Ap_Mast"] = dt;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvApplication, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        String UserId = Session["UserId"].ToString().ToString();
        b = objDesg.DeleteApplicationMaster(editid.Value, "false", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
        //FillGridBin();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListApplication(string prefixText, int count, string contextKey)
    {
        IT_ApplicationMaster objApplication = new IT_ApplicationMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objApplication.GetApplicationMaster(), "Application_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Application_Name"].ToString();
        }
        return txt;
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
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

            DataTable dtCust = (DataTable)Session["dtBinApplication"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvApplicationBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
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
        int Msg = 0;
        DataTable dt = objDesg.GetApplicationMasterInactive();

        if (GvApplicationBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {

                        Msg = objDesg.DeleteApplicationMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (Msg != 0)
            {
                FillGrid();
                FillGridBin();
                ViewState["Select"] = null;
                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");

            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvApplicationBin.Rows)
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtApplication = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtApplication.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Application_ID"]))
                {
                    lblSelectedRecord.Text += dr["Application_ID"] + ",";
                }
            }
            for (int i = 0; i < GvApplicationBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvApplicationBin.Rows[i].FindControl("lblApplicationId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvApplicationBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtApplication1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvApplicationBin, dtApplication1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvApplicationBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < GvApplicationBin.Rows.Count; i++)
        {
            ((CheckBox)GvApplicationBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvApplicationBin.Rows[i].FindControl("lblApplicationId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvApplicationBin.Rows[i].FindControl("lblApplicationId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvApplicationBin.Rows[i].FindControl("lblApplicationId"))).Text.Trim().ToString())
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
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvApplicationBin.Rows[index].FindControl("lblApplicationId");
        if (((CheckBox)GvApplicationBin.Rows[index].FindControl("chkgvSelect")).Checked)
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

        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void txtApplicationName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objDesg.GetApplicationMasterByApplicationName(txtApplicationName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtApplicationName.Text = "";
                DisplayMessage("Application Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
                return;
            }
            DataTable dt1 = objDesg.GetApplicationMasterInactive();
            dt1 = new DataView(dt1, "Application_Name='" + txtApplicationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtApplicationName.Text = "";
                DisplayMessage("Application Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
                return;
            }
            txtApplicationNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objDesg.GetApplicationMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Application_Name"].ToString() != txtApplicationName.Text)
                {
                    DataTable dt = objDesg.GetApplicationMaster();
                    dt = new DataView(dt, "Application_Name='" + txtApplicationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtApplicationName.Text = "";
                        DisplayMessage("Application Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
                        return;
                    }
                    DataTable dt1 = objDesg.GetApplicationMasterInactive();
                    dt1 = new DataView(dt1, "Application_Name='" + txtApplicationName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtApplicationName.Text = "";
                        DisplayMessage("Application Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApplicationName);
                        return;
                    }
                }
            }
            txtApplicationNameL.Focus();
        }

    }
    #endregion



}
