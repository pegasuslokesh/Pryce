using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using PegasusDataAccess;
public partial class Purchase_PurchaseRequest : BasePage
{
    #region Class Object
    DataAccessClass objDa = null;
    Set_Approval_Employee objEmpApproval = null;
    PurchaseRequestDetail ObjPurchaseRequestDetail = null;
    PurchaseRequestHeader ObjPurchaseReqestHeader = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_StockDetail objStockDetail = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Common cmn = null;
    Inv_PurchaseInquiryHeader objPIHeader = null;
    Inv_ParameterMaster objInvParam = null;
    DepartmentMaster ObjDept = null;
    EmployeeMaster objEmployee = null;
    NotificationMaster Obj_Notifiacation = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string UserId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjPurchaseRequestDetail = new PurchaseRequestDetail(Session["DBConnection"].ToString());
        ObjPurchaseReqestHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        StrCompId = Session["CompId"].ToString();
        UserId = Session["UserId"].ToString();
        //btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        if (!IsPostBack)
        {
           
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Purchase/PurchaseQuatation.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            hdnLocationID.Value = Session["LocId"].ToString();
            txtlRequestNo.Text = GetDocumentNum();
            ViewState["DocNo"] = txtlRequestNo.Text;
            txtCalenderExtender.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtRequestdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtExpDelDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillGrid();
            txtlRequestNo.Focus();
            txtValue.Focus();
            Session["DtSearchProduct"] = null;
            Session["DtRequestProduct"] = null;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            ddlFieldName.Focus();
        }
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region System Function :-

    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtRequestdate.Focus();
        if (Lbl_Tab_New.Text == "View")
        {
            btnSave.Enabled = false;
            btnReset.Visible = false;
            btnSavePrint.Visible = false;
        }
        else
        {
            btnSave.Enabled = true;
            btnReset.Visible = true;
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtbinValue.Focus();
        txtbinValue.Text = "";
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        //here we check that this page is updated by other user before save of current user 
        //code start
        if (editid.Value != "")
        {
            DataTable dt = ObjPurchaseReqestHeader.GetPurchaseRequestTrueAllByReqId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value);
            if (dt.Rows.Count != 0)
            {
                if (ViewState["TimeStamp"].ToString() != dt.Rows[0]["Field7"].ToString())
                {
                    DisplayMessage("Another User update Information reload and try again");
                    return;
                }
            }
        }
        //code end
        if (txtRequestdate.Text == "")
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Request Date");
            txtRequestdate.Focus();
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtRequestdate.Text);
            }
            catch
            {
                DisplayMessage("Enter Request Date in format " + Session["DateFormat"].ToString() + "");
                txtRequestdate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequestdate);
                return;
            }
        }
        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtRequestdate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }
        if (txtlRequestNo.Text == "")
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Request No.");
            txtlRequestNo.Focus();
            return;
        }
        if (txtExpDelDate.Text == "")
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Expected Delievery Date");
            txtExpDelDate.Focus();
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtExpDelDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Delievery Date in format " + Session["DateFormat"].ToString() + "");
                txtExpDelDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpDelDate);
                return;
            }
        }
        if (gvProductRequest.Rows.Count == 0)
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Product Details");
            btnAddProduct.Focus();
            return;
        }
        if (txtTermCondition.Text == "")
        {
            ViewState["Return"] = 1;
            DisplayMessage("Enter Description");
            txtTermCondition.Focus();
            return;
        }
        string IsApprovalParameter = "Approved";
        bool IsDept = true;
        //Add Approval New Concept On 08-12-2014
        DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseRequestApproval");
        DataTable dt1 = new DataTable();
        string EmpPermission = string.Empty;
        if (Dt.Rows.Count > 0)
        {
            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
            {
                IsApprovalParameter = "Pending";
                IsDept = false;
                EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("PurchaseRequest").Rows[0]["Approval_Level"].ToString();
                dt1 = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "44", Session["EmpId"].ToString());
                if (dt1.Rows.Count == 0)
                {
                    DisplayMessage("Approval setup issue , please contact to your admin");
                    return;
                }
            }
        }
        //End Approval Concept
        int b = 0;
        string sql = string.Empty;
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (editid.Value == "")
            {
                //SqlConnection conn = new SqlConnection(Session["DBConnection"].ToString());
                //conn.Open();
                //SqlTransaction sqltrans = conn.BeginTransaction();
                b = ObjPurchaseReqestHeader.InsertPurchaseRequestHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtlRequestNo.Text, ObjSysParam.getDateForInput(txtRequestdate.Text).ToString(), txtTermCondition.Text, IsApprovalParameter.ToString(), ObjSysParam.getDateForInput(txtExpDelDate.Text).ToString(), IsDept.ToString(), false.ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                //sql = "INSERT INTO Inv_PurchaseRequestHeader([Company_Id],[Brand_ID],[Location_Id],[RequestNo] ,[RequestDate] ,[TermCondition],[Status] ,[ExpDelDate] ,[DepartmentApproval] ,[ProcurementApproval],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])VALUES ('" + StrCompId + "','" + StrBrandId + "','" + StrLocationId + "','" + txtlRequestNo.Text + "','" + ObjSysParam.getDateForInput(txtRequestdate.Text).ToString() + "','" + txtTermCondition.Text + "','" + IsApprovalParameter.ToString() + "','" + ObjSysParam.getDateForInput(txtExpDelDate.Text).ToString() + "','" + IsDept.ToString() + "','" + false.ToString() + "','" + Session["DepartmentId"].ToString() + "','" + Session["EmpId"].ToString() + "',' ',' ',' ','" + true.ToString() + "','" + DateTime.Now.ToString() + "','" + true.ToString() + "','" + UserId.ToString() + "','" + DateTime.Now.ToString() + "','" + UserId.ToString() + "','" + DateTime.Now.ToString() + "')";
                //SqlCommand cmd = new SqlCommand(sql, conn,sqltrans);
                //cmd.ExecuteNonQuery();
                //b = 1;
                editid.Value = b.ToString();
                if (txtlRequestNo.Text == ViewState["DocNo"].ToString())
                {

                    int Requestcount = ObjPurchaseReqestHeader.GetRequestCountByLocationId1(Session["LocId"].ToString(),ref trns);
                    if (Requestcount == 0)
                    {
                        ObjPurchaseReqestHeader.Updatecode(b.ToString(), txtlRequestNo.Text + "1", ref trns);

                    }
                    else
                    {
                        if (objDa.return_DataTable("select RequestNo from inv_purchaserequestheader where location_id=" + Session["LocId"].ToString() + " and RequestNo='" + txtlRequestNo.Text.Trim() + Requestcount.ToString() + "'",ref trns).Rows.Count > 0)
                        {
                            bool bCodeFlag = true;
                            while (bCodeFlag)
                            {
                                Requestcount += 1;
                                if (objDa.return_DataTable("select RequestNo from inv_purchaserequestheader where location_id=" + Session["LocId"].ToString() + " and RequestNo='" + txtlRequestNo.Text.Trim() + Requestcount.ToString() + "'",ref trns).Rows.Count == 0)
                                {
                                    bCodeFlag = false;
                                }
                            }
                        }
                        ObjPurchaseReqestHeader.Updatecode(b.ToString(), txtlRequestNo.Text + Requestcount.ToString(), ref trns);

                    }
                }
                foreach (GridViewRow gvr in gvProductRequest.Rows)
                {
                    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
                    Label lblProductId = (Label)gvr.FindControl("lblPID");
                    Label lblUnitId = (Label)gvr.FindControl("lblUID");
                    Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
                    Label lblProductDescription = (Label)gvr.FindControl("lblDescription");
                    HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                    DataTable dtProduct = new DataTable();
                    try
                    {
                        dtProduct = ObjProductMaster.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), lblProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }
                    catch
                    {
                        lblProductId.Text = "0";
                    }
                    ObjPurchaseRequestDetail.InsertPurchaseRequestDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value, lblSerialNo.Text, lblProductId.Text, hdnSuggestedProductName.Value, lblProductDescription.Text, lblUnitId.Text, lblReqQty.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                    //sql = "INSERT INTO Inv_PurchaseRequestDetail           ([Company_Id]           ,[Brand_ID]           ,[Location_ID]           ,[RequestNo]           ,[Serial_No]           ,[Product_Id]           ,[SuggestedProductName]           ,[ProductDescription]           ,[UnitId]           ,[ReqQty]           ,[Field1]           ,[Field2]           ,[Field3]           ,[Field4]           ,[Field5]           ,[Field6]           ,[Field7]           ,[CreatedBy]           ,[CreatedDate]           ,[ModifiedBy]           ,[ModifiedDate]           ,[IsActive])   values('" + StrCompId + "','" + StrBrandId.ToString() + "','" + StrLocationId.ToString() + "'," + editid.Value + ",'" + lblSerialNo.Text + "','" + lblProductId.Text + "','" + hdnSuggestedProductName.Value + "','" + lblProductDescription.Text + "'," + lblUnitId.Text + "," + lblReqQty.Text + ",' ',' ',' ',' ',' ','" + true.ToString() + "','" + DateTime.Now.ToString() + "','" + UserId.ToString() + "','" + DateTime.Now.ToString() + "','" + UserId.ToString() + "','" + DateTime.Now.ToString() + "','" + true.ToString() + "')";
                    //cmd = new SqlCommand(sql, conn, sqltrans);
                    //cmd.ExecuteNonQuery();
                }
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                    {
                        if (dt1.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                string PriorityEmpId = dt1.Rows[j]["Emp_Id"].ToString();
                                string IsPriority = dt1.Rows[j]["Priority"].ToString();
                                int cur_trans_id = 0;
                                if (EmpPermission == "1")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (EmpPermission == "2")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else if (EmpPermission == "3")
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                else
                                {
                                    cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), ref trns);
                                }
                                // Insert Notification For Leave by  ghanshyam suthar
                                Session["PriorityEmpId"] = PriorityEmpId;
                                Session["cur_trans_id"] = cur_trans_id;
                                Session["Ref_ID"] = b.ToString();
                                Set_Notification();
                            }
                        }
                    }
                }
                if (b != 0)
                {
                    DisplayMessage("Record Saved", "green");
                }
                //sqltrans.Commit();
            }
            else
            {
                //use this datatable for get the status of this record
                DataTable DtPRHeader = ObjPurchaseReqestHeader.GetPurchaseRequestTrueAllByReqId(StrCompId, StrBrandId, StrLocationId, editid.Value, ref trns);
                DataTable DtApprove = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseRequestApproval", ref trns);
                if (DtApprove.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(DtApprove.Rows[0]["ParameterValue"]) == true)
                    {
                        if (DtPRHeader.Rows[0]["Status"].ToString().Trim() == "Rejected")
                        {
                            b = ObjPurchaseReqestHeader.UpdatePurchaseRequestHeader(editid.Value, StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtlRequestNo.Text, txtRequestdate.Text, txtTermCondition.Text, "Pending", ObjSysParam.getDateForInput(txtExpDelDate.Text).ToString(), false.ToString(), false.ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), ViewState["RequestStatus"].ToString(), "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            DataTable dtEmp = objEmpApproval.GetApprovalTransation(StrCompId, ref trns);
                            dtEmp = new DataView(dtEmp, "Approval_Id='6' and Ref_Id='" + editid.Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtEmp.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtEmp.Rows.Count; i++)
                                {
                                    objEmpApproval.UpdateApprovalTransaciton("PurchaseRequest", editid.Value.ToString(), "44", dtEmp.Rows[i]["Emp_Id"].ToString(), "Pending", dtEmp.Rows[i]["Description"].ToString(), dtEmp.Rows[i]["Approval_Id"].ToString(), Session["EmpId"].ToString(), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString());
                                }
                            }
                        }
                        else
                        {
                            b = ObjPurchaseReqestHeader.UpdatePurchaseRequestHeader(editid.Value, StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtlRequestNo.Text, txtRequestdate.Text, txtTermCondition.Text, DtPRHeader.Rows[0]["Status"].ToString(), ObjSysParam.getDateForInput(txtExpDelDate.Text).ToString(), Convert.ToBoolean(ViewState["DepartmentApproval"].ToString()).ToString(), false.ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), ViewState["RequestStatus"].ToString(), "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    else
                    {
                        b = ObjPurchaseReqestHeader.UpdatePurchaseRequestHeader(editid.Value, StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), txtlRequestNo.Text, txtRequestdate.Text, txtTermCondition.Text, DtPRHeader.Rows[0]["Status"].ToString(), ObjSysParam.getDateForInput(txtExpDelDate.Text).ToString(), Convert.ToBoolean(ViewState["DepartmentApproval"].ToString()).ToString(), false.ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), ViewState["RequestStatus"].ToString(), "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                ObjPurchaseRequestDetail.DeletePurchaseRequestDetailBYReqID(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value, ref trns);
                foreach (GridViewRow gvr in gvProductRequest.Rows)
                {
                    HiddenField hdnTransId = (HiddenField)gvr.FindControl("HdnTransId");
                    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
                    Label lblProductId = (Label)gvr.FindControl("lblPID");
                    Label lblUnitId = (Label)gvr.FindControl("lblUID");
                    Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
                    Label lblProductDescription = (Label)gvr.FindControl("lblDescription");
                    HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                    DataTable dtProduct = new DataTable();
                    try
                    {
                        dtProduct = ObjProductMaster.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), lblProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }
                    catch
                    {
                        lblProductId.Text = "0";
                    }
                    ObjPurchaseRequestDetail.InsertPurchaseRequestDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), editid.Value, lblSerialNo.Text, lblProductId.Text, hdnSuggestedProductName.Value, lblProductDescription.Text, lblUnitId.Text, lblReqQty.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                }
                // Insert Notification For Leave by  ghanshyam suthar
                //Session["PriorityEmpId"] = "";
                //Session["cur_trans_id"] = "";
                // Set_Notification();
                if (b != 0)
                {
                    DisplayMessage("Record Update");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            ViewState["RequestNo"] = txtlRequestNo.Text;
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
        FillGrid();
        Reset();
        txtRequestdate.Focus();
        Update_List.Update();
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnRequestListIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/purchase"));
        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);
        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for Purchase Request. on " + System.DateTime.Now.ToString();
        if (Hdn_Edit_ID.Value == "")
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");
        else
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Hdn_Edit_ID.Value, "15");
    }


    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                ViewState["Emp_Img"] = "";
            }
        }
        else
        {
            strEmployeeName = "";
            ViewState["Emp_Img"] = "";
        }
        return strEmployeeName;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtlRequestNo.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnNew_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        Hdn_Edit_ID.Value = e.CommandArgument.ToString();

        hdnLocationID.Value = e.CommandName.ToString();

        if (hdnLocationID.Value != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }


        using (DataTable dt = ObjPurchaseReqestHeader.GetPurchaseRequestTrueAllByReqId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString()))
        {
            if (dt.Rows.Count != 0)
            {
                if (objSenderID != "lnkViewDetail")
                {
                    using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseOrderApproval"))
                    {
                        if (Dt.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                            {
                                if (dt.Rows[0]["Status"].ToString().Trim() == "Approved")
                                {
                                    DisplayMessage("This Request is Approved From The Department");
                                    return;
                                }
                            }
                        }
                    }
                    using (DataTable DtInquiry = objPIHeader.GetPIHeaderAllDataByTransFromAndNo(StrCompId, StrBrandId, StrLocationId, "PR", e.CommandArgument.ToString()))
                    {
                        if (DtInquiry.Rows.Count > 0)
                        {
                            DisplayMessage("This Request Number is Used in Purchase Inquiry");
                            return;
                        }
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
                editid.Value = e.CommandArgument.ToString();
                txtlRequestNo.Text = dt.Rows[0]["RequestNo"].ToString();
                ViewState["TimeStamp"] = dt.Rows[0]["Field7"].ToString();
                txtRequestdate.Text = Convert.ToDateTime(dt.Rows[0]["RequestDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtExpDelDate.Text = Convert.ToDateTime(dt.Rows[0]["ExpDelDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                ViewState["DepartmentApproval"] = dt.Rows[0]["DepartmentApproval"].ToString();
                txtTermCondition.Text = dt.Rows[0]["TermCondition"].ToString();
                ViewState["RequestStatus"] = dt.Rows[0]["Field3"].ToString();
                fillgridDetail();
                btnNew_Click(null, null);
            }
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Focus();
        FillGrid();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        using (DataTable DtApprove = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseRequestApproval"))
        {
            if (DtApprove.Rows.Count > 0)
            {
                if (Convert.ToBoolean(DtApprove.Rows[0]["ParameterValue"]) == true)
                {
                    if (((Label)gvrow.FindControl("lblStatus")).Text.Trim() == "Approved")
                    {
                        DisplayMessage("This Request is Approved From The Department");
                        return;
                    }
                }
            }
        }

        using (DataTable DtInquiry = objPIHeader.GetPIHeaderAllDataByTransFromAndNo(StrCompId, StrBrandId, StrLocationId, "PR", e.CommandArgument.ToString()))
        {
            if (DtInquiry.Rows.Count > 0)
            {
                DisplayMessage("This Request Number is Used in Purchase Inquiry");
                return;
            }
        }
        ObjPurchaseReqestHeader.DeletePurchaseRequestHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString(), false.ToString(), UserId.ToString(), DateTime.Now.ToString());
        int b = objEmpApproval.Delete_Approval_Transaction("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", e.CommandArgument.ToString());
        FillGrid();
        DisplayMessage("Record Deleted");
        try
        {
            int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            ((LinkButton)gvPurchaseRequest.Rows[i].FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "RequestDate" || ddlFieldName.SelectedItem.Value == "ExpDelDate")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = (txtValueDate.Text).ToString();
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
        FillGrid();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        DataTable DtProduct = new DataTable();
        DtProduct.Columns.Add("Trans_Id", typeof(int));
        DtProduct.Columns.Add("Serial_No", typeof(int));
        DtProduct.Columns.Add("Product_Id");
        DtProduct.Columns.Add("UnitId");
        DtProduct.Columns.Add("ReqQty");
        DtProduct.Columns.Add("ProductDescription");
        DtProduct.Columns.Add("SuggestedProductName");
        string PDiscription = string.Empty;
        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Text = "";
            txtProductName.Focus();
            return;
        }
        if (ddlUnit == null)
        {
            DisplayMessage("Unit Not Found");
            ddlUnit.Focus();
            return;
        }
        else
        {
            if (ddlUnit.Items.Count == 0)
            {
                DisplayMessage("Unit Not Found");
                ddlUnit.Focus();
                return;
            }
        }
        //if (ddlUnit.SelectedIndex == 0)
        //{
        //    DisplayMessage("Select Unit");
        //    ddlUnit.SelectedIndex = 0;
        //    ddlUnit.Focus();
        //    return;
        //}
        if (txtRequestQty.Text == "")
        {
            txtRequestQty.Text = "1";
        }
        string ReqId = string.Empty;
        string ProductId = string.Empty;
        string UnitId = string.Empty;
        string SuggestedProductName = string.Empty;
        string ProductName = string.Empty;
        if (txtProductName.Text != "")
        {
            ProductId = ObjProductMaster.GetProductIdbyProductName(txtProductName.Text.ToString().Trim(), HttpContext.Current.Session["BrandId"].ToString());

            if (ProductId != "")
            {
                PDiscription = txtPDescription.Text;
                SuggestedProductName = "";
            }
            else
            {
                PDiscription = txtPDesc.Text;
                ProductId = "0";
                SuggestedProductName = txtProductName.Text;
            }
        }
        UnitId = ddlUnit.SelectedValue.ToString();
        if (hidProduct.Value == "" || hidProduct.Value == "0")
        {
            if (Session["DtRequestProduct"] != null)
            {
                DtProduct = (DataTable)Session["DtRequestProduct"];
            }
            if (Session["DtRequestProduct"] == null)
            {
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                dr["Serial_No"] = "1";
                dr["Product_Id"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["ReqQty"] = txtRequestQty.Text.ToString();
                dr["ProductDescription"] = PDiscription;
                dr["SuggestedProductName"] = SuggestedProductName;
                DtProduct.Rows.Add(dr);
                Session["DtRequestProduct"] = (DataTable)DtProduct;
            }
            else
            {
                DtProduct = (DataTable)Session["DtRequestProduct"];
                DataRow dr = DtProduct.NewRow();
                dr["Trans_Id"] = DtProduct.Rows.Count + 1;
                DataTable dtserialNo = new DataTable();
                try
                {
                    dtserialNo = new DataView(DtProduct, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
                    dr["Serial_No"] = (float.Parse(dtserialNo.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                }
                catch
                {
                    dr["Serial_No"] = "1";
                }
                dr["Product_Id"] = ProductId.ToString();
                dr["UnitId"] = UnitId.ToString();
                dr["ReqQty"] = txtRequestQty.Text.ToString();
                dr["ProductDescription"] = PDiscription;
                dr["SuggestedProductName"] = SuggestedProductName;
                DtProduct.Rows.Add(dr);
                Session["DtRequestProduct"] = (DataTable)DtProduct;
            }
        }
        else
        {
            DataTable dt = (DataTable)Session["DtRequestProduct"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = DtProduct.NewRow();
                if (dt.Rows[i]["Trans_Id"].ToString() == hidProduct.Value)
                {
                    dr["Trans_Id"] = hidProduct.Value;
                    dr["Serial_No"] = dt.Rows[i]["Serial_No"].ToString();
                    dr["Product_Id"] = ProductId.ToString();
                    dr["UnitId"] = UnitId.ToString();
                    dr["ReqQty"] = txtRequestQty.Text.ToString();
                    dr["ProductDescription"] = PDiscription;
                    dr["SuggestedProductName"] = SuggestedProductName;
                    DtProduct.Rows.Add(dr);
                }
                else
                {
                    dr["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                    dr["Serial_No"] = dt.Rows[i]["Serial_No"].ToString();
                    dr["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                    dr["UnitId"] = dt.Rows[i]["UnitId"].ToString();
                    dr["ReqQty"] = dt.Rows[i]["ReqQty"].ToString();
                    dr["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                    dr["SuggestedProductName"] = dt.Rows[i]["SuggestedProductName"].ToString();
                    DtProduct.Rows.Add(dr);
                }
            }
        }
        //fillgridDetail();  //comment by jitendra
        Session["DtRequestProduct"] = (DataTable)DtProduct;
        //bind gridview by function in common class
        objPageCmn.FillData((object)gvProductRequest, DtProduct, "", "");
        ResetDetail();
        txtProductcode.Focus();
    }
    protected void btnEdit_Command1(object sender, CommandEventArgs e)
    {
        hidProduct.Value = e.CommandArgument.ToString();
        //updated by jitendra upadhyay on 26-oct-2013 after create the dynamic table for product gridview
        DataTable dtproduct = new DataTable();
        dtproduct = (DataTable)Session["DtRequestProduct"];
        DataTable dt = new DataView(dtproduct, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Product_Id"].ToString() != "0")
            {
                txtProductName.Text = ObjProductMaster.GetProductNamebyProductId(dt.Rows[0]["Product_Id"].ToString());
                //when we edit the product then we showing the product code also with product name  so create new function as Product code(string productId)
                //this code is created by jitendra upadhyay on 24-03-2014
                txtProductcode.Text = ObjProductMaster.GetProductCodebyProductId(dt.Rows[0]["Product_Id"].ToString());
                pnlPDescription.Visible = true;
                txtPDesc.Visible = false;
                txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
                FillUnit(dt.Rows[0]["Product_Id"].ToString());
            }
            else
            {
                txtProductName.Text = dt.Rows[0]["SuggestedProductName"].ToString();
                txtPDesc.Visible = true;
                txtPDesc.Text = dt.Rows[0]["ProductDescription"].ToString();
                pnlPDescription.Visible = false;
                FillUnit("0");
            }
            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            txtRequestQty.Text = dt.Rows[0]["ReqQty"].ToString();
            ViewState["SNO"] = dt.Rows[0]["Serial_No"].ToString();
            txtProductcode.Focus();
        }
    }
    protected void IbtnDelete_Command1(object sender, CommandEventArgs e)
    {
        DataTable dtproduct = (DataTable)Session["DtRequestProduct"];
        DataTable dt = new DataView(dtproduct, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Serial_No"] = i + 1;
        }
        //bind gridview by function in common class
        objPageCmn.FillData((object)gvProductRequest, dt, "", "");
        Session["DtRequestProduct"] = (DataTable)dt;
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetDetail();
        txtTermCondition.Focus();
    }
    protected void btnProductClose_Click(object sender, EventArgs e)
    {
        txtTermCondition.Focus();
    }
    protected void txtlRequestNo_TextChanged(object sender, EventArgs e)
    {
        if (txtlRequestNo.Text != "")
        {
            DataTable dt = new DataView(ObjPurchaseReqestHeader.GetPurchaseRequestHeaderTrueAll(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString()), "RequestNo='" + txtlRequestNo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    DisplayMessage("Request No Already Exists");
                }
                else
                {
                    DisplayMessage("Request No Already Exists :- Go To Bin Tab");
                }
                txtlRequestNo.Text = "";
                txtlRequestNo.Focus();
            }
            else
            {
                btnAddProduct.Focus();
            }
        }
    }
    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        txtProductcode.Focus();
        ResetDetail();
        Session["DtSearchProduct"] = null;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductName.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;
            }
            else
            {
                FillUnit("0");
                txtPDescription.Text = "";
                txtPDesc.Visible = true;
                pnlPDescription.Visible = false;
            }
            ddlUnit.Focus();
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            using (DataTable dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductcode.Text.ToString()))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    FillUnit(dt.Rows[0]["ProductId"].ToString());
                    txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                    txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                    txtPDescription.Text = dt.Rows[0]["Description"].ToString();
                    txtPDesc.Visible = false;
                    pnlPDescription.Visible = true;
                }
                else
                {
                    FillUnit("0");
                    txtPDescription.Text = "";
                    txtPDesc.Visible = true;
                    pnlPDescription.Visible = false;
                }
            }
            ddlUnit.Focus();
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
    }
    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(StrCompId, StrBrandId, StrLocationId, pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void gvPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (gvPurchaseRequest.Attributes["CurrentSortField"] != null &&
            gvPurchaseRequest.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == gvPurchaseRequest.Attributes["CurrentSortField"])
            {
                if (gvPurchaseRequest.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        gvPurchaseRequest.Attributes["CurrentSortField"] = sortField;
        gvPurchaseRequest.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnRequestListIndex.Value));
    }
    #endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnProductSave.Visible = clsPagePermission.bAdd;
        btnSavePrint.Visible = clsPagePermission.bAdd;
        try
        {
            hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
            hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
            hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
            hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        }
        catch
        {
        }
        imgBtnRestore.Visible = clsPagePermission.bRestore;

    }
    #endregion
    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["EProductName"].ToString();
                }
            }
            dt = null;
            return str;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
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
    #endregion
    #region User Defined Function
    public void ResetDetail()
    {
        txtProductName.Text = "";
        txtProductcode.Text = "";
        txtPDescription.Text = "";
        ddlUnit.Items.Clear();
        txtRequestQty.Text = "1";
        hidProduct.Value = "";
        txtPDesc.Text = "";
        txtProductcode.Focus();
    }
    public void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }
    public void fillgridDetail()
    {
        string ReqId = ObjPurchaseReqestHeader.getAutoId();
        if (editid.Value == "")
        {
            ReqId = ObjPurchaseReqestHeader.getAutoId();
        }
        else
        {
            ReqId = editid.Value.ToString();
        }
        DataTable dt = ObjPurchaseRequestDetail.GetPurchaseRequestDetailbyRequestId(StrCompId, StrBrandId, StrLocationId, ReqId.ToString());
        Session["DtRequestProduct"] = dt;
        //bind gridview by function in common class
        objPageCmn.FillData((object)gvProductRequest, dt, "", "");
        dt = null;
    }
    public string SuggestedProductName(string ProductId, string TransId)
    {
        string ProductName = string.Empty;
        // string[] pname;
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(StrCompId.ToString(), StrBrandId, ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
                DtPurchaseDetail = ObjPurchaseRequestDetail.GetPurchaseRequestDetailbyTransIdandRequestId(StrCompId, StrBrandId, StrLocationId, editid.Value, TransId);
                try
                {
                    ProductName = DtPurchaseDetail.Rows[0]["SuggestedProductName"].ToString();
                }
                catch
                {
                    DataTable DtProduct = (DataTable)Session["DtRequestProduct"];
                    ProductName = (new DataView(DtProduct, "Trans_Id=" + TransId, "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["SuggestedProductName"].ToString();
                    DtProduct = null;
                }
                DtPurchaseDetail = null;
            }
            else
            {
                // ProductName = ProductId;
                DataTable DtProduct = (DataTable)Session["DtRequestProduct"];
                ProductName = (new DataView(DtProduct, "Trans_Id=" + TransId, "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["SuggestedProductName"].ToString();
                DtProduct = null;
            }
        }
        dt = null;
        return ProductName;
    }
    private void FillGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlOption.SelectedIndex == 1)
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
        strWhereClause = "Location_id in (" + ddlLocation.SelectedValue + ") and isActive='true'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        if (ddlStatus.SelectedItem.Value == "Open")
        {
            strWhereClause += " and Field3='Open'";
        }
        if (ddlStatus.SelectedItem.Value == "Close")
        {
            strWhereClause += " and Field3='Close'";
        }
        if (ddlStatus.SelectedItem.Value == "Reject")
        {
            strWhereClause += " and Field3='Reject'";
        }
        int totalRows = 0;
        using (DataTable dt = ObjPurchaseReqestHeader.getRequestList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), gvPurchaseRequest.Attributes["CurrentSortField"], gvPurchaseRequest.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvPurchaseRequest, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                gvPurchaseRequest.DataSource = null;
                gvPurchaseRequest.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
    }
    public void Reset()
    {
        txtTermCondition.Text = "";
        txtExpDelDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtRequestdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtlRequestNo.Text = ViewState["DocNo"].ToString();
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtbinValue.Text = "";
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueBinDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        ViewState["DepartmentApproval"] = null;
        txtRequestdate.Focus();
        txtValue.Focus();
        hdnLocationID.Value = Session["LocId"].ToString();
        Session["DtSearchProduct"] = null;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        Session["DtRequestProduct"] = null;
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
    protected string GetDocumentNum()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "12", "44", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    #endregion
    #region Bin Section
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedItem.Value == "RequestDate" || ddlbinFieldName.SelectedItem.Value == "ExpDelDate")
        {
            if (txtValueBinDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtbinValue.Text = (txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtbinValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                txtbinValue.Text = "";
                return;
            }
        }
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtBinPurchaseRequest = (DataTable)Session["DtBinPurchaseRequest"];
            DataView view = new DataView(dtBinPurchaseRequest, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //bind gridview by function in common class
            objPageCmn.FillData((object)gvBinPurchaseRequest, view.ToTable(), "", "");
        }
        if (txtbinValue.Text != "")
            txtbinValue.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtbinValue.Visible = true;
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvBinPurchaseRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBinPurchaseRequest.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            //bind gridview by function in common class
            objPageCmn.FillData((object)gvBinPurchaseRequest, (DataTable)Session["dtbinFilter"], "", "");
        }
        string temp = string.Empty;
        bool isselcted;
        for (int i = 0; i < gvBinPurchaseRequest.Rows.Count; i++)
        {
            Label lblconid = (Label)gvBinPurchaseRequest.Rows[i].FindControl("lblReqId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvBinPurchaseRequest.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        gvBinPurchaseRequest.BottomPagerRow.Focus();
    }
    protected void gvBinPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtBinFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //bind gridview by function in common class
        objPageCmn.FillData((object)gvBinPurchaseRequest, (DataTable)Session["dtbinFilter"], "", "");
        gvBinPurchaseRequest.HeaderRow.Focus();
        dt = null;
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvBinPurchaseRequest.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvBinPurchaseRequest.Rows.Count; i++)
        {
            ((CheckBox)gvBinPurchaseRequest.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvBinPurchaseRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvBinPurchaseRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvBinPurchaseRequest.Rows[i].FindControl("lblReqId"))).Text.Trim().ToString())
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
        ((CheckBox)gvBinPurchaseRequest.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvBinPurchaseRequest.Rows[index].FindControl("lblReqId");
        if (((CheckBox)gvBinPurchaseRequest.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)gvBinPurchaseRequest.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPr = (DataTable)Session["dtBinFilter"];
        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPr.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < gvBinPurchaseRequest.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvBinPurchaseRequest.Rows[i].FindControl("lblReqId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvBinPurchaseRequest.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtPr1 = (DataTable)Session["dtBinFilter"];
            //bind gridview by function in common class
            objPageCmn.FillData((object)gvBinPurchaseRequest, (DataTable)Session["dtbinFilter"], "", "");
            ViewState["Select"] = null;
        }

    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseRequestApproval");
            DataTable dt1 = new DataTable();
            string EmpPermission = string.Empty;
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    EmpPermission = ObjSysParam.Get_Approval_Parameter_By_Name("PurchaseRequest").Rows[0]["Approval_Level"].ToString();
                    dt1 = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "44", Session["EmpId"].ToString());
                    if (dt1.Rows.Count == 0)
                    {
                        DisplayMessage("Approval setup issue , please contact to your admin");
                        return;
                    }
                }
            }
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjPurchaseReqestHeader.DeletePurchaseRequestHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), UserId.ToString(), DateTime.Now.ToString());
                    //Add Approval New Concept On 08-12-2014
                    if (dt1.Rows.Count > 0)
                    {
                        for (int h = 0; h < dt1.Rows.Count; h++)
                        {
                            string PriorityEmpId = dt1.Rows[h]["Emp_Id"].ToString();
                            string IsPriority = dt1.Rows[h]["Priority"].ToString();
                            int cur_trans_id = 0;
                            if (EmpPermission == "1")
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else if (EmpPermission == "2")
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else if (EmpPermission == "3")
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                            else
                            {
                                cur_trans_id = objEmpApproval.InsertApprovalTransaciton("6", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                            }
                        }
                    }
                    //End Approval Concept                                    
                }
            }
            Dt = null;
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
            foreach (GridViewRow Gvr in gvBinPurchaseRequest.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
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
        txtbinValue.Focus();
    }
    public void FillGridBin()
    {
        DataTable dt = ObjPurchaseReqestHeader.GetPurchaseRequestHeaderFalseAll(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString());
        //if (HttpContext.Current.Session["DepartmentId"].ToString() != "0")
        //{
        //    dt = new DataView(dt, "Field1='" + HttpContext.Current.Session["DepartmentId"].ToString() + "'", "Trans_Id Desc", DataViewRowState.CurrentRows).ToTable();
        //}
        //bind gridview by function in common class
        objPageCmn.FillData((object)gvBinPurchaseRequest, dt, "", "");
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count;
        Session["DtBinPurchaseRequest"] = dt;
        Session["DtBinFilter"] = dt;
        dt = null;
    }
    #endregion
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/PurchaseRequestPrint.aspx?RId=" + e.CommandArgument.ToString() + "','window','width=1024');", true);
    }
    protected void btnSavePrint_Click(object sender, EventArgs e)
    {
        btnSave_Click(null, null);
        if (ViewState["Return"] == null)
        {
            txtlRequestNo.Text = ViewState["RequestNo"].ToString();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/PurchaseRequestPrint.aspx?RId=" + txtlRequestNo.Text.Trim() + "','window','width=1024');", true);
            Reset();
            ViewState["RequestNo"] = null;
        }
        ViewState["Return"] = null;
    }
    public string GetDesc(string Desc)
    {
        string s = "";
        if (Desc.ToString().Length > 15)
        {
            s = Desc.ToString().Substring(0, 14);
        }
        else
        {
            s = Desc.ToString();
        }
        return s;
    }
    //protected string GetEmployeeCode(string strEmployeeId)
    //{
    //    string strEmployeeName = string.Empty;
    //    if (strEmployeeId != "0" && strEmployeeId != "")
    //    {
    //        using (DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId))
    //        {
    //            if (dtEName.Rows.Count > 0)
    //            {
    //                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
    //                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
    //            }
    //            else
    //            {
    //                ViewState["Emp_Img"] = "";
    //            }
    //        }
    //    }
    //    else
    //    {
    //        strEmployeeName = "";
    //    }
    //    return strEmployeeName;
    //}
    #region View
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    #endregion
    #region Date Search
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "RequestDate" || ddlFieldName.SelectedItem.Value == "ExpDelDate")
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
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinFieldName.SelectedItem.Value == "RequestDate" || ddlbinFieldName.SelectedItem.Value == "ExpDelDate")
        {
            txtValueBinDate.Visible = true;
            txtbinValue.Visible = false;
            txtbinValue.Text = "";
            txtValueBinDate.Text = "";
        }
        else
        {
            txtValueBinDate.Visible = false;
            txtbinValue.Visible = true;
            txtbinValue.Text = "";
            txtValueBinDate.Text = "";
        }
        ddlbinFieldName.Focus();
    }
    #endregion
    #region Advance Search
    public DataTable CreateProductDataTable()
    {
        DataTable dtProduct = new DataTable();
        dtProduct.Columns.Add("Trans_Id", typeof(int));
        dtProduct.Columns.Add("Serial_No", typeof(int));
        dtProduct.Columns.Add("Product_Id");
        dtProduct.Columns.Add("UnitId");
        dtProduct.Columns.Add("ReqQty");
        dtProduct.Columns.Add("ProductDescription");
        dtProduct.Columns.Add("SuggestedProductName");
        return dtProduct;
    }
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        DataTable dt = CreateProductDataTable();
        int i = 0;
        foreach (GridViewRow gvr in gvProductRequest.Rows)
        {
            i = i + 1;
            DataRow dr = dt.NewRow();
            Label lblSerialNo = (Label)gvr.FindControl("lblSerialNO");
            Label lblProductId = (Label)gvr.FindControl("lblPID");
            Label lblUnitId = (Label)gvr.FindControl("lblUID");
            Label lblReqQty = (Label)gvr.FindControl("lblReqQty");
            Label lblProductDescription = (Label)gvr.FindControl("lblDescription");
            HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
            dr["Trans_Id"] = lblSerialNo.Text;
            dr["Serial_No"] = lblSerialNo.Text;
            dr["Product_Id"] = lblProductId.Text;
            dr["UnitId"] = lblUnitId.Text;
            dr["ProductDescription"] = lblProductDescription.Text;
            dr["ReqQty"] = lblReqQty.Text;
            dr["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows.Add(dr);
        }
        Session["DtRequestProduct"] = dt;
        Session["DtSearchProduct"] = Session["DtRequestProduct"];
        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=PR','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        dt = null;
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        if (Session["DtSearchProduct"] != null)
        {
            Session["DtRequestProduct"] = Session["DtSearchProduct"];
            if (Session["DtRequestProduct"] != null)
            {
                //bind gridview by function in common class
                objPageCmn.FillData((object)gvProductRequest, (DataTable)Session["DtRequestProduct"], "", "");
            }
            Session["DtSearchProduct"] = null;
        }
        else
        {
            if (Session["DtRequestProduct"] != null)
            {
                //bind gridview by function in common class
                objPageCmn.FillData((object)gvProductRequest, (DataTable)Session["DtRequestProduct"], "", "");
            }
            DisplayMessage("Product Not Found");
            return;
        }
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFormView.Checked == true)
        {
            btnAddProduct.Visible = true;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
            btnAddProduct.Focus();
            btnAddProduct_Click(null, null);
            pnlproductpanel.Visible = true;
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddProduct.Visible = false;
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
            btnAddProductScreen.Focus();
            btnProductClose_Click(null, null);
            pnlproductpanel.Visible = false;
        }
    }
    #endregion
    #region Status
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    #endregion

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }


    #region ProductHistory
    public string GetProductStock(string strProductId)
    {
        string SysQty = string.Empty;
        try
        {
            SysQty = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();
        }
        catch
        {
            SysQty = "0";
        }
        if (SysQty == "")
        {
            SysQty = "0.000";
        }
        return GetAmountDecimal(SysQty);
    }

    public string GetAmountDecimal(string Amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);

    }
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        string CustomerName = string.Empty;
        try
        {
            CustomerName = "";
        }
        catch
        {
        }
        string strCmd = "window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=PURCHASE&&Contact=" + CustomerName + "')";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
    }
    #endregion
}