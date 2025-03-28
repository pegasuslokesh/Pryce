using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

// created by Divya Parakh date-15/9/2018

public partial class WebUserControl_crm_agreement : System.Web.UI.UserControl
{

    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmployee = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Common cmn = null;
    Document_Master ObjDocument = null;
    CountryMaster objCountryMaster = null;
    Set_AddressChild address = null;
    CRM_Agreements obj_agreement = null;
    CRM_Agreements_Product obj_agreement_product = null;
    Inv_ProductMaster obj_ProductMaster = null;
    Inv_ProductCategoryMaster obj_ProductCategory = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        objCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        address = new Set_AddressChild(Session["DBConnection"].ToString());
        obj_agreement = new CRM_Agreements(Session["DBConnection"].ToString());
        obj_agreement_product = new CRM_Agreements_Product(Session["DBConnection"].ToString());
        obj_ProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        obj_ProductCategory = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        hdnTableName = (HiddenField)Parent.FindControl("hdncrm_agreementTableName");
        txtValueDateBincrm_agreement.Attributes.Add("readonly", "true");
        txtValueDatecrm_agreement.Attributes.Add("readonly", "true");
        txtfromDate.Attributes.Add("readonly", "true");
        txtToDate.Attributes.Add("readonly", "true");
        txtAgreementDate.Attributes.Add("readonly", "true");

