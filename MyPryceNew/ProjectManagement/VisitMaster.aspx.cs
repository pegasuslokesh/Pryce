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
using System.Text;
using System.IO;
using PegasusDataAccess;
using System.Xml;
using System.Net;
using System.Threading;
using System.Data.SqlClient;

public partial class ProjectManagement_VisitMaster : System.Web.UI.Page
{
    #region Defined Class Object
    Common cmn = null;
    Prj_ProjectMaster objProjctMaster = null;
    Ems_ContactMaster objContactmaster = null;
    SystemParameter ObjsysParam = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Prj_VisitMaster objVisitMaster = null;
    EmployeeMaster objEmpMaster = null;
    Prj_ProjectTask objProjectTask = null;
    SystemParameter ObjSysParam = null;
    Prj_ProjectTeam objProjectteam = null;
    SM_Ticket_Master objticketmaster = null;
    Prj_Project_Task_Employeee objTaskEmp = null;
    SM_TicketEmployee objTicketEmployee = null;
    Inv_SalesInvoiceHeader objSalesInvoiceheader = null;
    Set_AddressChild objAddChild = null;
    LocationMaster objLocation = null;
    DataAccessClass objDa = null;
    CountryMaster objCountryMaster = null;
    Country_Currency objCountryCurrency = null;
    Sys_AreaMaster objAreamaster = null;
    Set_AddressCategory ObjAddressCat = null;
    Prj_Visit_Task objVisitTask = null;
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

        cmn = new Common(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        objContactmaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjsysParam = new SystemParameter(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objVisitMaster = new Prj_VisitMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objProjectTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objProjectteam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        objticketmaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        objTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        objTicketEmployee = new SM_TicketEmployee(Session["DBConnection"].ToString());
        objSalesInvoiceheader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objAreamaster = new Sys_AreaMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objVisitTask = new Prj_Visit_Task(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/CallRegister.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Fillddlprojectname();
            CalendarExtendertxtVisitDate.Format = Session["DateFormat"].ToString();
            btList_Click(null, null);
            FillEmployee();
            BindGrid();
            Session["dtVisitTaskList"] = null;
            LoadVisitTask();

            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            txtVisitDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Session["Long"] = null;
            Session["Lati"] = null;
            UpdatevisitmastertableStatus();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            FillAddressCategory();
            CalendarExtender_txtFromdateReport.Format = Session["DateFormat"].ToString();
            CalendarExtender_txttodatereport.Format = Session["DateFormat"].ToString();


        }

        GvVisitMaster.DataSource = Session["dtFilter_Visit"] as DataTable;
        GvVisitMaster.DataBind();
        //AllPageCode();

    }
    private void FillAddressCategory()
    {
        ddlAddressCategory.Items.Clear();

        DataTable dsAddressCat = null;
        dsAddressCat = ObjAddressCat.GetAddressCategoryAll();
        if (dsAddressCat.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlAddressCategory, dsAddressCat, "Address_Name", "Address_Category_Id");
        }
        else
        {
            ddlAddressCategory.Items.Insert(0, "--Select--");
            ddlAddressCategory.SelectedIndex = 0;
        }

    }

