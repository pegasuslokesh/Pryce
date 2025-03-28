using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using PegasusDataAccess;

public partial class MasterSetUp_SupplierMaster : BasePage
{
    #region defined Class Object
    CompanyMaster ObjCompany = null;
    Ac_ChartOfAccount objCOf = null;
    Ac_SubChartOfAccount objSubCOA = null;
    Common cmn = null;
    Set_Suppliers objSupplier = null;
    Set_AddressChild objAddChild = null;
    CountryMaster ObjCountry = null;
    Set_AddressMaster objAddMaster = null;
    Ems_ContactMaster objContact = null;
    SystemParameter ObjSysParam = null;
    DepartmentMaster objDepartment = null;
    ReligionMaster objReligion = null;
    DesignationMaster objDesg = null;
    CurrencyMaster objCurrency = null;
    PurchaseOrderHeader objPoheader = null;
    Set_BankMaster objBankMaster = null;
    Contact_BankDetail objContactBankDetail = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    Contact_PriceList objContactPriceList = null;
    Ems_GroupMaster objGroup = null;
    //For Arcawing
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Document_Master ObjDocument = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    Ac_AccountMaster objAccMaster = null;
    Ac_Ageing_Detail objAcAgeing = null;
    DataAccessClass da = null;
    PageControlCommon objPageCmn = null;
    //end
    Ac_AccountMaster objAcAccountMaster = null;
    CountryMaster objCountryMaster = null;
    //For Multiple Logos
    Set_CustomerSupplier_Logo objCustSupLogo = null;
    //end code
    Set_Approval_Employee objEmpApproval = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Finance_Year_Info objFYI = null;
    Ems_Contact_ProductCategory ObjContactProductcategory = null;
    NotificationMaster Obj_Notifiacation = null;
    EmployeeMaster objEmployee = null;
    ContactNoMaster objContactnoMaster = null;
    
    public const int grdDefaultColCount = 4;
    private const string strPageName = "SupplierMaster";
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        hdntxtaddressid.Value = txtAddressName.ID;

        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objCOf = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objReligion = new ReligionMaster(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objPoheader = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        objContactBankDetail = new Contact_BankDetail(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objContactPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objAccMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objAcAgeing = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCustSupLogo = new Set_CustomerSupplier_Logo(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        ObjContactProductcategory = new Ems_Contact_ProductCategory(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        //ViewState["ImageClick"] = "0";
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetup/SupplierMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
         
            FillGroup();

            Session["DtContactBankSupplier"] = null;
            Session["DtCustomerPriceList"] = null;

            Session["AddCtrl_State_Id"] = "";
            Session["AddCtrl_Country_Id"] = "";

            BtnShowCpriceList.Visible = false;
            Session["dtArcaWing"] = null;
            FillCurrencyDDL();
            ViewState["ImageClick"] = "0";

            gvFileMaster.DataSource = null;
            gvFileMaster.DataBind();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

            Session["Long"] = null;
            Session["Lati"] = null;


            FillProductCategory();
            FillCountryCode();
            FillLocation();
            FillGrid();
            getPageControlsVisibility();
            try
            {
             string strCurrencyId = Session["LocCurrencyId"].ToString();
                if (strCurrencyId != "0" && strCurrencyId != "")
                {
                    ddlBankCurrency.SelectedValue = strCurrencyId;
                    FillCountryCode(ddlAccount1ContactNo_CountryCode, strCurrencyId);
                    FillCountryCode(ddlAccount1FaxNo_CountryCode, strCurrencyId);
                }
            }
            catch
            {

            }
        }
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
            }
        }

        return txt;
    }
    public void FillCountryCode(DropDownList DDlCountryCode, string strCurrencyId)
    {
        DataTable dt = objCountryMaster.getCountryCallingCode();
        if (dt.Rows.Count > 0)
        {

            DDlCountryCode.DataSource = dt;
            DDlCountryCode.DataTextField = "CountryCodeName";
            DDlCountryCode.DataValueField = "CountryCodeValue";
            DDlCountryCode.DataBind();


            try
            {
                DDlCountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
            }
            catch
            {
            }
        }
    }
    public string SetDecimal(string amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }

