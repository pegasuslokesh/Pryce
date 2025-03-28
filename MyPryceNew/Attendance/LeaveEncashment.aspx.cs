using PegasusDataAccess;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_LeaveEncashment : System.Web.UI.Page
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
    EmployeeParameter objEmpParam = null;

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
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
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

            if (Request.QueryString["Emp_Id"] != null)
            {
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Request.QueryString["Emp_Id"] == null ? "616" : "617", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }

                txtEmpName.Text = Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                hdnEmpId.Value = Session["EmpId"].ToString();
                txtEmpName.Enabled = false;
                FillLeaveStatus();
                FillLeaveSummary(Session["EmpId"].ToString());
                FillApproveLeave(Session["EmpId"].ToString());
                //lblHeader.Text = Resources.Attendance.Employee_Leave_Request_Setup;
                lblHeader.Text = "Employee Leave Encashment";
                ddlLoc.Visible = false;
            }
            else
            {
                ddlLoc.Visible = true;
                objPageCmn.fillLocationWithAllOption(ddlLoc);
                FillLeaveStatus();
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "616", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
                //lblHeader.Text = Resources.Attendance.HR_Leave_Request;
                lblHeader.Text = "HR Leave Encashment";
            }
            if (TotalPayAmount.Text == "")
            {

                lblTotalPayAmount.Visible = false;
            }

            txtRequestDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            txtRemRequestDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");
            CalendarExtendertxtValueDate.Format = objSys.SetDateFormat();
        }

        CalendarExtender1.Format = objSys.SetDateFormat();
        CalendarExtender2.Format = objSys.SetDateFormat();
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
            dtModule = objObjectEntry.GetModuleIdAndName("616", (DataTable)Session["ModuleName"]);
        }
        else
        {
            dtModule = objObjectEntry.GetModuleIdAndName("617", (DataTable)Session["ModuleName"]);
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
                Update_Remainig.Visible = true;
                Li_RemUpdate.Visible = true;
            }
            else
            {
                btnApply.Visible = false;

            }
            try
            {
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
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "616", HttpContext.Current.Session["CompId"].ToString());
            }
            else
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "617", HttpContext.Current.Session["CompId"].ToString());
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
                        Update_Remainig.Visible = true;
                        Li_RemUpdate.Visible = true;
                    }
                    else
                    {
                        btnApply.Visible = false;
                    }
                    try
                    {
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
                            Update_Remainig.Visible = true;
                            Li_RemUpdate.Visible = true;
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
            dtLeave = ObjDa.return_DataTable("Select Att_Leave_Encashment_Header.Trans_Id, Att_Leave_Encashment_Header.Emp_Id, Emp_Code,Emp_Name,Request_Date,case when Att_Leave_Encashment_Header.Is_Pending = 1 then 'Pending' when Att_Leave_Encashment_Header.Is_Approved = 1 then 'Approved'when Att_Leave_Encashment_Header.Is_Canceled = 1 then 'Rejected' end as Status, Description, (Att_Leave_Encashment_Header.Field1)as DaysCount,(Att_Leave_Encashment_Header.Field2)as TotalPay From Att_Leave_Encashment_Header inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Id = Att_Leave_Encashment_Header.Emp_Id where Set_EmployeeMaster.Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' order by Att_Leave_Encashment_Header.Trans_Id Desc");
            //dtLeave = ObjDa.return_DataTable("select Att_LeaveRequest_header.Trans_id, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name , Att_LeaveRequest_header.TotalDays as DaysCount, cast( Att_LeaveRequest_header.CreatedDate as date) as Application_Date,Att_LeaveRequest_header.From_Date,Att_LeaveRequest_header.To_date,case when Is_Pending=1 then 'Pending' when Is_Approved=1 then 'Approved' when Is_Canceled=1 then 'Rejected' end as Status, Att_LeaveRequest_header.Field7 as Field7 from Att_LeaveRequest_header inner join Set_EmployeeMaster on Att_LeaveRequest_header.Emp_Id = Set_EmployeeMaster.Emp_Id where Set_EmployeeMaster.Location_Id in (" + ddlLoc.SelectedValue.Trim() + ") and Set_EmployeeMaster.Field2='False'  order by cast(Set_EmployeeMaster.Emp_Code as int),   From_date desc  ");
            //dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtLeave = ObjDa.return_DataTable("Select Att_Leave_Encashment_Header.Trans_Id,Att_Leave_Encashment_Header.Emp_Id,Emp_Code,Emp_Name,Request_Date,case when Att_Leave_Encashment_Header.Is_Pending = 1 then 'Pending' when Att_Leave_Encashment_Header.Is_Approved = 1 then 'Approved'when Att_Leave_Encashment_Header.Is_Canceled = 1 then 'Rejected' end as Status, Description, (Att_Leave_Encashment_Header.Field1)as DaysCount,(Att_Leave_Encashment_Header.Field2)as TotalPay From Att_Leave_Encashment_Header inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Id = Att_Leave_Encashment_Header.Emp_Id where Att_Leave_Encashment_Header.Emp_Id='" + HttpContext.Current.Session["EmpId"].ToString() + "' And Set_EmployeeMaster.Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' order by Att_Leave_Encashment_Header.Trans_Id Desc");
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

        }
        else
        {
            //gvLeaveStatus.DataSource = null;
            //gvLeaveStatus.DataBind();
        }

        Session["Leave_DtLeaveStatus1"] = dtLeave;
        Session["Leave_DtLeaveStatus1_Filter"] = dtLeave;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtLeave.Rows.Count.ToString() + "";

        AllPageCode();
    }
    public void FillLeaveSummary(string EmpId)
    {

        string empid = string.Empty;
        empid = EmpId;
        if (empid != "")
        {
            DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpIdForAnnualLeave(empid);
            ///DataTable dtLeaveSummary = ObjDa.return_DataTable("");

            dtLeaveSummary = new DataView(dtLeaveSummary, "Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

            objPageCmn.FillData((object)gvLeaveSummary, dtLeaveSummary, "", "");
            Session["Leave_DtLeaveStatus"] = dtLeaveSummary;
        }
        AllPageCode();
    }

    public void FillRemaainigLeave(string EmpId)
    {

        string empid = string.Empty;
        empid = EmpId;

        if (empid != "")
        {
            DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(empid);


            dtLeaveSummary = new DataView(dtLeaveSummary, "Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

            objPageCmn.FillData((object)gvLeaveRemainingChanges, dtLeaveSummary, "", "");
            //Session["Leave_DtLeaveStatus"] = dtLeaveSummary;
        }
        AllPageCode();
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {

        string Emp_Name = txtEmpName.Text;
        int index = Emp_Name.IndexOf("/");
        if (index > 0)
            Emp_Name = Emp_Name.Substring(0, index);

        string Rep_Emp_Name = "";
        int Rep_index = Rep_Emp_Name.IndexOf("/");
        if (Rep_index > 0)
            Rep_Emp_Name = Rep_Emp_Name.Substring(0, Rep_index);

        if (Emp_Name == Rep_Emp_Name)
        {
            DisplayMessage("Employee name and replacement emp name cannot be same");
            return;
        }
        else
        {
            string empid = string.Empty;
            if (((TextBox)sender).ID.Trim() == "txtEmpName")
            {
                gvLeaveSummary.DataSource = null;
                gvLeaveSummary.DataBind();
                gvLeaveSumary_Approved.DataSource = null;
                gvLeaveSumary_Approved.DataBind();
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

    protected void txtRemEmpName_textChanged(object sender, EventArgs e)
    {

        string Emp_Name = txtEmpNameRem.Text;
        int index = Emp_Name.IndexOf("/");
        if (index > 0)
            Emp_Name = Emp_Name.Substring(0, index);

        string Rep_Emp_Name = "";
        int Rep_index = Rep_Emp_Name.IndexOf("/");
        if (Rep_index > 0)
            Rep_Emp_Name = Rep_Emp_Name.Substring(0, Rep_index);

        if (Emp_Name == Rep_Emp_Name)
        {
            DisplayMessage("Employee name and replacement emp name cannot be same");
            return;
        }
        else
        {
            string empid = string.Empty;
            if (((TextBox)sender).ID.Trim() == "txtEmpName")
            {
                gvLeaveRemainingChanges.DataSource = null;
                gvLeaveRemainingChanges.DataBind();
                gvLeaveRemainingChanges.DataSource = null;
                gvLeaveRemainingChanges.DataBind();
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

                        if (((TextBox)sender).ID.Trim() == "txtEmpNameRem")
                        {
                            hdnRemEmpId.Value = empid;

                            FillRemaainigLeave(empid);
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
        }
        else
        {
            txtEmpName.Text = "";
            gvLeaveSummary.DataSource = null;
            gvLeaveSummary.DataBind();
            gvLeaveSumary_Approved.DataSource = null;
            gvLeaveSumary_Approved.DataBind();
            txtEmpName.Enabled = true;
            hdnEmpId.Value = "";
        }

        //ddlLeaveType.Items.Clear();
        //ddlLeaveType.DataSource = null;
        //ddlLeaveType.DataBind();
        //ddlLeaveType.Visible = false;
        txtRequestDate.Text = "";
        txtDescription.Text = "";
        lblTotalPayAmount.Visible = false;
        txtTotalLeaveDays.Text = "";
        txtTotalPayAmount.Text = "";
        TotalPayAmount.Text = "";
        txtEmpName.Focus();
        hdnTransid.Value = "";
        Session["Leave_DtEmpLeave_Approved"] = null;
        Session["Leave_DtEmpLeave_Pending"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        AllPageCode();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void Reset()
    {
        Session["Leave_DtLeaveStatus"] = "";
        // txtFromDate.Text = Convert.ToDateTime(txtToDate.Text).AddDays(1).ToString(objSys.SetDateFormat());
        Session["Leave_DtEmpLeave_Approved"] = null;
        Session["Leave_DtEmpLeave_Pending"] = null;
        txtDescription.Text = "";
        txtTotalLeaveDays.Text = "";
        txtTotalPayAmount.Text = "";
        lblTotalPayAmount.Visible = false;
        TotalPayAmount.Text = "";
        txtTotalLeaveDays.Text = "";
        txtTotalPayAmount.Text = "";
        txtRequestDate.Focus();
        gvLeaveRemainingChanges.DataSource = null;
        gvLeaveRemainingChanges.DataBind();
        hdnRemEmpId.Value = "";
        txtEmpNameRem.Text = "";
        FillLeaveSummary(hdnEmpId.Value);
        FillApproveLeave(hdnEmpId.Value);

    }

    protected void btnApply_Click(object sender, EventArgs e)
    {

        DataTable dtEmpInfo = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value);
        string strCompanyId = Session["CompId"].ToString();
        string strUserId = Session["UserId"].ToString();
        string strBrandId = Session["BrandId"].ToString();
        string strLocationId = Session["LocId"].ToString();
        string strTimeZoneId = HttpContext.Current.Session["TimeZoneId"].ToString();

        if (dtEmpInfo.Rows.Count > 0)
        {
            if (dtEmpInfo.Rows[0]["Location_Id"].ToString() != Session["LocId"].ToString())
            {
                DisplayMessage("You need to request from your registered Location.");
                return;
            }
        }
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
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("LeaveEncashment").Rows[0]["Approval_Level"].ToString();
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }
        bool LeaveFunctionality = false;
        int iLeaveTypeId = 0;
        string LeaveDays = "";
        foreach (GridViewRow gvr in gvLeaveSummary.Rows)
        {
            LeaveFunctionality = Common.LeaveApprovalFunctionality(((HiddenField)gvr.FindControl("hdnLeaveTypeId")).Value, Session["DBConnection"].ToString());
            TextBox txtLeaveDays = (TextBox)gvr.FindControl("txtLeaveDay");
            //Label lblPayAmount = (Label)gvr.FindControl("lblPayAmount");
            LeaveDays = txtLeaveDays.Text;
            iLeaveTypeId = int.Parse(((HiddenField)gvr.FindControl("hdnLeaveTypeId")).Value);
        }
        if (LeaveDays == "" || LeaveDays == "0")
        {
            DisplayMessage("Please enter Leave Days");
            return;
        }
        DataTable dtLeaveDetail = new DataTable();
        dtLeaveDetail = (DataTable)Session["Leave_DtLeaveStatus"];
        DateTime Doj = Convert.ToDateTime(objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpId.Value).Rows[0]["DOJ"].ToString());
        string chkValidation = CheckLeaveValidation(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, iLeaveTypeId.ToString(), DateTime.Parse(txtRequestDate.Text), hdnTransid.Value, ProbationMonth, IsProbationPeriod, Doj, 0, 0, dtLeaveDetail, LeaveFunctionality, HttpContext.Current.Session["TimeZoneId"].ToString());

        if (chkValidation != "")
        {
            DisplayMessage(chkValidation);
            return;
        }

        if (gvLeaveSummary.Rows.Count == 0)
        {
            DisplayMessage("Add Your Encashment Details");
            return;
        }
        if (txtTotalPayAmount.Text == "0" && txtTotalPayAmount.Text == "")
        {
            DisplayMessage("You Cannot Apply, You Have 0 Pay Amount");
            return;
        }
        string strsql = string.Empty;
        string strMaxId = string.Empty;
        DataTable dtTemp1 = new DataTable();

        SqlConnection con = new SqlConnection();

        DataTable dtObjectId = ObjDa.return_DataTable("Select Object_Id from IT_ObjectEntry Where Object_Name='Employee Leave Encashment Request'");

        string ObId = "0";
        if (dtObjectId.Rows.Count > 0)
        {
            ObId = dtObjectId.Rows[0]["Object_Id"].ToString();
        }
        DataTable dt1 = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ObId, hdnEmpId.Value);
        // dt1 = new DataView(dt1, "Approval='Leave'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt1.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }

        GetEmployeeName(hdnEmpId.Value);


        // strsql = "INSERT INTO [Att_LeaveRequest_header] ([Emp_Id] ,[From_Date] ,[To_Date] ,[TotalDays] ,[Is_Pending] ,[Is_Approved] ,[Is_Canceled] ,[Field1] ,[Field2] ,[Field3] ,[Field4] ,[Field5] ,[Field6] ,[Field7] ,[IsActive] ,[CreatedBy] ,[CreatedDate] ,[ModifiedBy] ,[ModifiedDate]) VALUES (" + strEmpId + " ,'" + dtFromDate.ToString() + "' ,'" + dtToDate.ToString() + "' ," + TotalDays.ToString() + " ,'" + IsPending.ToString() + "' ,'" + IsApproved.ToString() + "' ,'False' ,' ',' ',' ' ,' ' , ' ', '" + false.ToString() + "','" + Rejoin_Date.ToString() + "' ,'" + true.ToString() + "' ,'" + strUserId + "' , '" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString() + "','" + strUserId + "' ,'" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString() + "')";



        con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            TimeSpan time = DateTime.Now.TimeOfDay;
            DateTime date = Convert.ToDateTime(txtRequestDate.Text);

            ///string requstDate= txtRequestDate.Text+ time;           
            strsql = "INSERT INTO [dbo].[Att_Leave_Encashment_Header] ([Company_Id],[Emp_Id],[Request_Date],[Is_Pending],[Is_Approved],[Is_Canceled],[Description],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate]) VALUES('" + Session["CompId"].ToString() + "','" + hdnEmpId.Value + "','" + txtRequestDate.Text + "','True','False','False','" + txtDescription.Text + "','" + txtTotalLeaveDays.Text + "','" + txtTotalPayAmount.Text + "','','','','True','" + DateTime.Now + "','True','" + Session["UserId"].ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString() + "','" + DateTime.Now + "')";
            ObjDa.execute_Command(strsql, ref trns);
            strMaxId = ObjDa.return_DataTable("select MAX(Trans_id) from Att_Leave_Encashment_Header", ref trns).Rows[0][0].ToString();
            foreach (GridViewRow gvr in gvLeaveSummary.Rows)
            {
                Label lblLeaveName = (Label)gvr.FindControl("lblLeaveName");
                HiddenField hdnLeaveTypeId = (HiddenField)gvr.FindControl("hdnLeaveTypeId");
                Label lblScheduleType = (Label)gvr.FindControl("lblScheduleType");
                Label lblMonthName = (Label)gvr.FindControl("lblMonthNameNew");
                Label lblYearName = (Label)gvr.FindControl("lblYearName");
                Label lblPreviousDays = (Label)gvr.FindControl("lblPreviousDays");
                Label lblAssignDays = (Label)gvr.FindControl("lblassignDays");
                //Label lblTotalDays = (Label)gvr.FindControl("lblTotalDays");
                Label lblUsedDays = (Label)gvr.FindControl("lblUsedDays");
                Label lblUsedDaysPending = (Label)gvr.FindControl("lblUsedDaysPending");
                Label lblActualBalance = (Label)gvr.FindControl("lblActRemainingDays");
                Label lblApplicableBalance = (Label)gvr.FindControl("lblActRemainingDays");
                TextBox txtLeaveDays = (TextBox)gvr.FindControl("txtLeaveDay");
                Label lblPayAmount = (Label)gvr.FindControl("lblPayAmount");
                strsql = "INSERT INTO[dbo].[Att_Leave_Encashment_Detail]([Header_Ref_Id],[Leave_Type_Id],[ScheduleType],[Month],[Year],[Previous_Leave],[Assigned_Leave],[Used_Leave],[Pending_Approval],[ActualBalance],[ApplicableBalance],[Leave_Days],[PayAmount],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])VALUES('" + strMaxId + "','" + hdnLeaveTypeId.Value + "','" + lblScheduleType.Text + "','" + lblMonthName.Text + "','" + lblYearName.Text + "','" + lblPreviousDays.Text + "','" + lblAssignDays.Text + "','" + lblUsedDays.Text + "','" + lblUsedDaysPending.Text + "','" + lblActualBalance.Text + "','" + lblApplicableBalance.Text + "','" + txtLeaveDays.Text + "','" + lblPayAmount.Text + "','','','','','','True','" + DateTime.Now + "','True','" + Session["UserId"].ToString() + "','" + DateTime.Now + "','" + Session["UserId"].ToString() + "','" + DateTime.Now + "')";
                ObjDa.execute_Command(strsql, ref trns);
                //Rahul
                DataTable dtLeave = ObjDa.return_DataTable("select * from Att_Employee_Leave_Trans where Emp_Id=" + hdnEmpId.Value + " and Leave_Type_Id=" + hdnLeaveTypeId.Value + " and Field3='Open' and IsActive='True'", ref trns);

                //DataTable dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(strEmpId, strleaveTypeId, "0", year4, ref trns);
                int strPreviousDays = 0;
                string TransNo = string.Empty;
                double remain = 0;
                double useddays = 0;
                int totaldays = 0;
                double PaidRemain = 0;
                double TotalPaiddays = 0;
                double RemainPaidDays = 0;
                int PendingDays = 0;
                int dayscount = Convert.ToInt32(txtLeaveDays.Text);
                if (dtLeave.Rows.Count > 0)
                {

                    TransNo = dtLeave.Rows[0]["Trans_Id"].ToString();
                    remain = Convert.ToDouble(dtLeave.Rows[0]["Remaining_Days"].ToString());
                    totaldays = int.Parse(dtLeave.Rows[0]["Total_Days"].ToString());
                    TotalPaiddays = Convert.ToDouble(dtLeave.Rows[0]["Field1"].ToString());
                    RemainPaidDays = Convert.ToDouble(dtLeave.Rows[0]["Field2"].ToString());
                    PendingDays = int.Parse(dtLeave.Rows[0]["Pending_Days"].ToString());
                    useddays = Convert.ToDouble(dtLeave.Rows[0]["Used_Days"].ToString());

                    if (hdnTransid.Value != "")
                    {
                        PendingDays = PendingDays - strPreviousDays;
                        remain = remain + strPreviousDays;
                        RemainPaidDays = RemainPaidDays + strPreviousDays;
                        //PendingDays = 0;
                    }

                    //code end
                }

                remain = remain - dayscount;

                if (LeaveFunctionality)
                {
                    PendingDays = PendingDays + dayscount;
                }
                else
                {
                    useddays = useddays + dayscount;
                }

                // useddays = totaldays;
                // dayscount = dayscount + PendingDays;


                if (!Common.LeaveNegativeBalance(hdnLeaveTypeId.Value, HttpContext.Current.Session["DBConnection"].ToString()))
                {

                    if (RemainPaidDays > 0)
                    {
                        if (RemainPaidDays > dayscount)
                        {
                            PaidRemain = RemainPaidDays - dayscount;
                        }
                        else
                        {
                            PaidRemain = 0;
                        }
                    }
                }
                else
                {
                    if (TotalPaiddays > 0)
                    {
                        PaidRemain = RemainPaidDays - dayscount;
                    }
                }
                string year4 = string.Empty;
                int FinancialYearMonth = 0;
                try
                {
                    FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", strCompanyId, strBrandId, strLocationId, ref trns));
                }
                catch
                {
                    FinancialYearMonth = 1;
                }

                DateTime FinancialYearStartDate = new DateTime();
                DateTime FinancialYearEndDate = new DateTime();
                if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month < FinancialYearMonth)
                {

                    FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year - 1, FinancialYearMonth, 1);

                    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                }
                else
                {
                    FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, FinancialYearMonth, 1);
                    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);

                }


                if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
                {
                    year4 = FinancialYearStartDate.Year.ToString();


                }
                else
                {
                    year4 = FinancialYearStartDate.Year.ToString();


                }
                try
                {
                    objleaveReq.UpdateEmployeeLeaveTransactionByTransNo(TransNo, strCompanyId, hdnEmpId.Value, hdnLeaveTypeId.Value, year4, "0", "0", "0", "0", useddays.ToString(), remain.ToString(), PendingDays.ToString(), PaidRemain.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "0", ref trns);
                }
                catch (Exception ex)
                {
                    DisplayMessage("Leave not Assigned to employee");

                }
            }
            DisplayMessage("Leave submitted");
            //strMaxId = strResult[1].Trim();
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
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






        //for (int i = 0; i < dtTemp.Rows.Count; i++)
        //{
        //    con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        //    con.Open();
        //    SqlTransaction trns;
        //    trns = con.BeginTransaction();
        //    dtLeaveDetail = new DataView(dtTemp1, "Is_Approval='" + dtTemp.Rows[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //    try
        //    {
        //        dtFromDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "From_Date", DataViewRowState.CurrentRows).ToTable().Rows[0]["From_Date"].ToString());
        //        dtToDate = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "To_Date desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Date"].ToString());
        //        Rejoin_Date = Convert.ToDateTime(new DataView(dtLeaveDetail, "", "Field7", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field7"].ToString());

        //        TotalDays = Convert.ToInt32((dtToDate - dtFromDate).TotalDays) + 1;

        //        strResult = objleaveReq.SaveLeaveRequest(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), hdnEmpId.Value, dtFromDate, dtToDate, Rejoin_Date, TotalDays, dtLeaveDetail, hdnTransid.Value, Convert.ToBoolean(dtTemp.Rows[i][0].ToString()), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        //        if (strResult[0].Trim() == "")
        //        {
        //            DisplayMessage("Leave submitted");
        //            strMaxId = strResult[1].Trim();
        //            trns.Commit();
        //            if (con.State == System.Data.ConnectionState.Open)
        //            {
        //                con.Close();
        //            }
        //            trns.Dispose();
        //            con.Dispose();
        //        }
        //        else
        //        {
        //            DisplayMessage(strResult[0].Trim());
        //            trns.Rollback();
        //            if (con.State == System.Data.ConnectionState.Open)
        //            {

        //                con.Close();
        //            }
        //            trns.Dispose();
        //            con.Dispose();
        //            return;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
        //        trns.Rollback();
        //        if (con.State == System.Data.ConnectionState.Open)
        //        {

        //            con.Close();
        //        }
        //        trns.Dispose();
        //        con.Dispose();
        //        return;
        //    }


        //    //created common function for insert approval and notification 
        //    //created by jitendra upadhyay on 16/12/2017
        //    objleaveReq.InsertLeaveApproval(hdnEmpId.Value, EmpPermission, dt1, strMaxId, dtLeaveDetail, txtEmpName.Text, hdnTransid.Value, ViewState["Emp_Img"].ToString(), dtFromDate, dtToDate, true, Convert.ToBoolean(dtTemp.Rows[i][0].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString(), HttpContext.Current.Session["EmpId"].ToString());

        //}
        DateTime requestDate = Convert.ToDateTime(txtRequestDate.Text);
        objleaveReq.InsertLeaveEnCashmentApproval(hdnEmpId.Value, EmpPermission, dt1, strMaxId, dtLeaveDetail, txtEmpName.Text, hdnTransid.Value, ViewState["Emp_Img"].ToString(), requestDate, requestDate, true, Convert.ToBoolean("true"), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString(), HttpContext.Current.Session["EmpId"].ToString());
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        FillLeaveStatus();
        //PrintReport(strMaxId, hdnEmpId.Value);
        btnReset_Click(null, null);
    }
    protected void PreviousDaysChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)txt.Parent.Parent;

        TextBox txtRemDays = (TextBox)currentRow.FindControl("txtRemDays");
        Label lblUsedDays = (Label)currentRow.FindControl("lblRemUsedDays");
        Label lblAsignDays = (Label)currentRow.FindControl("lblRemAssignDays");
        TextBox lblPreviousDays = (TextBox)currentRow.FindControl("lblRemPreviousDays");
        Label lblLeaveName = (Label)currentRow.FindControl("lblRemLeaveName");

        double ActualBalance = Convert.ToDouble(lblAsignDays.Text) + Convert.ToDouble(lblPreviousDays.Text) - Convert.ToDouble(lblUsedDays.Text);
        txtRemDays.Text = ActualBalance.ToString();
    }
    protected void RemaningDaysChanged(object sender, EventArgs e)
    {

        TextBox txt = (TextBox)sender;
        GridViewRow currentRow = (GridViewRow)txt.Parent.Parent;

        TextBox txtRemDays = (TextBox)currentRow.FindControl("txtRemDays");
        Label lblUsedDays = (Label)currentRow.FindControl("lblRemUsedDays");
        Label lblAsignDays = (Label)currentRow.FindControl("lblRemAssignDays");
        TextBox lblPreviousDays = (TextBox)currentRow.FindControl("lblRemPreviousDays");
        Label lblLeaveName = (Label)currentRow.FindControl("lblRemLeaveName");

        if (Convert.ToDouble(lblAsignDays.Text) < Convert.ToDouble(txtRemDays.Text))
        {
            double ActualBalance = Convert.ToDouble(txtRemDays.Text) - Convert.ToDouble(lblAsignDays.Text) - Convert.ToDouble(lblUsedDays.Text);
            lblPreviousDays.Text = ActualBalance.ToString();
        }
    }
    protected void btnRemSave_Click(object sender, EventArgs e)
    {

        if (txtEmpNameRem.Text == "")
        {
            DisplayMessage("Employee Name Required");
            return;
        }
        foreach (GridViewRow gvr in gvLeaveRemainingChanges.Rows)
        {
            HiddenField hdnLeaveTypeId = (HiddenField)gvr.FindControl("hdnRemLeaveTypeId");
            TextBox txtRemDays = (TextBox)gvr.FindControl("txtRemDays");
            Label lblUsedDays = (Label)gvr.FindControl("lblRemUsedDays");
            Label lblAsignDays = (Label)gvr.FindControl("lblRemAssignDays");
            TextBox lblPreviousDays = (TextBox)gvr.FindControl("lblRemPreviousDays");
            Label lblLeaveName = (Label)gvr.FindControl("lblRemLeaveName");
            double ActualBalance = Convert.ToDouble(lblAsignDays.Text) + Convert.ToDouble(lblPreviousDays.Text) - Convert.ToDouble(lblUsedDays.Text);
            if (Convert.ToDouble(txtRemDays.Text) > ActualBalance)
            {
                DisplayMessage("Remaining Days is Less Then " + Convert.ToString(ActualBalance) + " in " + lblLeaveName.Text + "");
                return;
            }

        }
        foreach (GridViewRow gvr in gvLeaveRemainingChanges.Rows)
        {
            HiddenField hdnLeaveTypeId = (HiddenField)gvr.FindControl("hdnRemLeaveTypeId");
            TextBox txtRemDays = (TextBox)gvr.FindControl("txtRemDays");
            Label lblUsedDays = (Label)gvr.FindControl("lblRemUsedDays");
            Label lblAsignDays = (Label)gvr.FindControl("lblRemAssignDays");
            TextBox lblPreviousDays = (TextBox)gvr.FindControl("lblRemPreviousDays");
            int b = 0;
            b = ObjDa.execute_Command("Update Att_Employee_Leave_Trans Set Remaining_Days='" + txtRemDays.Text + "',Previous_Days='" + lblPreviousDays.Text + "' where Field3='Open' And Emp_Id='" + hdnRemEmpId.Value + "' And Leave_Type_Id='" + hdnLeaveTypeId.Value + "'");
            if (b != 0)
            {
                DisplayMessage("Leave Updates");
            }
        }

        Reset();
    }
    public string CheckLeaveValidation(string strCompanyId, string strBrandId, string strLocationId, string strEmpId, string strLeaveTypeId, DateTime dtFromdate, string strTransId, int ProbationMonth, bool IsProbationPeriod, DateTime DOJ, int remainingdayscount, int Applieddayscount, DataTable dtLeaveDetail, bool LeaveApprovalFunctionality, string strTimeZoneId, bool isLeaveExistsValidation = true)
    {
        Attendance objAttendance = new Attendance(Session["DBConnection"].ToString());
        DataTable dtleavesalary = new DataTable();
        dtLeaveDetail = new DataView(dtLeaveDetail, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();

        DateTime dtTodate = dtFromdate;
        string strValidation = string.Empty;
        DataTable dtPostedList = new DataTable();
        DataTable dtPostedList1 = new DataTable();
        DataTable dtApprovalMaster = new DataTable();
        string strMaxLeavesingleAttempt = string.Empty;
        int FinancialYearMonth = 0;
        dtPostedList = objAttLog.Get_Pay_Employee_Attendance(strEmpId, dtFromdate.Month.ToString(), dtFromdate.Year.ToString());
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", strCompanyId, strBrandId, strLocationId));
        }
        catch
        {
            FinancialYearMonth = 1;
        }

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }


        if (dtPostedList.Rows.Count > 0 || dtPostedList1.Rows.Count > 0)
        {
            strValidation = "Log Posted For This Date Criteria";

        }

        else if (strTransId == "" && ObjDa.return_DataTable("select Emp_Id from Att_Leave_Request where Emp_Id=" + strEmpId + " and Is_Pending='True' and Leave_Type_id=" + strLeaveTypeId + "").Rows.Count > 0)
        {
            strValidation = "you can not apply a new request because previous request is under process";

        }
        else if (strTransId != "" && ObjDa.return_DataTable("select Emp_Id from Att_Leave_Request where Emp_Id=" + strEmpId + " and Is_Pending='True' and Field2<>" + strTransId + " and Leave_Type_id=" + strLeaveTypeId + "").Rows.Count > 0)
        {
            strValidation = "you can not apply a new request because previous request is under process";
        }
        else if (!objAttendance.IsLeaveMaturity(strCompanyId, strEmpId, strLeaveTypeId, strTimeZoneId))
        {
            strValidation = "Employee not eligible";
        }
        else if (dtFromdate <= DOJ.AddMonths(ProbationMonth) && IsProbationPeriod == true)
        {
            strValidation = "You are not eligible for Leave request during Probation Period";
        }

        if (strValidation == "" && isLeaveExistsValidation)
        {
            string FullDay = objAttendance.GetLeaveApprovalStatus(strCompanyId, strEmpId, dtFromdate, dtFromdate, "FullDay", 0, strBrandId, strLocationId, strTimeZoneId);

            if (FullDay != string.Empty && strTransId == "")
            {
                strValidation = FullDay.ToString();

            }
        }


        if (strValidation == "")
        {
            string HalfDay = objAttendance.GetLeaveApprovalStatus(strCompanyId, strEmpId, dtFromdate, dtFromdate, "HalfDay", 0, strBrandId, strLocationId, strTimeZoneId);
            if (HalfDay != string.Empty)
            {
                strValidation = HalfDay.ToString();
            }
        }

        if (strValidation == "")
        {
            string PartialDay = objAttendance.GetLeaveApprovalStatus(strCompanyId, strEmpId, dtFromdate, dtFromdate, "PartialLeave", 0, strBrandId, strLocationId, strTimeZoneId);
            if (PartialDay != string.Empty)
            {
                strValidation = PartialDay.ToString();
            }
        }

        if (strValidation == "")
        {
            dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid(strCompanyId, strBrandId, strLocationId, "56", strEmpId);
            if (dtApprovalMaster.Rows.Count == 0)
            {
                strValidation = "Approval setup issue , please contact to your admin";

            }
        }


        if (strValidation == "")
        {
            //if negative balance flag was false then validation will work 
            if (!Common.LeaveNegativeBalance(strLeaveTypeId, Session["DBConnection"].ToString()))
            {
                if (Applieddayscount > remainingdayscount)
                {
                    strValidation = "Employee does not have Sufficient Leave";

                }
            }
        }



        //if (strValidation == "")
        //{
        //    while (dtFromdate <= dtTodate)
        //    {
        //        if (new DataView(dtLeaveDetail, "(From_Date<='" + dtFromdate + "' and To_Date>='" + dtFromdate + "') or (From_Date='" + dtFromdate + "') or ( To_Date='" + dtFromdate + "') ", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
        //        {
        //            strValidation = "Leave Already Applied between selected date criteria";
        //            break;
        //        }
        //        dtFromdate = dtFromdate.AddDays(1);
        //    }
        //}

        if (strValidation == "")
        {
            dtleavesalary = ObjDa.return_DataTable("select emp_id  from att_leavesalary where emp_id=" + strEmpId + " and F5='Pending' and Leave_Type_Id=" + strLeaveTypeId + "");

            if (dtleavesalary.Rows.Count > 0)
            {
                strValidation = "Leave salary request is pending";
            }
        }

        dtPostedList.Dispose();
        dtPostedList1.Dispose();
        dtApprovalMaster.Dispose();
        //dtLeaveSummary.Dispose();



        return strValidation;
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

        txtRequestDate.Text = "";
        txtEmpName.Text = "";
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

    public string GetDate(object obj)
    {

        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());



        return Date.ToString(objSys.SetDateFormat());

    }


    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);

    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //DataTable dtleaveDetail = ObjDa.return_DataTable("select *,(select  COUNT(*) from Att_Leave_Encashment_Detail where Header_Ref_Id = Att_Leave_Encashment_Header.Trans_Id) as LeaveCount from Att_Leave_Encashment_Header where Trans_Id=" + e.CommandArgument.ToString() + "");
        DataTable dtleaveHeader = ObjDa.return_DataTable("Select Att_Leave_Encashment_Header.Emp_Id,Emp_Name,FORMAT (Att_Leave_Encashment_Header.Request_Date, 'dd-MMM-yyyy ') as Request_Date ,(Att_Leave_Encashment_Header.Field1) as TotalLeaveDasy,(Att_Leave_Encashment_Header.Field2)as TotalPayAmount,Att_Leave_Encashment_Header.Description From Att_Leave_Encashment_Header inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Id=Att_Leave_Encashment_Header.Emp_Id where Att_Leave_Encashment_Header.Trans_Id='" + e.CommandArgument.ToString() + "'");
        //dtleaveDetail = dtleaveDetail.DefaultView.ToTable(true, "Trans_id", "Emp_Id", "Leave_Type_id", "From_date", "To_Date", "Field1", "Emp_Description", "Field3", "LeaveCount", "Field7", "Field4", "Field5");
        DataTable dtleaveDetail = ObjDa.return_DataTable("Select Att_LeaveMaster.Leave_Name,Att_Leave_Encashment_Detail.Leave_Type_Id,(Att_Leave_Encashment_Detail.ScheduleType) as Shedule_Type, Att_Leave_Encashment_Detail.Month,(Att_Leave_Encashment_Detail.Month) as MonthName, Att_Leave_Encashment_Detail.Year,(Att_Leave_Encashment_Detail.Previous_Leave) as Previous_Days, (Att_Leave_Encashment_Detail.Assigned_Leave) as Assign_Days,(Att_Leave_Encashment_Detail.Used_Leave) as Used_Days, (Att_Leave_Encashment_Detail.Pending_Approval) as Pending_Days,(Att_Leave_Encashment_Detail.ApplicableBalance) as Remaining_Days, Att_Leave_Encashment_Detail.Leave_Days,Att_Leave_Encashment_Detail.PayAmount, (Select(Sum(Cast(Att_Leave_Encashment_Detail.Previous_Leave as float)) +Sum(Cast(Att_Leave_Encashment_Detail.Assigned_Leave as float)) * Sum(MONTH(Cast(Att_Leave_Encashment_Detail.CreatedDate as Date)) - 1) / Sum(12)) - (Sum(Cast(Att_Leave_Encashment_Detail.Used_Leave as Float)) + Sum(Cast(Att_Leave_Encashment_Detail.Pending_Approval as Float))) From Att_Leave_Encashment_Detail where Att_Leave_Encashment_Detail.Header_Ref_Id = '" + e.CommandArgument.ToString() + "') as ActualBalance,(Select(Sum(Cast(Att_Leave_Encashment_Detail.Assigned_Leave as float)) * Sum(MONTH(Att_Leave_Encashment_Detail.CreatedDate) - 1) / Sum(12)) From Att_Leave_Encashment_Detail where Att_Leave_Encashment_Detail.Header_Ref_Id = '" + e.CommandArgument.ToString() + "') as ThisYearAssignLeave From Att_Leave_Encashment_Detail inner join Att_LeaveMaster on Att_Leave_Encashment_Detail.Leave_Type_Id = Att_LeaveMaster.Leave_Id where Header_Ref_Id='" + e.CommandArgument.ToString() + "'");

        // objPageCmn.FillData((GridView)gvleaveDetail, dtleaveDetail, "", "");

        hdnTransid.Value = e.CommandArgument.ToString();
        txtEmpName.Text = dtleaveHeader.Rows[0]["Emp_Name"].ToString();
        hdnEmpId.Value = dtleaveHeader.Rows[0]["Emp_Id"].ToString();
        txtEmpName.Enabled = false;
        objPageCmn.FillData((object)gvLeaveSummary, dtleaveDetail, "", "");
        Session["Leave_DtLeaveStatus"] = dtleaveDetail;
        foreach (GridViewRow gvr in gvLeaveSummary.Rows)
        {
            HiddenField hdnLeaveTypeId = (HiddenField)gvr.FindControl("hdnLeaveTypeId");
            TextBox txtLeaveDays = (TextBox)gvr.FindControl("txtLeaveDay");
            Label lblPayAmount = (Label)gvr.FindControl("lblPayAmount");
            for (int i = 0; i < dtleaveDetail.Rows.Count; i++)
            {
                if (hdnLeaveTypeId.Value == dtleaveDetail.Rows[i]["Leave_Type_Id"].ToString())
                {
                    txtLeaveDays.Text = dtleaveDetail.Rows[i]["Leave_Days"].ToString();
                    lblPayAmount.Text = dtleaveDetail.Rows[i]["PayAmount"].ToString();
                }
            }
        }
        txtTotalLeaveDays.Text = dtleaveHeader.Rows[0]["TotalLeaveDasy"].ToString();
        txtTotalPayAmount.Text = dtleaveHeader.Rows[0]["TotalPayAmount"].ToString();
        txtDescription.Text = dtleaveHeader.Rows[0]["Description"].ToString();
        txtRequestDate.Text = dtleaveHeader.Rows[0]["Request_Date"].ToString();
        //FillLeaveSummary(hdnEmpId.Value);
        FillApproveLeave(hdnEmpId.Value);

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
    public void FillApproveLeave(string Empid)
    {
        //Commented by Ghanshyam Suthar on 06/12/2017
        //DataTable dtLeave = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        // DataTable dtLeave = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Empid);
        DataTable dtLeave = objleaveReq.GetLeaveEncashment_ByEmpId(Empid);
        if (dtLeave.Rows.Count > 0)
        {
            //dtLeave = new DataView(dtLeave, "Is_Approved='True'", "Request_Date", DataViewRowState.CurrentRows).ToTable();
            if (dtLeave.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSumary_Approved, dtLeave, "", "");
                Session["Leave_DtEmpLeave_Approved"] = dtLeave;
            }
            else
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLeaveSumary_Approved, dtLeave, "", "");
                Session["Leave_DtEmpLeave_Approved"] = null;
            }

            double total = 0;
            foreach (GridViewRow row in gvLeaveSumary_Approved.Rows)
            {
                var numberLabel = row.FindControl("PayAmount") as Label;
                double numberLabel1 = Convert.ToDouble(numberLabel.Text);
                //int number=Convert.ToInt32(numberLabel1);
                total += numberLabel1;

            }

            lblTotalPayAmount.Visible = true;
            TotalPayAmount.Text = Convert.ToString(total);

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
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Vacation Leave Application.aspx?TransId=" + e.CommandArgument.ToString() + "&&EmpId=" + e.CommandName.ToString() + " ','window','width=1024');", true);
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
                view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            }

            Session["Leave_DtLeaveStatus1_Filter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvLeaveStatus, view.ToTable(), "", "");

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

    protected void OnTextChanged(object sender, EventArgs e)
    {
        double Basic_Salary = 0;
        double Per_Day_Salary = 0;
        double TotalAmount = 0;
        double TotalLeaveDays = 0;
        string strMessage = "";
        //Reference the TextBox.
        TextBox textBox = sender as TextBox;

        //Get the ID of the TextBox.
        string id = textBox.ID;

        string regexString = @"^\d+?$";

        Regex regex = new Regex(regexString);

        if (textBox.Text.Length == 0)
        {
            strMessage = "Length is valid!";
        }
        else
        {
            if (regex.IsMatch(textBox.Text))
            {
                // strMessage = "only numbers";
                Basic_Salary = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(hdnEmpId.Value, "Basic_Salary"));

                Per_Day_Salary = Basic_Salary / 26;

                TotalAmount = 0;
                TotalLeaveDays = 0;
                foreach (GridViewRow gvr in gvLeaveSummary.Rows)
                {
                    Label lblActualBalance = (Label)gvr.FindControl("lblActRemainingDays");
                    Label lblAmount = (Label)gvr.FindControl("lblPayAmount");
                    TextBox txtLeaveDays = (TextBox)gvr.FindControl("txtLeaveDay");
                    if (lblActualBalance.Text != "")
                    {
                        if (float.Parse(lblActualBalance.Text) < float.Parse(txtLeaveDays.Text))
                        {
                            txtLeaveDays.Text = "";
                            DisplayMessage("You Cannot Apply More then Actual Balance");
                            break;
                        }
                    }
                    else
                    {
                        txtLeaveDays.Text = "";
                        DisplayMessage("You have no balance to Apply");
                        break;
                    }


                    if (txtLeaveDays.Text != "")
                    {
                        lblAmount.Text = (float.Parse(txtLeaveDays.Text) * float.Parse(Per_Day_Salary.ToString())).ToString();
                    }
                    else
                    {
                        lblAmount.Text = "0.00";
                    }

                    if (TotalAmount == 0)
                    {
                        TotalAmount = Convert.ToDouble(lblAmount.Text);
                    }
                    else
                    {
                        TotalAmount = TotalAmount + Convert.ToDouble(lblAmount.Text);
                    }

                    if (TotalLeaveDays == 0)
                    {
                        TotalLeaveDays = Convert.ToDouble(txtLeaveDays.Text);
                        txtTotalLeaveDays.Text = Convert.ToString(TotalLeaveDays);
                    }
                    else
                    {
                        TotalLeaveDays = TotalLeaveDays + Convert.ToDouble(txtLeaveDays.Text);
                        txtTotalLeaveDays.Text = Convert.ToString(TotalLeaveDays);
                    }
                }
            }
            else
            {
                strMessage = "Not numbers !!";
                textBox.Text = "";
                DisplayMessage(strMessage);
                foreach (GridViewRow gvr in gvLeaveSummary.Rows)
                {
                    TextBox txtLeaveDays = (TextBox)gvr.FindControl("txtLeaveDay");
                    Label lblAmount = (Label)gvr.FindControl("lblPayAmount");
                    if (txtLeaveDays.Text == "")
                    {
                        lblAmount.Text = "0.0";
                    }
                }
            }
        }


        if (TotalAmount != 0)
        {
            txtTotalPayAmount.Text = TotalAmount.ToString();
        }
        else
        {
            txtTotalPayAmount.Text = "0";
        }

    }

}