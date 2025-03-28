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
using PegasusDataAccess;

public partial class MasterSetUp_DepartmentMaster : BasePage
{
    ArrayList arr = new ArrayList();
    Common cmn = null;
    SystemParameter objSys = null;
    DepartmentMaster objDep = null;
    LocationMaster objLocation = null;
    Set_Location_Department objLocDept = null;
    IT_ObjectEntry objObjectEntry = null;
    DataAccessClass objda = null;
    EmployeeMaster objEmp = null;
    UserDataPermission objUserDataPerm = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objUserDataPerm = new UserDataPermission(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/DepartmentMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtValue.Focus();
            FillLocation();
            FillGridBin();
            FillGrid();
            FillddlParentDepDDL();
            btnList_Click(null, null);
            Main_Div.Visible = true;
            Div_Move.Visible = false;
            btnSave.Text = Resources.Attendance.Save;
            Session["CHECKED_ITEMS"] = null;
        }
      
    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }


    private void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        DataTable dtLoc = new DataTable();

        dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }

    

    //--------------------------------------------------------------------------------
    protected void btnPushLoc_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushLoc.Focus();
    }
    protected void btnPullLoc_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    DataTable dtDeptEmp = objEmp.GetDepartmentofEmployeeByLocation(Session["CompId"].ToString(), Session["CompId"].ToString(), li.Value.ToString());

                    if (dtDeptEmp.Rows.Count > 0)
                    {
                        dtDeptEmp = new DataView(dtDeptEmp, "Department_Id =" + editid.Value, "", DataViewRowState.CurrentRows).ToTable();
                        if (dtDeptEmp.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            lstLocation.Items.Add(li);
                        }
                    }
                    else
                    {
                        lstLocation.Items.Add(li);
                    }

                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                lstLocationSelect.Items.Remove(li);

            }

        }
        btnPullLoc.Focus();
    }

    protected void btnPushAllLoc_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllLoc.Focus();
    }

    protected void btnPullAllLoc_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            DataTable dtDeptEmp = objEmp.GetDepartmentofEmployeeByLocation(Session["CompId"].ToString(), Session["CompId"].ToString(), li.Value.ToString());

            if (dtDeptEmp.Rows.Count > 0)
            {
                dtDeptEmp = new DataView(dtDeptEmp, "Department_Id =" + editid.Value, "", DataViewRowState.CurrentRows).ToTable();
                if (dtDeptEmp.Rows.Count > 0)
                {

                }
                else
                {
                    lstLocation.Items.Add(li);
                }
            }
            else
            {
                lstLocation.Items.Add(li);
            }

            // lstLocation.Items.Add(li);
        }
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(li);
        }
        btnPullAllLoc.Focus();
    }

    protected void btnSaveLoc_Click(object sender, EventArgs e)
    {

        if (lstLocationSelect.Items.Count == 0)
        {
            DisplayMessage("Select Location");
            return;
        }
        if (editid.Value == string.Empty)
        {
            objLocDept.DeleteLocationMasterByDeptId(ViewState["DeptId"].ToString());
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
            {
                objda.execute_Command("delete from Set_UserDataPermission where (record_type='D' and record_id=" + ViewState["DeptId"].ToString() + ")");
                //objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "L", editid.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }

        }
        else
        {
            objLocDept.DeleteLocationMasterByDeptId(editid.Value);
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
            {
                objda.execute_Command("delete from Set_UserDataPermission where (record_type='D' and record_id=" + editid.Value + ")");
                //objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "L", editid.Value, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

       

        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (editid.Value == string.Empty)
            {
                objLocDept.InsertLocationDepartmentMaster(lstLocationSelect.Items[i].Value, ViewState["DeptId"].ToString(), "0", "", "", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
                {
                    objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "D", ViewState["DeptId"].ToString(), lstLocationSelect.Items[i].Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
            else
            {
                objLocDept.InsertLocationDepartmentMaster(lstLocationSelect.Items[i].Value, editid.Value, "0", "", "", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
                {
                    objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "D", editid.Value, lstLocationSelect.Items[i].Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        Reset();
        btnSave.Text = Resources.Attendance.Save;
        btnList_Click(null, null);
        FillGrid();
        FillLocation();
        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();
        DisplayMessage("Record Saved","green");
        Main_Div.Visible = true;
        Div_Move.Visible = false;
    }

    protected void btnResetLoc_Click(object sender, EventArgs e)
    {
        FillLocation();
        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();
    }
    protected void btnCancelLoc_Click(object sender, EventArgs e)
    {
        FillLocation();
        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
        Main_Div.Visible = true;
        Div_Move.Visible = false;
        btnSave.Text = Resources.Attendance.Save;
    }
    //--------------------------------------------------------------------------------
    public void FillddlParentDepDDL()
    {

        ddlParentDep.Items.Clear();

        DataTable dt = objDep.GetDepartmentMaster();
        dt = new DataView(dt, "", "Dep_Name", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)ddlParentDep, dt, "Dep_Name", "Dep_Id");

    }

    public void FillGrid()
    {
        DataTable dt = objDep.GetDepartmentMaster();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDepMaster, dt, "", "");
    
        Session["dtFilter_Deprt_Mstr"] = dt;
        Session["Dep"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objDep.GetDepartmentMasterInactive();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDepMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinDep"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
       
    }
    protected void gvDepMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDepMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Deprt_Mstr"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDepMaster, dt, "", "");
        
        try
        {
            gvDepMaster.HeaderRow.Focus();
        }
        catch
        {
        }

    }
    protected void gvDepMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Deprt_Mstr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Deprt_Mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDepMaster, dt, "", "");
      
        try
        {
            gvDepMaster.HeaderRow.Focus();
        }
        catch
        {
        }
    }
    protected void txtDeptCode_OnTextChanged(object sender, EventArgs e)
    {
        if (txtDeptCode.Text.Trim() != "")
        {
            if (editid.Value == "")
            {
                DataTable dt = objDep.GetDepartmentMaster();
                try
                {
                    dt = new DataView(dt, "Dep_Code ='" + txtDeptCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt.Rows.Count > 0)
                {
                    txtDeptCode.Text = "";
                    DisplayMessage("Department Code is Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptCode);
                    return;
                }
                DataTable dt1 = objDep.GetDepartmentMasterInactive();
                dt1 = new DataView(dt1, "Dep_Code='" + txtDeptCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    txtDeptCode.Text = "";
                    DisplayMessage("Department Code Already Exists in Bin Section");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptCode);
                    return;
                }
                txtDeptName.Focus();
            }
            else
            {
                DataTable dt = objDep.GetDepartmentMaster();
                try
                {
                    dt = new DataView(dt, "Dep_Code ='" + txtDeptCode.Text.Trim() + "' and Dep_Id<>" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt.Rows.Count > 0)
                {
                    txtDeptCode.Text = "";
                    DisplayMessage("Department Code is Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptCode);
                    return;
                }
                DataTable dt1 = objDep.GetDepartmentMasterInactive();
                dt1 = new DataView(dt1, "Dep_Code='" + txtDeptCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    txtDeptCode.Text = "";
                    DisplayMessage("Department Code Already Exists in Bin Section");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptCode);
                    return;
                }
                txtDeptName.Focus();
            }
        }
    }
    protected void txtDepName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtDeptName.Text.Trim() != "")
        {
            if (editid.Value == "")
            {
                DataTable dt = objDep.GetDepartmentMasterByDepName(txtDeptName.Text.Trim());
                if (dt.Rows.Count > 0)
                {
                    txtDeptName.Text = "";
                    DisplayMessage("Department Name is Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptName);
                    return;
                }
                DataTable dt1 = objDep.GetDepartmentMasterInactive();
                dt1 = new DataView(dt1, "Dep_Name='" + txtDeptName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    txtDeptName.Text = "";
                    DisplayMessage("Department Name Already Exists in Bin Section");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptName);
                    return;
                }
                txtDeptNameL.Focus();
            }
            else
            {
                DataTable dtTemp = objDep.GetDepartmentMasterById(editid.Value);
                if (dtTemp.Rows.Count > 0)
                {
                    if (dtTemp.Rows[0]["Dep_Name"].ToString() != txtDeptName.Text.Trim())
                    {
                        DataTable dt = objDep.GetDepartmentMaster();
                        dt = new DataView(dt, "Dep_Name='" + txtDeptName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt.Rows.Count > 0)
                        {
                            txtDeptName.Text = "";
                            DisplayMessage("Department Name is Already Exists");
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptName);
                            return;
                        }
                        DataTable dt1 = objDep.GetDepartmentMasterInactive();
                        dt1 = new DataView(dt1, "Dep_Name='" + txtDeptName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            txtDeptName.Text = "";
                            DisplayMessage("Department Name Already Exists in Bin Section");
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeptName);
                            return;
                        }
                    }
                }
                txtDeptNameL.Focus();
            }
        }
    }
    protected void gvDepMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvDepMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDepMasterBin, dt, "", "");

        PopulateCheckedValues();
      

    }
    protected void gvDepMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objDep.GetDepartmentMasterInactive();
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDepMasterBin, dt, "", "");

     
        gvDepMasterBin.HeaderRow.Focus();
        PopulateCheckedValues();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        CheckBox chkSelAll = ((CheckBox)gvDepMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvDepMasterBin.Rows)
        {
            index = (int)gvDepMasterBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objDep.GetDepartmentMasterById(editid.Value);
        if (editid.Value == "29")
        {
            DisplayMessage("Record not Editable");
        }
        else
        {
            if (dt.Rows.Count > 0)
            {
                FillddlParentDepDDL();
                try
                {
                    txtDeptCode.Text = dt.Rows[0]["Dep_Code"].ToString();
                    txtDeptName.Text = dt.Rows[0]["Dep_Name"].ToString();
                    txtDeptNameL.Text = dt.Rows[0]["Dep_Name_L"].ToString();
                    ddlParentDep.SelectedValue = dt.Rows[0]["Parent_Id"].ToString();
                }
                catch
                {

                }
                //TabContainer1.ActiveTabIndex = 1;
                btnNew_Click(null, null);
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
                txtDeptCode.Focus();
            }
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        if (editid.Value != "29")
        {
            DataTable dt = new DataTable();
            dt = cmn.GetCheckEsistenceId(editid.Value, "3");

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Record is in Use ,you cannot delete this");
                editid.Value = "";
                return;
            }


            b = objDep.DeleteDepartmentMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        else
        {
            DisplayMessage("Record Can Not Deleted");
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
                ArebicMessage = EnglishMessage;
            }
        }

        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        txtDeptCode.Text = "";
        txtDeptName.Text = "";
        txtDeptNameL.Text = "";
        ddlParentDep.SelectedIndex = 0;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
       // ddlFieldName.SelectedIndex = 0;
        txtDeptCode.Focus();
        BindTreeView();
        FillddlParentDepDDL();
        Main_Div.Visible = true;
        Div_Move.Visible = false;

    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        txtValue.Focus();
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //PnlList.Visible = true;
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {

        txtDeptCode.Focus();
        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        //PnlList.Visible = false;
        //PnlNewEdit.Visible = true;
        Main_Div.Visible = true;
        Div_Move.Visible = false;
        btnSave.Text = Resources.Attendance.Save;
        //PnlBin.Visible = false;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        //pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        //pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlNewEdit.Visible = false;
        //PnlBin.Visible = true;
        //PnlList.Visible = false;
        FillGridBin();
    }

    
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlOption.SelectedIndex = 2;
       // ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
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
            DataTable dtCust = (DataTable)Session["Dep"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Deprt_Mstr"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvDepMaster, view.ToTable(), "", "");
        
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
            DataTable dtCust = (DataTable)Session["dtbinDep"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvDepMasterBin, view.ToTable(), "", "");

           
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        if (gvDepMasterBin.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        Msg = objDep.DeleteDepartmentMaster(userdetails[i].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                    if (Msg != 0)
                    {
                        FillGrid();
                        FillGridBin();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        DisplayMessage("Record Activated");
                        Session["CHECKED_ITEMS"] = null;
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
                return;
            }
        }
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvDepMasterBin.Rows)
            {
                int index = (int)gvDepMasterBin.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvDepMasterBin.Rows)
        {
            index = (int)gvDepMasterBin.DataKeys[gvrow.RowIndex].Value;
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
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtDEPARTMENT.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Dep_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Dep_Id"]));

            }
            foreach (GridViewRow gvrow in gvDepMasterBin.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvDepMasterBin, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }





    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strParentValue = string.Empty;
        int b = 0;
        if (txtDeptCode.Text.Trim() == "")
        {
            DisplayMessage("Enter Department Code");
            txtDeptCode.Focus();
            return;
        }
        if (txtDeptName.Text.Trim() == "")
        {
            DisplayMessage("Enter Department Name");
            txtDeptName.Focus();
            return;
        }
        if (ddlParentDep.SelectedItem.Text == "--Select--")
        {
            strParentValue = "0";
        }
        else
        {
            strParentValue = ddlParentDep.SelectedValue;
        }

        if (editid.Value == "")
        {
            DataTable dt = objDep.GetDepartmentMaster();

            dt = new DataView(dt, "Dep_Code='" + txtDeptCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Department Code is Already Exists");
                txtDeptCode.Focus();
                return;

            }
            DataTable dt1 = objDep.GetDepartmentMaster();

            dt1 = new DataView(dt1, "Dep_Name='" + txtDeptName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Department Name is Already Exists");
                txtDeptName.Focus();
                return;
            }

            b = objDep.InsertDepartmentMaster(txtDeptName.Text.Trim(), txtDeptNameL.Text.Trim(), txtDeptCode.Text.Trim(), strParentValue, "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                ViewState["DeptId"] = b;
                lblDeptId1.Text = txtDeptCode.Text.Trim();
                lblDeptName1.Text = txtDeptName.Text.Trim();
                //PnlNewEdit.Visible = false;
                Main_Div.Visible = false;
                Div_Move.Visible = true;
                // btnSave.Text = Resources.Attendance.Save;

                DisplayMessage("Record Saved","green");
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            DataTable dt = objDep.GetDepartmentMaster();
            string DepartmentCode = string.Empty;
            string DepartmentName = string.Empty;

            try
            {
                DepartmentCode = (new DataView(dt, "Dep_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Dep_Code"].ToString().Trim();
            }
            catch
            {
                DepartmentCode = "";
            }

            dt = new DataView(dt, "Dep_Code='" + txtDeptCode.Text.Trim() + "' and Dep_Code<>'" + DepartmentCode + "' ", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Department Code is Already Exists");
                txtDeptCode.Focus();
                return;

            }



            DataTable dt1 = objDep.GetDepartmentMaster();
            try
            {
                DepartmentName = (new DataView(dt1, "Dep_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Dep_Name"].ToString().Trim();
            }
            catch
            {
                DepartmentName = "";
            }
            dt1 = new DataView(dt1, "Dep_Name='" + txtDeptName.Text.Trim() + "' and Dep_Name<>'" + DepartmentName + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Department Name is Already Exists");
                txtDeptName.Focus();
                return;

            }

            b = objDep.UpdateDepartmentMaster(editid.Value, txtDeptName.Text.Trim(), txtDeptNameL.Text.Trim(), txtDeptCode.Text.Trim(), strParentValue, "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                //////// btnList_Click(null, null);
                //PnlNewEdit.Visible = false;
                Main_Div.Visible = false;
                Div_Move.Visible = true;
                btnSave.Text = Resources.Attendance.Save;
                lstLocationSelect.Items.Clear();
                lstLocationSelect.DataSource = null;
                lstLocationSelect.DataBind();
                lblDeptId1.Text = txtDeptCode.Text;
                lblDeptName1.Text = txtDeptName.Text;
                DisplayMessage("Record Updated", "green");
                //Lbl_Tab_New.Text = Resources.Attendance.New;
                //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

                DataTable dtLoc = objLocDept.GetLocationByDeptId(editid.Value);
                for (int i = 0; i < dtLoc.Rows.Count; i++)
                {
                    try
                    {
                        ListItem li = new ListItem();
                        li.Value = dtLoc.Rows[i]["Location_Id"].ToString();
                        li.Text = objLocation.GetLocationMasterByLocationId(dtLoc.Rows[i]["Location_Id"].ToString()).Rows[0]["Location_Name"].ToString();
                        //DataTable Loc=  objLocation.GetLocationMasterById(Session["CompId"].ToString(), editid.Value);
                        // li.Text = dtLoc.Rows[i]["Location_Name"].ToString();
                        lstLocationSelect.Items.Add(li);
                        lstLocation.Items.Remove(li);
                    }
                    catch
                    {
                    }
                    //Reset();
                    //FillGrid();
                }
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
        Main_Div.Visible = true;
        Div_Move.Visible = false;
        Lbl_Tab_New.Text = Resources.Attendance.New;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepCode(string prefixText, int count, string contextKey)
    {
        DepartmentMaster objDepMaster = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objDepMaster.GetDepartmentMaster(), "Dep_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Dep_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepName(string prefixText, int count, string contextKey)
    {
        DepartmentMaster objDepMaster = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = new DataView(objDepMaster.GetDepartmentMaster(), "Dep_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Dep_Name"].ToString();
        }
        return txt;
    }


    #region TreeViewConcept
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewDepartment.Visible == true)
        {
            TreeViewDepartment.Visible = false;
            gvDepMaster.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvDepMaster.Visible = false;
            TreeViewDepartment.Visible = true;
            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }

    protected void TreeViewDepartment_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewDepartment.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }
    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewDepartment.Visible == true)
        {
            TreeViewDepartment.Visible = false;
            gvDepMaster.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvDepMaster.Visible = false;
            TreeViewDepartment.Visible = true;

        }
        BindTreeView();

        btnTreeView.Focus();
    }

    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewDepartment.Nodes.Clear();
        DataTable dt = new DataTable();

        string x = "Parent_Id=" + "0" + "";


        dt = objDep.GetDepartmentMaster();
        dt = new DataView(dt, x, "Dep_Name asc", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn.Value = dt.Rows[i]["Dep_Id"].ToString();
            TreeViewDepartment.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn);
            i++;
        }
        TreeViewDepartment.DataBind();
    }
    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = objDep.GetAllDepartmentMaster_By_ParentId(index);

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Dep_Name"].ToString();
            tn1.Value = dt.Rows[i]["Dep_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Dep_Id"].ToString()), tn1);
            i++;
        }
        TreeViewDepartment.DataBind();
    }

    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        Reset();
        //panelOverlay.Visible = false;
        //panelPopUpPanel.Visible = false;
        txtValue.Focus();
    }

    protected void btnDeleteChild_Click(object sender, EventArgs e)
    {
        objDep.DeleteDepartmentMaster_ByParentId(editid.Value, "False", Session["userId"].ToString(), DateTime.Now.ToString());
        objDep.DeleteDepartmentMaster(editid.Value, "False", Session["userId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Delete");
        FillGrid();
        //panelOverlay.Visible = false;
        //panelPopUpPanel.Visible = false;
        editid.Value = "";

    }
    protected void btnMoveChild_Click(object sender, EventArgs e)
    {
        btnDeleteChild.Visible = false;
        btnBack.Visible = true;
        btnMoveChild.Visible = false;
        pnlMoveChild.Visible = true;
        FillMoveChildDropDownList(editid.Value);
    }
    protected void btnUpdateParent_Click(object sender, EventArgs e)
    {
        if (ddlMoveCategory.SelectedItem.Text != "No Department Available Here")
        {
            objDep.DeleteDepartmentMaster(editid.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());

            objDep.UpdateParentproject(editid.Value, ddlMoveCategory.SelectedValue, Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Delete and Move Child");
            //panelOverlay.Visible = false;
            //panelPopUpPanel.Visible = false;
            FillGrid();
            editid.Value = "";
        }
    }

    public void FillMoveChildDropDownList(string strExceptId) //Function to fill up items in drop down list of New Parent after delete
    {
        DataTable dt = objDep.GetDepartmentMaster();
        string query = "Dep_Id not in(";

        FindChildNode(strExceptId);

        for (int i = 0; i < arr.Count; i++)
        {
            query += "'" + arr[i].ToString() + "',";
        }
        query = query.Substring(0, query.Length - 1).ToString() + ")";
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();


        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        if (dt.Rows.Count > 0)
        {

            ddlMoveCategory.Items.Clear();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015


            objPageCmn.FillData((object)ddlMoveCategory, dt, "Dep_Name", "Dep_Id");


        }
        else
        {
            ddlMoveCategory.Items.Add("No Department Available Here");
        }

        arr.Clear();
    }

    private void FindChildNode(string p)  //Function to find child nodes and child of child nodes and so on
    {
        arr.Add(p);
        DataTable dt = objDep.GetAllDepartmentMaster_By_ParentId(p);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FindChildNode(dt.Rows[i]["Dep_Id"].ToString());
            }
        }
        else
        {
            return;
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {

        btnDeleteChild.Visible = true;
        btnBack.Visible = false;
        btnMoveChild.Visible = true;
        pnlMoveChild.Visible = false;
    }

    #endregion
}
