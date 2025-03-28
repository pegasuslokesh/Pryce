using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Duty_Master_Duty_Master : System.Web.UI.Page
{
    Common cmn = null;
    DutyMaster DutyMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
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

            cmn = new Common(Session["DBConnection"].ToString());
            DutyMaster = new DutyMaster(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
            Page.Title = objSys.GetSysTitle();
          
            if (!IsPostBack)
            {
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Duty_Master/Duty_Master.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                Fill_Grid_List();
                Fill_Designation();
            }
        }
        catch
        {
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        Img_Emp_Bin_Select_All.Visible = clsPagePermission.bRestore ;
        Img_Emp_List_Active.Visible = clsPagePermission.bRestore;
        hdnCanrestore.Value = clsPagePermission.bRestore.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        Img_Emp_List_Select_All.Visible = clsPagePermission.bDelete;
        Img_Emp_List_Delete_All.Visible = clsPagePermission.bDelete;
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

    public string GetCycle(object obj)
    {
        string Cycle = string.Empty;
        if (obj.ToString() == "")
            Cycle = "";
        else if (obj.ToString() == "1")
            Cycle = "Daily";
        else if (obj.ToString() == "2")
            Cycle = "Weekly";
        else if (obj.ToString() == "3")
            Cycle = "Biweekly";
        else if (obj.ToString() == "4")
            Cycle = "Monthly";
        else if (obj.ToString() == "5")
            Cycle = "Quarterly";
        else if (obj.ToString() == "6")
            Cycle = "Half Yearly";
        else if (obj.ToString() == "7")
            Cycle = "Yearly";
        return Cycle;
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
            Session["DM_CHECKED_ITEMS_LIST"] = null;
            Session["DM_Dt_Filter"] = null;
            DataTable Dt_Duty_Master_List = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "","", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "1");
            Session["DM_Dt_Duty_Master_List_Active"] = Dt_Duty_Master_List;
            if (Dt_Duty_Master_List.Rows.Count > 0)
            {
                Fill_Gv_Duty_Master(Dt_Duty_Master_List);
            }
            else
            {
                Fill_Gv_Duty_Master(Dt_Duty_Master_List);
            }
        }
        catch
        {
        }
    }

    protected void Fill_Gv_Duty_Master(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Duty_Master_List.DataSource = Dt_Grid;
            Gv_Duty_Master_List.DataBind();
          
           
        }
        else
        {
            Gv_Duty_Master_List.DataSource = null;
            Gv_Duty_Master_List.DataBind();
           
        }
    }

    private void Save_Checked_Duty_Master_Master()
    {
        ArrayList Duty_Master_Delete_Alls = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Duty_Master_List.Rows)
        {
            index = (int)Gv_Duty_Master_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["DM_CHECKED_ITEMS_LIST"] != null)
                Duty_Master_Delete_Alls = (ArrayList)Session["DM_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Duty_Master_Delete_Alls.Contains(index))
                    Duty_Master_Delete_Alls.Add(index);
            }
            else
                Duty_Master_Delete_Alls.Remove(index);
        }
        if (Duty_Master_Delete_Alls != null && Duty_Master_Delete_Alls.Count > 0)
            Session["DM_CHECKED_ITEMS_LIST"] = Duty_Master_Delete_Alls;
    }

    protected void Populate_Checked_Duty_Master_Master()
    {
        ArrayList Duty_Master_Delete_Alls = (ArrayList)Session["DM_CHECKED_ITEMS_LIST"];
        if (Duty_Master_Delete_Alls != null && Duty_Master_Delete_Alls.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Duty_Master_List.Rows)
            {
                int index = (int)Gv_Duty_Master_List.DataKeys[gvrow.RowIndex].Value;
                if (Duty_Master_Delete_Alls.Contains(index))
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
            Session["DM_Dt_Filter"] = null;
            Save_Checked_Duty_Master_Master();
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
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                }
                DataTable Dt_Duty_Master_List = (DataTable)Session["DM_Dt_Duty_Master_List_Active"];
                DataView view = new DataView(Dt_Duty_Master_List, condition, "", DataViewRowState.CurrentRows);
                Session["DM_Dt_Filter"] = view.ToTable();
                Fill_Gv_Duty_Master(view.ToTable());
                txtValue.Focus();
            }
            Populate_Checked_Duty_Master_Master();
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
            Session["DM_CHECKED_ITEMS_LIST"] = null;
            Session["DM_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Duty_Master_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Duty_Master(Session["DM_Dt_Duty_Master_List_Active"] as DataTable);
            txtValue.Text = "";
            ddlOption.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_List_Select_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        DataTable dtUnit = (DataTable)Session["DM_Dt_Duty_Master_List_Active"];
        ArrayList Duty_Master_Delete_Alls = new ArrayList();
        Session["DM_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["DM_CHECKED_ITEMS_LIST"] != null)
                {
                    Duty_Master_Delete_Alls = (ArrayList)Session["DM_CHECKED_ITEMS_LIST"];
                }
                if (!Duty_Master_Delete_Alls.Contains(dr["Trans_ID"]))
                {
                    Duty_Master_Delete_Alls.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Duty_Master_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Duty_Master_Delete_Alls.Count > 0 && Duty_Master_Delete_Alls != null)
            {
                Session["DM_CHECKED_ITEMS_LIST"] = Duty_Master_Delete_Alls;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["DM_Dt_Duty_Master_List_Active"];
            objPageCmn.FillData((object)Gv_Duty_Master_List, dt, "", "");
        
            ViewState["Select"] = null;
            
        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        int check = 0;
        int b = 0;
        DataTable Dt_Check_Duty = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "6");

        ArrayList Duty_Master_Delete_All = new ArrayList();
        if (Gv_Duty_Master_List.Rows.Count > 0)
        {
            Save_Checked_Duty_Master_Master();
            if (Session["DM_CHECKED_ITEMS_LIST"] != null)
            {
                Duty_Master_Delete_All = (ArrayList)Session["DM_CHECKED_ITEMS_LIST"];
                if (Duty_Master_Delete_All.Count > 0)
                {
                    for (int j = 0; j < Duty_Master_Delete_All.Count; j++)
                    {
                        if (Duty_Master_Delete_All[j].ToString() != "")
                        {
                            DataTable Dt_Check = new DataView(Dt_Check_Duty, "Duty_ID = " + Duty_Master_Delete_All[j].ToString(), "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Check.Rows.Count > 0)
                            {
                                check++;
                            }
                        }
                    }
                }
                if(check==0)
                {
                    if (Duty_Master_Delete_All.Count > 0)
                    {
                        for (int j = 0; j < Duty_Master_Delete_All.Count; j++)
                        {
                            if (Duty_Master_Delete_All[j].ToString() != "")
                            {
                                b = DutyMaster.Set_Duty_Master(Duty_Master_Delete_All[j].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text, Txt_Description.Text, Ddl_Duty_Cycle.SelectedItem.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage("Some Reference already used in application");
                    return;
                }
                if (b != 0)
                {
                    Session["DM_CHECKED_ITEMS_LIST"] = null;
                    Session["DM_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["DM_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Duty_Master_List.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("Chk_Gv_Select");
                        if (chk.Checked)
                        {
                            fleg++;
                        }
                        else
                        {
                            fleg++;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
                    }
                    else
                    {
                        DisplayMessage("Selected Duty Reference already used in application");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                Gv_Duty_Master_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Duty_Master_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Duty_Master_List.Rows)
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
        DataTable Dt_Duty_Master_List = Session["DM_Dt_Duty_Master_List_Active"] as DataTable;
        Dt_Duty_Master_List = new DataView(Dt_Duty_Master_List, "Trans_ID = " + Trans_ID, "", DataViewRowState.CurrentRows).ToTable();
        if (Dt_Duty_Master_List.Rows.Count > 0)
        {
            Txt_Duty_Title.Text = Dt_Duty_Master_List.Rows[0]["Title"].ToString();
            Txt_Description.Text = Dt_Duty_Master_List.Rows[0]["Description"].ToString();
            if (Dt_Duty_Master_List.Rows[0]["Duty_Cycle"].ToString() == "")
                Ddl_Duty_Cycle.SelectedValue = "0";
            else
                Ddl_Duty_Cycle.SelectedValue = Dt_Duty_Master_List.Rows[0]["Duty_Cycle"].ToString();

            if(Dt_Duty_Master_List.Rows[0]["Designation_ID"].ToString()=="")
            {
                Ddl_Designation.SelectedIndex = 0;
            }
            else
            {
                Ddl_Designation.SelectedValue = Dt_Duty_Master_List.Rows[0]["Designation_ID"].ToString();
            }

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        DataTable Dt_Check = DutyMaster.Get_Duty_Master(Trans_ID, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "5");
        if (Dt_Check.Rows.Count > 0)
        {
            DisplayMessage("This Reference already used in application");
        }
        else
        {
            int b = 0;
            String CompanyId = Session["CompId"].ToString();
            b = DutyMaster.Set_Duty_Master(Trans_ID, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text, Txt_Description.Text, Ddl_Duty_Cycle.SelectedItem.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");
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
    }

    protected void Gv_Duty_Master_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Master_Master();
            Gv_Duty_Master_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["DM_Dt_Filter"] != null)
                dt = (DataTable)Session["DM_Dt_Filter"];
            else if (Session["DM_Dt_Duty_Master_List_Active"] != null)
                dt = (DataTable)Session["DM_Dt_Duty_Master_List_Active"];
            objPageCmn.FillData((object)Gv_Duty_Master_List, dt, "", "");
           
            Gv_Duty_Master_List.HeaderRow.Focus();
            Populate_Checked_Duty_Master_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Duty_Master_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Master_Master();
            DataTable dt = new DataTable();
            if (Session["DM_Dt_Filter"] != null)
                dt = (DataTable)Session["DM_Dt_Filter"];
            else if (Session["DM_Dt_Duty_Master_List_Active"] != null)
                dt = (DataTable)Session["DM_Dt_Duty_Master_List_Active"];

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
            Session["DM_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Duty_Master_List, dt, "", "");
     
            Gv_Duty_Master_List.HeaderRow.Focus();
            Populate_Checked_Duty_Master_Master();
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
            Session["DM_CHECKED_ITEMS_BIN"] = null;
            Session["DM_Dt_Filter_Bin"] = null;
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
            DataTable Dt_Duty_Master_Bin = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "","", "", "False", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "1");            
            Session["DM_Dt_Duty_Master_Bin_InActive"] = Dt_Duty_Master_Bin;
            if (Dt_Duty_Master_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Duty_Master_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Duty_Master_Bin);
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
            Gv_Duty_Master_Bin.DataSource = Dt_Grid;
            Gv_Duty_Master_Bin.DataBind();
        
            
        }
        else
        {
            Gv_Duty_Master_Bin.DataSource = Dt_Grid;
            Gv_Duty_Master_Bin.DataBind();
        
           
        }
    }

    private void Save_Checked_Duty_Master_Master_Bin()
    {
        ArrayList Duty_Master_Delete_Alls = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Duty_Master_Bin.Rows)
        {
            index = (int)Gv_Duty_Master_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["DM_CHECKED_ITEMS_BIN"] != null)
                Duty_Master_Delete_Alls = (ArrayList)Session["DM_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Duty_Master_Delete_Alls.Contains(index))
                    Duty_Master_Delete_Alls.Add(index);
            }
            else
                Duty_Master_Delete_Alls.Remove(index);
        }
        if (Duty_Master_Delete_Alls != null && Duty_Master_Delete_Alls.Count > 0)
            Session["DM_CHECKED_ITEMS_BIN"] = Duty_Master_Delete_Alls;
    }

    protected void Populate_Checked_Duty_Master_Master_Bin()
    {
        ArrayList Duty_Master_Delete_Alls = (ArrayList)Session["DM_CHECKED_ITEMS_BIN"];
        if (Duty_Master_Delete_Alls != null && Duty_Master_Delete_Alls.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Duty_Master_Bin.Rows)
            {
                int index = (int)Gv_Duty_Master_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Duty_Master_Delete_Alls.Contains(index))
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
            Session["DM_Dt_Filter_Bin"] = null;
            Save_Checked_Duty_Master_Master_Bin();
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
                DataTable Dt_Duty_Master_Bin = (DataTable)Session["DM_Dt_Duty_Master_Bin_InActive"];
                DataView view = new DataView(Dt_Duty_Master_Bin, condition, "", DataViewRowState.CurrentRows);
                Session["DM_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                txtValueBin.Focus();
            }
            Populate_Checked_Duty_Master_Master_Bin();
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
            Session["DM_CHECKED_ITEMS_BIN"] = null;
            Session["DM_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Duty_Master_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["DM_Dt_Duty_Master_Bin_InActive"] as DataTable);
            txtValueBin.Text = "";
            ddlOptionBin.SelectedIndex = 2;
        }
        catch
        {
        }
    }

    protected void Img_Emp_Bin_Select_All_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        DataTable dtUnit = (DataTable)Session["DM_Dt_Duty_Master_Bin_InActive"];
        ArrayList Duty_Master_Delete_Alls = new ArrayList();
        Session["DM_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["DM_CHECKED_ITEMS_BIN"] != null)
                {
                    Duty_Master_Delete_Alls = (ArrayList)Session["DM_CHECKED_ITEMS_BIN"];
                }
                if (!Duty_Master_Delete_Alls.Contains(dr["Trans_ID"]))
                {
                    Duty_Master_Delete_Alls.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Duty_Master_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Duty_Master_Delete_Alls.Count > 0 && Duty_Master_Delete_Alls != null)
            {
                Session["DM_CHECKED_ITEMS_BIN"] = Duty_Master_Delete_Alls;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["DM_Dt_Duty_Master_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Duty_Master_Bin, dt, "", "");
         
            ViewState["Select"] = null;
            if (dt.Rows.Count > 0)
            {
              
            }
            else
            {
                
            }
        }
    }

    protected void Img_Emp_List_Active_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int b = 0;
        ArrayList Duty_Master_Delete_All = new ArrayList();
        if (Gv_Duty_Master_Bin.Rows.Count > 0)
        {
            Save_Checked_Duty_Master_Master_Bin();
            if (Session["DM_CHECKED_ITEMS_BIN"] != null)
            {
                Duty_Master_Delete_All = (ArrayList)Session["DM_CHECKED_ITEMS_BIN"];
                if (Duty_Master_Delete_All.Count > 0)
                {
                    for (int j = 0; j < Duty_Master_Delete_All.Count; j++)
                    {
                        if (Duty_Master_Delete_All[j].ToString() != "")
                        {
                            b = DutyMaster.Set_Duty_Master(Duty_Master_Delete_All[j].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text, Txt_Description.Text, Ddl_Duty_Cycle.SelectedItem.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");
                        }
                    }
                }
                if (b != 0)
                {
                    Session["DM_CHECKED_ITEMS_BIN"] = null;
                    Session["DM_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["DM_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Duty_Master_Bin.Rows)
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
                Gv_Duty_Master_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Duty_Master_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Duty_Master_Bin.Rows)
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
        b = DutyMaster.Set_Duty_Master(Trans_ID, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text, Txt_Description.Text, Ddl_Duty_Cycle.SelectedItem.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");
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

    protected void Gv_Duty_Master_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Master_Master_Bin();
            Gv_Duty_Master_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["DM_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["DM_Dt_Filter_Bin"];
            else if (Session["DM_Dt_Duty_Master_Bin_InActive"] != null)
                dt = (DataTable)Session["DM_Dt_Duty_Master_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Duty_Master_Bin, dt, "", "");
          
            Gv_Duty_Master_Bin.HeaderRow.Focus();
            Populate_Checked_Duty_Master_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Duty_Master_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Master_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["DM_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["DM_Dt_Filter_Bin"];
            else if (Session["DM_Dt_Duty_Master_Bin_InActive"] != null)
                dt = (DataTable)Session["DM_Dt_Duty_Master_Bin_InActive"];

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
            Session["DM_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Duty_Master_Bin, dt, "", "");

            Gv_Duty_Master_Bin.HeaderRow.Focus();
            Populate_Checked_Duty_Master_Master_Bin();
        }
        catch
        {
        }
    }

    //--------------- End Bin ---------------


    //--------------- Start New ---------------

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Duty_Title(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        DutyMaster DutyMaster = new DutyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "","", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "2"), "Title like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Title"].ToString();
        }
        return txt;
    }

    protected void Btn_New_Li_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Duty_Title.Text == "")
            {
                DisplayMessage("Enter Title");
                return;
            }
            else if (Txt_Description.Text == "")
            {
                DisplayMessage("Enter Description");
                return;
            }
            else if (Ddl_Duty_Cycle.SelectedValue == "0")
            {
                DisplayMessage("Select Duty Cycle");
                return;
            }
            else
            {
                if (Edit_ID.Value == "")
                {
                    int b = 0;
                    b = DutyMaster.Insert_Duty_Master( HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text, Txt_Description.Text, Ddl_Duty_Cycle.SelectedValue.ToString(), Ddl_Designation.SelectedValue.ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                    if (b != 0)
                    {
                        DisplayMessage("Record Saved", "green");
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
                    b = DutyMaster.Update_Duty_Master(Edit_ID.Value, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text, Txt_Description.Text, Ddl_Duty_Cycle.SelectedValue.ToString(), Ddl_Designation.SelectedValue.ToString(), Session["UserId"].ToString());
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
            Edit_ID.Value = "";
            Txt_Duty_Title.Text = "";
            Txt_Description.Text = "";
            Ddl_Designation.SelectedIndex = 0;
            Ddl_Duty_Cycle.SelectedValue = "0";
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
    
    protected void Txt_Duty_Title_TextChanged(object sender, EventArgs e)
    {
        int counter = 0;
        if (Txt_Duty_Title.Text.Trim() != "")
        {
            if (Edit_ID.Value == "")
            {
                DataTable dt = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text,"", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "4");
                if (dt.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Title is Already Exists");
                    Txt_Duty_Title.Text = "";
                    Txt_Duty_Title.Focus();
                }
                DataTable dt1 = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text,"", "", "False", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "4");
                if (dt1.Rows.Count > 0)
                {
                    counter = 1;
                    DisplayMessage("Title Already Exists in Bin Section");
                    Txt_Duty_Title.Text = "";
                    Txt_Duty_Title.Focus();
                }
            }
            else
            {
                DataTable dtTemp = DutyMaster.Get_Duty_Master(Edit_ID.Value, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text,"", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "3");
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Title"].ToString() != Txt_Duty_Title.Text)
                    {
                        DataTable dt = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text,"", "", "True", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "4");
                        if (dt.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Title is Already Exists");
                            Txt_Duty_Title.Text = "";
                            Txt_Duty_Title.Focus();
                        }
                        DataTable dt1 = DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Txt_Duty_Title.Text,"", "", "False", "", DateTime.Now.ToString(), "", DateTime.Now.ToString(), "4");
                        if (dt1.Rows.Count > 0)
                        {
                            counter = 1;
                            DisplayMessage("Title Already Exists in Bin Section");
                            Txt_Duty_Title.Text = "";
                            Txt_Duty_Title.Focus();
                        }
                    }
                }
            }
        }
        Txt_Duty_Title.Focus();
        if (counter == 0)
            Ddl_Duty_Cycle.Focus();
    }

    public void Fill_Designation()
    {
        DataTable dt = objDesg.GetDesignationMaster();
        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            Ddl_Designation.DataSource = null;
            Ddl_Designation.DataBind();
            objPageCmn.FillData((object)Ddl_Designation, dt, "Designation", "Designation_Id");
        }
        else
        {
            try
            {
                Ddl_Designation.Items.Insert(0, "--Select--");
                Ddl_Designation.SelectedIndex = 0;
            }
            catch
            {
                Ddl_Designation.Items.Insert(0, "--Select--");
                Ddl_Designation.SelectedIndex = 0;
            }
        }
    }

    //--------------- End New ---------------
}