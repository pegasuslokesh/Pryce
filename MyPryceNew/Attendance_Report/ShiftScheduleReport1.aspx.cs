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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class AttendanceReports_ShiftScheduleReport1 : BasePage
{
    BrandMaster objBrand = null;
    Common ObjComman = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    Att_ScheduleMaster objEmpSch = null;
    Att_ShiftDescription objShift = null;
    Att_Leave_Request ObjLeaveReq = null;
    Set_Employee_Holiday objEmpHoli = null;
    CompanyMaster objComp = null;
    Set_AddressChild ObjAddress = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    RoleMaster objRole = null;


    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        sm.RegisterPostBackControl(btnLogProcess);


        sm.RegisterPostBackControl(btnExportPdf);



        sm.RegisterPostBackControl(btnExportToExcel);
        //New Code 

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("134", (DataTable)Session["ModuleName"]);
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
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objShift = new Att_ShiftDescription(Session["DBConnection"].ToString());
        ObjLeaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objEmpHoli = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "134", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
            pnlReport.Visible = false; Session["FromDate1"] = null;
            Session["FromDate1"] = null;
            Session["CHECKED_ITEMS"] = null;
            Session["ToDate1"] = null;
            GetReportPermission();
        }


        if (Session["EmpList"] == null)
        {
            Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
        }
        else
        {

            GetReport();
        }


        txtFrom_CalendarExtender.Format = objSys.SetDateFormat();
        CalendarExtender1.Format = objSys.SetDateFormat();



    }

    private void GetReportPermission()
    {
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), "9", "73", HttpContext.Current.Session["CompId"].ToString());
        for (int i = 0; i < dtAllPageCode.Rows.Count; i++)
        {
            btnLogProc.Visible = true;
        }
    }



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
            ddlLocation.Items.Insert(0, "--Select");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, "--Select");
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select");
            }
        }
    }

    private void FillddlDeaprtment()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        // Modified By Nitin Jain On 25-09-2014

        dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        //---------------------------------------

        if (dt.Rows.Count > 0)
        {
            dpDepartment.DataSource = null;
            dpDepartment.DataBind();
            dpDepartment.DataSource = dt;
            dpDepartment.DataTextField = "DeptName";
            dpDepartment.DataValueField = "Dep_Id";
            dpDepartment.DataBind();
            dpDepartment.Items.Insert(0, "--Select--");
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
    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        lblSelectRecord.Text = "";
        ddlField.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        ddlLocation.SelectedIndex = 0;
        dpDepartment.SelectedIndex = 0;
        FillGrid();
        Session["dtLeave"] = null;

    }
    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        try
        {
            if (Session["CompId"].ToString() != null || Session["CompId"].ToString() != string.Empty)
            {
                if (dpDepartment.SelectedIndex == 0)
                {
                    lblSelectRecord.Text = "";
                    FillGrid();
                }
                else
                {
                    dt = objEmp.GetEmployeeOrDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dpDepartment.SelectedValue.ToString(), "1");

                    if (dt.Rows.Count > 0)
                    {
                        gvEmployee.DataSource = dt;
                        gvEmployee.DataBind();
                        Session["dtEmp"] = dt;
                        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

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
            //DisplayMessage("Select Company");
        }
    }
    public void FillGrid()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dpDepartment.SelectedIndex != 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + dpDepartment.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

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

    protected void btnLogProc_Click(object sender, EventArgs e)
    {
        string url = "../Attendance/LogProcess.aspx";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
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

    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["SalaryReportSelected"] = 0;
        if (rbtnGroupSal.Checked)
        {
            pnlEmp.Visible = false;
            pnlGroupSal.Visible = true;

            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            pnlserachdepartment.Visible = false;
            Label1.Visible = false;

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
            pnlserachdepartment.Visible = true;
            Label1.Visible = true;
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
        //OLD CODE
        //gvEmployee.PageIndex = e.NewPageIndex;
        //DataTable dtEmp = (DataTable)Session["dtEmp"];
        //gvEmployee.DataSource = dtEmp;
        //gvEmployee.DataBind();
        //string temp = string.Empty;


        //for (int i = 0; i < gvEmployee.Rows.Count; i++)
        //{
        //    Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
        //    string[] split = lblSelectRecord.Text.Split(',');

        //    for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
        //    {
        //        if (lblSelectRecord.Text.Split(',')[j] != "")
        //        {
        //            if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
        //            {
        //                ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
        //            }
        //        }
        //    }
        //}
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


        //OLD CODE
        //DataTable dtProduct = (DataTable)Session["dtEmp"];

        //if (ViewState["Select"] == null)
        //{
        //    ViewState["Select"] = 1;
        //    foreach (DataRow dr in dtProduct.Rows)
        //    {
        //        if (!lblSelectRecord.Text.Split(',').Contains(dr["Emp_Id"]))
        //        {
        //            lblSelectRecord.Text += dr["Emp_Id"] + ",";
        //        }
        //    }
        //    for (int i = 0; i < gvEmployee.Rows.Count; i++)
        //    {
        //        string[] split = lblSelectRecord.Text.Split(',');
        //        Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
        //        for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
        //        {
        //            if (lblSelectRecord.Text.Split(',')[j] != "")
        //            {
        //                if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
        //                {
        //                    ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    lblSelectRecord.Text = "";
        //    DataTable dtProduct1 = (DataTable)Session["dtEmp"];
        //    gvEmployee.DataSource = dtProduct1;
        //    gvEmployee.DataBind();
        //    ViewState["Select"] = null;
        //}

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
        //
        int b = 0;
        // Selected Emp Id 
        string empidlist = lblSelectRecord.Text;
        Session["EmpList1"] = empidlist;
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




                return;
            }
        }
        catch (Exception)
        {

            DisplayMessage("Date not in proper format");

            return;
        }

        int m1 = 0;
        int m2 = 0;
        int y1 = 0;
        int y2 = 0;

        m1 = objSys.getDateForInput(txtFromDate.Text.ToString()).Month;
        m2 = objSys.getDateForInput(txtToDate.Text.ToString()).Month;
        y1 = objSys.getDateForInput(txtFromDate.Text.ToString()).Year;
        y2 = objSys.getDateForInput(txtToDate.Text.ToString()).Year;
        if (m1 != m2 && y1 == y2)
        {
            DisplayMessage("From Date and To Date should be of same month year");

            return;

        }


        if (m1 == m2 && y1 != y2)
        {
            DisplayMessage("From Date and To Date should be of same month year");

            return;

        }

        if (m1 != m2 && y1 != y2)
        {
            DisplayMessage("From Date and To Date should be of same month year");

            return;

        }


        Session["FromDate1"] = txtFromDate.Text;
        Session["ToDate1"] = txtToDate.Text;
        ViewState["SalaryReportSelected"] = "1";

        if (rbtnEmpSal.Checked)
        {
            if (empidlist == "")
            {
                DisplayMessage("Select Atleast One Employee");
                return;
            }
            else
            {
                Session["SelectedEmpId"] = empidlist;
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
                else
                {
                    Session["EmpList1"] = empidlist;
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }

        }

        pnlEmpAtt.Visible = false;
        pnlReport.Visible = true;

        Panel1.Visible = true;

        GetReport();

    }

    public void GetReport()
    {


        pnlEmpAtt.Visible = false;
        pnlReport.Visible = true;

        Panel1.Visible = true;

        DataTable dtTime = new DataTable();


        dtTime.Columns.Add("EmpCode");
        dtTime.Columns.Add("EmpName");
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;

        FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
        ToDate = objSys.getDateForInput(Session["ToDate"].ToString());

        Emplist = Session["EmpList"].ToString();
        //if (lblSelectRecord.Text == "")
        //{
        //    if (Session["EmpList1"] != null)
        //    {
        //        Emplist = Session["EmpList1"].ToString();

        //    }

        //}


        DataTable dtFilter = new DataTable();

        AttendanceDataSet rptdata = new AttendanceDataSet();

        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_ScheduleDescription_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Att_ScheduleDescription_Report, Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Emplist);



        if (Emplist != "")
        {
            dtFilter = new DataView(rptdata.sp_Att_ScheduleDescription_Report, "Emp_Id in (" + Emplist.Substring(0, Emplist.Length - 1) + ") ", "Att_Date", DataViewRowState.CurrentRows).ToTable();
        }

        int dayscount = 0;

        dayscount = Convert.ToDateTime(ToDate).Subtract(Convert.ToDateTime(FromDate)).Days;

        for (int r = 1; r <= dayscount + 1; r++)
        {

            dtTime.Columns.Add("Date" + r.ToString());

        }



        DataTable dtEmpCode = new DataTable();
        try
        {
            dtEmpCode = dtFilter.DefaultView.ToTable(true, "Emp_Id");

        }
        catch
        {
        }

        DataTable dtPerDate = new DataTable();

        DataTable dtDate = new DataTable();





        try
        {
            dtPerDate = dtFilter.DefaultView.ToTable(true, "Att_Date");
        }
        catch
        {

        }

        int j = 0;

        DataRow dr = dtTime.NewRow();
        while (j < dtPerDate.Rows.Count)
        {






            //dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date"].ToString()).ToString("dd-MMM-yyyy") + "  " + Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date"].ToString()).DayOfWeek.ToString().Substring(0, 3);
            dr["Date" + (j + 1).ToString()] = Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date"].ToString()).Day.ToString() + "  " + Convert.ToDateTime(dtPerDate.Rows[j]["Att_Date"].ToString()).DayOfWeek.ToString().Substring(0, 3);

            j++;


        }
        dr["EmpCode"] = "Code";
        dr["EmpName"] = "Name";


        dtTime.Rows.Add(dr);
        int j1 = 0;
        if (dtEmpCode.Rows.Count > 0)
        {

            for (int i = 0; i < dtEmpCode.Rows.Count; i++)
            {
                j1 = 0;
                DataTable dtDate1 = new DataTable();
                dtDate = new DataView(dtFilter, "Emp_Id='" + dtEmpCode.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataRow dr1 = dtTime.NewRow();
                if (dtDate.Rows.Count > 0)
                {

                    dr1["EmpCode"] = dtDate.Rows[0]["Emp_Code"].ToString();
                    dr1["EmpName"] = dtDate.Rows[0]["Emp_Name"].ToString();


                    while (j1 < dtPerDate.Rows.Count)
                    {

                        dtDate1 = new DataView(dtDate, "Att_Date='" + dtPerDate.Rows[j1]["Att_Date"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                        if (objEmpHoli.GetEmployeeHolidayOnDateAndEmpId(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString()))
                        {
                            dr1["Date" + (j1 + 1).ToString()] = "Holiday";
                            j++;


                        }
                        else if (ObjLeaveReq.IsLeaveOnDate(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString()))
                        {
                            string strLeaveName = string.Empty;

                            strLeaveName = ObjLeaveReq.GetLeavetypeName(Convert.ToDateTime(dtPerDate.Rows[j1]["Att_Date"].ToString()).ToString(), dtEmpCode.Rows[i]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                            if (!strLeaveName.ToUpper().ToString().Contains("LEAVE"))
                            {
                                strLeaveName += " Leave";
                            }


                            dr1["Date" + (j1 + 1).ToString()] = strLeaveName;
                            j++;

                        }


                        else if (dtDate1.Rows.Count > 0)
                        {
                            if (dtDate1.Rows.Count > 1)
                            {
                                string Time = string.Empty;

                                dtDate1 = new DataView(dtDate1, "", "OnDuty_Time", DataViewRowState.CurrentRows).ToTable();

                                for (int t = 0; t < dtDate1.Rows.Count; t++)
                                {
                                    try
                                    {
                                        Time += Convert.ToDateTime(dtDate1.Rows[t]["OnDuty_Time"].ToString()).ToString("HH:mm") + "-" + Convert.ToDateTime(dtDate1.Rows[t]["OffDuty_Time"].ToString()).ToString("HH:mm") + "  ";
                                        dr1["Date" + (j1 + 1).ToString()] = Time;

                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    dr1["Date" + (j1 + 1).ToString()] = Convert.ToDateTime(dtDate1.Rows[0]["OnDuty_Time"].ToString()).ToString("HH:mm") + "-" + Convert.ToDateTime(dtDate1.Rows[0]["OffDuty_Time"]).ToString("HH:mm");
                                }
                                catch
                                {

                                    if (Convert.ToBoolean(dtDate1.Rows[0]["Is_Off"].ToString()))
                                    {

                                        dr1["Date" + (j1 + 1).ToString()] = "Week Off";
                                    }

                                }
                            }


                        }





                        j1++;


                    }







                }

                dtTime.Rows.Add(dr1);


            }


        }




        Map(dtTime);



        lblTitle.Text = "Employee Shift Schedule Report" + " From " + FromDate.ToString(objSys.SetDateFormat()) + " To " + ToDate.ToString(objSys.SetDateFormat());
        if (ddlLocation.SelectedIndex == 0)
        {
            lblLocation.Text = "All";
        }
        else
        {
            lblLocation.Text = ddlLocation.SelectedItem.Text;
        }
        if (dpDepartment.SelectedIndex == 0)
        {
            lblDept.Text = "All";
        }
        else
        {
            lblDept.Text = dpDepartment.SelectedItem.Text;
        }

        DataTable DtBrand = objBrand.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                lblBrand.Text = DtBrand.Rows[0]["Brand_Name"].ToString();
            }
            else
            {
                lblBrand.Text = DtBrand.Rows[0]["Brand_Name_L"].ToString();

            }
        }
    }


    public void Map(System.Data.DataTable
     dataTable)
    {
        int i = 0;

        foreach (System.Data.DataRow dataRow in dataTable.Rows)
        {
            TableRow webRow = new TableRow();
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                System.Web.UI.WebControls.TableCell webCell = new
                System.Web.UI.WebControls.TableCell();

                Label label = new Label();
                string colname = dataColumn.ColumnName;
                label.Text = dataRow[colname].ToString();
                if (colname == "EmpName")
                {

                    label.Style.Add(HtmlTextWriterStyle.OverflowX, "hidden");

                }
                else
                {
                    label.Width = 90;
                }

                if (label.Text == "Week Off")
                {
                    webCell.BackColor = System.Drawing.Color.SeaGreen;


                }
                if (label.Text == "Holiday")
                {
                    webCell.BackColor = System.Drawing.Color.Red;

                }
                if (label.Text == "Leave")
                {
                    webCell.BackColor = System.Drawing.Color.Yellow;

                }
                if (i == 0)
                {
                    webCell.BackColor = System.Drawing.Color.SkyBlue;
                }

                webCell.Controls.Add(label);
                webRow.Cells.Add(webCell);
            }
            i++;
            Table1.Rows.Add(webRow);
        }


    }

    protected void btnExportPdf_Command(object sender, CommandEventArgs e)
    {

        if (e.CommandArgument.ToString() == "1")
        {
            GetReport();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=ShiftScheduleReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            ExportPanel1.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A2, 0f, 0f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();

        }
        else if (e.CommandArgument.ToString() == "2")
        {
            GetReport();
            btnExportPdf.Visible = false;
            btnExportToExcel.Visible = false;
            ExportPanel1.OpenInBrowser = true;
            ExportPanel1.ExportType = ControlFreak.ExportPanel.AppType.Excel;

        }
    }
    protected void lnkback_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
        pnlEmpAtt.Visible = true;
        btnReset_Click(null, null);
        Session["EmpList1"] = null;
        Session["FromDate1"] = null;
        Session["ToDate1"] = null;
        ViewState["Select"] = null;
    }


    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        Session["EmpList1"] = null;
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
    }


    protected void btnarybind_Click1(object sender, ImageClickEventArgs e)
    {
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
//OLD CODE
//    CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
//    for (int i = 0; i < gvEmployee.Rows.Count; i++)
//    {
//        ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
//        if (chkSelAll.Checked)
//        {
//            if (!lblSelectRecord.Text.Split(',').Contains(((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
//            {
//                lblSelectRecord.Text += ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + ",";
//            }
//        }
//        else
//        {
//            string temp = string.Empty;
//            string[] split = lblSelectRecord.Text.Split(',');
//            foreach (string item in split)
//            {
//                if (item != ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString())
//                {
//                    if (item != "")
//                    {
//                        temp += item + ",";
//                    }
//                }
//            }
//            lblSelectRecord.Text = temp;
//        }
//    }


//}


