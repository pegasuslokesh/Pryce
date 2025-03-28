using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
public partial class ProjectManagement_ProjectTask : BasePage
{
    Document_Master ObjDocument = null;
    Common cmn = null;
    Prj_ProjectTeam objProjectTeam = null;
    Prj_ProjectTask objProjectTask = null;
    Prj_ProjectMaster objProjctMaster = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContactmaster = null;
    Prj_Project_Task_Employeee objTaskEmp = null;
    EmployeeMaster objEmpMaster = null;
    UserMaster objUser = null;
    CompanyMaster objComp = null;
    Set_AddressChild objAddChild = null;
    Set_AddressMaster objAddress = null;
    Prj_Project_TaskType ObjTaskType = null;
    Prj_TaskContractReport objTaskcontract = null;
    Common.clsPagePermission clsPagePermission;
    HR_EmployeeDetail objemp = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProjectTeam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        objProjectTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContactmaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objAddress = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjTaskType = new Prj_Project_TaskType(Session["DBConnection"].ToString());
        objTaskcontract = new Prj_TaskContractReport(Session["DBConnection"].ToString());
        objemp = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        btnsave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnsave, "").ToString());
        if (!IsPostBack)
        {
         
            clsPagePermission = cmn.getPagePermission("../ProjectManagement/ProjectTask.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            if (Request.QueryString["Task"] == null)
            {
                Session["DT_ddlProjectName"] = null;
                Session["DT_TreeView"] = null;
            }
            Session["DT_BugGrid"] = null;
            //used to fill the projects name
            FillddlProjectname();
            if (Request.QueryString["Project_Id"] != null)
            {
                hdnProjectId.Value = Request.QueryString["Project_Id"].ToString();
                txtSearchprojectName.Text = objProjctMaster.GetProjectNameById(Request.QueryString["Project_Id"].ToString());
                
                if (Request.QueryString["Task"] == null)
                {
                    using (DataTable dt = objProjectTask.GetTaskTreeData(Session["EmpId"].ToString(), Request.QueryString["Project_id"].ToString()))
                    {
                        TaskTreeList.DataSource = dt;
                        TaskTreeList.DataBind();
                        Session["DT_TreeView"] = dt;
                        fillGridNTreeData();
                    }
                }
            }

            imgBtnTreeNGrid.Visible = false;
            if (Request.QueryString["Task"] != null)
            {
                // used to hide the status filter
                //ddlStatus.Visible = false;
                //lblStatus.Visible = false;
                Label19.Text = Resources.Attendance.Severity;
                GvrProjecttask.Columns[15].HeaderText = "Severity";
                GvrProjecttask.Columns[4].HeaderText = "Bugs";
                ddlPriority.Items.Clear();
                ddlPriority.Items.Insert(0, "Trivial");
                ddlPriority.Items.Insert(1, "Minor");
                ddlPriority.Items.Insert(2, "Major");
                ddlPriority.Items.Insert(3, "Critical");
                TaskTreeList.Visible = false;
                SetBugsCaption();
                GvrProjecttask.Visible = true;
                gridbind();
            }
            else
            {
                imgBtnTreeNGrid.Visible = true;
                //ddlStatus.Visible = true;
                //lblStatus.Visible = true;
                TaskTreeList.Visible = false;
                gvTaskData.Visible = false;
                if (hdnTreeNGrid.Value == "2")
                {
                    TaskTreeList.Visible = true;
                }
                else
                {
                    gvTaskData.Visible = true;
                }
                GvrProjecttask.Visible = false;
            }
            FillTaskType();
        }

        //used to fill project list and search project list
        //using (DataTable dt_list = Session["DT_ddlProjectName"] as DataTable)
        //{
        //objPageCmn.FillData((object)ddlprojectname, dt_list, "Project_Name", "");
        //objPageCmn.FillData((object)ddlSearchprojectName, dt_list, "Project_Name", "");
        //}
        // used to fill the task tree and this will not run when it is called from task bug page
        try
        {
            if (Request.QueryString["Task"] == null)
            {
                // hdnDllProjectChange = 1  means Prj_ProjectTask.DT_TreeView has been filled
                if (hdnDllProjectChange.Value == "1" && hdnTreeNGrid.Value == "2")
                {
                    DataTable dt = Session["DT_TreeView"] as DataTable;
                    if (dt.Rows.Count > 0)
                    {
                        if (ddlStatus.SelectedValue != "All")
                        {
                            dt = new DataView(dt, "Status='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        TaskTreeList.DataSource = dt;
                        TaskTreeList.DataBind();
                        //treeList.DataSource = dt;
                        ////treeList.ExpandToLevel(1);
                        //treeList.DataBind();                        
                        dt = null;
                    }
                }
                else
                {
                    hdnDllProjectChange.Value = "1";
                }
                //used for task contract report 
                if (hdnRPTClick.Value == "1")
                {
                    GetReportStatement();
                }
            }
        }
        catch (Exception err)
        { }
    }
    #region aspxTreelistbindingandevent
    public void FillTaskList()
    {
        if (Request.QueryString["Task"] != null || hdnProjectId.Value != "")
        {
            ViewState["CanPrintNAssign_Task"] = "false";
            return;
        }
        DataTable dtprojectTask = new DataTable();
        dtprojectTask = objProjectTask.GetTaskTreeData(Session["EmpId"].ToString(), hdnProjectId.Value.ToString());
        if (ddlStatus.SelectedIndex != 0)
        {
            dtprojectTask = new DataView(dtprojectTask, "Status='" + ddlStatus.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        Session["DT_TreeView"] = dtprojectTask;
        if (hdnTreeNGrid.Value == "2")
        {
            TaskTreeList.DataSource = dtprojectTask;
            TaskTreeList.DataBind();
        }
        else
        {
            gvTaskData.DataSource = dtprojectTask;
            gvTaskData.DataBind();
        }
        dtprojectTask.Dispose();
    }

    protected void treeList_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
    {
        CommandEventArgs CmdEvntArgs;
        string[] key = e.Argument.Split('|');
        if (e.Argument.StartsWith("TreeListGetparenttask1"))
        {
            resetToAddChild();
            if (Request.QueryString["Task"] != null)
            {
                DisplayMessage("Bug can not be allowed as associated task");
                return;
            }
            DataTable dt = Session["DT_TreeView"] as DataTable;
            dt = new DataView(dt, "Task_Id='" + key[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //objProjectTask.GetRecordByTaskId(key[1].ToString());
            if (dt.Rows.Count > 0)
            {
                if (Request.QueryString["Task"] == null)
                {
                    if (dt.Rows[0]["Status"].ToString() == "Closed")
                    {
                        DisplayMessage("Closed task can not be allowed as associated or parent task");
                        return;
                    }
                }
                //ddlprojectname.SelectedIndex = ddlSearchprojectName.SelectedIndex;
                txtProjectName.Text = txtSearchprojectName.Text;
                //ddlprojectname_SelectedIndexChanged(null, null);

                // fillDetails is replacement of ddlprojectname_SelectedIndexChanged
                fillDetails();
                hdnTaskid.Value = "";
                try
                {
                    ddlAssigningTasktype.SelectedValue = dt.Rows[0]["Field6"].ToString();
                }
                catch
                {
                    ddlAssigningTasktype.SelectedIndex = 0;
                }
                hdnProjId.Value = dt.Rows[0]["project_Id"].ToString();
                ddlTaskType.SelectedValue = dt.Rows[0]["Task_Type"].ToString();
                ddlTaskType_SelectedIndexChanged(null, null);
                if (ddlParentTask.Items.FindByValue(dt.Rows[0]["Task_Id"].ToString()) != null)
                {
                    //ddlParentTask.Value = dt.Rows[0]["Task_Id"].ToString();
                    ddlParentTask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
                }
                else
                {
                    ddlParentTask.SelectedIndex = 0;
                }
            }
            dt.Dispose();
            hdnTaskid.Value = "";
            txtsubject.Focus();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        if (e.Argument.StartsWith("TreeListEditTask"))
        {
            CmdEvntArgs = new CommandEventArgs("", key[1].ToString());
            btnEdit_command(null, CmdEvntArgs);
        }
        else if (e.Argument.StartsWith("TreeListEditComments"))
        {
            string strCmd = string.Format("window.open('../ProjectManagement/Projecttaskfeedback.aspx?Task_Id=" + key[1].ToString() + "','window','width=1024, ');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        }
        else if (e.Argument.StartsWith("TreeListExtended"))
        {
            DataTable dt = objProjectTask.GetRecordByTaskId(key[1].ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Assign_Date"].ToString().Trim() == "1/1/1900 12:00:00 AM" || dt.Rows[0]["Emp_Close_Date"].ToString().Trim() == "1/1/1900 12:00:00 AM")
                {
                    DisplayMessage("Cant Extend Because Assign Date and Expected Date are not present");
                    return;
                }
                if (dt.Rows[0]["Status"].ToString() != "Assigned")
                {
                    DisplayMessage("Cant Extend, Because The Task Status Already Changed To " + dt.Rows[0]["Status"].ToString());
                    return;
                }
                hdnProjId.Value = dt.Rows[0]["Project_Id"].ToString();
                try
                {
                    ddlAssigningTasktype.SelectedValue = dt.Rows[0]["Field6"].ToString();
                }
                catch
                {
                    ddlAssigningTasktype.SelectedIndex = 0;
                }
                try
                {
                    txtProjectName.Text = dt.Rows[0]["Project_Name"].ToString();
                    hdnProjectId.Value = dt.Rows[0]["Project_Id"].ToString();
                    //ddlprojectname.Value = dt.Rows[0]["Project_Id"].ToString();
                }
                catch
                {
                    //ddlprojectname.Items.Clear();
                    //ListEditItem Li1 = new ListEditItem();
                    //Li1.Text = dt.Rows[0]["Project_Name"].ToString();
                    //Li1.Value = dt.Rows[0]["Project_Id"].ToString();
                    //ddlprojectname.Items.Insert(0, Li1);
                    //ddlprojectname.SelectedIndex = 0;
                }
                //ddlprojectname_SelectedIndexChanged(null, null);
                // fillDetails is replacement of ddlprojectname_SelectedIndexChanged
                fillDetails();

                DataTable dttaskemp = new DataTable();
                foreach (ListItem li in listtaskEmployee.Items)
                {
                    dttaskemp = objTaskEmp.GetRecordBy_RefType_and_RefId("Task", key[1].ToString());
                    dttaskemp = new DataView(dttaskemp, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dttaskemp.Rows.Count > 0)
                    {
                        li.Selected = true;
                    }
                }
                dttaskemp.Dispose();
                txtExtendedId.Text = key[1].ToString();
                txtsubject.Text = dt.Rows[0]["Subject"].ToString();
                Editor1.Text = dt.Rows[0]["Description"].ToString();
                txtassigndate.Text = Formatdate(dt.Rows[0]["Assign_Date"].ToString());
                //txtassigntime.Text = FormatTime(dt.Rows[0]["Assign_Time"].ToString());
                txtempenddate.Text = Formatdate(dt.Rows[0]["Emp_Close_Date"].ToString());
                //txtempendtime.Text = FormatTime(dt.Rows[0]["Emp_Close_Time"].ToString());
                txtRequiredHrs.Text = dt.Rows[0]["RequiredHours"].ToString();
                txtAssignBy.Text = dt.Rows[0]["assignbyname"].ToString() + "/" + dt.Rows[0]["assignby"].ToString();
                txtExtendedDate.Text = "";
                txtExtendedTime.Text = "";
                div_extended.Style.Add("display", "block");
                div_assign.Attributes.Add("Class", "col-md-4");
                div_end.Attributes.Add("Class", "col-md-4");
                txttlenddate.Text = Formatdate(dt.Rows[0]["TL_Close_Date"].ToString());
                txttlendtime.Text = FormatTime(dt.Rows[0]["TL_Close_Time"].ToString());
                txtsubject.Text = dt.Rows[0]["Subject"].ToString();
                Editor1.Text = dt.Rows[0]["Description"].ToString();
                HidCustId.Value = dt.Rows[0]["Emp_Id"].ToString();
                hdnfileidupdate.Value = dt.Rows[0]["File_Id"].ToString();
                if (ddlParentTask.Items.FindByValue(dt.Rows[0]["Task_Id"].ToString()) != null)
                {
                    //ddlParentTask.Value = dt.Rows[0]["Task_Id"].ToString();
                    ddlParentTask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
                }
                else
                {
                    ddlParentTask.SelectedIndex = 0;
                }
                if (dt.Rows[0]["Expected_Cost"].ToString() != "")
                {
                    txtExpectedCost.Text = Common.GetAmountDecimal(dt.Rows[0]["Expected_Cost"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                }
                txtDuration.Text = dt.Rows[0]["Field1"].ToString();
                Editor1.Enabled = true;
                Lbl_Tab_New.Text = "Extended";
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                dt.Dispose();
            }
        }
        else if (e.Argument.StartsWith("TreeListEditBugs"))
        {
            string strCmd = string.Format("window.open('../ProjectManagement/Projecttask.aspx?Task=" + key[1].ToString() + "&Project_Id=" + hdnProjectId.Value.ToString() + "','window','width=1024, ');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        }
        else if (e.Argument.StartsWith("TreeListFileUpload1"))
        {
            DataTable dt = Session["DT_TreeView"] as DataTable;
            dt = new DataView(dt, "Task_Id='" + key[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            CmdEvntArgs = new CommandEventArgs("", dt.Rows[0]["project_id"].ToString()+"/"+ dt.Rows[0]["field7"].ToString());
            btnFileUpload_Command(sender, CmdEvntArgs);
        }
    }
    
    public bool GetUserPermission()
    {
        bool result = false;
        if (Session["EmpId"].ToString() == "0")
        {
            result = true;
        }
        else
        {
            DataTable dtAllPageCode = new DataTable();
            if (Request.QueryString["Task"] == null)
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), Session["AccordianId"].ToString(), "262", HttpContext.Current.Session["CompId"].ToString());
            }
            else
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), Session["AccordianId"].ToString(), "354", HttpContext.Current.Session["CompId"].ToString());
            }
            dtAllPageCode = new DataView(dtAllPageCode, "Op_Id=2", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAllPageCode.Rows.Count != 0)
            {
                result = true;
            }
            dtAllPageCode.Dispose();
        }
        return result;
    }
    #endregion
    public void FillTaskType()
    {
        DataTable dt = ObjTaskType.GetAllTrueRecord();
        if (Request.QueryString["Task"] != null)
        {
            dt = new DataView(dt, "Is_Bug='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = new DataView(dt, "Is_Bug='False'", "", DataViewRowState.CurrentRows).ToTable();
        }
        ddlAssigningTasktype.DataSource = dt;
        ddlAssigningTasktype.DataTextField = "TaskType_Name";
        ddlAssigningTasktype.DataValueField = "Trans_Id";
        ddlAssigningTasktype.DataBind();
        dt = null;
    }
    public void SetBugsCaption()
    {
        if (Request.QueryString["Task"] != null)
        {
            try
            {
                GvrProjecttask.Columns[5].Visible = true;
                GvrProjecttask.Columns[4].HeaderText = "Bugs";
            }
            catch
            {
            }
        }
        lblTaskcaption.Text = "Bug";
        Label21.Text = "Bug Type";
        Label15.Text = "Bug Duration(In Hours)";
        Label20.Text = "Bug Completion (%)";
        Label18.Text = "Associated Task";
        Label9.Text = "Bug Description";
        lblHeader.Text = "Project Bugs";
        Label5.Text = "Project Bugs";
    }
    public string GetAssignBy(object UserId)
    {
        return objUser.GetNameByUserId(UserId.ToString(), Session["CompId"].ToString());
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnsave.Visible = clsPagePermission.bAdd;
        hdnCanView.Value= clsPagePermission.bView.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        canModifyDate.Value = clsPagePermission.bModifyDate.ToString();
        
        if (Request.QueryString["Task"] == null)
        {
            hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        }
        //Project_Id 
        if (Request.QueryString["Task"] != null)
        {
            return;
        }
        hdnCanPrint.Value = "false";
        hdnCanAssignTask.Value = "false";
        if (clsPagePermission.bPrint == true)
        {
            hdnCanPrint.Value = "true";
        }
        if (clsPagePermission.bAdd == true)
        {
            hdnCanAssignTask.Value = "true";
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "','"+ color + "','white');", true);
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
        dtres.Dispose();
        return ArebicMessage;
    }
    public void Reset()
    {
        txtassigndate.Enabled = true;
        txtempenddate.Enabled = true;
        txtRequiredHrs.Enabled = true;

        HidCustId.Value = "";
        hdnTaskid.Value = "";
        hdnProjId.Value = "";
        hdnfileidupdate.Value = "";
        hdnfileid.Value = "";
        chkCancel.Checked = false;
        div_extended.Style.Add("display", "none");
        div_assign.Attributes.Add("Class", "col-md-4");
        div_end.Attributes.Add("Class", "col-md-4");
        txtassigndate.Text = "";
        //txtassigntime.Text = "";
        txtempenddate.Text = "";
        txtRequiredHrs.Text = "";
        txtAssignBy.Text = "";
        txttlenddate.Text = "";
        txttlendtime.Text = "";
        txtsubject.Text = "";
        Editor1.Text = "";
        txtExpectedCost.Text = "";
        //listtaskEmployee.Items.Clear();
        //listtaskEmployee.DataSource = null;
        //listtaskEmployee.DataBind();
        txtExtendedId.Text = "";
        txtDuration.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlTaskType.SelectedIndex = 0;
        ddlTaskType_SelectedIndexChanged(null, null);
        txtSiteAddress.Text = "";
        ChkContactPerson.Items.Clear();
        trParentttaskdetail.Visible = false;
        tlEnddate.Visible = false;
        //if (ddlprojectname.SelectedIndex > 0)
        //{
        //    GetparentTask(ddlprojectname.Value.ToString());
        //}
        //else
        //{
        //    GetparentTask("0");
        //}
        //if (hdnProjectId.Value !="")
        //{
        //    GetparentTask(hdnProjectId.Value);
        //}
        //else
        //{
        //    GetparentTask("0");
        //}
        FillTaskList();

        foreach (ListItem li in listtaskEmployee.Items)
        {
            li.Selected = false;
        }
    }
    public string FormatTime(object input)
    {
        try
        {
            return Convert.ToDateTime(input).ToString("HH:mm");
        }
        catch
        {
            return "";
        }
    }
    public string Formatdate(object Date)
    {
        string strDate = string.Empty;
        if (Date.ToString() != "" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01/01/1900" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01-01-2000")
        {
            strDate = Convert.ToDateTime(Date).ToString(ObjSysParam.SetDateFormat());
        }
        return strDate;
    }
    public void gridbind()
    {
        if (Request.QueryString["Task"] == null)
        {
            return;
        }
        DataTable dtProjecttask = new DataTable();

        if (Request.QueryString["Task"] == "0")
        {
            dtProjecttask = objProjectTask.GetRecordTaskVisibilityTrueWithBugs(Session["EmpId"].ToString());
        }
        else
        {
            //dtProjecttask = new DataView(objProjectTask.GetRecordTaskVisibilityTrueWithBugs(EmpId), "ParentTaskId=" + Request.QueryString["Task"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            dtProjecttask = fill_BugGrid_ForProjectId(Request.QueryString["Project_Id"].ToString());
            //Reset();
            Reset_Fields();
            DataTable dt = objProjectTask.GetRecordByTaskId(Request.QueryString["Task"].ToString());
            if (dt.Rows.Count > 0)
            {

                txtProjectName.Text = dt.Rows[0]["Project_Name"].ToString();
                hdnProjectId.Value = dt.Rows[0]["Project_Id"].ToString();
                txtSearchprojectName.Text = dt.Rows[0]["Project_Name"].ToString();
                fillDetails();

                hdnTaskid.Value = "";
                try
                {
                    ddlAssigningTasktype.SelectedValue = dt.Rows[0]["Field6"].ToString();
                }
                catch
                {
                    ddlAssigningTasktype.SelectedIndex = 0;
                }
                hdnProjId.Value = dt.Rows[0]["project_Id"].ToString();
                ddlTaskType.SelectedValue = dt.Rows[0]["Task_Type"].ToString();
                ddlTaskType_SelectedIndexChanged(null, null);
                if (ddlParentTask.Items.FindByValue(dt.Rows[0]["Task_Id"].ToString()) != null)
                {
                    ddlParentTask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
                }
                else
                {
                    ddlParentTask.SelectedIndex = 0;
                }
            }
            txtsubject.Focus();
            hdnTaskid.Value = "";
        }
        ///here we are checking project filter
        if (hdnProjectId.Value != "")
        {
            dtProjecttask = new DataView(dtProjecttask, "Project_Id='" + hdnProjectId.Value.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        try
        {
            GvrProjecttask.Columns[8].Visible = true;
            GvrProjecttask.Columns[9].Visible = true;
            GvrProjecttask.Columns[10].Visible = false;
            GvrProjecttask.Columns[11].Visible = false;
        }
        catch
        {
        }
        try
        {
            if (lblHeader.Text != "Project Bugs")
            {
                Fill_treeList(hdnProjectId.Value);
            }

        }
        catch
        { }
        Session["dtFilter_ProjectTask"] = dtProjecttask;
        Session["dtProjecttask"] = dtProjecttask;
        objPageCmn.FillData((object)GvrProjecttask, dtProjecttask, "", "");
        lblTotalRecords.Text = "Total Records: " + dtProjecttask.Rows.Count.ToString();
        Session["DT_TreeView"] = dtProjecttask;
        dtProjecttask = null;
    }
    public bool Check24Format(TextBox txt, string ErrorMessage)
    {
        if (!System.Text.RegularExpressions.Regex.IsMatch(txt.Text.ToString(), "^((0?[1-9]|1[012])(:[0-5]\\d){0,2}(\\ [AP]M))$|^([01]\\d|2[0-3])(:[0-5]\\d){0,2}$"))
        {
            DisplayMessage("Enter valid time in 24 Hour Format");
            txt.Focus();
            return false;
        }
        else
        {
            return true; ;
        }
    }
    public void FillddlProjectname()
    {
        DataTable dtProjecrtteam = new DataTable();
        if (Request.QueryString["Task"] != null)
        {
            dtProjecrtteam = objProjectTeam.GetRecordByTaskVisibility("true", Session["EmpId"].ToString());
        }
        else
        {
            //earlier using GetRecordById sp
            dtProjecrtteam = objProjectTeam.GetProjectListbyEmpId(Session["EmpId"].ToString());
        }
        //objPageCmn.FillData((object)ddlprojectname, dtProjecrtteam, "Project_Name", "");
        Session["DT_ddlProjectName"] = dtProjecrtteam;
        dtProjecrtteam = null;
    }
    #region ParentTask
    public void GetparentTask(string strProjectId)
    {
        if (strProjectId == "--Select--")
        {
            strProjectId = "0";
        }
        DataTable dtProjecttask = new DataTable();


        if (hdnTaskid.Value == "")
        {
            if (Request.QueryString["Task"] == null)
            {
                dtProjecttask = objProjectTask.GetParentTaskVisibilityTrueWithoutBugs(Session["EmpId"].ToString(), strProjectId, "Assigned");
            }
        }
        else
        {
            if (Request.QueryString["Task"] == null)
            {
                dtProjecttask = objProjectTask.GetParentTaskVisibilityTrueWithoutBugs(Session["EmpId"].ToString(), strProjectId, "Assigned", hdnTaskid.Value);
            }
            else
            {
                dtProjecttask = objProjectTask.GetParentTaskVisibilityTrueWithoutBugs(Session["EmpId"].ToString(), strProjectId, hdnTaskid.Value);
            }
        }

        //dtProjecttask = objProjectTask.GetRecordTaskVisibilityTrueWithoutBugs(Session["EmpId"].ToString(), strProjectId);
        //if (hdnTaskid.Value == "")
        //{
        //    if (Request.QueryString["Task"] == null)
        //    {
        //        dtProjecttask = new DataView(dtProjecttask, "Status in ('Assigned','ReAssigned')", "Task_Id", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}
        //else
        //{
        //    if (Request.QueryString["Task"] == null)
        //    {
        //        dtProjecttask = new DataView(dtProjecttask, "Task_Id<>" + hdnTaskid.Value + " and Status in ('Assigned','ReAssigned')", "Task_Id", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    else
        //    {
        //        dtProjecttask = new DataView(dtProjecttask, "Task_Id<>" + hdnTaskid.Value + "", "Task_Id", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}
        objPageCmn.FillData((object)ddlParentTask, dtProjecttask, "Subject", "Task_Id");
        dtProjecttask.Dispose();
    }
    #endregion
    protected void GvrProjecttask_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvrProjecttask.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_ProjectTask"];
        objPageCmn.FillData((object)GvrProjecttask, dt, "", "");
        dt.Dispose();
    }
    protected void GvrProjecttask_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_ProjectTask"];
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
        Session["dtFilter_ProjectTask"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjecttask, dt, "", "");
        dt.Dispose();
        //AllPageCode();
    }
    public string GetRequiredHours1(string strExpDate, string strExpTime)
    {
        DateTime dtStartdate = new DateTime(Convert.ToDateTime(txtassigndate.Text.Trim()).Year, Convert.ToDateTime(txtassigndate.Text.Trim()).Month, Convert.ToDateTime(txtassigndate.Text.Trim()).Day);
        DateTime dtEnddate = new DateTime(Convert.ToDateTime(strExpDate).Year, Convert.ToDateTime(strExpDate).Month, Convert.ToDateTime(strExpDate).Day);
        DateTime dtAssingdate = dtStartdate;
        int Assinghour = 0;//Convert.ToDateTime(txtassigntime.Text).Hour;
        int AssingMinute = 0;//Convert.ToDateTime(txtassigntime.Text).Minute;
        int exphour = Convert.ToDateTime(strExpTime).Hour;
        int ExpMinute = Convert.ToDateTime(strExpTime).Minute;
        string ondutyTime = "10:00";
        string OffDytyTime = "19:00";
        foreach (ListItem li in listtaskEmployee.Items)
        {
            if (li.Selected)
            {
                //DataTable dtBind = objProjectTeam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
                DataTable dtBind = objProjectTeam.GetRecordByProjectId("", hdnProjectId.Value);
                dtBind = new DataView(dtBind, "Emp_id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtBind.Rows.Count > 0)
                {
                    ondutyTime = dtBind.Rows[0]["Field2"].ToString();
                    OffDytyTime = dtBind.Rows[0]["Field3"].ToString();
                }
                dtBind.Dispose();
                break;
            }
        }
        int totalMinute = 0;
        if (dtStartdate == dtEnddate)
        {
            totalMinute += GetMinuteDiff(exphour + ":" + ExpMinute, Assinghour + ":" + AssingMinute);
        }
        else
        {
            while (dtStartdate <= dtEnddate)
            {
                if (dtStartdate == dtAssingdate)
                {
                    totalMinute += GetMinuteDiff(OffDytyTime, Assinghour + ":" + AssingMinute);
                }
                else if (dtStartdate == dtEnddate)
                {
                    totalMinute += GetMinuteDiff(exphour + ":" + ExpMinute, ondutyTime);
                }
                else
                {
                    totalMinute += GetMinuteDiff(OffDytyTime, ondutyTime);
                }
                dtStartdate = dtStartdate.AddDays(1);
            }
        }
        return GetHours(totalMinute);
    }
    public double getMinuitebyHour(string strHour)
    {
        double totalMinute = 0;
        if (strHour == "")
        {
            strHour = "00:00";
        }
        totalMinute = Convert.ToDouble(strHour.Split(':')[0]) * 60;
        totalMinute += Convert.ToDouble(strHour.Split(':')[1]);
        return totalMinute;
    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {
        if (greatertime == "__:__:__" || greatertime == "")
        {
            return 0;
        }
        if (lesstime == "__:__:__" || lesstime == "")
        {
            return 0;
        }
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);
        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);
        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }
    public string GetHours(object obj)
    {
        if (obj.ToString() == "")
        {
            return "";
        }
        string retval = string.Empty;
        retval = ((Convert.ToInt32(obj) / 60) < 10) ? "0" + (Convert.ToInt32(obj) / 60).ToString() : ((Convert.ToInt32(obj) / 60)).ToString();
        retval += ":" + (((Convert.ToInt32(obj) % 60) < 10) ? "0" + (Convert.ToInt32(obj) % 60) : (Convert.ToInt32(obj) % 60).ToString());
        return retval;
    }
    protected void btnCheckTeamAvailable_Click(object sender, EventArgs e)
    {
        string strProjectId = string.Empty;
        //if (ddlprojectname.SelectedIndex > 0)
        //{
        //    strProjectId = ddlprojectname.Value.ToString();
        //}
        //else
        //{
        //    strProjectId = "0";
        //}
        if (hdnProjectId.Value != "")
        {
            strProjectId = hdnProjectId.Value;
        }
        else
        {
            strProjectId = "0";
        }
        string strCmd = string.Format("window.open('../ProjectManagement/ProjectDetail.aspx?Project_Id=" + strProjectId + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        btnsave.Enabled = false;
        string strTaskStatus = string.Empty;
        if (hdnTaskid.Value != "")
        {
            strTaskStatus = objProjectTask.GetRecordByTaskId(hdnTaskid.Value.ToString()).Rows[0]["Status"].ToString().Trim();
        }
        if (hdnProjectId.Value != "" && txtProjectName.Text == "")
        {
            DisplayMessage("Select Project Name");
            txtProjectName.Focus();
            btnsave.Enabled = true;
            return;
        }
        if (ddlAssigningTasktype.Items.Count == 0)
        {
            DisplayMessage("Select Task Type");
            ddlAssigningTasktype.Focus();
            btnsave.Enabled = true;
            return;
        }
        if (strTaskStatus != "Closed")
        {
            if (chkCancel.Checked)
            {
                strTaskStatus = "Cancelled";
            }
            else
            {
                strTaskStatus = "Assigned";
            }
        }
        if (txtAssignBy.Text == "")
        {
            DisplayMessage("Please Enter Assigned By Name");
            txtAssignBy.Focus();
            btnsave.Enabled = true;
            return;
        }
        string fileid = "";
        bool b = false;
        string strMailTo = "";
        string strSubject = "";
        string strMsg = "";
        string EmailId = string.Empty;
        string CompanyName = "";
        DataTable DtCompany = objComp.GetCompanyMasterById(Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
        }
        DtCompany.Dispose();
        string strContactperson = string.Empty;
        string strSiteAddress = string.Empty;
        strContactperson = "0";
        strSiteAddress = "0";
        if (ddlTaskType.SelectedIndex == 1)
        {
            DataTable dtAddId = objAddress.GetAddressDataByAddressName(txtSiteAddress.Text);
            if (dtAddId.Rows.Count > 0)
            {
                strSiteAddress = dtAddId.Rows[0]["Trans_Id"].ToString();
            }
            dtAddId.Dispose();

            foreach (ListItem li in ChkContactPerson.Items)
            {
                if (li.Selected)
                {
                    if (strContactperson.Trim() == "0")
                    {
                        strContactperson = li.Value;
                    }
                    else
                    {
                        strContactperson = strContactperson + "," + li.Value;
                    }
                }
            }
        }
        int a = 0;
        string AssignBy = "";
        string AssignTo = "";
        string AssignTime = "";
        string EmpEndTime = "";
        string EmpEndDate = "";
        string AssignDate = "";
        string TCloseDate = "";
        string TCloseTime = "";
        string PSDate = "";
        string PExpEndDate = "";
        bool IsEmployeeselcted = false;
        string strExpectedCost = "0";
        string strparenttaskId = string.Empty;
        strparenttaskId = "0";
        string assignBy = "0";

        if (Session["UserId"].ToString().ToUpper() != "SUPERADMIN")
        {
            assignBy = objemp.GetEmployeeId(txtAssignBy.Text.Split('/')[1].ToString());
        }
        if (ddlParentTask.SelectedIndex > 0)
        {
            //strparenttaskId = ddlParentTask.Value.ToString();
            strparenttaskId = ddlParentTask.SelectedValue.ToString();
        }
        try
        {
            foreach (ListItem li in listtaskEmployee.Items)
            {
                if (li.Selected)
                {
                    IsEmployeeselcted = true;
                    break;
                }
            }
            AssignTime = "1/1/1900";
            EmpEndTime = "1/1/1900";
            EmpEndDate = "1/1/1900";
            AssignDate = "1/1/1900";
            //if (IsEmployeeselcted)
            //{
            if (txtassigndate.Text.Trim() != "")
            {
                AssignDate = txtassigndate.Text.Trim();
            }
            //if (txtassigntime.Text != "")
            //{
            //    AssignTime = txtassigntime.Text;
            //    if (!Check24Format(txtassigntime, "Invalid Time"))
            //    {
            //        btnsave.Enabled = true;
            //        return;
            //    }
            //}
            if (txtempenddate.Text != "")
            {
                EmpEndDate = txtempenddate.Text;
            }
            //if (txtempendtime.Text != "")
            //{
            //    EmpEndTime = txtempendtime.Text;
            //}
            if (txtassigndate.Text.Trim() != "")
            {
                if (Convert.ToDateTime(txtassigndate.Text.Trim()) < Convert.ToDateTime(lblProjectStartdateValue.Text))
                {
                    DisplayMessage("Assign date should be greater or equal to project start date");
                    btnsave.Enabled = true;
                    return;
                }
                if (Convert.ToDateTime(txtassigndate.Text.Trim()) > Convert.ToDateTime(lblProjectExpenddateValue.Text))
                {
                    DisplayMessage("Assign date should be less then or equal to project expected end date");
                    btnsave.Enabled = true;
                    return;
                }
                //if (txtassigntime.Text == "" || txtassigntime.Text == "00:00")
                //{
                //    DisplayMessage("Enter Assign Time");
                //    txtassigntime.Focus();
                //    btnsave.Enabled = true;
                //    return;
                //}
            }
            if (txtempenddate.Text != "")
            {
                if (Convert.ToDateTime(txtempenddate.Text) > Convert.ToDateTime(lblProjectExpenddateValue.Text))
                {
                    DisplayMessage(lblTaskcaption.Text + " Expected end date should be less then or equal to project expected end date");
                    btnsave.Enabled = true;
                    return;
                }
                //if (txtempendtime.Text == "" || txtempendtime.Text == "00:00")
                //{
                //    DisplayMessage("Enter expected end Time");
                //    txtempendtime.Focus();
                //    btnsave.Enabled = true;
                //    return;
                //}
            }
            if (txtassigndate.Text.Trim() != "" && txtempenddate.Text != "")
            {
                if (Convert.ToDateTime(txtassigndate.Text.Trim()) > Convert.ToDateTime(txtempenddate.Text))
                {
                    DisplayMessage("Assign Date should be less then end date");
                    btnsave.Enabled = true;
                    return;
                }
            }
            //if (txtassigntime.Text != "" && txtempendtime.Text != "")
            //{
            //    if (txtassigndate.Text.Trim() == txtempenddate.Text.Trim() && txtassigndate.Text.Trim() != "" && txtempendtime.Text.Trim() != "")
            //    {
            //        if (Convert.ToDateTime(txtassigntime.Text) > Convert.ToDateTime(txtempendtime.Text))
            //        {
            //            DisplayMessage("Enter valid task end time");
            //            btnsave.Enabled = true;
            //            return;
            //        }
            //        else
            //        {
            //            if (Convert.ToDateTime(txtassigntime.Text) == Convert.ToDateTime(txtempendtime.Text))
            //            {
            //                DisplayMessage("Assign time and expected end time cannot be equal");
            //                btnsave.Enabled = true;
            //                return;
            //            }
            //            else
            //            {
            //                txtDuration.Text = GetRequiredHours(txtempenddate.Text, txtempendtime.Text);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        txtDuration.Text = GetRequiredHours(txtempenddate.Text, txtempendtime.Text);
            //    }
            //    //}
            //}
            
            if (txtExtendedId.Text != "")
            {               
                if (txtExtendedDate.Text == "" && strTaskStatus == "Extended")
                {
                    DisplayMessage("Enter Extended Date");
                    txtExtendedDate.Focus();
                    btnsave.Enabled = true;
                    return;
                }
                if (txtExtendedTime.Text == "" && txtExtendedTime.Text == "__:__" && strTaskStatus == "Extended")
                {
                    DisplayMessage("Enter Extended Time");
                    txtExtendedTime.Focus();
                    btnsave.Enabled = true;
                    return;
                }
            }
        }
        catch
        {
            AssignTime = "1/1/1900";
            EmpEndTime = "1/1/1900";
            EmpEndDate = "1/1/1900";
            AssignDate = "1/1/1900";
        }
        if (txtsubject.Text.Trim() == "")
        {
            DisplayMessage("Enter " + lblTaskcaption.Text);
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtsubject);
            btnsave.Enabled = true;
            return;
        }
        DataTable dtProject = new DataTable();
        dtProject = objProjctMaster.GetStartNEndDateProjectMasteer(hdnProjectId.Value);
        if (dtProject.Rows.Count > 0)
        {
            try
            {
                PSDate = Convert.ToDateTime(dtProject.Rows[0]["Start_Date"].ToString()).ToString("dd-MMM-yyyy");
                PExpEndDate = Convert.ToDateTime(dtProject.Rows[0]["Exp_End_Date"].ToString()).ToString("dd-MMM-yyyy");
            }
            catch
            {
            }
        }
        dtProject.Dispose();
        if (fileid == "" || fileid == "--Select--")
        {
            fileid = "0";
        }
        if (hdnProjectId.Value == "")
        {
            hdnProjId.Value = "0";
        }
        bool IsEmployeeForTask = false;
        foreach (ListItem li in listtaskEmployee.Items)
        {
            if (li.Selected)
            {
                if (HidCustId.Value == "")
                {
                    HidCustId.Value = li.Value;
                }
                else
                {
                    HidCustId.Value = HidCustId.Value + "," + li.Value;
                }
                IsEmployeeForTask = true;
            }
        }
        try
        {
            strExpectedCost = "0";
        }
        catch
        {
        }
        //get customer information
        //code start
        string[] strCustInfo = getCustomerInformation(objProjctMaster.GetCustomerNameByID(hdnProjectId.Value));
        //code end
        DataTable dtTask = new DataTable();
        if (hdnTaskid.Value != "")
        {
            InsertTaskEmployee("Task", hdnTaskid.Value);
            if (!IsEmployeeselcted)
            {
                objProjectTask.UpdateProjcetTask(hdnTaskid.Value.ToString(), hdnProjId.Value.ToString(), "0", AssignDate, AssignTime, EmpEndDate, EmpEndTime, "1/1/1900", "1/1/1900", txtsubject.Text.Trim(), Editor1.Text.Trim(), hdnfileidupdate.Value.ToString(), strTaskStatus, txtDuration.Text.Trim(), strparenttaskId, ddlPriority.SelectedValue, ddlTaskCompletion.SelectedValue, Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTaskType.SelectedValue, strSiteAddress, strContactperson, ddlAssigningTasktype.SelectedValue, strExpectedCost, txtExpectedCost.Text, txtRequiredHrs.Text, assignBy);
                DisplayMessage("Record Updated","green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                a = objProjectTask.UpdateProjcetTask(hdnTaskid.Value.ToString(), hdnProjId.Value.ToString(), "0", AssignDate, AssignTime, EmpEndDate, EmpEndTime, "1/1/1900", "1/1/1900", txtsubject.Text.Trim(), Editor1.Text.Trim(), hdnfileidupdate.Value.ToString(), strTaskStatus, txtDuration.Text.Trim(), strparenttaskId, ddlPriority.SelectedValue, ddlTaskCompletion.SelectedValue, Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTaskType.SelectedValue, strSiteAddress, strContactperson, ddlAssigningTasktype.SelectedValue, strExpectedCost, txtExpectedCost.Text, txtRequiredHrs.Text, assignBy);
                if (a != 0)
                {
                    DataTable dtTeam = new DataTable();
                    dtTeam = objProjectTeam.GetRecordByProjectId("", hdnProjId.Value.ToString());
                    dtTeam = new DataView(dtTeam, "Task_Visibility='True'", "", DataViewRowState.CurrentRows).ToTable();
                    DataTable dtEmp = new DataTable();
                    DataTable dtEmp1 = new DataTable();
                    foreach (ListItem li in listtaskEmployee.Items)
                    {
                        HidCustId.Value = li.Value;
                        if (HidCustId.Value != "")
                        {
                            dtEmp1 = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), HidCustId.Value);
                            if (dtEmp1.Rows.Count > 0)
                            {
                                EmailId = dtEmp1.Rows[0]["Email_Id"].ToString();
                                b = false;
                                if (EmailId != "")
                                {
                                    AssignTo = GetEmployeeName(HidCustId.Value);
                                    if (dtTask.Rows.Count > 0)
                                    {
                                        AssignBy = GetEmployeeNameByUserId(dtTask.Rows[0]["CreatedBy"].ToString());
                                    }
                                    try
                                    {
                                        TCloseDate = Convert.ToDateTime(txtempenddate.Text).ToString("dd-MMM-yyyy");
                                        //TCloseTime = Convert.ToDateTime(txtempendtime.Text).ToString("HH:mm");
                                    }
                                    catch
                                    {
                                    }
                                    // strMsg = "<html><head>    <title>Task Detail</title>    <style>        body        {            font-family: Arial, Helvetica, sans-serif;            font-size: 12px;            color: #333333;        }        .main        {            width: 800px;            background-color: #F9F9F9;            margin-left: auto;            margin-right: auto;            padding: 30px;        }                                    label        {            display: inline-block;            width: 100px;            margin-right: 30px;            text-align: left;            font-size: 14px;        }        input        {        }        fieldset        {            border: none;            float: left;            margin: 0px auto;        }    </style></head><body>    <div class='main'>   <table  style='color: #C60000;font-weight:bold;font-size: 14px;margin: 0;'   width='700' align='center' cellspacing='0'>        <tr>        <td >        Company Name        </td>        <td>        :        </td>                <td>        " + CompanyName + "        </td>                        </tr>        <tr>        <td>                Date        </td>        <td>        :        </td>                <td>       " + DateTime.Now.ToString("dd-MMM-yyyy") + "                 </td>                </tr>        <tr>        <td colspan='3' align='center'>        Task Detail        </td>        </tr>        </table>                <table width='700' height='136' border='0' align='center' cellspacing='0'>            <tr>                <td width='120' height='36' >                    Task ID                </td>                <td align='center' width='120' height='36'>                    <b >:</b>                </td>                <td width='153'>                    " + hdnTaskid.Value.ToString() + "                </td>                <td width='130'>                    Deadline                </td>                <td width='130' align='center'>                    <b>:</b>                </td>                <td width='173'>                    " + txtDuration.Text + "                </td>            </tr>            <tr>                <td height='32'>                    Assign Date                </td>                <td height='32' align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "                </td>                <td>                    Assign Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "                </td>            </tr>            <tr>                <td height='34'>                    Assign To                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td>                    " + AssignTo + "                </td>                <td>                    Assign By                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + AssignBy + "                </td>            </tr>            <tr>                <td height='34'>                    Task Close Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseDate + "                </td>                <td>                    Task Close Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseTime + "                </td>            </tr>            <tr>                <td height='34'>                    Project Start Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + PSDate + "                </td>                <td>                    Project Exp. End Date                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + PExpEndDate + "                </td>            </tr>            <tr>                <td height='34'>                    Title                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td colspan='4'>                    " + txtsubject.Text + "                </td>            </tr>            <tr>                <td valign='top'>                    Description                </td>                <td align='center'  valign='top'>                    <b>:</b>                </td>                <td colspan='4'>                    " + Editor1.Content + "                </td>            </tr>        </table>    </div></body></html>";
                                    strMsg = "<html>" +
        "<head>" + "<title>Task Detail</title>" + "</head>" +
        "<body>" +
        "<table width='100%' cellpadding='2' cellspacing='0' style='letter-spacing:1px;font-size:12px; text-indent:10px;  font:Verdana, Arial, Helvetica, sans-serif; border:1px solid #ccc;'>" + "<tbody>" +
                  "<tr  style='background:#1886b9; color:#fff; font-size: 16px;' >" +
                        "<td colspan='2' style='text-indent:10px;  ' >" + CompanyName + "</td>" +
                        "<td colspan='2' align='right' style='padding-right:5px; '  >Date :" +
                            DateTime.Now.ToString("dd-MMM-yyyy") + "</td> </tr> <tr >" +
                        "<td colspan='4' style='font-size:20px; color:#333333; font-weight:bold; background-color:#cccccc; text-align:center;'>Task Detail   </td> </tr>                            <tr>" +
                        "<td   style='text-indent:10px; width:300px;  border-right:1px solid #ccc; '>" +
                        "Task Id </td><td style='text-align:left;  width:200px; border-right:1px solid #ccc; '>" + hdnTaskid.Value + "</td><td style='text-indent:10px; width:200px;  border-right:1px solid #ccc; '>" +
                        "Deadline </td><td width='300'> " + txtDuration.Text.Trim() + " </td></tr><tr  style='background-color:#dce7ec' >" +
                        "<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>Assign Date </td>" +
                        "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
                            Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "</td><td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>" +
                        "Assign Time          </td> <td>" + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "     </td>" +
        "          </tr>         <tr   >         		<td   style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Assign To" +
                        "</td>                <td style='text-align:left;  border-right:1px solid #ccc; '>" +
                       AssignTo +
                        "</td>       <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>              AssignBy" +
                        "</td>     <td>" + AssignBy + "     </td>" +
                  "</tr> <tr  style='background-color:#dce7ec' ><td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'> Task Close Date          </td>" +
                        "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
                        TCloseDate + "</td> <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>     Task Close Time       </td>   <td>" +
                        TCloseTime + "            </td>      </tr>            <tr  >         		<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Project Start Date                </td>                <td style='text-align:left;   border-right:1px solid #ccc; '>" +
                       PSDate + "                </td>     <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>  Project Exp. End Date" + "</td>   <td>" + PExpEndDate + " </td></tr> " +
                             "<tr style='background-color:#dce7ec'>	<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Title" +
                        "</td>         <td style='text-align:left;  border-right:1px solid #ccc;' colspan='3'>" + txtsubject.Text.Trim() + "</td>" + "</tr>          <tr > <td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>               Description                </td>       <td style='text-align:left;  border-right:1px solid #ccc; ' colspan='3'>" + Editor1.Text.Trim() + "</td></tr>	</tbody></table></body></html>";
                                    if (b)
                                    {
                                        objProjectTask.UpdateProjcetTaskMailStatus(hdnTaskid.Value.ToString(), "Send");
                                    }
                                    else
                                    {
                                        objProjectTask.UpdateProjcetTaskMailStatus(hdnTaskid.Value.ToString(), "UnSend");
                                    }
                                }
                                else
                                {
                                    objProjectTask.UpdateProjcetTaskMailStatus(hdnTaskid.Value.ToString(), "UnSend");
                                }
                            }
                        }
                    }
                    for (int i = 0; i < dtTeam.Rows.Count; i++)
                    {
                        dtEmp = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), dtTeam.Rows[i]["Emp_Id"].ToString());
                        EmailId = string.Empty;
                        if (dtEmp.Rows.Count > 0)
                        {
                            EmailId = dtEmp.Rows[0]["Email_Id"].ToString();
                            b = false;
                            if (EmailId != "")
                            {
                                if (dtTask.Rows.Count > 0)
                                {
                                    AssignBy = GetEmployeeNameByUserId(dtTask.Rows[0]["CreatedBy"].ToString());
                                }
                                try
                                {
                                    TCloseDate = Convert.ToDateTime(txtempenddate.Text).ToString("dd-MMM-yyyy");
                                    //TCloseTime = Convert.ToDateTime(txtempendtime.Text).ToString("HH:mm");
                                }
                                catch
                                {
                                }
                                //strMsg = "<html><head>    <title>Task Detail</title>    <style>        body        {            font-family: Arial, Helvetica, sans-serif;            font-size: 12px;            color: #333333;        }        .main        {            width: 800px;            background-color: #F9F9F9;            margin-left: auto;            margin-right: auto;            padding: 30px;        }                                    label        {            display: inline-block;            width: 100px;            margin-right: 30px;            text-align: left;            font-size: 14px;        }        input        {        }        fieldset        {            border: none;            float: left;            margin: 0px auto;        }    </style></head><body>    <div class='main'>   <table  style='color: #C60000;font-weight:bold;font-size: 14px;margin: 0;'   width='700' align='center' cellspacing='0'>        <tr>        <td >        Company Name        </td>        <td>        :        </td>                <td>        " + CompanyName + "        </td>                        </tr>        <tr>        <td>                Date        </td>        <td>        :        </td>                <td>       " + DateTime.Now.ToString("dd-MMM-yyyy") + "                 </td>                </tr>        <tr>        <td colspan='3' align='center'>        Task Detail        </td>        </tr>        </table>                <table width='700' height='136' border='0' align='center' cellspacing='0'>            <tr>                <td width='120' height='36' >                    Task ID                </td>                <td align='center' width='120' height='36'>                    <b >:</b>                </td>                <td width='153'>                    " + hdnTaskid.Value.ToString() + "                </td>                <td width='130'>                    Deadline                </td>                <td width='130' align='center'>                    <b>:</b>                </td>                <td width='173'>                    " + txtDuration.Text + "                </td>            </tr>            <tr>                <td height='32'>                    Assign Date                </td>                <td height='32' align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "                </td>                <td>                    Assign Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "                </td>            </tr>            <tr>                <td height='34'>                    Assign To                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td>                    " + AssignTo + "                </td>                <td>                    Assign By                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + AssignBy + "                </td>            </tr>            <tr>                <td height='34'>                    Task Close Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseDate + "                </td>                <td>                    Task Close Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseTime + "                </td>            </tr>            <tr>                <td height='34'>                    Project Start Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + PSDate + "                </td>                <td>                    Project Exp. End Date                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + PExpEndDate + "                </td>            </tr>            <tr>                <td height='34'>                    Title                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td colspan='4'>                    " + txtsubject.Text + "                </td>            </tr>            <tr>                <td valign='top'>                    Description                </td>                <td align='center'  valign='top'>                    <b>:</b>                </td>                <td colspan='4'>                    " + Editor1.Content + "                </td>            </tr>        </table>    </div></body></html>";
                                strMsg = "<html>" +
"<head>" + "<title>Task Detail</title>" + "</head>" +
"<body>" +
"<table width='100%' cellpadding='2' cellspacing='0' style='letter-spacing:1px;font-size:12px; text-indent:10px;  font:Verdana, Arial, Helvetica, sans-serif; border:1px solid #ccc;'>" + "<tbody>" +
"<tr  style='background:#1886b9; color:#fff; font-size: 16px;' >" +
    "<td colspan='2' style='text-indent:10px;  ' >" + CompanyName + "</td>" +
    "<td colspan='2' align='right' style='padding-right:5px; '  >Date :" +
        DateTime.Now.ToString("dd-MMM-yyyy") + "</td> </tr> <tr >" +
    "<td colspan='4' style='font-size:20px; color:#333333; font-weight:bold; background-color:#cccccc; text-align:center;'>Task Detail   </td> </tr>                            <tr>" +
    "<td   style='text-indent:10px; width:300px;  border-right:1px solid #ccc; '>" +
    "Task Id </td><td style='text-align:left;  width:200px; border-right:1px solid #ccc; '>" + hdnTaskid.Value + "</td><td style='text-indent:10px; width:200px;  border-right:1px solid #ccc; '>" +
    "Deadline </td><td width='300'> " + txtDuration.Text.Trim() + " </td></tr><tr  style='background-color:#dce7ec' >" +
    "<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>Assign Date </td>" +
    "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
        Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "</td><td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>" +
    "Assign Time          </td> <td>" + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "     </td>" +
"          </tr>         <tr   >         		<td   style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Assign To" +
    "</td>                <td style='text-align:left;  border-right:1px solid #ccc; '>" +
   AssignTo +
    "</td>       <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>              AssignBy" +
    "</td>     <td>" + AssignBy + "     </td>" +
"</tr> <tr  style='background-color:#dce7ec' ><td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'> Task Close Date          </td>" +
    "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
    TCloseDate + "</td> <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>     Task Close Time       </td>   <td>" +
    TCloseTime + "            </td>      </tr>            <tr  >         		<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Project Start Date                </td>                <td style='text-align:left;   border-right:1px solid #ccc; '>" +
   PSDate + "                </td>     <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>  Project Exp. End Date" + "</td>   <td>" + PExpEndDate + " </td></tr> " +
         "<tr style='background-color:#dce7ec'>	<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Title" +
    "</td>         <td style='text-align:left;  border-right:1px solid #ccc;' colspan='3'>" + txtsubject.Text.Trim() + "</td>" + "</tr>          <tr > <td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>               Description                </td>       <td style='text-align:left;  border-right:1px solid #ccc; ' colspan='3'>" + Editor1.Text.Trim() + "</td></tr>	</tbody></table></body></html>";
                                //    b = ObjMail.SendMail(EmailId, txtsubject.Text, strMsg, Session["CompId"].ToString(), FilePath);
                            }
                        }
                    }
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Updated");
                }
            }
            Reset();
            txtsubject.Focus();
        }
        else
        {
            hdnProjId.Value = hdnProjectId.Value.ToString();
            if (!IsEmployeeselcted)
            {
                if (txtExtendedId.Text == "")
                {
                    a = objProjectTask.InsertProjectTask(hdnProjId.Value.ToString(), "0", AssignDate, AssignTime, EmpEndDate, EmpEndTime, "1/1/1900", "1/1/1900", txtsubject.Text.Trim(), Editor1.Text.Trim(), fileid, strTaskStatus, txtDuration.Text.Trim(), "", strparenttaskId, ddlPriority.SelectedValue, ddlTaskCompletion.SelectedValue, ddlAssigningTasktype.SelectedValue, strExpectedCost, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTaskType.SelectedValue, strSiteAddress, strContactperson, txtExpectedCost.Text, txtRequiredHrs.Text, assignBy);
                }
                else
                {
                    a = objProjectTask.InsertProjectTask(hdnProjId.Value.ToString(), "0", AssignDate, AssignTime, txtExtendedDate.Text, txtExtendedTime.Text, "1/1/1900", "1/1/1900", txtsubject.Text.Trim(), Editor1.Text.Trim(), fileid, strTaskStatus, txtDuration.Text.Trim(), "", strparenttaskId, ddlPriority.SelectedValue, ddlTaskCompletion.SelectedValue, ddlAssigningTasktype.SelectedValue, strExpectedCost, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTaskType.SelectedValue, strSiteAddress, strContactperson, txtExpectedCost.Text, txtRequiredHrs.Text, assignBy);
                    objProjectTask.UpdateExtendedIdNStatus(txtExtendedId.Text, a.ToString());
                }
                InsertTaskEmployee("Task", a.ToString());
                if (a != 0)
                {
                    DisplayMessage("Record Saved", "green");
                    Reset();
                    txtsubject.Focus();
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
                if (txtExtendedId.Text == "")
                {
                    a = objProjectTask.InsertProjectTask(hdnProjId.Value.ToString(), "0", AssignDate, AssignTime, EmpEndDate, EmpEndTime, "1/1/1900", "1/1/1900", txtsubject.Text.Trim(), Editor1.Text.Trim(), fileid, strTaskStatus, txtDuration.Text.Trim(), "", strparenttaskId, ddlPriority.SelectedValue, ddlTaskCompletion.SelectedValue, ddlAssigningTasktype.SelectedValue, strExpectedCost, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTaskType.SelectedValue, strSiteAddress, strContactperson, txtExpectedCost.Text, txtRequiredHrs.Text, assignBy);
                }
                else
                {
                    a = objProjectTask.InsertProjectTask(hdnProjId.Value.ToString(), "0", AssignDate, AssignTime, txtExtendedDate.Text, txtExtendedTime.Text, "1/1/1900", "1/1/1900", txtsubject.Text.Trim(), Editor1.Text.Trim(), fileid, strTaskStatus, txtDuration.Text.Trim(), "", strparenttaskId, ddlPriority.SelectedValue, ddlTaskCompletion.SelectedValue, ddlAssigningTasktype.SelectedValue, strExpectedCost, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTaskType.SelectedValue, strSiteAddress, strContactperson, txtExpectedCost.Text, txtRequiredHrs.Text, assignBy);
                    objProjectTask.UpdateExtendedIdNStatus(txtExtendedId.Text, a.ToString());
                }
                if (a != 0)
                {
                    InsertTaskEmployee("Task", a.ToString());
                    DataTable dtTeam = new DataTable();
                    dtTeam = objProjectTeam.GetRecordByProjectId("", hdnProjId.Value.ToString());
                    dtTeam = new DataView(dtTeam, "Task_Visibility='True'", "", DataViewRowState.CurrentRows).ToTable();
                    DataTable dtEmp = new DataTable();
                    DataTable dtEmp1 = new DataTable();
                    foreach (ListItem li in listtaskEmployee.Items)
                    {
                        HidCustId.Value = li.Value;
                        if (HidCustId.Value != "")
                        {
                            dtEmp1 = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), HidCustId.Value);
                            if (dtEmp1.Rows.Count > 0)
                            {
                                EmailId = dtEmp1.Rows[0]["Email_Id"].ToString();
                                b = false;
                                if (EmailId != "")
                                {
                                    AssignTo = GetEmployeeName(HidCustId.Value);
                                    DataTable dtUser = new DataTable();
                                    dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
                                    if (dtUser.Rows.Count > 0)
                                    {
                                        try
                                        {
                                            AssignBy = GetEmployeeName(dtUser.Rows[0]["Emp_Id"].ToString());
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    try
                                    {
                                        TCloseDate = Convert.ToDateTime(txtempenddate.Text).ToString("dd-MMM-yyyy");
                                        //TCloseTime = Convert.ToDateTime(txtempendtime.Text).ToString("HH:mm");
                                    }
                                    catch
                                    {
                                    }
                                    //  strMsg = "<html><head>    <title>Task Detail</title>    <style>        body        {            font-family: Arial, Helvetica, sans-serif;            font-size: 12px;            color: #333333;        }        .main        {            width: 800px;            background-color: #F9F9F9;            margin-left: auto;            margin-right: auto;            padding: 30px;        }                                    label        {            display: inline-block;            width: 100px;            margin-right: 30px;            text-align: left;            font-size: 14px;        }        input        {        }        fieldset        {            border: none;            float: left;            margin: 0px auto;        }    </style></head><body>    <div class='main'>   <table  style='color: #C60000;font-weight:bold;font-size: 14px;margin: 0;'   width='700' align='center' cellspacing='0'>        <tr>        <td >        Company Name        </td>        <td>        :        </td>                <td>        " + CompanyName + "        </td>                        </tr>        <tr>        <td>                Date        </td>        <td>        :        </td>                <td>       " + DateTime.Now.ToString("dd-MMM-yyyy") + "                 </td>                </tr>        <tr>        <td colspan='3' align='center'>        Task Detail        </td>        </tr>        </table>                <table width='700' height='136' border='0' align='center' cellspacing='0'>            <tr>                <td width='120' height='36' >                    Task ID                </td>                <td align='center' width='120' height='36'>                    <b >:</b>                </td>                <td width='153'>                    " + hdnTaskid.Value.ToString() + "                </td>                <td width='130'>                    Deadline                </td>                <td width='130' align='center'>                    <b>:</b>                </td>                <td width='173'>                    " + txtDuration.Text + "                </td>            </tr>            <tr>                <td height='32'>                    Assign Date                </td>                <td height='32' align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "                </td>                <td>                    Assign Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "                </td>            </tr>            <tr>                <td height='34'>                    Assign To                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td>                    " + AssignTo + "                </td>                <td>                    Assign By                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + AssignBy + "                </td>            </tr>            <tr>                <td height='34'>                    Task Close Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseDate + "                </td>                <td>                    Task Close Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseTime + "                </td>            </tr>            <tr>                <td height='34'>                    Project Start Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + PSDate + "                </td>                <td>                    Project Exp. End Date                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + PExpEndDate + "                </td>            </tr>            <tr>                <td height='34'>                    Title                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td colspan='4'>                    " + txtsubject.Text + "                </td>            </tr>            <tr>                <td valign='top'>                    Description                </td>                <td align='center'  valign='top'>                    <b>:</b>                </td>                <td colspan='4'>                    " + Editor1.Content + "                </td>            </tr>        </table>    </div></body></html>";
                                    strMsg = "<html>" +
    "<head>" + "<title>Task Detail</title>" + "</head>" +
    "<body>" +
    "<table width='100%' cellpadding='2' cellspacing='0' style='letter-spacing:1px; font-size:12px;text-indent:10px;  font:Verdana, Arial, Helvetica, sans-serif; border:1px solid #ccc;'>" + "<tbody>" +
    "<tr  style='background:#1886b9; color:#fff; font-size: 16px;' >" +
        "<td colspan='2' style='text-indent:10px;  ' >" + CompanyName + "</td>" +
        "<td colspan='2' align='right' style='padding-right:5px; '  >Date :" +
            DateTime.Now.ToString("dd-MMM-yyyy") + "</td> </tr> <tr >" +
        "<td colspan='4' style='font-size:20px; color:#333333; font-weight:bold; background-color:#cccccc; text-align:center;'>Task Detail   </td> </tr>                            <tr>" +
        "<td   style='text-indent:10px; width:300px;  border-right:1px solid #ccc; '>" +
        "Task Id </td><td style='text-align:left;  width:200px; border-right:1px solid #ccc; '>" + hdnTaskid.Value + "</td><td style='text-indent:10px; width:200px;  border-right:1px solid #ccc; '>" +
        "Deadline </td><td width='300'> " + txtDuration.Text.Trim() + " </td></tr><tr  style='background-color:#dce7ec' >" +
        "<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>Assign Date </td>" +
        "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
            Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "</td><td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>" +
        "Assign Time          </td> <td>" + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "     </td>" +
    "          </tr>         <tr   >         		<td   style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Assign To" +
        "</td>                <td style='text-align:left;  border-right:1px solid #ccc; '>" +
       AssignTo +
        "</td>       <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>              AssignBy" +
        "</td>     <td>" + AssignBy + "     </td>" +
    "</tr> <tr  style='background-color:#dce7ec' ><td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'> Task Close Date          </td>" +
        "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
        TCloseDate + "</td> <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>     Task Close Time       </td>   <td>" +
        TCloseTime + "            </td>      </tr>            <tr  >         		<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Project Start Date                </td>                <td style='text-align:left;   border-right:1px solid #ccc; '>" +
       PSDate + "                </td>     <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>  Project Exp. End Date" + "</td>   <td>" + PExpEndDate + " </td></tr> " +
             "<tr style='background-color:#dce7ec'>	<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Title" +
        "</td>         <td style='text-align:left;  border-right:1px solid #ccc;' colspan='3'>" + txtsubject.Text.Trim() + "</td>" + "</tr>          <tr > <td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>               Description                </td>       <td style='text-align:left;  border-right:1px solid #ccc; ' colspan='3'>" + Editor1.Text.Trim() + "</td></tr>	</tbody></table></body></html>";
                                    if (b)
                                    {
                                        objProjectTask.UpdateProjcetTaskMailStatus(a.ToString(), "Send");
                                    }
                                    else
                                    {
                                        objProjectTask.UpdateProjcetTaskMailStatus(a.ToString(), "UnSend");
                                    }
                                }
                                else
                                {
                                    objProjectTask.UpdateProjcetTaskMailStatus(a.ToString(), "UnSend");
                                }
                            }
                        }
                    }
                    for (int i = 0; i < dtTeam.Rows.Count; i++)
                    {
                        dtEmp = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), dtTeam.Rows[i]["Emp_Id"].ToString());
                        EmailId = string.Empty;
                        if (dtEmp.Rows.Count > 0)
                        {
                            EmailId = dtEmp.Rows[0]["Email_Id"].ToString();
                            b = false;
                            if (EmailId != "")
                            {
                                DataTable dtUser = new DataTable();
                                dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
                                if (dtUser.Rows.Count > 0)
                                {
                                    try
                                    {
                                        AssignBy = GetEmployeeName(dtUser.Rows[0]["Emp_Id"].ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                                try
                                {
                                    TCloseDate = Convert.ToDateTime(txtempenddate.Text).ToString("dd-MMM-yyyy");
                                    //TCloseTime = Convert.ToDateTime(txtempendtime.Text).ToString("HH:mm");
                                }
                                catch
                                {
                                }
                                //   strMsg = "<html><head>    <title>Task Detail</title>    <style>        body        {            font-family: Arial, Helvetica, sans-serif;            font-size: 12px;            color: #333333;        }        .main        {            width: 800px;            background-color: #F9F9F9;            margin-left: auto;            margin-right: auto;            padding: 30px;        }                                    label        {            display: inline-block;            width: 100px;            margin-right: 30px;            text-align: left;            font-size: 14px;        }        input        {        }        fieldset        {            border: none;            float: left;            margin: 0px auto;        }    </style></head><body>    <div class='main'>   <table  style='color: #C60000;font-weight:bold;font-size: 14px;margin: 0;'   width='700' align='center' cellspacing='0'>        <tr>        <td >        Company Name        </td>        <td>        :        </td>                <td>        " + CompanyName + "        </td>                        </tr>        <tr>        <td>                Date        </td>        <td>        :        </td>                <td>       " + DateTime.Now.ToString("dd-MMM-yyyy") + "                 </td>                </tr>        <tr>        <td colspan='3' align='center'>        Task Detail        </td>        </tr>        </table>                <table width='700' height='136' border='0' align='center' cellspacing='0'>            <tr>                <td width='120' height='36' >                    Task ID                </td>                <td align='center' width='120' height='36'>                    <b >:</b>                </td>                <td width='153'>                    " + hdnTaskid.Value.ToString() + "                </td>                <td width='130'>                    Deadline                </td>                <td width='130' align='center'>                    <b>:</b>                </td>                <td width='173'>                    " + txtDuration.Text + "                </td>            </tr>            <tr>                <td height='32'>                    Assign Date                </td>                <td height='32' align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "                </td>                <td>                    Assign Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "                </td>            </tr>            <tr>                <td height='34'>                    Assign To                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td>                    " + AssignTo + "                </td>                <td>                    Assign By                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + AssignBy + "                </td>            </tr>            <tr>                <td height='34'>                    Task Close Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseDate + "                </td>                <td>                    Task Close Time                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + TCloseTime + "                </td>            </tr>            <tr>                <td height='34'>                    Project Start Date                </td>                <td height='34' align='center'>                    <b>:</b>                </td>                <td>                    " + PSDate + "                </td>                <td>                    Project Exp. End Date                </td>                <td align='center'>                    <b>:</b>                </td>                <td>                    " + PExpEndDate + "                </td>            </tr>            <tr>                <td height='34'>                    Title                </td>                <td align='center' height='34'>                    <b>:</b>                </td>                <td colspan='4'>                    " + txtsubject.Text + "                </td>            </tr>            <tr>                <td valign='top'>                    Description                </td>                <td align='center'  valign='top'>                    <b>:</b>                </td>                <td colspan='4'>                    " + Editor1.Content + "                </td>            </tr>        </table>    </div></body></html>";
                                strMsg = "<html>" +
"<head>" + "<title>Task Detail</title>" + "</head>" +
"<body>" +
"<table width='100%' cellpadding='2' cellspacing='0' style='letter-spacing:1px;font-size:12px; text-indent:10px;  font:Verdana, Arial, Helvetica, sans-serif; border:1px solid #ccc;'>" + "<tbody>" +
"<tr  style='background:#1886b9; color:#fff; font-size: 16px;' >" +
  "<td colspan='2' style='text-indent:10px;  ' >" + CompanyName + "</td>" +
  "<td colspan='2' align='right' style='padding-right:5px; '  >Date :" +
      DateTime.Now.ToString("dd-MMM-yyyy") + "</td> </tr> <tr >" +
  "<td colspan='4' style='font-size:20px; color:#333333; font-weight:bold; background-color:#cccccc; text-align:center;'>Task Detail   </td> </tr>                            <tr>" +
  "<td   style='text-indent:10px; width:300px;  border-right:1px solid #ccc; '>" +
  "Task Id </td><td style='text-align:left;  width:200px; border-right:1px solid #ccc; '>" + hdnTaskid.Value + "</td><td style='text-indent:10px; width:200px;  border-right:1px solid #ccc; '>" +
  "Deadline </td><td width='300'> " + txtDuration.Text.Trim() + " </td></tr><tr  style='background-color:#dce7ec' >" +
  "<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>Assign Date </td>" +
  "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
      Convert.ToDateTime(AssignDate).ToString("dd-MMM-yyyy") + "</td><td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>" +
  "Assign Time          </td> <td>" + Convert.ToDateTime(AssignTime).ToString("HH:mm") + "     </td>" +
"          </tr>         <tr   >         		<td   style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Assign To" +
  "</td>                <td style='text-align:left;  border-right:1px solid #ccc; '>" +
 AssignTo +
  "</td>       <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>              AssignBy" +
  "</td>     <td>" + AssignBy + "     </td>" +
"</tr> <tr  style='background-color:#dce7ec' ><td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'> Task Close Date          </td>" +
  "<td style='text-align:left;  border-right:1px solid #ccc; '>" +
  TCloseDate + "</td> <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>     Task Close Time       </td>   <td>" +
  TCloseTime + "            </td>      </tr>            <tr  >         		<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Project Start Date                </td>                <td style='text-align:left;   border-right:1px solid #ccc; '>" +
 PSDate + "                </td>     <td style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>  Project Exp. End Date" + "</td>   <td>" + PExpEndDate + " </td></tr> " +
       "<tr style='background-color:#dce7ec'>	<td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>                Title" +
  "</td>         <td style='text-align:left;  border-right:1px solid #ccc;' colspan='3'>" + txtsubject.Text.Trim() + "</td>" + "</tr>          <tr > <td    style='text-indent:10px; width:200px; border-right:1px solid #ccc;'>               Description                </td>       <td style='text-align:left;  border-right:1px solid #ccc; ' colspan='3'>" + Editor1.Text.Trim() + "</td></tr>	</tbody></table></body></html>";
                                if (fileid != "")
                                {
                                }
                                // b = ObjMail.SendMail(EmailId, txtsubject.Text, strMsg, Session["CompId"].ToString(), FilePath);
                            }
                        }
                    }
                    DisplayMessage("Record Saved", "green");
                    hdnProjectId.Value = hdnProjId.Value;
                    Reset();
                    txtsubject.Focus();
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
        }
        //FillTaskList();
        if (Request.QueryString["Task"] != null)
        {
            //on bug page we pass projectid to Fill_treeList function and for task we dont pass projectid 
            //used to fill tree list for bug case
            Fill_treeList(hdnProjectId.Value.ToString());
            //used to fill the bug list after updating
            fill_BugGrid_ForProjectId(hdnProjectId.Value.ToString());
        }
        else
        {
            TaskTreeList.DataSource = FillTaskList(hdnProjectId.Value, "All", "");
            TaskTreeList.DataBind();
        }
        GetparentTask(hdnProjectId.Value);
        HidCustId.Value = "";
        hdnTaskid.Value = "";
        hdnProjId.Value = "";
        hdnfileidupdate.Value = "";
        hdnfileid.Value = "";
        //AllPageCode(clsPagePermission);
        btnsave.Enabled = true;
    }
    public void InsertTaskEmployee(string RefType, string RefId)
    {
        //this function for insert record in task_employee
        //first delete record by task_id
        objTaskEmp.DeleteRecord_By_RefTypeandRefid(RefType, RefId);
        foreach (ListItem li in listtaskEmployee.Items)
        {
            if (li.Selected)
            {
                objTaskEmp.InsertRecord(RefType, RefId, li.Value, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
    }
    private string GetEmployeeNameByUserId(string p)
    {
        //string name = string.Empty;
        //string empid = string.Empty;
        //using (DataTable dt = objUser.GetUserMasterByUserId(p.ToString(), Session["CompId"].ToString()))
        //{
        //    if (dt.Rows.Count > 0)
        //    {
        //        empid = dt.Rows[0]["Emp_Id"].ToString();
        //        try
        //        {
        //            if (empid != "")
        //            {
        //                name = GetEmployeeName(empid);
        //            }
        //        }
        //        catch
        //        {
        //        }
        //    }
        //    return name;
        //}
        DataTable dt_user = new HR_EmployeeDetail(Session["DBConnection"].ToString()).GetEmpName_CodeByUserID(p.ToString());
        if (dt_user.Rows.Count > 0)
        {
            return dt_user.Rows[0]["Emp_Name"].ToString();
        }
        else
        {
            return "No Name";
        }
    }
    public string GetEmployeeName(object empid)
    {
        //string empname = string.Empty;
        //DataTable dt = objEmpMaster.GetEmployeeMasterAllData(Session["CompId"].ToString());
        //dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //if (dt.Rows.Count > 0)
        //{
        //    empname = dt.Rows[0]["Emp_Name"].ToString();
        //    if (empname == "")
        //    {
        //        empname = "No Name";
        //    }
        //}
        //else
        //{
        //    empname = "No Name";
        //}
        //return empname;
        DataTable dt_name = new HR_EmployeeDetail(Session["DBConnection"].ToString()).GetEmpName_Code(empid.ToString());
        if (dt_name.Rows.Count > 0)
        {
            return dt_name.Rows[0]["Emp_Name"].ToString();
        }
        else
        {
            return "No Name";
        }

    }
    protected void btncencel_Click(object sender, EventArgs e)
    {
        if(lblHeader.Text.ToLower()=="project bugs")
        {
            hdnProjectId.Value = "";
        }
        Reset();
        gridbind();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        hdnProjectId.Value = "";
        txtProjectName.Text = "";
        lblProjectStartdateValue.Text = "";
        lblProjectExpenddateValue.Text = "";
        Reset();
    }
    protected void ddlParentTask_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        trParentttaskdetail.Visible = false;
        if (ddlParentTask.SelectedIndex > 0)
        {
            //dt = objProjectTask.GetRecordByTaskId(ddlParentTask.Value.ToString());
            dt = objProjectTask.GetRecordByTaskId(ddlParentTask.SelectedValue.ToString());
            if (dt.Rows.Count > 0)
            {
                trParentttaskdetail.Visible = true;
                lblParentttaskDetailStart.Text = "Start date & Time = " + Formatdate(dt.Rows[0]["Assign_Date"].ToString()) + " " + FormatTime(dt.Rows[0]["Assign_Time"].ToString());
                lblParentttaskDetailEnd.Text = "End date & time = " + Formatdate(dt.Rows[0]["Emp_Close_Date"].ToString()) + " " + FormatTime(dt.Rows[0]["Emp_Close_Time"].ToString());
            }
        }
        dt.Dispose();
    }
    protected void btnEdit_command(object sender, CommandEventArgs e)
    {
        if (Session["EmpId"].ToString() == "0")
        {
            DisplayMessage("You cannot edit this task");
            return;
        }
        hdnProjId.Value = "";
        hdnTaskid.Value = "";
        hdnfileid.Value = "";
        hdnTaskid.Value = e.CommandArgument.ToString();
        DataTable dt = new DataTable();
        dt = objProjectTask.GetRecordByTaskId(hdnTaskid.Value.ToString());
        if (dt.Rows.Count > 0)
        {
            hdnProjId.Value = dt.Rows[0]["Project_Id"].ToString();
            if (dt.Rows[0]["Status"].ToString() == "Closed")
            {
                tlEnddate.Visible = true;
            }
            try
            {
                ddlAssigningTasktype.SelectedValue = dt.Rows[0]["Field6"].ToString();
            }
            catch
            {
                ddlAssigningTasktype.SelectedIndex = 0;
            }
            try
            {
                hdnProjectId.Value = dt.Rows[0]["Project_Id"].ToString();
                txtProjectName.Text = dt.Rows[0]["Project_Name"].ToString();
            }
            catch
            {
                //ddlprojectname.Items.Clear();
                //ListEditItem Li1 = new ListEditItem();
                //Li1.Text = dt.Rows[0]["Project_Name"].ToString();
                //Li1.Value = dt.Rows[0]["Project_Id"].ToString();
                //ddlprojectname.Items.Insert(0, Li1);
                //ddlprojectname.SelectedIndex = 0;
            }
            txtExtendedId.Text = dt.Rows[0]["Extended_Id"].ToString();
            //ddlprojectname_SelectedIndexChanged(null, null);

            // fillDetails is replacement of ddlprojectname_SelectedIndexChanged
            fillDetails();

            if (dt.Rows[0]["Status"].ToString() == "Pending")
            {
                DataTable dttaskemp = new DataTable();
                foreach (ListItem li in listtaskEmployee.Items)
                {
                    dttaskemp = objTaskEmp.GetRecordBy_RefType_and_RefId("Task", e.CommandArgument.ToString());
                    dttaskemp = new DataView(dttaskemp, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dttaskemp.Rows.Count > 0)
                    {
                        li.Selected = true;
                    }
                }
                txtassigndate.Text = Formatdate(dt.Rows[0]["Assign_Date"].ToString());
                if(txtassigndate.Text.Trim() != "")
                {
                    if(dt.Rows[0]["Task_Type"].ToString() == "Internal")
                        txtassigndate.Enabled = Convert.ToBoolean(canModifyDate.Value);
                    else
                        txtassigndate.Enabled = true;
                }
                //txtassigntime.Text = FormatTime(dt.Rows[0]["Assign_Time"].ToString());
                txtempenddate.Text = Formatdate(dt.Rows[0]["Emp_Close_Date"].ToString());
                if (txtempenddate.Text.Trim() != "")
                {
                    if (dt.Rows[0]["Task_Type"].ToString() == "Internal")
                        txtempenddate.Enabled = Convert.ToBoolean(canModifyDate.Value);
                    else
                        txtempenddate.Enabled = true;
                }
                //txtempendtime.Text = FormatTime(dt.Rows[0]["Emp_Close_Time"].ToString());
                txttlenddate.Text = Formatdate(dt.Rows[0]["TL_Close_Date"].ToString());
                txtRequiredHrs.Text = dt.Rows[0]["requiredhours"].ToString();
                if (txtRequiredHrs.Text.Trim() != "")
                {
                    txtRequiredHrs.Enabled = Convert.ToBoolean(canModifyDate.Value);
                }
                txtAssignBy.Text = dt.Rows[0]["assignbyname"].ToString() + "/" + dt.Rows[0]["assignby"].ToString();
                txtAssignBy.Text = dt.Rows[0]["assignbyname"].ToString() + "/" + dt.Rows[0]["assignby"].ToString();
                txttlendtime.Text = FormatTime(dt.Rows[0]["TL_Close_Time"].ToString());
                txtsubject.Text = dt.Rows[0]["Subject"].ToString();
                Editor1.Text = dt.Rows[0]["Description"].ToString();
                HidCustId.Value = dt.Rows[0]["Emp_Id"].ToString();
                hdnfileidupdate.Value = dt.Rows[0]["File_Id"].ToString();
                if (dt.Rows[0]["Expected_Cost"].ToString() != "")
                {
                    txtExpectedCost.Text = Common.GetAmountDecimal(dt.Rows[0]["Expected_Cost"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                }
                txtDuration.Text = dt.Rows[0]["Field1"].ToString();
                Editor1.Enabled = true;
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else
            {
                if (dt.Rows[0]["Status"].ToString().Trim() == "Cancelled")
                {
                    chkCancel.Checked = true;
                }
                string EmpId = string.Empty;
                DataTable dtUser = new DataTable();
                dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
                if (dtUser.Rows.Count > 0)
                {
                    EmpId = dtUser.Rows[0]["Emp_Id"].ToString();
                }
                else
                {
                    EmpId = "0";
                }
                DataTable dtTeam = new DataTable();
                dtTeam = objProjectTeam.GetRecordByProjectId("", dt.Rows[0]["Project_Id"].ToString());
                dtTeam = new DataView(dtTeam, "Emp_Id='" + EmpId + "' and Task_Visibility='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTeam.Rows.Count > 0)
                {
                    DataTable dttaskemp = new DataTable();
                    foreach (ListItem li in listtaskEmployee.Items)
                    {
                        dttaskemp = objTaskEmp.GetRecordBy_RefType_and_RefId("Task", e.CommandArgument.ToString());
                        dttaskemp = new DataView(dttaskemp, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dttaskemp.Rows.Count > 0)
                        {
                            li.Selected = true;
                        }
                    }
                    Editor1.Text = dt.Rows[0]["Description"].ToString();
                    txtassigndate.Text = Formatdate(dt.Rows[0]["Assign_Date"].ToString());
                    //txtassigntime.Text = FormatTime(dt.Rows[0]["Assign_Time"].ToString());
                    txtempenddate.Text = Formatdate(dt.Rows[0]["Emp_Close_Date"].ToString());
                    txtRequiredHrs.Text = dt.Rows[0]["Requiredhours"].ToString();
                    txtAssignBy.Text = dt.Rows[0]["assignbyname"].ToString() + "/" + dt.Rows[0]["assignby"].ToString();
                    if (txtassigndate.Text.Trim() != "")
                    {
                        
                        if (dt.Rows[0]["Task_Type"].ToString() == "Internal")
                            txtassigndate.Enabled = Convert.ToBoolean(canModifyDate.Value);
                        else
                            txtassigndate.Enabled = true;
                    }
                    if (txtempenddate.Text.Trim() != "")
                    {
                        if (dt.Rows[0]["Task_Type"].ToString() == "Internal")
                            txtempenddate.Enabled = Convert.ToBoolean(canModifyDate.Value);
                        else
                            txtempenddate.Enabled = true;
                    }
                    if (txtRequiredHrs.Text.Trim() != "")
                    {
                        if (dt.Rows[0]["Task_Type"].ToString() == "Internal")
                            txtRequiredHrs.Enabled = Convert.ToBoolean(canModifyDate.Value);
                        else
                            txtempenddate.Enabled = true;
                        
                    }
                    //txtempendtime.Text = FormatTime(dt.Rows[0]["Emp_Close_Time"].ToString());
                    txttlenddate.Text = Formatdate(dt.Rows[0]["TL_Close_Date"].ToString());
                    txttlendtime.Text = FormatTime(dt.Rows[0]["TL_Close_Time"].ToString());
                    txtsubject.Text = dt.Rows[0]["Subject"].ToString();
                    if (dt.Rows[0]["Expected_Cost"].ToString() != "")
                    {
                        txtExpectedCost.Text = Common.GetAmountDecimal(dt.Rows[0]["Expected_Cost"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                    }
                    //txtfilename.Text = dt.Rows[0]["File_Name"].ToString();
                    HidCustId.Value = dt.Rows[0]["Emp_Id"].ToString();
                    hdnfileidupdate.Value = dt.Rows[0]["File_Id"].ToString();
                    txtDuration.Text = dt.Rows[0]["Field1"].ToString();
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }
                else
                {
                    DisplayMessage("You cannot edit this task");
                    return;
                }
            }
            try
            {
                ddlPriority.SelectedValue = dt.Rows[0]["Field4"].ToString();
                ddlTaskCompletion.SelectedValue = dt.Rows[0]["Field5"].ToString();
                ddlTaskType.SelectedValue = dt.Rows[0]["Task_Type"].ToString();
            }
            catch
            {
            }
            ddlTaskType_SelectedIndexChanged(null, null);
            if (dt.Rows[0]["Field3"].ToString() != "" && dt.Rows[0]["Field3"].ToString() != "0")
            {
                //ddlParentTask.Value = dt.Rows[0]["Field3"].ToString();
                ddlParentTask.SelectedValue = dt.Rows[0]["Field3"].ToString();
            }
            //here we are checking task type internal or external
            if (ddlTaskType.SelectedIndex == 1)
            {
                txtSiteAddress.Text = dt.Rows[0]["Address_Name"].ToString();
                hdnAddressId.Value = dt.Rows[0]["Task_Site_Address"].ToString();
                if (dt.Rows[0]["Contact_Person"].ToString().Trim() != "")
                {
                    foreach (ListItem li in ChkContactPerson.Items)
                    {
                        if (dt.Rows[0]["Contact_Person"].ToString().Trim().Split(',').Contains(li.Value))
                        {
                            li.Selected = true;
                        }
                    }
                }
            }
        }
        //listtaskEmployee_OnSelectedIndexChanged(null, null);
        dt.Dispose();
        txtsubject.Focus();
    }
    protected void btnComment_command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../ProjectManagement/Projecttaskfeedback.aspx?Task_Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnBugs_command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../ProjectManagement/Projecttask.aspx?Task=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void ddlprojectname_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnProjectId.Value = objProjctMaster.GetProjectIdByName(txtProjectName.Text);
        if (hdnProjectId.Value == "")
        {
            DisplayMessage("Please Select from suggestions");
            txtProjectName.Text = "";
            txtProjectName.Focus();
            return;
        }
        if (Request.QueryString["Task"] == null)
        {
            Fill_treeList("");
        }
        else
        {
            try
            {
                Fill_treeList(Request.QueryString["Project_id"].ToString());
            }
            catch
            {
                Fill_treeList(hdnProjectId.Value.ToString());
            }
        }
        trAssigneddateInfo.Visible = true;
        trassignee.Visible = true;
       // trteamavailable.Visible = true;
        trparenttask.Visible = true;
        string strCustomerId = string.Empty;
        ChkContactPerson.Items.Clear();
        listtaskEmployee.Items.Clear();
        listtaskEmployee.DataSource = null;
        listtaskEmployee.DataBind();
        CBLAssignTo.Items.Clear();
        CBLAssignTo.DataSource = null;
        CBLAssignTo.DataBind();

        //ddlSearchprojectName.SelectedIndex = ddlprojectname.SelectedIndex;
        txtSearchprojectName.Text = txtProjectName.Text;
        DataTable dt = new DataTable();
        string strProjectId = string.Empty;
        if (hdnProjectId.Value != "")
        {
            strProjectId = hdnProjectId.Value.ToString();
        }
        else
        {
            strProjectId = "0";
        }

        try
        {
            strCustomerId = dt.Rows[0]["Customer_Id"].ToString();
        }
        catch
        {
            strCustomerId = "0";
        }
        DataTable dtAddress = objContactmaster.GetAddressByRefType_Id("Contact", strCustomerId);
        if (dtAddress != null && dtAddress.Rows.Count > 0)
        {
            if (txtSiteAddress.Text.Trim() == "")
            {
                txtSiteAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
            }
        }
        else
        {
            txtSiteAddress.Text = "";
        }
        if (hdnProjectId.Value != "")
        {
            tdprojectSchedule.Visible = true;
            DataTable dtPrjectRecord = new DataTable();
            dtPrjectRecord = objProjctMaster.GetStartNEndDateProjectMasteer(strProjectId);
            if (dtPrjectRecord.Rows.Count > 0)
            {
                lblProjectStartdateValue.Text = Formatdate(dtPrjectRecord.Rows[0]["Start_Date"].ToString());
                lblProjectExpenddateValue.Text = Formatdate(dtPrjectRecord.Rows[0]["Exp_End_Date"].ToString());
            }
            else
            {
                lblProjectStartdateValue.Text = "";
                lblProjectExpenddateValue.Text = "";
            }
            dtPrjectRecord = null;
        }
        else
        {
            tdprojectSchedule.Visible = false;
        }
        //GetContactPerson();
        //ViewState["CanPrintNAssign_Task"] = "false";
        //if (Request.QueryString["Task"] == null)
        //{
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        if (dr["Emp_Id"].ToString() == Session["EmpId"].ToString() && dr["Task_Visibility"].ToString().ToUpper() == "TRUE")
        //        {
        //            ViewState["CanPrintNAssign_Task"] = "true";
        //            break;
        //        }
        //    }
        //}
        dt = objProjectTeam.GetRecordByProjectId("", strProjectId);
        try
        {
            dt = new DataView(dt, "", "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                listtaskEmployee.DataSource = dt;
                listtaskEmployee.DataTextField = "EmpName_Designation";
                listtaskEmployee.DataValueField = "Emp_Id";
                listtaskEmployee.DataBind();
                CBLAssignTo.DataSource = dt;
                CBLAssignTo.DataTextField = "EmpName_Designation";
                CBLAssignTo.DataValueField = "Emp_Id";
                CBLAssignTo.DataBind();
            }
        }
        catch
        {

        }
        hdnProjId.Value = strProjectId;
        GetparentTask(strProjectId);
        //here we are set visibility according login employee and bugs
        if (Request.QueryString["Task"] != null)
        {
            DataTable dtBugs = objProjectTeam.GetRecordEmpIdTaskVisibility(Session["EmpId"].ToString(), "True", strProjectId);
            if (dtBugs.Rows.Count == 0)
            {
                trAssigneddateInfo.Visible = false;
                trassignee.Visible = false;
                //trteamavailable.Visible = false;
                trparenttask.Visible = false;
            }
            dtBugs.Dispose();
        }
    }
    public void GetContactPerson(string addressId)
    {
        ChkContactPerson.Items.Clear();
        DataTable dtIndividual = objContactmaster.GetAllContactNameByAddressId(addressId);
        objPageCmn.FillData((object)ChkContactPerson, dtIndividual, "Name", "Trans_Id");
    }
    protected void listtaskEmployee_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bool isEmpselected = false;
        foreach (ListItem li in listtaskEmployee.Items)
        {
            if (li.Selected)
            {
                isEmpselected = true;
                break;
            }
        }
        HidCustId.Value = "";
        if (!isEmpselected)
        {
            HidCustId.Value = "0";
            txtempenddate.Text = "";
            //txtempendtime.Text = "";
            txtassigndate.Text = "";
            //txtassigntime.Text = "";
        }
        else
        {
            txtempenddate.Enabled = true;
            //txtempendtime.Enabled = true;
            txtassigndate.Enabled = true;
            //txtassigntime.Enabled = true;
        }
    }
    public string[] getCustomerInformation(string Contactid)
    {
        //code start
        string[] strCusInfo = new string[4];
        string strContactNo = string.Empty;
        string Address = string.Empty;
        string Longitude = string.Empty;
        string Latitude = string.Empty;
        DataTable dtContact = objContactmaster.GetContactTrueById(Contactid);
        if (dtContact.Rows.Count > 0)
        {
            strCusInfo[1] = dtContact.Rows[0]["Field2"].ToString();
            if (strCusInfo[1] == null)
            {
                strCusInfo[1] = "";
            }
        }
        else
        {
            strCusInfo[1] = "";
        }
        //for get address
        DataTable dt = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", Contactid);
        try
        {
            dt = new DataView(dt, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            Address = dt.Rows[0]["FullAddress"].ToString();
            strCusInfo[0] = Address;
            strCusInfo[2] = dt.Rows[0]["Longitude"].ToString();
            strCusInfo[3] = dt.Rows[0]["Latitude"].ToString();
        }
        else
        {
            strCusInfo[0] = "";
            strCusInfo[2] = "0.0000";
            strCusInfo[3] = "0.0000";
        }
        return strCusInfo;
    }

    #region ProjectType
    protected void ddlTaskType_SelectedIndexChanged(object sender, EventArgs e)
    {
        trContactperson.Visible = false;
        trAddress.Visible = false;
        trIsAddNewCustomer.Visible = false;
        if (ddlTaskType.SelectedIndex == 1)
        {
            trContactperson.Visible = true;
            trAddress.Visible = true;
            trIsAddNewCustomer.Visible = IsAddCustomerPermission();
        }
    }
    #endregion
    #region GetSiteAddress
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.GetDistinctAddressName(prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
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
    protected void txtShipingAddress_TextChanged(object sender, EventArgs e)
    {
        if (txtSiteAddress.Text != "")
        {
            DataTable dtAM = objAddress.GetAddressDataByAddressName(txtSiteAddress.Text.Trim().Split('/')[0].ToString());
            //txtShipCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dtAM.Rows.Count > 0)
            {
            }
            else
            {
                txtSiteAddress.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSiteAddress);
                return;
            }
        }
    }
    #endregion
    #region AddContact
    protected void btnaddContact_Click(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../Sales/AddContact.aspx?Page=SINV','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (Session["EmpId"].ToString() == "0")
        {
            allow = true;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "107", "19", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            allow = true;
        }
        return allow;
    }
    protected void btnRefreshContactList_Click(object sender, EventArgs e)
    {
        GetContactPerson(hdnAddressId.Value);
    }
    #endregion
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string ID = e.CommandArgument.ToString().Split('/')[0];
            string projectNo = e.CommandArgument.ToString().Split('/')[1];
            FUpload1.setID(ID, Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/Project", "ProjectManagement", "Project", projectNo, e.CommandName.ToString());
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
        }
        catch (Exception ex)
        {

        }
    }
    protected string GetDate_New(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(ObjSysParam.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

    protected void btnAssignTaskToEmp_Click(object sender, EventArgs e)
    {
        if (hdnProjectId.Value == "")
        {
            DisplayMessage("Please Select a Project");
            return;
        }
        string data1 = "", data2 = "";
        bool AllSameEmp = true;
        string SelectedTask = "";
        hdnSelectedTask.Value = "";
        txtFrDt.Text = "";
        txtToDt.Text = "";
        txtRequiredHr.Text = "";
        CBLAssignTo.Items.Clear();
        CBLAssignTo.DataSource = null;
        CBLAssignTo.DataBind();
        DataTable dt = objProjectTeam.GetRecordByProjectId("", hdnProjectId.Value.ToString());
        DataView dvProjectName = new DataView(dt);
        dvProjectName.Sort = "Emp_Name";
        CBLAssignTo.DataSource = dvProjectName;
        CBLAssignTo.DataTextField = "Emp_Name";
        CBLAssignTo.DataValueField = "Emp_Id";
        CBLAssignTo.DataBind();
        foreach (ListItem li in CBLAssignTo.Items)
        {
            li.Selected = false;
        }
        if (TaskTreeList.SelectionCount == 0)
        {
            DisplayMessage("Please select a task");
            return;
        }
        for (int i = 0; i < TaskTreeList.SelectionCount; i++)
        {
            SelectedTask += TaskTreeList.GetSelectedNodes().ToList()[i].Key.ToString() + ",";
            if (i == 0)
            {
                data1 = objProjectTeam.GetEmpIdListByTrnsID(TaskTreeList.GetSelectedNodes().ToList()[i].Key.ToString());
            }
            else
            {
                data2 = objProjectTeam.GetEmpIdListByTrnsID(TaskTreeList.GetSelectedNodes().ToList()[i].Key.ToString());
            }
            if (data1 != data2 && data2 != "")
            {
                AllSameEmp = false;
                break;
            }
        }
        if (!AllSameEmp)
        {
            DisplayMessage("Selected Task Contains Different Employee List");
            return;
        }
        DataTable dttaskemp = new DataTable();
        DataTable dttaskempFiltered = new DataTable();
        dttaskemp = objTaskEmp.GetRecordBy_RefType_and_RefId("Task", TaskTreeList.GetSelectedNodes().ToList()[0].Key.ToString());
        foreach (ListItem li in CBLAssignTo.Items)
        {
            if (dttaskemp.Rows.Count > 0)
            {
                dttaskempFiltered = new DataView(dttaskemp, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dttaskempFiltered.Rows.Count > 0)
                {
                    li.Selected = true;
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "", "OpenEmployeeListPopup()", true);
        hdnSelectedTask.Value = SelectedTask;
    }
    protected void btnUpdateTask_Click(object sender, EventArgs e)
    {
        if (hdnSelectedTask.Value == "")
        {
            DisplayMessage("No Task Selected");
            return;
        }
        if (txtFrDt.Text.Trim() == "")
        {
            DisplayMessage("Enter From Date");
            txtFrDt.Focus();
            return;
        }
        if (txtToDt.Text.Trim() == "")
        {
            DisplayMessage("Enter To Date");
            txtToDt.Focus();
            return;
        }

        if (txtRequiredHr.Text.Trim() == "")
        {
            DisplayMessage("Enter Required Hour");
            txtToDt.Focus();
            return;
        }

        List<string> letters = hdnSelectedTask.Value.Split(',').ToList();
        foreach (var letter in letters)
        {
            if (letter == "")
            {
                continue;
            }
            if (!checkValidation(letter))
            {
                break;
            }
            objProjectTask.UpdateFromDt_ToDt_TimeByTaskId(txtFrDt.Text, txtToDt.Text, txtRequiredHr.Text, letter.ToString());
            objTaskEmp.DeleteRecord_By_RefTypeandRefid("Task", letter.ToString());
            foreach (ListItem li in CBLAssignTo.Items)
            {
                if (li.Selected)
                {
                    objTaskEmp.InsertRecord("Task", letter.ToString(), li.Value, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Updated Successfully');", true);
        TaskTreeList.DataSource = FillTaskList(hdnProjectId.Value, "All", "");
        TaskTreeList.DataBind();
    }
    public bool checkValidation(string taskId)
    {
        DataTable dtFeedback = new DataTable();
        dtFeedback = objProjectTask.GetRecordByTaskId(taskId);
        if (dtFeedback.Rows[0]["Status"].ToString().Trim() != "Assigned")
        {
            string msg = "Cant Edit this task because its status is already changed to " + dtFeedback.Rows[0]["Status"].ToString();
            FillTaskList();
            DisplayMessage(msg);
            return false;
        }
        DateTime dtStartdate = Convert.ToDateTime(dtFeedback.Rows[0]["Start_Date"].ToString());
        DateTime dtTLStartdate = Convert.ToDateTime(txtFrDt.Text);
        if (dtStartdate > dtTLStartdate)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('From Date Should be greater then Project Start Date');", true);
            return false;
        }
        DateTime dtEnddate = Convert.ToDateTime(dtFeedback.Rows[0]["Exp_End_Date"].ToString());
        DateTime dtTLclosedate = Convert.ToDateTime(txtToDt.Text);
        if (dtEnddate < dtTLclosedate)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('To Date should be Less or equal to Project Close Date');", true);
            return false;
        }
        return true;
    }
    protected void btnPrintTask_Click(object sender, EventArgs e)
    {
        if (hdnProjectId.Value.ToString() == "" && txtSearchprojectName.Text == "")
        {
            DisplayMessage("Select project");
            txtSearchprojectName.Text = "";
            txtSearchprojectName.Focus();
            return;
        }
        ViewState["dt_TaskContractReport"] = null;
        bool AllSameEmp = true;
        lblSelectRecord.Text = "";
        string data1 = "", data2 = "";
        hdnRPTClick.Value = "1";
        if (TaskTreeList.SelectionCount > 1)
        {
            for (int i = 0; i < TaskTreeList.SelectionCount; i++)
            {
                lblSelectRecord.Text += TaskTreeList.GetSelectedNodes().ToList()[i].Key.ToString() + ",";
                if (i == 0)
                {
                    data1 = objProjectTeam.GetEmpIdListByTrnsID(TaskTreeList.GetSelectedNodes().ToList()[i].Key.ToString());
                }
                else
                {
                    data2 = objProjectTeam.GetEmpIdListByTrnsID(TaskTreeList.GetSelectedNodes().ToList()[i].Key.ToString());
                    if (data1 != data2)
                    {
                        AllSameEmp = false;
                    }
                }
            }
        }
        else
        {
            try
            {
                lblSelectRecord.Text = TaskTreeList.GetSelectedNodes().ToList()[0].Key.ToString() + ",";
            }
            catch
            {
            }
        }
        if (lblSelectRecord.Text == "")
        {
            DisplayMessage("Please select task");
            return;
        }
        if (!AllSameEmp)
        {
            DisplayMessage("Selected Task Contains Different Employees");
            return;
        }
        try
        {
            if (hdnRPTClick.Value == "1")
            {
                GetReportStatement();
            }
        }
        catch (Exception er)
        {
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OpenTaskContractReportPopup()", true);
    }
    public void GetReportStatement()
    {
        try
        {
            if (hdnProjectId.Value == "")
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
        if (lblSelectRecord.Text != "")
        {
            dtFilter = new DataView(dtFilter, "Task_Id in (" + lblSelectRecord.Text.Substring(0, lblSelectRecord.Text.Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
        }
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
        DataTable dtFinalTable = GetFinalTable(dtFilter);
        dtFilter = new DataView(dtFilter, "ParentTaskId='0'", "", DataViewRowState.CurrentRows).ToTable();
        Session["DtprojectTask"] = dtFilter.DefaultView.ToTable(true, "Task_Id", "Subject", "Assign_date", "Address", "Status");
        objTaskcontract.DataSource = dtFinalTable;
        objTaskcontract.DataMember = "sp_Prj_ProjectHistory_Report";
        objTaskcontract.CreateDocument();
        ReportViewer1.OpenReport(objTaskcontract);
        dtFilter.Dispose();
        dtFinalTable.Dispose();
    }
    public DataTable GetFinalTable(DataTable DtFilter)
    {
        DataTable dt = new DataTable();
        dt = DtFilter.Clone();
        DataTable dtTemp = new DataView(DtFilter, "ParentTaskId='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtTemp.Rows.Count == 0)
        {
            dt = DtFilter.Copy();
        }
        else
        {
            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                try
                {
                    dtTemp.Rows[i]["Subject"] = (i + 1).ToString() + " " + dtTemp.Rows[i]["Subject"].ToString();
                }
                catch
                {
                }
                try
                {
                    dtTemp.Rows[i]["Expr15"] = (i + 1).ToString() + " " + dtTemp.Rows[i]["Expr15"].ToString();
                }
                catch
                {
                }
                dt.ImportRow(dtTemp.Rows[i]);
                dt.Merge(childNodeSave(DtFilter, dtTemp.Rows[i]["Task_Id"].ToString(), dt, 1, (i + 1).ToString()));
            }
        }
        return dt;
    }
    public DataTable childNodeSave(DataTable dtData, string strRefTaskId, DataTable DtNewTable, int depth, string strlevel)
    {
        DataTable dtTemp = new DataView(dtData, "ParentTaskId=" + strRefTaskId + "", "", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dtTemp.Rows.Count)
        {
            try
            {
                dtTemp.Rows[i]["Subject"] = addSpaces(dtTemp.Rows[i]["Subject"].ToString(), (depth * 5), strlevel + "." + (i + 1).ToString());
            }
            catch
            {
            }
            try
            {
                dtTemp.Rows[i]["Expr15"] = addSpaces(dtTemp.Rows[i]["Expr15"].ToString(), (depth * 5), strlevel + "." + (i + 1).ToString());
            }
            catch
            {
            }
            DtNewTable.ImportRow(dtTemp.Rows[i]);
            childNodeSave(dtData, dtTemp.Rows[i]["Task_Id"].ToString(), DtNewTable, depth + 1, strlevel + "." + (i + 1).ToString());
            i++;
        }
        return DtNewTable;
    }
    public string addSpaces(string strValue, int spacecount, string strLevel)
    {
        string str = string.Empty;
        for (int i = 0; i < spacecount; i++)
        {
            str += " ";
        }
        return str + strLevel + " " + strValue;
    }
    [System.Web.Services.WebMethod(enableSession: true)]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool FillTaskListAjax(string projectId, string Status, string taskId)
    {
        Prj_ProjectTask objproject = new Prj_ProjectTask(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtprojectTask = new DataTable();
        string EmpId = HttpContext.Current.Session["EmpId"].ToString();
        bool isTask = false;
        if (taskId != "")
        {
            HttpContext.Current.Session["DT_BugGrid"] = null;
            dtprojectTask = objproject.GetBugDataForProjectId(projectId);
            HttpContext.Current.Session["DT_BugGrid"] = dtprojectTask;
            isTask = true;
        }
        else
        {
            HttpContext.Current.Session["DT_TreeView"] = null;
            if (projectId != "" && projectId != "0")
            {
                dtprojectTask = objproject.GetTaskTreeData(EmpId, projectId);
            }
            else
            {
                dtprojectTask = objproject.GetTaskTreeData(EmpId);
            }
            if (Status != "All")
            {
                dtprojectTask = new DataView(dtprojectTask, "Status='" + Status.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            HttpContext.Current.Session["DT_TreeView"] = dtprojectTask;
            isTask = false;
        }
        dtprojectTask.Dispose();
        return isTask;
    }
    public void Fill_treeList(string projectId)
    {
        DataTable dtprojectTask = new DataTable();
        //used for bug page
        if (projectId != "")
        {
            dtprojectTask = objProjectTask.GetBugDataForProjectId(projectId);
            Session["DT_BugGrid"] = dtprojectTask;
        }
        // used for task page 
        else
        {
            if (hdnProjectId.Value != "")
            {
                dtprojectTask = objProjectTask.GetTaskTreeData(Session["EmpId"].ToString(), hdnProjectId.Value.ToString());
            }

            Session["DT_TreeView"] = dtprojectTask;
        }
        TaskTreeList.DataSource = dtprojectTask;
        TaskTreeList.DataBind();
        //treeList.DataSource = dtprojectTask;
        //treeList.DataBind();
        dtprojectTask = null;
    }
    public DataTable fill_BugGrid_ForProjectId(string projectId)
    {
        DataTable dtprojectTask = objProjectTask.GetBugDataForProjectId(projectId);
        GvrProjecttask.DataSource = dtprojectTask;
        GvrProjecttask.DataBind();
        Session["dtFilter_ProjectTask"] = dtprojectTask;
        return dtprojectTask;
    }
    protected void Btn_FillBugGrid_Click(object sender, EventArgs e)
    {
        GvrProjecttask.DataSource = Session["DT_BugGrid"] as DataTable;
        GvrProjecttask.DataBind();
        Session["dtFilter_ProjectTask"] = Session["DT_BugGrid"] as DataTable;
    }
    public void Reset_Fields()
    {
        
        HidCustId.Value = "";
        hdnTaskid.Value = "";
        hdnProjId.Value = "";
        hdnfileidupdate.Value = "";
        hdnfileid.Value = "";
        chkCancel.Checked = false;
        div_extended.Style.Add("display", "none");
        div_assign.Attributes.Add("Class", "col-md-4");
        div_end.Attributes.Add("Class", "col-md-4");
        txtassigndate.Text = "";
        // txtassigntime.Text = "";
        txtempenddate.Text = "";
        txtRequiredHrs.Text = "";
        txtAssignBy.Text = "";
        txttlenddate.Text = "";
        txttlendtime.Text = "";
        txtsubject.Text = "";
        Editor1.Text = "";
        txtExpectedCost.Text = "";
        txtExtendedId.Text = "";
        txtDuration.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlTaskType.SelectedIndex = 0;
        ddlTaskType_SelectedIndexChanged(null, null);
        txtSiteAddress.Text = "";
        ChkContactPerson.Items.Clear();
        trParentttaskdetail.Visible = false;
        tlEnddate.Visible = false;
        if (hdnProjectId.Value != "")
        {
            GetparentTask(hdnProjectId.Value.ToString());
        }
        else
        {
            GetparentTask("0");
        }
        foreach (ListItem li in listtaskEmployee.Items)
        {
            li.Selected = false;
        }
    }

    public void fillDetails()
    {
        // start replacement of ddlprojectname_SelectedIndexChanged
        trAssigneddateInfo.Visible = true;
        trassignee.Visible = true;
        //trteamavailable.Visible = true;
        trparenttask.Visible = true;
        string strCustomerId = string.Empty;
        ChkContactPerson.Items.Clear();
        listtaskEmployee.Items.Clear();
        listtaskEmployee.DataSource = null;
        txtSearchprojectName.Text = txtProjectName.Text;
        DataTable dt1 = new DataTable();
        string strProjectId = string.Empty;
        if (hdnProjectId.Value != "")
        {
            strProjectId = hdnProjectId.Value.ToString();
        }
        else
        {
            strProjectId = "0";
        }
        dt1 = objProjectTeam.GetRecordByProjectId("", strProjectId);

        DataView dvProjectName = new DataView(dt1);
        dvProjectName.Sort = "Emp_Name";
        try
        {
            strCustomerId = dt1.Rows[0]["Customer_Id"].ToString();
        }
        catch
        {
            strCustomerId = "0";
        }
        dt1.Dispose();

        if (hdnProjectId.Value != "")
        {
            tdprojectSchedule.Visible = true;
            DataTable dtPrjectRecord = new DataTable();
            dtPrjectRecord = objProjctMaster.GetStartNEndDateProjectMasteer(strProjectId);
            if (dtPrjectRecord.Rows.Count > 0)
            {
                lblProjectStartdateValue.Text = Formatdate(dtPrjectRecord.Rows[0]["Start_Date"].ToString());
                lblProjectExpenddateValue.Text = Formatdate(dtPrjectRecord.Rows[0]["Exp_End_Date"].ToString());
            }
            else
            {
                lblProjectStartdateValue.Text = "";
                lblProjectExpenddateValue.Text = "";
            }
            dtPrjectRecord.Dispose();
        }
        else
        {
            tdprojectSchedule.Visible = false;
        }

        listtaskEmployee.DataSource = dvProjectName;
        listtaskEmployee.DataTextField = "EmpName_Designation";
        listtaskEmployee.DataValueField = "Emp_Id";
        listtaskEmployee.DataBind();
        hdnProjId.Value = strProjectId;
        GetparentTask(strProjectId);

        //here we are set visibility according login employee and bugs
        if (Request.QueryString["Task"] != null)
        {
            DataTable dtBugs = objProjectTeam.GetRecordEmpIdTaskVisibility(Session["EmpId"].ToString(), "True", strProjectId);
            if (dtBugs.Rows.Count == 0)
            {
                trAssigneddateInfo.Visible = false;
                trassignee.Visible = false;
                //trteamavailable.Visible = false;
                trparenttask.Visible = false;
            }
            dtBugs.Dispose();
        }

        //end
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProject(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["DT_ddlProjectName"] == null)
        {
            return null;
        }

        DataTable dtCon = HttpContext.Current.Session["DT_ddlProjectName"] as DataTable;
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
    public DataTable FillTaskList(string projectId, string Status, string taskId)
    {
        DataTable dtprojectTask = new DataTable();
        string EmpId = HttpContext.Current.Session["EmpId"].ToString();
        if (taskId != "")
        {
            HttpContext.Current.Session["DT_BugGrid"] = null;
            dtprojectTask = objProjectTask.GetBugDataForProjectId(projectId);
            HttpContext.Current.Session["DT_BugGrid"] = dtprojectTask;
        }
        else
        {
            HttpContext.Current.Session["DT_TreeView"] = null;
            if (projectId != "" && projectId != "0")
            {
                dtprojectTask = objProjectTask.GetTaskTreeData(EmpId, projectId);
            }
            else
            {
                dtprojectTask = objProjectTask.GetTaskTreeData(EmpId);
            }
            if (Status != "All")
            {
                dtprojectTask = new DataView(dtprojectTask, "Status='" + Status.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            HttpContext.Current.Session["DT_TreeView"] = dtprojectTask;
        }

        return dtprojectTask;
    }
    protected void txtSearchprojectName_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchprojectName.Text == "")
        {
            hdnProjectId.Value = "";
            return;
        }
        hdnProjectId.Value = objProjctMaster.GetProjectIdByName(txtSearchprojectName.Text);
        if (hdnProjectId.Value == "")
        {
            DisplayMessage("Please Select from suggestions");
            txtSearchprojectName.Text = "";
            txtSearchprojectName.Focus();
            return;
        }
        string task = "";
        if (Request.QueryString["Task"] != null)
        {
            task = Request.QueryString["Task"].ToString();
        }
        DataTable dt = new DataTable();
        if (lblHeader.Text != "Project Bugs")
        {
            dt = FillTaskList(hdnProjectId.Value, ddlStatus.SelectedValue, task);
            if (hdnTreeNGrid.Value == "2")
            {
                TaskTreeList.DataSource = dt;
                TaskTreeList.DataBind();
            }
            else
            {
                gvTaskData.DataSource = dt;
                gvTaskData.DataBind();
            }
        }
        else
        {
            dt = fill_BugGrid_ForProjectId(hdnProjectId.Value);
            GvrProjecttask.DataSource = dt;
            GvrProjecttask.DataBind();
            Session["dtFilter_ProjectTask"] = dt;
            txtProjectName.Text = txtSearchprojectName.Text;
            ddlprojectname_SelectedIndexChanged(null, null);
        }
        lblTotalRecords.Text = "Total Records: "+dt.Rows.Count.ToString();
        dt = null;
    }
    protected void imgBtnTreeNGrid_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        LinkButton ib = (LinkButton)sender;
        if (ib.ToolTip == "Grid View")
        {
            hdnTreeNGrid.Value = "2";
            ib.ToolTip = "Tree View";
            ib.CssClass = "fa fa-sitemap";
            gvTaskData.DataSource = null;
            gvTaskData.DataBind();
            gvTaskData.Visible = false;
            TaskTreeList.Visible = true;
            div_printReport.Visible = true;
            div_assignTask.Visible = true;
        }
        else
        {
            hdnTreeNGrid.Value = "1";
            ib.ToolTip = "Grid View";
            ib.CssClass = "fa fa-list";
            gvTaskData.Visible = true;
            TaskTreeList.Visible = false;
            div_printReport.Visible = false;
            div_assignTask.Visible = false;
        }

        fillGridNTreeData();

        div_printReport.Style.Add("display", "none");
        div_assignTask.Style.Add("display", "none");

        if (hdnCanPrint.Value.ToLower() == "true")
        {
            div_printReport.Style.Add("display", "block");
        }
        if (hdnCanAssignTask.Value.ToLower() == "true")
        {
            div_assignTask.Style.Add("display", "block");
        }
    }
    protected void imgBtnAddChild_Command(object sender, CommandEventArgs e)
    {
        resetToAddChild();
        if (Request.QueryString["Task"] != null)
        {
            DisplayMessage("Bug can not be allowed as associated task");
            return;
        }
        DataTable dt = Session["DT_TreeView"] as DataTable;
        dt = new DataView(dt, "Task_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //objProjectTask.GetRecordByTaskId(key[1].ToString());
        if (dt.Rows.Count > 0)
        {
            if (Request.QueryString["Task"] == null)
            {
                if (dt.Rows[0]["Status"].ToString() == "Closed")
                {
                    DisplayMessage("Closed task can not be allowed as associated or parent task");
                    return;
                }
            }
            //ddlprojectname.SelectedIndex = ddlSearchprojectName.SelectedIndex;
            txtProjectName.Text = txtSearchprojectName.Text;
            //ddlprojectname_SelectedIndexChanged(null, null);

            // fillDetails is replacement of ddlprojectname_SelectedIndexChanged
            fillDetails();
            hdnTaskid.Value = "";
            try
            {
                ddlAssigningTasktype.SelectedValue = dt.Rows[0]["Field6"].ToString();
            }
            catch
            {
                ddlAssigningTasktype.SelectedIndex = 0;
            }
            hdnProjId.Value = dt.Rows[0]["project_Id"].ToString();
            ddlTaskType.SelectedValue = dt.Rows[0]["Task_Type"].ToString();
            ddlTaskType_SelectedIndexChanged(null, null);
            if (ddlParentTask.Items.FindByValue(dt.Rows[0]["Task_Id"].ToString()) != null)
            {
                //ddlParentTask.Value = dt.Rows[0]["Task_Id"].ToString();
                ddlParentTask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
            }
            else
            {
                ddlParentTask.SelectedIndex = 0;
            }
        }
        dt.Dispose();
        hdnTaskid.Value = "";
        txtsubject.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    public void resetToAddChild()
    {
        HidCustId.Value = "";
        hdnTaskid.Value = "";
        hdnProjId.Value = "";
        hdnfileidupdate.Value = "";
        hdnfileid.Value = "";
        chkCancel.Checked = false;
        div_extended.Style.Add("display", "none");
        div_assign.Attributes.Add("Class", "col-md-4");
        div_end.Attributes.Add("Class", "col-md-4");
        txtassigndate.Text = "";
        txtempenddate.Text = "";
        txtRequiredHrs.Text = "";
        txtAssignBy.Text = "";
        txttlenddate.Text = "";
        txttlendtime.Text = "";
        txtsubject.Text = "";
        Editor1.Text = "";
        txtExpectedCost.Text = "";
        txtExtendedId.Text = "";
        txtDuration.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlTaskType.SelectedIndex = 0;
        ddlTaskType_SelectedIndexChanged(null, null);
        txtSiteAddress.Text = "";
        ChkContactPerson.Items.Clear();
        trParentttaskdetail.Visible = false;
        tlEnddate.Visible = false;
    }
    protected void imgBtnRenewal_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objProjectTask.GetRecordByTaskId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Assign_Date"].ToString().Trim() == "1/1/1900 12:00:00 AM" || dt.Rows[0]["Emp_Close_Date"].ToString().Trim() == "1/1/1900 12:00:00 AM")
            {
                DisplayMessage("Cant Extend Because Assign Date and Expected Date are not present");
                return;
            }
            if (dt.Rows[0]["Status"].ToString() != "Assigned")
            {
                DisplayMessage("Cant Extend, Because The Task Status Already Changed To " + dt.Rows[0]["Status"].ToString());
                return;
            }
            hdnProjId.Value = dt.Rows[0]["Project_Id"].ToString();
            try
            {
                ddlAssigningTasktype.SelectedValue = dt.Rows[0]["Field6"].ToString();
            }
            catch
            {
                ddlAssigningTasktype.SelectedIndex = 0;
            }
            try
            {
                txtProjectName.Text = dt.Rows[0]["Project_Name"].ToString();
                hdnProjectId.Value = dt.Rows[0]["Project_Id"].ToString();
                //ddlprojectname.Value = dt.Rows[0]["Project_Id"].ToString();
            }
            catch
            {

            }
            fillDetails();

            DataTable dttaskemp = objTaskEmp.GetRecordBy_RefType_and_RefId("Task", e.CommandArgument.ToString());
            foreach (ListItem li in listtaskEmployee.Items)
            {
                DataTable empData = new DataView(dttaskemp, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (empData.Rows.Count > 0)
                {
                    li.Selected = true;
                }
                empData = null;
            }
            dttaskemp.Dispose();
            txtExtendedId.Text = e.CommandArgument.ToString();
            txtsubject.Text = dt.Rows[0]["Subject"].ToString();
            Editor1.Text = dt.Rows[0]["Description"].ToString();
            txtassigndate.Text = Formatdate(dt.Rows[0]["Assign_Date"].ToString());
            //txtassigntime.Text = FormatTime(dt.Rows[0]["Assign_Time"].ToString());
            txtempenddate.Text = Formatdate(dt.Rows[0]["Emp_Close_Date"].ToString());
            //txtempendtime.Text = FormatTime(dt.Rows[0]["Emp_Close_Time"].ToString());
            txtRequiredHrs.Text = dt.Rows[0]["RequiredHours"].ToString();
            txtAssignBy.Text = dt.Rows[0]["assignbyname"].ToString() + "/" + dt.Rows[0]["assignby"].ToString();
            txtExtendedDate.Text = "";
            txtExtendedTime.Text = "";
            div_extended.Style.Add("display", "block");
            div_assign.Attributes.Add("Class", "col-md-4");
            div_end.Attributes.Add("Class", "col-md-4");
            txttlenddate.Text = Formatdate(dt.Rows[0]["TL_Close_Date"].ToString());
            txttlendtime.Text = FormatTime(dt.Rows[0]["TL_Close_Time"].ToString());
            txtsubject.Text = dt.Rows[0]["Subject"].ToString();
            Editor1.Text = dt.Rows[0]["Description"].ToString();
            HidCustId.Value = dt.Rows[0]["Emp_Id"].ToString();
            hdnfileidupdate.Value = dt.Rows[0]["File_Id"].ToString();
            if (ddlParentTask.Items.FindByValue(dt.Rows[0]["Task_Id"].ToString()) != null)
            {
                //ddlParentTask.Value = dt.Rows[0]["Task_Id"].ToString();
                ddlParentTask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
            }
            else
            {
                ddlParentTask.SelectedIndex = 0;
            }
            if (dt.Rows[0]["Expected_Cost"].ToString() != "")
            {
                txtExpectedCost.Text = Common.GetAmountDecimal(dt.Rows[0]["Expected_Cost"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            }
            txtDuration.Text = dt.Rows[0]["Field1"].ToString();
            Editor1.Enabled = true;
            Lbl_Tab_New.Text = "Extended";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            dt.Dispose();
        }
    }
    protected void gvTaskData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTaskData.PageIndex = e.NewPageIndex;
        fillGridNTreeData();
    }
    protected void btnPrint_Command(object sender, CommandEventArgs e)
    {
        if (hdnProjectId.Value.ToString() == "" && txtSearchprojectName.Text == "")
        {
            DisplayMessage("Select project");
            txtSearchprojectName.Text = "";
            txtSearchprojectName.Focus();
            return;
        }
        ViewState["dt_TaskContractReport"] = null;
        lblSelectRecord.Text = "";
        hdnRPTClick.Value = "1";

        try
        {
            lblSelectRecord.Text = e.CommandArgument.ToString() + ",";
        }
        catch
        {
        }
        try
        {
            if (hdnRPTClick.Value == "1")
            {
                GetReportStatement();
            }
        }
        catch (Exception er)
        {
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OpenTaskContractReportPopup()", true);
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        fillGridNTreeData();
    }
    public void fillGridNTreeData()
    {
        DataTable dt = new DataTable();
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
        if (lblHeader.Text == "Project Bugs")
        {
            
            dt = Session["dtFilter_ProjectTask"] as DataTable;
            if (ddlStatus.SelectedValue != "All")
            {
                dt = new DataView(dt, "Status='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            dt = new DataView(dt, strSearchCondition, "", DataViewRowState.CurrentRows).ToTable();
            GvrProjecttask.DataSource = dt;
            GvrProjecttask.DataBind();
        }
        else if (hdnProjectId.Value.ToString() != "")
        {
            dt = Session["DT_TreeView"] as DataTable;            
            dt = new DataView(dt, strSearchCondition, "", DataViewRowState.CurrentRows).ToTable();
            if (ddlStatus.SelectedValue != "All")
            {
                dt = new DataView(dt, "Status='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (hdnTreeNGrid.Value == "1")
            {
                gvTaskData.DataSource = dt;
                gvTaskData.DataBind();
            }
            else
            {
                TaskTreeList.DataSource = dt;
                TaskTreeList.DataBind();
            }
        }
        lblTotalRecords.Text = "Total Records: "+dt.Rows.Count.ToString();
        dt = null;
    }
    protected void gvTaskData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["DT_TreeView"];
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
        Session["DT_TreeView"] = dt;
        dt.Dispose();
        fillGridNTreeData();
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        fillGridNTreeData();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        fillGridNTreeData();
    }
}