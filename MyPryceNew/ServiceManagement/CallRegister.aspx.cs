using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;

public partial class ServiceManagement_CallRegister : System.Web.UI.Page
{
    SM_Ticket_Master objTicketMaster = null;
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    CallLogs ObjCustInquiry = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    Sm_TicketFeedback ObjTicketFeedback = null;
    Common cmn = null;
    PageControlsSetting objPageCtlSettting = null;
    PageControlCommon objPageCmn = null;

    public const int grdDefaultColCount = 9;
    private const string strPageName = "CallRegister";
    //modified by divya on 14/3/2018
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objTicketMaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjCustInquiry = new CallLogs(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjTicketFeedback = new Sm_TicketFeedback(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/CallRegister.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLoc);
            Reset();
            fillUser();
            // btnList_Click(null, null);
            txtCallTime.Text = DateTime.Now.ToString("hh:mm");
            btnAddCustomer.Visible = IsAddCustomerPermission();

            if (Request.QueryString["Followup_CustomerID"] != null)
            {
                DataTable DtDataByID = objContact.GetContactTrueById(Request.QueryString["Followup_CustomerID"].ToString());
                txtECustomer.Text = DtDataByID.Rows[0]["Name"].ToString() + "///" + DtDataByID.Rows[0]["Trans_Id"].ToString();
                txtEContact.Text = DtDataByID.Rows[0]["Name"].ToString() + "///" + DtDataByID.Rows[0]["Trans_Id"].ToString();
                txtContactNo.Text = DtDataByID.Rows[0]["Field2"].ToString();
                txtEmailId.Text = DtDataByID.Rows[0]["Field1"].ToString();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCINo);
                if (!ClientScript.IsStartupScriptRegistered("alert"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alert", "alertMe();", true);
                }
            }

            if (Request.QueryString["SearchField"] != null)
            {
                ddlStatusFilter.SelectedIndex = 0;
            }
            getPageControlsVisibility();
            FillGrid();
        }
        try
        {
            FillGrid();
        }
        catch
        {

        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnInquirySave.Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        btnControlsSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillBinGrid();
        ddlFieldNameBin.Focus();
    }


    //protected void btnRefreshReport_Click(object sender, EventArgs e)
    //{

