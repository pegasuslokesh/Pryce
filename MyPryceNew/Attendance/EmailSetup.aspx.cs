using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class Attendance_EmailSetup : BasePage
{
    Common cmn = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    Ser_GroupMaster objGroup = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    Ser_ReportMaster objReportMst = null;
    Ser_ReportNotification objReportNotification = null;
    Ser_ReportSetup objReportsetup = null;
    Country_Currency objCC = null;
    DataAccessClass Objda = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        cmn = new Common(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objGroup = new Ser_GroupMaster(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objReportMst = new Ser_ReportMaster(Session["DBConnection"].ToString());
        objReportNotification = new Ser_ReportNotification(Session["DBConnection"].ToString());
        objReportsetup = new Ser_ReportSetup(Session["DBConnection"].ToString());
        objCC = new Country_Currency(Session["DBConnection"].ToString());
        Objda = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
           

            Session["TimeZoneId"] = objCC.getTimeZoneIdNameByCurrencyId(Session["LocCurrencyId"].ToString());
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            FillGrid_Group();
            Session["ReportTransId"] = null;
            FillddlLocation();
            FillddlDeaprtment();
            Session["dtEmpR"] = null;
            Session["CHECKED_ITEMS"] = null;
            //FillGrid();
            rbtnEmpSal.Checked = true;
            rbtnGroupSal.Checked = false;
            //EmpGroupSal_CheckedChanged(null, null);
            rbtnEmail.Checked = true;
            FillReportType();
            //cmn.FillData((object)gvEmployee, null, "", "");
            Session["EmpFiltered"] = null;
            txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
            CalendarExtender1.Format = objSys.SetDateFormat();
            FillGroupName();
            ddlFieldName.SelectedValue = "Schedule_date";
            txtValue.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("dd-MMM-yyyy");
            ddlOption.SelectedIndex = 1;
            btnbind_Click(null, null);
            txtHelpContent.Content = "Here are the list of <span style='font-weight: bold;'>Keyword(@symbol)</span> you can use in your email template body To Retrieve data from our service<br />1.For Employee Name = @EMPLOYEENAME <br/>2.For Current Date= @CURRENTDATE <br/>  3.For From Date= @FROMDATE <br/>    4.For To Date= @TODATE <br/>           5.For <span style = 'font-weight:bold;' > </span> Device Name = @DEVICENAME <br/>            6.For In Time = @INTIME <br/>           7.For Out Time = @OUTTIME <br/>  8.For Company Logo = @COMPANYLOGO <br/> 9.For Attendance Detail = @ATTENDANCEDETAIL <br/>         &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ";

        }
        AllPageCode();
    }

    public void FillReportType()
    {
        pnlReportType.Visible = true;
        DataTable dt = GetReportDatatable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvReportType, dt, "", "");
    }


    public DataTable GetReportDatatable()
    {


        return objEmpNotice.GetAllNotification_By_NOtificationType("Email");

        //DataTable dtEmail = new DataTable();
        //DataTable dt = new DataTable();
        //if (Edit.Value == "")
        //{
        //    if (rbtnSms.Checked == true)
        //    {
        //        dt = objEmpNotice.GetAllNotification_By_NOtificationType("SMS");
        //    }
        //    if (rbtnEmail.Checked == true)
        //    {
        //        dt = objEmpNotice.GetAllNotification_By_NOtificationType("Email");
        //    }
        //    if (rbtnBoth.Checked == true)
        //    {

        //        dt = objEmpNotice.GetAllNotification_By_NOtificationType("SMS");
        //        dtEmail = objEmpNotice.GetAllNotification_By_NOtificationType("Email");
        //        try
        //        {
        //            dt.Merge(dtEmail);
        //        }
        //        catch
        //        {
        //        }
        //    }

        //}
        //else
        //{
        //    //dt = objEmpNotice.GetAllNotification_By_NOtificationType("SMS");
        //    dtEmail = objEmpNotice.GetAllNotification_By_NOtificationType("Email");
        //    try
        //    {
        //        dt.Merge(dtEmail);
        //    }
        //    catch
        //    {
        //    }
        //}


        //return dt;

    }


    protected void btnbindDeptEmp_Click(object sender, EventArgs e)
    {

        FillGrid();

        //string strStatus = "False";
        //foreach (ListItem li in listEmpDept.Items)
        //{
        //    if (li.Selected)
        //    {
        //        strStatus = "True";
        //        break;
        //    }
        //}

        //DataTable dt = new DataTable();
        //try
        //{
        //    if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
        //    {
        //        if (ddlLocation.SelectedIndex == 0 && strStatus == "False")
        //        {
        //            FillGrid();

        //            for (int i = 0; i < gvEmployee.Rows.Count; i++)
        //            {
        //                Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
        //                string[] split = lblEmp.Text.Split(',');

        //                for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
        //                {
        //                    if (lblEmp.Text.Split(',')[j] != "")
        //                    {
        //                        if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
        //                        {
        //                            ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {

        //            dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        //            if (ddlLocation.SelectedIndex == 0)
        //            {
        //                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //            }
        //            else
        //            {
        //                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

        //            }


        //            Session["DeptList"] = null;
        //            DataTable dtDepartment = new DataTable();

        //            if (strStatus == "True")
        //            {
        //                string strDeptId = string.Empty;
        //                foreach (ListItem li in listEmpDept.Items)
        //                {
        //                    if (li.Selected)
        //                    {
        //                        dtDepartment = Objda.return_DataTable("SELECT Dep_Id FROM set_departmentmaster where (dep_id =" + li.Value.ToString() + " or Parent_Id=" + li.Value.ToString() + ") and IsActive='True'");

        //                        int i = 0;
        //                        while (i < dtDepartment.Rows.Count)
        //                        {
        //                            if (Session["DeptList"] == null)
        //                            {
        //                                Session["DeptList"] = dtDepartment.Rows[i][0].ToString();
        //                            }
        //                            else
        //                            {
        //                                Session["DeptList"] = Session["DeptList"].ToString() + "," + dtDepartment.Rows[i][0].ToString();
        //                            }

        //                            FillChild(dtDepartment.Rows[i][0].ToString());

        //                            i++;

        //                        }



        //                        //strDeptId +=   Objda.return_DataTable("(SELECT STUFF((SELECT DISTINCT ',' + RTRIM(Dep_Id) FROM set_departmentmaster where dep_id =" + li.Value.ToString() + " or Parent_Id=" + li.Value.ToString() + " and IsActive='True'  FOR xml PATH ('')), 1, 1, ''))").Rows[0][0].ToString()  + ",";
        //                    }
        //                }


        //                try
        //                {
        //                    dt = new DataView(dt, "Department_Id in (" + Session["DeptList"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        //                }
        //                catch
        //                {

        //                }
        //            }





        //            if (dt.Rows.Count > 0)
        //            {


        //                //Common Function add By Lokesh on 22-05-2015
        //                cmn.FillData((object)gvEmployee, dt, "", "");
        //                Session["dtEmp_Report"] = dt;
        //                for (int i = 0; i < gvEmployee.Rows.Count; i++)
        //                {
        //                    Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
        //                    string[] split = lblEmp.Text.Split(',');

        //                    for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
        //                    {
        //                        if (lblEmp.Text.Split(',')[j] != "")
        //                        {
        //                            if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
        //                            {
        //                                ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
        //                            }
        //                        }
        //                    }
        //                }
        //                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        //            }
        //            else
        //            {
        //                gvEmployee.DataSource = null;
        //                gvEmployee.DataBind();
        //                Session["dtEmp_Report"] = dt;
        //                for (int i = 0; i < gvEmployee.Rows.Count; i++)
        //                {
        //                    Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
        //                    string[] split = lblEmp.Text.Split(',');

        //                    for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
        //                    {
        //                        if (lblEmp.Text.Split(',')[j] != "")
        //                        {
        //                            if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
        //                            {
        //                                ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
        //                            }
        //                        }
        //                    }
        //                }
        //                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";

        //            }
        //        }





        //    }
        //}
        //catch (Exception Ex)
        //{
        //    DisplayMessage("Select Company");
        //}

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
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }

    private void FillddlDeaprtment()
    {
        listEmpDept.Items.Clear();
        string DepIds = String.Empty;
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        if (ddlLocation.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Location_Id='" + ddlLocation.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.SelectedValue.Trim(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            dt = new DataView(dt, "Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }

        if (DepIds == "")
        {
            DepIds = "0,";
        }



        //if (!Common.GetStatus())
        //{
        if (DepIds != "")
        {
            dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        //}

        if (dt.Rows.Count > 0)
        {
            listEmpDept.DataSource = dt;
            listEmpDept.DataTextField = "Dep_Name";
            listEmpDept.DataValueField = "Dep_Id";
            listEmpDept.DataBind();
        }

    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("125",(DataTable)Session["ModuleName"]);
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


        Page.Title = objSys.GetSysTitle();

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            ImageButton4.Visible = true;
            hdnCanEdit.Value = "true";
            hdnCanDelete.Value = "true";
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "125", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {

            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "1")
                    {
                        btnSave.Visible = true;
                    }

                    if (DtRow["Op_Id"].ToString() == "2")
                    {
                        hdnCanEdit.Value = "true";
                       
                    }
                    if (DtRow["Op_Id"].ToString() == "3")
                    {
                        hdnCanDelete.Value = "true";
                        ImageButton4.Visible = true;
                    }
                    if (DtRow["Op_Id"].ToString() == "4")
                    {

                    }
                }
            }
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        FillGrid_Group();
        AllPageCode();
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != "")
        {
            string condition = string.Empty;


            if (ddlOption.SelectedIndex == 1)
            {
                if (ddlFieldName.SelectedValue == "Group_Name")
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValue.Text.Trim()) + "'";
                }
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtEmailGroup"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvBankMaster, view.ToTable(), "", "");
            AllPageCode();
        }
        txtValue.Focus();
    }


    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        Lbl_Tab_New.Text = Resources.Attendance.Edit;


        hdnEditId.Value = e.CommandArgument.ToString();


        DataTable dtGroup = objGroup.GetAllRecordByTRansId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (dtGroup.Rows.Count > 0)
        {
            txtGroupName.Text = dtGroup.Rows[0]["Group_Name"].ToString();
            txtGroupName.Enabled = false;
            txtDays.Text = dtGroup.Rows[0]["Schedule_Days"].ToString();
            txtserviceRunTime.Text = Convert.ToDateTime(dtGroup.Rows[0]["Run_Time"].ToString()).ToString("HH:mm");
            txtSubjectContent.Text = dtGroup.Rows[0]["Field1"].ToString();
            txtHeaderContent.Content = dtGroup.Rows[0]["Field2"].ToString();
            txtFooterContent.Content = dtGroup.Rows[0]["Field3"].ToString();
            //fill reporitng employee

            Session["EmpFiltered"] = null;
            Session["dtEmpR"] = null;


            DataTable dtreportMaster = Objda.return_DataTable("select Emp_Id from Ser_ReportMaster where Field1='" + hdnEditId.Value + "'");

            string strEmpId = string.Empty;

            foreach (DataRow dr in dtreportMaster.Rows)
            {
                if (strEmpId == "")
                {
                    strEmpId = dr["Emp_Id"].ToString();
                }
                else
                {
                    strEmpId = strEmpId + "," + dr["Emp_Id"].ToString();
                }
            }


            DataTable dtCompanyEmpList = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


            DataTable dtEmp = new DataTable();

            dtEmp = new DataView(dtCompanyEmpList, "Emp_Id in (" + strEmpId + ")", "", DataViewRowState.CurrentRows).ToTable();

            Session["dtEmpR"] = dtEmp;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");

            FillReportType();
            DataTable dtReport = new DataTable();
            try
            {
                dtReport = objReportNotification.GetRecord_By_RefId(hdnEditId.Value);
            }
            catch
            {
            }

            if (dtReport.Rows.Count > 0)
            {
                for (int i = 0; i < dtReport.Rows.Count; i++)
                {
                    for (int j = 0; j < gvReportType.Rows.Count; j++)
                    {
                        CheckBox chk = (CheckBox)gvReportType.Rows[j].FindControl("chkReportSel");
                        Label lblRptName = (Label)gvReportType.Rows[j].FindControl("lblReportName");
                        HiddenField reportid = (HiddenField)gvReportType.Rows[j].FindControl("hdnNotificationId");

                        if (dtReport.Rows[i]["Notification_Id"].ToString().Trim() == reportid.Value)
                        {
                            chk.Checked = true;
                        }

                    }
                }
            }

            //reported to employee list
            strEmpId = "";

            DataTable dtEmployee = objReportsetup.GetRecord_By_RefId(hdnEditId.Value);

            for (int i = 0; i < dtEmployee.Rows.Count; i++)
            {
                if (strEmpId == "")
                {
                    strEmpId = dtEmployee.Rows[i]["Emp_Id"].ToString();
                }
                else
                {
                    strEmpId = strEmpId + "," + dtEmployee.Rows[i]["Emp_Id"].ToString();
                }
            }




            dtEmp = new DataView(dtCompanyEmpList, "Emp_Id in (" + strEmpId + ")", "", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)GvEmpListSelected, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmployee, null, "", "");
            Session["EmpFiltered"] = dtEmp;

            dtEmp.Dispose();
            dtEmployee.Dispose();
            dtReport.Dispose();
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        AllPageCode();
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = objGroup.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());

        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGrid_Group();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }

    }
    protected void gvBankMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvBankMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMaster, dt, "", "");
        PopulateCheckedValues();
        AllPageCode();
    }
    protected void gvBankMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter"] = dt;
        gvBankMaster.DataSource = dt;
        gvBankMaster.DataBind();
        AllPageCode();
    }


    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    public void FillGrid_Group()
    {
        DataTable dt = objGroup.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvBankMaster, dt, "", "");

        AllPageCode();
        Session["dtFilter"] = dt;
        Session["DtEmailGroup"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        btnbind_Click(null, null);
        AllPageCode();


    }



    public void GetEmployeeDetail()
    {
        DataTable dt = new DataTable();

        dt = Objda.return_DataTable("");




    }

    #region EmailSetup



    private void FillChild(string index)//fill up child nodes and respective child nodes of them 
    {

        DataTable dt = Objda.return_DataTable("SELECT Dep_Id FROM set_departmentmaster where Parent_Id=" + index.ToString() + " and IsActive='True'");


        int i = 0;
        while (i < dt.Rows.Count)
        {
            if (Session["DeptList"] == null)
            {
                Session["DeptList"] = dt.Rows[i][0].ToString();
            }
            else
            {
                Session["DeptList"] = Session["DeptList"].ToString() + "," + dt.Rows[i][0].ToString();
            }

            FillChild(dt.Rows[i][0].ToString());

            i++;
        }

    }




    protected void lbxGroupSal_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupSal.Items.Count; i++)
        {
            if (lbxGroupSal.Items[i].Selected == true)
            {
                GroupIds += lbxGroupSal.Items[i].Value + ",";

            }

        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

            dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            {
                if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                {
                    EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                }
            }
            if (EmpIds != "")
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtEmp = new DataTable();
            }
            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp_Report"] = dtEmp;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");

                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            }
            else
            {
                Session["dtEmp_Report"] = null;
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();
                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";
            }
            for (int i = 0; i < gvEmployee.Rows.Count; i++)
            {
                Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
                string[] split = lblEmp.Text.Split(',');

                for (int j = 0; j < lblEmp.Text.Split(',').Length; j++)
                {
                    if (lblEmp.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblEmp.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";
        }
    }



    protected void btnaryRefresh_Click1(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";

        DataTable dtEmp = (DataTable)Session["dtEmp_Report"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
        AllPageCode();
    }


    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        SaveCheckedValues(gvEmployee, true);
        gvEmployee.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmp_Report"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
        PopulateCheckedValues(gvEmployee);
        AllPageCode();
    }

    private void PopulateCheckedValues(GridView GridCheckbox)
    {
        //ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        //if (userdetails != null && userdetails.Count > 0)
        //{
        //    foreach (GridViewRow gvrow in GridCheckbox.Rows)
        //    {
        //        int index = (int)GridCheckbox.DataKeys[gvrow.RowIndex].Value;
        //        if (userdetails.Contains(index))
        //        {
        //            CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
        //            myCheckBox.Checked = true;
        //        }
        //    }
        //}
        //AllPageCode();
    }
    private void SaveCheckedValues(GridView GridCheckbox, bool IsCheck)
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        bool result = false;
        foreach (GridViewRow gvrow in GridCheckbox.Rows)
        {
            index = (int)GridCheckbox.DataKeys[gvrow.RowIndex].Value;

            if (IsCheck)
            {
                result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
                if (result)
                {
                    if (!userdetails.Contains(index))
                        userdetails.Add(index);
                }

            }
            else
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }


            if (userdetails != null && userdetails.Count > 0)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
    }

    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployeeSal, (DataTable)Session["dtEmp_Report"], "", "");
        AllPageCode();
    }

    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {

        if (rbtnGroupSal.Checked)
        {
            //pnlEmp.Visible = false;
            //pnlGroupSal.Visible = true;
            Div_Group.Visible = true;
            Div_Employee_Search.Visible = false;
            //lblLocation.Visible = false;
            //ddlLocation.Visible = false;
            //lblGroupByDept.Visible = false;
            //listEmpDept.Visible = false;
            //pnlSearchdpl.Visible = false;


            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)lbxGroupSal, dtGroup, "Group_Name", "Group_Id");
            }

            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
            lbxGroupSal_SelectedIndexChanged(null, null);
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";
        }
        else if (rbtnEmpSal.Checked)
        {
            //pnlEmp.Visible = true;
            //pnlGroupSal.Visible = false;
            Div_Group.Visible = false;
            Div_Employee_Search.Visible = true;
            //lblLocation.Visible = true;
            //ddlLocation.Visible = true;
            //lblGroupByDept.Visible = true;
            //listEmpDept.Visible = true;
            //pnlSearchdpl.Visible = true;


            lblEmp.Text = "";
            Session["CHECKED_ITEMS"] = null;
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp_Report"] = dtEmp;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            }
            else
            {
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();
                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";
            }
        }
    }

    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlDeaprtment();

        //string strStatus = "False";
        //foreach (ListItem li in listEmpDept.Items)
        //{
        //    if (li.Selected)
        //    {
        //        strStatus = "True";
        //    }
        //}

        //DataTable dt = new DataTable();
        //try
        //{
        //    if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
        //    {
        //        if (ddlLocation.SelectedIndex == 0 && strStatus == "True")
        //        {
        //            FillGrid();
        //        }
        //        else
        //        {
        //            dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        //            if (ddlLocation.SelectedIndex == 0)
        //            {
        //                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //            }
        //            else
        //            {
        //                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

        //            }
        //            if (strStatus == "True")
        //            {
        //                string strDeptId = string.Empty;
        //                foreach (ListItem li in listEmpDept.Items)
        //                {
        //                    if (li.Selected)
        //                    {
        //                        strDeptId += li.Value.ToString() + ",";
        //                    }
        //                }


        //                try
        //                {
        //                    dt = new DataView(dt, "Department_Id in(" + strDeptId.Substring(0, strDeptId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //                }
        //                catch
        //                {

        //                }
        //            }

        //            if (dt.Rows.Count > 0)
        //            {
        //                //Common Function add By Lokesh on 22-05-2015
        //                cmn.FillData((object)gvEmployee, dt, "", "");
        //                Session["dtEmp_Report"] = dt;
        //                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        //            }
        //            else
        //            {
        //                gvEmployee.DataSource = null;
        //                gvEmployee.DataBind();
        //                Session["dtEmp_Report"] = dt;
        //                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";
        //            }
        //        }
        //    }
        //}
        //catch (Exception Ex)
        //{
        //    DisplayMessage("Select Company");
        //}

    }



    protected void ddlEmp_TextChanged(object sender, EventArgs e)
    {
        string empid = string.Empty;
        try
        {
            empid = txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1];
            ViewState["EmpCode"] = empid;
        }
        catch
        {

        }

        string strVal = HR_EmployeeDetail.GetEmployeeId(empid);
        if (strVal == "0" || strVal == "")
        {
            DisplayMessage("Employee not exists");
            txtEmp.Focus();
            txtEmp.Text = "";
            return;
        }

    }

    protected void txtGroupName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtGroupName.Text.Trim() != "")
        {

            DataTable dt = Objda.return_DataTable("select Group_Name from ser_groupmaster where location_id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Group_Name='" + txtGroupName.Text.Trim() + "'");

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Group Name already exists");
                txtGroupName.Text = "";
                txtGroupName.Focus();
                return;

            }
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        con.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Set_EmployeeMaster_SelectEmployeeName";
        cmd.Parameters.AddWithValue("@CompanyId", HttpContext.Current.Session["CompId"].ToString());
        cmd.Parameters.AddWithValue("@EmpName", prefixText);
        cmd.CommandType = CommandType.StoredProcedure;
        da.SelectCommand = cmd;
        da.Fill(dt);

        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
        }
        return str;
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListGroupName(string prefixText, int count, string contextKey)
    {
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjDa.return_DataTable("select distinct Group_Name from ser_groupmaster where location_id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Group_Name like '%" + prefixText + "%'");

        //DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString());

        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Group_Name"].ToString();
        }
        return str;
    }



    protected void chkSelAll_CheckedChangedR(object sender, EventArgs e)
    {

        CheckBox ChkAll = (CheckBox)gvReportType.HeaderRow.FindControl("chkSelAll");

        foreach (GridViewRow gvr in gvReportType.Rows)
        {

            CheckBox chk = (CheckBox)gvr.FindControl("chkReportSel");

            if (ChkAll.Checked)
            {
                chk.Checked = true;
            }
            else
            {
                chk.Checked = false;
            }

        }

    }


    protected void chkgvSelect_CheckedChanged1(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
        if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";

            if (!lblEmp.Text.Split(',').Contains(lb.Text))
            {
                lblEmp.Text += empidlist;
            }
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblEmp.Text += empidlist;
            string[] split = lblEmp.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblEmp.Text = temp;
        }
    }

    protected void chkgvSelectAll_CheckedChangedNF(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmpNF.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gvrow in gvEmpNF.Rows)
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
        AllPageCode();
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvEmployee.Rows.Count; i++)
        {
            ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblEmp.Text.Split(',').Contains(((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
                {
                    lblEmp.Text += ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblEmp.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblEmp.Text = temp;
            }
        }
    }

    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;

        Label Transid = (Label)gvBankMaster.Rows[index].FindControl("lblTransId");

        if (((CheckBox)gvBankMaster.Rows[index].FindControl("chkgvSelect")).Checked == true)
        {
            if (!userdetails.Contains(Convert.ToInt32(Transid.Text)))
            {
                userdetails.Add(Convert.ToInt32(Transid.Text));
            }
        }
        else
        {
            if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
            {
                userdetails.Remove(Convert.ToInt32(Transid.Text));
            }

        }

        Session["CHECKED_ITEMS"] = userdetails;

    }

    protected void chkgvSelectAll_CheckedChanged1(object sender, EventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        CheckBox chkSelAll = ((CheckBox)gvBankMaster.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvBankMaster.Rows)
        {
            Label Transid = (Label)gr.FindControl("lblTransId");

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
                if (!userdetails.Contains(Convert.ToInt32(Transid.Text)))
                {
                    userdetails.Add(Convert.ToInt32(Transid.Text));
                }
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
                if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
                {
                    userdetails.Remove(Convert.ToInt32(Transid.Text));
                }
            }
        }

        Session["CHECKED_ITEMS"] = userdetails;

    }

    protected void ImgbtnDeleteAll_Click1(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (gvBankMaster.Rows.Count == 0)
        {
            DisplayMessage("No Data Exists");
            return;
        }

        if (gvBankMaster.Rows.Count != 0)
        {
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (userdetails.Count > 0)
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        if (userdetails[i].ToString() != "")
                        {
                            objGroup.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString());
                        }
                    }

                    DisplayMessage("Record Deleted");
                    FillGrid_Group();
                }
            }
            else
            {
                DisplayMessage("Please Select Record ");
                gvBankMaster.Focus();
                return;
            }
        }
    }
    protected void ImgbtnSelectAll_Click1(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtFilter"];

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

                if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

            }
            foreach (GridViewRow gvrow in gvBankMaster.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtFilter"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvBankMaster, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }


    protected void ImgbtnSelectAll_Clickary(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmp_Report"];
        if(dtAllowance==null)
        {
            return;
        }

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
            foreach (GridViewRow gvrow in gvEmployee.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmp_Report"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }



    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvBankMaster.Rows)
            {
                int index = (int)gvBankMaster.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvBankMaster.Rows)
        {
            index = (int)gvBankMaster.DataKeys[gvrow.RowIndex].Value;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime Todaydate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());



        bool b = false;
        foreach (GridViewRow gvr in gvReportType.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkReportSel");
            if (chk.Checked)
            {
                b = true;
                break;
            }
        }
        
        if (hdnEditId.Value == "" && rbtnEmployeeNotification.Checked)
        {
            if (GvEmpListSelected.Rows.Count == 0 || b == false)
            {
                DisplayMessage("Record Required in step 1 & 2 , please confirm and try again");
                return;
            }
        }
        else if (GvEmpListSelected.Rows.Count == 0 || b == false || gvEmpNF.Rows.Count == 0)
        {
            DisplayMessage("Record Required in all step , please confirm and try again");
            return;
        }


        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            if (hdnEditId.Value == "")
            {



                //for save record
                int counter = Convert.ToInt32(txtDays.Text);

                int Days = Convert.ToInt32(txtDays.Text);

                int totalDays = DateTime.DaysInMonth(Todaydate.Year, Todaydate.Month);

               // while (Days <= totalDays)
                {
                    DateTime dtScheduleDate = new DateTime(Todaydate.Year, Todaydate.Month, Days);


                    //if (Days == 30)
                    //{
                    //    dtScheduleDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, totalDays);
                    //    Days = totalDays;
                    //}



                    if (rbtnEmployeeNotification.Checked)
                    {
                        foreach (GridViewRow gvrow in GvEmpListSelected.Rows)
                        {
                            int TransId = objGroup.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtGroupName.Text.Trim() + "_" + ((Label)gvrow.FindControl("lblEmpCode")).Text + "_" + dtScheduleDate.ToString("dd-MMM-yyyy") + "(" + txtserviceRunTime.Text + ")", dtScheduleDate.ToString(), txtserviceRunTime.Text, txtDays.Text, txtSubjectContent.Text, txtHeaderContent.Content, txtFooterContent.Content, "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                            objReportsetup.InsertRecord(TransId.ToString(), GvEmpListSelected.DataKeys[gvrow.RowIndex][0].ToString(), "", "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                            foreach (GridViewRow gvr in gvReportType.Rows)
                            {
                                CheckBox chk = (CheckBox)gvr.FindControl("chkReportSel");
                                Label lblRptName = (Label)gvr.FindControl("lblReportName");
                                HiddenField reportId = (HiddenField)gvr.FindControl("hdnNotificationId");

                                if (chk.Checked)
                                {
                                    objReportNotification.InsertRecord(TransId.ToString(), reportId.Value, "", "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                                }
                            }
                            objReportMst.InsertReportMaster(GvEmpListSelected.DataKeys[gvrow.RowIndex][0].ToString(), "0", "1/1/1900", TransId.ToString(), "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                        }

                    }
                    else
                    {

                        int TransId = objGroup.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtGroupName.Text.Trim() + "_" + dtScheduleDate.ToString("dd-MMM-yyyy") + "(" + txtserviceRunTime.Text + ")", dtScheduleDate.ToString(), txtserviceRunTime.Text, txtDays.Text, txtSubjectContent.Text, txtHeaderContent.Content, txtFooterContent.Content, "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);

                        foreach (GridViewRow gvrow in GvEmpListSelected.Rows)
                        {
                            objReportsetup.InsertRecord(TransId.ToString(), GvEmpListSelected.DataKeys[gvrow.RowIndex][0].ToString(), "", "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);

                        }

                        foreach (GridViewRow gvr in gvReportType.Rows)
                        {
                            CheckBox chk = (CheckBox)gvr.FindControl("chkReportSel");
                            Label lblRptName = (Label)gvr.FindControl("lblReportName");
                            HiddenField reportId = (HiddenField)gvr.FindControl("hdnNotificationId");

                            if (chk.Checked)
                            {
                                objReportNotification.InsertRecord(TransId.ToString(), reportId.Value, "", "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                            }
                        }



                        foreach (GridViewRow gvrow in gvEmpNF.Rows)
                        {
                            objReportMst.InsertReportMaster(gvEmpNF.DataKeys[gvrow.RowIndex][0].ToString(), "0", "1/1/1900", TransId.ToString(), "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                        }
                    }


                    //if (Days == totalDays)
                    //{
                    //    break;
                    //}
                    //else if ((Days + counter) > totalDays)
                    //{
                    //    Days = totalDays;
                    //}
                    //else
                    //{
                    //    Days = Days + counter;
                    //}

                }

                DisplayMessage("Record Saved Successfully", "green");

            }
            else
            {
                //for update record

                objGroup.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEditId.Value, txtGroupName.Text.Trim(), Todaydate.ToString(), txtserviceRunTime.Text, txtDays.Text, txtSubjectContent.Text, txtHeaderContent.Content, txtFooterContent.Content, "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);

                //delete record from reeport master by ref id
                objReportNotification.DeleteRecord_By_RefId(hdnEditId.Value, ref trns);
                Objda.execute_Command("delete from Ser_ReportMaster where Field1='" + hdnEditId.Value + "'", ref trns);
                objReportsetup.DeleteRecord_By_RefId(hdnEditId.Value, ref trns);

                foreach (GridViewRow gvrow in GvEmpListSelected.Rows)
                {
                    objReportsetup.InsertRecord(hdnEditId.Value, GvEmpListSelected.DataKeys[gvrow.RowIndex][0].ToString(), "", "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);

                }


                //delere record from notification and reinsert



                foreach (GridViewRow gvr in gvReportType.Rows)
                {
                    CheckBox chk = (CheckBox)gvr.FindControl("chkReportSel");
                    Label lblRptName = (Label)gvr.FindControl("lblReportName");
                    HiddenField reportId = (HiddenField)gvr.FindControl("hdnNotificationId");

                    if (chk.Checked)
                    {
                        objReportNotification.InsertRecord(hdnEditId.Value, reportId.Value, "", "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                    }
                }

                //insert in report setup



                foreach (GridViewRow gvrow in gvEmpNF.Rows)
                {
                    objReportMst.InsertReportMaster(gvEmpNF.DataKeys[gvrow.RowIndex][0].ToString(), "0", "1/1/1900", hdnEditId.Value.ToString(), "", "", "", "", false.ToString(), Todaydate.ToString(), true.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), Session["UserId"].ToString(), Todaydate.ToString(), ref trns);
                }
                DisplayMessage("Record Updated Successfully", "green");
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            reset();

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


    public void reset()
    {
        objPageCmn.FillData((object)GvEmpListSelected, null, "", "");
        Session["EmpFiltered"] = null;
        hdnEditId.Value = "";
        objPageCmn.FillData((object)gvEmpNF, null, "", "");
        FillddlDeaprtment();
        Session["dtEmpR"] = null;
        FillReportType();
        txtGroupName.Text = "";
        txtDays.Text = "";
        txtserviceRunTime.Text = "";
        txtEmp.Text = "";
        FillGrid_Group();
        txtGroupName.Enabled = true;
        rbtnManagerNotification.Checked = true;
        rbtnEmployeeNotification.Checked = false;
        pnlEmpNf.Visible = true;
        txtSubjectContent.Text = "";
        txtHeaderContent.Content = null;
        txtFooterContent.Content = null;
        TabContainer1.ActiveTabIndex = 0;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }


    protected void btnarybind_Click1(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        if (ddlOptionEmpFilter.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionEmpFilter.SelectedIndex == 1)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String)='" + txtValueEmpFilter.Text.Trim() + "'";
            }
            else if (ddlOptionEmpFilter.SelectedIndex == 2)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) like '%" + txtValueEmpFilter.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) Like '" + txtValueEmpFilter.Text.Trim() + "%'";

            }
            DataTable dtEmp = (DataTable)Session["dtEmp_Report"];
            if(dtEmp!=null)
            {
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployee, view.ToTable(), "", "");
                Session["CHECKED_ITEMS"] = null;
                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            }
           
        }
        AllPageCode();
        txtValueEmpFilter.Focus();
    }


    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in listEmpDept.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
        ddlLocation.SelectedIndex = 0;
        FillddlDeaprtment();
        objPageCmn.FillData((GridView)gvEmployee, null, "", "");
        objPageCmn.FillData((GridView)GvEmpListSelected, null, "", "");
        Session["EmpFiltered"] = null;
        Session["dtEmp_Report"] = null;

        //   FillGrid();


    }


    protected void IbtnDelete_gvEmpNF_Command(object sender, CommandEventArgs e)
    {

        DataTable dtEmp = new DataTable();


        dtEmp = new DataView((DataTable)Session["dtEmpR"], "Emp_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        Session["dtEmpR"] = dtEmp;
        objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");

    }

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void IbtnDelete_GvEmpListSelected_Command(object sender, CommandEventArgs e)
    {

        DataTable dtEmp = new DataTable();


        dtEmp = new DataView((DataTable)Session["EmpFiltered"], "Emp_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        Session["EmpFiltered"] = dtEmp;
        objPageCmn.FillData((object)GvEmpListSelected, dtEmp, "", "");

    }


    public void FillGrid()
    {
        string strselectedDeptId = string.Empty;
        foreach (ListItem li in listEmpDept.Items)
        {
            if (li.Selected)
            {
                strselectedDeptId += li.Value + ",";
            }
        }


        string DepIds = String.Empty;
        DataTable dt = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        if (ddlLocation.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Location_Id='" + ddlLocation.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.SelectedValue.Trim(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
            dt = new DataView(dt, "Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }

        if (DepIds == "")
        {
            DepIds = "0,";
        }

        //if (!Common.GetStatus())
        //{
        if (DepIds != "")
        {
            dt = new DataView(dt, "Department_Id in (" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        //}


        if (strselectedDeptId != "")
        {
            try
            {
                dt = new DataView(dt, "Department_Id in (" + strselectedDeptId.Substring(0, strselectedDeptId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dt, "", "");
            Session["dtEmp_Report"] = dt;
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        }
        else
        {

            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
            Session["dtEmp_Report"] = dt;
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        }

    }


    protected void btnAddReportingList_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");

        if (gvEmployee.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }



        DataTable dtEmp1 = new DataTable();
        DataTable dtEmp = (DataTable)Session["dtEmp_Report"];

        if (Session["EmpFiltered"] == null)
        {
            dtEmp1 = dtEmp.Clone();
        }
        else
        {
            dtEmp1 = (DataTable)Session["EmpFiltered"];

        }
        for (int i = 0; i < gvEmployee.Rows.Count; i++)
        {
            if (((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked)
            {
                if (new DataView(dtEmp1, "Emp_Id='" + gvEmployee.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                {
                    dtEmp1.Merge(new DataView(dtEmp, "Emp_Id='" + gvEmployee.DataKeys[i][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable());
                }
            }
        }



        dtEmp1 = new DataView(dtEmp1, "", "Emp_Code", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)GvEmpListSelected, dtEmp1, "", "");
        objPageCmn.FillData((object)gvEmployee, null, "", "");
        Session["EmpFiltered"] = dtEmp1;

    }

    protected void btnAddList_Click(object sender, EventArgs e)
    {
        string EmpId = GetEmpId(txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1].ToString());


        if (EmpId == "")
        {
            DisplayMessage("Employee not exists");
            txtEmp.Focus();
            txtEmp.Text = "";
            return;
        }
        DataTable dtTemp = new DataTable();

        DataTable dtEmp = new DataTable();


        if (Session["dtEmpR"] != null)
        {
            if (new DataView((DataTable)Session["dtEmpR"], "Emp_Id='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("Employee Already Exists");
                txtEmp.Focus();
                txtEmp.Text = "";
                return;
            }

        }

        dtTemp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmpId);

        dtTemp.Columns["Department_Name"].ColumnName = "Department";

        //dtTemp = new DataView(dtTemp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "' and Emp_Id='" + ViewState["EmpCode"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["dtEmpR"] == null)
        {
            dtEmp = dtTemp;
        }
        else
        {
            dtEmp = (DataTable)Session["dtEmpR"];

            dtEmp.Merge(dtTemp);

        }


        dtEmp = new DataView(dtEmp, "", "Emp_Code", DataViewRowState.CurrentRows).ToTable();

        Session["dtEmpR"] = dtEmp;
        objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
        txtEmp.Text = "";
        txtEmp.Focus();
    }



    public string GetEmpId(string empcode)
    {

        string empId = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Code='" + empcode.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            empId = dt.Rows[0]["Emp_Id"].ToString();
        }


        return empId;

    }

    #endregion




    public string GetTotalEmail(string strGroupId)
    {

        return Objda.return_DataTable("select COUNT(*) from Ser_ReportMaster where Field1='" + strGroupId + "'").Rows[0][0].ToString();

    }


    public string GetTotalPendingEmail(string strGroupId, string strscheduleDate)
    {


        return Objda.return_DataTable("select COUNT(*) from ser_reportlog where Field3='" + strGroupId + "' and (SELECT top 1 cast(value as date) from  dbo.F_Split(Field2,',')  order by cast(value as date) desc) = '" + Convert.ToDateTime(strscheduleDate) + "' and Status='Pending' ").Rows[0][0].ToString();

    }


    public string GetTotalSentEmail(string strGroupId, string strscheduleDate)
    {

        return Objda.return_DataTable("select COUNT(*) from ser_reportlog where Field3='" + strGroupId + "' and (SELECT top 1 cast(value as date) from  dbo.F_Split(Field2,',')  order by cast(value as date) desc) = '" + Convert.ToDateTime(strscheduleDate) + "' and Status='Send' ").Rows[0][0].ToString();

    }



    public string GetTotalFailedEmail(string strGroupId, string strscheduleDate)
    {


        return Objda.return_DataTable("select COUNT(*) from ser_reportlog where Field3='" + strGroupId + "' and (SELECT top 1 cast(value as date) from  dbo.F_Split(Field2,',')  order by cast(value as date) desc) = '" + Convert.ToDateTime(strscheduleDate) + "' and Status='Failed' ").Rows[0][0].ToString();

    }


    protected void btnHistory_Click(object sender, EventArgs e)
    {
        try
        {
            Convert.ToDateTime(txtFromDate.Text);
        }
        catch
        {
            DisplayMessage("From date is invalid");
            txtFromDate.Focus();
            return;
        }


        try
        {
            Convert.ToDateTime(txtToDate.Text);
        }
        catch
        {
            DisplayMessage("to date is invalid");
            txtToDate.Focus();
            return;
        }

        DataTable dt = new DataTable();

        if (ddlGroupName.SelectedIndex > 0)
        {
            dt = Objda.return_DataTable("select ser_groupMaster.Group_Name, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Email_Id,ser_reportlog.Generate_date,ser_reportlog.Status  from ser_reportlog inner join Set_EmployeeMaster on ser_reportlog.emp_id =Set_EmployeeMaster.emp_id inner join ser_groupMaster on ser_reportlog.Field3=ser_groupMaster.Trans_Id  where (SELECT top 1 cast(value as date) from  dbo.F_Split(ser_reportlog.Field2,',')  order by cast(value as date) desc) between '" + Convert.ToDateTime(txtFromDate.Text) + "' and '" + Convert.ToDateTime(txtToDate.Text) + "' and ser_reportlog.Field3='" + ddlGroupName.SelectedValue.Trim() + "' order by ser_groupMaster.Group_Name, cast(set_employeemaster.emp_id as int)");
        }
        else
        {
            dt = Objda.return_DataTable("select ser_groupMaster.Group_Name, Set_EmployeeMaster.Emp_Id, Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Email_Id,ser_reportlog.Generate_date,ser_reportlog.Status  from ser_reportlog inner join Set_EmployeeMaster on ser_reportlog.emp_id =Set_EmployeeMaster.emp_id inner join ser_groupMaster on ser_reportlog.Field3=ser_groupMaster.Trans_Id  where (SELECT top 1 cast(value as date) from  dbo.F_Split(ser_reportlog.Field2,',')  order by cast(value as date) desc) between '" + Convert.ToDateTime(txtFromDate.Text) + "' and '" + Convert.ToDateTime(txtToDate.Text) + "'  order by ser_groupMaster.Group_Name, cast(set_employeemaster.emp_id as int)");
        }



        if (dt != null)
        {

            if (ddlStatus.SelectedIndex > 0)
            {
                dt = new DataView(dt, "Status='" + ddlStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        Lbl_TotalRecords.Text = "Total Records: " + dt.Rows.Count.ToString();
        Lbl_TotalRecords.Visible = true;
        gvEmailStatus.DataSource = dt;
        gvEmailStatus.DataBind();
    }

    public void FillGroupName()
    {


        DataTable dtGroup = objGroup.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        dtGroup = new DataView(dtGroup, "", "Group_Name", DataViewRowState.CurrentRows).ToTable();
        ddlGroupName.DataSource = dtGroup;
        ddlGroupName.DataTextField = "Group_Name";
        ddlGroupName.DataValueField = "Trans_Id";
        ddlGroupName.DataBind();

        ddlGroupName.Items.Insert(0, "--Select--");

    }



    protected void rbtnManagerNotification_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnManagerNotification.Checked)
        {
            pnlEmpNf.Visible = true;
        }
        else
        {
            pnlEmpNf.Visible = false;
        }
    }
}