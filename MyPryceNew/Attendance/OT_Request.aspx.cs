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
using ClosedXML.Excel;

public partial class Attendance_OT_Request : BasePage
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
    LogProcess objLogProcess = null;
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
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objLogProcess = new LogProcess(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/OT_Request.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
        //if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        //{
        //    ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        //}
        ScriptManager.RegisterStartupScript(this, GetType(), "key", "CacheItems()", true);
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        //btnExecute_Click(null, null);

        if (gvOverTime.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition",
            "attachment;filename=Attendance.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < gvOverTime.Rows.Count; i++)
            {
                GridViewRow row = gvOverTime.Rows[i];

                //Change Color back to white
                row.BackColor = System.Drawing.Color.White;

                //Apply text style to each Row
                row.Attributes.Add("class", "textmode");

                //Apply style to Individual Cells of Alternating Row
                if (i % 2 != 0)
                {
                    // row.Cells[0].Style.Add("background-color", "#C2D69B");
                    // row.Cells[1].Style.Add("background-color", "#C2D69B");
                    // row.Cells[2].Style.Add("background-color", "#C2D69B");
                    //row.Cells[3].Style.Add("background-color", "#C2D69B");
                }
            }
            gvOverTime.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            DisplayMessage("No Record Available");
        }
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
        BindTreeView();
        GetEmpCodeRange();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        bool IsApprovalPerson = false;
        string strEmpIds = GetEmployeeList();
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        string strHRId = string.Empty;
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

        try
        {
            objda.execute_Command("Delete From Att_PartialLeave_Request  Where Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Emp_Id in (" + strEmpIds + ")");
            objda.execute_Command("Delete From Att_AttendanceRegister  Where Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att_date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Emp_Id in (" + strEmpIds + ")");

            DisplayMessage("Operation Completed!");
            btnGo_Click(null, null);
        }
        catch
        {
            DisplayMessage("Operation Error!");
        }
      


    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        bool IsApprovalPerson = false;
        string strEmpIds = GetEmployeeList();
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        string strHRId = string.Empty;
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

        DataTable dtHR = objda.return_DataTable("select set_employeemaster.Emp_Id from set_approvalmaster APR inner join set_location_department on  APR.ResponsibleDepartmentManager =set_location_department.Dep_Id inner join set_employeemaster on set_location_department.emp_id = set_employeemaster.emp_id   where APR.ResponsibleDepartmentManager<>0");

        if (dtHR.Rows.Count == 0)
        {
            DisplayMessage("Approval hierarchy is not configured");
            return;
        }
        else
        {
            strHRId = dtHR.Rows[0]["Emp_Id"].ToString();
        }


        if (ddlRequestType.SelectedValue.Trim() == "OverTime" && objda.return_DataTable("select isnull( count(*),0) from att_attendanceregister att inner join set_employeemaster on   att.emp_id =set_employeemaster.emp_id   where (att.TeamLeader_Id = " + Session["EmpId"].ToString() + " or att.DepManager_Id =" + Session["EmpId"].ToString() + " or att.ParentDepManager_Id =" + Session["EmpId"].ToString() + " or att.HR_Id=" + Session["EmpId"].ToString() + ") and (att.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date<='" + Convert.ToDateTime(txtTodate.Text) + "') and att.Emp_Id in (" + strEmpIds + ") and Set_EmployeeMaster.isactive='True' and att.Is_Absent<>1 and att.Is_Leave<>1 ").Rows[0][0].ToString() != "0")
        {
            IsApprovalPerson = true;
            btnupdate.Visible = true;

        }
        else if (ddlRequestType.SelectedValue.Trim() == "Absent" && objda.return_DataTable("select isnull( count(*),0) from att_attendanceregister att inner join set_employeemaster on   att.emp_id =set_employeemaster.emp_id   where (att.TeamLeader_Id = " + Session["EmpId"].ToString() + " or att.DepManager_Id =" + Session["EmpId"].ToString() + " or att.ParentDepManager_Id =" + Session["EmpId"].ToString() + " or att.HR_Id=" + Session["EmpId"].ToString() + ") and (att.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date<='" + Convert.ToDateTime(txtTodate.Text) + "') and att.Emp_Id in (" + strEmpIds + ") and Set_EmployeeMaster.isactive='True' and att.Is_Absent=1").Rows[0][0].ToString() != "0")
        {
            IsApprovalPerson = true;
            btnupdate.Visible = true;
        }


        // For Pryce Pegasus
        //if (strHRId == Session["EmpId"].ToString())
        //{
        //    btnReject.Visible = true;
        //}
        //else
        //{
        //    btnReject.Visible = false;
        //}


        //For NIC  
        btnReject.Visible = false;
        string[] strEId = ConfigurationManager.AppSettings["ApprovalEmpId"].ToString().Split(new char[] { ',' });
        for(int k=0;k< strEId.Length;k++)
        {
            if(strEId [k] == Session["EmpId"].ToString())
            {
                btnReject.Visible = true;
                
            }
        }




        if (ddlRequestType.SelectedValue.Trim() == "OverTime")
        {
            gvOverTime.Columns[10].Visible = true;
            gvOverTime.Columns[11].Visible = true;
            gvOverTime.Columns[17].Visible = true;
            //strsql = "select Att.Remarks,Att.trans_id,Set_EmployeeMaster.Emp_Id,Att.RequestStatus,att.att_date,Set_EmployeeMaster.Emp_Code,Att_ShiftManagement.shift_name,Att_TimeTable.TimeTable_Name,Set_EmployeeMaster.emp_name,att.onduty_time,att.OffDuty_Time,att.In_Time,att.Out_Time,case when att.Is_Holiday=1 then 'Holiday' when att.Is_Week_Off=1 then 'Week Off' else 'Normal' end as DayType  ,Att_ScheduleDescription.Field2 as AssignedOT,case when att.Is_Holiday=1 then Holiday_Min when att.Is_Week_Off=1 then week_off_min else overtime_min end as ActualOTValue ,att.TeamLeader_Id,att.TeamLeader_Status,att.depmanager_id,att.DepManager_Status,att.ParentDepManager_Id,att.ParentDepManager_Status,att.HR_Id,att.hr_status from att_attendanceregister att inner join set_employeemaster  on att.emp_id = set_employeemaster.emp_id left join Att_ScheduleDescription on Att_ScheduleDescription.emp_id = att.emp_id  and Att_ScheduleDescription.att_date=att.Att_Date  and Att_ScheduleDescription.shift_id = att.shift_id and Att_ScheduleDescription.timetable_id = att.timetable_id left join Att_TimeTable on att.TimeTable_Id = Att_TimeTable.TimeTable_Id left join Att_ShiftManagement on att.Shift_Id =  Att_ShiftManagement.shift_id  where att.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True' and att.Is_Absent<>1 and att.Is_Leave<>1";
            //strsql = "select Att.Remarks,Att.trans_id,Set_EmployeeMaster.Emp_Id,Att.RequestStatus,att.att_date,Set_EmployeeMaster.Emp_Code,Att_ShiftManagement.shift_name,Att_TimeTable.TimeTable_Name,Set_EmployeeMaster.emp_name,att.onduty_time,att.OffDuty_Time,att.In_Time,att.Out_Time,case when att.Is_Holiday = 1 then 'Holiday' when att.Is_Week_Off = 1 then 'Week Off' else  Case When  DATENAME(DW, att.Att_Date) = 'Friday' then  'Week Off' Else  'Normal'   End end as DayType  , Att_ScheduleDescription.Field2 as AssignedOT,  case when att.Is_Holiday = 1 then Holiday_Min when att.Is_Week_Off = 1 then week_off_min else overtime_min end as ActualOTValue ,((Select Field2 From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = att.Att_Date and Att_ScheduleDescription.Emp_Id = att.Emp_Id))  as PDOT, Case When  DATENAME(DW, att.Att_Date) = 'Friday' then((  case When(DATEDIFF(minute, (Case when(DATEDIFF(minute, Cast(Cast(att.In_Time as Time) as varchar(5)), Cast(Cast(att.OnDuty_Time as Time) as Varchar(5))) > 0) Then  Cast(Cast(att.OnDuty_Time as Time) as varchar(5)) else  Cast(Cast(att.In_Time as Time) as varchar(5)) end), (Case when(DATEDIFF(minute, Cast(Cast(att.OffDuty_Time as Time) as varchar(5)), Cast(Cast(att.Out_Time as Time) as Varchar(5))) > 0) then Cast(Cast(att.OffDuty_Time as Time) as varchar(5)) else  Cast(Cast(att.Out_Time as Time) as varchar(5)) end)) ) > ((Select Field2 From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = att.Att_Date and Att_ScheduleDescription.Emp_Id = att.Emp_Id))  then((Select Field2  From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = att.Att_Date and    Att_ScheduleDescription.Emp_Id = att.Emp_Id)) -LateMin - Early_Min  else   DATEDIFF(minute, (Case when(DATEDIFF(minute, Cast(Cast(att.In_Time as Time) as varchar(5)), Cast(Cast(att.OnDuty_Time as Time) as Varchar(5))) > 0) Then  Cast(Cast(att.OnDuty_Time as Time) as varchar(5)) else  Cast(Cast(att.In_Time as Time) as varchar(5)) end)   ,   (Case when(DATEDIFF(minute, Cast(Cast(att.OffDuty_Time as Time) as varchar(5)), Cast(Cast(att.Out_Time as Time) as Varchar(5))) > 0) then Cast(Cast(att.OffDuty_Time as Time) as varchar(5)) else  Cast(Cast(att.Out_Time as Time) as varchar(5)) end))  end )-LateMin - EarlyMin) else    case When att. OffDuty_Time <> '1900-01-01 00:00:00.000'  then  case  when datediff(minute, Cast(att.OffDuty_Time as Time), Cast(Out_Time as Time)  ) >= 60 then(datediff(minute, Cast(att.OffDuty_Time as Time), Cast(Out_Time as Time)))  else '' end  else  '' end end as 'AOT' ,att.TeamLeader_Id,att.TeamLeader_Status,att.depmanager_id,att.DepManager_Status,att.ParentDepManager_Id,att.ParentDepManager_Status,att.HR_Id,att.hr_status from att_attendanceregister att inner join set_employeemaster on att.emp_id = set_employeemaster.emp_id left join Att_ScheduleDescription on Att_ScheduleDescription.emp_id = att.emp_id  and Att_ScheduleDescription.att_date = att.Att_Date  and Att_ScheduleDescription.shift_id = att.shift_id and Att_ScheduleDescription.timetable_id = att.timetable_id left join Att_TimeTable on att.TimeTable_Id = Att_TimeTable.TimeTable_Id left join Att_ShiftManagement on att.Shift_Id = Att_ShiftManagement.shift_id  where att.Att_Date >= '" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date <= '" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive = 'True' and att.Is_Absent <> 1 and att.Is_Leave <> 1";
            strsql = "select Att.Remarks,Att.trans_id,Set_EmployeeMaster.Emp_Id,Att.RequestStatus,att.att_date,Set_EmployeeMaster.Emp_Code,Att_ShiftManagement.shift_name,Att_TimeTable.TimeTable_Name,Set_EmployeeMaster.emp_name,att.onduty_time,att.OffDuty_Time,att.In_Time,att.Out_Time,case when att.Is_Holiday = 1 then 'Holiday' when att.Is_Week_Off = 1 then 'Week Off' else  Case When  DATENAME(DW, att.Att_Date) = 'Friday' then  'Week Off' Else  'Normal'   End end as DayType  , Att_ScheduleDescription.Field2 as AssignedOT,  case when att.Is_Holiday = 1 then Holiday_Min when att.Is_Week_Off = 1 then week_off_min else overtime_min end as ActualOTValue ,((Select Field2 From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = att.Att_Date and Att_ScheduleDescription.Emp_Id = att.Emp_Id))  as PDOT,          CASE        WHEN Datename(dw, att.att_date) = 'Friday' THEN  	      	   ( ( CASE WHEN 	   ( Datediff (minute, 						( CASE	WHEN(	 Datediff	( minute,Cast(Cast(att.in_time AS TIME) AS  VARCHAR(5)),  Cast(Cast(att.onduty_time AS TIME) AS VARCHAR(5))) >0  ) 							THEN  Cast(Cast(att.in_time AS TIME) AS VARCHAR(5))      							ELSE Cast(Cast(att.onduty_time AS TIME) AS VARCHAR (5 ))  						END ),   					 ( CASE	WHEN(	Datediff	(minute, Cast(Cast(att.offduty_time AS TIME) AS VARCHAR(5)),Cast(Cast(att.out_time AS TIME) AS VARCHAR(5))) > 0 ) 							THEN Cast(Cast(att.out_time AS TIME) AS VARCHAR	(5)) 							ELSE Cast(Cast(att.offduty_time AS TIME)  AS VARCHAR (5))  							  END )              		  )  	)  		>  		((SELECT field2 FROM        att_scheduledescription WHERE        att_scheduledescription.att_date = att.att_date     AND att_scheduledescription.emp_id = att.emp_id))  		 		THEN 		 		Cast(((SELECT  field2 FROM  att_scheduledescription  WHERE       att_scheduledescription.att_date = att.att_date       AND att_scheduledescription.emp_id = att.emp_id))  as Int) 		-  		   Cast(( CASE	WHEN(	 Datediff	( minute,Cast(Cast(att.onduty_time AS TIME) AS  VARCHAR(5)),  Cast(Cast(att.in_time  AS TIME) AS VARCHAR(5))) > 0 ) 							THEN   Datediff	( minute,Cast(Cast(att.onduty_time AS TIME) AS  VARCHAR(5)),  Cast(Cast(att.in_time  AS TIME) AS VARCHAR(5)))  							ELSE '0'							END )  as Int)    		 							+   Cast( 					 ( CASE	WHEN(	Datediff	(minute, Cast(Cast(att.offduty_time AS TIME) AS VARCHAR(5)),Cast(Cast(att.out_time AS TIME) AS VARCHAR(5))) > 0 ) 							THEN Datediff	(minute, Cast(Cast(att.offduty_time AS TIME) AS VARCHAR(5)),Cast(Cast(att.out_time AS TIME) AS VARCHAR(5))) 							ELSE 0  							  END ) as Int)             ELSE      	  Datediff (minute,  									  	( CASE	WHEN(	 Datediff	( minute,Cast(Cast(att.in_time AS TIME) AS  VARCHAR(5)),  Cast(Cast(att.onduty_time AS TIME) AS VARCHAR(5))) > 0 ) 							THEN  Cast(Cast(att.in_time AS TIME) AS VARCHAR(5))      							ELSE Cast(Cast(att.onduty_time AS TIME) AS VARCHAR (5 )) 							END ),  					 ( CASE	WHEN(	Datediff	(minute, Cast(Cast(att.offduty_time AS TIME) AS VARCHAR(5)),Cast(Cast(att.out_time AS TIME) AS VARCHAR(5))) > 0 ) 							THEN Cast(Cast(att.out_time AS TIME) AS VARCHAR	(5)) 							ELSE Cast(Cast(att.offduty_time AS TIME)  AS VARCHAR (5)) 							  END )  			)        END  	    	   ) - latemin - earlymin ) 	           ELSE            CASE              WHEN att. offduty_time <> '1900-01-01 00:00:00.000' THEN               CASE WHEN Datediff(minute, Cast(att.offduty_time AS TIME), Cast(out_time AS TIME))>= 60  					THEN Case When Cast((Select Field2 From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = att.Att_Date and Att_ScheduleDescription.Emp_Id = att.Emp_Id)as Int) <  ( Datediff(minute, Cast(att.offduty_time AS TIME ), Cast(out_time AS TIME)) )    Then   Cast ((Select Field2 From  Att_ScheduleDescription Where Att_ScheduleDescription.Att_Date = att.Att_Date and Att_ScheduleDescription.Emp_Id = att.Emp_Id) as Int)  Else   ( Datediff(minute, Cast(att.offduty_time AS TIME ), Cast(out_time AS TIME)) ) End                  ELSE ''                END               ELSE ''            END         END                                                        	        	    	        	   AS 'AOT',att.TeamLeader_Id,att.TeamLeader_Status,att.depmanager_id,att.DepManager_Status,att.ParentDepManager_Id,att.ParentDepManager_Status,att.HR_Id,att.hr_status from att_attendanceregister att inner join set_employeemaster on att.emp_id = set_employeemaster.emp_id left join Att_ScheduleDescription on Att_ScheduleDescription.emp_id = att.emp_id  and Att_ScheduleDescription.att_date = att.Att_Date  and Att_ScheduleDescription.shift_id = att.shift_id and Att_ScheduleDescription.timetable_id = att.timetable_id left join Att_TimeTable on att.TimeTable_Id = Att_TimeTable.TimeTable_Id left join Att_ShiftManagement on att.Shift_Id = Att_ShiftManagement.shift_id  where att.Att_Date >= '" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date <= '" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive = 'True' and att.Is_Absent <> 1 and att.Is_Leave <> 1";


        }
        else
        {
            gvOverTime.Columns[10].Visible = false;
            gvOverTime.Columns[11].Visible = false;
            gvOverTime.Columns[17].Visible = false;
            //strsql = "select Att.Remarks,Att.trans_id,Set_EmployeeMaster.Emp_Id,Att.RequestStatus,att.att_date,Set_EmployeeMaster.Emp_Code,Att_ShiftManagement.shift_name,Att_TimeTable.TimeTable_Name,Set_EmployeeMaster.emp_name,att.onduty_time,att.OffDuty_Time,att.In_Time,att.Out_Time,'Absent' as DayType  ,Att_ScheduleDescription.Field2 as AssignedOT,case when att.Is_Holiday=1 then Holiday_Min when att.Is_Week_Off=1 then week_off_min else overtime_min end as ActualOTValue ,att.TeamLeader_Id,att.TeamLeader_Status,att.depmanager_id,att.DepManager_Status,att.ParentDepManager_Id,att.ParentDepManager_Status,att.HR_Id,att.hr_status from att_attendanceregister att inner join set_employeemaster  on att.emp_id = set_employeemaster.emp_id left join Att_ScheduleDescription on Att_ScheduleDescription.emp_id = att.emp_id  and Att_ScheduleDescription.att_date=att.Att_Date  and Att_ScheduleDescription.shift_id = att.shift_id and Att_ScheduleDescription.timetable_id = att.timetable_id left join Att_TimeTable on att.TimeTable_Id = Att_TimeTable.TimeTable_Id left join Att_ShiftManagement on att.Shift_Id =  Att_ShiftManagement.shift_id  where att.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True' and att.Is_Absent=1 ";
            //strsql = "select Att.Remarks,Att.trans_id,Set_EmployeeMaster.Emp_Id,Att.RequestStatus,att.att_date,Set_EmployeeMaster.Emp_Code,Att_ShiftManagement.shift_name,Att_TimeTable.TimeTable_Name,Set_EmployeeMaster.emp_name,att.onduty_time,att.OffDuty_Time,att.In_Time,att.Out_Time,'Absent' as DayType  ,Att_ScheduleDescription.Field2 as AssignedOT,case when att.Is_Holiday=1 then Holiday_Min when att.Is_Week_Off=1 then week_off_min else overtime_min end as ActualOTValue ,att.TeamLeader_Id,att.TeamLeader_Status,att.depmanager_id,att.DepManager_Status,att.ParentDepManager_Id,att.ParentDepManager_Status,att.HR_Id,att.hr_status from att_attendanceregister att inner join set_employeemaster  on att.emp_id = set_employeemaster.emp_id left join Att_ScheduleDescription on Att_ScheduleDescription.emp_id = att.emp_id  and Att_ScheduleDescription.att_date=att.Att_Date  and Att_ScheduleDescription.shift_id = att.shift_id and Att_ScheduleDescription.timetable_id = att.timetable_id left join Att_TimeTable on att.TimeTable_Id = Att_TimeTable.TimeTable_Id left join Att_ShiftManagement on att.Shift_Id =  Att_ShiftManagement.shift_id  where att.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True' and att.Is_Absent=1 ";

            strsql = "select Att.Remarks,Att.trans_id,Set_EmployeeMaster.Emp_Id,Att.RequestStatus,att.att_date,Set_EmployeeMaster.Emp_Code,Att_ShiftManagement.shift_name,Att_TimeTable.TimeTable_Name,Set_EmployeeMaster.emp_name,att.onduty_time,att.OffDuty_Time,att.In_Time,att.Out_Time,'Absent' as DayType  ,Att_ScheduleDescription.Field2 as AssignedOT,case when att.Is_Holiday=1 then Holiday_Min when att.Is_Week_Off=1 then week_off_min else overtime_min end as ActualOTValue ,att.TeamLeader_Id,att.TeamLeader_Status,att.depmanager_id,att.DepManager_Status,att.ParentDepManager_Id,att.ParentDepManager_Status,att.HR_Id,att.hr_status from att_attendanceregister att inner join set_employeemaster  on att.emp_id = set_employeemaster.emp_id left join Att_ScheduleDescription on Att_ScheduleDescription.emp_id = att.emp_id  and Att_ScheduleDescription.att_date=att.Att_Date  and Att_ScheduleDescription.shift_id = att.shift_id and Att_ScheduleDescription.timetable_id = att.timetable_id left join Att_TimeTable on att.TimeTable_Id = Att_TimeTable.TimeTable_Id left join Att_ShiftManagement on att.Shift_Id =  Att_ShiftManagement.shift_id  where att.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.att_date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True' and att.Is_Absent=1 ";
        }


        strsql = strsql + " and att.Emp_Id in (" + strEmpIds + ")";



        if (IsApprovalPerson)
        {
            strsql = strsql + " and (att.TeamLeader_Id = " + Session["EmpId"].ToString() + " or att.DepManager_Id =" + Session["EmpId"].ToString() + " or att.ParentDepManager_Id =" + Session["EmpId"].ToString() + " or att.HR_Id=" + Session["EmpId"].ToString() + ")";
        }

        strsql = strsql + " order by  cast(Emp_Code as int),att_date";

        dt = objda.return_DataTable(strsql);

        if (ddlRequestType.SelectedValue.Trim() == "OverTime" && ddlOTFilter.SelectedValue.Trim() == "OverTime")
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    // dt = new DataView(dt, "ActualOTValue>0", "", DataViewRowState.CurrentRows).ToTable();
                    dt = new DataView(dt, "AOT>0", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            catch
            {

            }
       

        }


        objPageCmn.FillData((GridView)gvOverTime, dt, "", "");
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
    }
    public string GetDay(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.DayOfWeek.ToString();
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
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        bool isselected = false;

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim() == "")
            {
                ((TextBox)gvrow.FindControl("txtOtMinute")).Text = "00:00";
            }


            if (((CheckBox)gvrow.FindControl("chktl")).Enabled && ((CheckBox)gvrow.FindControl("chktl")).Checked)
            {
                isselected = true;
                objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',TeamLeader_Status='" + ((CheckBox)gvrow.FindControl("chktl")).Checked.ToString() + "',TeamLeader_Action_Date='" + DateTime.Now.ToString() + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
            }

            if (((CheckBox)gvrow.FindControl("chkDepManager")).Enabled)
            {
                //if (((CheckBox)gvrow.FindControl("chkDepManager")).Enabled && ((CheckBox)gvrow.FindControl("chkDepManager")).Checked)
                //{
                isselected = true;
                objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',DepManager_Status='" + ((CheckBox)gvrow.FindControl("chkDepManager")).Checked.ToString() + "',DepManager_Action_Date='" + DateTime.Now.ToString() + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
            }
            if (((CheckBox)gvrow.FindControl("chkparentDepManager")).Enabled)
            {
                //if (((CheckBox)gvrow.FindControl("chkparentDepManager")).Enabled && ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked)
                //{
                isselected = true;


                if (((Label)gvrow.FindControl("lblDayType")).Text == "Holiday")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',ParentDepManager_Status='" + ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked.ToString() + "',ParentDepManager_Action_Date='" + DateTime.Now.ToString() + "',Holiday_Min ='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }
                if (((Label)gvrow.FindControl("lblDayType")).Text == "Week Off")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',ParentDepManager_Status='" + ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked.ToString() + "',ParentDepManager_Action_Date='" + DateTime.Now.ToString() + "',Week_Off_Min ='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'     where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }
                if (((Label)gvrow.FindControl("lblDayType")).Text == "Normal")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',ParentDepManager_Status='" + ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked.ToString() + "',ParentDepManager_Action_Date='" + DateTime.Now.ToString() + "',OverTime_Min ='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }
            }

            if (((CheckBox)gvrow.FindControl("chkHR")).Enabled && ((CheckBox)gvrow.FindControl("chkHR")).Checked)
            {
                isselected = true;
                if (((Label)gvrow.FindControl("lblDayType")).Text == "Holiday")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',HR_Status='" + ((CheckBox)gvrow.FindControl("chkHR")).Checked.ToString() + "',HR_Action_Date='" + DateTime.Now.ToString() + "',RequestStatus='Approved',Holiday_Min ='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }
                if (((Label)gvrow.FindControl("lblDayType")).Text == "Week Off")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',HR_Status='" + ((CheckBox)gvrow.FindControl("chkHR")).Checked.ToString() + "',HR_Action_Date='" + DateTime.Now.ToString() + "',RequestStatus='Approved',Week_Off_Min ='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'     where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }
                if (((Label)gvrow.FindControl("lblDayType")).Text == "Normal")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',HR_Status='" + ((CheckBox)gvrow.FindControl("chkHR")).Checked.ToString() + "',HR_Action_Date='" + DateTime.Now.ToString() + "',RequestStatus='Approved',OverTime_Min ='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }

                if (((Label)gvrow.FindControl("lblDayType")).Text.Trim() == "Absent")
                {
                    objda.execute_Command("update Att_AttendanceRegister set Field2='" + getMinute(((TextBox)gvrow.FindControl("txtOtMinute")).Text.Trim()) + "',HR_Status='" + ((CheckBox)gvrow.FindControl("chkHR")).Checked.ToString() + "',HR_Action_Date='" + DateTime.Now.ToString() + "',RequestStatus='Approved'   where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
                }

            }

        }


        if (!isselected)
        {
            DisplayMessage("Please select record");
            return;
        }
        else
        {
            DisplayMessage("Record Approved successfully");
            btnGo_Click(null, null);
        }


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
            ((CheckBox)gvrow.FindControl("chkSelect")).Checked = Result;


            if (((CheckBox)gvrow.FindControl("chktl")).Enabled)
            {
                ((CheckBox)gvrow.FindControl("chktl")).Checked = Result;
            }

            if (((CheckBox)gvrow.FindControl("chkDepManager")).Enabled)
            {
                ((CheckBox)gvrow.FindControl("chkDepManager")).Checked = Result;
            }

            if (((CheckBox)gvrow.FindControl("chkparentDepManager")).Enabled)
            {
                ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked = Result;
            }

            if (((CheckBox)gvrow.FindControl("chkHR")).Enabled)
            {
                ((CheckBox)gvrow.FindControl("chkHR")).Checked = Result;
            }

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

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmpCodeRange();
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        int counter = 0;

        if (gvOverTime.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }

        bool Isselected = false;

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                Isselected = true;
                break;
            }
        }


        if (!Isselected)
        {
            DisplayMessage("Select Record");
            return;
        }

        string strHRId = "0";





        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {

                objda.execute_Command("update Att_AttendanceRegister set RequestStatus='Rejected',HR_Status='" + false.ToString() + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                counter++;

            }
        }

        if (counter > 0)
        {
            DisplayMessage(counter.ToString() + " Request rejected successfully");
            btnGo_Click(null, null);
        }

    }

    protected void btnRequest_Click(object sender, EventArgs e)
    {
        int counter = 0;

        if (gvOverTime.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }

        bool Isselected = false;

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                Isselected = true;
                break;
            }
        }


        if (!Isselected)
        {
            DisplayMessage("Select Record");
            return;
        }

        string strHRId = "0";

        DataTable dtHR = objda.return_DataTable("select set_employeemaster.Emp_Id from set_approvalmaster APR inner join set_location_department on  APR.ResponsibleDepartmentManager =set_location_department.Dep_Id inner join set_employeemaster on set_location_department.emp_id = set_employeemaster.emp_id   where APR.ResponsibleDepartmentManager<>0");

        if (dtHR.Rows.Count == 0)
        {
            DisplayMessage("Approval hierarchy is not configured");
            return;
        }
        else
        {
            strHRId = dtHR.Rows[0]["Emp_Id"].ToString();
        }

        string strsecManager = string.Empty;
        string strDepManager = string.Empty;

        DataTable dtHierarchy = objda.return_DataTable("select set_employeemaster.emp_id,set_employeemaster.Field5 as TLID,Set_Location_Department.Emp_Id as SecManagerId,ldp.Emp_id as DepManagerId  from set_employeemaster inner join Set_Location_Department on set_employeemaster.department_id = Set_Location_Department.dep_id inner join Set_DepartmentMaster on Set_EmployeeMaster.Department_Id  = Set_DepartmentMaster.dep_id inner join Set_Location_Department ldp on ldp.dep_id = Set_DepartmentMaster.Parent_Id    where set_employeemaster.Emp_Id in (" + GetEmployeeList() + ")");

        DataTable dtEmphierarchy = new DataTable();

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {
            if (((Label)gvrow.FindControl("lblStatus")).Text.Trim() == "" && ((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                dtEmphierarchy = new DataView(dtHierarchy, "Emp_id=" + ((Label)gvrow.FindControl("lblEmpId")).Text + "", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmphierarchy.Rows.Count > 0)
                {
                    objda.execute_Command("update Att_AttendanceRegister set RequestStatus='Requested',RequestDate='" + DateTime.Now.ToString() + "',TeamLeader_Id=" + dtEmphierarchy.Rows[0]["TLID"].ToString() + ",DepManager_Id=" + dtEmphierarchy.Rows[0]["SecManagerId"].ToString() + ",ParentDepManager_Id=" + dtEmphierarchy.Rows[0]["DepManagerId"].ToString() + ",HR_Id =" + strHRId + ",ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "',Remarks='" + ((TextBox)gvrow.FindControl("txtremarks")).Text + "' where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                    counter++;
                }
            }
        }

        if (counter > 0)
        {
            DisplayMessage(counter.ToString() + " Request submitted successfully");
            btnGo_Click(null, null);
        }
        else
        {
            DisplayMessage("Request not submitted successfully , please check configuration");
        }
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;


        if(((CheckBox)gvrow.FindControl("chkSelect")).Checked)
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

        if (((CheckBox)gvrow.FindControl("chktl")).Enabled)
        {
            ((CheckBox)gvrow.FindControl("chktl")).Checked = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
        }

        if (((CheckBox)gvrow.FindControl("chkDepManager")).Enabled)
        {
            ((CheckBox)gvrow.FindControl("chkDepManager")).Checked = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
        }


      

        if (((CheckBox)gvrow.FindControl("chkparentDepManager")).Enabled)
        {
            ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
        }

        if (((CheckBox)gvrow.FindControl("chkHR")).Enabled)
        {
            ((CheckBox)gvrow.FindControl("chkHR")).Checked = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
        }

        //if (((CheckBox)gvrow.FindControl("chkDepManager")).Checked)
        //{
        //    gvrow.BackColor = System.Drawing.Color.Aqua;
        //}
        //else
        //{
        //    gvrow.BackColor = System.Drawing.Color.White;
        //}

        //if (((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked)
        //{
        //    gvrow.BackColor = System.Drawing.Color.Aqua;
        //}
        //else
        //{
        //    gvrow.BackColor = System.Drawing.Color.White;
        //}

        //if (((CheckBox)gvrow.FindControl("chkHR")).Checked)
        //{
        //    gvrow.BackColor = System.Drawing.Color.Aqua;
        //}
        //else
        //{
        //    gvrow.BackColor = System.Drawing.Color.White;
        //}

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

    protected void btnfilterdepartment_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "show_modal()", true);
        //string url = "../Attendance/LogProcess.aspx";
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }


    #endregion

    protected void btnOtReport_Click(object sender, EventArgs e)
    {

        //string strObjectId = Common.GetObjectIdbyPageURL("../Attendance/OT_Request.aspx", Session["DBConnection"].ToString());
        //string strReportId = objda.return_DataTable("select ReportId from sys_reports where objectid =" + strObjectId + "").Rows[0]["ReportId"].ToString();
        //Session["ReportID"] = strReportId;
        //Session["otfromdate"] = txtFromdate.Text;
        //Session["ottodate"] = txtTodate.Text;
        //Session["otemplist"] = GetEmployeeList();
        //string strCmd = string.Format("window.open('../Utility/reportViewer.aspx?reportId=" + strReportId + "','_blank','width=1024',false);");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);


        string strEmpIds = GetEmployeeList();
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        string strHRId = string.Empty;
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

        strsql = "Select  Set_CompanyMaster.Company_Name as [Employer] ,    Set_EmployeeMaster.Emp_Code as [Person Number],Att_AttendanceRegister.Att_Date , dbo.MinutesToDuration( OverTime_Min) , CASE WHEN  DateName(WEEKDAY,Att_AttendanceRegister.Att_Date ) = 'Friday' then   'Friday OT' else  case when Att_AttendanceRegister.Is_Holiday ='1' then 'Holiday OT' else  'Normal OT' end  end as 'OverTime Type' From Att_AttendanceRegister  INNER JOIN  Set_EmployeeMaster ON  Set_EmployeeMaster.Emp_Id = Att_AttendanceRegister.Emp_Id INNER JOIN Set_LocationMaster  On Set_LocationMaster.Location_Id = Set_EmployeeMaster.Location_Id  INNER JOIN   Set_CompanyMaster ON Set_CompanyMaster.Company_Id = Set_EmployeeMaster .Company_Id    INNER JOIN   Set_DepartmentMaster  On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id INNER JOIN   Set_DesignationMaster ON   Set_DesignationMaster.Designation_Id =  Set_EmployeeMaster.Designation_Id     where OverTime_Min > 0  and Att_AttendanceRegister.Att_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Att_AttendanceRegister.Att_Date <='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";

        strsql = strsql + " and Att_AttendanceRegister.Emp_Id in (" + strEmpIds + ")";


        strsql = strsql + " order by  cast(Emp_Code as int),Att_AttendanceRegister.Att_Date";

        dt = objda.return_DataTable(strsql);

        dt.TableName = "OverTime";


        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "OverTime");

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=OverTime" + DateTime.Now.ToString("ddmmyyyyHHMM") + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }


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
}