        if (!IsPostBack)
        {
            Lbl_Tab_New.Text = "New";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active1()", true);
            SetGeneratedByName();
            FillCountry();
            fillProductCategory();
        }

    }

    public void SetGeneratedByName()
    {
        if (txtHandledBy.Text == "")
        {
            if (Session["UserId"].ToString() == "superadmin")
            {
                txtHandledBy.Text = "";
            }
            else
            {
                DataTable dtEmployeeDtl = ObjUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
                txtHandledBy.Text = dtEmployeeDtl.Rows[0]["EmpName"].ToString() + "/" + dtEmployeeDtl.Rows[0]["Emp_Id"].ToString();
            }
        }
    }

    public void GetDocumentNumber()
    {
        txtAgreementNo.Text = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "171", "398", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        txtAgreementDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
    }

    public void AllPageCode()
    {

        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = new IT_ObjectEntry(Session["DBConnection"].ToString()).GetModuleIdAndName("57", (DataTable)Session["ModuleName"]);
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
        Page.Title = ObjSysParam.GetSysTitle();
        if (Session["EmpId"].ToString() == "0")
        {
            if (Lbl_Tab_New.Text == Resources.Attendance.New)
            {
                btncrm_agreementSave.Visible = true;
                //btnSQuoteSaveandPrint.Visible = true;
            }
            GvListData.Columns[0].Visible = true;
            GvListData.Columns[1].Visible = true;
            GvListData.Columns[2].Visible = true;

            imgBtnRestorecrm_agreement.Visible = true;
            ImgbtnSelectAllcrm_agreement.Visible = true;

        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "57",Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "1")
            {
                if (Lbl_Tab_New.Text == Resources.Attendance.New || Lbl_Tab_New.Text == Resources.Attendance.Edit)
                {
                    btncrm_agreementSave.Visible = true;
                }

            }

            //for edit
            if (DtRow["Op_Id"].ToString() == "2")
            {
                GvListData.Columns[1].Visible = true;
            }

            //for delete
            if (DtRow["Op_Id"].ToString() == "3")
            {
                GvListData.Columns[2].Visible = true;
            }

            if (DtRow["Op_Id"].ToString() == "4")
            {
                imgBtnRestorecrm_agreement.Visible = true;
                ImgbtnSelectAllcrm_agreement.Visible = true;
            }
            //for view
            if (DtRow["Op_Id"].ToString() == "5")
            {
                GvListData.Columns[0].Visible = true;
            }
        }

    }

    public void fillHeader(string agreementName, string PartyName)
    {
        lblHeaderL.Text = "Agreement (" + agreementName + ")";
        lblHeaderR.Text = PartyName;
        //lblheaderTitle.Text = "" + DtAllData.Rows[0]["Opportunity_name"].ToString() + " (" + DtAllData.Rows[0]["Currency_Code"].ToString() + " " + DtAllData.Rows[0]["Opportunity_amount"].ToString() + ") Generated On:  " + GetDate(DtAllData.Rows[0]["IDate"].ToString());
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
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "Emp_Name like '%" + prefixText.ToString() + "%'", "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
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

    protected void txtContactList_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactName.Text != "")
        {
            strCustomerId = GetContactId();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                hdnContact_Id.Value = strCustomerId;
                txtContactName.Focus();

            }
            else
            {
                //DisplayMessage("Select In Suggestions Only");
                txtContactName.Text = "";
                txtContactName.Focus();
                return;
            }
        }

    }

    private string GetContactId()
    {
        string retval = "";
        try
        {
            if (txtContactName.Text != "")
            {
                int start_pos = txtContactName.Text.LastIndexOf("/") + 1;
                int last_pos = txtContactName.Text.Length;
                string id = txtContactName.Text.Substring(start_pos, last_pos - start_pos);

                int Last_pos_name = txtContactName.Text.LastIndexOf("/");
                string name = txtContactName.Text.Substring(0, Last_pos_name - 0);

                //DataTable dtSupp = objContact.GetContactByContactName(txtContactList.Text.Trim().Split('/')[0].ToString());
                DataTable dtData = objContact.GetContactTrueById(id);
                if (dtData.Rows.Count > 0)
                {
                    string condition = "Name like'%" + name + "%'";
                    dtData = new DataView(dtData, condition, "", DataViewRowState.CurrentRows).ToTable();
                    //DataTable dtCompany = objContact.GetContactTrueById(retval);
                    if (dtData.Rows.Count > 0)
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

    protected void lnkcustomerHistory_OnClick(object sender, EventArgs e)
    {
        string Id = string.Empty;

        try
        {
            //Id = txtCustomerName.Text.Split('/')[1].ToString();
        }
        catch
        {
            Id = "0";
        }



        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "CustomerHistory(" + Id + ")", true);

    }

    protected void btnRefreshcrm_agreement_Click(object sender, ImageClickEventArgs e)
    {
        Session["gv_AgreementListData"] = null;

        fillGridSession(Session["ContactId"].ToString());
        fillGrid();
        txtValueDatecrm_agreement.Text = "";
        txtValuecrm_agreement.Text = "";
    }

    protected void btnListSearch_Click(object sender, ImageClickEventArgs e)
    {
        fillGridSession(Session["ContactId"].ToString());

        DataTable dtAdd = new DataTable();

        dtAdd = Session["gv_AgreementListData"] as DataTable;

        if (ddlOptioncrm_agreement.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlFieldNamecrm_agreement.SelectedItem.Value == "crm_agreement_date" || ddlFieldNamecrm_agreement.SelectedItem.Value == "Next_crm_agreement_date")
            {
                if (txtValueDatecrm_agreement.Text.Trim() != "")
                {
                    if (ddlOptioncrm_agreement.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNamecrm_agreement.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDatecrm_agreement.Text.Trim()) + "'";
                    }
                    else if (ddlOptioncrm_agreement.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNamecrm_agreement.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDatecrm_agreement.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNamecrm_agreement.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDatecrm_agreement.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Date");
                    txtValueDatecrm_agreement.Focus();
                    return;
                }
            }
            else
            {
                if (txtValuecrm_agreement.Text.Trim() != "")
                {
                    if (ddlOptioncrm_agreement.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNamecrm_agreement.SelectedValue + ",System.String)='" + txtValuecrm_agreement.Text.Trim() + "'";
                    }
                    else if (ddlOptioncrm_agreement.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNamecrm_agreement.SelectedValue + ",System.String) like '%" + txtValuecrm_agreement.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNamecrm_agreement.SelectedValue + ",System.String) Like '%" + txtValuecrm_agreement.Text.Trim() + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Some Value");
                    txtValuecrm_agreement.Focus();
                }
            }
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvListData, view.ToTable(), "", "");
            Session["gv_AgreementListData"] = view.ToTable();


            lblTotalRecordscrm_agreement.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

        }
    }

    public void reset()
    {
        //GvProductDatacrm_agreement.DataSource = null;
        Lbl_Tab_New.Text = "New";
        //GvProductDatacrm_agreement.DataBind();
        hdntransID.Value = "";
        chkReminder.Checked = false;
        GetDocumentNumber();
        SetGeneratedByName();
        txtAgreementNo.Text = "";
        txtAgreementDate.Text = "";
        txtTerms_Conditions.Text = "";
        txtContactName.Text = "";
        txtEmailAddress.Text = "";
        txtMobileNo.Text = "";
        ddlAddress.SelectedIndex = 0;
        ddlCountry.SelectedIndex = 0;
        txtState.Text = "";
        txtCity.Text = "";
        txtfromDate.Text = "";
        txtToDate.Text = "";
        txtHandledBy.Text = "";
        hdnstateId.Value = "";
        hdncityId.Value = "";
        hdnContact_Id.Value = "";
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //fileupload_divcrm_agreement.Visible = true;
        ImageButton b = (ImageButton)sender;
        string objSenderID = b.ID;
        hdntransID.Value = e.CommandArgument.ToString();


        DataTable dtAllData = obj_agreement.getActiveDataByTrans_Id(hdntransID.Value);
        if (dtAllData.Rows.Count > 0)
        {
            if (objSenderID == "lnkViewDetailcrm_agreement")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;

                btncrm_agreementSave.Visible = false;
                Btncrm_agreementReset.Visible = false;
            }

            if (b.ToolTip == "Edit")
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                Btncrm_agreementReset.Visible = true;
                btncrm_agreementSave.Visible = true;
            }
            else 
            {
                Lbl_Tab_New.Text = "Renew";
                Btncrm_agreementReset.Visible = true;
                btncrm_agreementSave.Visible = true;
            }

            fillControlsValue(dtAllData);

            DataTable dt_product = obj_agreement_product.getAllActiveDataByTrans_Id(hdntransID.Value);
            if (dt_product.Rows.Count > 0)
            {
                fillProductListData(dt_product);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active_agreement()", true);
        }



    }

    public void fillControlsValue(DataTable DtAgreementdata)
    {
        txtAgreementNo.Text = DtAgreementdata.Rows[0]["Agreement_No"].ToString();
        txtAgreementDate.Text = GetDate(DtAgreementdata.Rows[0]["Agreement_Date"].ToString());
        txtContactName.Text = DtAgreementdata.Rows[0]["contactName"].ToString() + "/" + DtAgreementdata.Rows[0]["Contact_Id"].ToString();
        hdnContact_Id.Value = DtAgreementdata.Rows[0]["Contact_Id"].ToString();
        txtEmailAddress.Text = DtAgreementdata.Rows[0]["Email_Address"].ToString();
        txtMobileNo.Text = DtAgreementdata.Rows[0]["Mobile_No"].ToString();
        ddlAddress.SelectedValue = DtAgreementdata.Rows[0]["AddressId"].ToString();
        ddlCountry.SelectedValue = DtAgreementdata.Rows[0]["Country_Id"].ToString();
        txtState.Text = DtAgreementdata.Rows[0]["State_Name"].ToString();
        hdnstateId.Value = DtAgreementdata.Rows[0]["State_Id"].ToString();
        txtCity.Text = DtAgreementdata.Rows[0]["City_Name"].ToString();
        hdncityId.Value = DtAgreementdata.Rows[0]["City_Id"].ToString();
        txtfromDate.Text = GetDate(DtAgreementdata.Rows[0]["From_Date"].ToString());
        txtToDate.Text = GetDate(DtAgreementdata.Rows[0]["To_Date"].ToString());
        txtHandledBy.Text = DtAgreementdata.Rows[0]["handledByName"].ToString() + "/" + DtAgreementdata.Rows[0]["Handled_By"].ToString();

        txtTerms_Conditions.Text = DtAgreementdata.Rows[0]["Terms_Condition"].ToString();
        txtSecurityAmt.Text = DtAgreementdata.Rows[0]["Security_Amount"].ToString();
    }

    public void fillProductListData(DataTable dtProductdata)
    {
        GvProductData.DataSource = dtProductdata;
        GvProductData.DataBind();
    }

    protected void ddlFieldNameBincrm_agreement_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBincrm_agreement.SelectedIndex == 0)
        {
            txtValueBincrm_agreement.Visible = false;
            txtValueDateBincrm_agreement.Visible = true;
        }
        else
        {
            txtValueBincrm_agreement.Visible = true;
            txtValueDateBincrm_agreement.Visible = false;
        }
    }

    protected void btnsearchBincrm_agreement_Click(object sender, ImageClickEventArgs e)
    {
        fillBinGridSession(Session["ContactId"].ToString());
        DataTable dtAdd = new DataTable();
        dtAdd = Session["gv_AgreementBinData"] as DataTable;

        if (ddlOptionBincrm_agreement.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlFieldNameBincrm_agreement.SelectedItem.Value == "crm_agreement_date")
            {
                if (txtValueDateBincrm_agreement.Text.Trim() != "")
                {
                    if (ddlOptionBincrm_agreement.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameBincrm_agreement.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDateBincrm_agreement.Text.Trim()) + "'";
                    }
                    else if (ddlOptionBincrm_agreement.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameBincrm_agreement.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDateBincrm_agreement.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameBincrm_agreement.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDateBincrm_agreement.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Date");
                    txtValueDateBincrm_agreement.Focus();
                    return;
                }
            }
            else
            {
                if (txtValueBincrm_agreement.Text.Trim() != "")
                {
                    if (ddlOptionBincrm_agreement.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameBincrm_agreement.SelectedValue + ",System.String)='" + txtValueBincrm_agreement.Text.Trim() + "'";
                    }
                    else if (ddlOptionBincrm_agreement.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameBincrm_agreement.SelectedValue + ",System.String) like '%" + txtValueBincrm_agreement.Text.Trim() + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameBincrm_agreement.SelectedValue + ",System.String) Like '%" + txtValueBincrm_agreement.Text.Trim() + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Some Value");
                    txtValueBincrm_agreement.Focus();
                }
            }
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvBinData, view.ToTable(), "", "");
            Session["gv_AgreementBinData"] = view.ToTable();

            lblTotalRecordsBincrm_agreement.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
    }

    protected void btnRefreshBincrm_agreement_Click(object sender, ImageClickEventArgs e)
    {
        Session["gv_AgreementBinData"] = null;
        fillBinGridSession(Session["ContactId"].ToString());
        fillBinGrid();
        txtValueBincrm_agreement.Text = "";
        txtValueDateBincrm_agreement.Text = "";
    }

    protected void imgBtnRestorecrm_agreement_Click(object sender, ImageClickEventArgs e)
    {

        if (lblSelectedRecords.Text != "")
        {
            for (int j = 0; j < lblSelectedRecords.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecords.Text.Split(',')[j] != "")
                {
                    obj_agreement.ActivateDataByTransId(lblSelectedRecords.Text.Split(',')[j].Trim());
                    //DisplayMessage("Record Activate");
                }
            }
        }

        int fleg = 0;
        foreach (GridViewRow Gvr in GvBinData.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)Gvr.FindControl("chkSelectcrm_agreement");
            if (chk.Checked)
            {
                fleg = 1;
            }
            else
            {
                fleg = 0;
            }
        }
        Session["gv_AgreementBinData"] = null;
        Session["gv_AgreementListData"] = null;

        fillBinGridSession(Session["ContactId"].ToString());
        fillBinGrid();
        fillGridSession(Session["ContactId"].ToString());
        fillGrid();
    }

    protected void ImgbtnSelectAllcrm_agreement_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = new DataTable();
        dtPbrand = Session["gv_AgreementBinData"] as DataTable;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecords.Text.Split(',').Contains(dr["Trans_id"]))
                {
                    lblSelectedRecords.Text += dr["Trans_id"] + ",";
                }
            }
            for (int i = 0; i < GvBinData.Rows.Count; i++)
            {
                string[] split = lblSelectedRecords.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvBinData.Rows[i].FindControl("hdntransIdcrm_agreement");
                for (int j = 0; j < lblSelectedRecords.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecords.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecords.Text.Split(',')[j].Trim().ToString())
                        {
                            ((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[i].FindControl("chkSelectcrm_agreement")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecords.Text = "";

            DataTable dtSelectedData = new DataTable();
            dtSelectedData = Session["gv_AgreementBinData"] as DataTable;
            objPageCmn.FillData((object)GvBinData, dtSelectedData, "", "");
            ViewState["Select"] = null;
        }

    }

    protected void chkSelectcrm_agreement_CheckedChanged(object sender, EventArgs e)
    {
        string crm_agreementidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((System.Web.UI.WebControls.CheckBox)sender).Parent.Parent).RowIndex;
        System.Web.UI.WebControls.Label lb = (System.Web.UI.WebControls.Label)GvBinData.Rows[index].FindControl("lbltransId");
        if (((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[index].FindControl("chkSelectcrm_agreement")).Checked)
        {
            crm_agreementidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecords.Text += crm_agreementidlist;
        }
        else
        {
            crm_agreementidlist += lb.Text.ToString().Trim();
            lblSelectedRecords.Text += crm_agreementidlist;
            string[] split = lblSelectedRecords.Text.Split(',');
            foreach (string item in split)
            {
                if (item != crm_agreementidlist)
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
            lblSelectedRecords.Text = temp;
        }
    }

    protected void chkgvSelectAllcrm_agreement_CheckedChanged(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.CheckBox chkSelAll = ((System.Web.UI.WebControls.CheckBox)GvBinData.HeaderRow.FindControl("chkgvSelectAllcrm_agreement"));
        for (int i = 0; i < GvBinData.Rows.Count; i++)
        {
            ((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[i].FindControl("chkSelectcrm_agreement")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecords.Text.Split(',').Contains(((HiddenField)(GvBinData.Rows[i].FindControl("hdntransIdcrm_agreement"))).Value.Trim().ToString()))
                {
                    lblSelectedRecords.Text += ((HiddenField)(GvBinData.Rows[i].FindControl("hdntransIdcrm_agreement"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecords.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvBinData.Rows[i].FindControl("hdntransIdcrm_agreement"))).Value.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecords.Text = temp;
            }
        }

    }

    protected void GvListData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = new DataTable();
        dt_Sorting = Session["gv_AgreementListData"] as DataTable;


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

        Session["gv_AgreementListData"] = dt_Sorting;

        objPageCmn.FillData((object)GvListData, dt_Sorting, "", "");

    }

    protected void GvListData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvListData.PageIndex = e.NewPageIndex;
        DataTable dtPaging = new DataTable();

        dtPaging = Session["gv_AgreementListData"] as DataTable;


        objPageCmn.FillData((object)GvListData, dtPaging, "", "");

    }

    protected void ddlFieldNamecrm_agreement_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNamecrm_agreement.SelectedIndex == 0 || ddlFieldNamecrm_agreement.SelectedIndex == 1)
        {
            txtValuecrm_agreement.Visible = false;
            txtValueDatecrm_agreement.Visible = true;
        }
        else
        {
            txtValuecrm_agreement.Visible = true;
            txtValueDatecrm_agreement.Visible = false;
        }
    }

    protected void GvBinData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBinData.PageIndex = e.NewPageIndex;
        DataTable dtPaging = new DataTable();

        dtPaging = Session["gv_AgreementBinData"] as DataTable;


        objPageCmn.FillData((object)GvBinData, dtPaging, "", "");

        for (int i = 0; i < GvBinData.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvBinData.Rows[i].FindControl("hdntransIdcrm_agreement");
            string[] split = lblSelectedRecords.Text.Split(',');

            for (int j = 0; j < lblSelectedRecords.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecords.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecords.Text.Split(',')[j].Trim().ToString())
                    {
                        ((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[i].FindControl("chkSelectcrm_agreement")).Checked = true;
                    }
                }
            }
        }
    }

    protected void GvBinData_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = new DataTable();

        dt_Sorting = Session["gv_AgreementBinData"] as DataTable;

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

        Session["gv_AgreementBinData"] = dt_Sorting;

        objPageCmn.FillData((object)GvBinData, dt_Sorting, "", "");

    }

    public bool checkValidation()
    {
        if (txtAgreementNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Agreement No')", true);
            txtAgreementNo.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtAgreementDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Agreement Date')", true);
            txtAgreementDate.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }

        if (txtAgreementNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Agreement No')", true);
            txtAgreementNo.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtContactName.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Contact Name')", true);
            txtContactName.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtEmailAddress.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Email Address')", true);
            txtEmailAddress.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtMobileNo.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Mobile No')", true);
            txtMobileNo.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (ddlAddress.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select Address')", true);
            ddlAddress.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (ddlCountry.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Select Country')", true);
            ddlCountry.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }


        if (txtState.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter State')", true);
            txtState.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtCity.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter City')", true);
            txtCity.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }

        if (txtfromDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Start Date')", true);
            txtfromDate.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtToDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Expiry Date')", true);
            txtToDate.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        if (txtHandledBy.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Enter Agreement Handled by Name')", true);
            txtHandledBy.Focus();
            btncrm_agreementSave.Enabled = true;
            return false;
        }
        return true;
    }

    protected void btncrm_agreementSave_Click(object sender, EventArgs e)
    {
        btncrm_agreementSave.Enabled = false;

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trans;
        trans = con.BeginTransaction();

        string trans_id = "0";


        try
        {
            //checking all validation
            if (checkValidation())
            {
                if (hdntransID.Value == "")
                {
                    //new case
                    int x = obj_agreement.Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtAgreementNo.Text, txtAgreementDate.Text, Session["ContactID"].ToString(), hdnContact_Id.Value, txtEmailAddress.Text, txtMobileNo.Text, ddlAddress.SelectedValue, ddlCountry.SelectedValue, hdnstateId.Value, hdncityId.Value, hdnRef_type.Value, "3", txtfromDate.Text, txtToDate.Text, txtSecurityAmt.Text, txtTurnover.Text, txtNotes.Text, txtTerms_Conditions.Text, "0", Session["UserId"].ToString(), Session["UserId"].ToString(), ref trans);
                    trans_id = x.ToString();
                }
                else
                {
                    if(Lbl_Tab_New.Text== "Renew")
                    {
                        //renewal case
                        int x = obj_agreement.Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtAgreementNo.Text, txtAgreementDate.Text, Session["ContactID"].ToString(), hdnContact_Id.Value, txtEmailAddress.Text, txtMobileNo.Text, ddlAddress.SelectedValue, ddlCountry.SelectedValue, hdnstateId.Value, hdncityId.Value, hdnRef_type.Value, "3", txtfromDate.Text, txtToDate.Text, txtSecurityAmt.Text, txtTurnover.Text, txtNotes.Text, txtTerms_Conditions.Text, hdntransID.Value, Session["UserId"].ToString(), Session["UserId"].ToString(), ref trans);
                        trans_id = x.ToString();
                    }
                    else
                    {
                        //update case
                        obj_agreement.Update(hdntransID.Value, txtAgreementDate.Text, hdnContact_Id.Value, txtEmailAddress.Text, txtMobileNo.Text, ddlAddress.SelectedValue, ddlCountry.SelectedValue, hdnstateId.Value, hdncityId.Value, "3", txtfromDate.Text, txtToDate.Text, txtSecurityAmt.Text, txtTurnover.Text, txtNotes.Text, txtTerms_Conditions.Text, Session["UserId"].ToString(), ref trans);
                        trans_id = hdntransID.Value;
                    }
                }

                //product updation and deletion
                if (GvProductData.Rows.Count > 0)
                {
                    obj_agreement_product.deleteProduct(trans_id, ref trans);

                    string productCatId = "", productId = "", TargetQty = "", TargetAmt = "", serial_no = "0";

                    for (int i = 0; i < GvProductData.Rows.Count; i++)
                    {
                        serial_no = (GvProductData.Rows[i].FindControl("gvhdnSerial_No") as HiddenField).Value;
                        productCatId = (GvProductData.Rows[i].FindControl("gvhdnCategoryId") as HiddenField).Value;

                        productId = (GvProductData.Rows[i].FindControl("gvhdnProductId") as HiddenField).Value;
                        productId = productId == "@NOTFOUND@" ? "0" : productId;

                        TargetQty = (GvProductData.Rows[i].FindControl("gvlbltargetQuantity") as Label).Text;
                        TargetQty = TargetQty == "" ? "0" : TargetQty;

                        TargetAmt = (GvProductData.Rows[i].FindControl("gvlblQuantity") as Label).Text;
                        TargetAmt = TargetAmt == "" ? "0" : TargetAmt;

                        obj_agreement_product.Insert(trans_id, serial_no, productCatId, productId, TargetQty, "0", TargetAmt, "0", Session["UserId"].ToString(), Session["UserId"].ToString(), ref trans);
                    }
                }
                //end
            }

            if (Lbl_Tab_New.Text == "New")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Saved Successfully')", true);
            }else  if (Lbl_Tab_New.Text == "Edit")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Updated Successfully')", true);
            }else if (Lbl_Tab_New.Text == "Renew")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record Renewed Successfully')", true);
            }

            trans.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trans.Dispose();
            con.Dispose();

            if(chkReminder.Checked)
            {
                // set reminder when contract ends.
                string message = "CRM Agreement of "+ hdnRef_typeName.Value + " for the "+ lblHeaderR.Text + " will expire on "+txtToDate.Text+"";
                setReminder(txtToDate.Text, Session["ContactId"].ToString(), trans_id.ToString(), message);
            }

            btncrm_agreementSave.Enabled = true;
            fillGridSession(Session["ContactId"].ToString());
            fillGrid();
            reset();
        }
        catch (Exception err)
        {
            btncrm_agreementSave.Enabled = true;
            trans.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trans.Dispose();
            con.Dispose();
        }


        return;

    }

    private void WebUserControl_crm_agreement_Click(object sender, ImageClickEventArgs e)
    {
        throw new NotImplementedException();
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

        return EmployeeName;
    }

    protected void Btncrm_agreementReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void lnkViewDetailcrm_agreement_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }

    protected void IbtnDeletecrm_agreement_Command(object sender, CommandEventArgs e)
    {
        obj_agreement.InActivateDataByTransId(e.CommandArgument.ToString());
        fillBinGridSession(Session["ContactId"].ToString());
        fillBinGrid();
        fillGridSession(Session["ContactId"].ToString());
        fillGrid();
    }

    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/crm_agreement", "CRM", "crm_agreement", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void btn_AddProduct_Click(object sender, EventArgs e)
    {

        string productCatId = "", productId = "", ProductName = "", TargetQty = "", TargetAmt = "", trans_id = "0", serial_no = "0", productCategory = "", productCode = "";

        DataTable Dt_Product_Grid = new DataTable();
        Dt_Product_Grid.Columns.AddRange(new DataColumn[9] { new DataColumn("Trans_Id"), new DataColumn("Serial_No", typeof(int)), new DataColumn("Product_Category_Id"), new DataColumn("Product_Category"), new DataColumn("Product_Code"), new DataColumn("Product_Id"), new DataColumn("EProductName"), new DataColumn("Target_Quantity"), new DataColumn("Target_Amount") });
        
        if (hdntransID.Value != "")
        {
            trans_id = hdntransID.Value;
        }

        for (int i = 0; i < GvProductData.Rows.Count; i++)
        {
            serial_no = (GvProductData.Rows[i].FindControl("gvhdnSerial_No") as HiddenField).Value;
            trans_id = (GvProductData.Rows[i].FindControl("gvhdnTrans_Id") as HiddenField).Value;
            productCategory = (GvProductData.Rows[i].FindControl("gvlblProductCategory") as Label).Text;
            productCatId = (GvProductData.Rows[i].FindControl("gvhdnCategoryId") as HiddenField).Value;
            productCode = (GvProductData.Rows[i].FindControl("gvlblProductCode") as Label).Text;
            productId = (GvProductData.Rows[i].FindControl("gvhdnProductId") as HiddenField).Value;
            ProductName = (GvProductData.Rows[i].FindControl("gvlblProductName") as Label).Text;
            TargetQty = (GvProductData.Rows[i].FindControl("gvlbltargetQuantity") as Label).Text;
            TargetAmt = (GvProductData.Rows[i].FindControl("gvlblQuantity") as Label).Text;
            Dt_Product_Grid.Rows.Add(trans_id, serial_no, productCatId, productCategory, productCode, productId, ProductName, TargetQty, TargetAmt);
        }

        productId = "0";
        productId = obj_ProductMaster.GetProductIdbyProductCode(txtProductCode.Text,Session["BrandId"].ToString());

        if (Dt_Product_Grid.Rows.Count > 0)
        {
            serial_no = Dt_Product_Grid.Rows.Count.ToString();
        }
        else
        {
            serial_no = "0";
        }

        Dt_Product_Grid.Rows.Add(trans_id, serial_no, ddlProductCategory.SelectedValue, ddlProductCategory.SelectedItem, txtProductCode.Text, productId, txtProductName.Text, txtTargetQuantity.Text, txtTargetAmount.Text);
        GvProductData.DataSource = Dt_Product_Grid;
        GvProductData.DataBind();
        reset_product();

    }
    public void reset_product()
    {
        ddlProductCategory.SelectedIndex = 0;
        txtProductCode.Text = "";
        txtProductName.Text = "";
        txtTargetQuantity.Text = "";
        txtTargetAmount.Text = "";
    }
    public void fillAddress(string customer_id)
    {
        ddlAddress.Items.Clear();
        DataTable dt = address.GetAddressChildDataByAddTypeAndAddRefId("Contact", customer_id);

        if (dt.Rows.Count > 0)
        {
            ddlAddress.DataSource = dt;
            ddlAddress.DataTextField = "Address_Name";
            ddlAddress.DataValueField = "Trans_Id1";
            ddlAddress.DataBind();
            ddlAddress.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddlAddress.DataSource = null;
            ddlAddress.DataBind();
        }
        setCountryId();
    }
    private void FillCountry()
    {
        DataTable dsCountry = null;
        dsCountry = objCountryMaster.GetCountryMaster();
        if (dsCountry.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlCountry, dsCountry, "Country_Name", "Country_Id");
        }
        else
        {
            ddlCountry.Items.Insert(0, "--Select--");
            ddlCountry.SelectedIndex = 0;
        }
    }
    public void setCountryId()
    {
        Country_Currency objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        ddlCountry.SelectedValue = objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString();
        Session["AddCtrl_Country_Id"] = ddlCountry.SelectedValue;
    }
    public void fillGrid()
    {
        if (Session["gv_AgreementListData"] != null)
        {
            DataTable dt = Session["gv_AgreementListData"] as DataTable;
            GvListData.DataSource = dt;
            GvListData.DataBind();
            lblTotalRecordscrm_agreement.Text = "Total Records: " + dt.Rows.Count.ToString();
            dt.Dispose();
        }
    }

    public void fillGridSession(string customer_id)
    {
        try
        {
            Session["gv_AgreementListData"] = null;
            DataTable dt = obj_agreement.getActiveDataByCustomer_Id(customer_id);
            Session["gv_AgreementListData"] = dt;

        }
        catch
        {

        }
    }

    public void fillBinGrid()
    {
        if (Session["gv_AgreementBinData"] != null)
        {
            DataTable dt = Session["gv_AgreementBinData"] as DataTable;
            GvBinData.DataSource = dt;
            GvBinData.DataBind();
            lblTotalRecordsBincrm_agreement.Text = "Total Records: " + dt.Rows.Count.ToString();
            dt.Dispose();
        }
    }

    public void fillBinGridSession(string customer_id)
    {
        try
        {
            Session["gv_AgreementBinData"] = null;
            DataTable dt = obj_agreement.getInActiveDataByCustomer_Id(customer_id);
            Session["gv_AgreementBinData"] = dt;
        }
        catch
        { }
    }
    public void fillcommonThings(string ref_typeName,string ref_typeId, string customer_name, string CustId)
    {
        fillHeader(ref_typeId, customer_name);
        hdnRef_type.Value = ref_typeId;
        hdnRef_typeName.Value = ref_typeName;
        fillAddress(CustId);
        fillGridSession(CustId);
        fillGrid();
        fillBinGridSession(CustId);
        fillBinGrid();
    }

    public void fillProductCategory()
    {
        try
        {
            OpportunityDashboard objOppoDashboard = new OpportunityDashboard(Session["DBConnection"].ToString());
            DataTable DtAllProductCat = new DataTable();
            DtAllProductCat = objOppoDashboard.getAllProductcategory();
            ddlProductCategory.DataSource = DtAllProductCat;
            ddlProductCategory.DataTextField = "Category_Name";
            ddlProductCategory.DataValueField = "CategoryId";
            ddlProductCategory.DataBind();
            ddlProductCategory.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch
        {
        }
    }

    protected void gvlblDelete_Command(object sender, CommandEventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        string productCatId = "", productId = "", ProductName = "", TargetQty = "", TargetAmt = "", trans_id = "0", serial_no = "0", productCategory = "", productCode = "";

        DataTable Dt_Product_Grid = new DataTable();
        Dt_Product_Grid.Columns.AddRange(new DataColumn[9] { new DataColumn("Trans_Id"), new DataColumn("Serial_No", typeof(int)), new DataColumn("Product_Category_Id"), new DataColumn("Product_Category"), new DataColumn("Product_Code"), new DataColumn("Product_Id"), new DataColumn("EProductName"), new DataColumn("Target_Quantity"), new DataColumn("Target_Amount") });

        string x = ib.CommandArgument;

        for (int i = 0; i < GvProductData.Rows.Count; i++)
        {
            serial_no = (GvProductData.Rows[i].FindControl("gvhdnSerial_No") as HiddenField).Value;
            trans_id = (GvProductData.Rows[i].FindControl("gvhdnTrans_Id") as HiddenField).Value;
            productCategory = (GvProductData.Rows[i].FindControl("gvlblProductCategory") as Label).Text;
            productCatId = (GvProductData.Rows[i].FindControl("gvhdnCategoryId") as HiddenField).Value;
            productCode = (GvProductData.Rows[i].FindControl("gvlblProductCode") as Label).Text;
            productId = (GvProductData.Rows[i].FindControl("gvhdnProductId") as HiddenField).Value;
            ProductName = (GvProductData.Rows[i].FindControl("gvlblProductName") as Label).Text;
            TargetQty = (GvProductData.Rows[i].FindControl("gvlbltargetQuantity") as Label).Text;
            TargetAmt = (GvProductData.Rows[i].FindControl("gvlblQuantity") as Label).Text;
            if (x != serial_no)
            {
                Dt_Product_Grid.Rows.Add(trans_id, serial_no, productCatId, productCategory, productCode, productId, ProductName, TargetQty, TargetAmt);
            }
        }

        GvProductData.DataSource = Dt_Product_Grid;
        GvProductData.DataBind();

    }

    public string getProductCodebyProductId(string ProductId)
    {
        if (ProductId == "")
        {
            return "";
        }
        return obj_ProductMaster.GetProductCodebyProductId(ProductId);
    }
    public string getProductIdFromProductCode(string ProductCode)
    {
        if (ProductCode == "")
        {
            return "";
        }
        return obj_ProductMaster.GetProductIdbyProductCode(ProductCode, Session["BrandId"].ToString());
    }
    public string getCategoryNameByCategoryId(string categoryId)
    {
        if (categoryId == "")
        {
            return "";
        }
        return obj_ProductCategory.GetCategoryNamebyCategoryId(categoryId);
    }
    public string getCategoryIdByCategoryName(string categoryName)
    {
        if (categoryName == "")
        {
            return "";
        }
        return obj_ProductCategory.GetCategoryIdbyCategoryName(categoryName);
    }


    protected void IbtnRenewcrm_agreement_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }

    public string getImageURL_Edit_or_extended(string Todate)
    {
        string imageURL = "";
        if (Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(Todate))
        {
            imageURL = "~/Images/contact_renewal.png";
        }
        else
        {
            imageURL = "~/Images/edit.png";
        }
        return imageURL;
    }
    public string getToolTip(string Todate)
    {
        string imageURL = "";
        if (Convert.ToDateTime(System.DateTime.Now.ToString("dd-MMM-yyyy")) > Convert.ToDateTime(Todate))
        {
            imageURL = "Extended";
        }
        else
        {
            imageURL = "Edit";
        }
        return imageURL;
    }
    public void setReminder(string dueDate, string Customer_Id, string agreement_Id, string message)
    {
        int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "crm_agreements", agreement_Id, message, "../CRM/CRM.aspx?contactInfo=" + Customer_Id + "", System.DateTime.Now.ToString(), "1", dueDate, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
        new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), dueDate, "", Session["UserId"].ToString(), Session["UserId"].ToString());
    }

}

