using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
public partial class WebUserControl_ContactInfo : System.Web.UI.UserControl
{
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
        DataAccessClass objDa = null;
    EmployeeMaster objEmployee = null;
    UserMaster ObjUser = null;
    Common cmn =null;
    Inv_UnitMaster UM = null;
    IT_ObjectEntry objObjectEntry = null;
    Reminder reminderClass = null;
    NotificationMaster Obj_Notifiacation = null;
    ReminderLogs objReminderlog = null;
    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    CountryMaster objCountryMaster = null;
    ContactNoMaster objContactnoMaster = null;
    ES_EmailMaster_Header objEmailHeader = null;
    Ems_Contact_Group objCG = null;
    ES_EmailMasterDetail objEmailDetail = null;
    Set_DocNumber objDocNo = null;
    Set_CustomerMaster objCustomer = null;
    Set_CustomerMaster_CreditParameter objCustomerCreditParam = null;
    CurrencyMaster objCurrency = null;
    PageControlCommon objPageCmn = null;
    Ems_ContactCompanyBrand ObjCompanyBrand = null;
    public static string customerID = "";
    static string strReferenceId = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        reminderClass = new Reminder(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objReminderlog = new ReminderLogs(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objCustomerCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Ems_ContactCompanyBrand(Session["DBConnection"].ToString());
        hdnTableName = (HiddenField)Parent.FindControl("hdnFollowupTableName");
        if (!IsPostBack)
        {
            Lbl_Tab_New.Text = "New";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Li_Tab_ContactList()", true);
            fillGroup();
        }
    }
    public static string getCustid()
    {
        if (customerID == "")
        {
            return "0";
        }
        else
        {
            return customerID;
        }
    }
    public void fillHeader(DataTable DtAllData)
    {
        if (DtAllData.Rows.Count > 0)
        {
            lblHeaderL.Text = "Contact List";
            lblHeaderR.Text = DtAllData.Rows[0]["Name"].ToString();
            hdncust_Id.Value = DtAllData.Rows[0]["Trans_id"].ToString();
            //lblheaderTitle.Text = "" + DtAllData.Rows[0]["Opportunity_name"].ToString() + " (" + DtAllData.Rows[0]["Currency_Code"].ToString() + " " + DtAllData.Rows[0]["Opportunity_amount"].ToString() + ") Generated On:  " + GetDate(DtAllData.Rows[0]["IDate"].ToString());
        }
    }
    public void fillFollowupList(string strPartyId = "0")
    {
        if (strPartyId == "0")
        {
            GvListData.DataSource = null;
            GvListData.DataBind();
            return;
        }
        DataTable DtAllData = new DataTable();
        string sql = "select ems_contactmaster.name,ems_contactmaster.field2 as ContactNo,Set_DesignationMaster.designation,Set_DepartmentMaster.Dep_Name, STUFF(( select ',' + ES_EmailMaster_Header.Email_Id from ES_EmailMaster_Header inner join ES_EmailMaster_Detail on ES_EmailMaster_Detail.email_ref_id=ES_EmailMaster_Header.Trans_Id where ES_EmailMaster_Detail.Ref_Id=Ems_ContactMaster.trans_id FOR XML PATH('') ), 1, 1, '') as email from ems_contactmaster left join Set_DepartmentMaster on Set_DepartmentMaster.dep_id=ems_contactmaster.dep_id left join Set_DesignationMaster on Set_DesignationMaster.Designation_Id=ems_contactmaster.Designation_Id where Company_Id='" + strPartyId + "'";
        DtAllData = objDa.return_DataTable(sql);
        if (DtAllData != null)
        {
            Session["company_contact_list"] = DtAllData;
        }
        if (DtAllData != null && DtAllData.Rows.Count > 0)
        {
            GvListData.DataSource = DtAllData;
            GvListData.DataBind();
            lblTotalRecordsFollowup.Text = "Total Records: " + DtAllData.Rows.Count.ToString();
        }
        else
        {
            GvListData.DataSource = null;
            GvListData.DataBind();
            lblTotalRecordsFollowup.Text = "Total Records: 0";
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCust = objcustomer.GetCustomerRecAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] filterlist = new string[dtCust.Rows.Count];
            if (dtCust.Rows.Count > 0)
            {
                for (int i = 0; i < dtCust.Rows.Count; i++)
                {
                    filterlist[i] = dtCust.Rows[i]["Name"].ToString() + "/" + dtCust.Rows[i]["Customer_Id"].ToString();
                }
            }
            dtCust = null;
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

            using (DataTable dt_Contact = ObjContactMaster.GetContactAsPerFilterText(prefixText, id))
            {
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
                    using (DataTable dtcon = ObjContactMaster.GetContactTrueById(id))
                    {
                        string[] filterlistcon = new string[dtcon.Rows.Count];
                        for (int i = 0; i < dtcon.Rows.Count; i++)
                        {
                            filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
                        }
                        return filterlistcon;
                    }
                }
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
        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }
        dt = null;
        return txt;
    }
    public void reset()
    {
        Lbl_Tab_New.Text = "New";
        hdntransID.Value = "";
    }
    protected void GvListData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = new DataTable();
        dt_Sorting = Session["company_contact_list"] as DataTable;
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
        Session["company_contact_list"] = dt_Sorting;
        objPageCmn.FillData((object)GvListData, dt_Sorting, "", "");
        // AllPageCode(hdnTableName.Value);
    }
    protected void GvListData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvListData.PageIndex = e.NewPageIndex;
        DataTable dtPaging = new DataTable();
        dtPaging = Session["company_contact_list"] as DataTable;
        objPageCmn.FillData((object)GvListData, dtPaging, "", "");
        //AllPageCode(hdnTableName.Value);
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
        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Message, "38", url, hdnTableName.Value, id, "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), "0", "17");
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
    public void setCountryCode_ContactNo()
    {
        string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string Country_Id = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
        string CountryCode = objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        ContactNo1.setCountryCode("+" + CountryCode);
    }
    public void fillGroup()
    {
        try
        {
            DataTable dtGroupdate = new FollowUp(Session["DBConnection"].ToString()).getAllGroupName();
            dtGroupdate = new DataView(dtGroupdate, "group_Id<>1 and group_Id<>2", "group_name", DataViewRowState.CurrentRows).ToTable();
          
            ddlGroup.DataSource = dtGroupdate;
            ddlGroup.DataTextField = "group_name";
            ddlGroup.DataValueField = "group_Id";
            ddlGroup.DataBind();
            ddlGroup.Items.Insert(0, new ListItem("Select", "0"));
            dtGroupdate = null;
        }
        catch
        {
        }
    }
    protected void BtnSaveContact_Click(object sender, EventArgs e)
    {
        if (txtName.Text.Trim() == "")
        {
            txtName.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Name')", true);
            return;
        }
        if (txtDesignation.Text.Trim() == "")
        {
            txtDesignation.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Designation.')", true);
            return;
        }
        if (txtEmail.Text.Trim() == "")
        {
            txtEmail.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Email.')", true);
            return;
        }
        if (txtMob.Text.Trim() == "")
        {
            txtMob.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Mobile No.')", true);
            return;
        }
        if (txtDepartment.Text.Trim() == "")
        {
            txtDepartment.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Department.')", true);
            return;
        }
        if (ddlGroup.SelectedIndex == 0)
        {
            ddlGroup.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select Group.')", true);
            return;
        }
        int parsedValue;
        if (!int.TryParse(txtDesignation.Text.Split('/')[1].ToString(), out parsedValue))
        {
            txtDesignation.Focus();
            return;
        }
        if (!int.TryParse(txtDepartment.Text.Split('/')[1].ToString(), out parsedValue))
        {
            txtDepartment.Focus();
            return;
        }
        string txtId = objDocNo.GetDocumentNo(true, "0", false, "8", "19", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        //string txtId = string.Empty;
        string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string Country_Id = objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString();
        string CountryCode = objCountryMaster.GetCountryMasterById(objCountryCurrency.GetCurrencyByCountryId(strCurrencyId, "2").Rows[0]["Country_Id"].ToString()).Rows[0]["Country_Code"].ToString();
        int T_id = objContact.InsertContactMaster(txtId.Trim(), txtName.Text.Trim(), txtName.Text, "", txtDepartment.Text.Split('/')[1].ToString(), txtDesignation.Text.Split('/')[1].Trim(), "0", hdncust_Id.Value, "true", "true", "Individual", "False", txtEmail.Text,"+" + CountryCode + txtMob.Text, ddlSalutation.SelectedItem.ToString(), Country_Id, strCurrencyId, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0");
        objContact.UpdateContactMaster(T_id.ToString(), txtId + T_id.ToString());

        objContactnoMaster.insertDate(Session["CompId"].ToString(), "Ems_ContactMaster", T_id.ToString(), "Mobile", CountryCode, txtMob.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        //txtContactList.Text = txtName.Text + "/(" + txtDepartment.Text.Split('/')[0].ToString() + ")/" + T_id.ToString();
        hdnContact_Id.Value = T_id.ToString();
        hdnEmail_Id.Value = txtEmail.Text;
        hdnMob_no.Value = txtMob.Text;
        int emailHeaderID = objEmailHeader.ES_EmailMasterHeader_Insert(txtEmail.Text, Country_Id, "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        objEmailDetail.ES_EmailMasterDetail_Insert(hdnContact_Id.Value.ToString(), "Contact", emailHeaderID.ToString(), "true", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        objCG.InsertContactGroup(hdnContact_Id.Value, ddlGroup.SelectedValue, "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
        ObjCompanyBrand.InsertContactCompanyBrand(hdnContact_Id.Value.Trim(),Session["CompId"].ToString(), Session["BrandId"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Contact Person Added Successfully')", true);
        ContactNo1.setNullToGV();
        (ContactNo1.FindControl("txtNumber") as TextBox).Text = txtMob.Text;
        ContactNo1.btnAddNum_Click(null, null);
        txtName.Text = "";
        txtEmail.Text = "";
        txtMob.Text = "";
        txtDesignation.Text = "";
        txtDepartment.Text = "";
        ddlSalutation.SelectedIndex = 0;
        ddlGroup.SelectedIndex = 0;
        fillFollowupList(hdncust_Id.Value);
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
    protected void txtDesignation_TextChanged(object sender, EventArgs e)
    {
        if (txtDesignation.Text.Trim() != "")
        {
            int start_pos = txtDesignation.Text.LastIndexOf("/") + 1;
            int last_pos = txtDesignation.Text.Length;
            string id = txtDesignation.Text.Substring(start_pos, last_pos - start_pos);
            if (start_pos == 0)
            {
                txtDesignation.Text = "";
                txtDesignation.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select from suggestions only.')", true);
                return;
            }
            DataTable dt = new DesignationMaster(Session["DBConnection"].ToString()).GetDesignationDataByName(txtDesignation.Text.Split('/')[0].Trim());
            dt = new DataView(dt, "Designation_Id = " + id + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Designation = '" + txtDesignation.Text.Split('/')[0].Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count == 0)
                {
                    txtDesignation.Text = "";
                    txtDesignation.Focus();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select from suggestions only.')", true);
                }
            }
            else
            {
                txtDesignation.Text = "";
                txtDesignation.Focus();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select from suggestions only.')", true);
            }
        }
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

    public void setLocationId(string locationid = "")
    {
        if (locationid != "")
        {
            hdnLocationId.Value = locationid;
        }
        else
        {
            hdnLocationId.Value = Session["LocId"].ToString();
        }
    }

    //add all these function on parent .cs page to access autocomplete list

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    //{
    //    //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
    //    DataTable dt = new DepartmentMaster().GetDepartmentListPreText(prefixText);
    //    string[] str = new string[dt.Rows.Count];
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
    //    }
    // dt=null; 
    //    return str;
    //}
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    //{
    //    ES_EmailMaster_Header Email = new ES_EmailMaster_Header();
    //    DataTable dt = Email.GetEmailIdPreFixText(prefixText);
    //    string[] str = new string[dt.Rows.Count];
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        str[i] = dt.Rows[i]["Email_Id"].ToString();
    //    }
    //      dt=null;
    //    return str;
    //}
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    //{
    //    DataTable dt = new DesignationMaster().GetDesignationDataPreText(prefixText);
    //    string[] str = new string[dt.Rows.Count];
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
    //    }
    //      dt=null;
    //    return str;
    //}
    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    //{
    //    ContactNoMaster objContactNumMaster = new ContactNoMaster();
    //    DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
    //    string[] txt = new string[dt.Rows.Count];
    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["Phone_no"].ToString();
    //    }
    //    dt=null;
    //    return txt;
    //}
}