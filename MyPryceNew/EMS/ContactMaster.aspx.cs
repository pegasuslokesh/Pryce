using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using PegasusDataAccess;
using System.Data.OleDb;
using ClosedXML.Excel;
using System.Data.SqlClient;
public partial class EMS_ContactMaster : BasePage
{

    #region Class Object
    ArrayList arr = new ArrayList();
    DataAccessClass da = null;
    Ems_ContactMaster ObjContactMaster = null;
    DepartmentMaster ObjDepMaster = null;
    DesignationMaster ObjDesMaster = null;
    ReligionMaster ObjRelMaster = null;
    Set_AddressMaster ObjAddMaster = null;
    Set_BankMaster objBankMaster = null;
    Set_AddressCategory ObjAddCat = null;
    Set_AddressChild objAddChild = null;
    Ems_Contact_Group objCG = null;
    Ems_GroupMaster objGroup = null;
    CompanyMaster ObjCompMaster = null;
    CountryMaster objCountryMaster = null;
    BrandMaster ObjBrandMaster = null;
    Set_CustomerMaster ObjCustomerMaster = null;
    Set_Suppliers ObjSupplierMaster = null;
    Ems_ContactCompanyBrand ObjCompanyBrand = null;
    Contact_BankDetail objContactBankDetail = null;
    Set_DocNumber ObjDocumentNo = null;
    //For Arcawing
    ES_EmailMasterDetail objEmailDetail = null;
    ES_EmailMaster_Header objEmailHeader = null;
    Country_Currency objCountryCurrency = null;
    CurrencyMaster objCurrency = null;
    Ac_ParameterMaster objAccParameter = null;
    Common cmn = null;
    Set_CustomerSupplier_Logo objCustSupLogo = null;
    ContactNoMaster objContactnoMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;
    Set_ApplicationParameter objAppParam = null;
    public const int grdDefaultColCount = 8;
    private const string strPageName = "ContactMaster";
    //end

    #endregion

