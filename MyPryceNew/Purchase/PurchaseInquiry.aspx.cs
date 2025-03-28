using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class Purchase_PurchaseInquiry : BasePage
{
    #region defined Class Object
    DataAccessClass objDa = null;
    Common cmn = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Inv_PurchaseInquiryHeader objPIHeader = null;
    Inv_PurchaseInquiryDetail ObjPIDetail = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    Ems_GroupMaster objGroupMaster = null;
    PurchaseRequestHeader objPRHeader = null;
    PurchaseRequestDetail ObjPRDetail = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
    Inv_PurchaseQuoteDetail objPQDetail = null;
    Inv_SalesInquiryDetail ObjSalesInquiryDetail = null;
    Inv_SalesInquiryHeader ObjSelesInquiryHeader = null;
    Inv_StockDetail objStockDetail = null;
    Set_DocNumber objDocNo = null;
    Inv_PurchaseQuoteHeader ObjPQuotheader = null;
    Inv_ParameterMaster objInvParameter = null;
    Inv_ProductSuppliers ObjProductSupplier = null;
    LocationMaster objLocation = null;
    Inv_SalesOrderDetail ObjSalesOrderDetail = null;
    PageControlCommon objPageCmn = null;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        btnPISave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPISave, "").ToString());

        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        ObjPIDetail = new Inv_PurchaseInquiryDetail(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        objGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        objPRHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        ObjPRDetail = new PurchaseRequestDetail(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPQDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        ObjSalesInquiryDetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        ObjSelesInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjPQuotheader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        objInvParameter = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjProductSupplier = new Inv_ProductSuppliers(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSalesOrderDetail = new Inv_SalesOrderDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Purchase/PurchaseInquiry.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            hdnLocationId.Value = Session["LocId"].ToString();

            ddlOption.SelectedIndex = 2;
            FillGrid();
            FillCurrency();
            Session["DtDetail"] = null;
            txtPINo.Text = GetDocumentNum();
            ViewState["DocNo"] = txtPINo.Text;
            txtPIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueRequestDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            BtnReset_Click(null, null);
            Session["DtSearchProduct"] = null;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            ddlFieldName.Focus();
            Session["dtInquirySupplier"] = null;
        }

    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region System defined Function
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        if (Session["LocId"].ToString() != e.CommandName.ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }
        DataTable dtPIEdit = objPIHeader.GetPIHeaderAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        //DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        editid.Value = e.CommandArgument.ToString();
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

        if (dtPIEdit.Rows.Count > 0)
        {
            txtPINo.Text = dtPIEdit.Rows[0]["PI_No"].ToString();
            ViewState["TimeStamp"] = dtPIEdit.Rows[0]["Row_Lock_Id"].ToString();
            txtPINo.ReadOnly = true;
            txtPIDate.Text = Convert.ToDateTime(dtPIEdit.Rows[0]["PIDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtDescription.Text = dtPIEdit.Rows[0]["Description"].ToString();
            try
            {
                txtBudgetPrice.Text = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), dtPIEdit.Rows[0]["Field1"].ToString()).ToString();
            }
            catch
            {
                txtBudgetPrice.Text = "0";
            }
            if (dtPIEdit.Rows[0]["Field2"].ToString() != "")
                ddlCurrency.SelectedValue = dtPIEdit.Rows[0]["Field2"].ToString();
            txtFooter.Content = dtPIEdit.Rows[0]["Field4"].ToString();
            txtHeader.Content = dtPIEdit.Rows[0]["Field3"].ToString();
            //Add Trans Panel
            string strTransFrom = dtPIEdit.Rows[0]["TransFrom"].ToString();
            string strTransNo = dtPIEdit.Rows[0]["TransNo"].ToString();
            hdnTransNo.Value = strTransNo;
            if (strTransFrom != "")
            {
                pnlTrans.Visible = true;
                if (strTransFrom == "PR")
                {
                    txtDepartment.Visible = true;
                    lblDepartment.Visible = true;
                    txtEmployeeName.Visible = true;
                    lblEmployeeName.Visible = true;
                    ViewState["InquiryType"] = "PI";
                    txtTransFrom.Text = Resources.Attendance.Purchase_Request;
                    DataTable dtRequest = objPRHeader.GetPurchaseRequestTrueAllByReqId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strTransNo);
                    if (dtRequest.Rows.Count > 0)
                    {
                        txtTransNo.Text = dtRequest.Rows[0]["RequestNo"].ToString();
                        txtDepartment.Text = dtRequest.Rows[0]["DepartmentName"].ToString();
                        txtEmployeeName.Text = dtRequest.Rows[0]["CreatedBy"].ToString();
                    }
                    dtRequest = null;
                }
                else
                {
                    ViewState["InquiryType"] = "SI";
                    txtDepartment.Visible = true;
                    lblDepartment.Visible = true;
                    txtEmployeeName.Visible = true;
                    lblEmployeeName.Text = Resources.Attendance.Customer_Name;
                    lblEmployeeName.Visible = true;
                    lblOCDate.Visible = true;
                    txtOCDt.Visible = true;
                    //lblOCDtColon.Visible = true;
                    lblTender.Visible = true;
                    txtTender.Visible = true;
                    // lblTenderColon.Visible = true;
                    txtTransFrom.Text = Resources.Attendance.Sales_Inquiry;
                    DataTable dtRequest = ObjSelesInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strTransNo);
                    if (dtRequest.Rows.Count > 0)
                    {
                        txtTransNo.Text = dtRequest.Rows[0]["SInquiryNo"].ToString();
                        txtDepartment.Text = dtRequest.Rows[0]["DepartmentName"].ToString();
                        txtEmployeeName.Text = dtRequest.Rows[0]["Name"].ToString();
                        txtOCDt.Text = GetDate(dtRequest.Rows[0]["OrderCompletionDate"].ToString());
                        txtTender.Text = dtRequest.Rows[0]["TenderNo"].ToString();
                    }
                    dtRequest = null;
                }
            }
            //Add Product Section For Edit
            DataTable dtProduct = ObjPIDetail.GetPIDetailByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            if (dtProduct.Rows.Count > 0)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, dtProduct, "", "");
                Session["DtDetail"] = dtProduct;
            }
            hdnTransNo.Value = dtPIEdit.Rows[0]["TransNo"].ToString();
            dtProduct = null;
        }
        dtPIEdit = null;
        //AllPageCode();
        tabNewClick();

        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
        if (objSenderID == "lnkViewDetail")
        {
            //btnPICancel.Visible = false;
            //Next_Div.Visible = true;
            //Main_Div.Visible = false;
            //btnResetSupplier.Visible = false;
            //tr1.Visible = false;
            //tr2.Visible = false;
            //traddsup.Visible = false;
            //CheckedandDisableSupplier();
            btnProductSave.Visible = false;
            btnPICancel.Visible = true;
            Next_Div.Visible = false;
            Main_Div.Visible = true;
            traddsup.Visible = true;
            btnResetSupplier.Visible = true;
            tr1.Visible = true;
            tr2.Visible = true;
            CheckedandDisableSupplier();
        }
        else
        {
            btnPICancel.Visible = true;
            Next_Div.Visible = false;
            Main_Div.Visible = true;
            traddsup.Visible = true;
            btnResetSupplier.Visible = true;
            tr1.Visible = true;
            tr2.Visible = true;
        }
    }
    protected void GvPurchaseInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseInquiry.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_PI_Inq"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseInquiry, dt, "", "");
        //AllPageCode();
        dt = null;
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "PIDate")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString("dd-MMM-yyyy");
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
                return;
            }
        }
        FillGrid(1);
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvPurchaseInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvPurchaseInquiry.Attributes["CurrentSortField"] != null &&
            GvPurchaseInquiry.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvPurchaseInquiry.Attributes["CurrentSortField"])
            {
                if (GvPurchaseInquiry.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvPurchaseInquiry.Attributes["CurrentSortField"] = sortField;
        GvPurchaseInquiry.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvPurchaseInquiryCurrentPageIndex.Value));
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        string quotHeader = ObjPQuotheader.purchaseQuoteHeaderCountFromPurchaseInquiry(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);

        if (quotHeader != "0")
        {
            DisplayMessage("Inquiry is used in quotation , you can not delete this record");
            return;
        }
        int b = 0;
        b = objPIHeader.DeletePIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        txtValue.Focus();
        FillGrid();
    }
    protected void btnPICancel_Click(object sender, EventArgs e)
    {
        Reset();
        //FillGrid();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        editid.Value = "";
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
    }
    protected void btnPISave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        //    DataTable dtInvEdit = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        //here we check that this page is updated by other user before save of current user 
        //this code is created by jitendra upadhyay on 02-06-2015
        //code start
        if (editid.Value != "")
        {
            DataTable dtPIEdit = objPIHeader.GetPIHeaderAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            if (dtPIEdit.Rows.Count != 0)
            {
                try
                {
                    if (ViewState["TimeStamp"].ToString() != dtPIEdit.Rows[0]["Row_Lock_Id"].ToString())
                    {
                        DisplayMessage("Another User update Information reload and try again");
                        return;
                    }
                }
                catch
                {
                }
            }
            dtPIEdit = null;
        }
        //code end
        if (txtPIDate.Text.Trim() == "")
        {
            DisplayMessage("Enter Purchase Inquiry Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPIDate);
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtPIDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Inquiry Date in format " + Session["DateFormat"].ToString() + "");
                txtPIDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPIDate);
                return;
            }
        }
        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtPIDate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }
        if (txtPINo.Text.Trim() == "")
        {
            DisplayMessage("Enter Purchase Inquiry No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
            return;
        }
        if (txtBudgetPrice.Text.Trim() == "")
        {
            txtBudgetPrice.Text = "0";
        }
        if (ddlCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Select Currency ");
            ddlCurrency.Focus();
            return;
        }
        else
        {
            if (editid.Value == "")
            {
                DataTable dtPINo = objPIHeader.CheckPurchaseInquiryNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtPINo.Text);
                if (dtPINo.Rows[0][0].ToString() != "0")
                {
                    DisplayMessage("Purchase Inquiry No. Already Exits");
                    txtPINo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
                    return;
                }
                dtPINo = null;
            }
        }
        if (pnlTrans.Visible == true)
        {
            //if (txtTransNo.Text.Trim() == "")
            //{
            //    DisplayMessage("Enter Transfer No.");
            //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTransNo);
            //    return;
            //}
            if (txtTransFrom.Text == "Purchase Request")
            {
                txtTransFrom.Text = "PR";
            }
            if (txtTransFrom.Text == "Sales Inquiry")
            {
                txtTransFrom.Text = "SI";
            }
        }
        if (GvProduct.Rows.Count > 0)
        {
            string ProductId = string.Empty;
            foreach (GridViewRow gvr in GvProduct.Rows)
            {
                Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
                if (ProductId == "")
                {
                    ProductId = lblProductId.Text;
                }
                else
                {
                    ProductId = ProductId + "," + lblProductId.Text;
                }
            }
            Session["SupplierProductList"] = ProductId;
        }
        else
        {
            DisplayMessage("Add Atleast One Product");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
            return;
        }
        int b = 0;
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (editid.Value != "")
            {
                b = objPIHeader.UpdatePIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtPINo.Text, ObjSysParam.getDateForInput(txtPIDate.Text).ToString(), txtTransFrom.Text, hdnTransNo.Value, txtDescription.Text, "Pending", txtBudgetPrice.Text, ddlCurrency.SelectedValue.ToString(), txtHeader.Content, txtFooter.Content, "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    //Add PIDetail Record
                    ObjPIDetail.DeletePIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ref trns);
                    foreach (GridViewRow gvr in GvProduct.Rows)
                    {
                        Label lblSerialNo = (Label)gvr.FindControl("lblSNo");
                        Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
                        Label lblUnitId = (Label)gvr.FindControl("lblgvUnitId");
                        Label lblReqQty = (Label)gvr.FindControl("lblgvRequiredQty");
                        Literal lblProductDescription = (Literal)gvr.FindControl("lblgvProductDescription");
                        HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                        Label gvlblSONo = (Label)gvr.FindControl("gvlblSONo");
                        HiddenField gvhdnSOID = (HiddenField)gvr.FindControl("gvhdnSOID");

                        DataTable dtProduct = new DataTable();
                        try
                        {
                            dtProduct = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), lblProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                        catch
                        {
                            lblProductId.Text = "0";
                        }
                        ObjPIDetail.InsertPIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, lblSerialNo.Text, lblProductId.Text, hdnSuggestedProductName.Value, lblProductDescription.Text, lblUnitId.Text, lblReqQty.Text, gvhdnSOID.Value, "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                else
                {
                    DisplayMessage("Add Atleast One Product");
                }
            }
            else
            {
                try
                {
                    b = objPIHeader.InsertPIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtPINo.Text, ObjSysParam.getDateForInput(txtPIDate.Text).ToString(), txtTransFrom.Text, hdnTransNo.Value, txtDescription.Text, "Pending", txtBudgetPrice.Text, ddlCurrency.SelectedValue.ToString(), txtHeader.Content, txtFooter.Content, "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Cannot insert duplicate key row in object"))
                    {
                        DisplayMessage("Inquiry already Created for this Purchase Request");
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
                if (b != 0)
                {
                    editid.Value = b.ToString();
                    int Id = b;
                    if (txtPINo.Text == ViewState["DocNo"].ToString())
                    {

                        DataTable dtCount = objPIHeader.getTotalCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                        if (dtCount.Rows[0][0].ToString() == "0")

                        {
                            if (dtCount.Rows.Count == 0)
                            {
                                objPIHeader.Updatecode(b.ToString(), txtPINo.Text + "1", ref trns);
                                txtPINo.Text = txtPINo.Text + "1";
                            }
                            else
                            {
                                objPIHeader.Updatecode(b.ToString(), txtPINo.Text + dtCount.Rows.Count, ref trns);
                                txtPINo.Text = txtPINo.Text + dtCount.Rows.Count;
                            }
                        }

                        else
                        {
                            objPIHeader.Updatecode(b.ToString(), txtPINo.Text + dtCount.Rows[0][0].ToString(), ref trns);
                            txtPINo.Text = txtPINo.Text + dtCount.Rows[0][0].ToString();
                        }

                    }
                    //}
                    //Strart Update Status of sales inquiry  
                    if (txtTransFrom.Text == "SI")
                    {
                        string SIId = string.Empty;
                        ObjSelesInquiryHeader.UpdateSalesInquiryStatusbyPI(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), hdnTransNo.Value.Trim(), "Send Inquiry To Supplier", ref trns);
                    }
                    //update status of purchase request 
                    if (txtTransFrom.Text.Trim() == "PR")
                    {
                        objPRHeader.UpdatePurchaseRequestStatusbyPI(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), hdnTransNo.Value.Trim(), "Close", ref trns);
                    }
                    //End
                    //Add PIDetail Record
                    foreach (GridViewRow gvr in GvProduct.Rows)
                    {
                        Label lblSerialNo = (Label)gvr.FindControl("lblSNo");
                        Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
                        Label lblUnitId = (Label)gvr.FindControl("lblgvUnitId");
                        Label lblReqQty = (Label)gvr.FindControl("lblgvRequiredQty");
                        Literal lblProductDescription = (Literal)gvr.FindControl("lblgvProductDescription");
                        HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
                        Label gvlblSONo = (Label)gvr.FindControl("gvlblSONo");
                        HiddenField gvhdnSOID = (HiddenField)gvr.FindControl("gvhdnSOID");

                        DataTable dtProduct = new DataTable();
                        try
                        {
                            dtProduct = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), lblProductId.Text.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        }
                        catch
                        {
                            lblProductId.Text = "0";
                        }
                        ObjPIDetail.InsertPIDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), lblSerialNo.Text, lblProductId.Text, hdnSuggestedProductName.Value, lblProductDescription.Text, lblUnitId.Text, lblReqQty.Text, gvhdnSOID.Value, "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                    //DisplayMessage("Record Saved","green");
                }
                else
                {
                    DisplayMessage("Record  Not Saved");
                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            txtSInquiryNo.Text = txtPINo.Text;
            txtSInquiryDate.Text = txtPIDate.Text;
            Next_Div.Visible = true;
            Main_Div.Visible = false;
            FillSupplierGroup();
            FillSupplier();
            FillGrid();
            Reset();
            //FillRequestGrid();
            CheckedandDisableSupplier();
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
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objPIHeader.DeletePIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
        FillGridBin();
    }
    protected void btnPRequest_Click(object sender, EventArgs e)
    {
        FillRequestGrid();
    }
    protected void GvPurchaseInquiryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseInquiryBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterBin"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseInquiryBin, dt, "", "");
        dt = null;
        string temp = string.Empty;
        for (int i = 0; i < GvPurchaseInquiryBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvPurchaseInquiryBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvPurchaseInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvPurchaseInquiryBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilterBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilterBin"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseInquiryBin, dt, "", "");
        lblSelectedRecord.Text = "";
        dt = null;
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objPIHeader.GetPIHeaderAllFalse(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseInquiryBin, dt, "", "");
        Session["dtPInquiryBin"] = dt;
        Session["dtFilterBin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        dt = null;
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "PIDate")
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
            Session["dtFilterBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseInquiryBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            //AllPageCode();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            // btnRefreshBin.Focus();
            dtCust = null;
            view = null;
        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();

    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvPurchaseInquiryBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvPurchaseInquiryBin.Rows.Count; i++)
        {
            ((CheckBox)GvPurchaseInquiryBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvPurchaseInquiryBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvPurchaseInquiryBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvPurchaseInquiryBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
        HiddenField lb = (HiddenField)GvPurchaseInquiryBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvPurchaseInquiryBin.Rows[index].FindControl("chkSelect")).Checked)
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
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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
            for (int i = 0; i < GvPurchaseInquiryBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvPurchaseInquiryBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvPurchaseInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
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
            objPageCmn.FillData((object)GvPurchaseInquiryBin, dtUnit1, "", "");
            ViewState["Select"] = null;
            dtUnit1 = null;
        }
        dtPbrand = null;
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objPIHeader.DeletePIHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            foreach (GridViewRow Gvr in GvPurchaseInquiryBin.Rows)
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
    protected string GetValuefromSI(string TransId, int i)
    {
        using (DataTable dtRequest = ObjSelesInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), TransId))
        {
            if (dtRequest.Rows.Count > 0)
            {
                if (i == 2)
                    return dtRequest.Rows[0]["TenderNo"].ToString();
                if (i == 1)
                    return dtRequest.Rows[0]["Name"].ToString();
                if (i == 3)
                    return GetDate(dtRequest.Rows[0]["OrderCompletiondate"].ToString());
                else
                    return "";
            }
            else
            {
                return "";
            }
        }
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

        int totalRows = 0;
        using (DataTable dt = objPIHeader.getPruchaseInquiryList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPurchaseInquiry.Attributes["CurrentSortField"], GvPurchaseInquiry.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPurchaseInquiry, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPurchaseInquiry.DataSource = null;
                GvPurchaseInquiry.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
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
        btnPICancel.Visible = true;
        btnResetSupplier.Visible = true;
        tr1.Visible = true;
        tr2.Visible = true;
        txtPIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtPINo.ReadOnly = false;
        txtDescription.Text = "";
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        pnlTrans.Visible = false;
        txtTransFrom.Text = "";
        txtTransNo.Text = "";
        Session["DtDetail"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        txtValueDate.Text = "";
        txtValueBinDate.Text = "";
        txtValueRequest.Text = "";
        txtValueRequestDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        txtValueRequest.Visible = true;
        txtValueRequestDate.Visible = false;
        txtDepartment.Text = "";
        txtDepartment.Visible = false;
        lblDepartment.Visible = false;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        //txtPINo.Text = GetDocumentNumber();
        txtPINo.Text = GetDocumentNum();
        ViewState["DocNo"] = txtPINo.Text;
        txtBudgetPrice.Text = "";
        txtHeader.Content = "";
        txtFooter.Content = "";
        pnlChkSupplier.Enabled = true;
        ChkSupplier.Enabled = true;
        Session["DtSearchProduct"] = null;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        ViewState["Dtproduct"] = null;
        Session["dtInquirySupplier"] = null;
        objPageCmn.FillData((object)GridProductSupplierCode, null, "", "");
        ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
    }
    protected string GetDocumentNum()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "12", "49", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    #endregion
    #region Add Product Concept
    public void FillUnit(string ProductId)
    {
        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetProductCode_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        dt = null;
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = HttpContext.Current.Session["ContactId"].ToString();
        DataTable dt = ObjContactMaster.GetContactTrueAllData(id, "Individual");
        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
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
    protected void btnAddNewProduct_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = true;
        ResetProduct();
        Session["DtSearchProduct"] = null;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text.Trim() != "")
        {
            DataTable dtProduct = new DataTable();
            try
            {
                //dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.ToString());
                dtProduct = objDa.return_DataTable("select top 1 Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_ProductMaster.Description, Inv_ProductMaster.UnitId, case when relatedProducts IS null then 0 else relatedProducts end as relatedProducts from Inv_ProductMaster left join (select count(Product_Id) relatedProducts,Product_Id from Inv_Product_RelProduct group by Product_Id) relatedP on relatedP.Product_Id=Inv_ProductMaster.ProductId where  Inv_ProductMaster.EProductName ='" + txtProductName.Text + "'");
            }
            catch
            {
            }
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                //DataTable dt = CreateProductDataTable();
                //for (int i = 0; i < GvProduct.Rows.Count; i++)
                //{
                //    dt.Rows.Add(i);
                //    Label lblSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
                //    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                //    Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
                //    Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
                //    Literal lblgvPDescription = (Literal)GvProduct.Rows[i].FindControl("lblgvProductDescription");
                //    dt.Rows[i]["Serial_No"] = lblSNo.Text;
                //    dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
                //    dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
                //    dt.Rows[i]["ReqQty"] = lblgvReqQty.Text;
                //    dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
                //}
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (dt.Rows[i]["Product_Id"].ToString() == dtProduct.Rows[0]["ProductId"].ToString())
                //    {
                //        //18-03-2016
                //        //commented for allow duplicate product 
                //        //DisplayMessage("Product is already exists!");
                //        //txtProductName.Text = "";
                //        //txtProductcode.Text = "";
                //        //txtProductName.Focus();
                //        //return;
                //    }
                //}
                FillUnit(dtProduct.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription0.Visible = true;
            }
            else
            {
                FillUnit("0");
                hdnNewProductId.Value = "0";
                txtPDesc.Visible = true;
                pnlPDescription0.Visible = false;
            }
            txtRequestQty.Text = "1";
            dtProduct = null;
        }
        else
        {
            ddlUnit.Items.Clear();
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text.Trim() != "")
        {
            DataTable dtProduct = new DataTable();
            try
            {
                //dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductcode.Text.ToString());
                dtProduct = objDa.return_DataTable("select top 1 Inv_ProductMaster.ProductId, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_ProductMaster.Description, Inv_ProductMaster.UnitId, case when relatedProducts IS null then 0 else relatedProducts end as relatedProducts from Inv_ProductMaster left join (select count(Product_Id) relatedProducts,Product_Id from Inv_Product_RelProduct group by Product_Id) relatedP on relatedP.Product_Id=Inv_ProductMaster.ProductId where Inv_ProductMaster.ProductCode = '" + txtProductcode.Text + "'");
            }
            catch
            {
            }
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                ////updated by varsha
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                //DataTable dt = CreateProductDataTable();
                //for (int i = 0; i < GvProduct.Rows.Count; i++)
                //{
                //    dt.Rows.Add(i);
                //    Label lblSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
                //    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                //    Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
                //    Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
                //    Literal lblgvPDescription = (Literal)GvProduct.Rows[i].FindControl("lblgvProductDescription");
                //    dt.Rows[i]["Serial_No"] = lblSNo.Text;
                //    dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
                //    dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
                //    dt.Rows[i]["ReqQty"] = lblgvReqQty.Text;
                //    dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
                //}
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    if (dt.Rows[i]["Product_Id"].ToString() == dtProduct.Rows[0]["ProductId"].ToString())
                //    {
                //        //18-03-2016
                //        //commented for allow duplicate product 
                //        //DisplayMessage("Product is already exists!");
                //        //txtProductName.Text = "";
                //        //txtProductcode.Text = "";
                //        //txtProductcode.Focus();
                //        //ddlUnit.Items.Clear();
                //        //return;
                //    }
                //}
                FillUnit(dtProduct.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription0.Visible = true;
            }
            else
            {
                FillUnit("0");
                hdnNewProductId.Value = "0";
                txtPDesc.Visible = true;
                pnlPDescription0.Visible = false;
            }
            txtRequestQty.Text = "1";
            dtProduct = null;
        }
        else
        {
            ddlUnit.Items.Clear();
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
    }
    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    //Date 16 aug 14
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        string SuggestedProductName = string.Empty;
        if (txtProductcode.Text.Trim() == "")
        {
            txtProductcode.Text = "";
            DisplayMessage("Enter Product Id");
            txtProductcode.Focus();
            return;
        }
        if (txtProductName.Text.Trim() != "")
        {
            //string strA = "0";
            //foreach (GridViewRow gve in GvProduct.Rows)
            //{
            //    Label lblgvProductName = (Label)gve.FindControl("lblgvProductName");
            //    if (txtProductName.Text == lblgvProductName.Text)
            //    {
            //        strA = "1";
            //    }
            //}
            if (hdnNewProductId.Value == "0")
            {
                if (txtProductName.Text.Trim() != "")
                {
                    DataTable dt = objProductM.GetProductName_PreText(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtProductName.Text);
                    //dt = new DataView(dt, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnNewProductId.Value = dt.Rows[0]["ProductId"].ToString();
                    }
                    else
                    {
                        hdnNewProductId.Value = "0";//here we insert the description
                        SuggestedProductName = txtProductName.Text;
                    }
                    dt = null;
                }
            }
            if (ddlUnit.Visible == true)
            {
                if (ddlUnit == null)
                {
                    DisplayMessage("Select Unit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlUnit);
                    return;
                }
                else
                {
                    hdnNewUnitId.Value = ddlUnit.SelectedValue;
                }
            }
            else if (txtUnit.Visible == true)
            {
                if (txtUnit.Text.Trim() != "")
                {
                    UM.InsertUnitMaster(Session["CompId"].ToString(), txtUnit.Text, txtUnit.Text, "", "0", "1", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    DataTable dtMaxId = UM.GetMaxUnitId(Session["CompId"].ToString());
                    if (dtMaxId.Rows.Count > 0)
                    {
                        hdnNewUnitId.Value = dtMaxId.Rows[0][0].ToString();
                    }
                    dtMaxId = null;
                }
                else
                {
                    DisplayMessage("Enter Unit Name");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtUnit);
                    return;
                }
            }
            if (txtRequestQty.Text.Trim() == "")
            {
                DisplayMessage("Enter Quantity");
                txtRequestQty.Text = "1";
                txtRequestQty.Focus();
                return;
            }
            if (hdnProductId.Value == "")
            {
                FillProductChidGird("Save");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
            }
            else
            {
                if (txtProductName.Text == hdnProductName.Value)
                {
                    FillProductChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                    //pnlProduct1.Visible = false;
                    //pnlProduct2.Visible = false;
                }
                else
                {
                    FillProductChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);
                    //pnlProduct1.Visible = false;
                    //pnlProduct2.Visible = false;
                }
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
        txtPDesc.Text = "";
        txtProductName.Focus();
        txtRequestQty.Text = "1";
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetProduct();
    }

    protected void btnProductClose_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = false;
    }
    public void ResetProduct()
    {
        txtProductName.Text = "";
        txtProductcode.Text = "";
        txtUnit.Text = "";
        ddlUnit.Visible = true;
        txtUnit.Visible = false;
        txtRequestQty.Text = "1";
        txtPDescription.Text = "";
        hdnProductId.Value = "";
        hdnProductName.Value = "";
        hdnNewProductId.Value = "0";
        txtProductcode.Focus();
        ddlUnit.Items.Clear();
        txtProductcode.Focus();
    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ReqQty");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("SOID");
        dt.Columns.Add("SONO");
        return dt;
    }
    public DataTable FillProductDataTabel()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateProductDataTable();
        if (GvProduct.Rows.Count > 0)
        {
            for (int i = 0; i < GvProduct.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvProduct.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
                    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                    Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
                    Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
                    Literal lblgvPDescription = (Literal)GvProduct.Rows[i].FindControl("lblgvProductDescription");
                    HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
                    Label gvlblSONo = (Label)GvProduct.Rows[i].FindControl("gvlblSONo");
                    HiddenField gvhdnSOID = (HiddenField)GvProduct.Rows[i].FindControl("gvhdnSOID");

                    dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
                    strNewSNo = lblgvSNo.Text;
                    dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
                    dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
                    dt.Rows[i]["ReqQty"] = lblgvReqQty.Text;
                    dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
                    dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
                    dt.Rows[i]["SONO"] = gvlblSONo.Text;
                    dt.Rows[i]["SOID"] = gvhdnSOID.Value;
                }
                else
                {
                    //code for get the unique serial number
                    DataTable DtMaxSerial = new DataTable();
                    try
                    {
                        DtMaxSerial.Merge(dt);
                        DtMaxSerial = new DataView(DtMaxSerial, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    dt.Rows.Add(i);
                    if (dt.Rows.Count > 0)
                    {
                        dt.Rows[i]["Serial_No"] = (float.Parse(DtMaxSerial.Rows[0]["Serial_No"].ToString()) + 1).ToString();
                    }
                    else
                    {
                        dt.Rows[i]["Serial_No"] = "1";
                    }
                    dt.Rows[i]["Product_Id"] = hdnNewProductId.Value;
                    dt.Rows[i]["UnitId"] = hdnNewUnitId.Value;
                    dt.Rows[i]["ReqQty"] = txtRequestQty.Text;
                    if (hdnNewProductId.Value == "0")
                    {
                        dt.Rows[i]["ProductDescription"] = txtPDesc.Text;
                        dt.Rows[i]["SuggestedProductName"] = txtProductName.Text;
                    }
                    else
                    {
                        dt.Rows[i]["ProductDescription"] = txtPDescription.Text;
                        dt.Rows[i]["SuggestedProductName"] = "";
                    }//here we check th description
                    dt.Rows[i]["SOID"] = hdnSOID.Value;
                    dt.Rows[i]["SONO"] = hdnSONO.Value;
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["Serial_No"] = "1";
            dt.Rows[0]["Product_Id"] = hdnNewProductId.Value;
            dt.Rows[0]["UnitId"] = hdnNewUnitId.Value;
            dt.Rows[0]["ReqQty"] = txtRequestQty.Text;
            if (hdnNewProductId.Value == "0")
            {
                dt.Rows[0]["ProductDescription"] = txtPDesc.Text;
                dt.Rows[0]["SuggestedProductName"] = txtProductName.Text;
            }
            else
            {
                dt.Rows[0]["ProductDescription"] = txtPDescription.Text;
                dt.Rows[0]["SuggestedProductName"] = "";
            }
            dt.Rows[0]["SOID"] = hdnSOID.Value;
            dt.Rows[0]["SONO"] = hdnSONO.Value;

        }
        return dt;
    }
    public DataTable FillProductDataTabelDelete()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
            Literal lblgvPDescription = (Literal)GvProduct.Rows[i].FindControl("lblgvProductDescription");
            HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
            Label gvlblSONo = (Label)GvProduct.Rows[i].FindControl("gvlblSONo");
            HiddenField gvhdnSOID = (HiddenField)GvProduct.Rows[i].FindControl("gvhdnSOID");
            dt.Rows[i]["Serial_No"] = lblgvSNo.Text;
            dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
            dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
            dt.Rows[i]["ReqQty"] = lblgvReqQty.Text;
            dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
            dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows[i]["SONO"] = gvlblSONo.Text;
            dt.Rows[i]["SOID"] = gvhdnSOID.Value;

        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No<>'" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        ViewState["Dtproduct"] = (DataTable)dt;
        return dt;
    }
    protected void imgBtnProductEdit_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductDataTabelEdit();
        pnlProduct1.Visible = true;
        txtProductcode.Focus();
    }
    public DataTable FillProductDataTabelEdit()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
            Literal lblgvPDescription = (Literal)GvProduct.Rows[i].FindControl("lblgvProductDescription");
            HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
            Label gvlblSONo = (Label)GvProduct.Rows[i].FindControl("gvlblSONo");
            HiddenField gvhdnSOID = (HiddenField)GvProduct.Rows[i].FindControl("gvhdnSOID");
            dt.Rows[i]["Serial_No"] = lblSNo.Text;
            dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
            dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
            dt.Rows[i]["ReqQty"] = lblgvReqQty.Text;
            dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
            dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows[i]["SONO"] = gvlblSONo.Text;
            dt.Rows[i]["SOID"] = gvhdnSOID.Value;

        }
        DataView dv = new DataView(dt);
        dv.RowFilter = "Serial_No='" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtProductName.Text = GetProductName(dt.Rows[0]["Product_Id"].ToString());
            txtProductcode.Text = ProductCode(dt.Rows[0]["Product_Id"].ToString());
            txtRequestQty.Text = dt.Rows[0]["ReqQty"].ToString();
            if (txtProductName.Text != "")
            {
                FillUnit(dt.Rows[0]["Product_Id"].ToString());
                txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
                pnlPDescription0.Visible = true;
                txtPDesc.Visible = false;
            }
            else
            {
                FillUnit("0");
                txtProductName.Text = dt.Rows[0]["SuggestedProductName"].ToString();
                txtPDesc.Text = dt.Rows[0]["ProductDescription"].ToString();
                pnlPDescription0.Visible = false;
                txtPDesc.Visible = true;
            }
            ddlUnit.SelectedValue = dt.Rows[0]["UnitId"].ToString();
            hdnProductName.Value = GetProductName(dt.Rows[0]["Product_Id"].ToString());
            hdnSOID.Value = dt.Rows[0]["SOID"].ToString();
            hdnSONO.Value = dt.Rows[0]["SONO"].ToString();
        }
        ViewState["Dtproduct"] = (DataTable)dt;
        return dt;
    }
    protected void imgBtnProductDelete_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductChidGird("Del");
    }
    public void FillProductChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillProductDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillProductDataTableUpdate();
        }
        else
        {
            dt = FillProductDataTabel();
        }
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProduct, dt, "", "");
        ResetProduct();
        dt = null;
        //AllPageCode();
    }
    public DataTable FillProductDataTableUpdate()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            Label lblgvReqQty = (Label)GvProduct.Rows[i].FindControl("lblgvRequiredQty");
            Literal lblgvPDescription = (Literal)GvProduct.Rows[i].FindControl("lblgvProductDescription");
            HiddenField hdnSuggestedProductName = (HiddenField)GvProduct.Rows[i].FindControl("hdnSuggestedProductName");
            Label gvlblSONo = (Label)GvProduct.Rows[i].FindControl("gvlblSONo");
            HiddenField gvhdnSOID = (HiddenField)GvProduct.Rows[i].FindControl("gvhdnSOID");

            dt.Rows[i]["Serial_No"] = lblSNo.Text;
            dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
            dt.Rows[i]["UnitId"] = lblgvUnitId.Text;
            dt.Rows[i]["ReqQty"] = lblgvReqQty.Text;
            dt.Rows[i]["ProductDescription"] = lblgvPDescription.Text;
            dt.Rows[i]["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows[i]["SONO"] = gvlblSONo.Text;
            dt.Rows[i]["SOID"] = gvhdnSOID.Value;

        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnProductId.Value == dt.Rows[i]["Serial_No"].ToString())
            {
                dt.Rows[i]["Product_Id"] = hdnNewProductId.Value;
                dt.Rows[i]["UnitId"] = hdnNewUnitId.Value;
                dt.Rows[i]["ReqQty"] = txtRequestQty.Text;
                if (hdnNewProductId.Value == "0")
                {
                    dt.Rows[i]["ProductDescription"] = txtPDesc.Text;
                    dt.Rows[i]["SuggestedProductName"] = txtProductName.Text;
                }
                else
                {
                    dt.Rows[i]["ProductDescription"] = txtPDescription.Text;
                    dt.Rows[i]["SuggestedProductName"] = "";
                }
                dt.Rows[i]["SONO"] = hdnSONO.Value;
                dt.Rows[i]["SOID"] = hdnSOID.Value;
            }
        }
        ViewState["Dtproduct"] = (DataTable)dt;
        return dt;
    }
    protected string GetProductName(string strProductId)
    {
        string strProductName = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductName = dtPName.Rows[0]["EProductName"].ToString();
            }
            dtPName = null;
        }
        else
        {
            strProductName = "";
        }
        return strProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    protected string SuggestedProductName(string ProductId, string TransId)
    {
        string HeaderId = string.Empty;
        if (hdnTransNo.Value != "" && hdnTransNo.Value != "0")
        {
            HeaderId = hdnTransNo.Value;
        }
        if (editid.Value != "" && editid.Value != "0")
        {
            HeaderId = editid.Value;
        }
        string ProductName = string.Empty;
        DataTable dt = new DataTable();
        try
        {
            dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
            if (HeaderId != "")
            {
                DataTable DtPurchaseDetail = new DataTable();
                if (hdnTransNo.Value != "" && hdnTransNo.Value != "0")
                {
                    if (ViewState["InquiryType"] != null && ViewState["InquiryType"].ToString().Trim() != "SI")
                    {
                        DtPurchaseDetail = ObjPRDetail.GetPurchaseRequestDetailbyRequestId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnTransNo.Value);
                        try
                        {
                            DtPurchaseDetail = new DataView(DtPurchaseDetail, "Serial_No=" + TransId + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        try
                        {
                            ProductName = DtPurchaseDetail.Rows[0]["SuggestedProductName"].ToString();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        DtPurchaseDetail = ObjSalesInquiryDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnTransNo.Value, Session["FinanceYearId"].ToString());
                        try
                        {
                            DtPurchaseDetail = new DataView(DtPurchaseDetail, "Serial_No=" + TransId + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        try
                        {
                            ProductName = DtPurchaseDetail.Rows[0]["SuggestedProductName"].ToString();
                        }
                        catch
                        {
                        }
                    }
                    if (ProductName.Trim() == "")
                    {
                        try
                        {
                            DataTable DtProduct = (DataTable)ViewState["Dtproduct"];
                            ProductName = (new DataView(DtProduct, "Serial_No=" + TransId, "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["SuggestedProductName"].ToString();
                            DtProduct = null;
                        }
                        catch
                        { }
                    }
                }
                if (editid.Value != "" && editid.Value != "0")
                {
                    DataTable DtPurchaseInquiryDetail = new DataTable();
                    DtPurchaseInquiryDetail = ObjPIDetail.GetPIDetailByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
                    try
                    {
                        DtPurchaseInquiryDetail = new DataView(DtPurchaseInquiryDetail, "Serial_No=" + TransId + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    try
                    {
                        ProductName = DtPurchaseInquiryDetail.Rows[0]["SuggestedProductName"].ToString();
                    }
                    catch
                    {
                    }
                    DtPurchaseInquiryDetail = null;
                    if (ProductName.Trim() == "")
                    {
                        try
                        {
                            DataTable DtProduct = (DataTable)ViewState["Dtproduct"];
                            ProductName = (new DataView(DtProduct, "Serial_No=" + TransId, "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["SuggestedProductName"].ToString();
                            DtProduct = null;
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                }
                DtPurchaseDetail = null;

            }
            else
            {
                // ProductName = ProductId;
                try
                {
                    DataTable DtProduct = (DataTable)ViewState["Dtproduct"];
                    ProductName = (new DataView(DtProduct, "Serial_No=" + TransId, "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["SuggestedProductName"].ToString();
                }
                catch { }
            }
        }
        dt = null;
        return ProductName;
    }
    protected string GetUnitName(string strUnitId)
    {
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
            if (dtUName.Rows.Count > 0)
            {
                strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
            }
            dtUName = null;
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }
    #endregion
    #region Add Request Section
    protected void btnbindRequest_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
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
            Session["dtPRequestFilter"] = view.ToTable();
            lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseRequest, view.ToTable(), "", "");
            //AllPageCode();
            // btnRefreshRequest.Focus();
            dtPurchaseRequest = null;
            view = null;
        }
        if (txtValueRequest.Text != "")
            txtValueRequest.Focus();
        else if (txtValueRequestDate.Text != "")
            txtValueRequestDate.Focus();
    }
    protected void btnRefreshRequest_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ddlFieldNameRequest.SelectedIndex = 0;
        ddlOptionRequest.SelectedIndex = 2;
        txtValueRequest.Text = "";
        txtValueRequest.Visible = true;
        txtValueRequestDate.Visible = false;
        txtValueRequestDate.Text = "";
        txtValueRequest.Focus();
        FillRequestGrid();
    }
    protected void GvPurchaseRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseRequest.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPRequestFilter"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseRequest, dt, "", "");
        dt = null;
        //AllPageCode();
    }
    protected void GvPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPRequestFilter"];
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
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseRequest, dt, "", "");
        dt = null;
        //AllPageCode();
    }
    private void FillRequestGrid()
    {
        DataTable dtPRequest = null;
        string PurchaseRequestApproval = objInvParameter.getParameterValueByParameterName("PurchaseRequestApproval", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        if (PurchaseRequestApproval != "")
        {
            if (Convert.ToBoolean(PurchaseRequestApproval) == true)
            {
                dtPRequest = objPRHeader.GetPurchaseRequestAndInquiryData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            }
            else
            {
                dtPRequest = objPRHeader.GetPurchaseRequestAndInquiryWithoutApproval(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            }
            //here we add validtion for allow only open requet and not reject request 
        }
        DataTable dtSI = new DataView(ObjSelesInquiryHeader.GetSIHeaderAll(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString()), "Field1='FWInPur'", "", DataViewRowState.CurrentRows).ToTable();
        dtPRequest.Columns.Add("RequestType");
        dtPRequest.Columns.Add("TenderNo");
        dtPRequest.Columns.Add("OrderCompletionDate");
        dtPRequest.Columns.Add("Name");
        //  dtPRequest.Columns.Add("CreatedBy");
        for (int i = 0; i < dtSI.Rows.Count; i++)
        {
            dtPRequest.Rows.Add();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["CreatedBy"] = dtSI.Rows[i]["CreatedBy"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["Trans_Id"] = dtSI.Rows[i]["SInquiryID"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["RequestNo"] = dtSI.Rows[i]["SInquiryNo"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["RequestDate"] = dtSI.Rows[i]["IDate"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["RequestType"] = "Sales Inquiry";
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["TenderNo"] = dtSI.Rows[i]["TenderNo"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["OrderCompletionDate"] = dtSI.Rows[i]["OrderCompletionDate"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["DepartmentName"] = dtSI.Rows[i]["DepartmentName"].ToString();
            dtPRequest.Rows[dtPRequest.Rows.Count - 1]["Name"] = dtSI.Rows[i]["Name"].ToString();
        }
        for (int i = 0; i < dtPRequest.Rows.Count; i++)
        {
            if (dtPRequest.Rows[i]["RequestType"].ToString() == "")
            {
                dtPRequest.Rows[i]["RequestType"] = "Purchase Request";
            }
        }
        Session["dtPRequest"] = dtPRequest;
        Session["dtPRequestFilter"] = dtPRequest;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseRequest, dtPRequest, "", "");
        Lbl_Request.Text = Resources.Attendance.Request;
        lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + ": " + dtPRequest.Rows.Count.ToString() + "";
        dtPRequest = null;
        dtSI = null;
        //AllPageCode();
    }
    protected void btnPREdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = "";
        Next_Div.Visible = false;
        Main_Div.Visible = true;
        Reset();
        tabNewClick();
        Button Imgbtn = (Button)sender;
        GridViewRow row = (GridViewRow)Imgbtn.NamingContainer;
        Label li = (Label)row.FindControl("lblgvRequestType");
        DataTable dtPRequest = new DataTable();
        string strRequestId = e.CommandArgument.ToString();
        editid.Value = "";
        Session["DtSearchProduct"] = null;
        rbtnFormView.Checked = true;
        rbtnAdvancesearchView.Checked = false;
        rbtnFormView_OnCheckedChanged(null, null);
        ViewState["Dtproduct"] = null;
        if (li.Text.Trim() == "Sales Inquiry")
        {
            dtPRequest = ObjSelesInquiryHeader.GetSIHeaderAllBySInquiryId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), strRequestId);
            pnlTrans.Visible = true;
            txtDepartment.Visible = true;
            lblDepartment.Visible = true;
            txtTransFrom.Text = Resources.Attendance.Sales_Inquiry;
            txtTransNo.Text = dtPRequest.Rows[0]["SInquiryNo"].ToString();
            txtDepartment.Text = dtPRequest.Rows[0]["DepartmentName"].ToString();
            ViewState["InquiryType"] = "SI";
            hdnTransNo.Value = dtPRequest.Rows[0]["SInquiryId"].ToString();
            DataTable dtRequestProduct = ObjSalesInquiryDetail.GetSIDetailBySInquiryId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRequestId, Session["FinanceYearId"].ToString());
            dtRequestProduct.Columns["Quantity"].ColumnName = "ReqQty";
            if (dtRequestProduct.Rows.Count > 0)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, dtRequestProduct, "", "");
            }
            dtRequestProduct = null;
        }
        else
        {
            ViewState["InquiryType"] = "PI";
            dtPRequest = objPRHeader.GetPurchaseRequestTrueAllByReqId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRequestId);
            pnlTrans.Visible = true;
            txtDepartment.Visible = true;
            lblDepartment.Visible = true;
            lblEmployeeName.Visible = true;
            txtEmployeeName.Visible = true;
            txtTransFrom.Text = Resources.Attendance.Purchase_Request;
            txtTransNo.Text = dtPRequest.Rows[0]["RequestNo"].ToString();
            hdnTransNo.Value = dtPRequest.Rows[0]["Trans_Id"].ToString();
            txtDepartment.Text = dtPRequest.Rows[0]["DepartmentName"].ToString();
            txtEmployeeName.Text = dtPRequest.Rows[0]["CreatedBy"].ToString();
            DataTable dtRequestProduct = ObjPRDetail.GetPurchaseRequestDetailbyRequestId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strRequestId);
            if (dtRequestProduct.Rows.Count > 0)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, dtRequestProduct, "", "");
            }
            dtRequestProduct = null;
        }
        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Request_Active()", true);
    }
    protected void IbtnUpdateRequestStatus_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.Trim() == "Purchase Request")
        {
            objPRHeader.UpdatePurchaseRequestStatusbyPI(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString(), "Reject");
            DisplayMessage("Purchase request rejected");
            FillRequestGrid();
            //AllPageCode();
        }
        else
        {
            DisplayMessage("You can reject only purchase request");
            return;
        }
    }
    #endregion
    #region Add Supplier
    public void FillSupplier()
    {
        DataTable dt = ObjPISupplier.GetAllSupplier(Session["CompId"].ToString(), Session["BrandId"].ToString());
        try
        {
            dt = new DataView(dt, "", "Name Asc", DataViewRowState.CurrentRows).ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ChkSupplier, dt, "Name", "Trans_Id");
            dt = null;
        }
        catch
        {
        }
    }
    private void FillSupplierGroup()
    {
        DataTable dsSupplierG = null;
        dsSupplierG = objGroupMaster.GetGroupMasterByParentId("2");
        if (dsSupplierG.Rows.Count > 0)
        {
            try
            {
                ddlSupplierGroup.DataSource = dsSupplierG;
                ddlSupplierGroup.DataTextField = "Group_Name";
                ddlSupplierGroup.DataValueField = "Group_Id";
                ddlSupplierGroup.DataBind();
                ddlSupplierGroup.Items.Insert(0, "--Select--");
            }
            catch
            {
            }
        }
        else
        {
            ddlSupplierGroup.Items.Clear();
            ddlSupplierGroup.Items.Insert(0, "--Select--");
        }
        dsSupplierG = null;
    }
    protected void ddlSupplierGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridProductSupplierCode.DataSource = null;
        GridProductSupplierCode.DataBind();

        if (ddlSupplierGroup.SelectedIndex > 0)
        {
            DataTable dtSG = ObjPISupplier.GetAllSupplierByGroupId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlSupplierGroup.SelectedValue);
            if (dtSG.Rows.Count > 0)
            {
                Session["SupGroupId"] = ddlSupplierGroup.SelectedValue;
            }
            else
            {
                Session["SupGroupId"] = null;
                //ChkSupplier.DataSource = null;
                //ChkSupplier.DataBind();
            }

            DataTable dt = (DataTable)Session["dtInquirySupplier"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Supplier_Id");
                dt.Columns.Add("Name");
            }


            foreach (DataRow dr in dtSG.Rows)
            {
                dt.Rows.Add(dr["Trans_id"].ToString(), dr["Name"].ToString());
            }

            Session["dtInquirySupplier"] = dt;
            objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
        }
        else if (ddlSupplierGroup.SelectedValue == "All Supplier")
        {
            Session["SupGroupId"] = null;
            //FillSupplier();
        }
    }
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/PurchaseInquiryPrint.aspx?RId=" + e.CommandArgument.ToString() + "','window','width=1024');", true);
    }
    protected void btnSaveSupplier_Click(object sender, EventArgs e)
    {
        int b = 0;
        string strSupplierGroup = string.Empty;
        if (ddlSupplierGroup.SelectedValue != "--Select--")
        {
            if (ddlSupplierGroup.SelectedValue != "All Supplier")
            {
                strSupplierGroup = ddlSupplierGroup.SelectedValue;
            }
            else if (ddlSupplierGroup.SelectedValue == "All Supplier")
            {
                strSupplierGroup = "2";
            }
        }
        if (editid.Value != "0" && editid.Value != "")
        {
            ObjPISupplier.DeletePurchaseInquirySupplier(Session["CompId"].ToString(), editid.Value);
            foreach (GridViewRow gvr in GridProductSupplierCode.Rows)
            {
                if (((CheckBox)gvr.FindControl("chkSupplier")).Checked == true)
                {
                    b = ObjPISupplier.InsertPurchaseInquirySupplier(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ((Label)gvr.FindControl("lblgvsupId")).Text, strSupplierGroup, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }
        Session["dtInquirySupplier"] = null;
        GridProductSupplierCode.DataSource = null;
        GridProductSupplierCode.DataBind();
        FillGrid();
        FillSupplier();
        FillSupplierGroup();
        string R_Id = string.Empty;
        R_Id = txtSInquiryNo.Text + editid.Value;
        txtSInquiryNo.Text = "";
        txtSInquiryDate.Text = "";
        DisplayMessage("Record Saved", "green");
        Next_Div.Visible = false;
        Main_Div.Visible = true;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/PurchaseInquiryPrint.aspx?RId=" + editid.Value.ToString() + "','window','width=1024');", true);
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void CheckedandDisableSupplier()
    {
        if (editid.Value != "")
        {
            DataTable dtSup = ObjPISupplier.GetAllPISupplierWithPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            if (dtSup.Rows.Count > 0)
            {
                dtSup = dtSup.DefaultView.ToTable(true, "Supplier_Id", "Name");
                Session["dtInquirySupplier"] = dtSup;
                objPageCmn.FillData((object)GridProductSupplierCode, dtSup, "", "");
            }
            foreach (GridViewRow gvrow in GridProductSupplierCode.Rows)
            {
                DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                try
                {
                    DtquotHeader = new DataView(DtquotHeader, "PI_No=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
                if (DtquotHeader.Rows.Count > 0)
                {
                    DataTable dtpQuotationdetail = objPQDetail.GetQuoteDetailAllData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    try
                    {
                        dtpQuotationdetail = new DataView(dtpQuotationdetail, "RPQ_No=" + DtquotHeader.Rows[0]["Trans_Id"].ToString() + " and Supplier_Id=" + ((Label)gvrow.FindControl("lblgvsupId")).Text + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtpQuotationdetail.Rows.Count > 0)
                    {
                        ((LinkButton)gvrow.FindControl("IbtnDeleteSupplier")).Enabled = false;
                    }
                    else
                    {
                        ((LinkButton)gvrow.FindControl("IbtnDeleteSupplier")).Enabled = true;
                    }
                }
            }
            dtSup = null;
        }
    }
    protected void btnResetSupplier_Click(object sender, EventArgs e)
    {
        FillSupplier();
        FillSupplierGroup();
        CheckedandDisableSupplier();
    }
    protected void btnCancelSupplier_Click(object sender, EventArgs e)
    {
        editid.Value = "";
        Next_Div.Visible = false;
        Main_Div.Visible = true;
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    #endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        if (Lbl_Tab_New.Text != Resources.Attendance.View)
        {
            btnPISave.Visible = clsPagePermission.bAdd;
            btnProductSave.Visible = clsPagePermission.bAdd;
            btnSaveSupplier.Visible = clsPagePermission.bAdd;
        }
        else
        {
            btnPISave.Visible = false;
            btnProductSave.Visible = false;
            btnSaveSupplier.Visible = false;
        }
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();

        GvProduct.Columns[0].Visible = clsPagePermission.bEdit;
        GvProduct.Columns[1].Visible = clsPagePermission.bDelete;
    }
    #endregion
    protected void GvPurchaseInquiry_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    #region View

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
            dtSName = null;
        }
        else
        {
            strSupplierName = "";
        }
        return strSupplierName;
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    #endregion
    #region supplierPricelist
    protected void BtnShowCpriceList_click(object sender, EventArgs e)
    {
        string IsSupplierChecked = string.Empty;
        string SupplierId = string.Empty;
        foreach (GridViewRow Gvrow in GridProductSupplierCode.Rows)
        {
            if (((CheckBox)Gvrow.FindControl("chkSupplier")).Checked)
            {
                IsSupplierChecked = "0";
                if (SupplierId == "")
                {
                    SupplierId = ((Label)Gvrow.FindControl("lblgvsupId")).Text;
                }
                else
                {
                    SupplierId = SupplierId + "," + ((Label)Gvrow.FindControl("lblgvsupId")).Text;
                }
            }
        }
        if (IsSupplierChecked == "")
        {
            DisplayMessage("Select At Least One supplier");
            ChkSupplier.Focus();
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/Compare_SupplierPriceList.aspx?SupplierId=" + SupplierId + "');", true);
        }
    }
    #endregion
    public void FillCurrency()
    {
        try
        {
            DataTable dt = ObjCurrencyMaster.GetCurrencyListForDDL();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            dt = null;
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;
        }
        if (ddlCurrency.Items.Count > 0)
        {
            try
            {
                ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
            }
            catch
            {
            }
        }
    }
    public void EnableDisableControls(Boolean st)
    {
        if (st)
        {
            btnPISave.Visible = true;
            BtnReset.Visible = true;
            //Supplier Section
            BtnShowCpriceList.Visible = true;
            ChkSupplier.Enabled = true;
            btnSaveSupplier.Visible = true;
            btnResetSupplier.Visible = true;
            Next_Div.Visible = true;
            Main_Div.Visible = false;
            ddlSupplierGroup.Visible = true;
            lblSupplierGroup.Visible = true;
        }
        else
        {
            btnPISave.Visible = false;
            BtnReset.Visible = false;
            btnCancelSupplier.Visible = true;
            //Supplier Section
            txtSInquiryDate.Visible = false;
            txtSInquiryNo.Visible = false;
            lblSInquiryDate.Visible = false;
            lblSInquiryNo.Visible = false;
            lblSIdtColon.Visible = false;
            lblSuppNoColon.Visible = false;
            DataTable dtSup = ObjPISupplier.GetAllPISupplierWithPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            try
            {
                ChkSupplier.Items.Clear();
                for (int i = 0; i < dtSup.Rows.Count; i++)
                {
                    ChkSupplier.Items.Add(GetSupplierName(dtSup.Rows[i]["Supplier_Id"].ToString()));
                    ChkSupplier.Items[i].Selected = true;
                }
            }
            catch
            {
            }
            dtSup = null;
            lblSupplierGroup.Visible = false;
            ddlSupplierGroup.Visible = false;
            BtnShowCpriceList.Visible = false;
            btnSaveSupplier.Visible = false;
            btnResetSupplier.Visible = false;
            Next_Div.Visible = true;
            Main_Div.Visible = false;
        }
    }
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "PIDate")
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
        if (ddlFieldNameBin.SelectedItem.Value == "PIDate")
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
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
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
    #region Advance Search
    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvr in GvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
            Label lblProductId = (Label)gvr.FindControl("lblgvProductId");
            Label lblUnitId = (Label)gvr.FindControl("lblgvUnitId");
            Label lblReqQty = (Label)gvr.FindControl("lblgvRequiredQty");
            Literal lblProductDescription = (Literal)gvr.FindControl("lblgvProductDescription");
            HiddenField hdnSuggestedProductName = (HiddenField)gvr.FindControl("hdnSuggestedProductName");
            dr["Serial_No"] = lblSerialNo.Text;
            dr["Product_Id"] = lblProductId.Text;
            dr["UnitId"] = lblUnitId.Text;
            dr["ProductDescription"] = lblProductDescription.Text;
            dr["ReqQty"] = lblReqQty.Text;
            dr["SuggestedProductName"] = hdnSuggestedProductName.Value;
            dt.Rows.Add(dr);
        }
        ViewState["Dtproduct"] = dt;
        Session["DtSearchProduct"] = ViewState["Dtproduct"];
        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=PI','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        dt = null;
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        if (Session["DtSearchProduct"] != null)
        {
            ViewState["Dtproduct"] = Session["DtSearchProduct"];
            if (ViewState["Dtproduct"] != null)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, (DataTable)ViewState["Dtproduct"], "", "");
            }
            Session["DtSearchProduct"] = null;
            //AllPageCode();
        }
        else
        {
            if (ViewState["Dtproduct"] != null)
            {
                //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvProduct, (DataTable)ViewState["Dtproduct"], "", "");
            }
            //AllPageCode();
            DisplayMessage("Product Not Found");
            return;
        }
    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        div_salesOrder.Visible = false;
        btnAddNewProduct.Visible = false;
        btnAddProductScreen.Visible = false;
        btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        pnlProduct1.Visible = false;

        if (rbtnFormView.Checked == true)
        {
            btnAddNewProduct.Visible = true;
            btnAddNewProduct_Click(null, null);
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
        }
        if (rbtnSalesOrder.Checked == true)
        {
            btnGetDataFromSalesOrder_Click();
            div_salesOrder.Visible = true;
        }
    }
    #endregion
    #region SupplierList
    protected void txtSuppliers_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtInquirySupplier"];
        if (dt != null)
        {
            if (txtSuppliers.Text != "")
            {
                try
                {
                    string strSupplierId = "";
                    strSupplierId = (txtSuppliers.Text.Split('/'))[txtSuppliers.Text.Split('/').Length - 1];
                    string query = "Supplier_Id = '" + strSupplierId + "'";
                    dt = new DataView(dt, query, "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        DisplayMessage("Supplier Name Already Exists");
                        txtSuppliers.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                    }
                    else
                    {
                        DataTable dt1 = ObjPISupplier.GetAllSupplier(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString());
                        dt1 = new DataView(dt1, "Trans_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            DisplayMessage("Invalid Supplier Name");
                            txtSuppliers.Text = "";
                            txtSuppliers.Focus();
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                        }
                    }
                }
                catch
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSuppliers.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                }
            }
        }
        else
        {
            if (txtSuppliers.Text != "")
            {
                string strSupplierId = "";
                strSupplierId = (txtSuppliers.Text.Split('/'))[txtSuppliers.Text.Split('/').Length - 1];
                DataTable dt1 = ObjPISupplier.GetAllSupplier(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString());
                try
                {
                    dt1 = new DataView(dt1, "Trans_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt1.Rows.Count > 0)
                {
                }
                else
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSuppliers.Focus();
                    txtSuppliers.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                }
            }
        }
        dt = null;
    }
    protected void GridProductSupplierCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridProductSupplierCode.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtInquirySupplier"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
        dt = null;
    }
    protected void btnGetRecommendedsupplier_Click(object sender, EventArgs e)
    {
        int count = 0;
        DataTable dt = (DataTable)Session["dtInquirySupplier"];
        if (dt == null)
        {
            dt = new DataTable();
            dt.Columns.Add("Supplier_Id");
            dt.Columns.Add("Name");
        }
        DataTable dtDetail = ObjPIDetail.GetPIDetailByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
        foreach (DataRow dr in dtDetail.Rows)
        {
            //first get from product master 
            //if record not found then get old supplier according last sent for selected prodcut 
            DataTable dtSupplier = ObjProductSupplier.GetProductSuppliersByProductId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), dr["Product_id"].ToString());
            if (dtSupplier.Rows.Count > 0)
            {
                for (int i = 0; i < dtSupplier.Rows.Count; i++)
                {
                    if ((DataTable)Session["dtInquirySupplier"] != null)
                    {
                        if (new DataView((DataTable)Session["dtInquirySupplier"], "Supplier_Id=" + dtSupplier.Rows[i]["Supplier_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            dt.Rows.Add(dtSupplier.Rows[i]["Supplier_Id"].ToString(), dtSupplier.Rows[i]["Name"].ToString());
                            Session["dtInquirySupplier"] = dt;
                            count++;
                        }
                    }
                    else
                    {
                        dt.Rows.Add(dtSupplier.Rows[i]["Supplier_Id"].ToString(), dtSupplier.Rows[i]["Name"].ToString());
                        Session["dtInquirySupplier"] = dt;
                        count++;
                    }
                }
            }
            dtSupplier = null;
            //else
            //{
            DataTable dtPiList = ObjPIDetail.GetPIDetailByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["Product_id"].ToString());
            for (int i = 0; i < dtPiList.Rows.Count; i++)
            {
                DataTable dtSup = ObjPISupplier.GetAllPISupplierWithPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPiList.Rows[i]["PI_No"].ToString());
                for (int j = 0; j < dtSup.Rows.Count; j++)
                {
                    if ((DataTable)Session["dtInquirySupplier"] != null)
                    {
                        if (new DataView((DataTable)Session["dtInquirySupplier"], "Supplier_Id=" + dtSup.Rows[j]["Supplier_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            dt.Rows.Add(dtSup.Rows[j]["Supplier_Id"].ToString(), dtSup.Rows[j]["Name"].ToString());
                            Session["dtInquirySupplier"] = dt;
                            count++;
                        }
                    }
                    else
                    {
                        dt.Rows.Add(dtSup.Rows[j]["Supplier_Id"].ToString(), dtSup.Rows[j]["Name"].ToString());
                        Session["dtInquirySupplier"] = dt;
                        count++;
                    }
                }
                dtSup = null;
            }
            dtPiList = null;
            //}
        }
        objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
        DisplayMessage(count + " Record found ");
        dt = null;
        dtDetail = null;
    }
    protected void IbtnAddProductSupplierCode_Click(object sender, EventArgs e)
    {
        if (txtSuppliers.Text != "")
        {
            DataTable dt = (DataTable)Session["dtInquirySupplier"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Supplier_Id");
                dt.Columns.Add("Name");
            }
            string strSupplierId = "";
            string strSupplierName = "";
            if (txtSuppliers.Text != "")
            {
                strSupplierId = (txtSuppliers.Text.Split('/'))[txtSuppliers.Text.Split('/').Length - 1];
                strSupplierName = txtSuppliers.Text.Split('/')[0];
            }
            dt.Rows.Add(strSupplierId, strSupplierName);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
            Session["dtInquirySupplier"] = dt;
            txtSuppliers.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
            foreach (GridViewRow gvrow in GridProductSupplierCode.Rows)
            {
                DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                try
                {
                    DtquotHeader = new DataView(DtquotHeader, "PI_No=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
                if (DtquotHeader.Rows.Count > 0)
                {
                    DataTable dtpQuotationdetail = objPQDetail.GetQuoteDetailAllData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    try
                    {
                        dtpQuotationdetail = new DataView(dtpQuotationdetail, "RPQ_No=" + DtquotHeader.Rows[0]["Trans_Id"].ToString() + " and Supplier_Id=" + ((Label)gvrow.FindControl("lblgvsupId")).Text + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtpQuotationdetail.Rows.Count > 0)
                    {
                        ((LinkButton)gvrow.FindControl("IbtnDeleteSupplier")).Enabled = false;
                    }
                    else
                    {
                        ((LinkButton)gvrow.FindControl("IbtnDeleteSupplier")).Enabled = true;
                    }
                    dtpQuotationdetail = null;
                }
                DtquotHeader = null;
            }
            dt = null;
        }
        else
        {
            // DisplayMessage("Please Select Supplier First");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
        }
    }
    protected void IbtnDeleteSupplier_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtInquirySupplier"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                var rows = dt.Select("Supplier_Id ='" + e.CommandArgument.ToString() + "'");
                foreach (var row in rows)
                    row.Delete();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GridProductSupplierCode, dt, "", "");
                Session["dtInquirySupplier"] = dt;
                foreach (GridViewRow gvrow in GridProductSupplierCode.Rows)
                {
                    DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    try
                    {
                        DtquotHeader = new DataView(DtquotHeader, "PI_No=" + editid.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    //DataTable DtquotHeader = ObjPQuotheader.GetQuoteHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
                    if (DtquotHeader.Rows.Count > 0)
                    {
                        DataTable dtpQuotationdetail = objPQDetail.GetQuoteDetailAllData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                        try
                        {
                            dtpQuotationdetail = new DataView(dtpQuotationdetail, "RPQ_No=" + DtquotHeader.Rows[0]["Trans_Id"].ToString() + " and Supplier_Id=" + ((Label)gvrow.FindControl("lblgvsupId")).Text + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        if (dtpQuotationdetail.Rows.Count > 0)
                        {
                            ((LinkButton)gvrow.FindControl("IbtnDeleteSupplier")).Enabled = false;
                        }
                        else
                        {
                            ((LinkButton)gvrow.FindControl("IbtnDeleteSupplier")).Enabled = true;
                        }
                        dtpQuotationdetail = null;
                    }
                    DtquotHeader = null;
                }
            }
        }
        dt = null;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Inv_PurchaseInquiry_Supplier ObjSupplier = new Inv_PurchaseInquiry_Supplier(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = new DataTable();
        if (HttpContext.Current.Session["SupGroupId"] == null)
        {
            dtSupplier = ObjSupplier.GetAllSupplier(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        }
        else
        {
            dtSupplier = ObjSupplier.GetAllSupplierByGroupId(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["SupGroupId"].ToString());
        }
        string filtertext = "Name like '%" + prefixText + "%' and IsActive='True'";
        DataTable dtCon = new DataView(dtSupplier, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        dtSupplier = null;
        dtCon = null;
        return filterlist;
    }
    #endregion

    public void tabNewClick()
    {
        if (Lbl_Tab_New.Text == "View")
        {
            btnPISave.Enabled = false;
            BtnReset.Visible = false;
            btnSaveSupplier.Enabled = false;
        }
        else
        {
            btnPISave.Enabled = true;
            BtnReset.Visible = true;
            btnSaveSupplier.Enabled = true;
        }
        txtPIDate.Focus();
    }

    protected void btnFillUnits_Click(object sender, EventArgs e)
    {
        txtPDescription.Text = hdnProductDesc.Value;
        FillUnit(hdnNewProductId.Value);
    }

    protected void lnkPage_Click(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvPurchaseInquiryCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void btnGetDataFromSalesOrder_Click()
    {
        DataTable dt = ObjSalesOrderDetail.getSODetailDataForPurchaseOrder(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());
        gvPendingSalesOrder.DataSource = dt;
        gvPendingSalesOrder.DataBind();
    }
    protected void chkAddSOToPO_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lblOrderNo = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblOrderNo");
        HiddenField trans_id = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvOrderId");
        Label lblProductCode = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblProductCode");
        HiddenField gvhdnProductId = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvhdnProductId");
        Label lblUnit = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblUnit");
        HiddenField gvhdnUnitId = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvhdnUnitId");
        HiddenField gvHdnUnitCost = (HiddenField)gvPendingSalesOrder.Rows[index].FindControl("gvHdnUnitCost");
        Label lblProductName = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblProductName");
        Label lblQuantity = (Label)gvPendingSalesOrder.Rows[index].FindControl("lblQuantity");

        hdnNewProductId.Value = gvhdnProductId.Value;

        hdnNewUnitId.Value = gvhdnUnitId.Value;

        txtRequestQty.Text = lblQuantity.Text;

        txtPDesc.Text = "";
        txtProductName.Text = lblProductName.Text;
        hdnSOID.Value = trans_id.Value;
        hdnSONO.Value = lblOrderNo.Text;
        DataTable dt = FillProductDataTabel();
        GvProduct.DataSource = dt;
        GvProduct.DataBind();
        ResetProduct();
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
}