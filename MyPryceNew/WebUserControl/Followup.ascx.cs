using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
// created by Divya Parakh date-28/2/2018
public partial class WebUserControl_Followup : System.Web.UI.UserControl
{
    SystemParameter ObjSysParam = null;
    FollowUp Followupclass = null;
    Ems_ContactMaster objContact = null;
    Inv_ProductMaster objProductM = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Inv_SalesInquiryDetail objSInquiryDetail = null;
    Inv_SalesQuotationHeader objSQuoteHeader = null;
    Inv_SalesQuotationDetail ObjSQuoteDetail = null;
    EmployeeMaster objEmployee = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Common cmn = null;
    Inv_UnitMaster UM = null;
    CurrencyMaster objCurrency = null;
    IT_ObjectEntry objObjectEntry = null;
    Document_Master ObjDocument = null;
    Arc_Directory_Master objDirectorymaster = null;
    Arc_FileTransaction ObjFile = null;
    Reminder reminderClass = null;
    NotificationMaster Obj_Notifiacation = null;
    ReminderLogs objReminderlog = null;
    OpportunityDashboard objOppoDashboard = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    CountryMaster objCountryMaster = null;
    CallLogs ObjCustInquiry = null;
    SM_WorkOrder ObjWorkOrder = null;
    ContactNoMaster objContactnoMaster = null;
    ES_EmailMaster_Header objEmailHeader = null;
    Ems_Contact_Group objCG = null;
    ES_EmailMasterDetail objEmailDetail = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        Followupclass = new FollowUp(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        objSInquiryDetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        objSQuoteHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjSQuoteDetail = new Inv_SalesQuotationDetail(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objDirectorymaster = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        reminderClass = new Reminder(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objReminderlog = new ReminderLogs(Session["DBConnection"].ToString());
        objOppoDashboard = new OpportunityDashboard(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        ObjCustInquiry = new CallLogs(Session["DBConnection"].ToString());
        ObjWorkOrder = new SM_WorkOrder(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        setLocationId();
        hdnTableName = (HiddenField)Parent.FindControl("hdnFollowupTableName");
        hdnFollowupLocationId.Value = Session["LocId"].ToString();
        txtValueDateBinFollowup.Attributes.Add("readonly", "true");
        txtValueDateFollowup.Attributes.Add("readonly", "true");
        txtNextFollowUpDate.Attributes.Add("readonly", "true");
        txtVisitDate.Attributes.Add("readonly", "true");
        if (!IsPostBack)
        {
            Lbl_Tab_New.Text = "New";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active1()", true);
            AllPageCode(hdnTableName.Value);
            SetGeneratedByName();
        }
    }
    public void SetGeneratedByName()
    {
        if (txtGeneratedByfollowup.Text == "")
        {
            if (Session["UserId"].ToString() == "superadmin")
            {
                txtGeneratedByfollowup.Text = "";
            }
            else
            {
                txtGeneratedByfollowup.Text = ObjUser.GetNameNEmpidByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
            }
        }
    }
    public void GetFollowupDocumentNumber()
    {
        txtfollowupId.Text = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "171", "398", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
    }
    public static string getCustid()
    {
        if (HttpContext.Current.Session["customerID"].ToString() == "")
        {
            return "0";
        }
        else
        {
            return HttpContext.Current.Session["customerID"].ToString();
        }
    }
    public void AllPageCode(string TblName)
    {
        Common.clsPagePermission clsPagePermission = new Common.clsPagePermission();
        if (TblName == "Inv_SalesInquiryHeader")
        {
            clsPagePermission = cmn.getPagePermission("../sales/salesinquiry.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        }
        if (TblName == "Inv_SalesQuotationHeader")
        {
            clsPagePermission = cmn.getPagePermission("../Sales/SalesQuotation.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        }
        if (TblName == "CustomerBalance")
        {
            clsPagePermission = cmn.getPagePermission("../CustomerReceivable/CustomerBalance.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        }
        if (TblName == "WorkOrder")
        {
            clsPagePermission = cmn.getPagePermission("../ServiceManagement/WorkOrder.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        }
        if (TblName == "CRM Follow Up")
        {
            clsPagePermission = cmn.getPagePermission("../CRM/CRM.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
        }
        allPageCodeNew(clsPagePermission);
    }

    public void allPageCodeNew(Common.clsPagePermission clsPagePermission)
    {
        btnFollowupSave.Visible = clsPagePermission.bAdd;
        GvListData.Columns[0].Visible = clsPagePermission.bView;
        GvListData.Columns[1].Visible = clsPagePermission.bEdit;
        GvListData.Columns[2].Visible = clsPagePermission.bDelete;
        imgBtnRestoreFollowup.Visible = clsPagePermission.bRestore;
    }

    public void fillHeader(DataTable DtAllData, string tblName)
    {
        if (DtAllData.Rows.Count > 0)
        {
            if (tblName == "Inv_SalesInquiryHeader")
            {
                lblHeaderL.Text = "Follow-Up (Opportunity)";
                lblHeaderR.Text = DtAllData.Rows[0]["PartyName"].ToString();
                lblheaderTitle.Text = "" + DtAllData.Rows[0]["Opportunity_name"].ToString() + " (" + DtAllData.Rows[0]["Currency_Code"].ToString() + " " + DtAllData.Rows[0]["Opportunity_amount"].ToString() + ") Generated On:  " + GetDate(DtAllData.Rows[0]["IDate"].ToString());
            }
            if (tblName == "Inv_SalesQuotationHeader")
            {
                lblHeaderL.Text = "Follow-Up (Quotation)";
                lblHeaderR.Text = DtAllData.Rows[0]["PartyName"].ToString();
                lblheaderTitle.Text = "Quotation Amt : " + DtAllData.Rows[0]["Currency_Code"].ToString() + " " + DtAllData.Rows[0]["Amount"].ToString() + " Generated On:  " + GetDate(DtAllData.Rows[0]["Quotation_Date"].ToString());
            }
            if (tblName == "CustomerBalance")
            {
                lblHeaderL.Text = "Follow-Up (Customer Balance)";
                lblHeaderR.Text = DtAllData.Rows[0]["Name"].ToString();
            }
            if (tblName == "CRM Follow Up")
            {
                lblHeaderL.Text = "Follow-Up (CRM)";
                lblHeaderR.Text = DtAllData.Rows[0]["Name"].ToString();
            }
            if (tblName == "WorkOrder")
            {
                lblHeaderL.Text = "Follow-Up (Work Order)";
                lblHeaderR.Text = DtAllData.Rows[0]["PartyName"].ToString();
            }


        }
    }
    public void fillHeader(string leftHeader, string partyName)
    {
        lblHeaderL.Text = leftHeader;
        lblHeaderR.Text = partyName;
    }
    public void fillFollowupListSession(string tblName)
    {
        DataTable DtAllData = new DataTable();
        if (tblName == "Inv_SalesInquiryHeader")
        {
            if (Session["Oppo_SInquiryID"] != null)
            {
                if (Session["Oppo_SInquiryID"].ToString() != "0")
                    DtAllData = Followupclass.getGridOppoByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["Oppo_SInquiryID"].ToString());
                if (DtAllData != null)
                {
                    Session["fillFollowupOppoList"] = DtAllData;
                }
            }
        }
        if (tblName == "Inv_SalesQuotationHeader")
        {
            if (Session["Quote_SQuotation_Id"] != null)
            {
                if (Session["Quote_SQuotation_Id"].ToString() != "0")
                    DtAllData = Followupclass.getGridQuoteDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["Quote_SQuotation_Id"].ToString());
                if (DtAllData != null)
                {
                    Session["fillFollowupQuoteList"] = DtAllData;
                }
            }
        }
        if (tblName == "CustomerBalance" || tblName == "CRM Follow Up")
        {
            if (Session["CustBal_CustID"] != null)
            {
                if (Session["CustBal_CustID"].ToString() != "0")
                    DtAllData = Followupclass.getGridQuoteDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["CustBal_CustID"].ToString());
                if (DtAllData != null)
                {
                    Session["fillFollowupCustBalList"] = DtAllData;
                }
            }
        }
        if (tblName == "WorkOrder")
        {
            if (Session["WorkOrder_CustID"] != null)
            {
                if (Session["WorkOrder_CustID"].ToString() != "0")
                    DtAllData = Followupclass.getGridQuoteDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["WorkOrder_CustID"].ToString());
                if (DtAllData != null)
                {
                    Session["fillFollowupWorkOrderList"] = DtAllData;
                }
            }
        }
        DtAllData = null;
    }
    public void fillFollowupList(string tblname)
    {
        DataTable DtAllData = new DataTable();
        if (tblname == "Inv_SalesInquiryHeader")
        {
            DtAllData = Session["fillFollowupOppoList"] as DataTable;
        }
        if (tblname == "Inv_SalesQuotationHeader")
        {
            DtAllData = Session["fillFollowupQuoteList"] as DataTable;
        }
        if (tblname == "CustomerBalance")
        {
            DtAllData = Session["fillFollowupCustBalList"] as DataTable;
        }
        if (tblname == "CRM Follow Up")
        {
            DtAllData = Session["fillFollowupCustBalList"] as DataTable;
        }
        if (tblname == "WorkOrder")
        {
            DtAllData = Session["fillFollowupWorkOrderList"] as DataTable;
        }
        if (DtAllData != null && DtAllData.Rows.Count > 0)
        {
            GvListData.DataSource = DtAllData;
            GvListData.DataBind();
            lblTotalRecordsFollowup.Text = "Total Records: " + DtAllData.Rows.Count.ToString();
            //AllPageCode(tblname);
        }
        else
        {
            GvListData.DataSource = null;
            GvListData.DataBind();
            lblTotalRecordsFollowup.Text = "Total Records: 0";
        }
        //fillHeader(tblname);
        DtAllData = null;
    }
    public void fillFollowupBinSession(string tblName)
    {
        DataTable DtAllData = new DataTable();
        if (tblName == "Inv_SalesInquiryHeader")
        {
            if (Session["Oppo_SInquiryID"] != null)
            {
                if (Session["Oppo_SInquiryID"].ToString() != "0")
                {
                    DtAllData = Followupclass.getAllInactiveDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["Oppo_SInquiryID"].ToString());
                    Session["fillFollowupOppoBin"] = DtAllData;
                }
            }
        }
        if (tblName == "Inv_SalesQuotationHeader")
        {
            if (Session["Quote_SQuotation_Id"] != null)
            {
                if (Session["Quote_SQuotation_Id"].ToString() != "0")
                {
                    DtAllData = Followupclass.getAllInactiveDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["Quote_SQuotation_Id"].ToString());
                    Session["fillFollowupQuoteBin"] = DtAllData;
                }
            }
        }
        if (tblName == "CustomerBalance")
        {
            if (Session["CustBal_CustID"] != null)
            {
                if (Session["CustBal_CustID"].ToString() != "0")
                    DtAllData = Followupclass.getAllInactiveDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["CustBal_CustID"].ToString());
                if (DtAllData != null)
                {
                    Session["fillFollowupCustBalBin"] = DtAllData;
                }
            }
        }
        if (tblName == "WorkOrder")
        {
            if (Session["WorkOrder_CustID"] != null)
            {
                if (Session["WorkOrder_CustID"].ToString() != "0")
                    DtAllData = Followupclass.getAllInactiveDataByTblNameAndPK(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, tblName, Session["WorkOrder_CustID"].ToString());
                if (DtAllData != null)
                {
                    Session["fillFollowupWorkOrderBin"] = DtAllData;
                }
            }
        }
        DtAllData = null;
    }
    public void fillFollowupBin(string tblName)
    {
        DataTable DtAllData = new DataTable();
        if (tblName == "Inv_SalesInquiryHeader")
        {
            DtAllData = Session["fillFollowupOppoBin"] as DataTable;
        }
        if (tblName == "Inv_SalesQuotationHeader")
        {
            DtAllData = Session["fillFollowupQuoteBin"] as DataTable;
        }
        if (tblName == "CustomerBalance")
        {
            DtAllData = Session["fillFollowupCustBalBin"] as DataTable;
        }
        if (tblName == "WorkOrder")
        {
            DtAllData = Session["fillFollowupWorkOrderBin"] as DataTable;
        }
        if (DtAllData != null && DtAllData.Rows.Count > 0)
        {
            GvFollowupBin.DataSource = DtAllData;
            GvFollowupBin.DataBind();
            lblTotalRecordsBinFollowup.Text = "Total Records: " + DtAllData.Rows.Count.ToString();
            //AllPageCode(tblName);
        }
        else
        {
            GvFollowupBin.DataSource = null;
            GvFollowupBin.DataBind();
            lblTotalRecordsBinFollowup.Text = "Total Records: 0";
        }
        DtAllData = null;
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
    protected void btnGenerateQuote_Command(object sender, CommandEventArgs e)
    {
        Response.Redirect("../Sales/SalesQuotation.aspx?InquiryId=" + e.CommandName.ToString());
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
            string filtertext = "Name like '%" + prefixText + "%'";
            DataTable dtCust = new DataView(dtCustomer, filtertext, "", DataViewRowState.CurrentRows).ToTable();
            if (dtCust.Rows.Count == 0)
            {
                dtCust = dtCustomer;
            }
            string[] filterlist = new string[dtCust.Rows.Count];
            if (dtCust.Rows.Count > 0)
            {
                for (int i = 0; i < dtCust.Rows.Count; i++)
                {
                    filterlist[i] = dtCust.Rows[i]["Name"].ToString() + "/" + dtCust.Rows[i]["Customer_Id"].ToString();
                }
            }
            return filterlist;
        }
        catch (Exception error)
        {
        }
        return null;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        try
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
            DataTable dtCon = ObjContactMaster.GetContactTrueAllData(id, "Individual");
            string filtertext = "Name like '%" + prefixText + "%'";
            DataTable dt_Contact = new DataView(dtCon, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable(true, "Name", "Trans_Id");
            //DataTable DtCompany = ObjContactMaster.GetContactTrueById(id).DefaultView.ToTable(true, "Name", "Trans_Id");
            //dt.Merge(DtCompany);
            string[] filterlist = new string[dt_Contact.Rows.Count];
            if (dt_Contact.Rows.Count > 0)
            {
                for (int i = 0; i < dt_Contact.Rows.Count; i++)
                {
                    filterlist[i] = dt_Contact.Rows[i]["Name"].ToString() + "/" + dt_Contact.Rows[i]["Trans_Id"].ToString();
                }
                return filterlist;
            }
            else
            {
                DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
                string[] filterlistcon = new string[dtcon.Rows.Count];
                for (int i = 0; i < dtcon.Rows.Count; i++)
                {
                    filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
                }
                return filterlistcon;
            }
        }
        catch (Exception error)
        {
        }
        return null;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
            }
        }
        return txt;
    }
    protected void txtContactList_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactList.Text != "")
        {
            strCustomerId = GetContactId();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                hdnContact_Id.Value = strCustomerId;
                txtContactList.Focus();
                ContactNo1.setNullToGV();
                ContactNo1.FillGridData(strCustomerId, "Ems_ContactMaster");
                using (DataTable dt = ContactNo1.getDatatable())
                {
                    if (dt.Rows.Count == 0)
                    {
                        ContactNo1.FillGridData(strCustomerId, "Set_AddressMaster");
                    }
                }
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
    private string GetContactId()
    {
        string retval = "";
        try
        {
            if (txtContactList.Text != "")
            {
                int start_pos = txtContactList.Text.LastIndexOf("/") + 1;
                int last_pos = txtContactList.Text.Length;
                string id = txtContactList.Text.Substring(start_pos, last_pos - start_pos);
                int Last_pos_name = txtContactList.Text.LastIndexOf("/");
                string name = txtContactList.Text.Substring(0, Last_pos_name - 0);
                //DataTable dtSupp = objContact.GetContactByContactName(txtContactList.Text.Trim().Split('/')[0].ToString());
                string contactName = "";
                contactName = objContact.GetContactNameByContactiD(id);
                if (contactName != "" && contactName == name)
                {
                    retval = id;
                }
            }
        }
        catch (Exception error)
        {
        }
        return retval;
    }
    protected void lnkcustomerHistory_OnClick(object sender, EventArgs e)
    {
        string Id = string.Empty;
        try
        {
            Id = txtCustomerName.Text.Split('/')[1].ToString();
        }
        catch
        {
            Id = "0";
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CustomerHistory(" + Id + ")", true);
    }
    protected void btnRefreshFollowup_Click(object sender, ImageClickEventArgs e)
    {
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            Session["fillFollowupOppoList"] = null;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            Session["fillFollowupQuoteList"] = null;
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            Session["fillFollowupCustBalList"] = null;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            Session["fillFollowupWorkOrderList"] = null;
        }
        fillFollowupListSession(hdnTableName.Value);
        fillFollowupList(hdnTableName.Value);
        txtValueDateFollowup.Text = "";
        txtValueFollowup.Text = "";
    }
    protected void btnListSearch_Click(object sender, ImageClickEventArgs e)
    {
        fillFollowupListSession(hdnTableName.Value);
        DataTable dtAdd = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dtAdd = Session["fillFollowupOppoList"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dtAdd = Session["fillFollowupQuoteList"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            dtAdd = Session["fillFollowupCustBalList"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dtAdd = Session["fillFollowupWorkOrderList"] as DataTable;
        }
        if (ddlOptionFollowup.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldNameFollowup.SelectedItem.Value == "Followup_date" || ddlFieldNameFollowup.SelectedItem.Value == "Next_followup_date")
            {
                if (txtValueDateFollowup.Text.Trim() != "")
                {
                    if (ddlOptionFollowup.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameFollowup.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDateFollowup.Text.Trim()) + "'";
                    }
                    else if (ddlOptionFollowup.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameFollowup.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDateFollowup.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameFollowup.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDateFollowup.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Date");
                    txtValueDateFollowup.Focus();
                    return;
                }
            }
            else
            {
                if (txtValueFollowup.Text.Trim() != "")
                {
                    if (ddlOptionFollowup.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameFollowup.SelectedValue + ",System.String)='" + txtValueFollowup.Text.Trim() + "'";
                    }
                    else if (ddlOptionFollowup.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameFollowup.SelectedValue + ",System.String) like '%" + txtValueFollowup.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameFollowup.SelectedValue + ",System.String) Like '%" + txtValueFollowup.Text.Trim() + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Some Value");
                    txtValueFollowup.Focus();
                }
            }
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvListData, view.ToTable(), "", "");
            if (hdnTableName.Value == "Inv_SalesInquiryHeader")
            {
                Session["fillFollowupOppoList"] = view.ToTable();
            }
            if (hdnTableName.Value == "Inv_SalesQuotationHeader")
            {
                Session["fillFollowupQuoteList"] = view.ToTable();
            }
            if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
            {
                Session["fillFollowupCustBalList"] = view.ToTable();
            }
            if (hdnTableName.Value == "WorkOrder")
            {
                Session["fillFollowupWorkOrderList"] = view.ToTable();
            }
            lblTotalRecordsFollowup.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode(hdnTableName.Value);

        }
        dtAdd = null;
    }
    public void reset()
    {
        txtNextFollowUpDate.Text = "";
        txtTypeReference.Text = "";
        txtRemark.Text = "";
        txtFollowupDt.Text = "";
        txtGeneratedByfollowup.Text = "";
        txtReferenceNo.Text = "";
        ddlFollowupType.SelectedIndex = 0;
        GvProductDataFollowup.DataSource = null;
        Lbl_Tab_New.Text = "New";
        GvProductDataFollowup.DataBind();
        hdntransID.Value = "";
        chkReminder.Checked = false;
        GetFollowupDocumentNumber();
        SetGeneratedByName();
        Session["strReferenceId"] = "0";
        txtfollowupId.Enabled = true;
    }
    public void newBtnCall()
    {
        //fileupload_divFollowup.Visible = false;
        txtfollowupId.Enabled = true;
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            if (Session["Oppo_SInquiryID"] == null)
            {
                return;
            }
            if (Session["Oppo_SInquiryID"].ToString() != "0")
            {
                //DataTable DtoppoData = Followupclass.GetOpportunityDataByOppoID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, Session["Oppo_SInquiryID"].ToString());
                DataTable DtoppoData = objOppoDashboard.getHeaderNDetailDataByTransId(Session["Oppo_SInquiryID"].ToString());
                if (DtoppoData != null && DtoppoData.Rows.Count != 0)
                {
                    reset();
                    setCountryCode_ContactNo();
                    txtFollowupDt.Text = GetDate(System.DateTime.Now.ToString());
                    txtReferenceNo.Text = DtoppoData.Rows[0]["SInquiryNo"].ToString();
                    txtCustomerName.Text = DtoppoData.Rows[0]["CustomerName"].ToString() + "/" + DtoppoData.Rows[0]["Customer_Id"].ToString();
                    hdncust_Id.Value = DtoppoData.Rows[0]["Customer_Id"].ToString();
                    Session["ContactID"] = hdncust_Id.Value;
                    hdnContact_Id.Value = DtoppoData.Rows[0]["Field2"].ToString();
                    ContactNo1.setNullToGV();
                    ContactNo1.FillGridData(hdnContact_Id.Value, "Ems_ContactMaster");
                    txtContactList.Text = DtoppoData.Rows[0]["contactName"].ToString() + "/" + DtoppoData.Rows[0]["Field2"].ToString();
                    //txtRemark.Text = DtoppoData.Rows[0]["Remark"].ToString();
                    GvProductDataFollowup.DataSource = DtoppoData;
                    GvProductDataFollowup.DataBind();
                    Session["customerID"] = DtoppoData.Rows[0]["Customer_Id"].ToString();
                    //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active1()", true);
                }
                DtoppoData = null;
            }
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            if (Session["Quote_SQuotation_Id"] == null)
            {
                return;
            }
            if (Session["Quote_SQuotation_Id"].ToString() != "0")
            {
                //DataTable DtQuoteData = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, Session["Quote_SQuotation_Id"].ToString());
                using (DataTable DtQuoteData = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, Session["Quote_SQuotation_Id"].ToString()))
                {
                    using (DataTable DtQuoteDetail = ObjSQuoteDetail.GetAllQuotationDetailBySQuotationID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, Session["Quote_SQuotation_Id"].ToString()))
                    {
                        if (DtQuoteData != null && DtQuoteData.Rows.Count != 0)
                        {
                            reset();
                            setCountryCode_ContactNo();
                            string strSalesInquiryId = DtQuoteData.Rows[0]["SInquiry_No"].ToString();
                            txtFollowupDt.Text = GetDate(System.DateTime.Now.ToString());
                            DataTable DtoppoData = Followupclass.GetOpportunityDataByOppoID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, strSalesInquiryId);
                            txtReferenceNo.Text = DtQuoteData.Rows[0]["SQuotation_No"].ToString();
                            txtCustomerName.Text = DtoppoData.Rows[0]["CustomerName"].ToString() + "/" + DtoppoData.Rows[0]["Customer_Id"].ToString();
                            txtContactList.Text = DtoppoData.Rows[0]["contactName"].ToString() + "/" + DtoppoData.Rows[0]["Field2"].ToString();
                            hdncust_Id.Value = DtoppoData.Rows[0]["Customer_Id"].ToString();
                            Session["ContactID"] = hdncust_Id.Value;
                            hdnContact_Id.Value = DtoppoData.Rows[0]["Field2"].ToString();
                            ContactNo1.setNullToGV();
                            ContactNo1.FillGridData(hdnContact_Id.Value, "Ems_ContactMaster");
                            //txtRemark.Text = DtoppoData.Rows[0]["Remark"].ToString();
                            GvProductDataFollowup.DataSource = DtQuoteDetail;
                            GvProductDataFollowup.DataBind();
                            Session["customerID"] = DtoppoData.Rows[0]["Customer_Id"].ToString();
                            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active1()", true);
                        }
                    }

                }

            }
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            if (Session["CustBal_CustID"] == null)
            {
                return;
            }
            if (Session["CustBal_CustID"].ToString() != "0")
            {
                DataTable Dt_custBalData = objContact.GetContactTrueById(Session["CustBal_CustID"].ToString());
                if (Dt_custBalData != null && Dt_custBalData.Rows.Count != 0)
                {
                    reset();
                    txtFollowupDt.Text = GetDate(System.DateTime.Now.ToString());
                    txtReferenceNo.Text = Dt_custBalData.Rows[0]["Code"].ToString();
                    txtCustomerName.Text = Dt_custBalData.Rows[0]["Name"].ToString() + "/" + Dt_custBalData.Rows[0]["Trans_Id"].ToString();
                    hdncust_Id.Value = Dt_custBalData.Rows[0]["Trans_Id"].ToString();
                    DataTable dt_ContactData = objContact.GetTop1FinanceContactPersonName(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, hdncust_Id.Value);
                    Session["ContactID"] = hdncust_Id.Value;
                    if (dt_ContactData != null && dt_ContactData.Rows.Count != 0)
                    {
                        hdnContact_Id.Value = dt_ContactData.Rows[0]["Trans_Id"].ToString();
                        ContactNo1.setNullToGV();
                        ContactNo1.FillGridData(hdnContact_Id.Value, "Ems_ContactMaster");
                        txtContactList.Text = dt_ContactData.Rows[0]["Name"].ToString() + "/" + dt_ContactData.Rows[0]["Trans_Id"].ToString();
                    }
                    else
                    {
                        hdnContact_Id.Value = "0";
                        ContactNo1.setNullToGV();
                        txtContactList.Text = "";
                    }
                    GvProductDataFollowup.DataSource = null;
                    GvProductDataFollowup.DataBind();
                    Session["customerID"] = Dt_custBalData.Rows[0]["Trans_Id"].ToString();
                    dt_ContactData = null;
                }
                Dt_custBalData = null;
            }
        }

        if (hdnTableName.Value == "WorkOrder")
        {
            if (Session["WorkOrder_CustID"] == null)
            {
                return;
            }
            if (Session["WorkOrder_CustID"].ToString() != "0")
            {
                DataTable Dt_custBalData = ObjWorkOrder.getWorkOrderFollowupData(Session["WorkOrder_CustID"].ToString());
                if (Dt_custBalData != null && Dt_custBalData.Rows.Count != 0)
                {
                    div_followupType.Attributes["style"] = "display:none";
                    product_div.Attributes["style"] = "display:none";
                    reset();

                    txtFollowupDt.Text = GetDate(System.DateTime.Now.ToString());
                    txtReferenceNo.Text = Dt_custBalData.Rows[0]["work_order_no"].ToString();
                    txtCustomerName.Text = Dt_custBalData.Rows[0]["customerName"].ToString() + "/" + Dt_custBalData.Rows[0]["customer_id"].ToString();
                    Session["customerID"] = Dt_custBalData.Rows[0]["customer_id"].ToString();
                    hdncust_Id.Value = Dt_custBalData.Rows[0]["customer_id"].ToString();
                    hdnContact_Id.Value = Dt_custBalData.Rows[0]["contact_id"].ToString();
                    Session["ContactID"] = hdncust_Id.Value;
                    ContactNo1.setNullToGV();
                    ContactNo1.FillGridData(hdnContact_Id.Value, "Ems_ContactMaster");
                    txtContactList.Text = Dt_custBalData.Rows[0]["ContactName"].ToString() + "/" + Dt_custBalData.Rows[0]["contact_Id"].ToString();                    
                }
                Dt_custBalData = null;
            }
        }

    }
    public string getID(string name)
    {
        string retval = "";
        try
        {
            if (name != "")
            {
                int start_pos = name.LastIndexOf("/") + 1;
                int last_pos = name.Length;
                string id = name.Substring(start_pos, last_pos - start_pos);
                int Last_pos_name = name.LastIndexOf("/");
                string empName = name.Substring(0, Last_pos_name - 0);
                if (start_pos != 0)
                {
                    string Employee = "";
                    Employee = objEmployee.GetEmployeeNameByEmployeeId(id, Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value);
                    if (Employee != "" && Employee == empName)
                    {
                        retval = id;
                    }
                }
            }
        }
        catch (Exception error)
        {
        }
        return retval;
    }
    protected void txtGeneratedByfollowup_TextChanged(object sender, EventArgs e)
    {
        string strGeneratedById = string.Empty;
        string name = txtGeneratedByfollowup.Text;
        if (txtGeneratedByfollowup.Text != "")
        {
            strGeneratedById = getID(txtGeneratedByfollowup.Text);
            if (strGeneratedById != "" && strGeneratedById != "0")
            {
                txtGeneratedByfollowup.Text = name;
                txtGeneratedByfollowup.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtGeneratedByfollowup.Text = "";
                txtGeneratedByfollowup.Focus();
            }
        }
        else
        {
            txtGeneratedByfollowup.Text = "";
            txtGeneratedByfollowup.Focus();
            txtGeneratedByfollowup.Focus();
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //fileupload_divFollowup.Visible = true;
        ImageButton b = (ImageButton)sender;
        string objSenderID = b.ID;
        hdntransID.Value = e.CommandArgument.ToString();
        DataTable dtAllData = Followupclass.getDataByTransID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, hdntransID.Value);
        if (hdnTableName.Value != "CustomerBalance")
        {
            DataTable dtProductData = objSInquiryDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, dtAllData.Rows[0]["Ref_table_pk"].ToString(),Session["FinanceYearId"].ToString());
            fillProductListData(dtProductData);
        }
        if (objSenderID == "lnkViewDetailFollowup")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            if (hdnTableName.Value == "Inv_SalesInquiryHeader")
                btnQuote.Visible = true;
            btnFollowupSave.Visible = false;
            BtnFollowupReset.Visible = false;
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            btnQuote.Visible = false;
            BtnFollowupReset.Visible = true;
            btnFollowupSave.Visible = true;
        }
        fillControlsValue(dtAllData);
        ContactNo1.setNullToGV();
        ContactNo1.FillGridData(dtAllData.Rows[0]["Contact_Id"].ToString(), "Ems_ContactMaster");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active1()", true);
        dtAllData = null;
        txtfollowupId.Enabled = false;
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Tab_New1()", true);
    }
    public void fillControlsValue(DataTable DtoppoData)
    {
        txtfollowupId.Text = DtoppoData.Rows[0]["Followup_No"].ToString();
        if (hdnTableName.Value == "CustomerBalance")
        {
            try
            {
                txtReferenceNo.Text = objContact.getContactCodeFromId(DtoppoData.Rows[0]["Ref_table_pk"].ToString());
            }
            catch
            {
            }
        }
        else
        {
            txtReferenceNo.Text = DtoppoData.Rows[0]["SInquiryno"].ToString();
        }
        txtFollowupDt.Text = GetDate(DtoppoData.Rows[0]["Followup_date"].ToString());
        //CalendarExtender2.SelectedDate = Convert.ToDateTime(DtoppoData.Rows[0]["Followup_date"].ToString());
        txtCustomerName.Text = DtoppoData.Rows[0]["PartyName"].ToString() + "/" + DtoppoData.Rows[0]["Party_Id"].ToString();
        hdncust_Id.Value = DtoppoData.Rows[0]["Party_Id"].ToString();
        txtContactList.Text = DtoppoData.Rows[0]["ContactName"].ToString() + "/" + DtoppoData.Rows[0]["Contact_Id"].ToString();
        hdnContact_Id.Value = DtoppoData.Rows[0]["Contact_Id"].ToString();
        txtGeneratedByfollowup.Text = DtoppoData.Rows[0]["FollowupByName"].ToString() + "/" + DtoppoData.Rows[0]["Followup_by"].ToString();
        txtRemark.Text = DtoppoData.Rows[0]["description"].ToString();
        txtNextFollowUpDate.Text = GetDate(DtoppoData.Rows[0]["Next_followup_date"].ToString());
        txtTypeReference.Text = DtoppoData.Rows[0]["Followup_type_ref_id"].ToString();
        if (txtTypeReference.Text.Trim() == "0" || txtTypeReference.Text.Trim() == "")
        {
            txtTypeReference.Enabled = false;
            txtTypeReference.Text = "";
            AutoCompleteExtenderCall.Enabled = false;
            AutoCompleteExtenderVisit.Enabled = false;
        }
        else
        {
            if (DtoppoData.Rows[0]["Followup_type"].ToString() == "Call")
            {
                txtTypeReference.Text = DtoppoData.Rows[0]["call_no"].ToString() + "/" + DtoppoData.Rows[0]["Followup_type_ref_id"].ToString();
                Session["strReferenceId"] = DtoppoData.Rows[0]["Followup_type_ref_id"].ToString();
                AutoCompleteExtenderCall.Enabled = true;
            }
            else
            {
                txtTypeReference.Text = DtoppoData.Rows[0]["Work_Order_No"].ToString() + "/" + DtoppoData.Rows[0]["Followup_type_ref_id"].ToString();
                Session["strReferenceId"] = DtoppoData.Rows[0]["Followup_type_ref_id"].ToString();
                AutoCompleteExtenderVisit.Enabled = true;
            }
            txtTypeReference.Enabled = true;
        }
        if (DtoppoData.Rows[0]["Followup_type"].ToString() == "" || DtoppoData.Rows[0]["Followup_type"].ToString() == "Select")
        {
            ddlFollowupType.SelectedValue = "Select";
        }
        else
        {
            ddlFollowupType.SelectedValue = DtoppoData.Rows[0]["Followup_type"].ToString();
        }
    }
    public void fillProductListData(DataTable dtProductdata)
    {
        GvProductDataFollowup.DataSource = dtProductdata;
        GvProductDataFollowup.DataBind();
    }
    public string setUnitName(string id)
    {
        if (id != "")
        {
            return UM.GetUnitNameByUnitId(id, Session["CompId"].ToString());
        }
        return id;
    }
    public string setCurrency(string Id)
    {
        return objCurrency.GetCurrencyNameById(Id);
    }
    protected void ddlFieldNameBinFollowup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBinFollowup.SelectedIndex == 0)
        {
            txtValueBinFollowup.Visible = false;
            txtValueDateBinFollowup.Visible = true;
        }
        else
        {
            txtValueBinFollowup.Visible = true;
            txtValueDateBinFollowup.Visible = false;
        }
    }
    protected void btnsearchBinFollowup_Click(object sender, ImageClickEventArgs e)
    {
        fillFollowupBinSession(hdnTableName.Value);
        DataTable dtAdd = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dtAdd = Session["fillFollowupOppoBin"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dtAdd = Session["fillFollowupQuoteBin"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance")
        {
            dtAdd = Session["fillFollowupCustBalBin"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dtAdd = Session["fillFollowupWorkOrderBin"] as DataTable;
        }
        if (ddlOptionBinFollowup.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldNameBinFollowup.SelectedItem.Value == "Followup_date")
            {
                if (txtValueDateBinFollowup.Text.Trim() != "")
                {
                    if (ddlOptionBinFollowup.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameBinFollowup.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDateBinFollowup.Text.Trim()) + "'";
                    }
                    else if (ddlOptionBinFollowup.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameBinFollowup.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDateBinFollowup.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameBinFollowup.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDateBinFollowup.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Date");
                    txtValueDateBinFollowup.Focus();
                    return;
                }
            }
            else
            {
                if (txtValueBinFollowup.Text.Trim() != "")
                {
                    if (ddlOptionBinFollowup.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameBinFollowup.SelectedValue + ",System.String)='" + txtValueBinFollowup.Text.Trim() + "'";
                    }
                    else if (ddlOptionBinFollowup.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameBinFollowup.SelectedValue + ",System.String) like '%" + txtValueBinFollowup.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameBinFollowup.SelectedValue + ",System.String) Like '%" + txtValueBinFollowup.Text.Trim() + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Some Value");
                    txtValueBinFollowup.Focus();
                }
            }
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvFollowupBin, view.ToTable(), "", "");
            if (hdnTableName.Value == "Inv_SalesInquiryHeader")
            {
                Session["fillFollowupOppoBin"] = view.ToTable();
            }
            if (hdnTableName.Value == "Inv_SalesQuotationHeader")
            {
                Session["fillFollowupQuoteBin"] = view.ToTable();
            }
            if (hdnTableName.Value == "CustomerBalance")
            {
                Session["fillFollowupCustBalBin"] = view.ToTable();
            }
            if (hdnTableName.Value == "WorkOrder")
            {
                Session["fillFollowupWorkOrderBin"] = view.ToTable();
            }
            lblTotalRecordsBinFollowup.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode(hdnTableName.Value);
        }
        dtAdd = null;
    }
    protected void btnRefreshBinFollowup_Click(object sender, ImageClickEventArgs e)
    {
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            Session["fillFollowupOppoBin"] = null;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            Session["fillFollowupQuoteBin"] = null;
        }
        if (hdnTableName.Value == "CustomerBalance")
        {
            Session["fillFollowupCustBalBin"] = null;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            Session["fillFollowupWorkOrderBin"] = null;
        }
        fillFollowupBinSession(hdnTableName.Value);
        fillFollowupBin(hdnTableName.Value);
        txtValueBinFollowup.Text = "";
        txtValueDateBinFollowup.Text = "";
    }
    protected void imgBtnRestoreFollowup_Click(object sender, ImageClickEventArgs e)
    {
        if (lblSelectedRecordFollowup.Text != "")
        {
            for (int j = 0; j < lblSelectedRecordFollowup.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecordFollowup.Text.Split(',')[j] != "")
                {
                    Followupclass.ActivateDataByTransID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, lblSelectedRecordFollowup.Text.Split(',')[j].Trim());
                    DisplayMessage("Record Activate");
                }
            }
        }
        int fleg = 0;
        foreach (GridViewRow Gvr in GvFollowupBin.Rows)
        {
            CheckBox chk = (CheckBox)Gvr.FindControl("chkSelectFollowup");
            if (chk.Checked)
            {
                fleg = 1;
            }
            else
            {
                fleg = 0;
            }
        }
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            Session["fillFollowupOppoBin"] = null;
            Session["fillFollowupOppoList"] = null;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            Session["fillFollowupQuoteBin"] = null;
            Session["fillFollowupQuoteList"] = null;
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            Session["fillFollowupCustBalBin"] = null;
            Session["fillFollowupCustBalList"] = null;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            Session["fillFollowupWorkOrderBin"] = null;
            Session["fillFollowupWorkOrderList"] = null;
        }
        fillFollowupBinSession(hdnTableName.Value);
        fillFollowupBin(hdnTableName.Value);
        //fillFollowupListSession(hdnTableName.Value);
        //fillFollowupList(hdnTableName.Value);
    }
    protected void ImgbtnSelectAllFollowup_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dtPbrand = Session["fillFollowupOppoBin"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dtPbrand = Session["fillFollowupQuoteBin"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance")
        {
            dtPbrand = Session["fillFollowupCustBalBin"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dtPbrand = Session["fillFollowupWorkOrderBin"] as DataTable;
        }

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecordFollowup.Text.Split(',').Contains(dr["Trans_id"]))
                {
                    lblSelectedRecordFollowup.Text += dr["Trans_id"] + ",";
                }
            }
            for (int i = 0; i < GvFollowupBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecordFollowup.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvFollowupBin.Rows[i].FindControl("hdntransIdFollowup");
                for (int j = 0; j < lblSelectedRecordFollowup.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecordFollowup.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecordFollowup.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvFollowupBin.Rows[i].FindControl("chkSelectFollowup")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecordFollowup.Text = "";
            DataTable dtSelectedData = new DataTable();
            if (hdnTableName.Value == "Inv_SalesInquiryHeader")
            {
                dtSelectedData = Session["fillFollowupOppoBin"] as DataTable;
            }
            if (hdnTableName.Value == "Inv_SalesQuotationHeader")
            {
                dtSelectedData = Session["fillFollowupQuoteBin"] as DataTable;
            }
            if (hdnTableName.Value == "CustomerBalance")
            {
                dtSelectedData = Session["fillFollowupCustBalBin"] as DataTable;
            }
            if (hdnTableName.Value == "WorkOrder")
            {
                dtSelectedData = Session["fillFollowupWorkOrderBin"] as DataTable;
            }

            objPageCmn.FillData((object)GvFollowupBin, dtSelectedData, "", "");
            ViewState["Select"] = null;
            dtSelectedData = null;
        }
        dtPbrand = null;
    }
    protected void chkSelectFollowup_CheckedChanged(object sender, EventArgs e)
    {
        string Followupidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)GvFollowupBin.Rows[index].FindControl("lbltransId");
        if (((CheckBox)GvFollowupBin.Rows[index].FindControl("chkSelectFollowup")).Checked)
        {
            Followupidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecordFollowup.Text += Followupidlist;
        }
        else
        {
            Followupidlist += lb.Text.ToString().Trim();
            lblSelectedRecordFollowup.Text += Followupidlist;
            string[] split = lblSelectedRecordFollowup.Text.Split(',');
            foreach (string item in split)
            {
                if (item != Followupidlist)
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
            lblSelectedRecordFollowup.Text = temp;
        }
    }
    protected void chkgvSelectAllFollowup_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvFollowupBin.HeaderRow.FindControl("chkgvSelectAllFollowup"));
        for (int i = 0; i < GvFollowupBin.Rows.Count; i++)
        {
            ((CheckBox)GvFollowupBin.Rows[i].FindControl("chkSelectFollowup")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecordFollowup.Text.Split(',').Contains(((HiddenField)(GvFollowupBin.Rows[i].FindControl("hdntransIdFollowup"))).Value.Trim().ToString()))
                {
                    lblSelectedRecordFollowup.Text += ((HiddenField)(GvFollowupBin.Rows[i].FindControl("hdntransIdFollowup"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecordFollowup.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvFollowupBin.Rows[i].FindControl("hdntransIdFollowup"))).Value.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecordFollowup.Text = temp;
            }
        }
    }
    protected void GvListData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dt_Sorting = Session["fillFollowupOppoList"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dt_Sorting = Session["fillFollowupQuoteList"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            dt_Sorting = Session["fillFollowupCustBalList"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dt_Sorting = Session["fillFollowupWorkOrderList"] as DataTable;
        }
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
        dt_Sorting = (new DataView(dt_Sorting, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            Session["fillFollowupOppoList"] = dt_Sorting;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            Session["fillFollowupQuoteList"] = dt_Sorting;
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            Session["fillFollowupCustBalList"] = dt_Sorting;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            Session["fillFollowupWorkOrderList"] = dt_Sorting;
        }

        objPageCmn.FillData((object)GvListData, dt_Sorting, "", "");
        dt_Sorting = null;
        //AllPageCode(hdnTableName.Value);

    }
    protected void GvListData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvListData.PageIndex = e.NewPageIndex;
        DataTable dtPaging = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dtPaging = Session["fillFollowupOppoList"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dtPaging = Session["fillFollowupQuoteList"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance" || hdnTableName.Value == "CRM Follow Up")
        {
            dtPaging = Session["fillFollowupCustBalList"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dtPaging = Session["fillFollowupWorkOrderList"] as DataTable;
        }

        objPageCmn.FillData((object)GvListData, dtPaging, "", "");
        dtPaging = null;
        //AllPageCode(hdnTableName.Value);
    }
    protected void ddlFieldNameFollowup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameFollowup.SelectedIndex == 0 || ddlFieldNameFollowup.SelectedIndex == 1)
        {
            txtValueFollowup.Visible = false;
            txtValueDateFollowup.Visible = true;
        }
        else
        {
            txtValueFollowup.Visible = true;
            txtValueDateFollowup.Visible = false;
        }
    }
    protected void GvFollowupBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvFollowupBin.PageIndex = e.NewPageIndex;
        DataTable dtPaging = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dtPaging = Session["fillFollowupOppoBin"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dtPaging = Session["fillFollowupQuoteBin"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance")
        {
            dtPaging = Session["fillFollowupCustBalBin"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dtPaging = Session["fillFollowupWorkOrderBin"] as DataTable;
        }

        objPageCmn.FillData((object)GvFollowupBin, dtPaging, "", "");
        for (int i = 0; i < GvFollowupBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvFollowupBin.Rows[i].FindControl("hdntransIdFollowup");
            string[] split = lblSelectedRecordFollowup.Text.Split(',');
            for (int j = 0; j < lblSelectedRecordFollowup.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecordFollowup.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecordFollowup.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvFollowupBin.Rows[i].FindControl("chkSelectFollowup")).Checked = true;
                    }
                }
            }
        }
        dtPaging = null;
    }
    protected void GvFollowupBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = new DataTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            dt_Sorting = Session["fillFollowupOppoBin"] as DataTable;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            dt_Sorting = Session["fillFollowupQuoteBin"] as DataTable;
        }
        if (hdnTableName.Value == "CustomerBalance")
        {
            dt_Sorting = Session["fillFollowupCustBalBin"] as DataTable;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            dt_Sorting = Session["fillFollowupWorkOrderBin"] as DataTable;
        }

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
        dt_Sorting = (new DataView(dt_Sorting, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
        {
            Session["fillFollowupOppoBin"] = dt_Sorting;
        }
        if (hdnTableName.Value == "Inv_SalesQuotationHeader")
        {
            Session["fillFollowupQuoteBin"] = dt_Sorting;
        }
        if (hdnTableName.Value == "CustomerBalance")
        {
            Session["fillFollowupCustBalBin"] = dt_Sorting;
        }
        if (hdnTableName.Value == "WorkOrder")
        {
            Session["fillFollowupWorkOrderBin"] = dt_Sorting;
        }

        objPageCmn.FillData((object)GvFollowupBin, dt_Sorting, "", "");
        dt_Sorting = null;
    }
    public bool checkValidation()
    {
        int parsedValue;
        if (txtFollowupDt.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Followup Date')", true);
            txtFollowupDt.Focus();
            return false;
        }
        if (txtContactList.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Contact Name')", true);
            txtContactList.Focus();
            return false;
        }
        if (txtCustomerName.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Customer Name')", true);
            txtCustomerName.Focus();
            return false;
        }
        if (txtGeneratedByfollowup.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Generated By Name')", true);
            txtGeneratedByfollowup.Focus();
            return false;
        }
        if (txtfollowupId.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Followup Id')", true);
            txtfollowupId.Focus();
            return false;
        }
        else
        {
            if (Lbl_Tab_New.Text == "New")
            {
                string count = Followupclass.CheckDuplicacyFollowupNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, txtfollowupId.Text);
                if (count != "" && count != "0")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myTimer()", true);
                    txtfollowupId.Focus();
                    txtfollowupId.Text = "";
                    lblfollowupDuplicacy.Visible = true;
                    return false;
                }
                else
                {
                    lblfollowupDuplicacy.Visible = false;
                }
            }
        }
        if (txtReferenceNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Reference No')", true);
            return false;
        }
        if (txtTypeReference.Text != "")
        {
            if (txtTypeReference.Text.Split('/')[1] != "")
            {
                if (!int.TryParse(txtTypeReference.Text.Split('/')[1], out parsedValue))
                {
                    txtTypeReference.Text = "";
                    txtTypeReference.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Must Enter Valid Reference Type')", true);
                    return false;
                }
                Session["strReferenceId"] = txtTypeReference.Text.Split('/')[1].ToString();
            }
        }
        else
        {
            //txtTypeReference.Text = "0";
            Session["strReferenceId"] = "0";
        }
        if (txtNextFollowUpDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Next Followup Date')", true);
            txtNextFollowUpDate.Focus();
            return false;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtNextFollowUpDate.Text);
            }
            catch
            {
                txtNextFollowUpDate.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Not a valid date')", true);
                return false;
            }
        }
        if (ObjSysParam.getDateForInput(txtNextFollowUpDate.Text) > ObjSysParam.getDateForInput(txtFollowupDt.Text))
        {
        }
        else
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Next Followup Date must be greater then Followup date')", true);
            txtNextFollowUpDate.Text = "";
            txtNextFollowUpDate.Focus();
            return false;
        }
        if (txtRemark.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Remarks')", true);
            txtRemark.Focus();
            return false;
        }
        return true;
    }
    protected void btnFollowupSave_Click(object sender, EventArgs e)
    {
        if (hdnFollowupLocationId.Value == "")
        {
            setLocationId();
        }
        btnFollowupSave.Enabled = false;
        string followupType;
        int refHeaderid = 0, refDetailid = 0;
        string pk_ID = "", reminder = "", Request_URL = "";
        if (checkValidation())
        {
            if (ddlFollowupType.SelectedValue == "Call" && txtTypeReference.Text == "")
            {
                btnGenerateCall_Click(null, null);
            }
            if (ddlFollowupType.SelectedValue == "Visit" && txtTypeReference.Text == "")
            {
                if (!btnGenerateVisit_Click())
                {
                    btnFollowupSave.Enabled = true;
                    return;
                }
            }
            if (hdnTableName.Value == "Inv_SalesInquiryHeader")
            {
                pk_ID = Session["Oppo_SInquiryID"].ToString();
                reminder = "Reminder for Opportunity(" + txtReferenceNo.Text + "), for Customer: " + lblHeaderR.Text + " and " + lblheaderTitle.Text;
                Request_URL = "../Sales/SalesInquiry.aspx?ReminderID=" + pk_ID;
            }
            if (hdnTableName.Value == "Inv_SalesQuotationHeader")
            {
                pk_ID = Session["Quote_SQuotation_Id"].ToString();
                reminder = "Reminder for Quotation(" + txtReferenceNo.Text + "), for Customer: " + lblHeaderR.Text + " and " + lblheaderTitle.Text;
                Request_URL = "../Sales/SalesQuotation.aspx?ReminderID=" + pk_ID;
            }
            if (hdnTableName.Value == "CustomerBalance")
            {
                pk_ID = Session["CustBal_CustID"].ToString();
                reminder = "Reminder for Customer Balance(" + txtReferenceNo.Text + "), for Customer: " + lblHeaderR.Text + " and " + lblheaderTitle.Text;
                Request_URL = "../CustomerReceivable/CustomerBalance.aspx?ReminderID=" + pk_ID;
            }

            if (hdnTableName.Value == "CRM Follow Up")
            {
                pk_ID = Session["CustBal_CustID"].ToString();
                reminder = "Reminder for CRM(" + txtReferenceNo.Text + "), for Customer: " + lblHeaderR.Text + " and " + lblheaderTitle.Text;
                Request_URL = "../CRM/CRM.aspx?ReminderID=" + pk_ID;
            }
            if (hdnTableName.Value == "WorkOrder")
            {
                pk_ID = Session["WorkOrder_CustID"].ToString();
                reminder = "Reminder for Work Order (" + txtReferenceNo.Text + "), for Customer: " + lblHeaderR.Text + " and " + lblheaderTitle.Text;
                Request_URL = "../ServiceManagement/WorkOrder.aspx?ReminderID=" + pk_ID;
            }
            //if (hdnTableName.Value == "Inv_SalesInquiryHeader")
            //{
            //DataTable dtEmpDtl = new DataTable();
            string id = Session["UserID"].ToString().ToLower();
            if (id == "superadmin")
            {
                id = "0";
            }
            //else
            //{
            //dtEmpDtl = ObjUser.GetUserMasterForUserName(id, Session["CompId"].ToString());
            //id = dtEmpDtl.Rows[0]["Emp_Id"].ToString();                
            //}
            if (ddlFollowupType.SelectedIndex == 0)
            {
                followupType = "";
            }
            else
            {
                followupType = ddlFollowupType.SelectedValue;
            }
            if (txtTypeReference.Text.Trim() == "")
            {
                txtTypeReference.Text = "0";
            }
            using (DataTable dtCount = Followupclass.GetCount())
            {
                if (Convert.ToInt32(dtCount.Rows[0][0].ToString()) == 0)
                {
                    txtfollowupId.Text = txtfollowupId.Text + "1";
                }
                else
                {
                    txtfollowupId.Text = txtfollowupId.Text + (Convert.ToInt32(dtCount.Rows[0][0].ToString()) + 1).ToString();
                }
            }
            CheckBox isdefault;
            int count = 0;
            DataTable dt_number = ContactNo1.getDatatable();
            for (int i = 0; i < dt_number.Rows.Count; i++)
            {
                if (dt_number.Rows[i]["Is_default"].ToString() == "True")
                {
                    count++;
                }
            }
            if (Lbl_Tab_New.Text == "New")
            {
                refHeaderid = Followupclass.insertHeaderData(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, txtfollowupId.Text, hdnTableName.Value, pk_ID.ToString(), txtFollowupDt.Text, hdncust_Id.Value, hdnContact_Id.Value, followupType, txtGeneratedByfollowup.Text.Split('/')[1].ToString(), txtRemark.Text, id, id, Session["strReferenceId"].ToString(), txtNextFollowUpDate.Text);
                objContactnoMaster.deteteDate("Ems_ContactMaster", hdnContact_Id.Value);
                new Inv_SalesInquiryHeader(Session["DBConnection"].ToString()).UpdateContactNameByTransId(pk_ID.ToString(), hdnContact_Id.Value);
                if (refHeaderid != 0)
                {
                    for (int i = 0; i < dt_number.Rows.Count; i++)
                    {
                        if (count == 0 && i == 0)
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", hdnContact_Id.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                        else
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", hdnContact_Id.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), dt_number.Rows[i]["Is_default"].ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                    }
                }
                if (hdnTableName.Value == "CustomerBalance")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Saved Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active1();DisplayMsg('Record Saved Successfully')", true);
                }
            }
            else
            {
                refHeaderid = Followupclass.UpdateHeaderData(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, hdntransID.Value, txtFollowupDt.Text, hdncust_Id.Value, hdnContact_Id.Value, followupType, txtGeneratedByfollowup.Text.Split('/')[1].ToString(), txtRemark.Text, id, id, Session["strReferenceId"].ToString(), txtNextFollowUpDate.Text);
                objContactnoMaster.deteteDate("Ems_ContactMaster", hdnContact_Id.Value);
                new Inv_SalesInquiryHeader(Session["DBConnection"].ToString()).UpdateContactNameByTransId(Session["strReferenceId"].ToString(), hdnContact_Id.Value);
                if (hdntransID.Value != "0")
                {
                    for (int i = 0; i < dt_number.Rows.Count; i++)
                    {
                        if (count == 0 && i == 0)
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", hdnContact_Id.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), "True", Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                        else
                        {
                            objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", hdnContact_Id.Value, dt_number.Rows[i]["Type"].ToString(), dt_number.Rows[i]["Country_code"].ToString(), dt_number.Rows[i]["Phone_no"].ToString(), dt_number.Rows[i]["Extension_no"].ToString(), dt_number.Rows[i]["Is_default"].ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
                        }
                    }
                }
                if (hdnTableName.Value == "CustomerBalance")
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Updated Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active1();DisplayMsg('Record Updated Successfully')", true);
                }
            }
            dt_number = null;
            if (chkReminder.Checked)
            {
                string date = System.DateTime.Now.ToString();
                int reminder_id = reminderClass.insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, hdnTableName.Value, pk_ID.ToString(), reminder, Request_URL, date, "1", txtNextFollowUpDate.Text, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                objReminderlog.insertLogData(reminder_id.ToString(), txtNextFollowUpDate.Text, "", Session["UserId"].ToString(), Session["UserId"].ToString());
            }
            reset();
            fillFollowupListSession(hdnTableName.Value);
            fillFollowupList(hdnTableName.Value);
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "btnSearchClick()", true);
            btnFollowupSave.Enabled = true;
        }
        else
        {
            btnFollowupSave.Enabled = true;
            return;
        }
    }
    private void WebUserControl_Followup_Click(object sender, ImageClickEventArgs e)
    {
        throw new NotImplementedException();
    }
    protected void Send_Notification_Task(string message, string id, string url)
    {
        int Save_Notification = 0;
        string Message = string.Empty;
        Message = message;
        GetEmployeeName(Session["EmpId"].ToString());
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Message, "38", url, hdnTableName.Value, id, "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
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
        Dt = null;
        return EmployeeName;
    }
    protected void BtnFollowupReset_Click(object sender, EventArgs e)
    {
        reset();
    }
    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        div_AddNewContact.Attributes["style"] = "display:block";
        txtContactList.Text = "";
        txtName.Focus();
        filGroup();
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "AddContact()", true);
    }
    protected void lnkViewDetailFollowup_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    protected void IbtnDeleteFollowup_Command(object sender, CommandEventArgs e)
    {
        int RefDtl = Followupclass.InActivateDataByTransID(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, e.CommandArgument.ToString());
        if (RefDtl == 1)
        {
            DisplayMessage("Record Deleted Successfully");
            //fillFollowupBinSession(hdnTableName.Value);
            //fillFollowupBin(hdnTableName.Value);
            fillFollowupListSession(hdnTableName.Value);
            fillFollowupList(hdnTableName.Value);
        }
    }
    protected void ddlFollowupType_SelectedIndexChanged(object sender, EventArgs e)
    {
        AutoCompleteExtenderCall.Enabled = false;
        AutoCompleteExtenderVisit.Enabled = false;
        txtTypeReference.Enabled = false;
        div_WorkOrder.Attributes["style"] = "display:none";
        if (ddlFollowupType.SelectedIndex == 1 || ddlFollowupType.SelectedIndex == 3)
        {
            if (ddlFollowupType.SelectedIndex == 1)
            {
                AutoCompleteExtenderCall.Enabled = true;
                AutoCompleteExtenderVisit.Enabled = false;
                txtTypeReference.Text = "";
            }
            else
            {
                AutoCompleteExtenderVisit.Enabled = true;
                AutoCompleteExtenderCall.Enabled = false;
                txtTypeReference.Text = "";
                div_WorkOrder.Attributes["style"] = "display:block";
            }
            txtTypeReference.Enabled = true;
        }
    }
    protected void btnQuote_Click(object sender, EventArgs e)
    {
        if (hdnTableName.Value == "Inv_SalesInquiryHeader")
            Response.Redirect("../Sales/SalesQuotation.aspx?InquiryId=" + Session["Oppo_SInquiryID"].ToString());
    }
    protected void btnGenerateCall_Click(object sender, EventArgs e)
    {
        int start_pos = txtCustomerName.Text.LastIndexOf("/") + 1;
        int last_pos = txtCustomerName.Text.Length;
        string id = txtCustomerName.Text.Substring(start_pos, last_pos - start_pos);
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CallRegister('"+ id + "')", true);
        //DataTable DtDataByID = objContact.GetContactTrueById(id);
        string txtCINo = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "270", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        int Call_id = ObjCustInquiry.InsertCallLogs(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, txtCINo.ToString(), System.DateTime.Now.ToString(), hdncust_Id.Value, hdnContact_Id.Value, hdnMob_no.Value, hdnEmail_Id.Value, "Sales Inquiry", "", Session["EmpId"].ToString(), "High", "Close", txtRemark.Text, "Not Generated", Session["EmpId"].ToString(), "", "", "True", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        ObjCustInquiry.Updatecode(Call_id.ToString(), txtCINo + Call_id);
        DataTable dt_callData = ObjCustInquiry.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, Call_id.ToString());
        txtTypeReference.Text = dt_callData.Rows[0]["Call_No"].ToString() + "/" + Call_id;
        Session["strReferenceId"] = Call_id.ToString();
        dt_callData = null;
        //Response.Redirect("../ServiceManagement/CallRegister.aspx?Followup_CustomerID=" + id);
    }
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), "Followup", "Sales", "Followup");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    protected bool btnGenerateVisit_Click()
    {
        if (txtVisitDate.Text.Trim() == "")
        {
            txtVisitDate.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Please Enter Visit Date')", true);
            return false;
        }
        if (txtInTime.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Please Enter In Time Details')", true);
            txtInTime.Focus();
            return false;
        }
        if (txtOuttime.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Please Enter Out Time Details')", true);
            txtOuttime.Focus();
            return false;
        }
        int start_pos = txtCustomerName.Text.LastIndexOf("/") + 1;
        int last_pos = txtCustomerName.Text.Length;
        string id = txtCustomerName.Text.Substring(start_pos, last_pos - start_pos);
        //DataTable DtDataByID = objContact.GetContactTrueById(id);
        string txtorderNo = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "323", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        int workOrderId = 0;
        workOrderId = ObjWorkOrder.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, txtorderNo, txtVisitDate.Text, "Direct", "", hdncust_Id.Value, Session["EmpId"].ToString(), "0", "", "In progress", txtRemark.Text, "", "0", "0", "0", txtInTime.Text, txtOuttime.Text, "0", "0", "0", hdnContact_Id.Value, "", false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "");
        ObjWorkOrder.Updatecode(workOrderId.ToString(), txtorderNo + workOrderId);
        DataTable dt_VisitData = ObjWorkOrder.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, workOrderId.ToString());
        txtTypeReference.Text = dt_VisitData.Rows[0]["Work_Order_No"].ToString() + "/" + workOrderId;
        Session["strReferenceId"] = workOrderId.ToString();
        txtVisitDate.Text = "";
        txtInTime.Text = "";
        txtOuttime.Text = "";
        dt_VisitData = null;
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "WorkOrder('" + id + "')", true);
        //Response.Redirect("../ServiceManagement/WorkOrder.aspx?Followup_CustomerID=" + id);
        return true;
    }
    protected void txtfollowupId_TextChanged(object sender, EventArgs e)
    {
        string count = Followupclass.CheckDuplicacyFollowupNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnFollowupLocationId.Value, txtfollowupId.Text);
        if (count != "" && count != "0")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myTimer()", true);
            txtfollowupId.Focus();
            txtfollowupId.Text = "";
            lblfollowupDuplicacy.Visible = true;
        }
        else
        {
            lblfollowupDuplicacy.Visible = false;
        }
    }
    public void setCountryCode_ContactNo()
    {
        string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), hdnFollowupLocationId.Value).Rows[0]["Field1"].ToString();
        string Country_Id = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
        string CountryCode = objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        ContactNo1.setCountryCode("+" + CountryCode);
    }
    public void filGroup()
    {
        DataTable dtGroupdate = Followupclass.getAllGroupName();
        dtGroupdate = new DataView(dtGroupdate, "group_Id<>1 and group_Id<>2", "", DataViewRowState.CurrentRows).ToTable();
        ddlGroup.DataSource = dtGroupdate;
        ddlGroup.DataTextField = "group_name";
        ddlGroup.DataValueField = "group_Id";
        ddlGroup.DataBind();
        ddlGroup.Items.Insert(0, new ListItem("Select", "0"));
        dtGroupdate = null;
    }
    protected void BtnSaveContact_Click(object sender, EventArgs e)
    {
        if (txtName.Text.Trim() == "")
        {
            txtName.Focus();
            return;
        }
        if (txtEmail.Text.Trim() == "")
        {
            txtEmail.Focus();
            return;
        }
        if (txtMob.Text.Trim() == "")
        {
            txtMob.Focus();
            return;
        }
        if (txtDepartment.Text.Trim() == "")
        {
            txtDepartment.Focus();
            return;
        }
        if (ddlGroup.SelectedIndex == 0)
        {
            ddlGroup.Focus();
            return;
        }
        int parsedValue;
        if (!int.TryParse(txtDepartment.Text.Split('/')[1].ToString(), out parsedValue))
        {
            txtDepartment.Focus();
            return;
        }
        string txtId = objDocNo.GetDocumentNo(true, "0", false, "8", "19", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), hdnFollowupLocationId.Value).Rows[0]["Field1"].ToString();
        string Country_Id = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
        string CountryCode = objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        int T_id = objContact.InsertContactMaster(txtId.Trim(), txtName.Text.Trim(), txtName.Text, "", txtDepartment.Text.Split('/')[1].ToString(), "0", "0", hdncust_Id.Value, "true", "true", "Individual", "False", txtEmail.Text, "+" + CountryCode + txtMob.Text, ddlSalutation.SelectedItem.ToString(), Country_Id, strCurrencyId, "false", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0");
        objContact.UpdateContactMaster(T_id.ToString(), txtId + T_id.ToString());
        txtContactList.Text = txtName.Text + "/(" + txtDepartment.Text.Split('/')[0].ToString() + ")/" + T_id.ToString();
        hdnContact_Id.Value = T_id.ToString();
        hdnEmail_Id.Value = txtEmail.Text;
        hdnMob_no.Value = txtMob.Text;
        int emailHeaderID = objEmailHeader.ES_EmailMasterHeader_Insert(txtEmail.Text, Country_Id, "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        objEmailDetail.ES_EmailMasterDetail_Insert(hdnContact_Id.Value.ToString(), "Contact", emailHeaderID.ToString(), "true", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        objCG.InsertContactGroup(hdnContact_Id.Value, ddlGroup.SelectedValue, "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
        ContactNo1.setNullToGV();
        (ContactNo1.FindControl("txtNumber") as TextBox).Text = txtMob.Text;
        ContactNo1.btnAddNum_Click(null, null);
        txtName.Text = "";
        txtEmail.Text = "";
        txtMob.Text = "";
        txtDepartment.Text = "";
        ddlSalutation.SelectedIndex = 0;
    }
    public void ResetFollowupType()
    {
        ddlFollowupType.SelectedIndex = 0;
        ddlFollowupType_SelectedIndexChanged(null, null);
        div_AddNewContact.Attributes["style"] = "display:none";
        if (hdnTableName.Value == "CustomerBalance")
        {
            product_div.Attributes["style"] = "display:none";
        }
        else
        {
            product_div.Attributes["style"] = "display:block";
        }
    }
    protected void txtDepartment_TextChanged(object sender, EventArgs e)
    {
        if (txtDepartment.Text.Trim() != "")
        {
            int start_pos = txtDepartment.Text.LastIndexOf("/") + 1;
            int last_pos = txtDepartment.Text.Length;
            string id = txtDepartment.Text.Substring(start_pos, last_pos - start_pos);
            if (start_pos == 0)
            {
                txtDepartment.Text = "";
                txtDepartment.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select from suggestions only.')", true);
                return;
            }
            DataTable dt = new DepartmentMaster(Session["DBConnection"].ToString()).GetDepartmentMaster();
            dt = new DataView(dt, "Dep_Id = " + id + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Dep_Name = '" + txtDepartment.Text.Split('/')[0].Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    txtDepartment.Text = "";
                    txtDepartment.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select from suggestions only.')", true);
                }
            }
            else
            {
                txtDepartment.Text = "";
                txtDepartment.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select from suggestions only.')", true);
            }
        }
    }
    protected void Btn_FollowupList1_Click(object sender, EventArgs e)
    {
        fillFollowupListSession(hdnTableName.Value);
        fillFollowupList(hdnTableName.Value);
    }
    protected void Btn_Bin1_Click(object sender, EventArgs e)
    {
        fillFollowupBinSession(hdnTableName.Value);
        fillFollowupBin(hdnTableName.Value);
    }
    public void setLocationId(string locationid = "")
    {
        if (locationid != "")
        {
            hdnFollowupLocationId.Value = locationid;
        }
        else
        {
            hdnFollowupLocationId.Value = Session["LocId"].ToString();
        }
    }
}