    public string getImageUrl(object ImageName)
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
    protected void txtTransPerson_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string strEmployeeId = string.Empty;
            if (textTransEmp.Text != "")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = textTransEmp.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                strEmployeeId = Emp_ID;
                if (strEmployeeId != "" && strEmployeeId != "0")
                {

                }
                else
                {
                    DisplayMessage("Select Employee In Suggestions Only");
                    textTransEmp.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(textTransEmp);
                }
            }
        }
        catch(Exception ex)
        {
            DisplayMessage("Select Employee In Suggestions Only");
            textTransEmp.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(textTransEmp);

        }

    }
    protected void gvBankDetails_RowCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            DataTable dt = (DataTable)ViewState["BankDetails"];

            if (rowIndex >= 0 && rowIndex < dt.Rows.Count)
            {
                // Remove the row at the specified index
                dt.Rows.RemoveAt(rowIndex);
                if (dt.Rows.Count > 0)
                {
                    // Bind the updated DataTable to the GridView
                    gvBankDetails.DataSource = dt;
                    gvBankDetails.DataBind();
                }
                else
                {
                    // Bind the updated DataTable to the GridView
                    gvBankDetails.DataSource = null;
                    gvBankDetails.DataBind();
                }

                // Store the DataTable in ViewState to persist data on postback
                ViewState["BankDetails"] = dt;
            }
        }
        else if (e.CommandName == "Edit")
        {
            DataTable dt = (DataTable)ViewState["BankDetails"];
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            hdnbankEditId.Value = rowIndex.ToString();
            if (rowIndex >= 0 && rowIndex < dt.Rows.Count)
            {
                DataRow rowToEdit = dt.Rows[rowIndex];
                // Populate your form fields with data from the DataRow
                
                ddlBankCurrency.SelectedValue = rowToEdit["Currency"].ToString();
                textTransEmp.Text = rowToEdit["trnsEmpName"].ToString() + "/" + rowToEdit["trnsEmpId"].ToString();
                txtExcnageName.Text = rowToEdit["ExchangeName"].ToString();
                txtAccountName1.Text = rowToEdit["AccountName"].ToString();
                txtBankSupplerAddress.Text = rowToEdit["BeneFeciaryAddress"].ToString();
                txtAccountNo1.Text = rowToEdit["AccountNo"].ToString();
                txtIfscCode.Text = rowToEdit["IFSCCode"].ToString();
                txtibanCode.Text = rowToEdit["IBANCode"].ToString();
                txtSwiftCode.Text = rowToEdit["SWIFTCode"].ToString();
                txtAccountbankerName1.Text = rowToEdit["BankerName"].ToString();
                txtAccountContactNo1.Text = rowToEdit["ContactNo"].ToString();
                txtAccountbankerAddress1.Text= rowToEdit["BankerAddress"].ToString();
                txtAccountFaxNo1.Text = rowToEdit["FaxNo"].ToString();
            }
            dt.Dispose();
        }
    }
    protected void gvBankDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        // Exit edit mode
        gvBankDetails.EditIndex = -1;
        // Rebind the GridView with updated data
        DataTable dt = (DataTable)ViewState["BankDetails"];
        if (dt.Rows.Count > 0)
        {
            gvBankDetails.DataSource = dt;
            gvBankDetails.DataBind();
        }
        else
        {
            gvBankDetails.DataSource = null;
            gvBankDetails.DataBind();
        }
    }

    protected void btnAddBankDetail_Click(object sender, EventArgs e)
    {
        if (txtAccountName1.Text == "")
        {
            DisplayMessage("Please fill Account Name");
            return;
        }
        if (txtAccountNo1.Text == "")
        {
            DisplayMessage("Please fill Account No.");
            return;
        }
        // Get data from form fields
        string currency = ddlBankCurrency.SelectedValue;
        string CurrencyName = ddlBankCurrency.SelectedItem.Text;
        string accountName = txtAccountName1.Text;
        string accountNo = txtAccountNo1.Text;
        string ifscCode = txtIfscCode.Text;
        string ibanCode = txtibanCode.Text;
        string swiftCode = txtSwiftCode.Text;
        string bankerName = txtAccountbankerName1.Text;
        string bankerAddress = txtAccountbankerAddress1.Text;
        string contactNo = ddlAccount1ContactNo_CountryCode.SelectedValue + txtAccountContactNo1.Text;
        string faxNo = ddlAccount1FaxNo_CountryCode.SelectedValue + txtAccountFaxNo1.Text;
        string trnsEmpName = textTransEmp.Text.Trim().Split('/')[0].ToString();
        string trnsEmpId = textTransEmp.Text.Trim().Split('/')[1].ToString();
        string Exchangename = txtExcnageName.Text;
        string BeneFeciaryAddress = txtBankSupplerAddress.Text;
        // Create a DataTable to store the data
        DataTable dt = new DataTable();
          if (ViewState["BankDetails"] != null)
            {
                dt = (DataTable)ViewState["BankDetails"];
            }
            else
            {
                // Define columns for the DataTable
                dt.Columns.Add("Currency");
                dt.Columns.Add("CurrencyName");
                dt.Columns.Add("AccountName");
                dt.Columns.Add("AccountNo");
                dt.Columns.Add("IFSCCode");
                dt.Columns.Add("IBANCode");
                dt.Columns.Add("SWIFTCode");
                dt.Columns.Add("BankerName");
                dt.Columns.Add("BankerAddress");
                dt.Columns.Add("ContactNo");
                dt.Columns.Add("FaxNo");
                dt.Columns.Add("trnsEmpId");
                dt.Columns.Add("trnsEmpName");
                dt.Columns.Add("ExchangeName");
                dt.Columns.Add("BeneFeciaryAddress");
            }
        if (hdnbankEditId.Value == "")
        {
            // Create a new row and add data to it
            DataRow dr = dt.NewRow();
            dr["Currency"] = currency;
            dr["CurrencyName"] = CurrencyName;
            dr["AccountName"] = accountName;
            dr["AccountNo"] = accountNo;
            dr["IFSCCode"] = ifscCode;
            dr["IBANCode"] = ibanCode;
            dr["SWIFTCode"] = swiftCode;
            dr["BankerName"] = bankerName;
            dr["BankerAddress"] = bankerAddress;
            dr["ContactNo"] = contactNo;
            dr["FaxNo"] = faxNo;
            dr["trnsEmpId"] = trnsEmpId;
            dr["trnsEmpName"] = trnsEmpName;
            dr["ExchangeName"] = Exchangename;
            dr["BeneFeciaryAddress"] = BeneFeciaryAddress;
            // Add the DataRow to the DataTable 
            dt.Rows.Add(dr);
        }
        else
        {
            int rowIndex = int.Parse(hdnbankEditId.Value);
            if (rowIndex >= 0 && rowIndex < dt.Rows.Count)
            {
                // Update the existing row with new data
                DataRow rowToUpdate = dt.Rows[rowIndex];
                rowToUpdate["Currency"] = currency;
                rowToUpdate["CurrencyName"] = CurrencyName;
                rowToUpdate["AccountName"] = accountName;
                rowToUpdate["AccountNo"] = accountNo;
                rowToUpdate["IFSCCode"] = ifscCode;
                rowToUpdate["IBANCode"] = ibanCode;
                rowToUpdate["SWIFTCode"] = swiftCode;
                rowToUpdate["BankerName"] = bankerName;
                rowToUpdate["BankerAddress"] = bankerAddress;
                rowToUpdate["ContactNo"] = contactNo;
                rowToUpdate["FaxNo"] = faxNo;
                rowToUpdate["trnsEmpId"] = trnsEmpId;
                rowToUpdate["trnsEmpName"] = trnsEmpName;
                rowToUpdate["ExchangeName"] = Exchangename;
                rowToUpdate["BeneFeciaryAddress"] = BeneFeciaryAddress;
            }

        }
        // Bind the DataTable to the GridView
        gvBankDetails.DataSource = dt;
        gvBankDetails.DataBind();
        hdnbankEditId.Value = "";
        // Store the DataTable in ViewState to persist data on postback
        ViewState["BankDetails"] = dt;

        // Clear form fields
        ClearBankFields();
    }
    public void ClearBankFields()
    {
        txtAccountName1.Text = "";
        txtAccountNo1.Text = "";
        txtIfscCode.Text = "";
        txtibanCode.Text = "";
        txtSwiftCode.Text = "";
        txtAccountbankerName1.Text = "";
        txtAccountContactNo1.Text = "";
        txtAccountFaxNo1.Text = "";
        txtAccountName1.Focus();
        txtAccountbankerAddress1.Text = "";
    }
    public void FillAccountCurrency(string SupplierId)
    {
        DataTable dt = da.return_DataTable("Select CM.Currency_ID,CM.Currency_Name From Ac_AccountMaster as AM inner join Sys_CurrencyMaster as CM on CM.Currency_ID=AM.Currency_Id where AM.Ref_Id='"+SupplierId+ "' And AM.Is_Active='1'  And AM.Ref_Type='Supplier'");
        objPageCmn.FillData((object)ddlBankCurrency, dt, "Currency_Name", "Currency_Id");
    }
    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            objPageCmn.FillData((object)ddlCurrencyCreditLimit, dt, "Currency_Name", "Currency_Id");
            objPageCmn.FillData((object)ddlBankAcCurrency, dt, "Currency_Name", "Currency_Id");           
        }
        else
        {

            try
            {
                ddlCurrencyCreditLimit.Items.Insert(0, "--Select--");
                ddlCurrencyCreditLimit.SelectedIndex = 0;
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
                ddlBankAcCurrency.Items.Insert(0, "--Select--");
                ddlBankAcCurrency.SelectedIndex = 0;
            }
            catch
            {
                ddlCurrencyCreditLimit.Items.Insert(0, "--Select--");
                ddlCurrencyCreditLimit.SelectedIndex = 0;
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
                ddlBankAcCurrency.Items.Insert(0, "--Select--");
                ddlBankAcCurrency.SelectedIndex = 0;
            }
        }
    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSupplierSave.Visible = clsPagePermission.bAdd;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
	TabBankDetail.Visible = clsPagePermission.bVerify;

        BtnDocumentAdd.Visible = clsPagePermission.bUpload;
        gvFileMaster.Columns[1].Visible = clsPagePermission.bDownload;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        imgAddAddressName.Visible = clsPagePermission.bAdd;
        btnAddNewAddress.Visible = clsPagePermission.bAdd;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    #endregion
    #region System defined Function

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string strSender = ((LinkButton)sender).ID;
        string strCurrency = Session["LocCurrencyId"].ToString();
        hdnSupplierId.Value = e.CommandArgument.ToString();
        FillAccountCurrency(hdnSupplierId.Value);
        DataTable dtSupplierEdit = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value);
        if (strSender.Trim() == "imgBtnDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            btnSupplierSave.Visible = false;
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            btnSupplierSave.Visible = true;
        }
        if (dtSupplierEdit.Rows.Count > 0)
        {
            BindDocumentList();
            txtId.Text = dtSupplierEdit.Rows[0]["Supplier_Code"].ToString();
            txtSupplierName.Text = dtSupplierEdit.Rows[0]["Name"].ToString();
            txtLSupplierName.Text = dtSupplierEdit.Rows[0]["Name_L"].ToString();
            txtCivilId.Text = dtSupplierEdit.Rows[0]["Civil_Id"].ToString();
            //start vs

            ViewState["Image"] = dtSupplierEdit.Rows[0]["ImagePath"].ToString();
            ViewState["SupplierCode"] = dtSupplierEdit.Rows[0]["Supplier_Code"].ToString();
            ViewState["Status"] = dtSupplierEdit.Rows[0]["Field5"].ToString();
            //end vs code
            if (dtSupplierEdit.Rows[0]["Field1"].ToString().Trim() == "Y")
            {
                BtnShowCpriceList.Visible = true;
                chkSpriceList.Checked = true;
                DataTable DtCPrice = new DataTable();
                DtCPrice.Columns.Add("Product_Id");
                DtCPrice.Columns.Add("Sales_Price");

                DataTable dtcustomerPriceList = objContactPriceList.GetContactPriceList(Session["CompId"].ToString(), e.CommandArgument.ToString(), "S");
                for (int i = 0; i < dtcustomerPriceList.Rows.Count; i++)
                {
                    DataRow dr = DtCPrice.NewRow();
                    dr["Product_Id"] = dtcustomerPriceList.Rows[i]["Product_Id"].ToString();
                    dr["Sales_Price"] = dtcustomerPriceList.Rows[i]["Sales_Price"].ToString();
                    DtCPrice.Rows.Add(dr);
                }
                if (DtCPrice.Rows.Count > 0)
                {
                    Session["DtCustomerPriceList"] = DtCPrice;
                }
                else
                {
                    Session["DtCustomerPriceList"] = null;
                }
            }
            else
            {
                BtnShowCpriceList.Visible = false;
                chkSpriceList.Checked = false;
            }

            //For Accounts data
            string strSupplierAccount = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
          

            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Supplier", hdnSupplierId.Value);
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
                //AllPageCode();
            }

            try
            {
                chkIsNonRegistered.Checked = Convert.ToBoolean(dtSupplierEdit.Rows[0]["Field2"].ToString());
            }
            catch
            {
                chkIsNonRegistered.Checked = false;
            }

            try
            {
                ddlCurrency.SelectedValue = dtSupplierEdit.Rows[0]["Field3"].ToString();
            }
            catch
            {
                ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
            }

            if (dtSupplierEdit.Rows[0]["Account_No"].ToString() != "0")
            {
                txtAccountNo.Text = GetAccountName(dtSupplierEdit.Rows[0]["Account_No"].ToString()).ToString() + "/" + dtSupplierEdit.Rows[0]["Account_No"].ToString();
            }
            else
            {
                txtAccountNo.Text = GetAccountName(strSupplierAccount).ToString() + "/" + strSupplierAccount;
            }




            //code for disable opening balance gridview in case of  previous financial year is exist

            //code by jitendra upadhyay on 06-03-2017

            DataTable dtFY = objFYI.GetInfoAllTrue(Session["CompId"].ToString());
            if (dtFY.Rows.Count > 0)
            {
                if (dtFY.Rows[0]["Trans_Id"].ToString() != Session["FinanceYearId"].ToString())
                {
                    GvLocation.Enabled = false;
                }

            }


            //for get selected product category in editable mode
            //code created by jitendra upadhyay pn 26-09-2016
            FillProductCategory();
            DataTable dtProductCate = ObjContactProductcategory.GetDateBy_ContactId_and_ContactType(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value, "S");
            for (int i = 0; i < dtProductCate.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dtProductCate.Rows[i]["CategoryId"].ToString();
                li.Text = ObjProductCateMaster.GetProductCategoryByCategoryId(Session["CompId"].ToString(), dtProductCate.Rows[i]["CategoryId"].ToString()).Rows[0]["Category_Name"].ToString();
                //lstSelectProductCategory.Items.Add(li);
                //lstProductCategory.Items.Remove(li);
                foreach (ListItem lival in lstProductCategory.Items)
                {
                    if (lival.Value == li.Value)
                    {
                        lival.Selected = true;
                        break;
                    }
                }
            }

            //here we will selectall credit information 
            try
            {
                //ddlIsCredit.SelectedValue = dtSupplierEdit.Rows[0]["Field4"].ToString();
            }
            catch
            {
                ddlIsCredit.SelectedValue = "False";
            }

         
            txtDebitAmount.Text = dtSupplierEdit.Rows[0]["Db_Amount"].ToString();
            txtDebitAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtDebitAmount.Text);
            txtCreditAmount.Text = dtSupplierEdit.Rows[0]["Cr_Amount"].ToString();
            txtCreditAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtCreditAmount.Text);
            txtODebitAmount.Text = dtSupplierEdit.Rows[0]["O_Db_Amount"].ToString();
            txtODebitAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtODebitAmount.Text);
            txtOCreditAmount.Text = dtSupplierEdit.Rows[0]["O_Cr_Amount"].ToString();
            txtOCreditAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtOCreditAmount.Text);
            txtPurchaseLimit.Text = dtSupplierEdit.Rows[0]["Purchase_Limit"].ToString();
            txtPurchaseLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtPurchaseLimit.Text);
            txtReturnDays.Text = dtSupplierEdit.Rows[0]["Return_Days"].ToString();



            //here we select the contact bank detail from the contact_bankdetail table and bind the gvcontactbandetail gridview in editable mode

            DataTable DtConactBankDetail = new DataTable();
            DtConactBankDetail.Columns.Add("Trans_Id");
            DtConactBankDetail.Columns.Add("Bank_Name");
            DtConactBankDetail.Columns.Add("Account_No");
            DtConactBankDetail.Columns.Add("Currency_id");
            DtConactBankDetail.Columns.Add("Branch_Address");
            DtConactBankDetail.Columns.Add("IFSC_Code");
            DtConactBankDetail.Columns.Add("MICR_Code");
            DtConactBankDetail.Columns.Add("Branch_Code");
            DtConactBankDetail.Columns.Add("IBAN_NUMBER");
            DataTable Dt = objContactBankDetail.GetContactBankDetail_By_ContactId_And_GroupId(e.CommandArgument.ToString(), "2");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                string BankName = string.Empty;

                DataTable DtBank = objBankMaster.GetBankMasterById(Dt.Rows[i]["Bank_Id"].ToString());

                DataRow dr = DtConactBankDetail.NewRow();
                dr["Trans_Id"] = Dt.Rows[i]["Trans_Id"].ToString();

                dr["Bank_Name"] = DtBank.Rows[0]["Bank_Name"].ToString();
                dr["Account_No"] = Dt.Rows[i]["Account_No"].ToString();
                try
                {
                    dr["Currency_id"] = Dt.Rows[i]["field1"].ToString();
                }
                catch(Exception ex)
                {
                    dr["Currency_id"] = "0";
                }
                dr["Branch_Address"] = Dt.Rows[i]["Branch_Address"].ToString();
                dr["IFSC_Code"] = Dt.Rows[i]["IFSC_Code"].ToString();
                dr["MICR_Code"] = Dt.Rows[i]["MICR_Code"].ToString();
                dr["Branch_Code"] = Dt.Rows[i]["Branch_Code"].ToString();
                dr["IBAN_NUMBER"] = Dt.Rows[i]["IBAN_NUMBER"].ToString();
                DtConactBankDetail.Rows.Add(dr);
            }

            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, DtConactBankDetail, "", "");
            Session["DtContactBankSupplier"] = DtConactBankDetail;

            //here select the record from file transaction
            FillDocGrid();
            //code for Multiple Logo
            DataTable dtLogo = objCustSupLogo.getSet_CustomerSupplier_LogoByIdAndGroup(e.CommandArgument.ToString(), "0");
            ViewState["LogoUpload"] = dtLogo;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvLogo, (DataTable)ViewState["LogoUpload"], "", "");
            //End Code For Multiple Logo
            FillSupplierBankDetail(e.CommandArgument.ToString());
            pnllblSupplierName.Text = txtSupplierName.Text;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

        }
    }
    public void FillSupplierBankDetail(string SupplierID)
     {
        try
        {
            DataTable dt = objSupplier.GetSupplierBankRecord(SupplierID);
            if (dt.Columns.Contains("Trans_Id"))
            {
                // Remove the "Trans_Id" column
                dt.Columns.Remove("Trans_Id");
            }
            if (dt.Rows.Count > 0)
            {
                ViewState["BankDetails"] = dt;
                gvBankDetails.DataSource = dt;
                gvBankDetails.DataBind();
            }
        }
        catch(Exception ex)
        {
            gvBankDetails.DataSource = null;
            gvBankDetails.DataBind();
        }
    }
    public void FillDocGrid()
    {
        try
        {
            DataTable dtDirectory = new DataView(objDir.getDirectoryMasterByCompanyid(Session["CompId"].ToString()), "Company_id='" + Session["CompId"].ToString().ToString() + "'and Field3='" + hdnSupplierId.Value + "'and Field4='Supplier'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtFile = new DataTable();
            string str = string.Empty;
            foreach (DataRow dr in dtDirectory.Rows)
            {
                dtFile.Merge(ObjFile.Get_FileTransaction_By_DirectoryidandObjectId(Session["CompId"].ToString(), "0", dr["Id"].ToString()));
            }
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvFileMaster, dtFile, "", "");
        }
        catch
        {
        }
    }
    //protected void GvSupplier_OnSelectedIndexChaged(object sender, GridViewRowEventArgs e)
    //{
    //    ViewState["SupplierCode"] = e.Row.Cells[3].Text;
    //}
    protected void GvSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSupplier.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtFilterSupplier"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvSupplier, dt, "", "");
        //GvSupplier.BottomPagerRow.Focus();
        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                condition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = ddlFieldName.SelectedValue + " Like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = ddlFieldName.SelectedValue + " like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtsupplier = (DataTable)Session["DtSupplier"];

            DataView view = new DataView(dtsupplier, condition, "", DataViewRowState.CurrentRows);

            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvSupplier, view.ToTable(), "", "");
            Session["DtFilterSupplier"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
            txtValue.Focus();
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGroup();
        ddlGroupSearch.SelectedIndex = 0;
        ddlGroupSearch.Focus();
        GvSupplier.DataSource = null;
        GvSupplier.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
        FillGrid();
    }
    protected void GvSupplier_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["DtFilterSupplier"];
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
        Session["DtFilterSupplier"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvSupplier, dt, "", "");
        //AllPageCode();
        //GvSupplier.HeaderRow.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        //here we check the validation that supplier can not delete if used in any page
        bool Check = objContact.IsUsedInInventory(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString(), "2", "2");
        if (Check)
        {

            DisplayMessage("This Supplier is in use,You Can not delete");
            return;

        }

        hdnSupplierId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objSupplier.DeleteSupplierMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());



        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin(); //Update grid view in bin tab
        //FillGrid();
        Reset();

    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
        GvSupplier.DataSource = null;
        GvSupplier.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
        FillGrid();
        //AllPageCode();
    }
    protected void btnSupplierCancel_Click(object sender, EventArgs e)
    {
        Reset();
       // btnList_Click(null, null);
        FillGridBin();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        btnSupplierCancel_Click(null, null);
        //Reset();
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlAddressCategory);
    }



    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                ViewState["Emp_Img"] = "";
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }
  
    protected void btnSupplierSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strTaxable = string.Empty;
        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Fill Supplier Name");
            txtSupplierName.Focus();
            return;
        }

        if (ddlCurrency.SelectedItem.Text == "--Select--")
        {
            DisplayMessage("Please Select Supplier Currency");
            ddlCurrency.Focus();
            return;
        }

        foreach (GridViewRow gv in GvLocation.Rows)
        {
            double OpeningDebitAmount = 0;
            double OpeningCreditAmount = 0;
            double OpeningFDebitAmount = 0;
            double OpeningFCreditAmount = 0;
            TextBox txtgvDebit = (TextBox)gv.FindControl("txtgvDebit");
            TextBox txtgvCredit = (TextBox)gv.FindControl("txtgvCredit");
            TextBox txtgvFDebit = (TextBox)gv.FindControl("txtgvForeignDebit");
            TextBox txtgvFCredit = (TextBox)gv.FindControl("txtgvForeignCredit");

            if (txtgvDebit.Text != "")
            {
                OpeningDebitAmount = Convert.ToDouble(txtgvDebit.Text);
            }
            if (txtgvCredit.Text != "")
            {
                OpeningCreditAmount = Convert.ToDouble(txtgvCredit.Text);
            }
            if (txtgvFDebit.Text != "")
            {
                OpeningFDebitAmount = Convert.ToDouble(txtgvFDebit.Text);
            }
            if (txtgvFCredit.Text != "")
            {
                OpeningFCreditAmount = Convert.ToDouble(txtgvFCredit.Text);
            }

            if (OpeningDebitAmount != 0 && OpeningCreditAmount != 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('You cant enter opening both values');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtODebitAmount);
                return;
            }

            if (OpeningFDebitAmount != 0 && OpeningFCreditAmount != 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('You cant enter opening both values');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtODebitAmount);
                return;
            }

            if (OpeningDebitAmount != 0)
            {
                if (OpeningFCreditAmount != 0)
                {
                    DisplayMessage("Need to Enter Both Debit Values");
                    return;
                }
            }
            if (OpeningCreditAmount != 0)
            {
                if (OpeningFDebitAmount != 0)
                {
                    DisplayMessage("Need to Enter Both Credit Values");
                    return;
                }
            }

            if (OpeningFDebitAmount != 0)
            {
                if (OpeningDebitAmount == 0)
                {
                    DisplayMessage("Need to Enter Local Debit Values");
                    return;
                }
            }
            if (OpeningFCreditAmount != 0)
            {
                if (OpeningCreditAmount == 0)
                {
                    DisplayMessage("Need to Enter Local Credit Values");
                    return;
                }
            }
        }
        //}

        if (txtPurchaseLimit.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtPurchaseLimit.Text, out flTemp))
            {

            }
            else
            {
                txtPurchaseLimit.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPurchaseLimit);
                return;
            }
        }
        else
        {
            txtPurchaseLimit.Text = "0";
        }

        if (txtReturnDays.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtReturnDays.Text, out flTemp))
            {

            }
            else
            {
                txtReturnDays.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReturnDays);
                return;
            }
        }
        else
        {
            txtReturnDays.Text = "0";
        }

        string IsSupplierPriceList = string.Empty;
        if (chkSpriceList.Checked == true)
        {
            IsSupplierPriceList = "Y";
        }

        string AccountNo = string.Empty;
        try
        {
            AccountNo = txtAccountNo.Text.Split('/')[1].ToString();
        }
        catch
        {
            AccountNo = "0";
        }

        string EmpPermission = string.Empty;
        DataTable dt1 = new DataTable();
        DataTable dtApproval = new DataTable();

        //(temp code) here set ddlIsCredit=false so rest of code will not execute - neelkanth purohit 03/Sep/2018
        ddlIsCredit.SelectedValue = false.ToString();

        if (ddlIsCredit.SelectedValue == "True")
        {
            EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("Supplier Credit").Rows[0]["Approval_Level"].ToString();

            dtApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),"20", Session["EmpId"].ToString());

            if (dtApproval.Rows.Count == 0)
            {
                DisplayMessage("Approval setup issue , please contact to your admin");
                return;
            }


            //here we set set validation for credir customer

            //code created by jitendra upadhyay on 17-06-2016

            if (txtCreditLimit.Text == "")
            {

                DisplayMessage("Enter Credit Limit");
                txtCreditLimit.Focus();
                return;
            }
            else
            {

            }
            if (txtCreditDays.Text == "")
            {
                DisplayMessage("Enter Credit days");
                txtCreditDays.Focus();
                return;
            }

            //if (!rbtnAdvanceCheque.Checked && !rbtnInvoicetoInvoice.Checked && !rbtnAdvanceHalfpayment.Checked)
            //{
            //    DisplayMessage("select at least one option in credit parameter");
            //    return;
            //}
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {


            int b = 0;
            if (hdnSupplierId.Value != "")
            {
                //For Accounts data
                string strSupplierAccount = string.Empty;
                DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString(), ref trns);
                DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCash.Rows.Count > 0)
                {
                    strSupplierAccount = dtCash.Rows[0]["Param_Value"].ToString();
                }
                //End

                string fn;
                b = objSupplier.UpdateSupplierMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value, AccountNo, txtDebitAmount.Text, txtCreditAmount.Text, txtODebitAmount.Text, txtOCreditAmount.Text, txtPurchaseLimit.Text, txtReturnDays.Text, IsSupplierPriceList, chkIsNonRegistered.Checked.ToString(), ddlCurrency.SelectedValue, ddlIsCredit.SelectedValue, ViewState["Status"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
              
                DataTable dtBankSupplier = (DataTable)ViewState["BankDetails"];
                da.execute_Command("Delete from Set_SupplierMaster_BankReferences where Supplier_Id='" + hdnSupplierId.Value + "'");
                if (dtBankSupplier.Rows.Count > 0)
                {    
                    for (int i = 0; i < dtBankSupplier.Rows.Count; i++)
                    {
                         
                        objSupplier.InsertSupplierBankRecord(hdnSupplierId.Value, dtBankSupplier.Rows[i]["Currency"].ToString(), dtBankSupplier.Rows[i]["AccountName"].ToString(), dtBankSupplier.Rows[i]["AccountNo"].ToString(),dtBankSupplier.Rows[i]["BankerName"].ToString(),dtBankSupplier.Rows[i]["BankerAddress"].ToString(),dtBankSupplier.Rows[i]["ContactNo"].ToString(),dtBankSupplier.Rows[i]["FaxNo"].ToString(),dtBankSupplier.Rows[i]["IFSCCode"].ToString(), dtBankSupplier.Rows[i]["IBANCode"].ToString(), dtBankSupplier.Rows[i]["SWIFTCode"].ToString(), dtBankSupplier.Rows[i]["trnsEmpId"].ToString(),dtBankSupplier.Rows[i]["ExchangeName"].ToString(),dtBankSupplier.Rows[i]["BeneFeciaryAddress"].ToString(), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

                //add supplier price list section
                objContactPriceList.DeleteContactPriceList(Session["CompId"].ToString(), hdnSupplierId.Value, "S", ref trns);
                if (chkSpriceList.Checked == true)
                {
                    if (Session["DtCustomerPriceList"] != null)
                    {
                        DataTable DtCustomerPriceList = (DataTable)Session["DtCustomerPriceList"];

                        for (int i = 0; i < DtCustomerPriceList.Rows.Count; i++)
                        {
                            objContactPriceList.InsertContact_PriceList(Session["CompId"].ToString(), hdnSupplierId.Value, DtCustomerPriceList.Rows[i]["Product_Id"].ToString(), "S", DtCustomerPriceList.Rows[i]["Sales_Price"].ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                //end

                //this code is added by jitendra upadhyay on 26-09-2016
                //this code is created for save product category for specfic supplier

                ObjContactProductcategory.DeleteProductCategory(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value, "S", ref trns);

                foreach (ListItem li in lstProductCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            ObjContactProductcategory.InsertProductCategory(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value, li.Value.ToString(), "S", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        catch
                        {

                        }
                    }
                }


                //Add Address Insert Section.
                objAddChild.DeleteAddressChild("Supplier", hdnSupplierId.Value, ref trns);
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
                        DataTable dtAddId = objAddMaster.GetAddressDataByAddressName(lblGvAddressName.Text, ref trns);
                        if (dtAddId.Rows.Count > 0)
                        {
                            string strAddressId = dtAddId.Rows[0]["Trans_Id"].ToString();
                            objAddChild.InsertAddressChild(strAddressId, "Supplier", hdnSupplierId.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                //End  

                //here we code for insert the contact bank detail
                objContactBankDetail.DeleteContactBankDetail_By_ContactId_And_GroupId(hdnSupplierId.Value, "2", ref trns);
                if (Session["DtContactBankSupplier"] != null)
                {
                    DataTable dtContactBankDetail = (DataTable)Session["DtContactBankSupplier"];
                    for (int i = 0; i < dtContactBankDetail.Rows.Count; i++)
                    {
                        DataTable DtBank = objBankMaster.GetBankMasterByBankName(dtContactBankDetail.Rows[i]["Bank_Name"].ToString(), ref trns);

                        objContactBankDetail.InsertContact_BankDetail(hdnSupplierId.Value, DtBank.Rows[0]["Bank_Id"].ToString(), dtContactBankDetail.Rows[i]["Account_No"].ToString(), dtContactBankDetail.Rows[i]["Branch_Address"].ToString(), dtContactBankDetail.Rows[i]["IFSC_Code"].ToString(), dtContactBankDetail.Rows[i]["MICR_Code"].ToString(), dtContactBankDetail.Rows[i]["Branch_Code"].ToString(), "2", dtContactBankDetail.Rows[i]["IBAN_NUMBER"].ToString(), dtContactBankDetail.Rows[i]["Currency_id"].ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

             

                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();

                hdnSupplierId.Value = "";


                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    Reset();
                    FillGrid();
                   // btnList_Click(null, null);
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

                }
                else
                {
                    DisplayMessage("Record  Not Updated");
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
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["LocCurrencyId"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    public string GetCurrencyForOpening(string strToCurrency, string strLocalAmount, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString(), ref trns).Rows[0]["Currency_Id"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strToCurrency, strCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString(), ref trns);
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

 
    public void CheckDirectory(string path)
    {
        if (path != "")
        {
            Directory.CreateDirectory(path);
        }
    }

    #endregion

    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        hdnSupplierId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objSupplier.DeleteSupplierMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnSupplierId.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        // FillGrid();
        FillGridBin();
        Reset();
    }
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {

        FillGridBin();
        txtValueBin.Focus();
        //AllPageCode();
    }

    protected void GvSupplierBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSupplierBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvSupplierBin, dt, "", "");
        //AllPageCode();
        GvSupplierBin.BottomPagerRow.Focus();

        string temp = string.Empty;
        for (int i = 0; i < GvSupplierBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvSupplierBin.Rows[i].FindControl("lblgvSupplierId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSupplierBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }

        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gv in GvSupplierBin.Rows)
        {
            Label lblgvPurchaseLimit = (Label)gv.FindControl("lblgvPurchaseLimit");

            lblgvPurchaseLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvPurchaseLimit.Text);
        }
    }
    protected void GvSupplierBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objSupplier.GetSupplierAllFalseData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvSupplierBin, dt, "", "");
        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gv in GvSupplierBin.Rows)
        {
            Label lblgvPurchaseLimit = (Label)gv.FindControl("lblgvPurchaseLimit");

            lblgvPurchaseLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvPurchaseLimit.Text);
        }
        //AllPageCode();
        lblSelectedRecord.Text = "";
        GvSupplierBin.HeaderRow.Focus();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objSupplier.GetSupplierAllFalseData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvSupplierBin, dt, "", "");
        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gv in GvSupplierBin.Rows)
        {
            Label lblgvPurchaseLimit = (Label)gv.FindControl("lblgvPurchaseLimit");

            lblgvPurchaseLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvPurchaseLimit.Text);
        }
        Session["dtSupplierBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)Session["dtSupplierBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvSupplierBin, view.ToTable(), "", "");
            string strCurrency = Session["LocCurrencyId"].ToString();
            foreach (GridViewRow gv in GvSupplierBin.Rows)
            {
                Label lblgvPurchaseLimit = (Label)gv.FindControl("lblgvPurchaseLimit");

                lblgvPurchaseLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvPurchaseLimit.Text);
            }
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtValueBin.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        DataTable dt = objSupplier.GetSupplierAllFalseData(Session["CompId"].ToString(), Session["BrandId"].ToString());

        if (GvSupplierBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        b = objSupplier.DeleteSupplierMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                //FillGrid();
                FillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvSupplierBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
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

    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSupplierBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSupplierBin.Rows.Count; i++)
        {
            ((CheckBox)GvSupplierBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvSupplierBin.Rows[i].FindControl("lblgvSupplierId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvSupplierBin.Rows[i].FindControl("lblgvSupplierId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvSupplierBin.Rows[i].FindControl("lblgvSupplierId"))).Text.Trim().ToString())
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
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvSupplierBin.Rows[index].FindControl("lblgvSupplierId");
        if (((CheckBox)GvSupplierBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ((CheckBox)GvSupplierBin.Rows[index].FindControl("chkSelect")).Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtSupplier = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtSupplier.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Supplier_Id"]))
                {
                    lblSelectedRecord.Text += dr["Supplier_Id"] + ",";
                }
            }
            for (int i = 0; i < GvSupplierBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvSupplierBin.Rows[i].FindControl("lblgvSupplierId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSupplierBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvSupplierBin, dtUnit1, "", "");
            string strCurrency = Session["LocCurrencyId"].ToString();
            foreach (GridViewRow gv in GvSupplierBin.Rows)
            {
                Label lblgvPurchaseLimit = (Label)gv.FindControl("lblgvPurchaseLimit");

                lblgvPurchaseLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvPurchaseLimit.Text);
            }
            //AllPageCode();
            ViewState["Select"] = null;
        }
     
    }
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objChartOfAcc = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = objChartOfAcc.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        dtCOA = new DataView(dtCOA, "AccountName Like '" + prefixText + "%'", "AccountName Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dtCOA.Rows.Count];
        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                txt = null;
            }
            else
            {
                dtCOA = objChartOfAcc.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dtCOA.Rows.Count > 0)
                {
                    txt = new string[dtCOA.Rows.Count];
                    for (int i = 0; i < dtCOA.Rows.Count; i++)
                    {
                        txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return txt;
    }

    protected void txtAccountName_TextChanged(object sender, EventArgs e)
    {
        if (txtAccountNo.Text != "")
        {
            string strTransId = GetAccountId();
            DataTable dtAccount = objCOf.GetCOAByTransId(Session["CompId"].ToString(), strTransId);
            if (dtAccount.Rows.Count > 0)
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDebitAmount);
            }
            else
            {
                txtAccountNo.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Account Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAccountNo);
        }
    }
    private string GetAccountId()
    {
        string retval = string.Empty;
        if (txtAccountNo.Text != "")
        {
            retval = (txtAccountNo.Text.Split('/'))[txtAccountNo.Text.Split('/').Length - 1];

            DataTable dtCOA = objCOf.GetCOAByTransId(Session["CompId"].ToString(), retval);
            if (dtCOA.Rows.Count > 0)
            {

            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    private string GetAccountName(string AccountId)
    {
        string retval = string.Empty;
        DataTable dtCOA = objCOf.GetCOAByTransId(Session["CompId"].ToString(), AccountId);
        if (dtCOA.Rows.Count > 0)
        {
            retval = dtCOA.Rows[0]["AccountName"].ToString();
        }
        else
        {
            retval = "";
        }

        return retval;
    }
    #region User defined Function
    public void FillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)GvLocation, dtLoc, "", "");
        }
    }

    private void FillGrid()
    {
        DataTable dtSupplier = objSupplier.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());

        if (ddlGroupSearch.SelectedIndex > 0)
        {
            string condition = string.Empty;
            condition = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();
            dtSupplier = objSupplier.GetSupplierAllTrueDataByGroup(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlGroupSearch.SelectedValue.ToString());
        }


        if (ddlCreditStatus.SelectedIndex != 0)
        {
            dtSupplier = new DataView(dtSupplier, "IsCredit='" + ddlCreditStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }


        GvSupplier.DataSource = dtSupplier;
        GvSupplier.DataBind();
        Session["DtSupplier"] = dtSupplier;
        Session["DtFilterSupplier"] = dtSupplier;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtSupplier.Rows.Count.ToString() + "";
        //AllPageCode();

    }

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //AllPageCode();
    }


    public void FillGroup()
    {
        DataTable DtGroup = objGroup.GetGroupMasterTrueAllData();

        try
        {
            DtGroup = new DataView(DtGroup, "Parent_Id=2", "Group_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (DtGroup.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)ddlGroupSearch, DtGroup, "Group_Name", "Group_Id");
        }
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
    public void Reset()
    {
        txtSupplierName.Text = "";
        txtLSupplierName.Text = "";
        txtId.Text = "";
        txtCivilId.Text = "";

        txtAddressName.Text = "";
        txtAccountNo.Text = "";

        txtPurchaseLimit.Text = "";
        txtReturnDays.Text = "";

        GvAddressName.DataSource = null;
        GvAddressName.DataBind();

        hdnSupplierId.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        Session["DtContactBankSupplier"] = null;
        GvContactBankDetail.DataSource = null;
        GvContactBankDetail.DataBind();
        hdnContactBankId.Value = "";
        txtContactBankName.Text = "";
        Session["DtCustomerPriceList"] = null;
        BtnShowCpriceList.Visible = false;
        chkSpriceList.Checked = false;
        Session["dtArcaWing"] = null;
        gvFileMaster.DataSource = null;
        gvFileMaster.DataBind();
        ddlGroupSearch.SelectedIndex = 0;
        pnllblSupplierName.Text = "";
        pnllblCompanyName.Text = "";
        ViewState["LogoUpload"] = null;
        ViewState["SupplierCode"] = null;
        ViewState["LogoUpload"] = null;
        ViewState["ImagePath"] = null;
        ViewState["Image"] = null;
        gvLogo.DataSource = null;
        gvLogo.DataBind();

        FillProductCategory();
        GvLocation.DataSource = null;
        GvLocation.DataBind();
        FillLocation();
    }
    #endregion

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
        dt.Columns.Add("Latitude");
        dt.Columns.Add("Longitude");
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
        DataTable dt = objAddMaster.GetAddressDataByAddressName(AddressName);

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
        DtAddress = objAddMaster.GetAddressDataByAddressName(AddressName);
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
        DtAddress = objAddMaster.GetAddressDataByAddressName(AddressName);
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
        DtAddress = objAddMaster.GetAddressDataByAddressName(AddressName);
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
        DtAddress = objAddMaster.GetAddressDataByAddressName(AddressName);
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
    #endregion

    #region Add New Address Concept

    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

        //pnlAddress2.Visible = false;
    }


    protected string GetLocationName(string strLocationId)
    {
        string strLocationName = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocName = objLocation.GetLocationMasterById(Session["CompId"].ToString(), strLocationId);
            if (dtLocName.Rows.Count > 0)
            {
                strLocationName = dtLocName.Rows[0]["Location_Name"].ToString();
            }
        }
        else
        {
            strLocationName = "";
        }
        return strLocationName;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCurrName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCurrName.Rows.Count > 0)
            {
                strCurrencyName = dtCurrName.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }

    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = Session["LocCurrencyId"].ToString();

            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();

            ViewState["CountryCode"] = ObjCountry.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        }
        catch
        {

        }
    }

    #endregion
    #region SupplierDetail
 
  
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        Reset();
        btnEdit_Command(sender, e);
        Lbl_Tab_New.Text = Resources.Attendance.View;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

        //AllPageCode();
    }
    protected void datalistCustomerDetailView_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        GridView gvAddressView = (GridView)e.Item.FindControl("GvAddressNameView");
        HiddenField hdnContactId = (HiddenField)e.Item.FindControl("hdnContactIdView");
        GridView GvContactBankDetail = (GridView)e.Item.FindControl("GvContactBankDetail");
        DataList dlViewcontactlist = (DataList)e.Item.FindControl("dlViewcontactlist");
        Label lblCSalesPrice = (Label)e.Item.FindControl("lblCSalesPrice");
        string Id = hdnContactId.Value;
        DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Supplier", Id);
        if (dtChild.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvAddressView, dtChild, "", "");
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
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, dtcontactBank, "", "");
        }
        DataTable dt = objContact.GetContactTrueAllData(Id, "Individual");
        if (dt.Rows.Count != 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)dlViewcontactlist, dt, "", "");
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

        DataTable dtDepartment = objDepartment.GetDepartmentMasterById(DepartmentId);
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

        DataTable dtDesg = objDesg.GetDesignationMasterById(DesgId);
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

        DataTable dtReligion = objReligion.GetReligionMasterById(ReligionId);
        try
        {
            ReligionName = dtReligion.Rows[0]["Religion"].ToString();
        }
        catch
        {
        }

        return ReligionName;
    }


    #endregion
    #region ContactBankDetail
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListBankName(string prefixText, int count, string contextKey)
    {
        Set_BankMaster objBankMaster = new Set_BankMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objBankMaster.GetBankMaster(), "Bank_Name like '" + prefixText.ToString() + "%'", "Bank_Name Asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
        }
        else
        {
            dt = objBankMaster.GetBankMaster();
        }
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
            if (dtBank.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in GvContactBankDetail.Rows)
                {
                    Label lblgvBankId = (Label)gvrow.FindControl("lblgvBankId");
                    if (lblgvBankId.Text == txtContactBankName.Text)
                    {
                        //DisplayMessage("This Bank is Already exists");
                        //txtContactBankName.Text = "";
                        //txtContactBankName.Focus();
                        //return;
                    }
                }
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(BtnContactBank);
            }
            else
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
            if (dtBank.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in GvContactBankDetail.Rows)
                {
                    Label lblgvBankId = (Label)gvrow.FindControl("lblgvBankId");
                    if (lblgvBankId.Text == txtBankName.Text)
                    {
                        //DisplayMessage("This Bank is Already exists");
                        //txtBankName.Text = "";
                        //txtBankName.Focus();
                        //return;
                    }
                }
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(BtnContactBank);
            }
            else
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
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Add_Bank()", true);
        //PnlContactBankdetail2.Visible = true;
        //PnlContactBankdetail3.Visible = true;

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

        if (ddlBankAcCurrency.SelectedItem.Text == "--Select--")
        {
            DisplayMessage("Please select currency");
            ddlBankAcCurrency.Focus();
            return;
        }
        

        DataTable dt = new DataTable();
        if (hdnContactBankId.Value == "")
        {



            if (Session["DtContactBankSupplier"] == null)
            {

                dt.Columns.Add("Trans_Id");


                dt.Columns.Add("Bank_Name");
                dt.Columns.Add("Account_No");
                dt.Columns.Add("Currency_Id");
                dt.Columns.Add("Branch_Address");
                dt.Columns.Add("IFSC_Code");
                dt.Columns.Add("MICR_Code");
                dt.Columns.Add("Branch_Code");
                dt.Columns.Add("IBAN_NUMBER");
                DataRow dr = dt.NewRow();
                dr["Trans_Id"] = "1";

                dr["Bank_Name"] = txtBankName.Text;
                dr["Account_No"] = txtCBAccountNo.Text;
                dr["Currency_Id"]=ddlBankAcCurrency.SelectedValue;
                dr["Branch_Address"] = txtCBBrachAddress.Text;
                dr["IFSC_Code"] = txtCBIFSCCode.Text;
                dr["MICR_Code"] = txtCBMICRCode.Text;
                dr["Branch_Code"] = txtCBBranchCode.Text;
                dr["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = (DataTable)Session["DtContactBankSupplier"];
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
                dr["Currency_Id"] = ddlBankAcCurrency.SelectedValue;
                dr["Branch_Address"] = txtCBBrachAddress.Text;
                dr["IFSC_Code"] = txtCBIFSCCode.Text;
                dr["MICR_Code"] = txtCBMICRCode.Text;
                dr["Branch_Code"] = txtCBBranchCode.Text;
                dr["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                dt.Rows.Add(dr);

            }
            Session["DtContactBankSupplier"] = dt;
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, dt, "", "");
            DisplayMessage("Record Saved","green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Add_Bank()", true);
        }
        else
        {
            dt = (DataTable)Session["DtContactBankSupplier"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Trans_id"].ToString() == hdnContactBankId.Value)
                {
                    dt.Rows[i]["Bank_Name"] = txtBankName.Text;
                    dt.Rows[i]["Account_No"] = txtCBAccountNo.Text;
                    dt.Rows[i]["Currency_Id"] = ddlBankAcCurrency.SelectedValue;
                    dt.Rows[i]["Branch_Address"] = txtCBBrachAddress.Text;
                    dt.Rows[i]["IFSC_Code"] = txtCBIFSCCode.Text;
                    dt.Rows[i]["MICR_Code"] = txtCBMICRCode.Text;
                    dt.Rows[i]["Branch_Code"] = txtCBBranchCode.Text;
                    dt.Rows[i]["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                }
            }
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, dt, "", "");
            Session["DtContactBankSupplier"] = dt;
            ResetContactBankPanel();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Add_Bank()", true);
            //PnlContactBankdetail2.Visible = false;
            //PnlContactBankdetail3.Visible = false;
        }
        hdnContactBankId.Value = "";
        ResetContactBankPanel();
        txtBankName.Focus();

    }
    protected void btnContactBankEdit_Command(object sender, CommandEventArgs e)
    {
        txtBankName.ReadOnly = true;
        hdnContactBankId.Value = e.CommandArgument.ToString();
        DataTable dt = (DataTable)Session["DtContactBankSupplier"];
        try
        {
            dt = new DataView(dt, "Trans_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        txtBankName.Text = dt.Rows[0]["Bank_Name"].ToString();
        txtCBAccountNo.Text = dt.Rows[0]["Account_No"].ToString();
        ddlBankAcCurrency.SelectedValue= dt.Rows[0]["Currency_Id"].ToString();
        txtCBBrachAddress.Text = dt.Rows[0]["Branch_Address"].ToString();
        txtCBIFSCCode.Text = dt.Rows[0]["IFSC_Code"].ToString();
        txtCBMICRCode.Text = dt.Rows[0]["MICR_Code"].ToString();
        txtCBBranchCode.Text = dt.Rows[0]["Branch_Code"].ToString();
        txtIBANNUMBER.Text = dt.Rows[0]["IBAN_NUMBER"].ToString();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Add_Bank()", true);
        //PnlContactBankdetail2.Visible = true;
        //PnlContactBankdetail3.Visible = true;
        txtCBAccountNo.Focus();

    }
    protected void btnContactBankDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["DtContactBankSupplier"];
        try
        {
            dt = new DataView(dt, "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        Session["DtContactBankSupplier"] = dt;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvContactBankDetail, dt, "", "");
    }
    protected void BtnCBCancel_Click(object sender, EventArgs e)
    {
        ResetContactBankPanel();
        txtBankName.Focus();
        //ModalPopupExtender3.Hide();
        ////PnlContactBankdetail2.Visible = false;
        ////PnlContactBankdetail3.Visible = false;
        hdnContactBankId.Value = "";

    }
    protected void mgContactBankClose_Click(object sender, ImageClickEventArgs e)
    {
        ResetContactBankPanel();

        //PnlContactBankdetail2.Visible = false;
        //PnlContactBankdetail3.Visible = false;
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
        ddlBankAcCurrency.SelectedIndex = 0;
    }

    #endregion
    #region Add_CustomerPriceList
    protected string GetUnitName(string strUnitId)
    {
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
            if (dtUName.Rows.Count > 0)
            {
                strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
            }
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }

    protected void BtnCustomerPriceListSave_Click(object sender, EventArgs e)
    {
        DataTable DtcustomerPriceList = new DataTable();
        DtcustomerPriceList.Columns.Add("Product_Id");
        DtcustomerPriceList.Columns.Add("Sales_Price");


        foreach (GridViewRow gvrow in GvProduct.Rows)
        {
            Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
            TextBox txtGvsalesPrice = (TextBox)gvrow.FindControl("txtGvsalesPrice");
            if (txtGvsalesPrice.Text != "" && txtGvsalesPrice.Text != "0")
            {
                DataRow dr = DtcustomerPriceList.NewRow();
                dr["Product_Id"] = lblgvProductId.Text;
                dr["Sales_Price"] = txtGvsalesPrice.Text;
                DtcustomerPriceList.Rows.Add(dr);
            }
        }

        Session["DtCustomerPriceList"] = DtcustomerPriceList;
        //PnlCustomerPricelist2.Visible = false;
        //PnlCustomerPricelist3.Visible = false;
        DisplayMessage("Record Saved","green");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close_Product()", true);
    }
    protected void BtnCustomerPriceListCancel_Click(object sender, EventArgs e)
    {

        //PnlCustomerPricelist2.Visible = false;
        //PnlCustomerPricelist3.Visible = false;
    }
    protected void BtnShowCpriceList_click(object sender, EventArgs e)
    {
        DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvProduct, dtProductMaster, "", "");
        Session["DtProduct"] = dtProductMaster;

        if (Session["DtCustomerPriceList"] != null)
        {
            DataTable DtCustomerPriceList = (DataTable)Session["DtCustomerPriceList"];

            for (int i = 0; i < DtCustomerPriceList.Rows.Count; i++)
            {
                foreach (GridViewRow gvrow in GvProduct.Rows)
                {
                    Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
                    TextBox txtGvsalesPrice = (TextBox)gvrow.FindControl("txtGvsalesPrice");
                    if (lblgvProductId.Text == DtCustomerPriceList.Rows[i]["Product_Id"].ToString())
                    {
                        txtGvsalesPrice.Text = Common.GetAmountDecimal(DtCustomerPriceList.Rows[i]["Sales_Price"].ToString(),Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                    }
                }

            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Show_Product()", true);
        //PnlCustomerPricelist2.Visible = true;
        //PnlCustomerPricelist3.Visible = true;
        pnllblSupplierName.Text = txtSupplierName.Text;
    }
    protected void ImgButtonCPClose_Click(object sender, ImageClickEventArgs e)
    {

        //PnlCustomerPricelist2.Visible = false;
        //PnlCustomerPricelist3.Visible = false;
    }
    protected void btnBindCPricelist_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlCpriceFieldoption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlCpriceFieldoption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlCpriceField.SelectedValue + ",System.String)='" + txtCpriceSearch.Text.Trim() + "'";
            }
            else if (ddlCpriceFieldoption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlCpriceField.SelectedValue + ",System.String) like '%" + txtCpriceSearch.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlCpriceField.SelectedValue + ",System.String) Like '" + txtCpriceSearch.Text.Trim() + "%'";
            }
            DataTable dtAdd = (DataTable)Session["DtProduct"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvProduct, view.ToTable(), "", "");
            Session["DtProduct"] = view.ToTable();

            btnBindCPricelist.Focus();
            if (Session["DtCustomerPriceList"] != null)
            {
                DataTable DtCustomerPriceList = (DataTable)Session["DtCustomerPriceList"];

                for (int i = 0; i < DtCustomerPriceList.Rows.Count; i++)
                {
                    foreach (GridViewRow gvrow in GvProduct.Rows)
                    {
                        Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
                        TextBox txtGvsalesPrice = (TextBox)gvrow.FindControl("txtGvsalesPrice");
                        if (lblgvProductId.Text == DtCustomerPriceList.Rows[i]["Product_Id"].ToString())
                        {
                            txtGvsalesPrice.Text = DtCustomerPriceList.Rows[i]["Sales_Price"].ToString();
                        }
                    }

                }
            }


        }
    }
    protected void btnRefreshCPricelist_Click(object sender, ImageClickEventArgs e)
    {
        txtCpriceSearch.Text = "";
        txtCpriceSearch.Focus();
        DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        Session["DtProduct"] = dtProductMaster;
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)GvProduct, dtProductMaster, "", "");

        if (Session["DtCustomerPriceList"] != null)
        {
            DataTable DtCustomerPriceList = (DataTable)Session["DtCustomerPriceList"];

            for (int i = 0; i < DtCustomerPriceList.Rows.Count; i++)
            {
                foreach (GridViewRow gvrow in GvProduct.Rows)
                {
                    Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
                    TextBox txtGvsalesPrice = (TextBox)gvrow.FindControl("txtGvsalesPrice");
                    if (lblgvProductId.Text == DtCustomerPriceList.Rows[i]["Product_Id"].ToString())
                    {
                        txtGvsalesPrice.Text = DtCustomerPriceList.Rows[i]["Sales_Price"].ToString();
                    }
                }

            }
        }
    }
    protected void chkSpriceList_checkedChanged(object sender, EventArgs e)
    {
        if (chkSpriceList.Checked == true)
        {
            BtnShowCpriceList.Visible = true;
            DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)GvProduct, dtProductMaster, "", "");
            Session["DtProduct"] = dtProductMaster;

            if (Session["DtCustomerPriceList"] != null)
            {
                DataTable DtCustomerPriceList = (DataTable)Session["DtCustomerPriceList"];

                for (int i = 0; i < DtCustomerPriceList.Rows.Count; i++)
                {
                    foreach (GridViewRow gvrow in GvProduct.Rows)
                    {
                        Label lblgvProductId = (Label)gvrow.FindControl("lblgvProductId");
                        TextBox txtGvsalesPrice = (TextBox)gvrow.FindControl("txtGvsalesPrice");
                        if (lblgvProductId.Text == DtCustomerPriceList.Rows[i]["Product_Id"].ToString())
                        {
                            txtGvsalesPrice.Text = DtCustomerPriceList.Rows[i]["Sales_Price"].ToString();
                        }
                    }

                }
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Show_Product()", true);
            //PnlCustomerPricelist2.Visible = true;
            //PnlCustomerPricelist3.Visible = true;
            pnllblSupplierName.Text = txtSupplierName.Text;

        }
        else
        {
            BtnShowCpriceList.Visible = false;
        }
    }


    #endregion
    #region Add_Arcawing

    void BindDocumentList()
    {
        DataTable dtdocument = new DataTable();
        string Documentid = "0";
        dtdocument = ObjDocument.getdocumentmaster(Session["CompId"].ToString(), Documentid);
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)ddlDocumentName, dtdocument, "Document_name", "Id");
    }


    protected void ImgButtonDocumentAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (UploadFile.HasFile == false)
        {
            DisplayMessage("Upload The File");
            UploadFile.Focus();
            return;
        }
        string filepath = string.Empty;
        int b = 0;
        string DirectoryName;
        try
        {
            filepath = "~/" + "ArcaWing/" + Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + ddlDocumentName.SelectedValue.ToString() + "/" + UploadFile.FileName;
            CreateDirectoryIfNotExist(Server.MapPath("~/" + "ArcaWing/" + Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + ddlDocumentName.SelectedValue.ToString()));


            DirectoryName = Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + ddlDocumentName.SelectedValue.ToString();
            DataTable dtDir = objDir.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), DirectoryName);
            if (dtDir.Rows.Count == 0)
            {

                b = objDir.InsertDirectorymaster(Session["CompId"].ToString(), DirectoryName, "1", "0", hdnSupplierId.Value.Trim(), "Supplier", "0", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["EmpId"].ToString());
            }
            else
            {
                b = Convert.ToInt32(dtDir.Rows[0]["Id"].ToString());
            }
            UploadFile.SaveAs(Server.MapPath(filepath));

        }
        catch
        {
        }

        Byte[] bytes = new Byte[0];
        try
        {
            bytes = FileToByteArray(Server.MapPath(filepath));
        }
        catch
        {
        }

        ObjFile.Insert_In_FileTransaction(Session["CompId"].ToString(), b.ToString(), ddlDocumentName.SelectedValue.ToString(), "0", UploadFile.FileName.ToString(), DateTime.Now.ToString(), bytes, "", DateTime.Now.AddYears(20).ToString(), "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        string ext = string.Empty;
        string filename = UploadFile.FileName;
        try
        {
            ext = Path.GetExtension(filepath);
        }
        catch
        {

        }

        FillDocGrid();
        //AllPageCode();
        ddlDocumentName.SelectedIndex = 0;
        UploadFile.FileContent.Dispose();
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

    public int CreateDirectoryIfNotExist(string NewDirectory)
    {
        int checkDirectory = 0;
        try
        {
            // Checking the existance of directory
            if (!Directory.Exists(NewDirectory))
            {
                //If No any such directory then creates the new one
                Directory.CreateDirectory(NewDirectory);
            }
            else
            {
                checkDirectory = 1;
            }
        }
        catch (IOException _err)
        {
            Response.Write(_err.Message);
        }
        return checkDirectory;
    }
    protected void IbtnDeleteDocument_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString().ToString(), e.CommandArgument.ToString());
        if (dt != null)
        {
            try
            {
                string FilePath = string.Empty;
                FilePath = Server.MapPath(Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + e.CommandName.ToString() + dt.Rows[0]["File_Name"].ToString());
                FilePath = FilePath.Replace("MasterSetup", "ArcaWing");
                System.IO.File.Delete(FilePath);
            }
            catch
            {

            }

            ObjFile.Delete_in_FileTransactionParmanent(Session["CompId"].ToString(), e.CommandArgument.ToString());
            FillDocGrid();
        }
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


    protected void OnDownloadCommand(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString(), e.CommandArgument.ToString());

        download(dt);
        Reset();
    }
    #endregion
    #region Multiple Logo Upload
    public DataTable CreateLogoDataTable()
    {
        DataTable dtLogo = new DataTable();
        dtLogo.Columns.Add("Trans_Id", typeof(int));
        dtLogo.Columns.Add("ImagePath");

        return dtLogo;
    }
    protected void ImgLogoAdd_Click(object sender, ImageClickEventArgs e)
    {

        if (FileUploadImage.HasFile == false)
        {
            DisplayMessage("Upload The File");
            FileUploadImage.Focus();
            return;
        }
        DataTable dtLogo = (DataTable)(ViewState["LogoUpload"]);

        string path = Server.MapPath("~/CompanyResource/Supplier");
        if (!Directory.Exists(path))
        {
            CheckDirectory(path);
        }


        dtLogo = FillLogoTable();
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvLogo, (DataTable)ViewState["LogoUpload"], "", "");
        //AllPageCode();
    }
    protected void IbtnDeleteLogo_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["LogoUpload"]), "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 14-05-2015
        objPageCmn.FillData((object)gvLogo, dt, "", "");
        ViewState["LogoUpload"] = dt;
    }
    public DataTable FillLogoTable()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateLogoDataTable();
        if (gvLogo.Rows.Count > 0)
        {
            for (int i = 0; i < gvLogo.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != gvLogo.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblSNo = (Label)gvLogo.Rows[i].FindControl("lblLogoId");
                    ImageButton lblImagePath = (ImageButton)gvLogo.Rows[i].FindControl("ImgLogo");

                    dt.Rows[i]["Trans_Id"] = lblSNo.Text;
                    strNewSNo = lblSNo.Text;
                    dt.Rows[i]["ImagePath"] = lblImagePath.ImageUrl.ToString();

                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["Trans_Id"] = (float.Parse(strNewSNo) + 1).ToString();
                    FileUploadImage.SaveAs(Server.MapPath("~/CompanyResource/Supplier/" + ViewState["SupplierCode"].ToString() + "_" + dt.Rows[i]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName)));
                    ViewState["Image"] = "~/CompanyResource/Supplier/" + ViewState["SupplierCode"].ToString() + "_" + dt.Rows[i]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName);
                    dt.Rows[i]["ImagePath"] = ViewState["Image"];
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";
            FileUploadImage.SaveAs(Server.MapPath("~/CompanyResource/Supplier/" + ViewState["SupplierCode"].ToString() + "_" + dt.Rows[0]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName)));

            ViewState["Image"] = "~/CompanyResource/Supplier/" + ViewState["SupplierCode"].ToString() + "_" + dt.Rows[0]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName);

            dt.Rows[0]["ImagePath"] = ViewState["Image"];

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 14-05-2015
            objPageCmn.FillData((object)gvLogo, dt, "", "");
            ViewState["LogoUpload"] = dt;
        }
        return dt;
    }
    #endregion
    #region LongitudeLatitude

    protected void lnkGetLatLong_Click(object sender, EventArgs e)
    {
        LinkButton lnkGotoMap = (LinkButton)sender;

        string[] arguments = lnkGotoMap.CommandArgument.ToString().Split(new char[] { ';' });

        string FullAddress = arguments[0];
        string longitute = arguments[1];
        string latitude = arguments[2];
        //txtAddress.Text + "," + txtCity.Text + "," + txtState.Text + "," + ddlCountry.SelectedItem.Text + "," + txtPinCode.Text;
        if ((longitute.ToString().Contains("0.0000")) && (latitude.ToString().Contains("0.0000")))
        {
            Session["Add"] = FullAddress;
        }
        else
        {
            Session["Long"] = longitute.ToString();
            Session["Lati"] = latitude.ToString();
        }




        ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../SystemSetup/GoogleMap.aspx','window','width=1024')", true);

    }

    public string GetContactLongitude(string AddressName)
    {
        string ContactLongitude = string.Empty;
        DataTable DtAddress = new DataTable();
        DtAddress = objAddMaster.GetAddressDataByAddressName(AddressName);
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
        DtAddress = objAddMaster.GetAddressDataByAddressName(AddressName);
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
    #region ProductCategory
    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString().ToString());
        if (dsCategory.Rows.Count > 0)
        {

            lstProductCategory.Items.Clear();


            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)lstProductCategory, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            lstProductCategory.Items.Add("No Category Available Here");
        }
    }
    #endregion

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (UploadFile.HasFile)
        {
            string filepath = string.Empty;
            int b = 0;
            string DirectoryName;
            try
            {
                filepath = "~/" + "ArcaWing/" + Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + ddlDocumentName.SelectedValue.ToString() + "/" + UploadFile.FileName;
                CreateDirectoryIfNotExist(Server.MapPath("~/" + "ArcaWing/" + Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + ddlDocumentName.SelectedValue.ToString()));
                DirectoryName = Session["CompId"].ToString() + "/Supplier/" + hdnSupplierId.Value + "/" + ddlDocumentName.SelectedValue.ToString();
                UploadFile.SaveAs(Server.MapPath(filepath));
            }
            catch
            {
            }
        }
    }


    protected void txtAddressName_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtAddressName.Text);
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
        addaddress.fillHeader(txtSupplierName.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
    }
    protected void btnAddressEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
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
        addaddress.fillHeader(txtSupplierName.Text);

        //used to fill location and location code
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
            dt.Rows[i]["FullAddress"] = (GvAddressName.Rows[i].FindControl("lblgvAddress") as Label).Text;
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


    protected void BtnMoreNumber_Command(object sender, CommandEventArgs e)
    {

        DataTable DtAddress = new DataTable();
        DtAddress = objAddMaster.GetAddressDataByAddressName(e.CommandArgument.ToString());
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
        DataTable DtAddress = objAddMaster.GetAddressDataByAddressName(data);
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



    protected void lnkAccountMaster_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];
        UcAcMaster.setUcAcMasterValues(CustomerId, "Supplier", myButton.Text);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);

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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        return str;

    }

    
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {

        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        return str;
    }
    
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvSupplier, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvSupplier, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void lbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString()+ "/SupplierMaster", "MasterSetup", "SupplierMaster", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void lnkContactDetail_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string SupplierId = arguments[0];
        DataTable dt = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), SupplierId);
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(SupplierId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_ContactInfo_Open()", true);
    }

    protected void rbtnupdall_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnUploadOb.Checked == true)
        {
            if (Session["ExcelSupplierObList"] != null)
            {
                List<clsObExcelImport> newLst = (List<clsObExcelImport>)Session["ExcelSupplierObList"];
                gvImport.DataSource = newLst;
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Count.ToString();
            }
        }
        else
        {
            if (Session["ExcelSupplierAgeingList"] != null)
            {
                List<clsAgeingExcelImport> newLst = (List<clsAgeingExcelImport>)Session["ExcelSupplierAgeingList"];
                gvImport.DataSource = newLst;
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Count.ToString();
            }
        }
    }

    protected void rbtnupdValid_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnUploadOb.Checked == true)
        {
            if (Session["ExcelSupplierObList"] != null)
            {
                List<clsObExcelImport> newLst = (List<clsObExcelImport>)Session["ExcelSupplierObList"];
                gvImport.DataSource = newLst.Where(m => m.is_valid == true).ToList();
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Where(m => m.is_valid == true).ToList().Count();
            }
        }
        else
        {
            if (Session["ExcelSupplierAgeingList"] != null)
            {
                List<clsAgeingExcelImport> newLst = (List<clsAgeingExcelImport>)Session["ExcelSupplierAgeingList"];
                gvImport.DataSource = newLst.Where(m => m.is_valid == true).ToList(); ;
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Where(m => m.is_valid == true).ToList().Count();
            }
        }
    }

    protected void rbtnupdInValid_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnUploadOb.Checked == true)
        {
            if (Session["ExcelSupplierObList"] != null)
            {
                List<clsObExcelImport> newLst = (List<clsObExcelImport>)Session["ExcelSupplierObList"];
                gvImport.DataSource = newLst.Where(m => m.is_valid == false).ToList();
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Where(m => m.is_valid == false).ToList().Count();
            }
        }
        else
        {
            if (Session["ExcelSupplierAgeingList"] != null)
            {
                List<clsAgeingExcelImport> newLst = (List<clsAgeingExcelImport>)Session["ExcelSupplierAgeingList"];
                gvImport.DataSource = newLst.Where(m => m.is_valid == false).ToList(); ;
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Where(m => m.is_valid == false).ToList().Count();
            }
        }
    }

    protected void rbtnUploadOb_CheckedChanged(object sender, EventArgs e)
    {
        resetUploadPanel();
        if (rbtnUploadOb.Checked == true)
        {
            lnkDownloadObData.Visible = true;
            lnkDownloadAgeingData.Visible = false;
        }
        else
        {
            lnkDownloadObData.Visible = false;
            lnkDownloadAgeingData.Visible = true;
        }
    }

    protected void rbtnUploadAgeing_CheckedChanged(object sender, EventArgs e)
    {
        resetUploadPanel();
        if (rbtnUploadOb.Checked == true)
        {
            lnkDownloadObData.Visible = true;
            lnkDownloadAgeingData.Visible = false;
        }
        else
        {
            lnkDownloadObData.Visible = false;
            lnkDownloadAgeingData.Visible = true;
        }
    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {

    }

    protected void btnResetUpload_Click(object sender, EventArgs e)
    {
        resetUploadPanel();
    }

    public void resetUploadPanel()
    {
        uploadOb.Visible = false;
        btnSaveExcelData.Enabled = false;
        Session["ExcelSupplierAgeingList"] = null;
        Session["ExcelSupplierObList"] = null;
    }

    protected void lnkDownloadAgeingData_Click(object sender, EventArgs e)
    {

    }

    public class clsBaseImport
    {
        public string supplier_id { get; set; }
        public string currency_id { get; set; }
        public string location_id { get; set; }
        public string other_account_id { get; set; }
        public string loc_currency_id { get; set; }
    }
    public class clsObExcelImport : clsBaseImport
    {
        public bool is_valid { get; set; }
        public string validation_remark { get; set; }
        public string location_name { get; set; }
        public string supplier_name { get; set; }
        public string currency { get; set; }
        public double opening_balance { get; set; }
        public string balance_type { get; set; }
        public double exchange_rate { get; set; }
    }
    public class clsAgeingExcelImport : clsObExcelImport
    {
        public string invoice_no { get; set; }
        public string invoice_date { get; set; }
        public double invoice_amount { get; set; }
        public double due_amount { get; set; }
    }

    protected void btnUploadExcel_Click(object sender, EventArgs e)
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

    public void Import(String path, int fileType)
    {
        try
        {
            //hdnTotalExcelRecords.Value = "0";
            //hdnInvalidExcelRecords.Value = "0";
            //hdnValidExcelRecords.Value = "0";

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

            using (OleDbConnection oledbConn = new OleDbConnection(strcon))
            {
                oledbConn.Open();
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                oleda.SelectCommand = cmd;
                DataSet ds = new DataSet();
                oleda.Fill(ds, "tbl1");

                if (rbtnUploadOb.Checked == true)
                {
                    List<clsObExcelImport> lstObImport = new List<clsObExcelImport> { };
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            clsObExcelImport _clsObj = new clsObExcelImport();
                            _clsObj.location_name = dr["location_name"].ToString().Trim();
                            _clsObj.supplier_name = dr["supplier_name"].ToString().Trim();
                            _clsObj.currency = dr["currency"].ToString().Trim();
                            Double ob = 0;
                            Double.TryParse(dr["opening_balance"].ToString().Trim(), out ob);
                            _clsObj.opening_balance = ob;
                            _clsObj.balance_type = dr["balance_type"].ToString().Trim();
                            if (!string.IsNullOrEmpty(_clsObj.balance_type))
                            {
                                if (_clsObj.balance_type.ToUpper() == "DEBIT" || _clsObj.balance_type.ToUpper() == "DR")
                                {
                                    _clsObj.balance_type = "DR";
                                }
                                else if (_clsObj.balance_type.ToUpper() == "CREDIT" || _clsObj.balance_type.ToUpper() == "CR")
                                {
                                    _clsObj.balance_type = "CR";
                                }
                            }
                            Double exchange_rate = 0;
                            Double.TryParse(dr["exchange_rate"].ToString().Trim(), out exchange_rate);
                            _clsObj.exchange_rate = exchange_rate==0?1:exchange_rate;
                            lstObImport.Add(_clsObj);
                        }
                        if (lstObImport.Count > 0)
                        {
                            List<clsObExcelImport> newObList = validateObExcelData(lstObImport);
                            if (newObList.Count > 0)
                            {
                                gvImport.DataSource = newObList;
                                gvImport.DataBind();
                                uploadOb.Visible = true;
                                lbltotaluploadRecord.Text = "Total Records:" + newObList.Count;
                                Session["ExcelSupplierObList"] = newObList;
                            }
                            else
                            {
                                Session["ExcelSupplierObList"] = null;
                            }
                        }
                    }
                }
                else
                {
                    List<clsAgeingExcelImport> lstAgeingImport = new List<clsAgeingExcelImport> { };
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            clsAgeingExcelImport _clsObj = new clsAgeingExcelImport();
                            _clsObj.location_name = dr["location_name"].ToString().Trim();
                            _clsObj.supplier_name = dr["supplier_name"].ToString().Trim();
                            _clsObj.currency = dr["currency"].ToString().Trim();
                            _clsObj.invoice_no = dr["invoice_no"].ToString().Trim();
                            _clsObj.invoice_date = dr["invoice_date"].ToString().Trim();
                            Double invoice_amt = 0;
                            Double.TryParse(dr["invoice_amount"].ToString().Trim(), out invoice_amt);
                            _clsObj.invoice_amount = invoice_amt;
                            Double due_amount = 0;
                            Double.TryParse(dr["due_amount"].ToString().Trim(), out due_amount);
                            _clsObj.due_amount = due_amount;
                            Double exchange_rate = 0;
                            Double.TryParse(dr["exchange_rate"].ToString().Trim(), out exchange_rate);
                            _clsObj.exchange_rate = exchange_rate == 0 ? 1 : exchange_rate;
                            lstAgeingImport.Add(_clsObj);
                        }
                        if (lstAgeingImport.Count > 0)
                        {
                            List<clsAgeingExcelImport> newAgeingList = validateAgeingExcelData(lstAgeingImport);
                            if (newAgeingList.Count > 0)
                            {
                                gvImport.DataSource = newAgeingList;
                                gvImport.DataBind();
                                uploadOb.Visible = true;
                                lbltotaluploadRecord.Text = "Total Records:" + newAgeingList.Count;
                                Session["ExcelSupplierAgeingList"] = newAgeingList;
                            }
                            else
                            {
                                Session["ExcelSupplierAgeingList"] = null;
                            }
                        }
                    }

                }
            }




        }
        catch (Exception ex)
        {
            //hdnInvalidExcelRecords.Value = "0";
            DisplayMessage("Error in excel uploading");
        }
        finally
        {
            if (gvImport.Rows.Count == 0)
            {
                btnSaveExcelData.Enabled = false;
            }
            else
            {
                btnSaveExcelData.Enabled = true;
            }

        }
    }



    protected void Btn_Li_Import_Click(object sender, EventArgs e)
    {

    }

    protected List<clsObExcelImport> validateObExcelData(List<clsObExcelImport> lstObExcel)
    {
        DataTable _dtSupplier = objSupplier.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataTable _dtCurrency = objCurrency.Get_ActiveCurrencyMaster();
        DataTable _dtLocation = objLocation.GetLocationMaster(Session["CompId"].ToString());

        ////find duplicate record by account_name
        //var lst = lstObExcel.GroupBy(x => x.x).Where(g => g.Count() > 1)
        //    .Select(y => y.Key)
        //    .ToList();
        //foreach (var ab in lst)
        //{
        //    foreach (var _cls in lstObExcel.Where(r => r.ac_name == ab))
        //    {
        //        _cls.is_valid = false;
        //        _cls.validation_remark = "duplicate record";
        //    }
        //}

        ////find duplicate record by account_no
        //lst = lstObExcel.GroupBy(x => x.ac_no).Where(g => g.Count() > 1)
        //    .Select(y => y.Key)
        //    .ToList();
        //foreach (var ab in lst)
        //{
        //    foreach (var _cls in lstObExcel.Where(r => r.ac_no == ab))
        //    {
        //        _cls.is_valid = false;
        //        _cls.validation_remark = "duplicate record";
        //    }
        //}

        foreach (var _cls in lstObExcel)
        {
            try
            {
                if (!string.IsNullOrEmpty(_cls.validation_remark))
                {
                    continue;
                }

                if (string.IsNullOrEmpty(_cls.location_name))
                {
                    _cls.location_id = Session["LocId"].ToString();
                    _cls.loc_currency_id = Session["LocCurrencyId"].ToString();
                }
                else
                {
                    using (DataTable _dtTempLoc = new DataView(_dtLocation, "Location_name='" + _cls.location_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_dtTempLoc.Rows.Count > 0)
                        {
                            _cls.location_id = _dtTempLoc.Rows[0]["Location_id"].ToString();
                            _cls.loc_currency_id = _dtTempLoc.Rows[0]["field1"].ToString();
                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "Invalidation Location";
                            continue;
                        }
                    }
                }
                if (string.IsNullOrEmpty(_cls.supplier_name))
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "invalid supplier";
                    continue;
                }
                else
                {
                    using (DataTable _tmpCustomer = new DataView(_dtSupplier, "Name='" + _cls.supplier_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpCustomer.Rows.Count > 0)
                        {
                            _cls.supplier_id = _tmpCustomer.Rows[0]["supplier_id"].ToString();
                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "invalid supplier";
                            continue;
                        }
                    }
                }

                if (string.IsNullOrEmpty(_cls.currency))
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "Invalid Currency";
                    continue;
                }
                else
                {
                    using (DataTable _tmpCurrency = new DataView(_dtCurrency, "Currency_Name='" + _cls.currency + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpCurrency.Rows.Count > 0)
                        {
                            _cls.currency_id = _tmpCurrency.Rows[0]["Currency_id"].ToString();
                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "Invalid Currency";
                            continue;
                        }
                    }
                }

                if (_cls.opening_balance < 0)
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "Invalid Opening balance";
                    continue;
                }

                if (_cls.opening_balance > 0)
                {
                    if (string.IsNullOrEmpty(_cls.balance_type))
                    {
                        _cls.is_valid = false;
                        _cls.validation_remark = "Invalid Balance Type";
                        continue;
                    }
                    else
                    {
                        if (_cls.balance_type.ToUpper() == "DR" || _cls.balance_type.ToUpper() == "CR" || _cls.balance_type.ToUpper() == "DEBIT" || _cls.balance_type.ToUpper() == "CREDIT")
                        {

                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "Invalid Balance Type";
                            continue;
                        }
                    }
                }

                if (_cls.currency_id != _cls.loc_currency_id)
                {
                    if (_cls.exchange_rate == 0)
                    {
                        _cls.is_valid = false;
                        _cls.validation_remark = "Location currency and balance currency is diffrenct so exchange rate should be there";
                        continue;
                    }
                }

                if (_cls.validation_remark == null)
                {
                    _cls.is_valid = true;
                }
            }
            catch (Exception ex)
            {
                _cls.validation_remark = ex.Message;
                _cls.is_valid = false;
            }
        }

        Session["ExcelSupplierObList"] = lstObExcel;
        return lstObExcel;

    }
    protected List<clsAgeingExcelImport> validateAgeingExcelData(List<clsAgeingExcelImport> lstObAgeing)
    {
        if (lstObAgeing.Count == 0)
        {
            return null;
        }
        DataTable _dtSupplier = objSupplier.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataTable _dtCurrency = objCurrency.Get_ActiveCurrencyMaster();
        DataTable _dtLocation = objLocation.GetLocationMaster(Session["CompId"].ToString());
        DataTable _dtAcMaster = objAccMaster.GetActiveAccountsWithFyearObByRefType("Supplier", Session["CompId"].ToString(), Session["FinanceYearId"].ToString());

        var grpAgeing = lstObAgeing.GroupBy(x => new { x.supplier_name, x.supplier_id })
            .Select(g => new { customer_name = g.Key.supplier_name, currency_id = g.Key.supplier_id, total_due_amt = g.Sum(x => x.due_amount) });


        ////find duplicate record by account_name
        //var lst = lstCof.GroupBy(x => x.ac_name).Where(g => g.Count() > 1)
        //    .Select(y => y.Key)
        //    .ToList();
        //foreach (var ab in lst)
        //{
        //    foreach (var _cls in lstCof.Where(r => r.ac_name == ab))
        //    {
        //        _cls.is_valid = false;
        //        _cls.validation_remark = "duplicate record";
        //    }
        //}

        ////find duplicate record by account_no
        //lst = lstCof.GroupBy(x => x.ac_no).Where(g => g.Count() > 1)
        //    .Select(y => y.Key)
        //    .ToList();
        //foreach (var ab in lst)
        //{
        //    foreach (var _cls in lstCof.Where(r => r.ac_no == ab))
        //    {
        //        _cls.is_valid = false;
        //        _cls.validation_remark = "duplicate record";
        //    }
        //}

        foreach (var _cls in lstObAgeing.Where(r => r.location_name == ""))
        {
            _cls.location_id = Session["LocId"].ToString();
            _cls.loc_currency_id = Session["LocCurrencyId"].ToString();
        }

        foreach (var _cls in lstObAgeing)
        {
            try
            {
                if (!string.IsNullOrEmpty(_cls.validation_remark))
                {
                    continue;
                }

                _cls.is_valid = false;

                if (string.IsNullOrEmpty(_cls.location_name))
                {
                    _cls.location_id = Session["LocId"].ToString();
                    _cls.loc_currency_id = Session["LocCurrencyId"].ToString();
                }
                else
                {
                    using (DataTable _dtTempLoc = new DataView(_dtLocation, "Location_name='" + _cls.location_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_dtTempLoc.Rows.Count > 0)
                        {
                            _cls.location_id = _dtTempLoc.Rows[0]["Location_id"].ToString();
                            _cls.loc_currency_id = _dtTempLoc.Rows[0]["field1"].ToString();
                        }
                        else
                        {
                            _cls.validation_remark = "Invalidation Location";
                            continue;
                        }
                    }
                }
                if (string.IsNullOrEmpty(_cls.supplier_name))
                {
                    _cls.validation_remark = "invalid Supplier";
                    continue;
                }
                else
                {
                    using (DataTable _tmpCustomer = new DataView(_dtSupplier, "Name='" + _cls.supplier_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpCustomer.Rows.Count > 0)
                        {
                            _cls.supplier_id = _tmpCustomer.Rows[0]["supplier_id"].ToString();
                        }
                        else
                        {
                            _cls.validation_remark = "invalid Supplier";
                            continue;
                        }
                    }
                }

                if (string.IsNullOrEmpty(_cls.currency))
                {
                    _cls.validation_remark = "Invalid Currency";
                    continue;
                }
                else
                {
                    using (DataTable _tmpCurrency = new DataView(_dtCurrency, "Currency_Name='" + _cls.currency + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpCurrency.Rows.Count > 0)
                        {
                            _cls.currency_id = _tmpCurrency.Rows[0]["Currency_id"].ToString();
                        }
                        else
                        {
                            _cls.validation_remark = "invalid Currency";
                            continue;
                        }
                    }
                }


                if (string.IsNullOrEmpty(_cls.invoice_no) || _cls.invoice_no == "0")
                {
                    _cls.validation_remark = "Invalid Invoice No";
                    continue;
                }

                try
                {
                    _cls.invoice_date = DateTime.Parse(_cls.invoice_date).ToString("dd-MMM-yyyy");
                }
                catch (Exception ex)
                {
                    _cls.validation_remark = "Invalid Invoice Date";
                    continue;
                }

                if (_cls.invoice_amount <= 0)
                {
                    _cls.validation_remark = "Invalid Invoice amount";
                    continue;
                }

                if (_cls.due_amount <= 0 || _cls.due_amount > _cls.invoice_amount)
                {
                    _cls.validation_remark = "Invalid due amount";
                    continue;
                }

                if (_cls.currency_id != _cls.loc_currency_id)
                {
                    if (_cls.exchange_rate == 0)
                    {
                        _cls.validation_remark = "Location currency and invoice currency is diffrenct so exchange rate should be there";
                        continue;
                    }
                }

                //check that account exist or not
                if (_dtAcMaster != null && _dtAcMaster.Rows.Count > 0)
                {
                    using (DataTable _tmpAcMaster = new DataView(_dtAcMaster, "location_id= '" + _cls.location_id + "' and ref_id='" + _cls.supplier_id + "' and currency_id='" + _cls.currency_id + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpAcMaster.Rows.Count == 0)
                        {
                            _cls.validation_remark = "Account does not exist or there is no opening balance on this location";
                            continue;
                        }
                        else
                        {
                            _cls.other_account_id = _tmpAcMaster.Rows[0]["Trans_id"].ToString();

                            var TotalDue = lstObAgeing.Where(x => x.supplier_name == _cls.supplier_name && x.currency_id == _cls.currency_id && x.location_id == _cls.location_id).Sum(x => x.due_amount);
                            //set supplier logic bcz supplier by default credit nature
                            double lBalance = 0;
                            double.TryParse(_tmpAcMaster.Rows[0]["lBalance"].ToString(), out lBalance);
                            lBalance = lBalance <= 0 ? Math.Abs(lBalance) : -lBalance;

                            double fBalance = 0;
                            double.TryParse(_tmpAcMaster.Rows[0]["fBalance"].ToString(), out fBalance);
                            fBalance = fBalance <= 0 ? Math.Abs(fBalance) : -fBalance;
                            if (_cls.loc_currency_id == _cls.currency_id)
                            {
                                if (TotalDue > lBalance)
                                {
                                    _cls.validation_remark = "Ageing is Greater then opening balance please check";
                                    continue;
                                }

                            }
                            else
                            {
                                if (TotalDue > fBalance)
                                {
                                    _cls.validation_remark = "Ageing is Greater then opening balance please check";
                                    continue;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _cls.validation_remark = "Account does not exist";
                    continue;
                }

                if (_cls.validation_remark == null)
                {
                    _cls.is_valid = true;
                }

            }
            catch (Exception ex)
            {
                _cls.validation_remark = ex.Message;
                _cls.is_valid = false;
            }
        }

        Session["ExcelSupplierAgeingList"] = lstObAgeing;
        return lstObAgeing;

    }
    protected void btnSaveExcelData_Click(object sender, EventArgs e)
    {
        if (!objFYI.isObUpdateAllow(Session["CompId"].ToString()))
        {
            DisplayMessage("system does allow to update opening balance bcz of closing year exist");
            return;
        }

        if (rbtnUploadOb.Checked == true)
        {
            saveObExcelData();
        }
        else
        {
            saveAgeingExcelData();
        }

    }

    public void saveObExcelData()
    {
        List<clsObExcelImport> lstObList = (List<clsObExcelImport>)Session["ExcelSupplierObList"];
        if (lstObList.Count == 0)
        {
            return;
        }

        if (lstObList.Where(m => m.is_valid == false).Count() > 0)
        {
            return;
        }
        string strSupplierAccount = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strAcDocNo = new Set_DocNumber(Session["DBConnection"].ToString()).GetDocumentNo(true, "0", false, "150", "400", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        Ac_AccountMaster objAccMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            foreach (var _cls in lstObList)
            {
                //delete existing data
                int otherAccountId = objAccMaster.GetSupplierAccountByCurrency(_cls.supplier_id, _cls.currency_id);
                if (otherAccountId == 0)
                {
                    //create new account in account master
                    int i = objAccMaster.InsertAc_AccountMaster("0", "Supplier", _cls.supplier_id, strAcDocNo, _cls.currency_id, "", "", "", false.ToString(), "01-Jan-1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    if (i > 0)
                    {
                        otherAccountId = i;
                        string sql = "select count(*) from ac_accountMaster";
                        string strRecCount = da.get_SingleValue(sql, ref trns);
                        string strAccNo = strRecCount == "0" ? strAcDocNo + "1" : strAcDocNo + strRecCount;
                        cmn.UpdateCodeForDocumentNo("ac_accountMaster", "Account_no", "Trans_Id", i.ToString(), strAccNo, ref trns);
                    }
                }
                else
                {
                    objSubCOA.deleteOtherAcIdAndFyId(Session["CompId"].ToString(), _cls.location_id, otherAccountId.ToString(), Session["FinanceYearId"].ToString(), ref trns);
                }
                double lDrAmt = _cls.balance_type.ToUpper() == "DR" ? _cls.opening_balance : 0;
                double lCrAmt = _cls.balance_type.ToUpper() == "CR" ? _cls.opening_balance : 0;
                double fDrAmt = lDrAmt * _cls.exchange_rate;
                double fCrAmt = lCrAmt * _cls.exchange_rate;
                int b = objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), _cls.location_id, strSupplierAccount, otherAccountId.ToString(), lDrAmt.ToString(), lCrAmt.ToString(), fDrAmt.ToString(), fCrAmt.ToString(), _cls.currency_id, "0", "0", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            trns.Commit();
            con.Close();
            DisplayMessage("Total " + lstObList.Count + " Records inserted successfully");
            gvImport.DataSource = null;
            gvImport.DataBind();
            resetUploadPanel();
        }
        catch (Exception ex)
        {
            trns.Rollback();
            con.Close();
            DisplayMessage(ex.Message);
        }
    }

    public void saveAgeingExcelData()
    {
        List<clsAgeingExcelImport> lstObList = (List<clsAgeingExcelImport>)Session["ExcelSupplierAgeingList"];
        if (lstObList.Count == 0)
        {
            return;
        }

        if (lstObList.Where(m => m.is_valid == false).Count() > 0)
        {
            return;
        }
        string strSupplierAccount = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        //string strAcDocNo = new Set_DocNumber().GetDocumentNo(true, "0", false, "150", "400", "0");

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            foreach (var _cls in lstObList)
            {
                //delete entry that exist with same invoice
                objAcAgeing.DeleteAgeingDetailByInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), _cls.location_id, "0", "PINV", _cls.invoice_no, _cls.other_account_id, ref trns);

                //insert new record
                int b = objAcAgeing.InsertAgeingDetail(Session["CompId"].ToString(),
                    Session["BrandId"].ToString(),
                    _cls.location_id,
                    "PINV",
                    "0",
                    _cls.invoice_no,
                    _cls.invoice_date,
                    strSupplierAccount,
                    _cls.other_account_id,
                    _cls.invoice_amount.ToString(),
                    "0",
                    _cls.due_amount.ToString(),
                    DateTime.Now.ToString(),
                    DateTime.Now.ToString(), "0",
                    "Opening",
                    Session["EmpId"].ToString(),
                    _cls.currency_id,
                    _cls.exchange_rate.ToString(),
                    (_cls.exchange_rate * _cls.due_amount).ToString(),
                    "0",
                    "0",
                    Session["FinanceYearId"].ToString(),
                    "PV",
                    "0", "", "", "0", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //delete existing data

            }
            trns.Commit();
            con.Close();
            DisplayMessage("Total " + lstObList.Count + " Records inserted successfully");
            gvImport.DataSource = null;
            gvImport.DataBind();
            resetUploadPanel();
        }
        catch (Exception ex)
        {
            trns.Rollback();
            con.Close();
            DisplayMessage(ex.Message);
        }
    }
    

   
}
