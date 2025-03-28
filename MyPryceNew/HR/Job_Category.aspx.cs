using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_Job_Category : System.Web.UI.Page
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
            Page.Title = objSys.GetSysTitle();
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

            if (!IsPostBack)
            {
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../HR/Job_Category.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
            Session["JC_CHECKED_ITEMS_LIST"] = null;
            Session["JC_Dt_Filter"] = null;
            DataTable Dt_Job_Cat = Jobs.Get_Hr_JobCategory("0", Session["CompId"].ToString(), "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
            Session["JC_Dt_Job_Cat_List"] = Dt_Job_Cat;

            DataTable Dt_Job_Cat_List = new DataView(Session["JC_Dt_Job_Cat_List"] as DataTable, "Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
            Session["JC_Dt_Job_Cat_List_Active"] = Dt_Job_Cat_List;
            if (Dt_Job_Cat_List.Rows.Count > 0)
            {
                Fill_Gv_Job_Cat(Dt_Job_Cat_List);
            }
            else
            {
                Fill_Gv_Job_Cat(Dt_Job_Cat_List);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Job_Cat(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Job_Cat_List.DataSource = Dt_Grid;
            Gv_Job_Cat_List.DataBind();
        }
        else
        {
            Gv_Job_Cat_List.DataSource = null;
            Gv_Job_Cat_List.DataBind();
        }
    }

    private void Save_Checked_Job_Cat_Master()
    {
        ArrayList Job_Categorys = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Job_Cat_List.Rows)
        {
            index = (int)Gv_Job_Cat_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["JC_CHECKED_ITEMS_LIST"] != null)
                Job_Categorys = (ArrayList)Session["JC_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Job_Categorys.Contains(index))
                    Job_Categorys.Add(index);
            }
            else
                Job_Categorys.Remove(index);
        }
        if (Job_Categorys != null && Job_Categorys.Count > 0)
            Session["JC_CHECKED_ITEMS_LIST"] = Job_Categorys;
    }

    protected void Populate_Checked_Job_Cat_Master()
    {
        ArrayList Job_Categorys = (ArrayList)Session["JC_CHECKED_ITEMS_LIST"];
        if (Job_Categorys != null && Job_Categorys.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Job_Cat_List.Rows)
            {
                int index = (int)Gv_Job_Cat_List.DataKeys[gvrow.RowIndex].Value;
                if (Job_Categorys.Contains(index))
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
            Session["JC_Dt_Filter"] = null;
            Save_Checked_Job_Cat_Master();
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
                DataTable Dt_Job_Cat_List = (DataTable)Session["JC_Dt_Job_Cat_List_Active"];
                DataView view = new DataView(Dt_Job_Cat_List, condition, "", DataViewRowState.CurrentRows);
                Session["JC_Dt_Filter"] = view.ToTable();
                Fill_Gv_Job_Cat(view.ToTable());

            }
            Populate_Checked_Job_Cat_Master();
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
            Session["JC_CHECKED_ITEMS_LIST"] = null;
            Session["JC_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Job_Cat_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Job_Cat(Session["JC_Dt_Job_Cat_List_Active"] as DataTable);
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["JC_Dt_Job_Cat_List_Active"];
        ArrayList Job_Categorys = new ArrayList();
        Session["JC_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["JC_CHECKED_ITEMS_LIST"] != null)
                {
                    Job_Categorys = (ArrayList)Session["JC_CHECKED_ITEMS_LIST"];
                }
                if (!Job_Categorys.Contains(dr["Trans_ID"]))
                {
                    Job_Categorys.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Job_Cat_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Job_Categorys.Count > 0 && Job_Categorys != null)
            {
                Session["JC_CHECKED_ITEMS_LIST"] = Job_Categorys;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["JC_Dt_Job_Cat_List_Active"];
            objPageCmn.FillData((object)Gv_Job_Cat_List, dt, "", "");
            ViewState["Select"] = null;
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        ArrayList Job_Category = new ArrayList();
        if (Gv_Job_Cat_List.Rows.Count > 0)
        {
            Save_Checked_Job_Cat_Master();
            if (Session["JC_CHECKED_ITEMS_LIST"] != null)
            {
                Job_Category = (ArrayList)Session["JC_CHECKED_ITEMS_LIST"];
                if (Job_Category.Count > 0)
                {
                    for (int j = 0; j < Job_Category.Count; j++)
                    {
                        if (Job_Category[j].ToString() != "")
                        {
                            b = Jobs.Set_Hr_JobCategory(Job_Category[j].ToString(), Session["CompId"].ToString(), "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["JC_CHECKED_ITEMS_LIST"] = null;
                    Session["JC_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["JC_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Job_Cat_List.Rows)
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
                Gv_Job_Cat_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Job_Cat_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Job_Cat_List.Rows)
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
        GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        string Trans_ID = e.CommandArgument.ToString();
        Edit_ID.Value = e.CommandArgument.ToString();
        DataTable Dt_Job_Cat_List = Session["JC_Dt_Job_Cat_List_Active"] as DataTable;
        Dt_Job_Cat_List = new DataView(Dt_Job_Cat_List, "Trans_ID = " + Trans_ID + " And Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt_Job_Cat_List.Rows.Count > 0)
        {
            Txt_Category.Text = Dt_Job_Cat_List.Rows[0]["Category"].ToString();
            Txt_Description.Text = Dt_Job_Cat_List.Rows[0]["Description"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        String CompanyId = Session["CompId"].ToString().ToString();
        b = Jobs.Set_Hr_JobCategory(Trans_ID, Session["CompId"].ToString(), "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
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

    protected void Gv_Job_Cat_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Job_Cat_Master();
            Gv_Job_Cat_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["JC_Dt_Filter"] != null)
                dt = (DataTable)Session["JC_Dt_Filter"];
            else if (Session["JC_Dt_Job_Cat_List_Active"] != null)
                dt = (DataTable)Session["JC_Dt_Job_Cat_List_Active"];
            objPageCmn.FillData((object)Gv_Job_Cat_List, dt, "", "");
            //AllPageCode();
            Gv_Job_Cat_List.HeaderRow.Focus();
            Populate_Checked_Job_Cat_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Job_Cat_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Job_Cat_Master();
            DataTable dt = new DataTable();
            if (Session["JC_Dt_Filter"] != null)
                dt = (DataTable)Session["JC_Dt_Filter"];
            else if (Session["JC_Dt_Job_Cat_List_Active"] != null)
                dt = (DataTable)Session["JC_Dt_Job_Cat_List_Active"];

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
            Session["JC_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Job_Cat_List, dt, "", "");
            //AllPageCode();
            Gv_Job_Cat_List.HeaderRow.Focus();
            Populate_Checked_Job_Cat_Master();
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
            Session["JC_CHECKED_ITEMS_BIN"] = null;
            Session["JC_Dt_Filter_Bin"] = null;
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
            DataTable Dt_Job_Cat_Bin = Jobs.Get_Hr_JobCategory("0", Session["CompId"].ToString(), "", "", "False", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
            Dt_Job_Cat_Bin = new DataView(Dt_Job_Cat_Bin, "Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();
            Session["JC_Dt_Job_Cat_Bin_InActive"] = Dt_Job_Cat_Bin;
            if (Dt_Job_Cat_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Job_Cat_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Job_Cat_Bin);
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
            Gv_Job_Cat_Bin.DataSource = Dt_Grid;
            Gv_Job_Cat_Bin.DataBind();
            //AllPageCode();
            Img_Emp_List_Active.Visible = true;
        }
        else
        {
            Gv_Job_Cat_Bin.DataSource = Dt_Grid;
            Gv_Job_Cat_Bin.DataBind();
            //AllPageCode();
            Img_Emp_List_Active.Visible = false;
        }
    }

    private void Save_Checked_Job_Cat_Master_Bin()
    {
        ArrayList Job_Categorys = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Job_Cat_Bin.Rows)
        {
            index = (int)Gv_Job_Cat_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["JC_CHECKED_ITEMS_BIN"] != null)
                Job_Categorys = (ArrayList)Session["JC_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Job_Categorys.Contains(index))
                    Job_Categorys.Add(index);
            }
            else
                Job_Categorys.Remove(index);
        }
        if (Job_Categorys != null && Job_Categorys.Count > 0)
            Session["JC_CHECKED_ITEMS_BIN"] = Job_Categorys;
    }

    protected void Populate_Checked_Job_Cat_Master_Bin()
    {
        ArrayList Job_Categorys = (ArrayList)Session["JC_CHECKED_ITEMS_BIN"];
        if (Job_Categorys != null && Job_Categorys.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Job_Cat_Bin.Rows)
            {
                int index = (int)Gv_Job_Cat_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Job_Categorys.Contains(index))
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
            Session["JC_Dt_Filter_Bin"] = null;
            Save_Checked_Job_Cat_Master_Bin();
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
                DataTable Dt_Job_Cat_Bin = (DataTable)Session["JC_Dt_Job_Cat_Bin_InActive"];
                DataView view = new DataView(Dt_Job_Cat_Bin, condition, "", DataViewRowState.CurrentRows);
                Session["JC_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                txtValueBin.Focus();
            }
            Populate_Checked_Job_Cat_Master_Bin();
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
            Session["JC_CHECKED_ITEMS_BIN"] = null;
            Session["JC_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Job_Cat_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["JC_Dt_Job_Cat_Bin_InActive"] as DataTable);
            txtValueBin.Text = "";
            ddlOptionBin.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["JC_Dt_Job_Cat_Bin_InActive"];
        ArrayList Job_Categorys = new ArrayList();
        Session["JC_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["JC_CHECKED_ITEMS_BIN"] != null)
                {
                    Job_Categorys = (ArrayList)Session["JC_CHECKED_ITEMS_BIN"];
                }
                if (!Job_Categorys.Contains(dr["Trans_ID"]))
                {
                    Job_Categorys.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Job_Cat_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Job_Categorys.Count > 0 && Job_Categorys != null)
            {
                Session["JC_CHECKED_ITEMS_BIN"] = Job_Categorys;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["JC_Dt_Job_Cat_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Job_Cat_Bin, dt, "", "");
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
        ArrayList Job_Category = new ArrayList();
        if (Gv_Job_Cat_Bin.Rows.Count > 0)
        {
            Save_Checked_Job_Cat_Master_Bin();
            if (Session["JC_CHECKED_ITEMS_BIN"] != null)
            {
                Job_Category = (ArrayList)Session["JC_CHECKED_ITEMS_BIN"];
                if (Job_Category.Count > 0)
                {
                    for (int j = 0; j < Job_Category.Count; j++)
                    {
                        if (Job_Category[j].ToString() != "")
                        {
                            b = Jobs.Set_Hr_JobCategory(Job_Category[j].ToString(), Session["CompId"].ToString(), "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["JC_CHECKED_ITEMS_BIN"] = null;
                    Session["JC_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["JC_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Job_Cat_Bin.Rows)
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
                Gv_Job_Cat_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Job_Cat_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Job_Cat_Bin.Rows)
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
        b = Jobs.Set_Hr_JobCategory(Trans_ID, Session["CompId"].ToString(), "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "2");
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

    protected void Gv_Job_Cat_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Job_Cat_Master_Bin();
            Gv_Job_Cat_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["JC_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["JC_Dt_Filter_Bin"];
            else if (Session["JC_Dt_Job_Cat_Bin_InActive"] != null)
                dt = (DataTable)Session["JC_Dt_Job_Cat_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Job_Cat_Bin, dt, "", "");
            //AllPageCode();
            Gv_Job_Cat_Bin.HeaderRow.Focus();
            Populate_Checked_Job_Cat_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Job_Cat_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Job_Cat_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["JC_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["JC_Dt_Filter_Bin"];
            else if (Session["JC_Dt_Job_Cat_Bin_InActive"] != null)
                dt = (DataTable)Session["JC_Dt_Job_Cat_Bin_InActive"];

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
            Session["JC_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Job_Cat_Bin, dt, "", "");
            //AllPageCode();
            Gv_Job_Cat_Bin.HeaderRow.Focus();
            Populate_Checked_Job_Cat_Master_Bin();
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
            //DataTable Dt_Job_Cat_Inactive = new DataTable();
            //DataTable Dt_Job_Cat_Active = new DataTable();
            //DataTable Dt_Job_Cat_List = Jobs.Get_Hr_JobCategory("0", Session["CompId"].ToString(), "", "", "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
            //if (Dt_Job_Cat_List.Rows.Count > 0)
            //    Dt_Job_Cat_Active = new DataView(Dt_Job_Cat_List, "Category = '" + Txt_Category.Text.Trim() + "' And Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();

            //else if (Dt_Job_Cat_List.Rows.Count > 0)
            //    Dt_Job_Cat_Inactive = new DataView(Dt_Job_Cat_List, "Category = '" + Txt_Category.Text.Trim() + "' And Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();

            if (Txt_Category.Text == "")
            {
                DisplayMessage("Enter Category");
                return;
            }
            else if (Txt_Description.Text == "")
            {
                DisplayMessage("Enter Description");
                return;
            }
            //else if(Dt_Job_Cat_Active.Rows.Count>0)
            //{
            //    DisplayMessage("Record Already Exists");
            //    return;
            //}
            //else if (Dt_Job_Cat_Inactive.Rows.Count > 0)
            //{
            //    DisplayMessage("Record Already Exists in Bin");
            //    return;
            //}
            else
            {
                if (Edit_ID.Value == "")
                {
                    int b = 0;
                    b = Jobs.Set_Hr_JobCategory("0", Session["CompId"].ToString(), Txt_Category.Text, Txt_Description.Text, "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "1");
                    if (b != 0)
                    {
                        DisplayMessage("Record Saved","green");
                        Reset();
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
                    b = Jobs.Set_Hr_JobCategory(Edit_ID.Value, Session["CompId"].ToString(), Txt_Category.Text, Txt_Description.Text, "True", Session["UserID"].ToString(), Session["UserID"].ToString(), "3");
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
        }
        catch (Exception ex)
        {
        }
    }

    protected void Reset()
    {
        try
        {
            Txt_Category.Text = "";
            Txt_Description.Text = "";
            Edit_ID.Value = "";
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
        DataTable Dt_Job_Cat_List = new DataView(Session["JC_Dt_Job_Cat_List"] as DataTable, "Category='" + Txt_Category.Text.Trim() + "' and Is_Active='True'", "", DataViewRowState.CurrentRows).ToTable();

        DataTable Dt_Job_Cat_Bin = new DataView(Session["JC_Dt_Job_Cat_List"] as DataTable, "Category='" + Txt_Category.Text.Trim() + "' and Is_Active='False'", "", DataViewRowState.CurrentRows).ToTable();

        if (Dt_Job_Cat_List.Rows.Count > 0)
        {
            Txt_Category.Text = "";
            counter = 1;
            DisplayMessage("Record Already Exists");
        }
        else if (Dt_Job_Cat_Bin.Rows.Count > 0)
        {
            Txt_Category.Text = "";
            counter = 1;
            DisplayMessage("Record Already Exists in Bin");
        }

        if (counter == 0)
            Txt_Description.Focus();
        else
            Txt_Category.Focus();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Category(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        Jobs Jobs = new Jobs(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(Jobs.Get_Hr_JobCategory("0", HttpContext.Current.Session["CompId"].ToString(), "", "", "True", HttpContext.Current.Session["UserID"].ToString(), HttpContext.Current.Session["UserID"].ToString(), "1"), "Category like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Category"].ToString();
        }
        return txt;
    }
}