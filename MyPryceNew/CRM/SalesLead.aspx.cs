using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
public partial class CRM_SalesLead : BasePage
{
    Common cmn = null;
    SalesLeadClass SLCalss = null;
    IT_ObjectEntry objObjectEntry = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmployee = null;
    CurrencyMaster objCurrency = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    LocationMaster objLocation = null;
    DataAccessClass objDa = null;
    Campaign objCampaign = null;
    PageControlCommon objPageCmn = null;
    string strCurrencyId = string.Empty;
    string GeneratedBy = string.Empty;
    Set_CustomerMaster ObjCustmer = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        txtLeadNo.ReadOnly = false;

        cmn = new Common(Session["DBConnection"].ToString());
        SLCalss = new SalesLeadClass(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjCustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objCampaign = new Campaign(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CRM/salesLead.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            //get all employee's name and id from Set_EmployeeMaster table, which i am using in getting information of all users in atuofill's of customer and contact and in checking that entered data is correct or not in both contact and customer.
            string where = "Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and IsActive='true'";
            DataTable EmpName_Id = objDa.return_NameNID("Emp_Name, Emp_Id", "Set_EmployeeMaster", where);
            if (EmpName_Id != null)
            {
                Session["EmpNameNId"] = EmpName_Id;
            }
            if (Session["UserId"].ToString() == "superadmin")
            {
                ViewState["StrEmpID"] = "0";
            }
            else
            {
                DataTable dtEmployeeDtl = ObjUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
                ViewState["StrEmpID"] = dtEmployeeDtl.Rows[0]["Emp_Id"].ToString();
                txtGeneratedBy.Text = dtEmployeeDtl.Rows[0]["EmpName"].ToString() + "/" + dtEmployeeDtl.Rows[0]["Emp_Code"].ToString();
                GeneratedBy = txtGeneratedBy.Text;
                dtEmployeeDtl.Dispose();
            }
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            fillUser();
            //cmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54").Rows[0]["Module_Id"].ToString(), "54");
            ddlOption.SelectedIndex = 2;
            FillGrid();
            FillGridBin();
            FillCurrency();
            try
            {
                ddlCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
                strCurrencyId = ddlCurrency.SelectedValue;
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message.ToString());
            }
            Calender.Format = Session["DateFormat"].ToString();
            txtLeadNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = GetDocumentNumber();
            txtLeadDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtLeadDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            int day = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            btnAddCustomer.Visible = IsAddCustomerPermission();
        }
        //AllPageCode();
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
        btnLeadSave.Visible = clsPagePermission.bAdd;
        btnSaveNOpportunity.Visible = clsPagePermission.bAdd;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanAdd.Value = clsPagePermission.bAdd.ToString().ToLower();
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        ImgbtnSelectAll.Visible = clsPagePermission.bRestore;
    }
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "171", "396", "0",Session["BrandId"].ToString(),Session["LocId"].ToString(), Session["DepartmentId"].ToString(),Session["EmpId"].ToString(),Session["TimeZoneId"].ToString());
        return s;
    }
    public void fillUserData(DataTable dtAllData)
    {
        txtLeadNo.Text = dtAllData.Rows[0]["Lead_no"].ToString();
        txtLeadNo.ReadOnly = true;
        txtLeadDate.Text = GetDate(dtAllData.Rows[0]["Lead_date"].ToString());
        txtTitle.Text = dtAllData.Rows[0]["Title"].ToString();
        if (dtAllData.Rows[0]["Lead_status"].ToString() == "")
        {
            ddlLeadStatus.SelectedIndex = 0;
            ddlLeadSource.SelectedIndex = 0;
            txtLeadSourceDesc.Text = "";
            txtLeadStatusDesc.Text = "";
            ddlLeadSource.Enabled = false;
            txtLeadSourceDesc.Enabled = false;
            txtLeadStatusDesc.Enabled = false;
        }
        else
        {
            ddlLeadStatus.SelectedValue = dtAllData.Rows[0]["Lead_status"].ToString();
            txtLeadStatusDesc.Enabled = true;
            ddlLeadSource.Enabled = true;
        }
        if (dtAllData.Rows[0]["Lead_source"].ToString() == "")
        {
            ddlLeadSource.SelectedIndex = 0;
            txtLeadSourceDesc.Enabled = false;
            txtLeadSourceDesc.Text = "";
        }
        else
        {
            ddlLeadSource.SelectedValue = dtAllData.Rows[0]["Lead_source"].ToString();
            txtLeadSourceDesc.Enabled = true;
        }
        txtLeadSourceDesc.Text = dtAllData.Rows[0]["Source_description"].ToString();
        txtLeadStatusDesc.Text = dtAllData.Rows[0]["Status_description"].ToString();
        ddlCurrency.SelectedValue = dtAllData.Rows[0]["Currency_ID"].ToString();
        strCurrencyId = ddlCurrency.SelectedValue;
        txtOppAmt1.Text = dtAllData.Rows[0]["Opportunity_amount"].ToString();
        txtRemark.Text = dtAllData.Rows[0]["Remark"].ToString();
        if (dtAllData.Rows[0]["Campaign_Id"].ToString() != "0")
        {
            txtCampaign1.Text = dtAllData.Rows[0]["Campaign_name"].ToString() + "/" + dtAllData.Rows[0]["Campaign_Id"].ToString();
        }
        else
        {
            txtCampaign1.Text = "";
        }
        if (dtAllData.Rows[0]["Assign_to"].ToString() != "0")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dtAllData.Rows[0]["Assign_to"].ToString());
            txtAssignedTo.Text = dtAllData.Rows[0]["Emp_name_AssignTo"].ToString() + "/" + Emp_Code;
        }
        else
        {
            txtAssignedTo.Text = "";
        }
        if (dtAllData.Rows[0]["Generated_by"].ToString() != "0")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dtAllData.Rows[0]["Generated_by"].ToString());
            txtGeneratedBy.Text = dtAllData.Rows[0]["Emp_name_GeneratedBy"].ToString() + "/" + Emp_Code;
        }
        else
        {
            txtGeneratedBy.Text = "";
        }
        txtReferredBy1.Text = dtAllData.Rows[0]["Refered_by"].ToString();
        if (dtAllData.Rows[0]["Customer_Id"].ToString() != "0")
        {
            txtCustomerName.Text = dtAllData.Rows[0]["Emp_name_Customer"].ToString() + "/" + dtAllData.Rows[0]["Customer_Id"].ToString();
        }
        else
        {
            txtCustomerName.Text = "";
        }
        txtContactList.Text = dtAllData.Rows[0]["Emp_name_Contact"].ToString() + "/" + dtAllData.Rows[0]["Contact_Id"].ToString();
        dtAllData.Dispose();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        string leadid = e.CommandArgument.ToString();
        hdnSalesLeadID.Value = leadid;
        DataTable dtAllData = SLCalss.getActiveLeadDataById(leadid);
        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            btnLeadSave.Visible = false;
            BtnReset.Visible = false;
            btnSaveNOpportunity.Visible = false;
        }
        else
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            BtnReset.Visible = true;
            btnLeadSave.Visible = true;
            btnSaveNOpportunity.Visible = true;
        }
        fillUserData(dtAllData);
    }
    protected void GvSalesLead_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesLead.PageIndex = e.NewPageIndex;
        DataTable dtPaging = (DataTable)Session["fillGridData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesLead, dtPaging, "", "");
        dtPaging.Dispose();
        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillGrid();
        DataTable dtAdd = Session["fillGridData"] as DataTable;
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldName.SelectedItem.Value == "Lead_date")
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
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Date");
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
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                    }
                }
                else
                {
                    DisplayMessage("Enter Some Value");
                    txtValueDate.Focus();
                }
                DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvSalesLead, view.ToTable(), "", "");
                Session["fillGridData"] = view.ToTable();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
                dtAdd.Dispose();
            }
        }
        //AllPageCode();
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedIndex == 0)
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";
        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";
        }
    }
    protected void GvSalesLead_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_Sorting = (DataTable)Session["fillGridData"];
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
        Session["fillGridData"] = dt_Sorting;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesLead, dt_Sorting, "", "");
        dt_Sorting.Dispose();
        //AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        //AllPageCode();
    }
    protected void btnSLeadCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLeadDate);
    }
    protected void GvSalesLeadBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesLeadBin.PageIndex = e.NewPageIndex;
        DataTable dt_Paging_Bin = (DataTable)Session["FillGridBinData"];
        objPageCmn.FillData((object)GvSalesLeadBin, dt_Paging_Bin, "", "");
        for (int i = 0; i < GvSalesLeadBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvSalesLeadBin.Rows[i].FindControl("hdnLeadId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesLeadBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        dt_Paging_Bin.Dispose();
        //AllPageCode();
    }
    public void checkBox_Check(string ids, bool check)
    {
        for (int i = 0; i < GvSalesLeadBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvSalesLeadBin.Rows[i].FindControl("hdnLeadId");
            string[] split = ids.Split(',');
            for (int j = 0; j < ids.Split(',').Length; j++)
            {
                if (ids.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == ids.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesLeadBin.Rows[i].FindControl("chkSelect")).Checked = check;
                    }
                }
            }
        }
    }
    protected void GvSalesLeadBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt_Sorting_Bin = (DataTable)Session["FillGridBinData"];
        DataView dv = new DataView(dt_Sorting_Bin);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt_Sorting_Bin = dv.ToTable();
        Session["FillGridBinData"] = dt_Sorting_Bin;
        objPageCmn.FillData((object)GvSalesLeadBin, dt_Sorting_Bin, "", "");
        lblSelectedRecord.Text = "";
        dt_Sorting_Bin.Dispose();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        if (ddlFieldNameBin.SelectedItem.Value == "Lead_date")
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
            if (txtValueBin.Text.Trim() == "")
            {
                DisplayMessage("Enter Some Value");
            }
            else
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
            }
            DataTable dtCust = Session["FillGridBinData"] as DataTable;
            dtCust = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows).ToTable();
            Session["FillGridBinData"] = dtCust;
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dtCust.Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
           objPageCmn.FillData((object)GvSalesLeadBin, dtCust, "", "");
            lblSelectedRecord.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            dtCust.Dispose();
        }
        //AllPageCode();
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueDateBin.Text != "")
            txtValueDateBin.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesLeadBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSalesLeadBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesLeadBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvSalesLeadBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvSalesLeadBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvSalesLeadBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvSalesLeadBin.Rows[index].FindControl("lblLead_Id");
        if (((CheckBox)GvSalesLeadBin.Rows[index].FindControl("chkSelect")).Checked)
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
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueDateBin.Visible = false;
        txtValueBin.Visible = true;
        txtValueDateBin.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtPbrand = (DataTable)Session["FillGridBinData"];
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Lead_Id"]))
                {
                    lblSelectedRecord.Text += dr["Lead_Id"] + ",";
                }
            }
            for (int i = 0; i < GvSalesLeadBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvSalesLeadBin.Rows[i].FindControl("hdnLeadId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSalesLeadBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["FillGridBinData"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
           objPageCmn.FillData((object)GvSalesLeadBin, dtUnit1, "", "");
            dtUnit1.Dispose();
            ViewState["Select"] = null;
        }
        dtPbrand.Dispose();
        //AllPageCode();
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
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesLeadBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < GvSalesLeadBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesLeadBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvSalesLeadBin.Rows[i].FindControl("hdnLeadId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvSalesLeadBin.Rows[i].FindControl("hdnLeadId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvSalesLeadBin.Rows[i].FindControl("hdnLeadId"))).Value.Trim().ToString())
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
    public string findAllNames()
    {
        DataTable dtAllUsers = new DataTable();
        string nameList = "";
        if (Session["UserId"].ToString() == "superadmin")
        {
            dtAllUsers = new DataView(ObjUser.GetUserMaster(Session["CompId"].ToString()), "Company_Id='" + Session["CompId"].ToString() + "' and Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtAllUsers.Rows.Count; i++)
            {
                if (i == dtAllUsers.Rows.Count - 1)
                {
                    nameList = nameList + "Generated_by =" + dtAllUsers.Rows[i]["Emp_Id"].ToString() + "";
                }
                else
                {
                    nameList = nameList + "Generated_by =" + dtAllUsers.Rows[i]["Emp_Id"].ToString() + " or ";
                }
            }
        }
        else
        {
            dtAllUsers = cmn.handledBy_Code(ViewState["StrEmpID"].ToString(),Session["CompId"].ToString());
            for (int i = 0; i < dtAllUsers.Rows.Count; i++)
            {
                if (i == dtAllUsers.Rows.Count - 1)
                {
                    nameList = nameList + "Generated_by =" + dtAllUsers.Rows[i]["Emp_id"].ToString() + "";
                }
                else
                {
                    nameList = nameList + "Generated_by =" + dtAllUsers.Rows[i]["Emp_id"].ToString() + " or ";
                }
            }
        }
        return nameList;
    }
    private void FillGrid()
    {
        string locid = "";
        DataTable dt_List_grid = new DataTable();
        
        locid = ddlLocation.SelectedIndex == 0 ? "0" : ddlLocation.SelectedValue.ToString();

        dt_List_grid = SLCalss.getAllActiveLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), locid);

        if (ddlUser.SelectedValue != "--Select User--")
        {
            if (dt_List_grid.Rows.Count > 0)
            {
                dt_List_grid = new DataView(dt_List_grid, "Emp_name_GeneratedBy='" + ddlUser.SelectedItem.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
               objPageCmn.FillData((object)GvSalesLead, dt_List_grid, "", "");
            }
            else
            {
               objPageCmn.FillData((object)GvSalesLead, null, "", "");
            }
        }
        else
        {
           objPageCmn.FillData((object)GvSalesLead, dt_List_grid, "", "");
        }
        Session["fillGridData"] = dt_List_grid;
        lblTotalRecords.Text = dt_List_grid == null ? "Total_Records : 0" : "Total_Records :" + dt_List_grid.Rows.Count.ToString();
    }
    public void FillGridBin()
    {
        DataTable dt_Grid_Bin = new DataTable();
        dt_Grid_Bin = SLCalss.getAllInActiveLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
        if (ddlLocation.SelectedItem.Text == "All")
        {
            dt_Grid_Bin = new DataView(dt_Grid_Bin, ViewState["locationCondition"].ToString(), "", DataViewRowState.CurrentRows).ToTable();
        }
        string nameList = findAllNames();
        dt_Grid_Bin = new DataView(dt_Grid_Bin, nameList, "", DataViewRowState.CurrentRows).ToTable();
        Session["FillGridBinData"] = dt_Grid_Bin;
       objPageCmn.FillData((object)GvSalesLeadBin, dt_Grid_Bin, "", "");
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt_Grid_Bin.Rows.Count.ToString() + "";
        if (dt_Grid_Bin.Rows.Count == 0)
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = new DataTable();
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
           objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }
        dsCurrency.Dispose();
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
        return ArebicMessage;
    }
    public void Reset()
    {
        FillCurrency();
        txtLeadNo.ReadOnly = false;
        txtLeadDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtLeadNo.Text = "";
        txtTitle.Text = "";
        ddlLeadSource.SelectedIndex = 0;
        ddlLeadStatus.SelectedIndex = 0;
        txtLeadSourceDesc.Text = "";
        txtLeadStatusDesc.Text = "";
        txtOppAmt1.Text = "";
        txtRemark.Text = "";
        txtCampaign1.Text = "";
        txtAssignedTo.Text = "";
        txtReferredBy1.Text = "";
        txtCustomerName.Text = "";
        txtContactList.Text = "";
        PnlNewContant.Enabled = true;
        txtCustomerName.Text = "";
        txtRemark.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        txtValueDateBin.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtLeadNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = GetDocumentNumber();
        objPageCmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54", (DataTable)Session["ModuleName"]).Rows[0]["Module_Id"].ToString(), "54");
    }
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtCustomerName.Text != "")
        {
            strCustomerId = GetCustomerId();
            Session["ContactID"] = strCustomerId;
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
    private string GetCustomerId()
    {
        string retval = "";
        try
        {
            if (txtCustomerName.Text != "")
            {
                int start_pos = txtCustomerName.Text.LastIndexOf("/") + 1;
                int last_pos = txtCustomerName.Text.Length;
                string id = txtCustomerName.Text.Substring(start_pos, last_pos - start_pos);
                int Last_pos_name = txtCustomerName.Text.LastIndexOf("/");
                string name = txtCustomerName.Text.Substring(0, Last_pos_name - 0);
              
                //DataTable dtSupp = objContact.GetContactByContactName(txtContactList.Text.Trim().Split('/')[0].ToString());
                DataTable dtSupp = objContact.GetContactTrueById(id);
                if (dtSupp.Rows.Count > 0)
                {
                    string condition = "Name like'%" + name + "%'";
                    dtSupp = new DataView(dtSupp, condition, "", DataViewRowState.CurrentRows).ToTable();
                    if (dtSupp.Rows.Count > 0)
                    {
                        retval = id;
                    }
                }
            }
            Session["ContactID"] = retval;
        }
        catch (Exception error)
        {
        }
        return retval;
    }
    protected void txtCampaign1_TextChanged(object sender, EventArgs e)
    {
        string strCampaignId = string.Empty;
        if (txtCampaign1.Text != "")
        {
            strCampaignId = GetCampaignId();
            if (strCampaignId != "" && strCampaignId != "0")
            {
                txtCampaign1.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtCampaign1.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCampaign1);
            }
        }
        else
        {
            txtCampaign1.Text = "";
            txtCampaign1.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCampaign1);
        }
    }

   

    private string GetCampaignId()
    {
        string retval = "";
        try
        {
            if (txtCampaign1.Text != "")
            {
                int start_pos = txtCampaign1.Text.LastIndexOf("/") + 1;
                int last_pos = txtCampaign1.Text.Length;
                string id = txtCampaign1.Text.Substring(start_pos, last_pos - start_pos);
                if (start_pos != 0)
                {
                    int Last_pos_name = txtCampaign1.Text.LastIndexOf("/");
                    string name = txtCampaign1.Text.Substring(0, Last_pos_name - 0);
                    DataTable dtCampaign = new DataTable();
                    dtCampaign = objCampaign.GetCampaignByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), id);
                    dtCampaign = new DataView(dtCampaign, "Campaign_name='" + name + "' and Trans_Id='" + id + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCampaign.Rows.Count > 0)
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
    protected void txtContactList_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtContactList.Text != "")
        {
            strCustomerId = GetContactId();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                txtContactList.Focus();
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
                name = txtContactList.Text.Split('/')[0].ToString();
                //DataTable dtSupp = objContact.GetContactByContactName(txtContactList.Text.Trim().Split('/')[0].ToString());
                DataTable dtSupp = objContact.GetContactTrueById(id);
                if (dtSupp.Rows.Count > 0)
                {
                    string condition = "Name like'%" + name + "%'";
                    dtSupp = new DataView(dtSupp, condition, "", DataViewRowState.CurrentRows).ToTable();
                    //DataTable dtCompany = objContact.GetContactTrueById(retval);
                    if (dtSupp.Rows.Count > 0)
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString());

            string filtertext = "Name like '" + prefixText + "%'";
            DataTable dtCon = new DataView(dtCustomer, filtertext, "", DataViewRowState.CurrentRows).ToTable();
            if (dtCon.Rows.Count == 0)
            {
                dtCon = dtCustomer;
            }
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
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
    public static string[] GetCompletionListCampaign(string prefixText, int count, string contextKey)
    {
        try
        {
            Campaign objCampaign = new Campaign(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCampaign = objCampaign.GetCampaignNameByPreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
            string[] filterlist = new string[dtCampaign.Rows.Count];
            if (dtCampaign.Rows.Count > 0)
            {
                for (int i = 0; i < dtCampaign.Rows.Count; i++)
                {
                    filterlist[i] = dtCampaign.Rows[i]["FilteredText"].ToString();
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
    public static string[] GetCompletionListGeneratedBy(string prefixText, int count, string contextKey)
    {
        try
        {
            EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText.ToString());
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString();
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
            DataTable dt_Customer = ObjContactMaster.GetContactAsPerFilterText(prefixText,id);
            string[] filterlist = new string[dt_Customer.Rows.Count];
            if (dt_Customer.Rows.Count > 0)
            {
                for (int i = 0; i < dt_Customer.Rows.Count; i++)
                {
                    filterlist[i] = dt_Customer.Rows[i]["FilterText"].ToString() ;
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
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    protected void ddlUser_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        //AllPageCode();
    }
    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../EMS/ContactMaster.aspx?Page=SINQ','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (ViewState["StrEmpID"].ToString() == "0")
        {
            allow = true;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "107", "19",Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            allow = true;
        }
        return allow;
    }
    protected void txtAssignedTo_TextChanged(object sender, EventArgs e)
    {
        string strAssignedToId = "";
        if (txtAssignedTo.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strAssignedToId = Emp_ID;
            if (strAssignedToId != "" && strAssignedToId != "0")
            {
                txtAssignedTo.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtAssignedTo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAssignedTo);
            }
        }
        else
        {
            txtAssignedTo.Text = "";
            txtAssignedTo.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtAssignedTo);
        }
    }
    protected void txtGeneratedBy_TextChanged(object sender, EventArgs e)
    {
        string strGeneratedById = string.Empty;
        string name = txtGeneratedBy.Text;
        if (txtGeneratedBy.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtGeneratedBy.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            string Emp_Name = txtGeneratedBy.Text.Split('/')[0].ToString() + "/" + Emp_ID;
            strGeneratedById = Emp_ID;
            //getID(Emp_Name);
            if (strGeneratedById != "" && strGeneratedById != "0")
            {
                txtGeneratedBy.Text = name;
                txtGeneratedBy.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtGeneratedBy.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGeneratedBy);
            }
        }
        else
        {
            txtGeneratedBy.Text = "";
            txtGeneratedBy.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGeneratedBy);
        }
    }
    public string getID(string name)
    {
        string retval = "";
        try
        {
            if (name != "")
            {
                string campaign = name;
                int start_pos = campaign.LastIndexOf("/") + 1;
                int last_pos = campaign.Length;
                string id = campaign.Substring(start_pos, last_pos - start_pos);
                int Last_pos_name = campaign.LastIndexOf("/");
                string empName = campaign.Substring(0, Last_pos_name - 0);
                if (start_pos != 0)
                {
                    DataTable dtEmployee = new DataTable();
                    //dtCampaign = HttpContext.Current.Session["EmpNameNId"] as DataTable;
                    dtEmployee = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), id);
                    string filtertext = "Emp_name like'%" + empName.Trim() + "%'";
                    dtEmployee = new DataView(dtEmployee, filtertext, "", DataViewRowState.CurrentRows).ToTable();
                    if (dtEmployee.Rows.Count > 0)
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
    public bool checkValidation()
    {
        DateTime date = Convert.ToDateTime(System.DateTime.Now.Date.ToString());
        int parsedValue;
        float parseddecimal;
        if (txtLeadDate.Text == "")
        {
            DisplayMessage("Enter Sales Inquiry date");
            txtLeadDate.Focus();
            return false;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtLeadDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Sales Inquiry Date in format " + Session["DateFormat"].ToString() + "");
                txtLeadDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLeadDate);
                return false;
            }
        }
        if (txtTitle.Text == "")
        {
            DisplayMessage("Please Enter Lead Title");
            txtTitle.Focus();
            return false;
        }
        if (txtContactList.Text == "")
        {
            DisplayMessage("Please Enter Contact Information");
            txtContactList.Focus();
            return false;
        }
        if (!int.TryParse(txtOppAmt1.Text, out parsedValue))
        {
            if (!float.TryParse(txtOppAmt1.Text, out parseddecimal))
            {
                DisplayMessage("Amount entered was not in correct format");
                txtOppAmt1.Text = "";
                txtOppAmt1.Focus();
                return false;
            }
        }
        return true;
    }
    protected void btnLeadSave_Click(object sender, EventArgs e)
    {
        btnLeadSave.Enabled = false;
        string custid, contactid, campaignId, generatedById, assignedToId, sourceDesc, statusdesc, strDocNo = string.Empty;
        int start_pos, last_pos;
        if (!checkValidation())
        {
            btnLeadSave.Enabled = true;
            return;
        }
        if (txtCustomerName.Text == "")
        {
            custid = "0";
        }
        else
        {
            custid = txtCustomerName.Text.Trim().Split('/')[1].ToString();
        }
        contactid = txtContactList.Text.Trim().Split('/')[1].ToString();
        if (txtCampaign1.Text == "")
        {
            campaignId = "0";
        }
        else
        {
            start_pos = txtCampaign1.Text.LastIndexOf("/") + 1;
            last_pos = txtCampaign1.Text.Length;
            campaignId = txtCampaign1.Text.Substring(start_pos, last_pos - start_pos);
        }
        if (txtGeneratedBy.Text == "")
        {
            generatedById = ViewState["StrEmpID"].ToString();
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtGeneratedBy.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            //start_pos = txtGeneratedBy.Text.LastIndexOf("/") + 1;
            //last_pos = txtGeneratedBy.Text.Length;
            //generatedById = txtGeneratedBy.Text.Substring(start_pos, last_pos - start_pos);
            generatedById = Emp_ID;
        }
        if (txtAssignedTo.Text == "")
        {
            assignedToId = "0";
        }
        else
        {
            //start_pos = txtAssignedTo.Text.LastIndexOf("/") + 1;
            //last_pos = txtAssignedTo.Text.Length;
            //assignedToId = txtAssignedTo.Text.Substring(start_pos, last_pos - start_pos);
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
            assignedToId = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
        }
        if (ddlLeadSource.SelectedValue == "Select")
        {
            sourceDesc = "";
        }
        else
        {
            sourceDesc = ddlLeadSource.SelectedItem.ToString();
        }
        if (ddlLeadStatus.SelectedValue == "Select")
        {
            statusdesc = "";
        }
        else
        {
            statusdesc = ddlLeadStatus.SelectedItem.ToString();
        }
        try
        {
            if (Lbl_Tab_New.Text == "Edit")
            {
                SLCalss.UpdateLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, txtLeadNo.Text, ObjSysParam.getDateForInput(txtLeadDate.Text).ToString(), custid, contactid, txtTitle.Text, sourceDesc, statusdesc, ddlCurrency.SelectedValue, txtOppAmt1.Text, txtLeadSourceDesc.Text, txtLeadStatusDesc.Text, txtRemark.Text, generatedById, assignedToId, campaignId, txtReferredBy1.Text, ViewState["StrEmpID"].ToString(), ViewState["StrEmpID"].ToString());
                DisplayMessage("Record Updated Successfully", "green");
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                DataTable dt_LeadNum = SLCalss.GetLeadNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (ViewState["DocNo"].ToString() == txtLeadNo.Text)
                {
                    strDocNo = txtLeadNo.Text + dt_LeadNum.Rows[0][0].ToString();
                }
                else
                {
                    strDocNo = txtLeadNo.Text;
                }
                SLCalss.insertLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDocNo, ObjSysParam.getDateForInput(txtLeadDate.Text).ToString(), custid, contactid, txtTitle.Text, sourceDesc, statusdesc, ddlCurrency.SelectedValue, txtOppAmt1.Text, txtLeadSourceDesc.Text, txtLeadStatusDesc.Text, txtRemark.Text, generatedById, assignedToId, campaignId, txtReferredBy1.Text, ViewState["StrEmpID"].ToString(), ViewState["StrEmpID"].ToString());
                DisplayMessage("Record Saved Successfully", "green");
                Reset();
                FillGrid();
            }
        }
        catch (Exception error)
        {
            DisplayMessage("Record Not Saved");
        }
        if (Session["UserId"].ToString() == "superadmin")
        {
            ViewState["StrEmpID"] = "0";
            txtGeneratedBy.Text = "";
        }
        else
        {
            DataTable dtEmployeeDtl = ObjUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
            ViewState["StrEmpID"] = dtEmployeeDtl.Rows[0]["Emp_Id"].ToString();
            txtGeneratedBy.Text = dtEmployeeDtl.Rows[0]["EmpName"].ToString() + "/" + dtEmployeeDtl.Rows[0]["Emp_Code"].ToString();
            GeneratedBy = txtGeneratedBy.Text;
            txtGeneratedBy.Text = GeneratedBy;
        }
        btnLeadSave.Enabled = true;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string leadid = e.CommandArgument.ToString();
        DateTime date = Convert.ToDateTime(System.DateTime.Now.ToString());
        string id = "";
        if (Session["UserId"].ToString() == "superadmin")
        {
            id = "0";
        }
        else
        {
            id = Session["UserId"].ToString();
        }
        SLCalss.RecordInActivate(leadid, id);
        DisplayMessage("Record Delete");
        Reset();
        FillGridBin();
        FillGrid();
        //AllPageCode();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string id = "";
        DateTime date = Convert.ToDateTime(System.DateTime.Now.ToString());
        if (Session["UserId"].ToString() == "superadmin")
        {
            id = "0";
        }
        else
        {
            id = Session["UserId"].ToString();
        }
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    SLCalss.RecordActivate(lblSelectedRecord.Text.Split(',')[j].Trim(), id);
                    FillGridBin();
                    FillGrid();
                    DisplayMessage("Record Activate");
                }
            }
        }
        int fleg = 0;
        foreach (GridViewRow Gvr in GvSalesLeadBin.Rows)
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
        //AllPageCode();
    }
    protected void btnSaveNOpportunity_Click(object sender, EventArgs e)
    {
        string custid, contactid, campaignId, generatedById, assignedToId, sourceDesc, statusdesc, strDocNo = string.Empty;
        if (!checkValidation())
        {
            btnLeadSave.Enabled = true;
            return;
        }
        if (txtCustomerName.Text == "")
        {
            custid = "0";
        }
        else
        {
            custid = txtCustomerName.Text.Trim().Split('/')[1].ToString();
        }
        contactid = txtContactList.Text.Trim().Split('/')[1].ToString();
        if (txtCampaign1.Text == "")
        {
            campaignId = "0";
        }
        else
        {
            campaignId = txtCampaign1.Text.Trim().Split('/')[1].ToString();
        }
        if (txtGeneratedBy.Text == "")
        {
            generatedById = ViewState["StrEmpID"].ToString();
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtGeneratedBy.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            generatedById = Emp_ID;
        }
        if (txtAssignedTo.Text == "")
        {
            assignedToId = "0";
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
            assignedToId = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
        }
        if (ddlLeadSource.SelectedValue == "Select")
        {
            sourceDesc = "";
        }
        else
        {
            sourceDesc = ddlLeadSource.SelectedItem.ToString();
        }
        if (ddlLeadStatus.SelectedValue == "Select")
        {
            statusdesc = "";
        }
        else
        {
            statusdesc = ddlLeadStatus.SelectedItem.ToString();
        }
        try
        {
            if (Lbl_Tab_New.Text == "Edit")
            {
                SLCalss.UpdateLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, txtLeadNo.Text, ObjSysParam.getDateForInput(txtLeadDate.Text).ToString(), custid, contactid, txtTitle.Text, sourceDesc, statusdesc, ddlCurrency.SelectedValue, txtOppAmt1.Text, txtLeadSourceDesc.Text, txtLeadStatusDesc.Text, txtRemark.Text, generatedById, assignedToId, campaignId, txtReferredBy1.Text, ViewState["StrEmpID"].ToString(), ViewState["StrEmpID"].ToString());
                //DataTable DtLeadDataByID= SLCalss.getActiveLeadDataById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, hdnSalesLeadID.Value);
                Response.Redirect("../Sales/SalesInquiry.aspx?SalesLeadID=" + hdnSalesLeadID.Value);
            }
            else
            {
                DataTable dt_LeadNum = SLCalss.GetLeadNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                if (ViewState["DocNo"].ToString() == txtLeadNo.Text)
                {
                    strDocNo = txtLeadNo.Text + dt_LeadNum.Rows[0][0].ToString();
                }
                else
                {
                    strDocNo = txtLeadNo.Text;
                }
                SLCalss.insertLeadData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDocNo, ObjSysParam.getDateForInput(txtLeadDate.Text).ToString(), custid, contactid, txtTitle.Text, sourceDesc, statusdesc, ddlCurrency.SelectedValue, txtOppAmt1.Text, txtLeadSourceDesc.Text, txtLeadStatusDesc.Text, txtRemark.Text, generatedById, assignedToId, campaignId, txtReferredBy1.Text, ViewState["StrEmpID"].ToString(), ViewState["StrEmpID"].ToString());
                DataTable DtLeadIDFromLeadNo = SLCalss.GetLeadIDFromLeadNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strDocNo);
                Response.Redirect("../Sales/SalesInquiry.aspx?SalesLeadID=" + DtLeadIDFromLeadNo.Rows[0]["Lead_Id"].ToString());
            }
        }
        catch (Exception error)
        {
            DisplayMessage("Record Not Saved");
        }
    }
    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedIndex == 2)
        {
            txtValueDateBin.Visible = true;
            txtValueBin.Visible = false;
        }
        else
        {
            txtValueDateBin.Visible = false;
            txtValueBin.Visible = true;
        }
    }
    protected void txtLeadNo_TextChanged(object sender, EventArgs e)
    {
        DataTable dt_Duplicacy = SLCalss.CheckDuplicacy(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtLeadNo.Text);
        if (dt_Duplicacy.Rows.Count != 0)
        {
            DisplayMessage("Lead Already Exist");
            txtLeadNo.Text = GetDocumentNumber();
            txtLeadNo.Focus();
            return;
        }
    }
    protected void ddlLeadStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLeadStatus.SelectedIndex != 0)
        {
            ddlLeadSource.Enabled = true;
            txtLeadStatusDesc.Enabled = true;
        }
        else
        {
            ddlLeadSource.SelectedIndex = 0;
            txtLeadSourceDesc.Text = "";
            txtLeadStatusDesc.Text = "";
            ddlLeadSource.Enabled = false;
            txtLeadSourceDesc.Enabled = false;
            txtLeadStatusDesc.Enabled = false;
        }
    }
    protected void ddlLeadSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLeadSource.SelectedIndex != 0)
        {
            txtLeadSourceDesc.Enabled = true;
        }
        else
        {
            txtLeadSourceDesc.Text = "";
            txtLeadSourceDesc.Enabled = false;
        }
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        objPageCmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54", (DataTable)Session["ModuleName"]).Rows[0]["Module_Id"].ToString(), "54", ddlLocation.SelectedValue);
        FillGrid();
        FillGridBin();
        //AllPageCode();
    }
    public void fillLocation()
    {
        ViewState["locationCondition"] = "Location_Id=";
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.Items.Add(new ListItem("All", "0"));
            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));
                if (i == dtLoc.Rows.Count - 1)
                {
                    ViewState["locationCondition"] = ViewState["locationCondition"].ToString() + dtLoc.Rows[i]["Location_Id"].ToString();
                }
                else
                {
                    ViewState["locationCondition"] = ViewState["locationCondition"].ToString() + dtLoc.Rows[i]["Location_Id"].ToString() + " or Location_Id=";
                }
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
    }
    public void fillUser()
    {
        objPageCmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54", (DataTable)Session["ModuleName"]).Rows[0]["Module_Id"].ToString(), "54", ddlLocation.SelectedValue, ViewState["locationCondition"].ToString());
    }

    protected void btnAssignTask_Command(object sender, CommandEventArgs e)
    {
        Session["SalesLeadData"] = "";
        lblId.Text = e.CommandArgument.ToString();
        try
        {
            Session["SalesLeadData"] = e.CommandArgument.ToString();
            AddTaskUC.requestData();
            AddTaskUC.fillDate();
        }
        catch (Exception ex)
        {
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = new DataTable();
        try
        {
            dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText.ToString());
        }
        catch
        {
            dtCon = new DataTable();
        }
        string[] txt = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                txt[i] += dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString() + "/" + dtCon.Rows[i]["Emp_Id"].ToString();
            }
        }
        return txt;
    }



    protected void lnkAddNewContact_Click(object sender, EventArgs e)
    {
        if (txtCustomerName.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomerName.Focus();
            return;
        }
        DataTable dt = ObjCustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
}