using AjaxControlToolkit;
using PegasusDataAccess;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_LeaveApproval : BasePage
{
    Common cmn = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    Att_AttendanceLog objAttLog = null;
    Attendance objAttendance = null;
    Att_Leave_Request objleaveReq = null;
    HolidayMaster objHoliday = null;
    Att_Employee_Leave objEmpleave = null;
    EmployeeMaster objEmp = null;
    LeaveMaster objleave = null;
    Set_Employee_Holiday objEmpHoliday = null;
    SystemParameter objSys = null;
    Set_Approval_Employee objApproalEmp = null;
    Set_ApplicationParameter objAppParam = null;
    Att_ScheduleMaster objEmpSch = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    Att_HalfDay_Request objHalfDay = null;
    Att_PartialLeave_Request objPartial = null;
    Set_ApprovalMaster ObjApproval = null;
    SendMailSms ObjSendMailSms = null;
    CompanyMaster objComp = null;
    DataAccessClass ObjDa = null;
    NotificationMaster Obj_Notifiacation = null;
    Set_Approval_Employee objApprovalEmp = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        btnApply.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnApply, "").ToString());

        cmn = new Common(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objHoliday = new HolidayMaster(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objleave = new LeaveMaster(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objApprovalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Session["empimgpath"] = null;
            txtEmpName.Focus();
            //FillLeaveStatus();
            rbtnYearly.Checked = true;
            rbtnYearly.Visible = false;
            txtOTAssignDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");

            if (Request.QueryString["Emp_Id"] != null)
            {
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Request.QueryString["Emp_Id"] == null ? "56" : "52", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }

                txtEmpName.Text = Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                hdnEmpId.Value = Session["EmpId"].ToString();
                txtEmpName.Enabled = false;
                FillLeaveSummary(Session["EmpId"].ToString());
                FillApproveLeave(Session["EmpId"].ToString());
                FillPendingLeave(Session["EmpId"].ToString());
                lblHeader.Text = Resources.Attendance.Employee_Leave_Request_Setup;
                ddlLoc.Visible = false;
                Update_OT.Visible = false;
                ChkExtraLeave.Visible = false;
                Li_OTLeave.Visible = false;
            }
            else
            {
                ChkExtraLeave.Visible = true;
                ddlLoc.Visible = true;
                objPageCmn.fillLocationWithAllOption(ddlLoc);
                FillLeaveStatus();
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "56", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                lblHeader.Text = Resources.Attendance.HR_Leave_Request;
            }

            CalendarExtendertxtValueDate.Format = objSys.SetDateFormat();
        }

        CalendarExtender1.Format = objSys.SetDateFormat();
        CalendarExtender2.Format = objSys.SetDateFormat();
        CalendarExtender3.Format = objSys.SetDateFormat();
        CalendarExtender4.Format = objSys.SetDateFormat();
        CalendarExtender5.Format = objSys.SetDateFormat();       
        AllPageCode();
        if (Settings.IsValid == false)
        {
            btnApply.Visible = false;
        }
    }
    protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
        AllPageCode();
    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        DataTable dtModule = new DataTable();
        DataTable dtAllPageCode = new DataTable();
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        if (Request.QueryString["Emp_Id"] == null)
        {
            dtModule = objObjectEntry.GetModuleIdAndName("56", (DataTable)Session["ModuleName"]);
        }
        else
        {
            dtModule = objObjectEntry.GetModuleIdAndName("52", (DataTable)Session["ModuleName"]);
        }
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code


        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        Page.Title = objSys.GetSysTitle();
        if (Session["EmpId"].ToString() == "0")
        {
            if (Lbl_Tab_New.Text != Resources.Attendance.View)
            {
                btnApply.Visible = true;
            }
            else
            {
                btnApply.Visible = false;

            }
            try
            {
                Li_OTLeave.Visible = true;
                Update_OT.Visible = true;
                //gvLeaveSummary.Columns[7].Visible = true;
                gvLeaveStatus.Columns[0].Visible = true;
                gvLeaveStatus.Columns[1].Visible = true;
                // gvLeaveStatus.Columns[2].Visible = true;
            }
            catch
            {

            }
        }
        else
        {
            if (Request.QueryString["Emp_Id"] == null)
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "56", HttpContext.Current.Session["CompId"].ToString());
            }
            else
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "52", HttpContext.Current.Session["CompId"].ToString());
            }

            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {

                    if (Lbl_Tab_New.Text != Resources.Attendance.View)
                    {
                        btnApply.Visible = true;
                    }
                    else
                    {
                        btnApply.Visible = false;
                    }
                    try
                    {
                        Li_OTLeave.Visible = true;
                        Update_OT.Visible = true;
                        //gvLeaveSummary.Columns[7].Visible = true;
                        gvLeaveStatus.Columns[0].Visible = true;
                        gvLeaveStatus.Columns[1].Visible = true;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {

                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            if (Lbl_Tab_New.Text != Resources.Attendance.View)
                            {
                                btnApply.Visible = true;
                            }
                            else
                            {
                                btnApply.Visible = false;
                            }
                        }
                        if (DtRow["Op_Id"].ToString() == "11")
                        {

                            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Allow Over Time Convert In Leaves", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                            {
                                Li_OTLeave.Visible = true;
                                Update_OT.Visible = true;
                                //gvLeaveSummary.Columns[7].Visible = true;
                            }
                            else
                            {
                                // gvLeaveSummary.Columns[7].Visible = false;
                                Update_OT.Visible = false;
                                Li_OTLeave.Visible = false;
                            }
                        }

                        //if (DtRow["Op_Id"].ToString() == "2")
                        //{
                        //    try
                        //    {
                        //        gvLeaveStatus.Columns[2].Visible = true;
                        //    }
                        //    catch
                        //    {
                        //    }
                        //}
                        if (DtRow["Op_Id"].ToString() == "5")
                        {
                            try
                            {
                                gvLeaveStatus.Columns[1].Visible = true;
                            }
                            catch
                            {
                            }

                        }
                        if (DtRow["Op_Id"].ToString() == "6")
                        {
                            try
                            {
                                gvLeaveStatus.Columns[0].Visible = true;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public string GetLeaveStatus(object TransId)
    {
        string status = string.Empty;
        //Commented by Ghanshyam Suthar on 06/12/2017
        //DataTable dt = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        DataTable dt = objleaveReq.GetLeaveRequest_ByTrans_ID(Session["CompId"].ToString(), "0", TransId.ToString());
        if (dt.Rows.Count > 0)
        {
            //Commented by Ghanshyam Suthar on 06/12/2017
            //dt = new DataView(dt, "Trans_Id='" + TransId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dt.Rows.Count > 0)
            //{
            if (Convert.ToBoolean(dt.Rows[0]["Is_Pending"].ToString()))
            {
                status = "Pending";
            }
            else if (Convert.ToBoolean(dt.Rows[0]["Is_Approved"].ToString()))
            {
                status = "Approved";
            }
            else if (Convert.ToBoolean(dt.Rows[0]["Is_Canceled"].ToString()))
            {
                status = "Rejected";
            }
        }
        return status;
    }
    protected void gvLeaveStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveStatus.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["Leave_DtLeaveStatus1_Filter"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLeaveStatus, dt, "", "");
    }
    public void FillLeaveStatus()
    {

        //Commented by Ghanshyam Suthar on 06/12/2017
        // DataTable dtLeave = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        DataTable dtLeave = new DataTable();
        if (Request.QueryString["Emp_Id"] == null)
        {
            dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id,Set_EmployeeMaster.Emp_Code, Set_EmployeeMaster.Emp_Name,(select Count(Trans_Id) from Att_Leave_Request_Child where Att_Leave_Request_Child.ref_Id=Att_Leave_Request.trans_id) as DaysCount,cast(Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_Leave_Request.From_Date, Att_Leave_Request.To_date,case when Att_LeaveRequest_header.Is_Pending = 1 then 'Pending' when Att_LeaveRequest_header.Is_Approved = 1 then 'Approved' when Att_LeaveRequest_header.Is_Canceled = 1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7,Att_Leave_Request.Field4, Att_Leave_Request.Field5 from Att_LeaveRequest_header inner join Att_Leave_Request on Att_Leave_Request.Field2 = Att_LeaveRequest_header.Trans_Id inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id in (" + ddlLoc.SelectedValue.Trim() + ") and Set_EmployeeMaster.Field2 = 'False' order by cast(Set_EmployeeMaster.Emp_Code as int), From_date desc");
            //dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name , Att_LeaveRequest_header.TotalDays as DaysCount, cast( Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Is_Pending=1 then 'Pending' when Is_Approved=1 then 'Approved' when Is_Canceled=1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7 from Att_LeaveRequest_header inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id in (" + ddlLoc.SelectedValue.Trim() + ") and Set_EmployeeMaster.Field2='False'  order by cast(Set_EmployeeMaster.Emp_Code as int),   From_date desc  ");
            //dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code, Set_EmployeeMaster.Emp_Name, (select Count(Trans_Id) from Att_Leave_Request_Child where Att_Leave_Request_Child.ref_Id=Att_Leave_Request.trans_id) as DaysCount,cast(Att_LeaveRequest_header.CreatedDate as date) as Application_Date, Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Att_LeaveRequest_header.Is_Pending = 1 then 'Pending' when Att_LeaveRequest_header.Is_Approved = 1 then 'Approved'when Att_LeaveRequest_header.Is_Canceled = 1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7,Att_Leave_Request.Field4, Att_Leave_Request.Field5 from Att_LeaveRequest_header inner join Att_Leave_Request on Att_Leave_Request.Field2 = Att_LeaveRequest_header.Trans_Id inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id=" + Session["LocId"].ToString() + " and Att_LeaveRequest_header.emp_id=" + Session["EmpId"].ToString() + " order by cast(Set_EmployeeMaster.Emp_Code as int), From_date desc");

            //dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id,Set_EmployeeMaster.Emp_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name , Att_LeaveRequest_header.TotalDays as DaysCount, cast( Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Is_Pending=1 then 'Pending' when Is_Approved=1 then 'Approved' when Is_Canceled=1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7 from Att_LeaveRequest_header inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id=" + Session["LocId"].ToString() + " and Att_LeaveRequest_header.emp_id=" + Session["EmpId"].ToString() + "  order by cast(Set_EmployeeMaster.Emp_Code as int), From_date desc   ");
            // dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (dtLeave.Rows.Count > 0)
        {
            if (ddlLeaveStatus.SelectedIndex == 1)
            {
                dtLeave = new DataView(dtLeave, "Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLeaveStatus.SelectedIndex == 2)
            {
                dtLeave = new DataView(dtLeave, "Status='Approved'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLeaveStatus.SelectedIndex == 3)
            {
                dtLeave = new DataView(dtLeave, "Status='Rejected'", "", DataViewRowState.CurrentRows).ToTable();
            }

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvLeaveStatus, dtLeave, "", "");

           
            foreach (GridViewRow gvr in gvLeaveStatus.Rows)
            {
                Label lblFileName = (Label)gvr.FindControl("lblFileDownloadList");
                Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathList");
                LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadList");

                if (lblFileName.Text == "" && lblFilePath.Text == "")
                {
                    lnkDownload.Visible = false;
                }
                else
                {
                    lnkDownload.Visible = true;
                }
            }

        }
        else
        {
            gvLeaveStatus.DataSource = null;
            gvLeaveStatus.DataBind();
        }

        Session["Leave_DtLeaveStatus1"] = dtLeave;
        Session["Leave_DtLeaveStatus1_Filter"] = dtLeave;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtLeave.Rows.Count.ToString() + "";

        AllPageCode();
        btnbind_Click(null, null);
    }
    public void FillLeaveSummary(string EmpId)
    {
        string empid = string.Empty;
        empid = EmpId;
        if (empid != "")
        {
            DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(empid);
            dtLeaveSummary = new DataView(dtLeaveSummary, "Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)gvLeaveSummary, dtLeaveSummary, "", "");
            Session["Leave_DtLeaveStatus"] = dtLeaveSummary;
        }
        AllPageCode();
    }
    public static string GetCurrentLeave(string Days, string LeaveID)
    {
        if (LeaveID == "18")
        {
            float AssignDay = float.Parse(Days);
            string sMonth = DateTime.Now.ToString("MM");
            float Day = AssignDay / 12;
            float CurrentDays = Day * (float.Parse(sMonth) - 1);

            Double CurrentAssignDays = CurrentDays;

            int roundedValue = (int)Math.Floor(CurrentAssignDays);

            return Convert.ToString(roundedValue);
        }
        else
        {
            return Days;
        }
    }
    public static string GetUsedLeave(string UsedDays, string EncashDays, string LeaveId)
    {
        if (LeaveId == "18")
        {
            float Used = float.Parse(UsedDays) - float.Parse(EncashDays);
            return Convert.ToString(Used);

        }
        else
        {

            return Convert.ToString(Math.Round(double.Parse(UsedDays)));
        }

    }
    protected void txtOTEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];
        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(empid);
        if (Emp_ID == "0" || Emp_ID == "")
        {
            DisplayMessage("Employee not exists");
            txtEmpName.Focus();
            txtEmpName.Text = "";
            return;
        }
        else
        {
            DataTable dtOTLeaveSummary = objleaveReq.GetOTLeaveSummary(Emp_ID, HttpContext.Current.Session["CompId"].ToString());
            if (dtOTLeaveSummary.Rows.Count > 0)
            {
                Session["OTLeaves"] = dtOTLeaveSummary;
                objPageCmn.FillData((object)OTLeaveSummary, dtOTLeaveSummary, "", "");
            }
        }
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {

        string Emp_Name = txtEmpName.Text;
        int index = Emp_Name.IndexOf("/");
        if (index > 0)
            Emp_Name = Emp_Name.Substring(0, index);

        string Rep_Emp_Name = txtResponsiblePerson.Text;
        int Rep_index = Rep_Emp_Name.IndexOf("/");
        if (Rep_index > 0)
            Rep_Emp_Name = Rep_Emp_Name.Substring(0, Rep_index);

        if (Emp_Name == Rep_Emp_Name)
        {
            DisplayMessage("Employee name and replacement emp name cannot be same");
            txtResponsiblePerson.Text = "";
            txtResponsiblePerson.Focus();
            return;
        }
        else
        {
            string empid = string.Empty;
            if (((TextBox)sender).ID.Trim() == "txtEmpName")
            {
                gvLeaveSummary.DataSource = null;
                gvLeaveSummary.DataBind();
                gvLeaveSumary_Pending.DataSource = null;
                gvLeaveSumary_Pending.DataBind();
                gvLeaveSumary_Approved.DataSource = null;
                gvLeaveSumary_Approved.DataBind();
                txtFromDate.Text = "";
                txtToDate.Text = "";
                lblRDays.Text = "";
                lblDays.Text = "";
                ddlLeaveType.Items.Clear();
                gvleaveDetail.DataSource = null;
                gvleaveDetail.DataBind();
            }

            if (((TextBox)sender).Text != "")
            {
                empid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];
                //Commented by Ghanshyam Suthar on 06/12/2017
                // DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(empid);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    txtEmpName.Focus();
                    txtEmpName.Text = "";
                    return;
                }

                DataTable dtEmp = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Emp_ID);
                if (dtEmp.Rows.Count > 0)
                {
                    dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtEmp.Rows.Count > 0)
                    {
                        empid = dtEmp.Rows[0]["Emp_Id"].ToString();

                        if (((TextBox)sender).ID.Trim() == "txtEmpName")
                        {
                            hdnEmpId.Value = empid;

                            FillLeaveSummary(empid);
                            FillApproveLeave(empid);
                            FillPendingLeave(empid);
                        }
                    }
                    else
                    {
                        DisplayMessage("Employee Not Exists");

                        ((TextBox)sender).Text = "";
                        ((TextBox)sender).Focus();

                        //if (((TextBox)sender).ID.Trim() == "txtEmpName")
                        //{
                        //    ((TextBox)sender).Text = "";
                        //    ((TextBox)sender).Focus();
                        //    hdnEmpId.Value = "";
                        //}
                        return;
                    }
                }
            }
        }
    }
    public string GetMonthName(object month, object monthname)
    {
        string month1 = string.Empty;
        if (month.ToString() == "0")
        {
            month1 = "-";
        }
        else
        {
            month1 = monthname.ToString();
        }
        return month1;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }
    public string GetScheduleType(object Date, object EmpId, object leavetypeid)
    {
        //string ScheduleType = string.Empty;
        //string Date1 = Date.ToString();
        //string year = Convert.ToDateTime(Date1).Year.ToString();
        //string empid = EmpId.ToString();
        //string leaveId = leavetypeid.ToString();
        //DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString());
        //int FinancialYearMonth = 0;

        //if (dt.Rows.Count > 0)
        //{
        //    FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());

        //}
        //DateTime FinancialYearStartDate = new DateTime();
        //DateTime FinancialYearEndDate = new DateTime();
        //if (DateTime.Now.Month < FinancialYearMonth)
        //{

        //    FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);

        //    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        //}
        //else
        //{
        //    FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
        //    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);

        //}

        //year = string.Empty;

        //if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
        //{
        //    year = FinancialYearStartDate.Year.ToString();


        //}
        //else
        //{


        //    year = FinancialYearStartDate.Year.ToString();


        //}
        //DataTable dtLeave = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(empid);

        //dtLeave = new DataView(dtLeave, "Year='" + year + "' and Leave_Type_Id='" + leaveId + "'", "", DataViewRowState.CurrentRows).ToTable();


        //if (dtLeave.Rows.Count > 0)
        //{
        //    if (dtLeave.Rows[0]["Month"].ToString() == "0")
        //    {
        //        ScheduleType = "Yearly";
        //    }
        //    else
        //    {
        //ScheduleType = "Monthly";
        //    }

        //}
        //else
        //{
        //    ScheduleType = "Monthly";
        //}
        return "Yearly";

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {


        if (Request.QueryString["Emp_Id"] != null)
        {
            txtEmpName.Text = Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
            hdnEmpId.Value = Session["EmpId"].ToString();
            txtEmpName.Enabled = false;
            FillLeaveSummary(Session["EmpId"].ToString());
            FillApproveLeave(Session["EmpId"].ToString());
            FillPendingLeave(Session["EmpId"].ToString());
        }
        else
        {
            txtEmpName.Text = "";
            gvLeaveSummary.DataSource = null;
            gvLeaveSummary.DataBind();
            gvLeaveSumary_Approved.DataSource = null;
            gvLeaveSumary_Approved.DataBind();
            gvLeaveSumary_Pending.DataSource = null;
            gvLeaveSumary_Pending.DataBind();
            txtEmpName.Enabled = true;
            hdnEmpId.Value = "";
            ddlLeaveType.Items.Clear();
        }


        rbtnMonthly.Checked = false;
        rbtnYearly.Checked = true;

        //ddlLeaveType.Items.Clear();
        //ddlLeaveType.DataSource = null;
        //ddlLeaveType.DataBind();
        //ddlLeaveType.Visible = false;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        lblDays.Text = "";

        txtDescription.Text = "";
        txtResponsiblePerson.Text = "";

        txtEmpName.Focus();
        lblRDays.Text = string.Empty;
        hdnTransid.Value = "";
        Session["Leave_DtEmpLeave_Approved"] = null;
        Session["Leave_DtEmpLeave_Pending"] = null;
        txtResponsiblePerson.Text = "";
        objPageCmn.FillData((GridView)gvleaveDetail, null, "", "");
        ChkExtraLeave.Checked = false;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        AllPageCode();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void Reset()
    {
        Session["Leave_DtLeaveStatus"] = "";
        txtFromDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(1).ToString(objSys.SetDateFormat());
        txtToDate.Text = "";
        lblDays.Text = "";
        lblRDays.Text = "";
        ddlLeaveType.SelectedIndex = 0;
        Session["Leave_DtEmpLeave_Approved"] = null;
        Session["Leave_DtEmpLeave_Pending"] = null;
        txtDescription.Text = "";
        txtFromDate.Focus();
        FillLeaveSummary(hdnEmpId.Value);
        FillApproveLeave(hdnEmpId.Value);
        FillPendingLeave(hdnEmpId.Value);

    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {

        DataTable dtEmpInfo = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value);
        //here we are added validation that if employee requesting in diffrent location then it will validate 

        if (dtEmpInfo.Rows.Count > 0)
        {
            if (dtEmpInfo.Rows[0]["Location_Id"].ToString() != Session["LocId"].ToString())
            {
                DisplayMessage("You need to request from your registered Location.");
                return;
            }
        }


        if (ddlLeaveType.SelectedIndex <= 0)
        {
            DisplayMessage("select leave Type");
            ddlLeaveType.Focus();
            return;
        }
        //here we are checking maximum leave applicable on one slot 

        int ProbationMonth = 0;
        bool IsProbationPeriod = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (IsProbationPeriod == true)
        {
            ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        else
        {
            ProbationMonth = 0;
        }
        int FinencialYearStartMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Leave").Rows[0]["Approval_Level"].ToString();
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
        if (ddlLeaveType.SelectedValue == "")
        {
            DisplayMessage("You do not have Assigned Yearly Leave");
            txtFromDate.Focus();
            //rbtnMonthly.Checked = false;
            //rbtnYearly.Checked = false;
            return;
        }
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtFromDate.Focus();
            //rbtnMonthly.Checked = false;
            //rbtnYearly.Checked = false;
            return;
        }
        else
        {
            try
            {
                objSys.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format " + objSys.SetDateFormat() + "");
                txtFromDate.Focus();
                //rbtnMonthly.Checked = false;
                //rbtnYearly.Checked = false;
                return;
            }
        }
        //Responsible Person
        string strResponsiblePerson = "0";
        if (txtResponsiblePerson.Text != "")
        {
            string empid = txtResponsiblePerson.Text.Split('/')[txtResponsiblePerson.Text.Split('/').Length - 1];
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
            //dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                strResponsiblePerson = dtEmp.Rows[0]["Emp_Id"].ToString();
            }
        }
        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date");
            txtToDate.Focus();
            //rbtnMonthly.Checked = false;
            //rbtnYearly.Checked = false;
            return;
        }
        else
        {
            try
            {
                objSys.getDateForInput(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format " + objSys.SetDateFormat() + "");
                txtToDate.Focus();
                //rbtnMonthly.Checked = false;
                //rbtnYearly.Checked = false;
                return;
            }
            rbtnYearly.Focus();
        }
        if (objSys.getDateForInput(txtFromDate.Text) > objSys.getDateForInput(txtToDate.Text))
        {
            DisplayMessage("From Date cannot be greater than To Date");
            //txtFromDate.Text = "";
            //txtToDate.Text = "";
            txtFromDate.Focus();
            //rbtnMonthly.Checked = false;
            //rbtnYearly.Checked = false;
            return;
        }

        if (rbtnMonthly.Checked == false && rbtnYearly.Checked == false)
        {
            DisplayMessage("Please select Monthly or Yearly");
            rbtnYearly.Focus();
            return;
        }
        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }

        bool LeaveApprovalFunctionality = Common.LeaveApprovalFunctionality(ddlLeaveType.SelectedValue, Session["DBConnection"].ToString());
        DateTime dtFromDate = Convert.ToDateTime(txtFromDate.Text);
        DateTime dtToDate = Convert.ToDateTime(txtToDate.Text);
        DataTable dtTemp = new DataTable();
        DataTable dtLeaveDetail = GetLeaveDetail();
        dtTemp = dtLeaveDetail;


        //while (dtFromDate <= dtToDate)
        //{
        //    if (new DataView(dtTemp, "(From_Date<='" + dtFromDate + "' and To_Date>='" + dtFromDate + "') or (From_Date='" + dtFromDate + "') or ( To_Date='" + dtFromDate + "') ", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
        //    {
        //        DisplayMessage("Leave Already Applied between selected date criteria");
        //        return;
        //    }
        //    dtFromDate = dtFromDate.AddDays(1);
        //}

        //dtFromDate = Convert.ToDateTime(txtFromDate.Text);

        DateTime Doj = Convert.ToDateTime(objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value).Rows[0]["DOJ"].ToString());
        //created new functiinfor check leave validation

        ///created by jitendra upadhyay on 15/12/2017
        ///
        //string chkValidation = objleaveReq.CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, ddlLeaveType.SelectedValue, objSys.getDateForInput(txtFromDate.Text), objSys.getDateForInput(txtToDate.Text), hdnTransid.Value, ProbationMonth, IsProbationPeriod, Doj, 12, Convert.ToInt32(lblDays.Text), dtLeaveDetail, LeaveApprovalFunctionality);
        if (ChkExtraLeave.Checked==false)
        {
            string chkValidation = objleaveReq.CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, ddlLeaveType.SelectedValue, objSys.getDateForInput(txtFromDate.Text), objSys.getDateForInput(txtToDate.Text), hdnTransid.Value, ProbationMonth, IsProbationPeriod, Doj, Convert.ToInt32(lblRDays.Text), Convert.ToInt32(lblDays.Text), dtLeaveDetail, LeaveApprovalFunctionality, HttpContext.Current.Session["TimeZoneId"].ToString());
            if (chkValidation != "")
            {
                DisplayMessage(chkValidation);
                return;
            }
        }
        //string chkValidation = string.Empty;

        

        dtLeaveDetail.Rows.Add();


        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][0] = dtLeaveDetail.Rows.Count + 1;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][1] = ddlLeaveType.SelectedValue;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][2] = txtFromDate.Text;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][3] = txtToDate.Text;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][4] = strResponsiblePerson;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][5] = txtDescription.Text;
        if (rbtnMonthly.Checked)
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][6] = "Monthly";
        }
        else
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][6] = "Yearly";
        }

        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][7] = lblDays.Text;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][8] = Att_Leave_Request.Get_Rejoin(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtToDate.Text, hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][9] = hdnEmpId.Value;

        if (Session["empimgpath"] != null)
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][11] = Session["empimgpath"].ToString();
        }
        else
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][11] = "";
        }
        if (Session["empimgpathFull"] != null)
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][12] = Session["empimgpathFull"].ToString();
        }
        else
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][12] = "";
        }
        if (ddlLeaveType.SelectedValue == "18" || ddlLeaveType.SelectedValue == "21")
        {
            if (txtDepartureDate.Text == "" || txtreturnDate.Text == "")
            {
                DisplayMessage("DepartureDate & Returning Date is Required");
                return;
            }
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][13] = txtDepartureDate.Text;
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][14] = txtreturnDate.Text;
        }
        else
        {
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][13] = "01-01-1900";
            dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][14] = "01-01-1900";

        }

        objPageCmn.FillData((object)gvleaveDetail, dtLeaveDetail, "", "");

        foreach (GridViewRow gvr in gvleaveDetail.Rows)
        {
            Label lblFileName = (Label)gvr.FindControl("lblFileDownload");
            Label lblFilePath = (Label)gvr.FindControl("lblFileFullPath");
            LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownload");
            Label lblDepartureDate = (Label)gvr.FindControl("lblDepartureDate");
            Label lblReturningDate = (Label)gvr.FindControl("lblReturningDate");
            if (lblDepartureDate.Text == "01-Jan-1900" || lblReturningDate.Text == "01-Jan-1900")
            {
                gvleaveDetail.Columns[9].Visible = false;
                gvleaveDetail.Columns[10].Visible = false;
            }
            if (lblFileName.Text == "" && lblFilePath.Text == "")
            {
                lnkDownload.Visible = false;
            }
            else
            {
                lnkDownload.Visible = true;
            }
        }

        Session["empimgpathFull"] = null;
        Session["empimgpath"] = null;
        Reset();
    }
    public string LeaveApplicableinSingleslot()
    {
        string strSingleAttempt = string.Empty;
        string strMaxLeave = string.Empty;

        strMaxLeave = objleave.GetLeaveMasterById(Session["CompId"].ToString(), ddlLeaveType.SelectedValue.Trim()).Rows[0]["Field3"].ToString();

        if (strMaxLeave == "")
        {
            strMaxLeave = "0";
        }
        int LeaveApplied = 0;

        try
        {
            LeaveApplied = Convert.ToInt32(lblDays.Text);
        }
        catch
        {

        }

        DataTable dtLeaveDetail = GetLeaveDetail();
        dtLeaveDetail = new DataView(dtLeaveDetail, "Leave_Type_id=" + ddlLeaveType.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();

        foreach (DataRow dr in dtLeaveDetail.Rows)
        {
            LeaveApplied += Convert.ToInt32(dr["LeaveCount"].ToString());
        }


        if (Convert.ToInt32(strMaxLeave) > 0)
        {
            if (LeaveApplied > Convert.ToInt32(strMaxLeave))
            {
                strSingleAttempt = "You Can apply " + strMaxLeave + " Leave in single attempt";
            }
        }
        return strSingleAttempt;
    }

    protected void imgBtnLeaveDetailDelete_Command(object sender, CommandEventArgs e)
    {


        DataTable dt = GetLeaveDetail();

        DataTable dtemp = dt.Copy();

        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)gvleaveDetail, dtemp, "", "");

        dtemp.Dispose();


        int UsedLeave = 0;

        if (ddlLeaveType.SelectedIndex > 0)
        {

            DataTable dtLeave = dt.Copy();

            dtLeave = new DataView(dtLeave, "Trans_Id=" + e.CommandArgument.ToString() + " and Leave_Type_Id=" + ddlLeaveType.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

            foreach (DataRow dr in dtLeave.Rows)
            {
                UsedLeave += Convert.ToInt32(dr["LeaveCount"].ToString());
            }

            if (lblRDays.Text == "")
            {
                lblRDays.Text = "0";
            }
            lblRDays.Text = (Convert.ToInt32(lblRDays.Text) + UsedLeave).ToString();
        }
        Session["empimgpathFull"] = null;
        Session["empimgpath"] = null;
    }
    //This function Add by rahul Sharma for Overtime Convert in Anual Leaves Date :27-06-2023
    protected void imgBtnOTLeaveDelete_Command(object sender, CommandEventArgs e)
    {
        string Trans_Id = e.CommandArgument.ToString();
        DataTable dt = objleaveReq.GetOTLeaveSummaryForAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            try
            {
                DataTable dtOTLeaveSummary = objleaveReq.EmployeeOTTrans(dt.Rows[0]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                string Remaining_Days = dtOTLeaveSummary.Rows[0]["Remaining_Days"].ToString();
                string TotalDays = dt.Rows[0]["TotalDays"].ToString();
                string OT_Days = dtOTLeaveSummary.Rows[0]["OT_Leave"].ToString();
                if (float.Parse(Remaining_Days) > float.Parse(TotalDays))
                {
                    string ActualRemainingDays = Convert.ToString(float.Parse(Remaining_Days) - float.Parse(TotalDays));
                    string ActualOTDays = Convert.ToString(float.Parse(OT_Days) - float.Parse(TotalDays));
                    SqlConnection con = new SqlConnection();
                    con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
                    con.Open();
                    SqlTransaction trns;
                    trns = con.BeginTransaction();
                    try
                    {
                        int i = 0;
                        i = objleaveReq.UpdateLeaveOTTrans(HttpContext.Current.Session["CompId"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), ActualRemainingDays, ActualOTDays, ref trns);
                        if (i != 0)
                        {
                            ObjDa.execute_Command("Update Att_Leave_OT Set IsActive='0' where Trans_Id='" + Trans_Id + "'");
                        }
                    }
                    catch (Exception ex)
                    {
                        trns.Rollback();
                        DisplayMessage("Leave not deleted");
                    }
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    DisplayMessage("Leave Delete Successfully");

                }
                else
                {
                    DisplayMessage("Leave is Used you Can not deleted");
                    return;
                }
            }
            catch (Exception ex)
            {
                DisplayMessage("Leave not deleted");
                return;
            }

        }
        else
        {
            DisplayMessage("Leave not found");
            return;
        }


    }
    public DataTable GetLeaveDetail()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_id", typeof(float));
        dt.Columns.Add("Leave_Type_id");
        dt.Columns.Add("From_date", typeof(DateTime));
        dt.Columns.Add("To_Date", typeof(DateTime));
        dt.Columns.Add("Field1");
        dt.Columns.Add("Emp_Description");
        dt.Columns.Add("Field3");
        dt.Columns.Add("LeaveCount", typeof(float));
        dt.Columns.Add("Field7", typeof(DateTime));
        dt.Columns.Add("Emp_Id", typeof(float));
        dt.Columns.Add("Is_Approval", typeof(Boolean));
        dt.Columns.Add("Field4");
        dt.Columns.Add("Field5");
        dt.Columns.Add("Departure_Date", typeof(DateTime));
        dt.Columns.Add("Returning_Date", typeof(DateTime));
        foreach (GridViewRow gvrow in gvleaveDetail.Rows)
        {
            DataRow dr = dt.NewRow();

            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblLeaveTypeId")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblFromdate")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblToDate")).Text;
            dr[4] = ((Label)gvrow.FindControl("lblResponsiblePersonNameID")).Text;
            dr[5] = ((Label)gvrow.FindControl("lbldesc")).Text;
            dr[6] = ((Label)gvrow.FindControl("lblScheduleType")).Text;
            dr[7] = ((Label)gvrow.FindControl("lblLeaveCount")).Text;
            dr[8] = ((Label)gvrow.FindControl("lblRejoinDate")).Text;
            dr[9] = hdnEmpId.Value;
            dr[10] = Common.LeaveApprovalFunctionality(((Label)gvrow.FindControl("lblLeaveTypeId")).Text, Session["DBConnection"].ToString());
            dr[11] = ((Label)gvrow.FindControl("lblFileFullPath")).Text;
            dr[12] = ((Label)gvrow.FindControl("lblFileDownload")).Text;
            dr[13] = ((Label)gvrow.FindControl("lblDepartureDate")).Text;
            dr[14] = ((Label)gvrow.FindControl("lblReturningDate")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    //This function Add by rahul Sharma for Overtime Convert in Anual Leaves Date :27-06-2023
    protected void btnOTSubmit_Clcik(object sender, EventArgs e)
    {
        OTSubmit.Enabled = false;

        if (txtOTEmpName.Text == "")
        {
            DisplayMessage("Please Fill Employee Name");
            return;
        }
        if (txtOTTotalDays.Text == "")
        {
            DisplayMessage("Please Fill Total Days");
            return;
        }
        if (txtOTDescription.Text == "")
        {
            DisplayMessage("Please Description");
            return;
        }
        if (txtOTAssignDate.Text == "")
        {
            DisplayMessage("Please Fill Assign Date");
        }
        string empid = txtOTEmpName.Text.Split('/')[2];
        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(empid);
        if (Emp_ID == "0" || Emp_ID == "")
        {
            DisplayMessage("Employee not exists");
            txtEmpName.Focus();
            txtEmpName.Text = "";
            return;
        }
        else
        {
            try
            {


                SqlConnection con = new SqlConnection();
                con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
                con.Open();
                SqlTransaction trns;
                trns = con.BeginTransaction();
                int i = 0;
                i = objleaveReq.InsertOTRequest(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "18", txtOTAssignDate.Text, Emp_ID, "0", txtOTTotalDays.Text, txtOTDescription.Text, "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (i != 0)
                {
                    try
                    {
                        string Remaining_Days = "";
                        string OT_Days = "";
                        string TotalDays = txtOTTotalDays.Text;
                        DataTable dtOTLeaveSummary = objleaveReq.EmployeeOTTrans(Emp_ID, HttpContext.Current.Session["CompId"].ToString());
                        if (dtOTLeaveSummary.Rows.Count > 0)
                        {
                            try
                            {
                                Remaining_Days = dtOTLeaveSummary.Rows[0]["Remaining_Days"].ToString();
                                OT_Days = dtOTLeaveSummary.Rows[0]["OT_Leave"].ToString();
                                if (OT_Days == "")
                                {
                                    OT_Days = "0";
                                    Remaining_Days = Convert.ToString((float.Parse(Remaining_Days)) + float.Parse(TotalDays));
                                    OT_Days = Convert.ToString(float.Parse(OT_Days) + float.Parse(TotalDays));
                                }
                                else
                                {
                                    Remaining_Days = Convert.ToString(float.Parse(Remaining_Days) + float.Parse(TotalDays));
                                    OT_Days = Convert.ToString(float.Parse(OT_Days) + float.Parse(TotalDays));
                                }
                                objleaveReq.UpdateLeaveOTTrans(HttpContext.Current.Session["CompId"].ToString(), Emp_ID, Remaining_Days, OT_Days, ref trns);
                            }
                            catch (Exception ex)
                            {
                                trns.Rollback();
                            }


                        }
                    }
                    catch
                    {
                        trns.Rollback();
                    }
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    ResetOT();
                    DisplayMessage("Leave added Successfully");
                }
                else
                {
                    trns.Rollback();
                    OTSubmit.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                OTSubmit.Enabled = true;
            }
        }

    }
    //This function Add by rahul Sharma for Overtime Convert in Anual Leaves Date :27-06-2023
    public void ResetOT()
    {

        OTSubmit.Enabled = true;
        txtOTAssignDate.Text = "";
        txtOTAssignDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        txtOTDescription.Text = "";
        txtOTTotalDays.Text = "";
        txtOTEmpName.Text = "";
        OTLeaveSummary.DataSource = null;
        OTLeaveSummary.DataBind();
    }
    protected void btnApply_Click(object sender, EventArgs e)
    {
        if (gvleaveDetail.Rows.Count == 0)
        {
            DisplayMessage("Add Leave Details");
            return;
        }

        string strMaxId = string.Empty;
        DataTable dtTemp1 = new DataTable();
        DataTable dtLeaveDetail = new DataTable();
        DateTime dtFromDate = new DateTime();
        DateTime dtToDate = new DateTime();
        DateTime Rejoin_Date = new DateTime();
        int TotalDays = 0;
        DataTable dtTemp = GetLeaveDetail();
        dtTemp1 = dtTemp.Copy();
        SqlConnection con = new SqlConnection();

        string[] strResult = new string[2];
        dtTemp = dtTemp.DefaultView.ToTable(true, "Is_Approval");

        string EmpPermission = string.Empty;

        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Leave").Rows[0]["Approval_Level"].ToString();
        //EmpPermission = objSys.GetSysParameterByParamName("Approval Setup").Rows[0]["Approval_Level"].ToString();

        DataTable dt1 = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "52", hdnEmpId.Value);


        DataTable dtEncash = new DataTable();

        dtEncash = ObjDa.return_DataTable("Select*from Att_Leave_Encashment_Header Where Is_Pending='1' And Emp_Id='" + hdnEmpId.Value + "'");
        if (dtEncash.Rows.Count > 0)
        {
            DisplayMessage("You have pending request regarding Leave Encashment");
            return;
        }

        // dt1 = new DataView(dt1, "Approval='Leave'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt1.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }
       

        GetEmployeeName(hdnEmpId.Value);

        for (int i = 0; i < dtTemp.Rows.Count; i++)
        {
            con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
            con.Open();
            SqlTransaction trns;
            trns = con.BeginTransaction();
            if (ChkExtraLeave.Checked == true)
            {
                dtTemp1.Rows[i]["Field4"] = "true";
            }
            else
            {
                dtTemp1.Rows[i]["Field4"] = "false";
            }
            dtLeaveDetail = new DataView(dtTemp1, "Is_Approval='" + dtTemp.Rows[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            try
            {
                dtFromDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "From_Date", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());
                dtToDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "To_Date desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Date"].ToString());
                Rejoin_Date = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "Field7", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field7"].ToString());

                TotalDays = Convert.ToInt32((dtToDate - dtFromDate).TotalDays) + 1;
                ////Updated Add Code On 01-08-2023---- START
                //int iLeaveCount = 0;
                //for (int j = 0; j < dtLeaveDetail.Rows.Count; j++)
                //{
                //    if (iLeaveCount == 0)
                //    {
                //        iLeaveCount = int.Parse(dtLeaveDetail.Rows[j]["LeaveCount"].ToString());
                //    }
                //    else
                //    {
                //        iLeaveCount = iLeaveCount + int.Parse(dtLeaveDetail.Rows[j]["LeaveCount"].ToString());
                //    }
                //}
                //TotalDays = Convert.ToInt32(iLeaveCount);
                ////Updated Add Code On 01-08-2023---- END

                strResult = objleaveReq.SaveLeaveRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), hdnEmpId.Value, dtFromDate, dtToDate, Rejoin_Date, TotalDays, dtLeaveDetail, hdnTransid.Value, Convert.ToBoolean(dtTemp.Rows[i][0].ToString()), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

                if (strResult[0].Trim() == "")
                {
                    DisplayMessage("Leave submitted");
                    strMaxId = strResult[1].Trim();
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }
                else
                {
                    DisplayMessage(strResult[0].Trim());
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


            //created common function for insert approval and notification 
            //created by jitendra upadhyay on 16/12/2017
            objleaveReq.InsertLeaveApproval(hdnEmpId.Value, EmpPermission, dt1, strMaxId, dtLeaveDetail, txtEmpName.Text, hdnTransid.Value, ViewState["Emp_Img"].ToString(), dtFromDate, dtToDate, true, Convert.ToBoolean(dtTemp.Rows[i][0].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString(), HttpContext.Current.Session["EmpId"].ToString());

        }


        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        FillLeaveStatus();
        PrintReport(strMaxId, hdnEmpId.Value);
        btnReset_Click(null, null);
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
    private void ClearFormFields()
    {
        rbtnMonthly.Checked = false;
        rbtnYearly.Checked = true;
        ddlLeaveType.Items.Clear();
        ddlLeaveType.DataSource = null;
        ddlLeaveType.DataBind();
        ddlLeaveType.Visible = false;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        lblDays.Text = "";
        txtEmpName.Text = "";
        lblRDays.Text = string.Empty;
        txtDescription.Text = string.Empty;
        txtEmpName.Focus();
        hdnTransid.Value = "";
        return;
    }
    private string DateFormat(string Date)
    {
        string SystemDate = string.Empty;
        try
        {
            SystemDate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());
        }
        catch
        {
            SystemDate = Date;
        }
        return SystemDate;
    }
    protected void txtToDate_textChanged(object sender, EventArgs e)
    {
        string IsLeaveOnHoliday = string.Empty;
        string IsLeaveOnWeekOff = string.Empty;
        string WeekOffDays = string.Empty;
        int LeaveDays = 0;
        lblDays.Text = "";
        if (txtEmpName.Text.Trim() == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }

        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            txtFromDate.Focus();
            lblDays.Text = "0";
            return;

        }
        if (txtToDate.Text == "" && ((TextBox)sender).ID != "txtFromDate")
        {
            txtToDate.Focus();
            DisplayMessage("Enter To Date");
            txtToDate.Focus();
            lblDays.Text = "0";
            return;

        }
        else
        {
            try
            {
                objSys.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format " + objSys.SetDateFormat() + "");
                txtFromDate.Focus();
                return;

            }

        }

        if (txtToDate.Text != "")
        {
            try
            {
                objSys.getDateForInput(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format " + objSys.SetDateFormat() + "");
                txtToDate.Focus();
                return;
            }


            if (objSys.getDateForInput(txtFromDate.Text) > objSys.getDateForInput(txtToDate.Text))
            {
                DisplayMessage("From Date cannot be greater than To Date");
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtFromDate.Focus();

                return;
            }

            DateTime fromdate = objSys.getDateForInput(txtFromDate.Text);
            DateTime todate = objSys.getDateForInput(txtToDate.Text);
            int days = 0;
            WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            IsLeaveOnWeekOff = objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            IsLeaveOnHoliday = objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            while (fromdate <= todate)
            {
                if (IsLeaveOnWeekOff == "False")
                {
                    foreach (string str in WeekOffDays.Split(','))
                    {
                        if (str == "Friday")
                        {
                            string WeekOff = fromdate.DayOfWeek.ToString();
                            if (WeekOff == "Friday")
                            {
                                LeaveDays += 1;
                            }
                        }

                        //if (WeekOff == str)
                        //{
                        //    LeaveDays += 1;
                        //}
                    }
                }
                if (IsLeaveOnHoliday == "False")
                {
                    DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    // DateTime fromdate2 = objSys.getDateForInput(fromdate.ToString());

                    DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + fromdate.ToString() + "' and Emp_Id='" + hdnEmpId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtHoliday1.Rows.Count > 0)
                    {
                        LeaveDays += 1;
                    }
                }
                days += 1;
                fromdate = fromdate.AddDays(1);

            }
            days = days - LeaveDays;

            rbtnMonthlyYearly(sender, e);

            // Here require week off code
            lblDays.Text = days.ToString();
        }

    }
    public string GetDate(object obj)
    {

        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());



        return Date.ToString(objSys.SetDateFormat());

    }
    protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLeaveType.SelectedValue == "0")
        {
            lblRDays.Text = string.Empty;
        }
        else
        {
            lblRDays.Text = string.Empty;
            int ProbationMonth = 0;
            bool IsProbationPeriod = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            if (IsProbationPeriod == true)
            {
                ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            else
            {
                ProbationMonth = 0;
            }
            DataTable Dt = objAttendance.GetEmployeeDOJ(Session["CompId"].ToString(), hdnEmpId.Value);
            DateTime NewDate = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).AddMonths(ProbationMonth);
            // if (Convert.ToDateTime(txtFromDate.Text).Month >= NewDate.Month && Convert.ToDateTime(txtFromDate.Text).Year >= NewDate.Year)
            if (ddlLeaveType.SelectedValue == "18" || ddlLeaveType.SelectedValue == "21")
            {
                Leave_Dep_Return.Visible = true;
            }
            else
            {
                Leave_Dep_Return.Visible = false;
            }
            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).AddMonths(ProbationMonth) || IsProbationPeriod == false)
            {
                // Nitin jain On 28/11/2014 , Code To Give Leave Yearly On Bases of Days Worked
                double UsedDays = 0;
                double PendingDays = 0;
                double Total_Days = 0;
                double PerDayLeave = 0;
                string Schedule_Type = string.Empty;
                int Leave_Id = 0;
                double TotalWorkedDays = 0;
                double Total_WorkedDays_Leave = 0;
                if (rbtnYearly.Checked == true)
                {

                    DataTable DtLeaveStatus = (DataTable)Session["Leave_DtLeaveStatus"];
                    Leave_Id = Convert.ToInt32(ddlLeaveType.SelectedValue);
                    DtLeaveStatus = new DataView(DtLeaveStatus, "Leave_Type_Id='" + Leave_Id + "' and Shedule_Type='Yearly' ", "", DataViewRowState.CurrentRows).ToTable();
                    //if (DtLeaveStatus.Rows[0]["IsRule"].ToString() == true.ToString())
                    //{
                    //    DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString());
                    //    int FinancialYearMonth = 0;
                    //    if (dt.Rows.Count > 0)
                    //    {
                    //        FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
                    //    }

                    //    DateTime FinancialYearStartDate = new DateTime();
                    //    DateTime FinancialYearEndDate = new DateTime();
                    //    if (DateTime.Now.Month < FinancialYearMonth)
                    //    {
                    //        FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
                    //        FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                    //    }
                    //    else
                    //    {
                    //        FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
                    //        FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                    //    }
                    //    if (DtLeaveStatus.Rows.Count > 0)
                    //    {

                    //        UsedDays = Convert.ToInt32(DtLeaveStatus.Rows[0]["Used_Days"].ToString());
                    //        PendingDays = Convert.ToInt32(DtLeaveStatus.Rows[0]["Pending_Days"].ToString());

                    //        Total_Days = Convert.ToInt32(DtLeaveStatus.Rows[0]["TotalAssignedLeave"].ToString());


                    //        // Total Used Lave Till today
                    //        DateTime DOJ = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString());
                    //        // Get Per Day Leave For Employee

                    //        PerDayLeave = Total_Days / 365;
                    //        // int FinalDateDays = ((Convert.ToDateTime(txtToDate.Text).Year),conv
                    //        DateTime FinalDate = new DateTime((Convert.ToDateTime(txtToDate.Text).Year), Convert.ToDateTime(txtToDate.Text).Month, 1).AddMonths(1);
                    //        FinalDate = FinalDate.AddDays(-1);

                    //        if (FinancialYearStartDate > DOJ)
                    //        {
                    //            TotalWorkedDays = (FinalDate - FinancialYearStartDate).TotalDays;
                    //        }
                    //        else
                    //        {
                    //            DOJ = new DateTime((Convert.ToDateTime(DOJ).Year), Convert.ToDateTime(DOJ).Month, 1);
                    //            TotalWorkedDays = (FinalDate - DOJ).TotalDays;
                    //        }
                    //        UsedDays = UsedDays + PendingDays;
                    //        Total_WorkedDays_Leave = Math.Round(PerDayLeave * TotalWorkedDays) + Convert.ToInt32(DtLeaveStatus.Rows[0]["Previous_days"].ToString());
                    //        lblRDays.Text = Convert.ToString(Total_WorkedDays_Leave);

                    //    }
                    //}
                    //else
                    //{


                    if ((Convert.ToInt32(Math.Round(Convert.ToDouble(DtLeaveStatus.Rows[0]["Remaining_Days"].ToString()), 0).ToString()) - Att_Leave_Request.GetleaveBalance(GetLeaveDetail(), ddlLeaveType.SelectedValue, hdnEmpId.Value)) < 0 && !Common.LeaveNegativeBalance(ddlLeaveType.SelectedValue, Session["DBConnection"].ToString()))
                    {
                        lblRDays.Text = "0";
                    }
                    else
                    {

                        lblRDays.Text = (Convert.ToInt32(Att_Leave_Request.GetRoundValue(DtLeaveStatus.Rows[0]["Remaining_Days"].ToString())) - Att_Leave_Request.GetleaveBalance(GetLeaveDetail(), ddlLeaveType.SelectedValue, hdnEmpId.Value)).ToString();
                    }

                    //}
                }
            }
        }
    }
    protected void rbtnMonthlyYearly(object sender, EventArgs e)
    {
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Enter From Date");
            rbtnMonthly.Checked = false;
            rbtnYearly.Checked = true;
            txtFromDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format dd-MMM-yyyy");
                txtFromDate.Focus();
                rbtnMonthly.Checked = false;
                rbtnYearly.Checked = true;
                return;

            }

        }

        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter To Date");
            txtToDate.Focus();
            rbtnMonthly.Checked = false;
            rbtnYearly.Checked = true;
            return;

        }
        else
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
                txtToDate.Focus();
                rbtnMonthly.Checked = false;
                rbtnYearly.Checked = true;
                return;

            }

        }
        if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
        {
            DisplayMessage("From Date cannot be greater than To Date");
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtFromDate.Focus();
            rbtnMonthly.Checked = false;
            rbtnYearly.Checked = true;
            return;

        }

        if (lblDays.Text.Trim() != "")
        {
            if (float.Parse(lblDays.Text) == 0 || float.Parse(lblDays.Text) < 0)
            {
                DisplayMessage("You can not apply leave for selected date");
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtFromDate.Focus();
                rbtnMonthly.Checked = false;
                rbtnYearly.Checked = true;
                lblDays.Text = "";
                return;

            }
        }

        if (rbtnMonthly.Checked)
        {
            DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(hdnEmpId.Value);

            string months = string.Empty;
            string year = string.Empty;
            DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            while (FromDate <= ToDate)
            {
                months += FromDate.Month.ToString() + ",";
                FromDate = FromDate.AddMonths(1);
                string year1 = FromDate.Year.ToString();
                if (!year.Split(',').Contains(year1))
                {
                    year += year1 + ",";
                }
            }


            string months4 = string.Empty;
            string year4 = string.Empty;
            DateTime FromDate1 = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            DateTime ToDate1 = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).AddMonths(2);
            while (FromDate <= ToDate)
            {
                months4 += FromDate1.Month.ToString() + ",";
                FromDate = FromDate1.AddMonths(1);
                string year5 = FromDate1.Year.ToString();
                if (!year4.Split(',').Contains(year5))
                {
                    year4 += year5 + ",";
                }
            }

            DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            int FinancialYearMonth = 0;

            if (dt.Rows.Count > 0)
            {
                FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());

            }

            DateTime FinancialYearStartDate = new DateTime();
            DateTime FinancialYearEndDate = new DateTime();
            if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
            {

                FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);

                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            else
            {
                FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);

            }
            year = string.Empty;

            if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
            {
                year4 = FinancialYearStartDate.Year.ToString();


            }
            else
            {
                year4 += FinancialYearStartDate.Year.ToString() + ",";

                year4 += FinancialYearEndDate.Year.ToString() + ",";
            }

            DateTime DateFrm = Convert.ToDateTime(txtFromDate.Text);

            if (!months4.Split(',').Contains(DateFrm.Month.ToString()) && !year4.Split(',').Contains(DateFrm.Year.ToString()))
            {
                DisplayMessage("You can not request leave for this month");
                txtFromDate.Text = "";
                txtToDate.Text = "";
                txtFromDate.Focus();
                rbtnMonthly.Checked = false;
                rbtnYearly.Checked = true;
                lblDays.Text = "";
                return;

            }

            dtLeaveSummary = new DataView(dtLeaveSummary, "month in(" + months + ") and year in (" + year4 + ") ", "", DataViewRowState.CurrentRows).ToTable();
            dtLeaveSummary = dtLeaveSummary.DefaultView.ToTable(true, "Leave_Type_Id");
            DataTable dtleave = new DataTable();
            //thsi code is created by jitendra upadhyay on 17-09-2014
            //this code for showing the multiple leave in dropdown which was working incorrect
            //code start
            DataTable dtLeave_DDl = new DataTable();
            for (int i = 0; i < dtLeaveSummary.Rows.Count; i++)
            {
                dtleave = objleave.GetLeaveMasterById(Session["CompId"].ToString(), dtLeaveSummary.Rows[i]["Leave_Type_Id"].ToString());
                dtLeave_DDl.Merge(dtleave);
                // Leave Type Id 1 is For Indemnity Leave So do not use in this dataTable
                // dtLeave_DDl = new DataView(dtLeave_DDl, "' AND Leave_Id<>'1'", "", DataViewRowState.CurrentRows).ToTable();
            }
            //code end
            if (dtLeave_DDl.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)ddlLeaveType, dtLeave_DDl, "Leave_Name", "Leave_Id");
                ddlLeaveType.Visible = true;
            }
            else
            {
                ddlLeaveType.Items.Clear();
                ddlLeaveType.DataSource = null;
                ddlLeaveType.DataBind();
                ddlLeaveType.Visible = false;
            }
        }
        else if (rbtnYearly.Checked)
        {
            DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(hdnEmpId.Value);

            string months = string.Empty;
            string year = string.Empty;
            DateTime FromDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());


            string year4 = string.Empty;


            DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            int FinancialYearMonth = 0;

            if (dt.Rows.Count > 0)
            {
                FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());

            }

            DateTime FinancialYearStartDate = new DateTime();
            DateTime FinancialYearEndDate = new DateTime();
            if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
            {

                FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);

                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            else
            {
                FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);

            }
            year = string.Empty;

            if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
            {
                year4 = FinancialYearStartDate.Year.ToString();
            }
            else
            {
                year4 += FinancialYearStartDate.Year.ToString() + ",";
            }
            // Nitin Jain on 10/12/2014 For Leave TypeId =1 , Indemnity Leave 
            //dtLeaveSummary = new DataView(dtLeaveSummary, "month='0' and Leave_Type_Id<>'1' AND year in (" + year4.ToString() + ") ", "", DataViewRowState.CurrentRows).ToTable();
            dtLeaveSummary = new DataView(dtLeaveSummary, "Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtLeaveSummary.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)ddlLeaveType, dtLeaveSummary, "Leave_Name", "Leave_Type_Id");
                ddlLeaveType.Visible = true;
            }
            else
            {
                ddlLeaveType.Items.Clear();
                ddlLeaveType.DataSource = null;
                ddlLeaveType.DataBind();
                ddlLeaveType.Visible = false;
            }
        }
        else if (rbtnMonthly.Checked == false && rbtnYearly.Checked == false)
        {

            ddlLeaveType.Items.Clear();
            ddlLeaveType.DataSource = null;
            ddlLeaveType.DataBind();
            ddlLeaveType.Visible = false;
        }
    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dtleaveDetail = ObjDa.return_DataTable("select *,(select  COUNT(*) from Att_Leave_Request_Child where Ref_Id = Att_Leave_Request.Trans_Id) as LeaveCount from Att_Leave_Request where Field2=" + e.CommandArgument.ToString() + "");

        dtleaveDetail = dtleaveDetail.DefaultView.ToTable(true, "Trans_id", "Emp_Id", "Leave_Type_id", "From_date", "To_Date", "Field1", "Emp_Description", "Field3", "LeaveCount", "Field7", "Field4", "Field5", "Departure_Date", "Returning_Date");


        objPageCmn.FillData((GridView)gvleaveDetail, dtleaveDetail, "", "");
        foreach (GridViewRow gvr in gvleaveDetail.Rows)
        {
            Label lblFileName = (Label)gvr.FindControl("lblFileDownload");
            Label lblFilePath = (Label)gvr.FindControl("lblFileFullPath");
            LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownload");
            Label lblDepartureDate = (Label)gvr.FindControl("lblDepartureDate");
            Label lblReturningDate = (Label)gvr.FindControl("lblReturningDate");
            if (lblDepartureDate.Text == "01-Jan-1900" || lblReturningDate.Text == "01-Jan-1900")
            {
                gvleaveDetail.Columns[9].Visible = false;
                gvleaveDetail.Columns[10].Visible = false;
            }
            if (lblFileName.Text == "" && lblFilePath.Text == "")
            {
                lnkDownload.Visible = false;
            }
            else
            {
                lnkDownload.Visible = true;
            }
        }
        hdnTransid.Value = e.CommandArgument.ToString();
        txtEmpName.Text = Common.GetEmployeeName(dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
        hdnEmpId.Value = dtleaveDetail.Rows[0]["Emp_Id"].ToString();

        txtEmpName.Enabled = false;
        FillLeaveSummary(hdnEmpId.Value);
        FillApproveLeave(hdnEmpId.Value);
        FillPendingLeave(hdnEmpId.Value);


        if (((ImageButton)sender).ID != "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
        }

        AllPageCode();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
    }
    public void FillPendingLeave(string Empid)
    {
        //Commented by Ghanshyam Suthar on 06/12/2017
        //DataTable dtLeave = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        DataTable dtLeave = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Empid);
        if (dtLeave.Rows.Count > 0)
        {
            dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id=" + Empid + "", "", DataViewRowState.CurrentRows).ToTable();

            if (dtLeave.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSumary_Pending, dtLeave, "", "");
                Session["Leave_DtEmpLeave_Pending"] = dtLeave;

                foreach (GridViewRow gvr in gvLeaveSumary_Pending.Rows)
                {
                    Label lblFileName = (Label)gvr.FindControl("lblFileDownloadPending");
                    Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathPending");
                    LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadPending");
                    Button ChoosefileUpload = (Button)gvr.FindControl("ChoosefileUpload");
                    Label Status = (Label)gvr.FindControl("lblStatus");
                    if (lblFileName.Text == "" && lblFilePath.Text == "")
                    {
                        lnkDownload.Visible = false;
                        if (Status.Text == "Pending")
                        {
                            ChoosefileUpload.Visible = true;
                        }
                        else
                        {
                            ChoosefileUpload.Visible = false;
                        }
                    }
                    else
                    {
                        lnkDownload.Visible = true;
                        ChoosefileUpload.Visible = true;
                    }
                }
            }
            else
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSumary_Pending, dtLeave, "", "");
                Session["Leave_DtEmpLeave_Pending"] = null;
            }
        }
    }
    protected void gvLeaveOTSumary_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        OTLeaveSummary.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["OTLeaves"];
        //Common Function add Rahul Date 22-06-2023
        objPageCmn.FillData((object)OTLeaveSummary, dt, "", "");

    }
    protected void gvLeaveSumary_Pending_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveSumary_Pending.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["Leave_DtEmpLeave_Pending"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLeaveSumary_Pending, dt, "", "");
        foreach (GridViewRow gvr in gvLeaveSumary_Approved.Rows)
        {
            Label lblFileName = (Label)gvr.FindControl("lblFileDownloadApproved");
            Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathApproved");
            LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadApproved");
            Button ChoosefileUpload = (Button)gvr.FindControl("ChoosefileUpload");
            if (lblFileName.Text == "" && lblFilePath.Text == "")
            {
                lnkDownload.Visible = false;
                ChoosefileUpload.Visible = true;
            }
            else
            {
                lnkDownload.Visible = true;
                ChoosefileUpload.Visible = false;
            }
        }
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "$('#myModal').modal('toggle');", "", true);
        FillPendingLeave(hdnEmpId.Value);
    }
    public void FillApproveLeave(string Empid)
    {
        //Commented by Ghanshyam Suthar on 06/12/2017
        //DataTable dtLeave = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        DataTable dtLeave = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Empid);
        if (dtLeave.Rows.Count > 0)
        {
            dtLeave = new DataView(dtLeave, "Is_Approved='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id=" + Empid + "", "From_Date", DataViewRowState.CurrentRows).ToTable();

            if (dtLeave.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSumary_Approved, dtLeave, "", "");
                Session["Leave_DtEmpLeave_Approved"] = dtLeave;
                foreach (GridViewRow gvr in gvLeaveSumary_Approved.Rows)
                {
                    Label lblFileName = (Label)gvr.FindControl("lblFileDownloadApproved");
                    Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathApproved");
                    LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadApproved");

                    if (lblFileName.Text == "" && lblFilePath.Text == "")
                    {
                        lnkDownload.Visible = false;
                    }
                    else
                    {
                        lnkDownload.Visible = true;
                    }
                }
            }
            else
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSumary_Approved, dtLeave, "", "");
                Session["Leave_DtEmpLeave_Approved"] = null;
            }
        }
    }
    protected void gvLeaveSumary_Approved_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveSumary_Approved.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["Leave_DtEmpLeave_Approved"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLeaveSumary_Approved, dt, "", "");
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Vacation Leave Application.aspx?TransId=" + e.CommandArgument.ToString() + "&&EmpId=" + e.CommandName.ToString() + " ','window','width=1024');", true);
    }
    public void PrintReport(string LeaveId, string EmpId)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Vacation Leave Application.aspx?TransId=" + LeaveId + "&&EmpId=" + EmpId.ToString() + " ','window','width=1024');", true);

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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Leave_DtLeaveStatus1"];
            DataView view = new DataView();

            if (ddlFieldName.SelectedValue.Trim() == "Application_Date")
            {
                view = new DataView(dtCust, "Application_Date='" + Convert.ToDateTime(txtValueDate.Text).ToString() + "'", "", DataViewRowState.CurrentRows);
            }
            else if (ddlFieldName.SelectedValue.Trim() == "From_Date")
            {
                view = new DataView(dtCust, "From_Date='" + Convert.ToDateTime(txtValueDate.Text) + "' or To_Date='" + Convert.ToDateTime(txtValueDate.Text) + "'", "", DataViewRowState.CurrentRows);
            }
            else
            {
                try
                {
                    view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
                }
                catch
                {

                }
            }

            try
            {
                Session["Leave_DtLeaveStatus1_Filter"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
                //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
                objPageCmn.FillData((object)gvLeaveStatus, view.ToTable(), "", "");
            }
            catch (Exception ex)
            { 
            
            }

          

            AllPageCode();
            txtValue.Focus();

        }

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Focus();
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        ddlOption.Enabled = true;
    }

    protected void gvLeaveStatus_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Leave_DtLeaveStatus1_Filter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["Leave_DtLeaveStatus1_Filter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvLeaveStatus, dt, "", "");
        AllPageCode();
    }

    protected void GvLeaveSummary_Approved_Shorting(object sender,GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Leave_DtEmpLeave_Approved"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["Leave_DtEmpLeave_Approved"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvLeaveSumary_Approved, dt, "", "");
        AllPageCode();
    }

    protected void ddlLeaveStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
        AllPageCode();
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValueDate.Text = "";

        txtValue.Text = "";
        if (ddlFieldName.SelectedItem.Text == "Apply Date" || ddlFieldName.SelectedItem.Text == "Leave Date")
        {
            txtValueDate.Visible = true;

            txtValue.Visible = false;
            ddlOption.Enabled = false;
            ddlOption.SelectedIndex = 1;
        }
        else
        {
            ddlOption.Enabled = true;
            txtValueDate.Visible = false;

            txtValue.Visible = true;
        }
    }


    #region Upload Attachment Added By Lokesh On 15-02-2023
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpeg") && (ext != ".pdf"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge ,.pdf  extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll"))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll");
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                string TransId = hidtransId.Value;

                string a = objleaveReq.UpdateImagePath(TransId, FULogoPath.FileName, "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName);
                Session["Leave_DtEmpLeave_Pending"] = null;
                FillPendingLeave(hdnEmpId.Value);
                Session["empimgpath"] = FULogoPath.FileName;
                Session["empimgpathFull"] = "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName;
            }
        }
        else if (FULogoPath1.HasFile)
        {

            string ext = FULogoPath1.FileName.Substring(FULogoPath1.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpeg") && (ext != ".pdf"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge ,.pdf  extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll"))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll");
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/") + FULogoPath1.FileName;
                FULogoPath1.SaveAs(path);
                string TransId = hidtransId.Value;

                string a = objleaveReq.UpdateImagePath(TransId, FULogoPath1.FileName, "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath1.FileName);
                Session["Leave_DtEmpLeave_Pending"] = null;
                FillPendingLeave(hdnEmpId.Value);
                Session["empimgpath"] = FULogoPath1.FileName;
                Session["empimgpathFull"] = "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath1.FileName;
            }


        }
    }

    public void OnDownloadDocumentCommand1(object sender, CommandEventArgs e)
    {
        DataTable dtLeaveDetail = GetLeaveDetail();
        dtLeaveDetail = new DataView(dtLeaveDetail, "Trans_id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        downloadfile(dtLeaveDetail);
        //resetfile();
        Page page = new Page();
    }
    public void downloadfile(DataTable dt)
    {
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.AddHeader("Content-Disposition", "attachment; filename=" + dt.Rows[0]["Field4"].ToString() + ";");
        response.TransmitFile(Server.MapPath(dt.Rows[0]["Field5"].ToString().Replace("~/", "~//")));
        response.Flush();
        response.End();
    }


    //for Pending Grid
    public void OnDownloadDocumentCommandPending(object sender, CommandEventArgs e)
    {
        try
        {
            string empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(empid);

            DataTable dtLeave = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Emp_ID);
            if (dtLeave.Rows.Count > 0)
            {
                dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id=" + Emp_ID + "", "", DataViewRowState.CurrentRows).ToTable();
                dtLeave = new DataView(dtLeave, "Trans_id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    downloadfile(dtLeave);
                    //resetfile();
                    Page page = new Page();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void OnDownloadDocumentCommandApproved(object sender, CommandEventArgs e)
    {
        try
        {
            string empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(empid);

            DataTable dtLeave = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Emp_ID);
            if (dtLeave.Rows.Count > 0)
            {
                dtLeave = new DataView(dtLeave, "Is_Approved='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id=" + Emp_ID + "", "From_Date", DataViewRowState.CurrentRows).ToTable();
                dtLeave = new DataView(dtLeave, "Trans_id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    downloadfile(dtLeave);
                    //resetfile();
                    Page page = new Page();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public void OnDownloadDocumentCommandList(object sender, CommandEventArgs e)
    {
        try
        {


            DataTable dtLeave = new DataTable();
            if (Request.QueryString["Emp_Id"] == null)
            {
                dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code, Set_EmployeeMaster.Emp_Name, Att_LeaveRequest_header.TotalDays as DaysCount,cast(Att_LeaveRequest_header.CreatedDate as date) as Application_Date, Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Att_LeaveRequest_header.Is_Pending = 1 then 'Pending' when Att_LeaveRequest_header.Is_Approved = 1 then 'Approved'when Att_LeaveRequest_header.Is_Canceled = 1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7,Att_Leave_Request.Field4, Att_Leave_Request.Field5 from Att_LeaveRequest_header inner join Att_Leave_Request on Att_Leave_Request.Field2 = Att_LeaveRequest_header.Trans_Id inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id in (" + ddlLoc.SelectedValue.Trim() + ") and Set_EmployeeMaster.Field2 = 'False'  order by cast(Set_EmployeeMaster.Emp_Code as int), From_date desc");

                //dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name , Att_LeaveRequest_header.TotalDays as DaysCount, cast( Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Is_Pending=1 then 'Pending' when Is_Approved=1 then 'Approved' when Is_Canceled=1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7 from Att_LeaveRequest_header inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id in (" + ddlLoc.SelectedValue.Trim() + ") and Set_EmployeeMaster.Field2='False'  order by cast(Set_EmployeeMaster.Emp_Code as int),   From_date desc  ");
                //dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id,Set_EmployeeMaster.Emp_Code, Set_EmployeeMaster.Emp_Name,DATEDIFF(DAY, Att_Leave_Request.From_Date, Att_Leave_Request.To_date) + 1 as DaysCount,cast(Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_Leave_Request.From_Date, Att_Leave_Request.To_date,case when Att_LeaveRequest_header.Is_Pending = 1 then 'Pending' when Att_LeaveRequest_header.Is_Approved = 1 then 'Approved' when Att_LeaveRequest_header.Is_Canceled = 1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7,Att_Leave_Request.Field4, Att_Leave_Request.Field5 from Att_LeaveRequest_header inner join Att_Leave_Request on Att_Leave_Request.Field2 = Att_LeaveRequest_header.Trans_Id inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id = " + Session["LocId"].ToString() + " and Att_LeaveRequest_header.emp_id = " + Session["EmpId"].ToString() + " order by cast(Set_EmployeeMaster.Emp_Code as int), From_date desc");

                //dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id,Set_EmployeeMaster.Emp_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name , Att_LeaveRequest_header.TotalDays as DaysCount, cast( Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Is_Pending=1 then 'Pending' when Is_Approved=1 then 'Approved' when Is_Canceled=1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7 from Att_LeaveRequest_header inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id=" + Session["LocId"].ToString() + " and Att_LeaveRequest_header.emp_id=" + Session["EmpId"].ToString() + "  order by cast(Set_EmployeeMaster.Emp_Code as int), From_date desc   ");
                // dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


            }

            if (dtLeave.Rows.Count > 0)
            {
                if (ddlLeaveStatus.SelectedIndex == 1)
                {
                    dtLeave = new DataView(dtLeave, "Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (ddlLeaveStatus.SelectedIndex == 2)
                {
                    dtLeave = new DataView(dtLeave, "Status='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else if (ddlLeaveStatus.SelectedIndex == 3)
                {
                    dtLeave = new DataView(dtLeave, "Status='Rejected'", "", DataViewRowState.CurrentRows).ToTable();
                }

                dtLeave = new DataView(dtLeave, "Trans_id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    downloadfile(dtLeave);
                    //resetfile();
                    Page page = new Page();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }



    #endregion

  

}
