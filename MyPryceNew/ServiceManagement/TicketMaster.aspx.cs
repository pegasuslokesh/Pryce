using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.IO;
using PegasusDataAccess;

public partial class ServiceManagement_TicketMaster : System.Web.UI.Page
{
    #region defined Class Object
    Common cmn = null;
    SM_Ticket_Master objTicketMaster = null;
    EmployeeMaster objEmployee = null;
    SystemParameter ObjSysParam = null;
    CallLogs objCalllogs = null;
    Set_DocNumber objDocNo = null;
    SM_TicketEmployee objTicketEmployee = null;
    Sm_TicketFeedback ObjTicketFeedback = null;
    Ems_ContactMaster objContact = null;
    Inv_SalesInvoiceHeader objSalesInvoiceheader = null;
    Set_ApplicationParameter objAppParam = null;
    SendMailSms ObjSendMailSms = null;
    Inv_ProductMaster ObjProductMaster = null;
    SM_Ticket_Product objTicketproduct = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Prj_VisitMaster objVisitMaster = null;
    Prj_Project_Task_Employeee objTaskEmp = null;
    Set_AddressChild objAddChild = null;
    DataAccessClass objDa = null;
    CompanyMaster objcomp = null;
    UserMaster ObjUser = null;
    PageControlCommon objPageCmn = null;

    static string customerID = "";

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objTicketMaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objCalllogs = new CallLogs(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objTicketEmployee = new SM_TicketEmployee(Session["DBConnection"].ToString());
        ObjTicketFeedback = new Sm_TicketFeedback(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objSalesInvoiceheader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objTicketproduct = new SM_Ticket_Product(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objVisitMaster = new Prj_VisitMaster(Session["DBConnection"].ToString());
        objTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objcomp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        txtExpectedEndDate.Attributes.Add("readonly", "readonly");
        txtScheduleDate.Attributes.Add("readonly", "readonly");
        txtTicketDate.Attributes.Add("readonly", "readonly");

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/TicketMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            // ddlOption.SelectedIndex = 2;
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLoc);

            btnList_Click(null, null);

            FillGrid();
            Session["DtDetail"] = null;
            FillRequestGrid();

            txtticketNo.Text = GetDocumentNum();
            ViewState["DocNo"] = txtticketNo.Text;
            txtTicketDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            Calender.Format = Session["DateFormat"].ToString();

            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueRequestDate.Format = Session["DateFormat"].ToString();
            //CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtVisitDate.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            BtnReset_Click(null, null);
            //ddlFieldName.Focus();
            LoadStores();
            hdnCallId.Value = "0";
            Session["dtToolsList"] = null;
            Session["DtVisit"] = null;
            LoadToolsRecord();
            txtVisitDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());


        }

        try
        {
            GvTicketMaster.DataSource = Session["dtPInquiry"] as DataTable;
            GvTicketMaster.DataBind();
        }
        catch
        {

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
    #region System defined Function
    protected void btnList_Click(object sender, EventArgs e)
    {
        //txtValue.Focus();
        //txtValue.Text = "";

        ddlStatusFilter.Visible = true;
        lblstatusFilter.Visible = true;
        // lblstatusFiltercolon.Visible = true;
        //ddlFieldName.Focus();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        Lbl_Tab_New.Enabled = false;
        Lbl_Tab_New.Enabled = true;
        ddlStatusFilter.Visible = false;
        lblstatusFilter.Visible = false;
        //lblstatusFiltercolon.Visible = false;
        //FillCurrency();

        if (Lbl_Tab_New.Text == "View")
        {
            //btnPISave.Visible = false;
            BtnReset.Visible = false;
        }
        else
        {

            BtnReset.Visible = true;
        }
        //AllPageCode();
        txtTicketDate.Focus();
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);

    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        ddlLoc.Enabled = false;
        ddlLoc.SelectedValue = e.CommandName.ToString();

        DataTable dtTicket = objTicketMaster.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());

        if (dtTicket.Rows.Count > 0)
        {
            pnlTicketDetail.Enabled = true;

            DataTable dt_visitData = new SM_WorkOrder(Session["DBConnection"].ToString()).GetRecordByRef_typeNRef_id("Ticket", dtTicket.Rows[0]["Ticket_No"].ToString());

            if (dt_visitData.Rows.Count > 0)
            {
                txtChargableAmount.Enabled = false;
            }
            else
            {
                txtChargableAmount.Enabled = true;
            }

            txtECustomer.Text = "";

            txtticketNo.ReadOnly = true;

            LinkButton b = (LinkButton)sender;
            string objSenderID = b.ID;



            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                btnPISave.Enabled = false;
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                btnPISave.Enabled = true;
            }

            if (objSenderID != "imgFeedback")
            {
                pnlFeedbacKdetail.Visible = false;
            }

            btnNew_Click(null, null);
            editid.Value = e.CommandArgument.ToString();
            txtticketNo.Text = dtTicket.Rows[0]["Ticket_No"].ToString();
            txtTicketDate.Text = GetDate(dtTicket.Rows[0]["Ticket_Date"].ToString());

            if (dtTicket.Rows[0]["Field3"].ToString() != "")
            {
                txtExpectedEndDate.Text = GetDate(dtTicket.Rows[0]["Field3"].ToString());
            }

            ddlStatus.Enabled = true;
            if (dtTicket.Rows[0]["Call_Id"].ToString() == "0")
            {
                txtRefNo.Text = dtTicket.Rows[0]["Ref_No"].ToString();
                txtCallDate.Text = "";
                trCallLogs.Visible = false;
                trCallLogsCallDetail.Visible = false;
                trCallLogsNotes.Visible = false;
                txtECustomer.Enabled = true;
                ddlRefType.Enabled = true;
                txtRefNo.Enabled = true;
            }
            else
            {
                ddlRefType.Enabled = false;
                txtRefNo.Enabled = false;
                ListItem liCall = new ListItem();
                liCall.Text = "By Call";
                liCall.Value = "Call";
                ddlRefType.Items.Add(liCall);
                hdnCallId.Value = dtTicket.Rows[0]["Call_Id"].ToString();
                txtRefNo.Text = dtTicket.Rows[0]["Call_No"].ToString();
                txtCallDate.Text = GetDate(dtTicket.Rows[0]["Call_Date"].ToString());
                lblCalldetail.Text = dtTicket.Rows[0]["Call_Detail"].ToString();
                lblCallNotes.Text = dtTicket.Rows[0]["Notes"].ToString();
                trCallLogs.Visible = true;
                trCallLogsCallDetail.Visible = true;
                trCallLogsNotes.Visible = true;

                txtECustomer.Enabled = false;

            }


            ddlRefType.SelectedValue = dtTicket.Rows[0]["Ref_Type"].ToString();

            if (dtTicket.Rows[0]["Status"].ToString() != "")
            {
                ddlStatus.SelectedValue = dtTicket.Rows[0]["Status"].ToString();
            }
            else
            {
                ddlStatus.SelectedIndex = 1;
            }
            if (dtTicket.Rows[0]["Task_Type"].ToString() != "")
            {
                ddlTaskType.SelectedValue = dtTicket.Rows[0]["Task_Type"].ToString();
            }
            else
            {
                ddlTaskType.SelectedIndex = 1;
            }
            //for get customer
            if (dtTicket.Rows[0]["Field6"].ToString() == "True")
            {
                DataTable dtcon = objContact.GetContactTrueById(dtTicket.Rows[0]["Customer_Name"].ToString());

                txtECustomer.Text = dtcon.Rows[0]["Name"].ToString() + "/" + dtcon.Rows[0]["Field2"].ToString() + "/" + dtcon.Rows[0]["Field1"].ToString() + "/" + dtcon.Rows[0]["Trans_Id"].ToString();
                customerID = dtcon.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                txtECustomer.Text = dtTicket.Rows[0]["Customer_Name"].ToString();

            }

            if (dtTicket.Rows[0]["CreatedBy"].ToString() == Session["UserId"].ToString())
            {
                chkInvolveCustomer.Visible = true;
            }
            else
            {
                chkInvolveCustomer.Visible = false;
                chkInvolveEmployee.Visible = false;
                chkInvolveEmployee.Checked = true;
            }
            txtEmailId.Text = dtTicket.Rows[0]["Email_Id"].ToString();

            txtInvoiceNo.Text = dtTicket.Rows[0]["InvoiceNo"].ToString();
            hdnInvoiceId.Value = dtTicket.Rows[0]["Field2"].ToString();
            txtScheduleDate.Text = GetScheduleDate(dtTicket.Rows[0]["Schedule_Date"].ToString());
            //if (txtScheduleDate.Text == "01-Jan-1900")
            //{
            //    txtScheduleDate.Text="";
            //}
            txtChargableAmount.Text = dtTicket.Rows[0]["Chargeable_Amount"].ToString();
            txtDescription.Text = dtTicket.Rows[0]["Description"].ToString();

            //get record from ticket_product table
            DataTable dtTicketProduct = objTicketproduct.GetRecordByTicketId(e.CommandArgument.ToString());
            if (dtTicketProduct.Rows.Count > 0)
            {
                dtTicketProduct = dtTicketProduct.DefaultView.ToTable(true, "Trans_Id", "ProductId", "ProductCode", "ProductName", "Problem");
                Session["dtToolsList"] = dtTicketProduct;

            }
            else
            {
                Session["dtToolsList"] = null;
            }
            LoadToolsRecord();
            DataTable dtTicketEmp = objTicketEmployee.GetAllRecord_ByTicketId(editid.Value);
            if (dtTicketEmp.Rows.Count > 0)
            {
                dtTicketEmp = dtTicketEmp.DefaultView.ToTable(true, "Employee_Id", "EmpName", "Emp_Code");
                Session["dtEmpList"] = dtTicketEmp;
            }
            else
            {

            }
            if (dtTicket.Rows[0]["Field1"].ToString() != "")
            {
                Session["FileName"] = dtTicket.Rows[0]["Field1"].ToString();

            }
            else
            {
                Session["FileName"] = "";
            }

            LoadStores();
            LoadFeedbackRecord(editid.Value);



            //for get visit record by ref type and ref id
            if (objVisitMaster.GetRecord_By_RefType_and_RefId("Ticket", e.CommandArgument.ToString()).Rows.Count > 0)
            {
                fillVisitGrid(objVisitMaster.GetRecord_By_RefType_and_RefId("Ticket", e.CommandArgument.ToString()));
            }
            try
            {
                gvVisitMaster.Columns[0].Visible = false;
            }
            catch
            {
            }

