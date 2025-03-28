using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Web;
using System.Net.Mail;
using System.Net;

public partial class MasterSetUp_Home : BasePage
{
    CompanyMaster ObjCompanyMaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    UserMaster objUserMaster = null;
    SystemParameter objSys = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    CurrencyMaster ObjCurrencyMaster = null;
    IT_ObjectEntry objObjectEntry = null;
    UserPermission objUserPermission = null;
    DataAccessClass objDa = null;
    Common cmn = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_Parameter_Location objAcParamLocation = null;
    Reminder objReminder = null;
    SystemParameter objSysParam = null;
    ChequeReport objChequeReport = null;
    Country_Currency objCC = null;
    UserDetail objUserDetail = null;

    //CustomReportStorageWebExtension objrpt = new CustomReportStorageWebExtension();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objUserDetail = new UserDetail(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
        ObjCompanyMaster = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(HttpContext.Current.Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        objUserMaster = new UserMaster(HttpContext.Current.Session["DBConnection"].ToString());
        objSys = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(HttpContext.Current.Session["DBConnection"].ToString());
        objRole = new RoleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(HttpContext.Current.Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(HttpContext.Current.Session["DBConnection"].ToString());
        objUserPermission = new UserPermission(HttpContext.Current.Session["DBConnection"].ToString());
        objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        cmn = new Common(HttpContext.Current.Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(HttpContext.Current.Session["DBConnection"].ToString());
        objAcParamLocation = new Ac_Parameter_Location(HttpContext.Current.Session["DBConnection"].ToString());
        objReminder = new Reminder(HttpContext.Current.Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        objChequeReport = new ChequeReport(HttpContext.Current.Session["DBConnection"].ToString());
        objCC = new Country_Currency(HttpContext.Current.Session["DBConnection"].ToString());



        if (!IsPostBack)
        {

            string strTitle = ConfigurationManager.AppSettings["AppType"].ToString();
            if (strTitle == "Timeman")
            {
                Page.Title = "Timeman Bio Attendance";
                hTitle.Visible = false;
                imgAppLogo.Src = "../images/timeman.png";
                imgAppBanner.Src = "../images/timeman.JPG";
                imgAppLogo.Width = 312;
            }
            else
            {
                Page.Title = "Pryce-Cloud-Login";
            }
        }

        Session["AccordianId"] = "107";
        //Session["AccordianId"] = "0";

        // Page.Title = objSys.GetSysTitle();
        // Code Location Replaced By Nitin on 25-09-2014 to Check Multiple Comp,Brand and Loc and For SIngle.........
        //For Module Name and ID Code
        //New Code Start On 24-09-2014 By Lokesh
        string strApplicationId = string.Empty;
        string strModuleId = string.Empty;

        DataTable dtParam = objSys.GetSysParameterByParamName("Application_Id");
        if (dtParam.Rows.Count > 0)
        {
            strApplicationId = dtParam.Rows[0]["Param_Value"].ToString();
        }

        //if (Convert.ToInt16(strApplicationId) >= 15)
        //    strApplicationId = "1";

        //if ((Convert.ToInt16(strApplicationId) >= 3) && (Convert.ToInt16(strApplicationId) <= 14))
        //    strApplicationId = "3";

        DataTable dtModuleId = objObjectEntry.GetModuleObjectByApplicationId(strApplicationId);

        if (dtModuleId.Rows.Count > 0)
        {
            Session["ModuleName"] = dtModuleId;
        }

        //New Code End
        //--------------------------------------------------------
        if (!IsPostBack)
        {
            //here we configuring financial year visibility according configured application
            //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim() == "Cloud")
            //{
            //    //for time man application , it should be false
            //    Div_Fyc.Visible = false;

            //}
            //else
            //{
            Div_Fyc.Visible = true;

            ////}


            //pnl1.Visible = true;
            //pnl2.Visible = true;
            if (fillCompanybyUser().Rows.Count > 0)
            {
                if (fillCompanybyUser().Rows[0]["Company_Name"].ToString().Length == 0)
                {
                    Response.Redirect("../Attendance/DefaultParameter.aspx");
                }
                else
                {
                    fillDropdown(ddlCompany, fillCompanybyUser(), "Company_Name", "Company_Id");
                }
            }
            else
            {
                ddlCompany.Items.Clear();
            }

            if (Session["CompName"] != null && Session["CompName"].ToString() != "")
            {
                try
                {
                    ddlCompany.SelectedValue = GetCompIdByName(Session["CompName"].ToString().Trim());
                    Session["CompanyId"] = ddlCompany.SelectedValue;
                }
                catch
                {

                }

                ddlCompany_SelectedIndexChanged(null, null);
                try
                {
                    ddlBrand.SelectedValue = GetBrandIdByName(ddlCompany.SelectedValue, Session["BrandName"].ToString());
                }
                catch
                {

                }

                ddlBrand_SelectedIndexChanged(null, null);
                try
                {
                    if (Session["emp_location"].ToString() != "0")
                    {
                        ddlLocation.SelectedValue = Session["emp_location"].ToString();
                    }
                    else
                    {
                        ddlLocation.SelectedValue = GetLocIdByName(ddlCompany.SelectedValue, Session["LocName"].ToString());
                    }
                }
                catch
                {

                }

                try
                {
                    ddlFinanceyear.SelectedValue = Session["FinanceYearId"].ToString();
                }
                catch
                {

                }
            }
            else
            {
                ddlCompany.SelectedIndex = 0;
                ddlCompany_SelectedIndexChanged(null, null);
            }


            try
            {
                ddlLanguage.SelectedValue = Session["lang"].ToString();
                ddlLanguage_SelectedIndexChanged(null, null);
            }
            catch
            {
                ddlLanguage.SelectedValue = "1";
                ddlLanguage_SelectedIndexChanged(null, null);
            }


            try
            {
                if (Session["emp_location"].ToString() != "0")
                {
                    ddlLocation.SelectedValue = Session["emp_location"].ToString();
                }
                else
                {
                    ddlLocation.SelectedValue = GetLocIdByName(ddlCompany.SelectedValue, Session["LocName"].ToString());
                }
            }
            catch
            {

            }
            BasePage bs = new BasePage();
            try
            {
                if (Session["Home"] != null)
                {


                    if (ddlCompany.Items.Count == 1 && ddlBrand.Items.Count == 1 && ddlLocation.Items.Count == 1)
                    {
                        //btnSave_Click(null, null);
                    }
                }
            }
            catch
            {

            }
            try
            {
                if (Request.QueryString["id"].ToString() == "123456789")
                {
                    btnSave_Click(null, null);
                }
            }
            catch
            {

            }

        }
        //else
        //{
        //    try
        //    {
        //        Label lblcomp = (Label)Master.FindControl("lblCompany1");
        //        Label lblBrand = (Label)Master.FindControl("lblBrand1");
        //        Label lblLocation = (Label)Master.FindControl("lblLocation1");
        //        Label lblLanguage1 = (Label)Master.FindControl("lblLanguage");
        //        Label ddlLanguage1 = (Label)Master.FindControl("ddlLanguage");

        //        lblcomp.Text = ddlCompany.SelectedItem.Text;

        //        lblBrand.Text = ddlBrand.SelectedItem.Text;
        //        lblLocation.Text = ddlLocation.SelectedItem.Text;

        //        Session["CompName"] = lblcomp.Text;
        //        Session["LocName"] = lblLocation.Text;
        //        Session["BrandName"] = lblBrand.Text;
        //        Session["Language"] = lblLanguage1.Text;
        //        Session["Currency"] = ObjCurrencyMaster.GetCurrencyMasterById(ObjCompanyMaster.GetCompanyMasterById(ddlCompany.SelectedValue.ToString()).Rows[0]["Currency_Id"].ToString()).Rows[0]["Currency_Name"].ToString();
        //        Session["CurrencyId"] = GetCurrencyIdByName(Session["Currency"].ToString());
        //    }
        //    catch
        //    {
        //        Label lblcomp = (Label)Master.FindControl("lblCompany1");

        //        ddlCompany.SelectedItem.Text = lblcomp.Text;
        //        ddlCompany_SelectedIndexChanged(null, null);

        //        Label lblBrand = (Label)Master.FindControl("lblBrand1");
        //        Label lblLocation = (Label)Master.FindControl("lblLocation1");

        //        try
        //        {
        //            ddlBrand.SelectedItem.Text = lblBrand.Text;
        //            ddlBrand_SelectedIndexChanged(null, null);
        //            ddlLocation.SelectedItem.Text = lblLocation.Text;
        //        }
        //        catch
        //        {

        //        }
        //    }
        //}



    }


    public string GetCompIdByName(string compname)
    {
        string compid = string.Empty;
        DataTable dt = ObjCompanyMaster.GetCompanyMasterByCompanyName(compname);

        if (dt.Rows.Count > 0)
        {
            compid = dt.Rows[0]["Company_Id"].ToString();
        }
        return compid;
    }
    public string GetBrandIdByName(string compid, string brandName)
    {
        string brandid = string.Empty;
        DataTable dt = ObjBrandMaster.GetBrandMasterByBrandName(compid, brandName);

        if (dt.Rows.Count > 0)
        {
            brandid = dt.Rows[0]["Brand_Id"].ToString();
        }
        return brandid;
    }
    public string GetCurrencyIdByName(string strCurrencyName)
    {
        string strCurrencyId = string.Empty;
        DataTable dtCurr = ObjCurrencyMaster.GetCurrencyMasterByCurrencyNames(strCurrencyName);
        if (dtCurr.Rows.Count > 0)
        {
            strCurrencyId = dtCurr.Rows[0]["Currency_ID"].ToString();
        }
        return strCurrencyId;
    }
    public string GetLocIdByName(string compid, string locname)
    {
        string locId = string.Empty;
        DataTable dt = ObjLocationMaster.GetLocationMasterByLocationName(compid, locname);

        if (dt.Rows.Count > 0)
        {
            locId = dt.Rows[0]["Location_Id"].ToString();
        }
        return locId;
    }
    protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Language"] = ddlLanguage.SelectedItem.Text;
        Session["lang"] = ddlLanguage.SelectedValue;
    }
    public DataTable fillCompanybyUser()
    {
        DataTable dtReturn = new DataTable();
        DataTable dt = objUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        dtReturn = ObjCompanyMaster.GetCompanyMaster();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Company_Id"].ToString() != "0" && dt.Rows[0]["Emp_Id"].ToString() != "0")
            {
                string CompanyId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "C", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                Session["Company_Permission"] = CompanyId;

                // Modified Date : 19-02-2014 , Modified By : Nitin,Kunal
                try
                {
                    dtReturn = new DataView(dtReturn, "Company_Id in(" + CompanyId.Substring(0, CompanyId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
            }
            else
            {
                //dtReturn = new DataView(dtReturn, "Company_Id in('" + dt.Rows[0]["Company_Id"].ToString() + "') or Parent_Company_Id='" + dt.Rows[0]["Company_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                dtReturn = ObjCompanyMaster.GetCompanyMaster();
            }
        }
        return dtReturn;
    }

    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CompId"] = ddlCompany.SelectedValue.ToString();
        try
        {
            Session["Currency"] = ObjCurrencyMaster.GetCurrencyMasterById(ObjCompanyMaster.GetCompanyMasterById(ddlCompany.SelectedValue.ToString()).Rows[0]["Currency_Id"].ToString()).Rows[0]["Currency_Name"].ToString();
        }
        catch
        {

        }

        DataTable dtBrand = ObjBrandMaster.GetBrandMaster(Session["CompId"].ToString());


        if (!Common.GetStatus(HttpContext.Current.Session["CompId"].ToString()))
        {
            string BrandIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "B", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (BrandIds != "")
            {
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BrandIds.Substring(0, BrandIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtBrand.Rows.Count > 0)
        {
            fillDropdown(ddlBrand, dtBrand, "Brand_Name", "Brand_Id");
            ddlBrand_SelectedIndexChanged(null, null);
        }
        else
        {
            ddlBrand.Items.Clear();
            ddlLocation.Items.Clear();
            return;
        }


        //here we will get the financial year for selected company

        DataTable dt = objFYI.GetInfoAllTrue(ddlCompany.SelectedValue);

        dt = new DataView(dt, "Status<>'New'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            fillDropdown(ddlFinanceyear, dt, "finance_code", "trans_id");
        }


        try
        {
            ddlFinanceyear.SelectedValue = new DataView(objFYI.GetInfoAllTrue(ddlCompany.SelectedValue), "Status='Open'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
        }
        catch
        {

        }

    }


    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {

        Session["BrandId"] = ddlBrand.SelectedValue.ToString();

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        Session["AllLocation"] = dtLoc;
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + ddlBrand.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {
        }


        if (!Common.GetStatus(HttpContext.Current.Session["CompId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            Session["MyAllLoc"] = LocIds;

            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }




        if (dtLoc.Rows.Count > 0)
        {
            fillDropdown(ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            ddlLocation.Items.Clear();
            ddlLocation.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        ddlLocation_SelectedIndexChanged(null, null);



    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {

        Session["LocId"] = ddlLocation.SelectedValue.ToString();

        if (ddlLocation.SelectedValue.ToString() != "0")
        {
            if (!Common.GetStatus(HttpContext.Current.Session["CompId"].ToString()))
            {
                string DeparmentIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocation.SelectedValue, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

                if (DeparmentIds != "")
                {
                    Session["SessionDepId"] = DeparmentIds;
                }
                else
                {
                    Session["SessionDepId"] = null;
                }
            }
        }
        else
        {
            Session["SessionDepId"] = "0";
        }
    }
    public void fillDropdown(DropDownList ddl, DataTable dt, string DataTextField, string DataValueField)
    {

        if (dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = DataTextField;
            ddl.DataValueField = DataValueField;
            ddl.DataBind();
        }
        else
        {
            ddl = null;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {


        try
        {
            if (ddlCompany.SelectedValue != "")
            {
                Session["CompId"] = ddlCompany.SelectedValue.ToString();
                if (Session["EmpId"].ToString().Trim() == "0")
                {
                    Session["LoginCompany"] = ddlCompany.SelectedValue.ToString();
                }
            }
            else
            {
                Session["CompId"] = "0";
            }
        }
        catch
        {
            Session["CompId"] = "0";
        }

        try
        {
            if (ddlLocation.SelectedValue != "" && ddlLocation.SelectedValue != "0")
            {
                Session["LocId"] = ddlLocation.SelectedValue.ToString();
                DataTable _dtTemp = new DataTable();
                _dtTemp = ObjLocationMaster.Get_Currency_By_Location_ID(Session["CompId"].ToString(), Session["LocId"].ToString());
                Session["LocCurrencyId"] = _dtTemp.Rows[0]["Currency_ID"].ToString();
                Session["LoginLocDecimalCount"] = _dtTemp.Rows[0]["decimal_count"].ToString();
                Session["LoginLocDecimalCount"] = Session["LoginLocDecimalCount"] == null ? 2 : Convert.ToInt32(Session["LoginLocDecimalCount"].ToString());
                Session["LoginLocCode"] = _dtTemp.Rows[0]["Location_Code"].ToString();
                Session["LocCurrencyCode"] = _dtTemp.Rows[0]["Currency_Code"].ToString();
                Session["TimeZoneId"] = objCC.getTimeZoneIdNameByCurrencyId(Session["LocCurrencyId"].ToString());
                _dtTemp.Dispose();

            }
            else
            {
                Session["TimeZoneId"] = "0";
                Session["LocId"] = "0";
                Session["LocCurrencyId"] = "0";
                Session["LocCurrencyCode"] = "0";
            }
        }
        catch
        {
            Session["LocId"] = "0";
        }
        try
        {
            if (ddlBrand.SelectedValue != "")
            {
                Session["BrandId"] = ddlBrand.SelectedValue.ToString();
            }
            else
            {
                Session["BrandId"] = "0";
            }
        }
        catch
        {
            Session["BrandId"] = "0";
        }

        //pnl1.Visible = false;
        //pnl2.Visible = false;

        //try
        //{

        //Label lblcomp = (Label)Master.FindControl("lblCompany1");
        //Label lblBrand = (Label)Master.FindControl("lblBrand1");
        //Label lblLocation = (Label)Master.FindControl("lblLocation1");
        //Label lblLanguage1 = (Label)Master.FindControl("lblLanguage");
        //Label lblFinancialyear = (Label)Master.FindControl("lblFinancialyear");

        //lblcomp.Text = ddlCompany.SelectedItem.Text;
        //try
        //{
        //    lblBrand.Text = ddlBrand.SelectedItem.Text;
        //}
        //catch
        //{
        //    lblBrand.Text = "";
        //}

        //try
        //{
        //    lblLocation.Text = ddlLocation.SelectedItem.Text;
        //}
        //catch
        //{
        //    lblLocation.Text = "";
        //}

        //try
        //{
        //    lblFinancialyear.Text = ddlFinanceyear.SelectedItem.Text;
        //}
        //catch
        //{

        //}

        //lblLanguage1.Text = ddlLanguage.SelectedItem.Text;
        //Session["CompName"] = lblcomp.Text;
        //Session["LocName"] = lblLocation.Text;
        //Session["BrandName"] = lblBrand.Text;
        //Session["Language"] = lblLanguage1.Text;

        string lblcomp, lblBrand, lblLocation, lblFinancialyear, lblLanguage1 = "";

        lblcomp = ddlCompany.SelectedItem.Text;
        try
        {
            lblBrand = ddlBrand.SelectedItem.Text;
        }
        catch
        {
            lblBrand = "";
        }

        try
        {
            lblLocation = ddlLocation.SelectedItem.Text;
        }
        catch
        {
            lblLocation = "";
        }

        try
        {
            lblFinancialyear = ddlFinanceyear.SelectedItem.Text;
        }
        catch
        {

        }

        lblLanguage1 = ddlLanguage.SelectedItem.Text;
        Session["CompName"] = lblcomp;
        Session["LocName"] = lblLocation;
        Session["BrandName"] = lblBrand;
        Session["Language"] = lblLanguage1;
        Session["Currency"] = ObjCurrencyMaster.GetCurrencyMasterById(ObjCompanyMaster.GetCompanyMasterById(ddlCompany.SelectedValue.ToString()).Rows[0]["Currency_Id"].ToString()).Rows[0]["Currency_Name"].ToString();
        Session["CurrencyId"] = GetCurrencyIdByName(Session["Currency"].ToString());


        //here we will ge the login finance id in session .
        Session["FinanceCode"] = ddlFinanceyear.SelectedItem.Text;
        Session["FinanceYearId"] = ddlFinanceyear.SelectedValue;
        Session["FinanceFromdate"] = objFYI.GetInfoByTransId(ddlCompany.SelectedValue, ddlFinanceyear.SelectedValue).Rows[0]["From_Date"].ToString();
        Session["FinanceTodate"] = objFYI.GetInfoByTransId(ddlCompany.SelectedValue, ddlFinanceyear.SelectedValue).Rows[0]["To_Date"].ToString();


        //For Update Database On 04-08-2015
        //objUpdateDatabase.Executescript();
        Session["EmpList"] = null;

        Session["Page1"] = "1";
        DataTable dtEmpDetails = new DataTable();

        if (Session["EmpId"].ToString() != "0")
        {
            dtEmpDetails = objUserMaster.GetUserMasterForUserName(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
        }
        else
        {
            dtEmpDetails = objUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        }
        Session["dtEmpDetails"] = dtEmpDetails;
        Common ObjCom = new Common(Session["DBConnection"].ToString());

        //Date :  23-10-2020
        if (Convert.ToInt16(Session["Application_Id"].ToString()) >= 15)
            Session["Application_Id"] = "1";
        DataTable dtAllModule1 = ObjCom.GetAccodion(Session["LoginCompany"].ToString(), Session["UserId"].ToString(), Session["Application_Id"].ToString());



        // End Code
        Common.clsApplicationModules _cls = ObjCom.getApplicationModules(Session["Application_Id"].ToString(), Session["CompId"].ToString());
        Session["clsApplicationModule"] = _cls;
        if (Session["UserId"].ToString().ToUpper() == "SUPERADMIN")
        {
            dtAllModule1 = new DataView(dtAllModule1, "ShowInNavigationMenu='true'", "", DataViewRowState.CurrentRows).ToTable();
        }
        Session["dtAllModule1"] = dtAllModule1;
        //check if finance module is there then only we have execute following methods

        try
        {
            if (_cls.isFinanceModule)
            {
                checkforSupplierPaymentReminder();
                checkforCustomerPaymentReminder();
                checkforCustomersCheque();
                checkforSupplierCheque();
            }
        }
        catch
        {

        }

        Session["MenuItem"] = null;
        string strCompanyLogoUrl = objDa.get_SingleValue("select Logo_Path from set_companymaster where company_id=" + ddlCompany.SelectedValue.ToString());
        strCompanyLogoUrl = "~/CompanyResource/" + ddlCompany.SelectedValue.ToString() + "/" + strCompanyLogoUrl;
        Session["CompanyLogoUrl"] = strCompanyLogoUrl;

        Session["GridSize"] = objSys.SetPageSize();



        bool Is_Emial = false;
        DataTable dtUser = new DataTable();
        dtUser = objDa.return_DataTable("Select Is_Email from Set_userMaster where User_Id='" + Session["UserId"].ToString() + "'");
        if (dtUser.Rows[0]["Is_Email"].ToString() != "" && dtUser.Rows[0]["Is_Email"].ToString() != null)
        {
            Is_Emial = Convert.ToBoolean(dtUser.Rows[0]["Is_Email"].ToString());
        }
        else
        {
            Is_Emial = false;
        }

        if (Is_Emial)
        {
            CheckEmailConfig();
        }


        if (Session["LocId"].ToString() == "0")
        {
            Response.Redirect("../MasterSetup/LocationMaster.aspx");
        }
        else
        {

            string strobjectid = Common.GetObjectIdbyPageURL("../Inventory/Inv_Dashboard.aspx", Session["DBConnection"].ToString());

            if (new DataView((DataTable)Session["dtAllModule1"], "Object_id=" + strobjectid + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                Response.Redirect("~/Inventory/Inv_Dashboard.aspx");
            }
            else
            {
                Response.Redirect("~/Dashboard/AttendanceDashboard.aspx");
            }




            //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["CloudApplicationCode"]!=null && Session["CloudApplicationCode"].ToString()== "SF-PRY1")
            //{
            //    Response.Redirect("~/Inventory/Inv_Dashboard.aspx");
            //}
            //else
            //{
            // Response.Redirect("~/Dashboard/AttendanceDashboard.aspx");
            //}

        }


        //Response.Redirect("~/MasterSetUp/ReligionMaster.aspx");

        //}
        //catch (Exception e1)
        //{
        //    Response.Redirect("~/Dashboard/AttendanceDashboard.aspx");
        //}



    }

    protected void checkforSupplierPaymentReminder()
    {
        //ref_table_name=ac_supplier_ageint_rerport we treated as supplier ageing

        string strSPVRemindTo = "0";
        try
        {
            strSPVRemindTo = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "SPV Remind To");
            if (strSPVRemindTo == Session["EmpId"].ToString())
            {
                DataTable dtReminder = objReminder.CurrentDateReminderData(Session["EmpId"].ToString());
                if (dtReminder.Rows.Count > 0)
                {
                    dtReminder = new DataView(dtReminder, "Ref_Table_Name='ac_supplier_ageing_report' and due_date='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //dtReminder = dtReminder.AsEnumerable().Where(dt => dt.Field<string>("Ref_Table_Name") == "ac_supplier_ageing_report" && dt.Field<DateTime>("due_date") == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString())).CopyToDataTable();
                }
                if (dtReminder.Rows.Count == 0)
                {
                    string strMessage = "";
                    string strPrams = Session["emp_location"].ToString() + "," + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
                    strPrams = Common.Encrypt(strPrams);
                    string strTargetUrl = "../SuppliersPayable/AgeingReportSupplier.aspx?SearchField=" + strPrams + "";

                    Ac_Ageing_Detail ageing = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
                    DataTable dtAgeing = ageing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PV", "0", "0", "0", true);
                    if (dtAgeing.Rows.Count > 0)
                    {
                        dtAgeing = new DataView(dtAgeing, "PaymentDate<='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("dd-MMM-yyyy") + "' and actual_balance_amt<>0 ", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    string strTotalPayment = "0";
                    if (dtAgeing.Rows.Count > 0)
                    {
                        strTotalPayment = dtAgeing.Compute("sum(actual_balance_amt)", "").ToString();

                        strMessage = "supplier payment Dues " + objSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), strTotalPayment) + "";
                    }

                    int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "ac_supplier_ageing_report", "0", strMessage, strTargetUrl, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                    new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "", Session["UserId"].ToString(), Session["UserId"].ToString());


                    //                dtAgeing.AsEnumerable()
                    //.GroupBy(r => new { Col1 = r["Currency_Name"], Col2 = r["Col2"] }) into grp
                    //.Select(g =>
                    //{
                    //    string[] row = {
                    //        g.Sum(r => r.Field<int>("PK")),
                    //        g.Key.Col1
                    //    };

                }
            }
            return;
        }
        catch (Exception ex) { }
    }

    protected void checkforCustomerPaymentReminder()
    {
        //ref_table_name=ac_customer_ageint_report we treated as customer ageing

        string strCRVRemindTo = "0";
        try
        {
            strCRVRemindTo = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "CRV Remind To");
            if (strCRVRemindTo == Session["EmpId"].ToString())
            {
                DataTable dtReminder = objReminder.CurrentDateReminderData(Session["EmpId"].ToString());
                if (dtReminder.Rows.Count > 0)
                {
                    dtReminder = new DataView(dtReminder, "Ref_Table_Name='ac_customer_ageing_report' and due_date='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //dtReminder = dtReminder.AsEnumerable().Where(dt => dt.Field<string>("Ref_Table_Name") == "ac_supplier_ageing_report" && dt.Field<DateTime>("due_date") == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString())).CopyToDataTable();
                }
                if (dtReminder.Rows.Count == 0)
                {
                    string strMessage = "";
                    string strPrams = Session["emp_location"].ToString() + "," + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy");
                    strPrams = Common.Encrypt(strPrams);
                    string strTargetUrl = "../CustomerReceivable/AgeingReportCustomer.aspx?SearchField=" + strPrams + "";

                    Ac_Ageing_Detail ageing = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
                    DataTable dtAgeing = ageing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "RV", "0", "0", "0", true);
                    if (dtAgeing.Rows.Count > 0)
                    {
                        dtAgeing = new DataView(dtAgeing, "PaymentDate<='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("dd-MMM-yyyy") + "' and actual_balance_amt<>0 ", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    string strTotalPayment = "0";
                    if (dtAgeing.Rows.Count > 0)
                    {
                        strTotalPayment = dtAgeing.Compute("sum(actual_balance_amt)", "").ToString();

                        strMessage = "Customer Dues " + objSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), strTotalPayment) + "";
                    }

                    int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "ac_customer_ageing_report", "0", strMessage, strTargetUrl, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                    new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "", Session["UserId"].ToString(), Session["UserId"].ToString());

                }
            }
            return;
        }
        catch (Exception ex) { }
    }


    protected void checkforCustomersCheque()
    {
        //ref_table_name=ac_customer_cheque_report we treated as customer ageing

        string strCRVRemindTo = "0";
        try
        {
            strCRVRemindTo = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "CRV Remind To");
            if (strCRVRemindTo == Session["EmpId"].ToString())
            {
                DataTable dtReminder = objReminder.CurrentDateReminderData(Session["EmpId"].ToString());
                if (dtReminder.Rows.Count > 0)
                {
                    dtReminder = new DataView(dtReminder, "Ref_Table_Name='ac_customer_cheque_report' and due_date='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //dtReminder = dtReminder.AsEnumerable().Where(dt => dt.Field<string>("Ref_Table_Name") == "ac_supplier_ageing_report" && dt.Field<DateTime>("due_date") == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString())).CopyToDataTable();
                }
                if (dtReminder.Rows.Count == 0)
                {
                    string acno = objAcParamLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString());
                    DataTable _dtTemp = objChequeReport.getGridData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), acno, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString());
                    var ChequeAmount = "0";
                    var ChequeCount = "0";
                    string strMessage = "";
                    string strTargetUrl = "#";
                    ChequeAmount = _dtTemp.Compute("sum(ChequeAmt)", "Field2='False' and Debit_Amount>0").ToString();
                    ChequeCount = _dtTemp.Compute("count(ChequeAmt)", "Field2='False' and Debit_Amount>0").ToString();
                    if (ChequeAmount != "0" && ChequeAmount != "")
                    {
                        ChequeAmount = objSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), ChequeAmount.ToString());
                        string strPrams = Session["emp_location"].ToString() + "," + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy") + ",NotReconcile,Inward";
                        strPrams = Common.Encrypt(strPrams);
                        strMessage = "Total " + ChequeCount + " Cheques Need to deposited";
                        strTargetUrl = "../bank/chequereport.aspx?SearchField=" + strPrams + "";
                    }
                    else
                    {
                        strMessage = "There is no cheque to deposit in bank";
                    }

                    int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "ac_customer_cheque_report", "0", strMessage, strTargetUrl, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                    new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "", Session["UserId"].ToString(), Session["UserId"].ToString());

                }
            }
            return;
        }
        catch (Exception ex) { }
    }

    protected void checkforSupplierCheque()
    {
        //ref_table_name=ac_supplier_cheque_report we treated as customer ageing

        string strRemindTo = "0";
        try
        {
            strRemindTo = objAcParamLocation.GetParameterValue_By_ParameterNameValue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "SPV Remind To");
            if (strRemindTo == Session["EmpId"].ToString())
            {
                DataTable dtReminder = objReminder.CurrentDateReminderData(Session["EmpId"].ToString());
                if (dtReminder.Rows.Count > 0)
                {
                    dtReminder = new DataView(dtReminder, "Ref_Table_Name='ac_supplier_cheque_report' and due_date='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //dtReminder = dtReminder.AsEnumerable().Where(dt => dt.Field<string>("Ref_Table_Name") == "ac_supplier_ageing_report" && dt.Field<DateTime>("due_date") == Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString())).CopyToDataTable();
                }
                if (dtReminder.Rows.Count == 0)
                {
                    string acno = objAcParamLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString());
                    DataTable _dtTemp = objChequeReport.getGridData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), acno, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString());
                    var ChequeAmount = "0";
                    var ChequeCount = "0";
                    string strMessage = "";
                    string strTargetUrl = "#";
                    ChequeAmount = _dtTemp.Compute("sum(ChequeAmt)", "Field2='False' and Credit_Amount>0").ToString();
                    ChequeCount = _dtTemp.Compute("count(ChequeAmt)", "Field2='False' and Credit_Amount>0").ToString();
                    if (ChequeAmount != "0" && ChequeAmount != "")
                    {
                        //ChequeAmount = objSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), ChequeAmount.ToString());
                        string strPrams = Session["emp_location"].ToString() + "," + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString("dd-MMM-yyyy") + ",NotReconcile,Outward";
                        strPrams = Common.Encrypt(strPrams);
                        strMessage = "Total " + ChequeCount + " Cheques will send for clearance";
                        strTargetUrl = "../bank/chequereport.aspx?SearchField=" + strPrams + "";
                    }
                    else
                    {
                        strMessage = "There is no cheques for clearance";
                    }

                    int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["emp_location"].ToString(), "ac_supplier_cheque_report", "0", strMessage, strTargetUrl, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "1", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                    new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Date.ToString(), "", Session["UserId"].ToString(), Session["UserId"].ToString());

                }
            }
            return;
        }
        catch (Exception ex) { }
    }


    //Email Function Added on 16-11-2023 
    public void CheckEmailConfig()
    {
        bool Is_Emial = false;
        DataTable dtUser = new DataTable();
        dtUser = objDa.return_DataTable("Select Is_Email from Set_userMaster where User_Id='" + Session["UserId"].ToString() + "'");
        if (dtUser.Rows[0]["Is_Email"].ToString() != "" && dtUser.Rows[0]["Is_Email"].ToString() != null)
        {
            Is_Emial = Convert.ToBoolean(dtUser.Rows[0]["Is_Email"].ToString());
        }
        if (Is_Emial == true)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            DataTable dtUserDetail = new DataTable();
            if (Session["CompId"] != null)
            {
                dtUserDetail = objUserDetail.GetbyUserId(Session["UserId"].ToString(), Session["CompId"].ToString());

            }
            if (dtUserDetail.Rows.Count > 0)
            {

                string test = SendEmail("" + dtUserDetail.Rows[0]["Email"].ToString() + "", "", "", "" + dtUserDetail.Rows[0]["Email"].ToString() + "", "" + dtUserDetail.Rows[0]["Password"].ToString() + "", "" + dtUserDetail.Rows[0]["Field1"].ToString() + "", "" + dtUserDetail.Rows[0]["Field2"].ToString() + "", "False", "done", "Login Successfully");
                if (test == "true")
                {
                    //Response.Redirect("~/MasterSetup/Home.aspx?id=123456789");
                }
                else
                {
                    if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "265", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                    {
                        // Response.Redirect("~/MasterSetup/Home.aspx?id=123456789");
                    }
                    else
                    {
                        Response.Redirect("~/EmailSystem/Mailbox.aspx");
                    }
                }
            }
            else
            {
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "265", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    //Response.Redirect("~/MasterSetup/Home.aspx?id=123456789");
                }
                else
                {
                    Response.Redirect("~/EmailSystem/Mailbox.aspx");
                }


            }
        }
    }

    public string SendEmail(string strTo, string strCC, string BCC, string strSenderEmail, string strSenderEmailPassword, string SenderHost, string Senderport, string strSenderSSL, string strBody, string strSubject)
    {
        bool IsEmail = false;

        System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();


        foreach (string str in strTo.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.To.Add(str);
        }
        foreach (string str in strCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.CC.Add(str);
        }
        foreach (string str in BCC.Split(';'))
        {
            if (str == "")
            {
                continue;
            }
            message.Bcc.Add(str);
        }

        message.From = new System.Net.Mail.MailAddress(strSenderEmail, "Pryce Email Testing");
        message.Subject = strSubject;
        message.IsBodyHtml = true;
        message.Body = strBody;
        SmtpClient smtp = new SmtpClient(SenderHost);
        NetworkCredential basiccr = new NetworkCredential(strSenderEmail, strSenderEmailPassword);
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = basiccr;
        smtp.Port = Convert.ToInt32(Senderport);
        smtp.EnableSsl = Convert.ToBoolean(strSenderSSL);
        try
        {
            smtp.Send(message);
            return "true";

        }
        catch (Exception ex)
        {
            return ex.Message.ToString();

        }

    }






}
