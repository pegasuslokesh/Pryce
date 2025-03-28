using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;

public partial class MasterSetUp_CustomerMaster : BasePage
{
    #region defined Class Object
    Common cmn = null;
    CompanyMaster ObjCompany = null;
    Ac_SubChartOfAccount objSubCOA = null;
    Inv_ParameterMaster objInvParam = null;
    Set_CustomerMaster objCustomer = null;
    Ac_ChartOfAccount objCOf = null;
    Set_AddressChild objAddChild = null;
    CountryMaster ObjCountry = null;
    Set_AddressMaster objAddMaster = null;
    Ems_ContactMaster objContact = null;
    SystemParameter ObjSysParam = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster objDepartment = null;
    ReligionMaster objReligion = null;
    DesignationMaster objDesg = null;
    Set_BankMaster objBankMaster = null;
    Contact_BankDetail objContactBankDetail = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    Contact_PriceList objContactPriceList = null;
    Ems_GroupMaster objGroup = null;
    Set_CustomerSupplier_Logo objCustSupLogo = null;
    NotificationMaster Obj_Notifiacation = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    CurrencyMaster objCurrency = null;
    CountryMaster objCountryMaster = null;
    Ac_Finance_Year_Info objFYI = null;
    ContactNoMaster objContactnoMaster = null;
    Set_CustomerMaster_CompanyInfo objCustomerCompanyInfo = null;
    Set_CustomerMaster_SponserDetail objSponserDetail = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    Set_CustomerMaster_AuthorizedPerson_Detail objAuthorizedpersondetail = null;
    Set_CustomerMaster_TradeReferences objCustomerTradeReference = null;
    Set_CustomerMaster_BankReferences objCustomerBankReference = null;
    Set_Approval_Employee objEmpApproval = null;
    DataAccessClass da = null;
    Ac_ChartOfAccount objCOA = null;
    Ems_Contact_ProductCategory ObjContactProductcategory = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    PageControlsSetting objPageCtlSettting = null;
    Ac_AccountMaster objAccMaster = null;
    Ac_Ageing_Detail objAcAgeing = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrUserId = string.Empty;
    string strLocationId = string.Empty;
    public const int grdDefaultColCount = 7;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        hdntxtaddressid.Value = txtAddressName.ID;
        StrCompId = Session["CompId"].ToString();
        StrUserId = Session["UserId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        cmn = new Common(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objSubCOA = new Ac_SubChartOfAccount(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objCOf = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objDepartment = new DepartmentMaster(Session["DBConnection"].ToString());
        objReligion = new ReligionMaster(Session["DBConnection"].ToString());
        objDesg = new DesignationMaster(Session["DBConnection"].ToString());
        objBankMaster = new Set_BankMaster(Session["DBConnection"].ToString());
        objContactBankDetail = new Contact_BankDetail(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objContactPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        objCustSupLogo = new Set_CustomerSupplier_Logo(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objCustomerCompanyInfo = new Set_CustomerMaster_CompanyInfo(Session["DBConnection"].ToString());
        objSponserDetail = new Set_CustomerMaster_SponserDetail(Session["DBConnection"].ToString());
        objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        objAuthorizedpersondetail = new Set_CustomerMaster_AuthorizedPerson_Detail(Session["DBConnection"].ToString());
        objCustomerTradeReference = new Set_CustomerMaster_TradeReferences(Session["DBConnection"].ToString());
        objCustomerBankReference = new Set_CustomerMaster_BankReferences(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjContactProductcategory = new Ems_Contact_ProductCategory(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objAccMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objAcAgeing = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../MasterSetup/CustomerMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            StrCompId = Session["CompId"].ToString();
            StrUserId = Session["UserId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Session["CHECKED_ITEMS"] = null;
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGridBin();
            FillGroup();
            FillCurrencyDDL();
            Session["DtCustomerPriceList"] = null;
            Session["DtContactBank"] = null;
            BtnShowCpriceList.Visible = false;
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            FillGrid();

            GvCustomer.PageSize = PageControlCommon.GetPageSize();
            FillCustomerCountryCode(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString());
            FillProductCategory();
            FillCountryCode();
            FillOpeningLocation();
            //Li_New.Style.Add("display", "none");
            AllPageCode(clsPagePermission);
            getPageControlsVisibility();
        }

    }
    public void FillOpeningLocation()
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

    public void FillCurrencyDDL()
    {
        DataTable dt = objCurrency.GetCurrencyMaster();

        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            objPageCmn.FillData((object)ddlCurrencyCreditLimit, dt, "Currency_Name", "Currency_Id");
        }
        else
        {
            try
            {
                ddlCurrencyCreditLimit.Items.Insert(0, "--Select--");
                ddlCurrencyCreditLimit.SelectedIndex = 0;

                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
            catch
            {

                ddlCurrencyCreditLimit.Items.Insert(0, "--Select--");
                ddlCurrencyCreditLimit.SelectedIndex = 0;

                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
        }
    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();


        btnCustomerSave.Visible = clsPagePermission.bAdd;

        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btnAddNewAddress.Visible = clsPagePermission.bAdd;
        imgAddAddressName.Visible = clsPagePermission.bAdd;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    #endregion

    #region System defined Function
    protected void btnList_Click(object sender, EventArgs e)
    {
        PanelView1.Visible = false;
        PanelView2.Visible = false;

        txtValue.Focus();
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

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        string strCurrency = Session["LocCurrencyId"].ToString();
        //string objSenderID;
        hdnCustomerId.Value = e.CommandArgument.ToString();
        Session["cm_customerId"] = hdnCustomerId.Value;
        DataTable dtCustomerEdit = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnCustomerId.Value);

        if (((LinkButton)sender).ID.Trim() == "btnEdit")
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;

        }

        if (dtCustomerEdit.Rows.Count > 0)
        {


            txtId.Text = dtCustomerEdit.Rows[0]["Customer_Code"].ToString();
            txtCustomerName.Text = dtCustomerEdit.Rows[0]["Name"].ToString();
            txtLCustomerName.Text = dtCustomerEdit.Rows[0]["Name_L"].ToString();
            txtCivilId.Text = dtCustomerEdit.Rows[0]["Civil_Id"].ToString();

            if (dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "SELECT" || dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "")
            {
                ratingControl.CurrentRating = 0;
            }
            else if (dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "A")
            {
                ratingControl.CurrentRating = 5;
            }
            else if (dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "B")
            {
                ratingControl.CurrentRating = 4;
            }
            else if (dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "C")
            {
                ratingControl.CurrentRating = 3;
            }
            else if (dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "D")
            {
                ratingControl.CurrentRating = 2;
            }
            else if (dtCustomerEdit.Rows[0]["Grade"].ToString().ToUpper() == "E")
            {
                ratingControl.CurrentRating = 1;
            }
            else
            {
                ratingControl.CurrentRating = Convert.ToInt32(dtCustomerEdit.Rows[0]["Grade"].ToString());
            }


            if (dtCustomerEdit.Rows[0]["MarketingEmpName"].ToString() != "")
            {
                txtMarketingEmp.Text = dtCustomerEdit.Rows[0]["MarketingEmpName"].ToString() + "/" + dtCustomerEdit.Rows[0]["Market_by_emp_id"].ToString();
            }


            string sql = "select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "', '" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "', '0','" + Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()) + "','" + hdnCustomerId.Value + "','1','" + Session["FinanceYearId"].ToString() + "')) OpeningBalance";
            string strBalance = da.get_SingleValue(sql);
            strBalance = SystemParameter.GetAmountWithDecimal(strBalance, Session["LoginLocDecimalCount"].ToString());
            lnkCustomerBalance.Text = strBalance;


            //imgCustomer.ImageUrl = dtCustomerEdit.Rows[0]["ImagePath"].ToString();
            ViewState["Image"] = dtCustomerEdit.Rows[0]["ImagePath"].ToString();
            ViewState["CustomerCode"] = dtCustomerEdit.Rows[0]["Customer_Code"].ToString();
            ViewState["Status"] = dtCustomerEdit.Rows[0]["Field51"].ToString();
            //end vs





            if (dtCustomerEdit.Rows[0]["Field1"].ToString() == "1")
            {
                rbtnSystemsalesprice.Checked = true;
                rbtnCustomerpricelist.Checked = false;
                BtnShowCpriceList.Visible = false;
            }
            if (dtCustomerEdit.Rows[0]["Field1"].ToString() == "2")
            {
                BtnShowCpriceList.Visible = true;
                rbtnCustomerpricelist.Checked = true;
                rbtnSystemsalesprice.Checked = false;
                DataTable DtCPrice = new DataTable();
                DtCPrice.Columns.Add("Product_Id");
                DtCPrice.Columns.Add("Sales_Price");
                DataTable dtcustomerPriceList = objContactPriceList.GetContactPriceList(StrCompId, e.CommandArgument.ToString(), "C");
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


            DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Customer", hdnCustomerId.Value);
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

                //AllPageCode();
            }
            string strCustomerAccount = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
            //For Accounts data               
            //DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
            //DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtCash.Rows.Count > 0)
            //{
            //    strCustomerAccount = dtCash.Rows[0]["Param_Value"].ToString();
            //}
            //End
            if (dtCustomerEdit.Rows[0]["Account_No"].ToString() != "0")
            {
                txtAccountNo.Text = GetAccountName(dtCustomerEdit.Rows[0]["Account_No"].ToString()).ToString() + "/" + dtCustomerEdit.Rows[0]["Account_No"].ToString();
            }
            else
            {
                txtAccountNo.Text = GetAccountName(strCustomerAccount).ToString() + "/" + strCustomerAccount;
            }

            ddlCustomerType.SelectedValue = dtCustomerEdit.Rows[0]["Customer_Type"].ToString();
            try
            {
                ddlCurrency.SelectedValue = dtCustomerEdit.Rows[0]["Field31"].ToString();
                ddlCurrencyCreditLimit.SelectedValue = dtCustomerEdit.Rows[0]["Field11"].ToString();
            }
            catch
            {
                ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
                ddlCurrencyCreditLimit.SelectedValue = Session["LocCurrencyId"].ToString();
            }

            string strFoundEmployeeId = dtCustomerEdit.Rows[0]["Found_By_Emp"].ToString();
            if (strFoundEmployeeId != "0")
            {
                txtFoundEmployee.Text = GetEmployeeName(strFoundEmployeeId) + "/" + strFoundEmployeeId;
            }
            string strHandeledEmployeeId = dtCustomerEdit.Rows[0]["Handled_By_Emp"].ToString();
            if (strHandeledEmployeeId != "0")
            {
                txtHandledEmployee.Text = GetEmployeeName(strHandeledEmployeeId) + "/" + strHandeledEmployeeId;
            }
            txtDebitAmount.Text = dtCustomerEdit.Rows[0]["Db_Amount"].ToString();
            txtDebitAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtDebitAmount.Text);
            txtCreditAmount.Text = dtCustomerEdit.Rows[0]["Cr_Amount"].ToString();
            txtCreditAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtCreditAmount.Text);
            txtODebitAmount.Text = dtCustomerEdit.Rows[0]["O_Db_Amount"].ToString();
            txtODebitAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtODebitAmount.Text);
            txtOCreditAmount.Text = dtCustomerEdit.Rows[0]["O_Cr_Amount"].ToString();
            txtOCreditAmount.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtOCreditAmount.Text);
            txtSalesQuota.Text = ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), dtCustomerEdit.Rows[0]["Sales_Quota"].ToString());
            txtCreditLimit.Text = dtCustomerEdit.Rows[0]["Credit_Limit"].ToString();
            txtCreditDays.Text = dtCustomerEdit.Rows[0]["Credit_Days"].ToString();
            txtPriceLevel.Text = dtCustomerEdit.Rows[0]["Price_Level"].ToString();
            string strIsTaxable = dtCustomerEdit.Rows[0]["Is_Taxable"].ToString();
            ddlIsCredit.SelectedValue = dtCustomerEdit.Rows[0]["Field41"].ToString();

            if (dtCustomerEdit.Rows[0]["Field11"].ToString() == "1")
            {
                rbtnSystemsalesprice.Checked = true;
                rbtnCustomerpricelist.Checked = false;
            }
            else if (dtCustomerEdit.Rows[0]["Field11"].ToString() == "2")
            {
                rbtnSystemsalesprice.Checked = false;
                rbtnCustomerpricelist.Checked = true;
            }
            if (strIsTaxable == "True")
            {
                chkIsTaxable.Checked = true;
            }
            else if (strIsTaxable == "False")
            {
                chkIsTaxable.Checked = false;
            }


            //for get selected product category in editable mode
            //code created by jitendra upadhyay pn 28-09-2016
            FillProductCategory();
            DataTable dtProductCate = ObjContactProductcategory.GetDateBy_ContactId_and_ContactType(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnCustomerId.Value, "C");
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




            //For Location Opening Balance 
            //code commented by neelkanth purohit - 03/09/2018 (bcz of new logic of customer account)
            //foreach (GridViewRow gvr in GvLocation.Rows)
            //{
            //    HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
            //    HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
            //    TextBox txtgvDebit = (TextBox)gvr.FindControl("txtgvDebit");
            //    TextBox txtgvCredit = (TextBox)gvr.FindControl("txtgvCredit");
            //    TextBox txtgvForeignDebit = (TextBox)gvr.FindControl("txtgvForeignDebit");
            //    TextBox txtgvForeignCredit = (TextBox)gvr.FindControl("txtgvForeignCredit");

            //    DataTable dtSubCOA = objSubCOA.GetSubCOAAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, Session["FinanceYearId"].ToString());
            //    if (dtSubCOA.Rows.Count > 0)
            //    {
            //        dtSubCOA = new DataView(dtSubCOA, "AccTransId='" + strCustomerAccount + "' and Other_Account_No='" + hdnCustomerId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        if (dtSubCOA.Rows.Count > 0)
            //        {
            //            txtgvDebit.Text = dtSubCOA.Rows[0]["LDr_Amount"].ToString();
            //            txtgvDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvDebit.Text);
            //            txtgvCredit.Text = dtSubCOA.Rows[0]["LCr_Amount"].ToString();
            //            txtgvCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvCredit.Text);
            //            txtgvForeignDebit.Text = dtSubCOA.Rows[0]["FDr_Amount"].ToString();
            //            txtgvForeignDebit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignDebit.Text);
            //            txtgvForeignCredit.Text = dtSubCOA.Rows[0]["FCr_Amount"].ToString();
            //            txtgvForeignCredit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, txtgvForeignCredit.Text);
            //        }
            //    }
            //}
            //End