    public void UpdatevisitmastertableStatus()
    {
        DataTable dtGeoWork = new DataTable();

        //here we check that GeoWork database is exist or not if not than return the finunction 
        try
        {
            dtGeoWork = objDa.return_DataTable("select * from sys.databases where name='GeoWork'");
        }
        catch
        {

        }

        if (dtGeoWork.Rows.Count > 0)
        {

            objDa.execute_Command("update prj_visitmaster set Status='Close' where Trans_Id in (select Ref_Order_No from GeoWork.dbo.gw_order where Is_Finished='True')");
            objDa.execute_Command("update prj_visitmaster set Status='Cancel' where Trans_Id in (select Ref_Order_No from GeoWork.dbo.gw_order where Is_Cancel='True')");

            //string sql = "select Ref_Order_No,Is_Finished from GeoWork.dbo.gw_order where Is_Finished='True' or Is_Cancel='True'";
            //DataTable dt = objDa.return_DataTable(sql);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (Convert.ToBoolean(dr["Is_Finished"].ToString()))
            //    {

            //        sql = "update prj_visitmaster set Status='Close' where Trans_Id=" + dr["Ref_Order_No"].ToString() + "";

            //    }
            //    else
            //    {
            //        sql = "update prj_visitmaster set Status='Cancel' where Trans_Id=" + dr["Ref_Order_No"].ToString() + "";

            //    }
            //    objDa.execute_Command(sql);

            //}
        }

    }
    public void FillEmployee()
    {
        DataTable dtEmp = objEmpMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());


        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        try
        {
            dtEmp = new DataView(dtEmp, "Emp_Name<>' '", "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        listtaskEmployee.DataSource = dtEmp;
        listtaskEmployee.DataTextField = "EmpDetailInfo";
        listtaskEmployee.DataValueField = "Emp_Id";
        listtaskEmployee.DataBind();

    }
    public void BindGrid()
    {

        DataTable dtVisitMaster = objVisitMaster.GetAllTrueRecord();
        try
        {
            if (ddlStatusFilter.SelectedIndex != 0)
            {
                dtVisitMaster = new DataView(dtVisitMaster, "Status='" + ddlStatusFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
        }
        //objPageCmn.FillData((object)GvVisitMaster, dtVisitMaster, "", "");
        GvVisitMaster.DataSource = dtVisitMaster;
        GvVisitMaster.DataBind();


        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtVisitMaster.Rows.Count + "";
        Session["dtFilter_Visit"] = dtVisitMaster;
        //AllPageCode();

        //foreach (GridViewRow gvrow in GvVisitMaster.Rows)
        //{
        //    LinkButton lnkref = (LinkButton)gvrow.FindControl("lnkstatus");

        //    if (lnkref.Text.Trim() == "Open")
        //    {
        //        lnkref.Style.Add("text-decoration", "none");
        //        lnkref.Style.Add("cursor", "none");
        //        lnkref.CssClass = "labelComman";
        //        lnkref.ForeColor = System.Drawing.ColorTranslator.FromHtml("#474646");
        //        lnkref.Font.Underline = false;
        //        lnkref.ToolTip = "";
        //        //lnkref.Visible = false;
        //        //e.Row.Cells[8].Text = "Opening Stock";
        //        //e.Row.Attributes["onmouseover"] = "this.style.cursor='crosshair';this.style.background='#f2f2f2';";
        //    }
        //}
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnsave.Visible = clsPagePermission.bEdit;
        GvVisitMaster.Columns[0].Visible = clsPagePermission.bView;
        GvVisitMaster.Columns[1].Visible = clsPagePermission.bEdit;
        GvVisitMaster.Columns[2].Visible = clsPagePermission.bDelete;
        UpdatevisitmastertableStatus();
    }
    protected void btList_Click(object sender, EventArgs e)
    {

        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");


        pnlTabtrackuser.Visible = false;
        pnlReport.Visible = false;
        pnllist.Visible = true;
        pnlprojectrecord.Visible = false;

        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.Visible = false;

    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        pnlprojectrecord.Visible = true;
        pnllist.Visible = false;
        pnlTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTabtrackuser.Visible = false;
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.Visible = false;
        //AllPageCode();
    }
    protected void ddlprojectname_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlprojectname.SelectedIndex != 0)
        {
            DataTable dt = objProjectTask.GetDataProjectId(ddlprojectname.SelectedValue);

            objPageCmn.FillData((object)ddltask, dt, "Subject", "Task_Id");
            ddltask.Items.Insert(0, "--Select---");

            if (objProjectteam.GetRecordByProjectId("", ddlprojectname.SelectedValue).Rows.Count > 0)
            {
                objPageCmn.FillData((object)listtaskEmployee, objProjectteam.GetRecordByProjectId("", ddlprojectname.SelectedValue), "Emp_Name", "Emp_Id");
            }
            else
            {
                listtaskEmployee.Items.Clear();
            }
            lnkTaskdesc.Visible = false;
            DataTable dtProject = objProjctMaster.GetRecordByProjectId(ddlprojectname.SelectedValue);
            if (dtProject.Rows.Count > 0)
            {
                if (dtProject.Rows[0]["Customer_Id"].ToString() == "0")
                {
                    txtcustomername.Text = "";

                    txtCustomerAddress.Text = "";
                    txtPermanentMobileNo.Text = "";
                    Session["Long"] = null;
                    Session["Lati"] = null;
                    txtLatitude.Text = "0.000000";
                    txtLongitude.Text = "0.000000";
                }
                else
                {
                    txtcustomername.Text = dtProject.Rows[0]["Name"].ToString() + "/" + dtProject.Rows[0]["Customer_Id"].ToString();
                    txtcustomername_TextChanged(null, null);
                }
            }
        }
        else
        {
            txttaskdescription.Content = null;
            lnkTaskdesc.Visible = false;
            ddltask.Items.Clear();
            FillEmployee();
        }
    }
    protected void ddltask_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddltask.SelectedIndex != 0)
        {
            txttaskdescription.Content = objProjectTask.GetRecordByTaskId(ddltask.SelectedValue).Rows[0]["Description"].ToString();
            lnkTaskdesc.Visible = true;
            trtaskDetail.Visible = true;
            DataTable dttaskemp = new DataTable();

            foreach (ListItem li in listtaskEmployee.Items)
            {
                dttaskemp = objTaskEmp.GetRecordBy_RefType_and_RefId("Task", ddltask.SelectedValue);
                dttaskemp = new DataView(dttaskemp, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dttaskemp.Rows.Count > 0)
                {
                    li.Selected = true;
                }
                else
                {
                    li.Selected = false;
                }
            }
        }
        else
        {
            txttaskdescription.Content = null;
            lnkTaskdesc.Visible = false;
            trtaskDetail.Visible = false;
            foreach (ListItem li in listtaskEmployee.Items)
            {

                li.Selected = false;

            }
        }
    }
    protected void txtcustomername_TextChanged(object sender, EventArgs e)
    {

        string custid = string.Empty;
        if (txtcustomername.Text != "")
        {
            try
            {
                custid = txtcustomername.Text.Split('/')[1].ToString();
            }
            catch
            {
                custid = "0";
            }
            DataTable dtContactmaster = objContactmaster.GetContactTrueById(custid);


            if (dtContactmaster.Rows.Count > 0)
            {
                custid = dtContactmaster.Rows[0]["Trans_Id"].ToString();

                string[] strcustInfo = getCustomerInformation(custid);
                txtCustomerAddress.Text = strcustInfo[0].ToString();
                txtPermanentMobileNo.Text = strcustInfo[1].ToString();
                txtLatitude.Text = strcustInfo[3].ToString();
                txtLongitude.Text = strcustInfo[2].ToString();
                //hdncust.Value = custid;
            }
            else
            {
                txtCustomerAddress.Text = "";
                txtPermanentMobileNo.Text = "";
                Session["Long"] = null;
                Session["Lati"] = null;
                txtLatitude.Text = "0.000000";
                txtLongitude.Text = "0.000000";
            }
        }
        else
        {
            txtCustomerAddress.Text = "";
            txtPermanentMobileNo.Text = "";
            Session["Long"] = null;
            Session["Lati"] = null;
            txtLatitude.Text = "0.000000";
            txtLongitude.Text = "0.000000";
        }
    }


    protected void ddlAddressCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        string custid = string.Empty;
        if (txtcustomername.Text != "")
        {
            try
            {
                custid = txtcustomername.Text.Split('/')[1].ToString();
            }
            catch
            {
                custid = "0";
            }
            DataTable dtContactmaster = objContactmaster.GetContactTrueById(custid);


            if (dtContactmaster.Rows.Count > 0)
            {
                custid = dtContactmaster.Rows[0]["Trans_Id"].ToString();

                string[] strcustInfo = getCustomerInformation(custid);
                txtCustomerAddress.Text = strcustInfo[0].ToString();
                txtPermanentMobileNo.Text = strcustInfo[1].ToString();
                txtLatitude.Text = strcustInfo[3].ToString();
                txtLongitude.Text = strcustInfo[2].ToString();
                //hdncust.Value = custid;
            }
            else
            {
                txtCustomerAddress.Text = "";
                txtPermanentMobileNo.Text = "";
                Session["Long"] = null;
                Session["Lati"] = null;
                txtLatitude.Text = "0.000000";
                txtLongitude.Text = "0.000000";
            }
        }
        else
        {
            txtCustomerAddress.Text = "";
            txtPermanentMobileNo.Text = "";
            Session["Long"] = null;
            Session["Lati"] = null;
            txtLatitude.Text = "0.000000";
            txtLongitude.Text = "0.000000";
        }

    }

    public string[] getCustomerInformation(string Contactid)
    {
        //code start
        string[] strCusInfo = new string[4];
        string strContactNo = string.Empty;
        string Address = string.Empty;
        string Longitude = string.Empty;
        string Latitude = string.Empty;


        DataTable dtContact = objContactmaster.GetContactTrueById(Contactid);
        if (dtContact.Rows.Count > 0)
        {
            strCusInfo[1] = dtContact.Rows[0]["Field2"].ToString();
        }
        //for get address
        DataTable dt = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", Contactid);



        try
        {
            if (ddlAddressCategory.SelectedIndex == 0)
            {
                dt = new DataView(dt, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dt = new DataView(dt, "Address_Category_Id=" + ddlAddressCategory.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {

            Address = dt.Rows[0]["Address"].ToString();
            if (dt.Rows[0]["Address"].ToString() != "")
            {
                Address = dt.Rows[0]["Address"].ToString();
            }
            if (dt.Rows[0]["Street"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Street"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Street"].ToString();
                }
            }
            if (dt.Rows[0]["Block"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Block"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Block"].ToString();
                }

            }
            if (dt.Rows[0]["Avenue"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Avenue"].ToString();
                }
            }

            if (dt.Rows[0]["CityId"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["CityId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["CityId"].ToString();
                }

            }
            if (dt.Rows[0]["StateId"].ToString() != "")
            {


                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["StateId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["StateId"].ToString();
                }

            }
            if (dt.Rows[0]["CountryId"].ToString() != "")
            {
                CountryMaster objCountry = new CountryMaster(Session["DBConnection"].ToString());


                if (Address != "")
                {
                    Address = Address + "," + objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }
                else
                {
                    Address = objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }

            }
            if (dt.Rows[0]["PinCode"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["PinCode"].ToString();
                }

            }
            strCusInfo[0] = Address;


            strCusInfo[2] = dt.Rows[0]["Longitude"].ToString();
            strCusInfo[3] = dt.Rows[0]["Latitude"].ToString();
        }
        else
        {
            strCusInfo[0] = "";
            strCusInfo[2] = "0.0000";
            strCusInfo[3] = "0.0000";
        }

        return strCusInfo;
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
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Fillddlprojectname()
    {


        string Prjectid = string.Empty;


        DataTable dt = objProjctMaster.GetAllProjectMasteer();
        dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();

        DataView dvProjectName = new DataView(dt);
        dvProjectName.Sort = "Project_Name";

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlprojectname, dvProjectName.ToTable(), "Project_Name", "Project_Id");
        }
        else
        {
            ddlprojectname.Items.Insert(0, "--Select--");
            ddlprojectname.SelectedIndex = 0;
        }
    }

    protected void txtvehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtvehiclename.Text.Trim() != "")
        {

            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtvehiclename.Text = "";
                txtvehiclename.Focus();
                return;
            }
            else
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtdrivername.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_Code"].ToString() + "/" + dt.Rows[0]["Emp_id"].ToString();
                }
            }
        }

    }





    protected void txtdrivername_TextChanged(object sender, EventArgs e)
    {



        string empname = string.Empty;
        if (((TextBox)sender).Text != "")
        {
            try
            {
                empname = ((TextBox)sender).Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }

            DataTable dtEmp = objEmpMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empname + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count == 0)
            {
                DisplayMessage("Employee Not Exists");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
                return;
            }
        }
    }

    protected void GvVisitMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilter_Visit"];
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
        Session["dtFilter_Visit"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVisitMaster, dt, "", "");
        //AllPageCode();
    }
    protected void GvVisitMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVisitMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Visit"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvVisitMaster, dt, "", "");
        //AllPageCode();
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedIndex == 1)
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        ddlFieldName.Focus();
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        GvVisitMaster.PageIndex = 0;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Text = "";
        txtValueDate.Text = "";
        BindGrid();
    }

    protected void btnbindrpt_Click(object sender, ImageClickEventArgs e)
    {

        if (ddlFieldName.SelectedIndex == 1)
        {
            if (txtValueDate.Text != "")
            {

                try
                {
                    txtValue.Text = ObjsysParam.getDateForInput(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                txtValue.Text = "";
                return;
            }
        }
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
            DataTable dtProjectteam = (DataTable)Session["dtFilter_Visit"];
            if (dtProjectteam != null)
            {
                DataView view = new DataView(dtProjectteam, condition, "", DataViewRowState.CurrentRows);
                //Common Function add By Lokesh on 23-05-2015
                //objPageCmn.FillData((object)GvVisitMaster, view.ToTable(), "", "");
                GvVisitMaster.DataSource = view.ToTable();
                GvVisitMaster.DataBind();

                Session["dtFilter_Visit"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                //AllPageCode();
            }
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = objVisitMaster.DeleteRecord_By_Trans_Id(e.CommandArgument.ToString());
        DisplayMessage("Record Deleted");
        BindGrid();
    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;

        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }

        DataTable dtvisit = objVisitMaster.GetRecord_By_TransId(e.CommandArgument.ToString());
        if (dtvisit.Rows.Count > 0)
        {
            ddlAddressCategory.SelectedIndex = 0;


            hdnvisitid.Value = e.CommandArgument.ToString();

            ddlReftype.SelectedValue = dtvisit.Rows[0]["Ref_Type"].ToString().Trim();

            ddlReftype_OnSelectedIndexChanged(null, null);

            DataTable dtvisitEmpList = objTaskEmp.GetRecordBy_RefType_and_RefId("Visit", e.CommandArgument.ToString());
            DataTable dtemp = new DataTable();

            if (ddlReftype.SelectedIndex == 1)
            {
                ddlprojectname.SelectedValue = dtvisit.Rows[0]["Project_Id"].ToString();
                ddlprojectname_SelectedIndexChanged(null, null);
                ddltask.SelectedValue = dtvisit.Rows[0]["Ref_Id"].ToString();
                ddltask_OnSelectedIndexChanged(null, null);
            }
            else if (ddlReftype.SelectedIndex == 2)
            {

                DataTable dtTicket = new DataTable();

                dtTicket = objticketmaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

                try
                {
                    dtTicket = new DataView(dtTicket, "Trans_Id=" + dtvisit.Rows[0]["Ref_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {

                }
                if (dtTicket.Rows.Count > 0)
                {
                    txtticketno.Text = dtTicket.Rows[0]["Ticket_No"].ToString();
                    txtticketno_OnTextChanged(null, null);
                }

                hdnTicketid.Value = dtvisit.Rows[0]["Ref_Id"].ToString();
            }
            else if (ddlReftype.SelectedValue.Trim() == "WORK")
            {

                txtworkorder.Text = dtvisit.Rows[0]["RefName"].ToString() + "/" + dtvisit.Rows[0]["Ref_Id"].ToString();
            }


            if (dtvisitEmpList.Rows.Count > 0)
            {
                foreach (ListItem li in listtaskEmployee.Items)
                {

                    dtemp = new DataView(dtvisitEmpList, "Employee_Id=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtemp.Rows.Count > 0)
                    {
                        li.Selected = true;
                    }
                    else
                    {
                        li.Selected = false;
                    }
                }
            }

            txtInvoiceNo.Text = dtvisit.Rows[0]["InvoiceNo"].ToString();

            if (dtvisit.Rows[0]["Field6"].ToString() == "True")
            {
                txtcustomername.Text = dtvisit.Rows[0]["CustomerName"].ToString() + "/" + dtvisit.Rows[0]["Customer_Id"].ToString();
            }
            else
            {
                txtcustomername.Text = dtvisit.Rows[0]["Customer_Id"].ToString();

            }
            txtVisitDate.Text = GetDate(dtvisit.Rows[0]["Visit_Date"].ToString());
            hdnInvoiceId.Value = dtvisit.Rows[0]["InvoiceNo"].ToString();
            txtChargableAmount.Text = dtvisit.Rows[0]["Chargable_Amount"].ToString();
            txtVisitTime.Text = dtvisit.Rows[0]["Visit_Time"].ToString();
            try
            {
                chkIsurgent.Checked = Convert.ToBoolean(dtvisit.Rows[0]["IsUrgent"].ToString());
            }
            catch
            {
                chkIsurgent.Checked = false;
            }
            //get task list from prj_visit_task table 

            DataTable dtVisitTask = objVisitTask.GetRecord_By_VisitId(e.CommandArgument.ToString());
            if (dtVisitTask.Rows.Count > 0)
            {
                dtVisitTask = dtVisitTask.DefaultView.ToTable(true, "Trans_Id", "Task", "Status");

                Session["dtVisitTaskList"] = dtVisitTask;
                LoadVisitTask();
            }
            txtCustomerAddress.Text = dtvisit.Rows[0]["Contact_Address"].ToString();
            txtPermanentMobileNo.Text = dtvisit.Rows[0]["Contact_No"].ToString();
            txtAreaName.Text = dtvisit.Rows[0]["AreaName"].ToString();
            txtLatitude.Text = dtvisit.Rows[0]["Latitude"].ToString();
            txtLongitude.Text = dtvisit.Rows[0]["Longitude"].ToString();
            txtvehiclename.Text = dtvisit.Rows[0]["VehicleName"].ToString() + "/" + dtvisit.Rows[0]["Vehicle_Id"].ToString();
            txtdrivername.Text = dtvisit.Rows[0]["EmpName"].ToString() + "/" + dtvisit.Rows[0]["Emp_Code"].ToString() + "/" + dtvisit.Rows[0]["Driver_Id"].ToString();
            ddlStatus.SelectedValue = dtvisit.Rows[0]["Status"].ToString().Trim();
            txtdescription.Text = dtvisit.Rows[0]["Description"].ToString();
        }
        //AllPageCode();
        btnnew_Click(null, null);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlReftype);
    }


    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        pnlFeedback1.Visible = false;
        pnlFeedback2.Visible = false;
        BindGrid();
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkref = (LinkButton)e.Row.FindControl("lnkstatus");

            if (lnkref.Text.Trim() == "Open")
            {
                lnkref.Style.Add("text-decoration", "none");
                lnkref.Style.Add("cursor", "none");
                lnkref.CssClass = "labelComman";
                lnkref.ForeColor = System.Drawing.ColorTranslator.FromHtml("#474646");
                lnkref.Font.Underline = false;
                lnkref.ToolTip = "";
                //lnkref.Visible = false;
                //e.Row.Cells[8].Text = "Opening Stock";
                //e.Row.Attributes["onmouseover"] = "this.style.cursor='crosshair';this.style.background='#f2f2f2';";
            }
        }
        //AllPageCode();

    }

    #region Geocoe

    protected void lnkstatus_OnCommand(object sender, CommandEventArgs e)
    {
        DataTable dtGeoWork = new DataTable();

        //here we check that GeoWork database is exist or not if not than return the finunction 
        try
        {
            dtGeoWork = objDa.return_DataTable("select * from sys.databases where name='GeoWork'");
        }
        catch
        {

        }

        if (dtGeoWork.Rows.Count > 0)
        {
            pnlFeedback1.Visible = true;
            pnlFeedback2.Visible = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Feedback_Popup()", true);
            string sql = "select r_name as Recevier_Name,r_contact_no as Receiver_ContactNo,feedback as Feedback,is_cancel as Is_Cancel,f_date as F_DateTime,_id as Trans_Id,r_sign from GeoWork.dbo.gw_order where ref_order_no='" + e.CommandArgument.ToString() + "'";
            DataTable dtFeedback = objDa.return_DataTable(sql);
            if (dtFeedback.Rows.Count > 0)
            {
                lblfeedbackDate.Text = dtFeedback.Rows[0]["F_DateTime"].ToString();
                lblReceiverName.Text = dtFeedback.Rows[0]["Recevier_Name"].ToString();
                lblreciverContactNo.Text = dtFeedback.Rows[0]["Receiver_ContactNo"].ToString();
                txtfeedback.Text = dtFeedback.Rows[0]["Feedback"].ToString();
                try
                {
                    imgsignature.Src = "http://91.140.147.22:81/GeoService/myimage/" + dtFeedback.Rows[0]["r_sign"].ToString() + ".png";

                }
                catch
                {
                }
            }
        }
        else
        {
            DisplayMessage("GeoWork database not Found");
            pnlFeedback1.Visible = false;
            pnlFeedback2.Visible = false;
            return;
        }

    }
    public string[] GeoCode(string Address)
    {

        string[] strcordinate = new string[2];
        //to Read the Stream
        StreamReader sr = null;

        //The Google Maps API Either return JSON or XML. We are using XML Here
        //Saving the url of the Google API 
        string url = String.Format("http://maps.googleapis.com/maps/api/geocode/xml?address=" +
        Address + "&sensor=false");

        //to Send the request to Web Client 
        WebClient wc = new WebClient();
        try
        {
            sr = new StreamReader(wc.OpenRead(url));
        }
        catch (Exception ex)
        {
            throw new Exception("The Error Occured" + ex.Message);
        }

        try
        {
            XmlTextReader xmlReader = new XmlTextReader(sr);
            bool latread = false;
            bool longread = false;

            while (xmlReader.Read())
            {
                xmlReader.MoveToElement();
                switch (xmlReader.Name)
                {
                    case "lat":

                        if (!latread)
                        {
                            xmlReader.Read();

                            //latitude
                            strcordinate[0] = xmlReader.Value.ToString();
                            latread = true;

                        }
                        break;
                    case "lng":
                        if (!longread)
                        {

                            //longitude
                            xmlReader.Read();
                            strcordinate[1] = xmlReader.Value.ToString();
                            longread = true;
                        }

                        break;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An Error Occured" + ex.Message);
        }
        return strcordinate;
    }
    public void insertRecordInGeoWorkdatabse(string EmpId, string VisitId, ref SqlTransaction trns)
    {
        DataTable dtGeoWork = new DataTable();

        //here we check that GeoWork database is exist or not if not than return the finunction 
        try
        {
            dtGeoWork = objDa.return_DataTable("select * from sys.databases where name='GeoWork'", ref trns);
        }
        catch
        {

        }

        if (dtGeoWork.Rows.Count > 0)
        {

            string Sql = string.Empty;
            string strRegistrationid = ConfigurationManager.AppSettings["RegistrationId"].ToString();

            DataTable dtEmployee = objEmpMaster.GetEmployeeMasterAll(Session["CompId"].ToString(), ref trns);
            try
            {
                dtEmployee = new DataView(dtEmployee, "Emp_id=" + EmpId + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtEmployee.Rows.Count > 0)
            {

                //first we chekc that user exist or not in getracker database

                if (objDa.return_DataTable("select * from GeoWork.dbo.gw_user where user_code='" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "'", ref trns).Rows.Count == 0)
                {
                    Sql = "insert into GeoWork.dbo.gw_user values('" + strRegistrationid + "','1','" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','" + dtEmployee.Rows[0]["Emp_Name"].ToString() + "','" + dtEmployee.Rows[0]["Email_Id"].ToString() + "','" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','0','" + dtEmployee.Rows[0]["Phone_No"].ToString() + "','0','0','True','0','" + DateTime.Now.ToString() + "')";
                    objDa.execute_Command(Sql, ref trns);
                }
                //here we chekc that record is exist or not in trn_orderlit table
                //if exist than update otherwise insert

                DateTime dtDate = Convert.ToDateTime(txtVisitDate.Text);
                string strhour = string.Empty;
                string strminute = string.Empty;
                strhour = txtVisitTime.Text.Split(':')[0].ToString();
                strminute = txtVisitTime.Text.Split(':')[1].ToString();


                DateTime dtvisitDate = new DateTime(dtDate.Year, dtDate.Month, dtDate.Day, Convert.ToInt32(strhour), Convert.ToInt32(strminute), 0);
                string strDefaultMessage = string.Empty;

                //herw we chekc that record exist or not in Trns_Order_List table 
                if (objDa.return_DataTable("select * from GeoWork.dbo.gw_order where ref_order_no='" + VisitId + "'", ref trns).Rows.Count == 0)
                {
                    //" + txtPermanentMobileNo.Text.ToString().Substring(3, txtPermanentMobileNo.Text.ToString().Length ).ToString () + "
                    Sql = "insert into GeoWork.dbo.gw_order values('" + strRegistrationid + "','Loc-0001','" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','" + DateTime.Now.ToString() + "','" + VisitId + "','" + txtcustomername.Text.Split('/')[0].ToString() + "','" + txtPermanentMobileNo.Text.Trim() + "','" + txtCustomerAddress.Text + "','" + txtLatitude.Text + "','" + txtLongitude.Text + "','" + dtvisitDate.ToString() + "','" + txtdescription.Text + "','" + chkIsurgent.Checked.ToString() + "','False','False','" + (new DateTime(1990, 1, 1)).ToString() + "',' ',' ',' ','0','0','0','True','1','" + DateTime.Now.ToString() + "')";

                    objDa.execute_Command(Sql, ref trns);

                    string RefId = objDa.return_DataTable("select max(_id) as _id from GeoWork.dbo.gw_order", ref trns).Rows[0]["_id"].ToString();

                    strDefaultMessage += "New Order Created!";

                    //for insert record in emp detail
                    int i = 0;
                    foreach (ListItem li in listtaskEmployee.Items)
                    {
                        if (li.Selected)
                        {
                            i++;
                            Sql = "insert into GeoWork.dbo.gw_order_emp values('" + strRegistrationid + "','" + VisitId + "','" + i.ToString() + "','" + li.Text.Split('/')[0].ToString() + "',' ')";
                            objDa.execute_Command(Sql, ref trns);
                        }
                    }
                    //for insert record in order detail

                    if (Session["dtVisitTaskList"] != null)
                    {
                        i = 0;
                        foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                        {
                            i++;
                            Sql = "insert into GeoWork.dbo.gw_order_detail values('" + strRegistrationid + "','" + VisitId + "','" + i.ToString() + "','" + dr["Task"].ToString() + "','1','0','0','Description')";
                            objDa.execute_Command(Sql, ref trns);

                        }
                    }
                }
                else
                {
                    string OrderTransId = objDa.return_DataTable("select * from GeoWork.dbo.gw_order where ref_order_no='" + VisitId + "'", ref trns).Rows[0]["_id"].ToString();

                    Sql = "update GeoWork.dbo.gw_order set user_code='" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "',description='" + txtdescription.Text + "',contact_person='" + txtcustomername.Text.Split('/')[0].ToString() + "',mobile_no='" + txtPermanentMobileNo.Text + "',address='" + txtCustomerAddress.Text + "',appointment_time='" + dtvisitDate.ToString() + "',latitute='" + txtLatitude.Text + "',longitute='" + txtLongitude.Text + "',is_urgent='" + chkIsurgent.Checked.ToString() + "', edited_by='" + VisitId + "',edited_date='" + DateTime.Now.ToString() + "' where _id=" + OrderTransId + "";
                    objDa.execute_Command(Sql, ref trns);



                    //for reinsert record in emp detail first delete 
                    Sql = "delete from GeoWork.dbo.gw_order_emp where ref_order_no=" + VisitId + " and registration_code='" + strRegistrationid + "'";
                    objDa.execute_Command(Sql, ref trns);
                    int i = 0;
                    foreach (ListItem li in listtaskEmployee.Items)
                    {
                        if (li.Selected)
                        {
                            i++;
                            Sql = "insert into GeoWork.dbo.gw_order_emp values('" + strRegistrationid + "','" + VisitId + "','" + i.ToString() + "','" + li.Text.Split('/')[0].ToString() + "',' ')";
                            objDa.execute_Command(Sql, ref trns);
                        }
                    }

                    //for insert record in order detail
                    Sql = "delete from GeoWork.dbo.gw_order_detail where ref_order_no=" + VisitId + " and registration_code='" + strRegistrationid + "'";
                    objDa.execute_Command(Sql, ref trns);
                    if (Session["dtVisitTaskList"] != null)
                    {
                        i = 0;
                        foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                        {
                            i++;
                            Sql = "insert into GeoWork.dbo.gw_order_detail values('" + strRegistrationid + "','" + VisitId + "','" + i.ToString() + "','" + dr["Task"].ToString() + "','1','0','0','Description')";
                            objDa.execute_Command(Sql, ref trns);

                        }
                    }
                }

                //code created by jitendra upadhyay on 20-01-2016
                //for notification
                //code start

                if (ddlStatus.SelectedValue == "Close")
                {
                    strDefaultMessage += "Order Closed!";
                }
                else if (ddlStatus.SelectedValue == "Cancel")
                {
                    strDefaultMessage += "Order Canceled!";
                }
                else
                {
                    // strDefaultMessage += "Order Updated!";
                }

                try
                {

                    string strSql = "select * from GeoWork.dbo.gw_user where user_code='" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "'";
                    DataTable dtuser = objDa.return_DataTable(strSql, ref trns);
                    if (dtuser.Rows.Count > 0)
                    {
                        if (dtuser.Rows[0]["device_Type"].ToString() != "" && dtuser.Rows[0]["device_Type"].ToString() != "0")
                        {

                            if (txtdescription.Text.Trim().ToString().Length > 0)
                            {
                                ThreadStart ts = delegate () { SendAlerts(dtuser.Rows[0]["device_Type"].ToString().Trim(), dtuser.Rows[0]["device_code"].ToString().Trim(), txtdescription.Text); };

                                // The thread.
                                Thread t = new Thread(ts);

                                // Run the thread.
                                t.Start();

                                strSql = "INSERT INTO [GeoWork].[dbo].[gw_notification]           ([registration_code]           ,[user_code]           ,[message_to]           ,[m_date]           ,[message]           ,[is_active]           ,[edited_by]           ,[edited_date])     VALUES           ('" + strRegistrationid + "','" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','" + getLoginEmployeeName() + "','" + DateTime.Now.ToString() + "','" + txtdescription.Text + "','" + true.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')";
                                objDa.execute_Command(strSql, ref trns);
                            }
                            if (strDefaultMessage.Length > 0)
                            {
                                ThreadStart ts1 = delegate () { SendAlerts(dtuser.Rows[0]["device_Type"].ToString().Trim(), dtuser.Rows[0]["device_code"].ToString().Trim(), strDefaultMessage); };

                                // The thread.
                                Thread t1 = new Thread(ts1);

                                // Run the thread.
                                t1.Start();

                                strSql = "INSERT INTO [GeoWork].[dbo].[gw_notification]           ([registration_code]           ,[user_code]           ,[message_to]           ,[m_date]           ,[message]           ,[is_active]           ,[edited_by]           ,[edited_date])     VALUES           ('" + strRegistrationid + "','" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','" + getLoginEmployeeName() + "','" + DateTime.Now.ToString() + "','" + strDefaultMessage + "','" + true.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')";
                                objDa.execute_Command(strSql, ref trns);
                            }
                            //if (dtuser.Rows[0]["device_Type"].ToString().Trim() == "1")
                            //{
                            //    obj.SendAndroidMessage(dtuser.Rows[0]["device_code"].ToString().Trim(), txtdescription.Text);
                            //}
                            //if (dtuser.Rows[0]["device_Type"].ToString().Trim() == "2")
                            //{
                            //    obj.SendIOSMessage(dtuser.Rows[0]["device_code"].ToString().Trim(), txtdescription.Text);
                            //}


                        }
                    }
                }
                catch
                {
                }
                //code end

                //code for update status in geo table when close or cancel order by Transport page
                //code created by jitendra upadhyay on 12-02-2016

                string strSqlCmd = string.Empty;
                if (ddlStatus.SelectedValue == "Close")
                {
                    strSqlCmd = "update GeoWork.dbo.gw_order set is_finished='True',f_date='" + DateTime.Now.ToString() + "',r_name='" + getLoginEmployeeName() + "',r_contact_no='0',feedback='Close by Web App',r_latitute='0',r_longitute='0',r_sign='0',edited_by='" + Session["UserId"].ToString() + "',edited_date='" + DateTime.Now.ToString() + "'  where ref_order_no=" + VisitId + "";
                    objDa.execute_Command(strSqlCmd, ref trns);
                }
                if (ddlStatus.SelectedValue == "Cancel")
                {
                    strSqlCmd = "update GeoWork.dbo.gw_order set is_cancel='True',f_date='" + DateTime.Now.ToString() + "',r_name='" + getLoginEmployeeName() + "',r_contact_no='0',feedback='Cancel by Web App',r_latitute='0',r_longitute='0',r_sign='0',edited_by='" + Session["UserId"].ToString() + "',edited_date='" + DateTime.Now.ToString() + "'   where ref_order_no=" + VisitId + "";
                    objDa.execute_Command(strSqlCmd, ref trns);
                }
            }
        }
    }

    void SendAlerts(string strDeviceType, string strDeviceCode, string strMessage)
    {
        NotificationService.MessageService obj = new NotificationService.MessageService();
        if (strDeviceType == "1")
        {
            try
            {
                obj.SendAndroidMessage(strDeviceCode, strMessage);
            }
            catch
            {
            }
        }
        else if (strDeviceType == "w")
        {
            try
            {
                obj.SendIOSMessage(strDeviceCode, strMessage);
            }
            catch
            {
            }
        }



        //SendNotifcation objSN = new SendNotifcation();
        //objSN.SendAndroidNotifcation(strC, strT);
        //objSN.SendIOSNotification(strC, strT);
    }

    public string getLoginEmployeeName()
    {
        string strEmpName = string.Empty;



        if (Session["EmpId"].ToString().Trim() == "0")
        {
            strEmpName = "Superadmin";
        }
        else
        {
            try
            {
                strEmpName = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Emp_Name"].ToString();
            }
            catch
            {
                strEmpName = "";
            }

        }
        return strEmpName;
    }

    #region getLatitudeandlongitude
    protected void BtnUpdateLatLong_Click(object sender, EventArgs e)
    {

        if (Session["Long"] != null && Session["Lati"] != null)
        {
            txtLongitude.Text = Session["Long"].ToString();
            txtLatitude.Text = Session["Lati"].ToString();
            Session["Long"] = null;
            Session["Lati"] = null;
        }
        else
        {
            try
            {
                if (txtLatitude.Text.Contains("0.0000") && txtLatitude.Text.Contains("0.0000"))
                {
                    //string FullAddress = txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;
                    string FullAddress = txtCustomerAddress.Text;


                    string url = "http://maps.google.com/maps/api/geocode/xml?address=" + FullAddress + "&sensor=false";
                    WebRequest request = WebRequest.Create(url);
                    using (WebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            DataSet dsResult = new DataSet();
                            dsResult.ReadXml(reader);
                            DataTable dtCoordinates = new DataTable();
                            dtCoordinates.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                        new DataColumn("Address", typeof(string)),
                        new DataColumn("Latitude",typeof(string)),
                        new DataColumn("Longitude",typeof(string)) });
                            foreach (DataRow row in dsResult.Tables["result"].Rows)
                            {
                                string geometry_id = dsResult.Tables["geometry"].Select("result_id = " + row["result_id"].ToString())[0]["geometry_id"].ToString();
                                DataRow location = dsResult.Tables["location"].Select("geometry_id = " + geometry_id)[0];
                                dtCoordinates.Rows.Add(row["result_id"], row["formatted_address"], location["lat"], location["lng"]);
                            }
                            txtLatitude.Text = dtCoordinates.Rows[0]["Latitude"].ToString();
                            txtLongitude.Text = dtCoordinates.Rows[0]["Longitude"].ToString();
                            Session["Long"] = null;
                            Session["Lati"] = null;
                        }
                    }
                }
                else
                {
                    Session["Long"] = txtLongitude.Text;
                    Session["Lati"] = txtLatitude.Text;

                }
            }
            catch
            {

            }
        }
    }
    protected void btnGetLatLong_Click(object sender, EventArgs e)
    {



        string FullAddress = txtCustomerAddress.Text;
        if (txtLatitude.Text.Contains("0.0000") && txtLatitude.Text.Contains("0.0000"))
        {
            //string FullAddress = txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;


            Session["Add"] = FullAddress;

        }
        else
        {
            Session["Long"] = txtLongitude.Text;
            Session["Lati"] = txtLatitude.Text;
            Session["Add"] = FullAddress;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../SystemSetup/GoogleMap.aspx','window','width=1024')", true);

    }
    #endregion
    #endregion
    protected void btnsave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        btnsave.Enabled = true;
        string strRefId = string.Empty;
        string strProjectId = string.Empty;

        //if ref type selected index = 0 means by direct
        if (ddlReftype.SelectedIndex == 0)
        {
            strRefId = "0";
            strProjectId = "0";
        }
        //if ref type selected index = 1 means by Project and Task
        else if (ddlReftype.SelectedIndex == 1)
        {
            if (ddlprojectname.SelectedIndex == 0)
            {
                DisplayMessage("Select Project Name");
                ddlprojectname.Focus();
                return;
            }
            else
            {
                strProjectId = ddlprojectname.SelectedValue;

            }

            if (ddltask.SelectedIndex == 0)
            {
                DisplayMessage("Select Task");
                ddltask.Focus();
                return;
            }
            else
            {
                strRefId = ddltask.SelectedValue;
            }
        }
        //if ref type selected index = 2 means by ticket No.
        else if (ddlReftype.SelectedIndex == 2)
        {
            if (txtticketno.Text == "")
            {
                DisplayMessage("Enter Ticket No.");
                txtticketno.Focus();
                return;
            }
            else
            {
                strRefId = hdnTicketid.Value;
                strProjectId = "0";
            }
        }
        else if (ddlReftype.SelectedIndex == 3)
        {
            if (txtworkorder.Text.Trim() == "")
            {
                DisplayMessage("Enter work order No.");
                txtworkorder.Focus();
                return;
            }
            else
            {
                strProjectId = "0";
                strRefId = txtworkorder.Text.Split('/')[1].ToString();
            }
        }


        string StrCustomer = string.Empty;
        bool strrValues = false;

        if (txtcustomername.Text == "")
        {
            DisplayMessage("Enter Customer name");
            txtcustomername.Focus();
            return;
        }
        else
        {
            try
            {
                StrCustomer = txtcustomername.Text.Split('/')[1].ToString();
                strrValues = true;
            }
            catch
            {
                StrCustomer = txtcustomername.Text;
                strrValues = false;
            }
        }

        if (txtCustomerAddress.Text == "")
        {
            DisplayMessage("Enter Customer Address");
            txtCustomerAddress.Focus();
            return;
        }

        if (txtCustomerAddress.Text == "")
        {
            DisplayMessage("Enter Customer Address");
            txtCustomerAddress.Focus();
            return;
        }

        if (txtLatitude.Text == "" || txtLongitude.Text == "" || txtLatitude.Text == "0.000000" || txtLongitude.Text == "0.000000")
        {
            DisplayMessage("Set Latitude and longitude");
            return;
        }






        if (txtVisitDate.Text == "")
        {
            DisplayMessage("Enter Visit Date");
            txtVisitDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtVisitDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Visit Date");
                txtVisitDate.Focus();
                return;
            }

        }
        if (txtVisitTime.Text == "")
        {
            DisplayMessage("Enter Visit time");
            txtVisitTime.Focus();
            return;
        }

        if (txtvehiclename.Text == "")
        {
            DisplayMessage("Enter Vehicle Name");
            txtvehiclename.Focus();
            return;
        }

        if (txtdrivername.Text == "")
        {
            DisplayMessage("Enter Driver Name");
            txtdrivername.Focus();
            return;
        }

        if (txtInvoiceNo.Text == "")
        {
            hdnInvoiceId.Value = "0";
        }



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();



        try
        {
            //insert record in area master 

            //if record not exist in area than we insert in area master table than  record will be showing in suggestion when we insert visit next time for this visit
            //code start
            string StrAreaId = "0";

            if (txtAreaName.Text != "")
            {
                if (IsAreaExists(txtAreaName.Text, ref trns) == "0")
                {
                    int c = objAreamaster.InsertAreaMaster(txtAreaName.Text.Trim(), txtAreaName.Text, "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    StrAreaId = c.ToString();
                }
                else
                {
                    StrAreaId = IsAreaExists(txtAreaName.Text, ref trns);
                }
            }
            //code end
            //here we wriore insert record
            if (hdnvisitid.Value == "")
            {
                int b = 0;
                b = objVisitMaster.InsertRecord(ddlReftype.SelectedValue, strRefId, strProjectId, StrCustomer, ObjsysParam.getDateForInput(txtVisitDate.Text).ToString(), txtvehiclename.Text.Split('/')[1].ToString(), txtdrivername.Text.Split('/')[2].ToString(), "0", ddlStatus.SelectedValue, txtdescription.Text, "", hdnInvoiceId.Value, txtChargableAmount.Text, txtVisitTime.Text, chkIsurgent.Checked.ToString(), txtCustomerAddress.Text, txtPermanentMobileNo.Text, StrAreaId, txtLongitude.Text, txtLatitude.Text, "", "", "", "", "", strrValues.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {

                    InsertVisitEmployee("Visit", b.ToString(), ref trns);
                    insertRecordInGeoWorkdatabse(txtdrivername.Text.Split('/')[2].ToString(), b.ToString(), ref trns);
                    //insert record in prj_visit_task table

                    //code created by jitendra upadhyay for link the selected latitude and longitude with exist contact


                    //code start

                    //09-02-2016


                    if (Convert.ToBoolean(strrValues))
                    {

                        DataTable dtaddRef = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", StrCustomer, ref trns);


                        if (dtaddRef.Rows.Count > 0)
                        {
                            try
                            {
                                dtaddRef = new DataView(dtaddRef, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {
                            }
                            if (dtaddRef.Rows.Count > 0)
                            {
                                string strsql = "update Set_AddressMaster set Latitude='" + txtLatitude.Text + "' ,Longitude='" + txtLongitude.Text + "' where Trans_Id=" + dtaddRef.Rows[0]["Trans_Id1"].ToString() + "";
                                objDa.execute_Command(strsql, ref trns);

                            }
                        }
                        else
                        {


                        }

                    }


                    //code end

                    if (Session["dtVisitTaskList"] != null)
                    {
                        int i = 0;
                        foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                        {
                            i++;
                            objVisitTask.InsertRecord(b.ToString(), i.ToString(), dr["Task"].ToString(), dr["Status"].ToString(), ref trns);
                        }
                    }

                    DisplayMessage("Record Saved","green");
                }
            }
            else
            {
                //here we write updated code
                objVisitMaster.UpdateRecord(hdnvisitid.Value, ddlReftype.SelectedValue, strRefId, strProjectId, StrCustomer, ObjsysParam.getDateForInput(txtVisitDate.Text).ToString(), txtvehiclename.Text.Split('/')[1].ToString(), txtdrivername.Text.Split('/')[2].ToString(), "0", ddlStatus.SelectedValue, txtdescription.Text, "", hdnInvoiceId.Value, txtChargableAmount.Text, txtVisitTime.Text, chkIsurgent.Checked.ToString(), txtCustomerAddress.Text, txtPermanentMobileNo.Text, StrAreaId, txtLongitude.Text, txtLatitude.Text, "", "", "", "", "", strrValues.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                InsertVisitEmployee("Visit", hdnvisitid.Value, ref trns);
                insertRecordInGeoWorkdatabse(txtdrivername.Text.Split('/')[2].ToString(), hdnvisitid.Value, ref trns);
                //insert record in prj_visit_task table
                //firts delete previos record by visit id
                objVisitTask.DeleteRecord_By_VisitId(hdnvisitid.Value, ref trns);

                if (Session["dtVisitTaskList"] != null)
                {
                    int i = 0;
                    foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                    {
                        i++;
                        objVisitTask.InsertRecord(hdnvisitid.Value, i.ToString(), dr["Task"].ToString(), dr["Status"].ToString(), ref trns);
                    }
                }
                btList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            BindGrid();
            //AllPageCode();
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
            return;
        }
    }
    public string IsAreaExists(string strAreaName, ref SqlTransaction trns)
    {
        string Result = "0";

        DataTable dt = objAreamaster.GetAreaMaster(ref trns);

        dt = new DataView(dt, "Area_Name='" + strAreaName + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            Result = dt.Rows[0]["Trans_Id"].ToString();
        }

        return Result;
    }
    public void InsertVisitEmployee(string RefType, string RefId, ref SqlTransaction trns)
    {

        //this function for insert record in task_employee

        //first delete record by task_id
        objTaskEmp.DeleteRecord_By_RefTypeandRefid(RefType, RefId, ref trns);

        foreach (ListItem li in listtaskEmployee.Items)
        {
            if (li.Selected)
            {
                objTaskEmp.InsertRecord(RefType, RefId, li.Value, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
        }

    }
    protected void ddlReftype_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlReftype.SelectedIndex == 0)
        {
            trprojectlist.Visible = false;
            trtasklist.Visible = false;
            lnkTaskdesc.Visible = false;
            trticketlist.Visible = false;
            lnkticketdesc.Visible = false;
            trtaskDetail.Visible = false;
            trTicketDetail.Visible = false;
            trWorkOrder.Visible = false;
            txtInvoiceNo.Text = "";
            txtChargableAmount.Text = "";
            FillEmployee();
        }

        else if (ddlReftype.SelectedIndex == 1)
        {
            trprojectlist.Visible = true;
            trtasklist.Visible = true;
            Fillddlprojectname();
            ddltask.Items.Clear();
            lnkTaskdesc.Visible = false;
            trticketlist.Visible = false;
            lnkticketdesc.Visible = false;
            trtaskDetail.Visible = false;
            trTicketDetail.Visible = false;
            txtInvoiceNo.Text = "";
            txtChargableAmount.Text = "";
            trWorkOrder.Visible = false;
            FillEmployee();
        }
        else if (ddlReftype.SelectedIndex == 2)
        {
            trprojectlist.Visible = false;
            trtasklist.Visible = false;
            lnkTaskdesc.Visible = false;
            txtticketno.Text = "";
            trticketlist.Visible = true;
            lnkticketdesc.Visible = false;
            trtaskDetail.Visible = false;
            trTicketDetail.Visible = false;
            trWorkOrder.Visible = false;
            txtInvoiceNo.Text = "";
            txtChargableAmount.Text = "";
            FillEmployee();
        }
        else if (ddlReftype.SelectedIndex == 3)
        {
            trprojectlist.Visible = false;
            trtasklist.Visible = false;
            lnkTaskdesc.Visible = false;
            trticketlist.Visible = false;
            lnkticketdesc.Visible = false;
            trtaskDetail.Visible = false;
            trTicketDetail.Visible = false;
            txtInvoiceNo.Text = "";
            txtChargableAmount.Text = "";
            FillEmployee();
            trWorkOrder.Visible = true;
            txtworkorder.Text = "";

        }
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    protected void btncencel_Click(object sender, EventArgs e)
    {
        Reset();
        btList_Click(null, null);
        BindGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void txtticketno_OnTextChanged(object sendre, EventArgs e)
    {
        if (txtticketno.Text != "")
        {

            DataTable dtTicket = new DataTable();

            dtTicket = objticketmaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

            try
            {
                dtTicket = new DataView(dtTicket, "Ticket_No='" + txtticketno.Text + "' and Status='Open' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
            if (dtTicket.Rows.Count > 0)
            {
                txtInvoiceNo.Text = dtTicket.Rows[0]["InvoiceNo"].ToString();
                hdnInvoiceId.Value = dtTicket.Rows[0]["Field2"].ToString();
                txtChargableAmount.Text = dtTicket.Rows[0]["Chargeable_Amount"].ToString();
                lnkticketdesc.Visible = true;

                hdnTicketid.Value = dtTicket.Rows[0]["Trans_Id"].ToString();
                trTicketDetail.Visible = true;
                lblTickeDate.Text = GetDate(dtTicket.Rows[0]["Ticket_Date"].ToString());
                lblStatus.Text = dtTicket.Rows[0]["Status"].ToString();
                lblTaskType.Text = dtTicket.Rows[0]["Task_Type"].ToString();
                lblCustomerNameValue.Text = dtTicket.Rows[0]["CustomerName"].ToString();
                if (dtTicket.Rows[0]["Field6"].ToString() == "True")
                {
                    txtcustomername.Text = dtTicket.Rows[0]["CustomerName"].ToString() + "/" + dtTicket.Rows[0]["Customer_Name"].ToString();
                }
                else
                {
                    txtcustomername.Text = dtTicket.Rows[0]["CustomerName"].ToString();

                }
                txtcustomername_TextChanged(null, null);
                lblScheduledate.Text = GetDate(dtTicket.Rows[0]["Schedule_Date"].ToString());
                lblDescriptionvalue.Text = dtTicket.Rows[0]["Description"].ToString();
                DataTable dtTicketEmp = objTicketEmployee.GetAllRecord_ByTicketId(hdnTicketid.Value);
                if (dtTicketEmp.Rows.Count > 0)
                {
                    dtTicketEmp = dtTicketEmp.DefaultView.ToTable(true, "Employee_Id", "EmpName");
                    objPageCmn.FillData((object)listtaskEmployee, dtTicketEmp, "EmpName", "Employee_Id");
                }
            }
            else
            {
                DisplayMessage("Ticket Not Found");
                txtticketno.Text = "";
                txtticketno.Focus();
                txtInvoiceNo.Text = "";
                txtChargableAmount.Text = "";
                lnkticketdesc.Visible = false;
                trTicketDetail.Visible = false;
                return;
            }

        }
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    public void Reset()
    {
        Fillddlprojectname();
        ddltask.Items.Clear();
        lnkTaskdesc.Visible = false;
        FillEmployee();
        txtcustomername.Text = "";
        txtVisitDate.Text = "";
        txtVisitTime.Text = "";
        txtvehiclename.Text = "";
        txtdrivername.Text = "";
        ddlStatus.SelectedIndex = 0;
        txtdescription.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtVisitDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        hdnvisitid.Value = "";
        ddlReftype.SelectedIndex = 0;
        ddlReftype_OnSelectedIndexChanged(null, null);
        txtInvoiceNo.Text = "";
        txtChargableAmount.Text = "";
        //AllPageCode();
        chkIsurgent.Checked = false;
        txtCustomerAddress.Text = "";
        txtPermanentMobileNo.Text = "";
        Session["Long"] = null;
        Session["Lati"] = null;
        txtAreaName.Text = "";
        txtLatitude.Text = "0.0000";
        txtLongitude.Text = "0.0000";
        Session["dtVisitTaskList"] = null;
        LoadVisitTask();
        btnsave.Enabled = true;
        ddlAddressCategory.SelectedIndex = 0;
    }
    protected void ddlStatusFilter_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void txtInvoiceNo_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dtInvoice = new DataTable();
        if (txtInvoiceNo.Text != "")
        {

            try
            {
                dtInvoice = objSalesInvoiceheader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInvoiceNo.Text);
            }
            catch
            {
            }


            if (dtInvoice.Rows.Count > 0)
            {
                hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();
                if (txtChargableAmount.Text == "")
                {
                    txtChargableAmount.Text = ObjsysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), dtInvoice.Rows[0]["GrandTotal"].ToString());
                }
            }
            else
            {
                DisplayMessage("Invoice Not Found");
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Focus();
                return;
            }
        }

    }
    #region ServiceMethos
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesInvoiceHeader objSinvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objSinvoiceHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Invoice_No"].ToString();
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objSinvoiceHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Invoice_No"].ToString();
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomerName(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtContact = ObjContactMaster.GetContactTrueAllData();


        string filtertext = "Name like '%" + prefixText + "%'";
        dtContact = new DataView(dtContact, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtContact.Rows.Count];
        if (dtContact.Rows.Count > 0)
        {
            for (int i = 0; i < dtContact.Rows.Count; i++)
            {
                filterlist[i] = dtContact.Rows[i]["Name"].ToString() + "/" + dtContact.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDriverName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTicketNo(string prefixText, int count, string contextKey)
    {
        SM_Ticket_Master objTickeMaster = new SM_Ticket_Master(HttpContext.Current.Session["DBConnection"].ToString());
        SM_TicketEmployee objTicketemployee = new SM_TicketEmployee(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtTicket = objTickeMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        try
        {
            dtTicket = new DataView(dtTicket, "Status='Open' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


        string[] filterlist = new string[dtTicket.Rows.Count];
        if (dtTicket.Rows.Count > 0)
        {
            for (int i = 0; i < dtTicket.Rows.Count; i++)
            {
                filterlist[i] = dtTicket.Rows[i]["Ticket_No"].ToString();
            }
        }

        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAreaName(string prefixText, int count, string contextKey)
    {
        Sys_AreaMaster objAreaMaster = new Sys_AreaMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAreaMaster.GetAreaMaster(), "Area_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Area_Name"].ToString();
        }
        return txt;
    }
    #endregion
    #region addproduct
    public void LoadVisitTask()
    {
        DataTable dt = new DataTable();
        if (Session["dtVisitTaskList"] != null)
        {


            dt = new DataTable();
            dt = (DataTable)Session["dtVisitTaskList"];



            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("Task", typeof(string));
                contacts.Columns.Add("Status", typeof(bool));

                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
                int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
                gvVisitTask.Rows[0].Cells.Clear();
                gvVisitTask.Rows[0].Cells.Add(new TableCell());
                gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvVisitTask.Rows[0].Visible = false;
            }

        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("Task", typeof(string));
            contacts.Columns.Add("Status", typeof(bool));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
            int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
            gvVisitTask.Rows[0].Cells.Clear();
            gvVisitTask.Rows[0].Cells.Add(new TableCell());
            gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvVisitTask.Rows[0].Visible = false;
        }



        foreach (GridViewRow gvrow in gvVisitTask.Rows)
        {
            int counter = 0;
            if (Session["dtVisitTaskList"] != null)
            {
                DataTable dtStatus = (DataTable)Session["dtVisitTaskList"];
                try
                {
                    dtStatus = new DataView(dtStatus, "Trans_Id=" + ((Label)gvrow.FindControl("lblTransId")).Text + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtStatus.Rows.Count > 0)
                {

                    if (Convert.ToBoolean(dtStatus.Rows[0]["Status"].ToString()))
                    {
                        counter = 1;
                    }
                }

            }

            if (counter == 1)
            {
                try
                {
                    ((CheckBox)gvrow.FindControl("chkItemStatus")).Checked = true;
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    ((CheckBox)gvrow.FindControl("chkItemStatus")).Checked = false;
                }
                catch
                {

                }
            }

        }

    }
    protected void gvVisitTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {



        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text == "")
            {
                DisplayMessage("Enter Task");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Focus();
                return;
            }



            if (Session["dtVisitTaskList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Task", typeof(string));
                dt.Columns.Add("Status", typeof(bool));

                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text;
                dr[2] = ((CheckBox)gvVisitTask.FooterRow.FindControl("chkFooterStatus")).Checked;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtVisitTaskList"];
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
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text;
                dr[2] = ((CheckBox)gvVisitTask.FooterRow.FindControl("chkFooterStatus")).Checked;
                dt.Rows.Add(dr);
            }
            Session["dtVisitTaskList"] = dt;
            gvVisitTask.EditIndex = -1;
            LoadVisitTask();
        }
        if (e.CommandName.Equals("Delete"))
        {

            if (Session["dtVisitTaskList"] != null)
            {
                dt = (DataTable)Session["dtVisitTaskList"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtVisitTaskList"] = dt;
            }
            gvVisitTask.EditIndex = -1;
            LoadVisitTask();
        }

        ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Focus();



    }
    protected void gvVisitTask_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVisitTask.EditIndex = e.NewEditIndex;
        LoadVisitTask();
    }
    protected void gvVisitTask_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVisitTask.EditIndex = -1;
        LoadVisitTask();
    }
    protected void gvVisitTask_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtVisitTaskList"];

        GridViewRow row = gvVisitTask.Rows[e.RowIndex];

        dt.Rows[row.DataItemIndex]["Task"] = ((TextBox)row.FindControl("txteditTask")).Text;
        dt.Rows[row.DataItemIndex]["Status"] = ((CheckBox)row.FindControl("chkeditItemStatus")).Checked;

        Session["dtVisitTaskList"] = dt;
        gvVisitTask.EditIndex = -1;
        LoadVisitTask();
    }
    protected void gvVisitTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    #endregion
    #region Report
    protected void btnreport_OnClick(object sender, EventArgs e)
    {

        if (Session["dtFilter_Visit"] != null)
        {

            if (((DataTable)Session["dtFilter_Visit"]).Rows.Count == 0)
            {
                DisplayMessage("Record Not Found");
                return;
            }
            else
            {
                Session["dtTransport"] = Session["dtFilter_Visit"];


                string strCmd = string.Format("window.open('../ProjectManagement_Report/Transport_Report.aspx','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;

        }
    }
    #endregion
    #region Timer
    //protected void Timer1_Tick(object sender, EventArgs e)
    //{
    //    //Session["Size"] = ddlSelectRecord.SelectedValue;
    //    //GetLog(ddlSelectRecord.SelectedValue);
    //    UpdatevisitmastertableStatus();
    //    AllPageCode();
    //}
    #endregion

    #region Trackuser

    protected void btnTrackUser_Click(object sender, EventArgs e)
    {

        pnlprojectrecord.Visible = false;
        pnllist.Visible = false;
        pnlTabtrackuser.Visible = true;
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlReport.Visible = false;


        //AllPageCode();
    }
    protected void btnTrack_OnClick(object sender, EventArgs e)
    {
        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid date");
                return;
            }
        }
        else
        {
            DisplayMessage("Enter From  Date");
            txtFromDate.Focus();
            return;
        }
        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid date");
                return;
            }
        }
        else
        {
            DisplayMessage("Enter to  Date");
            txtToDate.Focus();
            return;
        }

        if (txtemployeename.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtemployeename.Focus();
            return;
        }


        ArrayList al = new ArrayList();
        al.Add(txtemployeename.Text.Split('/')[1].ToString());
        al.Add(txtFromDate.Text);
        al.Add(txtToDate.Text);
        Session["TrackParam"] = al;

        string strsql = "SELECT   _id ,latitute as lat ,longitute as lng , user_code as title , '' as description FROM  GeoWork.dbo.gw_geocoords Where user_code ='" + al[0].ToString() + "' and ((CONVERT(Date,t_date)>=CONVERT(Date,'" + al[1].ToString() + "')) and (CONVERT(Date,t_date)<=CONVERT(Date,'" + al[2].ToString() + "') ))order by _id Desc";
        DataTable dtRecord = objDa.return_DataTable(strsql);

        if (dtRecord.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Mastersetup/ShowTrack.aspx','window','width=1024');", true);

    }
    protected void btnLiveTrack_OnClick(object sender, EventArgs e)
    {

        ArrayList ObjArr = new ArrayList();
        ObjArr.Add(txtemployeename.Text);
        Session["LiveTrackParam"] = ObjArr;

        DataTable dt = objDa.return_DataTable("SELECT p.User_code, (select set_employeemaster.Emp_Name from Pryce.dbo.set_employeemaster where set_employeemaster.emp_id=  (select Set_UserMaster.Emp_Id from Pryce.dbo.Set_UserMaster where Set_UserMaster.User_Id=p.User_code))  as Name, latitute as Latitude ,longitute as Longitude , '' as Description FROM   GeoWork.dbo.gw_geocoords p INNER JOIN (SELECT user_code,MAX(t_date) AS MAXDATE FROM GeoWork.dbo.gw_geocoords GROUP BY user_code) tp ON p.user_code = tp.user_code AND p.t_date = tp.MAXDATE");
        if (ObjArr[0].ToString() != "")
        {
            dt = new DataView(dt, "User_code='" + ObjArr[0].ToString().Split('/')[1].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Mastersetup/LiveTrack.aspx','window','width=1024');", true);
    }
    #endregion


    #region Report


    protected void btnMenuReport_Click(object sender, EventArgs e)
    {
        pnlprojectrecord.Visible = false;
        pnllist.Visible = false;
        pnlTabtrackuser.Visible = false;
        PanelList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlnew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlTrack.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReport.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlReport.Visible = true;


        //AllPageCode();
    }

    protected void txtReportVehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtReportVehiclename.Text.Trim() != "")
        {

            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Name='" + txtReportVehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtReportVehiclename.Text = "";
                txtReportVehiclename.Focus();
                return;
            }
            else
            {
            }
        }

    }


    protected void btnTransportreport_OnClick(object sender, EventArgs e)
    {
        string strsql = string.Empty;


        strsql = "select Prj_VisitMaster.Ref_Type, Prj_VisitMaster.Status, case when Prj_VisitMaster.Ref_Type='Direct' then ' '    when Prj_VisitMaster.Ref_Type='Task' then  (select Prj_Project_Master.Project_Name from Prj_Project_Master where Prj_Project_Master.Project_Id=Prj_VisitMaster.Project_Id)     when Prj_VisitMaster.Ref_Type='Ticket' then (select SM_Ticket_Master.Ticket_No from SM_Ticket_Master where SM_Ticket_Master.Trans_Id=Prj_VisitMaster.Ref_Id)      end as Ref_Name ,   case when Prj_VisitMaster.Field6='False' then Prj_VisitMaster.Customer_Id else (select Ems_ContactMaster.Name from Ems_ContactMaster where Ems_ContactMaster.Trans_Id=Prj_VisitMaster.Customer_Id) end as customername,  Prj_VisitMaster.Contact_Address, Prj_VisitMaster.Contact_No, (select Sys_AreaMaster.Area_Name from Sys_AreaMaster where Sys_AreaMaster.Trans_Id=Prj_VisitMaster.Area) as Area_name, (select Prj_VehicleMaster.Name from Prj_VehicleMaster where Prj_VehicleMaster.Vehicle_Id=Prj_VisitMaster.Vehicle_Id ) as Vehicle_Name,Prj_VisitMaster.Visit_Time,Prj_VisitMaster.Visit_Date,(select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=Prj_VisitMaster.Driver_Id) as Driver_Name,(SELECT STUFF((SELECT Distinct ',' +RTRIM(Prj_Visit_Task.Task) FROM Prj_Visit_Task where Prj_Visit_Task.Visit_Id in (Prj_VisitMaster.Trans_Id) FOR XML PATH('')),1,1,'') ) as Task_Detail,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Set_EmployeeMaster.Emp_Name) FROM Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id in (select distinct Prj_Project_Task_Employeee.Employee_Id from Prj_Project_Task_Employeee where Prj_Project_Task_Employeee.Ref_Type='Visit' and Prj_Project_Task_Employeee.Ref_Id=Prj_VisitMaster.Trans_Id) FOR XML PATH('')),1,1,'') ) as Visit_Employee_List,Prj_VisitMaster.Description from Prj_VisitMaster where Prj_VisitMaster.IsActive='True'";


        if (txtFromdateReport.Text != "" && txttodatereport.Text != "")
        {


            strsql = strsql + " and Prj_VisitMaster.Visit_Date>='" + txtFromdateReport.Text + "' and Prj_VisitMaster.Visit_Date<='" + txttodatereport.Text + "'";
        }

        if (ddlreportStatus.SelectedIndex > 0)
        {

            strsql = strsql + " and Prj_VisitMaster.Status='" + ddlreportStatus.SelectedValue + "' ";

        }

        if (ddlreportRefType.SelectedIndex > 0)
        {

            strsql = strsql + " and Prj_VisitMaster.Ref_Type='" + ddlreportRefType.SelectedValue + "' ";

        }



        if (ddlreportRefType.SelectedIndex > 0)
        {

            strsql = strsql + " and Prj_VisitMaster.Ref_Type='" + ddlreportRefType.SelectedValue + "' ";

        }


        if (txtReportVehiclename.Text != "")
        {

            strsql = strsql + " and Prj_VisitMaster.Vehicle_Id='" + txtReportVehiclename.Text.Trim().Split('/')[1].ToString() + "' ";

        }


        if (txtReportDrivername.Text != "")
        {

            strsql = strsql + " and Prj_VisitMaster.Driver_Id='" + txtReportDrivername.Text.Trim().Split('/')[1].ToString() + "' ";

        }

        if (txtReportCustomername.Text != "")
        {
            string strcustomerName = string.Empty;

            if (txtReportCustomername.Text.Trim().Contains("/"))
            {
                strsql = strsql + " and Prj_VisitMaster.Customer_Id='" + txtReportCustomername.Text.Trim().Split('/')[1].ToString() + "' ";
            }
            else
            {
                strsql = strsql + " and Prj_VisitMaster.Customer_Id='" + txtReportCustomername.Text.Trim().ToString() + "' ";
            }
        }

        strsql = strsql + " order by Prj_VisitMaster.Visit_Date";


        DataTable dt = objDa.return_DataTable(strsql);


        Session["DtTransportReport"] = dt;



        string strCmd = string.Format("window.open('../ProjectManagement_Report/Transport_Report.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);


    }
    #endregion




}