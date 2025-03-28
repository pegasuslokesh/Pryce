using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Data.SqlClient;

public partial class CloudLogin_Pryce : System.Web.UI.Page
{
    MasterDataAccess objMDa = null;
    static Random random = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
        if (!IsPostBack)
        {
            string str = Common.Decrypt("y4LB2hpWvpJ3qjKRl15ZJQ==");
            string strPassword = Common.Decrypt("nQcYTBMcvJmWKxc08Ix7Bw==");

            if (Request.QueryString["reg_code"] != null)
            {
                txtRegistratioCode.Text = Request.QueryString["reg_code"].ToString();
                txtEmail.Focus();
            }
            else
            {
                txtRegistratioCode.Text = "";
                txtRegistratioCode.Focus();
            }

            //Session.Abandon();
            lblReleaseDate.Text = "Release Date : " + ConfigurationManager.AppSettings["AppRelease"].ToString();
            lblVersion.Text = "Version No : " + ConfigurationManager.AppSettings["AppVersion"].ToString();
            string strTitle = ConfigurationManager.AppSettings["AppType"].ToString();
            if (strTitle == "Timeman")
            {
                Page.Title = "Timeman Bio Attendance";
                //divCaption.Visible = false;
                imgAppLogo.Src = "images/timeman.png";

            }
            else
            {
                Page.Title = "Pryce-Cloud-Login";
            }
        }
    }

    //protected void btnlogin_Click(object sender, EventArgs e)
    //{
    //    bool IsRegistered = true;

    //    DataTable dtTemp = new DataTable();

    //    if (txtRegistratioCode.Text.Trim() == "")
    //    {
    //        DisplayMessage("Enter Registration Code");
    //        txtRegistratioCode.Focus();
    //        return;
    //    }

    //    if (txtEmail.Text.Trim() == "")
    //    {
    //        DisplayMessage("Enter User Name");
    //        txtEmail.Focus();
    //        return;
    //    }

    //    if (txtPassword.Text.Trim() == "")
    //    {
    //        DisplayMessage("Enter Password");
    //        txtPassword.Focus();
    //        return;
    //    }


    //    //This Code Create by Rahul Sharma For Offline And Online both System Work According Licence System Date 17-10-2023
    //    PageControlCommon objPageCmn = null;
    //    DataAccessClass objDa = null;
    //    Common cmn = null;
    //    SystemParameter objSys = null;
    //    UserMaster ObjUserMaster = null;
    //    SystemParameter ObjSysParam = null;
    //    Set_ApplicationParameter ObjAppParam = null;
    //    EmployeeMaster ObjEmpMaster = null;
    //    CompanyMaster ObjCompanyMaster = null;
    //    Random random = new Random();
    //    SendMailSms ObjSendMailSms = null;
    //    string strCompanyId = string.Empty;
    //    string strPassword = Common.Encrypt(txtPassword.Text.Trim().ToString());


    //    DataTable dtConnection = new DataTable();
    //    MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(txtRegistratioCode.Text.Trim(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());



    //    if (clsMasterCmp == null)
    //    {
    //        try
    //        {
    //            clsMasterCmp = new MasterDataAccess.clsMasterCompany();

    //            clsMasterCmp.database = "pryce_client_" + txtRegistratioCode.Text.Trim() + "";


    //        }
    //        catch
    //        {
    //            clsMasterCmp.database = "pryce_client_" + txtRegistratioCode.Text.Trim() + "";
    //        }

    //        using (DataTable dtCmp = objMDa.return_DataTable("select * from sys.databases where name = '" + clsMasterCmp.database + "'"))
    //        {
    //            if (dtCmp.Rows.Count > 0)
    //            {
    //                //string strserverName = objMDa.get_SingleValue("select @@servername");
    //                string strserverName = "74.208.235.72";

    //                //strserverName = "104.238.86.46\\SQLEXPRESS";

    //                Session["DBConnection"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
    //                //Session["DBConnection"] = "Data Source=192.168.0.250;Initial Catalog=" + clsMasterCmp.database + ";User ID=sa;Password=Sa@1234;Max Pool Size=10000;";

    //                objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
    //                objDa = new DataAccessClass(Session["DBConnection"].ToString());
    //                cmn = new Common(Session["DBConnection"].ToString());
    //                objSys = new SystemParameter(Session["DBConnection"].ToString());
    //                ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
    //                ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
    //                ObjAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
    //                ObjEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
    //                ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
    //                random = new Random();
    //                ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
    //                DataTable dtMain = objDa.return_DataTable("Select*from Application_Lic_Main where registration_code='" + Common.Encrypt(txtRegistratioCode.Text) + "'");
    //                if (dtMain.Rows.Count > 0)
    //                {
    //                    clsMasterCmp.att_device_count = Common.Decrypt(dtMain.Rows[0]["att_device_count"].ToString());
    //                    clsMasterCmp.company_name = Common.Decrypt(dtMain.Rows[0]["company_name"].ToString());
    //                    clsMasterCmp.country_id = Common.Decrypt(dtMain.Rows[0]["country_id"].ToString());
    //                    clsMasterCmp.database = Common.Decrypt(dtMain.Rows[0]["database"].ToString());
    //                    clsMasterCmp.db_password = Common.Decrypt(dtMain.Rows[0]["db_password"].ToString());
    //                    clsMasterCmp.email = Common.Decrypt(dtMain.Rows[0]["email"].ToString());
    //                    clsMasterCmp.es_database = Common.Decrypt(dtMain.Rows[0]["es_database"].ToString());
    //                    clsMasterCmp.expiry_date = Common.Decrypt(dtMain.Rows[0]["expiry_date"].ToString());
    //                    clsMasterCmp.hostname = Common.Decrypt(dtMain.Rows[0]["hostname"].ToString());
    //                    clsMasterCmp.license_key = Common.Decrypt(dtMain.Rows[0]["license_key"].ToString());
    //                    clsMasterCmp.no_of_employee = Common.Decrypt(dtMain.Rows[0]["no_of_employee"].ToString());
    //                    clsMasterCmp.password = Common.Decrypt(dtMain.Rows[0]["password"].ToString());
    //                    clsMasterCmp.phone = Common.Decrypt(dtMain.Rows[0]["phone"].ToString());
    //                    clsMasterCmp.port = Common.Decrypt(dtMain.Rows[0]["port"].ToString());
    //                    clsMasterCmp.product_code = Common.Decrypt(dtMain.Rows[0]["product_code"].ToString());
    //                    clsMasterCmp.registration_code = Common.Decrypt(dtMain.Rows[0]["registration_code"].ToString());
    //                    clsMasterCmp.user = Common.Decrypt(dtMain.Rows[0]["user"].ToString());
    //                    clsMasterCmp.username = Common.Decrypt(dtMain.Rows[0]["username"].ToString());
    //                    clsMasterCmp.version_type = Common.Decrypt(dtMain.Rows[0]["version_type"].ToString());

    //                    if (!string.IsNullOrEmpty(clsMasterCmp.es_database))
    //                    {
    //                        Session["DBConnection_ES"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.es_database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
    //                    }
    //                }
    //                else
    //                {
    //                    DisplayMessage("Licence Detail Not Found");
    //                    txtRegistratioCode.Focus();
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                DisplayMessage("Invalid registration code");
    //                txtRegistratioCode.Focus();
    //                return;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        try
    //        {

    //            //strserverName = "104.238.86.46\\SQLEXPRESS";    


    //            using (DataTable dtCmp = objMDa.return_DataTable("select * from sys.databases where name = '" + clsMasterCmp.database + "'"))
    //            {
    //                if (dtCmp.Rows.Count > 0 && dtCmp.Rows[0]["State_desc"].ToString() != "ONLINE")
    //                {
    //                    DisplayMessage("Database Configuration is under processing");
    //                    return;
    //                }
    //                if (dtCmp.Rows.Count == 0)
    //                {
    //                    btndbConfigure_Click(clsMasterCmp);
    //                    // return;
    //                    IsRegistered = false;
    //                }
    //                else
    //                {
    //                    //btndbConfigure.Visible = false;
    //                }
    //            }
    //            //string strserverName = objMDa.get_SingleValue("select @@servername");

    //            string strserverName = "74.208.235.72";

    //            Session["DBConnection"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
    //            //Session["DBConnection"] = "Data Source=192.168.0.250;Initial Catalog=" + clsMasterCmp.database + ";User ID=sa;Password=Sa@1234;Max Pool Size=10000;";
    //            if (!string.IsNullOrEmpty(clsMasterCmp.es_database))
    //            {
    //                Session["DBConnection_ES"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.es_database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
    //            }
    //            objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
    //            objDa = new DataAccessClass(Session["DBConnection"].ToString());
    //            cmn = new Common(Session["DBConnection"].ToString());
    //            objSys = new SystemParameter(Session["DBConnection"].ToString());
    //            ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
    //            ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
    //            ObjAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
    //            ObjEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
    //            ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
    //            random = new Random();
    //            ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
    //            string Devices = clsMasterCmp.att_device_count;
    //            string company_name = clsMasterCmp.company_name;
    //            if (Devices == null || Devices == "")
    //            {
    //                Devices = "0";
    //            }
    //            if (company_name == "" || company_name == null)
    //            {
    //                company_name = "";
    //            }
    //            objDa.execute_Command("Delete from Application_Lic_Main");
    //            int LicMain = ObjAppParam.InsertApplicationLicenceDetailMain(Common.Encrypt(Devices), Common.Encrypt(company_name), Common.Encrypt(clsMasterCmp.country_id), Common.Encrypt(clsMasterCmp.database), Common.Encrypt(clsMasterCmp.db_password), Common.Encrypt(clsMasterCmp.email), Common.Encrypt(clsMasterCmp.es_database), Common.Encrypt(clsMasterCmp.expiry_date), Common.Encrypt(clsMasterCmp.hostname), Common.Encrypt(clsMasterCmp.license_key), Common.Encrypt(clsMasterCmp.no_of_employee), Common.Encrypt(clsMasterCmp.password), Common.Encrypt(clsMasterCmp.phone), Common.Encrypt(clsMasterCmp.port), Common.Encrypt(clsMasterCmp.product_code), Common.Encrypt(clsMasterCmp.registration_code), Common.Encrypt(clsMasterCmp.user), Common.Encrypt(clsMasterCmp.username), Common.Encrypt(clsMasterCmp.version_type), "", "", "", "", "", "true", DateTime.Now.ToString("yyyy-MM-dd"), "true", "admin", DateTime.Now.ToString("yyyy-MM-dd"), "admin", DateTime.Now.ToString("yyyy-MM-dd"));

    //            objDa.execute_Command("Delete from Application_Lic_Temp");

    //            int LicTemp = ObjAppParam.InsertApplicationLicenceDetailTemp(Common.Encrypt(Devices), Common.Encrypt(company_name), Common.Encrypt(clsMasterCmp.country_id), Common.Encrypt(clsMasterCmp.database), Common.Encrypt(clsMasterCmp.db_password), Common.Encrypt(clsMasterCmp.email), Common.Encrypt(clsMasterCmp.es_database), Common.Encrypt(clsMasterCmp.expiry_date), Common.Encrypt(clsMasterCmp.hostname), Common.Encrypt(clsMasterCmp.license_key), Common.Encrypt(clsMasterCmp.no_of_employee), Common.Encrypt(clsMasterCmp.password), Common.Encrypt(clsMasterCmp.phone), Common.Encrypt(clsMasterCmp.port), Common.Encrypt(clsMasterCmp.product_code), Common.Encrypt(clsMasterCmp.registration_code), Common.Encrypt(clsMasterCmp.user), Common.Encrypt(clsMasterCmp.username), Common.Encrypt(clsMasterCmp.version_type), "", "", "", "", "", "true", DateTime.Now.ToString("yyyy-MM-dd"), "true", "admin", DateTime.Now.ToString("yyyy-MM-dd"), "admin", DateTime.Now.ToString("yyyy-MM-dd"));



    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }

    //    ViewState["clsMasterCompany"] = clsMasterCmp;

    //    if (Convert.ToDateTime(clsMasterCmp.expiry_date).Date < DateTime.Now.Date)
    //    {
    //        DisplayMessage("Your License has been expired");
    //        UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
    //        //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
    //        Session.Abandon();
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OpenLicenseModel()", true);
    //        return;
    //    }




    //    if (!string.IsNullOrEmpty(clsMasterCmp.product_code) && GetApplicationIdbyName(clsMasterCmp.product_code) == "0")
    //    {

    //        DisplayMessage("Product code is invalid , please contact to Administrator");
    //        txtRegistratioCode.Focus();
    //        return;
    //    }

    //    Session["CloudDB"] = clsMasterCmp.database;
    //    Session["CloudDB_ES"] = clsMasterCmp.es_database;
    //    Session["CloudApplicationCode"] = clsMasterCmp.product_code;

    //    string strResult = Common.Decrypt("NKKQThSXM86X2NbcV6GFag==");

    //    DataTable dtUser = new DataTable();
    //    try
    //    {
    //        lblErrormessage.Text = "";
    //        dtUser = objDa.return_DataTable_Modify("SELECT Set_UserMaster.[User_Id], Set_UserMaster.[Company_Id], Set_UserMaster.[Emp_Id], Set_UserMaster.[Role_Id], Set_EmployeeMaster.Emp_Name AS EmpName, (SELECT Role_Name FROM Set_RoleMaster WHERE Role_Id = Set_UserMaster.Role_Id) AS Role_Name, Set_UserMaster.[Password], Set_UserMaster.[Is_Modified], Set_UserMaster.[Field1], Set_UserMaster.[Field2], Set_UserMaster.[Field3], Set_UserMaster.[Field4], Set_UserMaster.[Field5], Set_UserMaster.[Field6], Set_UserMaster.[Field7], Set_UserMaster.[IsActive], Set_UserMaster.[CreatedBy], Set_UserMaster.[CreatedDate], Set_UserMaster.[ModifiedBy], Set_UserMaster.[ModifiedDate] FROM Set_UserMaster LEFT JOIN Set_EmployeeMaster ON Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE (Set_UserMaster.User_Id = '" + txtEmail.Text + "' OR Set_EmployeeMaster.Email_Id = '" + txtEmail.Text + "') AND Set_UserMaster.Password = '" + strPassword + "' AND Set_UserMaster.IsActive = 'True' ");
    //        //dtUser = ObjUserMaster.GetUserMasterByUserIdPass(txtEmail.Text, strPassword, "1");
    //        IsRegistered = objDa.return_DataTable("select Param_Name from sys_parameter where Param_Name='Cloud_Default_Configuration'").Rows.Count == 0 ? false : true;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblErrormessage.Text = "Invalid Connection String";
    //        return;
    //    }

    //    if (dtUser != null && dtUser.Rows.Count != 0)
    //    {
    //        Session["CompId"] = "1";
    //        Session["BrandId"] = "1";
    //        Session["LocId"] = "1";
    //        Session["LoginCompany"] = "1";
    //        Session["MyModule"] = null;
    //        Session["ModuleDashBoardId"] = null;
    //        Session["UserId"] = dtUser.Rows[0]["User_Id"].ToString();
    //        Session["EmpId"] = dtUser.Rows[0]["Emp_Id"].ToString();
    //        Session["Device_Id"] = dtUser.Rows[0]["Field1"].ToString();
    //        Session["Home"] = "1";
    //        //here we are checking that password is expired or not 
    //        strCompanyId = "1";
    //        if (strCompanyId != "0")
    //        {
    //            //here we have to update code for handle confirmation
    //            if (objDa.return_DataTable("select * from Set_UserReminder where Reminder_flag='0' and Reminder_date<'" + DateTime.Now.ToString() + "' and User_Id='" + txtEmail.Text + "'").Rows.Count > 0)
    //            {
    //                Response.Redirect("~/MasterSetup/ChangePassword.aspx?ResetPassword=0");
    //            }
    //        }

    //        if (dtUser.Rows[0]["Field4"].ToString() != "")
    //        {
    //            Session["lang"] = dtUser.Rows[0]["Field4"].ToString();
    //        }
    //        else
    //        {
    //            Session["lang"] = null;
    //        }

    //        try
    //        {
    //            Session["DepartmentId"] = ObjEmpMaster.GetEmployeeMasterById(dtUser.Rows[0]["Company_Id"].ToString(), dtUser.Rows[0]["Emp_Id"].ToString()).Rows[0]["Department_Id"].ToString();
    //        }
    //        catch
    //        {
    //            Session["DepartmentId"] = "0";
    //        }
    //        Session["RegistrationCode"] = txtRegistratioCode.Text;

    //        //here we are updating application id in system parameter 
    //        if (!string.IsNullOrEmpty(clsMasterCmp.product_code))
    //        {
    //            objDa.execute_Command("update sys_parameter set Param_Value='" + GetApplicationIdbyName(clsMasterCmp.product_code) + "' where Param_Name='Application_Id'");
    //        }
    //        if (!string.IsNullOrEmpty(clsMasterCmp.company_name))
    //        {
    //            objDa.execute_Command("update sys_parameter set Param_Value='" + clsMasterCmp.company_name + "' where Param_Name='Owner_Name'");
    //        }
    //        if (!string.IsNullOrEmpty(clsMasterCmp.email))
    //        {
    //            objDa.execute_Command("update sys_parameter set Param_Value='" + clsMasterCmp.email + "' where Param_Name='Email'");
    //        }
    //        if (!string.IsNullOrEmpty(clsMasterCmp.phone))
    //        {
    //            objDa.execute_Command("update sys_parameter set Param_Value='" + clsMasterCmp.phone + "' where Param_Name='Phone_No'");
    //        }


    //        //if (string.IsNullOrEmpty(clsMasterCmp.product_code))
    //        //{
    //        System.Data.DataTable DtApp_Id = ObjSysParam.GetSysParameterByParamName("Application_Id");
    //        string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
    //        Session["Application_Id"] = Application_Id;
    //        //}
    //        //else
    //        //{
    //        //    Session["Application_Id"] = GetApplicationIdbyName(clsMasterCmp.product_code);
    //        //}
    //        Session["Page_Title"] = ObjSysParam.GetSysTitle();
    //        Session["DateFormat"] = ObjSysParam.SetDateFormat();

    //        Session["RoleId"] = dtUser.Rows[0]["Role_Id"].ToString();
    //        Session["HeaderText"] = "ERP";
    //        DataTable dtMessage1 = objPageCmn.GetArabicMessage();
    //        Session["MessageDt"] = dtMessage1;
    //        Session["emp_location"] = "0";
    //        ObjSysParam.SetPageSize();
    //        SystemLog.SaveSystemLog("User Login", DateTime.Now.ToString(), strCompanyId, Session["UserId"].ToString(), "", GetIpAddress()[0], GetIpAddress()[1], true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    //        try
    //        {

    //        }
    //        catch
    //        {

    //        }
    //        updateLicInfo(clsMasterCmp);

    //        if (IsRegistered && !string.IsNullOrEmpty(clsMasterCmp.es_database))
    //        {
    //            Response.Redirect("~/MasterSetup/Home.aspx");
    //        }
    //        else
    //        {
    //            Response.Redirect("~/attendance/DefaultParameter.aspx");
    //        }
    //    }
    //    else
    //    {
    //        DisplayMessage("Invaild UserName or Password");
    //        txtEmail.Text = "";
    //        txtPassword.Text = "";
    //        txtRegistratioCode.Text = "";
    //        txtRegistratioCode.Focus();
    //    }


    //}

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        bool IsRegistered = true;

        DataTable dtTemp = new DataTable();

        if (txtRegistratioCode.Text.Trim() == "")
        {
            DisplayMessage("Enter Registration Code");
            txtRegistratioCode.Focus();
            return;
        }

        if (txtEmail.Text.Trim() == "")
        {
            DisplayMessage("Enter User Name");
            txtEmail.Focus();
            return;
        }

        if (txtPassword.Text.Trim() == "")
        {
            DisplayMessage("Enter Password");
            txtPassword.Focus();
            return;
        }


        //This Code Create by Rahul Sharma For Offline And Online both System Work According Licence System Date 17-10-2023
        PageControlCommon objPageCmn = null;
        DataAccessClass objDa = null;
        Common cmn = null;
        SystemParameter objSys = null;
        UserMaster ObjUserMaster = null;
        SystemParameter ObjSysParam = null;
        Set_ApplicationParameter ObjAppParam = null;
        EmployeeMaster ObjEmpMaster = null;
        CompanyMaster ObjCompanyMaster = null;
        Random random = new Random();
        SendMailSms ObjSendMailSms = null;
        string strCompanyId = string.Empty;
        string strPassword = Common.Encrypt(txtPassword.Text.Trim().ToString());


        DataTable dtConnection = new DataTable();
        MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(txtRegistratioCode.Text.Trim(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());



        if (clsMasterCmp == null)
        {
            try
            {
                clsMasterCmp = new MasterDataAccess.clsMasterCompany();
                clsMasterCmp.database = "pryce_client_" + txtRegistratioCode.Text.Trim() + "";
            }
            catch
            {
                clsMasterCmp.database = "pryce_client_" + txtRegistratioCode.Text.Trim() + "";
            }

            using (DataTable dtCmp = objMDa.return_DataTable("select * from sys.databases where name = '" + clsMasterCmp.database + "'"))
            {
                if (dtCmp.Rows.Count > 0)
                {
                    //string strserverName = objMDa.get_SingleValue("select @@servername");
                    string strserverName = "74.208.235.72";

                    

                    Session["DBConnection"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
                    //Session["DBConnection"] = "Data Source=192.168.0.250;Initial Catalog=" + clsMasterCmp.database + ";User ID=sa;Password=Sa@1234;Max Pool Size=10000;";

                    objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
                    objDa = new DataAccessClass(Session["DBConnection"].ToString());
                    cmn = new Common(Session["DBConnection"].ToString());
                    objSys = new SystemParameter(Session["DBConnection"].ToString());
                    ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
                    ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
                    ObjAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
                    ObjEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
                    ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
                    random = new Random();
                    ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
                    DataTable dtMain = objDa.return_DataTable("Select*from Application_Lic_Main where registration_code='" + Common.Encrypt(txtRegistratioCode.Text) + "'");
                    if (dtMain.Rows.Count > 0)
                    {
                        clsMasterCmp.att_device_count = Common.Decrypt(dtMain.Rows[0]["att_device_count"].ToString());
                        clsMasterCmp.company_name = Common.Decrypt(dtMain.Rows[0]["company_name"].ToString());
                        clsMasterCmp.country_id = Common.Decrypt(dtMain.Rows[0]["country_id"].ToString());
                        try
                        {
                            clsMasterCmp.country_name = Common.Decrypt(dtCmp.Rows[0]["Field1"].ToString());
                        }
                        catch (Exception ex)
                        {
                            clsMasterCmp.country_name = "106";
                        }
                        clsMasterCmp.database = Common.Decrypt(dtMain.Rows[0]["database"].ToString());
                        clsMasterCmp.db_password = Common.Decrypt(dtMain.Rows[0]["db_password"].ToString());
                        clsMasterCmp.email = Common.Decrypt(dtMain.Rows[0]["email"].ToString());
                        clsMasterCmp.es_database = Common.Decrypt(dtMain.Rows[0]["es_database"].ToString());
                        clsMasterCmp.expiry_date = Common.Decrypt(dtMain.Rows[0]["expiry_date"].ToString());
                        clsMasterCmp.hostname = Common.Decrypt(dtMain.Rows[0]["hostname"].ToString());
                        clsMasterCmp.license_key = Common.Decrypt(dtMain.Rows[0]["license_key"].ToString());
                        clsMasterCmp.no_of_employee = Common.Decrypt(dtMain.Rows[0]["no_of_employee"].ToString());
                        clsMasterCmp.password = Common.Decrypt(dtMain.Rows[0]["password"].ToString());
                        clsMasterCmp.phone = Common.Decrypt(dtMain.Rows[0]["phone"].ToString());
                        clsMasterCmp.port = Common.Decrypt(dtMain.Rows[0]["port"].ToString());
                        clsMasterCmp.product_code = Common.Decrypt(dtMain.Rows[0]["product_code"].ToString());
                        clsMasterCmp.registration_code = Common.Decrypt(dtMain.Rows[0]["registration_code"].ToString());
                        clsMasterCmp.user = Common.Decrypt(dtMain.Rows[0]["user"].ToString());
                        clsMasterCmp.username = Common.Decrypt(dtMain.Rows[0]["username"].ToString());
                        clsMasterCmp.version_type = Common.Decrypt(dtMain.Rows[0]["version_type"].ToString());

                        if (!string.IsNullOrEmpty(clsMasterCmp.es_database))
                        {
                            Session["DBConnection_ES"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.es_database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
                        }
                    }
                    else
                    {
                        DisplayMessage("Licence Detail Not Found");
                        txtRegistratioCode.Focus();
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Invalid registration code");
                    txtRegistratioCode.Focus();
                    return;
                }
            }
        }
        else
        {
            try
            {

                //strserverName = "104.238.86.46\\SQLEXPRESS";    


                using (DataTable dtCmp = objMDa.return_DataTable("select * from sys.databases where name = '" + clsMasterCmp.database + "'"))
                {
                    if (dtCmp.Rows.Count > 0 && dtCmp.Rows[0]["State_desc"].ToString() != "ONLINE")
                    {
                        DisplayMessage("Database Configuration is under processing");
                        return;
                    }
                    if (dtCmp.Rows.Count == 0)
                    {
                        btndbConfigure_Click(clsMasterCmp);
                        // return;
                        IsRegistered = false;
                    }
                    else
                    {
                        //btndbConfigure.Visible = false;
                    }
                }
                //string strserverName = objMDa.get_SingleValue("select @@servername");

               string strserverName = "74.208.235.72";

                Session["DBConnection"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
                //Session["DBConnection"] = "Data Source=192.168.0.250;Initial Catalog=" + clsMasterCmp.database + ";User ID=sa;Password=Sa@1234;Max Pool Size=10000;";
                if (!string.IsNullOrEmpty(clsMasterCmp.es_database))
                {
                    Session["DBConnection_ES"] = "Data Source=" + strserverName + ";Persist Security Info=True;Initial Catalog=" + clsMasterCmp.es_database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
                }
                objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
                objDa = new DataAccessClass(Session["DBConnection"].ToString());
                cmn = new Common(Session["DBConnection"].ToString());
                objSys = new SystemParameter(Session["DBConnection"].ToString());
                ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
                ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
                ObjAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
                ObjEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
                ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
                random = new Random();
                ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
                string Devices = clsMasterCmp.att_device_count;
                string company_name = clsMasterCmp.company_name;
                if (Devices == null || Devices == "")
                {
                    Devices = "0";
                }
                if (company_name == "" || company_name == null)
                {
                    company_name = "";
                }
                objDa.execute_Command("Delete from Application_Lic_Main");
                int LicMain = ObjAppParam.InsertApplicationLicenceDetailMain(Common.Encrypt(Devices), Common.Encrypt(company_name), Common.Encrypt(clsMasterCmp.country_id), Common.Encrypt(clsMasterCmp.database), Common.Encrypt(clsMasterCmp.db_password), Common.Encrypt(clsMasterCmp.email), Common.Encrypt(clsMasterCmp.es_database), Common.Encrypt(clsMasterCmp.expiry_date), Common.Encrypt(clsMasterCmp.hostname), Common.Encrypt(clsMasterCmp.license_key), Common.Encrypt(clsMasterCmp.no_of_employee), Common.Encrypt(clsMasterCmp.password), Common.Encrypt(clsMasterCmp.phone), Common.Encrypt(clsMasterCmp.port), Common.Encrypt(clsMasterCmp.product_code), Common.Encrypt(clsMasterCmp.registration_code), Common.Encrypt(clsMasterCmp.user), Common.Encrypt(clsMasterCmp.username), Common.Encrypt(clsMasterCmp.version_type), Common.Encrypt(clsMasterCmp.country_name), "", "", "", "", "true", DateTime.Now.ToString("yyyy-MM-dd"), "true", "admin", DateTime.Now.ToString("yyyy-MM-dd"), "admin", DateTime.Now.ToString("yyyy-MM-dd"));

                objDa.execute_Command("Delete from Application_Lic_Temp");

                int LicTemp = ObjAppParam.InsertApplicationLicenceDetailTemp(Common.Encrypt(Devices), Common.Encrypt(company_name), Common.Encrypt(clsMasterCmp.country_id), Common.Encrypt(clsMasterCmp.database), Common.Encrypt(clsMasterCmp.db_password), Common.Encrypt(clsMasterCmp.email), Common.Encrypt(clsMasterCmp.es_database), Common.Encrypt(clsMasterCmp.expiry_date), Common.Encrypt(clsMasterCmp.hostname), Common.Encrypt(clsMasterCmp.license_key), Common.Encrypt(clsMasterCmp.no_of_employee), Common.Encrypt(clsMasterCmp.password), Common.Encrypt(clsMasterCmp.phone), Common.Encrypt(clsMasterCmp.port), Common.Encrypt(clsMasterCmp.product_code), Common.Encrypt(clsMasterCmp.registration_code), Common.Encrypt(clsMasterCmp.user), Common.Encrypt(clsMasterCmp.username), Common.Encrypt(clsMasterCmp.version_type), Common.Encrypt(clsMasterCmp.country_name), "", "", "", "", "true", DateTime.Now.ToString("yyyy-MM-dd"), "true", "admin", DateTime.Now.ToString("yyyy-MM-dd"), "admin", DateTime.Now.ToString("yyyy-MM-dd"));



            }
            catch (Exception ex)
            {

            }
        }

        ViewState["clsMasterCompany"] = clsMasterCmp;

        if (Convert.ToDateTime(clsMasterCmp.expiry_date).Date < DateTime.Now.Date)
        {
            DisplayMessage("Your License has been expired");
            UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
            //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
            Session.Abandon();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "OpenLicenseModel()", true);
            return;
        }




        if (!string.IsNullOrEmpty(clsMasterCmp.product_code) && GetApplicationIdbyName(clsMasterCmp.product_code) == "0")
        {

            DisplayMessage("Product code is invalid , please contact to Administrator");
            txtRegistratioCode.Focus();
            return;
        }

        Session["CloudDB"] = clsMasterCmp.database;
        Session["CloudDB_ES"] = clsMasterCmp.es_database;
        Session["CloudApplicationCode"] = clsMasterCmp.product_code;

        string strResult = Common.Decrypt("NKKQThSXM86X2NbcV6GFag==");

        DataTable dtUser = new DataTable();
        try
        {
            lblErrormessage.Text = "";
            dtUser = objDa.return_DataTable_Modify("SELECT Set_UserMaster.[User_Id], Set_UserMaster.[Company_Id], Set_UserMaster.[Emp_Id], Set_UserMaster.[Role_Id], Set_EmployeeMaster.Emp_Name AS EmpName, (SELECT Role_Name FROM Set_RoleMaster WHERE Role_Id = Set_UserMaster.Role_Id) AS Role_Name, Set_UserMaster.[Password], Set_UserMaster.[Is_Modified], Set_UserMaster.[Field1], Set_UserMaster.[Field2], Set_UserMaster.[Field3], Set_UserMaster.[Field4], Set_UserMaster.[Field5], Set_UserMaster.[Field6], Set_UserMaster.[Field7], Set_UserMaster.[IsActive], Set_UserMaster.[CreatedBy], Set_UserMaster.[CreatedDate], Set_UserMaster.[ModifiedBy], Set_UserMaster.[ModifiedDate] FROM Set_UserMaster LEFT JOIN Set_EmployeeMaster ON Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE (Set_UserMaster.User_Id = '" + txtEmail.Text + "' OR Set_EmployeeMaster.Email_Id = '" + txtEmail.Text + "') AND Set_UserMaster.Password = '" + strPassword + "' AND Set_UserMaster.IsActive = 'True' ");
            //dtUser = ObjUserMaster.GetUserMasterByUserIdPass(txtEmail.Text, strPassword, "1");
            IsRegistered = objDa.return_DataTable("select Param_Name from sys_parameter where Param_Name='Cloud_Default_Configuration'").Rows.Count == 0 ? false : true;
        }
        catch (Exception ex)
        {
            lblErrormessage.Text = "Invalid Connection String";
            return;
        }

        if (dtUser != null && dtUser.Rows.Count != 0)
        {
            Session["CompId"] = "1";
            Session["BrandId"] = "1";
            Session["LocId"] = "1";
            Session["LoginCompany"] = "1";
            Session["MyModule"] = null;
            Session["ModuleDashBoardId"] = null;
            Session["UserId"] = dtUser.Rows[0]["User_Id"].ToString();
            Session["EmpId"] = dtUser.Rows[0]["Emp_Id"].ToString();
            Session["Device_Id"] = dtUser.Rows[0]["Field1"].ToString();
            Session["Home"] = "1";
            //here we are checking that password is expired or not 
            strCompanyId = "1";
            if (strCompanyId != "0")
            {
                //here we have to update code for handle confirmation
                if (objDa.return_DataTable("select * from Set_UserReminder where Reminder_flag='0' and Reminder_date<'" + DateTime.Now.ToString() + "' and User_Id='" + txtEmail.Text + "'").Rows.Count > 0)
                {
                    Response.Redirect("~/MasterSetup/ChangePassword.aspx?ResetPassword=0");
                }
            }

            if (dtUser.Rows[0]["Field4"].ToString() != "")
            {
                Session["lang"] = dtUser.Rows[0]["Field4"].ToString();
            }
            else
            {
                Session["lang"] = null;
            }

            try
            {
                Session["DepartmentId"] = ObjEmpMaster.GetEmployeeMasterById(dtUser.Rows[0]["Company_Id"].ToString(), dtUser.Rows[0]["Emp_Id"].ToString()).Rows[0]["Department_Id"].ToString();
            }
            catch
            {
                Session["DepartmentId"] = "0";
            }
            Session["RegistrationCode"] = txtRegistratioCode.Text;

            //here we are updating application id in system parameter 
            if (!string.IsNullOrEmpty(clsMasterCmp.product_code))
            {
                objDa.execute_Command("update sys_parameter set Param_Value='" + GetApplicationIdbyName(clsMasterCmp.product_code) + "' where Param_Name='Application_Id'");
            }
            if (!string.IsNullOrEmpty(clsMasterCmp.company_name))
            {
                objDa.execute_Command("update sys_parameter set Param_Value='" + clsMasterCmp.company_name + "' where Param_Name='Owner_Name'");
            }
            if (!string.IsNullOrEmpty(clsMasterCmp.email))
            {
                objDa.execute_Command("update sys_parameter set Param_Value='" + clsMasterCmp.email + "' where Param_Name='Email'");
            }
            if (!string.IsNullOrEmpty(clsMasterCmp.phone))
            {
                objDa.execute_Command("update sys_parameter set Param_Value='" + clsMasterCmp.phone + "' where Param_Name='Phone_No'");
            }

            if (dtUser.Rows[0]["User_Id"].ToString() != "superadmin")
            {
                //if (string.IsNullOrEmpty(clsMasterCmp.product_code))
                //{
                System.Data.DataTable DtApp_Id = ObjSysParam.GetSysParameterByParamName("Application_Id");
                string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
                Session["Application_Id"] = Application_Id;
            }
            else
            {
                Session["Application_Id"] = "3";
            }

            //}
            //else
            //{
            //    Session["Application_Id"] = GetApplicationIdbyName(clsMasterCmp.product_code);
            //}
            Session["Page_Title"] = ObjSysParam.GetSysTitle();
            Session["DateFormat"] = ObjSysParam.SetDateFormat();

            Session["RoleId"] = dtUser.Rows[0]["Role_Id"].ToString();
            Session["HeaderText"] = "ERP";
            DataTable dtMessage1 = objPageCmn.GetArabicMessage();
            Session["MessageDt"] = dtMessage1;
            Session["emp_location"] = "0";
            ObjSysParam.SetPageSize();
            SystemLog.SaveSystemLog("User Login", DateTime.Now.ToString(), strCompanyId, Session["UserId"].ToString(), "", GetIpAddress()[0], GetIpAddress()[1], true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
            try
            {

            }
            catch
            {

            }
            updateLicInfo(clsMasterCmp);
            if (IsRegistered && !string.IsNullOrEmpty(clsMasterCmp.es_database))
            {
                Response.Redirect("~/MasterSetup/Home.aspx");
            }
            else
            {
                Response.Redirect("~/attendance/DefaultParameter.aspx");
            }
        }
        else
        {
            DisplayMessage("Invaild UserName or Password");
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtRegistratioCode.Text = "";
            txtRegistratioCode.Focus();
        }
    }

    public static string GetApplicationIdbyName(string strAppName)
    {
        // strAppName = "SF-PRY5";

        string strVal = "0";

        IT_ApplicationMaster objApp = new IT_ApplicationMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objApp.GetApplicationMaster();

        dt = new DataView(dt, "Application_Name='" + strAppName + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            strVal = dt.Rows[0]["Application_Id"].ToString();
        }
        return strVal;
    }


    public void UpdateConnectionString()
    {
        var settings = ConfigurationManager.ConnectionStrings["PegaConnection"];
        var fi = typeof(ConfigurationElement).GetField(
                      "_bReadOnly",
                      BindingFlags.Instance | BindingFlags.NonPublic);
        fi.SetValue(settings, false);
        settings.ConnectionString = Globals.ClientCon;
    }

    protected void updateLicInfo(MasterDataAccess.clsMasterCompany cls)
    {
        DataAccessClass objDa = new DataAccessClass(Session["DBConnection"].ToString());
        string key = "Reg_Code|" + txtRegistratioCode.Text + "#Expiry_Date|" + cls.expiry_date + "#Att_Device_Count|" + cls.att_device_count + "#Version_Type|" + cls.version_type + "#License_Key|" + cls.license_key + "#Reg_Email|" + cls.email + "#Reg_Phone|" + cls.phone;
        key = Common.Encrypt(key);
        objDa.execute_Command("update Sys_Parameter set param_value='" + key + "' where Param_Name='Lic_Key'");
    }

    public string[] GetIpAddress()
    {
        string[] IPAdd = new string[2];
        IPAdd[0] = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(IPAdd[0]))
            IPAdd[0] = Request.ServerVariables["REMOTE_ADDR"];
        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        IPAdd[1] = nics[0].GetPhysicalAddress().ToString();
        return IPAdd;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }

    protected void btndbConfigure_Click(MasterDataAccess.clsMasterCompany clsCompany)
    {
        //string strserverName = objMDa.get_SingleValue("select @@servername");

        string strserverName = "74.208.235.72";

        string strRegistrationCode = string.Empty;

        if (txtRegistratioCode.Text == "")
        {
            DisplayMessage("Enter Registration Code");
            txtRegistratioCode.Focus();
            return;
        }

        string strPassword = clsCompany.password;
        strRegistrationCode = txtRegistratioCode.Text.Trim().Replace("-", "");
        int i = 0;
        DataTable dt = objMDa.return_DataTable("SELECT file_id, name as [logical_file_name],physical_name from sys.database_files");
        string strlogincalName = ConfigurationManager.AppSettings["DBLogicalName"].ToString();
        string strlogincafilepath = dt.Rows[0][2].ToString();
        string strphysicalName = ConfigurationManager.AppSettings["DBPhysicalName"].ToString();
        string strphysicalfilepath = dt.Rows[1][2].ToString();
        string DbFilePath = ConfigurationManager.AppSettings["DBPath"].ToString();


        i = objMDa.execute_Command("create database " + clsCompany.database + " RESTORE FILELISTONLY FROM DISK='" + DbFilePath + "' RESTORE DATABASE " + clsCompany.database + " FROM DISK='" + DbFilePath + "' WITH REPLACE, RECOVERY, MOVE '" + strlogincalName + "' TO '" + strlogincafilepath.Replace("Pryce_Live_19102023", clsCompany.database) + "', MOVE '" + strphysicalName + "' TO '" + strphysicalfilepath.Replace("Pryce_Live_19102023", clsCompany.database) + "'");
        if (!string.IsNullOrEmpty(clsCompany.es_database) && ConfigurationManager.AppSettings["master_product_id"].ToString() == "639")
        {
            //string DbFilePath_ES = ConfigurationManager.AppSettings["DBPath_ES"].ToString();
            //strlogincalName = ConfigurationManager.AppSettings["DBLogicalName_ES"].ToString();
            //strphysicalName = ConfigurationManager.AppSettings["DBPhysicalName_ES"].ToString();
            //i = objMDa.execute_Command("create database " + clsCompany.es_database + " RESTORE FILELISTONLY FROM DISK='" + DbFilePath_ES + "' RESTORE DATABASE " + clsCompany.es_database + " FROM DISK='" + DbFilePath_ES + "' WITH REPLACE, RECOVERY, MOVE '" + strlogincalName + "' TO '" + strlogincafilepath.Replace("Pryce_es", clsCompany.es_database) + "', MOVE '" + strphysicalName + "' TO '" + strphysicalfilepath.Replace("Pryce_es", clsCompany.es_database) + "'");

            string DbFilePath_ES = ConfigurationManager.AppSettings["DBPath_ES"].ToString();
            strlogincalName = ConfigurationManager.AppSettings["DBLogicalName_ES"].ToString();
            strphysicalName = ConfigurationManager.AppSettings["DBPhysicalName_ES"].ToString();

            MasterDataAccess objesMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["pega_email_system"].ToString());
            DataTable dtes = objesMDa.return_DataTable("SELECT file_id, name as [logical_file_name],physical_name from sys.database_files");
            strlogincafilepath = dtes.Rows[0][2].ToString();
            strphysicalfilepath = dtes.Rows[1][2].ToString();
            i = objesMDa.execute_Command("create database " + clsCompany.es_database + " RESTORE FILELISTONLY FROM DISK='" + DbFilePath_ES + "' RESTORE DATABASE " + clsCompany.es_database + " FROM DISK='" + DbFilePath_ES + "' WITH REPLACE, RECOVERY, MOVE '" + strlogincalName + "' TO '" + strlogincafilepath.Replace("Pryce_es", clsCompany.es_database) + "', MOVE '" + strphysicalName + "' TO '" + strphysicalfilepath.Replace("Pryce_es", clsCompany.es_database) + "'");

        }



        if (i > 0)
        {

            //btndbConfigure.Visible = false;
            //if(ConfigurationManager.AppSettings["master_product_id"].ToString() =="639" )
            //{
            Session["DBConnection"] = "Data Source=" + strserverName + ";Initial Catalog=" + clsCompany.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";
            // Session["DBConnection"] = "Data Source=192.168.0.250;Initial Catalog=" + clsCompany.database + ";User ID=sa;Password=Sa@1234;Max Pool Size=10000;";
            //UpdateConnectionString();
            DataAccessClass Objda = new DataAccessClass(Session["DBConnection"].ToString());
            Objda.execute_Command("update set_usermaster set password='" + Common.Encrypt(strPassword) + "' where user_id='Admin'");
            Objda.execute_Command("update Set_EmployeeMaster set email_id='" + clsCompany.email + "'  where emp_code='9999'");


            Objda.execute_Command("Truncate table  set_rolepermission");
            Objda.execute_Command("Truncate table Set_RoleOpPermission");
            Objda.execute_Command("Truncate Table IT_App_Mod_Object");
            Objda.execute_Command("Truncate Table IT_ApplicationMaster");
            Objda.execute_Command("Truncate Table It_App_Op_permission");
            // Objda.execute_Command("Truncate Table IT_ObjectEntry");


            if (clsCompany.product_code == "TNA021" || clsCompany.product_code == "TNA025" || clsCompany.product_code == "TNA0220" || clsCompany.product_code == "TNA0250" || clsCompany.product_code == "TNA02100")
            {
                clsCompany.product_code = "TNA021";
            }
            if (clsCompany.product_code == "SFPRY1" || clsCompany.product_code == "SFPRY2" || clsCompany.product_code == "SFPRY3")
            {
                clsCompany.product_code = "SFPRY2";
            }
            if (clsCompany.product_code == "SFPRY4" || clsCompany.product_code == "SFPRY5")
            {
                clsCompany.product_code = "SFPRY4";
            }
            if (clsCompany.product_code == "SFPRY6" || clsCompany.product_code == "SFPRY7")
            {
                clsCompany.product_code = "SFPRY6";
            }
            if (clsCompany.product_code == "SFPRY8")
            {
                clsCompany.product_code = "SFPRY8";
            }
            if (clsCompany.product_code == "SFPRY9")
            {
                clsCompany.product_code = "SFPRY9";
            }

            string con = System.Configuration.ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
            //DataAccessClass objdb = new DataAccessClass("Data Source=192.168.29.250;Initial Catalog=Pryce_16112022;User ID=sa;Password=Sa@1234;Max Pool Size=10000;");
            DataAccessClass objdb = new DataAccessClass(con);

            DataTable dtAppMaster = objdb.return_DataTable("Select * from IT_ApplicationMaster");
            for (int Mcount = 0; Mcount < dtAppMaster.Rows.Count; Mcount++)
            {
                Objda.execute_Command("Insert Into IT_ApplicationMaster Values('" + dtAppMaster.Rows[Mcount]["Application_Name"].ToString() + "','" + dtAppMaster.Rows[Mcount]["Application_Name_L"].ToString() + "','" + dtAppMaster.Rows[Mcount]["IsActive"].ToString() + "','" + dtAppMaster.Rows[Mcount]["CreatedBy"].ToString() + "','" + dtAppMaster.Rows[Mcount]["CreatedDate"].ToString() + "','" + dtAppMaster.Rows[Mcount]["ModifiedBy"].ToString() + "','" + dtAppMaster.Rows[Mcount]["ModifiedDate"].ToString() + "')");
            }

            //DataTable dtItObject = objdb.return_DataTable("Select*from It_objectEntry");
            //for (int ObjCount = 0; ObjCount < dtItObject.Rows.Count; ObjCount++)
            //{
            //    Objda.execute_Command("Insert Into IT_ObjectEntry([Object_Name],[Object_Name_L],[Page_Url],[Order_Appear],[Form_Type],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate],[Approval_id],[notification_type_id],[ShowInNavigationMenu]) values('" + dtItObject.Rows[ObjCount]["Object_Name"].ToString() + "','" + dtItObject.Rows[ObjCount]["Object_Name_L"].ToString() + "','" + dtItObject.Rows[ObjCount]["Page_Url"].ToString() + "','" + dtItObject.Rows[ObjCount]["Order_Appear"].ToString() + "','" + dtItObject.Rows[ObjCount]["Form_Type"].ToString() + "','" + dtItObject.Rows[ObjCount]["IsActive"].ToString() + "','" + dtItObject.Rows[ObjCount]["CreatedBy"].ToString() + "','" + dtItObject.Rows[ObjCount]["CreatedDate"].ToString() + "','" + dtItObject.Rows[ObjCount]["ModifiedBy"].ToString() + "','" + dtItObject.Rows[ObjCount]["ModifiedDate"].ToString() + "','" + dtItObject.Rows[ObjCount]["Approval_id"].ToString() + "','" + dtItObject.Rows[ObjCount]["notification_type_id"].ToString() + "','" + dtItObject.Rows[ObjCount]["ShowInNavigationMenu"].ToString() + "')");
            //}

            DataTable dtAppObject = objdb.return_DataTable("Select * from IT_App_Mod_Object");
            for (int Obcount = 0; Obcount < dtAppObject.Rows.Count; Obcount++)
            {
                Objda.execute_Command("insert into IT_App_Mod_Object values('" + dtAppObject.Rows[Obcount]["Application_Id"].ToString() + "','" + dtAppObject.Rows[Obcount]["Module_Id"].ToString() + "','" + dtAppObject.Rows[Obcount]["Object_Id"].ToString() + "')");
            }

            DataTable dtOPPermission = objdb.return_DataTable("Select *from It_App_Op_Permission");
            for (int OpPer = 0; OpPer < dtOPPermission.Rows.Count; OpPer++)
            {
                Objda.execute_Command("Insert Into It_App_Op_permission ([Object_Id],[Op_Id]) values('" + dtOPPermission.Rows[OpPer]["Object_Id"].ToString() + "','" + dtOPPermission.Rows[OpPer]["Op_Id"].ToString() + "')");
            }

            DataTable dtRolePermission = objdb.return_DataTable("Select * From IT_App_Mod_Object where Application_Id =(Select Application_id From IT_ApplicationMaster Where Application_Name = '" + clsCompany.product_code + "')");
            for (int count = 0; count < dtRolePermission.Rows.Count; count++)
            {
                Objda.execute_Command("INSERT INTO set_rolepermission VALUES ('1','" + dtRolePermission.Rows[count]["Module_Id"].ToString() + "','" + dtRolePermission.Rows[count]["Object_Id"].ToString() + "','1','superadmin',getdate(),'superadmin',getdate())");
                DataTable dtIdentity = Objda.return_DataTable("Select Max(TransId) From set_rolepermission");
                DataTable dtRoleOpPermission = Objda.return_DataTable("Select * From It_App_Op_Permission where Object_Id = '" + dtRolePermission.Rows[count]["Object_Id"].ToString() + "'");
                for (int jCount = 0; jCount < dtRoleOpPermission.Rows.Count; jCount++)
                {
                    Objda.execute_Command("INSERT INTO Set_RoleOpPermission VALUES ('" + dtIdentity.Rows[0][0].ToString() + "','" + dtRoleOpPermission.Rows[jCount]["Op_Id"].ToString() + "')");
                }
            }

            //This line Add by Rahul Sharma Date 18-10-2023
            Objda.execute_Command("Update Set_CompanyMaster set Company_Name='" + clsCompany.company_name + "',Company_Name_L='" + clsCompany.company_name + "'");


            //This Line Add by Rahul Sharma date 09-01-2024
            Objda.execute_Command("truncate table Ac_FinancialYear_Detail");
            Objda.execute_Command("truncate table Ac_Finance_Year_Info");

            Objda.execute_Command("INSERT INTO [dbo].[Ac_Finance_Year_Info] ([Company_Id],[Finance_No],[Finance_Code],[From_Date],[To_Date],[Status],[Inv_Status],[Inv_Closed_By],[Inv_Closed_Date],[Remark],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])VALUES('1', '1', 'FYC-" + DateTime.Now.Year.ToString() + "', DATEFROMPARTS(YEAR(GETDATE()), 1, 1), DATEADD(DAY, -1, DATEADD(YEAR, 1, DATEFROMPARTS(YEAR(GETDATE()), 1, 1))), 'Open', '', '', '1800-01-01 00:00:00.000', 'For Finance Year " + DateTime.Now.Year.ToString() + "', '', '', '', '', '', '1', GETDATE(), '1', 'Admin', GETDATE(), 'Admin', GETDATE())");
            Objda.execute_Command("INSERT INTO [dbo].[Ac_FinancialYear_Detail]([Company_Id],[Brand_Id],[Location_Id],[Header_Trans_Id],[Status],[Closing_Status],[Inv_Status],[Inv_Closed_By],[Inv_Closed_Date],[Remark],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[ModifiedBy],[ModifiedDate]) VALUES ('1', '1', '1', '1', 'Open', '', 'Open', '', '', '', '', '', '', '', '', '0', GETDATE(), 'Admin', GETDATE())");
        }

        //DisplayMessage("Database Created Successfully , please try to login");
        //btndbConfigure.Visible = false;
    }

    protected void btnRenew_Click(object sender, EventArgs e)
    {

    }

    protected void btnMyAccount_Click(object sender, EventArgs e)
    {

    }

    protected void btnmyaccount_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CloseLicenseModel()", true);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + ConfigurationManager.AppSettings["LicenseLoginUrl"].ToString() + "')", true);
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('"+ ConfigurationManager.AppSettings["LicenseLoginUrl"].ToString() + "','','height=650,width=950,scrollbars=Yes')", true);
        //Response.Redirect(ConfigurationManager.AppSettings["RegistrationUrl"].ToString());
    }

    protected void lnkForgetpassword_Click(object sender, EventArgs e)
    {
        string strCompanyId = string.Empty;
        bool Result = false;

        if (txtRegistratioCode.Text == "")
        {
            DisplayMessage("Enter Registration Code");
            txtRegistratioCode.Text = "";
            txtRegistratioCode.Focus();
            return;
        }

        MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(txtRegistratioCode.Text, ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());

        if (clsMasterCmp.database == null)
        {
            DisplayMessage("Invalid registration code");
            txtRegistratioCode.Focus();
            return;
        }

        //string strserverName = objMDa.get_SingleValue("select @@servername");

       string strserverName = "74.208.235.72";

        Session["DBConnection"] = "Data Source=" + strserverName + ";Initial Catalog=" + clsMasterCmp.database + ";User ID=" + ConfigurationManager.AppSettings["DBUserName"].ToString() + ";Password=" + ConfigurationManager.AppSettings["DBPassword"].ToString() + ";Max Pool Size=10000;";


        DataAccessClass ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        UserMaster ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
        CompanyMaster ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
        SendMailSms ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());


        if (txtEmail.Text.Trim() == "")
        {
            DisplayMessage("Enter User Name");
            txtEmail.Focus();
            return;
        }

        if (ObjDa.return_DataTable("select name from sys.databases where name='" + clsMasterCmp.database + "'").Rows.Count == 0)
        {
            DisplayMessage("Database configuration is pending so please intialize database");
            return;
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            strCompanyId = ObjDa.return_DataTable("select top 1  company_id from Set_CompanyMaster where IsActive='true'", ref trns).Rows[0][0].ToString();


            DataTable dt = ObjUserMaster.GetUserMasterByUserId_ForgotPassword(txtEmail.Text.Trim(), strCompanyId, ref trns);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Email_Id"].ToString().Trim() == "")
                {
                    DisplayMessage("User email-id is not registered in employee setup, please contact to admin");
                    return;
                }
                else
                {
                    string strVerficationCode = (Convert.ToString(random.Next(10, 20000)));
                    ObjDa.execute_Command("update Set_UserMaster set Field5='" + strVerficationCode + "' where Emp_Id=" + dt.Rows[0]["Emp_Id"].ToString().Trim() + "", ref trns);
                    string strAppMailId = string.Empty;
                    string strAppPassword = string.Empty;
                    string strSmtp = string.Empty;
                    string str_smtp_port = string.Empty;


                    strAppMailId = ObjDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email' and Company_Id='" + strCompanyId + "'", ref trns).Rows[0][0].ToString();
                    strAppPassword = Common.Decrypt(ObjDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email_Password' and Company_Id='" + strCompanyId + "' ", ref trns).Rows[0][0].ToString());
                    strSmtp = ObjDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email_SMTP' and Company_Id='" + strCompanyId + "' ", ref trns).Rows[0][0].ToString();
                    str_smtp_port = ObjDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email_Port' and Company_Id='" + strCompanyId + "' ", ref trns).Rows[0][0].ToString();

                    string MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p><br />We received a request to access your pryce account.<br /><br />your pryce verification code is <b> " + strVerficationCode + "</b>.<br /><br />To change your password  <a href=" + GetForgotPasswordURL() + "?RegCode=@R&&User_Id=@U&&Employee_Id=@E>ClickHere</a>.<br /><h3>Thanks & Regards</h3><h2>Pryce Support Team</h2></div></body></html>";
                    MailMessage = MailMessage.Replace("@U", HttpUtility.UrlEncode(Common.Encrypt(dt.Rows[0]["User_Id"].ToString().Trim())));
                    MailMessage = MailMessage.Replace("@E", HttpUtility.UrlEncode(Common.Encrypt(dt.Rows[0]["Emp_Id"].ToString().Trim())));
                    MailMessage = MailMessage.Replace("@R", HttpUtility.UrlEncode(Common.Encrypt(txtRegistratioCode.Text.Trim())));
                    try
                    {
                        Result = ObjSendMailSms.SendMail_TicketInfo(dt.Rows[0]["Email_Id"].ToString().Trim(), "", "", "TimeMan Forgot password", MailMessage, strCompanyId, "", strAppMailId.ToString(), strAppPassword.ToString(), "TimeMan Forgot password", strSmtp, str_smtp_port, "", dt.Rows[0]["Brand_Id"].ToString().Trim(), dt.Rows[0]["Location_Id"].ToString().Trim());
                    }
                    catch (Exception ex)
                    {
                        DisplayMessage("Please confirm email setting or credential in company parameter page");
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
            }
            else
            {
                DisplayMessage("UserName or Email-ID is invalid");
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }

            if (Result)
            {
                DisplayMessage("Password reset link shared on your email , please check and proceed");
                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
            }
            else
            {
                DisplayMessage("There may be some Internet problem OR please check email configuration under Company Parameter");
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
    public static string GetForgotPasswordURL()
    {
        string host = string.Empty;
        string strPort = string.Empty;
        string strLocalPath = string.Empty;
        string strFinalURL = string.Empty;
        host = HttpContext.Current.Request.Url.Host;
        strPort = HttpContext.Current.Request.Url.Port.ToString();
        strLocalPath = HttpContext.Current.Request.Url.LocalPath;
        strFinalURL = "http://";
        if (strPort != "")
        {
            strFinalURL += host + ":" + strPort + strLocalPath;
        }
        else
        {
            strFinalURL += host + strLocalPath;
        }
        strFinalURL = strFinalURL.Replace("CloudLogin.aspx", "MasterSetUp/ForgetPassword.aspx");
        strFinalURL = strFinalURL.Replace(" ", "%20");
        return strFinalURL;
    }

}