using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Configuration;

public partial class Attendance_RollBackPostedLog : BasePage
{
    Pay_Employee_Month objPayEmpMonth = null;
    EmployeeParameter objempparam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    EmployeeMaster objEmp = null;
    Att_AttendanceLog objAttLog = null;
    Common cmn = null;
    DataAccessClass ObjDa = null;
    Common ObjComman = null;
    IT_ObjectEntry objObjectEntry = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "258", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlDeaprtment();
            FillddlLocation();
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";
            lblSelectRecd.Text = "";
            GetEmplyeeData();

            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            {
                rollbackPostedPayroll.Visible = false;
            }
            else
            {
                rollbackPostedPayroll.Visible = true;
            }
        }
        AllPageCode();
    }
    public void AllPageCode()
    {
        Session["AccordianId"] = "108";
        Session["HeaderText"] = "Attendance Setup";
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
        GetEmplyeeData();

    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmplyeeData();
    }
    public string getRollBackMonthYear(string EmployeeId)
    {
        string EmpStatus = string.Empty;
        // DataTable Dt = objPayEmpMonth.GetAllRecordPostedByCompanyId(Session["CompId"].ToString());
        DataTable Dt = objAttLog.Get_Pay_Employee_AttendanceByCompId(Session["CompId"].ToString());
        Dt = new DataView(Dt, "Emp_Id=" + EmployeeId + "", "", DataViewRowState.CurrentRows).ToTable();
        if (Dt.Rows.Count > 0)
        {
            EmpStatus = "0";
            try
            {
                Dt = new DataView(Dt, "", "Year desc", DataViewRowState.CurrentRows).ToTable();
                string sortYear = Dt.Rows[0]["Year"].ToString();
                Dt = new DataView(Dt, "Year=" + sortYear + "", "Month desc", DataViewRowState.CurrentRows).ToTable();
                ddlMonth.SelectedValue = Dt.Rows[0]["Month"].ToString();
                TxtYear.Text = Dt.Rows[0]["Year"].ToString();
            }
            catch
            {

            }
        }
        return EmpStatus;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    public void FillGrid()
    {
        DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());


        if (ddlLocation.SelectedIndex == 0)
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        try
        {
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
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
            objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";

        ddlLocation.SelectedIndex = 0;
        FillddlDeaprtment();
        GetEmplyeeData();
        Session["dtLeave"] = null;
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";
    }
    public void GetEmplyeeData()
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

        //if (!Common.GetStatus())
        //{
        if (DepIds != "")
        {
            dt = new DataView(dt, "Department_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        //}


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
            Session["dtEmpLeave"] = dt;
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmpLeave, dt, "", "");
            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        }
        else
        {
            Session["dtEmpLeave"] = null;
            gvEmpLeave.DataSource = null;
            gvEmpLeave.DataBind();
        }
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";


    }

    protected void gvEmpLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmpLeave.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpLeave"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvEmpLeave.Rows.Count; i++)
        {
            Label lblconid = (Label)gvEmpLeave.Rows[i].FindControl("lblEmpId");
            string[] split = lblSelectRecd.Text.Split(',');

            for (int j = 0; j < lblSelectRecd.Text.Split(',').Length; j++)
            {
                if (lblSelectRecd.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectRecd.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvEmpLeave.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        Session["dtLeave"] = null;


    }

    protected void btnLeavebind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption1.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption1.SelectedIndex == 1)
            {
                condition = "convert(" + ddlField1.SelectedValue + ",System.String)='" + txtValue1.Text.Trim() + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlField1.SelectedValue + ",System.String) like '%" + txtValue1.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlField1.SelectedValue + ",System.String) Like '" + txtValue1.Text.Trim() + "%'";

            }
            DataTable dtEmp = (DataTable)Session["dtEmpLeave"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            DataTable dtTotalRecord = view.ToTable();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvEmpLeave, view.ToTable(), "", "");

            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dtTotalRecord.Rows.Count.ToString() + "";
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";
        }
        Session["dtLeave"] = null;
        txtValue1.Focus();
    }
    protected void rollbackPostedPayroll_Click(object sender, EventArgs e)
    {
        string url = "../HR/RollBackPayroll.aspx";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }
    protected void BtnRollBackPayroll_Click(object sender, EventArgs e)
    {
        if (lblSelectRecd.Text == "")
        {
            DisplayMessage("Select Employee");
            return;
        }
        if (ddlMonth.SelectedIndex == 0 || TxtYear.Text == "")
        {
            DisplayMessage("You have Not Posted Month and Year for Rollback");
            return;
        }

        DataTable dtPay = objPayEmpMonth.GetAllRecordPostedEmpMonth(lblSelectRecd.Text, ddlMonth.SelectedValue, TxtYear.Text, HttpContext.Current.Session["CompId"].ToString());
        if (dtPay.Rows.Count > 0)
        {
            DisplayMessage("PayRoll Already Posted so Rollback Payroll then Rollback LogPost.");
            return;
        }

        int b = objAttLog.RollBackTransaction(Session["CompId"].ToString(), lblSelectRecd.Text, ddlMonth.SelectedValue, TxtYear.Text);
        if (b > 0 || b == 0)
        {
            DisplayMessage("LogPost is RollBack successsfully");



            //heere we are adding code for add leave balance having is rule true 
            DataTable dtLeaveSummary = ObjDa.return_DataTable("select Att_Employee_Leave_Trans.Trans_Id,Att_Employee_Leave_Trans.Remaining_Days,Att_Employee_Leave_Trans.Assign_Days from Att_Employee_Leave_Trans inner join Set_Att_Employee_Leave on  Att_Employee_Leave_Trans.emp_id= Set_Att_Employee_Leave.Emp_Id and Att_Employee_Leave_Trans.Leave_Type_Id =Set_Att_Employee_Leave.LeaveType_Id  where Att_Employee_Leave_Trans.Emp_Id='" + lblSelectRecd.Text + "' and Set_Att_Employee_Leave.Field4='True' and Att_Employee_Leave_Trans.Year='" + TxtYear.Text + "' and Att_Employee_Leave_Trans.Month='0'");

            foreach (DataRow dr in dtLeaveSummary.Rows)
            {


                double BalanceLeave = Convert.ToDouble(dr["Remaining_Days"].ToString());

                BalanceLeave = BalanceLeave - (Convert.ToDouble(dr["Assign_Days"].ToString()) / 12);


                ObjDa.execute_Command("update Att_Employee_Leave_Trans set Remaining_Days='" + BalanceLeave.ToString() + "' where Trans_id=" + dr["Trans_Id"].ToString() + "");



            }

        }
        else
        {

        }
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";
        GetEmplyeeData();
        lblSelectRecd.Text = "";
    }
    protected void btnLeaveRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        GetEmplyeeData();
        Session["dtLeave"] = null;
        ddlMonth.SelectedIndex = 0;
        TxtYear.Text = "";
    }
    protected void chkgvSelect_CheckedChangedLeave(object sender, EventArgs e)
    {
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvEmpLeave.Rows[index].FindControl("lblEmpId");
        foreach (GridViewRow gvRow in gvEmpLeave.Rows)
        {
            Label lblEmpId = (Label)gvRow.FindControl("lblEmpId");
            CheckBox chk = (CheckBox)gvRow.FindControl("chkgvSelect");

            if (lblEmpId.Text != lb.Text)
            {
                chk.Checked = false;
            }
        }

        if (getRollBackMonthYear(lb.Text) == "")
        {
            DisplayMessage("Employee Has no Posted Log");
            ((CheckBox)gvEmpLeave.Rows[index].FindControl("chkgvSelect")).Checked = false;
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";

            return;
        }

        if (((CheckBox)gvEmpLeave.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            //add employee id
            lblSelectRecd.Text = lb.Text;

        }

        else
        {
            lblSelectRecd.Text = "";
            ddlMonth.SelectedIndex = 0;
            TxtYear.Text = "";

        }
        Session["dtLeave"] = null;

    }
}
