using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Collections.Generic;

public partial class ProjectManagement_ProjectMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    ArrayList arr = new ArrayList();
    Prj_ProjectMaster objProjctMaster = null;
    Prj_ProjectTeam objProjectTeam = null;
    Prj_ProjectTask objProjectTask = null;
    SystemParameter ObjSysParam = null;
    Set_CustomerMaster objCustomermaster = null;
    Ems_ContactMaster objContactmaster = null;
    Document_Master ObjDocument = null;
    Inv_SalesOrderHeader objSalesOrderHeader = null;
    Prj_Project_Product objProjectproduct = null;
    Prj_Project_Tools objProjecttools = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster objUnitMaster = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    DataAccessClass objDa = null;
    Set_ApplicationParameter objAppParam = null;
    Prj_Project_Task_Employeee objTaskEmp = null;
    PurchaseOrderDetail ObjPoDetail = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;
    public const int grdDefaultColCount = 6;
    private const string strPageName = "ProjectMaster";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        cmn = new Common(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        objProjectTeam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        objProjectTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objCustomermaster = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objContactmaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objSalesOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objProjectproduct = new Prj_Project_Product(Session["DBConnection"].ToString());
        objProjecttools = new Prj_Project_Tools(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        ObjPoDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ProjectManagement/ProjectMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            Session["PrjMstr_ddlClose_ProjectName"] = null;
            Session["PrjMstr_ddlparentProjectname"] = null;
            Session["PrjMstr_ddlCustomerNamme"] = null;
            Session["PrjMstr_ddlEmployeeName"] = null;
            Session["PrjMstr_ddlContactPersonName"] = null;
            Session["dtProductList"] = null;
            Session["dtToolsList"] = null;
            Session["dtExpList"] = null;

            FillddlLocation();
            LoadProductRecord();
            LoadToolsRecord();
            LoadExpensesRecord();
            BindGrid();
            FillAllProjectList();
            FillCustomerName();
            FillEmployeeName();
            //FillSalesOrderList();

            ddlOption.SelectedIndex = 2;
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();




            //FillprojectList();
            txtProjectNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            txtenddate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            try
            {
                ObjSysParam.getDateForInput("1990-01-01 00:00:00.000").ToString();
            }
            catch
            {
            }
            Session["FileName"] = null;
            getPageControlsVisibility();

        }
        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
        //FillContactPerson();
        fillAllDevExpressList();
    }

    public void fillAllDevExpressList()
    {
        //used to fill ddlCloseProjectName list and ddlProjectName list
        try
        {
            DataTable dt = Session["PrjMstr_ddlClose_ProjectName"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlprojectname, dt, "Project_Name", "");
                objPageCmn.FillData((object)ddlCloseprojectName, dt, "Project_Name", "");
            }
            dt.Dispose();
        }
        catch
        { }
        //used to fill ddlparentProjectname list 
        try
        {
            DataTable dt = Session["PrjMstr_ddlparentProjectname"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlparentProjectname, dt, "Project_Name", "");
            }
            dt.Dispose();
        }
        catch
        { }
        //used to fill ddlCustomerNamme list 
        try
        {
            DataTable dt = Session["PrjMstr_ddlCustomerNamme"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlCustomerNamme, dt, "Name", "");
            }
            dt.Dispose();
        }
        catch
        { }
        //used to fill ddlEmployeeName list 
        try
        {
            DataTable dt = Session["PrjMstr_ddlEmployeeName"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlEmployeeName, dt, "Emp_Name", "");
            }
            dt.Dispose();
        }
        catch
        { }
        //used to fill ddlOrderNo list 
        try
        {
            DataTable dt = Session["PrjMstr_ddlOrderNo"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlOrderNo, dt, "", "");
            }
            dt.Dispose();
        }
        catch
        { }
    }
    #region Combobox
    public void FillContactPerson()
    {
        //ddlContactPerson.Items.Clear();
        DataTable dt = Session["PrjMstr_ddlContactPersonName"] as DataTable;
        try
        {

            //dt = new DataView(dt, "Name like '%" + ddlContactPerson.SelectedItem.Value.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)ddlContactPerson, dt, "", "");

        }
        catch
        {

        }
        dt = null;
    }
    public void FillContactPersonList()
    {
        //ddlContactPerson.Items.Clear();
        DataTable dt = new DataTable();
        if (ddlCustomerNamme.SelectedIndex > 0)
        {
            dt = objContactmaster.GetContactAsPerFilterText("", ddlCustomerNamme.SelectedItem.Value.ToString());
            Session["PrjMstr_ddlContactPersonName"] = dt;
            //objPageCmn.FillData((object)ddlContactPerson, dt, "", "");
        }
        dt.Dispose();
    }
    public void FillSalesOrderList()
    {
        DataTable dtSalesOrder = new DataTable();
        if (hdnCustomerId.Value != "")
        {
            dtSalesOrder = objSalesOrderHeader.GetSalesOrderForProjectManagement(hdnCustomerId.Value.Trim());
            objPageCmn.FillData((object)ddlOrderNo, dtSalesOrder, "", "");
            Session["PrjMstr_ddlOrderNo"] = dtSalesOrder;
        }
        dtSalesOrder = null;
    }
    public void FillCustomerName()
    {
        DataTable dtCustomer = objCustomermaster.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        objPageCmn.FillData((object)ddlCustomerNamme, dtCustomer, "Name", "");
        Session["PrjMstr_ddlCustomerNamme"] = dtCustomer;
        dtCustomer.Dispose();
    }
    public void FillAllProjectList()
    {
        DataTable dtProjectMAster = objProjctMaster.GetAllProjectName_Id_Company();
        try
        {
            dtProjectMAster = new DataView(dtProjectMAster, "Field4='" + Session["CompId"].ToString() + "'", "Project_Name", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        objPageCmn.FillData((object)ddlprojectname, dtProjectMAster, "Project_Name", "");
        objPageCmn.FillData((object)ddlCloseprojectName, dtProjectMAster, "Project_Name", "");
        Session["PrjMstr_ddlClose_ProjectName"] = dtProjectMAster;
        if (hdnPrjectId.Value != "")
        {
            dtProjectMAster = new DataView(dtProjectMAster, "Field4='" + Session["CompId"].ToString() + "'  and Project_id<>" + hdnPrjectId.Value + "", "Project_Name", DataViewRowState.CurrentRows).ToTable();
        }
        objPageCmn.FillData((object)ddlparentProjectname, dtProjectMAster, "Project_Name", "");
        Session["PrjMstr_ddlparentProjectname"] = dtProjectMAster;
        dtProjectMAster.Dispose();
    }
    public void FillEmployeeName()
    {
        EmployeeMaster ObjEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        DataTable dt = ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        objPageCmn.FillData((object)ddlEmployeeName, dt, "Emp_Name", "");
        Session["PrjMstr_ddlEmployeeName"] = dt;
        dt.Dispose();
    }
    #endregion
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnsave.Visible = clsPagePermission.bAdd;
        btnProjectClose.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanTask.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanBug.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();

        //txtstartdate.Enabled = clsPagePermission.bModifyDate;
        //txtexpenddate.Enabled = clsPagePermission.bModifyDate;
        txtenddate.Enabled = clsPagePermission.bModifyDate;
        GvrProjectteam.Columns[5].Visible = clsPagePermission.bUpload;

        if (hdnPrjectId.Value != "")
        {
            TabPanelCost.Visible = clsPagePermission.bShowCostPrice;
            TabPanelPurchaseDetail.Visible = clsPagePermission.bShowCostPrice;
        }
        else
        {
            TabPanelCost.Visible = false;
            TabPanelPurchaseDetail.Visible = false;
        }
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    protected string GetDocumentNumber()
    {
        string s = new Set_DocNumber(Session["DBConnection"].ToString()).GetDocumentNo(true, Session["CompId"].ToString(), true, "156", "121", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    protected void ddlProjectStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
    }
    public void BindGrid()
    {
        DataTable dtProjectMAster = new DataTable();
        if (ddlLocation.SelectedValue.ToString() == "All" && ddlLocation.SelectedItem.Text == "All")
        {
            string strLocationIDs = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            dtProjectMAster = objProjctMaster.GetAllRecordProjectMasteer(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationIDs.Substring(0, strLocationIDs.ToString().Length - 1));
            //dtProjectMAster = new DataView(dtProjectMAster, "Field4 = '" + Session["CompId"].ToString() + "' and Field5 = '" + Session["BrandId"].ToString() + "' and Field6 in (" + strLocationIDs.Substring(0, strLocationIDs.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtProjectMAster = objProjctMaster.GetAllRecordProjectMasteer(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
            //dtProjectMAster = new DataView(dtProjectMAster, "Field4='" + Session["CompId"].ToString() + "' and Field5='" + Session["BrandId"].ToString() + "' and Field6='" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlProjectStatus.SelectedIndex == 1 || ddlProjectStatus.SelectedIndex == 2)
        {
            try
            {
                dtProjectMAster = new DataView(dtProjectMAster, "Project_Title='" + ddlProjectStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (ddlProjectStatus.SelectedIndex == 3)
        {
            try
            {
                dtProjectMAster = new DataView(dtProjectMAster, "Overduedays>0", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (ddlProjectStatus.SelectedIndex == 4)
        {
            try
            {
                dtProjectMAster = new DataView(dtProjectMAster, "Start_Date>'" + DateTime.Now.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        if (dtProjectMAster.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvrProjectteam, dtProjectMAster, "", "");
            Session["dtFilter_Proje_mstr"] = dtProjectMAster;
            Session["dtProjectmaster"] = dtProjectMAster;
        }
        else
        {
            DataTable Dtclear = new DataTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvrProjectteam, Dtclear, "", "");
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtProjectMAster.Rows.Count + "";
        dtProjectMAster.Dispose();
        //AllPageCode();
        BindTreeView(dtProjectMAster);
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
    public string Formatdate(object Date)
    {
        string newdate = "";
        string set_date = "";
        Date = Convert.ToDateTime(Date).ToString(ObjSysParam.GetSysParameterByParamName("Date_Format").Rows[0]["Param_Value"].ToString());
        newdate = Convert.ToString(Date);
        if (newdate == "01-01-1990" || newdate.ToString() == "01-Jan-1990")
        {
            set_date = "";
        }
        else
        {
            set_date = Convert.ToString(Date);
        }
        return set_date;
    }
    private void download(DataTable dt)
    {
        Byte[] bytes = (Byte[])dt.Rows[0]["File_Data"];
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //Response.ContentType = dt.Rows[0]["ContentType"].ToString();
        Response.AddHeader("content-disposition", "attachment;filename="
        + dt.Rows[0]["File_Name"].ToString());
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }
    public void Reset()
    {
        txtprojectname.Text = "";
        txtprojectlocalname.Text = "";
        ddlCustomerNamme.SelectedIndex = 0;
        txtprojecttype.Text = "";
        txtstartdate.Text = "";
        txtexpenddate.Text = "";
        hdnPrjectId.Value = "";
        Editor1.Content = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtprojectname);
        ddlprojectname.SelectedIndex = 0;
        //txtDDlProjectName.Text = "";
        ddlOrderNo.SelectedIndex = 0;
        ddlOrderNo.Enabled = true;
        chkMultipledemo.Checked = false;
        ddlprojectname.Enabled = true;
        Session["dtProductList"] = null;
        Session["dtToolsList"] = null;
        Session["dtExpList"] = null;
        LoadProductRecord();
        LoadToolsRecord();
        LoadExpensesRecord();
        tabContainer.ActiveTabIndex = 1;
        TabPanelproduct.Visible = true;
        TabPaneltools.Visible = true;
        TabPanelExpenses.Visible = true;
        ddlparentProjectname.SelectedIndex = 0;
        txtProjectNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        hdnPrjectId.Value = "";
        txtexpenddate.Enabled = true;
        Session["ContactID"] = null;
        ddlContactPerson.Text = "";
        ddlEmployeeName.SelectedIndex = 0;
        lblValueActualCost.Text = "0";
        lblValueExpectedCost.Text = "0";
        trImport.Visible = false;
        chkImportRefProjectTask.Checked = false;
        chkImportRefProjectTeam.Checked = false;
    }
    protected void GvrProjectteam_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Proje_mstr"];
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
        Session["dtFilter_Proje_mstr"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dt, "", "");
        BindTreeView(dt);
        dt.Dispose();
        //AllPageCode();
    }
    protected void GvrProjectteam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvrProjectteam.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Proje_mstr"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dt, "", "");
        //AllPageCode();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        GvrProjectteam.PageIndex = 0;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        AutoCompleteExtender_ProjectTypeSuggestion.Enabled = false;
        BindGrid();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        DataTable dt = new DataTable();
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
            DataTable dtProjectteam = (DataTable)Session["dtProjectmaster"];
            if (dtProjectteam != null)
            {
                DataView view = new DataView(dtProjectteam, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)GvrProjectteam, view.ToTable(), "", "");
                Session["dtFilter_Proje_mstr"] = view.ToTable();
                dt = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                //AllPageCode();
            }
        }
        BindTreeView(dt);
        dt.Dispose();
        txtValue.Focus();
    }
    protected void ddlFieldName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedValue.Trim() == "Project_Type")
        {
            AutoCompleteExtender_ProjectTypeSuggestion.Enabled = true;
        }
        else
        {
            AutoCompleteExtender_ProjectTypeSuggestion.Enabled = false;
        }
    }
    public void btnsave_Click(object sender, EventArgs e)
    {
        btnsave.Enabled = false;
        string strsalesOrderid = string.Empty;
        string strContactPersonId = string.Empty;
        string strprojectManagerId = string.Empty;
        string strReProjectId = string.Empty;
        string strparentProjectId = string.Empty;
        string strcustomerId = string.Empty;
        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }
        //here we are cchecking that sales order selecred or not 
        if (ddlOrderNo.SelectedIndex > 0)
        {
            strsalesOrderid = ddlOrderNo.Value.ToString();
        }
        else
        {
            strsalesOrderid = "0";
        }
        if (ddlCustomerNamme.SelectedIndex > 0)
        {
            strcustomerId = ddlCustomerNamme.Value.ToString();
        }
        else
        {
            DisplayMessage("Please Add Customer Name");
            ddlCustomerNamme.Focus();
            btnsave.Enabled = true;
            return;

        }
        if (txtProjectNo.Text.Trim() == "")
        {
            DisplayMessage("Project no. not found , you can set in document number page");
            txtProjectNo.Focus();
            btnsave.Enabled = true;
            return;
        }
        if (txtprojecttype.Text == "")
        {
            DisplayMessage("Enter Project Type");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtprojecttype);
            btnsave.Enabled = true;
            return;
        }
        if (txtprojectname.Text == "")
        {
            DisplayMessage("Enter Project Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtprojectname);
            btnsave.Enabled = true;
            return;
        }
        else
        {
            DataTable dtPrjectRecord = new DataTable();
            dtPrjectRecord = objProjctMaster.GetAllProjectMasteer();
            if (hdnPrjectId.Value != "")
            {
                dtPrjectRecord = new DataView(dtPrjectRecord, "Project_Name='" + txtprojectname.Text.Trim() + "' and Project_id<>" + hdnPrjectId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtPrjectRecord = new DataView(dtPrjectRecord, "Project_Name='" + txtprojectname.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtPrjectRecord.Rows.Count > 0)
            {
                DisplayMessage("Project Name Already exists");
                txtprojectname.Focus();
                btnsave.Enabled = true;
                return;
            }
        }
        if (ddlprojectname.SelectedIndex > 0)
        {
            strReProjectId = ddlprojectname.SelectedItem.Value.ToString();
        }
        else
        {
            strReProjectId = "0";
        }
        if (ddlparentProjectname.SelectedIndex > 0)
        {
            strparentProjectId = ddlparentProjectname.Value.ToString();
        }
        else
        {
            strparentProjectId = "0";
        }
        if (ddlEmployeeName.SelectedIndex > 0)
        {
            strprojectManagerId = ddlEmployeeName.Value.ToString();
        }
        else
        {
            DisplayMessage("Enter Project Manager");
            ddlEmployeeName.Focus();
            btnsave.Enabled = true;
            return;
        }
        if (txtstartdate.Text == "")
        {
            DisplayMessage("Enter Start Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtstartdate);
            btnsave.Enabled = true;
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtstartdate.Text);
            }
            catch
            {
                DisplayMessage("Enter Start Date in format " + Session["DateFormat"].ToString() + "");
                txtstartdate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtstartdate);
                btnsave.Enabled = true;
                return;
            }
        }
        if (txtexpenddate.Text == "")
        {
            DisplayMessage("Enter Expected End Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtexpenddate);
            btnsave.Enabled = true;
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtexpenddate.Text);
            }
            catch
            {
                DisplayMessage("Enter Expected End Date in format " + Session["DateFormat"].ToString() + "");
                txtexpenddate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtexpenddate);
                btnsave.Enabled = true;
                return;
            }
        }
        if (ObjSysParam.getDateForInput(txtstartdate.Text) > ObjSysParam.getDateForInput(txtexpenddate.Text))
        {
            DisplayMessage("Expected End Date Is Not Valid");
            txtexpenddate.Focus();
            btnsave.Enabled = true;
            return;
        }
        if (ddlContactPerson.Text != "")
        {
            strContactPersonId = ddlContactPerson.Text.Split('/')[ddlContactPerson.Text.Split('/').Length - 1].ToString();
        }
        else
        {
            strContactPersonId = "0";
        }


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            if (hdnPrjectId.Value != "")
            {
                //heere we are retreving end date from database
                //code start
                DataTable dtPrjectRecord = new DataTable();
                dtPrjectRecord = objProjctMaster.GetAllProjectMasteer(hdnPrjectId.Value.ToString(), ref trns);
                //dtPrjectRecord = new DataView(dtPrjectRecord, "Project_Id=" + hdnPrjectId.Value.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                string strprojectEnddate = dtPrjectRecord.Rows[0]["End_Date"].ToString();
                string strRemarks = dtPrjectRecord.Rows[0]["File_Id"].ToString();
                dtPrjectRecord.Dispose();
                //code end
                objProjctMaster.UpdateProjcetMaster(hdnPrjectId.Value.ToString(), txtprojectname.Text, txtprojectlocalname.Text, txtprojecttype.Text, strcustomerId, strContactPersonId, ObjSysParam.getDateForInput(txtstartdate.Text).ToString(), ObjSysParam.getDateForInput(txtexpenddate.Text).ToString(), ObjSysParam.getDateForInput(strprojectEnddate).ToString(), ddlStatus.SelectedValue, Editor1.Content, strprojectManagerId, strsalesOrderid, strRemarks, strparentProjectId, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //this code for insert product information
                //code start
                //first delete record by project id
                objProjectproduct.DeleteRecord_ByProjectId(hdnPrjectId.Value.ToString(), ref trns);
                if (Session["dtProductList"] != null)
                {
                    DataTable dtProduct = (DataTable)Session["dtProductList"];
                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        objProjectproduct.InsertRecord(hdnPrjectId.Value.ToString(), dr["Product_Id"].ToString(), dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                //this code for insert Tools information
                //code start
                //first delete record by project id
                objProjecttools.DeleteRecord_ByReftypeandrefId("PROJECT", hdnPrjectId.Value.ToString(), ref trns);
                if (Session["dtToolsList"] != null)
                {
                    string Toolsid = string.Empty;
                    DataTable dtProduct = (DataTable)Session["dtToolsList"];
                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        if (dr["Tools_Id"].ToString() == "0")
                        {
                            Toolsid = dr["ProductCode"].ToString();
                        }
                        else
                        {
                            Toolsid = dr["Tools_Id"].ToString();
                        }
                        objProjecttools.InsertRecord(hdnPrjectId.Value.ToString(), Toolsid, dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), dr["IsToolsExists"].ToString(), "PROJECT", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                //this code for sav expenes
                //code start
                ObjShipExpDetail.ShipExpDetail_Delete("0", "0", "0", hdnPrjectId.Value, "PM", ref trns);
                if (Session["dtExpList"] != null)
                {
                    double companyCurrrencyAmt = 0;
                    string strExchnageRate = string.Empty;
                    foreach (DataRow dr in ((DataTable)Session["dtExpList"]).Rows)
                    {
                        if (dr["FCExpAmount"].ToString() == "")
                        {
                            dr["FCExpAmount"] = "0";
                        }
                        try
                        {
                            strExchnageRate = SystemParameter.GetExchageRate(dr["ExpCurrencyID"].ToString(), Session["CurrencyId"].ToString(), Session["DBConnection"].ToString());
                        }
                        catch
                        {
                            strExchnageRate = "0";
                        }
                        companyCurrrencyAmt = Convert.ToDouble(dr["FCExpAmount"].ToString()) * Convert.ToDouble(strExchnageRate);
                        ObjShipExpDetail.ShipExpDetail_Insert("0", "0", "0", hdnPrjectId.Value, dr["Expense_Id"].ToString(), "0", companyCurrrencyAmt.ToString(), dr["ExpCurrencyID"].ToString(), strExchnageRate, dr["FCExpAmount"].ToString(), "PM", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                //here we are inserting record in project team  as a manager
                //but need to check that task assinged or not of assinged then we can not delete from team as a  mormal employee
                DataTable dt = objProjectTeam.GetRecordByProjectId("", hdnPrjectId.Value, ref trns);
                dt = new DataView(dt, "Field1='Project Manager'", "", DataViewRowState.CurrentRows).ToTable();
                try
                {
                    if (strprojectManagerId.Trim() != dt.Rows[0]["Emp_Id"].ToString())
                    {
                        DataTable dtProjecttask = objProjectTask.GetRecordTaskVisibilityandProjectId(dt.Rows[0]["Emp_Id"].ToString(), hdnPrjectId.Value, ref trns);
                        if (dtProjecttask.Rows.Count == 0)
                        {
                            objProjectTeam.DeleteProjectTeam(dt.Rows[0]["Trans_Id"].ToString(), ref trns);
                        }
                        else
                        {
                            objDa.execute_Command("update Prj_Project_Team set Field1=' ' where Project_Id=" + hdnPrjectId.Value + "", ref trns);
                        }
                        if (new DataView(objProjectTeam.GetRecordByProjectId("", hdnPrjectId.Value, ref trns), "Emp_Id=" + strprojectManagerId + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                        {
                            objDa.execute_Command("update Prj_Project_Team set Field1='Project Manager',Task_Visibility='True' where Project_Id=" + hdnPrjectId.Value + " and Emp_id=" + strprojectManagerId + "", ref trns);
                        }
                        else
                        {
                            objProjectTeam.InsertProjectTeam(hdnPrjectId.Value, strprojectManagerId, "True", "Project Manager", GetEmployeeInfo(strprojectManagerId, ref trns)[1].ToString(), GetEmployeeInfo(strprojectManagerId, ref trns)[2].ToString(), GetEmployeeInfo(strprojectManagerId, ref trns)[0].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                catch
                {
                    if (new DataView(objProjectTeam.GetRecordByProjectId("", hdnPrjectId.Value, ref trns), "Emp_Id=" + strprojectManagerId + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        objDa.execute_Command("update Prj_Project_Team set Field1='Project Manager',Task_Visibility='True' where Project_Id=" + hdnPrjectId.Value + " and Emp_id=" + strprojectManagerId + "", ref trns);
                    }
                    else
                    {
                        objProjectTeam.InsertProjectTeam(hdnPrjectId.Value, strprojectManagerId, "True", "Project Manager", GetEmployeeInfo(strprojectManagerId, ref trns)[1].ToString(), GetEmployeeInfo(strprojectManagerId, ref trns)[2].ToString(), GetEmployeeInfo(strprojectManagerId, ref trns)[0].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                DisplayMessage("Record Updated", "green");
                hdnPrjectId.Value = "";
                ddlCustomerNamme.SelectedIndex = 0;
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {

                int a = 0;
                a = objProjctMaster.InsertProjectMaster(txtprojectname.Text, txtprojectlocalname.Text, txtprojecttype.Text, strcustomerId, strContactPersonId, ObjSysParam.getDateForInput(txtstartdate.Text).ToString(), ObjSysParam.getDateForInput(txtexpenddate.Text).ToString(), ObjSysParam.getDateForInput("01-01-1990").ToString(), "Open", Editor1.Content, strprojectManagerId, "", strReProjectId, strsalesOrderid, strparentProjectId, Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (txtProjectNo.Text == ViewState["DocNo"].ToString())
                {
                    DataTable dtCount = objProjctMaster.GetAllRecordProjectMaster_For_DocumentNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                    try
                    {
                        //dtCount = new DataView(dtCount, "Field4='" +  + "' and Field5='" +  + "' and Field6='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtCount.Rows[0][0].ToString() == "0")
                    {
                        objProjctMaster.Updatecode(a.ToString(), txtProjectNo.Text + "1", ref trns);
                        txtProjectNo.Text = txtProjectNo.Text + "1";
                    }
                    else
                    {
                        objProjctMaster.Updatecode(a.ToString(), txtProjectNo.Text + dtCount.Rows[0][0].ToString(), ref trns);
                        txtProjectNo.Text = txtProjectNo.Text + dtCount.Rows.Count;
                    }
                }
                //this code for insert product information
                //code start
                if (Session["dtProductList"] != null)
                {
                    DataTable dtProduct = (DataTable)Session["dtProductList"];
                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        objProjectproduct.InsertRecord(a.ToString(), dr["Product_Id"].ToString(), dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                //this code for insert Tools information
                //code start
                if (Session["dtToolsList"] != null)
                {
                    string Toolsid = string.Empty;
                    DataTable dtProduct = (DataTable)Session["dtToolsList"];
                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        if (dr["Tools_Id"].ToString() == "0")
                        {
                            Toolsid = dr["ProductCode"].ToString();
                        }
                        else
                        {
                            Toolsid = dr["Tools_Id"].ToString();
                        }
                        objProjecttools.InsertRecord(a.ToString(), Toolsid, dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), dr["IsToolsExists"].ToString(), "PROJECT", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                //this code for sav expenes
                //code start
                if (Session["dtExpList"] != null)
                {
                    double companyCurrrencyAmt = 0;
                    string strExchnageRate = string.Empty;
                    foreach (DataRow dr in ((DataTable)Session["dtExpList"]).Rows)
                    {
                        if (dr["FCExpAmount"].ToString() == "")
                        {
                            dr["FCExpAmount"] = "0";
                        }
                        try
                        {
                            strExchnageRate = SystemParameter.GetExchageRate(dr["ExpCurrencyID"].ToString(), Session["CurrencyId"].ToString(), ref trns);
                        }
                        catch
                        {
                            strExchnageRate = "0";
                        }
                        companyCurrrencyAmt = Convert.ToDouble(dr["FCExpAmount"].ToString()) * Convert.ToDouble(strExchnageRate);
                        ObjShipExpDetail.ShipExpDetail_Insert("0", "0", "0", a.ToString(), dr["Expense_Id"].ToString(), "0", companyCurrrencyAmt.ToString(), dr["ExpCurrencyID"].ToString(), strExchnageRate, dr["FCExpAmount"].ToString(), "PM", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                if (ddlprojectname.SelectedIndex > 0)
                {
                    //for create task according selected project
                    if (chkImportRefProjectTask.Checked)
                    {
                        DataTable dtprojecttask = objDa.return_DataTable("select * from prj_project_task where Project_Id=" + ddlprojectname.SelectedItem.Value.ToString() + " and  Prj_Project_Task.Field3='0' and  Prj_Project_Task.Field6 in (select Prj_Project_TaskTypeMaster.Trans_Id from Prj_Project_TaskTypeMaster where Prj_Project_TaskTypeMaster.Is_Bug='False')", ref trns);
                        foreach (DataRow dr in dtprojecttask.Rows)
                        {
                            int b = objProjectTask.InsertProjectTask(a.ToString(), "0", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", dr["Subject"].ToString(), dr["Description"].ToString(), dr["File_Id"].ToString(), "Assigned", "00:00", "", dr["Field3"].ToString(), dr["Field4"].ToString(), "0", dr["Field6"].ToString(), "0", dr["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dr["Task_Type"].ToString(), dr["Task_Site_Address"].ToString(), dr["Contact_Person"].ToString(), "0", "", "0", ref trns);
                            DataTable dtTaskEmployee = objDa.return_DataTable("select * from  Prj_Project_Task_Employeee where Ref_Type='Task' and Ref_Id=" + dr["Task_Id"].ToString() + "", ref trns);
                            foreach (DataRow drchild in dtTaskEmployee.Rows)
                            {
                                objTaskEmp.InsertRecord("Task", b.ToString(), drchild["Employee_Id"].ToString(), "", "", "", "", "", "", "", drchild["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            childNodeSave(a.ToString(), dr["Task_Id"].ToString(), b.ToString(), ref trns);
                        }
                    }
                    if (chkImportRefProjectTeam.Checked)
                    {
                        //for create project team 
                        DataTable dtProjectTeam = objProjectTeam.GetAllRecord(strReProjectId, ref trns);
                        dtProjectTeam = new DataView(dtProjectTeam, "Is_Terminated = False", "", DataViewRowState.CurrentRows).ToTable();
                        if (new DataView(dtProjectTeam, "Emp_id=" + ddlEmployeeName.Value.ToString() + " and Field1='Project Manager'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtProjectTeam.Rows)
                            {
                                objProjectTeam.InsertProjectTeam(a.ToString(), dr["Emp_Id"].ToString(), dr["Task_Visibility"].ToString(), dr["Field1"].ToString(), dr["Field2"].ToString(), dr["Field3"].ToString(), dr["Field4"].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                        else
                        {
                            dtProjectTeam = new DataView(dtProjectTeam, "Field1<>'Project Manager'", "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dtProjectTeam.Rows)
                            {
                                if (dr["Emp_Id"].ToString() == ddlEmployeeName.Value.ToString())
                                {
                                    continue;
                                }
                                objProjectTeam.InsertProjectTeam(a.ToString(), dr["Emp_Id"].ToString(), dr["Task_Visibility"].ToString(), dr["Field1"].ToString(), dr["Field2"].ToString(), dr["Field3"].ToString(), dr["Field4"].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            objProjectTeam.InsertProjectTeam(a.ToString(), ddlEmployeeName.Value.ToString(), true.ToString(), "Project Manager", "00:00:00", "00:00:000", "0", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                else
                {
                    //here we are inserting record in project team for project manager
                    objProjectTeam.InsertProjectTeam(a.ToString(), strprojectManagerId, "True", "Project Manager", GetEmployeeInfo(strprojectManagerId, ref trns)[1].ToString(), GetEmployeeInfo(strprojectManagerId, ref trns)[2].ToString(), GetEmployeeInfo(strprojectManagerId, ref trns)[0].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                DisplayMessage("Record Saved", "green");
                hdnPrjectId.Value = "";
                ddlCustomerNamme.SelectedIndex = 0;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtprojectname);
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            BindGrid();
            Reset();
        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
        }

        btnsave.Enabled = true;
    }
    public void childNodeSave(string strProjectId, string strRefTaskId, string strtaskId)
    {
        DataTable dtprojecttask = objDa.return_DataTable("select * from prj_project_task where Project_Id=" + ddlprojectname.SelectedItem.Value.ToString() + " and  Prj_Project_Task.Field3='" + strRefTaskId + "' and  Prj_Project_Task.Field6 in (select Prj_Project_TaskTypeMaster.Trans_Id from Prj_Project_TaskTypeMaster where Prj_Project_TaskTypeMaster.Is_Bug='False')");
        int i = 0;
        while (i < dtprojecttask.Rows.Count)
        {
            int b = objProjectTask.InsertProjectTask(strProjectId, "0", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", dtprojecttask.Rows[i]["Subject"].ToString(), dtprojecttask.Rows[i]["Description"].ToString(), dtprojecttask.Rows[i]["File_Id"].ToString(), "Assigned", "00:00", "", strtaskId, dtprojecttask.Rows[i]["Field4"].ToString(), "0", dtprojecttask.Rows[i]["Field6"].ToString(), "0", dtprojecttask.Rows[i]["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dtprojecttask.Rows[i]["Task_Type"].ToString(), dtprojecttask.Rows[i]["Task_Site_Address"].ToString(), dtprojecttask.Rows[i]["Contact_Person"].ToString(), "0", "", "0");
            DataTable dtTaskEmployee = objDa.return_DataTable("select * from  Prj_Project_Task_Employeee where Ref_Type='Task' and Ref_Id=" + dtprojecttask.Rows[i]["Task_Id"].ToString() + "");
            foreach (DataRow drchild in dtTaskEmployee.Rows)
            {
                objTaskEmp.InsertRecord("Task", b.ToString(), drchild["Employee_Id"].ToString(), "", "", "", "", "", "", "", drchild["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            childNodeSave(strProjectId, dtprojecttask.Rows[i]["Task_Id"].ToString(), b.ToString());
            i++;
        }
    }
    public void childNodeSave(string strProjectId, string strRefTaskId, string strtaskId, ref SqlTransaction trans)
    {
        DataTable dtprojecttask = objDa.return_DataTable("select * from prj_project_task where Project_Id=" + ddlprojectname.SelectedItem.Value.ToString() + " and  Prj_Project_Task.Field3='" + strRefTaskId + "' and  Prj_Project_Task.Field6 in (select Prj_Project_TaskTypeMaster.Trans_Id from Prj_Project_TaskTypeMaster where Prj_Project_TaskTypeMaster.Is_Bug='False')", ref trans);
        int i = 0;
        while (i < dtprojecttask.Rows.Count)
        {
            int b = objProjectTask.InsertProjectTask(strProjectId, "0", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", "1/1/1900", dtprojecttask.Rows[i]["Subject"].ToString(), dtprojecttask.Rows[i]["Description"].ToString(), dtprojecttask.Rows[i]["File_Id"].ToString(), "Assigned", "00:00", "", strtaskId, dtprojecttask.Rows[i]["Field4"].ToString(), "0", dtprojecttask.Rows[i]["Field6"].ToString(), "0", dtprojecttask.Rows[i]["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dtprojecttask.Rows[i]["Task_Type"].ToString(), dtprojecttask.Rows[i]["Task_Site_Address"].ToString(), dtprojecttask.Rows[i]["Contact_Person"].ToString(), "0", "", "0", ref trans);
            DataTable dtTaskEmployee = objDa.return_DataTable("select * from  Prj_Project_Task_Employeee where Ref_Type='Task' and Ref_Id=" + dtprojecttask.Rows[i]["Task_Id"].ToString() + "", ref trans);
            foreach (DataRow drchild in dtTaskEmployee.Rows)
            {
                objTaskEmp.InsertRecord("Task", b.ToString(), drchild["Employee_Id"].ToString(), "", "", "", "", "", "", "", drchild["IsActive"].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trans);
            }
            childNodeSave(strProjectId, dtprojecttask.Rows[i]["Task_Id"].ToString(), b.ToString(), ref trans);
            i++;
        }
    }
    public void btncencel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void btnreset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    public string[] GetEmployeeInfo(string strEmpId)
    {
        string[] strEmp = new string[3];
        try
        {
            strEmp[0] = new EmployeeParameter(Session["DBConnection"].ToString()).GetEmployeeParameterByEmpId(strEmpId, Session["CompId"].ToString()).Rows[0]["Basic_Salary"].ToString();
        }
        catch
        {
            strEmp[0] = "0";
        }
        try
        {
            DataTable dtShiftAllDate = new Att_ScheduleMaster(Session["DBConnection"].ToString()).GetSheduleDescription(strEmpId);
            dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + DateTime.Now.ToString() + "' and Att_Date<='" + DateTime.Now.ToString() + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
            if (dtShiftAllDate.Rows.Count > 0)
            {
                strEmp[1] = dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString();
                strEmp[2] = dtShiftAllDate.Rows[0]["OffDuty_Time"].ToString();
            }
            else
            {
                strEmp[1] = "00:00";
                strEmp[2] = "00:00";
            }
            if (strEmp[1] == "")
            {
                strEmp[1] = "00:00";
            }
            if (strEmp[2] == "")
            {
                strEmp[2] = "00:00";
            }
        }
        catch
        {
            strEmp[1] = "00:00";
            strEmp[2] = "00:00";
        }
        return strEmp;
    }
    public string[] GetEmployeeInfo(string strEmpId, ref SqlTransaction trans)
    {
        string[] strEmp = new string[3];
        try
        {
            strEmp[0] = new EmployeeParameter(Session["DBConnection"].ToString()).GetEmployeeParameterByEmpId(strEmpId, Session["CompId"].ToString(), ref trans).Rows[0]["Basic_Salary"].ToString();
        }
        catch
        {
            strEmp[0] = "0";
        }
        try
        {
            DataTable dtShiftAllDate = new Att_ScheduleMaster(Session["DBConnection"].ToString()).GetSheduleDescription(strEmpId, ref trans);
            dtShiftAllDate = new DataView(dtShiftAllDate, "Att_Date>='" + DateTime.Now.ToString() + "' and Att_Date<='" + DateTime.Now.ToString() + "'", "Att_Date", DataViewRowState.CurrentRows).ToTable();
            if (dtShiftAllDate.Rows.Count > 0)
            {
                strEmp[1] = dtShiftAllDate.Rows[0]["OnDuty_Time"].ToString();
                strEmp[2] = dtShiftAllDate.Rows[0]["OffDuty_Time"].ToString();
            }
            else
            {
                strEmp[1] = "00:00";
                strEmp[2] = "00:00";
            }
            if (strEmp[1] == "")
            {
                strEmp[1] = "00:00";
            }
            if (strEmp[2] == "")
            {
                strEmp[2] = "00:00";
            }
        }
        catch
        {
            strEmp[1] = "00:00";
            strEmp[2] = "00:00";
        }
        return strEmp;
    }
    protected void txtcustomername_TextChanged(object sender, EventArgs e)
    {
        FillContactPersonList();
        if (ddlCustomerNamme.SelectedItem.Value == null)
        {
            return;
        }


        hdnCustomerId.Value = ddlCustomerNamme.SelectedItem.Value.ToString();
        FillSalesOrderList();
        ddlContactPerson.Text = "";
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();
        string filtertext = "Name like '" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtCustomer;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetProjectType(string prefixText, int count, string contextKey)
    {
        Prj_ProjectMaster objProjctMaster = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objProjctMaster.GetAllProjectMasteerByPrefix(prefixText);
        string[] str = new string[0];
        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i][0].ToString();
                }
            }
            else
            {
                return null;
            }

            //else
            //{
            //    if (prefixText.Length > 2)
            //    {
            //        str = null;
            //    }
            //    else
            //    {
            //        dt = objProjctMaster.GetAllProjectMasteer();
            //        dt = dt.DefaultView.ToTable(true, "Project_Type");
            //        if (dt.Rows.Count > 0)
            //        {
            //            str = new string[dt.Rows.Count];
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //            {
            //                str[i] = dt.Rows[i][0].ToString();
            //            }
            //        }
            //    }
            //}
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetProjectName(string prefixText, int count, string contextKey)
    {
        DataTable dt = HttpContext.Current.Session["PrjMstr_ddlClose_ProjectName"] as DataTable;
        dt = new DataView(dt, "project_name like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] str = new string[0];
        if (dt != null)
        {

            if (dt.Rows.Count > 10)
            {
                str = new string[10];
                for (int i = 0; i < 10; i++)
                {
                    str[i] = dt.Rows[i]["project_name"].ToString();
                }
            }
            else
            {
                str = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["project_name"].ToString();
                }
            }
        }
        dt = null;
        return str;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = cmn.GetCheckEsistenceId(e.CommandArgument.ToString(), "9");
        if (dt.Rows.Count > 0)
        {
            DisplayMessage(" You Can Not Delete  This Record Is Currently Used");
            return;
        }
        dt.Dispose();
        DataTable dtChildCategory = objProjctMaster.GetAllProjectMaster_By_ParentId(e.CommandArgument.ToString());
        if (dtChildCategory.Rows.Count == 0)
        {
            objProjctMaster.DeleteProjectMaster(e.CommandArgument.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());
            BindGrid();
            DisplayMessage("Record Deleted", "green");
        }
        else
        {
            hdnPrjectId.Value = e.CommandArgument.ToString();
            btnBack.Visible = false;
            btnDeleteChild.Visible = true;
            btnMoveChild.Visible = true;
        }
    }
    protected void btnBugs_command(object sender, CommandEventArgs e)
    {
        if (!CheckTaskExistorNot(e.CommandName.ToString(), true))
        {
            DisplayMessage("Record Not Found");
            return;
        }
        string strCmd = string.Format("window.open('../ProjectManagement/Projecttask.aspx?Task=0&&Project_Id=" + e.CommandName.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public bool CheckTaskExistorNot(string strProjectId, bool IsBug)
    {
        bool Result = false;
        DataTable dtProjecttask = new DataTable();
        if (!IsBug)
        {
            dtProjecttask = objProjectTask.GetRecordTaskVisibilityTrueWithoutBugs(Session["EmpId"].ToString(), strProjectId);
        }
        else
        {
            dtProjecttask = objProjectTask.GetRecordTaskVisibilityTrueWithBugs(Session["EmpId"].ToString(), strProjectId);
        }
        ///here we are checking project filter
        //dtProjecttask = new DataView(dtProjecttask, "Project_Id='" + strProjectId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtProjecttask.Rows.Count > 0)
        {
            Result = true;
        }
        dtProjecttask.Dispose();
        return Result;
    }
    protected void lnkProjectName_command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Dashboard/Individual_Project_Dashboard.aspx?Project_Id=" + e.CommandName.ToString() + "')", true);
    }
    protected void btnTask_command(object sender, CommandEventArgs e)
    {
        if (!CheckTaskExistorNot(e.CommandName.ToString(), false))
        {
            DisplayMessage("Record Not Found");
            return;
        }
        string strCmd = string.Format("window.open('../ProjectManagement/Projecttask.aspx?Project_Id=" + e.CommandName.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        hdnPrjectId.Value = "";
        hdnPrjectId.Value = e.CommandArgument.ToString();
        DataTable dtPrjectRecord = new DataTable();
        dtPrjectRecord = objProjctMaster.GetAllProjectMasteer(hdnPrjectId.Value.ToString());
        //dtPrjectRecord = new DataView(dtPrjectRecord, "Project_Id=" + hdnPrjectId.Value.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //dtPrjectRecord = objProjctMaster.GetRecordByProjectId(hdnPrjectId.Value.ToString());
        if (dtPrjectRecord.Rows.Count > 0)
        {
            FillSalesOrderList();
            if (ddlEmployeeName.Items.FindByValue(dtPrjectRecord.Rows[0]["Project_Manager"].ToString()) != null)
            {
                ddlEmployeeName.Value = dtPrjectRecord.Rows[0]["Project_Manager"].ToString();
            }
            else
            {
                ddlEmployeeName.SelectedIndex = 0;
            }
            if (ddlOrderNo.Items.FindByValue(dtPrjectRecord.Rows[0]["Field2"].ToString()) != null)
            {
                ddlOrderNo.Value = dtPrjectRecord.Rows[0]["Field2"].ToString();
            }
            else
            {
                ddlOrderNo.SelectedIndex = 0;
            }
            if (ddlparentProjectname.Items.FindByValue(dtPrjectRecord.Rows[0]["Field3"].ToString()) != null)
            {
                ddlparentProjectname.Value = dtPrjectRecord.Rows[0]["Field3"].ToString();
            }
            else
            {
                ddlparentProjectname.SelectedIndex = 0;
            }
            //here we are getting the purchase detail using project id  refference
            objPageCmn.FillData((object)gvPurchaseDetail, ObjPoDetail.GetPurchaseOrderDetailbyProjectId(e.CommandArgument.ToString()), "", "");
            lblValueExpectedCost.Text = SystemParameter.GetCurrencySmbol(Session["CurrencyId"].ToString(), dtPrjectRecord.Rows[0]["ExpectedCost"].ToString(), Session["DBConnection"].ToString());
            lblValueActualCost.Text = SystemParameter.GetCurrencySmbol(Session["CurrencyId"].ToString(), dtPrjectRecord.Rows[0]["ActualCost"].ToString(), Session["DBConnection"].ToString());
            //txtexpenddate.Enabled = false;
            //txtenddate.Enabled = false;
            //ddlOrderNo.Enabled = false;
            ddlprojectname.Enabled = false;
            TabPanelproduct.Visible = true;
            TabPaneltools.Visible = true;
            TabPanelExpenses.Visible = true;
            txtprojectname.Text = dtPrjectRecord.Rows[0]["Project_Name"].ToString();
            txtprojectlocalname.Text = dtPrjectRecord.Rows[0]["Project_Name_L"].ToString();
            if (ddlCustomerNamme.Items.FindByValue(dtPrjectRecord.Rows[0]["Customer_Id"].ToString()) != null)
            {
                ddlCustomerNamme.Value = dtPrjectRecord.Rows[0]["Customer_Id"].ToString();
                txtcustomername_TextChanged(null, null);
            }
            else
            {
                ddlCustomerNamme.SelectedIndex = 0;
                Session["ContactID"] = null;
            }
            if (dtPrjectRecord.Rows[0]["ContactName"].ToString() != "")
            {
                ddlContactPerson.Text = dtPrjectRecord.Rows[0]["ContactName"].ToString() + "/" + dtPrjectRecord.Rows[0]["Contact_Person"].ToString();
            }
            txtprojecttype.Text = dtPrjectRecord.Rows[0]["Project_Type"].ToString();
            txtstartdate.Text = Formatdate(dtPrjectRecord.Rows[0]["Start_Date"].ToString());
            txtexpenddate.Text = Formatdate(dtPrjectRecord.Rows[0]["Exp_End_Date"].ToString());
            txtProjectNo.Text = dtPrjectRecord.Rows[0]["Field7"].ToString();

            if (ddlprojectname.Items.FindByValue(dtPrjectRecord.Rows[0]["Field1"].ToString()) != null)
            {
                ddlprojectname.Value = dtPrjectRecord.Rows[0]["Field1"].ToString();
            }
            else
            {
                ddlprojectname.SelectedIndex = 0;
            }
            if (Formatdate(dtPrjectRecord.Rows[0]["End_Date"].ToString()) == "01-Jan-1990")
            {
                txtenddate.Text = "";
            }
            else
            {
                txtenddate.Text = Formatdate(dtPrjectRecord.Rows[0]["End_Date"].ToString());
            }
            try
            {
                ddlStatus.SelectedValue = dtPrjectRecord.Rows[0]["Project_Title"].ToString();
            }
            catch
            {
                ddlStatus.SelectedIndex = 0;
            }
            Editor1.Content = dtPrjectRecord.Rows[0]["Project_Description"].ToString();
            txtRemarks.Text = dtPrjectRecord.Rows[0]["File_Id"].ToString();
            //  txtfilename.Text = dtPrjectRecord.Rows[0]["File_Name"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            //arcawing code
            //start
            tabContainer.ActiveTabIndex = 0;
            string DirectoryName = string.Empty;
            //this code for get product information
            //code start
            DataTable dtProduct = objProjectproduct.GetRecordByProjectId(e.CommandArgument.ToString());
            if (dtProduct.Rows.Count > 0)
            {
                dtProduct = dtProduct.DefaultView.ToTable(true, "Trans_Id", "Product_Id", "ProductCode", "EProductName", "Unit_Id", "Unit_Name", "Quantity");
                Session["dtProductList"] = dtProduct;
                LoadProductRecord();
            }
            //code end
            //this code for get Tools information
            //code start
            DataTable dtTools = objProjecttools.GetRecordByProjectId(e.CommandArgument.ToString());
            try
            {
                dtTools = new DataView(dtTools, "Field1='PROJECT'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtTools.Rows.Count > 0)
            {
                dtTools = dtTools.DefaultView.ToTable(true, "Trans_Id", "Tools_Id", "ProductCode", "EProductName", "Unit_Id", "Unit_Name", "Quantity", "IsToolsExists");
                Session["dtToolsList"] = dtTools;
                LoadToolsRecord();
            }
            //code end
            //this code for get Expenses information
            //code start
            DataTable dtExpenses = ObjShipExpDetail.Get_ShipExpDetailByInvoiceId("0", "0", "0", e.CommandArgument.ToString(), "PM");
            if (dtExpenses.Rows.Count > 0)
            {
                dtExpenses = dtExpenses.DefaultView.ToTable(true, "Expense_Id", "Exp_Name", "ExpCurrencyID", "CurrencyName", "FCExpAmount");
                Session["dtExpList"] = dtExpenses;
                LoadExpensesRecord();
            }
            //code end
            Session["FileName"] = null;
        }
        //AllPageCode();
    }
    protected void txtOrderNo_TextChanged(object sender, EventArgs e)
    {
        //DataTable dt = objSalesOrderHeader.GetSalesOrderForProjectManagement(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        Session["dtProductList"] = null;
        // LoadProductRecord();
        //ddlCustomerNamme.SelectedIndex = 0;
        if (ddlOrderNo.SelectedIndex > 0)
        {
            //dt = new DataView(dt, "Trans_Id='" + ddlOrderNo.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtsalesorderdetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString()).GetSODetailBySOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlOrderNo.Value.ToString());
            dtsalesorderdetail.Columns["UnitId"].ColumnName = "Unit_Id";
            Session["dtProductList"] = dtsalesorderdetail;
            LoadProductRecord();
        }
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            try
            {
                strNewDate = Convert.ToDateTime(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            catch
            {
            }
        }
        return strNewDate;
    }
    #region addproduct
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText.ToString());
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText.ToString());
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    protected void txtERelatedProduct_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text != "")
        {
            DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text);
            if (dt == null)
            {
                DisplayMessage("Select Product in Suggestion only");
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Focus();
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if (Session["dtProductList"] != null)
                {
                    DataTable DtProduct = (DataTable)Session["dtProductList"];
                    try
                    {
                        DtProduct = new DataView(DtProduct, "Product_Id='" + dt.Rows[0]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (DtProduct.Rows.Count > 0)
                    {
                        DisplayMessage("Product is already exists");
                        ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                        ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
                        ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Focus();
                        return;
                    }
                }
                ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedValue = dt.Rows[0]["UnitId"].ToString();
                ((HiddenField)gvproduct.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = dt.Rows[0]["ProductCode"].ToString();
                ((TextBox)gvproduct.FooterRow.FindControl("txtquantity")).Focus();
            }
            else
            {
                DisplayMessage("Select Product in Suggestion only");
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Focus();
                return;
            }
        }
        else
        {
            ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
            ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Focus();
        }
    }
    protected void txtProductCode_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text != "")
        {
            DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text);
            if (dt == null)
            {
                DisplayMessage("Select Product in Suggestion only");
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Focus();
                return;
            }
            if (dt.Rows.Count > 0)
            {
                if (Session["dtProductList"] != null)
                {
                    DataTable DtProduct = (DataTable)Session["dtProductList"];
                    try
                    {
                        DtProduct = new DataView(DtProduct, "Product_Id='" + dt.Rows[0]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (DtProduct.Rows.Count > 0)
                    {
                        DisplayMessage("Product is already exists");
                        ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                        ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
                        ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Focus();
                        return;
                    }
                }
                ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedValue = dt.Rows[0]["UnitId"].ToString();
                ((HiddenField)gvproduct.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = dt.Rows[0]["EProductName"].ToString();
                ((TextBox)gvproduct.FooterRow.FindControl("txtquantity")).Focus();
            }
            else
            {
                DisplayMessage("Select Product in Suggestion only");
                ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Focus();
                return;
            }
        }
        else
        {
            ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text = "";
            ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Focus();
        }
    }
    protected void gvproduct_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    public void LoadProductRecord()
    {
        DataTable dt = new DataTable();
        if (Session["dtProductList"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["dtProductList"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvproduct, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("Product_Id", typeof(int));
                contacts.Columns.Add("ProductCode", typeof(string));
                contacts.Columns.Add("EProductName", typeof(string));
                contacts.Columns.Add("Unit_Id", typeof(int));
                contacts.Columns.Add("Unit_Name", typeof(string));
                contacts.Columns.Add("Quantity", typeof(string));
                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvproduct, contacts, "", "");
                int TotalColumns = gvproduct.Rows[0].Cells.Count;
                gvproduct.Rows[0].Cells.Clear();
                gvproduct.Rows[0].Cells.Add(new TableCell());
                gvproduct.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvproduct.Rows[0].Visible = false;
            }
        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("Product_Id", typeof(int));
            contacts.Columns.Add("ProductCode", typeof(string));
            contacts.Columns.Add("EProductName", typeof(string));
            contacts.Columns.Add("Unit_Id", typeof(int));
            contacts.Columns.Add("Unit_Name", typeof(string));
            contacts.Columns.Add("Quantity", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvproduct, contacts, "", "");
            int TotalColumns = gvproduct.Rows[0].Cells.Count;
            gvproduct.Rows[0].Cells.Clear();
            gvproduct.Rows[0].Cells.Add(new TableCell());
            gvproduct.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvproduct.Rows[0].Visible = false;
        }
        FillUnit();
    }
    public void FillUnit()
    {
        try
        {
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataSource = objUnitMaster.GetUnitListforDDl(Session["CompId"].ToString());
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataTextField = "Unit_Name";
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataValueField = "Unit_Id";
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataBind();
        }
        catch
        {
        }
        try
        {
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataSource = objUnitMaster.GetUnitListforDDl(Session["CompId"].ToString());
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataTextField = "Unit_Name";
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataValueField = "Unit_Id";
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataBind();
        }
        catch
        {
        }
    }
    protected void gvproduct_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text == "")
            {
                DisplayMessage("Choose product in suggestion");
                ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Focus();
                return;
            }
            if (((TextBox)gvproduct.FooterRow.FindControl("txtquantity")).Text == "")
            {
                DisplayMessage("Enter Quantity");
                ((TextBox)gvproduct.FooterRow.FindControl("txtquantity")).Focus();
                return;
            }
            if (Session["dtProductList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Product_Id", typeof(int));
                dt.Columns.Add("ProductCode", typeof(string));
                dt.Columns.Add("EProductName", typeof(string));
                dt.Columns.Add("Unit_Id", typeof(int));
                dt.Columns.Add("Unit_Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((HiddenField)gvproduct.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedValue;
                dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                dr[6] = ((TextBox)gvproduct.FooterRow.FindControl("txtquantity")).Text;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtProductList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                    strTransid = (float.Parse(dtCopy.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
                }
                else
                {
                    strTransid = "1";
                }
                DataRow dr = dt.NewRow();
                dr[0] = strTransid;
                dr[1] = ((HiddenField)gvproduct.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvproduct.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvproduct.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedValue;
                dr[5] = ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                dr[6] = ((TextBox)gvproduct.FooterRow.FindControl("txtquantity")).Text;
                dt.Rows.Add(dr);
            }
            Session["dtProductList"] = dt;
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtProductList"] != null)
            {
                dt = (DataTable)Session["dtProductList"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtProductList"] = dt;
            }
        }
        gvproduct.EditIndex = -1;
        LoadProductRecord();
    }
    protected void gvproduct_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvproduct.EditIndex = e.NewEditIndex;
        LoadProductRecord();
    }
    protected void gvproduct_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvproduct.EditIndex = -1;
        LoadProductRecord();
    }
    #endregion
    #region addTools
    protected void txtToolsERelatedProduct_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text != "")
        {
            if (Session["dtToolsList"] != null)
            {
                DataTable DtProduct = (DataTable)Session["dtToolsList"];
                try
                {
                    DtProduct = new DataView(DtProduct, "EProductName='" + ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtProduct.Rows.Count > 0)
                {
                    DisplayMessage("Tools Name is already exists");
                    ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                    ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = "";
                    ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Focus();
                    return;
                }
            }
            DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text);
            if (dt == null)
            {
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = "0";
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue = dt.Rows[0]["UnitId"].ToString();
                    ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
                    ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = dt.Rows[0]["ProductCode"].ToString();
                    ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Focus();
                }
                else
                {
                    ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                    ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = "0";
                }
            }
        }
        else
        {
            ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = "";
            ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Focus();
        }
    }
    protected void txttoolsProductCode_OnTextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text != "")
        {
            if (Session["dtToolsList"] != null)
            {
                DataTable DtProduct = (DataTable)Session["dtToolsList"];
                try
                {
                    DtProduct = new DataView(DtProduct, "ProductCode='" + ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtProduct.Rows.Count > 0)
                {
                    DisplayMessage("tools id is already exists");
                    ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                    ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = "";
                    ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Focus();
                    return;
                }
            }
            DataTable dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text);
            if (dt == null)
            {
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = "0";
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue = dt.Rows[0]["UnitId"].ToString();
                    ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
                    ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = dt.Rows[0]["EProductName"].ToString();
                    ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Focus();
                }
                else
                {
                    ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                    ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = "0";
                }
            }
        }
        else
        {
            ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = "";
            ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Focus();
        }
    }
    protected void gvTools_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    protected void gvTools_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }
    protected void gvTools_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvTools.EditIndex = -1;
        LoadToolsRecord();
    }
    public void LoadToolsRecord()
    {
        DataTable dt = new DataTable();
        if (Session["dtToolsList"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["dtToolsList"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvTools, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("Tools_Id", typeof(string));
                contacts.Columns.Add("ProductCode", typeof(string));
                contacts.Columns.Add("EProductName", typeof(string));
                contacts.Columns.Add("Unit_Id", typeof(int));
                contacts.Columns.Add("Unit_Name", typeof(string));
                contacts.Columns.Add("Quantity", typeof(string));
                contacts.Columns.Add("IsToolsExists", typeof(bool));
                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvTools, contacts, "", "");
                int TotalColumns = gvTools.Rows[0].Cells.Count;
                gvTools.Rows[0].Cells.Clear();
                gvTools.Rows[0].Cells.Add(new TableCell());
                gvTools.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvTools.Rows[0].Visible = false;
            }
        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("Tools_Id", typeof(string));
            contacts.Columns.Add("ProductCode", typeof(string));
            contacts.Columns.Add("EProductName", typeof(string));
            contacts.Columns.Add("Unit_Id", typeof(int));
            contacts.Columns.Add("Unit_Name", typeof(string));
            contacts.Columns.Add("Quantity", typeof(string));
            contacts.Columns.Add("IsToolsExists", typeof(bool));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvTools, contacts, "", "");
            int TotalColumns = gvTools.Rows[0].Cells.Count;
            gvTools.Rows[0].Cells.Clear();
            gvTools.Rows[0].Cells.Add(new TableCell());
            gvTools.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvTools.Rows[0].Visible = false;
        }
        FillUnit();
    }
    protected void gvTools_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text == "")
            {
                DisplayMessage("Enter Product Code");
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Focus();
                return;
            }
            if (((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text == "")
            {
                DisplayMessage("Enter Product Name");
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Focus();
                return;
            }
            if (((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Text == "")
            {
                DisplayMessage("Enter Quantity");
                ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Focus();
                return;
            }
            bool isToolsExists = false;
            if (Session["dtToolsList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Tools_Id", typeof(string));
                dt.Columns.Add("ProductCode", typeof(string));
                dt.Columns.Add("EProductName", typeof(string));
                dt.Columns.Add("Unit_Id", typeof(int));
                dt.Columns.Add("Unit_Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("IsToolsExists", typeof(bool));
                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue;
                dr[5] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                dr[6] = ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Text;
                if (((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value == "0")
                {
                    isToolsExists = false;
                }
                else
                {
                    isToolsExists = true;
                }
                dr[7] = isToolsExists;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtToolsList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                    strTransid = (float.Parse(dtCopy.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
                }
                else
                {
                    strTransid = "1";
                }
                DataRow dr = dt.NewRow();
                dr[0] = strTransid;
                dr[1] = ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue;
                dr[5] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                dr[6] = ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Text;
                if (((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value == "0")
                {
                    isToolsExists = false;
                }
                else
                {
                    isToolsExists = true;
                }
                dr[7] = isToolsExists;
                dt.Rows.Add(dr);
            }
            Session["dtToolsList"] = dt;
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtToolsList"] != null)
            {
                dt = (DataTable)Session["dtToolsList"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtToolsList"] = dt;
            }
        }
        gvTools.EditIndex = -1;
        LoadToolsRecord();
    }
    #endregion
    #region addExpenses
    protected void gvExpenses_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    public void LoadExpensesRecord()
    {
        DataTable dt = new DataTable();
        if (Session["dtExpList"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["dtExpList"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvExpenses, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Expense_Id", typeof(int));
                contacts.Columns.Add("Exp_Name", typeof(string));
                contacts.Columns.Add("ExpCurrencyID", typeof(int));
                contacts.Columns.Add("CurrencyName", typeof(string));
                contacts.Columns.Add("FCExpAmount", typeof(string));
                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvExpenses, contacts, "", "");
                int TotalColumns = gvExpenses.Rows[0].Cells.Count;
                gvExpenses.Rows[0].Cells.Clear();
                gvExpenses.Rows[0].Cells.Add(new TableCell());
                gvExpenses.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvExpenses.Rows[0].Visible = false;
            }
        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Expense_Id", typeof(int));
            contacts.Columns.Add("Exp_Name", typeof(string));
            contacts.Columns.Add("ExpCurrencyID", typeof(int));
            contacts.Columns.Add("CurrencyName", typeof(string));
            contacts.Columns.Add("FCExpAmount", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvExpenses, contacts, "", "");
            int TotalColumns = gvExpenses.Rows[0].Cells.Count;
            gvExpenses.Rows[0].Cells.Clear();
            gvExpenses.Rows[0].Cells.Add(new TableCell());
            gvExpenses.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvExpenses.Rows[0].Visible = false;
        }
        fillExpenses();
        fillCurrency();
    }
    protected void gvExpenses_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        if (e.CommandName.Equals("AddNew"))
        {
            if (((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).SelectedIndex == 0)
            {
                DisplayMessage("Select Expenses");
                ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).Focus();
                return;
            }
            if (((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).SelectedIndex == 0)
            {
                DisplayMessage("Select Currency");
                ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).Focus();
                return;
            }
            if (((TextBox)gvExpenses.FooterRow.FindControl("txtexpAmount")).Text == "")
            {
                DisplayMessage("Enter Amount");
                ((TextBox)gvExpenses.FooterRow.FindControl("txtexpAmount")).Focus();
                return;
            }
            if (Session["dtExpList"] == null)
            {
                dt.Columns.Add("Expense_Id", typeof(int));
                dt.Columns.Add("Exp_Name", typeof(string));
                dt.Columns.Add("ExpCurrencyID", typeof(int));
                dt.Columns.Add("CurrencyName", typeof(string));
                dt.Columns.Add("FCExpAmount", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).SelectedValue;
                dr[1] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).SelectedItem.Text;
                dr[2] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).SelectedValue;
                dr[3] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).SelectedItem.Text;
                dr[4] = ((TextBox)gvExpenses.FooterRow.FindControl("txtexpAmount")).Text;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = (DataTable)Session["dtExpList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "Expense_Id=" + ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCopy.Rows.Count > 0)
                    {
                        DisplayMessage("Expense Already exists");
                        ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).Focus();
                        return;
                    }
                }
                else
                {
                }
                DataRow dr = dt.NewRow();
                dr[0] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).SelectedValue;
                dr[1] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")).SelectedItem.Text;
                dr[2] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).SelectedValue;
                dr[3] = ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).SelectedItem.Text;
                dr[4] = ((TextBox)gvExpenses.FooterRow.FindControl("txtexpAmount")).Text;
                dt.Rows.Add(dr);
            }
            Session["dtExpList"] = dt;
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtExpList"] != null)
            {
                dt = (DataTable)Session["dtExpList"];
                dt = new DataView(dt, "Expense_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtExpList"] = dt;
            }
        }
        gvExpenses.EditIndex = -1;
        LoadExpensesRecord();
    }
    public void FillCurrency()
    {
        try
        {
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataSource = objUnitMaster.GetUnitMaster(Session["CompId"].ToString());
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataTextField = "Unit_Name";
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataValueField = "Unit_Id";
            ((DropDownList)gvproduct.FooterRow.FindControl("ddlunit")).DataBind();
        }
        catch
        {
        }
        try
        {
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataSource = objUnitMaster.GetUnitMaster(Session["CompId"].ToString());
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataTextField = "Unit_Name";
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataValueField = "Unit_Id";
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataBind();
        }
        catch
        {
        }
    }
    public void fillExpenses()
    {
        DataTable dt = new Inv_ShipExpMaster(Session["DBConnection"].ToString()).GetShipExpMaster(Session["CompId"].ToString().ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        try
        {
            objPageCmn.FillData((object)((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpense")), dt, "Exp_Name", "Expense_Id");
        }
        catch
        {
        }
    }
    public void fillCurrency()
    {
        DataTable dt = new CurrencyMaster(Session["DBConnection"].ToString()).GetCurrencyListForDDL();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        try
        {
            objPageCmn.FillData((object)((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")), dt, "Currency_Name", "Currency_Id");
        }
        catch
        {
        }
        try
        {
            ((DropDownList)gvExpenses.FooterRow.FindControl("ddlExpCurrency")).SelectedValue = new LocationMaster(Session["DBConnection"].ToString()).GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        }
        catch
        {
        }
    }
    public string GetAmountDecimal(string Currency, string Amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(Currency, Amount);
    }
    #endregion
    #region TreeViewConcept
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");

        if (TreeViewCategory.Visible == true)
        {
            TreeViewCategory.Visible = false;
            GvrProjectteam.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            btnGridView.CssClass = "fa fa-list";
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
            GvrProjectteam.Visible = false;
            TreeViewCategory.Visible = true;
            btnGridView.CssClass = "fa fa-sitemap";
            //BindGrid();
        }
        btnGridView.Focus();
    }

    protected void TreeViewCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewCategory.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }
    private void BindTreeView(DataTable dt)//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewCategory.Nodes.Clear();
        //dt = objProjctMaster.GetAllProjectMasteer();
        //try
        //{
        //    dt = new DataView(dt, "Field4='" + Session["CompId"].ToString() + "' and Field5='" + Session["BrandId"].ToString() + "' and Field6='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //catch
        //{
        //}
        DataTable dtTemp = new DataView(dt, "Field3='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtTemp.Rows.Count == 0)
        {
            dtTemp = dt.Copy();
        }
        int i = 0;
        while (i < dtTemp.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dtTemp.Rows[i]["Project_Name"].ToString();
            tn.Value = dtTemp.Rows[i]["Project_Id"].ToString();
            TreeViewCategory.Nodes.Add(tn);
            FillChild((dtTemp.Rows[i]["Project_Id"].ToString()), tn, dt);
            i++;
        }
        TreeViewCategory.DataBind();
        dtTemp.Dispose();
        dt.Dispose();
    }
    private void FillChild(string index, TreeNode tn, DataTable dt)//fill up child nodes and respective child nodes of them 
    {
        DataTable dtTemp = new DataView(dt, "Field3=" + index + "", "", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dtTemp.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dtTemp.Rows[i]["Project_Name"].ToString();
            tn1.Value = dtTemp.Rows[i]["Project_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dtTemp.Rows[i]["Project_Id"].ToString()), tn1, dt);
            i++;
        }
        TreeViewCategory.DataBind();
        dtTemp.Dispose();
    }
    protected void btnDeleteChild_Click(object sender, EventArgs e)
    {
        objProjctMaster.DeleteProjectMaster_ByParentId(hdnPrjectId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        objProjctMaster.DeleteProjectMaster(hdnPrjectId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Delete", "green");
        BindGrid();
        Reset();
        hdnPrjectId.Value = "";
    }
    protected void btnMoveChild_Click(object sender, EventArgs e)
    {
        btnDeleteChild.Visible = false;
        btnBack.Visible = true;
        btnMoveChild.Visible = false;
        FillMoveChildDropDownList(hdnPrjectId.Value);
    }
    protected void btnUpdateParent_Click(object sender, EventArgs e)
    {
        if (ddlMoveCategory.SelectedItem.Text != "No Category Available Here")
        {
            objProjctMaster.DeleteProjectMaster(hdnPrjectId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
            objProjctMaster.UpdateParentproject(hdnPrjectId.Value, ddlMoveCategory.SelectedValue, Session["UserId"].ToString(), DateTime.Now.ToString());
            DisplayMessage("Record Delete and Move Child", "green");
            BindGrid();
            hdnPrjectId.Value = "";
        }
    }
    public void FillMoveChildDropDownList(string strExceptId) //Function to fill up items in drop down list of New Parent after delete
    {
        DataTable dt = objProjctMaster.GetAllProjectMasteer();
        string query = "Project_Id not in(";
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
            objPageCmn.FillData((object)ddlMoveCategory, dt, "Project_Name", "Project_Id");
        }
        else
        {
            ddlMoveCategory.Items.Add("No Category Available Here");
        }
        arr.Clear();
    }
    private void FindChildNode(string p)  //Function to find child nodes and child of child nodes and so on
    {
        arr.Add(p);
        DataTable dt = objProjctMaster.GetAllProjectMaster_By_ParentId(p);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FindChildNode(dt.Rows[i]["Project_Id"].ToString());
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
    }
    #endregion
    #region PendingOrder
    private void FillSalesOrder()
    {
        DataTable dt = objSalesOrderHeader.GetSalesOrderForProjectManagement(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            dt = new DataView(dt, "Post='Pending'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        { }
        Session["dtSorder"] = dt;
        Session["dtFilterSorder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
        lblOrderTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        dt.Dispose();
    }
    protected void btnSalesOrder_Click(object sender, EventArgs e)
    {
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtprojectname);
        FillSalesOrder();
    }
    protected void ddlOrderFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOrderFieldName.SelectedItem.Value == "SalesOrderDate")
        {
            txtOrderValuedate.Visible = true;
            txtOrderValue.Visible = false;
            txtOrderValue.Text = "";
            txtOrderValuedate.Text = "";
        }
        else
        {
            txtOrderValuedate.Visible = false;
            txtOrderValue.Visible = true;
            txtOrderValue.Text = "";
            txtOrderValuedate.Text = "";
        }
    }
    protected void btnOrderbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlOrderFieldName.SelectedItem.Value == "SalesOrderDate")
        {
            if (txtOrderValuedate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtOrderValuedate.Text);
                    txtOrderValue.Text = Convert.ToDateTime(txtOrderValuedate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtOrderValuedate.Text = "";
                    txtOrderValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtOrderValuedate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtOrderValuedate.Focus();
                return;
            }
        }
        if (ddlOrderOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOrderOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlOrderFieldName.SelectedValue + ",System.String)='" + txtOrderValue.Text.Trim() + "'";
            }
            else if (ddlOrderOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlOrderFieldName.SelectedValue + ",System.String) like '%" + txtOrderValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlOrderFieldName.SelectedValue + ",System.String) Like '" + txtOrderValue.Text.Trim() + "%'";
            }
            DataTable dtAdd = (DataTable)Session["dtSorder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesOrder, view.ToTable(), "", "");
            Session["dtFilterSorder"] = view.ToTable();
            lblOrderTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        btnOrderRefresh.Focus();
    }
    protected void btnOrderRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillSalesOrder();
        ddlOrderFieldName.SelectedIndex = 0;
        ddlOrderOption.SelectedIndex = 2;
        txtOrderValue.Text = "";
        txtOrderValuedate.Text = "";
        txtOrderValuedate.Visible = false;
        txtOrderValue.Visible = true;
        txtOrderValue.Focus();
        //AllPageCode();
    }
    protected void GvSalesOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterSorder"];
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
        Session["dtFilterSorder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
        //AllPageCode();
    }
    protected void GvSalesOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterSorder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesOrder, dt, "", "");
        //AllPageCode();
    }
    protected void btnproject_Command(object sender, CommandEventArgs e)
    {
        ddlOrderNo.Value = e.CommandArgument.ToString();
        //txtOrderNo.Text = ((Label)gvrow.FindControl("lblgvSONo")).Text;
        txtOrderNo_TextChanged(null, null);
        txtprojectname.Enabled = true;
        txtprojectname.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Sales_Active()", true);
        //AllPageCode();
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string CurrencyAmount = string.Empty;
        try
        {
            CurrencyAmount = SystemParameter.GetCurrencySmbol(CurrencyId, ObjSysParam.GetCurencyConversionForInv(CurrencyId, Amount), Session["DBConnection"].ToString());
        }
        catch
        {
        }
        return CurrencyAmount;
    }
    #endregion
    #region BindLocation
    private void FillddlLocation()
    {
        DataTable dtLoc = new LocationMaster(Session["DBConnection"].ToString()).GetLocationMaster(Session["CompId"].ToString());
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
        ddlLocation.DataSource = null;
        ddlLocation.DataBind();
        ddlLocation.DataSource = dtLoc;
        ddlLocation.DataTextField = "Location_Name";
        ddlLocation.DataValueField = "Location_Id";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, "All");
        ddlLocation.SelectedValue = Session["LocId"].ToString();
    }
    protected void ddlLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnRefresh_Click(null, null);
        BindGrid();
        I1.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
    }
    #endregion
    //#endregion
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //HERE WE ARE CHECKING VALIDATION THAT USER CAN  CLOSE PROJECT WHEN ALLL TASK WILL BE 100 % WHEN TASK ASSINGED OTHER WISE HE CAN CLOSE IT 
        //DataTable dttask = objProjectTask.GetStatusByProjectId(e.CommandArgument.ToString());
        //if (dttask.Rows.Count > 0)
        //{
        //    if (new DataView(dttask, "Status='Assigned'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
        //    {
        //        DisplayMessage("You can not Print , First close the Assigned task");
        //        return;
        //    }
        //}
        //Code for Approval
        string strCmd = string.Format("window.open('../ProjectManagement_Report/Final_Acceptance.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=P','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion
    #region reffrenceProjectmasterAutoCompleteList&event
    protected void ddlprojectname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlEmployeeName.SelectedIndex = 0;
        trImport.Visible = false;
        chkImportRefProjectTask.Checked = false;
        chkImportRefProjectTeam.Checked = false;
        Session["dtProductList"] = null;
        LoadProductRecord();
        Session["dtToolsList"] = null;
        LoadToolsRecord();
        Session["dtExpList"] = null;
        LoadExpensesRecord();

        if (ddlprojectname.SelectedIndex > 0)
        {
            trImport.Visible = true;
            DataTable dtTools = objProjecttools.GetRecordByProjectId(ddlprojectname.SelectedItem.Value.ToString());
            if (dtTools.Rows.Count > 0)
            {
                dtTools = dtTools.DefaultView.ToTable(true, "Trans_Id", "Tools_Id", "ProductCode", "EProductName", "Unit_Id", "Unit_Name", "Quantity", "IsToolsExists");
                Session["dtToolsList"] = dtTools;
                LoadToolsRecord();
            }
            //this code for get Expenses information
            //code start
            DataTable dtExpenses = ObjShipExpDetail.Get_ShipExpDetailByInvoiceId("0", "0", "0", ddlprojectname.SelectedItem.Value.ToString(), "PM");
            if (dtExpenses.Rows.Count > 0)
            {
                dtExpenses = dtExpenses.DefaultView.ToTable(true, "Expense_Id", "Exp_Name", "ExpCurrencyID", "CurrencyName", "Exp_Charges");
                Session["dtExpList"] = dtExpenses;
                LoadExpensesRecord();
            }
            DataTable dtPrjectRecord = objProjctMaster.GetAllProjectMasteer();
            dtPrjectRecord = new DataView(dtPrjectRecord, "Project_Id=" + ddlprojectname.SelectedItem.Value.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtPrjectRecord.Rows[0]["Project_Manager"].ToString() != "0")
            {
                ddlEmployeeName.Value = dtPrjectRecord.Rows[0]["Project_Manager"].ToString();
                //txtEmpName.Text = dtPrjectRecord.Rows[0]["ManagerName"].ToString() + "/" + dtPrjectRecord.Rows[0]["Project_Manager"].ToString();
            }
            else
            {
                ddlEmployeeName.SelectedIndex = 0;
            }
            //code end
            //dtProduct.Dispose();
            dtTools.Dispose();
            dtExpenses.Dispose();
            dtPrjectRecord.Dispose();
        }
    }
    #endregion
    #region ProjectClosing
    protected void ddlCloseprojectName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        btnProjectReset_Click(null, null);
        if (ddlCloseprojectName.SelectedIndex > 0)
        {
            DataTable dtPrjectRecord = new DataTable();
            dtPrjectRecord = objProjctMaster.GetAllProjectMasteer();
            dtPrjectRecord = new DataView(dtPrjectRecord, "Project_Id=" + ddlCloseprojectName.Value.ToString() + " and Project_Title='Close'", "", DataViewRowState.CurrentRows).ToTable();
            //dtPrjectRecord = objProjctMaster.GetRecordByProjectId(hdnPrjectId.Value.ToString());
            if (dtPrjectRecord.Rows.Count > 0)
            {
                txtenddate.Enabled = false;
                txtenddate.Text = Formatdate(dtPrjectRecord.Rows[0]["End_Date"].ToString());
                txtRemarks.Text = dtPrjectRecord.Rows[0]["File_id"].ToString();
                txtTestimonial.Text = dtPrjectRecord.Rows[0]["Testimonial"].ToString();
                txtAboutservices.Text = dtPrjectRecord.Rows[0]["About_Services"].ToString();
                txtSysBenefit.Text = dtPrjectRecord.Rows[0]["System_benefit"].ToString();
                btnProjectClose.Text = Resources.Attendance.Update;
            }
        }
    }
    protected void btnProjectClose_Click(object sender, EventArgs e)
    {
        if (ddlCloseprojectName.SelectedIndex <= 0)
        {
            DisplayMessage("Select Project Name");
            ddlCloseprojectName.Focus();
            return;
        }
        //HERE WE ARE CHECKING VALIDATION THAT USER CAN  CLOSE PROJECT WHEN ALLL TASK WILL BE 100 % WHEN TASK ASSINGED OTHER WISE HE CAN CLOSE IT 
        DataTable dttask = objProjectTask.GetStatusByProjectId(ddlCloseprojectName.Value.ToString());
        if (dttask.Rows.Count > 0)
        {
            if (new DataView(dttask, "Status='Assigned'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("You can not Print , First close the Assigned task");
                return;
            }
        }
        dttask.Dispose();
        if (txtenddate.Text == "")
        {
            DisplayMessage("Enter Project End Date");
            txtenddate.Focus();
            return;
        }
        if (txtenddate.Text != "")
        {
            try
            {
                ObjSysParam.getDateForInput(txtenddate.Text);
            }
            catch
            {
                DisplayMessage("Enter End Date in format " + Session["DateFormat"].ToString() + "");
                txtenddate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtenddate);
                return;
            }
        }
        if (txtRemarks.Text == "")
        {
            DisplayMessage("Enter Remarks");
            txtRemarks.Focus();
            return;
        }
        if (txtTestimonial.Text == "")
        {
            DisplayMessage("Enter Testimonial");
            txtTestimonial.Focus();
            return;
        }
        if (txtAboutservices.Text == "")
        {
            DisplayMessage("Enter About services");
            txtAboutservices.Focus();
            return;
        }
        if (txtSysBenefit.Text == "")
        {
            DisplayMessage("Enter System benefits");
            txtSysBenefit.Focus();
            return;
        }
        int a = objProjctMaster.UpdateProjcetStatus(ddlCloseprojectName.Value.ToString(), ObjSysParam.getDateForInput(txtenddate.Text).ToString(), "Close", txtRemarks.Text, txtTestimonial.Text, txtAboutservices.Text, txtSysBenefit.Text, Session["UserId"].ToString(), DateTime.Now.ToString());
        if (a != 0)
        {
            DisplayMessage("Project Closed Successfully");
            ddlCloseprojectName.SelectedIndex = 0;
            btnProjectReset_Click(null, null);
        }
    }
    protected void btnProjectReset_Click(object sender, EventArgs e)
    {
        txtenddate.Text = "";
        txtRemarks.Text = "";
        txtTestimonial.Text = "";
        txtSysBenefit.Text = "";
        txtAboutservices.Text = "";
        btnProjectClose.Text = Resources.Attendance.Close;
        txtenddate.Enabled = true;
        txtenddate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        I2.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }
    protected void btnCloseProjectCancel_Click(object sender, EventArgs e)
    {
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    #endregion
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/Project", "ProjectManagement", "Project", e.CommandName.ToString(), ((LinkButton)gvrow.FindControl("lnkProjectName")).Text + "(" + e.CommandName.ToString() + ")");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    [WebMethod(enableSession: true)]
    [ScriptMethod]
    public static string txtDDlProjectName_changed(string projectName)
    {
        string projectId = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetProjectIdByName(projectName);
        return projectId;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        try
        {
            DataTable dtCon = HttpContext.Current.Session["PrjMstr_ddlContactPersonName"] as DataTable;
            dtCon = new DataView(dtCon, "Name like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Field2"].ToString() + "/" + dtCon.Rows[i]["Field1"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
                }
            }
            return filterlist;
        }
        catch
        {
            return null;
        }
    }

    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }

    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvrProjectteam, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvrProjectteam, lstCls);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

}