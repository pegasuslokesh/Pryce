using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ITSetUp_ModuleMaster : BasePage
{

    Common cmn = null;
    SystemParameter objSys = null;
    ModuleMaster objModule = null;
    IT_ApplicationMaster objApp = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    protected string FuLogo_UploadFolderPath = "~/Images/";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objModule = new ModuleMaster(Session["DBConnection"].ToString());
        objApp = new IT_ApplicationMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ITSetup/ModuleMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            FillGridBin();
            FillGrid();
        }
    }   
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
      
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
   
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //FillGridBin();

        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }


    protected void txtModuleName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objModule.GetModuleMasterByModuleName(txtModuleName.Text);
            if (dt.Rows.Count > 0)
            {
                txtModuleName.Text = "";
                DisplayMessage("Module Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModuleName);
                return;
            }
            DataTable dt1 = objModule.GetModuleMasterInactive();
            dt1 = new DataView(dt1, "Module_Name='" + txtModuleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtModuleName.Text = "";
                DisplayMessage("Module Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModuleName);
                return;
            }
        }
        else
        {
            DataTable dtTemp = objModule.GetModuleMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Module_Name"].ToString() != txtModuleName.Text)
                {
                    DataTable dt = objModule.GetModuleMasterByModuleName(txtModuleName.Text);
                    if (dt.Rows.Count > 0)
                    {
                        txtModuleName.Text = "";
                        DisplayMessage("Module Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModuleName);
                        return;
                    }
                    DataTable dt1 = objModule.GetModuleMasterInactive();
                    dt1 = new DataView(dt1, "Module_Name='" + txtModuleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtModuleName.Text = "";
                        DisplayMessage("Module Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModuleName);
                        return;
                    }
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModuleNameL);
    }
    protected void btnbind_Click(object sender, EventArgs e)
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
            DataTable dtCust = (DataTable)Session["Module"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Module__M"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvModuleMaster, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinModule"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvModuleMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }

    protected void gvModuleMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvModuleMasterBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvModuleMasterBin, dt, "", "");
            //AllPageCode();
        }
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvModuleMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvModuleMasterBin.Rows[i].FindControl("lblModuleId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvModuleMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void gvModuleMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objModule.GetModuleMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvModuleMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;



        if (txtModuleName.Text == "")
        {
            DisplayMessage("Enter Module Name");
            txtModuleName.Focus();
            return;
        }
        //try
        //{
        //    if (ddlApplication.SelectedIndex == 0)
        //    {
        //        DisplayMessage("Select Application");
        //        ddlApplication.Focus();
        //        return;

        //    }
        //}
        //catch
        //{
        //}


        if (txtSquenceNo.Text == "")
        {
            txtSquenceNo.Text = "0";

        }

        if (editid.Value == "")
        {
            if (Session["empimgpath"] == null)
            {
                Session["empimgpath"] = "";

            }

            DataTable dt1 = objModule.GetModuleMaster();

            dt1 = new DataView(dt1, "Module_Name='" + txtModuleName.Text + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Module name already exists");
                txtModuleName.Focus();
                return;

            }


            b = objModule.InsertModuleMaster(txtModuleName.Text, txtModuleNameL.Text.Trim(), txtSquenceNo.Text, Session["empimgpath"].ToString(), Editor1.Content, txtDashBoardUrl.Text, txtIconUrl.Text, "0", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            if (b != 0)
            {
                DisplayMessage("Record Saved","green");
                FillGrid();
                Reset();

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {


            DataTable dt2 = objModule.GetModuleMaster();

            dt2 = new DataView(dt2, "Module_Name='" + txtModuleName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt2.Rows.Count > 0)
            {
                if (dt2.Rows[0]["Module_Id"].ToString() != editid.Value && editid.Value != "")
                {
                    DisplayMessage("Module Name Already Exists");
                    txtModuleName.Focus();
                    return;

                }
            }


            if (Session["empimgpath"] == null)
            {
                Session["empimgpath"] = "";

            }


            b = objModule.UpdateModuleMaster(editid.Value, txtModuleName.Text, txtModuleNameL.Text.Trim().ToString(), txtSquenceNo.Text, Session["empimgpath"].ToString(), Editor1.Content, txtDashBoardUrl.Text, txtIconUrl.Text, hdnParentModuleId.Value, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
               // btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();

                Lbl_New_tab.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }



    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();


        DataTable dt = objModule.GetModuleMasterById(editid.Value);
        if (dt.Rows.Count > 0)
        {

            hdnParentModuleId.Value = dt.Rows[0]["Field1"].ToString();

            txtModuleName.Text = dt.Rows[0]["Module_Name"].ToString();
            txtModuleNameL.Text = dt.Rows[0]["Module_Name_L"].ToString();

            txtSquenceNo.Text = dt.Rows[0]["Sequence_No"].ToString();

            txtDashBoardUrl.Text = dt.Rows[0]["DashBoard_Url"].ToString();

            //try
            //{
            //    ddlApplication.SelectedValue = dt.Rows[0]["Application_Id"].ToString();
            //}
            //catch
            //{

            //}

            txtIconUrl.Text = dt.Rows[0]["DashBoardIconUrl"].ToString();
            Editor1.Content = dt.Rows[0]["Module_Banner"].ToString();


            imgLogo.ImageUrl = "~/Images/" + "/" + dt.Rows[0]["Module_Image"].ToString();



            //btnNew_Click(null, null);
            Lbl_New_tab.Text = Resources.Attendance.Edit.ToString();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_View_tab_position()", true);

            //ddlApplication.Enabled = false;

        }



    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objModule.DeleteModuleMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");

            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvModuleMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModuleMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Module__M"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvModuleMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvModuleMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Module__M"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Module__M"] = dt;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvModuleMaster, dt, "", "");
        //AllPageCode();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModuleName(string prefixText, int count, string contextKey)
    {
        ModuleMaster objModuleMaster = new ModuleMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objModuleMaster.GetModuleMaster(), "Module_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        dt = dt.DefaultView.ToTable(true, "Module_Name");

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Module_Name"].ToString();
        }
        return txt;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //FillGrid();
        //FillGridBin();
        Reset();
        //btnList_Click(null, null);
        Lbl_New_tab.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void FillGrid()
    {
        DataTable dt = objModule.GetModuleMaster();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvModuleMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Module__M"] = dt;
        Session["Module"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objModule.GetModuleMasterInactive();

        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvModuleMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinModule"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
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


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvModuleMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvModuleMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvModuleMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvModuleMasterBin.Rows[i].FindControl("lblModuleId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvModuleMasterBin.Rows[i].FindControl("lblModuleId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvModuleMasterBin.Rows[i].FindControl("lblModuleId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvModuleMasterBin.Rows[index].FindControl("lblModuleId");
        if (((CheckBox)gvModuleMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Module_Id"]))
                {
                    lblSelectedRecord.Text += dr["Module_Id"] + ",";
                }
            }
            for (int i = 0; i < gvModuleMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvModuleMasterBin.Rows[i].FindControl("lblModuleId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvModuleMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvModuleMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objModule.DeleteModuleMaster(lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

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
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvModuleMasterBin.Rows)
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
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        imgLogo.ImageUrl = "~/Images/" + FULogoPath.FileName;
    }
    public void Reset()
    {



        txtModuleName.Text = "";
        txtModuleNameL.Text = "";
        Editor1.Content = "";
        imgLogo.ImageUrl = "";
        txtIconUrl.Text = "";
        //try
        //{
        //    ddlApplication.SelectedIndex = 0;
        //}
        //catch
        //{
        //}
        Session["empimgpath"] = null;
        txtSquenceNo.Text = "";
        txtDashBoardUrl.Text = "";




        Lbl_New_tab.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;

        //ddlApplication.Enabled = true;

    }

    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                string path = Server.MapPath("~/Images/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                Session["empimgpath"] = FULogoPath.FileName;
            }
        }
    }

}
