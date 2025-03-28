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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using System.Text;

public partial class Attendance_Report_AttendanceRegister : BasePage
{
    Set_EmployeeGroup_Master objEmpGroup = null;
    SystemParameter objSys = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    Set_ApplicationParameter objAppParam = null;
    Att_AttendanceRegister objAttReg = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    Common cmn = null;
    Common ObjComman = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("113", (DataTable)Session["ModuleName"]);
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

        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objAttReg = new Att_AttendanceRegister(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "113", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            txtYear.Text = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Year.ToString();
            ddlMonth.SelectedValue = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).Month.ToString();

            pnlEmpAtt.Visible = true;
            FillddlLocation();
            FillddlDeaprtment();
            FillGrid();
            rbtnEmpSal.Checked = true;
            rbtnGroupSal.Checked = false;
            EmpGroupSal_CheckedChanged(null, null);
            pnlReport.Visible = false;

            // Created by ghanshyam suthar on 1/nov/2017
            if (Session["Report_EmpList"] != null)
            {
                Session["SelectedEmpId"] = Session["Report_EmpList"];
                ddlMonth.SelectedValue = Session["Report_FromMonth"].ToString();
                txtYear.Text = Session["Report_FromYear"].ToString();

                Session["DtShortCode"] = null;

                DataTable dtColorCode = new DataTable();
                dtColorCode.Columns.Add("Type");
                dtColorCode.Columns.Add("ShortId");
                dtColorCode.Columns.Add("ColorCode");

                DataRow dr1 = dtColorCode.NewRow();
                dr1["Type"] = "Absent";
                dr1["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr1["ShortId"] = "A";
                dtColorCode.Rows.Add(dr1);

                DataRow dr2 = dtColorCode.NewRow();
                dr2["Type"] = "Normal";
                dr2["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr2["ShortId"] = "N";
                dtColorCode.Rows.Add(dr2);


                DataRow dr3 = dtColorCode.NewRow();
                dr3["Type"] = "Holiday";
                dr3["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr3["ShortId"] = "H";
                dtColorCode.Rows.Add(dr3);

                DataRow dr4 = dtColorCode.NewRow();
                dr4["Type"] = "Leave";
                dr4["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dr4["ShortId"] = "L";
                dtColorCode.Rows.Add(dr4);
                DataRow dr5 = dtColorCode.NewRow();

                dr5["Type"] = "WeekOff";
                dr5["ColorCode"] = "66FF99";
                dr5["ShortId"] = "W";
                dtColorCode.Rows.Add(dr5);

                Session["DtShortCode"] = dtColorCode;

                //GetReport();
                pnlEmpAtt.Visible = false;
                pnlReport.Visible = true;
            }
            //----------- End By ghanshyam suthar--------------
        }
        Table tResult = new Table();
        tResult = GetReport();
        mDiv.Controls.Add(tResult);
        //if (ViewState["SalaryReportSelected"] != null && ViewState["SalaryReportSelected"].ToString() == "1")
        //{
        //    GetReport();
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
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, "--Select--");

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
        dpDepartment.Focus();
    }

    private void FillddlDeaprtment()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D",ddlLocation.SelectedValue, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

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
            DisplayMessage("Select Company");
        }
    }
    public void FillGrid()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

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
        ViewState["SalaryReportSelected"] = 0;
        if (rbtnGroupSal.Checked)
        {
            pnlEmp.Visible = false;
            pnlGroupSal.Visible = true;
            lblLocation.Visible = false;
            ddlLocation.Visible = false;
            Div_Department.Visible = false;
            Div_location.Visible = false;
            ImageButton1.Visible = false;
            lblGroupByDept.Visible = false;
            dpDepartment.Visible = false;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
            lblLocation.Visible = true;
            ddlLocation.Visible = true;
            Div_Department.Visible = true;
            Div_location.Visible = true;
            ImageButton1.Visible = true;
            lblGroupByDept.Visible = true;
            dpDepartment.Visible = true;
            pnlEmp.Visible = true;
            pnlGroupSal.Visible = false;

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

    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvEmployee.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dtEmp;
        gvEmployee.DataBind();
        string temp = string.Empty;


        for (int i = 0; i < gvEmployee.Rows.Count; i++)
        {
            Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
            string[] split = lblSelectRecord.Text.Split(',');

            for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
    }

    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
        if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectRecord.Text += empidlist;

        }

        else
        {

            empidlist += lb.Text.ToString().Trim();
            lblSelectRecord.Text += empidlist;
            string[] split = lblSelectRecord.Text.Split(',');
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
            lblSelectRecord.Text = temp;
        }
    }

    protected void ImgbtnSelectAll_Clickary(object sender, ImageClickEventArgs e)
    {
        DataTable dtProduct = (DataTable)Session["dtEmp"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtProduct.Rows)
            {
                if (!lblSelectRecord.Text.Split(',').Contains(dr["Emp_Id"]))
                {
                    lblSelectRecord.Text += dr["Emp_Id"] + ",";
                }
            }
            for (int i = 0; i < gvEmployee.Rows.Count; i++)
            {
                string[] split = lblSelectRecord.Text.Split(',');
                Label lblconid = (Label)gvEmployee.Rows[i].FindControl("lblEmpId");
                for (int j = 0; j < lblSelectRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectRecord.Text = "";
            DataTable dtProduct1 = (DataTable)Session["dtEmp"];
            gvEmployee.DataSource = dtProduct1;
            gvEmployee.DataBind();
            ViewState["Select"] = null;
        }

    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        int b = 0;
        // Selected Emp Id 
        string empidlist = lblSelectRecord.Text;

        if (ddlMonth.SelectedIndex == 0)
        {
            DisplayMessage("Please select month");
            ddlMonth.Focus();
            return;

        }
        if (txtYear.Text == "")
        {
            DisplayMessage("Please enter year");
            txtYear.Focus();
            return;

        }
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
                    Session["SelectedEmpId"] = empidlist;
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }

        }

        pnlEmpAtt.Visible = false;
        pnlReport.Visible = true;
        Session["DtShortCode"] = null;

        DataTable dtColorCode = new DataTable();
        dtColorCode.Columns.Add("Type");
        dtColorCode.Columns.Add("ShortId");
        dtColorCode.Columns.Add("ColorCode");

        DataRow dr1 = dtColorCode.NewRow();
        dr1["Type"] = "Absent";
        dr1["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Absnet_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dr1["ShortId"] = "A";
        dtColorCode.Rows.Add(dr1);

        DataRow dr2 = dtColorCode.NewRow();
        dr2["Type"] = "Normal";
        dr2["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Present_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dr2["ShortId"] = "N";
        dtColorCode.Rows.Add(dr2);


        DataRow dr3 = dtColorCode.NewRow();
        dr3["Type"] = "Holiday";
        dr3["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Holiday_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dr3["ShortId"] = "H";
        dtColorCode.Rows.Add(dr3);

        DataRow dr4 = dtColorCode.NewRow();
        dr4["Type"] = "Leave";
        dr4["ColorCode"] = objAppParam.GetApplicationParameterValueByParamName("Leave_Color_Code", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dr4["ShortId"] = "L";
        dtColorCode.Rows.Add(dr4);
        DataRow dr5 = dtColorCode.NewRow();

        dr5["Type"] = "WeekOff";
        dr5["ColorCode"] = "66FF99";
        dr5["ShortId"] = "W";
        dtColorCode.Rows.Add(dr5);

        Session["DtShortCode"] = dtColorCode;
        fillColorInTable();
        Panel1.Visible = true;

        GetReport();
        //for (int i = 0; i < empidlist.Split(',').Length; i++)
        //{
        //    if (empidlist.Split(',')[i] == "")
        //    {
        //        continue;
        //    }

        //}       








    }

    private void fillColorInTable()
    {
        DataTable dtColorCode = (DataTable)Session["DtShortCode"];
        dtColorCode = new DataView(dtColorCode, "Type in('Normal','Absent','Holiday','WeekOff','Leave')", "", DataViewRowState.CurrentRows).ToTable();

        TableRow tblHeader = new TableRow();
        TableRow tblColor = new TableRow();
        TableRow TblShortCode = new TableRow();
        TableRow TblColorCode = new TableRow();
        for (int rowcounter = 0; rowcounter < dtColorCode.Rows.Count; rowcounter++)
        {
            TableCell tch = new TableCell();
            TableCell tcc = new TableCell();
            TableCell tcsh = new TableCell();
            TableCell tccode = new TableCell();

            if (dtColorCode.Rows[rowcounter]["Type"].ToString() == "Normal")
            {
                dtColorCode.Rows[rowcounter]["Type"] = "Present";
                dtColorCode.Rows[rowcounter]["ShortId"] = "P";
            }
            if (dtColorCode.Rows[rowcounter]["Type"].ToString() == "Leave")
            {
                dtColorCode.Rows[rowcounter]["ShortId"] = "L";
            }
            if (Session["lang"].ToString() == "1")
            {
                tch.Text = dtColorCode.Rows[rowcounter]["Type"].ToString();
            }

            try
            {
                tcc.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(dtColorCode.Rows[rowcounter]["ColorCode"].ToString()).ToString());
            }
            catch
            {
                tcc.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
            }

            tcc.Height = 25;
            tcsh.Text = dtColorCode.Rows[rowcounter]["ShortId"].ToString();
            tccode.Text = "#" + dtColorCode.Rows[rowcounter]["ColorCode"].ToString();
            tblColor.Cells.Add(tcc);
            tblHeader.Cells.Add(tch);
            tcsh.HorizontalAlign = HorizontalAlign.Center;
            TblShortCode.Cells.Add(tcsh);
            TblColorCode.Cells.Add(tccode);
        }
      
    }
    public int hextoint(string hexValue)
    {
        return int.Parse(hexValue, NumberStyles.AllowHexSpecifier);
    }
    public Table GetReport()
    {
        Table Table1 = new Table();
        Table1.Style["Text-align"] = "Left";
        Table1.Style["Text-align"] = "Left";
        Table1.Style["Width"] = "500";
        // Table1.GridLines = GridLines.Both;
        Table1.Font.Size = 10;

        int Counter = 0;
        if (Session["SelectedEmpId"] == null)
        {
            return Table1;
        }

       

        DataTable dtShortCode = (DataTable)Session["DtShortCode"];


        //set color and short code
        string Normal = "P";
        string Absent = (new DataView(dtShortCode, "Type='Absent'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();

        string WeekOff = (new DataView(dtShortCode, "Type='WeekOff'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string Holiday = (new DataView(dtShortCode, "Type='Holiday'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ShortId"].ToString();
        string Leave = "L";
        string Normalcol = (new DataView(dtShortCode, "Type='Normal'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Absentcol = (new DataView(dtShortCode, "Type='Absent'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();

        string Leavecol = (new DataView(dtShortCode, "Type='Leave'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string WeekOffcol = (new DataView(dtShortCode, "Type='WeekOff'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();
        string Holidaycol = (new DataView(dtShortCode, "Type='Holiday'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["ColorCOde"].ToString();

        DateTime dtFrom = new DateTime(Convert.ToInt32(txtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue), 1);
        DateTime InitDtFrom = Convert.ToDateTime(dtFrom.ToString());
        int totalDays = DateTime.DaysInMonth(Convert.ToInt32(txtYear.Text), Convert.ToInt32(ddlMonth.SelectedValue.ToString()));
        DateTime dtTo = dtFrom.AddDays(totalDays - 1);
        DateTime dtFromBK = dtFrom;
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_AttendanceRegister_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        try
        {
            adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(dtFrom.ToString()), Convert.ToDateTime(dtTo.ToString()), Session["SelectedEmpId"].ToString(), 10);
        }
        catch
        {

        }
        DataTable dtFilter = rptdata.sp_Att_AttendanceRegister_Report;
        //adp.Fill(rptdata.sp_Att_AttendanceRegister_Report, Convert.ToDateTime(dtFrom.ToString()), Convert.ToDateTime(dtTo.ToString()));

        //DataTable dtFilter = new DataTable();
        //if (Session["SelectedEmpId"].ToString() != "")
        //{
        //    dtFilter = new DataView(rptdata.sp_Att_AttendanceRegister_Report, "Emp_Id in (" + Session["SelectedEmpId"].ToString().Substring(0, Session["SelectedEmpId"].ToString().Length - 1) + ") ", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //else
        //{
        //    rptdata.sp_Att_AttendanceRegister_Report.Rows.Clear();
        //}



        TableRow trHeader = new TableRow();
        TableCell tcReportTitle = new TableCell();
        tcReportTitle.ColumnSpan = 3 ;
        tcReportTitle.Text = "Attendance Register";
        tcReportTitle.Style["Text-align"] = "Left";
        tcReportTitle.Font.Size = 12;
        tcReportTitle.Font.Bold = true;
        TableCell tcReportMonth = new TableCell();
        tcReportMonth.ColumnSpan = totalDays + 1; ;
        tcReportMonth.Text = "Month:" + ddlMonth.SelectedItem.Text;
        tcReportMonth.Style["Text-align"] = "Right";
        tcReportMonth.Font.Size = 12;
        tcReportMonth.Font.Bold = true;
        trHeader.Cells.Add(tcReportTitle);
        trHeader.Cells.Add(tcReportMonth);
        Table1.Rows.Add(trHeader);


        TableRow tr = new TableRow();
        TableRow trWeekDay = new TableRow();
        trWeekDay.Cells.Add(new TableCell());
        trWeekDay.Cells.Add(new TableCell());
        trWeekDay.Cells.Add(new TableCell());
        TableCell tcSNO = new TableCell();
        tcSNO.Text = "SNO";
        tcSNO.Width =30;
        tcSNO.Style["border"] = "dotted";
        tcSNO.Style["border-width"] = "1px";
        tr.Cells.Add(tcSNO);
        TableCell tcId = new TableCell();
        tcId.Wrap = false;
        tcId.Width = 50;
        tcId.Text = "ID";
        tcId.Style["border"] = "dotted";
        tcId.Style["border-width"] = "1px";
        tcId.Style["Text-align"] = "Left";
        tr.Cells.Add(tcId);
        TableCell tcName = new TableCell();
        tcName.Wrap = false;
        tcName.Text = "Name";
        tcName.Style["Text-align"] = "Left";
        tcName.Width = 130;
        tcName.Style["border"] = "dotted";
        tcName.Style["border-width"] = "1px";
        tr.Cells.Add(tcName);
        int count = 1;

        while (count <= totalDays)
        {
            TableCell tcDay = new TableCell();
            TableCell tcWeekDay = new TableCell();
            tcDay.Style["border"] = "dotted";
            tcDay.Style["border-width"] = "1px";
            tcWeekDay.Style["border"] = "dotted";
            tcWeekDay.Style["border-width"] = "1px";
            tcDay.Width =30;
            tcWeekDay.Width = 30;
            tcDay.Style["Text-align"] = "Center";
            tcWeekDay.Style["Text-align"] = "Center";
            tcDay.Text = count.ToString();
            tcWeekDay.Text = dtFrom.AddDays(count - 1).DayOfWeek.ToString().Substring(0, 2).ToUpper();
            tr.Cells.Add(tcDay);
            trWeekDay.Cells.Add(tcWeekDay);
            count++;
        }
        TableCell tcPresent = new TableCell();
        tcPresent.RowSpan = 2;

        tcPresent.Text = "Present/Total";
        tcPresent.Style["border"] = "dotted";
        tcPresent.Style["border-width"] = "1px";
        tr.Cells.Add(tcPresent);

        Table1.Rows.Add(tr);
        Table1.Rows.Add(trWeekDay);

        DataTable dtEmpList = dtFilter.DefaultView.ToTable(true, "Emp_Id");
        int empCounter = 0;
        if (dtEmpList.Rows.Count > 0)
        {

            while (empCounter < dtEmpList.Rows.Count)
            {

                DataTable dtAttbyEmpId = new DataView(dtFilter, "Emp_Id = '" + dtEmpList.Rows[empCounter][0].ToString() + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                DateTime dtFromTemp = InitDtFrom;
                DateTime dtToTemp = dtTo;
                int maxshift = 0;
                while (dtFromTemp < dtToTemp)
                {

                    dtFromTemp = dtFromTemp.AddDays(1);
                    DataTable dtTempDateRecordEmp = new DataView(dtAttbyEmpId, "Att_Date = '" + dtFromTemp.ToString("dd-MMM-yyyy") + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                    if (maxshift < dtTempDateRecordEmp.Rows.Count)
                    {
                        maxshift = dtTempDateRecordEmp.Rows.Count;
                    }
                }

                TableRow[] trEmp = new TableRow[maxshift];
                int[] absent = new int[maxshift];
                int[] present = new int[maxshift];
                int[] early = new int[maxshift];
                int[] late = new int[maxshift];
                int[] total = new int[maxshift];


                try
                {
                    present[0] = 0;
                    early[0] = 0;
                    late[0] = 0;
                    total[0] = 0;
                }
                catch
                {

                }
                int tempcounter = 0;

                while (tempcounter < maxshift)
                {
                    trEmp[tempcounter] = new TableRow();
                    tempcounter++;
                }

                TableCell tcSNOEmp = new TableCell();
                tcSNOEmp.Text = (empCounter + 1).ToString();
                tcSNOEmp.Width = 30;
                tcSNOEmp.Style["border"] = "dotted";
                tcSNOEmp.Style["border-width"] = "1px";
                tcSNOEmp.Style["Text-align"] = "Left";
                if (maxshift > 0)
                {

                    trEmp[0].Cells.Add(tcSNOEmp);



                    TableCell tcNameEmp1 = new TableCell();
                    tcNameEmp1.Text = dtAttbyEmpId.Rows[1]["Emp_Code"].ToString();
                    tcNameEmp1.Width = 50;
                    tcNameEmp1.Style["border"] = "dotted";
                    tcNameEmp1.Style["border-width"] = "1px";
                    tcNameEmp1.Style["Text-align"] = "Left";

                    trEmp[0].Cells.Add(tcNameEmp1);





                    TableCell tcNameEmp = new TableCell();
                    tcNameEmp.Text = dtAttbyEmpId.Rows[0]["Emp_Name"].ToString();
                    tcNameEmp.Width = 130;
                    tcNameEmp.Style["border"] = "dotted";
                    tcNameEmp.Style["border-width"] = "1px";
                    tcNameEmp.Style["Text-align"] = "Left";
                    trEmp[0].Cells.Add(tcNameEmp);




                    dtFrom = dtFromBK;
                }
                int presentcount = 0;
                while (dtFrom <= dtTo)
                {
                    int shiftCounter = 0;
                    DataTable dtTempDateRecordEmp = new DataView(dtAttbyEmpId, "Att_Date = '" + dtFrom.ToString("dd-MMM-yyyy") + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

                    while (shiftCounter < maxshift)
                    {
                        TableCell tcDay = new TableCell();
                        string attEmp = string.Empty;

                        if (shiftCounter < dtTempDateRecordEmp.Rows.Count)
                        {
                            if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Week_Off"].ToString())))
                            {
                                attEmp = attEmp + WeekOff;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(WeekOffcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if ((Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Holiday"].ToString())))
                            {
                                attEmp = attEmp + Holiday;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Holidaycol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Leave"].ToString()))
                            {
                                attEmp = attEmp + Leave;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Leavecol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else if (Convert.ToBoolean(dtTempDateRecordEmp.Rows[shiftCounter]["Is_Absent"].ToString()))
                            {
                                attEmp = attEmp + Absent;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Absentcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                            }
                            else
                            {
                                attEmp = attEmp + Normal;
                                try
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint(Normalcol).ToString());
                                }
                                catch
                                {
                                    tcDay.BackColor = System.Drawing.ColorTranslator.FromHtml(hextoint("ffffff").ToString());
                                }
                                presentcount++;
                            }
                            absent[shiftCounter] = absent[shiftCounter] + 1;
                        }
                        else
                        {
                            attEmp = "-";
                        }


                        tcDay.Text = attEmp;
                        tcDay.Width = 30;
                        tcDay.Style["border"] = "dotted";
                        tcDay.Style["border-width"] = "1px";
                        tcDay.Style["Text-align"] = "Center";
                        trEmp[shiftCounter].Cells.Add(tcDay);
                        trEmp[shiftCounter].Cells.Add(tcDay);

                        if (dtFrom == dtTo)
                        {
                            // Modified By Nitin jain On 23/10/2014
                            TableCell tcabsnt = new TableCell();
                            if (Counter == 0)
                            {
                                tcabsnt.Text = presentcount.ToString() + "/" + totalDays.ToString();
                                tcabsnt.Style["border"] = "dotted";
                                tcabsnt.Style["border-width"] = "1px";
                                tcabsnt.Style["Text-align"] = "Right";
                                Counter = 1;
                            }
                            trEmp[shiftCounter].Cells.Add(tcabsnt);

                        }



                        //

                        shiftCounter = shiftCounter + 1;
                        // Modified By Nitin jain On 23/10/2014
                        if (shiftCounter == maxshift)
                        {
                            Counter = 0;
                        }



                    }


                    dtFrom = dtFrom.AddDays(1);

                }



                for (int maxcounter = 1; maxcounter <= maxshift; maxcounter++)
                {
                    if (maxcounter < maxshift)
                    {
                        trEmp[maxcounter].Cells.AddAt(0, new TableCell());
                        trEmp[maxcounter].Cells.AddAt(0, new TableCell());
                        trEmp[maxcounter].Cells.AddAt(0, new TableCell());

                    }

                    Table1.Rows.Add(trEmp[maxcounter - 1]);
                }




                empCounter++;
            }

        }
        lblmonthname.Text = "Month " + ": " + ddlMonth.SelectedItem.Text;
        return Table1;

    }
    protected void btnExportPdf_Command(object sender, CommandEventArgs e)
    {
        //GridView gv = ((GridView)((ImageButton)sender).FindControl("gvRpt"));

        //if (e.CommandArgument.ToString() == "1")
        //{
       

        //}
        //else if (e.CommandArgument.ToString() == "2")
        //{
        //    btnGenerate_Click(null, null);
        //    btnExportPdf.Visible = false;
        //    btnExportToExcel.Visible = false;
        //    Panel11.OpenInBrowser = true;
        //    Panel11.FileName = "Attendanceregister";
        //    Panel11.ExportType = ControlFreak.ExportPanel.AppType.Excel;
        //}
    }
    protected void lnkback_Click(object sender, EventArgs e)
    {
        pnlReport.Visible = false;
        pnlEmpAtt.Visible = true;
        btnReset_Click(null, null);
        Session["SelectedEmpId"] = null;
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtYear.Text = "";
        ddlMonth.SelectedIndex = 0;

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

        DataTable dtEmp = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dtEmp;
        gvEmployee.DataBind();
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvEmployee.Rows.Count; i++)
        {
            ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectRecord.Text.Split(',').Contains(((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
                {
                    lblSelectRecord.Text += ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectRecord.Text.Split(',');
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
                lblSelectRecord.Text = temp;
            }
        }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment;filename = Attendance_Register " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMyyyy_HHmmss") + ".xls");
        Response.ContentEncoding = Encoding.UTF8;
        StringWriter tw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(tw);
        divexport.RenderControl(hw);
        Response.Write(tw.ToString());
        Response.End();
    }
}