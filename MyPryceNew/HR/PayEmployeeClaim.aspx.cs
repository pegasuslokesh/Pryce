using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;

public partial class HR_PayEmployeeClaim : BasePage
{
    #region defind Class Object

    Set_Approval_Employee objEmpApproval = null;
    Common ObjComman = null;
    Pay_Employee_Month objPayEmpMonth = null;
    Pay_Employee_claim ObjClaim = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Set_ApplicationParameter objAppParam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    NotificationMaster Obj_Notifiacation = null;
    PageControlCommon objPageCmn = null;
    #endregion
    string Click_Event = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());

        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        ObjClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/PayEmployeeClaim.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }

            DdlMonthList.SelectedValue = DateTime.Now.Month.ToString();
            TxtYearList.Text = DateTime.Now.Year.ToString();

            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 9;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            Session["dtClaim"] = null;
            FillddlLocation();
            FillddlDeaprtment();
            TxtYear.Text = DateTime.Now.Year.ToString();
            int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
            ddlMonth.SelectedValue = (CurrentMonth).ToString();


            Session["empimgpath"] = null;

            Page.Form.Attributes.Add("enctype", "multipart/form-data");


            bool IsCompOT = false;
            bool IsPartialComp = false;
            try
            {
                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Claim_Enable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }

            btnClaim_Click(null, null);

            lblWrongSequence.Text = "";
            lblDojIssue.Text = "";
            //this code is created by jitendra upadhyay on 17-07-2014
            //this code for set the gridview page size according the system parameter
            try
            {
                gvEmployee.PageSize = int.Parse(Session["GridSize"].ToString());
                gvEmployeeClaim.PageSize = int.Parse(Session["GridSize"].ToString());
                GridViewClaimList.PageSize = int.Parse(Session["GridSize"].ToString());
            }
            catch
            {

            }
            ddlLocation.Focus();
            ValidEmpList();
            btnLeast_Click(null, null);
        }
        Page.Title = objSys.GetSysTitle();

     
    }

    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
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
            //Common Function add By Lokesh on 23-05-2015
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
    void ValidEmpList()
    {
        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation,dpDepartment, true, Session["DBConnection"].ToString());

        
        string EmpLIst = string.Empty;
        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            if (EmpLIst.Trim() == "")
            {
                EmpLIst = dtEmp.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                EmpLIst = EmpLIst + "," + dtEmp.Rows[i]["Emp_Id"].ToString();
            }

        }
        Session["dtEmpList"] = EmpLIst;
    }

    private void FillddlDeaprtment()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dtDepartment = PageControlCommon.GetEmployeeDepartmentByLocationId(ddlLocation, dpDepartment, Session["DBConnection"].ToString());
        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
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
    public void FillGrid()
    {
        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());

        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpClaim"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmployeeClaim, dtEmp, "", "");
            lblTotalRecordsClaim.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        else
        {
            gvEmployeeClaim.DataSource = null;
            gvEmployeeClaim.DataBind();
            Session["dtEmpClaim"] = dtEmp;
            lblTotalRecordsClaim.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
    
    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillddlDeaprtment();
        FillGrid();
        Session["dtClaim"] = null;
     
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

    protected void lbxGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroup.Items.Count; i++)
        {
            if (lbxGroup.Items[i].Selected == true)
            {
                GroupIds += lbxGroup.Items[i].Value + ",";
            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

          

            DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

            dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            {
                if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                {
                    EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                }
            }
            dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp1"] = dtEmp;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            }
            else
            {
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();
            }
        }
        else
        {
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmployeeClaim.Rows)
            {
                int index = (int)gvEmployeeClaim.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvEmployeeClaim.Rows)
        {
            index = (int)gvEmployeeClaim.DataKeys[gvrow.RowIndex].Value;
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
    protected void gvEmployeeClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        SaveCheckedValues();
        gvEmployeeClaim.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmpClaim"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployeeClaim, dt, "", "");
        PopulateCheckedValues();
      
    }
    protected void chkgvSelectAll_CheckedChangedClaim(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");


        CheckBox chkSelAll = ((CheckBox)gvEmployeeClaim.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvEmployeeClaim.Rows)
        {
            index = (int)gvEmployeeClaim.DataKeys[gvrow.RowIndex].Value;
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


    protected void ImgbtnSelectAll_ClickClaim(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ArrayList userdetails = new ArrayList();
        DataTable dtClaim = (DataTable)Session["dtEmpClaim"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtClaim.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvEmployeeClaim.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpClaim"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmployeeClaim, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";


        FillGrid();
        Session["dtClaim"] = null;
     
        txtValue1.Focus();

    }
    protected void btnClaimRefreshSearchPanel_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ddlField1SearchPanel.SelectedIndex = 1;
        ddlOption1SearchPanel.SelectedIndex = 2;
        txtValue1SearchPanel.Text = "";
        GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);

        Session["dtClaim"] = null;
    
        txtValue1SearchPanel.Focus();

    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (Session["dtEmpClaim"] != null)
        {
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
                DataTable dtEmp = (DataTable)Session["dtEmpClaim"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmployeeClaim, view.ToTable(), "", "");
                Session["dtEmpClaim"] = view.ToTable();
                lblTotalRecordsClaim.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            }
        }
        Session["dtClaim"] = null;
     
        txtValue1.Focus();
    }
    protected void btnClaimbindSearchPanel_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (Session["dtFilter_Pay_Emp_Claim"] != null)
        {
            if (ddlOption1.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption1SearchPanel.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlField1SearchPanel.SelectedValue + ",System.String)='" + txtValue1SearchPanel.Text.Trim() + "'";
                }
                else if (ddlOption1SearchPanel.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlField1SearchPanel.SelectedValue + ",System.String) like '%" + txtValue1SearchPanel.Text.Trim() + "%'";
                }
                else
                {
                    condition = "convert(" + ddlField1SearchPanel.SelectedValue + ",System.String) Like '" + txtValue1SearchPanel.Text.Trim() + "%'";

                }
                DataTable dtEmp = (DataTable)Session["dtFilter_Pay_Emp_Claim"];
                DataView view = new DataView();
                try
                {
                    view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                }
                catch
                {

                }
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GridViewClaimList, view.ToTable(), "", "");
                Session["dtFilter_Pay_Emp_Claim"] = view.ToTable();
                lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            }
        }
        else
        {
            GridViewClaimList.DataSource = null;
            GridViewClaimList.DataBind();
            lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " :0";

        }
        Session["dtClaim"] = null;
        
        txtValue1SearchPanel.Focus();

    }
    protected void btnClaim_Click(object sender, EventArgs e)
    {

        pnlClaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        

        ////PnlEmployeeClaim.Visible = true;

        ////PanelSearchList.Visible = false;

        Session["CHECKED_ITEMS"] = null;
        gvEmployeeClaim.Visible = true;
        Session["dtClaim"] = null;
        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        EmpGroup_CheckedChanged(null, null);
        TxtClaimName.Focus();
        
    }
    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp1"], "", "");
    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroup.Checked)
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
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
            }
            lbxGroup_SelectedIndexChanged(null, null);
        }
        else if (rbtnEmp.Checked)
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;

            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            FillGrid();
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {

        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
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
    void Reset()
    {
        TxtClaimName.Text = "";
        TxtClaimDiscription.Text = "";
        ddlMonth.SelectedIndex = 0;
        txtCalValue.Text = "";
        DdlValueType.SelectedIndex = 0;
        txtValue1.Text = "";
        TxtYear.Text = "";
        TxtClaimName.Focus();
        ViewState["Select"] = null;
        TxtYear.Text = DateTime.Now.Year.ToString();
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());

        ddlMonth.SelectedValue = (CurrentMonth).ToString();
        foreach (GridViewRow Gvrow in gvEmployeeClaim.Rows)
        {
            CheckBox ChkHeader = (CheckBox)gvEmployeeClaim.HeaderRow.FindControl("chkgvSelectAll");
            CheckBox ChkItem = (CheckBox)Gvrow.FindControl("chkgvSelect");

            ChkHeader.Checked = false;
            ChkItem.Checked = false;
        }
        //btnClaim_Click(null, null);
    }
    protected void btnSaveClaim_Click(object sender, EventArgs e)
    {
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";

        int b = 0;

        string TransNo = string.Empty;


        if (rbtnGroup.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;

            for (int i = 0; i < lbxGroup.Items.Count; i++)
            {
                if (lbxGroup.Items[i].Selected)
                {
                    GroupIds += lbxGroup.Items[i].Value + ",";
                }
            }

            if (GroupIds == "")
            {
                DisplayMessage("Select Group First");
                lbxGroup.Focus();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                return;
            }
            if (TxtClaimName.Text.Trim() == "")
            {
                DisplayMessage("Enter Claim Name");
                TxtClaimName.Focus();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                return;
            }
            if (DdlValueType.SelectedIndex == 0)
            {
                DisplayMessage("Select Value Type");
                DdlValueType.Focus();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                return;
            }
            if (txtCalValue.Text.Trim() == "")
            {
                DisplayMessage("Enter Claim Value");
                txtCalValue.Focus();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                return;
            }
            if (ddlMonth.SelectedIndex == 0)
            {
                DisplayMessage("Select Month");
                ddlMonth.Focus();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                return;
            }
            if (TxtYear.Text.Trim() == "")
            {
                DisplayMessage("Enter Year");
                TxtYear.Focus();
                btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
                return;
            }

            DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

            dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            {
                if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                {

                    DataTable dtEmp = objEmp.GetEmployee_InPayroll(Session["CompId"].ToString());

                  
                    dtEmp = new DataView(dtEmp, "Emp_Id='" + dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtEmp.Rows.Count > 0)
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
            }



            EmpIds = ValidPayRoll(EmpIds, "Insert");
            if (lblWrongSequence.Text != "")
            {

                DisplayMessage(lblWrongSequence.Text);

            }
            if (lblDojIssue.Text != "")
            {
                DisplayMessage(lblDojIssue.Text);

            }
            foreach (string str in EmpIds.Split(','))
            {
                if (str != "")
                {

                    DataTable dtempmonth = new DataTable();
                    dtempmonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, ddlMonth.SelectedIndex.ToString(), TxtYear.Text.ToString(),Session["CompId"].ToString());
                    if (dtempmonth.Rows.Count > 0)
                    {
                        DisplayMessage("Payroll Posted For This Month and Year");
                        return;

                    }
                    else
                    {

                        DataTable dt = new DataTable();

                        string EmpPermission = string.Empty;
                        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Claim").Rows[0]["Approval_Level"].ToString();

                        dt = objEmpApproval.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "71", str);
                        if (dt.Rows.Count == 0)
                        {
                            DisplayMessage("Approval setup issue , please contact to your admin");
                            return;
                        }
                        b = ObjClaim.Insert_In_Pay_Employee_Claim(Session["CompId"].ToString(), str, TxtClaimName.Text.Trim(), TxtClaimDiscription.Text, DdlValueType.SelectedValue, txtCalValue.Text, DateTime.Now.ToString(), "Pending", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "71", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        DateTime Date = new DateTime(int.Parse(TxtYear.Text), int.Parse(ddlMonth.SelectedValue), 1, 0, 0, 0);

                        //objEmpApproval.InsertApprovalChildMaster("Claim",b.ToString(),"71",str,Date.ToString());

                    
                        if (dt.Rows.Count > 0)
                        {

                            for (int j = 0; j < dt.Rows.Count; j++)
                            {

                                string PriorityEmpId = dt.Rows[j]["Emp_Id"].ToString();
                                string IsPriority = dt.Rows[j]["Priority"].ToString();
                                int cur_trans_id = 0;
                                foreach (string a in EmpIds.Split(','))
                                {
                                    if (a != "")
                                        Session["Req_Emp_ID"] = a;
                                    if (EmpPermission == "1")
                                    {
                                        cur_trans_id=objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), "0", "0", "0", a.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                    }

                                    else if (EmpPermission == "2")
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                    }
                                    else if (EmpPermission == "3")
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                    }
                                    else
                                    {
                                        cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["User_Id"].ToString(), System.DateTime.Now.ToString(), Session["User_Id"].ToString(), System.DateTime.Now.ToString());
                                    }

                                    if (a != "")
                                    {
                                        // Insert Notification For Leave by  ghanshyam suthar
                                        Session["PriorityEmpId"] = PriorityEmpId;
                                        Session["cur_trans_id"] = cur_trans_id;
                                        Set_Notification();
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


            SaveCheckedValues();
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS"] != null)
            {

                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select Employee First");
                    return;
                }

            }
            else
            {
                DisplayMessage("Select Employee First");
                return;
            }


            if (TxtClaimName.Text.Trim() == "")
            {
                DisplayMessage("Enter Claim Name");
                TxtClaimName.Focus();
                return;
            }
            if (DdlValueType.SelectedIndex == 0)
            {
                DisplayMessage("Select Value Type");
                DdlValueType.Focus();
                return;

            }
            if (txtCalValue.Text.Trim() == "")
            {
                DisplayMessage("Enter Claim Value");
                txtCalValue.Focus();
                return;
            }
            if (ddlMonth.SelectedIndex == 0)
            {
                DisplayMessage("Select Month");
                ddlMonth.Focus();
                return;
            }
            if (TxtYear.Text.Trim() == "")
            {
                DisplayMessage("Enter Year");
                TxtYear.Focus();
                return;
            }
            lblSelectRecd.Text = "";

            for (int i = 0; i < userdetails.Count; i++)
            {
                lblSelectRecd.Text += userdetails[i].ToString() + ",";
            }


            string EmpIds = ValidPayRoll(lblSelectRecd.Text, "Insert");
            if (lblWrongSequence.Text != "")
            {

                DisplayMessage(lblWrongSequence.Text);

            }
            if (lblDojIssue.Text != "")
            {
                DisplayMessage(lblDojIssue.Text);

            }

            foreach (string str in EmpIds.Split(','))
            {
                if (str != "")
                {
                    Session["Req_Emp_ID"] = str;
                    DataTable dtempmonth = new DataTable();
                    dtempmonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, ddlMonth.SelectedIndex.ToString(), TxtYear.Text.ToString(),Session["CompId"].ToString());
                    if (dtempmonth.Rows.Count > 0)
                    {
                        DisplayMessage("Payroll Posted For This Month and Year");
                        return;

                    }
                    else
                    {
                        DataTable dt = new DataTable();

                        string EmpPermission = string.Empty;
                        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Claim").Rows[0]["Approval_Level"].ToString();

                        dt = objEmpApproval.getApprovalChainByObjectid(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),"71", str.ToString());




                        if (dt.Rows.Count == 0)
                        {
                            DisplayMessage("Approval setup issue , please contact to your admin");
                            return;
                        }




                        b = ObjClaim.Insert_In_Pay_Employee_Claim(Session["CompId"].ToString(), str, TxtClaimName.Text.Trim(), TxtClaimDiscription.Text, DdlValueType.SelectedValue, txtCalValue.Text, DateTime.Now.ToString(), "Pending", DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "71", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        DateTime Date = new DateTime(int.Parse(TxtYear.Text), int.Parse(ddlMonth.SelectedValue), 1, 0, 0, 0);

                        // objEmpApproval.InsertApprovalChildMaster("Claim", b.ToString(), "71",str,Date.ToString());


                        if (dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt.Rows.Count; j++)
                            {
                                int cur_trans_id = 0;
                                string PriorityEmpId = dt.Rows[j]["Emp_Id"].ToString();
                                string IsPriority = dt.Rows[j]["Priority"].ToString();
                                if (EmpPermission == "1")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), "0", "0", "0", str.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                }
                                else if (EmpPermission == "2")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", str.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                }
                                else if (EmpPermission == "3")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", str.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                }
                                else
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("4", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", str.ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", TxtClaimDiscription.Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                                }

                                // Insert Notification For Leave by  ghanshyam suthar
                                Session["PriorityEmpId"] = PriorityEmpId;
                                Session["cur_trans_id"] = cur_trans_id;
                                Set_Notification();
                            }
                        }
                    }
                }
            }
        }
        if (b != 0)
        {
            DisplayMessage("Record Saved","green");
            rbtnEmp.Checked = true;
            rbtnGroup.Checked = false;

            EmpGroup_CheckedChanged(null, null);
            Reset();
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = null;

        }
        else
        {
            DisplayMessage("Record Not Saved");
        }
        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
    }
    
    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/hr"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["Req_Emp_ID"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        GetEmployeeCode(Session["Req_Emp_ID"].ToString());
        Message = Session["Req_Emp_Name"].ToString() + " applied Claim Request for " + TxtClaimName.Text + " for " + ddlMonth.SelectedItem.ToString() + " " + TxtYear.Text + "";
        if (HiddeniD.Value == "")
        {
            // For Insert        
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["Req_Emp_ID"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "0");
        }
        else
        {
            // For Update
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["Req_Emp_ID"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), HiddeniD.Value, "14");
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
            if (dt.Rows[0]["Emp_Name"].ToString() != "")
                Session["Req_Emp_Name"] = dt.Rows[0]["Emp_Name"].ToString();
            else
                Session["Req_Emp_Name"] = dt.Rows[0]["Emp_Name_L"].ToString();

            if (empname == "")
            {
                empname = "No Code";
            }
            ViewState["Emp_Img"] = "../CompanyResource/2/" + dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            empname = "No Code";
            ViewState["Emp_Img"] = "";
        }

        return empname;



    }
    private string ValidPayRoll(string StrAllEmpId, string Mode)
    {
        DataAccessClass objDA = new DataAccessClass(Session["DBConnection"].ToString());
        string strMessage = string.Empty;
        string strEmpId = string.Empty;


        foreach (string str in StrAllEmpId.Split(','))
        {
            if ((str != ""))
            {


                DataTable dtPayPostedInfo = objDA.return_DataTable("select * from Pay_Employe_Month  where Emp_Id = '" + str + "' and Year = (Select MAX(Year) From Pay_Employe_Month  where Emp_Id = '" + str + "') order by MONTH desc");

                if (dtPayPostedInfo.Rows.Count > 0)
                {
                    DateTime tTemp = new DateTime(Convert.ToInt16(dtPayPostedInfo.Rows[0]["Year"].ToString()), Convert.ToInt16(dtPayPostedInfo.Rows[0]["Month"].ToString()), 1);
                    DateTime tCurrent = new DateTime();
                    if (Mode == "Insert")
                    {
                        tCurrent = new DateTime(Convert.ToInt16(TxtYear.Text), Convert.ToInt16(ddlMonth.SelectedValue), 1);
                    }
                    else
                    {
                        tCurrent = new DateTime(Convert.ToInt16(TxtpanelYearList.Text), Convert.ToInt16(DdlMonthListPanel.SelectedValue), 1);

                    }



                    if (tTemp < tCurrent)
                    {
                        if (Mode == "Insert")
                        {
                            strEmpId += str + ",";
                        }
                        else
                        {
                            strEmpId += str;
                        }
                    }
                    else
                    {
                        if (Mode == "Insert")
                        {

                            if (lblWrongSequence.Text == "")
                            {
                                if (Session["Lang"].ToString() == "1")
                                {

                                    lblWrongSequence.Text = "Claim Month and Year Should be Greater than Last Posted Month And Year For Employee Code=" + GetEmployeeCode(str);
                                }
                                else
                                {
                                    lblWrongSequence.Text = GetArebicMessage("Claim Month and Year Should be Greater than Last Posted Month And Year For Employee Code") + "=" + GetEmployeeCode(str);

                                }
                            }
                            else
                            {
                                lblWrongSequence.Text = lblWrongSequence.Text + "," + GetEmployeeCode(str);
                            }
                        }
                        else
                        {
                            DisplayMessage("Claim Month and Year Should be Greater than Last Posted Month And Year");
                        }




                    }


                }
                else
                {
                    string CurrentMonth = string.Empty;
                    string CurrentYear = string.Empty;

                    if (Mode == "Insert")
                    {
                        CurrentMonth = ddlMonth.SelectedValue;
                        CurrentYear = TxtYear.Text;

                    }
                    else
                    {
                        CurrentMonth = DdlMonthListPanel.SelectedValue;
                        CurrentYear = TxtpanelYearList.Text;
                    }
                    DateTime tEDOJ = Convert.ToDateTime(objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), str).Rows[0]["DOJ"].ToString());



                    if (tEDOJ.Year <= Convert.ToInt16(CurrentYear))
                    {
                        if (tEDOJ.Year < Convert.ToInt16(CurrentYear))
                        {
                            if (Mode == "Insert")
                            {
                                strEmpId += str + ",";
                            }
                            else
                            {
                                strEmpId += str;
                            }
                        }
                        else
                        {
                            if (tEDOJ.Month <= Convert.ToInt16(CurrentMonth))
                            {
                                if (Mode == "Insert")
                                {
                                    strEmpId += str + ",";
                                }
                                else
                                {
                                    strEmpId += str;
                                }
                            }
                            else
                            {
                                if (Mode == "Insert")
                                {
                                    if (lblDojIssue.Text == "")
                                    {

                                        if (Session["Lang"].ToString() == "1")
                                        {
                                            lblDojIssue.Text = "Claim Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code=" + GetEmployeeCode(str);
                                        }
                                        else
                                        {
                                            lblDojIssue.Text = GetArebicMessage("Claim Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code") + "=" + GetEmployeeCode(str);

                                        }
                                    }
                                    else
                                    {
                                        lblDojIssue.Text = lblDojIssue.Text + "," + GetEmployeeCode(str);
                                    }
                                }
                                else
                                {
                                    DisplayMessage("Claim Month and Year Should be Greater than or Equal to Date Of Joining");
                                }


                            }
                        }

                    }
                    else
                    {
                        if (Mode == "Insert")
                        {
                            if (lblDojIssue.Text == "")
                            {

                                if (Session["Lang"].ToString() == "1")
                                {
                                    lblDojIssue.Text = "Claim Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code=" + GetEmployeeCode(str);
                                }
                                else
                                {
                                    lblDojIssue.Text = GetArebicMessage("Claim Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code") + "=" + GetEmployeeCode(str);

                                }
                            }
                            else
                            {
                                lblDojIssue.Text = lblDojIssue.Text + "," + GetEmployeeCode(str);
                            }
                        }
                        else
                        {
                            DisplayMessage("Claim Month and Year Should be Greater than or Equal to Date Of Joining");
                        }

                    }





                }




            }
        }



        return strEmpId;


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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        TxtClaimName.Text = "";
        TxtClaimDiscription.Text = "";
        ddlMonth.SelectedIndex = 0;
        txtCalValue.Text = "";
        DdlValueType.SelectedIndex = 0;
        txtValue1.Text = "";
        TxtYear.Text = "";
        TxtClaimName.Focus();
        TxtYear.Text = DateTime.Now.Year.ToString();
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());

        ddlMonth.SelectedValue = (CurrentMonth).ToString();
        foreach (GridViewRow Gvrow in gvEmployeeClaim.Rows)
        {
            CheckBox ChkHeader = (CheckBox)gvEmployeeClaim.HeaderRow.FindControl("chkgvSelectAll");
            CheckBox ChkItem = (CheckBox)Gvrow.FindControl("chkgvSelect");

            ChkHeader.Checked = false;
            ChkItem.Checked = false;


        }
        ddlLocation.SelectedIndex = 0;
        dpDepartment.SelectedIndex = 0;
        btnClaim_Click(null, null);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        TxtClaimName.Text = "";
        TxtClaimDiscription.Text = "";
        ddlMonth.SelectedIndex = 0;
        txtCalValue.Text = "";
        DdlValueType.SelectedIndex = 0;
        txtValue1.Text = "";
        TxtYear.Text = "";
        TxtClaimName.Focus();
        TxtYear.Text = DateTime.Now.Year.ToString();
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());

        ddlMonth.SelectedValue = (CurrentMonth).ToString();
        foreach (GridViewRow Gvrow in gvEmployeeClaim.Rows)
        {
            CheckBox ChkHeader = (CheckBox)gvEmployeeClaim.HeaderRow.FindControl("chkgvSelectAll");
            CheckBox ChkItem = (CheckBox)Gvrow.FindControl("chkgvSelect");

            ChkHeader.Checked = false;
            ChkItem.Checked = false;


        }
        ddlLocation.SelectedIndex = 0;
        dpDepartment.SelectedIndex = 0;
        btnClaim_Click(null, null);
    }
    void GridBind_ClaimList(string Month, string Year)
    {
        DataTable Dt = new DataTable();
        Dt = ObjClaim.GetRecord_From_PayEmployeeClaim(Session["CompId"].ToString(), "0", "0", Month, Year, "Pending", "", "");


        try
        {
            if (Session["dtEmpList"] != null)
            {
                Dt = new DataView(Dt, "Emp_Id in (" + Session["dtEmpList"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();

            }

        }
        catch
        {
        }
        if (Dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewClaimList, Dt, "", "");
            Session["dtFilter_Pay_Emp_Claim"] = Dt;
            lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " : " + Dt.Rows.Count.ToString();
        }
        else
        {
            Dt.Clear();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewClaimList, Dt, "", "");
            if (Click_Event == "")
                DisplayMessage("Record Not found");
            DdlMonthList.Focus();
            Session["dtFilter_Pay_Emp_Claim"] = null;
            lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " :0";
        }
        
    }
    protected void btnLeast_Click(object sender, EventArgs e)
    {
        Click_Event = "True";
        Edit_Claim.Visible = false;

        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlClaim.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        DdlMonthList.Focus();
        //PanelSearchList.Visible = true;
        //PnlEmployeeClaim.Visible = false;
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month);
        DdlMonthList.SelectedValue = (CurrentMonth).ToString();
        TxtYearList.Text = DateTime.Now.Year.ToString();
        GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);
        
    }
    protected void BtnBindList_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (DdlMonthList.SelectedIndex == 0)
        {
            DisplayMessage("Select Month");
            ddlMonth.Focus();
            return;
        }
        if (TxtYearList.Text == "")
        {
            DisplayMessage("Enter Year");
            TxtYearList.Focus();
            return;
        }
        GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);
        BtnRefreshList.Focus();
        ddlField1SearchPanel.SelectedIndex = 1;
        ddlOption1SearchPanel.SelectedIndex = 2;
        txtValue1SearchPanel.Text = "";
     
    }
    protected void BtnRefreshList_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        ddlField1SearchPanel.SelectedIndex = 1;
        ddlOption1SearchPanel.SelectedIndex = 2;
        txtValue1SearchPanel.Text = "";
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month);
        DdlMonthList.SelectedValue = (CurrentMonth).ToString();
        TxtYearList.Text = DateTime.Now.Year.ToString();
        GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);
        DdlMonthList.Focus();
       

    }
    protected void btnEdit_command(object sender, CommandEventArgs e)
    {

        DataTable Dt = new DataTable();
        Dt = ObjClaim.GetRecord_From_PayEmployeeClaim_usingClaimId_And_EmployeeId(Session["CompId"].ToString(), "0", e.CommandArgument.ToString(), "0", "0", "", "", "");
        if (Dt.Rows.Count > 0)
        {

            DataTable dtApproved = objEmpApproval.GetApprovalChild("0", "4");

            try
            {
                dtApproved = new DataView(dtApproved, "Approval_Type='Claim'   and Ref_id=" + e.CommandArgument.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }


            bool IsAllow = true;
            foreach (DataRow dr in dtApproved.Rows)
            {
                if (dr["Status"].ToString() != "Pending")
                {
                    IsAllow = false;

                }
            }
            if (!IsAllow)
            {

                DisplayMessage("You Can not Edit ,Used in Approval");
                return;

            }


            DataTable dtempmonth = new DataTable();
            dtempmonth = objPayEmpMonth.GetAllRecordPostedByCompanyId(Session["compId"].ToString());
            try
            {

                dtempmonth = new DataView(dtempmonth, "Month=" + DdlMonthList.SelectedValue.ToString() + " and year=" + TxtYearList.Text + " and Emp_Id=" + Dt.Rows[0]["Emp_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
            if (dtempmonth.Rows.Count > 0)
            {
                DisplayMessage("Payroll Posted For This Month and Year");


                return;

            }

            Edit_Claim.Visible = true;
            HiddeniD.Value = e.CommandArgument.ToString();

            TxtClaimNameList.Text = Dt.Rows[0]["Claim_Name"].ToString();
            TxtClaimDiscList.Text = Dt.Rows[0]["Claim_Description"].ToString();
            DdlvalueTypelist.SelectedValue = Dt.Rows[0]["value_Type"].ToString();
            Txtvaluelist.Text = Dt.Rows[0]["Value"].ToString();
            DdlMonthListPanel.SelectedValue = Dt.Rows[0]["Claim_Month"].ToString();
            TxtpanelYearList.Text = Dt.Rows[0]["Claim_Year"].ToString();
            txtEmployeeId.Text = Dt.Rows[0]["Emp_Id"].ToString();
            TxtEmployeeName.Text = Dt.Rows[0]["Emp_Name"].ToString();


            TxtClaimNameList.Focus();
        }

    }
    protected void IbtnDelete_command(object sender, CommandEventArgs e)
    {
        DataTable Dt = new DataTable();
        DataTable dtEmpTrans = new DataTable();
        int Approval_ID = 0;
        Dt = ObjClaim.GetRecord_From_PayEmployeeClaim_usingClaimId_And_EmployeeId(Session["CompId"].ToString(), "0", e.CommandArgument.ToString(), "0", "0", "", "", "");
        if (Dt.Rows.Count > 0)
        {

            DataTable dtApproved = objEmpApproval.GetApprovalChild("0", "4");

            try
            {
                dtApproved = new DataView(dtApproved, "Approval_Type='Claim'   and Ref_id=" + e.CommandArgument.ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                Approval_ID = Convert.ToInt32(dtApproved.Rows[0]["Approval_Id"].ToString());
            }
            catch
            {

            }
            bool IsAllow = true;
            foreach (DataRow dr in dtApproved.Rows)
            {
                if (dr["Status"].ToString() != "Pending")
                {
                    IsAllow = false;

                }
            }
            if (!IsAllow)
            {

                DisplayMessage("You Can not Delete,Used in Approval");
                return;

            }



            DataTable dtempmonth = new DataTable();
            dtempmonth = objPayEmpMonth.GetAllRecordPostedByCompanyId(Session["compId"].ToString());
            try
            {

                dtempmonth = new DataView(dtempmonth, "Month=" + DdlMonthList.SelectedValue.ToString() + " and year=" + TxtYearList.Text + " and Emp_Id=" + Dt.Rows[0]["Emp_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
            if (dtempmonth.Rows.Count > 0)
            {
                DisplayMessage("Payroll Posted For This Month and Year");


                return;

            }
        }








        HiddeniD.Value = e.CommandArgument.ToString();
        int CheckDeletion = 0;
        CheckDeletion = ObjClaim.DeleteRecord_in_Pay_Employee_Claim(Session["CompId"].ToString(), HiddeniD.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (CheckDeletion != 0)
        {
            DisplayMessage("Record Deleted");
            // objEmpApproval.DeleteApprovalTransaciton("Claim", HiddeniD.Value, Dt.Rows[0]["Field3"].ToString(), Approval_ID.ToString());        
            //Change by ghanshyam suthar on 20-11-2017
            objEmpApproval.DeleteEmpLoanApproval(Approval_ID.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Dt.Rows[0]["Field3"].ToString(), HiddeniD.Value);
            //---------------End----------------
            DataTable dtGrid = new DataTable();
            ddlField1SearchPanel.SelectedIndex = 1;
            ddlOption1SearchPanel.SelectedIndex = 2;
            txtValue1SearchPanel.Text = "";
            GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
  
    }
    void ResetPanel()
    {
        TxtClaimNameList.Text = "";
        TxtClaimDiscList.Text = "";
        DdlvalueTypelist.SelectedIndex = 0;
        Txtvaluelist.Text = "";
        DdlMonthListPanel.SelectedIndex = 0;
        TxtpanelYearList.Text = "";
        HiddeniD.Value = "";
        RbtnApproved.Checked = false;
        RbtnCancelled.Checked = false;
        Edit_Claim.Visible = false;
        txtEmployeeId.Text = "";
        TxtEmployeeName.Text = "";
      


    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {

        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";

        if (HiddeniD.Value == "")
        {
            DisplayMessage("Edit The Record");
            DdlMonthList.Focus();
            return;
        }



        if (TxtClaimNameList.Text.Trim() == "")
        {
            DisplayMessage("Enter Claim Name");
            TxtClaimNameList.Focus();
            return;
        }

        if (DdlvalueTypelist.SelectedIndex == 0)
        {
            DisplayMessage("Select Value Type");
            DdlvalueTypelist.Focus();
            return;
        }

        if (Txtvaluelist.Text.Trim() == "")
        {
            DisplayMessage("Enter Claim Value");
            Txtvaluelist.Focus();
            return;
        }
        if (DdlMonthListPanel.SelectedIndex == 0)
        {
            DisplayMessage("Select Month");
            DdlMonthListPanel.Focus();
            return;

        }
        if (TxtpanelYearList.Text == "")
        {
            DisplayMessage("Enter Year");
            TxtpanelYearList.Focus();
            return;

        }


        int count = 0;
        string Status = "";


        string EmployeeId = ValidPayRoll(txtEmployeeId.Text, "Update");
        if (EmployeeId != "")
        {


            int UpdationCheck = 0;



            UpdationCheck = ObjClaim.UpdateRecord_In_Pay_Employee_Claim(Session["CompId"].ToString(), HiddeniD.Value, TxtClaimNameList.Text.Trim(), TxtClaimDiscList.Text, DdlvalueTypelist.SelectedValue, Txtvaluelist.Text, DdlMonthListPanel.SelectedValue, TxtpanelYearList.Text, "Pending", Session["UserId"].ToString(), DateTime.Now.ToString());

            // Insert Notification For Leave by  ghanshyam suthar
            //Session["PriorityEmpId"] = PriorityEmpId;
            //Session["cur_trans_id"] = UpdationCheck;
            //Set_Notification();

            //objEmpApproval.InsertApprovalChildMaster("Claim", HiddeniD.Value, "71", txtEmployeeId.Text);

            if (UpdationCheck != 0)
            {
                DisplayMessage("Record Updated", "green");
                GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);
                Edit_Claim.Visible = false;
                ResetPanel();
                DdlMonthList.Focus();

             
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
       
    }

    protected void GridViewClaimList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewClaimList.PageIndex = e.NewPageIndex;
        GridBind_ClaimList(DdlMonthList.SelectedValue, TxtYearList.Text);
       
        GridViewClaimList.HeaderRow.Focus();
    }


    protected void btnClaim_Click1(object sender, EventArgs e)
    { }
    protected void BtnResetClaimList_Click(object sender, EventArgs e)
    {
        ResetPanel();
        DdlMonthList.Focus();
    }
    public string GetType(string Type)
    {
        if (Type == "1")
        {
            Type = "Fixed";
        }
        else if (Type == "2")
        {
            Type = "Percentage";

        }
        return Type;
    }
    protected void GridViewClaimList_Sorting(object sender, GridViewSortEventArgs e)
    {
        HdfSort.Value = HdfSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pay_Emp_Claim"];
        DataView dv = new DataView(dt);

        string Query = "" + e.SortExpression + " " + HdfSort.Value + "";

        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pay_Emp_Claim"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GridViewClaimList, dt, "", "");
 
        try
        {
            GridViewClaimList.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    protected void gvEmployeeClaim_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdFSortgvEmpClaim.Value = hdFSortgvEmpClaim.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpClaim"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdFSortgvEmpClaim.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployeeClaim, dt, "", "");
   
        try
        {
            if (e.SortExpression == "Emp_Code")
            {
                gvEmployeeClaim.HeaderRow.Cells[1].Focus();
            }
            if (e.SortExpression == "Emp_Name")
            {
                gvEmployeeClaim.HeaderRow.Cells[2].Focus();
            }
            if (e.SortExpression == "Doj")
            {
                gvEmployeeClaim.HeaderRow.Cells[3].Focus();
            }
        }
        catch
        {

        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetClaimName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Pay_Employee_claim.GetClaimName("", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        dt = new DataView(dt, "Claim_Name lIKE '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();


        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i][0].ToString();
        }
        return str;
    }
    protected void TxtClaimName_textChanged(object sender, EventArgs e)
    {
        if (TxtClaimName.Text != "")
        {
            if (TxtClaimName.Text.Trim() == "Past Leave Settlement" || TxtClaimName.Text.Trim() == "Leave Settlement" || TxtClaimName.Text.Trim() == "Indemnity Claim" || TxtClaimName.Text.Trim() == "Leave Salary")
            {
                DisplayMessage("You cannot Insert this Claim Name");
                TxtClaimName.Text = "";
                TxtClaimName.Focus();
            }
        }

    }



    protected void DdlMonthList_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnBindList_Click(null, null);
    }
}