            //
            //code end
        }
    }
    protected void GvTicketMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTicketMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPInquiry"];

        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvTicketMaster, dt, "", "");
        //AllPageCode();
    }

    protected void GvTicketMaster_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPInquiry"];
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
        Session["dtPInquiry"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvTicketMaster, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //here we check that ticket is in use or not before delete it

        DataTable dt = ObjTicketFeedback.GetAllRecord();

        try
        {
            //TicketId

            dt = new DataView(dt, "Ticket_No=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dt.Rows.Count > 0)
        {
            DisplayMessage("Ticket is in use ,you can not delete");
            return;
        }

        editid.Value = e.CommandArgument.ToString();


        int b = 0;
        b = objTicketMaster.DeleteTicketMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        // FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();


    }
    protected void btnPICancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        FillGridBin();
        FillGrid();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        editid.Value = "";
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTicketDate);

    }
    protected void btnPISave_Click(object sender, EventArgs e)
    {


        if (objAppParam.GetApplicationParameterByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count == 0)
        {
            DisplayMessage("Configure your support email first");
            btnPISave.Enabled = true;
            return;
        }



        string Master_Email_SMTP = objAppParam.GetApplicationParameterValueByParamName("Support_Email_SMTP", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string Master_Email_Port = objAppParam.GetApplicationParameterValueByParamName("Support_Email_Port", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string Master_Email = objAppParam.GetApplicationParameterValueByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string Master_Email_Password = Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Support_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString());

        string Email_Display_Name = objAppParam.GetApplicationParameterValueByParamName("Support_Display_Text", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string RefNo = string.Empty;

        if (ddlRefType.SelectedValue == "Call")
        {
            RefNo = hdnCallId.Value;
        }
        else
        {
            RefNo = txtRefNo.Text;
        }

        string filepath = string.Empty;
        string FileName = string.Empty;
        if (txtticketNo.Text == "")
        {
            DisplayMessage("Enter Ticket Number");
            txtticketNo.Focus();
            btnPISave.Enabled = true;
            return;

        }
        if (txtTicketDate.Text == "")
        {
            DisplayMessage("Enter Ticket Date");
            txtTicketDate.Focus();
            btnPISave.Enabled = true;
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtTicketDate.Text.Trim());
            }
            catch
            {
                txtTicketDate.Text = "";
                txtTicketDate.Focus();
                DisplayMessage("Enter Valid Ticket Date");
                btnPISave.Enabled = true;
                return;
            }
        }

        if (txtExpectedEndDate.Text == "")
        {
            DisplayMessage("Enter End Date");
            txtExpectedEndDate.Focus();
            btnPISave.Enabled = true;
            return;
        }

        string StrCustomer = string.Empty;
        bool strrValues = false;

        string StrContact = string.Empty;
        string[] strCustInfo = new string[4];

        if (txtECustomer.Text == "")
        {
            txtECustomer.Focus();
            DisplayMessage("Enter Customer Name");
            btnPISave.Enabled = true;
            return;
        }
        else
        {
            try
            {
                StrCustomer = txtECustomer.Text.Split('/')[3].ToString();
                strrValues = true;
                //get customer information

                //code start
                strCustInfo = getCustomerInformation(StrCustomer);
                //code end


            }
            catch
            {
                StrCustomer = txtECustomer.Text;
                strrValues = false;
                strCustInfo = getCustomerInformation("0");
            }

        }

        if (txtEmailId.Text == "")
        {
            DisplayMessage("Enter Email-Id");
            txtEmailId.Focus();
            btnPISave.Enabled = true;
            return;
        }
        else
        {
            if (!IsValidEmailId(txtEmailId.Text))
            {
                DisplayMessage("Enter Valid Email-Id");
                txtEmailId.Focus();
                btnPISave.Enabled = true;
                return;
            }
        }
        if (txtScheduleDate.Text == "")
        {
            txtScheduleDate.Text = "1/1/1900";
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtScheduleDate.Text.Trim());
            }
            catch
            {
                txtScheduleDate.Text = "";
                DisplayMessage("Enter Valid schedule Date");
                txtScheduleDate.Focus();
                btnPISave.Enabled = true;
                return;
            }
        }
        if (txtDescription.Text == "")
        {
            DisplayMessage("Enter Ticket Description");
            txtDescription.Focus();
            btnPISave.Enabled = true;
            return;
        }

        if (UploadFile.HasFile)
        {
            FileName = UploadFile.FileName;
        }


        if (txtInvoiceNo.Text == "")
        {
            hdnInvoiceId.Value = "0";
        }

        if (editid.Value == "")
        {

            if (txtticketNo.Text != "")
            {

                if ((new DataView(objTicketMaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue), "Ticket_No='" + txtticketNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0))
                {
                    DisplayMessage("Ticket No Already Exist");
                    txtticketNo.Text = "";
                    btnPISave.Enabled = true;
                    return;
                }
            }

            int b = 0;

            b = objTicketMaster.InsertTicketMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtticketNo.Text, txtTicketDate.Text, ddlRefType.SelectedValue, RefNo, ddlTaskType.SelectedValue, hdnCallId.Value, txtScheduleDate.Text, txtChargableAmount.Text, ddlStatus.SelectedValue, txtDescription.Text, StrCustomer, txtEmailId.Text, FileName, hdnInvoiceId.Value, txtExpectedEndDate.Text, "", "", strrValues.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());




            string strMaxId = string.Empty;

            if (b != 0)
            {
                strMaxId = b.ToString();

                if (hdnCallId.Value != "0")
                {
                    objCalllogs.UpdateCallLogsStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnCallId.Value, ddlStatus.SelectedItem.Text, Session["UserId"].ToString(), DateTime.Now.ToString());

                    string strsql = "update SM_Call_Logs set Field1='" + ddlStatus.SelectedValue + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + hdnCallId.Value + "";
                    objDa.execute_Command(strsql);
                }




                //insert record in visit master table
                //code start
                string CustomerId = string.Empty;
                string CustomerName = string.Empty;
                if (!strrValues)
                {
                    CustomerId = "0";
                    CustomerName = txtECustomer.Text;
                }
                else
                {
                    CustomerId = txtECustomer.Text.Split('/')[3].ToString();
                    CustomerName = txtECustomer.Text.Split('/')[0].ToString();
                }

                if (Session["DtVisit"] != null)
                {
                    DataTable dtvisit = (DataTable)Session["DtVisit"];
                    foreach (DataRow dr in dtvisit.Rows)
                    {
                        int visitId = 0;

                        visitId = objVisitMaster.InsertRecord("Ticket", strMaxId, "0", StrCustomer, dr["Visit_Date"].ToString(), dr["Vehicle_Id"].ToString(), dr["Driver_Id"].ToString(), "0", "Open", "", "", "", "", dr["Visit_Time"].ToString(), false.ToString(), strCustInfo[0].ToString(), strCustInfo[1].ToString(), "", strCustInfo[2].ToString(), strCustInfo[3].ToString(), "", "", "", "", "", strrValues.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());



                        InsertTaskEmployee(visitId.ToString());


                        insertRecordIngeotrackingdatabse(dr["Driver_Id"].ToString(), visitId.ToString(), CustomerId, CustomerName, Convert.ToDateTime(dr["Visit_Date"].ToString()), dr["Visit_Time"].ToString());
                    }
                }

                //code end


                //insert record in ticket product table 

                //code start
                objTicketproduct.DeleteRecord_By_TicketId(strMaxId);

                string strProductName = string.Empty;

                if (Session["dtToolsList"] != null)
                {

                    string Toolsid = string.Empty;

                    DataTable dtProduct = (DataTable)Session["dtToolsList"];

                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        if (strProductName == "")
                        {
                            strProductName = dr["ProductName"].ToString();
                        }
                        else
                        {
                            strProductName = strProductName + "," + dr["ProductName"].ToString();
                        }

                        objTicketproduct.InsertRecord(strMaxId, dr["ProductId"].ToString(), dr["ProductCode"].ToString(), dr["ProductName"].ToString(), dr["Problem"].ToString());
                    }
                }
                //code end
                //for arcawing
                //start

                if (UploadFile.HasFile)
                {

                    string DirectroryName = Server.MapPath("Ticket/" + strMaxId);

                    CreateDirectoryIfNotExist(DirectroryName);

                    filepath = "~/" + "ServiceManagement/Ticket/" + strMaxId + "/" + UploadFile.FileName;

                    if (!Directory.Exists(filepath))
                    {
                        UploadFile.SaveAs(Server.MapPath(filepath));
                    }
                }
                //end

                string dtCount = objTicketMaster.getCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue);
                objTicketMaster.Updatecode(b.ToString(), txtticketNo.Text + dtCount);
                txtticketNo.Text = txtticketNo.Text + dtCount;


                string strCC = string.Empty;
                string strAdminEmailId = string.Empty;


                if (Session["EmpId"].ToString() != "0")
                {
                    strAdminEmailId = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Email_Id"].ToString();
                }

                if (Session["dtEmpList"] != null)
                {
                    DataTable DtEmployeeList = (DataTable)Session["dtEmpList"];


                    foreach (DataRow dr in DtEmployeeList.Rows)
                    {
                        if (objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString() != "")
                        {
                            if (strCC == "")
                            {
                                strCC = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                            }
                            else
                            {
                                strCC = strCC + ";" + objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                            }

                        }

                        objTicketEmployee.InsertRecord(b.ToString(), dr["Employee_Id"].ToString(), "", "", "", "", "", strrValues.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }



                //for send mail
                //code start
                //string confirmValue = Request.Form["confirm_value"];
                //if (confirmValue == "Yes")
                //{
                try
                {

                    if (txtEmailId.Text != "")
                    {

                        string strCustomerName = string.Empty;
                        string strContactNo = string.Empty;


                        try
                        {
                            if (txtECustomer.Text.Contains("/"))
                            {
                                strCustomerName = txtECustomer.Text.Split('/')[0].ToString();
                                strContactNo = txtECustomer.Text.Split('/')[1].ToString();
                            }
                            else
                            {
                                strCustomerName = txtECustomer.Text;
                            }
                        }
                        catch
                        {
                        }



                        string MailBody_Employee = string.Empty;
                        string MailBody_Customer = string.Empty;

                        MailBody_Employee = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Auto Reply - Customer Support System</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>Thanks for your interest in our organization. We’ve received your Ticket</p>                                    <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'>Our staff will reply to your Ticket as soon as possible. We do make every effort to provide an informative, detailed response to every Ticket we receive and strive to do so within two or three business days, though sometimes it may take longer.            </p>                        <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + txtticketNo.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000; '>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(txtTicketDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + strCustomerName + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + txtEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + strContactNo + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td> </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + txtDescription.Text + "</td>    </tr></table><br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	 <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> Your Friendly Customer Support System</p></body></html>";
                        MailBody_Customer = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px; '>Auto Reply - Customer Support System</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>Thanks for your interest in our organization. We’ve received your Ticket</p>                                    <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'>Our staff will reply to your Ticket as soon as possible. We do make every effort to provide an informative, detailed response to every Ticket we receive and strive to do so within two or three business days, though sometimes it may take longer.            </p>                        <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + txtticketNo.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000; '>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(txtTicketDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + strCustomerName + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + txtEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + strContactNo + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td>    </tr>             <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + txtDescription.Text + "</td>    </tr></table><br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	To add additional comments, you can simply click on this <a href=" + GetCustomerFeedbackURL() + "?TicketId=@T&&CompId=@C&&BrandId=@B&&LocId=@L>Link</a></p> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> Your Friendly Customer Support System</p></body></html>";

                        MailBody_Customer = MailBody_Customer.Replace("@T", HttpUtility.UrlEncode(Common.Encrypt(strMaxId)));
                        MailBody_Customer = MailBody_Customer.Replace("@C", HttpUtility.UrlEncode(Common.Encrypt(Session["CompId"].ToString())));
                        MailBody_Customer = MailBody_Customer.Replace("@B", HttpUtility.UrlEncode(Common.Encrypt(Session["BrandId"].ToString())));
                        MailBody_Customer = MailBody_Customer.Replace("@L", HttpUtility.UrlEncode(Common.Encrypt(ddlLoc.SelectedValue)));


                        string strsubject = string.Empty;
                        strsubject = "Ticket Generated ," + txtticketNo.Text + " , " + strCustomerName + " ," + txtDescription.Text + "";

                        //for send customer

                        //               ThreadStart ts = delegate()
                        //{
                        ObjSendMailSms.SendMail_TicketInfo(txtEmailId.Text, "", "", strsubject, MailBody_Customer, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());



                        //};

                        //               Thread t = new Thread(ts);

                        //               // Run the thread.
                        //               t.Start();
                        //                ThreadStart ts1 = delegate()
                        //{


                        //for send admin and related Employee
                        ObjSendMailSms.SendMail_TicketInfo(strAdminEmailId, strCC, "", strsubject, MailBody_Employee, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


                        //};
                        //                Thread t1 = new Thread(ts1);

                        //                // Run the thread.
                        //                t1.Start();

                    }
                }
                catch
                {
                }
                //}
                //code end




                DisplayMessage("Record Saved", "green");
                btnPISave.Enabled = true;
                Reset();
                FillGrid();
            }
        }
        else
        {
            if (UploadFile.HasFile)
            {
                FileName = UploadFile.FileName;
            }
            else
            {
                FileName = Session["FileName"].ToString();
            }

            objTicketMaster.UpdateTicketMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, editid.Value, txtticketNo.Text, txtTicketDate.Text, ddlRefType.SelectedValue, RefNo, ddlTaskType.SelectedValue, hdnCallId.Value, txtScheduleDate.Text, txtChargableAmount.Text, ddlStatus.SelectedValue, txtDescription.Text, StrCustomer, txtEmailId.Text, FileName, hdnInvoiceId.Value, txtExpectedEndDate.Text, "", "", strrValues.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());


            if (hdnCallId.Value != "0")
            {
                objCalllogs.UpdateCallLogsStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnCallId.Value, ddlStatus.SelectedItem.Text, Session["UserId"].ToString(), DateTime.Now.ToString());
                string strsql = "update SM_Call_Logs set Field1='" + ddlStatus.SelectedValue + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + hdnCallId.Value + "";
                objDa.execute_Command(strsql);
            }

            //insert record in ticket product table 

            //first delete then reinsert
            //insert record in ticket product table 

            //code start
            objTicketproduct.DeleteRecord_By_TicketId(editid.Value);

            string strProductName = string.Empty;
            if (Session["dtToolsList"] != null)
            {

                string Toolsid = string.Empty;

                DataTable dtProduct = (DataTable)Session["dtToolsList"];

                foreach (DataRow dr in dtProduct.Rows)
                {
                    if (strProductName == "")
                    {
                        strProductName = dr["ProductName"].ToString();
                    }
                    else
                    {
                        strProductName = strProductName + "," + dr["ProductName"].ToString();
                    }
                    objTicketproduct.InsertRecord(editid.Value, dr["ProductId"].ToString(), dr["ProductCode"].ToString(), dr["ProductName"].ToString(), dr["Problem"].ToString());
                }
            }
            //code end

            //for arcawing
            //start

            if (UploadFile.HasFile)
            {
                try
                {
                    if (Session["FileName"].ToString() != "")
                    {
                        if (System.IO.File.Exists(Server.MapPath("Ticket/" + editid.Value + "/" + Session["FileName"].ToString())))
                        {
                            System.IO.File.Delete(Server.MapPath("Ticket/" + editid.Value + "/" + Session["FileName"].ToString()));
                        }
                    }
                }
                catch
                {
                }

                string DirectroryName = Server.MapPath("Ticket/" + editid.Value);

                CreateDirectoryIfNotExist(DirectroryName);

                filepath = "~/" + "ServiceManagement/Ticket/" + editid.Value + "/" + UploadFile.FileName;

                if (!Directory.Exists(filepath))
                {
                    UploadFile.SaveAs(Server.MapPath(filepath));
                }
            }

            //end 
            objTicketEmployee.DeleteRecord_ByTicketId(editid.Value);

            string strCC = string.Empty;
            string strAdminEmailId = string.Empty;
            if (Session["EmpId"].ToString() != "0")
            {
                strAdminEmailId = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()).Rows[0]["Email_Id"].ToString();
            }


            if (Session["dtEmpList"] != null)
            {
                DataTable DtEmployeeList = (DataTable)Session["dtEmpList"];

                foreach (DataRow dr in DtEmployeeList.Rows)
                {
                    if (objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString() != "")
                    {
                        if (strCC == "")
                        {
                            strCC = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                        }
                        else
                        {
                            strCC = strCC + ";" + objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                        }

                    }
                    objTicketEmployee.InsertRecord(editid.Value, dr["Employee_Id"].ToString(), "", "", "", "", "", strrValues.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }


            //for send mail in close status 


            if (ddlStatus.SelectedValue.Trim() == "Close")
            {
                try
                {

                    if (txtEmailId.Text != "")
                    {

                        string strCustomerName = string.Empty;
                        string strContactNo = string.Empty;


                        try
                        {
                            if (txtECustomer.Text.Contains("/"))
                            {
                                strCustomerName = txtECustomer.Text.Split('/')[0].ToString();
                                strContactNo = txtECustomer.Text.Split('/')[1].ToString();
                            }
                            else
                            {
                                strCustomerName = txtECustomer.Text;
                            }
                        }
                        catch
                        {
                        }

                        string MailBody_Employee = string.Empty;
                        string MailBody_Customer = string.Empty;

                        MailBody_Employee = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Auto Reply - Customer Support System</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'> 	Your Ticket Successfully Closed from Customer Support System </p>                                                          <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + txtticketNo.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(txtTicketDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + strCustomerName + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + txtEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + strContactNo + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td> </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + txtDescription.Text + "</td>    </tr></table><br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	 <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> Your Friendly Customer Support System</p></body></html>";
                        MailBody_Customer = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Auto Reply - Customer Support System</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'> 	Your Ticket Successfully Closed from Customer Support System </p>                                                         <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + txtticketNo.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(txtTicketDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + strCustomerName + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + txtEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + strContactNo + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td>    </tr>             <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + txtDescription.Text + "</td>    </tr></table><br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	To add additional comments, you can simply click on this <a href=" + GetCustomerFeedbackURL() + "?TicketId=@T&&CompId=@C&&BrandId=@B&&LocId=@L>Link</a></p> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> Your Friendly Customer Support System</p></body></html>";

                        MailBody_Customer = MailBody_Customer.Replace("@T", HttpUtility.UrlEncode(Common.Encrypt(editid.Value)));
                        MailBody_Customer = MailBody_Customer.Replace("@C", HttpUtility.UrlEncode(Common.Encrypt(Session["CompId"].ToString())));
                        MailBody_Customer = MailBody_Customer.Replace("@B", HttpUtility.UrlEncode(Common.Encrypt(Session["BrandId"].ToString())));
                        MailBody_Customer = MailBody_Customer.Replace("@L", HttpUtility.UrlEncode(Common.Encrypt(ddlLoc.SelectedValue)));


                        string strsubject = string.Empty;
                        strsubject = "Ticket Closed ," + txtticketNo.Text + " , " + strCustomerName + " ," + txtDescription.Text + "";
                        //           //for send customer
                        //           ThreadStart ts = delegate()
                        //{
                        ObjSendMailSms.SendMail_TicketInfo(txtEmailId.Text, "", "", strsubject, MailBody_Customer, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                        //};

                        //          Thread t = new Thread(ts);

                        //          // Run the thread.
                        //          t.Start();


                        //ThreadStart ts1 = delegate()
                        //{

                        ObjSendMailSms.SendMail_TicketInfo(strAdminEmailId, strCC, "", strsubject, MailBody_Employee, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        //};

                        //Thread t1 = new Thread(ts1);

                        //// Run the thread.
                        //t1.Start();
                    }
                }
                catch
                {
                }

            }

            btnList_Click(null, null);
            DisplayMessage("Record Updated", "green");
            btnPISave.Enabled = true;
            Reset();
            FillGrid();
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
    }
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objTicketMaster.DeleteTicketMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {

        ddlStatusFilter.Visible = false;
        lblstatusFilter.Visible = false;
        // lblstatusFiltercolon.Visible = false;
        FillGridBin();
        txtValueBin.Text = "";
        ddlFieldNameBin.Focus();
    }
    protected void btnPRequest_Click(object sender, EventArgs e)
    {
        ddlStatusFilter.Visible = false;
        lblstatusFilter.Visible = false;
        // lblstatusFiltercolon.Visible = false;
        FillGridBin();
        FillRequestGrid();
        ddlFieldNameRequest.Focus();
    }
    protected void GvTicketMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvTicketMasterBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtPInquiryBin"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvTicketMasterBin, dt, "", "");


        string temp = string.Empty;

        for (int i = 0; i < GvTicketMasterBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvTicketMasterBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvTicketMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvTicketMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objTicketMaster.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtPInquiryBin"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvTicketMasterBin, dt, "", "");

        lblSelectedRecord.Text = "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objTicketMaster.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvTicketMasterBin, dt, "", "");
        Session["dtPInquiryBin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            //ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Ticket_Date")
        {
            if (txtValueBinDate.Text != "")
            {

                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                txtValueBin.Text = "";
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


            DataTable dtCust = (DataTable)Session["dtPInquiryBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtPInquiryBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvTicketMasterBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            //if (view.ToTable().Rows.Count == 0)
            //{
            //    FillGridBin();
            //}
            //AllPageCode();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            // btnRefreshBin.Focus();

        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvTicketMasterBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvTicketMasterBin.Rows.Count; i++)
        {
            ((CheckBox)GvTicketMasterBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvTicketMasterBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvTicketMasterBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvTicketMasterBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
        HiddenField lb = (HiddenField)GvTicketMasterBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvTicketMasterBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Value.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Value.ToString().Trim();
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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;

        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        txtValueBinDate.Text = "";


        lblSelectedRecord.Text = "";
        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtPInquiryBin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < GvTicketMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvTicketMasterBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvTicketMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtPInquiryBin"];
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvTicketMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objTicketMaster.DeleteTicketMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvTicketMasterBin.Rows)
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
    #endregion
    #endregion
    #region User defined Function
    private void FillGrid()
    {
        DataTable dtPInquiry = objTicketMaster.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue.ToString());
        if (ddlStatusFilter.SelectedIndex != 0)
        {
            try
            {
                dtPInquiry = new DataView(dtPInquiry, "Status='" + ddlStatusFilter.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        // lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count + "";

        Session["dtPInquiry"] = dtPInquiry;
        if (dtPInquiry.Rows.Count > 0)
        {
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            //cmn.FillData((object)GvTicketMaster, dtPInquiry, "", "");
            GvTicketMaster.DataSource = dtPInquiry;
            GvTicketMaster.DataBind();
        }
        else
        {
            GvTicketMaster.DataSource = null;
            GvTicketMaster.DataBind();
        }
        // lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataTable dtTicketEmp = objTicketEmployee.GetAllRecord_ByTicketIdandEmployeeId(((HiddenField)e.Row.FindControl("hdnTransId")).Value, HttpContext.Current.Session["EmpId"].ToString());
            if (dtTicketEmp.Rows.Count > 0)
            {
                ((ImageButton)e.Row.FindControl("imgFeedback")).Visible = true;
            }
            else
            {
                ((ImageButton)e.Row.FindControl("imgFeedback")).Visible = false;

                DataTable dtTicket = objTicketMaster.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((HiddenField)e.Row.FindControl("hdnTransId")).Value);

                if (dtTicket.Rows.Count > 0)
                {
                    if (dtTicket.Rows[0]["CreatedBy"].ToString() == Session["UserId"].ToString())
                    {
                        ((ImageButton)e.Row.FindControl("imgFeedback")).Visible = true;

                    }
                }
            }
            //e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#");
        }
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = Convert.ToDateTime(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected string GetScheduleDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "1/1/1900 12:00:00 AM")
        {
            strNewDate = Convert.ToDateTime(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
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
        ddlLoc.Enabled = true;
        ddlLoc.SelectedValue = Session["LocId"].ToString();
        txtTicketDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        hdnCallId.Value = "0";
        txtticketNo.Text = GetDocumentNum();
        ViewState["DocNo"] = txtticketNo.Text;
        txtCallDate.Text = "";
        txtRefNo.Text = "";
        txtECustomer.Enabled = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlTaskType.SelectedIndex = 1;
        ddlStatus.SelectedIndex = 0;
        txtScheduleDate.Text = "";
        txtChargableAmount.Text = "0";
        txtDescription.Text = "";
        editid.Value = "";
        Session["dtEmpList"] = null;
        LoadStores();
        trCallLogs.Visible = false;
        trCallLogsCallDetail.Visible = false;
        trCallLogsNotes.Visible = false;
        txtECustomer.Text = "";
        txtTicketDate.Focus();
        Session["FileName"] = "";
        pnlTicketDetail.Enabled = true;
        pnlFeedbacKdetail.Visible = false;
        BtnReset.Visible = true;
        txtInvoiceNo.Text = "";
        ddlRefType.Items.Clear();
        ListItem liWebsite = new ListItem();
        liWebsite.Text = "By Website";
        liWebsite.Value = "Website";
        ListItem liEmail = new ListItem();
        liEmail.Text = "By Email";
        liEmail.Value = "Email";
        ListItem liContract = new ListItem();
        liContract.Text = "By Contract";
        liContract.Value = "Contract";
        ddlRefType.Items.Add(liWebsite);
        ddlRefType.Items.Add(liEmail);
        ddlRefType.Items.Add(liContract);
        ddlRefType.Enabled = true;
        txtRefNo.Enabled = true;
        Session["dtToolsList"] = null;
        LoadToolsRecord();
        Session["DtVisit"] = null;
        ResetVisitPanel();
        gvVisitMaster.DataSource = null;
        gvVisitMaster.DataBind();
        ddlStatus.Enabled = false;
        ddlStatus.SelectedIndex = 0;
        customerID = "0";
        txtExpectedEndDate.Text = "";
        try
        {
            gvVisitMaster.Columns[0].Visible = true;
        }
        catch
        {
        }
        txtEmailId.Text = "";
        btnPICancel.Visible = true;
    }
    protected string GetDocumentNum()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "271", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnPISave.Visible = clsPagePermission.bEdit;
        try
        {
            GvTicketMaster.Columns[0].Visible = clsPagePermission.bView;
            GvTicketMaster.Columns[1].Visible = clsPagePermission.bEdit;
            GvTicketMaster.Columns[2].Visible = clsPagePermission.bDelete;
            GvTicketMaster.Columns[3].Visible = clsPagePermission.bDownload;
            GvTicketMaster.Columns[5].Visible = clsPagePermission.bView;

        }
        catch
        {
        }
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
    }
    #endregion
    #region View

    protected void lnkFeedbackDetail_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjTicketFeedback.GetAllRecord();

        try
        {
            //TicketId

            dt = new DataView(dt, "Ticket_No=" + e.CommandArgument.ToString() + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (dt == null)
        {
            DisplayMessage("Feedback Not Found");
            return;
        }
        else
        {
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Feedback Not Found");
                return;
            }
        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../ServiceManagement/ViewFeedBack.aspx?TicketId=" + HttpUtility.UrlEncode(Common.Encrypt(e.CommandArgument.ToString())) + "','window','width=1024');", true);


    }

    #endregion
    #region Purchase Request Search

    protected void btnbindRequest_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldNameRequest.SelectedItem.Value == "RequestDate") || (ddlFieldNameRequest.SelectedItem.Value == "ExpDelDate") || (ddlFieldNameRequest.SelectedItem.Value == "OrderCompletionDate"))
        {
            if (txtValueRequestDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueRequestDate.Text);
                    txtValueRequest.Text = Convert.ToDateTime(txtValueRequestDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueRequestDate.Text = "";
                    txtValueRequest.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueRequestDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueRequestDate.Focus();
                txtValueRequest.Text = "";
                return;
            }
        }


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
                condition = "convert(" + ddlFieldNameRequest.SelectedValue + ",System.String) Like '" + txtValueRequest.Text.Trim() + "%'";
            }

            DataTable dtPurchaseRequest = (DataTable)Session["dtPRequest"];


            DataView view = new DataView(dtPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtPRequest"] = view.ToTable();
            lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCallRequest, view.ToTable(), "", "");

            //AllPageCode();

            //btnRefreshRequest.Focus();

        }
        if (txtValueRequest.Text != "")
            txtValueRequest.Focus();
        else if (txtValueRequestDate.Text != "")
            txtValueRequestDate.Focus();
    }
    protected void btnRefreshRequest_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        btnPRequest_Click(sender, e);

        ddlFieldNameRequest.SelectedIndex = 0;
        ddlOptionRequest.SelectedIndex = 2;
        txtValueRequest.Text = "";

        txtValueRequest.Visible = true;
        txtValueRequestDate.Visible = false;
        txtValueRequestDate.Text = "";

        txtValueRequest.Focus();
    }

    #endregion
    #region Date Searching
    //protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlFieldName.SelectedItem.Value == "Ticket_Date")
    //    {
    //        txtValueDate.Visible = true;
    //        txtValue.Visible = false;
    //        txtValue.Text = "";
    //        txtValueDate.Text = "";

    //    }
    //    else
    //    {
    //        txtValueDate.Visible = false;
    //        txtValue.Visible = true;
    //        txtValue.Text = "";
    //        txtValueDate.Text = "";

    //    }
    //    ddlFieldName.Focus();
    //}

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedItem.Value == "Ticket_Date")
        {
            txtValueBinDate.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        else
        {
            txtValueBinDate.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        ddlFieldNameBin.Focus();
    }
    protected void ddlFieldNameRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlFieldNameRequest.SelectedItem.Value == "RequestDate") || (ddlFieldNameRequest.SelectedItem.Value == "ExpDelDate") || (ddlFieldNameRequest.SelectedItem.Value == "OrderCompletionDate"))
        {
            txtValueRequestDate.Visible = true;
            txtValueRequest.Visible = false;
            txtValueRequest.Text = "";
            txtValueRequestDate.Text = "";

        }
        else
        {
            txtValueRequestDate.Visible = false;
            txtValueRequest.Visible = true;
            txtValueRequest.Text = "";
            txtValueRequestDate.Text = "";

        }
    }

    #endregion
    #region Add Request Section
    protected void GvCallRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCallRequest.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPRequest"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCallRequest, dt, "", "");

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
        //AllPageCode();
    }
    private void FillRequestGrid()
    {
        DataTable DtPrequest = objCalllogs.GetPendingCallLogsRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (DtPrequest != null && DtPrequest.Rows.Count > 0)
        {
            DtPrequest = new DataView(DtPrequest, "Status='Open'", "", DataViewRowState.CurrentRows).ToTable();

            if (DtPrequest.Rows.Count > 0)
            {
                DtPrequest = new DataView(DtPrequest, "Call_Type<>'Sales Inquiry'", "", DataViewRowState.CurrentRows).ToTable();
            }
            Session["dtPRequest"] = DtPrequest;
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCallRequest, DtPrequest, "", "");

        }
        else
        {
            GvCallRequest.DataSource = null;
            GvCallRequest.DataBind();
        }

        lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + DtPrequest.Rows.Count.ToString() + "";
        //AllPageCode();

    }
    protected void btnPREdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objCalllogs.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            Reset();
            ListItem liCall = new ListItem();
            liCall.Text = "By Call";
            liCall.Value = "Call";
            ddlRefType.Items.Add(liCall);
            ddlRefType.SelectedValue = "Call";

            txtRefNo.Text = dt.Rows[0]["Call_No"].ToString();
            txtEmailId.Text = dt.Rows[0]["Email_Id"].ToString();
            ddlRefType.Enabled = false;
            txtRefNo.Enabled = false;
            txtCallNo_TextChanged(null, null);
            trCallLogs.Visible = true;
            trCallLogsCallDetail.Visible = true;
            trCallLogsNotes.Visible = true;
            btnNew_Click(null, null);
            txtScheduleDate.Focus();
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Call_Log_Active()", true);
        }
    }
    protected void IbtnUpdateCallLogs_Command(object sender, CommandEventArgs e)
    {
        objCalllogs.UpdateCallLogsStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "Cancel", Session["UserId"].ToString(), DateTime.Now.ToString());
        FillRequestGrid();
    }
    #endregion
    protected void txtInvoiceNo_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dtInvoice = new DataTable();
        if (txtInvoiceNo.Text != "")
        {

            try
            {
                dtInvoice = objSalesInvoiceheader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInvoiceNo.Text);
            }
            catch
            {
            }


            if (dtInvoice.Rows.Count > 0)
            {
                hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                DisplayMessage("Invoice Not Found");
                txtInvoiceNo.Text = "";
                txtInvoiceNo.Focus();
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
    public static string[] GetCompletionListCallLog(string prefixText, int count, string contextKey)
    {
        CallLogs objCalllogs = new CallLogs(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCallLogs = objCalllogs.GetPendingCallLogsRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        string filtertext = "Call_No like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtCallLogs, filtertext, "Trans_Id Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Call_No"].ToString();
            }
        }
        return filterlist;
    }
    protected void txtCallNo_TextChanged(object sender, EventArgs e)
    {
        if (txtRefNo.Text != "")
        {
            DataTable dtCAllLogs = objCalllogs.GetPendingCallLogsRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            dtCAllLogs = new DataView(dtCAllLogs, "Call_No='" + txtRefNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtCAllLogs.Rows.Count == 0)
            {
                DisplayMessage("Select Call Number Is Suggestion Only");
                txtCallDate.Text = "";
                txtCallDate.Focus();
                return;
            }
            else
            {
                hdnCallId.Value = dtCAllLogs.Rows[0]["Trans_Id"].ToString();
                txtCallDate.Text = Convert.ToDateTime(dtCAllLogs.Rows[0]["Call_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                ddlTaskType.SelectedValue = dtCAllLogs.Rows[0]["Call_Type"].ToString();
                //ddlStatus.SelectedValue = dtCAllLogs.Rows[0]["Status"].ToString();

                //for get customer
                if (dtCAllLogs.Rows[0]["Field6"].ToString() == "True")
                {
                    DataTable dtcon = objContact.GetContactTrueById(dtCAllLogs.Rows[0]["Customer_Name"].ToString());

                    txtECustomer.Text = dtcon.Rows[0]["Name"].ToString() + "/" + dtcon.Rows[0]["Field2"].ToString() + "/" + dtcon.Rows[0]["Field1"].ToString() + "/" + dtcon.Rows[0]["Trans_Id"].ToString();
                }
                else
                {
                    txtECustomer.Text = dtCAllLogs.Rows[0]["Customer_Name"].ToString();

                }
                lblCalldetail.Text = dtCAllLogs.Rows[0]["Call_Detail"].ToString();
                lblCallNotes.Text = dtCAllLogs.Rows[0]["Notes"].ToString();
                txtECustomer.Enabled = false;
            }
        }
    }
    protected void txtECustomer_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtECustomer.Text != "")
        {
            try
            {
                strCustomerId = txtECustomer.Text.Split('/')[3].ToString();
                customerID = strCustomerId;
            }
            catch
            {
                strCustomerId = "0";
                customerID = strCustomerId;
            }
            if (strCustomerId != "0")
            {
                try
                {
                    if (txtECustomer.Text.Split('/')[2].ToString() != "")
                    {
                        txtEmailId.Text = txtECustomer.Text.Split('/')[2].ToString();
                    }
                    else
                    {
                        txtEmailId.Text = "";
                    }
                }
                catch
                {
                    txtEmailId.Text = "";
                }


            }
            else
            {
                txtEmailId.Text = "";
            }
            ddlTaskType.Focus();
        }
        else
        {
            txtECustomer.Focus();
            strCustomerId = "0";
            customerID = strCustomerId;
            txtInvoiceNo.Text = "";
        }
        // AllPageCode();
    }
    #region addEmployee
    protected void txtEmpFooter_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text != "")
        {


            DataTable dt = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());
            dt = new DataView(dt, "Emp_Name='" + ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Choose Employee In Suggestion Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")));
                ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text = "";
                ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Focus();

                return;
            }
        }
        ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Focus();



    }
    protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    public void LoadStores()
    {
        DataTable dt = new DataTable();
        if (Session["dtEmpList"] != null)
        {

            dt = new DataTable();
            dt = (DataTable)Session["dtEmpList"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gridView, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Employee_Id", typeof(int));
                contacts.Columns.Add("EmpName", typeof(string));
                contacts.Columns.Add("Emp_Code", typeof(string));

                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gridView, contacts, "", "");
                int TotalColumns = gridView.Rows[0].Cells.Count;
                gridView.Rows[0].Cells.Clear();
                gridView.Rows[0].Cells.Add(new TableCell());
                gridView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gridView.Rows[0].Visible = false;
            }

        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Employee_Id", typeof(int));
            contacts.Columns.Add("EmpName", typeof(string));
            contacts.Columns.Add("Emp_Code", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gridView, contacts, "", "");
            int TotalColumns = gridView.Rows[0].Cells.Count;
            gridView.Rows[0].Cells.Clear();
            gridView.Rows[0].Cells.Add(new TableCell());
            gridView.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gridView.Rows[0].Visible = false;
        }
    }

    protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text == "")
            {
                DisplayMessage("Enter Employee name");
                return;
            }
            DataTable dtEmployee = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());
            dtEmployee = new DataView(dtEmployee, "Emp_Name='" + ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtEmployee.Rows.Count > 0)
            {
                EmpId = dtEmployee.Rows[0]["Emp_Id"].ToString();
            }
            else
            {
                DisplayMessage("Choose Employee in sugestion only");
                return;
            }


            if (Session["dtEmpList"] == null)
            {
                dt.Columns.Add("Employee_Id", typeof(int));
                dt.Columns.Add("EmpName", typeof(string));
                dt.Columns.Add("Emp_Code", typeof(string));

                DataRow dr = dt.NewRow();
                dr[0] = EmpId;
                dr[1] = ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text.Split('/')[0].ToString();
                dr[2] = ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text.Split('/')[1].ToString();

                dt.Rows.Add(dr);
            }
            else
            {
                DataView view = new DataView((DataTable)Session["dtEmpList"], "Employee_Id=" + EmpId + "", "", DataViewRowState.CurrentRows);

                if (view.ToTable().Rows.Count > 0)
                {
                    DisplayMessage("Employee Already Exists");
                    return;
                }
                dt = (DataTable)Session["dtEmpList"];
                DataRow dr = dt.NewRow();
                dr[0] = EmpId;
                dr[1] = ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text.Split('/')[0].ToString();
                dr[2] = ((TextBox)gridView.FooterRow.FindControl("txtEmpFooter")).Text.Split('/')[1].ToString();

                dt.Rows.Add(dr);
            }
            Session["dtEmpList"] = dt;
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtEmpList"] != null)
            {
                dt = (DataTable)Session["dtEmpList"];
                dt = new DataView(dt, "Employee_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtEmpList"] = dt;
            }
        }
        gridView.EditIndex = -1;
        LoadStores();

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmployeeMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        //dt = new DataView(dt, "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString();
        }
        return txt;
    }

    #endregion
    protected void ddlStatusFilter_Click(object sender, EventArgs e)
    {
        FillGrid();
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
    #region arcawing




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

    protected void OnDownloadCommand(object sender, CommandEventArgs e)
    {





        if (e.CommandName.ToString() == "")
        {
            DisplayMessage("File Not Found");
            return;
        }
        else
        {
            string filepath = "~/" + "ServiceManagement/Ticket/" + e.CommandArgument.ToString() + "/" + e.CommandName.ToString();

            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + e.CommandName.ToString() + "\"");
            Response.TransmitFile(Server.MapPath(filepath));
            Response.End();
        }


    }
    #endregion
    #region EmployeeFeedback


    protected void btnsaveFeedback_Click(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue == "Close")
        {
            DisplayMessage("Ticket closed ,you can not give feedback");
            return;
        }

        if (objAppParam.GetApplicationParameterByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count == 0)
        {
            DisplayMessage("Configure your support email first");
            return;
        }


        string Master_Email_SMTP = objAppParam.GetApplicationParameterValueByParamName("Support_Email_SMTP", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string Master_Email_Port = objAppParam.GetApplicationParameterValueByParamName("Support_Email_Port", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string Master_Email = objAppParam.GetApplicationParameterValueByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string Master_Email_Password = Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Support_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString());

        string Email_Display_Name = objAppParam.GetApplicationParameterValueByParamName("Support_Display_Text", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string filepath = string.Empty;
        string FileName = string.Empty;
        string strConversationType = string.Empty;
        if (txtFeedbackDate.Text == "")
        {
            DisplayMessage("Enter Feedback Date");
            txtFeedbackDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtFeedbackDate.Text);

            }
            catch
            {
                DisplayMessage("Enter Valid Date");
                txtFeedbackDate.Focus();
                return;
            }
        }


        if (txtAction.Text == "")
        {
            DisplayMessage("Enter Action !");
            txtAction.Focus();
            return;
        }


        if (chkInvolveCustomer.Checked == false && chkInvolveEmployee.Checked == false)
        {

            DisplayMessage("Please select option for involve in conversation");
            return;
        }


        if (chkInvolveCustomer.Checked)
        {
            strConversationType = "C";
        }

        if (chkInvolveEmployee.Checked)
        {

            if (chkInvolveCustomer.Checked)
            {
                strConversationType = "CE";
            }
            else
            {
                strConversationType = "E";

            }
        }

        if (UploadFeedbackFile.HasFile)
        {
            FileName = UploadFeedbackFile.FileName;
        }

        int b = ObjTicketFeedback.InsertRecord(editid.Value, Session["EmpId"].ToString(), ObjSysParam.getDateForInput(txtFeedbackDate.Text).ToString(), txtAction.Text, "0", ddlFeedbackStatus.SelectedValue, FileName, strConversationType, "", "", false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        if (ddlFeedbackStatus.SelectedItem.ToString() == "Close")
        {
            ddlStatus.SelectedValue = "Close";
            objTicketMaster.updateTicketStatus(Session["UserId"].ToString(), editid.Value);
        }
        //for arcawing
        //start
        if (UploadFeedbackFile.HasFile)
        {

            string DirectroryName = Server.MapPath("Feedback/" + b.ToString());

            CreateDirectoryIfNotExist(DirectroryName);

            filepath = "~/" + "ServiceManagement" + "/Feedback/" + b.ToString() + "/" + UploadFeedbackFile.FileName;

            if (!Directory.Exists(filepath))
            {
                UploadFile.SaveAs(Server.MapPath(filepath));
            }
        }



        //for send mail 

        //code start


        string strProductName = string.Empty;
        DataTable dtTicketProduct = objTicketproduct.GetRecordByTicketId(editid.Value);
        if (dtTicketProduct.Rows.Count > 0)
        {
            foreach (DataRow dr in dtTicketProduct.Rows)
            {
                if (strProductName == "")
                {
                    strProductName = dr["ProductName"].ToString();
                }
                else
                {
                    strProductName = strProductName + "," + dr["ProductName"].ToString();
                }
            }
        }

        string strCustomerName = string.Empty;
        string strContactNo = string.Empty;


        try
        {
            if (txtECustomer.Text.Contains("/"))
            {
                strCustomerName = txtECustomer.Text.Split('/')[0].ToString();
                strContactNo = txtECustomer.Text.Split('/')[1].ToString();
            }
            else
            {
                strCustomerName = txtECustomer.Text;
            }
        }
        catch
        {
        }


        string MailBody_Employee = string.Empty;
        string MailBody_Customer = string.Empty;





        MailBody_Employee = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Auto Reply - Customer Support System</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>Thanks for your interest in our organization. We’ve received your Ticket</p>       	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>" + txtAction.Text + "</p>                                                  <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + txtticketNo.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000; '>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(txtTicketDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + strCustomerName + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + txtEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + strContactNo + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td> </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + txtDescription.Text + "</td>    </tr></table><br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	 <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> Your Friendly Customer Support System</p></body></html>";
        MailBody_Customer = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>Pryce Client Support</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Auto Reply - Customer Support System</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>Thanks for your interest in our organization. We’ve received your Ticket</p>            	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'>" + txtAction.Text + "</p>                                              <h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Customer Support Team</h4>                </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'>	    <tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'>    	<td style='padding-left:10px;' colspan='2'>Ticket Information</td>    </tr>        <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Ticket No.</td>        <td style='padding-left:10px;'>" + txtticketNo.Text + "</td>    </tr>         <tr style='background:#eeeeee; color:#000; '>    	<td style='padding-left:10px;'>Ticket Date</td>        <td style='padding-left:10px;'>" + Convert.ToDateTime(txtTicketDate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString()) + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Customer Name</td>        <td style='padding-left:10px;'>" + strCustomerName + "</td>    </tr>	<tr style='color:#000; background:#eeeeee;'>    	<td style='padding-left:10px;'>Email</td>        <td style=' padding-left:10px;'>" + txtEmailId.Text + "</td>    </tr>        <tr style='background:#d0d1d2;'>    	<td style='padding-left:10px;'>Contact No.</td>        <td style='padding-left:10px;'>" + strContactNo + "</td>    </tr>        <tr style='background:#eeeeee;color:#000;'>    	<td style='padding-left:10px;'>Product</td>        <td style='padding-left:10px;'>" + strProductName + "</td>    </tr>             <tr style='background:#eeeeee; color:#000;'>    	<td style='padding-left:10px;'>Description</td>        <td style='padding-left:10px;'>" + txtDescription.Text + "</td>    </tr></table><br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	To add additional comments, you can simply click on this <a href=" + GetCustomerFeedbackURL() + "?TicketId=@T&&CompId=@C&&BrandId=@B&&LocId=@L>Link</a></p> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> Your Friendly Customer Support System</p></body></html>";

        MailBody_Customer = MailBody_Customer.Replace("@T", HttpUtility.UrlEncode(Common.Encrypt(editid.Value)));
        MailBody_Customer = MailBody_Customer.Replace("@C", HttpUtility.UrlEncode(Common.Encrypt(Session["CompId"].ToString())));
        MailBody_Customer = MailBody_Customer.Replace("@B", HttpUtility.UrlEncode(Common.Encrypt(Session["BrandId"].ToString())));
        MailBody_Customer = MailBody_Customer.Replace("@L", HttpUtility.UrlEncode(Common.Encrypt(Session["LocId"].ToString())));

        string strCreatedByEmailId = string.Empty;
        string strCC = string.Empty;
        string EmpId = string.Empty;



        DataTable dtTicket = objTicketMaster.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);


        try
        {
            EmpId = ObjUser.GetUserMasterByUserId(dtTicket.Rows[0]["CreatedBy"].ToString(), Session["CompId"].ToString()).Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
            EmpId = "0";
        }


        if (EmpId != "0")
        {
            strCreatedByEmailId = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmpId).Rows[0]["Email_Id"].ToString();
        }



        DataTable DtEmployeeList = objTicketEmployee.GetAllRecord_ByTicketId(editid.Value);


        foreach (DataRow dr in DtEmployeeList.Rows)
        {
            if (objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString() != "")
            {
                if (strCC == "")
                {
                    strCC = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                }
                else
                {
                    strCC = strCC + ";" + objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), dr["Employee_Id"].ToString()).Rows[0]["Email_Id"].ToString();
                }

            }
        }


        string strsubject = string.Empty;
        strsubject = "Ticket Feedback ," + txtticketNo.Text + " , " + strCustomerName + " ," + txtDescription.Text + "";
        //for send customer

        if (chkInvolveCustomer.Checked == true && chkInvolveEmployee.Checked == false)
        {
            //     ThreadStart ts = delegate()
            //{


            ObjSendMailSms.SendMail_TicketInfo(txtEmailId.Text, strCreatedByEmailId, "", strsubject, MailBody_Customer, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,string.Empty, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            //};
            //     Thread t = new Thread(ts);

            //     // Run the thread.
            //     t.Start();
        }


        if (chkInvolveCustomer.Checked == true && chkInvolveEmployee.Checked == true)
        {
            //     ThreadStart ts1 = delegate()
            //{

            ObjSendMailSms.SendMail_TicketInfo(txtEmailId.Text, "", "", strsubject, MailBody_Customer, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            //};
            //     Thread t1 = new Thread(ts1);

            //     // Run the thread.
            //     t1.Start();
        }
        //for send admin and related Employee
        if (chkInvolveEmployee.Checked == true)
        {
            //      ThreadStart ts2 = delegate()
            //{
            ObjSendMailSms.SendMail_TicketInfo(strCreatedByEmailId, strCC, "", strsubject, MailBody_Employee, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port,"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            //};
            //      Thread t2 = new Thread(ts2);

            //      // Run the thread.
            //      t2.Start();
        }


        //code end

        DisplayMessage("Feedback Saved");

        LoadFeedbackRecord(editid.Value);

        txtFeedbackDate.Text = "";
        txtAction.Text = "";
        chkInvolveCustomer.Checked = false;

        if (chkInvolveCustomer.Visible == false && chkInvolveEmployee.Visible == false)
        {

            chkInvolveEmployee.Checked = true;
        }
        else
        {
            chkInvolveEmployee.Checked = false;
        }
        txtFeedbackDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

    }
    public static string GetCustomerFeedbackURL()
    {
        string host = string.Empty;
        string strPort = string.Empty;
        string strLocalPath = string.Empty;
        string strFinalURL = string.Empty;

        host = HttpContext.Current.Request.Url.Host;
        strPort = HttpContext.Current.Request.Url.Port.ToString();
        strLocalPath = HttpContext.Current.Request.Url.LocalPath;


        strFinalURL = "http://";
        if (strPort != "")
        {
            strFinalURL += host + ":" + strPort + strLocalPath;
        }
        else
        {
            strFinalURL += host + strLocalPath;
        }
        strFinalURL = strFinalURL.Replace("TicketMaster.aspx", "ViewFeedBack.aspx");
        return strFinalURL;
    }

    protected void btnFeedbackRefresh_Click(object sender, EventArgs e)
    {
        LoadFeedbackRecord(editid.Value);

    }
    protected void gvFeedback_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    public void LoadFeedbackRecord(string TicketId)
    {
        DataTable dt = new DataTable();
        if (TicketId != "")
        {


            dt = new DataTable();
            dt = ObjTicketFeedback.GetAllRecord();

            try
            {
                //TicketId

                dt = new DataView(dt, "Ticket_No=" + TicketId + "", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (dt.Rows.Count > 0)
            {

                DataTable dtTicket = objTicketMaster.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, TicketId);


                if (dtTicket.Rows[0]["CreatedBy"].ToString() != Session["UserId"].ToString())
                {
                    dt = new DataView(dt, "Field3='CE' or Field3='E'", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable();
                }
                dt = dt.DefaultView.ToTable(true, "Trans_Id", "Date", "Field2", "Field1", "Action", "Emp_Name");

                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvFeedback, dt, "", "");
            }

        }


    }

    protected void gvFeedback_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string CustomerName = string.Empty;

        try
        {
            CustomerName = txtECustomer.Text.Split('/')[3].ToString();
        }
        catch
        {
            CustomerName = txtECustomer.Text;
        }
        if (ddlStatus.SelectedValue == "Close")
        {
            DisplayMessage("Ticket has been closed ,you can not add or delete Record !");
            return;
        }

        string filepath = string.Empty;
        string FileName = string.Empty;
        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvFeedback.FooterRow.FindControl("txtFeedbackDate")).Text == "")
            {
                DisplayMessage("Enter Feedback Date");
                ((TextBox)gvFeedback.FooterRow.FindControl("txtFeedbackDate")).Focus();
                return;
            }
            else
            {
                try
                {
                    Convert.ToDateTime(((TextBox)gvFeedback.FooterRow.FindControl("txtFeedbackDate")).Text);

                }
                catch
                {
                    DisplayMessage("Enter Valid Date");
                    ((TextBox)gvFeedback.FooterRow.FindControl("txtFeedbackDate")).Focus();
                    return;
                }
            }
            if (((TextBox)gvFeedback.FooterRow.FindControl("txtAction")).Text == "")
            {
                DisplayMessage("Enter Action");
                ((TextBox)gvFeedback.FooterRow.FindControl("txtAction")).Focus();
                return;
            }

            if (((FileUpload)gvFeedback.FooterRow.FindControl("UploadFeedbackFile")).HasFile)
            {
                FileName = ((FileUpload)gvFeedback.FooterRow.FindControl("UploadFeedbackFile")).FileName;
            }


            int b = ObjTicketFeedback.InsertRecord(editid.Value, Session["EmpId"].ToString(), ObjSysParam.getDateForInput(((TextBox)gvFeedback.FooterRow.FindControl("txtFeedbackDate")).Text).ToString(), ((TextBox)gvFeedback.FooterRow.FindControl("txtAction")).Text, "0", ((DropDownList)gvFeedback.FooterRow.FindControl("ddlStatus")).SelectedValue, FileName, "", "", "", false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());



            //for arcawing
            //start
            if (((FileUpload)gvFeedback.FooterRow.FindControl("UploadFeedbackFile")).HasFile)
            {

                string DirectroryName = Server.MapPath("Feedback/" + b.ToString());

                CreateDirectoryIfNotExist(DirectroryName);

                filepath = "~/" + "ServiceManagement" + "/Feedback/" + b.ToString() + "/" + ((FileUpload)gvFeedback.FooterRow.FindControl("UploadFeedbackFile")).FileName;

                if (!Directory.Exists(filepath))
                {
                    UploadFile.SaveAs(Server.MapPath(filepath));
                }
            }

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                string strAppMailId = string.Empty;
                string strAppPassword = string.Empty;
                DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (dtFrom.Rows.Count > 0)
                {
                    strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
                }
                DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (dtFrom.Rows.Count > 0)
                {
                    strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
                }
                string MailMessage = string.Empty;

                MailMessage = "Dear Sir , <br /><br />";


                MailMessage += ((TextBox)gvFeedback.FooterRow.FindControl("txtAction")).Text + "(" + ((TextBox)gvFeedback.FooterRow.FindControl("txtFeedbackDate")).Text + ") .";
                MailMessage += "<br /><br /> For check ticket Status or give feedback you can <a href=" + GetCustomerFeedbackURL() + "?TicketId=@T&&CompId=@C&&BrandId=@B&&LocId=@L>ClickHere</a><br /><br />From<br />" + ((LinkButton)Master.FindControl("lnkUserName")).Text;
                MailMessage = MailMessage.Replace("@T", HttpUtility.UrlEncode(Common.Encrypt(editid.Value)));
                MailMessage = MailMessage.Replace("@C", HttpUtility.UrlEncode(Common.Encrypt(Session["CompId"].ToString())));
                MailMessage = MailMessage.Replace("@B", HttpUtility.UrlEncode(Common.Encrypt(Session["BrandId"].ToString())));
                MailMessage = MailMessage.Replace("@L", HttpUtility.UrlEncode(Common.Encrypt(Session["LocId"].ToString())));
                if (txtEmailId.Text != "")
                {
                    //// ObjSendMailSms.SendApprovalMail(txtEmailId.Text, strAppMailId.ToString(), strAppPassword.ToString(), "From Pegasus Support(" + txtticketNo.Text + ")", MailMessage.ToString(), Session["CompId"].ToString(), "");

                    // //this code for send the mail from auto log process
                    // //code start

                    // string strsubject = string.Empty;
                    // strsubject = "Pegasus Support(" + txtticketNo.Text + ")";
                    // DateTime dtGeneratedate=new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,0,0,0);
                    // //string sql = "INSERT INTO [Ser_ReportLog]([Emp_Id]           ,[Message]           ,[Status]           ,[Type]           ,[Message_Heading]           ,[Generate_Date]           ,[Delivered_Date]           ,[Exception_Detail]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[IsActive]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate])     VALUES   ('0','" + MailMessage + "','Pending','Mail','Report','" + dtGeneratedate.ToString() + "','1/1/1900','1/1/1900','" + strsubject + "','" + txtEmailId.Text + "','" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "','" + false.ToString() + "','" + DateTime.Now.ToString() + "','" + true.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')";
                    // //objDa.execute_Command(sql);

                    // //code end
                }
            }

            gvFeedback.EditIndex = -1;
            LoadFeedbackRecord(editid.Value);
        }
        if (e.CommandName.Equals("Delete"))
        {



            dt = ObjTicketFeedback.GetRecord_ByTransId(e.CommandArgument.ToString());
            if (dt.Rows.Count != 0)
            {
                if (Session["EmpId"].ToString() != dt.Rows[0]["Emp_Id"].ToString())
                {
                    DisplayMessage("You Can not delete Record of another Employee");
                    return;
                }

                FileName = dt.Rows[0]["Field2"].ToString();
            }

            ObjTicketFeedback.DeleteRecord(e.CommandArgument.ToString(), "False", "superadmin", DateTime.Now.ToString());

            if (FileName != "")
            {
                try
                {
                    if (System.IO.File.Exists(Server.MapPath("Feedback/" + e.CommandArgument.ToString() + "/" + FileName)))
                    {
                        System.IO.File.Delete(Server.MapPath("Feedback/" + e.CommandArgument.ToString() + "/" + FileName));
                        Directory.Delete(Server.MapPath("Feedback/" + e.CommandArgument.ToString()));
                    }
                }
                catch
                {
                }
            }
            gvFeedback.EditIndex = -1;
            LoadFeedbackRecord(editid.Value);
        }
    }

    protected void gvFeedbackOnDownloadCommand(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() == "")
        {
            DisplayMessage("File Not Found");
            return;
        }
        else
        {
            string filepath = "~/" + "ServiceManagement/Feedback/" + e.CommandArgument.ToString() + "/" + e.CommandName.ToString();

            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + e.CommandName.ToString() + "\"");
            Response.TransmitFile(Server.MapPath(filepath));
            Response.End();
        }
    }
    protected void imgFeedback_OnCommand(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        pnlTicketDetail.Enabled = false;
        txtFeedbackDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        pnlFeedbacKdetail.Visible = true;
        BtnReset.Visible = false;
        btnPICancel.Visible = false;
        LoadFeedbackRecord(editid.Value);
        //AllPageCode();
    }

    #endregion
    #region addTools
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (HttpContext.Current.Session["hdnProductId"] != null)
        {
            dt = new DataView(dt, "ProductId<>" + HttpContext.Current.Session["hdnProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        }


        dt = new DataView(dt, "EProductName like '%" + prefixText.ToString() + "%'", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LOcId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (HttpContext.Current.Session["hdnProductId"] != null)
        {
            dt = new DataView(dt, "ProductId<>" + HttpContext.Current.Session["hdnProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        }


        dt = new DataView(dt, "ProductCode like '%" + prefixText.ToString() + "%'", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    protected void txtToolsERelatedProduct_OnTextChanged(object sender, EventArgs e)
    {



        if (Session["dtToolsList"] != null)
        {
            DataTable DtProduct = (DataTable)Session["dtToolsList"];

            try
            {
                DtProduct = new DataView(DtProduct, "ProductName='" + ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

            if (DtProduct.Rows.Count > 0)
            {
                DisplayMessage("Product is already exists");
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Focus();
                return;
            }

        }

        DataTable dt = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

        try
        {
            dt = new DataView(dt, "ProductName='" + ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (dt.Rows.Count > 0)
        {




            ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = dt.Rows[0]["ProductCode"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtproblem")).Focus();
        }
        else
        {
            ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
            ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = "0";
        }

    }
    protected void txttoolsProductCode_OnTextChanged(object sender, EventArgs e)
    {



        if (Session["dtToolsList"] != null)
        {
            DataTable DtProduct = (DataTable)Session["dtToolsList"];

            try
            {
                DtProduct = new DataView(DtProduct, "ProductCode='" + ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }

            if (DtProduct.Rows.Count > 0)
            {
                DisplayMessage("Product id is already exists");
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Focus();
                return;
            }

        }
        DataTable dt = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

        try
        {
            dt = new DataView(dt, "ProductCode='" + ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (dt.Rows.Count > 0)
        {


            ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = dt.Rows[0]["EProductName"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtproblem")).Focus();
        }
        else
        {
            ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
            ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = "0";
        }

    }
    protected void gvTools_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvTools_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }
    protected void gvTools_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvTools.EditIndex = -1;
        LoadToolsRecord();
    }



    public void LoadToolsRecord()
    {
        DataTable dt = new DataTable();
        if (Session["dtToolsList"] != null)
        {


            dt = new DataTable();
            dt = (DataTable)Session["dtToolsList"];



            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvTools, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("ProductId", typeof(string));
                contacts.Columns.Add("ProductCode", typeof(string));
                contacts.Columns.Add("ProductName", typeof(string));
                contacts.Columns.Add("Problem", typeof(string));
                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvTools, contacts, "", "");
                int TotalColumns = gvTools.Rows[0].Cells.Count;
                gvTools.Rows[0].Cells.Clear();
                gvTools.Rows[0].Cells.Add(new TableCell());
                gvTools.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvTools.Rows[0].Visible = false;
            }

        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("ProductId", typeof(string));
            contacts.Columns.Add("ProductCode", typeof(string));
            contacts.Columns.Add("ProductName", typeof(string));
            contacts.Columns.Add("Problem", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvTools, contacts, "", "");
            int TotalColumns = gvTools.Rows[0].Cells.Count;
            gvTools.Rows[0].Cells.Clear();
            gvTools.Rows[0].Cells.Add(new TableCell());
            gvTools.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvTools.Rows[0].Visible = false;
        }

    }
    protected void gvTools_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        DataTable dt = new DataTable();

        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text == "")
            {
                DisplayMessage("Enter Product Code");
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Focus();
                return;
            }

            if (((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text == "")
            {
                DisplayMessage("Enter Product Name");
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Focus();
                return;
            }

            if (((TextBox)gvTools.FooterRow.FindControl("txtproblem")).Text == "")
            {
                DisplayMessage("Enter Item problem");
                ((TextBox)gvTools.FooterRow.FindControl("txtproblem")).Focus();
                return;
            }
            bool isToolsExists = false;

            if (Session["dtToolsList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("ProductId", typeof(string));
                dt.Columns.Add("ProductCode", typeof(string));
                dt.Columns.Add("ProductName", typeof(string));
                dt.Columns.Add("Problem", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((TextBox)gvTools.FooterRow.FindControl("txtproblem")).Text;

                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtToolsList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                    strTransid = (float.Parse(dtCopy.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
                }
                else
                {
                    strTransid = "1";
                }

                DataRow dr = dt.NewRow();
                dr[0] = strTransid;
                dr[1] = ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((TextBox)gvTools.FooterRow.FindControl("txtproblem")).Text;
                dt.Rows.Add(dr);
            }
            Session["dtToolsList"] = dt;

        }
        if (e.CommandName.Equals("Delete"))
        {

            if (Session["dtToolsList"] != null)
            {
                dt = (DataTable)Session["dtToolsList"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtToolsList"] = dt;
            }


        }
        gvTools.EditIndex = -1;
        LoadToolsRecord();
    }
    #endregion
    #region Visit

    public void insertRecordIngeotrackingdatabse(string EmpId, string VisitId, string Customerid, string CustomerName, DateTime Visitdate, string strVisittime)
    {

        //get customer information

        //code start
        string[] strcustInfo = getCustomerInformation(Customerid);
        string Address = strcustInfo[0].ToString();
        string ContactNo = strcustInfo[1].ToString();
        string Latitude = strcustInfo[3].ToString();
        string Longitude = strcustInfo[2].ToString();
        //code end

        DataTable dtGeotracking = new DataTable();

        //here we check that geotracking database is exist or not if not than return the finunction 
        try
        {
            dtGeotracking = objDa.return_DataTable("select * from sys.databases where name='GeoTracking'");
        }
        catch
        {

        }

        if (dtGeotracking.Rows.Count > 0)
        {

            string Sql = string.Empty;
            string strRegistrationid = ConfigurationManager.AppSettings["RegistrationId"].ToString();

            DataTable dtEmployee = objEmployee.GetEmployeeMasterAll(Session["CompId"].ToString());
            try
            {
                dtEmployee = new DataView(dtEmployee, "Emp_id=" + EmpId + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtEmployee.Rows.Count > 0)
            {

                //first we chekc that user exist or not in getracker database

                if (objDa.return_DataTable("select * from GeoTracking.dbo.Set_User where user_code='" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "'").Rows.Count == 0)
                {
                    Sql = "insert into GeoTracking.dbo.Set_User values(" + strRegistrationid + ",1,'" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','" + dtEmployee.Rows[0]["Emp_Name"].ToString() + "','" + dtEmployee.Rows[0]["Email_Id"].ToString() + "','" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','0','" + dtEmployee.Rows[0]["Phone_No"].ToString() + "','0','0','True','0','" + DateTime.Now.ToString() + "')";
                    objDa.execute_Command(Sql);
                }


                //here we chekc that record is exist or not in trn_orderlit table
                //if exist than update otherwise insert


                string strhour = string.Empty;
                string strminute = string.Empty;
                strhour = strVisittime.Split(':')[0].ToString();
                strminute = strVisittime.Split(':')[1].ToString();


                DateTime dtvisitDate = new DateTime(Visitdate.Year, Visitdate.Month, Visitdate.Day, Convert.ToInt32(strhour), Convert.ToInt32(strminute), 0);

                //herw we chekc that record exist or not in Trns_Order_List table 
                if (objDa.return_DataTable("select * from GeoTracking.dbo.Trns_Order_List where Ref_Order_No='" + VisitId + "'").Rows.Count == 0)
                {

                    Sql = "insert into GeoTracking.dbo.Trns_Order_List values(" + strRegistrationid + ",'" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "','Loc-0001','" + DateTime.Now.ToString() + "','" + VisitId + "',' ','" + CustomerName + "','" + ContactNo + "',' ','" + Address + "','" + dtvisitDate.ToString() + "','" + Latitude + "','" + Longitude + "','" + false.ToString() + "','False','False','" + DateTime.Now.ToString() + "',' ',' ',' ','0','0','0','True','1','" + DateTime.Now.ToString() + "')";

                    objDa.execute_Command(Sql);

                    string RefId = objDa.return_DataTable("select max(Trans_Id) as Trans_Id from GeoTracking.dbo.Trns_Order_List").Rows[0]["Trans_Id"].ToString();
                    int i = 0;


                    if (Session["dtEmpList"] != null)
                    {
                        DataTable DtEmployeeList = (DataTable)Session["dtEmpList"];

                        foreach (DataRow dr in DtEmployeeList.Rows)
                        {
                            i++;
                            Sql = "insert into GeoTracking.dbo.Trns_Order_Emp values('" + RefId + "','" + i.ToString() + "','" + dr["EmpName"].ToString() + "',' ')";
                            objDa.execute_Command(Sql);
                        }
                    }

                }
                else
                {

                    DataTable dtOrderList = objDa.return_DataTable("select * from GeoTracking.dbo.Trns_Order_List where Ref_Order_No='" + VisitId + "'");
                    string OrderTransId = dtOrderList.Rows[0]["Trans_Id"].ToString();
                    string strCustomerName = dtOrderList.Rows[0]["Contact_Person"].ToString();
                    if (strCustomerName != CustomerName)
                    {
                        Sql = "update GeoTracking.dbo.Trns_Order_List set User_Code='" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "',Contact_Person='" + CustomerName + "',Mobile_No='" + ContactNo + "',Address='" + Address + "',Appointment_Time='" + dtvisitDate.ToString() + "',Latitude='" + Latitude + "',Longitude='" + Longitude + "', Edited_By='" + VisitId + "',Edited_Date='" + DateTime.Now.ToString() + "' where trans_id=" + OrderTransId + "";
                    }
                    else
                    {
                        Sql = "update GeoTracking.dbo.Trns_Order_List set User_Code='" + dtEmployee.Rows[0]["Emp_Code"].ToString() + "',Appointment_Time='" + dtvisitDate.ToString() + "', Edited_By='" + VisitId + "',Edited_Date='" + DateTime.Now.ToString() + "' where trans_id=" + OrderTransId + "";

                    }
                    objDa.execute_Command(Sql);

                    //delerte record in order_emp table and reinserted 
                    Sql = "delete from GeoTracking.dbo.Trns_Order_Emp where ref_id=" + OrderTransId + "";
                    objDa.execute_Command(Sql);

                    //reinsert record in order detail
                    int i = 0;
                    if (Session["dtEmpList"] != null)
                    {
                        DataTable DtEmployeeList = (DataTable)Session["dtEmpList"];

                        foreach (DataRow dr in DtEmployeeList.Rows)
                        {
                            i++;
                            Sql = "insert into GeoTracking.dbo.Trns_Order_Emp values('" + OrderTransId + "','" + i.ToString() + "','" + dr["EmpName"].ToString() + "',' ')";
                            objDa.execute_Command(Sql);
                        }
                    }
                }
            }
        }
    }
    public string[] getCustomerInformation(string Contactid)
    {
        //code start
        string[] strCusInfo = new string[4];
        string strContactNo = string.Empty;
        string Address = string.Empty;
        string Longitude = string.Empty;
        string Latitude = string.Empty;


        DataTable dtContact = objContact.GetContactTrueById(Contactid);
        if (dtContact.Rows.Count > 0)
        {
            strCusInfo[1] = dtContact.Rows[0]["Field2"].ToString();
            if (strCusInfo[1] == null)
            {
                strCusInfo[1] = "";
            }
        }
        else
        {
            strCusInfo[1] = "";
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

            Address = dt.Rows[0]["Address"].ToString();
            if (dt.Rows[0]["Address"].ToString() != "")
            {
                Address = dt.Rows[0]["Address"].ToString();
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
            strCusInfo[0] = Address;


            strCusInfo[2] = dt.Rows[0]["Longitude"].ToString();
            strCusInfo[3] = dt.Rows[0]["Latitude"].ToString();
        }
        else
        {
            strCusInfo[0] = "";
            strCusInfo[2] = "0.0000";
            strCusInfo[3] = "0.0000";
        }

        return strCusInfo;
    }
    public void InsertTaskEmployee(string RefId)
    {

        //this function for insert record in task_employee

        //first delete record by task_id
        objTaskEmp.DeleteRecord_By_RefTypeandRefid("Visit", RefId);

        if (Session["dtEmpList"] != null)
        {
            DataTable DtEmployeeList = (DataTable)Session["dtEmpList"];

            foreach (DataRow dr in DtEmployeeList.Rows)
            {
                objTaskEmp.InsertRecord("Visit", RefId, dr["Employee_Id"].ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVehicleName(string prefixText, int count, string contextKey)
    {
        Prj_VehicleMaster objVehicleMaster = new Prj_VehicleMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = objVehicleMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Vehicle_Id"].ToString();
        }
        return txt;
    }
    protected void txtvehiclename_TextChanged(object sender, EventArgs e)
    {
        if (txtvehiclename.Text.Trim() != "")
        {

            DataTable dt = objVehicleMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            try
            {
                dt = new DataView(dt, "Name='" + txtvehiclename.Text.Trim().Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Vehicle not found");
                txtvehiclename.Text = "";
                txtvehiclename.Focus();
                return;
            }
            else
            {
                if (dt.Rows[0]["Emp_id"].ToString() != "0")
                {
                    txtdrivername.Text = dt.Rows[0]["Emp_Name"].ToString() + "/" + dt.Rows[0]["Emp_id"].ToString();
                }
            }
        }

    }
    protected void txtEmpName_TextChanged(object sender, EventArgs e)
    {

        string empname = string.Empty;
        if (txtdrivername.Text != "")
        {
            try
            {
                empname = txtdrivername.Text.Split('/')[0].ToString();
            }
            catch
            {
                DisplayMessage("Employee Not Exists");
                txtdrivername.Text = "";
                txtdrivername.Focus();
                return;
            }

            DataTable dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Emp_Name='" + empname + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtEmp.Rows.Count == 0)
            {
                DisplayMessage("Employee Not Exists");
                txtdrivername.Text = "";
                txtdrivername.Focus();
                return;
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {

        EmployeeMaster objemployeemaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objemployeemaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        try
        {
            dt = new DataView(dt, "Emp_Name like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString();
        }
        return str;
    }

    protected void btnvisitsave_Click(object sender, EventArgs e)
    {


        if (txtVisitDate.Text == "")
        {
            DisplayMessage("Enter Visit Date");
            txtVisitDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtVisitDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Visit Date");
                txtVisitDate.Focus();
                return;
            }

        }


        if (txtVisitTime.Text == "")
        {
            DisplayMessage("Enter Visit time");
            txtVisitTime.Focus();
            return;
        }

        if (txtvehiclename.Text == "")
        {
            DisplayMessage("Enter Vehicle Name");
            txtvehiclename.Focus();
            return;
        }

        if (txtdrivername.Text == "")
        {
            DisplayMessage("Enter Driver Name");
            txtdrivername.Focus();
            return;
        }
        DataTable dt = new DataTable();

        if (Session["DtVisit"] != null)
        {
            dt = (DataTable)Session["DtVisit"];
        }
        else
        {

            dt.Columns.Add("Trans_Id");
            dt.Columns.Add("Visit_Date");
            dt.Columns.Add("Vehicle_Id");
            dt.Columns.Add("Driver_Id");
            dt.Columns.Add("Visit_Time");
            dt.Columns.Add("EmpName");
            dt.Columns.Add("VehicleName");
        }

        dt.Rows.Add();
        if (dt.Rows.Count == 1)
        {
            dt.Rows[dt.Rows.Count - 1]["Trans_Id"] = 1;
        }
        else
        {
            dt.Rows[dt.Rows.Count - 1]["Trans_Id"] = (Convert.ToInt32(dt.Rows[dt.Rows.Count - 2]["Trans_Id"].ToString()) + 1).ToString();
        }
        dt.Rows[dt.Rows.Count - 1]["Visit_Date"] = txtVisitDate.Text;
        dt.Rows[dt.Rows.Count - 1]["Vehicle_Id"] = txtvehiclename.Text.Split('/')[1].ToString();
        dt.Rows[dt.Rows.Count - 1]["Driver_Id"] = txtdrivername.Text.Split('/')[1].ToString();
        dt.Rows[dt.Rows.Count - 1]["Visit_Time"] = txtVisitTime.Text;
        dt.Rows[dt.Rows.Count - 1]["EmpName"] = txtdrivername.Text.Split('/')[0].ToString();
        dt.Rows[dt.Rows.Count - 1]["VehicleName"] = txtvehiclename.Text.Split('/')[0].ToString();

        fillVisitGrid(dt);

        if (editid.Value != "")
        {
            string StrCustomer = string.Empty;
            bool strrValues = false;
            string[] strCustInfo = new string[4];
            string Customerid = string.Empty;
            string CustomerName = string.Empty;

            if (txtECustomer.Text != "")
            {

                try
                {
                    StrCustomer = txtECustomer.Text.Split('/')[3].ToString();
                    strrValues = true;
                    //get customer information

                    //code start
                    strCustInfo = getCustomerInformation(StrCustomer);
                    //code end
                    CustomerName = txtECustomer.Text.Split('/')[0].ToString();
                    Customerid = txtECustomer.Text.Split('/')[3].ToString();
                }
                catch
                {
                    StrCustomer = txtECustomer.Text;
                    strrValues = false;
                    strCustInfo = getCustomerInformation("0");
                    Customerid = "0";
                    CustomerName = txtECustomer.Text;
                }

            }
            int visitId = 0;

            visitId = objVisitMaster.InsertRecord("Ticket", editid.Value, "0", StrCustomer, ObjSysParam.getDateForInput(txtVisitDate.Text).ToString(), txtvehiclename.Text.Split('/')[1].ToString(), txtdrivername.Text.Split('/')[1].ToString(), "0", "Open", "", "", "", "", txtVisitTime.Text, false.ToString(), strCustInfo[0].ToString(), strCustInfo[1].ToString(), "", strCustInfo[2].ToString(), strCustInfo[3].ToString(), "", "", "", "", "", strrValues.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


            InsertTaskEmployee(visitId.ToString());

            insertRecordIngeotrackingdatabse(txtdrivername.Text.Split('/')[1].ToString(), visitId.ToString(), Customerid, CustomerName, Convert.ToDateTime(txtVisitDate.Text), txtVisitTime.Text);


        }
        ResetVisitPanel();
    }




    public void ResetVisitPanel()
    {
        txtVisitDate.Text = "";
        txtVisitTime.Text = "";
        txtdrivername.Text = "";
        txtvehiclename.Text = "";
        txtVisitDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
    }

    public void fillVisitGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvVisitMaster, dt, "", "");
        Session["DtVisit"] = dt;
        //AllPageCode();
    }
    protected void IbtnDeleteVisit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView((DataTable)Session["DtVisit"], "Trans_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        fillVisitGrid(dt);

    }




    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        try
        {
            if (customerID == "0")
            {
                string[] arr = { };
                return arr;
            }

            Inv_SalesInvoiceHeader objSinvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());

            if (customerID == "")
            {
                getCustomerID();
            }

            DataTable dt = objSinvoiceHeader.GetSInvHeaderAllTrueByCustomerID(customerID);

            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Invoice_No"].ToString();
                }
            }

            return str;
        }
        catch
        {
            string[] arr = { };
            return arr;
        }
    }

    public static string getCustomerID()
    {
        try
        {
            Page page = (Page)HttpContext.Current.Handler;
            TextBox TextBox1 = (TextBox)page.FindControl("txtECustomer");
            customerID = TextBox1.Text.Split('/')[3].ToString();
            return customerID;
        }
        catch
        {
            return "0";
        }
    }

    protected void btnHistory_Command(object sender, CommandEventArgs e)
    {

        if (customerID == "0" || customerID == "")
        {
            txtECustomer.Text = "";
            txtECustomer.Focus();
            DisplayMessage("Enter Costomer Name");
            return;
        }

        DataTable dtTicketHistory = objTicketMaster.GetRecordByCustomerID(customerID);

        if (dtTicketHistory.Rows.Count > 0)
        {
            //dtTicketHistory = new DataView(dtTicketHistory, "Task_Type='Service'", "", DataViewRowState.CurrentRows).ToTable();
            gvTicketHistory.DataSource = dtTicketHistory;
            gvTicketHistory.DataBind();
        }
        else
        {
            gvTicketHistory.DataSource = null;
            gvTicketHistory.DataBind();
        }

        DataTable dtVisitHistory = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString()).GetRecordByCustomerID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), customerID);

        if (dtVisitHistory.Rows.Count > 0)
        {
            gvVisitHistory.DataSource = dtVisitHistory;
            gvVisitHistory.DataBind();
        }
        else
        {
            gvVisitHistory.DataSource = null;
            gvVisitHistory.DataBind();
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_TicketHistory()", true);

    }

    protected void txtExpectedEndDate_TextChanged(object sender, EventArgs e)
    {
        if (txtScheduleDate.Text.Trim() != "" && txtExpectedEndDate.Text.Trim() != "")
        {
            if (ObjSysParam.getDateForInput(txtScheduleDate.Text) > ObjSysParam.getDateForInput(txtExpectedEndDate.Text))
            {
                DisplayMessage("Scheduled Date Cant be Greater Then Expected End Date");
                txtExpectedEndDate.Text = "";
                txtExpectedEndDate.Focus();
            }
        }

        if (txtTicketDate.Text.Trim() != "" && txtExpectedEndDate.Text.Trim() != "")
        {
            if (ObjSysParam.getDateForInput(txtTicketDate.Text) > ObjSysParam.getDateForInput(txtExpectedEndDate.Text))
            {
                DisplayMessage("Ticket Date Cant be Greater Then Expected End Date");
                txtExpectedEndDate.Text = "";
                txtExpectedEndDate.Focus();
            }
        }
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

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();

    }
}