// used to add on parent page 

//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static void CategoryChange(string categoryId)
//{
//    HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] = categoryId;
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
//{
//    try
//    {
//        if (HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] == null)
//        {
//            HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] = "0";
//        }
//        Inv_Product_Category PC = new Inv_Product_Category();
//        DataTable dt = PC.GetProductcodeByCategoryId_PreText(HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"].ToString(), prefixText);
//        string[] txt = new string[dt.Rows.Count];
//        for (int i = 0; i < dt.Rows.Count; i++)
//        {
//            txt[i] = dt.Rows[i]["ProductCode"].ToString();
//        }
//        return txt;
//    }
//    catch
//    {
//        return null;
//    }
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
//{
//    try
//    {
//        Inv_ProductMaster PM = new Inv_ProductMaster();
//        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString());
//        string[] txt = new string[dt.Rows.Count];
//        for (int i = 0; i < dt.Rows.Count; i++)
//        {
//            txt[i] = dt.Rows[i]["EProductName"].ToString();
//        }
//        return txt;
//    }
//    catch
//    {
//        return null;
//    }
//}

//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static string txtCity_TextChanged(string stateId, string cityName)
//{
//    CityMaster ObjCityMaster = new CityMaster();

//    DataTable dt_CityData = ObjCityMaster.GetAllCityByPrefixText(cityName, stateId);