    //    FillGrid();
    //    FillBinGrid();
    //    ddlFieldName.SelectedIndex = 0;
    //    ddlOption.SelectedIndex = 2;
    //    txtValue.Text = "";
    //    ddlFieldNameBin.Focus();
    //}
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {
        if (txtCINo.Text == "")
        {
            DisplayMessage("Enter Call No");
            txtCINo.Focus();
            return;
        }
        if (txtCIDate.Text == "")
        {
            DisplayMessage("Enter Call Date");
            txtCIDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtCIDate.Text.Trim());
            }
            catch
            {
                txtCIDate.Text = "";
                txtCIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
                txtCIDate.Focus();

                DisplayMessage("Enter Date");
                return;
            }
        }


        string StrCustomer = string.Empty;

        string StrContact = string.Empty;

        bool isExistCustomer = false;
        bool isExistContact = false;


        if (txtECustomer.Text == "")
        {
            txtECustomer.Focus();
            DisplayMessage("Enter Customer Name");
            return;
        }
        else
        {
            try
            {
                StrCustomer = txtECustomer.Text.Split('/')[3].ToString();
                isExistCustomer = true;
            }
            catch
            {
                StrCustomer = txtECustomer.Text;
                isExistCustomer = false;
            }

        }



        if (txtEContact.Text != "")
        {
            try
            {
                StrContact = txtEContact.Text.Split('/')[3].ToString();
                isExistContact = true;

            }
            catch
            {
                StrContact = txtEContact.Text;
                isExistContact = false;
            }

        }




        //if (txtContactNo.Text == "")
        //{

        //    DisplayMessage("Enter Contact No");
        //    txtContactNo.Focus();
        //    return;
        //}

        if (txtEmailId.Text != "")
        {
            if (!IsValidEmailId(txtEmailId.Text))
            {
                DisplayMessage("Enter Valid EmailId");
                txtEmailId.Focus();
                return;
            }
        }
        string strhandledemployee = string.Empty;

        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Select Handled Employee in suggestion");
            txtHandledEmp.Focus();
            return;
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strhandledemployee = Emp_ID;
        }



        if (ddlCallType.SelectedIndex == 0)
        {
            DisplayMessage("Select Call Type");
            ddlCallType.Focus();
            return;
        }


        string strRefTo = string.Empty;
        if (txtRefTo.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtRefTo.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strRefTo = Emp_ID;
        }
        else
        {
            strRefTo = "0";
        }

        if (txtCallDetail.Text == "")
        {
            DisplayMessage("Enter Call Detail");
            txtCallDetail.Focus();
            return;
        }

        //Check controls Value from page setting
        string[] result = objPageCtlSettting.validateControlsSetting(strPageName, this.Page);
        if (result[0] == "false")
        {
            DisplayMessage(result[1]);
            return;
        }

        if (hdnValue.Value == "")
        {
            

            if (txtCINo.Text != "")
            {
                if ((new DataView(ObjCustInquiry.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue), "Call_No='" + txtCINo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0))
                {
                    DisplayMessage("Call No Already Exist");
                    txtCINo.Text = "";
                    return;
                }
            }


            int b = 0;


            b = ObjCustInquiry.InsertCallLogs(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtCINo.Text.Trim(), objSysParam.getDateForInput(txtCIDate.Text.Trim()).ToString(), StrCustomer.Trim(), StrContact, txtContactNo.Text.Trim(), txtEmailId.Text.Trim(), ddlCallType.SelectedValue.ToString(), txtCallDetail.Text, strRefTo, ddlPriority.SelectedValue, ddlStatus.SelectedValue, txtNotes.Text, "Not Generated", strhandledemployee, "", "", isExistContact.ToString(), isExistCustomer.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            string strMaxId = string.Empty;

            if (b != 0)
            {
                strMaxId = b.ToString();

                string count = ObjCustInquiry.getCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue);
                ObjCustInquiry.Updatecode(b.ToString(), txtCINo.Text + count);
                txtCINo.Text = txtCINo.Text + count;

                DisplayMessage("Record Saved", "green");

                if (ddlSetReminder.SelectedIndex == 1)
                {
                    string message = "New Call has been made for you against Customer: '" + txtECustomer.Text + "'";

                    string empid = "0";
                    try
                    {
                        empid = new HR_EmployeeDetail(Session["DBConnection"].ToString()).GetEmployeeId(txtHandledEmp.Text.Split('/')[1].ToString());
                    }
                    catch
                    {
                        DisplayMessage("Please Enter Valid Handled Employee Name");
                        txtHandledEmp.Text = "";
                        txtHandledEmp.Focus();
                        return;
                    }

                    int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, "SM_Call_logs", b.ToString(), message, "../ServiceManagement/CallRegister.aspx?SearchField=" + txtCINo.Text + "", System.DateTime.Now.ToString(), "1", txtCIDate.Text, "Once", empid, "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                    new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), txtCIDate.Text, "", Session["UserId"].ToString(), Session["UserId"].ToString());
                }
                Reset();
                FillGrid();
            }
        }
        else
        {
            ObjCustInquiry.UpdateCallLogs(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, txtCINo.Text.Trim(), objSysParam.getDateForInput(txtCIDate.Text.Trim()).ToString(), StrCustomer.Trim(), StrContact, txtContactNo.Text.Trim(), txtEmailId.Text.Trim(), ddlCallType.SelectedValue.ToString(), txtCallDetail.Text, strRefTo, ddlPriority.SelectedValue, ddlStatus.SelectedValue, txtNotes.Text, "Not Generated", strhandledemployee, "", "", isExistContact.ToString(), isExistCustomer.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());
            // btnList_Click(null, null);
            DisplayMessage("Record Updated", "green");
            Reset();
            FillGrid();
        }
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        
        //btnList_Click(null, null);
    }
    protected void GvCustomerInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtCallRegisterData"];
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
        Session["dtCallRegisterData"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCallRegister, dt, "", "");

        //AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCallRegister.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCallRegisterData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCallRegister, dt, "", "");

        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ddlLoc.Enabled = false;
        ddlLoc.SelectedValue = e.CommandArgument.ToString().Split(',')[1].ToString();
        DataTable dt = ObjCustInquiry.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString().Split(',')[1].ToString(), e.CommandArgument.ToString().Split(',')[0].ToString());
        if (dt.Rows.Count != 0)
        {
            TrIn.Visible = true;
            LinkButton b = (LinkButton)sender;
            string objSenderID = b.ID;

            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
            }
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

            //btnNew_Click(null, null);
            hdnValue.Value = e.CommandArgument.ToString().Split(',')[0].ToString();
            txtCIDate.Text = Convert.ToDateTime(dt.Rows[0]["Call_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtCINo.Text = dt.Rows[0]["Call_No"].ToString();
            txtCallTime.Text = Convert.ToDateTime(dt.Rows[0]["CreatedDate"].ToString()).ToString("hh:mm");
            //Editor1.Content = dt.Rows[0]["Description"].ToString();

            txtContactNo.Text = dt.Rows[0]["Contact_No"].ToString();
            txtEmailId.Text = dt.Rows[0]["Email_Id"].ToString();
            if (dt.Rows[0]["Status"].ToString() != "")
            {
                ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
            }
            else
            {
                ddlStatus.SelectedIndex = 1;
            }
            if (dt.Rows[0]["Call_Type"].ToString() != "")
            {
                ddlCallType.SelectedValue = dt.Rows[0]["Call_Type"].ToString();
            }
            else
            {
                ddlCallType.SelectedIndex = 0;
            }


            ddlCallType_OnSelectedIndexChanged(null, null);
            //if (ddlCallType.SelectedIndex == 1)
            //{

            if (dt.Rows[0]["Field2"].ToString().Trim() != "0")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Field2"].ToString());
                txtHandledEmp.Text = dt.Rows[0]["HandledEmployeeName"].ToString() + "/" + Emp_Code;
            }
            else
            {
                txtHandledEmp.Text = "";
            }
            //}
            //for get customer
            if (dt.Rows[0]["Field6"].ToString() == "True")
            {
                DataTable dtcon = objContact.GetContactTrueById(dt.Rows[0]["Customer_Name"].ToString());

                txtECustomer.Text = dtcon.Rows[0]["Name"].ToString() + "/" + dtcon.Rows[0]["Field2"].ToString() + "/" + dtcon.Rows[0]["Field1"].ToString() + "/" + dtcon.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                txtECustomer.Text = dt.Rows[0]["Customer_Name"].ToString();

            }

            //for get Contact
            if (dt.Rows[0]["Field5"].ToString() == "True")
            {
                DataTable dtcon = objContact.GetContactTrueById(dt.Rows[0]["Contact_Person"].ToString());

                txtEContact.Text = dtcon.Rows[0]["Name"].ToString() + "/" + dtcon.Rows[0]["Field2"].ToString() + "/" + dtcon.Rows[0]["Field1"].ToString() + "/" + dtcon.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                txtEContact.Text = dt.Rows[0]["Contact_Person"].ToString();

            }
            ddlPriority.SelectedValue = dt.Rows[0]["Priority"].ToString();
            txtCallDetail.Text = dt.Rows[0]["Call_Detail"].ToString();
            txtNotes.Text = dt.Rows[0]["Notes"].ToString();
            if (dt.Rows[0]["Receive_By"].ToString() != "0")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Receive_By"].ToString());
                txtRefTo.Text = dt.Rows[0]["EmployeeName"].ToString() + "/" + Emp_Code;
            }

        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        btnInquirySave.Enabled = false;
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtTicket = objTicketMaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString().Split(',')[1].ToString());
        ObjTicketFeedback.GetRecord_ByTransId(e.CommandArgument.ToString().Split(',')[0].ToString());
        try
        {
            dtTicket = new DataView(dtTicket, "Call_Id=" + e.CommandArgument.ToString().Split(',')[0].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtTicket.Rows.Count > 0)
        {
            DisplayMessage("You Can Not delete ,Ticket generated against this call");
            return;
        }

        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString().Split(',')[0].ToString();
        b = ObjCustInquiry.DeleteCallLogs(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString().Split(',')[1].ToString(), hdnValue.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillBinGrid(); //Update grid view in bin tab
        FillGrid();
        Reset();
        //AllPageCode();

    }
    protected void txtECustomer_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtECustomer.Text != "")
        {
            try
            {
                strCustomerId = txtECustomer.Text.Split('/')[3].ToString();
            }
            catch
            {
                strCustomerId = "0";

            }
            if (strCustomerId == "0")
            {
                //DisplayMessage("Select In Suggestions Only");
                //txtECustomer.Text = "";
                //txtEContact.Text = "";
                //txtContactNo.Text = "";
                //txtEmailId.Text = "";
                //txtECustomer.Focus();
                //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtECustomer);
                txtContactNo.Text = "";
                txtEContact.Text = "";
                txtEmailId.Text = "";
                Session["ContactID"] = "0";
            }
            else
            {
                txtContactNo.Text = txtECustomer.Text.Split('/')[1].ToString();
                txtEmailId.Text = txtECustomer.Text.Split('/')[2].ToString();
                DataTable dt = objContact.GetContactTrueAllData(strCustomerId, "Individual");
                txtEContact.Text = "";
                if (dt.Rows.Count > 0)
                {
                    txtEContact.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Field1"].ToString() + "/" + dt.Rows[0]["Trans_Id"].ToString();
                }
                else
                {
                    txtEContact.Text = txtECustomer.Text;
                }
                txtEContact_TextChanged(null, null);
                txtEContact.Focus();
                Session["ContactID"] = strCustomerId;
            }
            txtEContact.Focus();

        }
        // AllPageCode();
    }
    protected void txtEContact_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtEContact.Text != "")
        {
            try
            {
                strCustomerId = txtEContact.Text.Split('/')[3].ToString();

            }
            catch
            {
                strCustomerId = "0";

            }
            if (strCustomerId == "0")
            {
                //DisplayMessage("Select In Suggestions Only");
                //txtEContact.Text = "";
                //txtContactNo.Text = "";
                //txtEmailId.Text = "";
                //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEContact);
            }
            else
            {
                DataTable dtcust = objContact.GetContactTrueById(strCustomerId.ToString());

                if (dtcust.Rows.Count > 0)
                {
                    if (dtcust.Rows[0]["Field2"].ToString() != "")
                    {

                        txtContactNo.Text = dtcust.Rows[0]["Field2"].ToString();
                    }
                    if (dtcust.Rows[0]["Field1"].ToString() != "")
                    {
                        txtEmailId.Text = dtcust.Rows[0]["Field1"].ToString();
                    }
                }
            }

            txtContactNo.Focus();
        }
        // AllPageCode();
    }
    protected void txtRefTo_TextChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        if (txtRefTo.Text != "")
        {
            txtHandledEmp_TextChanged(sender, e);
        }
    }
    protected void txtHandledEmp_TextChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;

        TextBox tb = sender as TextBox;

        if (tb.Text != "")
        {
            try
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = tb.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                DataTable dt_emp_dtls = HR_EmployeeDetail.GetEmpName_Code(Emp_ID);
                if (dt_emp_dtls.Rows.Count > 0)
                {
                    if (tb.Text.Split('/')[0].ToString() != dt_emp_dtls.Rows[0]["Emp_Name"].ToString())
                    {
                        strEmpId = "0";
                    }
                    else
                    {
                        strEmpId = Emp_ID;
                        txtCallDetail.Focus();
                    }
                }
                else
                {
                    strEmpId = "0";
                }

            }
            catch
            {
                strEmpId = "0";
            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                tb.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(tb);
                return;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtCustomer, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Field2"].ToString() + "/" + dtCon.Rows[i]["Field1"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = HttpContext.Current.Session["ContactID"].ToString();


        if (id == "0")
        {
            dt = ObjContactMaster.GetContactTrueAllData();
            string filtertext = "Name like '%" + prefixText + "%'";
            dt = new DataView(dt, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = ObjContactMaster.GetContactTrueAllData(id, "Individual");
        }



        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Field1"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
            }
            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Field2"].ToString() + "/" + dtcon.Rows[i]["Field1"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            return filterlistcon;

        }

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefTo(string prefixText, int count, string contextKey)
    {

        EmployeeMaster ObjEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCon = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



        //DataTable dtMain = new DataTable();
        //dtMain = dt.Copy();


        //string filtertext = "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')";
        //DataTable dtCon = new DataView(dt, filtertext, "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();

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
        ddlLoc.SelectedValue = Session["LocId"].ToString();
        ddlLoc.Enabled = true;
        txtECustomer.Text = "";
        txtEContact.Text = "";
        ddlPriority.SelectedIndex = 0;
        ddlCallType.SelectedIndex = 0;
        //ddlStatus.SelectedIndex = 0;
        txtContactNo.Text = "";
        txtEmailId.Text = "";
        txtRefTo.Text = GetEmpName();
        txtCallDetail.Text = "";
        txtNotes.Text = "";
        //txtCIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
        txtCIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        ////Editor1.Content = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtCINo.Text = "";
        txtCINo.Text = GetDocumentNumber(ddlLoc.SelectedValue);
        ViewState["DocNo"] = txtCINo.Text;
        hdnValue.Value = "";
        ViewState["Select"] = null;
        TrIn.Visible = true;
        txtCallTime.Text = DateTime.Now.ToString("hh:mm");
        txtCIDate.Focus();
        ddlCallType_OnSelectedIndexChanged(null, null);
        ddlStatus.SelectedIndex = 0;
        txtHandledEmp.Text = "";
        btnInquirySave.Enabled = true;
    }

    public void FillGrid()
    {
        DataTable dt = new DataTable();

        if (hdnEmpList.Value == "")
        {
            fillUser();
        }

        dt = ObjCustInquiry.GetAllRecordsEmpHandledBy(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, hdnEmpList.Value);

        if (ddlStatusFilter.SelectedIndex != 0)
        {
            dt = new DataView(dt, "Status='" + ddlStatusFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //cmn.FillData((object)GvCustomerInquiry, dt, "", "");

        GvCallRegister.DataSource = dt;
        GvCallRegister.DataBind();

        // lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";

        Session["dtCallRegisterData"] = dt;
        //AllPageCode();
    }
    public void FillBinGrid()
    {
        DataTable dt = ObjCustInquiry.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");


        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtCallRegisterBinData"] = dt;
        //AllPageCode();
    }
    protected void GvCustomerInquiryBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Select"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjCustInquiry.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtCallRegisterBinData"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();

    }
    protected void GvCustomerInquiryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerInquiryBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtCallRegisterBinData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");


        string temp = string.Empty;

        for (int i = 0; i < GvCustomerInquiryBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvCustomerInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        //AllPageCode();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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


            DataTable dtCust = (DataTable)Session["dtCallRegisterBinData"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtCallRegisterBinData"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiryBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillBinGrid();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnRefreshBin);
        }
        //AllPageCode();
        txtValueBin.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillBinGrid();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        DataTable dt = ObjCustInquiry.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        if (GvCustomerInquiryBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = ObjCustInquiry.DeleteCallLogs(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillBinGrid();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvCustomerInquiryBin.Rows)
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        DataTable dtCustInq = (DataTable)Session["dtCallRegisterBinData"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtCustInq.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_ID"]))
                {
                    lblSelectedRecord.Text += dr["Trans_ID"] + ",";
                }
            }
            for (int i = 0; i < GvCustomerInquiryBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvCustomerInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtCustInqiury = (DataTable)Session["dtCallRegisterBinData"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiryBin, dtCustInqiury, "", "");

            ViewState["Select"] = null;
        }
        // AllPageCode();

    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvCustomerInquiryBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvCustomerInquiryBin.Rows.Count; i++)
        {
            ((CheckBox)GvCustomerInquiryBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvCustomerInquiryBin.Rows[index].FindControl("lblgvInquiryId");
        if (((CheckBox)GvCustomerInquiryBin.Rows[index].FindControl("chkSelect")).Checked)
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
    protected string GetDocumentNumber(string locationId)
    {
        string s = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), locationId, true, "158", "270", "0", Session["BrandId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    public string GetEmpName()
    {
        string EmpName = string.Empty;

        DataTable dtEmployee = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
        try
        {
            EmpName = dtEmployee.Rows[0]["Emp_Name"].ToString() + "/" + dtEmployee.Rows[0]["Emp_Code"].ToString();
        }
        catch
        {
        }

        return EmpName;
    }
    #region CheckValidEmailId
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
    #endregion
    #region ViewFeedback
    protected void lnkFeedbackDetail_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjTicketFeedback.GetAllRecord();
        DataTable dtTicket = objTicketMaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString().Split(',')[1].ToString());
        try
        {
            dtTicket = new DataView(dtTicket, "Call_Id=" + e.CommandArgument.ToString().Split(',')[0].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dtTicket.Rows.Count == 0)
        {
            DisplayMessage("Ticket not generated !");
            return;
        }

        try
        {
            //TicketId

            dt = new DataView(dt, "Ticket_No=" + dtTicket.Rows[0]["Trans_Id"].ToString() + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dt == null)
        {
            DisplayMessage("Feedback not found !");
            return;
        }
        else
        {
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Feedback not found !");
                return;
            }
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../ServiceManagement/ViewFeedBack.aspx?TicketId=" + HttpUtility.UrlEncode(Encrypt(dtTicket.Rows[0]["Trans_Id"].ToString())) + "','window','width=1024');", true);

    }
    #endregion
    //protected void Timer1_Tick(object sender, EventArgs e)
    //{

    //    txtCallTime.Text = DateTime.Now.ToString("hh:mm");
    //}
    protected void ddlCallType_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.PaperKind = System.Drawing.Printing.PaperKind.A3;
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WriteCsvToResponse();
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    #region StatusFilter

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    #endregion

    #region AddCustomer

    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../EMS/ContactMaster.aspx?Page=SINV','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    public bool IsAddCustomerPermission()
    {
        bool allow = false;
        if (Session["EmpId"].ToString() == "0")
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

    #endregion

    public void fillUser()
    {
        //ddlUser.Items.Clear();

        DataTable dt = objEmpMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["EmpId"].ToString() != "0")
        {
            dt = new DataView(dt, "Emp_Id in (" + GetTlList() + ")", "", DataViewRowState.CurrentRows).ToTable();
        }

        string empList = "";
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (i == dt.Rows.Count)
            {
                empList = empList + dt.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                empList = empList + dt.Rows[i]["Emp_Id"].ToString() + ",";
            }
        }

        hdnEmpList.Value = empList;

        //cmn.FillData((object)ddlUser, dt, "Emp_Name", "Emp_Id");
        //ddlUser.DataSource = dt;
        //ddlUser.DataTextField = "Emp_Name";
        //ddlUser.DataValueField = "Emp_Id";
        //ddlUser.DataBind();
        //ddlUser.Items.Insert(0, new ListItem("Select", "0"));
        //ddlUser.SelectedIndex = 0;
    }

    public string GetTlList()
    {
        DataTable dt = objEmpMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());


        //dt = new DataView(dt, "Emp_id=" + Session["EmpId"].ToString() + " or Field5 =" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        dt = new DataView(dt, "Emp_id='" + Session["EmpID"].ToString() + "' or Field5='" + Session["EmpID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            if (Session["To_Do_List_EmpList"] == null)
            {
                Session["To_Do_List_EmpList"] = dt.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                Session["To_Do_List_EmpList"] = Session["To_Do_List_EmpList"].ToString() + "," + dt.Rows[i]["Emp_Id"].ToString();
            }

            FillChild(dt.Rows[i]["Emp_Id"].ToString());

            i++;

        }

        return Session["To_Do_List_EmpList"].ToString();

    }

    private void FillChild(string index)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = objEmpMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Field5='" + index + "'", "", DataViewRowState.CurrentRows).ToTable();



        int i = 0;
        while (i < dt.Rows.Count)
        {
            if (Session["To_Do_List_EmpList"] == null)
            {
                Session["To_Do_List_EmpList"] = dt.Rows[i]["Emp_Id"].ToString();
            }
            else
            {
                Session["To_Do_List_EmpList"] = Session["To_Do_List_EmpList"].ToString() + "," + dt.Rows[i]["Emp_Id"].ToString();
            }

            FillChild(dt.Rows[i]["Emp_Id"].ToString());

            i++;
        }

    }



    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvCallRegister, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        //objPageCtlSettting.setColumnsVisibility(GvCallRegister, lstCls);
    }

    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
}