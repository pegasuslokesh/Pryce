using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class MasterSetUp_CompanyParameter : BasePage
{
    CompanyMaster objComp = null;
    SystemParameter objSys = null;
    Att_ShiftManagement objShift = null;
    Set_ApplicationParameter objAppParam = null;
    Inv_ParameterMaster objParam = null;
    Set_Allowance ObjAllowance = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;
    string StrUserId = string.Empty;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string compid = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();

        if (!IsPostBack)
        {
            setModuleWiseControls();
            //updated for commit
            if (Request.QueryString["CompanyId"] != null)
            {
                //btnTime_Click(null, null);
                //btnAttendence_Click(null, null);

                compid = Request.QueryString["CompanyId"].ToString().Substring(1, Request.QueryString["CompanyId"].ToString().Length - 2);
                DataTable dtComp = objComp.GetCompanyMasterById(compid);
                if (dtComp.Rows.Count > 0)
                {
                    lblHeader.Text = dtComp.Rows[0]["Company_Name"].ToString();
                    FillShift(compid);
                    SetCompanyParameter(compid);
                    hdnCompanyId.Value = compid;
                }
                else
                {
                    lblHeader.Text = "";
                }
            }
        }

        objSys.GetSysTitle();
        // Hide Panels According to Application Id..........................
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "1")
        {
            pnlTimetableList.Visible = false;
            pnlSMSEmailB.Visible = false;
            pnlKeyPref.Visible = false;
            pnlColorCode.Visible = false;
            PnlTime.Visible = false;
        }
        else if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "4")
        {
            //PnlHR.Visible = false;
        }
    }



    protected void txtLsApplicableAllowances_OnTextChanged(object sender, EventArgs e)
    {
        if (txtLsApplicableAllowances.Text != "")
        {

            foreach (string str in txtLsApplicableAllowances.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows.Count == 0)
                {
                    DisplayMessage("select allowance in suggestion only");
                    txtLsApplicableAllowances.Text = "";
                    txtLsApplicableAllowances.Focus();
                    break;
                }


            }

            txtLsApplicableAllowances.Focus();

        }


    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Get_Applicable_Allowance(string prefixText, int count, string contextKey)
    {
        Set_Allowance obj = new Set_Allowance(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = obj.GetDistinctAllowance(HttpContext.Current.Session["CompId"].ToString(), prefixText);
        string[] str = new string[dt.Rows.Count];

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] += dt.Rows[i][0].ToString() + ",";
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
                dt = obj.GetAllowanceTrueAll("1");
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] += dt.Rows[i]["Allowance"].ToString() + ",";
                    }
                }
            }
        }
        return str;
    }


    public void ChkDbLocation_CheckedChanged(Object sender, EventArgs e)
    {
        if (ChkDbLocation.Checked == true)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string Location = path + "DatabaseBackup";
            string Db_Backup_Restore_Path = path + Location;
            txtBackupLoc.Text = Db_Backup_Restore_Path;
            txtBackupLoc.Enabled = false;
        }
        else
        {
            txtBackupLoc.Enabled = true;
            txtBackupLoc.Text = "D:\\Nitin";
        }
    }
    public void AllPageCode()
    {

        Page.Title = objSys.GetSysTitle();
        Session["AccordianId"] = "8";
        Session["HeaderText"] = "Master Setup";
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();


        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "11", "158", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {
                btnCSave.Visible = true;
                foreach (GridViewRow Row in GvParameter.Rows)
                {
                    ((ImageButton)Row.FindControl("btnEdit")).Visible = true;

                }

            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (Convert.ToBoolean(DtRow["Op_Add"].ToString()))
                    {
                        btnCSave.Visible = true;
                    }
                    foreach (GridViewRow Row in GvParameter.Rows)
                    {
                        if (Convert.ToBoolean(DtRow["Op_Edit"].ToString()))
                        {
                            ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                        }
                        if (Convert.ToBoolean(DtRow["Op_Delete"].ToString()))
                        {
                            //((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                        }
                    }
                    if (Convert.ToBoolean(DtRow["Op_Restore"].ToString()))
                    {
                        //imgBtnRestore.Visible = true;
                        //ImgbtnSelectAll.Visible = false;
                    }
                    if (Convert.ToBoolean(DtRow["Op_View"].ToString()))
                    {

                    }
                    if (Convert.ToBoolean(DtRow["Op_Print"].ToString()))
                    {

                    }
                    if (Convert.ToBoolean(DtRow["Op_Download"].ToString()))
                    {

                    }
                    if (Convert.ToBoolean(DtRow["Op_Upload"].ToString()))
                    {

                    }
                }
            }
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
    }

    public void FillShift(string compid)
    {
        DataTable dt = objShift.GetShiftMaster(compid);
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlDefaultShift, dt, "Shift_Name", "Shift_Id");
        }
        else
        {
            ddlDefaultShift.DataSource = null;
            ddlDefaultShift.Items.Insert(0, "--Select--");
            ddlDefaultShift.SelectedIndex = 0;
        }
    }


    public string getAllowanceNamebyId(string strRefId)
    {
        string strAllowancename = string.Empty;

        if (strRefId != "0")
        {
            foreach (string str in strRefId.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                strAllowancename += ObjAllowance.GetAllowanceTruebyId(Session["CompId"].ToString(), str).Rows[0]["Allowance"].ToString() + ",";

            }
        }
        return strAllowancename;
    }



    public void SetCompanyParameter(string CompId)
    {

        string strTdsFunctionality = string.Empty;

        strTdsFunctionality = objAppParam.GetApplicationParameterValueByParamName("TDS_Functionality", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (strTdsFunctionality == "Auto")
        {
            rbtnTDSAuto.Checked = true;
            rbtnTDSManual.Checked = false;
        }
        else
        {
            rbtnTDSAuto.Checked = false;
            rbtnTDSManual.Checked = true;
        }

        txtLsApplicableAllowances.Text = getAllowanceNamebyId(objAppParam.GetApplicationParameterValueByParamName("LS_ApplicableAllowance", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        if (objAppParam.GetApplicationParameterValueByParamName("LS_Month_Days_Count", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "Month Days")
        {
            txtLsFixedDays.Text = "";
            ddlLSDaysCount.SelectedIndex = 1;
        }
        else
        {
            txtLsFixedDays.Text = objAppParam.GetApplicationParameterValueByParamName("LS_Month_Days_Count", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ddlLSDaysCount.SelectedIndex = 0;
        }


        if (objAppParam.GetApplicationParameterValueByParamName("Ls_Pay_Scale", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "Current")
        {
            rbtnCurrentSalary.Checked = true;
            rbtnactualSalary.Checked = false;
        }
        else
        {
            rbtnCurrentSalary.Checked = false;
            rbtnactualSalary.Checked = true;

        }
        try
        {
            //chkWorkMinutePenalty.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsworkMinute_In_Penalty", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkbreakMinutePenalty.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsbreakMinute_In_Penalty", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkRelaxationMinutePenalty.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsrelaxationMinute_In_Penalty", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

            txtEmailAlertDevice.Text = objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnDeviceStatusAlert", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            chkSalaryPlanEnable.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsSalaryPlanEnable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkLogPostMendatory.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLogPostMendatory", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            txtSeniorCitizenagelimit.Text = objAppParam.GetApplicationParameterValueByParamName("Senior_Citizen_Age_Limit", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtPfApplicablesalary.Text = objAppParam.GetApplicationParameterValueByParamName("PF_Applicable_Salary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtAdminCharges.Text = objAppParam.GetApplicationParameterValueByParamName("PF_Admin_Charges", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtPfEdLi.Text = objAppParam.GetApplicationParameterValueByParamName("PF_EDLI", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtpfInspectionCharges.Text = objAppParam.GetApplicationParameterValueByParamName("PF_Inspection_Charges", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtESicApplicablesalary.Text = objAppParam.GetApplicationParameterValueByParamName("ESIC_Applicable_Salary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch (Exception ex)
        {

        }
        string strHolidayValue = objAppParam.GetApplicationParameterValueByParamName("Holiday_Validity", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (strHolidayValue.Trim().Contains("-"))
        {
            ddlHolidayValidity.SelectedValue = strHolidayValue.Trim().Split('-')[0];
            ddlHolidayValidity_SelectedIndexChanged(null, null);
            txtHolidayValue.Text = strHolidayValue.Trim().Split('-')[1];
        }
        else
        {
            ddlHolidayValidity.SelectedValue = strHolidayValue.Trim();
            ddlHolidayValidity_SelectedIndexChanged(null, null);
        }



        chkShiftApproval.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Shift_Approval_Functionality", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        chkLeaveApproval.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Approval_Functionality", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkLeaveValidation.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Transaction_on_uploading", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        chkIsLeaveSalary.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsLeaveSalary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        try
        {
            chkIsManualLog.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_ManualLog_OnLeave", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            chkLogpriorityonLeave.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Logs_priority_OnLeave", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }




        //report notification related parameter 
        //added by jitendra on 12-09-2017

        try
        {
            chkNotification.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Full_Report_On_Month_End", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }


        chkWeekoffLeaveSal.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsWeekOffLeaveSalary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkHolidayLeaveSal.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsHolidayLeaveSalary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkAbsentLeaveSal.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAbsentLeaveSalary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkShiftFunctionKey.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("WithShiftWithoutFunctionKey", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkPaidLeaveSal.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsPaidLeaveForLeaveSalary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkUnPaidLeaveSal.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsUnPaidLeaveForLeaveSalary", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        //Add Company Parameter On 26-06-2015
        chkNewUserUpload.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAddNewUserUpload", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkNewUserDownload.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsAddNewUserDownload", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkForWorkHour.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("For Work Hour", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkLateWithoutPresent.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("LateEarlyWithoutPresent", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkWeekOffInShift.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("AddWeekOffInShift", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        ddlForLogReport.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("For Log Report", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlPageLevel.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Page Level", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlParameterLevel.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("ParameterWorksOn", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        chkAutoGenerateEmployeeCode.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("EmployeeCode", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkManualAttendanceVerified.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("ForManualAttendance", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkHolidayAssignOnWeekOff.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Holiday Assign On Week Off", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        //End


        txtHalfDayCount.Text = objAppParam.GetApplicationParameterValueByParamName("Half_Day_Count", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        chkweekoffsandwich.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Absent_sandwich_on_week_Off", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkholidaysandwich.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Absent_sandwich_on_holiday", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));


        txtShortestTime.Text = objAppParam.GetApplicationParameterValueByParamName("Shortest Time Table", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        // txtMinDiffTime.Text = objAppParam.GetApplicationParameterValueByParamName("Min Difference Between TimeTable in Shift", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtWorkDayMin.Text = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtServiceRunTime.Text = objAppParam.GetApplicationParameterValueByParamName("Service_Run_Time", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtDownloadServicePath.Text = objAppParam.GetApplicationParameterValueByParamName("ServiceInterval", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        // Service Interval 
        //txtDownloadServicePath.Text = objSys.GetSysParameterByParamName("ServiceLogStatusPath").ToString();
        ddlExculeDay.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlFinancialYear.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlEmpSync.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Employee Synchronization", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtBackupLoc.Text = objAppParam.GetApplicationParameterValueByParamName("Backup_Restore_Location", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        DataTable DtHrParam = objAppParam.GetApplicationParameterByCompanyId("HR Parameter", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            DtHrParam = new DataView(DtHrParam, "Param_Name='HR Parameter'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        string chkDbLoc = objAppParam.GetApplicationParameterValueByParamName("IsDefault_DbLocation", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (chkDbLoc.ToString() == "True")
        {
            ChkDbLocation.Checked = true;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string Location = path + "DatabaseBackup";
            string Db_Backup_Restore_Path = Location;
            txtBackupLoc.Text = Db_Backup_Restore_Path;
            txtBackupLoc.Enabled = false;
        }
        else
        {
            ChkDbLocation.Checked = false;
            txtBackupLoc.Enabled = false;
            txtBackupLoc.Text = objAppParam.GetApplicationParameterValueByParamName("Backup_Restore_Location", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }

        if (objAppParam.GetApplicationParameterValueByParamName("Display TimeTable In All Brand", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "True")
        {
            rbtnBrandYes.Checked = true;
        }
        else
        {
            // rbtnBrandNo.Checked = true;
        }

        for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
        {
            ChkWeekOffList.Items[i].Selected = false;
        }
        try
        {
            string[] WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Split(',');
            for (int j = 0; j < WeekOffDays.Length; j++)
            {
                for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
                {
                    if (ChkWeekOffList.Items[i].Text == WeekOffDays[j].ToString())
                    {
                        ChkWeekOffList.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }
        catch
        {
        }

        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnOtEnable.Checked = true;
            rbtnOtDisable.Checked = false;
            rbnWeekOffOTEnable.Checked = true;
            rbnWeekOffOTDisable.Checked = false;
            rbnHoliayOTEnable.Checked = true;
            rbnHoliayOTDisable.Checked = false;


            rbnApprovalOTEnable.Checked = true;
            rbnApprovalOTDisable.Checked = false;

            rbtOT_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnOtEnable.Checked = false;
            rbtnOtDisable.Checked = true;
            rbnWeekOffOTEnable.Checked = false;
            rbnWeekOffOTDisable.Checked = true;
            rbnHoliayOTEnable.Checked = false;
            rbnHoliayOTDisable.Checked = true;


            rbnApprovalOTEnable.Checked = false;
            rbnApprovalOTDisable.Checked = true;

            rbtOT_OnCheckedChanged(null, null);
        }
        try
        {
            chkLeaveCountForWeekOff.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkLeaveCountForHoliday.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }

        txtMaxOTMint.Text = objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtMinOTMint.Text = objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlCalculationMethod.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlWorkCal.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        // ddlShiftPref.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Without Shift Preference", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlSalCal.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Salary Calculate According To", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlSalCal_OnSelectedIndexChanged(null, null);

        ddlPaySal.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Pay Salary Acc To Work Hour or Ref Hour", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlPaySal_OnSelectedIndexChanged(null, null);
        ddlDeductionMinuteForDay.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("LateIn_MinuteDeduction_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlOutDeductionMinuteForDay.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("EarlyOut_MinuteDeduction_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtFixedDays.Text = objAppParam.GetApplicationParameterValueByParamName("Days In Month", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        //For Salary Increment
        string strSalInc = objAppParam.GetApplicationParameterValueByParamName("Salary_Increment_Enable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (strSalInc == "True")
        {
            rbSalIncEnable.Checked = true;
            rbSalIncDisable.Checked = false;
            rbSalInc_OnCheckedChanged(null, null);
        }
        else if (strSalInc == "False")
        {
            rbSalIncEnable.Checked = false;
            rbSalIncDisable.Checked = true;
            rbSalInc_OnCheckedChanged(null, null);
        }

        txtSalIncrDurationForExperience.Text = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtWorkPercentFrom1.Text = objAppParam.GetApplicationParameterValueByParamName("WorkPercentFrom1", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtWorkPercentFrom2.Text = objAppParam.GetApplicationParameterValueByParamName("WorkPercentFrom2", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtWorkPercentFrom3.Text = objAppParam.GetApplicationParameterValueByParamName("WorkPercentFrom3", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtWorkPercentTo1.Text = objAppParam.GetApplicationParameterValueByParamName("WorkPercentTo1", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtWorkPercentTo2.Text = objAppParam.GetApplicationParameterValueByParamName("WorkPercentTo2", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtWorkPercentTo3.Text = objAppParam.GetApplicationParameterValueByParamName("WorkPercentTo3", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtValue1.Text = objAppParam.GetApplicationParameterValueByParamName("Value1", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtValue2.Text = objAppParam.GetApplicationParameterValueByParamName("Value2", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtValue3.Text = objAppParam.GetApplicationParameterValueByParamName("Value3", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        // HR Parameter Nitin Jain On 27/11/2014
        // HR Parameter Added New On 27/08/2023

        try
        {
            chkAllowAllEmployeesonGratuity.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("AllowAllEmployeeOnGratuity", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkLogPostOnTermination.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryLogPostOnTermination", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkPayrollPostOnTermination.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryPayrollPostOnTermination", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            chkFinanceAccountConfiguration.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("NecessaryCheckFinanceOnTermination", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        }
        catch
        {

        }


        txtProbationPeriod.Text = objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            txtIndemnityYear.Enabled = true;
            txtIndemnityDays.Enabled = true;
            rbnIndemnity1.Checked = true;
            rbnIndemnity2.Checked = false;
        }
        else
        {
            txtIndemnityYear.Enabled = false;
            txtIndemnityDays.Enabled = false;
            rbnIndemnity1.Checked = false;
            rbnIndemnity2.Checked = true;
        }
        txtIndemnityYear.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtIndemnityDays.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        // txtSalGIven.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnitySalaryValue", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", CompId)))
        //{
        //    rbnIndemnity1.Checked = true;
        //    txtIndemnityYear.Enabled = true;
        //    ddlIndenityType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Indemnity_GivenType", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //    if (ddlIndenityType.SelectedValue == "1")
        //    {
        //        ddlIndemnitySalary.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Indemenity_SalaryCalculationType", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        txtIndemnityLeave.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        dvIndemnitySalary.Visible = true;
        //        dvIndemnityLeave.Visible = false;
        //    }
        //    else
        //    {
        //        ddlIndemnitySalary.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Indemenity_SalaryCalculationType", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        txtIndemnityLeave.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        dvIndemnityLeave.Visible = true;
        //        dvIndemnitySalary.Visible = false;
        //    }
        //}
        //else
        //{
        //    ddlIndenityType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Indemnity_GivenType", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //    if (ddlIndenityType.SelectedValue == "1")
        //    {
        //        ddlIndemnitySalary.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Indemenity_SalaryCalculationType", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        txtIndemnityLeave.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        dvIndemnitySalary.Visible = true;
        //        dvIndemnityLeave.Visible = false;
        //    }
        //    else
        //    {
        //        ddlIndemnitySalary.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Indemenity_SalaryCalculationType", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        txtIndemnityLeave.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        //        dvIndemnityLeave.Visible = true;
        //        dvIndemnitySalary.Visible = false;
        //    }
        //    rbnIndemnity2.Checked = true;
        //    txtIndemnityYear.Enabled = false;
        //    ddlIndenityType.Enabled = false;
        //    ddlIndemnitySalary.Enabled = false;
        //    txtIndemnityLeave.Enabled = false;
        //}


        //--------------------------------------
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("SMS_Enable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnSMSEnable.Checked = true;
            rbtnSMSDisable.Checked = false;
            rbtSMS_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnSMSEnable.Checked = false;
            rbtnSMSDisable.Checked = true;
            rbtSMS_OnCheckedChanged(null, null);
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Email_Enable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnEmailEnable.Checked = true;
            rbtnEmailDisable.Checked = false;
            rbtEmail_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnEmailEnable.Checked = false;
            rbtnEmailDisable.Checked = true;
            rbtEmail_OnCheckedChanged(null, null);
        }

        txtprmUploadSize.Text = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        chkIntegratedwithPOS.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("POSIntegrationWithPryce", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (chkIntegratedwithPOS.Checked == true)
        {
            btnSynchPOSData.Visible = true;
        }
        else
        {
            btnSynchPOSData.Visible = false;
        }

        txtSMSAPI.Text = objAppParam.GetApplicationParameterValueByParamName("SMS_API", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtSmsPassword.Attributes.Add("Value", Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("SMS_User_Password", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())));

        txtUserId.Text = objAppParam.GetApplicationParameterValueByParamName("SMS_User_Id", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtSenderId.Text = objAppParam.GetApplicationParameterValueByParamName("Sender_Id", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtEmail.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtPasswordEmail.Attributes.Add("Value", Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())));

        txtSMTP.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtPasswordReminder.Text = objAppParam.GetApplicationParameterValueByParamName("Password Reminder(In Days)", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtPort.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtPop3.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Server_In", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtpopport.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port_In", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        chkSendEmail.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        try
        {
            chkEnableSSL.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Master_Email_EnableSSL", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            chkEnableSSL.Checked = false;
        }


        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnPartialEnable.Checked = true;
            rbtnPartialDisable.Checked = false;
            rbtPartial_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnPartialEnable.Checked = false;
            rbtnPartialDisable.Checked = true;
            rbtPartial_OnCheckedChanged(null, null);
        }

        txtTotalMinutes.Text = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtMinuteday.Text = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        chkPLWithTimeWithOutTime.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("PLTimeWithOutTime", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkPLDateEditable.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("PL Date Editable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Carry Forward Partial Leave Minutes", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnCarryYes.Checked = true;
            rbtnCarryNo.Checked = false;
        }
        else
        {
            rbtnCarryYes.Checked = false;
            rbtnCarryNo.Checked = true;
        }

        txtViolation.Text = objAppParam.GetApplicationParameterValueByParamName("Partial_Violation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (objAppParam.GetApplicationParameterValueByParamName("With Key Preference", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "Yes")
        {
            rbtnKeyEnable.Checked = true;
            rbtnKeyDisable.Checked = false;
            rbtKeyPref_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnKeyEnable.Checked = false;
            rbtnKeyDisable.Checked = true;
            rbtKeyPref_OnCheckedChanged(null, null);
        }

        txtInKey.Text = objAppParam.GetApplicationParameterValueByParamName("In Func Key", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtOutKey.Text = objAppParam.GetApplicationParameterValueByParamName("Out Func Key", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtBreakInKey.Text = objAppParam.GetApplicationParameterValueByParamName("Break In Func Key", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtBreakOutKey.Text = objAppParam.GetApplicationParameterValueByParamName("Break Out Func Key", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtPartialInKey.Text = objAppParam.GetApplicationParameterValueByParamName("Partial Leave In  Func Key", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtPartialOutKey.Text = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Out  Func Key", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlShiftRange.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Shift Range", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Consider Next Day Log", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            ChkNextDayLog.Checked = true;
        }
        else
        {
            ChkNextDayLog.Checked = false;
        }
        // Nitin Jain on 05/01/2015 Late In Min Count As Half Day .............
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("LateInCountAsHalfDay", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            chkLateInCountAsHalfDay.Checked = true;
        }
        else
        {
            chkLateInCountAsHalfDay.Checked = false;
        }

        txtAbsentColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtPresentColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtLateColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("Late_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtEarlyColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("Early_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtLeaveColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtHolidayColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtWeekOffColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("WeekOff_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtTempShiftColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("TempShift_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtMIcolorcode.Text = objAppParam.GetApplicationParameterValueByParamName("MI_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtNRColorCode.Text = objAppParam.GetApplicationParameterValueByParamName("NR_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtMOcolorcode.Text = objAppParam.GetApplicationParameterValueByParamName("MO_Color_Code", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());



        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtLateInEnable.Checked = true;
            rbtLateInDisable.Checked = false;
            rbtLateIn_OnCheckedChanged(null, null);


        }
        else
        {
            rbtLateInEnable.Checked = false;
            rbtLateInDisable.Checked = true;
            rbtLateIn_OnCheckedChanged(null, null);
        }

        if (objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Trim() == "Salary")
        {
            rbtnLateSalary.Checked = true;
            rbtnLateMinutes.Checked = false;
            rbtLateType_OnCheckedChanged(null, null);

            txtLateRelaxMin.Text = objAppParam.GetApplicationParameterValueByParamName("Late_Relaxation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtLateCount.Text = objAppParam.GetApplicationParameterValueByParamName("Late_Occurence", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtLateValue.Text = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Value", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ddlLateType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        }
        else
        {
            rbtnLateSalary.Checked = false;
            rbtnLateMinutes.Checked = true;
            rbtLateType_OnCheckedChanged(null, null);
            ddlLateMinTime.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Late_Penalty_Min_Deduct", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtLateRelaxMin.Text = objAppParam.GetApplicationParameterValueByParamName("Late_Relaxation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtLateRelaxMinWithMTimes.Text = objAppParam.GetApplicationParameterValueByParamName("Late_Relaxation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            txtLateCount.Text = objAppParam.GetApplicationParameterValueByParamName("Late_Occurence", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtLateValue.Text = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Value", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ddlLateType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtEarlyOutEnable.Checked = true;
            rbtEarlyOutDisable.Checked = false;
            rbtEarlyOut_OnCheckedChanged(null, null);
        }
        else
        {
            rbtEarlyOutEnable.Checked = false;
            rbtEarlyOutDisable.Checked = true;
            rbtEarlyOut_OnCheckedChanged(null, null);
        }
        if (objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Trim() == "Salary")
        {
            rbtnEarlySalary.Checked = true;
            rbtnEarlyMinutes.Checked = false;
            rbtEarlyType_OnCheckedChanged(null, null);
            txtEarlyRelaxMin.Text = objAppParam.GetApplicationParameterValueByParamName("Early_Relaxation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtEarlyCount.Text = objAppParam.GetApplicationParameterValueByParamName("Early_Occurence", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtEarlyValue.Text = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Value", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ddlEarlyType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        }
        else
        {
            rbtnEarlySalary.Checked = false;
            rbtnEarlyMinutes.Checked = true;
            rbtEarlyType_OnCheckedChanged(null, null);
            ddlEarlyMinTime.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Early_Penalty_Min_Deduct", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            txtEarlyRelaxMin.Text = objAppParam.GetApplicationParameterValueByParamName("Early_Relaxation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            txtEarlyRelaxMinWithMinTimes.Text = objAppParam.GetApplicationParameterValueByParamName("Early_Relaxation_Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            txtEarlyCount.Text = objAppParam.GetApplicationParameterValueByParamName("Early_Occurence", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtEarlyValue.Text = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Value", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ddlEarlyType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        }


        if (objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Trim() == "Salary")
        {
            rbtnPartialSalary.Checked = true;
            rbtnPartialMinutes.Checked = false;
            rbtPartialType_OnCheckedChanged(null, null);

            txtPartialValue.Text = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Salary_Value", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ddlPartialType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Salary_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        }
        else
        {
            rbtnPartialSalary.Checked = false;
            rbtnPartialMinutes.Checked = true;
            rbtPartialType_OnCheckedChanged(null, null);

            ddlPartialMinTime.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Min_Deduct", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        }
        txtAbsentDeduc.Text = objAppParam.GetApplicationParameterValueByParamName("Absent_Value", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlAbsentType.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Absent_Type", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        if (objAppParam.GetApplicationParameterValueByParamName("Default_Shift", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) != "0")
        {
            try
            {
                ddlDefaultShift.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Default_Shift", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch
            {

            }
        }
        else
        {
            ddlDefaultShift.SelectedIndex = 0;
        }

        chkNoClockIn.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("No_Clock_In_CountAsAbsent", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        chkNoClockOut.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("No_Clock_Out_CountAsAbsent", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));

        ChkNoClockLate.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsNoClockInLate", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        txtNoClockLate.Text = objAppParam.GetApplicationParameterValueByParamName("NoClockInLateMin", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ChkNoClockEarly.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsNoClockOutEarly", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        txtNoClockEarly.Text = objAppParam.GetApplicationParameterValueByParamName("NoClockOutEarlyMin", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        chkAfterEInAbsent.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("AfterEndingInAbsent", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        //  txtCompareLog.Text = objAppParam.GetApplicationParameterValueByParamName("CompareLogs", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtEmpPF.Text = objAppParam.GetApplicationParameterValueByParamName("Employee_PF", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtEmployerPf.Text = objAppParam.GetApplicationParameterValueByParamName("Employer_PF", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            txtEmployerFPF.Text = objAppParam.GetApplicationParameterValueByParamName("Employer_FPF", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            txtEmployerFPF.Text = "0";

        }
        txtEmpESIC.Text = objAppParam.GetApplicationParameterValueByParamName("Employee_ESIC", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtEmployerESIC.Text = objAppParam.GetApplicationParameterValueByParamName("Employer_ESIC", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtSalIncrDuration.Text = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtFreshPerFrom.Text = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtFreshPerTo.Text = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtExpPerFrom.Text = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtExpPerTo.Text = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string HolidayOTEnable = objAppParam.GetApplicationParameterValueByParamName("HolidayOTEnable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (HolidayOTEnable == "1")
        {
            rbnHoliayOTEnable.Checked = true;
            rbnHoliayOTDisable.Checked = false;
        }
        else
        {
            rbnHoliayOTEnable.Checked = false;
            rbnHoliayOTDisable.Checked = true;
        }

        string ApprovalOTEnable = objAppParam.GetApplicationParameterValueByParamName("OverTime Approval", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (ApprovalOTEnable == "1")
        {
            rbnApprovalOTEnable.Checked = true;
            rbnApprovalOTDisable.Checked = false;
        }
        else
        {
            rbnApprovalOTEnable.Checked = false;
            rbnApprovalOTDisable.Checked = true;
        }

        string WeekOffOTEnable = objAppParam.GetApplicationParameterValueByParamName("WeekOffOTEnable", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (WeekOffOTEnable == "1")
        {
            rbnWeekOffOTEnable.Checked = true;
            rbnWeekOffOTDisable.Checked = false;
        }
        else
        {
            rbnWeekOffOTEnable.Checked = false;
            rbnWeekOffOTDisable.Checked = true;
        }

        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("WeekOff_Report", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            ChkWeekOff.Checked = true;
        }
        else
        {
            ChkWeekOff.Checked = false;
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Holiday_Report", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            ChkHoliday.Checked = true;
        }
        else
        {
            ChkHoliday.Checked = false;
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Report", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            ChkLeave.Checked = true;
        }
        else
        {
            ChkLeave.Checked = false;
        }



        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbnProbation1.Checked = true;
            rbnProbation2.Checked = false;
            txtProbationPeriod.Enabled = true;
        }
        else
        {
            rbnProbation2.Checked = true;
            rbnProbation1.Checked = false;
            txtProbationPeriod.Enabled = false;
        }
    }
    protected void rbtPartial_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPartialEnable.Checked)
        {
            rbtnCarryYes.Enabled = true;
            rbtnCarryNo.Enabled = true;
            txtTotalMinutes.Enabled = true;
            txtMinuteday.Enabled = true;
            txtViolation.Enabled = true;

        }
        else
        {
            rbtnCarryYes.Enabled = false;
            rbtnCarryNo.Enabled = false;
            txtTotalMinutes.Enabled = false;
            txtMinuteday.Enabled = false;
            txtViolation.Enabled = false;
        }
    }

    protected void rbSalInc_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbSalIncEnable.Checked)
        {

            txtSalIncrDuration.Enabled = true;
            txtFreshPerFrom.Enabled = true;
            txtFreshPerTo.Enabled = true;
            txtSalIncrDurationForExperience.Enabled = true;
            txtExpPerFrom.Enabled = true;
            txtExpPerTo.Enabled = true;
        }
        else if (rbSalIncDisable.Checked)
        {
            txtSalIncrDuration.Enabled = false;
            txtFreshPerFrom.Enabled = false;
            txtFreshPerTo.Enabled = false;
            txtSalIncrDurationForExperience.Enabled = false;
            txtExpPerFrom.Enabled = false;
            txtExpPerTo.Enabled = false;
        }
    }
    #region HR Parameter
    protected void btnProbation_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        string strTdsFunctionality = string.Empty;
        string strAgeLimit = string.Empty;

        if (rbtnTDSAuto.Checked)
        {
            strTdsFunctionality = "Auto";
        }
        else
        {
            strTdsFunctionality = "Manual";
        }

        if (txtSeniorCitizenagelimit.Text == "")
        {
            strAgeLimit = "0";
        }
        else
        {
            strAgeLimit = txtSeniorCitizenagelimit.Text;
        }




        if (strParameterWorksOn == "Location")
        {
            if (rbnProbation1.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsProbationPeriod", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsProbationPeriod", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }

            if (rbnIndemnity1.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsIndemnity", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsIndemnity", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }

            //   objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnityLeave", txtIndemnityLeave.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            // objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Indemnity_GivenType", ddlIndenityType.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            // objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnityYear", txtIndemnityYear.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //  objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Indemenity_SalaryCalculationType", ddlIndemnitySalary.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnityDayas", txtIndemnityDays.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "TDS_Functionality", strTdsFunctionality, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());



            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Senior_Citizen_Age_Limit", strAgeLimit, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsSalaryPlanEnable", chkSalaryPlanEnable.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsLogPostMendatory", chkLogPostMendatory.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "AllowAllEmployeeOnGratuity", chkAllowAllEmployeesonGratuity.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NecessaryLogPostOnTermination", chkLogPostOnTermination.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NecessaryPayrollPostOnTermination", chkPayrollPostOnTermination.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NecessaryCheckFinanceOnTermination", chkFinanceAccountConfiguration.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            //objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnitySalaryValue", txtSalGIven.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else if (strParameterWorksOn == "Company")
        {
            if (rbnProbation1.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsProbationPeriod", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsProbationPeriod", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            if (rbnIndemnity1.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsIndemnity", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsIndemnity", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            //   objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnityLeave", txtIndemnityLeave.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            // objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Indemnity_GivenType", ddlIndenityType.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            // objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnityYear", txtIndemnityYear.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //  objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Indemenity_SalaryCalculationType", ddlIndemnitySalary.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IndemnityDayas", txtIndemnityDays.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IndemnitySalaryValue", txtSalGIven.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "TDS_Functionality", strTdsFunctionality, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());



            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Senior_Citizen_Age_Limit", strAgeLimit, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsSalaryPlanEnable", chkSalaryPlanEnable.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsLogPostMendatory", chkLogPostMendatory.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "AllowAllEmployeeOnGratuity", chkAllowAllEmployeesonGratuity.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NecessaryLogPostOnTermination", chkLogPostOnTermination.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NecessaryPayrollPostOnTermination", chkPayrollPostOnTermination.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NecessaryCheckFinanceOnTermination", chkFinanceAccountConfiguration.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }

        objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "ProbationPeriod", txtProbationPeriod.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ///here we are checking that parameter already exist or not 
        ///




        DisplayMessage("Record Saved", "green");
    }
    protected void rbnIndemnity1_OnCheckedChanged(object sender, EventArgs e)
    {
        txtIndemnityYear.Enabled = true;
        txtIndemnityDays.Enabled = true;
    }
    protected void rbnIndemnity2_OnCheckedChanged(object sender, EventArgs e)
    {
        txtIndemnityYear.Enabled = false;
        txtIndemnityDays.Enabled = false;
    }
    protected void ddlIndenityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //dvIndemnitySalary.Visible = false;
        //dvIndemnityLeave.Visible = false;
        //if (ddlIndenityType.SelectedValue == "1")
        //{
        //    dvIndemnitySalary.Visible = true;
        //}
        //else
        //{
        //    dvIndemnityLeave.Visible = true;
        //}
    }
    protected void rbnProbation1_OnCheckedChanged(object sender, EventArgs e)
    {
        txtProbationPeriod.Enabled = true;
    }
    protected void rbnProbation2_OnCheckedChanged(object sender, EventArgs e)
    {
        txtProbationPeriod.Enabled = false;
    }
    #endregion

    protected void btnSavePartial_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        if (strParameterWorksOn == "Location")
        {
            if (rbtnPartialEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Leave_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Leave_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            if (txtTotalMinutes.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtTotalMinutes.Focus();
                return;
            }

            if (txtMinuteday.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtMinuteday.Focus();
                return;
            }

            if (txtMaxOTMint.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtMaxOTMint.Focus();
                return;
            }

            if (txtMinOTMint.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtMinOTMint.Focus();
                return;
            }


            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Total Partial Leave Minutes", txtTotalMinutes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial Leave Minute Use In A Day", txtMinuteday.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Violation_Min", txtViolation.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "PLTimeWithOutTime", chkPLWithTimeWithOutTime.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "PL Date Editable", chkPLDateEditable.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            //if (chkPLWithTimeWithOutTime.Checked == true)
            //{
            //    string strsqla = "Update IT_ObjectEntry Set Page_Url='../Attendance/EmployeePLRequestWithTime.aspx' where Object_Id='254'";
            //    int a = objDA.execute_Command(strsqla);

            //    string strsqlb = "Update IT_ObjectEntry Set Page_Url='../Attendance/HR_PLRequestWithTime.aspx' where Object_Id='68'";
            //    int b = objDA.execute_Command(strsqlb);
            //}
            //else if (chkPLWithTimeWithOutTime.Checked == false)
            //{
            //    string strsqlc = "Update IT_ObjectEntry Set Page_Url='../Attendance/EmployeePLRequestWithOutTime.aspx' where Object_Id='254'";
            //    int c = objDA.execute_Command(strsqlc);

            //    string strsqld = "Update IT_ObjectEntry Set Page_Url='../Attendance/HR_PLRequestWithOutTime.aspx' where Object_Id='68'";
            //    int d = objDA.execute_Command(strsqld);
            //}
            if (rbtnCarryYes.Checked)
            {


                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Carry Forward Partial Leave Minutes", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Carry Forward Partial Leave Minutes", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }

            if (rbtnOtEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsOverTime", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsOverTime", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            }
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Max Over Time Min", txtMaxOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Min OVer Time Min", txtMinOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Over Time Calculation Method", ddlCalculationMethod.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            // Week Of OT Functionality
            if (rbnWeekOffOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WeekOffOTEnable", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WeekOffOTEnable", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            // Holiday OT Functionality
            if (rbnHoliayOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "HolidayOTEnable", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "HolidayOTEnable", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }

            //Overtime Approval
            if (rbnApprovalOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "OverTime Approval", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "OverTime Approval", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
        }
        else if (strParameterWorksOn == "Company")
        {
            if (rbtnPartialEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Leave_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Leave_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            if (txtTotalMinutes.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtTotalMinutes.Focus();
                return;
            }

            if (txtMinuteday.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtMinuteday.Focus();
                return;
            }

            if (txtMaxOTMint.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtMaxOTMint.Focus();
                return;
            }

            if (txtMinOTMint.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtMinOTMint.Focus();
                return;
            }


            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Total Partial Leave Minutes", txtTotalMinutes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial Leave Minute Use In A Day", txtMinuteday.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Violation_Min", txtViolation.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PLTimeWithOutTime", chkPLWithTimeWithOutTime.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PL Date Editable", chkPLDateEditable.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (rbtnCarryYes.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Carry Forward Partial Leave Minutes", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Carry Forward Partial Leave Minutes", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            if (rbtnOtEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsOverTime", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsOverTime", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            }
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Max Over Time Min", txtMaxOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Min OVer Time Min", txtMinOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Over Time Calculation Method", ddlCalculationMethod.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            // Week Of OT Functionality
            if (rbnWeekOffOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WeekOffOTEnable", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WeekOffOTEnable", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            // Holiday OT Functionality
            if (rbnHoliayOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "HolidayOTEnable", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "HolidayOTEnable", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            // Overtime Approval Functionality
            if (rbnApprovalOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "OverTime Approval", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "OverTime Approval", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

        SystemLog.SaveSystemLog("Company Parameter : OT/PL", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "OT/PL Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");

    }

    protected void btnCancelPartial_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");
    }
    protected void btnSaveSMSEmail_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        if (txtEmail.Text != "")
        {

            if (!IsValidEmail(txtEmail.Text))
            {
                DisplayMessage("Email is invalid");
                txtEmail.Text = "";
                txtEmail.Focus();
                return;
            }
        }
        if (txtPasswordReminder.Text == "")
        {
            txtPasswordReminder.Text = "0";
        }
        objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Password Reminder(In Days)", txtPasswordReminder.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        if (strParameterWorksOn == "Location")
        {
            if (rbtnSMSEnable.Checked)
            {

                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "SMS_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "SMS_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            }
            if (rbtnEmailEnable.Checked)
            {

                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Email_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Email_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            if (txtSenderId.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSenderId.Focus();
                return;
            }

            if (txtUserId.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtUserId.Focus();
                return;
            }

            if (txtSmsPassword.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSmsPassword.Focus();
                return;
            }

            if (txtEmail.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtEmail.Focus();
                return;
            }

            //if (txtPasswordEmail.Text == "")
            //{
            //    DisplayMessage("Field Cant Be Blank");
            //    txtPasswordEmail.Focus();
            //    return;
            //}

            if (txtSMTP.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSMTP.Focus();
                return;
            }

            if (txtPort.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtPort.Focus();
                return;
            }

            if (txtPop3.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtPop3.Focus();
                return;
            }

            if (txtpopport.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtpopport.Focus();
                return;
            }

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "ImageFileUploadSize", txtprmUploadSize.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "POSIntegrationWithPryce", chkIntegratedwithPOS.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "SMS_API", txtSMSAPI.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "SMS_User_Password", Common.Encrypt(txtSmsPassword.Text), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "SMS_User_Id", txtUserId.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sender_Id", txtSenderId.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email", txtEmail.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email_Password", Common.Encrypt(txtPasswordEmail.Text), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email_SMTP", txtSMTP.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email_Port", txtPort.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email_EnableSSL", chkEnableSSL.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email_Server_In", txtPop3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Master_Email_Port_In", txtpopport.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Send_Email_OnLeaveRequest", chkSendEmail.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        else if (strParameterWorksOn == "Company")
        {
            if (rbtnSMSEnable.Checked)
            {

                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "SMS_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "SMS_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            }
            if (rbtnEmailEnable.Checked)
            {

                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Email_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Email_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            if (txtSenderId.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSenderId.Focus();
                return;
            }

            if (txtUserId.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtUserId.Focus();
                return;
            }

            if (txtSmsPassword.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSmsPassword.Focus();
                return;
            }

            if (txtEmail.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtEmail.Focus();
                return;
            }

            if (txtPasswordEmail.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtPasswordEmail.Focus();
                return;
            }

            if (txtSMTP.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSMTP.Focus();
                return;
            }

            if (txtPort.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtPort.Focus();
                return;
            }

            if (txtPop3.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtPop3.Focus();
                return;
            }

            if (txtpopport.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtpopport.Focus();
                return;
            }
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "ImageFileUploadSize", txtprmUploadSize.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "POSIntegrationWithPryce", chkIntegratedwithPOS.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "SMS_API", txtSMSAPI.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "SMS_User_Password", Common.Encrypt(txtSmsPassword.Text), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "SMS_User_Id", txtUserId.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sender_Id", txtSenderId.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email", txtEmail.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email_Password", Common.Encrypt(txtPasswordEmail.Text), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email_SMTP", txtSMTP.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email_Port", txtPort.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email_EnableSSL", chkEnableSSL.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email_Server_In", txtPop3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Master_Email_Port_In", txtpopport.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Send_Email_OnLeaveRequest", chkSendEmail.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        SystemLog.SaveSystemLog("Company Parameter : SMS/Email", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "SMS/Email Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");
    }

    protected void btnHelp_Click(object sender, EventArgs e)
    {
        string url = "../Help.htm";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);

    }

    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void txtWorkPercentTo1_TextChanged(object sender, EventArgs e)
    {
        if (txtWorkPercentTo1.Text == "")
        {

            txtWorkPercentTo1.Text = "0";
        }

        if (Convert.ToDouble(txtWorkPercentTo1.Text) >= Convert.ToDouble(txtWorkPercentTo2.Text))
        {
            txtWorkPercentFrom2.Text = (Convert.ToDouble(txtWorkPercentTo1.Text) + 1).ToString();

            txtWorkPercentTo2.Text = (Convert.ToDouble(txtWorkPercentFrom2.Text) + 1).ToString();

            txtWorkPercentTo2_TextChanged(null, null);


        }
        else
        {

            txtWorkPercentFrom2.Text = (Convert.ToDouble(txtWorkPercentTo1.Text) + 1).ToString();
        }
    }
    protected void txtWorkPercentTo2_TextChanged(object sender, EventArgs e)
    {

        if (Convert.ToDouble(txtWorkPercentTo2.Text) < Convert.ToDouble(txtWorkPercentFrom2.Text))
        {

            DisplayMessage("Value Should be Greater than " + txtWorkPercentFrom2.Text + " ");


        }


        txtWorkPercentFrom3.Text = (Convert.ToDouble(txtWorkPercentTo2.Text) + 1).ToString();


    }



    protected void ddlPaySal_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlPaySal.SelectedValue == "Work Hour" || ddlPaySal.SelectedValue == "Work Calculation")
        {
            pnlWorkPercent.Visible = false;

        }
        else
        {
            pnlWorkPercent.Visible = true;
        }


    }
    protected void btnSaveWork_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        string strSalIncStatus = string.Empty;

        if (txtFixedDays.Text == "")
        {
            DisplayMessage("Field Cant Be Blank");
            txtFixedDays.Focus();
            return;
        }

        if (txtEmpPF.Text == "")
        {
            DisplayMessage("Field Cant Be Blank");
            txtEmpPF.Focus();
            return;
        }

        if (txtEmployerPf.Text == "")
        {
            DisplayMessage("Field Cant Be Blank");
            txtEmployerPf.Focus();
            return;
        }

        if (txtEmpESIC.Text == "")
        {
            DisplayMessage("Field Cant Be Blank");
            txtEmpESIC.Focus();
            return;
        }

        if (rbSalIncEnable.Checked)
        {
            if (txtSalIncrDuration.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSalIncrDuration.Focus();
                return;
            }
            if (txtSalIncrDurationForExperience.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtSalIncrDurationForExperience.Focus();
                return;
            }
            if (txtFreshPerFrom.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtFreshPerFrom.Focus();
                return;
            }
            if (txtFreshPerTo.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtFreshPerTo.Focus();
                return;
            }
            if (txtExpPerFrom.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtExpPerFrom.Focus();
                return;
            }
            if (txtExpPerTo.Text == "")
            {
                DisplayMessage("Field Cant Be Blank");
                txtExpPerTo.Focus();
                return;
            }
            strSalIncStatus = "True";
        }
        else if (rbSalIncDisable.Checked)
        {
            txtSalIncrDuration.Text = "0";
            txtSalIncrDurationForExperience.Text = "0";
            txtFreshPerFrom.Text = "0";
            txtFreshPerTo.Text = "0";
            txtExpPerFrom.Text = "0";
            txtExpPerTo.Text = "0";
            strSalIncStatus = "False";
        }


        if (strParameterWorksOn == "Location")
        {
            //Add New On 25-07-2015
            PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "PF_Applicable_Salary", txtPfApplicablesalary.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "PF_Admin_Charges", txtAdminCharges.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "PF_EDLI", txtPfEdLi.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "PF_Inspection_Charges", txtpfInspectionCharges.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "ESIC_Applicable_Salary", txtESicApplicablesalary.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Shift_Approval_Functionality", chkShiftApproval.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Effective Work Calculation Method", ddlWorkCal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Pay Salary Acc To Work Hour or Ref Hour", ddlPaySal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Without Shift Preference", ddlShiftPref.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Salary Calculate According To", ddlSalCal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Days In Month", txtFixedDays.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            //Add New On 26-06-2015
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "For Work Hour", chkForWorkHour.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "LateEarlyWithoutPresent", chkLateWithoutPresent.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "AddWeekOffInShift", chkWeekOffInShift.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //End

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WorkPercentTo1", txtWorkPercentTo1.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WorkPercentTo2", txtWorkPercentTo2.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WorkPercentTo3", txtWorkPercentTo3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WorkPercentFrom1", txtWorkPercentFrom1.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WorkPercentFrom2", txtWorkPercentFrom2.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WorkPercentFrom3", txtWorkPercentFrom3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Value1", txtValue1.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Value2", txtValue2.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Value3", txtValue3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());



            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Employee_PF", txtEmpPF.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Employer_PF", txtEmployerPf.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Employer_FPF", txtEmployerFPF.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Employee_ESIC", txtEmpESIC.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Employer_ESIC", txtEmployerESIC.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            //
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Salary_Increment_Enable", strSalIncStatus, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sal_Increment_Duration", txtSalIncrDuration.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sal_Increment_Duration For Experience", txtSalIncrDurationForExperience.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sal_Increment_Per_Fresher_From", txtFreshPerFrom.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sal_Increment_Per_Fresher_To", txtFreshPerTo.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sal_Increment_Per_Experience_From", txtExpPerFrom.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Sal_Increment_Per_Experience_To", txtExpPerTo.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        else if (strParameterWorksOn == "Company")
        {
            //Add New On 25-07-2015
            PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PLTimeWithOutTime", chkPLWithTimeWithOutTime.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PL Date Editable", chkPLDateEditable.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            //if (chkPLWithTimeWithOutTime.Checked == true)
            //{
            //    string strsqla = "Update IT_ObjectEntry Set Page_Url='../Attendance/EmployeePLRequestWithTime.aspx' where Object_Id='254'";
            //    int a = objDA.execute_Command(strsqla);

            //    string strsqlb = "Update IT_ObjectEntry Set Page_Url='../Attendance/HR_PLRequestWithTime.aspx' where Object_Id='68'";
            //    int b = objDA.execute_Command(strsqlb);
            //}
            //else if (chkPLWithTimeWithOutTime.Checked == false)
            //{
            //    string strsqlc = "Update IT_ObjectEntry Set Page_Url='../Attendance/EmployeePLRequestWithOutTime.aspx' where Object_Id='254'";
            //    int c = objDA.execute_Command(strsqlc);

            //    string strsqld = "Update IT_ObjectEntry Set Page_Url='../Attendance/HR_PLRequestWithOutTime.aspx' where Object_Id='68'";
            //    int d = objDA.execute_Command(strsqld);
            //}

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Effective Work Calculation Method", ddlWorkCal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Pay Salary Acc To Work Hour or Ref Hour", ddlPaySal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Without Shift Preference", ddlShiftPref.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Salary Calculate According To", ddlSalCal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Days In Month", txtFixedDays.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            //Add New On 26-06-2015
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "For Work Hour", chkForWorkHour.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "LateEarlyWithoutPresent", chkLateWithoutPresent.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "AddWeekOffInShift", chkWeekOffInShift.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Shift_Approval_Functionality", chkShiftApproval.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //End

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WorkPercentTo1", txtWorkPercentTo1.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WorkPercentTo2", txtWorkPercentTo2.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WorkPercentTo3", txtWorkPercentTo3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WorkPercentFrom1", txtWorkPercentFrom1.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WorkPercentFrom2", txtWorkPercentFrom2.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WorkPercentFrom3", txtWorkPercentFrom3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Value1", txtValue1.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Value2", txtValue2.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Value3", txtValue3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());



            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Employee_PF", txtEmpPF.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Employer_PF", txtEmployerPf.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Employee_ESIC", txtEmpESIC.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Employer_ESIC", txtEmployerESIC.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            //
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Salary_Increment_Enable", strSalIncStatus, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sal_Increment_Duration", txtSalIncrDuration.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sal_Increment_Duration For Experience", txtSalIncrDurationForExperience.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sal_Increment_Per_Fresher_From", txtFreshPerFrom.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sal_Increment_Per_Fresher_To", txtFreshPerTo.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sal_Increment_Per_Experience_From", txtExpPerFrom.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Sal_Increment_Per_Experience_To", txtExpPerTo.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());




            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PF_Applicable_Salary", txtPfApplicablesalary.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PF_Admin_Charges", txtAdminCharges.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PF_EDLI", txtPfEdLi.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "PF_Inspection_Charges", txtpfInspectionCharges.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "ESIC_Applicable_Salary", txtESicApplicablesalary.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Employer_FPF", txtEmployerFPF.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        }

        SystemLog.SaveSystemLog("Company Parameter : Work Calculation", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Work Calculation Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");

        //25-07-2015
        //Session.Abandon();
        //Session.Clear();
        //Response.Redirect("~/ERPLogin.aspx");
    }
    protected void btnCancelWork_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");

    }
    protected void btnSavePanelty_Click(object sender, EventArgs e)
    {



        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        if (rbtLateInEnable.Checked)
        {
            if (rbtnLateSalary.Checked)
            {
                if (txtLateRelaxMin.Text == "")
                {
                    DisplayMessage("Enter late Relaxation minute");
                    txtLateRelaxMin.Focus();
                    return;

                }
                if (txtLateCount.Text == "")
                {
                    DisplayMessage("Enter late count");
                    txtLateCount.Focus();
                    return;


                }
                if (txtLateValue.Text == "")
                {
                    DisplayMessage("Enter late value");
                    txtLateValue.Focus();
                    return;


                }
            }
            else
            {
                if (txtLateRelaxMinWithMTimes.Text == "")
                {
                    DisplayMessage("Enter late Relaxation minute");
                    txtLateRelaxMinWithMTimes.Focus();
                    return;

                }
            }

        }


        if (rbtEarlyOutEnable.Checked)
        {
            if (rbtnEarlySalary.Checked)
            {
                if (txtEarlyRelaxMin.Text == "")
                {
                    DisplayMessage("Enter Early Relaxation minute");
                    txtEarlyRelaxMin.Focus();
                    return;

                }
                if (txtEarlyCount.Text == "")
                {
                    DisplayMessage("Enter Early count");
                    txtEarlyCount.Focus();
                    return;


                }
                if (txtEarlyValue.Text == "")
                {
                    DisplayMessage("Enter Early value");
                    txtEarlyValue.Focus();
                    return;


                }
            }
            else
            {
                if (txtEarlyRelaxMinWithMinTimes.Text == "")
                {
                    DisplayMessage("Enter Early Relaxation minute");
                    txtEarlyRelaxMinWithMinTimes.Focus();
                    return;

                }
            }

        }


        if (txtAbsentDeduc.Text == "")
        {
            DisplayMessage("Enter absent deduction value");
            txtAbsentDeduc.Focus();
            return;

        }
        if (rbtnEarlySalary.Checked)
        {


            //if (txtPartialValue.Text == "")
            //{
            //    DisplayMessage("Enter partial value");
            //    txtPartialValue.Focus();
            //    return;

            //}

            if (txtViolation.Text == "")
            {
                DisplayMessage("Enter partial violation value");
                txtViolation.Focus();
                return;

            }
        }
        else
        {
            if (txtViolation.Text == "")
            {
                DisplayMessage("Enter partial violation value");
                txtViolation.Focus();
                return;

            }
        }


        if (strParameterWorksOn == "Location")
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsworkMinute_In_Penalty", chkWorkMinutePenalty.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsbreakMinute_In_Penalty", chkbreakMinutePenalty.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsrelaxationMinute_In_Penalty", chkRelaxationMinutePenalty.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());



            if (rbtLateInDisable.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Late_Penalty", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Late_Penalty", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (rbtnLateSalary.Checked)
                {
                    // Updated By Nitin Jain On 07-08-2014 
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "LateIn_MinuteDeduction_Type", ddlDeductionMinuteForDay.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "EarlyOut_MinuteDeduction_Type", ddlDeductionMinuteForDay.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Late_Penalty_Method", "Salary", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Late_Relaxation_Min", txtLateRelaxMin.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Late_Occurence", txtLateCount.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Late_Penalty_Salary_Type", ddlLateType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Late_Penalty_Salary_Value", txtLateValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


                }
                else
                {
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Late_Penalty_Method", "Min", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Late_Penalty_Min_Deduct", ddlLateMinTime.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Late_Relaxation_Min", txtLateRelaxMinWithMTimes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


                }

            }

            if (rbtEarlyOutDisable.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Early_Penalty", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Early_Penalty", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (rbtnEarlySalary.Checked)
                {
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Early_Penalty_Method", "Salary", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Early_Relaxation_Min", txtEarlyRelaxMin.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Early_Occurence", txtEarlyCount.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Early_Penalty_Salary_Type", ddlEarlyType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Early_Penalty_Salary_Value", txtEarlyValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


                }
                else
                {
                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_Early_Penalty_Method", "Min", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Early_Penalty_Min_Deduct", ddlEarlyMinTime.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                    objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Early_Relaxation_Min", txtEarlyRelaxMinWithMinTimes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                }

            }

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Absent_Type", ddlAbsentType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Absent_Value", txtAbsentDeduc.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());





            if (rbtnPartialSalary.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Penalty_Method", "Salary", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Penalty_Salary_Type", ddlPartialType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Penalty_Salary_Value", txtPartialValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Penalty_Method", "Min", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Penalty_Min_Deduct", ddlPartialMinTime.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Penalty_Salary_Value", txtPartialValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            }


            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial_Violation_Min", txtViolation.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "No_Clock_In_CountAsAbsent", chkNoClockIn.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "No_Clock_Out_CountAsAbsent", chkNoClockOut.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            if (ChkNoClockLate.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsNoClockInLate", ChkNoClockLate.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NoClockInLateMin", txtNoClockLate.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsNoClockInLate", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NoClockInLateMin", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                txtNoClockLate.Text = string.Empty;
            }

            if (ChkNoClockEarly.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsNoClockOutEarly", ChkNoClockEarly.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NoClockOutEarlyMin", txtNoClockEarly.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsNoClockOutEarly", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NoClockOutEarlyMin", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                txtNoClockEarly.Text = string.Empty;
            }

            if (chkAfterEInAbsent.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "AfterEndingInAbsent", chkAfterEInAbsent.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "AfterEndingInAbsent", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }

            // Nitin Jain on 05/01/2015 Late In Count As Half Day or Not......
            if (chkLateInCountAsHalfDay.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "LateInCountAsHalfDay", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "LateInCountAsHalfDay", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
        }
        else if (strParameterWorksOn == "Company")
        {


            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsworkMinute_In_Penalty", chkWorkMinutePenalty.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsbreakMinute_In_Penalty", chkbreakMinutePenalty.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsrelaxationMinute_In_Penalty", chkRelaxationMinutePenalty.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());



            if (rbtLateInDisable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Late_Penalty", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Late_Penalty", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                if (rbtnLateSalary.Checked)
                {
                    // Updated By Nitin Jain On 07-08-2014 
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "LateIn_MinuteDeduction_Type", ddlDeductionMinuteForDay.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "EarlyOut_MinuteDeduction_Type", ddlDeductionMinuteForDay.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Late_Penalty_Method", "Salary", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Late_Relaxation_Min", txtLateRelaxMin.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Late_Occurence", txtLateCount.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Late_Penalty_Salary_Type", ddlLateType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Late_Penalty_Salary_Value", txtLateValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                }
                else
                {
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Late_Penalty_Method", "Min", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Late_Penalty_Min_Deduct", ddlLateMinTime.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Late_Relaxation_Min", txtLateRelaxMinWithMTimes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                }

            }

            if (rbtEarlyOutDisable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Early_Penalty", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Early_Penalty", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                if (rbtnEarlySalary.Checked)
                {
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Early_Penalty_Method", "Salary", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Early_Relaxation_Min", txtEarlyRelaxMin.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Early_Occurence", txtEarlyCount.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Early_Penalty_Salary_Type", ddlEarlyType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Early_Penalty_Salary_Value", txtEarlyValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                }
                else
                {
                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_Early_Penalty_Method", "Min", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Early_Penalty_Min_Deduct", ddlEarlyMinTime.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Early_Relaxation_Min", txtEarlyRelaxMinWithMinTimes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                }

            }

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Absent_Type", ddlAbsentType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Absent_Value", txtAbsentDeduc.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (rbtnPartialSalary.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Penalty_Method", "Salary", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Penalty_Salary_Type", ddlPartialType.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Penalty_Salary_Value", txtPartialValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Penalty_Method", "Min", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Penalty_Min_Deduct", ddlPartialMinTime.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Penalty_Salary_Value", txtPartialValue.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial_Violation_Min", txtViolation.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "No_Clock_In_CountAsAbsent", chkNoClockIn.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "No_Clock_Out_CountAsAbsent", chkNoClockOut.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (ChkNoClockLate.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsNoClockInLate", ChkNoClockLate.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NoClockInLateMin", txtNoClockLate.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsNoClockInLate", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NoClockInLateMin", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                txtNoClockLate.Text = string.Empty;
            }

            if (ChkNoClockEarly.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsNoClockOutEarly", ChkNoClockEarly.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NoClockOutEarlyMin", txtNoClockEarly.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsNoClockOutEarly", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NoClockOutEarlyMin", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                txtNoClockEarly.Text = string.Empty;
            }

            if (chkAfterEInAbsent.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "AfterEndingInAbsent", chkAfterEInAbsent.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "AfterEndingInAbsent", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            // Nitin Jain on 05/01/2015 Late In Count As Half Day or Not......
            if (chkLateInCountAsHalfDay.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "LateInCountAsHalfDay", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "LateInCountAsHalfDay", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        SystemLog.SaveSystemLog("Company Parameter : Penalty", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Penalty Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");


    }
    protected void btnCancelPanelty_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");

    }
    protected void btnRCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");

    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave1.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        PnlTime.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlColor.Visible = false;
        pnlPaneltyCalc.Visible = false;
        pnlList.Visible = false;
        panelHR.Visible = false;
        pnlReportView.Visible = true;
        return;
    }
    protected void btnSaveColorCode_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }

        if (strParameterWorksOn == "Location")
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Absnet_Color_Code", txtAbsentColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Leave_Color_Code", txtLeaveColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Present_Color_Code", txtPresentColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Holiday_Color_Code", txtHolidayColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Late_Color_Code", txtLateColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Early_Color_Code", txtEarlyColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WeekOff_Color_Code", txtWeekOffColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "TempShift_Color_Code", txtTempShiftColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "MI_Color_Code", txtMIcolorcode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "NR_Color_Code", txtNRColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "MO_Color_Code", txtMOcolorcode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());



        }
        else if (strParameterWorksOn == "Company")
        {
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Absnet_Color_Code", txtAbsentColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Leave_Color_Code", txtLeaveColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Present_Color_Code", txtPresentColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Holiday_Color_Code", txtHolidayColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Late_Color_Code", txtLateColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Early_Color_Code", txtEarlyColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WeekOff_Color_Code", txtWeekOffColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "TempShift_Color_Code", txtTempShiftColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "MI_Color_Code", txtMIcolorcode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "NR_Color_Code", txtNRColorCode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "MO_Color_Code", txtMOcolorcode.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }
        SystemLog.SaveSystemLog("Company Parameter : Color Code", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Color Code Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");

    }

    protected void btnCancelColorCode_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");

    }
    protected void btnSaveKeyPreference_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        if (strParameterWorksOn == "Location")
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "In Func Key", txtInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Out Func Key", txtOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Break In Func Key", txtBreakInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Break Out Func Key", txtBreakOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial Leave In  Func Key", txtPartialInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Partial Leave Out  Func Key", txtPartialOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Shift Range", ddlShiftRange.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            if (rbtnKeyEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "With Key Preference", "Yes", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "With Key Preference", "No", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            if (ChkNextDayLog.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Consider Next Day Log", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Consider Next Day Log", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            }
        }
        else if (strParameterWorksOn == "Company")
        {
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "In Func Key", txtInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Out Func Key", txtOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Break In Func Key", txtBreakInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Break Out Func Key", txtBreakOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial Leave In  Func Key", txtPartialInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Partial Leave Out  Func Key", txtPartialOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Shift Range", ddlShiftRange.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            if (rbtnKeyEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "With Key Preference", "Yes", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "With Key Preference", "No", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            if (ChkNextDayLog.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Consider Next Day Log", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Consider Next Day Log", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }
        SystemLog.SaveSystemLog("Company Parameter : Keys", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Keys Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");
    }

    protected void btnCancelKeyPreference_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");
    }

    protected void btnSaveTime_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strHolidayValue = string.Empty;
        string strApplicableAllowance = string.Empty;
        string LsMonthDays = string.Empty;
        string LsPayScale = string.Empty;
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }


        if (txtShortestTime.Text == "")
        {
            DisplayMessage("Enter Shortest Time");
            txtShortestTime.Focus();
            return;
        }

        if (txtWorkDayMin.Text == "")
        {
            DisplayMessage("Enter Shortest Time");
            txtWorkDayMin.Focus();
            return;
        }

        if (txtServiceRunTime.Text == "")
        {
            DisplayMessage("Enter Shortest Time");
            txtServiceRunTime.Focus();
            return;
        }

        if (txtHalfDayCount.Text == "")
        {
            DisplayMessage("Enter Shortest Time");
            txtHalfDayCount.Focus();
            return;
        }

        if (ddlHolidayValidity.SelectedIndex > 0 && txtHolidayValue.Text.Trim() == "")
        {
            DisplayMessage("Enter Holiday Validity value");
            txtHolidayValue.Focus();
            return;
        }

        if (ddlHolidayValidity.SelectedIndex == 0)
        {
            strHolidayValue = "0";
        }
        else
        {
            strHolidayValue = ddlHolidayValidity.SelectedValue.Trim() + "-" + txtHolidayValue.Text;
        }

        if (txtLsApplicableAllowances.Text != "")
        {
            foreach (string str in txtLsApplicableAllowances.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (!txtLsApplicableAllowances.Text.Trim().Split(',').Contains(ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows[0]["Allowance_Id"].ToString()))
                {
                    strApplicableAllowance += ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows[0]["Allowance_Id"].ToString() + ",";
                }

            }
        }
        else
        {
            strApplicableAllowance = "0";
        }


        if (ddlLSDaysCount.SelectedIndex == 0)
        {

            if (txtLsFixedDays.Text == "")
            {
                DisplayMessage("Enter Fixed Days for Leave salary Calculation");
                txtLsFixedDays.Focus();
                return;
            }


            LsMonthDays = txtLsFixedDays.Text;
        }
        else
        {
            LsMonthDays = "Month Days";
        }



        if (rbtnCurrentSalary.Checked)
        {
            LsPayScale = "Current";
        }
        else if (rbtnactualSalary.Checked)
        {
            LsPayScale = "Actual";
        }

        if (strParameterWorksOn == "Location")
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Absent_sandwich_on_week_Off", chkweekoffsandwich.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Absent_sandwich_on_holiday", chkholidaysandwich.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Leave_Approval_Functionality", chkLeaveApproval.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Leave_Transaction_on_uploading", chkLeaveValidation.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsPaidLeaveForLeaveSalary", chkPaidLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsUnPaidLeaveForLeaveSalary", chkUnPaidLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "LS_ApplicableAllowance", strApplicableAllowance, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "LS_Month_Days_Count", LsMonthDays, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Ls_Pay_Scale", LsPayScale, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Send_Full_Report_On_Month_End", chkNotification.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "EmployeeCode", chkAutoGenerateEmployeeCode.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsWeekOffLeaveSalary", chkWeekoffLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsHolidayLeaveSalary", chkHolidayLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsAbsentLeaveSalary", chkAbsentLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WithShiftWithoutFunctionKey", chkShiftFunctionKey.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsAddNewUserDownload", chkNewUserDownload.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsAddNewUserUpload", chkNewUserUpload.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsLeaveSalary", chkIsLeaveSalary.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Is_ManualLog_OnLeave", chkIsManualLog.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Logs_priority_OnLeave", chkLogpriorityonLeave.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Half_Day_Count", txtHalfDayCount.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Leave_Count_On_WeekOff", chkLeaveCountForWeekOff.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Leave_Count_On_Holiday", chkLeaveCountForHoliday.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "ServiceInterval", txtDownloadServicePath.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (ChkDbLocation.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsDefault_DbLocation", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsDefault_DbLocation", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Work Day Min", txtWorkDayMin.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Shortest Time Table", txtShortestTime.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Holiday_Validity", strHolidayValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            // objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Min Difference Between TimeTable in Shift", txtMinDiffTime.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Service_Run_Time", txtServiceRunTime.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            if (rbtnBrandNo.Checked)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Display TimeTable In All Brand", rbtnBrandNo.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Display TimeTable In All Brand", rbtnBrandYes.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }

            int flag = 0;
            string WeekOffDays = string.Empty;
            for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
            {
                if (ChkWeekOffList.Items[i].Selected == true)
                {

                    flag = 1;
                    if (WeekOffDays == "")
                    {
                        WeekOffDays = ChkWeekOffList.Items[i].Text;
                    }
                    else
                    {
                        WeekOffDays = WeekOffDays + "," + ChkWeekOffList.Items[i].Text;
                    }
                }
            }

            if (flag == 0)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Week Off Days", "No", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Week Off Days", WeekOffDays, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Exclude Day As Absent or IsOff", ddlExculeDay.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "FinancialYearStartMonth", ddlFinancialYear.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Employee Synchronization", ddlEmpSync.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Send_Email_OnDeviceStatusAlert", txtEmailAlertDevice.Text.Trim(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (ddlDefaultShift.SelectedIndex != 0)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Default_Shift", ddlDefaultShift.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            }
            else
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Default_Shift", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            }
            if (txtBackupLoc.Text != null && txtBackupLoc.Text != string.Empty)
            {
                objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Backup_Restore_Location", txtBackupLoc.Text.Trim(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            else
            {
                DisplayMessage("Select Backup Restore Location");
                txtBackupLoc.Focus();
                return;
            }
        }
        else if (strParameterWorksOn == "Company")
        {
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Absent_sandwich_on_week_Off", chkweekoffsandwich.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Absent_sandwich_on_holiday", chkholidaysandwich.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Leave_Approval_Functionality", chkLeaveApproval.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Leave_Transaction_on_uploading", chkLeaveValidation.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Send_Email_OnDeviceStatusAlert", txtEmailAlertDevice.Text.Trim(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsPaidLeaveForLeaveSalary", chkPaidLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsUnPaidLeaveForLeaveSalary", chkUnPaidLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "LS_ApplicableAllowance", strApplicableAllowance, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "LS_Month_Days_Count", LsMonthDays, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Ls_Pay_Scale", LsPayScale, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Holiday_Validity", strHolidayValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Send_Full_Report_On_Month_End", chkNotification.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "EmployeeCode", chkAutoGenerateEmployeeCode.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsWeekOffLeaveSalary", chkWeekoffLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsHolidayLeaveSalary", chkHolidayLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsAbsentLeaveSalary", chkAbsentLeaveSal.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WithShiftWithoutFunctionKey", chkShiftFunctionKey.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsAddNewUserDownload", chkNewUserDownload.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsAddNewUserUpload", chkNewUserUpload.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsLeaveSalary", chkIsLeaveSalary.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Is_ManualLog_OnLeave", chkIsManualLog.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Logs_priority_OnLeave", chkLogpriorityonLeave.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Half_Day_Count", txtHalfDayCount.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Leave_Count_On_WeekOff", chkLeaveCountForWeekOff.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Leave_Count_On_Holiday", chkLeaveCountForHoliday.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "ServiceInterval", txtDownloadServicePath.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (ChkDbLocation.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsDefault_DbLocation", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "IsDefault_DbLocation", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Work Day Min", txtWorkDayMin.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Shortest Time Table", txtShortestTime.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            // objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Min Difference Between TimeTable in Shift", txtMinDiffTime.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Service_Run_Time", txtServiceRunTime.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (rbtnBrandNo.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Display TimeTable In All Brand", rbtnBrandNo.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Display TimeTable In All Brand", rbtnBrandYes.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            int flag = 0;
            string WeekOffDays = string.Empty;
            for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
            {
                if (ChkWeekOffList.Items[i].Selected == true)
                {

                    flag = 1;
                    if (WeekOffDays == "")
                    {
                        WeekOffDays = ChkWeekOffList.Items[i].Text;
                    }
                    else
                    {
                        WeekOffDays = WeekOffDays + "," + ChkWeekOffList.Items[i].Text;
                    }
                }
            }

            if (flag == 0)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Week Off Days", "No", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Week Off Days", WeekOffDays, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Exclude Day As Absent or IsOff", ddlExculeDay.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "FinancialYearStartMonth", ddlFinancialYear.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Employee Synchronization", ddlEmpSync.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            if (ddlDefaultShift.SelectedIndex != 0)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Default_Shift", ddlDefaultShift.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Default_Shift", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            if (txtBackupLoc.Text != null && txtBackupLoc.Text != string.Empty)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Backup_Restore_Location", txtBackupLoc.Text.Trim(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                DisplayMessage("Select Backup Restore Location");
                txtBackupLoc.Focus();
                return;
            }
        }


        SystemLog.SaveSystemLog("Company Parameter : Time Table", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Time Table Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");


    }

    protected void btnCancelTime_Click(object sender, EventArgs e)
    {

        Response.Redirect("../MasterSetup/CompanyMaster.aspx");
    }




    protected void btnCancelSMSEmail_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");

    }


    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    protected void btnSaveOverTime_Click(object sender, EventArgs e)
    {
        if (rbtnOtEnable.Checked)
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsOverTime", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        }
        else
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "IsOverTime", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        }
        objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Max Over Time Min", txtMaxOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Min OVer Time Min", txtMinOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Over Time Calculation Method", ddlCalculationMethod.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        SystemLog.SaveSystemLog("Company Parameter : OverTime", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "OverTime Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");

    }
    protected void rbtKeyPref_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnKeyEnable.Checked)
        {
            txtInKey.Enabled = true;
            txtOutKey.Enabled = true;
            txtPartialInKey.Enabled = true;
            txtPartialOutKey.Enabled = true;
            txtBreakInKey.Enabled = true;
            txtBreakOutKey.Enabled = true;
            ChkNextDayLog.Enabled = true;
            ddlShiftRange.Enabled = true;
        }
        else
        {
            txtInKey.Enabled = false;
            txtOutKey.Enabled = false;
            txtPartialInKey.Enabled = false;
            txtPartialOutKey.Enabled = false;
            txtBreakInKey.Enabled = false;
            txtBreakOutKey.Enabled = false;
            ChkNextDayLog.Enabled = false;
            ddlShiftRange.Enabled = false;

        }

    }
    protected void rbtSMS_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnSMSEnable.Checked)
        {

            txtSMSAPI.Enabled = true;
            txtSmsPassword.Enabled = true;
            txtSenderId.Enabled = true;
            txtUserId.Enabled = true;


        }
        else
        {
            txtSMSAPI.Enabled = false;
            txtSmsPassword.Enabled = false;
            txtSenderId.Enabled = false;
            txtUserId.Enabled = false;


        }
    }
    protected void rbtEmail_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnEmailEnable.Checked)
        {
            txtEmail.Enabled = true;
            txtPasswordEmail.Enabled = true;
            txtSMTP.Enabled = true;
            txtPort.Enabled = true;
            chkEnableSSL.Enabled = true;
        }
        else
        {

            txtEmail.Enabled = false;
            txtPasswordEmail.Enabled = false;
            txtSMTP.Enabled = false;
            txtPort.Enabled = false;
            chkEnableSSL.Enabled = false;
        }



    }


    protected void rbtPartialType_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPartialSalary.Checked)
        {
            pnlPartialMin.Visible = false;
            pnlPartialSal.Visible = true;

        }
        else
        {
            pnlPartialMin.Visible = true;
            pnlPartialSal.Visible = false;
        }


    }
    protected void rbtLateType_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnLateSalary.Checked)
        {
            pnlLateMin.Visible = false;
            pnlLateSal.Visible = true;
        }
        else
        {
            pnlLateMin.Visible = true;
            pnlLateSal.Visible = false;
        }
    }
    protected void rbtEarlyType_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnEarlySalary.Checked)
        {
            pnlEarlyMin.Visible = false;
            pnlEarlySal.Visible = true;
        }
        else
        {
            pnlEarlyMin.Visible = true;
            pnlEarlySal.Visible = false;
        }
    }
    protected void rbtLateIn_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtLateInEnable.Checked)
        {
            rbtnLateMinutes.Enabled = true;
            rbtnLateSalary.Enabled = true;
            txtLateRelaxMin.Enabled = true;
            txtLateCount.Enabled = true;
            txtLateValue.Enabled = true;
            ddlLateType.Enabled = true;
            ddlLateMinTime.Enabled = true;

        }
        else
        {
            rbtnLateMinutes.Enabled = false;
            rbtnLateSalary.Enabled = false;
            txtLateRelaxMin.Enabled = false;
            txtLateCount.Enabled = false;
            txtLateValue.Enabled = false;
            ddlLateType.Enabled = false;
            ddlLateMinTime.Enabled = false;
        }
    }
    protected void rbtEarlyOut_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtEarlyOutEnable.Checked)
        {
            rbtnEarlyMinutes.Enabled = true;
            rbtnEarlySalary.Enabled = true;
            txtEarlyRelaxMin.Enabled = true;
            txtEarlyCount.Enabled = true;
            txtEarlyValue.Enabled = true;
            ddlEarlyType.Enabled = true;
            ddlEarlyMinTime.Enabled = true;
        }
        else
        {
            rbtnEarlyMinutes.Enabled = false;
            rbtnEarlySalary.Enabled = false;
            txtEarlyRelaxMin.Enabled = false;
            txtEarlyCount.Enabled = false;
            txtEarlyValue.Enabled = false;
            ddlEarlyType.Enabled = false;
            ddlEarlyMinTime.Enabled = false;
        }
    }
    protected void rbtOT_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnOtDisable.Checked)
        {
            rbtnOtDisable.Checked = true;
            txtMaxOTMint.Enabled = false;
            txtMinOTMint.Enabled = false;
            ddlCalculationMethod.Enabled = false;
            rbnWeekOffOTDisable.Checked = true;
            rbnHoliayOTDisable.Checked = true;
            rbnWeekOffOTEnable.Checked = false;
            rbnHoliayOTEnable.Checked = false;
            rbnApprovalOTEnable.Checked = false;
            rbnApprovalOTDisable.Checked = true;

            rbnApprovalOTEnable.Enabled = false;
            rbnApprovalOTDisable.Enabled = false;
        }
        else
        {
            rbtnOtEnable.Checked = true;
            txtMaxOTMint.Enabled = true;
            txtMinOTMint.Enabled = true;
            ddlCalculationMethod.Enabled = true;
            rbnWeekOffOTDisable.Checked = false;
            rbnHoliayOTDisable.Checked = false;
            rbnWeekOffOTEnable.Checked = true;
            rbnHoliayOTEnable.Checked = true;
            rbnApprovalOTDisable.Checked = false;
            rbnApprovalOTEnable.Checked = true;

            rbnApprovalOTEnable.Enabled = true;
            rbnApprovalOTDisable.Enabled = true;

            rbnWeekOffOTDisable.Enabled = true;
            rbnHoliayOTDisable.Enabled = true;
            rbnWeekOffOTEnable.Enabled = true;
            rbnHoliayOTEnable.Enabled = true;
        }
    }
    protected void btnCancelOverTime_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");
    }
    //protected void ChkWeekOffList_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    string value = string.Empty;

    //    string result = Request.Form["__EVENTTARGET"];

    //    string[] checkedBox = result.Split('$'); ;

    //    int index = int.Parse(checkedBox[checkedBox.Length - 1]);

    //    //for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
    //    //{
    //    //    ChkWeekOffList.Items[i].Selected = false;
    //    //}
    //    if (ChkWeekOffList.Items[index].Selected == true)
    //    {

    //        ChkWeekOffList.Items[index].Selected = true;
    //    }
    //    else
    //    {
    //        ChkWeekOffList.Items[index].Selected = false;
    //    }
    //}
    protected void ddlSalCal_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSalCal.SelectedValue == "Monthly")
        {
            txtFixedDays.Visible = false;
            lblDay.Visible = false;
        }
        else
        {
            txtFixedDays.Visible = true;
            lblDay.Visible = true;
        }
    }
    protected void btnKeyPref_Click(object sender, EventArgs e)
    {
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlReportView.Visible = false;
        pnlPaneltyCalc.Visible = false;
        pnlPartialLeave1.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        PnlTime.Visible = false;
        pnlKeyPreference.Visible = true;
        pnlSMSEmail.Visible = false;
        pnlColor.Visible = false;
        pnlList.Visible = false;
    }

    // Report

    protected void btnReports_Click(object sender, EventArgs e)
    {
        //Add On 08-08-2015
        string strParameterWorksOn = string.Empty;
        DataTable dtParameterWorksOn = objAppParam.GetApplicationParameterByCompanyId("For Work Hour", Session["CompId"].ToString());
        dtParameterWorksOn = new DataView(dtParameterWorksOn, "Param_Name='ParameterWorksOn'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtParameterWorksOn.Rows.Count > 0)
        {
            strParameterWorksOn = dtParameterWorksOn.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strParameterWorksOn = "Location";
        }

        if (ddlParameterLevel.SelectedValue == "Company")
        {
            strParameterWorksOn = "Company";
        }

        if (strParameterWorksOn == "Location")
        {
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "WeekOff_Report", ChkWeekOff.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Holiday_Report", ChkHoliday.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Leave_Report", ChkLeave.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "For Log Report", ddlForLogReport.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "EmployeeCode", chkAutoGenerateEmployeeCode.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "ForManualAttendance", chkManualAttendanceVerified.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Holiday Assign On Week Off", chkHolidayAssignOnWeekOff.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "Page Level", ddlPageLevel.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(hdnCompanyId.Value, "ParameterWorksOn", ddlParameterLevel.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        else if (strParameterWorksOn == "Company")
        {
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "WeekOff_Report", ChkWeekOff.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Holiday_Report", ChkHoliday.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Leave_Report", ChkLeave.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "For Log Report", ddlForLogReport.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "EmployeeCode", chkAutoGenerateEmployeeCode.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "ForManualAttendance", chkManualAttendanceVerified.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Holiday Assign On Week Off", chkHolidayAssignOnWeekOff.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "Page Level", ddlPageLevel.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(hdnCompanyId.Value, "ParameterWorksOn", ddlParameterLevel.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }


        SystemLog.SaveSystemLog("Company Parameter : Report", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Report Parameter Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        DisplayMessage("Record Updated", "green");
    }

    protected void btnPanelty_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave1.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        PnlTime.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlColor.Visible = false;
        pnlPaneltyCalc.Visible = true;
        pnlList.Visible = false;
        panelHR.Visible = false;
        txtLateRelaxMin.Focus();
        pnlReportView.Visible = false;
        return;
    }
    protected void btnColorCode_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPaneltyCalc.Visible = false;
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlReportView.Visible = false;
        pnlPartialLeave1.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        PnlTime.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlColor.Visible = true;
        pnlList.Visible = false;
        panelHR.Visible = false;
        txtPresentColorCode.Focus();
        return;
    }
    protected void btnPartialLeave_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPaneltyCalc.Visible = false;
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReportView.Visible = false;
        pnlColor.Visible = false;
        pnlPartialLeave1.Visible = true;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        PnlTime.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlList.Visible = false;
        panelHR.Visible = false;
        txtTotalMinutes.Focus();
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "1")
        {
            dvPL.Visible = false;
        }
        return;
    }
    protected void btnSMSEmail_Click(object sender, EventArgs e)
    {
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPaneltyCalc.Visible = false;
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReportView.Visible = false;
        pnlColor.Visible = false;
        pnlPartialLeave1.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        PnlTime.Visible = false;

        pnlSMSEmail.Visible = true;
        pnlKeyPreference.Visible = false;
        pnlList.Visible = false;
        panelHR.Visible = false;
        txtSMSAPI.Focus();
        return;
    }
    protected void btnWorkCalc_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPaneltyCalc.Visible = false;
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReportView.Visible = false;
        pnlColor.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = true;
        PnlTime.Visible = false;

        pnlSMSEmail.Visible = false;
        pnlPartialLeave1.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlList.Visible = false;
        panelHR.Visible = false;
        ddlWorkCal.Focus();
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "4")
        {
            dvPFEsic.Visible = false;
            dvSalInc.Visible = false;
        }
        if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "1")
        {
            dvWorkCal.Visible = false;
        }
        return;
    }
    protected void btnAttendence_Click(object sender, EventArgs e)
    {
        PnlAttendence.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        PnlHR.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.Visible = true;
        pnlWorkCal.Visible = true;
        pnlSMSEmailB.Visible = true;
        pnlPartialLeave.Visible = true;
        pnlKeyPref.Visible = true;
        pnlColorCode.Visible = true;
        pnlPanelty.Visible = true;
        pnlList.Visible = false;
        btnTime_Click(null, null);
        //panelHR.Visible = false;
        DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
        if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "1")
        {
            pnlTimetableList.Visible = false;
            pnlSMSEmailB.Visible = false;
            pnlKeyPref.Visible = false;
            pnlColorCode.Visible = false;
            PnlTime.Visible = false;
            pnlPartialLeave1.Visible = false;
        }
        else if (DtApp_Id.Rows[0]["Param_Value"].ToString() == "4")
        {
            //PnlHR.Visible = false;

        }
    }
    protected void BtnHr_Click(object sender, EventArgs e)
    {
        PnlAttendence.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlHR.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.Visible = false;
        pnlWorkCal.Visible = false;
        pnlSMSEmailB.Visible = false;
        pnlPartialLeave.Visible = false;
        pnlKeyPref.Visible = false;
        pnlColorCode.Visible = false;
        pnlPanelty.Visible = false;
        PnlTime.Visible = false;
        pnlList.Visible = false;
        PnlTime.Visible = false;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlPartialLeave.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlColor.Visible = false;
        pnlPaneltyCalc.Visible = false;
        panelHR.Visible = true;
        Button12.Visible = false;
        pnlReportView.Visible = false;
        pnlReport.Visible = false;
        //SetCompanyParameter(hdnCompanyId.Value);

    }
    protected void btnInventory_Click(object sender, EventArgs e)
    {
        PnlAttendence.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlHR.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlInventory.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlTimetableList.Visible = false;
        pnlWorkCal.Visible = false;
        pnlSMSEmailB.Visible = false;
        pnlPartialLeave.Visible = false;
        pnlKeyPref.Visible = false;
        pnlColorCode.Visible = false;
        pnlPanelty.Visible = false;
        ddlOption.SelectedIndex = 2;
        pnlReportView.Visible = false;


        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        txtParameterName.Focus();
        Reset();
        AllPageCode();

    }
    protected void btnTime_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPaneltyCalc.Visible = false;
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");


        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReportView.Visible = false;
        pnlColor.Visible = false;
        pnlPartialLeave1.Visible = false;
        PnlTime.Visible = true;
        PnlOT.Visible = false;
        pnlWork.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlList.Visible = false;
        panelHR.Visible = false;
        ddlEmpSync.Focus();
        return;
    }
    protected void btnOT_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPaneltyCalc.Visible = false;
        pnlKeyPref.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPartialLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");


        pnlTimetableList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlWorkCal.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSMSEmailB.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColorCode.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlColor.Visible = false;
        pnlPartialLeave1.Visible = false;
        PnlTime.Visible = false;
        PnlOT.Visible = true;
        pnlWork.Visible = false;
        pnlSMSEmail.Visible = false;
        pnlKeyPreference.Visible = false;
        pnlList.Visible = false;
        panelHR.Visible = false;

    }



    #region InventoryParameter
    private void FillGrid()
    {
        ////DataTable dtBrand = objParam.GetParameterMasterAllTrue(compid);
        //lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        //Session["dtParameter"] = dtBrand;
        //Session["dtFilter_C_P_Master"] = dtBrand;
        //if (dtBrand != null && dtBrand.Rows.Count > 0)
        //{
        //    GvParameter.DataSource = dtBrand;
        //    GvParameter.DataBind();
        //    AllPageCode();
        //}
        //else
        //{
        //    GvParameter.DataSource = null;
        //    GvParameter.DataBind();
        //}
    }
    public void Reset()
    {
        txtParameterValue.ReadOnly = true;
        txtParameterName.Text = "";
        txtParameterValue.Text = "";
        editid.Value = "";

        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {

        //if (txtParameterValue.Text == "" || txtParameterValue.Text == null)
        //{
        //    DisplayMessage("Enter Parameter Value");
        //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtParameterValue);
        //    return;
        //}
        //int b = 0;
        //if (editid.Value != "")
        //{
        //    b = objParam.UpdateParameterMaster(StrCompId, editid.Value, txtParameterName.Text, txtParameterValue.Text.Trim().ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
        //    editid.Value = "";
        //    if (b != 0)
        //    {
        //        Reset();
        //        FillGrid();
        //        DisplayMessage("Record Updated", "green");

        //    }
        //    else
        //    {
        //        DisplayMessage("Record  Not Updated");
        //    }
        //}
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //editid.Value = e.CommandArgument.ToString();
        //txtParameterValue.ReadOnly = false;

        //DataTable dtParameter = objParam.GetParameterMasterById(StrCompId, editid.Value);

        //txtParameterName.Text = dtParameter.Rows[0]["ParameterName"].ToString();
        //txtParameterValue.Text = dtParameter.Rows[0]["ParameterValue"].ToString();
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtParameterValue);
    }
    protected void GvParameter_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvParameter.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_C_P_Master"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvParameter, dt, "", "");
        AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, ImageClickEventArgs e)
    {

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
            }
            DataTable dtCurrency = (DataTable)Session["dtParameter"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvParameter, view.ToTable(), "", "");
            Session["dtFilter_C_P_Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            AllPageCode();
        }
    }
    protected void GvParameter_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_C_P_Master"];
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
        Session["dtFilter_C_P_Master"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvParameter, dt, "", "");
        AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();

        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }

    #endregion
    #region HrParameter
    protected void btnHrSave_Click(object sender, EventArgs e)
    {


    }
    protected void BtnHrClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("../MasterSetup/CompanyMaster.aspx");
    }
    #endregion

    protected void ddlHolidayValidity_SelectedIndexChanged(object sender, EventArgs e)
    {


        if (ddlHolidayValidity.SelectedIndex > 0)
        {
            txtHolidayValue.Text = "";
            txtHolidayValue.Visible = true;
        }
        else
        {
            txtHolidayValue.Text = "0";
            txtHolidayValue.Visible = false;
        }
    }

    private void setModuleWiseControls()
    {
        try
        {
            li_help.Visible = Help_New.Visible = false;
            Common.clsApplicationModules _cls = (Common.clsApplicationModules)Session["clsApplicationModule"];
            Li_TimeMan.Visible = TimeMan_New.Visible = _cls.isAttendanceModule;
            Li_HR.Visible = HR_New.Visible = _cls.isHrAndPayrollModule;
            lblsendemail.Visible = chkSendEmail.Visible = _cls.isAttendanceModule;
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkIntegratedwithPOS_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIntegratedwithPOS.Checked == true)
        {
            btnSynchPOSData.Visible = true;
        }
        else
        {
            btnSynchPOSData.Visible = false;
        }
    }

    protected void btnSynchPOSData_Click(object sender, EventArgs e)
    {
        DataUploadInPOS();
    }

    public string DataUploadInPOS()
    {
        string strResponse = string.Empty;

        ServicePointManager.Expect100Continue = true;
        //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 ;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | (SecurityProtocolType)3072;

        //var baseAddress = "https://localhost:44300/ItemMasterAPI";
        var baseAddress = ConfigurationManager.AppSettings["POSApiURLforPryce"].ToString();
        var http = (System.Net.HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
        http.Accept = "application/json; charset=utf-8";
        http.ContentType = "application/x-www-form-urlencoded";
        http.Method = "POST";

        var requestData = new GetItemData
        {
            Param = "InsertInPOS",
            ProductDetail = new ItemMaster
            {
                CompanyId = "2",
                BrandId = "2",
                ProductId = "4",
                ProductCode = "KX-NCS3508",
                PartNo = "False",
                ModelNo = "178",
                ModelName = "0",
                ProductName = "Product A",
                ProductNameAr = "Product A Arabic",
                CountryId = "789",
                UnitId = "1",
                ItemType = "S",
                HScode = "HS1234",
                HasBatchNo = "true",
                TypeOfBatchNo = "Internally",
                HasSerialNo = "false",
                ReorderQty = "10",
                CostPrice = "100.00",
                Description = "Description of Product A",
                SalesPrice1 = "150.00",
                SalesPrice2 = "160.00",
                SalesPrice3 = "170.00",
                ProductColor = "0",
                WSalesPrice = "140.00",
                ReservedQty = "5",
                DamageQty = "2",
                ExpiredQty = "1",
                MaximumQty = "50",
                MinimumQty = "5",
                Profit = "20.00",
                Discount = "5.00",
                MaintainStock = "NM",
                URL = "http://example.com/productA",
                Weight = "0",
                WeightUnitID = "0",
                DimLenth = "0",
                DimHeight = "0",
                DimDepth = "0",
                AlternateId1 = "ALT001",
                AlternateId2 = "ALT002",
                AlternateId3 = "ALT003",
                Field1 = "Field 1",
                Field2 = "Field 2",
                Field3 = "Field 3",
                Field4 = "Field 4",
                Field5 = "Field 5",
                Field6 = "false",
                Field7 = "2024-06-29T12:00:00",
                IsActive = "true",
                CreatedBy = "Admin",
                CreatedDate = "2024-06-29T12:00:00",
                ModifiedBy = "Admin",
                ModifiedDate = "2024-06-30T10:30:00",
                DeveloperCommission = "0",
                ProjectId = "0",
                CurrencyId = "10",
                SnoPrefix = "0",
                SnoSuffix = "0",
                SnoStartFrom = "0",
                SizeId = "1",
                ColourId = "1",
                ProductBrand = new List<ProductBrand>
                {
                    new ProductBrand { ProductBrandId = "8", ProductBrandName = ""}
                },
                ProductCategory = new List<ProductCategory>
                {
                    new ProductCategory { ProductCategoryId = "8", ProductCategoryName = ""}
                }
            }
        };

        //  string jsonContent = JsonConvert.SerializeObject(requestData);

        var jsonContent = JsonConvert.SerializeObject(new[] { requestData });

        ASCIIEncoding encoding = new ASCIIEncoding();
        Byte[] bytes = encoding.GetBytes(jsonContent);

        Stream newStream = http.GetRequestStream();
        newStream.Write(bytes, 0, bytes.Length);
        newStream.Close();

        var response = (HttpWebResponse)http.GetResponse();
        var stream = response.GetResponseStream();
        var sr = new StreamReader(stream);
        var content = sr.ReadToEnd();

        return content;
    }
    public class GetItemData
    {
        public string Param { get; set; }
        public ItemMaster ProductDetail { get; set; }
    }

    public class ItemMaster
    {
        public string CompanyId { get; set; }
        public string BrandId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string PartNo { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string ProductName { get; set; }
        public string ProductNameAr { get; set; }
        public string CountryId { get; set; }
        public string UnitId { get; set; }
        public string ItemType { get; set; }
        public string HScode { get; set; }
        public string HasBatchNo { get; set; }
        public string TypeOfBatchNo { get; set; }
        public string HasSerialNo { get; set; }
        public string ReorderQty { get; set; }
        public string CostPrice { get; set; }
        public string Description { get; set; }
        public string SalesPrice1 { get; set; }
        public string SalesPrice2 { get; set; }
        public string SalesPrice3 { get; set; }
        public string ProductColor { get; set; }
        public string WSalesPrice { get; set; }
        public string ReservedQty { get; set; }
        public string DamageQty { get; set; }
        public string ExpiredQty { get; set; }
        public string MaximumQty { get; set; }
        public string MinimumQty { get; set; }
        public string Profit { get; set; }
        public string Discount { get; set; }
        public string MaintainStock { get; set; }
        public string URL { get; set; }
        public string Weight { get; set; }
        public string WeightUnitID { get; set; }
        public string DimLenth { get; set; }
        public string DimHeight { get; set; }
        public string DimDepth { get; set; }
        public string AlternateId1 { get; set; }
        public string AlternateId2 { get; set; }
        public string AlternateId3 { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string DeveloperCommission { get; set; }
        public string ProjectId { get; set; }
        public string CurrencyId { get; set; }
        public string SnoPrefix { get; set; }
        public string SnoSuffix { get; set; }
        public string SnoStartFrom { get; set; }
        public string SizeId { get; set; }
        public string ColourId { get; set; }
        public List<ProductBrand> ProductBrand { get; set; }
        public List<ProductCategory> ProductCategory { get; set; }
    }

    public class ProductBrand
    {
        public string ProductBrandId { get; set; }
        public string ProductBrandName { get; set; }
    }
    public class ProductCategory
    {
        public string ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }
    }
}
