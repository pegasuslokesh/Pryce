using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;
using System.Configuration;
using PegasusDataAccess;

public partial class ERPMaster : System.Web.UI.MasterPage
{
    Common ObjCom = null;
    CompanyMaster ObjCompanyMaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    UserMaster objUserMaster = null;
    ModuleMaster ObjModuleMaster = null;
    SystemParameter ObjSysPeram = null;
    NotificationMaster Obj_Notification = null;
    EmployeeMaster objEmployee = null;
    Reminder Obj_reminder = null;
    ReminderLogs objReminderLog = null;
    DataAccessClass objDa = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjCom = new Common(Session["DBConnection"].ToString());
        ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objUserMaster = new UserMaster(Session["DBConnection"].ToString());
        ObjModuleMaster = new ModuleMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        Obj_Notification = new NotificationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        Obj_reminder = new Reminder(Session["DBConnection"].ToString());
        objReminderLog = new ReminderLogs(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {


            //code for configure financial year visibility on application basis


            //here we configuring financial year visibility according configured application
            if (Session["Application_Id"].ToString().Trim() == "1")
            {
                //for time man application , it should be false
                Div_Fyc.Visible = false;
                Li_Bug_data.Visible = false;
            }
            else
            {
                Div_Fyc.Visible = true;
                Li_Bug_data.Visible = true;

            }

            string TimeZoneId = HttpContext.Current.Session["TimeZoneId"].ToString();

            DateTime CaTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId));

            txtDateTimeNow.InnerText = CaTime.ToString("dd-MMM-yyyy hh:mm");
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            {
                try
                {
                    txtExpiryDate.InnerText = Convert.ToDateTime(Common.Decrypt(objDa.get_SingleValue("Select expiry_date from Application_Lic_main"))).ToString("dd-MMM-yyyy");
                }
                catch
                {

                }
            }
            else
            {
                lblExpiryDate.InnerText = "";
                lnkLicenceDetail.Visible = false;
            }
            
            
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            {
                Div_Active_User.Visible = false;
            }
            else
            {
                Div_Active_User.Visible = true;
            }

            try
            {
                Common.clsApplicationModules _cls = (Common.clsApplicationModules)Session["clsApplicationModule"];
                if (_cls.isProjectManagementModule)
                {
                    Li_Bug_data.Visible = true;
                }
                else
                {
                    Li_Bug_data.Visible = false;
                }
            }
            catch (Exception ex)
            {

            }

            Get_Emp_Details();

            try
            {
                FillLicenceDetails();
            }
            catch
            {

            }
            





            if (Session["Language"].ToString() == "US English")
            {
                if (Session["MenuItem"] == null)
                {
                    BindModuleList_LTR();
                }
            }
            else
            {
                if (Session["MenuItem"] == null)
                {
                    BindModuleList_RTL();
                }
            }
            Menu_Items.Text = Session["MenuItem"].ToString();


            BasePage bs = new BasePage();

            try
            {
                Lbl_Company_name.Text = Session["CompName"].ToString();
                Lbl_Location.Text = Session["LocName"].ToString();
                Lbl_Brand.Text = Session["BrandName"].ToString();
                Lbl_Language.Text = Session["Language"].ToString();
                Lbl_Currency.Text = Session["Currency"].ToString();
                Lbl_F_Year.Text = Session["FinanceCode"].ToString();
                //lblTotalUser.Text = Application["TotalUser"].ToString();
                string sql = "select count(*) from set_usermaster where (lastLoginTime>lastLogOutTime or lastLogOutTime is null) and datediff(hh,lastLoginTime,GETDATE())<6";
                lblTotalUser.Text = objDa.get_SingleValue(sql).ToString();
            }
            catch
            {

            }

            if (Session["SessEmpName"] == null)
            {
                string emp_name = " <img src=*" + Session["Emp_Image"].ToString() + "* class=*user-image* alt=**><span class=*hidden-xs*>" + GetEmpName(Session["UserId"].ToString()) + " (" + Session["UserId"].ToString() + ")" + "</span>";
                Ltr_Emp_Name.Text = emp_name.Replace('*', '"');
                Session["SessEmpName"] = Ltr_Emp_Name.Text;
            }
            else
            {
                Ltr_Emp_Name.Text = Session["SessEmpName"].ToString();
            }



