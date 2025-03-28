using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Attendance_Report_SalaryReport : BasePage
{
    Common ObjComman = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    // Filter Criteria According to Location and Department
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    DepartmentMaster objDept = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    //------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjComman = new Common(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        //Filter Criteria According to Location and Department
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "231", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            pnlEmpAtt.Visible = true;
            FillddlDeaprtment();
            FillddlLocation();
            FillGrid();
            rbtnEmpSal.Checked = true;
            rbtnGroupSal.Checked = false;
            EmpGroupSal_CheckedChanged(null, null);
            //pnlReport.Visible = false;
            //if (Session["EmpList"] != null)
            //{
            //    pnlEmpAtt.Visible = false;
            //    //pnlReport.Visible = true;
            //}
            Session["CHECKED_ITEMS"] = null;
            GetReportPermission();
            ddlLocation.Focus();



        }
        txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
        CalendarExtender1.Format = objSys.SetDateFormat();


    }



    private void GetReportPermission()
    {
        //New Code 

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("231", (DataTable)Session["ModuleName"]);
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
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        //End Code
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "73", HttpContext.Current.Session["CompId"].ToString());
        for (int i = 0; i < dtAllPageCode.Rows.Count; i++)
        {
            btnLogProc.Visible = true;
        }
    }
    protected void btnLogProc_Click(object sender, EventArgs e)
    {
        string url = "../Attendance/LogProcess.aspx";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
        ImageButton1.Focus();
        return;
    }
    #region Filter Criteria According to Location and Department



    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

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
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ListItem li = new ListItem("--Select--", "0");
            ddlLocation.Items.Insert(0, li);


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

        string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }

        dt = dt.DefaultView.ToTable(true, "DeptName", "Dep_Id");



        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            //Common Function add By Lokesh on 23-05-2015
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

    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (ddlLocation.SelectedIndex == 0 && dpDepartment.SelectedIndex == 0)
                {
                    FillGrid();
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
                        gvEmployee.DataSource = dt;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                        dpDepartment.Focus();
                    }
                    else
                    {
                        gvEmployee.DataSource = null;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                    }
                }





            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }

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
                    FillGrid();
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
                        gvEmployee.DataSource = dt;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
                        btnLogProc.Focus();
                    }
                    else
                    {
                        gvEmployee.DataSource = null;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

                    }
                }





            }
        }
        catch (Exception Ex)
        {
            DisplayMessage("Select Company");
        }

    }
    public void FillGrid()
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
        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

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
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        }
    }
    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        ddlLocation.Focus();


    }
    #endregion
    protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            CheckBox chkBxSelect = (CheckBox)e.Row.Cells[0].FindControl("chkBxSelect");
            CheckBox chkBxHeader = (CheckBox)this.gvEmployee.HeaderRow.FindControl("chkBxHeader");
            HiddenField hdnFldId = (HiddenField)e.Row.Cells[0].FindControl("hdnFldId");

            chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,document.getElementById('{0}'),'{1}');", chkBxHeader.ClientID, hdnFldId.Value.Trim());
        }
    }

    protected void ddlField_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlOption.Focus();
        return;
    }

    protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtValue.Focus();
        return;
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
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            }

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
                gvEmployeeSal.DataSource = dtEmp;
                gvEmployeeSal.DataBind();

            }
            else
            {
                Session["dtEmp4"] = null;
                gvEmployeeSal.DataSource = dtEmp;
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
            pnlEmp.Visible = false;
            pnlGroupSal.Visible = true;

            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            pnlSearchdpl.Visible = false;

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
                lbxGroupSal.DataSource = dtGroup;
                lbxGroupSal.DataTextField = "Group_Name";
                lbxGroupSal.DataValueField = "Group_Id";

                lbxGroupSal.DataBind();

            }
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();


            lbxGroupSal_SelectedIndexChanged(null, null);
        }
        else if (rbtnEmpSal.Checked)
        {
            pnlEmp.Visible = true;
            pnlGroupSal.Visible = false;

            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            lblGroupByDept.Visible = true;
            dpDepartment.Visible = true;
            pnlSearchdpl.Visible = true;


            lblEmp.Text = "";
            lblSelectRecord.Text = "";
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
                Session["dtEmp4"] = dtEmp;

                gvEmployee.DataSource = dtEmp;
                gvEmployee.DataBind();

            }
            else
            {
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();

            }

        }


    }
    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        gvEmployeeSal.DataSource = (DataTable)Session["dtEmp4"];
        gvEmployeeSal.DataBind();
    }
    //NEW FUNCTION
    private void SaveCheckedValues()
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
    private void PopulateCheckedValues()
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
        SaveCheckedValues();
        gvEmployee.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dt;
        gvEmployee.DataBind();
        PopulateCheckedValues();
    }


    protected void ImgbtnSelectAll_Clickary(object sender, ImageClickEventArgs e)
    {
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
            gvEmployee.DataSource = dtAddressCategory1;
            gvEmployee.DataBind();
            ViewState["Select"] = null;
        }
    }




    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {

        SaveCheckedValues();
        lblSelectRecord.Text = "";
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
        {

            userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (userdetails.Count == 0)
            {
                //DisplayMessage("Select Employee First");
                //return;
            }
            else
            {


                for (int i = 0; i < userdetails.Count; i++)
                {
                    lblSelectRecord.Text += userdetails[i].ToString() + ",";
                }

            }

        }
        else
        {
            //DisplayMessage("Select Employee First");
            //return;
        }
        string empidlist = lblSelectRecord.Text;

        //
        int b = 0;
        // Selected Emp Id 

        //  string empidlist = hdnFldSelectedValues.Value.Trim();


        try
        {
            if (txtFromDate.Text == "")
            {
                DisplayMessage("From Date Required");
                txtFromDate.Focus();
                return;
            }
            if (txtToDate.Text == "")
            {
                DisplayMessage("To Date Required");
                txtToDate.Focus();
                return;
            }

            if (objSys.getDateForInput(txtFromDate.Text.ToString()) > objSys.getDateForInput(txtToDate.Text.ToString()))
            {
                DisplayMessage("To Date should be greater");
                txtToDate.Text = "";
                txtToDate.Focus();
                return;
            }
        }
        catch (Exception)
        {

            DisplayMessage("Date not in proper format");

            return;
        }


        if (rbtnEmpSal.Checked)
        {
            if (empidlist == "")
            {
                DisplayMessage("Select Atleast One Employee");
                gvEmployee.Focus();
                return;
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
                empidlist = "";
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
                        empidlist += str + ",";

                    }
                }

                if (empidlist == "")
                {
                    DisplayMessage("Employees are not exists in selected groups");
                    return;
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }

        }
        Session["EmpList"] = empidlist;

        Session["FromDate"] = txtFromDate.Text;
        Session["ToDate"] = txtToDate.Text;
       // pnlEmpAtt.Visible = false;
        //Response.Redirect("~/Attendance_Report/SalarySummaryReport.aspx");
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('SalarySummaryReport.aspx','','height=650,width=950,scrollbars=Yes')", true);
        // pnlReport.Visible = true;

        //for (int i = 0; i < empidlist.Split(',').Length; i++)
        //{
        //    if (empidlist.Split(',')[i] == "")
        //    {
        //        continue;
        //    }

        //}       








    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Session["EmpList1"] = null;
        Session["FromDate1"] = null;
        Session["ToDate1"] = null;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        Session["CHECKED_ITEMS"] = null;

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
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        }

        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
        btnaryRefresh_Click1(null, null);
        ddlLocation.Focus();
    }

    protected void btnarybind_Click1(object sender, ImageClickEventArgs e)
    {
        if (txtValue.Text == "")
        {
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
            gvEmployee.DataSource = view.ToTable();
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            ImageButton9.Focus();
        }
        txtValue.Focus();
    }
    protected void btnaryRefresh_Click1(object sender, ImageClickEventArgs e)
    {
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        Session["CHECKED_ITEMS"] = null;

        DataTable dtEmp = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dtEmp;
        gvEmployee.DataBind();
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        ddlField.Focus();

    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvEmployee.Rows)
        {
            index = (int)gvEmployee.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }



        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }

}
