using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
public partial class ProjectManagement_ProjectTeam : BasePage
{
    Common cmn = null;
    Prj_ProjectMaster objProjctMaster = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmpmaster = null;
    Prj_ProjectTeam objProjectteam = null;
    EmployeeParameter objEmpParam = null;
    Att_ScheduleMaster objEmpSch = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objProjectteam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../projectmanagement/projectteam.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            getprojectteam();
            //txtOnDutyTime.Text = "00:00:00";
            //txtOffDutyTime.Text = "00:00:00";
            //ddlOption.SelectedIndex = 1;
            pnlgrid.Visible = false;
            btList_Click(null, null);
        }
        Fillddlprojectname();
        FillEmployeeName();
        //AllPageCode();;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        GvrProjectteam.Columns[0].Visible = clsPagePermission.bEdit;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnsubmit.Visible = clsPagePermission.bAdd;
        btnCheckTeamAvailable.Visible = clsPagePermission.bAdd;
        chkEligibleIncentive.Visible = clsPagePermission.bPayCommission;
    }
    public void Fillddlprojectname()
    {
        ListEditItem Li1 = new ListEditItem();
        Li1.Text = "---select---";
        //Li1.Value = "0";
        string sttrValue = string.Empty;
        try
        {
            sttrValue = ddlprojectname.Value.ToString();
        }
        catch
        {
        }
        DataTable dtProjectMAster = objProjctMaster.GetAllRecordProjectMasteer();
        try
        {
            //  dtProjectMAster = new DataView(dtProjectMAster, "Field4='" + Session["CompId"].ToString() + "' and Field5='" + Session["BrandId"].ToString() + "' and Field6='" + Session["LocId"].ToString() + "'", "Project_Name", DataViewRowState.CurrentRows).ToTable();
            dtProjectMAster = new DataView(dtProjectMAster, "Field4='" + Session["CompId"].ToString() + "'", "Project_Name", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtProjectMAster.Rows.Count > 0)
        {
            ddlprojectname.DataSource = dtProjectMAster;
            ddlprojectname.DataBind();
            ddlprojectname.Items.Insert(0, Li1);
            ddlprojectname.SelectedIndex = 0;
        }
        else
        {
            ddlprojectname.Items.Insert(0, Li1);
        }
        ddlprojectname.Value = sttrValue;
        dtProjectMAster.Dispose();
    }
    public void FillEmployeeName()
    {
        EmployeeMaster ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        if (HiddeniD.Value=="")
        {
            dt = ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        }
        else
        {
            dt = ObjEmp.GetEmployeeMasterDetailsForProjectTeam(HttpContext.Current.Session["CompId"].ToString());
        }
        ListEditItem Li1 = new ListEditItem();
        Li1.Text = "---select---";
        //Li1.Value = "0";
        string sttrValue = string.Empty;
        try
        {
            sttrValue = ddlEmployeeName.Value.ToString();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ddlEmployeeName.DataSource = dt;
            ddlEmployeeName.DataBind();
            ddlEmployeeName.Items.Insert(0, Li1);
            ddlEmployeeName.SelectedIndex = 0;
        }
        else
        {
            ddlEmployeeName.Items.Insert(0, Li1);
        }
        ddlEmployeeName.Value = sttrValue;
        dt.Dispose();
    }
    public void getprojectteam()
    {
        DataTable dtProjectrecord = new DataTable();
        dtProjectrecord = objProjectteam.GetAllProjectTeam();
        try
        {
            dtProjectrecord = new DataView(dtProjectrecord, "Field4='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (ddlProjectStatus.SelectedIndex > 0)
        {
            try
            {
                dtProjectrecord = new DataView(dtProjectrecord, "Project_Title='" + ddlProjectStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        dtProjectrecord = dtProjectrecord.DefaultView.ToTable(true, "Project_Id", "Field7", "Project_Name", "Customername", "ProjectTeam", "ManagerName");
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dtProjectrecord, "", "");
        Session["dtprojectteamrecord"] = dtProjectrecord;
        Session["dtFilter_Project_Team"] = dtProjectrecord;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtProjectrecord.Rows.Count + "";
        //AllPageCode();;
    }
    protected void ddlProjectStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        getprojectteam();
    }
    public void bindgriddetials()
    {
        pnlgrid.Visible = true;
        DataTable DtBindGrid = new DataTable();
        DtBindGrid = objProjectteam.GetRecordByProjectId("", HiddeniD.Value);
        if (DtBindGrid.Rows.Count > 0)
        {
            pnlgrid.Visible = true;
            ddlprojectname_SelectedIndexChanged(null, null);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)grvteamlistDetailrecord, DtBindGrid, "", "");
            Session["dtFilterRecord"] = DtBindGrid;
        }
        else
        {
            DataTable Dtclear = new DataTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)grvteamlistDetailrecord, Dtclear, "", "");
        }
        pnlgrid.Visible = true;
        //AllPageCode();;
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml(" #90bde9");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnllist.Visible = false;
        pnlgrid.Visible = false;
        pnlteamdetials.Visible = true;
        // ddlempname.Items.Clear();
        Fillddlprojectname();
        chktaskvisibility.Checked = false;
        ddlprojectname.Focus();
    }
    protected void grvteamlistDetailrecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvteamlistDetailrecord.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterRecord"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)grvteamlistDetailrecord, dt, "", "");
        //AllPageCode();;
    }

    protected void GvrProjectteam_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Project_Team"];
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
        Session["dtFilter_Project_Team"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dt, "", "");
        //AllPageCode();;
    }
    protected void btList_Click(object sender, EventArgs e)
    {
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnllist.Visible = true;
        pnlgrid.Visible = false;
        pnlteamdetials.Visible = false;
        getprojectteam();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        hidProId.Value = "";
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml(" #90bde9");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnllist.Visible = false;
        pnlteamdetials.Visible = true;
        pnlgrid.Visible = true;
        HiddeniD.Value = e.CommandArgument.ToString();
        DataTable Dt = new DataTable();
        Dt = objProjectteam.GetRecordByProjectId("", HiddeniD.Value);
        if (Dt.Rows.Count > 0)
        {
            //ddlprojectname_SelectedIndexChanged(null, null);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)grvteamlistDetailrecord, Dt, "", "");
            Session["dtFilterRecord"] = Dt;
            hidProId.Value = Dt.Rows[0]["Project_Id"].ToString();
            lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + Dt.Rows.Count + "";
        }
        else
        {
            DataTable Dtclear = new DataTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)grvteamlistDetailrecord, Dtclear, "", "");
        }
        Fillddlprojectname();
        ddlprojectname.Value = hidProId.Value;
        //AllPageCode();;
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void btnCheckTeamAvailable_Click(object sender, EventArgs e)
    {
        string strProjectId = string.Empty;
        if (ddlprojectname.SelectedIndex > 0)
        {
            strProjectId = ddlprojectname.Value.ToString();
        }
        else
        {
            strProjectId = "0";
        }
        string strCmd = string.Format("window.open('../ProjectManagement/ProjectDetail.aspx?Project_Id=" + strProjectId + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        int i = 0;
        if (ddlprojectname.SelectedIndex <= 0)
        {
            DisplayMessage("Select Project Name");
            return;
        }
        else if (ddlEmployeeName.SelectedIndex <= 0)
        {
            DisplayMessage("Select Employee Name");
            return;
        }
        //here we are cheecking that on duty time and off duty time is entered or not 
        if (txtOnDutyTime.Text == "" || txtOnDutyTime.Text == "00:00:00")
        {
            DisplayMessage("Enter on duty time");
            txtOnDutyTime.Focus();
            return;
        }
        if (txtOffDutyTime.Text == "" || txtOffDutyTime.Text == "00:00:00")
        {
            DisplayMessage("Enter off duty time");
            txtOffDutyTime.Focus();
            return;
        }
        if (txtBasicSalary.Text == "")
        {
            txtBasicSalary.Text = "0";
        }
        if (hidTransId.Value != "")
        {
            objProjectteam.UpdateProjcetTeam(HiddeniD.Value.ToString(), ddlprojectname.Value.ToString(), hdnemp.Value.ToString(), chktaskvisibility.Checked.ToString(), txtOnDutyTime.Text, txtOffDutyTime.Text, txtBasicSalary.Text, chkEligibleIncentive.Checked.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Updated","green");
            hdnemp.Value = "";
            chktaskvisibility.Checked = false;
            ddlEmployeeName.ReadOnly = false;
            if (chktaskvisibility.Checked)
            {
                ddlprojectname.SelectedIndex = 0;
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            hidTransId.Value = "";
            DataTable DtBindGrid1 = new DataTable();
            DtBindGrid1 = objProjectteam.GetRecordByProjectId("", hidProId.Value.ToString());
            if (DtBindGrid1.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)grvteamlistDetailrecord, DtBindGrid1, "", "");
            }
            ddlprojectname.SelectedIndex = 0;
            ddlprojectname.Value = hidProId.Value;
            ddlEmployeeName.ReadOnly = false;
        }
        else
        {
            DataTable dt = objProjectteam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
            dt = new DataView(dt, "Project_Id=" + ddlprojectname.Value + " and Emp_Id=" + hdnemp.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                ddlEmployeeName.SelectedIndex = 0;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlEmployeeName);
                hdnemp.Value = "";
                DisplayMessage("Employee Name Already Exists");
                return;
            }
            i = 0;
            i = Convert.ToInt32(ddlprojectname.Value.ToString());
            objProjectteam.InsertProjectTeam(ddlprojectname.Value.ToString(), hdnemp.Value.ToString(), chktaskvisibility.Checked.ToString(), "", txtOnDutyTime.Text, txtOffDutyTime.Text, txtBasicSalary.Text, chkEligibleIncentive.Checked.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Saved", "green");
            ddlEmployeeName.SelectedIndex = 0;
            hdnemp.Value = "";
            chktaskvisibility.Checked = false;
            ddlprojectname_SelectedIndexChanged(null, null);
            DataTable DtBindGrid2 = new DataTable();
            DtBindGrid2 = objProjectteam.GetRecordByProjectId("", i.ToString());
            if (DtBindGrid2.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)grvteamlistDetailrecord, DtBindGrid2, "", "");
            }
            ddlprojectname_SelectedIndexChanged(null, null);
            ddlprojectname.Value = i.ToString();
        }
        chktaskvisibility.Checked = false;
        chkEligibleIncentive.Checked = false;
        hdnemp.Value = "";
        hdnProjId.Value = "";
        hdnfileid.Value = "";
        HidCustId.Value = "";
        hidProId.Value = "";
        hidTransId.Value = "";
        txtOnDutyTime.Text = "00:00:00";
        txtOffDutyTime.Text = "000:000:00";
        txtBasicSalary.Text = "";
        chktaskvisibility.Enabled = true;
        //AllPageCode();;
        ddlEmployeeName.SelectedIndex = 0;
    }
    public string[] GetEmployeeInfo(string strEmpId)
    {
        string[] strEmp = new string[3];
        try
        {
            strEmp[0] = objEmpParam.GetEmployeeParameterByEmpId(strEmpId, Session["CompId"].ToString()).Rows[0]["Basic_Salary"].ToString();
        }
        catch
        {
            strEmp[0] = "0";
        }
        try
        {
            DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(strEmpId);
            dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + DateTime.Now.ToString() + "' and Att_Date<='" + DateTime.Now.ToString() + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
            if (dtShiftAllDate.Rows.Count > 0)
            {
                strEmp[1] = dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString();
                strEmp[2] = dtShiftAllDate.Rows[0]["OffDuty_Time"].ToString();
            }
            else
            {
                strEmp[1] = "00:00:00";
                strEmp[2] = "00:00:00";
            }
            if (strEmp[1] == "")
            {
                strEmp[1] = "00:00:00";
            }
            if (strEmp[2] == "")
            {
                strEmp[2] = "00:00:00";
            }
        }
        catch
        {
            strEmp[1] = "00:00:00";
            strEmp[2] = "00:00:00";
        }
        return strEmp;
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        chktaskvisibility.Checked = false;
        ddlEmployeeName.ReadOnly = false;
        chkEligibleIncentive.Checked = false;
        ddlprojectname_SelectedIndexChanged(null, null);
    }
    protected void GvrProjectteam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvrProjectteam.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Project_Team"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dt, "", "");
        //AllPageCode();;
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
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
            DataTable dtProjectteam = (DataTable)Session["dtprojectteamrecord"];
            if (dtProjectteam != null)
            {
                DataView view = new DataView(dtProjectteam, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvrProjectteam, view.ToTable(), "", "");
                Session["dtFilter_Project_Team"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                //AllPageCode();;
            }
        }
        txtValue.Focus();
    }
    protected void btnbindrpt_Click1(object sender, ImageClickEventArgs e)
    {
        if (ddlOption1.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption1.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName1.SelectedValue + ",System.String)='" + txtValueteam.Text + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName1.SelectedValue + ",System.String) like '%" + txtValueteam.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName1.SelectedValue + ",System.String) Like '" + txtValueteam.Text + "%'";
            }
            DataTable dtProjectteam = (DataTable)Session["dtFilterRecord"];
            if (dtProjectteam != null)
            {
                DataView view = new DataView(dtProjectteam, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)grvteamlistDetailrecord, view.ToTable(), "", "");
                Session["dtFilter_Project_Team"] = view.ToTable();
                lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                //AllPageCode();;
            }
        }
        txtValueteam.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        getprojectteam();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }
    protected void btnRefresh_Click1(object sender, ImageClickEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterRecord"];
        try
        {
            if (dt.Rows.Count > 0)
            {
                lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)grvteamlistDetailrecord, dt, "", "");
            }
            else
            {
                lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + 0 + "";
            }
        }
        catch
        {
            lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + 0 + "";
        }
        ddlFieldName1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 1;
        txtValueteam.Text = "";
        //AllPageCode();;
    }
    protected void ddlprojectname_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlgrid.Visible = true;
        if (ddlprojectname.SelectedIndex <= 0)
        {
            Session["dtFilterRecord"] = null;
            grvteamlistDetailrecord.DataSource = null;
            grvteamlistDetailrecord.DataBind();
            //DisplayMessage("Select Project Name");
            lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + 0 + "";
        }
        else
        {
            DataTable dtBind = new DataTable();
            ddlEmployeeName.SelectedIndex = 0;
            txtOnDutyTime.Text = "00:00:00";
            txtOffDutyTime.Text = "00:00:00";
            txtBasicSalary.Text = "";
            dtBind = objProjectteam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
            if (dtBind.Rows.Count > 0)
            {
                Session["ProjectId"] = dtBind.Rows[0]["Project_Id"].ToString();
                Session["dtFilterRecord"] = dtBind;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)grvteamlistDetailrecord, dtBind, "", "");
                lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + dtBind.Rows.Count + "";
            }
            else
            {
                DataTable Dtclear = new DataTable();
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)grvteamlistDetailrecord, Dtclear, "", "");
                ddlEmployeeName.SelectedIndex = 0;
                txtOnDutyTime.Text = "00:00:00";
                txtOffDutyTime.Text = "00:00:00";
                txtBasicSalary.Text = "";
                Session["dtFilterRecord"] = null;
                lblTotalTeamRecords.Text = Resources.Attendance.Total_Records + " : " + Dtclear.Rows.Count + "";
            }
            dtBind.Dispose();
        }
        //AllPageCode();;
    }
    protected void btnDeleteGrid_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        if (((Label)gvrow.FindControl("lblProjectManager")).Text.Trim() == "Project Manager")
        {
            DisplayMessage("project manager can not be deleted");
            return;
        }
        string TransId = string.Empty;
        TransId = e.CommandName.ToString();
        objProjectteam.DeleteProjectTeam(TransId);
        DisplayMessage("Record Deleted", "green");
        DataTable DtBindGrid = objProjectteam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
        pnlgrid.Visible = true;
        ddlprojectname_SelectedIndexChanged(null, null);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)grvteamlistDetailrecord, DtBindGrid, "", "");
        Session["dtFilterRecord"] = DtBindGrid;
        //AllPageCode();;
    }
    protected void btnEditGrid_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        if (((Label)gvrow.FindControl("lblProjectManager")).Text.Trim() == "Project Manager")
        {
            chktaskvisibility.Enabled = false;
        }
        hdnemp.Value = "";
        hidTransId.Value = "";
        int projectid = 0;
        int empid = 0;
        hidProId.Value = "";
        HiddeniD.Value = e.CommandArgument.ToString();
        hidTransId.Value = HiddeniD.Value;
        DataTable DtFill = new DataTable();
        string strtransid = HiddeniD.Value;
        Session["TeamTransId"] = strtransid;
        DtFill = objProjectteam.GetRecordByTransId(strtransid);
        //DtFill = new DataView(DtFill, "Trans_Id=" + HiddeniD.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        if (DtFill.Rows.Count > 0)
        {
            hdnemp.Value = DtFill.Rows[0]["Emp_Id"].ToString();
            ddlEmployeeName.Value = DtFill.Rows[0]["Emp_Id"].ToString();
            hidProId.Value = DtFill.Rows[0]["Project_Id"].ToString();
            //txtEmpName.Text = DtFill.Rows[0]["Emp_Name"].ToString();
            ddlEmployeeName.ReadOnly = true;
            ddlprojectname.Value = DtFill.Rows[0]["Project_Id"].ToString();
            if (DtFill.Rows[0]["Task_Visibility"].ToString() == "True")
            {
                chktaskvisibility.Checked = true;
            }
            else
            {
                chktaskvisibility.Checked = false;
            }
            txtOnDutyTime.Text = DtFill.Rows[0]["Field2"].ToString();
            txtOffDutyTime.Text = DtFill.Rows[0]["Field3"].ToString();
            txtBasicSalary.Text = DtFill.Rows[0]["Field4"].ToString();
            chkEligibleIncentive.Checked = DtFill.Rows[0]["Field5"].ToString().ToLower() == "true" ? true : false;
            if (txtBasicSalary.Text == "" || txtBasicSalary.Text == "0")
            {
                txtBasicSalary.Text = GetEmployeeInfo(DtFill.Rows[0]["Emp_Id"].ToString())[0].ToString();
            }
            if (txtOnDutyTime.Text == "")
            {
                txtOnDutyTime.Text = GetEmployeeInfo(DtFill.Rows[0]["Emp_Id"].ToString())[1].ToString();
            }
            if (txtOffDutyTime.Text == "")
            {
                txtOffDutyTime.Text = GetEmployeeInfo(DtFill.Rows[0]["Emp_Id"].ToString())[2].ToString();
            }
        }
        foreach (GridViewRow gvr in grvteamlistDetailrecord.Rows)
        {
            HiddenField hdntransid = (HiddenField)gvr.FindControl("hdntrans");
            Label lblprojectname = (Label)gvr.FindControl("lblpojectname");
            Label lblempname = (Label)gvr.FindControl("lblEmpIdList");
            HiddenField hdnprojectid = (HiddenField)gvr.FindControl("hdnprojectid");
            HiddenField hdnempid = (HiddenField)gvr.FindControl("hdnempid");
            projectid = Convert.ToInt32(hdnprojectid.Value);
            empid = Convert.ToInt32(hdnempid.Value);
            Session["TransId"] = hdntransid.Value;
        }
    }
    protected void ddlEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnProjId.Value = "";
        hdnemp.Value = "";
        txtOnDutyTime.Text = "00:00:00";
        txtOffDutyTime.Text = "00:00:00";
        txtBasicSalary.Text = "";
        HiddeniD.Value = "";
        hidTransId.Value = "";
        chktaskvisibility.Checked = false;
        string empid = string.Empty;
        if (ddlEmployeeName.SelectedIndex > 0)
        {
            try
            {
                empid = ddlEmployeeName.Value.ToString();
            }
            catch
            {
                empid = "0";
            }
            DataTable dtEmp = objEmpmaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Emp_Id='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                try
                {
                    txtBasicSalary.Text = GetEmployeeInfo(empid)[0].ToString();
                    txtOnDutyTime.Text = GetEmployeeInfo(empid)[1].ToString();
                    txtOffDutyTime.Text = GetEmployeeInfo(empid)[2].ToString();
                }
                catch
                {
                }
                if (ddlprojectname.SelectedIndex > 0)
                {
                    DataTable dtBind = objProjectteam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
                    dtBind = new DataView(dtBind, "Emp_id=" + dtEmp.Rows[0]["Emp_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtBind.Rows.Count > 0)
                    {
                        if (dtBind.Rows[0]["Task_Visibility"].ToString() == "True")
                        {
                            chktaskvisibility.Checked = true;
                        }
                        txtOnDutyTime.Text = dtBind.Rows[0]["Field2"].ToString();
                        txtOffDutyTime.Text = dtBind.Rows[0]["Field3"].ToString();
                        txtBasicSalary.Text = dtBind.Rows[0]["Field4"].ToString();
                        hidProId.Value = dtBind.Rows[0]["Project_Id"].ToString();
                        HiddeniD.Value = dtBind.Rows[0]["Trans_id"].ToString();
                        hidTransId.Value = HiddeniD.Value;
                    }
                }
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                hidemployeeid.Value = empid;
                hdnemp.Value = empid;
                chktaskvisibility.Focus();
            }

        }
    }
    protected void txtOnDutyTime_TextChanged(object sender, EventArgs e)
    {
        if (txtOnDutyTime.Text != "00:00:00" && txtOffDutyTime.Text != "00:00:00")
        {
            DateTime In_Time_Temp = Convert.ToDateTime(txtOnDutyTime.Text);
            DateTime Out_Time_Temp = Convert.ToDateTime(txtOffDutyTime.Text);
            if (In_Time_Temp > Out_Time_Temp)
            {
                DisplayMessage("On Duty Time must be greater than Off Duty Time");
                txtOnDutyTime.Text = "00:00:00";
                txtOffDutyTime.Text = "00:00:00";
                txtOnDutyTime.Focus();
            }
            else if (In_Time_Temp == Out_Time_Temp)
            {
                DisplayMessage("On Duty Time and Off Duty Time cannot be equal");
                txtOnDutyTime.Text = "00:00:00";
                txtOffDutyTime.Text = "00:00:00";
                txtOnDutyTime.Focus();
            }
        }
    }
}