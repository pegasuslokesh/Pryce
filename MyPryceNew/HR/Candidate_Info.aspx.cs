using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_Candidate_Info : System.Web.UI.Page
{
    HR_CandidateMaster objCandidate = null;
    CandidateFollowUp CandidateFollowUp = null;
    Common cmn = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
    Common ObjComman = null;
    CandidateMaster CandidateMaster = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }
            Txt_Follow_Date_CalendarExtender.Format = objSys.SetDateFormat();
            Txt_Next_Follow_Date_CalendarExtender.Format = objSys.SetDateFormat();

            CalendarExtender1.Format = objSys.SetDateFormat();
            CalendarExtender3.Format = objSys.SetDateFormat();
            Page.Title = objSys.GetSysTitle();

            objCandidate = new HR_CandidateMaster(Session["DBConnection"].ToString());
            CandidateFollowUp = new CandidateFollowUp(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            CandidateMaster = new CandidateMaster(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
            if (!IsPostBack)
            {
              
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/Candidate_Info.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
        Btn_Save.Visible = true;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        Img_Emp_List_Active.Visible = clsPagePermission.bRestore;
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
            Session["Cd_CHECKED_ITEMS_LIST"] = null;
            Session["Cd_Dt_Filter"] = null;
            DataTable Dt_Candidate_Follow = CandidateFollowUp.Get_HR_FollowUp("0", DateTime.Now.ToString(), "Candidate", "0", "", "", Session["UserId"].ToString(), "", "", "", "", "", DateTime.Now.ToString(), "0", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "0");
            Session["Cd_Dt_Candidate_Follow_List"] = Dt_Candidate_Follow;

            DataTable Dt_Candidate_Follow_List = new DataView(Session["Cd_Dt_Candidate_Follow_List"] as DataTable, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            Session["Cd_Dt_Candidate_Follow_List_Active"] = Dt_Candidate_Follow_List;
            if (Dt_Candidate_Follow_List.Rows.Count > 0)
            {
                Fill_Gv_Candidate_Follow(Dt_Candidate_Follow_List);
            }
            else
            {
                Fill_Gv_Candidate_Follow(Dt_Candidate_Follow_List);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Candidate_Follow(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Candidate_Follow_List.DataSource = Dt_Grid;
            Gv_Candidate_Follow_List.DataBind();
            
            ////AllPageCode();
        }
        else
        {
            Gv_Candidate_Follow_List.DataSource = null;
            Gv_Candidate_Follow_List.DataBind();
            
        }
    }

    private void Save_Checked_Candidate_Follow_Master()
    {
        ArrayList Candidate_Follow = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Candidate_Follow_List.Rows)
        {
            index = (int)Gv_Candidate_Follow_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["Cd_CHECKED_ITEMS_LIST"] != null)
                Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Candidate_Follow.Contains(index))
                    Candidate_Follow.Add(index);
            }
            else
                Candidate_Follow.Remove(index);
        }
        if (Candidate_Follow != null && Candidate_Follow.Count > 0)
            Session["Cd_CHECKED_ITEMS_LIST"] = Candidate_Follow;
    }

    protected void Populate_Checked_Candidate_Follow_Master()
    {
        ArrayList Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_LIST"];
        if (Candidate_Follow != null && Candidate_Follow.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Candidate_Follow_List.Rows)
            {
                int index = (int)Gv_Candidate_Follow_List.DataKeys[gvrow.RowIndex].Value;
                if (Candidate_Follow.Contains(index))
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
            Session["Cd_Dt_Filter"] = null;
            Save_Checked_Candidate_Follow_Master();
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
                DataTable Dt_Candidate_Follow_List = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];
                DataView view = new DataView(Dt_Candidate_Follow_List, condition, "", DataViewRowState.CurrentRows);
                Session["Cd_Dt_Filter"] = view.ToTable();
                Fill_Gv_Candidate_Follow(view.ToTable());
                txtValue.Focus();
            }
            Populate_Checked_Candidate_Follow_Master();
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
            Session["Cd_CHECKED_ITEMS_LIST"] = null;
            Session["Cd_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Candidate_Follow_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Candidate_Follow(Session["Cd_Dt_Candidate_Follow_List_Active"] as DataTable);
            txtValue.Text = "";
            TxtValueDate.Text = "";
            ddlOption.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];
        ArrayList Candidate_Follow = new ArrayList();
        Session["Cd_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["Cd_CHECKED_ITEMS_LIST"] != null)
                {
                    Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_LIST"];
                }
                if (!Candidate_Follow.Contains(dr["Trans_ID"]))
                {
                    Candidate_Follow.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Candidate_Follow_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Candidate_Follow.Count > 0 && Candidate_Follow != null)
            {
                Session["Cd_CHECKED_ITEMS_LIST"] = Candidate_Follow;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];
            objPageCmn.FillData((object)Gv_Candidate_Follow_List, dt, "", "");
            ////AllPageCode();
            ViewState["Select"] = null;
            
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        ArrayList Candidate_Follow = new ArrayList();
        if (Gv_Candidate_Follow_List.Rows.Count > 0)
        {
            Save_Checked_Candidate_Follow_Master();
            if (Session["Cd_CHECKED_ITEMS_LIST"] != null)
            {
                Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_LIST"];
                if (Candidate_Follow.Count > 0)
                {
                    for (int j = 0; j < Candidate_Follow.Count; j++)
                    {
                        if (Candidate_Follow[j].ToString() != "")
                        {
                            b = CandidateFollowUp.Set_HR_FollowUp(Candidate_Follow[j].ToString(), DateTime.Now.ToString(), "Candidate", "0", "", "", Session["UserId"].ToString(), "", "", "", "", "", DateTime.Now.ToString(), "0", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["Cd_CHECKED_ITEMS_LIST"] = null;
                    Session["Cd_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["Cd_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Candidate_Follow_List.Rows)
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
                Gv_Candidate_Follow_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Candidate_Follow_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Candidate_Follow_List.Rows)
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
        DataTable Dt_Candidate_Follow_List = Session["Cd_Dt_Candidate_Follow_List_Active"] as DataTable;
        Dt_Candidate_Follow_List = new DataView(Dt_Candidate_Follow_List, "Trans_ID = " + Trans_ID + " And IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt_Candidate_Follow_List.Rows.Count > 0)
        {
            Txt_Candidate_Name.Text = Dt_Candidate_Follow_List.Rows[0]["Candidate_Name"].ToString();
            Txt_Candidate_Name_TextChanged(null, null);
            Txt_Follow_Date.Text =GetDate(Dt_Candidate_Follow_List.Rows[0]["Follow_Date"].ToString());
            Txt_Title.Text = Dt_Candidate_Follow_List.Rows[0]["Title"].ToString();
            Txt_Description.Text = Dt_Candidate_Follow_List.Rows[0]["Description"].ToString();            
            Txt_Next_Follow_Date.Text =GetDate(Dt_Candidate_Follow_List.Rows[0]["Field6"].ToString());
            if(GetDate(Dt_Candidate_Follow_List.Rows[0]["Field6"].ToString())=="")
            {
                Chk_Next_Follow.Checked = false;
                Div_Next_Follow.Visible = false;
                Hdn_Next_Follow_Date.Value = "";
            }
            else
            {
                Chk_Next_Follow.Checked = true;
                Div_Next_Follow.Visible = true;
                Hdn_Next_Follow_Date.Value = GetDate(Dt_Candidate_Follow_List.Rows[0]["Field6"].ToString());
            }

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = CandidateFollowUp.Set_HR_FollowUp(Trans_ID, DateTime.Now.ToString(), "Candidate", "0", "", "", Session["UserId"].ToString(), "", "", "", "", "", DateTime.Now.ToString(), "0", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "2");
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

    protected void Gv_Candidate_Follow_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Candidate_Follow_Master();
            Gv_Candidate_Follow_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["Cd_Dt_Filter"] != null)
                dt = (DataTable)Session["Cd_Dt_Filter"];
            else if (Session["Cd_Dt_Candidate_Follow_List_Active"] != null)
                dt = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];
            objPageCmn.FillData((object)Gv_Candidate_Follow_List, dt, "", "");
            ////AllPageCode();
            Gv_Candidate_Follow_List.HeaderRow.Focus();
            Populate_Checked_Candidate_Follow_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Candidate_Follow_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Candidate_Follow_Master();
            DataTable dt = new DataTable();
            if (Session["Cd_Dt_Filter"] != null)
                dt = (DataTable)Session["Cd_Dt_Filter"];
            else if (Session["Cd_Dt_Candidate_Follow_List_Active"] != null)
                dt = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];

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
            Session["Cd_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Candidate_Follow_List, dt, "", "");
            ////AllPageCode();
            Gv_Candidate_Follow_List.HeaderRow.Focus();
            Populate_Checked_Candidate_Follow_Master();
        }
        catch
        {
        }
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedValue.ToString() == "Follow_Date")
        {
            txtValue.Text = "";
            TxtValueDate.Text = "";

            txtValue.Visible = false;
            btnbind.Visible = false;

            TxtValueDate.Visible = true;
            BtnBindDate.Visible = true;
        }
        else if (ddlFieldName.SelectedValue.ToString() == "Field6")
        {
            txtValue.Text = "";
            TxtValueDate.Text = "";

            txtValue.Visible = false;
            btnbind.Visible = false;

            TxtValueDate.Visible = true;
            BtnBindDate.Visible = true;
        }
        else
        {
            txtValue.Text = "";
            TxtValueDate.Text = "";

            txtValue.Visible = true;
            btnbind.Visible = true;

            TxtValueDate.Visible = false;
            BtnBindDate.Visible = false;
        }
    }

    protected void BtnBindDate_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TxtValueDate.Text != "")
        {
            if (ddlOption.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(TxtValueDate.Text.Trim()).ToString() + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(TxtValueDate.Text.Trim()).ToString() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(TxtValueDate.Text.Trim()).ToString() + "%'";
                }
                if (Session["Cd_Dt_Candidate_Follow_List_Active"] == null)
                {
                    DataTable Dt_Duty_List = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];
                    DataView view = new DataView(Dt_Duty_List, condition, "", DataViewRowState.CurrentRows);
                    Session["Cd_Dt_Filter"] = view.ToTable();
                    Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
                    objPageCmn.FillData((object)Gv_Candidate_Follow_List, view.ToTable(), "", "");
                    //AllPageCode();
                }
                else
                {
                    DataTable Dt_Duty_List = (DataTable)Session["Cd_Dt_Candidate_Follow_List_Active"];
                    DataView view = new DataView(Dt_Duty_List, condition, "", DataViewRowState.CurrentRows);
                    Session["Cd_Dt_Filter"] = view.ToTable();
                    Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
                    objPageCmn.FillData((object)Gv_Candidate_Follow_List, view.ToTable(), "", "");                  
                    //AllPageCode();
                }
                TxtValueDate.Focus();
            }
            TxtValueDate.Focus();
        }
        else
        {
            btnRefresh_Click(null, null);
            TxtValueDate.Focus();
        }
    }

    //--------------- End List ---------------

    //--------------- Start Bin ---------------
    protected void Btn_Bin_Li_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Cd_CHECKED_ITEMS_BIN"] = null;
            Session["Cd_Dt_Filter_Bin"] = null;
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
            DataTable Dt_Candidate_Follow_Bin = CandidateFollowUp.Get_HR_FollowUp("0", DateTime.Now.ToString(), "Candidate", "0", "", "", Session["UserId"].ToString(), "", "", "", "", "", DateTime.Now.ToString(), "0", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "0");
            Dt_Candidate_Follow_Bin = new DataView(Dt_Candidate_Follow_Bin, "IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
            Session["Cd_Dt_Candidate_Follow_Bin_InActive"] = Dt_Candidate_Follow_Bin;
            if (Dt_Candidate_Follow_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Candidate_Follow_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Candidate_Follow_Bin);
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
            Gv_Candidate_Follow_Bin.DataSource = Dt_Grid;
            Gv_Candidate_Follow_Bin.DataBind();
            //AllPageCode();
            Img_Emp_List_Active.Visible = true;
        }
        else
        {
            Gv_Candidate_Follow_Bin.DataSource = Dt_Grid;
            Gv_Candidate_Follow_Bin.DataBind();
            //AllPageCode();
            Img_Emp_List_Active.Visible = false;
        }
    }

    private void Save_Checked_Candidate_Follow_Master_Bin()
    {
        ArrayList Candidate_Follow = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Candidate_Follow_Bin.Rows)
        {
            index = (int)Gv_Candidate_Follow_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["Cd_CHECKED_ITEMS_BIN"] != null)
                Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Candidate_Follow.Contains(index))
                    Candidate_Follow.Add(index);
            }
            else
                Candidate_Follow.Remove(index);
        }
        if (Candidate_Follow != null && Candidate_Follow.Count > 0)
            Session["Cd_CHECKED_ITEMS_BIN"] = Candidate_Follow;
    }

    protected void Populate_Checked_Candidate_Follow_Master_Bin()
    {
        ArrayList Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_BIN"];
        if (Candidate_Follow != null && Candidate_Follow.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Candidate_Follow_Bin.Rows)
            {
                int index = (int)Gv_Candidate_Follow_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Candidate_Follow.Contains(index))
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
            Session["Cd_Dt_Filter_Bin"] = null;
            Save_Checked_Candidate_Follow_Master_Bin();
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
                DataTable Dt_Candidate_Follow_Bin = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];
                DataView view = new DataView(Dt_Candidate_Follow_Bin, condition, "", DataViewRowState.CurrentRows);
                Session["Cd_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                txtValueBin.Focus();
            }
            Populate_Checked_Candidate_Follow_Master_Bin();
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
            Session["Cd_CHECKED_ITEMS_BIN"] = null;
            Session["Cd_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Candidate_Follow_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["Cd_Dt_Candidate_Follow_Bin_InActive"] as DataTable);
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";
            ddlOptionBin.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];
        ArrayList Candidate_Follow = new ArrayList();
        Session["Cd_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["Cd_CHECKED_ITEMS_BIN"] != null)
                {
                    Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_BIN"];
                }
                if (!Candidate_Follow.Contains(dr["Trans_ID"]))
                {
                    Candidate_Follow.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Candidate_Follow_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Candidate_Follow.Count > 0 && Candidate_Follow != null)
            {
                Session["Cd_CHECKED_ITEMS_BIN"] = Candidate_Follow;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Candidate_Follow_Bin, dt, "", "");
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
        ArrayList Candidate_Follow = new ArrayList();
        if (Gv_Candidate_Follow_Bin.Rows.Count > 0)
        {
            Save_Checked_Candidate_Follow_Master_Bin();
            if (Session["Cd_CHECKED_ITEMS_BIN"] != null)
            {
                Candidate_Follow = (ArrayList)Session["Cd_CHECKED_ITEMS_BIN"];
                if (Candidate_Follow.Count > 0)
                {
                    for (int j = 0; j < Candidate_Follow.Count; j++)
                    {
                        if (Candidate_Follow[j].ToString() != "")
                        {
                            b = CandidateFollowUp.Set_HR_FollowUp(Candidate_Follow[j].ToString(), DateTime.Now.ToString(), "Candidate", "0", "", "", Session["UserId"].ToString(), "", "", "", "", "", DateTime.Now.ToString(), "0", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["Cd_CHECKED_ITEMS_BIN"] = null;
                    Session["Cd_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["Cd_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Candidate_Follow_Bin.Rows)
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
                Gv_Candidate_Follow_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Candidate_Follow_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Candidate_Follow_Bin.Rows)
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
        b = CandidateFollowUp.Set_HR_FollowUp(Trans_ID, DateTime.Now.ToString(), "Candidate", "0", "", "", Session["UserId"].ToString(), "", "", "", "", "", DateTime.Now.ToString(), "0", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "2");
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

    protected void Gv_Candidate_Follow_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Candidate_Follow_Master_Bin();
            Gv_Candidate_Follow_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["Cd_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["Cd_Dt_Filter_Bin"];
            else if (Session["Cd_Dt_Candidate_Follow_Bin_InActive"] != null)
                dt = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Candidate_Follow_Bin, dt, "", "");
            //AllPageCode();
            Gv_Candidate_Follow_Bin.HeaderRow.Focus();
            Populate_Checked_Candidate_Follow_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Candidate_Follow_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Candidate_Follow_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["Cd_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["Cd_Dt_Filter_Bin"];
            else if (Session["Cd_Dt_Candidate_Follow_Bin_InActive"] != null)
                dt = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];

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
            Session["Cd_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Candidate_Follow_Bin, dt, "", "");
            //AllPageCode();
            Gv_Candidate_Follow_Bin.HeaderRow.Focus();
            Populate_Checked_Candidate_Follow_Master_Bin();
        }
        catch
        {
        }
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedValue.ToString() == "Follow_Date")
        {
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

            txtValueBin.Visible = false;
            btnbindBin.Visible = false;

            txtValueDateBin.Visible = true;
            btnbindDateBin.Visible = true;
        }
        else if (ddlFieldNameBin.SelectedValue.ToString() == "Field6")
        {
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

            txtValueBin.Visible = false;
            btnbindBin.Visible = false;

            txtValueDateBin.Visible = true;
            btnbindDateBin.Visible = true;
        }
        else
        {
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

            txtValueBin.Visible = true;
            btnbindBin.Visible = true;

            txtValueDateBin.Visible = false;
            btnbindDateBin.Visible = false;
        }
    }

    protected void btnbindDateBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (txtValueDateBin.Text != "")
        {
            if (ddlOptionBin.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOptionBin.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDateBin.Text.Trim()).ToString() + "'";
                }
                else if (ddlOptionBin.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDateBin.Text.Trim()).ToString() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(txtValueDateBin.Text.Trim()).ToString() + "%'";
                }
                if (Session["Cd_Dt_Candidate_Follow_Bin_InActive"] == null)
                {
                    DataTable Dt_Duty_List = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];
                    DataView view = new DataView(Dt_Duty_List, condition, "", DataViewRowState.CurrentRows);
                    Session["Cd_Dt_Filter_Bin"] = view.ToTable();
                    Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
                    objPageCmn.FillData((object)Gv_Candidate_Follow_Bin, view.ToTable(), "", "");
                    //AllPageCode();
                }
                else
                {
                    DataTable Dt_Duty_List = (DataTable)Session["Cd_Dt_Candidate_Follow_Bin_InActive"];
                    DataView view = new DataView(Dt_Duty_List, condition, "", DataViewRowState.CurrentRows);
                    Session["Cd_Dt_Filter_Bin"] = view.ToTable();
                    Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
                    objPageCmn.FillData((object)Gv_Candidate_Follow_Bin, view.ToTable(), "", "");
                    //AllPageCode();
                }
            }
            txtValueDateBin.Focus();
        }
        else
        {
            btnRefreshBin_Click(null, null);
            txtValueDateBin.Focus();
        }
    }

    //--------------- End Bin ---------------

    //--------------- Start New ---------------
    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (Txt_Candidate_Name.Text == "")
        {
            DisplayMessage("Enter Candidate Name");
            Txt_Candidate_Name.Focus();
            return;
        }
        else if (Txt_Follow_Date.Text == "")
        {
            DisplayMessage("Enter Follow Up Date");
            Txt_Follow_Date.Focus();
            return;
        }
        else if (Txt_Title.Text == "")
        {
            DisplayMessage("Enter Title");
            Txt_Title.Focus();
            return;
        }

        else if (Txt_Description.Text == "")
        {
            DisplayMessage("Enter Description");
            Txt_Description.Focus();
            return;
        }
        else if (Chk_Next_Follow.Checked == true)
        {
            if (Txt_Next_Follow_Date.Text == "")
            {
                DisplayMessage("Enter Next Follow Up Date");
                Txt_Next_Follow_Date.Focus();
                return;
            }
        }
        if (Txt_Follow_Date.Text == "")
            Txt_Follow_Date.Text = DateTime.Now.ToString();
        string Next_Follow_Date = string.Empty;
        if (Txt_Next_Follow_Date.Text != "")
            Next_Follow_Date = Convert.ToDateTime(Txt_Next_Follow_Date.Text).ToString();
        
        if (Edit_ID.Value == "")
        {
            b = CandidateFollowUp.Set_HR_FollowUp("0", Txt_Follow_Date.Text, "Candidate", "0", Txt_Title.Text, Txt_Description.Text, Session["UserId"].ToString(), "", "", "", "", "",Next_Follow_Date, Hdn_Candidate_ID.Value, "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1");
        }
        else
        {
            b = CandidateFollowUp.Set_HR_FollowUp(Edit_ID.Value, Txt_Follow_Date.Text, "Candidate", "0", Txt_Title.Text, Txt_Description.Text, Session["UserId"].ToString(), "", "", "", "", "", Next_Follow_Date, Hdn_Candidate_ID.Value, "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "3");
        }
        if (b > 0)
        {
            Fill_Grid_List();
            DisplayMessage("Record Saved", "green");
            Reset();
            //AllPageCode();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
        else
        {
            DisplayMessage("Record Not Saved");
        }
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            Reset();
        }
        catch
        {

        }
    }

    protected void Reset()
    {
        Hdn_Candidate_ID.Value = "";
        Txt_Candidate_Name.Text = "";
        Lbl_Candidate_Type.Text = "";
        Lbl_Email.Text = "";
        Lbl_Father_Name.Text = "";
        Lbl_Mobile.Text = "";
        Lbl_Total_Experience.Text = "";
        Div_Candidate_Info.Visible = false;
        Txt_Follow_Date.Text = "";
        Txt_Next_Follow_Date.Text = "";
        Txt_Title.Text = "";
        Txt_Description.Text = "";
        Chk_Next_Follow.Checked = false;
        Div_Next_Follow.Visible = false;
        //Gv_Follow_UP.DataSource = null;
        //Gv_Follow_UP.DataBind();
        Edit_ID.Value = "";
    }
    
    protected void Chk_Next_Follow_CheckedChanged(object sender, EventArgs e)
    {
        if(Edit_ID.Value!="")
        {
            if (Chk_Next_Follow.Checked == true)
            {
                Div_Next_Follow.Visible = true;
                Txt_Next_Follow_Date.Text = Hdn_Next_Follow_Date.Value;
                Req_NextFollow.ValidationGroup = "Save";
            }
            else
            {
                Div_Next_Follow.Visible = false;
                Txt_Next_Follow_Date.Text = "";
                Req_NextFollow.ValidationGroup = "Not_Save";
            }
        }
        else
        {
            if (Chk_Next_Follow.Checked == true)
            {
                Div_Next_Follow.Visible = true;
                Txt_Next_Follow_Date.Text = "";
                Req_NextFollow.ValidationGroup = "Save";
            }
            else
            {
                Div_Next_Follow.Visible = false;
                Txt_Next_Follow_Date.Text = "";
                Req_NextFollow.ValidationGroup = "Not_Save";
            }
        }
    }

    protected void Txt_Candidate_Name_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Candidate_Name.Text != "")
        {
            DataTable dt = objCandidate.GetAllTrueRecord();
            dt = new DataView(dt, "Candidate_Name like '" + Txt_Candidate_Name.Text.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                Txt_Candidate_Name.Text = dt.Rows[0]["Candidate_Name"].ToString();
                Hdn_Candidate_ID.Value = dt.Rows[0]["Candidate_Id"].ToString();

                DataTable Dt_Candi_Mstr = CandidateMaster.Get_Hr_Candidate_Master(dt.Rows[0]["Candidate_Id"].ToString(), "", "", "", "", "", "", "0", "", "", DateTime.Now.ToString("dd-MMM-yyyy"), "", "", "True", "True", "0", "True", "True", "0", "", "0", "", "", "", "", "", "", "", "", "", "", "True", "", "", "4");
                if (Dt_Candi_Mstr.Rows.Count > 0)
                {
                    Div_Candidate_Info.Visible = true;
                    Lbl_Candidate_Type.Text = Dt_Candi_Mstr.Rows[0]["Candidate_Type"].ToString();
                    Lbl_Email.Text = Dt_Candi_Mstr.Rows[0]["Email_Id1"].ToString();
                    Lbl_Father_Name.Text = Dt_Candi_Mstr.Rows[0]["F_H_Name"].ToString();
                    Lbl_Mobile.Text = Dt_Candi_Mstr.Rows[0]["Mobile_No1"].ToString();
                    Lbl_Total_Experience.Text = Dt_Candi_Mstr.Rows[0]["Total_Experience"].ToString();
                    //Fill_Grid();
                }
                else
                {
                    Div_Candidate_Info.Visible = false;
                    Lbl_Candidate_Type.Text = "";
                    Lbl_Email.Text = "";
                    Lbl_Father_Name.Text = "";
                    Lbl_Mobile.Text = "";
                    Lbl_Total_Experience.Text = "";
                    //Fill_Grid();
                }
            }
            else
            {
                Txt_Candidate_Name.Text = "";
                DisplayMessage("Select Candidate From Suggested List");
                return;
            }
        }
        else
        {
            Reset();
        }
    }

    public string GetDate(object obj)
    {
        if (obj != "")
        {
            DateTime Date = new DateTime();
            Date = Convert.ToDateTime(obj.ToString());
            return Date.ToString(objSys.SetDateFormat());
        }
        else
        {
            return "";
        }
    }

    //protected void Fill_Grid()
    //{
    //    DataTable Dt_HR_FollowUp = CandidateFollowUp.Get_HR_FollowUp("0", DateTime.Now.ToString(), "Candidate", "0", "", "", "0", "0", "", "", "", "", "", Hdn_Candidate_ID.Value, "True", "", "", "2");
    //    if (Dt_HR_FollowUp.Rows.Count > 0)
    //    {
    //        Session["Cd_Info_Dt_Filter"] = Dt_HR_FollowUp;
    //        Gv_Follow_UP.DataSource = Dt_HR_FollowUp;
    //        Gv_Follow_UP.DataBind();            
    //        //AllPageCode();
    //    }
    //    else
    //    {
    //        Session["Cd_Info_Dt_Filter"] = null;
    //        Gv_Follow_UP.DataSource = null;
    //        Gv_Follow_UP.DataBind();
    //    }
    //}

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCandidateName(string prefixText, int count, string contextKey)
    {
        HR_CandidateMaster objCandidate = new HR_CandidateMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objCandidate.GetAllTrueRecord();


        dt = new DataView(dt, "Candidate_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Candidate_Name"].ToString();
        }
        return txt;
    }

    //protected void Gv_Follow_UP_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Gv_Follow_UP.PageIndex = e.NewPageIndex;
    //        DataTable dt = new DataTable();
    //        if (Session["Cd_Info_Dt_Filter"] != null)
    //            dt = (DataTable)Session["Cd_Info_Dt_Filter"];
    //        objPageCmn.FillData((object)Gv_Follow_UP, dt, "", "");
    //        //AllPageCode();
    //        Gv_Follow_UP.HeaderRow.Focus();
    //    }
    //    catch
    //    {
    //    }
    //}

    //protected void Gv_Follow_UP_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        if (Session["Cd_Info_Dt_Filter"] != null)
    //            dt = (DataTable)Session["Cd_Info_Dt_Filter"];
            
    //        string sortdir = "DESC";
    //        if (ViewState["SortDir"] != null)
    //        {
    //            sortdir = ViewState["SortDir"].ToString();
    //            if (sortdir == "ASC")
    //            {
    //                e.SortDirection = SortDirection.Descending;
    //                ViewState["SortDir"] = "DESC";
    //            }
    //            else
    //            {
    //                e.SortDirection = SortDirection.Ascending;
    //                ViewState["SortDir"] = "ASC";
    //            }
    //        }
    //        else
    //        {
    //            ViewState["SortDir"] = "DESC";
    //        }
    //        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
    //        Session["Cd_Info_Dt_Filter"] = dt;
    //        objPageCmn.FillData((object)Gv_Follow_UP, dt, "", "");
    //        //AllPageCode();
    //        Gv_Follow_UP.HeaderRow.Focus();
    //    }
    //    catch
    //    {
    //    }
    //}
    
    //protected void Btn_Edit_Command(object sender, CommandEventArgs e)
    //{
    //    Edit_ID.Value = e.CommandArgument.ToString();
    //    DataTable dt = CandidateFollowUp.Get_HR_FollowUp(Edit_ID.Value, DateTime.Now.ToString(), "Candidate", "0", "", "", "0", "0", "", "", "", "", "", "", "True", "", "", "3");
    //    if (dt.Rows.Count > 0)
    //    {
    //        Txt_Follow_Date.Text = GetDate(dt.Rows[0]["Follow_Date"].ToString());
    //        Txt_Title.Text = dt.Rows[0]["Title"].ToString();
    //        Txt_Description.Text = dt.Rows[0]["Description"].ToString();
    //        if (dt.Rows[0]["Field6"].ToString() != "")
    //        {
    //            Chk_Next_Follow.Checked = true;
    //            Txt_Next_Follow_Date.Text = GetDate(dt.Rows[0]["Field6"].ToString());
    //            Div_Next_Follow.Visible = true;
    //        }
    //        else
    //        {
    //            Chk_Next_Follow.Checked = false;
    //            Txt_Next_Follow_Date.Text = "";
    //            Div_Next_Follow.Visible = false;
    //        }            
    //    }
    //}

    //protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    //{
    //    string Trans_Id = e.CommandArgument.ToString();
    //    int b = 0;
    //    String CompanyId = Session["CompId"].ToString().ToString();
    //    b = CandidateFollowUp.Set_HR_FollowUp(Trans_Id, DateTime.Now.ToString(), "0", "0", "", "", "0", "0", "", "", "", "", "", "", "False", "", "", "2");
    //    if (b != 0)
    //    {
    //        Fill_Grid();
    //        DisplayMessage("Record Deleted");
    //    }
    //    else
    //    {
    //        DisplayMessage("Record Not Deleted");
    //    }
    //}

    protected void Txt_Follow_Date_TextChanged(object sender, EventArgs e)
    {
        if (Txt_Follow_Date.Text != "")
        {
            if (Convert.ToDateTime(Txt_Follow_Date.Text) < DateTime.Now.AddDays(-1))
            {
                Txt_Follow_Date.Text = "";
                DisplayMessage("You cannot select a date earlier than Today!");
            }
        }
        if (Txt_Next_Follow_Date.Text != "")
        {
            if (Convert.ToDateTime(Txt_Next_Follow_Date.Text) < DateTime.Now.AddDays(-1))
            {
                Txt_Next_Follow_Date.Text = "";
                DisplayMessage("You cannot select a date earlier than Today!");
            }
        }
        if (Txt_Follow_Date.Text != "" && Txt_Next_Follow_Date.Text != "")
        {
            if (Convert.ToDateTime(Txt_Follow_Date.Text) > Convert.ToDateTime(Txt_Next_Follow_Date.Text))
            {
                Txt_Next_Follow_Date.Text = "";
                DisplayMessage("You cannot select a date earlier than Date!");
            }
        }

    }
    //--------------------------------------------


    protected void Btn_New_Li_Click(object sender, EventArgs e)
    {

    }
    
    

    
}