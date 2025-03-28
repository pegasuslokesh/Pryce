using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using PegasusDataAccess;
using Newtonsoft.Json;
using System.Collections.Generic;

public partial class Sales_SalesInquiry : BasePage
{
    #region defined Class Object
    Common cmn = null;
    SalesLeadClass SLClass = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Inv_SalesInquiryDetail objSInquiryDetail = null;
    Inv_SalesQuotationHeader objSquotationHeader = null;
    IT_ObjectEntry objObjectEntry = null;
    Inv_StockDetail objStockDetail = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmployee = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    CurrencyMaster objCurrency = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_ParameterMaster objInvParam = null;
    Inv_Product_RelProduct objRelProduct = null;
    Inv_SalesOrderHeader objSOHeader = null;
    Inv_SalesQuotationHeader ObjsalesQuatat = null;
    LocationMaster objLocation = null;
    CallLogs objCalllogs = null;
    DataAccessClass objDa = null;
    Campaign objCampaign = null;
    FollowUp FollowupClass = null;
    Document_Master ObjDocument = null;
    Arc_Directory_Master objDirectorymaster = null;
    Arc_FileTransaction ObjFile = null;
    Set_CustomerMaster objCustomer = null;
    PageControlCommon objPageCmn = null;

    string strCurrencyId = string.Empty;
    static string locationCondition = "";
    PageControlsSetting objPageCtlSettting = null;
    public const int grdDefaultColCount = 6;
    private const string strPageName = "SalesInquiry";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        SLClass = new SalesLeadClass(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        objSInquiryDetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        objSquotationHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objRelProduct = new Inv_Product_RelProduct(Session["DBConnection"].ToString());
        objSOHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjsalesQuatat = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCalllogs = new CallLogs(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objCampaign = new Campaign(Session["DBConnection"].ToString());
        FollowupClass = new FollowUp(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDirectorymaster = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        txtInquiryDate.Attributes.Add("readonly", "true");
        txtOrderCompletionDate.Attributes.Add("readonly", "true");
        txtTenderDate.Attributes.Add("readonly", "true");
        try
        {
            strCurrencyId = Session["LocCurrencyId"].ToString();
        }
        catch
        {

        }

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../sales/salesinquiry.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }


            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            hdnLocationId.Value = ddlLocation.SelectedValue;
            ddlOption.SelectedIndex = 2;
            FillCurrency();
            FillUser();
            if (strCurrencyId != "0" && strCurrencyId != "")
            {
                ddlCurrency.SelectedValue = strCurrencyId;
                ddlPCurrency.SelectedValue = strCurrencyId;
            }

            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtender_txtOrderCompletionDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDateBin.Format = Session["DateFormat"].ToString();

            txtInquiryNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            txtInquiryDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtTenderDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            if (Session["EmpId"].ToString() != "0")
            {
                DataTable Dt_Emp_Temp = new DataTable();
                using (Dt_Emp_Temp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()))
                {
                    txtReceivedEmp.Text = Dt_Emp_Temp.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Emp_Temp.Rows[0]["Emp_Code"].ToString();
                    txtHandledEmp.Text = Dt_Emp_Temp.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Emp_Temp.Rows[0]["Emp_Code"].ToString();
                }
            }
            ddlBuyingPriority.SelectedIndex = 2;
            ddlInquiryType.SelectedIndex = 0;
            ViewState["Dtproduct"] = null;
            Session["DtSearchProduct"] = null;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            int day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            btnAddCustomer.Visible = IsAddCustomerPermission();


            if (Request.QueryString["SalesLeadID"] != null)
            {
                using (DataTable DtLeadDataByID = SLClass.getActiveLeadDataById(Request.QueryString["SalesLeadID"].ToString()))
                {
                    DataTable dtCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + DtLeadDataByID.Rows[0]["Customer_Id"].ToString() + "'");
                    if (dtCustomerName != null && dtCustomerName.Rows.Count>0)
                    {
                        string strCustomerId= dtCustomerName.Rows[0]["Trans_Id"].ToString();
                        string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                        string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                        txtCustomerName.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                        //txtCustomerName.Text = DtLeadDataByID.Rows[0]["Emp_name_Customer"].ToString() + "/" + DtLeadDataByID.Rows[0]["Customer_Id"].ToString();
                    }

                    DataTable dtContactName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + DtLeadDataByID.Rows[0]["Contact_Id"].ToString() + "'");
                    if (dtContactName != null && dtContactName.Rows.Count > 0)
                    {
                        string strContactId = dtContactName.Rows[0]["Trans_Id"].ToString();
                        string strContactEmail = dtContactName.Rows[0]["Field1"].ToString();
                        string strContactNumber = dtContactName.Rows[0]["Field2"].ToString();
                        txtContactList.Text = objContact.GetContactNameByContactiD(strContactId) + "/" + strContactNumber + "/" + strContactEmail + "/" + strContactId;
                        //txtContactList.Text = DtLeadDataByID.Rows[0]["Emp_name_Contact"].ToString() + "/" + DtLeadDataByID.Rows[0]["Contact_Id"].ToString();
                    }


                    ddlCurrency.SelectedValue = DtLeadDataByID.Rows[0]["Currency_ID"].ToString();
                    txtRemark.Text = DtLeadDataByID.Rows[0]["Remark"].ToString();
                    hdnLocationId.Value = DtLeadDataByID.Rows[0]["Location_id"].ToString();
                    HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                    string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(DtLeadDataByID.Rows[0]["Generated_by"].ToString());
                    txtReceivedEmp.Text = DtLeadDataByID.Rows[0]["Emp_name_GeneratedBy"].ToString() + "/" + Emp_Code;
                    string Emp_Code_1 = HR_EmployeeDetail.GetEmployeeCode(DtLeadDataByID.Rows[0]["Assign_to"].ToString());
                    txtHandledEmp.Text = DtLeadDataByID.Rows[0]["Emp_name_AssignTo"].ToString() + "/" + Emp_Code_1;
                    ddlLeadSource.SelectedValue = DtLeadDataByID.Rows[0]["Lead_source"].ToString();
                    if (DtLeadDataByID.Rows[0]["Campaign_Id"].ToString() != "0")
                    {
                        txtCampaignID.Text = DtLeadDataByID.Rows[0]["Campaign_Name"].ToString() + "/" + DtLeadDataByID.Rows[0]["Campaign_Id"].ToString();
                    }
                    txtCampaignID.Text = "";
                    ddlSalesStage.SelectedValue = DtLeadDataByID.Rows[0]["Lead_status"].ToString();
                    txtOpportunityAmt.Text = DtLeadDataByID.Rows[0]["Opportunity_amount"].ToString();
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryNo);
                    if (!ClientScript.IsStartupScriptRegistered("alert"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alertMe();", true);
                    }
                }
            }

            if (Request.QueryString["OpportunityID"] != null)
            {
                try
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    using (DataTable DtOppoDataByID = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString()).GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Request.QueryString["OpportunityID"].ToString()))
                    {
                        DataTable dtCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + DtOppoDataByID.Rows[0]["Customer_Id"].ToString() + "'");
                        if (dtCustomerName != null && dtCustomerName.Rows.Count > 0)
                        {
                            string strCustomerId = dtCustomerName.Rows[0]["Trans_Id"].ToString();
                            string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                            string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                            txtCustomerName.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                            //txtCustomerName.Text = DtOppoDataByID.Rows[0]["Name"].ToString() + "/" + DtOppoDataByID.Rows[0]["Customer_Id"].ToString();
                        }

                        DataTable dtContactName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + DtOppoDataByID.Rows[0]["Field2"].ToString() + "'");
                        if (dtContactName != null && dtContactName.Rows.Count > 0)
                        {
                            string strContactId = dtContactName.Rows[0]["Trans_Id"].ToString();
                            string strContactEmail = dtContactName.Rows[0]["Field1"].ToString();
                            string strContactNumber = dtContactName.Rows[0]["Field2"].ToString();
                            txtContactList.Text = objContact.GetContactNameByContactiD(strContactId) + "/" + strContactNumber + "/" + strContactEmail + "/" + strContactId;
                            //txtContactList.Text = DtOppoDataByID.Rows[0]["Contact_name"].ToString() + "/" + DtOppoDataByID.Rows[0]["Field2"].ToString();
                        }

                        ddlCurrency.SelectedValue = DtOppoDataByID.Rows[0]["Currency_ID"].ToString();
                        txtRemark.Text = DtOppoDataByID.Rows[0]["Remark"].ToString();
                        hdnLocationId.Value = DtOppoDataByID.Rows[0]["Location_id"].ToString();
                        txtOppoName.Text = DtOppoDataByID.Rows[0]["Opportunity_name"].ToString();
                        txtOrderCompletionDate.Text = GetDate(DtOppoDataByID.Rows[0]["OrderCompletionDate"].ToString());
                        ddlInquiryType.SelectedValue = DtOppoDataByID.Rows[0]["InquiryType"].ToString();
                        txtProbability.Text = DtOppoDataByID.Rows[0]["Probability"].ToString();
                        hdnSInquiryId.Value = Request.QueryString["OpportunityID"].ToString();
                        txtInquiryDate.Text = GetDate(DtOppoDataByID.Rows[0]["IDate"].ToString());
                        txtInquiryNo.Text = DtOppoDataByID.Rows[0]["SInquiryNo"].ToString();
                        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                        string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(DtOppoDataByID.Rows[0]["ReceivedEmpID"].ToString());
                        txtReceivedEmp.Text = DtOppoDataByID.Rows[0]["ReceivedEmployee"].ToString() + "/" + Emp_Code;
                        string Emp_Code_1 = HR_EmployeeDetail.GetEmployeeCode(DtOppoDataByID.Rows[0]["HandledEmpID"].ToString());
                        txtHandledEmp.Text = DtOppoDataByID.Rows[0]["HandledEmployee"].ToString() + "/" + Emp_Code_1;
                        ddlLeadSource.SelectedValue = DtOppoDataByID.Rows[0]["Lead_source"].ToString();
                        if (DtOppoDataByID.Rows[0]["Crm_campaign_id"].ToString() != "0")
                        {
                            txtCampaignID.Text = DtOppoDataByID.Rows[0]["Campaign_Name"].ToString() + "/" + DtOppoDataByID.Rows[0]["Crm_campaign_id"].ToString();
                        }
                        else
                        {
                            txtCampaignID.Text = "";
                        }
                        ddlSalesStage.SelectedValue = DtOppoDataByID.Rows[0]["Sales_Stage"].ToString();
                        txtOpportunityAmt.Text = DtOppoDataByID.Rows[0]["Opportunity_amount"].ToString();
                    }

                }
                catch
                {

                }

                try
                {
                    using (DataTable dt_opportunityDate = objSInquiryDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Request.QueryString["OpportunityID"].ToString(), Session["FinanceYearId"].ToString()))
                    {
                        GvProduct.DataSource = dt_opportunityDate;
                        GvProduct.DataBind();
                    }
                }
                catch
                {

                }
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryNo);
                if (!ClientScript.IsStartupScriptRegistered("alert"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alert", "alertMe();", true);
                }
            }

            if (Request.QueryString["ReminderID"] != null)
            {
                FillGrid(Request.QueryString["ReminderID"].ToString());
            }
            else
            {
                fillGrid(1);
            }
            FillUnit();
            AllPageCode(clsPagePermission);
            getPageControlsVisibility();
            fillConfigModalDropDown();
            txtOppoName.Focus();
        }
        ucCtlSetting.refreshControlsFromChild += new WebUserControl_ucControlsSetting.parentPageHandler(UcCtlSetting_refreshPageControl);
    }
    public void fillConfigModalDropDown()
    {
        DataTable dt = objEmployee.GetEmployeeMasterAll(Session["CompId"].ToString());
        dt = new DataView(dt, "Location_id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlPerSPersonName, dt, "emp_name", "emp_id");
            objPageCmn.FillData((object)ddlSharedSalesPerson, dt, "emp_name", "emp_id");
            if (ddlSharedSalesPerson.Items.Count > 0)
            {
                ddlSharedSalesPerson.Items.RemoveAt(0);
            }
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string GetProductPrice(string ProductId, string CustomerID)
    {
        string UnitPrice = "0";
        Inv_ProductMaster objProductM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            if (ProductId != null)
            {
                UnitPrice = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", CustomerID.Split('/')[1].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["Sales_Price"].ToString())).ToString();
                return UnitPrice;
            }
            else
            {
                return UnitPrice;

            }
        }
        catch
        {
            return UnitPrice;
        }

    }

    //added by divya parakh 3/7/2018
    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            if (dtLoc.Rows.Count > 1 && LocIds != "")
            {
                ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }

    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSInquirySave.Visible = clsPagePermission.bAdd;
        btnSinquirysaveandquotation.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        hdnCanFolloup.Value = "true";
        GvCallRequest.Columns[0].Visible = clsPagePermission.bAdd;
        //ddlUser.Visible = clsPagePermission.bViewAllUserRecord;
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        imgBtnSharedSalesPersonData.Visible = clsPagePermission.bViewAllUserRecord;
    }
    protected string GetDocumentNumber()
    {
        string docnum = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "54", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return docnum;
    }
    #region System defined Function
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //fileupload_div.Visible = true;

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;

        //hdnPrjectId.Value = e.CommandArgument.ToString();
        try
        {
            if (objSenderID != "lnkViewDetail")
            {
                hdnLocationId.Value = e.CommandName.ToString();
                string s = ObjsalesQuatat.GetQuotationIdBySInquiry_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, e.CommandArgument.ToString()).Rows[0]["SQuotation_Id"].ToString();
                if (objSOHeader.GetSOHeaderIDFromTransType(strCurrencyId, Session["BrandId"].ToString(), Session["LocId"].ToString(), "Q", s.ToString()).Rows.Count != 0)
                {
                    DisplayMessage("You can not edit ,sales order has created");
                    return;
                }
            }
        }
        catch (Exception err)
        {
        }
        using (DataTable dtSInquiryEdit = objSInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, e.CommandArgument.ToString()))
        {
            if (dtSInquiryEdit.Rows.Count > 0)
            {
                if (objSenderID != "lnkViewDetail")
                {
                    if (Convert.ToBoolean(dtSInquiryEdit.Rows[0]["Post"].ToString()))
                    {
                        DisplayMessage("Record is posted,you can not Edit this record");
                        //AllPageCode();
                        return;
                    }
                }
                hdnSInquiryId.Value = e.CommandArgument.ToString();
                string hadQuotation = "0";
                hadQuotation = objDa.get_SingleValue("select count(squotation_id) from inv_salesquotationheader where sinquiry_no='" + hdnSInquiryId.Value + "' and isActive='true' and company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "'"); //objSquotationHeader.GetQuotationHeaderAllBySInquiry_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnSInquiryId.Value))
                {
                    if (hadQuotation != "0")
                    {
                        hdnSquotationExist.Value = e.CommandArgument.ToString();
                    }
                    else
                    {
                        hdnSquotationExist.Value = "0";
                    }
                }


                if (objSenderID == "lnkViewDetail")
                {
                    Lbl_Tab_New.Text = Resources.Attendance.View;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }
                else
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }

                hdnLocationId.Value = dtSInquiryEdit.Rows[0]["Location_Id"].ToString();
                txtInquiryNo.Text = dtSInquiryEdit.Rows[0]["SInquiryNo"].ToString();
                ViewState["TimeStamp"] = dtSInquiryEdit.Rows[0]["Row_Lock_Id"].ToString();
                txtInquiryNo.ReadOnly = true;
                txtInquiryDate.Text = Convert.ToDateTime(dtSInquiryEdit.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());//updated by jitendra upadhyay on 07-nov-2013
                ddlInquiryType.SelectedValue = dtSInquiryEdit.Rows[0]["InquiryType"].ToString();

                if (ddlInquiryType.SelectedItem.Text == "Tender" || ddlInquiryType.SelectedItem.Text == "Semi Tender")
                {
                    //lblTenderDate.Visible = true;
                    //lblTenderNo.Visible = true;
                    txtTenderDate.Visible = true;
                    txtTenderNo.Visible = true;
                }
                else
                {
                    //lblTenderDate.Visible = false;
                    //lblTenderNo.Visible = false;
                    txtTenderDate.Visible = false;
                    txtTenderNo.Visible = false;
                }

                txtOrderCompletionDate.Text = Convert.ToDateTime(dtSInquiryEdit.Rows[0]["OrderCompletionDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                if (dtSInquiryEdit.Rows[0]["TenderDate"].ToString() != "")
                {
                    txtTenderDate.Text = Convert.ToDateTime(dtSInquiryEdit.Rows[0]["TenderDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                }
                if (dtSInquiryEdit.Rows[0]["Field3"].ToString() != "0" && dtSInquiryEdit.Rows[0]["Field3"].ToString().Trim() != "")
                {
                    txtCallNo.Text = dtSInquiryEdit.Rows[0]["Call_No"].ToString();
                    txtCallDate.Text = Convert.ToDateTime(dtSInquiryEdit.Rows[0]["Call_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    //trCallLogs.Visible = true;
                }
                txtTenderNo.Text = dtSInquiryEdit.Rows[0]["TenderNo"].ToString();

                string strCustomerId = dtSInquiryEdit.Rows[0]["Customer_Id"].ToString();
                DataTable dtCustomerDetail = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strCustomerId + "'");
                if (dtCustomerDetail != null)
                {
                    string strCustomerEmail = dtCustomerDetail.Rows[0]["Field1"].ToString();
                    string strCustomerNumber = dtCustomerDetail.Rows[0]["Field2"].ToString();
                    txtCustomerName.Text = objContact.GetContactNameByContactiD(strCustomerId) + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                }

                Session["ContactID"] = strCustomerId;

                string strContactId = dtSInquiryEdit.Rows[0]["Field2"].ToString();
                DataTable dtContactName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strContactId + "'");
                if (dtContactName != null)
                {
                    string strContactEmail = dtContactName.Rows[0]["Field1"].ToString();
                    string strContactNumber = dtContactName.Rows[0]["Field2"].ToString();
                    txtContactList.Text = objContact.GetContactNameByContactiD(strContactId) + "/" + strContactNumber + "/" + strContactEmail + "/" + strContactId;
                }



                ddlCurrency.SelectedValue = dtSInquiryEdit.Rows[0]["Currency_Id"].ToString();
                txtRemark.Text = dtSInquiryEdit.Rows[0]["Remark"].ToString();
                string strREmployeeId = dtSInquiryEdit.Rows[0]["ReceivedEmpID"].ToString();

                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(strREmployeeId);
                txtReceivedEmp.Text = objEmployee.GetEmployeeNameByEmployeeId(strREmployeeId, Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value) + "/" + Emp_Code;
                string strHEmployeeId = dtSInquiryEdit.Rows[0]["HandledEmpID"].ToString();
                string Emp_Code_1 = HR_EmployeeDetail.GetEmployeeCode(strHEmployeeId);
                txtHandledEmp.Text = objEmployee.GetEmployeeNameByEmployeeId(strHEmployeeId, Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value) + "/" + Emp_Code_1;

                string strBuyingPriority = dtSInquiryEdit.Rows[0]["BuyingPriority"].ToString();
                txtOppoName.Text = dtSInquiryEdit.Rows[0]["Opportunity_name"].ToString();
                ddlLeadSource.SelectedValue = dtSInquiryEdit.Rows[0]["Lead_source"].ToString();
                ddlSalesStage.SelectedValue = dtSInquiryEdit.Rows[0]["Sales_Stage"].ToString();
                txtOpportunityAmt.Text = dtSInquiryEdit.Rows[0]["Opportunity_amount"].ToString();
                txtProbability.Text = dtSInquiryEdit.Rows[0]["Probability"].ToString();


                if (dtSInquiryEdit.Rows[0]["Campaign_name"].ToString().Trim() != "")
                    txtCampaignID.Text = dtSInquiryEdit.Rows[0]["Campaign_name"].ToString() + "/" + dtSInquiryEdit.Rows[0]["Crm_campaign_id"].ToString();
                else
                    txtCampaignID.Text = "";

                if (strBuyingPriority != "")
                {

                    try
                    {
                        ddlBuyingPriority.SelectedValue = strBuyingPriority;
                    }
                    catch
                    {
                        ddlBuyingPriority.SelectedIndex = 0;
                    }

                }
                else if (strBuyingPriority == "")
                {

                    ddlBuyingPriority.SelectedIndex = 0;
                }

                string strSendEmail = dtSInquiryEdit.Rows[0]["EmailSendFlag"].ToString();
                if (strSendEmail == "True")
                {
                    chkSendMail.Checked = true;
                }
                else if (strSendEmail == "False")
                {
                    chkSendMail.Checked = false;
                }
                string strPost = dtSInquiryEdit.Rows[0]["Post"].ToString();


                txtCondition1.Content = dtSInquiryEdit.Rows[0]["Condition1"].ToString();

                try
                {
                    if (dtSInquiryEdit.Rows[0]["Field1"].ToString() == "FWInPur")
                    {

                        ChkSendInPurchase.Checked = true;
                    }
                    else
                    {
                        if (dtSInquiryEdit.Rows[0]["Field1"].ToString() == "Send Inquiry To Supplier")
                        {
                            ChkSendInPurchase.Checked = true;
                        }
                        else
                        {
                            ChkSendInPurchase.Checked = false;
                        }
                    }
                }
                catch
                {

                }
                //Add Child Concept
                try
                {
                    using (DataTable dtDetail = objSInquiryDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, hdnSInquiryId.Value, Session["FinanceYearId"].ToString()))
                    {
                        if (dtDetail.Rows.Count > 0)
                        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                            objPageCmn.FillData((object)GvProduct, dtDetail, "", "");
                            ViewState["Dtproduct"] = dtDetail;
                            GvProduct.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Estimated Unit Price", Session["DBConnection"].ToString());
                        }
                    }
                }
                catch (Exception err)
                {
                }
            }
        }

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryDate);
        //AllPageCode();
        if (objSenderID == "lnkViewDetail")
        {
            btnSInquirySave.Enabled = false;
            BtnReset.Visible = false;
            btnSinquirysaveandquotation.Enabled = false;
        }
        else
        {
            btnSInquirySave.Enabled = true;
            BtnReset.Visible = true;
            btnSinquirysaveandquotation.Enabled = true;
        }
    }

    protected void btnbindrpt_Click(object sender, EventArgs e)
    {

        string condition = string.Empty;
        if (ddlFieldName.SelectedItem.Value == "IDate" || ddlFieldName.SelectedItem.Value == "OrderCompletionDate")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString("dd-MMM-yyyy");
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                return;
            }
        }
        fillGrid(1);
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedItem.Value == "IDate" || ddlFieldName.SelectedItem.Value == "OrderCompletionDate")
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";
            ddlPermission.Visible = false;
        }
        else
        {
            if (ddlFieldName.SelectedItem.Value == "Field4")
            {
                txtValueDate.Visible = false;
                txtValue.Visible = false;
                txtValue.Text = "";
                txtValueDate.Text = "";
                ddlPermission.Visible = true;
            }
            else
            {
                txtValueDate.Visible = false;
                txtValue.Visible = true;
                txtValue.Text = "";
                txtValueDate.Text = "";
                ddlPermission.Visible = false;
            }
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedItem.Value == "IDate" || ddlFieldNameBin.SelectedItem.Value == "OrderCompletionDate")
        {
            txtValueDateBin.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

        }
        else
        {
            txtValueDateBin.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueDateBin.Text = "";

        }
    }
    protected void GvSalesInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvSalesInquiry.Attributes["CurrentSortField"] != null &&
            GvSalesInquiry.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesInquiry.Attributes["CurrentSortField"])
            {
                if (GvSalesInquiry.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesInquiry.Attributes["CurrentSortField"] = sortField;
        GvSalesInquiry.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        fillGrid(Int32.Parse(hdnGvSalesOpportunityCurrentPageIndex.Value));
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        int b = 0;


        if (((Label)gvrow.FindControl("lblQuotationCrerated")).Text != "0")
        {
            DisplayMessage("Sales Inquiry is used in Sales Quotation");
            return;
        }

        hdnSInquiryId.Value = e.CommandArgument.ToString();
        //DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Location_Name='" + ddlLocation.SelectedItem + "'", "", DataViewRowState.CurrentRows).ToTable();

        b = objSInquiryHeader.DeleteSIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, hdnSInquiryId.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        //FillGridBin(); //Update grid view in bin tab
        try
        {
            fillGrid(Convert.ToInt32(hdnGvSalesOpportunityCurrentPageIndex.Value));
        }
        catch
        {
            fillGrid(1);
        }
        Reset();
        //AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        ddlPermission.SelectedIndex = 0;
        txtValueDate.Visible = false;
        ddlPermission.Visible = false;
        txtValue.Visible = true;
        if (Request.QueryString["ReminderID"] != null)
        {
            FillGrid(Request.QueryString["ReminderID"].ToString());
        }
        else
        {
            fillGrid(1);
        }
    }
    protected void btnSInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();
        //fillGrid(1);
        //AllPageCode();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryDate);
    }
    protected void btnSinquirysaveandquotation_Click(object sender, EventArgs e)
    {
        Button btnsave = (Button)sender;

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        //here we check that this page is updated by other user before save of current user 
        //this code is created by jitendra upadhyay on 02-06-2015
        //code start
        if (hdnSInquiryId.Value != "0")
        {
            using (DataTable dtSInquiryEdit = objSInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnSInquiryId.Value))
            {
                if (ViewState["TimeStamp"] != null)
                {
                    if (dtSInquiryEdit.Rows.Count != 0)
                    {
                        if (ViewState["TimeStamp"].ToString() != dtSInquiryEdit.Rows[0]["Row_Lock_Id"].ToString())
                        {
                            DisplayMessage("Another User update Information reload and try again");
                            btnSinquirysaveandquotation.Enabled = true;
                            return;
                        }
                    }
                }
            }
        }
        //code end


        string SalesInquiryId = string.Empty;
        string strCustomerId = string.Empty;
        string strContactId = string.Empty;
        string strReceivedEmployeeId = string.Empty;
        string strHandledEmployeeId = string.Empty;
        string strBuyingPriority = string.Empty;
        string strSendMail = string.Empty;
        string strPost = string.Empty;

        if (txtOppoName.Text.Trim() == "")
        {
            DisplayMessage("Enter Opportunity Name");
            txtOppoName.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }

        // modified on 2/4/2018
        if (txtInquiryDate.Text == "")
        {
            DisplayMessage("Enter Sales Opportunity date");
            txtInquiryDate.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        if (txtOrderCompletionDate.Text == "")
        {
            DisplayMessage("Enter Order Close Date");
            txtOrderCompletionDate.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }

        if (ddlInquiryType.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Opportunity Type");
            ddlInquiryType.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            if (ddlInquiryType.SelectedIndex == 2 || ddlInquiryType.SelectedIndex == 3)
            {
                ddlInquiryType_SelectedIndexChanged(null, null);
                if (txtTenderDate.Text == "")
                {
                    DisplayMessage("Please Enter Sales Tender Date");
                    txtTenderDate.Focus();
                    btnSinquirysaveandquotation.Enabled = true;
                    return;
                }
            }
        }

        if (txtTenderDate.Text == "")
        {
            txtTenderDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }

        // end

        if (txtInquiryNo.Text == "")
        {
            DisplayMessage("Enter Opportunity No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryNo);
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            if (hdnSInquiryId.Value == "0")
            {
                string sql = "select count(*) from Inv_SalesInquiryHeader where company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + Session["LocId"].ToString() + "' and SInquiryNo='" + txtInquiryNo.Text + "'";
                int recCount = 0;
                int.TryParse(objDa.get_SingleValue(sql).ToString(), out recCount);
                if (recCount > 0)
                {
                    DisplayMessage("Sales Opportunity No. Already Exits");
                    txtInquiryNo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryNo);
                    btnSinquirysaveandquotation.Enabled = true;
                    return;
                }
            }
        }
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Fill Customer Name");
            txtCustomerName.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            //strCustomerId = objContact.GetContactIdByContactName(txtCustomerName.Text.Split('/')[0]);
            strCustomerId = txtCustomerName.Text.Split('/')[3];
            if (strCustomerId == "" || strCustomerId == "0")
            {
                DisplayMessage("Select Customer Name In Suggestions Only");
                txtCustomerName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
                btnSinquirysaveandquotation.Enabled = true;
                return;
            }
        }
        if (txtContactList.Text == "")
        {
            DisplayMessage("Select Contact Name");
            txtContactList.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            try
            {
                strContactId = txtContactList.Text.Split('/')[3];
            }
            catch
            {
                strContactId = "0";
            }
            //  strContactId = objContact.GetContactIdByContactName(txtContactList.Text.Split('/')[0]);
            if (strContactId == "" || strContactId == "0")
            {
                DisplayMessage("Select Contact Name In Suggestions Only");
                txtContactList.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactList);
                btnSinquirysaveandquotation.Enabled = true;
                return;
            }
        }
        if (ddlCurrency.SelectedValue == "--Select--")
        {
            DisplayMessage("Currency Required On Company Level");
            ddlCurrency.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }



        //if (GvProduct.Rows.Count == 0)
        //{
        //    DisplayMessage("Enter Product");
        //    btnProductSave.Focus();
        //    return;
        //}
        //here we et the validation of product add is necessary

        if (txtReceivedEmp.Text == "")
        {
            DisplayMessage("Enter Received Employee Name");
            txtReceivedEmp.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtReceivedEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

            //strReceivedEmployeeId = GetEmployeeId(txtReceivedEmp.Text);
            strReceivedEmployeeId = Emp_ID;
            if (strReceivedEmployeeId == "" || strReceivedEmployeeId == "0")
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtReceivedEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReceivedEmp);
                btnSinquirysaveandquotation.Enabled = true;
                return;
            }
        }

        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Enter Handled Employee Name");
            txtHandledEmp.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strHandledEmployeeId = Emp_ID;
            if (strHandledEmployeeId == "" || strHandledEmployeeId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtHandledEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmp);
                btnSinquirysaveandquotation.Enabled = true;
                return;
            }
        }
        if (txtOpportunityAmt.Text == "")
        {
            DisplayMessage("Enter Opportunity Amount");
            txtOpportunityAmt.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        int parsedValue;
        float parseddecimal;

        if (!int.TryParse(txtOpportunityAmt.Text, out parsedValue))
        {
            if (!float.TryParse(txtOpportunityAmt.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                txtOpportunityAmt.Text = "";
                txtOpportunityAmt.Focus();
                btnSinquirysaveandquotation.Enabled = true;
                return;
            }
        }
        if (ddlBuyingPriority.SelectedIndex == 0)
        {
            DisplayMessage("Select Buying Priority");

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlBuyingPriority);
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        else
        {
            strBuyingPriority = ddlBuyingPriority.SelectedValue;
        }
        if (chkSendMail.Checked == true)
        {
            strSendMail = "True";
        }
        else
        {
            strSendMail = "False";
        }

        strPost = "False";



        string InquiryStatus = string.Empty;
        InquiryStatus = "Send Inquiry To Supplier";

        string SalesLeadID;
        if (txtProbability.Text.Trim() == "")
        {
            DisplayMessage("Must Enter Probability");
            txtProbability.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        if (Convert.ToInt32(txtProbability.Text.Trim()) > 101)
        {
            DisplayMessage("Probability must be less than or equals to 100");
            txtProbability.Text = "";
            txtProbability.Focus();
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }

        if (Request.QueryString["SalesLeadID"] == null)
        {
            SalesLeadID = "0";
        }
        else
        {
            if (Request.QueryString["SalesLeadID"].ToString() != null)
            {
                SalesLeadID = Request.QueryString["SalesLeadID"].ToString();
            }
            else
            {
                SalesLeadID = "0";
            }

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
        int b = 0;
        try
        {
            if (hdnSInquiryId.Value != "0")
            {
                SalesInquiryId = hdnSInquiryId.Value;
                string id = "0";
                if (txtCampaignID.Text.Trim() != "")
                {
                    int start_pos = txtCampaignID.Text.LastIndexOf("/") + 1;
                    int last_pos = txtCampaignID.Text.Length;
                    id = txtCampaignID.Text.Substring(start_pos, last_pos - start_pos);
                }
                b = objSInquiryHeader.UpdateSIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, hdnSInquiryId.Value, txtInquiryNo.Text, ObjSysParam.getDateForInput(txtInquiryDate.Text).ToString(), txtTenderNo.Text.Trim(), ObjSysParam.getDateForInput(txtTenderDate.Text).ToString(), ddlInquiryType.SelectedValue, ObjSysParam.getDateForInput(txtOrderCompletionDate.Text).ToString(), strCustomerId, ddlCurrency.SelectedValue, txtRemark.Text, strReceivedEmployeeId, strHandledEmployeeId, strBuyingPriority, strSendMail, txtCondition1.Content, "", "", "", "", strPost, InquiryStatus.ToString(), strContactId, "", ddlAccessType.SelectedItem.ToString(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), txtOppoName.Text, ddlLeadSource.SelectedItem.ToString(), id, ddlSalesStage.SelectedItem.ToString(), txtOpportunityAmt.Text, txtProbability.Text, SalesLeadID, ref trns);
            }
            else
            {
                string id = "0";
                if (txtCampaignID.Text.Trim() != "")
                {
                    int start_pos = txtCampaignID.Text.LastIndexOf("/") + 1;
                    int last_pos = txtCampaignID.Text.Length;
                    id = txtCampaignID.Text.Substring(start_pos, last_pos - start_pos);
                }
                b = objSInquiryHeader.InsertSIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text, ObjSysParam.getDateForInput(txtInquiryDate.Text).ToString(), txtTenderNo.Text.Trim(), ObjSysParam.getDateForInput(txtTenderDate.Text).ToString(), ddlInquiryType.SelectedValue, ObjSysParam.getDateForInput(txtOrderCompletionDate.Text).ToString(), strCustomerId, ddlCurrency.SelectedValue, txtRemark.Text, strReceivedEmployeeId, strHandledEmployeeId, strBuyingPriority, strSendMail, txtCondition1.Content, "", "", "", "", strPost, InquiryStatus.ToString(), strContactId, "", ddlAccessType.SelectedItem.ToString(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtOppoName.Text, ddlLeadSource.SelectedItem.ToString(), id, ddlSalesStage.SelectedItem.ToString(), txtOpportunityAmt.Text, txtProbability.Text, SalesLeadID, ref trns);
                if (b != 0)
                {
                    SalesInquiryId = b.ToString();
                    if (txtInquiryNo.Text == ViewState["DocNo"].ToString())
                    {
                        string count = "0";
                        count = objDa.get_SingleValue(" select COUNT(SInquiryID)+1 from inv_salesinquiryheader where Company_Id='" + Session["CompId"].ToString() + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", ref trns);
                        if (count == "1")
                        {
                            objSInquiryHeader.Updatecode(b.ToString(), txtInquiryNo.Text + "1", ref trns);
                            txtInquiryNo.Text = txtInquiryNo.Text + "1";
                        }
                        else
                        {
                            objSInquiryHeader.Updatecode(b.ToString(), txtInquiryNo.Text + count, ref trns);
                            txtInquiryNo.Text = txtInquiryNo.Text + count;
                        }
                    }
                }
                if (txtCallNo.Text.Trim() != "")
                {
                    objCalllogs.updateCallStatusByCallNo("Close", txtCallNo.Text, Session["UserId"].ToString());
                }

                if (SalesLeadID != "0")
                {
                    using (DataTable dtAllData = SLClass.getActiveLeadDataById(SalesLeadID))
                    {
                        string date1 = GetDate(dtAllData.Rows[0]["Lead_date"].ToString());
                        string leadStatus = "";

                        if (dtAllData.Rows[0]["Lead_status"].ToString() == "" || dtAllData.Rows[0]["Lead_status"].ToString() == "New")
                        {
                            leadStatus = "In Process";
                            SLClass.UpdateLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtAllData.Rows[0]["Lead_no"].ToString(), ObjSysParam.getDateForInput(date1).ToString(), dtAllData.Rows[0]["Customer_Id"].ToString(), dtAllData.Rows[0]["Contact_Id"].ToString(), dtAllData.Rows[0]["Title"].ToString(), dtAllData.Rows[0]["Lead_source"].ToString(), leadStatus, dtAllData.Rows[0]["Currency_ID"].ToString(), dtAllData.Rows[0]["Opportunity_amount"].ToString(), dtAllData.Rows[0]["Source_description"].ToString(), dtAllData.Rows[0]["Status_description"].ToString(), dtAllData.Rows[0]["Remark"].ToString(), dtAllData.Rows[0]["Generated_by"].ToString(), dtAllData.Rows[0]["Assign_to"].ToString(), dtAllData.Rows[0]["Campaign_Id"].ToString(), dtAllData.Rows[0]["Refered_by"].ToString(), Session["EmpID"].ToString(), Session["EmpID"].ToString());
                        }
                    }
                }
            }

            if (GvProduct.Rows.Count > 0)
            {
                hdnLocationId.Value = hdnLocationId.Value == "" ? Session["LocId"].ToString() : hdnLocationId.Value;

                objSInquiryDetail.DeleteSIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, hdnSInquiryId.Value, ref trns);
                foreach (GridViewRow gvr in GvProduct.Rows)
                {
                    Label lblSerialNo = (Label)gvr.FindControl("lblSNo");
                    Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
                    Label lblUnitId = (Label)gvr.FindControl("lblgvUnitId");
                    Label lblProductDescription = (Label)gvr.FindControl("lblgvProductDescription");
                    Label lblCurrencyId = (Label)gvr.FindControl("lblgvCurrencyId");
                    TextBox txtgvEstimatedUnitPrice = (TextBox)gvr.FindControl("txtgvEstimatedUnitPrice");
                    Label lblRequiredQty = (Label)gvr.FindControl("lblgvRequiredQty");
                    HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                    Label lblgvProductName = (Label)gvr.FindControl("lblgvProductName");
                    TextBox txtgvFrequency1 = (TextBox)gvr.FindControl("txtgvFrequency");
                    if (txtgvEstimatedUnitPrice.Text == "")
                    {
                        txtgvEstimatedUnitPrice.Text = "0";
                    }
                    if (txtgvFrequency1.Text == "" || txtgvFrequency1.Text == null)
                    {
                        txtgvFrequency1.Text = "0";
                    }
                    objSInquiryDetail.InsertSIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, SalesInquiryId, lblSerialNo.Text, lblProductId.Text, lblgvProductName.Text, lblUnitId.Text, lblProductDescription.Text, lblCurrencyId.Text, txtgvEstimatedUnitPrice.Text, lblRequiredQty.Text, "0", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtgvFrequency1.Text, ref trns);
                }
            }

            if (hdnSInquiryId.Value != "0")
            {
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Saved", "green");
            }
            btnSinquirysaveandquotation.Enabled = true;

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            fillGrid(1);
            Reset();
            //AllPageCode();
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
            btnSinquirysaveandquotation.Enabled = true;
            return;
        }
        if (b != 0)
        {
            using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsProductNameShow"))
            {
                if (Dt.Rows.Count == 0)
                {
                    DisplayMessage("First Set company parameter for product");
                    btnSinquirysaveandquotation.Enabled = true;
                    return;
                }
            }

            if (btnsave.ID == "btnSinquirysaveandquotation")
            {
                if (Session["EmpId"].ToString() == "0")
                {
                    //Response.Redirect("../Sales/SalesQuatationJScript.aspx?InquiryId=" + SalesInquiryId.ToString());
                    Response.Redirect("../Sales/SalesQuotation.aspx?InquiryId=" + SalesInquiryId.ToString());
                }
                else
                {
                    using (DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString().ToString(), "144", "57", Session["CompId"].ToString()))
                    {
                        if (dtAllPageCode.Rows.Count != 0)
                        {
                            //Response.Redirect("../Sales/SalesQuatationJScript.aspx?InquiryId=" + SalesInquiryId.ToString());
                            Response.Redirect("../Sales/SalesQuotation.aspx?InquiryId=" + SalesInquiryId.ToString());
                        }
                        else
                        {
                            DisplayMessage("You have no permission of sales quotation");
                        }
                    }
                }
            }
            else
            {
                if (txtValue.Text.Trim() != "" || txtValueDate.Text.Trim() != "")
                {
                    btnbindrpt_Click(null, null);
                }
            }
        }
    }
    //used on client side on product table
    protected string GetUnitName(string strUnitId)
    {
        return UM.GetUnitNameByUnitId(strUnitId, Session["CompId"].ToString());
    }
    //used on client side on product table
    public string ProductCode(string ProductId)
    {
        return objProductM.GetProductCodebyProductId(ProductId.ToString());
    }
    //used on client side on product table
    public string SuggestedProductName(string ProductId)
    {
        return objProductM.GetProductNamebyProductId(ProductId.ToString());
    }

    public string getShortProductName(string ProductId)
    {
        string strProductName = "";
        strProductName = objProductM.GetProductNamebyProductId(ProductId.ToString());
        if (strProductName.Length > 16)
        {
            strProductName = strProductName.Substring(0, 15) + "...";
        }
        return strProductName;
    }

    protected void btnSInquirySave_Click(object sender, EventArgs e)
    {
        if (hdnLocationId.Value == "0" && hdnLocationId.Value == "")
        {
            hdnLocationId.Value = Session["LocId"].ToString();
        }
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        //here we check that this page is updated by other user before save of current user 
        //this code is created by jitendra upadhyay on 02-06-2015
        //code start
        if (hdnSInquiryId.Value != "0")
        {
            DataTable dtSInquiryEdit = objSInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue.ToString(), hdnSInquiryId.Value);

            if (dtSInquiryEdit.Rows.Count != 0)
            {
                ViewState["TimeStamp"] = dtSInquiryEdit.Rows[0]["Row_Lock_Id"].ToString();

                if (ViewState["TimeStamp"].ToString() != dtSInquiryEdit.Rows[0]["Row_Lock_Id"].ToString())
                {

                    DisplayMessage("Another User update Information reload and try again");
                    btnSInquirySave.Enabled = true;
                    return;

                }
            }
        }
        //code end




        string SalesInquiryId = string.Empty;
        string strCustomerId = string.Empty;
        string strContactId = string.Empty;
        string strReceivedEmployeeId = string.Empty;
        string strHandledEmployeeId = string.Empty;
        string strBuyingPriority = string.Empty;
        string strSendMail = string.Empty;
        string strPost = string.Empty;

        if (txtOppoName.Text.Trim() == "")
        {
            DisplayMessage("Enter Opportunity Name");
            txtOppoName.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }

        if (txtInquiryDate.Text == "")
        {
            DisplayMessage("Enter Sales Opportunity date");
            txtInquiryDate.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }

        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        //modified on 2/4/2018
        //if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtInquiryDate.Text), "I"))
        //{
        //    DisplayMessage("Log In Financial year not allowing to perform this action");
        //    return;
        //}

        if (ddlInquiryType.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Opportunity Type");
            ddlInquiryType.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            if (ddlInquiryType.SelectedIndex == 2 || ddlInquiryType.SelectedIndex == 3)
            {
                ddlInquiryType_SelectedIndexChanged(null, null);
                if (txtTenderDate.Text == "")
                {
                    DisplayMessage("Please Enter Sales Tender Date");
                    txtTenderDate.Focus();
                    btnSInquirySave.Enabled = true;
                    return;
                }
            }
        }

        if (txtTenderDate.Text == "")
        {
            txtTenderDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        //end

        if (txtInquiryNo.Text == "")
        {
            DisplayMessage("Enter Opportunity No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryNo);
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            if (hdnSInquiryId.Value == "0")
            {
                string sql = "select count(*) from Inv_SalesInquiryHeader where company_id='" + Session["CompId"].ToString() + "' and brand_id='" + Session["BrandId"].ToString() + "' and location_id='" + ddlLocation.SelectedValue.ToString() + "' and SInquiryNo='" + txtInquiryNo.Text + "'";
                int recCount = 0;
                int.TryParse(objDa.get_SingleValue(sql).ToString(), out recCount);
                // DataTable dtInquiryNo = objSInquiryHeader.GetSIHeaderAllBySInquiryNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text);
                if (recCount > 0)
                {
                    DisplayMessage("Sales Opportunity No. Already Exits");
                    txtInquiryNo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInquiryNo);
                    btnSInquirySave.Enabled = true;
                    return;
                }
            }
        }
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Fill Customer Name");
            txtCustomerName.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            //strCustomerId = objContact.GetContactIdByContactName(txtCustomerName.Text.Split('/')[0]);
            strCustomerId = txtCustomerName.Text.Split('/')[3];
            if (strCustomerId == "" || strCustomerId == "0")
            {
                DisplayMessage("Select Customer Name In Suggestions Only");
                txtCustomerName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
                btnSInquirySave.Enabled = true;
                return;
            }
        }
        if (txtContactList.Text == "")
        {
            DisplayMessage("Select Contact Name");
            txtContactList.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            strContactId = objContact.GetContactIdByContactName(txtContactList.Text.Split('/')[0]);
            if (strContactId == "" || strContactId == "0")
            {
                DisplayMessage("Select Contact Name In Suggestions Only");
                txtContactList.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtContactList);
                btnSInquirySave.Enabled = true;
                return;
            }
        }
        if (ddlCurrency.SelectedValue == "--Select--")
        {
            DisplayMessage("Currency Required On Company Level");
            ddlCurrency.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        if (ddlCurrency.SelectedValue == "")
        {
            FillCurrency();
            DisplayMessage("Currency Required On Company Level");
            ddlCurrency.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        if (ddlInquiryType.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Opportunity Type");
            ddlInquiryType.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        if (txtOrderCompletionDate.Text == "")
        {
            DisplayMessage("Enter Order Close Date");
            txtOrderCompletionDate.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }

        if (txtReceivedEmp.Text == "")
        {
            DisplayMessage("Enter Received Employee Name");
            txtReceivedEmp.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtReceivedEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

            //strReceivedEmployeeId = GetEmployeeId(txtReceivedEmp.Text);
            strReceivedEmployeeId = Emp_ID;
            if (strReceivedEmployeeId == "" || strReceivedEmployeeId == "0")
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtReceivedEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReceivedEmp);
                btnSInquirySave.Enabled = true;
                return;
            }
        }

        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Enter Handled Employee Name");
            txtHandledEmp.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strHandledEmployeeId = Emp_ID;
            if (strHandledEmployeeId == "" || strHandledEmployeeId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtHandledEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmp);
                btnSInquirySave.Enabled = true;
                return;
            }
        }

        if (ddlBuyingPriority.SelectedIndex == 0)
        {
            DisplayMessage("Select Buying Priority");

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlBuyingPriority);
            btnSInquirySave.Enabled = true;
            return;
        }
        else
        {
            strBuyingPriority = ddlBuyingPriority.SelectedValue;
        }
        if (chkSendMail.Checked == true)
        {
            strSendMail = "True";
        }
        else
        {
            strSendMail = "False";
        }

        strPost = "False";

        string InquiryStatus = string.Empty;
        InquiryStatus = "Send Inquiry To Supplier";

        if (txtOpportunityAmt.Text == "")
        {
            DisplayMessage("Enter Opportunity Amount");
            txtOpportunityAmt.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }
        int parsedValue;
        float parseddecimal;

        if (!int.TryParse(txtOpportunityAmt.Text, out parsedValue))
        {
            if (!float.TryParse(txtOpportunityAmt.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                txtOpportunityAmt.Text = "";
                txtOpportunityAmt.Focus();
                btnSInquirySave.Enabled = true;
                return;
            }
        }

        if (txtProbability.Text.Trim() == "")
        {
            DisplayMessage("Must Enter Probability");
            txtProbability.Focus();
            btnSInquirySave.Enabled = true;
            return;
        }



        string SalesLeadID;
        if (Request.QueryString["SalesLeadID"] != null)
        {
            SalesLeadID = Request.QueryString["SalesLeadID"].ToString();
        }
        else
        {
            SalesLeadID = "0";
        }

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            int b = 0;
            if (hdnSInquiryId.Value != "0")
            {
                string id = "0";
                if (txtCampaignID.Text.Trim() != "")
                {
                    int start_pos = txtCampaignID.Text.LastIndexOf("/") + 1;
                    int last_pos = txtCampaignID.Text.Length;
                    id = txtCampaignID.Text.Substring(start_pos, last_pos - start_pos);
                }

                b = objSInquiryHeader.UpdateSIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, hdnSInquiryId.Value, txtInquiryNo.Text, ObjSysParam.getDateForInput(txtInquiryDate.Text).ToString(), txtTenderNo.Text.Trim(), ObjSysParam.getDateForInput(txtTenderDate.Text).ToString(), ddlInquiryType.SelectedValue, ObjSysParam.getDateForInput(txtOrderCompletionDate.Text).ToString(), strCustomerId, ddlCurrency.SelectedValue, txtRemark.Text, strReceivedEmployeeId, strHandledEmployeeId, strBuyingPriority, strSendMail, txtCondition1.Content, "", "", "", "", strPost, InquiryStatus.ToString(), strContactId, hdnCalllogsId.Value, ddlAccessType.SelectedItem.ToString(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), txtOppoName.Text, ddlLeadSource.SelectedItem.ToString(), id, ddlSalesStage.SelectedItem.ToString(), txtOpportunityAmt.Text, txtProbability.Text, SalesLeadID, ref trns);

                //Add Detail Section.

                if (GvProduct.Rows.Count > 0)
                {
                    objSInquiryDetail.DeleteSIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, hdnSInquiryId.Value, ref trns);

                    foreach (GridViewRow gvr in GvProduct.Rows)
                    {
                        Label lblSerialNo = (Label)gvr.FindControl("lblSNo");
                        Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
                        Label lblUnitId = (Label)gvr.FindControl("lblgvUnitId");
                        Label lblProductDescription = (Label)gvr.FindControl("lblgvProductDescription");
                        Label lblCurrencyId = (Label)gvr.FindControl("lblgvCurrencyId");
                        TextBox txtgvEstimatedUnitPrice = (TextBox)gvr.FindControl("txtgvEstimatedUnitPrice");
                        HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                        Label lblgvProductName = (Label)gvr.FindControl("lblgvProductName");
                        Label lblRequiredQty = (Label)gvr.FindControl("lblgvRequiredQty");
                        TextBox txtgvFrequency = (TextBox)gvr.FindControl("txtgvFrequency");
                        if (txtgvFrequency.Text == "")
                            txtgvFrequency.Text = "0";

                        if (txtgvEstimatedUnitPrice.Text == "")
                        {
                            txtgvEstimatedUnitPrice.Text = "0";
                        }
                        if (txtgvFrequency.Text == "" || txtgvFrequency.Text == null)
                        {
                            txtgvFrequency.Text = "0";
                        }
                        objSInquiryDetail.InsertSIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnLocationId.Value, hdnSInquiryId.Value, lblSerialNo.Text, lblProductId.Text, lblgvProductName.Text, lblUnitId.Text, lblProductDescription.Text, lblCurrencyId.Text, txtgvEstimatedUnitPrice.Text, lblRequiredQty.Text, "0", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtgvFrequency.Text, ref trns);
                    }

                }


                //End  

                if (b != 0)
                {
                    DisplayMessage("Record Updated", "green");
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {
                string id = "0";
                if (txtCampaignID.Text.Trim() != "")
                {
                    int start_pos = txtCampaignID.Text.LastIndexOf("/") + 1;
                    int last_pos = txtCampaignID.Text.Length;
                    id = txtCampaignID.Text.Substring(start_pos, last_pos - start_pos);
                }


                b = objSInquiryHeader.InsertSIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text, ObjSysParam.getDateForInput(txtInquiryDate.Text).ToString(), txtTenderNo.Text.Trim(), ObjSysParam.getDateForInput(txtTenderDate.Text).ToString(), ddlInquiryType.SelectedValue, ObjSysParam.getDateForInput(txtOrderCompletionDate.Text).ToString(), strCustomerId, ddlCurrency.SelectedValue, txtRemark.Text, strReceivedEmployeeId, strHandledEmployeeId, strBuyingPriority, strSendMail, txtCondition1.Content, "", "", "", "", strPost, InquiryStatus.ToString(), strContactId, hdnCalllogsId.Value, ddlAccessType.SelectedItem.ToString(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtOppoName.Text, ddlLeadSource.SelectedItem.ToString(), id, ddlSalesStage.SelectedItem.ToString(), txtOpportunityAmt.Text, txtProbability.Text, SalesLeadID, ref trns);

                if (txtCallNo.Text.Trim() != "")
                {
                    objCalllogs.updateCallStatusByCallNo("Close", txtCallNo.Text, Session["UserId"].ToString());
                }

                if (SalesLeadID != "0")
                {
                    DataTable dtAllData = SLClass.getActiveLeadDataById(SalesLeadID);
                    string date1 = GetDate(dtAllData.Rows[0]["Lead_date"].ToString());
                    string leadStatus = "";

                    if (dtAllData.Rows[0]["Lead_status"].ToString() == "" || dtAllData.Rows[0]["Lead_status"].ToString() == "New")
                    {
                        leadStatus = "In Process";
                        SLClass.UpdateLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtAllData.Rows[0]["Lead_no"].ToString(), ObjSysParam.getDateForInput(date1).ToString(), dtAllData.Rows[0]["Customer_Id"].ToString(), dtAllData.Rows[0]["Contact_Id"].ToString(), dtAllData.Rows[0]["Title"].ToString(), dtAllData.Rows[0]["Lead_source"].ToString(), leadStatus, dtAllData.Rows[0]["Currency_ID"].ToString(), dtAllData.Rows[0]["Opportunity_amount"].ToString(), dtAllData.Rows[0]["Source_description"].ToString(), dtAllData.Rows[0]["Status_description"].ToString(), dtAllData.Rows[0]["Remark"].ToString(), dtAllData.Rows[0]["Generated_by"].ToString(), dtAllData.Rows[0]["Assign_to"].ToString(), dtAllData.Rows[0]["Campaign_Id"].ToString(), dtAllData.Rows[0]["Refered_by"].ToString(), Session["EmpID"].ToString(), Session["EmpID"].ToString());
                        //DisplayMessage("Record Updated Successfully", "green");
                    }

                }

                string strMaxId = string.Empty;
                //DataTable dtMaxId = objSInquiryHeader.GetMaxSalesInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (b != 0)
                {
                    strMaxId = b.ToString();
                    SalesInquiryId = strMaxId;
                    if (txtInquiryNo.Text == ViewState["DocNo"].ToString())
                    {

                        DataTable dtCount = objSInquiryHeader.GetSIHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                        if (dtCount.Rows.Count == 0)
                        {
                            objSInquiryHeader.Updatecode(b.ToString(), txtInquiryNo.Text + "1", ref trns);
                            txtInquiryNo.Text = txtInquiryNo.Text + "1";
                        }
                        else
                        {
                            objSInquiryHeader.Updatecode(b.ToString(), txtInquiryNo.Text + dtCount.Rows.Count, ref trns);
                            txtInquiryNo.Text = txtInquiryNo.Text + dtCount.Rows.Count;
                        }

                    }
                    //Add Detail Section.

                    if (GvProduct.Rows.Count > 0)
                    {
                        objSInquiryDetail.DeleteSIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, ref trns);
                        foreach (GridViewRow gvr in GvProduct.Rows)
                        {
                            Label lblSerialNo = (Label)gvr.FindControl("lblSNo");
                            Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
                            Label lblUnitId = (Label)gvr.FindControl("lblgvUnitId");
                            Label lblProductDescription = (Label)gvr.FindControl("lblgvProductDescription");
                            Label lblCurrencyId = (Label)gvr.FindControl("lblgvCurrencyId");
                            TextBox txtgvEstimatedUnitPrice = (TextBox)gvr.FindControl("txtgvEstimatedUnitPrice");
                            Label lblgvProductName = (Label)gvr.FindControl("lblgvProductName");
                            Label lblRequiredQty = (Label)gvr.FindControl("lblgvRequiredQty");
                            HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                            TextBox txtgvFrequency = (TextBox)gvr.FindControl("txtgvFrequency");
                            if (txtgvFrequency.Text == "")
                                txtgvFrequency.Text = "0";
                            if (txtgvEstimatedUnitPrice.Text == "")
                            {
                                txtgvEstimatedUnitPrice.Text = "0";
                            }
                            if (txtgvFrequency.Text == "" || txtgvFrequency.Text == null)
                            {
                                txtgvFrequency.Text = "0";
                            }
                            objSInquiryDetail.InsertSIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strMaxId, lblSerialNo.Text, lblProductId.Text, lblgvProductName.Text, lblUnitId.Text, lblProductDescription.Text, lblCurrencyId.Text, txtgvEstimatedUnitPrice.Text, lblRequiredQty.Text, "0", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtgvFrequency.Text, ref trns);
                        }


                    }

                    //End  
                }

                if (b != 0)
                {
                    DisplayMessage("Record Saved", "green");

                }
                else
                {
                    DisplayMessage("Record  Not Saved");
                }
                btnSInquirySave.Enabled = true;
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            fillGrid(1);
            //AllPageCode();
            if (GvProduct.Rows.Count > 0)
                if (SalesInquiryId != "")
                {
                    InquiryPrint(SalesInquiryId);
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
            btnSInquirySave.Enabled = true;
            return;
        }

        if (txtValue.Text.Trim() != "")
        {
            btnbindrpt_Click(null, null);
        }
        if (txtValueDate.Text.Trim() != "")
        {
            btnbindrpt_Click(null, null);
        }

    }

    #region Auto Complete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dtCon = ObjContactMaster.GetCustomerAsPerFilterText(prefixText))
        {
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
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetAllCampaignName(string prefixText, int count, string contextKey)
    {
        try
        {
            Campaign objCampaign = new Campaign(HttpContext.Current.Session["DBConnection"].ToString());
            using (DataTable dtCon = objCampaign.GetCampaignNameByPreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText))
            {
                string[] filterlist = new string[dtCon.Rows.Count];
                if (dtCon.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCon.Rows.Count; i++)
                    {
                        filterlist[i] = dtCon.Rows[i]["FilteredText"].ToString();
                    }
                }
                return filterlist;
            }
        }
        catch (Exception error)
        {

        }
        return null;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = string.Empty;
        if (HttpContext.Current.Session["ContactID"] != null)
        {
            id = HttpContext.Current.Session["ContactID"].ToString();
        }
        else
        {
            id = "0";
        }
        DataTable dt = ObjContactMaster.GetContactAsPerFilterText(prefixText, id);
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Filtertext"].ToString();
            }
            dt = null;
            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                //filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Field2"].ToString() + "/" + dtcon.Rows[i]["Field1"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            dtcon = null;
            return filterlistcon;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText.ToString()))
        {
            string[] txt = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_code"].ToString();
                }
            }
            return txt;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCallLogs(string prefixText, int count, string contextKey)
    {
        CallLogs call = new CallLogs(HttpContext.Current.Session["DBConnection"].ToString());
        string id = WebUserControl_Followup.getCustid();
        DataTable dt1 = call.GetCallLogsFor_CustomerID(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText);
        string[] txt = new string[dt1.Rows.Count];
        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                txt[i] = dt1.Rows[i]["filteredText"].ToString();
            }
        }
        dt1 = null;
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVisitLogs(string prefixText, int count, string contextKey)
    {
        SM_WorkOrder visit = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString());
        string id = WebUserControl_Followup.getCustid();
        DataTable dt1 = visit.GetWorkOrderNoPreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText);
        string[] txt = new string[dt1.Rows.Count];
        if (dt1.Rows.Count > 0)
        {
            for (int i = 0; i < dt1.Rows.Count; i++)
            {

                txt[i] = dt1.Rows[i]["Work_Order_No"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString();
            }
        }
        dt1 = null;
        return txt;
    }

    #endregion
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        ////AllPageCode();
    }
    protected void GvSalesInquiryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesInquiryBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilterSinquirybin"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInquiryBin, dt, "", "");

        dt = null;
        string temp = string.Empty;

        for (int i = 0; i < GvSalesInquiryBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvSalesInquiryBin.Rows[i].FindControl("lblgvInquiryId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        //AllPageCode();
    }
    protected void GvSalesInquiryBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["dtFilterSinquirybin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilterSinquirybin"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInquiryBin, dt, "", "");
        lblSelectedRecord.Text = "";
        dt = null;
        //AllPageCode();
    }
    public void FillGridBin()
    {
        //DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Location_Name='" + ddlLocation.SelectedItem + "'", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataTable();
        dt = objSInquiryHeader.GetSIHeaderAllFalse(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);

        if (ddlLocation.SelectedItem.Text == "All")
        {
            dt = new DataView(dt, locationCondition, "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlUser.SelectedValue != "--Select User--")
        {
            dt = new DataView(dt, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInquiryBin, dt, "", "");

        Session["dtSinquirybin"] = dt;
        Session["dtFilterSinquirybin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        dt = null;
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedItem.Value == "IDate" || ddlFieldNameBin.SelectedItem.Value == "OrderCompletionDate")
        {
            if (txtValueDateBin.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDateBin.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueDateBin.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDateBin.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDateBin);
                    return;
                }

            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDateBin.Focus();
                return;
            }
        }

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


            DataTable dtCust = (DataTable)Session["dtSinquirybin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilterSinquirybin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesInquiryBin, view.ToTable(), "", "");
            dtCust = null;
            view = null;
            lblSelectedRecord.Text = "";

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        //AllPageCode();
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueDateBin.Text != "")
            txtValueDateBin.Focus();
    }

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesInquiryBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSalesInquiryBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesInquiryBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvSalesInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvSalesInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvSalesInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString())
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
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvSalesInquiryBin.Rows[index].FindControl("lblgvInquiryId");
        if (((CheckBox)GvSalesInquiryBin.Rows[index].FindControl("chkSelect")).Checked)
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
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueDateBin.Visible = false;
        txtValueBin.Visible = true;
        txtValueDateBin.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        FillGridBin();
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtFilterSinquirybin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["SInquiryID"]))
                {
                    lblSelectedRecord.Text += dr["SInquiryID"] + ",";
                }
            }
            for (int i = 0; i < GvSalesInquiryBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvSalesInquiryBin.Rows[i].FindControl("lblgvInquiryId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSalesInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtFilterSinquirybin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesInquiryBin, dtUnit1, "", "");
            dtUnit1 = null;
            ViewState["Select"] = null;
        }
        dtPbrand = null;
        //AllPageCode();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        //DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Location_Name='" + ddlLocation.SelectedItem + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please Select One Location");
            return;
        }
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {

                    b = objSInquiryHeader.DeleteSIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            fillGrid(1);
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvSalesInquiryBin.Rows)
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
        //AllPageCode();
    }
    #endregion
    #endregion
    #region User defined Function
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
    private void FillGrid(string id)
    {
        DataTable dt = objSInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, id);

        if (dt != null && dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)GvSalesInquiry, dt, "", "");
        }
        else
        {
            GvSalesInquiry.DataSource = null;
            GvSalesInquiry.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        dt = null;
    }


    private void fillGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlFieldName.SelectedItem.Value == "IDate" || ddlFieldName.SelectedItem.Value == "OrderCompletionDate")
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName.SelectedValue + " Like '" + txtValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "isActive='true'";


        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }

        if (ddlUser.SelectedValue != "--Select User--")
        {
            // i am changing handledEmpId into user id and searching
            strWhereClause = strWhereClause + " and  User_Id='" + ddlUser.SelectedValue + "'";
        }
        else
        {
            bool isSingleUser = false;
            if (Session["EmpId"].ToString() != "0")
            {
                isSingleUser = bool.Parse(ObjUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString()));
            }

            if (isSingleUser == true)
            {
                string strAllUser = string.Empty;
                foreach (ListItem item in ddlUser.Items)
                {
                    if (item.Value == "--Select User--")
                    {
                        continue;
                    }
                    strAllUser += "," + item.Value.ToString();
                }
                if (!string.IsNullOrEmpty(strAllUser))
                {
                    strAllUser = strAllUser.Substring(1, strAllUser.Length - 1);
                    strWhereClause = strWhereClause + " and  case when User_Id = 'superadmin' then 0 else User_id end in(" + strAllUser + ")";
                }
            }
        }
        if (ddlSalesStageList.SelectedIndex != 0)
        {
            strWhereClause = strWhereClause + " and  Sales_Stage='" + ddlSalesStageList.SelectedValue.ToString() + "'";
        }
        if (ddlLocation.SelectedItem.Text != "All")
        {
            strWhereClause = strWhereClause + " and  Location_Id='" + ddlLocation.SelectedValue.ToString() + "'";
        }
        else
        {
            strWhereClause = strWhereClause + " and  Location_Id in (" + ddlLocation.SelectedValue.ToString() + ")";
        }

        int totalRows = 0;
        using (DataTable dt = objSInquiryHeader.getInqueryList((currentPageIndex - 1).ToString(), int.Parse(Session["GridSize"].ToString()).ToString(), GvSalesInquiry.Attributes["CurrentSortField"], GvSalesInquiry.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvSalesInquiry, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

            }
            else
            {
                GvSalesInquiry.DataSource = null;
                GvSalesInquiry.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }

            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }

    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSalesOpportunityCurrentPageIndex.Value = pageIndex.ToString();
        fillGrid(pageIndex);
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyListForDDL();
        if (dsCurrency.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
            objPageCmn.FillData((object)ddlPCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }
        dsCurrency = null;
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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        dtres = null;
        return ArebicMessage;
    }
    public void Reset()
    {
        //fillGrid(1);
        ddlCurrency.SelectedIndex = 0;
        txtInquiryNo.ReadOnly = false;
        txtInquiryDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtTenderNo.Text = "";
        txtTenderDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtOrderCompletionDate.Text = "";
        txtCustomerName.Text = "";
        txtOppoName.Text = "";
        txtOpportunityAmt.Text = "";
        txtCampaignID.Text = "";
        ddlSalesStage.SelectedIndex = 0;
        ddlLeadSource.SelectedIndex = 0;
        txtProbability.Text = "";
        ddlInquiryType.SelectedValue = "--Select--";
        txtRemark.Text = "";
        if (Session["EmpId"].ToString() != "0")
        {
            DataTable Dt_Temp = new DataTable();
            Dt_Temp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
            txtReceivedEmp.Text = Dt_Temp.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Temp.Rows[0]["Emp_Code"].ToString();
            //txtHandledEmp.Text = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Emp_Name"].ToString() + "/" + Session["EmpId"].ToString();
            txtHandledEmp.Text = Dt_Temp.Rows[0]["Emp_Name"].ToString() + "/" + Dt_Temp.Rows[0]["Emp_Code"].ToString();
        }
        else
        {
            txtReceivedEmp.Text = "";
            txtHandledEmp.Text = "";

        }
        hdnCalllogsId.Value = "0";
        ddlBuyingPriority.SelectedIndex = 2;
        ddlInquiryType.SelectedIndex = 2;
        txtContactList.Text = "";
        chkSendMail.Checked = false;
        txtCondition1.Content = "";
        hdnSquotationExist.Value = "0";
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        ChkSendInPurchase.Checked = false;
        hdnSInquiryId.Value = "0";
        hdnLocationId.Value = "0";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtInquiryNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            ddlCurrency.SelectedValue = strCurrencyId;
            ddlPCurrency.SelectedValue = strCurrencyId;
        }
        //cmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54").Rows[0]["Module_Id"].ToString(), "54");
        ViewState["Dtproduct"] = null;
        Session["DtSearchProduct"] = null;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        btnSInquirySave.Enabled = true;
        BtnReset.Visible = true;
        btnSinquirysaveandquotation.Enabled = true;
    }

    private void ResetSearchFields()
    {
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;

        txtValue.Text = "";
        txtValueBin.Text = "";

        txtValueDateBin.Text = "";
        txtValueDate.Text = "";

        txtValueDate.Visible = false;
        txtValue.Visible = true;
    }
    #endregion
    #region Invoice Section
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtCustomerName.Text != "")
        {
            strCustomerId = objContact.GetContactIdByContactName(txtCustomerName.Text.Split('/')[0]);
            if (strCustomerId != "" && strCustomerId != "0")
            {
                txtContactList.Text = "";
                txtContactList.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtCustomerName.Text = "";
                txtContactList.Text = "";
                Session["ContactID"] = null;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
            }
        }
        else
        {
            txtCustomerName.Text = "";
            txtContactList.Text = "";
            txtCustomerName.Focus();
            Session["ContactID"] = null;
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCustomerName);
        }

    }
    protected void txtContactList_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactList.Text != "")
        {
            strCustomerId = objContact.GetContactIdByContactName(txtContactList.Text.Split('/')[0]);
            if (strCustomerId != "" && strCustomerId != "0")
            {

                ddlCurrency.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtContactList.Text = "";
                txtContactList.Focus();
                return;

            }
        }

    }
    protected void txtReceivedEmp_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtReceivedEmp.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtReceivedEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

            //strEmployeeId = GetEmployeeId(txtReceivedEmp.Text);
            strEmployeeId = Emp_ID;
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtReceivedEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReceivedEmp);
            }
        }
        //AllPageCode();
    }
    protected void txtHandledEmp_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtHandledEmp.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);

            strEmployeeId = Emp_ID;
            if (strEmployeeId != "" && strEmployeeId != "0")
            {

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtHandledEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmp);
            }
        }
        //AllPageCode();
    }
    #endregion
    #region Add Product Concept
    private void FillUnit()
    {
        DataTable dsUnit = new DataTable();
        using (dsUnit = UM.GetUnitListforDDl(Session["CompId"].ToString()))
        {
            if (dsUnit.Rows.Count > 0)
            {
                objPageCmn.FillData((object)ddlUnit, dsUnit, "Unit_Name", "Unit_Id");
            }
        }
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Name"].ToString();
            }
            dtCName = null;
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }

    public void FillRelatedProduct(string ProductId)
    {
        DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId);
        if (dtRelatedProduct.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvRelatedProduct, dtRelatedProduct, "", "");
            GvRelatedProduct.Visible = true;
            foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
            {
                DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
                DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
                HiddenField hdnUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
                TextBox txtquantity = (TextBox)gvRow.FindControl("txtquantity");
                txtquantity.Text = "1";
                DataTable dsUnit = null;
                dsUnit = UM.GetUnitListforDDl(Session["CompId"].ToString());
                if (dsUnit.Rows.Count > 0)
                {
                    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)ddlgvUnit, dsUnit, "Unit_Name", "Unit_Id");
                    try
                    {
                        ddlgvUnit.SelectedValue = hdnUnitId.Value;
                    }
                    catch
                    {
                    }
                    dsUnit = null;
                }
                DataTable dsPCurrency = null;
                dsPCurrency = objCurrency.GetCurrencyListForDDL();
                if (dsPCurrency.Rows.Count > 0)
                {
                    //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)ddlgvCurrency, dsPCurrency, "Currency_Name", "Currency_ID");
                    try
                    {
                        ddlgvCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                    }
                    catch
                    {
                    }
                    dsPCurrency = null;
                }
                else
                {
                    ddlgvCurrency.Items.Insert(0, "--Select--");
                }
            }
        }
        else
        {
            GvRelatedProduct.DataSource = null;
            GvRelatedProduct.DataBind();
            hdnNewProductId.Value = "0";
            txtPDesc.Visible = true;
            pnlPDescription.Visible = false;
        }
        dtRelatedProduct = null;
    }
    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        string Description = string.Empty;
        string SuggestedProductName = string.Empty;
        if (txtProductName.Text != "")
        {
            if (hdnNewProductId.Value == "0")
            {
                if (txtProductcode.Text != "")
                {
                    string productid = objProductM.GetProductIdbyProductCode(txtProductcode.Text, Session["BrandId"].ToString());
                    if (productid != "")
                    {
                        hdnNewProductId.Value = productid;
                    }
                    else
                    {
                        hdnNewProductId.Value = "0";
                        SuggestedProductName = txtProductName.Text;
                    }
                }
                else
                {
                    hdnNewProductId.Value = "0";
                    SuggestedProductName = txtProductName.Text;
                }
            }

            if (ddlUnit.SelectedValue != "--Select--")
            {
                hdnUnitId.Value = ddlUnit.SelectedValue;
            }
            else
            {
                DisplayMessage("Select Unit Name");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlUnit);
                return;
            }
            if (txtRequiredQty.Text == "")
            {
                DisplayMessage("Enter Required Quantity");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
                return;
            }
            if (ddlPCurrency.SelectedValue != "--Select--")
            {
                hdnCurrencyId.Value = ddlPCurrency.SelectedValue;
            }
            else
            {
                DisplayMessage("Currency Required On Company Level");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlPCurrency);
                return;
            }

            if (txtEstimatedUnitPrice.Text != "")
            {
                float flTemp = 0;
                if (!float.TryParse(txtEstimatedUnitPrice.Text, out flTemp))
                {
                    txtEstimatedUnitPrice.Text = "";
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEstimatedUnitPrice);
                    return;
                }
            }
            else
            {
                txtEstimatedUnitPrice.Text = "0";
            }
            //foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
            //{
            //    CheckBox chk = (CheckBox)gvRow.FindControl("chk");
            //    DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
            //    DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
            //    HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
            //    TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
            //    Label txtgvProductdesc = (Label)gvRow.FindControl("Description");
            //    Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            //    Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            //    TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");
            //    TextBox txtGvFrequency = (TextBox)gvRow.FindControl("txtgvFrequency");

            //    if (chk.Checked == true)
            //    {
            //        foreach (GridViewRow gvDetailRow in GvProduct.Rows)
            //        {
            //            Label lblCheckProductId = (Label)gvDetailRow.FindControl("lblgvProductId");
            //            if (lblgvProductId.Text == lblCheckProductId.Text)
            //            {
            //                DisplayMessage("Related Product(" + lblgvProductName.Text + ") is Already Exists");
            //                chk.Checked = false;
            //                return;
            //            }
            //        }


            //        if (ddlgvUnit.SelectedIndex == 0)
            //        {
            //            DisplayMessage("Select Unit In Related Product List for product name=" + lblgvProductName.Text);
            //            ddlgvUnit.Focus();
            //            return;
            //        }
            //        if (txtgvquantity.Text.Trim() == "")
            //        {
            //            DisplayMessage("Enter Required Quantity In Related Product List for product name=" + lblgvProductName.Text);
            //            ddlgvUnit.Focus();
            //            return;
            //        }

            //        if (txtGvEstimatedUnitPrice.Text != "")
            //        {
            //            float flTemp = 0;
            //            if (!float.TryParse(txtGvEstimatedUnitPrice.Text, out flTemp))
            //            {
            //                txtGvEstimatedUnitPrice.Text = "";
            //                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Estimated Unit Price Should be numeric In Related Product List for product name=" + lblgvProductName.Text + "');", true);
            //                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGvEstimatedUnitPrice);
            //                return;
            //            }
            //        }
            //        else
            //        {
            //            txtGvEstimatedUnitPrice.Text = "0";
            //        }

            //        if (ddlgvCurrency.SelectedIndex == 0)
            //        {
            //            DisplayMessage("Select Currency In Related Product List for product name=" + lblgvProductName.Text);
            //            ddlgvUnit.Focus();
            //            return;
            //        }
            //    }
            //}

            if (hdnProductId.Value == "")
            {
                FillProductChidGird("Save");
                //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
            }
            else
            {
                if (txtProductName.Text == hdnProductName.Value)
                {
                    FillProductChidGird("Edit");
                    //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                }
                else
                {
                    FillProductChidGird("Edit");
                    //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                }
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
        txtEstimatedUnitPrice_TextChanged(sender, e);
        txtPDesc.Text = "";
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Add_Product_Info.Attributes.Add("Class", "box box-primary");
        txtProductcode.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", " SetFocus()", true);
    }
    public void ResetProduct()
    {
        txtFrequency.Text = "";
        txtProductName.Text = "";
        txtPDescription.Text = "";
        txtRequiredQty.Text = "1";
        //FillProductCurrency();

        if (ddlCurrency.SelectedIndex != 0)
        {
            try
            {
                ddlPCurrency.SelectedValue = ddlCurrency.SelectedValue;
            }
            catch
            {
                ddlPCurrency.SelectedValue = strCurrencyId;
            }
        }

        txtEstimatedUnitPrice.Text = "";
        hdnProductId.Value = "";
        hdnProductName.Value = "";
        hdnNewProductId.Value = "0";
        txtPDesc.Text = "";
        txtProductcode.Text = "";
        txtProductcode.Focus();

        GvRelatedProduct.DataSource = null;
        GvRelatedProduct.DataBind();

    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("DaysFrequency");
        return dt;
    }
    public DataTable FillProductDataTabel()
    {
        string strNewSNo = string.Empty;


        DataTable dt = CreateProductDataTable();
        if (GvProduct.Rows.Count > 0)
        {
            for (int i = 0; i < GvProduct.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvProduct.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
                    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                    Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
                    Label lblgvPDescription = (Label)GvProduct.Rows[i].FindControl("lblgvProductDescription");
                    Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
                    Label lblgvCurrencyId = (Label)GvProduct.Rows[i].FindControl("lblgvCurrencyId");
                    TextBox txtgvEstimatedUnitPrice = (TextBox)GvProduct.Rows[i].FindControl("txtgvEstimatedUnitPrice");
                    HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
                    LinkButton lnkstockqty = (LinkButton)GvProduct.Rows[i].FindControl("lnkStockInfo");
                    Label lblgvfreq = (Label)GvProduct.Rows[i].FindControl("lblgvFrequency");

                    dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
                    strNewSNo = lblgvSNo.Text;
                    dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
                    dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
                    dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
                    dt.Rows[i]["Quantity"] = lblgvReqQty.Text;
                    dt.Rows[i]["Currency_Id"] = lblgvCurrencyId.Text;
                    dt.Rows[i]["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
                    dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
                    dt.Rows[i]["DaysFrequency"] = lblgvfreq.Text;
                    try
                    {
                        dt.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
                    }
                    catch
                    {
                        dt.Rows[i]["Sysqty"] = "0";

                    }

                }
                else
                {
                    DataTable DtMaxSerial = new DataTable();
                    try
                    {

                        DtMaxSerial.Merge(dt);
                        DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                    dt.Rows.Add(i);
                    if (dt.Rows.Count > 0)
                    {


                        dt.Rows[i]["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                    }
                    else
                    {
                        dt.Rows[i]["Serial_No"] = "1";

                    }
                    dt.Rows[i]["Product_Id"] = hdnNewProductId.Value;
                    dt.Rows[i]["UnitId"] = hdnUnitId.Value;
                    if (hdnNewProductId.Value == "0")
                    {
                        dt.Rows[i]["ProductDescription"] = txtPDesc.Text;
                        dt.Rows[i]["SuggestedProductName"] = txtProductName.Text;
                    }
                    else
                    {
                        dt.Rows[i]["ProductDescription"] = txtPDescription.Text;

                        dt.Rows[i]["SuggestedProductName"] = txtProductName.Text;

                    }
                    dt.Rows[i]["Quantity"] = txtRequiredQty.Text;
                    dt.Rows[i]["Currency_Id"] = hdnCurrencyId.Value;

                    dt.Rows[i]["EstimatedUnitPrice"] = txtEstimatedUnitPrice.Text;
                    dt.Rows[i]["DaysFrequency"] = txtFrequency.Text;
                    try
                    {
                        dt.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), hdnNewProductId.Value).Rows[0]["Quantity"].ToString();
                    }
                    catch
                    {
                        dt.Rows[i]["Sysqty"] = "0";

                    }

                    foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
                    {
                        CheckBox chk = (CheckBox)gvRow.FindControl("chk");
                        DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
                        DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
                        HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
                        TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
                        Label txtgvProductdesc = (Label)gvRow.FindControl("lblgvProductDescription");
                        Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
                        Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
                        TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");
                        TextBox lblgvfreq = (TextBox)gvRow.FindControl("txtFrequency");

                        if (chk.Checked == true)
                        {

                            try
                            {

                                DtMaxSerial.Merge(dt);
                                DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
                            }
                            catch
                            {
                            }
                            DataRow dr = dt.NewRow();



                            try
                            {
                                dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                            }
                            catch
                            {
                                dr["Serial_No"] = "1";
                            }

                            dr["Product_Id"] = lblgvProductId.Text;
                            dr["UnitId"] = ddlgvUnit.SelectedValue;
                            if (hdnNewProductId.Value == "0")
                            {
                                dr["ProductDescription"] = txtgvProductdesc.Text;
                                dr["SuggestedProductName"] = lblgvProductName.Text;
                            }
                            else
                            {
                                dr["ProductDescription"] = txtgvProductdesc.Text;

                                dr["SuggestedProductName"] = lblgvProductName.Text;

                            }
                            dr["Quantity"] = txtgvquantity.Text;
                            dr["Currency_Id"] = ddlgvCurrency.SelectedValue;
                            dr["EstimatedUnitPrice"] = txtGvEstimatedUnitPrice.Text;
                            dr["DaysFrequency"] = lblgvfreq.Text;
                            try
                            {
                                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
                            }
                            catch
                            {
                                dr["Sysqty"] = "0";

                            }
                            dt.Rows.Add(dr);
                        }
                    }
                }
            }
        }
        else
        {

            dt.Rows.Add(0);
            dt.Rows[0]["Serial_No"] = "1";
            dt.Rows[0]["Product_Id"] = hdnNewProductId.Value;
            dt.Rows[0]["UnitId"] = hdnUnitId.Value;
            if (hdnNewProductId.Value == "0")
            {
                dt.Rows[0]["ProductDescription"] = txtPDesc.Text;
                dt.Rows[0]["SuggestedProductName"] = txtProductName.Text;
            }
            else
            {
                dt.Rows[0]["ProductDescription"] = txtPDescription.Text;

                dt.Rows[0]["SuggestedProductName"] = txtProductName.Text;
            }
            dt.Rows[0]["Quantity"] = txtRequiredQty.Text;
            dt.Rows[0]["Currency_Id"] = hdnCurrencyId.Value;
            dt.Rows[0]["EstimatedUnitPrice"] = txtEstimatedUnitPrice.Text;
            dt.Rows[0]["DaysFrequency"] = txtFrequency.Text;
            try
            {
                dt.Rows[0]["SysQty"] = objDa.get_SingleValue("SELECT Inv_StockDetail.Quantity FROM Inv_StockDetail WHERE Company_Id = '" + Session["CompId"].ToString() + "'    AND Brand_Id = '" + Session["BrandId"].ToString() + "'   AND Location_Id = '" + Session["LocId"].ToString() + "'    AND ProductId = '" + hdnNewProductId.Value + "' AND Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "'");
                dt.Rows[0]["SysQty"] = dt.Rows[0]["SysQty"].ToString() == "@NOTFOUND@" ? "0" : dt.Rows[0]["SysQty"].ToString();
            }
            catch
            {
                dt.Rows[0]["Sysqty"] = "0";

            }

            foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
            {
                CheckBox chk = (CheckBox)gvRow.FindControl("chk");
                DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
                DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
                HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
                TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
                Label txtgvProductdesc = (Label)gvRow.FindControl("lblgvProductDescription");
                Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
                Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
                TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");
                TextBox lblgvfreq = (TextBox)gvRow.FindControl("txtFrequency");

                if (chk.Checked == true)
                {
                    DataTable DtMaxSerial = new DataTable();
                    try
                    {

                        DtMaxSerial.Merge(dt);
                        DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    DataRow dr = dt.NewRow();


                    try
                    {

                        dr["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                    }
                    catch
                    {
                        dr["Serial_No"] = "1";

                    }

                    dr["Product_Id"] = lblgvProductId.Text;
                    dr["UnitId"] = ddlgvUnit.SelectedValue;
                    if (hdnNewProductId.Value == "0")
                    {
                        dr["ProductDescription"] = txtgvProductdesc.Text;
                        dr["SuggestedProductName"] = lblgvProductName.Text;
                    }
                    else
                    {
                        dr["ProductDescription"] = txtgvProductdesc.Text;

                        dr["SuggestedProductName"] = lblgvProductName.Text;

                    }
                    dr["Quantity"] = txtgvquantity.Text;
                    dr["Currency_Id"] = ddlgvCurrency.SelectedValue;
                    dr["EstimatedUnitPrice"] = txtGvEstimatedUnitPrice.Text;
                    dr["DaysFrequency"] = lblgvfreq.Text;
                    try
                    {
                        dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
                        dr["Sysqty"] = dr["Sysqty"].ToString() == "@NOTFOUNT@" ? "0" : dr["Sysqty"].ToString();
                    }
                    catch
                    {
                        dr["Sysqty"] = "0";
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            ViewState["Dtproduct"] = (DataTable)dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //objPageCmn.FillData((object)GvProduct, dt, "", "");
            //try
            //{
            //    GvProduct.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Estimated Unit Price");
            //}
            //catch
            //{
            //}
        }
        return dt;
    }
    public DataTable FillProductDataTabelDelete()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)GvProduct.Rows[i].FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)GvProduct.Rows[i].FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)GvProduct.Rows[i].FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
            Label lblgvfreq = (Label)GvProduct.Rows[i].FindControl("lblgvFrequency");

            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
            dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
            dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
            dt.Rows[i]["Quantity"] = lblgvReqQty.Text;
            dt.Rows[i]["Currency_Id"] = lblgvCurrencyId.Text;
            dt.Rows[i]["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows[i]["DaysFrequency"] = lblgvfreq.Text;
            try
            {
                dt.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dt.Rows[i]["Sysqty"] = "0";

            }
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No<>'" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        ViewState["Dtproduct"] = dt;
        return dt;
    }
    protected void imgBtnProductEdit_Command(object sender, CommandEventArgs e)
    {
        //Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        //Div_Add_Product.Attributes.Add("Class", "box box-primary");

        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductDataTabelEdit();
        pnlProduct1.Visible = true;
        GvRelatedProduct.DataSource = null;
        GvRelatedProduct.DataBind();
        txtProductcode.Focus();
    }
    public DataTable FillProductDataTabelEdit()
    {
        DataTable dt = CreateProductDataTable();

        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)GvProduct.Rows[i].FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)GvProduct.Rows[i].FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)GvProduct.Rows[i].FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
            Label lblgvfreq = (Label)GvProduct.Rows[i].FindControl("lblgvFrequency");

            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
            dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
            dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
            dt.Rows[i]["Quantity"] = lblgvReqQty.Text;
            dt.Rows[i]["Currency_Id"] = lblgvCurrencyId.Text;
            dt.Rows[i]["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows[i]["DaysFrequency"] = lblgvfreq.Text;
            try
            {
                dt.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dt.Rows[i]["Sysqty"] = "0";

            }
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No='" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            if (dt.Rows[0]["Product_Id"].ToString() != "0")
            {
                txtProductName.Text = objProductM.GetProductNamebyProductId(dt.Rows[0]["Product_Id"].ToString());
                txtProductcode.Text = objProductM.GetProductCodebyProductId(dt.Rows[0]["Product_Id"].ToString());
                txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
                pnlPDescription.Visible = true;
                txtPDesc.Visible = false;
            }
            else
            {
                txtProductName.Text = dt.Rows[0]["SuggestedProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["SuggestedProductName"].ToString();
                txtPDesc.Text = dt.Rows[0]["ProductDescription"].ToString();
                pnlPDescription.Visible = false;
                txtPDesc.Visible = true;
            }


            txtRequiredQty.Text = dt.Rows[0]["Quantity"].ToString();
            ddlPCurrency.SelectedValue = dt.Rows[0]["Currency_Id"].ToString();
            txtEstimatedUnitPrice.Text = dt.Rows[0]["EstimatedUnitPrice"].ToString();
            hdnProductName.Value = objProductM.GetProductNamebyProductId(dt.Rows[0]["Product_Id"].ToString());
            txtFrequency.Text = dt.Rows[0]["DaysFrequency"].ToString();
        }
        return dt;
    }
    protected void imgBtnProductDelete_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductChidGird("Del");
        txtEstimatedUnitPrice_TextChanged(sender, null);
    }
    public void FillProductChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillProductDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillProductDataTableUpdate();
        }
        else
        {
            dt = FillProductDataTabel();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProduct, dt, "", "");
        GvProduct.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Estimated Unit Price", Session["DBConnection"].ToString());
        dt = null;
        ResetProduct();
        //AllPageCode();
    }
    public DataTable FillProductDataTableUpdate()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)GvProduct.Rows[i].FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)GvProduct.Rows[i].FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)GvProduct.Rows[i].FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
            Label lblgvfreq = (Label)GvProduct.Rows[i].FindControl("lblgvFrequency");

            dt.Rows[i]["Serial_No"] = lblSNo.Text;
            dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
            dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
            dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
            dt.Rows[i]["Quantity"] = lblgvReqQty.Text;
            dt.Rows[i]["Currency_Id"] = lblgvCurrencyId.Text;
            dt.Rows[i]["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows[i]["DaysFrequency"] = lblgvfreq.Text;
            try
            {
                dt.Rows[i]["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dt.Rows[i]["Sysqty"] = "0";

            }
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnProductId.Value == dt.Rows[i]["Serial_No"].ToString())
            {
                dt.Rows[i]["Product_Id"] = hdnNewProductId.Value;
                dt.Rows[i]["UnitId"] = hdnUnitId.Value;
                if (hdnNewProductId.Value == "0")
                {
                    dt.Rows[i]["ProductDescription"] = txtPDesc.Text;
                    dt.Rows[i]["SuggestedProductName"] = txtProductName.Text;

                }
                else
                {
                    dt.Rows[i]["ProductDescription"] = txtPDescription.Text;
                    dt.Rows[i]["SuggestedProductName"] = txtProductName.Text;
                }
                dt.Rows[i]["Quantity"] = txtRequiredQty.Text;
                dt.Rows[i]["Currency_Id"] = hdnCurrencyId.Value;
                dt.Rows[i]["EstimatedUnitPrice"] = txtEstimatedUnitPrice.Text;
                dt.Rows[i]["DaysFrequency"] = txtFrequency.Text;
            }
        }
        ViewState["Dtproduct"] = (DataTable)dt;

        return dt;
    }
    #endregion
    protected void ChkSendInPurchase_CheckedChanged(object sender, EventArgs e)
    {

        ChkSendInPurchase.Checked = false;

    }

    #region View

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);

    }

    #endregion
    protected void lnkProductBulider_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../Inventory/ProductBuilderPopUp.aspx','window','width=1024,')", true);

    }
    public string GetAmountDecimal(string Amount)
    {
        if (ddlCurrency.SelectedIndex == 0)
        {
            return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);
        }
        else
        {
            return ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, Amount);
        }
    }
    protected void ddlUser_Click(object sender, EventArgs e)
    {

        ddlPermission.SelectedIndex = 0;
        fillGrid(1);
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //FillGridBin();
        //AllPageCode();
    }
    #region InquiryPrint

    public void InquiryPrint(string InquiryId)
    {
        string url = Server.MapPath("SalesInquiryPrint.aspx");

        string strCmd = string.Format("window.open('../Sales/SalesInquiryPrint.aspx?Id=" + InquiryId.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        InquiryPrint(e.CommandArgument.ToString());
    }
    #endregion
    #region Adavancesearch
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        DataTable dt = CreateProductDataTable();

        foreach (GridViewRow gvRow in GvProduct.Rows)
        {

            DataRow dr = dt.NewRow();
            Label lblSNo = (Label)gvRow.FindControl("lblSNo");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)gvRow.FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)gvRow.FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)gvRow.FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)gvRow.FindControl("hdnSuggestedProductName");
            TextBox txtgvFrequency = (TextBox)gvRow.FindControl("txtgvFrequency");

            dr["Serial_No"] = lblSNo.Text;
            dr["Product_Id"] = lblgvProductId.Text;
            dr["UnitId"] = lblgvUnitId.Text;
            dr["ProductDescription"] = lblgvPDescription.Text;
            dr["Quantity"] = lblgvReqQty.Text;
            dr["Currency_Id"] = lblgvCurrencyId.Text;
            dr["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dr["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dr["DaysFrequency"] = txtgvFrequency.Text;

            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";

            }
            dt.Rows.Add(dr);
        }

        ViewState["Dtproduct"] = dt;


        Session["DtSearchProduct"] = ViewState["Dtproduct"];

        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=SI&&CurId=" + ddlCurrency.SelectedValue + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        GvProduct.Visible = true;
        if (Session["DtSearchProduct"] != null)
        {
            ViewState["Dtproduct"] = Session["DtSearchProduct"];
            if (ViewState["Dtproduct"] != null)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, (DataTable)ViewState["Dtproduct"], "", "");

            }
            Session["DtSearchProduct"] = null;
        }
        else
        {

            if (ViewState["Dtproduct"] != null)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, (DataTable)ViewState["Dtproduct"], "", "");
            }
            DisplayMessage("Product Not Found");
            return;

        }
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        ResetProduct();
        divAdvanceSearch.Visible = false;
        divProductBuilder.Visible = false;
        pnlProduct1.Visible = false;
        divLabelBuilder.Visible = false;
        if (rbtnFormView.Checked == true)
        {
            pnlProduct1.Visible = true;
            Session["DtSearchProduct"] = null;
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            divAdvanceSearch.Visible = true;
            pnlProduct1.Visible = false;
        }
        if (rbtnProductBuilder.Checked)
        {
            divProductBuilder.Visible = true;
            Session["DtSearchProduct"] = null;
        }
        if (rbtnLabelBuilder.Checked)
        {
            divLabelBuilder.Visible = true;
            Session["DtSearchProduct"] = null;
        }
    }
    #endregion
    #region AddCustomer
    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (Session["EmpId"].ToString() == "0")
        {
            allow = true;
        }
        using (DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "107", "19", Session["CompId"].ToString()))
        {
            if (dtAllPageCode.Rows.Count != 0)
            {
                allow = true;
            }
        }
        return allow;
    }

    #endregion
    #region Request
    protected void btnbindRequest_Click(object sender, EventArgs e)
    {


        if (ddlOptionRequest.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOptionRequest.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String)='" + txtValueRequest.Text.Trim() + "'";
            }
            else if (ddlOptionRequest.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String) like '%" + txtValueRequest.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String) Like '%" + txtValueRequest.Text.Trim() + "%'";
            }

            DataTable dtPurchaseRequest = (DataTable)Session["dtPRequest"];


            DataView view = new DataView(dtPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtPRequest"] = view.ToTable();
            lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCallRequest, view.ToTable(), "", "");

            //AllPageCode();



        }
        txtValueRequest.Focus();
    }
    protected void btnRefreshRequest_Click(object sender, EventArgs e)
    {
        FillRequestGrid();
        ddlFieldNameRequest.SelectedIndex = 0;
        ddlOptionRequest.SelectedIndex = 2;
        txtValueRequest.Text = "";
        txtValueRequest.Visible = true;
        txtValueRequest.Focus();
    }
    protected void btncustomerinquiry_Click(object sender, EventArgs e)
    {
        FillRequestGrid();
        //AllPageCode();
    }
    protected void GvCallRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCallRequest.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPRequest"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCallRequest, dt, "", "");
        dt = null;
        //AllPageCode();
    }
    protected void GvCallRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPRequest"];
        string sortdir = "DESC";
        if (ViewState["PRSortDir"] != null)
        {
            sortdir = ViewState["PRSortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["PRSortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["PRSortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["PRSortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["PRSortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtPRequest"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCallRequest, dt, "", "");
        dt = null;
        //AllPageCode();
    }
    private void FillRequestGrid()
    {
        DataTable DtPrequest = objCalllogs.GetCallLogsFor_salesinquiry(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (DtPrequest != null && DtPrequest.Rows.Count > 0)
        {
            if (DtPrequest.Rows.Count > 0)
            {
                DtPrequest = new DataView(DtPrequest, "Field2='" + Session["EmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCallRequest, DtPrequest, "", "");
        }
        else
        {
            GvCallRequest.DataSource = null;
            GvCallRequest.DataBind();
        }
        Session["dtPRequest"] = DtPrequest;

        lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + DtPrequest.Rows.Count.ToString() + "";
        DtPrequest = null;
        //AllPageCode();

    }
    protected void btnPREdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        DataTable dt = objCalllogs.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Field6"].ToString() == "True")
            {
                DataTable dtcon = objContact.GetContactTrueById(dt.Rows[0]["Customer_Name"].ToString());

                txtCustomerName.Text = dtcon.Rows[0]["Name"].ToString() + "/" + dtcon.Rows[0]["Field2"].ToString() + "/" + dtcon.Rows[0]["Field1"].ToString() + "/" + dtcon.Rows[0]["Trans_Id"].ToString();
            }

            if (dt.Rows[0]["Field5"].ToString() == "True")
            {
                DataTable dtcon = objContact.GetContactTrueById(dt.Rows[0]["Contact_Person"].ToString());
                if (dtcon.Rows.Count > 0)
                {
                    txtContactList.Text = dtcon.Rows[0]["Name"].ToString() + "/" + dtcon.Rows[0]["Field2"].ToString() + "/" + dtcon.Rows[0]["Field1"].ToString() + "/" + dtcon.Rows[0]["Trans_Id"].ToString();
                }
            }

            //trCallLogs.Visible = true;
            txtCallNo.Text = dt.Rows[0]["Call_No"].ToString();
            txtCallDate.Text = Convert.ToDateTime(dt.Rows[0]["Call_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtRemark.Text = dt.Rows[0]["Call_Detail"].ToString();
            hdnCalllogsId.Value = e.CommandArgument.ToString();
            txtTenderNo.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Customer_Active()", true);
        }
    }
    #endregion
    #region LOcationStock    
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        string CustomerName = string.Empty;
        try
        {
            CustomerName = txtCustomerName.Text.Split('/')[0].ToString();
        }
        catch
        {
        }
        string strCmd = "window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=SALES&&Contact=" + CustomerName + "')";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
    }



    #endregion


    //product builder concept
    //added by jirtendra upadhyay on 09-11-2016

    protected void lnkProductBuilder_OnClick(object sender, EventArgs e)
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in GvProduct.Rows)
        {

            DataRow dr = dt.NewRow();
            Label lblSNo = (Label)gvRow.FindControl("lblSNo");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)gvRow.FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)gvRow.FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)gvRow.FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)gvRow.FindControl("hdnSuggestedProductName");

            dr["Serial_No"] = lblSNo.Text;
            dr["Product_Id"] = lblgvProductId.Text;
            dr["UnitId"] = lblgvUnitId.Text;
            dr["ProductDescription"] = lblgvPDescription.Text;
            dr["Quantity"] = lblgvReqQty.Text;
            dr["Currency_Id"] = lblgvCurrencyId.Text;
            dr["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dr["SuggestedProductName"] = hdnSuggestedProductName.Value;
            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            dt.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = dt;

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/ProductBuilder.aspx?Type=SINQ')", true);
        dt = null;

    }
    protected void lnkLabelBuilder_OnClick(object sender, EventArgs e)
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in GvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblSNo = (Label)gvRow.FindControl("lblSNo");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)gvRow.FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)gvRow.FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)gvRow.FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)gvRow.FindControl("hdnSuggestedProductName");

            dr["Serial_No"] = lblSNo.Text;
            dr["Product_Id"] = lblgvProductId.Text;
            dr["UnitId"] = lblgvUnitId.Text;
            dr["ProductDescription"] = lblgvPDescription.Text;
            dr["Quantity"] = lblgvReqQty.Text;
            dr["Currency_Id"] = lblgvCurrencyId.Text;
            dr["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dr["SuggestedProductName"] = hdnSuggestedProductName.Value;
            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            dt.Rows.Add(dr);
        }
        Session["DtSearchProduct"] = dt;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory/LabelBuilder.aspx?Type=SINQ')", true);
        dt = null;
    }
    protected void lbkGetProductList_OnClick(object sender, EventArgs e)
    {
        btnAddtoList_Click(null, null);
    }


    protected void txtEstimatedUnitPrice_TextChanged(object sender, EventArgs e)
    {
        decimal data = 0;
        try
        {
            foreach (GridViewRow gvrows in GvProduct.Rows)
            {
                data = data + Convert.ToDecimal((gvrows.FindControl("txtgvEstimatedUnitPrice") as TextBox).Text) * Convert.ToDecimal((gvrows.FindControl("lblgvRequiredQty") as Label).Text);
            }
            txtOpportunityAmt.Text = data.ToString();
        }
        catch (Exception)
        {
            DisplayMessage("Amount Entered Is Not In Proper Format");
            return;
        }
    }


    private string GetCampaignId()
    {
        string retval = "";
        try
        {
            if (txtCampaignID.Text != "")
            {
                int start_pos = txtCampaignID.Text.LastIndexOf("/") + 1;
                int last_pos = txtCampaignID.Text.Length;
                string id = txtCampaignID.Text.Substring(start_pos, last_pos - start_pos);

                if (start_pos != 0)
                {
                    int Last_pos_name = txtCampaignID.Text.LastIndexOf("/");
                    string name = txtCampaignID.Text.Substring(0, Last_pos_name - 0);
                    DataTable dtCampaign = new DataTable();
                    dtCampaign = objCampaign.GetCampaignByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), id);

                    dtCampaign = new DataView(dtCampaign, "Campaign_name='" + name + "' and Trans_Id='" + id + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtCampaign.Rows.Count > 0)
                    {
                        retval = id;
                    }
                    dtCampaign = null;
                }
            }
        }
        catch (Exception error)
        {

        }
        return retval;
    }


    protected void txtCampaignID_TextChanged(object sender, EventArgs e)
    {
        string strCampaignId = string.Empty;
        if (txtCampaignID.Text != "")
        {
            strCampaignId = GetCampaignId();

            if (strCampaignId != "" && strCampaignId != "0")
            {
                txtCampaignID.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtCampaignID.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCampaignID);
            }
        }
        else
        {
            txtCampaignID.Text = "";
            txtCampaignID.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCampaignID);
        }
    }

    protected void btnFolloup_Command(object sender, CommandEventArgs e)
    {
        Session["Oppo_SInquiryID"] = "0";
        Session["Oppo_SInquiryID"] = e.CommandArgument.ToString();
        FollowupUC.setLocationId(e.CommandName.ToString());
        FollowupUC.newBtnCall();
        FollowupUC.GetFollowupDocumentNumber();
        FollowupUC.SetGeneratedByName();
        FollowupUC.ResetFollowupType();
        DataTable dt = FollowupClass.getOppoHeaderDtlByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());
        FollowupUC.fillHeader(dt, hdnFollowupTableName.Value);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open();showNewTab();", true);
        dt = null;
    }



    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUser();
        fillGrid(1);
        if (ddlPermission.SelectedIndex != 0)
        {
            ddlPermission.SelectedIndex = 0;
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

    }

    protected void ddlInquiryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInquiryType.SelectedIndex == 2 || ddlInquiryType.SelectedIndex == 3)
        {
            //lblTenderDate.Visible = true;
            txtTenderDate.Visible = true;
            //lblTenderNo.Visible = true;
            txtTenderNo.Visible = true;
        }
        else
        {
            //lblTenderDate.Visible = false;
            txtTenderDate.Visible = false;
            //lblTenderNo.Visible = false;
            txtTenderNo.Visible = false;
        }
    }



    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        string inquiryNo = e.CommandName.Split('/')[0].ToString();
        string locationid = e.CommandName.Split('/')[1].ToString();
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + locationid + "/Opportunity", "Sales", "Opportunity", inquiryNo, inquiryNo);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }



    protected void ddlSalesStageList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["ReminderID"] != null)
        {
            FillGrid(Request.QueryString["ReminderID"].ToString());
        }
        else
        {
            fillGrid(1);
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        try
        {
            ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Phone_no"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
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
        dt = null;
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
        dt = null;
        return str;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetCustomer()
    {
        HttpContext.Current.Session["ContactID"] = "0";
    }



    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool txtCampaignID_TextChanged(string campaignName, string campaignId)
    {
        try
        {
            string sql = "select COUNT(Trans_ID) from crm_campaign where Campaign_name='" + campaignName + "' and Trans_ID='" + campaignId + "'";
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string count = "";
            count = objDa.get_SingleValue(sql);
            if (count == "" || count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch (Exception error)
        {
            return false;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtProduct_TextChanged(string product)
    {
        string[] ParameterValue = new string[7];
        DataTable dtProduct = new DataTable();
        try
        {
            using (dtProduct = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString()).return_DataTable("select top 1 case when Inv_StockDetail.Quantity IS null then '0' else Inv_StockDetail.Quantity end as Quantity , Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, (select dbo.StripHTML(Inv_ProductMaster.Description)) as Description, Inv_ProductMaster.UnitId, case when relatedProducts IS null then 0 else relatedProducts end as relatedProducts from Inv_ProductMaster left join Inv_StockDetail on Inv_StockDetail.productid=inv_productmaster.ProductId and Inv_StockDetail.Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "' and Inv_StockDetail.Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Inv_StockDetail.Brand_Id ='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Inv_StockDetail.company_id='" + HttpContext.Current.Session["CompId"].ToString() + "' left join (select count(Product_Id) relatedProducts,Product_Id from Inv_Product_RelProduct group by Product_Id) relatedP on relatedP.Product_Id=Inv_ProductMaster.ProductId where Inv_ProductMaster.ProductCode ='" + product.Trim() + "' or Inv_ProductMaster.EProductName = '" + product.Trim() + "'"))
            {
                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    ParameterValue[0] = dtProduct.Rows[0]["EProductName"].ToString();
                    ParameterValue[1] = dtProduct.Rows[0]["ProductCode"].ToString();
                    ParameterValue[2] = "0";
                    if (dtProduct.Rows[0]["UnitId"].ToString() != "0" && dtProduct.Rows[0]["UnitId"].ToString() != "")
                    {
                        ParameterValue[2] = dtProduct.Rows[0]["UnitId"].ToString();
                    }
                    ParameterValue[3] = dtProduct.Rows[0]["Description"].ToString();
                    ParameterValue[4] = dtProduct.Rows[0]["ProductId"].ToString();
                    ParameterValue[5] = dtProduct.Rows[0]["relatedProducts"].ToString();
                    ParameterValue[6] = dtProduct.Rows[0]["Quantity"].ToString();
                }
                else
                {
                    return null;
                }
                string json = "";
                json = JsonConvert.SerializeObject(ParameterValue);
                return json;
            }
        }
        catch
        {
            return null;
        }
        finally
        {
            dtProduct = null;
        }
    }

    protected void btnFillRelatedProducts_Click(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Add_Product_Info.Attributes.Add("Class", "box box-primary");
        FillRelatedProduct(hdnNewProductId.Value);
    }


    protected void btnAddProductToGrid_Click(object sender, EventArgs e)
    {
        DataTable dt1 = Session["DtQuotationItemList"] as DataTable;
        if (dt1 == null)
        {
            return;
        }
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("DaysFrequency");

        foreach (GridViewRow gvRow in GvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblSNo = (Label)gvRow.FindControl("lblSNo");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)gvRow.FindControl("lblgvUnitId");
            Label lblgvPDescription = (Label)gvRow.FindControl("lblgvProductDescription");
            Label lblgvReqQty = (Label)gvRow.FindControl("lblgvRequiredQty");
            Label lblgvCurrencyId = (Label)gvRow.FindControl("lblgvCurrencyId");
            TextBox txtgvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtgvEstimatedUnitPrice");
            HiddenField hdnSuggestedProductName = (HiddenField)gvRow.FindControl("hdnSuggestedProductName");
            TextBox txtgvFrequency = (TextBox)gvRow.FindControl("txtgvFrequency");

            dr["Serial_No"] = lblSNo.Text;
            dr["Product_Id"] = lblgvProductId.Text;
            dr["UnitId"] = lblgvUnitId.Text;
            dr["ProductDescription"] = lblgvPDescription.Text;
            dr["Quantity"] = lblgvReqQty.Text;
            dr["Currency_Id"] = lblgvCurrencyId.Text;
            dr["EstimatedUnitPrice"] = txtgvEstimatedUnitPrice.Text;
            dr["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dr["DaysFrequency"] = txtgvFrequency.Text;

            try
            {
                dr["Sysqty"] = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), lblgvProductId.Text).Rows[0]["Quantity"].ToString();
            }
            catch
            {
                dr["Sysqty"] = "0";
            }
            dt.Rows.Add(dr);
        }

        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            dt.Rows.Add(Convert.ToInt32(dt1.Rows[i]["Trans_Id"].ToString()), dt1.Rows[i]["ProductId"].ToString(), dt1.Rows[i]["UnitId"].ToString(), dt1.Rows[i]["ProductDescription"].ToString(), "1", dt1.Rows[i]["Quantity"].ToString(), ddlCurrency.SelectedValue.ToString(), dt1.Rows[i]["EstimatedUnitPrice"].ToString(), dt1.Rows[i]["SuggestedProductName"].ToString(), "0");
        }
        GvProduct.DataSource = dt;
        GvProduct.DataBind();
        Session["DtQuotationItemList"] = null;
        txtEstimatedUnitPrice_TextChanged(null, null);
        dt = null;
        dt1 = null;
    }


    public void FillUser()
    {
        try
        {
            string strEmpId = string.Empty;
            string strLocationDept = string.Empty;
            string strLocId = Session["LocId"].ToString();

            strLocId = ddlLocation.SelectedValue;
            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept);


            DataTable dtEmp = new DataTable();

            string isSingle = ObjUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString());
            bool IsSingleUser = false;
            try
            {
                IsSingleUser = Convert.ToBoolean(isSingle);
            }
            catch
            {
                IsSingleUser = false;
            }

            string sharedSalesPersons = string.Empty;
            if (Session["EmpId"].ToString() != "0" && IsSingleUser == true)
            {
                sharedSalesPersons = objDa.get_SingleValue("select top 1 Param_Value from set_employee_parameter_new where Param_Name='SharedSalesPersons' and Company_Id='" + Session["CompId"].ToString() + "' and EmpId='" + Session["EmpId"].ToString() + "' and IsActive='true'");
            }

            // can see multiple employee data
            if (IsSingleUser == false)
            {
                //for normal user
                if (Session["EmpId"].ToString() != "0")
                {
                    dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), strEmpId);
                    //dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    //for super admin
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                        dtEmp = new DataView(dtEmp, "Location_Id=" + ddlLocation.SelectedValue.Trim() + "", "emp_name asc", DataViewRowState.CurrentRows).ToTable();
                    }
                }
            }
            else
            {
                string strNewEmpList = Session["EmpId"].ToString();
                if (!string.IsNullOrEmpty(sharedSalesPersons) && sharedSalesPersons != "@NOTFOUND@")
                {
                    strNewEmpList = sharedSalesPersons;
                    strNewEmpList += "," + Session["EmpId"].ToString();
                }

                //dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                dtEmp = objEmployee.GetEmployeeMasterAll(Session["CompId"].ToString());
                dtEmp = new DataView(dtEmp, "emp_id in(" + strNewEmpList + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (ddlLocation.SelectedIndex > 0)
                {
                    dtEmp = new DataView(dtEmp, "location_id='" + ddlLocation.SelectedValue + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_name";
            ddlUser.DataValueField = "user_id";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("--Select User--", "--Select User--"));
        }
        catch
        {

        }
    }

    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";

        DataTable dtEmp = objDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id in (" + strLocationId + ") and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");

        if (dtEmp.Rows[0][0] != null)
        {
            strEmpList = dtEmp.Rows[0][0].ToString();
        }

        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;

    }

    protected void btnAddCustomer_Click(object sender, EventArgs e)
    {
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomerName.Focus();
            return;
        }

        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvSalesInquiry, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvSalesInquiry, lstCls);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string getSharedSalesPerson(string salesPersonId)
    {
        string result = string.Empty;
        try
        {
            string sql = "select Param_Value from Set_Employee_Parameter_New where company_id='" + HttpContext.Current.Session["CompId"].ToString() + "' and empid='" + salesPersonId + "' and param_name='SharedSalesPersons'";
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            result = objDa.get_SingleValue(sql);
        }
        catch
        {

        }
        return result;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool setSharedSalesPerson(string salesPersonId, string salesPersonList)
    {
        bool result = false;
        try
        {
            EmployeeParameter objEmpPara = new EmployeeParameter(HttpContext.Current.Session["DBConnection"].ToString());
            DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string sql = "update Set_Employee_Parameter_New set isActive='false' where empid='" + salesPersonId + "' and company_id='" + HttpContext.Current.Session["CompId"].ToString() + "' and param_name='SharedSalesPersons'";
            objDa.execute_Command(sql);
            string newSpList = string.Empty;
            foreach (var item in salesPersonList.Split(','))
            {
                if (item == salesPersonId)
                {
                    continue;
                }
                else
                {
                    if (newSpList == string.Empty)
                    {
                        newSpList = item;
                    }
                    else
                    {
                        newSpList += "," + item;
                    }
                }
            }
            int b = objEmpPara.InsertEmpParameterNew(salesPersonId, HttpContext.Current.Session["CompId"].ToString(), "SharedSalesPersons", newSpList, "0", "", "true", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b > 0)
            {
                result = true;
            }
        }
        catch (Exception ex)
        {

        }
        return result;
    }
    protected void chkShortProductName_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvRow in GvProduct.Rows)
        {
            Label lblDetailName = (Label)gvRow.FindControl("lblgvProductName");
            Label lblShortName = (Label)gvRow.FindControl("lblShortProductName1");
            if (((CheckBox)sender).Checked)
            {
                lblDetailName.Visible = true;
                lblShortName.Visible = false;
                ((CheckBox)sender).ToolTip = "Display short name";
            }
            else
            {
                lblDetailName.Visible = false;
                lblShortName.Visible = true;
                ((CheckBox)sender).ToolTip = "Display detail name";
            }
        }
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
        dt = null;
        return str;
    }


}
