using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Collections;

public partial class MasterSetUp_EmployeeApproval : BasePage
{
    PegasusDataAccess.DataAccessClass objDA = null;
    //LogProcess ObjLOgProcess = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    Att_HalfDay_Request objHalfDay = null;
    Att_AttendanceLog objAttLog = null;
    Set_Approval_Employee objEmpApproval = null;
    SystemParameter objSys = null;
    UserMaster objUser = null;
    Ems_ContactMaster objContact = null;
    EmployeeParameter objEmpParam = null;
    Pay_Employee_claim objPayClaim = null;
    Pay_Employee_Loan objLoan = null;
    Att_Employee_Leave objEmpleave = null;
    Att_Leave_Request objleaveReq = null;
    Att_OverTime_Request objOverTimeReq = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    Att_PartialLeave_Request objPartial = null;
    Att_ScheduleMaster objAtt_ScheduleMaster = null;
    Common cmn = null;
    CompanyMaster objComp = null;
    EmployeeMaster objEmployee = null;
    CurrencyMaster objCurrency = null;
    SendMailSms ObjSendMailSms = null;
    Set_CustomerMaster_CreditParameter ObjCreditParam = null;
    PurchaseRequestHeader objPurrchaseRequestHeader = null;
    PurchaseRequestDetail ObjPurrchaseRequestDetail = null;
    NotificationMaster Obj_Notifiacation = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    PurchaseOrderDetail ObjPurchaseOrderDetail = null;
    Ems_MailMarketing objMailMarket = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesOrderDetail ObjSOrderDetail = null;
    Inv_SalesQuotationHeader objSQuoteHeader = null;
    Inv_SalesQuotationDetail ObjSQuoteDetail = null;
    Inv_SalesInquiryHeader objSIHeader = null;
    Inv_SalesInquiryDetail ObjSIDetail = null;
    LocationMaster objLocation = null;
    Inv_ParameterMaster objInvParam = null;
    Set_ApprovalMaster ObjapprovalMaster = null;
    Att_tmpEmpShiftSchedule ObjTempEmpShift = null;
    PageControlCommon objPageCmn = null;
    Set_Employee_Holiday objEmpHoliday = null;
    Att_ScheduleMaster objEmpSch = null;
    Att_ShiftManagement objShift = null;
    Att_ShiftDescription objShiftdesc = null;


