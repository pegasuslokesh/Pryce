using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using PegasusDataAccess;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Diagnostics;
public partial class MasterSetUp_EmployeeMaster : BasePage
{
    #region defind Class Object
    Set_Approval_Employee objApprovalEmp = null;
    Set_Location_Department objLocDept = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    Att_DeviceMaster objDevice = null;
    Ser_UserTransfer objSer = null;
    Common ObjComman = null;
    Set_Bank_Info objBankInfo = null;
    CountryMaster ObjSysCountryMaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocMaster = null;
    Set_BankMaster objBank = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Group_Employee objGroupEmp = null;
    DepartmentMaster objDep = null;
    ReligionMaster objRel = null;
    NationalityMaster objNat = null;
    DesignationMaster objDesg = null;
    QualificationMaster objQualif = null;
    EmployeeMaster objEmp = null;
    SystemParameter objSys = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    Att_Employee_Leave objEmpleave = null;
    LeaveMaster objLeaveType = null;
    Att_Employee_Notification objEmpNotice = null;
    Set_ApplicationParameter objAppParam = null;
    CurrencyMaster objCurrency = null;
    CompanyMaster objComp = null;
    EmployeeParameter objEmpParam = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Emp_SalaryIncrement objEmpSalInc = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Document_Master ObjDocument = null;
    Att_HalfDay_Request objHalfDay = null;
    Ser_ReportMaster objReportMaster = null;
    Ser_ReportNotification objReportNotification = null;
    Ser_ReportSetup objReportSetup = null;
    HR_Indemnity_Master objIndemnity = null;
    Attendance objAtt = null;
    Country_Currency objCountryCurrency = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    DataAccessClass objDa = null;
    Pay_SalaryPlanHeader ObjSalaryPlan = null;
    Pay_SalaryPlanDetail objsalaryplandetail = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc = null;
    Pay_AdvancePayment objAdvancePayment = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_SubChartOfAccount objSubCOA = null;
    UserMaster ObjUser = null;
    hr_laborLaw_leave ObjLabourLeavedetail = null;
    UserDataPermission ObjUserdataPeram = null;
    Att_DeviceGroupMaster ObjdeviceGroup = null;
    Pay_SalaryPlanDetail Objsalaryplandetail = null;
    DateTime Increment_Date = new DateTime();
    Att_Device_Operation ObjdeviceOp = null;
    Set_DocNumber objDocNo = null;
    ContactNoMaster objContactnoMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;
    public const int grdDefaultColCount = 6;
    private const string strPageName = "EmployeeMaster";
    #endregion
    int Fr_IncrementDuration = 0;
    int Exp_IncrementDuration = 0;
    DataTable dtDevice = new DataTable();
    string EmpSync = string.Empty;
    string strForEmpCode = string.Empty;
    string strForAllLocation = string.Empty;
    private int PageSize = 9;


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)
    {
        try
        {
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            CompanyMaster objComp = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string compid = HttpContext.Current.Session["CompId"].ToString();

            try
            {
                try
                {
                    string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                    fullname = fullname.Replace("Product_", "Product//Product_");
                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                    Byte[] buffer = br.ReadBytes((int)numBytes);
                    string fullPath = "~/CompanyResource/" + RegistrationCode + "/" + compid + "";
                    string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                    if (Directory.Exists(MapfullPath))
                    {
                        if (RegistrationCode != "" && RegistrationCode != null)
                        {
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(MapfullPath);
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                    Byte[] buffer = br.ReadBytes((int)numBytes);
                    string fullPath = "~/CompanyResource/" + compid + "";
                    string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                    if (Directory.Exists(MapfullPath))
                    {
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    else
                    {
                        try
                        {
                            Directory.CreateDirectory(MapfullPath);
                            File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                        }
                        catch (Exception exp)
                        {

                        }
                    }
                }
            }
            catch
            {
            }
            HttpContext.Current.Session["empimgpath"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
        }
    }

    protected void ASPxFileManager1_SelectedFileOpened(object source, DevExpress.Web.FileManagerFileOpenedEventArgs e)
    {
        Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
        try
        {
            File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + e.File.Name), buffer);
        }
        catch
        {
        }
        imgLogo.ImageUrl = e.File.FullName.ToString();
        Session["empimgpath"] = e.File.Name;
    }

    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objApprovalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
        objDevice = new Att_DeviceMaster(Session["DBConnection"].ToString());
        objSer = new Ser_UserTransfer(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objBankInfo = new Set_Bank_Info(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocMaster = new LocationMaster(Session["DBConnection"].ToString());
        objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objRel = new ReligionMaster(Session["DBConnection"].ToString());
        objNat = new NationalityMaster(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objQualif = new QualificationMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objLeaveType = new LeaveMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objEmpSalInc = new Set_Emp_SalaryIncrement(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objReportMaster = new Ser_ReportMaster(Session["DBConnection"].ToString());
        objReportNotification = new Ser_ReportNotification(Session["DBConnection"].ToString());
        objReportSetup = new Ser_ReportSetup(Session["DBConnection"].ToString());
        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objAtt = new Attendance(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSalaryPlan = new Pay_SalaryPlanHeader(Session["DBConnection"].ToString());
        objsalaryplandetail = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(Session["DBConnection"].ToString());
        objAdvancePayment = new Pay_AdvancePayment(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        ObjLabourLeavedetail = new hr_laborLaw_leave(Session["DBConnection"].ToString());
        ObjUserdataPeram = new UserDataPermission(Session["DBConnection"].ToString());
        ObjdeviceGroup = new Att_DeviceGroupMaster(Session["DBConnection"].ToString());
        Objsalaryplandetail = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        ObjdeviceOp = new Att_Device_Operation(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        try
        {
            if (ConfigurationManager.AppSettings["ClientName"].ToString().Trim() == "NIC")
            {
                rbtnEmployee.Text = "Face";
                rbtnManager.Text = "Sign";
                rbtnCEO.Text = "No Sign";
            }
        }
        catch
        {

        }

        hdntxtaddressid.Value = txtAddressName.ID;
        if (!IsPostBack)
        {
            Session["empimgpath"] = "";
            Session["empSignimgpath"] = "";
            Session["AddCtrl_Country_Id"] = "";
            Session["AddCtrl_State_Id"] = "";

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "15", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim() == "Cloud")
            {
                btnRunservice.Visible = false;
            }
            else
            {
                btnRunservice.Visible = true;
            }
            Check_Financial_Year_Status();
            Session["dtModule_15"] = null;
            btnTerDel.ToolTip = "Deleted";
            EmpSync = objAppParam.GetApplicationParameterValueByParamName("Employee Synchronization", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            dtDevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());
            if (EmpSync == "Company")
            {
            }
            else
            {
                dtDevice = new DataView(dtDevice, "Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtDevice.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)listEmpSync, dtDevice, "Device_Name", "Device_Id");
                foreach (ListItem li in listEmpSync.Items)
                {
                    li.Selected = true;
                }
            }
            //Update On 20-04-2015 for All Location Data
            DataTable dtAllLocation = objAppParam.GetApplicationParameterByCompanyId("For All Location", Session["CompId"].ToString());
            dtAllLocation = new DataView(dtAllLocation, "Param_Name='For All Location'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtAllLocation.Rows.Count > 0)
            {
                strForAllLocation = dtAllLocation.Rows[0]["Param_Value"].ToString();
            }
            else
            {
                strForAllLocation = "False";
            }
            //Update On 17-04-2015 For Employee Code
            strForEmpCode = objAppParam.GetApplicationParameterValueByParamName("EmployeeCode", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (strForEmpCode == "True")
            {
                txtEmployeeCode.Text = objAtt.GetNewEmpCode();
                if (txtEmployeeCode.Text == "")
                {
                    txtEmployeeCode.Text = "1001";
                }
                txtEmployeeCode.Enabled = false;
            }
            else if (strForEmpCode == "False")
            {
                txtEmployeeCode.Text = "";
                txtEmployeeCode.Enabled = true;
            }
            // Modified By Nitin jain On 20/11/2014 Get Salary Increment Parameter For Fresher 
            try
            {
                Fr_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            catch
            {

            }

            // Experienced 
            try
            {
                Exp_IncrementDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            catch
            {

            }

            //.................................................................................
            if (Session["lang"] == null)
            {
                Session["lang"] = "1";
            }

            try
            {
                if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
                {
                    txtIndemnityYear.Enabled = true;
                    txtIndemnityDays.Enabled = true;
                }
                else
                {
                    txtIndemnityYear.Enabled = false;
                    txtIndemnityDays.Enabled = false;
                }
            }
            catch
            {
                txtIndemnityYear.Enabled = false;
                txtIndemnityDays.Enabled = false;
            }

            txtIndemnityYear.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            txtIndemnityDays.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            //Session["dtArcaWing"] = null;
            //BindDocumentList();
            ViewState["CurrIndex"] = 0;
            ViewState["SubSize"] = 9;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            Session["dtLeave"] = null;
            lnkPrev.Visible = false;
            lnkNext.Visible = false;
            lnkFirst.Visible = false;
            lnkLast.Visible = false;
            imbBtnGrid.Visible = true;
            //chkYearCarry.Visible = false;
            SetHalfDay();
            rbtnEmployee.Checked = true;
            imgBtnDatalist.Visible = false;
            ViewState["CurrIndexbin"] = 0;
            ViewState["SubSizebin"] = 9;
            lnkbinFirst.Visible = false;
            lnkbinPrev.Visible = false;
            lnkbinNext.Visible = false;
            lnkbinLast.Visible = false;
            pnlMap.Visible = false;
            pnlshowdata.Visible = false;
            FillLeaveTypeDDL();
            FillDesignationDDL();
            FillQualificationDDL();
            FillReligionDDL();
            FillNationalityDDL();
            FillddlLocationList();
            //FillddlDeaprtmentList();
            ddlLocationList.SelectedValue = Session["LocId"].ToString();
            btnList_Click(null, null);
            FillCurrencyDDL();
            FillCurrency1DDL();
            // FillDataListGrid();
            //FillbinDataListGrid();
            ddlCompany_SelectedIndexChanged(null, null);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            txtValue.Focus();
            bool IsCompOT = false;
            bool IsPartialComp = false;
            try
            {
                IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            }
            catch
            {
            }
            if (IsCompOT)
            {
                rbtnOTEnable.Checked = true;
                rbtnOTDisable.Checked = false;
                rbtOT_OnCheckedChanged(null, null);
            }
            else
            {
                rbtnOTEnable.Checked = false;
                rbtnOTDisable.Checked = true;
                rbtOT_OnCheckedChanged(null, null);
                rbtnOTEnable.Enabled = false;
                rbtnOTDisable.Enabled = false;
            }
            if (IsPartialComp)
            {
                rbtnPartialEnable.Checked = true;
                rbtnPartialDisable.Checked = false;
                rbtPartial_OnCheckedChanged(null, null);
            }
            else
            {
                rbtnPartialEnable.Checked = false;
                rbtnPartialDisable.Checked = true;
                rbtPartial_OnCheckedChanged(null, null);
                rbtnPartialEnable.Enabled = false;
                rbtnPartialDisable.Enabled = false;
            }
            FillteamLeader();
            //Calender.Format = objSys.SetDateFormat();
            // Indemnity Parameter................................
            //....................................................
            FillCountryCode();
            Page.Title = objSys.GetSysTitle();
            CalendarExtender2.Format = objSys.SetDateFormat();
            CalendarExtender1.Format = objSys.SetDateFormat();
            txtTermDate_CalendarExtender.Format = objSys.SetDateFormat();
            FillddlRole();
            setListLocationValue();
            // AllPageCode();
            //Calender.Format = objSys.SetDateFormat();
            Session["CHECKED_ITEMS_Assign"] = null;
            imgBtnDatalist.Visible = true;
            imbBtnGrid.Visible = false;
            imbBtnGrid_Click(null, null);
            //imgBtnbinDatalist.Visible = true;
            //imgBtnbinGrid.Visible = false;
            //btnTerDel.ToolTip = "Deleted";
            //imgBtnbinGrid_Click(null, null);
            FilldeviceGroup();
            GetDocumentNumber();
            rbtnupdateoption.Checked = true;
            rbtnReportoption.Checked = false;
            getPageControlsVisibility();
            FillGrade();
            //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            //{
            //    File_Manager_Employee.Visible = false;
            //}
            setModuleWiseControls(); //set tab visibility as of application

            RootFolder();
            try
            {
                string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }
            catch (Exception ex)
            {

            }
        }
    }
    public void FillGrade()
    {
        DataTable dt = objDa.return_DataTable("select rtrim( ltrim(Grade_Name)) as Grade_Name  from set_grademaster");
        ddlGrade.DataSource = dt;
        ddlGrade.DataTextField = "Grade_Name";
        ddlGrade.DataValueField = "Grade_Name";
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, "--Select--");
    }
    public void FilldeviceGroup()
    {
        DataTable dt = ObjdeviceGroup.GetHeaderAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        objPageCmn.FillData((DropDownList)ddldeviceGroup, dt, "Group_Name", "Group_Id");
        dt.Dispose();
    }
    private void GetCustomersPageWise(int pageIndex, DataTable dtEmp)
    {
        //DataTable dtEmp = Session["dtEmp"] as DataTable;
        if (dtEmp.Rows.Count > 0)
        {
            DataTable dtPage = dtEmp.Rows.Cast<System.Data.DataRow>().Skip((pageIndex - 1) * PageSize).Take(PageSize).CopyToDataTable();
            rptCustomers.DataSource = dtPage;
            rptCustomers.DataBind();
            int recordCount = Convert.ToInt32(dtEmp.Rows.Count);
            this.PopulatePager(recordCount, pageIndex);
        }
        else
        {
            rptCustomers.DataSource = dtEmp;
            rptCustomers.DataBind();
            div_Paging.Visible = false;
        }
    }
    private void PopulatePager(int recordCount, int currentPage)
    {
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(PageSize));
        int pageCount = (int)Math.Ceiling(dblPageCount);
        List<ListItem> pages = new List<ListItem>();
        if (pageCount > 0)
        {
            Session["Pageindex"] = pageCount;
            if (pageCount == 1)
            {
                Lbl_Page_Index.Text = "1";
                Btn_First.Enabled = false;
                Btn_Previous.Enabled = false;
                btn_Next.Enabled = false;
                Btn_Last.Enabled = false;
            }
            else
            {
                Lbl_Page_Index.Text = "1";
                Btn_First.Enabled = true;
                Btn_Previous.Enabled = true;
                btn_Next.Enabled = true;
                Btn_Last.Enabled = true;
            }
        }
    }
    protected void Btn_First_Click(object sender, EventArgs e)
    {
        this.GetCustomersPageWise(1, Session["dtEmp"] as DataTable);
        Lbl_Page_Index.Text = "1";
        Btn_First.Enabled = false;
        Btn_Previous.Enabled = false;
        btn_Next.Enabled = true;
        Btn_Last.Enabled = true;
    }
    protected void Btn_Previous_Click(object sender, EventArgs e)
    {
        if (Lbl_Page_Index.Text != "1")
        {
            int pageIndex = Convert.ToInt32(Lbl_Page_Index.Text) - 1;
            this.GetCustomersPageWise(pageIndex, Session["dtEmp"] as DataTable);
            Lbl_Page_Index.Text = pageIndex.ToString();
            if (pageIndex == 1)
            {
                Btn_First.Enabled = false;
                Btn_Previous.Enabled = false;
                btn_Next.Enabled = true;
                Btn_Last.Enabled = true;
            }
            else
            {
                Btn_First.Enabled = true;
                Btn_Previous.Enabled = true;
                btn_Next.Enabled = true;
                Btn_Last.Enabled = true;
            }
        }
    }
    protected void btn_Next_Click(object sender, EventArgs e)
    {
        if (Lbl_Page_Index.Text != Session["Pageindex"].ToString())
        {
            int pageIndex = Convert.ToInt32(Lbl_Page_Index.Text) + 1;
            this.GetCustomersPageWise(pageIndex, Session["dtEmp"] as DataTable);
            Lbl_Page_Index.Text = pageIndex.ToString();
            if (pageIndex == Convert.ToInt32(Session["Pageindex"].ToString()))
            {
                Btn_First.Enabled = true;
                Btn_Previous.Enabled = true;
                btn_Next.Enabled = false;
                Btn_Last.Enabled = false;
            }
            else
            {
                Btn_First.Enabled = true;
                Btn_Previous.Enabled = true;
                btn_Next.Enabled = true;
                Btn_Last.Enabled = true;
            }
        }
    }
    protected void Btn_Last_Click(object sender, EventArgs e)
    {
        this.GetCustomersPageWise(Convert.ToInt32(Session["Pageindex"].ToString()), Session["dtEmp"] as DataTable);
        Lbl_Page_Index.Text = Session["Pageindex"].ToString();
        Btn_First.Enabled = true;
        Btn_Previous.Enabled = true;
        btn_Next.Enabled = false;
        Btn_Last.Enabled = false;
    }
    public DataTable GetEmployeeFilteredRecord(DropDownList ddlLocationFilter, DropDownList ddlDepartmentFilter)
    {
        string strDepId = string.Empty;
        string strLocationId = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
        string strLocationIDs = string.Empty;
        if (ddlDepartmentFilter.SelectedIndex == -1)
        {
            dtEmp = new DataView(dtEmp, "Department_id=-1", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (Session["EmpId"].ToString() == "0")
        {
            if (ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex == 0)
            {
                //dtEmp = new DataView(dtEmp, "Department_id=-1", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Department_Id =" + ddlDepartmentFilter.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationFilter.SelectedIndex > 0 && ddlDepartmentFilter.SelectedIndex == 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocationFilter.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationFilter.SelectedIndex > 0 && ddlDepartmentFilter.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocationFilter.SelectedValue + " and Department_id='" + ddlDepartmentFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else
        {
            if ((ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex == 0) || (ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex > 0))
            {
                strLocationIDs = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                strLocationIDs = strLocationIDs.Substring(0, strLocationIDs.Length - 1);
                //----------Code to get location's Department-------------------------
                string strWhereClause = string.Empty;
                string strSql = "Select record_id,Field1 as location_id  From Set_UserDataPermission   where    Set_UserDataPermission.Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and  Set_UserDataPermission.User_Id =" + HttpContext.Current.Session["UserId"].ToString() + " and Set_UserDataPermission.IsActive='True' and Record_Type='D'";
                DataTable dtDepartment = objDa.return_DataTable(strSql);
                if (ddlDepartmentFilter.SelectedIndex > 0)
                {
                    dtDepartment = new DataView(dtDepartment, "record_id='" + ddlDepartmentFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                string[] LocationArray = strLocationIDs.Split(',');
                string StrDepIDs = string.Empty;
                foreach (string strLoc in LocationArray)
                {
                    StrDepIDs = "";
                    DataTable dtDepartmentTemp = new DataView(dtDepartment, "location_id='" + strLoc + "'", "", DataViewRowState.CurrentRows).ToTable();
                    for (int i = 0; i < dtDepartmentTemp.Rows.Count; i++)
                    {
                        StrDepIDs += dtDepartmentTemp.Rows[i]["record_id"].ToString() + ",";
                    }
                    if (StrDepIDs != "")
                    {
                        StrDepIDs = StrDepIDs.Substring(0, StrDepIDs.Length - 1);
                        if (strWhereClause == string.Empty)
                        {
                            strWhereClause = "(Location_id='" + strLoc + "' and department_id in (" + StrDepIDs + "))";
                        }
                        else
                        {
                            strWhereClause = strWhereClause + " or " + "(Location_id='" + strLoc + "' and department_id in (" + StrDepIDs + "))";
                        }
                    }
                }
                if (strWhereClause != string.Empty)
                {
                    dtEmp = new DataView(dtEmp, strWhereClause, "", DataViewRowState.CurrentRows).ToTable();
                }
                //-------------end------------------------------------
            }
            //else if (ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex > 0)
            //{
            //}
            else if (ddlLocationFilter.SelectedIndex > 0 && ddlDepartmentFilter.SelectedIndex == 0)
            {
                strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocationFilter.SelectedValue, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDepId == "")
                {
                    strDepId = "0,";
                }
                dtEmp = new DataView(dtEmp, "Location_id=" + ddlLocationFilter.SelectedValue + " and Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationFilter.SelectedIndex > 0 && ddlDepartmentFilter.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocationFilter.SelectedValue + " and Department_id='" + ddlDepartmentFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        return dtEmp;
    }
    public DataTable GetEmployeeFilteredRecord_ForLeave(DropDownList ddlLocationFilter, DropDownList ddlDepartmentFilter)
    {
        string strDepId = string.Empty;
        string strLocationId = string.Empty;
        DataTable dtEmp = new DataTable();
        DataTable dtTemp = new DataTable();
        DataTable dtTemp1 = new DataTable();
        DataTable dtemp1 = new DataTable();
        if (ddlLocationFilter.SelectedIndex > 0)
        {
            dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + ddlLocationFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            strLocationId = ddlLocationFilter.SelectedValue;
            if (Session["EmpId"].ToString() != "0")
            {
                strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocationId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDepId == "")
                {
                    strDepId = "0,";
                }
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else
        {
            dtTemp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());
            dtTemp = new DataView(dtTemp, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["EmpId"].ToString() != "0")
            {
                for (int j = 1; j < ddlLocationLeave.Items.Count; j++)
                {
                    if (strLocationId == "")
                    {
                        strLocationId = ddlLocationLeave.Items[j].Value;
                    }
                    else
                    {
                        strLocationId = strLocationId + "," + ddlLocationLeave.Items[j].Value;
                    }
                }
                dtTemp1 = dtTemp.DefaultView.ToTable(true, "Location_Id");
                dtTemp1 = new DataView(dtTemp1, "Location_Id in (" + strLocationId + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtTemp1.Rows.Count; i++)
                {
                    strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", dtTemp1.Rows[i]["Location_Id"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                    if (strDepId == "")
                    {
                        strDepId = "0,";
                    }
                    dtemp1 = new DataView(dtTemp, "Location_Id=" + dtTemp1.Rows[i]["Location_Id"].ToString() + " and  Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                    dtEmp.Merge(dtemp1);
                }
            }
            else
            {
                dtEmp = dtTemp;
            }
        }
        if (ddlDepartmentFilter.SelectedIndex > 0)
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + ddlDepartmentFilter.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        return dtEmp;
    }
    public void setListLocationValue()
    {
        string strLocationvalue = ddlLocationList.SelectedValue;
        ddlLocationLeave.SelectedValue = strLocationvalue;
        ddlLocationAlert.SelectedValue = strLocationvalue;
        ddlLocationSalary.SelectedValue = strLocationvalue;
        ddlLocationPenalty.SelectedValue = strLocationvalue;
        ddlLocationOTPL.SelectedValue = strLocationvalue;
        ddlLocationList_Bin.SelectedValue = strLocationvalue;
        FillDepartment(ddlLocationList, ddlDeptList);
        FillDepartment(ddlLocationList_Bin, ddlDeptList_Bin);
        FillDepartment(ddlLocationLeave, ddlDepartmentLeave);
        FillDepartment(ddlLocationAlert, ddlDeptAlert);
        FillDepartment(ddlLocationOTPL, ddlDepartmentOTPL);
        FillDepartment(ddlLocationPenalty, ddlDeptPenalty);
        FillDepartment(ddlLocationSalary, ddlDeptSalary);
    }
    public void FillddlRole()
    {
        DataTable dt = new DataTable();
        dt = objRole.GetRoleMaster(Session["CompId"].ToString());
        objPageCmn.FillData((object)ddlRole, dt, "Role_Name", "Role_Id");
        dt.Dispose();
    }
    public void FillteamLeader()
    {
        DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and IsActive='True' ", "Emp_Name", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)ddlTeamLeader, dt, "TlName", "Emp_Id");
    }
    #region CountryCallingCode
    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
            //ddlCountry.SelectedValue = ViewState["Country_Id"].ToString();
            ViewState["CountryCode"] = ObjSysCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        }
        catch
        {
        }
        DataTable dt = ObjSysCountryMaster.getCountryCallingCode();
        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlCountryCode, dt, "CountryCodeName", "CountryCodeValue");
            if (ViewState["CountryCode"] != null)
            {
                try
                {
                    ddlCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                }
                catch
                {
                }
            }
        }
    }
    #endregion
    protected void btnHelp_Click(object sender, EventArgs e)
    {
        string url = "../Help.htm";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }
    public void BindNotification()
    {
        //this code is created by the jitendra upadhyay on 18-09-2014 to get the all notification dynamically(from table) and bind using checkbox list control
        //code start
        DataTable dt = objEmpNotice.GetAllNotification_By_NOtificationType("SMS");
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ChkSmsList, dt, "Notification_Name", "Notification_Id");
        }
        dt = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ChkReportList, dt, "Notification_Name", "Notification_Id");
        }
        dt = objEmpNotice.GetAllNotification_By_NOtificationType("Email");
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)chkEmailList, dt, "Notification_Name", "Notification_Id");
        }
        foreach (ListItem item in ChkReportList.Items)
        {
            item.Selected = true;
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("SMS_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            foreach (ListItem item in ChkSmsList.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            ChkSmsList.Enabled = false;
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Email_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            foreach (ListItem item in chkEmailList.Items)
            {
                item.Selected = true;
            }
        }
        else
        {
            chkEmailList.Enabled = false;
        }
        //code end
    }
    # region Filter Criteria According to Location and Department
    private void FillddlLocationList()
    {
        ddlLocationList.Items.Clear();
        DataTable dtLoc = ObjLocMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        objPageCmn.FillData((object)ddlLocationList, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)ddlLocationLeave, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)ddlLocationAlert, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)ddlLocationPenalty, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)ddlLocationSalary, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)ddlLocationOTPL, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)ddlLocationList_Bin, dtLoc, "Location_Name", "Location_Id");

    }
    public void FillDepartment(DropDownList ddlLocation, DropDownList ddlDepartment)
    {
        ddlDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
        }
        dtDepartment = objDep.GetDepartmentMaster();
        DataTable dt = (DataTable)ddlLocation.DataSource;
        string strLocationIDs = ddlLocation.SelectedValue;
        if (ddlLocation.SelectedValue == "--Select--")
        {
            strLocationIDs = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        }
        else
        {
        }
        strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocationIDs, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (strDepId == "")
        {
            strDepId = "0,";
        }
        //if (Session["EmpId"].ToString() != "0")
        //{
        dtDepartment = new DataView(dtDepartment, "Dep_Id in(" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //}
        objPageCmn.FillData((object)ddlDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }
    public void FillDepartment_ForLeave(DropDownList ddlLocation, DropDownList ddlDepartment)
    {
        ddlDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
            dtDepartment = objDep.GetDepartmentMaster();
            strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strDepId == "")
            {
                strDepId = "0,";
            }
            //if (Session["EmpId"].ToString() != "0")
            //{
            dtDepartment = new DataView(dtDepartment, "Dep_Id in(" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            //}
            objPageCmn.FillData((object)ddlDepartment, dtDepartment, "Dep_Name", "Dep_Id");
        }
        else
        {
            ddlDepartmentLeave.Items.Clear();
            ddlDepartmentLeave.Items.Insert(0, "--Select--");
        }
    }
    private void FillddlDeaprtmentList()
    {
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dt.Rows.Count > 0)
        {
            ddlDeptList.DataSource = null;
            ddlDeptList.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDeptList, dt, "DeptName", "Dep_Id");
            objPageCmn.FillData((object)ddlDeptList_Bin, dt, "DeptName", "Dep_Id");
            ddlDepartmentLeave.DataSource = null;
            ddlDepartmentLeave.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDepartmentLeave, dt, "DeptName", "Dep_Id");
            ddlDeptAlert.DataSource = null;
            ddlDeptAlert.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDeptAlert, dt, "DeptName", "Dep_Id");
            ddlDeptSalary.DataSource = null;
            ddlDeptSalary.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDeptSalary, dt, "DeptName", "Dep_Id");
            ddlDepartmentOTPL.DataSource = null;
            ddlDepartmentOTPL.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDepartmentOTPL, dt, "DeptName", "Dep_Id");
            ddlDeptPenalty.DataSource = null;
            ddlDeptPenalty.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDeptPenalty, dt, "DeptName", "Dep_Id");
        }
        else
        {
            try
            {
                ddlDeptList.Items.Insert(0, "--Select--");
                ddlDeptList.SelectedIndex = 0;
                ddlDepartmentLeave.Items.Insert(0, "--Select--");
                ddlDepartmentLeave.SelectedIndex = 0;
                ddlDeptAlert.Items.Insert(0, "--Select--");
                ddlDeptAlert.SelectedIndex = 0;
                ddlDeptSalary.Items.Insert(0, "--Select--");
                ddlDeptSalary.SelectedIndex = 0;
                ddlDepartmentOTPL.Items.Insert(0, "--Select--");
                ddlDepartmentOTPL.SelectedIndex = 0;
                ddlDeptPenalty.Items.Insert(0, "--Select--");
                ddlDeptPenalty.SelectedIndex = 0;
            }
            catch
            {
                ddlDeptList.Items.Insert(0, "--Select--");
                ddlDeptList.SelectedIndex = 0;
                ddlDepartmentLeave.Items.Insert(0, "--Select--");
                ddlDepartmentLeave.SelectedIndex = 0;
                ddlDeptAlert.Items.Insert(0, "--Select--");
                ddlDeptAlert.SelectedIndex = 0;
                ddlDeptSalary.Items.Insert(0, "--Select--");
                ddlDeptSalary.SelectedIndex = 0;
                ddlDepartmentOTPL.Items.Insert(0, "--Select--");
                ddlDepartmentOTPL.SelectedIndex = 0;
                ddlDeptPenalty.Items.Insert(0, "--Select--");
                ddlDeptPenalty.SelectedIndex = 0;
            }
        }
    }
    protected void ddlLocationList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //setListLocationValue();
        FillDepartment(ddlLocationList, ddlDeptList);
        FillDataListGrid();

    }
    protected void ddlLocationList_Bin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        FillDepartment(ddlLocationList_Bin, ddlDeptList_Bin);
        FillbinDataListGrid();
    }
    protected void ddlLocationLeave_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        ddlDepartmentLeave.SelectedIndex = 0;
        DataTable dtEmpLeave = GetEmployeeFilteredRecord_ForLeave(ddlLocationLeave, ddlDepartmentLeave);
        Session["dtEmpLeave"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dtEmpLeave, "", "");
        lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDepartmentLeave.Focus();
        FillDepartment_ForLeave(ddlLocationLeave, ddlDepartmentLeave);
        AllPageCode();
    }
    protected void ddlLocationAlert_SelectedIndexChanged(object sender, EventArgs e)
    {
        I5.Attributes.Add("Class", "fa fa-minus");
        Div6.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationAlert, ddlDeptAlert);
        Session["dtEmpNF"] = dtEmpLeave;
        ////Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpNF, dtEmpLeave, "", "");
        lblTotalRecordNF.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        FillDepartment(ddlLocationAlert, ddlDeptAlert);
        AllPageCode();
    }
    protected void ddlLocationSalary_SelectedIndexChanged(object sender, EventArgs e)
    {
        I6.Attributes.Add("Class", "fa fa-minus");
        Div7.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationSalary, ddlDeptSalary);
        Session["dtEmpSal"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmpLeave, "", "");
        lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDeptSalary.Focus();
        FillDepartment(ddlLocationSalary, ddlDeptSalary);
        AllPageCode();
        if (ddlLocationSalary.SelectedValue.ToString() != "--Select--")
        {
            DataTable dt = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), ddlLocationSalary.SelectedValue.ToString());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Field1"].ToString() != "")
                    ddlCurrency.SelectedValue = dt.Rows[0]["Field1"].ToString();
                else
                    ddlCurrency.SelectedValue = "--Select--";
            }
            else
            {
                ddlCurrency.SelectedValue = "--Select--";
            }
        }
        else
        {
            ddlCurrency.SelectedValue = "--Select--";
        }
    }
    protected void ddlLocationOTPL_SelectedIndexChanged(object sender, EventArgs e)
    {
        I7.Attributes.Add("Class", "fa fa-minus");
        Div8.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationOTPL, ddlDepartmentOTPL);
        Session["dtEmpOT"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpOT, dtEmpLeave, "", "");
        lblTotalRecordOT.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDepartmentOTPL.Focus();
        FillDepartment(ddlLocationOTPL, ddlDepartmentOTPL);
        AllPageCode();
    }
    protected void ddlLocationPenalty_SelectedIndexChanged(object sender, EventArgs e)
    {
        I8.Attributes.Add("Class", "fa fa-minus");
        Div9.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationPenalty, ddlDeptPenalty);
        Session["dtEmpPenalty"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dtEmpLeave, "", "");
        lblTotalRecordPenalty.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDepartmentOTPL.Focus();
        FillDepartment(ddlLocationPenalty, ddlDeptPenalty);
        AllPageCode();
    }
    protected void ddlDeptList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillDataListGrid();
    }
    protected void ddlempTypeFilter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillDataListGrid();
    }
    protected void ddlDeptList_Bin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        FillbinDataListGrid();
    }
    protected void ddlDepartmentLeave_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationLeave, ddlDepartmentLeave);
        Session["dtEmpLeave"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dtEmpLeave, "", "");
        lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        AllPageCode();
    }
    protected void ddlDeptAlert_SelectedIndexChanged(object sender, EventArgs e)
    {
        I5.Attributes.Add("Class", "fa fa-minus");
        Div6.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationAlert, ddlDeptAlert);
        Session["dtEmpNF"] = dtEmpLeave;
        ////Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpNF, dtEmpLeave, "", "");
        lblTotalRecordNF.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        AllPageCode();
        //FillDepartment(ddlLocationAlert, ddlDeptAlert);
    }
    protected void ddlDeptSalary_SelectedIndexChanged(object sender, EventArgs e)
    {
        I6.Attributes.Add("Class", "fa fa-minus");
        Div7.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationSalary, ddlDeptSalary);
        Session["dtEmpSal"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmpLeave, "", "");
        lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDeptSalary.Focus();
        AllPageCode();
    }
    protected void ddlDepartmentOTPL_SelectedIndexChanged(object sender, EventArgs e)
    {
        I7.Attributes.Add("Class", "fa fa-minus");
        Div8.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationOTPL, ddlDepartmentOTPL);
        Session["dtEmpOT"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpOT, dtEmpLeave, "", "");
        lblTotalRecordOT.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDepartmentOTPL.Focus();
        AllPageCode();
    }
    protected void ddlDeptPenalty_SelectedIndexChanged(object sender, EventArgs e)
    {
        I8.Attributes.Add("Class", "fa fa-minus");
        Div9.Attributes.Add("Class", "box box-primary");
        DataTable dtEmpLeave = GetEmployeeFilteredRecord(ddlLocationPenalty, ddlDeptPenalty);
        Session["dtEmpPenalty"] = dtEmpLeave;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dtEmpLeave, "", "");
        lblTotalRecordPenalty.Text = Resources.Attendance.Total_Records + " : " + dtEmpLeave.Rows.Count.ToString() + "";
        ddlDepartmentOTPL.Focus();
        AllPageCode();
    }
    public void FillGrid()
    {
        string strFLocId = string.Empty;
        DataTable dtLoc = ObjLocMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLoc.Rows.Count; i++)
                    {
                        strFLocId += dtLoc.Rows[i]["Location_Id"].ToString() + ",";
                    }
                }
            }
        }

        DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
        if (ddlLocationList.SelectedValue.ToString() == "0" && ddlLocationList.SelectedItem.Text == "All")
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id in(" + strFLocId.Substring(0, strFLocId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlLocationList.SelectedValue.ToString() == "0" && ddlLocationList.SelectedItem.Text == "--Select--")
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id=" + ddlLocationList.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlDeptList.SelectedValue.ToString() != "0")
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + ddlDeptList.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmp, dtEmp, "", "");
            GetCustomersPageWise(1, dtEmp);
            //objPageCmn.FillData((object)dtlistEmp, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmployee, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmpPenalty, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmpOT, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
            objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblTotalRecordOT.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblTotalRecordNF.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblTotalRecordPenalty.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
        }
        AllPageCode();
        dtEmp.Dispose();
    }
    protected void btnAllRefresh_Click(object sender, ImageClickEventArgs e)
    {
        ddlDeptList.SelectedIndex = 0;
        ddlLocationList.SelectedIndex = 0;
        FillGrid();
        AllPageCode();
    }
    protected void imgbtnRefreshAllLeave_Click(object sender, ImageClickEventArgs e)
    {
        ddlDepartmentLeave.SelectedIndex = 0;
        ddlLocationLeave.SelectedIndex = 0;
        FillGrid();
        ddlLocationLeave.Focus();
    }
    protected void btnAllRefreshAlert_Click(object sender, ImageClickEventArgs e)
    {
        ddlDeptAlert.SelectedIndex = 0;
        ddlLocationAlert.SelectedIndex = 0;
        FillGrid();
        AllPageCode();
    }
    protected void btnAllRefreshSalary_Click(object sender, ImageClickEventArgs e)
    {
        ddlDeptSalary.SelectedIndex = 0;
        ddlLocationSalary.SelectedIndex = 0;
        FillGrid();
        AllPageCode();
    }
    protected void btnAllRefreshOTPL_Click(object sender, ImageClickEventArgs e)
    {
        ddlDepartmentOTPL.SelectedIndex = 0;
        ddlLocationOTPL.SelectedIndex = 0;
        FillGrid();
        AllPageCode();
    }
    protected void btnAllRefreshPenalty_Click(object sender, ImageClickEventArgs e)
    {
        ddlDeptPenalty.SelectedIndex = 0;
        ddlLocationPenalty.SelectedIndex = 0;
        FillGrid();
        AllPageCode();
    }
    #endregion
    public void SetHalfDay()
    {
        int halfday = 0;
        try
        {
            halfday = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Half_Day_Count", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            halfday = 0;
        }
        txtHalfDayCount.Text = halfday.ToString();
    }
    //public string GetRoleDataPermission(string RoleId, string RecordType)
    //{
    //    string IDs = string.Empty;
    //    DataTable dtRoleData = objRoleData.GetRoleDataPermissionById(RoleId);
    //    if (dtRoleData.Rows.Count > 0)
    //    {
    //        dtRoleData = new DataView(dtRoleData, "Record_Type='" + RecordType + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        if (dtRoleData.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dtRoleData.Rows.Count; i++)
    //            {
    //                IDs += dtRoleData.Rows[i]["Record_Id"].ToString() + ",";
    //            }
    //        }
    //    }
    //    return IDs;
    //}
    protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtBrand = ObjBrandMaster.GetBrandMaster(Session["CompId"].ToString());
        string BrandIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "B", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (BrandIds != "")
            {
                dtBrand = new DataView(dtBrand, "Brand_Id in(" + BrandIds.Substring(0, BrandIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        fillDropdown(ddlBrand, dtBrand, "Brand_Name", "Brand_Id");
        ddlBrand_SelectedIndexChanged(null, null);
        AllPageCode();
    }
    //public bool GetStatus(string RoleId)
    //{
    //    bool status = false;
    //    DataTable dtRole = objRole.GetRoleMaster();
    //    dtRole = new DataView(dtRole, "Role_Id='" + RoleId + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    if (dtRole.Rows.Count > 0)
    //    {
    //        string str = dtRole.Rows[0]["Role_Name"].ToString().Trim().ToLower();
    //        if (str == "super admin")
    //        {
    //            status = true;
    //        }
    //    }
    //    return status;
    //}
    protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtLoc = ObjLocMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + ddlBrand.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        fillDropdown(ddlLocation, dtLoc, "Location_Name", "Location_Id");
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        ddlLocation_SelectedIndexChanged(null, null);
        //AllPageCode();
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Modified By Nitin Jain On 25-09-2014............
        DataTable dtdept = new DataTable();
        try
        {
            dtdept = objLocDept.GetDepartmentByLocationId(ddlLocation.SelectedValue);
            //
            string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
            {
                if (DepIds != "")
                {
                    dtdept = new DataView(dtdept, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "Dep_Name", DataViewRowState.CurrentRows).ToTable();
                }
            }
            fillDropdown(ddlDepartment, dtdept, "Dep_Name", "Dep_Id");
        }
        catch
        {
        }
        //AllPageCode();
        //.....................................
    }
    public void fillDropdown(DropDownList ddl, DataTable dt, string DataTextField, string DataValueField)
    {
        ddl.DataSource = dt;
        ddl.DataTextField = DataTextField;
        ddl.DataValueField = DataValueField;
        ddl.DataBind();
        //AllPageCode();
    }
    protected void txtEmpName_OnTextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objEmp.GetEmployeeMasterByEmpName(Session["CompId"].ToString().ToString(), txtEmployeeName.Text.Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {
                txtEmployeeName.Text = "";
                DisplayMessage("Employee Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                return;
            }
            DataTable dt1 = objEmp.GetEmployeeMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Emp_Name='" + txtEmployeeName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtEmployeeName.Text = "";
                DisplayMessage("Employee Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                return;
            }
            txtEmployeeL.Focus();
        }
        else
        {
            DataTable dtTemp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Emp_Name"].ToString() != txtEmployeeName.Text.Split('/')[0].ToString())
                {
                    DataTable dt = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Emp_Name='" + txtEmployeeName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtEmployeeName.Text = "";
                        DisplayMessage("Employee Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                        return;
                    }
                    DataTable dt1 = objEmp.GetEmployeeMasterInactive(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Emp_Name='" + txtEmployeeName.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtEmployeeName.Text = "";
                        DisplayMessage("Employee Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEmployeeName);
                        return;
                    }
                }
            }
            txtEmployeeL.Focus();
        }
        AllPageCode();
    }
    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();
        DataTable dtComp = new DataTable();
        dtComp = objComp.GetCompanyMasterById(Session["CompId"].ToString());
        string Currency_Id = dtComp.Rows[0]["Currency_Id"].ToString();
        if (dt.Rows.Count > 0)
        {
            ddlCurrency.DataSource = null;
            ddlCurrency.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            ddlCurrency.SelectedValue = Currency_Id;
            ddlCurrency.SelectedValue = ObjLocMaster.Get_Currency_By_Location_ID(Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Currency_Id"].ToString();
        }
        else
        {
            try
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
            catch
            {
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
        }
        ddlWorkCalMethod.SelectedValue = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
    }
    public void FillCurrency1DDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();
        if (dt.Rows.Count > 0)
        {
            ddlCurrency1.DataSource = null;
            ddlCurrency1.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlCurrency1, dt, "Currency_Name", "Currency_Id");
            ddlCurrency1.SelectedValue = ObjLocMaster.Get_Currency_By_Location_ID(Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Currency_Id"].ToString();
        }
        else
        {
            try
            {
                ddlCurrency1.Items.Insert(0, "--Select--");
                ddlCurrency1.SelectedIndex = 0;
            }
            catch
            {
                ddlCurrency1.Items.Insert(0, "--Select--");
                ddlCurrency1.SelectedIndex = 0;
            }
        }
    }
    public void AllPageCode(string strCallFrom = "")
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = new DataTable();
        if (Session["dtModule_15"] == null)
        {
            Session["dtModule_15"] = objObjectEntry.GetModuleIdAndName("15", (DataTable)Session["ModuleName"]);
        }
        dtModule = (DataTable)Session["dtModule_15"];
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            btnExport.Visible = true;
            btnEmpexport.Visible = true;
            btnSave.Visible = true;
            btnSaveHalfday.Visible = true;
            btnSaveLeave.Visible = true;
            Btn_Alert_Save.Visible = true;
            btnSaveSal.Visible = true;
            btnSaveOT.Visible = true;
            btnSavePenalty.Visible = true;
            btnConnect.Visible = true;
            hdnCanEdit.Value = "true";
            hdnCanDelete.Value = "true";
            hdnCanUpload.Value = "true";
            btnviewcolumns.Visible = true;
            imgAddAddressName.Visible = true;
            btnAddNewAddress.Visible = true;
            HyperLink1.Visible = true;
            HyperLink2.Visible = true;
            for (int i = 0; i < dtlistbinEmp.Items.Count; i++)
            {
                ((CheckBox)dtlistbinEmp.Items[i].FindControl("chbAcInctive")).Visible = true;
            }
            for (int i = 0; i < rptCustomers.Items.Count; i++)
            {
                ((LinkButton)rptCustomers.Items[i].FindControl("lnkEmpname")).Enabled = true;
            }
            imgBtnRestore.Visible = true;
            //  Li_Upload.Visible = true;
            IbtnDownloadterminated.Visible = true;
            btnControlsSetting.Visible = true;
            btnGvListSetting.Visible = true;
        }
        Common cmn = new Common(Session["DBConnection"].ToString());
        DataTable dtAllPageCode = new DataTable();
        if (Session["dtAllPageCode_15"] == null)
        {
            Session["dtAllPageCode_15"] = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "15", HttpContext.Current.Session["CompId"].ToString());
        }
        //dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "15");
        dtAllPageCode = (DataTable)Session["dtAllPageCode_15"];
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {
            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "1")
                    {
                        btnSave.Visible = true;
                        btnSaveHalfday.Visible = true;
                        btnSaveLeave.Visible = true;
                        Btn_Alert_Save.Visible = true;
                        btnSaveSal.Visible = true;
                        btnSaveOT.Visible = true;
                        btnSavePenalty.Visible = true;
                        btnConnect.Visible = true;
                        btnviewcolumns.Visible = true;
                        imgAddAddressName.Visible = true;
                        btnAddNewAddress.Visible = true;
                    }

                    if (DtRow["Op_Id"].ToString() == "2")
                    {
                        hdnCanEdit.Value = "true";
                    }
                    if (DtRow["Op_Id"].ToString() == "3")
                    {
                        hdnCanDelete.Value = "true";
                    }
                    if (DtRow["Op_Id"].ToString() == "8")
                    {
                        hdnCanUpload.Value = "true";
                    }
                    if (DtRow["Op_Id"].ToString() == "2")
                    {
                        for (int i = 0; i < rptCustomers.Items.Count; i++)
                        {
                            ((LinkButton)rptCustomers.Items[i].FindControl("lnkEmpname")).Enabled = true;
                        }
                    }
                    if (DtRow["Op_Id"].ToString() == "4")
                    {
                        for (int i = 0; i < dtlistbinEmp.Items.Count; i++)
                        {
                            ((CheckBox)dtlistbinEmp.Items[i].FindControl("chbAcInctive")).Visible = true;
                        }
                        imgBtnRestore.Visible = true;
                    }
                    if (DtRow["Op_Id"].ToString() == "5")
                    {
                        gvEmpLeave.Columns[0].Visible = true;
                    }
                    if (DtRow["Op_Id"].ToString() == "7")
                    {
                        btnExport.Visible = true;
                        btnEmpexport.Visible = true;
                        HyperLink1.Visible = true;
                        HyperLink2.Visible = true;
                        IbtnDownloadterminated.Visible = true;
                    }
                    if (DtRow["Op_Id"].ToString() == "8")
                    {
                        //   Li_Upload.Visible = true;
                    }
                    if (DtRow["Op_Id"].ToString() == "23") //can change controls attribute
                    {
                        btnControlsSetting.Visible = true;
                        btnGvListSetting.Visible = true;
                    }
                }
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeCode(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterAllData(HttpContext.Current.Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "Emp_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
        }
        return txt;
    }
    public void FillDepDDL()
    {
        DataTable dt = objDep.GetDepartmentMaster();
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlDepartment.DataSource = null;
            ddlDepartment.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDepartment, dt, "Dep_Name", "Dep_Id");
        }
        else
        {
            try
            {
                ddlDepartment.Items.Insert(0, "--Select--");
                ddlDepartment.SelectedIndex = 0;
            }
            catch
            {
                ddlDepartment.Items.Insert(0, "--Select--");
                ddlDepartment.SelectedIndex = 0;
            }
        }
    }
    public void FillLeaveTypeDDL()
    {
        DataTable dt = objLeaveType.GetLeaveMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlLeaveType.DataSource = null;
            ddlLeaveType.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlLeaveType, dt, "Leave_Name", "Leave_Id");
        }
        else
        {
            ddlLeaveType.Items.Insert(0, "--Select--");
            ddlLeaveType.SelectedIndex = 0;
        }
    }
    public void FillReligionDDL()
    {
        DataTable dt = objRel.GetReligionMaster();
        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlReligion.DataSource = null;
            ddlReligion.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlReligion, dt, "Religion", "Religion_Id");
        }
        else
        {
            try
            {
                ddlReligion.Items.Insert(0, "--Select--");
                ddlReligion.SelectedIndex = 0;
            }
            catch
            {
                ddlReligion.Items.Insert(0, "--Select--");
                ddlReligion.SelectedIndex = 0;
            }
        }
    }
    public void FillNationalityDDL()
    {
        DataTable dt = objNat.GetNationalityMaster();
        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlNationality.DataSource = null;
            ddlNationality.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlNationality, dt, "Nationality", "Nationality_Id");
        }
        else
        {
            try
            {
                ddlNationality.Items.Insert(0, "--Select--");
                ddlNationality.SelectedIndex = 0;
            }
            catch
            {
                ddlNationality.Items.Insert(0, "--Select--");
                ddlNationality.SelectedIndex = 0;
            }
        }
    }
    public void FillDesignationDDL()
    {
        DataTable dt = objDesg.GetDesignationMaster();
        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlDesignation.DataSource = null;
            ddlDesignation.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlDesignation, dt, "Designation", "Designation_Id");
        }
        else
        {
            try
            {
                ddlDesignation.Items.Insert(0, "--Select--");
                ddlDesignation.SelectedIndex = 0;
            }
            catch
            {
                ddlDesignation.Items.Insert(0, "--Select--");
                ddlDesignation.SelectedIndex = 0;
            }
        }
    }
    public void FillQualificationDDL()
    {
        DataTable dt = objQualif.GetQualificationMaster();
        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            ddlQualification.DataSource = null;
            ddlQualification.DataBind();
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlQualification, dt, "Qualification", "Qualification_Id");
        }
        else
        {
            try
            {
                ddlQualification.Items.Insert(0, "--Select--");
                ddlQualification.SelectedIndex = 0;
            }
            catch
            {
                ddlQualification.Items.Insert(0, "--Select--");
                ddlQualification.SelectedIndex = 0;
            }
        }
    }
    public bool checkDatalist(object empid)
    {
        string chkdata = string.Empty;
        DataTable dtemp = new DataTable();
        string strsql = "select IsActive from Set_EmployeeMaster where Emp_Id=" + empid + "";
        dtemp = objDa.return_DataTable(strsql);
        if (dtemp.Rows.Count > 0)
        {
            chkdata = dtemp.Rows[0]["IsActive"].ToString();
        }
        return Convert.ToBoolean(chkdata);
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (txtValue.Text.Trim().ToString() == "")
        {
            txtValue.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        DataTable dtEmployee = (DataTable)Session["dtEmp"];
        if (dtEmployee.Rows.Count > 0)
        {
            lnkNext.Visible = true;
            lnkLast.Visible = true;
        }
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            DataView view = new DataView(dtEmployee, condition, "", DataViewRowState.CurrentRows);
            lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            // Session["dtEmp"] = view.ToTable();
            dtEmployee = view.ToTable();
            if (dtEmployee.Rows.Count <= 9)
            {
                btnRefresh.Focus();
                //Common Function add By Lokesh on 14-05-2015
                //objPageCmn.FillData((object)dtlistEmp, dtEmployee, "", "");
                GetCustomersPageWise(1, dtEmployee);
                objPageCmn.FillData((object)gvEmp, dtEmployee, "", "");
                lnkPrev.Visible = false;
                lnkFirst.Visible = false;
                lnkNext.Visible = false;
                lnkLast.Visible = false;
            }
            else
            {
                FillDataList(dtEmployee, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                objPageCmn.FillData((object)gvEmp, dtEmployee, "", "");
            }
        }
        AllPageCode();
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        lnkNext.Visible = true;
        lnkLast.Visible = true;
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        FillDataListGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        txtValue.Text = "";
        btngo_Click(null, null);
        ddlFieldName.Focus();
        AllPageCode();

    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        lnkFirst.Visible = false;
        lnkPrev.Visible = false;
        lnkNext.Visible = true;
        lnkLast.Visible = true;
        FillDataListGrid();
    }
    protected void imbBtnGrid_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        lnkNext.Visible = false;
        lnkLast.Visible = false;
        lnkFirst.Visible = false;
        lnkPrev.Visible = false;
        // dtlistEmp.Visible = false;
        rptCustomers.Visible = false;
        div_Paging.Visible = false;
        gvEmp.Visible = true;
        FillDataListGrid();
        imgBtnDatalist.Visible = true;
        //Img_Emp_List_Select_All.Visible = true;
        //Img_Emp_List_Delete_All.Visible = true;
        imbBtnGrid.Visible = false;
        txtValue.Focus();
        AllPageCode();
    }
    protected void imgBtnDatalist_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        lnkNext.Visible = true;
        lnkLast.Visible = true;
        rptCustomers.Visible = true;
        div_Paging.Visible = true;
        //dtlistEmp.Visible = true;
        gvEmp.Visible = false;
        FillDataListGrid();
        imgBtnDatalist.Visible = false;
        //Img_Emp_List_Select_All.Visible = false;
        //Img_Emp_List_Delete_All.Visible = false;
        imbBtnGrid.Visible = true;
        txtValue.Focus();
        AllPageCode();
    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        FillDataListGrid();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        AllPageCode();
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        lnkPrev.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = true;
        lnkNext.Visible = true;
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        DataTable dt = (DataTable)Session["dtEmp"];
        FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        ViewState["SubSize"] = 9;
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSize"].ToString());
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        ViewState["SubSize"] = 9;
        DataTable dt = (DataTable)Session["dtEmp"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSize"].ToString());
        ViewState["CurrIndex"] = index;
        int tot = dt.Rows.Count;
        if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) > 0)
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
        else if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) == 0)
        {
            FillDataList(dt, index - 1, Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
        else
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = true;
        lnkFirst.Visible = true;
    }
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtEmp"];
        ViewState["SubSize"] = 9;
        ViewState["CurrIndex"] = Convert.ToInt32(ViewState["CurrIndex"].ToString()) - 1;
        if (Convert.ToInt32(ViewState["CurrIndex"].ToString()) < 0)
        {
            ViewState["CurrIndex"] = 0;
        }
        if (Convert.ToInt16(ViewState["CurrIndex"]) == 0)
        {
            lnkFirst.Visible = false;
            lnkPrev.Visible = false;
            lnkNext.Visible = true;
            lnkLast.Visible = true;
        }
        else
        {
            lnkFirst.Visible = true;
            lnkLast.Visible = true;
            lnkNext.Visible = true;
        }
        FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        ViewState["SubSize"] = 9;
        DataTable dt = (DataTable)Session["dtEmp"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSize"].ToString());
        int k1 = Convert.ToInt32(ViewState["CurrIndex"].ToString());
        ViewState["CurrIndex"] = Convert.ToInt32(ViewState["CurrIndex"].ToString()) + 1;
        int k = Convert.ToInt32(ViewState["CurrIndex"].ToString());
        if (Convert.ToInt32(ViewState["CurrIndex"].ToString()) >= index)
        {
            ViewState["CurrIndex"] = index;
            lnkNext.Visible = false;
            lnkLast.Visible = false;
        }
        int tot = dt.Rows.Count;
        if (k == index)
        {
            if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) > 0)
            {
                FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                lnkPrev.Visible = true;
                lnkFirst.Visible = true;
            }
            else
            {
                lnkPrev.Visible = true;
                lnkFirst.Visible = true;
                lnkLast.Visible = true;
                lnkNext.Visible = true;
            }
        }
        else if (k < index)
        {
            if (k + 1 == index)
            {
                if (tot % Convert.ToInt32(ViewState["SubSize"].ToString()) > 0)
                {
                    FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                    lnkPrev.Visible = true;
                    lnkFirst.Visible = true;
                }
                else
                {
                    FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                    lnkNext.Visible = false;
                    lnkLast.Visible = false;
                    lnkPrev.Visible = true;
                    lnkFirst.Visible = true;
                }
            }
            else
            {
                FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
                lnkPrev.Visible = true;
                lnkFirst.Visible = true;
            }
        }
        else
        {
            FillDataList(dt, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
        }
    }
    protected void rbtOT1_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnOTEnable1.Checked)
        {
            ddlOTCalc1.Enabled = true;
            txtNormal1.Enabled = true;
            txtWeekOffValue1.Enabled = true;
            txtHolidayValue1.Enabled = true;
            ddlNormalType1.Enabled = true;
            ddlWeekOffType1.Enabled = true;
            ddlHolidayValue1.Enabled = true;
        }
        else
        {
            ddlOTCalc1.Enabled = false;
            txtNormal1.Enabled = false;
            txtWeekOffValue1.Enabled = false;
            txtHolidayValue1.Enabled = false;
            ddlNormalType1.Enabled = false;
            ddlWeekOffType1.Enabled = false;
            ddlHolidayValue1.Enabled = false;
        }
    }
    protected void rbtOT_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnOTEnable.Checked)
        {
            ddlOTCalc.Enabled = true;
            txtNoralType.Enabled = true;
            txtWeekOffValue.Enabled = true;
            txtHolidayValue.Enabled = true;
            ddlNormalType.Enabled = true;
            ddlWeekOffType.Enabled = true;
            ddlHolidayType.Enabled = true;
        }
        else
        {
            ddlOTCalc.Enabled = false;
            txtNoralType.Enabled = false;
            txtWeekOffValue.Enabled = false;
            txtHolidayValue.Enabled = false;
            ddlNormalType.Enabled = false;
            ddlWeekOffType.Enabled = false;
            ddlHolidayType.Enabled = false;
        }
    }
    protected void rbtPartial_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPartialEnable.Checked)
        {
            rbtnCarryYes.Enabled = true;
            rbtnCarryNo.Enabled = true;
            txtTotalMinutes.Enabled = true;
            txtMinuteday.Enabled = true;
        }
        else
        {
            rbtnCarryYes.Enabled = false;
            rbtnCarryNo.Enabled = false;
            txtTotalMinutes.Enabled = false;
            txtMinuteday.Enabled = false;
        }
    }
    protected void rbtPartial1_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPartialEnable1.Checked)
        {
            rbtnCarryYes1.Enabled = true;
            rbtnCarryNo1.Enabled = true;
            txtTotalMinutesP1.Enabled = true;
            txtMinuteOTOne.Enabled = true;
        }
        else
        {
            rbtnCarryYes1.Enabled = false;
            rbtnCarryNo1.Enabled = false;
            txtTotalMinutesP1.Enabled = false;
            txtMinuteOTOne.Enabled = false;
        }
        AllPageCode();
    }
    protected void gvEmp1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployee.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmployee, (DataTable)Session["dtEmp1"], "", "");
        AllPageCode();
    }
    protected void gvEmployeeNF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeNF.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmployeeNF, (DataTable)Session["dtEmp2"], "", "");
        AllPageCode();
    }
    protected void gvEmployeePenalty_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeePenalty.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmployeePenalty, (DataTable)Session["dtEmp10"], "", "");
        AllPageCode();
    }
    protected void gvEmployeeOT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeOT.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmployeeOT, (DataTable)Session["dtEmp5"], "", "");
        AllPageCode();
    }
    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmployeeSal, (DataTable)Session["dtEmp4"], "", "");
        AllPageCode();
    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmp.PageIndex = e.NewPageIndex;
        FillDataListGrid();
        AllPageCode();
    }
    protected void lnkEditCommand(object sender, CommandEventArgs e)
    {
        Lbl_Tab_Name.Text = Resources.Attendance.Edit;
        editid.Value = e.CommandArgument.ToString();
        Session["empimgpath"] = "";
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), editid.Value);
        if (dtEmp.Rows.Count > 0)
        {
            btnNew_Click(null, null);
            btnNew.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            ddlCompany_SelectedIndexChanged(null, null);
            DataTable dtEmpSal = objEmpParam.GetEmployeeParameterByEmpId(editid.Value, Session["CompId"].ToString());
            if (dtEmpSal.Rows.Count > 0)
            {
                try
                {
                    ddlCategoryNewTab.SelectedValue = dtEmpSal.Rows[0]["Field11"].ToString();
                }
                catch
                {
                }
            }
            txtCompanyMobileNo.Text = dtEmp.Rows[0]["Company_phone_no"].ToString();
            Hdn_Edit.Value = dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeCode.Text = dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeName.Text = dtEmp.Rows[0]["Emp_Name"].ToString() + "/" + dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeL.Text = dtEmp.Rows[0]["Emp_Name_L"].ToString();
            //CreateDirectory(txtEmployeeCode.Text, editid.Value);
            txtCivilId.Text = dtEmp.Rows[0]["Civil_Id"].ToString();
            txtEmailId.Text = dtEmp.Rows[0]["Email_Id"].ToString();
            try
            {
                ddlTeamLeader.SelectedValue = dtEmp.Rows[0]["Field5"].ToString();
            }
            catch
            {
                ddlTeamLeader.SelectedIndex = 0;
            }
            try
            {
                ddldeviceGroup.SelectedValue = dtEmp.Rows[0]["Device_Group_Id"].ToString();
            }
            catch
            {
                ddldeviceGroup.SelectedIndex = 0;
            }
            try
            {
                string[] mobileNumber = dtEmp.Rows[0]["Phone_No"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtPhoneNo.Text = mobileNumber[0].ToString();
                }
                else
                {
                    ddlCountryCode.SelectedValue = mobileNumber[0].ToString();
                    txtPhoneNo.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            txtDob.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOB"].ToString()).ToString(objSys.SetDateFormat());
            txtDoj.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()).ToString(objSys.SetDateFormat());
            if (Convert.ToDateTime(dtEmp.Rows[0]["Termination_Date"]) == new DateTime(1900, 1, 1))
            {
                txtTermDate.Text = "";
            }
            else
            {
                txtTermDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["Termination_Date"].ToString()).ToString(objSys.SetDateFormat());
            }
            try
            {
                ddlCompany_SelectedIndexChanged(null, null);
                Hdn_Brand.Value = dtEmp.Rows[0]["Brand_Id"].ToString();
                ddlBrand.SelectedValue = dtEmp.Rows[0]["Brand_Id"].ToString();
                ddlBrand_SelectedIndexChanged(sender, e);
            }
            catch
            {
            }
            try
            {
                Hdn_Location.Value = dtEmp.Rows[0]["Location_Id"].ToString();
                ddlLocation.SelectedValue = dtEmp.Rows[0]["Location_Id"].ToString();
                ddlLocation_SelectedIndexChanged(sender, e);
            }
            catch
            {
            }
            try
            {
                string strDeptID = dtEmp.Rows[0]["Department_Id"].ToString();
                if (strDeptID != "0" && strDeptID != "")
                {
                    Hdn_Department.Value = strDeptID;
                    ddlDepartment.SelectedValue = strDeptID;
                }
                string strDesigID = dtEmp.Rows[0]["Designation_Id"].ToString();
                if (strDesigID != "0" && strDesigID != "")
                {
                    ddlDesignation.SelectedValue = strDesigID;
                }
                string strEmpType = dtEmp.Rows[0]["Emp_Type"].ToString();
                if (strEmpType != "")
                {
                    ddlEmpType.SelectedValue = strEmpType;
                }
                string strRelID = dtEmp.Rows[0]["Religion_Id"].ToString();
                if (strRelID != "" && strRelID != "0")
                {
                    ddlReligion.SelectedValue = strRelID;
                }
                string strQualiID = dtEmp.Rows[0]["Qualification_Id"].ToString();
                if (strQualiID != "" && strQualiID != "0")
                {
                    ddlQualification.SelectedValue = strQualiID;
                }
                string strNationID = dtEmp.Rows[0]["Nationality_Id"].ToString();
                if (strNationID != "" && strNationID != "0")
                {
                    ddlNationality.SelectedValue = strNationID;
                }
                ddlGender.SelectedValue = dtEmp.Rows[0]["Gender"].ToString();
                // Modified By Nitin jain On 20/11/2014
                ViewState["IncrementDate"] = dtEmp.Rows[0]["Field7"].ToString();
                // Modified By Kalu Singh On 29/07/2017
                txtPanNo.Text = dtEmp.Rows[0]["Pan"].ToString() != null ? dtEmp.Rows[0]["Pan"].ToString() : "";
                txtFatherName.Text = dtEmp.Rows[0]["FatherName"].ToString() != null ? dtEmp.Rows[0]["FatherName"].ToString() : "";
                rblIsMarried.SelectedValue = dtEmp.Rows[0]["IsMarried"].ToString() != null ? dtEmp.Rows[0]["IsMarried"].ToString() : "";
                txtDLNo.Text = dtEmp.Rows[0]["DLNo"].ToString() != null ? dtEmp.Rows[0]["DLNo"].ToString() : "";
            }
            catch
            {
            }
            try
            {
                if (dtEmp.Rows[0]["Field1"].ToString() == "")
                {
                    rbtnEmployee.Checked = true;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = false;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "Manager")
                {
                    rbtnEmployee.Checked = false;
                    rbtnManager.Checked = true;
                    rbtnCEO.Checked = false;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "CEO")
                {
                    rbtnEmployee.Checked = false;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = true;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "Employee")
                {
                    rbtnEmployee.Checked = true;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = false;
                }
            }
            catch
            {
            }
            try
            {
                imgLogo.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Emp_Image"].ToString();
                Session["empimgpath"] = dtEmp.Rows[0]["Emp_Image"].ToString();
                ImgEmpSignature.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Field3"].ToString();
                Session["empSignimgpath"] = dtEmp.Rows[0]["Field3"].ToString();
            }
            catch
            {
            }
            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Employee", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)GvAddressName, dtChild, "", "");
                int Sr_No = 1;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    Label lblAddress = (Label)gvr.FindControl("lblgvAddress");
                    lblAddress.Text = GetAddressByAddressName(lblAddressName.Text);
                    Label lblSNo = (Label)gvr.FindControl("lblSNo");
                    lblSNo.Text = Sr_No.ToString();
                    Sr_No++;
                }
            }
            DataTable dtBank = objBankInfo.GetBankInfoByRefId(editid.Value, Session["CompId"].ToString());
            if (dtBank.Rows.Count > 0)
            {
                Session["BankId"] = dtBank.Rows[0]["Bank_Id"].ToString();
                txtBankName.Text = dtBank.Rows[0]["Bank_Name"].ToString();
                ddlAcountType.SelectedValue = dtBank.Rows[0]["Account_Type"].ToString();
                txtAccountNo.Text = dtBank.Rows[0]["Account_No"].ToString();
                Txt_Ifsc_Code.Text = dtBank.Rows[0]["Field1"].ToString();
                Txt_Branch_Code.Text = dtBank.Rows[0]["Field2"].ToString();
                Txt_Swift_Code.Text = dtBank.Rows[0]["Field3"].ToString();
                Txt_IBAN_Code.Text = dtBank.Rows[0]["Field4"].ToString();
            }
            else
            {
                txtAccountNo.Text = "";
                txtBankName.Text = "";
                Txt_Ifsc_Code.Text = "";
                Txt_Branch_Code.Text = "";
                Txt_Swift_Code.Text = "";
                Txt_IBAN_Code.Text = "";
                ddlAcountType.SelectedIndex = 0;
            }
            chkCreateUser.Enabled = CheckUserValidation(editid.Value);
            txtEmployeeCode.ReadOnly = true;
            //get record from filetransaction table
            //TabArcaWing.Visible = true;
            //arcawing code for select exist record
            //created by jitendra on 08-08-2014
            //start
            //txtExpiryDate.Text = DateTime.Now.AddYears(20).ToString(objSys.SetDateFormat());
            //string DirectoryName = string.Empty;
            //DirectoryName = Session["CompId"].ToString() + "/Employee/" + editid.Value;
            //DataTable dt = objDir.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), DirectoryName);
            //if (dt.Rows.Count > 0)
            //{
            //    FillArcawingGrid(dt.Rows[0]["Id"].ToString());
            //}
            //end
        }
    }
    protected void ImgBtnEmpEdit(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        Session["empimgpath"] = "";
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), editid.Value);
        if (dtEmp.Rows.Count > 0)
        {
            btnNew_Click(null, null);
            btnNew.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            ddlCompany_SelectedIndexChanged(null, null);
            DataTable dtEmpSal = objEmpParam.GetEmployeeParameterByEmpId(editid.Value, Session["CompId"].ToString());
            if (dtEmpSal.Rows.Count > 0)
            {
                try
                {
                    ddlCategoryNewTab.SelectedValue = dtEmpSal.Rows[0]["Field11"].ToString();
                }
                catch
                {
                }
            }
            try
            {
                ddlTeamLeader.SelectedValue = dtEmp.Rows[0]["Field5"].ToString();
            }
            catch
            {
                ddlTeamLeader.SelectedIndex = 0;
            }
            try
            {
                ddldeviceGroup.SelectedValue = dtEmp.Rows[0]["Device_Group_Id"].ToString();
            }
            catch
            {
                ddldeviceGroup.SelectedIndex = 0;
            }
            txtCompanyMobileNo.Text = dtEmp.Rows[0]["Company_phone_no"].ToString();
            txtEmployeeCode.Text = dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeCode.ReadOnly = true;
            txtEmployeeName.Text = dtEmp.Rows[0]["Emp_Name"].ToString() + "/" + dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeL.Text = dtEmp.Rows[0]["Emp_Name_L"].ToString();
            //CreateDirectory(txtEmployeeCode.Text, editid.Value);
            txtCivilId.Text = dtEmp.Rows[0]["Civil_Id"].ToString();
            txtEmailId.Text = dtEmp.Rows[0]["Email_Id"].ToString();
            try
            {
                string[] mobileNumber = dtEmp.Rows[0]["Phone_No"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtPhoneNo.Text = mobileNumber[0].ToString();
                }
                else
                {
                    ddlCountryCode.SelectedValue = mobileNumber[0].ToString();
                    txtPhoneNo.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            txtDob.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOB"].ToString()).ToString(objSys.SetDateFormat());
            txtDoj.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()).ToString(objSys.SetDateFormat());
            if (Convert.ToDateTime(dtEmp.Rows[0]["Termination_Date"]) == new DateTime(1900, 1, 1))
            {
                txtTermDate.Text = "";
            }
            else
            {
                txtTermDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["Termination_Date"].ToString()).ToString(objSys.SetDateFormat());
            }
            try
            {
                ddlCompany_SelectedIndexChanged(null, null);
                Hdn_Brand.Value = dtEmp.Rows[0]["Brand_Id"].ToString();
                ddlBrand.SelectedValue = dtEmp.Rows[0]["Brand_Id"].ToString();
                ddlBrand_SelectedIndexChanged(sender, e);
            }
            catch
            {
            }
            try
            {
                Hdn_Location.Value = dtEmp.Rows[0]["Location_Id"].ToString();
                ddlLocation.SelectedValue = dtEmp.Rows[0]["Location_Id"].ToString();
                ddlLocation_SelectedIndexChanged(sender, e);
            }
            catch
            {
            }
            try
            {
                string strDeptID = dtEmp.Rows[0]["Department_Id"].ToString();
                if (strDeptID != "0" && strDeptID != "")
                {
                    Hdn_Department.Value = strDeptID;
                    ddlDepartment.SelectedValue = strDeptID;
                }
                string strDesigID = dtEmp.Rows[0]["Designation_Id"].ToString();
                if (strDesigID != "0" && strDesigID != "")
                {
                    ddlDesignation.SelectedValue = strDesigID;
                }
                string strEmpType = dtEmp.Rows[0]["Emp_Type"].ToString();
                if (strEmpType != "")
                {
                    ddlEmpType.SelectedValue = strEmpType;
                }
                string strRelID = dtEmp.Rows[0]["Religion_Id"].ToString();
                if (strRelID != "" && strRelID != "0")
                {
                    ddlReligion.SelectedValue = strRelID;
                }
                string strQualiID = dtEmp.Rows[0]["Qualification_Id"].ToString();
                if (strQualiID != "" && strQualiID != "0")
                {
                    ddlQualification.SelectedValue = strQualiID;
                }
                string strNationID = dtEmp.Rows[0]["Nationality_Id"].ToString();
                if (strNationID != "" && strNationID != "0")
                {
                    ddlNationality.SelectedValue = strNationID;
                }
                ddlGender.SelectedValue = dtEmp.Rows[0]["Gender"].ToString();
                ViewState["IncrementDate"] = dtEmp.Rows[0]["Field7"].ToString();
            }
            catch
            {
            }
            try
            {
                imgLogo.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Emp_Image"].ToString();
                Session["empimgpath"] = dtEmp.Rows[0]["Emp_Image"].ToString();
                ImgEmpSignature.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Field3"].ToString();
                Session["empSignimgpath"] = dtEmp.Rows[0]["Field3"].ToString();
            }
            catch
            {
            }
            try
            {
                if (dtEmp.Rows[0]["Field1"].ToString() == "")
                {
                    rbtnEmployee.Checked = true;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = false;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "Manager")
                {
                    rbtnEmployee.Checked = false;
                    rbtnManager.Checked = true;
                    rbtnCEO.Checked = false;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "CEO")
                {
                    rbtnEmployee.Checked = false;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = true;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "Employee")
                {
                    rbtnEmployee.Checked = true;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = false;
                }
            }
            catch
            {
            }
            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Employee", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)GvAddressName, dtChild, "", "");
                int Sr_No = 1;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    Label lblAddress = (Label)gvr.FindControl("lblgvAddress");
                    lblAddress.Text = GetAddressByAddressName(lblAddressName.Text);
                    Label lblSNo = (Label)gvr.FindControl("lblSNo");
                    lblSNo.Text = Sr_No.ToString();
                    Sr_No++;
                }
            }
            DataTable dtBank = objBankInfo.GetBankInfoByRefId(editid.Value, Session["CompId"].ToString());
            if (dtBank.Rows.Count > 0)
            {
                txtBankName.Text = dtBank.Rows[0]["Bank_Name"].ToString();
                ddlAcountType.SelectedValue = dtBank.Rows[0]["Account_Type"].ToString();
                txtAccountNo.Text = dtBank.Rows[0]["Account_No"].ToString();
                Txt_Ifsc_Code.Text = dtBank.Rows[0]["Field1"].ToString();
                Txt_Branch_Code.Text = dtBank.Rows[0]["Field2"].ToString();
                Txt_Swift_Code.Text = dtBank.Rows[0]["Field3"].ToString();
                Txt_IBAN_Code.Text = dtBank.Rows[0]["Field4"].ToString();
            }
            else
            {
                txtAccountNo.Text = "";
                txtBankName.Text = "";
                Txt_Ifsc_Code.Text = "";
                Txt_Branch_Code.Text = "";
                Txt_Swift_Code.Text = "";
                Txt_IBAN_Code.Text = "";
                ddlAcountType.SelectedIndex = 0;
            }
            txtEmployeeCode.ReadOnly = true;
            //get record for arcawing
            //TabArcaWing.Visible = true;
            //txtExpiryDate.Text = DateTime.Now.AddYears(20).ToString(objSys.SetDateFormat());
            //arcawing code for select exist record
            //created by jitendra on 08-08-2014
            //start
            //string DirectoryName = string.Empty;
            //DirectoryName = Session["CompId"].ToString() + "/Employee/" + editid.Value;
            //DataTable dt = objDir.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), DirectoryName);
            //if (dt.Rows.Count > 0)
            //{
            //    FillArcawingGrid(dt.Rows[0]["Id"].ToString());
            //}
            //end
        }
    }
    protected void btnDeleteLeave_Command(object sender, CommandEventArgs e)
    {
        //this code is modify by jitendra upadhyay on  02-09-2014
        //here we check that if employee used this leave than can not delete
        //code start
        Att_Leave_Request objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), e.CommandName.ToString());
        try
        {
            dtLeaveR = new DataView(dtLeaveR, "Leave_Type_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        try
        {
            DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            int FinancialYearMonth = 0;
            if (dt.Rows.Count > 0)
            {
                FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
            }
            DateTime FinancialYearStartDate = new DateTime();
            DateTime FinancialYearEndDate = new DateTime();
            if (DateTime.Now.Month < FinancialYearMonth)
            {
                FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            else
            {
                FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            dtLeaveR = new DataView(dtLeaveR, "CreatedDate>='" + FinancialYearStartDate + "' and CreatedDate<='" + FinancialYearEndDate + "' ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtLeaveR.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtLeaveR.Rows[0]["Is_Canceled"].ToString()) == false)
            {
                DisplayMessage("This Leave is in used ,you can not delete");
                return;
            }
        }
        //code end
        int b = 0;
        b = objEmpleave.DeleteEmployeeLeaveByEmpIdandleaveTypeIdIsActive(e.CommandName.ToString(), e.CommandArgument.ToString(), DateTime.Now.Year.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            DataTable dtEmpLeave = objEmpleave.GetEmployeeLeaveByEmpId(Session["CompId"].ToString(), e.CommandName.ToString());
            if (dtEmpLeave.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvLeaveEmp, dtEmpLeave, "", "");
                foreach (GridViewRow gvr in gvLeaveEmp.Rows)
                {
                    string Schtype = ((DropDownList)gvr.FindControl("ddlSchType0")).SelectedValue;
                    if (Schtype == "Yearly")
                    {
                        ((CheckBox)gvr.FindControl("chkYearCarry0")).Enabled = true;
                    }
                    else
                    {
                        ((CheckBox)gvr.FindControl("chkYearCarry0")).Enabled = false;
                    }
                }
            }
            else
            {
                gvLeaveEmp.DataSource = null;
                gvLeaveEmp.DataBind();
                //pnl2.Visible = false;
            }
        }
        SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Leave Deleted", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dtLogs = objEmpleave.Get_Log_By_Date_EmpID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, DateTime.Now.ToString(), Convert.ToDateTime(DateTime.Now.ToString()).AddDays(1).ToString(), "1", "2");
        if (dtLogs != null && dtLogs.Rows.Count > 0)
        {
            DisplayMessage("Log exist for this employee so you cannot delete");
            return;
        }
        int b = 0;
        b = objEmp.DeleteEmployeeMaster(Session["CompId"].ToString(), editid.Value, false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillDataListGrid();
            FillbinDataListGrid();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
        SystemLog.SaveSystemLog("Employee Master", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Employee Deleted", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Lbl_Tab_Name.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
        editid.Value = e.CommandArgument.ToString();
        Session["empimgpath"] = "";
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), editid.Value);
        if (dtEmp.Rows.Count > 0)
        {
            DataTable dtEmpSal = objEmpParam.GetEmployeeParameterByEmpId(editid.Value, Session["CompId"].ToString());
            if (dtEmpSal.Rows.Count > 0)
            {
                try
                {
                    ddlCategoryNewTab.SelectedValue = dtEmpSal.Rows[0]["Field11"].ToString();
                }
                catch
                {
                }
            }
            btnNew_Click(null, null);
            btnNew.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            ddlCompany_SelectedIndexChanged(null, null);
            Hdn_Brand.Value = dtEmp.Rows[0]["Brand_Id"].ToString();
            ddlBrand.SelectedValue = dtEmp.Rows[0]["Brand_Id"].ToString();
            ddlBrand_SelectedIndexChanged(sender, e);
            Hdn_Location.Value = dtEmp.Rows[0]["Location_Id"].ToString();
            ddlLocation.SelectedValue = dtEmp.Rows[0]["Location_Id"].ToString();
            txtFatherName.Text = dtEmp.Rows[0]["FatherName"].ToString();
            try
            {
                ddlGrade.SelectedValue = dtEmp.Rows[0]["Grade"].ToString().Trim();
            }
            catch (Exception ex)
            {

            }
            try
            {
                ddldeviceGroup.SelectedValue = dtEmp.Rows[0]["Device_Group_Id"].ToString();
            }
            catch
            {
                ddldeviceGroup.SelectedIndex = 0;
            }
            ddlLocation_SelectedIndexChanged(sender, e);
            try
            {
                ddlTeamLeader.SelectedValue = dtEmp.Rows[0]["Field5"].ToString();
            }
            catch
            {
                ddlTeamLeader.SelectedIndex = 0;
            }
            txtCompanyMobileNo.Text = dtEmp.Rows[0]["Company_phone_no"].ToString();
            Hdn_Edit.Value = dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeCode.Text = dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeCode.ReadOnly = true;
            txtEmployeeName.Text = dtEmp.Rows[0]["Emp_Name"].ToString();
            //txtEmployeeName.Text = dtEmp.Rows[0]["Emp_Name"].ToString() + "/" + dtEmp.Rows[0]["Emp_Code"].ToString();
            txtEmployeeL.Text = dtEmp.Rows[0]["Emp_Name_L"].ToString();
            //CreateDirectory(txtEmployeeCode.Text, editid.Value);
            txtCivilId.Text = dtEmp.Rows[0]["Civil_Id"].ToString();
            txtEmailId.Text = dtEmp.Rows[0]["Email_Id"].ToString();
            try
            {
                string[] mobileNumber = dtEmp.Rows[0]["Phone_No"].ToString().Split('-');
                if (mobileNumber.Length == 1)
                {
                    txtPhoneNo.Text = mobileNumber[0].ToString();
                }
                else
                {
                    ddlCountryCode.SelectedValue = mobileNumber[0].ToString();
                    txtPhoneNo.Text = mobileNumber[1].ToString();
                }
            }
            catch
            {
            }
            txtDob.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOB"].ToString()).ToString(objSys.SetDateFormat());
            txtDoj.Text = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()).ToString(objSys.SetDateFormat());

            txtPassportNo.Text = dtEmp.Rows[0]["Passport_No"].ToString();
            if (dtEmp.Rows[0]["CiviId_Expire"].ToString() == "")
            {
                txtCivilIdExpiryDate.Text = "";
            }
            else
            {
                txtCivilIdExpiryDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["CiviId_Expire"].ToString()).ToString(objSys.SetDateFormat());
            }
            if (dtEmp.Rows[0]["Passport_Expire"].ToString() == "")
            {
                txtPassportExpiryDate.Text = "";
            }
            else
            {
                txtPassportExpiryDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["Passport_Expire"].ToString()).ToString(objSys.SetDateFormat());
            }


            if (Convert.ToDateTime(dtEmp.Rows[0]["Termination_Date"]) == new DateTime(1900, 1, 1))
            {
                txtTermDate.Text = "";
            }
            else
            {
                txtTermDate.Text = Convert.ToDateTime(dtEmp.Rows[0]["Termination_Date"].ToString()).ToString(objSys.SetDateFormat());
            }
            try
            {
                string strDeptID = dtEmp.Rows[0]["Department_Id"].ToString();
                if (strDeptID != "0" && strDeptID != "")
                {
                    Hdn_Department.Value = strDeptID;
                    ddlDepartment.SelectedValue = strDeptID;
                }
                string strDesigID = dtEmp.Rows[0]["Designation_Id"].ToString();
                if (strDesigID != "0" && strDesigID != "")
                {
                    ddlDesignation.SelectedValue = strDesigID;
                }
                //string strEmpType = dtEmp.Rows[0]["Emp_Type"].ToString();
                //if (strEmpType != "")
                //{
                //    ddlEmpType.SelectedValue = strEmpType;
                //}
                string strRelID = dtEmp.Rows[0]["Religion_Id"].ToString();
                if (strRelID != "" && strRelID != "0")
                {
                    ddlReligion.SelectedValue = strRelID;
                }
                string strQualiID = dtEmp.Rows[0]["Qualification_Id"].ToString();
                if (strQualiID != "" && strQualiID != "0")
                {
                    ddlQualification.SelectedValue = strQualiID;
                }
                string strNationID = dtEmp.Rows[0]["Nationality_Id"].ToString();
                if (strNationID != "" && strNationID != "0")
                {
                    ddlNationality.SelectedValue = strNationID;
                }
                ddlGender.SelectedValue = dtEmp.Rows[0]["Gender"].ToString();
                ViewState["IncrementDate"] = dtEmp.Rows[0]["Field7"].ToString();
            }
            catch
            {
            }
            try
            {
                //imgLogo.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Emp_Image"].ToString();
                //Session["empimgpath"] = dtEmp.Rows[0]["Emp_Image"].ToString();
                string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string Path = "~/CompanyResource/" + RegistrationCode + "/" + Session["CompId"].ToString() + "";
                imgLogo.ImageUrl = Path + "/" + dtEmp.Rows[0]["Emp_Image"].ToString();
                ImgEmpSignature.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Field3"].ToString();
                Session["empSignimgpath"] = dtEmp.Rows[0]["Field3"].ToString();
            }
            catch (Exception exc)
            {
                string Path = "~/CompanyResource/" + Session["CompId"].ToString() + "";
                imgLogo.ImageUrl = Path + "/" + dtEmp.Rows[0]["Emp_Image"].ToString();
                ImgEmpSignature.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtEmp.Rows[0]["Field3"].ToString();
                Session["empSignimgpath"] = dtEmp.Rows[0]["Field3"].ToString();
            }
            try
            {
                ddlEmpType.SelectedValue = dtEmp.Rows[0]["Emp_Type"].ToString();
            }
            catch
            {
                ddlEmpType.SelectedValue = "On Role";
            }
            try
            {
                if (dtEmp.Rows[0]["Field1"].ToString() == "")
                {
                    rbtnEmployee.Checked = true;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = false;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "Manager")
                {
                    rbtnEmployee.Checked = false;
                    rbtnManager.Checked = true;
                    rbtnCEO.Checked = false;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "CEO")
                {
                    rbtnEmployee.Checked = false;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = true;
                }
                else if (dtEmp.Rows[0]["Field1"].ToString().Trim() == "Employee")
                {
                    rbtnEmployee.Checked = true;
                    rbtnManager.Checked = false;
                    rbtnCEO.Checked = false;
                }
            }
            catch
            {
            }
            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Employee", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)GvAddressName, dtChild, "", "");
                int Sr_No = 1;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    Label lblAddress = (Label)gvr.FindControl("lblgvAddress");
                    lblAddress.Text = GetAddressByAddressName(lblAddressName.Text);
                    Label lblSNo = (Label)gvr.FindControl("lblSNo");
                    lblSNo.Text = Sr_No.ToString();
                    Sr_No++;
                }
            }
            DataTable dtBank = objBankInfo.GetBankInfoByRefId(editid.Value, Session["CompId"].ToString());
            if (dtBank.Rows.Count > 0)
            {
                txtBankName.Text = dtBank.Rows[0]["Bank_Name"].ToString();
                ddlAcountType.SelectedValue = dtBank.Rows[0]["Account_Type"].ToString();
                txtAccountNo.Text = dtBank.Rows[0]["Account_No"].ToString();
                Txt_Ifsc_Code.Text = dtBank.Rows[0]["Field1"].ToString();
                Txt_Branch_Code.Text = dtBank.Rows[0]["Field2"].ToString();
                Txt_Swift_Code.Text = dtBank.Rows[0]["Field3"].ToString();
                Txt_IBAN_Code.Text = dtBank.Rows[0]["Field4"].ToString();
            }
            else
            {
                txtAccountNo.Text = "";
                txtBankName.Text = "";
                Txt_Ifsc_Code.Text = "";
                Txt_Branch_Code.Text = "";
                Txt_Swift_Code.Text = "";
                Txt_IBAN_Code.Text = "";
                ddlAcountType.SelectedIndex = 0;
            }
            chkCreateUser.Enabled = CheckUserValidation(editid.Value);
            try
            {
                ddlBrand.SelectedValue = dtEmp.Rows[0]["Brand_Name"].ToString();
            }
            catch
            {
            }
            try
            {
                ddlLocation.SelectedValue = dtEmp.Rows[0]["Location_Name"].ToString();
            }
            catch
            {
            }
            txtEmployeeCode.ReadOnly = true;

            try
            {
                EmpSync = objAppParam.GetApplicationParameterValueByParamName("Employee Synchronization", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                dtDevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());
                if (EmpSync == "Company")
                {
                }
                else
                {
                    dtDevice = new DataView(dtDevice, "Location_Id='" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dtDevice.Rows.Count > 0)
                {
                    //Common Function add By Lokesh on 14-05-2015
                    objPageCmn.FillData((object)listEmpSync, dtDevice, "Device_Name", "Device_Id");
                }
            }
            catch (Exception Ex)
            {
            }
            //end
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        if (ddlDeptList.Items.Count > 0)
            ddlDeptList.SelectedValue = "--Select--";
        //ddlLocationList.SelectedIndex = 0;
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpSalary.Visible = false;
        PnlEmpSalary.Visible = false;
        gvEmpLeave.Visible = false;
        rptCustomers.Visible = false;
        div_Paging.Visible = false;
        //dtlistEmp.Visible = true;
        gvEmp.Visible = true;
        lnkPrev.Visible = false;
        lnkNext.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        //FillbinDataListGrid();
        if (IsPostBack)
        {
            FillDataListGrid();
        }
        imbBtnGrid.Visible = false;
        imgBtnDatalist.Visible = true;
        lblSelectedRecord.Text = "";
        ddlLocationList.Focus();
        AllPageCode();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        //Add On 17-04-2015
        strForEmpCode = objAppParam.GetApplicationParameterValueByParamName("EmployeeCode", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (strForEmpCode == "True" && Lbl_Tab_Name.Text == "New")
        {
            txtEmployeeCode.Text = objAtt.GetNewEmpCode();
            if (txtEmployeeCode.Text == "")
            {
                txtEmployeeCode.Text = "1001";
            }
            txtEmployeeCode.ReadOnly = true;
        }
        else if (strForEmpCode == "False" && Lbl_Tab_Name.Text == "New")
        {
            txtEmployeeCode.Text = "";
            txtEmployeeCode.ReadOnly = false;
        }
        try
        {
            ddlBrand.SelectedValue = Session["BrandId"].ToString();
            ddlBrand_SelectedIndexChanged(null, null);
        }
        catch (Exception Ex)
        {
        }
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        txtEmployeeCode.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlEmployeeLeave.Visible = false;
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        gvEmpSalary.Visible = false;
        PnlEmpSalary.Visible = false;
        lblSelectedRecord.Text = "";
        rbtnEmployee.Checked = true;
        rbtnManager.Checked = false;
        rbtnCEO.Checked = false;
        AllPageCode();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpLeave.Visible = false;
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        //PnlEmployeeLeave.Visible = false;
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        gvEmpSalary.Visible = false;
        PnlEmpSalary.Visible = false;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = true;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        btnTerDel.ToolTip = "Deleted";
        FillbinDataListGrid();
        imgBtnbinGrid.Visible = false;
        lblSelectedRecord.Text = "";
        imgBtnbinDatalist.Visible = true;
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
    }
    #region SaveLeave
    protected void btnLeave_Click(object sender, EventArgs e)
    {
        SetHalfDay();
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        gvEmpSalary.Visible = false;
        PnlEmpSalary.Visible = false;
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = false;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = false;
        //chkYearCarry.Visible = false;
        imgBtnbinGrid.Visible = false;
        imgBtnbinDatalist.Visible = false;
        lblSelectRecd.Text = "";
        gvEmpLeave.Visible = true;
        Session["dtLeave"] = null;
        gridEmpLeave.DataSource = null;
        gridEmpLeave.DataBind();
        rbtnEmp.Checked = true;
        rbtnGroup.Checked = false;
        EmpGroup_CheckedChanged(null, null);
        ddlLocationLeave.Focus();
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
        if (Hdn_Edit.Value != "")
        {
            //ddlLocationLeave.SelectedValue = ddlLocationList.SelectedValue;
            //ddlLocationLeave_SelectedIndexChanged(null, null);
            ddlField1.SelectedValue = "Emp_Code";
            txtValue1.Text = Hdn_Edit.Value;
            btnLeavebind_Click(null, null);
        }
    }
    protected void chkgvSelectAll_CheckedChangedLeave(object sender, EventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        CheckBox ChkHeader = (CheckBox)gvEmpLeave.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in gvEmpLeave.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        AllPageCode();
    }
    protected void ImgbtnSelectAll_ClickLeave(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpLeave"];
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
            foreach (GridViewRow gvrow in gvEmpLeave.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpLeave"];
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpLeave, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }
    protected void gvEmpLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        SaveCheckedValues(gvEmpLeave);
        gvEmpLeave.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtEmpLeave"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dt, "", "");
        PopulateCheckedValues(gvEmpLeave);
        AllPageCode();
    }
    protected void btnSaveHalfday_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (txtHalfDayCount.Text == "")
        {
            DisplayMessage("Enter Half Day Count");
            txtHalfDayCount.Focus();
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
                DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }
            foreach (string str in EmpIds.Split(','))
            {
                if (str != "")
                {
                    SaveHalfDayLeave(str, txtHalfDayCount.Text);
                    b++;
                }
            }
        }
        else
        {
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues(gvEmpLeave);
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select employee first");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select employee first");
                    return;
                }
            }
            for (int i = 0; i < userdetails.Count; i++)
            {
                //b = objEmpParam.UpdateEmployeeHalfDayCount(Session["CompId"].ToString(), userdetails[i].ToString(), txtHalfDayCount.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                SaveHalfDayLeave(userdetails[i].ToString(), txtHalfDayCount.Text);
                b++;
            }
        }
        if (b != 0)
        {
            DisplayMessage("Record Updated", "green");
        }
        else
        {
            DisplayMessage("Record not Updated");
        }
        AllPageCode();
    }
    #endregion
    protected void ddlScheduleGrid_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in gvLeaveEmp.Rows)
        {
            string Schtype = ((DropDownList)gvr.FindControl("ddlSchType0")).SelectedValue;
            if (Schtype == "Yearly")
            {
                ((CheckBox)gvr.FindControl("chkYearCarry0")).Enabled = true;
            }
            else
            {
                ((CheckBox)gvr.FindControl("chkYearCarry0")).Checked = false;
                ((CheckBox)gvr.FindControl("chkYearCarry0")).Enabled = false;
            }
        }
    }
    protected void btnUpdateHalfday_Click(object sender, EventArgs e)
    {
        int b = 0;
        SaveHalfDayLeave(hdnEmpIdHalfDay.Value, txtHalfDAy.Text);
        b = objEmpParam.UpdateEmployeeHalfDayCount(Session["CompId"].ToString(), hdnEmpIdHalfDay.Value, txtHalfDAy.Text, true.ToString(), Session["CompId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Updated", "green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Leave()", true);
            return;
        }
        SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Half Day Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }
    protected void btnCancelPopSal_Click(object sender, EventArgs e)
    {
        //pnlSal1.Visible = false;
        //pnlSal2.Visible = false;
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbnIndemnity1_OnCheckedChanged(null, null);
            rbnIndemnity1.Checked = true;
            //txtSalGIven.Enabled = true;
            //txtNextIndemnity.Enabled = true;
        }
        else
        {
            rbnIndemnity2_OnCheckedChanged(null, null);
            rbnIndemnity2.Checked = true;
            //txtSalGIven.Enabled = false;
            //txtNextIndemnity.Enabled = false;
            //rbnIndemnity1.Enabled = false;
            txtIndemnityYear.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
    }
    protected void btnCancelPopPenalty_Click(object sender, EventArgs e)
    {
        //pnlPen2.Visible = false;
        rbtEarlyOutDisable.Checked = false;
        rbtEarlyOutEnable.Checked = true;
        rbtLateInDisable.Checked = false;
        rbtLateInEnable.Checked = true;
        rbtnAbsentDisable.Checked = false;
        rbtnAbsentEnable.Checked = true;
        rbtnPartialLeaveEnable.Checked = true;
        rbtnPartialLeaveDisable.Checked = false;
    }
    protected void btnCancelPopOT_Click(object sender, EventArgs e)
    {
        //pnlOT2.Visible = false;
    }
    protected void btnCancelPopLeave_Click(object sender, EventArgs e)
    {
        //pnl2.Visible = false;
        gvLeaveEmp.DataSource = null;
        gvLeaveEmp.DataBind();
    }
    protected void btnCancelPopLeaveView_Click(object sender, EventArgs e)
    {
        //pnl2View.Visible = false;
        gvLeaveEmpView.DataSource = null;
        gvLeaveEmpView.DataBind();
    }
    protected void btnCancelPopLeave_Click1(object sender, EventArgs e)
    {
        //pnlNotice1.Visible = false;
        //pnlNotice2.Visible = false;
        BindNotification();
        //chkSMSDocExp.Checked = false;
        //chkSMSAbsent.Checked = false;
        //chkSMSLate.Checked = false;
        //chkSMSEarly.Checked = false;
        //ChkSmsLeave.Checked = false;
        //ChkSMSPartial.Checked = false;
        //chkSMSNoClock.Checked = false;
        //ChkRptAbsent.Checked = false;
        //chkRptLate.Checked = false;
        //chkRptEarly.Checked = false;
        //ChkRptInOut.Checked = false;
        //ChkRptSalary.Checked = false;
        //ChkRptOvertime.Checked = false;
        //ChkRptLog.Checked = false;
        //chkRptDoc.Checked = false;
        //chkRptViolation.Checked = false;
    }
    public string GetLeaveTypeName(object leavetypeid)
    {
        string leavetypename = string.Empty;
        DataTable dt = objLeaveType.GetLeaveMasterById(Session["CompId"].ToString(), leavetypeid.ToString());
        if (dt.Rows.Count > 0)
        {
            leavetypename = dt.Rows[0]["Leave_Name"].ToString();
        }
        return leavetypename;
    }
    public void SaveHalfDayLeave(string EmpId, string AssignLeave)
    {
        DataTable dtHalfDay = new DataTable();
        DateTime JoiningDate = new DateTime();
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmpId);
        if (dtEmp.Rows.Count > 0)
        {
            JoiningDate = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());
        }
        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }
        double TotalDays = 0;
        int TotalHalfDay = 0;
        double HalfDayPerMonth = 0;
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (DateTime.Now.Month >= FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            if (FinancialYearStartDate > JoiningDate)
            {
                FinancialYearStartDate = FinancialYearStartDate;
            }
            else
            {
                if (JoiningDate <= DateTime.Now)
                {
                    FinancialYearStartDate = JoiningDate;
                }
                else
                {
                    DisplayMessage("Date of Joining is grater then Current Date");
                    return;
                }
            }
            // FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            TotalDays = (FinancialYearEndDate.Subtract(FinancialYearStartDate)).Days;
            double Months1 = TotalDays / 30;
            Months1 = System.Math.Round(Months1);
            HalfDayPerMonth = double.Parse(AssignLeave) / 12;
            Months1 = HalfDayPerMonth * Months1;
            Months1 = System.Math.Round(Months1);
            TotalHalfDay = int.Parse(Months1.ToString());
            dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), EmpId);
            dtHalfDay = new DataView(dtHalfDay, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            try
            {
                dtHalfDay = new DataView(dtHalfDay, "CreatedDate>='" + FinancialYearStartDate + "' and CreatedDate<='" + FinancialYearEndDate + "' ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtHalfDay.Rows.Count > 0)
            {
                return;
            }
            objEmpHalfDay.DeleteEmployeeHalfDayTransByEmpIdYear(EmpId, DateTime.Now.Year.ToString());
            //objEmpHalfDay.InsertEmployeeHalfDayTrans(Session["CompId"].ToString(), EmpId, DateTime.Now.Year.ToString(), TotalHalfDay.ToString(), "0", TotalHalfDay.ToString(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objEmpParam.UpdateEmployeeHalfDayCount(Session["CompId"].ToString(), EmpId, txtHalfDayCount.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objEmpHalfDay.InsertEmployeeHalfDayTrans(Session["CompId"].ToString(), EmpId, DateTime.Now.Year.ToString(), AssignLeave.ToString(), "0", AssignLeave.ToString(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            if (JoiningDate > FinancialYearStartDate)
            {
                FinancialYearStartDate = JoiningDate;
            }
            TotalDays = FinancialYearEndDate.Subtract(FinancialYearStartDate).Days;
            double Months1 = TotalDays / 30;
            Months1 = System.Math.Round(Months1);
            HalfDayPerMonth = double.Parse(AssignLeave) / 12;
            Months1 = HalfDayPerMonth * Months1;
            Months1 = System.Math.Round(Months1);
            TotalHalfDay = int.Parse(Months1.ToString());
            dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), EmpId);
            dtHalfDay = new DataView(dtHalfDay, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            try
            {
                dtHalfDay = new DataView(dtHalfDay, "CreatedDate>='" + FinancialYearStartDate + "' and CreatedDate<='" + FinancialYearEndDate + "' ", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtHalfDay.Rows.Count > 0)
            {
                return;
            }
            objEmpHalfDay.DeleteEmployeeHalfDayTransByEmpIdYear(EmpId, (DateTime.Now.Year - 1).ToString());
            //objEmpHalfDay.InsertEmployeeHalfDayTrans(Session["CompId"].ToString(), EmpId, (DateTime.Now.Year - 1).ToString(), TotalHalfDay.ToString(), "0", TotalHalfDay.ToString(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objEmpParam.UpdateEmployeeHalfDayCount(Session["CompId"].ToString(), EmpId, txtHalfDayCount.Text, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objEmpHalfDay.InsertEmployeeHalfDayTrans(Session["CompId"].ToString(), EmpId, (DateTime.Now.Year - 1).ToString(), AssignLeave.ToString(), "0", AssignLeave.ToString(), "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Half Day Leave Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }
    protected void chkIsAuto_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox activeCheckBox = sender as CheckBox;
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        //foreach (GridViewRow rw in gvLeaveEmp.Rows)
        //{
        CheckBox chkBx = (CheckBox)gvLeaveEmp.Rows[gvrow.RowIndex].FindControl("chkYearCarry0");
        if (chkBx != activeCheckBox)
        {
            chkBx.Checked = false;
        }
        else
        {
            chkBx.Checked = true;
        }
        //}
    }
    protected void chkYearCarry0_OnCheckedChanged(object sender, EventArgs e)
    {
        CheckBox activeCheckBox = sender as CheckBox;
        GridViewRow gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;
        //foreach (GridViewRow rw in gvLeaveEmp.Rows)
        //{
        CheckBox chkBx = (CheckBox)gvLeaveEmp.Rows[gvrow.RowIndex].FindControl("chkIsAuto");
        if (chkBx != activeCheckBox)
        {
            chkBx.Checked = false;
        }
        else
        {
            chkBx.Checked = true;
        }
        //}
    }
    protected void ddlSchedule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchType.SelectedIndex != 0)
        {
            if (ddlSchType.SelectedIndex == 1)
            {
                //chkYearCarry.Visible = false;
            }
            else if (ddlSchType.SelectedIndex == 2)
            {
                chkYearCarry.Visible = true;
                // chkIsAuto.Visible = true;
                ChkIsRule.Visible = true;
            }
        }
        else
        {
            //chkYearCarry.Visible = false;
        }
        btnSaveLeave.Focus();
    }
    protected void btnClosePanel_Click2(object sender, EventArgs e)
    {
        //pnl2.Visible = false;
    }
    protected void btnClosePanel_Click1(object sender, EventArgs e)
    {
        //pnl2.Visible = false;
    }
    protected void btnClosePanelView_Click1(object sender, EventArgs e)
    {
        //pnl2View.Visible = false;
        gvLeaveEmpView.DataSource = null;
        gvLeaveEmpView.DataBind();
    }
    protected void btnClosePanel_Click5(object sender, EventArgs e)
    {
        //pnlOT2.Visible = false;
        //pnlSal1.Visible = false;
        //pnlSal2.Visible = false;
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbnIndemnity1_OnCheckedChanged(null, null);
            rbnIndemnity1.Checked = true;
            //txtSalGIven.Enabled = true;
            //txtNextIndemnity.Enabled = true;
        }
        else
        {
            rbnIndemnity2_OnCheckedChanged(null, null);
            rbnIndemnity2.Checked = true;
            //txtSalGIven.Enabled = false;
            //txtNextIndemnity.Enabled = false;
            //rbnIndemnity1.Enabled = false;
            txtIndemnityYear.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
    }
    protected void btnClosePanel_ClickPenalty(object sender, EventArgs e)
    {
        //pnlPen2.Visible = false;
        rbtEarlyOutDisable.Checked = false;
        rbtEarlyOutEnable.Checked = true;
        rbtLateInDisable.Checked = false;
        rbtLateInEnable.Checked = true;
        rbtnAbsentDisable.Checked = false;
        rbtnAbsentEnable.Checked = true;
        rbtnPartialLeaveEnable.Checked = true;
        rbtnPartialLeaveDisable.Checked = false;
    }
    protected void btnClosePanel_Click6(object sender, EventArgs e)
    {
        //pnlOT2.Visible = false;
    }
    protected void txtPaidLeaveTextChanged(object sender, EventArgs e)
    {
    }
    public void OnConfirm(object sender, EventArgs e)
    {
        bool yes = new bool();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "yes = confirm('You are want to stop this ?')", true);
        if (yes)
        {
        }
        else
        {
        }
        string confirmValue = Request.Form["confirm_value"];
        if (confirmValue == "Yes")
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
        }
        else
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
        }
    }
    #region Indemnity Parameter
    protected void ddlIndenityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //dvIndemnitySalary.Visible = false;
        //dvIndemnityLeave.Visible = false;
        //if (ddlIndenityType.SelectedValue == "1")
        //{
        //    dvIndemnitySalary.Visible = true;
        //}
        //else
        //{
        //    dvIndemnityLeave.Visible = true;
        //}
    }
    protected void ddlIndenityTypePopUp_SelectedIndexChanged(object sender, EventArgs e)
    {
        //dvIndemnitySalaryPopUp.Visible = false;
        //txtIndemnityLeavePopUp.Visible = false;
        //dvLeavePopUp.Visible = false;
        //if (ddlIndenityTypePopUp.SelectedValue == "1")
        //{
        //    dvIndemnitySalaryPopUp.Visible = true;
        //}
        //else
        //{
        //    dvLeavePopUp.Visible = true;
        //    txtIndemnityLeavePopUp.Visible = true;
        //}
    }
    protected void rbnIndemnity1_OnCheckedChanged(object sender, EventArgs e)
    {
        txtIndemnityYear.Enabled = true;
        rbnIndemnity1.Checked = true;
        txtIndemnityDays.Enabled = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Indemnity_Gratuity_Open()", true);
    }
    protected void rbnIndemnity2_OnCheckedChanged(object sender, EventArgs e)
    {
        rbnIndemnity2.Checked = true;
        txtIndemnityYear.Enabled = false;
        txtIndemnityDays.Enabled = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Indemnity_Gratuity_Open()", true);
    }
    protected void rbnIndemnity1PopUp_OnCheckedChanged(object sender, EventArgs e)
    {
        rbnIndemnity1PopUp.Checked = true;
        txtIndemnityYearPopUp.Enabled = true;
        txtIndemnityDaysPopUP.Enabled = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Indemnity_Gratuity_Open_Popup()", true);
    }
    protected void rbnIndemnity2PopUp_OnCheckedChanged(object sender, EventArgs e)
    {
        rbnIndemnity2PopUp.Checked = true;
        txtIndemnityYearPopUp.Enabled = false;
        txtIndemnityDaysPopUP.Enabled = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Indemnity_Gratuity_Open_Popup()", true);
    }
    #endregion
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strGrade = ddlGrade.SelectedIndex > 0 ? ddlGrade.SelectedValue.Trim() : "";
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        string strReligion = string.Empty;
        //bool IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        //int IndemnityYear = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        bool IsIndemnity = false;

        int IndemnityYear = 0;
        string IsEmpTerminatd = string.Empty;
        string EmployeeLevel = string.Empty;
        string DesignationId = string.Empty;
        string QualificationId = string.Empty;
        string NationalityId = string.Empty;
        if (rbtnEmployee.Checked)
        {
            EmployeeLevel = "Employee";
        }
        else if (rbtnManager.Checked)
        {
            EmployeeLevel = "Manager";
        }
        else if (rbtnCEO.Checked)
        {
            EmployeeLevel = "CEO";
        }
        if (chkCreateUser.Checked)
        {
            if (ddlRole.SelectedIndex <= 0)
            {
                DisplayMessage("Select Role Name");
                ddlRole.Focus();
                return;
            }
        }
        string BankId = "0";
        int b = 0;
        if (txtEmployeeCode.Text == "")
        {
            DisplayMessage("Enter Employee Code");
            txtEmployeeCode.Focus();
            return;
        }
        else if (txtEmployeeCode.Text.Trim().Contains('/'))
        {
            DisplayMessage("/ Sign not allow for Employee Code");
            txtEmployeeCode.Focus();
            return;
        }
        if (txtEmployeeName.Text == "")
        {
            DisplayMessage("Enter Employee Name");
            txtEmployeeName.Focus();
            return;
        }
        //if (txtEmailId.Text.Trim() == "")
        //{
        //    DisplayMessage("Enter Email-Id");
        //    txtEmailId.Focus();
        //    return;
        //}
        if (txtDob.Text == "")
        {
            txtDob.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtDob.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct Date of Birth Format");
                txtDob.Focus();
                return;
            }
        }
        if (txtEmailId.Text != "")
        {
            if (!IsValidEmail(txtEmailId.Text))
            {
                DisplayMessage("Enter Correct Email Id Format");
                txtEmailId.Focus();
                return;
            }
        }
        if (txtDoj.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtDoj.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct Date of joining Format");
                txtDoj.Focus();
                return;
            }
        }
        else
        {
            txtDoj.Text = DateTime.Now.ToString(objSys.SetDateFormat());
        }
        if (ddlBrand.SelectedValue == "")
        {
            DisplayMessage("Select Brand");
            ddlBrand.Focus();
            return;
        }
        else
        {
            //ddlLocation.Focus();
        }
        if (ddlLocation.SelectedValue == "")
        {
            DisplayMessage("Select Location");
            ddlLocation.Focus();
            return;
        }
        else
        {
            //ddlDepartment.Focus();
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }
        //here we are checking labour law for selected lolcation
        string strLabourLaw = string.Empty;
        strLabourLaw = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), ddlLocation.SelectedValue).Rows[0]["Field3"].ToString();

        string CountryCodewithMobileNumber = string.Empty;
        if (txtPhoneNo.Text != "")
        {
            CountryCodewithMobileNumber = ddlCountryCode.SelectedValue + "-" + txtPhoneNo.Text;
        }
        string strteamlederId = "0";
        if (ddlTeamLeader.SelectedIndex > 0)
        {
            strteamlederId = ddlTeamLeader.SelectedValue;
        }
        DateTime terminationDate = new DateTime();
        terminationDate = new DateTime(1900, 1, 1);


        IsEmpTerminatd = false.ToString();


        if (txtTermDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtTermDate.Text);
                IsEmpTerminatd = true.ToString();
            }
            catch
            {
                DisplayMessage("Enter Correct Date for termination Format");
                txtTermDate.Focus();
                return;
            }
        }
        if (Session["empimgpath"] == null)
        {
            Session["empimgpath"] = "";
        }
        if (ddlCategoryNewTab.SelectedValue == "Fresher")
        {
            Increment_Date = Convert.ToDateTime(txtDoj.Text).AddMonths(Fr_IncrementDuration);
        }
        else
        {
            Increment_Date = Convert.ToDateTime(txtDoj.Text).AddMonths(Exp_IncrementDuration);
        }
        if (Session["empSignimgpath"].ToString() == "")
        {
            Session["empSignimgpath"] = "";
        }
        if (ddlReligion.SelectedValue == "--Select--")
        {
            strReligion = "0";
        }
        else
        {
            strReligion = ddlReligion.SelectedValue;
        }
        if (ddlDesignation.SelectedValue == "--Select--")
        {
            DesignationId = "0";
        }
        else
        {
            DesignationId = ddlDesignation.SelectedValue;
        }
        if (ddlQualification.SelectedValue == "--Select--")
        {
            QualificationId = "0";
        }
        else
        {
            QualificationId = ddlQualification.SelectedValue;
        }
        if (ddlNationality.SelectedValue == "--Select--")
        {
            NationalityId = "0";
        }
        else
        {
            NationalityId = ddlNationality.SelectedValue;
        }
        if (editid.Value == "")
        {
            //16-04-2015
            DataTable dtEmpCode = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), txtEmployeeCode.Text);
            if (dtEmpCode.Rows.Count > 0)
            {
                DisplayMessage("Employee Code Already exists");
                return;
            }
            else
            {
                // if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")

                if (ConfigurationManager.AppSettings["ApplicationType"].ToString() == "cloud")
                {
                    MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
                    MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Session["RegistrationCode"].ToString(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
                    int attEmpCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from set_employeemaster where company_id = '" + Session["CompId"].ToString() + "'").Rows[0][0].ToString());
                    if ((attEmpCount + 1) > Convert.ToInt32(clsMasterCmp.no_of_employee.ToString()))
                    {

                        DisplayMessage("Maximum Employees is exceeded so please update your license");
                        //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
                        UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), "0", clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                        //DisplayMessage("Modal_UpdateLicense_Open()");
                        return;
                    }
                }
                //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && chkCreateUser.Checked)
                if (ConfigurationManager.AppSettings["ApplicationType"].ToString() == "cloud" && chkCreateUser.Checked)
                {
                    MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
                    MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Session["RegistrationCode"].ToString(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
                    int attUserCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from set_usermaster where company_id = '" + Session["CompId"].ToString() + "'").Rows[0][0].ToString());
                    if ((attUserCount + 1) > Convert.ToInt32(clsMasterCmp.user.ToString()))
                    {
                        DisplayMessage("Maximum Users is exceeded so please update your license");
                        //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
                        UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), "0", clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                        //DisplayMessage("Modal_UpdateLicense_Open()");
                        return;
                    }
                }

                if (txtCivilIdExpiryDate.Text == "")
                {
                    txtCivilIdExpiryDate.Text = "1900-01-01";
                }
                if (txtPassportExpiryDate.Text == "")
                {
                    txtPassportExpiryDate.Text = "1900-01-01";
                }


                b = objEmp.InsertEmployeeMaster(Session["CompId"].ToString(), txtEmployeeName.Text.Split('/')[0].ToString(), txtEmployeeL.Text, txtEmployeeCode.Text, Session["empimgpath"].ToString(), ddlBrand.SelectedValue, ddlLocation.SelectedValue, ddlDepartment.SelectedValue, txtCivilId.Text, DesignationId, strReligion, NationalityId, QualificationId, txtDob.Text, txtDoj.Text, ddlEmpType.SelectedValue, terminationDate.ToString(), ddlGender.SelectedValue, EmployeeLevel, IsEmpTerminatd, Session["empSignimgpath"].ToString(), "", strteamlederId, true.ToString(), Increment_Date.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtEmailId.Text, CountryCodewithMobileNumber, txtCompanyMobileNo.Text, txtPanNo.Text, txtFatherName.Text, rblIsMarried.SelectedValue.ToString(), txtDLNo.Text, strGrade, txtCivilIdExpiryDate.Text, txtPassportNo.Text, txtPassportExpiryDate.Text);
                if (chkCreateUser.Checked)
                {
                    CreateUser(b.ToString(), ddlRole.SelectedValue);
                }
                updateDeviceGoup(b.ToString(), ddldeviceGroup.SelectedIndex > 0 ? ddldeviceGroup.SelectedValue : "0");
                objEmp.InsertEmployeeLocationTransfer(b.ToString(), ddlLocation.SelectedValue, ddlLocation.SelectedValue, DateTime.Now.ToString(), "Employee Created", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
                // Nitin Jain On 27/11/2014 , Insert Into Indemnity Table 
                int Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), b.ToString(), Convert.ToDateTime(txtDoj.Text).AddYears(IndemnityYear).ToString(), "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                SystemLog.SaveSystemLog("Employee Master", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Employee Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
                if (b != 0)
                {
                    int c = 0;
                    int d = 0;
                    int f = 0;
                    //objEmpParam.DeleteEmployeeParameterByEmpId(b.ToString());
                    InsertEmployeeParameterOnEmployeeInsert(Session["CompId"].ToString(), b.ToString());
                    // New Parameter ...............
                    // Is Indemnity
                    //objEmpParam.DeleteEmployeeParameterByEmpIdNew(b.ToString());
                    c = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    // Indemnity Duration
                    d = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IndemnityYear", txtIndemnityYear.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    f = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IndemnityDayas", txtIndemnityDays.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    DataTable dtNf = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
                    foreach (DataRow dr in dtNf.Rows)
                    {
                        try
                        {
                            objEmpNotice.InsertEmployeeNotification(b.ToString(), dr["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        catch
                        {
                        }
                    }
                    //code end
                    //16-04-2015
                    //objSer.DeleteUserTransfer(b.ToString());
                    foreach (ListItem li in listEmpSync.Items)
                    {
                        if (li.Selected)
                        {
                            objSer.InsertUserTransfer(b.ToString(), li.Value.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                    string strMaxId = string.Empty;
                    strMaxId = b.ToString();
                    bool Isdefault = false;
                    foreach (GridViewRow gvr in GvAddressName.Rows)
                    {
                        if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                        {
                            Isdefault = true;
                            break;
                        }
                    }
                    foreach (GridViewRow gvr in GvAddressName.Rows)
                    {
                        Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                        CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                        if (GvAddressName.Rows.Count == 1)
                        {
                            chk.Checked = true;
                        }
                        else
                        {
                            if (Isdefault == false)
                            {
                                if (gvr.RowIndex == 0)
                                {
                                    chk.Checked = true;
                                }
                            }
                        }
                        if (lblGvAddressName.Text != "")
                        {
                            DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                            if (dtAddId.Rows.Count > 0)
                            {
                                string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                                objAddChild.InsertAddressChild(strAddressId, "Employee", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }
                        }
                    }
                    if (txtBankName.Text != "")
                    {
                        DataTable dt4 = objBank.GetBankMaster();
                        dt4 = new DataView(dt4, "Bank_Name='" + txtBankName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt4.Rows.Count == 0)
                        {
                            DisplayMessage("Bank Name Not Exists");
                            txtBankName.Focus();
                            return;
                        }
                        else
                        {
                            BankId = dt4.Rows[0]["Bank_Id"].ToString();
                            //objBankInfo.DeleteBankInfo(b.ToString(), "Employee");
                            objBankInfo.InsertBankInfo(Session["CompId"].ToString(), Session["BrandId"].ToString(), BankId, "Employee", b.ToString(), ddlAcountType.SelectedValue, txtAccountNo.Text, Txt_Ifsc_Code.Text.Trim(), Txt_Branch_Code.Text.Trim(), Txt_Swift_Code.Text.Trim(), Txt_IBAN_Code.Text.Trim(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                    //here we are assigning leave to employee according labour law for selected location
                    //code added on 24/10/2017 by jitendra upadhyay
                    //code start
                    if (strLabourLaw.Trim() != "0" && strLabourLaw.Trim() != "")
                    {
                        DataTable dtleavedetail = ObjLabourLeavedetail.GetRecord_By_LaborLawId(strLabourLaw);
                        dtleavedetail = new DataView(dtleavedetail, "Gender='" + ddlGender.SelectedItem.Text + "' or Gender='Both'", "", DataViewRowState.CurrentRows).ToTable();
                        foreach (DataRow dr in dtleavedetail.Rows)
                        {
                            objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), b.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), "100", dr["schedule_type"].ToString(), true.ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), dr["is_auto"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            SaveLeave("No", dr["Leave_Type_Id"].ToString(), b.ToString(), dr["schedule_type"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), FinancialYearStartDate, FinancialYearEndDate, Convert.ToDateTime(txtDoj.Text));
                            //here we are checking that any deduction slab exists or not for selected leave type
                            DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(dr["Leave_Type_Id"].ToString()).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");
                            foreach (DataRow childrow in dtdeduction.Rows)
                            {
                                ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, dr["Leave_Type_Id"].ToString(), b.ToString(), childrow["DaysFrom"].ToString(), childrow["Daysto"].ToString(), childrow["Deduction_Percentage"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }
                        }
                    }
                    //code end
                    DisplayMessage("Record Saved", "green");
                    //CreateDirectory(txtEmployeeCode.Text, strMaxId);
                    btnList_Click(null, null);
                    Reset();
                    //FillDataListGrid();
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
        }
        else
        {
            DataTable Dt_Pending_Approval = objApprovalEmp.GetApprovalTransation(Session["CompId"].ToString());
            if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
            {
                Dt_Pending_Approval = new DataView(Dt_Pending_Approval, "Emp_Id='" + editid.Value + "' and Status='Pending' and  Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Pending_Approval != null && Dt_Pending_Approval.Rows.Count > 0)
                {
                    if (Hdn_Brand.Value != "0" && Hdn_Brand.Value != "")
                    {
                        if (Hdn_Brand.Value != ddlBrand.SelectedValue.ToString())
                        {
                            ddlBrand.SelectedValue = Hdn_Brand.Value;
                            DisplayMessage("Request is in under processing , You cannot edit this Brand");
                            return;
                        }
                    }
                    if (Hdn_Location.Value != "0" && Hdn_Location.Value != "")
                    {
                        if (Hdn_Location.Value != ddlLocation.SelectedValue.ToString())
                        {
                            ddlLocation.SelectedValue = Hdn_Location.Value;
                            DisplayMessage("Request is in under processing , You cannot edit this Location");
                            return;
                        }
                    }
                    if (Hdn_Department.Value != "0" && Hdn_Department.Value != "")
                    {
                        if (Hdn_Department.Value != ddlDepartment.SelectedValue.ToString())
                        {
                            ddlDepartment.SelectedValue = Hdn_Department.Value;
                            DisplayMessage("Request is in under processing , You cannot edit this Department");
                            return;
                        }
                    }
                }
            }
            if (ViewState["IncrementDate"] == null)
            {
                ViewState["IncrementDate"] = Increment_Date;
            }
            string strEmpCurrentLocationId = string.Empty;
            DataTable dtempLocation = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), editid.Value);
            if (dtempLocation.Rows.Count > 0)
            {
                strEmpCurrentLocationId = dtempLocation.Rows[0]["Location_Id"].ToString();
            }

            if (txtCivilIdExpiryDate.Text == "")
            {
                txtCivilIdExpiryDate.Text = "1900-01-01";
            }
            if (txtPassportExpiryDate.Text == "")
            {
                txtPassportExpiryDate.Text = "1900-01-01";
            }

            // Modified By Nitin jain On 20/11/2014 to 
            b = objEmp.UpdateEmployeeMaster(editid.Value, Session["CompId"].ToString(), txtEmployeeName.Text.Split('/')[0].ToString(), txtEmployeeL.Text, txtEmployeeCode.Text, Session["empimgpath"].ToString(), ddlBrand.SelectedValue, ddlLocation.SelectedValue, ddlDepartment.SelectedValue, txtCivilId.Text, DesignationId, strReligion, NationalityId, QualificationId, txtDob.Text, txtDoj.Text, ddlEmpType.SelectedValue, terminationDate.ToString(), ddlGender.SelectedValue, EmployeeLevel, IsEmpTerminatd, Session["empSignimgpath"].ToString(), "", strteamlederId, true.ToString(), ViewState["IncrementDate"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtEmailId.Text, CountryCodewithMobileNumber, txtCompanyMobileNo.Text, txtPanNo.Text, txtFatherName.Text, rblIsMarried.SelectedValue.ToString(), txtDLNo.Text, strGrade, txtCivilIdExpiryDate.Text, txtPassportNo.Text, txtPassportExpiryDate.Text);
            if (chkCreateUser.Checked)
            {
                CreateUser(editid.Value, ddlRole.SelectedValue);
            }
            updateDeviceGoup(editid.Value, ddldeviceGroup.SelectedIndex > 0 ? ddldeviceGroup.SelectedValue : "0");
            if (strEmpCurrentLocationId != ddlLocation.SelectedValue)
            {
                objEmp.InsertEmployeeLocationTransfer(b.ToString(), strEmpCurrentLocationId, ddlLocation.SelectedValue, DateTime.Now.ToString(), "Location Updated", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
            }
            SystemLog.SaveSystemLog("Employee Master", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Employee Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
            if (b != 0)
            {
                //for (int i = 0; i < listEmpSync.Items.Count; i++)
                //{
                //    if (listEmpSync.SelectedValue.ToString() != "")
                //    {
                //        objSer.InsertUserTransfer(editid.Value.ToString(), listEmpSync.SelectedValue.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //    }
                //}
                //16-04-2015
                objSer.DeleteUserTransfer(b.ToString());
                foreach (ListItem li in listEmpSync.Items)
                {
                    if (li.Selected)
                    {
                        objSer.InsertUserTransfer(b.ToString(), li.Value.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }

                objAddChild.DeleteAddressChild("Employee", editid.Value);
                bool Isdefault = false;
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                    {
                        Isdefault = true;
                        break;
                    }
                }
                foreach (GridViewRow gvr in GvAddressName.Rows)
                {
                    Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                    CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                    if (GvAddressName.Rows.Count == 1)
                    {
                        chk.Checked = true;
                    }
                    else
                    {
                        if (Isdefault == false)
                        {
                            if (gvr.RowIndex == 0)
                            {
                                chk.Checked = true;
                            }
                        }
                    }
                    if (lblGvAddressName.Text != "")
                    {
                        DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                        if (dtAddId.Rows.Count > 0)
                        {
                            string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                            objAddChild.InsertAddressChild(strAddressId, "Employee", editid.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                //Bank Info of Employee 1 Oct 2013
                if (txtBankName.Text != "")
                {
                    DataTable dt1 = objBank.GetBankMaster();
                    dt1 = new DataView(dt1, "Bank_Name='" + txtBankName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count == 0)
                    {
                        DisplayMessage("Bank Name Not Exists");
                        txtBankName.Focus();
                        return;
                    }
                    else
                    {
                        BankId = dt1.Rows[0]["Bank_Id"].ToString();
                        objBankInfo.DeleteBankInfo(editid.Value, "Employee");
                        objBankInfo.InsertBankInfo(Session["CompId"].ToString(), Session["BrandId"].ToString(), BankId, "Employee", editid.Value, ddlAcountType.SelectedValue, txtAccountNo.Text, Txt_Ifsc_Code.Text.Trim(), Txt_Branch_Code.Text.Trim(), Txt_Swift_Code.Text.Trim(), Txt_IBAN_Code.Text.Trim(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
                else
                {
                    objBankInfo.DeleteBankInfo(editid.Value, "Employee");
                    DisplayMessage("Record Updated", "green");
                }
                btnList_Click(null, null);
                Reset();
                //FillDataListGrid();
                DisplayMessage("Record Updated", "green");
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
        //AllPageCode();
    }
    public void CreateUser(string strEmpId, string strRoleId)
    {
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        strsql = "select * from set_usermaster where emp_id=" + strEmpId + "";
        dt = objDa.return_DataTable(strsql);
        if (dt.Rows.Count == 0)
        {
            ObjUser.InsertUserMaster(Session["CompId"].ToString(), txtEmployeeCode.Text, Common.Encrypt(txtEmployeeCode.Text), strEmpId, strRoleId, false.ToString(), "0", "", "", "1", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "false");
            ObjUserdataPeram.InsertUserDataPermission(txtEmployeeCode.Text, Session["CompId"].ToString(), "C", Session["CompId"].ToString(), "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            ObjUserdataPeram.InsertUserDataPermission(txtEmployeeCode.Text, Session["CompId"].ToString(), "B", ddlBrand.SelectedValue, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            ObjUserdataPeram.InsertUserDataPermission(txtEmployeeCode.Text, Session["CompId"].ToString(), "L", ddlLocation.SelectedValue, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
    }
    public void CreateUser(string strEmpId, string strEmpCode, string strRoleId, string strLocationId, string strDepartmentId, ref SqlTransaction trns)
    {
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        strsql = "select * from set_usermaster where emp_id=" + strEmpId + "";
        dt = objDa.return_DataTable(strsql, ref trns);
        if (dt.Rows.Count == 0)
        {
            ObjUser.InsertUserMaster(Session["CompId"].ToString(), strEmpCode, Common.Encrypt(strEmpCode), strEmpId, strRoleId, false.ToString(), "0", "", "", "1", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "false", ref trns);
            ObjUserdataPeram.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "C", Session["CompId"].ToString(), "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            ObjUserdataPeram.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "B", Session["BrandId"].ToString(), "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            ObjUserdataPeram.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "L", strLocationId, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            ObjUserdataPeram.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "D", strDepartmentId, strLocationId, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
    }
    public void updateDeviceGoup(string strEmpId, string strGroupId)
    {
        objDa.execute_Command("update Set_EmployeeMaster set Device_Group_Id =" + strGroupId + " where Emp_Id=" + strEmpId + "");
    }
    public void updateDeviceGoup(string strEmpId, string strGroupId, ref SqlTransaction trns)
    {
        objDa.execute_Command("update Set_EmployeeMaster set Device_Group_Id =" + strGroupId + " where Emp_Id=" + strEmpId + "", ref trns);
    }
    public bool CheckUserValidation(string strEmpId)
    {
        bool Result = false;
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        strsql = "select * from set_usermaster where emp_id=" + strEmpId + "";
        dt = objDa.return_DataTable(strsql);
        if (dt.Rows.Count == 0)
        {
            Result = true;
        }
        dt.Dispose();
        return Result;
    }
    public void InsertEmployeeParameterOnEmployeeInsert(string CompanyId, string Emp_Id)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        string Basic_Salary = "0";
        string Salary_Type = "Monthly";
        string Currency_Id = "1";
        string Assign_Min = string.Empty;
        try
        {
            Assign_Min = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Assign_Min = "540";
        }
        string Effective_Work_Cal_Method = string.Empty;
        try
        {
            Effective_Work_Cal_Method = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Effective_Work_Cal_Method = "InOut";
        }
        string Is_OverTime = string.Empty;
        try
        {
            Is_OverTime = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Is_OverTime = false.ToString();
        }
        string Normal_OT_Method = string.Empty;
        try
        {
            Normal_OT_Method = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Normal_OT_Method = "Work Hour";
        }
        string Normal_OT_Type = "2";
        string Normal_OT_Value = "100";
        string Normal_HOT_Type = "2";
        string Normal_HOT_Value = "100";
        string Normal_WOT_Type = "2";
        string Normal_WOT_Value = "100";
        string Is_Partial_Enable = string.Empty;
        try
        {
            Is_Partial_Enable = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Is_Partial_Enable = false.ToString();
        }
        string Partial_Leave_Mins = string.Empty;
        try
        {
            Partial_Leave_Mins = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Partial_Leave_Mins = "240";
        }
        string Partial_Leave_Day = string.Empty;
        try
        {
            Partial_Leave_Day = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Partial_Leave_Day = "60";
        }
        string Is_Partial_Carry = false.ToString();
        string Field1 = string.Empty;
        try
        {
            Field1 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Field1 = false.ToString();
        }
        string Field2 = string.Empty;
        try
        {
            Field2 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Field2 = false.ToString();
        }
        string Field3 = string.Empty;
        Field3 = true.ToString();
        string Field4 = false.ToString();
        string Field5 = false.ToString();
        string Field6 = false.ToString();
        string Field7 = DateTime.Now.ToString();
        string Field8 = string.Empty;
        string Field9 = string.Empty;
        string Field10 = string.Empty;
        string Field11 = string.Empty;
        string Field12 = string.Empty;
        try
        {
            Field12 = objAppParam.GetApplicationParameterValueByParamName("Half_Day_Count", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
        }
        if (ddlCategoryNewTab.SelectedValue == "Fresher")
        {
            Field8 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Field9 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Field10 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Field11 = ddlCategoryNewTab.SelectedValue;
            DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), editid.Value);
            DateTime JoinDate = new DateTime();
            if (dtEmp.Rows.Count > 0)
            {
            }
            JoinDate = Convert.ToDateTime(txtDoj.Text);
            double IncrementPer = 0;
            try
            {
                IncrementPer = double.Parse(Field10);
            }
            catch
            {
            }
            double BasicSal = 0;
            double IncrementValue = 0;
            try
            {
                IncrementValue = (BasicSal * IncrementPer) / 100;
            }
            catch
            {
            }
            double IncrementSalary = 0;
            int Duration = 0;
            try
            {
                Duration = int.Parse(Field8);
            }
            catch
            {
            }
            IncrementSalary = BasicSal + IncrementValue;
            DateTime IncrementDate = JoinDate.AddMonths(Duration);
            objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(Emp_Id);
            //objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objEmpSalInc.InsertEmpSalaryIncrement(HttpContext.Current.Session["CompId"].ToString(), Emp_Id, BasicSal.ToString(), ddlCategoryNewTab.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            Field8 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Field9 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Field10 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            Field11 = ddlCategoryNewTab.SelectedValue;
            DateTime JoinDate = Convert.ToDateTime(txtDoj.Text);
            double IncrementPer = 0;
            try
            {
                IncrementPer = double.Parse(Field10);
            }
            catch
            {
            }
            double BasicSal = 0;
            double IncrementValue = 0;
            try
            {
                IncrementValue = (BasicSal * IncrementPer) / 100;
            }
            catch
            {
            }
            double IncrementSalary = 0;
            int Duration = 0;
            try
            {
                Duration = int.Parse(Field8);
            }
            catch
            {
            }
            IncrementSalary = BasicSal + IncrementValue;
            DateTime IncrementDate = JoinDate.AddMonths(Duration);
            objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(Emp_Id);
            objEmpSalInc.InsertEmpSalaryIncrement(HttpContext.Current.Session["CompId"].ToString(), Emp_Id, BasicSal.ToString(), ddlCategoryNewTab.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), Emp_Id, "0", Salary_Type, Currency_Id, Assign_Min, Effective_Work_Cal_Method, Is_OverTime, Normal_OT_Method, Normal_OT_Type, Normal_OT_Value, Normal_HOT_Type, Normal_HOT_Value, Normal_WOT_Type, Normal_WOT_Value, Is_Partial_Enable, Partial_Leave_Mins, Partial_Leave_Day, Is_Partial_Carry, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", true.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
    }
    public void InsertEmployeeParameterOnEmployeeInsert(string CompanyId, string Emp_Id, DateTime dtDOJ, ref SqlTransaction trns)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        string Basic_Salary = "0";
        string Salary_Type = "Monthly";
        string Currency_Id = "1";
        string Assign_Min = string.Empty;
        try
        {
            Assign_Min = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        }
        catch
        {
            Assign_Min = "540";
        }
        string Effective_Work_Cal_Method = string.Empty;
        try
        {
            Effective_Work_Cal_Method = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        }
        catch
        {
            Effective_Work_Cal_Method = "InOut";
        }
        string Is_OverTime = string.Empty;
        try
        {
            Is_OverTime = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns)).ToString();
        }
        catch
        {
            Is_OverTime = false.ToString();
        }
        string Normal_OT_Method = string.Empty;
        try
        {
            Normal_OT_Method = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        }
        catch
        {
            Normal_OT_Method = "Work Hour";
        }
        string Normal_OT_Type = "2";
        string Normal_OT_Value = "100";
        string Normal_HOT_Type = "2";
        string Normal_HOT_Value = "100";
        string Normal_WOT_Type = "2";
        string Normal_WOT_Value = "100";
        string Is_Partial_Enable = string.Empty;
        try
        {
            Is_Partial_Enable = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns)).ToString();
        }
        catch
        {
            Is_Partial_Enable = false.ToString();
        }
        string Partial_Leave_Mins = string.Empty;
        try
        {
            Partial_Leave_Mins = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        }
        catch
        {
            Partial_Leave_Mins = "240";
        }
        string Partial_Leave_Day = string.Empty;
        try
        {
            Partial_Leave_Day = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        }
        catch
        {
            Partial_Leave_Day = "60";
        }
        string Is_Partial_Carry = false.ToString();
        string Field1 = string.Empty;
        try
        {
            Field1 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns)).ToString();
        }
        catch
        {
            Field1 = false.ToString();
        }
        string Field2 = string.Empty;
        try
        {
            Field2 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns)).ToString();
        }
        catch
        {
            Field2 = false.ToString();
        }
        string Field3 = string.Empty;
        Field3 = true.ToString();
        string Field4 = false.ToString();
        string Field5 = false.ToString();
        string Field6 = false.ToString();
        string Field7 = DateTime.Now.ToString();
        string Field8 = string.Empty;
        string Field9 = string.Empty;
        string Field10 = string.Empty;
        string Field11 = string.Empty;
        string Field12 = string.Empty;
        try
        {
            Field12 = objAppParam.GetApplicationParameterValueByParamName("Half_Day_Count", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        }
        catch
        {
        }
        if (ddlCategoryNewTab.SelectedValue == "Fresher")
        {
            Field8 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            Field9 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            Field10 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            Field11 = ddlCategoryNewTab.SelectedValue;
            DateTime JoinDate = dtDOJ;
            double IncrementPer = 0;
            try
            {
                IncrementPer = double.Parse(Field10);
            }
            catch
            {
            }
            double BasicSal = 0;
            double IncrementValue = 0;
            try
            {
                IncrementValue = (BasicSal * IncrementPer) / 100;
            }
            catch
            {
            }
            double IncrementSalary = 0;
            int Duration = 0;
            try
            {
                Duration = int.Parse(Field8);
            }
            catch
            {
            }
            IncrementSalary = BasicSal + IncrementValue;
            DateTime IncrementDate = JoinDate.AddMonths(Duration);
            objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(Emp_Id, ref trns);
            //objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objEmpSalInc.InsertEmpSalaryIncrement(HttpContext.Current.Session["CompId"].ToString(), Emp_Id, BasicSal.ToString(), ddlCategoryNewTab.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
        else
        {
            Field8 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration For Experience", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            Field9 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            Field10 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", CompanyId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
            Field11 = ddlCategoryNewTab.SelectedValue;
            DateTime JoinDate = Convert.ToDateTime(txtDoj.Text);
            double IncrementPer = 0;
            try
            {
                IncrementPer = double.Parse(Field10);
            }
            catch
            {
            }
            double BasicSal = 0;
            double IncrementValue = 0;
            try
            {
                IncrementValue = (BasicSal * IncrementPer) / 100;
            }
            catch
            {
            }
            double IncrementSalary = 0;
            int Duration = 0;
            try
            {
                Duration = int.Parse(Field8);
            }
            catch
            {
            }
            IncrementSalary = BasicSal + IncrementValue;
            DateTime IncrementDate = JoinDate.AddMonths(Duration);
            objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(Emp_Id, ref trns);
            objEmpSalInc.InsertEmpSalaryIncrement(HttpContext.Current.Session["CompId"].ToString(), Emp_Id, BasicSal.ToString(), ddlCategoryNewTab.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), Emp_Id, "0", Salary_Type, Currency_Id, Assign_Min, Effective_Work_Cal_Method, Is_OverTime, Normal_OT_Method, Normal_OT_Type, Normal_OT_Value, Normal_HOT_Type, Normal_HOT_Value, Normal_WOT_Type, Normal_WOT_Value, Is_Partial_Enable, Partial_Leave_Mins, Partial_Leave_Day, Is_Partial_Carry, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", true.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", Salary_Payment_Option, ref trns);
    }

    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        ddlCompany_SelectedIndexChanged(null, null);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        Div_Main_Upload.Visible = true;
        pnlMap.Visible = false;
        Hdn_Edit.Value = "";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_New_Active()", true);
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        if (txtbinVal.Text.Trim().ToString() == "")
        {
            txtbinVal.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        DataTable dtproduct = new DataTable();
        dtproduct = (DataTable)Session["dtEmpBin"];

        if (dtproduct != null && dtproduct.Rows.Count > 0)
        {
            lnkbinNext.Visible = true;
            lnkbinLast.Visible = true;
        }
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinVal.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinVal.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinVal.Text.Trim() + "%'";
            }
            DataView view = new DataView();
            if (dtproduct != null && dtproduct.Rows.Count > 0)
            {
                view = new DataView(dtproduct, condition, "", DataViewRowState.CurrentRows);
                lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            }
            else
            {
                lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : 0";
            }
            //Session["dtEmpBin"] = view.ToTable();
            dtproduct = view.ToTable();
            if (dtproduct != null && dtproduct.Rows.Count <= 9)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)dtlistbinEmp, dtproduct, "", "");
                objPageCmn.FillData((object)gvBinEmp, dtproduct, "", "");
                lnkbinPrev.Visible = false;
                lnkbinFirst.Visible = false;
                lnkbinNext.Visible = false;
                lnkbinLast.Visible = false;
            }
            else
            {
                FillBinDataList(dtproduct, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
            }
            if (gvBinEmp.Visible == true)
            {
                if (dtproduct != null && dtproduct.Rows.Count == 0)
                {
                    //imgBtnRestore.Visible = false;
                    //ImgbtnSelectAll.Visible = false;
                }
                else
                {
                    AllPageCode();
                }
            }
        }
        if (btnTerDel.ToolTip == "Deleted")
        {
            foreach (GridViewRow gvr in gvBinEmp.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkgvSelect");
                GridViewRow header = gvBinEmp.HeaderRow;
                CheckBox chkSelectAll = header.FindControl("chkgvSelectAll") as CheckBox;
                //chkSelect.Enabled = false;
                //chkSelectAll.Enabled = false;
            }
            foreach (DataListItem dl in dtlistbinEmp.Items)
            {
                CheckBox chkInActive = (CheckBox)dl.FindControl("chbAcInctive");
                //chkInActive.Enabled = false;
            }
        }
        else if (btnTerDel.ToolTip == "Terminated")
        {
            foreach (GridViewRow gvr in gvBinEmp.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkgvSelect");
                GridViewRow header = gvBinEmp.HeaderRow;
                CheckBox chkSelectAll = header.FindControl("chkgvSelectAll") as CheckBox;
                chkSelect.Enabled = true;
                chkSelectAll.Enabled = true;
            }
            foreach (DataListItem dl in dtlistbinEmp.Items)
            {
                CheckBox chkInActive = (CheckBox)dl.FindControl("chbAcInctive");
                //chkInActive.Enabled = true;
            }
        }
        else if (btnTerDel.ToolTip == "")
        {
            foreach (GridViewRow gvr in gvBinEmp.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkgvSelect");
                GridViewRow header = gvBinEmp.HeaderRow;
                CheckBox chkSelectAll = header.FindControl("chkgvSelectAll") as CheckBox;
                //chkSelect.Enabled = false;
                //chkSelectAll.Enabled = false;
            }
            foreach (DataListItem dl in dtlistbinEmp.Items)
            {
                CheckBox chkInActive = (CheckBox)dl.FindControl("chbAcInctive");
                //chkInActive.Enabled = false;
            }
        }
        txtbinVal.Focus();
        AllPageCode();
    }
    protected void btnLeavebind_Click(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        if (txtValue1.Text.Trim().ToString() == "")
        {
            txtValue1.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
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
            DataTable dtEmp = (DataTable)Session["dtEmpLeave"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpLeave, view.ToTable(), "", "");
            Session["CHECKED_ITEMS"] = null;
        }
        Session["dtLeave"] = null;
        gridEmpLeave.DataSource = null;
        gridEmpLeave.DataBind();
        //chkYearCarry.Visible = false;        
        AllPageCode();
        imgBtnLeaveBind.Focus();
    }
    protected void btnLeaveRefresh_Click(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        DataTable dtEmp = (DataTable)Session["dtEmpLeave"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
        Session["dtLeave"] = null;
        Session["CHECKED_ITEMS"] = null;
        gridEmpLeave.DataSource = null;
        gridEmpLeave.DataBind();
        //chkYearCarry.Visible = false;
        AllPageCode();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        lnkbinNext.Visible = true;
        lnkbinLast.Visible = true;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        FillbinDataListGrid();
        ddlbinFieldName.SelectedIndex = 0;
        ddlbinOption.SelectedIndex = 3;
        txtbinVal.Text = "";
        btnbingo_Click(null, null);
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        AllPageCode();
    }
    protected void btnbingo_Click(object sender, EventArgs e)
    {
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = true;
        lnkbinLast.Visible = true;
        FillbinDataListGrid();
        AllPageCode();
    }
    protected void imgBtnbinGrid_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = true;
        FillbinDataListGrid();
        imgBtnbinDatalist.Visible = true;
        imgBtnbinGrid.Visible = false;
        txtbinVal.Focus();
        AllPageCode();
    }
    protected void imgbtnbinDatalist_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinNext.Visible = true;
        lnkbinLast.Visible = true;
        dtlistbinEmp.Visible = true;
        gvBinEmp.Visible = false;
        FillbinDataListGrid();
        imgBtnbinDatalist.Visible = false;
        imgBtnbinGrid.Visible = true;
        //ImgbtnSelectAll.Visible = false;
        //imgBtnRestore.Visible = false;
        txtbinVal.Focus();
        AllPageCode();
    }
    protected void btnBinResetSreach_Click(object sender, EventArgs e)
    {
        FillbinDataListGrid();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    protected void lnkbinFirst_Click(object sender, EventArgs e)
    {
        lnkbinPrev.Visible = false;
        lnkbinFirst.Visible = false;
        lnkbinLast.Visible = true;
        lnkbinNext.Visible = true;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        DataTable dt = (DataTable)Session["dtEmpBin"];
        FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        ViewState["SubSizebin"] = 9;
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSizebin"].ToString());
    }
    protected void lnkbinLast_Click(object sender, EventArgs e)
    {
        ViewState["SubSizebin"] = 9;
        DataTable dt = (DataTable)Session["dtEmpBin"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSizebin"].ToString());
        ViewState["CurrIndexbin"] = index;
        int tot = dt.Rows.Count;
        if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) > 0)
        {
            FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
        else if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) == 0)
        {
            FillBinDataList(dt, index - 1, Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
        else
        {
            FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
        lnkbinLast.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinPrev.Visible = true;
        lnkbinFirst.Visible = true;
    }
    protected void lnkbinPrev_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtEmpBin"];
        ViewState["SubSizebin"] = 9;
        ViewState["CurrIndexbin"] = Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) - 1;
        if (Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) < 0)
        {
            ViewState["CurrIndexbin"] = 0;
        }
        if (Convert.ToInt16(ViewState["CurrIndexbin"]) == 0)
        {
            lnkbinFirst.Visible = false;
            lnkbinPrev.Visible = false;
            lnkbinNext.Visible = true;
            lnkbinLast.Visible = true;
        }
        else
        {
            lnkbinFirst.Visible = true;
            lnkbinLast.Visible = true;
            lnkbinNext.Visible = true;
        }
        FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
    }
    protected void lnkbinNext_Click(object sender, EventArgs e)
    {
        ViewState["SubSizebin"] = 9;
        DataTable dt = (DataTable)Session["dtEmpBin"];
        int index = dt.Rows.Count / Convert.ToInt32(ViewState["SubSizebin"].ToString());
        int k1 = Convert.ToInt32(ViewState["CurrIndexbin"].ToString());
        ViewState["CurrIndexbin"] = Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) + 1;
        int k = Convert.ToInt32(ViewState["CurrIndexbin"].ToString());
        if (Convert.ToInt32(ViewState["CurrIndexbin"].ToString()) >= index)
        {
            ViewState["CurrIndexbin"] = index;
            lnkbinNext.Visible = false;
            lnkbinLast.Visible = false;
        }
        int tot = dt.Rows.Count;
        if (k == index)
        {
            if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) > 0)
            {
                FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                lnkbinPrev.Visible = true;
                lnkbinFirst.Visible = true;
            }
            else
            {
                lnkbinPrev.Visible = true;
                lnkbinFirst.Visible = true;
                lnkbinLast.Visible = true;
                lnkbinNext.Visible = true;
            }
        }
        else if (k < index)
        {
            if (k + 1 == index)
            {
                if (tot % Convert.ToInt32(ViewState["SubSizebin"].ToString()) > 0)
                {
                    FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                    lnkbinPrev.Visible = true;
                    lnkbinFirst.Visible = true;
                }
                else
                {
                    FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                    lnkbinNext.Visible = false;
                    lnkbinLast.Visible = false;
                    lnkbinPrev.Visible = true;
                    lnkbinFirst.Visible = true;
                }
            }
            else
            {
                FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                lnkbinPrev.Visible = true;
                lnkbinFirst.Visible = true;
            }
        }
        else
        {
            FillBinDataList(dt, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
        }
    }
    public string GetEmpIdByUserId(string Emp_Code)
    {
        string EmpId = string.Empty;
        if (Session["UserId"].ToString() == "superadmin")
        {
            EmpId = "0";
        }
        else
        {
            DataTable dtUser = ObjUser.GetUserMasterByUserId(Emp_Code, Session["CompId"].ToString());
            if (dtUser.Rows.Count > 0)
            {
                EmpId = dtUser.Rows[0]["Emp_Id"].ToString();
            }
            dtUser.Dispose();
        }
        return EmpId;
    }
    protected void chbAcInctive_CheckedChanged(object sender, EventArgs e)
    {
        //DataTable dtUnit = (DataTable)Session["dtEmpBin"];
        //ArrayList userdetails = new ArrayList();
        //Session["CHECKED_ITEMS"] = null;
        //if (ViewState["Select"] == null)
        //{
        //    ViewState["Select"] = 1;
        //    foreach (DataRow dr in dtUnit.Rows)
        //    {
        //        if (Session["CHECKED_ITEMS"] != null)
        //        {
        //            userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        //        }
        //        if (!userdetails.Contains(dr["Emp_Id"]))
        //        {
        //            userdetails.Add(dr["Emp_Id"]);
        //        }
        //    }
        //    foreach (DataListItem dl in dtlistbinEmp.Items)
        //    {
        //        CheckBox chkInActive = (CheckBox)dl.FindControl("chbAcInctive");
        //        chkInActive.Checked = true;
        //    }
        //    //foreach (GridViewRow GR in gvBinEmp.Rows)
        //    //{
        //    //    ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
        //    //}
        //    if (userdetails.Count > 0 && userdetails != null)
        //    {
        //        Session["CHECKED_ITEMS"] = userdetails;
        //    }
        //}
        //else
        //{
        //    lblSelectedRecord.Text = "";
        //    DataTable dtUnit1 = (DataTable)Session["dtEmpBin"];
        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)dtlistbinEmp, dtUnit1, "", "");
        //    ViewState["Select"] = null;
        //}
        Session["CHECKED_ITEMS"] = null;
        ArrayList userdetails = new ArrayList();
        int j = 0;
        for (int i = 0; i < dtlistbinEmp.Items.Count; i++)
        {
            CheckBox chb = (CheckBox)(dtlistbinEmp.Items[i].FindControl("chbAcInctive"));
            if (chb.Checked)
            {
                HiddenField hdempid = (HiddenField)(dtlistbinEmp.Items[i].FindControl("hdnChbActive"));
                ViewState["Select"] = 1;
                userdetails.Insert(j, hdempid.Value);
                j++;
            }
        }
        Session["CHECKED_ITEMS"] = userdetails;
        //FillbinDataListGrid();
        //FillDataListGrid();
        //DisplayMessage("Record Activated");
    }
    private void SaveCheckedValues(GridView GvCheckedEmployee)
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvCheckedEmployee.Rows)
        {
            index = (int)GvCheckedEmployee.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValues(GridView GvCheckedEmployee)
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvCheckedEmployee.Rows)
            {
                int index = (int)GvCheckedEmployee.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void gvBinEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvBinEmp);
        gvBinEmp.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtEmpBin"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvBinEmp, dt, "", "");
        AllPageCode();
        PopulateCheckedValues(gvBinEmp);
    }
    //protected void gvBinEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvBinEmp.PageIndex = e.NewPageIndex;
    //    FillbinDataListGrid();
    //    string temp = string.Empty;
    //    bool isselcted;
    //    for (int i = 0; i < gvBinEmp.Rows.Count; i++)
    //    {
    //        Label lblconid = (Label)gvBinEmp.Rows[i].FindControl("lblEmpId");
    //        string[] split = lblSelectedRecord.Text.Split(',');
    //        for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
    //        {
    //            if (lblSelectedRecord.Text.Split(',')[j] != "")
    //            {
    //                if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
    //                {
    //                    ((CheckBox)gvBinEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
    //                }
    //            }
    //        }
    //    }
    //    AllPageCode();
    //}
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBinEmp.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvBinEmp.Rows)
        {
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
        AllPageCode();
    }
    //protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    string empidlist = string.Empty;
    //    string temp = string.Empty;
    //    int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
    //    Label lb = (Label)gvBinEmp.Rows[index].FindControl("lblEmpId");
    //    if (((CheckBox)gvBinEmp.Rows[index].FindControl("chkgvSelect")).Checked)
    //    {
    //        empidlist += lb.Text.Trim().ToString() + ",";
    //        lblSelectedRecord.Text += empidlist;
    //    }
    //    else
    //    {
    //        empidlist += lb.Text.ToString().Trim();
    //        lblSelectedRecord.Text += empidlist;
    //        string[] split = lblSelectedRecord.Text.Split(',');
    //        foreach (string item in split)
    //        {
    //            if (item != empidlist)
    //            {
    //                if (item != "")
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //        }
    //        lblSelectedRecord.Text = temp;
    //    }
    //}
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        if (dtlistbinEmp.Visible == true)
        {
            DataTable dtUnit = (DataTable)Session["dtEmpBin"];
            ArrayList userdetails = new ArrayList();
            Session["CHECKED_ITEMS"] = null;
            if (ViewState["Select"] == null)
            {
                ViewState["Select"] = 1;
                foreach (DataRow dr in dtUnit.Rows)
                {
                    if (Session["CHECKED_ITEMS"] != null)
                    {
                        userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                    }
                    if (!userdetails.Contains(dr["Emp_Id"]))
                    {
                        userdetails.Add(dr["Emp_Id"]);
                    }
                }
                foreach (DataListItem dl in dtlistbinEmp.Items)
                {
                    CheckBox chkInActive = (CheckBox)dl.FindControl("chbAcInctive");
                    chkInActive.Checked = true;
                }
                //foreach (GridViewRow GR in gvBinEmp.Rows)
                //{
                //    ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
                //}
                if (userdetails.Count > 0 && userdetails != null)
                {
                    Session["CHECKED_ITEMS"] = userdetails;
                }
            }
            else
            {
                lblSelectedRecord.Text = "";
                DataTable dtUnit1 = (DataTable)Session["dtEmpBin"];
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)dtlistbinEmp, dtUnit1, "", "");
                ViewState["Select"] = null;
            }
        }
        else
        {
            DataTable dtUnit = (DataTable)Session["dtEmpBin"];
            ArrayList userdetails = new ArrayList();
            Session["CHECKED_ITEMS"] = null;
            if (ViewState["Select"] == null)
            {
                ViewState["Select"] = 1;
                foreach (DataRow dr in dtUnit.Rows)
                {
                    if (Session["CHECKED_ITEMS"] != null)
                    {
                        userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                    }
                    if (!userdetails.Contains(dr["Emp_Id"]))
                    {
                        userdetails.Add(dr["Emp_Id"]);
                    }
                }
                foreach (GridViewRow GR in gvBinEmp.Rows)
                {
                    ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
                }
                if (userdetails.Count > 0 && userdetails != null)
                {
                    Session["CHECKED_ITEMS"] = userdetails;
                }
            }
            else
            {
                lblSelectedRecord.Text = "";
                DataTable dtUnit1 = (DataTable)Session["dtEmpBin"];
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvBinEmp, dtUnit1, "", "");
                ViewState["Select"] = null;
            }
        }
        AllPageCode();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        if (dtlistbinEmp.Visible == true)
        {
            int b = 0;
            ArrayList userdetail = new ArrayList();
            // SaveCheckedValues(gvBinEmp);
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objEmp.DeleteEmployeeMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    FillbinDataListGrid();
                    FillDataListGrid();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                }
                else
                {
                    int fleg = 0;
                    foreach (DataListItem dl in dtlistbinEmp.Items)
                    {
                        CheckBox chk = (CheckBox)dl.FindControl("chbAcInctive");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
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
                gvBinEmp.Focus();
                return;
            }
        }
        else
        {
            int b = 0;
            ArrayList userdetail = new ArrayList();
            SaveCheckedValues(gvBinEmp);
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objEmp.DeleteEmployeeMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                if (b != 0)
                {
                    FillbinDataListGrid();
                    FillDataListGrid();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvBinEmp.Rows)
                    {
                        CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                        if (chk.Checked)
                        {
                            fleg = 1;
                        }
                        else
                        {
                            fleg = 0;
                        }
                    }
                    if (fleg == 0)
                    {
                        DisplayMessage("Please Select Record");
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
                gvBinEmp.Focus();
                return;
            }
        }
        AllPageCode();
    }

    protected void btnUpload1_Click(object sender, EventArgs e)
    {
        //imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPath.FileName + txtEmployeeCode.Text;
        if (Session["empimgpath"].ToString() != "")
            imgLogo.ImageUrl = "~/CompanyResource/" + Session["CompId"] + "/" + FULogoPath.FileName;
    }
    public void FillDataList(DataTable dt, int currentIndex, int SubSize)
    {
        int startRow = currentIndex * SubSize;
        int rowCounter = 0;
        DataTable dtBind = dt.Clone();
        while (rowCounter < SubSize)
        {
            if (startRow < dt.Rows.Count)
            {
                DataRow row = dtBind.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    row[dc.ColumnName] = dt.Rows[startRow][dc.ColumnName];
                }
                dtBind.Rows.Add(row);
                startRow++;
            }
            rowCounter++;
        }
        //Common Function add By Lokesh on 14-05-2015
        // objPageCmn.FillData((object)dtlistEmp, dtBind, "", "");
        GetCustomersPageWise(1, dtBind);
        AllPageCode();
    }
    private void FillDataListGrid()
    {
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationList, ddlDeptList);
        if (ddlempTypeFilter.SelectedIndex > 0)
        {
            dtEmp = new DataView(dtEmp, "Emp_Type='" + ddlempTypeFilter.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        Session["dtEmp"] = dtEmp;
        if (dtEmp.Rows.Count <= 9)
        {
            //Common Function add By Lokesh on 14-05-2015
            //objPageCmn.FillData((object)dtlistEmp, dtEmp, "", "");
            GetCustomersPageWise(1, dtEmp);
            objPageCmn.FillData((object)gvEmp, dtEmp, "", "");
            lnkPrev.Visible = false;
            lnkFirst.Visible = false;
            lnkNext.Visible = false;
            lnkLast.Visible = false;
        }
        else
        {
            lnkNext.Visible = true;
            lnkLast.Visible = true;
            //if (dtlistEmp.Visible == true)
            //{
            //    FillDataList(dtEmp, Convert.ToInt32(ViewState["CurrIndex"].ToString()), Convert.ToInt32(ViewState["SubSize"].ToString()));
            //}
            if (rptCustomers.Visible == true)
            {
                GetCustomersPageWise(1, dtEmp);
                div_Paging.Visible = true;
            }
            if (gvEmp.Visible == true)
            {
                lnkPrev.Visible = false;
                lnkFirst.Visible = false;
                lnkNext.Visible = false;
                lnkLast.Visible = false;
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvEmp, dtEmp, "", "");
                div_Paging.Visible = false;
            }
        }
        lblTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString();
        AllPageCode();
    }
    public string getdepartment(object empid)
    {
        string dept = string.Empty;
        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            DataTable dtDept = objDep.GetDepartmentMasterById(dt.Rows[0]["Department_Id"].ToString());
            if (dtDept.Rows.Count > 0)
            {
                dept = dtDept.Rows[0]["Dep_Name"].ToString();
            }
            else
            {
                dept = "No Department";
            }
        }
        return dept;
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
    public string getImageByEmpId(object empid)
    {
        string img = string.Empty;
        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Emp_Image"].ToString() != "")
            {
                img = "../CompanyResource/" + Session["CompId"] + "/" + dt.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                img = "../Bootstrap_Files/dist/img/Bavatar.png";
            }
        }
        return img;
    }


    public string getdesg(object empid)
    {
        string dept = string.Empty;
        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            DataTable dtDept = objDesg.GetDesignationMasterById(dt.Rows[0]["Designation_Id"].ToString());
            if (dtDept.Rows.Count > 0)
            {
                dept = dtDept.Rows[0]["Designation"].ToString();
            }
            else
            {
                dept = "No Designation";
            }
        }
        return dept;
    }
    public string getdate(object strDate)
    {
        DateTime dtnew = DateTime.Parse(strDate.ToString());
        string strNew = dtnew.ToString(objSys.SetDateFormat());
        return strNew;
    }
    public string getEmailId(object empid)
    {
        string email = string.Empty;
        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Email_Id"].ToString() != "")
            {
                email = dt.Rows[0]["Email_Id"].ToString();
            }
            else
            {
                email = "No Email Id";
            }
        }
        return email;
    }
    public string getMobileNo(object empid)
    {
        string email = string.Empty;
        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Phone_No"].ToString() != "")
            {
                email = dt.Rows[0]["Phone_No"].ToString();
            }
            else
            {
                email = "No Mobile No.";
            }
        }
        return email;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public void Reset()
    {
        ddlGrade.SelectedIndex = 0;
        Hdn_Brand.Value = "";
        Hdn_Location.Value = "";
        Hdn_Department.Value = "";
        Txt_Ifsc_Code.Text = "";
        Txt_Swift_Code.Text = "";
        Txt_IBAN_Code.Text = "";
        Txt_Branch_Code.Text = "";
        editid.Value = "";
        txtAccountNo.Text = "";
        txtBankName.Text = "";
        ddlAcountType.SelectedIndex = 0;
        ddlTeamLeader.SelectedIndex = 0;
        txtEmployeeCode.Text = "";
        txtEmployeeName.Text = "";
        txtEmployeeL.Text = "";
        txtEmployeeName.Text = "";
        txtCivilId.Text = "";
        txtEmailId.Text = "";
        txtPhoneNo.Text = "";
        txtDob.Text = "";
        txtDoj.Text = "";
        //ddlDocumentName.SelectedIndex = 0;
        //gvFileMaster.DataSource = null;
        //gvFileMaster.DataBind();
        //TabArcaWing.Visible = false;
        //Session["dtArcaWing"] = null;
        txtEmployeeCode.ReadOnly = false;
        ddlCategoryNewTab.SelectedIndex = 0;
        listEmpSync.ClearSelection();
        ViewState["IncrementDate"] = null;
        txtCompanyMobileNo.Text = "";
        try
        {
            ddlDepartment.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlEmpType.SelectedIndex = 0;
            ddlReligion.SelectedIndex = 0;
            ddlQualification.SelectedIndex = 0;
            ddlNationality.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
        }
        catch
        {
        }
        txtTermDate.Text = "";
        imgLogo.ImageUrl = "";
        ImgEmpSignature.ImageUrl = "";
        Session["empimgpath"] = "";
        Session["empSignimgpath"] = "";
        hdnProductId.Value = "";
        btnNew.Text = Resources.Attendance.New;
        Lbl_Tab_Name.Text = Resources.Attendance.New;
        txtAddressName.Text = "";
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        GvAddressName.DataSource = null;
        GvAddressName.DataBind();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        //Add On 17-04-2015
        strForEmpCode = objAppParam.GetApplicationParameterValueByParamName("EmployeeCode", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (strForEmpCode == "True")
        {
            txtEmployeeCode.Text = objAtt.GetNewEmpCode();
            txtEmployeeCode.ReadOnly = true;
        }
        if (ViewState["CountryCode"] != null)
        {
            try
            {
                ddlCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
            }
            catch
            {
            }
        }
        txtDLNo.Text = "";
        txtPanNo.Text = "";
        txtFatherName.Text = "";
        chkCreateUser.Checked = false;
        ddlRole.SelectedIndex = 0;
        chkCreateUser.Enabled = true;
        ddldeviceGroup.SelectedIndex = 0;
    }
    public void FillBinDataList(DataTable dt, int currentIndex, int SubSize)
    {
        int startRow = currentIndex * SubSize;
        int rowCounter = 0;
        DataTable dtBind = dt.Clone();
        while (rowCounter < SubSize)
        {
            if (startRow < dt.Rows.Count)
            {
                DataRow row = dtBind.NewRow();
                foreach (DataColumn dc in dt.Columns)
                {
                    row[dc.ColumnName] = dt.Rows[startRow][dc.ColumnName];
                }
                dtBind.Rows.Add(row);
                startRow++;
            }
            rowCounter++;
        }
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)dtlistbinEmp, dtBind, "", "");
        AllPageCode();
    }
    private void FillbinDataListGrid()
    {
        DataTable dtEmp = new DataTable();
        if (btnTerDel.ToolTip == "Deleted")
        {
            dtEmp = objEmp.GetTerminatedEmployeeRecord_By_CompanyId(Session["CompId"].ToString().ToString());
            //imgBtnRestore.Enabled = false;
            //ImgbtnSelectAll.Enabled = false;
        }
        else if (btnTerDel.ToolTip == "Terminated")
        {
            dtEmp = objEmp.GetEmployeeMasterInactive(Session["CompId"].ToString().ToString());
            //imgBtnRestore.Enabled = true;
            //ImgbtnSelectAll.Enabled = true;
        }
        else if (btnTerDel.ToolTip == "")
        {
            dtEmp = objEmp.GetEmployeeMasterInactive(Session["CompId"].ToString().ToString());
            //imgBtnRestore.Enabled = false;
            //ImgbtnSelectAll.Enabled = false;
        }
        //filter start here
        string strDepId = string.Empty;
        string strLocationId = string.Empty;
        string strLocationIDs = string.Empty;
        if (ddlDeptList_Bin.SelectedIndex == -1)
        {
            dtEmp = new DataView(dtEmp, "Department_id=-1", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (Session["EmpId"].ToString() == "0")
        {
            if (ddlLocationList_Bin.SelectedIndex == 0 && ddlDeptList_Bin.SelectedIndex == 0)
            {
                //dtEmp = new DataView(dtEmp, "Department_id=-1", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationList_Bin.SelectedIndex == 0 && ddlDeptList_Bin.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Department_Id =" + ddlDeptList_Bin.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationList_Bin.SelectedIndex > 0 && ddlDeptList_Bin.SelectedIndex == 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocationList_Bin.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationList_Bin.SelectedIndex > 0 && ddlDeptList_Bin.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocationList_Bin.SelectedValue + " and Department_id='" + ddlDeptList_Bin.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else
        {
            if ((ddlLocationList_Bin.SelectedIndex == 0 && ddlDeptList_Bin.SelectedIndex == 0) || (ddlLocationList_Bin.SelectedIndex == 0 && ddlDeptList_Bin.SelectedIndex > 0))
            {
                strLocationIDs = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                strLocationIDs = strLocationIDs.Substring(0, strLocationIDs.Length - 1);
                //----------Code to get location's Department-------------------------
                string strWhereClause = string.Empty;
                string strSql = "Select record_id,Field1 as location_id  From Set_UserDataPermission   where    Set_UserDataPermission.Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and  Set_UserDataPermission.User_Id =" + HttpContext.Current.Session["UserId"].ToString() + " and Set_UserDataPermission.IsActive='True' and Record_Type='D'";
                DataTable dtDepartment = objDa.return_DataTable(strSql);
                if (ddlDeptList_Bin.SelectedIndex > 0)
                {
                    dtDepartment = new DataView(dtDepartment, "record_id='" + ddlDeptList_Bin.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                string[] LocationArray = strLocationIDs.Split(',');
                string StrDepIDs = string.Empty;
                foreach (string strLoc in LocationArray)
                {
                    StrDepIDs = "";
                    DataTable dtDepartmentTemp = new DataView(dtDepartment, "location_id='" + strLoc + "'", "", DataViewRowState.CurrentRows).ToTable();
                    for (int i = 0; i < dtDepartmentTemp.Rows.Count; i++)
                    {
                        StrDepIDs += dtDepartmentTemp.Rows[i]["record_id"].ToString() + ",";
                    }
                    if (StrDepIDs != "")
                    {
                        StrDepIDs = StrDepIDs.Substring(0, StrDepIDs.Length - 1);
                        if (strWhereClause == string.Empty)
                        {
                            strWhereClause = "(Location_id='" + strLoc + "' and department_id in (" + StrDepIDs + "))";
                        }
                        else
                        {
                            strWhereClause = strWhereClause + " or " + "(Location_id='" + strLoc + "' and department_id in (" + StrDepIDs + "))";
                        }
                    }
                }
                if (strWhereClause != string.Empty)
                {
                    dtEmp = new DataView(dtEmp, strWhereClause, "", DataViewRowState.CurrentRows).ToTable();
                }
                //-------------end------------------------------------
            }
            //else if (ddlLocationFilter.SelectedIndex == 0 && ddlDepartmentFilter.SelectedIndex > 0)
            //{
            //}
            else if (ddlLocationList_Bin.SelectedIndex > 0 && ddlDeptList_Bin.SelectedIndex == 0)
            {
                strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", ddlLocationList_Bin.SelectedValue, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strDepId == "")
                {
                    strDepId = "0,";
                }
                dtEmp = new DataView(dtEmp, "Location_id=" + ddlLocationList_Bin.SelectedValue + " and Department_Id in (" + strDepId.Substring(0, strDepId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else if (ddlLocationList_Bin.SelectedIndex > 0 && ddlDeptList_Bin.SelectedIndex > 0)
            {
                dtEmp = new DataView(dtEmp, "Location_id =" + ddlLocationList_Bin.SelectedValue + " and Department_id='" + ddlDeptList_Bin.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        Session["dtEmpBin"] = dtEmp;
        if (dtEmp != null)
        {
            if (dtEmp.Rows.Count <= 9)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)dtlistbinEmp, dtEmp, "", "");
                objPageCmn.FillData((object)gvBinEmp, dtEmp, "", "");
                lnkbinPrev.Visible = false;
                lnkbinFirst.Visible = false;
                lnkbinNext.Visible = false;
                lnkbinLast.Visible = false;
            }
            else
            {
                lnkbinNext.Visible = true;
                lnkbinLast.Visible = true;
                FillBinDataList(dtEmp, Convert.ToInt32(ViewState["CurrIndexbin"].ToString()), Convert.ToInt32(ViewState["SubSizebin"].ToString()));
                if (gvBinEmp.Visible == true)
                {
                    lnkbinPrev.Visible = false;
                    lnkbinFirst.Visible = false;
                    lnkbinNext.Visible = false;
                    lnkbinLast.Visible = false;
                    //Common Function add By Lokesh on 14-05-2015
                    objPageCmn.FillData((object)gvBinEmp, dtEmp, "", "");
                }
            }
        }
        lblbinTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString();

        AllPageCode();
    }
    protected void IbtnDownloadterminated_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        DataTable dtContact = Session["dtEmpBin"] as DataTable;
        if (dtContact.Columns.Contains("DesignationName"))
        {
            dtContact = dtContact.DefaultView.ToTable(true, "Action_Type", "Effectivedate", "Location", "Emp_Code", "Emp_Name", "Emp_Name_L", "Gender", "DOB", "DOJ", "DepartmentName", "DeviceGroup", "Civil_Id", "Email_Id", "Phone_No", "DesignationName", "Religion", "Qualification", "Nationality", "UserRole", "Field3");
        }
        else
        {
            dtContact = dtContact.DefaultView.ToTable(true, "Action_Type", "Effectivedate", "Location", "Emp_Code", "Emp_Name", "Emp_Name_L", "Gender", "DOB", "DOJ", "DepartmentName", "DeviceGroup", "Civil_Id", "Email_Id", "Phone_No", "Designation", "Religion", "Qualification", "Nationality", "UserRole", "Field3");
        }
        dtContact.Columns["Emp_Code"].ColumnName = "Code";
        dtContact.Columns["Emp_Name"].ColumnName = "Name";
        dtContact.Columns["DOJ"].ColumnName = "Doj";
        dtContact.Columns["DOB"].ColumnName = "Dob";
        dtContact.Columns["Email_Id"].ColumnName = "Email-Id";
        dtContact.Columns["Phone_No"].ColumnName = "Phone_No";
        dtContact.Columns["DepartmentName"].ColumnName = "Department";
        dtContact.Columns["Emp_Name_L"].ColumnName = "Name_L";
        dtContact.Columns["Field3"].ColumnName = "ManagerCode";
        if (dtContact.Columns.Contains("DesignationName"))
        {
            dtContact.Columns["DesignationName"].ColumnName = "Designation";
        }
        dtContact.Columns["Civil_Id"].ColumnName = "Civil-id";
        if (dtContact.Rows.Count > 0)
        {
            ExportToExcel(dtContact, "Employee List");
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }
    }
    //Employee Notification
    protected void btnSalarybind_Click(object sender, EventArgs e)
    {
        I6.Attributes.Add("Class", "fa fa-minus");
        Div7.Attributes.Add("Class", "box box-primary");
        if (txtValueSal.Text.Trim() == "")
        {
            txtValueSal.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        if (ddlOptionSal.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionSal.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldSal.SelectedValue + ",System.String)='" + txtValueSal.Text.Trim() + "'";
            }
            else if (ddlOptionSal.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldSal.SelectedValue + ",System.String) like '%" + txtValueSal.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldSal.SelectedValue + ",System.String) Like '" + txtValueSal.Text.Trim() + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmpSal"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpSalary, view.ToTable(), "", "");
            Session["CHECKED_ITEMS"] = null;
            lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        AllPageCode();
        txtValueSal.Focus();
    }
    protected void btnSalaryRefresh_Click(object sender, EventArgs e)
    {
        I6.Attributes.Add("Class", "fa fa-minus");
        Div7.Attributes.Add("Class", "box box-primary");
        lblSelectRecordSal.Text = "";
        ddlFieldSal.SelectedIndex = 1;
        ddlOptionSal.SelectedIndex = 2;
        txtValueSal.Text = "";
        DataTable dtEmp = (DataTable)Session["dtEmpSal"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
        AllPageCode();
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnOTbind_Click(object sender, EventArgs e)
    {
        I7.Attributes.Add("Class", "fa fa-minus");
        Div8.Attributes.Add("Class", "box box-primary");
        if (txtValueOT.Text.Trim() == "")
        {
            txtValueOT.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        if (ddlOptionOT.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionOT.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldOT.SelectedValue + ",System.String)='" + txtValueOT.Text.Trim() + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldOT.SelectedValue + ",System.String) like '%" + txtValueOT.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldOT.SelectedValue + ",System.String) Like '" + txtValueOT.Text.Trim() + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmpOT"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpOT, view.ToTable(), "", "");
            Session["CHECKED_ITEMS"] = null;
            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        AllPageCode();
        txtValueOT.Focus();
    }
    protected void btnPenaltybind_Click(object sender, EventArgs e)
    {
        I8.Attributes.Add("Class", "fa fa-minus");
        Div9.Attributes.Add("Class", "box box-primary");
        if (txtValuePenalty.Text.Trim() == "")
        {
            txtValuePenalty.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        if (ddlOptionPenalty.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionPenalty.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldPenalty.SelectedValue + ",System.String)='" + txtValuePenalty.Text.Trim() + "'";
            }
            else if (ddlOptionPenalty.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldPenalty.SelectedValue + ",System.String) like '%" + txtValuePenalty.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldPenalty.SelectedValue + ",System.String) Like '" + txtValuePenalty.Text.Trim() + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmpPenalty"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpPenalty, view.ToTable(), "", "");
            Session["CHECKED_ITEMS"] = null;
            lblTotalRecordPenalty.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        AllPageCode();
        txtValuePenalty.Focus();
    }
    protected void btnNFbind_Click(object sender, EventArgs e)
    {
        I5.Attributes.Add("Class", "fa fa-minus");
        Div6.Attributes.Add("Class", "box box-primary");
        if (ddlOptionNF.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionNF.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNF.SelectedValue + ",System.String)='" + txtValueNF.Text.Trim() + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNF.SelectedValue + ",System.String) like '%" + txtValueNF.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNF.SelectedValue + ",System.String) Like '" + txtValueNF.Text.Trim() + "%'";
            }
            DataTable dtEmp = (DataTable)Session["dtEmpNF"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpNF, view.ToTable(), "", "");
            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
        }
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
        txtValueNF.Focus();
    }
    protected void btnNFRefresh_Click(object sender, EventArgs e)
    {
        I5.Attributes.Add("Class", "fa fa-minus");
        Div6.Attributes.Add("Class", "box box-primary");
        lblSelectRecordNF.Text = "";
        ddlFieldNF.SelectedIndex = 1;
        ddlOptionNF.SelectedIndex = 2;
        txtValueNF.Text = "";
        DataTable dtEmp = (DataTable)Session["dtEmpNF"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
        AllPageCode();
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btnOTRefresh_Click(object sender, EventArgs e)
    {
        I7.Attributes.Add("Class", "fa fa-minus");
        Div8.Attributes.Add("Class", "box box-primary");
        lblSelectRecordOT.Text = "";
        Session["CHECKED_ITEMS"] = null;
        ddlFieldOT.SelectedIndex = 1;
        ddlOptionOT.SelectedIndex = 2;
        txtValueOT.Text = "";
        DataTable dtEmp = (DataTable)Session["dtEmpOT"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpOT, dtEmp, "", "");
        AllPageCode();
    }
    protected void btnPenaltyRefresh_Click(object sender, EventArgs e)
    {
        I8.Attributes.Add("Class", "fa fa-minus");
        Div9.Attributes.Add("Class", "box box-primary");
        lblSelectRecordPenalty.Text = "";
        ddlFieldPenalty.SelectedIndex = 1;
        ddlOptionPenalty.SelectedIndex = 2;
        txtValuePenalty.Text = "";
        DataTable dtEmp = (DataTable)Session["dtEmpPenalty"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dtEmp, "", "");
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
    }
    protected void ImgbtnSelectAll_ClickSalary(object sender, EventArgs e)
    {
        I6.Attributes.Add("Class", "fa fa-minus");
        Div7.Attributes.Add("Class", "box box-primary");
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        ArrayList userdetails = new ArrayList();
        DataTable dtEmp = (DataTable)Session["dtEmpSal"];
        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtEmp.Rows)
            {
                //Allowance_Id
                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));
            }
            foreach (GridViewRow gvrow in gvEmpSalary.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpSal"];
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpSalary, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }
    protected void ImgbtnSelectAll_ClickNF(object sender, EventArgs e)
    {
        I5.Attributes.Add("Class", "fa fa-minus");
        Div6.Attributes.Add("Class", "box box-primary");
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpNF"];
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
            foreach (GridViewRow gvrow in gvEmpNF.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpNF"];
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpNF, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }
    protected void ImgbtnSelectAll_ClickOT(object sender, EventArgs e)
    {
        I7.Attributes.Add("Class", "fa fa-minus");
        Div8.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpOT"];
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
            foreach (GridViewRow gvrow in gvEmpOT.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtEmpOT"];
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpOT, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }
    protected void ImgbtnSelectAll_ClickPenalty(object sender, EventArgs e)
    {
        I8.Attributes.Add("Class", "fa fa-minus");
        Div9.Attributes.Add("Class", "box box-primary");
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["dtEmpPenalty"];
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
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpPenalty, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
        AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChangedNF(object sender, EventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        CheckBox ChkHeader = (CheckBox)gvEmpNF.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in gvEmpNF.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChangedPenalty(object sender, EventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        CheckBox ChkHeader = ((CheckBox)gvEmpPenalty.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gvrow in gvEmpPenalty.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChangedOT(object sender, EventArgs e)
    {
        //this code is created by jitendra upadhyay on 12-09-2014
        //this code for select checkbox without page refresh
        CheckBox ChkHeader = ((CheckBox)gvEmpOT.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gvrow in gvEmpOT.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChangedSal(object sender, EventArgs e)
    {
        CheckBox ChkHeader = (CheckBox)gvEmpSalary.HeaderRow.FindControl("chkgvSelectAll");
        foreach (GridViewRow gvrow in gvEmpSalary.Rows)
        {
            if (ChkHeader.Checked == true)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        AllPageCode();
    }
    protected void gvEmpSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvEmpSalary);
        gvEmpSalary.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpSal"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
        PopulateCheckedValues(gvEmpSalary);
        AllPageCode();
    }
    protected void gvEmpPenalty_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvEmpPenalty);
        gvEmpPenalty.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpPenalty"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dtEmp, "", "");
        PopulateCheckedValues(gvEmpPenalty);
        AllPageCode();
    }
    protected void gvEmpOT_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvEmpOT);
        gvEmpOT.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpOT"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpOT, dtEmp, "", "");
        PopulateCheckedValues(gvEmpOT);
        AllPageCode();
    }
    protected void gvEmpNF_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues(gvEmpNF);
        gvEmpNF.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpNF"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
        PopulateCheckedValues(gvEmpNF);
        AllPageCode();
    }
    protected void btnEditPenalty_Command(object sender, CommandEventArgs e)
    {
        string empid = e.CommandArgument.ToString();
        hdnEmpIdPenalty.Value = empid;
        string empname = GetEmployeeName(empid);
        lblEmpNamePen.Text = empname;
        lblEmpCodePen.Text = GetEmployeeCode(empid);
        DataTable dtEmpPenalty = objEmpParam.GetEmployeeParameterByEmpId(empid, Session["CompId"].ToString());
        if (dtEmpPenalty.Rows.Count > 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Penalty()", true);
            //pnlPen2.Visible = true;
            if (Convert.ToBoolean(dtEmpPenalty.Rows[0]["Field1"].ToString()))
            {
                rbtnLateInEnable.Checked = true;
                rbtnLateInDisable.Checked = false;
            }
            else
            {
                rbtnLateInEnable.Checked = false;
                rbtnLateInDisable.Checked = true;
            }
            if (Convert.ToBoolean(dtEmpPenalty.Rows[0]["Field2"].ToString()))
            {
                rbtnEarlyEnable.Checked = true;
                rbtnEarlyDisable.Checked = false;
            }
            else
            {
                rbtnEarlyEnable.Checked = false;
                rbtnEarlyDisable.Checked = true;
            }
            if (Convert.ToBoolean(dtEmpPenalty.Rows[0]["Field3"].ToString()))
            {
                rbtnAbsentEnableP.Checked = true;
                rbtnAbsentDisableP.Checked = false;
            }
            else
            {
                rbtnAbsentEnableP.Checked = false;
                rbtnAbsentDisableP.Checked = true;
            }
            try
            {
                if (Convert.ToBoolean(dtEmpPenalty.Rows[0]["Field13"].ToString()))
                {
                    PnlPlEnable.Checked = true;
                    PnlPlDisable.Checked = false;
                }
                else
                {
                    PnlPlEnable.Checked = false;
                    PnlPlDisable.Checked = true;
                }
            }
            catch
            {
                PnlPlEnable.Checked = false;
                PnlPlDisable.Checked = true;
            }
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                rbtnLateInEnable.Checked = false;
                rbtnLateInDisable.Checked = true;
                rbtnLateInEnable.Enabled = false;
                rbtnLateInDisable.Enabled = false;
            }
            else
            {
                rbtnLateInEnable.Enabled = true;
                rbtnLateInDisable.Enabled = true;
            }
            if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                rbtnEarlyEnable.Checked = false;
                rbtnEarlyDisable.Checked = true;
                rbtnEarlyEnable.Enabled = false;
                rbtnEarlyDisable.Enabled = false;
            }
            else
            {
                rbtnEarlyEnable.Enabled = true;
                rbtnEarlyDisable.Enabled = true;
            }
        }
        else
        {
            DisplayMessage("No Parameter save for this employee");
            return;
        }
    }
    protected void btnEditOT_Command(object sender, CommandEventArgs e)
    {
        string empid = e.CommandArgument.ToString();
        hdnEmpIdOt.Value = empid;
        string empname = GetEmployeeName(empid);
        lblEmpNameOT.Text = empname;
        lblEmpCodeOT.Text = GetEmployeeCode(empid);
        DataTable dtEmpOt = objEmpParam.GetEmployeeParameterByEmpId(empid, Session["CompId"].ToString());
        if (dtEmpOt.Rows.Count > 0)
        {
            DataTable Dt_Decimal_Count = Get_Decimal_Count(dtEmpOt.Rows[0]["Currency_Id"].ToString(), "", "", "0", "", "");
            if (Convert.ToBoolean(dtEmpOt.Rows[0]["Is_Partial_Enable"].ToString()))
            {
                rbtnPartialEnable1.Checked = true;
                rbtnPartialDisable1.Checked = false;
                rbtPartial1_OnCheckedChanged(null, null);
            }
            else
            {
                rbtnPartialEnable1.Checked = false;
                rbtnPartialDisable1.Checked = true;
                rbtPartial1_OnCheckedChanged(null, null);
            }
            if (Convert.ToBoolean(dtEmpOt.Rows[0]["Is_Partial_Carry"].ToString()))
            {
                rbtnCarryYes1.Checked = true;
                rbtnCarryNo1.Checked = false;
            }
            else
            {
                rbtnCarryYes1.Checked = false;
                rbtnCarryNo1.Checked = true;
            }
            if (Convert.ToBoolean(dtEmpOt.Rows[0]["Is_OverTime"].ToString()))
            {
                rbtnOTEnable1.Checked = true;
                rbtnOTDisable1.Checked = false;
                rbtnOTEnable1_CheckedChanged(null, null);
            }
            else
            {
                rbtnOTEnable1.Checked = false;
                rbtnOTDisable1.Checked = true;
                rbtnOTEnable1_CheckedChanged(null, null);
            }
            ddlOTCalc1.SelectedValue = dtEmpOt.Rows[0]["Normal_OT_Method"].ToString();
            ddlNormalType1.SelectedValue = dtEmpOt.Rows[0]["Normal_OT_Type"].ToString();
            ddlWeekOffType1.SelectedValue = dtEmpOt.Rows[0]["Normal_WOT_Type"].ToString();
            ddlHolidayValue1.SelectedValue = dtEmpOt.Rows[0]["Normal_HOT_Type"].ToString();
            if (dtEmpOt.Rows[0]["Normal_OT_Type"].ToString() == "1")
                txtNormal1.Text = Convert.ToDecimal(dtEmpOt.Rows[0]["Normal_OT_Value"].ToString()).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            else if (dtEmpOt.Rows[0]["Normal_OT_Type"].ToString() == "2")
                txtNormal1.Text = Convert.ToDecimal(dtEmpOt.Rows[0]["Normal_OT_Value"].ToString()).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            if (dtEmpOt.Rows[0]["Normal_WOT_Type"].ToString() == "1")
                txtWeekOffValue1.Text = Convert.ToDecimal(dtEmpOt.Rows[0]["Normal_OT_Value"].ToString()).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            else if (dtEmpOt.Rows[0]["Normal_WOT_Type"].ToString() == "2")
                txtWeekOffValue1.Text = Convert.ToDecimal(dtEmpOt.Rows[0]["Normal_OT_Value"].ToString()).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            if (dtEmpOt.Rows[0]["Normal_HOT_Type"].ToString() == "1")
                txtHolidayValue1.Text = Convert.ToDecimal(dtEmpOt.Rows[0]["Normal_OT_Value"].ToString()).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            else if (dtEmpOt.Rows[0]["Normal_HOT_Type"].ToString() == "2")
                txtHolidayValue1.Text = Convert.ToDecimal(dtEmpOt.Rows[0]["Normal_OT_Value"].ToString()).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            txtTotalMinutesP1.Text = dtEmpOt.Rows[0]["Partial_Leave_Mins"].ToString();
            txtMinuteOTOne.Text = dtEmpOt.Rows[0]["Partial_Leave_Day"].ToString();
            // Modified By Nitin On 20-09-2014
            bool IsCompOT = false;
            bool IsPartialComp = false;
            IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            if (IsCompOT)
            {
                //rbtnOTEnable1.Checked = true;
                //rbtnOTDisable1.Checked = false;
                //rbtOT1_OnCheckedChanged(null, null);
            }
            else
            {
                rbtnOTEnable1.Checked = false;
                rbtnOTDisable1.Checked = true;
                rbtnOTEnable1_CheckedChanged(null, null);
            }
            if (IsPartialComp)
            {
                //rbtnPartialEnable1.Checked = true;
                //rbtnPartialDisable1.Checked = false;
                //rbtPartial1_OnCheckedChanged(null, null);
            }
            else
            {
                rbtnPartialEnable1.Checked = false;
                rbtnPartialDisable1.Checked = true;
                rbtPartial1_OnCheckedChanged(null, null);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_OT_PL()", true);
        }
        else
        {
            DisplayMessage("No Parameter save for this employee");
            return;
        }
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
            DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
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
                //Common Function add By Lokesh on 14-05-2015
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
    }
    protected void lbxGroupPenalty_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupPenalty.Items.Count; i++)
        {
            if (lbxGroupPenalty.Items[i].Selected == true)
            {
                GroupIds += lbxGroupPenalty.Items[i].Value + ",";
            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
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
                Session["dtEmp10"] = dtEmp;
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvEmployeePenalty, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp10"] = null;
                gvEmployeePenalty.DataSource = null;
                gvEmployeePenalty.DataBind();
            }
        }
        else
        {
            gvEmployeePenalty.DataSource = null;
            gvEmployeePenalty.DataBind();
        }
    }
    protected void lbxGroupOT_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupOT.Items.Count; i++)
        {
            if (lbxGroupOT.Items[i].Selected == true)
            {
                GroupIds += lbxGroupOT.Items[i].Value + ",";
            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
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
                Session["dtEmp5"] = dtEmp;
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvEmployeeOT, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp5"] = null;
                gvEmployeeOT.DataSource = null;
                gvEmployeeOT.DataBind();
            }
        }
        else
        {
            gvEmployeeOT.DataSource = null;
            gvEmployeeOT.DataBind();
        }
    }
    protected void lbxGroupSal_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupSal.Items.Count; i++)
        {
            if (lbxGroupSal.Items[i].Selected == true)
            {
                GroupIds += lbxGroupSal.Items[i].Value + ",";
            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
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
                Session["dtEmp4"] = dtEmp;
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvEmployeeSal, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp4"] = null;
                gvEmployeeSal.DataSource = null;
                gvEmployeeSal.DataBind();
            }
        }
        else
        {
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();
        }
    }
    protected void lbxGroupNF_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupNF.Items.Count; i++)
        {
            if (lbxGroupNF.Items[i].Selected == true)
            {
                GroupIds += lbxGroupNF.Items[i].Value + ",";
            }
        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["SessionDepId"] != null)
            {
                dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
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
                Session["dtEmp2"] = dtEmp;
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvEmployeeNF, dtEmp, "", "");
            }
            else
            {
                Session["dtEmp2"] = null;
                gvEmployeeNF.DataSource = null;
                gvEmployeeNF.DataBind();
            }
        }
        else
        {
            gvEmployeeNF.DataSource = null;
            gvEmployeeNF.DataBind();
        }
    }
    protected void EmpGroup_CheckedChanged(object sender, EventArgs e)
    {
        I6.Attributes.Add("Class", "fa fa-minus");
        Div7.Attributes.Add("Class", "box box-primary");

        I4.Attributes.Add("Class", "fa fa-minus");
        Div5.Attributes.Add("Class", "box box-primary");
        if (rbtnGroup.Checked || rbtnGroupSal.Checked)
        {
            Panel_Leave_Employee.Visible = false;
            pnlEmpSal.Visible = false;
            pnlGroup.Visible = true;
            pnlGroupSal.Visible = true;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            //-------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                if (rbtnGroup.Checked)
                {
                    objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
                    lbxGroup_SelectedIndexChanged(null, null);
                }
                if (rbtnGroupSal.Checked)
                {
                    objPageCmn.FillData((object)lbxGroupSal, dtGroup, "Group_Name", "Group_Id");
                    lbxGroupSal_SelectedIndexChanged(null, null);
                }
            }
            ddlLocationLeave.Visible = false;
            lblLocationLeave.Visible = false;
            lblDepartmentLeave.Visible = false;
            ddlDepartmentLeave.Visible = false;
        }
        else if (rbtnEmp.Checked)
        {
            pnlGroupSal.Visible = false;
            pnlEmpSal.Visible = true;
            Panel_Leave_Employee.Visible = true;
            pnlGroup.Visible = false;
            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationLeave, ddlDepartmentLeave);
            Session["dtEmpLeave"] = dtEmp;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
            lblTotalRecordsLeave.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

            ddlLocationLeave.Visible = true;
            lblLocationLeave.Visible = true;
            lblDepartmentLeave.Visible = true;
            ddlDepartmentLeave.Visible = true;
        }
    }
    protected void EmpGroupNF_CheckedChanged(object sender, EventArgs e)
    {
        I5.Attributes.Add("Class", "fa fa-minus");
        Div6.Attributes.Add("Class", "box box-primary");
        if (rbtnGroupNF.Checked)
        {
            Div_Alert_Employee.Visible = false;
            Div_Alert_Group.Visible = true;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            //-------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)lbxGroupNF, dtGroup, "Group_Name", "Group_Id");
            }
            gvEmployeeNF.DataSource = null;
            gvEmployeeNF.DataBind();
            lbxGroupNF_SelectedIndexChanged(null, null);
            lblLocationAlert.Visible = false;
            ddlLocationAlert.Visible = false;
            lblDepDeptAlert.Visible = false;
            ddlDeptAlert.Visible = false;
            //btnAllRefreshAlert.Visible = false;
        }
        else if (rbtnEmpNF.Checked)
        {
            Div_Alert_Employee.Visible = true;
            Div_Alert_Group.Visible = false;
            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationAlert, ddlDeptAlert);
            Session["dtEmpNF"] = dtEmp;
            //    //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
            lblTotalRecordNF.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblLocationAlert.Visible = true;
            ddlLocationAlert.Visible = true;
            lblDepDeptAlert.Visible = true;
            ddlDeptAlert.Visible = true;
            //btnAllRefreshAlert.Visible = true;
        }
    }
    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroupSal.Checked)
        {
            pnlEmpSal.Visible = false;
            pnlGroupSal.Visible = true;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            //-------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)lbxGroupSal, dtGroup, "Group_Name", "Group_Id");
            }
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();
            lbxGroupSal_SelectedIndexChanged(null, null);
            lblLocationSalary.Visible = false;
            ddlLocationSalary.Visible = false;
            lblDeptSalary.Visible = false;
            ddlDeptSalary.Visible = false;
            //btnAllRefreshSalary.Visible = false;
        }
        else if (rbtnEmpSal.Checked)
        {
            pnlEmpSal.Visible = true;
            pnlGroupSal.Visible = false;
            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationSalary, ddlDeptSalary);
            Session["dtEmpSal"] = dtEmp;
            objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
            lblTotalRecordSal.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            lblLocationSalary.Visible = true;
            ddlLocationSalary.Visible = true;
            lblDeptSalary.Visible = true;
            ddlDeptSalary.Visible = true;
            //btnAllRefreshSalary.Visible = true;
        }
    }
    protected void EmpPenalty_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPenaltyGroup.Checked)
        {
            PnlPenaltyEmp.Visible = false;
            PnlGroupPenalty.Visible = true;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in (" + Session["RoleId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            //-------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)lbxGroupPenalty, dtGroup, "Group_Name", "Group_Id");
            }
            gvEmployeePenalty.DataSource = null;
            gvEmployeePenalty.DataBind();
            lbxGroupPenalty_SelectedIndexChanged(null, null);
            lblLocationPenalty.Visible = false;
            ddlLocationPenalty.Visible = false;
            lblDeptPenalty.Visible = false;
            ddlDeptPenalty.Visible = false;
            //btnAllRefreshPenalty.Visible = false;
        }
        else if (rbtnPenaltyEmp.Checked)
        {
            PnlPenaltyEmp.Visible = true;
            PnlGroupPenalty.Visible = false;
            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationPenalty, ddlDeptPenalty);
            Session["dtEmpPenalty"] = dtEmp;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpPenalty, dtEmp, "", "");
            lblTotalRecordPenalty.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

            lblLocationPenalty.Visible = true;
            ddlLocationPenalty.Visible = true;
            lblDeptPenalty.Visible = true;
            ddlDeptPenalty.Visible = true;
            //btnAllRefreshPenalty.Visible = true;
        }
    }
    protected void EmpGroupOT_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnGroupOT.Checked)
        {
            pnlEmpOT.Visible = false;
            pnlGroupOT.Visible = true;
            DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
            dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            // Modified By Nitin Jain On 25-09-2014
            string data = Session["RoleId"].ToString();
            if (Session["RoleId"] != null)
            {
                dtGroup = new DataView(dtGroup, "Role_Id in ('" + Session["RoleId"].ToString() + "')", "", DataViewRowState.CurrentRows).ToTable();
            }
            //-------------------------------------
            if (dtGroup.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)lbxGroupOT, dtGroup, "Group_Name", "Group_Id");
            }
            gvEmployeeOT.DataSource = null;
            gvEmployeeOT.DataBind();
            lbxGroupOT_SelectedIndexChanged(null, null);
            lblLocationOTPL.Visible = false;
            ddlLocationOTPL.Visible = false;
            lblGroupByDept.Visible = false;
            ddlDepartmentOTPL.Visible = false;
            //btnAllRefreshOTPL.Visible = false;
        }
        else if (rbtnEmpOT.Checked)
        {
            pnlEmpOT.Visible = true;
            pnlGroupOT.Visible = false;
            lblEmp.Text = "";
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationOTPL, ddlDepartmentOTPL);
            Session["dtEmpOT"] = dtEmp;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpOT, dtEmp, "", "");
            lblTotalRecordOT.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

            lblLocationOTPL.Visible = true;
            ddlLocationOTPL.Visible = true;
            lblGroupByDept.Visible = true;
            ddlDepartmentOTPL.Visible = true;
            //btnAllRefreshOTPL.Visible = true;
        }
    }
    protected void ddlCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string Duration = string.Empty;
        string FromPer = string.Empty;
        string ToPer = string.Empty;
        if (ddlCategory.SelectedValue == "Fresher")
        {
            Duration = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            FromPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ToPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtSalIncrDuration.Text = Duration;
            txtIncrementPerFrom.Text = FromPer;
            txtIncrementPerTo.Text = ToPer;
        }
        else if (ddlCategory.SelectedValue == "Experience")
        {
            Duration = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            FromPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ToPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtSalIncrDuration.Text = Duration;
            txtIncrementPerFrom.Text = FromPer;
            txtIncrementPerTo.Text = ToPer;
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Salary_Increment_Open()", true);
    }
    protected void ddlCategory1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string Duration = string.Empty;
        string FromPer = string.Empty;
        string ToPer = string.Empty;
        if (ddlCategory1.SelectedValue == "Fresher")
        {
            Duration = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            FromPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ToPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtSalIncrDuration1.Text = Duration;
            txtIncrementPerFrom1.Text = FromPer;
            txtIncrementPerTo1.Text = ToPer;
        }
        else if (ddlCategory1.SelectedValue == "Experience")
        {
            Duration = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            FromPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            ToPer = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Experience_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtSalIncrDuration1.Text = Duration;
            txtIncrementPerFrom1.Text = FromPer;
            txtIncrementPerTo1.Text = ToPer;
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Salary_Increment_Open_Popup()", true);
    }
    public DataTable Get_Decimal_Count(string Currency_ID, string Currency_Code, string Currency_Name, string Country_Id, string Country_Name, string Country_Code)
    {
        DataTable Dt_Decimal_Count = new DataTable();
        Dt_Decimal_Count = objCurrency.GetDecimalCount(Currency_ID, Currency_Code, Currency_Name, Country_Id, Country_Name, Country_Code, "1");
        return Dt_Decimal_Count;
    }
    protected void btnEditSalary_Command(object sender, CommandEventArgs e)
    {
        string empid = e.CommandArgument.ToString();
        hdnEmpIdSal.Value = empid;
        string empname = GetEmployeeName(empid);
        lblEmpNameSal.Text = empname;
        lblEmpCodeSal.Text = GetEmployeeCode(empid);
        DataTable dtEmpSal = objEmpParam.GetEmployeeParameterByEmpId(empid, Session["CompId"].ToString());
        if (dtEmpSal.Rows.Count > 0)
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + Session["CompId"].ToString() + " and Trans_Id='" + dtEmpSal.Rows[0]["Payment_Opt_Account_ID"] + "' and IsActive='True' and Field1='False'";
            DataTable dt_account_name = objDa.return_DataTable(sql);
            if (dt_account_name != null && dt_account_name.Rows.Count > 0)
                Txt_Salary_Payment_Option_M.Text = dt_account_name.Rows[0]["AccountName"].ToString() + "/" + dt_account_name.Rows[0]["Trans_Id"].ToString() + "/";
            else
                Txt_Salary_Payment_Option_M.Text = "";
            try
            {
                DataTable dtComp = objComp.GetCompanyMasterById(Session["CompId"].ToString());
                string Currency_Id = dtComp.Rows[0]["Currency_Id"].ToString();
                ddlCurrency1.SelectedValue = Currency_Id;
            }
            catch
            {
            }
            try
            {
                ddlCurrency1.SelectedValue = dtEmpSal.Rows[0]["Currency_Id"].ToString();
            }
            catch
            {
            }
            DataTable Dt_Decimal_Count = Get_Decimal_Count(ddlCurrency1.SelectedValue.ToString(), "", "", "0", "", "");
            FillSalaryPlan(ddlSalaryPlan1);
            txtGrossSalary.Text = Convert.ToDecimal(objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtEmpSal.Rows[0]["Gross_Salary"].ToString())).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            if (IsSalaryPlanEnable())
            {
                txtBasic1.Enabled = false;
            }
            else
            {
                txtBasic1.Enabled = true;
            }
            txtBasic1.Text = Convert.ToDecimal(objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtEmpSal.Rows[0]["Basic_Salary"].ToString())).ToString(Dt_Decimal_Count.Rows[0]["Decimal_Format"].ToString());
            try
            {
                chkisCtcEmployee1.Checked = Convert.ToBoolean(dtEmpSal.Rows[0]["IsCtc_Employee"].ToString());
            }
            catch
            {
                chkisCtcEmployee1.Checked = false;
            }
            try
            {
                ddlSalaryPlan1.SelectedValue = dtEmpSal.Rows[0]["Salary_Plan_Id"].ToString();
            }
            catch
            {
            }
            string EmployeeAccountId = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            if (!String.IsNullOrEmpty(EmployeeAccountId))
            {
                string EmpQuery = "Select * from Ac_SubChartOfAccount where Company_Id = " + Session["CompId"].ToString() + " and Brand_Id = " + Session["BrandId"].ToString() + " and Location_Id = " + Session["LocId"].ToString() + " and FinancialYearId = " + Session["FinanceYearId"].ToString() + " and AccTransId = " + EmployeeAccountId + " and Other_Account_No = " + empid + "";
                DataTable Balancedt = objDa.return_DataTable(EmpQuery);
                if (Balancedt != null && Balancedt.Rows.Count > 0)
                {
                    txtEditOpeningDebitAmt.Text = double.Parse(Balancedt.Rows[0]["LDr_Amount"].ToString()).ToString();
                    txtEditOpeningCreditAmt.Text = double.Parse(Balancedt.Rows[0]["LCr_Amount"].ToString()).ToString();
                }
            }
            // txtEditOpeningCreditAmt.Text = dtEmpSal.Rows[0]["Opening_Credit"].ToString();
            // txtEditOpeningDebitAmt.Text = dtEmpSal.Rows[0]["Opening_Debit"].ToString();
            txtEditEmployerTotalEarning.Text = dtEmpSal.Rows[0]["Previous_Employer_Earning"].ToString();
            txtEdittOtalTDS.Text = dtEmpSal.Rows[0]["Previous_Employer_TDS"].ToString();
            txtMobilleBillLimit1.Text = dtEmpSal.Rows[0]["MobileBill_Limit"].ToString();
            try
            {
                ddlPaymentType1.SelectedValue = dtEmpSal.Rows[0]["Salary_Type"].ToString();
            }
            catch
            {
            }
            txtWorkMin1.Text = dtEmpSal.Rows[0]["Assign_Min"].ToString();
            try
            {
                ddlWorkCal1.SelectedValue = dtEmpSal.Rows[0]["Effective_Work_Cal_Method"].ToString();
            }
            catch
            {
            }
            if (Convert.ToBoolean(dtEmpSal.Rows[0]["Field6"].ToString()))
            {
                chkEmpINPayroll1.Checked = true;
                chkEmpPf1.Enabled = true;
                chkEmpEsic1.Enabled = true;
                try
                {
                    chkEmpPf1.Checked = Convert.ToBoolean(dtEmpSal.Rows[0]["Field4"].ToString());
                    chkEmpEsic1.Checked = Convert.ToBoolean(dtEmpSal.Rows[0]["Field5"].ToString());
                }
                catch
                {
                }
            }
            else
            {
                chkEmpINPayroll1.Checked = false;
                //chkEmpPf1.Enabled = false;
                //chkEmpEsic1.Enabled = false;
                chkEmpPf1.Checked = false;
                chkEmpEsic1.Checked = false;
            }
            try
            {
                ddlCategory1.SelectedValue = dtEmpSal.Rows[0]["Field11"].ToString();
            }
            catch
            {
            }
            txtSalIncrDuration1.Text = dtEmpSal.Rows[0]["Field8"].ToString();
            txtIncrementPerFrom1.Text = dtEmpSal.Rows[0]["Field9"].ToString();
            txtIncrementPerTo1.Text = dtEmpSal.Rows[0]["Field10"].ToString();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Salary()", true);
            //pnlSal1.Visible = true;
            ////pnlSal2.Visible = true;
            bool IsCompIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            bool IsIndemnity = false;
            try
            {
                IsIndemnity = Convert.ToBoolean(objEmpParam.GetEmployeeParameterValueByParamNameNew("IsIndemnity", empid));
            }
            catch
            {
            }
            if (IsIndemnity == true && IsCompIndemnity == true)
            {
                rbnIndemnity1PopUp.Checked = true;
                //rbnIndemnity2PopUp.Checked = false;
                txtIndemnityYearPopUp.Enabled = true;
                txtIndemnityDaysPopUP.Enabled = true;
            }
            else
            {
                // rbnIndemnity1PopUp.Checked = false;
                rbnIndemnity2PopUp.Checked = true;
                // rbnIndemnity2PopUp.Enabled = false;
                txtIndemnityYearPopUp.Enabled = true;
                txtIndemnityDaysPopUP.Enabled = true;
            }
            txtIndemnityYearPopUp.Text = objEmpParam.GetEmployeeParameterValueByParamNameNew("IndemnityYear", empid);
            txtIndemnityDaysPopUP.Text = objEmpParam.GetEmployeeParameterValueByParamNameNew("IndemnityDayas", empid);
            checkSalaryPlan();
            try
            {
                ddlSalaryPlan1.SelectedValue = dtEmpSal.Rows[0]["Salary_Plan_Id"].ToString();
            }
            catch
            {
            }
        }
        else
        {
            DisplayMessage("No Parameter save for this employee");
            return;
        }
    }
    protected void btnEditNF_Command(object sender, CommandEventArgs e)
    {
        string empid = e.CommandArgument.ToString();
        hdnEmpIdNF.Value = empid;
        string empname = GetEmployeeName(empid);
        lblEmpNameNf.Text = empname;
        lblEmpCodeNF.Text = GetEmployeeCode(empid);
        //this code is created by the jitendra upadhyay on 18-09-2014 to get the all notification dynamically(from table) and bind using checkbox list control
        //code start
        DataTable dt = objEmpNotice.GetAllNotification_By_NOtificationType("SMS");
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ChkSmsList_popup, dt, "Notification_Name", "Notification_Id");
        }
        dt = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ChkReportList_popup, dt, "Notification_Name", "Notification_Id");
        }
        dt = objEmpNotice.GetAllNotification_By_NOtificationType("Email");
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ChkEmailList_popup, dt, "Notification_Name", "Notification_Id");
        }
        int RefId = 0;
        //code created on 27-09-2014 by jitendar upadhyay
        //this code for insert the sms and email notification in Reportmaster ,reportnotification and reportsetup table
        //code start
        DataTable dtReportMaster = objReportMaster.GetReportMasterByEmpId(empid);
        if (dtReportMaster.Rows.Count > 0)
        {
            RefId = Convert.ToInt32(dtReportMaster.Rows[0]["Trans_Id"].ToString());
            txtScheduleDays_Popup.Text = dtReportMaster.Rows[0]["Schedule_Days"].ToString();
            foreach (ListItem item in ChkSmsList_popup.Items)
            {
                DataTable dtNF = objReportNotification.GetRecord_By_RefId_and_NotificationId(RefId.ToString(), item.Value);
                if (dtNF.Rows.Count > 0)
                {
                    item.Selected = true;
                }
            }
            foreach (ListItem item in ChkEmailList_popup.Items)
            {
                DataTable dtNF = objReportNotification.GetRecord_By_RefId_and_NotificationId(RefId.ToString(), item.Value);
                if (dtNF.Rows.Count > 0)
                {
                    item.Selected = true;
                }
            }
        }
        foreach (ListItem item in ChkReportList_popup.Items)
        {
            DataTable dtNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(empid, item.Value);
            if (dtNF.Rows.Count > 0)
            {
                item.Selected = true;
            }
        }
        //code end
        if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("SMS_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            ChkSmsList_popup.Enabled = false;
        }
        if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Email_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            ChkEmailList_popup.Enabled = false;
        }
        //ModalPopupExtender3.Show();
        //pnlNotice1.Visible = true;
        //pnlNotice2.Visible = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Alert()", true);
    }
    protected void btnOTPartial_Click(object sender, EventArgs e)
    {
        //ddlLocationOTPL.SelectedIndex = 0;
        ddlDepartmentOTPL.SelectedIndex = 0;
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlEmpSalary.Visible = false;
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        ViewState["Select"] = null;
        //ImgbtnSelectAll.Visible = false;
        //imgBtnRestore.Visible = false;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = false;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = false;
        imgBtnbinGrid.Visible = false;
        imgBtnbinDatalist.Visible = false;
        lblSelectRecordNF.Text = "";
        gvEmpLeave.Visible = false;
        gvEmpSalary.Visible = false;
        gvEmpOT.Visible = true;
        rbtnEmpOT.Checked = true;
        rbtnGroupOT.Checked = false;
        EmpGroupOT_CheckedChanged(null, null);
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbtnPartialEnable.Checked = true;
            rbtPartial_OnCheckedChanged(null, null);
            txtTotalMinutes.Text = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtMinuteday.Text = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Carry Forward Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                rbtnCarryYes.Checked = true;
                rbtnCarryNo.Checked = false;
            }
            else
            {
                rbtnCarryYes.Checked = false;
                rbtnCarryNo.Checked = true;
            }
        }
        else
        {
            rbtnPartialEnable.Checked = false;
            rbtnPartialDisable.Checked = true;
            rbtPartial_OnCheckedChanged(null, null);
            txtTotalMinutes.Text = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            txtMinuteday.Text = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Carry Forward Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
            {
                rbtnCarryYes.Checked = true;
                rbtnCarryNo.Checked = false;
            }
            else
            {
                rbtnCarryYes.Checked = false;
                rbtnCarryNo.Checked = true;
            }
            rbtnPartialEnable.Enabled = false;
            rbtnPartialDisable.Enabled = true;
        }
        ddlLocationOTPL.Focus();
        AllPageCode();
        Session["CHECKED_ITEMS"] = null;
        if (Hdn_Edit.Value != "")
        {
            ddlFieldOT.SelectedValue = "Emp_Code";
            txtValueOT.Text = Hdn_Edit.Value;
            btnOTbind_Click(null, null);
        }
    }
    protected void btnPanelty_Click(object sender, EventArgs e)
    {
        //ddlLocationPenalty.SelectedIndex = 0;
        ddlDeptPenalty.SelectedIndex = 0;
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        PnlEmpSalary.Visible = false;
        ViewState["Select"] = null;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = false;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = false;
        imgBtnbinGrid.Visible = false;
        imgBtnbinDatalist.Visible = false;
        lblSelectRecordNF.Text = "";
        gvEmpLeave.Visible = false;
        gvEmpSalary.Visible = false;
        gvEmpPenalty.Visible = true;
        rbtnPenaltyEmp.Checked = true;
        rbtnPenaltyGroup.Checked = false;
        EmpPenalty_CheckedChanged(null, null);
        rbtLateInEnable.Checked = true;
        rbtLateInDisable.Checked = false;
        rbtEarlyOutEnable.Checked = true;
        rbtEarlyOutDisable.Checked = false;
        rbtnAbsentDisable.Checked = false;
        rbtnAbsentEnable.Checked = true;
        rbtnPartialLeaveEnable.Checked = true;
        rbtnPartialLeaveDisable.Checked = false;
        ddlLocationPenalty.Focus();
        AllPageCode();
        Session["CHECKED_ITEMS"] = null;
        if (Hdn_Edit.Value != "")
        {
            ddlFieldPenalty.SelectedValue = "Emp_Code";
            txtValuePenalty.Text = Hdn_Edit.Value;
            btnPenaltybind_Click(null, null);
        }
        gvEmpPenalty.Visible = true;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        PnlEmpSalary.Visible = false;

        //PnlEmployeeLeave.Visible = false;
        ViewState["Select"] = null;
        //ImgbtnSelectAll.Visible = false;
        //imgBtnRestore.Visible = false;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = false;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;

        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = false;
        imgBtnbinGrid.Visible = false;
        imgBtnbinDatalist.Visible = false;
        lblSelectRecordNF.Text = "";
        gvEmpLeave.Visible = false;
        gvEmpSalary.Visible = false;
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

    }
    protected void chkEmpINPayroll_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkEmpINPayroll.Checked)
        {
            chkEmpPf.Enabled = true;
            chkEmpEsic.Enabled = true;
            chkEmpPf.Checked = false;
            chkEmpEsic.Checked = false;
        }
        else
        {
            chkEmpPf.Enabled = false;
            chkEmpEsic.Enabled = false;
            chkEmpPf.Checked = false;
            chkEmpEsic.Checked = false;
        }
    }
    protected void chkEmpINPayroll_OnCheckedChanged1(object sender, EventArgs e)
    {
        if (chkEmpINPayroll1.Checked)
        {
            //chkEmpPf1.Enabled = true;
            //chkEmpEsic1.Enabled = true;
            chkEmpPf1.Checked = false;
            chkEmpEsic1.Checked = false;
        }
        else
        {
            //chkEmpPf1.Enabled = false;
            //chkEmpEsic1.Enabled = false;
            chkEmpPf1.Checked = false;
            chkEmpEsic1.Checked = false;
        }
    }
    protected void btnSalary_Click(object sender, EventArgs e)
    {
        //ddlLocationSalary.SelectedIndex = 0;
        ddlDeptSalary.SelectedIndex = 0;
        chkEmpINPayroll.Checked = true;
        chkEmpPf.Enabled = true;
        chkEmpEsic.Enabled = true;
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        PnlEmpSalary.Visible = true;
        //PnlEmployeeLeave.Visible = false;
        ViewState["Select"] = null;
        //ImgbtnSelectAll.Visible = false;
        //imgBtnRestore.Visible = false;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = false;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = false;
        imgBtnbinGrid.Visible = false;
        imgBtnbinDatalist.Visible = false;
        lblSelectRecordNF.Text = "";
        gvEmpLeave.Visible = false;
        gvEmpSalary.Visible = true;
        rbtnEmpSal.Checked = true;
        rbtnGroupSal.Checked = false;
        EmpGroupSal_CheckedChanged(null, null);
        ddlCategory.SelectedValue = "Fresher";
        ddlCategory_OnSelectedIndexChanged(null, null);
        ddlLocationSalary.Focus();
        AllPageCode();
        Session["CHECKED_ITEMS"] = null;
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            rbnIndemnity1_OnCheckedChanged(null, null);
            rbnIndemnity1.Checked = true;
            //txtSalGIven.Enabled = true;
            //txtNextIndemnity.Enabled = true;
        }
        else
        {
            rbnIndemnity2_OnCheckedChanged(null, null);
            rbnIndemnity2.Checked = true;
            //txtSalGIven.Enabled = false;
            //txtNextIndemnity.Enabled = false;
            //rbnIndemnity1.Enabled = false;
            txtIndemnityYear.Text = objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        checkSalaryPlan();
        if (Hdn_Edit.Value != "")
        {
            ddlFieldSal.SelectedValue = "Emp_Code";
            txtValueSal.Text = Hdn_Edit.Value;
            btnSalarybind_Click(null, null);
        }
    }
    #region SalaryPlanFunctionalityChecking
    public bool IsSalaryPlanEnable()
    {
        return Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsSalaryPlanEnable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
    }
    public void checkSalaryPlan()
    {
        //here we are checking that salary plan is enabled or not 
        if (IsSalaryPlanEnable())
        {
            RequiredFieldValidator14.ErrorMessage = "Enter Gross Salary";
            Div_Salary_Plan.Attributes.Add("style", "");
            RequiredFieldValidator26.ValidationGroup = "S_Save";
            RequiredFieldValidator24.ErrorMessage = "Enter Gross Salary";
            Div_Salary_Plan1.Attributes.Add("style", "");
            Div_Basic_Salary.Attributes.Add("style", "");
            RequiredFieldValidator27.ValidationGroup = "S_update";
            Label27.Text = "Gross Salary";
            Label9.Text = "Gross Salary";
            ddlSalaryPlan1.SelectedValue = "--Select--";
        }
        else
        {
            ddlSalaryPlan1.SelectedValue = "--Select--";
            RequiredFieldValidator14.ErrorMessage = "Enter Basic Salary";
            Div_Salary_Plan.Attributes.Add("style", "display:none");
            RequiredFieldValidator26.ValidationGroup = "";
            RequiredFieldValidator24.ErrorMessage = "Enter Basic Salary";
            Div_Salary_Plan1.Attributes.Add("style", "display:none");
            //added by jitendra upadhyay on 12-03-2018
            //always gross salary textbox was showing doesn't matter salary plan enable or disable 
            Div_GrossSalary_1.Attributes.Add("style", "display:none");
            Div_Basic_Salary.Attributes.Add("style", "");
            //Div_GrossSalary_1.Attributes.Add("style", "display:none");
            RequiredFieldValidator27.ValidationGroup = "";
            Label27.Text = "Basic Salary";
            Label9.Text = "Basic Salary";
        }
        FillSalaryPlan(ddlSalaryPlan);
    }
    public void FillSalaryPlan(DropDownList ddl)
    {
        DataTable dt = ObjSalaryPlan.GetRecordTrueAll(Session["CompId"].ToString());
        objPageCmn.FillData((object)ddl, dt, "Plan_Name", "Trans_id");
    }
    public string GetBasicSalaryBysalaryPlanId(string strSalaryPlanId, string strGrossAmount)
    {
        string strbasic = string.Empty;
        double BasicPercentage = 0;
        double grossamt = 0;
        try
        {
            BasicPercentage = Convert.ToDouble(ObjSalaryPlan.GetRecordTruebyId(Session["CompId"].ToString(), strSalaryPlanId).Rows[0]["Basic_Salary_Percentage"].ToString());
        }
        catch
        {
        }
        try
        {
            grossamt = Convert.ToDouble(strGrossAmount);
        }
        catch
        {
        }
        strbasic = objSys.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), ((grossamt * BasicPercentage) / 100).ToString());
        return strbasic;
    }
    #endregion
    protected void btnNotice_Click(object sender, EventArgs e)
    {
        //ddlLocationAlert.SelectedIndex = 0;
        ddlDeptAlert.SelectedIndex = 0;
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        gvEmpAssign.Visible = false;
        gvEmpSalary.Visible = false;
        PnlEmpSalary.Visible = false;
        //PnlEmployeeLeave.Visible = false;
        ViewState["Select"] = null;
        //ImgbtnSelectAll.Visible = false;
        //imgBtnRestore.Visible = false;
        dtlistbinEmp.Visible = false;
        gvBinEmp.Visible = false;
        ViewState["CurrIndexbin"] = 0;
        ViewState["SubSizebin"] = 9;
        lnkbinFirst.Visible = false;
        lnkbinPrev.Visible = false;
        lnkbinNext.Visible = false;
        lnkbinLast.Visible = false;
        lnkFirst.Visible = false;
        lnkLast.Visible = false;
        lnkNext.Visible = false;
        lnkPrev.Visible = false;
        imgBtnbinGrid.Visible = false;
        imgBtnbinDatalist.Visible = false;
        lblSelectRecordNF.Text = "";
        gvEmpLeave.Visible = false;
        gvEmpNF.Visible = true;

        BindNotification();
        rbtnEmpNF.Checked = true;
        rbtnGroupNF.Checked = false;
        EmpGroupNF_CheckedChanged(null, null);
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
        if (Hdn_Edit.Value != "")
        {
            ddlFieldNF.SelectedValue = "Emp_Code";
            txtValueNF.Text = Hdn_Edit.Value;
            btnNFbind_Click(null, null);
        }
    }
    protected void btnUpdatePenalty_Click(object sender, EventArgs e)
    {
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        int b = 0;
        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpIdPenalty.Value, Session["CompId"].ToString());
        //objEmpParam.DeleteEmployeeParameterByEmpId(hdnEmpIdPenalty.Value);
        bool IsLate = false;
        bool IsEarly = false;
        bool IsAbsent = false;
        bool IsPartial = false;
        if (rbtnLateInEnable.Checked)
        {
            IsLate = true;
        }
        else
        {
            IsLate = false;
        }
        if (rbtnEarlyEnable.Checked)
        {
            IsEarly = true;
        }
        else
        {
            IsEarly = false;
        }
        if (rbtnAbsentEnableP.Checked)
        {
            IsAbsent = true;
        }
        else
        {
            IsAbsent = false;
        }
        if (PnlPlEnable.Checked)
        {
            IsPartial = true;
        }
        else
        {
            IsPartial = false;
        }
        if (dt.Rows.Count > 0)
        {
            if (IsLate || IsEarly || IsAbsent || IsPartial)
            {
                if (dt.Rows[0]["Type"].ToString().ToUpper() == "MANAGER" || dt.Rows[0]["Type"].ToString().ToUpper() == "CEO")
                {
                    DisplayMessage("You can not enable penalty for CEO and Manager");
                    return;
                }
            }
            b = objDa.execute_Command("update Set_Employee_Parameter set Field1='" + IsLate.ToString() + "',Field2='" + IsEarly.ToString() + "',Field3='" + IsAbsent.ToString() + "',Field13='" + IsPartial.ToString() + "' where Emp_id=" + hdnEmpIdPenalty.Value + "");
        }
        //b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), hdnEmpIdPenalty.Value, dt.Rows[0]["Basic_Salary"].ToString(), dt.Rows[0]["Salary_Type"].ToString(), dt.Rows[0]["Currency_Id"].ToString(), dt.Rows[0]["Assign_Min"].ToString(), dt.Rows[0]["Effective_Work_Cal_Method"].ToString(), dt.Rows[0]["Is_OverTime"].ToString(), dt.Rows[0]["Normal_OT_Method"].ToString(), dt.Rows[0]["Normal_OT_Type"].ToString(), dt.Rows[0]["Normal_OT_Value"].ToString(), dt.Rows[0]["Normal_HOT_Type"].ToString(), dt.Rows[0]["Normal_HOT_Value"].ToString(), dt.Rows[0]["Normal_WOT_Type"].ToString(), dt.Rows[0]["Normal_WOT_Value"].ToString(), dt.Rows[0]["Is_Partial_Enable"].ToString(), dt.Rows[0]["Partial_Leave_Mins"].ToString(), dt.Rows[0]["Partial_Leave_Day"].ToString(), dt.Rows[0]["Is_Partial_Carry"].ToString(), IsLate.ToString(), IsEarly.ToString(), IsAbsent.ToString(), "", "", true.ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), IsPartial.ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), dt.Rows[0]["Payment_Opt_Account_ID"].ToString());
        SystemLog.SaveSystemLog("Employee Master : Penalty", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Penalty Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        if (b != 0)
        {
            DisplayMessage("Record Updated", "green");
        }
    }
    protected void btnUpdateOT_Click(object sender, EventArgs e)
    {
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        //here we set the validation that total minute for month is not greater than company parameter
        //and aso set validation for minute used in one day that this is not greater than total minute for month
        //this code is created by jitendra upadhyay pn 19-08-2014
        //code start
        if (txtTotalMinutesP1.Text.Trim() == "")
        {
            DisplayMessage("Enter Total Minute For Month");
            txtTotalMinutesP1.Focus();
            return;
        }
        if (txtMinuteOTOne.Text.Trim() == "")
        {
            DisplayMessage("Enter Minute Use in a Day One Time");
            txtMinuteOTOne.Focus();
            return;
        }
        double TotalComapany_PartialMinute = 0;
        TotalComapany_PartialMinute = float.Parse(objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (float.Parse(txtTotalMinutesP1.Text) > TotalComapany_PartialMinute)
        {
            DisplayMessage("Total Minute For Month Should be Less Than or Equal to " + TotalComapany_PartialMinute.ToString());
            txtTotalMinutesP1.Focus();
            return;
        }
        if (float.Parse(txtTotalMinutesP1.Text) < float.Parse(txtMinuteOTOne.Text))
        {
            DisplayMessage("Minute use in a Day For one Time Should be Less Than or Equal to Total Minute For Month");
            txtMinuteOTOne.Focus();
            return;
        }
        //code end
        int b = 0;
        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpIdOt.Value, Session["CompId"].ToString());
        objEmpParam.DeleteEmployeeParameterByEmpId(hdnEmpIdOt.Value);
        string strCarry = string.Empty;
        string OtEmp = string.Empty;
        string partialEnable = string.Empty;
        if (rbtnPartialEnable1.Checked)
        {
            partialEnable = "True";
        }
        else
        {
            partialEnable = "False";
        }
        if (rbtnOTEnable1.Checked)
        {
            OtEmp = "True";
        }
        else
        {
            OtEmp = "False";
        }
        if (rbtnCarryYes1.Checked)
        {
            strCarry = "True";
        }
        else
        {
            strCarry = "False";
        }
        b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), hdnEmpIdOt.Value, dt.Rows[0]["Basic_Salary"].ToString(), dt.Rows[0]["Salary_Type"].ToString(), dt.Rows[0]["Currency_Id"].ToString(), dt.Rows[0]["Assign_Min"].ToString(), dt.Rows[0]["Effective_Work_Cal_Method"].ToString(), OtEmp, ddlOTCalc1.SelectedValue, ddlNormalType1.SelectedValue, GetText(txtNormal1.Text), ddlHolidayValue1.SelectedValue, GetText(txtHolidayValue1.Text), ddlWeekOffType1.SelectedValue, GetText(txtWeekOffValue1.Text), partialEnable, GetText(txtTotalMinutesP1.Text), GetText(txtMinuteOTOne.Text), strCarry, dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), "", "", true.ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), dt.Rows[0]["Payment_Opt_Account_ID"].ToString());
        SystemLog.SaveSystemLog("Employee Master : OT/PL", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "OT/PL Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        if (b != 0)
        {
            DisplayMessage("Record Updated", "green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_OT_PL()", true);
        }
    }
    public Boolean Salary_Plan_Check(string Plan_Id, string Basic_Salary, string Gross_Salary)
    {
        double BasicAmount = 0;
        double TotalAmount = 0;
        BasicAmount = Convert.ToDouble(Basic_Salary);
        TotalAmount = Convert.ToDouble(Basic_Salary);
        DataTable dttDetail = Objsalaryplandetail.GetDeduction_By_headerId(Plan_Id);
        if (dttDetail != null && dttDetail.Rows.Count > 0)
        {
            foreach (DataRow DR in dttDetail.Rows)
            {
                if (DR["Calculation_Method"].ToString() == "Percent")
                {
                    TotalAmount = TotalAmount + ((BasicAmount * Convert.ToDouble(DR["Value"].ToString())) / 100);
                }
                else
                {
                    if (DR["Field1"].ToString() == "True")
                    {
                        TotalAmount = TotalAmount + (Convert.ToDouble(Gross_Salary) - TotalAmount);
                    }
                    else
                    {
                        TotalAmount = TotalAmount + Convert.ToDouble(DR["Value"].ToString());
                    }
                }
                if (TotalAmount > Convert.ToDouble(Gross_Salary))
                {
                    return false;
                }
            }
        }
        if (TotalAmount == Convert.ToDouble(Gross_Salary))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    protected void btnUpdateSal_Click(object sender, EventArgs e)
    {
        if (IsSalaryPlanEnable())
        {
            if (txtGrossSalary.Text == "")
            {
                DisplayMessage("Enter Gross Salary");
                txtGrossSalary.Focus();
                return;
            }
            if (ddlSalaryPlan1.SelectedValue.ToString() == "--Select--")
            {
                DisplayMessage("Select Salary Plan");
                ddlSalaryPlan1.Focus();
                return;
            }
            if (txtWorkMin1.Text == "")
            {
                DisplayMessage("Enter Work Minute");
                txtWorkMin1.Focus();
                return;
            }
            if (ddlCurrency1.SelectedIndex == 0)
            {
                DisplayMessage("Select Currency");
                ddlCurrency1.Focus();
                return;
            }
            ddlSalaryPlan1_SelectedIndexChanged(null, null);
        }
        else
        {
            if (txtBasic1.Text == "")
            {
                DisplayMessage("Enter Basic Salary");
                txtBasic1.Focus();
                return;
            }
            else
            {
                txtGrossSalary.Text = txtBasic1.Text;
            }
            if (txtWorkMin1.Text == "")
            {
                DisplayMessage("Enter Work Minute");
                txtWorkMin1.Focus();
                return;
            }
            if (ddlCurrency1.SelectedIndex == 0)
            {
                DisplayMessage("Select Currency");
                ddlCurrency1.Focus();
                return;
            }
        }
        string SalaryPlanId = string.Empty;
        string strGrossSalary = string.Empty;
        string strBasicSalary = string.Empty;
        if (ddlSalaryPlan1.SelectedIndex > 0)
        {
            SalaryPlanId = ddlSalaryPlan1.SelectedValue;
        }
        if (IsSalaryPlanEnable())
        {
            strGrossSalary = txtGrossSalary.Text;
            strBasicSalary = GetBasicSalaryBysalaryPlanId(SalaryPlanId, strGrossSalary);
            if (Salary_Plan_Check(SalaryPlanId, txtBasic1.Text, txtGrossSalary.Text) == false)
            {
                DisplayMessage("This Salary plan is not applicable for salary - " + txtGrossSalary.Text + "");
                return;
            }
        }
        else
        {
            strGrossSalary = txtGrossSalary.Text;
            strBasicSalary = txtBasic1.Text;
        }
        ddlCurrency1.SelectedValue = ObjLocMaster.Get_Currency_By_Location_ID(Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Currency_Id"].ToString();
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option_M.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option_M.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        if (txtEditOpeningCreditAmt.Text == "")
        {
            txtEditOpeningCreditAmt.Text = "0";
        }
        if (txtEditOpeningDebitAmt.Text == "")
        {
            txtEditOpeningDebitAmt.Text = "0";
        }
        if (txtEditEmployerTotalEarning.Text == "")
        {
            txtEditEmployerTotalEarning.Text = "0";
        }
        if (txtEdittOtalTDS.Text == "")
        {
            txtEdittOtalTDS.Text = "0";
        }
        bool IsOpeningBal = false;
        if (Convert.ToDouble(txtEditOpeningCreditAmt.Text.ToString()) > 0)
        {
            IsOpeningBal = true;
        }
        else if (Convert.ToDouble(txtEditOpeningDebitAmt.Text.ToString()) > 0)
        {
            IsOpeningBal = true;
        }
        if (IsOpeningBal)
        {
            string EmpAcc = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            if (EmpAcc == "0")
            {
                DisplayMessage("Please configure Employee Account in Finance");
                return;
            }
        }
        bool EmpInPayroll = false;
        bool EmpPF = false;
        bool EmpESIC = false;
        if (chkEmpINPayroll1.Checked)
        {
            EmpPF = chkEmpPf1.Checked;
            EmpESIC = chkEmpEsic1.Checked;
            EmpInPayroll = true;
        }
        else
        {
            EmpInPayroll = false;
            EmpPF = false;
            EmpESIC = false;
        }
        if (txtSalIncrDuration1.Text == "")
        {
            txtSalIncrDuration1.Text = "0";
        }
        if (txtIncrementPerFrom1.Text == "")
        {
            txtIncrementPerFrom1.Text = "0";
        }
        if (txtIncrementPerTo1.Text == "")
        {
            txtIncrementPerTo1.Text = "0";
        }
        int b = 0;
        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(hdnEmpIdSal.Value, Session["CompId"].ToString());
        objEmpParam.DeleteEmployeeParameterByEmpId(hdnEmpIdSal.Value);
        b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), hdnEmpIdSal.Value, strBasicSalary, ddlPaymentType1.SelectedValue, ddlCurrency1.SelectedValue, txtWorkMin1.Text, ddlWorkCal1.SelectedValue, dt.Rows[0]["Is_OverTime"].ToString(), dt.Rows[0]["Normal_OT_Method"].ToString(), dt.Rows[0]["Normal_OT_Type"].ToString(), dt.Rows[0]["Normal_OT_Value"].ToString(), dt.Rows[0]["Normal_HOT_Type"].ToString(), dt.Rows[0]["Normal_HOT_Value"].ToString(), dt.Rows[0]["Normal_WOT_Type"].ToString(), dt.Rows[0]["Normal_WOT_Value"].ToString(), dt.Rows[0]["Is_Partial_Enable"].ToString(), dt.Rows[0]["Partial_Leave_Mins"].ToString(), dt.Rows[0]["Partial_Leave_Day"].ToString(), dt.Rows[0]["Is_Partial_Carry"].ToString(), dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), EmpPF.ToString(), EmpESIC.ToString(), EmpInPayroll.ToString(), DateTime.Now.ToString(), txtSalIncrDuration1.Text, txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, ddlCategory1.SelectedValue, dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strGrossSalary, chkisCtcEmployee1.Checked.ToString(), txtMobilleBillLimit1.Text, SalaryPlanId, txtEditOpeningCreditAmt.Text, txtEditOpeningDebitAmt.Text, txtEditEmployerTotalEarning.Text, txtEdittOtalTDS.Text, "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
        SystemLog.SaveSystemLog("Employee Master : Salary", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Salary Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        if (b != 0)
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), hdnEmpIdSal.Value);
            if (dtEmp.Rows.Count > 0)
            {
                if (dtEmp.Rows[0]["DOB"].ToString() != "")
                {
                    DataTable dtSal = objEmpSalInc.GetEmpSalaryIncrementByEmpId(Session["CompId"].ToString(), hdnEmpIdSal.Value);
                    if (dtSal.Rows.Count == 0 || dtSal.Rows.Count == 1)
                    {
                        DateTime JoinDate = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());
                        double IncrementPer = 0;
                        try
                        {
                            IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                        }
                        catch
                        {
                        }
                        double BasicSal = 0;
                        try
                        {
                            BasicSal = double.Parse(txtBasic1.Text);
                        }
                        catch
                        {
                        }
                        double IncrementValue = 0;
                        try
                        {
                            IncrementValue = (BasicSal * IncrementPer) / 100;
                        }
                        catch
                        {
                        }
                        double IncrementSalary = 0;
                        int Duration = 0;
                        try
                        {
                            Duration = int.Parse(txtSalIncrDuration1.Text);
                        }
                        catch
                        {
                        }
                        IncrementSalary = BasicSal + IncrementValue;
                        DateTime IncrementDate = JoinDate.AddMonths(Duration);
                        objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(hdnEmpIdSal.Value);
                        objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else
                    {
                        DateTime IncrementDate = new DateTime();
                        if (dtSal.Rows.Count > 1)
                        {
                            IncrementDate = new DateTime(int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Year"].ToString()), int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Month"].ToString()), 1);
                        }
                        else
                        {
                            IncrementDate = new DateTime(int.Parse(dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString()), int.Parse(dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString()), 1);
                        }
                        if (IncrementDate.Year > DateTime.Now.Year)
                        {
                            if (dtSal.Rows.Count > 1)
                            {
                                DateTime JoinDate = new DateTime(int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Year"].ToString()), int.Parse(dtSal.Rows[dt.Rows.Count - 2]["Month"].ToString()), 1);
                                double IncrementPer = 0;
                                try
                                {
                                    IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                }
                                catch
                                {
                                }
                                double BasicSal = 0;
                                try
                                {
                                    BasicSal = double.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Increment_Salary"].ToString());
                                }
                                catch
                                {
                                }
                                double IncrementValue = 0;
                                try
                                {
                                    IncrementValue = (BasicSal * IncrementPer) / 100;
                                }
                                catch
                                {
                                }
                                double IncrementSalary = 0;
                                int Duration = 0;
                                try
                                {
                                    Duration = int.Parse(txtSalIncrDuration1.Text);
                                }
                                catch
                                {
                                }
                                IncrementSalary = BasicSal + IncrementValue;
                                IncrementDate = JoinDate.AddMonths(Duration);
                                objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }
                            else
                            {
                                DateTime JoinDate = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());
                                try
                                {
                                    double IncrementPer = 0;
                                    IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                    double BasicSal = 0;
                                    BasicSal = double.Parse(txtBasic1.Text);
                                    double IncrementValue = 0;
                                    IncrementValue = (BasicSal * IncrementPer) / 100;
                                    double IncrementSalary = 0;
                                    int Duration = 0;
                                    Duration = int.Parse(txtSalIncrDuration1.Text);
                                    IncrementSalary = BasicSal + IncrementValue;
                                    IncrementDate = JoinDate.AddMonths(Duration);
                                    objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                    objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                catch
                                {
                                }
                            }
                            //
                        }
                        else if (IncrementDate.Year == DateTime.Now.Year)
                        {
                            if (IncrementDate.Month < DateTime.Now.Month)
                            {
                                if (dtSal.Rows.Count > 1)
                                {
                                    try
                                    {
                                        DateTime JoinDate = new DateTime(int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Year"].ToString()), int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Month"].ToString()), 1);
                                        double IncrementPer = 0;
                                        IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                        double BasicSal = 0;
                                        BasicSal = double.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Increment_Salary"].ToString());
                                        double IncrementValue = 0;
                                        IncrementValue = (BasicSal * IncrementPer) / 100;
                                        double IncrementSalary = 0;
                                        int Duration = 0;
                                        Duration = int.Parse(txtSalIncrDuration1.Text);
                                        IncrementSalary = BasicSal + IncrementValue;
                                        IncrementDate = JoinDate.AddMonths(Duration);
                                        objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                        objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    DateTime JoinDate = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());
                                    try
                                    {
                                        double IncrementPer = 0;
                                        IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                        double BasicSal = 0;
                                        BasicSal = double.Parse(txtBasic1.Text);
                                        double IncrementValue = 0;
                                        IncrementValue = (BasicSal * IncrementPer) / 100;
                                        double IncrementSalary = 0;
                                        int Duration = 0;
                                        Duration = int.Parse(txtSalIncrDuration1.Text);
                                        IncrementSalary = BasicSal + IncrementValue;
                                        IncrementDate = JoinDate.AddMonths(Duration);
                                        objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                        objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                                //
                            }
                        }
                        else if (IncrementDate.Year < DateTime.Now.Year)
                        {
                            if (IncrementDate.Month > DateTime.Now.Month)
                            {
                                if (dtSal.Rows.Count > 1)
                                {
                                    DateTime JoinDate = new DateTime(int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Year"].ToString()), int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Month"].ToString()), 1);
                                    try
                                    {
                                        double IncrementPer = 0;
                                        IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                        double BasicSal = 0;
                                        BasicSal = double.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Increment_Salary"].ToString());
                                        double IncrementValue = 0;
                                        IncrementValue = (BasicSal * IncrementPer) / 100;
                                        double IncrementSalary = 0;
                                        int Duration = 0;
                                        Duration = int.Parse(txtSalIncrDuration1.Text);
                                        IncrementSalary = BasicSal + IncrementValue;
                                        IncrementDate = JoinDate.AddMonths(Duration);
                                        objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                        objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                                else
                                {
                                    DateTime JoinDate = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());
                                    double IncrementPer = 0;
                                    try
                                    {
                                        IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                    }
                                    catch
                                    {
                                    }
                                    double BasicSal = 0;
                                    try
                                    {
                                        BasicSal = double.Parse(txtBasic1.Text);
                                    }
                                    catch
                                    {
                                    }
                                    double IncrementValue = 0;
                                    try
                                    {
                                        IncrementValue = (BasicSal * IncrementPer) / 100;
                                    }
                                    catch
                                    {
                                    }
                                    double IncrementSalary = 0;
                                    int Duration = 0;
                                    try
                                    {
                                        Duration = int.Parse(txtSalIncrDuration1.Text);
                                    }
                                    catch
                                    {
                                    }
                                    IncrementSalary = BasicSal + IncrementValue;
                                    IncrementDate = JoinDate.AddMonths(Duration);
                                    objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                    objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                //
                            }
                            else
                            {
                                if (dtSal.Rows.Count > 1)
                                {
                                    DateTime JoinDate = new DateTime(int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Year"].ToString()), int.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Month"].ToString()), 1);
                                    try
                                    {
                                        double IncrementPer = 0;
                                        IncrementPer = double.Parse(txtIncrementPerTo1.Text);
                                        double BasicSal = 0;
                                        BasicSal = double.Parse(dtSal.Rows[dtSal.Rows.Count - 2]["Increment_Salary"].ToString());
                                        double IncrementValue = 0;
                                        IncrementValue = (BasicSal * IncrementPer) / 100;
                                        double IncrementSalary = 0;
                                        int Duration = 0;
                                        Duration = int.Parse(txtSalIncrDuration1.Text);
                                        IncrementSalary = BasicSal + IncrementValue;
                                        IncrementDate = JoinDate.AddMonths(Duration);
                                        objEmpSalInc.DeleteEmpSalaryIncrement(hdnEmpIdSal.Value, dtSal.Rows[dtSal.Rows.Count - 1]["Month"].ToString(), dtSal.Rows[dtSal.Rows.Count - 1]["Year"].ToString());
                                        objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                // Indemnity Code here.....
                bool IsIndemnity = false;
                if (rbnIndemnity1PopUp.Checked == true)
                {
                    IsIndemnity = true;
                }
                else
                {
                    IsIndemnity = false;
                }
                int c = 0;
                int d = 0;
                int f = 0;
                // Is Indemnity
                objEmpParam.DeleteEmployeeParameterByEmpIdNew(hdnEmpIdSal.Value);
                c = objEmpParam.InsertEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                // Indemnity Duration
                d = objEmpParam.InsertEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "IndemnityYear", txtIndemnityYearPopUp.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                f = objEmpParam.InsertEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "IndemnityDayas", txtIndemnityDaysPopUP.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                // if Drop Down Selection is Salary Type.. ddlIndenityType
                // b = objEmpParam.UpdateEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "Indemenity_SalaryCalculationType", ddlIndenityTypePopUp.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                //if (ddlIndenityType.SelectedValue == "1")
                //{
                //    b = objEmpParam.UpdateEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "Indemnity_GivenType", ddlIndemnitySalaryPopUp.SelectedValue, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                //    b = objEmpParam.UpdateEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "IndemnitySalaryValue", txtSalGIvenPopUp.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                //}
                //else
                //{
                //    b = objEmpParam.UpdateEmpParameterNew(hdnEmpIdSal.Value, Session["CompId"].ToString(), "IndemnityLeave", txtIndemnityLeavePopUp.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                //}
                //.............................
                //updating opening balance
                //opening balance entry
                //objDa.execute_Command("update Pay_EmployeeSalaryStatement set  where Emp_id=" + hdnEmpIdSal.Value + " and Is_Opening_Balance='True'");
                objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), hdnEmpIdSal.Value, DateTime.Now.ToString(), txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, Session["CurrencyId"].ToString(), txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, "Pay_EmployeeAdvancePayment", "0", "0", "Opening Balance", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), true.ToString());
                // Insert and Update Employee Opening Balance
                // Count = 1 then Update OtherWise Insert
                // Insert is happen only once for every employee
                string EmployeeAccountId = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                double DebAmt = Convert.ToDouble(txtEditOpeningDebitAmt.Text.ToString());
                double CreAmt = Convert.ToDouble(txtEditOpeningCreditAmt.Text.ToString());
                if (EmployeeAccountId != "0" && (DebAmt > 0 || CreAmt > 0))
                {
                    string EmpQuery = "Select COUNT(*) Count,ISNULL(Trans_Id,0) Id from Ac_SubChartOfAccount where Company_Id = " + Session["CompId"].ToString() + " and Brand_Id = " + Session["BrandId"].ToString() + " and Location_Id = " + Session["LocId"].ToString() + " and FinancialYearId = " + Session["FinanceYearId"].ToString() + " and AccTransId = " + EmployeeAccountId + " and Other_Account_No = " + hdnEmpIdSal.Value.ToString() + " group by Trans_Id";
                    DataTable EmpDt = objDa.return_DataTable(EmpQuery);
                    int EmpCount = 0;
                    string TransId = "0";
                    if (EmpDt != null && EmpDt.Rows.Count > 0)
                    {
                        EmpCount = int.Parse(EmpDt.Rows[0][0].ToString());
                        TransId = EmpDt.Rows[0][1].ToString();
                    }
                    string CurrencyId = Session["LocCurrencyId"].ToString();
                    double CompanyDrAmnt = 0;
                    double CompanyCrAmnt = 0;
                    double.TryParse(objAcParameter.GetCompanyCurrency(Session["CurrencyId"].ToString(), txtEditOpeningDebitAmt.Text.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out CompanyDrAmnt);
                    double.TryParse(objAcParameter.GetCompanyCurrency(Session["CurrencyId"].ToString(), txtEditOpeningCreditAmt.Text.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out CompanyCrAmnt);
                    string CompanyDrAmt = CompanyDrAmnt.ToString();
                    string CompanyCrAmt = CompanyCrAmnt.ToString();
                    if (EmpCount == 0)
                    {
                        int empinsert = objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), EmployeeAccountId, hdnEmpIdSal.Value.ToString(), txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, CurrencyId, CompanyDrAmt, CompanyCrAmt, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else if (EmpCount == 1)
                    {
                        int empupdate = objSubCOA.UpdateSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), TransId, EmployeeAccountId, txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, txtEditOpeningDebitAmt.Text, txtEditOpeningCreditAmt.Text, CurrencyId, CompanyDrAmt, CompanyCrAmt, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
            if (IsSalaryPlanEnable())
            {
                strGrossSalary = txtGrossSalary.Text;
                strBasicSalary = GetBasicSalaryBysalaryPlanId(SalaryPlanId, strGrossSalary);
                Objsalaryplandetail.Insert_Allowance_Using_SalaryPlan(Session["CompId"].ToString(), Session["UserId"].ToString(), SalaryPlanId, hdnEmpIdSal.Value, strGrossSalary, Convert.ToDouble(strBasicSalary));
            }
            Txt_Salary_Payment_Option_M.Text = "";
            DisplayMessage("Record Updated", "green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Salary()", true);
        }
    }
    public void InsertEmployeeeOpeningBalance(string strEmpId, string strCreditAmt, string strDebitAmt)
    {
    }
    protected void btnUpdateNotice_Click(object sender, EventArgs e)
    {
        if (txtScheduleDays_Popup.Text.Trim() == "")
        {
            DisplayMessage("Enter Schedule Days");
            txtScheduleDays_Popup.Focus();
            return;
        }
        int b = 0;
        //this variable is created by jitendra upadhyay on 18-09-2014
        //thsi variable for save the notification in employee notification table
        //code start
        objEmpNotice.DeleteEmployeeNotificationByEmpId(hdnEmpIdNF.Value);
        foreach (ListItem item in ChkReportList_popup.Items)
        {
            if (item.Selected == true)
            {
                b = objEmpNotice.InsertEmployeeNotification(hdnEmpIdNF.Value, item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        int RefId = 0;
        //code created on 27-09-2014 by jitendar upadhyay
        //this code for insert the sms and email notification in Reportmaster ,reportnotification and reportsetup table
        //code start
        DataTable dtReportMaster = objReportMaster.GetReportMasterByEmpId(hdnEmpIdNF.Value);
        if (dtReportMaster.Rows.Count > 0)
        {
            RefId = Convert.ToInt32(dtReportMaster.Rows[0]["Trans_Id"].ToString());
        }
        else
        {
            RefId = objReportMaster.InsertReportMaster(hdnEmpIdNF.Value, txtScheduleDays_Popup.Text, DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        objReportNotification.DeleteRecord_By_RefId(RefId.ToString());
        foreach (ListItem item in ChkSmsList_popup.Items)
        {
            if (item.Selected == true)
            {
                b = objReportNotification.InsertRecord(RefId.ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        foreach (ListItem item in ChkEmailList_popup.Items)
        {
            if (item.Selected == true)
            {
                b = objReportNotification.InsertRecord(RefId.ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        //Comment by Ghanshyam suthar on 20-02-2018 "Remove SMS, Email ,Schedule days & also remove the ""Delete Previous Report setup"" from the window."
        //if (objReportSetup.GetRecord_By_RefId(RefId.ToString()).Rows.Count > 0)
        //{
        //    if (chkprevious.Checked == true)
        //    {
        //        objReportSetup.DeleteRecord_By_RefId(RefId.ToString());
        //    }
        //    else
        //    {
        //        DisplayMessage("Previous Report setup is exists");
        //        return;
        //    }
        //}
        //------------------------------------------20-02-2018------------------------------------------
        try
        {
            b = objReportSetup.InsertRecord(RefId.ToString(), hdnEmpIdNF.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        catch
        {
        }
        //code end 27-09-2014
        //}
        //code end
        SystemLog.SaveSystemLog("Employee Master : Alert", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Notification Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        if (b != 0)
        {
            DisplayMessage("Record Updated", "green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Alert()", true);
        }
    }
    protected void btnSaveNF_Click(object sender, EventArgs e)
    {
        //this variable is created by jitendra upadhyay on 18-09-2014
        //thsi variable for save the notification in employee notification table
        //code start
        int b = 0;
        if (rbtnEmpNF.Checked)
        {
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues(gvEmpNF);
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select employee first");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select employee first");
                    return;
                }
            }
            if (txtScheduleDays.Text.Trim() == "")
            {
                DisplayMessage("Enter Schedule Days");
                txtScheduleDays.Focus();
                return;
            }
            for (int i = 0; i < userdetails.Count; i++)
            {
                objEmpNotice.DeleteEmployeeNotificationByEmpId(userdetails[i].ToString());
                foreach (ListItem item in ChkReportList.Items)
                {
                    if (item.Selected == true)
                    {
                        b = objEmpNotice.InsertEmployeeNotification(userdetails[i].ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
                int RefId = 0;
                //code created on 27-09-2014 by jitendar upadhyay
                //this code for insert the sms and email notification in Reportmaster ,reportnotification and reportsetup table
                //code start
                DataTable dtReportMaster = objReportMaster.GetReportMasterByEmpId(userdetails[i].ToString());
                if (dtReportMaster.Rows.Count > 0)
                {
                    RefId = Convert.ToInt32(dtReportMaster.Rows[0]["Trans_Id"].ToString());
                }
                else
                {
                    RefId = objReportMaster.InsertReportMaster(userdetails[i].ToString(), txtScheduleDays.Text, DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                try
                {
                    objReportNotification.DeleteRecord_By_RefId(RefId.ToString());
                    objReportSetup.DeleteRecord_By_RefId(RefId.ToString());
                }
                catch
                {
                }
                foreach (ListItem item in ChkSmsList.Items)
                {
                    if (item.Selected == true)
                    {
                        b = objReportNotification.InsertRecord(RefId.ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
                foreach (ListItem item in chkEmailList.Items)
                {
                    if (item.Selected == true)
                    {
                        b = objReportNotification.InsertRecord(RefId.ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
                try
                {
                    b = objReportSetup.InsertRecord(RefId.ToString(), userdetails[i].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
                catch
                {
                }
                //code end 27-09-2014
                //}
            }
        }
        else if (rbtnGroupNF.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            for (int i = 0; i < lbxGroupNF.Items.Count; i++)
            {
                if (lbxGroupNF.Items[i].Selected)
                {
                    GroupIds += lbxGroupNF.Items[i].Value + ",";
                }
            }
            if (GroupIds != "")
            {
                if (txtScheduleDays.Text.Trim() == "")
                {
                    DisplayMessage("Enter Schedule Days");
                    txtScheduleDays.Focus();
                    return;
                }
                DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }
            foreach (string str in EmpIds.Split(','))
            {
                if (str != "")
                {
                    objEmpNotice.DeleteEmployeeNotificationByEmpId(str);
                    foreach (ListItem item in ChkReportList.Items)
                    {
                        if (item.Selected == true)
                        {
                            b = objEmpNotice.InsertEmployeeNotification(str, item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                    int RefId = 0;
                    //code created on 27-09-2014 by jitendar upadhyay
                    //this code for insert the sms and email notification in Reportmaster ,reportnotification and reportsetup table
                    //code start
                    DataTable dtReportMaster = objReportMaster.GetReportMasterByEmpId(str);
                    if (dtReportMaster.Rows.Count > 0)
                    {
                        RefId = Convert.ToInt32(dtReportMaster.Rows[0]["Trans_Id"].ToString());
                    }
                    else
                    {
                        RefId = objReportMaster.InsertReportMaster(str, txtScheduleDays.Text, DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    try
                    {
                        objReportNotification.DeleteRecord_By_RefId(RefId.ToString());
                        objReportSetup.DeleteRecord_By_RefId(RefId.ToString());
                    }
                    catch
                    {
                    }
                    foreach (ListItem item in ChkSmsList.Items)
                    {
                        if (item.Selected == true)
                        {
                            b = objReportNotification.InsertRecord(RefId.ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                    foreach (ListItem item in chkEmailList.Items)
                    {
                        if (item.Selected == true)
                        {
                            b = objReportNotification.InsertRecord(RefId.ToString(), item.Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                    try
                    {
                        b = objReportSetup.InsertRecord(RefId.ToString(), str, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    catch
                    {
                    }
                    //code end 27-09-2014
                    //    b = objEmpNotice.InsertEmployeeNotification(Session["CompId"].ToString(), str, chkSMSDocExp.Checked.ToString(), chkSMSAbsent.Checked.ToString(), chkSMSLate.Checked.ToString(), chkSMSEarly.Checked.ToString(), ChkSmsLeave.Checked.ToString(), ChkSMSPartial.Checked.ToString(), chkSMSNoClock.Checked.ToString(), false.ToString(), false.ToString(), false.ToString(), false.ToString(), false.ToString(), ChkRptAbsent.Checked.ToString(), chkRptLate.Checked.ToString(), chkRptEarly.Checked.ToString(), ChkRptInOut.Checked.ToString(), ChkRptSalary.Checked.ToString(), ChkRptOvertime.Checked.ToString(), ChkRptLog.Checked.ToString(), chkRptDoc.Checked.ToString(), chkRptViolation.Checked.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), true.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    //}
                }
            }
            SystemLog.SaveSystemLog("Employee Master : Alert", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Notification Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        }
        if (b != 0)
        {
            DisplayMessage("Record Saved", "green");
            btnResetNF_Click(null, null);
        }
        AllPageCode();
    }
    protected void btnSavePenalty_Click(object sender, EventArgs e)
    {
        if (rbtLateInEnable.Checked == false && rbtLateInDisable.Checked == false)
        {
            DisplayMessage("Please select late type");
            return;
        }
        if (rbtEarlyOutEnable.Checked == false && rbtEarlyOutDisable.Checked == false)
        {
            DisplayMessage("Please select early out type");
            return;
        }
        if (rbtnAbsentEnable.Checked == false && rbtnAbsentDisable.Checked == false)
        {
            DisplayMessage("Please select absent type");
            return;
        }
        if (rbtnPartialLeaveEnable.Checked == false && rbtnPartialLeaveDisable.Checked == false)
        {
            DisplayMessage("Please select Partial Leave type");
            return;
        }
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        int b = 0;
        string strEmpList = string.Empty;
        bool IsLate = false;
        bool IsEarly = false;
        bool IsAbsent = false;
        bool IsPartialLeave = false;
        if (rbtLateInEnable.Checked)
        {
            IsLate = true;
        }
        if (rbtEarlyOutEnable.Checked)
        {
            IsEarly = true;
        }
        if (rbtnAbsentEnable.Checked)
        {
            IsAbsent = true;
        }
        if (rbtnPartialLeaveEnable.Checked)
        {
            IsPartialLeave = true;
        }
        if (rbtnPenaltyEmp.Checked)
        {
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues(gvEmpPenalty);
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select employee first");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select employee first");
                    return;
                }
            }
            for (int i = 0; i < userdetails.Count; i++)
            {
                DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(userdetails[i].ToString(), Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    if (IsLate || IsEarly || IsAbsent || IsPartialLeave)
                    {
                        if (dt.Rows[0]["Type"].ToString().ToUpper() == "MANAGER" || dt.Rows[0]["Type"].ToString().ToUpper() == "CEO")
                        {
                            if (strEmpList == "")
                            {
                                strEmpList = dt.Rows[0]["Emp_Code"].ToString();
                            }
                            else
                            {
                                strEmpList = strEmpList + "," + dt.Rows[0]["Emp_Code"].ToString();
                            }
                            continue;
                        }
                    }
                    objEmpParam.DeleteEmployeeParameterByEmpId(userdetails[i].ToString());
                    string Payment_Opt_Account_ID = string.Empty;
                    if (dt.Rows[0]["Payment_Opt_Account_ID"].ToString() == "")
                        Payment_Opt_Account_ID = "0";
                    else
                        Payment_Opt_Account_ID = dt.Rows[0]["Payment_Opt_Account_ID"].ToString();
                    b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), userdetails[i].ToString(), dt.Rows[0]["Basic_Salary"].ToString(), dt.Rows[0]["Salary_Type"].ToString(), dt.Rows[0]["Currency_Id"].ToString(), dt.Rows[0]["Assign_Min"].ToString(), dt.Rows[0]["Effective_Work_Cal_Method"].ToString(), dt.Rows[0]["Is_OverTime"].ToString(), dt.Rows[0]["Normal_OT_Method"].ToString(), dt.Rows[0]["Normal_OT_Type"].ToString(), dt.Rows[0]["Normal_OT_Value"].ToString(), dt.Rows[0]["Normal_HOT_Type"].ToString(), dt.Rows[0]["Normal_HOT_Value"].ToString(), dt.Rows[0]["Normal_WOT_Type"].ToString(), dt.Rows[0]["Normal_WOT_Value"].ToString(), dt.Rows[0]["Is_Partial_Enable"].ToString(), dt.Rows[0]["Partial_Leave_Mins"].ToString(), dt.Rows[0]["Partial_Leave_Day"].ToString(), dt.Rows[0]["Is_Partial_Carry"].ToString(), IsLate.ToString(), IsEarly.ToString(), IsAbsent.ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), dt.Rows[0]["Field6"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), IsPartialLeave.ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), Payment_Opt_Account_ID);
                }
                else
                {
                    b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), userdetails[i].ToString(), "0", "Monthly", "0", "0", "PairWise", false.ToString(), "Work Hour", "2", "0", "2", "0", "2", "0", false.ToString(), "0", "0", false.ToString(), IsLate.ToString(), IsEarly.ToString(), IsAbsent.ToString(), "", "", false.ToString(), DateTime.Now.ToString(), "", "", "", "", "", IsPartialLeave.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", false.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
                }
            }
        }
        else if (rbtnPenaltyGroup.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            for (int i = 0; i < lbxGroupPenalty.Items.Count; i++)
            {
                if (lbxGroupPenalty.Items[i].Selected)
                {
                    GroupIds += lbxGroupPenalty.Items[i].Value + ",";
                }
            }
            if (GroupIds != "")
            {
                DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
                foreach (string str in EmpIds.Split(','))
                {
                    if (str != "")
                    {
                        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                        if (dt.Rows.Count > 0)
                        {
                            if (IsLate || IsEarly || IsAbsent || IsPartialLeave)
                            {
                                if (dt.Rows[0]["Type"].ToString().ToUpper() == "MANAGER" || dt.Rows[0]["Type"].ToString().ToUpper() == "CEO")
                                {
                                    if (strEmpList == "")
                                    {
                                        strEmpList = dt.Rows[0]["Emp_Code"].ToString();
                                    }
                                    else
                                    {
                                        strEmpList = strEmpList + "," + dt.Rows[0]["Emp_Code"].ToString();
                                    }
                                    continue;
                                }
                            }
                            objEmpParam.DeleteEmployeeParameterByEmpId(str);
                            b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), str, dt.Rows[0]["Basic_Salary"].ToString(), dt.Rows[0]["Salary_Type"].ToString(), dt.Rows[0]["Currency_Id"].ToString(), dt.Rows[0]["Assign_Min"].ToString(), dt.Rows[0]["Effective_Work_Cal_Method"].ToString(), dt.Rows[0]["Is_OverTime"].ToString(), dt.Rows[0]["Normal_OT_Method"].ToString(), dt.Rows[0]["Normal_OT_Type"].ToString(), dt.Rows[0]["Normal_OT_Value"].ToString(), dt.Rows[0]["Normal_HOT_Type"].ToString(), dt.Rows[0]["Normal_HOT_Value"].ToString(), dt.Rows[0]["Normal_WOT_Type"].ToString(), dt.Rows[0]["Normal_WOT_Value"].ToString(), dt.Rows[0]["Is_Partial_Enable"].ToString(), dt.Rows[0]["Partial_Leave_Mins"].ToString(), dt.Rows[0]["Partial_Leave_Day"].ToString(), dt.Rows[0]["Is_Partial_Carry"].ToString(), IsLate.ToString(), IsEarly.ToString(), IsAbsent.ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), dt.Rows[0]["Field6"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), IsPartialLeave.ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), dt.Rows[0]["Payment_Opt_Account_ID"].ToString());
                        }
                        else
                        {
                            b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), str, "0", "Monthly", "0", "0", "", false.ToString(), "", "0", "0", "0", "0", "0", "0", false.ToString(), "0", "0", false.ToString(), IsLate.ToString(), IsEarly.ToString(), IsAbsent.ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), dt.Rows[0]["Field6"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), IsPartialLeave.ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), dt.Rows[0]["Payment_Opt_Account_ID"].ToString());
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }
        }
        SystemLog.SaveSystemLog("Employee Master : Penalty", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Penalty Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        if (b != 0)
        {
            if (strEmpList == "")
            {
                DisplayMessage("Record Saved", "green");
            }
            else
            {
                DisplayMessage("Record not saved for Manager and CEO (" + strEmpList + ")");
            }
            btnResetPenalty_Click(null, null);
        }
        AllPageCode();
    }
    protected void btnSaveOT_Click(object sender, EventArgs e)
    {
        int b = 0;
        bool IsCompOT = false;
        bool IsPartialComp = false;
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        string workminute = string.Empty;
        string CurrencyId = string.Empty;
        string CalMethod = string.Empty;
        DataTable dtComp = objComp.GetCompanyMasterById(Session["CompId"].ToString());
        if (dtComp.Rows.Count > 0)
        {
            CurrencyId = dtComp.Rows[0]["Currency_Id"].ToString();
        }
        workminute = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        CalMethod = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string strCarry = string.Empty;
        string OtEmp = string.Empty;
        string partialEnable = string.Empty;
        if (rbtnPartialEnable.Checked)
        {
            partialEnable = "True";
        }
        else
        {
            partialEnable = "False";
        }
        if (rbtnOTEnable.Checked)
        {
            OtEmp = "True";
        }
        else
        {
            OtEmp = "False";
        }
        if (rbtnCarryYes.Checked)
        {
            strCarry = "True";
        }
        else
        {
            strCarry = "False";
        }
        if (rbtnEmpOT.Checked)
        {
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues(gvEmpOT);
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select employee first");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select employee first");
                    return;
                }
            }
            //here we set the validation that total minute for month is not greater than company parameter
            //and aso set validation for minute used in one day that this is not greater than total minute for month
            //this code is created by jitendra upadhyay pn 19-08-2014
            //code start
            if (txtTotalMinutes.Text.Trim() == "")
            {
                DisplayMessage("Enter Total Minute For Month");
                txtTotalMinutes.Focus();
                return;
            }
            if (txtMinuteday.Text.Trim() == "")
            {
                DisplayMessage("Enter Minute Use in a Day One Time");
                txtMinuteday.Focus();
                return;
            }
            double TotalComapany_PartialMinute = 0;
            TotalComapany_PartialMinute = float.Parse(objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
            if (float.Parse(txtTotalMinutes.Text) > TotalComapany_PartialMinute)
            {
                DisplayMessage("Total Minute For Month Should be Less Than or Equal to " + TotalComapany_PartialMinute.ToString());
                txtTotalMinutes.Focus();
                return;
            }
            if (float.Parse(txtTotalMinutes.Text) < float.Parse(txtMinuteday.Text))
            {
                DisplayMessage("Minute use in a Day For one Time Should be Less Than or Equal to Total Minute For Month");
                txtMinuteday.Focus();
                return;
            }
            //code end
            for (int i = 0; i < userdetails.Count; i++)
            {
                DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(userdetails[i].ToString(), Session["CompId"].ToString());
                objEmpParam.DeleteEmployeeParameterByEmpId(userdetails[i].ToString());
                if (dt.Rows.Count > 0)
                {
                    b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), userdetails[i].ToString(), dt.Rows[0]["Basic_Salary"].ToString(), dt.Rows[0]["Salary_Type"].ToString(), dt.Rows[0]["Currency_Id"].ToString(), dt.Rows[0]["Assign_Min"].ToString(), dt.Rows[0]["Effective_Work_Cal_Method"].ToString(), OtEmp, ddlOTCalc.SelectedValue, ddlNormalType.SelectedValue, GetText(txtNoralType.Text), ddlHolidayType.SelectedValue, GetText(txtHolidayValue.Text), ddlWeekOffType.SelectedValue, GetText(txtWeekOffValue.Text), partialEnable, GetText(txtTotalMinutes.Text), GetText(txtMinuteday.Text), strCarry, dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), "", "", true.ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), GetText(dt.Rows[0]["Payment_Opt_Account_ID"].ToString()));
                }
                else
                {
                    b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), userdetails[i].ToString(), "0", "Monthly", CurrencyId, workminute, CalMethod, OtEmp, ddlOTCalc.SelectedValue, ddlNormalType.SelectedValue, GetText(txtNoralType.Text), ddlHolidayType.SelectedValue, GetText(txtHolidayValue.Text), ddlWeekOffType.SelectedValue, GetText(txtWeekOffValue.Text), partialEnable, GetText(txtTotalMinutes.Text), GetText(txtMinuteday.Text), strCarry, false.ToString(), false.ToString(), false.ToString(), "", "", true.ToString(), DateTime.Now.ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", "False", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
                }
            }
        }
        else if (rbtnGroupOT.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            for (int i = 0; i < lbxGroupOT.Items.Count; i++)
            {
                if (lbxGroupOT.Items[i].Selected)
                {
                    GroupIds += lbxGroupOT.Items[i].Value + ",";
                }
            }
            if (GroupIds != "")
            {
                //here we set the validation that total minute for month is not greater than company parameter
                //and aso set validation for minute used in one day that this is not greater than total minute for month
                //this code is created by jitendra upadhyay pn 19-08-2014
                //code start
                if (txtTotalMinutes.Text.Trim() == "")
                {
                    DisplayMessage("Enter Total Minute For Month");
                    txtTotalMinutes.Focus();
                    return;
                }
                if (txtMinuteday.Text.Trim() == "")
                {
                    DisplayMessage("Enter Minute Use in a Day One Time");
                    txtMinuteday.Focus();
                    return;
                }
                double TotalComapany_PartialMinute = 0;
                TotalComapany_PartialMinute = float.Parse(objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                if (float.Parse(txtTotalMinutes.Text) > TotalComapany_PartialMinute)
                {
                    DisplayMessage("Total Minute For Month Should be Less Than or Equal to " + TotalComapany_PartialMinute.ToString());
                    txtTotalMinutes.Focus();
                    return;
                }
                if (float.Parse(txtTotalMinutes.Text) < float.Parse(txtMinuteday.Text))
                {
                    DisplayMessage("Minute use in a Day For one Time Should be Less Than or Equal to Total Minute For Month");
                    txtMinuteday.Focus();
                    return;
                }
                //code end
                DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
                foreach (string str in EmpIds.Split(','))
                {
                    if (str != "")
                    {
                        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                        objEmpParam.DeleteEmployeeParameterByEmpId(str);
                        if (dt.Rows.Count > 0)
                        {
                            b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), str, dt.Rows[0]["Basic_Salary"].ToString(), dt.Rows[0]["Salary_Type"].ToString(), dt.Rows[0]["Currency_Id"].ToString(), dt.Rows[0]["Assign_Min"].ToString(), dt.Rows[0]["Effective_Work_Cal_Method"].ToString(), OtEmp, ddlOTCalc.SelectedValue, ddlNormalType.SelectedValue, GetText(txtNoralType.Text), ddlHolidayType.SelectedValue, GetText(txtHolidayValue.Text), ddlWeekOffType.SelectedValue, GetText(txtWeekOffValue.Text), partialEnable, GetText(txtTotalMinutes.Text), GetText(txtMinuteday.Text), strCarry, dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), dt.Rows[0]["Field6"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), dt.Rows[0]["Payment_Opt_Account_ID"].ToString());
                        }
                        else
                        {
                            b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), str, "0", "Monthly", CurrencyId, workminute, CalMethod, OtEmp, ddlOTCalc.SelectedValue, ddlNormalType.SelectedValue, GetText(txtNoralType.Text), ddlHolidayType.SelectedValue, GetText(txtHolidayValue.Text), ddlWeekOffType.SelectedValue, GetText(txtWeekOffValue.Text), partialEnable, GetText(txtTotalMinutes.Text), GetText(txtMinuteday.Text), strCarry, false.ToString(), false.ToString(), false.ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), dt.Rows[0]["Field6"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Field8"].ToString(), dt.Rows[0]["Field9"].ToString(), dt.Rows[0]["Field10"].ToString(), dt.Rows[0]["Field11"].ToString(), dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[0]["Gross_Salary"].ToString(), dt.Rows[0]["IsCtc_Employee"].ToString(), dt.Rows[0]["MobileBill_Limit"].ToString(), dt.Rows[0]["Salary_Plan_Id"].ToString(), dt.Rows[0]["Opening_Credit"].ToString(), dt.Rows[0]["Opening_Debit"].ToString(), dt.Rows[0]["Previous_Employer_Earning"].ToString(), dt.Rows[0]["Previous_Employer_TDS"].ToString(), dt.Rows[0]["Field15"].ToString(), dt.Rows[0]["Field16"].ToString(), dt.Rows[0]["Field17"].ToString(), dt.Rows[0]["Field18"].ToString(), dt.Rows[0]["Field19"].ToString(), dt.Rows[0]["Field20"].ToString(), dt.Rows[0]["Payment_Opt_Account_ID"].ToString());
                        }
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }
            SystemLog.SaveSystemLog("Employee Master : OT/PL", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "OT/PL Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        }
        if (b != 0)
        {
            SystemLog.SaveSystemLog("Employee Master : OT/PL", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "OT/PL Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
            DisplayMessage("Record Saved", "green");
            btnResetOT_Click(null, null);
        }
        AllPageCode();
    }
    public string GetText(string value)
    {
        string value1 = string.Empty; ;
        if (value == "")
        {
            value1 = "0";
        }
        else
        {
            value1 = value;
        }
        return value1;
    }
    protected void btnCancelPenalty_Click(object sender, EventArgs e)
    {
        lblSelectRecordPenalty.Text = "";
        gvEmpPenalty.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationPenalty, ddlDeptPenalty);
        Session["dtEmpPenalty"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpPenalty, dtEmp, "", "");
        Hdn_Edit.Value = "";
        btnResetPenalty_Click(null, null);
        btnList_Click(null, null);
        AllPageCode();
    }
    protected void btnCancelOT_Click(object sender, EventArgs e)
    {
        lblSelectRecordOT.Text = "";
        gvEmpOT.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationOTPL, ddlDepartmentOTPL);
        Session["dtEmpSal"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpOT, dtEmp, "", "");
        Hdn_Edit.Value = "";
        btnResetOT_Click(null, null);
        btnList_Click(null, null);
        AllPageCode();
    }
    protected void btnResetPenalty_Click(object sender, EventArgs e)
    {
        if (Session["dtEmpPenalty"] != null)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpPenalty, (DataTable)Session["dtEmpPenalty"], "", "");
        }
        lblSelectRecordPenalty.Text = "";
        rbtEarlyOutDisable.Checked = false;
        rbtEarlyOutEnable.Checked = true;
        rbtLateInDisable.Checked = false;
        rbtLateInEnable.Checked = true;
        rbtnAbsentDisable.Checked = false;
        rbtnAbsentEnable.Checked = true;
        rbtnPartialLeaveEnable.Checked = true;
        rbtnPartialLeaveDisable.Checked = false;
        Session["CHECKED_ITEMS"] = null;
        AllPageCode();
    }
    protected void btnResetOT_Click(object sender, EventArgs e)
    {
        ddlOTCalc.SelectedIndex = 0;
        txtNoralType.Text = "";
        txtWeekOffValue.Text = "";
        txtHolidayValue.Text = "";
        ddlNormalType.SelectedIndex = 0;
        ddlWeekOffType.SelectedIndex = 0;
        ddlHolidayType.SelectedIndex = 0;
        txtTotalMinutes.Text = "";
        txtMinuteday.Text = "";
        rbtnPartialEnable.Checked = true;
        rbtPartial_OnCheckedChanged(null, null);
        rbtnOTEnable.Checked = true;
        rbtOT_OnCheckedChanged(null, null);
        if (Session["dtEmpOT"] != null)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpOT, (DataTable)Session["dtEmpOT"], "", "");
        }
        lblSelectRecordOT.Text = "";
        Session["CHECKED_ITEMS"] = null;
        DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
        dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)lbxGroupOT, dtGroup, "Group_Name", "Group_Id");
        }
        gvEmployeeOT.DataSource = null;
        gvEmployeeOT.DataBind();
        bool IsCompOT = false;
        bool IsPartialComp = false;
        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        if (IsCompOT)
        {
            rbtnOTEnable.Checked = true;
            rbtnOTDisable.Checked = false;
            rbtOT_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnOTEnable.Checked = false;
            rbtnOTDisable.Checked = true;
            rbtOT_OnCheckedChanged(null, null);
            rbtnOTEnable.Enabled = false;
            rbtnOTDisable.Enabled = false;
        }
        if (IsPartialComp)
        {
            rbtnPartialEnable.Checked = true;
            rbtnPartialDisable.Checked = false;
            rbtPartial_OnCheckedChanged(null, null);
        }
        else
        {
            rbtnPartialEnable.Checked = false;
            rbtnPartialDisable.Checked = true;
            rbtPartial_OnCheckedChanged(null, null);
            rbtnPartialEnable.Enabled = false;
            rbtnPartialDisable.Enabled = false;
        }
    }
    protected void btnSaveSal_Click(object sender, EventArgs e)
    {
        if (txtWorkMinute.Text == "")
        {
            DisplayMessage("Enter Work Minute");
            txtWorkMinute.Focus();
            return;
        }
        //if (ddlCurrency.SelectedIndex == 0)
        //{
        //    DisplayMessage("Select Currency");
        //    ddlCurrency.Focus();
        //    return;
        //}
        if (txtOpeningCreditAmt.Text == "")
        {
            txtOpeningCreditAmt.Text = "0";
        }
        if (txtOpeningDebitAmt.Text == "")
        {
            txtOpeningDebitAmt.Text = "0";
        }
        if (txtEmployerTotalEarning.Text == "")
        {
            txtEmployerTotalEarning.Text = "0";
        }
        if (txttOtalTDS.Text == "")
        {
            txttOtalTDS.Text = "0";
        }
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        bool IsOpeningBal = false;
        if (int.Parse(txtOpeningCreditAmt.Text.ToString()) > 0)
        {
            IsOpeningBal = true;
        }
        else if (int.Parse(txtOpeningDebitAmt.Text.ToString()) > 0)
        {
            IsOpeningBal = true;
        }
        if (IsOpeningBal)
        {
            string EmpAcc = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            if (String.IsNullOrEmpty(EmpAcc))
            {
                DisplayMessage("Please configure Employee Account in Finance");
                return;
            }
        }
        string strGrossSalary = string.Empty;
        string strBasicSalary = string.Empty;
        string SalaryPlanId = string.Empty;
        if (ddlSalaryPlan.SelectedIndex > 0)
        {
            SalaryPlanId = ddlSalaryPlan.SelectedValue;
        }
        if (IsSalaryPlanEnable())
        {
            strGrossSalary = txtBasic.Text;
            strBasicSalary = GetBasicSalaryBysalaryPlanId(SalaryPlanId, strGrossSalary);
            if (Salary_Plan_Check(SalaryPlanId, strBasicSalary, strGrossSalary) == false)
            {
                DisplayMessage("This Salary plan is not applicable for salary - " + strGrossSalary + "");
                return;
            }
        }
        else
        {
            strGrossSalary = txtBasic.Text;
            strBasicSalary = txtBasic.Text;
        }
        int b = 0;
        bool IsCompOT = false;
        bool IsPartialComp = false;
        bool IsPartialCarry = false;
        string PartialMin = "";
        string PartialminOne = "";
        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        IsPartialComp = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        IsPartialCarry = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Carry Forward Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        PartialMin = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        PartialminOne = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        ddlCurrency.SelectedValue = ObjLocMaster.Get_Currency_By_Location_ID(Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Currency_Id"].ToString();
        string NormalOT = string.Empty;
        NormalOT = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        bool EmpInPayroll = false;
        bool EmpPF = false;
        bool EmpESIC = false;
        if (chkEmpINPayroll.Checked)
        {
            EmpPF = chkEmpPf.Checked;
            EmpESIC = chkEmpEsic.Checked;
            EmpInPayroll = true;
        }
        else
        {
            EmpInPayroll = false;
            EmpPF = false;
            EmpESIC = false;
        }
        if (rbtnEmpSal.Checked)
        {
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues(gvEmpSalary);
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select employee first");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select employee first");
                    return;
                }
            }
            for (int i = 0; i < userdetails.Count; i++)
            {
                DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(userdetails[i].ToString(), Session["CompId"].ToString());
                objEmpParam.DeleteEmployeeParameterByEmpId(userdetails[i].ToString());
                if (dt.Rows.Count > 0)
                {
                    b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), userdetails[i].ToString(), strBasicSalary, ddlPayment.SelectedValue, ddlCurrency.SelectedValue, txtWorkMinute.Text, ddlWorkCalMethod.SelectedValue, dt.Rows[0]["Is_OverTime"].ToString(), dt.Rows[0]["Normal_OT_Method"].ToString(), dt.Rows[0]["Normal_OT_Type"].ToString(), dt.Rows[0]["Normal_OT_Value"].ToString(), dt.Rows[0]["Normal_HOT_Type"].ToString(), dt.Rows[0]["Normal_HOT_Value"].ToString(), dt.Rows[0]["Normal_WOT_Type"].ToString(), dt.Rows[0]["Normal_WOT_Value"].ToString(), dt.Rows[0]["Is_Partial_Enable"].ToString(), dt.Rows[0]["Partial_Leave_Mins"].ToString(), dt.Rows[0]["Partial_Leave_Day"].ToString(), dt.Rows[0]["Is_Partial_Carry"].ToString(), dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), EmpPF.ToString(), EmpESIC.ToString(), EmpInPayroll.ToString(), DateTime.Now.ToString(), txtSalIncrDuration.Text, txtIncrementPerFrom.Text, txtIncrementPerTo.Text, ddlCategory.SelectedValue, dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strGrossSalary, chkisCtcEmployee.Checked.ToString(), txtMobilleBillLimit.Text, SalaryPlanId, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtEmployerTotalEarning.Text, txttOtalTDS.Text, "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
                }
                else
                {
                    b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), userdetails[i].ToString(), strBasicSalary, ddlPayment.SelectedValue, ddlCurrency.SelectedValue, txtWorkMinute.Text, ddlWorkCalMethod.SelectedValue, IsCompOT.ToString(), NormalOT, "2", "0", "2", "0", "2", "0", IsPartialComp.ToString(), PartialMin, PartialminOne, IsPartialCarry.ToString(), false.ToString(), false.ToString(), false.ToString(), EmpPF.ToString(), EmpESIC.ToString(), EmpInPayroll.ToString(), DateTime.Now.ToString(), txtSalIncrDuration.Text, txtIncrementPerFrom.Text, txtIncrementPerTo.Text, ddlCategory.SelectedValue, "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strGrossSalary, chkisCtcEmployee.Checked.ToString(), txtMobilleBillLimit.Text, SalaryPlanId, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtEmployerTotalEarning.Text, txttOtalTDS.Text, "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
                }
                bool IsIndemnity = false;
                if (rbnIndemnity1.Checked == true)
                {
                    IsIndemnity = true;
                }
                else
                {
                    IsIndemnity = false;
                }
                int c = 0;
                int d = 0;
                int f = 0;
                objEmpParam.DeleteEmployeeParameterByEmpIdNew(userdetails[i].ToString());
                c = objEmpParam.InsertEmpParameterNew(userdetails[i].ToString(), Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                d = objEmpParam.InsertEmpParameterNew(userdetails[i].ToString(), Session["CompId"].ToString(), "IndemnityYear", txtIndemnityYear.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                f = objEmpParam.InsertEmpParameterNew(userdetails[i].ToString(), Session["CompId"].ToString(), "IndemnityDayas", txtIndemnityDays.Text, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //here we are inserting employee opening balance 
                //objDa.execute_Command("delete from Pay_EmployeeSalaryStatement where Emp_id="+ userdetails[i].ToString() + " and Is_Opening_Balance='True'");
                objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), userdetails[i].ToString(), DateTime.Now.ToString(), txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, Session["CurrencyId"].ToString(), txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, "Pay_EmployeeAdvancePayment", "0", "0", "Opening Balance", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), true.ToString());
                // Insert and Update Employee Opening Balance
                // Count = 1 then Update OtherWise Insert
                // Insert is happen only once for every employee
                string EmployeeAccountId = Ac_ParameterMaster.GetEmployeeAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
                int DebAmt = int.Parse(txtOpeningDebitAmt.Text.ToString());
                int CreAmt = int.Parse(txtOpeningCreditAmt.Text.ToString());
                if (EmployeeAccountId != "0" && (DebAmt > 0 || CreAmt > 0))
                {
                    string EmpQuery = "Select COUNT(*) Count,ISNULL(Trans_Id,0) Id from Ac_SubChartOfAccount where Company_Id = " + Session["CompId"].ToString() + " and Brand_Id = " + Session["BrandId"].ToString() + " and Location_Id = " + Session["LocId"].ToString() + " and FinancialYearId = " + Session["FinanceYearId"].ToString() + " and AccTransId = " + EmployeeAccountId + " and Other_Account_No = " + userdetails[i].ToString() + " group by Trans_Id";
                    DataTable EmpDt = objDa.return_DataTable(EmpQuery);
                    int EmpCount = 0;
                    string TransId = "0";
                    if (EmpDt != null && EmpDt.Rows.Count > 0)
                    {
                        EmpCount = int.Parse(EmpDt.Rows[0][0].ToString());
                        TransId = EmpDt.Rows[0][1].ToString();
                    }
                    string CurrencyId = Session["LocCurrencyId"].ToString();
                    double CompanyDrAmnt = 0;
                    double CompanyCrAmnt = 0;
                    double.TryParse(objAcParameter.GetCompanyCurrency(Session["CurrencyId"].ToString(), txtEditOpeningDebitAmt.Text.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out CompanyDrAmnt);
                    double.TryParse(objAcParameter.GetCompanyCurrency(Session["CurrencyId"].ToString(), txtEditOpeningCreditAmt.Text.ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), out CompanyCrAmnt);
                    string CompanyDrAmt = CompanyDrAmnt.ToString();
                    string CompanyCrAmt = CompanyCrAmnt.ToString();
                    string OpBalTransId = userdetails[i].ToString();
                    if (EmpCount == 0)
                    {
                        int empinsert = objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), EmployeeAccountId, userdetails[i].ToString(), txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, CurrencyId, CompanyDrAmt, CompanyCrAmt, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else if (EmpCount == 1)
                    {
                        int empupdate = objSubCOA.UpdateSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), TransId, EmployeeAccountId, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, CurrencyId, CompanyDrAmt, CompanyCrAmt, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
        }
        else if (rbtnGroupSal.Checked)
        {
            string GroupIds = string.Empty;
            string EmpIds = string.Empty;
            for (int i = 0; i < lbxGroupSal.Items.Count; i++)
            {
                if (lbxGroupSal.Items[i].Selected)
                {
                    GroupIds += lbxGroupSal.Items[i].Value + ",";
                }
            }
            if (GroupIds != "")
            {
                DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
                foreach (string str in EmpIds.Split(','))
                {
                    if (str != "")
                    {
                        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                        objEmpParam.DeleteEmployeeParameterByEmpId(str);
                        if (dt.Rows.Count > 0)
                        {
                            b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), str, txtBasic.Text, ddlPayment.SelectedValue, ddlCurrency.SelectedValue, txtWorkMinute.Text, ddlWorkCalMethod.SelectedValue, dt.Rows[0]["Is_OverTime"].ToString(), dt.Rows[0]["Normal_OT_Method"].ToString(), dt.Rows[0]["Normal_OT_Type"].ToString(), dt.Rows[0]["Normal_OT_Value"].ToString(), dt.Rows[0]["Normal_HOT_Type"].ToString(), dt.Rows[0]["Normal_HOT_Value"].ToString(), dt.Rows[0]["Normal_WOT_Type"].ToString(), dt.Rows[0]["Normal_WOT_Value"].ToString(), dt.Rows[0]["Is_Partial_Enable"].ToString(), dt.Rows[0]["Partial_Leave_Mins"].ToString(), dt.Rows[0]["Partial_Leave_Day"].ToString(), dt.Rows[0]["Is_Partial_Carry"].ToString(), dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), EmpPF.ToString(), EmpESIC.ToString(), EmpInPayroll.ToString(), DateTime.Now.ToString(), txtSalIncrDuration.Text, txtIncrementPerFrom.Text, txtIncrementPerTo.Text, ddlCategory.SelectedValue, dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strGrossSalary, chkisCtcEmployee.Checked.ToString(), txtMobilleBillLimit.Text, SalaryPlanId, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtEmployerTotalEarning.Text, txttOtalTDS.Text, "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
                        }
                        else
                        {
                            b = objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), str, txtBasic.Text, ddlPayment.SelectedValue, ddlCurrency.SelectedValue, txtWorkMinute.Text, ddlWorkCalMethod.SelectedValue, IsCompOT.ToString(), NormalOT, "2", "0", "2", "0", "2", "0", IsPartialComp.ToString(), PartialMin, PartialminOne, IsPartialCarry.ToString(), false.ToString(), false.ToString(), false.ToString(), EmpPF.ToString(), EmpESIC.ToString(), EmpInPayroll.ToString(), DateTime.Now.ToString(), txtSalIncrDuration.Text, txtIncrementPerFrom.Text, txtIncrementPerTo.Text, ddlCategory.SelectedValue, dt.Rows[0]["Field12"].ToString(), dt.Rows[0]["Field13"].ToString(), dt.Rows[0]["Field14"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strGrossSalary, chkisCtcEmployee.Checked.ToString(), txtMobilleBillLimit.Text, SalaryPlanId, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtEmployerTotalEarning.Text, txttOtalTDS.Text, "0", "0", "0", "0", "0", "0", Salary_Payment_Option);
                        }
                        //opening balance entry
                        //objDa.execute_Command("delete from Pay_EmployeeSalaryStatement where Emp_id=" + str + " and Is_Opening_Balance='True'");
                        objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), str, DateTime.Now.ToString(), txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, Session["CurrencyId"].ToString(), txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, txtOpeningDebitAmt.Text, txtOpeningCreditAmt.Text, "Pay_EmployeeAdvancePayment", "0", "0", "Opening Balance", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), true.ToString());
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }
        }
        if (IsSalaryPlanEnable())
        {
            strGrossSalary = txtGrossSalary.Text;
            strBasicSalary = GetBasicSalaryBysalaryPlanId(SalaryPlanId, strGrossSalary);
            Objsalaryplandetail.Insert_Allowance_Using_SalaryPlan(Session["CompId"].ToString(), Session["UserId"].ToString(), SalaryPlanId, hdnEmpIdSal.Value, strGrossSalary, Convert.ToDouble(strBasicSalary));
        }
        if (b != 0)
        {
            DisplayMessage("Record Saved", "green");
            btnResetSal_Click(null, null);
        }
        AllPageCode();
    }
    protected void btnResetSal_Click(object sender, EventArgs e)
    {
        Txt_Salary_Payment_Option.Text = "";
        Txt_Salary_Payment_Option_M.Text = "";
        txtBasic.Text = "";
        txtWorkMinute.Text = "";
        ddlPayment.SelectedIndex = 0;
        ddlWorkCalMethod.SelectedIndex = 0;
        //ddlCurrency.SelectedIndex = 0;
        chkEmpINPayroll.Checked = true;
        chkEmpINPayroll_OnCheckedChanged(null, null);
        if (Session["dtEmpSal"] != null)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpSalary, (DataTable)Session["dtEmpSal"], "", "");
        }
        lblSelectRecordSal.Text = "";
        Session["CHECKED_ITEMS"] = null;
        DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
        dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)lbxGroupSal, dtGroup, "Group_Name", "Group_Id");
        }
        gvEmployeeSal.DataSource = null;
        gvEmployeeSal.DataBind();
    }
    protected void btnCancelSal_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectRecordSal.Text = "";
        gvEmpSalary.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationSalary, ddlDeptSalary);
        Session["dtEmpSal"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpSalary, dtEmp, "", "");
        Hdn_Edit.Value = "";
        btnResetSal_Click(null, null);
        btnList_Click(null, null);
        gvEmpSalary.Visible = true;
    }
    protected void btnClosePanel_ClickNf(object sender, EventArgs e)
    {
        //pnlNotice1.Visible = false;
        //pnlNotice2.Visible = false;
        BindNotification();
    }
    protected void btnCancelNF_Click(object sender, EventArgs e)
    {
        lblSelectRecordNF.Text = "";
        gvEmpNF.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationAlert, ddlDeptAlert);
        Session["dtEmpNF"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
        Hdn_Edit.Value = "";
        btnResetNF_Click(null, null);
        btnList_Click(null, null);
    }
    protected void btnResetNF_Click(object sender, EventArgs e)
    {
        lblSelectRecordNF.Text = "";
        gvEmpNF.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationAlert, ddlDeptAlert);
        Session["dtEmpNF"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpNF, dtEmp, "", "");
        Session["CHECKED_ITEMS"] = null;
        //chkSMSDocExp.Checked = false;
        //chkSMSAbsent.Checked = false;
        //chkSMSLate.Checked = false;
        //chkSMSEarly.Checked = false;
        //ChkSmsLeave.Checked = false;
        //ChkSMSPartial.Checked = false;
        //chkSMSNoClock.Checked = false;
        //ChkRptAbsent.Checked = false;
        //chkRptLate.Checked = false;
        //chkRptEarly.Checked = false;
        //ChkRptInOut.Checked = false;
        //ChkRptSalary.Checked = false;
        //ChkRptOvertime.Checked = false;
        //ChkRptLog.Checked = false;
        //chkRptDoc.Checked = false;
        //chkRptViolation.Checked = false;
        BindNotification();
        txtScheduleDays.Text = "";
        DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
        dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)lbxGroupNF, dtGroup, "Group_Name", "Group_Id");
        }
        gvEmployeeNF.DataSource = null;
        gvEmployeeNF.DataBind();
        AllPageCode();
    }
    #region Add AddressName Concept
    protected void chkgvSelect_CheckedChangedDefault(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GvAddressName.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
            chk.Checked = false;
        }
        CheckBox chk1 = (CheckBox)sender;
        chk1.Checked = true;
    }
    public DataTable CreateAddressDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("FullAddress");
        dt.Columns.Add("Is_Default", typeof(bool));
        return dt;
    }
    public DataTable FillAddressDataTabel()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateAddressDataTable();
        if (GvAddressName.Rows.Count > 0)
        {
            for (int i = 0; i < GvAddressName.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvAddressName.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
                    Label lblAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
                    dt.Rows[i]["Trans_Id"] = lblSNo.Text;
                    strNewSNo = lblSNo.Text;
                    dt.Rows[i]["Address_Name"] = lblAddressName.Text;
                    try
                    {
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblAddressName.Text);
                    }
                    catch
                    {
                    }
                    dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Trans_Id"] = (float.Parse(strNewSNo) + 1).ToString();
                    dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                    try
                    {
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
                    }
                    catch
                    {
                    }
                    dt.Rows[i]["Is_Default"] = false.ToString();
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";
            dt.Rows[0]["Address_Name"] = txtAddressName.Text;
            try
            {
                dt.Rows[0]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
            }
            catch
            {
            }
            dt.Rows[0]["Is_Default"] = false.ToString();
        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvAddressName, dt, "", "");
        }
        return dt;
    }
    public string GetAddressByAddressName(string AddressName)
    {
        string Address = string.Empty;
        DataTable dt = AM.GetAddressDataByAddressName(AddressName);
        if (dt.Rows.Count > 0)
        {
            Address = dt.Rows[0]["FullAddress"].ToString();
        }
        return Address;
    }
    public string GetContactEmailId(string AddressName)
    {
        string ContactEmailId = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["EmailId1"].ToString() != "")
            {
                ContactEmailId = DtAddress.Rows[0]["EmailId1"].ToString();
            }
        }
        return ContactEmailId;
    }
    public string GetContactFaxNo(string AddressName)
    {
        string ContactFaxNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Work Fax")
                    {
                        ContactFaxNo = ContactFaxNo + "Work: " + dr["Phone_no"].ToString() + " <br/> ";
                    }
                    if (dr["Type"].ToString() == "Home Fax")
                    {
                        ContactFaxNo = ContactFaxNo + "Home: " + dr["Phone_no"].ToString() + " <br/> ";
                    }
                }
            }
            //if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            //{
            //    ContactFaxNo = DtAddress.Rows[0]["FaxNo"].ToString();
            //}
        }
        return ContactFaxNo;
    }
    public string GetContactPhoneNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Work")
                    {
                        ContactPhoneNo = ContactPhoneNo + "Work: " + dr["Phone_no"].ToString() + " <br/> ";
                    }
                    if (dr["Type"].ToString() == "LandLine")
                    {
                        ContactPhoneNo = ContactPhoneNo + "LandLine:" + dr["Phone_no"].ToString() + " <br/> ";
                    }
                    if (dr["Type"].ToString() == "Home")
                    {
                        ContactPhoneNo = ContactPhoneNo + "Home:" + dr["Phone_no"].ToString() + " <br/> ";
                    }
                }
            }
            else
            {
                ContactPhoneNo = "";
            }
            //if (DtAddress.Rows[0]["PhoneNo1"].ToString() != "")
            //{
            //    ContactPhoneNo = DtAddress.Rows[0]["PhoneNo1"].ToString();
            //}
            //if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            //{
            //    if (ContactPhoneNo != "")
            //    {
            //        ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //    else
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["PhoneNo2"].ToString();
            //    }
            //}
        }
        return ContactPhoneNo;
    }
    public string GetContactMobileNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNoData.Rows)
                {
                    if (dr["Type"].ToString() == "Mobile" && dr["Is_default"].ToString() == "True")
                    {
                        ContactPhoneNo = ContactPhoneNo + dr["Phone_no"].ToString() + " ";
                    }
                }
            }
            //    if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["MobileNo1"].ToString();
            //    }
            //    if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            //    {
            //        if (ContactPhoneNo != "")
            //        {
            //            ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
            //        }
            //        else
            //        {
            //            ContactPhoneNo = DtAddress.Rows[0]["MobileNo2"].ToString();
            //        }
            //    }
        }
        return ContactPhoneNo;
    }
    #endregion
    #region Add New Address Concept
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        //pnlAddress2.Visible = false;
    }
    #endregion
    protected void txtEmployeeCode_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "0")
        {
            //DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
            DataTable dt = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), txtEmployeeCode.Text);
            //dt = new DataView(dt, "Emp_Code='" + txtEmployeeCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Employee Code Already Exists");
                txtEmployeeCode.Focus();
                return;
            }
        }
        else
        {
            string EmpCode = string.Empty;
            //DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
            DataTable dt = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), editid.Value);
            try
            {
                EmpCode = dt.Rows[0]["Emp_Code"].ToString();
            }
            catch
            {
                EmpCode = "";
            }
            dt = new DataView(dt, "Emp_Code='" + txtEmployeeCode.Text + "' and Emp_Code<>'" + EmpCode + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Employee Code Already Exist");
                txtEmployeeCode.Text = string.Empty;
                txtEmployeeCode.Focus();
                return;
            }
        }
    }
    protected void txtBankName_TextChanged(object sender, EventArgs e)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        try
        {
            if (editid.Value == "")
            {
                dt = new DataView(objBankMaster.GetBankMaster(), "Bank_Name like '" + txtBankName.Text + "%'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAcountType);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_General_Info_Open()", true);
                    return;
                }
                else
                {
                    txtBankName.Text = "";
                    DisplayMessage("Select Bank from List");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_General_Info_Open()", true);
                    return;
                }
            }
            else
            {
                dt = new DataView(objBankMaster.GetBankMaster(), "Bank_Name like '" + txtBankName.Text + "%'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    txtBankName.Text = "";
                    DisplayMessage("Select Bank From List");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName);
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_General_Info_Open()", true);
                    return;
                }
                else
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAcountType);
                }
            }
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_General_Info_Open()", true);
        }
        catch (Exception ex)
        {
            txtBankName.Text = "";
            DisplayMessage("Bank Name Already Exists");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_General_Info_Open()", true);
            return;
        }
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_General_Info_Open()", true);
        Btn_Div_General_Info.Attributes.Add("Class", "fa fa-minus");
        Div_General_Info.Attributes.Add("Class", "box box-primary");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListNewAddress(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster Address = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Address.GetDistinctAddressName(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Address_Name"].ToString();
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMaster(), "Bank_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Bank_Name"].ToString();
        }
        return txt;
    }
    //protected void btnConnect_Click(object sender, EventArgs e)
    //{
    //    int fileType = -1;
    //    if (fileLoad.HasFile)
    //    {
    //        string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
    //        if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
    //        {
    //            Literal l4 = new Literal();
    //            l4.Text = @"<font size=4 color=red></font><script>alert(""Please load a excel/access file"");</script></br></br>";
    //            this.Controls.Add(l4);
    //            return;
    //        }
    //        if (ext == ".xls")
    //        {
    //            fileType = 0;
    //        }
    //        if (ext == ".xlsx")
    //        {
    //            fileType = 1;
    //        }
    //        if (ext == ".mdb")
    //        {
    //            fileType = 2;
    //        }
    //        if (ext == ".accdb")
    //        {
    //            fileType = 3;
    //        }
    //        string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + fileLoad.FileName);
    //        fileLoad.SaveAs(path);
    //        //DataTable dt//
    //        Import(path, fileType);
    //        //if (dt != null)
    //        //{
    //        //  //  gvLoadContact.DataSource = dt;
    //        //   // Session["dtContact"] = dt;
    //        //   // gvLoadContact.DataBind();
    //        //}
    //        Literal l5 = new Literal();
    //        l5.Text = @"<font size=4 color=red></font><script>alert(""file succesfully uploaded"");</script></br></br>";
    //        this.Controls.Add(l5);
    //    }
    //    else
    //    {
    //        Literal l4 = new Literal();
    //        l4.Text = @"<font size=4 color=red></font><script>alert(""Please load a  file"");</script></br></br>";
    //        this.Controls.Add(l4);
    //    }
    //    //tr0.Visible = true;
    //}
    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }
        Session["cnn"] = strcon;
        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();
        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;
        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();
        conn.Close();
    }
    protected void btnviewcolumns_Click(object sender, EventArgs e)
    {
        OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
        OleDbDataAdapter adp = new OleDbDataAdapter("", "");
        adp.SelectCommand.CommandText = "Select *  From [" + ddlTables.SelectedValue.ToString() + "]";
        adp.SelectCommand.Connection = cnn;
        DataTable userTable = new DataTable();
        try
        {
            adp.Fill(userTable);
        }
        catch (Exception)
        {
            Literal l4 = new Literal();
            l4.Text = @"<font size=4 color=red></font><script>alert(""Error in Mapping File"");</script></br></br>";
            this.Controls.Add(l4);
            return;
        }
        Session["SourceData"] = userTable;
        DataTable dtcolumn = new DataTable();
        dtcolumn.Columns.Add("COLUMN_NAME");
        dtcolumn.Columns.Add("COLUMN");
        for (int i = 0; i < userTable.Columns.Count; i++)
        {
            dtcolumn.Rows.Add(dtcolumn.NewRow());
            if (Session["filetype"].ToString() != "excel")
            {
                dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Columns[i].ToString();
                dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
            }
            else
            {
                dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN_NAME"] = userTable.Rows[0][i].ToString();
                dtcolumn.Rows[dtcolumn.Rows.Count - 1]["COLUMN"] = userTable.Columns[i].ToString();
            }
        }
        Session["SourceTbl"] = dtcolumn;
        //get destination table field 
        DataTable dtDestinationDt = objEmp.GetFieldName(chkNecField.Checked);
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvFieldMapping, dtDestinationDt, "", "");
        //get source field
        //pnlUpload1.Visible = true;
        pnlMap.Visible = true;
        ////pnlUpload1.Visible = false;
        Div_Main_Upload.Visible = false;
    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "EmployeeInformation";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void btnUpload_Click2(object sender, EventArgs e)
    {
        string query = "";
        //// get columns name
        DataTable dtSource = (DataTable)Session["SourceData"];
        if (dtSource.Columns.Count > 2)
        {
            dtSource = new DataView(dtSource, "F6<>''", "", DataViewRowState.CurrentRows).ToTable();
        }
        DataTable dtDestTemp = new DataTable();
        for (int col = 0; col < gvFieldMapping.Rows.Count; col++)
        {
            if (((DropDownList)gvFieldMapping.Rows[col].FindControl("ddlExcelCol")).SelectedValue != "0")
            {
                dtDestTemp.Columns.Add(((Label)gvFieldMapping.Rows[col].FindControl("lblColName")).Text);
            }
        }
        for (int rowcountr = 0; rowcountr < dtSource.Rows.Count; rowcountr++)
        {
            dtDestTemp.Rows.Add(dtDestTemp.NewRow());
            for (int i = 0; i < gvFieldMapping.Rows.Count; i++)
            {
                if (((DropDownList)gvFieldMapping.Rows[i].FindControl("ddlExcelCol")).SelectedValue != "0")
                {
                    dtDestTemp.Rows[rowcountr][((Label)gvFieldMapping.Rows[i].FindControl("lblColName")).Text] = dtSource.Rows[rowcountr][((DropDownList)gvFieldMapping.Rows[i].FindControl("ddlExcelCol")).SelectedValue].ToString();
                }
            }
        }
        //EEmpFirstName','DOJ','DepartmentId','DesignationId','DOB','BrandId','LocationId','EmpId
        if (dtDestTemp.Columns.Contains("Emp_Name") && dtDestTemp.Columns.Contains("DOJ") && dtDestTemp.Columns.Contains("Department_Id") && dtDestTemp.Columns.Contains("Designation_Id") && dtDestTemp.Columns.Contains("DOB") && dtDestTemp.Columns.Contains("Brand_Id") && dtDestTemp.Columns.Contains("Location_Id") && dtDestTemp.Columns.Contains("Emp_Id"))
        {
            ddlFiltercol.DataSource = dtDestTemp.Columns;
        }
        else
        {
            DisplayMessage("Map all Necessary Field");
            return;
        }
        pnlshowdata.Visible = true;
        pnlMap.Visible = false;
        ddlFiltercol.DataBind();
        Session["dtDest"] = dtDestTemp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvSelected, dtDestTemp, "", "");
    }
    protected void gvFieldMapping_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string nec = gvFieldMapping.DataKeys[e.Row.RowIndex]["Nec"].ToString();
            if (nec.Trim() == "1")
            {
                ((Label)e.Row.FindControl("lblCompulsery")).Text = "*";
                ((Label)e.Row.FindControl("lblCompulsery")).ForeColor = System.Drawing.Color.Red;
            }
            DropDownList ddl = ((DropDownList)e.Row.FindControl("ddlExcelCol"));
            binddropdownlist(ddl);
        }
    }
    private void binddropdownlist(DropDownList ddl)
    {
        DataTable dt = (DataTable)Session["SourceTbl"];
        string filetype = Session["filetype"].ToString();
        int startingrow = 0;
        if (filetype == "excel")
            startingrow = 1;
        ListItem lst = new ListItem("--select one--", "0");
        if (ddl != null)
        {
            ddl.Items.Insert(0, lst);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst = new ListItem(dt.Rows[i]["COLUMN_NAME"].ToString(), dt.Rows[i]["COLUMN"].ToString());
                ddl.Items.Insert(i + 1, lst);
                //lst=new ListItem()
            }
        }
    }
    protected void btncancel_Click(object sender, EventArgs e)
    {
        //pnlUpload1.Visible = true;
        pnlMap.Visible = false;
        pnlshowdata.Visible = false;
        Div_Main_Upload.Visible = true;
    }
    protected void btnFilter_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtDest"];
        dt = new DataView(dt, "" + ddlFiltercol.SelectedValue + "='" + txtfiltercol.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvSelected, dt, "", "");
    }
    protected void btnresetgv_Click(object sender, EventArgs e)
    {
        pnlshowdata.Visible = false;
        pnlMap.Visible = true;
        //pnlUpload1.Visible = false;
        txtfiltercol.Text = "";
        //btnUpload_Click(null, null);
        // trnew.Visible = false;
    }
    public void SetAllId()
    {
        objEmp.CompanyId = Session["CompId"].ToString();
        objEmp.Gender = "Male";
        objEmp.NationalityId = "0";
        objEmp.ReligionId = "0";
        objEmp.Service_Status = "Active";
        objEmp.EEmpFirstName = "";
        objEmp.LEmpFirstName = "";
        objEmp.EmployeeImage = "";
        objEmp.CivilId = "0";
        objEmp.CreatedBy = Session["UserId"].ToString();
        objEmp.CreatedDate = DateTime.Now.ToString();
        objEmp.DepartmentId = "0";
        objEmp.DOL = "";
        objEmp.Education = "";
        objEmp.EmailId = "";
        objEmp.EmpType = "On Role";
        objEmp.NationalityId = "0";
        objEmp.ReligionId = "0";
        objEmp.IsActive = true.ToString();
        objEmp.ModifiedBy = Session["UserId"].ToString();
        objEmp.ModifiedDate = DateTime.Now.ToString();
        objEmp.PhoneNo = "123";
        objEmp.ReligionId = "0";
    }
    protected void btnUpload_Click1(object sender, EventArgs e)
    {
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        string strLabourLaw = string.Empty;
        string empids = "";
        string Date;
        string Month;
        string Year;
        string ActualDate;
        int Insertedrowcount = 0;
        int updatedrowcount = 0;
        DateTime date_Of_Joining = new DateTime();
        string compid = Session["CompId"].ToString();
        DateTime doj;
        for (int rowcounter = 1; rowcounter < gvSelected.Rows.Count; rowcounter++)
        {
            SetAllId();
            for (int col = 0; col < gvSelected.Rows[rowcounter].Cells.Count; col++)
            {
                string colname = gvSelected.HeaderRow.Cells[col].Text;
                string colval = gvSelected.Rows[rowcounter].Cells[col].Text;
                colval = colval.Replace("&#160;", "");
                colval = colval.Replace("&nbsp;", "");
                if (colname == "Emp_Name")
                {
                    objEmp.EEmpFirstName = colval;
                }
                else if (colname == "Email_Id")
                {
                    objEmp.EmailId = colval;
                }
                else if (colname == "DOJ")
                {
                    doj = Convert.ToDateTime(colval);
                    objEmp.DOJ = doj.ToString();
                    objEmp.IncrementDate = doj.AddMonths(Fr_IncrementDuration).ToString();
                    //colval = colval.Replace(" 0:00", "");
                    //DateTime Doj;
                    //if (colval == "")
                    //{
                    //    colval = DateTime.Now.ToString("dd/MM/yyyy");
                    //}
                    //string strDate = colval;
                    //string[] words = strDate.Split('/');
                    //if (words.Length == 1)
                    //{
                    //    words = strDate.Split('-');
                    //}
                    //Date = words[0].ToString();
                    //if (Date.Length == 1)
                    //{
                    //    Date = "0" + Date;
                    //}
                    //else
                    //{
                    //    Date = Date;
                    //}
                    //Month = words[1].ToString();
                    //if (Month.Length == 1)
                    //{
                    //    Month = "0" + Month;
                    //}
                    //else
                    //{
                    //    Month = Month; ;
                    //}
                    //Year = words[2].ToString();
                    //if (Year.Length == 1)
                    //{
                    //    Year = "200" + Year;
                    //}
                    //else if (Year.Length == 2)
                    //{
                    //    Year = "20" + Year;
                    //}
                    //else if (Year.Length == 3)
                    //{
                    //    Year = "2" + Year;
                    //}
                    //else if (Year.Length == 4)
                    //{
                    //    Year = Year;
                    //}
                    //else if (Year.Length == 2)
                    //{
                    //}
                    //else
                    //{
                    //}
                    //if (Date.Length == 2)
                    //{
                    //    ActualDate = Date + "/" + Month + "/" + Year;
                    //}
                    //else
                    //{
                    //    string dd = Year.Remove(0, 2);
                    //    ActualDate = dd + "/" + Month + "/" + Date;
                    //}
                    //try
                    //{
                    //    Doj = DateTime.ParseExact(ActualDate.Trim(), "dd/MM/yyyy", null);
                    //    date_Of_Joining = Doj;
                    //    objEmp.DOJ = Convert.ToString(Doj);
                    //    // Modified By Nitin Jain On 20/11/2014 For Increment Date
                    //    Increment_Date = Doj.AddMonths(Fr_IncrementDuration);
                    //    objEmp.IncrementDate = Convert.ToString(Increment_Date);
                    //}
                    //catch
                    //{
                    //    objEmp.DOJ = Convert.ToString(DateTime.ParseExact(ActualDate.Trim(), "MM/dd/yyyy", null));
                    //    date_Of_Joining = DateTime.ParseExact(ActualDate.Trim(), "MM/dd/yyyy", null);
                    //    // Modified By Nitin Jain On 20/11/2014 For Increment Date
                    //    Increment_Date = Convert.ToDateTime(ActualDate).AddMonths(Fr_IncrementDuration);
                    //    objEmp.IncrementDate = Convert.ToString(Increment_Date);
                    //}
                }
                else if (colname == "Department_Id")
                {
                    if (colval != "")
                    {
                        objEmp.DepartmentId = colval;
                    }
                }
                else if (colname == "Designation_Id")
                {
                    if (colval != "")
                    {
                        objEmp.DesignationId = colval;
                    }
                }
                else if (colname == "Nationality_Id")
                {
                    if (colval != "")
                    {
                        objEmp.NationalityId = colval;
                    }
                }
                else if (colname == "Gender")
                {
                    objEmp.Gender = colval;
                }
                else if (colname == "Religion_Id")
                {
                    if (colval != "")
                    {
                        objEmp.ReligionId = colval;
                    }
                }
                else if (colname == "Brand_Id")
                {
                    objEmp.BrandId = colval;
                }
                else if (colname == "Location_Id")
                {
                    objEmp.LocationId = colval;
                }
                else if (colname == "Emp_Id")
                {
                    objEmp.EmpId = colval;
                }
                else if (colname == "Nationality_Id")
                {
                    if (colval != "")
                    {
                        objEmp.NationalityId = colval;
                    }
                }
                else if (colname == "Gender")
                {
                    objEmp.Gender = colval;
                }
                else if (colname == "DOB")
                {
                    objEmp.DOB = Convert.ToDateTime(colval).ToString();

                }
                else if (colname == "Religion_Id")
                {
                    if (colval != "")
                    {
                        objEmp.ReligionId = colval;
                    }
                }
                else if (colname == "Company_Id")
                {
                    objEmp.CompanyId = colval;
                }
                else if (colname == "Brand_Id")
                {
                    objEmp.BrandId = colval;
                }
                else if (colname == "Location_Id")
                {
                    if (colval != "")
                    {
                        objEmp.LocationId = colval;
                    }
                }
                else if (colname == "Civil_Id")
                {
                    if (colval != "")
                    {
                        objEmp.CivilId = colval;
                    }
                }
                else if (colname == "Phone_No")
                {
                    objEmp.PhoneNo = colval;
                }
                else if (colname == "Emp_Name_L")
                {
                    objEmp.LEmpFirstName = colval;
                }
                else if (colname == "Emp_Image")
                {
                    objEmp.EmployeeImage = colval;
                }
            }
            if (objEmp.EmpId == "")
            {
                continue;
            }
            DataTable dtEmp = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), objEmp.EmpId);
            int b = 0;
            DataTable dtDevice = new DataTable();
            if (dtEmp.Rows.Count > 0)
            {
                b = objDa.execute_Command("update set_employeemaster set emp_name ='" + objEmp.EEmpFirstName + "',DOB='" + objEmp.DOB.ToString() + "',DOJ='" + objEmp.DOJ.ToString() + "',Department_Id='" + objEmp.DepartmentId + "',Email_Id='" + objEmp.EmailId + "' where Company_Id=" + Session["CompId"].ToString() + " and  Emp_Id='" + dtEmp.Rows[0]["Emp_Id"].ToString() + "'");
                updatedrowcount++;
            }
            else
            {
                b = objEmp.SaveEmpData();
                if (b != 0)
                {
                    Insertedrowcount++;
                }
                int IndemnityYear = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                // Nitin Jain On 27/11/2014 , Insert Into Indemnity Table 
                int Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), b.ToString(), Convert.ToDateTime(objEmp.DOJ).AddYears(IndemnityYear).ToString(), "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //this code is created by jitendra upadhyay on 19-09-2014
                //this code for insert the record in notifications table of new employee
                //code start
                DataTable dtNf = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
                foreach (DataRow dr in dtNf.Rows)
                {
                    try
                    {
                        objEmpNotice.InsertEmployeeNotification(b.ToString(), dr["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    catch
                    {
                    }
                }
                //code end
                objEmpParam.DeleteEmployeeParameterByEmpId(b.ToString());
                objEmpParam.InsertEmployeeParameterOnEmployeeInsert(Session["CompId"].ToString(), b.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                int c = 0;
                int d = 0;
                int f = 0;
                bool IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                string IndemnityDays = objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                objEmpParam.DeleteEmployeeParameterByEmpIdNew(b.ToString());
                c = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                d = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IndemnityYear", IndemnityYear.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                f = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "Indemenity_SalaryCalculationType", IndemnityDays.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //Insert For Notification
                int N = 0;
                objEmpNotice.DeleteEmployeeNotificationByEmpId(b.ToString());
                DataTable dtNoti = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
                if (dtNoti.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNoti.Rows.Count; i++)
                    {
                        N = objEmpNotice.InsertEmployeeNotification(b.ToString(), dtNoti.Rows[i]["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
                //here we are saving leave assing if labour law assigned for current location 
                //here we are assigning leave to employee according labour law for selected location
                //code added on 24/10/2017 by jitendra upadhyay
                //code start
                strLabourLaw = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), objEmp.LocationId).Rows[0]["Field3"].ToString();
                if (strLabourLaw.Trim() != "0" && strLabourLaw.Trim() != "")
                {
                    DataTable dtleavedetail = ObjLabourLeavedetail.GetRecord_By_LaborLawId(strLabourLaw);
                    dtleavedetail = new DataView(dtleavedetail, "Gender='" + ddlGender.SelectedItem.Text + "' or Gender='Both'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow dr in dtleavedetail.Rows)
                    {
                        objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), b.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), "100", dr["schedule_type"].ToString(), true.ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), dr["is_auto"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        SaveLeave("No", dr["Leave_Type_Id"].ToString(), b.ToString(), dr["schedule_type"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), FinancialYearStartDate, FinancialYearEndDate, Convert.ToDateTime(objEmp.DOJ));
                        //here we are checking that any deduction slab exists or not for selected leave type
                        DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(dr["Leave_Type_Id"].ToString()).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");
                        foreach (DataRow childrow in dtdeduction.Rows)
                        {
                            ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, dr["Leave_Type_Id"].ToString(), b.ToString(), childrow["DaysFrom"].ToString(), childrow["Daysto"].ToString(), childrow["Deduction_Percentage"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                //code end
                if (EmpSync == "Company")
                {
                    dtDevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());
                    if (dtDevice.Rows.Count > 0)
                    {
                        objSer.DeleteUserTransfer(b.ToString());
                        //for (int i = 0; i < listEmpSync.Items.Count; i++)
                        //{
                        //    objSer.InsertUserTransfer(b.ToString(), listEmpSync.SelectedValue.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //}
                        //16-04-2015
                        foreach (ListItem li in listEmpSync.Items)
                        {
                            objSer.InsertUserTransfer(b.ToString(), li.Value.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                else
                {
                    dtDevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());
                    dtDevice = new DataView(dtDevice, "Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDevice.Rows.Count > 0)
                    {
                        objSer.DeleteUserTransfer(b.ToString());
                        //for (int i = 0; i < listEmpSync.Items.Count; i++)
                        //{
                        //    objSer.InsertUserTransfer(b.ToString(), listEmpSync.SelectedValue.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        //}
                        //16-04-2015
                        foreach (ListItem li in listEmpSync.Items)
                        {
                            objSer.InsertUserTransfer(b.ToString(), li.Value.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }
            }
            if (b != 0)
            {
            }
            else
            {
                empids += "'" + objEmp.EmpId + "'" + ",";
            }
        }
        //string emp = empids;
        //if (emp != "")
        //{
        //    DataTable dtemp = (DataTable)Session["dtDest"];
        //    emp = emp.Substring(0, emp.Length - 1);
        //    dtemp = new DataView(dtemp, "Emp_Id in(" + emp + ")", "", DataViewRowState.CurrentRows).ToTable();
        //    //Common Function add By Lokesh on 14-05-2015
        //    objPageCmn.FillData((object)gvSelected, dtemp, "", "");
        //}
        pnlshowdata.Visible = false;
        pnlMap.Visible = false;
        Div_Main_Upload.Visible = true;
        ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('" + Insertedrowcount + " Row Inserted and " + updatedrowcount + " Row updated')", true);
    }
    protected void btnbacktoExcel_Click(object sender, EventArgs e)
    {
        //pnlUpload1.Visible = true;
        //trmap.Visible = false;
    }
    protected void btnBackToMapData_Click(object sender, EventArgs e)
    {
        pnlshowdata.Visible = false;
        pnlMap.Visible = false;
        Div_Main_Upload.Visible = true;
    }
    #region UploadSignature
    protected void btnuploadsignature_Click(object sender, EventArgs e)
    {
        if (Session["empSignimgpath"].ToString() != "")
            ImgEmpSignature.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPathEmployeeSign.FileName;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Additional_Info_Open()", true);
    }
    #endregion
    //For Add new Bin Concept On 26-06-2015
    protected void btnTerDel_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        if (btnTerDel.ToolTip == "Deleted")
        {
            btnTerDel.ToolTip = "Terminated";

            FillbinDataListGrid();
        }
        else if (btnTerDel.ToolTip == "Terminated")
        {
            btnTerDel.ToolTip = "Deleted";
            FillbinDataListGrid();
        }
    }
    #region LeaveDeductionSlab
    //delete
    protected void imgBtnEmpoloyeeDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtemp = GetDeductionList();
        dtemp = new DataView(dtemp, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)GvDeductionDetail, dtemp, "", "");
        txtexceedDays.Text = GetExceedFromValue();
        dtemp.Dispose();
    }
    //edit
    protected void imgBtnEmployeeEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = GetDeductionList();
        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        txtexceedDays.Text = dt.Rows[0]["DaysFrom"].ToString();
        txtexceedDaysto.Text = dt.Rows[0]["DaysTo"].ToString();
        txtexceedDaysto.Enabled = false;
        txtdeduction.Text = dt.Rows[0]["Deduction_Percentage"].ToString();
        hdndeductionTransId.Value = e.CommandArgument.ToString();
        dt.Dispose();
    }
    protected void btndeduction_Click(object sender, EventArgs e)
    {
        if (txtexceedDays.Text == "")
        {
            DisplayMessage("Enter Exceed From days");
            txtexceedDays.Focus();
            return;
        }
        if (txtexceedDaysto.Text == "")
        {
            DisplayMessage("Enter Exceed to days");
            txtexceedDaysto.Focus();
            return;
        }
        if (float.Parse(txtexceedDays.Text) > float.Parse(txtexceedDaysto.Text))
        {
            DisplayMessage("exceed days to value should be greater or equal to exceed days from value");
            txtexceedDaysto.Focus();
            return;
        }
        if (txtdeduction.Text == "")
        {
            DisplayMessage("Enter deduction percentage");
            txtdeduction.Focus();
            return;
        }
        DataTable dt = GetDeductionList();
        if (hdndeductionTransId.Value == "" || hdndeductionTransId.Value == "0")
        {
            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1][0] = dt.Rows.Count + 1;
            dt.Rows[dt.Rows.Count - 1][1] = txtexceedDays.Text;
            dt.Rows[dt.Rows.Count - 1][2] = txtexceedDaysto.Text;
            dt.Rows[dt.Rows.Count - 1][3] = txtdeduction.Text;
        }
        else
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString() == hdndeductionTransId.Value)
                {
                    dt.Rows[i][1] = txtexceedDays.Text;
                    dt.Rows[i][2] = txtexceedDaysto.Text;
                    dt.Rows[i][3] = txtdeduction.Text;
                    break;
                }
            }
        }
        objPageCmn.FillData((object)GvDeductionDetail, dt, "", "");
        btndeductionCancel_Click(null, null);
    }
    public DataTable GetDeductionList()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("DaysFrom");
        dt.Columns.Add("DaysTo", typeof(float));
        dt.Columns.Add("Deduction_Percentage");
        foreach (GridViewRow gvrow in GvDeductionDetail.Rows)
        {
            DataRow dr = dt.NewRow();
            dr[0] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr[1] = ((Label)gvrow.FindControl("lblDaysFrom")).Text;
            dr[2] = ((Label)gvrow.FindControl("lblDaysTo")).Text;
            dr[3] = ((Label)gvrow.FindControl("lbldeductionpercentage")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void btndeductionCancel_Click(object sender, EventArgs e)
    {
        txtexceedDays.Text = "";
        txtexceedDaysto.Text = "";
        txtdeduction.Text = "";
        txtexceedDays.Focus();
        hdndeductionTransId.Value = "";
        txtexceedDays.Text = GetExceedFromValue();
        txtexceedDaysto.Focus();
        txtexceedDaysto.Enabled = true;
    }
    public string GetExceedFromValue()
    {
        string strvalue = "0";
        DataTable dt = GetDeductionList();
        if (dt.Rows.Count > 0)
        {
            strvalue = (float.Parse(new DataView(dt, "", "DaysTo desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["DaysTo"].ToString()) + 1).ToString();
        }
        return strvalue;
    }
    protected void ddlLeaveType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        trdeductionSlab.Visible = false;
        txtexceedDays.Text = "";
        txtexceedDaysto.Text = "";
        txtdeduction.Text = "";
        hdndeductionTransId.Value = "";
        objPageCmn.FillData((object)GvDeductionDetail, null, "", "");
        if (ddlLeaveType.SelectedIndex > 0)
        {
            DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(ddlLeaveType.SelectedValue).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");
            if (dtdeduction.Rows.Count > 0)
            {
                txtTotalLeave.Text = "0";
                txtPaidLeave.Text = "0";
                //ddlSchType.SelectedIndex = 2;
                //ddlSchedule_SelectedIndexChanged(null, null);
                trdeductionSlab.Visible = true;
                objPageCmn.FillData((object)GvDeductionDetail, dtdeduction, "", "");
                txtexceedDays.Text = GetExceedFromValue();
                dtdeduction.Dispose();
            }
        }
    }
    protected void lnkLeaveViewDetail_Command(object sender, CommandEventArgs e)
    {
        //this code is created by jitendra upadhyay on  13-09-2014
        //this code for view the employee leave 
        //code start
        string empid = e.CommandArgument.ToString();
        string empname = GetEmployeeName(empid);
        lblEmpNameLeaveView.Text = empname;
        lblEmpCodeLeaveView.Text = GetEmployeeCode(empid);
        DataTable dtEmpSal = objEmpParam.GetEmployeeParameterByEmpId(empid, Session["CompId"].ToString());
        if (dtEmpSal.Rows.Count > 0)
        {
            txtHalfDAyView.Text = dtEmpSal.Rows[0]["Field12"].ToString();
        }
        DataTable dtEmpLeave = objEmpleave.GetEmployeeLeaveByEmpId(Session["CompId"].ToString(), empid);
        if (dtEmpLeave.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvLeaveEmpView, dtEmpLeave, "", "");
        }
        DataTable dtdeduction = ObjLeavededuction.GetRecordbyEmployeeId(empid).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage", "Leave_Name");
        objPageCmn.FillData((object)gvLeavededuction, dtdeduction, "", "");
        dtdeduction.Dispose();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Leave_View()", true);
        //pnl2View.Visible = true;
    }
    protected void btnEditLeave_Command(object sender, CommandEventArgs e)
    {
        bool Ishalfdayexist = false;
        bool IsFulldayLeaveexist = false;
        //this code is modify by jitendra upadhyay on  02-09-2014
        //here we check that if employee used the full day leave or half day leave than can not edit
        //code start
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        Att_Leave_Request objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        DataTable dtLeaveR = objleaveReq.GetLeaveRequestById(Session["CompId"].ToString(), e.CommandArgument.ToString());
        try
        {
            DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            int FinancialYearMonth = 0;
            if (dt.Rows.Count > 0)
            {
                FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
            }
            if (DateTime.Now.Month < FinancialYearMonth)
            {
                FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            else
            {
                FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
                FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
            }
            dtLeaveR = new DataView(dtLeaveR, "CreatedDate>='" + FinancialYearStartDate + "' and CreatedDate<='" + FinancialYearEndDate + "' ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        //try
        //{
        //    dtLeaveR = new DataView(dtLeaveR, "Leave_Type_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //catch
        //{
        //}
        if (dtLeaveR.Rows.Count > 0)
        {
            gvLeaveEmp.DataSource = null;
            gvLeaveEmp.DataBind();
            // IsFulldayLeaveexist = true;
            gvLeaveEmp.Visible = false;
            //Button1.Visible = false;
            //Button2.Visible = false;
            //DisplayMessage("This Leave is in used ,you can not Edit");
            //return;
        }
        else
        {
            gvLeaveEmp.Visible = true;
            //Button1.Visible = true;
            //Button2.Visible = true;
        }
        //here we check that half day is exist or not that employee
        DataTable dtHalfDay = objHalfDay.GetHalfDayRequestById(Session["CompId"].ToString(), e.CommandArgument.ToString());
        dtHalfDay = new DataView(dtHalfDay, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        try
        {
            dtHalfDay = new DataView(dtHalfDay, "CreatedDate>='" + FinancialYearStartDate + "' and CreatedDate<='" + FinancialYearEndDate + "' ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtHalfDay.Rows.Count > 0)
        {
            Ishalfdayexist = true;
            Label100.Visible = false;
            //lblHalfdayLeavecolon.Visible = false;
            txtHalfDAy.Visible = false;
            btnUpdateHalfday.Visible = false;
        }
        else
        {
            Label100.Visible = true;
            //lblHalfdayLeavecolon.Visible = true;
            txtHalfDAy.Visible = true;
            btnUpdateHalfday.Visible = true;
        }

        if (Ishalfdayexist == true && IsFulldayLeaveexist == true)
        {
            DisplayMessage("Employee Used Leave");
            return;
        }
        //code end
        string empid = e.CommandArgument.ToString();
        string empname = GetEmployeeName(empid);
        lblEmpNameLeave.Text = empname;
        lblEmpCodeLeave.Text = GetEmployeeCode(empid);
        hdnEmpIdHalfDay.Value = e.CommandArgument.ToString();
        DataTable dtEmpSal = objEmpParam.GetEmployeeParameterByEmpId(empid, Session["CompId"].ToString());
        if (dtEmpSal.Rows.Count > 0)
        {
            txtHalfDAy.Text = dtEmpSal.Rows[0]["Field12"].ToString();
        }
        if (Ishalfdayexist == true)
        {
            txtHalfDAy.Enabled = false;
            //btnSaveHalfday.Enabled = false;
        }
        else
        {
            txtHalfDAy.Enabled = true;
            //btnSaveHalfday.Enabled = true;
        }
        if (!IsFulldayLeaveexist)
        {
            string LeaveTypeId = string.Empty;
            for (int LT = 0; LT <= dtLeaveR.Rows.Count - 1; LT++)
            {
                if (dtLeaveR.Rows[LT]["Is_Pending"].ToString() == "True" || dtLeaveR.Rows[LT]["Is_Approved"].ToString() == "True")
                {
                    LeaveTypeId += dtLeaveR.Rows[LT]["Leave_Type_id"].ToString() + ",";
                }
            }
            gvLeaveEmp.Visible = false;
            gvLeaveEmp.DataSource = null;
            gvLeaveEmp.DataBind();
            DataTable dtEmpLeave = objEmpleave.GetEmployeeLeaveByEmpId(Session["CompId"].ToString(), empid);
            if (LeaveTypeId != "")
            {
                dtEmpLeave = new DataView(dtEmpLeave, "LeaveType_Id NOT IN(" + LeaveTypeId.Substring(0, LeaveTypeId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            Session["dtleaveDetail_Edit"] = dtEmpLeave;
            if (dtEmpLeave.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gvLeaveEmp, dtEmpLeave, "", "");
                foreach (GridViewRow gvr in gvLeaveEmp.Rows)
                {
                    string Schtype = ((DropDownList)gvr.FindControl("ddlSchType0")).SelectedValue;
                    if (Schtype == "Yearly")
                    {
                        ((CheckBox)gvr.FindControl("chkYearCarry0")).Enabled = true;
                    }
                    else
                    {
                        ((CheckBox)gvr.FindControl("chkYearCarry0")).Enabled = false;
                    }
                }
                gvLeaveEmp.Visible = true;
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Leave()", true);
        //pnl2.Visible = true;
        if (gvLeaveEmp.Rows.Count > 0)
        {
            Button1.Visible = true;
            Button1.Visible = true;
        }
        else
        {
            Button1.Visible = false;
            Button1.Visible = false;
        }
    }
    protected void btnUpdateLeave_Click(object sender, EventArgs e)
    {
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        int b = 0;
        string strleaveValidation = string.Empty;
        foreach (GridViewRow gvr in gvLeaveEmp.Rows)
        {
            string NoOfLeave = ((TextBox)gvr.FindControl("txtNoOfLeave0")).Text;
            string PaidLeave = ((TextBox)gvr.FindControl("txtPAidLEave1")).Text;
            string PerSalary = ((TextBox)gvr.FindControl("TextBox1")).Text;
            string Schtype = ((DropDownList)gvr.FindControl("ddlSchType0")).SelectedValue;
            bool isYearcarry = ((CheckBox)gvr.FindControl("chkYearCarry0")).Checked;
            bool IsRule = ((CheckBox)gvr.FindControl("chkIsRule")).Checked;
            bool IsAuto = ((CheckBox)gvr.FindControl("chkIsAuto")).Checked;
            string leavetypeid = ((HiddenField)gvr.FindControl("hdnLeaveTypeId")).Value;
            string TransNo = ((HiddenField)gvr.FindControl("hdnTranNo")).Value;
            string empid = ((HiddenField)gvr.FindControl("hdnEmpId1")).Value;
            //if (isYearcarry == IsAuto)
            //{
            //    ((CheckBox)gvr.FindControl("chkIsAuto")).Checked = false;
            //}
            if (((TextBox)gvr.FindControl("txtNoOfLeave0")).Text == "")
            {
                DisplayMessage("Enter value");
                ((TextBox)gvr.FindControl("txtNoOfLeave0")).Focus();
                return;
            }
            if (((TextBox)gvr.FindControl("txtPAidLEave1")).Text == "")
            {
                DisplayMessage("Enter value");
                ((TextBox)gvr.FindControl("txtPAidLEave1")).Focus();
                return;
            }
            if (float.Parse(PaidLeave) > float.Parse(NoOfLeave))
            {
                DisplayMessage("No of paid leave cannot be greater than total leave");
                ((TextBox)gvr.FindControl("txtPAidLEave1")).Text = "0";
                ((TextBox)gvr.FindControl("txtPAidLEave1")).Focus();
                return;
            }
            string PrevSchType = string.Empty;
            string PrevAssignLeave = string.Empty;
            DataTable dtEmpleave = objEmpleave.GetEmployeeLeaveByTransId(Session["CompId"].ToString(), TransNo);
            if (dtEmpleave.Rows.Count > 0)
            {
                PrevSchType = dtEmpleave.Rows[0]["Shedule_Type"].ToString();
                PrevAssignLeave = dtEmpleave.Rows[0]["Total_Leave"].ToString();
            }
            DataTable dtleaveTrans = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(empid);
            dtleaveTrans = new DataView(dtleaveTrans, "Field3='Open' and Leave_Type_Id=" + leavetypeid + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtleaveTrans.Rows.Count > 0)
            {
                if (Convert.ToDouble(dtleaveTrans.Rows[0]["Used_Days"].ToString()) > 0 || Convert.ToDouble(dtleaveTrans.Rows[0]["Pending_Days"].ToString()) > 0)
                {
                    strleaveValidation += dtleaveTrans.Rows[0]["Leave_Name"].ToString() + ",";
                    continue;
                }
            }
            if (Schtype == "Monthly")
            {
                b = objEmpleave.UpdateEmployeeLeaveByTransNo(TransNo, Session["CompId"].ToString(), empid, leavetypeid, NoOfLeave, PaidLeave, "100", Schtype, false.ToString(), false.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                SaveLeave("Yes", leavetypeid, empid, Schtype, NoOfLeave, PaidLeave, "False", PrevSchType, PrevAssignLeave, TransNo, ((CheckBox)gvr.FindControl("chkIsRule")).Checked.ToString(), FinancialYearStartDate, FinancialYearEndDate, DateTime.Now);
            }
            else if (Schtype == "Yearly")
            {
                b = objEmpleave.UpdateEmployeeLeaveByTransNo(TransNo, Session["CompId"].ToString(), empid, leavetypeid, NoOfLeave, PaidLeave, "100", Schtype, true.ToString(), isYearcarry.ToString(), "", "", "", IsRule.ToString(), IsAuto.ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                SaveLeave("Yes", leavetypeid, empid, Schtype, NoOfLeave, PaidLeave, "False", PrevSchType, PrevAssignLeave, TransNo, ((CheckBox)gvr.FindControl("chkIsRule")).Checked.ToString(), FinancialYearStartDate, FinancialYearEndDate, DateTime.Now);
            }
        }
        if (b != 0)
        {
            if (strleaveValidation != "")
            {
                DisplayMessage("Record not updated for = " + strleaveValidation + "");
            }
            else
            {
                DisplayMessage("Record Updated", "green");
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Leave()", true);
            SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Leave Updated", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
        }
        else
        {

            DisplayMessage("Record not saved");

        }
    }
    protected void btnSaveLeave_Click(object sender, EventArgs e)
    {
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
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
                DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());
                dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
                {
                    if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                    {
                        EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                    }
                }
            }
            else
            {
                DisplayMessage("Select Group First");
            }
            if (ddlLeaveType.SelectedIndex == 0)
            {
                DisplayMessage("Select Leave Type");
                return;
            }
            if (txtTotalLeave.Text == "")
            {
                DisplayMessage("Enter Total Leave");
                return;
            }
            if (txtPaidLeave.Text == "")
            {
                DisplayMessage("Enter Paid Leave");
                return;
            }

            int totleave = int.Parse(txtTotalLeave.Text);
            int paidleave = int.Parse(txtPaidLeave.Text);
            if (paidleave > totleave)
            {
                txtPaidLeave.Text = "0";
                txtPaidLeave.Focus();
                DisplayMessage("Paid leave cannot be greater then total leave");
                return;
            }
            foreach (string str in EmpIds.Split(','))
            {
                if (str != "")
                {
                    DataTable dtEmpLeave = objEmpleave.GetEmployeeLeaveByEmpId(Session["CompId"].ToString(), str);
                    dtEmpLeave = new DataView(dtEmpLeave, "LeaveType_Id='" + ddlLeaveType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtEmpLeave.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        // Nitin Jain 19/01/2015......
                        bool IsYearCarry = false;
                        bool IsAuto = false;
                        if (chkYearCarry.Checked)
                        {
                            IsYearCarry = true;
                        }
                        if (chkIsAuto.Checked)
                        {
                            IsAuto = true;
                        }
                        b = objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), str, ddlLeaveType.SelectedValue, txtTotalLeave.Text, txtPaidLeave.Text, txtPerSal.Text, ddlSchType.SelectedValue, true.ToString(), IsYearCarry.ToString(), "", "", "", ChkIsRule.Checked.ToString(), IsAuto.ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        SaveLeave("No", ddlLeaveType.SelectedValue, str, ddlSchType.SelectedValue, txtTotalLeave.Text, txtPaidLeave.Text, IsYearCarry.ToString(), "", "", "", ChkIsRule.Checked.ToString(), FinancialYearStartDate, FinancialYearEndDate, DateTime.Now);
                        TransNo = b.ToString();
                    }
                    //here we are inserting record in deduction slab
                    ObjLeavededuction.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlLeaveType.SelectedValue, str);
                    foreach (GridViewRow gvr in GvDeductionDetail.Rows)
                    {
                        b = ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlLeaveType.SelectedValue, str, ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
        }
        else
        {
            ArrayList userdetails = new ArrayList();
            SaveCheckedValues(gvEmpLeave);
            if (Session["CHECKED_ITEMS"] == null)
            {
                DisplayMessage("Select employee first");
                return;
            }
            else
            {
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Select employee first");
                    return;
                }
            }
            if (ddlLeaveType.SelectedIndex == 0)
            {
                DisplayMessage("Select Leave Type");
                return;
            }
            if (txtTotalLeave.Text == "")
            {
                DisplayMessage("Enter Total Leave");
                return;
            }
            if (txtPaidLeave.Text == "")
            {
                DisplayMessage("Enter Paid Leave");
                return;
            }
            //if (ddlSchType.SelectedIndex == 0)
            //{
            //    DisplayMessage("Select Schedule Type");
            //    return;
            //}
            int totleave = int.Parse(txtTotalLeave.Text);
            int paidleave = int.Parse(txtPaidLeave.Text);
            if (paidleave > totleave)
            {
                txtPaidLeave.Text = "0";
                txtPaidLeave.Focus();
                DisplayMessage("Paid leave cannot be greater then total leave");
                return;
            }
            for (int i = 0; i < userdetails.Count; i++)
            {
                DataTable dtEmpLeave = objEmpleave.GetEmployeeLeaveByEmpIdandLeaveTypeId(Session["CompId"].ToString(), userdetails[i].ToString(), ddlLeaveType.SelectedValue);
                //dtEmpLeave = new DataView(dtEmpLeave, "LeaveType_Id='" + ddlLeaveType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtEmpLeave.Rows.Count > 0)
                {
                }
                else
                {
                    bool IsYearCarry = false;
                    bool IsAuto = false;
                    if (chkYearCarry.Checked)
                    {
                        IsYearCarry = true;
                    }
                    if (chkIsAuto.Checked)
                    {
                        IsAuto = true;
                    }
                    b = objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), userdetails[i].ToString(), ddlLeaveType.SelectedValue, txtTotalLeave.Text, txtPaidLeave.Text, txtPerSal.Text, ddlSchType.SelectedValue, true.ToString(), IsYearCarry.ToString(), "", "", "", ChkIsRule.Checked.ToString(), IsAuto.ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    SaveLeave("No", ddlLeaveType.SelectedValue, userdetails[i].ToString(), ddlSchType.SelectedValue, txtTotalLeave.Text, txtPaidLeave.Text, IsYearCarry.ToString(), "", "", "", ChkIsRule.Checked.ToString(), FinancialYearStartDate, FinancialYearEndDate, DateTime.Now);
                    TransNo = b.ToString();
                }
                //here we are inserting record in deduction slab
                ObjLeavededuction.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlLeaveType.SelectedValue, userdetails[i].ToString());
                foreach (GridViewRow gvr in GvDeductionDetail.Rows)
                {
                    b = ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlLeaveType.SelectedValue, userdetails[i].ToString(), ((Label)gvr.FindControl("lblDaysFrom")).Text, ((Label)gvr.FindControl("lblDaysTo")).Text, ((Label)gvr.FindControl("lbldeductionpercentage")).Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }
        if (b != 0)
        {
            DisplayMessage("Record Saved", "green");
            DataTable dtEmpLeave = new DataTable();
            DataTable dt = objEmpleave.GetEmployeeLeaveByTransId(Session["CompId"].ToString(), TransNo);
            if (Session["dtLeave"] != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ((DataTable)Session["dtLeave"]).Merge(dt);
                    dtEmpLeave = (DataTable)Session["dtLeave"];
                }
                else
                {
                    dtEmpLeave = (DataTable)Session["dtLeave"];
                }
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    Session["dtLeave"] = dt;
                    dtEmpLeave = (DataTable)Session["dtLeave"];
                }
            }
            if (dtEmpLeave.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 14-05-2015
                objPageCmn.FillData((object)gridEmpLeave, dtEmpLeave, "", "");
            }
            // //chkYearCarry.Visible = false;
            ddlLeaveType.SelectedIndex = 0;
            ddlSchType.SelectedIndex = 0;
            txtPaidLeave.Text = "";
            txtTotalLeave.Text = "";
            txtPerSal.Text = "100";
            chkMonthCarry.Checked = false;
        }
        else
        {
            DisplayMessage("Record Not Saved");
            //chkYearCarry.Visible = false;
            ddlLeaveType.SelectedIndex = 0;
            ddlSchType.SelectedIndex = 0;
            txtPaidLeave.Text = "";
            txtTotalLeave.Text = "";
            txtPerSal.Text = "100";
            chkMonthCarry.Checked = false;
        }
        AllPageCode();
        //chkIsAuto.Visible = false;
        //ChkIsRule.Visible = false;
        chkYearCarry.Checked = false;
        chkIsAuto.Checked = false;
        // chkIsAuto.Checked = false;
        ChkIsRule.Checked = false;
        trdeductionSlab.Visible = false;
    }
    protected void btnResetLeave_Click(object sender, EventArgs e)
    {
        SetHalfDay();
        lblSelectRecd.Text = "";
        gvEmpLeave.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationLeave, ddlDepartmentLeave);
        //dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //if (Session["SessionDepId"] != null)
        //{
        //    dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        //}
        //if (dtEmp.Rows.Count > 0)
        //{
        Session["dtEmpLeave"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
        //}
        ddlLeaveType.SelectedIndex = 0;
        ddlSchType.SelectedIndex = 0;
        txtPaidLeave.Text = "";
        txtTotalLeave.Text = "";
        chkMonthCarry.Checked = false;
        // chkYearCarry.Checked = false;
        gvLeaveEmp.DataSource = null;
        gvLeaveEmp.DataBind();
        Session["CHECKED_ITEMS"] = null;
        Session["dtLeave"] = null;
        gridEmpLeave.DataSource = null;
        gridEmpLeave.DataBind();
        //chkYearCarry.Visible = false;
        DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
        dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
        }
        gvEmployee.DataSource = null;
        gvEmployee.DataBind();
        // chkIsAuto.Visible = false;
        //ChkIsRule.Visible = false;
        //chkIsAuto.Checked = false;
        ChkIsRule.Checked = false;
    }
    protected void btnCancelLeave_Click(object sender, EventArgs e)
    {
        lblSelectRecd.Text = "";
        gvEmpLeave.Visible = true;
        DataTable dtEmp = GetEmployeeFilteredRecord(ddlLocationLeave, ddlDepartmentLeave);
        Session["dtEmpLeave"] = dtEmp;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvEmpLeave, dtEmp, "", "");
        Session["CHECKED_ITEMS"] = null;
        ddlLeaveType.SelectedIndex = 0;
        ddlSchType.SelectedIndex = 0;
        txtPaidLeave.Text = "";
        txtTotalLeave.Text = "";
        gvLeaveEmp.DataSource = null;
        gvLeaveEmp.DataBind();
        chkMonthCarry.Checked = false;
        // chkYearCarry.Checked = false;
        btnList_Click(null, null);
        Session["dtLeave"] = null;
        gridEmpLeave.DataSource = null;
        gridEmpLeave.DataBind();
        //chkYearCarry.Visible = false;
        DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
        dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)lbxGroup, dtGroup, "Group_Name", "Group_Id");
        }
        gvEmployee.DataSource = null;
        gvEmployee.DataBind();
        // chkIsAuto.Visible = false;
        //ChkIsRule.Visible = false;
        // chkIsAuto.Checked = false;
        ChkIsRule.Checked = false;
        Hdn_Edit.Value = "";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void SaveLeave(string Edit, string LeaveTypeId, string EmpId, string SchType, string AssignLeave, string PaidLeave, string IsYearCarry, string PrevSchduleType, string PrevAssignLeave, string TransNo, string IsRule, DateTime FinancialYearStartDate, DateTime FinancialYearEndDate, DateTime dtJoining)
    {
        //code commneted by jitendra on 24-05-2017
        //new code wrote for proper leave assigning according date of joining
        /// means employee joined in january but hr forgot to assign leave in that case system assigning leave according current month instead of joining month and year 
        //code start
        DataTable dtEmp = objAtt.GetEmployeeDOJ(Session["CompId"].ToString(), EmpId);
        if (dtEmp.Rows.Count > 0)
        {
            dtJoining = Convert.ToDateTime(dtEmp.Rows[0]["Doj"].ToString());
        }
        if (dtJoining > DateTime.Now)
        {
            return;
        }
        double TotalAssignLeave = Convert.ToDouble(AssignLeave);
        double TotalpaidLeave = Convert.ToDouble(PaidLeave);
        if (SchType == "Yearly")
        {
            if (dtJoining >= FinancialYearStartDate)
            {
                int month = 1 + (FinancialYearEndDate.Month - dtJoining.Month) + 12 * (FinancialYearEndDate.Year - dtJoining.Year);
                if (Convert.ToBoolean(IsRule) == false)
                {
                    TotalAssignLeave = (TotalAssignLeave / 12) * month;
                    TotalAssignLeave = Math.Round(TotalAssignLeave);
                    TotalpaidLeave = (TotalpaidLeave / 12) * month;
                    TotalpaidLeave = Math.Round(TotalpaidLeave);
                }
            }
        }
        //here we are deleting previous row for same leave type
        if (SchType == "Monthly")
        {
            if (TransNo.Trim() != "")
            {
                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, DateTime.Now.Month.ToString(), FinancialYearStartDate.Year.ToString());
            }
            objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), DateTime.Now.Month.ToString(), "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            int totalLogPostedCount = Convert.ToInt32(objDa.return_DataTable("SELECT COUNT(*) from pay_employee_attendance where Emp_Id='" + EmpId + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))>='" + FinancialYearStartDate + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))<='" + FinancialYearEndDate + "'").Rows[0][0].ToString());
            double Remainingdays = 0;
            Remainingdays = ((Convert.ToDouble(AssignLeave) / 12) * totalLogPostedCount);
            if (Remainingdays.ToString().Contains("."))
            {
                Remainingdays = Convert.ToDouble(Remainingdays.ToString().Split('.')[0].ToString());
            }
            if (TransNo.Trim() != "")
            {
                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, "0", FinancialYearStartDate.Year.ToString());
            }
            if (Convert.ToBoolean(IsRule) == true)
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", Remainingdays.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Full Day Leave Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }
    public void SaveLeave(string Edit, string LeaveTypeId, string EmpId, string SchType, string AssignLeave, string PaidLeave, string IsYearCarry, string PrevSchduleType, string PrevAssignLeave, string TransNo, string IsRule, DateTime FinancialYearStartDate, DateTime FinancialYearEndDate, DateTime dtJoining, ref SqlTransaction trns)
    {
        //code commneted by jitendra on 24-05-2017
        //new code wrote for proper leave assigning according date of joining
        /// means employee joined in january but hr forgot to assign leave in that case system assigning leave according current month instead of joining month and year 
        //code start
        DataTable dtEmp = objAtt.GetEmployeeDOJ(Session["CompId"].ToString(), EmpId, ref trns);
        if (dtEmp.Rows.Count > 0)
        {
            dtJoining = Convert.ToDateTime(dtEmp.Rows[0]["Doj"].ToString());
        }
        if (dtJoining > DateTime.Now)
        {
            return;
        }
        double TotalAssignLeave = Convert.ToDouble(AssignLeave);
        double TotalpaidLeave = Convert.ToDouble(PaidLeave);
        if (SchType == "Yearly")
        {
            if (dtJoining >= FinancialYearStartDate)
            {
                int month = 1 + (FinancialYearEndDate.Month - dtJoining.Month) + 12 * (FinancialYearEndDate.Year - dtJoining.Year);
                if (Convert.ToBoolean(IsRule) == false)
                {
                    TotalAssignLeave = (TotalAssignLeave / 12) * month;
                    TotalAssignLeave = Math.Round(TotalAssignLeave);
                    TotalpaidLeave = (TotalpaidLeave / 12) * month;
                    TotalpaidLeave = Math.Round(TotalpaidLeave);
                }
            }
        }
        //here we are deleting previous row for same leave type
        if (SchType == "Monthly")
        {
            if (TransNo.Trim() != "")
            {
                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, DateTime.Now.Month.ToString(), FinancialYearStartDate.Year.ToString(), ref trns);
            }
            objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), DateTime.Now.Month.ToString(), "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
        else
        {
            int totalLogPostedCount = Convert.ToInt32(objDa.return_DataTable("SELECT COUNT(*) from pay_employee_attendance where Emp_Id='" + EmpId + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))>='" + FinancialYearStartDate + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))<='" + FinancialYearEndDate + "'", ref trns).Rows[0][0].ToString());
            double Remainingdays = 0;
            Remainingdays = ((Convert.ToDouble(AssignLeave) / 12) * totalLogPostedCount);
            if (Remainingdays.ToString().Contains("."))
            {
                Remainingdays = Convert.ToDouble(Remainingdays.ToString().Split('.')[0].ToString());
            }
            if (TransNo.Trim() != "")
            {
                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, "0", FinancialYearStartDate.Year.ToString(), ref trns);
            }
            if (Convert.ToBoolean(IsRule) == true)
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", Remainingdays.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
        }
    }
    #endregion
    #region Assign Department to Employee Methods
    protected void btnAssign_Click(object sender, EventArgs e)
    {
        // Add on 30-08-2017 By KSR
        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlPanelty.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlOTPartial.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        txtEmployeeCode.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlLeave.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlNotice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlSalary.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuAssign.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        gvEmpAssign.Visible = true;
        gvEmpSalary.Visible = false;
        PnlEmpSalary.Visible = false;
        lblSelectedRecord.Text = "";
        rbtnEmployee.Checked = true;
        rbtnManager.Checked = false;
        rbtnCEO.Checked = false;
        FillddlLocationListAssign();
        FillddlDeaprtmentListAssign();
        FillGridAssign();
        FillAllLocationAssign();
        FillAllDepartmentAssign();
        AllPageCode();
    }
    protected void btnbindAssign_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (txtValueAssign.Text.Trim().ToString() == "")
        {
            txtValueAssign.Focus();
            DisplayMessage("Please Fill Value");
            return;
        }
        // ViewState["CurrIndex"] = 0;
        // ViewState["SubSize"] = 9;
        DataTable dtEmployee = (DataTable)Session["dtEmpAssign"];
        if (ddlOptionAssign.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionAssign.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameAssign.SelectedValue + ",System.String)='" + txtValueAssign.Text.Trim() + "'";
            }
            else if (ddlOptionAssign.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameAssign.SelectedValue + ",System.String) Like '" + txtValueAssign.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameAssign.SelectedValue + ",System.String) like '%" + txtValueAssign.Text.Trim() + "%'";
            }

            DataView view = new DataView(dtEmployee, condition, "", DataViewRowState.CurrentRows);
            lblTotalRecordAssign.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";
            dtEmployee = view.ToTable();
            // if (dtEmployee.Rows.Count <= 9)
            {
                btnRefreshAssign.Focus();
                //Common Function add By Lokesh on 14-05-2015              
                objPageCmn.FillData((object)gvEmpAssign, dtEmployee, "", "");
            }
        }
        Session["CHECKED_ITEMS_Assign"] = null;
        AllPageCode();
        txtValueAssign.Focus();
    }
    protected void btnRefreshAssign_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ViewState["CurrIndex"] = 0;
        ViewState["SubSize"] = 9;
        FillGridAssign();
        ddlFieldNameAssign.SelectedIndex = 1;
        ddlOptionAssign.SelectedIndex = 3;
        txtValueAssign.Text = "";
        // btngo_Click(null, null);
        ddlFieldNameAssign.Focus();
        Session["CHECKED_ITEMS_Assign"] = null;
        AllPageCode();
        // System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlFieldName.Focus());
    }
    public void FillGridAssign()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Department_Id='0'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmpAssign"] = dtEmp;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvEmpAssign, dtEmp, "", "");

            lblTotalRecordAssign.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";
            ddlDepartmentAssign.Visible = true;
            btnSaveAssign.Visible = true;
            lblDepartmentAssign.Visible = true;
            btnCancelAssign.Visible = true;
        }
        else
        {
            gvEmpAssign.DataSource = null;
            gvEmpAssign.DataBind();
            lblDepartmentAssign.Visible = false;
            ddlDepartmentAssign.Visible = false;
            btnSaveAssign.Visible = false;
            btnCancelAssign.Visible = false;
        }
        Session["CHECKED_ITEMS_Assign"] = null;
        AllPageCode();
        dtEmp.Dispose();
    }

    protected void lnkempassignexport_Command(object sender, CommandEventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        DataTable dtContact = Session["dtEmpAssign"] as DataTable;
        dtContact = dtContact.DefaultView.ToTable(true, "Action_Type", "Effectivedate", "Emp_Type", "Location", "Emp_Code", "Emp_Name", "Emp_Name_L", "Gender", "DOB", "DOJ", "Department", "DeviceGroup", "Civil_Id", "Email_Id", "Phone_No", "Designation", "Religion", "Qualification", "Nationality", "UserRole", "Field3");
        dtContact.Columns["Emp_Code"].ColumnName = "Code";
        dtContact.Columns["Emp_Name"].ColumnName = "Name";
        dtContact.Columns["DOJ"].ColumnName = "Doj";
        dtContact.Columns["DOB"].ColumnName = "Dob";
        dtContact.Columns["Email_Id"].ColumnName = "Email-Id";
        dtContact.Columns["Phone_No"].ColumnName = "Phone_No";
        dtContact.Columns["Emp_Type"].ColumnName = "Type";
        dtContact.Columns["Civil_Id"].ColumnName = "Civil-id";
        dtContact.Columns["Emp_Name_L"].ColumnName = "Name_L";
        dtContact.Columns["Field3"].ColumnName = "ManagerCode";
        if (dtContact.Rows.Count > 0)
        {
            ExportToExcel(dtContact, "Employee List");
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }
    }
    protected void imbBtnGridAssign_Click(object sender, ImageClickEventArgs e)
    {
        lnkNext.Visible = false;
        lnkLast.Visible = false;
        lnkFirst.Visible = false;
        lnkPrev.Visible = false;
        rptCustomers.Visible = false;
        div_Paging.Visible = false;
        //dtlistEmp.Visible = false;
        gvEmp.Visible = false;
        gvEmpAssign.Visible = true;
        imbBtnGrid.Visible = false;
        txtValueAssign.Focus();
        AllPageCode();
    }
    protected void gvEmpAssign_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvEmpAssign.PageIndex = e.NewPageIndex;
        DataTable dtEmp = objEmp.GetEmployeeWithoutDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        gvEmpAssign.DataSource = dtEmp;
        gvEmpAssign.DataBind();
        PopulateCheckedValues();
        AllPageCode();
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS_Assign"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvEmpAssign.Rows)
            {
                int index = (int)gvEmpAssign.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvEmpAssign.Rows)
        {
            index = (int)gvEmpAssign.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
            // Check in the Session
            if (Session["CHECKED_ITEMS_Assign"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS_Assign"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS_Assign"] = userdetails;
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS_Assign"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS_Assign"];
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label Transid = (Label)gvEmpAssign.Rows[index].FindControl("lblTransIdAssign");
        if (((CheckBox)gvEmpAssign.Rows[index].FindControl("chkgvSelect")).Checked == true)
        {
            if (!userdetails.Contains(Convert.ToInt32(Transid.Text)))
            {
                userdetails.Add(Convert.ToInt32(Transid.Text));
            }
        }
        else
        {
            if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
            {
                userdetails.Remove(Convert.ToInt32(Transid.Text));
            }
        }
        Session["CHECKED_ITEMS_Assign"] = userdetails;
    }
    protected void chkgvSelectAll_CheckedChanged1(object sender, EventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS_Assign"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS_Assign"];
        CheckBox chkSelAll = ((CheckBox)gvEmpAssign.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvEmpAssign.Rows)
        {
            Label Transid = (Label)gr.FindControl("lblTransIdAssign");
            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
                if (!userdetails.Contains(Convert.ToInt32(Transid.Text)))
                {
                    userdetails.Add(Convert.ToInt32(Transid.Text));
                }
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
                if (userdetails.Contains(Convert.ToInt32(Transid.Text)))
                {
                    userdetails.Remove(Convert.ToInt32(Transid.Text));
                }
            }
        }
        Session["CHECKED_ITEMS_Assign"] = userdetails;
    }
    protected void ImgbtnSelectAll_Click1(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS_Assign"] = null;
            ViewState["Select"] = 1;
            DataTable dtEmp = objEmp.GetEmployeeWithoutDepartment(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            foreach (DataRow dr in dtEmp.Rows)
            {
                //Allowance_Id
                // Check in the Session
                if (Session["CHECKED_ITEMS_Assign"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS_Assign"];
                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS_Assign"] = userdetails;
            PopulateCheckedValues();
        }
        else
        {
            foreach (GridViewRow gvrow in gvEmpAssign.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
            Session["CHECKED_ITEMS_Assign"] = null;
            ViewState["Select"] = null;
        }
    }

    private void FillddlLocationListAssign()
    {
        DataTable dtLoc = ObjLocMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
    }
    private void FillddlDeaprtmentListAssign()
    {
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        //dt = new DataView(dt, "Location_Id in(" + FLocIds.Substring(0, FLocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

    }


    protected void ddlLocationAssign_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillAllDepartmentAssign();
    }

    private void FillAllDepartmentAssign()
    {
        ddlDepartmentAssign.Items.Clear();
        string MyQuery = "select Set_DepartmentMaster.Dep_Id,Set_DepartmentMaster.Dep_Name from set_location_department inner join set_locationmaster on set_location_department.location_id = set_locationmaster.location_id inner join Set_DepartmentMaster on Set_Location_Department.Dep_Id = Set_DepartmentMaster.Dep_Id  where set_location_department.Location_Id = " + ddlLocationAssign.SelectedValue + " and Set_DepartmentMaster.IsActive='True' order by Set_DepartmentMaster.Dep_Name ";
        DataTable dt = objDa.return_DataTable(MyQuery);
        if (dt != null && dt.Rows.Count > 0)
        {
            ddlDepartmentAssign.DataTextField = "Dep_Name";
            ddlDepartmentAssign.DataValueField = "Dep_Id";
            ddlDepartmentAssign.DataSource = dt;
            ddlDepartmentAssign.DataBind();
        }
        ddlDepartmentAssign.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void FillAllLocationAssign()
    {
        string MyQuery = "select location_name,location_id from set_locationmaster where isactive='True'";
        DataTable dt = objDa.return_DataTable(MyQuery);
        if (dt != null && dt.Rows.Count > 0)
        {
            ddlLocationAssign.DataTextField = "location_name";
            ddlLocationAssign.DataValueField = "location_id";
            ddlLocationAssign.DataSource = dt;
            ddlLocationAssign.DataBind();
            ddlLocationAssign.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }

    #endregion
    protected void btnSaveAssign_Click(object sender, EventArgs e)
    {
        if (ddlLocationAssign.SelectedValue.Trim() == "0")
        {
            DisplayMessage("Select Location");
            ddlLocationAssign.Focus();
            return;
        }
        string strLabourLaw = string.Empty;
        strLabourLaw = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), ddlLocation.SelectedValue).Rows[0]["Field3"].ToString();
        DataTable dtEmpLeave = new DataTable();
        DataTable dtEmpLeave1 = new DataTable();
        DataTable dtEmp = new DataTable();
        string strGender = string.Empty;
        if (ddlDepartmentAssign.SelectedIndex > 0)
        {
            ArrayList userdetails = new ArrayList();
            if (Session["CHECKED_ITEMS_Assign"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS_Assign"];
            if (userdetails.Count > 0)
            {
                for (int i = 0; i < userdetails.Count; i++)
                {
                    if (userdetails[i].ToString() != "")
                    {
                        objEmp.UpdateEmployeeDepartmentById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocationAssign.SelectedValue.Trim(), userdetails[i].ToString(), ddlDepartmentAssign.SelectedValue.ToString());

                    }
                }
                DisplayMessage("Record Updated", "green");
                FillGridAssign();
                ddlDepartmentAssign.SelectedIndex = 0;
                Session["CHECKED_ITEMS_Assign"] = null;
            }
            else
            {
                DisplayMessage("Please Select at least one Employee");
                return;
            }
        }
        else
        {
            DisplayMessage("Please Select at least one Department");
            return;
        }
    }
    protected void btnCancelAssign_Click(object sender, EventArgs e)
    {
        Reset();
        btnAssign_Click(null, null);
        Div_Main_Upload.Visible = true;
        pnlMap.Visible = false;
        //Hdn_Edit.Value = "";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Assign()", true);
    }
    protected void Img_Emp_List_Select_All_Click(object sender, ImageClickEventArgs e)
    {
    }
    protected void Img_Emp_List_Delete_All_Click(object sender, ImageClickEventArgs e)
    {
    }
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"]))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"]);
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                Session["empimgpath"] = FULogoPath.FileName;
            }
        }
    }
    protected void EmployeeSign_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPathEmployeeSign.HasFile)
        {
            string ext = FULogoPathEmployeeSign.FileName.Substring(FULogoPathEmployeeSign.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                //ImgEmpSignature.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPathEmployeeSign.FileName;
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"])))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"]));
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPathEmployeeSign.FileName);
                FULogoPathEmployeeSign.SaveAs(path);
                Session["empSignimgpath"] = FULogoPathEmployeeSign.FileName;
            }
        }
    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                int fileType = -1;
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                if (ext == ".mdb")
                {
                    fileType = 2;
                }
                if (ext == ".accdb")
                {
                    fileType = 3;
                }
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }
    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        BindTreeView();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Hierarchy()", true);
    }
    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeView_hierarchy.Nodes.Clear();
        DataTable dt = objDa.return_DataTable("select set_employeemaster.Emp_Id,set_employeemaster.emp_name from set_employeemaster  where  Emp_id='" + Session["EmpId"].ToString() + "'");
        // dt = new DataView(dt, "Emp_Id=" + Session["EmpId"].ToString() + "", "Emp_Name asc", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Emp_Name"].ToString();
            tn.Value = dt.Rows[i]["Emp_Id"].ToString();
            TreeView_hierarchy.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Emp_Id"].ToString()), tn);
            i++;
        }
        TreeView_hierarchy.DataBind();
    }
    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = objDa.return_DataTable("select set_employeemaster.Emp_Id,set_employeemaster.emp_name from set_employeemaster  where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Field5='" + index.ToString() + "' and Field2='False' and isactive='True' order by set_employeemaster.emp_name ");
        //DataTable dt = objEmp.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        //dt = new DataView(dt, "Field5='" + index.ToString() + "' and  Field2='False'", "", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Emp_Name"].ToString();
            tn1.Value = dt.Rows[i]["Emp_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Emp_Id"].ToString()), tn1);
            i++;
        }
        TreeView_hierarchy.DataBind();
    }
    protected void Lnk_UploadEmo_Name_Click(object sender, EventArgs e)
    {
        string strsql = string.Empty;
        strsql = "select Set_EmployeeMaster.Brand_Id as BrandId1, Set_EmployeeMaster.department_id as DeptId1, Set_EmployeeMaster.Designation_Id as DesigId1, Set_EmployeeMaster.dob as Dob1, Set_EmployeeMaster.Doj as Doj1, set_employeemaster.emp_code as EmpId1, set_employeemaster.Emp_Name as EmpName, set_employeemaster.location_id as LocationId1, set_employeemaster.email_id as EmailId1 from set_employeemaster where location_Id='" + Session["LocId"].ToString() + "' and isactive='True' and Field2='False' and Emp_Name=''";
        DataTable dt = objDa.return_DataTable(strsql);
        ExportTableData(dt);
    }
    protected void rbtnOTEnable1_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnOTEnable1.Checked)
        {
            ddlOTCalc1.Enabled = true;
            txtNormal1.Enabled = true;
            ddlNormalType1.Enabled = true;
            txtWeekOffValue1.Enabled = true;
            ddlWeekOffType1.Enabled = true;
            txtHolidayValue1.Enabled = true;
            ddlHolidayValue1.Enabled = true;
        }
        else
        {
            ddlOTCalc1.Enabled = false;
            txtNormal1.Enabled = false;
            ddlNormalType1.Enabled = false;
            txtWeekOffValue1.Enabled = false;
            ddlWeekOffType1.Enabled = false;
            txtHolidayValue1.Enabled = false;
            ddlHolidayValue1.Enabled = false;
        }
    }
    protected void gvEmp_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmp"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtEmp"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
        AllPageCode();
    }
    public void ExportToExcel(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtContact = Session["dtEmp"] as DataTable;
        dtContact = dtContact.DefaultView.ToTable(true, "Action_Type", "Effectivedate", "Emp_Type", "Location", "Emp_Code", "Emp_Name", "Emp_Name_L", "Gender", "DOB", "DOJ", "Department", "DeviceGroup", "Civil_Id", "Email_Id", "Phone_No", "Designation", "Religion", "Qualification", "Nationality", "UserRole", "Field3");
        dtContact.Columns["Emp_Code"].ColumnName = "Code";
        dtContact.Columns["Emp_Name"].ColumnName = "Name";
        dtContact.Columns["DOJ"].ColumnName = "Doj";
        dtContact.Columns["DOB"].ColumnName = "Dob";
        dtContact.Columns["Email_Id"].ColumnName = "Email-Id";
        dtContact.Columns["Phone_No"].ColumnName = "Phone_No";
        dtContact.Columns["Emp_Type"].ColumnName = "Type";
        dtContact.Columns["Civil_Id"].ColumnName = "Civil-id";
        dtContact.Columns["Emp_Name_L"].ColumnName = "Name_L";
        dtContact.Columns["Field3"].ColumnName = "ManagerCode";
        if (dtContact.Rows.Count > 0)
        {
            ExportToExcel(dtContact, "Employee List");
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }
    }
    protected void txtAddressName_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            DataTable dtAM = AM.GetAddressDataByAddressName(txtAddressName.Text);
            if (dtAM.Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(imgAddAddressName);
            }
            else
            {
                txtAddressName.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.getAddressNamePreText(prefixText);
        string[] str = new string[0];
        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        else
        {
            str = null;
        }
        return str;
    }
    protected void imgAddAddressName_Click(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            string strA = "0";
            foreach (GridViewRow gve in GvAddressName.Rows)
            {
                Label lblCAddressName = (Label)gve.FindControl("lblgvAddressName");
                if (txtAddressName.Text == lblCAddressName.Text)
                {
                    strA = "1";
                }
            }
            if (hdnAddressId.Value == "")
            {
                if (strA == "0")
                {
                    FillAddressChidGird("Save");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    txtAddressName.Text = "";
                    DisplayMessage("Address Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
            }
            else
            {
                if (txtAddressName.Text == hdnAddressName.Value)
                {
                    FillAddressChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                }
                else
                {
                    if (strA == "0")
                    {
                        FillAddressChidGird("Edit");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    }
                    else
                    {
                        txtAddressName.Text = "";
                        FillAddressChidGird("Edit");
                        DisplayMessage("Address Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressName);
        }
        txtAddressName.Focus();
    }
    protected void btnAddNewAddress_Click(object sender, EventArgs e)
    {
        addaddress.Reset();
        Session["Add_Address_Popup"] = "";
        txtAddressName.Text = "";
        Hdn_Address_ID.Value = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
        hdntxtaddressid.Value = txtAddressName.Text;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        if (ViewState["Country_Id"] != null)
        {
            addaddress.BtnNew_click(ViewState["Country_Id"].ToString());
        }
        addaddress.fillHeader(txtEmployeeName.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
    }
    protected void btnAddressEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        Label lblgvAddressName = gvRow.FindControl("lblgvAddressName") as Label;
        Hdn_Address_ID.Value = lblgvAddressName.Text;
        hdntxtaddressid.Value = lblgvAddressName.Text;
        txtAddressName.Text = lblgvAddressName.Text;
        Session["Add_Address_Popup"] = lblgvAddressName.Text;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
        hdnAddressId.Value = e.CommandArgument.ToString();
        //user to fill address data on edit button click 
        addaddress.GetAddressInformationByAddressname(lblgvAddressName.Text);
        FillAddressDataTabelEdit();
        addaddress.fillHeader(txtEmployeeName.Text);
        addaddress.FillLocationNCode();
    }
    public DataTable FillAddressDataTabelEdit()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id='" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtAddressName.Text = dt.Rows[0]["Address_Name"].ToString();
            hdnAddressName.Value = dt.Rows[0]["Address_Name"].ToString();
        }
        return dt;
    }
    protected void btnAddressDelete_Command(object sender, CommandEventArgs e)
    {
        hdnAddressId.Value = e.CommandArgument.ToString();
        FillAddressChidGird("Del");
    }
    public void FillAddressChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillAddressDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillAddressDataTableUpdate();
        }
        else
        {
            dt = FillAddressDataTabel();
        }
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressName, dt, "", "");
        ResetAddressName();
    }
    public DataTable FillAddressDataTabelDelete()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAddressId.Value + "'";
        dt = (DataTable)dv.ToTable();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Trans_Id"] = i + 1;
        }
        return dt;
    }
    public DataTable FillAddressDataTableUpdate()
    {
        DataTable dt = CreateAddressDataTable();
        for (int i = 0; i < GvAddressName.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressName.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressName.Rows[i].FindControl("lblgvAddressName");
            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressId.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressName.Text);
            }
        }
        return dt;
    }
    public void ResetAddressName()
    {
        txtAddressName.Text = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);
        string filtertext = "AccountName like '%" + prefixText + "%'";
        try
        {
            dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] txt = new string[dtCOA.Rows.Count];
        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "/";
            }
        }
        return txt;
    }
    public void Check_Financial_Year_Status()
    {
        string sql = "Select* from Ac_Finance_Year_Info where Status = 'Close'";
        DataTable dt_Finance = objDa.return_DataTable(sql);
        if (dt_Finance != null && dt_Finance.Rows.Count > 0)
        {
            txtOpeningCreditAmt.Enabled = false;
            txtOpeningDebitAmt.Enabled = false;
            txtEmployerTotalEarning.Enabled = false;
            txttOtalTDS.Enabled = false;
            txtEditOpeningCreditAmt.Enabled = false;
            txtEditOpeningDebitAmt.Enabled = false;
            txtEditEmployerTotalEarning.Enabled = false;
            txtEdittOtalTDS.Enabled = false;
        }
        else
        {
            txtOpeningCreditAmt.Enabled = true;
            txtOpeningDebitAmt.Enabled = true;
            txtEmployerTotalEarning.Enabled = true;
            txttOtalTDS.Enabled = true;
            txtEditOpeningCreditAmt.Enabled = true;
            txtEditOpeningDebitAmt.Enabled = true;
            txtEditEmployerTotalEarning.Enabled = true;
            txtEdittOtalTDS.Enabled = true;
        }
    }
    protected void Txt_Salary_Payment_Option_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();
                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = objDa.return_DataTable(sql);
                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose Account in Suggestion Only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose Account in Suggestion Only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Salary_Increment_Open_Popup()", true);
    }
    protected void ddlSalaryPlan1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSalaryPlan1.SelectedValue != "--Select--")
        {
            DataTable dt = ObjSalaryPlan.GetRecordTruebyId(Session["CompId"].ToString(), ddlSalaryPlan1.SelectedValue.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                if (txtGrossSalary.Text != "")
                {
                    double GrossSalary = Convert.ToDouble(txtGrossSalary.Text);
                    double Percent = Convert.ToDouble(dt.Rows[0]["Basic_Salary_Percentage"].ToString());
                    double BasicSalary = (GrossSalary * Percent) / 100;
                    txtBasic1.Text = BasicSalary.ToString();
                }
            }
        }
        else
        {
            txtBasic1.Text = "0";
        }
    }
    protected void txtGrossSalary_TextChanged(object sender, EventArgs e)
    {
        if (IsSalaryPlanEnable())
        {
            ddlSalaryPlan1_SelectedIndexChanged(null, null);
        }
    }
    #region DeviceOperation

    protected void FileUploadComplete(object sender, EventArgs e)
    {
        int fileType = 0;
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
            }
        }
    }
    protected void btnGetSheet_Click(object sender, EventArgs e)
    {
        if (txtuploadReferenceNo.Text == "")
        {
            DisplayMessage("Configure Document number");
            return;
        }
        int fileType = 0;
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }
                Import(Path, fileType);
            }
        }
    }
    protected void btnConnect_Click(object sender, EventArgs e)
    {
        int counter_Loc = 0;
        int counter_Dep = 0;
        int counter_Desg = 0;
        DataTable dtEmpList = objDa.return_DataTable("select  [Company_Id] ,[Brand_Id] ,[Location_Id] ,[Emp_Id] ,[Emp_Code] ,[Civil_Id] ,[Emp_Name] ,[Emp_Name_L] ,[Emp_Image] ,[Department_Id] ,[Designation_Id] ,[Religion_Id] ,[Nationality_Id] ,[Qualification_Id] ,[DOB] ,[DOJ] ,[Emp_Type] ,[Termination_Date] ,[Gender] ,[Email_Id] ,[Phone_No] ,[Field1] ,[Field2] ,[Field3] ,[Field4] ,[Field5] ,[Field6] ,[Field7] ,[IsActive] ,[CreatedBy] ,[CreatedDate] ,[ModifiedBy] ,[ModifiedDate] ,[company_phone_no] ,[Pan] ,[FatherName] ,[IsMarried] ,[DLNo] ,isNull( [Device_Group_Id],0) as Device_Group_Id  from set_employeemaster where Company_Id=" + Session["CompId"].ToString() + "");
        string strResult = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        string strDateVal = string.Empty;
        if (ddlTables == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (ddlTables.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        string strEmpId = string.Empty;
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }
                if (!CheckSheetValidation(dt))
                {
                    DisplayMessage("Upload valid excel sheet");
                    return;
                }
                if (!dt.Columns.Contains("IsValid"))
                {
                    dt.Columns.Add("IsValid");
                }
                dt = AddColumn(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["IsValid"] = "True";
                    if (dt.Rows[i][0].ToString() == "")
                    {
                        dt.Rows[i]["IsValid"] = "";
                        continue;
                    }
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Columns[j].ColumnName.Trim() == "Action_Type")
                        {
                            if (dt.Rows[i]["Action_Type"].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Action_Type - Enter Value)";
                                break;
                            }
                            else if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "NEW HIRE/UPDATE" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "TRANSFER" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "TERMINATION" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "TEMPTRANSFER" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "REVERSE TERMINATION" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "DELETE" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "ON ROLE" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "OFF ROLE")
                            {
                                dt.Rows[i]["IsValid"] = "False(Action_Type - Enter Valid Value)";
                                break;
                            }
                        }

                        if (dt.Columns[j].ColumnName.Trim() == "ManagerCode")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                if (strEmpId.Trim() != "0")
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = new DataView(dtEmpList, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field5"].ToString();
                                }
                                continue;
                            }
                            strResult = GetcolumnValue("Set_Employeemaster", "Emp_Code", dt.Rows[i][j].ToString(), "Emp_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(Manager Code - not exists)";
                                break;
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Code")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Code - Enter Value)";
                                break;
                            }
                            strResult = GetcolumnValue("Set_Employeemaster", "Emp_Code", dt.Rows[i][j].ToString(), "Emp_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() != "NEW HIRE/UPDATE" && strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(Code - not exists)";
                                break;
                            }
                            //here we are checking that employee already terminated or not 
                            if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TERMINATION")
                            {
                                if (IsemployeeTerminated(strResult))
                                {
                                    dt.Rows[i]["IsValid"] = "False(Employee already Terminated)";
                                    break;
                                }
                            }
                            strEmpId = strResult;
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Location" && (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TRANSFER" || dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TEMPTRANSFER" || dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "NEW HIRE/UPDATE"))
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Location - Enter Value)";
                                break;
                            }
                            strResult = GetcolumnValue("Set_LocationMaster", "Location_Name", dt.Rows[i][j].ToString(), "Location_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    counter_Loc++;
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = ObjLocMaster.InsertLocationMaster(Session["CompId"].ToString(), dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j].ToString().Trim(), "LOC-" + counter_Loc.ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), Session["CurrencyId"].ToString(), "0", "0", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Location - not exists)";
                                    break;
                                }
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Gender" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "NEW HIRE/UPDATE")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Gender - Enter Value)";
                                break;
                            }
                            if (dt.Rows[i][j].ToString().Trim().ToUpper() != "M" && dt.Rows[i][j].ToString().Trim().ToUpper() != "F")
                            {
                                dt.Rows[i]["IsValid"] = "False(Gender - value should be 'M or F')";
                                break;
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Dob" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "NEW HIRE/UPDATE")
                        {
                            strDateVal = CheckDatevalidation(dt.Rows[i][j].ToString());
                            if (strDateVal == "")
                            {
                                dt.Rows[i]["IsValid"] = "False((Dob - invalid date format)";
                                break;
                            }
                            else
                            {
                                dt.Rows[i][j] = Convert.ToDateTime(strDateVal).ToShortDateString();
                            }

                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Doj" && dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "NEW HIRE/UPDATE")
                        {
                            strDateVal = CheckDatevalidation(dt.Rows[i][j].ToString());
                            if (strDateVal == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Doj - invalid date format)";
                                break;
                            }
                            else
                            {
                                dt.Rows[i][j] = Convert.ToDateTime(strDateVal).ToShortDateString();
                            }

                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Department" && (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "NEW HIRE/UPDATE" || dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TRANSFER" || dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TEMPTRANSFER"))
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Department - Enter Value)";
                                break;
                            }
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                continue;
                            }
                            strResult = GetcolumnValue("Set_DepartmentMaster", "Dep_Name", dt.Rows[i][j].ToString(), "Dep_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    counter_Dep++;
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = objDep.InsertDepartmentMaster(dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j].ToString().Trim(), "DEP-" + counter_Dep.ToString(), "0", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Department - not exists)";
                                    break;
                                }
                            }
                            if (objDa.return_DataTable("select location_id from set_location_department where location_id=" + dt.Rows[i]["Location_Id"].ToString() + " and dep_id=" + strResult + "").Rows.Count == 0)
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    objLocDept.InsertLocationDepartmentMaster(dt.Rows[i]["Location_Id"].ToString().Trim(), dt.Rows[i]["Department_Id"].ToString().Trim(), "0", "", "", "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Department not exists on Assigned Location)";
                                    break;
                                }
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Designation")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                if (strEmpId.Trim() != "0")
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = new DataView(dtEmpList, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Designation_Id"].ToString();
                                }
                                continue;
                            }
                            strResult = GetcolumnValue("Set_DesignationMaster", "Designation", dt.Rows[i][j].ToString(), "Designation_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    counter_Desg++;
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = objDesg.InsertDesignationMaster(dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j].ToString().Trim(), "DESG-" + counter_Desg.ToString(), "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Designation - not exists)";
                                    break;
                                }
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Religion")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                if (strEmpId.Trim() != "0")
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = new DataView(dtEmpList, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Religion_Id"].ToString();
                                }
                                continue;
                            }
                            strResult = GetcolumnValue("Set_ReligionMaster", "Religion", dt.Rows[i][j].ToString(), "Religion_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = objRel.InsertReligionMaster(dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j].ToString().Trim(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Religion - not exists)";
                                    break;
                                }
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Qualification")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                if (strEmpId.Trim() != "0")
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = new DataView(dtEmpList, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Qualification_Id"].ToString();
                                }
                                continue;
                            }
                            strResult = GetcolumnValue("Set_QualificationMaster", "Qualification", dt.Rows[i][j].ToString(), "Qualification_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = objQualif.InsertQualificationMaster(dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j].ToString().Trim(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Qualification - not exists)";
                                    break;
                                }
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Nationality")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                if (strEmpId.Trim() != "0")
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = new DataView(dtEmpList, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Nationality_Id"].ToString();
                                }
                                continue;
                            }
                            strResult = GetcolumnValue("Set_NationalityMaster", "Nationality", dt.Rows[i][j].ToString(), "Nationality_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                if (chkisautoinsert.Checked)
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = objNat.InsertNationalityMaster(dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j].ToString().Trim(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                else
                                {
                                    dt.Rows[i]["IsValid"] = "False(Nationality - not exists)";
                                    break;
                                }
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "UserRole")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                continue;
                            }
                            strResult = GetcolumnValue("Set_RoleMaster", "Role_Name", dt.Rows[i][j].ToString(), "Role_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(UserRole - not exists)";
                                break;
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "DeviceGroup")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = "0";
                                if (strEmpId.Trim() != "0")
                                {
                                    dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = new DataView(dtEmpList, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Device_Group_Id"].ToString();
                                }
                                continue;
                            }
                            strResult = GetcolumnValue("Att_DeviceGroupMaster", "Group_Name", dt.Rows[i][j].ToString(), "Group_Id");
                            dt.Rows[i][dt.Columns[j].ColumnName + "_Id"] = strResult;
                            if (strResult == "0")
                            {
                                dt.Rows[i]["IsValid"] = "False(DeviceGroup - not exists)";
                                break;
                            }
                        }
                        if (dt.Columns[j].ColumnName.Trim() == "Effectivedate")
                        {
                            strDateVal = CheckDatevalidation(dt.Rows[i][j].ToString());
                            if (strDateVal == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(EffectiveDate - invalid date format)";
                                break;
                            }
                            else
                            {
                                dt.Rows[i][j] = strDateVal;
                            }
                        }
                    }
                }
                uploadEmpdetail.Visible = true;
                dtTemp = dt.DefaultView.ToTable(true, "Action_Type", "Effectivedate", "Location", "Code", "Name", "Gender", "Dob", "Doj", "Department", "DeviceGroup", "Civil-id", "Email-Id", "Phone_No", "Designation", "Religion", "Qualification", "Nationality", "UserRole", "ManagerCode", "Salary", "IsValid");
                gvSelected.DataSource = dtTemp;
                gvSelected.DataBind();
                lbltotaluploadRecord.Text = "Total Record : " + (dtTemp.Rows.Count - 1).ToString();
                Session["UploadEmpDtAll"] = dt;
                Session["UploadEmpDt"] = dtTemp;
                rbtnupdall.Checked = true;
                rbtnupdInValid.Checked = false;
                rbtnupdValid.Checked = false;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        dt.Dispose();
    }
    protected void btnRunservice_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (System.Diagnostics.Process PRC in System.Diagnostics.Process.GetProcesses())
            {
                if (PRC.ProcessName == "DeviceOperationService")
                {
                    DisplayMessage("Service already running");
                    return;
                }
            }
            Process p = new Process();
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = ConfigurationManager.AppSettings["ServicePath"].ToString();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            p.StartInfo = startInfo;
            p.Start();
            DisplayMessage("Device operation service has started successsfully");
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString());
        }
    }
    public string CheckDatevalidation(string strDateval)
    {
        string strDate = string.Empty;
        DateTime dt = new DateTime();
        int day = 0;
        int Month = 0;
        int year = 0;
        string strYear = string.Empty;
        try
        {
            strDateval = Convert.ToDateTime(strDateval).ToShortDateString();
            day = Convert.ToInt32(strDateval.ToString().Split('-')[0]);
            Month = getMonthNumber(strDateval.ToString().Split('-')[1]);
            year = Convert.ToInt32(strDateval.ToString().Split('-')[2]);
            dt = new DateTime(year, Month, day);
            strDate = dt.ToShortDateString().ToString();
        }
        catch (Exception ex)
        {
            try
            {

                day = Convert.ToInt32(strDateval.ToString().Split('/')[0]);
                Month = Convert.ToInt32(strDateval.ToString().Split('/')[1]);
                year = Convert.ToInt32(strDateval.ToString().Split('/')[2]);
                dt = new DateTime(year, Month, day);
                strDate = dt.ToShortDateString().ToString();
            }
            catch
            {
                strDate = Convert.ToDateTime(strDateval).ToString(objSys.SetDateFormat());
            }
        }
        return strDate;
    }
    public int getMonthNumber(string strMonthName)
    {
        int monthNumber = 0;
        if (strMonthName.ToLower().Trim() == "jan")
        {
            monthNumber = 1;
        }
        if (strMonthName.ToLower().Trim() == "feb")
        {
            monthNumber = 2;
        }
        if (strMonthName.ToLower().Trim() == "mar")
        {
            monthNumber = 3;
        }
        if (strMonthName.ToLower().Trim() == "apr")
        {
            monthNumber = 4;
        }
        if (strMonthName.ToLower().Trim() == "may")
        {
            monthNumber = 5;
        }
        if (strMonthName.ToLower().Trim() == "jun")
        {
            monthNumber = 6;
        }
        if (strMonthName.ToLower().Trim() == "jul")
        {
            monthNumber = 7;
        }
        if (strMonthName.ToLower().Trim() == "aug")
        {
            monthNumber = 8;
        }
        if (strMonthName.ToLower().Trim() == "sep")
        {
            monthNumber = 9;
        }
        if (strMonthName.ToLower().Trim() == "oct")
        {
            monthNumber = 10;
        }
        if (strMonthName.ToLower().Trim() == "nov")
        {
            monthNumber = 11;
        }
        if (strMonthName.ToLower().Trim() == "dec")
        {
            monthNumber = 12;
        }
        return monthNumber;
    }
    public bool IsemployeeTerminated(string strEmpId)
    {
        bool result = false;
        DataTable dt = objDa.return_DataTable("select emp_id from Set_EmployeeMaster where company_id=" + Session["CompId"].ToString() + " and field2='True' and Emp_id=" + strEmpId + "");
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public DataTable ConvetExcelToDataTable(string path, string strtableName)
    {
        DataTable dt = new DataTable();

        try
        {
            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + strtableName.ToString() + "]";
            adp.SelectCommand.Connection = cnn;
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {
            }

        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }
    protected void rbtnupdall_OnCheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["UploadEmpDt"];
        if (rbtnupdValid.Checked)
        {
            dt = new DataView(dt, "IsValid='True' or IsValid=''", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (rbtnupdInValid.Checked)
        {
            dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        gvSelected.DataSource = dt;
        gvSelected.DataBind();
        lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count - 1).ToString();
    }
    public bool CheckSheetValidation(DataTable dt)
    {
        bool Result = true;
        if (dt.Columns.Contains("Action_Type") && dt.Columns.Contains("Effectivedate") && dt.Columns.Contains("Location") && dt.Columns.Contains("Code") && dt.Columns.Contains("Name") && dt.Columns.Contains("Civil-id") && dt.Columns.Contains("Gender") && dt.Columns.Contains("Email-Id") && dt.Columns.Contains("Dob") && dt.Columns.Contains("Doj") && dt.Columns.Contains("Department") && dt.Columns.Contains("Designation") && dt.Columns.Contains("Religion") && dt.Columns.Contains("Qualification") && dt.Columns.Contains("Nationality") && dt.Columns.Contains("UserRole") && dt.Columns.Contains("DeviceGroup"))
        {
        }
        else
        {
            Result = false;
        }
        return Result;
    }
    public DataTable AddColumn(DataTable dt)
    {
        dt.Columns.Add("Location_Id");
        dt.Columns.Add("Code_Id");
        dt.Columns.Add("Department_Id");
        dt.Columns.Add("Designation_Id");
        dt.Columns.Add("Religion_Id");
        dt.Columns.Add("Qualification_Id");
        dt.Columns.Add("Nationality_Id");
        dt.Columns.Add("UserRole_Id");
        dt.Columns.Add("DeviceGroup_Id");
        dt.Columns.Add("ManagerCode_Id");
        return dt;
    }
    public string GetcolumnValue(string strtablename, string strKeyfieldname, string strKeyfieldvalue, string strKeyFieldResult)
    {
        string strResult = "0";
        strKeyfieldvalue = strKeyfieldvalue.Replace("'", "");
        DataTable dt = objDa.return_DataTable("select " + strKeyFieldResult + " from " + strtablename + " where " + strKeyfieldname + "=N'" + strKeyfieldvalue.Trim() + "' and IsActive='True'");
        if (dt.Rows.Count > 0)
        {
            strResult = dt.Rows[0][0].ToString();
        }
        return strResult;
    }
    protected void btnUploadEmpInfo_Click(object sender, EventArgs e)
    {
        string strEmpType = string.Empty;
        if (gvSelected.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        int newhirecount = 0;
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        int FinancialYearMonth = 0;
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        DataTable dtdevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());
        string strDeviceGroupId = string.Empty;
        string strLocationId = string.Empty;
        DataTable dtGroupDetail = new DataTable();
        int counter = 0;
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        int b = 0;
        DataTable dtEmpLeave = new DataTable();
        string strLocationValue = string.Empty;
        DataTable dtEmp = new DataTable();
        DataTable dtNf = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
        string strLabourLaw = string.Empty;
        trns = con.BeginTransaction();
        try
        {
            dt = (DataTable)Session["UploadEmpDtAll"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IsValid"].ToString().Trim() != "True")
                {
                    continue;
                }
                if (dt.Rows[i]["Code_Id"].ToString().Trim() != "0")
                {
                    dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), dt.Rows[i]["Code_Id"].ToString().Trim(), ref trns);
                    strLocationId = dtEmp.Rows[0]["Location_Id"].ToString();
                    strDeviceGroupId = dtEmp.Rows[0]["Device_Group_Id"].ToString();
                    strEmpType = dtEmp.Rows[0]["Emp_Type"].ToString();
                }
                else
                {
                    strEmpType = "On Role";
                    strDeviceGroupId = "0";
                }
                //here we  will insert record according the selected option
                //for upload
                if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "NEW HIRE/UPDATE")
                {
                    strEmpType = "On Role";
                    string strEmpId = string.Empty;
                    if (dt.Rows[i]["Code_Id"].ToString().Trim() == "0")
                    {
                        //HEER WE ARE CHECKING MAX EMPLOYEE COUNT IN CASE OF CLOUD


                        if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
                        {
                            MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
                            MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Session["RegistrationCode"].ToString(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
                            int attEmpCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from set_employeemaster where company_id = '" + Session["CompId"].ToString() + "'", ref trns).Rows[0][0].ToString());
                            if ((attEmpCount + 1) > Convert.ToInt32(clsMasterCmp.no_of_employee.ToString()))
                            {
                                DisplayMessage("Maximum Employees is exceeded so please update your license");
                                trns.Rollback();
                                if (con.State == System.Data.ConnectionState.Open)
                                {
                                    con.Close();
                                }
                                trns.Dispose();
                                con.Dispose();
                                UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                                //  UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
                                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                                //DisplayMessage("Modal_UpdateLicense_Open()");
                                return;
                            }

                            if (dt.Rows[i]["UserRole_Id"].ToString().Trim() != "0")
                            {
                                int attUserCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from set_usermaster where company_id = '" + Session["CompId"].ToString() + "'", ref trns).Rows[0][0].ToString());
                                if ((attUserCount + 1) > Convert.ToInt32(clsMasterCmp.user.ToString()))
                                {
                                    DisplayMessage("Maximum Users is exceeded so please update your license");
                                    trns.Rollback();
                                    if (con.State == System.Data.ConnectionState.Open)
                                    {
                                        con.Close();
                                    }
                                    trns.Dispose();
                                    con.Dispose();
                                    UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                                    //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                                    //DisplayMessage("Modal_UpdateLicense_Open()");
                                    return;
                                }
                            }
                        }


                        //here need to add code for insert new employee 
                        b = objEmp.InsertEmployeeMaster(Session["CompId"].ToString(), dt.Rows[i]["Name"].ToString(), "N" + dt.Rows[i]["Name_L"].ToString().Trim(), dt.Rows[i]["Code"].ToString().Trim(), "", Session["BrandId"].ToString(), dt.Rows[i]["Location_Id"].ToString(), dt.Rows[i]["Department_Id"].ToString(), dt.Rows[i]["Civil-Id"].ToString(), dt.Rows[i]["Designation_Id"].ToString(), dt.Rows[i]["Religion_Id"].ToString(), dt.Rows[i]["Nationality_Id"].ToString(), dt.Rows[i]["qualification_Id"].ToString(), Convert.ToDateTime(dt.Rows[i]["DOB"].ToString()).ToString(), Convert.ToDateTime(dt.Rows[i]["DOJ"].ToString()).ToString(), "On Role", "1900-01-01", dt.Rows[i]["Gender"].ToString(), "Employee", "False", "", "", dt.Rows[i]["ManagerCode_Id"].ToString(), true.ToString(), Convert.ToDateTime(dt.Rows[i]["DOJ"].ToString()).AddMonths(6).ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dt.Rows[i]["Email-Id"].ToString(), dt.Rows[i]["Phone_No"].ToString(), "", "", "", "False", "", "", "1900-01-01", "", "1900-01-01", ref trns);
                        updateDeviceGoup(b.ToString(), dt.Rows[i]["DeviceGroup_Id"].ToString(), ref trns);
                        objEmp.InsertEmployeeLocationTransfer(b.ToString(), dt.Rows[i]["Location_Id"].ToString(), dt.Rows[i]["Location_Id"].ToString(), DateTime.Now.ToString(), "Employee Created", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                        InsertEmployeeParameterOnEmployeeInsert(Session["CompId"].ToString(), b.ToString(), Convert.ToDateTime(dt.Rows[i]["DOJ"].ToString()), ref trns);
                        if (dt.Rows[i]["Salary"] != null && dt.Rows[i]["Salary"].ToString() != "")
                        {
                            objDa.execute_Command("Update Set_Employee_Parameter set Basic_Salary='" + dt.Rows[i]["Salary"].ToString() + "' where Emp_Id='" + b.ToString() + "'", ref trns);

                        }
                        foreach (DataRow dr in dtNf.Rows)
                        {
                            try
                            {
                                objEmpNotice.InsertEmployeeNotification(b.ToString(), dr["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            catch
                            {
                            }
                        }
                        for (int k = 0; k < dtDevice.Rows.Count; k++)
                        {
                            objSer.InsertUserTransfer(b.ToString(), dtDevice.Rows[k]["Device_Id"].ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        strLabourLaw = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), dt.Rows[i]["Location_Id"].ToString(), ref trns).Rows[0]["Field3"].ToString();
                        //for labour law
                        if (strLabourLaw.Trim() != "0" && strLabourLaw.Trim() != "")
                        {
                            DataTable dtleavedetail = ObjLabourLeavedetail.GetRecord_By_LaborLawId(strLabourLaw, ref trns);
                            dtleavedetail = new DataView(dtleavedetail, "Gender='" + dt.Rows[i]["Gender"].ToString() + "' or Gender='Both'", "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dtleavedetail.Rows)
                            {
                                objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), b.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), "100", dr["schedule_type"].ToString(), true.ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), dr["is_auto"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                SaveLeave("No", dr["Leave_Type_Id"].ToString(), b.ToString(), dr["schedule_type"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), FinancialYearStartDate, FinancialYearEndDate, Convert.ToDateTime(dt.Rows[i]["DOJ"].ToString()), ref trns);
                                //here we are checking that any deduction slab exists or not for selected leave type
                                DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(dr["Leave_Type_Id"].ToString(), ref trns).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");
                                foreach (DataRow childrow in dtdeduction.Rows)
                                {
                                    ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), dt.Rows[i]["Location_Id"].ToString(), dr["Leave_Type_Id"].ToString(), b.ToString(), childrow["DaysFrom"].ToString(), childrow["Daysto"].ToString(), childrow["Deduction_Percentage"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                        strEmpId = b.ToString();
                        newhirecount++;
                    }
                    else
                    {
                        strEmpId = dt.Rows[i]["Code_Id"].ToString();
                        objDa.execute_Command("update set_employeemaster set Location_Id='" + dt.Rows[i]["Location_Id"].ToString() + "',emp_type='On Role',Emp_Name='" + dt.Rows[i]["Name"].ToString() + "',Field5='" + dt.Rows[i]["ManagerCode_Id"].ToString() + "',Emp_Name_L= N'" + dt.Rows[i]["Name_L"].ToString().Trim() + "',Civil_Id='" + dt.Rows[i]["Civil-Id"].ToString() + "',Phone_No='" + dt.Rows[i]["Phone_No"].ToString() + "',Gender='" + dt.Rows[i]["Gender"].ToString() + "',Email_Id='" + dt.Rows[i]["Email-Id"].ToString() + "',DOB='" + Convert.ToDateTime(dt.Rows[i]["DOB"].ToString()).ToString() + "',DOJ='" + Convert.ToDateTime(dt.Rows[i]["DOJ"].ToString()).ToString() + "',Department_Id='" + dt.Rows[i]["Department_Id"].ToString() + "',Designation_Id='" + dt.Rows[i]["Designation_Id"].ToString() + "',Religion_Id='" + dt.Rows[i]["Religion_Id"].ToString() + "',Nationality_Id='" + dt.Rows[i]["Nationality_Id"].ToString() + "',Qualification_Id='" + dt.Rows[i]["qualification_Id"].ToString() + "',Device_Group_Id='" + dt.Rows[i]["DeviceGroup_Id"].ToString() + "' where Emp_Id='" + dt.Rows[i]["Code_Id"].ToString() + "'", ref trns);
                        counter++;
                    }
                    //creating newuser if not exists
                    if (dt.Rows[i]["UserRole_Id"].ToString().Trim() != "0")
                    {
                        CreateUser(strEmpId, dt.Rows[i]["Code"].ToString().Trim(), dt.Rows[i]["UserRole_Id"].ToString().Trim(), dt.Rows[i]["Location_Id"].ToString(), dt.Rows[i]["Department_Id"].ToString(), ref trns);
                    }
                    //deleting from old  device group
                    if (strDeviceGroupId != dt.Rows[i]["DeviceGroup_Id"].ToString())
                    {
                        if (strDeviceGroupId != "0")
                        {
                            InsertRecordIndeviceOperation(strDeviceGroupId, dt.Rows[i]["Effectivedate"].ToString(), "Delete", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, "0", "", ref trns);
                        }
                        //inserting record in device operation table
                        if (dt.Rows[i]["DeviceGroup_Id"].ToString() != "0" && strEmpType.Trim() == "On Role")
                        {
                            InsertRecordIndeviceOperation(dt.Rows[i]["DeviceGroup_Id"].ToString(), dt.Rows[i]["Effectivedate"].ToString(), "Upload", strEmpId.Trim(), dtdevice, strDeviceGroupId, "", ref trns);
                        }
                    }
                }
                else if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TRANSFER" || dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TEMPTRANSFER" || dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "REVERSE TERMINATION")
                {
                    if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TRANSFER")
                    {
                        //if location not same then we will insert record in location transfer history table
                        if (strLocationId != dt.Rows[i]["Location_Id"].ToString())
                        {
                            //if (Convert.ToDateTime(dt.Rows[i]["Effectivedate"].ToString()).Day == DateTime.Now.Day && Convert.ToDateTime(dt.Rows[i]["Effectivedate"].ToString()).Month == DateTime.Now.Month && Convert.ToDateTime(dt.Rows[i]["Effectivedate"].ToString()).Year == DateTime.Now.Year)
                            //{
                            objEmp.InsertEmployeeLocationTransfer(dt.Rows[i]["Code_Id"].ToString().Trim(), strLocationId, dt.Rows[i]["Location_Id"].ToString(), DateTime.Now.ToString(), "Location Updated", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                            objDa.execute_Command("update set_employeemaster set Location_Id=" + dt.Rows[i]["Location_Id"].ToString() + ",Department_Id='" + dt.Rows[i]["Department_Id"].ToString() + "' where emp_id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                            //}
                        }
                        else
                        {
                            objDa.execute_Command("update set_employeemaster set Department_Id='" + dt.Rows[i]["Department_Id"].ToString() + "' where emp_id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                        }
                        try
                        {
                            if (strDeviceGroupId != dt.Rows[i]["DeviceGroup_Id"].ToString())
                            {
                                //deleting from old  device group
                                if (strDeviceGroupId != "0")
                                {
                                    InsertRecordIndeviceOperation(strDeviceGroupId, dt.Rows[i]["Effectivedate"].ToString(), "Delete", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, "0", "", ref trns);
                                }
                                //uploading in new device group
                                if (dt.Rows[i]["DeviceGroup_Id"].ToString() != "0" && strEmpType.Trim() == "On Role")
                                {
                                    InsertRecordIndeviceOperation(dt.Rows[i]["DeviceGroup_Id"].ToString(), dt.Rows[i]["Effectivedate"].ToString(), "Transfer", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, strDeviceGroupId, "", ref trns);
                                }
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
                    else
                    {
                        //for update we will also assign leave according Labour law
                        dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), dt.Rows[i]["Code_Id"].ToString().Trim(), ref trns);
                        strLabourLaw = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(), ref trns).Rows[0]["Field3"].ToString();
                        //for labour law
                        if (strLabourLaw.Trim() != "0" && strLabourLaw.Trim() != "")
                        {
                            DataTable dtleavedetail = ObjLabourLeavedetail.GetRecord_By_LaborLawId(strLabourLaw, ref trns);
                            dtleavedetail = new DataView(dtleavedetail, "Gender='" + dt.Rows[i]["Gender"].ToString() + "' or Gender='Both'", "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dtleavedetail.Rows)
                            {
                                dtEmpLeave = objEmpleave.GetEmployeeLeaveByEmpIdandLeaveTypeId(Session["CompId"].ToString(), dt.Rows[i]["Code_Id"].ToString().Trim(), dr["Leave_Type_Id"].ToString(), ref trns);
                                if (dtEmpLeave.Rows.Count > 0)
                                {
                                    continue;
                                }
                                objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), dt.Rows[i]["Code_Id"].ToString().Trim(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), "100", dr["schedule_type"].ToString(), true.ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), dr["is_auto"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                SaveLeave("No", dr["Leave_Type_Id"].ToString(), dt.Rows[i]["Code_Id"].ToString().Trim(), dr["schedule_type"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), FinancialYearStartDate, FinancialYearEndDate, Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString()), ref trns);
                                //here we are checking that any deduction slab exists or not for selected leave type
                                DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(dr["Leave_Type_Id"].ToString(), ref trns).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");
                                foreach (DataRow childrow in dtdeduction.Rows)
                                {
                                    ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(), dr["Leave_Type_Id"].ToString(), dt.Rows[i]["Code_Id"].ToString().Trim(), childrow["DaysFrom"].ToString(), childrow["Daysto"].ToString(), childrow["Deduction_Percentage"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                        if (dt.Rows[i]["DeviceGroup_Id"].ToString() != "0")
                        {
                            objDa.execute_Command("update set_employeemaster set isactive='True',Field2='False',Device_Group_Id=" + dt.Rows[i]["DeviceGroup_Id"].ToString() + " where emp_id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                            InsertRecordIndeviceOperation(dt.Rows[i]["DeviceGroup_Id"].ToString(), dt.Rows[i]["Effectivedate"].ToString(), "Upload", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, strDeviceGroupId, "", ref trns);
                        }
                        else
                        {
                            objDa.execute_Command("update set_employeemaster set isactive='True',Field2='False' where emp_id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                        }
                    }
                    counter++;
                }
                else if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "TERMINATION")
                {
                    if (Convert.ToDateTime(dt.Rows[i]["Effectivedate"].ToString()).Date <= DateTime.Now.Date)
                    {
                        objDa.execute_Command("update set_employeemaster set Field2='True',Termination_date='" + Convert.ToDateTime(dt.Rows[i]["Effectivedate"].ToString()).ToString() + "' where emp_id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                    }
                    //inserting record in device operation table
                    InsertRecordIndeviceOperation("0", dt.Rows[i]["Effectivedate"].ToString(), "Terminate", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, "0", "", ref trns);
                    counter++;
                }
                else if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "ON ROLE")
                {
                    //FOR UPDATE EMPLOYEE TYPE AS ON ROLE
                    objDa.execute_Command("UPDATE Set_EmployeeMaster SET Emp_Type='On Role',ModifiedDate='" + DateTime.Now.ToString() + "',ModifiedBy='" + Session["userId"].ToString() + "' WHERE Emp_Id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                    if (dt.Rows[i]["DeviceGroup_Id"].ToString() != "0")
                    {
                        InsertRecordIndeviceOperation(dt.Rows[i]["DeviceGroup_Id"].ToString(), dt.Rows[i]["Effectivedate"].ToString(), "Upload", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, strDeviceGroupId, "0", ref trns);
                        objDa.execute_Command("UPDATE Set_EmployeeMaster SET Emp_Type='On Role',ModifiedDate='" + DateTime.Now.ToString() + "',ModifiedBy='" + Session["userId"].ToString() + "',Device_Group_Id=" + dt.Rows[i]["DeviceGroup_Id"].ToString() + " WHERE Emp_Id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                    }
                    else
                    {
                        objDa.execute_Command("UPDATE Set_EmployeeMaster SET Emp_Type='On Role',ModifiedDate='" + DateTime.Now.ToString() + "',ModifiedBy='" + Session["userId"].ToString() + "' WHERE Emp_Id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                    }
                    counter++;
                }
                else if (dt.Rows[i]["Action_Type"].ToString().Trim().ToUpper() == "OFF ROLE")
                {
                    //FOR UPDATE EMPLOYEE TYPE AS OFF ROLE
                    objDa.execute_Command("UPDATE Set_EmployeeMaster SET Emp_Type='Off Role',ModifiedDate='" + DateTime.Now.ToString() + "',ModifiedBy='" + Session["userId"].ToString() + "' WHERE Emp_Id=" + dt.Rows[i]["Code_Id"].ToString().Trim() + "", ref trns);
                    InsertRecordIndeviceOperation("0", dt.Rows[i]["Effectivedate"].ToString(), "Delete", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, "0", "", ref trns);
                    counter++;
                }
                else
                {
                    InsertRecordIndeviceOperation("0", dt.Rows[i]["Effectivedate"].ToString(), "Delete", dt.Rows[i]["Code_Id"].ToString().Trim(), dtdevice, "0", "", ref trns);
                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            GetDocumentNumber();
            if (newhirecount > 0)
            {
                DisplayMessage(newhirecount.ToString() + " new employee inserted and " + counter.ToString() + " employee information updated");
            }
            else
            {
                DisplayMessage(counter.ToString() + " Employee information updated");
            }
            btnResetEmpInfo_Click(null, null);
            rbtnupdateoption.Checked = true;
            rbtnReportoption.Checked = false;
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
    public void InsertRecordIndeviceOperation(string strDeviceGroupId, string strEffectivedate, string strOperation, string stEmpId, DataTable dtDevice, string strOld_Device_Group_Id, string strRemarks, ref SqlTransaction trns)
    {
        //here added condition for show old group in device operation when old and new is difference  
        if (strOperation.Trim().ToUpper() == "UPLOAD" || strOperation.Trim().ToUpper() == "TRANSFER")
        {
            if (strDeviceGroupId == strOld_Device_Group_Id)
            {
                strOld_Device_Group_Id = "0";
            }
        }
        DataTable dtGroupDetail = new DataTable();
        if (strDeviceGroupId.Trim() != "0")
        {
            dtGroupDetail = ObjdeviceGroup.GetDetailRecord(strDeviceGroupId, ref trns);
            for (int j = 0; j < dtGroupDetail.Rows.Count; j++)
            {
                ObjdeviceOp.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtuploadReferenceNo.Text.Trim(), stEmpId.Trim(), strDeviceGroupId.Trim(), dtGroupDetail.Rows[j]["Device_Id"].ToString(), "Pending", strEffectivedate, strOperation, true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), strOld_Device_Group_Id, strRemarks, ref trns);
            }
        }
        else
        {
            for (int k = 0; k < dtDevice.Rows.Count; k++)
            {
                ObjdeviceOp.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtuploadReferenceNo.Text.Trim(), stEmpId.Trim(), strDeviceGroupId.Trim(), dtDevice.Rows[k]["Device_Id"].ToString(), "Pending", strEffectivedate, strOperation, true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), strOld_Device_Group_Id, strRemarks, ref trns);
            }
        }
    }
    protected void btndownloadInvalid_Click(object sender, EventArgs e)
    {
        //this event for download inavid record excel sheet 
        if (Session["UploadEmpDt"] == null)
        {
            DisplayMessage("Record Not found");
            return;
        }
        DataTable dt = (DataTable)(Session["UploadEmpDt"]);
        dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        ExportTableData(dt);
    }
    protected void btnResetEmpInfo_Click(object sender, EventArgs e)
    {
        gvSelected.DataSource = null;
        gvSelected.DataBind();
        Session["UploadEmpDt"] = null;
        Session["UploadEmpDtAll"] = null;
        ddlTables.Items.Clear();
        //need to get document number 
        rbtnupdall.Checked = true;
        rbtnupdValid.Checked = false;
        rbtnupdInValid.Checked = false;
        uploadEmpdetail.Visible = false;
        rbtnupdateoption.Checked = true;
        rbtnReportoption.Checked = false;
    }
    #endregion
    protected string GetDocumentNumber()
    {
        DataTable dt = new DataTable();
        DataTable dtCount = ObjdeviceOp.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "8", "15", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        int NoRow = 0;
        if (s != "")
        {
            dt = objDa.return_DataTable("select COUNT(distinct Reference_No)  from Att_Device_Operation where company_id=" + Session["CompId"].ToString() + " and brand_id=" + Session["BrandId"].ToString() + " and location_id=" + Session["LocId"].ToString() + "");
            if (dt.Rows.Count == 0)
            {
                s += "1";
            }
            else
            {
                NoRow = Convert.ToInt32(dt.Rows[0][0].ToString());
                bool bCodeFlag = true;
                while (bCodeFlag)
                {
                    NoRow += 1;
                    DataTable dtTemp = new DataView(dtCount, "Reference_No='" + s + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtTemp.Rows.Count == 0)
                    {
                        bCodeFlag = false;
                    }
                }
                s += NoRow;
            }
        }
        txtuploadReferenceNo.Text = s;
        return s;
    }
    protected void btnRefDelete_Click(object sender, EventArgs e)
    {
        if (gvDeviceOpHistory.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        int b = 0;
        foreach (GridViewRow gvrow in gvDeviceOpHistory.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvselect")).Checked)
            {
                b = ObjdeviceOp.DeleteRecord(((Label)gvrow.FindControl("lblTransId")).Text);
            }
        }
        if (b != 0)
        {
            DisplayMessage("Record deleted successfully");
            btnGet_Click(null, null);
        }
        else
        {
            DisplayMessage("Record not deleted successfully");
        }
    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (txtReferenceReport.Text.Trim() != "")
        {
            dt = ObjdeviceOp.GetRecord_By_ReferenceNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtReferenceReport.Text.Trim());
        }
        else
        {
            dt = ObjdeviceOp.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        if (ddlStatus.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Status='" + ddlStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddldeviceOp.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Operation_Type='" + ddldeviceOp.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        gvDeviceOpHistory.DataSource = dt;
        gvDeviceOpHistory.DataBind();
        Session["dtDeviceOperation"] = dt;
        lbltotalrefRecords.Text = "Total Record : " + dt.Rows.Count.ToString();
        dt.Dispose();
    }
    protected void gvDeviceOpHistory_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtDeviceOperation"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        gvDeviceOpHistory.DataSource = dt;
        gvDeviceOpHistory.DataBind();
        gvDeviceOpHistory.HeaderRow.Focus();
    }
    protected void btnRefRefresh_Click(object sender, EventArgs e)
    {
        gvDeviceOpHistory.DataSource = null;
        gvDeviceOpHistory.DataBind();
        txtReferenceReport.Text = "";
        rbtnupdateoption.Checked = false;
        rbtnReportoption.Checked = true;
        lbltotalrefRecords.Text = "Total Record : 0";
    }
    protected void txtReferenceReport_TextChanged(object sender, EventArgs e)
    {
        gvDeviceOpHistory.DataSource = null;
        gvDeviceOpHistory.DataBind();
        lbltotalrefRecords.Text = "Total Record : 0";
        if (txtReferenceReport.Text != "")
        {
            DataTable dt = ObjdeviceOp.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dt != null)
            {
                dt = new DataView(dt, "Reference_No='" + txtReferenceReport.Text + "' and Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and loCation_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Reference no. not exists");
                    txtReferenceReport.Text = "";
                    txtReferenceReport.Focus();
                }
            }
            else
            {
                DisplayMessage("Reference no. not exists");
                txtReferenceReport.Text = "";
                txtReferenceReport.Focus();
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefName(string prefixText, int count, string contextKey)
    {
        Att_Device_Operation ObjdeviceOP = new Att_Device_Operation(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjdeviceOP.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] str = new string[0];
        if (dt != null)
        {
            dt = new DataView(dt, "Reference_No like '%" + prefixText + "%' and Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and loCation_Id=" + HttpContext.Current.Session["LocId"].ToString() + " ", "Trans_id", DataViewRowState.CurrentRows).ToTable(true, "Reference_No");
            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Reference_No"].ToString();
                }
            }
        }
        else
        {
            str[0] = "";
        }
        return str;
    }
    protected void rbtnupdateoption_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnupdateoption.Checked)
        {
            Div_device_Report_operation.Visible = false;
            Div_device_upload_operation.Visible = true;
            btnResetEmpInfo_Click(null, null);
        }
        else
        if (rbtnReportoption.Checked)
        {
            Div_device_Report_operation.Visible = true;
            Div_device_upload_operation.Visible = false;
            btnRefRefresh_Click(null, null);
        }
    }
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/Employee", "MasterSetUp", "Employee", e.CommandName.ToString(), ((Label)gvrow.FindControl("lblempName")).Text + "(" + e.CommandName.ToString() + ")");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    protected void btndeviceOpExport_Click(object sender, EventArgs e)
    {
        gvDeviceOpHistory.Columns[0].Visible = false;
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Device_Operation_History.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            pnlExport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        gvDeviceOpHistory.Columns[1].Visible = true;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }
    protected void BtnMoreNumber_Command(object sender, CommandEventArgs e)
    {
        DataTable DtAddress = new DataTable();
        DtAddress = AM.GetAddressDataByAddressName(e.CommandArgument.ToString());
        string data = "";
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNodata = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNodata.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNodata.Rows)
                {
                    data = data + "<b>" + dr["Type"].ToString() + "</b>:" + dr["Phone_no"].ToString() + " <br/>";
                }
            }
        }
        if (data.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_Number_Open('" + data + "');", true);
        }
    }
    public string IsVisible(string data)
    {
        DataTable DtAddress = AM.GetAddressDataByAddressName(data);
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNoData = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");
            if (dt_ContactNoData.Rows.Count > 1)
            {
                return "More";
            }
            else
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }
    protected void btnEmpexport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //this button added for download full information of all active employee
        DataTable dtEmpInfo = objDa.return_DataTable("select *, case when FingerStatus='True' and FaceStatus='True' then 'Both' when FingerStatus='True' then 'Finger Only' when FaceStatus='True' then 'Face Only' else 'Not Registered' end as Registartion_status from (select set_employeemaster.emp_code,case when Len( set_employeemaster.emp_code)>5 then 'External' else 'Employee' end as Employee_Type,set_employeemaster.emp_type as Status,set_employeemaster.emp_name,att_devicegroupmaster.group_name,set_locationmaster.location_name,Set_DepartmentMaster.Dep_Name,set_designationmaster.Designation, case when sum( case when (Set_EmployeeInformation.template3='' or Set_EmployeeInformation.template3 Is null) then 0 else 1 end)>0 then 'True' else 'False' end as FingerStatus ,min( case when (Set_EmployeeInformation.template4='' or Set_EmployeeInformation.template4 Is null) then 'False' else 'True' end) as FaceStatus,sum( case when (Set_EmployeeInformation.template3='' or Set_EmployeeInformation.template3 Is null) then 0 else 1 end) as Finger_Count from set_employeemaster left join att_devicegroupmaster on set_employeemaster.device_group_id=att_devicegroupmaster.group_id left join Set_EmployeeInformation on set_employeemaster.emp_id=Set_EmployeeInformation.emp_id and (Set_EmployeeInformation.template3<>'' or Set_EmployeeInformation.Template4<>'') left join set_designationmaster on set_employeemaster.Designation_Id= Set_DesignationMaster.Designation_Id left join Set_DepartmentMaster on set_employeemaster.department_id =Set_DepartmentMaster.dep_id left join set_locationmaster on Set_EmployeeMaster.Location_Id = set_locationmaster.Location_Id where set_employeemaster.Field2='False' group by Set_EmployeeMaster.Emp_Id, set_employeemaster.emp_code,set_employeemaster.emp_name,set_locationmaster.location_name,Set_DepartmentMaster.Dep_Name,set_designationmaster.Designation,set_employeemaster.Field2,set_employeemaster.emp_type,att_devicegroupmaster.group_name )ab");
        ExportTableData(dtEmpInfo);
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string City_id = ObjCityMaster.GetCityIdFromStateIdNCityName(stateId, cityName);
        if (City_id != "")
        {
            return City_id;
        }
        else
        {
            return "0";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtState_TextChanged(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string stateId = ObjStatemaster.GetStateIdFromCountryIdNStateName(CountryId, StateName);
        if (stateId != "")
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = stateId;
            return stateId;
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static int txtAddressNameNew_TextChanged(string AddressName, string addressId)
    {
        // return  1 when 'Address Name Already Exists' and 0 when not present
        Set_AddressMaster AM = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string data = AM.GetAddressDataExistOrNot(AddressName, addressId);
        if (data == "0")
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
        {
            return null;
        }
        StateMaster objStateMaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["State_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
            {
                return null;
            }
            CityMaster objCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["City_Name"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetDistinctEmailId(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, gvEmp, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(gvEmp, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    private void setModuleWiseControls()
    {
        try
        {
            Li_Help.Visible = Help.Visible = false; // set help visibile false by default
            Common.clsApplicationModules _cls = (Common.clsApplicationModules)Session["clsApplicationModule"];
            Li_Assign.Visible = Assign.Visible = _cls.isAttendanceModule;
            Li_Leave.Visible = Leave.Visible = _cls.isAttendanceModule;
            Li_Alert.Visible = Alert.Visible = _cls.isAttendanceModule;
            if (_cls.isAttendanceModule || _cls.isHrAndPayrollModule)
            {
                Li_Salary.Visible = Salary.Visible = true;
            }
            else
            {
                Li_Salary.Visible = Salary.Visible = false;
            }
            Li_OT_PL.Visible = OT_PL.Visible = _cls.isAttendanceModule;
            Li_Upload.Visible = Upload.Visible = true;
            Li_Penalty.Visible = Penalty.Visible = _cls.isAttendanceModule;
        }
        catch (Exception ex)
        {

        }
    }


    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            string folderPath = "";
            try
            {
                string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
                folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch (Exception ex)
            {
                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }

        }

    }
    #endregion

}