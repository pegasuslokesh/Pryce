using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_OverTimeRequest : BasePage
{
    Common cmn = null;
    Att_AttendanceLog objAttLog = null;
    Attendance objAttendance = null;
    Att_OverTime_Request objOvertimeReq = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Set_Approval_Employee objApproalEmp = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeParameter objEmpParam = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        objOvertimeReq = new Att_OverTime_Request(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/OverTimeRequest.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtEmpName.Focus();
            FillLeaveStatus();

        }

        CalendarExtender1.Format = objSys.SetDateFormat();

        //AllPageCode();
        if (Settings.IsValid == false)
        {
            btnApply.Visible = false;
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnApply.Visible = clsPagePermission.bAdd;
        gvOvertimeStatus.Columns[0].Visible = clsPagePermission.bEdit;
        gvOvertimeStatus.Columns[1].Visible = clsPagePermission.bEdit;
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    public string GetOverTimeStatus(object TransId)
    {
        string status = string.Empty;
        DataTable dt = objOvertimeReq.GetOvertimeRequestByCompany(Session["CompId"].ToString());
        dt = new DataView(dt, "Trans_Id='" + TransId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
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
    protected void gvOvertimeStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOvertimeStatus.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtLeaveStatus"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvOvertimeStatus, dt, "", "");

        foreach (GridViewRow gvr in gvOvertimeStatus.Rows)
        {
            Label lblStatus = (Label)(gvr.FindControl("lblStatus"));
            if (lblStatus.Text != "Pending")
            {
                ImageButton imgBtnApprove = (ImageButton)(gvr.FindControl("IbtnApprove"));
                ImageButton imgBtnReject = (ImageButton)(gvr.FindControl("IbtnReject"));
                imgBtnApprove.Visible = false;
                imgBtnReject.Visible = false;
            }
        }
    }
    public void FillLeaveStatus()
    {
        DataTable dtLeave = objOvertimeReq.GetOvertimeRequestByCompany(Session["CompId"].ToString());
        dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtLeave.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvOvertimeStatus, dtLeave, "", "");
            // Session["dtLeaveStatus"] = dtLeave;
            foreach (GridViewRow gvr in gvOvertimeStatus.Rows)
            {
                Label lblStatus = (Label)(gvr.FindControl("lblStatus"));
                if (lblStatus.Text != "Pending")
                {
                    ImageButton imgBtnApprove = (ImageButton)(gvr.FindControl("IbtnApprove"));
                    ImageButton imgBtnReject = (ImageButton)(gvr.FindControl("IbtnReject"));
                    imgBtnApprove.Visible = false;
                    imgBtnReject.Visible = false;
                }
            }
        }
        else
        {
            gvOvertimeStatus.DataSource = null;
            gvOvertimeStatus.DataBind();
        }
        //AllPageCode();
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        bool IsCompOT = false;
        bool IsEmpOT = false;
        string OvetimeApproval = string.Empty;
        OvetimeApproval = objAppParam.GetApplicationParameterValueByParamName("OverTime Approval", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        string empid = string.Empty;
        if (IsCompOT)
        {
            if (OvetimeApproval == "1")
            {
                if (txtEmpName.Text != "")
                {
                    empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];

                    DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                    dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtEmp.Rows.Count > 0)
                    {
                        empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                        hdnEmpId.Value = empid;
                        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpId.Value, Session["CompId"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            IsEmpOT = Convert.ToBoolean(dt.Rows[0]["Is_OverTime"].ToString());
                            if (IsEmpOT)
                            {
                                FillApproveLeave(empid);
                                FillPendingLeave(empid);
                            }
                            else
                            {
                                DisplayMessage("Emloyee has no Permission for Overtime");
                                txtEmpName.Text = "";
                                txtEmpName.Focus();
                                return;
                            }
                        }
                        else
                        {
                            DisplayMessage("Emloyee has no Overtime Parameter");
                            txtEmpName.Text = "";
                            txtEmpName.Focus();
                            return;
                        }


                    }
                    else
                    {
                        DisplayMessage("Employee Not Exists");
                        txtEmpName.Text = "";
                        txtEmpName.Focus();
                        hdnEmpId.Value = "";
                        return;
                    }
                }
            }
            else
            {
                DisplayMessage("Your Parameter for Overtime Approval is Not Set");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Company Not Provide Overtime");
            txtEmpName.Text = "";
            txtEmpName.Focus();
            return;
        }
    }
    protected void txtTimeDuration_OnTextChanged(object sender, EventArgs e)
    {
        if (txtTimeDuration.Text == "")
        {
            bool FromTime = CheckValid(txtFromTime, "From Time Required");
            bool ToTime = CheckValid(txtToTime, "To Time Required");
            string strMinuteDiff = string.Empty;
            if (FromTime == true)
            {
                if (ToTime == true)
                {
                    strMinuteDiff = (GetMinuteDiff(txtToTime.Text, txtFromTime.Text)).ToString();
                    txtTimeDuration.Text = (float.Parse(strMinuteDiff) / float.Parse("60")).ToString();
                    txtTimeDuration_OnTextChanged(sender, e);
                }
                else
                {
                    if (!CheckValid(txtToTime, "To Time Required"))
                    {
                        txtTimeDuration.Text = "";
                        txtToTime.Focus();
                        return;
                    }
                }
            }
            else if (ToTime == true)
            {
                if (FromTime == true)
                {
                    strMinuteDiff = (GetMinuteDiff(txtToTime.Text, txtFromTime.Text)).ToString();
                    txtTimeDuration.Text = (float.Parse(strMinuteDiff) / float.Parse("60")).ToString();
                    txtTimeDuration_OnTextChanged(sender, e);
                }
                else
                {
                    if (!CheckValid(txtFromTime, "From Time Required"))
                    {
                        txtTimeDuration.Text = "";
                        txtFromTime.Focus();
                        return;
                    }
                }
            }
        }
        else
        {
            int MaxOt = 0;
            int MinOt = 0;
            string minutes = (float.Parse(txtTimeDuration.Text) * float.Parse("60")).ToString();
            MaxOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            MinOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

            if (MinOt > int.Parse(minutes))
            {
                DisplayMessage("Your Time Duration is not according to Minimum Overtime");
                txtTimeDuration.Focus();
                return;
            }

            if (MaxOt < int.Parse(minutes))
            {
                DisplayMessage("Your Time Duration is Exceed to Maximum Overtime");
                txtTimeDuration.Focus();
                return;
            }

            txtDescription.Focus();
        }
    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {

        if (greatertime == "__:__" || greatertime == "")
        {
            return 0;
        }
        if (lesstime == "__:__" || lesstime == "")
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
    public bool CheckValid(TextBox txt, string Error_messagevalue)
    {

        if (txt.Text == "")
        {

            DisplayMessage(Error_messagevalue);
            txt.Focus();
            return false;
        }
        else if (txt.Text == "__:__")
        {
            DisplayMessage(Error_messagevalue);
            txt.Focus();
            return false;
        }
        else if (txt.Text == "00:00")
        {
            DisplayMessage(Error_messagevalue);
            txt.Focus();
            return false;
        }
        else
        {
            return true;
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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Session["dtLeaveStatus"] = "";
        txtOverTimeDate.Text = "";
        txtEmpName.Text = "";
        txtDescription.Text = "";
        txtEmpName.Focus();
        txtFromTime.Text = "";
        txtToTime.Text = "";
        txtTimeDuration.Text = "";
        hdnTransid.Value = "";
        txtEmpName.Enabled = true;
        gvOverTimeSumary_Approved.DataSource = null;
        gvOverTimeSumary_Approved.DataBind();
        gvOverTimeSumary_Pending.DataSource = null;
        gvOverTimeSumary_Pending.DataBind();
        Session["DtEmpLeave_Approved"] = null;
        Session["DtEmpLeave_Pending"] = null;
        txtEmpName.Enabled = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnApply_Click(object sender, EventArgs e)
    {
        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("OverTime").Rows[0]["Approval_Level"].ToString();
        if (txtEmpName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
        if (txtOverTimeDate.Text == "")
        {
            DisplayMessage("Enter Overtime Date");
            txtOverTimeDate.Focus();
            return;
        }
        else
        {
            try
            {
                objSys.getDateForInput(txtOverTimeDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct From Date Format " + objSys.SetDateFormat() + "");
                txtOverTimeDate.Focus();
                return;
            }
        }

        if (txtTimeDuration.Text == "")
        {
            DisplayMessage("You Need to Fill or Get Time Duration");
            txtEmpName.Focus();
            return;
        }
        else
        {
            int MaxOt = 0;
            int MinOt = 0;
            string minutes = (float.Parse(txtTimeDuration.Text) * float.Parse("60")).ToString();
            MaxOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            MinOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

            if (MinOt > int.Parse(minutes))
            {
                DisplayMessage("Your Time Duration is not according to Minimum Overtime");
                txtTimeDuration.Focus();
                return;
            }

            if (MaxOt < int.Parse(minutes))
            {
                DisplayMessage("Your Time Duration is Exceed to Maximum Overtime");
                txtTimeDuration.Focus();
                return;
            }
        }
        //if (objSys.getDateForInput(txtFromDate.Text) > objSys.getDateForInput(txtToDate.Text))
        //{
        //    DisplayMessage("From Date cannot be greater than To Date");
        //    //txtFromDate.Text = "";
        //    //txtToDate.Text = "";
        //    txtFromDate.Focus();
        //    //rbtnMonthly.Checked = false;
        //    //rbtnYearly.Checked = false;
        //    return;
        //}


        //this code for check posted month before leave request
        //code start
        int Month = objSys.getDateForInput(txtOverTimeDate.Text.ToString()).Month; ;
        int Year = Convert.ToDateTime(txtOverTimeDate.Text).Year;
        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(hdnEmpId.Value, Month.ToString(), Year.ToString());
        if (dtPostedList.Rows.Count > 0)
        {
            DisplayMessage("Log Posted According to Overtime Date");
            return;
        }
        //code end

        //For Checking Already Applied Overtime
        DataTable dtAlready = objOvertimeReq.GetOvertimeRequestByEmpAndOverTimedate(Session["CompId"].ToString(), hdnEmpId.Value, txtOverTimeDate.Text);
        dtAlready = new DataView(dtAlready, "Is_Canceled='False'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtAlready.Rows.Count > 0)
        {
            DisplayMessage("You Already Applied On Overtime Date");
            return;
        }
        //End




        DataTable dt1 = new DataTable();
        // Get dt on bases of Permission 
        dt1 = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),"273", hdnEmpId.Value.ToString());

        if (dt1.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }





        //this code is created by jitendra upadhyay on 28-03-2015
        //here we delete record of selected employee by transid in leave request and leaverequestchild table and also from approval table and also delete from approval trasnaction

        //code start

        if (hdnTransid.Value != "")
        {
            //this code for deleterecord from leave request and child table also
            try
            {
                objOvertimeReq.DeleteOvertimeRequest(hdnTransid.Value);
            }
            catch
            {
            }
            //this code for delete record from approval transaction table
            try
            {
                objApproalEmp.Delete_Approval_Transaction("13", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnTransid.Value);
            }
            catch
            {
            }
        }

        //code end



        int b = 0;
        b = objOvertimeReq.InsertOverTimeRequest(Session["CompId"].ToString(), hdnEmpId.Value, txtOverTimeDate.Text, txtFromTime.Text, txtToTime.Text, txtTimeDuration.Text, txtDescription.Text, true.ToString(), false.ToString(), false.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (dt1.Rows.Count > 0)
        {
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                string IsPriority = dt1.Rows[j]["Priority"].ToString();
                if (EmpPermission == "1")
                {
                    objApproalEmp.InsertApprovalTransaciton("13", Session["CompId"].ToString(), "0", "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
                else if (EmpPermission == "2")
                {
                    objApproalEmp.InsertApprovalTransaciton("13", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
                else if (EmpPermission == "3")
                {
                    objApproalEmp.InsertApprovalTransaciton("13", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
                else
                {
                    objApproalEmp.InsertApprovalTransaciton("13", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                }
            }
        }

        DisplayMessage("Record Updated Successfully", "green");
        //PrintReport(b.ToString(), hdnEmpId.Value);
        txtEmpName.Text = "";
        txtOverTimeDate.Text = "";
        txtFromTime.Text = "";
        txtToTime.Text = "";
        txtTimeDuration.Text = "";
        txtDescription.Text = "";
        btnReset_Click(null, null);
        btnReset.Focus();
        FillLeaveStatus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void PrintReport(string LeaveId, string EmpId)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Vacation Leave Application.aspx?TransId=" + LeaveId + "&&EmpId=" + EmpId.ToString() + " ','window','width=1024');", true);
    }
    private void ClearFormFields()
    {
        txtEmpName.Text = "";
        txtOverTimeDate.Text = "";
        txtFromTime.Text = "";
        txtToTime.Text = "";
        txtTimeDuration.Text = "";
        txtDescription.Text = "";

        txtEmpName.Focus();
        hdnTransid.Value = "";
        hdnEmpId.Value = "0";
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
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objOvertimeReq.GetOvertimeRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {
            txtEmpName.Enabled = false;
            hdnTransid.Value = e.CommandArgument.ToString();
            DataTable dtemployee = Common.GetEmployee(dt.Rows[0]["Emp_Name"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
            if (dtemployee.Rows.Count > 0)
            {
                txtEmpName.Text = "" + dtemployee.Rows[0][1].ToString() + "/(" + dtemployee.Rows[0][2].ToString() + ")/" + dtemployee.Rows[0][0].ToString() + "";
            }

            hdnEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();
            FillApproveLeave(hdnEmpId.Value);
            FillPendingLeave(hdnEmpId.Value);
            txtOverTimeDate.Text = Convert.ToDateTime(dt.Rows[0]["Overtime_Date"].ToString()).ToString(objSys.SetDateFormat());
            txtFromTime.Text = dt.Rows[0]["From_Time"].ToString();
            txtToTime.Text = dt.Rows[0]["To_Time"].ToString();
            txtTimeDuration.Text = dt.Rows[0]["Time_Duration"].ToString();
            txtDescription.Text = dt.Rows[0]["Description"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
        }
    }
    public void FillPendingLeave(string Empid)
    {
        DataTable dtLeave = objOvertimeReq.GetOvertimeRequestByCompany(Session["CompId"].ToString());
        dtLeave = new DataView(dtLeave, "Is_Pending='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id=" + Empid + "", "", DataViewRowState.CurrentRows).ToTable();

        if (dtLeave.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvOverTimeSumary_Pending, dtLeave, "", "");
            Session["DtEmpLeave_Pending"] = dtLeave;
        }
        else
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvOverTimeSumary_Pending, dtLeave, "", "");
            Session["DtEmpLeave_Pending"] = null;
        }
    }
    protected void gvOverTimeSumary_Pending_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOverTimeSumary_Pending.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["DtEmpLeave_Pending"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvOverTimeSumary_Pending, dt, "", "");
    }

    public void FillApproveLeave(string Empid)
    {
        DataTable dtLeave = objOvertimeReq.GetOvertimeRequestByCompany(Session["CompId"].ToString());
        dtLeave = new DataView(dtLeave, "Is_Approved='True' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id=" + Empid + "", "", DataViewRowState.CurrentRows).ToTable();

        if (dtLeave.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvOverTimeSumary_Approved, dtLeave, "", "");
            Session["DtEmpLeave_Approved"] = dtLeave;
        }
        else
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvOverTimeSumary_Approved, dtLeave, "", "");
            Session["DtEmpLeave_Approved"] = null;
        }
    }
    protected void gvOverTimeSumary_Approved_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOverTimeSumary_Approved.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtEmpLeave_Approved"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvOverTimeSumary_Approved, dt, "", "");
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        PrintReport(e.CommandArgument.ToString(), e.CommandName.ToString());
    }
}