    protected string FuLogoUploadFolderPath = "~/CompanyResource/Contact/";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        hdntxtaddressid.Value = txtAddressName.ID;
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjDepMaster = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjDesMaster = new DesignationMaster(Session["DBConnection"].ToString());
        ObjRelMaster = new ReligionMaster(Session["DBConnection"].ToString());
        ObjAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        ObjAddCat = new Set_AddressCategory(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjCompMaster = new CompanyMaster(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjCustomerMaster = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Ems_ContactCompanyBrand(Session["DBConnection"].ToString());
        objContactBankDetail = new Contact_BankDetail(Session["DBConnection"].ToString());
        ObjDocumentNo = new Set_DocNumber(Session["DBConnection"].ToString());
        //For Arcawing
        objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objCustSupLogo = new Set_CustomerSupplier_Logo(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../EMS/ContactMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            navTree.Attributes.Add("onclick", "OnTreeClick(event)");
            Reset();

            FillGroup();
            FillCurrencyDDL();
            Session["Long"] = null;
            Session["Lati"] = null;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            try
            {
                string strCurrencyId = Session["LocCurrencyId"].ToString();
                ddlCurrency.SelectedValue = strCurrencyId;
                hdncountryid.Value = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
                txtCountryName.Text = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Name"].ToString();
                txtCountryFilter.Text = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Name"].ToString();
                ViewState["CountryCode"] = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Code"].ToString();
            }
            catch
            {

            }

            if (Request.QueryString["EmailId"] != null)
            {
                txtPermanentMailId.Text = Request.QueryString["EmailId"].ToString();
                btnAddEmail_Click(null, null);
                btnNew_Click(null, null);
                if (Request.QueryString["CountryId"] != "0")
                {
                    hdncountryid.Value = Request.QueryString["CountryId"].ToString();
                    txtCountryName.Text = Request.QueryString["CountryName"].ToString();
                }
            }
            if (Request.QueryString["Page"] != null)
            {
                btnNew_Click(null, null);
                txtName.Focus();
            }

            FillCountryCode();

            txtId.Text = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

            chkIsEmail.Checked = true;
            chkIsSMS.Checked = true;
            btnList_Click(null, null);
            //btngo_Click(null, null);
            getPageControlsVisibility();
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
        navTree.Attributes.Add("onclick", "postBackByObject()");
        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
        addaddress.refreshControlsFromChild += new WebUserControl_AddressControl.parentPageHandler(ucAddAddress_refreshPageControl);

        txtId.Focus();
        //AllPageCode();
    }


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
            DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            CompanyMaster objComp = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string compid = (int.Parse(objComp.GetMaxCompanyId())).ToString();
            try
            {
                string RegistrationCode = Common.Decrypt(Objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                fullname = fullname.Replace("Product_", "Product//Product_");
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/" + RegistrationCode + "/Contact";
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
            catch
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/Contact";
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
            HttpContext.Current.Session["Contactimgpath"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
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
                string RegistrationCode = Common.Decrypt(da.get_SingleValue("Select registration_code from Application_Lic_Main"));
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
    #endregion

    private void ucAddAddress_refreshPageControl(string strAddressName)
    {
        txtAddressName.Text = strAddressName;
        Update_New.Update();
    }
    //Refresh update penel from web control
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
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
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region CountryCallingCode
    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = Session["LocCurrencyId"].ToString();

            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();

            ViewState["CountryCode"] = objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        }
        catch
        {

        }

        DataTable dt = objCountryMaster.getCountryCallingCode();
        if (dt.Rows.Count > 0)
        {
            //ddlCountryCode.DataSource = dt;
            //ddlCountryCode.DataTextField = "CountryCodeName";
            //ddlCountryCode.DataValueField = "CountryCodeValue";
            //ddlCountryCode.DataBind();

            //ddlCountryCode_Phoneno1.DataSource = dt;
            //ddlCountryCode_Phoneno1.DataTextField = "CountryCodeValue";
            //ddlCountryCode_Phoneno1.DataValueField = "CountryCodeValue";
            //ddlCountryCode_Phoneno1.DataBind();

            if (ViewState["CountryCode"] != null)
            {
                try
                {
                    //ddlCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                    //ddlCountryCode_Phoneno1.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                }
                catch
                {
                }
            }
        }



    }


    #endregion
    #region System Defined Function

    protected void btnList_Click(object sender, EventArgs e)
    {

        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;

        txtValue.Focus();
        if (ViewState["NotFillGrid"] == null)
        {
            //FillGrid();
        }
        else
        {
            if (ViewState["NotFillGrid"].ToString() == "Yes")
            {

            }
            else
            {
                //FillGrid();
            }
        }
        FillGrid();
        //AllPageCode();

    }

    protected void btnNew_Click(object sender, EventArgs e)
    {


        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        if (Lbl_Tab_New.Text == "View")
        {
            Lbl_Tab_New.Text = "New";
        }

        PnlNewEdit.Visible = true;
        txtId.Focus();
        //AllPageCode();
        ContactNo1.setNullToGV();
        if (ViewState["CountryCode"] != null)
        {
            ContactNo1.setCountryCode("+" + ViewState["CountryCode"].ToString());
        }
        ViewState["NotFillGrid"] = null;
        txtId.Focus();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {

        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        FillGridBin();
        txtValueBin.Focus();
        //AllPageCode();
    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {

        FillGrid();

        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Focus();
    }
    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "' and IsActive='True'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = ddlFieldName.SelectedValue + " Like '%" + txtValue.Text.Trim() + "%' and IsActive='True'";
            }
            else
            {
                condition = ddlFieldName.SelectedValue + " like '" + txtValue.Text.Trim() + "%' and IsActive='True'";
            }
            DataTable dtContact = null;
            if (ddlFieldName.SelectedValue != "CompanyName")
            {


                dtContact = ObjContactMaster.GetContactForMainGrid(condition);
            }
            else
            {
                condition = condition.Replace("CompanyName", "Name");
                dtContact = ObjContactMaster.GetContactForMainGridByComp(condition);
            }

            if (dtContact != null)
            {
                //  DataView view = new DataView(dtContact, condition, "", DataViewRowState.CurrentRows);

                gvContact.DataSource = dtContact;
                gvContact.DataBind();

                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count + "";
                ViewState["dtFilter"] = dtContact;
                //view.ToTable();
            }
        }
        //AllPageCode();
        btnbind.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        string strIsEmail = string.Empty;
        string strIsSMS = string.Empty;
        string strDepartmentId = string.Empty;
        string strDesignationId = string.Empty;
        string strReligionId = string.Empty;
        string strCurrencyId = string.Empty;
        if (txtId.Text == "")
        {
            DisplayMessage("Enter Id");
            txtId.Focus();
            return;

        }
        if (txtName.Text == "")
        {
            DisplayMessage("Enter Name");
            txtName.Focus();
            return;
        }


        if (ddlDepartment.SelectedValue == "--Select--")
        {
            strDepartmentId = "0";
        }
        else
        {
            strDepartmentId = ddlDepartment.SelectedValue;
        }
        if (ddlDesignation.SelectedValue == "--Select--")
        {
            strDesignationId = "0";
        }
        else
        {
            strDesignationId = ddlDesignation.SelectedValue;
        }
        if (ddlReligion.SelectedValue == "--Select--")
        {
            strReligionId = "0";
        }
        else
        {
            strReligionId = ddlReligion.SelectedValue;
        }


        if (ddlCurrency.SelectedValue == "--Select--")
        {
            strCurrencyId = "0";
        }
        else
        {
            strCurrencyId = ddlCurrency.SelectedValue;
        }

        if (chkIsEmail.Checked == true)
        {
            strIsEmail = "True";
        }
        else if (chkIsEmail.Checked == false)
        {
            strIsEmail = "False";
        }
        if (chkIsSMS.Checked == true)
        {
            strIsSMS = "True";
        }
        else if (chkIsSMS.Checked == false)
        {
            strIsSMS = "False";
        }
        string Status = string.Empty;
        string strNamePrefix = string.Empty;

        if (RdolistSelect.SelectedValue == "Individual")
        {
            Status = "Individual";
            strNamePrefix = ddlNamePrefix.SelectedItem.Value.ToString();
            PnlGSTIN.Visible = false;
        }
        else
        {
            Status = "Company";
            strNamePrefix = "0";
        }

        if (txtCountryName.Text.Trim() == "")
        {
            hdncountryid.Value = "0";
        }


        string DefaultEmailId = string.Empty;
        if (rbnEmailList.Items.Count > 0)
        {
            DefaultEmailId = rbnEmailList.SelectedValue.ToString();
        }

        string StrComp = string.Empty;
        try
        {
            StrComp = txtCompany.Text.Split('/')[1].ToString();
        }
        catch
        {

            StrComp = "0";

        }


        string strParentContactId = string.Empty;

        try
        {
            strParentContactId = txtParentContactName.Text.Split('/')[1].ToString();
        }
        catch
        {

            strParentContactId = "0";

        }


        if (txtPermanentMailId.Text == "" && rbnEmailList.Items.Count == 0)
        {
            DisplayMessage("Enter Email-Id");
            txtPermanentMailId.Focus();
            return;
        }
        else
        {
            if (txtPermanentMailId.Text != "")
            {
                if (!IsValidEmailId(txtPermanentMailId.Text))
                {
                    DisplayMessage("Enter Valid Email-Id");
                    txtPermanentMailId.Focus();
                    return;
                }
            }
        }


        string CountryCodewithMobileNumber = string.Empty;


        //if (txtRemark.Text.Trim() == "")
        //{
        //    DisplayMessage("Write about Business, size and suggested solution in Notes");
        //    txtRemark.Focus();
        //    return;
        //}

        if (Session["Contactimgpath"] == null)
        {
            Session["Contactimgpath"] = "";
        }


        //string type, code, number, extension;
        CheckBox isdefault;
        int count = 0;

        DataTable dt_number = ContactNo1.getDatatable();

        for (int i = 0; i < dt_number.Rows.Count; i++)
        {
            if (dt_number.Rows[i]["Is_default"].ToString() == "True")
            {
                count++;
                CountryCodewithMobileNumber = dt_number.Rows[i]["Country_code"].ToString() + "-" + dt_number.Rows[i]["Phone_no"].ToString();
            }
        }


        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Type"), new DataColumn("Country_code"), new DataColumn("Phone_no"), new DataColumn(("Is_default"), typeof(bool)), new DataColumn("Extension_no") });

        if (hdnContactId.Value != "")
        {
            b = ObjContactMaster.UpdateContactMaster(hdnContactId.Value, txtId.Text, txtName.Text.Trim(), txtNameL.Text, txtCivilId.Text, strDepartmentId, strDesignationId, strReligionId, StrComp, strIsEmail, strIsSMS, Status.ToString(), chkIsReseller.Checked.ToString(), DefaultEmailId.ToString(), CountryCodewithMobileNumber, strNamePrefix.ToString(), hdncountryid.Value, strCurrencyId, chkVerify.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), txtRemark.Text, strParentContactId);
            objContactnoMaster.deteteDate("Ems_ContactMaster", hdnContactId.Value);

            for (int i = 0; i < dt_number.Rows.Count; i++)
            {
                if (count == 0 && i == 0)
                {
                    objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", hdnContactId.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString());
                }
                else
                {
                    objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", hdnContactId.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), dt_number.Rows[i]["Is_default"].ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                }
            }




            ViewState["NotFillGrid"] = "Yes";
            if (b != 0)
            {
                //Delete Row From Address Child  Based on this Id
                objAddChild.DeleteAddressChild("Contact", hdnContactId.Value.ToString());
                objEmailDetail.ES_EmailMasterDetail_DeleteByRefIdType(hdnContactId.Value.ToString(), "Contact");

                //insert record for save contact logo 


                objCustSupLogo.DeleteSet_CustomerSupplier_LogoByCodeAndId(hdnContactId.Value, "0");

                if (Session["Contactimgpath"].ToString().Trim() != "")
                {
                    objCustSupLogo.InsertSet_CustomerSupplier_Logo(hdnContactId.Value, "0", Session["Contactimgpath"].ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
            else
            {
                DisplayMessage("Record Not Updated");
                //FillGrid();
                txtValue.Text = "";

            }
        }
        else
        {
            b = ObjContactMaster.InsertContactMaster(txtId.Text.Trim(), txtName.Text.Trim(), txtNameL.Text, txtCivilId.Text, strDepartmentId, strDesignationId, strReligionId, StrComp, strIsEmail, strIsSMS, Status.ToString(), chkIsReseller.Checked.ToString(), DefaultEmailId.ToString(), CountryCodewithMobileNumber, strNamePrefix.ToString(), hdncountryid.Value, strCurrencyId, chkVerify.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtRemark.Text, strParentContactId);

            if (b != 0)
            {
                for (int i = 0; i < dt_number.Rows.Count; i++)
                {
                    if (count == 0 && i == 0)
                    {
                        objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", b.ToString(), dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString());
                    }
                    else
                    {
                        objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", b.ToString(), dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), dt_number.Rows[i]["Is_default"].ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                    }
                }

                int Id = b;
                if (txtId.Text == ViewState["DocNo"].ToString())
                {
                    ObjContactMaster.UpdateContactMaster(Id.ToString(), txtId.Text + Id.ToString());

                    txtId.Text = txtId.Text + Id.ToString();
                }
                hdnContactId.Value = Id.ToString();


                if (Session["Contactimgpath"].ToString().Trim() != "")
                {
                    objCustSupLogo.InsertSet_CustomerSupplier_Logo(b.ToString(), "0", Session["Contactimgpath"].ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }

            }
            else
            {
                DisplayMessage("Record Not Saved");
                //FillGrid();

            }
        }
        //Insert New Enter in Address Child based  on contact Id

        bool Isdefault = false;
        foreach (GridViewRow gvr in GvAddressName.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkdefault")).Checked == true)
            {
                Isdefault = true;
                break;
            }

        }
        string AddressMailId = string.Empty;
        foreach (GridViewRow gvr in GvAddressName.Rows)
        {
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

            Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
            {
                if (lblGvAddressName.Text != "")
                {
                    DataTable dtAddId = ObjAddMaster.GetAddressDataByAddressName(lblGvAddressName.Text);
                    if (dtAddId.Rows.Count > 0)
                    {
                        string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                        string Email1 = dtAddId.Rows[0]["EmailId1"].ToString();
                        string Email2 = dtAddId.Rows[0]["EmailId2"].ToString();

                        objAddChild.InsertAddressChild(strAddressId, "Contact", hdnContactId.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True",
                            Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        if (Email1.ToString() != "")
                        {
                            ///objEmailHeader.ES_EmailMasterHeader_Insert(
                            objEmailHeader.ES_EmailMasterHeader_Insert(Email1.ToString(), hdncountryid.Value, "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }
                        if (Email2.ToString() != "")
                        {
                            objEmailHeader.ES_EmailMasterHeader_Insert(Email2.ToString(), hdncountryid.Value, "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }

                    }
                }

            }
        }
        //Contact Bank Detail 
        if (ViewState["DtContactBank"] != null)
        {
            DataTable dtbankDetbygroup = objContactBankDetail.GetContactBankDetail_By_ContactId_And_GroupId(hdnContactId.Value, "0");
            foreach (DataRow dr in dtbankDetbygroup.Rows)
            {
                //delete From the supplier bank Detail and Customer bank Detail if enter from the contact
                objContactBankDetail.DeleteContactBankDetail_By_ContactId_And_GroupId_And_BankId(hdnContactId.Value, "1", dr["Bank_Id"].ToString());
                objContactBankDetail.DeleteContactBankDetail_By_ContactId_And_GroupId_And_BankId(hdnContactId.Value, "2", dr["Bank_Id"].ToString());

            }
            //delete From the  Contact bank Detail
            objContactBankDetail.DeleteContactBankDetail_By_ContactId_And_GroupId(hdnContactId.Value, "0");


            DataTable dtContactBankDetail = (DataTable)ViewState["DtContactBank"];

            for (int i = 0; i < dtContactBankDetail.Rows.Count; i++)
            {
                DataTable DtContact = objContactBankDetail.GetContactBankDetailAll();
                DataTable DtBank = objBankMaster.GetBankMasterByBankName(dtContactBankDetail.Rows[i]["Bank_Name"].ToString());

                try
                {//insert  into  Contact bank Detail
                    DtContact = new DataView(DtContact, "Contact_Id=" + hdnContactId.Value + " and Bank_Id=" + DtBank.Rows[0]["Bank_Id"].ToString() + "and  Group_Id='0'", "", DataViewRowState.CurrentRows).ToTable();
                    if (DtContact.Rows.Count == 0)
                    {

                        objContactBankDetail.InsertContact_BankDetail(hdnContactId.Value, DtBank.Rows[0]["Bank_Id"].ToString(), dtContactBankDetail.Rows[i]["Account_No"].ToString(), dtContactBankDetail.Rows[i]["Branch_Address"].ToString(), dtContactBankDetail.Rows[i]["IFSC_Code"].ToString(), dtContactBankDetail.Rows[i]["MICR_Code"].ToString(), dtContactBankDetail.Rows[i]["Branch_Code"].ToString(), "0", dtContactBankDetail.Rows[i]["IBAN_NUMBER"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                }
                catch
                {
                    objContactBankDetail.InsertContact_BankDetail(hdnContactId.Value, DtBank.Rows[0]["Bank_Id"].ToString(), dtContactBankDetail.Rows[i]["Account_No"].ToString(), dtContactBankDetail.Rows[i]["Branch_Address"].ToString(), dtContactBankDetail.Rows[i]["IFSC_Code"].ToString(), dtContactBankDetail.Rows[i]["MICR_Code"].ToString(), dtContactBankDetail.Rows[i]["Branch_Code"].ToString(), "0", dtContactBankDetail.Rows[i]["IBAN_NUMBER"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                }
            }
        }

        // CreateDirectory(hdnContactId.Value);


        try
        {
            int g = 0;
            foreach (ListItem Item in rbnEmailList.Items)
            {
                g = objEmailHeader.ES_EmailMasterHeader_Insert(Item.Text, hdncountryid.Value, "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                string str = "False";
                if (Item.Selected == true)
                {
                    str = "True";
                }

                objEmailDetail.ES_EmailMasterDetail_Insert(hdnContactId.Value.ToString(), "Contact", g.ToString(), str.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }

        }
        catch
        {
        }
        if (ViewState["NotFillGrid"] == null)
        {
            //FillGrid();
        }
        else
        {
            if (ViewState["NotFillGrid"].ToString() == "Yes")
            {
            }
            else
            {

                // FillGrid();
            }
        }
        BindTree();
        if (RdolistSelect.SelectedValue != "Company")
        {
            // CustomValidator1.Enabled = false;
        }


        txtPnlGroupContactId.Text = txtId.Text.Trim();
        txtPnlGroupContactName.Text = txtName.Text.Trim();
        PnlGroup.Visible = true;
        PnlNewEdit.Visible = false;
        fillCompany();
        if (hdnContactId.Value != "")
        {
            DataTable dtCompanyBrand = ObjCompanyBrand.GetContactCompanyBrandAllData(hdnContactId.Value);
            foreach (ListItem li in chkCompany.Items)
            {
                try
                {
                    if ((new DataView(dtCompanyBrand, "Company_Id='" + li.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) != 0)
                    {
                        li.Selected = true;
                    }
                }
                catch
                {

                }
            }

        }
        chkCompany_SelectedIndexChanged(null, null);


    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtValue.Text = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        Reset();
        btnList_Click(null, null);
        txtValue.Text = "";

    }
    //Tree Event
    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {
        foreach (TreeNode child in parent.ChildNodes)
        {
            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;
            }

            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }
        }
    }
    protected void navTree_SelectedNodeChanged1(object sender, EventArgs e)
    {
        //try
        //{
        //    if (navTree.SelectedNode.Checked == true)
        //    {
        //        UnSelectChild(navTree.SelectedNode);

        //    }
        //    else
        //    {
        //        SelectChild(navTree.SelectedNode);
        //    }
        //}
        //catch (Exception)
        //{
        //}

    }
    protected void navTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        //if (e != null && e.Node != null)
        //{
        //    try
        //    {

        //        if (e.Node.Checked == true)
        //        {

        //            CheckTreeNodeRecursive(e.Node, true);
        //            try
        //            {
        //                SelectChild(e.Node);
        //            }
        //            catch (Exception)
        //            {
        //            }

        //        }
        //        else
        //        {
        //            CheckTreeNodeRecursive(e.Node, false);


        //            UnSelectChild(e.Node);

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
    }

    public void CheckParentNodes(TreeNode startNode)
    {

        startNode.Checked = true;

        if (startNode.Value == "1")
        {
            PnlCustomerId.Visible = true;
            PnlGSTIN.Visible = true;
        }
        if (startNode.Value == "2")
        {
            PnlSupplierId.Visible = true;
            PnlGSTIN.Visible = true;
        }
        if (startNode.Parent != null)
        {
            CheckParentNodes(startNode.Parent);
        }
    }
    //Grid event :- gvContact
    protected void BtnEdit_Command(object sender, CommandEventArgs e)
    {
        string strDepartmentId = string.Empty;
        string strDesignationId = string.Empty;
        string strReligionId = string.Empty;
        string strCurrencyId = string.Empty;
        Reset();

        btnNew_Click(null, null);

        try
        {
            if (((LinkButton)sender).ID != null && ((LinkButton)sender).ID == "btnEdit")
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
            }
        }
        catch
        {

        }

        string ConId = e.CommandArgument.ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);

        ContactNo1.setNullToGV();
        ContactNo1.FillGridData(ConId, "Ems_ContactMaster");
        addaddress.setCustomerID(ConId);

        DataTable dtCon = ObjContactMaster.GetContactTrueById(ConId);
        if (dtCon.Rows.Count != 0)
        {
            ViewState["NotFillGrid"] = "Yes";
            hdnContactId.Value = ConId;

            txtName.Text = dtCon.Rows[0]["Name"].ToString();
            txtNameL.Text = dtCon.Rows[0]["Name_L"].ToString();
            txtId.Text = dtCon.Rows[0]["Code"].ToString();
            txtCivilId.Text = dtCon.Rows[0]["Civil_Id"].ToString().Trim();
            chkIsReseller.Checked = Convert.ToBoolean(dtCon.Rows[0]["Is_Reseller"].ToString());
            strDepartmentId = dtCon.Rows[0]["Dep_Id"].ToString().Trim();
            RdolistSelect.SelectedValue = dtCon.Rows[0]["Status"].ToString();
            RdolistSelect_SelectedIndexChanged(null, null);
            txtRemark.Text = dtCon.Rows[0]["Remarks"].ToString();

            //here we get contactlogo if exist


            DataTable dtLogo = objCustSupLogo.getSet_CustomerSupplier_LogoByIdAndGroup(hdnContactId.Value, "0");

            if (dtLogo.Rows.Count > 0)
            {
                if (dtLogo.Rows[0]["ImagePath"].ToString() != "")
                {
                    try
                    {
                        string RegistrationCode = Common.Decrypt(da.get_SingleValue("Select registration_code from Application_Lic_Main"));

                        imgLogo.ImageUrl = "../CompanyResource/" + RegistrationCode + "/Contact/" + dtLogo.Rows[0]["ImagePath"].ToString();
                        Session["Contactimgpath"] = dtLogo.Rows[0]["ImagePath"].ToString();
                    }
                    catch (Exception ex)
                    {
                        imgLogo.ImageUrl = "../CompanyResource/Contact/" + dtLogo.Rows[0]["ImagePath"].ToString();
                        Session["Contactimgpath"] = dtLogo.Rows[0]["ImagePath"].ToString();
                    }
                }
            }

            // Fill GSTIN No
            string CompanyId = Session["CompId"].ToString();
            string BrandId = Session["BrandId"].ToString();
            string CustSql = "Declare @GSTIN_NO_Sup nvarchar(50); Declare @GSTIN_NO_Cust nvarchar(50) Set @GSTIN_NO_Sup = (Select GSTIN_NO from Set_Suppliers where Company_Id = " + CompanyId + " and Brand_Id = " + BrandId + " and Supplier_Id = " + ConId + " and IsActive='True') Set @GSTIN_NO_Cust =(Select GSTIN_NO from Set_CustomerMaster where Company_Id = " + CompanyId + " and Brand_Id = " + BrandId + " and Customer_Id = " + ConId + " and IsActive='True') IF ( @GSTIN_NO_Sup IS NOT NULL AND @GSTIN_NO_Sup != '') Select @GSTIN_NO_Sup As GSTIN_NO ELSE IF ( @GSTIN_NO_Cust IS NOT NULL AND @GSTIN_NO_Cust != '') Select @GSTIN_NO_Cust As GSTIN_NO ELSE Select '' As GSTIN_NO";
            string GSTIN = da.get_SingleValue(CustSql);
            if (GSTIN != "@NOTFOUND@")
                txtGSTIN.Text = GSTIN;

            // Fill TRN No            
            string Str_Trn_No = "Declare @TRN_No_Sup nvarchar(50); Declare @TRN_No_Cust nvarchar(50) Set @TRN_No_Sup = (Select TRN_No from Set_Suppliers where Company_Id = " + CompanyId + " and Brand_Id = " + BrandId + " and Supplier_Id = " + ConId + " and IsActive='True') Set @TRN_No_Cust =(Select TRN_No from Set_CustomerMaster where Company_Id = " + CompanyId + " and Brand_Id = " + BrandId + " and Customer_Id = " + ConId + " and IsActive='True') IF ( @TRN_No_Sup IS NOT NULL AND @TRN_No_Sup != '') Select @TRN_No_Sup As TRN_No ELSE IF ( @TRN_No_Cust IS NOT NULL AND @TRN_No_Cust != '') Select @TRN_No_Cust As TRN_No ELSE Select '' As TRN_No";
            string TRN_NO = da.get_SingleValue(Str_Trn_No);
            if (TRN_NO != "@NOTFOUND@")
                Txt_TRN_No.Text = TRN_NO;

            //for get parent contact id and name 
            if (dtCon.Rows[0]["ParentContactId"].ToString().Trim() != "0")
            {
                txtParentContactName.Text = dtCon.Rows[0]["ParentContactName"].ToString().Trim() + "/" + dtCon.Rows[0]["ParentContactId"].ToString().Trim();
            }
            if (dtCon.Rows[0]["Field4"].ToString().Trim() != "")
            {
                try
                {
                    txtCountryName.Text = objCountryMaster.GetCountryMasterById(dtCon.Rows[0]["Field4"].ToString()).Rows[0]["Country_Name"].ToString();
                }
                catch
                {

                }
                hdncountryid.Value = dtCon.Rows[0]["Field4"].ToString().Trim();
            }
            else
            {

            }

            if (dtCon.Rows[0]["Status"].ToString() == "Individual")
            {
                if (dtCon.Rows[0]["Field3"].ToString().Trim() != "")
                {
                    try
                    {
                        ddlNamePrefix.SelectedValue = dtCon.Rows[0]["Field3"].ToString();
                    }
                    catch
                    {

                    }
                }
                else
                {
                    ddlNamePrefix.SelectedIndex = 0;
                }
            }
            else
            {
                ddlNamePrefix.Visible = false;
            }

            try
            {
                txtCompany.Text = ObjContactMaster.GetContactTrueById(dtCon.Rows[0]["Company_Id"].ToString()).Rows[0]["Name"].ToString() + "/" + dtCon.Rows[0]["Company_Id"].ToString();
            }
            catch
            {
                txtCompany.Text = "";
            }

            if (strDepartmentId != "0" && strDepartmentId != "")
            {
                try
                {
                    ddlDepartment.SelectedValue = strDepartmentId;
                }
                catch
                {

                }
            }





            strDesignationId = dtCon.Rows[0]["Designation_Id"].ToString().Trim();
            if (strDesignationId != "0" && strDesignationId != "")
            {
                try
                {
                    ddlDesignation.SelectedValue = strDesignationId;
                }
                catch
                {

                }
            }

            strReligionId = dtCon.Rows[0]["Religion_Id"].ToString().Trim();
            if (strReligionId != "0" && strReligionId != "")
            {
                try
                {
                    ddlReligion.SelectedValue = strReligionId;
                }
                catch
                {

                }
            }

            strCurrencyId = dtCon.Rows[0]["Field5"].ToString().Trim();
            if (strCurrencyId != "0" && strCurrencyId != "")
            {
                try
                {
                    ddlCurrency.SelectedValue = strCurrencyId;
                }
                catch
                {

                }
            }

            string strIsMail = dtCon.Rows[0]["IsEmail"].ToString();
            if (strIsMail == "True")
            {
                chkIsEmail.Checked = true;
            }
            else if (strIsMail == "False")
            {
                chkIsEmail.Checked = false;
            }

            string strIsSMS = dtCon.Rows[0]["IsSMS"].ToString();
            if (strIsSMS == "True")
            {
                chkIsSMS.Checked = true;
            }
            else if (strIsSMS == "False")
            {
                chkIsSMS.Checked = false;
            }
            txtPermanentMailId.Text = dtCon.Rows[0]["Field1"].ToString();


            try
            {
                string[] mobileNumber = dtCon.Rows[0]["Field2"].ToString().Split('-');


            }
            catch
            {

            }

            //Add Address Section For Edit
            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", e.CommandArgument.ToString());

            if (dtChild.Rows.Count > 0)
            {
                GvAddressName.DataSource = dtChild;
                GvAddressName.DataBind();
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

            DataTable DtConactBankDetail = new DataTable();
            DtConactBankDetail.Columns.Add("Trans_Id");
            DtConactBankDetail.Columns.Add("Bank_Name");
            DtConactBankDetail.Columns.Add("Account_No");
            DtConactBankDetail.Columns.Add("Branch_Address");
            DtConactBankDetail.Columns.Add("IFSC_Code");
            DtConactBankDetail.Columns.Add("MICR_Code");
            DtConactBankDetail.Columns.Add("Branch_Code");
            DtConactBankDetail.Columns.Add("Group_Id");
            DtConactBankDetail.Columns.Add("IBAN_NUMBER");
            DataTable Dt = new DataView(objContactBankDetail.GetContactBankDetail_ByContactId(e.CommandArgument.ToString()), "Group_Id='0'", "", DataViewRowState.CurrentRows).ToTable();
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string BankName = string.Empty;

                DataTable DtBank = objBankMaster.GetBankMasterById(Dt.Rows[i]["Bank_Id"].ToString());

                DataRow dr = DtConactBankDetail.NewRow();
                dr["Trans_Id"] = Dt.Rows[i]["Trans_Id"].ToString();

                dr["Bank_Name"] = DtBank.Rows[0]["Bank_Name"].ToString();
                dr["Account_No"] = Dt.Rows[i]["Account_No"].ToString();
                dr["Branch_Address"] = Dt.Rows[i]["Branch_Address"].ToString();
                dr["IFSC_Code"] = Dt.Rows[i]["IFSC_Code"].ToString();
                dr["MICR_Code"] = Dt.Rows[i]["MICR_Code"].ToString();
                dr["Branch_Code"] = Dt.Rows[i]["Branch_Code"].ToString();
                dr["Group_Id"] = Dt.Rows[i]["Group_Id"].ToString();
                dr["IBAN_NUMBER"] = Dt.Rows[i]["IBAN_NUMBER"].ToString();

                DtConactBankDetail.Rows.Add(dr);
            }

            if (DtConactBankDetail.Rows.Count > 0)
            {
                GvContactBankDetail.DataSource = DtConactBankDetail;
                GvContactBankDetail.DataBind();
                ViewState["DtContactBank"] = DtConactBankDetail;
            }
            else
            {
                GvContactBankDetail.DataSource = null;
                GvContactBankDetail.DataBind();
                ViewState["DtContactBank"] = null;
            }

            DataTable dtEmailDetail = objEmailDetail.Get_EmailMasterDetailByRef_IdNRefType(e.CommandArgument.ToString(), "Contact");

            if (dtEmailDetail.Rows.Count > 0)
            {
                rbnEmailList.DataSource = dtEmailDetail;
                rbnEmailList.DataTextField = "Email_Id";
                rbnEmailList.DataValueField = "Email_Id";
                rbnEmailList.DataBind();

            }
            dtEmailDetail = objEmailDetail.Get_EmailMasterDetailByRef_IdNRefTypeDefaultTrue(e.CommandArgument.ToString(), "Contact");
            if (dtEmailDetail.Rows.Count > 0)
            {
                rbnEmailList.SelectedValue = dtEmailDetail.Rows[0]["Email_Id"].ToString();
            }
        }
        //AllPageCode();
        txtName.Focus();
        Update_New.Update();
        Update_Tab_Name.Update();
    }
    protected void imgBtnDelete_Command(object sender, CommandEventArgs e)
    {
        bool bc = ObjContactMaster.IsUsedInInventory("0", "0", e.CommandArgument.ToString(), "1", "1");
        bool bs = ObjContactMaster.IsUsedInInventory("0", "0", e.CommandArgument.ToString(), "2", "1");

        if (bc || bs)
        {
            DisplayMessage("This Contact is in use,You Can not delete");
            return;
        }
        string conid = e.CommandArgument.ToString();

        ObjContactMaster.DeleteContactMaster(conid, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        DataTable dtcompnaybrand = ObjCompanyBrand.GetContactCompanyBrandAllData(conid);
        foreach (DataRow dr in dtcompnaybrand.Rows)
        {
            ObjCustomerMaster.DeleteCustomerMasterByContactId(dr["Company_Id"].ToString(), dr["Brand_Id"].ToString(), conid.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());
            ObjSupplierMaster.DeleteSupplierMasterByContactId(dr["Company_Id"].ToString(), dr["Brand_Id"].ToString(), conid.ToString(), "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        hdnParentContactId.Value = e.CommandArgument.ToString();
        DisplayMessage("Record Deleted");

        FillGrid();

        try
        {
            ((((ImageButton)sender).Parent.Parent).FindControl("imgBtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }
        Update_List.Update();
    }
    protected void gvContact_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtFilter"];
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
        ViewState["dtFilter"] = dt;
        gvContact.DataSource = dt;
        gvContact.DataBind();


        //AllPageCode();
        gvContact.HeaderRow.Focus();
    }

    public void BindTree()
    {
        string RoleId = string.Empty;
        string moduleids = string.Empty;

        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();
        navTree.Attributes.Add("onclick", "OnTreeClick(event)");
        if (hdnContactId.Value != "")
        {
            txtCustomerId.Text = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "18", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()) + hdnContactId.Value;
            txtSupplierId.Text = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "20", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()) + hdnContactId.Value;

            DataTable dtContactGroup = objCG.GetContactGroupByContactId(hdnContactId.Value);

            DataTable DtGroupMainNode = new DataTable();

            DtGroupMainNode = new DataView(objGroup.GetGroupMasterOnlyMainNode(), "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

            foreach (DataRow datarow in DtGroupMainNode.Rows)
            {
                if ((datarow["Group_Id"].ToString() == "1" || datarow["Group_Id"].ToString() == "2") && RdolistSelect.Items[0].Selected)
                {
                    continue;
                }

                TreeNode tn = new TreeNode();

                tn = new TreeNode(datarow["Group_Name"].ToString(), datarow["Group_Id"].ToString());

                //if ((datarow["Group_Id"].ToString() == "1" || datarow["Group_Id"].ToString() == "2") && RdolistSelect.Items[0].Selected)
                //{
                //        tn.ShowCheckBox = false;
                //}


                DataTable dtModuleSaved = new DataView(dtContactGroup, "Group_Id='" + datarow["Group_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtModuleSaved.Rows.Count > 0)
                {
                    tn.Checked = true;
                    if (datarow["Group_Id"].ToString() == "1")
                    {

                        PnlCustomerId.Visible = true;
                        PnlGSTIN.Visible = true;
                        if (dtModuleSaved.Rows[0]["Field1"].ToString() != "")
                        {
                            txtCustomerId.Text = dtModuleSaved.Rows[0]["Field1"].ToString();

                            txtcustomerTinNo.Text = dtModuleSaved.Rows[0]["Field2"].ToString();
                            txtcustomerCstNo.Text = dtModuleSaved.Rows[0]["Field3"].ToString();

                        }


                    }
                    if (datarow["Group_Id"].ToString() == "2")
                    {
                        PnlSupplierId.Visible = true;
                        PnlGSTIN.Visible = true;
                        if (dtModuleSaved.Rows[0]["Field1"].ToString() != "")
                        {
                            txtSupplierId.Text = dtModuleSaved.Rows[0]["Field1"].ToString();
                            txtsupplierTinNo.Text = dtModuleSaved.Rows[0]["Field2"].ToString();
                            txtsupplierCstNo.Text = dtModuleSaved.Rows[0]["Field3"].ToString();

                        }
                    }
                }
                tn.SelectAction = TreeNodeSelectAction.Expand;
                FillChild(dtContactGroup, tn.Value, tn);
                navTree.Nodes.Add(tn);
            }
        }
        navTree.DataBind();
        navTree.CollapseAll();
        return;
    }
    private void FillChild(DataTable dtContactGroup, string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = new DataView(objGroup.GetGroupMasterByParentId(index), "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();


        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Group_Name"].ToString();
            tn1.Value = dt.Rows[i]["Group_Id"].ToString();
            tn1.SelectAction = TreeNodeSelectAction.Expand;
            tn.ChildNodes.Add(tn1);
            foreach (DataRow Dr in dtContactGroup.Rows)
            {
                if (dt.Rows[i]["Group_Id"].ToString() == Dr["Group_Id"].ToString())
                {
                    tn1.Checked = true;
                }
            }

            FillChild(dtContactGroup, (dt.Rows[i]["Group_Id"].ToString()), tn1);
            i++;
        }
        navTree.DataBind();
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnsave.Visible = btnGroupNext.Visible = imgAddAddressName.Visible = btnAddNewAddress.Visible = BtnContactBank.Visible = btnAddEmail.Visible = clsPagePermission.bAdd;
        btnRemoveEmail.Visible = clsPagePermission.bDelete;
        chkVerify.Visible = chkVerify.Checked = Li_Verify_Request.Visible = clsPagePermission.bVerify;
        GvAddressName.Columns[0].Visible = clsPagePermission.bEdit;
        GvAddressName.Columns[1].Visible = clsPagePermission.bDelete;
        GvContactBankDetail.Columns[0].Visible = clsPagePermission.bEdit;
        GvContactBankDetail.Columns[1].Visible = clsPagePermission.bDelete;

        //gvContact.Columns[0].Visible = true;
        //gvContact.Columns[2].Visible = clsPagePermission.bEdit;
        //gvContact.Columns[3].Visible = clsPagePermission.bDelete;
        //gvContact.Columns[1].Visible = clsPagePermission.bView;

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();



        imgBtnRestore.Visible = clsPagePermission.bRestore;
        ImgbtnSelectAll.Visible = clsPagePermission.bRestore;
        btnExport.Visible = clsPagePermission.bDownload;
        gvContact.Columns[4].Visible = clsPagePermission.bRestore;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    #endregion
    private void SelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = true;
        if (treeNode.Value == "1")
        {
            PnlCustomerId.Visible = true;
            PnlGSTIN.Visible = true;
        }
        if (treeNode.Value == "2")
        {
            PnlSupplierId.Visible = true;
            PnlGSTIN.Visible = true;
        }
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = true;

            SelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        try
        {
            CheckParentNodes(treeNode.Parent);

        }
        catch
        {

        }

        navTree.DataBind();


    }
    private void UnSelectChild(TreeNode treeNode)
    {
        int i = 0;

        treeNode.Checked = false;
        bool b = false;
        string value = treeNode.Value;
        if (treeNode.Value == "1")
        {
            PnlCustomerId.Visible = false;
            PnlGSTIN.Visible = true;

            try
            {
                //here we check that this customer is already used or not
                b = ObjContactMaster.IsUsedInInventory("0", "0", hdnContactId.Value.ToString(), "1", "1");

            }
            catch
            {

            }
        }

        if (treeNode.Value == "2")
        {
            PnlSupplierId.Visible = false;
            PnlGSTIN.Visible = true;

            try
            {
                b = ObjContactMaster.IsUsedInInventory("0", "0", hdnContactId.Value.ToString(), "2", "1");

            }
            catch
            {

            }
        }

        if (b)
        {
            DisplayMessage("This contact is in use,You can not remove from this group");
            SelectChild(treeNode);
            return;

        }


        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = false;


            UnSelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        navTree.DataBind();


    }


    void selectContact(TreeNode treenode)
    {
        treenode.Checked = true;
    }
    protected void chkCompany_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dtbrand = new DataTable(); ;
        foreach (ListItem lst in chkCompany.Items)
        {
            if (lst.Selected)
            {
                dtbrand.Merge(ObjBrandMaster.GetBrandMaster(lst.Value.ToString()));
            }

        }
        if (dtbrand != null && dtbrand.Rows.Count > 0)
        {
            dtbrand = new DataView(dtbrand, "", "Brand_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        chkBrand.DataSource = dtbrand;
        chkBrand.DataTextField = "Brand_Name";
        chkBrand.DataValueField = "Brand_Id";
        chkBrand.DataBind();

        if (hdnContactId.Value != "")
        {
            DataTable dtCompanyBrand = ObjCompanyBrand.GetContactCompanyBrandAllData(hdnContactId.Value);

            foreach (ListItem lstCompany in chkCompany.Items)
            {
                DataTable dtCompTemp = new DataView(dtCompanyBrand, "Company_Id='" + lstCompany.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCompTemp.Rows.Count == 0)
                {
                    if (lstCompany.Selected == true)
                    {
                        DataTable dtbrandTemp = ObjBrandMaster.GetBrandMaster(lstCompany.Value.ToString());
                        foreach (ListItem lstBrand in chkBrand.Items)
                        {
                            if ((new DataView(dtbrandTemp, "Brand_Id='" + lstBrand.Value + "'", "", DataViewRowState.CurrentRows)).ToTable().Rows.Count != 0)
                            {
                                lstBrand.Selected = true;
                            }
                        }

                    }
                }
                foreach (ListItem lstBrand in chkBrand.Items)
                {
                    try
                    {
                        DataTable dtTemp = new DataView(dtCompanyBrand, "Company_Id='" + lstCompany.Value + "' and Brand_Id='" + lstBrand.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTemp.Rows.Count != 0)
                        {
                            DataTable dtContactGroup = objCG.GetContactGroupByContactId(hdnContactId.Value);
                            bool bc = false;
                            bool bs = false;
                            if ((new DataView(dtContactGroup, "Group_Id='1'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) != 0)
                            {
                                try
                                {
                                    //here we check that this customer is already used or not
                                    bc = ObjContactMaster.IsUsedInInventory(dtTemp.Rows[0]["Company_Id"].ToString(), dtTemp.Rows[0]["Brand_Id"].ToString(), hdnContactId.Value.ToString(), "1", "2");
                                    if (bc)
                                    {
                                        txtCustomerId.Enabled = false;
                                    }
                                }
                                catch
                                {


                                }
                            }
                            if ((new DataView(dtContactGroup, "Group_Id='2'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) != 0)
                            {   //here we check that this Supplier is already used or not
                                try
                                {

                                    bs = ObjContactMaster.IsUsedInInventory(dtTemp.Rows[0]["Company_Id"].ToString(), dtTemp.Rows[0]["Brand_Id"].ToString(), hdnContactId.Value.ToString(), "2", "2");
                                    if (bs)
                                    {
                                        txtSupplierId.Enabled = false;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            lstBrand.Selected = true;
                            if (bc || bs)
                            {
                                lstBrand.Enabled = false;
                                lstCompany.Enabled = false;

                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
    public void fillCompany()
    {
        DataTable dt = ObjCompMaster.GetCompanyMaster();
        dt = new DataView(dt, "", "Company_Name aSc", DataViewRowState.CurrentRows).ToTable();
        chkCompany.DataSource = dt;
        chkCompany.DataTextField = "Company_Name";
        chkCompany.DataValueField = "Company_Id";
        chkCompany.DataBind();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        string condition = string.Empty;
        string SearchCountry = string.Empty;
        string SearchGroup = string.Empty;
        string searchGroupId = "";

        if (!String.IsNullOrEmpty(txtCountryFilter.Text))
            SearchCountry = "CM.Field4 = '" + GetCountryIdbyName(txtCountryFilter.Text) + "'";

        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
            SearchGroup = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();

        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
            searchGroupId = ddlGroupSearch.SelectedValue.ToString();

        if (SearchCountry.Trim() != "")
        {
            condition = SearchCountry;
        }
        if (SearchGroup.Trim() != "" && SearchCountry.Trim() != "")
        {
            condition += " and ";
        }
        if (SearchGroup.Trim() != "")
        {
            condition += SearchGroup;
        }

        string PageSize = PageControlCommon.GetPageSize().ToString();
        string BatchNo = "1";
        string Usr_ID_Filter = "and Fav.User_Id = '" + Session["UserId"].ToString() + "'";
        DataTable dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, BatchNo, PageSize, true.ToString(), "false", "", "", "", searchGroupId, "");
        DataTable dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, BatchNo, PageSize, false.ToString(), "false", "", "", "", searchGroupId, Usr_ID_Filter);

        gvContact.DataSource = dt;
        gvContact.DataBind();
        ViewState["dtFilter"] = dt;
        generatePager(int.Parse(dtRecord.Rows[0][0].ToString()), int.Parse(PageSize), 1);

        lblTotalRecordNumber.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString();

        //AllPageCode();
    }

    public string GetCountryIdbyName(string strCountryName)
    {
        string strCountryId = string.Empty;
        DataTable dt = da.return_DataTable("select Country_Id from Sys_CountryMaster where Country_Name = '" + strCountryName + "'");
        if (dt.Rows.Count > 0)
        {
            strCountryId = dt.Rows[0]["Country_Id"].ToString();
        }
        else
        {
            strCountryId = "0";
        }
        return strCountryId;
    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGroup();
        ddlGroupSearch.SelectedIndex = 0;
        ddlGroupSearch.Focus();
        btnRefresh_Click(null, null);
        txtCountryFilter.Text = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Name"].ToString();
        FillGrid();
        ddlMyFav.SelectedIndex = 0;
    }


    #region Add New Address Concept

    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        //pnlAddress1.Visible = false;
        //pnlAddress2.Visible = false;
    }
    //
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]


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


    public void ResetAddressName()
    {
        txtAddressName.Text = "";
        hdnAddressId.Value = "";
        hdnAddressName.Value = "";
    }
    public DataTable CreateAddressDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("FullAddress");
        dt.Columns.Add("Latitude");
        dt.Columns.Add("Longitude");
        dt.Columns.Add("PhoneNo");
        dt.Columns.Add("Add_Type");
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
                    Label lblLongitude = (Label)GvAddressName.Rows[i].FindControl("lblgvLogitude");
                    Label lblLatitude = (Label)GvAddressName.Rows[i].FindControl("lblgvLatitude");

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
                    try
                    {
                        dt.Rows[i]["PhoneNo"] = GetContactPhoneNo(lblAddressName.Text);
                    }
                    catch
                    {
                    }
                    try
                    {
                        dt.Rows[i]["Longitude"] = GetContactLongitude(lblAddressName.Text);
                    }
                    catch
                    {
                    }
                    try
                    {
                        dt.Rows[i]["Latitude"] = GetContactLatitude(lblAddressName.Text);
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
                    try
                    {
                        dt.Rows[i]["PhoneNo"] = GetContactPhoneNo(txtAddressName.Text);
                    }
                    catch
                    {
                    }
                    try
                    {
                        dt.Rows[i]["Longitude"] = GetContactLongitude(txtAddressName.Text);
                    }
                    catch
                    {
                    }
                    try
                    {
                        dt.Rows[i]["Latitude"] = GetContactLatitude(txtAddressName.Text);
                    }
                    catch
                    {
                    }
                    dt.Rows[i]["Add_Type"] = "Contact";
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
            try
            {
                dt.Rows[0]["PhoneNo"] = GetContactPhoneNo(txtAddressName.Text);
            }
            catch
            {
            }
            try
            {
                dt.Rows[0]["Longitude"] = GetContactLongitude(txtAddressName.Text);
            }
            catch
            {
            }
            try
            {
                dt.Rows[0]["Latitude"] = GetContactLatitude(txtAddressName.Text);
            }
            catch
            {
            }
            dt.Rows[0]["Add_Type"] = "Contact";
            dt.Rows[0]["Is_Default"] = false.ToString();
        }
        if (dt.Rows.Count > 0)
        {
            GvAddressName.DataSource = dt;
            GvAddressName.DataBind();
            //AllPageCode();
        }
        return dt;

    }
    #endregion

    public void FillGrid()
    {
        string condition = string.Empty;
        string SearchCountry = string.Empty;
        string SearchGroup = string.Empty;
        string SearchGroupId = string.Empty;

        if (!String.IsNullOrEmpty(txtCountryFilter.Text))
            SearchCountry = "CM.Field4 = '" + GetCountryIdbyName(txtCountryFilter.Text) + "'";

        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
            SearchGroup = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();

        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
            SearchGroupId = ddlGroupSearch.SelectedValue.ToString();

        if (SearchCountry.Trim() != "")
        {
            condition = SearchCountry;
        }
        if (SearchGroup.Trim() != "" && SearchCountry.Trim() != "")
        {
            condition += " and ";
        }
        if (SearchGroup.Trim() != "")
        {
            condition += SearchGroup;
        }


        string PageSize = PageControlCommon.GetPageSize().ToString();
        string BatchNo = "1";
        string Usr_ID_Filter = "and Fav.User_Id = '" + Session["UserId"].ToString() + "'";
        using (DataTable dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, BatchNo, PageSize, true.ToString(), "false", "", "", "", SearchGroupId, ""))
        {
            using (DataTable dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, BatchNo, PageSize, false.ToString(), "false", "", "", "", SearchGroupId, Usr_ID_Filter))
            {
                gvContact.DataSource = dt;
                gvContact.DataBind();
                ViewState["dtFilter"] = dt;
            }
            generatePager(int.Parse(dtRecord.Rows[0][0].ToString()), int.Parse(PageSize), 1);
            lblTotalRecordNumber.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString();
        }
    }
    public void FillGridBin()
    {
        DataTable dtContact = ObjContactMaster.GetContactFalseAllData();
        if (dtContact.Rows.Count > 0)
        {
            GvContactBin.DataSource = dtContact;
            GvContactBin.DataBind();
            foreach (GridViewRow gvrbin in GvContactBin.Rows)
            {
                Label lblPreBin = (Label)gvrbin.FindControl("lblPrefixBin");

                if (lblPreBin.Text == "0")
                {
                    lblPreBin.Text = "";
                }
            }
            //AllPageCode();
        }
        else
        {
            GvContactBin.DataSource = null;
            GvContactBin.DataBind();
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count.ToString();

        Session["dtContactBin"] = dtContact;
        Session["dtFilterBin"] = dtContact;
        dtContact = null;
    }

    private void FillDepartment()
    {
        DataTable dsDepartment = null;
        dsDepartment = ObjDepMaster.GetDepartmentMaster();
        dsDepartment = new DataView(dsDepartment, "", "Dep_Name Asc", DataViewRowState.CurrentRows).ToTable(true, "Dep_Name", "Dep_Id");
        if (dsDepartment.Rows.Count > 0)
        {
            ddlDepartment.DataSource = dsDepartment;
            ddlDepartment.DataTextField = "Dep_Name";
            ddlDepartment.DataValueField = "Dep_Id";
            ddlDepartment.DataBind();

            ddlDepartment.Items.Add("--Select--");
            ddlDepartment.SelectedValue = "--Select--";
        }
        else
        {
            ddlDepartment.Items.Add("--Select--");
            ddlDepartment.SelectedValue = "--Select--";
        }
        dsDepartment = null;
    }
    private void FillDesignation()
    {
        DataTable dsDesignation = null;
        dsDesignation = ObjDesMaster.GetDesignationMaster();
        dsDesignation = new DataView(dsDesignation, "", "Designation asc", DataViewRowState.CurrentRows).ToTable();
        if (dsDesignation.Rows.Count > 0)
        {
            ddlDesignation.DataSource = dsDesignation;
            ddlDesignation.DataTextField = "Designation";
            ddlDesignation.DataValueField = "Designation_Id";
            ddlDesignation.DataBind();

            ddlDesignation.Items.Add("--Select--");
            ddlDesignation.SelectedValue = "--Select--";
        }
        else
        {
            ddlDesignation.Items.Add("--Select--");
            ddlDesignation.SelectedValue = "--Select--";
        }
    }
    private void FillReligion()
    {
        DataTable dsReligion = null;
        try
        {
            dsReligion = ObjRelMaster.GetReligionMaster();
            dsReligion = new DataView(dsReligion, "", "Religion asc", DataViewRowState.CurrentRows).ToTable();
            if (dsReligion.Rows.Count > 0)
            {
                ddlReligion.DataSource = dsReligion;
                ddlReligion.DataTextField = "Religion";
                ddlReligion.DataValueField = "Religion_Id";
                ddlReligion.DataBind();

                ddlReligion.Items.Add("--Select--");
                ddlReligion.SelectedValue = "--Select--";
            }
            else
            {
                ddlReligion.Items.Add("--Select--");
                ddlReligion.SelectedValue = "--Select--";
            }
        }
        catch
        {

        }
    }
    public void Reset()
    {
        RdolistSelect.SelectedValue = "Individual";
        txtId.Text = "";
        txtName.Text = "";
        txtNameL.Text = "";
        txtPermanentMailId.Text = "";
        //txtPermanentMobileNo.Text = "";
        txtContactBankName.Text = "";
        txtAddressName.Text = "";
        txtCivilId.Text = "";
        GvContactBankDetail.DataSource = null;
        GvContactBankDetail.DataBind();
        GvAddressName.DataSource = null;
        GvAddressName.DataBind();
        FillDepartment();
        FillDesignation();
        FillReligion();
        ViewState["DtContactBank"] = null;
        txtCompany.Text = "";
        chkIsEmail.Checked = false;
        chkIsSMS.Checked = false;
        fillCompany();
        BindTree();
        chkCompany_SelectedIndexChanged(null, null);
        txtCustomerId.Text = "";
        txtSupplierId.Text = "";
        PnlCustomerId.Visible = false;
        PnlSupplierId.Visible = false;
        PnlGSTIN.Visible = true;

        hdnContactId.Value = "";
        PnlNewEdit.Visible = true;
        PnlGroup.Visible = false;
        txtSupplierId.Enabled = true;
        txtCustomerId.Enabled = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;

        RdolistSelect_SelectedIndexChanged(null, null);
        txtId.Text = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        ViewState["DocNo"] = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        chkIsReseller.Checked = false;
        ddlNamePrefix.SelectedIndex = 0;
        txtValue.Text = "";
        rbnEmailList.Items.Clear();
        rbnEmailList.SelectedIndex = -1;
        txtCountryName.Text = "";

        ContactNo1.setNullToGV();
        addaddress.setCustomerID("NewCust");

        try
        {

            //string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            string strCurrencyId = Session["LocCurrencyId"].ToString();
            ddlCurrency.SelectedValue = strCurrencyId;

            hdncountryid.Value = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
            txtCountryName.Text = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Name"].ToString();
        }
        catch
        {
        }

        //btnRefresh_Click(null, null);
        if (ViewState["CountryCode"] != null)
        {
            try
            {
                //ddlCountryCode.SelectedValue = "+" + ViewState["CountryCode"].ToString();
                ContactNo1.setCountryCode("+" + ViewState["CountryCode"].ToString());
            }
            catch
            {
            }
        }

        txtcustomerTinNo.Text = "";
        txtcustomerCstNo.Text = "";
        txtsupplierTinNo.Text = "";
        txtsupplierCstNo.Text = "";
        chkIsEmail.Checked = true;
        chkIsSMS.Checked = true;
        txtRemark.Text = "";
        txtParentContactName.Text = "";
        imgLogo.ImageUrl = getImageUrl("");
        Session["Contactimgpath"] = null;
        txtGSTIN.Text = string.Empty;
        Txt_TRN_No.Text = string.Empty;
    }

    public string getImageUrl(string ImageName)
    {
        string url = string.Empty;
        if (File.Exists(Server.MapPath("../CompanyResource/Contact/" + ImageName)) == true)
        {
            url = "../CompanyResource/Contact/" + ImageName;
        }
        else
        {
            url = "../Bootstrap_Files/dist/img/NoImage.jpg";
        }
        return url;
    }

    public void DisplayMessage(string Message, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + Message + "','" + color + "','white');", true); ;
    }
    public string GetAddressByAddressName(string AddressName)
    {
        string Address = string.Empty;
        DataTable dt = ObjAddMaster.GetAddressDataByAddressName(AddressName);

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
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {

            if (DtAddress.Rows[0]["EmailId1"].ToString() != "")
            {
                ContactEmailId = DtAddress.Rows[0]["EmailId1"].ToString();
            }
            if (DtAddress.Rows[0]["EmailId2"].ToString() != "")
            {
                ContactEmailId = ContactEmailId + "," + DtAddress.Rows[0]["EmailId2"].ToString();
            }
        }
        return ContactEmailId;
    }
    public string GetContactFaxNo(string AddressName)
    {
        string ContactFaxNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(AddressName);
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
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(AddressName);


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

        }
        return ContactPhoneNo;
    }
    public string GetContactMobileNo(string AddressName)
    {
        string ContactPhoneNo = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(AddressName);
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
        }
        return ContactPhoneNo;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactId(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjContactMaster.GetContactTrueAutoCompleteById(prefixText);
        if (dt.Rows.Count == 0)
        {

            //dt = ObjContactMaster.GetContactAllData();
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Code"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListComapnyName(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        //DataTable dt = new DataView(ObjContactMaster.GetAutoCompleteContactTrueAllData(prefixText), "Status='Company'", "Name Asc", DataViewRowState.CurrentRows).ToTable();
        DataTable dt = ObjContactMaster.GetCustomerAsPerFilterText(prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name_id"].ToString();// + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        dt = null;
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionContactList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjContactMaster.GetAutoCompleteContactTrueAllData(prefixText);

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        return txt;
    }


    protected void txtId_TextChanged(object sender, EventArgs e)
    {
        if (txtId.Text != "")
        {

            DataTable dtContact = new DataView(ObjContactMaster.GetContactAllData(), "Code='" + txtId.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtContact.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtContact.Rows[0]["IsActive"].ToString()))
                {

                    CommandEventArgs editE = new CommandEventArgs("", dtContact.Rows[0]["Trans_Id"].ToString());
                    BtnEdit_Command(null, editE);
                }
                else
                {

                    DisplayMessage("This Id is Already exists :- Go to Bin");

                    txtId.Text = "";
                    txtId.Focus();
                    return;
                }


            }
            else
            {
                string s = txtId.Text;
                //  Reset();
                txtId.Text = s;
                txtName.Focus();


            }
        }
        else
        {
            DisplayMessage("Enter Code");
            txtId.Focus();
            return;
        }
    }
    protected void txtCompany_TextChanged(object sender, EventArgs e)
    {
        if (txtCompany.Text != "")
        {
            DataTable dtContact = new DataView(ObjContactMaster.GetContactTrueAllData(), "Name='" + txtCompany.Text.Split('/')[0].Trim() + "'and Status='Company'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtContact.Rows.Count == 0)
            {
                DisplayMessage("Choose In Suggestions Only");

                txtCompany.Text = "";
                txtCompany.Focus();
                return;

            }

        }

    }
    protected void RdolistSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdolistSelect.SelectedValue == "Individual")
        {
            TbRefContact.Visible = true;
            ddlNamePrefix.Visible = true;
            lblCompanyName.Visible = true;
            txtCompany.Visible = true;
            txtParentContactName.Visible = false;
            Label26.Visible = false;
            //txtName.Width = 350;
        }
        if (RdolistSelect.SelectedValue == "Company")
        {
            TbRefContact.Visible = false;
            ddlNamePrefix.Visible = false;
            //txtName.Width = 412;
            lblCompanyName.Visible = false;
            txtCompany.Visible = false;
            txtParentContactName.Visible = true;
            Label26.Visible = true;
        }

    }



    #region ContactBankDetail
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMaster(), "Bank_Name like '%" + prefixText.ToString() + "%'", "Bank_Name Asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Bank_Name"].ToString();
        }
        return txt;
    }
    protected void txtContactBankName_TextChanged(object sender, EventArgs e)
    {
        if (txtContactBankName.Text != "")
        {
            DataTable dtBank = objBankMaster.GetBankMasterByBankName(txtContactBankName.Text);
            if (dtBank.Rows.Count == 0)
            {

                txtContactBankName.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactBankName);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Bank Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactBankName);
        }
    }
    protected void txtBankName_TextChanged(object sender, EventArgs e)
    {
        if (txtBankName.Text != "")
        {
            DataTable dtBank = objBankMaster.GetBankMasterByBankName(txtBankName.Text);
            if (dtBank.Rows.Count == 0)
            {

                txtBankName.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBankName);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Bank Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactBankName);
        }
    }
    protected void BtnContactBank_click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Bank_Details()", true);

        ResetContactBankPanel();
        txtBankName.Text = txtContactBankName.Text;
        txtContactBankName.Text = "";
        if (txtBankName.Text == "")
        {
            txtBankName.Focus();
        }
        else
        {
            txtCBAccountNo.Focus();
        }

        //foreach (GridViewRow gvrow in GvContactBankDetail.Rows)
        //{
        //    Label lblgvBankId = (Label)gvrow.FindControl("lblgvBankId");

        //}
    }
    protected void btnCBSave_Click(object sender, EventArgs e)
    {
        if (txtBankName.Text == "")
        {
            DisplayMessage("Enter Bank Name");
            txtBankName.Focus();
            return;

        }

        DataTable dt = new DataTable();
        if (hdnContactBankId.Value == "")
        {



            if (ViewState["DtContactBank"] == null)
            {

                dt.Columns.Add("Trans_Id");


                dt.Columns.Add("Bank_Name");
                dt.Columns.Add("Account_No");
                dt.Columns.Add("Branch_Address");
                dt.Columns.Add("IFSC_Code");
                dt.Columns.Add("MICR_Code");
                dt.Columns.Add("Branch_Code");
                dt.Columns.Add("Group_Id");
                dt.Columns.Add("IBAN_NUMBER");
                DataRow dr = dt.NewRow();
                dr["Trans_Id"] = "1";

                dr["Bank_Name"] = txtBankName.Text;
                dr["Account_No"] = txtCBAccountNo.Text;
                dr["Branch_Address"] = txtCBBrachAddress.Text;
                dr["IFSC_Code"] = txtCBIFSCCode.Text;
                dr["MICR_Code"] = txtCBMICRCode.Text;
                dr["Branch_Code"] = txtCBBranchCode.Text;
                dr["Group_Id"] = "0";
                dr["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = (DataTable)ViewState["DtContactBank"];
                DataRow dr = dt.NewRow();
                int TransId = 1;

                DataTable DtTransid = dt.Copy();
                DtTransid = new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable(true, "Trans_Id");
                if (DtTransid.Rows.Count > 0)
                {
                    TransId = Convert.ToInt32(DtTransid.Rows[0]["Trans_Id"].ToString());
                    TransId = TransId + 1;
                }

                dr["Trans_Id"] = TransId.ToString();

                dr["Bank_Name"] = txtBankName.Text;
                dr["Account_No"] = txtCBAccountNo.Text;
                dr["Branch_Address"] = txtCBBrachAddress.Text;
                dr["IFSC_Code"] = txtCBIFSCCode.Text;
                dr["MICR_Code"] = txtCBMICRCode.Text;
                dr["Branch_Code"] = txtCBBranchCode.Text;
                dr["Group_Id"] = "0";
                dr["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                dt.Rows.Add(dr);

            }

            ViewState["DtContactBank"] = dt;
            GvContactBankDetail.DataSource = dt;
            GvContactBankDetail.DataBind();
            //AllPageCode();
            DisplayMessage("Record saved", "green");


        }
        else
        {
            dt = (DataTable)ViewState["DtContactBank"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Trans_id"].ToString() == hdnContactBankId.Value)
                {
                    dt.Rows[i]["Bank_Name"] = txtBankName.Text;
                    dt.Rows[i]["Account_No"] = txtCBAccountNo.Text;
                    dt.Rows[i]["Branch_Address"] = txtCBBrachAddress.Text;
                    dt.Rows[i]["IFSC_Code"] = txtCBIFSCCode.Text;
                    dt.Rows[i]["MICR_Code"] = txtCBMICRCode.Text;
                    dt.Rows[i]["Branch_Code"] = txtCBBranchCode.Text;
                    dt.Rows[i]["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                }
            }
            GvContactBankDetail.DataSource = dt;
            GvContactBankDetail.DataBind();
            ViewState["DtContactBank"] = dt;
            ResetContactBankPanel();

            //AllPageCode();
        }
        hdnContactBankId.Value = "";
        ResetContactBankPanel();
        txtBankName.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Bank_Details()", true);
    }
    protected void imgBtnContactBankEdit_Command(object sender, CommandEventArgs e)
    {
        txtBankName.ReadOnly = true;
        hdnContactBankId.Value = e.CommandArgument.ToString();
        DataTable dt = (DataTable)ViewState["DtContactBank"];
        try
        {
            dt = new DataView(dt, "Trans_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        txtBankName.Text = dt.Rows[0]["Bank_Name"].ToString();
        txtCBAccountNo.Text = dt.Rows[0]["Account_No"].ToString();
        txtCBBrachAddress.Text = dt.Rows[0]["Branch_Address"].ToString();
        txtCBIFSCCode.Text = dt.Rows[0]["IFSC_Code"].ToString();
        txtCBMICRCode.Text = dt.Rows[0]["MICR_Code"].ToString();
        txtCBBranchCode.Text = dt.Rows[0]["Branch_Code"].ToString();
        txtIBANNUMBER.Text = dt.Rows[0]["IBAN_NUMBER"].ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Bank_Details()", true);
        txtCBAccountNo.Focus();

    }
    protected void imgBtnContactBankDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["DtContactBank"];
        try
        {
            dt = new DataView(dt, "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        ViewState["DtContactBank"] = dt;
        GvContactBankDetail.DataSource = dt;
        GvContactBankDetail.DataBind();
        //AllPageCode();

    }
    protected void BtnCBCancel_Click(object sender, EventArgs e)
    {
        ResetContactBankPanel();
        txtBankName.Focus();
        hdnContactBankId.Value = "";

    }
    protected void mgContactBankClose_Click(object sender, ImageClickEventArgs e)
    {
        ResetContactBankPanel();

        hdnContactBankId.Value = "";

    }
    void ResetContactBankPanel()
    {
        txtBankName.Text = "";
        txtCBAccountNo.Text = "";
        txtCBBrachAddress.Text = "";
        txtCBBranchCode.Text = "";
        txtCBIFSCCode.Text = "";
        txtCBMICRCode.Text = "";
        txtIBANNUMBER.Text = "";
        txtBankName.ReadOnly = false;

    }


    #endregion

    protected void btnGroupNext_Click(object serder, EventArgs e)
    {

        bool IsBlock = false;
        string strBlockReason = string.Empty;
        string IsCredit = "False";
        string Strstatus = "Approved";

        //gst no validation
        if (!string.IsNullOrEmpty(txtGSTIN.Text.Trim()))
        {
            if (!cmn.IsValidateGstNo(txtGSTIN.Text.Trim()))
            {
                DisplayMessage("Please enter valida gst no");
                return;
            }
        }

        if (RdolistSelect.SelectedValue == "Individual" && navTree.Nodes.Count == 0)
        {
            DisplayMessage("Please first create a contact group in Master Setup");
            return;
        }

        DataTable dtCustomerInfo = ObjCustomerMaster.GetCustomerAllDataByCustomerIdWithOutBrand(Session["CompId"].ToString(), hdnContactId.Value);
        if (dtCustomerInfo.Rows.Count > 0)
        {
            IsBlock = Convert.ToBoolean(dtCustomerInfo.Rows[0]["Is_Block"].ToString());
            strBlockReason = dtCustomerInfo.Rows[0]["Block_Reason"].ToString();
            IsCredit = dtCustomerInfo.Rows[0]["Field4"].ToString();
            Strstatus = dtCustomerInfo.Rows[0]["Field5"].ToString();
        }

        DataTable dtSupplierInfo = ObjSupplierMaster.GetSupplierAllDataBySupplierIdWithoutBrand(Session["CompId"].ToString(), hdnContactId.Value);
        if (dtCustomerInfo.Rows.Count > 0)
        {
            IsCredit = dtCustomerInfo.Rows[0]["Field4"].ToString();
            Strstatus = dtCustomerInfo.Rows[0]["Field5"].ToString();
        }

        string Id = string.Empty;
        bool IsAllow = false;
        bool IsAllowGroup = false;
        bool IsCustomer = false;
        bool IsSupplier = false;

        bool Isselectgroup = false;



        foreach (TreeNode ModuleNode in navTree.Nodes)
        {
            if (ModuleNode.Checked)
            {
                Isselectgroup = true;
                if (ModuleNode.Value == "1" || ModuleNode.Value == "2")
                {
                    if (ModuleNode.Value == "1")
                    {
                        IsCustomer = true;
                    }
                    if (ModuleNode.Value == "2")
                    {
                        IsSupplier = true;
                    }

                    IsAllowGroup = true;
                    foreach (ListItem li in chkCompany.Items)
                    {
                        if (li.Selected)
                        {
                            IsAllow = true;
                        }
                    }

                }
            }


        }


        //for Customer & Supplier Account
        string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strPaymentVoucherAcc = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

        if (IsAllowGroup)
        {
            if (!IsAllow)
            {
                DisplayMessage("Select atleast one company ");
                return;
            }
            if (PnlCustomerId.Visible == true)
            {
                if (txtCustomerId.Text == "")
                {
                    DisplayMessage("Enter customer id");
                    return;
                }
            }
            if (PnlSupplierId.Visible == true)
            {
                if (txtSupplierId.Text == "")
                {
                    DisplayMessage("Enter supplier id");
                    return;
                }
            }


        }
        else
        {
            if (!Isselectgroup)
            {
                DisplayMessage("Please select atleast one group");
                return;

            }
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        foreach (TreeNode ModuleNode in navTree.Nodes)
        {
            if (ModuleNode.Checked)
            {
                if (ModuleNode.Value == "1" || ModuleNode.Value == "2")
                {
                    string strType = string.Empty;


                    if (ViewState["DtContactBank"] != null)
                    {
                        DataTable dtContactBankDetail = (DataTable)ViewState["DtContactBank"];
                        for (int i = 0; i < dtContactBankDetail.Rows.Count; i++)
                        {
                            DataTable DtContact = objContactBankDetail.GetContactBankDetailAll();
                            DataTable DtBank = objBankMaster.GetBankMasterByBankName(dtContactBankDetail.Rows[i]["Bank_Name"].ToString());
                            try
                            {
                                DtContact = new DataView(DtContact, "Contact_Id=" + hdnContactId.Value + " and Bank_Id=" + DtBank.Rows[0]["Bank_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                                if (DtContact.Rows.Count != 0)
                                {
                                    try
                                    {
                                        if (ModuleNode.Value == "1")
                                        {
                                            strType = "Customer";

                                            if (new DataView(DtContact, "Group_Id='" + ModuleNode.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                                            {
                                                objContactBankDetail.InsertContact_BankDetail(hdnContactId.Value, DtBank.Rows[0]["Bank_Id"].ToString(), dtContactBankDetail.Rows[i]["Account_No"].ToString(), dtContactBankDetail.Rows[i]["Branch_Address"].ToString(), dtContactBankDetail.Rows[i]["IFSC_Code"].ToString(), dtContactBankDetail.Rows[i]["MICR_Code"].ToString(), dtContactBankDetail.Rows[i]["Branch_Code"].ToString(), ModuleNode.Value, dtContactBankDetail.Rows[i]["IBAN_NUMBER"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                        if (ModuleNode.Value == "2")
                                        {
                                            strType = "Supplier";

                                            if (new DataView(DtContact, "Group_Id='" + ModuleNode.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                                            {
                                                objContactBankDetail.InsertContact_BankDetail(hdnContactId.Value, DtBank.Rows[0]["Bank_Id"].ToString(), dtContactBankDetail.Rows[i]["Account_No"].ToString(), dtContactBankDetail.Rows[i]["Branch_Address"].ToString(), dtContactBankDetail.Rows[i]["IFSC_Code"].ToString(), dtContactBankDetail.Rows[i]["MICR_Code"].ToString(), dtContactBankDetail.Rows[i]["Branch_Code"].ToString(), ModuleNode.Value, dtContactBankDetail.Rows[i]["IBAN_NUMBER"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }

                            catch
                            {

                            }


                        }
                    }
                    if (ModuleNode.Value == "1")
                    {
                        strType = "Customer";
                    }
                    if (ModuleNode.Value == "2")
                    {
                        strType = "Supplier";

                    }

                    if (objAddChild.GetAddressChildDataByAddTypeAndAddRefId(strType, hdnContactId.Value).Rows.Count == 0)
                    {

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
                            Label lblGvAddressName = (Label)gvr.FindControl("lblgvAddressName");
                            if (lblGvAddressName.Text != "")
                            {
                                DataTable dtAddId = ObjAddMaster.GetAddressDataByAddressName(lblGvAddressName.Text);
                                if (dtAddId.Rows.Count > 0)
                                {
                                    string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();

                                    objAddChild.InsertAddressChild(strAddressId, strType.ToString(), hdnContactId.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                                }
                            }
                        }
                    }
                }

                foreach (ListItem li in chkCompany.Items)
                {
                    bool Check;

                    if (li.Selected)
                    {

                        Check = true;
                        DataTable dt = ObjBrandMaster.GetBrandMaster(li.Value);
                        foreach (ListItem liBrand in chkBrand.Items)
                        {
                            if ((new DataView(dt, "Brand_Id='" + liBrand.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count != 0)
                            {
                                if (liBrand.Selected)
                                {
                                    Check = false;

                                    //if (!ObjContactMaster.IsUsedInInventory(li.Value, liBrand.Value, hdnContactId.Value, "1", "2"))
                                    //{
                                    if (ModuleNode.Value == "1")
                                    {


                                        ObjCustomerMaster.DeletePermanentbyCustomerId(li.Value, liBrand.Value, hdnContactId.Value.ToString());

                                        ObjCustomerMaster.InsertCustomerMaster(li.Value, liBrand.Value, hdnContactId.Value.ToString(), strReceiveVoucherAcc, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", IsBlock.ToString(), strBlockReason, txtcustomerTinNo.Text, txtcustomerCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text, Txt_TRN_No.Text.Trim(), "0", "");

                                        string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "399", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                        string sql = "select count(*) from ac_accountMaster";
                                        string strRecCount = da.get_SingleValue(sql);
                                        AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                                        DataTable dtCheckAccount = new DataTable();
                                        dtCheckAccount = da.return_DataTable("Select * from Ac_AccountMaster where Ref_Id='" + hdnContactId.Value.ToString() + "' And Ref_Type='Customer'");
                                        if (dtCheckAccount.Rows.Count == 0)
                                        {
                                            da.execute_Command("delete from Ac_AccountMaster where Ref_Type='Customer' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");
                                            da.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Customer', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");


                                        }


                                    }
                                    if (ModuleNode.Value == "2")
                                    {
                                        if (!ObjContactMaster.IsUsedInInventory(li.Value, liBrand.Value, hdnContactId.Value, "2", "2"))
                                        {
                                            ObjSupplierMaster.DeletePermanentbySupplierId(li.Value, liBrand.Value, hdnContactId.Value.ToString());
                                            ObjSupplierMaster.InsertSupplierMaster(li.Value, liBrand.Value, hdnContactId.Value.ToString(), strPaymentVoucherAcc, "0", "0", "0", "0", "0", "0", txtsupplierTinNo.Text, txtsupplierCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text.Trim(), Txt_TRN_No.Text.Trim());

                                            string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "Supplier" == "Customer" ? "399" : "400", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                            string sql = "select count(*) from ac_accountMaster";
                                            string strRecCount = da.get_SingleValue(sql);
                                            AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                                            DataTable dtCheckAccount = new DataTable();
                                            dtCheckAccount = da.return_DataTable("Select * from Ac_AccountMaster where Ref_Id='" + hdnContactId.Value.ToString() + "' And Ref_Type='Supplier'");
                                            if (dtCheckAccount.Rows.Count == 0)
                                            {
                                                da.execute_Command("delete from Ac_AccountMaster where Ref_Type='Supplier' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");

                                                da.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Supplier', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");

                                            }
                                        }
                                        else
                                        {
                                            string strSQL = "UPDATE Set_Suppliers Set GSTIN_NO='" + txtGSTIN.Text.Trim() + "', TRN_No='" + Txt_TRN_No.Text.Trim() + "' Where Company_Id=" + li.Value + " and Brand_Id=" + liBrand.Value + " and Supplier_Id=" + hdnContactId.Value.ToString() + "";
                                            int v = da.execute_Command(strSQL);
                                        }
                                    }
                                    //}

                                }
                            }
                            else
                            {
                                if (ModuleNode.Value == "1")
                                {
                                    if (ObjCustomerMaster.GetCustomerAllDataByCustomerId(li.Value, liBrand.Value, hdnContactId.Value).Rows.Count != 0)
                                    {
                                        ObjCustomerMaster.DeletePermanentbyCustomerId(li.Value, liBrand.Value, hdnContactId.Value.ToString());

                                    }
                                }
                                if (ModuleNode.Value == "2")
                                {
                                    if (ObjSupplierMaster.GetSupplierAllDataBySupplierId(li.Value, liBrand.Value, hdnContactId.Value).Rows.Count != 0)
                                    {
                                        ObjSupplierMaster.DeletePermanentbySupplierId(li.Value, liBrand.Value, hdnContactId.Value.ToString());

                                    }
                                }
                            }
                        }
                        if (Check)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (!ObjContactMaster.IsUsedInInventory(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value, "1", "2"))
                                {
                                    if (ModuleNode.Value == "1")
                                    {

                                        //if (ObjCustomerMaster.GetCustomerAllDataByCustomerId(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value).Rows.Count != 0)
                                        // {
                                        ObjCustomerMaster.DeletePermanentbyCustomerId(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value.ToString());

                                        // }
                                        ObjCustomerMaster.InsertCustomerMaster(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value.ToString(), strReceiveVoucherAcc, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", IsBlock.ToString(), strBlockReason, txtcustomerTinNo.Text, txtcustomerCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text, Txt_TRN_No.Text.Trim(), "0", "");
                                        string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "399", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                        string sql = "select count(*) from ac_accountMaster";
                                        string strRecCount = da.get_SingleValue(sql);
                                        AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);

                                        DataTable dtCheckAccount = new DataTable();
                                        dtCheckAccount = da.return_DataTable("Select * from Ac_AccountMaster where Ref_Id='" + hdnContactId.Value.ToString() + "' And Ref_Type='Customer'");
                                        if (dtCheckAccount.Rows.Count == 0)
                                        {
                                            da.execute_Command("delete from Ac_AccountMaster where Ref_Type='Customer' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");
                                            da.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Customer', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");
                                        }
                                    }
                                    if (ModuleNode.Value == "2")
                                    {
                                        if (!ObjContactMaster.IsUsedInInventory(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value, "2", "2"))
                                        {
                                            // if (ObjSupplierMaster.GetSupplierAllDataBySupplierId(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value).Rows.Count != 0)
                                            // {
                                            ObjSupplierMaster.DeletePermanentbySupplierId(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value.ToString());

                                            // }
                                            ObjSupplierMaster.InsertSupplierMaster(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value.ToString(), strPaymentVoucherAcc, "0", "0", "0", "0", "0", "0", txtsupplierTinNo.Text, txtsupplierCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text.Trim(), Txt_TRN_No.Text.Trim());

                                            string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "Supplier" == "Customer" ? "399" : "400", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                            string sql = "select count(*) from ac_accountMaster";
                                            string strRecCount = da.get_SingleValue(sql);
                                            AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                                            DataTable dtCheckAccount = new DataTable();
                                            dtCheckAccount = da.return_DataTable("Select * from Ac_AccountMaster where Ref_Id='" + hdnContactId.Value.ToString() + "' And Ref_Type='Supplier'");
                                            if (dtCheckAccount.Rows.Count == 0)
                                            {
                                                da.execute_Command("delete from Ac_AccountMaster where Ref_Type='Supplier' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");

                                                da.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Supplier', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (ModuleNode.Value == "1")
                                    {
                                        if (ObjCustomerMaster.GetCustomerAllDataByCustomerId(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value).Rows.Count == 0)
                                        {
                                            ObjCustomerMaster.InsertCustomerMaster(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value.ToString(), strReceiveVoucherAcc, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", IsBlock.ToString(), strBlockReason, txtcustomerTinNo.Text, txtcustomerCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text, Txt_TRN_No.Text.Trim(), "0", "");

                                            string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "399", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                            string sql = "select count(*) from ac_accountMaster";
                                            string strRecCount = da.get_SingleValue(sql);
                                            AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                                            DataTable dtCheckAccount = new DataTable();
                                            dtCheckAccount = da.return_DataTable("Select * from Ac_AccountMaster where Ref_Id='" + hdnContactId.Value.ToString() + "' And Ref_Type='Customer'");
                                            if (dtCheckAccount.Rows.Count == 0)
                                            {
                                                da.execute_Command("delete from Ac_AccountMaster where Ref_Type='Customer' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");
                                                da.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Customer', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");

                                            }

                                        }
                                    }
                                    if (ModuleNode.Value == "2")
                                    {
                                        if (ObjSupplierMaster.GetSupplierAllDataBySupplierId(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value).Rows.Count == 0)
                                        {
                                            ObjSupplierMaster.InsertSupplierMaster(li.Value, dr["Brand_Id"].ToString(), hdnContactId.Value.ToString(), strPaymentVoucherAcc, "0", "0", "0", "0", "0", "0", txtsupplierTinNo.Text, txtsupplierCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text.Trim(), Txt_TRN_No.Text.Trim());

                                            string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "Supplier" == "Customer" ? "399" : "400", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                                            string sql = "select count(*) from ac_accountMaster";
                                            string strRecCount = da.get_SingleValue(sql);
                                            AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                                            DataTable dtCheckAccount = new DataTable();
                                            dtCheckAccount = da.return_DataTable("Select * from Ac_AccountMaster where Ref_Id='" + hdnContactId.Value.ToString() + "' And Ref_Type='Supplier'");
                                            if (dtCheckAccount.Rows.Count == 0)
                                            {
                                                da.execute_Command("delete from Ac_AccountMaster where Ref_Type='Supplier' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");
                                                da.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Supplier', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')");

                                            }

                                        }
                                    }


                                }



                            }

                        }

                    }
                    else
                    {

                        DataTable dt = ObjBrandMaster.GetBrandMaster(li.Value);
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (ModuleNode.Value == "1")
                            {
                                if (ObjCustomerMaster.GetCustomerAllDataByCustomerId(li.Value, dr["Brand_id"].ToString(), hdnContactId.Value).Rows.Count != 0)
                                {
                                    ObjCustomerMaster.DeletePermanentbyCustomerId(li.Value, dr["Brand_id"].ToString(), hdnContactId.Value.ToString());

                                }
                            }
                            if (ModuleNode.Value == "2")
                            {
                                if (ObjSupplierMaster.GetSupplierAllDataBySupplierId(li.Value, dr["Brand_id"].ToString(), hdnContactId.Value).Rows.Count != 0)
                                {
                                    ObjSupplierMaster.DeletePermanentbySupplierId(li.Value, dr["Brand_id"].ToString(), hdnContactId.Value.ToString());

                                }
                            }

                        }
                    }
                }
            }
            else
            {


                foreach (ListItem li in chkCompany.Items)
                {
                    DataTable dt = ObjBrandMaster.GetBrandMaster(li.Value);
                    foreach (ListItem liBrand in chkBrand.Items)
                    {
                        if ((new DataView(dt, "Brand_Id='" + liBrand.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count != 0)
                        {
                            if (ModuleNode.Value == "1")
                            {

                                if (ObjCustomerMaster.GetCustomerAllDataByCustomerId(li.Value, liBrand.Value, hdnContactId.Value).Rows.Count != 0)
                                {
                                    ObjCustomerMaster.DeletePermanentbyCustomerId(li.Value, liBrand.Value, hdnContactId.Value.ToString());

                                }

                            }
                            if (ModuleNode.Value == "2")
                            {
                                if (ObjSupplierMaster.GetSupplierAllDataBySupplierId(li.Value, liBrand.Value, hdnContactId.Value).Rows.Count != 0)
                                {
                                    ObjSupplierMaster.DeletePermanentbySupplierId(li.Value, liBrand.Value, hdnContactId.Value.ToString());

                                }
                            }
                        }
                    }


                }
            }

        }
        ObjCompanyBrand.DeleteContactCompanyBrand(hdnContactId.Value.Trim());
        foreach (ListItem li in chkCompany.Items)
        {
            DataTable dt = ObjBrandMaster.GetBrandMaster(li.Value);
            bool b;
            if (li.Selected)
            {
                b = true;
                foreach (ListItem liBrand in chkBrand.Items)
                {
                    if ((new DataView(dt, "Brand_Id='" + liBrand.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count != 0)
                    {

                        if (liBrand.Selected)
                        {
                            b = false;

                            ObjCompanyBrand.InsertContactCompanyBrand(hdnContactId.Value.Trim(), li.Value.ToString(), liBrand.Value.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
                        }
                    }
                }
                if (b)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ObjCompanyBrand.InsertContactCompanyBrand(hdnContactId.Value.Trim(), li.Value.ToString(), dr["Brand_Id"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());


                    }

                }
            }
        }
        objCG.DeleteContactGroup(hdnContactId.Value);

        foreach (TreeNode ModuleNode in navTree.Nodes)
        {

            string strTinNo = string.Empty;
            string strCstNo = string.Empty;
            if (ModuleNode.Checked)
            {
                if (ModuleNode.Value == "1")
                {
                    strTinNo = txtcustomerTinNo.Text;
                    strCstNo = txtcustomerCstNo.Text;

                    Id = txtCustomerId.Text.Trim();
                }
                if (ModuleNode.Value == "2")
                {
                    strTinNo = txtsupplierTinNo.Text;
                    strCstNo = txtsupplierCstNo.Text;
                    Id = txtSupplierId.Text.Trim();
                }
                objCG.InsertContactGroup(hdnContactId.Value, ModuleNode.Value, "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Id.ToString(), strTinNo, strCstNo, "", "", true.ToString(), DateTime.Now.ToString());
                foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
                {
                    if (ObjNode.Checked)
                    {
                        int refid1 = 0;
                        refid1 = objCG.InsertContactGroup(hdnContactId.Value, ObjNode.Value, "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
                        childNodeSave(ObjNode);
                    }
                }

            }
        }
        DisplayMessage("Record Saved", "green");
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

        Reset();
        btnList_Click(null, null);
        if (Request.QueryString["EmailId"] != null)
        {

            Session["AddMailReffrence"] = "Yes";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "checkLoad();", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);
        }

        if (Request.QueryString["Page"] != null)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);
        }
    }

    public void childNodeSave(TreeNode ModuleNode)
    {

        foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
        {
            if (ObjNode.Checked)
            {
                int refid1 = 0;
                refid1 = objCG.InsertContactGroup(hdnContactId.Value, ObjNode.Value, "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
                childNodeSave(ObjNode);
            }
        }


    }
    protected void GvContactBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvContactBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterbin"];
        GvContactBin.DataSource = dt;
        GvContactBin.DataBind();

        foreach (GridViewRow gvrbin in GvContactBin.Rows)
        {
            Label lblPreBin = (Label)gvrbin.FindControl("lblPrefixBin");

            if (lblPreBin.Text == "0")
            {
                lblPreBin.Text = "";
            }
        }
        GvContactBin.BottomPagerRow.Focus();
        string temp = string.Empty;
        //bool isselcted;

        for (int i = 0; i < GvContactBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvContactBin.Rows[i].FindControl("Label5");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvContactBin.Rows[i].FindControl("ChkActive")).Checked = true;
                    }
                }
            }
        }
    }
    protected void chkActive_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvContactBin.Rows[index].FindControl("Label5");
        if (((CheckBox)GvContactBin.Rows[index].FindControl("chkActive")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
        ((CheckBox)GvContactBin.Rows[index].FindControl("chkActive")).Focus();
    }
    protected void chkActiveAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvContactBin.HeaderRow.FindControl("chkActiveAll"));
        for (int i = 0; i < GvContactBin.Rows.Count; i++)
        {
            ((CheckBox)GvContactBin.Rows[i].FindControl("ChkActive")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvContactBin.Rows[i].FindControl("Label5"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvContactBin.Rows[i].FindControl("Label5"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvContactBin.Rows[i].FindControl("Label5"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
        chkSelAll.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        FillGridBin(); ;
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;

        txtValueBin.Focus();
        lblSelectedRecord.Text = "";
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        lblSelectedRecord.Text = "";
        if (ddlOptionBin.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = ddlFieldNameBin.SelectedValue + "='" + txtValueBin.Text.Trim() + "' and IsActive='False'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = ddlFieldNameBin.SelectedValue + " Like '%" + txtValueBin.Text.Trim() + "%' and IsActive='False'";
            }
            else
            {
                condition = ddlFieldNameBin.SelectedValue + " like '" + txtValueBin.Text.Trim() + "%' and IsActive='False'";
            }
            DataTable dtContact = (DataTable)Session["dtContactbin"];
            DataView view = new DataView(dtContact, condition, "", DataViewRowState.CurrentRows);
            GvContactBin.DataSource = view.ToTable();
            GvContactBin.DataBind();

            foreach (GridViewRow gvrbin in GvContactBin.Rows)
            {
                Label lblPreBin = (Label)gvrbin.FindControl("lblPrefixBin");

                if (lblPreBin.Text == "0")
                {
                    lblPreBin.Text = "";
                }
            }
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            Session["dtFilterbin"] = view.ToTable();

            //ddlOptionBin.SelectedIndex = 2;
            //ddlFieldNameBin.SelectedIndex = 1;
            txtValueBin.Focus();
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;

            }
            else
            {
                // AllPageCode();
            }
        }
        txtValueBin.Focus();
    }

    protected void GvContactBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterbin"];
        string sortdir = "DESC";
        if (ViewState["SortDirBin"] != null)
        {
            sortdir = ViewState["SortDirBin"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDirBin"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDirBin"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDirBin"] = "DESC";
        }
        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirBin"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtFilterbin"] = dt;
        GvContactBin.DataSource = dt;
        GvContactBin.DataBind();
        foreach (GridViewRow gvrbin in GvContactBin.Rows)
        {
            Label lblPreBin = (Label)gvrbin.FindControl("lblPrefixBin");

            if (lblPreBin.Text == "0")
            {
                lblPreBin.Text = "";
            }
        }
        GvContactBin.HeaderRow.Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        DataTable dtContact = (DataTable)Session["dtFilterbin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtContact.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvContactBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvContactBin.Rows[i].FindControl("Label5");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvContactBin.Rows[i].FindControl("chkActive")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtContact1 = (DataTable)Session["dtFilterbin"];
            GvContactBin.DataSource = dtContact1;
            GvContactBin.DataBind();
            foreach (GridViewRow gvrbin in GvContactBin.Rows)
            {
                Label lblPreBin = (Label)gvrbin.FindControl("lblPrefixBin");

                if (lblPreBin.Text == "0")
                {
                    lblPreBin.Text = "";
                }
            }
            ViewState["Select"] = null;
        }
        ImgbtnSelectAll.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjContactMaster.DeleteContactMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

                    ObjContactMaster.DeleteContactMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                    DataTable dtcompnaybrand = ObjCompanyBrand.GetContactCompanyBrandAllData(lblSelectedRecord.Text.Split(',')[j].Trim().ToString());
                    foreach (DataRow dr in dtcompnaybrand.Rows)
                    {
                        ObjCustomerMaster.DeleteCustomerMasterByContactId(dr["Company_Id"].ToString(), dr["Brand_Id"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
                        ObjSupplierMaster.DeleteSupplierMasterByContactId(dr["Company_Id"].ToString(), dr["Brand_Id"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

                    }
                }
            }
        }

        if (b != 0)
        {
            // FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
            if (GvContactBin.Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;

            }
            else
            {
                //AllPageCode();
            }
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvContactBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkActive");
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
        imgBtnRestore.Focus();
    }

    #endregion
    protected void txtCustomerId_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = objCG.GetContactGroupAllData();
        if (dt.Rows.Count != 0)
        {
            dt = new DataView(dt, "Field1='" + txtCustomerId.Text.Trim() + "'and Group_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0]["Contact_Id"].ToString() != hdnContactId.Value.Trim())
                {
                    DisplayMessage("Customer Id  Already Exists");
                    txtCustomerId.Text = "";
                }
            }
        }
    }
    protected void txtSupplierId_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = objCG.GetContactGroupAllData();
        if (dt.Rows.Count != 0)
        {
            dt = new DataView(dt, "Field1='" + txtSupplierId.Text.Trim() + "'and Group_Id='2'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0]["Contact_Id"].ToString() != hdnContactId.Value.Trim())
                {
                    DisplayMessage("Supplier Id  Already Exists");
                    txtSupplierId.Text = "";

                }
            }
        }
    }
    public void FillGroup()
    {
        DataTable DtGroup = objGroup.GetGroupMasterTrueAllData();
        DtGroup = new DataView(DtGroup, "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

        if (DtGroup.Rows.Count > 0)
        {
            ddlGroupSearch.DataSource = DtGroup;
            ddlGroupSearch.DataTextField = "Group_Name";
            ddlGroupSearch.DataValueField = "Group_Id";
            ddlGroupSearch.DataBind();

        }
        ddlGroupSearch.Items.Insert(0, "--Select--");


    }



    protected void BtnCancelView_Click(object sender, EventArgs e)
    {
        ViewReset();
        Reset();
        btnList_Click(null, null);
    }
    protected void btnCloseView_Click(object sender, EventArgs e)
    {
        ViewReset();
    }
    void ViewReset()
    {
        btnList_Click(null, null);
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        BtnEdit_Command(sender, e);
    }
    protected void datalistCustomerDetailView_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        GridView gvAddressView = (GridView)e.Item.FindControl("GvAddressNameView");
        HiddenField hdnContactId = (HiddenField)e.Item.FindControl("hdnContactIdView");
        GridView GvContactBankDetail = (GridView)e.Item.FindControl("GvContactBankDetail");
        DataList dlViewcontactlist = (DataList)e.Item.FindControl("dlViewcontactlist");
        Label lblCSalesPrice = (Label)e.Item.FindControl("lblCSalesPrice");
        string Id = hdnContactId.Value;
        DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", Id);
        if (dtChild.Rows.Count > 0)
        {
            gvAddressView.DataSource = dtChild;
            gvAddressView.DataBind();
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
        DataTable dtcontactBank = objContactBankDetail.GetContactBankDetail_By_ContactId_And_GroupId(Id, "2");
        if (dtcontactBank.Rows.Count > 0)
        {

            GvContactBankDetail.DataSource = dtcontactBank;
            GvContactBankDetail.DataBind();
        }
        DataTable dt = ObjContactMaster.GetContactTrueAllData(Id, "Individual");
        if (dt.Rows.Count != 0)
        {
            dlViewcontactlist.DataSource = dt;
            dlViewcontactlist.DataBind();
        }


    }
    public string GetBankName(string BankId)
    {
        string BankName = string.Empty;

        DataTable DtBank = objBankMaster.GetBankMasterById(BankId);
        try
        {
            BankName = DtBank.Rows[0]["Bank_Name"].ToString();
        }
        catch
        {
        }
        return BankName;
    }
    public string GetDepartmentName(string DepartmentId)
    {
        string DepartmentName = string.Empty;

        DataTable dtDepartment = ObjDepMaster.GetDepartmentMasterById(DepartmentId);
        try
        {
            DepartmentName = dtDepartment.Rows[0]["Dep_Name"].ToString();
        }
        catch
        {
        }

        return DepartmentName;
    }
    public string GetDesignationName(string DesgId)
    {
        string DesgName = string.Empty;

        DataTable dtDesg = ObjDesMaster.GetDesignationMasterById(DesgId);
        try
        {
            DesgName = dtDesg.Rows[0]["Designation"].ToString();
        }
        catch
        {
        }

        return DesgName;
    }
    public string GetReligionName(string ReligionId)
    {
        string ReligionName = string.Empty;

        DataTable dtReligion = ObjRelMaster.GetReligionMasterById(ReligionId);
        try
        {
            ReligionName = dtReligion.Rows[0]["Religion"].ToString();
        }
        catch
        {
        }

        return ReligionName;
    }
    #region LongitudeLatitude
    protected void lnkGetLatLong_Click(object sender, EventArgs e)
    {

        LinkButton lnkGotoMap = (LinkButton)sender;

        string[] arguments = lnkGotoMap.CommandArgument.ToString().Split(new char[] { ';' });

        string FullAddress = arguments[0];
        string longitute = arguments[1];
        string latitude = arguments[2];
        //txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;
        if (longitute.ToString() == "0.0000" && latitude.ToString() == "0.0000")
        {
            Session["Add"] = FullAddress;
        }
        else
        {
            Session["Add"] = "1";
            Session["Long"] = longitute.ToString();
            Session["Lati"] = latitude.ToString();
        }




        ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../SystemSetup/GoogleMap.aspx','window','width=1024')", true);

    }

    public string GetContactLongitude(string AddressName)
    {
        string ContactLongitude = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["Longitude"].ToString() != "")
            {
                ContactLongitude = DtAddress.Rows[0]["Longitude"].ToString();
            }
            else
            {
                ContactLongitude = "0.0000";
            }
        }
        return ContactLongitude;
    }
    public string GetContactLatitude(string AddressName)
    {
        string ContactLatitude = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(AddressName);
        if (DtAddress.Rows.Count > 0)
        {
            if (DtAddress.Rows[0]["Latitude"].ToString() != "")
            {
                ContactLatitude = DtAddress.Rows[0]["Latitude"].ToString();
            }
            else
            {
                ContactLatitude = "0.0000";
            }
        }
        return ContactLatitude;
    }

    #endregion
    #region Export to excel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //DataTable dtContact = ObjContactMaster.GetContactTrueAllData();
        string sql = @"select ecm.status as Contact_Type,ecm.code as Contact_Code, ecm.Name as  Contact_Name, isnull(ecm.Field1,'') as Email_ID, isnull(Address_Category,'') as Address_Category,isnull(Address_Name,'') as Address_Name,isnull(Address,'') as Address ,country.Country_Name as Country,scm.Currency_Name as Currency,ecm.Field2 as Contact_No,isnull(sdm.Dep_Name,'') as Department,isnull(dm.Designation,'') as Designation,(select top 1 Group_Name from dbo.Ems_Contact_Group inner join dbo.Ems_GroupMaster on Ems_GroupMaster.Group_Id=Ems_Contact_Group.Group_Id where Ems_Contact_Group.Contact_Id=ecm.Trans_Id order by Ems_GroupMaster.group_id) as [Group],isnull((case when cm.customer_id is null then sm.GSTIN_NO else cm.GSTIN_NO end),'') as GSTIN_No,isnull((case when cm.customer_id is null then sm.TRN_No else cm.TRN_No end),'') as TRN_No
from ems_contactmaster ecm
left join
(select distinct Set_AddressChild.Add_Ref_Id,Set_AddressMaster.Address_Name,Set_AddressCategory.Address_Name as address_category,Set_AddressMaster.Address,Sys_CountryMaster.Country_Name as Country from Set_AddressMaster inner join dbo.Set_AddressChild on Set_AddressMaster.Trans_Id=Set_AddressChild.Ref_Id inner join Set_AddressCategory on Set_AddressCategory.Address_Category_Id=Set_AddressMaster.Address_Category_Id 
left join Sys_CountryMaster on Sys_CountryMaster.Country_Id=Set_AddressMaster.CountryId
where Set_AddressChild.Is_Default='1' and Set_AddressChild.IsActive='true' and Set_AddressChild.Add_Type='Contact') ca
on ca.Add_Ref_Id = ecm.Trans_Id
left join Sys_CurrencyMaster scm
on scm.Currency_ID=ecm.field5
left join Set_DepartmentMaster sdm
on sdm.Dep_Id=ecm.Dep_Id
left join Set_DesignationMaster dm
on dm.Designation_Id=ecm.Designation_Id
left join (select distinct customer_id,GSTIN_NO,TRN_No from Set_CustomerMaster) cm
on cm.Customer_Id=ecm.Trans_Id
left join (select distinct supplier_id,GSTIN_NO,TRN_No from Set_Suppliers) sm
on sm.Supplier_Id=ecm.Trans_Id
left join sys_countrymaster country on country.Country_Id=ecm.field4
where ecm.IsActive='true'";

        if (!string.IsNullOrEmpty(txtCountryFilter.Text))
        {
            sql += " and country.Country_Name='" + txtCountryFilter.Text + "'";
        }

        DataTable dtContact = da.return_DataTable(sql);

        if (dtContact.Rows.Count > 0)
        {
            ExportToExcel(dtContact, "ContactList");
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }
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

    public void ExportTableData(DataTable Dt_Data, string FName)
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

    protected void btnUploadExcel_Click(object sender, EventArgs e)
    {

        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PnlNewEdit.Visible = false;

    }

    public DataTable ConvetExcelToDataTable(string path)
    {
        DataTable dt = new DataTable();
        string strcon = string.Empty;
        if (Path.GetExtension(path) == ".xls")
        {
            strcon = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + path + "; Extended Properties =\"Excel 8.0;HDR=YES;\"";
        }
        else if (Path.GetExtension(path) == ".xlsx")
        {
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        try
        {
            OleDbConnection oledbConn = new OleDbConnection(strcon);
            oledbConn.Open();
            DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string strquery = "select * from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] ";
            OleDbCommand com = new OleDbCommand(strquery, oledbConn);
            DataSet ds = new DataSet();
            OleDbDataAdapter oledbda = new OleDbDataAdapter(com);
            oledbda.Fill(ds, Sheets.Rows[0]["Table_Name"].ToString());
            oledbConn.Close();
            dt = ds.Tables[0];
        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }


    #endregion
    #region Multiple Email
    protected void btnAddEmail_Click(object sender, EventArgs e)
    {
        if (txtPermanentMailId.Text == "")
        {
            DisplayMessage("Enter EmailId");
            txtPermanentMailId.Text = "";
            txtPermanentMailId.Focus();
            return;
        }
        else
        {

            if (!IsValidEmailId(txtPermanentMailId.Text))
            {
                DisplayMessage("Enter Valid Email-Id");
                txtPermanentMailId.Focus();
                return;
            }
        }

        if (rbnEmailList.Items.Count > 0)
        {
            if (rbnEmailList.Items.FindByText(txtPermanentMailId.Text) == null)
            {
                rbnEmailList.Items.Add(txtPermanentMailId.Text);
                txtPermanentMailId.Text = "";
                txtPermanentMailId.Focus();
            }
            else
            {
                DisplayMessage("EmailId Already Exists");
                txtPermanentMailId.Text = "";
                txtPermanentMailId.Focus();
                return;
            }
        }
        else
        {
            rbnEmailList.Items.Add(txtPermanentMailId.Text);
            rbnEmailList.SelectedIndex = 0;
            txtPermanentMailId.Text = "";
            txtPermanentMailId.Focus();
        }
    }
    protected void btnRemoveEmail_Click(object sender, EventArgs e)
    {
        if (rbnEmailList.SelectedItem != null)
        {
            rbnEmailList.Items.Remove(rbnEmailList.SelectedItem.Value);
        }
        else
        {
            DisplayMessage("No Value Selected");

            return;
        }
    }
    private bool IsValidEmailId(string InputEmail)
    {
        //Regex To validate Email Address
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(InputEmail);
        if (match.Success)
            return true;
        else
            return false;
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
    public string GetEmailID(string ContactId)
    {

        string Emails = string.Empty;
        DataTable dtEmail = objEmailDetail.Get_EmailMasterHeaderDetailByRefIdNRefType(ContactId.ToString(), "Contact");
        try
        {
            for (int i = 0; i < dtEmail.Rows.Count; i++)
            {
                if (Emails.ToString() == "")
                {
                    Emails = dtEmail.Rows[i]["Email_Id"].ToString();
                }
                else
                {
                    Emails = Emails + "," + dtEmail.Rows[i]["Email_Id"].ToString();
                }
            }
        }
        catch
        {
        }

        return Emails;
    }

    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCountryName(string prefixText, int count, string contextKey)
    {
        CountryMaster objCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objCountryMaster.GetCountryMaster(), "Country_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Country_Name"].ToString();
        }
        return txt;
    }
    protected void txtCountryName_TextChanged(object sender, EventArgs e)
    {
        if (txtCountryName.Text != "")
        {
            DataTable dt = objCountryMaster.GetCountryMasterByCountryName(txtCountryName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                hdncountryid.Value = dt.Rows[0]["Country_Id"].ToString();
                ddlCurrency.SelectedValue = objCountryCurrency.GetCurrencyByCountryId(hdncountryid.Value, "1").Rows[0]["Currency_ID"].ToString();
                //ddlCountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Code"].ToString();
                ContactNo1.setCountryCode("+" + objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Code"].ToString());
            }
            else
            {
                DisplayMessage("Choose in Suggestion only");
                txtCountryName.Text = "";
                txtCountryName.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Country Name");
            txtCountryName.Focus();
            return;
        }
    }
    protected void txtCountryFilter_TextChanged(object sender, EventArgs e)
    {
        if (txtCountryFilter.Text != "")
        {
            string countryId = objCountryMaster.getCountryIdByName(txtCountryFilter.Text.Trim());
            if (countryId == "")
            {
                DisplayMessage("Choose in Suggestion only");
                txtCountryFilter.Text = "";
                txtCountryFilter.Focus();
                return;
            }
        }
    }
    #region LicFilter

    protected void chkLicContact_OnCheckedChanged(object sender, EventArgs e)
    {

        FillGrid();
        //AllPageCode();

    }

    #endregion
    #region VerifyProduct


    protected void btnVerify_Click(object sender, EventArgs e)
    {

        pnlUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        FillVeryGrid();
        txtValueBin.Focus();
        //AllPageCode();

    }
    public void FillVeryGrid()
    {
        DataTable dtProduct = new DataView(ObjContactMaster.GetContactTrueAllDataForGRid(), "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();
        gvVerifyProduct.DataSource = dtProduct;
        gvVerifyProduct.DataBind();

        Session["DtVerifyProduct"] = dtProduct;
        lblVerifyTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtProduct.Rows.Count.ToString() + "";
    }

    protected void btnVerifyProduct_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int Msg = 0;


        if (gvVerifyProduct.Rows.Count != 0)
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
                        Msg = ObjContactMaster.updateVerifyStatus(userdetails[i].ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                    if (Msg != 0)
                    {
                        FillVeryGrid();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        DisplayMessage("Record Verify successfully");
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

    protected void btnSelectRecord_Click(object sender, EventArgs e)
    {

        I3.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["DtVerifyProduct"];

        if (btnSelectRecord.Text == "Select All")
        {
            btnSelectRecord.Text = "UnSelect All";
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

            }
            foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["DtVerifyProduct"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvVerifyProduct, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
            btnSelectRecord.Text = "Select All";
        }


    }

    protected void imgSearchVeryProduct_Click(object sender, EventArgs e)
    {

        I3.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (ddlVerifyProductOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlVerifyProductOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlVerifyProductFieldName.SelectedValue + ",System.String)='" + txtVerifyProduct.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlVerifyProductFieldName.SelectedValue + ",System.String) like '%" + txtVerifyProduct.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlVerifyProductFieldName.SelectedValue + ",System.String) Like '" + txtVerifyProduct.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtVerifyProduct"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["DtVerifyProduct"] = view.ToTable();
            lblVerifyTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvVerifyProduct, view.ToTable(), "", "");
            //AllPageCode();
            btnbind.Focus();

        }

    }
    protected void imgSearchVeryRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        txtVerifyProduct.Text = "";
        ddlVerifyProductFieldName.SelectedIndex = 2;
        ddlVerifyProductOption.SelectedIndex = 2;
        txtVerifyProduct.Focus();
        FillVeryGrid();
        //AllPageCode();
    }

    protected void chkgvVerifySelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvVerifyProduct.HeaderRow.FindControl("chkgvSelectAll"));
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
        foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
        {
            index = (int)gvVerifyProduct.DataKeys[gvrow.RowIndex].Value;
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

    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
            {
                int index = (int)gvVerifyProduct.DataKeys[gvrow.RowIndex].Value;
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
        foreach (GridViewRow gvrow in gvVerifyProduct.Rows)
        {
            index = (int)gvVerifyProduct.DataKeys[gvrow.RowIndex].Value;
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

    protected void gvVerifyProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvVerifyProduct.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["DtVerifyProduct"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvVerifyProduct, dt, "", "");
        PopulateCheckedValues();
        //AllPageCode();
    }

    protected void gvVerifyProduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtVerifyProduct"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtVerifyProduct"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvVerifyProduct, dt, "", "");
        Session["CHECKED_ITEMS"] = null;
        //AllPageCode();
        gvVerifyProduct.HeaderRow.Focus();
    }

    #endregion
    #region TreeViewConcept

    protected void txtParentContactName_TextChanged(object sender, EventArgs e)
    {
        if (txtParentContactName.Text != "")
        {
            DataTable dtContact = new DataView(ObjContactMaster.GetContactTrueAllData(), "Name='" + txtParentContactName.Text.Split('/')[0].Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtContact.Rows.Count == 0)
            {
                DisplayMessage("Choose In Suggestions Only");
                txtParentContactName.Text = "";
                txtCompany.Text = "";
                txtCompany.Focus();
                return;

            }

        }

    }
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //Lbl_Heading_Grid.Text = "Tree View";
        if (TreeViewCategory.Visible == true)
        {
            Div_Tree.Visible = false;
            TreeViewCategory.Visible = false;
            gvContact.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvContact.Visible = false;
            Div_Tree.Visible = true;
            TreeViewCategory.Visible = true;
            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }
    protected void TreeViewCategory_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewCategory.SelectedValue.ToString());
        BtnEdit_Command(sender, CmdEvntArgs);
    }
    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //Lbl_Heading_Grid.Text = "Grid";
        if (TreeViewCategory.Visible == true)
        {
            Div_Tree.Visible = false;
            TreeViewCategory.Visible = false;
            gvContact.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvContact.Visible = false;
            Div_Tree.Visible = true;
            TreeViewCategory.Visible = true;
            BindTreeView();

        }
        btnTreeView.Focus();
    }
    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        string condition = string.Empty;
        TreeViewCategory.Nodes.Clear();
        DataTable dt = new DataTable();

        string x = "ParentContactId=" + "0" + "";


        dt = ObjContactMaster.GetContactTrueAllDataForGRid();

        if (!chkLicContact.Checked)
        {
            dt = new DataView(dt, "Code not like '%Lic-%'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (txtCountryFilter.Text != "")
        {

            dt = new DataView(dt, "CountryName='" + txtCountryFilter.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }


        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
        {
            condition = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();
            dt = ObjContactMaster.GetContactForMainGridByGroupId(condition);
        }
        dt = new DataView(dt, "ParentContactId='0'", "ContactName asc", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["ContactName"].ToString();
            tn.Value = dt.Rows[i]["Trans_Id"].ToString();
            TreeViewCategory.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Trans_Id"].ToString()), tn);
            i++;
        }
        TreeViewCategory.DataBind();

    }
    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = ObjContactMaster.GetAllContact_By_ParentId(index);

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["ContactName"].ToString();
            tn1.Value = dt.Rows[i]["Trans_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Trans_Id"].ToString()), tn1);
            i++;
        }
        TreeViewCategory.DataBind();
    }
    protected void btnCloseTreePanel_Click(object sender, EventArgs e)
    {
        Reset();
        txtValue.Focus();
    }
    protected void btnDeleteChild_Click(object sender, EventArgs e)
    {
        ObjContactMaster.DeleteContactMaster_By_ParentId(hdnParentContactId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        ObjContactMaster.DeleteContactMaster(hdnParentContactId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        DisplayMessage("Record Delete");
        FillGrid();

    }
    protected void btnMoveChild_Click(object sender, EventArgs e)
    {
        //btnDeleteChild.Visible = false;
        //btnBack.Visible = true;
        //btnMoveChild.Visible = false;
        //pnlMoveChild.Visible = true;
        FillMoveChildDropDownList(hdnParentContactId.Value);
    }
    protected void btnUpdateParent_Click(object sender, EventArgs e)
    {
        //if (ddlMoveCategory.SelectedItem.Text != "No Category Available Here")
        //{
        //    ObjContactMaster.DeleteContactMaster(hdnParentContactId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());

        //    ObjContactMaster.UpdateParentContact(hdnParentContactId.Value, ddlMoveCategory.SelectedValue, Session["UserId"].ToString(), DateTime.Now.ToString());
        //    DisplayMessage("Record Delete and Move Child");
        //    panelOverlay.Visible = false;
        //    panelPopUpPanel.Visible = false;
        //    FillGrid();
        //}
    }
    public void FillMoveChildDropDownList(string strExceptId) //Function to fill up items in drop down list of New Parent after delete
    {
        DataTable dt = ObjContactMaster.GetContactTrueAllDataForGRid();
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

            // ddlMoveCategory.Items.Clear();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015


            //objPageCmn.FillData((object)ddlMoveCategory, dt, "Name", "Trans_Id");


        }
        else
        {
            //ddlMoveCategory.Items.Add("No Category Available Here");
        }

        arr.Clear();
    }

    private void FindChildNode(string p)  //Function to find child nodes and child of child nodes and so on
    {
        arr.Add(p);
        DataTable dt = ObjContactMaster.GetAllContact_By_ParentId(p);
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FindChildNode(dt.Rows[i]["Trans_Id"].ToString());
            }
        }
        else
        {
            return;
        }
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {

        //btnDeleteChild.Visible = true;
        //btnBack.Visible = false;
        //btnMoveChild.Visible = true;
        //pnlMoveChild.Visible = false;
    }

    #endregion

    #region Imageupload

    protected void btnUpload1_Click(object sender, EventArgs e)
    {
        imgLogo.ImageUrl = getImageUrl(FULogoPath.FileName);
    }

    #endregion

    // Code By Kalu Singh on 04/05-08-2017

    protected void ddlMyFav_SelectedIndexChanged(object sender, EventArgs e)
    {
        string TypeName = string.Empty;
        string AllId = string.Empty;
        TypeName = ddlMyFav.SelectedValue.ToString();

        if (TypeName == "Add" || TypeName == "Remove")
        {

            foreach (GridViewRow row in gvContact.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox myChk = row.FindControl("chkFav") as CheckBox;
                    Label myLbl = row.FindControl("lblTransId") as Label;

                    if (myChk != null && myChk.Checked)
                    {
                        string id = myLbl.Text;

                        if (!String.IsNullOrEmpty(id))
                        {
                            if (String.IsNullOrEmpty(AllId))
                            {
                                AllId = id;
                            }
                            else
                            {
                                AllId = AllId + "," + id;
                            }
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(AllId))
            {
                string userid = Session["UserId"].ToString();
                int saveid = ObjContactMaster.SaveFavoriteContact(Session["UserId"].ToString(), AllId, TypeName);
                if (saveid > 0)
                {
                    if (TypeName == "Add")
                        DisplayMessage("Contact is add as Favorite");
                    else
                        DisplayMessage("Contact is remove from Favorite");

                    FillGrid();
                    ddlMyFav.SelectedIndex = 0;
                }
            }
            else
            {
                ddlMyFav.SelectedIndex = 0;
                DisplayMessage("Please Select at least one record");
                btnRefreshReport_Click(null, null);
            }
        }
        else if (TypeName == "Favorite")
        {
            int currentPage = 1, _TotalRowCount = 0;
            string PageSize = PageControlCommon.GetPageSize().ToString();
            string condition = string.Empty;

            if (!String.IsNullOrEmpty(txtCountryFilter.Text))
                condition = " Country_Name = '" + txtCountryFilter.Text + "'";
            string Usr_ID_Filter = "and Fav.User_Id = '" + Session["UserId"].ToString() + "'";
            DataTable dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, true.ToString(), "true", "", "", "", "", Usr_ID_Filter);
            DataTable dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, false.ToString(), "true", "", "", "", "", Usr_ID_Filter);

            _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());

            lblTotalRecordNumber.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

            gvContact.DataSource = dt;
            gvContact.DataBind();


            // lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(SetDecimal(sumObject.ToString()), strCurrencyId.ToString());

            generatePager(_TotalRowCount, int.Parse(PageSize), currentPage);
        }
        else
        {
            btnRefreshReport_Click(null, null);
        }

    }

    protected void gvContact_OnPreRender(object sender, EventArgs e)
    {
        if (gvContact.Rows.Count > 0)
        {
            //This replaces <td> with <th> and adds the scope attribute
            gvContact.UseAccessibleHeader = true;

            //This will add the <thead> and <tbody> elements
            gvContact.HeaderRow.TableSection = TableRowSection.TableHeader;

        }
    }

    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            bindGrid(Convert.ToInt32(e.CommandArgument));
        }
    }

    public void generatePager(int totalRowCount, int pageSize, int currentPage)
    {
        int totalLinkInPage = 5;
        int totalPageCount = (int)Math.Ceiling((decimal)totalRowCount / pageSize);

        int startPageLink = Math.Max(currentPage - (int)Math.Floor((decimal)totalLinkInPage / 2), 1);
        int lastPageLink = Math.Min(startPageLink + totalLinkInPage - 1, totalPageCount);

        if ((startPageLink + totalLinkInPage - 1) > totalPageCount)
        {
            lastPageLink = Math.Min(currentPage + (int)Math.Floor((decimal)totalLinkInPage / 2), totalPageCount);
            startPageLink = Math.Max(lastPageLink - totalLinkInPage + 1, 1);
        }

        List<ListItem> pageLinkContainer = new List<ListItem>();

        if (startPageLink != 1)
            pageLinkContainer.Add(new ListItem("First", "1", currentPage != 1));
        for (int i = startPageLink; i <= lastPageLink; i++)
        {
            pageLinkContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage != i));
        }
        if (lastPageLink != totalPageCount)
            pageLinkContainer.Add(new ListItem("Last", totalPageCount.ToString(), currentPage != totalPageCount));

        dlPager.DataSource = pageLinkContainer;
        dlPager.DataBind();
    }

    public void bindGrid(int currentPage)
    {
        int _TotalRowCount = 0;

        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;
        string condition = string.Empty;
        string SearchGroup = string.Empty;
        string SearchCountry = string.Empty;

        if (ddlOptionType.SelectedIndex == 1)
        {
            SearchType = "Equal";
        }
        else if (ddlOptionType.SelectedIndex == 2)
        {
            SearchType = "Contains";
        }
        else
        {
            SearchType = "Like";
        }

        SearchField = ddlOptionField.SelectedValue.ToString();
        SearchValue = txtSearchValue.Text.Trim();

        if (!String.IsNullOrEmpty(txtCountryFilter.Text))
            SearchCountry = " Country_Name = '" + txtCountryFilter.Text + "'";

        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
            SearchGroup = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();

        if (!String.IsNullOrEmpty(SearchGroup))
            condition = SearchGroup;

        if (!String.IsNullOrEmpty(SearchGroup))
            condition = condition + " AND " + SearchCountry;
        else
            condition = SearchCountry;

        string PageSize = PageControlCommon.GetPageSize().ToString();
        // string DPageSize = ddlPageSize.SelectedValue.ToString();
        //if (int.Parse(DPageSize) > int.Parse(PageSize))
        //    PageSize = DPageSize;
        DataTable dtRecord = new DataTable();
        DataTable dt = new DataTable();
        string Usr_ID_Filter = "and Fav.User_Id = '" + Session["UserId"].ToString() + "'";
        if (ddlMyFav.SelectedValue.Trim() == "Favorite")
        {
            dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, true.ToString(), "true", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
            dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, false.ToString(), "true", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
        }
        else
        {
            dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, true.ToString(), "false", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
            dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, false.ToString(), "false", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
        }
        //dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, true.ToString(), "false", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
        //dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, currentPage.ToString(), PageSize, false.ToString(), "false", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);

        _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());

        lblTotalRecordNumber.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

        gvContact.DataSource = dt;
        gvContact.DataBind();


        generatePager(_TotalRowCount, int.Parse(PageSize), currentPage);

    }

    protected void SetCustomerTextBox(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.Text == "Supplier_Id")
        {
            txtSearchValue.Visible = false;
            txtCustValue.Visible = true;
            txtValueDate.Visible = false;
        }
        else if (ddlFieldName.Text == "Invoice_Date")
        {
            txtSearchValue.Visible = false;
            txtCustValue.Visible = false;
            txtValueDate.Visible = true;
        }
        else
        {
            txtSearchValue.Visible = true;
            txtCustValue.Visible = false;
            txtValueDate.Visible = false;
        }
        txtSearchValue.Text = "";
        txtCustValue.Text = "";
        txtValueDate.Text = "";
    }

    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;
        string condition = string.Empty;
        string SearchGroup = string.Empty;
        string SearchCountry = string.Empty;

        if (String.IsNullOrEmpty(txtCountryFilter.Text))
        {
            DisplayMessage("Please enter country name");
            txtCountryFilter.Focus();
            return;
        }

        if (ddlOptionType.SelectedIndex == 1)
        {
            SearchType = "Equal";
        }
        else if (ddlOptionType.SelectedIndex == 2)
        {
            SearchType = "Contains";
        }
        else
        {
            SearchType = "Like";
        }

        SearchField = ddlOptionField.SelectedValue.ToString();
        SearchValue = txtSearchValue.Text.Trim();

        if (SearchField == "CreatedEmployee")
        {

        }


        if (!String.IsNullOrEmpty(txtCountryFilter.Text))
            SearchCountry = " Country_Name = '" + txtCountryFilter.Text + "'";

        if (ddlGroupSearch.SelectedIndex != 0 && ddlGroupSearch.SelectedIndex != -1)
            SearchGroup = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();

        if (!String.IsNullOrEmpty(SearchGroup))
            condition = SearchGroup;

        if (!String.IsNullOrEmpty(SearchGroup))
            condition = condition + " AND " + SearchCountry;
        else
            condition = SearchCountry;


        string PageSize = PageControlCommon.GetPageSize().ToString();
        //string DPageSize = ddlPageSize.SelectedValue.ToString();
        //if (int.Parse(DPageSize) > int.Parse(PageSize))
        //    PageSize = DPageSize;
        string Usr_ID_Filter = "and Fav.User_Id = '" + Session["UserId"].ToString() + "'";
        DataTable dtRecord = new DataTable();
        DataTable dt = new DataTable();
        if (ddlMyFav.SelectedValue.Trim() == "Favorite")
        {
            dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, "1", PageSize, true.ToString(), "True", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
            dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, "1", PageSize, false.ToString(), "True", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
        }
        else
        {

            dtRecord = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, "1", PageSize, true.ToString(), "false", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
            dt = ObjContactMaster.GetContactTrueAllDataForGRidByIndex(condition, "1", PageSize, false.ToString(), "false", SearchField, SearchType, SearchValue, SearchGroup, Usr_ID_Filter);
        }

        gvContact.DataSource = dt;
        gvContact.DataBind();


        lblTotalRecordNumber.Text = Resources.Attendance.Total_Records + " : " + dtRecord.Rows[0][0].ToString() + "";

        int _TotalRowCount = int.Parse(dtRecord.Rows[0][0].ToString());
        generatePager(_TotalRowCount, int.Parse(PageSize), 1);

        //AllPageCode();

        if (txtSearchValue.Text != "")
            txtSearchValue.Focus();
        else if (txtCustValue.Text != "")
            txtCustValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();


    }

    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlGroupSearch.SelectedIndex = 0;
        ddlFieldName.SelectedIndex = 1;
        ddlOptionField.SelectedIndex = 0;
        ddlMyFav.SelectedIndex = 0;
        txtSearchValue.Text = "";
        txtCustValue.Text = "";
        txtCustValue.Visible = false;
        txtSearchValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        FillGrid();
        //AllPageCode();
    }





    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/Contact")))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/Contact"));
            }
            string path = Server.MapPath("~/CompanyResource/Contact/") + FULogoPath.FileName;
            FULogoPath.SaveAs(path);
            Session["Contactimgpath"] = FULogoPath.FileName;
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
        addaddress.fillGridAdd(hdnContactId.Value);
        txtAddressName.Focus();
        Update_New.Update();
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
            HttpContext.Current.Session["AddCtrl_Country_Id"] = ViewState["Country_Id"].ToString();
        }
        if (hdnContactId.Value == "")
        {
            addaddress.setCustomerID("NewCust");
        }
        else
        {
            addaddress.fillGridAdd(hdnContactId.Value);
            addaddress.setCustomerID(hdnContactId.Value);
        }
        addaddress.showListOrNot("No");
        addaddress.fillHeader(txtName.Text);
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
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open();", true);
        hdnAddressId.Value = e.CommandArgument.ToString();

        //user to fill address data on edit button click 
        addaddress.GetAddressInformationByAddressname(lblgvAddressName.Text);

        FillAddressDataTabelEdit();
        addaddress.FillLocationNCode();
        addaddress.fillGridAdd(hdnContactId.Value);
        addaddress.showListOrNot("No");
        addaddress.fillHeader(txtName.Text);
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

        //Update by Rahul Sharma when Automatic  Select Currency Select
        try
        {
            if (txtAddressName.Text != "")
            {
                ddlCurrency.SelectedValue = da.get_SingleValue("Select Currency_Id from Sys_Country_Currency inner join Set_AddressMaster on Set_AddressMaster.CountryId=Sys_Country_Currency.Country_Id where Address_Name = '" + txtAddressName.Text + "' And Set_AddressMaster.IsActive = '1'");
            }
        }
        catch (Exception ex)
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




    protected void btnGetSheet_Click(object sender, EventArgs e)
    {
        int fileType = 0;

        if (FU_Contact_Upload.HasFile)
        {
            string Path = string.Empty;
            string ext = FU_Contact_Upload.FileName.Substring(FU_Contact_Upload.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                FU_Contact_Upload.SaveAs(Server.MapPath("~/Temp/" + FU_Contact_Upload.FileName));
                Path = Server.MapPath("~/Temp/" + FU_Contact_Upload.FileName);

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
        if (DDl_Excel_Sheet == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (DDl_Excel_Sheet.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        DataTable Dt_Sheet = new DataTable();
        if (FU_Contact_Upload.HasFile)
        {
            string Path = string.Empty;
            string ext = FU_Contact_Upload.FileName.Substring(FU_Contact_Upload.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                FU_Contact_Upload.SaveAs(Server.MapPath("~/Temp/" + FU_Contact_Upload.FileName));
                Path = Server.MapPath("~/Temp/" + FU_Contact_Upload.FileName);
                Dt_Sheet = ConvetExcelToDataTable(Path, DDl_Excel_Sheet.SelectedValue.Trim());

                if (Dt_Sheet.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }
                Dt_Sheet.Columns.Add("Remark");
                Dt_Sheet.Columns.Add("Is_Valid");
                Dt_Sheet.Columns.Add("Address_Category_ID");
                Dt_Sheet.Columns.Add("Country_ID");
                Dt_Sheet.Columns.Add("Country_Code");
                Dt_Sheet.Columns.Add("Currency_ID");
                Dt_Sheet.Columns.Add("Department_ID");
                Dt_Sheet.Columns.Add("Designation_ID");
                Dt_Sheet.Columns.Add("Group_ID");

                Dt_Sheet.AcceptChanges();

                DataTable Dt_Contact = ObjContactMaster.GetContactAllData();
                DataTable Dt_Address_Category = ObjAddCat.GetAddressCategoryAll();
                DataTable Dt_Address_Name = ObjAddMaster.GetAddressAllData(Session["CompId"].ToString());
                DataTable Dt_Country = objCountryMaster.GetCountryMaster();
                DataTable Dt_Currency = objCurrency.GetCurrencyMaster();
                DataTable Dt_Department = ObjDepMaster.GetDepartmentMaster();
                DataTable Dt_Designation = ObjDesMaster.GetDesignationMaster();
                DataTable Dt_Group = objGroup.GetDistinctGroupName("");
                DataTable Dt_Customer = ObjCustomerMaster.GetCustomerAllData(Session["CompId"].ToString(), Session["BrandId"].ToString());
                DataTable Dt_Supplier = ObjSupplierMaster.GetSupplierAllData(Session["CompId"].ToString(), Session["BrandId"].ToString());

                DataTable Dt_Temp_Invalid = new DataTable();
                Dt_Temp_Invalid.Clear();
                for (int Sheet_Row = 0; Sheet_Row < Dt_Sheet.Rows.Count; Sheet_Row++)
                {
                    if (Dt_Sheet.Rows[Sheet_Row]["Remark"].ToString() == "")
                    {
                        Dt_Sheet.Rows[Sheet_Row]["Remark"] = "";
                        Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "Valid";
                    }
                    for (int D_Sheet_Row = Dt_Sheet.Rows.Count - 1; D_Sheet_Row > Sheet_Row; D_Sheet_Row--)
                    {
                        if (Dt_Sheet.Rows[Sheet_Row]["Contact_Code"].ToString() == Dt_Sheet.Rows[D_Sheet_Row]["Contact_Code"].ToString())
                        {
                            Dt_Sheet.Rows[D_Sheet_Row]["Remark"] = "Contact Code Already Exists In Excel Sheet";
                            Dt_Sheet.Rows[D_Sheet_Row]["Is_Valid"] = "InValid";
                            break;
                        }

                        else if (Dt_Sheet.Rows[Sheet_Row]["Contact_Name"].ToString() == Dt_Sheet.Rows[D_Sheet_Row]["Contact_Name"].ToString())
                        {
                            Dt_Sheet.Rows[D_Sheet_Row]["Remark"] = "Contact Name Already Exists In Excel Sheet";
                            Dt_Sheet.Rows[D_Sheet_Row]["Is_Valid"] = "InValid";
                            break;
                        }

                        else if (!string.IsNullOrEmpty(Dt_Sheet.Rows[Sheet_Row]["GSTIN_No"].ToString()))
                        {
                            if (Dt_Sheet.Rows[Sheet_Row]["GSTIN_No"].ToString() == Dt_Sheet.Rows[D_Sheet_Row]["GSTIN_No"].ToString())
                            {
                                Dt_Sheet.Rows[D_Sheet_Row]["Remark"] = "GSTIN No Already Exists In Excel Sheet";
                                Dt_Sheet.Rows[D_Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        else if (!string.IsNullOrEmpty(Dt_Sheet.Rows[Sheet_Row]["TRN_No"].ToString()))
                        {
                            if (Dt_Sheet.Rows[Sheet_Row]["TRN_No"].ToString() == Dt_Sheet.Rows[D_Sheet_Row]["TRN_No"].ToString())
                            {
                                Dt_Sheet.Rows[D_Sheet_Row]["Remark"] = "TRN No Already Exists In Excel Sheet";
                                Dt_Sheet.Rows[D_Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                    }

                    for (int D_Sheet_Row = Sheet_Row; D_Sheet_Row >= Sheet_Row; D_Sheet_Row--)
                    {
                        // For Contact Type                        
                        if (Dt_Sheet.Rows[Sheet_Row]["Contact_Type"].ToString().Trim() != "")
                        {
                            if (Dt_Sheet.Rows[Sheet_Row]["Contact_Type"].ToString().Trim() != "Individual" && Dt_Sheet.Rows[Sheet_Row]["Contact_Type"].ToString().Trim() != "Company")
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Contact Type Not Exists in Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Contact Code Active
                        if (Dt_Sheet.Rows[Sheet_Row]["Contact_Code"].ToString().Trim() != "")
                        {
                            DataTable Dt_Code_Check_Active = new DataTable();
                            Dt_Code_Check_Active = new DataView(Dt_Contact, "Code='" + Dt_Sheet.Rows[Sheet_Row]["Contact_Code"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Code_Check_Active != null && Dt_Code_Check_Active.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Contact Code Already Exists In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Contact Code Inactive
                        if (Dt_Sheet.Rows[Sheet_Row]["Contact_Code"].ToString().Trim() != "")
                        {
                            DataTable Dt_Code_Check_InActive = new DataTable();
                            Dt_Code_Check_InActive = new DataView(Dt_Contact, "Code='" + Dt_Sheet.Rows[Sheet_Row]["Contact_Code"].ToString() + "' and IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Code_Check_InActive != null && Dt_Code_Check_InActive.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Contact Code Already Exists In Database (Bin)";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Contact Name Active
                        if (Dt_Sheet.Rows[Sheet_Row]["Contact_Name"].ToString().Trim() != "")
                        {
                            DataTable Dt_Name_Check_Active = new DataTable();
                            Dt_Name_Check_Active = new DataView(Dt_Contact, "Name='" + Dt_Sheet.Rows[Sheet_Row]["Contact_Name"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Name_Check_Active != null && Dt_Name_Check_Active.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Contact Name Already Exists In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Contact Code Inactive
                        if (Dt_Sheet.Rows[Sheet_Row]["Contact_Name"].ToString().Trim() != "")
                        {
                            DataTable Dt_Name_Check_InActive = new DataTable();
                            Dt_Name_Check_InActive = new DataView(Dt_Contact, "Name='" + Dt_Sheet.Rows[Sheet_Row]["Contact_Name"].ToString() + "' and IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Name_Check_InActive != null && Dt_Name_Check_InActive.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Contact Name Already Exists In Database (Bin)";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Address Category
                        if (Dt_Sheet.Rows[Sheet_Row]["Address_Category"].ToString().Trim() != "")
                        {
                            DataTable Dt_Address_Category_Check = new DataTable();
                            Dt_Address_Category_Check = new DataView(Dt_Address_Category, "Address_Name='" + Dt_Sheet.Rows[Sheet_Row]["Address_Category"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Address_Category_Check == null || Dt_Address_Category_Check.Rows.Count == 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Address Category Not Found In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                            else
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Address_Category_ID"] = Dt_Address_Category_Check.Rows[0]["Address_Category_Id"].ToString();
                            }
                        }
                        //---------------------

                        // For Address Name
                        if (Dt_Sheet.Rows[Sheet_Row]["Address_Name"].ToString().Trim() != "")
                        {
                            DataTable Dt_Address_Name_Check = new DataTable();
                            Dt_Address_Name_Check = new DataView(Dt_Address_Name, "Address_Name='" + Dt_Sheet.Rows[Sheet_Row]["Address_Name"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Address_Name_Check != null && Dt_Address_Name_Check.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Address Name Already Exists In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Country
                        if (Dt_Sheet.Rows[Sheet_Row]["Country"].ToString().Trim() != "")
                        {
                            DataTable Dt_Country_Check = new DataTable();
                            Dt_Country_Check = new DataView(Dt_Country, "Country_Name='" + Dt_Sheet.Rows[Sheet_Row]["Country"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Country_Check == null || Dt_Country_Check.Rows.Count == 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Country Not Found In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                            else
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Country_ID"] = Dt_Country_Check.Rows[0]["Country_Id"].ToString();
                                Dt_Sheet.Rows[Sheet_Row]["Country_Code"] = Dt_Country_Check.Rows[0]["Country_Code"].ToString();
                            }
                        }
                        //---------------------

                        // For Currency
                        if (Dt_Sheet.Rows[Sheet_Row]["Currency"].ToString().Trim() != "")
                        {
                            DataTable Dt_Currency_Check = new DataTable();
                            Dt_Currency_Check = new DataView(Dt_Currency, "Currency_Name='" + Dt_Sheet.Rows[Sheet_Row]["Currency"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Currency_Check == null || Dt_Currency_Check.Rows.Count == 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Currency Not Found In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                            else
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Currency_ID"] = Dt_Currency_Check.Rows[0]["Currency_Id"].ToString();
                            }
                        }
                        //---------------------

                        // For Department
                        if (Dt_Sheet.Rows[Sheet_Row]["Department"].ToString().Trim() != "")
                        {
                            DataTable Dt_Department_Check = new DataTable();
                            Dt_Department_Check = new DataView(Dt_Department, "Dep_Name='" + Dt_Sheet.Rows[Sheet_Row]["Department"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Department_Check == null || Dt_Department_Check.Rows.Count == 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Department Not Found In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                            else
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Department_ID"] = Dt_Department_Check.Rows[0]["Dep_Id"].ToString();
                            }
                        }
                        //---------------------

                        // For Designation
                        if (Dt_Sheet.Rows[Sheet_Row]["Designation"].ToString().Trim() != "")
                        {
                            DataTable Dt_Designation_Check = new DataTable();
                            Dt_Designation_Check = new DataView(Dt_Designation, "Designation='" + Dt_Sheet.Rows[Sheet_Row]["Designation"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Designation_Check == null || Dt_Designation_Check.Rows.Count == 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Designation Not Found In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                            else
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Designation_ID"] = Dt_Designation_Check.Rows[0]["Designation_Id"].ToString();
                            }
                        }
                        //---------------------

                        // For Group
                        if (Dt_Sheet.Rows[Sheet_Row]["Group"].ToString().Trim() != "")
                        {
                            DataTable Dt_Group_Check = new DataTable();
                            Dt_Group_Check = new DataView(Dt_Group, "Group_Name='" + Dt_Sheet.Rows[Sheet_Row]["Group"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_Group_Check == null || Dt_Group_Check.Rows.Count == 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Group Not Found In Database";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                            else
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Group_ID"] = Dt_Group_Check.Rows[0]["Group_Id"].ToString();
                            }
                        }
                        //---------------------

                        // For Customer GST No
                        if (Dt_Sheet.Rows[Sheet_Row]["GSTIN_No"].ToString().Trim() != "")
                        {
                            DataTable Dt_GST_Cust_Check = new DataTable();
                            Dt_GST_Cust_Check = new DataView(Dt_Customer, "GSTIN_NO='" + Dt_Sheet.Rows[Sheet_Row]["GSTIN_No"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_GST_Cust_Check != null && Dt_GST_Cust_Check.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "GST No Already Exists In Customer";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Supplier GST No
                        if (Dt_Sheet.Rows[Sheet_Row]["GSTIN_No"].ToString().Trim() != "")
                        {
                            DataTable Dt_GST_Sup_Check = new DataTable();
                            Dt_GST_Sup_Check = new DataView(Dt_Supplier, "GSTIN_NO='" + Dt_Sheet.Rows[Sheet_Row]["GSTIN_No"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_GST_Sup_Check != null && Dt_GST_Sup_Check.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "GST No Already Exists In Supplier";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Customer TRN
                        if (Dt_Sheet.Rows[Sheet_Row]["TRN_No"].ToString().Trim() != "")
                        {
                            DataTable Dt_TRN_Cust_Check = new DataTable();
                            Dt_TRN_Cust_Check = new DataView(Dt_Customer, "TRN_No='" + Dt_Sheet.Rows[Sheet_Row]["TRN_No"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_TRN_Cust_Check != null && Dt_TRN_Cust_Check.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "TRN No Already Exists In Customer";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                        //---------------------

                        // For Supplier TRN
                        if (Dt_Sheet.Rows[Sheet_Row]["TRN_No"].ToString().Trim() != "")
                        {
                            DataTable Dt_TRN_Sup_Check = new DataTable();
                            Dt_TRN_Sup_Check = new DataView(Dt_Supplier, "TRN_No='" + Dt_Sheet.Rows[Sheet_Row]["TRN_No"].ToString() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                            if (Dt_TRN_Sup_Check != null && Dt_TRN_Sup_Check.Rows.Count > 0)
                            {
                                Dt_Sheet.Rows[Sheet_Row]["Remark"] = "TRN No Already Exists In Supplier";
                                Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                break;
                            }
                        }
                    }
                }
                Session["Dt_Sheet_Upload"] = Dt_Sheet;
                Div_Upload_Grid.Visible = true;
                GV_Sheet_Upload.DataSource = Dt_Sheet;
                GV_Sheet_Upload.DataBind();
                lbltotaluploadRecord.Text = "Total Record : " + (Dt_Sheet.Rows.Count).ToString();
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
    }

    protected void Rbt_All_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Dt_Sheet_Upload"];


        if (Rbt_Valid.Checked)
        {
            dt = new DataView(dt, "Is_Valid='Valid' or Is_Valid=''", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (Rbt_Invalid.Checked)
        {
            dt = new DataView(dt, "Is_Valid<>'Valid'", "", DataViewRowState.CurrentRows).ToTable();
        }

        GV_Sheet_Upload.DataSource = dt;
        GV_Sheet_Upload.DataBind();
        lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
    }

    protected void Btn_Upload_Sheet_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            DataTable Dt_Upload_Sheet = Session["Dt_Sheet_Upload"] as DataTable;
            DataTable Dt_Valid_Sheet = new DataView(Dt_Upload_Sheet, "Is_Valid='Valid'", "", DataViewRowState.CurrentRows).ToTable();
            int Affected_Rows = 0;
            for (int Dt_VS_Row = 0; Dt_VS_Row < Dt_Valid_Sheet.Rows.Count; Dt_VS_Row++)
            {
                string Contact_Saved = "";
                Contact_Saved = Contact_Name_Save(Dt_Valid_Sheet, Dt_VS_Row, ref trns);
                if (Contact_Saved == "True")
                    Affected_Rows++;
            }
            if (Affected_Rows != 0)
                DisplayMessage(Affected_Rows + " Rows inserted");
            else
                DisplayMessage("No Rows inserted");

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnBackToMapData_Click(null, null);
            FillGrid();
            Update_List.Update();
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

    protected void btnBackToMapData_Click(object sender, EventArgs e)
    {
        Div_Upload_Grid.Visible = false;
        GV_Sheet_Upload.DataSource = null;
        GV_Sheet_Upload.DataBind();
        lbltotaluploadRecord.Text = "";
        DDl_Excel_Sheet.Items.Clear();
        FU_Contact_Upload.Dispose();
        FU_Contact_Upload.Attributes.Clear();
    }

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
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }


        Session["cnn"] = strcon;

        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();

        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        DDl_Excel_Sheet.DataSource = tables;

        DDl_Excel_Sheet.DataTextField = "TABLE_NAME";
        DDl_Excel_Sheet.DataValueField = "TABLE_NAME";
        DDl_Excel_Sheet.DataBind();
        conn.Close();
    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
        if (FU_Contact_Upload.HasFile)
        {
            string ext = FU_Contact_Upload.FileName.Substring(FU_Contact_Upload.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                FU_Contact_Upload.SaveAs(Server.MapPath("~/Temp/" + FU_Contact_Upload.FileName));
            }
        }
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

    public string Contact_Name_Save(DataTable Dt_Valid_Sheet, int Dt_VS_Row, ref SqlTransaction trns)
    {
        string Is_Save = "";
        int b = 0;
        string strIsEmail = string.Empty;
        string strIsSMS = string.Empty;
        string strDepartmentId = string.Empty;
        string strDesignationId = string.Empty;
        string strReligionId = string.Empty;
        string strCurrencyId = string.Empty;
        string Status = string.Empty;
        string strNamePrefix = string.Empty;
        string Country_Id = string.Empty;
        string DefaultEmailId = string.Empty;
        string CountryCodewithMobileNumber = string.Empty;
        string StrComp = Session["CompID"].ToString();
        string strParentContactId = string.Empty;

        //Set Department
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Department"].ToString().Trim() == "")
            strDepartmentId = "0";
        else
            strDepartmentId = Dt_Valid_Sheet.Rows[Dt_VS_Row]["Department_ID"].ToString().Trim();
        //----------------

        //Set Designation
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Designation"].ToString().Trim() == "")
            strDesignationId = "0";
        else
            strDesignationId = Dt_Valid_Sheet.Rows[Dt_VS_Row]["Designation_ID"].ToString().Trim();
        //----------------

        strReligionId = "0";

        //Set Department
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Currency"].ToString().Trim() == "")
            strCurrencyId = "0";
        else
            strCurrencyId = Dt_Valid_Sheet.Rows[Dt_VS_Row]["Currency_ID"].ToString().Trim();
        //----------------

        strIsEmail = "False";
        strIsSMS = "False";

        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Type"].ToString().Trim() == "Individual")
        {
            Status = "Individual";
            strNamePrefix = "0";
        }
        else if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Type"].ToString().Trim() == "Company")
        {
            Status = "Company";
            strNamePrefix = "0";
        }
        else
        {
            Status = "Individual";
            strNamePrefix = "0";
        }

        //Country
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country"].ToString().Trim() == "")
            Country_Id = "0";
        else
            Country_Id = Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_ID"].ToString().Trim();



        strParentContactId = "0";


        //if (txtPermanentMobileNo.Text != "")
        //{
        CountryCodewithMobileNumber = "+" + Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_Code"].ToString().Trim() + "-" + Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_No"].ToString().Trim();
        //}

        b = ObjContactMaster.InsertContactMaster(Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Code"].ToString().Trim(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Name"].ToString().Trim(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Name"].ToString().Trim(), "", strDepartmentId, strDesignationId, strReligionId, "0", strIsEmail, strIsSMS, Status.ToString(), "False", Dt_Valid_Sheet.Rows[Dt_VS_Row]["Email_ID"].ToString().Trim(), CountryCodewithMobileNumber, strNamePrefix.ToString(), Country_Id, strCurrencyId, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", strParentContactId, ref trns);

        if (b != 0)
        {
            Is_Save = "True";
            Address_Save(Dt_Valid_Sheet, Dt_VS_Row, ref trns, b.ToString());
            Email_Save(Dt_Valid_Sheet, Dt_VS_Row, ref trns, b.ToString());
            Group_Save(Dt_Valid_Sheet, Dt_VS_Row, ref trns, b.ToString());
        }
        return Is_Save;
    }

    public void Address_Save(DataTable Dt_Valid_Sheet, int Dt_VS_Row, ref SqlTransaction trns, string Contact_Id)
    {
        string AddressMailId = string.Empty;
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Address_Category"].ToString().Trim() != "" && Dt_Valid_Sheet.Rows[Dt_VS_Row]["Address_Name"].ToString().Trim() != "" && Dt_Valid_Sheet.Rows[Dt_VS_Row]["Address"].ToString().Trim() != "")
        {
            int country_id = 0;
            if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_ID"].ToString().Trim() != "")
                country_id = Convert.ToInt32(Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_ID"].ToString().Trim());
            int Address_ID = ObjAddMaster.InsertAddressMaster(Dt_Valid_Sheet.Rows[Dt_VS_Row]["Address_Category_ID"].ToString().Trim(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["Address_Name"].ToString().Trim(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["Address"].ToString().Trim(), "", "", "", country_id.ToString(), "", "", "", "", "", "+" + Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_Code"].ToString().Trim() + "-" + Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_No"].ToString().Trim(), "", Dt_Valid_Sheet.Rows[Dt_VS_Row]["Email_ID"].ToString().Trim(), "", "", "", "0.00", "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            objAddChild.InsertAddressChild(Address_ID.ToString(), "Contact", Contact_Id, "True", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
    }

    public void Email_Save(DataTable Dt_Valid_Sheet, int Dt_VS_Row, ref SqlTransaction trns, string Contact_Id)
    {
        string AddressMailId = string.Empty;
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Email_ID"].ToString().Trim() != "")
        {
            int country_id = 0;
            if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_ID"].ToString().Trim() != "")
                country_id = Convert.ToInt32(Dt_Valid_Sheet.Rows[Dt_VS_Row]["Country_ID"].ToString().Trim());
            objEmailHeader.ES_EmailMasterHeader_Insert(Dt_Valid_Sheet.Rows[Dt_VS_Row]["Email_ID"].ToString().Trim(), country_id.ToString(), "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
    }

    public void Group_Save(DataTable Dt_Valid_Sheet, int Dt_VS_Row, ref SqlTransaction trns, string Contact_Id)
    {
        bool IsBlock = false;
        string strBlockReason = string.Empty;
        string IsCredit = "False";
        string Strstatus = "Approved";
        DataTable dtCustomerInfo = ObjCustomerMaster.GetCustomerAllDataByCustomerIdWithOutBrand(Session["CompId"].ToString(), Contact_Id, ref trns);
        if (dtCustomerInfo.Rows.Count > 0)
        {
            IsBlock = Convert.ToBoolean(dtCustomerInfo.Rows[0]["Is_Block"].ToString());
            strBlockReason = dtCustomerInfo.Rows[0]["Block_Reason"].ToString();
            IsCredit = dtCustomerInfo.Rows[0]["Field4"].ToString();
            Strstatus = dtCustomerInfo.Rows[0]["Field5"].ToString();
        }
        DataTable dtSupplierInfo = ObjSupplierMaster.GetSupplierAllDataBySupplierIdWithoutBrand(Session["CompId"].ToString(), Contact_Id, ref trns);
        if (dtCustomerInfo.Rows.Count > 0)
        {
            IsCredit = dtCustomerInfo.Rows[0]["Field4"].ToString();
            Strstatus = dtCustomerInfo.Rows[0]["Field5"].ToString();
        }
        string Id = string.Empty;
        string strType = string.Empty;
        bool IsAllow = false;
        bool IsAllowGroup = false;
        bool IsCustomer = false;
        bool IsSupplier = false;
        bool Isselectgroup = false;
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Group"].ToString().Trim() == "Customer")
        {
            IsCustomer = true;
            IsAllowGroup = true;
            IsAllow = true;
            strType = "Customer";
        }
        else if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Group"].ToString().Trim() == "Supplier")
        {
            IsSupplier = true;
            IsAllowGroup = true;
            IsAllow = true;
            strType = "Supplier";
        }
        else if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Group"].ToString().Trim() == "")
        {
            IsAllowGroup = false;
            IsAllow = false;
        }

        //for Customer & Supplier Account
        string strReceiveVoucherAcc = string.Empty;
        DataTable dtParam = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Receive Vouchers", ref trns);
        if (dtParam.Rows.Count > 0)
        {
            strReceiveVoucherAcc = dtParam.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strReceiveVoucherAcc = "0";
        }
        string strPaymentVoucherAcc = string.Empty;
        DataTable dtPaymentVoucher = objAccParameter.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Payment Vouchers", ref trns);
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPaymentVoucherAcc = "0";
        }

        bool Check;

        Check = true;
        DataTable dt = ObjBrandMaster.GetBrandMaster(Session["CompId"].ToString(), ref trns);
        if ((new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows.Count != 0)
        {
            Check = false;
            if (strType == "Customer")
            {
                ObjCustomerMaster.DeletePermanentbyCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id, ref trns);
                ObjCustomerMaster.InsertCustomerMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id, strReceiveVoucherAcc, "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", IsBlock.ToString(), strBlockReason, "", "", "", IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["GSTIN_No"].ToString().Trim(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["TRN_No"].ToString().Trim(), "0", "", ref trns);
            }
            else if (strType == "Supplier")
            {
                if (!ObjContactMaster.IsUsedInInventory(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id, "2", "2", ref trns))
                {
                    ObjSupplierMaster.DeletePermanentbySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id.ToString(), ref trns);
                    ObjSupplierMaster.InsertSupplierMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id.ToString(), strPaymentVoucherAcc, "0", "0", "0", "0", "0", "0", txtsupplierTinNo.Text, txtsupplierCstNo.Text, ddlCurrency.SelectedValue, IsCredit, Strstatus, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtGSTIN.Text.Trim(), Txt_TRN_No.Text.Trim(), ref trns);
                }
                else
                {
                    string strSQL = "UPDATE Set_Suppliers Set GSTIN_NO='" + txtGSTIN.Text.Trim() + "', TRN_No='" + Txt_TRN_No.Text.Trim() + "' Where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Supplier_Id=" + Contact_Id.ToString() + "";
                    int v = da.execute_Command(strSQL, ref trns);
                }
            }
        }
        else
        {
            if (strType == "Customer")
            {
                if (ObjCustomerMaster.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id, ref trns).Rows.Count != 0)
                {
                    ObjCustomerMaster.DeletePermanentbyCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id.ToString(), ref trns);
                }
            }
            else if (strType == "Supplier")
            {
                if (ObjSupplierMaster.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id, ref trns).Rows.Count != 0)
                {
                    ObjSupplierMaster.DeletePermanentbySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Contact_Id.ToString(), ref trns);
                }
            }
        }
        ObjCompanyBrand.DeleteContactCompanyBrand(Contact_Id.Trim(), ref trns);
        ObjCompanyBrand.InsertContactCompanyBrand(Contact_Id.Trim(), Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);
        objCG.DeleteContactGroup(Contact_Id, ref trns);
        if (Dt_Valid_Sheet.Rows[Dt_VS_Row]["Group_ID"].ToString().Trim() != "")
            objCG.InsertContactGroup(Contact_Id, Dt_Valid_Sheet.Rows[Dt_VS_Row]["Group_ID"].ToString().Trim(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Code"].ToString().Trim(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);
        else
            objCG.InsertContactGroup(Contact_Id, "0", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Dt_Valid_Sheet.Rows[Dt_VS_Row]["Contact_Code"].ToString().Trim(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), ref trns);
    }


    protected void iBtnAlert_Command(object sender, CommandEventArgs e)
    {
        string transid = e.CommandArgument.ToString();
        DataTable DtAddress = new DataTable();
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(e.CommandName.ToString());
        string data = "";
        if (DtAddress.Rows.Count > 0)
        {
            DataTable dt_ContactNodata = objContactnoMaster.getDataByPKID(DtAddress.Rows[0]["Trans_Id"].ToString(), "Set_AddressMaster");

            if (dt_ContactNodata.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_ContactNodata.Rows)
                {
                    data = data + dr["Type"].ToString() + ":" + dr["Phone_no"].ToString() + " ";
                }
            }
        }

        if (data.Trim() != "")
            DisplayMessage(data);
    }

    protected void BtnMoreNumber_Command(object sender, CommandEventArgs e)
    {

        DataTable DtAddress = new DataTable();
        DtAddress = ObjAddMaster.GetAddressDataByAddressName(e.CommandArgument.ToString());
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
        DataTable DtAddress = ObjAddMaster.GetAddressDataByAddressName(data);
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


    public string getMoreNumber(string Trans_id)
    {
        DataTable Dt_contactNoData = objContactnoMaster.getDataByPKID(Trans_id, "Ems_ContactMaster");
        if (Dt_contactNoData.Rows.Count > 1)
        {
            return "More";
        }
        else
        {
            return "";
        }
    }

    protected void lBtnMoreNum_Command(object sender, CommandEventArgs e)
    {

        string data = "";
        DataTable dt_ContactNodata = objContactnoMaster.getDataByPKID(e.CommandArgument.ToString(), "Ems_ContactMaster");

        if (dt_ContactNodata.Rows.Count > 0)
        {
            foreach (DataRow dr in dt_ContactNodata.Rows)
            {
                data = data + "<b>" + dr["Type"].ToString() + "</b>:" + dr["Phone_no"].ToString() + " <br/>";
            }
        }

        if (data.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_Number_Open('" + data + "');", true);
        }
    }


    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        FUpload1.setID(e.CommandArgument.ToString(), "Contact", "EMS", "Contact", e.CommandName.ToString(), ((Label)gvrow.FindControl("lblName")).Text + "(" + e.CommandName.ToString() + ")");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion


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
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
    }

    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, gvContact, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(gvContact, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

}


