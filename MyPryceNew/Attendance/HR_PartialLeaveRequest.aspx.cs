using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.IO;

public partial class Attendance_HR_PartialLeaveRequest : BasePage
{
    Att_AttendanceLog objAttLog = null;
    SystemParameter objSys = null;
    Att_ScheduleMaster objEmpSch = null;
    EmployeeMaster objEmp = null;
    RoleDataPermission objRoleData = null;
    Set_Approval_Employee objApproalEmp = null;
    Att_PartialLeave_Request objPartial = null;
    EmployeeParameter objEmpParam = null;
    Set_ApplicationParameter objAppParam = null;
    Common cmn = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    Set_Employee_Holiday objEmpHoliday = null;
    Att_HalfDay_Request objHalfDay = null;
    Att_Leave_Request objleaveReq = null;
    Set_ApprovalMaster ObjApproval = null;
    SendMailSms ObjSendMailSms = null;
    CompanyMaster objComp = null;
    NotificationMaster Obj_Notifiacation = null;
    DataAccessClass daClass = null;
    string strPLWithTimeWithOutTime = string.Empty;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        btnApply.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnApply, "").ToString());

        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        daClass = new DataAccessClass(Session["DBConnection"].ToString());

        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Session["empimgpath"] = null;
            Session["empimgpathFull"] = null;
            string strPage = string.Empty;
            if (Request.QueryString["Emp_Id"] != null)
            {
                strPage = "../Attendance/HR_PartialLeaveRequest.aspx?emp_id=0";
            }
            else
            {
                strPage = "../Attendance/HR_PartialLeaveRequest.aspx";
            }

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission(strPage, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            setYearList();
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLoc);
            rbtnOfficial.Focus();
            FillLeaveStatus();
            FillddlDeaprtment();
            FillddlLocation();
            FillGvEmp();
            txtApplyDate.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat());
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("PL Date Editable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString())))
            {
                txtApplyDate.Enabled = false;
            }


            if (Request.QueryString["Emp_Id"] != null)
            {

                DataTable dt = Common.GetEmployee(Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Session["DBConnection"].ToString());

                dt = new DataView(dt, "Emp_Id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                if (dt.Rows.Count > 0)
                {
                    txtEmpName.Text = "" + dt.Rows[0][1].ToString() + "/(" + dt.Rows[0][2].ToString() + ")/" + dt.Rows[0][0].ToString() + "";
                }

                //txtEmpName.Text = Common.GetEmployeeName(Session["EmpId"].ToString());
                rbtnPersonal.Checked = true;
                rbtnOfficial.Checked = false;
                hdnEmpId.Value = Session["EmpId"].ToString();
                txtEmpName_textChanged(null, null);
                txtEmpName.Enabled = false;
                //rbtnPersonal.Visible = false;
                //rbtnOfficial.Visible = false;

            }


        }
        CalendarExtender2.Format = objSys.SetDateFormat();
        //AllPageCode();

    }
    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {
                    FillGvEmp();
                }
                else
                {

                    dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

                    if (ddlLocation.SelectedIndex == 0)
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    }

                    if (dpDepartment.SelectedIndex != 0)
                    {
                        try
                        {
                            dt = new DataView(dt, "Department_Id=" + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 22-05-2015
                        objPageCmn.FillData((object)GvEmpList, dt, "", "");
                        Session["Partial_DtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                    }
                    else
                    {
                        GvEmpList.DataSource = null;
                        GvEmpList.DataBind();
                        Session["Partial_DtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }

        ImageButton1.Focus();
    }
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {
                    FillGvEmp();
                }
                else
                {

                    dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

                    if (ddlLocation.SelectedIndex == 0)
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    if (Session["SessionDepId"] != null)
                    {
                        dt = new DataView(dt, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                    }
                    if (dpDepartment.SelectedIndex != 0)
                    {
                        try
                        {
                            dt = new DataView(dt, "Department_Id=" + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }

                    if (dt.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 22-05-2015
                        objPageCmn.FillData((object)GvEmpList, dt, "", "");
                        Session["Partial_DtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                    }
                    else
                    {
                        GvEmpList.DataSource = null;
                        GvEmpList.DataBind();
                        Session["Partial_DtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }
        dpDepartment.Focus();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "$('#myModal').modal('toggle');", "", true);
        FillEmpLeave(hdnEmpId.Value);

    }
    private void FillddlLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }


    private void FillddlDeaprtment()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)dpDepartment, dt, "DeptName", "Dep_Id");
        }
        else
        {
            try
            {
                dpDepartment.Items.Clear();
                dpDepartment.DataSource = null;
                dpDepartment.DataBind();
                dpDepartment.Items.Insert(0, "--Select--");
                dpDepartment.SelectedIndex = 0;
            }
            catch
            {
                dpDepartment.Items.Insert(0, "--Select--");
                dpDepartment.SelectedIndex = 0;
            }
        }
    }




    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGvEmp();
        ImageButton1.Focus();

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


        if (Request.QueryString["Emp_Id"] != null)
        {
            dtEmp = new DataView(dtEmp, "Emp_Id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }


        Session["Partial_DtEmp"] = dtEmp;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpList, dtEmp, "", "");
        Session["CHECKED_ITEMS"] = null;
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvEmpList.HeaderRow.FindControl("chkgvSelectAll"));

        foreach (GridViewRow gvrow in GvEmpList.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvEmpList.Rows)
            {
                int index = (int)GvEmpList.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvEmpList.Rows)
        {
            index = (int)GvEmpList.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void GvEmpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvEmpList.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpList, (DataTable)Session["Partial_DtEmp"], "", "");
        PopulateCheckedValues();
    }
    protected void btnarybind_Click1(object sender, EventArgs e)
    {
        //// I1.Attributes.Add("Class", "fa fa-minus");
        // Div1.Attributes.Add("Class", "box box-primary");
        if (txtValue.Text.Trim() == "")
        {
            txtValue.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";

            }
            DataTable dtEmp = (DataTable)Session["Partial_DtEmp"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvEmpList, view.ToTable(), "", "");
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            Session["CHECKED_ITEMS"] = null;
        }
        txtValue.Focus();
    }
    protected void btnaryRefresh_Click1(object sender, EventArgs e)
    {
        ////// I1.Attributes.Add("Class", "fa fa-minus");
        //    Div1.Attributes.Add("Class", "box box-primary");
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";

        FillGvEmp();
        Session["Partial_DtLeave"] = null;
        ViewState["Select"] = null;
        // lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        ddlField.Focus();
        Session["CHECKED_ITEMS"] = null;

    }
    protected void ImgbtnSelectAll_Clickary(object sender, EventArgs e)
    {
        //I1.Attributes.Add("Class", "fa fa-minus");
        // Div1.Attributes.Add("Class", "box box-primary");
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["Partial_DtEmp"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in GvEmpList.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["Partial_DtEmp"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)GvEmpList, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void rbtnPersonal_CheckedChanged(Object sender, EventArgs e)
    {
        dvOfficial.Visible = false;
        dvEmp.Visible = false;
        txtEmpName.Focus();
        GvEmpList.Visible = false;
        dvEmp.Visible = true;
        // A1.Visible = true;
        // valid.Visible = true;
        txtEmpName.Visible = true;
        Label3.Visible = true;
        gvLeaveSummary_PartialLeave.Visible = true;
        gvEmpPendingLeave.Visible = true;

    }
    protected void rbtnOfficial_CheckedChanged(Object sender, EventArgs e)
    {
        GvEmpList.Visible = true;
        dvEmp.Visible = false;
        dvOfficial.Visible = true;
        Session["CHECKED_ITEMS"] = null;
        gvLeaveSummary_PartialLeave.Visible = false;
        gvEmpPendingLeave.Visible = false;
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnApply.Visible = true;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
    }
    public string GetDate(object obj)
    {

        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //AllPageCode();
        dvEmp.Visible = true;
        GvEmpList.Visible = true;
        rbtnOfficial.Checked = true;
        rbtnOfficial_CheckedChanged(null, null);
        rbtnPersonal.Checked = true;
        rbtnOfficial.Checked = false;
        rbtnPersonal_CheckedChanged(null, null);
        try
        {
            GvEmpList.HeaderRow.Cells[0].Focus();
        }
        catch
        {
        }
    }
    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;

        if (txtEmpName.Text != "")
        {
            empid = txtEmpName.Text.Split('/')[txtEmpName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            try
            {
                dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmp.Rows.Count > 0)
                {
                    empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                    hdnEmpId.Value = empid;
                    FillEmpLeave(hdnEmpId.Value);
                    if (rbtnPersonal.Checked == true)
                    {
                        try
                        {
                            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                            {
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
                        rbBegining_CheckedChanged(sender, e);
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
            catch
            {
                DisplayMessage("Employee Not Exists");
                txtEmpName.Text = "";
                txtEmpName.Focus();
                hdnEmpId.Value = "";
            }
        }




    }
    private string GetTime24(string timepart)
    {
        string str = "00:00";
        DateTime date = Convert.ToDateTime(timepart);
        str = date.ToString("HH:mm");
        return str;
    }



    protected void btnGetTimings_Click(object sender, EventArgs e)
    {


        string PostedEmpList = string.Empty;
        string NonPostedLog = string.Empty;
        string empidlist = string.Empty;

        if (rbtnOfficial.Checked == true)
        {

            ArrayList userdetails = new ArrayList();
            SaveCheckedValues();

            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select Employee First");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
                else
                {

                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        empidlist += userdetails[i].ToString() + ",";
                    }
                }
                empidlist = empidlist.Substring(0, empidlist.Length - 1);
            }
        }
        else
        {
            empidlist = hdnEmpId.Value.ToString();

        }

        try
        {


            if (empidlist.Length == 0)
            {
                DisplayMessage("Select Employee!");
                return;
            }
        }
        catch
        {
            DisplayMessage("Select Employee!");
            return;
        }
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


        //DataTable dtAttendanceRecord = daClass.return_DataTable("Select Att_AttendanceRegister .Emp_Id , Att_AttendanceRegister .Att_Date ,  Att_AttendanceRegister .OnDuty_Time ,Att_AttendanceRegister .OffDuty_Time , Att_AttendanceRegister .In_Time ,Att_AttendanceRegister .Out_Time , LateMin , EarlyMin , (SELECT TOP 1 Cast(Cast(event_time AS TIME) AS VARCHAR(5))                     FROM   att_attendancelog                     WHERE  emp_id = att_attendanceregister.Emp_Id                             AND event_date = Cast(att_attendanceregister.att_date                                                  AS                                                  DATE                                             )                            AND func_code = '2'                            AND att_attendanceregister.emp_id =                                att_attendancelog.emp_id                     ORDER  BY event_time ASC)                    AS SIn,                    (SELECT TOP 1 Cast(Cast(event_time AS TIME) AS VARCHAR(5))                     FROM   att_attendancelog                      WHERE  emp_id =  att_attendanceregister.Emp_Id                             AND event_date = Cast(att_attendanceregister.att_date                                                  AS                                                  DATE                                             )                            AND func_code = '2'                            AND att_attendanceregister.emp_id =                                att_attendancelog.emp_id                     ORDER  BY event_time DESC)                    AS SOut    From   Att_AttendanceRegister where Att_AttendanceRegister.Emp_Id = "+ empidlist +" and Att_AttendanceRegister.Att_Date ='"+ txtApplyDate.Text  + "'             ");

        //if(dtAttendanceRecord.Rows.Count > 0)
        //{
        //    if(Convert.ToInt32(dtAttendanceRecord.Rows[0]["LateMin"].ToString()) > 0)
        //    {
        //        txtInTime.Text = Convert.ToDateTime(dtAttendanceRecord.Rows[0]["OnDuty_Time"].ToString()).ToString("HH:mm");
        //        txtOuttime.Text = Convert.ToDateTime(dtAttendanceRecord.Rows[0]["In_Time"].ToString()).AddMinutes(-1).ToString("HH:mm");
        //    }
        //    else if (Convert.ToInt32(dtAttendanceRecord.Rows[0]["EarlyMin"].ToString()) > 0)
        //    {
        //        txtInTime.Text = Convert.ToDateTime(dtAttendanceRecord.Rows[0]["Out_Time"].ToString()).AddMinutes(1).ToString("HH:mm");
        //        txtOuttime.Text = Convert.ToDateTime(dtAttendanceRecord.Rows[0]["OffDuty_Time"].ToString()).ToString("HH:mm");
        //    }
        //    else
        //    {

        //    }

        //}
        //else
        //{
        //    DisplayMessage("No Entry Found");
        //}




        DataTable dtLog = daClass.return_DataTable("Select  * From   Att_AttendanceLog Where   Emp_Id =  '" + empidlist + "'  and Event_Date =  '" + txtApplyDate.Text + "' Order by   Event_Time");
        DataTable dtShiftInfo = daClass.return_DataTable("Select  Att_TimeTable.OnDuty_Time ,Att_TimeTable.OffDuty_Time ,Att_TimeTable.Late_Min ,Att_TimeTable.Early_Min From  Att_ScheduleDescription   INNER JOIN  Att_TimeTable ON Att_TimeTable.TimeTable_Id = Att_ScheduleDescription.TimeTable_Id   Where   Emp_Id =  '" + empidlist + "'  and Att_Date  =  '" + txtApplyDate.Text + "'  ");

        if (dtShiftInfo.Rows.Count > 0)
        {
            int count = 0;
            DateTime tApply = objSys.getDateForInput(txtApplyDate.Text);
            bool bTwoDay = false;
            bool bParital = false;
            DateTime tOnTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OnDuty_Time"]);
            DateTime tOffTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OffDuty_Time"]);
            DateTime tAOnTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OnDuty_Time"]);
            DateTime tAOffTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OffDuty_Time"]);

            if (tOnTime > tOffTime)
            {
                bTwoDay = true;
            }
            tAOnTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOnTime.Hour, tOnTime.Minute, 0);
            tOnTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOnTime.Hour, tOnTime.Minute, 0).AddMinutes(Convert.ToInt16((dtShiftInfo.Rows[0]["Late_Min"])));

            if (bTwoDay)
            {
                DateTime tApply1 = tApply.AddDays(1);
                tAOffTime = new DateTime(tApply1.Year, tApply1.Month, tApply1.Day, tOffTime.Hour, tOffTime.Minute, 0);
                tOffTime = new DateTime(tApply1.Year, tApply1.Month, tApply1.Day, tOffTime.Hour, tOffTime.Minute, 0).AddMinutes(Convert.ToInt16((dtShiftInfo.Rows[0]["Early_Min"])) * -1);

            }
            else
            {
                tAOffTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOffTime.Hour, tOffTime.Minute, 0);
                tOffTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOffTime.Hour, tOffTime.Minute, 0).AddMinutes(Convert.ToInt16((dtShiftInfo.Rows[0]["Early_Min"])) * -1);

            }


            DataTable dtLogNormal = new DataView(dtLog, "func_code<>2", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtLogPartial = new DataView(dtLog, "func_code=2", "", DataViewRowState.CurrentRows).ToTable();

            count = dtLogNormal.Rows.Count;
            switch (count)
            {
                // if count 1  means we need to check late
                // if two then check late first and  then early
                case 1:
                    if (Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]) > tOnTime)
                    {
                        txtInTime.Text = tAOnTime.ToString("HH:mm");
                        txtOuttime.Text = Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");
                        bParital = true;
                        rbtnDIN.Checked = true;
                    }

                    break;
                case 2:
                    if (Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]) > tOnTime)
                    {
                        txtInTime.Text = tAOnTime.ToString("HH:mm");
                        txtOuttime.Text = Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");
                        bParital = true;
                        rbtnDIN.Checked = true;
                    }
                    else
                    {
                        if (Convert.ToDateTime(dtLogNormal.Rows[1]["Event_Time"]) < tOffTime)
                        {
                            txtInTime.Text = Convert.ToDateTime(dtLogNormal.Rows[1]["Event_Time"]).AddMinutes(1).ToString("HH:mm");
                            txtOuttime.Text = tAOffTime.ToString("HH:mm");
                            bParital = true;
                            rbtnDout.Checked = true;
                        }
                    }
                    break;
                default:
                    if (Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]) > tOnTime)
                    {
                        txtInTime.Text = tAOnTime.ToString("HH:mm");
                        txtOuttime.Text = Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");
                        bParital = true;
                        rbtnDIN.Checked = true;
                    }
                    else
                    {
                        if (Convert.ToDateTime(dtLogNormal.Rows[dtLogNormal.Rows.Count - 1]["Event_Time"]) < tOffTime)
                        {
                            txtInTime.Text = Convert.ToDateTime(dtLogNormal.Rows[dtLogNormal.Rows.Count - 1]["Event_Time"]).AddMinutes(1).ToString("HH:mm");
                            txtOuttime.Text = tAOffTime.ToString("HH:mm");
                            bParital = true;
                            rbtnDout.Checked = true;
                        }
                    }
                    break;
            }

            if (!bParital)
            {
                if (dtLogPartial.Rows.Count == 2)
                {
                    txtInTime.Text = Convert.ToDateTime(dtLogPartial.Rows[0]["Event_Time"]).AddMinutes(1).ToString("HH:mm");
                    txtOuttime.Text = Convert.ToDateTime(dtLogPartial.Rows[1]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");
                    bParital = true;
                    rbtnBetween.Checked = true;

                }
            }
            if (!bParital)
            {
                DisplayMessage("Partial Leave Not Found!");
            }


        }
        else
        {
            DisplayMessage("Shift Not Found!");
        }

    }

    //BUTTON APPLY 
    protected void btnApply_Click(object sender, EventArgs e)
    {
        if (hdnEdit.Value == "" && rbtnPersonal.Checked)
        {

            if (gvLeaveSummary_PartialLeave.Rows.Count == 0)
            {
                DisplayMessage("Partial Leave not assigned");
                return;
            }

            foreach (GridViewRow gvrow in gvLeaveSummary_PartialLeave.Rows)
            {

                if (Convert.ToInt32(((Label)gvrow.FindControl("lblRemainingDays")).Text) <= 0)
                {
                    DisplayMessage("You do not have Sufficient Leave");
                    return;
                }

            }



        }






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

        string OnDutyTime = string.Empty;
        string OffDutyTime = string.Empty;
        string strPLType = string.Empty;

        int Count = 0;
        int b = 0;
        string strPartialMethod = "";
        if (rbtnDIN.Checked == false && rbtnDout.Checked == false && rbtnBetween.Checked == false)
        {
            DisplayMessage("Please select partial method");
            rbtnPersonal.Focus();
            return;
        }
        else
        {
            if (rbtnBetween.Checked)
            {
                strPartialMethod = "In Between";
            }
            else if (rbtnDIN.Checked)
            {
                strPartialMethod = "Direct In";
            }
            else if (rbtnDout.Checked)
            {
                strPartialMethod = "Direct Out";
            }
        }



        if (rbtnOfficial.Checked == false && rbtnPersonal.Checked == false)
        {
            DisplayMessage("Please select Personal or Official");
            rbtnPersonal.Focus();
            return;
        }

        if (txtEmpName.Text == "" && rbtnPersonal.Checked == true)
        {
            DisplayMessage("Enter Employee Name");
            txtEmpName.Focus();
            return;
        }
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
                if (DateTime.Parse(txtApplyDate.Text) == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date)
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

        //if (rbtnOfficial.Checked == false && rbtnPersonal.Checked == true)
        //{
        //if (strPLWithTimeWithOutTime == "False")
        //{
        //    if (rbBegining.Checked == true || rbMiddle.Checked == true || rbEnding.Checked == true)
        //    {
        //        if (rbBegining.Checked == true)
        //        {
        //            strPLType = "B";
        //        }
        //        else if (rbMiddle.Checked == true)
        //        {
        //            strPLType = "M";
        //        }
        //        else if (rbEnding.Checked == true)
        //        {
        //            strPLType = "E";
        //        }
        //    }
        //    else
        //    {
        //        DisplayMessage("Select Partial Leave Type");
        //        return;
        //    }

        //    if (rbTimeTable.SelectedValue == "0" || rbTimeTable.SelectedValue == "")
        //    {
        //        DisplayMessage("Choose Time Table");
        //        rbTimeTable.Focus();
        //        return;
        //    }
        //}
        //else if (strPLWithTimeWithOutTime == "True")
        //{
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

        try
        {
            if (Convert.ToDateTime(txtInTime.Text) > Convert.ToDateTime(txtOuttime.Text))
            {
                DisplayMessage("To time should be gerater then From time ");
                txtInTime.Focus();
                return;
            }
        }
        catch
        {

        }

        //}
        //}
        //else if (rbtnOfficial.Checked == true && rbtnPersonal.Checked == false)
        //{
        //    if (txtInTime.Text == "")
        //    {
        //        DisplayMessage("Enter In Time");
        //        txtInTime.Focus();
        //        return;
        //    }
        //    if (txtOuttime.Text == "")
        //    {
        //        DisplayMessage("Enter Out Time");
        //        txtOuttime.Focus();
        //        return;
        //    }
        //}

        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }

        if (rbtnPersonal.Checked == true)
        {
            //this code is created by jitendra upadhyay on 10-09-2014
            //this code for check posted month before leave request
            //code start
            int Monthposted = objSys.getDateForInput(txtApplyDate.Text.ToString()).Month; ;
            //int Month = Convert.ToDateTime(txtFrom.Text).ToString();
            int Yearposted = Convert.ToDateTime(txtApplyDate.Text).Year;
            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(hdnEmpId.Value, Monthposted.ToString(), Yearposted.ToString());

            if (dtPostedList.Rows.Count > 0)
            {
                DisplayMessage("Log Posted For This Date Criteria");
                return;
            }
            //code end
        }


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

        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Description");
            txtDescription.Focus();
            return;
        }

        if (rbtnOfficial.Checked == true)
        {

            ArrayList userdetails = new ArrayList();
            SaveCheckedValues();

            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select Employee First");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
                else
                {

                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        hdnEmpId.Value = userdetails[i].ToString();
                    }
                }

            }
        }

        DataTable dtPartialLeave = new DataTable();
        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("PartialLeave").Rows[0]["Approval_Level"].ToString();
        dtPartialLeave = objApproalEmp.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "68", hdnEmpId.Value);

        if (dtPartialLeave.Rows.Count == 0)
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }


        if (rbtnPersonal.Checked)
        {

            if (strPLWithTimeWithOutTime == "True")
            {
                //Code For With Time PL
                if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())) == false)
                {
                    //
                    // Check Holiday or Not For Leave Apply For the Day...............
                    DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    DateTime fromdate2 = objSys.getDateForInput(txtApplyDate.Text.ToString());

                    DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + fromdate2.ToString() + "' and Emp_Id='" + hdnEmpId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtHoliday1.Rows.Count > 0)
                    {
                        DisplayMessage("Employee has Holiday on Date " + fromdate2.ToString("dd-MMM-yyyy") + " so cannot apply");
                        txtDescription.Text = string.Empty;
                        return;
                    }
                }
                // Holiday Code Over..............

                //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
                if (hdnEmpId.Value != "")
                {
                    if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())) == false)
                    {
                        DataTable dtSch = objEmpSch.GetSheduleDescription(hdnEmpId.Value);
                        // Modified By Nitin jain, Date 09-07-2014 Check If Week Off Or Not For Leave Apply Date....
                        DataTable dtSch1 = new DataView(dtSch, "Att_Date='" + txtApplyDate.Text + "' and Emp_Id='" + hdnEmpId.Value + "' and Is_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtSch1.Rows.Count > 0)
                        {
                            DisplayMessage("Employee has Week off on Date " + DateFormat(txtApplyDate.Text) + " so cannot apply");
                            return;
                        }
                    }
                }

                //
                txtEmpName.Focus();
                DataTable dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());

                dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Partial_Leave_Date='" + Convert.ToDateTime(txtApplyDate.Text) + "' and Emp_Id=" + hdnEmpId.Value + " and Partial_Leave_Type='0' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    dtLeave = new DataView(dtLeave, "Is_Confirmed ='Canceled'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtLeave.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        DisplayMessage("You Have Already Apply Partial Leave For Date : " + DateFormat(txtApplyDate.Text) + "");
                        return;
                    }
                }

                //code for check the half day and full day leave for same date
                //code start
                DateTime HalfDayInDate = objSys.getDateForInput(txtApplyDate.Text.ToString());

                DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
                dtHalfDay = new DataView(dtHalfDay, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id='" + hdnEmpId.Value + "' and HalfDay_Date='" + HalfDayInDate + "'", "", DataViewRowState.CurrentRows).ToTable();

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

                DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
                DateTime fromdate1 = objSys.getDateForInput(txtApplyDate.Text);

                DataTable dtLeaveReq2 = new DataView(dtLeaveR, "From_Date <='" + fromdate1.ToString() + "' and To_Date>='" + fromdate1.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtLeaveReq2.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtLeaveReq2.Rows[0]["Is_Canceled"].ToString()) == false)
                    {
                        DisplayMessage("You have Already Apply Leave For Date : " + DateFormat(txtApplyDate.Text) + "");
                        return;
                    }
                }
                //code end

                bool IsCompPartial = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                if (IsCompPartial)
                {
                    DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpId.Value, Session["CompId"].ToString());
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

                                                string FileName = "";
                                                string FileUrl = "";
                                                try
                                                {
                                                    FileName = Session["empimgpath"].ToString();
                                                    FileUrl = Session["empimgpathFull"].ToString();
                                                }
                                                catch
                                                {
                                                    FileName = "";
                                                    FileUrl = "";
                                                }
                                                b = objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), hdnEmpId.Value, "0", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", strPartialMethod, true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), FileName, FileUrl);

                                                if (b != 0)
                                                {
                                                    strTransId = b.ToString();
                                                    if (dtPartialLeave.Rows.Count > 0)
                                                    {
                                                        for (int j = 0; j < dtPartialLeave.Rows.Count; j++)
                                                        {
                                                            int cur_trans_id = 0;
                                                            string PriorityEmpId = dtPartialLeave.Rows[j]["Emp_Id"].ToString();
                                                            string IsPriority = dtPartialLeave.Rows[j]["Priority"].ToString();

                                                            if (EmpPermission == "1")
                                                            {
                                                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), "0", "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", strPartialMethod, true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                            }
                                                            else if (EmpPermission == "2")
                                                            {
                                                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                            }
                                                            else if (EmpPermission == "3")
                                                            {
                                                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                            }
                                                            else
                                                            {
                                                                cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                            }

                                                            // Insert Notification For Leave by  ghanshyam suthar
                                                            Session["PriorityEmpId"] = PriorityEmpId;
                                                            Session["cur_trans_id"] = cur_trans_id;
                                                            Set_Notification();

                                                            //for Email Code

                                                            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                                                            {

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

                                                                            string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + txtApplyDate.Text + "<br />From Time :" + txtInTime.Text + "  To Time :" + txtOuttime.Text + "<br /> Reason For Leave : " + txtDescription.Text + "" + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";

                                                                            //string MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>'" + Common.GetEmployeeName(hdnEmpId.Value) + "'Apply for Partial Leave 'Personal'<br />On Date '" + txtApplyDate.Text + "' Timing is From '" + txtInTime.Text + "' To '" + txtOuttime.Text + "' <br /> and Given Description is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                                                            ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Personal Partial Leave Apply By'" + Common.GetEmployeeName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On Date '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    DisplayMessage("Partial leave submitted");
                                                    btnReset_Click(null, null);
                                                    FillLeaveStatus();
                                                }
                                            }
                                            else
                                            {
                                                b = objPartial.UpdatePartialLeaveRequest(hdnEdit.Value, Session["CompId"].ToString(), hdnEmpId.Value, "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                                if (b != 0)
                                                {
                                                    if (dtPartialLeave.Rows.Count > 0)
                                                    {
                                                    }
                                                    Session["PriorityEmpId"] = hdnEmpId.Value;
                                                    Session["cur_trans_id"] = hdnEdit.Value;
                                                    Set_Notification();
                                                    DisplayMessage("Partial leave updated");
                                                    btnCancel_Click(null, null);
                                                    FillLeaveStatus();
                                                    hdnEdit.Value = "";
                                                    hdnEditDate.Value = "0";
                                                    Lbl_Tab_New.Text = Resources.Attendance.New;
                                                    txtEmpName.Enabled = true;
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
                //End
            }
            else if (strPLWithTimeWithOutTime == "False")
            {
                //Code For WithOut Time PL
                if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())) == false)
                {
                    //
                    // Check Holiday or Not For Leave Apply For the Day...............
                    DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    DateTime fromdate2 = objSys.getDateForInput(txtApplyDate.Text.ToString());

                    DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + fromdate2.ToString() + "' and Emp_Id='" + hdnEmpId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtHoliday1.Rows.Count > 0)
                    {
                        DisplayMessage("Employee has Holiday on Date " + fromdate2.ToString("dd-MMM-yyyy") + " so cannot apply");
                        txtDescription.Text = string.Empty;
                        return;
                    }
                }
                // Holiday Code Over..............

                //  Modified By Nitin jain, Date 09-07-2014 Get Shift Assign On That Day For Leave Apply By Employee.....
                if (hdnEmpId.Value != "")
                {
                    if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())) == false)
                    {
                        DataTable dtSch = objEmpSch.GetSheduleDescription(hdnEmpId.Value);
                        // Modified By Nitin jain, Date 09-07-2014 Check If Week Off Or Not For Leave Apply Date....
                        DataTable dtSch1 = new DataView(dtSch, "Att_Date='" + txtApplyDate.Text + "' and Emp_Id='" + hdnEmpId.Value + "' and Is_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtSch1.Rows.Count > 0)
                        {
                            DisplayMessage("Employee has Week off on Date " + DateFormat(txtApplyDate.Text) + " so cannot apply");
                            return;
                        }
                    }
                }

                //
                txtEmpName.Focus();
                DataTable dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());

                dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Partial_Leave_Date='" + Convert.ToDateTime(txtApplyDate.Text) + "' and Emp_Id=" + hdnEmpId.Value + " and Partial_Leave_Type='0' ", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeave.Rows.Count > 0)
                {
                    dtLeave = new DataView(dtLeave, "Is_Confirmed ='Canceled'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtLeave.Rows.Count > 0)
                    {

                    }
                    else
                    {
                        DisplayMessage("You Have Already Apply Partial Leave For Date : " + DateFormat(txtApplyDate.Text) + "");
                        return;
                    }
                }

                //code for check the half day and full day leave for same date
                //code start
                DateTime HalfDayInDate = objSys.getDateForInput(txtApplyDate.Text.ToString());

                DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
                dtHalfDay = new DataView(dtHalfDay, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id='" + hdnEmpId.Value + "' and HalfDay_Date='" + HalfDayInDate + "'", "", DataViewRowState.CurrentRows).ToTable();

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

                DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
                DateTime fromdate1 = objSys.getDateForInput(txtApplyDate.Text);

                DataTable dtLeaveReq2 = new DataView(dtLeaveR, "From_Date <='" + fromdate1.ToString() + "' and To_Date>='" + fromdate1.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtLeaveReq2.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dtLeaveReq2.Rows[0]["Is_Canceled"].ToString()) == false)
                    {
                        DisplayMessage("You have Already Apply Leave For Date : " + DateFormat(txtApplyDate.Text) + "");
                        return;
                    }
                }
                //code end

                bool IsCompPartial = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                if (IsCompPartial)
                {
                    DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpId.Value, Session["CompId"].ToString());
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

                                        string FileName = "";
                                        string FileUrl = "";
                                        try
                                        {
                                            FileName = Session["empimgpath"].ToString();
                                            FileUrl = Session["empimgpathFull"].ToString();
                                        }
                                        catch
                                        {
                                            FileName = "";
                                            FileUrl = "";
                                        }
                                        b = objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), hdnEmpId.Value, "0", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", strPLType, rbTimeTable.SelectedValue.ToString(), "", "", strPartialMethod, true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), FileName, FileUrl);


                                        if (b != 0)
                                        {
                                            strTransId = b.ToString();
                                            if (dtPartialLeave.Rows.Count > 0)
                                            {
                                                for (int j = 0; j < dtPartialLeave.Rows.Count; j++)
                                                {
                                                    int cur_trans_id = 0;
                                                    string PriorityEmpId = dtPartialLeave.Rows[j]["Emp_Id"].ToString();
                                                    string IsPriority = dtPartialLeave.Rows[j]["Priority"].ToString();
                                                    if (EmpPermission == "1")
                                                    {
                                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), "0", "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                    }
                                                    else if (EmpPermission == "2")
                                                    {
                                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                    }
                                                    else if (EmpPermission == "3")
                                                    {
                                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                    }
                                                    else
                                                    {
                                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", hdnEmpId.Value.ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                    }

                                                    // Insert Notification For Leave by  ghanshyam suthar
                                                    Session["PriorityEmpId"] = PriorityEmpId;
                                                    Session["cur_trans_id"] = cur_trans_id;
                                                    Set_Notification();
                                                    //--------------------------------------

                                                    //for Email Code
                                                    if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                                                    {
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

                                                                    string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + txtApplyDate.Text + "<br />From Time :" + txtInTime.Text + "  To Time :" + txtOuttime.Text + "<br /> Reason For Leave : " + txtDescription.Text + " " + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";

                                                                    //string MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>'" + Common.GetEmployeeName(hdnEmpId.Value) + "'Apply for Partial Leave 'Personal'<br />On Date '" + txtApplyDate.Text + "' Timing is From '" + txtInTime.Text + "' To '" + txtOuttime.Text + "' <br /> and Given Description is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                                                    ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Personal Partial Leave Apply By'" + Common.GetEmployeeName(hdnEmpId.Value, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On Date '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                                                }
                                                            }
                                                        }
                                                    }



                                                }
                                            }
                                            DisplayMessage("Partial leave submitted");
                                            btnReset_Click(null, null);
                                            FillLeaveStatus();
                                        }
                                    }
                                    else
                                    {
                                        b = objPartial.UpdatePartialLeaveRequest(hdnEdit.Value, Session["CompId"].ToString(), hdnEmpId.Value, "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", strPLType, rbTimeTable.SelectedValue.ToString(), "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                        if (b != 0)
                                        {
                                            if (dtPartialLeave.Rows.Count > 0)
                                            {
                                            }
                                            Session["PriorityEmpId"] = hdnEmpId.Value;
                                            Session["cur_trans_id"] = hdnEdit.Value;
                                            Set_Notification();
                                            DisplayMessage("Partial leave updated");
                                            btnCancel_Click(null, null);
                                            FillLeaveStatus();
                                            hdnEdit.Value = "";
                                            hdnEditDate.Value = "0";
                                            Lbl_Tab_New.Text = Resources.Attendance.New;
                                            txtEmpName.Enabled = true;
                                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                                        }
                                    }
                                    //}
                                    //}
                                    //else
                                    //{
                                    //    DisplayMessage("Employee does not have sufficient balance");
                                    //    return;
                                    //}
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
                //End
            }
        }
        else
        {
            //Strat Code For Official Partial Leave
            string PostedEmpList = string.Empty;
            string NonPostedLog = string.Empty;
            string empidlist = string.Empty;

            ArrayList userdetails = new ArrayList();
            SaveCheckedValues();

            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select Employee First");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
                else
                {

                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        empidlist += userdetails[i].ToString() + ",";
                    }
                }
            }

            for (int i = 0; i < empidlist.Split(',').Length; i++)
            {

                if (empidlist.Split(',')[i] == "")
                {
                    continue;
                }

                //this code is created by jitendra upadhyay on 10-09-2014
                //this code for check posted month before leave request
                //code start
                int Monthposted = objSys.getDateForInput(txtApplyDate.Text.ToString()).Month; ;
                //    int Month = Convert.ToDateTime(txtFrom.Text).ToString();
                int Yearposted = Convert.ToDateTime(txtApplyDate.Text).Year;
                DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(empidlist.Split(',')[i].ToString(), Monthposted.ToString(), Yearposted.ToString());

                if (dtPostedList.Rows.Count > 0)
                {
                    PostedEmpList += GetEmployeeCode(dtPostedList.Rows[0]["Emp_Id"].ToString()) + ",";

                }
                else
                {
                    NonPostedLog += GetEmployeeCode(empidlist.Split(',')[i].ToString()) + ",";
                }

                // Check Holiday or Not For Leave Apply For the Day...............
                DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                DateTime fromdate2 = objSys.getDateForInput(txtApplyDate.Text.ToString());
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
                DateTime HalfDayInDate = objSys.getDateForInput(txtApplyDate.Text.ToString());



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

                DateTime fromdate1 = objSys.getDateForInput(txtApplyDate.Text);


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

                        string FileName = "";
                        string FileUrl = "";
                        try
                        {
                            FileName = Session["empimgpath"].ToString();
                            FileUrl = Session["empimgpathFull"].ToString();
                        }
                        catch
                        {
                            FileName = "";
                            FileUrl = "";
                        }
                        b = objPartial.InsertPartialLeaveRequest(Session["CompId"].ToString(), empidlist.Split(',')[i], "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", strPartialMethod, true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), FileName, FileUrl);

                        if (b != 0)
                        {
                            strTransId = b.ToString();
                            if (dtPartialLeave.Rows.Count > 0)
                            {

                                for (int j = 0; j < dtPartialLeave.Rows.Count; j++)
                                {
                                    int cur_trans_id = 0;
                                    string PriorityEmpId = dtPartialLeave.Rows[j]["Emp_Id"].ToString();
                                    string IsPriority = dtPartialLeave.Rows[j]["Priority"].ToString();

                                    if (EmpPermission == "1")
                                    {
                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), "0", "0", "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }
                                    else if (EmpPermission == "2")
                                    {
                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }
                                    else if (EmpPermission == "3")
                                    {
                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }
                                    else
                                    {
                                        cur_trans_id = objApproalEmp.InsertApprovalTransaciton("3", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", empidlist.Split(',')[i].ToString(), b.ToString(), PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "01/01/1900", "Pending", txtDescription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                    }

                                    // Insert Notification For Leave by  ghanshyam suthar
                                    Session["PriorityEmpId"] = PriorityEmpId;
                                    Session["cur_trans_id"] = cur_trans_id;
                                    Set_Notification();
                                    //--------------------------------------

                                    //for Email Code
                                    if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                                    {
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

                                                    string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(empidlist.Split(',')[i], Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(empidlist.Split(',')[i], Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(empidlist.Split(',')[i], Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(empidlist.Split(',')[i], Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + txtApplyDate.Text + "<br />From Time :" + txtInTime.Text + "  To Time :" + txtOuttime.Text + "<br /> Reason For Leave : " + txtDescription.Text + " " + strPendingApproval + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";

                                                    //string MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>'" + Common.GetEmployeeName(empidlist.Split(',')[i]) + "'Apply for Partial Leave 'Personal'<br />On Date '" + txtApplyDate.Text + "' Timing is From '" + txtInTime.Text + "' To '" + txtOuttime.Text + "' <br /> and Given Description is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                                    ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Personal Partial Leave Apply By'" + Common.GetEmployeeName(empidlist.Split(',')[i], Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' On Date '" + txtApplyDate.Text + "'", MailMessage.ToString(), Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        b = objPartial.UpdatePartialLeaveRequest(hdnEdit.Value, Session["CompId"].ToString(), hdnEmpId.Value, "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), objSys.getDateForInput(txtApplyDate.Text).ToString(), txtInTime.Text, txtOuttime.Text, "Pending", txtDescription.Text, "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        if (b != 0)
                        {
                            if (dtPartialLeave.Rows.Count > 0)
                            {

                            }
                            Session["PriorityEmpId"] = hdnEmpId.Value;
                            Session["cur_trans_id"] = hdnEdit.Value;
                            Set_Notification();
                            DisplayMessage("Partial leave updated");
                            btnCancel_Click(null, null);
                            FillLeaveStatus();
                            hdnEdit.Value = "";
                            hdnEditDate.Value = "0";
                            Lbl_Tab_New.Text = Resources.Attendance.New;
                            txtEmpName.Enabled = true;
                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
            FillLeaveStatus();
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
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpeg") && (ext != ".pdf"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge ,.pdf extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll"))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll");
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/") + FULogoPath.FileName;

                string TransId = hidtransId.Value;
                string a = objPartial.UpdateImagePath(TransId, FULogoPath.FileName, "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName);
                FULogoPath.SaveAs(path);
                Session["empimgpath"] = FULogoPath.FileName;
                Session["empimgpathFull"] = "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath.FileName;
            }
        }
        else if (FULogoPath1.HasFile)
        {
            string ext = FULogoPath1.FileName.Substring(FULogoPath1.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpeg") && (ext != ".pdf"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge ,.pdf extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll"))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"] + "/LeaveDocAll");
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/") + FULogoPath1.FileName;

                string TransId = hidtransId.Value;
                string a = objPartial.UpdateImagePath(TransId, FULogoPath1.FileName, "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath1.FileName);
                FULogoPath1.SaveAs(path);
                Session["empimgpath"] = FULogoPath1.FileName;
                Session["empimgpathFull"] = "~/CompanyResource/" + "/" + Session["CompId"] + "/LeaveDocAll" + "/" + FULogoPath1.FileName;
            }
        }
    }

    private void Set_Notification()
    {
        string strEmpId = string.Empty;
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/attendance"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        if (rbtnPersonal.Checked)
        {
            strEmpId = hdnEmpId.Value + ",";
        }
        else
        {
            ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            for (int i = 0; i < userdetails.Count; i++)
            {
                strEmpId += userdetails[i].ToString() + ",";
            }

        }


        foreach (string str in strEmpId.Trim().Split(','))
        {
            if (str == "")
            {
                continue;
            }

            Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, str, Session["PriorityEmpId"].ToString());
            GetEmployeeName(str);
            string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
            string Message = string.Empty;
            Message = txtEmpName.Text.Trim() + " applied Partial Leave for " + txtApplyDate.Text + " Time : from " + txtInTime.Text + " to " + txtOuttime.Text + "";
            if (hdnEdit.Value == "")
            {
                // For Insert        
                Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), str, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "0", "0");
            }
            else
            {
                // For Update
                //  Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["cur_trans_id"].ToString(), "15");
            }
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


    public int getCurrentMonthLeaveCount(DateTime applydate)
    {
        int Count = 0;

        DataTable dt = objPartial.GetPartialLeaveRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), hdnEmpId.Value, applydate.Month.ToString(), applydate.Year.ToString());
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

        DataTable dt = objPartial.GetPartialLeaveRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), hdnEmpId.Value, applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());
                }
            }
            catch
            {
                useminutes = 0;
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

        DataTable dt = objPartial.GetPartialLeaveRequestById(Session["CompId"].ToString(), hdnEmpId.Value);
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

    protected void gvLeaveStatus_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Partial_DtLeaveStatus"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLeaveStatus, dt, "", "");

    }

    #region locationFilter

    protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
    {

        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
    }
    protected void ddlLeaveStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        txtValueDate.Text = "";

        txtValue.Text = "";
        if (ddlFieldName.SelectedValue.Trim() == "Partial_Leave_Date")
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

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValueFilter.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValueFilter.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValueFilter.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["Partial_DtLeaveStatus"];
            DataView view = new DataView();

            if (ddlFieldName.SelectedValue.Trim() == "Partial_Leave_Date")
            {
                view = new DataView(dtCust, "Partial_Leave_Date='" + Convert.ToDateTime(txtValueDate.Text).ToString() + "'", "", DataViewRowState.CurrentRows);
            }
            else
            {

                view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            }


            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvLeaveStatus, view.ToTable(), "", "");
            txtValue.Focus();

        }

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
        txtValueFilter.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValueFilter.Focus();
        txtValueDate.Visible = false;
        txtValueFilter.Visible = true;
        ddlOption.Enabled = true;
    }

    public void setYearList()
    {
        DataTable dt = getYearList();
        ddlYearList.DataSource = dt;
        ddlYearList.DataTextField = "year";
        ddlYearList.DataValueField = "year";
        //ddlYearList.DataValueField = dt.Rows.Count > 0 ? "Year" : DateTime.Now.Year.ToString();
        ddlYearList.DataBind();
        ddlYearList.SelectedIndex = 0;
    }

    public DataTable getYearList()
    {
        DataTable dtInfo = new DataTable();
        using (dtInfo = daClass.return_DataTable("select distinct year(Partial_Leave_Date) as year from Att_PartialLeave_Request order by year(Partial_Leave_Date) desc"))
            return dtInfo;
    }
    protected void ddlYearList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillLeaveStatus();
    }

    #endregion


    public void FillLeaveStatus()
    {
        DataTable dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());

        if (dtLeave.Rows.Count > 0)
        {
            if (Request.QueryString["Emp_Id"] == null)
            {
                //and year=" + ddlYearList.SelectedValue.Trim() + "
                dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id in (" + ddlLoc.SelectedValue.Trim() + ") ", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtLeave = new DataView(dtLeave, "Emp_Id=" + Session["EmpId"].ToString() + " and year=" + ddlYearList.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
            }


            if (ddlLeaveStatus.SelectedIndex > 0)
            {
                dtLeave = new DataView(dtLeave, "Is_Confirmed='" + ddlLeaveStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
            Session["Partial_DtLeaveStatus"] = dtLeave;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtLeave.Rows.Count.ToString() + "";
        }
        else
        {
            gvLeaveStatus.DataSource = null;
            gvLeaveStatus.DataBind();
            Session["Partial_DtLeaveStatus"] = null;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }
        //AllPageCode();
    }
    protected void gvLeaveStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLeaveStatus.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["Partial_DtLeaveStatus"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLeaveStatus, dt, "", "");
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

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        txtEmpName.Enabled = false;

        txtEmpName.Enabled = false;
        Common cmn = new Common(Session["DBConnection"].ToString());
        //hdnEmpId.Value = e.CommandName.ToString();

        DataTable dt = new DataTable();
        dt = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());

        if (dt.Rows.Count > 0)
        {
            hdnEmpId.Value = dt.Rows[0]["Emp_Id"].ToString();

            txtEmpName.Text = cmn.GetEmpName(hdnEmpId.Value, HttpContext.Current.Session["CompId"].ToString());


            DataTable dtEmployee = Common.GetEmployee(txtEmpName.Text, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), Session["DBConnection"].ToString());

            if (dtEmployee.Rows.Count > 0)
            {
                txtEmpName.Text = "" + dtEmployee.Rows[0][1].ToString() + "/(" + dtEmployee.Rows[0][2].ToString() + ")/" + dtEmployee.Rows[0][0].ToString() + "";
            }

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

            string strPLType = dt.Rows[0]["Field1"].ToString();
            string strTimeTableId = dt.Rows[0]["Field2"].ToString();

            txtInTime.Text = dt.Rows[0]["From_Time"].ToString();
            txtOuttime.Text = dt.Rows[0]["To_Time"].ToString();

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

            txtDescription.Text = dt.Rows[0]["Description"].ToString();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Edit_Active()", true);

            FillEmpLeave(hdnEmpId.Value);
            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

            //txtEmpName_textChanged(sender, e);
        }
    }
    protected void IbtnReject_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objPartial.PartialLeaveApproveReject(e.CommandArgument.ToString(), "Canceled", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

        if (b != 0)
        {
            DisplayMessage("Leave Rejected");
            FillLeaveStatus();
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Session["empimgpath"] = null;
        Session["empimgpathFull"] = null;
        rbtnPersonal.Checked = false;
        txtApplyDate.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat());

        txtInTime.Text = "";
        txtOuttime.Text = "";

        rbBegining.Checked = false;
        rbMiddle.Checked = false;
        rbEnding.Checked = false;

        rbTimeTable.ClearSelection();
        rbTimeTable.DataSource = null;
        rbTimeTable.DataBind();
        rbTimeTable.Items.Clear();

        txtDescription.Text = "";

        Lbl_Tab_New.Text = Resources.Attendance.New;
        hdnEdit.Value = "";
        hdnEditDate.Value = "0";
        dvEmp.Visible = true;
        GvEmpList.Visible = true;
        FillGvEmp();
        rbtnOfficial.Checked = true;

        rbtnPersonal.Checked = false;
        GvEmpList.Focus();

        if (Request.QueryString["Emp_Id"] == null)
        {

            txtEmpName.Text = "";
            gvLeaveSummary_PartialLeave.DataSource = null;
            gvLeaveSummary_PartialLeave.DataBind();
            rbtnPersonal.Checked = true;
            rbtnOfficial.Checked = false;
            rbtnPersonal_CheckedChanged(null, null);
            txtEmpName.Enabled = true;
            gvEmpPendingLeave.DataSource = null;
            gvEmpPendingLeave.DataBind();
            ViewState["DtEmpLeaveStatus"] = null;
        }
        else
        {
            txtEmpName_textChanged(null, null);
        }

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Lbl_Tab_New.Text = Resources.Attendance.New;
        btnReset_Click(null, null);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public void OnDownloadDocumentCommandList(object sender, CommandEventArgs e)
    {
        try
        {
            DataTable dtLeave = new DataTable();
            if (Request.QueryString["Emp_Id"] == null)
            {
                dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());

            }
            else
            {

                dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());

            }

            if (dtLeave.Rows.Count > 0)
            {

                //if (ddlLeaveStatus.SelectedIndex == 1)
                //{
                //    dtLeave = new DataView(dtLeave, "Is_Confirmed='Pending'", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //else if (ddlLeaveStatus.SelectedIndex == 2)
                //{
                //    dtLeave = new DataView(dtLeave, "Is_Confirmed='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //else if (ddlLeaveStatus.SelectedIndex == 3)
                //{
                //    dtLeave = new DataView(dtLeave, "Is_Confirmed='Canceled'", "", DataViewRowState.CurrentRows).ToTable();
                //}

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
    public void downloadfile(DataTable dt)
    {
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.AddHeader("Content-Disposition", "attachment; filename=" + dt.Rows[0]["FileName"].ToString() + ";");
        response.TransmitFile(Server.MapPath(dt.Rows[0]["FileUrl"].ToString().Replace("~/", "~//")));
        response.Flush();
        response.End();
    }
    public void FillEmpLeave(string Empid)
    {
        DataTable dtLeave = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
        try
        {
            dtLeave = new DataView(dtLeave, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Emp_Id=" + Empid + "", "", DataViewRowState.CurrentRows).ToTable();
            dtLeave = new DataView(dtLeave, "Is_Confirmed='Pending' or Is_Confirmed='Approved'", "Partial_Leave_Date desc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmpPendingLeave, dtLeave, "", "");
        foreach (GridViewRow gvr in gvEmpPendingLeave.Rows)
        {
            Label lblFileName = (Label)gvr.FindControl("lblFileDownloadList1");
            Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathList1");
            LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadList1");
            Button ChoosefileUpload = (Button)gvr.FindControl("ChoosefileUpload");
            Label Status = (Label)gvr.FindControl("lblStatus");
            if (lblFileName.Text == "" && lblFilePath.Text == "")
            {
                if(Status.Text== "Pending")
                {
                    ChoosefileUpload.Visible = true;
                }
                else
                {
                    ChoosefileUpload.Visible = false;
                }
                lnkDownload.Visible = false;
            }
            else
            {
                ChoosefileUpload.Visible = true;
                lnkDownload.Visible = true;
            }
        }
        ViewState["DtEmpLeaveStatus"] = dtLeave;

    }
    protected void gvEmpPendingLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmpPendingLeave.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["DtEmpLeaveStatus"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmpPendingLeave, dt, "", "");
        foreach (GridViewRow gvr in gvEmpPendingLeave.Rows)
        {
            Label lblFileName = (Label)gvr.FindControl("lblFileDownloadList1");
            Label lblFilePath = (Label)gvr.FindControl("lblFileFullPathList1");
            LinkButton lnkDownload = (LinkButton)gvr.FindControl("ImgDownloadList1");
            Button ChoosefileUpload = (Button)gvr.FindControl("ChoosefileUpload");
            if (lblFileName.Text == "" && lblFilePath.Text == "")
            {
                ChoosefileUpload.Visible = true;
                lnkDownload.Visible = false;
            }
            else
            {
                ChoosefileUpload.Visible = false;
                lnkDownload.Visible = true;
            }
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
            try
            {
                gvEmpPendingLeave.Columns[4].Visible = true;
                gvEmpPendingLeave.Columns[5].Visible = true;
                gvEmpPendingLeave.Columns[6].Visible = false;
            }
            catch
            {

            }
        }
        else if (strPLWithTimeWithOutTime == "False")
        {
            try
            {
                gvEmpPendingLeave.Columns[4].Visible = false;
                gvEmpPendingLeave.Columns[5].Visible = false;
                gvEmpPendingLeave.Columns[6].Visible = true;
            }
            catch
            {

            }
        }
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
    protected void rbBegining_CheckedChanged(object sender, EventArgs e)
    {
        string strEmployeeId = hdnEmpId.Value;

        if (rbtnPersonal.Checked == true)
        {
            if (rbBegining.Checked == true || rbMiddle.Checked == true || rbEnding.Checked == true)
            {
                if (txtEmpName.Text == "" && rbtnPersonal.Checked == true)
                {
                    DisplayMessage("Enter Employee Name");
                    txtEmpName.Focus();
                    return;
                }

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
        else if (rbtnOfficial.Checked == true)
        {
            string empidlist = string.Empty;
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues();

            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select Employee First");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        empidlist += userdetails[i].ToString() + ",";
                    }
                }
            }

            if (txtApplyDate.Text == "")
            {
                DisplayMessage("Enter Apply Date");
                txtApplyDate.Focus();
                return;
            }

            if (rbBegining.Checked == true || rbMiddle.Checked == true || rbEnding.Checked == true)
            {
                for (int i = 0; i < empidlist.Split(',').Length; i++)
                {

                    if (empidlist.Split(',')[i].ToString() != "")
                    {
                        DataTable dtShiftAllDate = objEmpSch.GetSheduleDescription(empidlist.Split(',')[i].ToString());
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


    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {

        PrintReport(e.CommandArgument.ToString());
    }


    public void PrintReport(string LeaveId)
    {
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/NICPL.aspx?TransId=" + LeaveId + "','window','width=1024');", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Vacation Leave Application.aspx?TransId=" + LeaveId + "','window','width=1024');", true);
    }

}