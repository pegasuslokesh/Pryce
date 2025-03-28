using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Dashboard_AttendanceDashboard : BasePage
{
    NotificationMaster Obj_Notifiacation = null;
    DutyMaster DutyMaster = null;
    Att_Employee_Leave objEmpleave = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Leave_Request objleaveReq = null;
    EmployeeParameter objEmpParam = null;
    Att_PartialLeave_Request objPartial = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    EmployeeMaster objEmp = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    Set_Approval_Employee objEmpApproval = null;

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            if (Session["CompId"] == null || Session["BrandId"] == null || Session["LocId"] == null || Session["EmpId"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }
            else
            {
                //here we writing code to redirect related dashboard according login user module permission
                //start
                Common.clsApplicationModules _cls = (Common.clsApplicationModules)Session["clsApplicationModule"];
                if (_cls.isAttendanceModule)
                {
                    //no need to redirect
                }
                else if (_cls.isInventoryModule)
                {
                    string strobjectid = Common.GetObjectIdbyPageURL("../Inventory/Inv_Dashboard.aspx", Session["DBConnection"].ToString());

                    if (new DataView((DataTable)Session["dtAllModule1"], "Object_id=" + strobjectid + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        Response.Redirect("~/Inventory/Inv_Dashboard.aspx");
                    }

                }
                TL_Dashbaord.Visible = TL_Leave.Visible = _cls.isAttendanceModule;
                //end



                //here we are checing that current application is timeman or pryce
                //if application is time man then remider, task and duty list should not visible
                //created by jitendra on 13-12-2018
                if (Session["Application_Id"].ToString() == "1")
                {
                    Div_Duty_List.Visible = false;
                    Div_Task_List.Visible = false;
                    Div_Reminder_List.Visible = false;
                }
                Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
                DutyMaster = new DutyMaster(Session["DBConnection"].ToString());
                objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
                objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
                objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
                objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
                objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
                objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
                objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
                objDa = new DataAccessClass(Session["DBConnection"].ToString());
                objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());

                if (!IsPostBack)
                {

                    //Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
                    //DutyMaster = new DutyMaster(Session["DBConnection"].ToString());
                    //objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
                    //objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
                    //objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
                    //objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
                    //objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
                    //objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
                    //objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
                    //objDa = new DataAccessClass(Session["DBConnection"].ToString());
                    FillPageSizeddl();
                    FillPageSizeddl2();
                    FillPageSizeddl3();
                    DataTable dtDashbaord = (DataTable)Session["dtAllModule1"];

                    DataTable dtDashbaordTemp = new DataView(dtDashbaord, "Object_Id = 422", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDashbaordTemp.Rows.Count > 0)
                    {
                        OT_Request.Visible = true;

                    }
                    else
                    {

                        OT_Request.Visible = false;

                    }

                    dtDashbaordTemp = new DataView(dtDashbaord, "Object_Id = 425", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDashbaordTemp.Rows.Count > 0)
                    {
                        OT_Manipulation.Visible = true;

                    }
                    else
                    {

                        OT_Manipulation.Visible = false;

                    }

                    dtDashbaordTemp = new DataView(dtDashbaord, "Object_Id = 426", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDashbaordTemp.Rows.Count > 0)
                    {
                        AutoShortLeave.Visible = true;

                    }
                    else
                    {

                        AutoShortLeave.Visible = false;

                    }

                    dtDashbaordTemp = new DataView(dtDashbaord, "Object_Id = 427", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDashbaordTemp.Rows.Count > 0)
                    {
                        Short_Leave_Request.Visible = true;

                    }
                    else
                    {

                        Short_Leave_Request.Visible = false;

                    }


                    dtDashbaordTemp = new DataView(dtDashbaord, "Object_Id = 428", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDashbaordTemp.Rows.Count > 0)
                    {
                        TimeCard.Visible = true;

                    }
                    else
                    {

                        TimeCard.Visible = false;

                    }
                }
                GetSelf_Attendance();
                GetTeam_Attendance();
                GetSelf_Leave_Status();
                GetBirthday_Employee();
                GetLogs();
                GetLeave_Request_Status();
                Get_Duty();
                Get_Task();
                fillReminder();




            }
        }
        catch (Exception ex)
        {

        }
    }
    private void FillPageSizeddl()
    {
        ddlPageSize.Items.Add(new ListItem("10", "10"));
        ddlPageSize.Items.Add(new ListItem("20", "20"));
        ddlPageSize.Items.Add(new ListItem("30", "30"));
        ddlPageSize.Items.Add(new ListItem("40", "40"));
        ddlPageSize.Items.Add(new ListItem("50", "50"));
        ddlPageSize.Items.Add(new ListItem("60", "60"));
        ddlPageSize.Items.Add(new ListItem("70", "70"));
        ddlPageSize.Items.Add(new ListItem("80", "80"));
        ddlPageSize.Items.Add(new ListItem("90", "90"));
        ddlPageSize.Items.Add(new ListItem("100", "100"));
        int StrPageSize = int.Parse(Session["GridSize"].ToString());

        if (String.IsNullOrEmpty(StrPageSize.ToString()))
        {
            StrPageSize = 1;
        }
        else
        {
            if (StrPageSize % 10 != 0)
            {
                StrPageSize = StrPageSize + (10 - (StrPageSize % 10));
            }
        }
        ddlPageSize.SelectedValue = StrPageSize.ToString();
    }
    private void FillPageSizeddl2()
    {
        PresentPageNo.Items.Add(new ListItem("10", "10"));
        PresentPageNo.Items.Add(new ListItem("20", "20"));
        PresentPageNo.Items.Add(new ListItem("30", "30"));
        PresentPageNo.Items.Add(new ListItem("40", "40"));
        PresentPageNo.Items.Add(new ListItem("50", "50"));
        PresentPageNo.Items.Add(new ListItem("60", "60"));
        PresentPageNo.Items.Add(new ListItem("70", "70"));
        PresentPageNo.Items.Add(new ListItem("80", "80"));
        PresentPageNo.Items.Add(new ListItem("90", "90"));
        PresentPageNo.Items.Add(new ListItem("100", "100"));
        int StrPageSize = int.Parse(Session["GridSize"].ToString());
        if (String.IsNullOrEmpty(StrPageSize.ToString()))
        {
            StrPageSize = 1;
        }
        else
        {
            if (StrPageSize % 10 != 0)
            {
                StrPageSize = StrPageSize + (10 - (StrPageSize % 10));
            }
        }
        PresentPageNo.SelectedValue = StrPageSize.ToString();
    }
    private void FillPageSizeddl3()
    {
        LeavePageNo.Items.Add(new ListItem("10", "10"));
        LeavePageNo.Items.Add(new ListItem("20", "20"));
        LeavePageNo.Items.Add(new ListItem("30", "30"));
        LeavePageNo.Items.Add(new ListItem("40", "40"));
        LeavePageNo.Items.Add(new ListItem("50", "50"));
        LeavePageNo.Items.Add(new ListItem("60", "60"));
        LeavePageNo.Items.Add(new ListItem("70", "70"));
        LeavePageNo.Items.Add(new ListItem("80", "80"));
        LeavePageNo.Items.Add(new ListItem("90", "90"));
        LeavePageNo.Items.Add(new ListItem("100", "100"));
        int StrPageSize = int.Parse(Session["GridSize"].ToString());
        if (String.IsNullOrEmpty(StrPageSize.ToString()))
        {
            StrPageSize = 1;
        }
        else
        {
            if (StrPageSize % 10 != 0)
            {
                StrPageSize = StrPageSize + (10 - (StrPageSize % 10));
            }
        }
        LeavePageNo.SelectedValue = StrPageSize.ToString();
    }
    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            //(Convert.ToInt32(e.CommandArgument));
        }
    }
    protected void ddlPageSize_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        int currentpagesize = int.Parse(ddlPageSize.SelectedItem.Text.ToString());
        // int _TotalRowCount = 0;     
        if (ddlOption.SelectedIndex != 0)
        {
            int Page = currentpagesize;
            DataTable dt = (DataTable)Session["DtAbsent"];
            AbsentData.PageSize = Page;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0][0].ToString() + "";
            AbsentData.DataSource = dt;
            AbsentData.DataBind();

        }
    }
    protected void ddlPageSize_OnSelectedIndexChanged1(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        int currentpagesize = int.Parse(PresentPageNo.SelectedItem.Text.ToString());
        // int _TotalRowCount = 0;     
        if (ddlOption.SelectedIndex != 0)
        {
            int Page = currentpagesize;
            DataTable dt = (DataTable)Session["DtPresent"];
            PresentView.PageSize = Page;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0][0].ToString() + "";
            PresentView.DataSource = dt;
            PresentView.DataBind();
        }
    }
    protected void ddlPageSize_OnSelectedIndexChanged2(object sender, EventArgs e)
    {
        DropDownList ddl = sender as DropDownList;
        int currentpagesize = int.Parse(LeavePageNo.SelectedItem.Text.ToString());
        // int _TotalRowCount = 0;     
        if (ddlOption.SelectedIndex != 0)
        {
            int Page = currentpagesize;
            LeeaveGridView.PageSize = Page;
            DataTable DtLeave = (DataTable)Session["DtLeaveEmp"];
            LeeaveGridView.DataSource = DtLeave;
            LeeaveGridView.DataBind();
        }
    }

    protected void GvAbsent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        AbsentData.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtAbsent"];
        AbsentData.DataSource = dt;
        AbsentData.DataBind();
        //ShowPopup.Value= "True";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "<script>$(function () { $('#modelViewDetail').modal('show');});</script>", false);
        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "Modal_ViewInfo_Open();", true);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
    }
    protected void GvPresent_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PresentView.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtPresent"];
        PresentView.DataSource = dt;
        PresentView.DataBind();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
    }
    protected void GvLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        LeeaveGridView.PageIndex = e.NewPageIndex;
        DataTable DtLeave = (DataTable)Session["DtLeaveEmp"];
        LeeaveGridView.DataSource = DtLeave;
        LeeaveGridView.DataBind();
    }



    protected void GetSelf_Attendance()
    {
        DataTable dtSelfAttendance = objEmpleave.Get_Attendance_Dashboard(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), "0", "1");
        Ltr_Present.Text = dtSelfAttendance.Rows[0]["Present"].ToString() + "<small>/" + dtSelfAttendance.Rows[0]["Total_Days"].ToString() + "</small>";
        Ltr_Absent.Text = dtSelfAttendance.Rows[0]["Absent"].ToString() + "<small>/" + dtSelfAttendance.Rows[0]["Total_Days"].ToString() + "</small>";
        Ltr_Leave.Text = dtSelfAttendance.Rows[0]["Leave"].ToString() + "<small>/" + dtSelfAttendance.Rows[0]["Total_Days"].ToString() + "</small>";
        Ltr_Week_Off.Text = dtSelfAttendance.Rows[0]["Week_OFF"].ToString() + "<small>/" + dtSelfAttendance.Rows[0]["Total_Days"].ToString() + "</small>";
        Ltr_Holiday.Text = dtSelfAttendance.Rows[0]["Holiday"].ToString() + "<small>/" + dtSelfAttendance.Rows[0]["Total_Days"].ToString() + "</small>";
    }

    public DataTable GetDepId()
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        connection.Close();
        SqlCommand command = new SqlCommand();
        command = new SqlCommand("Get_Attendance_Dashboard", connection);
        command.CommandType = System.Data.CommandType.StoredProcedure;
        command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 12;
        command.Parameters.Add("@Emp_Id", SqlDbType.VarChar).Value = Session["EmpId"].ToString();
        command.Parameters.Add("@Company_Id", SqlDbType.VarChar).Value = StrCompId;
        command.Parameters.Add("@Location_Id", SqlDbType.VarChar).Value = StrLocId;
        command.Parameters.Add("@Brand_Id", SqlDbType.VarChar).Value = StrBrandId;
        command.Parameters.Add("@LogDays", SqlDbType.VarChar).Value = 0;
        connection.Open();
        DataTable dt = new DataTable();
        dt.Load(command.ExecuteReader());
        return dt;
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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

            DataTable dtCust = (DataTable)Session["DtAbsent"];
            DataTable dtPre = (DataTable)Session["DtPresent"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            DataView view1 = new DataView(dtPre, condition, "", DataViewRowState.CurrentRows);
            // Session["dtFilter_Bank__Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            PresentView.DataSource = view1;
            Session["DtPresent"] = view1;
            PresentView.DataBind();
            AbsentData.DataSource = view;
            Session["DtAbsent"] = view;
            AbsentData.DataBind();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "<script>$(function () { $('#modelViewDetail').modal('show');});</script>", false);
            //Common Function add By Lokesh on 12-05-2015
            //objPageCmn.FillData((object)gvBankMaster, view.ToTable(), "", "");
            // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
        }
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        GetTeam_Attendance();
        //Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "<script>$(function () { $('#modelViewDetail').modal('show');});</script>", false);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
    }

    protected void btnbindLeave_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlOption2.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOption2.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName2.SelectedValue + ",System.String)='" + txtValue2.Text.Trim() + "'";
            }
            else if (ddlOption2.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName2.SelectedValue + ",System.String) like '%" + txtValue2.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName2.SelectedValue + ",System.String) Like '%" + txtValue2.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtLeaveEmp"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            LeeaveGridView.DataSource = view;
            LeeaveGridView.DataBind();
            Session["DtLeaveEmp"] = view;
            // Session["dtFilter_Bank__Master"] = view.ToTable();
            TotalLeaves.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "<script>$(function () { $('#modelViewDetail2').modal('show');});</script>", false);
            //Common Function add By Lokesh on 12-05-2015
            //objPageCmn.FillData((object)gvBankMaster, view.ToTable(), "", "");
            // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
        }
        txtValue2.Focus();
    }
    protected void btnRefreshLeave_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        GetTeam_Attendance();
        //Session["CHECKED_ITEMS"] = null;
        ddlOption2.SelectedIndex = 2;
        ddlFieldName2.SelectedIndex = 0;
        txtValue2.Text = "";
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "<script>$(function () { $('#modelViewDetail2').modal('show');});</script>", false);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Modal_ViewInfo_Open();", true);
    }

    protected void GetTeam_Attendance()
    {
        string StrCompId = HttpContext.Current.Session["CompId"].ToString();
        string StrBrandId = HttpContext.Current.Session["BrandId"].ToString();
        string StrLocId = HttpContext.Current.Session["LocId"].ToString();
        string strAssignedlocation = Session["MyAllLoc"].ToString();
        // string StrLocId = strAssignedlocation.Remove(strAssignedlocation.Length - 1, 1);
        string Dep_Id = "";
        DataTable DepDt = GetDepId();
        if (DepDt.Rows.Count > 0)
        {
            Dep_Id = DepDt.Rows[0]["Department_ID"].ToString();
        }

        Session["DtLeaveEmp"] = null;
        Session["DtAbsent"] = null;
        Session["DtPresent"] = null;
        DataTable dtTeamTotal = objEmpleave.Get_Attendance_Dashboard(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), "0", "6");
        // GetAbsentData();  
        SqlConnection connection = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        if (dtTeamTotal.Rows[0][0].ToString() == "1" || dtTeamTotal.Rows[0][0].ToString() == "0")
        {
            if (Dep_Id == "29")
            {
                SqlCommand commands = new SqlCommand();
                commands = new SqlCommand("Get_Attendance_Dashboard", connection);
                commands.CommandType = System.Data.CommandType.StoredProcedure;
                commands.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 11;
                commands.Parameters.Add("@Emp_Id", SqlDbType.VarChar).Value = Session["EmpId"].ToString();
                commands.Parameters.Add("@Company_Id", SqlDbType.NVarChar).Value = StrCompId;
                commands.Parameters.Add("@Location_Id", SqlDbType.NVarChar).Value = strAssignedlocation;
                commands.Parameters.Add("@Brand_Id", SqlDbType.NVarChar).Value = StrBrandId;
                commands.Parameters.Add("@LogDays", SqlDbType.VarChar).Value = 0;
                connection.Open();
                DataTable dt2 = new DataTable();
                dt2.Load(commands.ExecuteReader());
                if (dt2.Rows.Count > 0)
                {
                    DataTable DtAbsent = new DataView(dt2, "LogStatus='Absent'", "", DataViewRowState.CurrentRows).ToTable();
                    DataTable DtPresent = new DataView(dt2, "LogStatus='Present'", "", DataViewRowState.CurrentRows).ToTable();
                    DataTable DtLeave = new DataView(dt2, "LogStatus='Leave'", "", DataViewRowState.CurrentRows).ToTable();
                    //var a = (object)LeeaveGridView;              
                    PresentView.DataSource = DtPresent;
                    Session["DtPresent"] = DtPresent;
                    PresentView.DataBind();
                    AbsentData.DataSource = DtAbsent;
                    Session["DtAbsent"] = DtAbsent;
                    AbsentData.DataBind();

                    lblTotalRecords.Text = DtPresent.Rows.Count + DtAbsent.Rows.Count.ToString();
                    Ltr_Present_Team.Text = DtPresent.Rows.Count + " / " + dt2.Rows.Count.ToString();
                    Ltr_Leave_Team.Text = DtLeave.Rows.Count + " / " + dt2.Rows.Count.ToString();
                    connection.Close();
                    TL_Leave.Visible = true;
                    if (DtLeave.Rows.Count > 0)
                    {
                        LeeaveGridView.DataSource = DtLeave;
                        LeeaveGridView.DataBind();
                        Session["DtLeaveEmp"] = DtLeave;
                        LeaveData.Visible = true;
                    }
                    else
                    {
                        LeeaveGridView.DataSource = null;
                        LeaveData.Visible = false;
                    }
                }
                else
                {
                    AbsentData.DataSource = null;
                    PresentView.DataSource = null;
                    LeeaveGridView.DataSource = null;
                    TL_Leave.Visible = false;
                }
            }
            else
            {
                AbsentData.DataSource = null;
                PresentView.DataSource = null;
                LeeaveGridView.DataSource = null;
                TL_Leave.Visible = false;
            }
        }
        else
        {
            connection.Close();
            SqlCommand command = new SqlCommand();
            command = new SqlCommand("Get_Attendance_Dashboard", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.Add("@Optype", SqlDbType.VarChar).Value = 10;
            command.Parameters.Add("@Emp_Id", SqlDbType.VarChar).Value = Session["EmpId"].ToString();
            command.Parameters.Add("@Company_Id", SqlDbType.NVarChar).Value = StrCompId;
            command.Parameters.Add("@Location_Id", SqlDbType.NVarChar).Value = StrLocId;
            command.Parameters.Add("@Brand_Id", SqlDbType.VarChar).Value = StrBrandId;
            command.Parameters.Add("@LogDays", SqlDbType.VarChar).Value = 0;
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            // DataTable Dtfilter = new DataView(dt, "Location_Id="+StrLocId +"", "", DataViewRowState.CurrentRows).ToTable();
            connection.Close();
            DataTable DtAbsent = new DataView(dt, "LogStatus='Absent'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable DtPresent = new DataView(dt, "LogStatus='Present'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable DtLeave = new DataView(dt, "LogStatus='Leave'", "", DataViewRowState.CurrentRows).ToTable();
            //var a = (object)LeeaveGridView;              
            PresentView.DataSource = DtPresent;
            Session["DtPresent"] = DtPresent;
            PresentView.DataBind();
            AbsentData.DataSource = DtAbsent;
            Session["DtAbsent"] = DtAbsent;
            AbsentData.DataBind();
            TotalLeaves.Text = Resources.Attendance.Total_Records + ": " + DtLeave.Rows.Count.ToString() + "";
            lblTotalRecords.Text = DtPresent.Rows.Count + DtAbsent.Rows.Count.ToString();
            Ltr_Present_Team.Text = DtPresent.Rows.Count + " / " + dtTeamTotal.Rows[0][0].ToString();
            Ltr_Leave_Team.Text = DtLeave.Rows.Count + " / " + dtTeamTotal.Rows[0][0].ToString();
            if (DtLeave.Rows.Count > 0)
            {
                LeeaveGridView.DataSource = DtLeave;
                LeeaveGridView.DataBind();
                Session["DtLeaveEmp"] = DtLeave;
                LeaveData.Visible = true;
            }
            else
            {
                LeeaveGridView.DataSource = null;
                LeaveData.Visible = false;
            }
        }

    }
    private void FillChild(string index)//fill up child nodes and respective child nodes of them 
    {
        DataTable Dt_Child = new DataTable();
        Dt_Child = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        Dt_Child = new DataView(Dt_Child, "Field5=" + index + "", "", DataViewRowState.CurrentRows).ToTable();



        int i = 0;
        while (i < Dt_Child.Rows.Count)
        {
            if (Session["EmpList"] == null)
            {
                Session["EmpList"] = Dt_Child.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                Session["EmpList"] = Session["EmpList"].ToString() + "," + Dt_Child.Rows[i]["Emp_Id"].ToString();
            }

            FillChild(Dt_Child.Rows[i]["Emp_Id"].ToString());

            i++;
        }

    }
    protected void GetSelf_Leave_Status()
    {
        DataTable Dt_Temp_Leave_Status = new DataTable();
        Dt_Temp_Leave_Status.Columns.Add("Leave_Status");
        Dt_Temp_Leave_Status.Columns.Add("Total_Days");
        Dt_Temp_Leave_Status.Columns.Add("Total_Remaining_Days");

        // Full Day Leave
        string empid = string.Empty;
        empid = Session["EmpId"].ToString();
        if (empid != "")
        {
            DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(empid);
            string months = string.Empty;
            DateTime FromDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            DateTime ToDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).AddMonths(2);
            while (FromDate <= ToDate)
            {
                months += FromDate.Month.ToString() + ",";
                FromDate = FromDate.AddMonths(1);
            }
            DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            int Financial_Year_Month = 0;
            if (dt.Rows.Count > 0)
            {
                Financial_Year_Month = int.Parse(dt.Rows[0]["Param_Value"].ToString());
            }
            DateTime FinancialYearStartDate = new DateTime();
            DateTime FinancialYearEndDate = new DateTime();
            if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < Financial_Year_Month)
            {
                FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, Financial_Year_Month, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            else
            {
                FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Financial_Year_Month, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            string year = string.Empty;
            string year1 = string.Empty;
            if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
            {
                year = FinancialYearStartDate.Year.ToString();
                year1 = year;
            }
            else
            {
                year += FinancialYearStartDate.Year.ToString() + ",";
                year1 = year;
                year += FinancialYearEndDate.Year.ToString() + ",";
            }
            DataTable dtleave = dtLeaveSummary;
            dtleave = new DataView(dtleave, "month='0' AND year in(" + year1 + ")", "", DataViewRowState.CurrentRows).ToTable();
            dtLeaveSummary = new DataView(dtLeaveSummary, "month in(" + months + ") AND  year in (" + year + ") ", "", DataViewRowState.CurrentRows).ToTable();
            dtLeaveSummary.Merge(dtleave);
            if (dtLeaveSummary.Rows.Count > 0)
            {
                foreach (DataRow dr in dtLeaveSummary.Rows)
                {
                    //string Percentage = Convert.ToString((Convert.ToDecimal(dr["Used_Days"]) / Convert.ToDecimal(dr["TotalAssignedLeave"])) * 100) + "%";
                    string Percentage = Convert.ToString((Convert.ToDecimal(dr["Used_Days"]) / Convert.ToDecimal(dr["Total_days"])) * 100) + "%";
                    //Dt_Temp_Leave_Status.Rows.Add("<div class='progress-group'><span class='progress-text'>" + dr["Leave_Name"].ToString() + "</span><span class='progress-number'><b>" + Math.Round(Convert.ToDouble(dr["Used_Days"].ToString()), 0) + "</b>/" + dr["TotalAssignedLeave"].ToString() + "</span><div class='progress sm'><div class='progress-bar progress-bar-aqua' style='width: " + Percentage + "'></div></div></div>");
                    Dt_Temp_Leave_Status.Rows.Add("<div class='progress-group'><span class='progress-text'>" + dr["Leave_Name"].ToString() + "</span><span class='progress-number'><b>" + Math.Round(Convert.ToDouble(dr["Used_Days"].ToString()), 0) + "</b>/" + dr["Remaining_Days"].ToString() + "</span><div class='progress sm'><div class='progress-bar progress-bar-aqua' style='width: " + Percentage + "'></div></div></div>");
                }
            }
        }
        //--------------------------------------------------------------------------
        // Partial Leave

        DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(empid, Session["CompId"].ToString());
        if (dtEmpParam.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtEmpParam.Rows[0]["Is_Partial_Enable"].ToString()))
            {
                DataTable dtPartial_Leave_summary = new DataTable();
                dtPartial_Leave_summary.Columns.Add("Total_Days");
                dtPartial_Leave_summary.Columns.Add("Used_Days");
                dtPartial_Leave_summary.Columns.Add("Pending_Days");
                dtPartial_Leave_summary.Columns.Add("Remaning_Days");

                int totalminutes = 0;
                int useinday = 0;
                int Pending = 0;
                double leaveCount = 0;

                totalminutes = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString());
                useinday = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());

                leaveCount = double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString()) / double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());

                leaveCount = System.Math.Round(leaveCount);

                DataRow dr = dtPartial_Leave_summary.NewRow();
                dr["Total_Days"] = leaveCount.ToString();

                DateTime StartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month, 1, 0, 0, 0);
                int TotalDays = DateTime.DaysInMonth(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month);
                DateTime EndDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month, TotalDays, 23, 59, 1);
                DataTable DtFilter = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
                DataTable DtApproved = new DataTable();
                try
                {
                    DtApproved = new DataView(DtFilter, "Partial_Leave_Date>='" + StartDate.ToString() + "' and Partial_Leave_Date<='" + EndDate.ToString() + "' and Emp_Id=" + empid + " and Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
                    dr["Used_Days"] = DtApproved.Rows.Count.ToString();
                    dtPartial_Leave_summary.Rows.Add(dr);
                }
                catch
                {
                    dr["Used_Days"] = "0";
                    dtPartial_Leave_summary.Rows.Add(dr);
                }

                try
                {
                    DataTable DtPending = new DataView(DtFilter, "Partial_Leave_Date>='" + StartDate.ToString() + "' and Partial_Leave_Date<='" + EndDate.ToString() + "' and Emp_Id=" + empid + " and Is_Confirmed='Pending' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
                    if (DtPending.Rows.Count == 0)
                    {
                        dr["Pending_Days"] = "0";
                    }
                    else
                    {
                        dr["Pending_Days"] = DtPending.Rows.Count.ToString();
                    }
                    Pending = Convert.ToInt32(dr["Pending_Days"].ToString());
                    dtPartial_Leave_summary.Rows.Add(dr);
                }
                catch (Exception Ex)
                {
                    //dr["Pending_Days"] ="0";
                    //dtPartial_Leave_summary.Rows.Add(dr);
                }

                try
                {
                    int RemailDays = Convert.ToInt32(leaveCount.ToString()) - (Convert.ToInt32(DtApproved.Rows.Count.ToString()) + Pending);
                    dr["Remaning_Days"] = RemailDays.ToString();
                    dtPartial_Leave_summary.Rows.Add(dr);
                }
                catch (Exception Ex)
                {

                }


                if (dtPartial_Leave_summary != null)
                {
                    if (dtPartial_Leave_summary.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dtPartial_Leave_summary.Rows)
                        {
                            string Percentage = Convert.ToString((Convert.ToDecimal(dr1["Used_Days"]) / Convert.ToDecimal(dr1["Total_Days"])) * 100) + "%";
                            Dt_Temp_Leave_Status.Rows.Add("<div class='progress-group'><span class='progress-text'>Partial Leave</span><span class='progress-number'><b>" + dr1["Used_Days"].ToString() + "</b>/" + dr1["Total_Days"].ToString() + "</span><div class='progress sm'><div class='progress-bar progress-bar-red' style='width: " + Percentage + "'></div></div></div>");
                        }
                    }
                }
            }
        }

        //------------------------------------------------------------------------------------------------------
        // Half Day
        string Year = string.Empty;
        DataTable dt_FYear = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt_FYear.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt_FYear.Rows[0]["Param_Value"].ToString());
        }
        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month >= FinancialYearMonth)
        {
            Year = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
        }
        else
        {
            Year = (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1).ToString();
        }
        DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(Session["EmpId"].ToString(), Year);
        if (dtEmpHalf.Rows.Count > 0)
        {
            foreach (DataRow dr1 in dtEmpHalf.Rows)
            {
                if (dr1["Used_Days"].ToString() != "0" && dr1["Total_Days"].ToString() != "0")
                {
                    string Percentage = Convert.ToString((Convert.ToDecimal(dr1["Used_Days"]) / Convert.ToDecimal(dr1["Total_Days"])) * 100) + "%";
                    Dt_Temp_Leave_Status.Rows.Add("<div class='progress-group'><span class='progress-text'>Half Days</span><span class='progress-number'><b>" + dr1["Used_Days"].ToString() + "</b>/" + dr1["Total_Days"].ToString() + "</span><div class='progress sm'><div class='progress-bar progress-bar-green' style='width: " + Percentage + "'></div></div></div>");
                }
                else
                {
                    string Percentage = "0%";
                    Dt_Temp_Leave_Status.Rows.Add("<div class='progress-group'><span class='progress-text'>Half Days</span><span class='progress-number'><b>" + dr1["Used_Days"].ToString() + "</b>/" + dr1["Total_Days"].ToString() + "</span><div class='progress sm'><div class='progress-bar progress-bar-green' style='width: " + Percentage + "'></div></div></div>");
                }
            }
        }
        //------------------------------------------------------------------------------------------------------

        //dt23.Rows.Add("<div class='progress-group'><span class='progress-text'>Privilege Leave(Yealy Leave)</span><span class='progress-number'><b>160</b>/200</span><div class='progress sm'><div class='progress-bar progress-bar-aqua' style='width: 20%'></div></div></div>");
        if (Dt_Temp_Leave_Status.Rows.Count > 0)
        {
            Rpt_Leave_Status.DataSource = Dt_Temp_Leave_Status;
            Rpt_Leave_Status.DataBind();
        }
        else
        {
            Rpt_Leave_Status.DataSource = null;
            Rpt_Leave_Status.DataBind();
        }
    }

    protected void GetBirthday_Employee()
    {
        DataTable dtBirthday = objEmpleave.Get_Attendance_Dashboard(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), "0", "2");
        Ltr_Bday_Count.Text = dtBirthday.Rows.Count.ToString() + " Members";

        DataTable Dt_Temp_Birthday = new DataTable();
        Dt_Temp_Birthday.Columns.Add("Birthday");

        foreach (DataRow Dr_Birthday in dtBirthday.Rows)
        {
            Dt_Temp_Birthday.Rows.Add("<li><img width = '50' src='" + getImageByEmpId(Dr_Birthday["Emp_Id"].ToString()) + "'><a class='users-list-name'>" + Dr_Birthday["Emp_Name"].ToString() + "</a><span class='users-list-date'>" + Convert.ToDateTime(Dr_Birthday["dob"]).ToString("dd MMM") + "</span></li>");
        }
        Rpt_Birthday.DataSource = Dt_Temp_Birthday;
        Rpt_Birthday.DataBind();
    }

    public string getImageByEmpId(object empid)
    {
        string img = string.Empty;
        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());

        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Emp_Image"].ToString() != "")
            {
                if (File.Exists(Server.MapPath("../CompanyResource/" + Session["CompId"] + "/" + dt.Rows[0]["Emp_Image"].ToString())) == true)
                {
                    img = "../CompanyResource/" + Session["CompId"] + "/" + dt.Rows[0]["Emp_Image"].ToString();
                }
                else
                {
                    img = "../Bootstrap_Files/dist/img/Bavatar.png";
                }
            }
            else
            {
                img = "../Bootstrap_Files/dist/img/Bavatar.png";
            }
        }
        return img;
    }

    protected void GetLogs()
    {
        DataTable dtLogs = objEmpleave.Get_Attendance_Dashboard(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), "1", "3");

        DataTable Dt_Temp_Log = new DataTable();
        Dt_Temp_Log.Columns.Add("Log_Time");
        Dt_Temp_Log.Columns.Add("Type");
        Dt_Temp_Log.Columns.Add("Status");
        Dt_Temp_Log.Columns.Add("Date");

        foreach (DataRow Dr_Log in dtLogs.Rows)
        {
            Dt_Temp_Log.Rows.Add(Convert.ToDateTime(Dr_Log["Event_Time"]).ToString("HH:mm"), Dr_Log["Type"], "Active", Convert.ToDateTime(Dr_Log["Event_Time"]).ToString("dd MMM yyyy"));
        }
        Rpt_Log.DataSource = Dt_Temp_Log;
        Rpt_Log.DataBind();



        //DataTable dtShortLeave = objEmpleave.GenerateDashboardShortLeave(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString());
        //rpt_short_leave.DataSource = dtShortLeave;
        //rpt_short_leave.DataBind();


        DataTable dtAppliedShortLeave = objEmpleave.DashboardAppliedShortLeave(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString());


        for (int count = 0; count < dtAppliedShortLeave.Rows.Count; count++)
        {
            if (dtAppliedShortLeave.Rows[count]["Is_Confirmed"].ToString() == "Pending")
            {
                dtAppliedShortLeave.Rows[count]["Is_Confirmed"] = "<span class='label label-warning'>" + dtAppliedShortLeave.Rows[count]["Is_Confirmed"].ToString() + "</span>";
            }
            else if (dtAppliedShortLeave.Rows[count]["Is_Confirmed"].ToString() == "Approved")
            {
                dtAppliedShortLeave.Rows[count]["Is_Confirmed"] = "<span class='label label-success'>" + dtAppliedShortLeave.Rows[count]["Is_Confirmed"].ToString() + "</span>";
            }
            else if (dtAppliedShortLeave.Rows[count]["Is_Confirmed"].ToString() == "Rejected")
            {
                dtAppliedShortLeave.Rows[count]["Is_Confirmed"] = "<span class='label label-danger'>" + dtAppliedShortLeave.Rows[count]["Is_Confirmed"].ToString() + "</span>";
            }
        }

        dtAppliedShortLeave.AcceptChanges();


        rpt_applied_short_leave.DataSource = dtAppliedShortLeave;
        rpt_applied_short_leave.DataBind();

    }
    protected void GetLeave_Request_Status()
    {
        DataTable dt_Leave_Status = objEmpleave.Get_Attendance_Dashboard(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), "0", "4");

        //dt_Leave_Status.Select("ModifiedDate DESC");
        dt_Leave_Status = new DataView(dt_Leave_Status, "", "ModifiedDate DESC", DataViewRowState.CurrentRows).ToTable();

        //dt_Leave_Status.DefaultView.Sort = "ModifiedDate DESC";

        DataTable Dt_Temp_Leave_Status = new DataTable();
        Dt_Temp_Leave_Status.Columns.Add("EmpCode");
        Dt_Temp_Leave_Status.Columns.Add("EmpName");
        Dt_Temp_Leave_Status.Columns.Add("FromDate");
        Dt_Temp_Leave_Status.Columns.Add("ToDate");
        Dt_Temp_Leave_Status.Columns.Add("Status");
        Dt_Temp_Leave_Status.Columns.Add("StatusOriginal");
        Dt_Temp_Leave_Status.Columns.Add("Leave_Type");
        Dt_Temp_Leave_Status.Columns.Add("Reason");

        foreach (DataRow Dr_Leave_Stauts in dt_Leave_Status.Rows)
        {
            string Status = string.Empty;
            if (Dr_Leave_Stauts["Status"].ToString() == "Pending")
                Status = "<span class='label label-warning'>" + Dr_Leave_Stauts["Status"].ToString() + "</span>";
            else if (Dr_Leave_Stauts["Status"].ToString() == "Approved")
                Status = "<span class='label label-success'>" + Dr_Leave_Stauts["Status"].ToString() + "</span>";
            else if (Dr_Leave_Stauts["Status"].ToString() == "Denied")
                Status = "<span class='label label-danger'>" + Dr_Leave_Stauts["Status"].ToString() + "</span>";
            if (Dr_Leave_Stauts["Emp_Description"].ToString().Length > 100)
                Dt_Temp_Leave_Status.Rows.Add(Dr_Leave_Stauts["Emp_Code"].ToString(), Dr_Leave_Stauts["EmpName"].ToString(), Convert.ToDateTime(Dr_Leave_Stauts["From_Date"]).ToString("dd MMM yyyy"), Convert.ToDateTime(Dr_Leave_Stauts["To_Date"]).ToString("dd MMM yyyy"), Status, Dr_Leave_Stauts["Status"].ToString(), Dr_Leave_Stauts["Leave_Name"].ToString(), Dr_Leave_Stauts["Emp_Description"].ToString().Substring(0, 100));
            else
                Dt_Temp_Leave_Status.Rows.Add(Dr_Leave_Stauts["Emp_Code"].ToString(), Dr_Leave_Stauts["EmpName"].ToString(), Convert.ToDateTime(Dr_Leave_Stauts["From_Date"]).ToString("dd MMM yyyy"), Convert.ToDateTime(Dr_Leave_Stauts["To_Date"]).ToString("dd MMM yyyy"), Status, Dr_Leave_Stauts["Status"].ToString(), Dr_Leave_Stauts["Leave_Name"].ToString(), Dr_Leave_Stauts["Emp_Description"].ToString());
        }
        Rpt_Leave_Request_Status.DataSource = Dt_Temp_Leave_Status;
        Rpt_Leave_Request_Status.DataBind();


        foreach (RepeaterItem gvr in Rpt_Leave_Request_Status.Items)
        {
            HiddenField hdnLeaveStatus = (HiddenField)gvr.FindControl("hdnLeaveStatus");
            Label lblLeaveType = (Label)gvr.FindControl("lblLeave_Type");
            Label lblPendingLink = (Label)gvr.FindControl("lblStatus");
            HyperLink hypPendingLink = (HyperLink)gvr.FindControl("hypApprovalPage");

            if (lblLeaveType.Text != "Half Day" && lblLeaveType.Text != "Partial Leave")
            {
                if (hdnLeaveStatus.Value == "Pending")
                {
                    DataTable dt = objEmpApproval.getApprovalTypeByEmpId(Session["EmpId"].ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt = new DataView(dt, "Approval_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lblPendingLink.Visible = false;
                            hypPendingLink.Visible = true;
                            hypPendingLink.NavigateUrl = "~/MasterSetUp/EmployeeApproval.aspx?Type=DashLeave";
                        }
                        else
                        {
                            lblPendingLink.Visible = true;
                            hypPendingLink.Visible = false;
                        }
                    }
                    else
                    {
                        lblPendingLink.Visible = true;
                        hypPendingLink.Visible = false;
                    }
                }
                else
                {
                    lblPendingLink.Visible = true;
                    hypPendingLink.Visible = false;
                }
            }
            else if (lblLeaveType.Text == "Half Day")
            {
                if (hdnLeaveStatus.Value == "Pending")
                {
                    DataTable dt = objEmpApproval.getApprovalTypeByEmpId(Session["EmpId"].ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt = new DataView(dt, "Approval_Id='2'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lblPendingLink.Visible = false;
                            hypPendingLink.Visible = true;
                            hypPendingLink.NavigateUrl = "~/MasterSetUp/EmployeeApproval.aspx?Type=DashHalf";
                        }
                        else
                        {
                            lblPendingLink.Visible = true;
                            hypPendingLink.Visible = false;
                        }
                    }
                    else
                    {
                        lblPendingLink.Visible = true;
                        hypPendingLink.Visible = false;
                    }
                }
                else
                {
                    lblPendingLink.Visible = true;
                    hypPendingLink.Visible = false;
                }
            }
            else if (lblLeaveType.Text == "Partial Leave")
            {
                if (hdnLeaveStatus.Value == "Pending")
                {
                    DataTable dt = objEmpApproval.getApprovalTypeByEmpId(Session["EmpId"].ToString());
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt = new DataView(dt, "Approval_Id='3'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            lblPendingLink.Visible = false;
                            hypPendingLink.Visible = true;
                            hypPendingLink.NavigateUrl = "~/MasterSetUp/EmployeeApproval.aspx?Type=DashPartial";
                        }
                        else
                        {
                            lblPendingLink.Visible = true;
                            hypPendingLink.Visible = false;
                        }
                    }
                    else
                    {
                        lblPendingLink.Visible = true;
                        hypPendingLink.Visible = false;
                    }
                }
                else
                {
                    lblPendingLink.Visible = true;
                    hypPendingLink.Visible = false;
                }
            }
            else
            {
                lblPendingLink.Visible = true;
                hypPendingLink.Visible = false;
            }
        }

    }
    protected void Get_Duty()
    {
        string Task = string.Empty;
        DataTable dt_Task = DutyMaster.Get_Duty_Chart("0", Session["EmpID"].ToString(), "0", "", "0", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "0", "0", "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "6");
        if (dt_Task.Rows.Count > 0)
        {
            foreach (DataRow DR in dt_Task.Rows)
            {
                if (Task == "")
                {
                    Task = "<li><span class='handle'><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i></span><span class='text'>" + DR["Title"].ToString() + "</span><small class='label label-danger'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Effect_Date_From"].ToString() + "</small><small class='label label-primary'><i class='fa fa-clock - o'></i>&nbsp;&nbsp;" + GetCycle(DR["Duty_Cycle"].ToString()) + "</small><small class='label label-info'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Effect_Date_To"].ToString() + "</small></li>";
                }
                else
                {
                    Task = Task + "<li><span class='handle'><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i></span><span class='text'>" + DR["Title"].ToString() + "</span><small class='label label-danger'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Effect_Date_From"].ToString() + "</small><small class='label label-primary'><i class='fa fa-clock - o'></i>&nbsp;&nbsp;" + GetCycle(DR["Duty_Cycle"].ToString()) + "</small><small class='label label-info'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Effect_Date_To"].ToString() + "</small></li>";
                }
                // Send_Notification_Duty(DR["Title"].ToString(), DR["Effect_Date_From"].ToString(), DR["Duty_Cycle"].ToString(),DR["Employee_ID"].ToString(), DR["Effect_Date_To"].ToString(),DR["Report_To"].ToString());
            }
            Ltr_Duty.Text = Task;
        }
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

    //protected void Send_Notification_Duty(string Title,string Effect_Date_From,string Duty_Cycle,string Employee_ID,string Effect_Date_To,string Report_To)
    //{
    //    int Save_Notification = 0;
    //    DataTable Dt_Request_Type = new DataTable();
    //    Dt_Request_Type = Obj_Notifiacation.Get_Notification_Type_Request("2");
    //    string Request_URL = "../Duty_Master/Duty_Feedback.aspx";
    //    string Message = string.Empty;
    //    Message = "Duty " + Title + " assign for " + GetEmployeeName(Employee_ID) + " on  " + Effect_Date_From + " and Due Date is " + Effect_Date_To + " Duty Cycle is " + Duty_Cycle;
    //    if (Report_To != "")
    //    {
    //        string[] Report_To_Send = Report_To.ToString().Split(',');
    //        for (int i = 0; i < Report_To_Send.Count(); i++)
    //        {
    //            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Report_To_Send[i].ToString(), Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), "0", "17");
    //        }
    //    }
    //    Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), "0", "17");
    //}

    protected void Get_Task()
    {
        string Task = string.Empty;
        DataTable dt_Task = DutyMaster.Get_Duty_Chart("0", Session["EmpID"].ToString(), "0", "", "0", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "0", "0", "0", "False", "True", Session["UserId"].ToString(), Session["UserId"].ToString(), "7");
        if (dt_Task.Rows.Count > 0)
        {
            foreach (DataRow DR in dt_Task.Rows)
            {
                if (Task == "")
                {
                    Task = "<li><span class='handle'><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i></span><span class='text'>" + DR["Title"].ToString() + "</span><small class='label label-danger'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Start_Date"].ToString() + "</small><small class='label label-info'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Due_Date"].ToString() + "</small></li>";
                }
                else
                {
                    Task = Task + "<li><span class='handle'><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i></span><span class='text' tool >" + DR["Title"].ToString() + "</span><small class='label label-danger'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Start_Date"].ToString() + "</small><small class='label label-info'><i class='fa fa-clock-o'></i>&nbsp;&nbsp;" + DR["Due_Date"].ToString() + "</small></li>";
                }
                // Send_Notification_Task(DR["Title"].ToString(), DR["Start_Date"].ToString(), DR["Assign_To"].ToString(), DR["Due_Date"].ToString());
            }
            Ltr_Task.Text = Task;
        }
    }

    //protected void Send_Notification_Task(string Title, string Start_Date, string Assign_To, string Due_Date)
    //{
    //    int Save_Notification = 0;
    //    DataTable Dt_Request_Type = new DataTable();
    //    Dt_Request_Type = Obj_Notifiacation.Get_Notification_Type_Request("4");
    //    string Request_URL = "../Duty_Master/Task.aspx";
    //    string Message = string.Empty;
    //    Message = "Task " + Title + " assign for " + GetEmployeeName(Assign_To) + ". Task Start Date  " + Start_Date + " and Due Date is " + Due_Date;        
    //    Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["Trans_ID"].ToString(), Request_URL, "", "0", "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), "0", "17");
    //}


    public void fillReminder()
    {
        string reminder = string.Empty;
        //DataTable dt_ReminderData = new Reminder().AllDataByUserID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString());

        //style 1 to show data
        DataTable dt_ReminderData = new NotificationMaster(Session["DBConnection"].ToString()).Get_ReminderNotification_By_EmpId(Session["EmpId"].ToString(), "10");
        if (dt_ReminderData.Rows.Count > 0)
        {
            foreach (DataRow DR in dt_ReminderData.Rows)
            {
                if (reminder == "")
                {
                    reminder = "<li><span class='handle'><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i></span><span class='text'>    <a href=" + DR["Link_url"].ToString() + " style='color:black;' title=*" + DR["N_Message"].ToString() + "*> " + DR["N_Message"].ToString() + "</a>  </span></li>";
                }
                else
                {
                    reminder = reminder + "<li><span class='handle'><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i><i class='fa fa-ellipsis-v'></i></span><span class='text' tool ><a href=" + DR["Link_url"].ToString() + " style='color:black;' title=*" + DR["N_Message"].ToString() + "*> " + DR["N_Message"].ToString() + "</a>  </span></li>";
                }

            }

            reminder = reminder.Replace('*', '"');

            Lit_Reminder.Text = reminder;
        }

        //style 2

        //NotificationMaster Obj_Notification = new NotificationMaster();
        //DataTable Dt_reminder = Obj_Notification.Get_ReminderNotification_By_EmpId(Session["EmpId"].ToString(), ViewState["More_Reminder"].ToString());

        //string number = "";
        //int IsReadNo = 0;
        //DataView dv = new DataView(Dt_reminder, "Is_read='False'", "", DataViewRowState.CurrentRows);
        //IsReadNo = dv.Count;

        //if (IsReadNo == 0)
        //{
        //    number = "";
        //}
        //else
        //{
        //    number = IsReadNo.ToString();
        //}

        //string demo = "";




        //for (int i = 0; i < Dt_reminder.Rows.Count; i++)
        //{
        //    demo = demo + "<li> <a href=" + Dt_reminder.Rows[i]["Link_url"].ToString() + " title=*" + Dt_reminder.Rows[i]["N_Message"].ToString() + "*> <i class=*fa fa-users text-aqua*></i> " + Dt_reminder.Rows[i]["N_Message"].ToString() + " </a> </li>";
        //}

        //if (numberOfRecords != 0)
        //    Start = "<li id=*Li_Remider_Menu* class=*dropdown notifications-menu*> <a href=*#* onclick=*Read_Reminder_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*> <i class=*fa fa-clock-o*></i> <span class=*label label-warning*>" + number + "</span> </a> <ul class=*dropdown-menu*> <li class=*header*>All Reminder Logs</li> <li> <!-- inner menu: contains the actual data --> <ul class=*menu*> " + demo + "  </ul> </li> <li class=*footer*><a href=*#*  onclick=*View_More_Reminder()*>View More</a></li> </ul> </li>                     <li id=*Li_Menu* class=*dropdown messages-menu*><a id=*A_Dropdown* href = *#* onclick=*Read_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*><i class=*fa fa-bell-o*></i><span class=*label label-success*>" + numberOfRecords + "</span></a><ul class=*dropdown-menu*><li><ul class=*menu*>";
        //else
        //    Start = "<li id=*Li_Remider_Menu* class=*dropdown notifications-menu*> <a href=*#* onclick=*Read_Reminder_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*> <i class=*fa fa-clock-o*></i> <span class=*label label-warning*>" + number + "</span> </a> <ul class=*dropdown-menu*> <li class=*header*>All Remider Logs</li> <li> <!-- inner menu: contains the actual data --> <ul class=*menu*> " + demo + " </ul> </li> <li class=*footer*><a href=*#*  onclick=*View_More_Reminder()*>View More</a></li> </ul> </li>                      <li id=*Li_Menu* class=*dropdown messages-menu*><a id=*A_Dropdown* href = *#* onclick=*Read_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*><i class=*fa fa-bell-o*></i></a><ul class=*dropdown-menu*><li><ul class=*menu*>";


        //End = "</ul></li></li><li class=*footer*><a href=*#* onclick=*View_More()*>More</a></li></ul></li>";


        // style 3
        //DataTable dt_ReminderData = new Reminder().AllDataByUserID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString());
        //if (dt_ReminderData.Rows.Count > 0)
        //{
        //    listReminder.DataSource = dt_ReminderData;
        //    listReminder.DataBind();
        //}
    }

}