//    if (dt_CityData.Rows.Count > 0)
//    {
//        return dt_CityData.Rows[0]["Trans_Id"].ToString();
//    }
//    else
//    {
//        return "0";
//    }
//}


//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static string txtState_TextChanged(string CountryId, string StateName)
//{
//    StateMaster ObjStatemaster = new StateMaster();
//    DataTable dt_stateData = ObjStatemaster.GetAllStateByPrefixText(StateName, CountryId);
//    if (dt_stateData.Rows.Count > 0)
//    {
//        HttpContext.Current.Session["AddCtrl_State_Id"] = dt_stateData.Rows[0]["Trans_Id"].ToString();
//        return dt_stateData.Rows[0]["Trans_Id"].ToString();
//    }
//    else
//    {
//        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
//        return "0";
//    }
//}

//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static void ddlCountry_IndexChanged(string CountryId)
//{
//    HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
//{
//    try
//    {
//        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster();
//        string id = string.Empty;

//        if (HttpContext.Current.Session["ContactID"] != null)
//        {
//            id = HttpContext.Current.Session["ContactID"].ToString();
//        }
//        else
//        {
//            id = "0";
//        }

//        DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(prefixText,id);

//        string[] filterlist = new string[dtCon.Rows.Count];
//        if (dtCon.Rows.Count > 0)
//        {
//            for (int i = 0; i < dtCon.Rows.Count; i++)
//            {
//                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
//            }
//            return filterlist;
//        }
//        else
//        {
//            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
//            string[] filterlistcon = new string[dtcon.Rows.Count];
//            for (int i = 0; i < dtcon.Rows.Count; i++)
//            {
//                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
//            }
//            return filterlistcon;
//        }
//    }
//    catch (Exception error)
//    {

