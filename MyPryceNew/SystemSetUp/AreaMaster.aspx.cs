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
using System.IO;

public partial class SystemSetUp_AreaMaster : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    Sys_AreaMaster objAreamaster = null;
    PageControlCommon objPageCmn = null;

    IT_ObjectEntry objObjectEntry = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAreamaster = new Sys_AreaMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetUp/AreaMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGridBin();
            FillGrid();

            btnList_Click(null, null);
        }
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        txtValue.Focus();
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlList.Visible = true;
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = false;
    }
    protected void Lbl_Tab_New_Click(object sender, EventArgs e)
    {

        txtAreaName.Focus();
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlList.Visible = false;
        //PnlNewEdit.Visible = true;
        //PnlBin.Visible = false;

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = true;
        //PnlList.Visible = false;
        FillGridBin();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
    }

    protected void txtAreaName_TextChanged(object sender, EventArgs e)
    {

        if (txtAreaName.Text.Trim() != "")
        {
            if (editid.Value == "")
            {

                DataTable dt = objAreamaster.GetAreaMaster();

                dt = new DataView(dt, "Area_Name='" + txtAreaName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {

                    DisplayMessage("Area Name is Already Exists");
                    txtAreaName.Text = "";
                    txtAreaName.Focus();
                }
                DataTable dt1 = objAreamaster.GetAreaMasterInactive();
                dt1 = new DataView(dt1, "Area_Name='" + txtAreaName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {

                    DisplayMessage("Area Name Already Exists in Bin Section");
                    txtAreaName.Text = "";
                    txtAreaName.Focus();
                }
            }
            else
            {
                DataTable dtTemp = objAreamaster.GetAreaMasterById(editid.Value);
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Area_Name"].ToString() != txtAreaName.Text)
                    {
                        DataTable dt = objAreamaster.GetAreaMaster();
                        dt = new DataView(dt, "Area_Name='" + txtAreaName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt.Rows.Count > 0)
                        {

                            DisplayMessage("Area Name is Already Exists");
                            txtAreaName.Text = "";
                            txtAreaName.Focus();
                        }
                        DataTable dt1 = objAreamaster.GetAreaMasterInactive();
                        dt1 = new DataView(dt1, "Area_Name='" + txtAreaName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {

                            DisplayMessage("Area Name Already Exists in Bin Section");
                            txtAreaName.Text = "";
                            txtAreaName.Focus();
                        }
                    }
                }
            }
        }


    }
    protected void txtParentArea_TextChanged(object sender, EventArgs e)
    {
        if (txtParentArea.Text != "")
        {

            DataTable dt = objAreamaster.GetAreaMaster();

            dt = new DataView(dt, "Area_Name='" + txtParentArea.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Area Name not Exists");
                txtParentArea.Text = "";
                txtParentArea.Focus();
            }
            else
            {
                hdnParentid.Value = dt.Rows[0]["Trans_Id"].ToString();
            }
        }
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
            DataTable dtCust = (DataTable)Session["Country"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Areamstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvAreaMaster, view.ToTable(), "", "");

         
        }
        //btnRefresh.Focus();
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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
            DataTable dtCust = (DataTable)Session["dtbinCountry"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)gvAreaMasterBin, view.ToTable(), "", "");

           
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvAreaMasterBin.Rows)
        {
            index = (int)gvAreaMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvAreaMasterBin.Rows)
            {
                int index = (int)gvAreaMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvAreaMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvAreaMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvAreaMasterBin, dt, "", "");

      
        PopulateCheckedValuesemplog();
    }
    protected void gvAreaMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objAreamaster.GetAreaMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvAreaMasterBin, dt, "", "");

   
        gvAreaMasterBin.HeaderRow.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (txtAreaName.Text.Trim() == "")
        {
            DisplayMessage("Enter Area Name");
            txtAreaName.Focus();
            return;
        }
        if (txtParentArea.Text.Trim() == "")
        {
            hdnParentid.Value = "0";
        }
        if (editid.Value == "")
        {
            b = objAreamaster.InsertAreaMaster(txtAreaName.Text.Trim(), txtAreanameL.Text.Trim(), hdnParentid.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                btnList_Click(null, null);
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
            b = objAreamaster.UpdateAreaMaster(editid.Value, txtAreaName.Text.Trim(), txtAreanameL.Text, hdnParentid.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
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
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objAreamaster.GetAreaMasterById(editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtAreaName.Text = dt.Rows[0]["Area_Name"].ToString();
            txtAreanameL.Text = dt.Rows[0]["Area_Name_L"].ToString();
            txtParentArea.Text = dt.Rows[0]["ParentAreaName"].ToString();
            hdnParentid.Value = dt.Rows[0]["Parent_Area_Id"].ToString();
            Lbl_Tab_New_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            txtAreaName.Focus();
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();
        int b = 0;


        b = objAreamaster.DeleteAreaMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        try
        {
            gvAreaMaster.Rows[gvRow.RowIndex].Cells[1].Focus();
        }
        catch
        {
        }
    }
    protected void gvAreaMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvAreaMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Areamstr"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvAreaMaster, dt, "", "");

    
        gvAreaMaster.HeaderRow.Focus();

    }
    protected void gvAreaMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Areamstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Areamstr"] = dt;
        gvAreaMaster.DataSource = dt;
        gvAreaMaster.DataBind();
   
        gvAreaMaster.HeaderRow.Focus();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCountryCode(string prefixText, int count, string contextKey)
    {
        CountryMaster objCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objCountryMaster.GetCountryMaster(), "Country_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Country_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAreaName(string prefixText, int count, string contextKey)
    {
        Sys_AreaMaster objAreaMaster = new Sys_AreaMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAreaMaster.GetAreaMaster(), "Area_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Area_Name"].ToString();
        }
        return txt;
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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void FillGrid()
    {
        DataTable dt = objAreamaster.GetAreaMaster();
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvAreaMaster, dt, "", "");

  
        Session["dtFilter_Areamstr"] = dt;
        Session["Country"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objAreamaster.GetAreaMasterInactive();
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvAreaMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinCountry"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        CheckBox chkSelAll = ((CheckBox)gvAreaMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvAreaMasterBin.Rows)
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
            foreach (GridViewRow GR in gvAreaMasterBin.Rows)
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
            objPageCmn.FillData((object)gvAreaMasterBin, dtUnit1, "", "");

            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvAreaMasterBin.Rows.Count > 0)
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
                            b = objAreamaster.DeleteAreaMaster(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
                    foreach (GridViewRow Gvr in gvAreaMasterBin.Rows)
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
                gvAreaMasterBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        txtAreaName.Text = "";
        txtAreanameL.Text = "";
        txtParentArea.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtAreaName.Focus();
        hdnParentid.Value = "0";
    }
}