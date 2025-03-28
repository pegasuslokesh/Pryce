using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using DevExpress.Web;

public partial class ServiceManagement_WorkOrder : System.Web.UI.Page
{
    SM_WorkOrder ObjWorkOrder = null;
    SM_Ticket_Master objTicketMaster = null;
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    SM_JobPlan_Header objjobPlanHeader = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    Sm_TicketFeedback ObjTicketFeedback = null;
    SM_Ticket_Master objticketmaster = null;
    SM_JobPlan_Detail objjobPlanDetail = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster objUnitMaster = null;
    Prj_Project_Tools objProjecttools = null;
    Prj_Project_Task_Employeee objTaskEmp = null;
    Prj_ProjectMaster objProjctMaster = null;
    SM_Contract_Master objContractMaster = null;
    DataAccessClass objDa = null;
    Set_AddressChild objAddChild = null;
    Prj_VehicleMaster objVehicleMaster = null;
    Prj_VisitMaster objVisitMaster = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Set_AddressMaster objAddress = null;
    Common cmn = null;
    Prj_ProjectTask objProjectTask = null;
    Prj_ProjectTeam objProjectTeam = null;
    Inv_SalesInvoiceHeader ObjSalesInvoiceheader = null;
    UserMaster ObjUser = null;
    Common.clsPagePermission clsPagePermission;
    Set_CustomerMaster objCustomer = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjWorkOrder = new SM_WorkOrder(Session["DBConnection"].ToString());
        objTicketMaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objjobPlanHeader = new SM_JobPlan_Header(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjTicketFeedback = new Sm_TicketFeedback(Session["DBConnection"].ToString());
        objticketmaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        objjobPlanDetail = new SM_JobPlan_Detail(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objProjecttools = new Prj_Project_Tools(Session["DBConnection"].ToString());
        objTaskEmp = new Prj_Project_Task_Employeee(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        objContractMaster = new SM_Contract_Master(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        objVisitMaster = new Prj_VisitMaster(Session["DBConnection"].ToString());
        objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSInvDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        objAddress = new Set_AddressMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProjectTask = new Prj_ProjectTask(Session["DBConnection"].ToString());
        objProjectTeam = new Prj_ProjectTeam(Session["DBConnection"].ToString());
        ObjSalesInvoiceheader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();
        if (!IsPostBack)
        {
            clsPagePermission = cmn.getPagePermission("../ServiceManagement/WorkOrder.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLoc);
            Reset();

            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            btnRefreshReport_Click(null, null);
            Session["dtVisitTaskList"] = null;
            //LoadTask();
            Session["dtToolsList"] = null;
            LoadToolsRecord();
            btnAddCustomer.Visible = IsAddCustomerPermission();
            if (Request.QueryString["Followup_CustomerID"] != null)
            {
                DataTable DtDataByID = objContact.GetContactTrueById(Request.QueryString["Followup_CustomerID"].ToString());
                txtCustomer.Text = DtDataByID.Rows[0]["Name"].ToString() + "/" + DtDataByID.Rows[0]["Trans_Id"].ToString();
                txtEContact.Text = DtDataByID.Rows[0]["Name"].ToString() + "/" + DtDataByID.Rows[0]["Trans_Id"].ToString();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtorderNo);
                if (!ClientScript.IsStartupScriptRegistered("alert"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alert", "alertMe();", true);
                }
            }
            if (Request.QueryString["SearchField"] != null)
            {
                ddlPosted.SelectedIndex = 3;
                ddlOption.SelectedIndex = 2;
                ddlFieldName.SelectedValue = "Ref_Id";
                txtValue.Text = Request.QueryString["SearchField"].ToString();
                btnbindrpt_Click(null, null);
            }

            if (Request.QueryString["ReminderId"] != null)
            {
                btnEdit_Command("btnEdit", Request.QueryString["ReminderId"].ToString());
            }
        }
        GvWorkOrder.DataSource = Session["dtCInquiry"] as DataTable;
        GvWorkOrder.DataBind();
        btnInquirySave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
        btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        hasvalue.Value = "yes";
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
        canEdit.Value = clsPagePermission.bEdit.ToString();
        canView.Value = clsPagePermission.bView.ToString();
        canDelete.Value = clsPagePermission.bDelete.ToString();
        canSave.Value = clsPagePermission.bAdd.ToString();
        canPrint.Value = clsPagePermission.bPrint.ToString();
        canUpload.Value = clsPagePermission.bUpload.ToString();
        canFollowup.Value = "true";
        hdnHavePermission.Value = "false";
        btnInquirySave.Visible = clsPagePermission.bAdd;
        btnInquiryCloseandInvoice.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        btnClose.Visible = clsPagePermission.bAdd;

        string strModuleId = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("323", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "323",Session["CompId"].ToString());
        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "23")
            {
                hdnHavePermission.Value = "true";
                break;
            }
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillBinGrid();
        ddlFieldNameBin.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Focus();
        ddlFieldNameBin.Focus();
    }
    protected void btnInquiryCloseandInvoice_Click(object sender, EventArgs e)
    {
        if (hdnValue.Value != "")
        {
            DataTable dtInvoice = objDa.return_DataTable("select Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader where SIFromTransType='W' and SIFromTransNo=" + hdnValue.Value + " and Post='True' ");
            if (dtInvoice.Rows.Count > 0)
            {
                btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
                DisplayMessage("Invoice already posted , you can not update it");
                return;
            }
        }
        btnInquirySave_Click(sender, e);
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        btnInquirySave_Click(sender, e);
    }
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {
        string strtaskTd = "0";
        string strSiteAddress = string.Empty;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (((Button)sender).ID.Trim() == "btnClose")
        {
            ddlStatus.SelectedIndex = 1;
        }
        else
        {
            ddlStatus.SelectedIndex = 0;
        }
        string jobPlanId = string.Empty;
        try
        {
            if (txtjobPlanId.Text.Trim() != "")
            {
                jobPlanId = txtjobPlanId.Text.Split('/')[1].ToString();
            }
            else
            {
                jobPlanId = "0";
            }
        }
        catch
        {
            jobPlanId = "0";
        }
        if (txtInTime.Text == "__:__" || txtInTime.Text == "")
        {
            btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
            btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
            DisplayMessage("Enter Start Time");
            txtInTime.Focus();
            btnInquirySave.Enabled = true;
            return;
        }
        if (txtOuttime.Text == "__:__" || txtOuttime.Text == "")
        {
            btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
            btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
            DisplayMessage("Enter End Time");
            txtOuttime.Focus();
            btnInquirySave.Enabled = true;
            return;
        }
        if (txtorderNo.Text == "")
        {
            btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
            btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
            DisplayMessage("Enter work order no.");
            txtorderNo.Focus();
            btnInquirySave.Enabled = true;
            return;
        }
        if (txtOrderDate.Text == "")
        {
            btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
            btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
            DisplayMessage("Enter work ordre date");
            txtOrderDate.Focus();
            btnInquirySave.Enabled = true;
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtOrderDate.Text.Trim());
            }
            catch
            {
                txtOrderDate.Text = "";
                txtOrderDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
                txtOrderDate.Focus();
                btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
                btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
                DisplayMessage("Enter valid Date");
                btnInquirySave.Enabled = true;
                return;
            }
        }
        if (txtSiteAddress.Text != "")
        {
            DataTable dtAddId = objAddress.GetAddressDataByAddressName(txtSiteAddress.Text);
            if (dtAddId.Rows.Count > 0)
            {
                strSiteAddress = dtAddId.Rows[0]["Trans_Id"].ToString();
            }
        }
        else
        {
            strSiteAddress = "0";
        }
        //ref No. Validation
        if (ddlReftype.SelectedIndex == 1 || ddlReftype.SelectedIndex == 2 || ddlReftype.SelectedIndex == 3 || ddlReftype.SelectedIndex == 4)
        {
            if (txtRefNo.Text.Trim() == "")
            {
                btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
                btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
                DisplayMessage("Enter Ref No.");
                txtRefNo.Focus();
                btnInquirySave.Enabled = true;
                return;
            }
        }
        if (ddlReftype.SelectedIndex == 1)
        {
            if (ddltask.SelectedIndex > 0)
            {
                strtaskTd = ddltask.SelectedValue;
            }
        }
        string StrCustomer = string.Empty;
        if (txtCustomer.Text == "")
        {
            StrCustomer = "0";
        }
        else
        {
            int start_pos = txtCustomer.Text.LastIndexOf("/") + 1;
            int last_pos = txtCustomer.Text.Length;
            string id = txtCustomer.Text.Substring(start_pos, last_pos - start_pos);
            StrCustomer = id;
        }
        string strContactPersonId = string.Empty;
        if (txtEContact.Text.Trim() == "")
        {
            strContactPersonId = "0";
        }
        else
        {
            int start_pos = txtEContact.Text.LastIndexOf("/") + 1;
            int last_pos = txtEContact.Text.Length;
            string id = txtEContact.Text.Substring(start_pos, last_pos - start_pos);
            strContactPersonId = id;
        }
        string strHandledEmployee = string.Empty;
        if (txtHandledEmp.Text == "")
        {
            strHandledEmployee = "0";
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strHandledEmployee = Emp_ID;
        }
        if (ddlReftype.SelectedValue == "Campaign")
        {
            int start_pos1 = txtRefNo.Text.LastIndexOf("/") + 1;
            int last_pos1 = txtRefNo.Text.Length;
            string id1 = txtRefNo.Text.Substring(start_pos1, last_pos1 - start_pos1);
            txtRefNo.Text = id1;
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            //for new work order
            if (hdnValue.Value == "")
            {
                if (txtorderNo.Text != "")
                {
                    if (ObjWorkOrder.validateWorkorderExistOrNot(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtorderNo.Text.Trim(), ref trns))
                    {
                        btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
                        btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
                        DisplayMessage("Work Order No. Already Exist");
                        txtorderNo.Text = "";
                        btnInquirySave.Enabled = true;
                        return;
                    }
                }
                int b = 0;
                b = ObjWorkOrder.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtorderNo.Text, objSysParam.getDateForInput(txtOrderDate.Text.Trim()).ToString(), ddlReftype.SelectedValue, txtRefNo.Text, StrCustomer, strHandledEmployee, DdlWorkType.SelectedValue, "", ddlStatus.SelectedValue, txtProbelm.Text, txtEngComments.Text, strSiteAddress, "0", strtaskTd, txtInTime.Text, txtOuttime.Text, hdnTicketid.Value, txtticketno.Text, jobPlanId, strContactPersonId, txtCustomerfeedback.Text, false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtWorkingHr.Text, ref trns);
                string strMaxId = string.Empty;
                if (b != 0)
                {
                    strMaxId = b.ToString();
                    string dtCount = ObjWorkOrder.getAllCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
                    ObjWorkOrder.Updatecode(b.ToString(), txtorderNo.Text + dtCount, ref trns);
                    txtorderNo.Text = txtorderNo.Text + dtCount;

                    //here we save record for work list
                    //here we insert record in detail table
                    if (Session["dtVisitTaskList"] != null)
                    {
                        int i = 0;
                        foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                        {
                            i++;
                            objjobPlanDetail.InsertRecord("WORK", b.ToString(), dr["Work"].ToString(), dr["Minute"].ToString(), ref trns);
                        }
                    }
                    //here we are inserting multiple employee 
                    objTaskEmp.DeleteRecord_By_RefTypeandRefid("WO", b.ToString(), ref trns);
                    int start_pos, last_pos;
                    string id, StrTo = "";
                    foreach (string name in txtselectEmployee.Text.Split(';'))
                    {
                        start_pos = name.LastIndexOf("/") + 1;
                        last_pos = name.Length;
                        id = name.Substring(start_pos, last_pos - start_pos);
                        if (id != "")
                        {
                            if (!StrTo.Split(';').Contains(id))
                            {
                                objTaskEmp.InsertRecord("WO", b.ToString(), id, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                StrTo = StrTo + id + ";";
                            }
                        }
                    }
                    if (Session["dtToolsList"] != null)
                    {
                        string Toolsid = string.Empty;
                        DataTable dtProduct = (DataTable)Session["dtToolsList"];
                        foreach (DataRow dr in dtProduct.Rows)
                        {
                            if (dr["Tools_Id"].ToString() == "0")
                            {
                                Toolsid = dr["ProductCode"].ToString();
                            }
                            else
                            {
                                Toolsid = dr["Tools_Id"].ToString();
                            }
                            objProjecttools.InsertRecord(b.ToString(), Toolsid, dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), dr["IsToolsExists"].ToString(), "WORK", dr["Field2"].ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    if (((Button)sender).ID.Trim() == "btnInquiryCloseandInvoice")
                    {
                        SaveInvoice(b.ToString(), StrCustomer, ref trns);
                    }
                    //code end
                    btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
                    btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
                    DisplayMessage("Record Saved", "green");
                    btnInquirySave.Enabled = true;
                }
            }
            else
            {
                // for extended workorder
                if (hdnExtendedID.Value != "0")
                {
                    if (hdnExtendedID.Value != "")
                    {
                        int b = 0;
                        b = ObjWorkOrder.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtorderNo.Text, objSysParam.getDateForInput(txtOrderDate.Text.Trim()).ToString(), ddlReftype.SelectedValue, txtRefNo.Text, StrCustomer, strHandledEmployee, DdlWorkType.SelectedValue, "", ddlStatus.SelectedValue, txtProbelm.Text, txtEngComments.Text, strSiteAddress, "0", strtaskTd, txtInTime.Text, txtOuttime.Text, hdnTicketid.Value, txtticketno.Text, jobPlanId, strContactPersonId, txtCustomerfeedback.Text, false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtWorkingHr.Text, ref trns);
                        string strMaxId = string.Empty;
                        if (b != 0)
                        {
                            strMaxId = b.ToString();
                            string dtCount = ObjWorkOrder.getAllCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
                            ObjWorkOrder.Updatecode(b.ToString(), txtorderNo.Text + dtCount, ref trns);
                            txtorderNo.Text = txtorderNo.Text + dtCount;

                            //here we save record for work list
                            //here we insert record in detail table
                            if (Session["dtVisitTaskList"] != null)
                            {
                                int i = 0;
                                foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                                {
                                    i++;
                                    objjobPlanDetail.InsertRecord("WORK", b.ToString(), dr["Work"].ToString(), dr["Minute"].ToString(), ref trns);
                                }
                            }
                            //here we are inserting multiple employee 
                            objTaskEmp.DeleteRecord_By_RefTypeandRefid("WO", b.ToString(), ref trns);
                            foreach (string name in txtselectEmployee.Text.Split(';'))
                            {
                                int start_pos1 = name.LastIndexOf("/") + 1;
                                int last_pos1 = name.Length;
                                string id1 = name.Substring(start_pos1, last_pos1 - start_pos1);
                                string StrTo1 = "";
                                if (id1 != "")
                                {
                                    if (!StrTo1.Split(';').Contains(id1))
                                    {
                                        objTaskEmp.InsertRecord("WO", b.ToString(), id1, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        StrTo1 = StrTo1 + id1 + ";";
                                    }
                                }
                            }
                            if (Session["dtToolsList"] != null)
                            {
                                string Toolsid = string.Empty;
                                DataTable dtProduct = (DataTable)Session["dtToolsList"];
                                foreach (DataRow dr in dtProduct.Rows)
                                {
                                    if (dr["Tools_Id"].ToString() == "0")
                                    {
                                        Toolsid = dr["ProductCode"].ToString();
                                    }
                                    else
                                    {
                                        Toolsid = dr["Tools_Id"].ToString();
                                    }
                                    objProjecttools.InsertRecord(b.ToString(), Toolsid, dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), dr["IsToolsExists"].ToString(), "WORK", dr["Field2"].ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                        ObjWorkOrder.UpdateExtendedID(hdnValue.Value, b.ToString(), ref trns);
                        DisplayMessage("Record Extended Successfully");
                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        Reset();
                        FillGrid();
                        btnInquirySave.Enabled = true;
                        return;
                    }
                }
                // for update case
                ObjWorkOrder.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, txtorderNo.Text, objSysParam.getDateForInput(txtOrderDate.Text.Trim()).ToString(), ddlReftype.SelectedValue, txtRefNo.Text, StrCustomer, strHandledEmployee, DdlWorkType.SelectedValue, "", ddlStatus.SelectedValue, txtProbelm.Text, txtEngComments.Text, strSiteAddress, "0", strtaskTd, txtInTime.Text, txtOuttime.Text, hdnTicketid.Value, txtticketno.Text, jobPlanId, strContactPersonId, txtCustomerfeedback.Text, false.ToString(), DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ChkCourtesyCall.Checked.ToString(), txtWorkingHr.Text, ref trns);
                //here we insert record in detail table
                objjobPlanDetail.DeleteRecord_BY_RefTypeandRefId("WORK", hdnValue.Value, ref trns);
                if (Session["dtVisitTaskList"] != null)
                {
                    int i = 0;
                    foreach (DataRow dr in ((DataTable)Session["dtVisitTaskList"]).Rows)
                    {
                        i++;
                        objjobPlanDetail.InsertRecord("WORK", hdnValue.Value, dr["Work"].ToString(), dr["Minute"].ToString(), ref trns);
                    }
                }
                //here we are inserting multiple employee 
                objTaskEmp.DeleteRecord_By_RefTypeandRefid("WO", hdnValue.Value, ref trns);
                int start_pos, last_pos;
                string id, StrTo = "";
                foreach (string name in txtselectEmployee.Text.Split(';'))
                {
                    start_pos = name.LastIndexOf("/") + 1;
                    last_pos = name.Length;
                    id = name.Substring(start_pos, last_pos - start_pos);
                    if (id != "")
                    {
                        if (!StrTo.Split(';').Contains(id))
                        {
                            objTaskEmp.InsertRecord("WO", hdnValue.Value, id, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            StrTo = StrTo + id + ";";
                        }
                    }
                }
                foreach (ListItem li in listtaskEmployee.Items)
                {
                    if (li.Selected)
                    {
                        objTaskEmp.InsertRecord("WO", hdnValue.Value, li.Value, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //this code for insert Tools information
                //code start
                //first delete record by project id
                objProjecttools.DeleteRecord_ByReftypeandrefId("WORK", hdnValue.Value.ToString(), ref trns);
                if (Session["dtToolsList"] != null)
                {
                    string Toolsid = string.Empty;
                    DataTable dtProduct = (DataTable)Session["dtToolsList"];
                    foreach (DataRow dr in dtProduct.Rows)
                    {
                        if (dr["Tools_Id"].ToString() == "0")
                        {
                            Toolsid = dr["ProductCode"].ToString();
                        }
                        else
                        {
                            Toolsid = dr["Tools_Id"].ToString();
                        }
                        objProjecttools.InsertRecord(hdnValue.Value, Toolsid, dr["Unit_Id"].ToString(), dr["Quantity"].ToString(), dr["IsToolsExists"].ToString(), "WORK", dr["Field2"].ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                if (((Button)sender).ID.Trim() == "btnInquiryCloseandInvoice")
                {
                    SaveInvoice(hdnValue.Value, StrCustomer, ref trns);
                }
                //code end
                btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
                btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
                DisplayMessage("Record Updated", "green");
                btnInquirySave.Enabled = true;
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            //code end
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            FillGrid();
        }
        catch (Exception ex)
        {
            btnInquirySave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquirySave, "").ToString());
            btnInquiryCloseandInvoice.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnInquiryCloseandInvoice, "").ToString());
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));
            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            btnInquirySave.Enabled = true;
            return;
        }
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        FillGrid();
        if (ddlFieldName.SelectedItem.Value == "Work_Order_Date" || ddlFieldName.SelectedItem.Value == "Order_Date")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    txtValue.Text = objSysParam.getDateForInput(txtValueDate.Text).ToString();
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
                txtValue.Text = "";
                return;
            }
        }
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
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
            DataTable dtCustomInq = (DataTable)Session["dtCInquiry"];
            DataView view = new DataView(dtCustomInq, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //cmn.FillData((object)GvCustomerInquiry, view.ToTable(), "", "");
            GvWorkOrder.DataSource = view.ToTable();
            GvWorkOrder.DataBind();
            Session["dtCInquiry"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
        }
        //btnRefresh.Focus();
        //AllPageCode();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvCustomerInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtCInquiry"];
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
        Session["dtCInquiry"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvWorkOrder, dt, "", "");
        //AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvWorkOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCInquiry"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvWorkOrder, dt, "", "");
        //AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        div_inTime.Attributes.Add("Class", "col-md-3");
        div_outTime.Attributes.Add("Class", "col-md-3");
        div_workingHrs.Attributes.Add("Class", "col-md-3");
        div_call.Attributes["style"] = "display:block;";
        DataTable dt = ObjWorkOrder.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            ImageButton b = (ImageButton)sender;
            string objSenderID = b.ID;
            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                txtorderNo.Text = dt.Rows[0]["Work_Order_No"].ToString();
            }
            if (objSenderID == "btnEdit")
            {
                //here we are adding validation that we can not edit if work order is closed 
                if (dt.Rows[0]["Parent_Id"].ToString().Trim() != "")
                {
                    DisplayMessage("Work Visit Already Extended");
                    hdnExtendedID.Value = "0";
                    return;
                }
                if (dt.Rows[0]["Status"].ToString().Trim() == "Complete")
                {
                    DisplayMessage("Work order closed , you can not edit");
                    return;
                }
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                txtorderNo.Text = dt.Rows[0]["Work_Order_No"].ToString();
            }
            if (objSenderID == "btnExtended")
            {
                Lbl_Tab_New.Text = Resources.Attendance.New;
                if (dt.Rows[0]["Parent_Id"].ToString().Trim() != "")
                {
                    DisplayMessage("Work Visit Already Extended");
                    hdnExtendedID.Value = "0";
                    GvWorkOrder.Columns[4].Visible = false;
                    GvWorkOrder.Columns[5].Visible = false;
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                    hdnExtendedID.Value = dt.Rows[0]["Trans_Id"].ToString();
                    txtExtendID.Text = dt.Rows[0]["Work_Order_No"].ToString();
                    txtExtendID.Enabled = false;
                    txtorderNo.Text = GetDocumentNumber();
                }
            }
            ///if invoice create then change thee caption of create invoice 
            if (objDa.return_DataTable("select Inv_SalesInvoiceHeader.Post from Inv_SalesInvoiceHeader where SIFromTransType='W' and SIFromTransNo=" + e.CommandArgument.ToString() + "").Rows.Count > 0)
            {
                btnInquiryCloseandInvoice.Text = "Update Invoice";
            }
            txtOrderDate.Text = Convert.ToDateTime(dt.Rows[0]["Work_Order_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            ddlReftype.SelectedValue = dt.Rows[0]["Ref_Type"].ToString();
            hdnValue.Value = e.CommandArgument.ToString();
            ddlReftype_OnSelectedIndexChanged(null, null);
            if (dt.Rows[0]["CourtesyCall"].ToString().Trim() == "" || dt.Rows[0]["CourtesyCall"].ToString().Trim() == "False")
            {
                ChkCourtesyCall.Checked = false;
            }
            else
            {
                ChkCourtesyCall.Checked = true;
            }
            txtRefNo.Text = dt.Rows[0]["Ref_Id"].ToString();
            txtRefNo_OnTextChanged(null, null);
            Session["ContactID"] = null;
            if (dt.Rows[0]["Customer_Id"].ToString() != "0")
            {
                Session["ContactID"] = dt.Rows[0]["Customer_Id"].ToString();
                txtCustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
            }
            if (dt.Rows[0]["Field4"].ToString() != "0")
            {
                txtEContact.Text = dt.Rows[0]["ContactPersonName"].ToString() + "/" + dt.Rows[0]["Field4"].ToString();
            }
            if (dt.Rows[0]["Handled_Emp_Id"].ToString() != "0")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Handled_Emp_Id"].ToString());
                txtHandledEmp.Text = dt.Rows[0]["EmployeeName"].ToString() + "/" + Emp_Code;
            }
            //txtInTime.Text = dt.Rows[0]["Work_Type"].ToString();
            //txtOuttime.Text = dt.Rows[0]["Problems"].ToString();
            DdlWorkType.SelectedValue = dt.Rows[0]["Work_Type"].ToString();
            //ddlProblems.SelectedValue = dt.Rows[0]["Problems"].ToString();
            ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
            txtProbelm.Text = dt.Rows[0]["Remarks"].ToString();
            txtticketno.Text = dt.Rows[0]["field2"].ToString();
            hdnTicketid.Value = dt.Rows[0]["field1"].ToString();
            txtCustomerfeedback.Text = dt.Rows[0]["field5"].ToString();
            txtEngComments.Text = dt.Rows[0]["Engineer_Remarks"].ToString();
            if (dt.Rows[0]["Address_Id"].ToString() != "" && dt.Rows[0]["Address_Id"].ToString() != "0")
            {
                txtSiteAddress.Text = dt.Rows[0]["Address_Name"].ToString();
            }
            txtInTime.Text = dt.Rows[0]["Start_Time"].ToString();
            txtOuttime.Text = dt.Rows[0]["End_Time"].ToString();
            txtInTime_TextChanged(null, null);
            try
            {
                ddltask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
            }
            catch
            {
            }
            if (txtticketno.Text.Trim() != "")
            {
                //txtticketno_OnTextChanged(null, null);
            }
            //if job plan id is selected
            if (dt.Rows[0]["field3"].ToString() != "0" && dt.Rows[0]["field3"].ToString() != "")
            {
                txtjobPlanId.Text = objjobPlanHeader.GetRecord_By_TransId(dt.Rows[0]["Field3"].ToString()).Rows[0]["JobPlanId"].ToString() + "/" + dt.Rows[0]["Field3"].ToString();
                txtjobPlanName.Text = objjobPlanHeader.GetRecord_By_TransId(dt.Rows[0]["Field3"].ToString()).Rows[0]["JobPlanName"].ToString();
            }
            //for get work list
            //get record of plan list
            if (objjobPlanDetail.GetRecord_By_RefType_and_RefId("WORK", hdnValue.Value).Rows.Count > 0)
            {
                Session["dtVisitTaskList"] = objjobPlanDetail.GetRecord_By_RefType_and_RefId("WORK", hdnValue.Value).DefaultView.ToTable(true, "Trans_Id", "Work", "Minute");
                LoadTask();
            }
            else
            {
                Session["dtVisitTaskList"] = null;
            }
            //this code for get Tools information
            //code start
            DataTable dtTools = objProjecttools.GetRecordByProjectId(e.CommandArgument.ToString());
            try
            {
                dtTools = new DataView(dtTools, "Field1='WORK'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtTools.Rows.Count > 0)
            {
                dtTools = dtTools.DefaultView.ToTable(true, "Trans_Id", "Tools_Id", "ProductCode", "EProductName", "Unit_Id", "Unit_Name", "Quantity", "IsToolsExists", "Field2");
                Session["dtToolsList"] = dtTools;
                LoadToolsRecord();
            }
            //here we are gettin list of visit employee
            DataTable dtvisitEmpList = objTaskEmp.GetRecordBy_RefType_and_RefId("WO", e.CommandArgument.ToString());
            DataTable dtCon = new DataTable();
            if (dtvisitEmpList.Rows.Count > 0)
            {
                txtselectEmployee.Text = "";
            }
            for (int i = 0; i < dtvisitEmpList.Rows.Count; i++)
            {
                dtCon = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), dtvisitEmpList.Rows[i]["Employee_id"].ToString());
                txtselectEmployee.Text += dtCon.Rows[0]["Emp_Name"].ToString() + "/(" + dtCon.Rows[0]["designation"].ToString() + ")" + "/" + dtCon.Rows[0]["Emp_Id"].ToString() + ";";
            }
            TabContainer2.ActiveTabIndex = 0;
        }
    }
    protected void btnEdit_Command(string objSenderID, string CommandArgument)
    {
        ddlLoc.Enabled = false;
        btnShowMaps.Enabled = false;
        div_inTime.Attributes.Add("Class", "col-md-3");
        div_outTime.Attributes.Add("Class", "col-md-3");
        div_workingHrs.Attributes.Add("Class", "col-md-3");
        div_call.Attributes["style"] = "display:block;";
        DataTable dt = ObjWorkOrder.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, CommandArgument);
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Latitude"].ToString() != "")
            {
                Session["Lati"] = dt.Rows[0]["Latitude"].ToString();
            }
            if (dt.Rows[0]["Longitude"].ToString() != "")
            {
                Session["Long"] = dt.Rows[0]["Longitude"].ToString();
            }
            if ((dt.Rows[0]["Latitude"].ToString() != "" || dt.Rows[0]["Longitude"].ToString() != "") && (dt.Rows[0]["Latitude"].ToString() != "0" || dt.Rows[0]["Longitude"].ToString() != "0"))
            {
                btnShowMaps.Enabled = true;
            }
            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                btnInquirySave.Visible = false;
                txtorderNo.Text = dt.Rows[0]["Work_Order_No"].ToString();
            }
            if (objSenderID == "btnEdit")
            {
                btnInquirySave.Visible = true;
                //here we are adding validation that we can not edit if work order is closed 
                if (dt.Rows[0]["Parent_Id"].ToString().Trim() != "")
                {
                    DisplayMessage("Work Visit Already Extended");
                    hdnExtendedID.Value = "0";
                    return;
                }
                if (dt.Rows[0]["Status"].ToString().Trim() == "Complete")
                {
                    DisplayMessage("Work order closed , you can not edit");
                    return;
                }
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                txtorderNo.Text = dt.Rows[0]["Work_Order_No"].ToString();
            }
            if (objSenderID == "btnExtended")
            {
                if (dt.Rows[0]["Status"].ToString().Trim() == "Complete")
                {
                    DisplayMessage("Cant Extend Closed Work Order");
                    return;
                }
                Lbl_Tab_New.Text = "Extended";
                btnInquirySave.Visible = true;
                if (dt.Rows[0]["Parent_Id"].ToString().Trim() != "")
                {
                    DisplayMessage("Work Visit Already Extended");
                    hdnExtendedID.Value = "0";
                    GvWorkOrder.Columns[4].Visible = false;
                    GvWorkOrder.Columns[5].Visible = false;
                    return;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                    hdnExtendedID.Value = dt.Rows[0]["Trans_Id"].ToString();
                    txtExtendID.Text = dt.Rows[0]["Work_Order_No"].ToString();
                    txtExtendID.Enabled = false;
                    txtorderNo.Text = GetDocumentNumber();
                }
            }
            if (objDa.return_DataTable("select Inv_SalesInvoiceHeader.Post from Inv_SalesInvoiceHeader where SIFromTransType='W' and SIFromTransNo=" + CommandArgument + "").Rows.Count > 0)
            {
                btnInquiryCloseandInvoice.Text = "Update Invoice";
            }
            txtOrderDate.Text = Convert.ToDateTime(dt.Rows[0]["Work_Order_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            ddlReftype.SelectedValue = dt.Rows[0]["Ref_Type"].ToString();
            hdnValue.Value = CommandArgument;
            ddlReftype_OnSelectedIndexChanged(null, null);
            if (dt.Rows[0]["CourtesyCall"].ToString().Trim() == "" || dt.Rows[0]["CourtesyCall"].ToString().Trim() == "False")
            {
                ChkCourtesyCall.Checked = false;
            }
            else
            {
                ChkCourtesyCall.Checked = true;
            }

            Session["ContactID"] = null;
            if (dt.Rows[0]["Customer_Id"].ToString() != "0")
            {
                Session["ContactID"] = dt.Rows[0]["Customer_Id"].ToString();
                txtCustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
            }


            if (ddlReftype.SelectedValue == "Campaign")
            {
                txtRefNo.Text = new Campaign(Session["DBConnection"].ToString()).GetCampaignNameByID(dt.Rows[0]["Ref_Id"].ToString());
                txtRefNo_OnTextChanged(null, null);
            }
            else
            {
                txtRefNo.Text = dt.Rows[0]["Ref_Id"].ToString();
                txtRefNo_OnTextChanged(null, null);
            }
            txtActualVisitDate.Text = GetDate(dt.Rows[0]["ActualVisitDate"].ToString());

            if (dt.Rows[0]["Field4"].ToString() != "0")
            {
                txtEContact.Text = dt.Rows[0]["ContactPersonName"].ToString() + "/" + dt.Rows[0]["Field4"].ToString();
            }
            if (dt.Rows[0]["Handled_Emp_Id"].ToString() != "0")
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Handled_Emp_Id"].ToString());
                txtHandledEmp.Text = dt.Rows[0]["EmployeeName"].ToString() + "/" + Emp_Code;
            }
            //txtInTime.Text = dt.Rows[0]["Work_Type"].ToString();
            //txtOuttime.Text = dt.Rows[0]["Problems"].ToString();
            DdlWorkType.SelectedValue = dt.Rows[0]["Work_Type"].ToString();
            //ddlProblems.SelectedValue = dt.Rows[0]["Problems"].ToString();
            ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
            txtProbelm.Text = dt.Rows[0]["Remarks"].ToString();
            txtticketno.Text = dt.Rows[0]["field2"].ToString();
            hdnTicketid.Value = dt.Rows[0]["field1"].ToString();
            txtCustomerfeedback.Text = dt.Rows[0]["field5"].ToString();
            txtEngComments.Text = dt.Rows[0]["Engineer_Remarks"].ToString();
            if (dt.Rows[0]["Address_Id"].ToString() != "" && dt.Rows[0]["Address_Id"].ToString() != "0")
            {
                txtSiteAddress.Text = dt.Rows[0]["Address_Name"].ToString();
            }
            txtInTime.Text = dt.Rows[0]["Start_Time"].ToString();
            txtOuttime.Text = dt.Rows[0]["End_Time"].ToString();
            txtInTime_TextChanged(null, null);
            try
            {
                ddltask.SelectedValue = dt.Rows[0]["Task_Id"].ToString();
            }
            catch
            {
            }
            if (txtticketno.Text.Trim() != "")
            {
                //txtticketno_OnTextChanged(null, null);
            }
            //if job plan id is selected
            if (dt.Rows[0]["field3"].ToString() != "0" && dt.Rows[0]["field3"].ToString() != "")
            {
                txtjobPlanId.Text = objjobPlanHeader.GetRecord_By_TransId(dt.Rows[0]["Field3"].ToString()).Rows[0]["JobPlanId"].ToString() + "/" + dt.Rows[0]["Field3"].ToString();
                txtjobPlanName.Text = objjobPlanHeader.GetRecord_By_TransId(dt.Rows[0]["Field3"].ToString()).Rows[0]["JobPlanName"].ToString();
            }
            //for get work list
            //get record of plan list
            if (objjobPlanDetail.GetRecord_By_RefType_and_RefId("WORK", hdnValue.Value).Rows.Count > 0)
            {
                Session["dtVisitTaskList"] = objjobPlanDetail.GetRecord_By_RefType_and_RefId("WORK", hdnValue.Value).DefaultView.ToTable(true, "Trans_Id", "Work", "Minute");
                LoadTask();
            }
            else
            {
                Session["dtVisitTaskList"] = null;
            }
            //this code for get Tools information
            //code start
            DataTable dtTools = objProjecttools.GetRecordByProjectId(CommandArgument);
            try
            {
                dtTools = new DataView(dtTools, "Field1='WORK'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dtTools.Rows.Count > 0)
            {
                dtTools = dtTools.DefaultView.ToTable(true, "Trans_Id", "Tools_Id", "ProductCode", "EProductName", "Unit_Id", "Unit_Name", "Quantity", "IsToolsExists", "Field2");
                Session["dtToolsList"] = dtTools;
                LoadToolsRecord();
            }
            //here we are gettin list of visit employee
            DataTable dtvisitEmpList = objTaskEmp.GetRecordBy_RefType_and_RefId("WO", CommandArgument);
            DataTable dtCon = new DataTable();
            if (dtvisitEmpList.Rows.Count > 0)
            {
                txtselectEmployee.Text = "";
            }
            for (int i = 0; i < dtvisitEmpList.Rows.Count; i++)
            {
                dtCon = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), dtvisitEmpList.Rows[i]["Employee_id"].ToString());
                txtselectEmployee.Text += dtCon.Rows[0]["Emp_Name"].ToString() + "/(" + dtCon.Rows[0]["designation"].ToString() + ")" + "/" + dtCon.Rows[0]["Emp_Id"].ToString() + ";";
            }
            TabContainer2.ActiveTabIndex = 0;
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        // GridViewRow gvRow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        if (ddlPosted.SelectedValue == "Complete")
        {
            DisplayMessage("Work order closed , you can not delete");
            return;
        }
        if (ddlPosted.SelectedValue == "Extended")
        {
            DisplayMessage("Work order has been Extended , you can not delete");
            return;
        }
        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString();
        b = ObjWorkOrder.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnValue.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
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
    protected void IbtnDelete_Command(string transId)
    {
        if (ddlPosted.SelectedValue == "Complete")
        {
            DisplayMessage("Work order closed , you can not delete");
            return;
        }
        if (ddlPosted.SelectedValue == "Extended")
        {
            DisplayMessage("Work order has been Extended , you can not delete");
            return;
        }
        int b = 0;
        hdnValue.Value = transId;
        b = ObjWorkOrder.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnValue.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillBinGrid();
        FillGrid();
        Reset();
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        Session["ContactID"] = null;
        string Parameter_Id = string.Empty;
        string ParameterValue = string.Empty;
        if (txtCustomer.Text != "")
        {
            string contactId = "";
            contactId = objContact.GetContactIdByContactName(txtCustomer.Text.Split('/')[0].ToString());
            if (contactId == "")
            {
                DisplayMessage("Select Customer Name");
                txtCustomer.Text = "";
                txtCustomer.Focus();
            }
            else
            {
                string strCustomerId = contactId;
                Session["ContactID"] = strCustomerId;
                DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", strCustomerId);
                if (dtAddress != null && dtAddress.Rows.Count > 0)
                {
                    if (txtSiteAddress.Text.Trim() == "")
                    {
                        txtSiteAddress.Text = dtAddress.Rows[0]["Address_Name"].ToString();
                    }
                }
                else
                {
                    txtSiteAddress.Text = "";
                }
            }
        }
        else
        {
            DisplayMessage("Select Customer Name");
            txtCustomer.Focus();
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        string id = string.Empty;
        DataTable dt = new DataTable();
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            id = HttpContext.Current.Session["ContactID"].ToString();
        }
        catch
        {
            id = "0";
        }
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
                filterlist[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
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
    protected void txtHandledEmp_TextChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        if (txtHandledEmp.Text != "")
        {
            try
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                strEmpId = Emp_ID;
                ddlStatus.Focus();
            }
            catch
            {
                strEmpId = "0";
            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtHandledEmp.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtHandledEmp);
                return;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        using (DataTable dtCon = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetCustomerRecAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText))
        {
            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_id"].ToString();
                }
            }
            return filterlist;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactPerson(string prefixText, int count, string contextKey)
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
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            dtcon = null;
            return filterlistcon;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefTo(string prefixText, int count, string contextKey)
    {
        //EmployeeMaster ObjEmp = new EmployeeMaster();
        DataTable dtCon = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        //dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and LOcation_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVisit(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["ContactID"] != null)
            {
                DataTable dtCon = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString()).GetRecordByCustomerID(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["ContactID"].ToString());
                string[] filterlist = new string[dtCon.Rows.Count];
                if (dtCon.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCon.Rows.Count; i++)
                    {
                        filterlist[i] = dtCon.Rows[i]["Work_Order_No"].ToString();
                    }
                }
                return filterlist;
            }
            else
            {
                string[] filterlist = { };
                return filterlist;
            }
        }
        catch
        {
            string[] filterlist = { };
            return filterlist;
        }
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
        ddlLoc.Enabled = true;
        ddlLoc.SelectedValue = Session["LocId"].ToString();
        div_inTime.Attributes.Add("Class", "col-md-4");
        div_outTime.Attributes.Add("Class", "col-md-4");
        div_workingHrs.Attributes.Add("Class", "col-md-4");
        div_call.Attributes["style"] = "display:none;";
        ChkCourtesyCall.Checked = false;
        txtorderNo.Text = "";
        txtOrderDate.Text = "";
        ddlReftype.SelectedIndex = 0;
        ddlReftype_OnSelectedIndexChanged(null, null);
        AutoCompleteExtenderProject.Enabled = false;
        AutoCompleteExtenderContract.Enabled = false;
        txtRefNo.Text = "";
        txtCustomer.Text = "";
        txtHandledEmp.Text = "";
        ddlStatus.SelectedIndex = 0;
        txtProbelm.Text = "";
        hdnTicketid.Value = "0";
        txtWorkingHr.Text = "";
        txtActualVisitDate.Text = "";
        //txtticketno.Text = "";
        trTicketDetail.Visible = false;
        //lnkticketdesc.Visible = false;
        txtOrderDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        ////Editor1.Content = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtorderNo.Text = "";
        txtorderNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtorderNo.Text;
        hdnValue.Value = "";
        try
        {
            DataTable dtEmployeeDtl = ObjUser.GetUserMasterForUserName(Session["UserId"].ToString(), Session["CompId"].ToString());
            string StrEmpID = dtEmployeeDtl.Rows[0]["Emp_Id"].ToString();
            txtselectEmployee.Text = dtEmployeeDtl.Rows[0]["EmpName"].ToString() + "/(" + dtEmployeeDtl.Rows[0]["Emp_Designation"].ToString() + ")" + "/" + dtEmployeeDtl.Rows[0]["Emp_Id"].ToString() + ";";
            txtHandledEmp.Text= dtEmployeeDtl.Rows[0]["EmpName"].ToString() + "/" + dtEmployeeDtl.Rows[0]["Emp_Code"].ToString();
        }
        catch
        {
            txtselectEmployee.Text = "";
            txtHandledEmp.Text = "";
        }
        ViewState["Select"] = null;
        Session["dtVisitTaskList"] = null;
        LoadTask();
        txtjobPlanId.Text = "";
        txtjobPlanName.Text = "";
        txtOrderDate.Focus();
        Session["dtToolsList"] = null;
        LoadToolsRecord();
        TabContainer2.ActiveTabIndex = 0;
        //FillEmployee();
        Session["ContactID"] = null;
        txtEContact.Text = "";
        txtCustomerfeedback.Text = "";
        txtInTime.Text = "";
        txtOuttime.Text = "";
        //ResetVisitPanel();
        DdlWorkType.SelectedIndex = 0;
        //Session["DtVisit"] = null;
        //cmn.FillData((GridView)gvVisitMaster, null, "", "");
        txtSiteAddress.Text = "";
        txtEngComments.Text = "";
        txtInTime.Text = "";
        txtOuttime.Text = "";
        btnInquiryCloseandInvoice.Text = "Create Invoice";
        btnShowMaps.Enabled = false;
    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    public void FillGrid()
    {
        DataTable dt = new DataTable();
        if (hdnEmpList.Value == "")
        {
            fillUser();
        }

        dt = ObjWorkOrder.GetAllTrueRecordByHandledEmpId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, hdnEmpList.Value);
        if (ddlPosted.SelectedIndex == 0 || ddlPosted.SelectedIndex == 1 || ddlPosted.SelectedIndex == 2)
        {
            dt = new DataView(dt, "Status='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //cmn.FillData((object)GvCustomerInquiry, dt, "", "");
        GvWorkOrder.DataSource = dt;
        GvWorkOrder.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtCInquiry"] = dt;
        Session["dtAllCInquiry"] = dt;
        dt.Dispose();
    }
    public void FillBinGrid()
    {
        DataTable dt = ObjWorkOrder.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtCInquiryBin"] = dt;
        //AllPageCode();
    }
    protected void GvCustomerInquiryBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Select"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjWorkOrder.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtCInquiryBin"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");
        lblSelectedRecord.Text = "";
        //AllPageCode();
    }
    protected void GvCustomerInquiryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerInquiryBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCInquiryBin"];
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
        if (ddlFieldNameBin.SelectedItem.Value == "Work_Order_Date" || ddlFieldNameBin.SelectedItem.Value == "Order_Date")
        {
            if (txtValueBinDate.Text != "")
            {
                try
                {
                    objSysParam.getDateForInput(txtValueBinDate.Text);
                    txtValueBin.Text = objSysParam.getDateForInput(txtValueBinDate.Text).ToString();
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
            DataTable dtCust = (DataTable)Session["dtCInquiryBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtCInquiryBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiryBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            //if (view.ToTable().Rows.Count == 0)
            //{
            //    FillBinGrid();
            //}
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnRefreshBin);
        }
        //AllPageCode();
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        //FillGrid();
        FillBinGrid();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        txtValueBin.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        DataTable dt = ObjWorkOrder.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (GvCustomerInquiryBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = ObjWorkOrder.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dtCustInq = (DataTable)Session["dtCInquiryBin"];
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
            DataTable dtCustInqiury = (DataTable)Session["dtCInquiryBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiryBin, dtCustInqiury, "", "");
            ViewState["Select"] = null;
        }
        //AllPageCode();
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
    protected string GetDocumentNumber()
    {
        string s = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "323", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    public string GetEmpName()
    {
        string EmpName = string.Empty;
        DataTable dtEmployee = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
        try
        {
            EmpName = dtEmployee.Rows[0]["Emp_Name"].ToString() + "/" + dtEmployee.Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
        }
        return EmpName;
    }
    #region Date Search
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedItem.Value == "Work_Order_Date" || ddlFieldName.SelectedItem.Value == "Order_Date")
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
        ddlFieldName.Focus();
    }
    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Work_Order_Date" || ddlFieldNameBin.SelectedItem.Value == "Order_Date")
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
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTicketNo(string prefixText, int count, string contextKey)
    {
        SM_Ticket_Master objTickeMaster = new SM_Ticket_Master(HttpContext.Current.Session["DBConnection"].ToString());
        SM_TicketEmployee objTicketemployee = new SM_TicketEmployee(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtTicket = objTickeMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            dtTicket = new DataView(dtTicket, "Status='Open' and IsActive='True' and Ticket_No like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] filterlist = new string[dtTicket.Rows.Count];
        if (dtTicket.Rows.Count > 0)
        {
            for (int i = 0; i < dtTicket.Rows.Count; i++)
            {
                filterlist[i] = dtTicket.Rows[i]["Ticket_No"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCampaign(string prefixText, int count, string contextKey)
    {
        Campaign objCampaign = new Campaign(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtTicket = objCampaign.GetAllCampaignActiveData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] filterlist = new string[dtTicket.Rows.Count];
        if (dtTicket.Rows.Count > 0)
        {
            for (int i = 0; i < dtTicket.Rows.Count; i++)
            {
                filterlist[i] = dtTicket.Rows[i]["Campaign_name"].ToString() + "/" + dtTicket.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    #region JobPlan
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListJobPlan(string prefixText, int count, string contextKey)
    {
        SM_JobPlan_Header objheader = new SM_JobPlan_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtTicket = objheader.GetAllRecord();
        try
        {
            dtTicket = new DataView(dtTicket, "JobPlanId like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string[] filterlist = new string[dtTicket.Rows.Count];
        if (dtTicket.Rows.Count > 0)
        {
            for (int i = 0; i < dtTicket.Rows.Count; i++)
            {
                filterlist[i] = dtTicket.Rows[i]["JobPlanId"].ToString() + "/" + dtTicket.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    protected void txtjobPlanId_OnTextChanged(object sender, EventArgs e)
    {
        if (txtjobPlanId.Text != "")
        {
            try
            {
                DataTable dt = objjobPlanHeader.GetRecord_By_TransId(txtjobPlanId.Text.Split('/')[1].ToString());
                if (dt.Rows.Count > 0)
                {
                    txtjobPlanName.Text = dt.Rows[0]["JobPlanName"].ToString();
                    if (objjobPlanDetail.GetRecord_By_RefType_and_RefId("JOB", txtjobPlanId.Text.Split('/')[1].ToString()).Rows.Count > 0)
                    {
                        Session["dtVisitTaskList"] = objjobPlanDetail.GetRecord_By_RefType_and_RefId("JOB", txtjobPlanId.Text.Split('/')[1].ToString()).DefaultView.ToTable(true, "Trans_Id", "Work", "Minute");
                        LoadTask();
                    }
                    else
                    {
                        Session["dtVisitTaskList"] = null;
                    }
                }
            }
            catch
            {
                DisplayMessage("Job plan not found");
                txtjobPlanId.Text = "";
                return;
            }
        }
        else
        {
            txtjobPlanName.Text = "";
        }
    }
    public void LoadTask()
    {
        DataTable dt = new DataTable();
        if (Session["dtVisitTaskList"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["dtVisitTaskList"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, dt, "", "");
            }
            else
            {
                DataTable contacts = new DataTable();
                contacts.Columns.Add("Trans_Id", typeof(int));
                contacts.Columns.Add("Work", typeof(string));
                contacts.Columns.Add("Minute", typeof(string));
                contacts.Rows.Add(contacts.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
                int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
                gvVisitTask.Rows[0].Cells.Clear();
                gvVisitTask.Rows[0].Cells.Add(new TableCell());
                gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvVisitTask.Rows[0].Visible = false;
            }
        }
        else
        {
            DataTable contacts = new DataTable();
            contacts.Columns.Add("Trans_Id", typeof(int));
            contacts.Columns.Add("Work", typeof(string));
            contacts.Columns.Add("Minute", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvVisitTask, contacts, "", "");
            int TotalColumns = gvVisitTask.Rows[0].Cells.Count;
            gvVisitTask.Rows[0].Cells.Clear();
            gvVisitTask.Rows[0].Cells.Add(new TableCell());
            gvVisitTask.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvVisitTask.Rows[0].Visible = false;
        }
    }
    protected void gvVisitTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text.Trim() == "")
            {
                DisplayMessage("Enter Description");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Focus();
                return;
            }
            if (Session["dtVisitTaskList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Work", typeof(string));
                dt.Columns.Add("Minute", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text.Trim();
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtMinutes")).Text;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtVisitTaskList"];
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
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Text.Trim();
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtMinutes")).Text;
                dt.Rows.Add(dr);
            }
            Session["dtVisitTaskList"] = dt;
            gvVisitTask.EditIndex = -1;
            LoadTask();
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtVisitTaskList"] != null)
            {
                dt = (DataTable)Session["dtVisitTaskList"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtVisitTaskList"] = dt;
            }
            gvVisitTask.EditIndex = -1;
            LoadTask();
        }
        ((TextBox)gvVisitTask.FooterRow.FindControl("txtFooterTask")).Focus();
    }
    protected void gvVisitTask_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVisitTask.EditIndex = e.NewEditIndex;
        LoadTask();
    }
    protected void gvVisitTask_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVisitTask.EditIndex = -1;
        LoadTask();
    }
    protected void gvVisitTask_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtVisitTaskList"];
        GridViewRow row = gvVisitTask.Rows[e.RowIndex];
        dt.Rows[row.DataItemIndex]["Work"] = ((TextBox)row.FindControl("txteditTask")).Text;
        dt.Rows[row.DataItemIndex]["Minute"] = ((TextBox)row.FindControl("txEdittMinutes")).Text;
        Session["dtVisitTaskList"] = dt;
        gvVisitTask.EditIndex = -1;
        LoadTask();
    }
    protected void gvVisitTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion
    #region addTools
    protected void txtToolsERelatedProduct_OnTextChanged(object sender, EventArgs e)
    {
        if (Session["dtToolsList"] != null)
        {
            DataTable DtProduct = (DataTable)Session["dtToolsList"];
            try
            {
                DtProduct = new DataView(DtProduct, "EProductName='" + ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (DtProduct.Rows.Count > 0)
            {
                DisplayMessage("Tools Name is already exists");
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = "";
                ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = "";
                ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Focus();
                return;
            }
        }
        DataTable dt = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        try
        {
            dt = new DataView(dt, "EProductName='" + ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue = dt.Rows[0]["UnitId"].ToString();
            ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text = dt.Rows[0]["ProductCode"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Focus();
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
                DisplayMessage("tools id is already exists");
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
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue = dt.Rows[0]["UnitId"].ToString();
            ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value = dt.Rows[0]["ProductId"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text = dt.Rows[0]["EProductName"].ToString();
            ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Focus();
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
                contacts.Columns.Add("Tools_Id", typeof(string));
                contacts.Columns.Add("ProductCode", typeof(string));
                contacts.Columns.Add("EProductName", typeof(string));
                contacts.Columns.Add("Unit_Id", typeof(int));
                contacts.Columns.Add("Unit_Name", typeof(string));
                contacts.Columns.Add("Quantity", typeof(string));
                contacts.Columns.Add("IsToolsExists", typeof(bool));
                contacts.Columns.Add("Field2", typeof(string));
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
            contacts.Columns.Add("Tools_Id", typeof(string));
            contacts.Columns.Add("ProductCode", typeof(string));
            contacts.Columns.Add("EProductName", typeof(string));
            contacts.Columns.Add("Unit_Id", typeof(int));
            contacts.Columns.Add("Unit_Name", typeof(string));
            contacts.Columns.Add("Quantity", typeof(string));
            contacts.Columns.Add("IsToolsExists", typeof(bool));
            contacts.Columns.Add("Field2", typeof(string));
            contacts.Rows.Add(contacts.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvTools, contacts, "", "");
            int TotalColumns = gvTools.Rows[0].Cells.Count;
            gvTools.Rows[0].Cells.Clear();
            gvTools.Rows[0].Cells.Add(new TableCell());
            gvTools.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvTools.Rows[0].Visible = false;
        }
        FillUnit();
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
            if (((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Text == "")
            {
                DisplayMessage("Enter Quantity");
                ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Focus();
                return;
            }
            if (((TextBox)gvTools.FooterRow.FindControl("txtUnitPrice")).Text == "")
            {
                ((TextBox)gvTools.FooterRow.FindControl("txtUnitPrice")).Text = "0";
            }
            bool isToolsExists = false;
            if (Session["dtToolsList"] == null)
            {
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Tools_Id", typeof(string));
                dt.Columns.Add("ProductCode", typeof(string));
                dt.Columns.Add("EProductName", typeof(string));
                dt.Columns.Add("Unit_Id", typeof(int));
                dt.Columns.Add("Unit_Name", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("IsToolsExists", typeof(bool));
                dt.Columns.Add("Field2", typeof(string));
                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value;
                dr[2] = ((TextBox)gvTools.FooterRow.FindControl("txtProductCode")).Text;
                dr[3] = ((TextBox)gvTools.FooterRow.FindControl("txtERelatedProduct")).Text;
                dr[4] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue;
                dr[5] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                dr[6] = ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Text;
                dr[8] = ((TextBox)gvTools.FooterRow.FindControl("txtUnitPrice")).Text;
                if (((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value == "0")
                {
                    isToolsExists = false;
                }
                else
                {
                    isToolsExists = true;
                }
                dr[7] = isToolsExists;
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
                dr[4] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedValue;
                dr[5] = ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).SelectedItem.Text;
                dr[6] = ((TextBox)gvTools.FooterRow.FindControl("txtquantity")).Text;
                dr[8] = ((TextBox)gvTools.FooterRow.FindControl("txtUnitPrice")).Text;
                if (((HiddenField)gvTools.FooterRow.FindControl("hdnProductId")).Value == "0")
                {
                    isToolsExists = false;
                }
                else
                {
                    isToolsExists = true;
                }
                dr[7] = isToolsExists;
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
    public void FillUnit()
    {
        try
        {
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataSource = objUnitMaster.GetUnitMaster(Session["CompId"].ToString());
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataTextField = "Unit_Name";
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataValueField = "Unit_Id";
            ((DropDownList)gvTools.FooterRow.FindControl("ddlunit")).DataBind();
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRelatedProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
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
        DataTable dt = new DataTable();
        dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    #endregion
    #region RefNoAutoCompleteandTextChangeEvent
    protected void ddlReftype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //FillEmployee();
        txtCustomer.Enabled = true;
        trtasklist.Visible = false;
        AutoCompleteExtenderProject.Enabled = false;
        AutoCompleteExtenderContract.Enabled = false;
        AutoCompleteExtenderInvoiceNo.Enabled = false;
        AutoCompleteExtenderCampaign.Enabled = false;
        txtRefNo.Text = "";
        if (ddlReftype.SelectedIndex == 1)
        {
            AutoCompleteExtenderProject.Enabled = true;
        }
        if (ddlReftype.SelectedIndex == 2)
        {
            AutoCompleteExtenderContract.Enabled = true;
        }
        if (ddlReftype.SelectedIndex == 3)
        {
            AutoCompleteExtenderInvoiceNo.Enabled = true;
        }
        if (ddlReftype.SelectedIndex == 4)
        {
            AutoCompleteExtenderTicket.Enabled = true;
        }
        if (ddlReftype.SelectedIndex == 5)
        {
            AutoCompleteExtenderCampaign.Enabled = true;
        }
    }
    protected void txtRefNo_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (txtRefNo.Text != "")
        {
            if (ddlReftype.SelectedIndex == 1 || ddlReftype.SelectedIndex == 2 || ddlReftype.SelectedIndex == 3 || ddlReftype.SelectedIndex == 4 || ddlReftype.SelectedIndex == 5)
            {
                //for project
                if (ddlReftype.SelectedIndex == 1)
                {
                    trTicketDetail.Visible = false;
                    trtasklist.Visible = true;
                    dt = objProjctMaster.GetAllRecordProjectMasteer();
                    dt = new DataView(dt, "Field4='" + HttpContext.Current.Session["CompId"].ToString() + "' and Field5='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Field6='" + HttpContext.Current.Session["LocId"].ToString() + "' and Field7='" + txtRefNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtCustomer.Enabled = false;
                        objPageCmn.FillData((object)ddltask, null, "Subject", "Task_Id");
                        DataTable dtTask = objProjectTask.GetDataProjectId(dt.Rows[0]["project_Id"].ToString());
                        objPageCmn.FillData((object)ddltask, dtTask, "Subject", "Task_Id");
                        DataTable dtprojectTeam = objProjectTeam.GetRecordByProjectId("", dt.Rows[0]["project_Id"].ToString());
                        //if (dtprojectTeam != null)
                        //{
                        //    objPageCmn.FillData((object)listtaskEmployee, dtprojectTeam, "Emp_Name", "Emp_Id");
                        //}
                        //else
                        //{
                        //    listtaskEmployee.Items.Clear();
                        //}
                        if (dtprojectTeam != null)
                        {
                            txtselectEmployee.Text = "";
                            //cmn.FillData((object)listtaskEmployee, dtprojectTeam, "Emp_Name", "Emp_Id");
                            foreach (DataRow dr in dtprojectTeam.Rows)
                            {
                                txtselectEmployee.Text += dr["Emp_Name"].ToString() + "/(" + dr["Designation"].ToString() + ")/" + dr["Emp_Id"].ToString() + ";";
                            }
                        }
                        else
                        {
                            txtselectEmployee.Text = "";
                        }
                        //getting customer detail 
                        DataTable dtProject = objProjctMaster.GetRecordByProjectId(dt.Rows[0]["project_Id"].ToString());
                        txtCustomer.Text = dtProject.Rows[0]["Name"].ToString() + "/" + dtProject.Rows[0]["Customer_Id"].ToString();
                        txtCustomer_TextChanged(null, null);
                    }
                }
                //for contract
                if (ddlReftype.SelectedIndex == 2)
                {
                    trTicketDetail.Visible = false;
                    txtCustomer.Enabled = false;
                    dt = objContractMaster.GetAllRecordByContractNo(txtRefNo.Text);
                    if (hdnHavePermission.Value == "false")
                    {
                        //DisplayMessage("You dont have Permission To use this Work Visit against this contract");
                        //txtRefNo.Text = "";
                        //txtRefNo.Focus();
                        //return;
                        DataTable dt_contractDetailData = new SM_Contract_Detail(HttpContext.Current.Session["DBConnection"].ToString()).GetAllDetailDataByContractID(dt.Rows[0]["Trans_Id"].ToString());
                        if (dt_contractDetailData.Rows.Count > 0)
                        {
                            dt_contractDetailData = new DataView(dt_contractDetailData, "(Schedule_Date <='" + txtOrderDate.Text + "' and Invoice_Id ='0') or (Schedule_Date <='" + txtOrderDate.Text + "' and Invoice_Id <>'0'  and (DueAmt > 0) )", "", DataViewRowState.CurrentRows).ToTable();
                            if (dt_contractDetailData.Rows.Count > 0)
                            {
                                DisplayMessage("Cant Select This Contract as It does not contains invoice or it consist some due balance");
                                txtRefNo.Text = "";
                                txtCustomer.Text = "";
                                txtSiteAddress.Text = "";
                                txtEContact.Text = "";
                                return;
                            }
                        }
                        else
                        {
                            DisplayMessage("Please Enter Contract Payment Details");
                            txtRefNo.Text = "";
                            txtCustomer.Text = "";
                            txtSiteAddress.Text = "";
                            txtEContact.Text = "";
                            return;
                        }
                    }
                    dt = new DataView(dt, "Contract_No='" + txtRefNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtCustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "/" + dt.Rows[0]["Customer_Id"].ToString();
                        txtCustomer_TextChanged(null, null);
                    }
                }
                if (ddlReftype.SelectedIndex == 3)
                {
                    trTicketDetail.Visible = false;
                    txtCustomer.Enabled = false;
                    bool data = ObjSalesInvoiceheader.validateByInvoiceNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), ddlLoc.SelectedValue, HttpContext.Current.Session["ContactId"].ToString(), txtRefNo.Text.Trim());
                    if (!data)
                    {
                        DisplayMessage("Please Select from suggestions");
                        txtRefNo.Text = "";
                    }
                    return;

                }
                if (ddlReftype.SelectedIndex == 5)
                {
                    string strCampaignId = string.Empty;
                    int start_pos = txtRefNo.Text.LastIndexOf("/") + 1;
                    int last_pos = txtRefNo.Text.Length;
                    string id = txtRefNo.Text.Substring(start_pos, last_pos - start_pos);
                    if (start_pos != 0)
                    {
                        int Last_pos_name = txtRefNo.Text.LastIndexOf("/");
                        string name = txtRefNo.Text.Substring(0, Last_pos_name - 0);
                        DataTable dtCampaign = new DataTable();
                        dt = new Campaign(HttpContext.Current.Session["DBConnection"].ToString()).GetCampaignByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), id);
                        dt = new DataView(dt, "Campaign_name='" + name + "' and Trans_Id=" + id + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }
                if (ddlReftype.SelectedIndex == 4)
                {
                    dt = objticketmaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    try
                    {
                        dt = new DataView(dt, "Ticket_No='" + txtRefNo.Text + "' and Status='Open' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dt.Rows.Count > 0)
                    {
                        txtticketno.Text = txtRefNo.Text;
                        txtCustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "/" + dt.Rows[0]["Customer_Name"].ToString();
                        txtCustomer_TextChanged(null, null);
                        trTicketDetail.Visible = true;
                        //   lnkticketdesc.Visible = true;
                        hdnTicketid.Value = dt.Rows[0]["Trans_Id"].ToString();
                        lblTickeDate.Text = GetDate(dt.Rows[0]["Ticket_Date"].ToString());
                        lblStatus.Text = dt.Rows[0]["Status"].ToString();
                        lblTaskType.Text = dt.Rows[0]["Task_Type"].ToString();
                        lblCustomerNameValue.Text = dt.Rows[0]["CustomerName"].ToString();
                        lblScheduledate.Text = GetDate(dt.Rows[0]["Schedule_Date"].ToString());
                        lblDescriptionvalue.Text = dt.Rows[0]["Description"].ToString();
                    }
                    else
                    {
                        DisplayMessage("Ticket Not Found");
                        txtRefNo.Text = "";
                        txtRefNo.Focus();
                        //lnkticketdesc.Visible = false;
                        trTicketDetail.Visible = false;
                        hdnTicketid.Value = "0";
                        return;
                    }
                }
                else
                {
                    //lnkticketdesc.Visible = false;
                    hdnTicketid.Value = "0";
                }
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Refference number select in suggestion only");
                    txtRefNo.Text = "";
                    trTicketDetail.Visible = false;
                    txtRefNo.Focus();
                    dt.Dispose();
                    return;
                }
            }
        }
        dt.Dispose();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProjectNo(string prefixText, int count, string contextKey)
    {
        Prj_ProjectMaster objProjctMaster = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtProjectMAster = objProjctMaster.GetAllRecordProjectMasteer();
        dtProjectMAster = new DataView(dtProjectMAster, "Field4='" + HttpContext.Current.Session["CompId"].ToString() + "' and Field5='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Field6='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (HttpContext.Current.Session["ContactID"] != null)
        {
            dtProjectMAster = new DataView(dtProjectMAster, "Customer_Id=" + HttpContext.Current.Session["ContactID"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        DataTable DtTemp = dtProjectMAster.Copy();
        dtProjectMAster = new DataView(dtProjectMAster, "Field7 like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtProjectMAster.Rows.Count == 0)
        {
            dtProjectMAster = DtTemp.Copy();
        }
        string[] filterlist = new string[dtProjectMAster.Rows.Count];
        if (dtProjectMAster.Rows.Count > 0)
        {
            for (int i = 0; i < dtProjectMAster.Rows.Count; i++)
            {
                filterlist[i] = dtProjectMAster.Rows[i]["Field7"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInvoiceNo(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_SalesInvoiceHeader ObjSinvHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
            if (HttpContext.Current.Session["ContactID"] == null)
            {
                return null;
            }
            DataTable dtProjectMAster = ObjSinvHeader.FilterByInvoiceNo(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["ContactID"].ToString(), prefixText);
            string[] filterlist = new string[dtProjectMAster.Rows.Count];
            if (dtProjectMAster.Rows.Count > 0)
            {
                for (int i = 0; i < dtProjectMAster.Rows.Count; i++)
                {
                    filterlist[i] = dtProjectMAster.Rows[i]["Invoice_No"].ToString();
                }
            }
            dtProjectMAster.Dispose();
            return filterlist;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContractNo(string prefixText, int count, string contextKey)
    {
        SM_Contract_Master objContractMaster = new SM_Contract_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContractMaster.GetAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (HttpContext.Current.Session["ContactID"] != null)
        {
            dt = new DataView(dt, "Customer_Id=" + HttpContext.Current.Session["ContactID"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        DataTable DtTemp = dt.Copy();
        dt = new DataView(dt, "Contract_No like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = DtTemp.Copy();
        }
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Contract_No"].ToString();
            }
        }
        return filterlist;
    }
    #endregion
    #region Invoicesaving
    public void SaveInvoice(string WorkOrderId, string strCustomerId, ref SqlTransaction trns)
    {
        //here we are checking that  invoice created created or not againg this work order if created than we delete and recreate it but it should be unpost
        DataTable dtInvoice = objDa.return_DataTable("select Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader where SIFromTransType='W' and SIFromTransNo=" + WorkOrderId + " ", ref trns);
        if (dtInvoice.Rows.Count > 0)
        {
            objDa.execute_Command("delete from Inv_SalesInvoiceDetail where Invoice_No=" + dtInvoice.Rows[0]["Trans_Id"].ToString() + "", ref trns);
            objDa.execute_Command("delete from Inv_SalesInvoiceHeader where Trans_Id=" + dtInvoice.Rows[0]["Trans_Id"].ToString() + "", ref trns);
            objDa.execute_Command("delete from Inv_PaymentTrn where TypeTrans='SI' and TransNo=" + dtInvoice.Rows[0]["Trans_Id"].ToString() + "", ref trns);
            objDa.execute_Command("delete from Inv_StockBatchMaster where TransType='SI' and TransTypeId=" + dtInvoice.Rows[0]["Trans_Id"].ToString() + "", ref trns);
        }
        DataTable dtProduct = new DataTable();
        string strAddressId = string.Empty;
        //here we are getting shipping address and invoice address
        DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", strCustomerId, ref trns);
        if (dtAddress != null && dtAddress.Rows.Count > 0)
        {
            strAddressId = dtAddress.Rows[0]["Trans_id"].ToString();
        }
        Set_DocNumber objDocNo = new Set_DocNumber(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_StockDetail objStockDetail = new Inv_StockDetail(HttpContext.Current.Session["DBConnection"].ToString());
        double TotalQty = 0;
        double TotalAmount = 0;
        if (Session["dtToolsList"] != null)
        {
            dtProduct = (DataTable)Session["dtToolsList"];
            foreach (DataRow dr in dtProduct.Rows)
            {
                if (Convert.ToBoolean(dr["IsToolsExists"].ToString()))
                {
                    try
                    {
                        TotalQty += Convert.ToDouble(dr["Quantity"].ToString());
                    }
                    catch
                    {
                        TotalQty += 0;
                    }
                    try
                    {
                        TotalAmount += Convert.ToDouble(dr["Quantity"].ToString()) * Convert.ToDouble(dr["Field2"].ToString());
                    }
                    catch
                    {
                        TotalAmount += 0;
                    }
                }
            }
        }
        if (TotalAmount == 0)
        {
            return;
        }
        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(HttpContext.Current.Session["DBConnection"].ToString());
        string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
        int b = objSInvHeader.InsertSInvHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", objSysParam.getDateForInput(txtOrderDate.Text).ToString(), "0", SystemParameter.GetLocationCurrencyId(HttpContext.Current.Session["DBConnection"].ToString(),Session["CompId"].ToString(),Session["LocId"].ToString()), "W", WorkOrderId, Emp_ID, "", "", "", "0", "", false.ToString(), "0", TotalAmount.ToString(), TotalQty.ToString(), TotalAmount.ToString(), "0", "0", TotalAmount.ToString(), "0", "0", TotalAmount.ToString(), strCustomerId, "", "0",txtorderNo.Text, "", TotalAmount.ToString(), "", "", "", strAddressId, "0", strAddressId, "Approved", "1", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", "0", ref trns);
        //update invoice id in job card detail table
        objDa.execute_Command("update SM_WorkOrder set Invoice_Id='" + b.ToString() + "' where Trans_Id=" + WorkOrderId + "", ref trns);
        string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "13", "92", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        DataTable Dttemp = new DataTable();
        DataTable dtCount = objSInvHeader.GetSInvHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
        string strInvoiceNo = string.Empty;
        if (dtCount.Rows.Count == 0)
        {
            objSInvHeader.Updatecode(b.ToString(), strVoucherNo + "1", ref trns);
            strInvoiceNo = strVoucherNo + "1";
        }
        else
        {
            DataTable dtCount1 = new DataView(dtCount, "Invoice_No='" + strVoucherNo + dtCount.Rows.Count + "'", "", DataViewRowState.CurrentRows).ToTable();
            int NoRow = dtCount.Rows.Count;
            if (dtCount1.Rows.Count > 0)
            {
                bool bCodeFlag = true;
                while (bCodeFlag)
                {
                    NoRow += 1;
                    DataTable dtTemp = new DataView(dtCount, "Invoice_No='" + strVoucherNo + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtTemp.Rows.Count == 0)
                    {
                        bCodeFlag = false;
                    }
                }
            }
            objSInvHeader.Updatecode(b.ToString(), strVoucherNo + NoRow.ToString(), ref trns);
            strInvoiceNo = strVoucherNo + NoRow.ToString();
        }
        int counter = 0;
        string AvgCost = string.Empty;
        foreach (DataRow dr in dtProduct.Rows)
        {
            if (Convert.ToBoolean(dr["IsToolsExists"].ToString()))
            {
                try
                {
                    AvgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), dr["Tools_Id"].ToString()).Rows[0]["Field2"].ToString();
                }
                catch
                {
                    AvgCost = "0";
                }
                objSInvDetail.InsertSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), counter.ToString(), "0", "", "D", "0", dr["Tools_Id"].ToString(), "", dr["Unit_Id"].ToString(), dr["Field2"].ToString(), "0", "0", dr["Quantity"].ToString(), "0", "0", "0", "0", "False", false.ToString(), AvgCost, "0", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales/SalesInvoicePrint.aspx?Id=" + b.ToString() + "','','height=650,width=950,scrollbars=Yes')", true);
    }
    #endregion
    #region GetSiteAddress
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAddressName(string prefixText, int count, string contextKey)
    {
        Set_AddressMaster AddressN = new Set_AddressMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = AddressN.GetDistinctAddressName(prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Address_Name"].ToString();
            }
        }
        return str;
    }
    protected void txtShipingAddress_TextChanged(object sender, EventArgs e)
    {
        if (txtSiteAddress.Text != "")
        {
            DataTable dtAM = objAddress.GetAddressDataByAddressName(txtSiteAddress.Text.Trim().Split('/')[0].ToString());
            //txtShipCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dtAM.Rows.Count > 0)
            {
            }
            else
            {
                txtSiteAddress.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSiteAddress);
                return;
            }
        }
        //AllPageCode();
    }
    #endregion
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../ServiceManagementReport/WorkOrderReport.aspx?Id=" + e.CommandArgument.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void IbtnAcceptancePrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../ProjectManagement_Report/Final_Acceptance.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=W','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void IbtnTrainingPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../ProjectManagement_Report/Final_Acceptance.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=T','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion
    #region AddCustomer
    protected void btnAddCustomer_OnClick(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomer.Focus();
            return;
        }

        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
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
    protected void txtInTime_TextChanged(object sender, EventArgs e)
    {
        if (txtInTime.Text != "__:__" && txtOuttime.Text != "__:__")
        {
            if (txtOuttime.Text == "" || txtOuttime.Text == "__:__")
            {
                return;
            }
            DateTime In_Time_Temp = Convert.ToDateTime(txtInTime.Text);
            DateTime Out_Time_Temp = Convert.ToDateTime(txtOuttime.Text);
            if (In_Time_Temp > Out_Time_Temp)
            {
                DisplayMessage("Start Time must be greater than End Time");
                txtInTime.Text = "__:__";
                txtOuttime.Text = "__:__";
                txtInTime.Focus();
            }
            else if (In_Time_Temp == Out_Time_Temp)
            {
                DisplayMessage("Start Time and End Time cannot be equal");
                txtInTime.Text = "__:__";
                txtOuttime.Text = "__:__";
                txtInTime.Focus();
            }
            else
            {
                DateTime startTime = DateTime.Parse(txtInTime.Text);
                DateTime endTime = DateTime.Parse(txtOuttime.Text);
                TimeSpan ts = endTime.Subtract(startTime);
                txtWorkingHr.Text = ts.Hours.ToString() + ":" + ts.Minutes.ToString();
            }
        }
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
                txt[i] += dtCon.Rows[i]["Emp_Name"].ToString() + "/(" + dtCon.Rows[i]["designation"].ToString() + ")" + "/" + dtCon.Rows[i]["Emp_Id"].ToString() + ";";
            }
        }
        return txt;
    }
    protected void txtExtendID_TextChanged(object sender, EventArgs e)
    {
        hdnExtendedID.Value = "0";
        if (txtExtendID.Text.Trim() != "")
        {
            if (Session["ContactID"] != null)
            {
                DataTable dtCon = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString()).GetRecordByCustomerID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["ContactID"].ToString());
                dtCon = new DataView(dtCon, "Work_Order_No='" + txtExtendID.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtCon.Rows.Count > 0)
                {
                    hdnExtendedID.Value = dtCon.Rows[0]["Trans_Id"].ToString();
                }
            }
            else
            {
                DisplayMessage("Please Select Customer");
                txtExtendID.Text = "";
                txtCustomer.Focus();
            }
        }
        else
        {
            DisplayMessage("Please Select from Suggestions");
            txtExtendID.Text = "";
            txtExtendID.Focus();
        }
    }
    public string SetDecimal(string amount)
    {
        try
        {
            if (amount == "")
            {
                return "";
            }
            return objSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), amount);
        }
        catch
        {
            return "0";
        }
    }
    protected void IbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), "WorkOrder", "ServiceManagement", "WorkOrder");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
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
    protected void GvWorkOrder_FillContextMenuItems(object sender, DevExpress.Web.ASPxGridViewContextMenuEventArgs e)
    {
        if (e.MenuType == GridViewContextMenuType.Rows)
        {
           // e.Items.ForEach(x => x.Visible = false);
            if (canPrint.Value.ToLower() == "true")
            {
                var item = e.CreateItem("Print", "Print");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);
                item.Items.Add("Print Training Form", "IbtnTrainingPrint").Image.Url = "~/Images/print.png";
                item.Items.Add("Print Acceptance", "IbtnAcceptancePrint").Image.Url = "~/Images/print.png";
                item.Items.Add("Print Job Order", "IbtnPrint").Image.Url = "~/Images/print.png";
                item.BeginGroup = true;
                item.Image.Url = "~/Images/print.png";
                item.Image.Height = 15;
                item.Items[0].Image.Height = 15;
                item.Items[1].Image.Height = 15;
                item.Items[2].Image.Height = 15;
            }
            if (canPrint.Value.ToLower() == "true")
            {
                var item = e.CreateItem("Report System", "lnkReportSystem");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);
                item.Image.Url= "~/Images/print.png"; 
                item.Image.Height = 15;
                item.BeginGroup = true;
            }
            if (canView.Value.ToLower() == "true")
            {
                var View = e.CreateItem("View", "lnkViewDetail");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), View);
                View.Image.Url = "~/Images/Detail1.png";
                View.Image.Height = 15;
                View.BeginGroup = true;
            }
            if (canEdit.Value.ToLower() == "true")
            {
                var Edit = e.CreateItem("Edit", "btnEdit");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), Edit);
                Edit.Image.Url = "~/Images/edit.png";
                Edit.Image.Height = 15;
                Edit.BeginGroup = true;
                var Extended = e.CreateItem("Extended", "btnExtended");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), Extended);
                Extended.Image.Url = "~/Images/contact_renewal.png";
                Extended.Image.Height = 15;
                Extended.BeginGroup = true;
            }
            if (canDelete.Value.ToLower() == "true")
            {
                var Delete = e.CreateItem("Delete", "IbtnDelete");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), Delete);
                Delete.Image.Url = "~/Images/Erase.png";
                Delete.BeginGroup = true;
                Delete.Image.Height = 15;
            }
            if (canUpload.Value.ToLower() == "true")
            {
                var FileUpload = e.CreateItem("File Upload", "IbtnFileUpload");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), FileUpload);
                FileUpload.Image.Url = "~/Images/ModuleIcons/archiving.png";
                FileUpload.BeginGroup = true;
                FileUpload.Image.Height = 15;
            }
            if (canFollowup.Value.ToLower() == "true")
            {
                var followup = e.CreateItem("Followup", "IbtnFollowup");
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), followup);
                followup.Image.Url = "~/Images/follow-up.png";
                followup.BeginGroup = true;
                followup.Image.Height = 15;
            }
            var Map = e.CreateItem("Map", "btnShowMap");
            e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), Map);
            Map.Image.Url = "~/Images/Detail1.png";
            Map.Image.Height = 15;
            Map.BeginGroup = true;
        }
    }
    protected void GvWorkOrder_ContextMenuItemClick(object sender, DevExpress.Web.ASPxGridViewContextMenuItemClickEventArgs e)
    {
        ImageButton ib = new ImageButton();
        switch (e.Item.Name)
        {
            case "IbtnTrainingPrint":
                DataRowView dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id1 = dr["Trans_Id"].ToString();
                string strCmd1 = string.Format("window.open('../ProjectManagement_Report/Final_Acceptance.aspx?Id=" + trans_id1 + "&&Type=T','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd1, true);
                break;
            case "lnkReportSystem":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_idNew = dr["Trans_Id"].ToString();
                string cmdNew = string.Format("getReportData(" + trans_idNew + ");");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", cmdNew, true);
                break;
            case "IbtnAcceptancePrint":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id2 = dr["Trans_Id"].ToString();
                string strCmd2 = string.Format("window.open('../ProjectManagement_Report/Final_Acceptance.aspx?Id=" + trans_id2 + "&&Type=W','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd2, true);
                break;
            case "IbtnPrint":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id3 = dr["Trans_Id"].ToString();
                string strCmd3 = string.Format("window.open('../ServiceManagementReport/WorkOrderReport.aspx?Id=" + trans_id3 + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd3, true);
                break;
            case "lnkViewDetail":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id4 = dr["Trans_Id"].ToString();
                ddlLoc.SelectedValue = dr.Row.ItemArray[2].ToString();
                btnEdit_Command("lnkViewDetail", trans_id4);
                break;
            case "btnEdit":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id5 = dr["Trans_Id"].ToString();
                ddlLoc.SelectedValue = dr.Row.ItemArray[2].ToString();
                btnEdit_Command("btnEdit", trans_id5);
                break;
            case "btnExtended":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id8 = dr["Trans_Id"].ToString();
                ddlLoc.SelectedValue = dr.Row.ItemArray[2].ToString();
                btnEdit_Command("btnExtended", trans_id8);
                break;
            case "IbtnDelete":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id6 = dr["Trans_Id"].ToString();
                ddlLoc.SelectedValue = dr.Row.ItemArray[2].ToString();
                IbtnDelete_Command(trans_id6);
                break;
            case "IbtnFileUpload":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id7 = dr["Trans_Id"].ToString();
                string work_order_no = dr["Work_Order_No"].ToString();
                ddlLoc.SelectedValue = dr.Row.ItemArray[2].ToString();
                FUpload1.setID(trans_id7, Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + ddlLoc.SelectedValue + "/WorkOrder", "ServiceManagement", "WorkOrder", work_order_no, work_order_no);
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
                break;
            case "IbtnFollowup":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id10 = dr["Trans_Id"].ToString();
                string CustomerID = dr["Customer_ID"].ToString();
                Session["WorkOrder_CustID"] = "0";
                Session["WorkOrder_CustID"] = trans_id10;
                FollowupUC.setLocationId(dr["Location_ID"].ToString());
                FollowupUC.newBtnCall();
                FollowupUC.GetFollowupDocumentNumber();
                FollowupUC.SetGeneratedByName();
                //FollowupUC.ResetFollowupType();
                FollowUp FollowupClass = new FollowUp(HttpContext.Current.Session["DBConnection"].ToString());
                DataTable dt1 = FollowupClass.getOppoHeaderDtlByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), dr["Location_Id"].ToString(), dr["Trans_Id"].ToString());
                FollowupUC.fillHeader("Follow-Up (Work Order)", dr["CustomerName"].ToString());

                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Followup_Open();showNewTab();", true);
                break;
            case "btnShowMap":
                dr = (System.Data.DataRowView)GvWorkOrder.GetRow(e.ElementIndex);
                string trans_id9 = dr["Trans_Id"].ToString();
                DataTable dt = ObjWorkOrder.GetRecordByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), trans_id9);
                if (dt.Rows.Count != 0)
                {
                    Session["Lati"] = dt.Rows[0]["Latitude"].ToString();
                    Session["Long"] = dt.Rows[0]["Longitude"].ToString();
                    if (Session["Lati"].ToString() != "" || Session["Long"].ToString() != "")
                    {
                        if (Session["Lati"].ToString() == "0")
                        {
                            DisplayMessage("Location has not updated till yet !!!");
                            return;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, typeof(Page), "nothing", "window.open('../SystemSetup/GoogleMap.aspx','window','width=1024')", true);
                        }
                    }
                    else
                    {
                        DisplayMessage("Location has not updated till yet !!!");
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Location has not updated till yet !!!");
                    return;
                }
                break;
        }
    }
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
    }
    public string GetTlList()
    {
        Session["To_Do_List_EmpList"] = Session["EmpId"].ToString();

        DataTable dt = objEmpMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());
        
        dt = new DataView(dt, "Emp_id='" + Session["EmpID"].ToString() + "' or Field5='" + Session["EmpID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



        //dt = new DataView(dt, "Field5='" + Session["EmpID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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
    public System.Drawing.Color getColor(string date)
    {
        if (date.Trim() != "")
        {
            return System.Drawing.Color.Green;
        }
        else
        {
            return System.Drawing.Color.White;
        }
    }
    protected void AddAddress_Click(object sender, EventArgs e)
    {
        if (Session["ContactID"] == null)
        {
            DisplayMessage("Please Select a Customer To Add New Address");
            return;
        }

        if (Session["ContactID"].ToString() == "" || Session["ContactID"].ToString() == "0")
        {
            DisplayMessage("Please Select a Customer To Add New Address");
            return;
        }
        addaddress1.Reset();

        Country_Currency objCountryCurrency = new Country_Currency(HttpContext.Current.Session["DBConnection"].ToString());

        ViewState["Country_Id"] = objCountryCurrency.GetCurrencyByCountryId(Session["LocCurrencyId"].ToString(), "2").Rows[0]["Country_Id"].ToString();

        if (ViewState["Country_Id"] != null)
        {
            addaddress1.BtnNew_click(ViewState["Country_Id"].ToString());
            Session["AddCtrl_Country_Id"] = ViewState["Country_Id"].ToString();
        }

        addaddress1.fillGridAdd(Session["ContactID"].ToString());
        addaddress1.setCustomerID(Session["ContactID"].ToString());
        addaddress1.fillHeader(txtCustomer.Text);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_NewAddress_Open();displayList()", true);
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
    public static string ddlCountry_IndexChanged(string CountryId)
    {
        CountryMaster ObjSysCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
        return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void resetAddress()
    {
        HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
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
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            dtcon = null;
            return filterlistcon;
        }
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
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    #region AddCustomer

    protected void btnAddNewCustomer_OnClick(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../Sales/AddContact.aspx?Page=SINV','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }    
    #endregion
}