//    }
//    return null;
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
//{
//    ES_EmailMaster_Header Email = new ES_EmailMaster_Header();
//    DataTable dt = Email.GetDistinctEmailId(prefixText);

//    string[] str = new string[dt.Rows.Count];
//    for (int i = 0; i < dt.Rows.Count; i++)
//    {
//        str[i] = dt.Rows[i]["Email_Id"].ToString();
//    }
//    return str;
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static void save_email(string emailAddress, string countryId, string contactId)
//{
//    ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header();
//    ES_EmailMasterDetail objEmailDetail = new ES_EmailMasterDetail();

//    int emailHeaderID = objEmailHeader.ES_EmailMasterHeader_Insert(emailAddress, countryId, "", "", "", "", "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
//    objEmailDetail.ES_EmailMasterDetail_Insert(contactId, "Contact", emailHeaderID.ToString(), "true", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
//{
//    if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
//    {
//        return null;
//    }
//    StateMaster objStateMaster = new StateMaster();
//    DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
//    string[] txt = new string[dt.Rows.Count];

//    for (int i = 0; i < dt.Rows.Count; i++)
//    {
//        txt[i] = dt.Rows[i]["State_Name"].ToString();
//    }
//    return txt;
//}

//[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
//public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
//{
//    if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
//    {
//        return null;
//    }
//    CityMaster objCityMaster = new CityMaster();
//    DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
//    string[] txt = new string[dt.Rows.Count];

//    for (int i = 0; i < dt.Rows.Count; i++)
//    {
//        txt[i] = dt.Rows[i]["City_Name"].ToString();
//    }
//    return txt;
//}

//[System.Web.Services.WebMethod()]
//[System.Web.Script.Services.ScriptMethod()]
//public static string productcode_change(string productCode)
//{
//    Inv_ProductMaster ipm = new Inv_ProductMaster();
//    string productId = ipm.GetProductIdbyProductCode(productCode);

//    if (productId != "")
//    {
//        string productName = ipm.GetProductNamebyProductId(productId);
//        return productName;
//    }

//    return "";
//}

