using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using DevExpress.XtraRichEdit.Forms;
using DevExpress.Utils;
using System.IO;
using System.Runtime.Remoting;

public partial class Attendance_DefaultParameter : System.Web.UI.Page
{
    Att_ShiftManagement objShift = null;
    CountryMaster objCountry = null;
    CurrencyMaster ObjCurrency = null;
    Common cmn = null;
    hr_gratuity_plan objGratuityPlanMaster = null;
    DataAccessClass objDa = null;
    Set_ApplicationParameter objAppParam = null;
    SystemParameter objSys = null;
    PageControlCommon objPageCmn = null;
    Set_Allowance ObjAllowance = null;
    hr_gratuity_plan objGratuityPlan = null;
    hr_gratuity_days_detail objGratuityPlanDetail = null;
    hr_laborLaw_config ObjLabourLaw = null;
    hr_laborLaw_leave ObjLabourLawLeave = null;
    LeaveMaster objLeaveType = null;
    Att_Device_Operation ObjdeviceOp = null;
    Set_DocNumber objDocNo = null;
    Set_Allowance ObjAddAll = null;
    Set_Deduction ObjAddDed = null;
    Set_DeductionDetail ObjDeductiondetail = null;
    Set_ApprovalMaster ObjApproval = null;
    Set_Approval_Employee objApprovalEmp = null;
    DepartmentMaster objDep = null;
    EmployeeMaster objEmp = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objCountry = new CountryMaster(Session["DBConnection"].ToString());
        ObjCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        objGratuityPlan = new hr_gratuity_plan(Session["DBConnection"].ToString());
        objGratuityPlanDetail = new hr_gratuity_days_detail(Session["DBConnection"].ToString());
        ObjLabourLaw = new hr_laborLaw_config(Session["DBConnection"].ToString());
        ObjLabourLawLeave = new hr_laborLaw_leave(Session["DBConnection"].ToString());
        objLeaveType = new LeaveMaster(Session["DBConnection"].ToString());
        ObjdeviceOp = new Att_Device_Operation(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjAddAll = new Set_Allowance(Session["DBConnection"].ToString());
        objGratuityPlanMaster = new hr_gratuity_plan(Session["DBConnection"].ToString());
        ObjAddDed = new Set_Deduction(Session["DBConnection"].ToString());
        ObjDeductiondetail = new Set_DeductionDetail(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objApprovalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            FillCountry();
            FillCurrency();
            MVDeafult.ActiveViewIndex = 0;
            FillShift();
            rbtnOtDisable.Checked = true;
            rbtOT_OnCheckedChanged(null, null);
            rbtnPartialDisable.Checked = true;
            rbtPartial_OnCheckedChanged(null, null);
            rbtnKeyDisable.Checked = true;
            rbtKeyPref_OnCheckedChanged(null, null);
            SetCompanyParameter(Session["CompId"].ToString());
            FillGridGratuity();
            FillLeaveTypeDDL();
            //GetDocumentNumber();
            FillLabourLawGrid();
            FillPlan();
            FillAllownceGrid();
            FillGridList();
            FillGrid();
        }
    }

    public void SetCompanyParameter(string CompId)
    {
        DataTable dtCompanyInfo = objDa.return_DataTable("select set_companymaster.Company_Name,Set_BrandMaster.Brand_Name, Set_LocationMaster.Location_Name,set_companymaster.country_id,set_companymaster.currency_id from Set_LocationMaster inner join set_companymaster on Set_LocationMaster.company_id = set_companymaster.company_id inner join Set_BrandMaster on Set_LocationMaster.Brand_id =Set_BrandMaster.Brand_id where Set_LocationMaster.location_id= " + Session["LocId"].ToString() + "");

        txtCompanyName.Text = dtCompanyInfo.Rows[0]["Company_Name"].ToString();
        txtBrandName.Text = dtCompanyInfo.Rows[0]["Brand_Name"].ToString();
        txtLocationname.Text = dtCompanyInfo.Rows[0]["Location_Name"].ToString();
        if (dtCompanyInfo.Rows[0]["country_id"].ToString().Trim() == "0")
        {
            ddlCountry.SelectedIndex = 0;
        }
        else
        {
            ddlCountry.SelectedValue = dtCompanyInfo.Rows[0]["country_id"].ToString();
        }
        if (dtCompanyInfo.Rows[0]["currency_id"].ToString().Trim() == "0")
        {
            ddlCurrency.SelectedIndex = 0;
        }
        else
        {
            ddlCurrency.SelectedValue = dtCompanyInfo.Rows[0]["currency_id"].ToString();
        }

        string[] WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Split(',');
        for (int j = 0; j < WeekOffDays.Length; j++)
        {
            if (WeekOffDays[j].ToString().ToLower() == "sunday")
            {
                chkSunday.Checked = true;
            }
            if (WeekOffDays[j].ToString().ToLower() == "monday")
            {
                chkMonday.Checked = true;
            }
            if (WeekOffDays[j].ToString().ToLower() == "tuesday")
            {
                chkTuesday.Checked = true;
            }
            if (WeekOffDays[j].ToString().ToLower() == "wednesday")
            {
                chkWednesday.Checked = true;
            }
            if (WeekOffDays[j].ToString().ToLower() == "thursday")
            {
                chkThursday.Checked = true;
            }
            if (WeekOffDays[j].ToString().ToLower() == "friday")
            {
                chkFriday.Checked = true;
            }
            if (WeekOffDays[j].ToString().ToLower() == "saturday")
            {
                chkSaturday.Checked = true;
            }

        }


        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnOtEnable.Checked = true;
            rbtnOtDisable.Checked = false;
            rbnWeekOffOTEnable.Checked = true;
            rbnWeekOffOTDisable.Checked = false;
            rbnHoliayOTEnable.Checked = true;
            rbnHoliayOTDisable.Checked = false;

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

            rbtOT_OnCheckedChanged(null, null);
        }


