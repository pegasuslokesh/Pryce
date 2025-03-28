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

public partial class HR_EmpAllowanceDeduction : BasePage
{
    #region defind Class Object

    Common ObjComman = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc =null;
    Set_Allowance ObjAllow = null;
    Set_Deduction ObjDeduc = null;
    Set_EmployeeGroup_Master objEmpGroup = null;

    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    SystemParameter ObjSysParam = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeParameter objEmpParam = null;
    Set_Allowance objAllowance = null;
    Set_Deduction objDeduction = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(Session["DBConnection"].ToString());
        ObjAllow = new Set_Allowance(Session["DBConnection"].ToString());
        ObjDeduc = new Set_Deduction(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());

        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAllowance = new Set_Allowance(Session["DBConnection"].ToString());
        objDeduction = new Set_Deduction(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/EmpAllowanceDeduction.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
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
            Session["dtAllowDeduction"] = null;
            Session["CHECKED_ITEMS"] = null;
            chkYearCarry.Visible = false;
            Session["empimgpath"] = null;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            FillddlLocation();
            FillddlDeaprtment();
            btnAllowDeduction_Click(null, null);
            bool IsCompOT = false;
            bool IsPartialComp = false;
            try
            {

                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));


                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_AllowDeduction_Enable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
            }
            catch
            {
            }

            ddlLocation.Focus();
        }

        Page.Title = ObjSysParam.GetSysTitle();
      
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSaveAllowDeduction.Visible = clsPagePermission.bAdd;
        
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
      
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

    private void FillddlDeaprtment()
    {
        DataTable dtDepartment = PageControlCommon.GetEmployeeDepartmentByLocationId(ddlLocation, dpDepartment, Session["DBConnection"].ToString());
        objPageCmn.FillData((object)dpDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }

    protected void dpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlDeaprtment();
        FillGrid();
    
        dpDepartment.Focus();
    }

    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
      
        //gvEmployee_Allowancededuction.HeaderRow.FindControl("chkgvSelectAll").Focus();
    }
    public void FillGrid()
    {
        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpAllowDeduction"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmployee_Allowancededuction, dtEmp, "", "");
            lblTotalRecordsAllowDeduction.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        else
        {
            gvEmployee_Allowancededuction.DataSource = null;
            gvEmployee_Allowancededuction.DataBind();
            Session["dtEmpAllowDeduction"] = dtEmp;
            lblTotalRecordsAllowDeduction.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
   
    }
    public string DateFormat(string Date)
    {
        string SystemDate = string.Empty;

        try
        {
            SystemDate = Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        catch
        {
            SystemDate = Date;
        }

        return SystemDate;
    }
    protected void gvEmployee_Allowancededuction_Sorting(object sender, GridViewSortEventArgs e)
    {
        hdFSortgvEmpAllowDeduction.Value = hdFSortgvEmpAllowDeduction.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpAllowDeduction"];
        DataView dv = new DataView(dt);

        string Query = "" + e.SortExpression + " " + hdFSortgvEmpAllowDeduction.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployee_Allowancededuction, dt, "", "");
       
        try
        {
            if (e.SortExpression == "Emp_Code")
            {
                gvEmployee_Allowancededuction.HeaderRow.Cells[2].Focus();
            }
            if (e.SortExpression == "Emp_Name")
            {
                gvEmployee_Allowancededuction.HeaderRow.Cells[3].Focus();
            }
            if (e.SortExpression == "DOJ")
            {
                gvEmployee_Allowancededuction.HeaderRow.Cells[4].Focus();
            }
        }
        catch
        {

        }

    }
    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        dpDepartment.SelectedIndex = 0;
        ddlLocation.SelectedIndex = 0;
        FillGrid();
        Session["dtAllowDeduction"] = null;
     
        ddlLocation.Focus();

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
            DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());

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
                Session["dtEmp1"] = dtEmp;
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp1"] = null;
                gvEmployee.DataSource = null;
                gvEmployee.DataBind();

            }

        }
        else
        {
            gvEmployee.DataSource = null;
            gvEmployee.DataBind();

        }


        ddlType.Focus();

    }
    protected void gvEmployee_Allowancededuction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmployee_Allowancededuction.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtEmpAllowDeduction"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployee_Allowancededuction, dt, "", "");
        PopulateCheckedValues();
     
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmployee_Allowancededuction.Rows)
            {
                int index = (int)gvEmployee_Allowancededuction.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvEmployee_Allowancededuction.Rows)
        {
            index = (int)gvEmployee_Allowancededuction.DataKeys[gvrow.RowIndex].Value;
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
    protected void chkgvSelectAll_CheckedChangedAllowDeduction(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        CheckBox chkSelAll = ((CheckBox)gvEmployee_Allowancededuction.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvEmployee_Allowancededuction.Rows)
        {
            index = (int)gvEmployee_Allowancededuction.DataKeys[gvrow.RowIndex].Value;
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

    protected void ImgbtnSelectAll_ClickAllowDeduction(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpAllowDeduction"];

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
            foreach (GridViewRow gvrow in gvEmployee_Allowancededuction.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpAllowDeduction"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmployee_Allowancededuction, dtAddressCategory1, "", "");
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
        Session["dtAllowDeduction"] = null;

        chkYearCarry.Visible = false;
   
        txtValue1.Focus();
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (Session["dtEmpAllowDeduction"] != null)
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
                DataTable dtEmp = (DataTable)Session["dtEmpAllowDeduction"];
                DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEmployee_Allowancededuction, view.ToTable(), "", "");
                lblTotalRecordsAllowDeduction.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
                Session["dtEmpAllowDeduction"] = view.ToTable();
            }
        }
        Session["dtAllowDeduction"] = null;

        chkYearCarry.Visible = false;
      
        txtValue1.Focus();
    }

    protected void btnAllowDeduction_Click(object sender, EventArgs e)
    {

        pnlAllowDeduction.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        chkYearCarry.Visible = false;
        PnlEmployeeAllowDeduction.Visible = true;
        chkYearCarry.Visible = false;
        Session["CHECKED_ITEMS"] = null;
        gvEmployee_Allowancededuction.Visible = true;
        FillGrid();
        Session["dtAllowDeduction"] = null;
        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        EmpGroup_CheckedChanged(null, null);
    }
    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp1"], "", "");
        gvEmployee.HeaderRow.Focus();
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
            Div_Group.Visible = true;
            Div_Employee.Visible = false;
        }
        else if (rbtnEmp.Checked)
        {
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
            FillGrid();
            Div_Group.Visible = false;
            Div_Employee.Visible = true;
        }
       
    }
    protected void ddlType_SelectedIndexChanged1(object sender, EventArgs e)
    {

        if (ddlType.SelectedIndex == 0)
        {
            try
            {
                ddlAllDeduc.Items.Clear();
            }
            catch
            {
            }

        }



        if (ddlType.SelectedIndex == 1)
        {


            DataTable dsEmpCat = null;
            dsEmpCat = ObjAllow.GetAllowanceTrueAll(Session["CompId"].ToString());


            if (dsEmpCat.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)ddlAllDeduc, dsEmpCat, "Allowance", "Allowance_Id");
            }
            else
            {
                ddlAllDeduc.Items.Insert(0, "--Select--");
                ddlAllDeduc.SelectedIndex = 0;
            }
        }
        if (ddlType.SelectedIndex == 2)
        {
            DataTable dsEmpCat = null;
            dsEmpCat = ObjDeduc.GetDeductionTrueAll(Session["CompId"].ToString());
            if (dsEmpCat.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)ddlAllDeduc, dsEmpCat, "Deduction", "Deduction_Id");
            }
            else
            {
                ddlAllDeduc.Items.Insert(0, "--Select--");
                ddlAllDeduc.SelectedIndex = 0;
            }
        }
        ddlAllDeduc.Focus();
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
    protected void btnSaveAllowDeduction_Click(object sender, EventArgs e)
    {
        int b = 0;


        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsSalaryPlanEnable", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString())) && ddlType.SelectedIndex == 1)
        {
            DisplayMessage("Salary plan is enable on company level , you can not assign allowance");
            return;
        }

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
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        DataTable dtEmp = PageControlCommon.GetEmployeeListbyLocationandDepartment(ddlLocation, dpDepartment, true, Session["DBConnection"].ToString());

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
                return;
            }
            if (ddlType.SelectedIndex == 0)
            {
                DisplayMessage("Select Type");
                ddlType.Focus();
                return;
            }








            if (ddlAllDeduc.SelectedItem.Text == "--Select--")
            {
                DisplayMessage("Select Allowance or Deduction");
                ddlAllDeduc.Focus();
                return;
            }

            if (txtCalValue.Text == "")
            {
                DisplayMessage("Insert Calculation Value");
                txtCalValue.Focus();
                return;

            }


            foreach (string str in EmpIds.Split(','))
            {
                if (str != "")
                {
                    DataTable DtAllDeduc = new DataTable();

                    DtAllDeduc = ObjAllDeduc.GetPayAllowDeducAll(Session["CompId"].ToString(), str, ddlType.SelectedValue, ddlAllDeduc.SelectedValue);

                    DtAllDeduc = new DataView(DtAllDeduc, "Emp_Id ='" + str + "' and Type='" + ddlType.SelectedValue + "' and Ref_Id= '" + ddlAllDeduc.SelectedValue + "' ", "", DataViewRowState.CurrentRows).ToTable();

                    if (DtAllDeduc.Rows.Count > 0)
                    {
                        b = ObjAllDeduc.UpdatePayEmpAllowDeduc(Session["CompId"].ToString(), DtAllDeduc.Rows[0]["Trans_Id"].ToString(), str, ddlType.SelectedValue, ddlAllDeduc.SelectedValue, ddlCalculation.SelectedValue.ToString(), ddlValueType.SelectedValue.ToString(), txtCalValue.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                    else
                    {
                        b = ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), str, ddlType.SelectedValue, ddlAllDeduc.SelectedValue, ddlCalculation.SelectedValue.ToString(), ddlValueType.SelectedValue.ToString(), txtCalValue.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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

            //if (lblSelectRecd.Text == "")
            //{

            //    DisplayMessage("Select Employee First");
            //    gvEmployee_Allowancededuction.HeaderRow.FindControl("chkgvSelectAll").Focus();
            //    return;

            //}
            if (ddlType.SelectedIndex == 0)
            {

                DisplayMessage("Select Type");
                ddlType.Focus();
                return;
            }
            if (ddlAllDeduc.SelectedItem.Text == "--Select--")
            {
                DisplayMessage("Select Allowance or Deduction");
                ddlAllDeduc.Focus();
                return;
            }

            if (txtCalValue.Text == "")
            {
                DisplayMessage("Insert Calculation Value");
                txtCalValue.Focus();
                return;

            }


            for (int i = 0; i < userdetails.Count; i++)
            {
                string str = string.Empty;
                str = userdetails[i].ToString();

                if (str != "")
                {
                    DataTable DtAllDeducEmp = new DataTable();

                    DtAllDeducEmp = ObjAllDeduc.GetPayAllowDeducAll(Session["CompId"].ToString(), str, ddlType.SelectedValue, ddlAllDeduc.SelectedValue);

                    DtAllDeducEmp = new DataView(DtAllDeducEmp, "Emp_Id ='" + str + "' and Type='" + ddlType.SelectedValue + "' and Ref_Id= '" + ddlAllDeduc.SelectedValue + "' ", "", DataViewRowState.CurrentRows).ToTable();

                    if (DtAllDeducEmp.Rows.Count > 0)
                    {
                        b = ObjAllDeduc.UpdatePayEmpAllowDeduc(Session["CompId"].ToString(), DtAllDeducEmp.Rows[0]["Trans_Id"].ToString(), str, ddlType.SelectedValue, ddlAllDeduc.SelectedValue, ddlCalculation.SelectedValue.ToString(), ddlValueType.SelectedValue.ToString(), txtCalValue.Text, "0", "0", "0", "0", "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                    else
                    {
                        b = ObjAllDeduc.InsertPayEmpAllowDeduc(Session["CompId"].ToString(), str, ddlType.SelectedValue, ddlAllDeduc.SelectedValue, ddlCalculation.SelectedValue.ToString(), ddlValueType.SelectedValue.ToString(), txtCalValue.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
        }
        if (b != 0)
        {


            DisplayMessage("Record Saved", "green");





        }
        else
        {
            DisplayMessage("Record Not Saved");
        }

        Reset();

        ddlType.Focus();

    }



    protected void btnEmpEdit_Command(object sender, CommandEventArgs e)
    {

        pnl1.Visible = true;
        pnl2.Visible = true;
        RbtBoth.Checked = true;
        DataTable dtBindGrid = new DataTable();
        dtBindGrid = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dtBindGrid.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEditAllowDeduction, dtBindGrid, "", "");

            Session["PayAllow"] = dtBindGrid;
            lblEmpCodeAllowDeduction.Text = GetEmployeeCode(e.CommandArgument.ToString());
            lblEmpNameAllowDeduction.Text = GetEmployeeName(e.CommandArgument.ToString());
            Session["Allowance_EmpId"] = e.CommandArgument.ToString();
            RbtAllowance.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "show_modal()", true);
        }
        else
        {
            pnl1.Visible = false;
            pnl2.Visible = false;
            DisplayMessage("Employee has no Allowance or Deduction");
            return;

        }

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

    protected void Imgbtn_Click(object sender, EventArgs e)
    {
        pnl1.Visible = false;
        pnl2.Visible = false;
    }

    protected void BtnCancelPopUp_Click(object sender, EventArgs e)
    {
        pnl1.Visible = false;
        pnl2.Visible = false;
    }



    protected void BtnDel_Click(object sender, EventArgs e)
    {
        int c = 0;

        int a = 0;
        foreach (GridViewRow gvr in gvEditAllowDeduction.Rows)
        {

            CheckBox chk = (CheckBox)gvr.FindControl("ChkEmpCheck");
            if (chk.Checked)
            {
                HiddenField Empid = (HiddenField)gvr.FindControl("hdnEmpId");
                HiddenField hdnTId = (HiddenField)gvr.FindControl("hdntransId");
                c = ObjAllDeduc.DeletePayAllowDeduc(Session["CompId"].ToString(), hdnTId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
                a = 1;

            }
        }
        if (a == 0)
        {
            DisplayMessage("Please Select Record");
            try
            {
                gvEditAllowDeduction.Rows[0].Cells[0].Focus();
            }
            catch
            {

            }
            return;
        }

        if (a != 0)
        {
            DisplayMessage("Record Deleted");
            DataTable dtBindGrid = new DataTable();

            dtBindGrid = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), Session["Allowance_EmpId"].ToString());
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEditAllowDeduction, dtBindGrid, "", "");
        }
        try
        {
            gvEditAllowDeduction.Rows[0].Cells[0].Focus();
        }
        catch
        {

        }

    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {


        int c = 0;

        foreach (GridViewRow gvr in gvEditAllowDeduction.Rows)
        {



            if (true)
            {
                //string Value = gvAllowDeductionEmp.DataKeys[e.RowIndex].Values["Value"].ToString();
                TextBox txtvalue1 = (TextBox)gvr.FindControl("txtValue");
                DropDownList ddlValuetyp = (DropDownList)gvr.FindControl("ddlSchType0");
                DropDownList ddlCalc = (DropDownList)gvr.FindControl("ddlCalcuationGrid");
                HiddenField hdnTrans = (HiddenField)gvr.FindControl("hdntransId");
                HiddenField hdngvType = (HiddenField)gvr.FindControl("hdngvType");
                Label lblgvtype = (Label)gvr.FindControl("lblType");
                HiddenField hdnrefId = (HiddenField)gvr.FindControl("hdnRefId");
                //Label lblvrefId = (Label)gvr.FindControl("lblRefId");
                if (txtvalue1.Text == "")
                {
                    txtvalue1.Text = "0";
                }

                c = ObjAllDeduc.UpdatePayEmpAllowDeduc(Session["CompId"].ToString(), hdnTrans.Value, Session["Allowance_EmpId"].ToString(), hdngvType.Value.ToString(), hdnrefId.Value.ToString(), ddlCalc.SelectedValue.ToString(), ddlValuetyp.SelectedValue.ToString(), txtvalue1.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());




            }




        }

        DataTable dtBindGrid = new DataTable();

        dtBindGrid = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), Session["Allowance_EmpId"].ToString());
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEditAllowDeduction, dtBindGrid, "", "");

        Session["PayAllow"] = dtBindGrid;
        if (dtBindGrid.Rows.Count == 0)
        {
            pnl1.Visible = false;
            pnl2.Visible = false;

        }
        else
        {
            RbtBoth.Checked = true;
            RbtBoth_CheckedChanged(null, null);
        }
        if (c != 0)
        {
            DisplayMessage("Record Updated", "green");
            Reset();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
            try
            {
                gvEditAllowDeduction.Rows[0].Cells[0].Focus();
            }
            catch
            {

            }

        }
        else
        {
            DisplayMessage("Please Select Record");




        }

    }


    protected void gvEditAllowDeduction_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEditAllowDeduction.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEditAllowDeduction, (DataTable)Session["PayAllow"], "", "");
    
    }
    protected void RbtAllowance_CheckedChanged(object sender, EventArgs e)
    {
        int i = 0;
        if (RbtAllowance.Checked)
        {
            DataTable DtAllow = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), Session["Allowance_EmpId"].ToString());
            DtAllow = new DataView(DtAllow, "Type='1'", "", DataViewRowState.CurrentRows).ToTable();

            if (DtAllow.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEditAllowDeduction, DtAllow, "", "");
                Session["PayAllow"] = DtAllow;
            }
            else
            {
                gvEditAllowDeduction.DataSource = null;
                gvEditAllowDeduction.DataBind();
            }
        }
        else
        {
            gvEditAllowDeduction.DataSource = null;
            gvEditAllowDeduction.DataBind();
        }
    }
    protected void RbtDeduction_CheckedChanged(object sender, EventArgs e)
    {


        if (RbtDeduction.Checked)
        {
            DataTable DtDeduct = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), Session["Allowance_EmpId"].ToString());
            DtDeduct = new DataView(DtDeduct, "Type='2'", "", DataViewRowState.CurrentRows).ToTable();

            if (DtDeduct.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEditAllowDeduction, DtDeduct, "", "");
                Session["PayAllow"] = DtDeduct;
            }
            else
            {
                gvEditAllowDeduction.DataSource = null;
                gvEditAllowDeduction.DataBind();
            }


        }
        else
        {
            gvEditAllowDeduction.DataSource = null;
            gvEditAllowDeduction.DataBind();
        }


    }
    protected void RbtBoth_CheckedChanged(object sender, EventArgs e)
    {

        if (RbtBoth.Checked)
        {
            DataTable dtBothAllowDeduc = ObjAllDeduc.GetPayAllowDeducByEmpId(Session["CompId"].ToString(), Session["Allowance_EmpId"].ToString());
            dtBothAllowDeduc = new DataView(dtBothAllowDeduc, "", "", DataViewRowState.CurrentRows).ToTable();

            if (dtBothAllowDeduc.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvEditAllowDeduction, dtBothAllowDeduc, "", "");
                Session["PayAllow"] = dtBothAllowDeduc;
            }
        }
        else
        {
            gvEditAllowDeduction.DataSource = null;
            gvEditAllowDeduction.DataBind();
        }
    }
    public string GetType(string Type)
    {
        if (Type == "1")
        {
            Type = "Allowance";
        }
        else if (Type == "2")
        {
            Type = "Deduction";

        }
        return Type;
    }
    public string GetCalculation(string Value_Type)
    {
        if (Value_Type == "1")
        {
            Value_Type = "Fixed";
        }
        else if (Value_Type == "2")
        {
            Value_Type = "Percentage";

        }
        return Value_Type;
    }

    public void Reset()
    {
        ddlType.SelectedIndex = 0;
        ddlAllDeduc.Items.Clear();
        ddlCalculation.SelectedIndex = 0;
        txtCalValue.Text = "";
        ddlValueType.SelectedIndex = 0;
        txtValue1.Text = "";
     
        //btnAllowDeduction_Click(null, null);

    }



    protected void btnResetAllowDeduction_Click(object sender, EventArgs e)
    {
        ddlType.SelectedIndex = 0;
        if (ddlType.SelectedIndex == 0)
        {
            ddlAllDeduc.Items.Clear();

        }
        ddlCalculation.SelectedIndex = 0;

        txtCalValue.Text = "";
        ddlValueType.SelectedIndex = 0;
        txtValue1.Text = "";
        ddlLocation.SelectedIndex = 0;
        dpDepartment.SelectedIndex = 0;

        btnAllowDeduction_Click(null, null);


    }
}