            //here we select the contact bank detail from the contact_bankdetail table and bind the gvcontactbandetail gridview in editable mode
            //this code is created on 08-march-2014 by jitendra upadhyay
            DataTable DtConactBankDetail = new DataTable();
            DtConactBankDetail.Columns.Add("Trans_Id");

            DtConactBankDetail.Columns.Add("Bank_Name");
            DtConactBankDetail.Columns.Add("Account_No");
            DtConactBankDetail.Columns.Add("Branch_Address");
            DtConactBankDetail.Columns.Add("IFSC_Code");
            DtConactBankDetail.Columns.Add("MICR_Code");
            DtConactBankDetail.Columns.Add("Branch_Code");
            DtConactBankDetail.Columns.Add("IBAN_NUMBER");
            DataTable Dt = objContactBankDetail.GetContactBankDetail_By_ContactId_And_GroupId(e.CommandArgument.ToString(), "1");
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
                dr["IBAN_NUMBER"] = Dt.Rows[i]["IBAN_NUMBER"].ToString();

                DtConactBankDetail.Rows.Add(dr);

            }
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, DtConactBankDetail, "", "");
            Session["DtContactBank"] = DtConactBankDetail;

            //here select the record from file transaction

            //code for Multiple Logo
            DataTable dtLogo = objCustSupLogo.getSet_CustomerSupplier_LogoByIdAndGroup(hdnCustomerId.Value, "0");
            ViewState["LogoUpload"] = dtLogo;
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvLogo, (DataTable)ViewState["LogoUpload"], "", "");

            //End Code For Multiple Logo


            //here we get customer detail related credit detail 

            //first we get company info

            DataTable dtCompanyInfo = objCustomerCompanyInfo.GetRecord_By_CustomerId(hdnCustomerId.Value);


            if (dtCompanyInfo.Rows.Count > 0)
            {
                txtCompanyName.Text = dtCompanyInfo.Rows[0]["Customer_Company_Name"].ToString().Trim() + "/" + dtCompanyInfo.Rows[0]["Company_Name"].ToString().Trim();
                txtCompanyAddress.Text = dtCompanyInfo.Rows[0]["Company_Address"].ToString().Trim();
                int strCompanyAddress = 0;
                int.TryParse(dtCompanyInfo.Rows[0]["Company_Address"].ToString().Trim(), out strCompanyAddress);
                if (strCompanyAddress > 0)
                {
                    using (DataTable _dt = objAddMaster.GetAddressDataByTransId(strCompanyAddress.ToString(), Session["CompId"].ToString()))
                    {
                        if (_dt.Rows.Count > 0)
                        {
                            txtCompanyAddress.Text = _dt.Rows[0]["Address_name"].ToString();
                        }
                    }
                }

                txtCompanyPermanentMobileNo.Text = dtCompanyInfo.Rows[0]["Company_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                ddlCompanyMobileNoCountryCode.SelectedValue = dtCompanyInfo.Rows[0]["Company_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtCompanyFaxNo.Text = dtCompanyInfo.Rows[0]["Company_Fax_No"].ToString().Trim().Split('-')[1].ToString();
                ddlCompanyFaxNoCountryCode.SelectedValue = dtCompanyInfo.Rows[0]["Company_Fax_No"].ToString().Trim().Split('-')[0].ToString();
                txtCompanyEmailId.Text = dtCompanyInfo.Rows[0]["Company_Email_Id"].ToString().Trim();
                txtCompanyWebsite.Text = dtCompanyInfo.Rows[0]["Company_WebSite"].ToString().Trim();
                ddlBusinessNature.SelectedValue = dtCompanyInfo.Rows[0]["Company_Business_Nature"].ToString().Trim();

                if (txtCompanyName.Text.Trim() == "" || txtCompanyName.Text.Trim() == "/0")
                {
                    txtCompanyName.Text = dtCustomerEdit.Rows[0]["Name"].ToString() + "/" + dtCustomerEdit.Rows[0]["Customer_Id"].ToString();
                    txtCompanyName_TextChanged(null, null);
                }
            }
            else
            {
                txtCompanyName.Text = "";
                txtCompanyAddress.Text = "";
                txtCompanyPermanentMobileNo.Text = "";
                txtCompanyFaxNo.Text = "";
                txtCompanyEmailId.Text = "";
                txtCompanyWebsite.Text = "";
                try
                {
                    ddlCompanyMobileNoCountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlCompanyFaxNoCountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                }
                catch
                {
                }
                txtCompanyName.Text = dtCustomerEdit.Rows[0]["Name"].ToString() + "/" + dtCustomerEdit.Rows[0]["Customer_Id"].ToString();
                txtCompanyName_TextChanged(null, null);
            }


            //here we get company owner or sponser detail

            DataTable dtSponserDetail = objSponserDetail.GetRecord_By_CustomerId(hdnCustomerId.Value);

            if (dtSponserDetail.Rows.Count > 0)
            {
                txtContactPerson_Accounts.Text = dtSponserDetail.Rows[0]["Account_Contact_Person_Name"].ToString().Trim() + "/" + dtSponserDetail.Rows[0]["Accounts_Contact_Person"].ToString().Trim();
                ddlContactNo_Accounts_CountryCode.SelectedValue = dtSponserDetail.Rows[0]["Accounts_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtContactNo_Accounts.Text = dtSponserDetail.Rows[0]["Accounts_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                txtEmailId_Accounts.Text = dtSponserDetail.Rows[0]["Accounts_Email_Id"].ToString().Trim();


                txtContactPerson_Sales.Text = dtSponserDetail.Rows[0]["Sales_Contact_Person_Name"].ToString().Trim() + "/" + dtSponserDetail.Rows[0]["Sales_Contact_Person"].ToString().Trim();
                ddlContactNo_Sales_CountryCode.SelectedValue = dtSponserDetail.Rows[0]["Sales_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtContactNo_Sales.Text = dtSponserDetail.Rows[0]["Sales_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                txtEmailId_Sales.Text = dtSponserDetail.Rows[0]["Sales_Email_Id"].ToString().Trim();

            }
            else
            {
                txtContactPerson_Accounts.Text = "";
                txtContactNo_Accounts.Text = "";
                txtEmailId_Accounts.Text = "";
                txtContactPerson_Sales.Text = "";
                txtContactNo_Sales.Text = "";
                txtEmailId_Sales.Text = "";
                try
                {
                    ddlContactNo_Accounts_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlContactNo_Sales_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();

                }
                catch
                {
                }

            }

            //here we get credit parameter value

            //code commented by neelkanth purohit - 03/09/2018 (bcz of new logic of customer account)
            //DataTable dtCreditParameter = objCustomerCreditParam.GetRecord_By_CustomerId(hdnCustomerId.Value);


            //dtCreditParameter = new DataView(dtCreditParameter, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();

            //if (dtCreditParameter.Rows.Count > 0)
            //{
            //    txtCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim());
            //    ddlCurrencyCreditLimit.SelectedValue = dtCreditParameter.Rows[0]["Credit_Limit_Currency"].ToString().Trim();
            //    txtCreditDays.Text = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
            //    rbtnAdvanceCheque.Checked = Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim());
            //    rbtnInvoicetoInvoice.Checked = Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim());
            //    rbtnAdvanceHalfpayment.Checked = Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim());

            //    if (!rbtnAdvanceCheque.Checked && !rbtnInvoicetoInvoice.Checked && !rbtnAdvanceHalfpayment.Checked)
            //    {
            //        rbtnNone.Checked = true;
            //    }

            //    if (dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim() != "")
            //    {
            //        Session["StatementPath"] = dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim();
            //        lnkDownloadFiancialstatement.Text = dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim();
            //    }
            //    else
            //    {
            //        Session["StatementPath"] = null;
            //        lnkDownloadFiancialstatement.Text = "Download";
            //    }

            //}
            //else
            //{
            //    txtCreditLimit.Text = "";
            //    txtCreditDays.Text = "";
            //    rbtnAdvanceCheque.Checked = false;
            //    rbtnInvoicetoInvoice.Checked = false;
            //    rbtnAdvanceHalfpayment.Checked = false;
            //    rbtnNone.Checked = true;
            //    Session["StatementPath"] = null;
            //    lnkDownloadFiancialstatement.Text = "Download";
            //    ddlCurrencyCreditLimit.SelectedValue = ddlCurrency.SelectedValue;
            //}

            //for fill block information

            chkBlockReason.Checked = Convert.ToBoolean(dtCustomerEdit.Rows[0]["Is_Block"].ToString());
            txtBlockReason.Text = dtCustomerEdit.Rows[0]["Block_Reason"].ToString();

            //for get authorized person detail


            DataTable dtAuthorizedPerson = objAuthorizedpersondetail.GetRecord_By_CustomerId(hdnCustomerId.Value);

            if (dtAuthorizedPerson.Rows.Count > 0)
            {
                int autPerId = 0;
                int.TryParse(dtAuthorizedPerson.Rows[0]["Name"].ToString().Trim(), out autPerId);
                string StrAutoPerName = string.Empty;
                if (autPerId > 0)
                {
                    StrAutoPerName = objContact.GetContactNameByContactiD(autPerId.ToString());
                    if (!string.IsNullOrEmpty(StrAutoPerName))
                    {
                        txtAuthorizedpersonName.Text = StrAutoPerName + "/" + autPerId.ToString();
                    }
                }
                else
                {
                    txtAuthorizedpersonName.Text = dtAuthorizedPerson.Rows[0]["Name"].ToString().Trim();
                }

                txtDesignationName.Text = dtAuthorizedPerson.Rows[0]["Designation_Name"].ToString().Trim() + "/" + dtAuthorizedPerson.Rows[0]["Designation_Id"].ToString().Trim();


                if (dtAuthorizedPerson.Rows[0]["Signature"].ToString().Trim() != "")
                {
                    Session["Signaturepath"] = dtAuthorizedPerson.Rows[0]["Signature"].ToString().Trim();
                    imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + Session["Signaturepath"].ToString();
                }
                else
                {
                    Session["Signaturepath"] = null;
                    imgLogo.ImageUrl = null;
                }
            }
            else
            {
                txtAuthorizedpersonName.Text = "";
                txtDesignationName.Text = "";
                Session["Signaturepath"] = null;
                imgLogo.ImageUrl = null;
            }

            //getr trade reference detail

            DataTable dtTradeReference = objCustomerTradeReference.GetRecord_By_CustomerId(hdnCustomerId.Value);


            if (dtTradeReference.Rows.Count > 0)
            {   //1
                int contactId = 0;
                txtSupplier1CompanyName.Text = string.Empty;
                int.TryParse(dtTradeReference.Rows[0]["Supplier1_Company_Name"].ToString().Trim(), out contactId);
                if (contactId > 0)
                {
                    txtSupplier1CompanyName.Text = objContact.GetContactNameByContactiD(contactId.ToString());
                    if (!string.IsNullOrEmpty(txtSupplier1CompanyName.Text))
                    {
                        txtSupplier1CompanyName.Text += "/" + contactId.ToString();
                    }
                }
                else
                {
                    txtSupplier1CompanyName.Text = dtTradeReference.Rows[0]["Supplier1_Company_Name"].ToString().Trim();
                }
                //txtSupplier1CompanyName.Text = dtTradeReference.Rows[0]["Supplier1_Company_Name"].ToString().Trim();
                //txtSupplier1ContactPerson.Text = dtTradeReference.Rows[0]["Supplier1_Contact_Person"].ToString().Trim();
                contactId = 0;
                txtSupplier1ContactPerson.Text = string.Empty;
                int.TryParse(dtTradeReference.Rows[0]["Supplier1_Contact_Person"].ToString().Trim(), out contactId);
                if (contactId > 0)
                {
                    txtSupplier1ContactPerson.Text = objContact.GetContactNameByContactiD(contactId.ToString());
                    if (!string.IsNullOrEmpty(txtSupplier1ContactPerson.Text))
                    {
                        txtSupplier1ContactPerson.Text += "/" + contactId.ToString();
                    }
                }
                else
                {
                    txtSupplier1ContactPerson.Text = dtTradeReference.Rows[0]["Supplier1_Contact_Person"].ToString().Trim();
                }

                ddlSupplier1ContactNo_CountryCode.SelectedValue = dtTradeReference.Rows[0]["Supplier1_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtSupplier1ContactNo.Text = dtTradeReference.Rows[0]["Supplier1_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                ddlSupplier1FaxNo_CountryCode.SelectedValue = dtTradeReference.Rows[0]["Supplier1_Fax_No"].ToString().Trim().Split('-')[0].ToString();
                txtSupplier1FaxNo.Text = dtTradeReference.Rows[0]["Supplier1_Fax_No"].ToString().Trim().Split('-')[1].ToString();
                txtSupplier1EmailId.Text = dtTradeReference.Rows[0]["Supplier1_Email_Id"].ToString().Trim();
                //2
                //txtSupplier2CompanyName.Text = dtTradeReference.Rows[0]["Supplier2_Company_Name"].ToString().Trim();
                contactId = 0;
                txtSupplier2CompanyName.Text = string.Empty;
                int.TryParse(dtTradeReference.Rows[0]["Supplier2_Company_Name"].ToString().Trim(), out contactId);
                if (contactId > 0)
                {
                    txtSupplier2CompanyName.Text = objContact.GetContactNameByContactiD(contactId.ToString());
                    if (!string.IsNullOrEmpty(txtSupplier2CompanyName.Text))
                    {
                        txtSupplier2CompanyName.Text += "/" + contactId.ToString();
                    }
                }
                else
                {
                    txtSupplier2CompanyName.Text = dtTradeReference.Rows[0]["Supplier2_Company_Name"].ToString().Trim();
                }

                //txtSupplier2ContactPerson.Text = dtTradeReference.Rows[0]["Supplier2_Contact_Person"].ToString().Trim();
                contactId = 0;
                txtSupplier2ContactPerson.Text = string.Empty;
                int.TryParse(dtTradeReference.Rows[0]["Supplier2_Contact_Person"].ToString().Trim(), out contactId);
                if (contactId > 0)
                {
                    txtSupplier2ContactPerson.Text = objContact.GetContactNameByContactiD(contactId.ToString());
                    if (!string.IsNullOrEmpty(txtSupplier2ContactPerson.Text))
                    {
                        txtSupplier2ContactPerson.Text += "/" + contactId.ToString();
                    }
                }
                else
                {
                    txtSupplier2ContactPerson.Text = dtTradeReference.Rows[0]["Supplier2_Contact_Person"].ToString().Trim();
                }

                ddlSupplier2ContactNo_CountryCode.SelectedValue = dtTradeReference.Rows[0]["Supplier2_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtSupplier2ContactNo.Text = dtTradeReference.Rows[0]["Supplier2_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                ddlSupplier2FaxNo_CountryCode.SelectedValue = dtTradeReference.Rows[0]["Supplier2_Fax_No"].ToString().Trim().Split('-')[0].ToString();
                txtSupplier2FaxNo.Text = dtTradeReference.Rows[0]["Supplier2_Fax_No"].ToString().Trim().Split('-')[1].ToString();
                txtSupplier2EmailId.Text = dtTradeReference.Rows[0]["Supplier2_Email_Id"].ToString().Trim();
                //3
                //txtSupplier3CompanyName.Text = dtTradeReference.Rows[0]["Supplier3_Company_Name"].ToString().Trim();
                contactId = 0;
                txtSupplier3CompanyName.Text = string.Empty;
                int.TryParse(dtTradeReference.Rows[0]["Supplier3_Company_Name"].ToString().Trim(), out contactId);
                if (contactId > 0)
                {
                    txtSupplier3CompanyName.Text = objContact.GetContactNameByContactiD(contactId.ToString());
                    if (!string.IsNullOrEmpty(txtSupplier3CompanyName.Text))
                    {
                        txtSupplier3CompanyName.Text += "/" + contactId.ToString();
                    }
                }
                else
                {
                    txtSupplier3CompanyName.Text = dtTradeReference.Rows[0]["Supplier3_Company_Name"].ToString().Trim();
                }


                //txtSupplier3ContactPerson.Text = dtTradeReference.Rows[0]["Supplier3_Contact_Person"].ToString().Trim();
                contactId = 0;
                txtSupplier3ContactPerson.Text = string.Empty;
                int.TryParse(dtTradeReference.Rows[0]["Supplier3_Contact_Person"].ToString().Trim(), out contactId);
                if (contactId > 0)
                {
                    txtSupplier3ContactPerson.Text = objContact.GetContactNameByContactiD(contactId.ToString());
                    if (!string.IsNullOrEmpty(txtSupplier3ContactPerson.Text))
                    {
                        txtSupplier3ContactPerson.Text += "/" + contactId.ToString();
                    }
                }
                else
                {
                    txtSupplier3ContactPerson.Text = dtTradeReference.Rows[0]["Supplier3_Contact_Person"].ToString().Trim();
                }

                ddlSupplier3ContactNo_CountryCode.SelectedValue = dtTradeReference.Rows[0]["Supplier3_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtSupplier3ContactNo.Text = dtTradeReference.Rows[0]["Supplier3_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                ddlSupplier3FaxNo_CountryCode.SelectedValue = dtTradeReference.Rows[0]["Supplier3_Fax_No"].ToString().Trim().Split('-')[0].ToString();
                txtSupplier3FaxNo.Text = dtTradeReference.Rows[0]["Supplier3_Fax_No"].ToString().Trim().Split('-')[1].ToString();
                txtSupplier3EmailId.Text = dtTradeReference.Rows[0]["Supplier3_Email_Id"].ToString().Trim();
            }
            else
            {

                txtSupplier1CompanyName.Text = "";
                txtSupplier1ContactPerson.Text = "";
                txtSupplier1ContactNo.Text = "";
                txtSupplier1FaxNo.Text = "";
                txtSupplier1EmailId.Text = "";
                //2
                txtSupplier2CompanyName.Text = "";
                txtSupplier2ContactPerson.Text = "";
                txtSupplier2ContactNo.Text = "";
                txtSupplier2FaxNo.Text = "";
                txtSupplier2EmailId.Text = "";
                //3
                txtSupplier3CompanyName.Text = "";
                txtSupplier3ContactPerson.Text = "";
                txtSupplier3ContactNo.Text = "";
                txtSupplier3FaxNo.Text = "";
                txtSupplier3EmailId.Text = "";
                try
                {
                    ddlSupplier1ContactNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlSupplier1FaxNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlSupplier2ContactNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlSupplier2FaxNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlSupplier3ContactNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlSupplier3FaxNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();

                }
                catch
                {
                }

            }


            //get bank reference detail 

            DataTable dtBankReference = objCustomerBankReference.GetRecord_By_CustomerId(hdnCustomerId.Value);

            if (dtBankReference.Rows.Count > 0)
            {
                //account 1

                txtAccountName1.Text = dtBankReference.Rows[0]["Account1_Account_Name"].ToString().Trim();
                txtAccountNo1.Text = dtBankReference.Rows[0]["Account1_Account_No"].ToString().Trim();
                txtAccountbankerName1.Text = dtBankReference.Rows[0]["Account1_Banker_Name"].ToString().Trim();
                txtAccountbankerAddress1.Text = dtBankReference.Rows[0]["Account1_Banker_Address"].ToString().Trim();
                ddlAccount1ContactNo_CountryCode.SelectedValue = dtBankReference.Rows[0]["Account1_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtAccountContactNo1.Text = dtBankReference.Rows[0]["Account1_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                ddlAccount1FaxNo_CountryCode.SelectedValue = dtBankReference.Rows[0]["Account1_Fax_No"].ToString().Trim().Split('-')[0].ToString();
                txtAccountFaxNo1.Text = dtBankReference.Rows[0]["Account1_Fax_No"].ToString().Trim().Split('-')[1].ToString();

                //account 2


                txtAccountName2.Text = dtBankReference.Rows[0]["Account2_Account_Name"].ToString().Trim();
                txtAccountNo2.Text = dtBankReference.Rows[0]["Account2_Account_No"].ToString().Trim();
                txtAccountbankerName2.Text = dtBankReference.Rows[0]["Account2_Banker_Name"].ToString().Trim();
                txtAccountbankerAddress2.Text = dtBankReference.Rows[0]["Account2_Banker_Address"].ToString().Trim();
                ddlAccount2ContactNo_CountryCode.SelectedValue = dtBankReference.Rows[0]["Account2_Contact_No"].ToString().Trim().Split('-')[0].ToString();
                txtAccountContactNo2.Text = dtBankReference.Rows[0]["Account2_Contact_No"].ToString().Trim().Split('-')[1].ToString();
                ddlAccount2FaxNo_CountryCode.SelectedValue = dtBankReference.Rows[0]["Account2_Fax_No"].ToString().Trim().Split('-')[0].ToString();
                txtAccountFaxNo2.Text = dtBankReference.Rows[0]["Account2_Fax_No"].ToString().Trim().Split('-')[1].ToString();
            }
            else
            {
                txtAccountName1.Text = "";
                txtAccountNo1.Text = "";
                txtAccountbankerName1.Text = "";
                txtAccountbankerAddress1.Text = "";
                txtAccountContactNo1.Text = "";
                txtAccountFaxNo1.Text = "";


                txtAccountName2.Text = "";
                txtAccountNo2.Text = "";
                txtAccountbankerName2.Text = "";
                txtAccountbankerAddress2.Text = "";
                txtAccountContactNo2.Text = "";
                txtAccountFaxNo2.Text = "";

                try
                {
                    ddlAccount1ContactNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlAccount1FaxNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlAccount2ContactNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                    ddlAccount2FaxNo_CountryCode.SelectedValue = "+" + objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(ddlCurrency.SelectedValue, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
                }
                catch
                {
                }

            }
            //Li_New.Style.Add("display", "");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            pnllblCustomerName.Text = txtCustomerName.Text;
        }

    }


    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        DataTable dtCurr = objCurrency.GetCurrencyMasterById(strCurrencyId);
        if (dtCurr.Rows.Count > 0)
        {
            strCurrencyName = dtCurr.Rows[0]["Currency_Name"].ToString();
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }





    protected void GvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomer.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtPaging"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCustomer, dt, "", "");
        string strCurrency = Session["LocCurrencyId"].ToString();
        //foreach (GridViewRow gvr in GvCustomer.Rows)
        //{
        //    Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

        //    lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
        //}
        //AllPageCode();
        //GvCustomer.BottomPagerRow.Focus();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedValue == "Phone_no")
        {
            FillGridForPhoneSearch();
        }
        else
        {
            FillGrid();
        }

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption.SelectedIndex == 1)
            {
                // condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                condition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";

            }
            else if (ddlOption.SelectedIndex == 2)
            {
                // condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                condition = ddlFieldName.SelectedValue + " Like '%" + txtValue.Text.Trim() + "%'";

            }
            else
            {
                //condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                condition = ddlFieldName.SelectedValue + " like '" + txtValue.Text.Trim() + "%'";

            }
            //DataTable dtCustomer = (DataTable)Session["dtCustomer"];

            //if (ddlFieldName.SelectedValue == "Customer_Code")
            //{
            //    condition = condition.Replace("Customer_Code", "Ems_Contact_Group.Field1");
            //}

            DataTable dtCustomer = (DataTable)Session["dtFilter_C_Master"];
            DataView view = new DataView(dtCustomer, condition, "", DataViewRowState.CurrentRows);
            dtCustomer = view.ToTable();
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCustomer, dtCustomer, "", "");
            string strCurrency = Session["LocCurrencyId"].ToString();
            //foreach (GridViewRow gvr in GvCustomer.Rows)
            //{
            //    Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

            //    lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
            //}
            Session["DtPaging"] = dtCustomer;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtCustomer.Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlGroupSearch.SelectedIndex == 0)
        {
            DisplayMessage("Select Customer Type");
            ddlGroupSearch.Focus();
            return;
        }

        else
        {
            string condition = string.Empty;
            condition = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();
            DataTable dtContact = objCustomer.GetCustomerAllTrueDataByGroup(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlGroupSearch.SelectedValue.ToString());

            if (ddlCreditStatus.SelectedIndex != 0)
            {

                try
                {
                    dtContact = new DataView(dtContact, "IsCredit='" + ddlCreditStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }

            if (dtContact.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)GvCustomer, dtContact, "", "");
            }
            else
            {
                GvCustomer.DataSource = null;
                GvCustomer.DataBind();
            }

            string strCurrency = Session["LocCurrencyId"].ToString();
            foreach (GridViewRow gvr in GvCustomer.Rows)
            {
                Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

                lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
            }

            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count.ToString();

            Session["dtContact"] = dtContact;
            Session["dtFilter_C_Master"] = dtContact;
        }

        //AllPageCode();
        btnResetSreach.Focus();
    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGroup();
        ddlGroupSearch.SelectedIndex = 0;
        ddlGroupSearch.Focus();
        FillGrid();
    }
    protected void GvCustomer_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["DtPaging"];
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
        Session["dtFilter_C_Master"] = dt;
        Session["DtPaging"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCustomer, dt, "", "");

        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gvr in GvCustomer.Rows)
        {
            Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

            lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
        }

        //AllPageCode();
        //GvCustomer.HeaderRow.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        //here we check that this customer is used or not if use than  we can't delete

        bool Check = objContact.IsUsedInInventory(StrCompId, StrBrandId, e.CommandArgument.ToString(), "1", "2");
        if (Check)
        {

            DisplayMessage("This Customer is in use,You Can not delete");
            return;

        }

        hdnCustomerId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objCustomer.DeleteCustomerMaster(StrCompId, StrBrandId, hdnCustomerId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin();
        //FillGrid();
        Reset();
        //try
        //{
        //    int i = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
        //    ((ImageButton)GvCustomer.Rows[i].FindControl("IbtnDelete")).Focus();
        //}
        //catch
        //{
        //    txtValue.Focus();
        //}
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        //FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void btnCustomerCancel_Click(object sender, EventArgs e)
    {
        Reset();

        btnList_Click(null, null);
        FillGridBin();
        //FillGrid();
        //pnlMenuNew.Visible = false;
        //Li_New.Style.Add("display", "none");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        btnCustomerCancel_Click(null, null);
    }



    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId);
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

    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();

        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/mastersetup"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Customer Credit for " + txtCustomerName.Text + ". on " + System.DateTime.Now.ToString();
        if (Hdn_Edit_ID.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
    }




    protected void btnCustomerSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string EmpPermission = string.Empty;
        DataTable dt1 = new DataTable();
        DataTable dtApproval = new DataTable();

        //this one is set bcz to skip related code bcz of new logic of account and credit - Neelkanth Puroit - 03/09/2018
        ddlIsCredit.SelectedValue = "False";

        if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
        {
            if (ddlIsCredit.SelectedValue == "True")
            {
                EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("Customer Credit").Rows[0]["Approval_Level"].ToString();


                dtApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),"18", Session["EmpId"].ToString());

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
                    if (float.Parse(txtCreditLimit.Text) > 1000)
                    {
                        if (Session["StatementPath"] == null)
                        {
                            DisplayMessage("your credit limit is greater than 1000 so financial statement should be required !");
                            return;
                        }
                    }
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
        }

        //FinancialYear Status







        string strTaxable = string.Empty;
        string strCustomerType = string.Empty;

        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Fill Customer Name");
            txtCustomerName.Focus();
            return;
        }

        string strFoundEmployee = string.Empty;
        if (txtFoundEmployee.Text != "")
        {
            if (GetEmployeeId(txtFoundEmployee.Text) == "")
            {
                strFoundEmployee = "0";
            }
            else
            {
                strFoundEmployee = GetEmployeeId(txtFoundEmployee.Text);
            }
        }
        else
        {
            strFoundEmployee = "0";
        }

        if (ddlCustomerType.SelectedItem.Value == "0")
        {
            strCustomerType = "0";
        }
        else if (ddlCustomerType.SelectedItem.Value == "1")
        {
            strCustomerType = "1";
        }
        else
        {
            strCustomerType = "2";
        }


        //Check Opening Balance Location Validations
        //if (strFYStatus == "True")
        //{
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

        string strHandledEmployee = string.Empty;
        if (txtHandledEmployee.Text != "")
        {
            if (GetEmployeeId(txtHandledEmployee.Text) == "")
            {
                strHandledEmployee = "0";
            }
            else
            {
                strHandledEmployee = GetEmployeeId(txtHandledEmployee.Text);
            }
        }
        else
        {
            strHandledEmployee = "0";
        }

        if (txtDebitAmount.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtDebitAmount.Text, out flTemp))
            {

            }
            else
            {
                txtDebitAmount.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDebitAmount);
                return;
            }
        }
        else
        {
            txtDebitAmount.Text = "0";
        }

        if (txtCreditAmount.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtCreditAmount.Text, out flTemp))
            {

            }
            else
            {
                txtCreditAmount.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCreditAmount);
                return;
            }
        }
        else
        {
            txtCreditAmount.Text = "0";
        }


        if (txtSalesQuota.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtSalesQuota.Text, out flTemp))
            {

            }
            else
            {
                txtSalesQuota.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesQuota);
                return;
            }
        }
        else
        {
            txtSalesQuota.Text = "0";
        }

        if (txtCreditLimit.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtCreditLimit.Text, out flTemp))
            {

            }
            else
            {
                txtCreditLimit.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCreditLimit);
                return;
            }
        }
        else
        {
            txtCreditLimit.Text = "0";
        }

        if (txtCreditDays.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtCreditDays.Text, out flTemp))
            {

            }
            else
            {
                txtCreditDays.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCreditDays);
                return;
            }
        }
        else
        {
            txtCreditDays.Text = "0";
        }

        if (txtPriceLevel.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtPriceLevel.Text, out flTemp))
            {

            }
            else
            {
                txtPriceLevel.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPriceLevel);
                return;
            }
        }
        else
        {
            txtPriceLevel.Text = "0";
        }

        if (chkIsTaxable.Checked == true)
        {
            strTaxable = "True";
        }
        else if (chkIsTaxable.Checked == false)
        {
            strTaxable = "False";
        }
        if (rbtnCustomerpricelist.Checked == false && rbtnSystemsalesprice.Checked == false)
        {
            rbtnSystemsalesprice.Checked = true;
        }
        string SalesPrice = string.Empty;
        if (rbtnSystemsalesprice.Checked == true)
        {
            SalesPrice = "1";

        }
        if (rbtnCustomerpricelist.Checked == true)
        {
            SalesPrice = "2";
        }



        DataTable dt = objContact.GetContactTrueById(hdnCustomerId.Value);
        string fn;

        string retval = GetAccountId();
        Boolean f6 = false;
        if (dt.Rows[0]["Field6"].ToString().Trim() == "")
        {
            f6 = false;
        }
        else
        {
            if (dt.Rows[0]["Field6"].ToString().Trim() == "True")
            {
                f6 = true;
            }
            else
            {
                f6 = false;
            }
        }

        //validateCompanyAddress
        string strCompanyAddressId = "0";
        if (txtCompanyAddress.Text != "")
        {
            using (DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtCompanyAddress.Text.Trim().Split('/')[0].ToString()))
            {
                if (dtAM.Rows.Count == 0)
                {
                    txtCompanyAddress.Text = "";
                    DisplayMessage("Choose company address from suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyAddress);
                    return;
                }
                else
                {
                    strCompanyAddressId = dtAM.Rows[0]["Trans_id"].ToString();
                }
            }
        }

        //For Accounts data
        string strCustomerAccount = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());


        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting("CustomerMaster", this.Page);
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
            if (hdnCustomerId.Value != "")
            {

                if (dt.Rows.Count != 0)
                {
                    b = objContact.UpdateContactMaster(hdnCustomerId.Value, dt.Rows[0]["Code"].ToString(), txtCustomerName.Text, txtLCustomerName.Text.Trim(), txtCivilId.Text, dt.Rows[0]["Dep_Id"].ToString(), dt.Rows[0]["Designation_Id"].ToString(), dt.Rows[0]["Religion_Id"].ToString(), dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["IsEmail"].ToString(), dt.Rows[0]["IsSMS"].ToString(), dt.Rows[0]["Status"].ToString(), dt.Rows[0]["Is_Reseller"].ToString(), dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), f6.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0", ref trns);
                    //b = objContact.UpdateContactMaster(hdnCustomerId.Value, dt.Rows[0]["Code"].ToString(), txtCustomerName.Text, txtLCusomerName.Text.Trim(), txtCivilId.Text, dt.Rows[0]["Dep_Id"].ToString(), dt.Rows[0]["Designation_Id"].ToString(), dt.Rows[0]["Religion_Id"].ToString(), dt.Rows[0]["Company_Id"].ToString(), false.ToString(), false.ToString(), dt.Rows[0]["Status"].ToString(), dt.Rows[0]["Field1"].ToString(), dt.Rows[0]["Field2"].ToString(), dt.Rows[0]["Field3"].ToString(), dt.Rows[0]["Field4"].ToString(), dt.Rows[0]["Field5"].ToString(), false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    int start_pos, last_pos;
                    string id;
                    if (txtMarketingEmp.Text.Trim() != "")
                    {
                        start_pos = txtMarketingEmp.Text.LastIndexOf("/") + 1;
                        last_pos = txtMarketingEmp.Text.Length;
                        id = txtMarketingEmp.Text.Substring(start_pos, last_pos - start_pos);
                    }
                    else
                    {
                        id = "0";
                    }

                    objCustomer.UpdateCustomerMaster(StrCompId, StrBrandId, hdnCustomerId.Value, retval, strCustomerType, strFoundEmployee, strHandledEmployee, txtDebitAmount.Text, txtCreditAmount.Text, txtODebitAmount.Text, txtOCreditAmount.Text, txtSalesQuota.Text, txtCreditLimit.Text, txtCreditDays.Text, strTaxable, txtPriceLevel.Text, chkBlockReason.Checked.ToString(), txtBlockReason.Text, SalesPrice, "", ddlCurrency.SelectedValue, ddlIsCredit.SelectedValue, ViewState["Status"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", id.ToString(), ratingControl.CurrentRating.ToString(), ref trns);


                    //-----------------Start - customer Opening  balance----------------------------------
                    bool isPreviousYearExist = false;
                    string yearCount = "0";
                    string sql = "select count(*) as year_count from Ac_Finance_Year_Info where company_id='" + Session["CompId"].ToString() + "' and isActive='true' and Status='Close'";
                    yearCount = da.get_SingleValue(sql);
                    isPreviousYearExist = yearCount != "0" ? true : false;
                    if (isPreviousYearExist == false)
                    {
                        DataTable dtSubCOAData = objSubCOA.GetSubCOAAllTrueByAccountId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strCustomerAccount, Session["FinanceYearId"].ToString(), ref trns);
                        if (dtSubCOAData.Rows.Count > 0)
                        {

                            dtSubCOAData = new DataView(dtSubCOAData, "Other_Account_No='" + hdnCustomerId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtSubCOAData.Rows.Count > 0)
                            {


                                foreach (GridViewRow gvr in GvLocation.Rows)
                                {


                                    HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                                    if (hdngvLocationId.Value != "0" && hdngvLocationId.Value != "")
                                    {
                                        DataTable dtSubByLocation = new DataView(dtSubCOAData, "Location_Id='" + hdngvLocationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtSubByLocation.Rows.Count > 0)
                                        {
                                            objSubCOA.DeleteSubCOADetailByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, dtSubByLocation.Rows[0]["Trans_Id"].ToString(), ref trns);
                                        }
                                    }
                                }
                            }
                        }



                        //ForSupplierOpeningBalanceAccordingToLocation.
                        foreach (GridViewRow gvr in GvLocation.Rows)

                        {
                            double DLocal = 0;
                            double CLocal = 0;

                            string strCompanyDebit = string.Empty;
                            string strCompanyCredit = string.Empty;
                            HiddenField hdngvLocationId = (HiddenField)gvr.FindControl("hdngvLocationId");
                            HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdngvCurrencyId");
                            TextBox txtgvDebit = (TextBox)gvr.FindControl("txtgvDebit");
                            TextBox txtgvCredit = (TextBox)gvr.FindControl("txtgvCredit");
                            TextBox txtgvFDebit = (TextBox)gvr.FindControl("txtgvForeignDebit");
                            TextBox txtgvFCredit = (TextBox)gvr.FindControl("txtgvForeignCredit");

                            if (txtgvDebit.Text != "")

                            {
                                DLocal = Convert.ToDouble(txtgvDebit.Text);

                            }
                            if (txtgvCredit.Text != "")

                            {
                                CLocal = Convert.ToDouble(txtgvCredit.Text);

                            }

                            if (DLocal != 0)
                            {
                                string strCompanyCrrValueDr = GetCurrencyForOpening(hdngvCurrencyId.Value, txtgvDebit.Text);
                                strCompanyDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            }
                            else
                            {
                                txtgvDebit.Text = "0.00";
                                strCompanyDebit = "0.00";
                            }
                            if (CLocal != 0)
                            {
                                string strCompanyCrrValueCr = GetCurrencyForOpening(hdngvCurrencyId.Value, txtgvCredit.Text);
                                strCompanyCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            }
                            else
                            {
                                txtgvCredit.Text = "0.00";
                                strCompanyCredit = "0.00";

                            }

                            if (txtgvFDebit.Text == "")

                            {
                                txtgvFDebit.Text = "0.00";

                            }

                            if (txtgvFCredit.Text == "")

                            {
                                txtgvFCredit.Text = "0.00";

                            }

                            objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), hdngvLocationId.Value, strCustomerAccount, hdnCustomerId.Value, txtgvDebit.Text, txtgvCredit.Text, txtgvFDebit.Text, txtgvFCredit.Text, ddlCurrency.SelectedValue, strCompanyDebit, strCompanyCredit, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                    }
                    //-----------------End - customer Opening  balance----------------------------------


                }

                objContactPriceList.DeleteContactPriceList(StrCompId, hdnCustomerId.Value, "C", ref trns);

                if (rbtnCustomerpricelist.Checked == true)
                {
                    if (Session["DtCustomerPriceList"] != null)
                    {
                        DataTable DtCustomerPriceList = (DataTable)Session["DtCustomerPriceList"];

                        for (int i = 0; i < DtCustomerPriceList.Rows.Count; i++)
                        {
                            objContactPriceList.InsertContact_PriceList(StrCompId, hdnCustomerId.Value, DtCustomerPriceList.Rows[i]["Product_Id"].ToString(), "C", DtCustomerPriceList.Rows[i]["Sales_Price"].ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                //Add Address Insert Section.
                objAddChild.DeleteAddressChild("Customer", hdnCustomerId.Value, ref trns);


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
                            objAddChild.InsertAddressChild(strAddressId, "Customer", hdnCustomerId.Value, chk.Checked.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }

                //here we code for insert the contact bank detail
                objContactBankDetail.DeleteContactBankDetail_By_ContactId_And_GroupId(hdnCustomerId.Value, "1", ref trns);
                if (Session["DtContactBank"] != null)
                {
                    DataTable dtContactBankDetail = (DataTable)Session["DtContactBank"];
                    for (int i = 0; i < dtContactBankDetail.Rows.Count; i++)
                    {
                        DataTable DtBank = objBankMaster.GetBankMasterByBankName(dtContactBankDetail.Rows[i]["Bank_Name"].ToString(), ref trns);

                        objContactBankDetail.InsertContact_BankDetail(hdnCustomerId.Value, DtBank.Rows[0]["Bank_Id"].ToString(), dtContactBankDetail.Rows[i]["Account_No"].ToString(), dtContactBankDetail.Rows[i]["Branch_Address"].ToString(), dtContactBankDetail.Rows[i]["IFSC_Code"].ToString(), dtContactBankDetail.Rows[i]["MICR_Code"].ToString(), dtContactBankDetail.Rows[i]["Branch_Code"].ToString(), "1", dtContactBankDetail.Rows[i]["IBAN_NUMBER"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }



                InsertRecord_In_CustomerSetup(hdnCustomerId.Value, dtApproval, EmpPermission, strCompanyAddressId, ref trns);



                //this code is added by jitendra upadhyay on 28-09-2016
                //this code is created for save product category for specfic supplier

                ObjContactProductcategory.DeleteProductCategory(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnCustomerId.Value, "C", ref trns);

                foreach (ListItem li in lstProductCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {

                            ObjContactProductcategory.InsertProductCategory(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnCustomerId.Value, li.Value.ToString(), "C", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                        catch
                        {

                        }
                    }
                }


                trns.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();

                //End  

                hdnCustomerId.Value = "";
                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                    Reset();
                    FillGrid();
                    btnList_Click(null, null);
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

    public string GetCurrencyForOpening(string strToCurrency, string strLocalAmount)
    {
        DataAccessClass Objda = new DataAccessClass(Session["DBConnection"].ToString());
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Currency_Id"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strToCurrency, strCurrency, Session["DBConnection"].ToString());
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

    public void InsertRecord_In_CustomerSetup(string strCustomerId, DataTable dt1, string EmpPermission, string strCompanyAddressId, ref SqlTransaction trns)
    {
        DataAccessClass Objda = new DataAccessClass(Session["DBConnection"].ToString());

        //first we insert customer company detail 
        string strCustomerCompanyId = string.Empty;

        if (txtCompanyName.Text == "")
        {
            strCustomerCompanyId = "0";
        }
        else
        {
            strCustomerCompanyId = txtCompanyName.Text.Split('/')[1].ToString();
        }

        //first we deleted old record and reinsert new record

        objCustomerCompanyInfo.DeleteRecord_By_CustomerId(strCustomerId, ref trns);


        objCustomerCompanyInfo.InsertRecord(strCustomerId, strCustomerCompanyId, strCompanyAddressId, ddlCompanyMobileNoCountryCode.SelectedValue + "-" + txtCompanyPermanentMobileNo.Text, ddlCompanyFaxNoCountryCode.SelectedValue + "-" + txtCompanyFaxNo.Text, txtCompanyEmailId.Text, txtCompanyWebsite.Text, ddlBusinessNature.SelectedValue, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


        //insert sponser detail

        string strAccountsContactPerson = string.Empty;
        string strSalesContactPerson = string.Empty;

        if (txtContactPerson_Sales.Text == "")
        {
            strSalesContactPerson = "0";
        }
        else
        {
            strSalesContactPerson = txtContactPerson_Sales.Text.Split('/')[1].ToString();
        }

        if (txtContactPerson_Accounts.Text == "")
        {
            strAccountsContactPerson = "0";
        }
        else
        {
            strAccountsContactPerson = txtContactPerson_Accounts.Text.Split('/')[1].ToString();
        }

        objSponserDetail.DeleteRecord_By_CustomerId(strCustomerId, ref trns);


        objSponserDetail.InsertRecord(strCustomerId, strAccountsContactPerson, ddlContactNo_Accounts_CountryCode.SelectedValue + "-" + txtContactNo_Accounts.Text, txtEmailId_Accounts.Text, strSalesContactPerson, ddlContactNo_Sales_CountryCode.SelectedValue + "-" + txtContactNo_Sales.Text, txtEmailId_Sales.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

        //for save all record in case of credit customer and parameter is true for currenct location 

        //insert credit parameter 
        if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "CreditInvoiceApproval").Rows[0]["ParameterValue"]) == true)
        {

            if (ddlIsCredit.SelectedValue == "True")
            {


                string Creditmethod = string.Empty;

                if (rbtnAdvanceCheque.Checked)
                {
                    Creditmethod = "Advance Cheque basis";
                }
                else if (rbtnInvoicetoInvoice.Checked)
                {
                    Creditmethod = "Invoice to invoice credit";
                }
                else if (rbtnAdvanceHalfpayment.Checked)
                {
                    Creditmethod = "50% advance and 50% on delivery";
                }
                else
                {
                    Creditmethod = "only credit limit and credit days basis";
                }


                string strLocalCreditLimit = string.Empty;


                strLocalCreditLimit = (Convert.ToDouble(txtCreditLimit.Text) * Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrencyCreditLimit.SelectedValue, Session["CurrencyId"].ToString(), ref trns))).ToString();


                DataTable dtParam = new DataTable();

                dtParam = objCustomerCreditParam.GetRecord_By_CustomerId(strCustomerId, ref trns);

                dtParam = new DataView(dtParam, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtParam.Rows.Count == 0)
                {
                    //update customer master flag as Pending

                    objCustomer.UpdateCustomerStatus(strCustomerId, "Pending", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);



                    if (dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                            string IsPriority = dt1.Rows[j]["Priority"].ToString();
                            int cur_trans_id = 0;
                            if (EmpPermission == "1")
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strCustomerId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "2")
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strCustomerId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else if (EmpPermission == "3")
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strCustomerId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }
                            else
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strCustomerId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                            }

                            Session["PriorityEmpId"] = PriorityEmpId;
                            Session["cur_trans_id"] = cur_trans_id;
                            Session["Ref_ID"] = strCustomerId.ToString();
                            Set_Notification();
                        }
                    }

                    if (Session["StatementPath"] == null)
                    {
                        Session["StatementPath"] = "";
                    }
                    //insert value in credit parameter table 

                    objCustomerCreditParam.InsertRecord(strCustomerId, txtCreditLimit.Text, ddlCurrencyCreditLimit.SelectedValue, txtCreditDays.Text, rbtnAdvanceCheque.Checked.ToString(), rbtnInvoicetoInvoice.Checked.ToString(), rbtnAdvanceHalfpayment.Checked.ToString(), Session["StatementPath"].ToString(), strLocalCreditLimit, "C", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                }
                else
                {

                    if (Session["StatementPath"] == null)
                    {
                        Session["StatementPath"] = "";
                    }


                    string OldCreditLimit = string.Empty;
                    string OldCreditDays = string.Empty;
                    string IsAdvanceChequeBasis = string.Empty;
                    string IsInvoicetoInvoice = string.Empty;
                    string IsAdvanceAmount = string.Empty;



                    OldCreditLimit = dtParam.Rows[0]["Credit_Limit"].ToString();
                    OldCreditDays = dtParam.Rows[0]["Credit_Days"].ToString();
                    IsAdvanceChequeBasis = dtParam.Rows[0]["Is_Adavance_Cheque_Basis"].ToString();
                    IsInvoicetoInvoice = dtParam.Rows[0]["Is_Invoice_To_Invoice"].ToString();
                    IsAdvanceAmount = dtParam.Rows[0]["Is_Half_Advance"].ToString();

                    //if parameter is chnaged then we again resend for approval in employee approval and update parameter and also set staus as pending in customer master table 

                    bool isCreditLimitupdated = false;

                    if (ViewState["Status"].ToString().Trim() == "Pending")
                    {
                        if (Convert.ToDouble(txtCreditLimit.Text) != Convert.ToDouble(OldCreditLimit) || Convert.ToDouble(txtCreditDays.Text) != Convert.ToDouble(OldCreditDays) || rbtnAdvanceCheque.Checked.ToString().ToUpper().Trim() != IsAdvanceChequeBasis.Trim().ToUpper() || rbtnInvoicetoInvoice.Checked.ToString().Trim().ToUpper() != IsInvoicetoInvoice.Trim().ToUpper() || rbtnAdvanceHalfpayment.Checked.ToString().Trim().ToUpper() != IsAdvanceAmount.Trim().ToUpper())
                        {
                            isCreditLimitupdated = true;

                            objCustomerCreditParam.DeleteRecord_By_CustomerId(strCustomerId, "C", ref trns);
                            objCustomerCreditParam.InsertRecord(strCustomerId, txtCreditLimit.Text, ddlCurrencyCreditLimit.SelectedValue, txtCreditDays.Text, rbtnAdvanceCheque.Checked.ToString(), rbtnInvoicetoInvoice.Checked.ToString(), rbtnAdvanceHalfpayment.Checked.ToString(), Session["StatementPath"].ToString(), strLocalCreditLimit, "C", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                    }
                    else
                    {

                        if (Convert.ToDouble(txtCreditLimit.Text) != Convert.ToDouble(OldCreditLimit) || Convert.ToDouble(txtCreditDays.Text) != Convert.ToDouble(OldCreditDays) || rbtnAdvanceCheque.Checked.ToString().ToUpper().Trim() != IsAdvanceChequeBasis.Trim().ToUpper() || rbtnInvoicetoInvoice.Checked.ToString().Trim().ToUpper() != IsInvoicetoInvoice.Trim().ToUpper() || rbtnAdvanceHalfpayment.Checked.ToString().Trim().ToUpper() != IsAdvanceAmount.Trim().ToUpper())
                        {
                            isCreditLimitupdated = true;
                            objCustomerCreditParam.DeleteRecord_By_CustomerId(strCustomerId, "C", ref trns);
                            objCustomerCreditParam.InsertRecord(strCustomerId, txtCreditLimit.Text, ddlCurrencyCreditLimit.SelectedValue, txtCreditDays.Text, rbtnAdvanceCheque.Checked.ToString(), rbtnInvoicetoInvoice.Checked.ToString(), rbtnAdvanceHalfpayment.Checked.ToString(), Session["StatementPath"].ToString(), strLocalCreditLimit, "C", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            //update pending status in customer table 
                            objCustomer.UpdateCustomerStatus(strCustomerId, "Pending", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }

                    if (isCreditLimitupdated)
                    {
                        DataTable dtEmp = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
                        dtEmp = new DataView(dtEmp, "Approval_Id='14' and Ref_Id='" + strCustomerId + "'", "", DataViewRowState.CurrentRows).ToTable();
                        for (int i = 0; i < dtEmp.Rows.Count; i++)
                        {
                            Objda.execute_Command("update Set_Approval_Transaction set isActive='False',ModifiedDate='" + DateTime.Now.ToString() + "'  where Trans_Id=" + dtEmp.Rows[i]["Trans_Id"].ToString() + "");

                            // objEmpApproval.UpdateApprovalTransaciton("SalesOrder", strCustomerId, "67", dtEmp.Rows[i]["Emp_Id"].ToString(), "Rejected", dtEmp.Rows[i]["Description"].ToString(), dtEmp.Rows[i]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns);
                        }

                        if (dt1.Rows.Count > 0)
                        {


                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                                string IsPriority = dt1.Rows[j]["Priority"].ToString();
                                int cur_trans_id = 0;
                                if (EmpPermission == "1")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), strCustomerId, PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (EmpPermission == "2")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), strCustomerId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (EmpPermission == "3")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strCustomerId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("14", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), strCustomerId.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", Creditmethod, txtCreditLimit.Text, txtCreditDays.Text, "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                Session["PriorityEmpId"] = PriorityEmpId;
                                Session["cur_trans_id"] = cur_trans_id;
                                Session["Ref_ID"] = strCustomerId.ToString();
                                Set_Notification();
                            }
                        }
                    }


                }
            }
        }

        //insert record for authorized signature

        objAuthorizedpersondetail.DeleteRecord_By_CustomerId(strCustomerId, ref trns);


        string strDesignationId = string.Empty;


        if (txtDesignationName.Text == "")
        {
            strDesignationId = "0";
        }
        else
        {
            strDesignationId = txtDesignationName.Text.Split('/')[1].ToString();
        }

        if (Session["Signaturepath"] == null)
        {
            Session["Signaturepath"] = "";
        }


        int AuthorizePerson = 0;
        if (!string.IsNullOrEmpty(txtAuthorizedpersonName.Text) && txtAuthorizedpersonName.Text.Contains("/"))
        {
            int.TryParse(txtAuthorizedpersonName.Text.Split('/')[1].ToString(), out AuthorizePerson);
        }


        objAuthorizedpersondetail.InsertRecord(strCustomerId, AuthorizePerson.ToString(), strDesignationId, Session["Signaturepath"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


        //insert record for customer trade reference

        objCustomerTradeReference.DeleteRecord_By_CustomerId(strCustomerId, ref trns);



        int supplier1, supplier1Contact, supplier2, supplier2Contact, supplier3, supplier3Contact = 0;
        supplier1 = supplier1Contact = supplier2 = supplier2Contact = supplier3 = supplier3Contact = 0;


        if (!string.IsNullOrEmpty(txtSupplier1CompanyName.Text) && txtSupplier1CompanyName.Text.Contains("/"))
        {
            int.TryParse(txtSupplier1CompanyName.Text.Split('/')[1].ToString(), out supplier1);
        }

        if (!string.IsNullOrEmpty(txtSupplier1ContactPerson.Text) && txtSupplier1ContactPerson.Text.Contains("/"))
        {
            int.TryParse(txtSupplier1ContactPerson.Text.Split('/')[1].ToString(), out supplier1Contact);
        }

        if (!string.IsNullOrEmpty(txtSupplier2CompanyName.Text) && txtSupplier2CompanyName.Text.Contains("/"))
        {
            int.TryParse(txtSupplier2CompanyName.Text.Split('/')[1].ToString(), out supplier2);
        }

        if (!string.IsNullOrEmpty(txtSupplier2ContactPerson.Text) && txtSupplier2ContactPerson.Text.Contains("/"))
        {
            int.TryParse(txtSupplier2ContactPerson.Text.Split('/')[1].ToString(), out supplier2Contact);
        }

        if (!string.IsNullOrEmpty(txtSupplier3CompanyName.Text) && txtSupplier3CompanyName.Text.Contains("/"))
        {
            int.TryParse(txtSupplier3CompanyName.Text.Split('/')[1].ToString(), out supplier3);
        }

        if (!string.IsNullOrEmpty(txtSupplier3ContactPerson.Text) && txtSupplier3ContactPerson.Text.Contains("/"))
        {
            int.TryParse(txtSupplier3ContactPerson.Text.Split('/')[1].ToString(), out supplier3Contact);
        }

        objCustomerTradeReference.InsertRecord(strCustomerId, supplier1.ToString(), supplier1Contact.ToString(), ddlSupplier1ContactNo_CountryCode.SelectedValue + "-" + txtSupplier1ContactNo.Text, ddlSupplier1FaxNo_CountryCode.SelectedValue + "-" + txtSupplier1FaxNo.Text, txtSupplier1EmailId.Text, supplier2.ToString(), supplier2Contact.ToString(), ddlSupplier2ContactNo_CountryCode.SelectedValue + "-" + txtSupplier2ContactNo.Text, ddlSupplier2FaxNo_CountryCode.SelectedValue + "-" + txtSupplier2FaxNo.Text, txtSupplier2EmailId.Text, supplier3.ToString(), supplier3Contact.ToString(), ddlSupplier3ContactNo_CountryCode.SelectedValue + "-" + txtSupplier3ContactNo.Text, ddlSupplier3FaxNo_CountryCode.SelectedValue + "-" + txtSupplier3FaxNo.Text, txtSupplier3EmailId.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


        //insert record in bank reference


        objCustomerBankReference.DeleteRecord_By_CustomerId(strCustomerId, ref trns);


        objCustomerBankReference.InsertRecord(strCustomerId, txtAccountName1.Text, txtAccountNo1.Text, txtAccountbankerName1.Text, txtAccountbankerAddress1.Text, ddlAccount1ContactNo_CountryCode.SelectedValue + "-" + txtAccountContactNo1.Text, ddlAccount1FaxNo_CountryCode.SelectedValue + "-" + txtAccountFaxNo1.Text, txtAccountName2.Text, txtAccountNo2.Text, txtAccountbankerName2.Text, txtAccountbankerAddress2.Text, ddlAccount2ContactNo_CountryCode.SelectedValue + "-" + txtAccountContactNo2.Text, ddlAccount2FaxNo_CountryCode.SelectedValue + "-" + txtAccountFaxNo2.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


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
    public string GetCurrency(string strToCurrency, string strLocalAmount, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, ref trns);
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
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        hdnCustomerId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objCustomer.DeleteCustomerMaster(StrCompId, StrBrandId, hdnCustomerId.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        //FillGrid();
        FillGridBin();
        Reset();
    }

    #region upload image
    //protected void btnloading_Click(object sender, EventArgs e)
    //{

    //    try
    //    {

    //        if (FileUploadImage.HasFile)
    //        {
    //            string path = Server.MapPath("~/CompanyResource/Customer");
    //            if (!Directory.Exists(path))
    //            {
    //                CheckDirectory(path);
    //            }
    //            imgCustomer.ImageUrl = null;

    //            FileUploadImage.SaveAs(Server.MapPath("~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName)));
    //            imgCustomer.ImageUrl = "~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName);
    //            ViewState["Image"] = "~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName);
    //        }
    //        else
    //        {
    //            DisplayMessage("First Upload File");
    //            return;
    //        }
    //    }
    //    catch
    //    {
    //        Exception ex = new Exception();
    //        DisplayMessage(ex.Message);
    //    }

    //}//end load function
    public void CheckDirectory(string path)
    {
        if (path != "")
        {
            Directory.CreateDirectory(path);
        }
    }

    #endregion

    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Focus();
    }

    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvCustomerBin.Rows)
        {
            index = (int)GvCustomerBin.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkSelect")).Checked;


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
            foreach (GridViewRow gvrow in GvCustomerBin.Rows)
            {
                int index = (int)GvCustomerBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void GvCustomerBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvCustomerBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCustomerBin"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCustomerBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();

        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gvr in GvCustomerBin.Rows)
        {
            Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

            lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
        }
    }
    protected void GvCustomerBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objCustomer.GetCustomerAllFalseData(StrCompId, StrBrandId);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCustomerBin, dt, "", "");
        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gvr in GvCustomerBin.Rows)
        {
            Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

            lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
        }
        lblSelectedRecord.Text = "";
        //AllPageCode();
        GvCustomerBin.HeaderRow.Focus();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objCustomer.GetCustomerAllFalseData(StrCompId, StrBrandId);
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvCustomerBin, dt, "", "");
        string strCurrency = Session["LocCurrencyId"].ToString();
        foreach (GridViewRow gvr in GvCustomerBin.Rows)
        {
            Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

            lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
        }
        Session["dtCustomerBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            ImgbtnSelectAll.Visible = false;
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


            DataTable dtCust = (DataTable)Session["dtCustomerBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCustomerBin, view.ToTable(), "", "");
            string strCurrency = Session["LocCurrencyId"].ToString();
            foreach (GridViewRow gvr in GvCustomerBin.Rows)
            {
                Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

                lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
            }
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                ImgbtnSelectAll.Visible = false;
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
        ArrayList userdetail = new ArrayList();
        if (GvCustomerBin.Rows.Count > 0)
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
                            b = objCustomer.DeleteCustomerMaster(StrCompId, StrBrandId, userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }
                    }
                }

                if (b != 0)
                {


                    FillGridBin();
                    //FillGrid();
                    lblSelectedRecord.Text = "";
                    DisplayMessage("Record Activate");
                    Session["CHECKED_ITEMS"] = null;

                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in GvCustomerBin.Rows)
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
            else
            {
                DisplayMessage("Please Select Record");
                GvCustomerBin.Focus();
                return;

            }

        }
    }

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvCustomerBin.HeaderRow.FindControl("chkCurrent"));
        foreach (GridViewRow gr in GvCustomerBin.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkSelect")).Checked = false;
            }
        }
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        Session["CHECKED_ITEMS"] = null;
        //FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueBin.Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        DataTable dtUnit = (DataTable)Session["dtCustomerBin"];
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

                if (!userdetails.Contains(dr["Customer_Id"]))
                {
                    userdetails.Add(dr["Customer_Id"]);
                }
            }
            foreach (GridViewRow gr in GvCustomerBin.Rows)
            {
                ((CheckBox)gr.FindControl("chkSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvCustomerBin, dtUnit1, "", "");
            string strCurrency = Session["LocCurrencyId"].ToString();
            foreach (GridViewRow gvr in GvCustomerBin.Rows)
            {
                Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

                lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
            }
            //AllPageCode();
            ViewState["Select"] = null;
            ImgbtnSelectAll.Focus();
        }
    }
    //old code

    #endregion

    #endregion

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objChartOfAcc = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = objChartOfAcc.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        dtCOA = new DataView(dtCOA, "AccountName Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();


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
            DataTable dtAccount = objCOf.GetCOAByTransId(StrCompId, strTransId);
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
            try
            {
                retval = (txtAccountNo.Text.Split('/'))[txtAccountNo.Text.Split('/').Length - 1];

                DataTable dtCOA = objCOf.GetCOAByTransId(StrCompId, retval);
                if (dtCOA.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            catch
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
        DataTable dtCOA = objCOf.GetCOAByTransId(StrCompId, AccountId);
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
    public void FillGrid()
    {
        DataTable dtCustomer = objCustomer.GetCustomerAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        //DataTable dtCustomer = objCustomer.GetCustomerAllTrueDataWithBalance(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (ddlGroupSearch.SelectedIndex > 0)
        {
            string condition = string.Empty;
            condition = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();
            dtCustomer = objCustomer.GetCustomerAllTrueDataByGroup(StrCompId, StrBrandId, ddlGroupSearch.SelectedValue.ToString());
        }

        if (ddlCreditStatus.SelectedIndex != 0)
        {
            dtCustomer = new DataView(dtCustomer, "IsCredit='" + ddlCreditStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }


        GvCustomer.DataSource = dtCustomer;
        GvCustomer.DataBind();
        Session["dtFilter_C_Master"] = dtCustomer;
        Session["DtPaging"] = dtCustomer;
        string strCurrency = Session["LocCurrencyId"].ToString();
        //foreach (GridViewRow gvr in GvCustomer.Rows)
        //{
        //    Label lblgvCreditLimit = (Label)gvr.FindControl("lblgvCreditLimit");

        //    lblgvCreditLimit.Text = ObjSysParam.GetCurencyConversionForInv(strCurrency, lblgvCreditLimit.Text);
        //}
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtCustomer.Rows.Count.ToString() + "";
        ////AllPageCode();
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

        DtGroup = new DataView(DtGroup, "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

        if (DtGroup.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlGroupSearch, DtGroup, "Group_Name", "Group_Id");
        }

    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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
        txtCustomerName.Text = "";
        txtLCustomerName.Text = "";
        Session["CHECKED_ITEMS"] = null;
        txtCivilId.Text = "";
        txtAddressName.Text = "";
        txtAccountNo.Text = "";
        ddlCustomerType.SelectedValue = "0";
        txtFoundEmployee.Text = "";
        txtHandledEmployee.Text = "";
        txtSalesQuota.Text = "";
        txtCreditLimit.Text = "";
        txtCreditDays.Text = "";
        txtPriceLevel.Text = "";
        chkIsTaxable.Checked = false;
        txtMarketingEmp.Text = "";
        ratingControl.CurrentRating = 0;
        GvAddressName.DataSource = null;
        GvAddressName.DataBind();

        hdnCustomerId.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        Session["DtContactBank"] = null;
        GvContactBankDetail.DataSource = null;
        GvContactBankDetail.DataBind();
        hdnContactBankId.Value = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtContactBankName.Text = "";
        rbtnCustomerpricelist.Checked = false;
        rbtnSystemsalesprice.Checked = false;
        BtnShowCpriceList.Visible = false;
        Session["DtCustomerPriceList"] = null;
        Session["dtArcaWing"] = null;

        ddlGroupSearch.SelectedIndex = 0;
        pnllblCustomerName.Text = "";

        ViewState["LogoUpload"] = null;
        ViewState["CustomerCode"] = null;
        ViewState["LogoUpload"] = null;
        ViewState["ImagePath"] = null;
        ViewState["Image"] = null;
        gvLogo.DataSource = null;
        gvLogo.DataBind();
        FillOpeningLocation();
    }
    #endregion


    #region Add AddressName Concept

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
            //Common Function add By Lokesh on 12-05-2015
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

    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

        //pnlAddress2.Visible = false;
    }



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
    }



    #endregion

    #region Auto Complete Method :- Funcation
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }

        return txt;
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

    #endregion

    protected void txtFoundEmployee_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtFoundEmployee.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtFoundEmployee.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmployee);
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtFoundEmployee.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFoundEmployee);
            }
        }
    }
    protected void txtHandledEmployee_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtHandledEmployee.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtHandledEmployee.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDebitAmount);
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtHandledEmployee.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmployee);
            }
        }
    }
    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(StrCompId, strEmployeeName.Split('/')[0].ToString());
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(StrCompId, retval);
                if (dtEmp.Rows.Count > 0)
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
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    protected string GetEmployeeName(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
            }
        }
        else
        {
            strEmployeeName = "";
        }
        return strEmployeeName;
    }

    #region CustomerDetail
    protected void BtnCancelView_Click(object sender, EventArgs e)
    {
        PanelView1.Visible = false;
        PanelView2.Visible = false;
        ViewReset();
    }

    protected void btnCloseView_Click(object sender, EventArgs e)
    {
        PanelView1.Visible = false;
        PanelView2.Visible = false;
        ViewReset();
    }
    void ViewReset()
    {
        btnList_Click(null, null);
        //DataListView.DataSource = null;
        //DataListView.DataBind();

        GvAddressNameView.DataSource = null;
        GvAddressNameView.DataBind();

        GvContactBankDetailView.DataSource = null;
        GvContactBankDetailView.DataBind();

        dlViewcontactlist.DataSource = null;
        dlViewcontactlist.DataBind();

        hdneditidView.Value = "";
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);
        //AllPageCode();
        Lbl_Tab_New.Text = Resources.Attendance.View;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        //Li_New.Style.Add("display", "");
    }
    protected void datalistCustomerDetailView_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        GridView gvAddressView = (GridView)e.Item.FindControl("GvAddressNameView");
        HiddenField hdnContactId = (HiddenField)e.Item.FindControl("hdnContactIdView");
        GridView GvContactBankDetail = (GridView)e.Item.FindControl("GvContactBankDetail");
        DataList dlViewcontactlist = (DataList)e.Item.FindControl("dlViewcontactlist");
        Label lblCSalesPrice = (Label)e.Item.FindControl("lblCSalesPrice");
        string Id = hdnContactId.Value;
        DataTable dtChild = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Customer", Id);
        if (dtChild.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
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
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, dtcontactBank, "", "");
        }
        DataTable dt = objContact.GetContactTrueAllData(Id, "Individual");
        if (dt.Rows.Count != 0)
        {
            //Common Function add By Lokesh on 12-05-2015
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
        DataTable dt = new DataView(objBankMaster.GetBankMaster(), "Bank_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
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

        DataTable dt = new DataTable();
        if (hdnContactBankId.Value == "")
        {



            if (Session["DtContactBank"] == null)
            {

                dt.Columns.Add("Trans_Id");


                dt.Columns.Add("Bank_Name");
                dt.Columns.Add("Account_No");
                dt.Columns.Add("Branch_Address");
                dt.Columns.Add("IFSC_Code");
                dt.Columns.Add("MICR_Code");
                dt.Columns.Add("Branch_Code");
                dt.Columns.Add("IBAN_NUMBER");
                DataRow dr = dt.NewRow();
                dr["Trans_Id"] = "1";

                dr["Bank_Name"] = txtBankName.Text;
                dr["Account_No"] = txtCBAccountNo.Text;
                dr["Branch_Address"] = txtCBBrachAddress.Text;
                dr["IFSC_Code"] = txtCBIFSCCode.Text;
                dr["MICR_Code"] = txtCBMICRCode.Text;
                dr["Branch_Code"] = txtCBBranchCode.Text;
                dr["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = (DataTable)Session["DtContactBank"];
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
                dr["IBAN_NUMBER"] = txtIBANNUMBER.Text;
                dt.Rows.Add(dr);

            }

            Session["DtContactBank"] = dt;
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, dt, "", "");
            DisplayMessage("Record Saved", "green");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Bank_Close()", true);
        }
        else
        {
            dt = (DataTable)Session["DtContactBank"];
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
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvContactBankDetail, dt, "", "");
            Session["DtContactBank"] = dt;
            ResetContactBankPanel();

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
        DataTable dt = (DataTable)Session["DtContactBank"];
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
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Add_Bank()", true);
        //PnlContactBankdetail2.Visible = true;
        //PnlContactBankdetail3.Visible = true;
        txtCBAccountNo.Focus();

    }
    protected void btnContactBankDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["DtContactBank"];
        try
        {
            dt = new DataView(dt, "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        Session["DtContactBank"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)GvContactBankDetail, dt, "", "");
    }
    protected void BtnCBCancel_Click(object sender, EventArgs e)
    {
        ResetContactBankPanel();
        txtBankName.Focus();

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

    }


    #endregion
    #region Add_CustomerPriceList
    protected string GetUnitName(string strUnitId)
    {
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(StrCompId, strUnitId);
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
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Product_Close()", true);
        //PnlCustomerPricelist2.Visible = false;
        //PnlCustomerPricelist3.Visible = false;
    }
    protected void BtnCustomerPriceListCancel_Click(object sender, EventArgs e)
    {

        //PnlCustomerPricelist2.Visible = false;
        //PnlCustomerPricelist3.Visible = false;
    }
    protected void BtnShowCpriceList_click(object sender, EventArgs e)
    {
        ddlCpriceFieldoption.SelectedIndex = 2;
        ddlCpriceField.SelectedIndex = 2;

        txtCpriceSearch.Text = "";
        txtCpriceSearch.Focus();

        DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(StrCompId, Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        GvProduct.DataSource = dtProductMaster;
        GvProduct.DataBind();
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
        pnllblCustomerName.Text = txtCustomerName.Text;

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Show_Product()", true);
        //PnlCustomerPricelist2.Visible = true;
        //PnlCustomerPricelist3.Visible = true;

    }
    protected void ImgButtonCPClose_Click(object sender, ImageClickEventArgs e)
    {

        //PnlCustomerPricelist2.Visible = false;
        //PnlCustomerPricelist3.Visible = false;

    }
    protected void rbtnSystemsalesprice_CheckedChanged(object sender, EventArgs e)
    {
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        BtnShowCpriceList.Visible = false;
    }
    protected void rbtnCustomerpricelist_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(StrCompId, Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        Session["DtProduct"] = dtProductMaster;
        GvProduct.DataSource = dtProductMaster;
        GvProduct.DataBind();

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

        BtnShowCpriceList.Visible = true;
        pnllblCustomerName.Text = txtCustomerName.Text;

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
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)GvProduct, view.ToTable(), "", "");

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
        ddlCpriceFieldoption.SelectedIndex = 2;
        ddlCpriceField.SelectedIndex = 2;

        txtCpriceSearch.Text = "";
        txtCpriceSearch.Focus();

        DataTable dtProductMaster = objProductM.GetProductMasterTrueAll(StrCompId, Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        Session["DtProduct"] = dtProductMaster;
        //Common Function add By Lokesh on 12-05-2015
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

        string path = Server.MapPath("~/CompanyResource/Customer");
        if (!Directory.Exists(path))
        {
            CheckDirectory(path);
        }


        dtLogo = FillLogoTable();

        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvLogo, (DataTable)ViewState["LogoUpload"], "", "");
        //AllPageCode();
    }
    protected void IbtnDeleteLogo_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)ViewState["LogoUpload"]), "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 12-05-2015
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
                    FileUploadImage.SaveAs(Server.MapPath("~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + "_" + dt.Rows[i]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName)));
                    ViewState["Image"] = "~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + "_" + dt.Rows[i]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName);
                    dt.Rows[i]["ImagePath"] = ViewState["Image"];
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Trans_Id"] = "1";

            FileUploadImage.SaveAs(Server.MapPath("~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + "_" + dt.Rows[0]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName)));

            ViewState["Image"] = "~/CompanyResource/Customer/" + ViewState["CustomerCode"].ToString() + "_" + dt.Rows[0]["Trans_Id"] + System.IO.Path.GetExtension(FileUploadImage.PostedFile.FileName);

            dt.Rows[0]["ImagePath"] = ViewState["Image"];

        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 12-05-2015
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
    #region CustomerSetup and customer approval system
    protected void txtCompanyName_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtCompanyName.Text != "")
        {
            try
            {
                strCustomerId = txtCompanyName.Text.Split('/')[1].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            if (strCustomerId != "" && strCustomerId != "0")
            {
                Session["ContactID"] = strCustomerId;



                string[] strCustomerInfo = getCustomerInformation(strCustomerId);

                txtCompanyEmailId.Text = strCustomerInfo[0].ToString();
                txtCompanyPermanentMobileNo.Text = strCustomerInfo[1].ToString();
                txtCompanyAddress.Text = strCustomerInfo[2].ToString(); ;
                txtCompanyFaxNo.Text = strCustomerInfo[3].ToString();
                txtCompanyWebsite.Text = strCustomerInfo[4].ToString();


            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtCompanyName.Text = "";

                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
            }
        }
        else
        {
            txtCompanyName.Text = "";

            txtCustomerName.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
        }
        if (sender != null)
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Company_Detail_Div()", true);
    }

    protected void txtContactPerson_Accounts_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactPerson_Accounts.Text != "")
        {
            try
            {
                strCustomerId = txtContactPerson_Accounts.Text.Split('/')[1].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            if (strCustomerId != "" && strCustomerId != "0")
            {

                string[] strCustomerInfo = getCustomerInformation(strCustomerId);

                txtEmailId_Accounts.Text = strCustomerInfo[0].ToString();
                txtContactNo_Accounts.Text = strCustomerInfo[1].ToString();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtContactPerson_Accounts.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactPerson_Accounts);
            }
        }
        else
        {
            txtContactPerson_Accounts.Text = "";

            txtContactPerson_Accounts.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactPerson_Accounts);
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Company_Owner_or_Sponser_detail_Div()", true);
    }

    protected void txtContactPerson_Sales_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactPerson_Sales.Text != "")
        {
            try
            {
                strCustomerId = txtContactPerson_Sales.Text.Split('/')[1].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            if (strCustomerId != "" && strCustomerId != "0")
            {

                string[] strCustomerInfo = getCustomerInformation(strCustomerId);

                txtEmailId_Sales.Text = strCustomerInfo[0].ToString();
                txtContactNo_Sales.Text = strCustomerInfo[1].ToString();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtContactPerson_Sales.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactPerson_Sales);
            }
        }
        else
        {
            txtContactPerson_Sales.Text = "";

            txtContactPerson_Sales.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactPerson_Sales);
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Company_Owner_or_Sponser_detail_Div()", true);
    }

    public void FillCustomerCountryCode(string strCurrencyId)
    {

        FillCountryCode(ddlCompanyMobileNoCountryCode, strCurrencyId);
        FillCountryCode(ddlCompanyFaxNoCountryCode, strCurrencyId);
        FillCountryCode(ddlContactNo_Accounts_CountryCode, strCurrencyId);
        FillCountryCode(ddlContactNo_Sales_CountryCode, strCurrencyId);
        FillCountryCode(ddlSupplier1ContactNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlSupplier2ContactNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlSupplier3ContactNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlSupplier1FaxNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlSupplier2FaxNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlSupplier3FaxNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlAccount1ContactNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlAccount2ContactNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlAccount1FaxNo_CountryCode, strCurrencyId);
        FillCountryCode(ddlAccount2FaxNo_CountryCode, strCurrencyId);
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



    #region UploadFinancialStatement

    protected void btnuploadFiancialstatement_Click(object sender, EventArgs e)
    {
        //imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPath.FileName + txtEmployeeCode.Text;
        lnkDownloadFiancialstatement.Text = FileUploadFinancilaStatement.FileName;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Credit_Approval_Div()", true);
    }

    protected void btnDownloadFiancialstatement_Click(object sender, EventArgs e)
    {

        if (Session["StatementPath"] == null)
        {
            DisplayMessage("Statement not found");
            return;
        }
        try
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../CompanyResource/" + Session["CompId"].ToString() + "/" + Session["StatementPath"].ToString() + "')", true);
        }
        catch
        {

        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Credit_Approval_Div()", true);
    }

    #endregion


    #region UploadSignature

    protected void btnUpload1_Click(object sender, EventArgs e)
    {
        //imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPath.FileName + txtEmployeeCode.Text;
        imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPath.FileName;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Personal_Authorized_Signature_For_purchase_Order_Div()", true);
    }

    protected void btndownlodSignature_Click(object sender, EventArgs e)
    {
        if (Session["Signaturepath"] == null)
        {
            DisplayMessage("Signature Not Found");
            return;
        }
        try
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../CompanyResource/" + Session["CompId"].ToString() + "/" + Session["Signaturepath"].ToString() + "')", true);
        }
        catch
        {
            DisplayMessage("Signature Not Found");
            return;
        }

        //imgLogo.ImageUrl = "~/CompanyResource/" + "/" + Session["CompId"] + "/" + FULogoPath.FileName + txtEmployeeCode.Text;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Personal_Authorized_Signature_For_purchase_Order_Div()", true);
    }
    #endregion


    protected void txtDesignationName_TextChanged(object sender, EventArgs e)
    {
        if (txtDesignationName.Text != "")
        {

            string strDesignationId = string.Empty;

            try
            {
                strDesignationId = txtDesignationName.Text.Split('/')[1].ToString();

            }
            catch
            {
                strDesignationId = "0";
            }


            if (strDesignationId != "0")
            {

                DataTable dt = objDesg.GetDesignationMasterById(strDesignationId);
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Select dsignation Name in suggestion only");
                    txtDesignationName.Text = "";
                    txtDesignationName.Focus();
                    //  return;
                }
            }
            else
            {
                DisplayMessage("Select designation Name in suggestion only");
                txtDesignationName.Focus();
                txtDesignationName.Text = "";
                //  return;

            }
        }
        else
        {
            DisplayMessage("Enter Designation Name");
            txtDesignationName.Focus();
            //return;

        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Personal_Authorized_Signature_For_purchase_Order_Div()", true);
    }


    public string[] getCustomerInformation(string Contactid)
    {
        //code start
        string[] strCusInfo = new string[5];



        strCusInfo[0] = "";
        strCusInfo[1] = "";
        string Address = string.Empty;

        DataTable dtContact = objContact.GetContactTrueById(Contactid);
        if (dtContact.Rows.Count > 0)
        {
            strCusInfo[0] = dtContact.Rows[0]["Field1"].ToString();


            //for contact No
            try
            {
                strCusInfo[1] = dtContact.Rows[0]["Field2"].ToString().Split('-')[1].ToString();
            }
            catch
            {
                strCusInfo[1] = dtContact.Rows[0]["Field2"].ToString();
            }


        }
        //for get address
        DataTable dt = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Contact", Contactid);
        try
        {
            dt = new DataView(dt, "Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {

            Address = dt.Rows[0]["FullAddress"].ToString();
            if (dt.Rows[0]["FullAddress"].ToString() != "")
            {
                Address = dt.Rows[0]["FullAddress"].ToString();
            }
            if (dt.Rows[0]["Street"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Street"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Street"].ToString();
                }
            }
            if (dt.Rows[0]["Block"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Block"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Block"].ToString();
                }

            }
            if (dt.Rows[0]["Avenue"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Avenue"].ToString();
                }
            }

            if (dt.Rows[0]["CityId"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["CityId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["CityId"].ToString();
                }

            }
            if (dt.Rows[0]["StateId"].ToString() != "")
            {


                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["StateId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["StateId"].ToString();
                }

            }
            if (dt.Rows[0]["CountryId"].ToString() != "")
            {
                CountryMaster objCountry = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());


                if (Address != "")
                {
                    Address = Address + "," + objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }
                else
                {
                    Address = objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }

            }
            if (dt.Rows[0]["PinCode"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["PinCode"].ToString();
                }

            }
            strCusInfo[2] = Address;

            //fax no
            try
            {
                strCusInfo[3] = dt.Rows[0]["FaxNo"].ToString().Split('-')[1].ToString();
            }
            catch
            {
                strCusInfo[3] = dt.Rows[0]["FaxNo"].ToString();
            }

            //website
            strCusInfo[4] = dt.Rows[0]["WebSite"].ToString();


        }
        else
        {
            strCusInfo[2] = "";
            strCusInfo[3] = "";
            strCusInfo[4] = "";

        }

        return strCusInfo;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();
        try
        {
            dtCustomer = new DataView(dtCustomer, "Field6='True' and (Status='Company' or Is_Reseller='True')", "", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {
            dtCustomer = ObjContactMaster.GetContactTrueAllData();
        }
        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Filtertext like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ems = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = "0";
        if (HttpContext.Current.Session["cm_customerId"] == null)
        {
            id = "0";
        }
        else
        {
            id = HttpContext.Current.Session["cm_customerId"].ToString();
        }

        DataTable dtCon = ems.GetAllContactAsPerFilterText(prefixText, id);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesg(string prefixText, int count, string contextKey)
    {
        DesignationMaster objquali = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objquali.GetDesignationMaster(), "Designation like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        return txt;
    }
    #endregion
    #region CustomerStatement


    private string GetLocationId()
    {
        string retval = string.Empty;
        ////if (txtToLocation.Text != "")
        ////{
        //    DataTable dtLocation = ObjLocation.GetLocationMasterByLocationName(StrCompId.ToString(), txtToLocation.Text.Trim().Split('/')[0].ToString());
        //    if (dtLocation.Rows.Count > 0)
        //    {
        //        //retval = (txtToLocation.Text.Split('/'))[txtToLocation.Text.Split('/').Length - 1];
        //    }
        //    else
        //    {
        //        retval = "";
        //    }
        ////}
        ////else
        ////{
        ////    retval = "";
        ////}
        return retval;
    }

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

    #region Auto Complete Method


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocation(string prefixText, int count, string contextKey)
    {
        LocationMaster objLoc = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Location_Name Like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objLoc.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Location_Name"].ToString() + "/" + dt.Rows[i]["Location_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    #endregion





    #region ViewSection

    protected string GetCustomerNameByContactId(string strContactId)
    {
        string strCustomerName = string.Empty;
        if (strContactId != "0" && strContactId != "")
        {
            DataTable dtAccName = objContact.GetContactTrueById(strContactId);
            if (dtAccName.Rows.Count > 0)
            {
                strCustomerName = dtAccName.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strCustomerName = "";
        }
        return strCustomerName;
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            DataTable dtAccName = objCOA.GetCOAByTransId(StrCompId, strAccountNo);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString();
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
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

    protected void btnCancelPopLeave_Click(object sender, EventArgs e)
    {
        //pnl1.Visible = false;
        //pnl2.Visible = false;
        //Reset();
    }
    #endregion

    #endregion
    #region CreditApplication
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/CustomerCreditApplication.aspx?Id=" + e.CommandArgument.ToString() + "')", true);

    }
    #endregion
    #region ProductCategory
    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(StrCompId.ToString());
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
                Session["Signaturepath"] = FULogoPath.FileName;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Personal_Authorized_Signature_For_purchase_Order_Div()", true);
            }
        }
    }

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FileUploadFinancilaStatement.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/") + Session["CompId"]))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/") + Session["CompId"]);
            }
            string path = Server.MapPath("~/CompanyResource/" + "/" + Session["CompId"] + "/") + FileUploadFinancilaStatement.FileName;
            FileUploadFinancilaStatement.SaveAs(path);
            Session["StatementPath"] = FileUploadFinancilaStatement.FileName;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Credit_Approval_Div()", true);
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

        addaddress.fillHeader(txtCustomerName.Text);
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
        addaddress.fillHeader(txtCustomerName.Text);
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




    protected void lBtnMoreNum_List_Command(object sender, CommandEventArgs e)
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

    public void FillGridForPhoneSearch()
    {
        DataTable dtCustomer = objCustomer.GetCustomerAllTrueDataForMobSearch(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (ddlGroupSearch.SelectedIndex > 0)
        {
            string condition = string.Empty;
            condition = "Group_Id = " + ddlGroupSearch.SelectedValue.ToString();
            dtCustomer = objCustomer.GetCustomerAllTrueDataByGroup(StrCompId, StrBrandId, ddlGroupSearch.SelectedValue.ToString());
        }

        if (ddlCreditStatus.SelectedIndex != 0)
        {
            dtCustomer = new DataView(dtCustomer, "IsCredit='" + ddlCreditStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        Session["dtFilter_C_Master"] = dtCustomer;
        Session["DtPaging"] = dtCustomer;

    }
    public string getAmountInLoginCurrency(string strAmount)
    {
        string _result = string.Empty;
        _result = SystemParameter.GetAmountWithDecimal(strAmount, HttpContext.Current.Session["LoginLocDecimalCount"].ToString());
        return _result;
    }


    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;
        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId, HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }



    protected void lnkCustomerBalance_Command(object sender, CommandEventArgs e)
    {
        double dblBalance;
        double.TryParse(lnkCustomerBalance.Text, out dblBalance);
        if (dblBalance == 0)
        {
            return;
        }
        if (IsObjectPermission("162", "308"))
        {
            string strCustomerId = hdnCustomerId.Value;
            Session["CusID"] = strCustomerId;
            Session["CusterStatementFromDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["CustomerStatementToDate"] = DateTime.Now.ToString("dd-MMM-yyyy");
            Session["CustomerStatementLocations"] = Session["LocId"].ToString();
            string strCmd = string.Format("window.open('../CustomerReceivable/CustomerStatement.aspx?Id=" + strCustomerId + "','window','width=1024, ');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        }
        else
        {
            DisplayMessage("You do not have permission to view statement");
            return;
        }
    }

    protected void lBtnCustomerInfo_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];
        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), CustomerId);

        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(CustomerId);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
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

    protected void lnkcustomerHistory_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + e.CommandArgument.ToString() + "&&Page=SINQ','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/Customer", "MasterSetup", "Customer", e.CommandName.ToString(), ((LinkButton)gvrow.FindControl("lBtnCustomerInfo")).Text + "(" + e.CommandName.ToString() + ")");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void ratingControl_Changed(object sender, AjaxControlToolkit.RatingEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "On_Load_Modal()", true);
    }

    protected void lnkAccountMaster_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];
        //GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
        UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", arguments[1]);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "$('#ModelAcMaster').modal('toggle');" ,true);
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
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting("CustomerMaster");
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvCustomer, lstCls);
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings("CustomerMaster", GvCustomer, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting("CustomerMaster");
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting("CustomerMaster", lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void txtAuthorizedpersonName_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtAuthorizedpersonName.Text != "")
        {
            try
            {
                strCustomerId = txtAuthorizedpersonName.Text.Split('/')[1].ToString();
            }
            catch
            {
                strCustomerId = "0";
            }
            if (strCustomerId != "" && strCustomerId != "0")
            {
                string[] strCustomerInfo = getCustomerInformation(strCustomerId);

                //txtEmailId_Accounts.Text = strCustomerInfo[0].ToString();
                //txtContactNo_Accounts.Text = strCustomerInfo[1].ToString();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAuthorizedpersonName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAuthorizedpersonName);
            }
        }
        else
        {
            txtAuthorizedpersonName.Text = "";

            txtAuthorizedpersonName.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAuthorizedpersonName);
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Personal_Authorized_Signature_For_purchase_Order_Div()", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtSupplier = ObjSupplier.GetSupplierAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText))
        {
            string[] filterlist = new string[dtSupplier.Rows.Count];
            if (dtSupplier.Rows.Count > 0)
            {
                for (int i = 0; i < dtSupplier.Rows.Count; i++)
                {
                    filterlist[i] = dtSupplier.Rows[i]["Filtertext"].ToString();
                }
            }
            return filterlist;
        }

    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Contacts(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Filtertext"].ToString();
            }
            dt = null;
        }
        return filterlist;
    }

    protected void txtCompanyAddress_TextChanged(object sender, EventArgs e)
    {
        if (txtCompanyAddress.Text != "")
        {
            using (DataTable dtAM = objAddMaster.GetAddressDataByAddressName(txtCompanyAddress.Text.Trim().Split('/')[0].ToString()))
            {
                if (dtAM.Rows.Count == 0)
                {
                    txtCompanyAddress.Text = "";
                    DisplayMessage("Choose In Suggestions Only");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyAddress);
                    return;
                }
            }
        }
    }

    protected void rbtnupdall_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnUploadOb.Checked == true)
        {
            if (Session["ExcelCustomerObList"] != null)
            {
                List<clsObExcelImport> newLst = (List<clsObExcelImport>)Session["ExcelCustomerObList"];
                gvImport.DataSource = newLst;
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Count.ToString();
            }
        }
        else
        {
            if (Session["ExcelCustomerAgeingList"] != null)
            {
                List<clsAgeingExcelImport> newLst = (List<clsAgeingExcelImport>)Session["ExcelCustomerAgeingList"];
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
            if (Session["ExcelCustomerObList"] != null)
            {
                List<clsObExcelImport> newLst = (List<clsObExcelImport>)Session["ExcelCustomerObList"];
                gvImport.DataSource = newLst.Where(m => m.is_valid == true).ToList();
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Where(m => m.is_valid == true).ToList().Count();
            }
        }
        else
        {
            if (Session["ExcelCustomerAgeingList"] != null)
            {
                List<clsAgeingExcelImport> newLst = (List<clsAgeingExcelImport>)Session["ExcelCustomerAgeingList"];
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
            if (Session["ExcelCustomerObList"] != null)
            {
                List<clsObExcelImport> newLst = (List<clsObExcelImport>)Session["ExcelCustomerObList"];
                gvImport.DataSource = newLst.Where(m => m.is_valid == false).ToList();
                gvImport.DataBind();
                lbltotaluploadRecord.Text = "Total Records:" + newLst.Where(m => m.is_valid == false).ToList().Count();
            }
        }
        else
        {
            if (Session["ExcelCustomerAgeingList"] != null)
            {
                List<clsAgeingExcelImport> newLst = (List<clsAgeingExcelImport>)Session["ExcelCustomerAgeingList"];
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
        Session["ExcelCustomerAgeingList"] = null;
        Session["ExcelCustomerObList"] = null;
    }

    protected void lnkDownloadAgeingData_Click(object sender, EventArgs e)
    {

    }

    public class clsBaseImport
    {
        public string customer_id { get; set; }
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
        public string customer_name { get; set; }
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
            hdnTotalExcelRecords.Value = "0";
            hdnInvalidExcelRecords.Value = "0";
            hdnValidExcelRecords.Value = "0";

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
                            _clsObj.customer_name = dr["customer_name"].ToString().Trim();
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
                            _clsObj.exchange_rate = exchange_rate == 0 ? 1 : exchange_rate;
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
                                Session["ExcelCustomerObList"] = newObList;
                            }
                            else
                            {
                                Session["ExcelCustomerObList"] = null;
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
                            _clsObj.customer_name = dr["customer_name"].ToString().Trim();
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
                                Session["ExcelCustomerAgeingList"] = newAgeingList;
                            }
                            else
                            {
                                Session["ExcelCustomerAgeingList"] = null;
                            }
                        }
                    }

                }
            }




        }
        catch (Exception ex)
        {
            hdnInvalidExcelRecords.Value = "0";
            DisplayMessage("Error in excel uploading");
        }
        finally
        {
            if (hdnInvalidExcelRecords.Value != "0")
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
        DataTable _dtCustomer = objCustomer.GetCustomerAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
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
                if (string.IsNullOrEmpty(_cls.customer_name))
                {
                    _cls.is_valid = false;
                    _cls.validation_remark = "invalid customer";
                    continue;
                }
                else
                {
                    using (DataTable _tmpCustomer = new DataView(_dtCustomer, "Name='" + _cls.customer_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpCustomer.Rows.Count > 0)
                        {
                            _cls.customer_id = _tmpCustomer.Rows[0]["customer_id"].ToString();
                        }
                        else
                        {
                            _cls.is_valid = false;
                            _cls.validation_remark = "invalid customer";
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

        Session["ExcelCustomerObList"] = lstObExcel;
        return lstObExcel;

    }
    protected List<clsAgeingExcelImport> validateAgeingExcelData(List<clsAgeingExcelImport> lstObAgeing)
    {
        if (lstObAgeing.Count == 0)
        {
            return null;
        }

        DataTable _dtCustomer = objCustomer.GetCustomerAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataTable _dtCurrency = objCurrency.Get_ActiveCurrencyMaster();
        DataTable _dtLocation = objLocation.GetLocationMaster(Session["CompId"].ToString());
        DataTable _dtAcMaster = objAccMaster.GetActiveAccountsWithFyearObByRefType("Customer", Session["CompId"].ToString(), Session["FinanceYearId"].ToString());

        var grpAgeing = lstObAgeing.GroupBy(x => new { x.customer_name, x.customer_id })
            .Select(g => new { customer_name = g.Key.customer_name, currency_id = g.Key.customer_id, total_due_amt = g.Sum(x => x.due_amount) });


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
                if (string.IsNullOrEmpty(_cls.customer_name))
                {
                    _cls.validation_remark = "invalid customer";
                    continue;
                }
                else
                {
                    using (DataTable _tmpCustomer = new DataView(_dtCustomer, "Name='" + _cls.customer_name + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpCustomer.Rows.Count > 0)
                        {
                            _cls.customer_id = _tmpCustomer.Rows[0]["customer_id"].ToString();
                        }
                        else
                        {
                            _cls.validation_remark = "invalid customer";
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


                if (string.IsNullOrEmpty(_cls.invoice_no) || _cls.invoice_no=="0")
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
                    using (DataTable _tmpAcMaster = new DataView(_dtAcMaster, "location_id= '" + _cls.location_id + "' and ref_id='" + _cls.customer_id + "' and currency_id='" + _cls.currency_id + "'", "", DataViewRowState.CurrentRows).ToTable())
                    {
                        if (_tmpAcMaster.Rows.Count == 0)
                        {
                            _cls.validation_remark = "Account does not exist or there is no opening balance on this location";
                            continue;
                        }
                        else
                        {
                            _cls.other_account_id = _tmpAcMaster.Rows[0]["Trans_id"].ToString();

                            var TotalDue = lstObAgeing.Where(x => x.customer_name == _cls.customer_name && x.currency_id == _cls.currency_id && x.location_id==_cls.location_id).Sum(x => x.due_amount);
                            if (_cls.loc_currency_id == _cls.currency_id)
                            {
                                if (TotalDue > Double.Parse(_tmpAcMaster.Rows[0]["lBalance"].ToString()))
                                {
                                    _cls.validation_remark = "Ageing is Greater then opening balance please check";
                                    continue;
                                }

                            }
                            else
                            {
                                if (TotalDue > Double.Parse(_tmpAcMaster.Rows[0]["fBalance"].ToString()))
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

        Session["ExcelCustomerAgeingList"] = lstObAgeing;
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


        List<clsObExcelImport> lstObList = (List<clsObExcelImport>)Session["ExcelCustomerObList"];
        if (lstObList.Count == 0)
        {
            return;
        }

        if (lstObList.Where(m => m.is_valid == false).Count() > 0)
        {
            return;
        }
        string strCustomerAccount = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strAcDocNo = new Set_DocNumber(Session["DBConnection"].ToString()).GetDocumentNo(true, "0", false, "150", "399", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
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
                int otherAccountId = objAccMaster.GetCustomerAccountByCurrency(_cls.customer_id, _cls.currency_id);
                if (otherAccountId == 0)
                {
                    //create new account in account master
                    int i = objAccMaster.InsertAc_AccountMaster("0", "Customer", _cls.customer_id, strAcDocNo, _cls.currency_id, "", "", "", false.ToString(), "01-Jan-1900", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
                int b = objSubCOA.InsertSubCOA(Session["FinanceYearId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), _cls.location_id, strCustomerAccount, otherAccountId.ToString(), lDrAmt.ToString(), lCrAmt.ToString(), fDrAmt.ToString(), fCrAmt.ToString(), _cls.currency_id, "0", "0", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
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
        List<clsAgeingExcelImport> lstObList = (List<clsAgeingExcelImport>)Session["ExcelCustomerAgeingList"];
        if (lstObList.Count == 0)
        {
            return;
        }

        if (lstObList.Where(m => m.is_valid == false).Count() > 0)
        {
            return;
        }
        string strCustomerAccount = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
        string strAcDocNo = new Set_DocNumber(Session["DBConnection"].ToString()).GetDocumentNo(true, "0", false, "150", "399", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            foreach (var _cls in lstObList)
            {
                //delete entry that exist with same invoice
                objAcAgeing.DeleteAgeingDetailByInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), _cls.location_id, "0", "SINV", _cls.invoice_no,_cls.other_account_id, ref trns);
                
                //insert new record
                int b = objAcAgeing.InsertAgeingDetail(Session["CompId"].ToString(),
                    Session["BrandId"].ToString(),
                    _cls.location_id,
                    "SINV",
                    "0",
                    _cls.invoice_no,
                    _cls.invoice_date,
                    strCustomerAccount,
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
                    "RV",
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
    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xls");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }

    protected void lnkDownloadData_Click(object sender, EventArgs e)
    {

        try
        {
            using (DataTable _dt = new DataTable())
            {
                _dt.Columns.Add("location");
                _dt.Columns.Add("account_no");
                _dt.Columns.Add("account_name");
                _dt.Columns.Add("account_name_l");
                _dt.Columns.Add("account_group");
                _dt.Columns.Add("opening_balance");
                _dt.Columns.Add("balance_type");
                DataTable _dtAc = objCOA.GetRecordsForExcelDownload(Session["compId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
                if (_dtAc.Rows.Count > 0)
                {
                    foreach (DataRow _dr in _dtAc.Rows)
                    {
                        DataRow _drNewRow = _dt.Rows.Add();
                        _drNewRow["location"] = "";
                        _drNewRow["account_no"] = _dr["account_no"].ToString();
                        _drNewRow["account_name"] = _dr["AccountName"].ToString();
                        _drNewRow["account_name_l"] = _dr["AccountNameL"].ToString();
                        _drNewRow["account_group"] = _dr["Ac_GroupName"].ToString();
                        _drNewRow["opening_balance"] = _dr["opening_balance"].ToString();
                        _drNewRow["balance_type"] = _dr["balance_type"].ToString();
                    }
                }
                ExportTableData(_dt, "ChartOfAccount");
            }
        }
        catch (Exception ex)
        {

        }

    }
}