            string strTitle = ConfigurationManager.AppSettings["AppType"].ToString();
            if (strTitle == "Timeman")
            {
                Page.Title = "Timeman Bio Attendance";
                // imgLogo.Src = "images/timeman.png";
            }
            else
            {
                Page.Title = Session["Page_Title"].ToString();
            }


            if (Session["SessFooterContent"] == null)
            {
                string Footer = "";

                Footer = "<div class=*pull-right hidden-xs*>      <b>Version</b> " + ConfigurationManager.AppSettings["AppVersion"].ToString() + "" + ConfigurationManager.AppSettings["AppRelease"].ToString() + "    </div>    <strong>Copyright &copy;  Pryce All rights reserved";

                Ltr_Footer_Content.Text = Footer.Replace('*', '"');
                Session["SessFooterContent"] = Ltr_Footer_Content.Text;
            }
            else
            {
                Ltr_Footer_Content.Text = Session["SessFooterContent"].ToString();
            }


            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
                Session["CompId"] = Session["CompId"].ToString();

                string strcompanyId = Session["CompId"].ToString();
                body1.Style[HtmlTextWriterStyle.Direction] = "ltr";
            }
            else if (Session["lang"].ToString() == "2")
            {
                body1.Style[HtmlTextWriterStyle.Direction] = "rtl";
            }
            else if (Session["lang"].ToString() == "1")
            {
                body1.Style[HtmlTextWriterStyle.Direction] = "ltr";
            }


            Session["NotificationCount"] = 10;
            Session["ReminderCount"] = 10;
            try
            {
                ReminderNotification();
                //Get_Notification();
            }
            catch (Exception err)
            {
            }

            try
            {
                if (Session["bInfoRefreshed"] == null)
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "", "refreshNotifications();", true);
                    Session["bInfoRefreshed"] = true;
                }
            }
            catch (Exception ex)
            {

            }


            DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
            Session["AllLocation"] = dtLoc;
            try
            {
                dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {

            }


            if (!Common.GetStatus(HttpContext.Current.Session["CompId"].ToString()))
            {
                string LocIds = ObjCom.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                Session["MyAllLoc"] = LocIds;

                if (LocIds != "")
                {
                    dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            if (dtLoc.Rows.Count > 0)
            {
                ddlLocationList.DataSource = dtLoc;
                ddlLocationList.DataTextField = "Location_Name";
                ddlLocationList.DataValueField = "Location_Id";
                ddlLocationList.DataBind();

                ddlLocationList.SelectedValue = Session["LocId"].ToString();
            }
            else
            {
                ddlLocationList = null;
            }

        }

        if (Session["Language"].ToString() != null)
        {
            if (Session["Language"].ToString() == "US English")
            {
                string Css = "<link href=*../BootStrap_Files/bootstrap/css/bootstrap.min.css* rel=*stylesheet* /><link href=*../BootStrap_Files/dist/css/AdminLTE.min.css* rel=*stylesheet* />";
                Ltr_Change_CSS.Text = Css.Replace('*', '"');
                //BindModuleList_LTR();
            }
            else
            {
                string Css = "<link href=*../BootStrap_LTR/bootstrap/css/bootstrap.min.css* rel=*stylesheet* /><link href=*../BootStrap_LTR/dist/css/AdminLTE.min.css* rel=*stylesheet* />";
                Ltr_Change_CSS.Text = Css.Replace('*', '"');
                //BindModuleList_RTL();
            }
        }
    }

    public void FillLicenceDetails()
    {
        DataTable dt = objDa.return_DataTable("Select * from Application_Lic_Main where IsActive='1'");
        if (dt.Rows.Count > 0)
        {
            try
            {
                txtcompanyname.Text = Common.Decrypt(dt.Rows[0]["company_name"].ToString());
                txtCountry.Text = Common.Decrypt(dt.Rows[0]["Field1"].ToString());
                txtRegistrationCode.Text = Common.Decrypt(dt.Rows[0]["registration_code"].ToString());
                txtLicenceKey.Text = Common.Decrypt(dt.Rows[0]["license_key"].ToString());
                lbExpiryDate.Text = Common.Decrypt(dt.Rows[0]["expiry_date"].ToString());
                lblEmailID.Text = Common.Decrypt(dt.Rows[0]["email"].ToString());
                lblAppMode.Text = "Licenced";
                lblProductCode.Text = Common.Decrypt(dt.Rows[0]["product_code"].ToString());
                txtDevice.Text = Common.Decrypt(dt.Rows[0]["att_device_count"].ToString());
                lblUsers.Text = Common.Decrypt(dt.Rows[0]["no_of_employee"].ToString());
                lblEmployees.Text = Common.Decrypt(dt.Rows[0]["no_of_employee"].ToString());
            }
            catch (Exception ex)
            {

            }
        }
    }


    public void Get_Emp_Details()
    {
        string Emp_Designation = string.Empty;
        string Emp_Designation_L = string.Empty;
        string Emp_DOJ = string.Empty;
        string Emp_Image = string.Empty;
        DataTable dtEmpDetails = (DataTable)Session["dtEmpDetails"];
        if (dtEmpDetails.Rows.Count > 0)
        {
            try
            {
                Emp_Designation = dtEmpDetails.Rows[0]["Emp_Designation"].ToString();
                Emp_Designation_L = dtEmpDetails.Rows[0]["Emp_Designation_L"].ToString();
                if (dtEmpDetails.Rows[0]["EmpDOJ"].ToString() != null && dtEmpDetails.Rows[0]["EmpDOJ"].ToString() != "")
                {
                    DateTime DTime = Convert.ToDateTime(dtEmpDetails.Rows[0]["EmpDOJ"].ToString());
                    Emp_DOJ = Convert.ToString(DTime.ToString("MMM.  yyyy"));
                }
            }
            catch
            { }

            try
            {

                if (dtEmpDetails.Rows[0]["Emp_Image"].ToString() != null)
                {
                    if (File.Exists(Server.MapPath("../CompanyResource/2/" + dtEmpDetails.Rows[0]["Emp_Image"].ToString())) == true)
                    {
                        Emp_Image = "../CompanyResource/" + Session["CompId"] + "/" + dtEmpDetails.Rows[0]["Emp_Image"].ToString();
                        Session["Emp_Image"] = Emp_Image;
                    }
                    else
                    {
                        Emp_Image = "../Bootstrap_Files/dist/img/Bavatar.png";
                        Session["Emp_Image"] = Emp_Image;
                    }
                }
                else
                {
                    Emp_Image = "../Bootstrap_Files/dist/img/Bavatar.png";
                    Session["Emp_Image"] = Emp_Image;
                }


            }
            catch
            {


            }
        }
        else
        {
            Emp_Designation = "";
            Emp_Designation_L = "";
            Emp_DOJ = "";
            Emp_Image = "../Bootstrap_Files/dist/img/Bavatar.png";
            Session["Emp_Image"] = Emp_Image;
        }
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
            Session["CompId"] = Session["CompId"].ToString();
            string Emp_Ltr = "<li class=*user-header*><img src=*" + Emp_Image + "* class=*img-circle* alt=**><p>" + Emp_Designation + "<small> " + Emp_DOJ + "</small></p></li>";
            Ltr_Emp_Details.Text = Emp_Ltr.Replace('*', '"');
        }
        else if (Session["lang"].ToString() == "2")
        {
            string Emp_Ltr = "<li class=*user-header*><img src=*" + Emp_Image + "* class=*img-circle* alt=**><p>" + Emp_Designation_L + "<small> " + Emp_DOJ + "</small></p></li>";
            Ltr_Emp_Details.Text = Emp_Ltr.Replace('*', '"');
        }
        else if (Session["lang"].ToString() == "1")
        {
            string Emp_Ltr = "<li class=*user-header*><img src=*" + Emp_Image + "* class=*img-circle* alt=**><p>" + Emp_Designation + "<small> " + Emp_DOJ + "</small></p></li>";
            Ltr_Emp_Details.Text = Emp_Ltr.Replace('*', '"');
        }
    }

    public string GetEmpName(string UserId)
    {
        string EmpName = string.Empty;
        DataTable dtUser = (DataTable)Session["dtEmpDetails"];

        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
            Session["CompId"] = Session["CompId"].ToString();
            if (dtUser.Rows.Count > 0)
            {
                EmpName = dtUser.Rows[0]["EmpName"].ToString();
                if (EmpName == "")
                {
                    EmpName = UserId;
                }
            }
            else
            {
                EmpName = UserId;
            }

            if (EmpName == "")
            {
                EmpName = UserId;
            }
        }
        else if (Session["lang"].ToString() == "2")
        {
            if (dtUser.Rows.Count > 0)
            {
                EmpName = dtUser.Rows[0]["EmpName_L"].ToString();
                if (EmpName == "")
                {
                    EmpName = UserId;
                }
            }
            else
            {
                EmpName = UserId;
            }

            if (EmpName == "")
            {
                EmpName = UserId;
            }
        }
        else if (Session["lang"].ToString() == "1")
        {
            if (dtUser.Rows.Count > 0)
            {
                EmpName = dtUser.Rows[0]["EmpName"].ToString();
                if (EmpName == "")
                {
                    EmpName = UserId;
                }
            }
            else
            {
                EmpName = UserId;
            }

            if (EmpName == "")
            {
                EmpName = UserId;
            }
        }


        return EmpName;
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
    protected void btn_log_Out_Click(object sender, EventArgs e)
    {
        //user log out log entry 
        SystemLog.SaveSystemLog("User LogOut", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "", GetIpAddress()[0].ToString(), GetIpAddress()[1].ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["DBConnection"].ToString());
        objUserMaster.UpdateLogOutTime(Session["UserId"].ToString());
        string Reg_Code = "";
        try
        {
             Reg_Code = Session["RegistrationCode"].ToString();
        }
        catch(Exception ex)
        {
            Reg_Code = "";
        }
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/ERPLogin.aspx?reg_code="+Reg_Code+"");
    }

    public void BindModuleList_LTR()
    {

        string Application_Id = Session["Application_Id"].ToString();
        DataTable dtAllModule1 = (DataTable)Session["dtAllModule1"];
        string Li_Source = "";

        DataView Parent_View = new DataView(dtAllModule1);
        DataTable Parent_Table = Parent_View.ToTable(true, "ParentModuleName", "ParentModuleImage");
        for (int i = Parent_Table.Rows.Count - 1; i > 0; i--)
        {
            if (Parent_Table.Rows[i][0].ToString() == "")
            {
                Parent_Table.Rows[i].Delete();
            }
        }
        string Parent = "";
        string Sub_Menu = "";
        string Child_Menu = "";
        string Final_Close = "";
        string Close = "</li></ul></li>";
        DataTable dt_Pt = new DataTable();

        foreach (DataRow Parent_Row in Parent_Table.Rows)
        {
            Child_Menu = "";
            Sub_Menu = "";
            dt_Pt = new DataView(dtAllModule1, "ParentModuleName='" + Parent_Row["ParentModuleName"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            Parent = "<li class=*treeview*><a href=*#*>" + Parent_Row["ParentModuleImage"].ToString() + " <span>" + Parent_Row["ParentModuleName"].ToString() + "</span><span class=*pull-right-container*><i class=*fa fa-angle-left pull-right*></i></span></a>";

            DataView Sub_Patent_View = new DataView(dt_Pt);
            DataTable Sub_Patent_Table = Sub_Patent_View.ToTable(true, "Module_Name", "DashBoardIconUrl");

            string Front_Child = "<ul class=*treeview-menu*>";

            foreach (DataRow Sub_Parent_Row in Sub_Patent_Table.Rows)
            {
                DataTable dt_Child = new DataTable();
                dt_Child = new DataView(dtAllModule1, "Module_Name='" + Sub_Parent_Row["Module_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Child_Menu = "";
                string Child_Start = "<ul class=*treeview-menu*>";

                dt_Child = dt_Child.DefaultView.ToTable(true, "Object_Name", "Page_URL", "Module_Id");


                foreach (DataRow Child_Row in dt_Child.Rows)
                {
                    if (Child_Row["Module_Id"].ToString() == "105" || Child_Row["Module_Id"].ToString() == "109" || Child_Row["Module_Id"].ToString() == "145")
                    {
                        Child_Menu += "  <li><a href=*" + Child_Row["Page_URL"].ToString() + "* target='_blank' class='acc'><i class=*fa fa-circle-thin*></i> " + Child_Row["Object_Name"].ToString() + "</a></li>";
                    }
                    else
                    {
                        Child_Menu += "  <li><a href=*" + Child_Row["Page_URL"].ToString() + "*><i class=*fa fa-circle-thin*></i> " + Child_Row["Object_Name"].ToString() + "</a></li>";
                    }

                }
                if (Sub_Parent_Row["Module_Name"].ToString() != Parent_Row["ParentModuleName"].ToString())
                    Sub_Menu += " <li> <a href=*#*><img src=*" + Sub_Parent_Row["DashBoardIconUrl"].ToString() + "*  width=*25px* height=*25px*/>" + Sub_Parent_Row["Module_Name"] + "<span class=*pull-right-container*><i class=*fa fa-angle-left pull-right*></i></span></a>" + Child_Start + Child_Menu + "</ul></li>";
                else
                    Sub_Menu += Child_Menu;
            }

            string End_Child = "</ul></li>";

            Final_Close = Final_Close + Parent + Front_Child + Sub_Menu + End_Child;
        }

        string Menu_Content = "<aside class=*main-sidebar*>    <section class=*sidebar*>      <div style=*display:none;* class=*user-panel*>        <div class=*pull-left image*>          <img src=*" + Session["Emp_Image"].ToString() + "* class=*img-circle* alt=**>        </div>        <div class=*pull-left info*>          <p>" + GetEmpName(Session["UserId"].ToString()) + "</p>          <a href=*#*><i class=*fa fa-circle-thin text-success*></i> Online</a>        </div>      </div>  <div action=*#* method=*get* class=*sidebar-form*><div ><input type=*text* name=*q* class=*form-control search-menu-box* placeholder=*Search...*><span class=*input-group-btn*></span></div></div><ul class=*sidebar-menu* id=*menu*><li class=*header*></li>" + Final_Close + "</ul></section></aside>";
        Menu_Content = Menu_Content.Replace('*', '"');
        Session["MenuItem"] = Menu_Content;
        //Menu_Items.Text = Menu_Content.Replace('*', '"');
    }

    public void BindModuleList_RTL()
    {

        string Application_Id = Session["Application_Id"].ToString();
        DataTable dtAllModule1 = (DataTable)Session["dtAllModule1"];
        string Li_Source = "";

        DataView Parent_View = new DataView(dtAllModule1);
        DataTable Parent_Table = Parent_View.ToTable(true, "ParentModuleName_L", "ParentModuleImage");
        for (int i = Parent_Table.Rows.Count - 1; i > 0; i--)
        {
            if (Parent_Table.Rows[i][0].ToString() == "")
            {
                Parent_Table.Rows[i].Delete();
            }
        }
        string Parent = "";
        string Sub_Menu = "";
        string Child_Menu = "";
        string Final_Close = "";
        string Close = "</li></ul></li>";
        DataTable dt_Pt = new DataTable();

        foreach (DataRow Parent_Row in Parent_Table.Rows)
        {
            Child_Menu = "";
            Sub_Menu = "";
            dt_Pt = new DataView(dtAllModule1, "ParentModuleName_L='" + Parent_Row["ParentModuleName_L"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            Parent = "<li class=*treeview*><a href=*#*>" + Parent_Row["ParentModuleImage"].ToString() + " <span>" + Parent_Row["ParentModuleName_L"].ToString() + "</span><span class=*pull-right-container*><i class=*fa fa-angle-left pull-right*></i></span></a>";

            DataView Sub_Patent_View = new DataView(dt_Pt);
            DataTable Sub_Patent_Table = Sub_Patent_View.ToTable(true, "Module_Name_L", "DashBoardIconUrl");

            string Front_Child = "<ul class=*treeview-menu*>";

            foreach (DataRow Sub_Parent_Row in Sub_Patent_Table.Rows)
            {
                DataTable dt_Child = new DataTable();
                dt_Child = new DataView(dtAllModule1, "Module_Name_L='" + Sub_Parent_Row["Module_Name_L"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Child_Menu = "";
                string Child_Start = "<ul class=*treeview-menu*>";
                foreach (DataRow Child_Row in dt_Child.Rows)
                {
                    Child_Menu += "  <li><a href=*" + Child_Row["Page_URL"].ToString() + "*><i class=*fa fa-circle-thin*></i> " + Child_Row["Object_Name_L"].ToString() + "</a></li>";
                }
                if (Sub_Parent_Row["Module_Name_L"].ToString() != Parent_Row["ParentModuleName_L"].ToString())
                    Sub_Menu += " <li> <a href=*#*><img src=*" + Sub_Parent_Row["DashBoardIconUrl"].ToString() + "*  width=*25px* height=*25px*/>" + Sub_Parent_Row["Module_Name_L"] + "<span class=*pull-right-container*><i class=*fa fa-angle-left pull-right*></i></span></a>" + Child_Start + Child_Menu + "</ul></li>";
                else
                    Sub_Menu += Child_Menu;
            }

            string End_Child = "</ul></li>";

            Final_Close = Final_Close + Parent + Front_Child + Sub_Menu + End_Child;
        }

        string Menu_Content = "<aside class=*main-sidebar*>    <section class=*sidebar*>      <div style=*display:none;* class=*user-panel*>        <div class=*pull-left image*>          <img src=*" + Session["Emp_Image"].ToString() + "* class=*img-circle* alt=**>        </div>        <div class=*pull-left info*>          <p>" + GetEmpName(Session["UserId"].ToString()) + "</p>          <a href=*#*><i class=*fa fa-circle-thin text-success*></i> Online</a>        </div>      </div>  <div action=*#* method=*get* class=*sidebar-form*><div ><input type=*text* name=*q* class=*form-control search-menu-box* placeholder=*Search...*><span class=*input-group-btn*></span></div></div><ul class=*sidebar-menu* id=*menu*><li class=*header*></li>" + Final_Close + "</ul></section></aside>";
        //Menu_Items.Text = Menu_Content.Replace('*', '"');
        Session["MenuItem"] = Menu_Content.Replace('*', '"');
    }

    public void Get_Notification()
    {
        try
        {


            if (Session["CompId"] != null && Session["BrandId"] != null && Session["LocId"] != null && Session["UserId"] != null && Session["NotificationCount"] != null)
            {
                DataTable Dt_Notification = Obj_Notification.Get_Notification(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString(), Session["NotificationCount"].ToString());
                string Start = string.Empty;
                string Alert_Message = string.Empty;
                string End = string.Empty;

                //int numberOfRecords = Dt_Notification.AsEnumerable().Where(x => x["Is_read"].ToString() == "False").ToList().Count;
                int numberOfRecords = 0;
                if (Dt_Notification.Rows.Count > 0)
                {
                    numberOfRecords = Convert.ToInt32(Dt_Notification.Rows[0]["Tot_Record"].ToString());
                }

                DataTable Dt_reminder = Obj_Notification.Get_ReminderNotification_By_EmpId(Session["EmpId"].ToString(), Session["ReminderCount"].ToString());

                string number = "";
                int IsReadNo = 0;
                DataView dv = new DataView(Dt_reminder, "Is_read='False'", "", DataViewRowState.CurrentRows);
                IsReadNo = dv.Count;

                if (IsReadNo == 0)
                {
                    number = "";
                }
                else
                {
                    number = IsReadNo.ToString();
                }

                string demo = "";




                for (int i = 0; i < Dt_reminder.Rows.Count; i++)
                {
                    demo = demo + "<li> <a href=" + Dt_reminder.Rows[i]["Link_url"].ToString() + " title=*" + Dt_reminder.Rows[i]["N_Message"].ToString() + "*> <i class=*fa fa-users text-aqua*></i> " + Dt_reminder.Rows[i]["N_Message"].ToString() + " </a> </li>";
                }

                if (numberOfRecords != 0)
                    Start = "<li id=*Li_Remider_Menu* class=*dropdown notifications-menu*> <a href=*#* onclick=*Read_Reminder_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*> <i class=*fa fa-clock-o*></i> <span class=*label label-warning*>" + number + "</span> </a> <ul class=*dropdown-menu*> <li class=*header*>All Reminder Logs</li> <li> <!-- inner menu: contains the actual data --> <ul class=*menu*> " + demo + "  </ul> </li> <li class=*footer*><a href=*#*  onclick=*View_More_Reminder()*>View More</a></li> </ul> </li>                     <li id=*Li_Menu* class=*dropdown messages-menu*><a id=*A_Dropdown* href = *#* onclick=*Read_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*><i class=*fa fa-bell-o*></i><span class=*label label-success*>" + numberOfRecords + "</span></a><ul class=*dropdown-menu*><li><ul class=*menu*>";
                else
                    Start = "<li id=*Li_Remider_Menu* class=*dropdown notifications-menu*> <a href=*#* onclick=*Read_Reminder_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*> <i class=*fa fa-clock-o*></i> <span class=*label label-warning*>" + number + "</span> </a> <ul class=*dropdown-menu*> <li class=*header*>All Remider Logs</li> <li> <!-- inner menu: contains the actual data --> <ul class=*menu*> " + demo + " </ul> </li> <li class=*footer*><a href=*#*  onclick=*View_More_Reminder()*>View More</a></li> </ul> </li>                      <li id=*Li_Menu* class=*dropdown messages-menu*><a id=*A_Dropdown* href = *#* onclick=*Read_Notification()* class=*dropdown-toggle* data-toggle=*dropdown*><i class=*fa fa-bell-o*></i></a><ul class=*dropdown-menu*><li><ul class=*menu*>";


                End = "</ul></li></li><li class=*footer*><a href=*#* onclick=*View_More()*>More</a></li></ul></li>";
                int Tot_Row = 0;
                if (Convert.ToInt16(Session["NotificationCount"].ToString()) <= Dt_Notification.Rows.Count)
                {
                    Tot_Row = Convert.ToInt16(Session["NotificationCount"].ToString());
                }
                if (Convert.ToInt16(Session["NotificationCount"].ToString()) > Dt_Notification.Rows.Count)
                {
                    Tot_Row = Convert.ToInt16(Dt_Notification.Rows.Count.ToString());
                }
                for (int i = 0; i < Tot_Row; i++)
                {
                    string Time = string.Empty;

                    if (Convert.ToInt64(Dt_Notification.Rows[i]["Years"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Months"].ToString()) >= 12 && Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) >= 366)
                    {
                        Time = Dt_Notification.Rows[i]["Years"].ToString() + " Year";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Months"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) >= 30)
                    {
                        Time = Dt_Notification.Rows[i]["Months"].ToString() + " Months";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(Dt_Notification.Rows[i]["Days"].ToString()) == 1)
                            Time = " Yesterday";
                        else
                            Time = Dt_Notification.Rows[i]["Days"].ToString() + " Days";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Hours"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(Dt_Notification.Rows[i]["Hours"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Minutes"].ToString()) <= 59)
                            Time = Dt_Notification.Rows[i]["Minutes"].ToString() + " Minutes";
                        else
                            Time = Dt_Notification.Rows[i]["Hours"].ToString() + " Hours";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Minutes"].ToString()) > 0)
                    {
                        if (Convert.ToInt64(Dt_Notification.Rows[i]["Minutes"].ToString()) > 0 && Convert.ToInt64(Dt_Notification.Rows[i]["Seconds"].ToString()) <= 59)
                            Time = Dt_Notification.Rows[i]["Seconds"].ToString() + " Seconds";
                        else
                            Time = Dt_Notification.Rows[i]["Minutes"].ToString() + " Minutes";
                    }
                    else if (Convert.ToInt64(Dt_Notification.Rows[i]["Seconds"].ToString()) >= 0)
                    {
                        Time = Dt_Notification.Rows[i]["Seconds"].ToString() + " Seconds";
                    }
                    int Msg_Length = 0;
                    if (Dt_Notification.Rows[i]["Message_Length"].ToString() != "")
                        Msg_Length = Convert.ToInt32(Dt_Notification.Rows[i]["Message_Length"].ToString());
                    int message_s = Dt_Notification.Rows[i]["N_Message"].ToString().Length;
                    string msg = "";
                    if (message_s > Msg_Length)
                    {
                        msg = Dt_Notification.Rows[i]["N_Message"].ToString().Substring(0, Convert.ToInt32(Msg_Length));
                    }
                    else
                    {
                        msg = Dt_Notification.Rows[i]["N_Message"].ToString();
                    }
                    string Emp_Image_Notification = Dt_Notification.Rows[i]["Field1"].ToString();

                    if (Dt_Notification.Rows[i]["Field1"].ToString() != null)
                    {
                        if (File.Exists(Server.MapPath(Dt_Notification.Rows[i]["Field1"].ToString())) == true)
                        {
                            Emp_Image_Notification = Dt_Notification.Rows[i]["Field1"].ToString();
                        }
                        else
                        {
                            Emp_Image_Notification = "../Bootstrap_Files/dist/img/Bavatar.png";
                        }
                    }
                    else
                    {
                        Emp_Image_Notification = "../Bootstrap_Files/dist/img/Bavatar.png";
                    }

                    Alert_Message = Alert_Message + "<li><a id=*mybtn* title=*" + Dt_Notification.Rows[i]["N_Message"].ToString() + "* href = *" + Dt_Notification.Rows[i]["Link_url"].ToString() + "* ><div class=*pull-left*><img src = *" + Emp_Image_Notification + "* class=*img-circle* alt=**></div><h4>" + Dt_Notification.Rows[i]["Title"].ToString() + "<small><i class=*fa fa-clock-o*></i>" + " " + "" + Time + "</small></h4><p>" + msg + "</p></a></li>";
                }
                //Ltr_Alert_Notification.Text = Start.Replace('*', '"') + Alert_Message.Replace('*', '"') + End.Replace('*', '"');
            }
        }
        catch (Exception err)
        {

        }
    }

    //protected void Btn_More_Record_ServerClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Notification_Open()", true);
    //        Session["NotificationCount"] = Convert.ToInt16(Session["NotificationCount"].ToString()) + 10;
    //        Get_Notification();
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Notification_Open()", true);
    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void Btn_Read_Notification_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int Update_Record = Obj_Notification.Update_Notification(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["UserId"].ToString());
    //        Get_Notification();
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Notification_Open()", true);
    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void Btn_Read_Reminder_Notification_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int Update_Record = Obj_Notification.Update_Reminder_Notification(Session["UserId"].ToString());
    //        Get_Notification();
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Reminder_Notification_Open()", true);
    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void Timer2_Tick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Get_Notification();
    //    }
    //    catch
    //    {

    //    }
    //}

    private void ReminderNotification()
    {
        DateTime date = Convert.ToDateTime(GetDate(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString()));
        using (DataTable dt = Obj_reminder.CurrentDateReminderData(Session["EmpId"].ToString()))
        {
            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["Status"].ToString() == "On" && dr["Is_read"].ToString() == "False" && dr["Remark"].ToString() != "1")
                        {
                            Send_Notification_Task(dr["Reminder_text"].ToString(), dr["Remind_to"].ToString(), dr["Target_url"].ToString(), dr["Trans_id"].ToString());
                            objReminderLog.Set_LogsNotificationFlag(dr["LogTransId"].ToString());
                        }
                    }

                }

        }
    }


    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(ObjSysPeram.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

    protected void Send_Notification_Task(string message, string id, string url, string ref_id)
    {
        int Save_Notification = 0;
        string Message = string.Empty;
        Message = message;
        GetEmployeeName(Session["EmpId"].ToString());
        Save_Notification = Obj_Notification.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), id, Message, "39", url, "Reminder", ref_id, "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "0", "18");
    }

    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Img"] = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            ViewState["Emp_Img"] = "";
        }
        return EmployeeName;
    }

    //protected void Btn_More_Reminder_Record_ServerClick(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Reminder_Notification_Open()", true);
    //        Session["ReminderCount"] = Convert.ToInt16(Session["ReminderCount"].ToString()) + 10;
    //        Get_Notification();
    //        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Reminder_Notification_Open()", true);
    //    }
    //    catch
    //    {

    //    }

    //}

    protected void lnkTotalActiveUser_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "select um.User_Id, sem.Emp_Name, lastLoginTime  from set_usermaster um left join Set_EmployeeMaster sem on sem.Emp_Id=um.Emp_Id where (um.lastLoginTime>um.lastLogOutTime or um.lastLogOutTime is null) and datediff(hh,um.lastLoginTime,GETDATE())<6";
            using (DataTable _dt = objDa.return_DataTable(sql))
            {
                if (_dt.Rows.Count > 0)
                {
                    gvActiveUsers.DataSource = _dt;
                    gvActiveUsers.DataBind();
                    upActiveUser.Update();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "fnShowModalActiveUser()", true);
                }
                else
                {

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "alert('There is some error please contact to technical person')", true);
        }
    }

    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LocId"] = ddlLocationList.SelectedValue.ToString();
        Response.Redirect(HttpContext.Current.Request.Url.PathAndQuery);
    }
}

