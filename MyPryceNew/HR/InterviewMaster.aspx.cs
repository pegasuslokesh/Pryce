using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_InterviewMaster : System.Web.UI.Page
{
    HR_CandidateMaster objCandidate = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    Interview_Master Interview_Master = null;
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

            objCandidate = new HR_CandidateMaster(Session["DBConnection"].ToString());
            objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            Interview_Master = new Interview_Master(Session["DBConnection"].ToString());
            Jobs = new Jobs(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            Page.Title = objSys.GetSysTitle();
            txtInterviewDate_CalendarExtender.Format = objSys.SetDateFormat();
            if (!IsPostBack)
            {
              
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/InterviewMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                Fill_Grid_List();
                Fill_Grid_Bin();
                Fill_Profile();
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

    public void Fill_Profile()
    {

        DataTable Dt_Job_Open = Jobs.Get_Hr_JobOpenings("0", Session["CompId"].ToString(), "0", "0", "", "", "", "", "", "", "", "", "", "", "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), "", "0", "4");
        if (Dt_Job_Open.Rows.Count > 0)
        {
            DDL_Profile.DataSource = Dt_Job_Open;
            DDL_Profile.DataBind();
            objPageCmn.FillData((object)DDL_Profile, Dt_Job_Open, "Profile_Name", "Trans_ID");
        }
        else
        {
            try
            {
                DDL_Profile.Items.Clear();
                DDL_Profile.DataSource = null;
                DDL_Profile.DataBind();
            }
            catch
            {

            }
        }
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
            Session["In_Ms_CHECKED_ITEMS_LIST"] = null;
            Session["In_Ms_Dt_Filter"] = null;
            DataTable Dt_Interview_Master = Interview_Master.Get_Hr_Interview_Master("0", "0", "0", DateTime.Now.ToString(), "0", "0", "0", "", "", "0", "0", "True", "True", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
            Session["In_Ms_Dt_Interview_Master_List"] = Dt_Interview_Master;

            DataTable Dt_Interview_Master_List = new DataView(Session["In_Ms_Dt_Interview_Master_List"] as DataTable, "Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
            Session["In_Ms_Dt_Interview_Master_List_Active"] = Dt_Interview_Master_List;
            if (Dt_Interview_Master_List.Rows.Count > 0)
            {
                Fill_Gv_Interview_Master_Bin(Dt_Interview_Master_List);
            }
            else
            {
                Fill_Gv_Interview_Master_Bin(Dt_Interview_Master_List);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fill_Gv_Interview_Master_Bin(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Interview_Master_List.DataSource = Dt_Grid;
            Gv_Interview_Master_List.DataBind();
            //AllPageCode();
        }
        else
        {
            Gv_Interview_Master_List.DataSource = null;
            Gv_Interview_Master_List.DataBind();
            
        }
    }

    private void Save_Checked_Interview_Master_Master()
    {
        ArrayList Interview_Master_Array = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Interview_Master_List.Rows)
        {
            index = (int)Gv_Interview_Master_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["In_Ms_CHECKED_ITEMS_LIST"] != null)
                Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Interview_Master_Array.Contains(index))
                    Interview_Master_Array.Add(index);
            }
            else
                Interview_Master_Array.Remove(index);
        }
        if (Interview_Master_Array != null && Interview_Master_Array.Count > 0)
            Session["In_Ms_CHECKED_ITEMS_LIST"] = Interview_Master_Array;
    }

    protected void Populate_Checked_Interview_Master_Master()
    {
        ArrayList Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_LIST"];
        if (Interview_Master_Array != null && Interview_Master_Array.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Interview_Master_List.Rows)
            {
                int index = (int)Gv_Interview_Master_List.DataKeys[gvrow.RowIndex].Value;
                if (Interview_Master_Array.Contains(index))
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
            Session["In_Ms_Dt_Filter"] = null;
            Save_Checked_Interview_Master_Master();
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
                DataTable Dt_Interview_Master_List = (DataTable)Session["In_Ms_Dt_Interview_Master_List_Active"];
                DataView view = new DataView(Dt_Interview_Master_List, condition, "", DataViewRowState.CurrentRows);
                Session["In_Ms_Dt_Filter"] = view.ToTable();
                Fill_Gv_Interview_Master_Bin(view.ToTable());

            }
            Populate_Checked_Interview_Master_Master();
        }
        catch
        {
        }
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["In_Ms_CHECKED_ITEMS_LIST"] = null;
            Session["In_Ms_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Interview_Master_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Interview_Master_Bin(Session["In_Ms_Dt_Interview_Master_List_Active"] as DataTable);
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["In_Ms_Dt_Interview_Master_List_Active"];
        ArrayList Interview_Master_Array = new ArrayList();
        Session["In_Ms_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["In_Ms_CHECKED_ITEMS_LIST"] != null)
                {
                    Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_LIST"];
                }
                if (!Interview_Master_Array.Contains(dr["Interview_Id"]))
                {
                    Interview_Master_Array.Add(dr["Interview_Id"]);
                }
            }
            foreach (GridViewRow GR in Gv_Interview_Master_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Interview_Master_Array.Count > 0 && Interview_Master_Array != null)
            {
                Session["In_Ms_CHECKED_ITEMS_LIST"] = Interview_Master_Array;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["In_Ms_Dt_Interview_Master_List_Active"];
            objPageCmn.FillData((object)Gv_Interview_Master_List, dt, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
           
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        ArrayList Interview_Master_Array = new ArrayList();
        if (Gv_Interview_Master_List.Rows.Count > 0)
        {
            Save_Checked_Interview_Master_Master();
            if (Session["In_Ms_CHECKED_ITEMS_LIST"] != null)
            {
                Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_LIST"];
                if (Interview_Master_Array.Count > 0)
                {
                    for (int j = 0; j < Interview_Master_Array.Count; j++)
                    {
                        if (Interview_Master_Array[j].ToString() != "")
                        {
                            b = Interview_Master.Set_Hr_Interview_Master(Interview_Master_Array[j].ToString(), "0", "0", DateTime.Now.ToString(), "0", "0", "0", "", "", "0", "0", "True", "True", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["In_Ms_CHECKED_ITEMS_LIST"] = null;
                    Session["In_Ms_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["In_Ms_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Interview_Master_List.Rows)
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
                Gv_Interview_Master_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Interview_Master_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Interview_Master_List.Rows)
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
        string Interview_Id = e.CommandArgument.ToString();
        Edit_ID.Value = e.CommandArgument.ToString();
        DataTable Dt_Interview_Master_List = Session["In_Ms_Dt_Interview_Master_List_Active"] as DataTable;
        Dt_Interview_Master_List = new DataView(Dt_Interview_Master_List, "Interview_Id = " + Interview_Id + " And Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt_Interview_Master_List.Rows.Count > 0)
        {
            Hdn_Candidate_ID.Value = Dt_Interview_Master_List.Rows[0]["Candidate_Id"].ToString();
            Txt_CandidateName.Text = Dt_Interview_Master_List.Rows[0]["Candidate_Name"].ToString();
            if (Dt_Interview_Master_List.Rows[0]["Vacancy_Id"].ToString() == "0")
                DDL_Profile.SelectedValue = "--Select--";
            else
                DDL_Profile.SelectedValue = Dt_Interview_Master_List.Rows[0]["Vacancy_Id"].ToString();

            Txt_InterviewerName.Text = Dt_Interview_Master_List.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Interview_Master_List.Rows[0]["Emp_Code"].ToString();
            Hdn_Interview_ID.Value = Dt_Interview_Master_List.Rows[0]["Emp_Id"].ToString();
            Txt_InterviewDate.Text = GetDate(Dt_Interview_Master_List.Rows[0]["Interview_Date"].ToString());
            Txt_ExpectedSalary.Text = Dt_Interview_Master_List.Rows[0]["Expected_Salary"].ToString();
            Txt_NoticePeriod.Text = Dt_Interview_Master_List.Rows[0]["Notice_Period"].ToString();
            Txt_Title.Text = Dt_Interview_Master_List.Rows[0]["Title"].ToString();
            Txt_Feedback.Text = Dt_Interview_Master_List.Rows[0]["Feedback"].ToString();
            DDL_Communication.SelectedValue = Dt_Interview_Master_List.Rows[0]["Communication_Skill"].ToString();
            DDL_SubjectKnowledge.SelectedValue = Dt_Interview_Master_List.Rows[0]["Subject_Knowledge"].ToString();
            Chk_IsEmployee.Checked = Convert.ToBoolean(Dt_Interview_Master_List.Rows[0]["Is_Employee"].ToString());
            Chk_IsSelect.Checked = Convert.ToBoolean(Dt_Interview_Master_List.Rows[0]["Is_Selected"].ToString());
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Interview_Id = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = Interview_Master.Set_Hr_Interview_Master(Interview_Id, "0", "0", DateTime.Now.ToString(), "0", "0", "0", "", "", "0", "0", "True", "True", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
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

    protected void Gv_Interview_Master_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Interview_Master_Master();
            Gv_Interview_Master_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["In_Ms_Dt_Filter"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Filter"];
            else if (Session["In_Ms_Dt_Interview_Master_List_Active"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Interview_Master_List_Active"];
            objPageCmn.FillData((object)Gv_Interview_Master_List, dt, "", "");
            //AllPageCode();
            Gv_Interview_Master_List.HeaderRow.Focus();
            Populate_Checked_Interview_Master_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Interview_Master_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Interview_Master_Master();
            DataTable dt = new DataTable();
            if (Session["In_Ms_Dt_Filter"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Filter"];
            else if (Session["In_Ms_Dt_Interview_Master_List_Active"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Interview_Master_List_Active"];

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
            Session["In_Ms_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Interview_Master_List, dt, "", "");
            //AllPageCode();
            Gv_Interview_Master_List.HeaderRow.Focus();
            Populate_Checked_Interview_Master_Master();
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
            Session["In_Ms_CHECKED_ITEMS_BIN"] = null;
            Session["In_Ms_Dt_Filter_Bin"] = null;
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
            DataTable Dt_Interview_Master_Bin = Interview_Master.Get_Hr_Interview_Master("0", "0", "0", DateTime.Now.ToString(), "0", "0", "0", "", "", "0", "0", "True", "True", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
            Dt_Interview_Master_Bin = new DataView(Dt_Interview_Master_Bin, "Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();
            Session["In_Ms_Dt_Interview_Master_Bin_InActive"] = Dt_Interview_Master_Bin;
            if (Dt_Interview_Master_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Interview_Master_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Interview_Master_Bin);
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
            Gv_Interview_Master_Bin.DataSource = Dt_Grid;
            Gv_Interview_Master_Bin.DataBind();
            //AllPageCode();
            
            Img_Emp_List_Active.Visible = true;
        }
        else
        {
            Gv_Interview_Master_Bin.DataSource = Dt_Grid;
            Gv_Interview_Master_Bin.DataBind();
            //AllPageCode();
            Img_Emp_List_Active.Visible = false;
        }
    }

    private void Save_Checked_Interview_Master_Master_Bin()
    {
        ArrayList Interview_Master_Array = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Interview_Master_Bin.Rows)
        {
            index = (int)Gv_Interview_Master_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["In_Ms_CHECKED_ITEMS_BIN"] != null)
                Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Interview_Master_Array.Contains(index))
                    Interview_Master_Array.Add(index);
            }
            else
                Interview_Master_Array.Remove(index);
        }
        if (Interview_Master_Array != null && Interview_Master_Array.Count > 0)
            Session["In_Ms_CHECKED_ITEMS_BIN"] = Interview_Master_Array;
    }

    protected void Populate_Checked_Interview_Master_Master_Bin()
    {
        ArrayList Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_BIN"];
        if (Interview_Master_Array != null && Interview_Master_Array.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Interview_Master_Bin.Rows)
            {
                int index = (int)Gv_Interview_Master_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Interview_Master_Array.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["In_Ms_Dt_Filter_Bin"] = null;
            Save_Checked_Interview_Master_Master_Bin();
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
                DataTable Dt_Interview_Master_List = (DataTable)Session["In_Ms_Dt_Interview_Master_Bin_InActive"];
                DataView view = new DataView(Dt_Interview_Master_List, condition, "", DataViewRowState.CurrentRows);
                Session["In_Ms_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());

            }
            Populate_Checked_Interview_Master_Master_Bin();
        }
        catch
        {
        }
        txtValueBin.Focus();
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        try
        {
            Session["In_Ms_CHECKED_ITEMS_BIN"] = null;
            Session["In_Ms_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Interview_Master_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["In_Ms_Dt_Interview_Master_Bin_InActive"] as DataTable);
            txtValueBin.Text = "";
            ddlOptionBin.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["In_Ms_Dt_Interview_Master_Bin_InActive"];
        ArrayList Interview_Master_Array = new ArrayList();
        Session["In_Ms_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["In_Ms_CHECKED_ITEMS_BIN"] != null)
                {
                    Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_BIN"];
                }
                if (!Interview_Master_Array.Contains(dr["Interview_Id"]))
                {
                    Interview_Master_Array.Add(dr["Interview_Id"]);
                }
            }
            foreach (GridViewRow GR in Gv_Interview_Master_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Interview_Master_Array.Count > 0 && Interview_Master_Array != null)
            {
                Session["In_Ms_CHECKED_ITEMS_BIN"] = Interview_Master_Array;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["In_Ms_Dt_Interview_Master_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Interview_Master_Bin, dt, "", "");
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
        ArrayList Interview_Master_Array = new ArrayList();
        if (Gv_Interview_Master_Bin.Rows.Count > 0)
        {
            Save_Checked_Interview_Master_Master_Bin();
            if (Session["In_Ms_CHECKED_ITEMS_BIN"] != null)
            {
                Interview_Master_Array = (ArrayList)Session["In_Ms_CHECKED_ITEMS_BIN"];
                if (Interview_Master_Array.Count > 0)
                {
                    for (int j = 0; j < Interview_Master_Array.Count; j++)
                    {
                        if (Interview_Master_Array[j].ToString() != "")
                        {
                            b = Interview_Master.Set_Hr_Interview_Master(Interview_Master_Array[j].ToString(), "0", "0", DateTime.Now.ToString(), "0", "0", "0", "", "", "0", "0", "True", "True", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["In_Ms_CHECKED_ITEMS_BIN"] = null;
                    Session["In_Ms_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["In_Ms_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Interview_Master_Bin.Rows)
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
                Gv_Interview_Master_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Interview_Master_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Interview_Master_Bin.Rows)
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
        string Interview_Id = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = Interview_Master.Set_Hr_Interview_Master(Interview_Id, "0", "0", DateTime.Now.ToString(), "0", "0", "0", "", "", "0", "0", "True", "True", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
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

    protected void Gv_Interview_Master_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Interview_Master_Master_Bin();
            Gv_Interview_Master_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["In_Ms_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Filter_Bin"];
            else if (Session["In_Ms_Dt_Interview_Master_Bin_InActive"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Interview_Master_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Interview_Master_Bin, dt, "", "");
            //AllPageCode();
            Gv_Interview_Master_Bin.HeaderRow.Focus();
            Populate_Checked_Interview_Master_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Interview_Master_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Interview_Master_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["In_Ms_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Filter_Bin"];
            else if (Session["In_Ms_Dt_Interview_Master_Bin_InActive"] != null)
                dt = (DataTable)Session["In_Ms_Dt_Interview_Master_Bin_InActive"];

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
            Session["In_Ms_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Interview_Master_Bin, dt, "", "");
            //AllPageCode();
            Gv_Interview_Master_Bin.HeaderRow.Focus();
            Populate_Checked_Interview_Master_Master_Bin();
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
            if (Txt_InterviewerName.Text == "")
            {
                DisplayMessage("Enter Interviewer Name");
                return;
            }
            //else if (DDL_Profile.SelectedValue== "--Select--")
            //{
            //    DisplayMessage("Select Vacancy Name");
            //    return;
            //}
            else if (Txt_InterviewDate.Text == "")
            {
                DisplayMessage("Enter Interview Date");
                return;
            }
            else if (Txt_ExpectedSalary.Text == "")
            {
                DisplayMessage("Enter Expected Salary");
                return;
            }
            else if (Txt_Title.Text == "")
            {
                DisplayMessage("Enter Title");
                return;
            }
            else if (Txt_Feedback.Text == "")
            {
                DisplayMessage("Enter Feedback");
                return;
            }
            else if (DDL_Communication.SelectedValue == "--Select--")
            {
                DisplayMessage("Select Communication Skill");
                return;
            }
            else if (DDL_SubjectKnowledge.SelectedValue == "--Select--")
            {
                DisplayMessage("Select Subject Knowledge");
                return;
            }
            else
            {
                string Profile = string.Empty;
                if (DDL_Profile.SelectedValue == "--Select--")
                    Profile = "0";
                else if (DDL_Profile.SelectedValue == "")
                    Profile = "0";
                else
                    Profile = DDL_Profile.SelectedValue.ToString();
                if (Edit_ID.Value == "")
                {
                    int b = 0;
                    b = Interview_Master.Set_Hr_Interview_Master("0", Hdn_Candidate_ID.Value, Profile, Txt_InterviewDate.Text, Hdn_Interview_ID.Value, Txt_ExpectedSalary.Text, Txt_NoticePeriod.Text, Txt_Title.Text, Txt_Feedback.Text, DDL_Communication.SelectedValue, DDL_SubjectKnowledge.SelectedValue, Chk_IsSelect.Checked.ToString(), "False", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                    if (b != 0)
                    {
                        DisplayMessage("Record Saved","green");
                        Reset();
                        Hdn_Candidate_ID.Value = "";
                        Hdn_Interview_ID.Value = "";
                        Edit_ID.Value = "";
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
                    b = Interview_Master.Set_Hr_Interview_Master(Edit_ID.Value, Hdn_Candidate_ID.Value, Profile, Txt_InterviewDate.Text, Hdn_Interview_ID.Value, Txt_ExpectedSalary.Text, Txt_NoticePeriod.Text, Txt_Title.Text, Txt_Feedback.Text, DDL_Communication.SelectedValue, DDL_SubjectKnowledge.SelectedValue, Chk_IsSelect.Checked.ToString(), "False", "", "", "", "", "", "", "", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
                    if (b != 0)
                    {
                        DisplayMessage("Record Updated", "green");
                        Reset();
                        Hdn_Candidate_ID.Value = "";
                        Hdn_Interview_ID.Value = "";
                        Edit_ID.Value = "";
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
        }
        catch (Exception ex)
        {
        }
    }

    protected void Reset()
    {
        try
        {
            Txt_CandidateName.Text = "";
            Txt_InterviewerName.Text = "";
            Txt_InterviewDate.Text = "";
            Txt_ExpectedSalary.Text = "";
            Txt_NoticePeriod.Text = "";
            Txt_Title.Text = "";
            Txt_Feedback.Text = "";
            DDL_Communication.SelectedValue = "--Select--";
            DDL_SubjectKnowledge.SelectedValue = "--Select--";
            Chk_IsEmployee.Checked = false;
            Chk_IsSelect.Checked = false;
            Lbl_Tab_New.Text = Resources.Attendance.New;
            DDL_Profile.SelectedValue = "--Select--";
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

    protected void Txt_CandidateName_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = objCandidate.GetAllTrueRecord();
        dt = new DataView(dt, "Candidate_Name like '" + Txt_CandidateName.Text.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            Txt_CandidateName.Text = dt.Rows[0]["Candidate_Name"].ToString();
            Hdn_Candidate_ID.Value = dt.Rows[0]["Candidate_Id"].ToString();
            DDL_Profile.Focus();
        }
        else
        {
            Txt_CandidateName.Text = "";
            DisplayMessage("Select Candidate From Suggested List");
            return;
        }
    }

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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetInterviewerName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
        }
        return txt;
    }

    protected void Txt_InterviewerName_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = objEmp.GetEmployeeMasterByEmpName(Session["CompId"].ToString().ToString(), Txt_InterviewerName.Text.Trim().Split('/')[0].ToString());
        if (dt.Rows.Count > 0)
        {
            Txt_InterviewerName.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Code"].ToString();
            Hdn_Interview_ID.Value = dt.Rows[0]["Emp_Id"].ToString();
            Txt_InterviewDate.Focus();
        }
        else
        {
            Txt_InterviewerName.Text = "";
            DisplayMessage("Select Interviewer From Suggested List");
            return;
        }
    }

    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }

    protected void DDL_Profile_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Opening_From_Date"] = null;
        Session["Opening_To_Date"] = null;
        if (DDL_Profile.SelectedValue == "--Select--")
        {
            Session["Opening_From_Date"] = "";
            Session["Opening_To_Date"] = "";
        }
        else
        {
            string Opening_Date = DDL_Profile.SelectedItem.ToString();
            int start = Opening_Date.IndexOf("(") + 1;
            int end = Opening_Date.IndexOf(")", start);
            string Emp_ID = Opening_Date.Substring(start, end - start);
            string[] Dates = Emp_ID.Split('-');
            Session["Opening_From_Date"] = GetDate(Dates[0].ToString());
            Session["Opening_To_Date"] = GetDate(Dates[1].ToString());
        }
    }

    protected void Txt_InterviewDate_TextChanged(object sender, EventArgs e)
    {
        if (DDL_Profile.SelectedValue != "--Select--")
        {
            if (Txt_InterviewDate.Text != "")
            {
                if (Convert.ToDateTime(Txt_InterviewDate.Text) < Convert.ToDateTime(Session["Opening_From_Date"].ToString()))
                {
                    Txt_InterviewDate.Text = "";
                    DisplayMessage("You cannot select a date earlier than Job Opening Date!");
                }
                else if (Convert.ToDateTime(Txt_InterviewDate.Text) > Convert.ToDateTime(Session["Opening_To_Date"].ToString()).AddDays(10))
                {
                    Txt_InterviewDate.Text = "";
                    DisplayMessage("You cannot select a date after " + GetDate(Convert.ToDateTime(Session["Opening_To_Date"].ToString()).AddDays(10)) + "!");
                }
            }
        }
        else
        {
            if (Txt_InterviewDate.Text != "")
            {
                if (Convert.ToDateTime(Txt_InterviewDate.Text) < DateTime.Now.AddDays(-1))
                {
                    Txt_InterviewDate.Text = "";
                    DisplayMessage("You cannot select a date earlier than Today!");
                }
            }
        }
    }
}