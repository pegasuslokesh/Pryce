using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

public partial class MasterSetUp_UserMaster : BasePage
{
    Set_Approval_Employee objApprovalEmp = null;
    Set_Location_Department objLocDept = null;
    And_DeviceMaster objAndDevice = null;
    Common objCmn = null;
    UserMaster objUser = null;
    And_Device_Group objDeviceGroup = null;
    UserPermission objUserPermission = null;
    IT_Application_Customer objITCust = null;
    SystemParameter objSysParam = null;
    And_DeviceMaster objDevice = null;
    ModuleMaster objModule = null;
    ObjectMaster ObjItObject = null;
    RoleMaster objRole = null;
    RolePermission objRolePermission = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    IT_ObjectEntry objObjectEntry = null;
    And_EmpParameter objAndParam = null;
    IT_App_Op_Permission obj_OP_Permission = null;
    Set_UserReminder ObjUserReminder = null;
    Set_ApplicationParameter objAppParam = null;
    DataAccessClass objDa = null;
    ObjectMaster objObject = null;
    CompanyMaster ObjComp = null;
    RoleDataPermission objRoleDataPerm = null;
    UserDataPermission objUserDataPerm = null;
    BrandMaster objBrand = null;
    LocationMaster objLocation = null;
    PageControlCommon objPageCmn = null;
    string CompanyId = string.Empty;
    string CompanyIds = string.Empty;
    string BrandChecked = string.Empty;
    string LocationChecked = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objApprovalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objAndDevice = new And_DeviceMaster(Session["DBConnection"].ToString());
        objCmn = new Common(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objDeviceGroup = new And_Device_Group(Session["DBConnection"].ToString());
        objUserPermission = new UserPermission(Session["DBConnection"].ToString());
        objITCust = new IT_Application_Customer(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDevice = new And_DeviceMaster(Session["DBConnection"].ToString());
        objModule = new ModuleMaster(Session["DBConnection"].ToString());
        ObjItObject = new ObjectMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRolePermission = new RolePermission(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objAndParam = new And_EmpParameter(Session["DBConnection"].ToString());
        obj_OP_Permission = new IT_App_Op_Permission(Session["DBConnection"].ToString());
        ObjUserReminder = new Set_UserReminder(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objObject = new ObjectMaster(Session["DBConnection"].ToString());
        ObjComp = new CompanyMaster(Session["DBConnection"].ToString());
        objRoleDataPerm = new RoleDataPermission(Session["DBConnection"].ToString());
        objUserDataPerm = new UserDataPermission(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = objCmn.getPagePermission("../MasterSetUp/UserMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);



            navTree.Attributes.Add("onclick", "OnTreeClick(event)");
            TreeViewDepartment.Attributes.Add("onclick", "OnTreeClick(event)");
            txtValue.Focus();
            Div_Rol_Permission.Style.Add("display", "none");
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillddlRole();
            FillAndroidDevice();
            Session["CHECKED_ITEMS"] = null;
            FillGrid();
            //FillGridBin();
            string userid = Session["UserId"].ToString().Trim();
            userid = userid.ToLower();
            string str = string.Empty;
            for (int i = 0; i < Session.Contents.Count; i++)
            {
                str = str + "<br>" + Session.Keys[i].ToString();
                //Response.Write(Session.Keys[i] + " - " + Session[i] + "<br />");
            }
            if (Session["UserId"].ToString().ToUpper().Trim() == "SUPERADMIN")
            {
                trSuperUser.Visible = true;
                rbtnUser.Checked = true;
            }
            else
            {
                trSuperUser.Visible = false;
            }
            //}
            FillModule();
            Fillopeartion();
            FillCompany();
            fillPermissionList();
            CloudSetup();
            // BindTreeChanged();
        }
        txtPassword.Attributes.Add("Value", txtPassword.Text);
        //txtEmailPassword.Attributes.Add("Value", txtEmailPassword.Text);
        Page.Title = objSys.GetSysTitle();
    }

    public void CloudSetup()
    {
        if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
        {
            //lblAndroidDevice.Visible = false;
            //ddlAndroidDevice.Visible = false;
            chkIsGlobalAccess.Checked = true;
            chkIsGlobalAccess.Visible = false;
            lblLanguage.Visible = false;
            ddlLanguage.Visible = false;
        }
    }

    public void FillModule()
    {
        DataTable dt = objModule.GetModuleMaster();
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlSearchModuleName, dt, "Module_Name", "Module_Id");
        }
        else
        {
            ddlSearchModuleName.Items.Clear();
        }
        dt.Dispose();
    }
    public void FillAndroidDevice()
    {
        DataTable dt = new DataTable();
        dt = objAndDevice.GetAndroidDeviceMaster(Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            ddlAndroidDevice.DataSource = null;
            ddlAndroidDevice.DataBind();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)ddlAndroidDevice, dt, "Device_Name", "Device_Id");
        }
        else
        {
            try
            {
                ddlAndroidDevice.Items.Clear();
                ddlAndroidDevice.DataSource = null;
                ddlAndroidDevice.DataBind();
                ddlAndroidDevice.Items.Insert(0, "--Select--");
                ddlAndroidDevice.SelectedIndex = 0;
            }
            catch
            {
                ddlAndroidDevice.Items.Insert(0, "--Select--");
                ddlAndroidDevice.SelectedIndex = 0;
            }
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
        {
            chkIsGlobalAccess.Checked = true;
            chkIsGlobalAccess.Visible = false;
        }
    }
    public void FillddlRole()
    {
        DataTable dt = new DataTable();
        dt = objRole.GetRoleMaster(Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            ddlRole.DataSource = null;
            ddlRole.DataBind();
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)ddlRole, dt, "Role_Name", "Role_Id");
        }
        else
        {
            try
            {
                ddlRole.Items.Clear();
                ddlRole.DataSource = null;
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, "--Select--");
                ddlRole.SelectedIndex = 0;
            }
            catch
            {
                ddlRole.Items.Insert(0, "--Select--");
                ddlRole.SelectedIndex = 0;
            }
        }
    }
    private void UnSelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = false;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = false;
            UnSelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        navTree.DataBind();
    }
    private void SelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = true;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = true;
            SelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        try
        {
            treeNode.Parent.Checked = true;
            treeNode.Parent.Parent.Checked = true;
            treeNode.Parent.Parent.Parent.Checked = true;
            treeNode.Parent.Parent.Parent.Parent.Checked = true;
        }
        catch { }
        navTree.DataBind();
    }
    protected void ChkSelectAll_OnCheckedChanged(object sender, EventArgs e)
    {
        BindTree();
        if (chkSelectAll.Checked)
        {
            foreach (TreeNode Tn in navTree.Nodes)
            {
                SelectChild(Tn);
            }
        }
        else
        {
            foreach (TreeNode Tn in navTree.Nodes)
            {
                UnSelectChild(Tn);
            }
        }
        //GetRoleInformation();
    }
    protected void chkEditRoleCheckedChanged(object sender, EventArgs e)
    {
        if (chkEditRole.Checked)
        {
            //navTree.Enabled = true;
            //chkSelectAll.Visible = true;
            chkSelectAll.Checked = true;
        }
        else
        {
            //navTree.Enabled = false;
            //chkSelectAll.Visible = false;
            chkSelectAll.Checked = true;
        }
        BindTree();
    }
    protected void rbtnUserCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnSuperAdmin.Checked)
        {
            trEmp.Visible = false;
            Div_Rol_Permission.Style.Add("display", "none");
        }
        else if (rbtnUser.Checked)
        {
            trEmp.Visible = true;
        }
    }
    protected void ddlRole_SelectIndexChanged(object sender, EventArgs e)
    {
        if (ddlRole.SelectedIndex != 0)
        {
            Div_Rol_Permission.Style.Add("display", "");
            hdnRoleId.Value = ddlRole.SelectedValue;
            if (chkEditRole.Checked)
            {
                chkEditRoleCheckedChanged(null, null);
            }
            else
            {
                chkEditRoleCheckedChanged(null, null);
            }
            //  BindTreeChanged();
        }
        else
        {
            Div_Rol_Permission.Style.Add("display", "none");
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ViewState["Device_Id"] = "";
        FillGrid();
        //FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["User"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_User_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvUserMaster, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinUser"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvUserMasterBin, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        //FillGrid();
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        string index = "-1";
        foreach (GridViewRow gvrow in gvUserMasterBin.Rows)
        {
            index = (string)gvUserMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvUserMasterBin.Rows)
            {
                string index = (string)gvUserMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void gvUserMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvUserMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvUserMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvUserMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objUser.GetUserMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvUserMasterBin, dt, "", "");
        //AllPageCode();
    }
    public bool CheckEmailId1(string EmailAddress)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(EmailAddress,
                      "\\w+([-+.']\\+)*@\\w+([-.]\\+)*\\.\\w+([-.]\\+)*");
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        if (!CheckValidation(chkCompany))
        {
            DisplayMessage("Select company");
            chkCompany.Focus();
            return;
        }
        if (!CheckValidation(chkBrand))
        {
            DisplayMessage("Select at least one brand");
            chkCompany.Focus();
            return;
        }
        if (!CheckValidation(chkLocation))
        {
            DisplayMessage("Select at least one location");
            chkCompany.Focus();
            return;
        }
        int b = 0;
        string strAndroidDevice = "0";
        string optype = string.Empty;
        string RoleName = string.Empty;
        string strRoleId = string.Empty;
        if (txtUserName.Text == "")
        {
            DisplayMessage("Enter User Name");
            txtUserName.Focus();
            return;
        }
        if (txtPassword.Text == "")
        {
            DisplayMessage("Enter Password");
            txtPassword.Focus();
            return;
        }
        DataTable dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            if (dtUser.Rows[0]["Role_Name"].ToString() == "Super Admin")
            {
                optype = "4";
            }
            else
            {
                optype = "2";
            }
        }
        string empid = txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1];
        string strEmpCode = empid;
        //string empid = txtEmp.Text.Split('/')[0].ToString();
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        // dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        if (dtEmp.Rows.Count > 0)
        {
            empid = dtEmp.Rows[0]["Emp_Id"].ToString();
        }
        else
        {
            empid = "0";
        }

        //if (objDa.return_DataTable("select * from set_userdetail where User_Id='" + strEmpCode + "'").Rows.Count == 0 && IssalesPerson(empid))
        //{
        //    DisplayMessage("Please configure email account for user");
        //    return;
        //}




        if (editid.Value == "")
        {
            DataTable dt1 = objUser.GetUserMaster(HttpContext.Current.Session["CompId"].ToString());
            dt1 = new DataView(dt1, "User_Id='" + txtUserName.Text + "' and Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("User Id Already Exists");
                txtUserName.Focus();
                return;
            }
            // Modified By Nitin Jain On 20/11/2014 , Restrict To Assign Same Device Id To Multiple  User // Insert
            if (ddlAndroidDevice.SelectedValue != "--Select--")
            {
                strAndroidDevice = ddlAndroidDevice.SelectedValue;
                DataTable dtAllUser = objUser.GetUserMaster(HttpContext.Current.Session["CompId"].ToString());
                dt1 = new DataView(dtAllUser, "Field1='" + strAndroidDevice + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0 && ViewState["Device_Id"].ToString() != strAndroidDevice)
                {
                    DisplayMessage("Device Already Assign");
                    ddlAndroidDevice.Focus();
                    ddlAndroidDevice.SelectedValue = "--Select--";
                    return;
                }
            }
            else
            {
                strAndroidDevice = "0";
            }


            // if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "Web")
            // {

            try
            {
                MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
                MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Session["RegistrationCode"].ToString(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
                int attUserCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from set_usermaster where company_id = '" + Session["CompId"].ToString() + "'").Rows[0][0].ToString());
                if ((attUserCount + 1) > Convert.ToInt32(clsMasterCmp.user.ToString()))
                {
                    DisplayMessage("Please update your license");
                    // UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
                    UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), "0", clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                    //DisplayMessage("Modal_UpdateLicense_Open()");
                    return;
                }
            }
            catch (Exception ex)
            { 
            
            }
            
           //}



            if (rbtnSuperAdmin.Checked)
            {
                DataTable dtRole = objRole.GetRoleMasterByRoleName("0", "Super Admin");
                string roleid = string.Empty;
                if (dtRole.Rows.Count > 0)
                {
                    roleid = dtRole.Rows[0]["Role_Id"].ToString();
                }
                else
                {
                    roleid = "0";

                }
                b = objUser.InsertUserMaster("0", txtUserName.Text, Common.Encrypt(txtPassword.Text.Trim()), "0", roleid, false.ToString(), strAndroidDevice, "", "", ddlLanguage.SelectedValue, "", chkIsStandAloneUser.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkIsGlobalAccess.Checked.ToString(),chkEmailConfig.Checked.ToString());
                txtPassword.Attributes.Add("Value", txtPassword.Text);
                //txtEmailPassword.Attributes.Add("Value", txtEmailPassword.Text);
            }
            else
            {
                if (dtEmp.Rows.Count == 0)
                {
                    DisplayMessage("Employee does not exists");
                    return;
                }
                //if (ddlRole.SelectedIndex == 0)
                //{
                //    DisplayMessage("Select Role Name");
                //    ddlRole.Focus();
                //    return;
                //}
                DataTable dt2 = objUser.GetUserMaster(HttpContext.Current.Session["CompId"].ToString());
                dt2 = new DataView(dt1, "User_Id='" + txtUserName.Text + "' and Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt2.Rows.Count > 0)
                {
                    DisplayMessage("User Id Already Exists");
                    txtUserName.Focus();
                    return;
                }
                b = objUser.InsertUserMaster(Session["CompId"].ToString(), txtUserName.Text, Common.Encrypt(txtPassword.Text.Trim()), empid, GetRoleId(), chkEditRole.Checked.ToString(), strAndroidDevice, "", "", ddlLanguage.SelectedValue, "", chkIsStandAloneUser.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkIsGlobalAccess.Checked.ToString(),chkEmailConfig.Checked.ToString());
                txtPassword.Attributes.Add("Value", txtPassword.Text);
                //txtEmailPassword.Attributes.Add("Value", txtEmailPassword.Text);
            }
            if (b != 0)
            {
                int ReminderDays = 0;
                ReminderDays = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Password Reminder(In Days)", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                ObjUserReminder.InsertRecord(txtUserName.Text, DateTime.Now.AddDays(ReminderDays).ToString(), "0", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                DisplayMessage("Record Saved", "green");
                SaveEmployeeToAndroidDevice();
                if (rbtnSuperAdmin.Checked)
                {
                    //FillGrid();
                    Reset();
                    FillGrid();
                }
                else
                {
                    //FillGrid();
                    editid.Value = txtUserName.Text;
                    SaveCompBrandLocDept(editid.Value);
                    if (chkEditRole.Checked)
                    {
                        btnSavePerm_Click(null, null);
                    }
                    Reset();
                    FillGrid();
                }
            }
            else
            {
                DisplayMessage("Employeee already Exists");
            }
        }
        else
        {
            DataTable Dt_Pending_Approval = objApprovalEmp.GetApprovalTransation(Session["CompId"].ToString(),empid);
            if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
            {
               // Dt_Pending_Approval = new DataView(Dt_Pending_Approval, "Emp_Id='" + empid + "' and Status='Pending' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
                {
                    string Return_Val = Check_Comp_Brand_Loc_Dept(Dt_Pending_Approval);
                    if (Return_Val != "")
                    {
                        DisplayMessage(Return_Val);
                        return;
                    }
                }
            }
            // DataTable dt1 = objUser.GetUserMasterByUserIdByCompId(Session["UserId"].ToString(), optype);
            DataTable dt1 = objUser.GetUserMasterByUserIdByCompId(txtUserName.Text, optype, HttpContext.Current.Session["CompId"].ToString());
            DataTable dv = new DataView(dt1, "User_Id='" + txtUserName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dv.Rows.Count > 0 && dv.Rows[0]["User_Id"].ToString() != editid.Value)
            {
                DisplayMessage("User Id Already Exists");
                txtUserName.Focus();
                return;
            }
            // Modified By Nitin Jain On 20/11/2014 , Restrict To Assign Same Device Id To Multiple  User
            if (ddlAndroidDevice.SelectedValue != "--Select--")
            {
                strAndroidDevice = ddlAndroidDevice.SelectedValue;
                DataTable dtAllUser = objUser.GetUserMaster(HttpContext.Current.Session["CompId"].ToString());
                DataTable dtTemp = dtAllUser.Copy();
                dt1 = new DataView(dtAllUser, " Emp_Id=" + empid + " AND Field1='" + strAndroidDevice + "'", "", DataViewRowState.CurrentRows).ToTable();
                // DataTable DtEmp = new DataView(dtAllUser, "Emp_Id='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count == 0)
                {
                    dt1 = new DataView(dtAllUser, " Emp_Id<>" + empid + " AND Field1='" + strAndroidDevice + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        DisplayMessage("Device Already Assign");
                        ddlAndroidDevice.Focus();
                        ddlAndroidDevice.SelectedValue = "--Select--";
                        return;
                    }
                }
                else
                {
                }
                //if (DtEmp.Rows[0]["Field1"].ToString() == "0" && dt1.Rows[0]["Field1"].ToString() != ddlAndroidDevice.SelectedValue)
                //{
                //    DisplayMessage("Device Already Assign");
                //    ddlAndroidDevice.Focus();
                //    ddlAndroidDevice.SelectedValue = "0";
                //    return;
                //}
            }
            else
            {
                strAndroidDevice = "0";
            }
            if (rbtnSuperAdmin.Checked)
            {
                DataTable dtRole = objRole.GetRoleMasterByRoleName("0", "Super Admin");
                string roleid = string.Empty;
                if (dtRole.Rows.Count > 0)
                {
                    roleid = dtRole.Rows[0]["Role_Id"].ToString();
                }
                else
                {
                    roleid = "0";
                }
                b = objUser.UpdateUserMaster(editid.Value, "0", txtUserName.Text, Common.Encrypt(txtPassword.Text.Trim()), empid, roleid, false.ToString(), strAndroidDevice, "", "", ddlLanguage.SelectedValue, "", chkIsStandAloneUser.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkIsGlobalAccess.Checked.ToString(),chkEmailConfig.Checked.ToString());
            }
            else
            {
                //if (ddlRole.SelectedIndex == 0)
                //{
                //    DisplayMessage("Select Role Name");
                //    ddlRole.Focus();
                //    return;
                //}
                b = objUser.UpdateUserMaster(editid.Value, Session["CompId"].ToString(), txtUserName.Text, Common.Encrypt(txtPassword.Text.Trim()), empid, GetRoleId(), chkEditRole.Checked.ToString(), strAndroidDevice, "", "", ddlLanguage.SelectedValue, "", chkIsStandAloneUser.Checked.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkIsGlobalAccess.Checked.ToString(),chkEmailConfig.Checked.ToString());
            }
            if (b != 0)
            {
                if (chkEditRole.Checked == false)
                {
                    objUserPermission.DeleteUserPermission(Session["CompId"].ToString(), txtUserName.Text);
                }
                string UserName = txtUserName.Text;
                bool EditRole = chkEditRole.Checked;
                string RoleId = ddlRole.SelectedValue;
                // Nitin Jain
                SaveEmployeeToAndroidDevice();
                DisplayMessage("Record Updated", "green");
                Lbl_New_tab.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                if (rbtnSuperAdmin.Checked)
                {
                    txtPassword.Attributes.Add("Value", txtPassword.Text);
                    //txtEmailPassword.Attributes.Add("Value", txtEmailPassword.Text);
                    //FillGrid();
                    Reset();
                    FillGrid();
                }
                else
                {
                    //FillGrid();
                    txtPassword.Attributes.Add("Value", txtPassword.Text);
                    //txtEmailPassword.Attributes.Add("Value", txtEmailPassword.Text);
                    objUserDataPerm.DeleteUserDataPermission(Session["CompId"].ToString(), editid.Value);
                    SaveCompBrandLocDept(editid.Value);
                    if (chkEditRole.Checked)
                    {
                        btnSavePerm_Click(null, null);
                    }
                    Reset();
                    FillGrid();
                }
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
        //AllPageCode();
    }
    public string Check_Comp_Brand_Loc_Dept(DataTable Dt_Pending_Approval)
    {
        string Return_Value = "";
        if (Session["UserId"].ToString() != "0" && Session["UserId"].ToString() != "superadmin")
        {
            string Comp_ID = "Not_Selected";
            string Brand_ID = "Not_Selected";
            string Loc_ID = "Not_Selected";
            string Dep_ID = "Not_Selected";
            if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
            {
                DataTable Dt_Company = Dt_Pending_Approval.DefaultView.ToTable(true, "Company_Id");
                DataTable Dt_Brand = Dt_Pending_Approval.DefaultView.ToTable(true, "Brand_Id");
                DataTable Dt_Location = Dt_Pending_Approval.DefaultView.ToTable(true, "Location_Id");
                DataTable Dt_Department = Dt_Pending_Approval.DefaultView.ToTable(true, "Department_Id");
                if (Dt_Company != null && Dt_Company.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Cmp in Dt_Company.Rows)
                    {
                        if (Dr_Cmp["Company_Id"].ToString() != "0")
                        {
                            foreach (ListItem item in chkCompany.Items)
                            {
                                if (item.Selected)
                                {
                                    if (item.Value.ToString() == Dr_Cmp["Company_Id"].ToString())
                                    {
                                        Comp_ID = "Is_Selected";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Dt_Brand != null && Dt_Brand.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Brand in Dt_Brand.Rows)
                    {
                        if (Dr_Brand["Brand_Id"].ToString() != "0")
                        {
                            foreach (ListItem item in chkBrand.Items)
                            {
                                if (item.Selected)
                                {
                                    if (item.Value.ToString() == Dr_Brand["Brand_Id"].ToString())
                                    {
                                        Brand_ID = "Is_Selected";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Dt_Location != null && Dt_Location.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Location in Dt_Location.Rows)
                    {
                        if (Dr_Location["Location_Id"].ToString() != "0")
                        {
                            foreach (ListItem item in chkLocation.Items)
                            {
                                if (item.Selected)
                                {
                                    if (item.Value.ToString() == Dr_Location["Location_Id"].ToString())
                                    {
                                        Loc_ID = "Is_Selected";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (Dt_Department != null && Dt_Department.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Department in Dt_Department.Rows)
                    {
                        if (Dr_Department["Department_Id"].ToString() != "0")
                        {
                            foreach (TreeNode tn in TreeViewDepartment.Nodes)
                            {
                                foreach (TreeNode tnchildnode in tn.ChildNodes)
                                {
                                    if (tnchildnode.Checked)
                                    {
                                        if (tnchildnode.Value.ToString() == Dr_Department["Department_Id"].ToString())
                                        {
                                            Dep_ID = "Is_Selected";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Dep_ID != "Is_Selected")
            {
                Return_Value = "Request is in under processing , You cannot edit this Department";
            }
            else if (Loc_ID != "Is_Selected")
            {
                Return_Value = "Request is in under processing , You cannot edit this Location";
            }
            else if (Brand_ID != "Is_Selected")
            {
                Return_Value = "Request is in under processing , You cannot edit this Brand";
            }
            else if (Comp_ID != "Is_Selected")
            {
                Return_Value = "Request is in under processing , You cannot edit this Company";
            }
        }
        return Return_Value;
    }
    public void SaveCompBrandLocDept(string RoleId)
    {
        foreach (ListItem item in chkCompany.Items)
        {
            if (item.Selected)
            {
                objUserDataPerm.InsertUserDataPermission(RoleId, Session["CompId"].ToString(), "C", item.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        foreach (ListItem item in chkBrand.Items)
        {
            if (item.Selected)
            {
                objUserDataPerm.InsertUserDataPermission(RoleId, Session["CompId"].ToString(), "B", item.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        foreach (ListItem item in chkLocation.Items)
        {
            if (item.Selected)
            {
                objUserDataPerm.InsertUserDataPermission(RoleId, Session["CompId"].ToString(), "L", item.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        foreach (TreeNode tn in TreeViewDepartment.Nodes)
        {
            foreach (TreeNode tnchildnode in tn.ChildNodes)
            {
                if (tnchildnode.Checked)
                {
                    objUserDataPerm.InsertUserDataPermission(RoleId, Session["CompId"].ToString(), "D", tnchildnode.Value, tn.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }
    }
    public bool CheckValidation(CheckBoxList chkList)
    {
        bool Result = false;
        foreach (ListItem li in chkList.Items)
        {
            if (li.Selected)
            {
                Result = true;
                break;
            }
        }
        return Result;
    }
    public string GetRoleId()
    {
        string strRoleId = string.Empty;
        foreach (TreeNode RoleNode in navTree.Nodes)
        {
            if (strRoleId == "")
            {
                strRoleId = RoleNode.Value;
            }
            else
            {
                strRoleId = strRoleId + "," + RoleNode.Value;
            }
        }
        if (strRoleId == "")
        {
            strRoleId = "0";
        }
        return strRoleId;
    }
    private void SaveEmployeeToAndroidDevice()
    {
        if (ddlAndroidDevice.SelectedValue != "--Select--")
        {
            ViewState["Device_Id"] = ddlAndroidDevice.SelectedValue;
            // Updated By Nitin jain On 20/11/2014 To Insert Into And_Device_Employee For ANdroid TImeMan Use
            DataTable dtPermitedUser = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            // Filter On Brand and Location Bases...
            dtPermitedUser = new DataView(dtPermitedUser, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Filter On Bases Of Department........
            if (Session["SessionDepId"] != null)
            {
                dtPermitedUser = new DataView(dtPermitedUser, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            objAndDevice.DeleteEmployeeDeviceMaster(Session["CompId"].ToString(), ddlAndroidDevice.SelectedValue);
            for (int i = 0; i < dtPermitedUser.Rows.Count; i++)
            {
                int b = objDevice.InsertAndroidDeviceEmp(Session["CompId"].ToString(), ddlAndroidDevice.SelectedValue, Session["BrandId"].ToString(), ddlLocation.SelectedValue, dtPermitedUser.Rows[i]["Emp_Id"].ToString(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                string Passwd = dtPermitedUser.Rows[i]["Emp_Code"].ToString();
                b = objAndParam.InsertAndroidEmpParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, dtPermitedUser.Rows[i]["Emp_Id"].ToString(), Passwd, "", "", "", "", "", "", true.ToString(), System.DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        //...............................................................................................
    }
    public void BindTree()
    {
        DataTable dtLoginuserPermission = new DataTable();
        DataTable DtTemp = new DataTable();
        dtLoginuserPermission = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
        string AppId = string.Empty;
        DataTable dtApp = objSysParam.GetSysParameterByParamName("Application_Id");
        if (dtApp.Rows.Count > 0)
        {
            AppId = dtApp.Rows[0]["Param_Value"].ToString().Trim();
        }
        else
        {
            return;
        }
        if (Session["UserId"].ToString().Trim().ToLower() != "superadmin")
        {
            //DataTable dtCust = objITCust.GetApplicationCustomer();
            //dtCust = new DataView(dtCust, "Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtCust.Rows.Count > 0)
            //{
            //    AppId = dtCust.Rows[0]["Application_Id"].ToString();
            //}
        }
        string RoleId = string.Empty;
        DataTable dtRoleP = new DataTable();
        DataTable dtUserP = new DataTable();
        if (editid.Value != "")
        {
            dtUserP = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), editid.Value);
        }
        foreach (TreeNode RoleNode in navTree.Nodes)
        {
            if (chkEditRole.Checked)
            {

                try
                {
                    DataTable dtRoleExists = new DataView(dtUserP, "Role_Id='" + RoleNode.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtRoleExists.Rows.Count > 0)
                    {
                        RoleNode.Checked = true;
                    }
                    else
                    {
                        RoleNode.Checked = false;
                    }
                }
                catch
                {
                }
            }
            else
            {
                RoleNode.Checked = true;
            }



            foreach (TreeNode ParentNode in RoleNode.ChildNodes)
            {
                foreach (TreeNode ModuleNode in ParentNode.ChildNodes)
                {


                    if (chkEditRole.Checked)
                    {
                        //here save one row for module
                        try
                        {
                            DataTable dtParenetModule = new DataView(dtUserP, "Module_Id='" + ModuleNode.Value.ToString() + "' and Role_Id='" + RoleNode.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtParenetModule.Rows.Count > 0)
                            {
                                ModuleNode.Checked = true;
                                ParentNode.Checked = true;
                            }
                            else
                            {
                                ModuleNode.Checked = false;
                                ParentNode.Checked = false;
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        ModuleNode.Checked = true;
                        ParentNode.Checked = true;
                    }
                    foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
                    {

                        if (chkEditRole.Checked)
                        {


                            try
                            {
                                DataTable dtObj = new DataView(dtUserP, "Object_Id='" + ObjNode.Value.ToString() + "' and Role_Id='" + RoleNode.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtObj.Rows.Count > 0)
                                {
                                    ObjNode.Checked = true;
                                }
                                else
                                {
                                    ObjNode.Checked = false;
                                }
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            ObjNode.Checked = true;
                        }
                        foreach (TreeNode OpNode in ObjNode.ChildNodes)
                        {
                            if (chkEditRole.Checked)
                            {
                                try
                                {
                                    DataTable dtOp = new DataView(dtUserP, "Op_Id=" + OpNode.Value.ToString() + " and Object_Id='" + ObjNode.Value.ToString() + "' and  Role_Id='" + RoleNode.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dtOp.Rows.Count > 0)
                                    {
                                        OpNode.Checked = true;
                                    }
                                    else
                                    {
                                        OpNode.Checked = false;
                                    }
                                }
                                catch
                                {
                                }
                            }
                            else
                            {
                                OpNode.Checked = true;
                            }
                        }
                    }
                }
            }
        }
        //DataTable dtRoleList = objRole.GetRoleMaster(Session["CompId"].ToString());
        //foreach (DataRow drRole in dtRoleList.Rows)
        //{
        //    string moduleids = string.Empty;
        //    TreeNode tnRole = new TreeNode(drRole["Role_Name"].ToString(), drRole["Role_Id"].ToString());
        //    tnRole.SelectAction = TreeNodeSelectAction.Expand;
        //    tnRole.ShowCheckBox = true;
        //    try
        //    {
        //        DataTable dtRoleExists = new DataView(dtUserP, "Role_Id='" + drRole["Role_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //        if (dtRoleExists.Rows.Count > 0)
        //        {
        //            tnRole.Checked = true;
        //        }
        //        else
        //        {
        //            tnRole.Checked = false;
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    //here we check that login user have permission or not of current role
        //    //itherwise we disable or visible false
        //    if (Session["EmpId"].ToString() != "0")
        //    {
        //        DtTemp = new DataView(dtLoginuserPermission, "Role_Id='" + drRole["Role_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //        if (DtTemp.Rows.Count == 0)
        //        {
        //            //tnRole.ShowCheckBox = false;
        //            //tnRole.SelectAction = TreeNodeSelectAction.None;
        //        }
        //    }
        //    dtRoleP = objRolePermission.GetRolePermissionById(drRole["Role_Id"].ToString());
        //    DataTable dtModule = objModule.GetModuleMaster();
        //    DataTable DtModuleApp = new DataTable();
        //    DtModuleApp = objModule.GetModuleObjectByApplicatonId(AppId);
        //    for (int i = 0; i < DtModuleApp.Rows.Count; i++)
        //    {
        //        //if login user is not superadmin in that case showing only selected module  in assigned role
        //        //code start
        //        DataTable dtRoleModule = new DataTable();
        //        //if (Session["RoleId"].ToString() != "0")
        //        //{
        //        try
        //        {
        //            dtRoleModule = new DataView(dtRoleP, "Module_Id=" + DtModuleApp.Rows[i]["Module_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //        if (dtRoleModule.Rows.Count > 0)
        //        {
        //            moduleids += "'" + DtModuleApp.Rows[i]["Module_Id"].ToString() + "'" + ",";
        //        }
        //        //}
        //        ////code end
        //        //else
        //        //{
        //        //    moduleids += "'" + DtModuleApp.Rows[i]["Module_Id"].ToString() + "'" + ",";
        //        //}
        //    }
        //    DataTable DtOpType = ObjItObject.GetOpType();
        //    DataTable DtModuleParentId = new DataTable();
        //    try
        //    {
        //        DtModuleParentId = new DataView(dtModule, "Module_Id in (" + moduleids.Substring(0, moduleids.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "ParentModuleId", "ParentModuleName");
        //    }
        //    catch
        //    {
        //    }
        //    foreach (DataRow dataParentrow in DtModuleParentId.Rows)
        //    {
        //        TreeNode tnParent = new TreeNode(dataParentrow["ParentModuleName"].ToString(), dataParentrow["ParentModuleId"].ToString());
        //        tnParent.SelectAction = TreeNodeSelectAction.Expand;
        //        tnParent.ShowCheckBox = true;
        //        tnRole.ChildNodes.Add(tnParent);
        //        DataTable dtModuleList = new DataView(dtModule, "Module_Id=" + dataParentrow["ParentModuleId"].ToString() + " or ParentModuleId='" + dataParentrow["ParentModuleId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //        foreach (DataRow datarow in dtModuleList.Rows)
        //        {
        //            DataTable dtModuleSaved = new DataView(dtRoleP, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //            if (dtModuleSaved.Rows.Count == 0)
        //            {
        //                continue;
        //            }
        //            if (dtUserP.Rows.Count > 0)
        //            {
        //                dtModuleSaved = new DataView(dtUserP, "Module_Id='" + datarow["Module_Id"].ToString() + "' and Role_Id='" + drRole["Role_Id"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //            }
        //            TreeNode tn = new TreeNode();
        //            tn = new TreeNode(datarow["Module_Name"].ToString(), datarow["Module_Id"].ToString());
        //            tn.SelectAction = TreeNodeSelectAction.Expand;
        //            //for checkbox vicibility 
        //            tn.ShowCheckBox = true;
        //            if (dtModuleSaved.Rows.Count > 0)
        //            {
        //                tn.Checked = true;
        //                tnParent.Checked = true;
        //            }
        //            //here we check that login user have permission or not of current role
        //            //itherwise we disable or visible false
        //            if (Session["EmpId"].ToString() != "0")
        //            {
        //                DtTemp = new DataView(dtLoginuserPermission, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //                if (DtTemp.Rows.Count == 0)
        //                {
        //                    //tn.ShowCheckBox = false;
        //                }
        //            }
        //            tnParent.ChildNodes.Add(tn);
        //            DataTable dtAllChild = ObjItObject.GetObjectMasterByModuleId_ApplicationId(datarow["Module_Id"].ToString(), AppId);
        //            dtAllChild = new DataView(dtAllChild, "", "Order_Appear", DataViewRowState.CurrentRows).ToTable();
        //            foreach (DataRow childrow in dtAllChild.Rows)
        //            {
        //                DtOpType = ObjItObject.GetOpType();
        //                string GetUrl = string.Empty;
        //                GetUrl = childrow[0].ToString();
        //                //if login user is not superadmin in that case showing only selected module  in assigned role
        //                //code start
        //                //if (Session["RoleId"].ToString() != "0")
        //                //{
        //                DataTable DtObject = new DataView(dtRoleP, "Object_Id=" + GetUrl + "", "", DataViewRowState.CurrentRows).ToTable();
        //                if (DtObject.Rows.Count == 0)
        //                {
        //                    continue;
        //                }
        //                //}
        //                //code end
        //                TreeNode tnChild = new TreeNode(childrow[1].ToString(), GetUrl);
        //                tnChild.SelectAction = TreeNodeSelectAction.Expand;
        //                //for checkbox vicibility 
        //                tnChild.ShowCheckBox = true;
        //                DataTable dtObj = new DataTable();
        //                if (dtUserP.Rows.Count > 0)
        //                {
        //                    dtObj = new DataView(dtUserP, "Object_Id='" + GetUrl + "' and Role_Id='" + drRole["Role_Id"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //                }
        //                if (dtObj.Rows.Count > 0)
        //                {
        //                    tnChild.Checked = true;
        //                }
        //                //here we check that login user have permission or not of current role
        //                //itherwise we disable or visible false
        //                if (Session["EmpId"].ToString() != "0")
        //                {
        //                    DtTemp = new DataView(dtLoginuserPermission, "Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
        //                    if (DtTemp.Rows.Count == 0)
        //                    {
        //                        //tnChild.ShowCheckBox = false;
        //                    }
        //                }
        //                tn.ChildNodes.Add(tnChild);
        //                //this code is created by jitendra upadhyay on 05-09-2014
        //                //this code for get the record fro it_app_op_permission for showing specific operation according the object id and module id 
        //                //code start
        //                DataTable DtopPermission = obj_OP_Permission.GetRecord(GetUrl);
        //                if (DtopPermission.Rows.Count > 0)
        //                {
        //                    string Object_Opeartion = string.Empty;
        //                    foreach (DataRow dr_op in DtopPermission.Rows)
        //                    {
        //                        if (Object_Opeartion == "")
        //                        {
        //                            Object_Opeartion = dr_op["Op_Id"].ToString();
        //                        }
        //                        else
        //                        {
        //                            Object_Opeartion = Object_Opeartion + "," + dr_op["Op_Id"].ToString();
        //                        }
        //                    }
        //                    try
        //                    {
        //                        DtOpType = new DataView(DtOpType, "Op_id in (" + Object_Opeartion + ")", "", DataViewRowState.CurrentRows).ToTable();
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    foreach (DataRow drOpType in DtOpType.Rows)
        //                    {
        //                        //if login user is not superadmin in that case showing only selected module  in assigned role
        //                        //code start
        //                        //if (Session["RoleId"].ToString() != "0")
        //                        //{
        //                        DataTable DtOpObject = new DataView(dtRoleP, "Object_Id=" + childrow["Object_Id"].ToString() + " and Op_Id=" + drOpType["Op_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //                        if (DtObject.Rows.Count == 0)
        //                        {
        //                            continue;
        //                        }
        //                        //}
        //                        //code end
        //                        TreeNode tnOpType = new TreeNode(drOpType[1].ToString(), drOpType[0].ToString());
        //                        tnOpType.SelectAction = TreeNodeSelectAction.Expand;
        //                        //for checkbox vicibility 
        //                        tnOpType.ShowCheckBox = true;
        //                        //this code is modify by the jitendra on 17-10-2014
        //                        //this code update according the database modification
        //                        DataTable dtOp = new DataTable();
        //                        if (dtUserP.Rows.Count > 0)
        //                        {
        //                            dtOp = new DataView(dtUserP, "Op_Id=" + drOpType["Op_Id"].ToString() + " and Object_Id='" + GetUrl + "' and  Role_Id='" + drRole["Role_Id"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //                        }
        //                        if (dtOp.Rows.Count > 0)
        //                        {
        //                            tnOpType.Checked = true;
        //                        }
        //                        //here we check that login user have permission or not of current role
        //                        //itherwise we disable or visible false
        //                        if (Session["EmpId"].ToString() != "0")
        //                        {
        //                            DtTemp = new DataView(dtLoginuserPermission, "Op_Id='" + drOpType[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //                            if (DtTemp.Rows.Count == 0)
        //                            {
        //                                //tnOpType.ShowCheckBox = false;
        //                            }
        //                        }
        //                        tnChild.ChildNodes.Add(tnOpType);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    navTree.Nodes.Add(tnRole);
        //}
        //navTree.DataBind();
        //navTree.CollapseAll();
        //return;
        navTree.CollapseAll();
    }
    public void BindTreeChanged(string StrRoleId)
    {
        DataTable dtLoginuserPermission = new DataTable();
        DataTable DtTemp = new DataTable();
        dtLoginuserPermission = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
        string AppId = string.Empty;
        DataTable dtApp = objSysParam.GetSysParameterByParamName("Application_Id");
        if (dtApp.Rows.Count > 0)
        {
            AppId = dtApp.Rows[0]["Param_Value"].ToString().Trim();
        }
        else
        {
            return;
        }
        string RoleId = string.Empty;
        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();
        navTree.Attributes.Add("onclick", "OnTreeClick(event)");
        DataTable dtRoleP = new DataTable();
        DataTable dtUserP = new DataTable();
        DataTable dtRoleList = objRole.GetRoleMaster(Session["CompId"].ToString());
        chkSelectAll.Checked = false;
        DataTable dtModule = objModule.GetModuleMaster();
        DataTable DtModuleApp = new DataTable();
        DataTable DtOpType = ObjItObject.GetOpType();
        DtModuleApp = objModule.GetModuleObjectByApplicatonId(AppId);
        //if (chkEditRole.Checked)
        //{
        if (editid.Value != "")
        {
            dtUserP = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), editid.Value);
        }
        //}
        DataTable DtopPermissionAllRec = objDa.return_DataTable("select * from IT_App_Op_Permission");
        DataTable DtopPermission = new DataTable();
        dtRoleList = new DataView(dtRoleList, "Role_id in (" + StrRoleId.ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        foreach (DataRow drRole in dtRoleList.Rows)
        {
            string moduleids = string.Empty;
            //if (!StrRoleId.Split(',').Contains(drRole["Role_Id"].ToString()))
            //{
            //    continue;
            //}
            TreeNode tnRole = new TreeNode(drRole["Role_Name"].ToString(), drRole["Role_Id"].ToString());
            tnRole.SelectAction = TreeNodeSelectAction.Expand;
            tnRole.ShowCheckBox = true;
            //here we check that login user have permission or not of current role
            //itherwise we disable or visible false
            //if (Session["EmpId"].ToString() != "0")
            //{
            //    DtTemp = new DataView(dtRoleP, "Role_Id='" + drRole["Role_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    if (DtTemp.Rows.Count == 0)
            //    {
            //        //tnRole.ShowCheckBox = false;
            //    }
            //}
            //tnRole.Checked = true;
            dtRoleP = objRolePermission.GetRolePermissionById(drRole["Role_Id"].ToString());
            for (int i = 0; i < DtModuleApp.Rows.Count; i++)
            {
                //if login user is not superadmin in that case showing only selected module  in assigned role
                //code start
                DataTable dtRoleModule = new DataTable();
                //if (Session["RoleId"].ToString() != "0")
                //{
                try
                {
                    dtRoleModule = new DataView(dtRoleP, "Module_Id=" + DtModuleApp.Rows[i]["Module_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtRoleModule.Rows.Count > 0)
                {
                    moduleids += "'" + DtModuleApp.Rows[i]["Module_Id"].ToString() + "'" + ",";
                }
                //}
                ////code end
                //else
                //{
                //    moduleids += "'" + DtModuleApp.Rows[i]["Module_Id"].ToString() + "'" + ",";
                //}
            }
            if (moduleids == "")
            {
                moduleids = "0,";
            }
            DataTable DtModuleParentId = new DataView(dtModule, "Module_Id in (" + moduleids.Substring(0, moduleids.Length - 1) + ")", "Sequence_No", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "ParentModuleId", "ParentModuleName");
            foreach (DataRow dataParentrow in DtModuleParentId.Rows)
            {
                TreeNode tnParent = new TreeNode(dataParentrow["ParentModuleName"].ToString(), dataParentrow["ParentModuleId"].ToString());
                tnParent.SelectAction = TreeNodeSelectAction.Expand;
                tnParent.ShowCheckBox = true;
                DataTable dtModuleList = new DataView(dtModule, "Module_Id=" + dataParentrow["ParentModuleId"].ToString() + " or ParentModuleId='" + dataParentrow["ParentModuleId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                tnRole.ChildNodes.Add(tnParent);
                foreach (DataRow datarow in dtModuleList.Rows)
                {
                    TreeNode tn = new TreeNode();
                    tn = new TreeNode(datarow["Module_Name"].ToString(), datarow["Module_Id"].ToString());
                    DataTable dtModuleSaved = new DataTable();
                    //if (dtUserP.Rows.Count > 0)
                    //{
                    //    dtModuleSaved = new DataView(dtUserP, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //}
                    //else
                    //{
                    dtModuleSaved = new DataView(dtRoleP, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //}
                    if (dtModuleSaved.Rows.Count == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //tn.Checked = true;
                    }
                    //for checkbox vicibility 
                    tn.ShowCheckBox = true;
                    //here we check that login user have permission or not of current role
                    //itherwise we disable or visible false
                    if (Session["EmpId"].ToString() != "0")
                    {
                        DtTemp = new DataView(dtLoginuserPermission, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (DtTemp.Rows.Count == 0)
                        {
                            //tn.ShowCheckBox = false;
                        }
                    }
                    tnParent.ChildNodes.Add(tn);
                    // code commented by neelkanth purohit 18-jun-2017 reuse same datatable
                    //DataTable dtAllChild = ObjItObject.GetObjectMasterByModuleId_ApplicationId(datarow["Module_Id"].ToString(), AppId);
                    //dtAllChild = new DataView(dtAllChild, "", "Order_Appear", DataViewRowState.CurrentRows).ToTable();
                    //dtAllChild = ObjItObject.GetObjectMasterByModuleId_ApplicationId(datarow["Module_Id"].ToString(), AppId);
                    DataTable dtAllChild = new DataView(DtModuleApp, "Module_Id='" + datarow["Module_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow childrow in dtAllChild.Rows)
                    {
                        DtOpType = ObjItObject.GetOpType();
                        string GetUrl = string.Empty;
                        GetUrl = childrow["object_id"].ToString();
                        //if login user is not superadmin in that case showing only selected module  in assigned role
                        //code start
                        //if (Session["RoleId"].ToString() != "0")
                        //{
                        DataTable DtObject = new DataView(dtRoleP, "Object_Id=" + GetUrl + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (DtObject.Rows.Count == 0)
                        {
                            continue;
                        }
                        //}
                        //code end
                        TreeNode tnChild = new TreeNode(childrow["Object_Name"].ToString(), GetUrl);
                        DataTable dtObj = new DataTable();
                        //if (dtUserP.Rows.Count > 0)
                        //{
                        //    dtObj = new DataView(dtUserP, "Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        //else
                        //{
                        dtObj = new DataView(dtRoleP, "Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
                        //}
                        if (dtObj.Rows.Count > 0)
                        {
                            //tnChild.Checked = true;
                        }
                        //for checkbox vicibility 
                        tnChild.ShowCheckBox = true;
                        //here we check that login user have permission or not of current role
                        //itherwise we disable or visible false
                        if (Session["EmpId"].ToString() != "0")
                        {
                            DtTemp = new DataView(dtLoginuserPermission, "Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (DtTemp.Rows.Count == 0)
                            {
                                //tnChild.ShowCheckBox = false;
                            }
                        }
                        tn.ChildNodes.Add(tnChild);
                        //this code is created by jitendra upadhyay on 05-09-2014
                        //this code for get the record fro it_app_op_permission for showing specific operation according the object id and module id 
                        //code start
                        //commented by neelkanth purohit get all records in single attempt and reuse
                        //DataTable DtopPermission = obj_OP_Permission.GetRecord(childrow["Object_Id"].ToString());
                        DtopPermission = new DataView(DtopPermissionAllRec, "Object_Id='" + childrow["Object_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (DtopPermission.Rows.Count > 0)
                        {
                            string Object_Opeartion = string.Empty;
                            foreach (DataRow dr_op in DtopPermission.Rows)
                            {
                                if (Object_Opeartion == "")
                                {
                                    Object_Opeartion = dr_op["Op_Id"].ToString();
                                }
                                else
                                {
                                    Object_Opeartion = Object_Opeartion + "," + dr_op["Op_Id"].ToString();
                                }
                            }
                            try
                            {
                                DtOpType = new DataView(DtOpType, "Op_id in (" + Object_Opeartion + ")", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {
                            }
                            foreach (DataRow drOpType in DtOpType.Rows)
                            {
                                //if login user is not superadmin in that case showing only selected module  in assigned role
                                //code start
                                //if (Session["RoleId"].ToString() != "0")
                                //{
                                DataTable DtOpObject = new DataView(dtRoleP, "Object_Id=" + childrow["Object_Id"].ToString() + " and Op_Id=" + drOpType["Op_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                                if (DtOpObject.Rows.Count == 0)
                                {
                                    continue;
                                }
                                //}
                                //code end
                                TreeNode tnOpType = new TreeNode(drOpType[1].ToString(), drOpType[0].ToString());
                                //this code is modify by the jitendra on 17-10-2014
                                //this code update according the database modification
                                DataTable dtOp = new DataTable();
                                //if (dtUserP.Rows.Count > 0)
                                //{
                                //    dtOp = new DataView(dtUserP, "Op_Id=" + drOpType["Op_Id"].ToString() + " and Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
                                //}
                                //else
                                //{
                                dtOp = new DataView(dtRoleP, "Op_Id=" + drOpType["Op_Id"].ToString() + " and Object_Id='" + GetUrl + "'", "", DataViewRowState.CurrentRows).ToTable();
                                //for checkbox vicibility 
                                tnOpType.ShowCheckBox = true;
                                if (dtOp.Rows.Count > 0)
                                {
                                    //tnOpType.Checked = true;
                                }
                                //here we check that login user have permission or not of current role
                                //itherwise we disable or visible false
                                if (Session["EmpId"].ToString() != "0")
                                {
                                    DtTemp = new DataView(dtLoginuserPermission, "Op_Id='" + drOpType[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (DtTemp.Rows.Count == 0)
                                    {
                                        //tnOpType.ShowCheckBox = false;
                                    }
                                }
                                tnChild.ChildNodes.Add(tnOpType);
                            }
                        }
                    }
                }
            }
            navTree.Nodes.Add(tnRole);
        }
        navTree.DataBind();
        navTree.CollapseAll();
        if (chkEditRole.Checked)
        {
            //navTree.Enabled = true;
            chkSelectAll.Visible = true;
        }
        else
        {
            //navTree.Enabled = false;
            chkSelectAll.Visible = false;
        }
        return;
    }
    protected void btnSavePerm_Click(object sender, EventArgs e)
    {
        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
        {
        }
        objUserPermission.DeleteUserPermission(Session["CompId"].ToString(), editid.Value);
        DataTable dtUserPAll = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), editid.Value);
        DataTable dtUserP = new DataTable();
        foreach (TreeNode RoleNode in navTree.Nodes)
        {
            foreach (TreeNode ParentNode in RoleNode.ChildNodes)
            {
                foreach (TreeNode ModuleNode in ParentNode.ChildNodes)
                {
                    //here save one row for module
                    if (ModuleNode.Checked)
                    {
                        foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
                        {
                            if (ObjNode.Checked)
                            {
                                //here we are checking that this object id is already inserte dor not with another role
                                dtUserP = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), editid.Value);
                                try
                                {
                                    dtUserP = new DataView(dtUserP, "Module_Id='" + ModuleNode.Value.Trim() + "' and Object_Id='" + ObjNode.Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                catch
                                {
                                }
                                if (dtUserP.Rows.Count > 0)
                                {
                                    dtUserP.Dispose();
                                    continue;
                                }
                                dtUserP.Dispose();
                                int refid1 = 0;
                                refid1 = objUserPermission.InsertUserPermission(Session["CompId"].ToString(), editid.Value, ModuleNode.Value, ObjNode.Value, RoleNode.Value, "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                string refid = refid1.ToString();
                                //this code is modify by jitendra upadhyay on 25-09-2014
                                //this code for delete the all permission using ref id and insert single row in user_oppermission
                                //code start
                                objUserPermission.DeleteUserOpPermission_By_RefId(refid.ToString());
                                //code end
                                //First Delete On Basis Of refid
                                // bool b2= false;
                                // Decalre Variable B1 To B7 As Boolean And Set False
                                foreach (TreeNode OpNode in ObjNode.ChildNodes)
                                {
                                    //this code is modify by jitendra upadhyay on 17-10-2014
                                    //modify the insert operation according the table modifications
                                    if (OpNode.Checked == true)
                                    {
                                        objUserPermission.InsertUserOpPermission(refid, OpNode.Value);
                                    }
                                }
                                //code end
                                dtUserP.Clear();
                            }
                        }
                    }
                }
            }
        }
        DisplayMessage("Record Saved", "green");
        //btnList_Click(null, null);
        //Reset();
    }
    protected void btnCancelPerm_Click(object sender, EventArgs e)
    {
        Reset();
        FillGrid();
        FillGridBin();
        Div_Rol_Permission.Style.Add("display", "none");
    }
    protected void navTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (e != null && e.Node != null)
        {
            try
            {
                if (e.Node.Checked == true)
                {
                    CheckTreeNodeRecursive(e.Node, true);
                    try
                    {
                        SelectChild(e.Node);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    CheckTreeNodeRecursive(e.Node, false);
                    UnSelectChild(e.Node);
                }
            }
            catch (Exception)
            {
            }
        }
        //GetRoleInformation();
    }
    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {
        foreach (TreeNode child in parent.ChildNodes)
        {
            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;
            }
            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }
        }
    }
    protected void navTree_SelectedNodeChanged1(object sender, EventArgs e)
    {
        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
        {
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string SelectedComp = string.Empty;
        string SelectedBrand = string.Empty;
        string SelectedLocation = string.Empty;
        string SelectedDepartment = string.Empty;
        string strAndroidDevice = string.Empty;
        //FillddlRole();
        editid.Value = e.CommandArgument.ToString();
        //FillAndroidDevice();
        DataTable dt = objUser.GetUserMasterByUserId(editid.Value, "");
        try
        {
            dt = new DataView(dt, "Company_Id=" + Session["CompId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            dt = new DataTable();
        }
        chkEditRole.Checked = false;
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Is_Email"].ToString() != "" && dt.Rows[0]["Is_Email"].ToString() != null)
            {
                chkEmailConfig.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Email"].ToString());
            }

            txtUserName.Text = dt.Rows[0]["User_Id"].ToString();
            hdnempid.Value = dt.Rows[0]["Emp_Id"].ToString();
            btnEmailConfiguration.Visible = IssalesPerson(hdnempid.Value);
            txtUserName.ReadOnly = true;
            txtPassword.Attributes.Add("Value", Common.Decrypt(dt.Rows[0]["Password"].ToString()));
            if (dt.Rows[0]["IsGlobalAccess"].ToString() == "" || dt.Rows[0]["IsGlobalAccess"].ToString().ToLower() == "false")
            {
                chkIsGlobalAccess.Checked = false;
            }
            else
            {
                chkIsGlobalAccess.Checked = true;
            }
            chklocation_SelectAll.Checked = false;
            chkdepartment.Checked = false;
            FillCompany();
            FillBrand();
            FillLocation();
            bindTreeDepartment();
            DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), editid.Value);
            if (dtRoleData.Rows.Count > 0)
            {
                // Company Master .............
                DataTable dtComp = new DataView(dtRoleData, "Record_Type='C'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtComp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtComp.Rows)
                    {
                        SelectedComp += dr["Record_Id"] + ",";
                        ViewState["CompIds"] = SelectedComp;
                    }
                    for (int j = 0; j < SelectedComp.Split(',').Length; j++)
                    {
                        if (SelectedComp.Split(',')[j] != "")
                        {
                            try
                            {
                                chkCompany.Items.FindByValue(SelectedComp.Split(',')[j]).Selected = true;
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }
                ///...........................
                chkCompany_SelectedIndexChanged(null, null);
                foreach (ListItem lst1 in chkBrand.Items)
                {
                    chkBrand.Items.FindByValue(lst1.Value).Selected = false;
                }
                DataTable dtBrand = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtBrand.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtBrand.Rows)
                    {
                        SelectedBrand += dr["Record_Id"] + ",";
                        ViewState["BrandChecked"] = SelectedBrand;
                    }
                    for (int j = 0; j < SelectedBrand.Split(',').Length; j++)
                    {
                        if (SelectedBrand.Split(',')[j] != "")
                        {
                            try
                            {
                                chkBrand.Items.FindByValue(SelectedBrand.Split(',')[j]).Selected = true;
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }
                chkBrand_SelectedIndexChanged(null, null);
                foreach (ListItem lst1 in chkLocation.Items)
                {
                    chkLocation.Items.FindByValue(lst1.Value).Selected = false;
                }
                DataTable dtLocation = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLocation.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtLocation.Rows)
                    {
                        SelectedLocation += dr["Record_Id"] + ",";
                        ViewState["LocationChecked"] = SelectedLocation;
                    }
                    for (int j = 0; j < SelectedLocation.Split(',').Length; j++)
                    {
                        if (SelectedLocation.Split(',')[j] != "")
                        {
                            try
                            {
                                chkLocation.Items.FindByValue(SelectedLocation.Split(',')[j]).Selected = true;
                            }
                            catch (Exception ex)
                            {
                                continue;
                            }
                        }
                    }
                }
                chkLocation_SelectedIndexChanged(null, null);
            }
            try
            {
                strAndroidDevice = dt.Rows[0]["Field1"].ToString();
                if (strAndroidDevice != "0")
                {
                    ddlAndroidDevice.SelectedValue = strAndroidDevice;
                    ViewState["Device_Id"] = strAndroidDevice;
                    if (ViewState["Device_Id"].ToString() == null)
                    {
                        ViewState["Device_Id"] = "";
                    }
                }
            }
            catch
            {
            }
            try
            {
                ddlLanguage.SelectedValue = dt.Rows[0]["Field4"].ToString();
            }
            catch
            {
            }
            try
            {
                chkIsStandAloneUser.Checked = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
            }
            catch
            {
                chkIsStandAloneUser.Checked = false;
            }
            try
            {
                chkEditRole.Checked = Convert.ToBoolean(dt.Rows[0]["Is_Modified"].ToString());
            }
            catch
            {
                chkEditRole.Checked = false;
            }
            chkEditRoleCheckedChanged(null, null);
            //here we are getting strrole Id 
            DataTable dtUserP = objUserPermission.GetUserPermissionById(Session["CompId"].ToString(), editid.Value);
            DataTable dtUser = objUser.GetUserMasterByUserId(editid.Value.ToLower(), Session["CompId"].ToString());
            try
            {
                dtUser = new DataView(dtUser, "Company_Id=" + Session["CompId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtUser.Rows.Count > 0)
            {
                if (dtUser.Rows[0]["Role_Name"].ToString() == "Super Admin")
                {
                    rbtnUser.Checked = false;
                    rbtnSuperAdmin.Checked = true;
                    trRole.Visible = false;
                    trEmp.Visible = false;
                    trEditRole.Visible = false;
                }
                else
                {
                    rbtnSuperAdmin.Checked = false;
                    rbtnUser.Checked = true;
                    trEmp.Visible = true;
                    try
                    {
                        ddlRole.SelectedValue = dtUser.Rows[0]["Role_Id"].ToString();
                    }
                    catch
                    {
                    }
                    hdnRoleId.Value = ddlRole.SelectedValue;
                    txtEmp.Text = objCmn.GetEmpName(dt.Rows[0]["Emp_Id"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    BindTreeChanged(dt.Rows[0]["DistinctRoleId"].ToString());
                    BindTree();
                    chkSelectAll.Visible = true;
                    //GetRoleInformation();
                }
            }
            ViewState["Device_Id"] = "";
            txtEmp.Focus();
            Lbl_New_tab.Text = Resources.Attendance.Edit.ToString();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_Edit_tab_position()", true);
        }
        Div_Rol_Permission.Style.Add("display", "");
        chkSelectAll.Checked = false;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objUser.DeleteUserMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvUserMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUserMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_User_Mstr"];
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvUserMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvUserMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_User_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_User_Mstr"] = dt;
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvUserMaster, dt, "", "");
        //AllPageCode();
        string userid = Session["UserId"].ToString().Trim();
        userid = userid.ToLower();
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //FillGrid();
        //FillGridBin();
        Reset();
        FillGrid();
        Lbl_New_tab.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void FillGrid()
    {
        string optype = string.Empty;
        string RoleName = string.Empty;
        DataTable dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            if (dtUser.Rows[0]["Role_Name"].ToString() == "Super Admin" || dtUser.Rows[0]["Role_Id"].ToString() == "0")
            {
                optype = "4";
            }
            else
            {
                optype = "5";
            }
        }

        DataTable dt = objUser.GetUserMasterByUserIdByCompId(Session["UserId"].ToString(), "12", HttpContext.Current.Session["CompId"].ToString());
        dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        if(ddlLocation.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id in (" + ddlLocation.SelectedValue + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        //dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id in (" + ddlLocation.SelectedValue + ")", "", DataViewRowState.CurrentRows).ToTable();


        //Common Function add By Lokesh on 22-05-2015

        objPageCmn.FillData((object)gvUserMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_User_Mstr"] = dt;
        Session["User"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        string userid = Session["UserId"].ToString().Trim();
        userid = userid.ToLower();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objUser.GetUserMasterInactive(Session["CompId"].ToString());
        //Common Function add By Lokesh on 22-05-2015
        objPageCmn.FillData((object)gvUserMasterBin, dt, "", "");
        Session["dtbinFilter"] = dt;
        Session["dtbinUser"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvUserMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvUserMasterBin.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }
                if (!userdetails.Contains(dr["User_Id"]))
                {
                    userdetails.Add(dr["User_Id"]);
                }
            }
            foreach (GridViewRow GR in gvUserMasterBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtbinUser"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvUserMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {

        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvUserMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objUser.DeleteUserMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvUserMasterBin.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
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
                gvUserMasterBin.Focus();
                return;
            }
        }
    }
    public void Reset()
    {
        Session["CHECKED_ITEMS"] = null;
        txtUserName.Text = "";
        txtUserName.ReadOnly = false;
        txtPassword.Text = "";
        txtEmp.Text = "";
        txtPassword.Attributes.Add("Value", txtPassword.Text);
        chkEditRole.Checked = false;
        Lbl_New_tab.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        hdnRoleId.Value = "";
        ddlRole.SelectedIndex = 0;
        rbtnSuperAdmin.Checked = false;
        rbtnUser.Checked = true;
        trRole.Visible = false;
        trEmp.Visible = true;
        trEditRole.Visible = false;
        //FillAndroidDevice();
        chkSelectAll.Visible = false;
        txtRoleName.Text = "";
        btnEmailConfiguration.Visible = false;
        //GetRoleInformation();
        FillCompany();
        hdnempid.Value = "0";
        chkBrand.Items.Clear();
        chkLocation.Items.Clear();
        TreeViewDepartment.DataSource = null;
        TreeViewDepartment.DataBind();
        TreeViewDepartment.Nodes.Clear();
        Div_Rol_Permission.Style.Add("display", "none");
        chklocation_SelectAll.Checked = false;
        chkdepartment.Checked = false;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["emp_name"].ToString() + "/(" + dt.Rows[i]["Designation"].ToString() + ")/" + dt.Rows[i]["emp_code"].ToString() + "";
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListUser(string prefixText, int count, string contextKey)
    {
        UserMaster objUser1 = new UserMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string optype = string.Empty;
        string RoleName = string.Empty;
        DataTable dtUser = objUser1.GetUserMasterByUserId(HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            if (dtUser.Rows[0]["Role_Name"].ToString() == "Super Admin")
            {
                optype = "4";
            }
            else
            {
                optype = "5";
            }
        }
        DataTable dt = new DataView(objUser1.GetUserMasterByUserIdByCompId(HttpContext.Current.Session["CompId"].ToString(), optype, HttpContext.Current.Session["CompId"].ToString()), "User_Id like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["User_Id"].ToString();
        }
        return txt;
    }
    protected void ddlEmp_TextChanged(object sender, EventArgs e)
    {
        Div_Rol_Permission.Style.Add("display", "none");
        chkSelectAll.Visible = false;
        string empid = txtEmp.Text.Split('/')[txtEmp.Text.Split('/').Length - 1];
        txtUserName.Text = empid;
        txtPassword.Attributes.Add("Value", empid);
        ddlRole.Focus();
        DataTable dt = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empid);
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Select Employee in suggestion Only");
            txtEmp.Text = "";
            txtEmp.Focus();
            hdnempid.Value = "0";
            return;
        }
        else
        {
            hdnempid.Value = dt.Rows[0]["Emp_Id"].ToString();
            btnEmailConfiguration.Visible = IssalesPerson(hdnempid.Value);
        }

        Div_Rol_Permission.Style.Add("display", "");
        navTree.Nodes.Clear();
        navTree.DataSource = null;
        navTree.DataBind();
        chkCompany.DataSource = null;
        chkCompany.DataBind();
        chkBrand.DataSource = null;
        chkBrand.DataBind();
        chkLocation.DataSource = null;
        chkLocation.DataBind();
        //chkDepartment.DataSource = null;
        //chkDepartment.DataBind();
        //foreach (TreeNode RoleNode in navTree.Nodes)
        //{
        //    RoleNode.Checked = false;
        //    foreach (TreeNode ParentNode in RoleNode.ChildNodes)
        //    {
        //        ParentNode.Checked = false;
        //        foreach (TreeNode ModuleNode in ParentNode.ChildNodes)
        //        {
        //            ModuleNode.Checked = false;
        //            foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
        //            {
        //                ObjNode.Checked = false;
        //                //here we are checking that this object id is already inserte dor not with another role
        //                foreach (TreeNode OpNode in ObjNode.ChildNodes)
        //                {
        //                    OpNode.Checked = false;
        //                }
        //                //code end
        //            }
        //        }
        //    }
        //}
        chkSelectAll.Visible = true;
        //GetRoleInformation();
    }

    public bool IssalesPerson(string strEmpId)
    {
        bool result = false;
        DataTable dt = objDa.return_DataTable("select * from Inv_SalesCommissionConfiguration_Header where employee_id = " + strEmpId + "");
        if (dt.Rows.Count > 0)
            result = true;
        return result;

    }

    #region GetRoleInformation
    //public void GetRoleInformation()
    //{
    //    chkCompany.DataSource = null;
    //    chkCompany.DataBind();
    //    chkBrand.DataSource = null;
    //    chkBrand.DataBind();
    //    chkLocation.DataSource = null;
    //    chkLocation.DataBind();
    //    string strRoleId = string.Empty;
    //    foreach (TreeNode RoleNode in navTree.Nodes)
    //    {
    //        if (!strRoleId.Trim().Split(',').Contains(RoleNode.Value))
    //        {
    //            strRoleId += RoleNode.Value.Trim() + ",";
    //        }
    //    }
    //    if (strRoleId.Trim() != "")
    //    {
    //        string strSql = "select distinct Set_CompanyMaster.Company_Name  from Set_RoleDataPermission inner join Set_CompanyMaster on Set_RoleDataPermission.Record_Id=Set_CompanyMaster.Company_Id where Record_Type='C' and Role_Id in (" + strRoleId.Substring(0, strRoleId.Length - 1) + ")";
    //        objPageCmn.FillData((object)chkCompany, objDa.return_DataTable(strSql), "Company_Name", "Company_Name");
    //        strSql = "select distinct Set_BrandMaster.Brand_Name from Set_RoleDataPermission inner join Set_BrandMaster on Set_RoleDataPermission.Record_Id=Set_BrandMaster.Brand_Id where Record_Type='B' and  Role_Id in (" + strRoleId.Substring(0, strRoleId.Length - 1) + ")";
    //        objPageCmn.FillData((object)chkBrand, objDa.return_DataTable(strSql), "Brand_Name", "Brand_Name");
    //        strSql = "select distinct Set_LocationMaster.Location_Name from Set_RoleDataPermission inner join Set_LocationMaster on Set_RoleDataPermission.Record_Id=Set_LocationMaster.Location_Id where Record_Type='L' and  Role_Id in (" + strRoleId.Substring(0, strRoleId.Length - 1) + ")";
    //        objPageCmn.FillData((object)chkLocation, objDa.return_DataTable(strSql), "Location_Name", "Location_Name");
    //        strSql = "select distinct Set_DepartmentMaster.Dep_Name from Set_RoleDataPermission inner join Set_Location_Department on Set_RoleDataPermission.Record_Id=Set_Location_Department.Dep_Id left join Set_DepartmentMaster on Set_Location_Department.Dep_Id=Set_DepartmentMaster.Dep_Id where Record_Type='D' and   Role_Id in (" + strRoleId.Substring(0, strRoleId.Length - 1) + ")";
    //        objPageCmn.FillData((object)chkDepartment, objDa.return_DataTable(strSql), "Dep_Name", "Dep_Name");
    //    }
    //}
    #endregion
    #region addRoleDetail
    protected void btnAddRole_Click(object sender, EventArgs e)
    {
        string strRoleId = string.Empty;
        if (txtRoleName.Text == "")
        {
            DisplayMessage("Enter Role name");
            txtRoleName.Text = "";
            txtRoleName.Focus();
            return;
        }
        try
        {
            strRoleId = txtRoleName.Text.Split('/')[1].ToString() + ",";
        }
        catch
        {
            DisplayMessage("Select role name in suggestion only");
            txtRoleName.Text = "";
            txtRoleName.Focus();
            return;
        }
        foreach (TreeNode RoleNode in navTree.Nodes)
        {
            strRoleId += RoleNode.Value + ",";
        }
        BindTreeChanged(strRoleId);
        txtRoleName.Text = "";
        if (editid.Value != "")
        {
            BindTree();
            //GetRoleInformation();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRoleName(string prefixText, int count, string contextKey)
    {
        RoleMaster objRoleMaster = new RoleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objRoleMaster.GetRoleMaster(HttpContext.Current.Session["CompId"].ToString()), "Role_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Role_Name"].ToString() + "/" + dt.Rows[i]["Role_Id"].ToString();
        }
        return txt;
    }
    protected void txtRoleName_OnTextChanged(object sender, EventArgs e)
    {
        string strRoleName = string.Empty;
        string strRoleID = string.Empty;
        try
        {
            strRoleName = ((TextBox)sender).Text.Split('/')[0].ToString();
            strRoleID = ((TextBox)sender).Text.Split('/')[1].ToString();
            string roleid = objRole.GetRoleMasterByRoleName(Session["CompId"].ToString().ToString(), strRoleName.Trim(), strRoleID.Trim());
            if (roleid == "1")
            {
            }
            else
            {
                ((TextBox)sender).Text = "";
                DisplayMessage("Role name not Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
                return;
            }
        }
        catch
        {
            ((TextBox)sender).Text = "";
            DisplayMessage("select role name in suggestion only");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
            return;
        }
    }
    #endregion
    #region Search
    protected void txtSearchUserName_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSearchUserName.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtSearchUserName.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
            }
            else
            {
                DisplayMessage("Select user In Suggestions Only");
                txtSearchUserName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSearchUserName);
            }
        }
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        DataTable dtEmployee = new DataTable();
        DataTable dtEmp = new DataTable();
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            dtEmployee = objEmp.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];
                dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
                if (dtEmp.Rows.Count == 0)
                { retval = ""; }
            }
            else
            { retval = ""; }
        }
        else
        { retval = ""; }
        dtEmployee.Dispose();
        dtEmp.Dispose();
        return retval;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSearchEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        // dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        DataTable dt = new DataView(dt1, "Emp_Name like '%" + prefixText.ToString() + "%'", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }
        return txt;
    }
    protected void ddlSearchModuleName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable DtModuleApp = new DataTable();
        ddlserachObjectname.Items.Clear();
        if (ddlSearchModuleName.SelectedIndex > 0)
        {
            DtModuleApp = objModule.GetModuleObjectByApplicatonId(objSysParam.GetSysParameterByParamName("Application_Id").Rows[0]["Param_Value"].ToString());
            DtModuleApp = new DataView(DtModuleApp, "Module_Id=" + ddlSearchModuleName.SelectedValue.Trim() + "", "Order_Appear", DataViewRowState.CurrentRows).ToTable();
            if (DtModuleApp.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlserachObjectname, DtModuleApp, "Object_Name", "Object_Id");
            }
        }
        DtModuleApp.Dispose();
    }
    public void Fillopeartion()
    {
        DataTable DtOpType = objObject.GetOpType();
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)ddlOperation, DtOpType, "Op_Type", "Op_Id");
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        string strselect = string.Empty;
        string strwhere = string.Empty;
        string strSort = string.Empty;
        strselect = "SELECT Set_EmployeeMaster.Emp_Name as UserName, Set_RoleMaster.Role_Name as RoleName, IT_ModuleMaster.Module_Name as ModuleName, IT_ObjectEntry.Object_Name as ObjectName, (SELECT STUFF((SELECT DISTINCT ',' + RTRIM(Set_OperationType.Op_Type) FROM Set_OperationType WHERE Set_OperationType.Op_Id IN (SELECT DISTINCT Set_UserOpPermission.Op_Id FROM Set_UserOpPermission WHERE Set_UserOpPermission.User_Record_Id = Set_UserPermission.TransId) FOR xml PATH ('')), 1, 1, '')) AS Operation FROM Set_UserPermission left join Set_UserMaster on Set_UserPermission.User_Id = Set_UserMaster.User_Id left join IT_ObjectEntry on Set_UserPermission.Object_Id= IT_ObjectEntry.Object_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id left join Set_RoleMaster on Set_UserPermission.Field1 = Set_RoleMaster.Role_Id left join IT_ModuleMaster on Set_UserPermission.Module_Id= IT_ModuleMaster.Module_Id WHERE set_usermaster.isactive='True' and  set_employeemaster.field2='False'  ";
        if (txtSearchUserName.Text != "")
        {
            strwhere = " and set_usermaster.emp_id='" + txtSearchUserName.Text.Split('/')[1].ToString() + "'";
        }
        if (ddlRole.SelectedIndex > 0)
        {
            strwhere += " and Set_UserPermission.Field1='" + ddlRole.SelectedValue + "' ";
        }
        if (ddlSearchModuleName.SelectedIndex > 0)
        {
            strwhere += " and  Set_UserPermission.Module_Id='" + ddlSearchModuleName.SelectedValue + "' ";
        }
        if (ddlserachObjectname.SelectedIndex > 0)
        {
            strwhere += " and   Set_UserPermission.Object_Id='" + ddlserachObjectname.SelectedValue + "' ";
        }
        if (ddlOperation.SelectedIndex > 0)
        {
            strwhere += "   and Set_UserPermission.transid  in (select Set_UserOpPermission.User_Record_Id from Set_UserOpPermission where Set_UserOpPermission.Op_Id=" + ddlOperation.SelectedValue + ") ";
        }
        strSort = " order by  set_employeemaster.Emp_Name,IT_ModuleMaster.Module_Name,IT_ObjectEntry.Object_Name";
        try
        {
            dt = objDa.return_DataTable(strselect + strwhere + strSort);
        }
        catch
        {
            dt = null;
        }
        objPageCmn.FillData((object)gvRoleUser, dt, "", "");
        dt.Dispose();
    }
    #endregion
    #region CompanyBrandlocandDepartment
    private void FillCompany()
    {
        string CompIds = string.Empty;
        DataTable dtCompany = new DataTable();
        dtCompany = ObjComp.GetCompanyMaster();
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {
            //DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString(),"C");
            //DataTable dtComp = new DataView(dtRoleData, "Record_Type='C'", "", DataViewRowState.CurrentRows).ToTable();

            DataTable dtComp = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString(), "C");
            for (int C = 0; C < dtComp.Rows.Count; C++)
            {
                if (dtComp.Rows.Count > 0)
                {
                    CompIds += dtComp.Rows[C]["Record_Id"].ToString() + ",";
                }
            }
            if (CompIds != "")
                dtCompany = new DataView(dtCompany, "Company_Id in(" + CompIds.Substring(0, CompIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            else
            {
                DisplayMessage("Company is not selected for this user");
                return;
            }
            //}
        }
        if (dtCompany.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)chkCompany, dtCompany, "Company_Name", "Company_Id");
        }
    }
    public void FillBrand()
    {
        DataTable dtBrand = new DataTable();
        string BndIds = string.Empty;
        dtBrand = objBrand.GetAllBrandMaster();
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {

            //DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
            //DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtBnd = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString(), "B");

            for (int C = 0; C < dtBnd.Rows.Count; C++)
            {
                if (dtBnd.Rows.Count > 0)
                {
                    BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
                }
            }
            if (BndIds != "")
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            else
            {
                DisplayMessage("Brand is not selected for this user");
                return;
            }
            //}
        }
        if (dtBrand.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)chkBrand, dtBrand, "Brand_Name", "Brand_Id");
            //foreach (ListItem lst1 in chkBrand.Items)
            //{
            //    chkBrand.Items.FindByValue(lst1.Value).Selected = true;
            //}
        }
    }
    public void FillLocation()
    {
        DataTable dtLocation = new DataTable();
        string LocIds = string.Empty;
        dtLocation = objLocation.GetAllLocationMaster();
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {
            //DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
            //DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtLoc = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString(), "L");
            for (int L = 0; L < dtLoc.Rows.Count; L++)
            {
                if (dtLoc.Rows.Count > 0)
                {
                    LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
                }
            }
            if (LocIds != "")
                dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            else
            {
                DisplayMessage("Location is not selected for this user");
                return;
            }
            //}
        }
        if (dtLocation.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)chkLocation, dtLocation, "Location_Name", "Location_Id");
        }
    }
    protected void chkCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtBrand = new DataTable();
        string BndIds = string.Empty;
        string B1 = string.Empty;
        string B2 = string.Empty;
        string B3 = string.Empty;
        string TB = string.Empty;
        for (int i = 0; i < chkBrand.Items.Count; i++)
        {
            if (chkBrand.Items[i].Selected)
            {
                BrandChecked += chkBrand.Items[i].Value + ",";
            }
        }
        ViewState["BrandChecked"] = BrandChecked;
        CompanyIds = string.Empty;
        for (int i = 0; i < chkCompany.Items.Count; i++)
        {
            if (chkCompany.Items[i].Selected)
            {
                CompanyId = chkCompany.Items[i].Value;
                CompanyIds += CompanyId + ",";
            }
        }
        dtBrand = objBrand.GetBrandMaster(CompanyIds);
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {
            //if (editid.Value.ToString() != "")
            //{
            //    DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), editid.Value);
            //    DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
            //    for (int C = 0; C < dtBnd.Rows.Count; C++)
            //    {
            //        if (dtBnd.Rows.Count > 0)
            //        {
            //            BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
            //        }
            //    }
            //    if (BndIds != "")
            //        dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            //    else
            //    {
            //        DisplayMessage("Brand is not selected for this user");
            //        return;
            //    }
            //}
            //else
            //{
            DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
            DataTable dtBnd = new DataView(dtRoleData, "Record_Type='B'", "", DataViewRowState.CurrentRows).ToTable();
            for (int C = 0; C < dtBnd.Rows.Count; C++)
            {
                if (dtBnd.Rows.Count > 0)
                {
                    BndIds += dtBnd.Rows[C]["Record_Id"].ToString() + ",";
                }
            }
            if (BndIds != "")
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BndIds.Substring(0, BndIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            else
            {
                DisplayMessage("Brand is not selected for this user");
                return;
            }
            //}
        }
        for (int i = 0; i < dtBrand.Rows.Count; i++)
        {
            if (BrandChecked.Split(',').Contains(dtBrand.Rows[i]["Brand_Id"].ToString()))
            {
                B2 += dtBrand.Rows[i]["Brand_Id"].ToString() + ",";
            }
        }
        ViewState["BrandChecked"] = B2;
        DataTable dtFinal = dtBrand.Clone();
        string selectedBrand = string.Empty;
        for (int i = 0; i < chkCompany.Items.Count; i++)
        {
            if (chkCompany.Items[i].Selected)
            {
                selectedBrand += chkCompany.Items[i].Value + ",";
            }
        }
        foreach (ListItem lst in chkCompany.Items)
        {
            if (lst.Selected)
            {
                dtFinal.Merge(new DataView(dtBrand, "Company_Id='" + lst.Value + "'", "", DataViewRowState.CurrentRows).ToTable());
            }
        }
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)chkBrand, dtFinal, "Brand_Name", "Brand_Id");
        ViewState["CompIds"] = CompanyIds;
        chkBrand_SelectedIndexChanged(null, null);
    }
    protected void chkBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtLocation = new DataTable();
        List<int> List = new List<int>();
        string BIds = string.Empty;
        string LocIds = string.Empty;
        int BId = 0;
        string L2 = string.Empty;
        BrandChecked = ViewState["BrandChecked"].ToString();
        for (int i = 0; i < chkBrand.Items.Count; i++)
        {
            if (chkBrand.Items[i].Selected)
            {
                BId = Convert.ToInt32(chkBrand.Items[i].Value);
                //   chkBrand.Items.FindByValue(chkBrand.Items[i]. = true
                // List.Add(BId);
                BIds += BId + ",";
            }
        }
        if (BIds == "")
        {
            BrandChecked = BIds + BrandChecked;
        }
        else
        {
            BrandChecked = BIds;
        }
        for (int j = 0; j < BrandChecked.Split(',').Length - 1; j++)
        {
            try
            {
                chkBrand.Items.FindByValue(BrandChecked.Split(',')[j]).Selected = true;
            }
            catch
            {
            }
        }
        for (int i = 0; i < chkLocation.Items.Count; i++)
        {
            if (chkLocation.Items[i].Selected)
            {
                LocationChecked += chkLocation.Items[i].Value + ",";
            }
        }
        CompanyIds = ViewState["CompIds"].ToString();
        dtLocation = objLocation.GetLocationMaster(CompanyIds);
        if (Session["CompId"].ToString() == "0" || Session["RoleId"].ToString() == "0")
        {
        }
        else
        {
            //if (editid.Value.ToString() != "")
            //{
            //    DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), editid.Value);
            //    DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
            //    for (int L = 0; L < dtLoc.Rows.Count; L++)
            //    {
            //        if (dtLoc.Rows.Count > 0)
            //        {
            //            LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
            //        }
            //    }
            //    if (LocIds != "")
            //        dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            //    else
            //    {
            //        DisplayMessage("Location is not selected for this user");
            //        return;
            //    }
            //}
            //else
            //{
            DataTable dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
            DataTable dtLoc = new DataView(dtRoleData, "Record_Type='L'", "", DataViewRowState.CurrentRows).ToTable();
            for (int L = 0; L < dtLoc.Rows.Count; L++)
            {
                if (dtLoc.Rows.Count > 0)
                {
                    LocIds += dtLoc.Rows[L]["Record_Id"].ToString() + ",";
                }
            }
            if (LocIds != "")
                dtLocation = new DataView(dtLocation, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            else
            {
                DisplayMessage("Location is not selected for this user");
                return;
            }
            //}
        }
        try
        {
            dtLocation = new DataView(dtLocation, "Brand_Id in(" + BIds.Substring(0, BIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            // dtLocation = new DataView(dtLocation, "Brand_Id='" + BrandChecked.Split(',') + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        for (int i = 0; i < dtLocation.Rows.Count; i++)
        {
            if (LocationChecked.Split(',').Contains(dtLocation.Rows[i]["Location_Id"].ToString()))
            {
                L2 += dtLocation.Rows[i]["Location_Id"].ToString() + ",";
            }
        }
        DataTable dtFinal = dtLocation.Clone();
        string selectedLocation = string.Empty;
        for (int i = 0; i < chkBrand.Items.Count; i++)
        {
            if (chkBrand.Items[i].Selected)
            {
                selectedLocation += chkBrand.Items[i].Value + ",";
            }
        }
        foreach (ListItem lst in chkBrand.Items)
        {
            if (lst.Selected)
            {
                dtFinal.Merge(new DataView(dtLocation, "Brand_Id='" + lst.Value + "'", "", DataViewRowState.CurrentRows).ToTable());
            }
        }
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)chkLocation, dtFinal, "Location_Name", "Location_Id");
        ViewState["LocationChecked"] = L2;
        ViewState["BrandChecked"] = "";
        //ViewState["LocationChecked"] = "";
        chkLocation_SelectedIndexChanged(null, null);
        foreach (ListItem li in chkLocation.Items)
        {
            li.Selected = chklocation_SelectAll.Checked;
        }
        bindTreeDepartment();
    }
    protected void chkLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindTreeDepartment();
    }
    public void bindTreeDepartment()
    {
        string strRecordId = string.Empty;
        DataTable dtRoleData = new DataTable();
        DataTable dtUserdata = new DataTable();
        if (editid.Value != "")
        {
            dtRoleData = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), editid.Value);
            dtRoleData = new DataView(dtRoleData, "Record_Type='D'", "", DataViewRowState.CurrentRows).ToTable();
        }
        DataTable dtRolePermission = new DataTable();
        DataTable dtLDempart = new DataTable();
        TreeViewDepartment.DataSource = null;
        TreeViewDepartment.DataBind();
        TreeViewDepartment.Nodes.Clear();
        for (int i = 0; i < chkLocation.Items.Count; i++)
        {
            if (chkLocation.Items[i].Selected)
            {
                TreeNode tn = new TreeNode(chkLocation.Items[i].Text, chkLocation.Items[i].Value);
                dtLDempart = objLocDept.GetDepartmentByLocationId(chkLocation.Items[i].Value);
                if (Session["EmpId"].ToString() != "0")
                {
                    dtUserdata = objUserDataPerm.GetUserDataPermissionById(Session["CompId"].ToString(), Session["UserId"].ToString());
                    dtUserdata = new DataView(dtUserdata, "Record_Type='D' and Field1='" + chkLocation.Items[i].Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow dr in dtUserdata.Rows)
                    {
                        strRecordId += dr["Record_Id"].ToString() + ",";
                    }
                    if (strRecordId == "")
                    {
                        strRecordId = "0,";
                    }
                    dtLDempart = new DataView(dtLDempart, "Dep_Id in (" + strRecordId.Substring(0, strRecordId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                dtLDempart = new DataView(dtLDempart, "Dep_Id<>'0'", "Dep_Name", DataViewRowState.CurrentRows).ToTable();
                for (int j = 0; j < dtLDempart.Rows.Count; j++)
                {
                    TreeNode tnchild = new TreeNode();
                    tnchild = new TreeNode(dtLDempart.Rows[j]["Dep_Name"].ToString(), dtLDempart.Rows[j]["Dep_Id"].ToString());
                    if (editid.Value != "")
                    {
                        dtRolePermission = new DataView(dtRoleData, "Field1=" + chkLocation.Items[i].Value + " and Record_Id=" + dtLDempart.Rows[j]["Dep_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtRolePermission.Rows.Count > 0)
                        {
                            tnchild.Checked = true;
                            tn.Checked = true;
                        }
                    }
                    tn.ChildNodes.Add(tnchild);
                }
                TreeViewDepartment.Nodes.Add(tn);
            }
        }
        TreeViewDepartment.DataBind();
        TreeViewDepartment.ExpandAll();
    }
    #endregion
    protected void chklocation_SelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem li in chkLocation.Items)
        {
            li.Selected = chklocation_SelectAll.Checked;
        }
        bindTreeDepartment();
    }
    protected void BtnDeleteRole_Click(object sender, EventArgs e)
    {
        string strRoleId = string.Empty;
        foreach (TreeNode RoleNode in navTree.Nodes)
        {
            if (!RoleNode.Checked)
            {
                strRoleId += RoleNode.Value + ",";
            }
        }
        if (strRoleId == "")
        {
            strRoleId = "0";
        }
        BindTreeChanged(strRoleId);
        txtRoleName.Text = "";
        if (editid.Value != "")
        {
            BindTree();
        }
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = objCmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        ddlLocation.Items.Insert(0, new ListItem("--Select Location--"));
        dtLoc = null;
    }
    protected void btnRoleEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;

        string userName = "", roleName = "", moduleName = "", objectName = "", operation = "";
        userName = ((Label)gvrow.FindControl("lbluserName")).Text;
        roleName = ((Label)gvrow.FindControl("lblRoleName")).Text;
        moduleName = ((Label)gvrow.FindControl("lblModuleName")).Text;
        objectName = ((Label)gvrow.FindControl("lblObjectName")).Text;
        operation = ((Label)gvrow.FindControl("lblOperation")).Text;

        hdnUserName.Value = userName;
        hdnModuleName.Value = moduleName;
        hdnObjectName.Value = objectName;

        if (operation != "")
        {
            foreach (string str in operation.Split(','))
            {
                foreach (ListItem li in chkPermissionList.Items)
                {
                    if (li.Text == str)
                    {
                        li.Selected = true;
                        break;
                    }
                }
            }
        }
    }
    public void fillPermissionList()
    {
        DataTable dt = objDa.return_DataTable("select * from Set_OperationType");
        chkPermissionList.DataSource = dt;
        chkPermissionList.DataTextField = "op_type";
        chkPermissionList.DataValueField = "op_id";
        chkPermissionList.DataBind();

    }

    protected void btnSavePermission_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trans = con.BeginTransaction();

        try
        {
            string transId = "";
            transId = objDa.get_SingleValue("select Set_UserPermission.transid from Set_UserPermission inner join Set_UserMaster on Set_UserMaster.User_Id= Set_UserPermission.User_Id inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id inner join IT_ModuleMaster on IT_ModuleMaster.Module_Id = Set_UserPermission.Module_Id inner join IT_ObjectEntry on IT_ObjectEntry.Object_Id = Set_UserPermission.Object_Id where IT_ModuleMaster.Module_Name ='" + hdnModuleName.Value + "' and IT_ObjectEntry.Object_Name='" + hdnObjectName.Value + "' and Set_EmployeeMaster.Emp_Name='" + hdnUserName.Value + "'", ref trans);
            transId = transId == "@NOTFOUND@" ? "" : transId;
            if (transId == "")
            {
                trans.Rollback();
                trans.Dispose();
                con.Close();
                return;
            }
            objDa.execute_Command("delete from Set_UserOpPermission where User_Record_Id='" + transId + "'", ref trans);
            foreach (ListItem li in chkPermissionList.Items)
            {
                if (li.Selected)
                {
                    objDa.execute_Command("insert into Set_UserOpPermission values('" + transId + "','" + li.Value + "')", ref trans);
                }
            }
            trans.Commit();
            trans.Dispose();
            con.Close();
            btngo_Click(null, null);
        }
        catch (Exception er)
        {
            trans.Rollback();
            trans.Dispose();
            con.Close();
        }

    }

    protected void btnRoleDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;

        string userName = "", moduleName = "", objectName = "", transId = "";
        userName = ((Label)gvrow.FindControl("lbluserName")).Text;
        moduleName = ((Label)gvrow.FindControl("lblModuleName")).Text;
        objectName = ((Label)gvrow.FindControl("lblObjectName")).Text;

        transId = objDa.get_SingleValue("select Set_UserPermission.transid from Set_UserPermission inner join Set_UserMaster on Set_UserMaster.User_Id= Set_UserPermission.User_Id inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id inner join IT_ModuleMaster on IT_ModuleMaster.Module_Id = Set_UserPermission.Module_Id inner join IT_ObjectEntry on IT_ObjectEntry.Object_Id = Set_UserPermission.Object_Id where IT_ModuleMaster.Module_Name ='" + moduleName + "' and IT_ObjectEntry.Object_Name='" + objectName + "' and Set_EmployeeMaster.Emp_Name='" + userName + "'");
        transId = transId == "@NOTFOUND@" ? "" : transId;
        objDa.execute_Command("delete from Set_UserOpPermission where User_Record_Id='" + transId + "'");
        btngo_Click(null, null);
        DisplayMessage("Role Deleted");
    }

    protected void btnEmailConfiguration_Click(object sender, EventArgs e)
    {
        if (hdnempid.Value == "0")
        {
            DisplayMessage("user not found");
            return;
        }

        Email_Config.setUserID(hdnempid.Value, txtUserName.Text.Trim(), false);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "show_modal()", true);
        //string url = "../Attendance/LogProcess.aspx";
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }
}