        txtMaxOTMint.Text = objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        txtMinOTMint.Text = objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlCalculationMethod.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlWorkCal.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());




        txtEmail.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtEmailPassword.Attributes.Add("Value", Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Master_Email_Password", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())));

        txtSMTP.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email_SMTP", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        txtPort.Text = objAppParam.GetApplicationParameterValueByParamName("Master_Email_Port", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        try
        {
            chkenablessl.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Master_Email_EnableSSL", CompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            chkenablessl.Checked = false;
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

    }
    public void FillShift()
    {
        DataTable dt = objShift.GetShiftMaster(Session["CompId"].ToString());
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
    public void FillCountry()
    {
        DataTable dt = objCountry.GetCountryMaster();
        objPageCmn.FillData((object)ddlCountry, dt, "Country_Name", "Country_Id");

    }
    public void FillCurrency()
    {
        DataTable dt = ObjCurrency.GetCurrencyMaster();
        objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {

    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/ERPLogin.aspx");
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/ERPLogin.aspx");
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCountry.SelectedIndex > 0)
        {
            DataTable dt = objDa.return_DataTable("select Currency_Id from sys_country_currency where Country_Id=" + ddlCountry.SelectedValue + "");
            if (dt.Rows.Count > 0)
            {
                ddlCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
            }
            else
            {
                ddlCurrency.SelectedIndex = 0;
            }
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {

        try
        {
            string Password = txtEmailPassword.Text;
            txtEmailPassword.Attributes.Add("value", Password);
            //updating company name
            objDa.execute_Command("update Set_CompanyMaster set Company_Name='" + txtCompanyName.Text + "',Company_Name_L='" + txtCompanyName.Text + "',currency_id=" + ddlCurrency.SelectedValue + ",country_id=" + ddlCountry.SelectedValue + "");
            //updating brand name
            objDa.execute_Command("update Set_BrandMaster set Brand_Name='" + txtBrandName.Text + "',Brand_Name_L='" + txtBrandName.Text + "'");
            //updating location name
            objDa.execute_Command("update Set_LocationMaster  set Location_Name='" + txtLocationname.Text + "',Location_Name_L='" + txtLocationname.Text + "',Field1='" + ddlCurrency.SelectedValue + "' where Location_Id=" + Session["LocId"].ToString() + "");

            int flag = 0;
            string WeekOffDays = string.Empty;

            if (chkSunday.Checked)
            {
                WeekOffDays += "Sunday" + ","; ;
            }
            if (chkMonday.Checked)
            {
                WeekOffDays += "Monday" + ",";
            }
            if (chkTuesday.Checked)
            {
                WeekOffDays += "Tuesday" + ",";
            }
            if (chkWednesday.Checked)
            {
                WeekOffDays += "Wednesday" + ",";

            }
            if (chkThursday.Checked)
            {
                WeekOffDays += "Thursday" + ",";
            }
            if (chkFriday.Checked)
            {
                WeekOffDays += "Friday" + ",";
            }
            if (chkSaturday.Checked)
            {
                WeekOffDays += "Saturday" + ",";
            }

            if (WeekOffDays.Trim() != "")
            {
                WeekOffDays = WeekOffDays.Substring(0, WeekOffDays.Length - 1);
                objDa.execute_Command("update Set_ApplicationParameter set Param_Value='" + WeekOffDays.Trim() + "' where Param_Name='Week Off Days'");
            }
            else
            {
                objDa.execute_Command("update Set_ApplicationParameter set Param_Value='No' where Param_Name='Week Off Days'");
            }

            //updating email credential
            objDa.execute_Command("update Set_ApplicationParameter set Param_Value='" + txtEmail.Text.Trim() + "' where Param_Name='Master_Email'");
            objDa.execute_Command("update Set_ApplicationParameter set Param_Value='" + Common.Encrypt(txtEmailPassword.Text.Trim()) + "' where Param_Name='Master_Email_Password'");
            objDa.execute_Command("update Set_ApplicationParameter set Param_Value='" + txtSMTP.Text.Trim() + "' where Param_Name='Master_Email_SMTP'");
            objDa.execute_Command("update Set_ApplicationParameter set Param_Value='" + txtPort.Text.Trim() + "' where Param_Name='Master_Email_Port'");
            objDa.execute_Command("update Set_ApplicationParameter set Param_Value='" + chkenablessl.Checked.ToString() + "' where Param_Name='Master_Email_EnableSSL'");
            objDa.execute_Command("update Sys_Parameter set Param_Value='" + ddlCurrency.SelectedValue + "' where Param_Name='Base Currency'");
            objDa.execute_Command("INSERT INTO [Sys_Parameter] ([Param_Name] ,[Param_Value] ,[User_Display] ,[Field1] ,[Field2] ,[Field3] ,[Field4] ,[Field5] ,[IsActive] ,[CreatedBy] ,[CreatedDate] ,[ModifiedBy] ,[ModifiedDate]) VALUES ('Cloud_Default_Configuration','" + true.ToString() + "', '" + false.ToString() + "','','','','','','" + true.ToString() + "','Admin','" + DateTime.Now.ToString() + "' ,'Admin','" + DateTime.Now.ToString() + "')");

            if (ddlDefaultShift.SelectedIndex != 0)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Default_Shift", ddlDefaultShift.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Default_Shift", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Effective Work Calculation Method", ddlWorkCal.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (rbtnPartialEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Partial_Leave_Enable", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Partial_Leave_Enable", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            if (txtTotalMinutes.Text == "")
            {
                txtTotalMinutes.Text = "0";
            }

            if (txtMinuteday.Text == "")
            {
                txtMinuteday.Text = "0";
            }

            if (txtMaxOTMint.Text == "")
            {
                txtMaxOTMint.Text = "0";
            }

            if (txtMinOTMint.Text == "")
            {
                txtMaxOTMint.Text = "0";
            }


            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Total Partial Leave Minutes", txtTotalMinutes.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Partial Leave Minute Use In A Day", txtMinuteday.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "PLTimeWithOutTime", chkPLWithTimeWithOutTime.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "PL Date Editable", chkPLDateEditable.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            if (rbtnOtEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "IsOverTime", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "IsOverTime", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            }
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Max Over Time Min", txtMaxOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Min OVer Time Min", txtMinOTMint.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Over Time Calculation Method", ddlCalculationMethod.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            // Week Of OT Functionality
            if (rbnWeekOffOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "WeekOffOTEnable", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "WeekOffOTEnable", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            // Holiday OT Functionality
            if (rbnHoliayOTEnable.Checked == true)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "HolidayOTEnable", "1", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "HolidayOTEnable", "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "In Func Key", txtInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Out Func Key", txtOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Break In Func Key", txtBreakInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Break Out Func Key", txtBreakOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Partial Leave In  Func Key", txtPartialInKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Partial Leave Out  Func Key", txtPartialOutKey.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Shift Range", ddlShiftRange.SelectedValue, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            if (rbtnKeyEnable.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "With Key Preference", "Yes", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "With Key Preference", "No", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            if (ChkNextDayLog.Checked)
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Consider Next Day Log", true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
            else
            {
                objAppParam.UpdateApplicationParameterMasterByCompany(Session["CompId"].ToString(), "Consider Next Day Log", false.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }



            DisplayMessage("Deafult setup has been Completed");

            Response.Redirect("~/MasterSetup/Home.aspx");
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            return;
        }
    }


    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }

    protected void lnkTestemail_Click(object sender, EventArgs e)
    {


        if (txtEmail.Text.Trim() == "")
        {
            DisplayMessage("Enter Email-Id");
            txtEmail.Focus();
            return;
        }

        if (txtEmailPassword.Text.Trim() == "")
        {
            DisplayMessage("Enter Email Password");
            txtEmailPassword.Focus();
            return;
        }
        else
        {
            string Password = txtEmailPassword.Text;
            txtEmailPassword.Attributes.Add("value", Password);
        }

        if (txtSMTP.Text.Trim() == "")
        {
            DisplayMessage("Enter SMIP Information");
            txtSMTP.Focus();
            return;
        }

        if (txtPort.Text.Trim() == "")
        {
            DisplayMessage("Enter Port Information");
            txtPort.Focus();
            return;
        }

        if (SendEmail(txtEmail.Text.Trim(), "", "", txtEmail.Text.Trim(), txtEmailPassword.Text.Trim(), txtSMTP.Text.Trim(), txtPort.Text.Trim(), "This is an e-mail message sent automatically by Pegasus TimeMan while testing the settings for your account. ", "Pegasus TimeMan Test Message"))
        {
            DisplayMessage("Email tests completed successfully");
        }
        else
        {
            DisplayMessage("Email tests result is failed, please check email information");
        }
    }



    public bool SendEmail(string strTo, string strCC, string BCC, string strSenderEmail, string strSenderEmailPassword, string SenderHost, string Senderport, string strBody, string strSubject)
    {
        bool IsEmail = false;

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();


        foreach (string str in strTo.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.To.Add(str);
        }
        foreach (string str in strCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.CC.Add(str);
        }
        foreach (string str in BCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.Bcc.Add(str);
        }

        message.From = new System.Net.Mail.MailAddress(strSenderEmail, "Pryce Cloud Solution");
        message.Subject = strSubject;
        message.IsBodyHtml = true;
        message.Body = strBody;
        SmtpClient smtp = new SmtpClient(SenderHost);
        NetworkCredential basiccr = new NetworkCredential(strSenderEmail, strSenderEmailPassword);
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = basiccr;
        smtp.Port = Convert.ToInt32(Senderport);
        try
        {
            smtp.Send(message);
            IsEmail = true;
        }
        catch (Exception ex)
        {


        }
        return IsEmail;

    }

    protected void btnNext1_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 1;

    }

    protected void btnbacktohome_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 2;
    }

    #region Overtimepartial
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


            rbnWeekOffOTDisable.Enabled = true;
            rbnHoliayOTDisable.Enabled = true;
            rbnWeekOffOTEnable.Enabled = true;
            rbnHoliayOTEnable.Enabled = true;
        }
    }
    protected void rbtPartial_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPartialEnable.Checked)
        {

            txtTotalMinutes.Enabled = true;
            txtMinuteday.Enabled = true;


        }
        else
        {
            txtTotalMinutes.Enabled = false;
            txtMinuteday.Enabled = false;

        }
    }

    #endregion

    protected void btnBackOTPL_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 0;
    }

    protected void btnNextOTPL_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 2;
    }

    #region FunctionKey
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
    #endregion

    protected void btnbackFuncKy_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 1;
    }

    protected void btnNextFunckey_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 3;
    }

    protected void btnNextGratutuity_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 4;
    }
    #region Create Gratuaty plan Pannel Add by Rahul Sharma on date 16-05-2024
    protected void txtBenefitAmountLimit_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).ID == "txtBenefitAmountLimit")
        {
            txtbenefitwagemonth.Text = "0";

        }
        else
        {
            txtBenefitAmountLimit.Text = "0";
        }
    }
    protected void txtLsApplicableAllowances_OnTextChanged(object sender, EventArgs e)
    {
        if (txtApplicableAllowance.Text != "")
        {

            foreach (string str in txtApplicableAllowance.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows.Count == 0)
                {
                    DisplayMessage("select allowance in suggestion only");
                    txtApplicableAllowance.Text = "";
                    txtApplicableAllowance.Focus();
                    break;
                }


            }

            txtApplicableAllowance.Focus();

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
    protected void btnAddGartuaty_Click(object sender, EventArgs e)
    {


        if (txtFromYear.Text == "")
        {
            DisplayMessage("Enter From Year");
            txtFromYear.Focus();
            return;
        }

        if (txttoYear.Text == "")
        {
            DisplayMessage("Enter To year");
            txttoYear.Focus();
            return;
        }


        if (float.Parse(txtFromYear.Text) > float.Parse(txttoYear.Text))
        {
            DisplayMessage("Year days to value should be greater or equal to Year days from value");
            txttoYear.Focus();
            return;
        }

        if (txtremunerationDays.Text == "")
        {
            DisplayMessage("Enter deduction percentage");
            txtremunerationDays.Focus();
            return;
        }

        DataTable dt = GetDeductionList();

        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {
            dt.Rows.Add();

            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = txtFromYear.Text;
            dt.Rows[dt.Rows.Count - 1][2] = txttoYear.Text;
            dt.Rows[dt.Rows.Count - 1][3] = txtremunerationDays.Text;
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {
                    dt.Rows[i][1] = txtFromYear.Text;
                    dt.Rows[i][2] = txttoYear.Text;
                    dt.Rows[i][3] = txtremunerationDays.Text;

                    break;
                }
            }

        }

        try
        {
            GvGartuityDetail.DataSource = dt;
            GvGartuityDetail.DataBind();
        }
        catch (Exception ex)
        {

        }
        btnCancelGratuity_Click(null, null);
        HtmlGenericControl liNew = Li_New;

        // Add the "active" class to the <li> element
        liNew.Attributes["class"] = "active";
    }
    public string GetCurruncyValue()
    {
        string Currency = "0";
        try
        {
            Currency = ddlCurrency.SelectedValue;
        }
        catch
        {

        }
        return Currency;
    }
    protected void btnCancelGratuity_Click(object sender, EventArgs e)
    {
        txtFromYear.Text = "";
        txttoYear.Text = "";
        txtremunerationDays.Text = "";
        txtFromYear.Focus();
        hdndeductionTransId.Value = "";
        txtFromYear.Text = GetExceedFromValue();
        txttoYear.Focus();
        txttoYear.Enabled = true;
    }
    public string GetExceedFromValue()
    {
        string strvalue = "0";

        DataTable dt = GetDeductionList();

        if (dt.Rows.Count > 0)
        {
            strvalue = (float.Parse(new DataView(dt, "", "To_Year desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["To_Year"].ToString()) + 1).ToString();

        }

        return strvalue;

    }
    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = GetDeductionList();


        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        txtFromYear.Text = dt.Rows[0]["From_Year"].ToString();
        txttoYear.Text = dt.Rows[0]["To_Year"].ToString();
        txttoYear.Enabled = false;
        txtremunerationDays.Text = dt.Rows[0]["Remuneration_days"].ToString();
        hdndeductionTransId.Value = e.CommandArgument.ToString();

        dt.Dispose();
    }

    public DataTable GetDeductionList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("From_Year", typeof(float));
        dt.Columns.Add("To_Year", typeof(float));
        dt.Columns.Add("Remuneration_days", typeof(float));


        foreach (GridViewRow gvrow in GvGartuityDetail.Rows)
        {
            DataRow dr = dt.NewRow();

            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblDaysFrom")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblDaysTo")).Text;
            dr[3] = ((Label)gvrow.FindControl("lbldeductionpercentage")).Text;

            dt.Rows.Add(dr);
        }



        return dt;
    }
    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtemp = GetDeductionList();


        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)GvGartuityDetail, dtemp, "", "");


        txtFromYear.Text = GetExceedFromValue();
        dtemp.Dispose();

    }
    protected void btnSaveGratuityPlan_Click(object sender, EventArgs e)
    {
        string strApplicableAllowance = string.Empty;

        DataTable dtGratuity = objGratuityPlan.GetAllTrueRecord(Session["CompId"].ToString(), "0", "0");



        if (txtApplicableAllowance.Text != "")
        {
            foreach (string str in txtApplicableAllowance.Text.Split(','))
            {

                if (str.Trim() == "")
                {
                    continue;
                }

                if (!txtApplicableAllowance.Text.Trim().Split(',').Contains(ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows[0]["Allowance_Id"].ToString()))
                {
                    strApplicableAllowance += ObjAllowance.GetAllowanceByAllowance(Session["CompId"].ToString(), str).Rows[0]["Allowance_Id"].ToString() + ",";
                }

            }
        }
        else
        {
            strApplicableAllowance = "0";
        }

        int b = 0;


        //here we are checking that gratuity plan is already exist or not 

        if (editid.Value == "")
        {
            dtGratuity = new DataView(dtGratuity, "Plan_Name='" + txtPlanName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        else
        {
            dtGratuity = new DataView(dtGratuity, "Plan_Name='" + txtPlanName.Text.Trim() + "' and Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();


        }

        if (dtGratuity.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtGratuity.Rows[0]["IsActive"].ToString()))
            {
                DisplayMessage("Plan Name is already exist");
                txtPlanName.Focus();
                return;

            }
            else
            {
                DisplayMessage("Plan Name is already exist in bin section");
                txtPlanName.Focus();
                return;
            }
        }


        if (GvGartuityDetail.Rows.Count == 0)
        {
            DisplayMessage("Enter Gratuity days detail");
            return;
        }



        if (editid.Value == "")
        {
            b = objGratuityPlan.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtPlanName.Text, txtEligibleMonth.Text, txtBenefitAmountLimit.Text, txtbenefitwagemonth.Text, chkIsTaxFree.Checked.ToString(), rbtnserviceCalc_Nearestinteger.Checked ? "0" : "1", strApplicableAllowance, chkIsforefeitprovision.Checked.ToString(), txtMonthDaysCount.Text, txtbenefitperonTermination.Text, txtbenefitperonresign.Text, txtbenefitperonretirement.Text, txtbenefitperondeath.Text, txtbenefitperonother.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkisabsent.Checked.ToString(), chkisunpaidLeave.Checked.ToString(), chkispaidleave.Checked.ToString(), chkisholiday.Checked.ToString(), chkisweekoff.Checked.ToString());
            //b = objLeave.InsertLeaveMaster(Session["CompId"].ToString(), txtLeaveName.Text, txtLeaveNameL.Text, Session["BrandId"].ToString(), false.ToString(), chkIsLeaveSalaryGiven.Checked.ToString(), txtRequiredservicedays.Text, txtMaxAttempt.Text, txtMaxLeaveBalance.Text, "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            //here we inserting record in deduction table if exist

            foreach (GridViewRow gvr in GvGartuityDetail.Rows)
            {

                objGratuityPlanDetail.InsertRecord(b.ToString(), ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }


            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                FillGridGratuity();
                ResetGratuity();
                btnList_Click(null, null);
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            b = objGratuityPlan.updateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtPlanName.Text, txtEligibleMonth.Text, txtBenefitAmountLimit.Text, txtbenefitwagemonth.Text, chkIsTaxFree.Checked.ToString(), rbtnserviceCalc_Nearestinteger.Checked ? "0" : "1", strApplicableAllowance, chkIsforefeitprovision.Checked.ToString(), txtMonthDaysCount.Text, txtbenefitperonTermination.Text, txtbenefitperonresign.Text, txtbenefitperonretirement.Text, txtbenefitperondeath.Text, txtbenefitperonother.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkisabsent.Checked.ToString(), chkisunpaidLeave.Checked.ToString(), chkispaidleave.Checked.ToString(), chkisholiday.Checked.ToString(), chkisweekoff.Checked.ToString());

            //b = objLeave.UpdateLeaveMaster(editid.Value, Session["CompId"].ToString(), txtLeaveName.Text, txtLeaveNameL.Text, Session["BrandId"].ToString(), false.ToString(), chkIsLeaveSalaryGiven.Checked.ToString(), txtRequiredservicedays.Text, txtMaxAttempt.Text, txtMaxLeaveBalance.Text, "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            //here we inserting record in deduction table if exist

            objGratuityPlanDetail.Deleterecord(editid.Value);
            foreach (GridViewRow gvr in GvGartuityDetail.Rows)
            {

                objGratuityPlanDetail.InsertRecord(editid.Value, ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }


            if (b != 0)
            {
                btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                ResetGratuity();
                FillGridGratuity();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }
    public void FillGridGratuity()
    {
        DataTable dt = objGratuityPlan.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_LeaveType_Mstr"] = dt;
        Session["Leave"] = dt;

    }
    protected void btnList_Click(object sender, EventArgs e)
    {


    }
    protected void btnResetGratuity_Click(object sender, EventArgs e)
    {
        ResetGratuity();
    }
    public void ResetGratuity()
    {
        txtPlanName.Text = "";
        txtEligibleMonth.Text = "";
        txtBenefitAmountLimit.Text = "";
        txtbenefitwagemonth.Text = "";
        txtApplicableAllowance.Text = "";
        chkIsTaxFree.Checked = false;
        chkIsforefeitprovision.Checked = false;
        txtMonthDaysCount.Text = "";
        txtbenefitperonTermination.Text = "";
        txtbenefitperonresign.Text = "";
        txtbenefitperonretirement.Text = "";
        txtbenefitperondeath.Text = "";
        txtbenefitperonother.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;

        ViewState["Select"] = null;
        editid.Value = "";

        txtFromYear.Text = "0";
        txttoYear.Text = "";
        txtremunerationDays.Text = "";
        hdndeductionTransId.Value = "";
        objPageCmn.FillData((object)GvGartuityDetail, null, "", "");
        rbtnserviceCalc_Nearestinteger.Checked = true;
        rbtnserviceCalc_proratebasis.Checked = false;
        chkisabsent.Checked = false;
        chkisunpaidLeave.Checked = false;
        chkispaidleave.Checked = false;
        chkisholiday.Checked = false;
        chkisweekoff.Checked = false;


    }

    protected void gvLeaveMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_LeaveType_Mstr"];
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvLeaveMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_LeaveType_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_LeaveType_Mstr"] = dt;
        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLeaveMaster, dt, "", "");
        //AllPageCode();

    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string IsLeaveSalaryGiven = string.Empty;

        DataTable dt = objGratuityPlan.GetAllRecord_BY_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtPlanName.Text = dt.Rows[0]["Plan_Name"].ToString();
            txtEligibleMonth.Text = dt.Rows[0]["eligibility_month"].ToString();
            txtBenefitAmountLimit.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_amount_limit"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitwagemonth.Text = dt.Rows[0]["benefit_wage_month_limit"].ToString();
            chkIsTaxFree.Checked = Convert.ToBoolean(dt.Rows[0]["is_tax_free"].ToString());
            chkIsforefeitprovision.Checked = Convert.ToBoolean(dt.Rows[0]["is_forefeit_provision"].ToString());
            chkisabsent.Checked = Convert.ToBoolean(dt.Rows[0]["is_absent_day"].ToString());
            chkisunpaidLeave.Checked = Convert.ToBoolean(dt.Rows[0]["is_unpaid_leave"].ToString());
            chkispaidleave.Checked = Convert.ToBoolean(dt.Rows[0]["is_paid_leave"].ToString());
            chkisholiday.Checked = Convert.ToBoolean(dt.Rows[0]["is_holiday"].ToString());
            chkisweekoff.Checked = Convert.ToBoolean(dt.Rows[0]["is_weekoff"].ToString());

            if (dt.Rows[0]["calc_service_period"].ToString() == "0")
            {
                rbtnserviceCalc_Nearestinteger.Checked = true;
                rbtnserviceCalc_proratebasis.Checked = false;

            }
            else
            {
                rbtnserviceCalc_Nearestinteger.Checked = false;
                rbtnserviceCalc_proratebasis.Checked = true;

            }


            txtApplicableAllowance.Text = getAllowanceNamebyId(dt.Rows[0]["applicable_allowances"].ToString());
            txtMonthDaysCount.Text = dt.Rows[0]["month_days_count"].ToString();
            txtbenefitperonTermination.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_termination"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperonresign.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_resign"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperonretirement.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_retirement"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperondeath.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_death"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtbenefitperonother.Text = Common.GetAmountDecimal(dt.Rows[0]["benefit_per_on_other"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            //here we are getting deduction slab if exists

            DataTable dtdeduction = objGratuityPlanDetail.GetRecordBy_GratuiryPlanId(editid.Value).DefaultView.ToTable(true, "Trans_Id", "From_Year", "To_Year", "Remuneration_days");

            objPageCmn.FillData((object)GvGartuityDetail, dtdeduction, "", "");

            txtFromYear.Text = GetExceedFromValue();
            dtdeduction.Dispose();

            btnNewGratuity_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);
        }
    }
    protected void btnNewGratuity_Click(object sender, EventArgs e)
    {
        txtPlanName.Focus();
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
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //code end
        int b = 0;
        b = objGratuityPlan.Restorerecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");


            FillGridGratuity();
            ResetGratuity();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGridGratuity();

        ResetGratuity();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;

    }

    #endregion


    #region Labour Law Function Created By rahul Shrama On Date 18-05-2024
    protected void btnAddLabour_Click(object sender, EventArgs e)
    {
        string strMaxId = "1";



        DataTable dt = GetLeaveDatatable();


        DataTable dtTemp = dt.Copy();

        dtTemp = new DataView(dt, "Leave_Type_Id='" + ddlLeaveType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtTemp.Rows.Count > 0)
        {
            if (dtTemp.Rows[0]["Gender"].ToString() == "Both" || ddlGender.SelectedValue.Trim() == "Both")
            {
                DisplayMessage("Leave type already exists");
                ddlLeaveType.Focus();
                return;
            }


            dtTemp = new DataView(dt, "Leave_Type_Id='" + ddlLeaveType.SelectedValue + "' and Gender='" + ddlGender.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTemp.Rows.Count > 0)
            {
                DisplayMessage("Leave type already exists");
                ddlLeaveType.Focus();
                return;
            }

        }



        if (dt.Rows.Count > 0)
        {
            strMaxId = (float.Parse(new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1).ToString();
        }


        dt.Rows.Add(ddlGender.SelectedValue, ddlLeaveType.SelectedValue, txtTotalLeave.Text, txtPaidLeave.Text, ddlSchType.SelectedValue, chkYearCarry.Checked, chkIsAuto.Checked, ChkIsRule.Checked, strMaxId);


        objPageCmn.FillData((object)GvLeaveDetail, dt, "", "");

        btnCancelLabour_Click(null, null);


    }

    protected void btnCancelLabour_Click(object sender, EventArgs e)
    {
        ddlLeaveType.SelectedIndex = 0;
        txtTotalLeave.Text = "";
        txtPaidLeave.Text = "";
        chkYearCarry.Checked = false;
        chkIsAuto.Checked = false;
        ChkIsRule.Checked = false;
        ddlLeaveType.Focus();

    }
    public DataTable GetLeaveDatatable()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("Gender");
        dt.Columns.Add("Leave_Type_Id");
        dt.Columns.Add("Total_Leave_days");
        dt.Columns.Add("Paid_Leave_days");
        dt.Columns.Add("schedule_type");
        dt.Columns.Add("is_yearcarry");
        dt.Columns.Add("is_auto");
        dt.Columns.Add("is_rule");
        dt.Columns.Add("Trans_Id", typeof(float));

        foreach (GridViewRow gvrow in GvLeaveDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            dr[0] = ((Label)gvrow.FindControl("lblGender")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblLeave_Type_Id")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblTotalleaveDay")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblTotalPaidDay")).Text;
            dr[4] = ((Label)gvrow.FindControl("lblscheduleType")).Text;
            dr[5] = ((Label)gvrow.FindControl("lblisYearcarry")).Text;
            dr[6] = ((Label)gvrow.FindControl("lblisauto")).Text;
            dr[7] = ((Label)gvrow.FindControl("lblisRule")).Text;
            dr[8] = dt.Rows.Count + 1;
            dt.Rows.Add(dr);
        }

        return dt;

    }
    protected void btnResetLabourLaw_Click(object sender, EventArgs e)
    {
        ResetLabourLaw();
    }
    public void ResetLabourLaw()
    {

        Lbl_Tab_New.Text = Resources.Attendance.New;

        ViewState["Select"] = null;
        editid.Value = "";
        objPageCmn.FillData((object)GvLeaveDetail, null, "", "");
        btnCancelLabour_Click(null, null);
        txtLabourLawname.Focus();
        txtLabourLawname.Text = "";
        ddlFinancestartMonth.SelectedIndex = 0;
        txtDescription.Text = "";
        txtWorkdayMinute.Text = "";
        txtyearlyHalfDay.Text = "";
        ChkWeekOffList.ClearSelection();

    }
    protected void btnSavelabourLaw_Click(object sender, EventArgs e)
    {

        string strIndemnityPlanId = "0";
        string WeekOffDays = string.Empty;


        if (ddlPlanName.SelectedIndex > 0)
        {
            strIndemnityPlanId = ddlPlanName.SelectedValue;
        }



        for (int i = 0; i < ChkWeekOffList.Items.Count; i++)
        {
            if (ChkWeekOffList.Items[i].Selected == true)
            {
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


        DataTable dtTemp = ObjLabourLaw.GetAllRecord(Session["CompId"].ToString());



        if (editid.Value == "")
        {
            dtTemp = new DataView(dtTemp, "Laborlaw_Name='" + txtLabourLawname.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtTemp = new DataView(dtTemp, "Laborlaw_Name='" + txtLabourLawname.Text.Trim() + "' and Trans_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        }


        if (dtTemp.Rows.Count > 0)
        {


            if (Convert.ToBoolean(dtTemp.Rows[0]["IsActive"].ToString()))
            {
                DisplayMessage("Labour law name is already exists");
                txtLabourLawname.Focus();
                return;
            }
            else
            {
                DisplayMessage("Labour law name is already exists in bin section");
                txtLabourLawname.Focus();
                return;
            }
        }


        int b = 0;

        if (editid.Value == "")
        {

            b = ObjLabourLaw.Insertrecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtLabourLawname.Text, txtDescription.Text, ddlFinancestartMonth.SelectedValue, strIndemnityPlanId, txtWorkdayMinute.Text, WeekOffDays, txtyearlyHalfDay.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            DataTable dtLeave = GetLeaveDatatable();

            foreach (DataRow dr in dtLeave.Rows)
            {
                ObjLabourLawLeave.Insertrecord(b.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["schedule_type"].ToString(), dr["is_yearcarry"].ToString(), dr["is_auto"].ToString(), dr["is_rule"].ToString(), dr["Gender"].ToString());
            }



            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                FillLabourLawGrid();
                ResetLabourLaw();

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }

        }
        else
        {

            b = ObjLabourLaw.Updaterecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtLabourLawname.Text, txtDescription.Text, ddlFinancestartMonth.SelectedValue, strIndemnityPlanId, txtWorkdayMinute.Text, WeekOffDays, txtyearlyHalfDay.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            ObjLabourLawLeave.DeleteRecord(editid.Value);

            DataTable dtLeave = GetLeaveDatatable();

            foreach (DataRow dr in dtLeave.Rows)
            {
                ObjLabourLawLeave.Insertrecord(editid.Value, dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["schedule_type"].ToString(), dr["is_yearcarry"].ToString(), dr["is_auto"].ToString(), dr["is_rule"].ToString(), dr["Gender"].ToString());
            }


            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                FillLabourLawGrid();
                ResetLabourLaw();
                btnList_Click(null, null);

            }
            else
            {
                DisplayMessage("Record Not updated");
            }
        }
    }
    public void FillLeaveTypeDDL()
    {
        DataTable dt = objLeaveType.GetLeaveMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            ddlLeaveType.DataSource = null;
            ddlLeaveType.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlLeaveType, dt, "Leave_Name", "Leave_Id");
        }
        else
        {
            ddlLeaveType.Items.Insert(0, "--Select--");
            ddlLeaveType.SelectedIndex = 0;
        }
    }
    public void FillPlan()
    {
        ddlPlanName.Items.Clear();

        DataTable dt = objGratuityPlanMaster.GetAllTrueRecord(Session["CompId"].ToString(), Session["brandid"].ToString(), Session["locid"].ToString());

        ddlPlanName.DataSource = dt;
        ddlPlanName.DataTextField = "Plan_Name";
        ddlPlanName.DataValueField = "Trans_Id";
        ddlPlanName.DataBind();

        ddlPlanName.Items.Insert(0, "--select--");

        if (dt.Rows.Count > 0)
        {
            ddlPlanName.SelectedIndex = 1;
        }
        else
        {
            ddlPlanName.SelectedIndex = 0;
        }
    }
    public void FillLabourLawGrid()
     {
        DataTable dt = ObjLabourLaw.GetAllTrueRecord(Session["CompId"].ToString());

        //Common Function add By Lokesh on 13-05-2015
        objPageCmn.FillData((object)gvLabourLaw, dt, "", "");
        Session["dtFilter_LeaveType_Mstr"] = dt;
        Session["Leave"] = dt;

    }
    protected void chkDefultLabourLaw_Changed(object sender,EventArgs e)
    {
        if (gvLabourLaw.Rows.Count > 0)
        {
            foreach (GridViewRow gvr in gvLabourLaw.Rows)
            {
                CheckBox chkDefult = (CheckBox)gvr.FindControl("ChkDefultLabourLaw");
                Label lbLeaveName = (Label)gvr.FindControl("lbLeaveName");
                HiddenField hdnTransId = (HiddenField)gvr.FindControl("hdnTransId");
                if (chkDefult == (CheckBox)sender)
                {
                    // Set the sender checkbox to true
                    chkDefult.Checked = true;

                    objDa.execute_Command("Update Set_LocationMaster Set Field3='"+ hdnTransId.Value.ToString()+ "' where Location_Id='1'");
                }
                else
                {
                    // Set all other checkboxes to false
                    chkDefult.Checked = false;
                }
            }
        }

    }
    protected void btnLabourBack_Click(object sender, EventArgs e)
    {
        
        MVDeafult.ActiveViewIndex = 3;

    }
    protected void btnLabourNext_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 5;
    }
    #endregion

    //#region Employee Upload Section

    //protected void FileUploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    //{
    //    if (fileLoad!=null)
    //    {
    //        int fileType = 0;
    //        string fileName = Path.GetFileName(fileLoad.FileName);
    //        string filePath = Server.MapPath("~/Uploads/" + fileName);
    //        fileLoad.SaveAs(filePath);
    //        string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
    //        if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
    //        {
    //            DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
    //            return;
    //        }
    //        else
    //        {
    //            if (ext == ".xls")
    //            {
    //                fileType = 0;
    //            }
    //            else if (ext == ".xlsx")
    //            {
    //                fileType = 1;
    //            }
    //            else if (ext == ".mdb")
    //            {
    //                fileType = 2;
    //            }
    //            else if (ext == ".accdb")
    //            {
    //                fileType = 3;
    //            }
    //            string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
    //            fileLoad.SaveAs(path);
    //            Import(path, fileType);
    //        }
    //        // Handle the uploaded file (e.g., save to database, process data, etc.)
    //    }
    //    else
    //    {
    //        // Handle the error
    //        // e.g., display an error message
    //    }
    //}

    ////protected void FileUploadComplete(object sender, EventArgs e)
    ////{
    ////    int fileType = 0;
    ////    if (fileLoad.HasFile)
    ////    {
    ////        string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
    ////        if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
    ////        {
    ////            DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
    ////            return;
    ////        }
    ////        else
    ////        {
    ////            if (ext == ".xls")
    ////            {
    ////                fileType = 0;
    ////            }
    ////            else if (ext == ".xlsx")
    ////            {
    ////                fileType = 1;
    ////            }
    ////            else if (ext == ".mdb")
    ////            {
    ////                fileType = 2;
    ////            }
    ////            else if (ext == ".accdb")
    ////            {
    ////                fileType = 3;
    ////            }
    ////            string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
    ////            fileLoad.SaveAs(path);
    ////            Import(path, fileType);
    ////        }
    ////    }
    ////}
    //public void Import(String path, int fileType)
    //{
    //    string strcon = string.Empty;
    //    if (fileType == 1)
    //    {
    //        Session["filetype"] = "excel";
    //        strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
    //    }
    //    else if (fileType == 0)
    //    {
    //        Session["filetype"] = "excel";
    //        strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
    //    }
    //    else
    //    {
    //        Session["filetype"] = "access";
    //        //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
    //        strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
    //    }
    //    Session["cnn"] = strcon;
    //    OleDbConnection conn = new OleDbConnection(strcon);
    //    conn.Open();
    //    DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
    //    ddlTables.DataSource = tables;
    //    ddlTables.DataTextField = "TABLE_NAME";
    //    ddlTables.DataValueField = "TABLE_NAME";
    //    ddlTables.DataBind();
    //    conn.Close();
    //}
    //protected string GetDocumentNumber()
    //{
    //    txtuploadReferenceNo.Text = "EM-";
    //    return "EM-";

    //    //DataTable dt = new DataTable();
    //    //DataTable dtCount = ObjdeviceOp.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

    //    //string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "8", "15", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
    //    //int NoRow = 0;
    //    //if (s != "")
    //    //{
    //    //    dt = objDa.return_DataTable("select COUNT(distinct Reference_No)  from Att_Device_Operation where company_id=" + Session["CompId"].ToString() + " and brand_id=" + Session["BrandId"].ToString() + " and location_id=" + Session["LocId"].ToString() + "");
    //    //    if (dt.Rows.Count == 0)
    //    //    {
    //    //        s += "1";
    //    //    }
    //    //    else
    //    //    {
    //    //        NoRow = Convert.ToInt32(dt.Rows[0][0].ToString());
    //    //        bool bCodeFlag = true;
    //    //        while (bCodeFlag)
    //    //        {
    //    //            NoRow += 1;
    //    //            DataTable dtTemp = new DataView(dtCount, "Reference_No='" + s + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    //            if (dtTemp.Rows.Count == 0)
    //    //            {
    //    //                bCodeFlag = false;
    //    //            }
    //    //        }
    //    //        s += NoRow;
    //    //    }
    //    //}
    //    //txtuploadReferenceNo.Text = s;
    //    //return s;
    //}
    //protected void btnGetSheet_Click(object sender, EventArgs e)
    //{
    //    if (txtuploadReferenceNo.Text == "")
    //    {
    //        DisplayMessage("Configure Document number");
    //        return;
    //    }
    //    int fileType = 0;
    //    if (fileLoad.HasFile)
    //    {
    //        string Path = string.Empty;
    //        string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
    //        if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
    //        {
    //            DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
    //            return;
    //        }
    //        else
    //        {
    //            fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
    //            Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
    //            if (ext == ".xls")
    //            {
    //                fileType = 0;
    //            }
    //            else if (ext == ".xlsx")
    //            {
    //                fileType = 1;
    //            }
    //            else if (ext == ".mdb")
    //            {
    //                fileType = 2;
    //            }
    //            else if (ext == ".accdb")
    //            {
    //                fileType = 3;
    //            }
    //            Import(Path, fileType);
    //        }
    //    }
    //}
    //protected void btnConnect_Click(object sender, EventArgs e)
    //{


    //}
    //protected void btnRunservice_Click(object sender, EventArgs e)
    //{


    //}
    //protected void rbtnupdall_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    DataTable dt = (DataTable)Session["UploadEmpDt"];
    //    if (rbtnupdValid.Checked)
    //    {
    //        dt = new DataView(dt, "IsValid='True' or IsValid=''", "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    if (rbtnupdInValid.Checked)
    //    {
    //        dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    gvSelected.DataSource = dt;
    //    gvSelected.DataBind();
    //    lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
    //}
    //protected void btnUploadEmpInfo_Click(object sender, EventArgs e)
    //{

    //}

    //protected void btnResetEmpInfo_Click(object sender,EventArgs e)
    //{

    //}

    //#endregion

    #region Allownce Master Code
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        try
        {
            dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }


        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;
    }
    protected void ddlCalculationType_TextChanged(object sender, EventArgs e)
    {

        chkPresent.Checked = false;
        chkweekoff.Checked = false;
        chkHoliday.Checked = false;
        chkabsent.Checked = false;
        chkPaidLeave.Checked = false;
        chkUnpaidLeave.Checked = false;
        chkHalfday.Checked = false;
        if (ddlCalculationType.SelectedIndex == 0)
        {
            Div_IncludeDay.Visible = false;
        }
        else
        {
            Div_IncludeDay.Visible = true;
        }
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();
                if (Ac_ParameterMaster.GetAccountNameByTransId(((TextBox)sender).Text.Split('/')[1].ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString()) == "")
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {

                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        string strIncludeDay = string.Empty;

        if (ddlCalculationType.SelectedIndex == 1)
        {
            //present day
            if (chkPresent.Checked)
            {
                strIncludeDay = "0" + ",";
            }
            if (chkweekoff.Checked)
            {
                strIncludeDay += "1" + ",";
            }
            if (chkHoliday.Checked)
            {
                strIncludeDay += "2" + ",";
            }
            if (chkabsent.Checked)
            {
                strIncludeDay += "3" + ",";
            }
            if (chkPaidLeave.Checked)
            {
                strIncludeDay += "4" + ",";
            }
            if (chkUnpaidLeave.Checked)
            {
                strIncludeDay += "5" + ",";
            }
            if (chkHalfday.Checked)
            {
                strIncludeDay += "6" + ",";
            }


            if (strIncludeDay.Trim() == "")
            {
                DisplayMessage("select day option for day basis calculation");
                chkPresent.Focus();
                return;
            }

        }


        string strAccTransId = txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[0].ToString();

        if (txtAllowanceName.Text.Trim() == "" || txtAllowanceName.Text.Trim() == null)
        {
            DisplayMessage("Enter Allowance Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            txtAllowanceName.Text = "";
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {
            
        }
        else
        {
            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dtPro = ObjAddAll.GetAllowanceTrueAll(Session["CompId"].ToString());
            dtPro = new DataView(dtPro, "Allowance='" + txtAllowanceName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {
                txtAllowanceName.Text = "";
                txtAllowanceNameL.Text = "";
                DisplayMessage("Allowance Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
                return;
            }
            //code end


            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dt2 = ObjAddAll.GetAllowanceFalseAll(Session["CompId"].ToString());
            dt2 = new DataView(dt2, "Allowance='" + txtAllowanceName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt2.Rows.Count > 0)
            {
                txtAllowanceName.Text = "";
                txtAllowanceNameL.Text = "";
                DisplayMessage("Allowance Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
                return;
            }

            //code end


            //here we insert the record in allowancemaster table
            //code start
            b = ObjAddAll.InsertAllowance(Session["CompId"].ToString(), txtAllowanceName.Text.Trim(), txtAllowanceNameL.Text.Trim(), txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", "", Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlCalculationType.SelectedValue.Trim(), strIncludeDay.Trim());
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                ResetAllownce();
                FillAllownceGrid();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            }
            else
            {
                DisplayMessage("Record Not Saved");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAllowanceName);
            }
            //code end
        }
    }
    public void ResetAllownce()
    {

        txtAllowanceName.Text = "";
        editid.Value = "";      
        txtAllowanceNameL.Text = "";
        txtAccountNo.Text = "";       
        ddlCalculationType.SelectedIndex = 0;
        ddlCalculationType_TextChanged(null, null);


    }

    protected   void BtnReset_Click(object sender, EventArgs e)
    {
        ResetAllownce();

    }
    public string GetAccountNamebyTransId(string strTransId)
    {
        return Ac_ParameterMaster.GetAccountNameByTransId(strTransId, Session["DBConnection"].ToString(), Session["CompId"].ToString()).Split('/')[0].ToString();
    }
    private void FillAllownceGrid()
    {
        DataTable dtAllowance = ObjAddAll.GetAllowanceTrueAll(Session["CompId"].ToString(), Session["LocId"].ToString());        
        Session["dtAllowance"] = dtAllowance;
        Session["dtFilter_All_Master"] = dtAllowance;
        if (dtAllowance != null && dtAllowance.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvAllowance, dtAllowance, "", "");
        }
        else
        { 
            GvAllowance.DataSource = null;
            GvAllowance.DataBind();
        }

    }
    protected void btnAllownceBack_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 4;
    }
    protected void btnAllownceNext_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 6;
    }

    #endregion


    #region Deduction Master Code

    protected void btnCanceldeductionCancel_Click(object sender, EventArgs e)
    {
        ResetDeductionDetail();
        
    }
    protected void btnSaveDeduction_Click(object sender, EventArgs e)
    {

        string strIncludeDay = string.Empty;
        string StrCompId = Session["CompId"].ToString();
        if (ddlDeductionOptionType.SelectedIndex == 1)
        {
            //present day
            if (chkDeductionPresent.Checked)
            {
                strIncludeDay = "0" + ",";
            }
            if (chkDeductionweekoff.Checked)
            {
                strIncludeDay += "1" + ",";
            }
            if (chkDeductionHoliday.Checked)
            {
                strIncludeDay += "2" + ",";
            }
            if (chkDeductionabsent.Checked)
            {
                strIncludeDay += "3" + ",";
            }
            if (chkDeductionPaidLeave.Checked)
            {
                strIncludeDay += "4" + ",";
            }
            if (chkDeductionUnpaidLeave.Checked)
            {
                strIncludeDay += "5" + ",";
            }
            if (chkDeductionHalfday.Checked)
            {
                strIncludeDay += "6" + ",";
            }


            if (strIncludeDay.Trim() == "")
            {
                DisplayMessage("select day option for day basis calculation");
                chkPresent.Focus();
                return;
            }

        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        if (txtDeductionName.Text.Trim() == "" || txtDeductionName.Text.Trim() == null)
        {
            DisplayMessage("Enter Deduction Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
            return;
        }
        if (editid.Value != "")
        {
            //here we check that record is exist or not in true mode
            //if exists than showing message and return the function
            //code start
            DataTable dtCate = ObjAddDed.GetDeductionTrueAll(StrCompId.ToString());
            dtCate = new DataView(dtCate, "Deduction='" + txtDeductionName.Text.Trim() + "' and Deduction_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {


                DisplayMessage("Deduction Already Exists");
                dtCate.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;

            }
            //code end


            //here we check that record is exist or not in false mode
            //if exists than showing message and return the function

            //code start
            dtCate = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
            dtCate = new DataView(dtCate, "Deduction='" + txtDeductionName.Text.Trim() + "' and Deduction_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCate.Rows.Count > 0)
            {


                DisplayMessage("Deduction Already Exists-Go to Bin Tab");
                dtCate.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;

            }
        }
        else
        {

            DataTable dtPro = ObjAddDed.GetDeductionTrueAll(StrCompId.ToString());
            dtPro = new DataView(dtPro, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {

                DisplayMessage("Deduction Name Already Exists");
                dtPro.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;
            }

            //code end

            //here we check that record is exist or not in False mode
            //if exists than showing message and return the function
            //code start
            dtPro = ObjAddDed.GetDeductionFalseAll(StrCompId.ToString());
            dtPro = new DataView(dtPro, "Deduction='" + txtDeductionName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtPro.Rows.Count > 0)
            {

                DisplayMessage("Deduction Already Exists-Go to Bin Tab");
                dtPro.Dispose();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                return;
            }

        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (editid.Value != "")
            {


                //this code for update the record in deductionmaster table

                //code start

                b = ObjAddDed.UpdateDeduction(StrCompId.ToString(), editid.Value, txtDeductionName.Text.Trim(), txtDeductionNameL.Text.Trim().ToString(), ddlDeductionType.SelectedValue, txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", hdnDeductionBransID.Value, hdnDeductionLocationId.Value, true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlDeductionOptionType.SelectedValue, strIncludeDay, ref trns);



                ObjDeductiondetail.DeleteDeduction_By_headerId(editid.Value, ref trns);

                DataTable dtDeductionList = GetDeductionList();

                foreach (DataRow dr in dtDeductionList.Rows)
                {

                    ObjDeductiondetail.InsertDeduction(editid.Value, dr["Calculation_Type"].ToString(), dr["From_Amount"].ToString(), dr["To_Amount"].ToString(), dr["Value"].ToString(), dr["Is_Senior_Citizen"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                }
                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }

                //code end
            }

            else
            {
                //code start
                if (chkAddToAllLocation.Checked)
                {
                    string currentValue = "1";

                    string allLocations = "1";


                    foreach (string LocId in allLocations.Split(','))
                    {
                        if (LocId != "")
                        {
                            b = ObjAddDed.InsertDeduction(StrCompId.ToString(), txtDeductionName.Text.Trim(), txtDeductionNameL.Text.Trim(), ddlDeductionType.SelectedValue, txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", Session["BrandId"].ToString(), LocId, true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlDeductionOptionType.SelectedValue, strIncludeDay, ref trns);
                            DataTable dtDeductionList = GetDeductionDetailList();
                            foreach (DataRow dr in dtDeductionList.Rows)
                            {
                                ObjDeductiondetail.InsertDeduction(b.ToString(), dr["Calculation_Type"].ToString(), dr["From_Amount"].ToString(), dr["To_Amount"].ToString(), dr["Value"].ToString(), dr["Is_Senior_Citizen"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                }
                else
                {
                    b = ObjAddDed.InsertDeduction(StrCompId.ToString(), txtDeductionName.Text.Trim(), txtDeductionNameL.Text.Trim(), ddlDeductionType.SelectedValue, txtAccountNo.Text.Trim() == "" ? "0" : txtAccountNo.Text.Trim().Split('/')[1].ToString(), "", Session["BrandId"].ToString(), Session["LocId"].ToString(), true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlDeductionOptionType.SelectedValue, strIncludeDay, ref trns);

                    DataTable dtDeductionList = GetDeductionDetailList();

                    foreach (DataRow dr in dtDeductionList.Rows)
                    {
                        ObjDeductiondetail.InsertDeduction(b.ToString(), dr["Calculation_Type"].ToString(), dr["From_Amount"].ToString(), dr["To_Amount"].ToString(), dr["Value"].ToString(), dr["Is_Senior_Citizen"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }


                if (b != 0)
                {
                    DisplayMessage("Record Saved", "green");
                    ///System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                    //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeductionName);
                }
            }

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




    }

    public void ResetDeductionDetail()
    {
        txtDeductionFromAmount.Text = "";
        txtDeductionToAmount.Text = "";
        txtdeductionValue.Text = "";
        chkseniorcitizen.Checked = false;

        txtDeductionFromAmount.Focus();
        hdndeductionTransId.Value = "";
    }

    protected void btnAddDeduction_Click(object sender, EventArgs e)
    {



        if (float.Parse(txtDeductionFromAmount.Text) >= float.Parse(txtDeductionToAmount.Text))
        {
            DisplayMessage("From Amount should be less then To amount");
            txtDeductionFromAmount.Focus();
            return;
        }

        DataTable dt = GetDeductionDetailList();




        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {
            dt.Rows.Add();


            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = ddlDeductionOptionType.SelectedValue;
            dt.Rows[dt.Rows.Count - 1][2] = txtDeductionFromAmount.Text;
            dt.Rows[dt.Rows.Count - 1][3] = txtDeductionToAmount.Text;
            dt.Rows[dt.Rows.Count - 1][4] = txtdeductionValue.Text;
            dt.Rows[dt.Rows.Count - 1][5] = chkseniorcitizen.Checked;

        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {

                    dt.Rows[i][1] = ddlDeductionOptionType.SelectedValue;
                    dt.Rows[i][2] = txtDeductionFromAmount.Text;
                    dt.Rows[i][3] = txtDeductionToAmount.Text;
                    dt.Rows[i][4] = txtdeductionValue.Text;
                    dt.Rows[i][5] = chkseniorcitizen.Checked;
                    break;
                }

            }

        }

        objPageCmn.FillData((object)GVDeduction, dt, "", "");

        ResetDeductionDetail();
      //  ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
    }

    public DataTable GetDeductionDetailList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("Calculation_Type");
        dt.Columns.Add("From_Amount", typeof(float));
        dt.Columns.Add("To_Amount", typeof(float));
        dt.Columns.Add("Value", typeof(float));
        dt.Columns.Add("Is_Senior_Citizen", typeof(bool));

        foreach (GridViewRow gvrow in GVDeduction.Rows)
        {
            DataRow dr = dt.NewRow();

            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblcalculationType")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblDaysFrom")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblDaysTo")).Text;
            dr[4] = ((Label)gvrow.FindControl("lblValue")).Text;
            dr[5] = ((Label)gvrow.FindControl("lblSeniorcitizen")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private void FillGridList()
    {
        DataTable dtBrand = ObjAddDed.GetDeductionTrueAll("1","1");       
        Session["dtDeduction"] = dtBrand;
        Session["dtFilter_Deud_Master"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvDeductionList, dtBrand, "", "");
        }
        else
        {
            GvDeductionList.DataSource = null;
            GvDeductionList.DataBind();
        }
    }
    protected void btnbackDeduction_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 5;
    }

    protected void btnNextDeduction_Click(object sender, EventArgs e)
    {
        MVDeafult.ActiveViewIndex = 8;
    }
    #endregion

    #region Approval Master Code
    protected void btnApprovalEdit_Command(object sender, CommandEventArgs e)
    {
        Session["EmpPermission"] = objSys.Get_Approval_Parameter_By_ID(e.CommandArgument.ToString()).Rows[0]["Approval_Level"].ToString();
        rdopriority.Enabled = true;
        rdoHierarchy.Enabled = true;
        chkTeamLeader.Enabled = true;
        chkDepartmentManager.Enabled = true;
        chkParentDepartmentManager.Enabled = true;
        Session["CompId"] = "1";
        Session["BrandId"] = "1";
        Session["LocId"] = "1";

        txtResponsibeDepartmentName.Enabled = true;
       string EmpAccessPermission = Session["EmpPermission"].ToString();
        DataTable dtTrans = objApprovalEmp.GetApprovalTransation(Session["CompId"].ToString());
        if (EmpAccessPermission == "2")
        {
            dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (EmpAccessPermission == "3")
        {
            dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (EmpAccessPermission == "4")
        {
            dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        dtTrans = new DataView(dtTrans, "Approval_Id='" + e.CommandArgument.ToString() + "' and Status='Pending' and  Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtTrans.Rows.Count > 0)
        {
            if (((LinkButton)sender).ID == "btnEdit")
            {
                if (dtTrans.Rows[0]["Approval_Type"].ToString() == "Priority")
                {
                    //hdnIsPendingApproval.Value = "true";
                    //DisplayMessage("Request is in under processing , You cannot edit this approval type");
                    dtTrans.Dispose();
                    //return;
                }
                if (dtTrans.Rows[0]["Approval_Type"].ToString() == "Hierarchy")
                {
                    rdopriority.Enabled = false;
                    rdoHierarchy.Enabled = false;
                    chkTeamLeader.Enabled = false;
                    chkDepartmentManager.Enabled = false;
                    chkParentDepartmentManager.Enabled = false;
                    txtResponsibeDepartmentName.Enabled = false;
                    DisplayMessage("Request is in under processing , You can edit only hierarchy rules");
                    dtTrans.Dispose();
                }
            }
        }
        if (((LinkButton)sender).ID == "btnEdit")
        {
            //btnNew.Text = Resources.Attendance.Edit;
            Lbl_Modal_Title.Text = Resources.Attendance.EditApprovalSetup;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
        }
        else
        {
          //  btnNew.Text = Resources.Attendance.View;
            Lbl_Modal_Title.Text = Resources.Attendance.ViewApprovalSetup;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
        }
        DataTable dtApproval = ObjApproval.GetApprovalMasterById(e.CommandArgument.ToString());
        if (dtApproval.Rows.Count > 0)
        {
            editid.Value = e.CommandArgument.ToString();
            rdopriority.Checked = false;
            rdoHierarchy.Checked = false;
            rdoOpen.Checked = true;
            rdorestricted.Checked = false;
            lblHeaderApprovalName.Text = e.CommandName.ToString();
            if (dtApproval.Rows[0]["Approval_Type"] != null)
            {
                if (dtApproval.Rows[0]["Approval_Type"].ToString() == "Priority")
                {
                    rdopriority.Checked = true;
                    rdoHierarchy.Checked = false;
                }
                else if (dtApproval.Rows[0]["Approval_Type"].ToString() == "Hierarchy")
                {
                    rdopriority.Checked = false;
                    rdoHierarchy.Checked = true;
                }
                if (dtApproval.Rows[0]["Is_Open"].ToString() == "True")
                {
                    rdoOpen.Checked = true;
                    rdorestricted.Checked = false;
                }
                else
                {
                    rdoOpen.Checked = false;
                    rdorestricted.Checked = true;
                }
            }
            else
            {
                rdopriority.Checked = false;
                rdoHierarchy.Checked = false;
            }
            rdoHierarchy_OnCheckedChanged(null, null);
            if (rdopriority.Checked)
            {
                editid.Value = e.CommandArgument.ToString();
                EmpAccessPermission = objSys.Get_Approval_Parameter_By_ID(e.CommandArgument.ToString()).Rows[0]["Approval_Level"].ToString();
                DataTable dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), "0", "0", "0", "1");
                if (EmpAccessPermission == "2")
                {
                    dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", "2");
                }
                else if (EmpAccessPermission == "3")
                {
                    dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "3");
                }
                else if (EmpAccessPermission == "4")
                {
                    dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "4");
                }
                dt = dt.DefaultView.ToTable(true, "Emp_id", "Emp_Code", "Emp_name", "Priority");
                objPageCmn.FillData((object)GvApprovalEmployeeDetail, dt, "", "");
            }
            else if (rdoHierarchy.Checked)
            {
                chkTeamLeader.Checked = Convert.ToBoolean(dtApproval.Rows[0]["Is_TeamLeader"].ToString());
                chkDepartmentManager.Checked = Convert.ToBoolean(dtApproval.Rows[0]["Is_DepartmentManager"].ToString());
                chkParentDepartmentManager.Checked = Convert.ToBoolean(dtApproval.Rows[0]["Is_Parent_departmentManager"].ToString());
                txtResponsibeDepartmentName.Text = dtApproval.Rows[0]["dep_Name"].ToString() + "/" + dtApproval.Rows[0]["ResponsibleDepartmentManager"].ToString();
            }
        }
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListDepName(string prefixText, int count, string contextKey)
    {
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DepartmentMaster objDepMaster = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        dt = Objda.return_DataTable("select Dep_Name,Dep_Id from set_departmentmaster where dep_name like '%" + prefixText.ToString().Trim() + "%' and isactive='True'");
        //dt = new DataView(dt, "Dep_Name like '%" + prefixText.ToString().Trim() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        return txt;
    }
    protected void rdoHierarchy_OnCheckedChanged(object sender, EventArgs e)
    {
        panelHerrachy.Visible = false;
        panelPriority.Visible = false;
        chkTeamLeader.Checked = false;
        chkDepartmentManager.Checked = false;
        chkParentDepartmentManager.Checked = false;
        txtResponsibeDepartmentName.Text = "";
        txtResponsiblePerson.Text = "";
        chkPriority.Checked = false;
        objPageCmn.FillData((object)GvApprovalEmployeeDetail, null, "", "");
        if (rdoHierarchy.Checked)
        {
            panelHerrachy.Visible = true;
            btnsaveConfig.ValidationGroup = "Save";
        }
        if (rdopriority.Checked)
        {
            panelPriority.Visible = true;
            btnsaveConfig.ValidationGroup = "H_Save";
        }
    }
    protected void txtDepName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtResponsibeDepartmentName.Text != "")
        {
            string strDepName = string.Empty;
            try
            {
                strDepName = txtResponsibeDepartmentName.Text.Trim().Split('/')[0].ToString();
            }
            catch
            {
                txtResponsibeDepartmentName.Text = "";
                DisplayMessage("Select department in suggestion only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtResponsibeDepartmentName);
                return;
            }
            DataTable dt = objDep.GetDepartmentMasterByDepName(strDepName);
            if (dt.Rows.Count == 0)
            {
                txtResponsibeDepartmentName.Text = "";
                DisplayMessage("Select department in suggestion only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtResponsibeDepartmentName);
                return;
            }
        }
    }
    public DataTable GetApprovalEmployee()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Emp_id");
        dt.Columns.Add("Emp_Code");
        dt.Columns.Add("Emp_name");
        dt.Columns.Add("Priority");
        foreach (GridViewRow gvrow in GvApprovalEmployeeDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            dr[0] = ((Label)gvrow.FindControl("lblEmpId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblgvEmployeeCode")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblgvEmployeeName")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblgvempPriority")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void imgBtnApprovalEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtemp = GetApprovalEmployee();
        dtemp = new DataView(dtemp, "Emp_id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)GvApprovalEmployeeDetail, dtemp, "", "");
        dtemp.Dispose();
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = "0";
        if (txtResponsiblePerson.Text != "")
        {
            try
            {
                empid = txtResponsiblePerson.Text.Split('/')[2].ToString();
            }
            catch
            {
                empid = "0";
            }
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
          
            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                DataTable dtTemp = new DataView(GetApprovalEmployee(), "Emp_Id=" + empid + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count > 0)
                {
                    chkPriority.Checked = Convert.ToBoolean(dtTemp.Rows[0]["Priority"].ToString());
                }
                else
                {
                }
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtResponsiblePerson.Text = "";
                txtResponsiblePerson.Focus();
                return;
            }
        }
    }
    protected void imgBtnApprovalEmployeeEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), e.CommandArgument.ToString());
        txtResponsiblePerson.Text = "" + dt.Rows[0]["Emp_Name"].ToString() + "/(" + dt.Rows[0]["Designation"].ToString() + ")/" + dt.Rows[0]["Emp_Code"].ToString() + "";
        txtEmpName_textChanged(null, null);
        dt.Dispose();
    }

    protected void btnAddAppprovalEmployee_Click(object sender, EventArgs e)
    {
        string EmployeeCode = string.Empty;
        if (txtResponsiblePerson.Text != "")
        {
            DataTable dt = GetApprovalEmployee();
            try
            {
                EmployeeCode = txtResponsiblePerson.Text.Split('/')[2].ToString();
            }
            catch
            {
                EmployeeCode = "0";
            }
            string strEmployeeeName = string.Empty;
            string strEmpId = string.Empty;
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Emp_Code='" + EmployeeCode + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                strEmpId = dtEmp.Rows[0]["Emp_Id"].ToString();
                strEmployeeeName = dtEmp.Rows[0]["Emp_Name"].ToString();
            }
            DataTable dtTemp = new DataView(dt, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count == 0)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = strEmpId;
                dt.Rows[dt.Rows.Count - 1][1] = EmployeeCode;
                dt.Rows[dt.Rows.Count - 1][2] = strEmployeeeName;
                dt.Rows[dt.Rows.Count - 1][3] = chkPriority.Checked;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == strEmpId)
                    {
                        dt.Rows[i][3] = chkPriority.Checked;
                        break;
                    }
                }
            }
            objPageCmn.FillData((object)GvApprovalEmployeeDetail, dt, "", "");
            txtResponsiblePerson.Text = "";
            chkPriority.Checked = false;
        }
        else
        {
            DisplayMessage("Enter Employeee Name");
            txtResponsiblePerson.Focus();
            return;
        }
    }
    protected void btnApprovalsaveConfig_Click(object sender, EventArgs e)
    {
        string strApprovalType = string.Empty;
        string strAuthorizeddepartment = "0";
        bool IsOpen = false;
        if (!rdoHierarchy.Checked && !rdopriority.Checked)
        {
            DisplayMessage("Select Process Type");
            return;
        }
        if (rdoHierarchy.Checked)
        {
            if (txtResponsibeDepartmentName.Text == "")
            {
                DisplayMessage("Enter authorized department");
                txtResponsibeDepartmentName.Focus();
                return;
            }
            else
            {
                strAuthorizeddepartment = txtResponsibeDepartmentName.Text.Split('/')[1].ToString();
            }
            strApprovalType = "Hierarchy";
        }
        if (rdopriority.Checked)
        {
            if (GvApprovalEmployeeDetail.Rows.Count == 0)
            {
                DisplayMessage("add approval person");
                return;
            }
            DataTable dttemp = GetApprovalEmployee();
            dttemp = new DataView(dttemp, "Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dttemp.Rows.Count == 0)
            {
                DisplayMessage("Priority not assigned");
                return;
            }
            strApprovalType = "Priority";
        }
        if (rdoOpen.Checked)
        {
            IsOpen = true;
        }
        string strsql = string.Empty;
        objDa.execute_Command("update set_approvalmaster set Approval_Type='" + strApprovalType + "',Is_TeamLeader='" + chkTeamLeader.Checked.ToString() + "',Is_DepartmentManager='" + chkDepartmentManager.Checked.ToString() + "',Is_Parent_departmentManager='" + chkParentDepartmentManager.Checked.ToString() + "',ResponsibleDepartmentManager='" + strAuthorizeddepartment + "',Is_Open='" + IsOpen.ToString() + "' where approval_id=" + editid.Value + "");
        string EmpAccessPermission = string.Empty;
        EmpAccessPermission = objSys.Get_Approval_Parameter_By_ID(editid.Value).Rows[0]["Approval_Level"].ToString();
        if (EmpAccessPermission == "1")
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), "0", "0", "0", "1");
        }
        else if (EmpAccessPermission == "2")
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", "2");
        }
        else if (EmpAccessPermission == "3")
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "3");
        }
        else
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "4");
        }
        if (rdopriority.Checked)
        {
            foreach (GridViewRow gvr in GvApprovalEmployeeDetail.Rows)
            {
                if (EmpAccessPermission == "1")
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), "0", "0", "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else if (EmpAccessPermission == "2")
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else if (EmpAccessPermission == "3")
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        objApprovalEmp.DeleteDuplicate_Record(EmpAccessPermission, editid.Value);
        //check pending approval list and update it with new configuration
        SqlTransaction trns = null;
        SqlConnection con = null;
        if (rdopriority.Checked)
        {

            try
            {
                //if (hdnIsPendingApproval.Value == "true")
                //{
                string whereClause = string.Empty;
                if (EmpAccessPermission == "1")
                {
                    whereClause = "Company_id=" + Session["CompId"].ToString();
                }
                else if (EmpAccessPermission == "2")
                {
                    whereClause = "Company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "'";
                }
                else if (EmpAccessPermission == "3")
                {
                    whereClause = "Company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "'";
                }
                else
                {
                    whereClause = "Company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "'";
                }
                string sql = "select * from Set_Approval_Transaction where " + whereClause + " and approval_id='" + editid.Value + "' and Status='Pending' and is_default='true' and IsActive='true'";
                DataTable dtPendings = objDa.return_DataTable(sql);
                if (dtPendings.Rows.Count > 0)
                {


                    con = new SqlConnection(Session["DBConnection"].ToString());
                    con.Open();
                    trns = con.BeginTransaction();
                    //update existing records to set is_active=false
                    string sql1 = "update Set_Approval_Transaction set IsActive='false' where trans_id in(select trans_id from Set_Approval_Transaction where " + whereClause + " and approval_id='" + editid.Value + "' and Status='Pending' and IsActive='true')";
                    objDa.execute_Command(sql1, ref trns);
                    int oldRefId = 0;
                    foreach (DataRow dr in dtPendings.Rows)
                    {
                        int newRefId = int.Parse(dr["Ref_id"].ToString());
                        if (oldRefId != newRefId)
                        {
                            foreach (GridViewRow gvr in GvApprovalEmployeeDetail.Rows)
                            {
                                objApprovalEmp.InsertApprovalTransaciton(dr["approval_id"].ToString(), dr["company_id"].ToString(), dr["brand_id"].ToString(), dr["location_id"].ToString(), dr["dep_id"].ToString(), dr["request_emp_id"].ToString(), dr["ref_id"].ToString(), ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, dr["request_date"].ToString(), dr["status_update_date"].ToString(), dr["status"].ToString(), dr["description"].ToString(), dr["field1"].ToString(), dr["field2"].ToString(), dr["field3"].ToString(), dr["field4"].ToString(), dr["field5"].ToString(), dr["field6"].ToString(), dr["field7"].ToString(), true.ToString(), dr["createdBy"].ToString(), dr["CreatedDate"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        oldRefId = newRefId;
                    }
                    trns.Commit();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                if (trns != null)
                {
                    trns.Rollback();
                }
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        DisplayMessage("Record Updated Successfully", "green");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
    }
    private void FillGrid()
    {
        DataTable dtBrand = ObjApproval.GetApprovalMaster();
       // lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtApproval"] = dtBrand;
        Session["dtFilter_Appr__Master"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GvApproval, dtBrand, "", "");
            //AllPageCode();
        }
        else
        {
            GvApproval.DataSource = null;
            GvApproval.DataBind();
        }
    }
    #endregion















}