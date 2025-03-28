using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.IO;
using ClosedXML.Excel;

public partial class Attendance_Short_Leave_Request : BasePage
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
    LogProcess objLogProcess = null;
    static string Depart;
    static string locationids;
    PageControlCommon objPageCmn = null;
    protected override void OnPreRender(EventArgs e)
    {

        // add base.OnPreRender(e); at the beginning of the method.

        base.OnPreRender(e);
        // codes to handle with your controls.



    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
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


        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/Short_Leave_Request.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
        //ScriptManager.RegisterStartupScript(this, GetType(), "key", "FreezeHeader();", true);
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

        //if (gvOverTime.Rows.Count > 0)
        //{
        //    Response.Clear();
        //    Response.Buffer = true;

        //    Response.AddHeader("content-disposition",
        //    "attachment;filename=Attendance.xls");
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.ms-excel";
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    for (int i = 0; i < gvOverTime.Rows.Count; i++)
        //    {
        //        GridViewRow row = gvOverTime.Rows[i];

        //        //Change Color back to white
        //        row.BackColor = System.Drawing.Color.White;

        //        //Apply text style to each Row
        //        row.Attributes.Add("class", "textmode");

        //        //Apply style to Individual Cells of Alternating Row
        //        if (i % 2 != 0)
        //        {
        //            // row.Cells[0].Style.Add("background-color", "#C2D69B");
        //            // row.Cells[1].Style.Add("background-color", "#C2D69B");
        //            // row.Cells[2].Style.Add("background-color", "#C2D69B");
        //            //row.Cells[3].Style.Add("background-color", "#C2D69B");
        //        }
        //    }
        //    gvOverTime.RenderControl(hw);

        //    //style to format numbers to string
        //    string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        //    Response.Write(style);
        //    Response.Output.Write(sw.ToString());
        //    Response.Flush();
        //    Response.End();
        //}
        //else
        //{
        //    DisplayMessage("No Record Available");
        //}



        //if (gvOverTime.Rows.Count > 0)
        //{
        //    DataTable dt = new DataTable("GridView_Data");
        //    dt =(DataTable) Session["FilterRecord"];
        //    dt.TableName = "Approval Report";
        //    using (XLWorkbook wb = new XLWorkbook())
        //    {
        //        wb.Worksheets.Add(dt);

        //        Response.Clear();
        //        Response.Buffer = true;
        //        Response.Charset = "";
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.AddHeader("content-disposition", "attachment;filename=GridView.xlsx");
        //        using (MemoryStream MyMemoryStream = new MemoryStream())
        //        {
        //            wb.SaveAs(MyMemoryStream);
        //            MyMemoryStream.WriteTo(Response.OutputStream);
        //            Response.Flush();
        //            Response.End();
        //        }
        //    }
        //}



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

        strsql = "Select  Set_CompanyMaster.Company_Name as [Employer] ,    Set_EmployeeMaster.Emp_Code as [Person Number] ,'Short Leave' as [Absence Type],  Att_PartialLeave_Request.Partial_Leave_Date as [Start Date] ,Att_PartialLeave_Request.From_Time as [Start Time] , Att_PartialLeave_Request.Partial_Leave_Date as [End Date] , Att_PartialLeave_Request.To_Time as [End Time] , '' as [End Date Duration], '' as [Start Date Duration],Att_PartialLeave_Request.Is_Confirmed AS [Approval Status],'Submitted' as  [Absence Status] From    Att_PartialLeave_Request INNER JOIN  Set_EmployeeMaster ON  Set_EmployeeMaster.Emp_Id = Att_PartialLeave_Request.Emp_Id INNER JOIN Set_LocationMaster  On Set_LocationMaster.Location_Id = Set_EmployeeMaster.Location_Id  INNER JOIN   Set_CompanyMaster ON Set_CompanyMaster.Company_Id = Set_EmployeeMaster .Company_Id    INNER JOIN   Set_DepartmentMaster  On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id INNER JOIN   Set_DesignationMaster ON   Set_DesignationMaster.Designation_Id =  Set_EmployeeMaster.Designation_Id   where Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";

        strsql = strsql + " and Att_PartialLeave_Request.Emp_Id in (" + strEmpIds + ")";


        strsql = strsql + " order by  cast(Emp_Code as int),Att_PartialLeave_Request.Partial_Leave_Date";

        dt = objda.return_DataTable(strsql);

        dt.TableName = "PartialLeave";


        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dt, "Customers");

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=ShortLeave" + DateTime.Now.ToString("ddmmyyyyHHMM") + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
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


        if (objda.return_DataTable("select isnull( count(*),0) from Att_PartialLeave_Request att inner join set_employeemaster on   att.emp_id =set_employeemaster.emp_id   where ((att.TeamLeader_Id = " + Session["EmpId"].ToString() + " or att.DepManager_Id =" + Session["EmpId"].ToString() + " or att.ParentDepManager_Id =" + Session["EmpId"].ToString() + " or att.HR_Id=" + Session["EmpId"].ToString() + ") or (',' + RTRIM(Multiple_HR_Id) + ',') LIKE '%,"+ Session["EmpId"].ToString() + ",%'   or (',' + RTRIM(Multiple_ParentDepManager_Id) + ',') LIKE '%," + Session["EmpId"].ToString() + ",%' )and (att.Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and att.Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "') and att.Emp_Id in (" + strEmpIds + ") and Set_EmployeeMaster.isactive='True'").Rows[0][0].ToString() != "0")
        {
            IsApprovalPerson = true;
            btnupdate.Visible = true;

        }

        if (strHRId == Session["EmpId"].ToString())
        {
            btnReject.Visible = true;
        }
        else
        {
            btnReject.Visible = false;
        }

        //        strsql = "select Att_PartialLeave_Request.emp_id,Att_PartialLeave_Request.Trans_Id,Att_PartialLeave_Request.Partial_Leave_Date,att_attendanceregister.in_time,att_attendanceregister.out_time, Set_EmployeeMaster.Emp_Code,set_employeemaster.emp_name,set_employeemaster.emp_name_l,Att_TimeTable.TimeTable_Name,att_attendanceregister.onduty_time,att_attendanceregister.offduty_time,Att_PartialLeave_Request.From_Time,Att_PartialLeave_Request.To_Time,case when  Att_PartialLeave_Request.Field1='Auto' then 'Normal' else 'Authorized' end as Leave_Type ,Att_PartialLeave_Request.field1 as Entered,Is_Confirmed as RequestStatus,[Att_PartialLeave_Request].[RequestDate]      ,[Att_PartialLeave_Request].[TeamLeader_Id]      ,[Att_PartialLeave_Request].[TeamLeader_Status]      ,[Att_PartialLeave_Request].[TeamLeader_Action_Date]      ,[Att_PartialLeave_Request].[DepManager_Id]      ,[Att_PartialLeave_Request].[DepManager_Status]      ,[Att_PartialLeave_Request].[DepManager_Action_Date]      ,[Att_PartialLeave_Request].[ParentDepManager_Id]      ,[Att_PartialLeave_Request].[ParentDepManager_Status]      ,[Att_PartialLeave_Request].[ParentDepManager_Action_Date]      ,[Att_PartialLeave_Request].[HR_Id]      ,[Att_PartialLeave_Request].[HR_Status]      ,[Att_PartialLeave_Request].[HR_Action_Date] from Att_PartialLeave_Request left join att_attendanceregister on Att_PartialLeave_Request.emp_id = att_attendanceregister.emp_id and Att_PartialLeave_Request.Partial_Leave_Date = att_attendanceregister.att_date inner join att_timetable   on att_attendanceregister.TimeTable_Id = att_timetable.TimeTable_Id inner join Set_EmployeeMaster on Att_PartialLeave_Request.emp_id = Set_EmployeeMaster.Emp_Id where Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";
        strsql = "select  'false' as HRState,  Multiple_HR_Id,Case When Att_PartialLeave_Request.Partial_Leave_Type = 0 then  'Personal' else 'Official' end as PartialLeaveType,Att_PartialLeave_Request.emp_id,Att_PartialLeave_Request.Trans_Id,Att_PartialLeave_Request.Partial_Leave_Date,att_attendanceregister.in_time,att_attendanceregister.out_time, Set_EmployeeMaster.Emp_Code,set_employeemaster.emp_name,set_employeemaster.emp_name_l,Att_TimeTable.TimeTable_Name,att_attendanceregister.onduty_time,att_attendanceregister.offduty_time,Att_PartialLeave_Request.From_Time,Att_PartialLeave_Request.To_Time,case when  Att_PartialLeave_Request.Field1='Auto' then 'System Generated' else 'Manually' end as Leave_Type ,Att_PartialLeave_Request.field1 as Entered,Is_Confirmed as RequestStatus,[Att_PartialLeave_Request].[RequestDate]      ,[Att_PartialLeave_Request].[TeamLeader_Id]      ,[Att_PartialLeave_Request].[TeamLeader_Status]      ,[Att_PartialLeave_Request].[TeamLeader_Action_Date]      ,[Att_PartialLeave_Request].[DepManager_Id]      ,[Att_PartialLeave_Request].[DepManager_Status]      ,[Att_PartialLeave_Request].[DepManager_Action_Date]      ,[Att_PartialLeave_Request].[ParentDepManager_Id]      ,[Att_PartialLeave_Request].[ParentDepManager_Status]      ,[Att_PartialLeave_Request].[ParentDepManager_Action_Date]      ,[Att_PartialLeave_Request].[HR_Id]      ,[Att_PartialLeave_Request].[HR_Status]      ,[Att_PartialLeave_Request].[HR_Action_Date] from Att_PartialLeave_Request left join att_attendanceregister on Att_PartialLeave_Request.emp_id = att_attendanceregister.emp_id and Att_PartialLeave_Request.Partial_Leave_Date = att_attendanceregister.att_date inner join att_timetable   on att_attendanceregister.TimeTable_Id = att_timetable.TimeTable_Id inner join Set_EmployeeMaster on Att_PartialLeave_Request.emp_id = Set_EmployeeMaster.Emp_Id where Partial_Leave_Date>='" + Convert.ToDateTime(txtFromdate.Text) + "' and Partial_Leave_Date<='" + Convert.ToDateTime(txtTodate.Text) + "' and Set_EmployeeMaster.isactive='True'";

        strsql = strsql + " and Att_PartialLeave_Request.Emp_Id in (" + strEmpIds + ")";

        if (IsApprovalPerson)
        {
            strsql = strsql + " and ((Att_PartialLeave_Request.TeamLeader_Id = " + Session["EmpId"].ToString() + " or Att_PartialLeave_Request.TeamLeader_Id IS NULL ) or Att_PartialLeave_Request.DepManager_Id =" + Session["EmpId"].ToString() + " or Att_PartialLeave_Request.ParentDepManager_Id =" + Session["EmpId"].ToString() + " or Att_PartialLeave_Request.HR_Id=" + Session["EmpId"].ToString() + ")";
        }

        strsql = strsql + " order by  cast(Emp_Code as int),Att_PartialLeave_Request.Partial_Leave_Date";

        dt = objda.return_DataTable(strsql);

        if (ddlleaveType.SelectedIndex > 0)
        {
            if (ddlleaveType.SelectedValue.Trim() == "Auto")
            {
                dt = new DataView(dt, "Entered='Auto'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dt = new DataView(dt, "Entered=''", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        dt.Columns["HRState"].ReadOnly = false;


        for(int rCounter=0;rCounter<dt.Rows.Count;rCounter++)
        {
            try
            {
                string strArr = dt.Rows[rCounter]["Multiple_HR_Id"].ToString();
                string[] strArr1 = strArr.Split(',');
                bool bFlag = strArr1.Contains(Session["EmpId"].ToString());
                if (bFlag)
                {
                    dt.Rows[rCounter]["HRState"] = "true";
                }
            }
            catch(Exception ex)
            {

            }
          
        }
        dt.AcceptChanges();
        Session["FilterRecord"] = dt;
        objPageCmn.FillData((GridView)gvOverTime, dt, "", "");
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + str + "');", true);
    }

    protected void btnupdate_Click(object sender, EventArgs e)
    {
        bool isselected = false;

        foreach (GridViewRow gvrow in gvOverTime.Rows)
        {

            if (((CheckBox)gvrow.FindControl("chktl")).Enabled && ((CheckBox)gvrow.FindControl("chktl")).Checked)
            {
                isselected = true;
                objda.execute_Command("update Att_PartialLeave_Request set TeamLeader_Status='" + ((CheckBox)gvrow.FindControl("chktl")).Checked.ToString() + "',TeamLeader_Action_Date='" + DateTime.Now.ToString() + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
            }

            if (((CheckBox)gvrow.FindControl("chkDepManager")).Enabled && ((CheckBox)gvrow.FindControl("chkDepManager")).Checked)
            {
                isselected = true;
                objda.execute_Command("update Att_PartialLeave_Request set DepManager_Status='" + ((CheckBox)gvrow.FindControl("chkDepManager")).Checked.ToString() + "',DepManager_Action_Date='" + DateTime.Now.ToString() + "'   where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
            }

            if (((CheckBox)gvrow.FindControl("chkparentDepManager")).Enabled && ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked)
            {
                isselected = true;
                objda.execute_Command("update Att_PartialLeave_Request set ParentDepManager_Status='" + ((CheckBox)gvrow.FindControl("chkparentDepManager")).Checked.ToString() + "',ParentDepManager_Action_Date='" + DateTime.Now.ToString() + "',From_Time='" + ((TextBox)gvrow.FindControl("txtfromTime")).Text + "',To_Time='" + ((TextBox)gvrow.FindControl("txttoTime")).Text + "'    where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
            }

            if (((CheckBox)gvrow.FindControl("chkHR")).Enabled && ((CheckBox)gvrow.FindControl("chkHR")).Checked)
            {
                isselected = true;
                objda.execute_Command("update Att_PartialLeave_Request set HR_Id =  '"+ Session["EmpId"].ToString() +"' , HR_Status='" + ((CheckBox)gvrow.FindControl("chkHR")).Checked.ToString() + "',HR_Action_Date='" + DateTime.Now.ToString() + "',Is_Confirmed='Approved',From_Time='" + ((TextBox)gvrow.FindControl("txtfromTime")).Text + "',To_Time='" + ((TextBox)gvrow.FindControl("txttoTime")).Text + "'   where trans_id = '" + ((Label)gvrow.FindControl("lblTransId")).Text + "'");
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


    public string GetTimeDuration(string strFromTime, string strToTime)
    {
        DateTime dtfrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strFromTime.Split(':')[0]), Convert.ToInt32(strFromTime.Split(':')[1]), 0);
        DateTime dtto = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strToTime.Split(':')[0]), Convert.ToInt32(strToTime.Split(':')[1]), 0);

        if (dtfrom <= dtto)
        {
            //TimeSpan duration = DateTime.Parse(strToTime).Subtract(DateTime.Parse(strFromTime));
            int minutes = (((dtto - dtfrom).Hours) * 60) + (dtto - dtfrom).Minutes;
            if (minutes > 0)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes(minutes);
                string workHours = spWorkMin.ToString(@"hh\:mm");
                return workHours;
            }
        }
        else
        {

            int minutes = (new DateTime(dtfrom.Year, dtfrom.Month, dtfrom.Day, 23, 59, 0) - dtfrom).Minutes;
            minutes = ((new DateTime(dtfrom.Year, dtfrom.Month, dtfrom.Day, 23, 59, 0) - dtfrom).Hours * 60) + minutes;
            int minutes1 = (dtto - new DateTime(dtto.Year, dtto.Month, dtto.Day, 0, 0, 0)).Minutes;
            minutes1 = ((dtto - new DateTime(dtto.Year, dtto.Month, dtto.Day, 0, 0, 0)).Hours * 60) + minutes1;

            if ((minutes + minutes1) > 0)
            {
                TimeSpan spWorkMin = TimeSpan.FromMinutes((minutes + minutes1));
                string workHours = spWorkMin.ToString(@"hh\:mm");
                return workHours;
            }
        }

        return "00:00";


        //TimeSpan duration = DateTime.Parse(strToTime).Subtract(DateTime.Parse(strFromTime));

        //return duration.Hours.ToString() + ":" + duration.Minutes.ToString();
        //DateTime dtfrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strFromTime.Split(':')[0]), Convert.ToInt32(strFromTime.Split(':')[1]), 0);
        //DateTime dtto = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(strToTime.Split(':')[0]), Convert.ToInt32(strToTime.Split(':')[1]), 0);

        //return (Convert.ToDateTime(dtfrom) - Convert.ToDateTime(dtto)).ToString("HH:mm");
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

                objda.execute_Command("update Att_PartialLeave_Request set Is_Confirmed='Canceled',HR_Status='" + false.ToString() + "',HR_Action_Date='" + DateTime.Now.ToString() + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
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
            //if (((Label)gvrow.FindControl("lblStatus")).Text.Trim() == "" && ((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            //{
            //    dtEmphierarchy = new DataView(dtHierarchy, "Emp_id=" + ((Label)gvrow.FindControl("lblEmpId")).Text + "", "", DataViewRowState.CurrentRows).ToTable();

            //    if (dtEmphierarchy.Rows.Count > 0)
            //    {
            //        objda.execute_Command("update Att_PartialLeave_Request set Is_Confirmed='Pending',RequestDate='" + DateTime.Now.ToString() + "',TeamLeader_Id=" + dtEmphierarchy.Rows[0]["TLID"].ToString() + ",DepManager_Id=" + dtEmphierarchy.Rows[0]["SecManagerId"].ToString() + ",ParentDepManager_Id=" + dtEmphierarchy.Rows[0]["DepManagerId"].ToString() + ",HR_Id =" + strHRId + ",ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
            //        counter++;
            //    }
            //}

            if (((Label)gvrow.FindControl("lblStatus")).Text.Trim() == "Pending" && ((CheckBox)gvrow.FindControl("chkSelect")).Checked)
            {
                string strREmpId = ((Label)gvrow.FindControl("lblEmpId")).Text;

                try
                {
                    DataTable dtSDM = objda.return_DataTable("Select Set_DepartmentMaster.Parent_Id ,   Set_Location_Department.Emp_Id As 'Main_Manager', Set_Location_Department.Field1  As 'Main_Manager_Alt1', Set_Location_Department.Field2  As 'Main_Manager_Alt2' , Set_Location_Department.Field3  As 'Main_Manager_Alt3', Set_Location_Department.Field4  As 'Main_Manager_Alt4', Set_Location_Department.Field5  As 'Main_Manager_Alt5',Set_EmployeeMaster.Field5 as SectionManager  From  Set_EmployeeMaster   INNER JOIN  Set_Location_Department ON  Set_EmployeeMaster.Department_Id =  Set_Location_Department.Dep_Id AND  Set_EmployeeMaster.Location_Id = Set_Location_Department.Location_Id     INNER JOIN  Set_DepartmentMaster ON  Set_DepartmentMaster.Dep_Id  =Set_EmployeeMaster.Department_Id  Where  Set_EmployeeMaster. Emp_Id = '" + strREmpId + "'   ");

                    if (dtSDM.Rows.Count > 0)
                    {
                        counter++;
                        objda.execute_Command("update Att_PartialLeave_Request set Is_Confirmed='Pending',RequestDate='" + DateTime.Now.ToString() + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                        if (!((dtSDM.Rows[0]["SectionManager"].ToString() == "") || (dtSDM.Rows[0]["SectionManager"].ToString() == "")))
                        {
                            objda.execute_Command("update Att_PartialLeave_Request set DepManager_Id=" + dtSDM.Rows[0]["SectionManager"].ToString() + "  where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                        }

                        string strManagerId = "";
                        bool bFlag = false;
                        if (!((dtSDM.Rows[0]["Main_Manager"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager"].ToString() == "")))
                        {
                            strManagerId = dtSDM.Rows[0]["Main_Manager"].ToString();
                            objda.execute_Command("update Att_PartialLeave_Request set ParentDepManager_Id=" + strManagerId + "  where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                            bFlag = true;
                        }
                        if (!((dtSDM.Rows[0]["Main_Manager_Alt1"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt1"].ToString() == "0")))
                        {
                            if (bFlag)
                            {
                                strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt1"].ToString();
                            }
                            else
                            {
                                strManagerId = dtSDM.Rows[0]["Main_Manager_Alt1"].ToString();
                                bFlag = true;
                            }


                        }
                        if (!((dtSDM.Rows[0]["Main_Manager_Alt2"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt2"].ToString() == "0")))
                        {
                            if (bFlag)
                            {
                                strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt2"].ToString();
                            }
                            else
                            {
                                strManagerId = dtSDM.Rows[0]["Main_Manager_Alt2"].ToString();
                                bFlag = true;
                            }


                        }

                        if (!((dtSDM.Rows[0]["Main_Manager_Alt3"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt3"].ToString() == "0")))
                        {
                            if (bFlag)
                            {
                                strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt3"].ToString();
                            }
                            else
                            {
                                strManagerId = dtSDM.Rows[0]["Main_Manager_Alt3"].ToString();
                                bFlag = true;
                            }


                        }

                        if (!((dtSDM.Rows[0]["Main_Manager_Alt4"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt4"].ToString() == "0")))
                        {
                            if (bFlag)
                            {
                                strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt4"].ToString();
                            }
                            else
                            {
                                strManagerId = dtSDM.Rows[0]["Main_Manager_Alt4"].ToString();
                                bFlag = true;
                            }


                        }

                        if (!((dtSDM.Rows[0]["Main_Manager_Alt5"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt5"].ToString() == "0")))
                        {
                            if (bFlag)
                            {
                                strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt5"].ToString();
                            }
                            else
                            {
                                strManagerId = dtSDM.Rows[0]["Main_Manager_Alt5"].ToString();
                                bFlag = true;
                            }


                        }

                        if (bFlag)
                        {
                            objda.execute_Command("update Att_PartialLeave_Request set Multiple_ParentDepManager_Id='" + strManagerId + "'  where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                        }

                        if (!(dtSDM.Rows[0]["Parent_Id"].ToString() == "") || (dtSDM.Rows[0]["Parent_Id"].ToString() == "0"))
                        {


                            dtSDM = objda.return_DataTable("Select Set_DepartmentMaster.Parent_Id ,   Set_Location_Department.Emp_Id As 'Main_Manager', Set_Location_Department.Field1  As 'Main_Manager_Alt1', Set_Location_Department.Field2  As 'Main_Manager_Alt2' , Set_Location_Department.Field3  As 'Main_Manager_Alt3', Set_Location_Department.Field4  As 'Main_Manager_Alt4', Set_Location_Department.Field5  As 'Main_Manager_Alt5' From  Set_Location_Department   INNER JOIN  Set_DepartmentMaster ON   Set_DepartmentMaster.Dep_Id  =Set_Location_Department.Dep_Id   WHERE Set_Location_Department.Dep_Id  ='" + dtSDM.Rows[0]["Parent_Id"].ToString() + "' ");
                            if (dtSDM.Rows.Count > 0)
                            {



                                strManagerId = "";
                                bFlag = false;
                                if (!((dtSDM.Rows[0]["Main_Manager"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager"].ToString() == "0")))
                                {
                                    strManagerId = dtSDM.Rows[0]["Main_Manager"].ToString();
                                    objda.execute_Command("update Att_PartialLeave_Request set HR_Id=" + strManagerId + "  where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                                    bFlag = true;
                                }
                                if (!((dtSDM.Rows[0]["Main_Manager_Alt1"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt1"].ToString() == "0")))
                                {
                                    if (bFlag)
                                    {
                                        strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt1"].ToString();
                                    }
                                    else
                                    {
                                        strManagerId = dtSDM.Rows[0]["Main_Manager_Alt1"].ToString();
                                        bFlag = true;
                                    }


                                }
                                if (!((dtSDM.Rows[0]["Main_Manager_Alt2"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt2"].ToString() == "0")))
                                {
                                    if (bFlag)
                                    {
                                        strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt2"].ToString();
                                    }
                                    else
                                    {
                                        strManagerId = dtSDM.Rows[0]["Main_Manager_Alt2"].ToString();
                                        bFlag = true;
                                    }


                                }

                                if (!((dtSDM.Rows[0]["Main_Manager_Alt3"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt3"].ToString() == "0")))
                                {
                                    if (bFlag)
                                    {
                                        strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt3"].ToString();
                                    }
                                    else
                                    {
                                        strManagerId = dtSDM.Rows[0]["Main_Manager_Alt3"].ToString();
                                        bFlag = true;
                                    }


                                }

                                if (!((dtSDM.Rows[0]["Main_Manager_Alt4"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt4"].ToString() == "0")))
                                {
                                    if (bFlag)
                                    {
                                        strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt4"].ToString();
                                    }
                                    else
                                    {
                                        strManagerId = dtSDM.Rows[0]["Main_Manager_Alt4"].ToString();
                                        bFlag = true;
                                    }


                                }

                                if (!((dtSDM.Rows[0]["Main_Manager_Alt5"].ToString() == "") || (dtSDM.Rows[0]["Main_Manager_Alt5"].ToString() == "0")))
                                {
                                    if (bFlag)
                                    {
                                        strManagerId = strManagerId + "," + dtSDM.Rows[0]["Main_Manager_Alt5"].ToString();
                                    }
                                    else
                                    {
                                        strManagerId = dtSDM.Rows[0]["Main_Manager_Alt5"].ToString();
                                        bFlag = true;
                                    }


                                }

                                if (bFlag)
                                {
                                    objda.execute_Command("update Att_PartialLeave_Request set Multiple_HR_Id='" + strManagerId + "'  where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                                }
                            }
                        }
                    }

                }
                catch
                {

                }

                //if (dtEmphierarchy.Rows.Count > 0)
                //{
                //    objda.execute_Command("update Att_PartialLeave_Request set Is_Confirmed='Pending',RequestDate='" + DateTime.Now.ToString() + "',TeamLeader_Id=" + dtEmphierarchy.Rows[0]["TLID"].ToString() + ",DepManager_Id=" + dtEmphierarchy.Rows[0]["SecManagerId"].ToString() + ",ParentDepManager_Id=" + dtEmphierarchy.Rows[0]["DepManagerId"].ToString() + ",HR_Id =" + strHRId + ",ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where trans_id = " + ((Label)gvrow.FindControl("lblTransId")).Text + "");
                //    counter++;
                //}
            }
        }

        if (counter > 0)
        {
            DisplayMessage(counter.ToString() + " Request submitted successfully");
            btnGo_Click(null, null);
        }
        else
        {
            DisplayMessage("Request not submitted successfully , please check configuration or request already submitted");
        }


    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;


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

    protected void btntest_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "FreezeHeader()", true);
    }
}