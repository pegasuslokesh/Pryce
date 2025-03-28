using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Purchase_PurchaseReturn : BasePage
{
    #region defind Class Object
    Common cmn = null;
    DataAccessClass da = null;
    //Set_Suppliers objSupplier = new Set_Suppliers();
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Inv_PurchaseReturnHeader objPReturnHeader = null;
    Inv_PurchaseReturnDetail objPReturnDetail = null;
    PurchaseInvoice objPInvoice = null;
    PurchaseInvoiceDetail objPInvoiceD = null;
    Ems_ContactMaster objContact = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    SystemParameter ObjSysParam = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductLadger = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseInquiryHeader ObjPIHeader = null;
    Inv_ParameterMaster objInvParam = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_Ageing_Detail objAgeingDetail = null;
    LocationMaster ObjLocation = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Set_Payment_Mode_Master ObjPaymentMaster = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    Inv_TaxRefDetail objTaxRefDetail = null;
    Ac_AccountMaster objAcAccountMaster = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strDepartmentId = string.Empty;

    #endregion
    public static int Decimal_Count_For_Tax;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        //Set_Suppliers objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objPReturnHeader = new Inv_PurchaseReturnHeader(Session["DBConnection"].ToString());
        objPReturnDetail = new Inv_PurchaseReturnDetail(Session["DBConnection"].ToString());
        objPInvoice = new PurchaseInvoice(Session["DBConnection"].ToString());
        objPInvoiceD = new PurchaseInvoiceDetail(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjPaymentMaster = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objAcAccountMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        //Take Current Page in Session["Page"] 
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        strDepartmentId = Session["DepartmentId"].ToString();

        //here we are checking for physical inventory
        //code start
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Don't transaction ,Stock Work is going On... ");
            Response.Redirect("../MasterSetup/Home.aspx");
        }
        //code end
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Purchase/PurchaseReturn.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);

            string Decimal_Count = string.Empty;
            txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
            ddlOption.SelectedIndex = 2;
            FillGrid();
            FillPaymentMode();
            txtPInvoiceNo.Visible = true;
            txtReturnNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtReturnNo.Text;
            Calender.Format = Session["DateFormat"].ToString();
            txtReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            ViewState["dtSerial"] = null;
            ViewState["PId"] = null;
            ViewState["dtFinal"] = null;
            ViewState["Post"] = null;
            CalendarExtender_txtInvoiceValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            if (Request.QueryString["Id"] != null)
            {
                //ImageButton imgeditbutton = new ImageButton();
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";

                btnEdit_Command(imgeditbutton, new CommandEventArgs(Session["LocId"].ToString(), Request.QueryString["Id"].ToString()));
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_LI()", true);
                strLocationId = Request.QueryString["LocId"].ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_LI()", true);
            }
            FillCurrency(ddlCurrency);
            TaxandDiscountParameter();
        }

    }
    private void FillPaymentMode()
    {
        DataTable dsPaymentMode = null;
        dsPaymentMode = objPaymentMode.GetPaymentModeMaster(StrCompId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        try
        {
            dsPaymentMode = new DataView(dsPaymentMode, "Pay_Mod_Name in ('Cash','Credit')", "Pay_Mode_Id asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        if (dsPaymentMode.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            ddlInvoiceType.DataSource = dsPaymentMode;
            ddlInvoiceType.DataTextField = "Pay_Mod_Name";
            ddlInvoiceType.DataValueField = "Pay_Mod_Name";
            ddlInvoiceType.DataBind();
            ddlInvoiceType.SelectedIndex = 0;
        }
        else
        {
            ddlInvoiceType.Items.Insert(0, "--Select--");
            ddlInvoiceType.SelectedIndex = 0;
        }
        dsPaymentMode = null;
        fillTabPaymentMode(ddlInvoiceType.SelectedItem.Text);
    }
    protected void ddlInvoiceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillTabPaymentMode(ddlInvoiceType.SelectedItem.Text);
    }
    public void fillTabPaymentMode(string PaymentType)
    {
        DataTable dt = ObjPaymentMaster.GetPaymentModeMaster(StrCompId.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        dt = new DataView(dt, "Field1='" + PaymentType.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)ddlPaymentMode, dt, "Pay_Mod_Name", "Pay_Mode_Id");
        dt = null;

    }
    public void FillCurrency(DropDownList ddlCurrency)
    {
        try
        {
            using (DataTable dt = ObjCurrencyMaster.GetCurrencyMaster())
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            }

        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");

        }

    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    void TaxandDiscountParameter()
    {
        GvInvoiceDetail.Columns[8].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        Div_Tax.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        GvInvoiceDetail.Columns[9].Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
    }
    #region System defined Function
    protected void GvInvoiceDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((HiddenField)e.Row.FindControl("hdnIProductId")).Value;
            if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
            {
                ((Button)e.Row.FindControl("Btn_Add_Return")).Visible = true;
                ((TextBox)e.Row.FindControl("txtReturnQty")).Enabled = false;
            }
            else
            {
                ((Button)e.Row.FindControl("Btn_Add_Return")).Visible = false;
                ((TextBox)e.Row.FindControl("txtReturnQty")).Enabled = true;
            }

        }
    }    
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }
        bool IsNonRegistered = false;
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        using (DataTable dtPReturnEdit = objPReturnHeader.GetPRHeaderAllDataByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString()))
        {
            if (dtPReturnEdit.Rows.Count > 0)
            {
                try
                {
                    if (objSenderID.ToString() != "lnkViewDetail")
                    {
                        if (Convert.ToBoolean(dtPReturnEdit.Rows[0]["Field1"].ToString()))
                        {
                            DisplayMessage("This Record has posted ,Can not edit");
                            return;
                        }
                    }
                }
                catch
                {
                }
                if (objSenderID.ToString() == "lnkViewDetail")
                {
                    Lbl_Tab_New.Text = Resources.Attendance.View;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }
                else
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }

                txtPInvoiceNo.Visible = true;

                if (Lbl_Tab_New.Text.Trim() == "View")
                {
                    btnPReturnSave.Enabled = false;
                    btnPost.Enabled = false;
                    BtnReset.Visible = false;
                }
                else
                {
                    btnPReturnSave.Enabled = true;
                    btnPost.Enabled = true;
                    BtnReset.Visible = true;
                }

                hdnPReturnId.Value = e.CommandArgument.ToString();
                txtReturnNo.Text = dtPReturnEdit.Rows[0]["PReturn_No"].ToString();
                ddlPIncoiceNo.Visible = false;
                ddlPIncoiceNo.DataSource = null;
                ddlPIncoiceNo.DataBind();
                txtReturnNo.ReadOnly = true;
                txtReturnDate.Text = Convert.ToDateTime(dtPReturnEdit.Rows[0]["PRDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                try
                {
                    ddlInvoiceType.SelectedValue = dtPReturnEdit.Rows[0]["InvoiceType"].ToString();
                    fillTabPaymentMode(ddlInvoiceType.SelectedItem.Text);
                }
                catch
                {
                }
                try
                {
                    ddlPaymentMode.SelectedValue = dtPReturnEdit.Rows[0]["PaymentModeID"].ToString();
                }
                catch
                {

                }
                txtRemark.Text = dtPReturnEdit.Rows[0]["Remark"].ToString();
                Txt_Net_Tax_Amount.Text = "0.00";
                ddlCurrency.SelectedValue = dtPReturnEdit.Rows[0]["CurrencyID"].ToString();
                txtPInvoiceNo.Enabled = false;
                //Add Invoice Detail
                string strInvoiceId = dtPReturnEdit.Rows[0]["Invoice_Id"].ToString();
                if (strInvoiceId != "" && strInvoiceId != "0")
                {
                    using (DataTable dtInvoiceNo = objPInvoice.GetPurchaseInvoiceTrueAllByTransId(StrCompId, StrBrandId, strLocationId, strInvoiceId))
                    {
                        if (dtInvoiceNo.Rows.Count > 0)
                        {
                            ViewState["InvoiceId"] = dtInvoiceNo.Rows[0]["TransID"].ToString();
                            hdnInvoiceId.Value = dtInvoiceNo.Rows[0]["TransID"].ToString();
                            txtPInvoiceNo.Text = dtInvoiceNo.Rows[0]["InvoiceNo"].ToString();

                            string strSupplierId = dtInvoiceNo.Rows[0]["SupplierId"].ToString();
                            txtPInvoiceDate.Text = Convert.ToDateTime(dtInvoiceNo.Rows[0]["InvoiceDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                            //Add Child Detail
                            using (DataTable dtDetail = objPReturnDetail.GetPRDetailByPReturn_No(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString()))
                            {
                                if (dtDetail.Rows.Count > 0)
                                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                                    objPageCmn.FillData((object)GvInvoiceDetail, dtDetail, "", "");

                                    GvInvoiceDetail.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(dtInvoiceNo.Rows[0]["CurrencyId"].ToString(), "Unit Cost", Session["DBConnection"].ToString());

                                }
                                else
                                {
                                    objPageCmn.FillData((object)GvInvoiceDetail, dtDetail, "", "");
                                }
                            }

                            //Add Supplier Name
                            if (strSupplierId != "" && strSupplierId != "0")
                            {
                                using (DataTable dtSupplier = objContact.GetContactTrueById(strSupplierId))
                                {
                                    if (dtSupplier.Rows.Count > 0)
                                    {
                                        txtSupplierName.Text = dtSupplier.Rows[0]["Name"].ToString() + "/" + dtSupplier.Rows[0]["Trans_Id"].ToString();
                                    }
                                    else
                                    {
                                        txtSupplierName.Text = "";
                                    }
                                }
                            }
                            else
                            {
                                txtSupplierName.Text = "";
                            }
                        }
                        else
                        {
                            txtPInvoiceDate.Text = "";
                            txtSupplierName.Text = "";
                        }
                    }
                }
                double NetTotal = 0;
                double NetTaxTotal_Edit = 0;
                string SupplierId = txtSupplierName.Text.Split('/')[1];

                string SupplierQuery = "Select Field2 from Set_Suppliers where Supplier_Id = " + SupplierId + "";
                using (DataTable dtSupplier_Edit = da.return_DataTable(SupplierQuery))
                {
                    if (!String.IsNullOrEmpty(dtSupplier_Edit.Rows[0]["Field2"].ToString()))
                        IsNonRegistered = Convert.ToBoolean(dtSupplier_Edit.Rows[0]["Field2"].ToString());
                }
                foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
                {
                    Label lblNetTotal = (Label)gvr.FindControl("lblIAmout");
                    Label lblIUnitCost_Gv = (Label)gvr.FindControl("lblIUnitCost");
                    Label lblITaxValue_gv = (Label)gvr.FindControl("lblITaxValue");
                    Label lblIDiscountValue_gv = (Label)gvr.FindControl("lblIDiscountValue");
                    TextBox txtReturnQty_gv = (TextBox)gvr.FindControl("txtReturnQty");
                    if (IsNonRegistered == false)
                    {
                        NetTotal += Convert.ToDouble(lblNetTotal.Text);
                        NetTaxTotal_Edit += Convert.ToDouble(lblITaxValue_gv.Text) * Convert.ToDouble(txtReturnQty_gv.Text);
                    }
                    else
                    {
                        NetTotal += (Convert.ToDouble(lblIUnitCost_Gv.Text) - Convert.ToDouble(lblIDiscountValue_gv.Text)) * Convert.ToDouble(txtReturnQty_gv.Text);
                        NetTaxTotal_Edit += 0;
                    }
                }
                txtNetAmount.Text = SetDecimal(NetTotal.ToString());
            }
        }
        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            if (((Label)gvr.FindControl("lblPoID")).Text == "0")
            {
                GvInvoiceDetail.Columns[1].Visible = false;
                break;
            }
            else
            {
                GvInvoiceDetail.Columns[1].Visible = true;
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
        if (objSenderID.ToString() == "lnkViewDetail")
        {
            btnPReturnSave.Enabled = false;
            btnPost.Enabled = false;
            BtnReset.Visible = false;
            
        }
        else
        {
            btnPReturnSave.Enabled = true;
            btnPost.Enabled = true;
            BtnReset.Visible = true;

            DataTable dtTemp = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(StrCompId, StrBrandId, strLocationId, "PR", hdnPReturnId.Value);

            if (dtTemp.Rows.Count > 0)
            {
                dtTemp = dtTemp.DefaultView.ToTable(false, "ProductId", "SerialNo", "Barcode", "BatchNo", "LotNo", "ExpiryDate", "Field1", "TransType", "TransTypeId", "ManufacturerDate", "Quantity", "Trans_Id", "Field2", "Field3");
                dtTemp.Columns["Field1"].ColumnName = "POID";
                dtTemp.Columns["Barcode"].ColumnName = "BarcodeNo";
                dtTemp.Columns["Field2"].ColumnName = "Invoiceqty";
                dtTemp.Columns["Field3"].ColumnName = "BatchRefId";
                ViewState["dtFinal"] = dtTemp;
            }
            dtTemp = null;
        }

        Txt_Net_Tax_Amount.Text = "0.00";
        double NetTaxTotal = 0;
        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            Label lblITaxValue_gv = (Label)gvr.FindControl("lblITaxValue");
            TextBox txtReturnQty_gv = (TextBox)gvr.FindControl("txtReturnQty");
            if (IsNonRegistered == false)
            {
                NetTaxTotal += Convert.ToDouble(lblITaxValue_gv.Text) * Convert.ToDouble(txtReturnQty_gv.Text);
            }
            else
            {
                NetTaxTotal += 0;
            }
        }
        Txt_Net_Tax_Amount.Text = SetDecimal(NetTaxTotal.ToString());
        Update_New.Update();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "PRDate" || ddlFieldName.SelectedItem.Value == "InvoiceDate")
        {
            if (txtValueDate.Text != "")
            {

                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
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
    protected void GvPReturn_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvPReturn.Attributes["CurrentSortField"] != null &&
            GvPReturn.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvPReturn.Attributes["CurrentSortField"])
            {
                if (GvPReturn.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvPReturn.Attributes["CurrentSortField"] = sortField;
        GvPReturn.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvreturnindex.Value));
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        if (Convert.ToBoolean(((Label)gvrow.FindControl("lblgvpost")).Text))
        {
            DisplayMessage("This Record has posted ,Can not Delete");
            return;
        }
        hdnPReturnId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objPReturnHeader.DeletePRHeader(StrCompId, StrBrandId, strLocationId, hdnPReturnId.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGrid();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        FillGrid();
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvreturnindex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void btnPReturnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReturnDate);
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        bool IsReturnqty = false;

        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            TextBox txtgvReturnQty = (TextBox)gvr.FindControl("txtReturnQty");

            if (txtgvReturnQty.Text.Trim() == "")
            {
                txtgvReturnQty.Text = "0";
            }

            if (Convert.ToDouble(txtgvReturnQty.Text) > 0)
            {
                IsReturnqty = true;
                break;
            }
        }

        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }
        ViewState["Post"] = true.ToString();
        btnPReturnSave_Click(sender, e);
    }
    protected void btnPReturnSave_Click(object sender, EventArgs e)
    {
        btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
        if (sender is Button)
        {
            Button btnId = (Button)sender;

            if (btnId.ID == "btnPost")
            {
                ViewState["Post"] = "True";
            }

            if (btnId.ID == "btnPReturnSave")
            {
                ViewState["Post"] = "False";
            }
        }

        bool IsReturnqty = false;

        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            TextBox txtgvReturnQty = (TextBox)gvr.FindControl("txtReturnQty");

            if (txtgvReturnQty.Text.Trim() == "")
            {
                txtgvReturnQty.Text = "0";
            }

            if (Convert.ToDouble(txtgvReturnQty.Text) > 0)
            {
                IsReturnqty = true;
                break;
            }
        }



        if (!IsReturnqty)
        {
            DisplayMessage("Return Quantity Should be greater than 0 !");
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        string strPaymentAccount = string.Empty;
        string strCashAccount = string.Empty;
        string strPurchaseReturn = string.Empty;
        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtCash.Rows.Count > 0)
        {
            strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
        }

        strPaymentAccount = Ac_ParameterMaster.GetSupplierAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());

        using (DataTable dtPurchaseReturn = new DataView(dtAcParameter, "Param_Name='Purchase Return'", "", DataViewRowState.CurrentRows).ToTable())
        {
            if (dtPurchaseReturn.Rows.Count > 0)
            {
                strPurchaseReturn = dtPurchaseReturn.Rows[0]["Param_Value"].ToString();
            }
        }

        if (txtReturnDate.Text == "")
        {
            DisplayMessage("Enter Return Date");
            txtReturnDate.Focus();
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtReturnDate.Text);

            }
            catch
            {
                DisplayMessage("Enter return Date in Format " + Session["DateFormat"].ToString() + "");
                txtReturnDate.Text = "";
                btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
                return;
            }
        }



        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtReturnDate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }



        if (txtReturnNo.Text == "")
        {
            DisplayMessage("Enter Return No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReturnNo);
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }
        else
        {
            if (hdnPReturnId.Value == "0")
            {
                DataTable dtReturnNo = objPReturnHeader.GetPRHeaderAllDataByPReturn_No(StrCompId, StrBrandId, strLocationId, txtReturnNo.Text);
                if (dtReturnNo.Rows.Count > 0)
                {
                    DisplayMessage("Purchase Return No. Already Exits");
                    txtReturnNo.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReturnNo);
                    btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
                    return;
                }
            }
        }

        if (txtSupplierName.Text == "")
        {
            DisplayMessage("Fill Supplier Name");
            txtSupplierName.Focus();
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }

        if (txtPInvoiceNo.Visible == true)
        {
            if (txtPInvoiceNo.Text == "")
            {
                DisplayMessage("Enter Purchase Invoice No.");
                txtPInvoiceNo.Focus();
                btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
                return;
            }
        }
        else if (txtPInvoiceNo.Visible == false)
        {
            if (ddlPIncoiceNo.Visible == true)
            {
                if (ddlPIncoiceNo.SelectedValue == "--Select--")
                {
                    DisplayMessage("Select Purchase Invoice No.");
                    ddlPIncoiceNo.Focus();
                    btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
                    return;
                }
            }
        }

        if (ddlInvoiceType.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Invoice Type");
            ddlInvoiceType.Focus();
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }
        if (ddlPaymentMode.SelectedValue == "--Select--")
        {
            DisplayMessage("Select Payment Mode");
            ddlPaymentMode.Focus();
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }
        if (txtRemark.Text == "")
        {
            DisplayMessage("Enter Remark");
            txtRemark.Focus();
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }
        if (txtNetAmount.Text == "0")
        {
            DisplayMessage("NetAmount is should be greater than Zero");
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
            return;
        }
        if (ViewState["Post"] == null)
        {
            ViewState["Post"] = false.ToString();
        }

        //code to check Account No exist or not in case of credit return - Neelknath Purohit - 04/09/2018
        string strOtherAccountId = "0";
        if (Convert.ToBoolean(ViewState["Post"].ToString()))
        {
            DataTable _dtPayMode = objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), ddlPaymentMode.SelectedValue, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            bool isCreditMode = _dtPayMode.Rows.Count == 0 || _dtPayMode.Rows[0]["field1"].ToString() != "Credit" ? false : true;
            if (isCreditMode == true)
            {
                strOtherAccountId = objAcAccountMaster.GetSupplierAccountByCurrency(txtSupplierName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue).ToString();
                if (strOtherAccountId == "0")
                {
                    DisplayMessage("Account Detail not exist for this customer, Please first create Account");
                    return;
                }
            }
            _dtPayMode.Dispose();
        }
        //-------------------end---------------------------


        string TaxQuery = "Select * from Inv_TaxRefDetail where Ref_Type='PINV' and Ref_Id = '" + hdnInvoiceId.Value + "' and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 and (Expenses_Id is null or Expenses_Id = '')";
        DataTable dtTaxDetails = da.return_DataTable(TaxQuery);

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            int b = 0;
            if (hdnPReturnId.Value != "0")
            {
                b = objPReturnHeader.UpdatePRHeader(StrCompId, StrBrandId, strLocationId, hdnPReturnId.Value, txtReturnNo.Text, ObjSysParam.getDateForInput(txtReturnDate.Text).ToString(), hdnInvoiceId.Value, ddlPaymentMode.SelectedValue, ddlInvoiceType.SelectedValue, txtRemark.Text, ViewState["Post"].ToString(), txtNetAmount.Text, ddlCurrency.SelectedValue, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Add Detail Section.
                objPReturnDetail.DeletePRDetail(StrCompId, StrBrandId, strLocationId, hdnPReturnId.Value, ref trns);

                objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PR", hdnPReturnId.Value, ref trns);

                Double TotalAmount = 0;
                string[] ProductDetail = null;
                double TaxAfterReturn = 0;
                List<string> newList = new List<string>();
                foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
                {
                    Label lblPoId = (Label)gvr.FindControl("lblPoID");
                    Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
                    HiddenField hdnProductId = (HiddenField)gvr.FindControl("hdnIProductId");
                    HiddenField hdnIUnitId = (HiddenField)gvr.FindControl("hdnIUnitId");
                    TextBox txtReturnQty = (TextBox)gvr.FindControl("txtReturnQty");
                    Label lblUnitCost = (Label)gvr.FindControl("lblIUnitCost");
                    Label lblITaxValue = (Label)gvr.FindControl("lblITaxValue");
                    Label lblIDiscountValue = (Label)gvr.FindControl("lblIDiscountValue");
                    Label lblIAmout = (Label)gvr.FindControl("lblIAmout");
                    Label lblIOrderQty = (Label)gvr.FindControl("lblIOrderQty");

                    if (txtReturnQty.Text.Trim() == "")
                    {
                        txtReturnQty.Text = "0";
                    }

                    TotalAmount = Convert.ToDouble(txtNetAmount.Text);
                    //string TotalReturnqty = (float.Parse(txtReturnQty.Text.ToString()) + float.Parse(lblTotalReturnqty.Text.ToString())).ToString();

                    int ProductledgerRefId = 0;

                    int Details_ID = objPReturnDetail.InsertPRDetail(StrCompId, StrBrandId, strLocationId, hdnPReturnId.Value, hdnProductId.Value, lblSerialNo.Text, txtReturnQty.Text, lblUnitCost.Text, lblPoId.Text, lblIDiscountValue.Text, lblITaxValue.Text, lblIAmout.Text, hdnIUnitId.Value, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    if (Convert.ToDouble(lblIAmout.Text) != 0)
                        insertTaxEntry(dtTaxDetails, hdnPReturnId.Value, hdnProductId.Value, lblIOrderQty.Text, txtReturnQty.Text, ddlCurrency.SelectedValue, ref trns, Details_ID.ToString(), lblIAmout.Text);

                    //code is modified by jitendra upadhyay on 09-08-2016
                    //code modifed for also insert row of non stockable item in ledger table for check complete cycle 
                    if (Convert.ToBoolean(ViewState["Post"].ToString()))
                    {
                        ObjProductLadger.DeleteProduct_Ledger(StrCompId, StrBrandId, strLocationId, "PR", hdnPReturnId.Value, hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                        ProductledgerRefId = ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "PR", hdnPReturnId.Value, "0", hdnProductId.Value, hdnIUnitId.Value, "O", "0", "0", "0", txtReturnQty.Text, "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtReturnDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }

                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                    {
                        if (ViewState["dtFinal"] != null)
                        {
                            DataTable dt = (DataTable)ViewState["dtFinal"];
                            ObjStockBatchMaster.DeleteStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "PR", hdnPReturnId.Value, hdnProductId.Value, ref trns);
                            dt = new DataView(dt, "ProductId='" + hdnProductId.Value + "' and POId='" + lblPoId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToBoolean(ViewState["Post"].ToString()))
                                {
                                    if (float.Parse(dr["Quantity"].ToString()) == 0)
                                    {
                                        continue;
                                    }
                                }
                                //ObjStockBatchMaster.DeleteStockBatchMaster(StrCompId, StrBrandId, strLocationId, "PR", hdnPReturnId.Value, hdnProductId.Value);
                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "PR", hdnPReturnId.Value, hdnProductId.Value, hdnIUnitId.Value, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), dr["BarcodeNo"].ToString(), "", "", "", dr["POId"].ToString(), dr["Invoiceqty"].ToString(), dr["BatchRefId"].ToString(), ProductledgerRefId.ToString(), "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                        }
                    }
                    if (Convert.ToDouble(txtReturnQty.Text) > 0)
                    {
                        string StrDetail = hdnProductId.Value + "," + lblIOrderQty.Text + "," + txtReturnQty.Text;
                        newList.Add(StrDetail);
                    }

                }
                ProductDetail = newList.ToArray();
                //End  
                if (b != 0)
                {
                    
                    if (Convert.ToBoolean(ViewState["Post"].ToString()))
                    {
                        //For Voucher & Ageing(Due) Entries
                        objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), strDepartmentId, hdnPReturnId.Value, "PR", "0", txtReturnDate.Text, txtReturnNo.Text, txtReturnDate.Text, "PR", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue.ToString(), "0.00", "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        string strVMaxId = string.Empty;
                        DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                        if (dtVMaxId.Rows.Count > 0)
                        {
                            strVMaxId = dtVMaxId.Rows[0][0].ToString();
                        }

                        //Added by KSR on 11-09-2017
                        string InvoiceQuery = "Select TransId,ExchangeRate from Inv_PurchaseInvoiceHeader where InvoiceNo = '" + txtPInvoiceNo.Text.Trim() + "'";
                        DataTable dtInvoice = da.return_DataTable(InvoiceQuery);
                        string InvoiceNo = string.Empty;
                        InvoiceNo = dtInvoice.Rows[0]["TransId"].ToString();

                        bool IsNonRegistered = false;
                        string SupplierId = txtSupplierName.Text.Split('/')[1];
                        string SupplierQuery = "Select * from Set_Suppliers where Supplier_Id = " + SupplierId + "";
                        DataTable dtSupplier = da.return_DataTable(SupplierQuery);
                        if (!String.IsNullOrEmpty(dtSupplier.Rows[0]["Field2"].ToString()))
                            IsNonRegistered = Convert.ToBoolean(dtSupplier.Rows[0]["Field2"].ToString());

                        //Lokesh
                        //string strSupplierCurrencyDr =SystemParameter.GetCurrency(ddlCurrency.SelectedValue, TotalAmount.ToString(),ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        string strSupplierCurrencyDr = SystemParameter.GetCurrency(ddlCurrency.SelectedValue, TotalAmount.ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        string strSuppCurrency = strSupplierCurrencyDr.Trim().Split('/')[0].ToString();
                        string strSupExchange = strSupplierCurrencyDr.Trim().Split('/')[1].ToString();

                        int j = 0;
                        Hdn_Exchange_Rate.Value = dtInvoice.Rows[0]["ExchangeRate"].ToString();
                        double Exchange_Rate = Convert.ToDouble(Hdn_Exchange_Rate.Value);





                        double L_Net_Amount_Return = 0;
                        double C_Net_Amount_Return = 0;
                        double F_Net_Amount_Return = 0;

                        double L_Tax_Amount_Return = 0;
                        double C_Tax_Amount_Return = 0;
                        double F_Tax_Amount_Return = 0;



                        if (txtNetAmount.Text.Trim() != "")
                        {
                            L_Net_Amount_Return = Convert.ToDouble(SetDecimal((Convert.ToDouble(txtNetAmount.Text) * Exchange_Rate).ToString()));
                            C_Net_Amount_Return = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Net_Amount_Return).ToString())), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).Split('/')[0].ToString());
                            F_Net_Amount_Return = Convert.ToDouble(SetDecimal((L_Net_Amount_Return / Exchange_Rate).ToString()));
                        }
                        TaxAfterReturn = Convert.ToDouble(SaveTaxEntry(IsNonRegistered, InvoiceNo, trns, ProductDetail, L_Tax_Amount_Return, strVMaxId, Exchange_Rate.ToString(), L_Net_Amount_Return.ToString(), true, j));
                        if (TaxAfterReturn != 0)
                        {
                            L_Tax_Amount_Return = Convert.ToDouble(SetDecimal((Convert.ToDouble(TaxAfterReturn) * Exchange_Rate).ToString()));
                            C_Tax_Amount_Return = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Tax_Amount_Return).ToString())), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).Split('/')[0].ToString());
                            F_Tax_Amount_Return = Convert.ToDouble(SetDecimal((L_Tax_Amount_Return / Exchange_Rate).ToString()));
                        }




                        if (ddlInvoiceType.SelectedValue.Trim() == "Cash")
                        {
                            //Debit Entry
                            //string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strSuppCurrency, ref trns);
                            //string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();

                            string L_Net_Amount_Return_Temp = L_Net_Amount_Return.ToString();
                            string C_Net_Amount_Return_Temp = C_Net_Amount_Return.ToString();
                            string F_Net_Amount_Return_Temp = F_Net_Amount_Return.ToString();

                            string L_Tax_Amount_Return_Temp = L_Tax_Amount_Return.ToString();
                            string C_Tax_Amount_Return_Temp = C_Tax_Amount_Return.ToString();
                            string F_Tax_Amount_Return_Temp = F_Tax_Amount_Return.ToString();

                            if (IsNonRegistered == true)
                            {
                                L_Net_Amount_Return_Temp = (L_Net_Amount_Return - L_Tax_Amount_Return).ToString();
                                C_Net_Amount_Return_Temp = (C_Net_Amount_Return - C_Tax_Amount_Return).ToString();
                                F_Net_Amount_Return_Temp = (F_Net_Amount_Return - F_Tax_Amount_Return).ToString();
                                //CompanyCurrDebit = (Convert.ToDouble(CompanyCurrDebit) - Convert.ToDouble(TaxAfterReturn)).ToString();
                            }
                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strCashAccount, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", L_Net_Amount_Return_Temp, "0.00", "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, C_Net_Amount_Return_Temp, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            //Credit Entry
                            //string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), strSuppCurrency, ref trns);
                            //string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            //CompanyCurrCredit = (Convert.ToDouble(CompanyCurrCredit) - Convert.ToDouble(TaxAfterReturn)).ToString();

                            L_Net_Amount_Return_Temp = (L_Net_Amount_Return - L_Tax_Amount_Return).ToString();
                            C_Net_Amount_Return_Temp = (C_Net_Amount_Return - C_Tax_Amount_Return).ToString();
                            F_Net_Amount_Return_Temp = (F_Net_Amount_Return - F_Tax_Amount_Return).ToString();

                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strPurchaseReturn, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", "0.00", L_Net_Amount_Return_Temp, "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, "0.00", C_Net_Amount_Return_Temp, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                        else if (ddlInvoiceType.SelectedValue.Trim() == "Credit")
                        {
                            //Debit Entry

                            string L_Net_Amount_Return_Temp = L_Net_Amount_Return.ToString();
                            string C_Net_Amount_Return_Temp = C_Net_Amount_Return.ToString();
                            string F_Net_Amount_Return_Temp = F_Net_Amount_Return.ToString();

                            string L_Tax_Amount_Return_Temp = L_Tax_Amount_Return.ToString();
                            string C_Tax_Amount_Return_Temp = C_Tax_Amount_Return.ToString();
                            string F_Tax_Amount_Return_Temp = F_Tax_Amount_Return.ToString();

                            if (IsNonRegistered == true)
                            {
                                L_Net_Amount_Return_Temp = (L_Net_Amount_Return - L_Tax_Amount_Return).ToString();
                                C_Net_Amount_Return_Temp = (C_Net_Amount_Return - C_Tax_Amount_Return).ToString();
                                F_Net_Amount_Return_Temp = (F_Net_Amount_Return - F_Tax_Amount_Return).ToString();
                                //CompanyCurrDebit = (Convert.ToDouble(CompanyCurrDebit) - Convert.ToDouble(TaxAfterReturn)).ToString();
                            }

                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strPaymentAccount, strOtherAccountId, hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", L_Net_Amount_Return_Temp, "0.00", "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, C_Net_Amount_Return_Temp, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            L_Net_Amount_Return_Temp = (L_Net_Amount_Return - L_Tax_Amount_Return).ToString();
                            C_Net_Amount_Return_Temp = (C_Net_Amount_Return - C_Tax_Amount_Return).ToString();
                            F_Net_Amount_Return_Temp = (F_Net_Amount_Return - F_Tax_Amount_Return).ToString();

                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strPurchaseReturn, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", "0.00", L_Net_Amount_Return_Temp, "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, "0.00", C_Net_Amount_Return_Temp, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        }


                        if (IsNonRegistered == true)
                        {
                            string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                            //string ExpensesAmount = TaxAfterReturn.ToString();

                            string L_Tax_Amount_Return_Temp = L_Tax_Amount_Return.ToString();
                            string C_Tax_Amount_Return_Temp = C_Tax_Amount_Return.ToString();
                            string F_Tax_Amount_Return_Temp = F_Tax_Amount_Return.ToString();

                            //objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), ExpensesAccountId, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", "0.00", L_Tax_Amount_Return_Temp, "From PR On '" + txtReturnNo.Text + "' On '" + GetLocationCode(Session["LocId"].ToString()) + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Tax_Amount_Return_Temp, "0.00", C_Tax_Amount_Return_Temp, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }

                        SaveTaxEntry(IsNonRegistered, InvoiceNo, trns, ProductDetail, L_Tax_Amount_Return, strVMaxId, Exchange_Rate.ToString(), L_Net_Amount_Return.ToString(), false, j);

                        DisplayMessage("Record has been Posted");
                    }
                    else
                    {
                        DisplayMessage("Record Updated", "green");
                        Lbl_Tab_New.Text = Resources.Attendance.New;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    }
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {

                b = objPReturnHeader.InsertPRHeader(StrCompId, StrBrandId, strLocationId, txtReturnNo.Text, ObjSysParam.getDateForInput(txtReturnDate.Text).ToString(), hdnInvoiceId.Value, ddlPaymentMode.SelectedValue, ddlInvoiceType.SelectedValue, txtRemark.Text, ViewState["Post"].ToString(), txtNetAmount.Text, ddlCurrency.SelectedValue, "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PR", b.ToString(), ref trns);
                if (b != 0)
                {
                    string strMaxId = b.ToString();

                    int MaxreturnCount = Convert.ToInt32(da.return_DataTable("select COUNT(*) from Inv_PurchaseReturnHeader where Location_Id=" + Session["LocId"].ToString()+"",ref trns).Rows[0][0].ToString());
                    if (txtReturnNo.Text == ViewState["DocNo"].ToString())
                    {
                        objPReturnHeader.Updatecode(b.ToString(), txtReturnNo.Text + (MaxreturnCount).ToString(), ref trns);
                        txtReturnNo.Text = txtReturnNo.Text + (MaxreturnCount).ToString();
                    }

                    //Add Detail Section.
                    objPReturnDetail.DeletePRDetail(StrCompId, StrBrandId, strLocationId, strMaxId, ref trns);

                    Double TotalAmount = 0;
                    string[] ProductDetail = null;
                    double TaxAfterReturn = 0;
                    List<string> newList = new List<string>();
                    foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
                    {
                        Label lblPoId = (Label)gvr.FindControl("lblPoID");
                        Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
                        HiddenField hdnProductId = (HiddenField)gvr.FindControl("hdnIProductId");
                        TextBox txtReturnQty = (TextBox)gvr.FindControl("txtReturnQty");
                        Label lblUnitCost = (Label)gvr.FindControl("lblIUnitCost");
                        Label lblITaxValue = (Label)gvr.FindControl("lblITaxValue");
                        Label lblIDiscountValue = (Label)gvr.FindControl("lblIDiscountValue");
                        Label lblIAmout = (Label)gvr.FindControl("lblIAmout");
                        Label lblIOrderQty = (Label)gvr.FindControl("lblIOrderQty");
                        HiddenField hdnIUnitId = (HiddenField)gvr.FindControl("hdnIUnitId");

                        if (txtReturnQty.Text.Trim() == "")
                        {
                            txtReturnQty.Text = "0";
                        }
                        TotalAmount = Convert.ToDouble(txtNetAmount.Text);

                        int ProductledgerRefId = 0;
                        int Details_ID = objPReturnDetail.InsertPRDetail(StrCompId, StrBrandId, strLocationId, strMaxId, hdnProductId.Value, lblSerialNo.Text, txtReturnQty.Text, lblUnitCost.Text, lblPoId.Text, lblIDiscountValue.Text, lblITaxValue.Text, lblIAmout.Text, hdnIUnitId.Value, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        //code is modified by jitendra upadhyay on 09-08-2016
                        if (Convert.ToDouble(lblIAmout.Text) != 0)
                            insertTaxEntry(dtTaxDetails, strMaxId, hdnProductId.Value, lblIOrderQty.Text, txtReturnQty.Text, ddlCurrency.SelectedValue, ref trns, Details_ID.ToString(), lblIAmout.Text);


                        //code modifed for also insert row of non stockable item in ledger table for check complete cycle 
                        if (Convert.ToBoolean(ViewState["Post"].ToString()))
                        {
                            ProductledgerRefId = ObjProductLadger.InsertProductLedger(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "PR", strMaxId, "0", hdnProductId.Value, hdnIUnitId.Value, "O", "0", "0", "0", txtReturnQty.Text, "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", lblUnitCost.Text, "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtReturnDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                            // ViewState["Post"] = false.ToString();
                        }

                        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnProductId.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                        {

                            if (ViewState["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)ViewState["dtFinal"];
                                dt = new DataView(dt, "ProductId='" + hdnProductId.Value + "' and POId='" + lblPoId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (DataRow dr in dt.Rows)
                                {

                                    if (Convert.ToBoolean(ViewState["Post"].ToString()))
                                    {
                                        if (float.Parse(dr["Quantity"].ToString()) == 0)
                                        {
                                            continue;
                                        }
                                    }
                                    ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "PR", strMaxId, hdnProductId.Value, hdnIUnitId.Value, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString(), dr["ManufacturerDate"].ToString(), dr["BarcodeNo"].ToString(), "", "", "", dr["POId"].ToString(), dr["Invoiceqty"].ToString(), dr["Trans_Id"].ToString(), ProductledgerRefId.ToString(), "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }
                        string StrDetail = hdnProductId.Value + "," + lblIOrderQty.Text + "," + txtReturnQty.Text;
                        newList.Add(StrDetail);
                    }
                    ProductDetail = newList.ToArray();
                    if (Convert.ToBoolean(ViewState["Post"].ToString()))
                    {
                        //For Voucher & Ageing(Due) Entries
                        objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), strDepartmentId, strMaxId, "PR", "0", txtReturnDate.Text, txtReturnNo.Text, txtReturnDate.Text, "PR", "1/1/1800", "1/1/1800", "", "", ddlCurrency.SelectedValue.ToString(), "0.00", "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        string strVMaxId = string.Empty;
                        DataTable dtVMaxId = objVoucherHeader.GetVoucherHeaderMaxId(ref trns);
                        if (dtVMaxId.Rows.Count > 0)
                        {
                            strVMaxId = dtVMaxId.Rows[0][0].ToString();
                        }

                        //                        ObjSysParam.
                        //string strSupplierCurrencyDr =SystemParameter.GetCurrency(ddlCurrency.SelectedValue, TotalAmount.ToString(), ref trns, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                        //string strSupplierCurrencyDr = TotalAmount.ToString();
                        //string strSuppCurrency = strSupplierCurrencyDr.Trim().Split('/')[0].ToString();
                        //string strSupExchange = strSupplierCurrencyDr.Trim().Split('/')[1].ToString();


                        string strSupplierCurrencyDr = TotalAmount.ToString();
                        string strSuppCurrency = TotalAmount.ToString();
                        string strSupExchange = "1".ToString();


                        //Added by KSR on 14-09-2017
                        string InvoiceQuery = "Select TransId from Inv_PurchaseInvoiceHeader where InvoiceNo = '" + txtPInvoiceNo.Text.Trim() + "'";
                        DataTable dtInvoice = da.return_DataTable(InvoiceQuery);
                        string InvoiceNo = string.Empty;
                        InvoiceNo = dtInvoice.Rows[0]["TransId"].ToString();

                        bool IsNonRegistered = false;
                        string SupplierId = txtSupplierName.Text.Split('/')[1];
                        string SupplierQuery = "Select Field2 from Set_Suppliers where Supplier_Id = " + SupplierId + "";
                        DataTable dtSupplier = da.return_DataTable(SupplierQuery);
                        if (!String.IsNullOrEmpty(dtSupplier.Rows[0]["Field2"].ToString()))
                            IsNonRegistered = Convert.ToBoolean(dtSupplier.Rows[0]["Field2"].ToString());

                        int j = 0;

                        double Exchange_Rate = Convert.ToDouble(Hdn_Exchange_Rate.Value);
                        double L_Net_Amount_Return = 0;
                        double C_Net_Amount_Return = 0;
                        double F_Net_Amount_Return = 0;
                        double L_Tax_Amount_Return = 0;
                        double C_Tax_Amount_Return = 0;
                        double F_Tax_Amount_Return = 0;
                        if (txtNetAmount.Text.Trim() != "")
                        {
                            L_Net_Amount_Return = Convert.ToDouble(SetDecimal((Convert.ToDouble(txtNetAmount.Text) * Exchange_Rate).ToString()));
                            C_Net_Amount_Return = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Net_Amount_Return).ToString())), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).Split('/')[0].ToString());
                            F_Net_Amount_Return = Convert.ToDouble(SetDecimal((L_Net_Amount_Return / Exchange_Rate).ToString()));
                        }
                        TaxAfterReturn = Convert.ToDouble(SaveTaxEntry(IsNonRegistered, InvoiceNo, trns, ProductDetail, L_Tax_Amount_Return, strVMaxId, Exchange_Rate.ToString(), L_Net_Amount_Return.ToString(), true, j));
                        if (TaxAfterReturn != 0)
                        {
                            L_Tax_Amount_Return = Convert.ToDouble(SetDecimal((Convert.ToDouble(TaxAfterReturn) * Exchange_Rate).ToString()));
                            C_Tax_Amount_Return = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Tax_Amount_Return).ToString())), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).Split('/')[0].ToString());
                            F_Tax_Amount_Return = Convert.ToDouble(SetDecimal((L_Tax_Amount_Return / Exchange_Rate).ToString()));
                        }
                        if (ddlInvoiceType.SelectedValue.Trim() == "Cash")
                        {
                            string L_Net_Amount_Return_Temp = L_Net_Amount_Return.ToString();
                            string C_Net_Amount_Return_Temp = C_Net_Amount_Return.ToString();
                            string F_Net_Amount_Return_Temp = F_Net_Amount_Return.ToString();

                            string L_Tax_Amount_Return_Temp = L_Tax_Amount_Return.ToString();
                            string C_Tax_Amount_Return_Temp = C_Tax_Amount_Return.ToString();
                            string F_Tax_Amount_Return_Temp = F_Tax_Amount_Return.ToString();

                            if (IsNonRegistered == true)
                            {
                                L_Net_Amount_Return_Temp = (L_Net_Amount_Return - L_Tax_Amount_Return).ToString();
                                C_Net_Amount_Return_Temp = (C_Net_Amount_Return - C_Tax_Amount_Return).ToString();
                                F_Net_Amount_Return_Temp = (F_Net_Amount_Return - F_Tax_Amount_Return).ToString();
                                //CompanyCurrDebit = (Convert.ToDouble(CompanyCurrDebit) - Convert.ToDouble(TaxAfterReturn)).ToString();
                            }

                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strCashAccount, "0", strMaxId, "PR", "1/1/1800", "1/1/1800", "", L_Net_Amount_Return_Temp, "0.00", "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, C_Net_Amount_Return_Temp, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            L_Net_Amount_Return_Temp = (Convert.ToDouble(L_Net_Amount_Return_Temp) - L_Tax_Amount_Return).ToString();
                            C_Net_Amount_Return_Temp = (Convert.ToDouble(C_Net_Amount_Return_Temp) - C_Tax_Amount_Return).ToString();
                            F_Net_Amount_Return_Temp = (Convert.ToDouble(F_Net_Amount_Return_Temp) - F_Tax_Amount_Return).ToString();

                            //CompanyCurrDebit = (Convert.ToDouble(CompanyCurrDebit) - Convert.ToDouble(TaxAfterReturn)).ToString();


                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strPurchaseReturn, "0", strMaxId, "PR", "1/1/1800", "1/1/1800", "", "0.00", L_Net_Amount_Return_Temp, "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, "0.00", C_Net_Amount_Return_Temp, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else if (ddlInvoiceType.SelectedValue.Trim() == "Credit")
                        {
                            //Debit Entry
                            //string strCompanyCrrValueDr = GetCurrency(Session["CurrencyId"].ToString(), strSuppCurrency, ref trns);
                            //string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                            //if (IsNonRegistered)
                            //    CompanyCurrDebit = (Convert.ToDouble(CompanyCurrDebit) - Convert.ToDouble(TaxAfterReturn)).ToString();

                            string L_Net_Amount_Return_Temp = L_Net_Amount_Return.ToString();
                            string C_Net_Amount_Return_Temp = C_Net_Amount_Return.ToString();
                            string F_Net_Amount_Return_Temp = F_Net_Amount_Return.ToString();

                            string L_Tax_Amount_Return_Temp = L_Tax_Amount_Return.ToString();
                            string C_Tax_Amount_Return_Temp = C_Tax_Amount_Return.ToString();
                            string F_Tax_Amount_Return_Temp = F_Tax_Amount_Return.ToString();

                            if (IsNonRegistered == true)
                            {
                                L_Net_Amount_Return_Temp = (L_Net_Amount_Return - L_Tax_Amount_Return).ToString();
                                C_Net_Amount_Return_Temp = (C_Net_Amount_Return - C_Tax_Amount_Return).ToString();
                                F_Net_Amount_Return_Temp = (F_Net_Amount_Return - F_Tax_Amount_Return).ToString();
                                //CompanyCurrDebit = (Convert.ToDouble(CompanyCurrDebit) - Convert.ToDouble(TaxAfterReturn)).ToString();
                            }

                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strPaymentAccount, strOtherAccountId, strMaxId, "PR", "1/1/1800", "1/1/1800", "", L_Net_Amount_Return_Temp, "0.00", "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, C_Net_Amount_Return_Temp, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                            //Credit Entry
                            //string strCompanyCrrValueCr = GetCurrency(Session["CurrencyId"].ToString(), strSuppCurrency, ref trns);
                            //string CompanyCurrCredit = strCompanyCrrValueCr.Trim().Split('/')[0].ToString();
                            //CompanyCurrCredit = (Convert.ToDouble(CompanyCurrCredit) - Convert.ToDouble(TaxAfterReturn)).ToString();

                            L_Net_Amount_Return_Temp = (Convert.ToDouble(L_Net_Amount_Return_Temp) - L_Tax_Amount_Return).ToString();
                            C_Net_Amount_Return_Temp = (Convert.ToDouble(C_Net_Amount_Return_Temp) - C_Tax_Amount_Return).ToString();
                            F_Net_Amount_Return_Temp = (Convert.ToDouble(F_Net_Amount_Return_Temp) - F_Tax_Amount_Return).ToString();

                            objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), strPurchaseReturn, "0", strMaxId, "PR", "1/1/1800", "1/1/1800", "", "0.00", L_Net_Amount_Return_Temp, "From PR On '" + txtReturnNo.Text + "' On '" + Session["LoginLocCode"].ToString() + "'", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Net_Amount_Return_Temp, "0.00", C_Net_Amount_Return_Temp, "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        if (IsNonRegistered == true)
                        {
                            string ExpensesAccountId = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter").Rows[0]["ParameterValue"].ToString();
                            //string ExpensesAmount = TaxAfterReturn.ToString();

                            string L_Tax_Amount_Return_Temp = L_Tax_Amount_Return.ToString();
                            string C_Tax_Amount_Return_Temp = C_Tax_Amount_Return.ToString();
                            string F_Tax_Amount_Return_Temp = F_Tax_Amount_Return.ToString();
                        }

                        SaveTaxEntry(IsNonRegistered, InvoiceNo, trns, ProductDetail, L_Tax_Amount_Return, strVMaxId, Exchange_Rate.ToString(), L_Net_Amount_Return.ToString(), false, j);
                    }
                    //End
                }
               

                if (b != 0)
                {
                    if (Convert.ToBoolean(ViewState["Post"].ToString()))
                    {
                        DisplayMessage("Record has been Posted");
                    }
                    else
                    {
                        DisplayMessage("Record Saved","green");
                    }
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
            Reset();
            btnPReturnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPReturnSave, "").ToString());
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

    protected void insertTaxEntry(DataTable dtTaxDetails, string prReturnID, string product_id, string strInvoiceQty, string strReturnQty, string strCurrencyId, ref SqlTransaction trns, string Details_ID, string lblIAmout)
    {
        double returnQty = 0;
        double.TryParse(strReturnQty, out returnQty);
        if (dtTaxDetails.Rows.Count > 0 && returnQty > 0)
        {
            DataTable dttemp = dtTaxDetails.DefaultView.ToTable("Temp", true, "productId", "Tax_Value", "field1", "Tax_Id", "Tax_Per");

            DataRow[] drTax = dttemp.Select("productId = " + product_id);
            foreach (DataRow dr in drTax)
            {
                double actualInvoiceQty = 0;
                double.TryParse(strInvoiceQty, out actualInvoiceQty);
                string taxAmount = "0";
                string taxableAmount = "0";

                //taxAmount = ((double.Parse(dr["Tax_Value"].ToString()) / actualInvoiceQty) * returnQty).ToString();
                //taxableAmount = ((double.Parse(dr["field1"].ToString()) / actualInvoiceQty) * returnQty).ToString();
                //taxAmount = GetCurrency(strCurrencyId, taxAmount).Split('/')[0].ToString();
                //taxableAmount = GetCurrency(strCurrencyId, taxableAmount).Split('/')[0].ToString();


                taxAmount = Convert_Into_DF(((Convert.ToDouble(Convert_Into_DF(dr["Tax_Value"].ToString())) / Convert.ToDouble(Convert_Into_DF(actualInvoiceQty.ToString()))) * Convert.ToDouble(Convert_Into_DF(returnQty.ToString()))).ToString());
                taxableAmount = Convert_Into_DF(((Convert.ToDouble(Convert_Into_DF(dr["field1"].ToString())) / Convert.ToDouble(Convert_Into_DF(actualInvoiceQty.ToString()))) * Convert.ToDouble(Convert_Into_DF(returnQty.ToString()))).ToString());


                if (Convert.ToDouble(lblIAmout) != 0)
                    objTaxRefDetail.InsertRecord("PR", prReturnID, "0", "0", product_id, dr["Tax_Id"].ToString(), SetDecimal(dr["Tax_Per"].ToString()).ToString(), taxAmount.ToString(), false.ToString(), taxableAmount.ToString(), Details_ID.ToString(), "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
        }
    }

    protected string SaveTaxEntry(bool IsNonRegistered, string InvoiceNo, SqlTransaction trns, string[] ProductDetail, double TaxAfterReturn, string strVMaxId, string strSupExchange, string TotalAmount, bool CalculatedOnly, int j)
    {
        //Credit Entry for Tax
        //Added By KSR on 11-09-2017
        if (IsNonRegistered == false)
        {
            string Return_Product_Id = "";
            if (ProductDetail.Length > 0)
            {
                foreach (string Pdetail in ProductDetail)
                {
                    string PD_Id = Pdetail.Split(',')[0];
                    string PD_OrQty = Pdetail.Split(',')[1];
                    string PD_ReQty = Pdetail.Split(',')[2];
                    if (Convert.ToDouble(PD_ReQty) != 0)
                    {
                        Return_Product_Id = Return_Product_Id + PD_Id + ",";
                    }
                }
            }


            string TaxQuery = "Declare @JJ int = 2;  Select ITRD.Tax_Id,ITRD.Tax_Per,ITRD.ProductId, STR(ROUND(((Cast((STR(ROUND(ITRD.Tax_value,@JJ,@JJ),10,@JJ)) as numeric (32,6)))/ (Cast((STR(ROUND(IPID.RecQty,@JJ,@JJ),10,@JJ)) as numeric (32,6)))),@JJ,@JJ),10,@JJ) as Tax_value from Inv_TaxRefDetail ITRD Inner Join Inv_PurchaseInvoiceDetail IPID on ITRD.Field2=IPID.TransID where ITRD.Ref_Type='PINV' and ITRD.Ref_Id = " + InvoiceNo + " and CAST(ITRD.Tax_Per as decimal(38,6)) <> 0 and CAST(ITRD.Tax_value as decimal(38,6)) <> 0 and (ITRD.Expenses_Id is null or ITRD.Expenses_Id = '') and ITRD.ProductId in (SELECT CAST(Value AS INT) FROM F_Split('" + Return_Product_Id + "', ','))";
            DataTable dtTaxDetails = da.return_DataTable(TaxQuery);
            if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
            {
                string TaxGrouping = "Declare @JJ int = 2;  Select ITRD.Tax_Id,STM.Tax_Name,STM.Field3,SUM(CAST(STR(ROUND(((Cast((STR(ROUND(ITRD.Tax_value,@JJ,@JJ),10,@JJ)) as numeric (32,6)))/ (Cast((STR(ROUND(IPID.RecQty,@JJ,@JJ),10,@JJ)) as numeric (32,6)))),@JJ,@JJ),10,@JJ) as numeric (32,6))) as TaxAmount from Inv_TaxRefDetail ITRD Inner Join Inv_PurchaseInvoiceDetail IPID on ITRD.Field2=IPID.TransID Inner Join Sys_TaxMaster STM on STM.Trans_Id = ITRD.Tax_Id where ITRD.Ref_Type='PINV' and ITRD.Ref_Id = " + InvoiceNo + " and CAST(ITRD.Tax_Per as decimal(38,6)) <> 0 and CAST(ITRD.Tax_value as decimal(38,6)) <> 0 and (ITRD.Expenses_Id is null or ITRD.Expenses_Id = '') and ITRD.ProductId in (SELECT CAST(Value AS INT) FROM F_Split('" + Return_Product_Id + "', ',')) group by Tax_Id,Tax_Name,STM.Field3";
                DataTable TaxTableGrouping = da.return_DataTable(TaxGrouping, ref trns);

                string TaxAccountNo = string.Empty;
                string TaxIdInfo = string.Empty;
                string GroupTaxId = string.Empty;
                string GroupTaxAmount = string.Empty;
                string GroupTaxValue = string.Empty;
                string GroupTaxName = string.Empty;
                string foreignTaxAmount = "0";
                string strTaxPer = string.Empty;
                TaxAfterReturn = 0;
                bool IsSave = false;
                foreach (DataRow grouprow in TaxTableGrouping.Rows)
                {
                    GroupTaxId = grouprow["Tax_Id"].ToString();
                    GroupTaxAmount = Convert_Into_DF(grouprow["TaxAmount"].ToString());
                    GroupTaxName = grouprow["Tax_Name"].ToString();
                    TaxAccountNo = grouprow["Field3"].ToString();
                    foreignTaxAmount =SystemParameter.GetCurrency(ddlCurrency.SelectedValue, GroupTaxAmount, Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Trim().Split('/')[0].ToString();
                    IsSave = false;
                    if (!CalculatedOnly)
                        TaxAfterReturn = 0;
                    foreach (DataRow row in dtTaxDetails.Rows)
                    {
                        if (!TaxIdInfo.Contains(GroupTaxId))
                        {
                            DataView groupView = new DataView(dtTaxDetails);
                            groupView.RowFilter = "Tax_Id = " + GroupTaxId + "";
                            GroupTaxName = string.Empty;
                            GroupTaxValue = string.Empty;
                            foreach (DataRow newrow in groupView.ToTable().Rows)
                            {
                                if (String.IsNullOrEmpty(GroupTaxValue))
                                    GroupTaxValue = newrow["Tax_Per"].ToString();
                                else if (strTaxPer != newrow["Tax_Per"].ToString())
                                {
                                    GroupTaxValue = GroupTaxValue + "," + newrow["Tax_Per"].ToString();
                                }
                                strTaxPer = newrow["Tax_Per"].ToString();

                                if (ProductDetail.Length > 0)
                                {
                                    foreach (string Pdetail in ProductDetail)
                                    {
                                        string PD_Id = Pdetail.Split(',')[0];
                                        string PD_OrQty = Pdetail.Split(',')[1];
                                        string PD_ReQty = Pdetail.Split(',')[2];

                                        if (PD_Id == newrow["ProductId"].ToString() && GroupTaxId == newrow["Tax_Id"].ToString())
                                        {
                                            double tax_val = Convert.ToDouble(newrow["Tax_value"].ToString());
                                            double unit_tax_val = (tax_val * Convert.ToDouble(PD_ReQty));
                                            //double new_tax_val = unit_tax_val * (Convert.ToDouble(PD_ReQty));
                                            double new_tax_val = unit_tax_val;
                                            //string FinalAmount = (Convert.ToDouble(GroupTaxAmount) - new_tax_val).ToString();
                                            string FinalAmount = new_tax_val.ToString();
                                            if (FinalAmount == "0")
                                                TaxAfterReturn += Convert.ToDouble(new_tax_val);
                                            else
                                                TaxAfterReturn += Convert.ToDouble(FinalAmount);

                                            GroupTaxAmount = FinalAmount;

                                            if (!CalculatedOnly)
                                                GroupTaxAmount = TaxAfterReturn.ToString();

                                            IsSave = true;
                                        }
                                    }
                                }
                            }

                            if (CalculatedOnly == false && IsSave)
                            {
                                double Exchange_Rate = Convert.ToDouble(strSupExchange);
                                double L_Tax_Amount_Return = Convert.ToDouble(SetDecimal((Convert.ToDouble(GroupTaxAmount) * Exchange_Rate).ToString()));
                                double C_Tax_Amount_Return = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (SetDecimal((L_Tax_Amount_Return).ToString())), Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).Split('/')[0].ToString());
                                double F_Tax_Amount_Return = Convert.ToDouble(SetDecimal((L_Tax_Amount_Return / Exchange_Rate).ToString()));



                                if (IsNonRegistered == false)
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), TaxAccountNo, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", "0.00", L_Tax_Amount_Return.ToString(), GroupTaxValue + "% " + GroupTaxName + "From PR On '" + txtReturnNo.Text + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Tax_Amount_Return.ToString(), "0.00", C_Tax_Amount_Return.ToString(), "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                else
                                    objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), strVMaxId, (j++).ToString(), TaxAccountNo, "0", hdnPReturnId.Value, "PR", "1/1/1800", "1/1/1800", "", L_Tax_Amount_Return.ToString(), "0.00", GroupTaxValue + "% " + GroupTaxName + "From PR On '" + txtReturnNo.Text + "", "", Session["EmpId"].ToString(), ddlCurrency.SelectedValue, Exchange_Rate.ToString(), F_Tax_Amount_Return.ToString(), C_Tax_Amount_Return.ToString(), "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            if (String.IsNullOrEmpty(TaxIdInfo))
                                TaxIdInfo = GroupTaxId;
                            else
                                TaxIdInfo = TaxIdInfo + "," + GroupTaxId;
                            break;
                        }
                    }
                }
                TaxTableGrouping = null;
            }
            dtTaxDetails = null;
        }
        return TaxAfterReturn.ToString();
    }
  
   
   
   
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {
        hdnPReturnId.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objPReturnHeader.DeletePRHeader(StrCompId, StrBrandId, strLocationId, hdnPReturnId.Value, true.ToString(), StrUserId, DateTime.Now.ToString());
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
    }
    #region Auto Complete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSupplier(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtSupplier = ObjSupplier.GetSupplierAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] filterlist = new string[dtSupplier.Rows.Count];
            if (dtSupplier.Rows.Count > 0)
            {
                for (int i = 0; i < dtSupplier.Rows.Count; i++)
                {
                    filterlist[i] = dtSupplier.Rows[i]["Filtertext"].ToString();
                }
            }
            return filterlist;
        }
        catch
        {
            return null;
        }

    }
    #endregion
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        
        FillGridBin();
    }
    protected void GvPReturnBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPReturnBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPReturnBin, dt, "", "");
        string temp = string.Empty;

        for (int i = 0; i < GvPReturnBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvPReturnBin.Rows[i].FindControl("lblgvReturnId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvPReturnBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void GvPReturnBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objPReturnHeader.GetPRHeaderAllFalse(StrCompId, StrBrandId, strLocationId);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPReturnBin, dt, "", "");
        lblSelectedRecord.Text = "";

    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objPReturnHeader.GetPRHeaderAllFalse(StrCompId, StrBrandId, strLocationId);
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPReturnBin, dt, "", "");
        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";

    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "PRDate")
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


            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPReturnBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";

            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }

        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvPReturnBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvPReturnBin.Rows.Count; i++)
        {
            ((CheckBox)GvPReturnBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvPReturnBin.Rows[i].FindControl("lblgvReturnId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvPReturnBin.Rows[i].FindControl("lblgvReturnId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvPReturnBin.Rows[i].FindControl("lblgvReturnId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvPReturnBin.Rows[index].FindControl("lblgvReturnId");
        if (((CheckBox)GvPReturnBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ddlFieldNameBin.SelectedIndex = 0;

        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;

        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

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
            for (int i = 0; i < GvPReturnBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvPReturnBin.Rows[i].FindControl("lblgvReturnId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvPReturnBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPReturnBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        bool Result = true;

        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (!Common.IsFinancialyearAllow(Convert.ToDateTime(objPReturnHeader.GetPRHeaderAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].ToString()).Rows[0]["PRDate"].ToString()), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                    {
                        Result = false;
                        break;
                    }
                }
            }
        }


        if (!Result)
        {
            DisplayMessage("You can not restore closed financial year record");
            return;
        }


        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objPReturnHeader.DeletePRHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    //updated by jitendra upadhyay on 08-Jan-2014 to update the status in purchase Inquiry
                    string InvoiceNo = string.Empty;
                    DataTable DtPReturn = objPReturnHeader.GetPRHeaderAllDataByTransId(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim());
                    if (DtPReturn.Rows.Count > 0)
                    {
                        InvoiceNo = DtPReturn.Rows[0]["Invoice_Id"].ToString();

                    }
                    DataTable DtInvoiceDetail = objPInvoiceD.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, strLocationId, InvoiceNo);
                    try
                    {
                        DtInvoiceDetail = new DataView(DtInvoiceDetail, "", "", DataViewRowState.CurrentRows).ToTable(true, "POId");

                    }
                    catch
                    {
                    }
                    for (int i = 0; i < DtInvoiceDetail.Rows.Count; i++)
                    {
                        string QuotationId = string.Empty;
                        DataTable DtPoheader = ObjPurchaseOrder.GetPurchaseOrderTrueAllByReqId(StrCompId, StrBrandId, strLocationId, DtInvoiceDetail.Rows[i]["POId"].ToString());
                        if (DtPoheader.Rows.Count > 0)
                        {
                            if (DtPoheader.Rows[0]["ReferenceVoucherType"].ToString() == "PQ")
                            {

                                QuotationId = DtPoheader.Rows[0]["ReferenceID"].ToString();

                                DataTable DtPoHeaderAll = ObjPurchaseOrder.GetPurchaseOrderTrueAll(StrCompId, StrBrandId, strLocationId);
                                try
                                {
                                    DtPoHeaderAll = new DataView(DtPoHeaderAll, "ReferenceVoucherType='PQ' and ReferenceID='" + QuotationId + "'", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                catch
                                {

                                }

                                string PinquiryNo = string.Empty;
                                DataTable DtQuotation = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, strLocationId, QuotationId);
                                if (DtQuotation.Rows.Count > 0)
                                {
                                    PinquiryNo = DtQuotation.Rows[0]["PI_No"].ToString();
                                }
                                ObjPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, strLocationId, PinquiryNo, "Goods Return", Session["UserId"].ToString(), DateTime.Now.ToString());


                            }


                        }
                    }



                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activate");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvPReturnBin.Rows)
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
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            strWhereClause += " and Field1='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            strWhereClause += " and Field1='False'";
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = objPReturnHeader.getReturnList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPReturn.Attributes["CurrentSortField"], GvPReturn.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPReturn, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPReturn.DataSource = null;
                GvPReturn.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
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
        FillGrid();
        FillPaymentMode();
        txtPInvoiceNo.Visible = true;
        txtPInvoiceNo.Text = "";
        ddlPIncoiceNo.Visible = false;
        txtSupplierName.Text = "";
        txtReturnNo.Text = objPReturnHeader.GetAutoID(StrCompId, StrBrandId, strLocationId);
        txtReturnNo.ReadOnly = false;
        txtReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtRemark.Text = "";
        txtPInvoiceNo.Enabled = true;
        GvInvoiceDetail.DataSource = null;
        GvInvoiceDetail.DataBind();
        hdnPReturnId.Value = "0";
        hdnInvoiceId.Value = "0";
        ViewState["InvoiceId"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueBin.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = true;
        txtValueBin.Visible = false;
        txtNetAmount.Text = "0";
        Txt_Net_Tax_Amount.Text = "0.00";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        txtReturnNo.Text = ViewState["DocNo"].ToString();
        ViewState["dtSerial"] = null;
        ViewState["PId"] = null;
        ViewState["dtFinal"] = null;
        ViewState["Post"] = null;
        btnPReturnSave.Enabled = true;
        btnPost.Enabled = true;
        BtnReset.Visible = true;
    }
    public string getQty(string Qty)
    {
        if (Qty == "")
        {
            Qty = "0";

        }
        return Qty;
    }
    #endregion
    #region Invoice Section
    private void FillInvoiceNo(string strSupplierId)
    {
        DataTable dsInvoiceNo = null;
        dsInvoiceNo = objPInvoice.GetInvoiceNoBySupplierId(StrCompId, StrBrandId, strLocationId, strSupplierId);
        try
        {
            dsInvoiceNo = new DataView(dsInvoiceNo, "Post='True'", "InvoiceNo Asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dsInvoiceNo.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlPIncoiceNo, dsInvoiceNo, "InvoiceNo", "TransID");
        }
        else
        {
            ddlPIncoiceNo.DataSource = dsInvoiceNo;
            ddlPIncoiceNo.DataBind();
            ddlPIncoiceNo.Items.Insert(0, "--Select--");

        }
        dsInvoiceNo = null;
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        string strSupplierId = string.Empty;
        if (txtSupplierName.Text != "")
        {
            strSupplierId =Set_Suppliers.GetSupplierId(txtSupplierName.Text, Session["DBConnection"].ToString());
            if (strSupplierId != "" && strSupplierId != "0")
            {
                FillInvoiceNo(strSupplierId);
                ddlPIncoiceNo.Visible = true;
                txtPInvoiceNo.Visible = false;
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtSupplierName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierName);
            }
        }
        else
        {
            FillInvoiceNo("0");
            txtPInvoiceNo.Text = "";
            GvInvoiceDetail.DataSource = null;
            GvInvoiceDetail.DataBind();
        }
    }
    protected void txtPInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (txtPInvoiceNo.Text != "")
        {
            using (DataTable dtInvoiceNo = objPInvoice.GetDataByInvoiceNo(StrCompId, StrBrandId, strLocationId, txtPInvoiceNo.Text))
            {
                if (dtInvoiceNo.Rows.Count > 0)
                {
                    if (dtInvoiceNo.Rows[0]["Post"].ToString() == "False")
                    {
                        DisplayMessage("You can not create return voucher for unposted invoice");
                        txtPInvoiceNo.Text = "";
                        txtPInvoiceNo.Focus();
                        return;
                    }

                    ddlCurrency.SelectedValue = dtInvoiceNo.Rows[0]["CurrencyID"].ToString();
                    ViewState["InvoiceId"] = dtInvoiceNo.Rows[0]["TransID"].ToString();
                    hdnInvoiceId.Value = dtInvoiceNo.Rows[0]["TransID"].ToString();
                    string strSupplierId = dtInvoiceNo.Rows[0]["SupplierId"].ToString();
                    txtPInvoiceDate.Text = Convert.ToDateTime(dtInvoiceNo.Rows[0]["InvoiceDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Hdn_Exchange_Rate.Value = dtInvoiceNo.Rows[0]["ExchangeRate"].ToString();
                    //Add Child Detail
                    using (DataTable dtDetail = objPInvoiceD.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, strLocationId, hdnInvoiceId.Value))
                    {
                        if (dtDetail.Rows.Count > 0)
                        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                            objPageCmn.FillData((object)GvInvoiceDetail, dtDetail, "", "");
                        }
                        else
                        {
                            GvInvoiceDetail.DataSource = dtDetail;
                            GvInvoiceDetail.DataBind();
                        }
                    }

                    //here we selecled invoice type according the invoice payment

                    using (DataTable Dt = ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "PI", dtInvoiceNo.Rows[0]["TransID"].ToString()))
                    {

                        if (Dt.Rows.Count > 0)
                        {
                            try
                            {
                                ddlInvoiceType.SelectedValue = Dt.Rows[0]["PaymentType"].ToString();
                                fillTabPaymentMode(ddlInvoiceType.SelectedItem.Text);
                            }
                            catch
                            {

                            }
                        }
                    }

                    //Add Supplier Name
                    if (strSupplierId != "" && strSupplierId != "0")
                    {
                        using (DataTable dtSupplier = objContact.GetContactTrueById(strSupplierId))
                        {
                            if (dtSupplier.Rows.Count > 0)
                            {
                                txtSupplierName.Text = dtSupplier.Rows[0]["Name"].ToString() + "/" + dtSupplier.Rows[0]["Trans_Id"].ToString();
                            }
                            else
                            {
                                txtSupplierName.Text = "";
                            }
                        }
                    }
                    else
                    {
                        txtSupplierName.Text = "";
                    }
                }
                else
                {
                    DisplayMessage("Record Not Found");
                    txtPInvoiceNo.Text = "";
                    txtPInvoiceDate.Text = "";
                    txtSupplierName.Text = "";
                    txtPInvoiceNo.Focus();
                    Reset();
                }
            }
        }
        else
        {
            txtPInvoiceDate.Text = "";
            txtSupplierName.Text = "";
            DisplayMessage("Select In Suggestions Only");
            txtPInvoiceNo.Text = "";
            Reset();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPInvoiceNo);
        }

        //Add Edit Section
        if (hdnInvoiceId.Value != "" && hdnInvoiceId.Value != "0")
        {
            using (DataTable dtReturnData = objPReturnHeader.GetPRHeaderAllDataByInvoiceId(StrCompId, StrBrandId, strLocationId, hdnInvoiceId.Value))
            {
                if (dtReturnData.Rows.Count > 0)
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    hdnPReturnId.Value = dtReturnData.Rows[0]["Trans_Id"].ToString();
                    txtReturnNo.Text = dtReturnData.Rows[0]["PReturn_No"].ToString();
                    txtReturnNo.ReadOnly = true;
                    txtReturnDate.Text = Convert.ToDateTime(dtReturnData.Rows[0]["PRDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    try
                    {
                        ddlInvoiceType.SelectedValue = dtReturnData.Rows[0]["InvoiceType"].ToString();
                        fillTabPaymentMode(ddlInvoiceType.SelectedItem.Text);
                    }
                    catch
                    {
                    }

                    try
                    {
                        ddlPaymentMode.SelectedValue = dtReturnData.Rows[0]["PaymentModeID"].ToString();
                    }
                    catch
                    {
                    }
                    txtRemark.Text = dtReturnData.Rows[0]["Remark"].ToString();
                }
                else
                {
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    hdnPReturnId.Value = "0";
                    txtReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    txtRemark.Text = "";
                }
            }
        }
        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            if (((Label)gvr.FindControl("lblPoID")).Text == "0")
            {
                GvInvoiceDetail.Columns[1].Visible = false;
                break;
            }
            else
            {
                GvInvoiceDetail.Columns[1].Visible = true;
            }
        }
    }
    protected void ddlPIncoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPIncoiceNo.SelectedValue != "--Select--")
        {
            ViewState["InvoiceId"] = ddlPIncoiceNo.SelectedValue;
            hdnInvoiceId.Value = ddlPIncoiceNo.SelectedValue;
            using (DataTable dtData = objPInvoice.GetPurchaseInvoiceTrueAllByTransId(StrCompId, StrBrandId, strLocationId, ddlPIncoiceNo.SelectedValue))
            {
                if (dtData.Rows.Count > 0)
                {
                    txtPInvoiceDate.Text = Convert.ToDateTime(dtData.Rows[0]["InvoiceDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                    //Add Child Detail
                    using (DataTable dtDetail = objPInvoiceD.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId, StrBrandId, strLocationId, hdnInvoiceId.Value))
                    {
                        if (dtDetail.Rows.Count > 0)
                        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                            objPageCmn.FillData((object)GvInvoiceDetail, dtDetail, "", "");
                        }
                        else
                        {
                            GvInvoiceDetail.DataSource = dtDetail;
                            GvInvoiceDetail.DataBind();
                        }
                    }
                }
                else
                {
                    txtPInvoiceDate.Text = "";
                }
            }
        }
        else
        {
            GvInvoiceDetail.DataSource = null;
            GvInvoiceDetail.DataBind();
            txtPInvoiceDate.Text = "";
        }

        //Add Edit Section
        if (hdnInvoiceId.Value != "" && hdnInvoiceId.Value != "0")
        {
            using (DataTable dtReturnData = objPReturnHeader.GetPRHeaderAllDataByInvoiceId(StrCompId, StrBrandId, strLocationId, hdnInvoiceId.Value))
            {
                if (dtReturnData.Rows.Count > 0)
                {
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    hdnPReturnId.Value = dtReturnData.Rows[0]["Trans_Id"].ToString();
                    txtReturnNo.Text = dtReturnData.Rows[0]["PReturn_No"].ToString();
                    txtReturnNo.ReadOnly = true;
                    txtReturnDate.Text = Convert.ToDateTime(dtReturnData.Rows[0]["PRDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    try
                    {
                        ddlInvoiceType.SelectedValue = dtReturnData.Rows[0]["InvoiceType"].ToString();
                        fillTabPaymentMode(ddlInvoiceType.SelectedItem.Text);
                    }
                    catch
                    {

                    }
                    try
                    {
                        ddlPaymentMode.SelectedValue = dtReturnData.Rows[0]["PaymentModeID"].ToString();
                    }
                    catch
                    {

                    }

                    txtRemark.Text = dtReturnData.Rows[0]["Remark"].ToString();
                }
                else
                {
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    hdnPReturnId.Value = "0";

                    //txtReturnNo.Text = objPReturnHeader.GetAutoID(StrCompId, StrBrandId, strLocationId);
                    txtReturnNo.ReadOnly = false;
                    txtReturnDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                    txtRemark.Text = "";
                }
            }
        }
        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            if (((Label)gvr.FindControl("lblPoID")).Text == "0")
            {
                GvInvoiceDetail.Columns[1].Visible = false;
                break;
            }
            else
            {
                GvInvoiceDetail.Columns[1].Visible = true;

            }
        }
    }
    public string GetPOrderNo(string POId)
    {
        if (POId.Trim() != "0")
        {
            return da.return_DataTable("select pono from inv_purchaseorderheader where Transid=" + POId + "").Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }

    }
    protected void txtReturnQty_TextChanged(object sender, EventArgs e)
    {

        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        Label lblReceivedQty = (Label)gvRow.FindControl("lblIReceiveQty");
        TextBox txtReturnQty = (TextBox)gvRow.FindControl("txtReturnQty");
        Label lblIUnitCost = (Label)gvRow.FindControl("lblIUnitCost");
        Label lblITaxValue = (Label)gvRow.FindControl("lblITaxValue");
        Label lblIDiscountValue = (Label)gvRow.FindControl("lblIDiscountValue");
        Label lblIAmout = (Label)gvRow.FindControl("lblIAmout");

        if (lblReceivedQty.Text == "")
        {
            lblReceivedQty.Text = "0";
        }
        if (txtReturnQty.Text == "")
        {
            txtReturnQty.Text = "0";
        }

        if (float.Parse(txtReturnQty.Text) > float.Parse(lblReceivedQty.Text))
        {
            DisplayMessage("Return quantity should be less than or equal to received quantity");
            txtReturnQty.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtReturnQty);
            return;
        }


        lblIAmout.Text = SetDecimal(((Convert.ToDouble(lblIUnitCost.Text) - Convert.ToDouble(lblIDiscountValue.Text) + Convert.ToDouble(lblITaxValue.Text)) * Convert.ToDouble(txtReturnQty.Text)).ToString());
        double NetTotal = 0;
        double NetTaxTotal = 0;

        bool IsNonRegistered = false;
        string SupplierId = txtSupplierName.Text.Split('/')[1];
        string SupplierQuery = "Select Field2 from Set_Suppliers where Supplier_Id = " + SupplierId + "";
        DataTable dtSupplier = da.return_DataTable(SupplierQuery);
        if (!String.IsNullOrEmpty(dtSupplier.Rows[0]["Field2"].ToString()))
            IsNonRegistered = Convert.ToBoolean(dtSupplier.Rows[0]["Field2"].ToString());
        dtSupplier = null;

        foreach (GridViewRow gvr in GvInvoiceDetail.Rows)
        {
            Label lblNetTotal = (Label)gvr.FindControl("lblIAmout");
            Label lblIUnitCost_Gv = (Label)gvr.FindControl("lblIUnitCost");
            Label lblITaxValue_gv = (Label)gvr.FindControl("lblITaxValue");
            Label lblIDiscountValue_gv = (Label)gvr.FindControl("lblIDiscountValue");
            TextBox txtReturnQty_gv = (TextBox)gvr.FindControl("txtReturnQty");
            if (IsNonRegistered == false)
            {
                NetTotal += Convert.ToDouble(lblNetTotal.Text);
                NetTaxTotal += Convert.ToDouble(lblITaxValue_gv.Text) * Convert.ToDouble(txtReturnQty_gv.Text);
            }
            else
            {
                NetTotal += (Convert.ToDouble(lblIUnitCost_Gv.Text) - Convert.ToDouble(lblIDiscountValue_gv.Text)) * Convert.ToDouble(txtReturnQty_gv.Text);
                NetTaxTotal += 0;
            }
        }
        txtNetAmount.Text = SetDecimal(NetTotal.ToString());
        Txt_Net_Tax_Amount.Text = SetDecimal(NetTaxTotal.ToString());
    }
    #endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnPReturnSave.Visible = clsPagePermission.bAdd;
        btnPReturnSave.Visible = clsPagePermission.bAdd;
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
    }
    #endregion
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "12", "53", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
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
    #region View
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);

    }
    #endregion
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "PRDate" || ddlFieldName.SelectedItem.Value == "InvoiceDate")
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
    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (ddlFieldNameBin.SelectedItem.Value == "PRDate")
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
    }
    #endregion
    #region Postconcept
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    #endregion
    #region Serial Number
  
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        if (txtSerialNo.Text.Trim() != "")
        {
            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;
            if (ViewState["dtSerial"] == null)
            {
                dt = new DataTable();
                dt.Columns.Add("ProductId", typeof(int));
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("BarcodeNo");
                dt.Columns.Add("BatchNo");
                dt.Columns.Add("LotNo");
                dt.Columns.Add("ExpiryDate", typeof(DateTime));
                dt.Columns.Add("POID");
                dt.Columns.Add("TransType");
                dt.Columns.Add("TransTypeId", typeof(int));
                dt.Columns.Add("ManufacturerDate", typeof(DateTime));
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Trans_Id", typeof(int));
                dt.Columns.Add("Invoiceqty");
                dt.Columns.Add("BatchRefId");
                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {


                        if (new DataView(dt, "SerialNo='" + txt[i].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            string[] result = isSerialNumberValid(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                            if (result[0] == "VALID")
                            {

                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dr[0] = ViewState["PID"].ToString();
                                dr[1] = txt[i].ToString();
                                dr[2] = "0";
                                dr[3] = "0";
                                dr[4] = "0";
                                dr[5] = DateTime.Now.ToString();
                                dr[6] = ViewState["POId"].ToString();
                                dr[7] = "PG";
                                dr[8] = hdnInvoiceId.Value;
                                dr[9] = DateTime.Now.ToString();
                                dr[10] = Convert.ToDouble("1");
                                dr[11] = "0";

                                counter++;

                            }
                            else if (result[0].ToString() == "DUPLICATE")
                            {
                                DuplicateserialNo += txt[i].ToString() + ",";
                            }
                            else if (result[0].ToString().ToUpper() == "NOT EXIST")
                            {
                                serialNoExists += txt[i].ToString() + ",";
                            }
                            else if (result[0].ToString().ToUpper() == "ALREADY OUT")
                            {
                                alreadyout += txt[i].ToString() + ",";
                            }
                        }
                        else
                        {

                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                    }
                }

            }
            else
            {
                dt = (DataTable)ViewState["dtSerial"];
                dtTemp = dt.Copy();

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        if (new DataView(dt, "SerialNo='" + txt[i].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            string[] result = isSerialNumberValid(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                            if (result[0] == "VALID")
                            {

                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dr[0] = ViewState["PID"].ToString();
                                dr[1] = txt[i].ToString();
                                dr[2] = "0";
                                dr[3] = "0";
                                dr[4] = "0";
                                dr[5] = DateTime.Now.ToString();
                                dr[6] = ViewState["POId"].ToString();
                                dr[7] = "PR";
                                dr[8] = hdnInvoiceId.Value;
                                dr[9] = DateTime.Now.ToString();
                                dr[10] = Convert.ToDouble("1");
                                dr[11] = "0";
                                counter++;

                            }
                            else if (result[0].ToString().ToUpper() == "DUPLICATE")
                            {
                                DuplicateserialNo += txt[i].ToString() + ",";
                            }
                            else if (result[0].ToString().ToUpper() == "NOT EXIST")
                            {
                                serialNoExists += txt[i].ToString() + ",";
                            }
                            else if (result[0].ToString().ToUpper() == "ALREADY OUT")
                            {
                                alreadyout += txt[i].ToString() + ",";
                            }
                        }
                        else
                        {

                            DuplicateserialNo += txt[i].ToString() + ",";
                        }

                    }
                }
            }

        }
        else
        {

            if (gvStockwithManf_and_expiry.Rows.Count > 0)
            {

                DataTable dtexp = new DataTable();
                dtexp.Columns.Add("ProductId", typeof(int));
                dtexp.Columns.Add("SerialNo");
                dtexp.Columns.Add("BarcodeNo");
                dtexp.Columns.Add("BatchNo");
                dtexp.Columns.Add("LotNo");
                dtexp.Columns.Add("ExpiryDate", typeof(DateTime));
                dtexp.Columns.Add("POID");
                dtexp.Columns.Add("TransType");
                dtexp.Columns.Add("TransTypeId", typeof(int));
                dtexp.Columns.Add("ManufacturerDate", typeof(DateTime));
                dtexp.Columns.Add("Quantity", typeof(decimal));
                dtexp.Columns.Add("Trans_Id", typeof(int));
                dtexp.Columns.Add("Invoiceqty");
                dtexp.Columns.Add("BatchRefId");
                foreach (GridViewRow gvrow in gvStockwithManf_and_expiry.Rows)
                {
                    if (((TextBox)gvrow.FindControl("txtreturnqty")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txtreturnqty")).Text = "0";
                    }
                    DataRow dr = dtexp.NewRow();
                    dtexp.Rows.Add(dr);
                    dr[0] = ViewState["PID"].ToString();
                    dr[1] = "0";
                    dr[2] = "0";
                    dr[3] = "0";
                    dr[4] = "0";
                    if (gvStockwithManf_and_expiry.Columns[1].Visible == true)
                    {
                        dr[5] = ((Label)gvrow.FindControl("lblexpdate")).Text;
                    }
                    else
                    {
                        dr[5] = DateTime.Now.ToString();
                    }
                    dr[6] = ViewState["POId"].ToString();
                    dr[7] = "PG";
                    dr[8] = hdnPReturnId.Value;
                    if (gvStockwithManf_and_expiry.Columns[2].Visible == true)
                    {
                        dr[9] = ((Label)gvrow.FindControl("lblmfd")).Text;
                    }
                    else
                    {
                        dr[9] = DateTime.Now.ToString();
                    }

                    dr[10] = ((TextBox)gvrow.FindControl("txtreturnqty")).Text;
                    dr[11] = ((HiddenField)gvrow.FindControl("hdnTransId")).Value;
                    dr[12] = ((Label)gvrow.FindControl("lblInvoiceqty")).Text;
                    dr[13] = ((HiddenField)gvrow.FindControl("hdnBatchRefId")).Value;
                }


                ViewState["dtSerial"] = dtexp;
            }
            if (ViewState["dtSerial"] != null)
            {

                dt = (DataTable)ViewState["dtSerial"];
            }

        }
        string Message = string.Empty;
        if (DuplicateserialNo != "" || serialNoExists != "" || alreadyout != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is Already Exists=" + DuplicateserialNo;
            }
            if (serialNoExists != "")
            {
                Message += "Following serial Number is Not Exists in stock=" + serialNoExists;
            }
            if (alreadyout != "")
            {
                Message += "Following serial Number already out from stock=" + alreadyout;
            }

            DisplayMessage(Message);
        }


        ViewState["dtSerial"] = dt;
        if (ViewState["dtFinal"] == null)
        {
            if (ViewState["dtSerial"] != null)
            {
                ViewState["dtFinal"] = (DataTable)ViewState["dtSerial"];
            }
        }
        else
        {
            dt = new DataTable();
            DataTable Dtfinal = (DataTable)ViewState["dtFinal"];
            if (ViewState["POId"].ToString() == "0")
            {
                Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DataTable DtTemp = Dtfinal.Copy();
                try
                {
                    DtTemp = new DataView(DtTemp, "ProductId='" + ViewState["PID"].ToString() + "' and POrderNo='" + ViewState["POID"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

                if (DtTemp.Rows.Count > 0)
                {
                    string s = "POrderNo Not In('" + ViewState["PoId"].ToString() + "') or ProductId Not In('" + ViewState["PID"].ToString() + "')";
                    Dtfinal = new DataView(Dtfinal, s.ToString(), "", DataViewRowState.CurrentRows).ToTable();
                }

            }

            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];
            }
            Dtfinal.Merge(dt);
            ViewState["dtFinal"] = Dtfinal;
        }


        float QtyCount = 0;
        if (ViewState["dtSerial"] != null)
        {
            if (pnlSerialNumber.Visible == true)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerialNumber, (DataTable)ViewState["dtSerial"], "", "");
                txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
                QtyCount = gvSerialNumber.Rows.Count;
            }
            else
            {
                DataTable dtcountqty = (DataTable)ViewState["dtSerial"];
                foreach (DataRow dr in dtcountqty.Rows)
                {
                    try
                    {
                        QtyCount += float.Parse(dr["Quantity"].ToString());
                    }
                    catch
                    {
                        QtyCount += 0;
                    }
                }

            }
        }


        foreach (GridViewRow gvRow in GvInvoiceDetail.Rows)
        {

            TextBox txtReturnQty = (TextBox)gvRow.FindControl("txtReturnQty");
            if (txtReturnQty.Text == "")
            {
                txtReturnQty.Text = "0";
            }

            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                if (ViewState["dtSerial"] != null)
                {
                    txtReturnQty.Text = QtyCount.ToString();
                }
                else
                {
                    txtReturnQty.Text = "0";
                }
                txtReturnQty_TextChanged(((TextBox)gvRow.FindControl("txtReturnQty")), null);
                break;
            }

        }
        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();
        if (pnlexp_and_Manf.Visible == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Close_Modal_Popup()", true);
        }
    }
    //public static string[] isSerialNumberValid_PurchaseReturn(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber, string InvoiceId, string PoId)
    //{
    //    Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster();
    //    SystemParameter ObjSysParam = new SystemParameter();
    //    string[] Result = new string[5];


    //    int counter = 0;

    //    foreach (GridViewRow gvrow in gvSerialNumber.Rows)
    //    {
    //        if (((Label)gvrow.FindControl("lblsrno")).Text.Trim() == serialNumber)
    //        {
    //            counter = 1;
    //            break;
    //        }
    //    }

    //    if (counter == 0)
    //    {



    //        DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

    //        try
    //        {
    //            //ViewState["InvoiceId"] 

    //            dtserial = new DataView(dtserial, "SerialNo='" + serialNumber + "' and ProductId=" + ProductId + " and TransType='PG' and TransTypeId=" + InvoiceId + " and Field1='" + PoId + "'", "", DataViewRowState.CurrentRows).ToTable();

    //            //}
    //        }
    //        catch
    //        {
    //        }

    //        if (dtserial.Rows.Count == 0)
    //        {
    //            //if we not found in database with thsi product id that we are allow this serial number
    //            Result[0] = "Not Exist";
    //        }
    //        else
    //        {
    //            if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "O")
    //            {
    //                Result[0] = "ALREADY OUT";
    //            }
    //            if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "I")
    //            {

    //                Result[0] = "VALID";
    //                Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
    //                Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
    //                Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
    //                Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Result[0] = "DUPLICATE";
    //    }

    //    return Result;
    //}
    protected void lnkAddSerial_Command(object sender, CommandEventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";
        Session["RdoType"] = "SerialNo";
        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["PID"] = e.CommandArgument.ToString();
        ViewState["RowIndex"] = Row.RowIndex;
        ViewState["POId"] = ((Label)Row.FindControl("lblPoID")).Text;
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblproductcode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblIProductName")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        gvStockwithManf_and_expiry.DataSource = null;
        gvStockwithManf_and_expiry.DataBind();
        txtSerialNo.Text = "";
        ViewState["dtSerial"] = null;
        DataTable dt = new DataTable();
        if (ViewState["dtFinal"] == null)
        {
        }
        else
        {
            dt = (DataTable)ViewState["dtFinal"];
            dt = new DataView(dt, "ProductId='" + ViewState["PID"].ToString() + "' and POID='" + ViewState["POId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            ViewState["dtSerial"] = dt;
        }
        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "SNO")
        {
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            pnlSerialNumber.Visible = true;
            pnlexp_and_Manf.Visible = false;
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LE" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FE")
        {
            if (ViewState["dtSerial"] == null)
            {
                ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
            }
            else
            {
                if (((DataTable)ViewState["dtSerial"]).Rows.Count == 0)
                {
                    ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
                }
            }
            DataTable dtTemp = (DataTable)ViewState["dtSerial"];
            if (((DataTable)ViewState["dtSerial"]).Rows.Count > 0)
            {

                dtTemp = new DataView(dtTemp, "", "ExpiryDate asc", DataViewRowState.CurrentRows).ToTable();
            }
            gvStockwithManf_and_expiry.DataSource = dtTemp;
            gvStockwithManf_and_expiry.DataBind();
            pnlSerialNumber.Visible = false;
            pnlexp_and_Manf.Visible = true;
            try
            {
                gvStockwithManf_and_expiry.Columns[1].Visible = true;
                gvStockwithManf_and_expiry.Columns[2].Visible = false;
            }
            catch
            {
            }
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LM" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FM")
        {
            if (ViewState["dtSerial"] == null)
            {
                ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
            }
            else
            {
                if (((DataTable)ViewState["dtSerial"]).Rows.Count == 0)
                {
                    ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
                }
            }
            DataTable dtTemp = (DataTable)ViewState["dtSerial"];
            if (((DataTable)ViewState["dtSerial"]).Rows.Count > 0)
            {

                dtTemp = new DataView(dtTemp, "", "ManufacturerDate asc", DataViewRowState.CurrentRows).ToTable();
            }

            gvStockwithManf_and_expiry.DataSource = dtTemp;
            gvStockwithManf_and_expiry.DataBind();
            pnlSerialNumber.Visible = false;
            pnlexp_and_Manf.Visible = true;
            try
            {
                gvStockwithManf_and_expiry.Columns[1].Visible = false;
                gvStockwithManf_and_expiry.Columns[2].Visible = true;
            }
            catch
            {
            }
        }
    
        txtSerialNo.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
    }
    public string setRoundValue(string Value)
    {
        string strRoundValue = string.Empty;
        try
        {
            strRoundValue = Math.Round(float.Parse(Value), 0).ToString();
        }
        catch
        {
        }


        return strRoundValue;


    }
    protected void btnResetSerial_Click(object sender, EventArgs e)
    {
        ViewState["dtSerial"] = null;
        txtSerialNo.Text = "";
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        LoadStores();
        txtCount.Text = "0";
        txtselectedSerialNumber.Text = "0";
        if (ViewState["dtFinal"] != null)
        {
            DataTable Dtfinal = (DataTable)ViewState["dtFinal"];
            if (ViewState["POId"].ToString() == "0")
            {
                Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            else
            {
                Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "' and  POID<>'" + ViewState["POId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                int POId = Convert.ToInt32(ViewState["POId"].ToString());
            }
            ViewState["dtFinal"] = Dtfinal;
        }

        foreach (GridViewRow gvRow in GvInvoiceDetail.Rows)
        {

            TextBox txtReturnQty = (TextBox)gvRow.FindControl("txtReturnQty");


            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                if (ViewState["dtSerial"] != null)
                {
                    txtReturnQty.Text = ((DataTable)ViewState["dtSerial"]).Rows.Count.ToString();
                }
                else
                {
                    txtReturnQty.Text = "0";
                }


                txtReturnQty_TextChanged(((TextBox)gvRow.FindControl("txtReturnQty")), null);


                break;
            }
        }
        txtSerialNo.Focus();
    }
    protected void btncloseserial_Click(object sender, EventArgs e)
    {
        pnlSerialNumber.Visible = false;
        pnlexp_and_Manf.Visible = false;
        //lblDuplicateserialNo.Text = "";
        ViewState["dtSerial"] = null;
    }
    protected void Btnloadfile_Click(object sender, EventArgs e)
    {
        int counter = 0;
        txtSerialNo.Text = "";
        try
        {

            string Path = Server.MapPath("~/Temp/" + FULogoPath.FileName);
            string[] csvRows = System.IO.File.ReadAllLines(Path);
            string[] fields = null;

            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(',');

                if (fields[0].ToString() != "")
                {

                    if (txtSerialNo.Text == "")
                    {
                        txtSerialNo.Text = fields[0].ToString();

                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + fields[0].ToString();
                    }

                    counter++;

                }

            }


            if (Directory.Exists(Path))
            {
                try
                {
                    Directory.Delete(Path);
                }
                catch
                {
                }
            }
            txtCount.Text = counter.ToString();
        }
        catch
        {
            txtSerialNo.Text = "";

            DisplayMessage("File Not Found ,Try Again");

        }
        txtCount.Text = counter.ToString();

        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }


    }
    protected void IbtnDeleteserialNumber_Command(object sender, CommandEventArgs e)
    {
        if (ViewState["dtSerial"] != null)
        {
            DataTable dt = (DataTable)ViewState["dtSerial"];

            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "SerialNo<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            ViewState["dtSerial"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }
    protected void btnsearchserial_Click(object sender, EventArgs e)
    {
        if (txtserachserialnumber.Text != "")
        {
            DataTable dt = new DataTable();
            if (ViewState["dtSerial"] != null)
            {
                dt = (DataTable)ViewState["dtSerial"];

                if (dt.Rows.Count > 0)
                {
                    dt = new DataView(dt, "SerialNo='" + txtserachserialnumber.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }


            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Serial Number Not Found");
                txtserachserialnumber.Text = "";
                txtserachserialnumber.Focus();
                return;
            }
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            btnRefreshserial.Focus();
        }
        else
        {
            DisplayMessage("Enter Serial Number");
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }
    protected void btnRefreshserial_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["dtSerial"] != null)
        {
            dt = (DataTable)ViewState["dtSerial"];

        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");

        txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        txtserachserialnumber.Text = "";
        txtserachserialnumber.Focus();

    }
    protected void gvSerialNumber_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtSerial"];

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtSerial"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
        lblSelectedRecord.Text = "";

    }
    protected void btnexecute_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;

        int counter = 0;
        txtSerialNo.Text = "";


        DataTable dtSockCopy = new DataTable();

        DataTable dtstock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PG", ViewState["InvoiceId"].ToString());

        try
        {




            try
            {
                dtstock = new DataView(dtstock, "ProductId=" + ViewState["PID"].ToString() + "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            dtstock = dtstock.DefaultView.ToTable(true, "SerialNo");


            for (int i = 0; i < dtstock.Rows.Count; i++)
            {

                string[] result = isSerialNumberValid(dtstock.Rows[i]["SerialNo"].ToString(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                if (result[0] == "VALID")
                {
                    if (txtSerialNo.Text == "")
                    {
                        txtSerialNo.Text = dtstock.Rows[i]["SerialNo"].ToString();

                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + dtstock.Rows[i]["SerialNo"].ToString();


                    }
                    counter++;

                }



            }

        }
        catch
        {
        }



        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }




    }
    public string[] isSerialNumberValid(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber)
    {
        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        string[] Result = new string[5];


        int counter = 0;

        foreach (GridViewRow gvrow in gvSerialNumber.Rows)
        {
            if (((Label)gvrow.FindControl("lblsrno")).Text.Trim() == serialNumber)
            {
                counter = 1;
                break;
            }
        }

        if (counter == 0)
        {

            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PG", ViewState["InvoiceId"].ToString());




            try
            {
                dtserial = new DataView(dtserial, "SerialNo='" + serialNumber + "'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }


            DataTable dt = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ProductId, serialNumber);



            if (dtserial.Rows.Count > 0)
            {

                if (dt.Rows[0]["InOut"].ToString() == "O")
                {
                    Result[0] = "ALREADY OUT";
                }
                else if (dt.Rows[0]["InOut"].ToString() == "I")
                {
                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
            }
            else
            {
                Result[0] = "NOT EXISTS";
            }
        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
    }
    public DataTable GetDatatableForPurchaseReturn()
    {
        DataTable dtTemp = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PG", hdnInvoiceId.Value);

        if (dtTemp.Rows.Count > 0)
        {
            dtTemp = new DataView(dtTemp, "TransType='PG' and TransTypeId=" + hdnInvoiceId.Value + " and ProductId=" + ViewState["PID"].ToString() + " and Field1='" + ViewState["POId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dtTemp.Rows.Count > 0)
            {
                dtTemp = dtTemp.DefaultView.ToTable(false, "ProductId", "SerialNo", "Barcode", "BatchNo", "LotNo", "ExpiryDate", "Field1", "TransType", "TransTypeId", "ManufacturerDate", "Quantity", "Trans_Id");
                dtTemp.Columns["Field1"].ColumnName = "POID";
                dtTemp.Columns["Barcode"].ColumnName = "BarcodeNo";
                dtTemp.Columns["Quantity"].ColumnName = "Invoiceqty";
                dtTemp.Columns.Add("Quantity");
                dtTemp.Columns.Add("BatchRefId");
            }

        }
        return dtTemp;

    }
    #region addheadertax

    public void LoadStores()
    {
        DataTable dt = new DataTable();
        if (ViewState["dtSerial"] != null)
        {
            dt = new DataTable();

            dt = (DataTable)ViewState["dtSerial"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvStockwithManf_and_expiry, dt, "", "");
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("ProductId");
                dt.Columns.Add("SerialNo");
                dt.Columns.Add("BarcodeNo");
                dt.Columns.Add("BatchNo");
                dt.Columns.Add("LotNo");
                dt.Columns.Add("ExpiryDate");
                dt.Columns.Add("POID");
                dt.Columns.Add("TransType");
                dt.Columns.Add("TransTypeId");
                dt.Columns.Add("ManufacturerDate");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Trans_Id");
                dt.Columns.Add("Invoiceqty");
                dt.Rows.Add(dt.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvStockwithManf_and_expiry, dt, "", "");
                int TotalColumns = gvStockwithManf_and_expiry.Rows[0].Cells.Count;
                gvStockwithManf_and_expiry.Rows[0].Cells.Clear();
                gvStockwithManf_and_expiry.Rows[0].Cells.Add(new TableCell());
                gvStockwithManf_and_expiry.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvStockwithManf_and_expiry.Rows[0].Visible = false;
            }

        }
        else
        {
            dt.Columns.Add("ProductId");
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("BarcodeNo");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("LotNo");
            dt.Columns.Add("ExpiryDate");
            dt.Columns.Add("POID");
            dt.Columns.Add("TransType");
            dt.Columns.Add("TransTypeId");
            dt.Columns.Add("ManufacturerDate");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Trans_Id");
            dt.Columns.Add("Invoiceqty");
            dt.Rows.Add(dt.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvStockwithManf_and_expiry, dt, "", "");
            int TotalColumns = gvStockwithManf_and_expiry.Rows[0].Cells.Count;
            gvStockwithManf_and_expiry.Rows[0].Cells.Clear();
            gvStockwithManf_and_expiry.Rows[0].Cells.Add(new TableCell());
            gvStockwithManf_and_expiry.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvStockwithManf_and_expiry.Rows[0].Visible = false;
        }
    }



    #endregion
    #endregion
    protected void txtgvStockreturnqty_TextChnaged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;
        if (((TextBox)gvrow.FindControl("txtreturnqty")).Text == "")
        {
            ((TextBox)gvrow.FindControl("txtreturnqty")).Text = "0";
        }

        if (float.Parse(((TextBox)gvrow.FindControl("txtreturnqty")).Text) > float.Parse(((Label)gvrow.FindControl("lblInvoiceqty")).Text))
        {
            DisplayMessage("Return Quantity should not be greater than invoice quantity");
            ((TextBox)gvrow.FindControl("txtreturnqty")).Text = "0";
            ((TextBox)gvrow.FindControl("txtreturnqty")).Focus();
            return;
        }

    }
    #region InvoiceList
    private void FillInvoiceGrid(int currentPageIndex = 1)
    {

        string strSearchCondition = string.Empty;
        if (ddlInvoiceOption.SelectedIndex != 0 && txtInvoiceValue.Text != string.Empty)
        {
            if (ddlInvoiceOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlInvoiceFieldName.SelectedValue + "='" + txtInvoiceValue.Text.Trim() + "'";
            }
            else if (ddlInvoiceOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlInvoiceFieldName.SelectedValue + " like '%" + txtInvoiceValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlInvoiceFieldName.SelectedValue + " Like '" + txtInvoiceValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and isActive='true' and Post='True'";

        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }

        int totalRows = 0;
        using (DataTable dt = objPInvoice.getInvoiceList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPurchaseInvoice.Attributes["CurrentSortField"], GvPurchaseInvoice.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPurchaseInvoice, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblInvoiceTotalRecord.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPurchaseInvoice.DataSource = null;
                GvPurchaseInvoice.DataBind();
                lblInvoiceTotalRecord.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPagerInvoice, totalRows, currentPageIndex);
        }
    }
    protected void rptPagerInvoice_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvInvoiceIndex.Value = pageIndex.ToString();
        FillInvoiceGrid(pageIndex);
    }
    protected void ddlInvoiceFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if ((ddlInvoiceFieldName.SelectedItem.Value == "InvoiceDate") || (ddlInvoiceFieldName.SelectedItem.Value == "SupInvoiceDate"))
        {
            txtInvoiceValueDate.Visible = true;
            txtInvoiceValue.Visible = false;
            txtValue.Text = "";
            txtInvoiceValueDate.Text = "";

        }
        else
        {
            txtInvoiceValueDate.Visible = false;
            txtInvoiceValue.Visible = true;
            txtInvoiceValue.Text = "";
            txtInvoiceValueDate.Text = "";

        }
    }
    protected void btnInvoicebind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if ((ddlInvoiceFieldName.SelectedItem.Value == "InvoiceDate") || (ddlInvoiceFieldName.SelectedItem.Value == "SupInvoiceDate"))
        {
            if (txtInvoiceValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtInvoiceValueDate.Text);
                    txtInvoiceValue.Text = Convert.ToDateTime(txtInvoiceValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtInvoiceValueDate.Text = "";
                    txtInvoiceValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoiceValueDate);
                    return;
                }
                txtInvoiceValueDate.Focus();
            }
            else
            {
                DisplayMessage("Enter Date");
                txtInvoiceValueDate.Focus();
                return;
            }
        }
        FillInvoiceGrid();

    }
    protected void btnInvoiceRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ddlInvoiceFieldName.SelectedIndex = 0;
        ddlInvoiceOption.SelectedIndex = 2;
        txtInvoiceValue.Text = "";
        txtInvoiceValue.Visible = true;
        txtInvoiceValueDate.Visible = false;
        txtInvoiceValueDate.Text = "";
        FillInvoiceGrid();
    }

    protected void GvPurchaseInvoice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvPurchaseInvoice.Attributes["CurrentSortField"] != null &&
            GvPurchaseInvoice.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvPurchaseInvoice.Attributes["CurrentSortField"])
            {
                if (GvPurchaseInvoice.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvPurchaseInvoice.Attributes["CurrentSortField"] = sortField;
        GvPurchaseInvoice.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillInvoiceGrid(Int32.Parse(hdnGvInvoiceIndex.Value));
    }

    protected void btnInvoiceList_Click(object sender, EventArgs e)
    {
        FillInvoiceGrid();
    }
    protected void btnInvoice_Command(object sender, CommandEventArgs e)
    {
        Reset();
        txtPInvoiceNo.Text = e.CommandArgument.ToString();
        txtPInvoiceNo_TextChanged(null, null);
        txtPInvoiceNo.Enabled = false;
        Update_New.Update();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Active()", true);
    }

    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(Amount.ToString()), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }

        return Amountwithsymbol;

    }

    protected void lblgvSupplierName_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + e.CommandArgument.ToString() + "&&Page=PINV','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    #endregion
    protected void Btn_Add_Return_Click(object sender, CommandEventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";
        Session["RdoType"] = "SerialNo";
        GridViewRow Row = (GridViewRow)((Button)sender).Parent.Parent;
        ViewState["PID"] = e.CommandArgument.ToString();
        ViewState["RowIndex"] = Row.RowIndex;
        ViewState["POId"] = ((Label)Row.FindControl("lblPoID")).Text;
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblproductcode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblIProductName")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        gvStockwithManf_and_expiry.DataSource = null;
        gvStockwithManf_and_expiry.DataBind();
        txtSerialNo.Text = "";
        ViewState["dtSerial"] = null;
        DataTable dt = new DataTable();
        if (ViewState["dtFinal"] == null)
        {
        }
        else
        {
            dt = (DataTable)ViewState["dtFinal"];
            dt = new DataView(dt, "ProductId='" + ViewState["PID"].ToString() + "' and POID='" + ViewState["POId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            ViewState["dtSerial"] = dt;
        }
        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "SNO")
        {
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            pnlSerialNumber.Visible = true;
            pnlexp_and_Manf.Visible = false;
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LE" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FE")
        {
            if (ViewState["dtSerial"] == null)
            {
                ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
            }
            else
            {
                if (((DataTable)ViewState["dtSerial"]).Rows.Count == 0)
                {
                    ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
                }
            }
            DataTable dtTemp = (DataTable)ViewState["dtSerial"];
            if (((DataTable)ViewState["dtSerial"]).Rows.Count > 0)
            {

                dtTemp = new DataView(dtTemp, "", "ExpiryDate asc", DataViewRowState.CurrentRows).ToTable();
            }
            gvStockwithManf_and_expiry.DataSource = dtTemp;
            gvStockwithManf_and_expiry.DataBind();
            pnlSerialNumber.Visible = false;
            pnlexp_and_Manf.Visible = true;
            try
            {
                gvStockwithManf_and_expiry.Columns[1].Visible = true;
                gvStockwithManf_and_expiry.Columns[2].Visible = false;
            }
            catch
            {
            }
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LM" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FM")
        {
            if (ViewState["dtSerial"] == null)
            {
                ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
            }
            else
            {
                if (((DataTable)ViewState["dtSerial"]).Rows.Count == 0)
                {
                    ViewState["dtSerial"] = GetDatatableForPurchaseReturn();
                }
            }
            DataTable dtTemp = (DataTable)ViewState["dtSerial"];
            if (((DataTable)ViewState["dtSerial"]).Rows.Count > 0)
            {

                dtTemp = new DataView(dtTemp, "", "ManufacturerDate asc", DataViewRowState.CurrentRows).ToTable();
            }

            gvStockwithManf_and_expiry.DataSource = dtTemp;
            gvStockwithManf_and_expiry.DataBind();
            pnlSerialNumber.Visible = false;
            pnlexp_and_Manf.Visible = true;
            try
            {
                gvStockwithManf_and_expiry.Columns[1].Visible = false;
                gvStockwithManf_and_expiry.Columns[2].Visible = true;
            }
            catch
            {
            }
        }
    
        txtSerialNo.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }
    public string SetDecimal(string amount)
    {
        return SystemParameter.GetAmountWithDecimal(amount, Session["LoginLocDecimalCount"].ToString());
    }
    public string Convert_Into_DF(string Amount)
    {
        try
        {
            if (Amount == "")
                Amount = "0";

            if (Decimal_Count_For_Tax.ToString() == "")
                Decimal_Count_For_Tax = 0;

            if (Decimal_Count_For_Tax == 0)
            {
                string Decimal_Count = string.Empty;
                Decimal_Count = Session["LoginLocDecimalCount"].ToString();
                Decimal_Count_For_Tax = Convert.ToInt32(Decimal_Count);
            }

            decimal Amount_D = Convert.ToDecimal((Convert.ToDouble(Amount)).ToString("F7"));
            Amount = Amount_D.ToString();
            int index = Amount.ToString().LastIndexOf(".");
            if (index > 0)
                Amount = Amount.Substring(0, index + (Decimal_Count_For_Tax + 1));
            return Amount;
        }
        catch
        {
            return "0";
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
}
