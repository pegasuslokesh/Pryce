using PegasusDataAccess;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Duty_Master_Duty_Chart : System.Web.UI.Page
{
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    DutyMaster DutyMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    SystemParameter objSys = null;
    DesignationMaster objDesg = null;
    Common ObjComman = null;
    PageControlCommon objPageCmn = null;
    DataTable dt_Grid = new DataTable();
    DataAccessClass objDa = null;
    //--------------- Start System ---------------

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }
            objDa = new DataAccessClass(Session["DBConnection"].ToString());
            Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
            objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
            cmn = new Common(Session["DBConnection"].ToString());
            DutyMaster = new DutyMaster(Session["DBConnection"].ToString());
            objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
            objSys = new SystemParameter(Session["DBConnection"].ToString());
            objDesg = new DesignationMaster(Session["DBConnection"].ToString());
            ObjComman = new Common(Session["DBConnection"].ToString());
            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
            Page.Title = objSys.GetSysTitle();

            if (!IsPostBack)
            {
                DataTable dt_Grid = new DataTable(Session["DBConnection"].ToString());
                Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Duty_Master/Duty_Chart.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
                if (clsPagePermission.bHavePermission == false)
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                AllPageCode(clsPagePermission);
                Fill_Designation();
                Fill_Grid_List();
            }
        }
        catch (Exception ex)
        {
        }
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        Img_Emp_Bin_Select_All.Visible = clsPagePermission.bRestore;
        Img_Emp_List_Active.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        Img_Emp_List_Select_All.Visible = clsPagePermission.bDelete;
        Img_Emp_List_Delete_All.Visible = clsPagePermission.bDelete;
    }



    public void DisplayMessage(string str, string color = "orange")
    {
        try
        {
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }
            if (Session["lang"].ToString() == "1")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
            }
            else if (Session["lang"].ToString() == "2")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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

    public string GetCycle_ID(object obj)
    {
        string Cycle = string.Empty;
        if (obj.ToString() == "--Select--")
            Cycle = "0";
        else if (obj.ToString() == "Daily")
            Cycle = "1";
        else if (obj.ToString() == "Weekly")
            Cycle = "2";
        else if (obj.ToString() == "Biweekly")
            Cycle = "3";
        else if (obj.ToString() == "Monthly")
            Cycle = "4";
        else if (obj.ToString() == "Quarterly")
            Cycle = "5";
        else if (obj.ToString() == "Half Yearly")
            Cycle = "6";
        else if (obj.ToString() == "Yearly")
            Cycle = "7";
        return Cycle;
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
            Session["Dc_CHECKED_ITEMS_LIST"] = null;
            Session["Dc_Dt_Filter"] = null;
            DataTable Dt_Duty_Chart_List = DutyMaster.Get_Duty_Chart("0", "0", "0", "", "0", DateTime.Now.ToString(), "0", "0", "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "2");
            Session["Dc_Dt_Duty_Chart_List_Active"] = Dt_Duty_Chart_List;
            if (Dt_Duty_Chart_List.Rows.Count > 0)
            {
                Fill_Gv_Duty_Chart(Dt_Duty_Chart_List);
            }
            else
            {
                Fill_Gv_Duty_Chart(Dt_Duty_Chart_List);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fill_Gv_Duty_Chart(DataTable Dt_Grid)
    {
        Lbl_TotalRecords.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();

        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Duty_Chart_List.DataSource = Dt_Grid;
            Gv_Duty_Chart_List.DataBind();

        }
        else
        {
            Gv_Duty_Chart_List.DataSource = null;
            Gv_Duty_Chart_List.DataBind();

        }
    }

    private void Save_Checked_Duty_Chart_Master()
    {
        ArrayList Duty_Chart_Delete_Alls = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Duty_Chart_List.Rows)
        {
            index = (int)Gv_Duty_Chart_List.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select")).Checked;
            if (Session["Dc_CHECKED_ITEMS_LIST"] != null)
                Duty_Chart_Delete_Alls = (ArrayList)Session["Dc_CHECKED_ITEMS_LIST"];
            if (result)
            {
                if (!Duty_Chart_Delete_Alls.Contains(index))
                    Duty_Chart_Delete_Alls.Add(index);
            }
            else
                Duty_Chart_Delete_Alls.Remove(index);
        }
        if (Duty_Chart_Delete_Alls != null && Duty_Chart_Delete_Alls.Count > 0)
            Session["Dc_CHECKED_ITEMS_LIST"] = Duty_Chart_Delete_Alls;
    }

    protected void Populate_Checked_Duty_Chart_Master()
    {
        ArrayList Duty_Chart_Delete_Alls = (ArrayList)Session["Dc_CHECKED_ITEMS_LIST"];
        if (Duty_Chart_Delete_Alls != null && Duty_Chart_Delete_Alls.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Duty_Chart_List.Rows)
            {
                int index = (int)Gv_Duty_Chart_List.DataKeys[gvrow.RowIndex].Value;
                if (Duty_Chart_Delete_Alls.Contains(index))
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
            Session["Dc_Dt_Filter"] = null;
            Save_Checked_Duty_Chart_Master();
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
                DataTable Dt_Duty_Chart_List = (DataTable)Session["Dc_Dt_Duty_Chart_List_Active"];
                DataView view = new DataView(Dt_Duty_Chart_List, condition, "", DataViewRowState.CurrentRows);
                Session["Dc_Dt_Filter"] = view.ToTable();
                Fill_Gv_Duty_Chart(view.ToTable());
                txtValue.Focus();
            }
            Populate_Checked_Duty_Chart_Master();
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
            Session["Dc_CHECKED_ITEMS_LIST"] = null;
            Session["Dc_Dt_Filter"] = null;
            foreach (GridViewRow GR in Gv_Duty_Chart_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = false;
            }
            Fill_Gv_Duty_Chart(Session["Dc_Dt_Duty_Chart_List_Active"] as DataTable);
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
        DataTable dtUnit = (DataTable)Session["Dc_Dt_Duty_Chart_List_Active"];
        ArrayList Duty_Chart_Delete_Alls = new ArrayList();
        Session["Dc_CHECKED_ITEMS_LIST"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["Dc_CHECKED_ITEMS_LIST"] != null)
                {
                    Duty_Chart_Delete_Alls = (ArrayList)Session["Dc_CHECKED_ITEMS_LIST"];
                }
                if (!Duty_Chart_Delete_Alls.Contains(dr["Emp_ID"]))
                {
                    Duty_Chart_Delete_Alls.Add(dr["Emp_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Duty_Chart_List.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select")).Checked = true;
            }
            if (Duty_Chart_Delete_Alls.Count > 0 && Duty_Chart_Delete_Alls != null)
            {
                Session["Dc_CHECKED_ITEMS_LIST"] = Duty_Chart_Delete_Alls;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["Dc_Dt_Duty_Chart_List_Active"];
            objPageCmn.FillData((object)Gv_Duty_Chart_List, dt, "", "");

            ViewState["Select"] = null;

        }
    }

    protected void Img_Emp_List_Delete_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        int c = 0;
        ArrayList Duty_Chart_Delete_All = new ArrayList();
        if (Gv_Duty_Chart_List.Rows.Count > 0)
        {
            Save_Checked_Duty_Chart_Master();
            if (Session["Dc_CHECKED_ITEMS_LIST"] != null)
            {
                Duty_Chart_Delete_All = (ArrayList)Session["Dc_CHECKED_ITEMS_LIST"];
                if (Duty_Chart_Delete_All.Count > 0)
                {
                    for (int j = 0; j < Duty_Chart_Delete_All.Count; j++)
                    {
                        if (Duty_Chart_Delete_All[j].ToString() != "")
                        {
                            b = DutyMaster.Set_DutyChart("0", Duty_Chart_Delete_All[j].ToString(), "0", "0", "0", DateTime.Now.ToString(), "0", "0", "1", "False", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "4", DateTime.Now.ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    Session["Dc_CHECKED_ITEMS_LIST"] = null;
                    Session["Dc_Dt_Filter"] = null;
                    Fill_Grid_List();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Deleted");
                    Session["Dc_CHECKED_ITEMS_LIST"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Duty_Chart_List.Rows)
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
                Gv_Duty_Chart_List.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Duty_Chart_List.HeaderRow.FindControl("Chk_Gv_Select_All"));
        foreach (GridViewRow gr in Gv_Duty_Chart_List.Rows)
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
        Session["Dc_Dt_Duty_Edit"] = null;
        Session["Dc_Dt_Duty_Grid"] = null;
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        string Trans_ID = e.CommandArgument.ToString();
        Edit_ID.Value = e.CommandArgument.ToString();
        if (Session["Dc_Dt_Duty_Edit"] == null)
        {
            DataTable Dt_Duty_Edit = new DataTable();
            Dt_Duty_Edit.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
            Session["Dc_Dt_Duty_Edit"] = Dt_Duty_Edit;
        }

        if (Session["Dc_Dt_Duty_Grid"] == null)
        {
            DataTable Dt_Duty_Grid = new DataTable();
            Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
            Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
        }

        DataTable Dt_Duty_Master = DutyMaster.Get_Duty_Chart("0", Trans_ID, "0", "", "0", DateTime.Now.ToString(), "0", "0", "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "8");
        if (Dt_Duty_Master.Rows.Count > 0)
        {
            Txt_Emp_Name.Text = Dt_Duty_Master.Rows[0]["Employee_Name"].ToString();
            Txt_Emp_Name_TextChanged(null, null);
            DataTable Dt_Duty_Edit = Session["Dc_Dt_Duty_Edit"] as DataTable;
            foreach (DataRow drv in Dt_Duty_Master.Rows)
            {
                Dt_Duty_Edit.Rows.Add(drv["Trans_ID"].ToString(), drv["Title"].ToString(), drv["Description"].ToString(), drv["Duty_Cycle"].ToString(), GetDate(drv["WEF_Date"].ToString()), drv["Report_To"].ToString(), drv["Created_By"].ToString(), drv["Modified_By"].ToString());
            }
            Gv_Duty_Master_List.DataSource = Dt_Duty_Edit;
            Gv_Duty_Master_List.DataBind();
            Session["Dc_Dt_Duty_Edit"] = Dt_Duty_Edit;
            Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Edit;
        }
        else
        {
            Reset();
        }
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }

    protected void IBtn_Delete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int b = 0;
        int c = 0;
        String CompanyId = Session["CompId"].ToString();
        b = DutyMaster.Set_DutyChart("0", Trans_ID, "0", "0", "0", DateTime.Now.ToString(), "0", "0", "1", "False", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "4", DateTime.Now.ToString(), DateTime.Now.ToString());
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

    protected void Gv_Duty_Chart_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Chart_Master();
            Gv_Duty_Chart_List.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["Dc_Dt_Filter"] != null)
                dt = (DataTable)Session["Dc_Dt_Filter"];
            else if (Session["Dc_Dt_Duty_Chart_List_Active"] != null)
                dt = (DataTable)Session["Dc_Dt_Duty_Chart_List_Active"];
            objPageCmn.FillData((object)Gv_Duty_Chart_List, dt, "", "");

            Gv_Duty_Chart_List.HeaderRow.Focus();
            Populate_Checked_Duty_Chart_Master();
        }
        catch
        {
        }
    }

    protected void Gv_Duty_Chart_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Chart_Master();
            DataTable dt = new DataTable();
            if (Session["Dc_Dt_Filter"] != null)
                dt = (DataTable)Session["Dc_Dt_Filter"];
            else if (Session["Dc_Dt_Duty_Chart_List_Active"] != null)
                dt = (DataTable)Session["Dc_Dt_Duty_Chart_List_Active"];

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
            Session["Dc_Dt_Filter"] = dt;
            objPageCmn.FillData((object)Gv_Duty_Chart_List, dt, "", "");

            Gv_Duty_Chart_List.HeaderRow.Focus();
            Populate_Checked_Duty_Chart_Master();
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
            Session["Dc_CHECKED_ITEMS_BIN"] = null;
            Session["Dc_Dt_Filter_Bin"] = null;
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
            DataTable Dt_Duty_Chart_Bin = DutyMaster.Get_Duty_Chart("0", "0", "0", "", "0", DateTime.Now.ToString(), "0", "0", "0", "False", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "9");
            Session["Dc_Dt_Duty_Chart_Bin_InActive"] = Dt_Duty_Chart_Bin;
            if (Dt_Duty_Chart_Bin.Rows.Count > 0)
            {
                Fill_Gv_Bin(Dt_Duty_Chart_Bin);
            }
            else
            {
                Fill_Gv_Bin(Dt_Duty_Chart_Bin);
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void Fill_Gv_Bin(DataTable Dt_Grid)
    {
        Lbl_TotalRecords_Bin.Text = "Total Records: " + Dt_Grid.Rows.Count.ToString();
        if (Dt_Grid.Rows.Count > 0)
        {
            Gv_Duty_Chart_Bin.DataSource = Dt_Grid;
            Gv_Duty_Chart_Bin.DataBind();

        }
        else
        {
            Gv_Duty_Chart_Bin.DataSource = Dt_Grid;
            Gv_Duty_Chart_Bin.DataBind();

        }
    }

    private void Save_Checked_Duty_Chart_Master_Bin()
    {
        ArrayList Duty_Chart_Delete_Alls = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in Gv_Duty_Chart_Bin.Rows)
        {
            index = (int)Gv_Duty_Chart_Bin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("Chk_Gv_Select_Bin")).Checked;
            if (Session["Dc_CHECKED_ITEMS_BIN"] != null)
                Duty_Chart_Delete_Alls = (ArrayList)Session["Dc_CHECKED_ITEMS_BIN"];
            if (result)
            {
                if (!Duty_Chart_Delete_Alls.Contains(index))
                    Duty_Chart_Delete_Alls.Add(index);
            }
            else
                Duty_Chart_Delete_Alls.Remove(index);
        }
        if (Duty_Chart_Delete_Alls != null && Duty_Chart_Delete_Alls.Count > 0)
            Session["Dc_CHECKED_ITEMS_BIN"] = Duty_Chart_Delete_Alls;
    }

    protected void Populate_Checked_Duty_Chart_Master_Bin()
    {
        ArrayList Duty_Chart_Delete_Alls = (ArrayList)Session["Dc_CHECKED_ITEMS_BIN"];
        if (Duty_Chart_Delete_Alls != null && Duty_Chart_Delete_Alls.Count > 0)
        {
            foreach (GridViewRow gvrow in Gv_Duty_Chart_Bin.Rows)
            {
                int index = (int)Gv_Duty_Chart_Bin.DataKeys[gvrow.RowIndex].Value;
                if (Duty_Chart_Delete_Alls.Contains(index))
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
            Session["Dc_Dt_Filter_Bin"] = null;
            Save_Checked_Duty_Chart_Master_Bin();
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
                DataTable Dt_Duty_Chart_Bin = (DataTable)Session["Dc_Dt_Duty_Chart_Bin_InActive"];
                DataView view = new DataView(Dt_Duty_Chart_Bin, condition, "", DataViewRowState.CurrentRows);
                Session["Dc_Dt_Filter_Bin"] = view.ToTable();
                Fill_Gv_Bin(view.ToTable());
                txtValueBin.Focus();
            }
            Populate_Checked_Duty_Chart_Master_Bin();
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
            Session["Dc_CHECKED_ITEMS_BIN"] = null;
            Session["Dc_Dt_Filter_Bin"] = null;
            foreach (GridViewRow GR in Gv_Duty_Chart_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = false;
            }
            Fill_Gv_Bin(Session["Dc_Dt_Duty_Chart_Bin_InActive"] as DataTable);
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
        DataTable dtUnit = (DataTable)Session["Dc_Dt_Duty_Chart_Bin_InActive"];
        ArrayList Duty_Chart_Delete_Alls = new ArrayList();
        Session["Dc_CHECKED_ITEMS_BIN"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["Dc_CHECKED_ITEMS_BIN"] != null)
                {
                    Duty_Chart_Delete_Alls = (ArrayList)Session["Dc_CHECKED_ITEMS_BIN"];
                }
                if (!Duty_Chart_Delete_Alls.Contains(dr["Trans_ID"]))
                {
                    Duty_Chart_Delete_Alls.Add(dr["Trans_ID"]);
                }
            }
            foreach (GridViewRow GR in Gv_Duty_Chart_Bin.Rows)
            {
                ((CheckBox)GR.FindControl("Chk_Gv_Select_Bin")).Checked = true;
            }
            if (Duty_Chart_Delete_Alls.Count > 0 && Duty_Chart_Delete_Alls != null)
            {
                Session["Dc_CHECKED_ITEMS_BIN"] = Duty_Chart_Delete_Alls;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["Dc_Dt_Duty_Chart_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Duty_Chart_Bin, dt, "", "");

            ViewState["Select"] = null;

        }
    }

    protected void Img_Emp_List_Active_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        int c = 0;
        ArrayList Duty_Chart_Delete_All = new ArrayList();
        if (Gv_Duty_Chart_Bin.Rows.Count > 0)
        {
            Save_Checked_Duty_Chart_Master_Bin();
            if (Session["Dc_CHECKED_ITEMS_BIN"] != null)
            {
                Duty_Chart_Delete_All = (ArrayList)Session["Dc_CHECKED_ITEMS_BIN"];
                if (Duty_Chart_Delete_All.Count > 0)
                {
                    for (int j = 0; j < Duty_Chart_Delete_All.Count; j++)
                    {
                        if (Duty_Chart_Delete_All[j].ToString() != "")
                        {
                            b = DutyMaster.Set_DutyChart(Duty_Chart_Delete_All[j].ToString(), "0", "0", "0", "0", DateTime.Now.ToString(), "0", "0", "1", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "5", DateTime.Now.ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    Session["Dc_CHECKED_ITEMS_BIN"] = null;
                    Session["Dc_Dt_Filter_Bin"] = null;
                    Fill_Grid_List();
                    Fill_Grid_Bin();
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["Dc_CHECKED_ITEMS_BIN"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in Gv_Duty_Chart_Bin.Rows)
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
                Gv_Duty_Chart_Bin.Focus();
                return;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }

    protected void Chk_Gv_Select_All_Bin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Duty_Chart_Bin.HeaderRow.FindControl("Chk_Gv_Select_All_Bin"));
        foreach (GridViewRow gr in Gv_Duty_Chart_Bin.Rows)
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
        Session["Dc_Dt_Duty_Edit"] = null;
        Session["Dc_Dt_Duty_Grid"] = null;
        string Trans_ID = e.CommandArgument.ToString();
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        Label Lbl_Duty_ID_Bin = gvrow.FindControl("Lbl_Duty_ID_Bin") as Label;
        Edit_ID.Value = e.CommandArgument.ToString();

        DataTable Dt_Check = DutyMaster.Get_Duty_Chart("0", "0", Lbl_Duty_ID_Bin.Text, "", "0", DateTime.Now.ToString(), "0", "0", "0", "False", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "11");
        if (Dt_Check.Rows.Count > 0)
        {
            DisplayMessage("This Duty is inactive in master");
            return;
        }
        else
        {

            if (Session["Dc_Dt_Duty_Edit"] == null)
            {
                DataTable Dt_Duty_Edit = new DataTable();
                Dt_Duty_Edit.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
                Session["Dc_Dt_Duty_Edit"] = Dt_Duty_Edit;
            }

            if (Session["Dc_Dt_Duty_Grid"] == null)
            {
                DataTable Dt_Duty_Grid = new DataTable();
                Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
                Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
            }



            DataTable Dt_Duty_Master = DutyMaster.Get_Duty_Chart(Trans_ID, "0", "0", "", "0", DateTime.Now.ToString(), "0", "0", "0", "False", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "10");
            if (Dt_Duty_Master.Rows.Count > 0)
            {
                Txt_Emp_Name.Text = Dt_Duty_Master.Rows[0]["Employee_Name"].ToString();
                Txt_Emp_Name_TextChanged(null, null);
                DataTable Dt_Duty_Edit = Session["Dc_Dt_Duty_Edit"] as DataTable;
                foreach (DataRow drv in Dt_Duty_Master.Rows)
                {
                    Dt_Duty_Edit.Rows.Add(drv["Trans_ID"].ToString(), drv["Title"].ToString(), drv["Description"].ToString(), drv["Duty_Cycle"].ToString(), GetDate(drv["WEF_Date"].ToString()), drv["Report_To"].ToString(), drv["Created_By"].ToString(), drv["Modified_By"].ToString());
                }
                Gv_Duty_Master_List.DataSource = Dt_Duty_Edit;
                Gv_Duty_Master_List.DataBind();
                Session["Dc_Dt_Duty_Edit"] = Dt_Duty_Edit;
                Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Edit;
            }
            else
            {
                Reset();
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Active_Active()", true);
        }







        //int b = 0;
        //int c = 0;
        //String CompanyId = Session["CompId"].ToString().ToString();
        //b = DutyMaster.Set_DutyChart(Trans_ID, "0", "0", "0", "0", DateTime.Now.ToString(), "0", "0", "1", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "5", DateTime.Now.ToString(), DateTime.Now.ToString());
        //if (b != 0)
        //{
        //    Fill_Grid_List();
        //    Fill_Grid_Bin();
        //    DisplayMessage("Record Activated");
        //}
        //else
        //{
        //    DisplayMessage("Record Not Activated");
        //}
    }

    protected void Gv_Duty_Chart_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Chart_Master_Bin();
            Gv_Duty_Chart_Bin.PageIndex = e.NewPageIndex;
            DataTable dt = new DataTable();
            if (Session["Dc_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["Dc_Dt_Filter_Bin"];
            else if (Session["Dc_Dt_Duty_Chart_Bin_InActive"] != null)
                dt = (DataTable)Session["Dc_Dt_Duty_Chart_Bin_InActive"];
            objPageCmn.FillData((object)Gv_Duty_Chart_Bin, dt, "", "");

            Gv_Duty_Chart_Bin.HeaderRow.Focus();
            Populate_Checked_Duty_Chart_Master_Bin();
        }
        catch
        {
        }
    }

    protected void Gv_Duty_Chart_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            Save_Checked_Duty_Chart_Master_Bin();
            DataTable dt = new DataTable();
            if (Session["Dc_Dt_Filter_Bin"] != null)
                dt = (DataTable)Session["Dc_Dt_Filter_Bin"];
            else if (Session["Dc_Dt_Duty_Chart_Bin_InActive"] != null)
                dt = (DataTable)Session["Dc_Dt_Duty_Chart_Bin_InActive"];

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
            Session["Dc_Dt_Filter_Bin"] = dt;
            objPageCmn.FillData((object)Gv_Duty_Chart_Bin, dt, "", "");

            Gv_Duty_Chart_Bin.HeaderRow.Focus();
            Populate_Checked_Duty_Chart_Master_Bin();
        }
        catch
        {
        }
    }

    //--------------- End Bin ---------------


    //--------------- Start New ---------------
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        // DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "", HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethod()]
    public static string[] Get_Employee(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        DataTable dt = Common.GetActiveEmployeeWithoutLocation(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "(" + dt.Rows[i][0].ToString() + ")";
        }
        return str;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Duty_Group(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        DutyMaster DutyMaster = new DutyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(DutyMaster.Get_Duty_Group("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", "0", "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), "1"), "Title like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Title"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Duty_Title(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["UserId"] == null)
        {
            HttpContext.Current.Response.Redirect("~/ERPLogin.aspx");
        }
        DutyMaster DutyMaster = new DutyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(DutyMaster.Get_Duty_Master("0", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", "", "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), "2"), "Title like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
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
    protected void Btn_Copy_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Dc_Dt_Duty_Grid"] = null;
            if (Session["Dc_Dt_Duty_Grid"] == null)
            {
                DataTable Dt_Duty_Grid = new DataTable();
                Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
                Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
            }
            int b = 0;
            if (Txt_Emp_Name.Text == "")
            {
                DisplayMessage("Enter Employee Name");
                Txt_Emp_Name.Focus();
                return;
            }
            else if (txtReferenceEmployee.Text == "")
            {
                DisplayMessage("Enter Reference Employee Name");
                Txt_Emp_Name.Focus();
                return;
            }
            else if (Gv_Duty_Master_List == null || Gv_Duty_Master_List.Rows.Count == 0)
            {
                DisplayMessage("Select atleast one duty");
                Rbt_Group.Checked = true;
                Txt_Duty_Group.Visible = true;
                Txt_Duty_Group.Focus();
                return;
            }
            else
            {
                DataTable Dt_Duty_Grid = Session["Dc_Dt_Duty_Grid"] as DataTable;
                foreach (GridViewRow GVR in Gv_Duty_Master_List.Rows)
                {
                    HiddenField Hdn_Trans_ID_Master = GVR.FindControl("Hdn_Trans_ID_Master") as HiddenField;
                    Label Txt_Duty_Title_Master = GVR.FindControl("Txt_Duty_Title_Master") as Label;
                    Label Txt_Duty_Description_Master = GVR.FindControl("Txt_Duty_Description_Master") as Label;
                    DropDownList Ddl_Duty_Cycle_Master = GVR.FindControl("Ddl_Duty_Cycle_Master") as DropDownList;
                    TextBox Txt_WEF_Date_Master = GVR.FindControl("Txt_WEF_Date_Master") as TextBox;
                    TextBox Txt_Report_To_Master = GVR.FindControl("Txt_Report_To_Master") as TextBox;
                    Label Lbl_Create_by = GVR.FindControl("Lbl_Create_by") as Label;
                    Label Lbl_Modified_by = GVR.FindControl("Lbl_Modified_by") as Label;
                    Dt_Duty_Grid.Rows.Add(Hdn_Trans_ID_Master.Value, Txt_Duty_Title_Master.Text, Txt_Duty_Description_Master.Text, Ddl_Duty_Cycle_Master.SelectedValue.ToString(), Txt_WEF_Date_Master.Text, Txt_Report_To_Master.Text, Lbl_Create_by.Text, Lbl_Modified_by.Text);
                }
                foreach (DataRow DVR_Grid in Dt_Duty_Grid.Rows)
                {
                    if (DVR_Grid["Duty_Cycle"].ToString() != "0" && DVR_Grid["WEF_Date"].ToString() != "" && DVR_Grid["Report_To"].ToString() != "")
                    {
                        string Report_To_ID = string.Empty;
                        //string[] Report_To = DVR_Grid["Report_To"].ToString().Split(',');
                        //for (int i = 0; i < Report_To.Length; i++)
                        //{
                        //    string Emp = Report_To[i].ToString();
                        //    int Emp_Start = Emp.IndexOf("(") + 1;
                        //    int Emp_End = Emp.IndexOf(")", Emp_Start);
                        //    Report_To_ID = Report_To_ID + Emp.Substring(Emp_Start, Emp_End - Emp_Start) + ",";
                        //}

                        // Change Hidden Filed Emp id and Report to Person
                        string refEmpId = string.Empty;
                        string refempdoj  = string.Empty;
                        string refreportEmpName = string.Empty;
                        string refreportEmpId = string.Empty;
                        string refreportEmpCode = string.Empty;


                        string refEmployeeCode = txtReferenceEmployee.Text.Split('/')[txtReferenceEmployee.Text.Split('/').Length - 1];

                        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                        DataTable Cur_Emp_Record = new DataView(dtEmp, "Emp_Code='" + refEmployeeCode + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (Cur_Emp_Record.Rows.Count > 0)
                        {
                            refEmpId = Cur_Emp_Record.Rows[0]["Emp_Id"].ToString();

                            refempdoj = Cur_Emp_Record.Rows[0]["DOJ"].ToString();
                        }

                        DataTable Dt_Report_To = new DataView(dtEmp, "Emp_ID='" + Cur_Emp_Record.Rows[0]["Field5"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (Dt_Report_To.Rows.Count > 0)
                        {
                            refreportEmpId  = Dt_Report_To.Rows[0]["Emp_ID"].ToString();
                            refreportEmpCode = Dt_Report_To.Rows[0]["Emp_Code"].ToString();
                            refreportEmpName = Dt_Report_To.Rows[0]["Emp_Name"].ToString();
                        }






                        //
                        string Effect_Date_From = string.Empty;
                        string Effect_Date_To = string.Empty;
                        if (DVR_Grid["Duty_Cycle"].ToString() == "1")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddDays(0).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "2")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddDays(6).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "3")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddDays(3).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "4")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "5")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddMonths(3).AddDays(-1).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "6")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddMonths(6).AddDays(-1).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "7")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddYears(1).AddDays(-1).ToString("dd-MMM-yyyy");
                        }

                       
                            b = DutyMaster.Set_DutyChart("0", refEmpId , DVR_Grid["Trans_ID"].ToString(), DVR_Grid["Description"].ToString(), DVR_Grid["Duty_Cycle"].ToString(), DVR_Grid["WEF_Date"].ToString(), refreportEmpCode +",", refreportEmpName+"("+ refreportEmpCode +")" , "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1", Effect_Date_From, Effect_Date_To);
                           // Send_Notification_Duty(DVR_Grid["Title"].ToString(), Effect_Date_From, DVR_Grid["Duty_Cycle"].ToString(), Hdn_Emp_ID.Value, Effect_Date_To, Report_To_ID);
                       
                        
                    }
                    else
                    {
                        DisplayMessage("Please Check Duty Cycle, WEF Date and Report To");
                    }
                }
                if (b != 0)
                {
                    Reset();
                    Fill_Grid_List();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    DisplayMessage("Record Saved", "green");
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
        }
        catch (Exception ex)
        {
            DisplayMessage("Record Not Saved");
        }
    }

    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Dc_Dt_Duty_Grid"] = null;
            if (Session["Dc_Dt_Duty_Grid"] == null)
            {
                DataTable Dt_Duty_Grid = new DataTable();
                Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
                Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
            }
            int b = 0;
            if (Txt_Emp_Name.Text == "")
            {
                DisplayMessage("Enter Employee Name");
                Txt_Emp_Name.Focus();
                return;
            }
            else if (Gv_Duty_Master_List == null || Gv_Duty_Master_List.Rows.Count == 0)
            {
                DisplayMessage("Select atleast one duty");
                Rbt_Group.Checked = true;
                Txt_Duty_Group.Visible = true;
                Txt_Duty_Group.Focus();
                return;
            }
            else
            {
                DataTable Dt_Duty_Grid = Session["Dc_Dt_Duty_Grid"] as DataTable;
                foreach (GridViewRow GVR in Gv_Duty_Master_List.Rows)
                {
                    HiddenField Hdn_Trans_ID_Master = GVR.FindControl("Hdn_Trans_ID_Master") as HiddenField;
                    Label Txt_Duty_Title_Master = GVR.FindControl("Txt_Duty_Title_Master") as Label;
                    Label Txt_Duty_Description_Master = GVR.FindControl("Txt_Duty_Description_Master") as Label;
                    DropDownList Ddl_Duty_Cycle_Master = GVR.FindControl("Ddl_Duty_Cycle_Master") as DropDownList;
                    TextBox Txt_WEF_Date_Master = GVR.FindControl("Txt_WEF_Date_Master") as TextBox;
                    TextBox Txt_Report_To_Master = GVR.FindControl("Txt_Report_To_Master") as TextBox;
                    Label Lbl_Create_by = GVR.FindControl("Lbl_Create_by") as Label;
                    Label Lbl_Modified_by = GVR.FindControl("Lbl_Modified_by") as Label;
                    Dt_Duty_Grid.Rows.Add(Hdn_Trans_ID_Master.Value, Txt_Duty_Title_Master.Text, Txt_Duty_Description_Master.Text, Ddl_Duty_Cycle_Master.SelectedValue.ToString(), Txt_WEF_Date_Master.Text, Txt_Report_To_Master.Text, Lbl_Create_by.Text, Lbl_Modified_by.Text);
                }
                foreach (DataRow DVR_Grid in Dt_Duty_Grid.Rows)
                {
                    if (DVR_Grid["Duty_Cycle"].ToString() != "0" && DVR_Grid["WEF_Date"].ToString() != "" && DVR_Grid["Report_To"].ToString() != "")
                    {
                        string Report_To_ID = string.Empty;
                        string[] Report_To = DVR_Grid["Report_To"].ToString().Split(',');
                        for (int i = 0; i < Report_To.Length; i++)
                        {
                            string Emp = Report_To[i].ToString();
                            int Emp_Start = Emp.IndexOf("(") + 1;
                            int Emp_End = Emp.IndexOf(")", Emp_Start);
                            Report_To_ID = Report_To_ID + Emp.Substring(Emp_Start, Emp_End - Emp_Start) + ",";
                        }
                        string Effect_Date_From = string.Empty;
                        string Effect_Date_To = string.Empty;
                        if (DVR_Grid["Duty_Cycle"].ToString() == "1")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddDays(0).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "2")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddDays(6).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "3")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddDays(3).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "4")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "5")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddMonths(3).AddDays(-1).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "6")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddMonths(6).AddDays(-1).ToString("dd-MMM-yyyy");
                        }
                        else if (DVR_Grid["Duty_Cycle"].ToString() == "7")
                        {
                            Effect_Date_From = DVR_Grid["WEF_Date"].ToString();
                            Effect_Date_To = Convert.ToDateTime(Effect_Date_From).AddYears(1).AddDays(-1).ToString("dd-MMM-yyyy");
                        }

                        if (Session["Dc_Dt_Duty_Edit"] == null)
                        {
                            b = DutyMaster.Set_DutyChart("0", Hdn_Emp_ID.Value, DVR_Grid["Trans_ID"].ToString(), DVR_Grid["Description"].ToString(), DVR_Grid["Duty_Cycle"].ToString(), DVR_Grid["WEF_Date"].ToString(), Report_To_ID, DVR_Grid["Report_To"].ToString(), "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1", Effect_Date_From, Effect_Date_To);
                            Send_Notification_Duty(DVR_Grid["Title"].ToString(), Effect_Date_From, DVR_Grid["Duty_Cycle"].ToString(), Hdn_Emp_ID.Value, Effect_Date_To, Report_To_ID);
                        }
                        else if (Session["Dc_Dt_Duty_Edit"] != null)
                        {
                            DataTable Dt_Duty_Edit = Session["Dc_Dt_Duty_Edit"] as DataTable;
                            if (Dt_Duty_Edit.Rows.Count == 0)
                            {
                                b = DutyMaster.Set_DutyChart("0", Hdn_Emp_ID.Value, DVR_Grid["Trans_ID"].ToString(), DVR_Grid["Description"].ToString(), DVR_Grid["Duty_Cycle"].ToString(), DVR_Grid["WEF_Date"].ToString(), Report_To_ID, DVR_Grid["Report_To"].ToString(), "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1", Effect_Date_From, Effect_Date_To);
                                Send_Notification_Duty(DVR_Grid["Title"].ToString(), Effect_Date_From, DVR_Grid["Duty_Cycle"].ToString(), Hdn_Emp_ID.Value, Effect_Date_To, Report_To_ID);
                            }
                            else if (Dt_Duty_Edit.Rows.Count > 0)
                            {
                                foreach (DataRow Drv_Edit in Dt_Duty_Edit.Rows)
                                {
                                    if ((Drv_Edit["Trans_ID"].ToString() == DVR_Grid["Trans_ID"].ToString()) && (Drv_Edit["Title"].ToString() != DVR_Grid["Title"].ToString() || Drv_Edit["Description"].ToString() != DVR_Grid["Description"].ToString() || Drv_Edit["Duty_Cycle"].ToString() != DVR_Grid["Duty_Cycle"].ToString() || Drv_Edit["WEF_Date"].ToString() != DVR_Grid["WEF_Date"].ToString() || Drv_Edit["Report_To"].ToString() != DVR_Grid["Report_To"].ToString()))
                                    {
                                        b = DutyMaster.Set_DutyChart("0", Hdn_Emp_ID.Value, DVR_Grid["Trans_ID"].ToString(), DVR_Grid["Description"].ToString(), DVR_Grid["Duty_Cycle"].ToString(), DVR_Grid["WEF_Date"].ToString(), Report_To_ID, DVR_Grid["Report_To"].ToString(), "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "3", Effect_Date_From, Effect_Date_To);
                                    }
                                    DataTable Delete_Dt = new DataView(Dt_Duty_Grid, "Trans_ID='" + Drv_Edit["Trans_ID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (Delete_Dt.Rows.Count == 0)
                                    {
                                        b = DutyMaster.Set_DutyChart("0", Hdn_Emp_ID.Value, Drv_Edit["Trans_ID"].ToString(), "", "", Drv_Edit["WEF_Date"].ToString(), Report_To_ID, Drv_Edit["Report_To"].ToString(), "0", "False", "False", Session["UserId"].ToString(), Session["UserId"].ToString(), "2", Effect_Date_From, Effect_Date_To);
                                    }
                                }
                                DataTable Insert_Dt = new DataView(Dt_Duty_Edit, "Trans_ID='" + DVR_Grid["Trans_ID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (Insert_Dt.Rows.Count == 0)
                                {
                                    b = DutyMaster.Set_DutyChart("0", Hdn_Emp_ID.Value, DVR_Grid["Trans_ID"].ToString(), DVR_Grid["Description"].ToString(), DVR_Grid["Duty_Cycle"].ToString(), DVR_Grid["WEF_Date"].ToString(), Report_To_ID, DVR_Grid["Report_To"].ToString(), "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "1", Effect_Date_From, Effect_Date_To);
                                }
                            }
                        }
                    }
                    else
                    {
                        DisplayMessage("Please Check Duty Cycle, WEF Date and Report To");
                    }
                }
                if (b != 0)
                {
                    Reset();
                    Fill_Grid_List();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    DisplayMessage("Record Saved", "green");
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
        }
        catch (Exception ex)
        {
            DisplayMessage("Record Not Saved");
        }
    }

    protected void Reset()
    {
        try
        {
            Edit_ID.Value = "";
            Hdn_Emp_ID.Value = "";
            Hdn_Report_To_ID.Value = "";
            Hdn_Report_To_Code.Value = "";
            Hdn_Report_To_Name.Value = "";
            Hdn_Emp_DOJ.Value = "";
            Hdn_Emp_Code.Value = "";
            Txt_Emp_Name.Text = "";
            txtReferenceEmployee.Text = "";
            Txt_Duty_Duty.Text = "";
            Txt_Duty_Group.Text = "";
            Rbt_Group.Enabled = true;
            Rbt_Duty.Enabled = true;


            Rbt_Group.Checked = false;
            Rbt_Duty.Checked = false;

            Txt_Duty_Duty.Visible = false;
            Txt_Duty_Group.Visible = true;
            Session["Dc_Dt_Duty_Grid"] = null;
            Session["Dc_Dt_Duty_Edit"] = null;
            Gv_Duty_Master_List.DataSource = Session["Dc_Dt_Duty_Edit"] as DataTable;
            Gv_Duty_Master_List.DataBind();
            Lbl_Tab_New.Text = Resources.Attendance.New;

            lblEmpTotalDailyDuties.Text = "";
            lblEmpTotalDailyDuties.Text = "";
            lblEmpTotalWeeklyDuties.Text = "";
            lblEmpTotalMonthlyDuties.Text = "";
        }
        catch
        {
        }
    }

    protected void Reset_Emp_Details()
    {
        try
        {
            Hdn_Emp_ID.Value = "";
            Hdn_Report_To_ID.Value = "";
            Hdn_Report_To_Code.Value = "";
            Hdn_Report_To_Name.Value = "";
            Hdn_Emp_DOJ.Value = "";
            Hdn_Emp_Code.Value = "";
            Txt_Duty_Duty.Text = "";
            Txt_Duty_Group.Text = "";
            Rbt_Group.Checked = true;
            Txt_Duty_Duty.Visible = false;
            Txt_Duty_Group.Visible = true;
            Session["Dc_Dt_Duty_Grid"] = null;
            Gv_Duty_Master_List.DataSource = Session["Dc_Dt_Duty_Grid"] as DataTable;
            Gv_Duty_Master_List.DataBind();
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

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Img"] = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            ViewState["Emp_Img"] = "";
        }

        return EmployeeName;
    }

    protected void Send_Notification_Duty(string Title, string Effect_Date_From, string Duty_Cycle, string Employee_ID, string Effect_Date_To, string Report_To)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        Dt_Request_Type = Obj_Notifiacation.Get_Notification_Type_Request("2");
        string Request_URL = "../Duty_Master/Duty_Feedback.aspx";
        string Message = string.Empty;
        Message = "Duty " + Title + " assign for " + GetEmployeeName(Employee_ID) + " on  " + Effect_Date_From + " and Due Date is " + Effect_Date_To + " Duty Cycle is " + Duty_Cycle;
        if (Report_To != "")
        {
            string[] Report_To_Send = Report_To.ToString().Split(',');
            for (int i = 0; i < Report_To_Send.Length - 1; i++)
            {
                Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Employee_ID, Report_To_Send[i].ToString(), Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
            }
        }
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Employee_ID, Employee_ID, Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
    }


    //--------------- End New ---------------
    protected void Get_Emp_Duty(string Employee_ID)
    {
        Session["Dc_Dt_Duty_Edit"] = null;
        Session["Dc_Dt_Duty_Grid"] = null;
        string Trans_ID = Employee_ID;
        //  Edit_ID.Value = Employee_ID;

        if (Session["Dc_Dt_Duty_Edit"] == null)
        {
            DataTable Dt_Duty_Edit = new DataTable();
            Dt_Duty_Edit.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
            Session["Dc_Dt_Duty_Edit"] = Dt_Duty_Edit;
        }

        if (Session["Dc_Dt_Duty_Grid"] == null)
        {
            DataTable Dt_Duty_Grid = new DataTable();
            Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
            Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
        }

        DataTable Dt_Duty_Master = DutyMaster.Get_Duty_Chart("0", Trans_ID, "0", "", "0", DateTime.Now.ToString(), "0", "0", "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "8");
        if (Dt_Duty_Master.Rows.Count > 0)
        {
            Txt_Emp_Name.Text = Dt_Duty_Master.Rows[0]["Employee_Name"].ToString();
            //    Txt_Emp_Name_TextChanged(null, null);
            DataTable Dt_Duty_Edit = Session["Dc_Dt_Duty_Edit"] as DataTable;
            foreach (DataRow drv in Dt_Duty_Master.Rows)
            {
                Dt_Duty_Edit.Rows.Add(drv["Trans_ID"].ToString(), drv["Title"].ToString(), drv["Description"].ToString(), drv["Duty_Cycle"].ToString(), GetDate(drv["WEF_Date"].ToString()), drv["Report_To"].ToString(), drv["Created_By"].ToString(), drv["Modified_By"].ToString());
            }
            Gv_Duty_Master_List.DataSource = Dt_Duty_Edit;
            Gv_Duty_Master_List.DataBind();
            Session["Dc_Dt_Duty_Edit"] = Dt_Duty_Edit;
            Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Edit;


            lblEmpTotalDuties.Text  = Dt_Duty_Edit.Rows.Count.ToString();
            lblEmpTotalDailyDuties.Text = (new DataView(Dt_Duty_Edit,"Duty_Cycle=1","",DataViewRowState.CurrentRows).ToTable()).Rows.Count.ToString();
            lblEmpTotalWeeklyDuties.Text = (new DataView(Dt_Duty_Edit, "Duty_Cycle=2", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count.ToString();
            lblEmpTotalMonthlyDuties.Text = (new DataView(Dt_Duty_Edit, "Duty_Cycle=4", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count.ToString();



        }
        else
        {
            //  Reset_Emp_Details();
            lblEmpTotalDailyDuties.Text = "";
            lblEmpTotalDailyDuties.Text = "";
            lblEmpTotalWeeklyDuties.Text = "";
            lblEmpTotalMonthlyDuties.Text = "";
        }
    }

    protected void Txt_Emp_Name_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Reset_Emp_Details();

            if (Txt_Emp_Name.Text != "")
            {
                Hdn_Emp_Code.Value = Txt_Emp_Name.Text.Split('/')[Txt_Emp_Name.Text.Split('/').Length - 1];

                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                DataTable Cur_Emp_Record = new DataView(dtEmp, "Emp_Code='" + Hdn_Emp_Code.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Cur_Emp_Record.Rows.Count > 0)
                {
                    Hdn_Emp_ID.Value = Cur_Emp_Record.Rows[0]["Emp_Id"].ToString();

                    Hdn_Emp_DOJ.Value = Cur_Emp_Record.Rows[0]["DOJ"].ToString();

                    if (Edit_ID.Value == "")
                    {
                        Get_Emp_Duty(Hdn_Emp_ID.Value);
                    }

                    //if (Edit_ID.Value == "")
                    //{
                    //    DataTable Dt_All_Active_Duty = new DataView(Session["Dc_Dt_Duty_Chart_List_Active"] as DataTable, "Emp_ID='" + Hdn_Emp_ID.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //    DataTable Dt_All_IN_Active_Duty = new DataView(Session["Dc_Dt_Duty_Chart_Bin_InActive"] as DataTable, "Emp_ID='" + Hdn_Emp_ID.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //    if (Dt_All_Active_Duty.Rows.Count > 0)
                    //    {
                    //        Reset();
                    //        DisplayMessage("Employee duties already exist in record. Please Update Employee Duties!");
                    //        return;
                    //    }
                    //    else if (Dt_All_IN_Active_Duty.Rows.Count > 0)
                    //    {
                    //        Reset();
                    //        DisplayMessage("Employee duties already exist in Bin list. Please Active and Update Employee Duties!");
                    //        return;
                    //    }
                    //}

                    DataTable Dt_Report_To = new DataView(dtEmp, "Emp_ID='" + Cur_Emp_Record.Rows[0]["Field5"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (Dt_Report_To.Rows.Count > 0)
                    {
                        Hdn_Report_To_ID.Value = Dt_Report_To.Rows[0]["Emp_ID"].ToString();
                        Hdn_Report_To_Code.Value = Dt_Report_To.Rows[0]["Emp_Code"].ToString();
                        Hdn_Report_To_Name.Value = Dt_Report_To.Rows[0]["Emp_Name"].ToString();
                    }
                }
            }
        }
        catch
        {
            Txt_Emp_Name.Text = "";
            Txt_Emp_Name.Focus();
            DisplayMessage("Employee Name not exist in record!");
        }
    }

    protected void Ref_Emp_Name_TextChanged(object sender, EventArgs e)
    {
        try
        {


            if (txtReferenceEmployee.Text != "")
            {
                string refEmployeeCode = Txt_Emp_Name.Text.Split('/')[Txt_Emp_Name.Text.Split('/').Length - 1];

                DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                DataTable Cur_Emp_Record = new DataView(dtEmp, "Emp_Code='" + refEmployeeCode + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Cur_Emp_Record.Rows.Count > 0)
                {
                   
                }
            }
        }
        catch
        {
            Txt_Emp_Name.Text = "";
            Txt_Emp_Name.Focus();
            DisplayMessage("Employee Name not exist in record!");
        }
    }


    protected void Rbt_Group_CheckedChanged(object sender, EventArgs e)
    {
        if (Rbt_Group.Checked == true)
        {
            Txt_Duty_Duty.Visible = false;
            Txt_Duty_Group.Visible = true;
        }
        else
        {
            Txt_Duty_Duty.Visible = true;
            Txt_Duty_Group.Visible = false;
        }
    }

    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }
        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);
        return dTable;
    }

    protected void Btn_Add_Group_Click(object sender, EventArgs e)
    {
        if (Txt_Emp_Name.Text != "")
        {
            Session["Dc_Dt_Duty_Grid"] = null;
            if (Session["Dc_Dt_Duty_Grid"] == null)
            {
                DataTable Dt_Duty_Grid = new DataTable();
                Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
                Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
            }

            if (Rbt_Duty.Checked == true)
            {
                if (Txt_Duty_Duty.Text != "")
                {
                    DataTable Dt_Duty_Master = DutyMaster.Get_Duty_Master("0", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Txt_Duty_Duty.Text, "", "", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "4");
                    if (Dt_Duty_Master.Rows.Count > 0)
                    {
                        foreach (GridViewRow gvr in Gv_Duty_Master_List.Rows)
                        {
                            HiddenField Hdn_Trans_ID_Master = (HiddenField)gvr.Cells[1].FindControl("Hdn_Trans_ID_Master");
                            if (Hdn_Trans_ID_Master.Value == Dt_Duty_Master.Rows[0]["Trans_ID"].ToString())
                            {
                                Txt_Duty_Duty.Text = "";
                                Txt_Duty_Duty.Focus();
                                return;
                            }
                        }
                        DataTable Dt_Duty_Grid = Session["Dc_Dt_Duty_Grid"] as DataTable;
                        foreach (GridViewRow GVR in Gv_Duty_Master_List.Rows)
                        {
                            HiddenField Hdn_Trans_ID_Master = GVR.FindControl("Hdn_Trans_ID_Master") as HiddenField;
                            Label Txt_Duty_Title_Master = GVR.FindControl("Txt_Duty_Title_Master") as Label;
                            Label Txt_Duty_Description_Master = GVR.FindControl("Txt_Duty_Description_Master") as Label;
                            DropDownList Ddl_Duty_Cycle_Master = GVR.FindControl("Ddl_Duty_Cycle_Master") as DropDownList;
                            TextBox Txt_WEF_Date_Master = GVR.FindControl("Txt_WEF_Date_Master") as TextBox;
                            TextBox Txt_Report_To_Master = GVR.FindControl("Txt_Report_To_Master") as TextBox;
                            Label Lbl_Create_by = GVR.FindControl("Lbl_Create_by") as Label;
                            Label Lbl_Modified_by = GVR.FindControl("Lbl_Modified_by") as Label;
                            Dt_Duty_Grid.Rows.Add(Hdn_Trans_ID_Master.Value, Txt_Duty_Title_Master.Text, Txt_Duty_Description_Master.Text, Ddl_Duty_Cycle_Master.SelectedValue.ToString(), Txt_WEF_Date_Master.Text, Txt_Report_To_Master.Text, Lbl_Create_by.Text, Lbl_Modified_by.Text);
                        }
                        string Report_To_emp = Hdn_Report_To_Name.Value + "(" + Hdn_Report_To_Code.Value + ")";
                        if (Report_To_emp == "()")
                            Report_To_emp = "";
                        //Dt_Duty_Grid.Rows.Add(Dt_Duty_Master.Rows[0]["Trans_ID"].ToString(), Dt_Duty_Master.Rows[0]["Title"].ToString(), Dt_Duty_Master.Rows[0]["Description"].ToString(), Dt_Duty_Master.Rows[0]["Duty_Cycle"].ToString(), GetDate(Hdn_Emp_DOJ.Value), Report_To_emp);
                        Dt_Duty_Grid.Rows.Add(Dt_Duty_Master.Rows[0]["Trans_ID"].ToString(), Dt_Duty_Master.Rows[0]["Title"].ToString(), Dt_Duty_Master.Rows[0]["Description"].ToString(), Dt_Duty_Master.Rows[0]["Duty_Cycle"].ToString(), GetDate(DateTime.Now.ToString()), Report_To_emp, GetEmployeeName(Session["EmpID"].ToString()), "");
                        Gv_Duty_Master_List.DataSource = Dt_Duty_Grid;
                        Gv_Duty_Master_List.DataBind();
                        Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
                        Txt_Duty_Duty.Text = "";
                        Txt_Duty_Duty.Focus();
                    }
                    else
                    {
                        Txt_Duty_Duty.Text = "";
                        Txt_Duty_Duty.Focus();
                    }
                }
            }
            else if (Rbt_Group.Checked == true)
            {
                if (Txt_Duty_Group.Text != "")
                {
                    DataTable Dt_Duty_Master = DutyMaster.Get_Duty_Group("0", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Txt_Duty_Group.Text.Trim(), "", "0", "True", Session["UserID"].ToString(), DateTime.Now.ToString(), Session["UserID"].ToString(), DateTime.Now.ToString(), "6");
                    if (Dt_Duty_Master.Rows.Count > 0)
                    {
                        DataTable Dt_Duty_Grid = Session["Dc_Dt_Duty_Grid"] as DataTable;
                        foreach (GridViewRow GVR in Gv_Duty_Master_List.Rows)
                        {
                            HiddenField Hdn_Trans_ID_Master = GVR.FindControl("Hdn_Trans_ID_Master") as HiddenField;
                            Label Txt_Duty_Title_Master = GVR.FindControl("Txt_Duty_Title_Master") as Label;
                            Label Txt_Duty_Description_Master = GVR.FindControl("Txt_Duty_Description_Master") as Label;
                            DropDownList Ddl_Duty_Cycle_Master = GVR.FindControl("Ddl_Duty_Cycle_Master") as DropDownList;
                            TextBox Txt_WEF_Date_Master = GVR.FindControl("Txt_WEF_Date_Master") as TextBox;
                            TextBox Txt_Report_To_Master = GVR.FindControl("Txt_Report_To_Master") as TextBox;
                            Label Lbl_Create_by = GVR.FindControl("Lbl_Create_by") as Label;
                            Label Lbl_Modified_by = GVR.FindControl("Lbl_Modified_by") as Label;
                            Dt_Duty_Grid.Rows.Add(Hdn_Trans_ID_Master.Value, Txt_Duty_Title_Master.Text, Txt_Duty_Description_Master.Text, Ddl_Duty_Cycle_Master.SelectedValue.ToString(), Txt_WEF_Date_Master.Text, Txt_Report_To_Master.Text, Lbl_Create_by.Text, Lbl_Modified_by.Text);
                        }

                        foreach (DataRow Dvr in Dt_Duty_Master.Rows)
                        {
                            string Report_To_emp = Hdn_Report_To_Name.Value + "(" + Hdn_Report_To_Code.Value + ")";
                            if (Report_To_emp == "()")
                                Report_To_emp = "";
                            Dt_Duty_Grid.Rows.Add(Dvr["Trans_ID"].ToString(), Dvr["Title"].ToString(), Dvr["Description"].ToString(), Dvr["Duty_Cycle"].ToString(), GetDate(DateTime.Now.ToString()), Report_To_emp, GetEmployeeName(Session["EmpID"].ToString()), "");
                            //Dt_Duty_Grid.Rows.Add(Dvr["Trans_ID"].ToString(), Dvr["Title"].ToString(), Dvr["Description"].ToString(), Dvr["Duty_Cycle"].ToString(), GetDate(Hdn_Emp_DOJ.Value), Report_To_emp);
                        }

                        Session["Dc_Dt_Duty_Grid"] = RemoveDuplicateRows(Dt_Duty_Grid, "Trans_ID");
                        Gv_Duty_Master_List.DataSource = Session["Dc_Dt_Duty_Grid"] as DataTable;
                        Gv_Duty_Master_List.DataBind();
                        Txt_Duty_Group.Text = "";
                        Txt_Duty_Group.Focus();
                    }
                    else
                    {
                        Txt_Duty_Group.Text = "";
                        Txt_Duty_Group.Focus();
                    }
                }
            }
        }
        else
        {
            Txt_Duty_Duty.Text = "";
            Txt_Duty_Group.Text = "";
            Txt_Emp_Name.Focus();
            DisplayMessage("Enter Employee Name");
        }
    }

    protected void Gv_Duty_Master_List_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void Txt_WEF_Date_TextChanged(object sender, EventArgs e)
    {

        if (Convert.ToDateTime(((TextBox)sender).Text) <= Convert.ToDateTime(DateTime.Now.ToString()))
        {
            ((TextBox)sender).Text = "";
            DisplayMessage("You cannot select a date earlier than date " + GetDate(DateTime.Now.ToString()) + "!");
        }


        //if (Hdn_Emp_DOJ.Value != "")
        //{
        //    if(Convert.ToDateTime(((TextBox)sender).Text) < Convert.ToDateTime(Hdn_Emp_DOJ.Value))
        //    {
        //        ((TextBox)sender).Text = "";
        //        DisplayMessage("You cannot select a date earlier employee joining date " + GetDate(Hdn_Emp_DOJ.Value) + "!");
        //    }
        //}
        //else if(((TextBox)sender).Text!="")
        //{

        //}
        //else
        //{
        //    ((TextBox)sender).Text = "";
        //    DisplayMessage("Enter Employee Name!");
        //}
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

    protected void IBtn_Delete_Duty_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        DataTable Dt_Duty_Grid_Delete = Session["Dc_Dt_Duty_Grid"] as DataTable;
        if (Dt_Duty_Grid_Delete != null)
        {
            if (Dt_Duty_Grid_Delete.Rows.Count > 0)
                Dt_Duty_Grid_Delete = new DataView(Dt_Duty_Grid_Delete, "TRans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        Gv_Duty_Master_List.DataSource = Dt_Duty_Grid_Delete;
        Gv_Duty_Master_List.DataBind();
        Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid_Delete;
    }

    protected void btnGetDuties_Click(object sender, EventArgs e)
    {
        Session["Dc_Dt_Duty_Grid"] = null;
        if (Session["Dc_Dt_Duty_Grid"] == null)
        {
            DataTable Dt_Duty_Grid = new DataTable();
            Dt_Duty_Grid.Columns.AddRange(new DataColumn[8] { new DataColumn("Trans_ID", typeof(int)), new DataColumn("Title"), new DataColumn("Description"), new DataColumn("Duty_Cycle"), new DataColumn("WEF_Date"), new DataColumn("Report_To"), new DataColumn("Created_By"), new DataColumn("Modified_By") });
            Session["Dc_Dt_Duty_Grid"] = Dt_Duty_Grid;
        }
        DataTable Dt_Duty_Master = objDa.return_DataTable("Select Trans_ID ,Title,Description ,Duty_Cycle From HR_DutyMaster Where Designation_ID = " + Ddl_Designation.SelectedValue.ToString() + " and Is_Active ='1'");
        if (Dt_Duty_Master.Rows.Count > 0)
        {
            DataTable Dt_Duty_Grid = Session["Dc_Dt_Duty_Grid"] as DataTable;
            foreach (GridViewRow GVR in Gv_Duty_Master_List.Rows)
            {
                HiddenField Hdn_Trans_ID_Master = GVR.FindControl("Hdn_Trans_ID_Master") as HiddenField;
                Label Txt_Duty_Title_Master = GVR.FindControl("Txt_Duty_Title_Master") as Label;
                Label Txt_Duty_Description_Master = GVR.FindControl("Txt_Duty_Description_Master") as Label;
                DropDownList Ddl_Duty_Cycle_Master = GVR.FindControl("Ddl_Duty_Cycle_Master") as DropDownList;
                TextBox Txt_WEF_Date_Master = GVR.FindControl("Txt_WEF_Date_Master") as TextBox;
                TextBox Txt_Report_To_Master = GVR.FindControl("Txt_Report_To_Master") as TextBox;
                Label Lbl_Create_by = GVR.FindControl("Lbl_Create_by") as Label;
                Label Lbl_Modified_by = GVR.FindControl("Lbl_Modified_by") as Label;
                Dt_Duty_Grid.Rows.Add(Hdn_Trans_ID_Master.Value, Txt_Duty_Title_Master.Text, Txt_Duty_Description_Master.Text, Ddl_Duty_Cycle_Master.SelectedValue.ToString(), Txt_WEF_Date_Master.Text, Txt_Report_To_Master.Text, Lbl_Create_by.Text, Lbl_Modified_by.Text);
            }

            foreach (DataRow Dvr in Dt_Duty_Master.Rows)
            {
                string Report_To_emp = Hdn_Report_To_Name.Value + "(" + Hdn_Report_To_Code.Value + ")";
                if (Report_To_emp == "()")
                    Report_To_emp = "";
                Dt_Duty_Grid.Rows.Add(Dvr["Trans_ID"].ToString(), Dvr["Title"].ToString(), Dvr["Description"].ToString(), Dvr["Duty_Cycle"].ToString(), GetDate(DateTime.Now.ToString()), Report_To_emp, GetEmployeeName(Session["EmpID"].ToString()), "");
                //Dt_Duty_Grid.Rows.Add(Dvr["Trans_ID"].ToString(), Dvr["Title"].ToString(), Dvr["Description"].ToString(), Dvr["Duty_Cycle"].ToString(), GetDate(Hdn_Emp_DOJ.Value), Report_To_emp);
            }

            Session["Dc_Dt_Duty_Grid"] = RemoveDuplicateRows(Dt_Duty_Grid, "Trans_ID");
            Gv_Duty_Master_List.DataSource = Session["Dc_Dt_Duty_Grid"] as DataTable;
            Gv_Duty_Master_List.DataBind();
            Txt_Duty_Group.Text = "";
            Txt_Duty_Group.Focus();
        }
        else
        {
            Txt_Duty_Group.Text = "";
            Txt_Duty_Group.Focus();
        }


    }
}