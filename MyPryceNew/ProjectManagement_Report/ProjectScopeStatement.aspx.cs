using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Dynamic;
using DevExpress.Web;
using DevExpress.Web.ASPxTreeList;
public partial class ProjectManagement_Report_ProjectScopeStatement : System.Web.UI.Page
{
    Common cmn = null;
    Prj_ProjectHistory RptShift = null;
    Prj_ProjectMaster objProj = null;
    Prj_ProjectTeam objProjectTeam = null;
    SystemParameter ObjSysParam = null;
    Prj_ProjectTask objProjTask = null;
    UserMaster objUser = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    Prj_ProjectScopeStatement objProjectStatement = null;
    ProjectProgressReport objProjectProgress = null;
    Prj_TaskContractReport objTaskcontract = null;
    Prj_Project_TaskType ObjTaskType = null;
    EmployeeMaster ObjEmployeeMaster = null;
    Prj_Project_Task_Employeee objTaskEmp = null;
    PageControlCommon objPageCmn = null;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        RptShift = new Prj_ProjectHistory(Session["DBConnection"].ToString());
        objProj = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        objProjectTeam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objProjTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objProjectStatement = new Prj_ProjectScopeStatement(Session["DBConnection"].ToString());
        objProjectProgress = new ProjectProgressReport(Session["DBConnection"].ToString());
        objTaskcontract = new Prj_TaskContractReport(Session["DBConnection"].ToString());
        ObjTaskType = new Prj_Project_TaskType(Session["DBConnection"].ToString());
        ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        txtFromDate.Attributes.Add("readonly", "true");
        txtToDate.Attributes.Add("readonly", "true");
        ScriptManager sm = (ScriptManager)Master.FindControl("SM1");
        sm.RegisterPostBackControl(btnprojectStatement);
        StrUserId = Session["UserId"].ToString();

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "353", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            pnlReport.Visible = false;
            //btnreset.Visible = false;
            // btnsave.Visible = false;
            FillTaskType();
            treeList.ExpandAll();
        }
        FillddlProjectname();
        ObjSysParam.GetSysTitle();
        GetReportStatement();
        FillTreeList();
        AllPageCode();
        countLiteral.Text = treeList.SelectionCount.ToString();
    }
    #region TreeListbindingandselection
    protected void btngetRecord_Click(object sender, EventArgs e)
    {
        trreportAction.Visible = false;
        if (ddlprojectname.SelectedIndex <= 0)
        {
            DisplayMessage("Select project Name");
            ddlprojectname.Focus();
        }
        FillTreeList();
    }
    protected void treeList_CustomDataCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomDataCallbackEventArgs e)
    {
        e.Result = treeList.SelectionCount.ToString();
    }
    protected void cmbMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbMode.SelectedIndex > 0)
            treeList.UnselectAll();
        SetNodeSelectionSettings();
    }
    protected void treeList_DataBound(object sender, EventArgs e)
    {
        SetNodeSelectionSettings();
    }
    void SetNodeSelectionSettings()
    {
        TreeListNodeIterator iterator = treeList.CreateNodeIterator();
        TreeListNode node;
        while (true)
        {
            node = iterator.GetNext();
            if (node == null) break;
            switch (cmbMode.SelectedIndex)
            {
                case 1:
                    node.AllowSelect = !node.HasChildren;
                    break;
                case 2:
                    node.AllowSelect = node.HasChildren;
                    break;
                case 3:
                    node.AllowSelect = node.Level > 2;
                    break;
            }
        }
    }
    public void FillTreeList()
    {
        DataTable dt = new DataTable();
        lblSelectRecord.Text = "";
        Session["dtFilter_Project_SS"] = null;
        treeList.DataSource = null;
        treeList.DataBind();
        //ChkselectAll.Visible = false;
        //ChkselectAll.Checked = false;
        string strTaskTypeId = "0";
        if (ddlprojectname.SelectedIndex > 0)
        {
            string employeeList = "";
            foreach (ListItem li in listtaskEmployee.Items)
            {
                if (li.Selected)
                {
                    employeeList += li.Value + ",";
                }
            }
            if (employeeList != "")
            {
                int Last_pos = employeeList.LastIndexOf(",");
                employeeList = employeeList.Substring(0, Last_pos);
            }
            else
            {
                employeeList = "0";
            }
            dt = objProjTask.GetDataProjectId(ddlprojectname.Value.ToString(), employeeList);
            if (ddlTaskType.SelectedIndex > 0)
            {
                dt = new DataView(dt, "Task_Type='" + ddlTaskType.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                dt = new DataView(dt, "Assign_Date>='" + txtFromDate.Text.Trim() + "' and Emp_Close_Date <='" + txtToDate.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (ddlStatus.SelectedIndex != 0)
            {
                dt = new DataView(dt, "Status ='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            for (int i = 0; i < lstLocationSelect.Items.Count; i++)
            {
                if (strTaskTypeId == "")
                {
                    strTaskTypeId = lstLocationSelect.Items[i].Value;
                }
                else
                {
                    strTaskTypeId = strTaskTypeId + "," + lstLocationSelect.Items[i].Value;
                }
            }
            if (strTaskTypeId != "0")
            {
                dt = new DataView(dt, "IsBugs in (" + strTaskTypeId + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            dt = new DataView(dt, "", "Task_id", DataViewRowState.CurrentRows).ToTable();
            //  dt = GetFinalTable(dt);
            Session["dtFilter_Project_SS"] = dt;
            treeList.DataSource = dt;
            treeList.DataBind();
            if (dt.Rows.Count > 0)
            {
                trreportAction.Visible = true;
            }

        }
        dt.Dispose();
    }
    #endregion
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        //357
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("353", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        dtModule.Dispose();
    }
    protected void ddlprojectname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void GetReportStatement()
    {
        try
        {
            if (ddlprojectname.SelectedIndex <= 0)
            {
                return;
            }
        }
        catch
        {
        }
        if (Session["ReportType"] == null)
        {
            return;
        }
        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Prj_ProjectHistory_ReportTableAdapter();
        adp.Fill(rptdata.sp_Prj_ProjectHistory_Report, int.Parse(ddlprojectname.SelectedItem.Value.ToString()), 1);
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
        DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
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
        if (Session["ReportType"].ToString() == "0")
        {
            objProjectStatement.setreportCode(strreportCode);
            objProjectStatement.SetImage(Imageurl);
            objProjectStatement.setTitleName("Project Scope Statement");
            objProjectStatement.setcompanyname(CompanyName);
            objProjectStatement.setaddress(CompanyAddress);
            objProjectStatement.DataSource = dtFinalTable;
            objProjectStatement.DataMember = "sp_Prj_ProjectHistory_Report";
            rptViewer.Report = objProjectStatement;
            rptToolBar.ReportViewer = rptViewer;
        }
        if (Session["ReportType"].ToString() == "1")
        {
            objProjectProgress.setreportCode(strreportCode);
            objProjectProgress.SetImage(Imageurl);
            objProjectProgress.setTitleName("Task Check List");
            objProjectProgress.setcompanyname(CompanyName);
            objProjectProgress.setaddress(CompanyAddress);
            objProjectProgress.DataSource = dtFinalTable;
            objProjectProgress.DataMember = "sp_Prj_ProjectHistory_Report";
            rptViewer.Report = objProjectProgress;
            rptToolBar.ReportViewer = rptViewer;
        }
        if (Session["ReportType"].ToString() == "2")
        {
            objTaskcontract.DataSource = dtFinalTable;
            objTaskcontract.DataMember = "sp_Prj_ProjectHistory_Report";
            rptViewer.Report = objTaskcontract;
            rptToolBar.ReportViewer = rptViewer;
        }
        dtFilter.Dispose();
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
    public string addSerial(string strlevel, string strSNo, string strSubject)
    {
        string strSerial = string.Empty;
        strSerial = strlevel + "." + strSNo + " " + strSubject;
        return strSerial;
    }
    protected void lnkback_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
        pnlProjectfilter.Visible = true;
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvrProjecttask.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gvHeaderrow in GvrProjecttask.Rows)
        {
            ((CheckBox)gvHeaderrow.FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectRecord.Text.Split(',').Contains(((Label)gvHeaderrow.FindControl("lblProjectId")).Text))
                {
                    lblSelectRecord.Text += ((Label)gvHeaderrow.FindControl("lblProjectId")).Text.Trim().ToString() + ",";
                }
            }
        }
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvrProjecttask.Rows[index].FindControl("lblProjectId");
        if (((CheckBox)GvrProjecttask.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectRecord.Text += empidlist;
            string[] split = lblSelectRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectRecord.Text = temp;
        }
        GridView gvchildGrid = (GridView)gvRow.FindControl("gvchildGrid");
        foreach (GridViewRow gvchildrow in gvchildGrid.Rows)
        {
            ((CheckBox)gvchildrow.FindControl("chkgvSelect")).Checked = ((CheckBox)GvrProjecttask.Rows[index].FindControl("chkgvSelect")).Checked;
            if (((CheckBox)GvrProjecttask.Rows[index].FindControl("chkgvSelect")).Checked)
            {
                if (!lblSelectRecord.Text.Split(',').Contains(((Label)gvchildrow.FindControl("lblProjectId")).Text))
                {
                    lblSelectRecord.Text += ((Label)gvchildrow.FindControl("lblProjectId")).Text.Trim().ToString() + ",";
                }
            }
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (ddlprojectname.SelectedIndex <= 0)
        {
            DisplayMessage("Select project ");
            ddlprojectname.Focus();
            return;
        }
        bool AllSameEmp = true;
        lblSelectRecord.Text = "";
        string data1 = "", data2 = "";
        for (int i = 0; i < treeList.SelectionCount; i++)
        {
            lblSelectRecord.Text += treeList.GetSelectedNodes().ToList()[i].Key.ToString() + ",";
            if (i == 0)
            {
                data1 = objProjectTeam.GetEmpIdListByTrnsID(treeList.GetSelectedNodes().ToList()[i].Key.ToString());
            }
            else
            {
                data2 = objProjectTeam.GetEmpIdListByTrnsID(treeList.GetSelectedNodes().ToList()[i].Key.ToString());
            }
            if (data1 != data2 && data2 != "")
            {
                AllSameEmp = false;
            }
        }
        if (lblSelectRecord.Text == "")
        {
            DisplayMessage("Please select task");
            return;
        }
        //here we are using session for check that user clicked for scope statement or task check list
        //GetRequiredHour(DateTime.Now.ToString(), DateTime.Now.ToString("HH:mm"), DateTime.Now.AddDays(2).ToString(), DateTime.Now.AddDays(2).ToString("HH:mm"));
        if (((Button)sender).ID == "btnprojectStatement")
        {
            Session["ReportType"] = "0";
        }
        if (((Button)sender).ID == "btnProjectProgrss")
        {
            Session["ReportType"] = "1";
        }
        if (((Button)sender).ID == "btnPrtTask")
        {
            if (!AllSameEmp)
            {
                DisplayMessage("Selected Task Contains Different Employees");
                return;
            }
            Session["ReportType"] = "2";
        }
        pnlProjectfilter.Visible = false;
        pnlReport.Visible = true;
        GetReportStatement();
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        ddlprojectname.SelectedIndex = 0;
        lblSelectRecord.Text = "";
        ddlTaskType.SelectedIndex = 0;
        objPageCmn.FillData((object)GvrProjecttask, null, "", "");
        ChkselectAll.Visible = false;
        ChkselectAll.Checked = false;
        trreportAction.Visible = false;
        AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
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
            dt.Dispose();
        }
        return str;
    }
    protected void txtprojecttype_TextChanged(object sender, EventArgs e)
    {
        FillddlProjectname();
    }
    public void FillddlProjectname()
    {
        DataTable dtProject = new DataTable();
        dtProject = objProj.GetAllRecordProjectMasteer();
        string sttrValue = string.Empty;
        try
        {
            sttrValue = ddlprojectname.Value.ToString();
        }
        catch
        {
        }
        try
        {
            if (txtprojecttype.Text.Trim() != "")
            {
                dtProject = new DataView(dtProject, "Field4='" + Session["CompId"].ToString() + "' and Field5='" + Session["BrandId"].ToString() + "' and Project_Type='" + txtprojecttype.Text.Trim() + "'", "Project_Name", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtProject = new DataView(dtProject, "Field4='" + Session["CompId"].ToString() + "' and Field5='" + Session["BrandId"].ToString() + "'", "Project_Name", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
        }
        ListEditItem Li1 = new ListEditItem();
        Li1.Text = "---select---";
        if (dtProject.Rows.Count > 0)
        {
            ddlprojectname.DataSource = dtProject;
            ddlprojectname.DataBind();
            ddlprojectname.Items.Insert(0, Li1);
            ddlprojectname.SelectedIndex = 0;
        }
        else
        {
            ddlprojectname.Items.Insert(0, Li1);
        }
        ddlprojectname.Value = sttrValue;
        dtProject.Dispose();
    }
    protected void ChkselectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        if (GvrProjecttask.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            ChkselectAll.Checked = false;
            return;
        }
        lblSelectRecord.Text = "";
        DataTable dt = objProjTask.GetDataProjectId(ddlprojectname.SelectedItem.Value.ToString());
        if (ddlTaskType.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Task_Type='" + ddlTaskType.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ChkselectAll.Checked)
        {
            foreach (DataRow dr in dt.Rows)
            {
                lblSelectRecord.Text += dr["task_Id"].ToString() + ",";
            }
            ((CheckBox)GvrProjecttask.HeaderRow.FindControl("chkgvSelectAll")).Checked = true;
            chkgvSelectAll_CheckedChanged(null, null);
        }
        else
        {
            ((CheckBox)GvrProjecttask.HeaderRow.FindControl("chkgvSelectAll")).Checked = false;
            chkgvSelectAll_CheckedChanged(null, null);
        }
    }
    public string GetRequiredHour(string strAssingdate, string strAssingTime, string strExpectedDate, string strExpecttIme)
    {
        string strRequiredHours = string.Empty;
        DateTime assigndate = Convert.ToDateTime(strAssingdate);
        DateTime assignTime = Convert.ToDateTime(strAssingTime);
        DateTime Expecteddate = Convert.ToDateTime(strExpectedDate);
        DateTime ExpectedTime = Convert.ToDateTime(strExpecttIme);
        assigndate = new DateTime(assigndate.Year, assigndate.Month, assigndate.Day, assignTime.Hour, assignTime.Minute, assignTime.Second);
        Expecteddate = new DateTime(Expecteddate.Year, Expecteddate.Month, Expecteddate.Day, ExpectedTime.Hour, ExpectedTime.Minute, ExpectedTime.Second);
        while (assigndate < Expecteddate)
        {
            GetMinuteDiff(assigndate.ToString("HH:mm"), Expecteddate.ToString("HH:mm"));
            assigndate.AddDays(1);
        }
        return strRequiredHours;
    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {
        lesstime = "10:00:00";
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
    public string GetAssignBy(string strUserId)
    {
        string strUsername = string.Empty;
        DataTable dt = objUser.GetUserMasterByUserId(strUserId, "");
        if (dt.Rows.Count > 0)
        {
            strUsername = dt.Rows[0]["EmpName"].ToString();
        }
        dt.Dispose();
        return strUsername;
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
        if (Date.ToString() != "" && Convert.ToDateTime(Date.ToString()).ToString("dd/MM/yyyy") != "01/01/1900")
        {
            strDate = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        return strDate;
    }
    protected void GvrProjecttask_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AllPageCode();
        GvrProjecttask.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Project_SS"];
        GvrProjecttask.DataSource = dt;
        GvrProjecttask.DataBind();
        foreach (GridViewRow gvHeaderrow in GvrProjecttask.Rows)
        {
            if (lblSelectRecord.Text.Split(',').Contains(((Label)gvHeaderrow.FindControl("lblProjectId")).Text))
            {
                ((CheckBox)gvHeaderrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvHeaderrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        dt.Dispose();
    }
    protected void GvrProjecttask_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Project_SS"];
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
        Session["dtFilter_Project_SS"] = dt;
        GvrProjecttask.DataSource = dt;
        GvrProjecttask.DataBind();
        dt.Dispose();
    }
    protected void chkselecttask_OnCheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        GridView Childgrid = (GridView)(gvrow.Parent.Parent);
        GridViewRow Gv1Row = (GridViewRow)(Childgrid.NamingContainer);
        string empidlist = string.Empty;
        string temp = string.Empty;
        Label lb = (Label)gvrow.FindControl("lblProjectId");
        if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked)
        {
            ((CheckBox)GvrProjecttask.Rows[Gv1Row.RowIndex].FindControl("chkgvSelect")).Checked = true;
            empidlist += ((Label)GvrProjecttask.Rows[Gv1Row.RowIndex].FindControl("lblProjectId")).Text + ",";
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectRecord.Text += empidlist;
            string[] split = lblSelectRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectRecord.Text = temp;
        }
    }
    protected void imgViewDetail_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        if (((ImageButton)gvrow.FindControl("imgBtnaddtax")).ImageUrl == "~/Images/plus.png")
        {
            ((GridView)gvrow.FindControl("gvchildGrid")).Visible = true;
            ((ImageButton)gvrow.FindControl("imgBtnaddtax")).ImageUrl = "~/Images/minus.png";
        }
        else
        {
            ((GridView)gvrow.FindControl("gvchildGrid")).Visible = false;
            ((ImageButton)gvrow.FindControl("imgBtnaddtax")).ImageUrl = "~/Images/plus.png";
        }
    }
    #region LocationWork
    public void FillTaskType()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();
        DataTable dtLoc = ObjTaskType.GetAllTrueRecord();
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)lstLocation, dtLoc, "TaskType_Name", "Trans_Id");
    }
    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();
    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    #endregion
    protected void ddlprojectname_SelectedIndexChanged(object sender, EventArgs e)
    {
        listtaskEmployee.Items.Clear();
        listtaskEmployee.DataSource = null;
        listtaskEmployee.DataBind();
       
        DataTable dt = new DataTable();
        dt = objProjectTeam.GetRecordByProjectId("", ddlprojectname.Value.ToString());
        // dt = new DataView(dt, "Project_Id=" + ddlprojectname.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        DataView dvProjectName = new DataView(dt);
        dvProjectName.Sort = "Emp_Name";
        listtaskEmployee.DataSource = dvProjectName;
        listtaskEmployee.DataTextField = "EmpName_Designation";
        listtaskEmployee.DataValueField = "Emp_Id";
        listtaskEmployee.DataBind();
       
    }

}