using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebUserControl_AddEmployee : System.Web.UI.UserControl
{
    BrandMaster ObjBrandMaster = null;
    Common ObjComman = null;
    private const string strPageName = "EmployeeMaster";
    LocationMaster ObjLocMaster = null;
    Set_ApplicationParameter objAppParam = null;
    SystemParameter objSys = null;
    PageControlsSetting objPageCtlSettting = null;
    EmployeeMaster objEmp = null;
    DataAccessClass objDa = null;
    Att_Employee_Notification objEmpNotice = null;
    public delegate void parentPageHandler(string strEmployeeName);
    public event parentPageHandler refreshControlsFromChild;


    protected void Page_Load(object sender, EventArgs e)
    {
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            FillBrand();
            Reset();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strGrade = "";
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
        bool IsIndemnity = false;

        int IndemnityYear = 0;
        string IsEmpTerminatd = string.Empty;
        string EmployeeLevel = "";
        string DesignationId = string.Empty;
        string QualificationId = string.Empty;
        string NationalityId = string.Empty;
        if (EmployeeLevel == "")
        {
            EmployeeLevel = "Employee";
        }


        string BankId = "0";
        int b = 0;

        string DOj = "";



        if (DOj != "")
        {


        }
        else
        {
            DOj = DateTime.Now.ToString(objSys.SetDateFormat());
        }
        if (ddlBrand.SelectedValue == "")
        {

            ddlBrand.Focus();
            return;
        }
        else
        {
            //ddlLocation.Focus();
        }
        if (ddlLocation.SelectedValue == "")
        {

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
            // DisplayMessage(result[1]);
            return;
        }
        //here we are checking labour law for selected lolcation
        string strLabourLaw = string.Empty;
        strLabourLaw = ObjLocMaster.GetLocationMasterById(Session["CompId"].ToString(), ddlLocation.SelectedValue).Rows[0]["Field3"].ToString();

        string CountryCodewithMobileNumber = string.Empty;
        string txtPhoneNo = "";
        if (txtPhoneNo != "")
        {
            CountryCodewithMobileNumber = txtPhoneNo;
        }
        string strteamlederId = "0";

        DateTime terminationDate = new DateTime();
        terminationDate = new DateTime(1900, 1, 1);


        IsEmpTerminatd = false.ToString();



        if (Session["empimgpath"] == null)
        {
            Session["empimgpath"] = "";
        }
        try
        {
            if (Session["empSignimgpath"].ToString() == null)
            {
                Session["empSignimgpath"] = "";
            }
        }
        catch (Exception ex)
        {
            Session["empSignimgpath"] = "";
        }

        strReligion = "0";
        DesignationId = "0";
        QualificationId = "0";
        NationalityId = "0";




        //16-04-2015
        DataTable dtEmpCode = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), txtEmpCode.Text);
        if (dtEmpCode.Rows.Count > 0)
        {
            //DisplayMessage("Employee Code Already exists");
            //return;
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

                    //DisplayMessage("Maximum Employees is exceeded so please update your license");
                    //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count == null ? "0" : clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString());
                    //UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), "0", clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                    //DisplayMessage("Modal_UpdateLicense_Open()");
                    return;
                }
            }
            try
            {
                b = objEmp.InsertEmployeeMaster(Session["CompId"].ToString(), txtEmpName.Text.Split('/')[0].ToString(), txtEmpNameL.Text, txtEmpCode.Text, Session["empimgpath"].ToString(), ddlBrand.SelectedValue, ddlLocation.SelectedValue, "", "", DesignationId, strReligion, NationalityId, QualificationId, "1900-01-01", DOj, "", terminationDate.ToString(), ddlGender.SelectedValue, EmployeeLevel, IsEmpTerminatd, Session["empSignimgpath"].ToString(), "", strteamlederId, true.ToString(), DateTime.Now.ToString("yyyy-MMM-dd"), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", CountryCodewithMobileNumber, "", "", "", "False", "", strGrade, "", "", "");


                objEmp.InsertEmployeeLocationTransfer(b.ToString(), ddlLocation.SelectedValue, ddlLocation.SelectedValue, DateTime.Now.ToString(), "Employee Created", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
                // Nitin Jain On 27/11/2014 , Insert Into Indemnity Table 
                //int Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), b.ToString(), Convert.ToDateTime(DOj).AddYears(IndemnityYear).ToString(), "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                SystemLog.SaveSystemLog("Employee Master", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Employee Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
                if (b != 0)
                {
                    int c = 0;
                    int d = 0;
                    int f = 0;
                    // InsertEmployeeParameterOnEmployeeInsert(Session["CompId"].ToString(), b.ToString());

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

                    string strMaxId = string.Empty;
                    strMaxId = b.ToString();
                    bool Isdefault = false;
                    //Lokesh / () / 1
                    try
                    {
                        ((TextBox)Parent.FindControl(((HiddenField)Parent.FindControl("hdnEmployeeId")).Value)).Text = txtEmpName.Text + "/()/" + txtEmpCode.Text;
                    }
                    catch
                    {
                    }
                    Reset();
                    string strEmployeeName = txtEmpName.Text;
                    if (this.refreshControlsFromChild != null)
                    {
                        refreshControlsFromChild(strEmployeeName);
                    }
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), UniqueID, "Modal_Employee_Close()", true);
                }
            }
            catch (Exception ex)
            {


            }               //FillDataListGrid();

        }

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    public void Reset()
    {
        txtEmpName.Text = "";
        txtEmpNameL.Text = "";
        txtEmpCode.Text = "";
    }

    public void FillBrand()
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
    }
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

    }
    public void fillDropdown(DropDownList ddl, DataTable dt, string DataTextField, string DataValueField)
    {
        ddl.DataSource = dt;
        ddl.DataTextField = DataTextField;
        ddl.DataValueField = DataValueField;
        ddl.DataBind();
        //AllPageCode();
    }
}