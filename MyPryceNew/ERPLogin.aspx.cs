using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Reflection;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Activities.Statements;
using iTextSharp.text.pdf;
using System.Text;
using System.Configuration;

public partial class ERPLogin : BasePage
{
    //DutyMaster DutyMaster = new DutyMaster();
    Common cmn = new Common(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    DataAccessClass objDa = new DataAccessClass(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    SystemParameter objSys = new SystemParameter(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    UserMaster ObjUserMaster = new UserMaster(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    SystemParameter ObjSysParam = new SystemParameter(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    Set_ApplicationParameter ObjAppParam = new Set_ApplicationParameter(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    EmployeeMaster ObjEmpMaster = new EmployeeMaster(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    CompanyMaster ObjCompanyMaster = new CompanyMaster(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    static Random random = new Random();
    SendMailSms ObjSendMailSms = new SendMailSms(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    IPAddress objipAddress = new IPAddress(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
    PageControlCommon objPageCmn = new PageControlCommon(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {

        string str12 = Common.Decrypt("j6iSzh4BjpZXqeDXkw28OA==");
        str12 = Common.Decrypt("NKKQThSXM86X2NbcV6GFag==");
        if (!IsPostBack)
        {
            Session.Abandon();
            lblReleaseDate.Text = "Release Date : " + ConfigurationManager.AppSettings["AppRelease"].ToString();
            lblVersion.Text = "Version No : " + ConfigurationManager.AppSettings["AppVersion"].ToString();
            //here we are checking if application type is cloud then we will redirect to cloud login page 
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            {
                if (Request.QueryString["reg_code"] != null)
                {
                    Response.Redirect("~/CloudLogin.aspx?reg_code=" + Request.QueryString["reg_code"].ToString() + "");
                }
                else
                {
                    Response.Redirect("~/CloudLogin.aspx");
                }

            }
            else
            {
                Session["DBConnection"] = System.Configuration.ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
                Session["DBConnection_ES"] = System.Configuration.ConfigurationManager.ConnectionStrings["Pega_Email_System"].ConnectionString;
            }
        }

        //try
        //{
        //    cmn = new Common(Session["DBConnection"].ToString());
        //    objDa = new DataAccessClass(Session["DBConnection"].ToString());
        //    objSys = new SystemParameter(Session["DBConnection"].ToString());
        //    ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
        //    ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        //    ObjAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        //    ObjEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        //    ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
        //    ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        //    objipAddress = new IPAddress(Session["DBConnection"].ToString());
        //}
        //catch(Exception ex)
        //{

        //}

        try
        {

            if (!IsPostBack)
            {
                DataTable Dt_Company = ObjCompanyMaster.Get_Company_Master_Active();
                if (Dt_Company != null && Dt_Company.Rows.Count > 0)
                {
                    try
                    {
                        if (Dt_Company.Rows[0]["Logo_Path"].ToString() != String.Empty)
                        {
                            imgLogo.Src = "~/CompanyResource/" + "/" + Dt_Company.Rows[0]["Company_Id"].ToString() + "/" + Dt_Company.Rows[0]["Logo_Path"].ToString();
                        }
                    }
                    catch
                    {

                    }
                    objPageCmn.FillData((object)ddlCompany, Dt_Company, "Company_Name", "Company_Id");
                    lblErrormessage.Text = "";
                }
                else
                {
                    if (ddlCompany.Items.Count == 0)
                        ddlCompany.Items.Insert(0, "--Select--");
                }
            }

            double val = 14 % 7;

            string str1 = Common.Encrypt("");
            DataTable dt = new DataTable("User");
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Name", typeof(string)));

            string commaSeparatedString = String.Join(",", dt.AsEnumerable().Select(x => x.Field<string>("Name").ToString()).ToArray());


            string str = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("HH:mm");
            Settings.IsValid = true;
            Settings.IsDemo = false;
            txtUserName.Focus();
        }
        catch
        {
            if (ddlCompany.Items.Count == 0)
                ddlCompany.Items.Insert(0, "--Select--");
        }

        if (ddlCompany.Items.Count == 1 || ddlCompany.Items.Count == 2)
        {
            Div_Company.Visible = false;
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        Session["DBConnection"] = System.Configuration.ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString;
        Session["DBConnection_ES"] = System.Configuration.ConfigurationManager.ConnectionStrings["Pega_Email_System"].ConnectionString;
        Session["CloudDB_ES"] = GetDatabaseName(System.Configuration.ConfigurationManager.ConnectionStrings["Pega_Email_System"].ConnectionString);
        string strCompanyId = string.Empty;
        if (txtUserName.Text == "")
        {
            DisplayMessage("Please Enter UserName Or Email-ID");
            txtUserName.Focus();
            return;
        }
        if (txtPassWord.Text == "")
        {
            DisplayMessage("Please Enter Password");
            txtPassWord.Focus();
            return;
        }


        if (ddlCompany.Items.Count == 1 || ddlCompany.Items.Count == 2)
        {
            if (txtUserName.Text.ToLower() == "superadmin")
            {
                ddlCompany.SelectedIndex = 0;
            }
            else if (ddlCompany.Items.Count > 1)
            {
                ddlCompany.SelectedIndex = 1;
            }
            else
            {
                ddlCompany.SelectedIndex = 0;
            }
        }

        if (ddlCompany.SelectedValue == "--Select--")
        {
            strCompanyId = "0";
            if (txtUserName.Text != "superadmin")
            {
                DisplayMessage("Please Select Company");
                ddlCompany.Focus();
                return;
            }
            else
            {
                Session["LoginCompany"] = "0";
            }
        }
        else
        {
            strCompanyId = ddlCompany.SelectedValue.ToString();
            Session["LoginCompany"] = ddlCompany.SelectedValue.ToString();
            Session["CompId"] = ddlCompany.SelectedValue.ToString();
        }

        string strPassword = Common.Encrypt(txtPassWord.Text.Trim().ToString());

        string sstrnew = "HyscJe6PaY15kC/+t/T9Pg==";
        string strPass = Common.Decrypt(sstrnew);

        string strnew = Common.Encrypt("superadmin");

        string strcloudSuperadmin = Common.Decrypt("qD6c024YN4VZdvgK0AMMiGN2+aXWrW7w1t/oOD3QbBs=");
        //string str3000 = Common.Decrypt("TTiCDkSRljTd8yIx0prZKg==");

        string strEmailPass = Common.Decrypt("qd3RAK7GIGo7oRIVr/UK9dL+jQMQCvMQF9TiXYiTKpg=");

        string strAlphaSuperadmin = Common.Decrypt("dkvI7cyk1GQMGU9a6/UEnaKMord/d+6QRI4zmzYO+VY=");

        string str3000Local = Common.Decrypt("HyscJe6PaY15kC/+t/T9Pg==");
        //Pega@1234 Touristic


        DataTable dtUser = new DataTable();
        try
        {
            lblErrormessage.Text = "";
            if (ddlCompany.SelectedValue == "--Select--")
            {
                dtUser = ObjUserMaster.GetUserMasterByUserIdPass(txtUserName.Text, strPassword, strCompanyId);
            }
            else
            {
                dtUser = ObjUserMaster.GetUserMasterByUserIdPassForWithoutEmployeeId(txtUserName.Text, strPassword, strCompanyId);
            }
        }
        catch (Exception ex)
        {
            lblErrormessage.Text = "Invalid Connection String";
            return;
        }

        if (dtUser != null && dtUser.Rows.Count != 0)
        {

            Session["MyModule"] = null;
            Session["ModuleDashBoardId"] = null;
            Session["UserId"] = dtUser.Rows[0]["User_Id"].ToString();
            Session["EmpId"] = dtUser.Rows[0]["Emp_Id"].ToString();
            Session["Device_Id"] = dtUser.Rows[0]["Field1"].ToString();
            Session["Home"] = "1";
            //here we are checking that password is expired or not 
            if (strCompanyId != "0")
            {
                //here we have to update code for handle confirmation
                //if (objDa.return_DataTable("select * from Set_UserReminder where Reminder_flag='0' and Reminder_date<'" + DateTime.Now.ToString() + "' and User_Id='" + txtUserName.Text + "'").Rows.Count > 0)
                //{
                //    Response.Redirect("~/MasterSetup/ChangePassword.aspx?ResetPassword=0");
                //}
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


            System.Data.DataTable DtApp_Id = ObjSysParam.GetSysParameterByParamName("Application_Id");
            string Application_Id = DtApp_Id.Rows[0]["Param_Value"].ToString();
            Session["Application_Id"] = Application_Id;
            Session["Page_Title"] = ObjSysParam.GetSysTitle();
            Session["DateFormat"] = ObjSysParam.SetDateFormat();

            try
            {
                Session["emp_location"] = ObjEmpMaster.GetEmployeeMasterById(dtUser.Rows[0]["Company_Id"].ToString(), dtUser.Rows[0]["Emp_Id"].ToString()).Rows[0]["location_id"].ToString();
            }
            catch
            {
                Session["emp_location"] = "0";
            }
            Session["RoleId"] = dtUser.Rows[0]["Role_Id"].ToString();
            Session["HeaderText"] = "ERP";
            DataTable dtMessage1 = objPageCmn.GetArabicMessage();
            Session["MessageDt"] = dtMessage1;
            ObjSysParam.SetPageSize();
            string ipAddress1 = GetIpAddress()[0].ToString();
            string ipAddress2 = GetIpAddress()[1].ToString();
            bool IsValid = false;


            if (Session["UserId"].ToString().ToLower() == "superadmin")
            {
                IsValid = true;
            }
            else
            {
                DataTable dt_validUser = ObjUserMaster.GetUserMaster(HttpContext.Current.Session["CompId"].ToString());
                dt_validUser = new DataView(dt_validUser, "User_Id='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dt_validUser.Rows.Count > 0)
                {
                    if (dt_validUser.Rows[0]["IsGlobalAccess"].ToString().ToLower() == "true")
                    {
                        IsValid = true;
                    }
                    else
                    {
                        DataTable dt_ipData = objipAddress.GetAllActiveData();
                        dt_ipData = new DataView(dt_ipData, "is_blocked='False'", "", DataViewRowState.CurrentRows).ToTable();

                        for (int k = 0; k < dt_ipData.Rows.Count; k++)
                        {
                            if (ipAddress1 == dt_ipData.Rows[k]["ip_address"].ToString())
                            {
                                IsValid = true;
                                break;
                            }
                            if (ipAddress2 == dt_ipData.Rows[k]["ip_address"].ToString())
                            {
                                IsValid = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    DisplayMessage("Not A Valid User !!!");
                    IsValid = false;
                    return;
                }
            }


            if (IsValid)
            {
                SystemLog.SaveSystemLog("User Login", DateTime.Now.ToString(), strCompanyId, Session["UserId"].ToString(), "", ipAddress1, ipAddress1, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
                ObjUserMaster.UpdateLoginTime(HttpContext.Current.Session["UserId"].ToString());
                Response.Redirect("~/MasterSetup/Home.aspx?id=123456789");

            }
            else
            {
                DisplayMessage("IP Not Valid To Access Application!");
                txtUserName.Text = "";
                txtPassWord.Text = "";
                txtUserName.Focus();
            }
        }
        else
        {
            DisplayMessage("Invaild UserName or Password");
            txtUserName.Text = "";
            txtPassWord.Text = "";
            txtUserName.Focus();
        }
    }

    public string GetDatabaseName(string strConnection)
    {
        string DBName = string.Empty;
        foreach (string str in strConnection.Split(';'))
        {
            if (str.Contains("Initial Catalog"))
            {
                DBName = str.Split('=')[1];
            }
        }
        return DBName;
    }

    private void DefaultCheck()
    {
        try
        {
            DataTable dtInfo = objDa.return_DataTable("Select * From App_Settings");
            PegasusUtility.EncryptionUtility objEU = new PegasusUtility.EncryptionUtility();
            if (dtInfo.Rows.Count > 0)
            {
                string C1, C2, C3, C4;
                string[] res = objEU.Decrypt(dtInfo.Rows[0]["System_Code1"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                C1 = res[0].ToString();
                res = objEU.Decrypt(dtInfo.Rows[0]["System_Code2"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                C2 = res[0].ToString();
                res = objEU.Decrypt(dtInfo.Rows[0]["System_Code3"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                C3 = res[0].ToString();
                res = objEU.Decrypt(dtInfo.Rows[0]["System_Code4"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                C4 = res[0].ToString();
                if ((Settings.GetCode(1).ToString() == C1) && (Settings.GetCode(2).ToString() == C2) && (Settings.GetCode(3).ToString() == C3) && (Settings.GetCode(4).ToString() == C4))
                {
                    //Match project id
                    //Set both flag here then run application accordingly
                    res = objEU.Decrypt(dtInfo.Rows[0]["Installation_Type"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                    if (res[0].ToString() == "1")
                    {
                        Settings.IsDemo = true;
                    }
                    else
                    {
                        Settings.IsDemo = false;
                    }
                    res = objEU.Decrypt(dtInfo.Rows[0]["Expiry_Date"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                    if (DateTime.Now <= Convert.ToDateTime(res[0].ToString()))
                    {
                        Settings.IsValid = true;
                    }
                    else
                    {
                        Settings.IsValid = false;
                    }
                    res = objEU.Decrypt(dtInfo.Rows[0]["Project_Id"].ToString()).Split(new string[] { "##" }, StringSplitOptions.None);
                    string strProjectId = res[0].ToString();
                    DataTable DtApp_Id = objSys.GetSysParameterByParamName("Application_Id");
                    if (DtApp_Id.Rows[0]["Param_Value"].ToString() == strProjectId)
                    {
                    }
                    else
                    {
                        Response.Redirect("Error.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Error.aspx");
                }
            }
            else
            {
                Response.Redirect("Demo.aspx", false);
            }
        }
        catch
        {
            //Response.Redirect("Error.aspx");
        }
    }
    public void a()
    {
        int b = 0;
        int a;
        int j = 0;
        int k = 0;
    }
    protected string GetApplicationVersion()
    {
        return "Version - " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "-" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "-" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build.ToString() + "-" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
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
    protected void lnkFogetPassword_OnClick(object sender, EventArgs e)
    {
        if (txtUserName.Text == "")
        {
            DisplayMessage("Please Enter UserName Or Email-ID");
            txtUserName.Focus();
            return;
        }
        if (ddlCompany.SelectedValue == "--Select--")
        {
            DisplayMessage("Please Select Company");
            ddlCompany.Focus();
            return;
        }
        // SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PegaConnection"].ConnectionString);
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        bool Result = false;
        DataTable dt = new DataTable();
        try
        {
            lblErrormessage.Text = "";
            if (ddlCompany.SelectedValue != "--Select--")
            {
                dt = ObjUserMaster.GetUserMasterByUserId_ForgotPassword(txtUserName.Text, ddlCompany.SelectedValue.ToString(), ref trns);
            }
            //DataTable dt = objDa.return_DataTable("SELECT dbo.Set_UserMaster.Emp_Id, Set_EmployeeMaster.Email_Id, set_employeemaster.Emp_Name, Set_UserMaster.User_Id FROM dbo.Set_UserMaster LEFT JOIN dbo.Set_RoleMaster ON dbo.Set_UserMaster.Role_Id = dbo.Set_RoleMaster.Role_Id INNER JOIN dbo.Set_EmployeeMaster ON dbo.Set_UserMaster.Emp_Id = dbo.Set_EmployeeMaster.Emp_Id WHERE (Set_UserMaster.User_Id = '" + txtUserName.Text + "' OR Set_EmployeeMaster.Email_Id = '" + txtUserName.Text + "') AND Set_UserMaster.IsActive = 'True' AND Set_UserMaster.Company_Id = '" + ddlCompany.SelectedValue + "' AND Set_EmployeeMaster.IsActive = 'True' AND Set_EmployeeMaster.Field2 = 'False'");
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Email_Id"].ToString().Trim() == "")
                {
                    DisplayMessage("Sorry Your email-id is not registered with application, please contact to admin");
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    return;
                }
                else
                {
                    string strVerficationCode = (Convert.ToString(random.Next(10, 20000)));
                    objDa.execute_Command("update Set_UserMaster set Field5='" + strVerficationCode + "' where Emp_Id=" + dt.Rows[0]["Emp_Id"].ToString().Trim() + "", ref trns);
                    string strAppMailId = string.Empty;
                    string strAppPassword = string.Empty;
                    string strSmtp = string.Empty;
                    string str_smtp_port = string.Empty;
                    try
                    {
                        DataTable Dt_Configuration = ObjCompanyMaster.Get_Company_Configuration(txtUserName.Text.Trim(), "1", ref trns);
                        if (Dt_Configuration != null && Dt_Configuration.Rows.Count > 0)
                        {
                            strAppMailId = Dt_Configuration.Rows[0][0].ToString();
                            strAppPassword = Common.Decrypt(Dt_Configuration.Rows[1][0].ToString());
                        }
                        else
                        {
                            DisplayMessage("There may be Employee Terminated OR Not Registered");
                            strAppMailId = "";
                            strAppPassword = "";
                            return;
                        }
                        //strAppMailId = "Jitendra@pegasustech.net";
                        //strAppMailId = objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email' and Company_Id='" + ddlCompany.SelectedValue + "'", ref trns).Rows[0][0].ToString();
                    }
                    catch
                    {
                        strAppMailId = "";
                        strAppPassword = "";
                    }



                    strAppMailId = objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email' and Company_Id='" + ddlCompany.SelectedValue + "'", ref trns).Rows[0][0].ToString();
                    strAppPassword = Common.Decrypt(objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email_Password' and Company_Id='" + ddlCompany.SelectedValue + "'", ref trns).Rows[0][0].ToString());
                    strSmtp = objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email_SMTP' and Company_Id='" + ddlCompany.SelectedValue + "'", ref trns).Rows[0][0].ToString();
                    str_smtp_port = objDa.return_DataTable("select Set_ApplicationParameter.Param_Value from Set_ApplicationParameter where Param_Name = 'Master_Email_Port' and Company_Id='" + ddlCompany.SelectedValue + "'", ref trns).Rows[0][0].ToString();

                    string MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p><br />We received a request to access your pryce account.<br /><br />your pryce verification code is <b> " + strVerficationCode + "</b>.<br /><br />To change your password  <a href=" + GetForgotPasswordURL() + "?User_Id=@U&&Employee_Id=@E>ClickHere</a>.<br /><h3>Thanks & Regards</h3><h2>Pryce Support Team</h2></div></body></html>";
                    MailMessage = MailMessage.Replace("@U", HttpUtility.UrlEncode(Common.Encrypt(dt.Rows[0]["User_Id"].ToString())));
                    MailMessage = MailMessage.Replace("@E", HttpUtility.UrlEncode(Common.Encrypt(dt.Rows[0]["Emp_Id"].ToString())));
                    try
                    {
                        // Result = ObjSendMailSms.SendMail_TicketInfo(dt.Rows[0]["Email_Id"].ToString(), "", "", "Pryce Forgot password", MailMessage, ddlCompany.SelectedValue, "", strAppMailId.ToString(), strAppPassword.ToString(), "Pryce Forgot password", strSmtp, str_smtp_port, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        Result = ObjSendMailSms.SendMail_TicketInfo(dt.Rows[0]["Email_Id"].ToString(), "", "", "Pryce Forgot password", MailMessage, ddlCompany.SelectedValue, "", strAppMailId.ToString(), strAppPassword.ToString(), "Pryce Forgot password", strSmtp, str_smtp_port, "", "2", "2");
                    }
                    catch
                    {
                        DisplayMessage("Please try again or contact to Admin.");
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
                DisplayMessage("Verification code has been sent to your registered Email addresss, Please check and proceed");
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
        catch
        {
            lblErrormessage.Text = "Invalid Connection String";
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
        strFinalURL = strFinalURL.Replace("ERPLogin.aspx", "MasterSetUp/ForgetPassword.aspx");
        strFinalURL = strFinalURL.Replace("erplogin.aspx", "MasterSetUp/ForgetPassword.aspx");
        strFinalURL = strFinalURL.Replace(" ", "%20");
        return strFinalURL;
    }

    protected void btnredirecttocloud_Click(object sender, EventArgs e)
    {



    }
}