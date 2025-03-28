using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using PegasusDataAccess;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public partial class MasterSetUp_CompanyMaster : BasePage
{
    UserMaster objUser = null;
    EmployeeMaster objEmp = null;
    Common cmn = null;
    CompanyMaster objComp = null;
    CountryMaster objCountry = null;
    CurrencyMaster objCurrency = null;
    SystemParameter objSys = null;
    Set_AddressMaster AM = null;
    Set_AddressCategory ObjAddressCat = null;
    Set_AddressChild objAddChild = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Device_Parameter objDeviceParam = null;
    Country_Currency objCountryCurrency = null;
    Set_DocNumber objDocNo = null;
    Set_ApprovalMaster ObjApproval = null;
    BrandMaster objBrand = null;
    LocationMaster objLocation = null;
    Set_Location_Department objLocDept = null;
    DepartmentMaster objDep = null;
    IT_ObjectEntry objObjectEntry = null;
    Inv_ParameterMaster objInvParam = null;
    Ac_Finance_Year_Info ObjFinance = null;
    Ac_FinancialYear_Detail ObjFinancedetail = null;
    hr_laborLaw_config ObjLabourLaw = null;
    Sys_LocationType_Master objLocationType = null;
    ContactNoMaster objContactnoMaster = null;
    UserDataPermission objUserDataPerm = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass objDa = null;
    UserMaster objUserMaster = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        hdntxtaddressid.Value = txtAddressName.ID;
        objUserMaster = new UserMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objCountry = new CountryMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        AM = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjAddressCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objDeviceParam = new Att_Device_Parameter(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjFinance = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        ObjFinancedetail = new Ac_FinancialYear_Detail(Session["DBConnection"].ToString());
        ObjLabourLaw = new hr_laborLaw_config(Session["DBConnection"].ToString());
        objLocationType = new Sys_LocationType_Master(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objUserDataPerm = new UserDataPermission(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        hdnEmployeeId.Value = txtManagerName.ID;
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetUp/CompanyMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            Session["empimgpath"] = null;
            txtValue.Focus();
            Session["CHECKED_ITEMS"] = null;
            Session["AddCtrl_Country_Id"] = "";
            Session["AddCtrl_State_Id"] = "";
            FillGridBin();
            FillGrid();
            FillCountryDDL();
            FillCurrencyDDL();
            FillddlParentCompanyDDL();
            ViewState["imgpath"] = null;
            Session["EditValue"] = "";
            try
            {
                ddlParentCompany.SelectedValue = Session["CompId"].ToString();
            }
            catch
            {

            }
            txtCompanyCode.Text = GetDocumentNumber();
            FillCountryCode();
            //AllPageCode();
            FillLocationType();
            FillLabourLaw();
            RootFolder();
            string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (ParmValue != "" && ParmValue != null)
            {
                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }            
        }
    }
    public void FillLabourLaw()
    {
        DataTable dt = ObjLabourLaw.GetAllTrueRecord(Session["CompId"].ToString());

        objPageCmn.FillData((object)ddlLabour, dt, "Laborlaw_Name", "Trans_Id");


    }
    protected void btnNewEmployee_Click(object sender, EventArgs e)
    {
        addEmployee.Reset();
        hdnEmployeeId.Value = txtManagerName.Text;
        //addEmployee.Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Employee_Open()", true);
    }
    public void FillLocationType()
    {
        objPageCmn.FillData((object)ddlLocationType, new DataView(objLocationType.GetAllActiveRecord(), "", "Location_Type_Name", DataViewRowState.CurrentRows).ToTable(), "Location_Type_Name", "Trans_Id");
    }

    #region CountryCallingCode
    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
            ddlCountry1.SelectedValue = ViewState["Country_Id"].ToString();
            ddlCountry1_SelectedIndexChanged(null, null);
            ViewState["CountryCode"] = objCountry.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        }
        catch
        {

        }
    }
    #endregion
    protected void txtManagerName_TextChanged(object sender, EventArgs e)
    {
        if (Session["EditValue"] != null)
        {
            if (HttpContext.Current.Session["EditValue"].ToString() != null)
            {
                if (txtManagerName.Text.Split('/').Length == 3)
                {
                    DataTable dt = Common.GetEmployee(txtManagerName.Text.Trim(), HttpContext.Current.Session["EditValue"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
                    if (dt.Rows.Count != 0)
                    {
                        txtManagerName.Text = "";
                        DisplayMessage("Select manager in the suggestion ");
                    }
                }
                else
                {
                    txtManagerName.Text = "";
                    DisplayMessage("Select manager in the suggestion ");

                }
            }
            else
            {
                txtManagerName.Text = "";
                DisplayMessage("Save company first then enter manager ");

            }
        }
        else
        {
            txtManagerName.Text = "";
            DisplayMessage("Save company first then enter manager ");
        }

    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        imgAddAddressName.Visible = clsPagePermission.bAdd;
        btnAddNewAddress.Visible = clsPagePermission.bAdd;

    }
    public string GetDocumentNumber()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(Session["CompId"].ToString(), "8", "6");

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }



            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += Session["CompId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += Session["BrandId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += Session["LocId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["DepartmentId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {

                DataTable Dtuser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();

            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }
            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(objComp.GetMaxCompanyId()) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(objComp.GetMaxCompanyId()) + 1).ToString();

            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(objComp.GetMaxCompanyId()) + 1).ToString();
        }

        return DocumentNo;

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["EditValue"].ToString() != null)
        {
            DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["EditValue"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

            string[] str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
            }
            return str;
        }
        else
        {
            return null;
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        Session["EditValue"] = editid.Value;

        DataTable dt = objComp.GetCompanyMasterById(editid.Value);
        if (dt.Rows.Count > 0)
        {
            DataTable dtrecord = objDa.return_DataTable("Select * from Inv_SalesInvoiceHeader where IsActive='1'");
            if(dtrecord.Rows.Count > 0)
            {
                txtCompanyName.ReadOnly = true;
                txtCompanyNameL.ReadOnly = true;
            }
            txtCompanyCode.ReadOnly = true;
            txtCompanyCode.Text = dt.Rows[0]["Company_Code"].ToString();
            txtCompanyName.Text = dt.Rows[0]["Company_Name"].ToString();            
            txtCompanyNameL.Text = dt.Rows[0]["Company_Name_L"].ToString();
            txtLicenceNo.Text = dt.Rows[0]["Commerical_License_No"].ToString();
            txtGSTIN.Text = dt.Rows[0]["Field1"].ToString();
            btnSave.Text = Resources.Attendance.Save;
            try
            {
                string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string Path = "~/CompanyResource/" + RegistrationCode + "/" + editid.Value + "";
                imgLogo.ImageUrl = Path + "/" + dt.Rows[0]["Logo_Path"].ToString();
            }
            catch(Exception ex)
            {
                try
                {
                    string Path = "~/CompanyResource/" + editid.Value + "";
                    imgLogo.ImageUrl = Path + "/" + dt.Rows[0]["Logo_Path"].ToString();
                }
                catch(Exception exp)
                {

                }
            }
            Session["empimgpath"] = dt.Rows[0]["Logo_Path"].ToString();
            try
            {
                ddlParentCompany.SelectedValue = dt.Rows[0]["Parent_Company_Id"].ToString();
            }
            catch
            {
            }
            try
            {
                ddlCountry1.SelectedValue = dt.Rows[0]["Country_Id"].ToString();
                DataTable dtCurency = objCountryCurrency.GetCurrencyByCountryId(ddlCountry1.SelectedValue, "1");
                if (dtCurency.Rows.Count > 0)
                {
                    ddlCurrency.SelectedValue = dtCurency.Rows[0]["Currency_Id"].ToString();
                }
                else
                {
                }
            }
            catch
            {
            }
            if (dt.Rows[0]["Emp_Id"].ToString().Trim() != "0" || dt.Rows[0]["Emp_Id"].ToString().Trim() != "")
            {
                txtManagerName.Text = cmn.GetEmpName(dt.Rows[0]["Emp_Id"].ToString(),HttpContext.Current.Session["CompId"].ToString());
            }
            else
            {
                txtManagerName.Text = "";
            }
            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Company", editid.Value);
            if (dtChild.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
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
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
        }
        else
        {
            btnSave.Text = Resources.Attendance.Next;
        }
        //AllPageCode();
    }
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
    protected void IbtnParam_Command(object sender, CommandEventArgs e)
    {


        if (objAppParam.GetApplicationParameterByCompanyId("In Func Key", e.CommandArgument.ToString()).Rows.Count == 0)
        {
            DisplayMessage("Company parameter not found  , add at least one location ");
            return;

        }



        Response.Redirect("../MasterSetup/CompanyParameter.aspx?CompanyId='" + e.CommandArgument.ToString() + "'");
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string txtCompanyName = objDa.get_SingleValue("Select Company_Name from Set_CompanyMaster where Company_Id='"+e.CommandArgument.ToString()+"'");
        string CompanyName = Common.Decrypt(objDa.get_SingleValue("Select company_name from Application_Lic_Main "));
        if (CompanyName == txtCompanyName)
        {
            DisplayMessage("This Company cannot be deleted");
            return;
        }

        int b = 0;
        string CompId = Session["CompId"].ToString();
        if (CompId == "0")
        {
            b = objComp.DeleteCompanyMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(e.CommandArgument.ToString());
            if (dtEmp.Rows.Count > 0)
            {
                b = -11;
            }
            else
            {
                b = objComp.DeleteCompanyMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }
        if (b != 0)
        {
            if (b == -11)
            {
                DisplayMessage("Company is Used for Transaction so you can not Delete this Company");
            }
            else
            {
                DisplayMessage("Record Deleted");
                FillGridBin();
                FillGrid();
                Reset();
            }
        }
        else
        {
            DisplayMessage("Record not Deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
    }
    protected void gvCompanyMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCompanyMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Cmp__Master"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvCompanyMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvCompanyMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Cmp__Master"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Cmp__Master"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvCompanyMaster, dt, "", "");
        //AllPageCode();
    }
    protected void ddlCountry1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable dtCurrEdit = objCountryCurrency.GetCurrencyByCountryId(ddlCountry1.SelectedValue, "1");
            if (dtCurrEdit.Rows.Count > 0)
            {
                ddlCurrency.SelectedValue = dtCurrEdit.Rows[0]["Currency_Id"].ToString();
                ddlCurrency.Enabled = true;
            }
            else
            {
                ddlCurrency.SelectedValue = "0";
                ddlCurrency.Enabled = false;
            }
        }
        catch (Exception Ex)
        {
        }
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvCompanyMasterBin.Rows)
        {
            index = (int)gvCompanyMasterBin.DataKeys[gvrow.RowIndex].Value;
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
    private void PopulateCheckedValuesemplog()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvCompanyMasterBin.Rows)
            {
                int index = (int)gvCompanyMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvCompanyMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvCompanyMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvCompanyMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvCompanyMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objComp.GetCompanyMasterInactive();

        DataTable dtUser = new DataTable();

        string CompanyId = string.Empty;

        dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            CompanyId = dtUser.Rows[0]["Company_Id"].ToString();
        }
        if (CompanyId == "0")
        {
            dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' or Parent_Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            if (Session["Company_Permission"] != null)
            {
                string Company_Id = Session["Company_Permission"].ToString();
                try
                {
                    dt = new DataView(dt, "Company_Id in(" + Company_Id.Substring(0, Company_Id.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
        }
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvCompanyMasterBin, dt, "", "");
        //AllPageCode();
    }

    protected void txtCompanyName_OnTextChanged(object sender, EventArgs e)
    {

        if (editid.Value == "")
        {
            DataTable dt = objComp.GetCompanyMasterByCompanyName(txtCompanyName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtCompanyName.Text = "";
                DisplayMessage("Company Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
                return;
            }
            DataTable dt1 = objComp.GetCompanyMasterInactive();
            dt1 = new DataView(dt1, "Company_Name='" + txtCompanyName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtCompanyName.Text = "";
                DisplayMessage("Company Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
                return;
            }
            txtCompanyNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objComp.GetCompanyMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Company_Name"].ToString() != txtCompanyName.Text)
                {
                    DataTable dt = objComp.GetCompanyMaster();
                    dt = new DataView(dt, "Company_Name='" + txtCompanyName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtCompanyName.Text = "";
                        DisplayMessage("Company Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
                        return;
                    }
                    DataTable dt1 = objComp.GetCompanyMaster();
                    dt1 = new DataView(dt1, "Company_Name='" + txtCompanyName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtCompanyName.Text = "";
                        DisplayMessage("Company Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
                        return;
                    }
                }
            }
            txtCompanyNameL.Focus();
        }
    }

    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }
    protected void btnHelp_Click(object sender, EventArgs e)
    {
        string url = "../Help.htm";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);

    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        Session["CHECKED_ITEMS"] = null;
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
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
            DataTable dtCust = (DataTable)Session["Company"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Cmp__Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvCompanyMaster, view.ToTable(), "", "");
            //AllPageCode();
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
            DataTable dtCust = (DataTable)Session["dtbinCompany"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvCompanyMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }

        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }



    public void InsertCompanyFinancialYear(string strCompanyId)
    {


        Ac_Finance_Year_Info objfinance = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());


        DataTable dtFinance = objfinance.GetInfoAllTrue(strCompanyId);

        if (dtFinance.Rows.Count == 0)
        {

            string strFinancecode = string.Empty;

            strFinancecode = "FYC-" + DateTime.Now.Year;


            objfinance.InsertInfo(strCompanyId, "1", strFinancecode, "2017-01-01", "2017-12-31", "Open", "", "", "1800-01-01 00:00:00.000", "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());

        }


    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ParentComany = string.Empty;
        int b = 0;

       

        if (txtCompanyCode.Text == "")
        {
            DisplayMessage("Enter Company Code");
            txtCompanyCode.Focus();
            return;
        }

        if (txtCompanyName.Text == "")
        {
            DisplayMessage("Enter Company Name");
            txtCompanyName.Focus();
            return;
        }

        if (Session["empimgpath"] == null)
        {
            Session["empimgpath"] = "";

        }
        //if (Session["CompId"].ToString() != "0" && Session["EmpId"].ToString() != "0")
        //{
        if (ddlParentCompany.SelectedIndex == 0)
        {
            ParentComany = "0";

        }
        else
        {
            ParentComany = ddlParentCompany.SelectedValue;
        }

        //}





        string empid = string.Empty;
        if (txtManagerName.Text != "")
        {
            empid = txtManagerName.Text.Split('/')[txtManagerName.Text.Split('/').Length - 1];

            DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count > 0)
            {
                empid = dtEmp.Rows[0]["Emp_Id"].ToString();
            }
            else
            {
                empid = "0";
            }

        }
        else
        {
            empid = "0";
        }
        if (editid.Value == "")
        {
            DataTable dtCheck = new DataTable();
            dtCheck = objDa.return_DataTable("");
            if (dtCheck.Rows.Count > 0)
            {
                DisplayMessage("Can't Create another Company");
                txtCompanyCode.Focus();
                return;
            }
            DataTable dt = objComp.GetCompanyMaster();

            dt = new DataView(dt, "Company_Code='" + txtCompanyCode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Company Code Already Exists");
                txtCompanyCode.Focus();
                return;

            }
            DataTable dt1 = objComp.GetCompanyMaster();

            dt1 = new DataView(dt1, "Company_Name='" + txtCompanyName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Company Name Already Exists");
                txtCompanyName.Focus();
                return;

            }

            if (ddlCountry1.SelectedIndex == 0)
            {

                DisplayMessage("Select Country");
                ddlCountry1.Focus();
                return;

            }
            if (ddlCurrency.SelectedIndex == 0)
            {

                DisplayMessage("Select Currency");
                ddlCurrency.Focus();
                return;

            }

            b = objComp.InsertCompanyMaster(txtCompanyName.Text, txtCompanyNameL.Text, txtCompanyCode.Text, Session["empimgpath"].ToString(), ParentComany, empid, ddlCurrency.SelectedValue, ddlCountry1.SelectedValue, txtLicenceNo.Text, txtGSTIN.Text, "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                Session["EditValue"] = b.ToString();

                string strMaxId = string.Empty;

                strMaxId = b.ToString();

                //here we are giving default permission to admin user 

                if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud" && Session["UserId"].ToString().Trim().ToLower() == "admin")
                {
                    objUserDataPerm.InsertUserDataPermission(Session["UserId"].ToString().Trim(), Session["CompId"].ToString(), "C", strMaxId, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }

                InsertCompanyFinancialYear(strMaxId);

                //insert the record in approval master 
                //code start

                //DataTable dtApproval = ObjApproval.GetApprovalMaster("1");

                //if (dtApproval.Rows.Count == 0)
                //{
                //    dtApproval = ObjApproval.GetApprovalMaster("2");
                //}


                //foreach (DataRow dr in dtApproval.Rows)
                //{
                //    b = ObjApproval.InsertApprovalMaster(strMaxId, dr["Approval"].ToString(), dr["Approval_L"].ToString(), dr["Field1"].ToString(), dr["Field2"].ToString(), dr["Field3"].ToString(), dr["Field4"].ToString(), dr["Field5"].ToString(), true.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //}
                //code end



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
                            objAddChild.InsertAddressChild(strAddressId, "Company", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }



                //if (ddlParentCompany.SelectedIndex != 0)
                //{
                //    DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", ddlParentCompany.SelectedValue);
                //    if (dtParam.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dtParam.Rows.Count; i++)
                //        {
                //            // objAppParam.InsertApplicationParameterMaster(strMaxId, dtParam.Rows[i]["Param_Name"].ToString(), dtParam.Rows[i]["Param_Value"].ToString(), dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //        }

                //    }

                //    //this function is created by jitendra upadhyay on 20-12-2014
                //    //this function for insert all inventory parameter for new company according parent company

                //    //code start
                //    //DataTable dtInvParameter = new DataTable();
                //    //dtInvParameter = objInvParam.GetParameterMasterAllTrue(ddlParentCompany.SelectedValue);
                //    //if (dtInvParameter.Rows.Count > 0)
                //    //{
                //    //    InsertInventoryParameter(strMaxId, ddlParentCompany.SelectedValue);
                //    //}
                //    //else
                //    //{
                //    //    InsertInventoryParameter(strMaxId,"1");
                //    //}
                //    //code end


                //}
                //else
                //{
                //    DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", "1");
                //    if (dtParam.Rows.Count == 0)
                //    {
                //        dtParam = objAppParam.GetApplicationParameterByCompanyId("", "2");
                //    }


                //    if (dtParam.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dtParam.Rows.Count; i++)
                //        {
                //            //  objAppParam.InsertApplicationParameterMaster(strMaxId, dtParam.Rows[i]["Param_Name"].ToString(), dtParam.Rows[i]["Param_Value"].ToString(), dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //        }

                //    }

                //    //this function is created by jitendra upadhyay on 20-12-2014
                //    //this function for insert all inventory parameter for new company according company id =1

                //    //code start
                //    //InsertInventoryParameter(strMaxId,"1");
                //    //code end


                //}


                //if (ddlParentCompany.SelectedIndex != 0)
                //{
                //    DataTable dtParam1 = objDeviceParam.GetDeviceParameterByCompanyId("", ddlParentCompany.SelectedValue);
                //    if (dtParam1.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dtParam1.Rows.Count; i++)
                //        {
                //            objDeviceParam.InsertDeviceParameterMaster(b.ToString(), "0", "0", dtParam1.Rows[i]["Param_Name"].ToString(), dtParam1.Rows[i]["Param_Value"].ToString(), dtParam1.Rows[i]["Param_Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //        }

                //    }

                //}
                //else
                //{
                //    DataTable dtParam1 = objDeviceParam.GetDeviceParameterByCompanyId("", "1");
                //    if (dtParam1.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dtParam1.Rows.Count; i++)
                //        {
                //            objDeviceParam.InsertDeviceParameterMaster(b.ToString(), "0", "0", dtParam1.Rows[i]["Param_Name"].ToString(), dtParam1.Rows[i]["Param_Value"].ToString(), dtParam1.Rows[i]["Param_Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //        }

                //    }

                //}


                hdnCompanyId.Value = strMaxId;
                //DisplayMessage("Record Saved","green");
                txtCompanyNameBrand.Text = txtCompanyName.Text;
                FillGrid();
                Reset();
                pnlBrand.Visible = true;
                PnlNewEdit.Visible = false;
                pnlLoc.Visible = false;
                txtBrandCode.Focus();
                //btnList_Click(null, null);
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {

            //This Function Created By Rahul Sharma on date 02-12-2023
            //Here We  SetExchangeRate According Currency
            objDa.execute_Command("Update Sys_CurrencyMaster Set Is_BaseCurrency='1' where Currency_Id='"+ ddlCurrency.SelectedValue + "'");
            string obj = objCurrency.SetCountryExchangeReate(ddlCurrency.SelectedValue);
            if (obj != "False")
            {
                // Deserialize the JSON string to a dynamic object using JObject
                dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(obj);
                JObject conversionRates = (JObject)jsonObject["conversion_rates"]; // Corrected line

                DataTable dtCurrency = new DataTable();
                dtCurrency = objDa.return_DataTable("Select * from Sys_CurrencyMaster where IsActive='1'");
                if (dtCurrency.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCurrency.Rows.Count; i++)
                    {
                        string CurrencyCode = dtCurrency.Rows[i]["Currency_Code"].ToString();
                        // Access the properties of the dynamic object
                        foreach (var kvp in conversionRates)
                        {
                            string Currency = kvp.Key;
                            decimal conversionRate = kvp.Value.Value<decimal>();
                            if (CurrencyCode == Currency)
                            {
                                objDa.execute_Command("Update Sys_CurrencyMaster Set Is_BaseCurrency='0',Currency_Value='" + conversionRate + "' where Currency_ID='" + dtCurrency.Rows[i]["Currency_ID"].ToString() + "'");
                                break;
                            }
                        }
                    }
                }
            }
            string CompanyCode = string.Empty;
            string CompanyName = string.Empty;

            DataTable dt = objComp.GetCompanyMaster();

            try
            {
                CompanyCode = (new DataView(dt, "Company_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Company_Code"].ToString();
            }
            catch
            {
                CompanyCode = "";
            }


            dt = new DataView(dt, "Company_Code='" + txtCompanyCode.Text + "' and Company_Code <>'" + CompanyCode + "'  ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Company Code Already Exists");
                txtCompanyCode.Focus();
                return;

            }
            DataTable dt1 = objComp.GetCompanyMaster();
            try
            {
                CompanyName = (new DataView(dt1, "Company_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Company_Name"].ToString();
            }
            catch
            {
                CompanyName = "";
            }
            dt1 = new DataView(dt1, "Company_Name='" + txtCompanyName.Text + "' and Company_Name<>'" + CompanyName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Company Name Already Exists");
                txtCompanyName.Focus();
                return;

            }
            if (ddlCountry1.SelectedIndex == 0)
            {

                DisplayMessage("Select Country");
                ddlCountry1.Focus();
                return;

            }
            if (ddlCurrency.SelectedIndex == 0)
            {

                DisplayMessage("Select Currency");
                ddlCurrency.Focus();
                return;

            }
           b = objComp.UpdateCompanyMaster(editid.Value, txtCompanyName.Text, txtCompanyNameL.Text, txtCompanyCode.Text, Session["empimgpath"].ToString(), ParentComany, empid, ddlCurrency.SelectedValue, ddlCountry1.SelectedValue, txtLicenceNo.Text, txtGSTIN.Text, "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {


                InsertCompanyFinancialYear(editid.Value);
                objAddChild.DeleteAddressChild("Company", editid.Value);

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
                            objAddChild.InsertAddressChild(strAddressId, "Company", editid.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                    }
                }

                DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", editid.Value);
                if (dtParam.Rows.Count == 0)
                {
                    dtParam = objAppParam.GetApplicationParameterByCompanyId("", "1");
                    for (int i = 0; i < dtParam.Rows.Count; i++)
                    {
                        //  objAppParam.InsertApplicationParameterMaster(editid.Value, dtParam.Rows[i]["Param_Name"].ToString(), dtParam.Rows[i]["Param_Value"].ToString(), dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                }

                DisplayMessage("Record Updated", "green");

                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                Logout();

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }

    }
    public void Logout()
    {
        SystemLog.SaveSystemLog("User LogOut", Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "", GetIpAddress()[0].ToString(), GetIpAddress()[1].ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["DBConnection"].ToString());
        objUserMaster.UpdateLogOutTime(Session["UserId"].ToString());
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/ERPLogin.aspx");
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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        Session["EditValue"] = null;
        //AllPageCode();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        Session["EditValue"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCompanyCode(string prefixText, int count, string contextKey)
    {
        CompanyMaster ObjCompanyMaster = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjCompanyMaster.GetCompanyMaster(), "Company_Code like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Company_Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCompanyName(string prefixText, int count, string contextKey)
    {
        CompanyMaster ObjCompanyMaster = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjCompanyMaster.GetCompanyMaster(), "Company_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Company_Name"].ToString();
        }
        return txt;
    }

    public void Reset()
    {

        FillCountryDDL();
        //FillCurrencyDDL();
        ddlCurrency.SelectedIndex = 0;
        ddlCurrencyLocation.SelectedIndex = 0;
        FillddlParentCompanyDDL();
        txtCompanyCode.Text = "";
        txtCompanyName.Text = "";
        txtCompanyNameL.Text = "";
        txtLicenceNo.Text = "";
        txtManagerName.Text = "";
        imgLogo.ImageUrl = "";

        ViewState["imgpath"] = null;

        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtAddressName.Text = "";
        GvAddressName.DataSource = null;
        GvAddressName.DataBind();
        GvAddressNameBrand.DataSource = null;
        GvAddressNameBrand.DataBind();
        addaddress.setCustomerID("NewCust");
        btnSave.Text = Resources.Attendance.Next;
        try
        {
            ddlParentCompany.SelectedValue = Session["CompId"].ToString();
        }
        catch
        {
        }
        txtCompanyCode.Text = GetDocumentNumber();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string compid = (int.Parse(objComp.GetMaxCompanyId()) + 1).ToString();
        if (editid.Value != "")
        {
            compid = editid.Value;
        }
        string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int fileSizeKB = FULogoPath.PostedFile.ContentLength / 1000;
        if (fileSizeKB > int.Parse(ParmValue))
        {

            FULogoPath.FileContent.Dispose();
            lblImgMessageShow.Text = "File size should be " + ParmValue + "KB or less.";
            //DisplayMessage("File size should be " + ParmValue + "KB or less.");
            return;

        }
        imgLogo.ImageUrl = "~/CompanyResource/" + "/" + compid + "/" + FULogoPath.FileName;
    }

    public void FillGrid()
    {


        DataTable dtUser = new DataTable();

        string CompanyId = string.Empty;
        dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            CompanyId = dtUser.Rows[0]["Company_Id"].ToString();
        }



        DataTable dt = new DataTable();

        dt = objComp.GetCompanyMaster();

        if (CompanyId == "0")
        {
            dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' or Parent_Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            if (Session["Company_Permission"] != null)
            {
                string Company_Id = Session["Company_Permission"].ToString();
                try
                {
                    dt = new DataView(dt, "Company_Id in(" + Company_Id.Substring(0, Company_Id.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
        }

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvCompanyMaster, dt, "", "");
        Session["dtFilter_Cmp__Master"] = dt;
        Session["Company"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    public void FillCountryDDL()
    {
        DataTable dt = objCountry.GetCountryMaster();
        if (dt.Rows.Count > 0)
        {
            ddlCountry1.DataSource = null;
            ddlCountry1.DataBind();
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlCountry1, dt, "Country_Name", "Country_Id");
        }
        else
        {
            try
            {
                ddlCountry1.Items.Insert(0, "--Select--");
                ddlCountry1.SelectedIndex = 0;
            }
            catch
            {
                ddlCountry1.Items.Insert(0, "--Select--");
                ddlCountry1.SelectedIndex = 0;
            }
        }
    }
    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();

        if (dt.Rows.Count > 0)
        {
            ddlCurrency.DataSource = null;
            ddlCurrency.DataBind();

            ddlCurrencyLocation.DataSource = null;
            ddlCurrencyLocation.DataBind();

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            objPageCmn.FillData((object)ddlCurrencyLocation, dt, "Currency_Name", "Currency_Id");
        }
        else
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;

            ddlCurrencyLocation.Items.Insert(0, "--Select--");
            ddlCurrencyLocation.SelectedIndex = 0;
        }

    }

    public void FillddlParentCompanyDDL()
    {
        DataTable dt = objComp.GetCompanyMaster();

        DataTable dtUser = new DataTable();

        string CompanyId = string.Empty;
        dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            CompanyId = dtUser.Rows[0]["Company_Id"].ToString();
        }

        if (CompanyId == "0")
        {
            dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' or Parent_Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            if (Session["Company_Permission"] != null)
            {
                string Company_Id = Session["Company_Permission"].ToString();
                try
                {
                    dt = new DataView(dt, "Company_Id in(" + Company_Id.Substring(0, Company_Id.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
            }
        }

        if (dt.Rows.Count > 0)
        {
            ddlParentCompany.DataSource = null;
            ddlParentCompany.DataBind();
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlParentCompany, dt, "Company_Name", "Company_Id");
        }
        else
        {
            try
            {
                ddlParentCompany.Items.Insert(0, "--Select--");
                ddlParentCompany.SelectedIndex = 0;
            }
            catch
            {
                ddlParentCompany.Items.Insert(0, "--Select--");
                ddlParentCompany.SelectedIndex = 0;
            }
        }

    }


    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = objComp.GetCompanyMasterInactive();

        DataTable dtUser = new DataTable();

        string CompanyId = string.Empty;
        dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
        if (dtUser.Rows.Count > 0)
        {
            CompanyId = dtUser.Rows[0]["Company_Id"].ToString();
        }


        if (CompanyId == "0")
        {
            dt = new DataView(dt, "Company_Id='" + Session["CompId"].ToString() + "' or Parent_Company_Id='" + Session["CompId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            if (Session["Company_Permission"] != null)
            {
                string Company_Id = Session["Company_Permission"].ToString();
                try
                {
                    dt = new DataView(dt, "Company_Id in(" + Company_Id.Substring(0, Company_Id.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
        }

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvCompanyMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinCompany"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {

            //AllPageCode();
        }
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvCompanyMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvCompanyMasterBin.Rows)
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
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
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

                if (!userdetails.Contains(dr["Company_Id"]))
                {
                    userdetails.Add(dr["Company_Id"]);
                }
            }
            foreach (GridViewRow GR in gvCompanyMasterBin.Rows)
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
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvCompanyMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvCompanyMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objComp.DeleteCompanyMaster(userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                        }
                    }
                }

                if (b != 0)
                {
                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;
                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvCompanyMasterBin.Rows)
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
                gvCompanyMasterBin.Focus();
                return;

            }
        }

    }
    //old code

    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        //ModalPopupExtender2.Hide();
        //pnlAddress2.Visible = false;
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
            //if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            //{
            //    ContactPhoneNo = DtAddress.Rows[0]["MobileNo1"].ToString();
            //}
            //if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            //{
            //    if (ContactPhoneNo != "")
            //    {
            //        ContactPhoneNo = ContactPhoneNo + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //    else
            //    {
            //        ContactPhoneNo = DtAddress.Rows[0]["MobileNo2"].ToString();
            //    }
            //}
        }
        return ContactPhoneNo;
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
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressName, dt, "", "");
        }
        return dt;
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
    #region Brand


    #region AddBrandAddress
    protected void chkgvSelect_CheckedChangedDefaultBrand(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GvAddressNameBrand.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
            chk.Checked = false;
        }

        CheckBox chk1 = (CheckBox)sender;

        chk1.Checked = true;


    }
    public DataTable CreateAddressDataTableBrand()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("Address");
        dt.Columns.Add("Is_Default", typeof(bool));
        return dt;
    }
    public DataTable FillAddressDataTabelBrand()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateAddressDataTableBrand();
        if (GvAddressNameBrand.Rows.Count > 0)
        {
            for (int i = 0; i < GvAddressNameBrand.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvAddressNameBrand.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblSNo = (Label)GvAddressNameBrand.Rows[i].FindControl("lblSNo");
                    Label lblAddressName = (Label)GvAddressNameBrand.Rows[i].FindControl("lblgvAddressName");

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
                    dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameBrand.Rows[i].FindControl("chkdefault")).Checked;

                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Trans_Id"] = (float.Parse(strNewSNo) + 1).ToString();
                    dt.Rows[i]["Address_Name"] = txtAddressNameBrand.Text;
                    try
                    {
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressNameBrand.Text);
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
            dt.Rows[0]["Address_Name"] = txtAddressNameBrand.Text;
            try
            {
                dt.Rows[0]["FullAddress"] = GetAddressByAddressName(txtAddressNameBrand.Text);
            }
            catch
            {

            }
            dt.Rows[0]["Is_Default"] = false.ToString();

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressNameBrand, dt, "", "");
        }

        return dt;
    }
    public DataTable FillAddressDataTabelDeleteBrand()
    {
        DataTable dt = CreateAddressDataTableBrand();
        for (int i = 0; i < GvAddressNameBrand.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressNameBrand.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressNameBrand.Rows[i].FindControl("lblgvAddressName");


            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;

            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameBrand.Rows[i].FindControl("chkdefault")).Checked;

        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAddressIdBrand.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    public DataTable FillAddressDataTableUpdateBrand()
    {
        DataTable dt = CreateAddressDataTableBrand();
        for (int i = 0; i < GvAddressNameBrand.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressNameBrand.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressNameBrand.Rows[i].FindControl("lblgvAddressName");

            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;

            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameBrand.Rows[i].FindControl("chkdefault")).Checked;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressIdBrand.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressNameBrand.Text;
                dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressNameBrand.Text);
            }
        }
        return dt;
    }
    protected void txtAddressNameBrand_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressNameBrand.Text != "")
        {
            DataTable dtAM = AM.GetAddressDataByAddressName(txtAddressNameBrand.Text);
            if (dtAM.Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(imgAddAddressNameBrand);
            }
            else
            {
                txtAddressNameBrand.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
        }
    }
    protected void imgAddAddressNameBrand_Click(object sender, EventArgs e)
    {
        if (txtAddressNameBrand.Text != "")
        {
            string strA = "0";
            foreach (GridViewRow gve in GvAddressNameBrand.Rows)
            {
                Label lblCAddressName = (Label)gve.FindControl("lblgvAddressName");
                if (txtAddressNameBrand.Text == lblCAddressName.Text)
                {
                    strA = "1";
                }
            }


            if (hdnAddressIdBrand.Value == "")
            {
                if (strA == "0")
                {
                    FillAddressChidGirdBrand("Save");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
                }
                else
                {
                    txtAddressNameBrand.Text = "";
                    DisplayMessage("Address Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
                }
            }
            else
            {
                if (txtAddressNameBrand.Text == hdnAddressNameBrand.Value)
                {
                    FillAddressChidGirdBrand("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
                }
                else
                {
                    if (strA == "0")
                    {
                        FillAddressChidGirdBrand("Edit");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
                    }
                    else
                    {
                        txtAddressNameBrand.Text = "";
                        DisplayMessage("Address Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameBrand);
        }
        txtAddressNameBrand.Focus();
    }
    protected void btnAddNewAddressBrand_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Close()", true);
        hdntxtaddressid.Value = txtAddressNameBrand.ID;
    }
    protected void btnAddressEditBrand_Command(object sender, CommandEventArgs e)
    {
        hdnAddressIdBrand.Value = e.CommandArgument.ToString();
        FillAddressDataTabelEditBrand();
        hdnAddressIdBrand.Value = "";
    }
    protected void btnAddressDeleteBrand_Command(object sender, CommandEventArgs e)
    {
        hdnAddressIdBrand.Value = e.CommandArgument.ToString();
        FillAddressChidGirdBrand("Del");
    }
    public void FillAddressChidGirdBrand(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillAddressDataTabelDeleteBrand();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillAddressDataTableUpdateBrand();
        }
        else
        {
            dt = FillAddressDataTabelBrand();
        }

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressNameBrand, dt, "", "");
        ResetAddressName();
    }
    public DataTable FillAddressDataTabelEditBrand()
    {
        DataTable dt = CreateAddressDataTableBrand();

        for (int i = 0; i < GvAddressNameBrand.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressNameBrand.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressNameBrand.Rows[i].FindControl("lblgvAddressName");

            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameBrand.Rows[i].FindControl("chkdefault")).Checked;

        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id='" + hdnAddressIdBrand.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtAddressNameBrand.Text = dt.Rows[0]["Address_Name"].ToString();
            hdnAddressNameBrand.Value = dt.Rows[0]["Address_Name"].ToString();
        }
        return dt;
    }
    #endregion


    protected void btnUploadBrand_Click(object sender, EventArgs e)
    {
        imgLogoBrand.ImageUrl = "~/CompanyResource/" + "/" + hdnCompanyId.Value + "/" + FULogoPathBrand.FileName;
    }

    protected void BtnSaveBrand_Click(object sender, EventArgs e)
    {
        int b = 0;

        if (txtBrandCode.Text == "")
        {
            DisplayMessage("Enter Brand Code");
            txtBrandCode.Focus();
            return;
        }

        if (txtBrandName.Text == "")
        {
            DisplayMessage("Enter Brand Name");
            txtBrandName.Focus();
            return;
        }


        if (ViewState["empimgpathBrand"] == null)
        {
            ViewState["empimgpathBrand"] = "";

        }

        string empid = string.Empty;

        empid = "0";









        b = objBrand.InsertBrandMaster(hdnCompanyId.Value, txtBrandName.Text, txtBrandNameL.Text, txtBrandCode.Text, empid, ViewState["empimgpathBrand"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            string strMaxId = string.Empty;
            strMaxId = b.ToString();
            hdnBrandId.Value = strMaxId;


            bool Isdefault = false;
            foreach (GridViewRow gvr in GvAddressNameBrand.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                {
                    Isdefault = true;
                    break;
                }

            }


            foreach (GridViewRow gvr in GvAddressNameBrand.Rows)
            {

                CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                if (GvAddressNameBrand.Rows.Count == 1)
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

                Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");

                if (lblGvAddressName.Text != "")
                {
                    DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                    if (dtAddId.Rows.Count > 0)
                    {
                        string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                        objAddChild.InsertAddressChild(strAddressId, "Brand", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }


            //DisplayMessage("Record Saved","green");
            txtCompanyNameLocation.Text = txtCompanyNameBrand.Text;
            txtBrandNameLocation.Text = txtBrandName.Text;
            ResetBrand();
            pnlLoc.Visible = true;
            pnlBrand.Visible = false;
        }
        else
        {
            DisplayMessage("Record Not Saved");
        }


    }
    protected void BtnResetBrand_Click(object sender, EventArgs e)
    {
        ResetBrand();

    }
    protected void BtnCancelBrand_Click(object sender, EventArgs e)
    {
        Reset();
        FillGrid();
        FillGridBin();
        ResetBrand();
        ResetLocation();
        PnlNewEdit.Visible = true;
        pnlBrand.Visible = false;
        pnlLoc.Visible = false;
        pnlLocDept.Visible = false;
        PnlList.Visible = true;
    }
    public void ResetBrand()
    {


        txtBrandCode.Text = "";
        txtBrandName.Text = "";
        txtBrandNameL.Text = "";

        imgLogoBrand.ImageUrl = "";
        txtAddressNameBrand.Text = "";
        GvAddressNameBrand.DataSource = null;
        GvAddressNameBrand.DataBind();




    }

    #endregion


    #region Location
    #region AddLocationAddress
    protected void chkgvSelect_CheckedChangedDefaultLocation(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GvAddressNameLocation.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
            chk.Checked = false;
        }

        CheckBox chk1 = (CheckBox)sender;

        chk1.Checked = true;


    }
    public DataTable CreateAddressDataTableLocation()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("Address");
        dt.Columns.Add("Is_Default", typeof(bool));
        return dt;
    }
    public DataTable FillAddressDataTabelLocation()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateAddressDataTableLocation();
        if (GvAddressNameLocation.Rows.Count > 0)
        {
            for (int i = 0; i < GvAddressNameLocation.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvAddressNameLocation.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblSNo = (Label)GvAddressNameLocation.Rows[i].FindControl("lblSNo");
                    Label lblAddressName = (Label)GvAddressNameLocation.Rows[i].FindControl("lblgvAddressName");

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
                    dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameLocation.Rows[i].FindControl("chkdefault")).Checked;


                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Trans_Id"] = (float.Parse(strNewSNo) + 1).ToString();
                    dt.Rows[i]["Address_Name"] = txtAddressNameLocation.Text;
                    try
                    {
                        dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressNameLocation.Text);
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
            dt.Rows[0]["Address_Name"] = txtAddressNameLocation.Text;
            try
            {
                dt.Rows[0]["FullAddress"] = GetAddressByAddressName(txtAddressNameLocation.Text);
            }
            catch
            {

            }
            dt.Rows[0]["Is_Default"] = false.ToString();

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvAddressNameLocation, dt, "", "");
        }
        return dt;
    }
    public DataTable FillAddressDataTabelDeleteLocation()
    {
        DataTable dt = CreateAddressDataTableLocation();
        for (int i = 0; i < GvAddressNameLocation.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressNameLocation.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressNameLocation.Rows[i].FindControl("lblgvAddressName");


            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;

            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameLocation.Rows[i].FindControl("chkdefault")).Checked;


        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id<>'" + hdnAddressIdLocation.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    public DataTable FillAddressDataTableUpdateLocation()
    {
        DataTable dt = CreateAddressDataTableLocation();
        for (int i = 0; i < GvAddressNameLocation.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressNameLocation.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressNameLocation.Rows[i].FindControl("lblgvAddressName");

            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;

            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameLocation.Rows[i].FindControl("chkdefault")).Checked;

        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressIdLocation.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressNameLocation.Text;
                dt.Rows[i]["FullAddress"] = GetAddressByAddressName(txtAddressNameLocation.Text);
            }
        }
        return dt;
    }
    protected void txtAddressNameLocation_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressNameLocation.Text != "")
        {
            DataTable dtAM = AM.GetAddressDataByAddressName(txtAddressNameLocation.Text);
            if (dtAM.Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(imgAddAddressNameLocation);
            }
            else
            {
                txtAddressNameLocation.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
        }
    }
    protected void imgAddAddressNameLocation_Click(object sender, EventArgs e)
    {
        if (txtAddressNameLocation.Text != "")
        {
            string strA = "0";
            foreach (GridViewRow gve in GvAddressNameLocation.Rows)
            {
                Label lblCAddressName = (Label)gve.FindControl("lblgvAddressName");
                if (txtAddressNameLocation.Text == lblCAddressName.Text)
                {
                    strA = "1";
                }
            }


            if (hdnAddressIdLocation.Value == "")
            {
                if (strA == "0")
                {
                    FillAddressChidGirdLocation("Save");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
                }
                else
                {
                    txtAddressNameLocation.Text = "";
                    DisplayMessage("Address Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
                }
            }
            else
            {
                if (txtAddressNameLocation.Text == hdnAddressNameLocation.Value)
                {
                    FillAddressChidGirdLocation("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
                }
                else
                {
                    if (strA == "0")
                    {
                        FillAddressChidGirdLocation("Edit");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
                    }
                    else
                    {
                        txtAddressNameLocation.Text = "";
                        DisplayMessage("Address Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
                    }
                }
            }
        }
        else
        {
            DisplayMessage("Enter Address Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAddressNameLocation);
        }
        txtAddressNameLocation.Focus();
        txtAddressNameLocation.Text = "";
    }
    protected void btnAddNewAddressLocation_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Close()", true);
        hdntxtaddressid.Value = txtAddressNameLocation.ID;
    }
    protected void btnAddressEditLocation_Command(object sender, CommandEventArgs e)
    {
        FillAddressDataTabelEditLocation();
    }
    protected void btnAddressDeleteLocation_Command(object sender, CommandEventArgs e)
    {
        hdnAddressIdLocation.Value = e.CommandArgument.ToString();
        FillAddressChidGirdLocation("Del");
        hdnAddressIdLocation.Value = "";
    }
    public void FillAddressChidGirdLocation(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillAddressDataTabelDeleteLocation();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillAddressDataTableUpdateLocation();
        }
        else
        {
            dt = FillAddressDataTabelLocation();
        }
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvAddressNameLocation, dt, "", "");
        ResetAddressName();
    }
    public DataTable FillAddressDataTabelEditLocation()
    {
        DataTable dt = CreateAddressDataTableLocation();

        for (int i = 0; i < GvAddressNameLocation.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvAddressNameLocation.Rows[i].FindControl("lblSNo");
            Label lblgvAddressName = (Label)GvAddressNameLocation.Rows[i].FindControl("lblgvAddressName");

            dt.Rows[i]["Trans_Id"] = lblSNo.Text;
            dt.Rows[i]["Address_Name"] = lblgvAddressName.Text;
            dt.Rows[i]["FullAddress"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressNameLocation.Rows[i].FindControl("chkdefault")).Checked;


        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Trans_Id='" + hdnAddressIdLocation.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtAddressNameLocation.Text = dt.Rows[0]["Address_Name"].ToString();
            hdnAddressNameLocation.Value = dt.Rows[0]["Address_Name"].ToString();

        }
        return dt;
    }
    #endregion

    #region upload

    protected void btnUploadLocationLoGo_Click(object sender, EventArgs e)
    {

        imgLocationLogo.ImageUrl = "~/CompanyResource/" + "/" + hdnCompanyId.Value + "/" + FULogoPathLocation.FileName;
        ViewState["imgpath"] = FULogoPathLocation.FileName;


    }
    #endregion

    void ResetLocation()
    {
        txtLocationCode.Text = "";
        txtLocationName.Text = "";
        txtLocationNameL.Text = "";
        GvAddressNameLocation.DataSource = null;
        GvAddressNameLocation.DataBind();
        ddlLocationType.SelectedIndex = 0;
        txtLocationCode.Focus();
        ViewState["imgpath"] = null;
        ddlCurrencyLocation.SelectedIndex = 0;
    }

    public void insertFinancerecord(string strLocationId)
    {
        string FinanceId = ObjFinance.GetInfoByStatus(Session["CompId"].ToString()).Rows[0]["Trans_Id"].ToString();

        ObjFinancedetail.InsertFinancialYearDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, FinanceId, "Open", "", "Open", "", DateTime.Now.ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());

    }
    protected void btnSaveLocation_Click(object sender, EventArgs e)
    {
        int b = 0;

        if (txtLocationCode.Text == "")
        {
            DisplayMessage("Enter Location Code");
            txtLocationCode.Focus();
            return;
        }
        if (ddlLocationType.SelectedIndex == 0)
        {
            DisplayMessage("Select Location Type");
            ddlLocationType.Focus();
            return;
        }

        if (txtLocationName.Text == "")
        {
            DisplayMessage("Enter Location Name");
            txtLocationName.Focus();
            return;
        }

        if (ddlCurrencyLocation.SelectedIndex == 0)
        {
            DisplayMessage("Enter Currency Name in Dropdownlist");
            ddlCurrencyLocation.Focus();
            return;
        }

        if (ViewState["imgpath"] == null)
        {
            ViewState["imgpath"] = "";
        }


        string empid = string.Empty;

        empid = "0";

        string LabourLaw = "0";
        string strFinancialMonth = string.Empty;
        string strworkdayMinute = string.Empty;
        string strYearlyHalfDay = string.Empty;
        string strweekoffday = string.Empty;
        if (ddlLabour.SelectedIndex > 0)
        {
            LabourLaw = ddlLabour.SelectedValue;


            DataTable dtLabourlaw = ObjLabourLaw.GetRecordbyTRans_Id(Session["CompId"].ToString(), ddlLabour.SelectedValue);

            if (dtLabourlaw.Rows.Count > 0)
            {
                strFinancialMonth = dtLabourlaw.Rows[0]["fy_start_month"].ToString();
                strworkdayMinute = dtLabourlaw.Rows[0]["work_day_minutes"].ToString();
                strYearlyHalfDay = dtLabourlaw.Rows[0]["yearly_halfday"].ToString();
                strweekoffday = dtLabourlaw.Rows[0]["week_off_day"].ToString();

            }

        }


        b = objLocation.InsertLocationMaster(hdnCompanyId.Value, txtLocationName.Text, txtLocationNameL.Text, txtLocationCode.Text, hdnBrandId.Value, "0", ddlLocationType.SelectedValue, empid, ddlCurrencyLocation.SelectedValue, ViewState["imgpath"].ToString(), LabourLaw, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        if (b != 0)
        {


            //financial year entry

            insertFinancerecord(b.ToString());



            //this code is created by jitendra upadhyay on 07-01-2015
            //this code for insert the all inventory parameter acording new location id

            //code start

            // ---------------------------------------------------
            DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", Session["CompId"].ToString());
            dtParam = new DataView(dtParam, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtParam.Rows.Count == 0)
            {
                dtParam = objAppParam.GetApplicationParameterByCompanyId("", "1");
                dtParam = new DataView(dtParam, "Brand_Id='1'  and Location_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (dtParam.Rows.Count > 0)
            {

                string strParamValue = string.Empty;

                for (int i = 0; i < dtParam.Rows.Count; i++)
                {



                    if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "FinancialYearStartMonth" && ddlLabour.SelectedIndex > 0)
                    {
                        strParamValue = strFinancialMonth;
                    }
                    else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Work Day Min" && ddlLabour.SelectedIndex > 0)
                    {
                        strParamValue = strworkdayMinute;
                    }
                    else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Half_Day_Count" && ddlLabour.SelectedIndex > 0)
                    {
                        strParamValue = strYearlyHalfDay;
                    }
                    else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Week Off Days" && ddlLabour.SelectedIndex > 0)
                    {
                        strParamValue = strweekoffday;
                    }
                    else
                    {
                        strParamValue = dtParam.Rows[i]["Param_Value"].ToString();
                    }


                    objAppParam.InsertApplicationParameterMaster(hdnCompanyId.Value, hdnBrandId.Value, b.ToString(), dtParam.Rows[i]["Param_Name"].ToString(), strParamValue, dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
            // -------------------------------------------------


            DataTable dtInvParam = objInvParam.GetParameterMasterAllTrue("1", "1", "1");
            if (dtInvParam.Rows.Count > 0)
            {
                for (int i = 0; i < dtInvParam.Rows.Count; i++)
                {
                    if (dtInvParam.Rows[i]["ParameterName"].ToString() == "Purchase Account Parameter" || dtInvParam.Rows[i]["ParameterName"].ToString() == "Sales Account Parameter")
                    {
                        objInvParam.InsertParameterMaster(hdnCompanyId.Value, hdnBrandId.Value, b.ToString(), dtInvParam.Rows[i]["ParameterName"].ToString(), "0", dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                    else
                    {
                        objInvParam.InsertParameterMaster(hdnCompanyId.Value, hdnBrandId.Value, b.ToString(), dtInvParam.Rows[i]["ParameterName"].ToString(), dtInvParam.Rows[i]["ParameterValue"].ToString(), dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
            //code end






            string strMaxId = string.Empty;
            strMaxId = b.ToString();
            hdnLocationId.Value = strMaxId;
            bool Isdefault = false;
            foreach (GridViewRow gvr in GvAddressNameLocation.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
                {
                    Isdefault = true;
                    break;
                }

            }



            foreach (GridViewRow gvr in GvAddressNameLocation.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("chkdefault");
                if (GvAddressNameLocation.Rows.Count == 1)
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

                Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");

                if (lblGvAddressName.Text != "")
                {
                    DataTable dtAddId = AM.GetAddressDataByAddressName(lblGvAddressName.Text);
                    if (dtAddId.Rows.Count > 0)
                    {
                        string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                        objAddChild.InsertAddressChild(strAddressId, "Location", strMaxId, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            //DisplayMessage("Record Saved","green");
            FillGrid();
            //Reset();
            //btnList_Click(null, null);
            lblLocationId1.Text = txtLocationCode.Text;
            lblLocationName1.Text = txtLocationName.Text;
            lblCompanyNameDept.Text = txtCompanyNameLocation.Text;
            lblBrandNameDept.Text = txtBrandNameLocation.Text;
            ResetLocation();
            pnlLoc.Visible = false;
            pnlLocDept.Visible = true;

            lstDepartmentSelect.DataSource = null;
            lstDepartmentSelect.DataBind();

            FillDepartment();
        }
        else
        {
            DisplayMessage("Record Not Saved");
        }


    }
    protected void btnResetLocation_Click(object sender, EventArgs e)
    {
        ResetLocation();
    }
    protected void btnCancelLocation_Click(object sender, EventArgs e)
    {
        Reset();
        FillGrid();
        FillGridBin();
        ResetBrand();
        ResetLocation();
        PnlNewEdit.Visible = true;
        pnlBrand.Visible = false;
        pnlLoc.Visible = false;
        pnlLocDept.Visible = false;
        PnlList.Visible = true;
    }
    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstDepartment.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstDepartment.Items)
            {
                if (li.Selected)
                {
                    lstDepartmentSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstDepartmentSelect.Items)
            {
                lstDepartment.Items.Remove(li);

            }

            lstDepartmentSelect.SelectedIndex = -1;

        }
        btnPushDept.Focus();


    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstDepartmentSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstDepartmentSelect.Items)
            {
                if (li.Selected)
                {
                    lstDepartment.Items.Add(li);
                }
            }
            foreach (ListItem li in lstDepartment.Items)
            {
                lstDepartmentSelect.Items.Remove(li);

            }

        }
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstDepartment.Items)
        {
            lstDepartmentSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstDepartmentSelect.Items)
        {
            lstDepartment.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstDepartmentSelect.Items)
        {
            lstDepartment.Items.Add(li);
        }
        foreach (ListItem li in lstDepartment.Items)
        {
            lstDepartmentSelect.Items.Remove(li);
        }
        btnPullAllDept.Focus();
    }
    protected void btnSaveDept_Click(object sender, EventArgs e)
    {
        if (lstDepartmentSelect.Items.Count == 0)
        {
            DisplayMessage("Select Department");
            return;
        }

        objLocDept.DeleteLocationMaster(hdnLocationId.Value);
        for (int i = 0; i < lstDepartmentSelect.Items.Count; i++)
        {
            objLocDept.InsertLocationDepartmentMaster(hdnLocationId.Value, lstDepartmentSelect.Items[i].Value, "0", "", "", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }

        DisplayMessage("Record Saved","green");
        Reset();
        FillGrid();
        FillGridBin();
        ResetBrand();
        ResetLocation();
        PnlNewEdit.Visible = true;
        pnlBrand.Visible = false;
        pnlLoc.Visible = false;
        pnlLocDept.Visible = false;
        PnlList.Visible = true;

    }
    protected void btnResetDept_Click(object sender, EventArgs e)
    {
        lstDepartment.Items.Clear();
        lstDepartment.DataSource = null;
        lstDepartment.DataBind();
        FillDepartment();
        lstDepartmentSelect.Items.Clear();
        lstDepartmentSelect.DataSource = null;
        lstDepartmentSelect.DataBind();
    }
    protected void btnCancelDept_Click(object sender, EventArgs e)
    {
        Reset();
        FillGrid();
        FillGridBin();
        ResetBrand();
        ResetLocation();
        PnlNewEdit.Visible = true;
        pnlBrand.Visible = false;
        pnlLoc.Visible = false;
        pnlLocDept.Visible = false;
        PnlList.Visible = true;
    }
    public void FillDepartment()
    {
        lstDepartment.Items.Clear();
        lstDepartment.DataSource = null;
        lstDepartment.DataBind();
        DataTable dtDept = new DataTable();
        dtDept = objDep.GetDepartmentMaster();
        if (dtDept.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)lstDepartment, dtDept, "Dep_Name", "Dep_Id");
        }
    }
    #endregion
    #region Filetobytearray
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
    #endregion
    #region Upload
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
            string compid = (int.Parse(objComp.GetMaxCompanyId())).ToString();         
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

                    string fullPath= "~/CompanyResource/" + RegistrationCode + "/" + compid + "";
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
                        catch(Exception ex)
                        {

                        }
                    }

                }
                catch
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
                        catch (Exception ex)
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
        string compid = (int.Parse(objComp.GetMaxCompanyId()) + 1).ToString();
        Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
        try
        {
            File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + "/" + compid + "/" + e.File.Name), buffer);
        }
        catch
        {
        }
        imgLogo.ImageUrl = e.File.FullName.ToString();
        Session["empimgpath"] = e.File.Name;
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
            }
            catch
            {
                folderPath = "~\\Product\\";
            }
            ASPxFileManager1.Settings.RootFolder = folderPath;
            ASPxFileManager1.Settings.InitialFolder = folderPath;
            ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
        }

    }
    #endregion
    #endregion
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {

            string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            int fileSizeKB = FULogoPath.PostedFile.ContentLength / 1000;
            if (fileSizeKB > int.Parse(ParmValue))
            {
                              
                lblImgMessageShow.Text = "File size should be " + ParmValue + "KB or less.";
                //DisplayMessage("File size should be " + ParmValue + "KB or less.");
                return;

            }
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                string compid = (int.Parse(objComp.GetMaxCompanyId()) + 1).ToString();
                if (editid.Value != "")
                {
                    compid = editid.Value;
                }
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + compid))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + compid);
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + compid + "/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                Session["empimgpath"] = FULogoPath.FileName;
            }
        }
    }

    protected void BrandLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPathBrand.HasFile)
        {
            string ext = FULogoPathBrand.FileName.Substring(FULogoPathBrand.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + hdnCompanyId.Value))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + hdnCompanyId.Value);
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + hdnCompanyId.Value + "/") + FULogoPathBrand.FileName;
                FULogoPathBrand.SaveAs(path);
                ViewState["empimgpathBrand"] = FULogoPathBrand.FileName;
            }
        }
    }

    protected void LocationLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPathLocation.HasFile)
        {
            string ext = FULogoPathLocation.FileName.Substring(FULogoPathLocation.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + hdnCompanyId.Value))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + hdnCompanyId.Value);
                }
                string path = Server.MapPath("~/CompanyResource/" + "/" + hdnCompanyId.Value + "/") + FULogoPathLocation.FileName;
                FULogoPathLocation.SaveAs(path);
                ViewState["imgpath"] = FULogoPathLocation.FileName;
            }
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
                        DisplayMessage("Address Name Already Exists");
                        FillAddressChidGird("Edit");
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
        addaddress.fillHeader(txtCompanyName.Text);
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
        addaddress.FillLocationNCode();
        addaddress.fillHeader(txtCompanyName.Text);
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
        //function Update by Rahul Sharma for automatic country select when add address
        try
        {
            if (txtAddressName.Text != "")
            {
                ddlCountry1.SelectedValue = objDa.get_SingleValue("Select CountryId from Set_AddressMaster where Address_Name='" + txtAddressName.Text + "' And IsActive='1' ");
                ddlCountry1_SelectedIndexChanged(null, null);
            }

        }
        catch(Exception ex)
        {

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

}