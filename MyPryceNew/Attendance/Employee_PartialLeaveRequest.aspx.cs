using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

public partial class Attendance_Employee_PartialLeaveRequest : System.Web.UI.Page
{
    CompanyMaster objComp = null;
    Att_AttendanceLog objAttLog = null;
    Att_ScheduleMaster objEmpSch = null;
    EmployeeMaster objEmp = null;
    Set_Approval_Employee objApproalEmp = null;
    Att_PartialLeave_Request objPartial = null;
    EmployeeParameter objEmpParam = null;
    Set_ApplicationParameter objAppParam = null;
    Common cmn = null;
    Set_Employee_Holiday objEmpHoliday = null;
    SystemParameter objSys = null;
    Att_HalfDay_Request objHalfDay = null;
    Att_Leave_Request objleaveReq = null;
    Attendance objAttendance = null;
    string strPLWithTimeWithOutTime = string.Empty;
    SendMailSms ObjSendMailSms = null;
    Set_ApprovalMaster ObjApproval = null;
    NotificationMaster Obj_Notifiacation = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        

        btnApply.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnApply, "").ToString());

        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "254", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            pnlList.Visible = true;
            pnlNew.Visible = false;
            FillLeaveStatusApproved();
            FillLeaveStatusPending();
            btnList_Click(null, null);
            FillGvEmp();
            rbtnPersonal.Checked = true;
        }

        //Add On 30-07-2015       
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }

        if (strPLWithTimeWithOutTime == "True")
        {
            TrWithTime1.Visible = true;
            TrWithOutTime1.Visible = false;
            TrWithOutTime2.Visible = false;
        }
        else if (strPLWithTimeWithOutTime == "False")
        {
            TrWithTime1.Visible = false;
            TrWithOutTime1.Visible = true;
            TrWithOutTime2.Visible = true;
        }

        CalendarExtender2.Format = objSys.SetDateFormat();
        AllPageCode();
        if (Settings.IsValid == false)
        {
            btnApply.Visible = false;
        }
    }
    

    // Fill Emp List
    private void FillGvEmp()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        Session["EmpDt"] = dtEmp;
        //lblSelectRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        //GvEmpListSelected.DataSource = null;
        //GvEmpListSelected.DataBind();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("254", (DataTable)Session["ModuleName"]);
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
            btnApply.Visible = true;
            try
            {
                GvPending.Columns[0].Visible = true;
                GvPending.Columns[1].Visible = true;
            }
            catch
            {

            }
        }
        else
        {

            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "254", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {
                    foreach (GridViewRow Row in gvLeaveStatus.Rows)
                    {
                        //((ImageButton)Row.FindControl("IbtnReject")).Visible = true;
                        //((ImageButton)Row.FindControl("IbtnApprove")).Visible = true;
                        //((ImageButton)Row.FindControl("IbtnEdit")).Visible = true;
                    }
                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnApply.Visible = true;
                        }

                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            try
                            {
                                GvPending.Columns[1].Visible = true;
                            }
                            catch
                            {
                            }
                        }
                        if (DtRow["Op_Id"].ToString() == "6")
                        {
                            try
                            {
                                GvPending.Columns[0].Visible = true;
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
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlList.Visible = true;
        pnlNew.Visible = false;
        AllPageCode();
        FillGvEmp();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //Reset();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlList.Visible = false;
        pnlNew.Visible = true;
        AllPageCode();
        txtApplyDate.Focus();
        FillStatus();
    }

    private void FillStatus()
    {
        try
        {
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(Session["EmpId"].ToString(), Session["CompId"].ToString());
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

                        DateTime StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                        int TotalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, TotalDays, 23, 59, 1);
                        DataTable DtFilter = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
                        DataTable DtApproved = new DataTable();
                        try
                        {
                            DtApproved = new DataView(DtFilter, "Partial_Leave_Date>='" + StartDate.ToString() + "' and Partial_Leave_Date<='" + EndDate.ToString() + "' and Emp_Id=" + Session["EmpId"].ToString() + " and Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
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
                            DataTable DtPending = new DataView(DtFilter, "Partial_Leave_Date>='" + StartDate.ToString() + "' and Partial_Leave_Date<='" + EndDate.ToString() + "' and Emp_Id=" + Session["EmpId"].ToString() + " and Is_Confirmed='Pending' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
                            if (DtPending.Rows.Count == 0)
                            {
                                dr["Pending_Days"] = "0";
                            }
                            else
                            {
                                dr["Pending_Days"] = DtPending.Rows.Count.ToString();
                            }
                            //dtPartial_Leave_summary.Rows.Add(dr);
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
                                //Common Function add By Lokesh on 22-05-2015
                                objPageCmn.FillData((object)gvLeaveSummary_PartialLeave, dtPartial_Leave_summary, "", "");
                                gvLeaveSummary_PartialLeave.Visible = true;
                                //lblTypeLeaveStatus_PartialLeave.Visible = true;
                                //lblcolonLeaveStatus_PartialLeave.Visible = true;
                                //lblNameLeaveStatus_PartialLeave.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    private void Reset()
    {
        txtApplyDate.Text = string.Empty;
        rbBegining.Checked = false;
        rbMiddle.Checked = false;
        rbEnding.Checked = false;

        rbTimeTable.ClearSelection();
        rbTimeTable.DataSource = null;
        rbTimeTable.DataBind();
        rbTimeTable.Items.Clear();
        txtDescription.Text = string.Empty;
    }
    private string GetTime24(string timepart)
    {
        string str = "00:00";
        DateTime date = Convert.ToDateTime(timepart);
        str = date.ToString("HH:mm");
        return str;
    }

    //BUTTON APPLY 
    protected void btnApply_Click(object sender, EventArgs e)
    {

        string strTransId = string.Empty;
        //Add On 30-07-2015
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }

        //Add On 07-07-2016
        string strPLDateEditable = string.Empty;
        DataTable dtPLDateEditable = objAppParam.GetApplicationParameterByCompanyId("PL Date Editable", Session["CompId"].ToString());
        dtPLDateEditable = new DataView(dtPLDateEditable, "Param_Name='PL Date Editable' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLDateEditable.Rows.Count > 0)
        {
            strPLDateEditable = dtPLDateEditable.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLDateEditable = "False";
        }

        //For Mail Credentials
        string MailMessage = string.Empty;
        string strAppMailId = string.Empty;
        string strAppPassword = string.Empty;
        DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (dtFrom.Rows.Count > 0)
        {
            strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
        }

        DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (dtFrom.Rows.Count > 0)
        {


            strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
        }

        if (rbtnOfficial.Checked == false && rbtnPersonal.Checked == false)
        {
            DisplayMessage("Please select Personal or Official");
            rbtnPersonal.Focus();
            return;
        }

        string strPLType = string.Empty;
        if (txtApplyDate.Text == "")
        {
            DisplayMessage("Enter Apply Date");
            txtApplyDate.Focus();
            return;
        }
        else
        {
            try
            {
                objSys.getDateForInput(txtApplyDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct Apply Date Format dd-MMM-yyyy");
                txtApplyDate.Focus();
                return;
            }
        }


        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtApplyDate.Text), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        if (hdnEdit.Value == "")
        {
            if (strPLDateEditable == "True")
            { }
            else if (strPLDateEditable == "False")
            {
                if (DateTime.Parse(txtApplyDate.Text) == DateTime.Now.Date)
                {

                }
                else
                {
                    DisplayMessage("You Can Enter Partial Leave for Today Only");
                    txtApplyDate.Focus();
                    return;
                }
            }
        }
        else
        {
            if (strPLDateEditable == "True")
            { }
            else if (strPLDateEditable == "False")
            {
                if (hdnEditDate.Value != "0" && hdnEditDate.Value != "")
                {
                    if (DateTime.Parse(txtApplyDate.Text) == DateTime.Parse(hdnEditDate.Value))
                    {

                    }
                    else
                    {
                        DisplayMessage("You Cant Change you Apply Date");
                        txtApplyDate.Focus();
                        return;
                    }
                }
            }
        }


        if (rbtnOfficial.Checked == false && rbtnPersonal.Checked == true)
        {
            if (strPLWithTimeWithOutTime == "False")
            {
                if (rbBegining.Checked == true || rbMiddle.Checked == true || rbEnding.Checked == true)
                {
                    if (rbBegining.Checked == true)
                    {
                        strPLType = "B";
                    }
                    else if (rbMiddle.Checked == true)
                    {
                        strPLType = "M";
                    }
                    else if (rbEnding.Checked == true)
                    {
                        strPLType = "E";
                    }
                }
                else
                {
                    DisplayMessage("Select Partial Leave Type");
                    return;
                }

                if (rbTimeTable.SelectedValue == "0" || rbTimeTable.SelectedValue == "")
                {
                    DisplayMessage("Choose Time Table");
                    rbTimeTable.Focus();
                    return;
                }
            }
            else if (strPLWithTimeWithOutTime == "True")
            {
                if (txtInTime.Text == "")
                {
                    DisplayMessage("Enter In Time");
                    txtInTime.Focus();
                    return;
                }
                if (txtOuttime.Text == "")
                {
                    DisplayMessage("Enter Out Time");
                    txtOuttime.Focus();
                    return;
                }
            }
        }
        else if (rbtnOfficial.Checked == true && rbtnPersonal.Checked == false)
        {
            if (txtInTime.Text == "")
            {
                DisplayMessage("Enter In Time");
                txtInTime.Focus();
                return;
            }
            if (txtOuttime.Text == "")
            {
                DisplayMessage("Enter Out Time");
                txtOuttime.Focus();
                return;
            }
        }


        //here we are checking that in time and out  time should not same 




        if (Convert.ToDateTime(txtInTime.Text) == Convert.ToDateTime(txtOuttime.Text))
        {
            DisplayMessage("From time and To time should not be same");
            txtInTime.Focus();
            return;

        }



        //here we are checking that always to time should be greater 



        if (Convert.ToDateTime(txtInTime.Text) > Convert.ToDateTime(txtOuttime.Text))
        {
            DisplayMessage("From time should be less then To Time");
            txtInTime.Focus();
            return;


        }





        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }

        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("PartialLeave").Rows[0]["Approval_Level"].ToString();

        DataTable dtPartialLeave = new DataTable();



        dtPartialLeave = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "254", Session["EmpId"].ToString());

        // dtPartialLeave = new DataView(dtPartialLeave, "Approval_Id='3'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPartialLeave.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }

        string OnDutyTime = string.Empty;
        string OffDutyTime = string.Empty;
        int Count = 0;
        int b = 0;
        if (Session["EmpId"].ToString() == "0")
        {
            DisplayMessage("Superadmin Can Not Request For Partial Leave");
            return;
        }

        //this code is created by jitendra upadhyay on 10-09-2014
        //this code for check posted month before leave request
        //code start
        int Monthposted = objSys.getDateForInput(txtApplyDate.Text.ToString()).Month; ;
        //    int Month = Convert.ToDateTime(txtFrom.Text).ToString();
        int Yearposted = Convert.ToDateTime(txtApplyDate.Text).Year;
        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(Session["EmpId"].ToString(), Monthposted.ToString(), Yearposted.ToString());

        if (dtPostedList.Rows.Count > 0)
        {
            DisplayMessage("Log Posted For This Date Criteria");
            return;
        }

        //code end

        //if (txtInTime.Text == "")
        //{
        //    DisplayMessage("Enter In Time");
        //    txtInTime.Focus();

        //    return;
        //}
        //if (txtOuttime.Text == "")
        //{
        //    DisplayMessage("Enter Out Time");
        //    txtOuttime.Focus();

        //    return;
        //}
        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }
        // Check Holiday or Not For Leave Apply For the Day...............
        DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        DateTime fromdate2 = objSys.getDateForInput(txtApplyDate.Text.ToString());


        string Holiday = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), Session["EmpId"].ToString(), fromdate2, fromdate2, "Holiday", 1, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

        if (Holiday != string.Empty)
        {
            DisplayMessage(Holiday.ToString());
            ClearFormFields();
            return;
        }

        // Holiday Code over ........................................

        //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
        DateTime fromdateW = Convert.ToDateTime(txtApplyDate.Text.ToString());

        string WeekOff = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), Session["EmpId"].ToString(), fromdateW, fromdateW, "WeekOff", 0, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

        if (WeekOff != string.Empty)
        {
            DisplayMessage(WeekOff.ToString());
            ClearFormFields();
            return;
        }

        // Week Off Code Over .........................................................................
        // Check For Half Day Leave Is Apply For the Day Or Not
        DateTime HalfDayInDate = objSys.getDateForInput(txtApplyDate.Text.ToString());
        string HalfDay = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), Session["EmpId"].ToString(), HalfDayInDate, HalfDayInDate, "HalfDay", 0, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

        if (HalfDay != string.Empty)
        {
            DisplayMessage(HalfDay.ToString());
            ClearFormFields();
            return;
        }
        // Half Day Leave Check Over .................................................

        // Nitin Jain Is Already Approved,Pending  Full Day Leave on Date FOr Employee 
        DateTime fromdate1 = objSys.getDateForInput(txtApplyDate.Text);
        string FullDay = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), Session["EmpId"].ToString(), fromdate1, fromdate1, "FullDay", 0, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

        if (FullDay != string.Empty)
        {
            DisplayMessage(FullDay.ToString());
            ClearFormFields();
            return;
        }

        DataTable dtEmployeeApprovalLeave = new DataTable();
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("PartialLeave").Rows[0]["Approval_Level"].ToString();


        dtEmployeeApprovalLeave = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "254", Session["EmpId"].ToString());

        if (dtEmployeeApprovalLeave.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }


        // Check For Partial Leave Apply Between Days.................................

        DateTime DatePartial = Convert.ToDateTime(txtApplyDate.Text);
        string PartialDay = objAttendance.GetLeaveApprovalStatus(Session["CompId"].ToString(), Session["EmpId"].ToString(), DatePartial, DatePartial, "PartialLeave", 0, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        if (PartialDay != string.Empty)
        {
            DisplayMessage(PartialDay.ToString());
            ClearFormFields();
            return;
        }
        //Partial Leave Check Code Over ...........................................



        if (rbtnPersonal.Checked)
        {

            bool IsCompPartial = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

            if (IsCompPartial)
            {
                DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(Session["EmpId"].ToString(), Session["CompId"].ToString());
                if (dtEmpParam.Rows.Count > 0)
                {
                    bool IsEmpPartial = Convert.ToBoolean(dtEmpParam.Rows[0]["Is_Partial_Enable"].ToString());
                    if (IsEmpPartial)
                    {
                        int totalminutes = 0;
                        int useinday = 0;
                        double leaveCount = 0;

                        totalminutes = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString());
                        useinday = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());

                        leaveCount = double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString()) / double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());

                        leaveCount = System.Math.Round(leaveCount);

                        leaveCount = leaveCount - getCurrentMonthLeaveCount(objSys.getDateForInput(txtApplyDate.Text));

                        if (leaveCount > 0)
                        {
                            if (totalminutes > 0)
                            {
                                if (strPLWithTimeWithOutTime == "True")
                                {
                                    //Add  On 30-07-2015
                                    int CurrentUseMin = getCurrentMonth(objSys.getDateForInput(txtApplyDate.Text));
                                    if (CurrentUseMin > 0)
                                    {
                                        totalminutes = totalminutes - CurrentUseMin;
                                    }

                                    int OneDayMin = getMinuteInADay(objSys.getDateForInput(txtApplyDate.Text));
                                    if (OneDayMin >= useinday)
                                    {
                                        DisplayMessage("You cannot request more than " + useinday.ToString() + " minutes in a day");
                                        return;
                                    }

                                    int RequestMin = GetMinuteDiff(txtOuttime.Text, txtInTime.Text);
                                    if (RequestMin <= totalminutes)
                                    {
                                        if (RequestMin > useinday)
                                        {
                                            DisplayMessage("You cannot request more than " + useinday.ToString() + " minutes in a day");
                                            return;
                                        }
                                        else
                                        {
                                            int remainmin = totalminutes - RequestMin;
                                            if (hdnEdit.Value == "")
                                            {
                                                b = objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), Session["EmpId"].ToString(), "0", DateTime.Now.ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(),"","");

                                                if (b != 0)
                                                {
                                                    strTransId = b.ToString();
                                                    // objApproalEmp.InsertApprovalChildMaster("PartialLeave", b.ToString(),"236", Session["EmpId"].ToString(), objSys.getDateForInput(txtApplyDate.Text.ToString()).ToString());

                                                    if (dtEmployeeApprovalLeave.Rows.Count > 0)
                                                    {
                                                        for (int j = 0; j < dtEmployeeApprovalLeave.Rows.Count; j++)
                                                        {
                                                            int cur_trans_id = 0;
                                                            string PriorityEmpId = dtEmployeeApprovalLeave.Rows[j]["Emp_Id"].ToString();
                                                            string IsPriority = dtEmployeeApprovalLeave.Rows[j]["Priority"].ToString();
                                                            if (EmpPermission == "1")
                                                            {
                                                                cur_trans_id=objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());


                                                            }
                                                            else if (EmpPermission == "2")
                                                            {
                                                                cur_trans_id=objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                                            }
                                                            else if (EmpPermission == "3")
                                                            {
                                                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                                            }
                                                            else
                                                            {
                                                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                                            }

                                                            // Insert Notification For Leave by  ghanshyam suthar
                                                            hdnEmpId.Value = Session["EmpId"].ToString();
                                                            Session["PriorityEmpId"] = PriorityEmpId;
                                                            Session["cur_trans_id"] = cur_trans_id;
                                                            Set_Notification();

                                                            //for Email Code
                                                            if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtEmployeeApprovalLeave.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                                                            {
                                                                if (PriorityEmpId != "" && PriorityEmpId != "0")
                                                                {
                                                                    DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), PriorityEmpId);
                                                                    if (dtEmpDetail.Rows.Count > 0)
                                                                    {
                                                                        string strPendingApproval = string.Empty;
                                                                        if (ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtEmployeeApprovalLeave.Rows.Count > 1)
                                                                        {
                                                                            strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(dtEmployeeApprovalLeave.Rows[1]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                                                                        }

                                                                        MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + txtApplyDate.Text + "<br />From Time :" + txtInTime.Text + "  To Time :" + txtOuttime.Text + "<br /> Reason For Leave : " + txtDescription.Text + " " + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";

                                                                        //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>'" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "'Apply for Partial Leave 'Personal'<br />On Date '" + txtApplyDate.Text + "' Timing is From '" + txtInTime.Text + "' To '" + txtOuttime.Text + "' <br /> and Given Description is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                                                        try
                                                                        {
                                                                            ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Personal Partial Leave Apply By'" + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On Date '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "","", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                                                        }
                                                                        catch
                                                                        {


                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    DisplayMessage("Partial leave submitted");
                                                    btnReset_Click(null, null);
                                                    FillLeaveStatusApproved();
                                                    FillLeaveStatusPending();
                                                    FillStatus();

                                                }
                                            }
                                            else
                                            {
                                                b = objPartial.UpdatePartialLeaveRequest(hdnEdit.Value, Session["CompId"].ToString(), Session["EmpId"].ToString(), "0", DateTime.Now.ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                if (b != 0)
                                                {
                                                    if (dtPartialLeave.Rows.Count > 0)
                                                    {

                                                    }
                                                    hdnEmpId.Value = Session["EmpId"].ToString();
                                                    Session["PriorityEmpId"] = hdnEmpId.Value;
                                                    Session["cur_trans_id"] = hdnEdit.Value;
                                                    Set_Notification();
                                                    DisplayMessage("Partial leave updated");
                                                    btnCancel_Click(null, null);
                                                    FillLeaveStatusApproved();
                                                    FillLeaveStatusPending();
                                                    FillStatus();
                                                    hdnEdit.Value = "";
                                                    hdnEditDate.Value = "0";
                                                    Lbl_Tab_New.Text = Resources.Attendance.New;
                                                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DisplayMessage("Employee does not have sufficient balance");
                                        return;
                                    }
                                    //End
                                }
                                else if (strPLWithTimeWithOutTime == "False")
                                {
                                    //Add On 30-07-2015
                                    int CurrentUseMin = getCurrentMonth(objSys.getDateForInput(txtApplyDate.Text));
                                    if (CurrentUseMin > 0)
                                    {
                                        totalminutes = totalminutes - CurrentUseMin;

                                    }

                                    int OneDayMin = getMinuteInADay(objSys.getDateForInput(txtApplyDate.Text));
                                    if (OneDayMin >= useinday)
                                    {
                                        DisplayMessage("You cannot request more than " + useinday.ToString() + " minutes in a day");
                                        return;
                                    }

                                    //int RequestMin = GetMinuteDiff(txtOuttime.Text, txtInTime.Text);
                                    //if (RequestMin <= totalminutes)
                                    //{
                                    //if (RequestMin > useinday)
                                    //{
                                    //    DisplayMessage("You cannot request more than " + useinday.ToString() + " minutes in a day");
                                    //    return;
                                    //}
                                    //else
                                    //{
                                    //int remainmin = totalminutes - RequestMin;
                                    if (hdnEdit.Value == "")
                                    {
                                        b = objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), Session["EmpId"].ToString(), "0", DateTime.Now.ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), "", "", "Pending", txtDescription.Text, "", strPLType, rbTimeTable.SelectedValue, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "");

                                        if (b != 0)
                                        {
                                            strTransId = b.ToString();
                                            // objApproalEmp.InsertApprovalChildMaster("PartialLeave", b.ToString(),"236", Session["EmpId"].ToString(), objSys.getDateForInput(txtApplyDate.Text.ToString()).ToString());

                                            if (dtEmployeeApprovalLeave.Rows.Count > 0)
                                            {

                                                for (int j = 0; j < dtEmployeeApprovalLeave.Rows.Count; j++)
                                                {
                                                    int cur_trans_id = 0;
                                                    string PriorityEmpId = dtEmployeeApprovalLeave.Rows[j]["Emp_Id"].ToString();
                                                    string IsPriority = dtEmployeeApprovalLeave.Rows[j]["Priority"].ToString();
                                                    if (EmpPermission == "1")
                                                    {
                                                        cur_trans_id= objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());


                                                    }
                                                    else if (EmpPermission == "2")
                                                    {
                                                        cur_trans_id=objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                                    }
                                                    else if (EmpPermission == "3")
                                                    {
                                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                                    }
                                                    else
                                                    {
                                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                                    }

                                                    // Insert Notification For Leave by  ghanshyam suthar
                                                    hdnEmpId.Value = Session["EmpId"].ToString();
                                                    Session["PriorityEmpId"] = PriorityEmpId;
                                                    Session["cur_trans_id"] = cur_trans_id;
                                                    Set_Notification();
                                                    //--------------------------------------

                                                    //for Email Code
                                                    if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtEmployeeApprovalLeave.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                                                    {
                                                        if (PriorityEmpId != "" && PriorityEmpId != "0")
                                                        {
                                                            DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), PriorityEmpId);
                                                            if (dtEmpDetail.Rows.Count > 0)
                                                            {
                                                                string strPendingApproval = string.Empty;
                                                                if (ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtEmployeeApprovalLeave.Rows.Count > 1)
                                                                {
                                                                    strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(dtEmployeeApprovalLeave.Rows[1]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                                                                }

                                                                MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + txtApplyDate.Text + "<br />From Time :" + txtInTime.Text + "  To Time :" + txtOuttime.Text + "<br /> Reason For Leave : " + txtDescription.Text + "  " + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";

                                                                // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>'" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "'Apply for Partial Leave 'Personal'<br />On Date '" + txtApplyDate.Text + "' Timing is From '" + txtInTime.Text + "' To '" + txtOuttime.Text + "' <br /> and Given Description is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                                                try
                                                                {
                                                                    ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Personal Partial Leave Apply By'" + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On Date '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "","",HttpContext.Current.Session["BrandId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
                                                                }
                                                                catch
                                                                {

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            DisplayMessage("Partial leave submitted");
                                            btnReset_Click(null, null);
                                            FillLeaveStatusApproved();
                                            FillLeaveStatusPending();
                                            FillStatus();
                                        }
                                    }
                                    else
                                    {
                                        b = objPartial.UpdatePartialLeaveRequest(hdnEdit.Value, Session["CompId"].ToString(), Session["EmpId"].ToString(), "1", DateTime.Now.ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), "", "", "Pending", txtDescription.Text, "", strPLType, rbTimeTable.SelectedValue, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                                        if (b != 0)
                                        {
                                            if (dtPartialLeave.Rows.Count > 0)
                                            {
                                            }
                                            hdnEmpId.Value = Session["EmpId"].ToString();
                                            Session["PriorityEmpId"] = hdnEmpId.Value;
                                            Session["cur_trans_id"] = hdnEdit.Value;
                                            Set_Notification();

                                            DisplayMessage("Partial leave updated");
                                            btnCancel_Click(null, null);
                                            FillLeaveStatusApproved();
                                            FillLeaveStatusPending();
                                            FillStatus();
                                            hdnEdit.Value = "";
                                            hdnEditDate.Value = "0";
                                            Lbl_Tab_New.Text = Resources.Attendance.New;

                                        }
                                    }
                                    //}
                                    //}
                                    //else
                                    //{
                                    //    DisplayMessage("Employee does not have sufficient balance");
                                    //    return;

                                    //}
                                    //End
                                }
                            }
                            else
                            {
                                DisplayMessage("Employee does not have sufficient balance");
                                return;
                            }

                        }
                        else
                        {
                            DisplayMessage("Employee does not have sufficient balance");
                            return;

                        }
                    }
                    else
                    {
                        DisplayMessage("Partial leave cannot assign to this employee");
                        return;
                    }

                }
                else
                {

                    DisplayMessage("Partial leave cannot assign to this employee");
                    return;
                }
            }
            else
            {
                DisplayMessage("Company does not provide partial leave");
                return;
            }
        }
        else
        {
            //Strat Code For Official Partial Leave
            string PostedEmpList = string.Empty;
            string NonPostedLog = string.Empty;
            string empidlist = string.Empty;

            empidlist += Session["EmpId"].ToString() + ",";

            for (int i = 0; i < empidlist.Split(',').Length; i++)
            {

                if (empidlist.Split(',')[i] == "")
                {
                    continue;
                }

                //this code is created by jitendra upadhyay on 10-09-2014
                //this code for check posted month before leave request
                //code start
                Monthposted = objSys.getDateForInput(txtApplyDate.Text.ToString()).Month; ;
                //    int Month = Convert.ToDateTime(txtFrom.Text).ToString();
                Yearposted = Convert.ToDateTime(txtApplyDate.Text).Year;
                dtPostedList = objAttLog.Get_Pay_Employee_Attendance(empidlist.Split(',')[i].ToString(), Monthposted.ToString(), Yearposted.ToString());

                if (dtPostedList.Rows.Count > 0)
                {
                    PostedEmpList += GetEmployeeCode(dtPostedList.Rows[0]["Emp_Id"].ToString()) + ",";

                }
                else
                {
                    NonPostedLog += GetEmployeeCode(empidlist.Split(',')[i].ToString()) + ",";
                }

                // Check Holiday or Not For Leave Apply For the Day...............
                dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                fromdate2 = objSys.getDateForInput(txtApplyDate.Text.ToString());
                if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())) == false)
                {
                    DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + fromdate2.ToString() + "' and Emp_Id='" + empidlist.Split(',')[i] + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtHoliday1.Rows.Count > 0)
                    {

                        DisplayMessage("Employee has Holiday on Date " + fromdate2.ToString("dd-MMM-yyyy") + " so cannot apply");
                        txtDescription.Text = string.Empty;
                        return;
                    }
                }
                // Holiday Code Over..............

                //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
                if (empidlist.Split(',')[i].ToString() != "")
                {

                    if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())) == false)
                    {
                        DataTable dtSch = objEmpSch.GetSheduleDescription(empidlist.Split(',')[i].ToString());
                        // Modified By Nitin jain, Date 09-07-2014 Check If Week Off Or Not For Leave Apply Date....
                        DataTable dtSch1 = new DataView(dtSch, "Att_Date='" + txtApplyDate.Text + "' and Emp_Id='" + empidlist.Split(',')[i].ToString() + "' and Is_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtSch1.Rows.Count > 0)
                        {

                            DisplayMessage("Employee has Week off on Date " + DateFormat(txtApplyDate.Text) + " so cannot apply");
                            //txtApplyDate.Text = string.Empty;
                            //rbtnPersonal.Checked = false;
                            //rbtnOfficial.Checked = false;
                            //txtEmpName.Text = "";
                            //txtDescription.Text = string.Empty;
                            return;
                        }
                    }
                }
                DataTable dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
                dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Partial_Leave_Date='" + Convert.ToDateTime(txtApplyDate.Text) + "' and Emp_Id=" + empidlist.Split(',')[i].ToString() + " AND Is_Confirmed<>'Canceled'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    for (int PL = 0; PL < dtLeave.Rows.Count; PL++)
                    {


                        if (strPLWithTimeWithOutTime == "True")
                        {
                            TimeSpan FT = TimeSpan.Parse(dtLeave.Rows[PL]["From_Time"].ToString());
                            TimeSpan LT = TimeSpan.Parse(dtLeave.Rows[PL]["To_Time"].ToString());
                            if ((TimeSpan.Parse(txtInTime.Text) >= FT && (TimeSpan.Parse(txtInTime.Text) <= LT)) || (TimeSpan.Parse(txtOuttime.Text) >= FT && (TimeSpan.Parse(txtOuttime.Text) <= LT)))
                            {
                                string EmpCode = GetEmployeeCode(dtLeave.Rows[PL]["Emp_Id"].ToString());
                                DisplayMessage("You Have Already Apply Partial Leave between Apply Time For Employee:-" + EmpCode.ToString() + "");
                                return;
                            }
                        }
                        else if (strPLWithTimeWithOutTime == "False")
                        {
                            ////string strPLTypeCheck = dtLeave.Rows[PL]["Field1"].ToString();

                            ////if (strPLTypeCheck == strPLType)
                            ////{
                            ////    string EmpCode = GetEmployeeCode(dtLeave.Rows[PL]["Emp_Id"].ToString());
                            ////    DisplayMessage("You Have Already Apply Partial Leave between Apply Time For Employee:-" + EmpCode.ToString() + "");
                            ////    return;
                            ////}
                        }
                    }
                }

                //code start for leave and half day
                //code for check the half day and full day leave for same date
                //code start
                HalfDayInDate = objSys.getDateForInput(txtApplyDate.Text.ToString());



                DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), empidlist.Split(',')[i].ToString());
                dtHalfDay = new DataView(dtHalfDay, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id='" + empidlist.Split(',')[i].ToString() + "' and HalfDay_Date='" + HalfDayInDate + "'", "", DataViewRowState.CurrentRows).ToTable();


                if (dtHalfDay.Rows.Count > 0)
                {
                    if (dtHalfDay.Rows[0]["Is_Confirmed"].ToString() != "Canceled")
                    {
                        DisplayMessage("Your Half Day Leave Already Apply On Date : " + DateFormat(HalfDayInDate.ToString()) + " So You May Not Apply Partial Leave");
                        txtDescription.Text = string.Empty;
                        return;
                    }
                }
                // Half Day Leave Check Over .................................................

                DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), empidlist.Split(',')[i].ToString());

                //
                //
                fromdate1 = objSys.getDateForInput(txtApplyDate.Text);

                DataTable dtLeaveReq2 = new DataView(dtLeaveR, "From_Date <='" + fromdate1.ToString() + "' and To_Date>='" + fromdate1.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeaveReq2.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtLeaveReq2.Rows[0]["Is_Canceled"].ToString()) == false)
                    {
                        DisplayMessage("You have Already Apply Leave For Date : " + DateFormat(txtApplyDate.Text) + "");
                        //txtApplyDate.Text = string.Empty;
                        //txtDescription.Text = string.Empty;
                        return;
                    }
                }
                //code end

                //code end
                //Week Off Code Over........                         
                bool IsCompPartial = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                if (IsCompPartial)
                {
                    if (hdnEdit.Value == "")
                    {
                        b = objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), empidlist.Split(',')[i], "1", DateTime.Now.ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "");

                        if (b != 0)
                        {
                            strTransId = b.ToString();
                            if (dtPartialLeave.Rows.Count > 0)
                            {
                                for (int j = 0; j < dtPartialLeave.Rows.Count; j++)
                                {
                                    string PriorityEmpId = dtPartialLeave.Rows[j]["Emp_Id"].ToString();
                                    string IsPriority = dtPartialLeave.Rows[j]["Priority"].ToString();
                                    int cur_trans_id = 0;

                                    if (EmpPermission == "1")
                                    {
                                        cur_trans_id= objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), "0", "0", "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());


                                    }
                                    else if (EmpPermission == "2")
                                    {
                                        cur_trans_id=objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                    }
                                    else if (EmpPermission == "3")
                                    {
                                        cur_trans_id=objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                    }
                                    else
                                    {
                                        cur_trans_id= objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());

                                    }

                                    // Insert Notification For Leave by  ghanshyam suthar
                                    hdnEmpId.Value = Session["EmpId"].ToString();
                                    Session["PriorityEmpId"] = PriorityEmpId;
                                    Session["cur_trans_id"] = cur_trans_id;
                                    Set_Notification();
                                    //--------------------------------------

                                    //for Email Code
                                    if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtPartialLeave.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                                    {
                                        if (PriorityEmpId != "" && PriorityEmpId != "0")
                                        {
                                            DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), PriorityEmpId);
                                            if (dtEmpDetail.Rows.Count > 0)
                                            {
                                                string strPendingApproval = string.Empty;
                                                if (ObjApproval.GetApprovalMasterById("3").Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtPartialLeave.Rows.Count > 1)
                                                {
                                                    strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(dtPartialLeave.Rows[1]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                                                }

                                                MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + txtApplyDate.Text + "<br />From Time :" + txtInTime.Text + "  To Time :" + txtOuttime.Text + "<br /> Reason For Leave : " + txtDescription.Text + "" + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";


                                                //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>'" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "'Apply for Partial Leave 'Personal'<br />On Date '" + txtApplyDate.Text + "' Timing is From '" + txtInTime.Text + "' To '" + txtOuttime.Text + "' <br /> and Given Description is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                                try
                                                {
                                                    ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Personal Partial Leave Apply By'" + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On Date '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "","", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                                }
                                                catch
                                                {

                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            hdnEmpId.Value = Session["EmpId"].ToString();
                            Session["PriorityEmpId"] = hdnEmpId.Value;
                            Session["cur_trans_id"] = hdnEdit.Value;
                            Set_Notification();
                            DisplayMessage("Partial leave Submitted");
                            btnCancel_Click(null, null);
                            FillLeaveStatusApproved();
                            FillLeaveStatusPending();
                            FillStatus();
                        }
                    }
                    else
                    {
                        b = objPartial.UpdatePartialLeaveRequest(hdnEdit.Value, Session["CompId"].ToString(), Session["EmpId"].ToString(), "1", DateTime.Now.ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        if (b != 0)
                        {
                            if (dtPartialLeave.Rows.Count > 0)
                            {

                            }

                            FillLeaveStatusApproved();
                            FillLeaveStatusPending();
                            FillStatus();
                            hdnEdit.Value = "";
                            hdnEditDate.Value = "0";
                            Lbl_Tab_New.Text = Resources.Attendance.New;
                        }
                    }
                }
                else
                {
                    DisplayMessage("Company does not provide partial leave");
                    return;
                }
            }

            if (PostedEmpList.ToString() != "" && NonPostedLog.ToString() != "")
            {
                DisplayMessage("Log Posted For Employee :- " + PostedEmpList.ToString().TrimEnd() + " Partial Leave Submitted For Employee :- " + NonPostedLog.ToString().TrimEnd() + "");
            }
            if (PostedEmpList.ToString() == "" && NonPostedLog.ToString().TrimEnd() != "")
            {
                DisplayMessage("Partial Leave Submitted");
            }
            if (PostedEmpList.ToString() != "" && NonPostedLog.ToString().TrimEnd() == "")
            {
                DisplayMessage("Log Posted For Employee :- " + PostedEmpList.ToString().TrimEnd() + "");
            }
            btnReset_Click(null, null);
            //FillLeaveStatus();
        }

        PrintReport(strTransId);
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

    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/attendance"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, hdnEmpId.Value, Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeName(hdnEmpId.Value) + " applied Partial Leave for " + txtApplyDate.Text + " Time : from " + txtInTime.Text + " to " + txtOuttime.Text + "";
        if (hdnEdit.Value == "")
        {
            // For Insert        
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "0");
        }
        else
        {
            // For Update
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["cur_trans_id"].ToString(), "15");
        }
    }



    public string GetEmployeeCode(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Code"].ToString();

            if (empname == "")
            {
                empname = "No Code";
            }

        }
        else
        {
            empname = "No Code";

        }

        return empname;



    }
    private void ClearFormFields()
    {
        txtApplyDate.Text = string.Empty;
        txtDescription.Text = string.Empty;
        return;
    }


    public int getCurrentMonthLeaveCount(DateTime applydate)
    {
        int Count = 0;

        DataTable dt = objPartial.GetPartialLeaveRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), Session["EmpId"].ToString(), applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Count++;
            }

        }
        else
        {
            Count = 0;

        }

        return Count;
    }

    public int getCurrentMonth(DateTime applydate)
    {
        int useminutes = 0;

        DataTable dt = objPartial.GetPartialLeaveRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), Session["EmpId"].ToString(), applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());

            }

        }
        else
        {
            useminutes = 0;

        }

        return useminutes;
    }

    public int getMinuteInADay(DateTime applydate)
    {
        int useminutes = 0;

        DataTable dt = objPartial.GetPartialLeaveRequestById(Session["CompId"].ToString(), Session["EmpId"].ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0' and Partial_Leave_Date='" + applydate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());

            }

        }
        else
        {
            useminutes = 0;

        }

        return useminutes;
    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {
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
    public string leavetype(string type)
    {
        string t = string.Empty;
        if (type == "0")
        {
            t = "Personal";

        }
        else
        {
            t = "Official";

        }
        return t;
    }
    public void FillLeaveStatusApproved()
    {
        DataTable dtPartialLeave = objAttendance.GetLeavePending(Session["EmpId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PartialDay", "2");
        dtPartialLeave = new DataView(dtPartialLeave, "Is_Confirmed='Approved'  AND CurrYear='" + DateTime.Today.Year + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPartialLeave.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvLeaveStatus, dtPartialLeave, "", "");
            Session["dtLeaveStatus"] = dtPartialLeave;

            //Add On 30-07-2015 
            DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
            dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPLTimeWithOutTime.Rows.Count > 0)
            {
                strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strPLWithTimeWithOutTime = "True";
            }

            if (strPLWithTimeWithOutTime == "True")
            {
                try
                {
                    gvLeaveStatus.Columns[4].Visible = true;
                    gvLeaveStatus.Columns[5].Visible = true;
                    gvLeaveStatus.Columns[6].Visible = false;
                }
                catch
                {

                }
            }
            else if (strPLWithTimeWithOutTime == "False")
            {
                try
                {
                    gvLeaveStatus.Columns[4].Visible = false;
                    gvLeaveStatus.Columns[5].Visible = false;
                    gvLeaveStatus.Columns[6].Visible = true;
                }
                catch
                {

                }
            }
        }
        else
        {
            gvLeaveStatus.DataSource = null;
            gvLeaveStatus.DataBind();
            Session["dtLeaveStatus"] = null;

        }
        AllPageCode();
    }
    public void FillLeaveStatusPending()
    {
        DataTable dtPartialLeaveP = objAttendance.GetLeavePending(Session["EmpId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PartialDay", "2");
        dtPartialLeaveP = new DataView(dtPartialLeaveP, "Is_Confirmed='Pending' AND CurrYear='" + DateTime.Today.Year + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPartialLeaveP.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvPending, dtPartialLeaveP, "", "");

            //Add On 30-07-2015 
            DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
            dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPLTimeWithOutTime.Rows.Count > 0)
            {
                strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strPLWithTimeWithOutTime = "True";
            }
            if (strPLWithTimeWithOutTime == "True")
            {
                try
                {
                    GvPending.Columns[5].Visible = true;
                    GvPending.Columns[6].Visible = true;
                    GvPending.Columns[7].Visible = false;
                }
                catch
                {

                }
            }
            else if (strPLWithTimeWithOutTime == "False")
            {
                try
                {
                    GvPending.Columns[5].Visible = false;
                    GvPending.Columns[6].Visible = false;
                    GvPending.Columns[7].Visible = true;
                }
                catch
                {

                }
            }
        }
        else
        {
            GvPending.DataSource = null;
            GvPending.DataBind();
        }
        AllPageCode();
    }
    protected void gvLeaveStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveStatus.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtLeaveStatus"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLeaveStatus, dt, "", "");

        //Add On 30-07-2015 
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }

        if (strPLWithTimeWithOutTime == "True")
        {
            try
            {
                gvLeaveStatus.Columns[4].Visible = true;
                gvLeaveStatus.Columns[5].Visible = true;
                gvLeaveStatus.Columns[6].Visible = false;
            }
            catch
            {

            }
        }
        else if (strPLWithTimeWithOutTime == "False")
        {
            try
            {
                gvLeaveStatus.Columns[4].Visible = false;
                gvLeaveStatus.Columns[5].Visible = false;
                gvLeaveStatus.Columns[6].Visible = true;
            }
            catch
            {

            }
        }

        foreach (GridViewRow gvr in gvLeaveStatus.Rows)
        {
            Label lblStatus = (Label)(gvr.FindControl("lblStatus"));
            if (lblStatus.Text != "Pending")
            {
                ImageButton imgBtnApprove = (ImageButton)(gvr.FindControl("IbtnApprove"));
                ImageButton imgBtnReject = (ImageButton)(gvr.FindControl("IbtnReject"));
                // imgBtnApprove.Visible = false;
                //imgBtnReject.Visible = false;

            }

        }

        AllPageCode();
    }
    protected void GvPending_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

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
    //protected void btnApprove_Command(object sender, CommandEventArgs e)
    //{
    //    int b = 0;

    //    DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(e.CommandName.ToString(), Session["CompId"].ToString());

    //    int totalminutes = 0;
    //    int useinday = 0;

    //    double leaveCount = 0;


    //    totalminutes = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString());
    //    useinday = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());

    //    leaveCount = double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString()) / double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());

    //    leaveCount = System.Math.Round(leaveCount);

    //    DataTable dt = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());



    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "1")
    //        {

    //            b = objPartial.PartialLeaveApproveReject(e.CommandArgument.ToString(), "Approved", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

    //            if (b != 0)
    //            {
    //                DisplayMessage("Partial Leave Approved");
    //                FillLeaveStatus();

    //            }
    //        }
    //        else
    //        {


    //            hdnEmpId.Value = e.CommandName.ToString();
    //            leaveCount = leaveCount - getCurrentMonthLeaveCount(Convert.ToDateTime(dt.Rows[0]["Partial_Leave_Date"].ToString()));

    //            int OneDayMin = getMinuteInADay(Convert.ToDateTime(dt.Rows[0]["Partial_Leave_Date"].ToString()));
    //            if (OneDayMin >= useinday)
    //            {
    //                DisplayMessage("You cannot request more than " + useinday.ToString() + " minutes in a day");
    //                return;
    //            }
    //            if (leaveCount > 0)
    //            {
    //                if (totalminutes > 0)
    //                {
    //                    int CurrentUseMin = getCurrentMonth(Convert.ToDateTime(dt.Rows[0]["Partial_Leave_Date"].ToString()));
    //                    if (CurrentUseMin > 0)
    //                    {
    //                        totalminutes = totalminutes - CurrentUseMin;

    //                    }

    //                    int RequestMin = GetMinuteDiff(dt.Rows[0]["To_Time"].ToString(), dt.Rows[0]["From_Time"].ToString());
    //                    if (RequestMin <= totalminutes)
    //                    {
    //                        if (RequestMin > useinday)
    //                        {
    //                            DisplayMessage("You cannot request more than " + useinday.ToString() + " minutes in a day");
    //                            return;
    //                        }
    //                        else
    //                        {




    //                        }
    //                    }
    //                    else
    //                    {
    //                        DisplayMessage("Employee does not have sufficient balance");
    //                        return;

    //                    }
    //                }
    //                else
    //                {
    //                    DisplayMessage("Employee does not have sufficient balance");
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                DisplayMessage("Employee does not have sufficient balance");
    //                return;
    //            }
    //            b = objPartial.PartialLeaveApproveReject(e.CommandArgument.ToString(), "Approved", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

    //            if (b != 0)
    //            {
    //                DisplayMessage("Partial Leave Approved");
    //                FillLeaveStatus();

    //            }
    //        }
    //    }




    //}


    //protected void btnEdit_Command(object sender, CommandEventArgs e)
    //{
    //    txtEmpName.Enabled = false;
    //    Common cmn = new Common();
    //    hdnEmpId.Value = e.CommandName.ToString();
    //    hdnEdit.Value = e.CommandArgument.ToString();
    //    DataTable dt = new DataTable();
    //    dt = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());

    //    if (dt.Rows.Count > 0)
    //    {
    //        txtEmpName.Text = cmn.GetEmpName(e.CommandName.ToString());

    //        if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "0")
    //        {
    //            rbtnPersonal.Checked = true;
    //            rbtnOfficial.Checked = false;
    //        }
    //        else
    //        {
    //            rbtnOfficial.Checked = true;
    //            rbtnPersonal.Checked = false;
    //        }

    //        txtApplyDate.Text = Convert.ToDateTime(dt.Rows[0]["Partial_Leave_Date"].ToString()).ToString(objSys.SetDateFormat());
    //        txtInTime.Text = dt.Rows[0]["From_Time"].ToString();
    //        txtOuttime.Text = dt.Rows[0]["To_Time"].ToString();
    //        txtDescription.Text = dt.Rows[0]["Description"].ToString();


    //        btnNew_Click(null, null);
    //        btnNew.Text = Resources.Attendance.Edit;


    //    }
    //}


    protected void IbtnReject_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objPartial.PartialLeaveApproveReject(e.CommandArgument.ToString(), "Canceled", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            DisplayMessage("Leave Rejected");
            FillLeaveStatusApproved();
            FillLeaveStatusPending();
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtApplyDate.Text = "";
        rbBegining.Checked = false;
        rbMiddle.Checked = false;
        rbEnding.Checked = false;

        rbTimeTable.ClearSelection();
        rbTimeTable.DataSource = null;
        rbTimeTable.DataBind();
        rbTimeTable.Items.Clear();

        txtInTime.Text = "";
        txtOuttime.Text = "";

        txtDescription.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;

        FillGvEmp();
        hdnEditDate.Value = "0";
        hdnEdit.Value = "";
        return;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
       txtApplyDate.Text = "";
        rbBegining.Checked = false;
        rbMiddle.Checked = false;
        rbEnding.Checked = false;

        txtInTime.Text = "";
        txtOuttime.Text = "";

        rbTimeTable.ClearSelection();
        rbTimeTable.DataSource = null;
        rbTimeTable.DataBind();
        rbTimeTable.Items.Clear();

        txtDescription.Text = "";

        Lbl_Tab_New.Text = Resources.Attendance.New;

        btnList_Click(null, null);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
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

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Common cmn = new Common(Session["DBConnection"].ToString());
        //hdnEmpId.Value = e.CommandName.ToString();

        //Add On 30-07-2015
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }

        DataTable dt = new DataTable();
        dt = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {

            hdnEdit.Value = e.CommandArgument.ToString();
            if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "0")
            {
                rbtnPersonal.Checked = true;
                rbtnOfficial.Checked = false;
            }
            else
            {
                rbtnOfficial.Checked = true;
                rbtnPersonal.Checked = false;
            }

            txtApplyDate.Text = Convert.ToDateTime(dt.Rows[0]["Partial_Leave_Date"].ToString()).ToString(objSys.SetDateFormat());
            hdnEditDate.Value = txtApplyDate.Text;

            if (strPLWithTimeWithOutTime == "False")
            {
                string strPLType = dt.Rows[0]["Field1"].ToString();
                string strTimeTableId = dt.Rows[0]["Field2"].ToString();
                if (strPLType == "B")
                {
                    rbBegining.Checked = true;
                    rbMiddle.Checked = false;
                    rbEnding.Checked = false;
                    rbBegining_CheckedChanged(sender, e);
                    rbTimeTable.SelectedValue = strTimeTableId;
                }
                else if (strPLType == "M")
                {
                    rbBegining.Checked = false;
                    rbMiddle.Checked = true;
                    rbEnding.Checked = false;
                    rbBegining_CheckedChanged(sender, e);
                    rbTimeTable.SelectedValue = strTimeTableId;
                }
                else if (strPLType == "E")
                {
                    rbBegining.Checked = false;
                    rbMiddle.Checked = false;
                    rbEnding.Checked = true;
                    rbBegining_CheckedChanged(sender, e);
                    rbTimeTable.SelectedValue = strTimeTableId;
                }
                else if (strPLType == "")
                {
                    rbBegining.Checked = false;
                    rbMiddle.Checked = false;
                    rbEnding.Checked = false;
                }
            }
            else
            {
                txtInTime.Text = dt.Rows[0]["From_Time"].ToString();
                txtOuttime.Text = dt.Rows[0]["To_Time"].ToString();
            }
            txtDescription.Text = dt.Rows[0]["Description"].ToString();


            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
            //txtEmpName_textChanged(sender, e);
        }
    }
    protected void rbBegining_CheckedChanged(object sender, EventArgs e)
    {
        string strEmployeeId = Session["EmpId"].ToString();


        if (rbBegining.Checked == true || rbMiddle.Checked == true || rbEnding.Checked == true)
        {
            if (txtApplyDate.Text == "")
            {
                DisplayMessage("Enter Apply Date");
                txtApplyDate.Focus();
                return;
            }

            if (strEmployeeId != "0" && strEmployeeId != "")
            {
                DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(strEmployeeId);
                if (dtShiftAllDate.Rows.Count > 0)
                {
                    dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + objSys.getDateForInput(txtApplyDate.Text.ToString()) + "' and Att_Date<='" + objSys.getDateForInput(txtApplyDate.Text.ToString()) + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
                    if (dtShiftAllDate.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 22-05-2015
                        objPageCmn.FillData((object)rbTimeTable, dtShiftAllDate, "TimeTable_Name", "TimeTable_Id");
                    }
                    else
                    {
                        rbTimeTable.ClearSelection();
                        rbTimeTable.DataSource = null;
                        rbTimeTable.DataBind();
                        rbTimeTable.Items.Clear();
                        DisplayMessage("First Assign Shift To This Employee");
                        return;
                    }
                }
            }
        }
    }
    protected string GetPLType(string strPLType)
    {
        string strType = string.Empty;
        if (strPLType == "B")
        {
            strType = "Begining";
        }
        else if (strPLType == "M")
        {
            strType = "Middle";
        }
        else if (strPLType == "E")
        {
            strType = "Ending";
        }
        else if (strPLType == "")
        {
            strType = "";
        }
        return strType;
    }


    //Added On 01-12-2015
    protected void rbtnPersonal_CheckedChanged(Object sender, EventArgs e)
    {
        //Add On 30-07-2015       
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }

        if (rbtnPersonal.Checked == true)
        {
            if (strPLWithTimeWithOutTime == "True")
            {
                TrWithTime1.Visible = true;
                TrWithOutTime1.Visible = false;
                TrWithOutTime2.Visible = false;
            }
            else if (strPLWithTimeWithOutTime == "False")
            {
                TrWithTime1.Visible = false;
                TrWithOutTime1.Visible = true;
                TrWithOutTime2.Visible = true;
            }
        }
        else if (rbtnOfficial.Checked == true)
        {
            TrWithTime1.Visible = true;
            TrWithOutTime1.Visible = false;
            TrWithOutTime2.Visible = false;
        }
    }
    protected void rbtnOfficial_CheckedChanged(Object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;

        //Add On 30-07-2015       
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }

        if (rbtnPersonal.Checked == true)
        {
            if (strPLWithTimeWithOutTime == "True")
            {
                TrWithTime1.Visible = true;
                TrWithOutTime1.Visible = false;
                TrWithOutTime2.Visible = false;
            }
            else if (strPLWithTimeWithOutTime == "False")
            {
                TrWithTime1.Visible = false;
                TrWithOutTime1.Visible = true;
                TrWithOutTime2.Visible = true;
            }
        }
        else if (rbtnOfficial.Checked == true)
        {
            TrWithTime1.Visible = true;
            TrWithOutTime1.Visible = false;
            TrWithOutTime2.Visible = false;
        }
    }

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {

        PrintReport(e.CommandArgument.ToString());
    }


    public void PrintReport(string LeaveId)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Vacation Leave Application.aspx?TransId=" + LeaveId + "','window','width=1024');", true);
    }



}