    string MailMessage = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objShiftdesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        objEmpHoliday = new Set_Employee_Holiday(Session["DBConnection"].ToString());
        objEmpSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        objAtt_ScheduleMaster = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        // ObjLOgProcess = new LogProcess(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objHalfDay = new Att_HalfDay_Request(Session["DBConnection"].ToString());
        objAttLog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objPayClaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        objLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objOverTimeReq = new Att_OverTime_Request(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpHalfDay = new Att_Employee_HalfDay(Session["DBConnection"].ToString());
        objPartial = new Att_PartialLeave_Request(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        ObjCreditParam = new Set_CustomerMaster_CreditParameter(Session["DBConnection"].ToString());
        objPurrchaseRequestHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        ObjPurrchaseRequestDetail = new PurchaseRequestDetail(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        ObjPurchaseOrderDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        objMailMarket = new Ems_MailMarketing(Session["DBConnection"].ToString());
        objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSInvDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjSOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objSQuoteHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjSQuoteDetail = new Inv_SalesQuotationDetail(Session["DBConnection"].ToString());
        objSIHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        ObjSIDetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjapprovalMaster = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        ObjTempEmpShift = new Att_tmpEmpShiftSchedule(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        Page.Title = objSys.GetSysTitle();
        btnRejectPopup.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnRejectPopup, "").ToString());
        btnApprovePopup.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnApprovePopup, "").ToString());
        if (!IsPostBack)
        {
            txtSelectYear.Text = DateTime.Now.Year.ToString();
            ddlMonth.SelectedIndex = DateTime.Now.Month;
            FillFilterLoc();
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "162", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            pnlApproval.Visible = false;
            FillApprovalType();
            if (!string.IsNullOrEmpty(Request.QueryString["Request_ID"]))
            {
                Show_Request_Record();
            }
        }
        AllPageCode();
    }

    public void FillFilterLoc()
    {
        string LocIds = Session["MyAllLoc"].ToString();
        DataTable dtLocation = new DataTable();
        dtLocation = objLocation.GetAllLocationMaster();

        if (LocIds.Length > 0)
            dtLocation = new DataView(dtLocation, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        ddlFilterLoc.DataSource = dtLocation;
        ddlFilterLoc.DataValueField = "Location_Id";
        ddlFilterLoc.DataTextField = "Location_Name";
        ddlFilterLoc.DataBind();

        ddlFilterLoc.Items.Insert(0, "--Select--");
        ddlFilterLoc.SelectedIndex = 0;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }
    #region shiftView
    protected void gvApproval_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dtTemp = new DataTable();
        string strRefId = string.Empty;
        DataTable dtDetail = new DataTable();
        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GridView gvEmp = (GridView)e.Row.FindControl("gvEmp");
                gvEmp.Columns[2].Visible = false;
                GridView gvShiftView = (GridView)e.Row.FindControl("gvShiftView");
                if (e.Row.RowIndex % 2 == 0 || e.Row.RowIndex == 0)
                {
                }
                else
                {
                    gvShiftView.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
                }
                strRefId = ((HiddenField)e.Row.FindControl("hdnRefId")).Value;
                dtDetail = ObjTempEmpShift.GetDetailRecordBy_Header_Id(strRefId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                dtTemp = GetShiftInformation(dtDetail);
                objPageCmn.FillData((object)gvShiftView, dtTemp, "", "");
            }
        }
    }
    public DataTable GetShiftInformation(DataTable dtdetail)
    {
        DataTable dtTemp = new DataTable();
        DateTime dtFromdate = new DateTime();
        string strdatecell = string.Empty;
        dtTemp.Columns.Add("Month");
        dtTemp.Columns.Add("Year");
        dtTemp.Rows.Add();
        dtTemp.Rows[dtTemp.Rows.Count - 1][0] = Convert.ToDateTime(dtdetail.Rows[0]["schedule_date"].ToString()).Month;
        dtTemp.Rows[dtTemp.Rows.Count - 1][1] = Convert.ToDateTime(dtdetail.Rows[0]["schedule_date"].ToString()).Year;
        foreach (DataRow dr in dtdetail.Rows)
        {
            dtFromdate = Convert.ToDateTime(dr["schedule_date"].ToString());
            strdatecell = "" + dtFromdate.Day.ToString() + "(" + dtFromdate.DayOfWeek.ToString().Substring(0, 3) + ")" + "";
            dtTemp.Columns.Add(strdatecell, typeof(string));
            dtTemp.Rows[0][strdatecell] = dr["Ref_Name"].ToString();
        }
        return dtTemp;
    }
    #endregion
    public void Show_Request_Record()
    {
        string Emp_ID = Request.QueryString["Request_ID"].ToString();
        string Request_Type = Request.QueryString["Request_Type"].ToString();
        string Approval_Type = "Pending";
        if (Request.QueryString["Approval_Type"] != null)
            Approval_Type = Request.QueryString["Approval_Type"].ToString();
        ddlApprovalType.SelectedValue = Request_Type;
        hdnApprovalType.Value = Request_Type;
        lblApprovalType1.Text = ddlApprovalType.SelectedItem.Text;
        if (ddlApprovalType.SelectedItem.Text != "--Select--")
        {
            pnlApprovalType.Visible = false;
            pnlApproval.Visible = true;
            ddlStatus.SelectedValue = Approval_Type;
            ddlStatus_OnSelectedIndexChanged(null, null);
            ddlFieldName.SelectedValue = "RequestEmp_Code";
            ddlOption.SelectedValue = "Equal";
            txtValue.Text = Emp_ID;
            btnbind_Click(null, null);
        }
    }
    public void FillApprovalType()
    {
        DataTable dt = objEmpApproval.getApprovalTypeByEmpId(Session["EmpId"].ToString());
        //objEmpApproval.GetApprovalType();
        if (dt.Rows.Count > 0)
        {
            ddlApprovalType.DataSource = null;
            ddlApprovalType.DataBind();
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlApprovalType, dt, "Approval_Name", "Approval_Id");
        }
        else
        {
            try
            {
                ddlApprovalType.Items.Insert(0, "--Select--");
                ddlApprovalType.SelectedIndex = 0;
            }
            catch
            {
                ddlApprovalType.Items.Insert(0, "--Select--");
                ddlApprovalType.SelectedIndex = 0;
            }
        }
        dt.Dispose();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName("162", (DataTable)Session["ModuleName"]);
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
        Page.Title = objSys.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            gvApproval.Visible = true;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "162", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {
            }
            else
            {
                btnSave.Visible = true;
                gvApproval.Visible = true;
            }
        }
    }
    public string GetEmpIdByUserId()
    {
        string EmpId = string.Empty;
        if (Session["UserId"].ToString() == "superadmin")
        {
            EmpId = "0";
        }
        else
        {
            DataTable dtUser = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString());
            if (dtUser.Rows.Count > 0)
            {
                EmpId = dtUser.Rows[0]["Emp_Id"].ToString();
            }
            dtUser.Dispose();
        }
        return EmpId;
    }
    protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlReferenceNo.SelectedIndex = 0;
        }
        catch
        {

        }

        ddlReferenceNo.DataSource = null;
        ddlReferenceNo.DataBind();

        FillGrid(hdnApprovalType.Value);
        if (ddlStatus.SelectedValue.Trim() == "Pending" && (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift")))
        {
            btnShiftApprove.Visible = true;
            btnShiftReject.Visible = true;
        }
        else
        {
            btnShiftApprove.Visible = false;
            btnShiftReject.Visible = false;
        }
        FillshiftReferenceno();
    }
    protected void btnBackClick(object sender, EventArgs e)
    {
        pnlApproval.Visible = false;
        pnlApprovalType.Visible = true;
        hdnApprovalType.Value = "";
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (ddlApprovalType.SelectedIndex == 0)
        {
            DisplayMessage("Please select approval type");
            return;
        }
        hdnApprovalType.Value = ddlApprovalType.SelectedValue;
        lblApprovalType1.Text = ddlApprovalType.SelectedItem.Text;
        div_shift_Ref.Visible = false;
        //FillGrid(hdnApprovalType.Value);
        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
        {
            btnShiftApprove.Visible = true;
            btnShiftReject.Visible = true;
            div_shift_Ref.Visible = true;
        }
        else
        {
            btnShiftApprove.Visible = false;
            btnShiftReject.Visible = false;
        }
        pnlApprovalType.Visible = false;
        pnlApproval.Visible = true;
        ddlStatus.SelectedValue = "Pending";
        ddlStatus_OnSelectedIndexChanged(null, null);
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate.Trim() != "" && strDate.Trim() != "01-Jan-2000 00:00" && strDate.Trim() != "01-Jan-1900 00:00")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string CurrencyAmount = string.Empty;
        try
        {
            CurrencyAmount = SystemParameter.GetCurrencySmbol(CurrencyId, objSys.GetCurencyConversionForInv(CurrencyId, Amount), Session["DBConnection"].ToString());
        }
        catch
        {

        }
        return CurrencyAmount;
    }
    public string SetDecimal(string Amount)
    {
        string CurrencyAmount = string.Empty;
        try
        {
            CurrencyAmount = objSys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Amount);
        }
        catch
        {

        }
        return CurrencyAmount;
    }
    public void FillGrid(string ApprovalType, string strReferenece = "")
    {

        if (ApprovalType == "17")
        {
            divFilter.Visible = true;
        }
        else
        {
            divFilter.Visible = false;
        }
        DataTable dtshiftreflist = new DataTable();
        Session["dtRefApproval"] = null;
        gvApproval.DataSource = null;
        gvApproval.DataBind();
        dtlistShift.DataSource = null;
        dtlistShift.DataBind();
        DataSet ds = new DataSet();
        //variable for check hierarchy or priority basis
        string strApprovalType = ObjapprovalMaster.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString();
        string IsOpen = string.Empty;
        DataTable dtTempData = new DataTable();
        DataTable dt = new DataTable();
        DataTable dtNew = new DataTable();
        string EmpPermission = string.Empty;
        EmpPermission = objSys.Get_Approval_Parameter_By_ID(ApprovalType).Rows[0]["Approval_Level"].ToString();
        if (ApprovalType != "17")
        {
            // dt = objEmpApproval.GetApprovalChildByStatus(Session["CompId"].ToString(), ApprovalType);
        }
        else
        {
            string strStatus = "1";
            if (ddlStatus.SelectedItem.ToString() == "Approved")
            {
                strStatus = "2";
            }
            else if (ddlStatus.SelectedItem.ToString() == "Rejected")
            {
                strStatus = "3";
            }
            dt = objEmpApproval.GetApprovalChildByStatus_New(Session["CompId"].ToString(), ddlMonth.SelectedIndex.ToString(), txtSelectYear.Text, ApprovalType, strStatus);
        }

        //this code is created by jitendra upadhyay for column visibility according the condition
        //for show sales order no ,order date and order amount
        //created on 02-06-2016
        //code start
        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
        {
            gvApproval.Columns[0].Visible = true;
            gvApproval.AllowPaging = false;
        }
        else
        {
            gvApproval.Columns[0].Visible = false;
        }
        if (ApprovalType == "9")
        {
            //gvApproval.Columns[1].Visible = false;
            gvApproval.Columns[3].Visible = false;
            gvApproval.Columns[4].Visible = true;
            gvApproval.Columns[5].Visible = true;
            gvApproval.Columns[6].Visible = true;
            gvApproval.Columns[8].Visible = false;
        }
        else
        {
            //gvApproval.Columns[1].Visible = true;
            gvApproval.Columns[3].Visible = true;
            gvApproval.Columns[4].Visible = false;
            gvApproval.Columns[5].Visible = false;
            gvApproval.Columns[6].Visible = false;
            gvApproval.Columns[8].Visible = true;
        }
        //for check  header caption
        if (ApprovalType == "14")
        {
            try
            {
                gvApproval.Columns[2].HeaderText = Resources.Attendance.Customer;
            }
            catch
            {

            }
        }
        ///code end 
        if (EmpPermission == "2")
        {
            if (ApprovalType == "17")
            {
                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtNew = objEmpApproval.GetApprovalChildByStatus1(Session["CompId"].ToString(), Session["BrandId"].ToString(), ApprovalType, ddlStatus.SelectedValue);
            }
        }
        else if (EmpPermission == "3")
        {
            if (ApprovalType == "17")
            {
                string LocIds = Session["MyAllLoc"].ToString();
                if (ddlFilterLoc.SelectedIndex > 0)
                {
                    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id in(" + ddlFilterLoc.SelectedValue.ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            else
            {
                string LocIds = Session["MyAllLoc"].ToString();
                if (ddlFilterLoc.SelectedIndex > 0)
                {
                    dtNew = objEmpApproval.GetApprovalChildByStatus2(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlFilterLoc.SelectedValue.ToString(), ApprovalType, ddlStatus.SelectedValue);
                }
                else
                {
                    dtNew = objEmpApproval.GetApprovalChildByStatus2(Session["CompId"].ToString(), Session["BrandId"].ToString(), LocIds, ApprovalType, ddlStatus.SelectedValue);
                }
            }
        }
        else if (EmpPermission == "4" && strApprovalType.Trim() == "Priority")
        {
            if (ApprovalType == "17")
            {
                if (Session["SessionDepId"] != null)
                {
                    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "' and Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = null;
                }
            }
            else
            {
                if (Session["SessionDepId"] != null)
                {
                    dtNew = objEmpApproval.GetApprovalChildByStatus3(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["SessionDepId"].ToString(), ApprovalType, ddlStatus.SelectedValue);
                }
                else
                {
                    dtNew = null;
                }
            }
        }

        if (ApprovalType == "17")
        {
            if (dt != null)
            {
                dtTempData = new DataView(dt, "Approval_Id='" + ApprovalType + "' ", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtTempData"] = dtTempData;
                dt = new DataView(dt, "Approval_Id='" + ApprovalType + "' and Priority='True'  ", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else if (dt.Rows.Count == 0 & dtNew.Rows.Count == 0)
        {
            //dtNew = objEmpApproval.GetApprovalChildByStatu
            //s(Session["CompId"].ToString(), ApprovalType);
            if (ApprovalType == "15")
            {
                dtNew = objEmpApproval.GetApprovalChildByStatus5(Session["CompId"].ToString(), ApprovalType, ddlStatus.SelectedValue);
            }
            else
            {
                dtNew = objEmpApproval.GetApprovalChildByStatus4(Session["CompId"].ToString(), ApprovalType, ddlStatus.SelectedValue);
            }

            if (dtNew != null)
            {
                Session["dtTempData"] = dtNew;
                dtNew = new DataView(dtNew, "Priority='True' and Status='" + ddlStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        else
        {
            if (dtNew != null)
            {
                Session["dtTempData"] = dtNew;
                dtNew = new DataView(dtNew, "Priority='True'  ", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (ApprovalType == "17")
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Status='" + ddlStatus.SelectedValue + "' ", "Request_date", DataViewRowState.CurrentRows).ToTable();
            }
            Session["dtRefApproval"] = dt;
        }
        else
        {
            if (dtNew != null && dtNew.Rows.Count > 0)
            {
                Session["dtRefApproval"] = dtNew;
            }
        }

        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
        {
            if (ddlReferenceNo.SelectedIndex > 0)
            {
                dt = new DataView(dt, "ReferenceNo='" + ddlReferenceNo.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            dtshiftreflist = dt;
        }



        if (dt != null && dt.Rows.Count > 0)
        {
            if ((ddlReferenceNo.SelectedIndex > 0) || ApprovalType != "17")
            {
                if (ApprovalType != "15")
                {
                    dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                dt = dt.DefaultView.ToTable(true, "Ref_Id", "Request_Emp_Id", "Request_Date", "Expr1", "Status", "Approval_Id", "Approval_Type", "SalesOrderNo", "SalesOrderDate", "NetAmount", "Currency_Id", "RequestEmp_Code", "RequestEmp_Name");
            }
        }
        else if (dtNew != null && dtNew.Rows.Count > 0)
        {
            if ((ddlReferenceNo.SelectedIndex > 0) || ApprovalType != "17")
            {
                if (ApprovalType != "15")
                {
                    dtNew = new DataView(dtNew, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                dtNew = dtNew.DefaultView.ToTable(true, "Ref_Id", "Request_Emp_Id", "Request_Date", "Expr1", "Status", "Approval_Id", "Approval_Type", "SalesOrderNo", "SalesOrderDate", "NetAmount", "Currency_Id", "RequestEmp_Code", "RequestEmp_Name");
            }
        }


        string EmpId = GetEmpIdByUserId();
        chkselectall.Checked = false;
        chkselectall.Visible = false;
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows.Count > 0)
            {
                lblTotalRecords.Text = "Total Records: " + dt.Rows.Count.ToString();
                if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                {
                    chkselectall.Visible = true;
                    gvApproval.DataSource = null;
                    gvApproval.DataBind();
                    DataTable dtrefdetail = new DataTable();
                    if ((ddlReferenceNo.SelectedIndex > 0) && ApprovalType == "17")
                    {
                        //if (Session["EmpId"].ToString() != "892")
                        //    dtrefdetail =  ObjTempEmpShift.GetHeaderRecordByApproveStatus(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), Session["LocId"].ToString(), "0");
                        //else
                        //    dtrefdetail = ObjTempEmpShift.GetHeaderRecordByApproveStatus(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0".ToString(), "0");
                        dtrefdetail = ObjTempEmpShift.GetHeaderRecordByApproveStatus(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0".ToString(), "0");
                        string LocIds = Session["MyAllLoc"].ToString();
                        if (ddlFilterLoc.SelectedIndex > 0)
                        {
                            dtrefdetail = new DataView(dtrefdetail, "Location_Id in(" + ddlFilterLoc.SelectedValue.ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        else
                        {
                            dtrefdetail = new DataView(dtrefdetail, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                        }


                        DataTable dtReferenceList = new DataView(dtshiftreflist, "", "Request_date", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "ReferenceNo");
                        dtlistShift.DataSource = dtReferenceList;
                        dtlistShift.DataBind();
                        DataTable dtReferenceDetail = new DataTable();
                        DataTable dtapprovaldetail = new DataTable();
                        string strApprovalperson = string.Empty;
                        string strApprovaldate = string.Empty;

                        foreach (DataListItem dtlistrow in dtlistShift.Items)
                        {
                            strApprovalperson = string.Empty;
                            strApprovaldate = string.Empty;
                            dtReferenceDetail = new DataView(dtrefdetail, "ReferenceNo='" + ((Label)dtlistrow.FindControl("lblRefno")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            dtapprovaldetail = objDA.return_DataTable("select distinct Set_EmployeeMaster.Emp_Name+'/'+Set_EmployeeMaster.Emp_Code as Approval_person,cast(Set_Approval_Transaction.Status_Update_Date as DATE) as Status_Update_Date from Set_Approval_Transaction left join Set_EmployeeMaster on  Set_Approval_Transaction.Emp_Id = Set_EmployeeMaster.Emp_Id where  Set_Approval_Transaction.Approval_Id=17  and Set_Approval_Transaction.Ref_Id in (select Att_tmpEmpShiftSchedule.trans_id from Att_tmpEmpShiftSchedule where Att_tmpEmpShiftSchedule.Field2 = '" + ((Label)dtlistrow.FindControl("lblRefno")).Text.Trim().Trim() + "')");
                            if (dtapprovaldetail.Rows.Count > 0)
                            {
                                foreach (DataRow drapproval in dtapprovaldetail.Rows)
                                {
                                    strApprovalperson += drapproval["Approval_person"] + ",";
                                }
                                if (dtapprovaldetail.Rows[0]["Status_Update_Date"].ToString() != "1900-01-01 12:00:00 AM")
                                {
                                    strApprovaldate = Convert.ToDateTime(dtapprovaldetail.Rows[0]["Status_Update_Date"].ToString()).ToString("dd-MMM-yyyy");
                                }
                            }

                            try
                            {
                                ((Label)dtlistrow.FindControl("lbllocation")).Text = dtReferenceDetail.Rows[0]["Location_Name"].ToString();
                                ((Label)dtlistrow.FindControl("lblstatus")).Text = dtReferenceDetail.Rows[0]["Approval_Status"].ToString();
                                ((Label)dtlistrow.FindControl("lbluploadedby")).Text = dtReferenceDetail.Rows[0]["uploaded_BY"].ToString();
                                ((Label)dtlistrow.FindControl("lbluploaddate")).Text = Convert.ToDateTime(dtReferenceDetail.Rows[0]["CreatedDate"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss");
                            }
                            catch
                            {

                            }
                            ds = GetshiftDatatable(new DataView(dtshiftreflist, "ReferenceNo='" + ((Label)dtlistrow.FindControl("lblRefno")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable(), strReferenece);

                            dtTempData = new DataView(ds.Tables[0], "", "Code", DataViewRowState.CurrentRows).ToTable();

                            dtTempData = RemoveDuplicateRows(dtTempData, "Code");

                            GridView gvshiftViewList = ((GridView)dtlistrow.FindControl("gvShiftView"));
                            GridView gvshiftsummaryList = ((GridView)dtlistrow.FindControl("gvshiftsummary"));
                            gvshiftViewList.DataSource = dtTempData;
                            gvshiftViewList.DataBind();
                            gvshiftsummaryList.DataSource = ds.Tables[1];
                            gvshiftsummaryList.DataBind();
                            if (gvshiftsummaryList.Rows.Count > 0)
                            {
                                for (int j = 0; j < gvshiftsummaryList.HeaderRow.Cells.Count; j++)
                                {
                                    string s = gvshiftsummaryList.HeaderRow.Cells[j].Text.ToString(); //Returns ""
                                    if (s == "Name" || s == "Month" || s == "Year")
                                    {
                                        gvshiftsummaryList.HeaderRow.Cells[j].Text = "";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dt != null)
                    {
                        //Common Function add By Lokesh on 12-05-2015
                        dt = new DataView(dt, "", "request_date DESC", DataViewRowState.CurrentRows).ToTable();
                        objPageCmn.FillData((object)gvApproval, dt, "", "");
                        Session["Approval"] = dt;
                        Session["dtFilter_EmpAppr_Mstr"] = dt;
                        FillChildGrid();
                        dtlistShift.DataSource = null;
                        dtlistShift.DataBind();
                    }
                    else
                    {
                        //Common Function add By Lokesh on 12-05-2015
                        dtNew = new DataView(dtNew, "", "request_date DESC", DataViewRowState.CurrentRows).ToTable();
                        objPageCmn.FillData((object)gvApproval, dtNew, "", "");
                        Session["Approval"] = dtNew;
                        Session["dtFilter_EmpAppr_Mstr"] = dtNew;
                        FillChildGrid();
                        dtlistShift.DataSource = null;
                        dtlistShift.DataBind();
                    }
                }
                //for shift view we are using new grid so making datatable
            }
            else
            {
                lblTotalRecords.Text = "Total Records: 0";
                gvApproval.DataSource = null;
                gvApproval.DataBind();
                Session["Approval"] = null;
            }
        }
        else if (dtNew != null && dtNew.Rows.Count > 0)
        {

            //Common Function add By Lokesh on 12-05-2015
            lblTotalRecords.Text = "Total Records: " + dtNew.Rows.Count.ToString();
            dtNew = new DataView(dtNew, "", "request_date DESC", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)gvApproval, dtNew, "", "");
            Session["Approval"] = dtNew;
            Session["dtFilter_EmpAppr_Mstr"] = dtNew;
            FillChildGrid();
            dtlistShift.DataSource = null;
            dtlistShift.DataBind();

        }
        else
        {
            lblTotalRecords.Text = "Total Records: 0";
            gvApproval.DataSource = null;
            gvApproval.DataBind();
            Session["Approval"] = null;
        }


        if (dtTempData != null)
            dtTempData.Dispose();
        if (dt != null)
            dt.Dispose();
    }

    public DataTable RemoveDuplicateRows(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        //Add list of all the unique item value to hashtable, which stores combination of key, value pair.
        //And add duplicate item value in arraylist.
        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
                duplicateList.Add(drow);
            else
                hTable.Add(drow[colName], string.Empty);
        }

        //Removing a list of duplicate items from datatable.
        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);

        //Datatable which contains unique records will be return as output.
        return dTable;
    }
    public void FillshiftReferenceno()
    {
        ddlReferenceNo.Items.Clear();
        if (Session["dtRefApproval"] != null)
        {
            ddlReferenceNo.DataSource = ((DataTable)Session["dtRefApproval"]).DefaultView.ToTable(true, "ReferenceNo");
            ddlReferenceNo.DataTextField = "ReferenceNo";
            ddlReferenceNo.DataValueField = "ReferenceNo";
            ddlReferenceNo.DataBind();
        }
        ddlReferenceNo.Items.Insert(0, "--Select--");
    }
    public void FillChildGrid()
    {
        DataTable dtTempData = new DataTable();
        //DataTable dtTransactiondata = new DataTable();
        //dtTransactiondata = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
        dtTempData = (DataTable)Session["dtTempData"];
        if (dtTempData != null)
        {
            if (dtTempData.Rows.Count > 0)
            {
                string strApprovalType = ((DataTable)Session["dtTempData"]).Rows[0]["Approval_Type1"].ToString();
                string IsOpen = ((DataTable)Session["dtTempData"]).Rows[0]["is_open"].ToString();
                DataTable dt1 = new DataTable();
                DataTable dtPri2 = new DataTable();
                DataTable dtPri = new DataTable();
                foreach (GridViewRow gvr in gvApproval.Rows)
                {
                    GridView gvEmp = (GridView)(gvr.FindControl("gvEmp"));
                    Label lblRequestStatus = (Label)(gvr.FindControl("lblRequestStatus"));
                   // dt1 = dtTransactiondata.Copy();
                    //dt1 = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
                    HiddenField hdnRef = (HiddenField)gvr.FindControl("hdnRefId");
                    HiddenField hdnReq = (HiddenField)gvr.FindControl("hdnRequestDate");
                    HiddenField hdnApproval = (HiddenField)gvr.FindControl("hdnApprovalType");
                    HiddenField hdnApprovalId = (HiddenField)gvr.FindControl("hdnApprovalId");
                    Label lblStaus = (Label)gvr.FindControl("lblAppStatus");
                    // dt1 = new DataView(dt1, "Ref_Id='" + hdnRef.Value + "' and Approval_Id='" + hdnApprovalId.Value + "' and Approval_Name='" + lblApprovalType1.Text + "' and Status='" + lblRequestStatus.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //dt1 = new DataView(dt1, "Ref_Id='" + hdnRef.Value + "' and Approval_Id='" + hdnApprovalId.Value + "' and Approval_Name='" + lblApprovalType1.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dt1= objEmpApproval.GetApprovalTransationNew(Session["CompId"].ToString(), hdnRef.Value, lblApprovalType1.Text, hdnApprovalId.Value);
                    int flag = 0;
                    dtPri = new DataView(dt1, "Emp_Id='" + Session["EmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                     
                    if (dtPri.Rows.Count > 0)
                    {
                        flag = 1;
                    }
                    int flag2 = 0;
                    dtPri2 = new DataView(dt1, "Status<>'Pending' ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPri2.Rows.Count > 0)
                    {
                        flag2 = 1;
                    }
                    else
                    {
                        if (ddlApprovalType.SelectedValue != "15")
                        {
                            dtPri2 = new DataView(dt1, "Status<>'Pending' ", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtPri2.Rows.Count > 0)
                            {
                                flag2 = 1;
                            }
                        }
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        DataTable dtTemp = new DataTable();
                        dtTemp.Columns.Add("Emp_Name");
                        dtTemp.Columns.Add("Emp_Id");
                        dtTemp.Columns.Add("Priority");
                        dtTemp.Columns.Add("Status");
                        dtTemp.Columns.Add("Trans_Id");
                        dtTemp.Columns.Add("Date");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["Emp_Name"] = dt1.Rows[i]["Emp_Name"].ToString();
                            dr["Priority"] = dt1.Rows[i]["Priority"].ToString();
                            dr["Status"] = dt1.Rows[i]["Status"].ToString();
                            dr["Trans_Id"] = dt1.Rows[i]["Trans_Id"].ToString();
                            dr["Emp_Id"] = dt1.Rows[i]["Emp_Id"].ToString();
                            if (dt1.Rows[i]["Status_Update_Date"].ToString() != "")
                            {
                                dr["Date"] = Convert.ToDateTime(dt1.Rows[i]["Status_Update_Date"].ToString()).ToString("dd-MMM-yyyy HH:mm");
                            }
                            else
                            {
                                dr["Date"] = "-";
                            }
                            dtTemp.Rows.Add(dr);
                        }
                        dtTemp = new DataView(dtTemp, "", "Priority", DataViewRowState.CurrentRows).ToTable();
                        if (dtTemp.Rows.Count > 0)
                        {
                            //Common Function add By Lokesh on 12-05-2015
                            objPageCmn.FillData((object)gvEmp, dtTemp, "", "");
                            foreach (GridViewRow gr in gvEmp.Rows)
                            {
                                HiddenField hdnTransId = (HiddenField)gr.FindControl("hdnTransId");
                                HiddenField hdnEmpId = (HiddenField)gr.FindControl("hdnGvEmpId");
                                ImageButton imgBtnApprove = (ImageButton)gr.FindControl("imgBtnApprove");
                                ImageButton imgBtnReject = (ImageButton)gr.FindControl("imgBtnReject");
                                ImageButton imgBtnView = (ImageButton)gr.FindControl("imgBtnView");
                                Label lblPriority = (Label)gr.FindControl("lblPriority");
                                Label lblEmpStatus = (Label)gr.FindControl("lblStatus");
                                imgBtnView.Visible = false;
                                if (lblPriority.Text == "True")
                                {
                                    lblRequestStatus.Text = lblEmpStatus.Text;
                                }
                                if (flag2 == 1)
                                {
                                    imgBtnApprove.Visible = false;
                                    imgBtnReject.Visible = false;
                                    imgBtnView.Visible = true;
                                }
                                else
                                {
                                    if (Session["EmpId"].ToString() == hdnEmpId.Value)
                                    {
                                        if (strApprovalType == "Priority")
                                        {
                                            imgBtnApprove.Visible = true;
                                            imgBtnReject.Visible = true;
                                            imgBtnView.Visible = true;
                                        }
                                        //open
                                        else if (strApprovalType == "Hierarchy" && IsOpen == "True")
                                        {
                                            imgBtnApprove.Visible = true;
                                            imgBtnReject.Visible = true;
                                            imgBtnView.Visible = true;
                                        }
                                        //restrcted
                                        else if (strApprovalType == "Hierarchy" && IsOpen == "False")
                                        {
                                            if (new DataView(dtTempData, "Ref_Id=" + hdnRef.Value + " and Trans_Id < " + hdnTransId.Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                                            {
                                                imgBtnApprove.Visible = true;
                                                imgBtnReject.Visible = true;
                                                imgBtnView.Visible = true;
                                            }
                                            else if (new DataView(dtTempData, "Ref_Id=" + hdnRef.Value + " and Trans_Id = " + (Convert.ToInt32(hdnTransId.Value) - 1).ToString() + " and Status<>'Pending' ", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                            {
                                                imgBtnApprove.Visible = true;
                                                imgBtnReject.Visible = true;
                                                imgBtnView.Visible = true;
                                            }
                                            else
                                            {
                                                imgBtnApprove.Visible = false;
                                                imgBtnReject.Visible = false;
                                                imgBtnView.Visible = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        imgBtnApprove.Visible = false;
                                        imgBtnReject.Visible = false;
                                        if (strApprovalType == "Priority")
                                        {
                                            if (flag == 1)
                                            {
                                                imgBtnView.Visible = true;
                                            }
                                            else
                                            {
                                                imgBtnView.Visible = false;
                                            }
                                        }
                                        else if (strApprovalType == "Hierarchy")
                                        {
                                            if (flag == 1)
                                            {
                                                imgBtnView.Visible = true;
                                            }
                                            else if (new DataView(dtTempData, "Ref_Id=" + hdnRef.Value + " and Emp_Id = " + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                            {
                                                if (Convert.ToInt32(new DataView(dtTempData, "Ref_Id=" + hdnRef.Value + " and Emp_Id = " + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) >= Convert.ToInt32(hdnTransId.Value))
                                                {
                                                    imgBtnView.Visible = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        dtTemp.Dispose();
                    }
                }
                dt1.Dispose();
                dtPri.Dispose();
                dtPri2.Dispose();
            }
        }
        dtTempData.Dispose();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dttemp = new DataTable();
        if (Session["Approval"] != null)
        {
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
                DataTable dtAdd = (DataTable)Session["Approval"];
                DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                if (view.ToTable().Rows.Count > 0)
                    lblTotalRecords.Text = "Total Records: " + view.ToTable().Rows.Count.ToString();
                else
                    lblTotalRecords.Text = "Total Records: 0";
                Session["dtFilter_EmpAppr_Mstr"] = view.ToTable();
                if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                {
                    dttemp = view.ToTable();
                    dttemp.Columns["RequestEmp_Name"].ColumnName = "Name";
                    dttemp.Columns["RequestEmp_Code"].ColumnName = "Code";
                    //objPageCmn.FillData((object)gvShiftView, dttemp, "", "");
                }
                else
                {
                    objPageCmn.FillData((object)gvApproval, view.ToTable(), "", "");
                    FillChildGrid();
                }
                AllPageCode();
            }
        }
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid(hdnApprovalType.Value);
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        AllPageCode();
        ddlFieldName.Focus();
    }
    protected void EditRecord(Object sender, GridViewEditEventArgs e)
    {
    }
    protected void gvApproval_OnSorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_EmpAppr_Mstr"];
        if (dt.Rows.Count > 0)
        {
            HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
            DataView dv = new DataView(dt);
            string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
            dv.Sort = Query;
            dt = dv.ToTable();
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvApproval, dt, "", "");
            FillChildGrid();
        }
    }
    protected void gvApproal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvApproval.PageIndex = e.NewPageIndex;
        objPageCmn.FillData((object)gvApproval, (DataTable)Session["dtFilter_EmpAppr_Mstr"], "", "");
        FillChildGrid();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
    }
    public int Approve_ParialLeave(string Ref_Id, string strTrans_Id, string LogStatus, ref SqlTransaction trns)
    {
        //Add On 31-10-2015
        string strPLWithTimeWithOutTime = string.Empty;
        DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPLTimeWithOutTime.Rows.Count > 0)
        {
            strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPLWithTimeWithOutTime = "True";
        }
        string InKey = objAppParam.GetApplicationParameterValueByParamName("In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        string OutKey = objAppParam.GetApplicationParameterValueByParamName("Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);

        string PartialInKey = objAppParam.GetApplicationParameterValueByParamName("Partial Leave In  Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        string PartialOutKey = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Out  Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);


        string strPartialLeaveDate = string.Empty;
        string strActualFromTime = string.Empty;
        string strActualToTime = string.Empty;
        string strEmp_Id = string.Empty;
        int IsApproval = 0;
        int b = 0;
        DataTable dtLog = new DataTable();
        DataTable dt = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
        if (dt.Rows.Count > 0)
        {
            //Old Code
            //MailMessage = "Your Partial Leave has been Approved For Date : '" + dt.Rows[0]["Partial_Leave_Date"].ToString() + "' From Time : '" + dt.Rows[0]["From_Time"].ToString() + "' To Time : '" + dt.Rows[0]["To_Time"].ToString() + "'";
            //ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), AppralMail.ToString(), Passwd.ToString(), "Partial Leave Approved", MailMessage.ToString(), Session["CompId"].ToString(), "");
            strPartialLeaveDate = DateTime.Parse(dt.Rows[0]["Partial_Leave_Date"].ToString()).ToString("dd-MMM-yyyy");
            strActualFromTime = dt.Rows[0]["From_Time"].ToString();
            strActualToTime = dt.Rows[0]["To_Time"].ToString();
            strEmp_Id = dt.Rows[0]["Emp_Id"].ToString();
            dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(strEmp_Id, strPartialLeaveDate, strPartialLeaveDate);
            //New Code Start By Lokesh On 11-03-2015
            // DataTable dtPriority = null;
            // DataTable dtPartialDay = objEmpApproval.GetApprovalTransationbyTransId(Session["CompId"].ToString(), strTrans_Id);
            //// dtPartialDay = new DataView(dtPartialDay, "Ref_Id='" + Ref_Id + "' and Priority='True' and Approval_Id='3'", "", DataViewRowState.CurrentRows).ToTable();
            // if (dtPartialDay.Rows.Count > 0)
            // {
            //     dtPriority = new DataView(dtPartialDay, "Trans_Id='" + strTrans_Id + "' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
            // }
            string strAppMailId = string.Empty;
            string strAppPassword = string.Empty;
            DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
            }
            DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
            }
            string strPLType = string.Empty;
            if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "0")
            {
                strPLType = "Personal";
            }
            else if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "1")
            {
                strPLType = "Official";
            }
            try
            {
                //for (int i = 0; i < dtPartialDay.Rows.Count; i++)
                //{
                //    MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtPartialDay.Rows[i]["Emp_Name"].ToString() + "</h4><hr/><p>Partial Leave(" + strPLType + ") Approved for " + dt.Rows[0]["Emp_Name"].ToString() + " (" + strPartialLeaveDate + ") has been Approved By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strPartialLeaveDate + "', From Time: '" + dt.Rows[0]["From_Time"].ToString() + "' To Time: '" + dt.Rows[0]["To_Time"].ToString() + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                //    ObjSendMailSms.SendApprovalMail(dtPartialDay.Rows[i]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Partial Leave(" + strPLType + ") Approved for " + dt.Rows[0]["Emp_Name"].ToString() + " (Date : " + strPartialLeaveDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns);
                //}
            }
            catch (Exception Ex)
            {
            }
            MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4> <hr /> Your Partial Leave Application Status <br /><br /> Leave Type : Partial Leave(" + strPLType + ")<br /> Leave Date : " + strPartialLeaveDate + " <br /> Approval Status : Approved <br />Approval By : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br />Approval Date : " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat()) + " <br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
            // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Partial Leave(" + strPLType + ") Approved for You (" + strPartialLeaveDate + ") has been Approved By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strPartialLeaveDate + "', From Time: '" + dt.Rows[0]["From_Time"].ToString() + "' To Time: '" + dt.Rows[0]["To_Time"].ToString() + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Partial Leave(" + strPLType + ") Approved for " + dt.Rows[0]["Emp_Name"].ToString() + " (Date : " + strPartialLeaveDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch (Exception Ex)
            {
            }
            //New Code End By Lokesh On 11-03-2015
        }
        int totalminutes = 0;
        int useinday = 0;
        double leaveCount = 0;
        DataTable dtEmpParam = objEmpParam.GetEmployeeParameterByEmpId(dt.Rows[0]["Emp_Id"].ToString(), Session["CompId"].ToString(), ref trns);
        try
        {
            totalminutes = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString());
            useinday = int.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());
            leaveCount = double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Mins"].ToString()) / double.Parse(dtEmpParam.Rows[0]["Partial_Leave_Day"].ToString());
            leaveCount = System.Math.Round(leaveCount);
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "1")
            {
                b = objPartial.PartialLeaveApproveReject(Ref_Id, "Approved", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                if (b != 0)
                {
                    if (strPLWithTimeWithOutTime == "True")
                    {
                        if (LogStatus == "Direct")
                        {
                            DateTime EventTimeIn = new DateTime();
                            if (strActualFromTime != "")
                            {

                                EventTimeIn = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(strActualFromTime).Hour, Convert.ToDateTime(strActualFromTime).Minute, Convert.ToDateTime(strActualFromTime).Second);
                                objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeIn.ToString(), PartialInKey, "In", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            }
                            if (strActualToTime != "")
                            {
                                DateTime EventTimeOut = new DateTime();
                                EventTimeOut = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(strActualToTime).Hour, Convert.ToDateTime(strActualToTime).Minute, Convert.ToDateTime(strActualToTime).Second);
                                objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeOut.ToString(), PartialOutKey, "Out", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                // 

                                //   Get Time Table Information according date and empid   
                                // here will be two case default shift   and assigned shift 
                                DataTable dtSchdule = objDA.return_DataTable("Select * From Att_TimeTable  Where TimeTable_Id IN  (Select TimeTable_Id  From Att_ScheduleDescription Where Emp_Id =  '" + strEmp_Id + "' and  Att_Date = '" + strPartialLeaveDate.ToString() + "'  )", ref trns);
                                if (dtSchdule.Rows.Count > 0)
                                {
                                    // Means we got timetable information 
                                    DateTime dtSchduleTimeOut = Convert.ToDateTime(dtSchdule.Rows[0]["OffDuty_Time"]);
                                    dtSchduleTimeOut = new DateTime(EventTimeOut.Year, EventTimeOut.Month, EventTimeOut.Day, dtSchduleTimeOut.Hour, dtSchduleTimeOut.Minute, dtSchduleTimeOut.Second);

                                    if (EventTimeOut == dtSchduleTimeOut)
                                    {
                                        // Means Out Time and Partial Time Out Same So We need to put one extra entry
                                        objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeOut.AddMinutes(1).ToString(), OutKey, "Out", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                    }
                                    DateTime dtSchduleTimeIn = Convert.ToDateTime(dtSchdule.Rows[0]["OnDuty_Time"]);
                                    dtSchduleTimeIn = new DateTime(EventTimeIn.Year, EventTimeIn.Month, EventTimeIn.Day, dtSchduleTimeIn.Hour, dtSchduleTimeIn.Minute, dtSchduleTimeIn.Second);

                                    if (EventTimeIn == dtSchduleTimeIn)
                                    {
                                        // Means Out Time and Partial Time Out Same So We need to put one extra entry
                                        objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeIn.AddMinutes(-1).ToString(), InKey, "In", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                    }

                                }
                                else
                                {
                                    dtSchdule = objDA.return_DataTable("Select * From Set_ApplicationParameter  Where Company_Id = '2' and Brand_Id='2' and Location_Id ='2' and Param_Name = 'Default_Shift'", ref trns);
                                    if (dtSchdule.Rows.Count > 0)
                                    {
                                        // Means we got default shift information then we need to got time table according to shift parameter

                                    }

                                }

                                //


                            }
                        }
                        else if (LogStatus == "Indirect")
                        {
                            DateTime EventTimeIn = new DateTime();
                            if (txtPLInLog.Text != "")
                            {

                                EventTimeIn = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(txtPLInLog.Text).Hour, Convert.ToDateTime(txtPLInLog.Text).Minute, Convert.ToDateTime(txtPLInLog.Text).Second);
                                if (new DataView(dtLog, "Event_Time1='" + EventTimeIn.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    return 2;
                                }
                                objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeIn.ToString(), PartialInKey, "In", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            }
                            if (txtPLOutLog.Text != "")
                            {
                                DateTime EventTimeOut = new DateTime();
                                EventTimeOut = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(txtPLOutLog.Text).Hour, Convert.ToDateTime(txtPLOutLog.Text).Minute, Convert.ToDateTime(txtPLOutLog.Text).Second);
                                if (new DataView(dtLog, "Event_Time1='" + EventTimeOut.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                {
                                    return 2;
                                }
                                objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeOut.ToString(), PartialOutKey, "Out", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);


                                // 

                                //   Get Time Table Information according date and empid   
                                // here will be two case default shift   and assigned shift 
                                DataTable dtSchdule = objDA.return_DataTable("Select * From Att_TimeTable  Where TimeTable_Id IN  (Select TimeTable_Id  From Att_ScheduleDescription Where Emp_Id =  '" + strEmp_Id + "' and  Att_Date = '" + strPartialLeaveDate.ToString() + "'  )", ref trns);
                                if (dtSchdule.Rows.Count > 0)
                                {
                                    // Means we got timetable information 
                                    DateTime dtSchduleTimeOut = Convert.ToDateTime(dtSchdule.Rows[0]["OffDuty_Time"]);
                                    dtSchduleTimeOut = new DateTime(EventTimeOut.Year, EventTimeOut.Month, EventTimeOut.Day, dtSchduleTimeOut.Hour, dtSchduleTimeOut.Minute, dtSchduleTimeOut.Second);

                                    if (EventTimeOut == dtSchduleTimeOut)
                                    {
                                        // Means Out Time and Partial Time Out Same So We need to put one extra entry
                                        objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeOut.AddMinutes(1).ToString(), OutKey, "Out", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                    }

                                    DateTime dtSchduleTimeIn = Convert.ToDateTime(dtSchdule.Rows[0]["OnDuty_Time"]);
                                    dtSchduleTimeIn = new DateTime(EventTimeIn.Year, EventTimeIn.Month, EventTimeIn.Day, dtSchduleTimeIn.Hour, dtSchduleTimeIn.Minute, dtSchduleTimeIn.Second);

                                    if (EventTimeIn == dtSchduleTimeIn)
                                    {
                                        // Means Out Time and Partial Time Out Same So We need to put one extra entry
                                        objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeIn.AddMinutes(-1).ToString(), InKey, "In", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);

                                    }

                                }
                                else
                                {
                                    dtSchdule = objDA.return_DataTable("Select * From Set_ApplicationParameter  Where Company_Id = '2' and Brand_Id='2' and Location_Id ='2' and Param_Name = 'Default_Shift'", ref trns);
                                    if (dtSchdule.Rows.Count > 0)
                                    {
                                        // Means we got default shift information then we need to got time table according to shift parameter

                                    }

                                }

                                //

                            }
                        }
                    }
                    DisplayMessage("Partial Leave Approved");
                    txtPLInLog.Text = "";
                    txtPLOutLog.Text = "";
                    GvEmp_Log.DataSource = null;
                    GvEmp_Log.DataBind();
                    // FillLeaveStatus();
                }
            }
            else
            {
                DateTime Requestdate = Convert.ToDateTime(dt.Rows[0]["Partial_Leave_Date"].ToString());
                DateTime StartDate = new DateTime(Requestdate.Year, Requestdate.Month, 1, 0, 0, 0);
                int TotalDays = DateTime.DaysInMonth(Requestdate.Year, Requestdate.Month);
                DateTime EndDate = new DateTime(Requestdate.Year, Requestdate.Month, TotalDays, 23, 59, 1);
                // DataTable DtFilter = objPartial.GetPartialLeaveRequest(Session["CompId"].ToString());
                DataTable DtFilter = objDA.return_DataTable("select isnull( COUNT(*),0) from Att_PartialLeave_Request where Emp_Id= " + dt.Rows[0]["Emp_Id"].ToString() + " and Is_Confirmed='Approved' and Partial_Leave_Type='0' and CAST( Partial_Leave_Date as date)=cast( '" + StartDate + "' as date)", ref trns);
                //try
                //{
                //    DtFilter = new DataView(DtFilter, "Partial_Leave_Date>='" + StartDate.ToString() + "' and Partial_Leave_Date<='" + EndDate.ToString() + "' and Emp_Id=" + dt.Rows[0]["Emp_Id"].ToString() + " and Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //catch
                //{
                //}
                if (DtFilter.Rows.Count == leaveCount)
                {
                    IsApproval = 1;
                    //DisplayMessage("You Can Not Approve Greater than Assigned Leave");
                    //return;
                }
                else
                {
                    b = objPartial.PartialLeaveApproveReject(Ref_Id, "Approved", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                    if (b != 0)
                    {
                        if (strPLWithTimeWithOutTime == "True")
                        {
                            if (LogStatus == "Direct")
                            {
                                if (strActualFromTime != "")
                                {
                                    DateTime EventTimeIn = new DateTime();
                                    EventTimeIn = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(strActualFromTime).Hour, Convert.ToDateTime(strActualFromTime).Minute, Convert.ToDateTime(strActualFromTime).Second);
                                    objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeIn.ToString(), PartialInKey, "In", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                                if (strActualToTime != "")
                                {
                                    DateTime EventTimeOut = new DateTime();
                                    EventTimeOut = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(strActualToTime).Hour, Convert.ToDateTime(strActualToTime).Minute, Convert.ToDateTime(strActualToTime).Second);
                                    objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeOut.ToString(), PartialOutKey, "Out", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                            }
                            else if (LogStatus == "Indirect")
                            {
                                if (txtPLInLog.Text != "")
                                {
                                    DateTime EventTimeIn = new DateTime();
                                    EventTimeIn = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(txtPLInLog.Text).Hour, Convert.ToDateTime(txtPLInLog.Text).Minute, Convert.ToDateTime(txtPLInLog.Text).Second);
                                    if (new DataView(dtLog, "Event_Time1='" + EventTimeIn.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                    {
                                        return 2;
                                    }
                                    objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeIn.ToString(), PartialInKey, "In", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                                if (txtPLOutLog.Text != "")
                                {
                                    DateTime EventTimeOut = new DateTime();
                                    EventTimeOut = new DateTime(objSys.getDateForInput(strPartialLeaveDate).Year, objSys.getDateForInput(strPartialLeaveDate).Month, objSys.getDateForInput(strPartialLeaveDate).Day, Convert.ToDateTime(txtPLOutLog.Text).Hour, Convert.ToDateTime(txtPLOutLog.Text).Minute, Convert.ToDateTime(txtPLOutLog.Text).Second);
                                    if (new DataView(dtLog, "Event_Time1='" + EventTimeOut.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                                    {
                                        return 2;
                                    }
                                    objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strPartialLeaveDate.ToString(), EventTimeOut.ToString(), PartialOutKey, "Out", "By Partial Leave", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                            }
                        }
                        DisplayMessage("Partial Leave Approved");
                        txtPLInLog.Text = "";
                        txtPLOutLog.Text = "";
                        GvEmp_Log.DataSource = null;
                        GvEmp_Log.DataBind();
                        // FillLeaveStatus();
                    }
                }
            }
        }
        return IsApproval;
    }
    public int getCurrentMonth(DateTime applydate)
    {
        int useminutes = 0;
        DataTable dt = objPartial.GetPartialLeaveRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), ViewState["Emp_Id"].ToString(), applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());
            }
        }
        else
        {
            useminutes = 0;
        }
        return useminutes;
    }
    public int getCurrentMonthLeaveCount(DateTime applydate)
    {
        int Count = 0;
        DataTable dt = objPartial.GetPartialLeaveRequestByEmpIdAndCurrentMonthYear(Session["CompId"].ToString(), ViewState["Emp_Id"].ToString(), applydate.Month.ToString(), applydate.Year.ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Count++;
            }
        }
        else
        {
            Count = 0;
        }
        return Count;
    }
    public int getMinuteInADay(DateTime applydate)
    {
        int useminutes = 0;
        DataTable dt = objPartial.GetPartialLeaveRequestById(Session["CompId"].ToString(), ViewState["Emp_Id"].ToString());
        dt = new DataView(dt, "Is_Confirmed='Approved' and Partial_Leave_Type='0' and Partial_Leave_Date='" + applydate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                useminutes += GetMinuteDiff(dt.Rows[i]["To_Time"].ToString(), dt.Rows[i]["From_Time"].ToString());
            }
        }
        else
        {
            useminutes = 0;
        }
        return useminutes;
    }
    private int GetMinuteDiff(string greatertime, string lesstime)
    {
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);
        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);
        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }
    public bool Approve_HalfDay(string Ref_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        bool Result = true;
        string InKey = objAppParam.GetApplicationParameterValueByParamName("In Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        string OutKey = objAppParam.GetApplicationParameterValueByParamName("Out Func Key", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), ref trns);
        int b = 0;
        b = objHalfDay.HalfDayApproveReject(Ref_Id, "Approved", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        int RemainHalfDay = 0;
        int PendingHalfDay = 0;
        int UsedDay = 0;
        string year = string.Empty;
        int month = 0;
        string strHalfDayDate = string.Empty;
        DataTable dt = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
        if (dt.Rows.Count > 0)
        {
            strHalfDayDate = DateTime.Parse(dt.Rows[0]["HalfDay_Date"].ToString()).ToString("dd-MMM-yyyy");
            //New Code Start By Lokesh On 11-03-2015
            DataTable dtPriority = null;
            DataTable dtHalfDay = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
            dtHalfDay = new DataView(dtHalfDay, "Ref_Id='" + Ref_Id + "' and Priority='True' and Approval_Id='2'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtHalfDay.Rows.Count > 0)
            {
                dtPriority = new DataView(dtHalfDay, "Trans_Id='" + strTrans_Id + "' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            string strAppMailId = string.Empty;
            string strAppPassword = string.Empty;
            DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
            }
            DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
            }
            //MailMessage = "'" + dt.Rows[0]["HalfDay_Type"].ToString() + "' Half Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' On Date : '" + strHalfDayDate + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "'";
            try
            {
                //for (int i = 0; i < dtHalfDay.Rows.Count; i++)
                //{
                //    MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtHalfDay.Rows[i]["Emp_Name"].ToString() + "</h4><hr/><p>Half Day Leave(" + dt.Rows[0]["HalfDay_Type"].ToString() + ") has been Approved By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strHalfDayDate + "' For '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "' </p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                //    ObjSendMailSms.SendApprovalMail(dtHalfDay.Rows[i]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Half Day('" + dt.Rows[0]["HalfDay_Type"].ToString() + "')Approved For " + dt.Rows[0]["Emp_Name"].ToString() + "(Date:" + strHalfDayDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns);
                //}
            }
            catch (Exception Ex)
            {
            }
            ////Old
            //MailMessage = "Your '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' Half Day Leave has been Approved for Date : '" + dt.Rows[0]["HalfDay_Date"].ToString() + "'";
            //ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), AppralMail.ToString(), Passwd.ToString(), "Half Day Leave Approved", MailMessage.ToString(), Session["CompId"].ToString(), "");
            //MailMessage = "Your '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' Half Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' for Date : '" + strHalfDayDate + "'";
            MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4> <hr /> Your Half Day Leave Application Status <br /><br /> Leave Type : Half Day (" + dt.Rows[0]["HalfDay_Type"].ToString() + ")<br /> Leave Date : " + strHalfDayDate + " <br /> Approval Status : Approved <br />Approval By : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br />Approval Date : " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat()) + " <br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
            //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Your Half Day Leave(" + dt.Rows[0]["HalfDay_Type"].ToString() + ") has been Approved By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strHalfDayDate + "' For '" + dt.Rows[0]["HalfDay_Type"].ToString() + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Half Day('" + dt.Rows[0]["HalfDay_Type"].ToString() + "')Approved For " + dt.Rows[0]["Emp_Name"].ToString() + "(Date:" + strHalfDayDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch (Exception Ex)
            {
            }
            //New Code End By Lokesh On 11-03-2015
            month = Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Month;
        }
        DataTable dtApp = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dtApp.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dtApp.Rows[0]["Param_Value"].ToString());
        }
        if (month >= FinancialYearMonth)
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year).ToString();
        }
        else
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year - 1).ToString();
        }
        DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(dt.Rows[0]["Emp_Id"].ToString(), year, ref trns);
        if (dtEmpHalf.Rows.Count > 0)
        {
            UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
            PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
            RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
            PendingHalfDay = PendingHalfDay - 1;
            UsedDay = UsedDay + 1;
        }
        objEmpHalfDay.UpdateEmployeeHalfDayTransaction(Session["CompId"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dtEmpHalf.Rows[0]["Year"].ToString(), "0", UsedDay.ToString(), RemainHalfDay.ToString(), PendingHalfDay.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        //here we are checking that log exists or not for same time
        DataTable dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(dt.Rows[0]["Emp_Id"].ToString(), strHalfDayDate, strHalfDayDate);
        //For Half Day Logs
        string strEmp_Id = dt.Rows[0]["Emp_Id"].ToString();
        if (txtHLInLog.Text != "")
        {
            DateTime EventTimeIn = new DateTime();
            EventTimeIn = new DateTime(objSys.getDateForInput(strHalfDayDate).Year, objSys.getDateForInput(strHalfDayDate).Month, objSys.getDateForInput(strHalfDayDate).Day, Convert.ToDateTime(txtHLInLog.Text).Hour, Convert.ToDateTime(txtHLInLog.Text).Minute, Convert.ToDateTime(txtHLInLog.Text).Second);
            if (new DataView(dtLog, "Event_Time1='" + EventTimeIn.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                return false;
            }
            objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strHalfDayDate.ToString(), EventTimeIn.ToString(), InKey, "In", "By Half Day", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        }
        if (txtHLOutLog.Text != "")
        {
            DateTime EventTimeOut = new DateTime();
            EventTimeOut = new DateTime(objSys.getDateForInput(strHalfDayDate).Year, objSys.getDateForInput(strHalfDayDate).Month, objSys.getDateForInput(strHalfDayDate).Day, Convert.ToDateTime(txtHLOutLog.Text).Hour, Convert.ToDateTime(txtHLOutLog.Text).Minute, Convert.ToDateTime(txtHLOutLog.Text).Second);
            if (new DataView(dtLog, "Event_Time1='" + EventTimeOut.ToString("HH:mm") + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                return false;
            }
            objAttLog.InsertAttendanceLog(Session["CompId"].ToString(), strEmp_Id, "0", strHalfDayDate.ToString(), EventTimeOut.ToString(), OutKey, "Out", "By Half Day", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        }
        if (b != 0)
        {
            DisplayMessage("Request Approved");
            txtHLInLog.Text = "";
            txtHLOutLog.Text = "";
            gvHLLog.DataSource = null;
            gvHLLog.DataBind();
        }
        return Result;
    }
    protected void HierarchysendEmailForFullDayLeave(string Ref_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        //New Code Start By Lokesh On 11-03-2015
        DataTable dtFullDay = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
        DataTable dtleaveRquestDetail = objDA.return_DataTable("select Trans_id,Leave_Type_Id,From_Date,To_Date from Att_Leave_Request where Field2='" + Ref_Id + "'", ref trns);
        string strFromDate = DateTime.Parse(dtleaveRquestDetail.Rows[0]["From_Date"].ToString()).ToString("dd-MMM-yyyy");
        string strToDate = DateTime.Parse(dtleaveRquestDetail.Rows[0]["To_Date"].ToString()).ToString("dd-MMM-yyyy");
        DataTable dt = objleaveReq.GetLeaveRequest(Session["CompId"].ToString(), ref trns);
        dt = new DataView(dt, "Trans_Id='" + dtleaveRquestDetail.Rows[0]["Trans_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        dtFullDay = new DataView(dtFullDay, "Trans_Id = " + (float.Parse(strTrans_Id) + 1).ToString() + " and Ref_id=" + Ref_Id + " and Approval_Id='1'", "Trans_Id", DataViewRowState.CurrentRows).ToTable();
        if (dtFullDay.Rows.Count > 0)
        {
            MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + dtFullDay.Rows[0]["Emp_Name"].ToString() + "</h4> <hr /> Find below the pending leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(dtFullDay.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(dtFullDay.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(dtFullDay.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(dtFullDay.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> " + Common.GetmailContentByLeaveTypeId(Ref_Id, dtFullDay.Rows[0]["request_emp_id"].ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString()) + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
            // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtFullDay.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Full Day Leave has been Approved by '" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "' For '" + Common.GetEmployeeName(dtFullDay.Rows[0]["request_emp_id"].ToString()) + "' <br />From : '" + strFromDate + "' To : '" + strToDate + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dtFullDay.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Full Day Leave Approved For " + Common.GetEmployeeName(dtFullDay.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch
            {
            }
            MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(dtFullDay.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Your Leave Application Status " + Common.GetmailContentByLeaveTypeId(Ref_Id, dtFullDay.Rows[0]["request_emp_id"].ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString()) + "<br /> Approval Status : Approved <br />Approval By : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br />Approval Date : " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat()) + " <br />Pending Approval : " + dtFullDay.Rows[0]["Emp_Name"].ToString() + " <br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
            //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(dtFullDay.Rows[0]["request_emp_id"].ToString()) + "</h4><hr/><p>Your Full Day Leave has been Approved by '" + Common.GetEmployeeName(Session["EmpId"].ToString()) + "' and now it has gone to '"+dtFullDay.Rows[0]["Emp_Name"].ToString()+"' for further approval<br />From : '" + strFromDate + "' To : '" + strToDate + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Full Day Leave Approved by " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch
            {
            }
        }
        //MailMessage = "Your Full Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' From : '" + strFromDate + "' To : '" + strToDate + "'";
    }
    protected void HierarchysendEmailForHalfDayLeave(string Ref_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        //New Code Start By Lokesh On 11-03-2015
        DataTable dtHalfDay = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
        DataTable dt = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
        int b = 0;
        string strHalfDayDate = string.Empty;
        if (dt.Rows.Count > 0)
        {
            strHalfDayDate = DateTime.Parse(dt.Rows[0]["HalfDay_Date"].ToString()).ToString("dd-MMM-yyyy");
        }
        dtHalfDay = new DataView(dtHalfDay, "Trans_Id = " + (float.Parse(strTrans_Id) + 1).ToString() + " and Ref_id=" + Ref_Id + " and Approval_Id='2'", "Trans_Id", DataViewRowState.CurrentRows).ToTable();
        if (dtHalfDay.Rows.Count > 0)
        {
            MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Half Day leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(dtHalfDay.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(dtHalfDay.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(dtHalfDay.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Half Day (" + dt.Rows[0]["HalfDay_Type"].ToString() + ")<br />Leave Date : " + strHalfDayDate + "<br /> Reason For Leave : " + dt.Rows[0]["Description"].ToString() + " <br />  <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
            // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Emp_Id"].ToString()) + "</h4><hr/><p>Half Day Leave(" + dt.Rows[0]["HalfDay_Type"].ToString() + ") has been Approved By " + Common.GetEmployeeName(Session["EmpId"].ToString()) + "<br />On Date '" + strHalfDayDate + "' For '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' For '" + Common.GetEmployeeName(dtHalfDay.Rows[0]["Request_Emp_Id"].ToString()) + "' </p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dtHalfDay.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Half Day('" + dt.Rows[0]["HalfDay_Type"].ToString() + "')Approved For " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "(Date:" + strHalfDayDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch
            {
            }
            MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Your Half Day Leave Application Status <br /><br /> Leave Type : Half Day (" + dt.Rows[0]["HalfDay_Type"].ToString() + ")<br /> Leave Date : " + strHalfDayDate + " <br /> Approval Status : Approved <br />Approval By : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br />Approval Date : " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat()) + " <br />Pending Approval : " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
            //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(dtHalfDay.Rows[0]["Emp_Id"].ToString()) + "</h4><hr/><p>Your Half Day Leave(" + dt.Rows[0]["HalfDay_Type"].ToString() + ") has been Approved By " + Common.GetEmployeeName(Session["EmpId"].ToString()) + "<br />On Date '" + strHalfDayDate + "' For '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' and now it has gone to '" + Common.GetEmployeeName(dtHalfDay.Rows[0]["Emp_Id"].ToString()) + "' for further approval</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dtHalfDay.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Half Day('" + dt.Rows[0]["HalfDay_Type"].ToString() + "')Approved by " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "(Date:" + strHalfDayDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch (Exception Ex)
            {
            }
        }
        //MailMessage = "Your Full Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' From : '" + strFromDate + "' To : '" + strToDate + "'";
    }
    protected void HierarchysendEmailForPartialLeave(string Ref_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        string strPartialLeaveDate = string.Empty;
        string strActualFromTime = string.Empty;
        string strActualToTime = string.Empty;
        DataTable dtPartialLeavedetail = new DataTable();
        DataTable dtApproval = new DataTable();
        dtApproval = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
        dtPartialLeavedetail = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
        if (dtPartialLeavedetail.Rows.Count > 0)
        {
            string strPLType = string.Empty;
            if (dtPartialLeavedetail.Rows[0]["Partial_Leave_Type"].ToString() == "0")
            {
                strPLType = "Personal";
            }
            else if (dtPartialLeavedetail.Rows[0]["Partial_Leave_Type"].ToString() == "1")
            {
                strPLType = "Official";
            }
            strPartialLeaveDate = DateTime.Parse(dtPartialLeavedetail.Rows[0]["Partial_Leave_Date"].ToString()).ToString("dd-MMM-yyyy");
            strActualFromTime = dtPartialLeavedetail.Rows[0]["From_Time"].ToString();
            strActualToTime = dtPartialLeavedetail.Rows[0]["To_Time"].ToString();
            dtApproval = new DataView(dtApproval, "Trans_Id = " + (float.Parse(strTrans_Id) + 1).ToString() + " and Ref_id=" + Ref_Id + " and Approval_Id='3'", "Trans_Id", DataViewRowState.CurrentRows).ToTable();
            if (dtApproval.Rows.Count > 0)
            {
                MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(dtApproval.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Find below the pending Partial leave application for your Approval <br /><br /> Employee Id : '" + Common.GetEmployeeCode(dtApproval.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(dtApproval.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(dtApproval.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(dtApproval.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Leave Type : Partial Leave<br />Leave Date : " + strPartialLeaveDate + "<br />From Time :" + dtPartialLeavedetail.Rows[0]["From_Time"].ToString() + "  To Time :" + dtPartialLeavedetail.Rows[0]["To_Time"].ToString() + "<br /> Reason For Leave : " + txtDescription.Text + " <br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
                // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(dtApproval.Rows[0]["Emp_Id"].ToString()) + "</h4><hr/><p>Partial Leave(" + strPLType + ") Approved for " + Common.GetEmployeeName(dtApproval.Rows[0]["Request_Emp_Id"].ToString()) + " (" + strPartialLeaveDate + ") has been Approved By " + Common.GetEmployeeName(Session["EmpId"].ToString()) + "<br />On Date '" + strPartialLeaveDate + "', From Time: '" + dtPartialLeavedetail.Rows[0]["From_Time"].ToString() + "' To Time: '" + dtPartialLeavedetail.Rows[0]["To_Time"].ToString() + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                try
                {
                    ObjSendMailSms.SendApprovalMail(dtApproval.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Partial Leave(" + strPLType + ") Approved for " + Common.GetEmployeeName(dtApproval.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " (Date : " + strPartialLeaveDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
                catch
                {
                }
                MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(dtApproval.Rows[0]["Request_Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "</h4> <hr /> Your Partial Leave Application Status <br /><br /> Leave Type : Partial Leave(" + strPLType + ")<br /> Leave Date : " + strPartialLeaveDate + " <br /> Approval Status : Approved <br />Approval By : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br />Approval Date : " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat()) + " <br />Pending Approval : " + Common.GetEmployeeName(dtApproval.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
                // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(dtApproval.Rows[0]["Request_Emp_Id"].ToString()) + "</h4><hr/><p>Partial Leave(" + strPLType + ")  has been Approved By " + Common.GetEmployeeName(Session["EmpId"].ToString()) + "<br />On Date '" + strPartialLeaveDate + "', From Time: '" + dtPartialLeavedetail.Rows[0]["From_Time"].ToString() + "' To Time: '" + dtPartialLeavedetail.Rows[0]["To_Time"].ToString() + " and now it has gone to '" + Common.GetEmployeeName(dtApproval.Rows[0]["Emp_Id"].ToString()) + "' for further approval'<br /></p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                try
                {
                    ObjSendMailSms.SendApprovalMail(dtApproval.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), Common.GetMasterEmailPassword(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Time Man:Partial Leave(" + strPLType + ") Approved by " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " (Date : " + strPartialLeaveDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
                catch
                {
                }
            }
        }
    }
    public string GetScheduleType(object Date, object EmpId, object leavetypeid)
    {
        string ScheduleType = string.Empty;
        string Date1 = Date.ToString();
        string year = Convert.ToDateTime(Date1).Year.ToString();
        string empid = EmpId.ToString();
        string leaveId = leavetypeid.ToString();
        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        year = string.Empty;
        if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
        {
            year = FinancialYearStartDate.Year.ToString();
        }
        else
        {
            year = FinancialYearStartDate.Year.ToString();
        }
        DataTable dtLeave = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(empid);
        dtLeave = new DataView(dtLeave, "Year='" + year + "' and Leave_Type_Id='" + leaveId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtLeave.Rows.Count > 0)
        {
            if (dtLeave.Rows[0]["Month"].ToString() == "0")
            {
                ScheduleType = "Yearly";
            }
            else
            {
                ScheduleType = "Monthly";
            }
        }
        return ScheduleType;
    }
    public string GetLeaveStatus(object TransId)
    {
        string status = string.Empty;
        DataTable dt = objleaveReq.GetLeaveRequest(Session["CompId"].ToString());
        dt = new DataView(dt, "Trans_Id='" + TransId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dt.Rows[0]["Is_Pending"].ToString()))
            {
                status = "Pending";
            }
            else if (Convert.ToBoolean(dt.Rows[0]["Is_Approved"].ToString()))
            {
                status = "Approved";
            }
            else if (Convert.ToBoolean(dt.Rows[0]["Is_Canceled"].ToString()))
            {
                status = "Rejected";
            }
        }
        return status;
    }
    public string GetOTStatus(object TransId)
    {
        string status = string.Empty;
        DataTable dt = objOverTimeReq.GetOvertimeRequestByTransId(Session["CompId"].ToString(), TransId.ToString());
        if (dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dt.Rows[0]["Is_Pending"].ToString()))
            {
                status = "Pending";
            }
            else if (Convert.ToBoolean(dt.Rows[0]["Is_Approved"].ToString()))
            {
                status = "Approved";
            }
            else if (Convert.ToBoolean(dt.Rows[0]["Is_Canceled"].ToString()))
            {
                status = "Rejected";
            }
        }
        return status;
    }
    protected void View_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
        string EmpId = GetEmpIdByUserId();
        hdnTransId.Value = e.CommandArgument.ToString();
        txtDescription.Text = "";
        txtPLInLog.Text = "";
        txtPLOutLog.Text = "";
        txtHLInLog.Text = "";
        txtHLOutLog.Text = "";
        txtDescription.Enabled = true;
        pnlEmailMarketing.Visible = false;
        //DataTable dt = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
        DataTable dt = objEmpApproval.GetApprovalTransationNewWithTransId(Session["CompId"].ToString(), hdnTransId.Value);
        //dt = new DataView(dt, "Trans_Id='" + hdnTransId.Value + "' ", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "1")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlHalfDay.Visible = false;
                pnlLeave.Visible = true;
                pnlClaim.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlLoan.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtleaveDetail = objDA.return_DataTable("select * from Att_Leave_Request where Field2=" + dt.Rows[0]["Ref_Id"].ToString() + "");
                if (dtleaveDetail.Rows.Count > 0)
                {
                    //This Code Add By Rahul Sharma on date 17-11-2023
                    lblEmpCode.Text = Common.GetEmployeeCode(dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    lblEmpName.Text = Common.GetEmployeeName(dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    DataTable leaveTrans = new DataTable();
                    leaveTrans = objEmpApproval.GetLeaveTrans(dt.Rows[0]["Ref_Id"].ToString(), dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                    objPageCmn.FillData((object)gvLeaveView, leaveTrans, "", "");
                    //lblEmpCode.Text = Common.GetEmployeeCode(dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    //lblEmpName.Text = Common.GetEmployeeName(dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    //ltrLeaveDetail.Text = Common.GetmailContentByLeaveTypeId(dt.Rows[0]["Ref_Id"].ToString(), dtleaveDetail.Rows[0]["Emp_Id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "4")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlHalfDay.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = true;
                pnlLoan.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtClaim = new DataTable();
                dtClaim = objPayClaim.GetRecord_From_PayEmployeeClaimByClaimId(dt.Rows[0]["Ref_Id"].ToString());
                if (dtClaim.Rows.Count > 0)
                {
                    //lblClaimId.Text = dtClaim.Rows[0]["Claim_Id"].ToString();
                    lblClaimName.Text = dtClaim.Rows[0]["Claim_Name"].ToString();
                    lblClaimDescription.Text = dtClaim.Rows[0]["Claim_Description"].ToString();
                    System.Globalization.DateTimeFormatInfo mfi = new System.Globalization.DateTimeFormatInfo();
                    string strMonthName = mfi.GetMonthName(Convert.ToInt32(dtClaim.Rows[0]["Claim_Month"].ToString())).ToString();
                    lblClaimMonth.Text = strMonthName;
                    lblClaimYear.Text = dtClaim.Rows[0]["Claim_Year"].ToString();
                    lblClaimReqDate.Text = Convert.ToDateTime(dtClaim.Rows[0]["Claim_Req_Date"].ToString()).ToString("dd-MMM-yyyy HH:mm");
                    lblClaimStatus.Text = dtClaim.Rows[0]["Claim_Approved"].ToString();
                    if (dtClaim.Rows[0]["Value_Type"].ToString() == "1")
                    {
                        lblClaimType.Text = "Fixed";
                    }
                    else
                    {
                        lblClaimType.Text = "Percentage";
                    }
                    lblClaimValue.Text = String.Format("{0:0.00}", dtClaim.Rows[0]["Value"]);
                    lblClaimDescription.Text = dtClaim.Rows[0]["Claim_Description"].ToString();
                    lblClaimEmpCode.Text = dtClaim.Rows[0]["Emp_Code"].ToString();
                    lblClaimEmpName.Text = dtClaim.Rows[0]["Emp_Name"].ToString();
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "5")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlHalfDay.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = true;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtLoan = objLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString().Trim(), "");
                if (dtLoan.Rows.Count > 0)
                {
                    //lblLoanId.Text = dtLoan.Rows[0]["Loan_Id"].ToString();
                    lblLoanName.Text = dtLoan.Rows[0]["Loan_Name"].ToString();
                    lblLoanEmpCode.Text = dtLoan.Rows[0]["Emp_Code"].ToString();
                    lblLoanEmpName.Text = dtLoan.Rows[0]["Emp_Name"].ToString();
                    lblLoanAmount.Text = String.Format("{0:0.00}", dtLoan.Rows[0]["Loan_Amount"]);
                    lblLoanInterest.Text = dtLoan.Rows[0]["Loan_Interest"].ToString() + "%";
                    lblLoanDuration.Text = dtLoan.Rows[0]["Loan_Duration"].ToString();
                    lblLoanGrossAmount.Text = String.Format("{0:0.00}", dtLoan.Rows[0]["Gross_Amount"]);
                    try
                    {
                        if (Convert.ToDateTime(dtLoan.Rows[0]["Loan_Request_Date"].ToString()).ToString("dd/MM/yyyy") == "01/01/1990")
                        {
                            lblLoanRequestDate.Text = "NA";
                        }
                        else
                        {
                            lblLoanRequestDate.Text = Convert.ToDateTime(dtLoan.Rows[0]["Loan_Request_Date"].ToString()).ToString(objSys.SetDateFormat());
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (Convert.ToDateTime(dtLoan.Rows[0]["Loan_Approval_Date"].ToString()).ToString("dd/MM/yyyy") == "01/01/1990")
                        {
                            lblLoanApprovalDate.Text = "NA";
                        }
                        else
                        {
                            lblLoanApprovalDate.Text = Convert.ToDateTime(dtLoan.Rows[0]["Loan_Approval_Date"].ToString()).ToString(objSys.SetDateFormat());
                        }
                    }
                    catch
                    {
                    }
                    lblLoanStatus.Text = dtLoan.Rows[0]["Is_Status"].ToString();
                    lblLoanInstallment.Text = String.Format("{0:0.00}", dtLoan.Rows[0]["Monthly_Installment"]);
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "2")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = true;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtHalfDay = new DataTable();
                dtHalfDay = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                if (dtHalfDay.Rows.Count > 0)
                {
                    lblHalfDayLeaveType.Text = dtHalfDay.Rows[0]["HalfDay_Type"].ToString();
                    lblHalfDayApplyDate.Text = Convert.ToDateTime(dtHalfDay.Rows[0]["HalfDay_Date"].ToString()).ToString("dd-MMM-yyyy"); ;
                    lblHalfDayReqDAte.Text = Convert.ToDateTime(dtHalfDay.Rows[0]["Request_Date_Time"].ToString()).ToString("dd-MMM-yyyy HH:mm");
                    lblHalfDayDescription.Text = dtHalfDay.Rows[0]["Description"].ToString();
                    lblHalfDAyEmpcode.Text = dtHalfDay.Rows[0]["Emp_Code"].ToString();
                    lblHalfdayEmpName.Text = dtHalfDay.Rows[0]["Emp_Name"].ToString();
                    lblHalfDAyStatus.Text = dtHalfDay.Rows[0]["Is_Confirmed"].ToString();
                    if (lblHalfDAyStatus.Text == "Approved")
                    {
                        div_InLog.Visible = false;
                        div_OutLog.Visible = false;
                    }
                    else
                    {
                        div_InLog.Visible = true;
                        div_OutLog.Visible = true;
                    }
                    //Code for Log
                    string strEmp_Id = dtHalfDay.Rows[0]["Emp_Id"].ToString();
                    //DataTable dtLOg = objAttLog.GetAttendanceLog(Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    DataTable dtLOg = objEmpApproval.GetAttendanceLog(Session["CompId"].ToString(), strEmp_Id);
                    DateTime Fromdate = new DateTime(DateTime.Parse(lblHalfDayApplyDate.Text).Year, DateTime.Parse(lblHalfDayApplyDate.Text).Month, DateTime.Parse(lblHalfDayApplyDate.Text).Day);
                    try
                    {
                        dtLOg = new DataView(dtLOg, "Event_Date='" + Fromdate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                    if (dtLOg.Rows.Count > 0)
                    {
                        gvHLLog.DataSource = dtLOg;
                        gvHLLog.DataBind();
                    }
                    else
                    {
                        gvHLLog.DataSource = null;
                        gvHLLog.DataBind();
                    }
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "3")
            {
                div_Pl_Request.Visible = false;
                //Add On 31-10-2015
                string strPLWithTimeWithOutTime = string.Empty;
                DataTable dtPLTimeWithOutTime = objAppParam.GetApplicationParameterByCompanyId("PLTimeWithOutTime", Session["CompId"].ToString());
                dtPLTimeWithOutTime = new DataView(dtPLTimeWithOutTime, "Param_Name='PLTimeWithOutTime'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtPLTimeWithOutTime.Rows.Count > 0)
                {
                    strPLWithTimeWithOutTime = dtPLTimeWithOutTime.Rows[0]["Param_Value"].ToString();
                }
                else
                {
                    strPLWithTimeWithOutTime = "True";
                }
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = true;
                pnlLeaveSalary.Visible = false;
                //if (strPLWithTimeWithOutTime == "True")
                //{
                if (dt.Rows[0]["Priority"].ToString() == "True")
                {
                    trInsertLog.Visible = true;
                }
                else
                {
                    trInsertLog.Visible = false;
                }
                trLogDetail.Visible = true;
                //}
                //else if (strPLWithTimeWithOutTime == "False")
                //{
                //    trInsertLog.Visible = false;
                //    trLogDetail.Visible = false;
                //}
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlPaymentApproval.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                DataTable dtPartialLeave = new DataTable();
                dtPartialLeave = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                if (dtPartialLeave.Rows.Count > 0)
                {
                    lblPartialLeaveId.Text = dtPartialLeave.Rows[0]["Partial_Leave_Type"].ToString();
                    string LeaveId = dtPartialLeave.Rows[0]["Partial_Leave_Type"].ToString();
                    if (LeaveId == "0")
                    {
                        lblPartialLeaveName.Text = "Personal";
                    }
                    else if (LeaveId == "1")
                    {
                        lblPartialLeaveName.Text = "Official";
                        div_Pl_Request.Visible = true;
                    }
                    else
                    {
                        lblPartialLeaveName.Text = string.Empty;
                    }
                    lblPartialLeaveDate.Text = Convert.ToDateTime(dtPartialLeave.Rows[0]["Partial_Leave_Date"].ToString()).ToString("dd-MMM-yyyy"); ;
                    lblRequestDate.Text = Convert.ToDateTime(dtPartialLeave.Rows[0]["Request_Date_Time"].ToString()).ToString("dd-MMM-yyyy HH:mm");
                    //For Log Work On 30-10-2015
                    //if (strPLWithTimeWithOutTime == "True")
                    //{
                    string strEmp_Id = dtPartialLeave.Rows[0]["Emp_Id"].ToString();
                    //DataTable dtLOg = objAttLog.GetAttendanceLog(Session["CompId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
                    DataTable dtLOg = objEmpApproval.GetAttendanceLog(Session["CompId"].ToString(), strEmp_Id);
                    DateTime Fromdate = new DateTime(DateTime.Parse(lblPartialLeaveDate.Text).Year, DateTime.Parse(lblPartialLeaveDate.Text).Month, DateTime.Parse(lblPartialLeaveDate.Text).Day);
                    try
                    {
                        dtLOg = new DataView(dtLOg, "Event_Date='" + Fromdate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    GvEmp_Log.DataSource = dtLOg;
                    GvEmp_Log.DataBind();
                    TrWithOutTime.Visible = false;
                    TrWithTime.Visible = true;
                    lblFromTime.Text = dtPartialLeave.Rows[0]["From_Time"].ToString();
                    lblToTime.Text = dtPartialLeave.Rows[0]["To_Time"].ToString();
                    txtPLInLog.Text = dtPartialLeave.Rows[0]["From_Time"].ToString();
                    txtPLOutLog.Text = dtPartialLeave.Rows[0]["To_Time"].ToString();
                    //}
                    lblPartialStatus.Text = dtPartialLeave.Rows[0]["Is_Confirmed"].ToString();
                    lblPartialDescription.Text = dtPartialLeave.Rows[0]["Description"].ToString();
                    lblEmployeeCode.Text = dtPartialLeave.Rows[0]["Emp_Code"].ToString();
                    lblEmployeeName.Text = dtPartialLeave.Rows[0]["Emp_Name"].ToString();
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "6")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = true;
                pnlPurchaseOrder.Visible = false;
                pnlLeaveSalary.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                DataTable dtPRheader = objPurrchaseRequestHeader.GetPurchaseRequestTrueAllByReqId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                if (dtPRheader != null && dtPRheader.Rows.Count > 0)
                {
                    try
                    {
                        txtRequestNo.Text = dtPRheader.Rows[0]["RequestNo"].ToString();
                        editid.Value = dtPRheader.Rows[0]["Trans_Id"].ToString();
                        txtRequestDate.Text = Convert.ToDateTime(dtPRheader.Rows[0]["RequestDate"].ToString()).ToString(objSys.SetDateFormat());
                        txtExpectedDeliveryDate.Text = Convert.ToDateTime(dtPRheader.Rows[0]["ExpDelDate"].ToString()).ToString(objSys.SetDateFormat());
                        txtDesc.Text = dtPRheader.Rows[0]["TermCondition"].ToString();
                        DataTable dtPRDetail = ObjPurrchaseRequestDetail.GetPurchaseRequestDetailbyRequestId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPRheader.Rows[0]["Trans_Id"].ToString());
                        //Common Function add By Lokesh on 12-05-2015
                        objPageCmn.FillData((object)gvProductRequest, dtPRDetail, "", "");
                    }
                    catch
                    {
                    }
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "7")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = true;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtPOheader = ObjPurchaseOrder.GetPurchaseOrderTrueAllByReqId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                try
                {
                    txtPONo.Text = dtPOheader.Rows[0]["PONo"].ToString();
                    editid.Value = dtPOheader.Rows[0]["TransId"].ToString();
                    txtOrderDate.Text = Convert.ToDateTime(dtPOheader.Rows[0]["PODate"].ToString()).ToString(objSys.SetDateFormat());
                    lblRefTypePO.Text = dtPOheader.Rows[0]["ReferenceVoucherType"].ToString();
                    lblRefNo.Text = dtPOheader.Rows[0]["ReferenceID"].ToString();
                    lblPOSupplier.Text = GetSupplierName(dtPOheader.Rows[0]["SupplierId"].ToString());
                    txtDescPO.Text = dtPOheader.Rows[0]["Remark"].ToString();
                    DataTable dtPODetail = ObjPurchaseOrderDetail.GetPurchaseOrderDetailbyPOId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPOheader.Rows[0]["TransId"].ToString());
                    //Common Function add By Lokesh on 12-05-2015
                    objPageCmn.FillData((object)gvProductOrder, dtPODetail, "", "");
                    float FooterAmount = 0;
                    foreach (GridViewRow dr in gvProductOrder.Rows)
                    {
                        FooterAmount += float.Parse(((Label)dr.FindControl("lblamtPO")).Text);
                    }
                    ((Label)gvProductOrder.FooterRow.FindControl("lblPOFooterAmount")).Text = FooterAmount.ToString();
                }
                catch
                {
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "9")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = true;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtOrderEdit = objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                if (dtOrderEdit.Rows.Count > 0)
                {
                    //txtSONo.Text = dtOrderEdit.Rows[0]["SalesOrderNo"].ToString();

                    hypSONo.Text = dtOrderEdit.Rows[0]["SalesOrderNo"].ToString();
                    hypSONo.NavigateUrl = "../Sales/SalesOrder1.aspx?Id=" + dt.Rows[0]["Ref_Id"].ToString() + "";

                    txtSODate.Text = Convert.ToDateTime(dtOrderEdit.Rows[0]["SalesOrderDate"].ToString()).ToString(objSys.SetDateFormat());
                    string strSOFromTransType = dtOrderEdit.Rows[0]["SOfromTransType"].ToString();
                    if (strSOFromTransType == "Q")
                    {
                        txtOrderType.Text = "By Quotation";
                    }
                    if (strSOFromTransType == "D")
                    {
                        txtOrderType.Text = "Direct";
                    }
                    DataTable dtOrderDetail = ObjSOrderDetail.GetSODetailBySOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                    if (dtOrderDetail.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 12-05-2015
                        objPageCmn.FillData((object)GvProductDetailQuotation, dtOrderDetail, "", "");
                    }
                    //for tax and discount field visibility
                    if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDiscountSales").Rows[0]["ParameterValue"].ToString()))
                    {
                        GvProductDetailQuotation.Columns[9].Visible = true;
                    }
                    if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsTaxSales").Rows[0]["ParameterValue"].ToString()))
                    {
                        GvProductDetailQuotation.Columns[10].Visible = true;
                    }
                    string strCustomerId = dtOrderEdit.Rows[0]["CustomerId"].ToString();
                    txtCustomer.Text = GetCustomerName(strCustomerId) + "/" + strCustomerId;
                    strCustomerId = dtOrderEdit.Rows[0]["Field2"].ToString();
                    lblsalesOrderRemarks.Text = dtOrderEdit.Rows[0]["Remark"].ToString();
                    txtEstimateDeliveryDate.Text = Convert.ToDateTime(dtOrderEdit.Rows[0]["EstimateDeliveryDate"].ToString()).ToString(objSys.SetDateFormat());
                    string strAddressId = dtOrderEdit.Rows[0]["ShipToAddressID"].ToString();
                    strAddressId = dtOrderEdit.Rows[0]["Field1"].ToString();


                    //New Code Added on 15-09-2022 By Lokesh

                    string strPayModeId = dtOrderEdit.Rows[0]["PaymentModeId"].ToString();
                    DataTable dtPayMode = objDA.return_DataTable("select * from Set_Payment_Mode_Master where Pay_Mode_Id='" + strPayModeId + "'");
                    if (dtPayMode.Rows.Count > 0 && dtPayMode != null)
                    {
                        txtPaymentMode.Text = dtPayMode.Rows[0]["Pay_Mod_Name"].ToString();
                    }
                    else
                    {
                        txtPaymentMode.Text = "";
                    }


                    Ac_Ageing_Detail objAgeing = null;
                    objAgeing = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
                    DataTable dtAgeing = objAgeing.getPendingAgeingTable(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "RV", strCustomerId, "0", "", true);
                    if (dtAgeing.Rows.Count > 0 && dtAgeing != null)
                    {
                        txtOverDue.Text = dtAgeing.Rows[0]["Due_Days"].ToString() + "  " + "Days";
                        string strOverDueAmount = "0";
                        foreach (DataRow row in dtAgeing.Rows)
                        {
                            if (strOverDueAmount == "0")
                            {
                                strOverDueAmount = row["L_Balance_Amount"].ToString();
                            }
                            else
                            {
                                strOverDueAmount = float.Parse(strOverDueAmount) + float.Parse(row["L_Balance_Amount"].ToString()).ToString();
                            }
                        }
                        if (float.Parse(strOverDueAmount.ToString()) == 0)
                        {
                            txtOverDue.Text = txtOverDue.Text + "/" + "0.00" + " " + "Amount";
                        }
                        else
                        {
                            txtOverDue.Text = txtOverDue.Text + "/" + strOverDueAmount + " " + "Amount";
                        }
                    }
                    else
                    {
                        txtOverDue.Text = "0 " + "Days";
                        txtOverDue.Text = txtOverDue.Text + "/" + "0.00" + " " + "Amount";
                    }


                    DataTable dtCreditDays = objDA.return_DataTable("select * from Set_CustomerMaster where Customer_Id='" + strCustomerId + "'");
                    if (dtCreditDays.Rows.Count > 0 && dtCreditDays != null)
                    {
                        txtCreditInfo.Text = dtCreditDays.Rows[0]["Credit_Days"].ToString() + "  " + "Days";

                        if (float.Parse(dtCreditDays.Rows[0]["Credit_Limit"].ToString()) == 0)
                        {
                            txtCreditInfo.Text = txtCreditInfo.Text + "/" + "0.00" + "Amount";
                        }
                        else
                        {
                            txtCreditInfo.Text = txtCreditInfo.Text + "/" + dtCreditDays.Rows[0]["Credit_Limit"].ToString() + "Amount";
                        }
                    }
                    else
                    {
                        txtCreditInfo.Text = "0" + "Days";
                        txtCreditInfo.Text = txtCreditInfo.Text + "/" + "0.00" + "Amount";
                    }

                    //For Conact Information

                    string strAccountMasterId = "0";
                    DataTable dtContactInfo = objDA.return_DataTable("select* from Ems_ContactMaster where Company_Id = '" + strCustomerId + "'");
                    if (dtContactInfo.Rows.Count > 0 && dtContactInfo != null)
                    {
                        txtContactEmail.Text = dtContactInfo.Rows[0]["Name"].ToString() + " / " + dtContactInfo.Rows[0]["Field1"].ToString() + " / " + dtContactInfo.Rows[0]["Field2"].ToString();

                        DataTable dtAccountId = objDA.return_DataTable("select * from Ac_AccountMaster where  Ref_Type='Customer' and Ref_Id= '" + strCustomerId + "'");
                        if (dtAccountId.Rows.Count > 0 && dtAccountId != null)
                        {
                            strAccountMasterId = dtAccountId.Rows[0]["Trans_Id"].ToString();
                        }
                    }
                    else
                    {
                        DataTable dtContactInfoCustomer = objDA.return_DataTable("select* from Ems_ContactMaster where Trans_Id = '" + strCustomerId + "'");
                        if (dtContactInfoCustomer.Rows.Count > 0 && dtContactInfoCustomer != null)
                        {
                            txtContactEmail.Text = dtContactInfoCustomer.Rows[0]["Name"].ToString() + " / " + dtContactInfoCustomer.Rows[0]["Field1"].ToString() + " / " + dtContactInfoCustomer.Rows[0]["Field2"].ToString();
                            DataTable dtAccountId = objDA.return_DataTable("select * from Ac_AccountMaster where  Ref_Type='Customer' and Ref_Id= '" + strCustomerId + "'");
                            if (dtAccountId.Rows.Count > 0 && dtAccountId != null)
                            {
                                strAccountMasterId = dtAccountId.Rows[0]["Trans_Id"].ToString();
                            }
                        }
                    }



                    DataTable dtFileDownload = objDA.return_DataTable("select * from Arc_File_Transaction where File_Path like '%" + hypSONo.Text + "%'");
                    if (dtFileDownload.Rows.Count > 0 && dtFileDownload != null)
                    {
                        string strFilePath = dtFileDownload.Rows[0]["File_Path"].ToString();
                        if (strFilePath != "")
                        {
                            txtFileDownload.Text = "";
                            txtFileDownload.Visible = false;
                            hypFileDownload.NavigateUrl = strFilePath;
                            hypFileDownload.Visible = true;
                        }
                        else
                        {
                            txtFileDownload.Text = "No Attachment for Download";
                            txtFileDownload.Visible = true;
                            hypFileDownload.NavigateUrl = "";
                            hypFileDownload.Visible = false;
                        }
                    }
                    else
                    {
                        txtFileDownload.Text = "No Attachment for Download";
                        txtFileDownload.Visible = true;
                        hypFileDownload.NavigateUrl = "";
                        hypFileDownload.Visible = false;
                    }

                    //string strFileNew = "~/ArcaWing/2/2/2/SO/SO292019-7463/PURCHASE ORDER/PO_Sama Catering_220919.pdf";
                    //txtFileDownload.Text = "";
                    //txtFileDownload.Visible = false;
                    //hypFileDownload.NavigateUrl = strFileNew;
                    //hypFileDownload.Visible = true;

                    var today = DateTime.Today;
                    var monthStart = new DateTime(today.Year, today.Month, 1);
                    var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                    hypCutomerStatement.NavigateUrl = "../CustomerReceivable/CustomerStatement.aspx?Id=" + strAccountMasterId + "";

                    Session["CusterStatementFromDate"] = monthStart.ToString("dd-MMM-yyyy");
                    Session["CustomerStatementToDate"] = monthEnd.ToString("dd-MMM-yyyy");
                    Session["CustomerStatementLocations"] = Session["LocId"].ToString();

                    //hypCutomerStatement.NavigateUrl = "../CustomerReceivable/CustomerStatement.aspx";



                    //End
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "8")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = true;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtQuoteEdit = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                try
                {
                    if (dtQuoteEdit.Rows.Count > 0)
                    {
                        lblQuotationNoView.Text = dtQuoteEdit.Rows[0]["SQuotation_No"].ToString();
                        lblSQDateView.Text = Convert.ToDateTime(dtQuoteEdit.Rows[0]["Quotation_Date"].ToString()).ToString(objSys.SetDateFormat());//updated by varsha on 11/09/2014
                        //Add Inquiry Data
                        string strSalesInquiryId = dtQuoteEdit.Rows[0]["SInquiry_No"].ToString();
                        lblInquiryNoView.Text = strSalesInquiryId.ToString();
                        lblCurrencyView.Text = GetCurrencyName(dtQuoteEdit.Rows[0]["Currency_Id"].ToString());
                        string strEmployeeId = dtQuoteEdit.Rows[0]["Emp_ID"].ToString();
                        lblEmployeeView.Text = GetEmployeeName(strEmployeeId) + "/" + strEmployeeId;
                        //lblCustomerNameView.Text = GetCustomerName(dtQuoteEdit.Rows[0]["Customer_Id"].ToString());
                        DataTable dtDetail = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), Session["FinanceYearId"].ToString());
                        if (dtDetail.Rows.Count > 0)
                        {
                            GvDetailView.DataSource = dtDetail;
                            GvDetailView.DataBind();
                        }
                    }
                }
                catch
                {
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "10")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = true;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                if (dtInvEdit.Rows.Count > 0)
                {
                    lblInvoiceNumberView.Text = dtInvEdit.Rows[0]["Invoice_No"].ToString();
                    lblSInvDateView.Text = Convert.ToDateTime(dtInvEdit.Rows[0]["Invoice_Date"].ToString()).ToString(objSys.SetDateFormat());
                    string strSOFromTransType = dtInvEdit.Rows[0]["SIFromTransType"].ToString();
                    if (strSOFromTransType == "S")
                    {
                        lblTransFromView.Text = "Sales Order";
                        lblorderTypeView.Text = "By Sales Order";
                    }
                    else if (strSOFromTransType == "D")
                    {
                        lblorderTypeView.Text = "Direct";
                    }
                    lblTransFromView.Text = dtInvEdit.Rows[0]["SIFromTransType"].ToString();
                    string strCustomerId = dtInvEdit.Rows[0]["Supplier_Id"].ToString();
                    lblSICustName.Text = GetCustomerName(strCustomerId) + "/" + strCustomerId;
                    DataTable dtInvoiceDetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtInvEdit.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString());
                    if (dtInvoiceDetail.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 12-05-2015
                        objPageCmn.FillData((object)GvInvoiceProductDetail, dtInvoiceDetail, "", "");
                    }
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "12")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = true;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                string sql = "select * from dbo.HR_Salary_Increment where Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString().Trim() + "";
                DataTable dtSalary = objDA.return_DataTable(sql);
                if (dtSalary.Rows.Count > 0)
                {
                    lblEmployeeName_SalInc.Text = GetEmployeeName(dtSalary.Rows[0]["Employee_Id"].ToString());
                    lblBasicsalary_SalInc.Text = dtSalary.Rows[0]["Basic_Salary"].ToString();
                    lblIncPer_SalInc.Text = dtSalary.Rows[0]["Increment_Percent"].ToString();
                    lblIncValue_SalInc.Text = dtSalary.Rows[0]["Increment_Value"].ToString();
                    lblIncSalary_SalInc.Text = dtSalary.Rows[0]["Increment_Salary"].ToString();
                    lblIncRequestDate_SalInc.Text = Convert.ToDateTime(dtSalary.Rows[0]["CreatedDate"].ToString()).ToString(objSys.SetDateFormat());
                }
                DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                if (dtInvEdit.Rows.Count > 0)
                {
                    lblInvoiceNumberView.Text = dtInvEdit.Rows[0]["Invoice_No"].ToString();
                    lblSInvDateView.Text = Convert.ToDateTime(dtInvEdit.Rows[0]["Invoice_Date"].ToString()).ToString(objSys.SetDateFormat());
                    string strSOFromTransType = dtInvEdit.Rows[0]["SIFromTransType"].ToString();
                    if (strSOFromTransType == "S")
                    {
                        lblTransFromView.Text = "Sales Order";
                        lblorderTypeView.Text = "By Sales Order";
                        //hdnSalesOrderId.Value = dtInvEdit.Rows[0]["SIFromTransNo"].ToString();    
                    }
                    else if (strSOFromTransType == "D")
                    {
                        lblorderTypeView.Text = "Direct";
                    }
                    lblTransFromView.Text = dtInvEdit.Rows[0]["SIFromTransType"].ToString();
                    string strCustomerId = dtInvEdit.Rows[0]["Supplier_Id"].ToString();
                    lblSICustName.Text = GetCustomerName(strCustomerId) + "/" + strCustomerId;
                    DataTable dtInvoiceDetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtInvEdit.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString());
                    if (dtInvoiceDetail.Rows.Count > 0)
                    {
                        //Common Function add By Lokesh on 12-05-2015
                        objPageCmn.FillData((object)GvInvoiceProductDetail, dtInvoiceDetail, "", "");
                    }
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "13")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = true;
                pnlSalaryIncrement.Visible = false;
                pnlHalfDay.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlLoan.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                DataTable dtOT = objOverTimeReq.GetOvertimeRequestByCompany(Session["CompId"].ToString());
                dtOT = new DataView(dtOT, "Trans_Id='" + dt.Rows[0]["Ref_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtOT.Rows.Count > 0)
                {
                    txtOvertimeId.Text = dtOT.Rows[0]["Trans_Id"].ToString();
                    txtEmployeeCode.Text = dtOT.Rows[0]["Emp_Code"].ToString();
                    txtEmployeeName.Text = dtOT.Rows[0]["Emp_Name"].ToString();
                    try
                    {
                        lblOTStatus.Text = GetOTStatus(dtOT.Rows[0]["Trans_Id"].ToString());
                    }
                    catch
                    {
                    }
                    try
                    {
                        lblOTDate.Text = Convert.ToDateTime(dtOT.Rows[0]["Overtime_Date"].ToString()).ToString(objSys.SetDateFormat());
                        lblOTFromTime.Text = dtOT.Rows[0]["From_Time"].ToString();
                        lblOTtoTime.Text = dtOT.Rows[0]["To_Time"].ToString();
                        lblOTTimeDuration.Text = dtOT.Rows[0]["Time_Duration"].ToString();
                        lblOTDescription.Text = dtOT.Rows[0]["Description"].ToString();
                    }
                    catch
                    {
                    }
                }
            }
            //for credit approval
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "14" || dt.Rows[0]["Approval_Id"].ToString().Trim() == "16")
            {
                pnlPaymentApproval.Visible = false;
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlHalfDay.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlLoan.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = true;
                pnlLeaveSalary.Visible = false;
                //DataTable dtCreditParameter = ObjCreditParam.GetRecord_By_CustomerId(dt.Rows[0]["Ref_Id"].ToString());
                DataTable dtCreditParameter = new DataTable();
                int otherAcId = 0;
                string acId = "0";
                if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "14")
                {
                    dtCreditParameter = ObjCreditParam.GetCustomerRecord_By_OtherAccountId(dt.Rows[0]["Ref_Id"].ToString());
                    Label61.Text = Resources.Attendance.Customer_Name;
                    dtCreditParameter = new DataView(dtCreditParameter, "RecordType='C'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCreditParameter.Rows.Count > 0)
                    {
                        otherAcId = new Ac_AccountMaster(Session["DBConnection"].ToString()).GetCustomerAccountByCurrency(dtCreditParameter.Rows[0]["Customer_id"].ToString(), dtCreditParameter.Rows[0]["credit_limit_currency"].ToString());
                        acId = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()).ToString();
                    }
                }
                else
                {
                    dtCreditParameter = ObjCreditParam.GetSupplierRecord_By_OtherAccountId(dt.Rows[0]["Ref_Id"].ToString());
                    Label61.Text = Resources.Attendance.Supplier_Name;
                    dtCreditParameter = new DataView(dtCreditParameter, "RecordType='S'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtCreditParameter.Rows.Count > 0)
                    {
                        otherAcId = new Ac_AccountMaster(Session["DBConnection"].ToString()).GetSupplierAccountByCurrency(dtCreditParameter.Rows[0]["Customer_id"].ToString(), dtCreditParameter.Rows[0]["credit_limit_currency"].ToString());
                        acId = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString()).ToString();
                    }
                }
                if (dtCreditParameter.Rows.Count > 0)
                {
                    lblCreditCustomerName.Text = dtCreditParameter.Rows[0]["Customer_Name"].ToString().Trim();
                    txtCreditLimit.Text = Common.GetAmountDecimal(dtCreditParameter.Rows[0]["Credit_Limit"].ToString().Trim(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                    try
                    {
                        if (acId != "0" && otherAcId > 0)
                        {
                            string sql = "select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + Session["LocId"].ToString() + "', '" + DateTime.Now.Date.ToString("dd-MMM-yyyy") + "', '0','" + acId + "','" + otherAcId.ToString() + "','3','" + Session["FinanceYearId"].ToString() + "')) OpeningBalance";
                            lblCurrentBalance.Text = SystemParameter.GetAmountWithDecimal(objDA.get_SingleValue(sql), Session["LoginLocDecimalCount"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        lblCurrentBalance.Text = "0";
                    }

                    lblCurrencyCreditLimit.Text = dtCreditParameter.Rows[0]["Currency_Symbol"].ToString().Trim();
                    lblCreditDays.Text = dtCreditParameter.Rows[0]["Credit_Days"].ToString().Trim();
                    rbtnAdvanceCheque.Checked = Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Adavance_Cheque_Basis"].ToString().Trim());
                    rbtnInvoicetoInvoice.Checked = Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Invoice_To_Invoice"].ToString().Trim());
                    rbtnAdvanceHalfpayment.Checked = Convert.ToBoolean(dtCreditParameter.Rows[0]["Is_Half_Advance"].ToString().Trim());
                    if (dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim() != "")
                    {
                        lnkDownloadFiancialstatement.Text = dtCreditParameter.Rows[0]["Financial_Statement_Name"].ToString().Trim();
                    }
                    else
                    {
                        lnkDownloadFiancialstatement.Text = "";
                    }
                }
            }
            //For Payment Approval
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "15")
            {
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlPaymentApproval.Visible = true;
                pnlLeaveSalary.Visible = false;
                //DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeaderByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                DataTable dtVoucherHeader = objDA.return_DataTable("select * from ac_voucher_header where company_id='" + Session["CompId"].ToString() + "' and trans_id=" + dt.Rows[0]["Ref_Id"].ToString());
                if (dtVoucherHeader.Rows.Count > 0)
                {
                    txtVoucherNo.Text = dtVoucherHeader.Rows[0]["Voucher_No"].ToString();
                    txtVoucherDate.Text = Convert.ToDateTime(dtVoucherHeader.Rows[0]["Voucher_Date"].ToString()).ToString(objSys.SetDateFormat());
                    txtVNarration.Text = dtVoucherHeader.Rows[0]["Narration"].ToString();
                    DataTable dtVoucherDetail = objDA.return_DataTable("select * from ac_voucher_detail where company_id='" + Session["CompId"].ToString() + "' and voucher_no=" + dt.Rows[0]["Ref_Id"].ToString());
                    //DataTable dtVoucherDetail = objVoucherDetail.GetVoucherDetailByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());
                    if (dtVoucherDetail.Rows.Count > 0)
                    {
                        GvVoucherDetail.DataSource = dtVoucherDetail;
                        GvVoucherDetail.DataBind();
                    }
                }
            }
            else if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "20")
            {
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlPaymentApproval.Visible = false;
                pnlLeaveSalary.Visible = false;
                pnlEmailMarketing.Visible = true;
                DataTable dtmailMarket = objMailMarket.GetRecordHeader(dt.Rows[0]["Ref_Id"].ToString(), "5");
                lbltemplatename.Text = dtmailMarket.Rows[0]["Template_Name"].ToString();
                lblMailHeader.Text = dtmailMarket.Rows[0]["MailHeader"].ToString();
                lbldisplayText.Text = dtmailMarket.Rows[0]["Field3"].ToString();
                lblTotalEmail.Text = dtmailMarket.Rows[0]["TotalMail"].ToString();
            }
            else if (ddlApprovalType.SelectedItem.Text.ToLower().Contains("salary"))
            {
                pnlOvertime.Visible = false;
                pnlSalaryIncrement.Visible = false;
                pnlLeave.Visible = false;
                pnlClaim.Visible = false;
                pnlLoan.Visible = false;
                pnlHalfDay.Visible = false;
                PnlPartialLeave.Visible = false;
                pnlPurchaseRequest.Visible = false;
                pnlPurchaseOrder.Visible = false;
                pnlSalesQuotation.Visible = false;
                pnlSalesInvoice.Visible = false;
                pnlSalesOrder.Visible = false;
                pnlCreditApproval.Visible = false;
                pnlPaymentApproval.Visible = false;
                pnlLeaveSalary.Visible = true;
                lblempcodeValueLS.Text = Common.GetEmployeeCode(dt.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                lblempnameValueLS.Text = Common.GetEmployeeName(dt.Rows[0]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                DataTable dtLeavesalary = objDA.return_DataTable("select att_leavemaster.Leave_Name as LeaveName, att_leavesalary.from_date,att_leavesalary.to_date, att_leavesalary.f2 as TotalLeave,att_leavesalary.Leave_Count as ActualLeave,att_leavesalary.F3 as UsedLeave,att_leavesalary.F4 as BalanceLeave,att_leavesalary.Per_Day_Salary,att_leavesalary.Total from att_leavesalary inner join att_leavemaster on att_leavesalary.Leave_Type_Id = att_leavemaster.Leave_Id where att_leavesalary.f6=" + dt.Rows[0]["Ref_Id"].ToString() + "");
                gvLeavesalary.DataSource = dtLeavesalary;
                gvLeavesalary.DataBind();
                double Total = 0;
                for (int i = 0; i < dtLeavesalary.Rows.Count; i++)
                {
                    Total += Convert.ToDouble(dtLeavesalary.Rows[i]["Total"].ToString());
                }
                try
                {
                    ((Label)gvLeavesalary.FooterRow.FindControl("lblTotalSum")).Text = Total.ToString();
                }
                catch
                {
                }
            }
            txtDescription.Text = dt.Rows[0]["Description"].ToString();
            txtDescription.Enabled = false;
            btnApprovePopup.Visible = false;
            btnRejectPopup.Visible = false;
            //if ((ddlStatus.SelectedItem.Text == "Rejected") && dt.Rows[0]["Priority"].ToString().Trim() == "True" && dt.Rows[0]["Emp_Id"].ToString().Trim() == Session["EmpId"].ToString())
            //{
            //    txtDescription.Enabled = true;
            //    btnApprovePopup.Visible = true;
            //}
            if ((ddlStatus.SelectedItem.Text == "Approved") && dt.Rows[0]["Priority"].ToString().Trim() == "True" && dt.Rows[0]["Emp_Id"].ToString().Trim() == Session["EmpId"].ToString())
            {
                txtDescription.Enabled = true;
                btnRejectPopup.Visible = true;
            }
            else if ((ddlStatus.SelectedItem.Text == "Pending") && dt.Rows[0]["Emp_Id"].ToString().Trim() == Session["EmpId"].ToString())
            {
                txtDescription.Enabled = true;
                btnApprovePopup.Visible = true;
                btnRejectPopup.Visible = true;
            }
        }
        dt.Dispose();
    }
    protected void btnDownloadFiancialstatement_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../CompanyResource/" + Session["CompId"].ToString() + "/" + lnkDownloadFiancialstatement.Text.Trim().ToString() + "')", true);
        }
        catch
        {
        }
    }
    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            DataTable dtAccName = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountNo);
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
    protected void Approve_Command(object sender, CommandEventArgs e)
    {
        string Str_Request_For_Name = string.Empty;
        string Str_Sender_Name = string.Empty;
        string LogStatus = string.Empty;
        string strVoucherLocationId = "0";
        string strVoucherBrandId = "0";
        if (((ImageButton)sender).ID == "imgBtnApprove")
        {
            LogStatus = "Direct";
        }
        else
        {
            LogStatus = "Indirect";
        }
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        bool IsPriority = false;
        string EmpId = GetEmpIdByUserId();
        string transId = e.CommandArgument.ToString();
        DataTable dtapproval = new DataTable();
        DataTable dt = new DataTable();
        if (ddlApprovalType.SelectedValue.Trim() == "1")
        {
            // dt = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
            dt = objEmpApproval.GetApprovalTransationbyTransId(Session["CompId"].ToString(), transId);
            dtapproval = dt.Copy();
        }
        else
        {
            dt = objEmpApproval.GetApprovalTransationbyTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        }
        //DataTable dt = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
        //dtapproval = dt.Copy();
        dt = new DataView(dt, "Trans_Id='" + transId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        if (Convert.ToBoolean(dt.Rows[0]["Priority"].ToString()) && dt.Rows[0]["Status"].ToString().Trim() == "Approved")
        {
            DisplayMessage("Request is already approved");
            return;
        }
        Str_Request_For_Name = GetEmployeeName(dt.Rows[0]["request_emp_id"].ToString());
        Str_Sender_Name = GetEmployeeName(dt.Rows[0]["Emp_Id"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            DataTable dtPriority = new DataTable();
            switch (Convert.ToInt32(ddlApprovalType.SelectedValue))
            {
                case 1:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='1' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        Approve_Leave(dtapproval, dt.Rows[0]["Ref_Id"].ToString(), transId, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    else
                    {
                        if (ObjapprovalMaster.GetApprovalMasterById(ddlApprovalType.SelectedValue, ref trns).Rows[0]["Approval_Type"].ToString().Trim() != "Priority")
                        {
                            HierarchysendEmailForFullDayLeave(dt.Rows[0]["Ref_Id"].ToString(), transId, ref trns);
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                        }
                    }
                    break;
                case 2:
                    if (txtHLInLog.Text == "" || txtHLOutLog.Text == "")
                    {
                        DisplayMessage("Enter Half day In and Out Log");
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='2' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        if (!Approve_HalfDay(dt.Rows[0]["Ref_Id"].ToString(), transId, ref trns))
                        {
                            DisplayMessage("Halfday log already exists");
                            trns.Rollback();
                            if (con.State == System.Data.ConnectionState.Open)
                            {
                                con.Close();
                            }
                            trns.Dispose();
                            con.Dispose();
                            return;
                        }
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    else
                    {
                        if (ObjapprovalMaster.GetApprovalMasterById(ddlApprovalType.SelectedValue, ref trns).Rows[0]["Approval_Type"].ToString().Trim() != "Priority")
                        {
                            HierarchysendEmailForHalfDayLeave(dt.Rows[0]["Ref_Id"].ToString(), transId, ref trns);
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                        }
                    }
                    break;
                case 3:
                    if (txtPLInLog.Text == "" || txtPLOutLog.Text == "")
                    {
                        DisplayMessage("Enter Partial In and Out Log");
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    int totalMinute = 0;
                    int MinuteUsed_In_Minute = 0;
                    int TotalPartialMinute = 0;
                    //here we are checking that partial minute should be equal or less then to company parameter value
                    // totalMinute = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString()));
                    MinuteUsed_In_Minute = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
                    //TotalPartialMinute = totalMinute / MinuteUsed_In_Minute;
                    //here we are checking that partial leave type official or personal 
                    //condiion added on 30/10/2017
                    //this validation for personal partial leave only
                    if (objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString()).Rows[0]["Partial_Leave_Type"].ToString().Trim() == "0")
                    {
                        if (GetMinuteDiff(txtPLOutLog.Text, txtPLInLog.Text) > MinuteUsed_In_Minute)
                        {
                            DisplayMessage("You can not request more than " + MinuteUsed_In_Minute.ToString() + " minutes in a day");
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
                    else if (chkModifyPLTime.Checked)
                    {
                        objDA.execute_Command("update Att_PartialLeave_Request set  Field2=From_Time,Field3=To_Time,From_Time='" + txtPLInLog.Text + "' ,To_Time='" + txtPLOutLog.Text + "'  where Trans_Id =" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                    }
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='3' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        ViewState["Emp_Id"] = dtPriority.Rows[0]["Emp_Id"].ToString();
                        IsPriority = true;
                        int leavestatus = Approve_ParialLeave(dt.Rows[0]["Ref_Id"].ToString(), transId, LogStatus, ref trns);
                        if (leavestatus == 1)
                        {
                            DisplayMessage("You Can Not Approve Greater than Assigned Leave");
                            trns.Rollback();
                            if (con.State == System.Data.ConnectionState.Open)
                            {
                                con.Close();
                            }
                            trns.Dispose();
                            con.Dispose();
                            return;
                        }
                        else if (leavestatus == 2)
                        {
                            DisplayMessage("Partial Leave log already exists");
                            trns.Rollback();
                            if (con.State == System.Data.ConnectionState.Open)
                            {
                                con.Close();
                            }
                            trns.Dispose();
                            con.Dispose();
                            return;
                        }
                        else
                        {
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                        }
                    }
                    else
                    {
                        if (ObjapprovalMaster.GetApprovalMasterById(ddlApprovalType.SelectedValue, ref trns).Rows[0]["Approval_Type"].ToString().Trim() != "Priority")
                        {
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                            HierarchysendEmailForPartialLeave(dt.Rows[0]["Ref_Id"].ToString(), transId, ref trns);
                        }
                    }
                    break;
                case 4:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='4' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        DataTable dtClaim = new DataTable();
                        dtClaim = objPayClaim.GetRecord_From_PayEmployeeClaimByClaimId(dt.Rows[0]["Ref_Id"].ToString(), ref trns);
                        int UpdationCheck = 0;
                        if (dtClaim.Rows.Count > 0)
                        {
                            UpdationCheck = objPayClaim.UpdateRecord_In_Pay_Employee_Claim(Session["CompId"].ToString(), dtClaim.Rows[0]["Claim_Id"].ToString(), dtClaim.Rows[0]["Claim_Name"].ToString(), dtClaim.Rows[0]["Claim_Description"].ToString(), dtClaim.Rows[0]["Value_Type"].ToString(), dtClaim.Rows[0]["Value"].ToString(), dtClaim.Rows[0]["Claim_Month"].ToString(), dtClaim.Rows[0]["Claim_Year"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                        }
                    }
                    break;
                case 5:
                    dtPriority = new DataView(dt, "Emp_Id='" + Session["EmpId"].ToString() + "' and Approval_Id='5' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objLoan.UpdateStatus_In_Pay_Employee_Loan(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 6:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='6' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        //objPurrchaseRequestHeader.UpdatePurchaseRequestHeaderApproval(dt.Rows[0]["Ref_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Approved", true.ToString(), false.ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                        objPurrchaseRequestHeader.UpdatePurchaseRequestHeaderApproval(dt.Rows[0]["Ref_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Approved", true.ToString(), false.ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 7:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='7' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        //ObjPurchaseOrder.UpdatePurchaseOrderApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        ObjPurchaseOrder.UpdatePurchaseOrderApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 8:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='8' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        // objSQuoteHeader.UpdateSalesQuotationApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        objSQuoteHeader.UpdateSalesQuotationApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 9:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='9' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        //objSOrderHeader.UpdateSalesOrderApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        objSOrderHeader.UpdateSalesOrderApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 10:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='10' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        // objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 12:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='12' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        int a = 0;
                        string sql = "select * from dbo.HR_Salary_Increment where Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString().Trim() + "";
                        DataTable dtSalary = objDA.return_DataTable(sql, ref trns);
                        if (dtSalary.Rows.Count > 0)
                        {
                            sql = "update dbo.HR_Salary_Increment set Field1='Approved', Field2='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "'  where Trans_Id=" + dtSalary.Rows[0]["Trans_Id"].ToString() + "";
                            a = objDA.execute_Command(sql, ref trns);
                            sql = "Update Set_Emp_SalaryIncrement SET Month=" + Convert.ToDateTime(dtSalary.Rows[0]["Field2"].ToString()).Month.ToString() + ",Year =" + Convert.ToDateTime(dtSalary.Rows[0]["Field2"].ToString()).Year.ToString() + ",Basic_Salary=" + dtSalary.Rows[0]["Increment_Salary"].ToString() + " where Emp_Id=" + dtSalary.Rows[0]["Employee_Id"].ToString() + "";
                            a = objDA.execute_Command(sql, ref trns);
                            sql = "Update Set_EmployeeMaster  Set Field7 ='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "' WHERE Emp_Id =" + dtSalary.Rows[0]["Employee_Id"].ToString() + "";
                            a = objDA.execute_Command(sql, ref trns);
                            sql = "Update Set_Employee_Parameter SET Basic_Salary =" + dtSalary.Rows[0]["Increment_Salary"].ToString() + " Where Emp_Id = " + dtSalary.Rows[0]["Employee_Id"].ToString() + "";
                            a = objDA.execute_Command(sql, ref trns);
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                        }
                    }
                    break;
                case 13:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='13' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objOverTimeReq.UpdateOvertimeRequestByTransId(dt.Rows[0]["Ref_Id"].ToString(), Session["CompId"].ToString(), false.ToString(), true.ToString(), false.ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 14:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='14' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        // objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        string sql = "Update Set_CustomerMaster_CreditParameter set field4='Approved', ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "'  where field2='C' and field3='" + dt.Rows[0]["Ref_Id"].ToString() + "'";
                        objDA.execute_Command(sql, ref trns);
                        //objCustomer.UpdateCustomerStatus(dt.Rows[0]["Ref_Id"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 15:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='15' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                    }
                    Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    break;
                case 16:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='16' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        // objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        //objSupplier.UpdateSupplierStatus(dt.Rows[0]["Ref_Id"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        string sql = "Update Set_CustomerMaster_CreditParameter set field4='Approved', ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "'  where field2='S' and field3='" + dt.Rows[0]["Ref_Id"].ToString() + "'";
                        objDA.execute_Command(sql, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 17:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='17' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                        {
                            DataTable dtTempShiftHeader = ObjTempEmpShift.GetHeaderRecordBy_Trans_Id(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), ref trns);
                            DataTable dtTempShiftDetail = ObjTempEmpShift.GetDetailRecordBy_Header_Id(dt.Rows[0]["Ref_Id"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            if (dtTempShiftHeader.Rows.Count > 0)
                            {
                                dtTempShiftDetail = new DataView(dtTempShiftDetail, "ref_type<>'Leave'", "", DataViewRowState.CurrentRows).ToTable();
                                AssignShift1(dtTempShiftDetail, dt.Rows[0]["request_emp_id"].ToString(), Convert.ToDateTime(dtTempShiftHeader.Rows[0]["schedule_from"].ToString()), Convert.ToDateTime(dtTempShiftHeader.Rows[0]["schedule_to"].ToString()), ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["UserId"].ToString());
                            }
                            ObjTempEmpShift.UpdateHeaderApprovalStatus(dt.Rows[0]["Ref_Id"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        }
                        else
                        {
                            objDA.execute_Command("update att_leavesalary set f5='Approved',F7='True' where F6=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            objDA.execute_Command("update ac_voucher_header set Field3='Approved' where Ref_Type='Leave Salary' and Ref_id=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            UpdateLeaveBalanceForLeaveSalary(dt.Rows[0]["Ref_Id"].ToString(), "Approved", ref trns);
                        }
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 18:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='18' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                        {
                            DataTable dtTempShiftHeader = ObjTempEmpShift.GetHeaderRecordBy_Trans_Id(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), ref trns);
                            DataTable dtTempShiftDetail = ObjTempEmpShift.GetDetailRecordBy_Header_Id(dt.Rows[0]["Ref_Id"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            if (dtTempShiftHeader.Rows.Count > 0)
                            {
                                dtTempShiftDetail = new DataView(dtTempShiftDetail, "ref_type<>'Leave'", "", DataViewRowState.CurrentRows).ToTable();
                                Att_ScheduleMaster.AssignShift(dtTempShiftDetail, dt.Rows[0]["request_emp_id"].ToString(), Convert.ToDateTime(dtTempShiftHeader.Rows[0]["schedule_from"].ToString()), Convert.ToDateTime(dtTempShiftHeader.Rows[0]["schedule_to"].ToString()), ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Session["UserId"].ToString());
                            }
                            ObjTempEmpShift.UpdateHeaderApprovalStatus(dt.Rows[0]["Ref_Id"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        }
                        else
                        {
                            objDA.execute_Command("update att_leavesalary set f5='Approved',F7='True' where F6=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            objDA.execute_Command("update ac_voucher_header set Field3='Approved' where Ref_Type='Leave Salary' and Ref_id=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            UpdateLeaveBalanceForLeaveSalary(dt.Rows[0]["Ref_Id"].ToString(), "Approved", ref trns);
                        }
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
                case 20:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='20' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objDA.execute_Command("update Ems_TemplateSelection_Header set Field5='Approved' where Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Approved", transId);
                    }
                    break;
            }
            string Description = string.Empty;
            Description = txtDescription.Text;
            if (IsPriority)
            {
                Description += " Approved By-" + GetEmployeeName(dt.Rows[0]["Emp_Id"].ToString(), ref trns);
            }
            else
            {
                if (Description.Trim() == "")
                {
                    Description += "Approved";
                }
            }
            string strVoucherDate = string.Empty;
            if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "15")
            {
                if (dt.Rows[0]["Ref_Id"].ToString() != "" && dt.Rows[0]["Ref_Id"].ToString() != "0")
                {
                    //DataTable dtVoucherHeaderData = objVoucherHeader.GetVoucherHeaderByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), ref trns);
                    DataTable dtVoucherHeaderData = objDA.return_DataTable("select * from ac_voucher_header where company_id='" + Session["CompId"].ToString() + "' and trans_id=" + dt.Rows[0]["Ref_Id"].ToString());
                    if (dtVoucherHeaderData.Rows.Count > 0)
                    {
                        strVoucherBrandId = dtVoucherHeaderData.Rows[0]["brand_id"].ToString();
                        strVoucherLocationId = dtVoucherHeaderData.Rows[0]["location_id"].ToString();
                        strVoucherDate = dtVoucherHeaderData.Rows[0]["Voucher_Date"].ToString();
                        if (strVoucherDate != "")
                        {

                            if (!Common.IsFinancialyearAllow(Convert.ToDateTime(strVoucherDate), "F", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                            {
                                DisplayMessage("Log In Financial year not allowing to perform this action You need to Reject Only");
                                dt.Dispose();
                                trns.Commit();
                                if (con.State == System.Data.ConnectionState.Open)
                                {
                                    con.Close();
                                }
                                trns.Dispose();
                                con.Dispose();
                                return;
                            }
                            else
                            {
                                objEmpApproval.UpdateApprovalTransaciton(dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "0", dt.Rows[0]["Emp_Id"].ToString(), "Approved", Description, dt.Rows[0]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                                //objEmpApproval.UpdateApprovalTransaciton(dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "0", dt.Rows[0]["Emp_Id"].ToString(), "Approved", Description, dt.Rows[0]["Approval_Id"].ToString(), ref trns);
                                //objEmpApproval.UpdateApprovalTransacitonByEmpId(dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "0", dt.Rows[0]["Emp_Id"].ToString(), "Approved", Description, dt.Rows[0]["Approval_Id"].ToString(), ref trns);
                            }
                        }
                    }
                }
            }
            else
            {
                objEmpApproval.UpdateApprovalTransaciton(dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "0", dt.Rows[0]["Emp_Id"].ToString(), "Approved", Description, dt.Rows[0]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                ObjTempEmpShift.UpdateHeaderApprovalStatus(dt.Rows[0]["Ref_Id"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
            }
            if (dt.Rows.Count > 0)
            {
                //for Payment Approval
                if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "15" && dtPriority.Rows.Count > 0)
                {
                    IsPriority = true;
                    string strFinalStatus = "Approved";
                    if (strVoucherBrandId == "")
                    {
                        strVoucherBrandId = Session["BrandId"].ToString();
                    }
                    if (strVoucherLocationId == "")
                    {
                        strVoucherLocationId = Session["LocId"].ToString();
                    }
                    //DataTable dtCheckStatus = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
                    DataTable dtCheckStatus = objEmpApproval.GetApprovalTransationPayment(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(),"15", ref trns);
                    //dtCheckStatus = new DataView(dtCheckStatus, "Approval_Id='15' and Ref_Id='" + dt.Rows[0]["Ref_Id"].ToString() + "' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    for (int j = 0; j < dtCheckStatus.Rows.Count; j++)
                    {
                        string Status = dtCheckStatus.Rows[j]["Status"].ToString();
                        if (Status == "Pending")
                        {
                            strFinalStatus = Status;
                            //break;
                        }
                    }
                    if (strFinalStatus == "Approved")
                    {
                        objVoucherHeader.UpdateVoucherHeaderApprovalStatus(Session["CompId"].ToString(), strVoucherBrandId, strVoucherLocationId, dt.Rows[0]["Ref_Id"].ToString(), "Approved", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        if (objAgeingDetail.checkAgeingConsistency_and_Insert(Session["CompId"].ToString(), strVoucherBrandId, strVoucherLocationId, dt.Rows[0]["Ref_Id"].ToString(), ref trns, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["EmpId"].ToString()) == true)
                        {
                        }
                    }
                }
                //for log process
            }
            DisplayMessage("Request Approved");
            string strSender = string.Empty;
            try
            {
                strSender = ((ImageButton)sender).ID.ToString();
            }
            catch { }
            if (strSender != "imgBtnApprove")
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            if (!ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
            {
                FillGrid(hdnApprovalType.Value);
            }
            string strsql = string.Empty;
            //if (dt.Rows[0]["Approval_Id"].ToString().Trim() == "1" && dtPriority.Rows.Count > 0)
            //{
            //    DataTable dtleave = objDA.return_DataTable("select Emp_Id,From_date,To_Date from Att_LeaveRequest_header where Trans_id=" + dt.Rows[0]["Ref_Id"].ToString() + "");
            //    ObjLOgProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtleave.Rows[0]["Emp_Id"].ToString(), Session["UserId"].ToString(), "0", Convert.ToDateTime(dtleave.Rows[0]["From_Date"].ToString()), Convert.ToDateTime(dtleave.Rows[0]["To_Date"].ToString()), Session["EmpId"].ToString(),"");
            //}
            //if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
            //{
            //    DataTable dtTempShiftHeader = ObjTempEmpShift.GetHeaderRecordBy_Trans_Id(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString());

            //    ThreadStart ts = delegate () { ObjLOgProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtTempShiftHeader.Rows[0]["emp_id"].ToString(), Session["UserId"].ToString(), "0", Convert.ToDateTime(dtTempShiftHeader.Rows[0]["schedule_from"].ToString()), Convert.ToDateTime(dtTempShiftHeader.Rows[0]["schedule_to"].ToString()), Session["EmpId"].ToString()); };

            //    // The thread.
            //    Thread t = new Thread(ts);

            //    // Run the thread.
            //    t.Start();

            //}
            //if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("leave"))
            //{
            //    DataTable dtLeave = objDA.return_DataTable("select From_Date,To_Date,Emp_id from Att_LeaveRequest_header where Trans_Id = "+ dt.Rows[0]["Ref_id"].ToString()+ "");

            //    ThreadStart ts = delegate () { ObjLOgProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtLeave.Rows[0]["emp_id"].ToString(), Session["UserId"].ToString(), "0", Convert.ToDateTime(dtLeave.Rows[0]["from_Date"].ToString()), Convert.ToDateTime(dtLeave.Rows[0]["To_Date"].ToString()), Session["EmpId"].ToString()); };

            //    // The thread.
            //    Thread t = new Thread(ts);

            //    // Run the thread.
            //    t.Start();

            //}
            dt.Dispose();
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

    public bool AssignShift1(DataTable dtTempShiftDetail, string strEmpId, DateTime DtFromDate, DateTime dtToDate, ref SqlTransaction trns, string strCompanyid, string strBrandid, string strLocationId, string strTimeZoneId, string struserid)
    {


        DataTable dtTempsch = new DataTable();
        DataTable dtSch = new DataTable();
        DataTable dtTime = new DataTable();
        DateTime dtStartCheck = new DateTime();
        DataTable dtTempShift = new DataTable();
        DataTable dtShift = new DataTable();
        DataTable dtShiftD = new DataTable();
        string OverlapDate = string.Empty;
        string ExcludeDayAs = string.Empty;
        string CompWeekOffDay = string.Empty;
        string strShiftId = string.Empty;
        ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", strCompanyid, strBrandid, strLocationId, ref trns);

        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", strCompanyid, ref trns, strBrandid, strLocationId);
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }


        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyid, strBrandid, strLocationId, ref trns);
        }

        int b = 1;
        int rem = 0;



        //while (DtFromDate <= dtToDate)
        //{

        foreach (DataRow dr in dtTempShiftDetail.Rows)
        {

            dtSch = objDA.return_DataTable("SELECT Schedule_Id,shift_id from Att_ScheduleMaster  where IsActive='True' and emp_id=" + strEmpId + " and Shift_Type='Normal Shift'", ref trns);

            DtFromDate = Convert.ToDateTime(dr["schedule_date"].ToString());

            //deleting exists holiday
            objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDateOnly(strCompanyid, strEmpId, DtFromDate.ToString(), ref trns);



            if (dr["ref_Type"].ToString().ToUpper() == "OFF")
            {
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, "0", dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;
            }
            if (dr["ref_Type"].ToString().ToUpper() == "HOLIDAY")
            {
                //deleting scheduled shift
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                //inserting holiday
                objEmpHoliday.InsertEmployeeHolidayMaster(strCompanyid, dr["ref_Id"].ToString(), DtFromDate.ToString(), strEmpId, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;

            }
            if (dr["ref_Type"].ToString().Trim() == "")
            {
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;
            }

            //here we are deleting shift for specific day

            objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

            strShiftId = dr["ref_id"].ToString();

            dtShift = objShift.GetShiftMasterById(strCompanyid, strShiftId, ref trns);
            dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(strShiftId, ref trns);

            if (dtSch != null)
            {
                dtTempsch = new DataView(dtSch, "Shift_Id='" + strShiftId + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTempsch.Rows.Count == 0)
                {

                    b = objEmpSch.InsertScheduleMaster(strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtTempsch.Rows[0]["Schedule_Id"].ToString(), strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                    //DisplayMessage("Shift Already Assignd to this Employee");
                    //return;
                }
            }
            else
            {
                b = objEmpSch.InsertScheduleMaster(strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
            }


            // From Date to To Date

            dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());


            int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
            int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

            string cycletype = string.Empty;
            string cycleday = string.Empty;

            if (index == 7)
            {
                int daysShift = cycle * index;
                string weekday = DtFromDate.DayOfWeek.ToString();

                int k = Att_ScheduleMaster.GetCycleDay(weekday);
                int j = 1;
                int a = k;
                int f = 0;

                if (k % 7 == 0)
                {
                    if (f != 0)
                    {
                        if (k % 7 == 0 && j > cycle)
                        {
                            j++;
                            rem = 1;
                        }
                        //else
                        //{
                        //    j++;
                        //}

                        if (j > cycle)
                        {
                            j = 1;
                        }
                    }

                }
                f++;
                if (k <= daysShift || j == cycle)
                {


                    a = Att_ScheduleMaster.GetCycleDay(DtFromDate.DayOfWeek.ToString());
                    if (rem == 1 && k % 7 == 0)
                    {
                        j++;
                    }

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (k % 7 == 0 && j == cycle)
                    {
                        j = 1;
                    }
                    if (k % 7 == 0 && j < cycle)
                    {
                        j++;
                    }

                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);


                    if (dtGetTemp1.Rows.Count > 0)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dtTime.Rows.Count > 0)
                        {
                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                            {

                                //here we are deleting shift for selected date 

                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }
                                else
                                {

                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {

                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {


                                        if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                        {
                                            flag1 = 1;
                                            OverlapDate += dtTime.Rows[s]["Att_Date"].ToString() + ",";
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                    }
                                }


                            }
                        }
                        else
                        {
                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }

                            }
                            else
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                            }

                        }
                    }
                    else
                    {
                        //Modified accoding to excludedays parameter 19 sept 2013 kunal
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                            }
                            else
                            {
                                // Modified By Nitin Jain On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (str == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                                //..............
                            }

                        }
                        else
                        {


                        }
                    }

                    k++;
                }
                else
                {
                    k = 1;
                    j = 1;
                    f = 0;
                    a = Att_ScheduleMaster.GetCycleDay(DtFromDate.DayOfWeek.ToString());

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                    if (dtGetTemp1.Rows.Count > 0)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dtTime.Rows.Count > 0)
                        {
                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                            {
                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }
                                else
                                {

                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {
                                        if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                        {
                                            flag1 = 1;
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                    }
                                }

                            }
                        }
                        else
                        {
                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }

                            }
                            else
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                            }
                        }
                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modifed By Nitin On 27/8/2014/////
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }
                    k++;

                }

            }
            else if (index == 31)
            {
                int daysShift = cycle * index;

                int k = DtFromDate.Day;
                int a = 0;
                int j = 1;
                int mon = DtFromDate.Month;
                if (k <= daysShift)
                {
                    a = DtFromDate.Day;

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //
                    if (dtGetTemp1.Rows.Count > 0)
                    {


                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                        {
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                            {

                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                if (dts.Rows.Count == 0)
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                int flag1 = 0;

                                if (dtTime.Rows.Count > 0)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {


                                            if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                        }
                                    }

                                }
                                else
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                        }

                                    }

                                }
                            }

                        }



                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }
                }

                k++;
                if (k > daysShift)
                {

                    k = 1;
                    j = 1;
                }
                if (DtFromDate.Day == 1)
                {

                    j++;

                }
            }
            else if (index == 1)
            {
                int k = 1;
                int a = k;
                int daysShift = cycle * index;
                if (k <= daysShift)
                {
                    a = k;


                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //
                    if (dtGetTemp1.Rows.Count > 0)
                    {


                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                        {
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                            {

                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                if (dts.Rows.Count == 0)
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                int flag1 = 0;

                                if (dtTime.Rows.Count > 0)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(Att_ScheduleMaster.GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {

                                            if (Att_ScheduleMaster.ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                        }

                                    }
                                }
                                else
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {

                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                        }
                                    }

                                }
                            }

                        }

                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }


                }

                k++;
                if (k > daysShift)
                {

                    k = 1;

                }

            }

            DtFromDate = DtFromDate.AddDays(1);
        }


        dtTempsch.Dispose();
        dtSch.Dispose();
        dtShift.Dispose();
        dtShiftD.Dispose();
        dtTime.Dispose();
        dtTempShift.Dispose();
        return true;
    }

    public void UpdateLeaveBalanceForLeaveSalary(string strRef_Id, string strStatus, ref SqlTransaction trns)
    {
        double UsedDays_Trans = 0;
        double PaidDays_Trans = 0;
        double Remainingdays_Trans = 0;
        DataTable dtleave = new DataTable();
        DataTable dtLeaveTrans = new DataTable();
        dtleave = objDA.return_DataTable("select Emp_id,Leave_Type_Id as LeaveId,SUM( CAST(F2 as numeric(18,3))) as TotalLeave,SUM(CAST(F3 as numeric(18,3))) as UsedLeave,L_year,SUM(total) as Total from att_leavesalary where F6=" + strRef_Id + " group by Leave_Type_Id,Emp_id,L_year", ref trns);
        foreach (DataRow dr in dtleave.Rows)
        {
            if (Convert.ToDouble(dr["Total"].ToString()) < 0)
            {
                continue;
            }
            dtLeaveTrans = objDA.return_DataTable("select Trans_Id,Att_Employee_Leave_Trans.Leave_Type_Id,Att_Employee_Leave_Trans.used_days,Att_Employee_Leave_Trans.Remaining_Days,Att_Employee_Leave_Trans.Field2 from Att_Employee_Leave_Trans inner join Att_LeaveMaster on Att_Employee_Leave_Trans.Leave_Type_Id = Att_LeaveMaster.Leave_Id where Att_Employee_Leave_Trans.emp_id = " + dr["Emp_Id"].ToString() + " and Att_LeaveMaster.Field1 = 'True' and Att_LeaveMaster.IsActive = 'True' and Att_Employee_Leave_Trans.Year = '" + dr["L_year"].ToString() + "' and Att_Employee_Leave_Trans.Field1 <> '0' and Att_Employee_Leave_Trans.IsActive='True' and Att_Employee_Leave_Trans.Leave_Type_Id='" + dr["LeaveId"].ToString() + "'", ref trns);
            //used leave updated for india location
            if (dtLeaveTrans.Rows.Count > 0)
            {
                UsedDays_Trans = Convert.ToDouble(dtLeaveTrans.Rows[0]["used_days"].ToString());
                PaidDays_Trans = Convert.ToDouble(dtLeaveTrans.Rows[0]["Field2"].ToString());
                Remainingdays_Trans = Convert.ToDouble(dtLeaveTrans.Rows[0]["Remaining_Days"].ToString());
                if (strStatus == "Approved")
                {
                    UsedDays_Trans = UsedDays_Trans + (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                    PaidDays_Trans = PaidDays_Trans - (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                    Remainingdays_Trans = Remainingdays_Trans - (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                }
                else
                {
                    UsedDays_Trans = UsedDays_Trans - (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                    PaidDays_Trans = PaidDays_Trans + (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                    Remainingdays_Trans = Remainingdays_Trans + (Convert.ToDouble(dr["TotalLeave"].ToString()) - Convert.ToDouble(dr["usedLeave"].ToString()));
                }
                objDA.execute_Command("update Att_Employee_Leave_Trans set used_days=" + UsedDays_Trans + ",Remaining_Days=" + Remainingdays_Trans + ",Field2='" + PaidDays_Trans + "' where trans_id=" + dtLeaveTrans.Rows[0]["Trans_Id"].ToString() + "", ref trns);
            }
        }
    }
    private void Set_Notification(string Str_Sender_Name, string Str_Request_For_Name, string Company_ID, string Brand_ID, string Location_ID, string Sender_ID, string Request_For_ID, string Receipent_ID, string Approval_Type, string Approval_ID, string Ref_Table_PK, string Ref_ID, string ReSend_Request, string Trans_ID)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        Dt_Request_Type = Obj_Notifiacation.Get_Request_Notification_ID(Approval_ID);
        DataTable Dt_Request_For = new DataTable();
        Dt_Request_For = Obj_Notifiacation.Get_Request_For(Company_ID, Location_ID, Brand_ID, Sender_ID, Ref_ID, Dt_Request_Type.Rows[0]["notification_type_id"].ToString());
        string Request_URL = string.Empty;
        string Message = string.Empty;
        string Request_URL_For_Sender = string.Empty;
        if (Dt_Request_For.Rows.Count > 0)
        {
            if (Dt_Request_For.Rows[0]["Request_for_id"].ToString() == Dt_Request_For.Rows[0]["Sender_id"].ToString())
            {
                if (Approval_ID == "1")
                {
                    Request_URL = "../Attendance/LeaveApproval.aspx?Emp_Id=0";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "2")
                {
                    Request_URL = "../Attendance/HalfDayRequestByEmployee.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "3")
                {
                    Request_URL = "../Attendance/Employee_PartialLeaveRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "4")
                {
                    Request_URL = "../HR/PayEmployeeClaim.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "5")
                {
                    Request_URL = "../HR/Pay_Employee_Loan.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "6")
                {
                    Request_URL = "../Purchase/PurchaseRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "7")
                {
                    Request_URL = "../Purchase/PurchaseOrder.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "8")
                {
                    Request_URL = "../Sales/SalesQuotation.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "9")
                {
                    Request_URL = "../Sales/SalesOrder.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "10")
                {
                    Request_URL = "../Sales/SalesInvoice.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "11")
                {
                    Request_URL = "";
                    Message = "";
                }
                else if (Approval_ID == "12")
                {
                    Request_URL = "../MasterSetup/SalaryIncrement.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "13")
                {
                    Request_URL = "../Attendance/OverTimeRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "14")
                {
                    Request_URL = "../MasterSetup/CustomerMaster.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "15")
                {
                    Request_URL = "../VoucherEntries/PaymentVouchers.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "16")
                {
                    Request_URL = "../MasterSetup/SupplierMaster.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "20")
                {
                    Request_URL = "../EMS/MailMarketing.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                {
                    Request_URL = "../Attendance/ShiftAssignToEmp.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("salary"))
                {
                    Request_URL = "../HR/EmployeeLeaveSalary.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Company_ID, Brand_ID, Location_ID, Sender_ID, Request_For_ID, Request_For_ID, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Ref_Table_PK, "False", ViewState["Emp_Img"].ToString(), ReSend_Request, Trans_ID, "", "", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Ref_ID, "1");
                //Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Company_ID, Brand_ID, Location_ID, Sender_ID, Request_For_ID, Sender_ID, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Ref_Table_PK, "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), System.Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Ref_ID, "2");
            }
            else
            {
                if (Approval_ID == "1")
                {
                    Request_URL = "../Attendance/LeaveApproval.aspx?Emp_Id=0";
                    Request_URL_For_Sender = "../Attendance/LeaveApproval.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "2")
                {
                    Request_URL = "../Attendance/HalfDayRequestByEmployee.aspx";
                    Request_URL_For_Sender = "../Attendance/HalfDayRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "3")
                {
                    Request_URL = "../Attendance/Employee_PartialLeaveRequest.aspx";
                    Request_URL_For_Sender = "../Attendance/HR_PartialLeaveRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "4")
                {
                    Request_URL = "../HR/EmployeeClaimRequest.aspx";
                    Request_URL_For_Sender = "../HR/PayEmployeeClaim.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "5")
                {
                    Request_URL = "../HR/EmployeeLoanRequest.aspx";
                    Request_URL_For_Sender = "../HR/Pay_Employee_Loan.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "6")
                {
                    Request_URL = "../Purchase/PurchaseRequest.aspx";
                    Request_URL_For_Sender = "../Purchase/PurchaseRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "7")
                {
                    Request_URL = "../Purchase/PurchaseOrder.aspx";
                    Request_URL_For_Sender = "../Purchase/PurchaseOrder.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "8")
                {
                    Request_URL = "../Sales/SalesQuotation.aspx";
                    Request_URL_For_Sender = "../Sales/SalesQuotation.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "9")
                {
                    Request_URL = "../Sales/SalesOrder.aspx";
                    Request_URL_For_Sender = "../Sales/SalesOrder.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "10")
                {
                    Request_URL = "../Sales/SalesInvoice.aspx";
                    Request_URL_For_Sender = "../Sales/SalesInvoice.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "11")
                {
                    Request_URL = "";
                    Request_URL_For_Sender = "";
                    Message = "";
                }
                else if (Approval_ID == "12")
                {
                    Request_URL = "../MasterSetup/SalaryIncrement.aspx";
                    Request_URL_For_Sender = "../MasterSetup/SalaryIncrement.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "13")
                {
                    Request_URL = "../Attendance/OverTimeRequest.aspx";
                    Request_URL_For_Sender = "../Attendance/OverTimeRequest.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "14")
                {
                    Request_URL = "../MasterSetup/CustomerMaster.aspx";
                    Request_URL_For_Sender = "../MasterSetup/CustomerMaster.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "15")
                {
                    Request_URL = "../VoucherEntries/PaymentVouchers.aspx";
                    Request_URL_For_Sender = "../VoucherEntries/PaymentVouchers.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "16")
                {
                    Request_URL = "../MasterSetup/SupplierMaster.aspx";
                    Request_URL_For_Sender = "../MasterSetup/SupplierMaster.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (Approval_ID == "20")
                {
                    Request_URL = "../EMS/MailMarketing.aspx";
                    Request_URL_For_Sender = "../EMS/MailMarketing.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                {
                    Request_URL = "../Attendance/ShiftAssignToEmp.aspx";
                    Request_URL_For_Sender = "../Attendance/ShiftAssignToEmp.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                else if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("salary"))
                {
                    Request_URL = "../HR/EmployeeLeaveSalary.aspx";
                    Request_URL_For_Sender = "../HR/EmployeeLeaveSalary.aspx";
                    Message = Str_Sender_Name + " " + ReSend_Request + " " + Approval_Type + " for " + Str_Request_For_Name + " on " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString();
                }
                Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Company_ID, Brand_ID, Location_ID, Sender_ID, Request_For_ID, Dt_Request_For.Rows[0]["Sender_id"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL_For_Sender, "Set_Approval_Transaction", Ref_Table_PK, "False", ViewState["Emp_Img"].ToString(), ReSend_Request, Trans_ID, "", "", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Ref_ID, "1");
                Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Company_ID, Brand_ID, Location_ID, Sender_ID, Request_For_ID, Dt_Request_For.Rows[0]["Request_for_id"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Ref_Table_PK, "False", ViewState["Emp_Img"].ToString(), ReSend_Request, Trans_ID, "", "", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Ref_ID, "2");
            }
        }
    }
    protected void Reject_Command(object sender, CommandEventArgs e)
    {
        string Str_Request_For_Name = string.Empty;
        string Str_Sender_Name = string.Empty;
        DataTable dtapproval = new DataTable();
        DataTable dt = new DataTable();
        if (ddlApprovalType.SelectedValue.Trim() == "1")
        {
            dt = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
            dtapproval = dt.Copy();
        }
        else
        {
            dt = objEmpApproval.GetApprovalTransationbyTransId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        }
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        string strrejectReason = string.Empty;
        if (txtDescription.Text != "")
        {
            strrejectReason = " and Given Reason is '" + txtDescription.Text + "'";
        }
        bool IsPriority = false;
        string transId = e.CommandArgument.ToString();
        dt = new DataView(dt, "Trans_Id='" + transId + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        if (Convert.ToBoolean(dt.Rows[0]["Priority"].ToString()) && dt.Rows[0]["Status"].ToString().Trim() == "Rejected")
        {
            DisplayMessage("Request is already rejected");
            return;
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            Str_Request_For_Name = GetEmployeeName(dt.Rows[0]["request_emp_id"].ToString());
            Str_Sender_Name = GetEmployeeName(dt.Rows[0]["Emp_Id"].ToString());
            string EmpId = GetEmpIdByUserId();
            string strsql = string.Empty;
            DataTable dtPriority = new DataTable();
            DataTable dtLogPosted = new DataTable();
            switch (Convert.ToInt32(ddlApprovalType.SelectedValue))
            {
                case 1:
                    strsql = "select Att_LeaveRequest_header.Emp_Id from Att_LeaveRequest_header inner join pay_employee_attendance on Att_LeaveRequest_header.Emp_Id =pay_employee_attendance.Emp_Id and (((DATEPART(month, Att_LeaveRequest_header.From_date)=Pay_Employee_Attendance.Month) and (DATEPART(YEAR, Att_LeaveRequest_header.From_Date)=Pay_Employee_Attendance.Year))  or ((DATEPART(month, Att_LeaveRequest_header.to_date)=Pay_Employee_Attendance.Month) and (DATEPART(YEAR, Att_LeaveRequest_header.to_Date)=Pay_Employee_Attendance.Year)))   and Att_LeaveRequest_header.Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString() + "";
                    dtLogPosted = objDA.return_DataTable(strsql, ref trns);
                    if (dtLogPosted.Rows.Count > 0)
                    {
                        DisplayMessage("Log has posted , you can not reject");
                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        dt.Dispose();
                        dtLogPosted.Dispose();
                        dtPriority.Dispose();
                        return;
                    }
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='1'" + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        Reject_Leave(dtapproval, dt.Rows[0]["Ref_Id"].ToString(), transId, "ByReject", strrejectReason, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 2:
                    strsql = "select Att_HalfDay_Request.Emp_Id from Att_HalfDay_Request inner join pay_employee_attendance on Att_HalfDay_Request.Emp_Id =pay_employee_attendance.Emp_Id and (DATEPART(month, Att_HalfDay_Request.halfday_date)=Pay_Employee_Attendance.Month )  and (DATEPART(Year,  Att_HalfDay_Request.HalfDay_Date)=Pay_Employee_Attendance.Year) and Att_HalfDay_Request.Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString() + "";
                    dtLogPosted = objDA.return_DataTable(strsql, ref trns);
                    if (dtLogPosted.Rows.Count > 0)
                    {
                        DisplayMessage("Log has posted , you can not reject");
                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        dt.Dispose();
                        dtLogPosted.Dispose();
                        dtPriority.Dispose();
                        return;
                    }
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='2'" + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        Reject_HalfDay(dt.Rows[0]["Ref_Id"].ToString(), transId, strrejectReason, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 3:
                    strsql = "select Att_PartialLeave_Request.Emp_Id from Att_PartialLeave_Request inner join pay_employee_attendance on Att_PartialLeave_Request.Emp_Id =pay_employee_attendance.Emp_Id and (DATEPART(month, Att_PartialLeave_Request.Partial_Leave_Date)=Pay_Employee_Attendance.Month )  and (DATEPART(Year,  Att_PartialLeave_Request.Partial_Leave_Date)=Pay_Employee_Attendance.Year) and Att_PartialLeave_Request.Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString() + "";
                    dtLogPosted = objDA.return_DataTable(strsql, ref trns);
                    if (dtLogPosted.Rows.Count > 0)
                    {
                        DisplayMessage("Log has posted , you can not reject");
                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        dt.Dispose();
                        dtLogPosted.Dispose();
                        dtPriority.Dispose();
                        return;
                    }
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='3'" + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        Reject_PartialLeave(dt.Rows[0]["Ref_Id"].ToString(), transId, strrejectReason, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 4:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='4' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        DataTable dtClaim = new DataTable();
                        dtClaim = objPayClaim.GetRecord_From_PayEmployeeClaimByClaimId(dt.Rows[0]["Ref_Id"].ToString(), ref trns);
                        int UpdationCheck = 0;
                        if (dtClaim.Rows.Count > 0)
                        {
                            UpdationCheck = objPayClaim.UpdateRecord_In_Pay_Employee_Claim(Session["CompId"].ToString(), dtClaim.Rows[0]["Claim_Id"].ToString(), dtClaim.Rows[0]["Claim_Name"].ToString(), dtClaim.Rows[0]["Claim_Description"].ToString(), dtClaim.Rows[0]["Value_Type"].ToString(), dtClaim.Rows[0]["Value"].ToString(), dtClaim.Rows[0]["Claim_Month"].ToString(), dtClaim.Rows[0]["Claim_Year"].ToString(), "Cancelled", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                        }
                    }
                    break;
                case 5:
                    DataTable Dt_Emp_Payment_Loan = objLoan.Get_Employee_Loan(dt.Rows[0]["Ref_Id"].ToString(), "0", "0", "True", "True", "2");
                    if (Dt_Emp_Payment_Loan != null && Dt_Emp_Payment_Loan.Rows.Count > 0)
                    {
                        DisplayMessage("Loan Payment Voucher Is Created, So It cannot be Rejected !");
                        return;
                    }
                    else
                    {
                        dtPriority = new DataView(dt, "Emp_Id='" + Session["EmpId"].ToString() + "' and Approval_Id='5' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtPriority.Rows.Count > 0)
                        {
                            IsPriority = true;
                            DataTable dtLoan = objLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "", ref trns);
                            if (dtLoan.Rows.Count > 0)
                            {
                                int b = 0;
                                b = objLoan.UpdateRecord_In_Pay_Employee_Loan(Session["CompId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), dtLoan.Rows[0]["Loan_Amount"].ToString(), dtLoan.Rows[0]["Loan_Request_Date"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), dtLoan.Rows[0]["Loan_Duration"].ToString(), dtLoan.Rows[0]["Loan_Interest"].ToString(), dtLoan.Rows[0]["Gross_Amount"].ToString(), dtLoan.Rows[0]["Monthly_Installment"].ToString(), "Cancelled", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                            }
                        }
                    }
                    break;
                case 6:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='6' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objPurrchaseRequestHeader.UpdatePurchaseRequestHeaderApproval(dt.Rows[0]["Ref_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Rejected", false.ToString(), false.ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 7:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='7' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        ObjPurchaseOrder.UpdatePurchaseOrderApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 8:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='8' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objSQuoteHeader.UpdateSalesQuotationApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 9:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='9' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objSOrderHeader.UpdateSalesOrderApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 10:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='10' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 12:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='12' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        int a = 0;
                        //PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass();
                        string sql = "select * from dbo.HR_Salary_Increment where Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString().Trim() + "";
                        DataTable dtSalary = objDA.return_DataTable(sql, ref trns);
                        if (dtSalary.Rows.Count > 0)
                        {
                            sql = "update dbo.HR_Salary_Increment set Field1='Rejected' where Trans_Id=" + dtSalary.Rows[0]["Trans_Id"].ToString() + "";
                            a = objDA.execute_Command(sql, ref trns);
                            //updated by jitendra upadhyay and prahlad sir on 28-01-2016
                            //code start

                            //Double Comment on 04-03-2024 By Lokesh	
                            ////sql = "Update Set_Emp_SalaryIncrement SET Month=" + Convert.ToDateTime(dtSalary.Rows[0]["Field2"].ToString()).Month.ToString() + ",Year =" + Convert.ToDateTime(dtSalary.Rows[0]["Field2"].ToString()).Year.ToString() + ",Basic_Salary=" + dtSalary.Rows[0]["Increment_Salary"].ToString() + " where Emp_Id=" + dtSalary.Rows[0]["Employee_Id"].ToString() + "";
                            ////a = objDA.execute_Command(sql, ref trns);
                            ////sql = "Update Set_EmployeeMaster  Set Field7 ='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "' WHERE Emp_Id =" + dtSalary.Rows[0]["Employee_Id"].ToString() + "";
                            ////a = objDA.execute_Command(sql, ref trns);
                            ////sql = "Update Set_Employee_Parameter SET Basic_Salary =" + dtSalary.Rows[0]["Increment_Salary"].ToString() + " Where Emp_Id = " + dtSalary.Rows[0]["Employee_Id"].ToString() + "";
                            ////a = objDA.execute_Command(sql, ref trns);

                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                            //code end
                        }
                    }
                    break;
                case 13:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='13' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objOverTimeReq.UpdateOvertimeRequestByTransId(dt.Rows[0]["Ref_Id"].ToString(), Session["CompId"].ToString(), false.ToString(), false.ToString(), true.ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 14:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='14' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        // objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        //objCustomer.UpdateCustomerStatus(dt.Rows[0]["Ref_Id"].ToString(), "Rejected", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        string sql = "Update Set_CustomerMaster_CreditParameter set field4='Rejected', ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "'  where field2='C' and field3='" + dt.Rows[0]["Ref_Id"].ToString() + "'";
                        objDA.execute_Command(sql, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 15:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='15' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        DataTable dtVoucherHeaderData = objDA.return_DataTable("select * from ac_voucher_header where company_id='" + Session["CompId"].ToString() + "' and trans_id=" + dt.Rows[0]["Ref_Id"].ToString());
                        if (dtVoucherHeaderData.Rows.Count > 0)
                        {
                            string strVoucherBrandId = dtVoucherHeaderData.Rows[0]["brand_id"].ToString();
                            string strVoucherLocationId = dtVoucherHeaderData.Rows[0]["location_id"].ToString();
                            IsPriority = true;
                            objVoucherHeader.UpdateVoucherHeaderApprovalStatus(Session["CompId"].ToString(), strVoucherBrandId, strVoucherLocationId, dt.Rows[0]["Ref_Id"].ToString(), "Rejected", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                            //if (objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Auto_Ageing_Settlement").Rows.Count > 0)
                            //{
                            //    if (objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Auto_Ageing_Settlement").Rows[0]["Param_Value"].ToString() == "True")
                            //    {
                            objDA.execute_Command("update ac_ageing_detail set isActive='false' where company_id='" + Session["CompId"].ToString() + "' and brand_id='" + strVoucherBrandId + "' and location_id='" + strVoucherLocationId + "' and voucherId='" + dt.Rows[0]["Ref_Id"].ToString() + "'", ref trns);
                            Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                        }
                    }
                    break;
                case 16:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='16' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        // objSInvHeader.UpdateSalesInvoiceApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), true.ToString());
                        //objSupplier.UpdateSupplierStatus(dt.Rows[0]["Ref_Id"].ToString(), "Rejected", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        string sql = "Update Set_CustomerMaster_CreditParameter set field4='Rejected', ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "'  where field2='S' and field3='" + dt.Rows[0]["Ref_Id"].ToString() + "'";
                        objDA.execute_Command(sql, ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 17:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='17' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                        {
                            ObjTempEmpShift.UpdateHeaderApprovalStatus(dt.Rows[0]["Ref_Id"].ToString(), "Rejected", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        }
                        else
                        {
                            if (objDA.return_DataTable("select Field3 from  ac_voucher_header  where Ref_Type='Leave Salary' and Ref_id=" + dt.Rows[0]["Ref_Id"].ToString() + " and ReconciledFromFinance='True'", ref trns).Rows.Count > 0)
                            {
                                DisplayMessage("Finance voucher posted, you can not reject");
                                return;
                            }
                            if (objDA.return_DataTable("select emp_id from att_leavesalary where F6=" + dt.Rows[0]["Ref_Id"].ToString() + " and Is_Report='True'", ref trns).Rows.Count > 0)
                            {
                                DisplayMessage("Leave salary paid to employee, you can not reject");
                                return;
                            }
                            objDA.execute_Command("update att_leavesalary set f5='Rejected' where F6=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            objDA.execute_Command("update ac_voucher_header set Field3='Rejected' where Ref_Type='Leave Salary' and Ref_id=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            UpdateLeaveBalanceForLeaveSalary(dt.Rows[0]["Ref_Id"].ToString(), "Rejected", ref trns);
                        }
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 18:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='18' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        if (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift"))
                        {
                            ObjTempEmpShift.UpdateHeaderApprovalStatus(dt.Rows[0]["Ref_Id"].ToString(), "Rejected", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                        }
                        else
                        {
                            if (objDA.return_DataTable("select Field3 from  ac_voucher_header  where Ref_Type='Leave Salary' and Ref_id=" + dt.Rows[0]["Ref_Id"].ToString() + " and ReconciledFromFinance='True'", ref trns).Rows.Count > 0)
                            {
                                DisplayMessage("Finance voucher posted, you can not reject");
                                return;
                            }
                            if (objDA.return_DataTable("select emp_id from att_leavesalary where F6=" + dt.Rows[0]["Ref_Id"].ToString() + " and Is_Report='True'", ref trns).Rows.Count > 0)
                            {
                                DisplayMessage("Leave salary paid to employee, you can not reject");
                                return;
                            }
                            objDA.execute_Command("update att_leavesalary set f5='Rejected' where F6=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            objDA.execute_Command("update ac_voucher_header set Field3='Rejected' where Ref_Type='Leave Salary' and Ref_id=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                            UpdateLeaveBalanceForLeaveSalary(dt.Rows[0]["Ref_Id"].ToString(), "Rejected", ref trns);
                        }
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
                case 20:
                    dtPriority = new DataView(dt, "Emp_Id='" + EmpId + "' and Approval_Id='20' and Priority='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPriority.Rows.Count > 0)
                    {
                        IsPriority = true;
                        objDA.execute_Command("update Ems_TemplateSelection_Header set Field5='Rejected' where Trans_Id=" + dt.Rows[0]["Ref_Id"].ToString() + "", ref trns);
                        Set_Notification(Str_Sender_Name, Str_Request_For_Name, dt.Rows[0]["Company_Id"].ToString(), dt.Rows[0]["Brand_Id"].ToString(), dt.Rows[0]["Location_Id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["request_emp_id"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Approval_Id"].ToString(), dt.Rows[0]["Trans_Id"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "Rejected", transId);
                    }
                    break;
            }
            string Description = string.Empty;
            if (txtDescription.Text != "")
            {
                Description = txtDescription.Text;
            }
            if (IsPriority)
            {
                Description += " Rejected By-" + GetEmployeeName(dt.Rows[0]["Emp_Id"].ToString(), ref trns);
            }
            else
            {
                if (Description.Trim() == "")
                {
                    Description = " Rejected";
                }
            }
            objEmpApproval.UpdateApprovalTransaciton(dt.Rows[0]["Approval_Name"].ToString(), dt.Rows[0]["Ref_Id"].ToString(), "0", dt.Rows[0]["Emp_Id"].ToString(), "Rejected", Description, dt.Rows[0]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
            //this code for update for reject record
            //
            string strPriority = string.Empty;
            DataTable dtappproval = ObjapprovalMaster.GetApprovalMasterById(ddlApprovalType.SelectedValue, ref trns);
            string strHierarchyRules = string.Empty;
            string strApprovalType = dtappproval.Rows[0]["Approval_Type"].ToString();
            if (dtappproval.Rows[0]["Is_Open"] != null)
            {
                strHierarchyRules = dtappproval.Rows[0]["Is_Open"].ToString();
            }
            if (strApprovalType.Trim() == "Hierarchy")
            {
                //if (strApprovalType.Trim() == "Hierarchy" && strHierarchyRules == "False")
                //{
                objDA.execute_Command("update Set_Approval_Transaction set Status='Rejected',Status_Update_Date='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "',Description='" + Description + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString() + "' where  Ref_Id=" + dt.Rows[0]["Ref_Id"].ToString() + "  and Approval_Id=" + dt.Rows[0]["Approval_Id"].ToString() + "  and Is_Default='True'", ref trns);
            }
            DisplayMessage("Request Rejected");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            dtPriority.Dispose();
            FillGrid(hdnApprovalType.Value);
            dt.Dispose();
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
    protected void btnUpdateApproval_Click(object sender, EventArgs e)
    {
        ImageButton imgeditbutton = new ImageButton();
        imgeditbutton.ID = "lnkViewDetail";
        Approve_Command(imgeditbutton, new CommandEventArgs("commandName", hdnTransId.Value));
        pnlApproval1.Visible = false;
        pnlApproval1.Visible = false;
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
    }
    protected void btnCancelPopReject_Click(object sender, EventArgs e)
    {
        ImageButton imgeditbutton = new ImageButton();
        imgeditbutton.ID = "lnkViewDetail";
        Reject_Command(imgeditbutton, new CommandEventArgs("commandName", hdnTransId.Value));
        pnlApproval1.Visible = false;
        pnlApproval1.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
    }
    protected void Approve_Leave(DataTable dtApproval, string Ref_Id, string strTrans_Id, ref SqlTransaction trns)
    {
        int b = 0;
        DataTable dtFullDay = new DataTable();
        DataTable dt = new DataTable();
        string Leave_Type_Id = "";
        string empid = string.Empty;
        string DaysCount = string.Empty;
        DateTime FromDate = new DateTime();
        DataTable dt1 = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt1.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt1.Rows[0]["Param_Value"].ToString());
        }
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        DataTable dtleaveRquestDetail = objDA.return_DataTable("select Trans_id,Leave_Type_Id,From_Date,To_Date,Emp_Id from Att_Leave_Request where Field2='" + Ref_Id + "'", ref trns);
        objDA.execute_Command("update Att_LeaveRequest_header set Is_Pending='" + false.ToString() + "',Is_Approved='" + true.ToString() + "',Is_Canceled='" + false.ToString() + "' where Trans_id=" + Ref_Id + "", ref trns);
        string strFromDate = DateTime.Parse(dtleaveRquestDetail.Rows[0]["From_Date"].ToString()).ToString("dd-MMM-yyyy");
        string strToDate = DateTime.Parse(dtleaveRquestDetail.Rows[0]["To_Date"].ToString()).ToString("dd-MMM-yyyy");
        dtFullDay = new DataView(dtApproval, "Ref_Id='" + Ref_Id + "' and Priority='True' and Approval_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
        foreach (DataRow drdetail in dtleaveRquestDetail.Rows)
        {
            b = objleaveReq.UpdateLeaveRequestByTransId(drdetail["Trans_Id"].ToString(), Session["CompId"].ToString(), false.ToString(), true.ToString(), false.ToString(), "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
            dt = objleaveReq.GetLeaveRequest_ByTrans_ID(Session["CompId"].ToString(), drdetail["Emp_Id"].ToString(), drdetail["Trans_Id"].ToString(), ref trns);
            if (dt.Rows.Count > 0)
            {
                FromDate = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString());
                empid = dt.Rows[0]["Emp_Id"].ToString();
                DaysCount = dt.Rows[0]["DaysCount"].ToString();
                Leave_Type_Id = dt.Rows[0]["Leave_Type_Id"].ToString();
            }
            if (b != 0)
            {
                string Schedule = string.Empty;
                DataTable dtLeave = new DataTable();
                dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(empid, Leave_Type_Id, FromDate.Month.ToString(), FromDate.Year.ToString(), ref trns);
                if (dtLeave.Rows.Count > 0)
                {
                    Schedule = "Monthly";
                }
                else
                {
                    dtLeave = objDA.return_DataTable("select * from Att_Employee_Leave_Trans where Emp_Id=" + empid + " and Leave_Type_Id=" + Leave_Type_Id + " and Field3='Open' and IsActive = 'True'", ref trns);
                    // dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(empid, Leave_Type_Id, "0", FinancialYearStartDate.Year.ToString(), ref trns);
                    Schedule = "Yearly";
                }
                double remain = 0;
                double useddays = 0;
                int Totaldays = 0;
                double RemainPaidLeave = 0;
                int PendingDays = 0;
                if (dtLeave.Rows.Count > 0)
                {
                    if (dtLeave.Rows.Count > 0)
                    {
                        PendingDays = int.Parse(dtLeave.Rows[0]["Pending_Days"].ToString());
                        remain = Convert.ToDouble(dtLeave.Rows[0]["Remaining_Days"].ToString());
                        useddays = Convert.ToDouble(dtLeave.Rows[0]["Used_Days"].ToString());
                        Totaldays = int.Parse(dtLeave.Rows[0]["Total_Days"].ToString());
                        RemainPaidLeave = Convert.ToDouble(dtLeave.Rows[0]["Field2"].ToString());
                    }
                    // useddays = PendingDays + useddays;
                    useddays = Convert.ToInt32(dt.Rows[0]["DaysCount"].ToString()) + useddays;
                    if (PendingDays != 0)
                        PendingDays = PendingDays - Convert.ToInt32(dt.Rows[0]["DaysCount"].ToString());
                }
                if (Schedule == "Yearly")
                {
                    objEmpleave.UpdateEmployeeLeaveTransactionByTransNo(dtLeave.Rows[0]["Trans_Id"].ToString(), Session["CompId"].ToString(), empid, Leave_Type_Id, FinancialYearStartDate.Year.ToString(), "0", "0", Totaldays.ToString(), Totaldays.ToString(), useddays.ToString(), remain.ToString(), PendingDays.ToString(), RemainPaidLeave.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                }
                else
                {
                    objEmpleave.UpdateEmployeeLeaveTransactionByTransNo(dtLeave.Rows[0]["Trans_Id"].ToString(), Session["CompId"].ToString(), empid, Leave_Type_Id, FromDate.Year.ToString(), FromDate.Month.ToString(), "0", Totaldays.ToString(), Totaldays.ToString(), useddays.ToString(), remain.ToString(), "0", RemainPaidLeave.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                }
            }
        }
        if (b != 0)
        {
            DisplayMessage("Request Approved");
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            string strAppMailId = string.Empty;
            string strAppPassword = string.Empty;
            DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
            }
            DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
            }
            try
            {
                for (int i = 0; i < dtFullDay.Rows.Count; i++)
                {
                    MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + dtFullDay.Rows[i]["Emp_Name"].ToString() + "</h4> <hr /> Full Day Leave application has been appproved   <br /><br /> Employee Id : '" + Common.GetEmployeeCode(dtFullDay.Rows[i]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'<br /> Employee Name : " + Common.GetEmployeeName(dtFullDay.Rows[i]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Department : " + Common.GetDepartmentName(dtFullDay.Rows[i]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "<br /> Date of join :" + Common.GetEmployeeDateOfJoining(dtFullDay.Rows[i]["request_emp_id"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "" + Common.GetmailContentByLeaveTypeId(Ref_Id, dtFullDay.Rows[i]["request_emp_id"].ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString()) + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";
                    // MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtFullDay.Rows[i]["Emp_Name"].ToString() + "</h4><hr/><p>Full Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "' <br />From : '" + strFromDate + "' To : '" + strToDate + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                    ObjSendMailSms.SendApprovalMail(dtFullDay.Rows[i]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Full Day Leave Approved For " + dt.Rows[0]["Emp_Name"].ToString() + "", MailMessage.ToString(), Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
            }
            catch (Exception Ex)
            {
            }
            //MailMessage = "Your Full Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' From : '" + strFromDate + "' To : '" + strToDate + "'";
            //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Your Full Day Leave has been Approved by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "'<br />From : '" + strFromDate + "' To : '" + strToDate + "'</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4> <hr /> Your Leave Application Status " + Common.GetmailContentByLeaveTypeId(Ref_Id, dtFullDay.Rows[0]["request_emp_id"].ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString()) + "<br /> Approval Status : Approved <br />Approval By : " + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + " <br />Approval Date : " + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(objSys.SetDateFormat()) + "<br /> <h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2> </div> </body> </html>";

                ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Full Day Leave Approved For " + dt.Rows[0]["Emp_Name"].ToString() + "", MailMessage.ToString(), Session["CompId"].ToString(), "", "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch (Exception Ex)
            {
            }
        }
    }
    protected void Reject_Leave(DataTable dtApproval, string Ref_Id, string strTrans_Id, string strStatus, string strRejectReason, ref SqlTransaction trns)
    {
        double remain = 0;
        double useddays = 0;
        int Totaldays = 0;
        double RemainPaidLeave = 0;
        int PendingDays = 0;
        int TotalPaidleave = 0;
        DataTable dtleaveRquestDetail = objDA.return_DataTable("select Trans_id,Leave_Type_Id,From_Date,To_Date,Emp_Id from Att_Leave_Request where Field2='" + Ref_Id + "'", ref trns);
        DataTable dt1 = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt1.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt1.Rows[0]["Param_Value"].ToString());
        }
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        objDA.execute_Command("update Att_LeaveRequest_header set Is_Pending='" + false.ToString() + "',Is_Approved='" + false.ToString() + "',Is_Canceled='" + true.ToString() + "' where Trans_id=" + Ref_Id + "", ref trns);
        DataTable dtFullDay = new DataTable();
        DataTable dt = new DataTable();
        string strFromDate = DateTime.Parse(dtleaveRquestDetail.Rows[0]["From_Date"].ToString()).ToString("dd-MMM-yyyy");
        string strToDate = DateTime.Parse(dtleaveRquestDetail.Rows[0]["To_Date"].ToString()).ToString("dd-MMM-yyyy");
        dtFullDay = new DataView(dtApproval, "Ref_Id='" + Ref_Id + "' " + checkPriority(ref trns) + " and Approval_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
        foreach (DataRow drdetail in dtleaveRquestDetail.Rows)
        {
            string Leave_Type_Id = "";
            string empid = string.Empty;
            string DaysCount = string.Empty;
            DateTime FromDate = new DateTime();
            dt = objleaveReq.GetLeaveRequest_ByTrans_ID(Session["CompId"].ToString(), drdetail["Emp_Id"].ToString(), drdetail["Trans_Id"].ToString(), ref trns);
            if (dt.Rows.Count > 0)
            {
                string strPending = dt.Rows[0]["Is_Pending"].ToString();
                if (strPending == "True")
                {
                    strStatus = "ByReject";
                }
                else
                {
                    strStatus = "ByCancel";
                }
                FromDate = Convert.ToDateTime(dt.Rows[0]["From_Date"].ToString());
                empid = dt.Rows[0]["Emp_Id"].ToString();
                DaysCount = dt.Rows[0]["DaysCount"].ToString();
                Leave_Type_Id = dt.Rows[0]["Leave_Type_Id"].ToString();
            }
            int b = 0;
            b = objleaveReq.UpdateLeaveRequestByTransId(drdetail["Trans_Id"].ToString(), Session["CompId"].ToString(), false.ToString(), false.ToString(), true.ToString(), "", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
            if (b != 0)
            {
                string Schedule = string.Empty;
                DataTable dtLeave = new DataTable();
                objleaveReq.DeleteLeaveRequestChildByRefId(drdetail["Trans_Id"].ToString(), ref trns);
                dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(empid, Leave_Type_Id, FromDate.Month.ToString(), FromDate.Year.ToString(), ref trns);
                if (dtLeave.Rows.Count > 0)
                {
                    Schedule = "Monthly";
                }
                else
                {
                    dtLeave = objDA.return_DataTable("select * from Att_Employee_Leave_Trans where Emp_Id=" + empid + " and Leave_Type_Id=" + Leave_Type_Id + " and Field3='Open' and IsActive = 'True'", ref trns);
                    //dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(empid, Leave_Type_Id, "0", FinancialYearStartDate.Year.ToString(), ref trns);
                    Schedule = "Yearly";
                }
                if (dtLeave.Rows.Count > 0)
                {
                    if (dtLeave.Rows.Count > 0)
                    {
                        remain = Convert.ToDouble(dtLeave.Rows[0]["Remaining_Days"].ToString());
                        useddays = Convert.ToDouble(dtLeave.Rows[0]["Used_Days"].ToString());
                        Totaldays = int.Parse(dtLeave.Rows[0]["Total_Days"].ToString());
                        PendingDays = int.Parse(dtLeave.Rows[0]["Pending_Days"].ToString());
                        RemainPaidLeave = Convert.ToDouble(dtLeave.Rows[0]["Field2"].ToString());
                        TotalPaidleave = int.Parse(dtLeave.Rows[0]["Field1"].ToString());
                    }
                }
                if (strStatus == "ByReject")
                {
                    if (PendingDays > 0)
                    {
                        PendingDays = PendingDays - int.Parse(DaysCount);
                    }
                    if (useddays > 0)
                    {
                        // useddays = useddays - int.Parse(DaysCount);
                    }
                }
                else if (strStatus == "ByCancel")
                {
                    if (useddays > 0)
                    {
                        useddays = useddays - int.Parse(DaysCount);
                    }
                }
                remain = remain + int.Parse(DaysCount);
                double getmaxLeaveBalance = Attendance.GetMaxLeaveBalance(Leave_Type_Id, Session["DBConnection"].ToString());
                if (getmaxLeaveBalance > 0)
                {
                    if (remain > getmaxLeaveBalance)
                    {
                        remain = getmaxLeaveBalance;
                    }
                }
                //PendingDays = 0;
                int PaidLeave = 0;
                if (Schedule == "Yearly")
                {
                    DataTable dtL = objleaveReq.GetLeaveRequestChildByTrandId(drdetail["Trans_Id"].ToString(), ref trns);
                    if (dtL.Rows.Count > 0)
                    {
                        PaidLeave = dtL.Rows.Count;
                        RemainPaidLeave = RemainPaidLeave + PaidLeave;
                    }
                    if (RemainPaidLeave > TotalPaidleave)
                    {
                        RemainPaidLeave = TotalPaidleave;
                    }
                    objEmpleave.UpdateEmployeeLeaveTransactionByTransNo(dtLeave.Rows[0]["Trans_Id"].ToString(), Session["CompId"].ToString(), empid, Leave_Type_Id, FinancialYearStartDate.Year.ToString(), "0", "0", Totaldays.ToString(), Totaldays.ToString(), useddays.ToString(), remain.ToString(), PendingDays.ToString(), RemainPaidLeave.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                }
                else
                {
                    DataTable dtL = objleaveReq.GetLeaveRequestChildByTrandId(drdetail["Trans_Id"].ToString(), ref trns);
                    if (dtL.Rows.Count > 0)
                    {
                        PaidLeave = dtL.Rows.Count;
                        RemainPaidLeave = RemainPaidLeave + PaidLeave;
                    }
                    if (RemainPaidLeave > TotalPaidleave)
                    {
                        RemainPaidLeave = TotalPaidleave;
                    }
                    objEmpleave.UpdateEmployeeLeaveTransaction(Session["CompId"].ToString(), empid, Leave_Type_Id, FromDate.Year.ToString(), FromDate.Month.ToString(), "0", Totaldays.ToString(), Totaldays.ToString(), useddays.ToString(), remain.ToString(), "0", RemainPaidLeave.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                }
            }
        }
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {
            string strAppMailId = string.Empty;
            string strAppPassword = string.Empty;
            DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
            }
            DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
            }
            try
            {
                for (int i = 0; i < dtFullDay.Rows.Count; i++)
                {
                    MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtFullDay.Rows[i]["Emp_Name"].ToString() + "</h4><hr/><p>Full Day Leave has been Rejected by '" + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "'" + strRejectReason + "  " + Common.GetmailContentByLeaveTypeId(Ref_Id, dtFullDay.Rows[0]["Request_Emp_Id"].ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString()) + "</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                    ObjSendMailSms.SendApprovalMail(dtFullDay.Rows[i]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Full Day Leave Rejected For " + dt.Rows[0]["Emp_Name"].ToString() + "", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
            }
            catch (Exception Ex)
            {
            }

            try
            {
                MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Your Full Day Leave has been Rejected by '" + Common.GetEmployeeName(Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString()) + "'   <br />From : '" + strFromDate + "' To : '" + strToDate + "' " + Common.GetmailContentByLeaveTypeId(Ref_Id, dt.Rows[0]["Emp_Id"].ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString()) + strRejectReason + "</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Full Day Leave Rejected For " + dt.Rows[0]["Emp_Name"].ToString() + "", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch (Exception Ex)
            {
            }
        }
        dt.Dispose();
        dtFullDay.Dispose();
        dtleaveRquestDetail.Dispose();
    }
    private void Reject_PartialLeave(string Ref_Id, string strTrans_Id, string strrejectReason, ref SqlTransaction trns)
    {
        int b = 0;
        b = objPartial.PartialLeaveApproveReject(Ref_Id, "Canceled", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        if (b != 0)
        {
            DataTable dt = objPartial.GetPartialLeaveRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
            if (dt.Rows.Count > 0)
            {
                string strPartialLeaveDate = DateTime.Parse(dt.Rows[0]["Partial_Leave_Date"].ToString()).ToString("dd-MMM-yyyy");
                string strPLType = string.Empty;
                if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "0")
                {
                    strPLType = "Personal";
                }
                else if (dt.Rows[0]["Partial_Leave_Type"].ToString() == "1")
                {
                    strPLType = "Official";
                }
                //New Code Start By Lokesh On 11-03-2015
                DataTable dtPriority = null;
                DataTable dtPartialDay = objEmpApproval.GetApprovalTransationByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
                dtPartialDay = new DataView(dtPartialDay, "Ref_Id='" + Ref_Id + "' " + checkPriority(ref trns) + " and Approval_Id='3'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtPartialDay.Rows.Count > 0)
                {
                    dtPriority = new DataView(dtPartialDay, "Trans_Id='" + strTrans_Id + "' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                string strAppMailId = string.Empty;
                string strAppPassword = string.Empty;
                DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (dtFrom.Rows.Count > 0)
                {
                    strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
                }
                DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                if (dtFrom.Rows.Count > 0)
                {
                    strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
                }
                //MailMessage = "Partial Leave has been Rejected by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' On Date : '" + strPartialLeaveDate + "' From Time : '" + dt.Rows[0]["From_Time"].ToString() + "' To Time : '" + dt.Rows[0]["To_Time"].ToString() + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "'";
                try
                {
                    for (int i = 0; i < dtPartialDay.Rows.Count; i++)
                    {
                        MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtPartialDay.Rows[i]["Emp_Name"].ToString() + "</h4><hr/><p>Partial Leave(" + strPLType + ") Rejected for " + dt.Rows[0]["Emp_Name"].ToString() + " (" + strPartialLeaveDate + ") has been Rejected By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strPartialLeaveDate + "', From Time: '" + dt.Rows[0]["From_Time"].ToString() + "' To Time: '" + dt.Rows[0]["To_Time"].ToString() + "'" + strrejectReason + "</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                        ObjSendMailSms.SendApprovalMail(dtPartialDay.Rows[i]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Partial Leave(" + strPLType + ") Rejected for " + dt.Rows[0]["Emp_Name"].ToString() + " (Date : " + strPartialLeaveDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                }
                catch (Exception Ex)
                {
                }
                //MailMessage = "Your Partial Leave has been Rejected by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' For Date : '" + strPartialLeaveDate + "'From Time : '" + dt.Rows[0]["From_Time"].ToString() + "' To Time : '" + dt.Rows[0]["To_Time"].ToString() + "'";
                MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Your Partial Leave(" + strPLType + ") has been Rejected By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strPartialLeaveDate + "', From Time: '" + dt.Rows[0]["From_Time"].ToString() + "' To Time: '" + dt.Rows[0]["To_Time"].ToString() + "'" + strrejectReason + "</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                try
                {
                    ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Partial Leave(" + strPLType + ") Rejected for " + dt.Rows[0]["Emp_Name"].ToString() + " (Date : " + strPartialLeaveDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
                catch (Exception Ex)
                {
                }
                //New Code End By Lokesh On 11-03-2015
            }
            //Old Code
            //MailMessage = "Your Partial Leave has been Approved For Date : '" + dt.Rows[0]["Partial_Leave_Date"].ToString() + "' From Time : '" + dt.Rows[0]["From_Time"].ToString() + "' To Time : '" + dt.Rows[0]["To_Time"].ToString() + "'";
            //ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), AppralMail.ToString(), Passwd.ToString(), "Partial Leave Rejected", MailMessage.ToString(), Session["CompId"].ToString(), "");
            DisplayMessage("Leave Rejected");
        }
    }
    protected void Reject_HalfDay(string Ref_Id, string strTrans_Id, string strrejectReason, ref SqlTransaction trns)
    {
        //Get Status
        string strHDStatus = string.Empty;
        DataTable dtHD = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
        if (dtHD.Rows.Count > 0)
        {
            strHDStatus = dtHD.Rows[0]["Is_Confirmed"].ToString();
        }
        int b = 0;
        b = objHalfDay.HalfDayApproveReject(Ref_Id, "Canceled", true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        int RemainHalfDay = 0;
        int PendingHalfDay = 0;
        int UsedDay = 0;
        string year = string.Empty;
        int month = 0;
        DataTable dt = objHalfDay.GetHalfDayRequestByTransId(Session["CompId"].ToString(), Ref_Id, ref trns);
        if (dt.Rows.Count > 0)
        {
            string strHalfDayDate = DateTime.Parse(dt.Rows[0]["HalfDay_Date"].ToString()).ToString("dd-MMM-yyyy");
            //New Code Start By Lokesh On 11-03-2015
            DataTable dtPriority = null;
            DataTable dtHalfDay = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString(), ref trns);
            dtHalfDay = new DataView(dtHalfDay, "Ref_Id='" + Ref_Id + "' " + checkPriority(ref trns) + " and Approval_Id='2'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtHalfDay.Rows.Count > 0)
            {
                dtPriority = new DataView(dtHalfDay, "Trans_Id='" + strTrans_Id + "' " + checkPriority(ref trns) + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            string strAppMailId = string.Empty;
            string strAppPassword = string.Empty;
            DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
            }
            DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            if (dtFrom.Rows.Count > 0)
            {
                strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
            }
            //MailMessage = "'" + dt.Rows[0]["HalfDay_Type"].ToString() + "' Half Day Leave has been Rejected by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' On Date : '" + strHalfDayDate + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "'";
            try
            {
                for (int i = 0; i < dtHalfDay.Rows.Count; i++)
                {
                    MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dtHalfDay.Rows[i]["Emp_Name"].ToString() + "</h4><hr/><p>Half Day Leave(" + dt.Rows[0]["HalfDay_Type"].ToString() + ") has been Rejected By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strHalfDayDate + "' For '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' For '" + dt.Rows[0]["Emp_Name"].ToString() + "'" + strrejectReason + " </p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                    ObjSendMailSms.SendApprovalMail(dtHalfDay.Rows[i]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Half Day('" + dt.Rows[0]["HalfDay_Type"].ToString() + "')Rejected For " + dt.Rows[0]["Emp_Name"].ToString() + "(Date:" + strHalfDayDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
            }
            catch (Exception Ex)
            {
            }
            //MailMessage = "Your '" + dt.Rows[0]["HalfDay_Type"].ToString() + "' Half Day Leave has been Rejected by '" + dtPriority.Rows[0]["Emp_Name"].ToString() + "' for Date : '" + strHalfDayDate + "'";
            MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + dt.Rows[0]["Emp_Name"].ToString() + "</h4><hr/><p>Your Half Day Leave(" + dt.Rows[0]["HalfDay_Type"].ToString() + ") has been Rejected By " + dtPriority.Rows[0]["Emp_Name"].ToString() + "<br />On Date '" + strHalfDayDate + "' For '" + dt.Rows[0]["HalfDay_Type"].ToString() + "'" + strrejectReason + "</p><h3>HR</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
            try
            {
                ObjSendMailSms.SendApprovalMail(dt.Rows[0]["Email_Id"].ToString(), strAppMailId.ToString(), strAppPassword.ToString(), "Time Man:Half Day('" + dt.Rows[0]["HalfDay_Type"].ToString() + "')Rejected For " + dt.Rows[0]["Emp_Name"].ToString() + "(Date:" + strHalfDayDate + ")", MailMessage.ToString(), Session["CompId"].ToString(), "", ref trns, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            }
            catch (Exception Ex)
            {
            }
            month = Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Month;
        }
        DataTable dtApp = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dtApp.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dtApp.Rows[0]["Param_Value"].ToString());
        }
        if (month >= FinancialYearMonth)
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year).ToString();
        }
        else
        {
            year = (Convert.ToDateTime(dt.Rows[0]["HalfDay_Date"].ToString()).Year - 1).ToString();
        }
        DataTable dtEmpHalf = objEmpHalfDay.GetEmployeeHalfDayTransactionData(dt.Rows[0]["Emp_Id"].ToString(), year, ref trns);
        if (dtEmpHalf.Rows.Count > 0)
        {
            if (strHDStatus == "Pending")
            {
                RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
                PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
                PendingHalfDay = PendingHalfDay - 1;
                RemainHalfDay = RemainHalfDay + 1;
                UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
            }
            else if (strHDStatus == "Approved")
            {
                RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
                PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
                if (PendingHalfDay != 0)
                {
                    PendingHalfDay = PendingHalfDay - 1;
                }
                RemainHalfDay = RemainHalfDay + 1;
                UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
                if (UsedDay != 0)
                {
                    UsedDay = UsedDay - 1;
                }
            }
            else
            {
                RemainHalfDay = int.Parse(dtEmpHalf.Rows[0]["Remaining_Days"].ToString());
                PendingHalfDay = int.Parse(dtEmpHalf.Rows[0]["Pending_Days"].ToString());
                UsedDay = int.Parse(dtEmpHalf.Rows[0]["Used_Days"].ToString());
            }
        }
        objEmpHalfDay.UpdateEmployeeHalfDayTransaction(Session["CompId"].ToString(), dt.Rows[0]["Emp_Id"].ToString(), dtEmpHalf.Rows[0]["Year"].ToString(), "0", UsedDay.ToString(), RemainHalfDay.ToString(), PendingHalfDay.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        if (b != 0)
        {
            DisplayMessage("Request Rejected");
        }
    }
    protected void btnClosePanel_ClickApproval(object sender, EventArgs e)
    {
        pnlApproval1.Visible = false;
        pnlApproval1.Visible = false;
    }
    protected void btnCancelPopCancel_Click(object sender, EventArgs e)
    {
        pnlApproval1.Visible = false;
        pnlApproval1.Visible = false;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public string GetEmployeeName(string EmployeeId, ref SqlTransaction trns)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
        }
        return EmployeeName;
    }
    public string GetEmployeeName(string EmployeeId)
    {
        string EmployeeName = string.Empty;
        DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
            ViewState["Emp_Img"] = GetImage(Dt.Rows[0]["Emp_Image"].ToString());
        }
        else
        {
            ViewState["Emp_Img"] = "";
        }
        return EmployeeName;
    }
    public string GetImage(string EmpImage)
    {
        string Emp_Image = string.Empty;
        if (File.Exists(Server.MapPath("../CompanyResource/" + Session["CompId"] + "/" + EmpImage)) == true)
        {
            Emp_Image = "../CompanyResource/" + Session["CompId"] + "/" + EmpImage;
        }
        else
        {
            Emp_Image = "../Bootstrap_Files/dist/img/Bavatar.png";
        }
        return Emp_Image;
    }
    public string GetEmployeeCode(string EmployeeId)
    {
        string EmployeeCode = string.Empty;
        DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeCode = Dt.Rows[0]["Emp_Code"].ToString();
        }
        return EmployeeCode;
    }
    public void FilterFillGrid(string ApprovalType, string RequestEmployeeId)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["Approval"];
        try
        {
            dt = new DataView(dt, "Request_Emp_Id in (" + RequestEmployeeId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string EmpId = GetEmpIdByUserId();
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)gvApproval, dt, "", "");
                Session["Approval"] = dt;
                foreach (GridViewRow gvr in gvApproval.Rows)
                {
                    GridView gvEmp = (GridView)(gvr.FindControl("gvEmp"));
                    Label lblRequestStatus = (Label)(gvr.FindControl("lblRequestStatus"));
                    DataTable dt1 = new DataTable();
                    dt1 = objEmpApproval.GetApprovalTransation(Session["CompId"].ToString());
                    HiddenField hdnRef = (HiddenField)gvr.FindControl("hdnRefId");
                    HiddenField hdnReq = (HiddenField)gvr.FindControl("hdnRequestDate");
                    HiddenField hdnApproval = (HiddenField)gvr.FindControl("hdnApprovalType");
                    HiddenField hdnApprovalId = (HiddenField)gvr.FindControl("hdnApprovalId");
                    Label lblStaus = (Label)gvr.FindControl("lblAppStatus");
                    dt1 = new DataView(dt1, "Ref_Id='" + hdnRef.Value + "' and Approval_Id='" + hdnApprovalId.Value + "' and Approval_Name='" + lblApprovalType1.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    int flag = 0;
                    DataTable dtPri = new DataView(dt1, "Priority='True' and Emp_Id='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPri.Rows.Count > 0)
                    {
                        flag = 1;
                    }
                    int flag2 = 0;
                    DataTable dtPri2 = new DataView(dt1, "Priority='True' and Emp_Id='" + EmpId + "' and Status<>'Pending' ", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtPri2.Rows.Count > 0)
                    {
                        flag2 = 1;
                    }
                    else
                    {
                        dtPri2 = new DataView(dt1, "Priority='True'  and Status<>'Pending' ", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtPri2.Rows.Count > 0)
                        {
                            flag2 = 1;
                        }
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        DataTable dtTemp = new DataTable();
                        dtTemp.Columns.Add("Emp_Name");
                        dtTemp.Columns.Add("Emp_Id");
                        dtTemp.Columns.Add("Priority");
                        dtTemp.Columns.Add("Status");
                        dtTemp.Columns.Add("Trans_Id");
                        dtTemp.Columns.Add("Date");
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            DataRow dr = dtTemp.NewRow();
                            dr["Emp_Name"] = dt1.Rows[i]["Emp_Name"].ToString();
                            dr["Priority"] = dt1.Rows[i]["Priority"].ToString();
                            dr["Status"] = dt1.Rows[i]["Status"].ToString();
                            dr["Trans_Id"] = dt1.Rows[i]["Trans_Id"].ToString();
                            dr["Emp_Id"] = dt1.Rows[i]["Emp_Id"].ToString();
                            if (dt1.Rows[i]["Status_Update_Date"].ToString() != "")
                            {
                                dr["Date"] = Convert.ToDateTime(dt1.Rows[i]["Status_Update_Date"].ToString()).ToString("dd-MMM-yyyy HH:mm");
                            }
                            else
                            {
                                dr["Date"] = "-";
                            }
                            dtTemp.Rows.Add(dr);
                        }
                        if (dtTemp.Rows.Count > 0)
                        {
                            //Common Function add By Lokesh on 12-05-2015
                            objPageCmn.FillData((object)gvEmp, dtTemp, "", "");
                            foreach (GridViewRow gr in gvEmp.Rows)
                            {
                                HiddenField hdnEmpId = (HiddenField)gr.FindControl("hdnGvEmpId");
                                ImageButton imgBtnApprove = (ImageButton)gr.FindControl("imgBtnApprove");
                                ImageButton imgBtnReject = (ImageButton)gr.FindControl("imgBtnReject");
                                ImageButton imgBtnView = (ImageButton)gr.FindControl("imgBtnView");
                                Label lblPriority = (Label)gr.FindControl("lblPriority");
                                Label lblEmpStatus = (Label)gr.FindControl("lblStatus");
                                if (lblPriority.Text == "True")
                                {
                                    lblRequestStatus.Text = lblEmpStatus.Text;
                                }
                                if (flag2 == 1)
                                {
                                    imgBtnApprove.Visible = false;
                                    imgBtnReject.Visible = false;
                                }
                                else
                                {
                                    if (EmpId == hdnEmpId.Value)
                                    {
                                        imgBtnApprove.Visible = true;
                                        imgBtnReject.Visible = true;
                                        imgBtnView.Visible = true;
                                    }
                                    else
                                    {
                                        imgBtnApprove.Visible = false;
                                        imgBtnReject.Visible = false;
                                        if (flag == 1)
                                        {
                                            imgBtnView.Visible = true;
                                        }
                                        else
                                        {
                                            imgBtnView.Visible = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                gvApproval.DataSource = null;
                gvApproval.DataBind();
                Session["Approval"] = null;
            }
        }
        else
        {
            gvApproval.DataSource = null;
            gvApproval.DataBind();
            Session["Approval"] = null;
        }
    }
    protected string GetCustomerName(string strCustomerId)
    {
        string strCustomerName = string.Empty;
        if (strCustomerId != "0" && strCustomerId != "")
        {
            DataTable dtCName = objContact.GetContactTrueById(strCustomerId);
            if (dtCName.Rows.Count > 0)
            {
                strCustomerName = dtCName.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strCustomerName = "";
        }
        return strCustomerName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "0";
        }
        return ProductName;
    }
    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        else
        {
            ProductName = "0";
        }
        return ProductName;
    }
    public string SuggestedProductName(string ProductId, string TransId)
    {
        string ProductName = string.Empty;
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {
        }
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        else
        {
            if (editid.Value != "")
            {
                DataTable DtPurchaseDetail = new DataTable();
                DtPurchaseDetail = ObjPurrchaseRequestDetail.GetPurchaseRequestDetailbyTransIdandRequestId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, TransId);
                try
                {
                    ProductName = DtPurchaseDetail.Rows[0]["SuggestedProductName"].ToString();
                }
                catch
                {
                    ProductName = "0";
                }
            }
            else
            {
                ProductName = ProductId;
            }
        }
        return ProductName;
    }
    protected string GetSupplierName(string strSupplierId)
    {
        string strSupplierName = string.Empty;
        if (strSupplierId != "0" && strSupplierId != "")
        {
            DataTable dtSName = objContact.GetContactTrueById(strSupplierId);
            if (dtSName.Rows.Count > 0)
            {
                strSupplierName = dtSName.Rows[0]["Name"].ToString();
            }
        }
        else
        {
            strSupplierName = "";
        }
        return strSupplierName;
    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnit.GetUnitMasterById(Session["CompId"].ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Code"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    protected void txtResponsibePerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtResponsibePerson.Text != "")
        {
            try
            {
                strEmployeeId = txtResponsibePerson.Text.Trim().Split('/')[1].ToString();
                if (objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), strEmployeeId).Rows.Count == 0)
                {
                    DisplayMessage("Select Employee In Suggestions Only");
                    txtResponsibePerson.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtResponsibePerson);
                    return;
                }
            }
            catch
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtResponsibePerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtResponsibePerson);
                return;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        DataTable dt = new DataView(dt1, "Emp_Name like '%" + prefixText.ToString() + "%'", "Emp_Name Asc", DataViewRowState.CurrentRows).ToTable();
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
    public string checkPriority(ref SqlTransaction trns)
    {
        string strPriority = string.Empty;
        DataTable dt = ObjapprovalMaster.GetApprovalMasterById(ddlApprovalType.SelectedValue, ref trns);
        string strHierarchyRules = string.Empty;
        string strApprovalType = dt.Rows[0]["Approval_Type"].ToString();
        if (dt.Rows[0]["Is_Open"] != null)
        {
            strHierarchyRules = dt.Rows[0]["Is_Open"].ToString();
        }
        if (strApprovalType.Trim() == "Priority")
        {
            strPriority = " and Priority='True'";
            ///need to check employee have priority or not 
        }
        return strPriority;
    }
    #region ShiftApprovalRejection
    public string ReadHRMSConStringFromFile()
    {
        try
        {
            FileStream fs = new FileStream("C:\\PegasusSQL\\HRMSCONNECTIONSTRING.txt", FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string sqlStrSetting = sw.ReadLine();
            fs.Close();
            sw.Close();
            return sqlStrSetting;
        }
        catch
        {
            return "";
        }
    }
    protected void btnShiftApprove_Click(object sender, EventArgs e)
    {
        if (!IsApprovalPerson())
        {
            DisplayMessage("Approval setup issue , please contact to your admin");
            return;
        }
        string strTransId = GetSelectedList();
        if (strTransId == "")
        {
            DisplayMessage("Select at least one record");
            return;
        }
        ImageButton imgeditbutton = new ImageButton();
        imgeditbutton.ID = "imgBtnApprove";
        foreach (string str in strTransId.Split(','))
        {
            if (str == "")
            {
                continue;
            }
            Approve_Command(imgeditbutton, new CommandEventArgs("commandName", str));
        }
        FillGrid(hdnApprovalType.Value);
        FillshiftReferenceno();
        DisplayMessage("Record Approved Successfully");
    }
    public bool IsApprovalPerson()
    {
        bool Result = true;
        if (ConfigurationManager.AppSettings["ApprovalIntegration"] != null && ConfigurationManager.AppSettings["ApprovalIntegration"].ToString().Trim() == "1")
        {
            DataTable dtLeaveIntegration = objDA.GeTOracleRecord("SELECT Code FROM  apps.XXTSC_TMS_EMP_MF where shift_approver='YES' and code='" + Session["UserId"].ToString().Trim() + "'", ReadHRMSConStringFromFile());
            if (dtLeaveIntegration.Rows.Count == 0)
            {
                Result = false;
            }
        }
        return Result;
    }
    protected void btnShiftReject_Click(object sender, EventArgs e)
    {
        if (!IsApprovalPerson())
        {
            DisplayMessage("Approval setup issue, please contact to your admin");
            return;
        }
        string strTransId = GetSelectedList();
        if (strTransId == "")
        {
            DisplayMessage("Select at least one record");
            return;
        }
        ImageButton imgeditbutton = new ImageButton();
        imgeditbutton.ID = "lnkViewDetail";
        foreach (string str in strTransId.Split(','))
        {
            if (str == "")
            {
                continue;
            }
            Reject_Command(imgeditbutton, new CommandEventArgs("commandName", str));
        }
        FillshiftReferenceno();
        DisplayMessage("Record Rejected Successfully");
    }
    public string GetSelectedList()
    {
        DataTable dtTemp = new DataTable();
        DataTable dt = (DataTable)Session["dtRefApproval"];
        string strEmpId = string.Empty;
        string strTransId = string.Empty;
        int counter = 0;
        foreach (DataListItem dtlistrow in dtlistShift.Items)
        {
            GridView gvShiftView = ((GridView)dtlistrow.FindControl("gvShiftView"));
            foreach (GridViewRow gvr in gvShiftView.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkitemselect")).Checked)
                {
                    strEmpId = GetEmpId(gvShiftView.DataKeys[gvr.RowIndex][0].ToString());
                    //if (Session["EmpId"].ToString() != "892")
                    //    dtTemp = new DataView(dt, "Request_Emp_id=" + strEmpId + " and Emp_id=" + Session["Empid"].ToString() + " and ReferenceNo='" + ((Label)dtlistrow.FindControl("lblRefno")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //else
                    //    dtTemp = new DataView(dt, "Request_Emp_id=" + strEmpId + "  and ReferenceNo='" + ((Label)dtlistrow.FindControl("lblRefno")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    dtTemp = new DataView(dt, "Request_Emp_id=" + strEmpId + "  and ReferenceNo='" + ((Label)dtlistrow.FindControl("lblRefno")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();


                    strTransId += dtTemp.Rows[0]["Trans_Id"].ToString() + ",";
                }
                counter++;
            }
        }
        //foreach (GridViewRow gvrow in GvShiftApproval.Rows)
        //{
        //    if (((CheckBox)gvrow.FindControl("chk")).Checked)
        //    {
        //        GridView gvChild = (GridView)gvrow.FindControl("gvEmp");
        //        foreach (GridViewRow gvchildrow in gvChild.Rows)
        //        {
        //            if (((HiddenField)gvchildrow.FindControl("hdnGvEmpId")).Value == Session["EmpId"].ToString())
        //            {
        //                strTransId += ((HiddenField)gvchildrow.FindControl("hdnTransId")).Value + ",";
        //                break;
        //            }
        //        }
        //    }
        //}
        return strTransId;
    }
    public string GetEmpId(string empcode)
    {
        string empId = string.Empty;
        DataTable dt = objDA.return_DataTable("select emp_id from Set_EmployeeMaster where Company_Id=" + Session["CompId"].ToString() + " and  Emp_Code='" + empcode + "'");
        if (dt.Rows.Count > 0)
        {
            empId = dt.Rows[0]["Emp_Id"].ToString();
        }
        else
        {
            empId = "0";
        }
        dt.Dispose();
        return empId;
    }
    #endregion
    protected void chkHeader_CheckedChanged(object sender, EventArgs e)
    {
        bool isCheck = ((CheckBox)gvApproval.HeaderRow.FindControl("chkHeader")).Checked;
        foreach (GridViewRow gvrow in gvApproval.Rows)
        {
            ((CheckBox)gvrow.FindControl("chk")).Checked = isCheck;
        }
    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        FillGrid(hdnApprovalType.Value);
        if (ddlStatus.SelectedValue.Trim() == "Pending" && (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift")))
        {
            btnShiftApprove.Visible = true;
            btnShiftReject.Visible = true;
        }
        else
        {
            btnShiftApprove.Visible = false;
            btnShiftReject.Visible = false;
        }
    }
    public DataSet GetshiftDatatable(DataTable dtApproval, string strReferenceType = "")
    {
        string strTransid = string.Empty;
        DataSet dtdataset = new DataSet();
        string strdatecell = string.Empty;
        DateTime dtFromdate = new DateTime();
        DataTable dt = new DataTable();
        DataTable dtDetail = new DataTable();
        int counter = 0;
        dt.Columns.Add("Code", typeof(float));
        dt.Columns.Add("Name");
        dt.Columns.Add("Month");
        dt.Columns.Add("Year");
        if (strReferenceType.Length > 0 && strReferenceType != "--Select--")
        {
            foreach (DataRow dr in dtApproval.Rows)
            {
                dt.Rows.Add();
                dtDetail = ObjTempEmpShift.GetDetailRecordBy_Header_Id(dr["Ref_Id"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                dtDetail = new DataView(dtDetail, "", "schedule_date", DataViewRowState.CurrentRows).ToTable();
                dt.Rows[dt.Rows.Count - 1]["Code"] = dtDetail.Rows[0]["Emp_Code"].ToString();
                dt.Rows[dt.Rows.Count - 1]["Name"] = dtDetail.Rows[0]["Emp_Name"].ToString();
                dt.Rows[dt.Rows.Count - 1]["Month"] = getMonth(Convert.ToDateTime(dtDetail.Rows[0]["schedule_date"].ToString()).Month).ToString();
                dt.Rows[dt.Rows.Count - 1]["Year"] = Convert.ToDateTime(dtDetail.Rows[0]["schedule_date"].ToString()).Year.ToString();
                foreach (DataRow drchild in dtDetail.Rows)
                {
                    dtFromdate = Convert.ToDateTime(drchild["schedule_date"].ToString());
                    strdatecell = "" + dtFromdate.Day.ToString() + "(" + dtFromdate.DayOfWeek.ToString().Substring(0, 3) + ")" + "";
                    if (counter == 0)
                    {
                        dt.Columns.Add(strdatecell, typeof(string));
                    }
                    try
                    {
                        dt.Rows[dt.Rows.Count - 1][strdatecell] = drchild["Ref_Name"].ToString();
                    }
                    catch
                    {
                    }
                }
                strTransid += dr["Ref_Id"].ToString() + ",";
                counter++;
            }
        }

        dtdataset.Merge(dt);
        DataTable dtshiftsummary = new DataTable();
        try
        {
            if (strReferenceType.Length > 0 && strReferenceType != "--Select--")
            {

                DataTable dtShiftdetail = objDA.return_DataTable("select schedule_date,COUNT(ref_id) as count,MAX(Att_ShiftManagement.shift_name) as shift_name,MAX(Att_LeaveMaster.Leave_Name) as LeaveName,MAX(Set_HolidayMaster.Holiday_Name) as Holiday_Name,ref_type from Att_tmpEmpShiftSchedule inner join Att_tmpEmpShiftScheduleDetail on Att_tmpEmpShiftSchedule.trans_id=Att_tmpEmpShiftScheduleDetail.tmpEmpShiftSchedule_id left join Att_ShiftManagement on Att_tmpEmpShiftScheduleDetail.ref_id=Att_ShiftManagement.Shift_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Shift' left join Att_LeaveMaster on Att_tmpEmpShiftScheduleDetail.ref_id =Att_LeaveMaster.leave_id and Att_tmpEmpShiftScheduleDetail.ref_type='Leave' left join  set_holidaymaster on Att_tmpEmpShiftScheduleDetail.ref_id =Set_HolidayMaster.Holiday_Id and Att_tmpEmpShiftScheduleDetail.ref_type='Holiday'  where Att_tmpEmpShiftSchedule.trans_id in (" + strTransid.Substring(0, strTransid.Length - 1) + ") and  Att_tmpEmpShiftScheduleDetail.ref_id<>0 group by Att_tmpEmpShiftScheduleDetail.schedule_date,Att_tmpEmpShiftScheduleDetail.ref_id,Att_tmpEmpShiftScheduleDetail.ref_type,Att_ShiftManagement.shift_name");
                if (dtShiftdetail != null && dtShiftdetail.Rows.Count > 0)
                {
                    DataTable dtdistinctshiftName = new DataView(dtShiftdetail, "Ref_type='Shift'", "", DataViewRowState.CurrentRows).ToTable(true, "shift_name");
                    DataTable dtdistinctshiftdate = new DataView(dtShiftdetail, "", "", DataViewRowState.CurrentRows).ToTable(true, "schedule_date");
                    DataTable dtTempShiftsummary1 = new DataTable();
                    dtshiftsummary.Columns.Add("Shift");
                    dtshiftsummary.Columns.Add("Name");
                    dtshiftsummary.Columns.Add("Month");
                    dtshiftsummary.Columns.Add("Year");
                    foreach (DataRow dr in dtdistinctshiftdate.Rows)
                    {
                        dtshiftsummary.Columns.Add(Convert.ToDateTime(dr["schedule_date"].ToString()).ToString(objSys.SetDateFormat()));
                    }
                    foreach (DataRow dr in dtdistinctshiftName.Rows)
                    {
                        dtshiftsummary.Rows.Add();
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1]["Shift"] = dr["Shift_Name"].ToString();
                        for (int j = 4; j < dtshiftsummary.Columns.Count; j++)
                        {
                            dtTempShiftsummary1 = new DataView(dtShiftdetail, "Schedule_date='" + Convert.ToDateTime(dtshiftsummary.Columns[j].ColumnName.ToString()) + "' and Shift_Name='" + dr["Shift_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempShiftsummary1.Rows.Count > 0)
                            {
                                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = dtTempShiftsummary1.Rows[0]["Count"].ToString();
                            }
                            else
                            {
                                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = "0";
                            }
                        }
                    }
                    dtdistinctshiftName = new DataView(dtShiftdetail, "Ref_type='Leave'", "", DataViewRowState.CurrentRows).ToTable(true, "LeaveName");
                    foreach (DataRow dr in dtdistinctshiftName.Rows)
                    {
                        dtshiftsummary.Rows.Add();
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1]["Shift"] = dr["LeaveName"].ToString();
                        for (int j = 4; j < dtshiftsummary.Columns.Count; j++)
                        {
                            dtTempShiftsummary1 = new DataView(dtShiftdetail, "Schedule_date='" + Convert.ToDateTime(dtshiftsummary.Columns[j].ColumnName.ToString()) + "' and LeaveName='" + dr["LeaveName"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempShiftsummary1.Rows.Count > 0)
                            {
                                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = dtTempShiftsummary1.Rows[0]["Count"].ToString();
                            }
                            else
                            {
                                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = "0";
                            }
                        }
                    }
                    dtdistinctshiftName = new DataView(dtShiftdetail, "Ref_type='Holiday'", "", DataViewRowState.CurrentRows).ToTable(true, "Holiday_Name");
                    foreach (DataRow dr in dtdistinctshiftName.Rows)
                    {
                        dtshiftsummary.Rows.Add();
                        dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1]["Shift"] = dr["Holiday_Name"].ToString();
                        for (int j = 4; j < dtshiftsummary.Columns.Count; j++)
                        {
                            dtTempShiftsummary1 = new DataView(dtShiftdetail, "Schedule_date='" + Convert.ToDateTime(dtshiftsummary.Columns[j].ColumnName.ToString()) + "' and Holiday_Name='" + dr["Holiday_Name"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempShiftsummary1.Rows.Count > 0)
                            {
                                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = dtTempShiftsummary1.Rows[0]["Count"].ToString();
                            }
                            else
                            {
                                dtshiftsummary.Rows[dtshiftsummary.Rows.Count - 1][j] = "0";
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
        dtdataset.Merge(dtshiftsummary);
        return dtdataset;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefName(string prefixText, int count, string contextKey)
    {
        DataTable dt = (DataTable)HttpContext.Current.Session["dtRefApproval"];
        string[] str = new string[0];
        if (dt != null)
        {
            dt = new DataView(dt, "ReferenceNo like '%" + prefixText + "%'", "Trans_id", DataViewRowState.CurrentRows).ToTable(true, "ReferenceNo");
            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["ReferenceNo"].ToString();
                }
            }
        }
        else
        {
            str[0] = "";
        }
        return str;
    }
    protected void btnRefRefresh_Click(object sender, EventArgs e)
    {
        ddlReferenceNo.SelectedIndex = 0;
        ddlReferenceNo.DataSource = null;
        ddlReferenceNo.DataBind();
        ddlStatus_OnSelectedIndexChanged(null, null);
        ddlReferenceNo.SelectedIndex = 0;
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        foreach (DataListItem dtlistrow in dtlistShift.Items)
        {
            GridView gvShiftView = ((GridView)dtlistrow.FindControl("gvShiftView"));
            gvShiftView.Columns[0].Visible = false;
        }
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=ShiftSummary.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            pnlExport.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        foreach (DataListItem dtlistrow in dtlistShift.Items)
        {
            GridView gvShiftView = ((GridView)dtlistrow.FindControl("gvShiftView"));
            gvShiftView.Columns[0].Visible = true;
        }
    }
    public string getMonth(int MonthNumber)
    {
        string strMonthName = string.Empty;
        System.Globalization.DateTimeFormatInfo mfi = new
System.Globalization.DateTimeFormatInfo();
        strMonthName = mfi.GetMonthName(MonthNumber).ToString();
        return strMonthName;
    }
    protected void ddlReferenceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strText = ddlReferenceNo.SelectedItem.ToString();
        FillGrid(hdnApprovalType.Value, strText);
        if (ddlStatus.SelectedValue.Trim() == "Pending" && (ddlApprovalType.SelectedItem.Text.ToString().ToLower().Contains("shift")))
        {
            btnShiftApprove.Visible = true;
            btnShiftReject.Visible = true;
        }
        else
        {
            btnShiftApprove.Visible = false;
            btnShiftReject.Visible = false;
        }
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        try
        {
            string[] pValues = editid.Value.Split(new Char[] { '#' });
            if (pValues.Length == 3)
            {
                string strSQL = "Select Ref_Id,COUNT(*) From Set_Approval_Transaction  Where Ref_Id IN( Select Ref_Id From   Set_Approval_Transaction Where Approval_Id = " + pValues[0] + " and  Request_Emp_Id = " + pValues[1] + " and  Emp_Id = " + pValues[2] + " and Status ='Pending') Group BY  Ref_Id Having  COUNT(*) > 1";
                using (DataTable _dt = objDA.return_DataTable(strSQL))
                {
                    if (_dt.Rows.Count > 0)
                    {
                        for (int rCount = 0; rCount < _dt.Rows.Count; rCount++)
                        {
                            strSQL = "Delete  From   Set_Approval_Transaction  Where Approval_Id = " + pValues[0] + " and  Request_Emp_Id = " + pValues[1] + " and  Emp_Id = " + pValues[2] + " and Ref_Id ='" + _dt.Rows[rCount][0].ToString() + "' ";
                            objDA.execute_Command(strSQL);

                        }

                        try
                        {
                            //string sql = "";
                            //string sql = "select em.emp_name as approval_person, am.Approval_Name, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id where am.Approval_Id='" + e.CommandArgument.ToString() + "' and at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name";
                            //string sql = "select em.emp_name as approval_person, am.Approval_Name, rem.Emp_Name as request_person, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_EmployeeMaster rem on rem.Emp_Id=at.Request_Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id where at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name,at.Request_Emp_Id,rem.Emp_Name order by em.Emp_Name,am.Approval_Name,rem.Emp_Name";
                            string sql = "select em.emp_name as Approval_Person, am.Approval_Name, rem.Emp_Name as Request_Person, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings , Set_LocationMaster.Location_Name AS Request_Location, (cast(at.Approval_Id as varchar)+'#'+ cast(at.Request_Emp_Id  as varchar)+'#'+ cast(at.Emp_Id  as varchar)) as PairedValues from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_EmployeeMaster rem on rem.Emp_Id=at.Request_Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id    INNER JOIN Set_LocationMaster ON rem.Company_Id = Set_LocationMaster.Company_Id AND rem.Brand_Id = Set_LocationMaster.Brand_Id AND rem.Location_Id = Set_LocationMaster.Location_Id where at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name,at.Request_Emp_Id,rem.Emp_Name, Set_LocationMaster.Location_Name order by em.Emp_Name,am.Approval_Name,rem.Emp_Name";
                            using (DataTable _dtRecord = objDA.return_DataTable(sql))
                            {
                                if (_dtRecord.Rows.Count > 0)
                                {
                                    gvPendingApprovals.DataSource = _dtRecord;
                                    gvPendingApprovals.DataBind();

                                    lblApprovalType.Text = "Pending Approval Summary";

                                }
                                else
                                {
                                    DisplayMessage("There is no pending records");
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            DisplayMessage(ex.Message);
                        }
                    }
                }

            }
            else
            {
                DisplayMessage("String Not Valid");
            }
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message);
        }



    }

    protected void btnViewAllPendings_Click(object sender, EventArgs e)
    {
        try
        {
            //string sql = "";
            //string sql = "select em.emp_name as approval_person, am.Approval_Name, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id where am.Approval_Id='" + e.CommandArgument.ToString() + "' and at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name";
            //string sql = "select em.emp_name as approval_person, am.Approval_Name, rem.Emp_Name as request_person, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_EmployeeMaster rem on rem.Emp_Id=at.Request_Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id where at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name,at.Request_Emp_Id,rem.Emp_Name order by em.Emp_Name,am.Approval_Name,rem.Emp_Name";
            string sql = "select em.emp_name as Approval_Person, am.Approval_Name, rem.Emp_Name as Request_Person, at.Approval_Id,at.Emp_Id,COUNT(*) as TotalPendings , Set_LocationMaster.Location_Name AS Request_Location, (cast(at.Approval_Id as varchar)+'#'+ cast(at.Request_Emp_Id  as varchar)+'#'+ cast(at.Emp_Id  as varchar)) as PairedValues from Set_Approval_Transaction at inner join Set_EmployeeMaster em on em.Emp_Id=at.Emp_Id inner join Set_EmployeeMaster rem on rem.Emp_Id=at.Request_Emp_Id inner join Set_ApprovalMaster am on am.Approval_Id=at.Approval_Id    INNER JOIN Set_LocationMaster ON rem.Company_Id = Set_LocationMaster.Company_Id AND rem.Brand_Id = Set_LocationMaster.Brand_Id AND rem.Location_Id = Set_LocationMaster.Location_Id where at.isactive='true' and at.Status='pending' and at.Is_Default='true' and (case when isnull(am.field1,'0')='3' then at.Company_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["CompId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='2' then at.Brand_Id else '1' end) = (case when isnull(am.field1,'0')='2' then '" + Session["BrandId"].ToString() + "' else '1' end) and (case when isnull(am.field1,'0')='3' then at.Location_Id else '1' end) = (case when isnull(am.field1,'0')='3' then '" + Session["LocId"].ToString() + "' else '1' end) group by at.Approval_Id,at.emp_id,am.Approval_Name,em.emp_name,at.Request_Emp_Id,rem.Emp_Name, Set_LocationMaster.Location_Name order by em.Emp_Name,am.Approval_Name,rem.Emp_Name";
            using (DataTable _dt = objDA.return_DataTable(sql))
            {
                if (_dt.Rows.Count > 0)
                {
                    gvPendingApprovals.DataSource = _dt;
                    gvPendingApprovals.DataBind();
                    //objPageCmn.FillData((object)gvPendingApprovals, _dt, "", "");
                    lblApprovalType.Text = "Pending Approval Summary";
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Pendings()", true);
                }
                else
                {
                    DisplayMessage("There is no pending records");
                }
            }

        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message);
        }
    }
}