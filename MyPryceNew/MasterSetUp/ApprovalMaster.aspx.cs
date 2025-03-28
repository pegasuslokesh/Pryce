using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class MasterSetUp_ApprovalMaster : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    Set_ApprovalMaster ObjApproval = null;
    EmployeeMaster objEmp = null;
    Set_Group_Employee objGroupEmp = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Approval_Employee objApprovalEmp = null;
    DepartmentMaster objDep = null;
    DataAccessClass ObjDa = null;
    PageControlCommon objPageCmn = null;
    string EmpAccessPermission = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objApprovalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/ApprovalMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillGrid();
        }
        //AllPageCode();
    }
    private void FillGrid()
    {
        DataTable dtBrand = ObjApproval.GetApprovalMaster();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtBrand.Rows.Count + "";
        Session["dtApproval"] = dtBrand;
        Session["dtFilter_Appr__Master"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GvApproval, dtBrand, "", "");
            //AllPageCode();
        }
        else
        {
            GvApproval.DataSource = null;
            GvApproval.DataBind();
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnsaveConfig.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
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
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }

    public void Reset()
    {
        txtApprovalName.Text = "";
        txtApprovalNameL.Text = "";
        editid.Value = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtApprovalNameL.Text = "";
        btnNew.Text = Resources.Attendance.New;
        //hdnIsPendingApproval.Value = "false";
        //AllPageCode();
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApprovalName);
    }

    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtApprovalName.Text == "" || txtApprovalName.Text == null)
        {
            DisplayMessage("Enter Approval Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApprovalName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {
        }
        else
        {
            DataTable dtPro = ObjApproval.GetApprovalMaster();
            dtPro = new DataView(dtPro, "Approval='" + txtApprovalName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPro.Rows.Count > 0)
            {
                txtApprovalName.Text = "";
                DisplayMessage("Approval name on this object already exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtApprovalName);
                return;
            }
            //b = ObjApproval.InsertApprovalMaster(Session["CompId"].ToString().ToString(), txtApprovalName.Text, txtApprovalNameL.Text.Trim(),ddlObject.SelectedValue, "", "", "", "", true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            b = ObjApproval.InsertApprovalMaster(txtApprovalName.Text, txtApprovalNameL.Text.Trim(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                FillGrid();
                editid.Value = b.ToString();
            }
            else
            {
                DisplayMessage("Record  Not Saved");
            }
        }
    }

    protected void GvApproval_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvApproval.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Appr__Master"];
        objPageCmn.FillData((object)GvApproval, dt, "", "");
        //AllPageCode();
    }

    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
            }
            DataTable dtCurrency = (DataTable)Session["dtApproval"];
            DataView view = new DataView(dtCurrency, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvApproval, view.ToTable(), "", "");
            Session["dtFilter_Appr__Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }

    protected void GvApproval_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Appr__Master"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }
        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtFilter_Appr__Master"] = dt;
        objPageCmn.FillData((object)GvApproval, dt, "", "");
        //AllPageCode();
    }

    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Session["EmpPermission"] = objSys.Get_Approval_Parameter_By_ID(e.CommandArgument.ToString()).Rows[0]["Approval_Level"].ToString();
        rdopriority.Enabled = true;
        rdoHierarchy.Enabled = true;
        chkTeamLeader.Enabled = true;
        chkDepartmentManager.Enabled = true;
        chkParentDepartmentManager.Enabled = true;
        txtResponsibeDepartmentName.Enabled = true;
        EmpAccessPermission = Session["EmpPermission"].ToString();
        DataTable dtTrans = objApprovalEmp.GetApprovalTransation(Session["CompId"].ToString());
        if (EmpAccessPermission == "2")
        {
            dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (EmpAccessPermission == "3")
        {
            dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (EmpAccessPermission == "4")
        {
            dtTrans = new DataView(dtTrans, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        dtTrans = new DataView(dtTrans, "Approval_Id='" + e.CommandArgument.ToString() + "' and Status='Pending' and  Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtTrans.Rows.Count > 0)
        {
            if (((LinkButton)sender).ID == "btnEdit")
            {
                if (dtTrans.Rows[0]["Approval_Type"].ToString() == "Priority")
                {
                    //hdnIsPendingApproval.Value = "true";
                    //DisplayMessage("Request is in under processing , You cannot edit this approval type");
                    dtTrans.Dispose();
                    //return;
                }
                if (dtTrans.Rows[0]["Approval_Type"].ToString() == "Hierarchy")
                {
                    rdopriority.Enabled = false;
                    rdoHierarchy.Enabled = false;
                    chkTeamLeader.Enabled = false;
                    chkDepartmentManager.Enabled = false;
                    chkParentDepartmentManager.Enabled = false;
                    txtResponsibeDepartmentName.Enabled = false;
                    DisplayMessage("Request is in under processing , You can edit only hierarchy rules");
                    dtTrans.Dispose();
                }
            }
        }
        if (((LinkButton)sender).ID == "btnEdit")
        {
            btnNew.Text = Resources.Attendance.Edit;
            Lbl_Modal_Title.Text = Resources.Attendance.EditApprovalSetup;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
        }
        else
        {
            btnNew.Text = Resources.Attendance.View;
            Lbl_Modal_Title.Text = Resources.Attendance.ViewApprovalSetup;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
        }
        DataTable dtApproval = ObjApproval.GetApprovalMasterById(e.CommandArgument.ToString());
        if (dtApproval.Rows.Count > 0)
        {
            editid.Value = e.CommandArgument.ToString();
            rdopriority.Checked = false;
            rdoHierarchy.Checked = false;
            rdoOpen.Checked = true;
            rdorestricted.Checked = false;
            lblHeaderApprovalName.Text = e.CommandName.ToString();
            if (dtApproval.Rows[0]["Approval_Type"] != null)
            {
                if (dtApproval.Rows[0]["Approval_Type"].ToString() == "Priority")
                {
                    rdopriority.Checked = true;
                    rdoHierarchy.Checked = false;
                }
                else if (dtApproval.Rows[0]["Approval_Type"].ToString() == "Hierarchy")
                {
                    rdopriority.Checked = false;
                    rdoHierarchy.Checked = true;
                }
                if (dtApproval.Rows[0]["Is_Open"].ToString() == "True")
                {
                    rdoOpen.Checked = true;
                    rdorestricted.Checked = false;
                }
                else
                {
                    rdoOpen.Checked = false;
                    rdorestricted.Checked = true;
                }
            }
            else
            {
                rdopriority.Checked = false;
                rdoHierarchy.Checked = false;
            }
            rdoHierarchy_OnCheckedChanged(null, null);
            if (rdopriority.Checked)
            {
                editid.Value = e.CommandArgument.ToString();
                EmpAccessPermission = objSys.Get_Approval_Parameter_By_ID(e.CommandArgument.ToString()).Rows[0]["Approval_Level"].ToString();
                DataTable dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), "0", "0", "0", "1");
                if (EmpAccessPermission == "2")
                {
                    dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", "2");
                }
                else if (EmpAccessPermission == "3")
                {
                    dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "3");
                }
                else if (EmpAccessPermission == "4")
                {
                    dt = objApprovalEmp.GetApprovalToEmployee(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "4");
                }
                dt = dt.DefaultView.ToTable(true, "Emp_id", "Emp_Code", "Emp_name", "Priority");
                objPageCmn.FillData((object)GvApprovalEmployeeDetail, dt, "", "");
            }
            else if (rdoHierarchy.Checked)
            {
                chkTeamLeader.Checked = Convert.ToBoolean(dtApproval.Rows[0]["Is_TeamLeader"].ToString());
                chkDepartmentManager.Checked = Convert.ToBoolean(dtApproval.Rows[0]["Is_DepartmentManager"].ToString());
                chkParentDepartmentManager.Checked = Convert.ToBoolean(dtApproval.Rows[0]["Is_Parent_departmentManager"].ToString());
                txtResponsibeDepartmentName.Text = dtApproval.Rows[0]["dep_Name"].ToString() + "/" + dtApproval.Rows[0]["ResponsibleDepartmentManager"].ToString();
            }
        }
        //AllPageCode();
    }

    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListApproval(string prefixText, int count, string contextKey)
    {
        Set_ApprovalMaster objApproval = new Set_ApprovalMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objApproval.GetApprovalMaster(), "Approval like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Approval"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListDepName(string prefixText, int count, string contextKey)
    {
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DepartmentMaster objDepMaster = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        dt = Objda.return_DataTable("select Dep_Name,Dep_Id from set_departmentmaster where dep_name like '%" + prefixText.ToString().Trim() + "%' and isactive='True'");
        //dt = new DataView(dt, "Dep_Name like '%" + prefixText.ToString().Trim() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        return txt;
    }

    protected void txtDepName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtResponsibeDepartmentName.Text != "")
        {
            string strDepName = string.Empty;
            try
            {
                strDepName = txtResponsibeDepartmentName.Text.Trim().Split('/')[0].ToString();
            }
            catch
            {
                txtResponsibeDepartmentName.Text = "";
                DisplayMessage("Select department in suggestion only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtResponsibeDepartmentName);
                return;
            }
            DataTable dt = objDep.GetDepartmentMasterByDepName(strDepName);
            if (dt.Rows.Count == 0)
            {
                txtResponsibeDepartmentName.Text = "";
                DisplayMessage("Select department in suggestion only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtResponsibeDepartmentName);
                return;
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        //try
        //{
        //    EmployeeMaster ObjEmployeeMaster = new EmployeeMaster();
        //    DataTable dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "", prefixText.ToString());

        //    string[] filterlist = new string[dtCon.Rows.Count];
        //    if (dtCon.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtCon.Rows.Count; i++)
        //        {
        //            filterlist[i] = dtCon.Rows[i]["Emp_Name"].ToString() + "/(" + dtCon.Rows[i]["Dep_name"].ToString() + ")/" + dtCon.Rows[i]["Emp_Code"].ToString();
        //        }
        //    }
        //    return filterlist;
        //}
        //catch (Exception error)
        //{

        //}
        //return null;
        EmployeeMaster objEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        //if (HttpContext.Current.Session["EmpPermission"].ToString().Trim() == "2")
        //{
        //    dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //else if (HttpContext.Current.Session["EmpPermission"].ToString().Trim() == "3")
        //{
        //    dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //else if (HttpContext.Current.Session["EmpPermission"].ToString().Trim() == "4")
        //{
        //    if (HttpContext.Current.Session["SessionDepId"] != null)
        //    {
        //        dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //}

        dtEmp = new DataView(dtEmp, "Emp_Name like '%" + prefixText + "%' ", "", DataViewRowState.CurrentRows).ToTable();
        string[] str = new string[dtEmp.Rows.Count];
        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            str[i] = "" + dtEmp.Rows[i]["Emp_Name"].ToString() + "/(" + dtEmp.Rows[i]["Department"].ToString() + ")/" + dtEmp.Rows[i]["Emp_Code"].ToString() + "";
        }
        return str;
    }

    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        string empid = "0";
        if (txtResponsiblePerson.Text != "")
        {
            try
            {
                empid = txtResponsiblePerson.Text.Split('/')[2].ToString();
            }
            catch
            {
                empid = "0";
            }
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            //if (HttpContext.Current.Session["EmpPermission"].ToString().Trim() == "2")
            //{
            //    dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //else if (HttpContext.Current.Session["EmpPermission"].ToString().Trim() == "3")
            //{
            //    dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //else if (HttpContext.Current.Session["EmpPermission"].ToString().Trim() == "4")
            //{
            //    if (HttpContext.Current.Session["SessionDepId"] != null)
            //    {
            //        dtEmp = new DataView(dtEmp, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    }
            //}
            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
                DataTable dtTemp = new DataView(GetApprovalEmployee(), "Emp_Id=" + empid + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count > 0)
                {
                    chkPriority.Checked = Convert.ToBoolean(dtTemp.Rows[0]["Priority"].ToString());
                }
                else
                {
                }
            }
            else
            {
                DisplayMessage("Employee Not Exists");
                txtResponsiblePerson.Text = "";
                txtResponsiblePerson.Focus();
                return;
            }
        }
    }

    protected void rdoHierarchy_OnCheckedChanged(object sender, EventArgs e)
    {
        panelHerrachy.Visible = false;
        panelPriority.Visible = false;
        chkTeamLeader.Checked = false;
        chkDepartmentManager.Checked = false;
        chkParentDepartmentManager.Checked = false;
        txtResponsibeDepartmentName.Text = "";
        txtResponsiblePerson.Text = "";
        chkPriority.Checked = false;
        objPageCmn.FillData((object)GvApprovalEmployeeDetail, null, "", "");
        if (rdoHierarchy.Checked)
        {
            panelHerrachy.Visible = true;
            btnsaveConfig.ValidationGroup = "Save";
        }
        if (rdopriority.Checked)
        {
            panelPriority.Visible = true;
            btnsaveConfig.ValidationGroup = "H_Save";
        }
    }

    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtemp = GetApprovalEmployee();
        dtemp = new DataView(dtemp, "Emp_id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)GvApprovalEmployeeDetail, dtemp, "", "");
        dtemp.Dispose();
    }

    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), e.CommandArgument.ToString());
        txtResponsiblePerson.Text = "" + dt.Rows[0]["Emp_Name"].ToString() + "/(" + dt.Rows[0]["Designation"].ToString() + ")/" + dt.Rows[0]["Emp_Code"].ToString() + "";
        txtEmpName_textChanged(null, null);
        dt.Dispose();
    }

    protected void btnAddAppprovalEmployee_Click(object sender, EventArgs e)
    {
        string EmployeeCode = string.Empty;
        if (txtResponsiblePerson.Text != "")
        {
            DataTable dt = GetApprovalEmployee();
            try
            {
                EmployeeCode = txtResponsiblePerson.Text.Split('/')[2].ToString();
            }
            catch
            {
                EmployeeCode = "0";
            }
            string strEmployeeeName = string.Empty;
            string strEmpId = string.Empty;
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Emp_Code='" + EmployeeCode + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmp.Rows.Count > 0)
            {
                strEmpId = dtEmp.Rows[0]["Emp_Id"].ToString();
                strEmployeeeName = dtEmp.Rows[0]["Emp_Name"].ToString();
            }
            DataTable dtTemp = new DataView(dt, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count == 0)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = strEmpId;
                dt.Rows[dt.Rows.Count - 1][1] = EmployeeCode;
                dt.Rows[dt.Rows.Count - 1][2] = strEmployeeeName;
                dt.Rows[dt.Rows.Count - 1][3] = chkPriority.Checked;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() == strEmpId)
                    {
                        dt.Rows[i][3] = chkPriority.Checked;
                        break;
                    }
                }
            }
            objPageCmn.FillData((object)GvApprovalEmployeeDetail, dt, "", "");
            txtResponsiblePerson.Text = "";
            chkPriority.Checked = false;
        }
        else
        {
            DisplayMessage("Enter Employeee Name");
            txtResponsiblePerson.Focus();
            return;
        }
    }

    public DataTable GetApprovalEmployee()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Emp_id");
        dt.Columns.Add("Emp_Code");
        dt.Columns.Add("Emp_name");
        dt.Columns.Add("Priority");
        foreach (GridViewRow gvrow in GvApprovalEmployeeDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            dr[0] = ((Label)gvrow.FindControl("lblEmpId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblgvEmployeeCode")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblgvEmployeeName")).Text;
            dr[3] = ((Label)gvrow.FindControl("lblgvempPriority")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    protected void btnsaveConfig_Click(object sender, EventArgs e)
    {
        string strApprovalType = string.Empty;
        string strAuthorizeddepartment = "0";
        bool IsOpen = false;
        if (!rdoHierarchy.Checked && !rdopriority.Checked)
        {
            DisplayMessage("Select Process Type");
            return;
        }
        if (rdoHierarchy.Checked)
        {
            if (txtResponsibeDepartmentName.Text == "")
            {
                DisplayMessage("Enter authorized department");
                txtResponsibeDepartmentName.Focus();
                return;
            }
            else
            {
                strAuthorizeddepartment = txtResponsibeDepartmentName.Text.Split('/')[1].ToString();
            }
            strApprovalType = "Hierarchy";
        }
        if (rdopriority.Checked)
        {
            if (GvApprovalEmployeeDetail.Rows.Count == 0)
            {
                DisplayMessage("add approval person");
                return;
            }
            DataTable dttemp = GetApprovalEmployee();
            dttemp = new DataView(dttemp, "Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dttemp.Rows.Count == 0)
            {
                DisplayMessage("Priority not assigned");
                return;
            }
            strApprovalType = "Priority";
        }
        if (rdoOpen.Checked)
        {
            IsOpen = true;
        }
        string strsql = string.Empty;
        ObjDa.execute_Command("update set_approvalmaster set Approval_Type='" + strApprovalType + "',Is_TeamLeader='" + chkTeamLeader.Checked.ToString() + "',Is_DepartmentManager='" + chkDepartmentManager.Checked.ToString() + "',Is_Parent_departmentManager='" + chkParentDepartmentManager.Checked.ToString() + "',ResponsibleDepartmentManager='" + strAuthorizeddepartment + "',Is_Open='" + IsOpen.ToString() + "' where approval_id=" + editid.Value + "");
        string EmpAccessPermission = string.Empty;
        EmpAccessPermission = objSys.Get_Approval_Parameter_By_ID(editid.Value).Rows[0]["Approval_Level"].ToString();
        if (EmpAccessPermission == "1")
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), "0", "0", "0", "1");
        }
        else if (EmpAccessPermission == "2")
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", "2");
        }
        else if (EmpAccessPermission == "3")
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "3");
        }
        else
        {
            objApprovalEmp.DeleteEmployeeApproval(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "4");
        }
        if (rdopriority.Checked)
        {
            foreach (GridViewRow gvr in GvApprovalEmployeeDetail.Rows)
            {
                if (EmpAccessPermission == "1")
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), "0", "0", "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else if (EmpAccessPermission == "2")
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else if (EmpAccessPermission == "3")
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                else
                {
                    objApprovalEmp.InsertApprovalMaster(editid.Value, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        objApprovalEmp.DeleteDuplicate_Record(EmpAccessPermission, editid.Value);
        //check pending approval list and update it with new configuration
        SqlTransaction trns = null;
        SqlConnection con = null;
        if (rdopriority.Checked)
        {

            try
            {
                //if (hdnIsPendingApproval.Value == "true")
                //{
                string whereClause = string.Empty;
                if (EmpAccessPermission == "1")
                {
                    whereClause = "Company_id=" + Session["CompId"].ToString();
                }
                else if (EmpAccessPermission == "2")
                {
                    whereClause = "Company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "'";
                }
                else if (EmpAccessPermission == "3")
                {
                    whereClause = "Company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "'";
                }
                else
                {
                    whereClause = "Company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "'";
                }
                string sql = "select * from Set_Approval_Transaction where " + whereClause + " and approval_id='" + editid.Value + "' and Status='Pending' and is_default='true' and IsActive='true'";
                DataTable dtPendings = ObjDa.return_DataTable(sql);
                if (dtPendings.Rows.Count > 0)
                {


                    con = new SqlConnection(Session["DBConnection"].ToString());
                    con.Open();
                    trns = con.BeginTransaction();
                    //update existing records to set is_active=false
                    string sql1 = "update Set_Approval_Transaction set IsActive='false' where trans_id in(select trans_id from Set_Approval_Transaction where " + whereClause + " and approval_id='" + editid.Value + "' and Status='Pending' and IsActive='true')";
                    ObjDa.execute_Command(sql1, ref trns);
                    int oldRefId = 0;
                    foreach (DataRow dr in dtPendings.Rows)
                    {
                        int newRefId = int.Parse(dr["Ref_id"].ToString());
                        if (oldRefId != newRefId)
                        {
                            foreach (GridViewRow gvr in GvApprovalEmployeeDetail.Rows)
                            {
                                objApprovalEmp.InsertApprovalTransaciton(dr["approval_id"].ToString(), dr["company_id"].ToString(), dr["brand_id"].ToString(), dr["location_id"].ToString(), dr["dep_id"].ToString(), dr["request_emp_id"].ToString(), dr["ref_id"].ToString(), ((Label)gvr.FindControl("lblEmpId")).Text, ((Label)gvr.FindControl("lblgvempPriority")).Text, dr["request_date"].ToString(), dr["status_update_date"].ToString(), dr["status"].ToString(), dr["description"].ToString(), dr["field1"].ToString(), dr["field2"].ToString(), dr["field3"].ToString(), dr["field4"].ToString(), dr["field5"].ToString(), dr["field6"].ToString(), dr["field7"].ToString(), true.ToString(), dr["createdBy"].ToString(), dr["CreatedDate"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        oldRefId = newRefId;
                    }
                    trns.Commit();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                if (trns != null)
                {
                    trns.Rollback();
                }
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }
        DisplayMessage("Record Updated Successfully", "green");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
    }

    protected void lnkViewPendings_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string sql = "select em.emp_name as approval_person, am.Approval_Name, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id where am.Approval_Id='" + e.CommandArgument.ToString() + "' and at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name";
            using (DataTable _dt = ObjDa.return_DataTable(sql))
            {
                if (_dt.Rows.Count > 0)
                {
                    gvPendingApprovals.DataSource = _dt;
                    gvPendingApprovals.DataBind();
                    //objPageCmn.FillData((object)gvPendingApprovals, _dt, "", "");
                    lblApprovalType.Text = _dt.Rows[0]["Approval_Name"].ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Pendings()", true);
                }
                else
                {
                    DisplayMessage("There is no pending records");
                }
            }
            
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message);
        }
    }
}