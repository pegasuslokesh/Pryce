﻿using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_Job_Profile : System.Web.UI.Page
{
    EmployeeMaster objEmp = null;
    Common cmn = null;
    Jobs Jobs = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
    Common ObjComman = null;
    PageControlCommon objPageCmn = null;

    //--------------- Start System ---------------

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }
            objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            Jobs = new Jobs(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            Page.Title = objSys.GetSysTitle();
            //AllPageCode();
            if (!IsPostBack)
            {
                Txt_Selection_Process.Text = "1st round- Telephonic \n2nd round- face to face interviewing, (if shortlisted in 1st round) \n3rd round- HR and Operations testing then selecting the best available candidate.";
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/EmployeeLeaveSalary.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                Fill_Grid_List();
            }
        }
        catch
        {
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bEdit;
        Img_Emp_List_Active.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    public void DisplayMessage(string str,string color="orange")
    {
        try
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
        catch
        {
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

    //--------------- End System ---------------

    //--------------- Start List ---------------

    protected void Btn_List_Li_Click(object sender, EventArgs e)
    {
        try
        {
            Fill_Grid_List();
        }
        catch
        {
        }
    }

    protected void Fill_Grid_List()
    {
        try
        {
            Session["JP_CHECKED_ITEMS_LIST"] = null;
            Session["JP_Dt_Filter"] = null;
            DataTable Dt_Job_Pro = Jobs.Get_Hr_JobProfile("0", Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
            Session["JP_Dt_Job_Pro_List"] = Dt_Job_Pro;

            DataTable Dt_Job_Pro_List = new DataView(Session["JP_Dt_Job_Pro_List"] as DataTable, "Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
            Session["JP_Dt_Job_Pro_List_Active"] = Dt_Job_Pro_List;
            if (Dt_Job_Pro_List.Rows.Count > 0)
            {
                Fill_Gv_Job_Pro(Dt_Job_Pro_List);
            }
            else
            {
                Fill_Gv_Job_Pro(Dt_Job_Pro_List);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Job_Pro(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Job_Pro_List.DataSource = Dt_Grid;
            Gv_Job_Pro_List.DataBind();
            //AllPageCode();
        }
        else
        {
            Gv_Job_Pro_List.DataSource = null;
            Gv_Job_Pro_List.DataBind();
        }
    }

    private void Save_Checked_Job_Pro_Master()
    {
        ArrayList Job_Profiles = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Job_Pro_List.Rows)
        {
            index = (int)Gv_Job_Pro_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["JP_CHECKED_ITEMS_LIST"] != null)
                Job_Profiles = (ArrayList)Session["JP_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Job_Profiles.Contains(index))
                    Job_Profiles.Add(index);
            }
            else
                Job_Profiles.Remove(index);
        }
        if (Job_Profiles != null && Job_Profiles.Count > 0)
            Session["JP_CHECKED_ITEMS_LIST"] = Job_Profiles;
    }

    protected void Populate_Checked_Job_Pro_Master()
    {
        ArrayList Job_Profiles = (ArrayList)Session["JP_CHECKED_ITEMS_LIST"];
        if (Job_Profiles != null && Job_Profiles.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Job_Pro_List.Rows)
            {
                int index = (int)Gv_Job_Pro_List.DataKeys[gvrow.RowIndex].Value;
                if (Job_Profiles.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["JP_Dt_Filter"] = null;
            Save_Checked_Job_Pro_Master();
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
                DataTable Dt_Job_Pro_List = (DataTable)Session["JP_Dt_Job_Pro_List_Active"];
                DataView view = new DataView(Dt_Job_Pro_List, condition, "", DataViewRowState.CurrentRows);
                Session["JP_Dt_Filter"] = view.ToTable();
                Fill_Gv_Job_Pro(view.ToTable());
                txtValue.Focus();
            }
            Populate_Checked_Job_Pro_Master();
        }
        catch
        {
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["JP_CHECKED_ITEMS_LIST"] = null;
            Session["JP_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Job_Pro_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Job_Pro(Session["JP_Dt_Job_Pro_List_Active"] as DataTable);
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["JP_Dt_Job_Pro_List_Active"];
        ArrayList Job_Profiles = new ArrayList();
        Session["JP_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["JP_CHECKED_ITEMS_LIST"] != null)
                {
                    Job_Profiles = (ArrayList)Session["JP_CHECKED_ITEMS_LIST"];
                }
                if (!Job_Profiles.Contains(dr["Trans_ID"]))
                {
                    Job_Profiles.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Job_Pro_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Job_Profiles.Count > 0 && Job_Profiles != null)
            {
                Session["JP_CHECKED_ITEMS_LIST"] = Job_Profiles;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["JP_Dt_Job_Pro_List_Active"];
            objPageCmn.FillData((object)Gv_Job_Pro_List, dt, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
            
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        ArrayList Job_Profile = new ArrayList();
        if (Gv_Job_Pro_List.Rows.Count > 0)
        {
            Save_Checked_Job_Pro_Master();
            if (Session["JP_CHECKED_ITEMS_LIST"] != null)
            {
                Job_Profile = (ArrayList)Session["JP_CHECKED_ITEMS_LIST"];
                if (Job_Profile.Count > 0)
                {
                    for (int j = 0; j < Job_Profile.Count; j++)
                    {
                        if (Job_Profile[j].ToString() != "")
                        {
                            b = Jobs.Set_Hr_JobProfile(Job_Profile[j].ToString(), Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["JP_CHECKED_ITEMS_LIST"] = null;
                    Session["JP_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["JP_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Job_Pro_List.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select");
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
                Gv_Job_Pro_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Job_Pro_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Job_Pro_List.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select")).Checked = false;
            }
        }
    }

    protected void Btn_Edit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        string Trans_ID = e.CommandArgument.ToString();
        Edit_ID.Value = e.CommandArgument.ToString();
        DataTable Dt_Job_Pro_List = Session["JP_Dt_Job_Pro_List_Active"] as DataTable;
        Dt_Job_Pro_List = new DataView(Dt_Job_Pro_List, "Trans_ID = " + Trans_ID + " And Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt_Job_Pro_List.Rows.Count > 0)
        {
            Txt_Category.Text = Dt_Job_Pro_List.Rows[0]["Category"].ToString();
            Hdn_Category_ID.Value = Dt_Job_Pro_List.Rows[0]["Category_ID"].ToString();
            Txt_Profile.Text = Dt_Job_Pro_List.Rows[0]["Profile_Name"].ToString();
            Txt_Description.Text = Dt_Job_Pro_List.Rows[0]["Description"].ToString();
            Txt_Proficiency.Text = Dt_Job_Pro_List.Rows[0]["Proficiency"].ToString();
            Txt_Key_Skill.Text = Dt_Job_Pro_List.Rows[0]["Key_Skills"].ToString();
            Txt_Qualification.Text = Dt_Job_Pro_List.Rows[0]["Qualification"].ToString();
            Txt_Experience.Text = Dt_Job_Pro_List.Rows[0]["Experience"].ToString();
            Txt_Relevant_Experience.Text = Dt_Job_Pro_List.Rows[0]["Relevant_Experience"].ToString();
            Txt_Salary_Range.Text = Dt_Job_Pro_List.Rows[0]["Salary_Range"].ToString();
            Txt_Incentive_Commission.Text = Dt_Job_Pro_List.Rows[0]["Incentive_Commission"].ToString();
            if (Dt_Job_Pro_List.Rows[0]["Job_Type"].ToString() == "Permanent")
                DDL_Job_Type.SelectedValue = "1";
            else if (Dt_Job_Pro_List.Rows[0]["Job_Type"].ToString() == "Temporary")
                DDL_Job_Type.SelectedValue = "2";
            Txt_Language.Text = Dt_Job_Pro_List.Rows[0]["Language_Skill_Required"].ToString();
            Txt_Selection_Process.Text = Dt_Job_Pro_List.Rows[0]["Selection_Process"].ToString().Replace("\r\n", "<br />");
            Editor_Responsibilities.Content = Dt_Job_Pro_List.Rows[0]["Responsibilities"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = Jobs.Set_Hr_JobProfile(Trans_ID, Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
        if (b != 0)
        {
            Fill_Grid_List();
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }

    protected void Gv_Job_Pro_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Job_Pro_Master();
            Gv_Job_Pro_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["JP_Dt_Filter"] != null)
                dt = (DataTable)Session["JP_Dt_Filter"];
            else if (Session["JP_Dt_Job_Pro_List_Active"] != null)
                dt = (DataTable)Session["JP_Dt_Job_Pro_List_Active"];
            objPageCmn.FillData((object)Gv_Job_Pro_List, dt, "", "");
            //AllPageCode();
            Gv_Job_Pro_List.HeaderRow.Focus();
            Populate_Checked_Job_Pro_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Job_Pro_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Job_Pro_Master();
            DataTable dt = new DataTable();
            if (Session["JP_Dt_Filter"] != null)
                dt = (DataTable)Session["JP_Dt_Filter"];
            else if (Session["JP_Dt_Job_Pro_List_Active"] != null)
                dt = (DataTable)Session["JP_Dt_Job_Pro_List_Active"];

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
            Session["JP_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Job_Pro_List, dt, "", "");
            //AllPageCode();
            Gv_Job_Pro_List.HeaderRow.Focus();
            Populate_Checked_Job_Pro_Master();
        }
        catch
        {
        }
    }

    //--------------- End List ---------------

    //--------------- Start Bin ---------------
    protected void Btn_Bin_Li_Click(object sender, EventArgs e)
    {
        try
        {
            Session["JP_CHECKED_ITEMS_BIN"] = null;
            Session["JP_Dt_Filter_Bin"] = null;
            Fill_Grid_Bin();
        }
        catch
        {

        }
    }

    protected void Fill_Grid_Bin()
    {
        try
        {
            DataTable Dt_Job_Pro_Bin = Jobs.Get_Hr_JobProfile("0", Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
            Dt_Job_Pro_Bin = new DataView(Dt_Job_Pro_Bin, "Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();
            Session["JP_Dt_Job_Pro_Bin_InActive"] = Dt_Job_Pro_Bin;
            if (Dt_Job_Pro_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Job_Pro_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Job_Pro_Bin);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Bin(DataTable Dt_Grid)
    {
        Lbl_TotalRecords_Bin.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();
        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Job_Pro_Bin.DataSource = Dt_Grid;
            Gv_Job_Pro_Bin.DataBind();
            Img_Emp_List_Active.Visible = true;
        }
        else
        {
            Gv_Job_Pro_Bin.DataSource = Dt_Grid;
            Gv_Job_Pro_Bin.DataBind();
            Img_Emp_List_Active.Visible = false;
        }
    }

    private void Save_Checked_Job_Pro_Master_Bin()
    {
        ArrayList Job_Profiles = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Job_Pro_Bin.Rows)
        {
            index = (int)Gv_Job_Pro_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["JP_CHECKED_ITEMS_BIN"] != null)
                Job_Profiles = (ArrayList)Session["JP_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Job_Profiles.Contains(index))
                    Job_Profiles.Add(index);
            }
            else
                Job_Profiles.Remove(index);
        }
        if (Job_Profiles != null && Job_Profiles.Count > 0)
            Session["JP_CHECKED_ITEMS_BIN"] = Job_Profiles;
    }

    protected void Populate_Checked_Job_Pro_Master_Bin()
    {
        ArrayList Job_Profiles = (ArrayList)Session["JP_CHECKED_ITEMS_BIN"];
        if (Job_Profiles != null && Job_Profiles.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Job_Pro_Bin.Rows)
            {
                int index = (int)Gv_Job_Pro_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Job_Profiles.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["JP_Dt_Filter_Bin"] = null;
            Save_Checked_Job_Pro_Master_Bin();
            if (ddlOptionBin.SelectedIndex != 0)
            {
                string condition = string.Empty;
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
                DataTable Dt_Job_Pro_Bin = (DataTable)Session["JP_Dt_Job_Pro_Bin_InActive"];
                DataView view = new DataView(Dt_Job_Pro_Bin, condition, "", DataViewRowState.CurrentRows);
                Session["JP_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                txtValueBin.Focus();
            }
            Populate_Checked_Job_Pro_Master_Bin();
        }
        catch
        {
        }
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["JP_CHECKED_ITEMS_BIN"] = null;
            Session["JP_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Job_Pro_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["JP_Dt_Job_Pro_Bin_InActive"] as DataTable);
            txtValueBin.Text = "";
            ddlOptionBin.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["JP_Dt_Job_Pro_Bin_InActive"];
        ArrayList Job_Profiles = new ArrayList();
        Session["JP_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["JP_CHECKED_ITEMS_BIN"] != null)
                {
                    Job_Profiles = (ArrayList)Session["JP_CHECKED_ITEMS_BIN"];
                }
                if (!Job_Profiles.Contains(dr["Trans_ID"]))
                {
                    Job_Profiles.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Job_Pro_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Job_Profiles.Count > 0 && Job_Profiles != null)
            {
                Session["JP_CHECKED_ITEMS_BIN"] = Job_Profiles;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["JP_Dt_Job_Pro_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Job_Pro_Bin, dt, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
            if (dt.Rows.Count > 0)
            {
                Img_Emp_List_Active.Visible = true;
            }
            else
            {
                
                Img_Emp_List_Active.Visible = false;
            }
        }
    }

    protected void Img_Emp_List_Active_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList Job_Profile = new ArrayList();
        if (Gv_Job_Pro_Bin.Rows.Count > 0)
        {
            Save_Checked_Job_Pro_Master_Bin();
            if (Session["JP_CHECKED_ITEMS_BIN"] != null)
            {
                Job_Profile = (ArrayList)Session["JP_CHECKED_ITEMS_BIN"];
                if (Job_Profile.Count > 0)
                {
                    for (int j = 0; j < Job_Profile.Count; j++)
                    {
                        if (Job_Profile[j].ToString() != "")
                        {
                            b = Jobs.Set_Hr_JobProfile(Job_Profile[j].ToString(), Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["JP_CHECKED_ITEMS_BIN"] = null;
                    Session["JP_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["JP_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Job_Pro_Bin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select_Bin");
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
                Gv_Job_Pro_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Job_Pro_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Job_Pro_Bin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
        }
    }

    protected void IBtn_Active_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = Jobs.Set_Hr_JobProfile(Trans_ID, Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
        if (b != 0)
        {
            Fill_Grid_List();
            Fill_Grid_Bin();
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
    }

    protected void Gv_Job_Pro_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Job_Pro_Master_Bin();
            Gv_Job_Pro_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["JP_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["JP_Dt_Filter_Bin"];
            else if (Session["JP_Dt_Job_Pro_Bin_InActive"] != null)
                dt = (DataTable)Session["JP_Dt_Job_Pro_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Job_Pro_Bin, dt, "", "");
            //AllPageCode();
            Gv_Job_Pro_Bin.HeaderRow.Focus();
            Populate_Checked_Job_Pro_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Job_Pro_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Job_Pro_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["JP_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["JP_Dt_Filter_Bin"];
            else if (Session["JP_Dt_Job_Pro_Bin_InActive"] != null)
                dt = (DataTable)Session["JP_Dt_Job_Pro_Bin_InActive"];

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
            Session["JP_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Job_Pro_Bin, dt, "", "");
            //AllPageCode();
            Gv_Job_Pro_Bin.HeaderRow.Focus();
            Populate_Checked_Job_Pro_Master_Bin();
        }
        catch
        {
        }
    }

    //--------------- End Bin ---------------


    //--------------- Start New ---------------

    protected void Btn_New_Li_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt_Job_Pro_Active = new DataTable();
            DataTable Dt_Job_Pro_InActive = new DataTable();
            DataTable Dt_Job_Pro_List = Jobs.Get_Hr_JobProfile("0", Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");

            if (Dt_Job_Pro_List.Rows.Count > 0)
                Dt_Job_Pro_Active = new DataView(Dt_Job_Pro_List, "Category_ID = " + Hdn_Category_ID.Value + " and Profile_Name='" + Txt_Profile.Text + "' and Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();

            if (Dt_Job_Pro_List.Rows.Count > 0)
                Dt_Job_Pro_InActive = new DataView(Dt_Job_Pro_List, "Category_ID = " + Hdn_Category_ID.Value + " and Profile_Name='" + Txt_Profile.Text + "' and Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();

            if (Txt_Category.Text == "")
            {
                DisplayMessage("Enter Category");
                return;
            }
            else if (Txt_Profile.Text == "")
            {
                DisplayMessage("Enter Profile");
                return;
            }
            else if (Txt_Proficiency.Text == "")
            {
                DisplayMessage("Enter Proficiency");
                return;
            }
            else if (Txt_Key_Skill.Text == "")
            {
                DisplayMessage("Enter Key Skill");
                return;
            }
            else if (Txt_Qualification.Text == "")
            {
                DisplayMessage("Enter Qualification");
                return;
            }
            else if (Txt_Experience.Text == "")
            {
                DisplayMessage("Enter Experience");
                return;
            }
            else if (Txt_Salary_Range.Text == "")
            {
                DisplayMessage("Enter Salary Range");
                return;
            }
            else if (Edit_ID.Value == "")
            {
                if (Dt_Job_Pro_Active.Rows.Count > 0)
                {
                    DisplayMessage("Record Already Exists");
                    return;
                }
                else if (Dt_Job_Pro_InActive.Rows.Count > 0)
                {
                    DisplayMessage("Record Already Exists in Bin");
                    return;
                }
            }

            if (Edit_ID.Value == "")
            {
                int b = 0;
                b = Jobs.Set_Hr_JobProfile("0", Session["CompId"].ToString(), Hdn_Category_ID.Value, Txt_Profile.Text, Txt_Description.Text, Txt_Proficiency.Text, Txt_Key_Skill.Text, Txt_Qualification.Text, Txt_Experience.Text, Txt_Relevant_Experience.Text, Txt_Salary_Range.Text, Txt_Incentive_Commission.Text, DDL_Job_Type.SelectedItem.ToString(), Txt_Language.Text, Txt_Selection_Process.Text, Editor_Responsibilities.Content, "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                if (b != 0)
                {
                    DisplayMessage("Record Saved","green");
                    Reset();
                    Fill_Grid_List();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
            else
            {
                int b = 0;
                b = Jobs.Set_Hr_JobProfile(Edit_ID.Value, Session["CompId"].ToString(), Hdn_Category_ID.Value, Txt_Profile.Text, Txt_Description.Text, Txt_Proficiency.Text, Txt_Key_Skill.Text, Txt_Qualification.Text, Txt_Experience.Text, Txt_Relevant_Experience.Text, Txt_Salary_Range.Text, Txt_Incentive_Commission.Text, DDL_Job_Type.SelectedItem.ToString(), Txt_Language.Text, Txt_Selection_Process.Text, Editor_Responsibilities.Content, "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    Reset();
                    Fill_Grid_List();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Updated");
                }
            }

        }
        catch (Exception ex)
        {
        }
    }

    protected void Reset()
    {
        try
        {
            Edit_ID.Value = "";
            Hdn_Category_ID.Value = "";
            Txt_Category.Text = "";
            Txt_Profile.Text = "";
            Txt_Description.Text = "";
            Txt_Proficiency.Text = "";
            Txt_Key_Skill.Text = "";
            Txt_Qualification.Text = "";
            Txt_Experience.Text = "";
            Txt_Relevant_Experience.Text = "";
            Txt_Salary_Range.Text = "";
            Txt_Incentive_Commission.Text = "";
            DDL_Job_Type.SelectedValue = "0";
            Txt_Language.Text = "";
            Txt_Selection_Process.Text = "";
            Editor_Responsibilities.Content = "";
            Txt_Selection_Process.Text = "1st round- Telephonic \n2nd round- face to face interviewing, (if shortlisted in 1st round) \n3rd round- HR and Operations testing then selecting the best available candidate.";
            Lbl_Tab_New.Text = Resources.Attendance.New;
        }
        catch
        {
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
        catch
        {
        }
    }

    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
        }
        catch
        {
        }
    }

    //--------------- End New ---------------
    protected void Txt_Category_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        DataTable Dt_Job_Cat = Jobs.Get_Hr_JobCategory("0", Session["CompId"].ToString(), Txt_Category.Text, "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");

        if (Dt_Job_Cat.Rows.Count == 0)
        {
            Hdn_Category_ID.Value = "";
            Txt_Category.Text = "";
            counter = 1;
            DisplayMessage("Record Not Exists in Category");
        }
        else
        {
            Hdn_Category_ID.Value = Dt_Job_Cat.Rows[0]["Trans_ID"].ToString();
        }

        if (counter == 0)
            Txt_Profile.Focus();
        else
            Txt_Category.Focus();
    }

    protected void Txt_Profile_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        DataTable Dt_Job_Pro_List = new DataView(Session["JP_Dt_Job_Pro_List"] as DataTable, "Profile_Name='" + Txt_Profile.Text.Trim() + "' and Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();

        DataTable Dt_Job_Pro_Bin = new DataView(Session["JP_Dt_Job_Pro_List"] as DataTable, "Profile_Name='" + Txt_Profile.Text.Trim() + "' and Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();

        if (Dt_Job_Pro_List.Rows.Count > 0)
        {
            Txt_Profile.Text = "";
            counter = 1;
            DisplayMessage("Record Already Exists");
        }
        else if (Dt_Job_Pro_Bin.Rows.Count > 0)
        {
            Txt_Profile.Text = "";
            counter = 1;
            DisplayMessage("Record Already Exists in Bin");
        }

        if (counter == 0)
            Txt_Description.Focus();
        else
            Txt_Profile.Focus();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Profile_Name(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        Jobs Jobs = new Jobs(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(Jobs.Get_Hr_JobProfile("0", HttpContext.Current.Session["CompId"].ToString(), "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "True", HttpContext.Current.Session["UserID"].ToString(), HttpContext.Current.Session["UserID"].ToString(), "1"), "Profile_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Profile_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Category(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        Jobs Jobs = new Jobs(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(Jobs.Get_Hr_JobCategory("0", HttpContext.Current.Session["CompId"].ToString(), "", "", "True", HttpContext.Current.Session["UserID"].ToString(), HttpContext.Current.Session["UserID"].ToString(), "2"), "Category like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Category"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Qualification(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        QualificationMaster QualificationMaster = new QualificationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(QualificationMaster.GetQualification("0"), "Qualification like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Qualification"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Skill(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        SkillMaster SkillMaster = new SkillMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(SkillMaster.GetAllRecord(), "Skill like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Skill"].ToString();
        }
        return txt;
    }
}