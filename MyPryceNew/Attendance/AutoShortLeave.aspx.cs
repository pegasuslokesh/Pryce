using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Configuration;

public partial class Attendance_AutoShortLeave : BasePage
{
    Att_ShiftDescription objShiftdesc = null;
    LocationMaster ObjLocationMaster = null;
    DepartmentMaster objDep = null;
    Common cmn = null;
    Att_PartialLeave_Request objPartial = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    Att_ShiftManagement objShift = null;
    DataAccessClass objda = null;
    Att_ScheduleMaster objEmpSch = null;
    Set_ApplicationParameter objAppParam = null;
    LogProcess objLogProcess = null;
    Att_AttendanceLog objAttLog = null;
    PageControlCommon objPageCmn = null;
    static string Depart;
    static string locationids;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objShiftdesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objLogProcess = new LogProcess(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/AutoShortLeave.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            fillLocation();
            TreeViewDepartment.Attributes.Add("onclick", "OnTreeClick(event)");
            BindTreeView();
            GetEmpCodeRange();
            CalenderFromdate.Format = Session["DateFormat"].ToString();
            CalenderTodate.Format = Session["DateFormat"].ToString();
            FillGrade();

        }
        ScriptManager.RegisterStartupScript(this, GetType(), "key", "CacheItems()", true);
       // ScriptManager.RegisterStartupScript(this, GetType(), "", "CacheItems()", true);
    }
    public void FillGrade()
    {
        DataTable dt = objda.return_DataTable("select rtrim( ltrim(Grade_Name)) as Grade_Name  from set_grademaster");
        ddlGradeFrom.DataSource = dt;
        ddlGradeFrom.DataTextField = "Grade_Name";
        ddlGradeFrom.DataValueField = "Grade_Name";
        ddlGradeFrom.DataBind();
        ddlGradeFrom.Items.Insert(0, "--Select--");
        ddlGradeTo.DataSource = dt;
        ddlGradeTo.DataTextField = "Grade_Name";
        ddlGradeTo.DataValueField = "Grade_Name";
        ddlGradeTo.DataBind();
        ddlGradeTo.Items.Insert(0, "--Select--");
    }
    public void FillEmpCodeList(DropDownList ddlEmocodeList, DataTable dt)
    {
        ddlEmocodeList.Items.Clear();
        ddlEmocodeList.DataSource = dt;
        ddlEmocodeList.DataTextField = "Emp_Code";
        ddlEmocodeList.DataValueField = "Emp_Id";
        ddlEmocodeList.DataBind();
    }
    public DataTable GetEmployeeFilteredRecord()
    {

        Session["deptFilter"] = null;

        foreach (TreeNode ModuleNode in TreeViewDepartment.Nodes)
        {
            if (ModuleNode.Checked)
            {
                Session["deptFilter"] += ModuleNode.Value + ",";
                //objLocDept.InsertLocationDepartmentMaster(editid.Value, ModuleNode.Value, "0", "", "", "", "", "", "", "", "", "True", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), "True", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString());

                childNodeSave(ModuleNode);
            }
        }

        string strDepId = string.Empty;
        string strLocationId = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        string strLocationIDs = string.Empty;
        if (Session["EmpId"].ToString() == "0")
        {

            if (ddlLocation.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (Session["deptFilter"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id  in (" + Session["deptFilter"].ToString().Substring(0, Session["deptFilter"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else
        {
            if ((ddlLocation.SelectedIndex == 0 && Session["deptFilter"] == null) || (ddlLocation.SelectedIndex == 0 && Session["deptFilter"] != null))
            {
                strLocationIDs = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["EmpId"].ToString());
                strLocationIDs = strLocationIDs.Substring(0, strLocationIDs.Length - 1);
                //----------Code to get location's Department-------------------------
                string strWhereClause = string.Empty;
                string strSql = "Select record_id,Field1 as location_id  From Set_UserDataPermission   where    Set_UserDataPermission.Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and  Set_UserDataPermission.User_Id ='" + HttpContext.Current.Session["UserId"].ToString() + "' and Set_UserDataPermission.IsActive='True' and Record_Type='D'";
                DataTable dtDepartment = objda.return_DataTable(strSql);
                if (Session["deptFilter"] != null)
                {
                    dtDepartment = new DataView(dtDepartment, "record_id in (" + Session["deptFilter"].ToString().Substring(0, Session["deptFilter"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                string[] LocationArray = strLocationIDs.Split(',');
                string StrDepIDs = string.Empty;
                foreach (string strLoc in LocationArray)
                {
                    StrDepIDs = "";
                    DataTable dtDepartmentTemp = new DataView(dtDepartment, "location_id='" + strLoc + "'", "", DataViewRowState.CurrentRows).ToTable();
                    for (int i = 0; i < dtDepartmentTemp.Rows.Count; i++)
                    {
                        StrDepIDs += dtDepartmentTemp.Rows[i]["record_id"].ToString() + ",";
                    }
                    if (StrDepIDs != "")
                    {
                        StrDepIDs = StrDepIDs.Substring(0, StrDepIDs.Length - 1);
                        if (strWhereClause == string.Empty)
                        {
                            strWhereClause = "(Location_id='" + strLoc + "' and department_id in (" + StrDepIDs + "))";
                        }
                        else
                        {
                            strWhereClause = strWhereClause + " or " + "(Location_id='" + strLoc + "' and department_id in (" + StrDepIDs + "))";
                        }
                    }
                }
                if (strWhereClause != string.Empty)
                {
                    dtEmp = new DataView(dtEmp, strWhereClause, "", DataViewRowState.CurrentRows).ToTable();
                }
                //-------------end------------------------------------
            }
            //else if (ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex > 0)
            //{
            //}
            else if (ddlLocation.SelectedIndex > 0 && Session["deptFilter"] == null)
            {
                strDepId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.SelectedValue, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDepId == "")
                {
                    strDepId = "0,";
                }
                dtEmp = new DataView(dtEmp, "Location_id=" + ddlLocation.SelectedValue + " and Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocation.SelectedIndex > 0 && Session["deptFilter"] != null)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocation.SelectedValue + " and Department_id in (" + Session["deptFilter"].ToString().Substring(0, Session["deptFilter"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }



        return dtEmp;
    }
    public void childNodeSave(TreeNode ModuleNode)
    {
        string strDepId = string.Empty;


        foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
        {
            if (ObjNode.Checked)
            {
                Session["deptFilter"] += ObjNode.Value + ",";

                childNodeSave(ObjNode);
            }
        }

    }
    public void GetEmpCodeRange()
    {


        DataTable dt = GetEmployeeFilteredRecord();

        FillEmpCodeList(ddlcodefrom, dt);
        FillEmpCodeList(ddlcodeto, dt);

        try
        {
            ddlcodefrom.SelectedValue = new DataView(dt, "", "Emp_Code", DataViewRowState.CurrentRows).ToTable().Rows[0]["Emp_id"].ToString();
            ddlcodeto.SelectedValue = new DataView(dt, "", "Emp_Code desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Emp_id"].ToString();
        }
        catch
        {
        }

    }
    public void fillLocation()
    {
        ddlLocation.Items.Clear();
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        GetEmpCodeRange();
        BindTreeView();
    }
    protected void btnServiceShort_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        string strsql = string.Empty;

        if (txtFromdate.Text == "")
        {
            //if (DateTime.Now.Day > 15)
            //{
            //    DateTime tFrom = DateTime.Now;
            //    tFrom = new DateTime(tFrom.Year, tFrom.Month, 16);
            //    txtFromdate.Text = Convert.ToDateTime(tFrom).ToString("dd-MMM-yyyy");
            //    txtTodate.Text = Convert.ToDateTime(tFrom).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");
            //}
            //else
            //{
            //    DateTime tFrom = DateTime.Now;
            //    tFrom = new DateTime(tFrom.Year, tFrom.Month, 16).AddMonths(-1);
            //    txtFromdate.Text = Convert.ToDateTime(tFrom).ToString("dd-MMM-yyyy");
            //    txtTodate.Text = Convert.ToDateTime(tFrom).AddMonths(1).AddDays(-1).ToString("dd-MMM-yyyy");
            //}

            txtFromdate.Text = Convert.ToDateTime(DateTime.Now.AddDays(-10)).ToString("dd-MMM-yyyy");
            txtTodate.Text = Convert.ToDateTime(DateTime.Now.AddDays(-5)).ToString("dd-MMM-yyyy");
        }
       
       
        string strEmpIds = GetEmployeeList();

        //objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpIds, Session["UserId"].ToString(), "0", Convert.ToDateTime(txtFromdate.Text), Convert.ToDateTime(txtTodate.Text), Session["EmpId"].ToString(), null, HttpContext.Current.Session["TimeZoneId"].ToString(), ConfigurationManager.AppSettings["LeaveIntegration"].ToString(), ConfigurationManager.AppSettings["ErrorFile"].ToString(), ConfigurationManager.AppSettings["Shift_Range_Auto"].ToString());

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            GenerateShortLeave(strEmpIds, ref trns);
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnGetRecord_Click(null, null);
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        string strsql = string.Empty;

        if (txtFromdate.Text == "")
        {
            DisplayMessage("Enter From date");
            txtFromdate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtFromdate.Text);
            }
            catch
            {
                DisplayMessage("From date is invalid");
                return;
            }
        }


        if (txtTodate.Text == "")
        {
            DisplayMessage("Enter To date");
            txtTodate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtTodate.Text);
            }
            catch
            {
                DisplayMessage("To date is invalid");
                txtTodate.Focus();
                return;
            }
        }

        string strEmpIds = GetEmployeeList();

        objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpIds, Session["UserId"].ToString(), "0", Convert.ToDateTime(txtFromdate.Text), Convert.ToDateTime(txtTodate.Text), Session["EmpId"].ToString(), null, HttpContext.Current.Session["TimeZoneId"].ToString(), ConfigurationManager.AppSettings["LeaveIntegration"].ToString(), ConfigurationManager.AppSettings["ErrorFile"].ToString(), ConfigurationManager.AppSettings["Shift_Range_Auto"].ToString());

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            GenerateShortLeave(strEmpIds, ref trns);
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnGetRecord_Click(null, null);
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }
    }


    public void GenerateShortLeave(string strEmpList, ref SqlTransaction trns)
    {
        DateTime dtOndutyTime = new DateTime();
        DateTime dtOffdutyTime = new DateTime();
        DateTime dtinTime = new DateTime();
        DateTime dtoutTime = new DateTime();
        DataTable dtTemp = new DataTable();
        DataTable dtlog = new DataTable();
        DataTable dtlogtemp = new DataTable();
        DataTable dtRegister = objda.return_DataTable("select emp_id, att_date,timetable_id,onduty_time,OffDuty_Time,In_Time,out_time,LateMin,late_relaxation_min,earlymin,Early_Relaxation_Min from att_attendanceregister where emp_id in (" + strEmpList + ") and (Is_Leave=0 and is_absent=0 and is_holiday=0 and Is_Week_Off=0) and (cast( att_Date as date)>='" + Convert.ToDateTime(txtFromdate.Text).Date.ToString() + "' and cast( att_Date as date)<='" + Convert.ToDateTime(txtTodate.Text).Date.ToString() + "')", ref trns);
        DateTime dtFromdate = Convert.ToDateTime(txtFromdate.Text).Date;
        DateTime dttoDate = Convert.ToDateTime(txtTodate.Text).Date;
        foreach (string str in strEmpList.Split(','))
        {
            if (str == "0")
            {
                continue;
            }
            dtFromdate = Convert.ToDateTime(txtFromdate.Text).Date;
            dtlog = objAttLog.GetAttendanceLogByDateByEmpId1(str, dtFromdate.AddDays(-1).ToString(), dttoDate.AddDays(1).ToString(), ref trns);
            objda.execute_Command("delete from  Att_PartialLeave_Request  where emp_id =" + str + " and Field1='Auto' and (cast( Partial_Leave_Date as date)>='" + Convert.ToDateTime(txtFromdate.Text).Date.ToString() + "' and cast( Partial_Leave_Date as date)<='" + Convert.ToDateTime(txtTodate.Text).Date.ToString() + "') and Is_Confirmed=''", ref trns);

            while (dtFromdate <= dttoDate)
            {
                if(dtRegister.Rows.Count > 0)
                {
                    dtTemp = new DataView(dtRegister, "att_date='" + dtFromdate.ToString() + "' and timetable_id<>0 and emp_id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                




                for (int j = 0; j < dtTemp.Rows.Count; j++)
                {

                    dtinTime = Convert.ToDateTime(dtTemp.Rows[j]["In_Time"].ToString());
                    dtoutTime = Convert.ToDateTime(dtTemp.Rows[j]["out_time"].ToString());
                    dtOndutyTime = Convert.ToDateTime(dtTemp.Rows[j]["onduty_time"].ToString());
                    dtOffdutyTime = Convert.ToDateTime(dtTemp.Rows[j]["OffDuty_Time"].ToString());
                    if (dtOndutyTime < dtOffdutyTime)
                    {
                        dtOndutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, dtOndutyTime.Hour, dtOndutyTime.Minute, dtOndutyTime.Second);
                        dtOffdutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, dtOffdutyTime.Hour, dtOffdutyTime.Minute, dtOffdutyTime.Second);
                    }
                    else
                    {
                        dtOndutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, dtOndutyTime.Hour, dtOndutyTime.Minute, dtOndutyTime.Second);
                        dtOffdutyTime = new DateTime(dtFromdate.AddDays(1).Year, dtFromdate.AddDays(1).Month, dtFromdate.AddDays(1).Day, dtOffdutyTime.Hour, dtOffdutyTime.Minute, dtOffdutyTime.Second);
                    }
                    //generate short leave for late in

                    if (dtOndutyTime < dtinTime && dtTemp.Rows[j]["LateMin"].ToString().Trim() != "0")
                    {
                        objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), str, "0", DateTime.Now.ToString(), dtFromdate.ToString(), dtOndutyTime.ToString("HH:mm").ToString(), dtinTime.AddMinutes(-1).ToString("HH:mm").ToString(), "", "Auto generated", "", "Auto", "0", "", "", "Direct In", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }

                    //generate short leave for Early out

                    if (dtOffdutyTime > dtoutTime && dtTemp.Rows[j]["earlymin"].ToString().Trim() != "0")
                    {
                        objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), str, "0", DateTime.Now.ToString(), dtFromdate.ToString(), dtoutTime.AddMinutes(1).ToString("HH:mm").ToString(), dtOffdutyTime.ToString("HH:mm").ToString(), "", "Auto generated", "", "Auto", "0", "", "", "Direct Out", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

                //generating short leave which taken between duty time

                dtlogtemp = new DataView(dtlog, "event_date='" + dtFromdate.ToString() + "' and func_code='2'", "event_time", DataViewRowState.CurrentRows).ToTable();
                if (dtlogtemp.Rows.Count > 1)
                {
                    objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), str, "0", DateTime.Now.ToString(), dtFromdate.ToString(), Convert.ToDateTime(dtlogtemp.Rows[0]["Event_time"].ToString()).ToString("HH:mm").ToString(), Convert.ToDateTime(dtlogtemp.Rows[dtlogtemp.Rows.Count - 1]["Event_time"].ToString()).ToString("HH:mm").ToString(), "", "Auto generated", "", "Auto", "0", "", "", "In Between", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                dtFromdate = dtFromdate.AddDays(1);
            }
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
    }
    public string GetEmplist()
    {
        string strEmpids = string.Empty;

        //int mincode = Convert.ToInt32(ddlcodefrom.SelectedValue);
        //int maxcode = Convert.ToInt32(ddlcodeto.SelectedValue);

        int mincode = 0;
        int maxcode = 0;
        try
        {
            mincode = Convert.ToInt32(ddlcodefrom.SelectedItem.Text);
        }
        catch
        {

        }
        try
        {
            maxcode = Convert.ToInt32(ddlcodeto.SelectedItem.Text);
        }
        catch
        {

        }

        foreach (ListItem ddlitem in ddlcodeto.Items)
        {
            if (Convert.ToInt32(ddlitem.Text) >= mincode && Convert.ToInt32(ddlitem.Text) <= maxcode)
            {
                if (strEmpids == "")
                {
                    strEmpids = ddlitem.Value;
                }
                else
                {
                    strEmpids = strEmpids + "," + ddlitem.Value;
                }
            }
        }

        if (strEmpids == "")
        {
            strEmpids = "0";
        }

        return strEmpids;

    }
    protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
    {
        bool Result = ((CheckBox)gvOverTime.HeaderRow.FindControl("chkHeaderSelect")).Checked;

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Enabled)
            {
                ((CheckBox)gvrow.FindControl("chkSelect")).Checked = Result;
            }
        }
    }
    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmpCodeRange();
    }
    protected void btnfilterdepartment_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "show_modal()", true);
        //string url = "../Attendance/LogProcess.aspx";
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }
    #region BindDepartmentTreeview
    public int getMinute(string strOtmin)
    {
        int min = 0;

        string[] str = strOtmin.Split(':');

        min = Convert.ToInt32(str[0]) * 60 + Convert.ToInt32(str[1]);

        return min;
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
    public DataTable GetUserDepartment()
    {

        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();
        string strDeptvalue = string.Empty;
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;

            strDepId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            for (int j = 1; j < ddlLocation.Items.Count; j++)
            {
                strDeptvalue = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.Items[j].Value, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDeptvalue != "")
                {
                    strDepId += strDeptvalue;
                }
            }
        }


        dtDepartment = objDep.GetDepartmentMaster();



        if (strDepId == "")
        {
            strDepId = "0,";
        }

        //if (Session["EmpId"].ToString() != "0")
        //{
        dtDepartment = new DataView(dtDepartment, "Dep_Id in  (" + strDepId.Substring(0, strDepId.Length - 1) + ")", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable(true, "Dep_Name", "Dep_Id", "Parent_Id");
        //}



        return dtDepartment;
    }

    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {

        TreeViewDepartment.Nodes.Clear();

        DataTable dt = GetUserDepartment();

        string x = "Parent_Id=" + "0" + "";



        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, x, "Dep_Name asc", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count == 0)
        {
            dt = new DataView(dtTemp, "", "Dep_Name asc", DataViewRowState.CurrentRows).ToTable();
        }

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.ShowCheckBox = true;
            tn.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn.Value = dt.Rows[i]["Dep_Id"].ToString();

            TreeViewDepartment.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn);

            i++;
        }
        TreeViewDepartment.DataBind();
        TreeViewDepartment.CollapseAll();
    }


    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = objDep.GetAllDepartmentMaster_By_ParentId(index);


        dt = new DataView(dt, "", "Dep_Name asc", DataViewRowState.OriginalRows).ToTable();

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn1.Value = dt.Rows[i]["Dep_Id"].ToString();
            tn1.ShowCheckBox = true;
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn1);
            i++;
        }
        TreeViewDepartment.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetEmpCodeRange();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
    }

    #endregion
    protected void btnGetRecord_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string strsql = string.Empty;

        if (txtFromdate.Text == "")
        {
            DisplayMessage("Enter From date");
            txtFromdate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtFromdate.Text);
            }
            catch
            {
                DisplayMessage("From date is invalid");
                return;
            }
        }


        if (txtTodate.Text == "")
        {
            DisplayMessage("Enter To date");
            txtTodate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtTodate.Text);
            }
            catch
            {
                DisplayMessage("To date is invalid");
                txtTodate.Focus();
                return;
            }
        }

        string strEmpIds = GetEmployeeList();

        //strsql = "select Att_PartialLeave_Request.Trans_Id,Att_PartialLeave_Request.Partial_Leave_Date,att_attendanceregister.in_time,att_attendanceregister.out_time, Set_EmployeeMaster.Emp_Code,set_employeemaster.emp_name,set_employeemaster.emp_name_l,Att_TimeTable.TimeTable_Name,att_attendanceregister.onduty_time,att_attendanceregister.offduty_time,Att_PartialLeave_Request.From_Time,Att_PartialLeave_Request.To_Time, case when  Att_PartialLeave_Request.Field1='Auto' then 'Normal' else 'Authorized' end as Leave_Type,Att_PartialLeave_Request.field1 as Entered,Is_Confirmed from Att_PartialLeave_Request left join att_attendanceregister on Att_PartialLeave_Request.emp_id = att_attendanceregister.emp_id and Att_PartialLeave_Request.Partial_Leave_Date = att_attendanceregister.att_date left join att_timetable   on att_attendanceregister.TimeTable_Id = att_timetable.TimeTable_Id inner join Set_EmployeeMaster on Att_PartialLeave_Request.emp_id = Set_EmployeeMaster.Emp_Id where Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";

        strsql = "select Case When Att_PartialLeave_Request.Partial_Leave_Type = 0 then  'Personal' else 'Official' end as PartialLeaveType, Att_PartialLeave_Request.Trans_Id,Att_PartialLeave_Request.Partial_Leave_Date,att_attendanceregister.in_time,att_attendanceregister.out_time, Set_EmployeeMaster.Emp_Code,set_employeemaster.emp_name,set_employeemaster.emp_name_l,Att_TimeTable.TimeTable_Name,att_attendanceregister.onduty_time,att_attendanceregister.offduty_time,Att_PartialLeave_Request.From_Time,Att_PartialLeave_Request.To_Time, case when  Att_PartialLeave_Request.Field1='Auto' then 'System Generated' else 'Manually' end as Leave_Type,Att_PartialLeave_Request.field1 as Entered,Is_Confirmed from Att_PartialLeave_Request left join att_attendanceregister on Att_PartialLeave_Request.emp_id = att_attendanceregister.emp_id and Att_PartialLeave_Request.Partial_Leave_Date = att_attendanceregister.att_date left join att_timetable   on att_attendanceregister.TimeTable_Id = att_timetable.TimeTable_Id inner join Set_EmployeeMaster on Att_PartialLeave_Request.emp_id = Set_EmployeeMaster.Emp_Id where Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";

        strsql = strsql + " and Att_PartialLeave_Request.emp_id in (" + strEmpIds + ")";

        strsql = strsql + " order by  cast(Emp_Code as int),Partial_Leave_Date";

        dt = objda.return_DataTable(strsql);

        if (ddlleaveType.SelectedIndex > 0)
        {
            if (ddlleaveType.SelectedValue.Trim() == "Auto")
            {
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt = new DataView(dt, "Entered='Auto'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }
                catch
                {

                }

               
            }
            else
            {
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        dt = new DataView(dt, "Entered=''", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }
                catch
                {

                }

                
            }
        }
        objPageCmn.FillData((GridView)gvOverTime, dt, "", "");
        if (dt == null || dt.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
        }

    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        int counter = 0;

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();


        try
        {
            foreach (GridViewRow gvrow in gvOverTime.Rows)
            {
                if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
                {
                    objda.execute_Command("delete from Att_PartialLeave_Request where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text.Trim() + "");
                    counter++;
                }
            }

            DisplayMessage(counter.ToString() + " Record deleted successfully");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnGetRecord_Click(null, null);
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }
    }
    public string GetTimeDuration(string strFromTime, string strToTime)
    {
        DateTime dtfrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strFromTime.Split(':')[0]), Convert.ToInt32(strFromTime.Split(':')[1]), 0);
        DateTime dtto = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strToTime.Split(':')[0]), Convert.ToInt32(strToTime.Split(':')[1]), 0);

        if(dtfrom <= dtto)
        {
            //TimeSpan duration = DateTime.Parse(strToTime).Subtract(DateTime.Parse(strFromTime));
            int minutes =(((dtto - dtfrom).Hours) * 60 ) + (dtto - dtfrom).Minutes;
            if(minutes>0)
            { 
                TimeSpan spWorkMin = TimeSpan.FromMinutes(minutes);
                string workHours = spWorkMin.ToString(@"hh\:mm");
                return workHours;
            }            
        }
        else
        {

            int minutes = (new DateTime(dtfrom.Year,dtfrom.Month ,dtfrom.Day,23,59,0) - dtfrom).Minutes;
            minutes = ((new DateTime(dtfrom.Year, dtfrom.Month, dtfrom.Day, 23, 59, 0) - dtfrom).Hours * 60) + minutes;
            int minutes1 = (dtto -  new DateTime( dtto.Year, dtto.Month, dtto.Day, 0, 0, 0) ).Minutes;
            minutes1 = ((dtto - new DateTime(dtto.Year, dtto.Month, dtto.Day, 0, 0, 0)).Hours * 60) + minutes1; 

            if ((minutes  + minutes1 )> 0)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes((minutes + minutes1));
                string workHours = spWorkMin.ToString(@"hh\:mm");
                return workHours;
            }
        }

        return "00:00";

       
        
        
        //return (Convert.ToDateTime(dtfrom) - Convert.ToDateTime(dtto)).ToString("HH:mm");
    }

    public string GetEmployeeList()
    {
        string stremplist = "0";
        string strSql = "select Emp_Id from set_employeemaster where set_employeemaster.Emp_Id in (" + GetEmplist() + ") and Isactive='True' and Field2='False'";
        if (ddlGradeFrom.SelectedIndex > 0 && ddlGradeTo.SelectedIndex > 0)
        {
            strSql = strSql + " and  cast(ltrim(rtrim(Grade)) as int)>=" + ddlGradeFrom.SelectedValue.Trim() + " and cast(ltrim(rtrim(Grade)) as int)<=" + ddlGradeTo.SelectedValue.Trim() + "";
        }
        DataTable dt = objda.return_DataTable(strSql);
        foreach (DataRow dr in dt.Rows)
        {
            stremplist = stremplist + "," + dr["Emp_Id"].ToString();
        }

        return stremplist;

    }
}