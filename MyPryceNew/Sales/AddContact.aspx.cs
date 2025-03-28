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
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.Collections.Generic;
using PegasusDataAccess;
using System.Data.OleDb;

public partial class Sales_Add_Contact : System.Web.UI.Page
{
    Common cmn = null;
    Ems_ContactMaster ObjContactMaster = null;
    Set_AddressMaster ObjAddMaster = null;
    ES_EmailMasterDetail objEmailDetail = null;
    CountryMaster ObjCountryMaster = null;
    Set_AddressChild objAddChild = null;
    ES_EmailMaster_Header objEmailHeader = null;
    Set_CustomerMaster ObjCustomerMaster = null;
    Ems_ContactCompanyBrand ObjCompanyBrand = null;
    Ems_Contact_Group objCG = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    Inv_ParameterMaster objInvParam = null;
    CountryMaster objCountryMaster = null;
    Set_DocNumber ObjDocumentNo = null;
    DepartmentMaster ObjDepMaster = null;
    DesignationMaster ObjDesMaster = null;
    ReligionMaster ObjRelMaster = null;
    Set_Suppliers ObjSupplierMaster = null;
    BrandMaster ObjBrandMaster = null;
    ContactNoMaster objContactnoMaster = null;
    DataAccessClass ObjDa = null;
    CurrencyMaster objCurrency = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        hdntxtaddressid.Value = txtAddressName.ID;
        cmn = new Common(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        ObjCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        ObjCustomerMaster = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Ems_ContactCompanyBrand(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjDocumentNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjDepMaster = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjDesMaster = new DesignationMaster(Session["DBConnection"].ToString());
        ObjRelMaster = new ReligionMaster(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
         objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/ERPLogin.aspx");
            }
            //string Footer = "<div class=*pull-right hidden-xs*>      <b>Version</b> " + ObjSysPeram.GetSysParameterByParamName("Application_Version").Rows[0]["Param_Value"].ToString() + "    </div>    <strong>Copyright &copy; " + DateTime.Now.Year.ToString() + " Pryce All rights reserved";
            //Ltr_Footer_Content.Text = Footer.Replace('*', '"');
            try
            {
                string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();


                hdncountryid.Value = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
                txtCountryName.Text = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Name"].ToString();
                ViewState["CountryCode"] = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Code"].ToString();
            }
            catch
            {
            }
            FillCountryCode();
            FillCurrencyDDL();
            txtId.Text = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
            ViewState["DocNo"] = txtId.Text;

            if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()) || Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTax").Rows[0]["ParameterValue"].ToString()))
            {
                tr_tincst.Visible = true;

            }
            else
            {
                tr_tincst.Visible = false;
            }
            FillDepartment();
            FillDesignation();
            FillReligion();
            AllPageCode();
            RdolistSelect.SelectedValue = "Company";
            RdolistSelect_SelectedIndexChanged(null, null);
            chkVerify.Checked = true;
        }
    }
    public void AllPageCode()
    {
        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
        //Common ObjComman = new Common();

        ////New Code by jitendra on 09-12-2014
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        //DataTable dtModule = objObjectEntry.GetModuleIdAndName("19");
        //if (dtModule.Rows.Count > 0)
        //{
        //    strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        //    strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        //}
        //else
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}


        //if (Session["EmpId"].ToString() == "0")
        //{
        //    chkVerify.Visible = true;
        //    chkVerify.Checked = true;
        //    btnAddNewAddress.Visible = true;
        //    imgAddAddressName.Visible = true;
        //}
        //DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "20");

        //if (dtAllPageCode.Rows.Count == 0)
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}
        //foreach (DataRow DtRow in dtAllPageCode.Rows)
        //{

        //    if (DtRow["Op_Id"].ToString() == "11")
        //    {
        //        chkVerify.Visible = true;
        //        chkVerify.Checked = true;
        //    }
        //    if (DtRow["Op_Id"].ToString() == "1")
        //    {
        //        btnAddNewAddress.Visible = true;
        //        imgAddAddressName.Visible = true;
        //    }

        //}
    }
    public void FillCountryCode()
    {
        try
        {
            string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

            ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();

            ViewState["CountryCode"] = objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        }
        catch
        {

        }

        DataTable dt = objCountryMaster.getCountryCallingCode();
        if (dt.Rows.Count > 0)
        {

            ddlCountryCode.DataSource = dt;
            ddlCountryCode.DataTextField = "CountryCodeName";
            ddlCountryCode.DataValueField = "CountryCodeValue";
            ddlCountryCode.DataBind();


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
    private void FillDepartment()
    {
        DataTable dsDepartment = null;
        dsDepartment = ObjDepMaster.GetDepartmentMaster();
        dsDepartment = new DataView(dsDepartment, "", "Dep_Name Asc", DataViewRowState.CurrentRows).ToTable();
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
        txtPermanentMobileNo.Text = "";

        txtAddressName.Text = "";
        txtCivilId.Text = "";

        GvAddressName.DataSource = null;
        GvAddressName.DataBind();
        FillDepartment();
        FillDesignation();
        FillReligion();

        txtCompany.Text = "";
        chkIsEmail.Checked = false;
        chkIsSMS.Checked = false;
        hdnContactId.Value = "";
        pnlbasic.Visible = true;
        RdolistSelect_SelectedIndexChanged(null, null);
        txtId.Text = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        ViewState["DocNo"] = txtId.Text;

        ddlNamePrefix.SelectedIndex = 0;

        rbnEmailList.Items.Clear();
        rbnEmailList.SelectedIndex = -1;
        txtCountryName.Text = "";
        try
        {
            string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();


            hdncountryid.Value = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
            txtCountryName.Text = objCountryMaster.GetCountryMasterById(hdncountryid.Value).Rows[0]["Country_Name"].ToString();
        }
        catch
        {
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

        txtTinno.Text = "";
        txtCstNo.Text = "";

        //  ddlCountryCode.Visible = true;

    }


    protected void RdolistSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdolistSelect.SelectedValue == "Individual")
        {
            TbRefContact.Visible = true;
            ddlNamePrefix.Visible = true;
            //txtName.Width = 350;
            txtName.Style.Add("width", "70%");
        }
        if (RdolistSelect.SelectedValue == "Company")
        {
            TbRefContact.Visible = false;
            ddlNamePrefix.Visible = false;
            //txtName.Width = 412;
            txtName.Style.Add("width", "100%");
        }

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

                    DisplayMessage("Code Already exists");
                    txtId.Text = "";
                    txtId.Focus();
                    return;
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
    public void DisplayMessage(string Message, string color = "orange")
    {

        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + Message + "','" + color + "','white');", true);
    }

    #region Addresss

    public DataTable CreateAddressDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id");
        dt.Columns.Add("Address_Name");
        dt.Columns.Add("Address");
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
                        dt.Rows[i]["Address"] = GetAddressByAddressName(lblAddressName.Text);
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
                        dt.Rows[i]["Address"] = GetAddressByAddressName(txtAddressName.Text);
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
                dt.Rows[0]["Address"] = GetAddressByAddressName(txtAddressName.Text);
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

        }
        return dt;

    }
    public string GetAddressByAddressName(string AddressName)
    {
        string Address = string.Empty;
        DataTable dt = ObjAddMaster.GetAddressDataByAddressName(AddressName);

        if (dt.Rows.Count > 0)
        {

            Address = dt.Rows[0]["Address"].ToString();

            if (dt.Rows[0]["Street"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Street"].ToString();

            }
            if (dt.Rows[0]["Block"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Block"].ToString();

            }
            if (dt.Rows[0]["Avenue"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Avenue"].ToString();

            }
            if (dt.Rows[0]["CityId"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["CityId"].ToString();

            }
            if (dt.Rows[0]["StateId"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["StateId"].ToString();

            }
            if (dt.Rows[0]["Country_Name"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["Country_Name"].ToString();

            }
            if (dt.Rows[0]["PinCode"].ToString() != "")
            {
                Address += "," + dt.Rows[0]["PinCode"].ToString();

            }


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
                DisplayMessage("Enter Valid EmailId");
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


    #region addcountry
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
            DataTable dt = ObjCountryMaster.GetCountryMasterByCountryName(txtCountryName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                hdncountryid.Value = dt.Rows[0]["Country_Id"].ToString();
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
    #endregion

    #region CompanyName
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListComapnyName(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjContactMaster.GetAutoCompleteContactTrueAllData(prefixText), "Status='Company'", "Name Asc", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {

            //dt = new DataView(ObjContactMaster.GetContactAllData(), "Status='Company'", "Name Asc", DataViewRowState.CurrentRows).ToTable();

        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
        }
        return txt;
    }

    #endregion



    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        string strIsEmail = string.Empty;
        string strIsSMS = string.Empty;
        string strDepartmentId = string.Empty;
        string strDesignationId = string.Empty;
        string strReligionId = string.Empty;
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
        string CountryCodewithMobileNumber = string.Empty;
        if (txtPermanentMobileNo.Text != "")
        {
            CountryCodewithMobileNumber = ddlCountryCode.SelectedValue + "-" + txtPermanentMobileNo.Text;
        }
        b = ObjContactMaster.InsertContactMaster(txtId.Text.Trim(), txtName.Text, txtNameL.Text, txtCivilId.Text, strDepartmentId, strDesignationId, strReligionId, StrComp, strIsEmail, strIsSMS, Status.ToString(), chkIsReseller.Checked.ToString(), DefaultEmailId.ToString(), CountryCodewithMobileNumber, strNamePrefix.ToString(), hdncountryid.Value, "", chkVerify.Checked.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0");

        objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster",b.ToString(), "Mobile", ddlCountryCode.SelectedValue, txtPermanentMobileNo.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            int Id = b;
            if (txtId.Text == ViewState["DocNo"].ToString())
            {
                ObjContactMaster.UpdateContactMaster(Id.ToString(), txtId.Text + Id.ToString());

                txtId.Text = txtId.Text + Id.ToString();
            }
            hdnContactId.Value = Id.ToString();
            DisplayMessage("Record Saved","green");
        }
        else
        {
            DisplayMessage("Record Not Saved");
            return;
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

                        objAddChild.InsertAddressChild(strAddressId, "Contact", hdnContactId.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objAddChild.InsertAddressChild(strAddressId, "Customer", hdnContactId.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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

        //in customer master and supplier master table insret record accordng query string 
        DataTable dt = ObjBrandMaster.GetBrandMaster(Session["CompId"].ToString());
        string id = string.Empty;

        if (Request.QueryString["Page"].ToString().Trim() == "SINV")
        {
            id = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "18", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + hdnContactId.Value;
            foreach (DataRow dr in dt.Rows)
            {
                ObjCustomerMaster.InsertCustomerMaster(Session["CompId"].ToString(), dr["Brand_Id"].ToString(), hdnContactId.Value.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", false.ToString(), "0", false.ToString(), "", txtTinno.Text, txtCstNo.Text, "", "False", "Approved", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "","0","");
                ObjCompanyBrand.InsertContactCompanyBrand(hdnContactId.Value.Trim(), Session["CompId"].ToString(), dr["Brand_Id"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());

                string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "399", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                string sql = "select count(*) from ac_accountMaster";
                string strRecCount = ObjDa.get_SingleValue(sql);
                AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                ObjDa.execute_Command("delete from Ac_AccountMaster where Ref_Type='Customer' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");
                ObjDa.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Customer', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')");
            }
            objCG.InsertContactGroup(hdnContactId.Value, "1", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), id, txtTinno.Text, txtCstNo.Text, "", "", true.ToString(), DateTime.Now.ToString());
        }
        if (Request.QueryString["Page"].ToString().Trim() == "PINV"|| Request.QueryString["Page"].ToString().Trim() == "PO")
        {
            id = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "20", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString()) + hdnContactId.Value;
            foreach (DataRow dr in dt.Rows)
            {
                ObjSupplierMaster.InsertSupplierMaster(Session["CompId"].ToString(), dr["Brand_Id"].ToString(), hdnContactId.Value.ToString(), "0", "0", "0", "0", "0", "0", "0", txtTinno.Text, txtCstNo.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "");
                ObjCompanyBrand.InsertContactCompanyBrand(hdnContactId.Value.Trim(), Session["CompId"].ToString(), dr["Brand_Id"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());

                string AccountDocNo = ObjDocumentNo.GetDocumentNo(true, "0", false, "150", "Supplier" == "Customer" ? "399" : "400", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                string sql = "select count(*) from ac_accountMaster";
                string strRecCount = ObjDa.get_SingleValue(sql);
                AccountDocNo = AccountDocNo + (int.Parse(strRecCount) + 1);
                ObjDa.execute_Command("delete from Ac_AccountMaster where Ref_Type='Supplier' And Ref_Id='" + hdnContactId.Value.ToString() + "' and Currency_Id='" + ddlCurrency.SelectedValue + "'");
                ObjDa.execute_Command("Insert Into Ac_AccountMaster([Ref_Type],[Ref_Id],[Account_No],[Currency_id],[Field1],[Field2],[Field3],[Field4],[Field5],[Is_Active],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])values('Supplier', '" + hdnContactId.Value.ToString() + "', '" + AccountDocNo + "', '" + ddlCurrency.SelectedValue + "', '', '', '', '0', '1900-01-01 00:00:00.000', '1', '" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')");
            }
            objCG.InsertContactGroup(hdnContactId.Value, "2", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), id, txtTinno.Text, txtCstNo.Text, "", "", true.ToString(), DateTime.Now.ToString());

        }

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
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);

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

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        pnlAddress1.Visible = false;
        pnlAddress2.Visible = false;
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

    protected void txtAddressName_TextChanged(object sender, EventArgs e)
    {
        if (txtAddressName.Text != "")
        {
            DataTable dtAM = ObjAddMaster.GetAddressDataByAddressName(txtAddressName.Text);
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
            dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
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
        new PageControlCommon(Session["DBConnection"].ToString()).FillData((object)GvAddressName, dt, "", "");
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
            dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
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
            dt.Rows[i]["Address"] = GetAddressByAddressName(lblgvAddressName.Text);
            dt.Rows[i]["Is_Default"] = ((CheckBox)GvAddressName.Rows[i].FindControl("chkdefault")).Checked;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnAddressId.Value == dt.Rows[i]["Trans_Id"].ToString())
            {
                dt.Rows[i]["Address_Name"] = txtAddressName.Text;
                dt.Rows[i]["Address"] = GetAddressByAddressName(txtAddressName.Text);
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
}