using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
public partial class CRM_PartnerManagement : System.Web.UI.Page
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
    Ems_GroupMaster objGroup = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        txtValueDateBin.Attributes.Add("readonly", "true");
        txtValueDate.Attributes.Add("readonly", "true");
        txtfromDate.Attributes.Add("readonly", "true");
        txtToDate.Attributes.Add("readonly", "true");
        txtAgreementDate.Attributes.Add("readonly", "true");

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
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CRM/PartnerManagement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            Session["ContactId"] = "0";
            Session["ContactPersonID"] = "0";

            Lbl_Tab_New.Text = "New";
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active1()", true);
            SetGeneratedByName();
            FillCountry();
            fillProductCategory();
            fillGrid();
            fillBinGrid();
            FillGroup();
            GetDocumentNumber();
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

                dtEmployeeDtl.Dispose();
            }
        }
    }

    public void GetDocumentNumber()
    {
        txtAgreementNo.Text = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "171", Common.GetObjectIdbyPageURL("../CRM/PartnerManagement.aspx", Session["DBConnection"].ToString()), "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        txtAgreementDate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanPrint.Value = clsPagePermission.bView.ToString().ToLower(); 

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
        dt.Dispose();
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
                dtData.Dispose();
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

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        Session["gv_AgreementListData"] = null;


        fillGrid();
        txtValueDate.Text = "";
        txtValue.Text = "";
    }

    protected void btnListSearch_Click(object sender, ImageClickEventArgs e)
    {
        fillGrid();
        DataTable dtAdd = new DataTable();
        dtAdd = Session["gv_AgreementListData"] as DataTable;

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlFieldName.SelectedItem.Value == "From_Date" || ddlFieldName.SelectedItem.Value == "To_Date" || ddlFieldName.SelectedItem.Value == "Agreement_Date")
            {
                if (txtValueDate.Text.Trim() != "")
                {
                    if (ddlOption.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "'";
                    }
                    else if (ddlOption.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Date");
                    txtValueDate.Focus();
                    return;
                }
            }
            else
            {
                if (txtValue.Text.Trim() != "")
                {
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
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Some Value");
                    txtValue.Focus();
                }
            }
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvListData, view.ToTable(), "", "");
            Session["gv_AgreementListData"] = view.ToTable();


            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            dtAdd.Dispose();
        }
    }

    public void reset()
    {
        GvProductData.DataSource = null;
        GvProductData.DataBind();
        Lbl_Tab_New.Text = "New";
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
        txtNotes.Text = "";
        txtTurnover.Text = "";
        GetDocumentNumber();
        ddlGroupSearch.SelectedIndex = 0;
        txtCustomer.Text = "";
        hdncust_Id.Value = "";
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
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        dtres.Dispose();
        return ArebicMessage;
    }




    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //fileupload_div.Visible = true;
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        hdntransID.Value = e.CommandArgument.ToString();


        DataTable dtAllData = obj_agreement.getActiveDataByTrans_Id(hdntransID.Value);
        if (dtAllData.Rows.Count > 0)
        {
            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                btnSave.Visible = false;
                BtnReset.Visible = false;
            }

            if (b.ToolTip == "Edit")
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                BtnReset.Visible = true;
                btnSave.Visible = true;
            }
            if (b.ToolTip == "Extend")
            {
                if (e.CommandName.ToString() != "")
                {
                    DisplayMessage("Cant Extend because it has already been extended");
                    return;
                }
                //if(obj_agreement.CheckExtendedOrNot(hdntransID.Value))
                //{
                //    DisplayMessage("Cant Extend because it has already been extended");
                //    return;
                //}
                Lbl_Tab_New.Text = "Extend";
                BtnReset.Visible = true;
                btnSave.Visible = true;
            }

            fillControlsValue(dtAllData);

            DataTable dt_product = obj_agreement_product.getAllActiveDataByTrans_Id(hdntransID.Value);
            if (dt_product.Rows.Count > 0)
            {
                fillProductListData(dt_product);
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active_agreement()", true);
            dt_product.Dispose();
        }

        dtAllData.Dispose();

    }

    public void fillControlsValue(DataTable DtAgreementdata)
    {
        txtAgreementNo.Text = DtAgreementdata.Rows[0]["Agreement_No"].ToString();
        txtAgreementDate.Text = GetDate(DtAgreementdata.Rows[0]["Agreement_Date"].ToString());
        txtContactName.Text = DtAgreementdata.Rows[0]["contactName"].ToString() + "/" + DtAgreementdata.Rows[0]["Contact_Id"].ToString();
        txtCustomer.Text = DtAgreementdata.Rows[0]["customerName"].ToString() + "/" + DtAgreementdata.Rows[0]["Customer_Id"].ToString();
        Session["ContactId"] = DtAgreementdata.Rows[0]["Customer_Id"].ToString();

        try
        {
            if (DtAgreementdata.Rows[0]["Ref_Type"].ToString() != "" && DtAgreementdata.Rows[0]["Ref_Type"].ToString() != "0")
            {
                ddlGroupSearch.SelectedValue = DtAgreementdata.Rows[0]["Ref_Type"].ToString();
            }
        }
        catch
        {

        }

        hdncust_Id.Value = DtAgreementdata.Rows[0]["Customer_Id"].ToString();
        hdnContact_Id.Value = DtAgreementdata.Rows[0]["Contact_Id"].ToString();
        txtEmailAddress.Text = DtAgreementdata.Rows[0]["Email_Address"].ToString();
        txtMobileNo.Text = DtAgreementdata.Rows[0]["Mobile_No"].ToString();
        try
        {
            ddlAddress.SelectedValue = DtAgreementdata.Rows[0]["AddressId"].ToString();
        }
        catch(Exception ex)
        {
            string str = ex.Message.ToString();
        }
        
        ddlCountry.SelectedValue = DtAgreementdata.Rows[0]["Country_Id"].ToString();
        txtState.Text = DtAgreementdata.Rows[0]["State_Name"].ToString();
        hdnstateId.Value = DtAgreementdata.Rows[0]["State_Id"].ToString();
        txtCity.Text = DtAgreementdata.Rows[0]["City_Name"].ToString();
        hdncityId.Value = DtAgreementdata.Rows[0]["City_Id"].ToString();
        txtfromDate.Text = GetDate(DtAgreementdata.Rows[0]["From_Date"].ToString());
        txtToDate.Text = GetDate(DtAgreementdata.Rows[0]["To_Date"].ToString());
        txtHandledBy.Text = DtAgreementdata.Rows[0]["handledByName"].ToString() + "/" + DtAgreementdata.Rows[0]["Handled_By"].ToString();
        txtTerms_Conditions.Text = DtAgreementdata.Rows[0]["Terms_Condition"].ToString();
        txtSecurityAmt.Text = Common.GetAmountDecimal(DtAgreementdata.Rows[0]["Security_Amount"].ToString(),Session["DBConnection"].ToString(), ddlCountry.SelectedValue);
        txtNotes.Text = DtAgreementdata.Rows[0]["Field2"].ToString();
        txtTurnover.Text = DtAgreementdata.Rows[0]["Field1"].ToString();
        fillAddress(hdncust_Id.Value);
    }

    public void fillProductListData(DataTable dtProductdata)
    {
        GvProductData.DataSource = dtProductdata;
        for(int count=0;count< dtProductdata.Rows.Count;count++)
        {
            dtProductdata.Rows[count]["Target_Amount"] =  Common.GetAmountDecimal(dtProductdata.Rows[count]["Target_Amount"].ToString(), Session["DBConnection"].ToString(), ddlCountry.SelectedValue);
        }
        GvProductData.DataBind();
    }

    #region PrintReport
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        PrintReport(e.CommandArgument.ToString());
    }
    void PrintReport(string strAgreementNo)
    {
        string strCmd = string.Format("window.open('../CRM/PartnerCertificate.aspx?Id=" + strAgreementNo.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedIndex == 0)
        {
            txtValueBin.Visible = false;
            txtValueDateBin.Visible = true;
        }
        else
        {
            txtValueBin.Visible = true;
            txtValueDateBin.Visible = false;
        }
    }

    protected void btnsearchBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        fillBinGrid();
        DataTable dtAdd = new DataTable();
        dtAdd = Session["gv_AgreementBinData"] as DataTable;

        if (ddlOptionBin.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlFieldNameBin.SelectedItem.Value == "_date")
            {
                if (txtValueDateBin.Text.Trim() != "")
                {
                    if (ddlOptionBin.SelectedIndex == 1)
                    {
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDateBin.Text.Trim()) + "'";
                    }
                    else if (ddlOptionBin.SelectedIndex == 2)
                    {
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDateBin.Text.Trim()) + "%'";
                    }
                    else
                    {
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDateBin.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Date");
                    txtValueDateBin.Focus();
                    return;
                }
            }
            else
            {
                if (txtValueBin.Text.Trim() != "")
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
                        condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '%" + txtValueBin.Text.Trim() + "%'";
                    }
                }
                else
                {
                    //DisplayMessage("Enter Some Value");
                    txtValueBin.Focus();
                }
            }
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            objPageCmn.FillData((object)GvBinData, view.ToTable(), "", "");
            Session["gv_AgreementBinData"] = view.ToTable();

            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        dtAdd.Dispose();
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["gv_AgreementBinData"] = null;
        fillBinGrid();
        txtValueBin.Text = "";
        txtValueDateBin.Text = "";
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)Gvr.FindControl("chkSelect");
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
        fillBinGrid();
        fillGrid();
    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
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
                HiddenField lblconid = (HiddenField)GvBinData.Rows[i].FindControl("hdntransId");
                for (int j = 0; j < lblSelectedRecords.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecords.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecords.Text.Split(',')[j].Trim().ToString())
                        {
                            ((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[i].FindControl("chkSelect")).Checked = true;
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
            dtSelectedData.Dispose();
        }
        dtPbrand.Dispose();
    }

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string idlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((System.Web.UI.WebControls.CheckBox)sender).Parent.Parent).RowIndex;
        System.Web.UI.WebControls.Label lb = (System.Web.UI.WebControls.Label)GvBinData.Rows[index].FindControl("lbltransId");
        if (((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[index].FindControl("chkSelect")).Checked)
        {
            idlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecords.Text += idlist;
        }
        else
        {
            idlist += lb.Text.ToString().Trim();
            lblSelectedRecords.Text += idlist;
            string[] split = lblSelectedRecords.Text.Split(',');
            foreach (string item in split)
            {
                if (item != idlist)
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

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.CheckBox chkSelAll = ((System.Web.UI.WebControls.CheckBox)GvBinData.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < GvBinData.Rows.Count; i++)
        {
            ((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecords.Text.Split(',').Contains(((HiddenField)(GvBinData.Rows[i].FindControl("hdntransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecords.Text += ((HiddenField)(GvBinData.Rows[i].FindControl("hdntransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecords.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvBinData.Rows[i].FindControl("hdntransId"))).Value.Trim().ToString())
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
        dt_Sorting.Dispose();
    }

    protected void GvListData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvListData.PageIndex = e.NewPageIndex;
        DataTable dtPaging = new DataTable();

        dtPaging = Session["gv_AgreementListData"] as DataTable;


        objPageCmn.FillData((object)GvListData, dtPaging, "", "");
        dtPaging.Dispose();
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedIndex == 0 || ddlFieldName.SelectedIndex == 1)
        {
            txtValue.Visible = false;
            txtValueDate.Visible = true;
        }
        else
        {
            txtValue.Visible = true;
            txtValueDate.Visible = false;
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
            HiddenField lblconid = (HiddenField)GvBinData.Rows[i].FindControl("hdntransId");
            string[] split = lblSelectedRecords.Text.Split(',');

            for (int j = 0; j < lblSelectedRecords.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecords.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecords.Text.Split(',')[j].Trim().ToString())
                    {
                        ((System.Web.UI.WebControls.CheckBox)GvBinData.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        dtPaging.Dispose();
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
        dt_Sorting.Dispose();
    }

    public bool checkValidation()
    {
        if (txtAgreementNo.Text == "")
        {
            DisplayMessage("Enter Agreement No");
            txtAgreementNo.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (txtAgreementDate.Text == "")
        {
            DisplayMessage("Enter Agreement Date");
            txtAgreementDate.Focus();
            btnSave.Enabled = true;
            return false;
        }

        if (txtAgreementNo.Text == "")
        {
            DisplayMessage("Enter Agreement No");
            txtAgreementNo.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (txtContactName.Text == "")
        {
            DisplayMessage("Enter Contact Name");
            txtContactName.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (txtEmailAddress.Text == "")
        {
            DisplayMessage("Enter Email Address");
            txtEmailAddress.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (txtMobileNo.Text == "")
        {
            DisplayMessage("Enter Mobile No");
            txtMobileNo.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (ddlAddress.SelectedIndex == 0)
        {
            DisplayMessage("Select Address");
            ddlAddress.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (ddlCountry.SelectedIndex == 0)
        {
            DisplayMessage("Select Country");
            ddlCountry.Focus();
            btnSave.Enabled = true;
            return false;
        }


        if (txtState.Text == "")
        {
            DisplayMessage("Enter State");
            txtState.Focus();
            btnSave.Enabled = true;
            return false;
        }
        //if (txtCity.Text == "")
        //{
        //    DisplayMessage("Enter City");
        //    txtCity.Focus();
        //    btnSave.Enabled = true;
        //    return false;
        //}

        if (txtfromDate.Text == "")
        {
            DisplayMessage("Enter Start Date");
            txtfromDate.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (txtToDate.Text == "")
        {
            DisplayMessage("Enter Expiry Date");
            txtToDate.Focus();
            btnSave.Enabled = true;
            return false;
        }
        if (txtHandledBy.Text == "")
        {
            DisplayMessage("Enter Agreement Handled by Name");
            txtHandledBy.Focus();
            btnSave.Enabled = true;
            return false;
        }
        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;

        string trans_id = "0";
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        SqlTransaction trans = null;

        try
        {
            //checking all validation
            if (checkValidation())
            {
                con.Open();
                trans = con.BeginTransaction();

                if (hdntransID.Value == "")
                {
                    //new case
                    int x = obj_agreement.Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtAgreementNo.Text, txtAgreementDate.Text, Session["ContactID"].ToString(), hdnContact_Id.Value, txtEmailAddress.Text, txtMobileNo.Text, ddlAddress.SelectedValue, ddlCountry.SelectedValue, hdnstateId.Value, hdncityId.Value, ddlGroupSearch.SelectedValue.ToString(), "3", txtfromDate.Text, txtToDate.Text, string.IsNullOrEmpty(txtSecurityAmt.Text) ? "0" : txtSecurityAmt.Text, txtTurnover.Text, txtNotes.Text, txtTerms_Conditions.Text, "0", Session["UserId"].ToString(), Session["UserId"].ToString(), ref trans);
                    trans_id = x.ToString();
                }
                else
                {
                    if (Lbl_Tab_New.Text == "Extend")
                    {
                        //Extend case
                        int x = obj_agreement.Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtAgreementNo.Text, txtAgreementDate.Text, Session["ContactID"].ToString(), hdnContact_Id.Value, txtEmailAddress.Text, txtMobileNo.Text, ddlAddress.SelectedValue, ddlCountry.SelectedValue, hdnstateId.Value, hdncityId.Value, ddlGroupSearch.SelectedValue.ToString(), "3", txtfromDate.Text, txtToDate.Text, txtSecurityAmt.Text, txtTurnover.Text, txtNotes.Text, txtTerms_Conditions.Text, hdntransID.Value, Session["UserId"].ToString(), Session["UserId"].ToString(), ref trans);
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
                trans.Commit();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trans.Dispose();
                con.Dispose();
                //end
            }
            else
            {
                return;
            }

            if (Lbl_Tab_New.Text == "New")
            {
                DisplayMessage("Record Saved Successfully", "green");
            }
            else if (Lbl_Tab_New.Text == "Edit")
            {
                DisplayMessage("Record Updated Successfully", "green");
            }
            else if (Lbl_Tab_New.Text == "Extend")
            {
                DisplayMessage("Record Renewed Successfully", "green");
            }



            if (chkReminder.Checked)
            {
                // set reminder when contract ends.
                string message = "CRM Agreement of " + ddlGroupSearch.SelectedItem.ToString() + " for the 'customer' will expire on " + txtToDate.Text + "";
                setReminder(txtToDate.Text, Session["ContactId"].ToString(), trans_id.ToString(), message);
            }

            btnSave.Enabled = true;
            fillGrid();
            reset();
        }
        catch (Exception err)
        {
            btnSave.Enabled = true;
            if (trans != null)
            {
                trans.Rollback();
            }
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trans.Dispose();
            con.Dispose();
        }


        return;

    }

    private void WebUserControl__Click(object sender, ImageClickEventArgs e)
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
        Dt.Dispose();
        return EmployeeName;
    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        obj_agreement.InActivateDataByTransId(e.CommandArgument.ToString());
        try
        {
            new Reminder(Session["DBConnection"].ToString()).setIsActiveFalseByRef_table_name_n_pk("crm_agreements", e.CommandArgument.ToString());
        }
        catch
        {

        }
        fillBinGrid();
        fillGrid();
    }

    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/crm_agreement", "CRM", "crm_agreement", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void btn_AddProduct_Click(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");

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
        Dt_Product_Grid.Dispose();
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
        try
        {
            DataTable dt = address.GetAddressListByAddTypeAndAddRefId("Contact", customer_id);

            if (dt.Rows.Count > 0)
            {
                ddlAddress.DataSource = dt;
                ddlAddress.DataTextField = "Address_Name";
                ddlAddress.DataValueField = "Trans_Id";
                ddlAddress.DataBind();
                ddlAddress.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlAddress.DataSource = null;
                ddlAddress.DataBind();
            }
            setCountryId();
            dt.Dispose();
        }
        catch(Exception ex)
        {

        }

    }
    private void FillCountry()
    {
        DataTable dsCountry = null;
        dsCountry = objCountryMaster.GetCountryMaster();
        if (dsCountry.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlCountry, dsCountry, "Country_Name", "Country_Id");
            dsCountry.Dispose();
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
        DataTable dt = obj_agreement.getAllActiveData();
        GvListData.DataSource = dt;
        GvListData.DataBind();
        lblTotalRecords.Text = "Total Records: " + dt.Rows.Count.ToString();
        Session["gv_AgreementListData"] = dt;
        dt.Dispose();

    }

    public void fillBinGrid()
    {
        DataTable dt = obj_agreement.getAllInActiveData();
        GvBinData.DataSource = dt;
        GvBinData.DataBind();
        lblTotalRecordsBin.Text = "Total Records: " + dt.Rows.Count.ToString();
        Session["gv_AgreementBinData"] = dt;
        dt.Dispose();
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
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");

        LinkButton ib = (LinkButton)sender;

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
        Dt_Product_Grid.Dispose();
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
        return obj_ProductMaster.GetProductIdbyProductCode(ProductCode,Session["BrandId"].ToString());
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


    protected void IbtnRenew_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }


    public void setReminder(string dueDate, string Customer_Id, string agreement_Id, string message)
    {
        int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "s", agreement_Id, message, "../CRM/CRM.aspx?contactInfo=" + Customer_Id + "", System.DateTime.Now.ToString(), "1", dueDate, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
        new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), dueDate, "", Session["UserId"].ToString(), Session["UserId"].ToString());
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void CategoryChange(string categoryId)
    {
        HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] = categoryId;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] == null)
            {
                HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] = "0";
            }
            Inv_Product_Category PC = new Inv_Product_Category(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PC.GetProductcodeByCategoryId_PreText(HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt_CityData = ObjCityMaster.GetAllCityByPrefixText(cityName, stateId);

        if (dt_CityData.Rows.Count > 0)
        {
            return dt_CityData.Rows[0]["Trans_Id"].ToString();
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
        DataTable dt_stateData = ObjStatemaster.GetAllStateByPrefixText(StateName, CountryId);
        if (dt_stateData.Rows.Count > 0)
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = dt_stateData.Rows[0]["Trans_Id"].ToString();
            return dt_stateData.Rows[0]["Trans_Id"].ToString();
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void ddlCountry_IndexChanged(string CountryId)
    {
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
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

            DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(prefixText, id);

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["filterText"].ToString();
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
    public static string[] GetCustomerList(string prefixText, int count, string contextKey)
    {
        try
        {
            Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string id = string.Empty;

            DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["filterText"].ToString();
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
    public static string[] GetCompletionListMobileNo(string prefixText, int count, string contextKey)
    {
        ContactNoMaster CNM = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());

        if (HttpContext.Current.Session["ContactPersonID"] == null)
        {
            return null;
        }

        DataTable dt = CNM.GetAllContactNoByPKID(HttpContext.Current.Session["ContactPersonID"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static void save_email(string emailAddress, string countryId, string contactId)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        ES_EmailMasterDetail objEmailDetail = new ES_EmailMasterDetail(HttpContext.Current.Session["DBConnection"].ToString());

        int emailHeaderID = objEmailHeader.ES_EmailMasterHeader_Insert(emailAddress, countryId, "", "", "", "", "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        objEmailDetail.ES_EmailMasterDetail_Insert(contactId, "Contact", emailHeaderID.ToString(), "true", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
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

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string productcode_change(string productCode)
    {
        Inv_ProductMaster ipm = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string productId = ipm.GetProductIdbyProductCode(productCode,HttpContext.Current.Session["BrandId"].ToString());

        if (productId != "")
        {
            string productName = ipm.GetProductNamebyProductId(productId);
            return productName;
        }

        return "";
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static int Check_email(string emailAddress)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt_email = objEmailHeader.GetDistinctEmailId(emailAddress);
        if (dt_email.Rows.Count > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ContactName_textchange(string contactName, string contactId, string parentId)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = string.Empty;

        DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(contactName, parentId);

        if (dtCon.Rows.Count == 0)
        {
            dtCon = ObjContactMaster.GetContactTrueById(parentId);
            dtCon = new DataView(dtCon, "Trans_Id = '" + parentId + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtCon.Rows.Count > 0)
            {
                HttpContext.Current.Session["ContactPersonID"] = dtCon.Rows[0]["Trans_Id"].ToString();
                return dtCon.Rows[0]["Name"].ToString() + "/" + dtCon.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                HttpContext.Current.Session["ContactPersonID"] = "0";
                return "0";
            }
        }
        else
        {
            dtCon = new DataView(dtCon, "filterText = '" + contactName + "/" + contactId + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (dtCon.Rows.Count > 0)
        {
            HttpContext.Current.Session["ContactPersonID"] = contactId;
            return dtCon.Rows[0]["Filtertext"].ToString();
        }
        else
        {
            HttpContext.Current.Session["ContactPersonID"] = "0";
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string CustomerName_textchange(string CustomerName, string CustomerId)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(CustomerName);
        dtCon = new DataView(dtCon, "filterText = '" + CustomerName + "/" + CustomerId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtCon.Rows.Count > 0)
        {
            HttpContext.Current.Session["ContactId"] = CustomerId;
            return dtCon.Rows[0]["Filtertext"].ToString();
        }
        else
        {
            HttpContext.Current.Session["ContactId"] = "0";
            return "0";
        }
    }


    public void FillGroup()
    {
        DataTable DtGroup = objGroup.getGroupDataByPartnerManagementStatus("true");
        if (DtGroup == null)
            return;
        DtGroup = new DataView(DtGroup, "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

        if (DtGroup.Rows.Count > 0)
        {
            ddlGroupSearch.DataSource = DtGroup;
            ddlGroupSearch.DataTextField = "Group_Name";
            ddlGroupSearch.DataValueField = "Group_Id";
            ddlGroupSearch.DataBind();

            AS_ddlGroup.DataSource = DtGroup;
            AS_ddlGroup.DataTextField = "Group_Name";
            AS_ddlGroup.DataValueField = "Group_Id";
            AS_ddlGroup.DataBind();
            DtGroup.Dispose();
        }
        ddlGroupSearch.Items.Insert(0, new ListItem("Select", "0"));
        AS_ddlGroup.Items.Insert(0, new ListItem("Select", "0"));

    }

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(txtCustomer.Text))
            {
                int customer_id = 0;
                Int32.TryParse(txtCustomer.Text.Split('/')[1].ToString(), out customer_id);
                if (customer_id==0)
                {
                    DisplayMessage("Invalid customer");
                    return;
                }
                else
                {
                    fillAddress(customer_id.ToString());
                }
                
            }
        }
        catch(Exception ex)
        {

        }
        
    }

    protected void ddlFieldName_TextChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedValue == "Agreement_Date" || ddlFieldName.SelectedValue == "To_Date" || ddlFieldName.SelectedValue == "From_Date")
        {
            txtValue.Visible = false;
            txtValueDate.Visible = true;
        }
        else
        {
            txtValue.Visible = true;
            txtValueDate.Visible = false;
        }
    }

    protected void btn_advanceSearch_Click(object sender, EventArgs e)
    {
        DataTable dt_advanceSearch = obj_agreement.getAdvanceSearchData(AS_ddlGroup.SelectedItem.ToString(), AS_txtAgreementNo.Text, AS_txtCustomer.Text, AS_txtCountry.Text, AS_TxtState.Text, As_TxtCity.Text, AS_TxtCategory.Text, AS_TxtProductCode.Text, AS_TxtProductName.Text);
        GvListData.DataSource = dt_advanceSearch;
        GvListData.DataBind();
        GvListData.Columns[10].Visible = true;
        GvListData.Columns[11].Visible = true;
        GvListData.Columns[12].Visible = true;
        lblTotalRecords.Text = "Total Records: " + dt_advanceSearch.Rows.Count;
        dt_advanceSearch.Dispose();
    }

    protected void btnResetAdvanceSearch_Click(object sender, EventArgs e)
    {
        fillGrid();
        GvListData.Columns[10].Visible = false;
        GvListData.Columns[11].Visible = false;
        GvListData.Columns[12].Visible = false;
    }
}