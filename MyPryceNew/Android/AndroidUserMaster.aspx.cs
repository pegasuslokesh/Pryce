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

public partial class Android_AndroidUserMaster : BasePage
{
    #region defind Class Object
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    And_EmpParameter objAndParam = null;
    And_DeviceMaster objAndDevice = null;
    Common cmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAndParam = new And_EmpParameter(Session["DBConnection"].ToString());
        objAndDevice = new And_DeviceMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();

        

        if (!IsPostBack)
        {
           
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Android/AndroidUserMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillGrid();
            fillddlDevice();
        }
        Session["AccordianId"] = "26";
        //AllPageCode();
    }

    private void fillddlDevice()
    {
        DataTable dtDevice = objAndDevice.GetAndroidDeviceMaster(Session["CompId"].ToString());
        dtDevice = new DataView(dtDevice, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (Session["EmpId"].ToString() == "0")
        {
        }
        else
        {
            dtDevice = new DataView(dtDevice, "Device_Id='" + Session["Device_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (dtDevice.Rows.Count > 0)
        {
            ddlDevice.DataSource = null;
            ddlDevice.DataBind();
            ddlDevice.DataSource = dtDevice;
            ddlDevice.DataTextField = "Device_Name";
            ddlDevice.DataValueField = "Device_Id";
            ddlDevice.DataBind();
            ListItem li = new ListItem("--Select--", "0");
            ddlDevice.Items.Insert(0, li);
            ddlDevice.SelectedIndex = 0;

        }
        else
        {
            try
            {
                ddlDevice.Items.Clear();
                ddlDevice.DataSource = null;
                ddlDevice.DataBind();
                ListItem li = new ListItem("--Select--", "0");
                ddlDevice.Items.Insert(0, li);
                ddlDevice.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem("--Select--", "0");
                ddlDevice.Items.Insert(0, li);
                ddlDevice.SelectedIndex = 0;

            }
        }
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSaveUser.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();

    }
    #endregion

    #region System defind Funcation
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlField.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        FillGrid();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        And_DeviceMaster objDev = new And_DeviceMaster(Session["DBConnection"].ToString());
        if (ddlField.SelectedValue == "Device_Id")
        {
            string EmpIds = string.Empty;
            if (txtValue1.Text != "")
            {
                DataTable dt = objDev.GetEmployeeDeviceMasterById(Session["CompId"].ToString(), txtValue1.Text);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        EmpIds += dt.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }


                if (EmpIds != "")
                {
                    DataTable dtEmp = (DataTable)Session["dtEmp"];
                    dtEmp = new DataView(dtEmp, "Emp_Id in (" + EmpIds + ")", "", DataViewRowState.CurrentRows).ToTable();
                    Session["dtEmp"] = dtEmp;
                }
                else
                {
                    DataTable dtEmp = new DataTable();
                    Session["dtEmp"] = dtEmp;
                }
            }

            DataTable dtEmp1 = (DataTable)Session["dtEmp"];

            gvEmployee.DataSource = dtEmp1;
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp1.Rows.Count.ToString() + "";
        }
        else if (ddlOption1.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption1.SelectedIndex == 1)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String)='" + txtValue1.Text.Trim() + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) like '%" + txtValue1.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlField.SelectedValue + ",System.String) Like '" + txtValue1.Text.Trim() + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmp"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            gvEmployee.DataSource = view.ToTable();
            gvEmployee.DataBind();
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        //AllPageCode();
        txtValue1.Focus();
    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmp"];
        gvEmployee.DataSource = dtEmp;
        gvEmployee.DataBind();
        //AllPageCode();
    }
    protected void btnClosePanel_Click1(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
    }

    protected void ddlDevice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDevice.SelectedValue == "0")
        {
            ddlDevice.Focus();
            DisplayMessage("Select Device From List");
            return;
        }
        else
        {
            DataTable dtEmp = objAndDevice.GetAndroidDeviceEmployee(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            dtEmp = new DataView(dtEmp, "Device_Id='" + ddlDevice.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                gvEmployee.DataSource = dtEmp;
                gvEmployee.DataBind();
                Session["dtEmp"] = dtEmp;
                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString();
            }
            else
            {
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();
                Session["dtEmp"] = null;
                lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + "0";
            }
        }
        //AllPageCode();
    }
    protected void btnUpdatePop_Click(object sender, EventArgs e)
    {
        DataTable dt = objAndParam.GetAndroidParameterById(Session["CompId"].ToString(), hdnEmpId.Value);
        if (txtUserName.Text == "")
        {
            txtUserName.Focus();
            DisplayMessage("Type UserName");
            return;
        }
        if (txtPassword.Text == "")
        {
            txtPassword.Focus();
            DisplayMessage("Type Password");
            return;
        }
        if (dt.Rows.Count > 0)
        {
            objAndParam.UpdateAndroidEmpParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, txtPassword.Text, txtCardNo.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Updated", "green");
        }
        else
        {
            objAndParam.InsertAndroidEmpParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, txtPassword.Text, txtCardNo.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Saved", "green");
        }
    }
    protected void btnCancelPop_Click(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string empid = e.CommandArgument.ToString();
        string empname = GetEmployeeName(empid);
        lblEmpName1.Text = empname;
        lblEmpCode1.Text = GetEmployeeCode(empid);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);

        txtUserName.Text = GetEmployeeCode(empid);

        hdnEmpId.Value = empid.ToString();
        DataTable dt = objAndParam.GetAndroidParameterById(Session["CompId"].ToString(), empid);

        if (dt.Rows.Count > 0)
        {
            txtPassword.Text = dt.Rows[0]["Password"].ToString().Trim();
            txtCardNo.Text = dt.Rows[0]["CardNo"].ToString().Trim();
        }
        txtPassword.Focus();
    }

    #endregion

    #region User defind Funcation
    public void FillGrid()
    {

        DataTable dtEmp = objEmp.GetEmployeeDeviceMaster(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }

        if (dtEmp.Rows.Count > 0)
        {
            gvEmployee.DataSource = dtEmp;
            gvEmployee.DataBind();
            Session["dtEmp"] = dtEmp;
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString();

        }

        //AllPageCode();
    }
    public string GetEmployeeName(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Name"].ToString();

            if (empname == "")
            {
                empname = "No Name";
            }

        }
        else
        {
            empname = "No Name";

        }

        return empname;



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

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    #endregion





    protected void btnSaveUser_Click(object sender, EventArgs e)
    {


    }




    protected void btnSaveUser_Click1(object sender, EventArgs e)
    {
        DataTable dt = objAndParam.GetAndroidParameterById(Session["CompId"].ToString(), hdnEmpId.Value);
        if (txtUserName.Text == "")
        {
            txtUserName.Focus();
            DisplayMessage("Type UserName");
            return;
        }
        if (txtPassword.Text == "")
        {
            txtPassword.Focus();
            DisplayMessage("Type Password");
            return;
        }
        if (dt.Rows.Count > 0)
        {
            objAndParam.UpdateAndroidEmpParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, txtPassword.Text.Trim(), txtCardNo.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Updated", "green");
        }
        else
        {
            objAndParam.InsertAndroidEmpParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, txtPassword.Text.Trim(), txtCardNo.Text.Trim(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Saved", "green");
        }
    }
}
