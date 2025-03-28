using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using PegasusDataAccess;
using System.Web;

public partial class Attendance_ManualAttendance : BasePage
{
    Att_AttendanceLog objAttLog = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Leave_Request ObjLeave = null;
    DataAccessClass Objda = null;
    PageControlCommon objPageCmn = null;
    string strForByVerified = string.Empty;
    protected string UploadFolderPath = "~/Uploads/";
    static string locIds;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }



        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjLeave = new Att_Leave_Request(Session["DBConnection"].ToString());
        Objda = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "61", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            fillLocation();
            ddlLocationList.SelectedValue = Session["LocId"].ToString();
            FillddlDeaprtment();
            FillddlLocation();
            rbtnByManual.Checked = true;
            rbtnByTour.Checked = false;
            Session["CHECKED_ITEMS"] = null;
            CalendarExtender1.Format = objSys.SetDateFormat();
            CalendarExtender2.Format = objSys.SetDateFormat();
        }
        txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
        AllPageCode();
    }


    public void FillGrid()
    {

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

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Department_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
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
            objPageCmn.FillData((object)gvEmployee, dt, "", "");
            Session["dtEmp"] = dt;
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            dpDepartment.Focus();
        }
        else
        {
            btnAllRefresh.Focus();
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
            Session["dtEmp"] = dt;
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        }
        Session["CHECKED_ITEMS"] = null;


    }

    #region Filter Criteria According to Location and Department

    private void FillddlLocation()
    {

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


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
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
        string DepIds = String.Empty;
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

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
            dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        //}

        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            objPageCmn.FillData((object)dpDepartment, dt, "Dep_Name", "Dep_Id");
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
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlDeaprtment();
        FillGrid();

    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        Session["CHECKED_ITEMS"] = null;

        FillGrid();



    }
    #endregion
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("61", (DataTable)Session["ModuleName"]);
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
            btnSearch.Visible = true;
            btnSave.Visible = true; ImageButton1.Visible = true;
            imgBtnRestore.Visible = true;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "61", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {
                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "5")
                        {
                            btnSearch.Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnSave.Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "4")
                        {
                            imgBtnRestore.Visible = true;
                            ImgbtnSelectAll.Visible = false;
                        }
                        if (DtRow["Op_Id"].ToString() == "3")
                        {
                            ImageButton1.Visible = true;
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
                Session["dtEmp4"] = dtEmp;
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmployeeSal, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp4"] = null;
                gvEmployeeSal.DataSource = null;
                gvEmployeeSal.DataBind();
            }
        }
        else
        {
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();

        }
    }
    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroupSal.Checked)
        {

            Div_Group.Visible = true;
            Div_Employee.Visible = false;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
            //--------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)lbxGroupSal, dtGroup, "Group_Name", "Group_Id");
            }
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();
            lbxGroupSal_SelectedIndexChanged(null, null);
        }
        else if (rbtnEmpSal.Checked)
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            FillGrid();

        }
    }
    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployeeSal, (DataTable)Session["dtEmp4"], "", "");
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmpLog.Rows)
        {
            index = (int)gvEmpLog.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmpLog.Rows)
            {
                int index = (int)gvEmpLog.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvLogBin.Rows)
        {
            index = (int)gvLogBin.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvLogBin.Rows)
            {
                int index = (int)gvLogBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }



    private void SaveCheckedValuesemp()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValuesemp()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmployee.Rows)
            {
                int index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemp();
        gvEmployee.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmp"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
        PopulateCheckedValuesemp();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        if (gvLogBin.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = objAttLog.RestoreAttendanceLog(Session["CompId"].ToString(), userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                    }

                    if (Msg != 0)
                    {
                        //  FillGrid();
                        // FillGridBin();
                        btnSearchBin_Click(null, null);
                        ViewState["Select"] = null;
                        lblSelectRecordBin.Text = "";
                        DisplayMessage("Record Activated");
                        Session["CHECKED_ITEMS"] = null;
                    }
                    else
                    {
                        DisplayMessage("Record Not Activated");
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                return;
            }
        }
        //  FillMannualLogGrid();
    }


    protected void chkgvSelectAllBin_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckkseall = ((CheckBox)gvLogBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvLogBin.Rows)
        {
            if (ckkseall.Checked)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }

    protected void chkgvSelect_CheckedChanged1(object sender, EventArgs e)
    {

        //this code is created by jitendra upadhyay pn 10-09-2014
        //this code for check validation before delete log that log posted or not
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];


        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;

        Label Transid = (Label)gvEmpLog.Rows[index].FindControl("lblTransId");
        Label lblEmpId = (Label)gvEmpLog.Rows[index].FindControl("lblEmpid");
        Label lblEventDate = (Label)gvEmpLog.Rows[index].FindControl("lblItemType");


        DateTime Event_Date = Convert.ToDateTime(lblEventDate.Text);

        DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lblEmpId.Text, Event_Date.Month.ToString(), Event_Date.Year.ToString());
        if (dtPostedList.Rows.Count > 0)
        {
            DisplayMessage("Log Posted So You Can not Delete Log");
            ((CheckBox)gvEmpLog.Rows[index].FindControl("chkgvSelect")).Checked = false;
            if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
            {
                userdetails.Remove(Convert.ToInt32(Transid.Text));
            }
            return;
        }
        else
        {
            if (((CheckBox)gvEmpLog.Rows[index].FindControl("chkgvSelect")).Checked == true)
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

        }

        Session["CHECKED_ITEMS"] = userdetails;


    }


    //protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    string empidlist = string.Empty;
    //    string temp = string.Empty;
    //    int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
    //    Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
    //    if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
    //    {
    //        empidlist += lb.Text.Trim().ToString() + ",";
    //        lblSelectRecord.Text += empidlist;

    //    }

    //    else
    //    {

    //        empidlist += lb.Text.ToString().Trim();
    //        lblSelectRecord.Text += empidlist;
    //        string[] split = lblSelectRecord.Text.Split(',');
    //        foreach (string item in split)
    //        {
    //            if (item != empidlist)
    //            {
    //                if (item != "")
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //        }
    //        lblSelectRecord.Text = temp;
    //    }
    //}




    protected void ImgbtnSelectAll_Clickary(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAttendenceReport = (DataTable)Session["dtEmp"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAttendenceReport.Rows)
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
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmp"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void ImgbtnDeleteAll_Click1(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (gvEmpLog.Rows.Count == 0)
        {

            DisplayMessage("No Data Exists");
            return;
        }

        if (gvEmpLog.Rows.Count != 0)
        {
            SaveCheckedValuesemplog();
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


                            objAttLog.DeleteAttendanceLog(userdetails[i].ToString());



                        }
                    }


                    lblSelectRecord1.Text = "";
                    DisplayMessage("Record Deleted");
                    btnSearch_Click(null, null);
                    btnarybind_Click(null, null);
                    //FillGrid();
                    // btnaryRefresh_Click(null, null);
                    // FillGridBin();

                }
            }
            else
            {
                DisplayMessage("Please Select Record ");
                gvEmpLog.Focus();
                return;
            }
        }
    }
    protected void ImgbtnSelectAll_Click1(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        string PostedEmpLIst = string.Empty;
        ArrayList userdetails = new ArrayList();
        ArrayList PostedEmpListarr = new ArrayList();
        DataTable dtAttendenceReport = (DataTable)Session["dtEmpLogFilter"];
        if (dtAttendenceReport != null && dtAttendenceReport.Rows.Count > 0)
        {
            if (ViewState["Select"] == null)
            {
                Session["CHECKED_ITEMS"] = null;
                ViewState["Select"] = 1;
                foreach (DataRow dr in dtAttendenceReport.Rows)
                {
                    //this code is created by jitendra upadhyay on 10-09-2014
                    //this code for check validation before delete that log posted or not for selected employee
                    //code start
                    int Counter = 0;


                    DateTime Event_Date = Convert.ToDateTime(dr["Event_Date"].ToString());

                    DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(dr["Emp_Id"].ToString(), Event_Date.Month.ToString(), Event_Date.Year.ToString());
                    if (dtPostedList.Rows.Count > 0)
                    {
                        Counter = 1;
                        if (!PostedEmpListarr.Contains(GetEmployeeCode(dr["Emp_Id"].ToString())))
                        {
                            PostedEmpListarr.Add(GetEmployeeCode(dr["Emp_Id"].ToString()));
                            if (PostedEmpLIst == "")
                            {
                                PostedEmpLIst = GetEmployeeCode(dr["Emp_Id"].ToString());
                            }
                            else
                            {

                                PostedEmpLIst = PostedEmpLIst + "," + GetEmployeeCode(dr["Emp_Id"].ToString());
                            }
                        }


                    }

                    //Allowance_Id

                    // Check in the Session
                    if (Session["CHECKED_ITEMS"] != null)
                        userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                    if (Counter == 0)
                    {
                        if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                            userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));
                    }

                }
                foreach (GridViewRow gvrow in gvEmpLog.Rows)
                {
                    Label Transid = (Label)gvrow.FindControl("lblTransId");
                    if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
                        ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

                }
                if (userdetails != null && userdetails.Count > 0)
                    Session["CHECKED_ITEMS"] = userdetails;

            }
            else
            {
                Session["CHECKED_ITEMS"] = null;
                DataTable dtAddressCategory1 = (DataTable)Session["dtEmpLogFilter"];
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmpLog, dtAddressCategory1, "", "");
                ViewState["Select"] = null;
            }
            if (PostedEmpLIst != "")
            {
                DisplayMessage("Log Posted For Employee :- " + PostedEmpLIst.ToString().TrimEnd() + " ");
            }
            //code end
        }
    }

    public void FillFunctionKeydropdown()
    {
        //this function  is created by jitendra upadhyay on 10-09-2014
        //this function for get the function key value according the company parameter 



        ddlFunction.Items.Clear();
        ListItem item = new ListItem();
        ListItem item1 = new ListItem();
        ListItem item2 = new ListItem();
        ListItem item3 = new ListItem();
        ListItem item4 = new ListItem();
        ListItem item5 = new ListItem();
        item.Text = "In(F1)";
        item.Value = objAppParam.GetApplicationParameterValueByParamName("In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        item1.Text = "Out(F2)";
        item1.Value = objAppParam.GetApplicationParameterValueByParamName("Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        item2.Text = "Break In(F3)";
        item2.Value = objAppParam.GetApplicationParameterValueByParamName("Break In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        item3.Text = "Break Out(F4)";
        item3.Value = objAppParam.GetApplicationParameterValueByParamName("Break Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        item4.Text = "Partial Leave In(F5)";
        item4.Value = objAppParam.GetApplicationParameterValueByParamName("Partial Leave In  Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        item5.Text = "Partial Leave Out(F6)";
        item5.Value = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Out  Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        ddlFunction.Items.Insert(0, item);
        ddlFunction.Items.Insert(1, item1);
        ddlFunction.Items.Insert(2, item2);
        ddlFunction.Items.Insert(3, item3);
        ddlFunction.Items.Insert(4, item4);
        ddlFunction.Items.Insert(5, item5);
        ddlFunction.SelectedIndex = 0;

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {

        if (objAppParam.GetApplicationParameterValueByParamName("In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
        {
            DisplayMessage("First set Function Key Value in Company Parameter");
            return;
        }
        if (objAppParam.GetApplicationParameterValueByParamName("Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
        {
            DisplayMessage("First set Function Key Value in Company Parameter");
            return;
        }
        if (objAppParam.GetApplicationParameterValueByParamName("Break In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
        {
            DisplayMessage("First set Function Key Value in Company Parameter");
            return;
        }


        if (objAppParam.GetApplicationParameterValueByParamName("Break Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
        {
            DisplayMessage("First set Function Key Value in Company Parameter");
            return;
        }
        if (objAppParam.GetApplicationParameterValueByParamName("Partial Leave In  Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
        {
            DisplayMessage("First set Function Key Value in Company Parameter");
            return;
        }
        if (objAppParam.GetApplicationParameterValueByParamName("Partial Leave Out  Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == "")
        {
            DisplayMessage("First set Function Key Value in Company Parameter");
            return;
        }


        FillFunctionKeydropdown();

        ddlLocation.Focus();
        rbtnByManual.Checked = true;
        rbtnByTour.Checked = false;
        rbtnByPartialLeave.Checked = false;
        rbtnByHalfDay.Checked = false;

        lblSelectRecord1.Text = "";
        txtToDate.Text = "";
        txtFromDate.Text = "";
        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
        AllPageCode();
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
    public bool Check24Format(TextBox txt, string ErrorMessage)
    {

        if (!System.Text.RegularExpressions.Regex.IsMatch(txt.Text.ToString(), "^((0?[1-9]|1[012])(:[0-5]\\d){0,2}(\\ [AP]M))$|^([01]\\d|2[0-3])(:[0-5]\\d){0,2}$"))
        {
            DisplayMessage(ErrorMessage);
            txt.Focus();
            return false;
        }
        else
        {
            return true; ;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strEmpCode = string.Empty;
        string PostedEmpId = string.Empty;
        string SavedEmployee = string.Empty;
        string strLeave = string.Empty;
        int b = 0;
        string Verifiedtype = string.Empty;

        if (rbtnByManual.Checked)
        {
            Verifiedtype = "By Manual";
        }
        else if (rbtnByTour.Checked)
        {
            Verifiedtype = "By Tour";
        }

        else if (rbtnByHalfDay.Checked)
        {
            Verifiedtype = "By Half Day";
        }

        else if (rbtnByPartialLeave.Checked)
        {
            Verifiedtype = "By Partial Leave";
        }
        //dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(empidlist.Split(',')[i], FromDate.AddDays(-1).ToString(), ToDate.AddDays(1).ToString());

        DataTable dtLog = new DataTable();




        //if (gvEmployee.Rows.Count != 0)
        //{

        if (txtDate.Text.Trim() == "")
        {
            txtDate.Focus();
            DisplayMessage("Please Fill Date");

            return;
        }
        if (txtOnDuty.Text.Trim() == "")
        {
            txtOnDuty.Focus();
            DisplayMessage("Please Fill Time");

            return;
        }
        try
        {
            objSys.getDateForInput(txtDate.Text);
        }
        catch (Exception)
        {
            txtDate.Focus();
            DisplayMessage("Date Not In Proper Format");

            return;
        }

        if (!Check24Format(txtOnDuty, "Invalid Time Format"))
        {
            txtOnDuty.Focus();
            return;
        }


        DateTime EventTime = new DateTime();
        EventTime = new DateTime(objSys.getDateForInput(txtDate.Text).Year, objSys.getDateForInput(txtDate.Text).Month, objSys.getDateForInput(txtDate.Text).Day, Convert.ToDateTime(txtOnDuty.Text).Hour, Convert.ToDateTime(txtOnDuty.Text).Minute, Convert.ToDateTime(txtOnDuty.Text).Second);
        string EmpId = string.Empty;
        if (rbtnEmpSal.Checked)
        {

            SaveCheckedValuesemp();
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Please Select Record");
                gvEmployee.Focus();
                return;
            }

            ArrayList empidlist = (ArrayList)Session["CHECKED_ITEMS"];

            if (empidlist.Count == 0)
            {
                DisplayMessage("Select Atleast One Employee");
                return;
            }
            for (int i = 0; i < empidlist.Count; i++)
            {
                if (empidlist[i] == "")
                {
                    continue;
                }

                DateTime OnDate = Convert.ToDateTime(txtDate.Text.Trim().ToString());
                if (!IsLogAllowOnleave(OnDate, empidlist[i].ToString()))
                {
                    strEmpCode = GetEmployeeCode(empidlist[i].ToString());

                    if (strEmpCode != "No Code")
                    {
                        strLeave += strEmpCode + ",";
                        continue;
                    }

                }

                dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(empidlist[i].ToString(), OnDate.ToString(), OnDate.ToString());


                dtLog = new DataView(dtLog, "Event_Time1='" + EventTime.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLog.Rows.Count > 0)
                {
                    strEmpCode = GetEmployeeCode(empidlist[i].ToString());


                    if (strEmpCode != "No Code")
                    {
                        EmpId += strEmpCode + ",";
                    }
                }

                else
                {


                    DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(empidlist[i].ToString(), Convert.ToDateTime(txtDate.Text).Month.ToString(), Convert.ToDateTime(txtDate.Text).Year.ToString());
                    if (dtPostedList.Rows.Count > 0)
                    {
                        strEmpCode = GetEmployeeCode(empidlist[i].ToString());

                        if (strEmpCode != "No Code")
                        {
                            PostedEmpId += strEmpCode + ",";
                        }

                    }
                    else
                    {
                        strEmpCode = GetEmployeeCode(empidlist[i].ToString());
                        if (strEmpCode != "No Code")
                        {
                            SavedEmployee += strEmpCode + ",";
                            b = objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), empidlist[i].ToString(), "", objSys.getDateForInput(txtDate.Text).ToString(), EventTime.ToString(), ddlFunction.SelectedValue, ddlType.SelectedValue, Verifiedtype, true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        }

                    }
                }
            }
        }
        else
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;





            for (int i = 0; i < lbxGroupSal.Items.Count; i++)
            {
                if (lbxGroupSal.Items[i].Selected)
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
                foreach (string str in EmpIds.Split(','))
                {
                    if (str != "")
                    {
                        DateTime OnDate = Convert.ToDateTime(txtDate.Text);

                        if (!IsLogAllowOnleave(OnDate, str))
                        {
                            strEmpCode = GetEmployeeCode(str);
                            if (strEmpCode != "No Code")
                            {
                                strLeave += strEmpCode + ",";
                                continue;
                            }

                        }


                        dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(str, OnDate.ToString(), OnDate.ToString());


                        dtLog = new DataView(dtLog, "Event_Time1='" + EventTime.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtLog.Rows.Count > 0)
                        {
                            strEmpCode = GetEmployeeCode(str);
                            if (strEmpCode != "No Code")
                            {
                                EmpId += strEmpCode + ",";
                            }
                        }
                        else
                        {
                            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(str, Convert.ToDateTime(txtDate.Text).Month.ToString(), Convert.ToDateTime(txtDate.Text).Year.ToString());
                            if (dtPostedList.Rows.Count > 0)
                            {
                                strEmpCode = GetEmployeeCode(str);
                                if (strEmpCode != "No Code")
                                {
                                    PostedEmpId += strEmpCode + ",";
                                }

                            }
                            else
                            {
                                strEmpCode = GetEmployeeCode(str);
                                if (strEmpCode != "No Code")
                                {
                                    SavedEmployee += strEmpCode + ",";
                                }

                                b = objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), str, "", objSys.getDateForInput(txtDate.Text).ToString(), EventTime.ToString(), ddlFunction.SelectedValue, ddlType.SelectedValue, Verifiedtype, true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                            }

                        }


                    }
                }

            }
            else
            {
                DisplayMessage("Select Group First");
            }
        }

        string Message = string.Empty;
        if (SavedEmployee != "")
        {
            Message = "Record Saved For Employee:- " + SavedEmployee;
        }


        if (EmpId != "")
        {
            Message += " Log Exists For Employee Codes:- " + EmpId;

        }
        if (PostedEmpId != "")
        {
            Message += " Log Posted For Employee Codes " + PostedEmpId;

        }

        if (strLeave != "")
        {
            Message += " Leave Exist For Employee Codes " + strLeave;

        }


        if (Message != "")
        {
            DisplayMessage(Message);


        }

        else
        {
            DisplayMessage("Record Saved", "green");


            lblSelectRecord1.Text = "";
            lblTotalRecord.Text = "";
            txtToDate.Text = "";
            txtFromDate.Text = "";
            // FillGrid();
            //btnList_Click(null, null);
            // btnaryRefresh_Click(null, null);
            txtOnDuty.Text = "";
            txtOnDuty.Focus();
            btnAllRefresh_Click(null, null);
            btnNew_Click(null, null);


        }


        //}


    }

    public bool IsLogAllowOnleave(DateTime dtLogDate, string strEmpId)
    {
        bool Result = true;

        DataTable dt = new DataTable();

        try
        {
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_ManualLog_OnLeave", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString()))
            {

                dt = Objda.return_DataTable("select Emp_Id from dbo.Att_Leave_Request where Emp_Id=" + strEmpId + " and ('" + dtLogDate + "' between From_Date and To_Date ) and (Is_Approved='True' or Is_Pending='True')");

                if (dt.Rows.Count > 0)
                {
                    Result = false;

                }

            }
        }
        catch
        {


        }



        return Result;

    }
    public string DateFormat(string Date)
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

    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        rbtnByManual.Checked = true;
        rbtnByTour.Checked = false;
        rbtnByPartialLeave.Checked = false;
        rbtnByHalfDay.Checked = false;
        txtDate.Text = "";
        txtOnDuty.Text = "";
        try
        {
            ddlFunction.SelectedIndex = 0;
        }
        catch
        {
        }
        ddlType.SelectedIndex = 0;
        lblSelectRecord.Text = "";
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
    }

    protected void btnarybind_Click1(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (txtValue.Text.Trim().ToString() == string.Empty)
        {
            DisplayMessage("Please Fill Value");
            txtValue.Focus();
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
            DataTable dtEmp = (DataTable)Session["dtEmp"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, view.ToTable(), "", "");

            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            ImgBtnRefreshList.Focus();
            return;
        }
        txtValue.Focus();
    }
    protected void btnaryRefresh_Click1(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        //btnList_Click(null, null);
        ddlField1.Focus();
        //return;

        //gvEmployee.DataSource = dtEmp;
        //gvEmployee.DataBind();

    }


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox ckkseall = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvEmployee.Rows)
        {
            if (ckkseall.Checked)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }




    }

    protected void chkgvSelectAll_CheckedChanged1(object sender, EventArgs e)
    {
        string PostedEmpLIst = string.Empty;
        ArrayList PostedEmpListarr = new ArrayList();
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        CheckBox chkSelAll = ((CheckBox)gvEmpLog.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvEmpLog.Rows)
        {
            Label Transid = (Label)gr.FindControl("lblTransId");
            Label lblEmpId = (Label)gr.FindControl("lblEmpid");
            Label lblEventDate = (Label)gr.FindControl("lblItemType");


            DateTime Event_Date = Convert.ToDateTime(lblEventDate.Text);
            DataTable dtPostedList = objAttLog.Get_Pay_Employee_Attendance(lblEmpId.Text, Event_Date.Month.ToString(), Event_Date.Year.ToString());
            if (dtPostedList.Rows.Count > 0)
            {
                if (!PostedEmpListarr.Contains(GetEmployeeCode(lblEmpId.Text)))
                {
                    PostedEmpListarr.Add(GetEmployeeCode(lblEmpId.Text));
                    if (PostedEmpLIst == "")
                    {
                        PostedEmpLIst = GetEmployeeCode(lblEmpId.Text);
                    }
                    else
                    {

                        PostedEmpLIst = PostedEmpLIst + "," + GetEmployeeCode(lblEmpId.Text);
                    }
                }

                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
                if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
                {
                    userdetails.Remove(Convert.ToInt32(Transid.Text));
                }
            }
            else
            {
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


        }

        Session["CHECKED_ITEMS"] = userdetails;
        if (PostedEmpLIst != "")
        {
            DisplayMessage("Log Posted For Employee :- " + PostedEmpLIst.ToString().TrimEnd() + " ");

        }

    }

    #region uploadlog
    protected void btnConnect_Click(object sender, EventArgs e)
    {
        int fileType = -1;

        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                Literal l4 = new Literal();
                l4.Text = @"<font size=4 color=red></font><script>alert(""Please load a excel/access file"");</script></br></br>";
                this.Controls.Add(l4);
                return;
            }

            if (ext == ".xls")
            {
                fileType = 0;
            }
            if (ext == ".xlsx")
            {
                fileType = 1;
            }
            if (ext == ".mdb")
            {
                fileType = 2;
            }
            if (ext == ".accdb")
            {
                fileType = 3;
            }
            string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + fileLoad.FileName);
            fileLoad.SaveAs(path);

            //DataTable dt//
            Import(path, fileType);
            //if (dt != null)
            //{
            //  //  gvLoadContact.DataSource = dt;
            //   // Session["dtContact"] = dt;
            //   // gvLoadContact.DataBind();
            //}
            Literal l5 = new Literal();
            l5.Text = @"<font size=4 color=red></font><script>alert(""file succesfully uploaded"");</script></br></br>";
            this.Controls.Add(l5);
        }
        else
        {
            Literal l4 = new Literal();
            l4.Text = @"<font size=4 color=red></font><script>alert(""Please load a  file"");</script></br></br>";
            this.Controls.Add(l4);
        }
        fileLoad.FileContent.Dispose();
        //tr0.Visible = true;
    }
    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=NO;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=NO;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }
        Session["cnn"] = strcon;

        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();

        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;

        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();
        // OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", conn);
        //create new dataset
        //  DataSet ds = new DataSet();
        // fill dataset
        // da.Fill(ds);
        //populate grid with data
        //this.gvLoadContact.DataSource = ds.Tables[0];
        ////close connection
        conn.Close();

        //return ds.Tables[0];
    }
    protected void btnviewcolumns_Click(object sender, EventArgs e)
    {
        if (Session["cnn"] != null)
        {

            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + ddlTables.SelectedValue.ToString() + "]";
            adp.SelectCommand.Connection = cnn;

            DataTable userTable = new DataTable();
            try
            {
                adp.Fill(userTable);
            }
            catch (Exception)
            {
                Literal l4 = new Literal();
                l4.Text = @"<font size=4 color=red></font><script>alert(""Error in Mapping File"");</script></br></br>";
                this.Controls.Add(l4);
                return;
            }
            int counter = 0;
            DataTable dtSourceData = new DataTable();
            dtSourceData = userTable.Copy();
            //for (int i = 0; i < dtSourceData.Rows.Count; i++)
            //{
            //    if (i > 98)
            //    {
            //        if (counter == 1)
            //        {
            //            break;
            //        }
            //        for (int j = 0; j < 5; j++)
            //        {
            //            if (dtSourceData.Rows[i][j].ToString() == "")
            //            {
            //                counter = 1;
            //                break;

            //            }
            //        }
            //    }

            //}
            //if (counter == 1)
            //{
            //    Literal l4 = new Literal();
            //    l4.Text = @"<font size=4 color=red></font><script>alert(""Excel Sheet should not be blank,so check it and upload again"");</script></br></br>";
            //    this.Controls.Add(l4);
            //    //lblMessage.Text = "Column should not be blank,so check it and upload again";
            //    //return;
            //}
            //else
            //{
            lblMessage.Text = "";

            Session["SourceData"] = userTable;
            DataTable dtcolumn = new DataTable();
            dtcolumn.Columns.Add("COLUMN_NAME");
            dtcolumn.Columns.Add("COLUMN");
            for (int i = 0; i < userTable.Columns.Count; i++)
            {
                dtcolumn.Rows.Add(dtcolumn.NewRow());
                if (Session["filetype"].ToString() != "excel")
                {
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Columns[i].ToString();
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
                }
                else
                {
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Rows[0][i].ToString();
                    dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
                }
            }

            Session["SourceTbl"] = dtcolumn;
            //get destination table field 
            DataTable dtDestinationDt = objAttLog.GetFieldName();

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvFieldMapping, dtDestinationDt, "", "");

            //get source field
            div_Grid.Visible = true;

            div_Grid.Visible = true;


        }
    }
    protected void btnUpload_Click2(object sender, EventArgs e)
    {
        string query = "";
        //// get columns name
        DataTable dtSource = (DataTable)Session["SourceData"];

        DataTable dtDestTemp = new DataTable();
        for (int col = 0; col < gvFieldMapping.Rows.Count; col++)
        {
            if (((DropDownList)gvFieldMapping.Rows[col].FindControl("ddlExcelCol")).SelectedValue != "0")
            {
                dtDestTemp.Columns.Add(((Label)gvFieldMapping.Rows[col].FindControl("lblColName")).Text);
            }
        }

        for (int rowcountr = 0; rowcountr < dtSource.Rows.Count; rowcountr++)
        {
            dtDestTemp.Rows.Add(dtDestTemp.NewRow());

            for (int i = 0; i < gvFieldMapping.Rows.Count; i++)
            {
                if (((DropDownList)gvFieldMapping.Rows[i].FindControl("ddlExcelCol")).SelectedValue != "0")
                {
                    dtDestTemp.Rows[rowcountr][((Label)gvFieldMapping.Rows[i].FindControl("lblColName")).Text] = dtSource.Rows[rowcountr][((DropDownList)gvFieldMapping.Rows[i].FindControl("ddlExcelCol")).SelectedValue].ToString();
                }
            }
        }
        //EEmpFirstName','DOJ','DepartmentId','DesignationId','DOB','BrandId','LocationId','EmpId
        if (dtDestTemp.Columns.Contains("Emp_Id") && dtDestTemp.Columns.Contains("Event_Date") && dtDestTemp.Columns.Contains("Event_Time") && dtDestTemp.Columns.Contains("Func_Code") && dtDestTemp.Columns.Contains("Type"))
        {
            ddlFiltercol.DataSource = dtDestTemp.Columns;
        }
        else
        {
            DisplayMessage("Map all Necessary Field");
            return;
        }

        Div_showdata.Visible = true;
        div_Grid.Visible = false;
        //ddlFiltercol.DataTextField = "Column_Name";
        //ddlFiltercol.DataValueField = "Column_Name";
        ddlFiltercol.DataBind();

        Session["dtDest"] = dtDestTemp;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvSelected, dtDestTemp, "", "");


    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        div_Grid.Visible = false;
        Div_showdata.Visible = false;
        ddlTables.Items.Clear();
    }
    protected void gvFieldMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string nec = gvFieldMapping.DataKeys[e.Row.RowIndex]["Nec"].ToString();
            if (nec.Trim() == "1")
            {
                ((Label)e.Row.FindControl("lblCompulsery")).Text = "*";
                ((Label)e.Row.FindControl("lblCompulsery")).ForeColor = System.Drawing.Color.Red;
            }
            DropDownList ddl = ((DropDownList)e.Row.FindControl("ddlExcelCol"));
            binddropdownlist(ddl);
        }
    }
    private void binddropdownlist(DropDownList ddl)
    {
        DataTable dt = (DataTable)Session["SourceTbl"];

        string filetype = Session["filetype"].ToString();
        int startingrow = 0;
        if (filetype == "excel")
            startingrow = 1;
        ListItem lst = new ListItem("--select one--", "0");

        if (ddl != null)
        {
            ddl.Items.Insert(0, lst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst = new ListItem(dt.Rows[i]["COLUMN_NAME"].ToString(), dt.Rows[i]["COLUMN"].ToString());
                ddl.Items.Insert(i + 1, lst);
                //lst=new ListItem()
            }
        }
    }
    protected void btnUpload_Click1(object sender, EventArgs e)
    {

        //objPageCmn.FillData((object)gvSelected, null, "", "");

        DataTable dtDuplicateRecords = new DataTable();
        dtDuplicateRecords.Columns.Add("Emp_Id");
        dtDuplicateRecords.Columns.Add("Event_Date");
        dtDuplicateRecords.Columns.Add("Event_Time");
        dtDuplicateRecords.Columns.Add("Func_Code");
        dtDuplicateRecords.Columns.Add("Type");
        string empids = string.Empty;
        string empidnotexists = string.Empty;
        string strLeaveExists = string.Empty;
        string Date;
        string Month;
        string Year;
        string ActualDate;
        int Insertedrowcount = 0;
        DateTime date_Of_Joining = new DateTime();
        string compid = Session["CompId"].ToString();
        DataTable dtLog = new DataTable();
        string strsql = string.Empty;
        for (int rowcounter = 1; rowcounter < gvSelected.Rows.Count; rowcounter++)
        {
            int counter = 0;

            SetAllId();
            for (int col = 0; col < gvSelected.Rows[rowcounter].Cells.Count; col++)
            {
                string colname = gvSelected.HeaderRow.Cells[col].Text;

                string colval = gvSelected.Rows[rowcounter].Cells[col].Text;

                colval = colval.Replace("&#160;", "");

                colval = colval.Replace("&nbsp;", "");
                if (colval == "")
                {
                    counter = 1;
                    break;
                }

                if (colname == "Emp_Id")
                {
                    objAttLog.Emp_Id = colval;
                }
                if (colname == "Event_Date")
                {
                    objAttLog.Event_Date = colval;
                }
                if (colname == "Event_Time")
                {
                    DateTime dt = Convert.ToDateTime(objAttLog.Event_Date);
                    DateTime dtEventTime = Convert.ToDateTime(colval);

                    DateTime newdate = new DateTime(dt.Year, dt.Month, dt.Day, dtEventTime.Hour, dtEventTime.Minute, dtEventTime.Second);

                    objAttLog.Event_Time = newdate.ToString();
                }
                if (colname == "Func_Code")
                {
                    objAttLog.Func_Code = colval;
                }
                if (colname == "Type")
                {
                    objAttLog.Type1 = colval;
                }
            }

            if (counter == 1)
            {
                continue;
            }

            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), objAttLog.Emp_Id);
            int b = 0;
            if (dtEmp.Rows.Count > 0)
            {
                DateTime OnDate = Convert.ToDateTime(objAttLog.Event_Date);
                DateTime EventDate = Convert.ToDateTime(objAttLog.Event_Time);
                strsql = "SELECT Set_EmployeeMaster.Emp_Code FROM Att_DeviceMaster RIGHT OUTER JOIN Att_AttendanceLog ON Att_DeviceMaster.Device_Id = Att_AttendanceLog.Device_Id LEFT OUTER JOIN Set_DepartmentMaster RIGHT OUTER JOIN Set_EmployeeMaster ON Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id ON Att_AttendanceLog.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE (Att_AttendanceLog.Company_Id = " + Session["CompId"].ToString() + ") and  (Set_EmployeeMaster.Brand_Id = " + Session["BrandId"].ToString() + ")  and (Set_EmployeeMaster.Location_Id = " + Session["LOcId"].ToString() + ")  and (Att_AttendanceLog.Emp_Id='" + dtEmp.Rows[0]["Emp_Id"].ToString() + "') AND (Att_AttendanceLog.IsActive = 'True') and Att_AttendanceLog.event_date>='" + objAttLog.Event_Date + "' and Att_AttendanceLog.event_date<='" + objAttLog.Event_Date + "' and CONVERT(VARCHAR(5),Att_AttendanceLog.event_time, 108)='" + EventDate.ToString("HH:mm") + "'";
                //DataTable dtLog = objAttLog.GetAttendanceLog(Session["CompId"].ToString());
                dtLog = Objda.return_DataTable(strsql);

                //dtLog = new DataView(dtLog, "Emp_Id='" + dtEmp.Rows[0]["Emp_Id"].ToString() + "' and Event_Date='" + OnDate + "' and Event_Time1='" + EventDate.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLog.Rows.Count > 0)
                {
                    empids += GetEmployeeCode(objAttLog.Emp_Id) + ",";
                    DataRow dr = dtDuplicateRecords.NewRow();
                    dr[0] = objAttLog.Emp_Id;
                    dr[1] = objAttLog.Event_Date;
                    dr[2] = EventDate.ToString("HH:mm:ss");
                    dr[3] = objAttLog.Func_Code;
                    dr[4] = objAttLog.Type1;
                    dtDuplicateRecords.Rows.Add(dr);

                }
                else if (!IsLogAllowOnleave(OnDate, dtEmp.Rows[0]["Emp_Id"].ToString()))
                {
                    strLeaveExists += GetEmployeeCode(dtEmp.Rows[0]["Emp_Id"].ToString()) + ",";
                    continue;

                }
                else
                {
                    objAttLog.Emp_Id = dtEmp.Rows[0]["Emp_Id"].ToString();
                    b = objAttLog.SaveAttendanceLog();
                }
            }
            else
            {
                empidnotexists += objAttLog.Emp_Id + ",";

            }


            if (b != 0)
            {
                Insertedrowcount++;
            }


        }



        string strMessge = string.Empty;


        strMessge = Insertedrowcount + " Row Inserted";


        if (empidnotexists != "")
        {
            strMessge += " and Following Employee Id does not exists" + "(" + empidnotexists + ")";
        }


        if (strLeaveExists != "")
        {
            strMessge += " and leave exist  for employee code " + "(" + strLeaveExists + ")";
        }

        if (empids != "")
        {
            strMessge += " and  some Log record already exist which is showing in below list";
        }


        if (empids != "")
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvSelected, dtDuplicateRecords, "", "");
        }
        else
        {
            objPageCmn.FillData((object)gvSelected, null, "", "");
            Reset_Upload();
        }
        DisplayMessage(strMessge);



    }
    public void SetAllId()
    {
        objAttLog.CompanyId = Session["CompId"].ToString();
        objAttLog.Device_Id = "0";
        objAttLog.Verified_Type = "By Manual";
        objAttLog.IsActive = true.ToString();
        objAttLog.CreatedBy = Session["UserId"].ToString();
        objAttLog.CreatedDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        ViewState["Select"] = null;
        div_Grid.Visible = false;
        Div_showdata.Visible = false;
        ddlTables.Items.Clear();
        lblMessage.Text = "";
        Session["cnn"] = null;

        // FillGridBin();

    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtDest"];

        dt = new DataView(dt, "" + ddlFiltercol.SelectedValue + "='" + txtfiltercol.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvSelected, dt, "", "");
    }
    protected void btnresetgv_Click(object sender, EventArgs e)
    {
        Div_showdata.Visible = false;
        div_Grid.Visible = true;
        txtfiltercol.Text = "";

    }
    protected void btnBackToMapData_Click(object sender, EventArgs e)
    {
        Div_showdata.Visible = false;
        div_Grid.Visible = false;
        ddlTables.Items.Clear();
        //trmap.Visible = false;
        //trnew.Visible = false;

    }
    #endregion

    protected void FileUploadComplete(object sender, EventArgs e)
    {
        //string filename = System.IO.Path.GetFileName(fileLoad.FileName);
        //fileLoad.SaveAs(Server.MapPath(this.UploadFolderPath) + filename);

        int fileType = -1;

        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }

    public void Reset_Upload()
    {
        fileLoad.FileContent.Dispose();
        ddlTables.Items.Clear();
        div_Grid.Visible = false;
        Div_showdata.Visible = false;
    }

    protected void ddlField1_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlField1.SelectedValue == "Event_Date")
        {
            txtVal1.Text = "";
            Txt_Val_Date.Text = "";
            txtVal1.Visible = false;
            Txt_Val_Date.Visible = true;
            Ddl_Val_Type.Visible = false;
            Ddl_Val_Function_Key.Visible = false;
            Ddl_Val_Verified_By.Visible = false;
        }
        else if (ddlField1.SelectedValue == "Type")
        {
            txtVal1.Text = "";
            Txt_Val_Date.Text = "";
            txtVal1.Visible = false;
            Txt_Val_Date.Visible = false;
            Ddl_Val_Type.Visible = true;
            Ddl_Val_Function_Key.Visible = false;
            Ddl_Val_Verified_By.Visible = false;
        }
        else if (ddlField1.SelectedValue == "Func_Code")
        {
            txtVal1.Text = "";
            Txt_Val_Date.Text = "";
            txtVal1.Visible = false;
            Txt_Val_Date.Visible = false;
            Ddl_Val_Type.Visible = false;
            Ddl_Val_Function_Key.Visible = true;
            Ddl_Val_Verified_By.Visible = false;
        }
        else if (ddlField1.SelectedValue == "Verified_Type")
        {
            txtVal1.Text = "";
            Txt_Val_Date.Text = "";
            txtVal1.Visible = false;
            Txt_Val_Date.Visible = false;
            Ddl_Val_Type.Visible = false;
            Ddl_Val_Function_Key.Visible = false;
            Ddl_Val_Verified_By.Visible = true;
        }
        else
        {
            txtVal1.Text = "";
            Txt_Val_Date.Text = "";
            txtVal1.Visible = true;
            Txt_Val_Date.Visible = false;
            Ddl_Val_Type.Visible = false;
            Ddl_Val_Function_Key.Visible = false;
            Ddl_Val_Verified_By.Visible = false;
        }
    }

    protected void ddlbinFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (ddlbinFieldName.SelectedValue == "Event_Date")
        {
            txtbinValue.Text = "";
            Txt_Bin_Val_Date.Text = "";
            txtbinValue.Visible = false;
            Txt_Bin_Val_Date.Visible = true;
            Ddl_Bin_Val_Type.Visible = false;
            Ddl_Bin_Val_Function_Key.Visible = false;
            Ddl_Bin_Val_Verified_By.Visible = false;
        }
        else if (ddlbinFieldName.SelectedValue == "Type")
        {
            txtbinValue.Text = "";
            Txt_Bin_Val_Date.Text = "";
            txtbinValue.Visible = false;
            Txt_Bin_Val_Date.Visible = false;
            Ddl_Bin_Val_Type.Visible = true;
            Ddl_Bin_Val_Function_Key.Visible = false;
            Ddl_Bin_Val_Verified_By.Visible = false;
        }
        else if (ddlbinFieldName.SelectedValue == "Func_Code")
        {
            txtbinValue.Text = "";
            Txt_Bin_Val_Date.Text = "";
            txtbinValue.Visible = false;
            Txt_Bin_Val_Date.Visible = false;
            Ddl_Bin_Val_Type.Visible = false;
            Ddl_Bin_Val_Function_Key.Visible = true;
            Ddl_Bin_Val_Verified_By.Visible = false;
        }
        else if (ddlbinFieldName.SelectedValue == "Verified_Type")
        {
            txtbinValue.Text = "";
            Txt_Bin_Val_Date.Text = "";
            txtbinValue.Visible = false;
            Txt_Bin_Val_Date.Visible = false;
            Ddl_Bin_Val_Type.Visible = false;
            Ddl_Bin_Val_Function_Key.Visible = false;
            Ddl_Bin_Val_Verified_By.Visible = true;
        }
        else
        {
            txtbinValue.Text = "";
            Txt_Bin_Val_Date.Text = "";
            txtbinValue.Visible = true;
            Txt_Bin_Val_Date.Visible = false;
            Ddl_Bin_Val_Type.Visible = false;
            Ddl_Bin_Val_Function_Key.Visible = false;
            Ddl_Bin_Val_Verified_By.Visible = false;
        }
    }


    // Change By ghanshyam Suthar on 21-02-2018
    protected void btnList_Click(object sender, EventArgs e)
    {
        txtFromDate.Focus();
        ddlVerified.SelectedIndex = 0;
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        Session["CHECKED_ITEMS"] = null;
        //FillGrid();
        btnReset_Click(null, null);
        lblSelectRecord1.Text = "";
        lblTotalRecord.Text = "";
        txtToDate.Text = "";
        txtFromDate.Text = "";
        //FillMannualLogGrid();
    }
    private void FillMannualLogGrid()
    {
        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DataTable dt = Session["dtEmpLog"] as DataTable;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmpLog, dt, "", "");
            Session["dtEmpLogFilter"] = dt;

            lblTotalRecord1.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

            //Update On 02-06-2015 For Verified Tyee Coloumn
            DataTable dtVerifiedCode = objAppParam.GetApplicationParameterByCompanyId("ForManualAttendance", Session["CompId"].ToString());
            dtVerifiedCode = new DataView(dtVerifiedCode, "Param_Name='ForManualAttendance'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVerifiedCode.Rows.Count > 0)
            {
                strForByVerified = dtVerifiedCode.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strForByVerified = "True";
            }

            if (strForByVerified == "False")
            {
                try
                {
                    gvEmpLog.Columns[9].Visible = false;
                }
                catch
                {

                }
            }
        }
    }
    public void FillGrid1()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


        if (ddlLocation.SelectedIndex == 0)
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

        }

        if (dpDepartment.SelectedIndex != 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        //if (Session["SessionDepId"] != null)
        //{
        //    dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        //}
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpLeave"] = dtEmp;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }

        dtEmp.Dispose();

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {

        ViewState["Select"] = null;
        txtbinValue.Focus();
        txtFromBin.Text = "";
        txtToBin.Text = "";
        txtFromBin.Focus();
        ddlVerifiedBin.SelectedIndex = 0;
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        string strsql = string.Empty;
        DataTable dt = new DataTable();
        if (txtFromDate.Text == "")
        {
            DisplayMessage("Fill From Date");
            txtFromDate.Focus();
            return;
        }

        if (txtToDate.Text == "")
        {
            DisplayMessage("Fill To Date");
            txtToDate.Focus();
            return;
        }




        if (txtFromDate.Text.Trim() == "")
        {
            DisplayMessage("Fill From Date");
            txtFromDate.Focus();
            return;
        }
        if (txtToDate.Text.Trim() == "")
        {
            DisplayMessage("Fill To Date");
            txtToDate.Focus();
            return;
        }

        try
        {
            objSys.getDateForInput(txtFromDate.Text);
        }
        catch (Exception)
        {
            txtFromDate.Focus();
            DisplayMessage("From Date Not In Proper Format");
            return;
        }

        try
        {
            objSys.getDateForInput(txtToDate.Text);
        }
        catch (Exception)
        {
            txtToDate.Focus();
            DisplayMessage("To Date Not In Proper Format");
            return;
        }

        if (objSys.getDateForInput(txtToDate.Text) < objSys.getDateForInput(txtFromDate.Text))
        {
            txtFromDate.Focus();
            DisplayMessage("From Date Can Not be Greater then To Date");
            return;
        }



        if (ddlLocationList.SelectedValue == "0")
        {
            fillLocation();
            int last_pos = locIds.LastIndexOf(",");
            locIds = locIds.Substring(0, last_pos - 0);

            strsql = "SELECT Set_DepartmentMaster.Dep_Name,Att_AttendanceLog.Trans_Id, Att_AttendanceLog.Device_Id, Att_AttendanceLog.Emp_Id, Att_AttendanceLog.Event_Time,Att_AttendanceLog.Event_Date, Att_AttendanceLog.Func_Code, Att_AttendanceLog.Type, Att_AttendanceLog.Verified_Type, Set_EmployeeMaster.Emp_Name, Set_EmployeeMaster.Emp_Name_L, Set_EmployeeMaster.Emp_Code FROM Att_DeviceMaster RIGHT OUTER JOIN Att_AttendanceLog ON Att_DeviceMaster.Device_Id = Att_AttendanceLog.Device_Id LEFT OUTER JOIN Set_DepartmentMaster RIGHT OUTER JOIN Set_EmployeeMaster ON Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id ON Att_AttendanceLog.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE (Att_AttendanceLog.Company_Id = " + Session["CompId"].ToString() + ") and  (Set_EmployeeMaster.Brand_Id = " + Session["BrandId"].ToString() + ")  and Set_EmployeeMaster.Location_Id in (" + locIds + ") AND (Att_AttendanceLog.IsActive = 'True') and (Att_AttendanceLog.event_date>='" + Convert.ToDateTime(txtFromDate.Text).ToString() + "' and Att_AttendanceLog.event_date<='" + Convert.ToDateTime(txtToDate.Text).ToString() + "')";
        }
        else
        {
            strsql = "SELECT Set_DepartmentMaster.Dep_Name,Att_AttendanceLog.Trans_Id, Att_AttendanceLog.Device_Id, Att_AttendanceLog.Emp_Id, Att_AttendanceLog.Event_Time,Att_AttendanceLog.Event_Date, Att_AttendanceLog.Func_Code, Att_AttendanceLog.Type, Att_AttendanceLog.Verified_Type, Set_EmployeeMaster.Emp_Name, Set_EmployeeMaster.Emp_Name_L, Set_EmployeeMaster.Emp_Code FROM Att_DeviceMaster RIGHT OUTER JOIN Att_AttendanceLog ON Att_DeviceMaster.Device_Id = Att_AttendanceLog.Device_Id LEFT OUTER JOIN Set_DepartmentMaster RIGHT OUTER JOIN Set_EmployeeMaster ON Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id ON Att_AttendanceLog.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE (Att_AttendanceLog.Company_Id = " + Session["CompId"].ToString() + ") and  (Set_EmployeeMaster.Brand_Id = " + Session["BrandId"].ToString() + ")  and (Set_EmployeeMaster.Location_Id = " + ddlLocationList.SelectedValue + ") AND (Att_AttendanceLog.IsActive = 'True') and (Att_AttendanceLog.event_date>='" + Convert.ToDateTime(txtFromDate.Text).ToString() + "' and Att_AttendanceLog.event_date<='" + Convert.ToDateTime(txtToDate.Text).ToString() + "')";
        }




        //if (Session["EmpId"].ToString() != "0")
        //{
        //    strsql += " and Set_EmployeeMaster.Department_Id in (" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")";
        //}

        if (ddlVerified.SelectedIndex != 0)
        {
            strsql += " and Att_AttendanceLog.Verified_Type ='" + ddlVerified.SelectedValue + "'";

        }

        strsql += " ORDER BY Set_EmployeeMaster.Emp_Name ,Att_AttendanceLog.Event_Time  ";


        dt = Objda.return_DataTable(strsql);

        ddlField1.Focus();
        if (dt != null && dt.Rows.Count > 0)
        {
            DataTable Distinct_Type = dt.DefaultView.ToTable(true, "Type");
            DataTable Distinct_Func_Code = dt.DefaultView.ToTable(true, "Func_Code");
            DataTable Distinct_Verified_Type = dt.DefaultView.ToTable(true, "Verified_Type");
            Ddl_Val_Type.DataSource = Distinct_Type;
            Ddl_Val_Type.DataTextField = "Type";
            Ddl_Val_Type.DataBind();

            Ddl_Val_Function_Key.DataSource = Distinct_Func_Code;
            Ddl_Val_Function_Key.DataTextField = "Func_Code";
            Ddl_Val_Function_Key.DataBind();

            Ddl_Val_Verified_By.DataSource = Distinct_Verified_Type;
            Ddl_Val_Verified_By.DataTextField = "Verified_Type";
            Ddl_Val_Verified_By.DataBind();
        }

        //Common Function add By Lokesh on 22-05-2015
        if (dt != null && dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)gvEmpLog, dt, "", "");
            Session["dtEmpLog"] = dt;
            Session["dtEmpLogFilter"] = dt;
            lblTotalRecord1.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            btnResetLog.Focus();
        }
        else
        {
            gvEmpLog.DataSource = null;
            gvEmpLog.DataBind();
            DisplayMessage("Record Not Found");
            lblTotalRecord1.Text = Resources.Attendance.Total_Records + " : 0";
        }
    }
    protected void btnResetLog_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        gvEmpLog.DataSource = null;
        gvEmpLog.DataBind();
        Session["dtEmpLog"] = null;
        Session["dtEmpLogFilter"] = null;
        lblTotalRecord1.Text = Resources.Attendance.Total_Records + " : 0";
        lblSelectRecord.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtVal1.Text = "";
        Session["CHECKED_ITEMS"] = null;
        ddlField1.Focus();
        return;
    }
    protected void btnarybind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlField1.SelectedValue == "Emp_Code")
        {
            if (txtVal1.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Employee Code");
                txtVal1.Focus();
                return;
            }
        }
        else if (ddlField1.SelectedValue == "Emp_Name")
        {
            if (txtVal1.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Employee Name");
                txtVal1.Focus();
                return;
            }
        }
        else if (ddlField1.SelectedValue == "Emp_Name_L")
        {
            if (txtVal1.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Employee Local Name");
                txtVal1.Focus();
                return;
            }
        }
        else if (ddlField1.SelectedValue == "Dep_Name")
        {
            if (txtVal1.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Department");
                txtVal1.Focus();
                return;
            }
        }
        else if (ddlField1.SelectedValue == "Event_Date")
        {
            if (Txt_Val_Date.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Date");
                Txt_Val_Date.Focus();
                return;
            }
        }


        if (ddlOption1.SelectedIndex != 0)
        {
            string condition1 = string.Empty;

            if (ddlField1.SelectedValue == "Event_Date")
            {
                condition1 = "convert(" + ddlField1.SelectedValue + ",System.String)='" + Convert.ToDateTime(Txt_Val_Date.Text.Trim()) + "'";
            }
            else if (ddlField1.SelectedValue == "Type")
            {
                condition1 = "convert(" + ddlField1.SelectedValue + ",System.String)='" + Ddl_Val_Type.SelectedItem.Text.ToString() + "'";
            }
            else if (ddlField1.SelectedValue == "Func_Code")
            {
                condition1 = "convert(" + ddlField1.SelectedValue + ",System.String)='" + Ddl_Val_Function_Key.SelectedItem.Text.ToString() + "'";
            }
            else if (ddlField1.SelectedValue == "Verified_Type")
            {
                condition1 = "convert(" + ddlField1.SelectedValue + ",System.String)='" + Ddl_Val_Verified_By.SelectedItem.Text.ToString() + "'";
            }
            else
            {
                if (ddlOption1.SelectedIndex == 1)
                {
                    condition1 = "convert(" + ddlField1.SelectedValue + ",System.String)='" + txtVal1.Text.Trim() + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition1 = "convert(" + ddlField1.SelectedValue + ",System.String) like '%" + txtVal1.Text.Trim() + "%'";
                }
                else
                {
                    condition1 = "convert(" + ddlField1.SelectedValue + ",System.String) Like '" + txtVal1.Text.Trim() + "%'";
                }
            }
            DataTable dtEmp = (DataTable)Session["dtEmpLog"];
            if (dtEmp != null && dtEmp.Rows.Count > 0)
            {
                DataView view = new DataView(dtEmp, condition1, "", DataViewRowState.CurrentRows);
                Session["dtEmpLogFilter"] = view.ToTable();
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvEmpLog, view.ToTable(), "", "");
                lblTotalRecord1.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
                ImgBtnRefreshList.Focus();
            }
            return;
        }
        txtVal1.Focus();
    }
    protected void btnaryRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        lblSelectRecord.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtVal1.Text = "";
        // btnResetLog_Click(null, null);
        FillMannualLogGrid();

        Session["CHECKED_ITEMS"] = null;

        //  btnList_Click(null, null);
        ddlField1.Focus();
        return;

        //gvEmpLog.DataSource = dtEmp;
        //gvEmpLog.DataBind();

    }
    protected void gvEmpLog_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpLogFilter"];
        if (dt != null && dt.Rows.Count > 0)
        {
            DataView dv = new DataView(dt);
            string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
            dv.Sort = Query;
            dt = dv.ToTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpLog, dt, "", "");
        }

    }
    protected void gvEmpLog_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvEmpLog.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpLogFilter"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmpLog, dtEmp, "", "");
        PopulateCheckedValuesemplog();
    }







    // for bin

    protected void btnSearchBin_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();


        if (txtFromBin.Text.Trim().ToString() == "")
        {
            txtFromBin.Focus();
            DisplayMessage("Please Fill From Date");

            return;
        }
        if (txtToBin.Text.Trim().ToString() == "")
        {
            txtToBin.Focus();
            DisplayMessage("Please Fill To Date");

            return;
        }

        try
        {
            objSys.getDateForInput(txtFromBin.Text);
        }
        catch (Exception)
        {
            txtFromBin.Focus();
            DisplayMessage("From Date Not In Proper Format");

            return;
        }

        try
        {
            objSys.getDateForInput(txtToBin.Text);
        }
        catch (Exception)
        {
            txtToBin.Focus();
            DisplayMessage("To Date Not In Proper Format");

            return;
        }

        if (objSys.getDateForInput(txtToBin.Text) < objSys.getDateForInput(txtFromBin.Text))
        {
            txtFromBin.Focus();
            DisplayMessage("From Date Cannot Be Greater Than To Date");
            return;

        }
        dt = objAttLog.GetAttendanceLogInactive(Session["CompId"].ToString(), objSys.getDateForInput(txtFromBin.Text).ToString(), objSys.getDateForInput(txtToBin.Text).ToString());



        if (ddlVerifiedBin.SelectedIndex != 0)
        {
            dt = new DataView(dt, "Verified_Type='" + ddlVerifiedBin.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        DataTable Distinct_Type_Bin = dt.DefaultView.ToTable(true, "Type");
        DataTable Distinct_Func_Code_Bin = dt.DefaultView.ToTable(true, "Func_Code");
        DataTable Distinct_Verified_Type_Bin = dt.DefaultView.ToTable(true, "Verified_Type");
        Ddl_Bin_Val_Type.DataSource = Distinct_Type_Bin;
        Ddl_Bin_Val_Type.DataTextField = "Type";
        Ddl_Bin_Val_Type.DataBind();

        Ddl_Bin_Val_Function_Key.DataSource = Distinct_Func_Code_Bin;
        Ddl_Bin_Val_Function_Key.DataTextField = "Func_Code";
        Ddl_Bin_Val_Function_Key.DataBind();

        Ddl_Bin_Val_Verified_By.DataSource = Distinct_Verified_Type_Bin;
        Ddl_Bin_Val_Verified_By.DataTextField = "Verified_Type";
        Ddl_Bin_Val_Verified_By.DataBind();

        if (dt.Rows.Count > 0)
        {


            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvLogBin, dt, "", "");
            Session["dtEmpLogBin"] = dt;
            Session["dtEmpLogBinFilter"] = dt;
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        }
        else
        {
            gvLogBin.DataSource = null;
            gvLogBin.DataBind();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + "0" + "";
            Session["dtEmpLogBin"] = null;
            Session["dtEmpLogBinFilter"] = null;
            DisplayMessage("Record Not Found");
        }
    }
    public void FillGridBin()
    {
        if (txtFromBin.Text != "" && txtToBin.Text != "")
        {
            DataTable dt = Session["dtEmpLogBin"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvLogBin, dt, "", "");
                Session["dtEmpLogBin"] = dt;
                Session["dtEmpLogBinFilter"] = dt;
                lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            }
            else
            {
                Session["dtEmpLogBin"] = null;
                Session["dtEmpLogBinFilter"] = null;
                gvLogBin.DataSource = null;
                gvLogBin.DataBind();
                lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            }
        }
    }

    protected void btnResetLogBin_Click(object sender, EventArgs e)
    {
        ddlVerifiedBin.SelectedIndex = 0;
        txtFromBin.Text = "";
        txtToBin.Text = "";
        gvLogBin.DataSource = null;
        gvLogBin.DataBind();
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
        Session["dtEmpLogBinFilter"] = null;
        Session["dtEmpLogBin"] = null;
        //btnSearchBin_Click(null, null);
        //   btnBin_Click(null, null);
    }

    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedValue == "Emp_Code")
        {
            if (txtbinValue.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Employee Code");
                txtbinValue.Focus();
                return;
            }
        }
        else if (ddlbinFieldName.SelectedValue == "Emp_Name")
        {
            if (txtbinValue.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Employee Name");
                txtbinValue.Focus();
                return;
            }
        }
        else if (ddlbinFieldName.SelectedValue == "Emp_Name_L")
        {
            if (txtbinValue.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Employee Local Name");
                txtbinValue.Focus();
                return;
            }
        }
        else if (ddlbinFieldName.SelectedValue == "Dep_Name")
        {
            if (txtbinValue.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Department");
                txtbinValue.Focus();
                return;
            }
        }
        else if (ddlbinFieldName.SelectedValue == "Event_Date")
        {
            if (Txt_Bin_Val_Date.Text.Trim() == string.Empty)
            {
                DisplayMessage("Please Enter Date");
                Txt_Bin_Val_Date.Focus();
                return;
            }
        }


        if (ddlOption1.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlbinFieldName.SelectedValue == "Event_Date")
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(Txt_Bin_Val_Date.Text.Trim()) + "'";
            }
            else if (ddlbinFieldName.SelectedValue == "Type")
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + Ddl_Bin_Val_Type.SelectedItem.Text.ToString() + "'";
            }
            else if (ddlbinFieldName.SelectedValue == "Func_Code")
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + Ddl_Bin_Val_Function_Key.SelectedItem.Text.ToString() + "'";
            }
            else if (ddlbinFieldName.SelectedValue == "Verified_Type")
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + Ddl_Bin_Val_Verified_By.SelectedItem.Text.ToString() + "'";
            }
            else
            {
                if (ddlbinOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
                }
                else if (ddlbinOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text.Trim() + "%'";
                }
            }
            DataTable dtCust = (DataTable)Session["dtEmpLogBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtEmpLogBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvLogBin, view.ToTable(), "", "");
            ImgBtnRefreshList.Focus();

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
            }
            else
            {
                AllPageCode();
                ImgBtnRefreshList.Focus();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        //   FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectRecordBin.Text = "";
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAttendenceReport = (DataTable)Session["dtEmpLogBinFilter"];
        Session["CHECKED_ITEMS"] = null;
        if (dtAttendenceReport != null && dtAttendenceReport.Rows.Count > 0)
        {
            if (ViewState["Select"] == null)
            {

                ViewState["Select"] = 1;
                foreach (DataRow dr in dtAttendenceReport.Rows)
                {
                    //Allowance_Id

                    // Check in the Session
                    if (Session["CHECKED_ITEMS"] != null)
                        userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                    if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                        userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

                }
                foreach (GridViewRow gvrow in gvLogBin.Rows)
                {
                    ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

                }
                if (userdetails != null && userdetails.Count > 0)
                    Session["CHECKED_ITEMS"] = userdetails;
            }


            else
            {
                Session["CHECKED_ITEMS"] = null;
                DataTable dtAddressCategory1 = (DataTable)Session["dtEmpLogBinFilter"];
                //Common Function add By Lokesh on 22-05-2015
                objPageCmn.FillData((object)gvLogBin, dtAddressCategory1, "", "");
                ViewState["Select"] = null;
            }
        }
    }
    protected void gvLogBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpLogBinFilter"];
        if (dt == null && dt.Rows.Count == 0)
        {
            dt = (DataTable)Session["dtEmpLogBinFilter"];
        }

        if (dt != null && dt.Rows.Count > 0)
        {
            DataView dv = new DataView(dt);
            string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
            dv.Sort = Query;
            dt = dv.ToTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvLogBin, dt, "", "");
        }
    }
    protected void gvLogBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvLogBin.PageIndex = e.NewPageIndex;

        DataTable dtEmp = (DataTable)Session["dtEmpLogBinFilter"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvLogBin, dtEmp, "", "");
        PopulateCheckedValues();
    }

    public void fillLocation()
    {
        locIds = "";
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            ddlLocationList.Items.Add(new ListItem("All", "0"));
            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocationList.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));
                locIds = locIds + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
            }
        }
        else
        {
            ddlLocationList.Items.Clear();
        }
    }

}