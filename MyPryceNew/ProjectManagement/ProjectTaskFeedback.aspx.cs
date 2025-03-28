using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProjectManagement_ProjectTaskFeedback : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    Prj_ProjectTask objProjectTask = null;
    Prj_ProjectTeam objProjectTeam = null;
    Prj_ProjectTaskFeedback objProjectFeedback = null;
    SystemParameter ObjSysParam = null;
    Prj_Project_Task_Employeee objProjectTaskEmp = null;
    UserMaster objUser = null;
    Prj_ProjectMaster objProjectmaster = null;
    Prj_TaskContractReport objTaskcontract = null;
    Country_Currency objCC = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        txtTaskFeedbackDate.Attributes.Add("readonly", "readonly");

        cmn = new Common(Session["DBConnection"].ToString());
        objProjectTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objProjectTeam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        objProjectFeedback = new Prj_ProjectTaskFeedback(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objProjectTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objProjectmaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        objTaskcontract = new Prj_TaskContractReport(Session["DBConnection"].ToString());
        objCC = new Country_Currency(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            hdnUserId.Value = Session["UserId"].ToString();
            Session["TimeZoneId"] = objCC.getTimeZoneIdNameByCurrencyId(Session["LocCurrencyId"].ToString());

            hdnIsTaskStart.Value = "false";
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ProjectManagement/ProjectTaskFeedback.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Session["ProjectIdListByEmpId"] = objProjectmaster.getProjectIdListByPermission(Session["EmpId"].ToString());

            FillddlProjectname();
            txtTaskFeedbackDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");

            FillprojectTaskType();
            ddlOption.SelectedIndex = 2;
            pnllist.Visible = false;
            pnlstustrecord.Visible = false;
            DateTime now = DateTime.Now;
            Session["isGridUpdated"] = "true";
            // if redirecting from task page
            if (Request.QueryString["Task_Id"] != null)
            {

                DataTable dtProjecttask = new DataTable();
                getAllPendingData();
                dtProjecttask = objProjectTask.GetRecordTaskFeedbackbyEmpId(Session["EmpId"].ToString(), Request.QueryString["Task_Id"].ToString());
                if (dtProjecttask.Rows.Count > 0)
                {
                    gvrstatus.DataSource = dtProjecttask;
                    gvrstatus.DataBind();
                    pnllist.Visible = true;
                }
            }
            else
            {
                btngo_Click(null, null);
                I1.Attributes.Add("Class", "fa fa-plus");
                Div1.Attributes.Add("Class", "box box-info collapsed-box");
            }
        }
        if (hdnRPTClick.Value == "1")
        {
            GetReportStatement();
        }
    }
    public void FillprojectTaskType()
    {
        ddlTaskType.Items.Clear();
        DataTable dt = new Prj_Project_TaskType(Session["DBConnection"].ToString()).GetAllTrueRecord();
        ddlTaskType.DataSource = dt;
        ddlTaskType.DataTextField = "TaskType_Name";
        ddlTaskType.DataValueField = "Trans_Id";
        ddlTaskType.DataBind();
        ddlTaskType.Items.Insert(0, "--Select Task Type--");
        dt.Dispose();
    }
    public string Formatdate1(object Date)
    {
        string strDate = string.Empty;
        if (Date.ToString() != "" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01/01/1990")
        {
            strDate = Convert.ToDateTime(Date).ToString(Session["DateFormat"].ToString() + " HH:mm");
        }
        return strDate;
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnsave.Visible = clsPagePermission.bAdd;
        grvcomment.Columns[0].Visible = clsPagePermission.bEdit;
        grvcomment.Columns[1].Visible = clsPagePermission.bDelete;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        btnTaskHistory.Visible = true;
    }
    public string FormatTime(object input)
    {
        return Convert.ToDateTime(input).ToString("HH:mm");
    }
    public string Formatdate(object Date)
    {
        string strDate = string.Empty;
        if (Date.ToString() != "" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01/01/1900" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01-01-2000")
        {
            strDate = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        return strDate;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+ color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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
    public bool GetStatus(object status)
    {
        bool chkstatus = false;
        if (status.ToString() == "Assigned")
        {
            chkstatus = false;
        }
        else
        {
            chkstatus = true;
        }
        return chkstatus;
    }
    protected void txtprojecttype_TextChanged(object sender, EventArgs e)
    {
        FillddlProjectname();
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }
    [WebMethodAttribute(), ScriptMethodAttribute()]
    public static string[] GetProjectType(string prefixText, int count, string contextKey)
    {
        Prj_ProjectMaster objProjctMaster = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objProjctMaster.GetAllProjectMasteerByPrefix(prefixText);
        string[] str = new string[0];
        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = "" + dt.Rows[i][0].ToString();
                }
            }
            else
            {
                if (prefixText.Length > 2)
                {
                    str = null;
                }
                else
                {
                    dt = objProjctMaster.GetAllProjectMasteer();
                    dt = dt.DefaultView.ToTable(true, "Project_Type");
                    if (dt.Rows.Count > 0)
                    {
                        str = new string[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            str[i] = dt.Rows[i][0].ToString();
                        }
                    }
                }
            }
        }
        return str;
    }
    public void FillddlProjectname()
    {
        DataTable dtProjecrtteam = new DataTable();
        dtProjecrtteam = objProjectTeam.GetAllProjectNamePreText(Session["EmpId"].ToString(), txtprojecttype.Text.Trim());
        if (dtProjecrtteam.Rows.Count > 0)
        {
            if (dtProjecrtteam.Rows.Count > 0)
            {
                Session["DT_ddlProjectName_Followup"] = dtProjecrtteam;
            }
        }
        dtProjecrtteam.Dispose();
    }

    public void fillComment()
    {
        if (hdntaskid.Value != "")
        {
            DataTable dt = new DataTable();
            dt = objProjectFeedback.GetRecordByTaskId(hdntaskid.Value);
            if (dt.Rows.Count > 0)
            {
                dt = dt.DefaultView.ToTable(true, "CommentBy", "DateTime", "Task_Id", "Trans_Id", "Description", "field2", "field3","timeZoneID");
                objPageCmn.FillData((object)grvcomment, dt, "", "");
                Session["dtFilterComment"] = dt;
            }
            else
            {
                grvcomment.DataSource = null;
                grvcomment.DataBind();
            }
        }
    }
    protected void gvrstatus_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["isGridUpdated"].ToString() == "false")
        {
            btngo_Click(null, null);
            Session["isGridUpdated"] = "true";
        }
        DataTable dt = (DataTable)Session["dtprojectteamrecord"];
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
        Session["dtprojectteamrecord"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvrstatus, dt, "", "");
    }
    protected void gvrstatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (Session["isGridUpdated"].ToString() == "false")
        {
            btngo_Click(null, null);
            Session["isGridUpdated"] = "true";
        }
        gvrstatus.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtprojectteamrecord"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvrstatus, dt, "", "");
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        btngo_Click(null, null);
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }
  

    public string GetEmployeeName(object empid)
    {
        string empname = "No Name";
        try
        {
            DataTable dt = new EmployeeMaster(Session["DBConnection"].ToString()).GetEmployeeMasterAllData(Session["CompId"].ToString());
            dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                empname = dt.Rows[0]["Emp_Name"].ToString();
                if (empname == "")
                {
                    empname = "No Name";
                }
            }
            return empname;
        }
        catch
        {
            return "";
        }
    }
    private string GetEmployeeIdByUserId(string p)
    {
        string empid = "0";
        DataTable dt = objUser.GetUserMasterByUserId(p.ToString(), Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            empid = dt.Rows[0]["Emp_Id"].ToString();
        }
        return empid;
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        //here we are adding validation which are mendatory before closing of task
        ///start
        ///
        if (ddlTaskCompletion.SelectedIndex > 0)
        {
            if (lbldisassigndate.Text.Trim() == "" || lblEmpCloseDate.Text == "")
            {
                DisplayMessage("You can not update Task completion % , assign date and expected date not found");
                if (chkCloseTask.Checked)
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
        }
        if (txtTaskFeedbackDate.Text == "")
        {
            DisplayMessage("Enter Task Feedback Date");
            txtTaskFeedbackDate.Focus();
            if (chkCloseTask.Checked)
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
            return;
        }
        if (chkCloseTask.Checked)
        {
            DataTable dt = objProjectTask.GetAllWorkingEmployeeTasks();
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Task_id = '" + hdntaskid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                    DisplayMessage(dt.Rows[0]["emp_name"].ToString() + " is working on the task, Please ask him to stop the task");
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                    return;
                }
            }

            if (Editor1.Text == "")
            {
                DisplayMessage("Please Give Feedback");
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
            //DataTable dtTaskdetail = objProjectTask.GetRecordTaskVisibilityTrueWithoutBugs(Session["EmpId"].ToString());
            //dtTaskdetail = new DataView(dtTaskdetail, "ParentTaskId=" + hdntaskid.Value + " and Status in ('Assigned','ReAssigned') and Project_id=" + hdnProjectId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            string count = objProjectTask.CheckChildTaskClosedOrNot(hdntaskid.Value, hdnProjectId.Value, Session["EmpId"].ToString());
            if (count != "0")
            {
                DisplayMessage("You can not close parent task because some child task is still open");
                chkCloseTask.Checked = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
            //here we are checking that assign date and assignee is exist or not  
            if (lbldisassigndate.Text.Trim() == "" || lblEmpCloseDate.Text == "")
            {
                DisplayMessage("You can not close , assign date and expected date not found");
                chkCloseTask.Checked = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
            DataTable dtTaskEmployee = objProjectTaskEmp.GetRecordBy_RefType_and_RefId("Task", hdntaskid.Value);
            if (dtTaskEmployee.Rows.Count == 0)
            {
                DisplayMessage("You can not close , assignee not found");
                chkCloseTask.Checked = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
            //here we are checking  that close date and time shoul dbe greater or equal to assign date 
            if (txttlenddate.Text == "")
            {
                DisplayMessage("Enter task end date");
                chkCloseTask.Checked = false;
                txttlenddate.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
            else
            {
                try
                {
                    Convert.ToDateTime(txttlenddate.Text);
                }
                catch
                {
                    DisplayMessage("Task end date is invalid");
                    txttlenddate.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                    return;
                }
            }


            if (txttlendtime.Text == "" || txttlendtime.Text == "00:00")
            {
                DisplayMessage("Enter task end time");
                txttlendtime.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }
            DateTime dtassigndate = Convert.ToDateTime(lbldisassigndate.Text);
            DateTime dtTLclosedate = Convert.ToDateTime(txttlenddate.Text);
            DateTime dtTLclosetime = Convert.ToDateTime(txttlendtime.Text);
            dtTLclosedate = dtTLclosedate.AddHours(dtTLclosetime.Hour);
            dtTLclosedate = dtTLclosedate.AddMinutes(dtTLclosetime.Minute);
            if (dtassigndate >= dtTLclosedate)
            {
                DisplayMessage("Task end date should be greater or equal to assign date");
                txttlenddate.Focus();
                chkCloseTask.Checked = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                return;
            }

            txtTaskFeedbackDate.Text = txttlenddate.Text;
        }
        //end 
        string EmpId = string.Empty;
        try
        {
            EmpId = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString()).Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
        }
        string CommentTo = string.Empty;
        string AssignTo1 = string.Empty;
        DataTable dtTask1 = objProjectTask.GetRecordByTaskId(hdntaskid.Value);
        if (dtTask1.Rows.Count > 0)
        {
            AssignTo1 = dtTask1.Rows[0]["Emp_Id"].ToString();
        }
        if (AssignTo1 == EmpId)
        {
            CommentTo = GetEmployeeIdByUserId(dtTask1.Rows[0]["CreatedBy"].ToString());
        }
        else
        {
            CommentTo = EmpId;
        }
        int a = 0;
        if (Editor1.Text != "")
        {
            if (hdnEditId.Value == "")
            {
                a = objProjectFeedback.InsertProjectFeedback(Session["CompId"].ToString(), hdntaskid.Value.ToString(), Editor1.Text, txtTaskFeedbackDate.Text, EmpId, CommentTo, "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                DisplayMessage("Record Saved","green");
            }
            else
            {
                a = objProjectFeedback.UpdateProjcetFeedback(hdnEditId.Value, Session["CompId"].ToString(), hdntaskid.Value.ToString(), Editor1.Text, txtTaskFeedbackDate.Text, EmpId, Session["UserId"].ToString(), DateTime.Now.ToString());
                DisplayMessage("Record updated","green");
                hdnIsTaskStart.Value = "false";
            }
            //fillComment();
            objProjectFeedback.daClass.execute_Command("update prj_project_task set Field5='" + ddlTaskCompletion.SelectedValue + "' where Task_Id=" + hdntaskid.Value.ToString() + "");
            if (a != 0)
            {
                //
                //DataTable dtTeam = new DataTable();
                //dtTeam = objProjectTeam.GetRecordByProjectId("", hdnProjectId.Value.ToString());
                //dtTeam = new DataView(dtTeam, "Task_Visibility='True'", "", DataViewRowState.CurrentRows).ToTable();
                //DataTable dtEmp = new DataTable();
                //string AssignTo = string.Empty;
                //string AssignBy = string.Empty;
                //string strMsg = string.Empty;
                //string FeedBackBy = string.Empty;
                //string CompanyName = string.Empty;
                //DataTable DtCompany = new CompanyMaster().GetCompanyMasterById(Session["CompId"].ToString());
                //if (DtCompany.Rows.Count > 0)
                //{
                //    CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
                //}
                //string TCloseDate = string.Empty;
                //string TCloseTime = string.Empty;
                //string Duration = string.Empty;
                //string PExpEndDate = string.Empty;
                //DataTable dtTask = objProjectTask.GetRecordByTaskId(hdntaskid.Value);
                //AssignTo = dtTask.Rows[0]["Emp_Name"].ToString();
                //try
                //{
                //    FeedBackBy = GetEmployeeName(EmpId);
                //}
                //catch
                //{
                //}
                //DataTable dtUserTo = new DataTable();
                //string AssignEmpId = string.Empty;
                //try
                //{
                //    dtUserTo = objUser.GetUserMasterByUserId(dtTask.Rows[0]["CreatedBy"].ToString(), Session["CompId"].ToString());
                //    AssignEmpId = dtUserTo.Rows[0]["Emp_Id"].ToString();
                //}
                //catch
                //{
                //}
                //AssignBy = GetEmployeeName(AssignEmpId);
                //try
                //{
                //    TCloseDate = Convert.ToDateTime(dtTask.Rows[0]["Emp_Close_Date"].ToString()).ToString("dd-MMM-yyyy");
                //    TCloseTime = Convert.ToDateTime(dtTask.Rows[0]["Emp_Close_Time"].ToString()).ToString("HH:mm");
                //}
                //catch
                //{
                //}
                //try
                //{
                //    Duration = dtTask.Rows[0]["Field1"].ToString();
                //}
                //catch
                //{
                //}
                //string AssignTime = string.Empty;
                //AssignTime = dtTask.Rows[0]["Assign_Time"].ToString();
                //PExpEndDate = dtTask.Rows[0]["Exp_End_Date"].ToString();
                //strMsg = "<html><head>    <title>Task Detail</title>    <style>        body        {            font-family: Arial, Helvetica, sans-serif;            font-size: 12px;            color: #333333;        }        .main        {            width: 800px;            background-color: #F9F9F9;            margin-left: auto;            margin-right: auto;            padding: 30px;        }                                    label        {            display: inline-block;            width: 100px;            margin-right: 30px;            text-align: left;            font-size: 14px;        }        input        {        }        fieldset        {            border: none;            float: left;            margin: 0px auto;        }    </style></head><body>    <div class='main'>   <table  style='color: #C60000;font-weight:bold;font-size: 14px;margin: 0;'   width='700' align='center' cellspacing='0'>        <tr>        <td >        Company Name        </td>        <td>        :        </td>                <td>        " + CompanyName + "        </td>                        </tr>        <tr>        <td>                Date        </td>        <td>        :        </td>                <td>       " + DateTime.Now.ToString("dd-MMM-yyyy") + "                 </td>                </tr>        <tr>        <td colspan='3' align='center'>        Task Detail        </td>        </tr>        </table>                <table width='700' height='136' border='0' align='center' cellspacing='0'>            <tr>                <td width='120' height='36' >                    Task ID                </td>                <td align='center' width='120' height='36'>                    <b >:</b>                </td>                <td width='153'>                    " + hdntaskid.Value.ToString() + "                </td>                <td width='130'>                    Deadline                </td>                <td width='130' align='center'>                    <b>:</b>                </td>                <td>                    " + Duration + "                </td>            </tr>            <tr>                <td height='32'>                    Assign Date                </td>                <td height='32' align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(dtTask.Rows[0]["Assign_Date"].ToString()).ToString("dd-MMM-yyyy") + "                </td>                <td>                    Assign Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "                </td>            </tr>            <tr>                <td height='34'>                    Assign To                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td>                    " + AssignTo + "                </td>                <td>                    Assign By                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + AssignBy + "                </td>            </tr>            <tr>                <td height='34'>                    Task Close Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseDate + "                </td>                <td>                    Task Close Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseTime + "                </td>            </tr>            <tr>                <td height='34'>                    Project Start Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(dtTask.Rows[0]["Start_Date"].ToString()).ToString("dd-MMM-yyyy") + "                </td>                <td>                    Project Exp. End Date                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(PExpEndDate).ToString("dd-MMM-yyyy") + "                </td>            </tr>            <tr>                <td height='34'>                    Title                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td colspan='4'>                    " + dtTask.Rows[0]["Subject"].ToString() + "                </td>            </tr>            <tr>                            <td height='36'>                    Description                </td>                            <td align='center'  valign='middle'>                    <b>:</b>                </td>                <td colspan='4'>                    " + dtTask.Rows[0]["Description"].ToString() + "                </td>            </tr>  <tr>                            <td  height='36' valign='middle'>                    Feed Back                </td>                            <td align='center'  valign='middle'>                    <b>:</b>                </td>                <td colspan='4'>                    " + Editor1.Text + "                </td>            </tr>   <tr>                        <td  height='36' valign='middle'>                    Feed Back By               </td>                            <td align='center'  valign='middle'>                    <b>:</b>                </td>                <td colspan='4'>                    " + FeedBackBy + "                </td>            </tr>      </table>    </div></body></html>";
                //                strMsg = "<html>" +
                //"<head>" + "<title>Task Detail</title>" + "</head>" +
                //"<body>" +
                //"<table width='100%' cellpadding='2' cellspacing='0' style='letter-spacing:1px; text-indent:10px;  font:Verdana, Arial, Helvetica, sans-serif; font-size:12px;border:1px solid #ccc;'>" + "<tbody>" +
                //  "<tr  style='background:#1886b9; color:#fff; font-size: 16px;' >" +
                //        "<td colspan='2' style='text-indent:10px;  ' >" + CompanyName + "</td>" +
                //        "<td colspan='2' align='right' style='padding-right:5px; '  >Date :" +
                //            DateTime.Now.ToString("dd-MMM-yyyy") + "</td> </tr> <tr >" +
                //        "<td colspan='4' style='font-size:20px; color:#333333; font-weight:bold; background-color:#cccccc; text-align:center;'>Task Detail   </td> </tr>                            <tr>" +
                //        "<td   style='text-indent:10px; width:300px;  border-right:1px solid #ccc; '>" +
                //        "Task Id </td><td style='text-align:left;  width:200px; border-right:1px solid #ccc; '>" + hdntaskid.Value + "</td><td style='text-indent:10px; width:200px;  border-right:1px solid #ccc; '>" +
                //        "Deadline </td><td width='300'> " + Duration + " </td></tr><tr  style='background-color:#dce7ec' >" +
                //        "<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>Assign Date </td>" +
                //        "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
                //            Convert.ToDateTime(dtTask.Rows[0]["Assign_Date"].ToString()).ToString("dd-MMM-yyyy") + "</td><td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>" +
                //        "Assign Time          </td> <td>" + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "     </td>" +
                //"          </tr>         <tr   >         		<td   style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Assign To" +
                //        "</td>                <td style='text-align:left;  border-right:1px solid #ccc; '>" +
                //       AssignTo +
                //        "</td>       <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>              AssignBy" +
                //        "</td>     <td>" + AssignBy + "     </td>" +
                //  "</tr> <tr  style='background-color:#dce7ec' ><td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'> Task Close Date          </td>" +
                //        "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
                //        TCloseDate + "</td> <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>     Task Close Time       </td>   <td>" +
                //        TCloseTime + "            </td>      </tr>            <tr  >         		<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Project Start Date                </td>                <td style='text-align:left;   border-right:1px solid #ccc; '>" +
                //       Convert.ToDateTime(dtTask.Rows[0]["Start_Date"].ToString()).ToString("dd-MMM-yyyy") + "                </td>     <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>  Project Exp. End Date" + "</td>   <td>" + Convert.ToDateTime(PExpEndDate).ToString("dd-MMM-yyyy") + " </td></tr>            <tr  style='background-color:#dce7ec' >         		<td  height='20'  style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>              FeedBack By                </td>                <td style='text-align:left;  border-right:1px solid #ccc; ' colspan='3'>" + FeedBackBy + "     </td>" +
                //"    </tr>     <tr >	<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Title" +
                //        "</td>         <td style='text-align:left;  border-right:1px solid #ccc;' colspan='3'>" + dtTask.Rows[0]["Subject"].ToString() + "</td>" + "</tr>           <tr   style='background-color:#dce7ec' >         		<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                FeedBack </td> <td style='text-align:left;   border-right:1px solid #ccc; ' colspan='3'>" + Editor1.Text + "  </td> </tr>  <tr > <td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>               Description                </td>       <td style='text-align:left;  border-right:1px solid #ccc; ' colspan='3'>" + dtTask.Rows[0]["Description"].ToString() + "</td></tr>	</tbody></table></body></html>";
                //string EmailId = string.Empty;
                //DataTable dtEmpDetials = new DataTable();
                //bool b = false;
                //string strEmpId = dtTask.Rows[0]["Emp_Id"].ToString();
                //for (int i = 0; i < strEmpId.Split(',').Length; i++)
                //{
                //    try
                //    {
                //        dtEmpDetials = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), strEmpId.Split(',')[i].ToString());
                //    }
                //    catch
                //    {
                //    }
                //    if (dtEmpDetials.Rows.Count > 0)
                //    {
                //        EmailId = dtEmpDetials.Rows[0]["Email_Id"].ToString();
                //    }
                //    if (EmailId != "")
                //    {
                //        // b = ObjMail.SendMail(EmailId, dtTask.Rows[0]["Subject"].ToString(), strMsg, Session["CompId"].ToString(), "");
                //    }
                //}
                //for (int i = 0; i < dtTeam.Rows.Count; i++)
                //{
                //    dtEmp = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), dtTeam.Rows[i]["Emp_Id"].ToString());
                //    EmailId = string.Empty;
                //    if (dtEmp.Rows.Count > 0)
                //    {
                //        EmailId = dtEmp.Rows[0]["Email_Id"].ToString();
                //        if (EmailId != "")
                //        {
                //  //  b = ObjMail.SendMail(EmailId, dtTask.Rows[0]["Subject"].ToString(), strMsg, Session["CompId"].ToString(), "");
                //        }
                //    }
                //}
                //
            }
            Editor1.Text = "";
        }
        else if (!chkCloseTask.Checked)
        {
            DisplayMessage("Enter Comment");
            return;
        }
        string strTlEnddate = string.Empty;
        string strTlEndTime = string.Empty;
        //here we are checking that if recenttask have any child task then it should be close 
        //code start
        if (chkCloseTask.Checked)
        {
            DataTable dt = objProjectTask.GetDataProjectId(hdnProjectId.Value);
            dt = new DataView(dt, "ParentTaskId=" + hdntaskid.Value + " and Status='Assigned'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("You can not close parent task because some child task is still open");
                dt.Dispose();
                return;
            }
            //code end
            int i = 0;
            if (txttlenddate.Text == "")
            {
                strTlEnddate = DateTime.Now.ToString();
            }
            else
            {
                strTlEnddate = Convert.ToDateTime(txttlenddate.Text).ToString();
            }
            if (txttlendtime.Text == "" || txttlendtime.Text == "00:00")
            {
                strTlEndTime = DateTime.Now.ToString();
            }
            else
            {
                strTlEndTime = Convert.ToDateTime(txttlendtime.Text).ToString();
            }
            //here we are getting actual hours and actual cost 
            string strActualHour = "00:00";
            //string stractualCost = "0";
            try
            {
                strActualHour = objProjectTask.getActualHours(hdntaskid.Value);
            }
            catch
            {
            }
            try
            {
                //stractualCost = GetProjectCost(strActualHour);
            }
            catch
            {
            }
            i = objProjectTask.UpdateProjcetTaskStatus(hdntaskid.Value, ddlResult.SelectedValue.ToString(), strActualHour, txtFinalCost.Text, Session["UserId"].ToString(), DateTime.Now.ToString(), strTlEnddate, strTlEndTime, "100", txtRemark.Text, txtLoss.Text);
            //  objDa.execute_Command("update prj_project_task set Field5='100' where Task_Id=" + hdntaskid.Value.ToString() + "");
            ddlTaskCompletion.SelectedValue = "100";
            if (i != 0)
            {
                DisplayMessage("Task Closed", "green");
                btncencel_Click(null, null);
                fillGrid(1);
                return;
            }
            
        }
        hdnEditId.Value = "";
        btncencel_Click(null, null);
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        Editor1.Text = "";
        ddlResult.SelectedIndex = 0;
        txtLoss.Text = "";
        txtRemark.Text = "";
    }
    protected void btncencel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Task_Id"] != null)
        {
            Response.Redirect("../ProjectManagement/ProjectTaskFeedback.aspx");
        }
        Editor1.Text = "";
        hdnEditId.Value = "";
        pnlfield.Visible = true;
        txtTaskFeedbackDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        pnlstustrecord.Visible = false;
        pnllist.Visible = true;
        lblNA.Visible = false;
        hdnProjectId.Value = "";        
        //btngo_Click(null, null);
        //btnbindrpt_Click(null, null);
        getAllPendingData();
    }
    protected void btnfeedback_Click(object sender, EventArgs e)
    {
        pnlfield.Visible = false;
        pnllist.Visible = false;
        pnlstustrecord.Visible = true;
    }

    private void download(DataTable dt)
    {
        Byte[] bytes = (Byte[])dt.Rows[0]["File_Data"];
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = dt.Rows[0]["ContentType"].ToString();
        Response.AddHeader("content-disposition", "attachment;filename="
        + dt.Rows[0]["File_Name"].ToString());
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
        
        Label lblTotalHr = (Label)gvrstatus.Rows[index].FindControl("lblTotalHr");
        Label lblAssignDate = (Label)gvrstatus.Rows[index].FindControl("lblAssignDate");
        Label lblExpectedEndDate = (Label)gvrstatus.Rows[index].FindControl("lblExpectedEndDate");
        HiddenField gvHdnProjectID = (HiddenField)gvrstatus.Rows[index].FindControl("gvHdnProjectID");

        hdntaskid.Value = "";
        ddlResult.SelectedIndex = 0;
        txtLoss.Text = "";
        txttlenddate.Text = "";
        txttlendtime.Text = "";
        hdnEditId.Value = "";
           
        try
        {
            hdntaskid.Value = e.CommandArgument.ToString().Split('/')[0].ToString();
            
            
            if (e.CommandArgument.ToString().Split('/')[1].ToString() == Session["EmpId"].ToString())
            {
                chkCloseTask.Visible = true;
            }
            else
            {
                DataTable dt = objProjectmaster.getProjectManagerName(gvHdnProjectID.Value);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["project_manager"].ToString() == Session["EmpId"].ToString())
                    {
                        chkCloseTask.Visible = true;
                    }
                }
                else
                {
                    chkCloseTask.Visible = false;
                }
                dt = null;             
            }
        }
        catch
        {
            chkCloseTask.Visible = false;
        }
       
        pnlfield.Visible = false;
        pnllist.Visible = false;
        pnlstustrecord.Visible = true;

        DataTable dtFeedback = new DataTable();
        dtFeedback = objProjectTask.GetRecordByTaskId(hdntaskid.Value.ToString());
        txtTaskFeedbackDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        if (dtFeedback.Rows.Count > 0)
        {
            hdnProjectId.Value = dtFeedback.Rows[0]["Project_id"].ToString();
            lblTaskId.Text = dtFeedback.Rows[0]["Task_Id"].ToString();
            lblidisproname.Text = dtFeedback.Rows[0]["Project_Name"].ToString();
            lblRequiredTime.Text = lblTotalHr.Text.Split('/')[1];
            lblTaskDuration.Text = lblTotalHr.Text.Split('/')[0];
            lbldisassigndate.Text = lblAssignDate.Text;
            lblEmpCloseDate.Text = lblExpectedEndDate.Text;
            lbldissubject.Text = dtFeedback.Rows[0]["Subject"].ToString();
            lbldisdescription.Text = dtFeedback.Rows[0]["Description"].ToString().Replace("\n", "<br />");
            lblPrjStartDate.Text = Formatdate(dtFeedback.Rows[0]["Start_Date"].ToString());
            lblPrjExpEndate.Text = Formatdate(dtFeedback.Rows[0]["Exp_End_Date"].ToString());
            try
            {
                ddlTaskCompletion.SelectedValue = dtFeedback.Rows[0]["Field5"].ToString();
            }
            catch
            {
                ddlTaskCompletion.SelectedIndex = 0;
            }
            txttlenddate.Text = Formatdate(dtFeedback.Rows[0]["TL_Close_Date"].ToString());
            txttlendtime.Text = FormatTime(dtFeedback.Rows[0]["TL_Close_Time"].ToString());
            if (dtFeedback.Rows[0]["Loss"].ToString() != "")
            {
                txtLoss.Text = Common.GetAmountDecimal(dtFeedback.Rows[0]["Loss"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            }
            txtFinalCost.Text = objProjectTask.getProjectCostByProjectId(dtFeedback.Rows[0]["Project_id"].ToString(), dtFeedback.Rows[0]["Task_Id"].ToString());

            txtRemark.Text = dtFeedback.Rows[0]["Closed_Remarks"].ToString();
            lblAssignBy.Text = GetAssignBy(dtFeedback.Rows[0]["CreatedBy"].ToString());
            HidCustId.Value = dtFeedback.Rows[0]["CreatedBy"].ToString();
            //here we are gettin project manager name 
            lblProjectManagerValue.Text = objProjectTeam.GetProjectManagerNameByProjectId(dtFeedback.Rows[0]["Project_id"].ToString());

            if (dtFeedback.Rows[0]["Status"].ToString() == "Assigned")
            {
                Editor1.Visible = true;
                lblNA.Visible = false;
                txttlenddate.Text = "";
                txttlendtime.Text = "00:00";
                //chkCloseTask.Visible = true;
                chkCloseTask.Enabled = true;
                btnsave.Enabled = true;
                chkCloseTask.Checked = false;
                lblTaskCompletion.Visible = true;
                ddlTaskCompletion.Visible = true;
            }
            else
            {
                //chkCloseTask.Visible = true;
                chkCloseTask.Enabled = false;
                chkCloseTask.Checked = true;
                btnsave.Enabled = false;
                lblTaskCompletion.Visible = true;
                ddlTaskCompletion.Visible = true;
                try
                {
                    ddlResult.SelectedValue = dtFeedback.Rows[0]["Status"].ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
                }
                catch
                {
                    chkCloseTask.Checked = false;
                    ddlResult.SelectedIndex = 0;
                }

            }
            fillComment();
        }
    }
    public string GetAssignBy(object UserId)
    {
        string EmpName = string.Empty;
        DataTable dt = new DataTable();
        dt = objUser.GetUserMasterByUserId(UserId.ToString(), Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            EmpName = dt.Rows[0]["EmpName"].ToString();
        }
        return EmpName;
    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        btngo_Click(null, null);
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        fillGrid(1);
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }

    private void fillGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName.SelectedValue + " Like '%" + txtValue.Text.Trim() + "%'";
            }
        }

        if (txtEndFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtEndFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format dd-MMM-yyyy");
                txtEndFromDate.Focus();
                return;
            }
        }
        if (txtEndToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtEndToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
                txtEndToDate.Focus();
                return;
            }
        }
        if (txtEndFromDate.Text != "" && txtEndToDate.Text != "")
        {
            if (ObjSysParam.getDateForInput(txtEndFromDate.Text) > ObjSysParam.getDateForInput(txtEndToDate.Text))
            {
                DisplayMessage("From Date cannot be greater than To Date");
                txtEndFromDate.Focus();
                return;
            }
        }

        string strWhereClause = string.Empty;
        strWhereClause = "isActive='true'";

        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        if (txtEndFromDate.Text.Trim() != "" && txtEndToDate.Text.Trim() != "")
        {
            strWhereClause = strWhereClause + " and emp_close_date >='" + txtEndFromDate.Text + "' and emp_close_date <= '" + txtEndToDate.Text + "'";
        }
   
        string status = ddlPosted.SelectedValue;
        status = status.ToLower() == "all" ? "" : " and status ='" + status + "'";
        strWhereClause = strWhereClause + status;
        if (txtProjectName.Text != "")
        {
            strWhereClause = strWhereClause + " and project_Name like '%" + txtProjectName.Text + "%'";
        }

        if (ddlTaskType.SelectedIndex != 0)
        {
            strWhereClause = strWhereClause + " and tasktypeId ='" + ddlTaskType.SelectedIndex + "'";
        }

        DataTable dtProjecttask = objProjectFeedback.getTaskFeedbackList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), strWhereClause, Session["EmpId"].ToString());

        Session["dtprojectteamrecord"] = dtProjecttask;
        Session["dtFilter_Project_Task_Feed"] = dtProjecttask;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvrstatus, dtProjecttask, "", "");
        int totalRows = 0;

        if (dtProjecttask.Rows.Count > 0)
        {
            totalRows = Int32.Parse(dtProjecttask.Rows[0]["TotalCount"].ToString());
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + totalRows + "";
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
        else
        {
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
            PageControlCommon.PopulatePager(rptPager, totalRows, 0);
        }

        pnllist.Visible = true;

        dtProjecttask = new DataView(dtProjecttask, "workStatus='Stop'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtProjecttask.Rows.Count > 0)
        {
            hdnIsTaskStart.Value = "true";
            hdnEditId.Value = dtProjecttask.Rows[0]["FeedbackTrans_id"].ToString();
        }
        else
        {
            hdnIsTaskStart.Value = "false";
            hdnEditId.Value = "";
        }
        getAllPendingData();
        if (dtProjecttask != null)
            dtProjecttask.Dispose();
    }

    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnCurrentPageIndex.Value = pageIndex.ToString();
        fillGrid(pageIndex);
    }

    //protected void btngo_Click(object sender, EventArgs e)
    //{
    //    if (txtFromDate.Text != "")
    //    {
    //        try
    //        {
    //            Convert.ToDateTime(txtFromDate.Text);
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Correct From Date Format dd-MMM-yyyy");
    //            txtFromDate.Focus();
    //            return;
    //        }
    //    }
    //    if (txtToDate.Text != "")
    //    {
    //        try
    //        {
    //            Convert.ToDateTime(txtToDate.Text);
    //        }
    //        catch
    //        {
    //            DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
    //            txtToDate.Focus();
    //            return;
    //        }
    //    }
    //    if (txtFromDate.Text != "" && txtToDate.Text != "")
    //    {
    //        if (ObjSysParam.getDateForInput(txtFromDate.Text) > ObjSysParam.getDateForInput(txtToDate.Text))
    //        {
    //            DisplayMessage("From Date cannot be greater than To Date");
    //            txtFromDate.Focus();
    //            return;
    //        }
    //    }
    //    DataTable dtProjecttask = new DataTable();

    //    if (hdnProjectId.Value != "")
    //    {
    //        string status = ddlPosted.SelectedValue;
    //        status = status.ToLower() == "all" ? "" : status;
    //        dtProjecttask = objProjectTask.GetRecordTaskFeedbackbyEmpId(Session["EmpId"].ToString(), hdnProjectId.Value.ToString(), status);
    //    }
    //    else
    //    {
    //        dtProjecttask = objProjectTask.GetRecordTaskFeedbackbyEmpId(Session["EmpId"].ToString());
    //    }


    //    if (txtFromDate.Text != "" && txtToDate.Text != "")
    //    {
    //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
    //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
    //        try
    //        {
    //            dtProjecttask = new DataView(dtProjecttask, "Assign_Date>='" + txtFromDate.Text + "' and Assign_Date<='" + ToDate.ToString() + "'", "workstatus desc", DataViewRowState.CurrentRows).ToTable();
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    if (ddlTaskType.SelectedIndex != 0)
    //    {
    //        dtProjecttask = new DataView(dtProjecttask, "TaskTypeId='" + ddlTaskType.SelectedValue.Trim() + "'", "workstatus desc", DataViewRowState.CurrentRows).ToTable();
    //    }


    //    if (txtValue.Text != "")
    //    {
    //        string condition = string.Empty;
    //        if (ddlOption.SelectedIndex == 1)
    //        {
    //            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
    //        }
    //        else if (ddlOption.SelectedIndex == 2)
    //        {
    //            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
    //        }
    //        else
    //        {
    //            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
    //        }
    //        dtProjecttask = new DataView(dtProjecttask, condition, "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    Session["dtprojectteamrecord"] = dtProjecttask;
    //    Session["dtFilter_Project_Task_Feed"] = dtProjecttask;

    //    //Common Function add By Lokesh on 23-05-2015
    //    objPageCmn.FillData((object)gvrstatus, dtProjecttask, "", "");
    //    if (dtProjecttask != null)
    //    {
    //        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtProjecttask.Rows.Count + "";
    //    }
    //    else
    //        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
    //    pnllist.Visible = true;

    //    dtProjecttask = new DataView(dtProjecttask, "workStatus='Stop'", "", DataViewRowState.CurrentRows).ToTable();
    //    if (dtProjecttask.Rows.Count > 0)
    //    {
    //        //hdnStartedTask.Value = dtProjecttask.Rows[0]["Task_id"].ToString();
    //        hdnIsTaskStart.Value = "true";
    //        hdnEditId.Value = dtProjecttask.Rows[0]["FeedbackTrans_id"].ToString();
    //    }
    //    else
    //    {
    //        //hdnStartedTask.Value = "";
    //        hdnIsTaskStart.Value = "false";
    //        hdnEditId.Value = "";
    //    }
    //    getAllPendingData();
    //    if (dtProjecttask != null)
    //        dtProjecttask.Dispose();
    //}


    protected void btnEditcomment_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = objProjectFeedback.GetRecordByTransId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            Editor1.Text = dt.Rows[0]["Description"].ToString();
            hdnEditId.Value = e.CommandArgument.ToString();
            txtTaskFeedbackDate.Text = GetDate_New(dt.Rows[0]["DateTime"].ToString());
        }
        dt.Dispose();
    }

    protected void IbtnDeletecomment_Command(object sender, CommandEventArgs e)
    {
        objProjectFeedback.DeleteProjectFeedBack(e.CommandArgument.ToString(), false.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
        fillComment();
        Btn_Div_General_Info.Attributes.Add("Class", "fa fa-minus");
        Div_General_Info.Attributes.Add("Class", "box box-primary");
        DisplayMessage("Record deleted successfully", "green");
    }
    protected void grvcomment_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterComment"];
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
        Session["dtFilterComment"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)grvcomment, dt, "", "");
    }
    protected void grvcomment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvcomment.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterComment"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)grvcomment, dt, "", "");

        Btn_Div_General_Info.Attributes.Add("Class", "fa fa-minus");
        Div_General_Info.Attributes.Add("Class", "box box-info");
    }

    //--------------------------------------------------------------------------------------------------------------------------------------
    protected string GetDate_New(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        string ID = e.CommandArgument.ToString().Split('/')[0];
        string projectNo = e.CommandArgument.ToString().Split('/')[1];
        FUpload1.setID(ID, Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/Project", "ProjectManagement", "Project", projectNo, e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    protected void btnStart_Click(object sender, EventArgs e)
    {
        hdnEditId.Value = objProjectFeedback.daClass.get_SingleValue("insert into Prj_Project_Feedback (company_id,task_id,field2,field3,isactive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) values ('" + Session["CompId"].ToString() + "','" + hdntaskid.Value + "','" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "','','true','" + Session["UserId"].ToString() + "','" + System.DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "','" + System.DateTime.Now.ToString() + "'); select scope_identity()");
        //hdnStartedTask.Value = hdntaskid.Value;
        btncencel_Click(null, null);
    }

    public void getAllPendingData()
    {
        using (DataTable dt = objProjectTask.GetAllWorkingTask(Session["UserId"].ToString()))
        {
            if (dt.Rows.Count > 0)
            {
                lnkCurrentWorkingTask.Text = "Show Current Working Task";
                lnkCurrentWorkingTask.CommandArgument = dt.Rows[0]["project_id"].ToString();
                hdnProjectId.Value = dt.Rows[0]["project_id"].ToString();
                if (dt.Rows.Count > 0)
                {
                    hdnIsTaskStart.Value = "true";
                }
                else
                {
                    hdnIsTaskStart.Value = "false";
                }
            }
            else
            {
                lnkCurrentWorkingTask.Text = "";
                lnkCurrentWorkingTask.CssClass = "";
            }
        }
    }

    protected void lnkCurrentWorkingTask_Command(object sender, CommandEventArgs e)
    {
        getAllPendingData();
        if (lnkCurrentWorkingTask.Text == "")
        {
            return;
        }

        //string projectId = "";
        //if(e.CommandArgument.ToString()=="")
        //{
        //    projectId = hdnProjectId.Value;
        //}
        //else
        //{
        //    projectId = e.CommandArgument.ToString();
        //}
        lnkCurrentWorkingTask.CssClass = "btn btn-primary";
        ddlPosted.SelectedValue = "Assigned";
        DataTable dtProjecttask = new DataTable();
        dtProjecttask = objProjectTask.GetCurrentWorkingTask(Session["EmpId"].ToString(), hdnProjectId.Value, "Assigned");
        txtProjectName.Text = objProjectmaster.GetProjectNameById(hdnProjectId.Value);
        txtEndFromDate.Text = "";
        txtEndToDate.Text = "";
        Session["dtprojectteamrecord"] = dtProjecttask;
        Session["dtFilter_Project_Task_Feed"] = dtProjecttask;
        objPageCmn.FillData((object)gvrstatus, dtProjecttask, "", "");

        int totalRows = 1;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + totalRows + "";
        PageControlCommon.PopulatePager(rptPager, totalRows, 1);


        if (dtProjecttask != null)
        {
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtProjecttask.Rows.Count + "";
        }
        else
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        pnllist.Visible = true;

        //dtProjecttask = new DataView(dtProjecttask, "workStatus='Stop'", "", DataViewRowState.CurrentRows).ToTable();
        //if (dtProjecttask.Rows.Count > 0)
        //{
        //hdnStartedTask.Value = dtProjecttask.Rows[0]["Task_id"].ToString();
        hdnIsTaskStart.Value = "true";
        hdnEditId.Value = dtProjecttask.Rows[0]["FeedbackTrans_id"].ToString();
        //}
        //else
        //{
        //    hdnStartedTask.Value = "";
        //    hdnIsTaskStart.Value = "false";
        //    hdnEditId.Value = "";
        //}

        if (dtProjecttask != null)
            dtProjecttask.Dispose();
    }


    protected void btnSaveFeedback_Command(object sender, CommandEventArgs e)
    {
        if (Editor1.Text == "")
        {
            DisplayMessage("Please Enter Comments");
            return;
        }
        string EmpId = string.Empty;
        try
        {
            EmpId = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString()).Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
        }
        string CommentTo = string.Empty;
        string AssignTo1 = string.Empty;
        DataTable dtTask1 = objProjectTask.GetRecordByTaskId(hdntaskid.Value);
        if (dtTask1.Rows.Count > 0)
        {
            AssignTo1 = dtTask1.Rows[0]["Emp_Id"].ToString();
        }
        if (AssignTo1 == EmpId)
        {
            CommentTo = GetEmployeeIdByUserId(dtTask1.Rows[0]["CreatedBy"].ToString());
        }
        else
        {
            CommentTo = EmpId;
        }
        objProjectFeedback.InsertProjectFeedback(Session["CompId"].ToString(), hdntaskid.Value.ToString(), Editor1.Text, txtTaskFeedbackDate.Text, EmpId, CommentTo, "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Saved", "green");
        Editor1.Text = "";
        btncencel_Click(null, null);
    }



    [WebMethod(enableSession: true)]
    [ScriptMethod()]
    public static string getProjectId(string projectName)
    {
        return new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetProjectIdByName(projectName);
    }


    protected void txtProjectName_TextChanged(object sender, EventArgs e)
    {
        hdnProjectId.Value = objProjectmaster.GetProjectIdByName(txtProjectName.Text);
        if (hdnProjectId.Value == "")
        {
            DisplayMessage("Please Select from suggestion");
            txtProjectName.Text = "";
            txtProjectName.Focus();
            return;
        }
    }

    //protected void chkCloseTask_CheckedChanged(object sender, EventArgs e)
    //{
    //    div_start.Visible = false;
    //    div_stop.Visible = true;
    //    if (chkCloseTask.Checked)
    //    {
    //        if (hdnTaskStarted.Value == "0")
    //        {
    //            div_start.Visible = false;
    //            div_stop.Visible = false;
    //        }
    //    }
    //    else
    //    {
    //        if (hdnTaskStarted.Value == "1")
    //        {
    //            div_start.Visible = true;
    //            div_stop.Visible = false;
    //        }
    //        else
    //        {
    //            div_start.Visible = false;
    //            div_stop.Visible = false;
    //        }
    //    }
    //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ShowClosedExtraFields_onEdit()", true);
    //}

    [WebMethod(enableSession: true)]
    [ScriptMethod()]
    public static string txtProjectName_TextChanged(string projectName)
    {
        string projectId = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetProjectIdByName(projectName);
        if (projectId == "")
        {
            return "0";
        }
        else
        {
            return projectId;
        }
    }

    protected void btnPrint_Command(object sender, CommandEventArgs e)
    {
        hdnReportTaskId.Value = e.CommandArgument.ToString();
        hdnProjectId.Value = e.CommandName.ToString();
        GetReportStatement();
        hdnRPTClick.Value = "1";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OpenTaskContractReportPopup()", true);
    }
    public void GetReportStatement()
    {
        CompanyMaster objComp = new CompanyMaster(Session["DBConnection"].ToString());
        Set_AddressChild objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        try
        {
            if (hdnProjectId.Value == "")
            {
                return;
            }
            if (hdnReportTaskId.Value == "")
            {
                return;
            }
        }
        catch
        {
        }
        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter();
        adp.Fill(rptdata.sp_Prj_ProjectHistory_Report, int.Parse(hdnProjectId.Value.ToString()), 1);
        dtFilter = rptdata.sp_Prj_ProjectHistory_Report;
        dtFilter = new DataView(dtFilter, "Task_Id in (" + hdnReportTaskId.Value + ") ", "", DataViewRowState.CurrentRows).ToTable();
        DataTable dtDetail = new DataView(dtFilter, "ParentTaskId<>'0'", "", DataViewRowState.CurrentRows).ToTable();
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        DataTable DtCompany = objComp.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }
        string strreportCode = string.Empty;
        if (ddlTaskType.SelectedIndex > 0)
        {
            strreportCode = ddlTaskType.SelectedValue.Trim() == "Internal" ? "PTC01" : "PC01";
        }
        Session["DtprojectTask"] = dtFilter.DefaultView.ToTable(true, "Task_Id", "Subject", "Assign_date", "Address", "Status");
        objTaskcontract.DataSource = dtFilter;
        objTaskcontract.DataMember = "sp_Prj_ProjectHistory_Report";
        objTaskcontract.CreateDocument();
        ReportViewer1.OpenReport(objTaskcontract);
        dtFilter.Dispose();
    }

    [WebMethod(enableSession: true)]
    [ScriptMethod()]
    public static string[] btnStart_Click(string taskId, string rowno,string userId)
    {
        string[] feedbackId = new string[2];
        if(userId == HttpContext.Current.Session["UserId"].ToString())
        {
            using (DataTable dt = new Prj_ProjectTask(HttpContext.Current.Session["DBConnection"].ToString()).GetAllWorkingTask(HttpContext.Current.Session["UserId"].ToString()))
            {
                if (dt.Rows.Count > 0)
                {
                    feedbackId[0] = "Please stop the started task";
                    feedbackId[1] = "";
                    return feedbackId;
                }
                else
                {
                    feedbackId[0] = new Prj_ProjectTaskFeedback(HttpContext.Current.Session["DBConnection"].ToString()).daClass.get_SingleValue("insert into Prj_Project_Feedback (company_id,task_id,field1,field2,field3,field4,isactive,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) values ('" + HttpContext.Current.Session["CompId"].ToString() + "','" + taskId + "','" + HttpContext.Current.Session["EmpId"].ToString() + "','" + DateTime.Now.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm") + "','','"+ HttpContext.Current.Session["TimeZoneId"].ToString() + "','true','" + HttpContext.Current.Session["UserId"].ToString() + "','" + System.DateTime.Now.ToString() + "','" + HttpContext.Current.Session["UserId"].ToString() + "','" + System.DateTime.Now.ToString() + "'); select scope_identity()");
                    feedbackId[1] = rowno;
                    HttpContext.Current.Session["isGridUpdated"] = "false";
                    return feedbackId;
                }
            }
        }
        else
        {
            feedbackId[0] = "Login user and current user are different, please logout and try again later !!";
            feedbackId[1] = "";
            return feedbackId;
        }        
    }
    
    [WebMethod(enableSession: true)]
    [ScriptMethod()]
    public static string[] btnStop_Click(string row, string feedbackId, string taskId, string feedback,string percentage)
    {
        string[] data = new string[5];
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        Prj_ProjectTaskFeedback objProjectFeedback = new Prj_ProjectTaskFeedback(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            string feedbackdata = "";
            if (feedbackId == "")
            {
                feedbackdata = objProjectFeedback.daClass.get_SingleValue("select trans_id from prj_project_feedback where task_id='" + taskId + "' and field2<>'' and field3=''  and createdby='" + HttpContext.Current.Session["UserId"].ToString() + "'", ref trns);
            }
            else
            {
                feedbackdata = feedbackId;
            }
            objProjectFeedback.UpdateProjcetFeedback(feedbackdata, HttpContext.Current.Session["CompId"].ToString(), taskId, feedback, System.DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), DateTime.Now.ToUniversalTime().ToString("dd-MMM-yyyy HH:mm"), ref trns);
            objProjectFeedback.daClass.execute_Command("update Prj_Project_Task set Field5='" + percentage + "' where Task_Id='" + taskId + "'", ref trns);
            HttpContext.Current.Session["isGridUpdated"] = "false";
            
            data[0] = row;
            data[1] = percentage;
            data[2] = "true";
            data[4] = new Prj_ProjectTask(HttpContext.Current.Session["DBConnection"].ToString()).getActualHours(taskId, ref trns);

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            data[2] = "true";
        }
        catch(Exception er)
        {
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            data[3] = "Due to some problem, we cannot process your request, please try after some time";
            data[2] = "false";
        }
        data[0] = row;
        data[1] = percentage;
        
        return data;
    }
    [WebMethodAttribute(), ScriptMethodAttribute()]
    public static string btnTaskDetails_Click(string taskDate, string AssignToId)
    {
        Prj_ProjectTaskFeedback objProjectFeedback = new Prj_ProjectTaskFeedback(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtHistory = new DataTable();
        string projectList = HttpContext.Current.Session["ProjectIdListByEmpId"].ToString();
        if (AssignToId.Trim() == "")
        {
            dtHistory = objProjectFeedback.getTaskHistoryData(taskDate, projectList);
        }
        else
        {
            dtHistory = objProjectFeedback.getTaskHistoryData(taskDate, projectList, AssignToId.Split('/')[1]);
        }
        List<TaskFeedbackReport>  tfr = new List<TaskFeedbackReport> { };
        foreach(DataRow dr in dtHistory.Rows)
        {
            TaskFeedbackReport rowData = new TaskFeedbackReport();
            rowData.actualCost = dr["ActualCost"].ToString();
            rowData.empName = dr["emp_name"].ToString();
            //rowData.empCode = dr["emp_code"].ToString();
            rowData.startTime = getCountryTimeFormatStatic(dr["field2"].ToString(), dr["timeZoneID"].ToString());
            rowData.stopTime = getCountryTimeFormatStatic(dr["field3"].ToString(), dr["timeZoneID"].ToString());
            rowData.expectedCost = dr["expected_cost"].ToString();
            rowData.requiredTime = dr["requiredhours"].ToString();
            rowData.taskId = dr["task_id"].ToString();
            rowData.subject = dr["subject"].ToString();
            rowData.projectName = dr["project_name"].ToString();
            rowData.actualTime = dr["totalworkinghr"].ToString();
            tfr.Add(rowData);
        }
        return JsonConvert.SerializeObject(tfr);
    }
    [WebMethodAttribute(), ScriptMethodAttribute()]
    public static string btnShowFeedbacks_Command(string taskId, string assignTo, string taskDate)
    {
        Prj_ProjectTaskFeedback objProjectFeedback = new Prj_ProjectTaskFeedback(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtHistory = new DataTable();
        if (assignTo.Trim() == "")
        {
            dtHistory = objProjectFeedback.getTaskFeedbackHistory(taskDate, taskId);
        }
        else
        {
            dtHistory = objProjectFeedback.getTaskFeedbackHistory(taskDate, taskId, assignTo.Split('/')[1]);
        }
        for (int i = 0; i < dtHistory.Rows.Count; i++)
        {
            if (dtHistory.Rows[i]["field2"].ToString() != "")
            {
                dtHistory.Rows[i]["field2"] = getCountryTimeFormatStatic(dtHistory.Rows[i]["field2"].ToString(), dtHistory.Rows[i]["timeZoneID"].ToString());
            }
            if (dtHistory.Rows[i]["field3"].ToString() != "")
            {
                dtHistory.Rows[i]["field3"] = getCountryTimeFormatStatic(dtHistory.Rows[i]["field3"].ToString(), dtHistory.Rows[i]["timeZoneID"].ToString());
            }
        }
        return JsonConvert.SerializeObject(dtHistory);
    }

    //public string getCountryTimeFormat(string time)
    //{
    //    //store dbDateTime in the database
    //    //var timeZones = TimeZoneInfo.GetSystemTimeZones();

    //    //DateTime serverDateTime = DateTime.Now;
    //    //DateTime dbDateTime = serverDateTime.ToUniversalTime();
    //    if(time=="")
    //    {
    //        return "";
    //    }
    //    DateTime dbDateTime = Convert.ToDateTime(time);

    //    //get date time offset for UTC date stored in the database
    //    DateTimeOffset dbDateTimeOffset = new DateTimeOffset(dbDateTime, TimeSpan.Zero);

    //    //get user's time zone from profile stored in the database
    //    TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(Session["TimeZoneId"].ToString());
    //    DateTimeOffset userDateTimeOffset = TimeZoneInfo.ConvertTime(dbDateTimeOffset, userTimeZone);
    //    string datetime = userDateTimeOffset.ToString("dd-MMM-yyyy HH:mm");
    //    return datetime;
    //}
    public static string getCountryTimeFormatStatic(string time,string timeZoneID)
    {
        if (time == "")
        {
            return "";
        }
        timeZoneID = timeZoneID == "" ? "Arab Standard Time" : timeZoneID;

        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        DateTime dbDateTime = Convert.ToDateTime(time);
        DateTimeOffset dbDateTimeOffset = new DateTimeOffset(dbDateTime, TimeSpan.Zero);
        TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
        DateTimeOffset userDateTimeOffset = TimeZoneInfo.ConvertTime(dbDateTimeOffset, userTimeZone);
        string datetime = userDateTimeOffset.ToString("dd-MMM-yyyy HH:mm");
        return datetime;
    }

    // add GetCompletionListProject, saveBug function to parent pages.
    [WebMethod(), ScriptMethod()]
    public static string[] GetCompletionListProject(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["DT_ddlProjectName_Followup"] == null)
        {
            HttpContext.Current.Session["DT_ddlProjectName_Followup"] = new Prj_ProjectTeam(HttpContext.Current.Session["DBConnection"].ToString()).GetAllProjectNamePreText(HttpContext.Current.Session["EmpId"].ToString(), "");
        }

        DataTable dtCon = HttpContext.Current.Session["DT_ddlProjectName_Followup"] as DataTable;
        using (dtCon = new DataView(dtCon, "Project_Name like '%" + prefixText + "%'", "Project_Name asc", DataViewRowState.CurrentRows).ToTable())
        {

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Project_Name"].ToString();
                }
            }
            return filterlist;
        }
    }



    [WebMethodAttribute(), ScriptMethodAttribute()]
    public static string[] GetCompletionListAssignBy(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();
        EmployeeMaster em = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        dt = em.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), "0", prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Emp_name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
            }
        }
        return str;
    }
    [WebMethodAttribute(), ScriptMethodAttribute()]
    public static string btnEmployeeWorking_Click(string projectId)
    {
        DataTable dt = new DataTable();
        Prj_ProjectTask objProjectTask = new Prj_ProjectTask(HttpContext.Current.Session["DBConnection"].ToString());
        projectId = projectId == "" ? HttpContext.Current.Session["ProjectIdListByEmpId"].ToString() : projectId;
        using (dt = objProjectTask.GetAllWorkingEmployeeTasks(projectId))
        {
            if (dt.Rows.Count > 0)
            {
                List<TaskFeedbackReport> tfr = new List<TaskFeedbackReport> { };
                foreach (DataRow dr in dt.Rows)
                {
                    TaskFeedbackReport rowData = new TaskFeedbackReport();
                    rowData.actualCost = dr["ActualCost"].ToString();
                    rowData.empName = dr["emp_name"].ToString();
                    rowData.startTime = getCountryTimeFormatStatic(dr["field2"].ToString(), dr["timeZoneID"].ToString());
                    rowData.expectedCost = dr["expected_cost"].ToString();
                    rowData.requiredTime = dr["requiredhours"].ToString();
                    rowData.taskId = dr["task_id"].ToString();
                    rowData.subject = dr["subject"].ToString();
                    rowData.projectName = dr["project_name"].ToString();
                    rowData.actualTime = dr["timediff"].ToString();
                    tfr.Add(rowData);
                }
                return JsonConvert.SerializeObject(tfr);
            }
            else
            {
                return "";
            }
        }            
    }

    public class TaskFeedbackReport
    {
        public string empName { get; set; }
        public string startTime { get; set; }
        public string stopTime { get; set; }
        public string projectName { get; set; }
        public string projectId { get; set; }
        public string subject { get; set; }
        public string taskId { get; set; }
        public string expectedCost { get; set; }
        public string actualCost { get; set; }
        public string empCode { get; set; }
        public string requiredTime { get; set; }
        public string actualTime { get; set; }
        public string feedbackDescription { get; set; }
    }
}

