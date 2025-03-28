using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SystemSetUp_CountryMaster : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    CountryMaster objCountry = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objCountry = new CountryMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../SystemSetUp/CountryMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            
           

            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            ddlTimeZone.DataSource = timeZones;
            ddlTimeZone.DataTextField = "DisplayName";
            ddlTimeZone.DataValueField = "id";
            ddlTimeZone.DataBind();
            ddlTimeZone.Items.Insert(0, new ListItem("--Select Time Zone--", ""));
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
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        txtValue.Focus();
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlList.Visible = true;
        //PnlNewEdit.Visible = false;
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
    protected void txtCountryCode_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        if (txtCountryCode.Text.Trim() != "")
        {
            if (editid.Value == "")
            {
                DataTable dtActiveRecord = objCountry.GetCountryMaster();

                dtActiveRecord = new DataView(dtActiveRecord, "Country_Code='" + txtCountryCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtActiveRecord.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Country Code is Already Exists");
                    txtCountryCode.Text = "";
                    txtCountryCode.Focus();
                }
                else
                {
                    DataTable dtInActive = objCountry.GetCountryMasterInactive();

                    dtInActive = new DataView(dtInActive, "Country_Code='" + txtCountryCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtInActive.Rows.Count > 0)
                    {
                        counter = 1;
                        DisplayMessage("Country Code is Already Exists in Bin Section");
                        txtCountryCode.Text = "";
                        txtCountryCode.Focus();
                    }
                }
            }
            else
            {
                DataTable dtActiveRecord = objCountry.GetCountryMaster();
                dtActiveRecord = new DataView(dtActiveRecord, "Country_Code='" + txtCountryCode.Text.Trim() + "' and Country_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtActiveRecord.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Country Code is Already Exists");
                    txtCountryCode.Text = "";
                    txtCountryCode.Focus();
                }
                else
                {
                    DataTable dtInActive = objCountry.GetCountryMasterInactive();
                    dtInActive = new DataView(dtInActive, "Country_Code='" + txtCountryCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtInActive.Rows.Count > 0)
                    {
                        counter = 1;
                        DisplayMessage("Country Code is Already Exists in Bin Section");
                        txtCountryCode.Text = "";
                        txtCountryCode.Focus();
                    }
                }
            }
        }
        if (counter == 0)
        {
            txtCountryName.Focus();
        }

    }
    protected void txtCountryName_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        if (txtCountryName.Text.Trim() != "")
        {
            if (editid.Value == "")
            {
                DataTable dt = objCountry.GetCountryMasterByCountryName(txtCountryName.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Country Name is Already Exists");
                    txtCountryName.Text = "";
                    txtCountryName.Focus();
                }
                DataTable dt1 = objCountry.GetCountryMasterInactive();
                dt1 = new DataView(dt1, "Country_Name='" + txtCountryName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Country Name Already Exists in Bin Section");
                    txtCountryName.Text = "";
                    txtCountryName.Focus();
                }
            }
            else
            {
                DataTable dtTemp = objCountry.GetCountryMasterById(editid.Value);
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Country_Name"].ToString() != txtCountryName.Text)
                    {
                        DataTable dt = objCountry.GetCountryMasterByCountryName(txtCountryName.Text.Trim());
                        if (dt.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Country Name is Already Exists");
                            txtCountryName.Text = "";
                            txtCountryName.Focus();
                        }
                        DataTable dt1 = objCountry.GetCountryMasterInactive();
                        dt1 = new DataView(dt1, "Country_Name='" + txtCountryName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Country Name Already Exists in Bin Section");
                            txtCountryName.Text = "";
                            txtCountryName.Focus();
                        }
                    }
                }
            }
        }

        if (counter == 0)
            txtCountryNameL.Focus();
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
            Session["dtFilter_Country"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvCountryMaster, view.ToTable(), "", "");

           
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
            objPageCmn.FillData((object)gvCountryMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                // imgBtnRestore.Visible = false;
                // ImgbtnSelectAll.Visible = false;
            }
            else
            {
                
            }
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
        foreach (GridViewRow gvrow in gvCountryMasterBin.Rows)
        {
            index = (int)gvCountryMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvCountryMasterBin.Rows)
            {
                int index = (int)gvCountryMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvCountryMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvCountryMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvCountryMasterBin, dt, "", "");

    
        PopulateCheckedValuesemplog();
    }
    protected void gvCountryMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objCountry.GetCountryMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvCountryMasterBin, dt, "", "");

        
        gvCountryMasterBin.HeaderRow.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (txtCountryCode.Text.Trim() == "")
        {
            DisplayMessage("Enter Country Code");
            txtCountryCode.Focus();
            return;
        }
        if (txtCountryName.Text.Trim() == "")
        {
            DisplayMessage("Enter Country Name");
            txtCountryName.Focus();
            return;
        }
        if (editid.Value == "")
        {
            b = objCountry.InsertCountryMaster(txtCountryName.Text.Trim(), txtCountryNameL.Text.Trim(), txtCountryCode.Text.Trim(), ddlTimeZone.SelectedValue, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            b = objCountry.UpdateCountryMaster(editid.Value, txtCountryName.Text.Trim(), txtCountryNameL.Text.Trim(), txtCountryCode.Text.Trim(), ddlTimeZone.SelectedValue, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
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

        DataTable dt = objCountry.GetCountryMasterById(editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtCountryCode.Text = dt.Rows[0]["Country_Code"].ToString();
            txtCountryName.Text = dt.Rows[0]["Country_Name"].ToString();
            txtCountryNameL.Text = dt.Rows[0]["Country_Name_L"].ToString();
            ddlTimeZone.SelectedValue = dt.Rows[0]["field1"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            txtCountryCode.Focus();
        }

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        editid.Value = e.CommandArgument.ToString();
        int b = 0;

        DataTable dt = new DataTable();
        dt = cmn.GetCheckEsistenceId(editid.Value, "2");

        if (dt.Rows.Count > 0)
        {
            DisplayMessage("Record is in Use ,you cannot delete this");
            try
            {
                gvCountryMaster.Rows[gvRow.RowIndex].Cells[1].Focus();
            }
            catch
            {

            }
            return;
        }
        b = objCountry.DeleteCountryMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            gvCountryMaster.Rows[gvRow.RowIndex].Cells[1].Focus();
        }
        catch
        {
        }
    }
    protected void gvCountryMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCountryMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Country"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvCountryMaster, dt, "", "");

        
        gvCountryMaster.HeaderRow.Focus();

    }
    protected void gvCountryMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Country"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Country"] = dt;
        gvCountryMaster.DataSource = dt;
        gvCountryMaster.DataBind();
        
        gvCountryMaster.HeaderRow.Focus();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCountryCode(string prefixText, int count, string contextKey)
    {
        CountryMaster objCountryMaster = new CountryMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objCountryMaster.GetCountryMaster(), "Country_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Country_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCountryName(string prefixText, int count, string contextKey)
    {
        CountryMaster objCountryMaster = new CountryMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objCountryMaster.GetCountryMaster(), "Country_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Country_Name"].ToString();
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

    }
    public void FillGrid()
    {
        DataTable dt = objCountry.GetCountryMaster();
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvCountryMaster, dt, "", "");

        
        Session["dtFilter_Country"] = dt;
        Session["Country"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objCountry.GetCountryMasterInactive();
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)gvCountryMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinCountry"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            // imgBtnRestore.Visible = false;
            // ImgbtnSelectAll.Visible = false;
        }
        else
        {
           
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvCountryMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvCountryMasterBin.Rows)
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

                if (!userdetails.Contains(dr["Country_Id"]))
                {
                    userdetails.Add(dr["Country_Id"]);
                }
            }
            foreach (GridViewRow GR in gvCountryMasterBin.Rows)
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
            objPageCmn.FillData((object)gvCountryMasterBin, dtUnit1, "", "");

            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvCountryMasterBin.Rows.Count > 0)
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
                            b = objCountry.DeleteCountryMaster(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
                    foreach (GridViewRow Gvr in gvCountryMasterBin.Rows)
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
                gvCountryMasterBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        txtCountryCode.Text = "";
        txtCountryName.Text = "";
        txtCountryNameL.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtCountryCode.Focus();
        ddlTimeZone.SelectedIndex = 0;
    }
}
