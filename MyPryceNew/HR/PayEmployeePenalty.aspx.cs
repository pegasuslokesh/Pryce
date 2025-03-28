using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;

public partial class HR_PayEmployeePenalty : BasePage
{
    #region defind Class Object

    Common ObjComman = null;
    Pay_Employee_Month objPayEmpMonth = null;
    Pay_Employee_Penalty ObjPenalty = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Set_ApplicationParameter objAppParam = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    PageControlCommon objPageCmn = null;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["AccordianId"] = "19";
        Session["HeaderText"] = "HR";

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjComman = new Common(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        ObjPenalty = new Pay_Employee_Penalty(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/PayEmployeePenalty.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 9;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            Session["dtPenalty"] = null;
            FillddlLocation();
            FillddlDeaprtment();
            lblWrongSequence.Text = "";
            lblDojIssue.Text = "";
            TxtYear.Text = DateTime.Now.Year.ToString();
            int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());
            ddlMonth.SelectedValue = (CurrentMonth).ToString();

            //this function is used to get the valid employee list
            ValidEmpList();

            Session["empimgpath"] = null;

            Page.Form.Attributes.Add("enctype", "multipart/form-data");


            bool IsCompOT = false;
            bool IsPartialComp = false;
            try
            {
                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Enable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {

            }
            btnPenalty_Click(null, null);
            btnLeast_Click(null, null);

            //this code is created by jitendra upadhyay on 17-07-2014
            //this code for set the gridview page size according the system parameter
            try
            {
                gvEmployee.PageSize = int.Parse(Session["GridSize"].ToString());
                gvEmpPenalty.PageSize = int.Parse(Session["GridSize"].ToString());
                GridViewPenaltyList.PageSize = int.Parse(Session["GridSize"].ToString());
            }
            catch
            {

            }
            ddlLocation.Focus();

        }

        Page.Title = objSys.GetSysTitle();

       
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
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
    protected void GridViewPenaltyList_Sorting(object sender, GridViewSortEventArgs e)
    {
        HdfSort.Value = HdfSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pay_Emp_Pen"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HdfSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pay_Emp_Pen"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GridViewPenaltyList, dt, "", "");
    
        GridViewPenaltyList.HeaderRow.Focus();
    }
    protected void GridViewEmpPenalty_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdFSortgvEmpPenalty.Value = hdFSortgvEmpPenalty.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpPenalty"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + hdFSortgvEmpPenalty.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dt, "", "");
       
