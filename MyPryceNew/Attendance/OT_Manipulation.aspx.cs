using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.IO;
using System.Configuration;

public partial class Attendance_OT_Manipulation : BasePage
{
    Att_ShiftDescription objShiftdesc = null;
    LocationMaster ObjLocationMaster = null;
    DepartmentMaster objDep = null;
    Common cmn = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    Att_ShiftManagement objShift = null;
    DataAccessClass objda = null;
    Att_ScheduleMaster objEmpSch = null;
    Set_ApplicationParameter objAppParam = null;
    PageControlCommon objPageCmn = null;
    static string Depart;
    static string locationids;
    LogProcess objLogProcess = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objLogProcess = new LogProcess(Session["DBConnection"].ToString());
        objShiftdesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/ot_manipulation.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtOtMinute.Text = "00:00";

            fillLocation();
            TreeViewDepartment.Attributes.Add("onclick", "OnTreeClick(event)");
            BindTreeView();
            GetEmpCodeRange();
            CalenderFromdate.Format = Session["DateFormat"].ToString();
            CalenderTodate.Format = Session["DateFormat"].ToString();
            FillShift();
            FillGrade();
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                div_Ot.Visible = false;
                btnupdate.Visible = false;
                gvOverTime.Columns[0].Visible = false;
                gvOverTime.Columns[8].Visible = false;
                lblHeader.Text = "Predefined shift";
            }
        }
        //if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        //}
        ScriptManager.RegisterStartupScript(this, GetType(), "key", "CacheItems()", true);
    }

    public void FillEmpCodeList(DropDownList ddlEmocodeList, DataTable dt)
    {
        ddlEmocodeList.Items.Clear();
        ddlEmocodeList.DataSource = dt;
        ddlEmocodeList.DataTextField = "Emp_Code";
        ddlEmocodeList.DataValueField = "Emp_Code";
        ddlEmocodeList.DataBind();
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


    public DataTable GetEmployeeFilteredRecord()
    {

        Session["deptFilter"] = null;

        foreach (TreeNode ModuleNode in TreeViewDepartment.Nodes)
        {
            if (ModuleNode.Checked)
            {
                Session["deptFilter"] += ModuleNode.Value + ",";
                //objLocDept.InsertLocationDepartmentMaster(editid.Value, ModuleNode.Value, "0", "", "", "", "", "", "", "", "", "True", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "True", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

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
                strLocationIDs = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
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
            ddlcodefrom.SelectedValue = new DataView(dt, "", "Emp_Code", DataViewRowState.CurrentRows).ToTable().Rows[0]["Emp_Code"].ToString();
            ddlcodeto.SelectedValue = new DataView(dt, "", "Emp_Code desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Emp_Code"].ToString();
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
    protected void gvOverTime_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblDay = e.Row.FindControl("lblDay") as Label;
            string strDay = lblDay.Text; // returns as NULL
            if (strDay.ToString() == "Friday")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(254, 250, 108);
            }
            if (strDay.ToString() == "Saturday")
            {
                e.Row.BackColor = System.Drawing.Color.FromArgb(254, 250, 95);
            }



            //if (ListPrice > 1000)
            //{
            //    e.Row.BackColor = System.Drawing.Color.Red;
            //}
        }
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }
    public string GetDay(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.DayOfWeek.ToString();
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        GetEmpCodeRange();
        BindTreeView();

    }

    protected void btnGo_Click(object sender, EventArgs e)
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




        strsql = "select att_scheduledescription.trans_id,Set_EmployeeMaster.Emp_Code,set_employeemaster.emp_name,set_employeemaster.emp_name_l,Att_TimeTable.TimeTable_Name,Att_ScheduleDescription.Att_Date,Att_TimeTable.OnDuty_Time,Att_TimeTable.offduty_time,att_scheduledescription.Field2,Set_EmployeeMaster.Emp_id from att_scheduledescription inner join att_timetable   on att_scheduledescription.TimeTable_Id = att_timetable.TimeTable_Id inner join Set_EmployeeMaster on Att_ScheduleDescription.emp_id = Set_EmployeeMaster.Emp_Id where Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att_date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";

        //if (ddlLocation.SelectedIndex > 0)
        //{
        //    strsql = strsql + " and Set_EmployeeMaster.location_id = " + ddlLocation.SelectedValue;
        //}
        //if (dpDepartment.SelectedIndex > 0)
        //{
        //    strsql = strsql + " and Set_EmployeeMaster.department_id = " + dpDepartment.SelectedValue;
        //}

        strsql = strsql + " and emp_code in (" + GetEmplist() + ")";
        //strsql = strsql + " and ( cast(emp_code as int)>=" + txtcodefrom.Text + " and cast(emp_code as int)<=" + txtcodeTo.Text + ")";

        if (ddlGradeFrom.SelectedIndex > 0 && ddlGradeTo.SelectedIndex > 0)
        {
            strsql = strsql + " and  cast(ltrim(rtrim(Grade)) as int)>=" + ddlGradeFrom.SelectedValue.Trim() + " and cast(ltrim(rtrim(Grade)) as int)<=" + ddlGradeTo.SelectedValue.Trim() + "";
        }

        strsql = strsql + " order by  cast(Emp_Code as int),att_date";

        dt = objda.return_DataTable(strsql);

        objPageCmn.FillData((GridView)gvOverTime, dt, "", "");
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        string strMessage = "";
        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {



            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                if (((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim() == "")
                {
                    ((TextBox)gvrow.FindControl("txtOtMinute")).Text = "00:00";
                }

                string strDate = ((Label)gvrow.FindControl("lblDate")).Text;
                string strEmpId = ((Label)gvrow.FindControl("lblEmp_Id")).Text;

                DataTable dtTemp = objda.return_DataTable(" Select  * From Att_AttendanceRegister Where Emp_Id = '" + strEmpId + "'  and Att_Date = '" + strDate + "' and(DepManager_Status = '1'  or ParentDepManager_Status = '1' or  HR_Status = '1')");
                if (dtTemp.Rows.Count == 0)
                {
                    objda.execute_Command("update  att_scheduledescription set ModifiedBy ='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "',Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "'  where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");

                }
                else
                {
                    strMessage += " " + strDate + " Date Already Take Action for OT! ";
                }
            }
           


        }



        DisplayMessage("Operation completed successfully! <br/>" + strMessage);
        btnGo_Click(null, null);
        
    }
    protected void btnUdpateShift_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                // Update Time on Date
                string strDate = ((Label)gvrow.FindControl("lblDate")).Text.ToString();
                string strEmpCode = ((Label)gvrow.FindControl("lblEmpId")).Text.ToString();
                UpdateShift(strEmpCode, strDate, strDate);
            }
        }
        btnGo_Click(null, null);
        DisplayMessage("Shift updated successfully");
    }




    public int GetCycleDay(string day)
    {
        string cycleday = string.Empty;
        string[] weekdays = new string[8];

        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        for (int i = 1; i <= 7; i++)
        {
            if (weekdays[i] == day)
            {
                cycleday = i.ToString();

            }
        }

        return int.Parse(cycleday);

    }
    protected void btnupdateshift_Click(object sender, EventArgs e)
    {


        string OverlapDate = string.Empty;

        bool IsTemp = false;
        int rem = 0;

        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        string ExcludeDayAs = string.Empty;
        string CompWeekOffDay = string.Empty;

        ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString());
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }


        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }

        if (ddlShift.SelectedIndex == 0)
        {
            DisplayMessage("Select Shift Name");
            ddlShift.Focus();
            return;
        }

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





        string strsql = "select emp_id from  Set_EmployeeMaster where  Set_EmployeeMaster.isactive='True' and Set_EmployeeMaster.Field2='False'";

        strsql = strsql + " and emp_code in (" + GetEmplist() + ")";
        //strsql = strsql + " and ( cast(emp_code as int)>=" + txtcodefrom.Text + " and cast(emp_code as int)<=" + txtcodeTo.Text + ")";
        //if (ddlLocation.SelectedIndex > 0)
        //{
        //    strsql = strsql + " and Set_EmployeeMaster.location_id = " + ddlLocation.SelectedValue;
        //}
        //if (dpDepartment.SelectedIndex > 0)
        //{
        //    strsql = strsql + " and Set_EmployeeMaster.department_id = " + dpDepartment.SelectedValue;
        //}

        if (ddlGradeFrom.SelectedIndex > 0 && ddlGradeTo.SelectedIndex > 0)
        {
            strsql = strsql + " and  cast(ltrim(rtrim(Grade)) as int)>=" + ddlGradeFrom.SelectedValue.Trim() + " and cast(ltrim(rtrim(Grade)) as int)<=" + ddlGradeTo.SelectedValue.Trim() + "";
        }

        strsql = strsql + " order by  cast(Emp_Code as int)";

        DataTable dt = objda.return_DataTable(strsql);


        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Employee not found");
            return;
        }

        int b = 0;
        //start

        for (int i = 0; i < dt.Rows.Count; i++)
        {


            // objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), Convert.ToDateTime(txtFromdate.Text).ToString(), Convert.ToDateTime(txtTodate.Text).ToString());

            DataTable dtSch = objEmpSch.GetSheduleMaster();

            dtSch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + ddlShift.SelectedValue + "' and Emp_Id='" + dt.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtSch.Rows.Count == 0)
            {
                b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "Normal Shift", objSys.getDateForInput(txtFromdate.Text).ToString(), objSys.getDateForInput(txtTodate.Text).ToString(), "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "Normal Shift", objSys.getDateForInput(txtFromdate.Text).ToString(), objSys.getDateForInput(txtTodate.Text).ToString(), "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //DisplayMessage("Shift Already Assignd to this Employee");
                //return;

            }

            if (b != 0)
            {
                DateTime DtFromDate = objSys.getDateForInput(txtFromdate.Text.ToString());
                DateTime DtToDate = objSys.getDateForInput(txtTodate.Text.ToString());
                int days = DtToDate.Subtract(DtFromDate).Days + 1;
                if (IsTemp == false)
                {

                    int counter = 0;
                    // From Date to To Date
                    DataTable dtShift = objShift.GetShiftMasterById(Session["CompId"].ToString(), ddlShift.SelectedValue);


                    DataTable dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(ddlShift.SelectedValue);

                    if (dtShiftD.Rows.Count == 0)
                    {
                        DisplayMessage("Shift Not Defined");
                        return;

                    }

                    dtShiftD = new DataView(dtShiftD, "", "", DataViewRowState.CurrentRows).ToTable();


                    DataTable dtTime = new DataTable();
                    DateTime dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());
                    DataTable dtTempShift = new DataTable();
                    int TotalDays = 1;
                    int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
                    int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

                    string cycletype = string.Empty;
                    string cycleday = string.Empty;
                    DateTime ApplyFromDate = new DateTime();
                    if (index == 7)
                    {

                        bool IsweekOff = false;
                        int daysShift = cycle * index;
                        string weekday = DtFromDate.DayOfWeek.ToString();


                        int state = 0;
                        int k = GetCycleDay(weekday);
                        int j = 1;
                        int a = k;
                        int f = 0;
                        while (DtFromDate <= DtToDate)
                        {
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                            if (k % 7 == 0)
                            {

                                if (f != 0)
                                {
                                    if (k % 7 == 0 && j > cycle)
                                    {
                                        j++;
                                        rem = 1;
                                    }
                                    //else
                                    //{
                                    //    j++;
                                    //}

                                    if (j > cycle)
                                    {
                                        j = 1;
                                    }
                                }

                            }
                            f++;
                            if (k <= daysShift || j == cycle)
                            {


                                a = GetCycleDay(DtFromDate.DayOfWeek.ToString());
                                if (rem == 1 && k % 7 == 0)
                                {
                                    j++;
                                }

                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (k % 7 == 0 && j == cycle)
                                {
                                    j = 1;
                                }
                                if (k % 7 == 0 && j < cycle)
                                {
                                    j++;
                                }

                                if (dtGetTemp1.Rows.Count > 0)
                                {
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dtTime.Rows.Count > 0)
                                    {
                                        if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                        {
                                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());
                                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {

                                            //for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            //{
                                            //    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                            //    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                            //    int flag1 = 0;
                                            //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                            //    {

                                            //        //if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                            //        //{
                                            //        //    flag1 = 1;

                                            //        //    OverlapDate += dtTime.Rows[s]["Att_Date"].ToString() + ",";
                                            //        //}

                                            //    }


                                            //    if (flag1 == 0)
                                            //    {
                                            //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                            //    }
                                            //}


                                        }
                                    }
                                    else
                                    {
                                        if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                        {
                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }

                                        }
                                        else
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                        }

                                    }
                                }
                                else
                                {
                                    //Modified accoding to excludedays parameter 19 sept 2013 kunal
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                        }
                                        else
                                        {
                                            // Modified By Nitin Jain On 27/08/2014....
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (str == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                            //..............
                                        }

                                    }
                                }




                                k++;
                            }
                            else
                            {
                                k = 1;
                                j = 1;
                                f = 0;
                                a = GetCycleDay(DtFromDate.DayOfWeek.ToString());

                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                if (dtGetTemp1.Rows.Count > 0)
                                {
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dtTime.Rows.Count > 0)
                                    {
                                        if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                        {
                                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            //{
                                            //    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                            //    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                            //    int flag1 = 0;
                                            //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                            //    {

                                            //        if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                            //        {
                                            //            flag1 = 1;
                                            //        }

                                            //    }


                                            //    if (flag1 == 0)
                                            //    {
                                            //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                            //    }
                                            //}

                                        }
                                    }
                                    else
                                    {
                                        if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                        {
                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }

                                        }
                                        else
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                        }
                                    }
                                }
                                else
                                {
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            // Modifed By Nitin On 27/8/2014/////
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                                k++;

                            }



                            DtFromDate = DtFromDate.AddDays(1);
                        }

                    }
                    else if (index == 31)
                    {
                        int daysShift = cycle * index;

                        int k = DtFromDate.Day;
                        int a = 0;
                        int j = 1;
                        int mon = DtFromDate.Month;
                        while (DtFromDate <= DtToDate)
                        {
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                            if (k <= daysShift)
                            {
                                a = DtFromDate.Day;


                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //
                                if (dtGetTemp1.Rows.Count > 0)
                                {


                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                        if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                        {

                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int flag1 = 0;

                                            if (dtTime.Rows.Count > 0)
                                            {
                                                //DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                //{
                                                //    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());
                                                //    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                //}
                                                //else
                                                //{
                                                //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                //    {

                                                //        //if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                //        //{
                                                //        //    flag1 = 1;
                                                //        //}

                                                //    }
                                                //    if (flag1 == 0)
                                                //    {

                                                //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                                //    }
                                                //}

                                            }
                                            else
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                                    }

                                                }

                                            }
                                        }

                                    }



                                }
                                else
                                {
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                    }
                                }


                            }

                            k++;
                            if (k > daysShift)
                            {

                                k = 1;
                                j = 1;
                            }


                            DtFromDate = DtFromDate.AddDays(1);
                            if (DtFromDate.Day == 1)
                            {

                                j++;

                            }
                        }

                    }
                    else if (index == 1)
                    {
                        int k = 1;
                        int a = k;
                        int daysShift = cycle * index;
                        while (DtFromDate <= DtToDate)
                        {
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());
                            if (k <= daysShift)
                            {
                                a = k;


                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //
                                if (dtGetTemp1.Rows.Count > 0)
                                {


                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                        if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                        {

                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int flag1 = 0;

                                            if (dtTime.Rows.Count > 0)
                                            {
                                                //DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                //{
                                                //    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                //    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                //}
                                                //else
                                                //{
                                                //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                //    {

                                                //        //if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                //        //{
                                                //        //    flag1 = 1;
                                                //        //}

                                                //    }
                                                //    if (flag1 == 0)
                                                //    {
                                                //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                                //    }

                                                //}
                                            }
                                            else
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {

                                                    for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                    {



                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());





                                                    }
                                                }

                                            }
                                        }

                                    }



                                }
                                else
                                {
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                    }
                                }


                            }

                            k++;
                            if (k > daysShift)
                            {

                                k = 1;

                            }


                            DtFromDate = DtFromDate.AddDays(1);

                        }




                    }


                }
                else
                {//temporary


                }
            }

        }
        ddlShift.SelectedIndex = 0;
        btnGo_Click(null, null);
        DisplayMessage("Shift assigned successfully");

        //end


    }


    protected void UpdateShift(string strEmpCode, string strFromDate, string strToDate)
    {


        string OverlapDate = string.Empty;

        bool IsTemp = false;
        int rem = 0;

        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        string ExcludeDayAs = string.Empty;
        string CompWeekOffDay = string.Empty;

        ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString());
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }


        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }

        if (ddlShift.SelectedIndex == 0)
        {
            DisplayMessage("Select Shift Name");
            ddlShift.Focus();
            return;
        }










        string strsql = "select emp_id from  Set_EmployeeMaster where  Set_EmployeeMaster.isactive='True' and Set_EmployeeMaster.Field2='False'";

        strsql = strsql + " and emp_code in (" + strEmpCode + ")";




        strsql = strsql + " order by  cast(Emp_Code as int)";

        DataTable dt = objda.return_DataTable(strsql);


        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Employee not found");
            return;
        }

        int b = 0;
        //start

        for (int i = 0; i < dt.Rows.Count; i++)
        {


            // objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), Convert.ToDateTime(txtFromstrFromDate).ToString(), Convert.ToDateTime(strToDate).ToString());

            DataTable dtSch = objEmpSch.GetSheduleMaster();

            dtSch = new DataView(dtSch, "Shift_Type='Normal Shift'  and Shift_Id='" + ddlShift.SelectedValue + "' and Emp_Id='" + dt.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtSch.Rows.Count == 0)
            {
                b = objEmpSch.InsertScheduleMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "Normal Shift", objSys.getDateForInput(strFromDate).ToString(), objSys.getDateForInput(strToDate).ToString(), "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                b = objEmpSch.UpdateScheduleMaster(dtSch.Rows[0]["Schedule_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "Normal Shift", objSys.getDateForInput(strFromDate).ToString(), objSys.getDateForInput(strToDate).ToString(), "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //DisplayMessage("Shift Already Assignd to this Employee");
                //return;

            }

            if (b != 0)
            {
                DateTime DtFromDate = objSys.getDateForInput(strFromDate.ToString());
                DateTime DtToDate = objSys.getDateForInput(strToDate.ToString());
                int days = DtToDate.Subtract(DtFromDate).Days + 1;
                if (IsTemp == false)
                {

                    int counter = 0;
                    // From Date to To Date
                    DataTable dtShift = objShift.GetShiftMasterById(Session["CompId"].ToString(), ddlShift.SelectedValue);


                    DataTable dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(ddlShift.SelectedValue);

                    if (dtShiftD.Rows.Count == 0)
                    {
                        DisplayMessage("Shift Not Defined");
                        return;

                    }

                    dtShiftD = new DataView(dtShiftD, "", "", DataViewRowState.CurrentRows).ToTable();


                    DataTable dtTime = new DataTable();
                    DateTime dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());
                    DataTable dtTempShift = new DataTable();
                    int TotalDays = 1;
                    int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
                    int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

                    string cycletype = string.Empty;
                    string cycleday = string.Empty;
                    DateTime ApplyFromDate = new DateTime();
                    if (index == 7)
                    {

                        bool IsweekOff = false;
                        int daysShift = cycle * index;
                        string weekday = DtFromDate.DayOfWeek.ToString();


                        int state = 0;
                        int k = GetCycleDay(weekday);
                        int j = 1;
                        int a = k;
                        int f = 0;
                        while (DtFromDate <= DtToDate)
                        {
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                            if (k % 7 == 0)
                            {

                                if (f != 0)
                                {
                                    if (k % 7 == 0 && j > cycle)
                                    {
                                        j++;
                                        rem = 1;
                                    }
                                    //else
                                    //{
                                    //    j++;
                                    //}

                                    if (j > cycle)
                                    {
                                        j = 1;
                                    }
                                }

                            }
                            f++;
                            if (k <= daysShift || j == cycle)
                            {


                                a = GetCycleDay(DtFromDate.DayOfWeek.ToString());
                                if (rem == 1 && k % 7 == 0)
                                {
                                    j++;
                                }

                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (k % 7 == 0 && j == cycle)
                                {
                                    j = 1;
                                }
                                if (k % 7 == 0 && j < cycle)
                                {
                                    j++;
                                }

                                if (dtGetTemp1.Rows.Count > 0)
                                {
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dtTime.Rows.Count > 0)
                                    {
                                        if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                        {
                                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());
                                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {



                                        }
                                    }
                                    else
                                    {
                                        if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                        {
                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }

                                        }
                                        else
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                        }

                                    }
                                }
                                else
                                {
                                    //Modified accoding to excludedays parameter 19 sept 2013 kunal
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                        }
                                        else
                                        {
                                            // Modified By Nitin Jain On 27/08/2014....
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (str == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                            //..............
                                        }

                                    }
                                }




                                k++;
                            }
                            else
                            {
                                k = 1;
                                j = 1;
                                f = 0;
                                a = GetCycleDay(DtFromDate.DayOfWeek.ToString());

                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                if (dtGetTemp1.Rows.Count > 0)
                                {
                                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dtTime.Rows.Count > 0)
                                    {
                                        if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                                        {
                                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                            else
                                            {

                                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            //{
                                            //    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                            //    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                            //    int flag1 = 0;
                                            //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                            //    {

                                            //        if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                            //        {
                                            //            flag1 = 1;
                                            //        }

                                            //    }


                                            //    if (flag1 == 0)
                                            //    {
                                            //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                            //    }
                                            //}

                                        }
                                    }
                                    else
                                    {
                                        if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                                        {
                                            for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                            {
                                                objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }

                                        }
                                        else
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                        }
                                    }
                                }
                                else
                                {
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            // Modifed By Nitin On 27/8/2014/////
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                                k++;

                            }



                            DtFromDate = DtFromDate.AddDays(1);
                        }

                    }
                    else if (index == 31)
                    {
                        int daysShift = cycle * index;

                        int k = DtFromDate.Day;
                        int a = 0;
                        int j = 1;
                        int mon = DtFromDate.Month;
                        while (DtFromDate <= DtToDate)
                        {
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                            if (k <= daysShift)
                            {
                                a = DtFromDate.Day;


                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //
                                if (dtGetTemp1.Rows.Count > 0)
                                {


                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                        if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                        {

                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int flag1 = 0;

                                            if (dtTime.Rows.Count > 0)
                                            {
                                                //DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                //{
                                                //    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());
                                                //    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                //}
                                                //else
                                                //{
                                                //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                //    {

                                                //        //if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                //        //{
                                                //        //    flag1 = 1;
                                                //        //}

                                                //    }
                                                //    if (flag1 == 0)
                                                //    {

                                                //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                                //    }
                                                //}

                                            }
                                            else
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                    {
                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                                    }

                                                }

                                            }
                                        }

                                    }



                                }
                                else
                                {
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                    }
                                }


                            }

                            k++;
                            if (k > daysShift)
                            {

                                k = 1;
                                j = 1;
                            }


                            DtFromDate = DtFromDate.AddDays(1);
                            if (DtFromDate.Day == 1)
                            {

                                j++;

                            }
                        }

                    }
                    else if (index == 1)
                    {
                        int k = 1;
                        int a = k;
                        int daysShift = cycle * index;
                        while (DtFromDate <= DtToDate)
                        {
                            objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());
                            if (k <= daysShift)
                            {
                                a = k;


                                DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //
                                if (dtGetTemp1.Rows.Count > 0)
                                {


                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                        if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                                        {

                                            DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                            if (dts.Rows.Count == 0)
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int flag1 = 0;

                                            if (dtTime.Rows.Count > 0)
                                            {
                                                //DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString()));
                                                //if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                //{
                                                //    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                //    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                //}
                                                //else
                                                //{
                                                //    for (int s = 0; s < dtTime.Rows.Count; s++)
                                                //    {

                                                //        //if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime))
                                                //        //{
                                                //        //    flag1 = 1;
                                                //        //}

                                                //    }
                                                //    if (flag1 == 0)
                                                //    {
                                                //        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                                //    }

                                                //}
                                            }
                                            else
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.DeleteScheduleDescByEmpIdandDate(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString(), DtFromDate.ToString());

                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                                else
                                                {

                                                    for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                                    {



                                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());





                                                    }
                                                }

                                            }
                                        }

                                    }



                                }
                                else
                                {
                                    DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(dt.Rows[i]["Emp_Id"].ToString(), DtFromDate.ToString());

                                    if (dts.Rows.Count == 0)
                                    {
                                        if (ExcludeDayAs == "IsOff")
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                        }
                                        else
                                        {
                                            // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                            foreach (string str in CompWeekOffDay.Split(','))
                                            {
                                                if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                                {
                                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dt.Rows[i]["Emp_Id"].ToString(), ddlShift.SelectedValue, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                }
                                            }
                                        }
                                    }
                                }


                            }

                            k++;
                            if (k > daysShift)
                            {

                                k = 1;

                            }


                            DtFromDate = DtFromDate.AddDays(1);

                        }




                    }


                }
                else
                {//temporary


                }
            }

        }

        //end


    }


    public string GetEmplist()
    {
        string strEmpids = "0";

        int mincode = Convert.ToInt32(ddlcodefrom.SelectedValue);
        int maxcode = Convert.ToInt32(ddlcodeto.SelectedValue);

        foreach (ListItem ddlitem in ddlcodeto.Items)
        {
            if (Convert.ToInt32(ddlitem.Value) >= mincode && Convert.ToInt32(ddlitem.Value) <= maxcode)
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


        return strEmpids;

    }
    protected void btnassignOT_Click(object sender, EventArgs e)
    {
        if (txtOtMinute.Text == "")
        {
            txtOtMinute.Text = "00:00";
        }
        bool Result = false;
        string strMessage = "";
        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                Result = true;

                string strDate = ((Label)gvrow.FindControl("lblDate")).Text;
                string strEmpId = ((Label)gvrow.FindControl("lblEmp_Id")).Text;

                DataTable dtTemp = objda.return_DataTable(" Select  * From Att_AttendanceRegister Where Emp_Id = '" + strEmpId + "'  and Att_Date = '" + strDate + "' and(DepManager_Status = '1'  or ParentDepManager_Status = '1' or  HR_Status = '1')");
                if (dtTemp.Rows.Count == 0)
                {
                    objda.execute_Command("update  att_scheduledescription set ModifiedBy ='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "',Field2='" + getMinute(txtOtMinute.Text.Trim()) + "'  where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");

                }
                else
                {
                    strMessage += " " + strDate + " Date Already Take Action for OT! " ;
                }

            }
        }

        if (!Result)
        {
            DisplayMessage("Please Select Record");
            txtOtMinute.Focus();
            return;
        }

        txtOtMinute.Text = "";
        btnGo_Click(null, null);
        DisplayMessage("Overtime assigned successfully <br/> "+ strMessage);

    }

    public void FillShift()
    {
        DataTable dtShift = objda.return_DataTable("select shift_id,(shift_name+'/'+shift_name_L) as shift_name  from Att_ShiftManagement where isactive='True' order by shift_name");
        ddlShift.DataSource = dtShift;
        ddlShift.DataTextField = "shift_name";
        ddlShift.DataValueField = "shift_id";
        ddlShift.DataBind();
        ddlShift.Items.Insert(0, "--Select--");
    }

    protected void chkHeaderSelect_CheckedChanged(object sender, EventArgs e)
    {
        bool Result = ((CheckBox)gvOverTime.HeaderRow.FindControl("chkHeaderSelect")).Checked;

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkSelect")).Checked = Result;
            if (Result)
            {
                gvrow.BackColor = System.Drawing.Color.FromArgb(66, 165, 245);
            }
            else
            {
                if (((Label)gvrow.FindControl("lblDay")).Text == "Friday")
                {
                    gvrow.BackColor = System.Drawing.Color.FromArgb(254, 250, 108);
                }
                else if (((Label)gvrow.FindControl("lblDay")).Text == "Saturday")
                {
                    gvrow.BackColor = System.Drawing.Color.FromArgb(254, 250, 108);
                }
                else
                {
                    gvrow.BackColor = System.Drawing.Color.White;
                }
            }
        }
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
        {
            gvrow.BackColor = System.Drawing.Color.Aqua;
        }
        else
        {
            if (((Label)gvrow.FindControl("lblDay")).Text == "Friday")
            {
                gvrow.BackColor = System.Drawing.Color.FromArgb(254, 250, 108);
            }
            else if (((Label)gvrow.FindControl("lblDay")).Text == "Saturday")
            {
                gvrow.BackColor = System.Drawing.Color.FromArgb(254, 250, 108);
            }
            else
            {
                gvrow.BackColor = System.Drawing.Color.White;
            }

        }
    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmpCodeRange();
    }
    public string GetEmployeeList()
    {
        string stremplist = "0";
        string strSql = "select Emp_Id from set_employeemaster where set_employeemaster.Emp_Code in (" + GetEmplist() + ") and Isactive='True' and Field2='False'";
        if (ddlGradeFrom.SelectedIndex > 0 && ddlGradeTo.SelectedIndex > 0)
        {
            strSql = strSql + " and  cast(ltrim(rtrim(Grade)) as int)>=" + ddlGradeFrom.SelectedValue.Trim() + " and cast(ltrim(rtrim(Grade)) as int)<=" + ddlGradeTo.SelectedValue.Trim() + "";
        }
        DataTable dt = objda.return_DataTable(strSql);
        foreach (DataRow dr in dt.Rows)
        {
            if (stremplist == "")
            {
                stremplist = dr["Emp_Id"].ToString();
            }
            else
            {
                stremplist = stremplist + "," + dr["Emp_Id"].ToString();
            }
        }

        return stremplist;

    }
    protected void btnLogProcess_Click(object sender, EventArgs e)
    {
        string strEmpIds = GetEmployeeList();


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

        objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strEmpIds, Session["UserId"].ToString(), "0", Convert.ToDateTime(txtFromdate.Text), Convert.ToDateTime(txtTodate.Text), Session["EmpId"].ToString(), null, HttpContext.Current.Session["TimeZoneId"].ToString(), ConfigurationManager.AppSettings["LeaveIntegration"].ToString(), ConfigurationManager.AppSettings["ErrorFile"].ToString(), ConfigurationManager.AppSettings["Shift_Range_Auto"].ToString());

        DisplayMessage("Log Processed successfully");

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
                strDeptvalue = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.Items[j].Value, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
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




}