        gvEmpPenalty.HeaderRow.Focus();
    }
    private void FillddlDeaprtment()
    {
        DataTable dtDepartment = PageControlCommon.GetEmployeeDepartmentByLocationId(ddlLocation, dpDepartment, Session["DBConnection"].ToString());
        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
        
    }
    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillddlDeaprtment();
        FillGrid();
    
    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
       
    }
    public void FillGrid()
    {
        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());

        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpPenalty"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpPenalty, dtEmp, "", "");
            lblTotalRecordsPenalty.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        else
        {
            gvEmpPenalty.DataSource = null;
            gvEmpPenalty.DataBind();
            Session["dtEmpPenalty"] = dtEmp;
            lblTotalRecordsPenalty.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        
    }
    protected void btnAllRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        Session["dtPenalty"] = null;
    

    }
  
    void GridBind_PenaltyList(string Month, string Year)
    {
        DataTable Dt = new DataTable();
        Dt = ObjPenalty.GetRecord_From_PayEmployeePenalty_By_MonthAndYear(Session["CompId"].ToString(), Month, Year);
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
            objPageCmn.FillData((object)GridViewPenaltyList, Dt, "", "");
            Session["dtFilter_Pay_Emp_Pen"] = Dt;
            lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " : " + Dt.Rows.Count.ToString();
        }
        else
        {
            Dt.Clear();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewPenaltyList, Dt, "", "");
            DisplayMessage("Record Not found");
            DdlMonthList.Focus();
            Session["dtFilter_Pay_Emp_Pen"] = null;
            lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " :0";
        }
      
    }
    void ValidEmpList()
    {
        DataTable dtEmp = Common.GetEmployeeListbyLocationIdandDepartmentValue(Session["LocId"].ToString(),"0",true, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["RoleId"].ToString(), Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());

        
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
            try
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {

            }
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
      
        lbxGroup.Focus();
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmpPenalty.Rows)
            {
                int index = (int)gvEmpPenalty.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvEmpPenalty.Rows)
        {
            index = (int)gvEmpPenalty.DataKeys[gvrow.RowIndex].Value;
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
    protected void gvEmpPenalty_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmpPenalty.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmpPenalty"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dt, "", "");
        PopulateCheckedValues();
      
    }
    protected void chkgvSelectAll_CheckedChangedPenalty(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)gvEmpPenalty.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvEmpPenalty.Rows)
        {
            index = (int)gvEmpPenalty.DataKeys[gvrow.RowIndex].Value;
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


    protected void ImgbtnSelectAll_ClickPenalty(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtPenalty = (DataTable)Session["dtEmpPenalty"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPenalty.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvEmpPenalty.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpPenalty"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmpPenalty, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void btnPenaltyRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";

        FillGrid();
        Session["dtPenalty"] = null;
        txtValue1.Focus();

    }
    protected void btnPenaltyRefreshSearchPanel_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlField1SearchPanel.SelectedIndex = 1;
        ddlOption1SearchPanel.SelectedIndex = 2;
        txtValue1SearchPanel.Text = "";
        GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);

        Session["dtPenalty"] = null;
   
        txtValue1SearchPanel.Focus();

    }
    protected void btnPenaltybind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (Session["dtEmpPenalty"] != null)
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
                DataTable dtEmp = (DataTable)Session["dtEmpPenalty"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmpPenalty, view.ToTable(), "", "");
                Session["dtEmpPenalty"] = view.ToTable();
                lblTotalRecordsPenalty.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            }
        }
        
        Session["dtPenalty"] = null;
      
        txtValue1.Focus();
    }
    protected void btnPenaltybindSearchPanel_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (Session["dtFilter_Pay_Emp_Pen"] != null)
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
                DataTable dtEmp = (DataTable)Session["dtFilter_Pay_Emp_Pen"];
                DataView view = new DataView();
                try
                {
                    view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                }
                catch
                {

                }
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GridViewPenaltyList, view.ToTable(), "", "");
                Session["dtFilter_Pay_Emp_Pen"] = view.ToTable();
                lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            }
        }
        else
        {
            GridViewPenaltyList.DataSource = null;
            GridViewPenaltyList.DataBind();
            lblTotalRecordSearchPanel.Text = Resources.Attendance.Total_Records + " :0";
        }
        Session["dtPenalty"] = null;
      
        txtValue1SearchPanel.Focus();
    }
    protected void btnPenalty_Click(object sender, EventArgs e)
    {

        pnlPenalty.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");


        PnlEmployeePenalty.Visible = true;
        PanelSearchList.Visible = false;
        

        Session["CHECKED_ITEMS"] = null;
        gvEmpPenalty.Visible = true;
        FillGrid();
        Session["dtPenalty"] = null;
        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        EmpGroup_CheckedChanged(null, null);
        TxtPenaltyName.Focus();
      
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
            lbxGroup.Focus();
        }
        else if (rbtnEmp.Checked)
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;

            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = Common.GetEmployeeListbyLocationIdandDepartmentValue(Session["LocId"].ToString(), "0", true, Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["RoleId"].ToString(), Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString());
            
            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp"] = dtEmp;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            }
            ddlLocation.Focus();
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
        TxtPenaltyName.Text = "";
        TxtPenaltyDiscription.Text = "";
        ddlMonth.SelectedIndex = 0;
        txtCalValue.Text = "";
        DdlValueType.SelectedIndex = 0;
        TxtYear.Text = "";

        txtValue1.Text = "";
        TxtPenaltyName.Focus();
        TxtYear.Text = DateTime.Now.Year.ToString();
        Session["CHECKED_ITEMS"] = null;
        ViewState["Select"] = null;
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month.ToString());

        ddlMonth.SelectedValue = (CurrentMonth).ToString();
        foreach (GridViewRow Gvrow in gvEmpPenalty.Rows)
        {
            CheckBox ChkHeader = (CheckBox)gvEmpPenalty.HeaderRow.FindControl("chkgvSelectAll");
            CheckBox ChkItem = (CheckBox)Gvrow.FindControl("chkgvSelect");

            ChkHeader.Checked = false;
            ChkItem.Checked = false;


        }

    }
    protected void btnSavePenalty_Click(object sender, EventArgs e)
    {
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



            if (GroupIds != "")
            {



                if (TxtPenaltyName.Text.Trim() == "")
                {
                    DisplayMessage("Enter Penalty Name");
                    TxtPenaltyName.Focus();
                    return;
                }
                if (DdlValueType.SelectedIndex == 0)
                {
                    DisplayMessage("Select Value Type");
                    DdlValueType.Focus();
                    return;

                }
                if (txtCalValue.Text == "")
                {
                    DisplayMessage("Enter Penalty Value");
                    txtCalValue.Focus();
                    return;
                }
                if (ddlMonth.SelectedIndex == 0)
                {
                    DisplayMessage("Select Month");
                    ddlMonth.Focus();
                    return;
                }
                if (TxtYear.Text == "")
                {
                    DisplayMessage("Enter Year");
                    TxtYear.Focus();
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





            }
            else
            {
                DisplayMessage("Select Group First");
                lbxGroup.Focus();
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
                        b = ObjPenalty.Insert_In_Pay_Employee_Penalty(Session["CompId"].ToString(), str, TxtPenaltyName.Text.Trim(), TxtPenaltyDiscription.Text.Trim(), DdlValueType.SelectedValue, txtCalValue.Text, DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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


            if (TxtPenaltyName.Text.Trim() == "")
            {
                DisplayMessage("Enter Penalty Name");
                TxtPenaltyName.Focus();
                return;
            }
            if (DdlValueType.SelectedIndex == 0)
            {
                DisplayMessage("Select Value Type");
                DdlValueType.Focus();
                return;

            }
            if (txtCalValue.Text == "")
            {
                DisplayMessage("Enter Penalty Value");
                txtCalValue.Focus();
                return;
            }
            if (ddlMonth.SelectedIndex == 0)
            {
                DisplayMessage("Select Month");
                ddlMonth.Focus();
                return;
            }
            if (TxtYear.Text == "")
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
                    DataTable dtempmonth = new DataTable();
                    dtempmonth = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, ddlMonth.SelectedIndex.ToString(), TxtYear.Text.ToString(),Session["CompId"].ToString());
                    if (dtempmonth.Rows.Count > 0)
                    {
                        DisplayMessage("Payroll Posted For This Month and Year");
                        return;

                    }
                    else
                    {

                        b = ObjPenalty.Insert_In_Pay_Employee_Penalty(Session["CompId"].ToString(), str, TxtPenaltyName.Text.Trim(), TxtPenaltyDiscription.Text.Trim(), DdlValueType.SelectedValue, txtCalValue.Text, DateTime.Now.ToString(), ddlMonth.SelectedValue, TxtYear.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
            ViewState["Select"] = null;
            Session["CHECKED_ITEMS"] = null;
        }
        else
        {
            DisplayMessage("Record Not Saved");
        }


    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lblWrongSequence.Text = "";
        lblDojIssue.Text = "";
        Reset();
    }
    protected void btnLeast_Click(object sender, EventArgs e)
    {
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlPenalty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        DdlMonthList.Focus();
        PanelSearchList.Visible = true;
        PnlEmployeePenalty.Visible = false;
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month);
        DdlMonthList.SelectedValue = (CurrentMonth).ToString();
        TxtYearList.Text = DateTime.Now.Year.ToString();
        GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);
    }
    protected void BtnBindList_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
        GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);
        Session["searchMonth"] = DdlMonthList.SelectedValue;
        Session["SearchYear"] = TxtYearList.Text;
        BtnRefreshList.Focus();
        ddlField1SearchPanel.SelectedIndex = 1;
        ddlOption1SearchPanel.SelectedIndex = 2;
        txtValue1SearchPanel.Text = "";
       
    }
    protected void BtnRefreshList_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int CurrentMonth = Convert.ToInt32(DateTime.Now.Month);
        DdlMonthList.SelectedValue = (CurrentMonth).ToString();
        TxtYearList.Text = DateTime.Now.Year.ToString();
        GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);
        DdlMonthList.Focus();
        ddlField1SearchPanel.SelectedIndex = 1;
        ddlOption1SearchPanel.SelectedIndex = 2;
        txtValue1SearchPanel.Text = "";
      

    }
    protected void btnEdit_command(object sender, CommandEventArgs e)
    {


        DataTable Dt = new DataTable();
        Dt = ObjPenalty.GetRecord_From_PayEmployeePenalty_usingPenaltyId(Session["CompId"].ToString(), "0", e.CommandArgument.ToString(), "0", "0", "", "");
        if (Dt.Rows.Count > 0)
        {




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


            PanelUpdatePenalty.Visible = true;
            HiddeniD.Value = e.CommandArgument.ToString();


            TxtPenaltyNameList.Text = Dt.Rows[0]["Penalty_Name"].ToString();
            TxtPenaltyDiscList.Text = Dt.Rows[0]["Penalty_Description"].ToString();
            DdlvalueTypelist.SelectedValue = Dt.Rows[0]["value_Type"].ToString();
            Txtvaluelist.Text = Dt.Rows[0]["Value"].ToString();
            DdlMonthListPanel.SelectedValue = Dt.Rows[0]["Penalty_Month"].ToString();
            TxtpanelYearList.Text = Dt.Rows[0]["Penalty_Year"].ToString();
            txtEmployeeId.Text = Dt.Rows[0]["Emp_Id"].ToString();
            txtEmployeeName.Text = Dt.Rows[0]["Emp_Name"].ToString();
            TxtPenaltyNameList.Focus();
            Edit_Penalty.Visible = true;
        }

    }
    protected void IbtnDelete_command(object sender, CommandEventArgs e)
    {
        DataTable Dt = new DataTable();
        Dt = ObjPenalty.GetRecord_From_PayEmployeePenalty_usingPenaltyId(Session["CompId"].ToString(), "0", e.CommandArgument.ToString(), "0", "0", "", "");
        if (Dt.Rows.Count > 0)
        {




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


        string PenaltyId = e.CommandArgument.ToString();
        int CheckDeletion = 0;
        CheckDeletion = ObjPenalty.DeleteRecord_in_Pay_Employee_penalty(Session["CompId"].ToString(), PenaltyId, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (CheckDeletion != 0)
        {
            DisplayMessage("Record Deleted");
            ddlField1SearchPanel.SelectedIndex = 1;
            ddlOption1SearchPanel.SelectedIndex = 2;
            txtValue1SearchPanel.Text = "";
            GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
      
    }
    void ResetPanel()
    {
        TxtPenaltyNameList.Text = "";
        TxtPenaltyDiscList.Text = "";
        DdlvalueTypelist.SelectedIndex = 0;
        Txtvaluelist.Text = "";
        DdlMonthListPanel.SelectedIndex = 0;
        TxtpanelYearList.Text = "";
        HiddeniD.Value = "";
        PanelUpdatePenalty.Visible = false;
        txtEmployeeId.Text = "";
        txtEmployeeName.Text = "";

        Edit_Penalty.Visible = false;


    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {

        if (HiddeniD.Value == "")
        {

            DisplayMessage("Edit The Record");
            DdlMonthList.Focus();
            return;
        }



        if (TxtPenaltyNameList.Text.Trim() == "")
        {
            DisplayMessage("Enter Penalty Name");
            TxtPenaltyNameList.Focus();
            return;
        }

        if (DdlvalueTypelist.SelectedIndex == 0)
        {
            DisplayMessage("Select Value Type");
            DdlvalueTypelist.Focus();
            return;
        }

        if (Txtvaluelist.Text == "")
        {
            DisplayMessage("Enter Penalty Value");
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

        string EmployeeId = ValidPayRoll(txtEmployeeId.Text, "Update");
        if (EmployeeId != "")
        {
            int UpdationCheck = 0;
            UpdationCheck = ObjPenalty.UpdateRecord_In_Pay_Employee_Penalty(Session["CompId"].ToString(), HiddeniD.Value, TxtPenaltyNameList.Text.Trim(), TxtPenaltyDiscList.Text.Trim(), DdlvalueTypelist.SelectedValue, Txtvaluelist.Text, DdlMonthListPanel.SelectedValue, TxtpanelYearList.Text, Session["UserId"].ToString(), DateTime.Now.ToString());

            if (UpdationCheck != 0)
            {
                DisplayMessage("Record Updated", "green");

                GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);
                DdlMonthList.Focus();
                ResetPanel();
            
                Edit_Penalty.Visible = false;

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
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

                                    lblWrongSequence.Text = "Penalty Month and Year Should be Greater than Last Posted Month And Year For Employee Code=" + GetEmployeeCode(str);
                                }
                                else
                                {
                                    lblWrongSequence.Text = GetArebicMessage("Penalty Month and Year Should be Greater than Last Posted Month And Year For Employee Code") + "=" + GetEmployeeCode(str);


                                }
                            }
                            else
                            {
                                lblWrongSequence.Text = lblWrongSequence.Text + "," + GetEmployeeCode(str);
                            }
                        }
                        else
                        {
                            DisplayMessage("Penalty Month and Year Should be Greater than Last Posted Month And Year");
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

                                            lblDojIssue.Text = "Penalty Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code=" + GetEmployeeCode(str);
                                        }
                                        else
                                        {
                                            lblDojIssue.Text = GetArebicMessage("Penalty Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code") + "=" + GetEmployeeCode(str);

                                        }
                                    }
                                    else
                                    {
                                        lblDojIssue.Text = lblDojIssue.Text + "," + GetEmployeeCode(str);
                                    }
                                }
                                else
                                {
                                    DisplayMessage("Penalty Month and Year Should be Greater than or Equal to Date Of Joining");
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

                                    lblDojIssue.Text = "Penalty Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code=" + GetEmployeeCode(str);
                                }
                                else
                                {
                                    lblDojIssue.Text = GetArebicMessage("Penalty Month and Year Should be Greater than or Equal to Date Of Joining For Employee Code") + "=" + GetEmployeeCode(str);

                                }
                            }
                            else
                            {
                                lblDojIssue.Text = lblDojIssue.Text + "," + GetEmployeeCode(str);
                            }
                        }
                        else
                        {
                            DisplayMessage("Penalty Month and Year Should be Greater than or Equal to Date Of Joining");
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

    protected void GridViewPenaltyList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewPenaltyList.PageIndex = e.NewPageIndex;
        GridBind_PenaltyList(DdlMonthList.SelectedValue, TxtYearList.Text);
        GridViewPenaltyList.HeaderRow.Focus();
    }

    protected void BtnResetPenalty_Click(object sender, EventArgs e)
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetPenaltyName(string prefixText, int count, string contextKey)
    {
        Pay_Employee_Penalty objpenalty = new Pay_Employee_Penalty(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objpenalty.GetPenaltyName("", HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Penalty_Name lIKE '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();


        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i][0].ToString();
        }
        return str;
    }
}
