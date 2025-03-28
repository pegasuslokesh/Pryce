using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlClient;
using PegasusDataAccess;
public partial class Purchase_PurchaseQuatation : System.Web.UI.Page
{
    #region defined Class Object
    Inv_TaxRefDetail objTaxRefDetail = null;
    Common cmn = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseQuoteDetail ObjQuoteDetail = null;
    Inv_PurchaseInquiryHeader objPIHeader = null;
    Inv_PurchaseInquiryDetail ObjPIDetail = null;
    Inv_PurchaseInquiry_Supplier objPISupplier = null;
    PurchaseRequestHeader ObjPRHeader = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Inv_ProductMaster ObjProductMaster = null;
    Set_DocNumber objDocNo = null;
    Inv_SalesInquiryDetail ObjSalesInqdetail = null;
    Inv_SalesInquiryHeader ObjSalesInquiryHeader = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    Inv_ParameterMaster objInvParam = null;
    Contact_PriceList objCustomerPriceList = null;
    Set_Suppliers objSupplier = null;
    Country_Currency objCountryCurrency = null;
    Inv_StockDetail objStockDetail = null;
    EmployeeMaster objEmployee = null;
    DepartmentMaster ObjDept = null;
    LocationMaster objLocation = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string StrUserId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        this.MaintainScrollPositionOnPostBack = true;
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjQuoteDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        objPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        ObjPIDetail = new Inv_PurchaseInquiryDetail(Session["DBConnection"].ToString());
        objPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        ObjPRHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjSalesInqdetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        ObjSalesInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objCustomerPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        StrUserId = Session["UserId"].ToString();
        btnQuoteSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
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
            Session["Temp_Product_Tax_PQ"] = null;
            txtRPQNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtRPQNo.Text;
            ddlOption.SelectedIndex = 2;
            btnList_Click(null, null);
            FillGrid();
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueRequestDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            txtRPQDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillCurrency();
            FillTransactionType();
            DataTable Dt_Individual = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "Allow TAX edit on individual transactions Purchase");
            if (Dt_Individual.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt_Individual.Rows[0]["ParameterValue"]) == true)
                    Btn_Update_Tax.Visible = true;
                else
                    Btn_Update_Tax.Visible = false;
            }
            else
            {
                Btn_Update_Tax.Visible = false;
            }
        }
        if (!Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            Trans_Div.Visible = false;
            ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", "DL_Tax_Hide();", true);
        }
        if (!Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", "DL_Discount_Hide();", true);

        Page.MaintainScrollPositionOnPostBack = true;
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void SetDecimalFormat()
    {
        foreach (GridViewRow gvRow in GvSupplier.Rows)
        {
            Label lblGvAmount = (Label)gvRow.FindControl("lblgvAmount");
            if (lblGvAmount.Text == "")
            {
                lblGvAmount.Text = "0";
            }
            lblGvAmount.Text = GetAmountDecimal(lblGvAmount.Text.ToString());
        }
        foreach (DataListItem items in dlProductDetail.Items)
        {
            TextBox gvtxtUnitPrice = (TextBox)items.FindControl("txtUnitPrice");
            Label gvtxtQtyPrice = (Label)items.FindControl("txtQtyPrice");
            TextBox gvtxtTaxP = (TextBox)items.FindControl("txtTaxP");
            TextBox gvtxtTaxV = (TextBox)items.FindControl("txtTaxV");
            TextBox txtDiscountP = (TextBox)items.FindControl("txtDiscountP");
            TextBox gvtxtPriceAfterTax = (TextBox)items.FindControl("txtPriceAfterTax");
            TextBox gvtxtDiscountV = (TextBox)items.FindControl("txtDiscountV");
            TextBox gvtxtAmount = (TextBox)items.FindControl("txtAmount");
            try
            {
                gvtxtUnitPrice.Text = GetAmountDecimal(gvtxtUnitPrice.Text);
                gvtxtQtyPrice.Text = GetAmountDecimal(gvtxtQtyPrice.Text);
                gvtxtTaxV.Text = GetAmountDecimal(gvtxtTaxV.Text);
                gvtxtTaxP.Text = GetAmountDecimal(gvtxtTaxP.Text);
                gvtxtPriceAfterTax.Text = GetAmountDecimal(gvtxtPriceAfterTax.Text);
                gvtxtDiscountV.Text = GetAmountDecimal(gvtxtDiscountV.Text);
                txtDiscountP.Text = GetAmountDecimal(txtDiscountP.Text);
                gvtxtAmount.Text = GetAmountDecimal(gvtxtAmount.Text);
            }
            catch
            {
            }
        }
        foreach (DataListItem items in datalistProduct.Items)
        {
            GridView gvSupplier = (GridView)items.FindControl("gvSupplier");
            foreach (GridViewRow gvrow in gvSupplier.Rows)
            {
                Label lblAmmount = (Label)gvrow.FindControl("lblAmmount");
                lblAmmount.Text = GetAmountDecimal(lblAmmount.Text);
            }
        }
    }
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        return SystemParameter.GetCurrencySmbol(CurrencyId, SystemParameter.GetAmountWithDecimal(Amount, Session["LoginLocDecimalCount"].ToString()), Session["DBConnection"].ToString());
    }
    public void FillCurrency()
    {
        try
        {
            using (DataTable dt = objCurrency.GetCurrencyMaster())
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");
            }
        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;
        }
    }
    //#region System defined Function
    protected void btnList_Click(object sender, EventArgs e)
    {
        PnlNewEdit.Visible = false;
        pnlProduct.Visible = false;
        pnlSalesQuotation.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        PnlNewEdit.Visible = true;
        pnlProduct.Visible = false;
        pnlSalesQuotation.Visible = false;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }

        ddlTransType.Enabled = false;
        editid.Value = e.CommandArgument.ToString();
        using (DataTable dtQuoteEdit = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, editid.Value))
        {
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            if (dtQuoteEdit.Rows.Count > 0)
            {
                txtRPQNo.Text = dtQuoteEdit.Rows[0]["RPQ_No"].ToString();
                ViewState["CurrencyId"] = dtQuoteEdit.Rows[0]["Field1"].ToString();
                txtRPQNo.ReadOnly = true;
                txtRPQDate.Text = Convert.ToDateTime(dtQuoteEdit.Rows[0]["RPQ_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                //Add Inquiry Data
                string strPI_Id = dtQuoteEdit.Rows[0]["PI_No"].ToString();
                hdnPIId.Value = strPI_Id;
                if (strPI_Id != "" && strPI_Id != "0")
                {
                    using (DataTable dtInquiry = objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value))
                    {
                        if (dtInquiry.Rows.Count > 0)
                        {
                            ddlCurrency.SelectedValue = dtInquiry.Rows[0]["Field2"].ToString();
                            txtInquiryNo.Text = dtInquiry.Rows[0]["PI_No"].ToString();
                            txtInquiryDate.Text = Convert.ToDateTime(dtInquiry.Rows[0]["PIDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        }
                    }
                }
                //Add Supplier Section For Edit
                using (DataTable dtSupplier = objPISupplier.GetAllPISupplierWithPI_NoandAmount(StrCompId, StrBrandId, Session["LocId"].ToString(), strPI_Id))
                {
                    if (dtSupplier != null && dtSupplier.Rows.Count > 0)
                    {
                        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                        objPageCmn.FillData((object)GvSupplier, dtSupplier, "", "");
                        ViewState["DefaultCurrency"] = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), Resources.Attendance.Amount, Session["DBConnection"].ToString());
                        if (GvSupplier != null && GvSupplier.Rows.Count > 0)
                        {
                            if (ViewState["DefaultCurrency"] != null)
                            {
                                GvSupplier.HeaderRow.Cells[3].Text = ViewState["DefaultCurrency"].ToString();
                            }
                        }
                    }
                }
            }
        }
        btnNew_Click(null, null);
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
        SetDecimalFormat();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        FillGrid();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "RPQ_Date")
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
    protected void GvPurchaseQuote_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvPurchaseQuote.Attributes["CurrentSortField"] != null &&
            GvPurchaseQuote.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvPurchaseQuote.Attributes["CurrentSortField"])
            {
                if (GvPurchaseQuote.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvPurchaseQuote.Attributes["CurrentSortField"] = sortField;
        GvPurchaseQuote.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnquotationListIndex.Value));
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnquotationListIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        //code added by jitendra upadhyay on  //29-11-2018// for check that po generated or not against this quotation
        //code start
        if (objDa.return_DataTable("select PONO from inv_purchaseorderheader where ReferenceVoucherType='PQ' and ReferenceID=" + e.CommandArgument.ToString() + "").Rows.Count > 0)
        {
            DisplayMessage("Quotation is used in purchase order ,can not Delete this record");
            return;
        }
        //code end
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objQuoteHeader.DeleteQuoteHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, "false", StrUserId, DateTime.Now.ToString());
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
    protected void btnQuoteCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        FillGrid();
        PnlNewEdit.Visible = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
    }
    protected void btnQuoteSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (txtRPQDate.Text == "")
        {
            DisplayMessage("Enter Purchase Quotation Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQDate);
            btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtRPQDate.Text);
            }
            catch
            {
                DisplayMessage("Entern Purchase Quotation Date in format " + Session["DateFormat"].ToString() + "");
                txtRPQDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQDate);
                btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
                return;
            }
        }
        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtRPQDate.Text), "I", Session["DBConnection"].ToString(), Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
            return;
        }
        if (txtInquiryNo.Text.Trim() == "")
        {
            DisplayMessage("Choose Record In Inquiry Section For Create Quotation");
            btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
            return;
        }
        if (txtRPQNo.Text == "")
        {
            DisplayMessage("Enter Purchase Quotation No.");
            btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
            return;
        }
        int Amount_Value = 0;
        foreach (GridViewRow GVR in GvSupplier.Rows)
        {
            Label Amount = (Label)GVR.FindControl("lblgvAmount");
            if (Convert.ToDouble(Amount.Text) != 0)
            {
                Amount_Value++;
            }
        }
        //if (Amount_Value == 0)
        //{
        //    ddlTransType.Enabled = true;
        //    DisplayMessage("Amount should not be zero!!!");
        //    btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
        //    return;
        //}
        //else
        {
            if (editid.Value == "")
            {
                //uncommnet by jitendra Upadhyay
                DataTable dtPINo = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text);
                if (dtPINo.Rows.Count > 0)
                {
                    DisplayMessage("Purchase Quotation No. Already Exists");
                    txtRPQNo.Text = "";
                    btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
                    return;
                }
            }
            else
            {
                DataTable dtPINo = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text);
                if (dtPINo.Rows.Count > 0)
                {
                    if (dtPINo.Rows[0]["Trans_Id"].ToString() != editid.Value)
                    {
                        DisplayMessage("Purchase Quotation No. Already Exists");
                        txtRPQNo.Text = "";
                        btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
                        return;
                    }
                }
            }
        }
        DataTable dt = objPIHeader.GetPIHeaderAllDataByPI_No(StrCompId, StrBrandId, StrLocationId, txtInquiryNo.Text.Trim());
        if (dt.Rows.Count != 0)
        {
            Session["TransFrom"] = dt.Rows[0]["TransFrom"].ToString();
            Session["SalesInquiryId"] = dt.Rows[0]["TransNo"].ToString();
        }
        if (Session["TransFrom"].ToString() != null)
        {
            if (Session["TransFrom"].ToString() == "SI")
            {
                try
                {
                    fillDataList(txtRPQNo.Text.Trim());
                }
                catch
                {
                    DisplayMessage("Enetr detail Information");
                    return;
                }
                PnlNewEdit.Visible = false;
                pnlProduct.Visible = false;
                pnlSalesQuotation.Visible = true;
                pnlProduct.Visible = false;
            }
            else
            {
                Session["SalesInquiryId"] = null;
            }
        }
        double TotalAmount = 0;
        if (hdnPIId.Value != "0")
        {
            if (GvSupplier.Rows.Count > 0)
            {
                foreach (GridViewRow gvRow in GvSupplier.Rows)
                {
                    try
                    {
                        TotalAmount += Convert.ToDouble(((Label)gvRow.FindControl("lblgvAmount")).Text);
                    }
                    catch
                    {
                        TotalAmount += 0;
                    }
                }
            }
            else
            {
                DisplayMessage("You have no supplier");
                return;
            }
        }
        else if (hdnPIId.Value == "0")
        {
            DisplayMessage("Choose Record In Inquiry Section For Create Quotation");
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
                b = objQuoteHeader.UpdateQuoteHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtRPQNo.Text, ObjSysParam.getDateForInput(txtRPQDate.Text).ToString(), hdnPIId.Value, TotalAmount.ToString(), ViewState["CurrencyId"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                string Status = string.Empty;
                if (Session["TransFrom"].ToString() != null)
                {
                    if (Session["TransFrom"].ToString() == "SI")
                    {
                        Status = "Quotation Come From Supplier(Belong to sales)";
                    }
                    else
                    {
                        Status = "Quotation Come From Supplier";
                    }
                }
                else
                {
                    Status = "Quotation Come From Supplier";
                }
                objQuoteHeader.UpdateQuoteHeaderStatus(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value, Status, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                objPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value, "Quotation Come From Supplier", StrUserId, DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    if (Session["TransFrom"].ToString() == "SI")
                    {
                    }
                    else
                    {
                        DisplayMessage("Record Updated", "green");
                        btnList_Click(null, null);
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
                b = objQuoteHeader.InsertQuoteHeader(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text, ObjSysParam.getDateForInput(txtRPQDate.Text).ToString(), hdnPIId.Value, TotalAmount.ToString(), ViewState["CurrencyId"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                if (txtRPQNo.Text.Trim() == ViewState["DocNo"].ToString().Trim())
                {
                    int Quotationcount = objQuoteHeader.GetQuotationCountByLocationId1(Session["LocId"].ToString(), ref trns);
                    if (Quotationcount == 0)
                    {
                        objQuoteHeader.Updatecode(b.ToString(), txtRPQNo.Text + "1", ref trns);
                        txtRPQNo.Text = txtRPQNo.Text + "1";
                    }
                    else
                    {
                        if (objDa.return_DataTable("select RPQ_no from Inv_PurchaseQuoteHeader where location_id=" + Session["LocId"].ToString() + " and RPQ_no='" + txtRPQNo.Text.Trim() + Quotationcount.ToString() + "'").Rows.Count > 0)
                        {
                            bool bCodeFlag = true;
                            while (bCodeFlag)
                            {
                                Quotationcount += 1;
                                if (objDa.return_DataTable("select RPQ_no from Inv_PurchaseQuoteHeader where location_id=" + Session["LocId"].ToString() + " and RPQ_no='" + txtRPQNo.Text.Trim() + Quotationcount.ToString() + "'").Rows.Count == 0)
                                {
                                    bCodeFlag = false;
                                }
                            }
                        }
                        objQuoteHeader.Updatecode(b.ToString(), txtRPQNo.Text + Quotationcount.ToString(), ref trns);
                        txtRPQNo.Text = txtRPQNo.Text + Quotationcount.ToString();
                    }
                }
                using (DataTable dtQuotHeader = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text, ref trns))
                {
                    string Status = string.Empty;
                    if (Session["TransFrom"].ToString() != null)
                    {
                        if (Session["TransFrom"].ToString() == "SI")
                        {
                            Status = "Quotation Come From sales";
                        }
                        else
                        {
                            Status = "Quotation Come From Supplier";
                        }
                    }
                    else
                    {
                        Status = "Quotation Come From Supplier";
                    }
                    objQuoteHeader.UpdateQuoteHeaderStatus(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value, Status, Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    objPIHeader.UpdatePIHeaderStatus(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value, "Send To Supplier", StrUserId, DateTime.Now.ToString(), ref trns);
                }
                if (b != 0)
                {
                    DisplayMessage("Record Saved","green");
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
            FillGrid();
            btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
        }
        catch (Exception ex)
        {
            DisplayMessage(ex.Message.ToString().Replace("'", " ") + " Line Number : " + Common.GetLineNumber(ex));
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
    //#region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        PnlNewEdit.Visible = false;
        pnlProduct.Visible = false;
        pnlSalesQuotation.Visible = false;
        FillGridBin();
    }
    protected void btnPRequest_Click(object sender, EventArgs e)
    {
        PnlNewEdit.Visible = false;
        pnlProduct.Visible = false;
        pnlSalesQuotation.Visible = false;
        FillRequestGrid();
    }
    protected void GvPurchaseQuoteBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvPurchaseQuoteBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterPQBin"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseQuoteBin, dt, "", "");
        string temp = string.Empty;
        for (int i = 0; i < GvPurchaseQuoteBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvPurchaseQuoteBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvPurchaseQuoteBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        dt = null;
    }
    protected void GvPurchaseQuoteBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilterPQBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvPurchaseQuoteBin, dt, "", "");
        lblSelectedRecord.Text = "";
        dt = null;
    }
    public void FillGridBin()
    {
        using (DataTable dt = objQuoteHeader.GetQuoteHeaderAllFalse(StrCompId, StrBrandId, StrLocationId))
        {
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseQuoteBin, dt, "", "");
            Session["dtPQBin"] = dt;
            Session["dtFilterPQBin"] = dt;
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            lblSelectedRecord.Text = "";
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "RPQ_Date")
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
            DataTable dtPQBin = (DataTable)Session["dtPQBin"];
            DataView view = new DataView(dtPQBin, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilterPQBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseQuoteBin, view.ToTable(), "", "");
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
        CheckBox chkSelAll = ((CheckBox)GvPurchaseQuoteBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvPurchaseQuoteBin.Rows.Count; i++)
        {
            ((CheckBox)GvPurchaseQuoteBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvPurchaseQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvPurchaseQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvPurchaseQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
        HiddenField lb = (HiddenField)GvPurchaseQuoteBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvPurchaseQuoteBin.Rows[index].FindControl("chkSelect")).Checked)
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
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtPQBin"];
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
            for (int i = 0; i < GvPurchaseQuoteBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvPurchaseQuoteBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvPurchaseQuoteBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtPQBin"];
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseQuoteBin, dtUnit1, "", "");
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
                    b = objQuoteHeader.DeleteQuoteHeader(StrCompId, StrBrandId, StrLocationId, lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            foreach (GridViewRow Gvr in GvPurchaseQuoteBin.Rows)
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
    //#endregion
    //#endregion
    //#region User defind Funcation
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
        if (ddlPosted.SelectedIndex != 2)
        {
            strWhereClause = strWhereClause + " and QuotationStatus='" + ddlPosted.SelectedValue.Trim() + "'";
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = objQuoteHeader.getQuotationList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPurchaseQuote.Attributes["CurrentSortField"], GvPurchaseQuote.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPurchaseQuote, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPurchaseQuote.DataSource = null;
                GvPurchaseQuote.DataBind();
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
        ddlTransType.Enabled = true;
        txtRPQNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtRPQNo.Text;
        // txtRPQNo.Text = objQuoteHeader.GetAutoID(StrCompId, StrBrandId, StrLocationId);
        txtRPQDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtRPQNo.ReadOnly = false;
        txtInquiryNo.Text = "";
        txtInquiryDate.Text = "";
        GvSupplier.DataSource = null;
        GvSupplier.DataBind();
        dlProductDetail.DataSource = null;
        dlProductDetail.DataBind();
        hdnPIId.Value = "0";
        hdnSupplierId.Value = "0";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        ddlFieldNameBin.SelectedIndex = 0;
        ddlOptionBin.SelectedIndex = 2;
        txtValueBin.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        ddlFieldNameRequest.SelectedIndex = 0;
        ddlOptionRequest.SelectedIndex = 2;
        txtValueRequest.Text = "";
        txtValueRequestDate.Text = "";
        txtValueRequestDate.Visible = false;
        txtValueRequest.Visible = true;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
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
        FillRequestGrid(Int32.Parse(hdnInquiryIndexValue.Value));
    }
    protected void RPTPager_Inquiry_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnInquiryIndexValue.Value = pageIndex.ToString();
        FillRequestGrid(pageIndex);
    }
    private void FillRequestGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOptionRequest.SelectedIndex != 0 && txtValueRequest.Text != string.Empty)
        {
            if (ddlOptionRequest.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldNameRequest.SelectedValue + "='" + txtValueRequest.Text.Trim() + "'";
            }
            else if (ddlOptionRequest.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldNameRequest.SelectedValue + " like '%" + txtValueRequest.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldNameRequest.SelectedValue + " Like '" + txtValueRequest.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and isActive='true' and Trans_id not in (select pi_no from Inv_PurchaseQuoteHeader)";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = objPIHeader.getInquiryListForQuotation((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPurchaseInquiry.Attributes["CurrentSortField"], GvPurchaseInquiry.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPurchaseInquiry, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPurchaseInquiry.DataSource = null;
                GvPurchaseInquiry.DataBind();
                lblTotalRecordsRequest.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(RPTPager_Inquiry, totalRows, currentPageIndex);
        }
    }
    protected void btnPREdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        FillTransactionType();
        ddlTransType.Enabled = true;
        string strRequestId = e.CommandArgument.ToString();
        using (DataTable dtPRequest = objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, strRequestId))
        {
            if (dtPRequest.Rows.Count > 0)
            {
                txtInquiryNo.Text = dtPRequest.Rows[0]["PI_No"].ToString();
                txtInquiryDate.Text = Convert.ToDateTime(dtPRequest.Rows[0]["PIDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                hdnPIId.Value = dtPRequest.Rows[0]["Trans_Id"].ToString();
                Session["TransFrom"] = dtPRequest.Rows[0]["TransFrom"].ToString();
                Session["SalesInquiryId"] = dtPRequest.Rows[0]["TransNo"].ToString();
                ViewState["CurrencyId"] = dtPRequest.Rows[0]["Field2"].ToString();
                ddlCurrency.SelectedValue = dtPRequest.Rows[0]["Field2"].ToString();
                //Add Supplier Grid
                DataTable dtSupplier = objPISupplier.GetAllPISupplierWithPI_NoandAmount(StrCompId, StrBrandId, Session["LocId"].ToString(), strRequestId);
                if (dtSupplier.Rows.Count > 0)
                {//this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)GvSupplier, dtSupplier, "", "");
                    GvSupplier.HeaderRow.Cells[3].Text = SystemParameter.GetCurrencySmbol(dtPRequest.Rows[0]["Field2"].ToString(), Resources.Attendance.Amount, Session["DBConnection"].ToString());
                }
            }
        }
        btnNew_Click(null, null);
        editid.Value = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_New_Request_Active()", true);
    }
    //#endregion
    //#region Supplier Section
   
    protected void ImgSupplierDetail_Command(object sender, CommandEventArgs e)
    {
        if (txtRPQDate.Text == "")
        {
            DisplayMessage("Enter Purchase Quotation Date");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQDate);
            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtRPQDate.Text);
            }
            catch
            {
                DisplayMessage("Entern Purchase Quotation Date in format " + Session["DateFormat"].ToString() + "");
                txtRPQDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQDate);
                return;
            }
        }
        if (txtRPQNo.Text == "")
        {
            DisplayMessage("Enter Purchase Quotation No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
            return;
        }
        else
        {
            if (editid.Value == "")
            {
                //uncommnet by jitendra Upadhyay
                DataTable dtPINo = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text);
                if (dtPINo.Rows.Count > 0)
                {
                    if (dtPINo.Rows[0]["Trans_Type"].ToString() != "")
                    {
                        ddlTransType.SelectedValue = dtPINo.Rows[0]["Trans_Type"].ToString();
                    }
                    DisplayMessage("Purchase Quotation No. Already Exists");
                    txtRPQNo.Focus();
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
                    return;
                }
            }
            else
            {
                using (DataTable dtPINo = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text))
                {
                    if (dtPINo.Rows.Count > 0)
                    {
                        if (dtPINo.Rows[0]["Trans_Type"].ToString() != "")
                        {
                            ddlTransType.SelectedValue = dtPINo.Rows[0]["Trans_Type"].ToString();
                        }
                        if (dtPINo.Rows[0]["Trans_Id"].ToString() != editid.Value)
                        {
                            DisplayMessage("Purchase Quotation No. Already Exists");
                            txtRPQNo.Focus();
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRPQNo);
                            return;
                        }
                    }
                }
            }
        }
        hdnSupplierId.Value = e.CommandArgument.ToString();
        using (DataTable DtSup = objSupplier.GetSupplierAllDataBySupplierId(StrCompId, StrBrandId, hdnSupplierId.Value))
        {
            if (DtSup.Rows.Count > 0)
            {
                lblsupplierName.Text = DtSup.Rows[0]["Name"].ToString();
                lblSuppliercode.Text = DtSup.Rows[0]["Supplier_Code"].ToString();
            }
        }
        using (DataTable dtProductDetail = ObjPIDetail.GetPIDetailByPI_No(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value))
        {
            if (dtProductDetail.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)dlProductDetail, dtProductDetail, "", "");
                pnlProduct.Visible = true;
                PnlNewEdit.Visible = false;
                pnlSalesQuotation.Visible = false;
                foreach (DataListItem dl in dlProductDetail.Items)
                {
                    Label Label19 = (Label)dl.FindControl("Label19");
                    HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                    HiddenField hdnsuggestedProductName = (HiddenField)dl.FindControl("hdnSuggestedProductName");
                    Label lblProductName = (Label)dl.FindControl("txtProductName");
                    Label lblProductDescription = (Label)dl.FindControl("txtProductDescription");
                    Label lblTaxP = (Label)dl.FindControl("lblTaxP");
                    TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
                    Label lblTaxV = (Label)dl.FindControl("Label2");
                    ImageButton BtnAddTax = (ImageButton)dl.FindControl("BtnAddTax");
                    TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
                    Label lbltaxcolon = (Label)dl.FindControl("lbltaxcolon");
                    Label lblPriceAfterTax = (Label)dl.FindControl("lblPriceAfterTax");
                    TextBox txtPriceAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
                    Label lblPriceaftertaxcolon = (Label)dl.FindControl("lblPriceaftertaxcolon");
                    Label lblDiscountP = (Label)dl.FindControl("lblDiscountP");
                    TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
                    Label lbldiscountv = (Label)dl.FindControl("Label10");
                    TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
                    Label lbldiscountpcolon = (Label)dl.FindControl("lbldiscountpcolon");
                    TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
                    Label txtQtyPrice = (Label)dl.FindControl("txtQtyPrice");
                    Label txtReqQty = (Label)dl.FindControl("txtRequiredQuantity");
                    TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
                    //this code is created by jitendra upadhyay on 15-03-2014
                    //this code for get the unit price according the supplier price list of selected supplier
                    DataTable dtContactPriceList = objCustomerPriceList.GetContactPriceList(StrCompId, e.CommandArgument.ToString(), "S");
                    try
                    {
                        dtContactPriceList = new DataView(dtContactPriceList, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    txtUnitPrice.Text = "0";
                    if (txtUnitPrice.Text != "" && txtReqQty.Text != "")
                    {
                        txtQtyPrice.Text = (Convert.ToDouble(txtUnitPrice.Text) * Convert.ToDouble(txtReqQty.Text)).ToString();
                        txtAmount.Text = txtQtyPrice.Text;
                    }
                    else
                    {
                        txtQtyPrice.Text = "";
                        txtAmount.Text = "";
                    }
                    lblTaxP.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    txtTaxP.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    Label2.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    lblTaxV.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    BtnAddTax.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    txtTaxV.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    Label19.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    lblPriceAfterTax.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    txtPriceAfterTax.Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Tax_Hide()", true);
                    lblDiscountP.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    txtDiscountP.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    lbldiscountv.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    txtDiscountV.Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    if (hdnProductId.Value == "0")
                    {
                        lblProductName.Text = hdnsuggestedProductName.Value;
                    }
                    Get_Product_Details(dl);
                }
            }
            else
            {
                dlProductDetail.DataSource = null;
                dlProductDetail.DataBind();
            }
        }
        //add edit Detail
        if (editid.Value == "")
        {
            if (txtRPQNo.Text != "")
            {
                using (DataTable dtData = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text))
                {
                    if (dtData.Rows.Count > 0)
                    {
                        editid.Value = dtData.Rows[0]["Trans_Id"].ToString();
                        foreach (DataListItem dl in dlProductDetail.Items)
                        {
                            HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                            TextBox txtRefrencedPName = (TextBox)dl.FindControl("txtRefProductName");
                            TextBox txtRefPartNo = (TextBox)dl.FindControl("txtRefPartNo");
                            HiddenField hdnUnitId = (HiddenField)dl.FindControl("hdngvUnitId");
                            Label txtReqQty = (Label)dl.FindControl("txtRequiredQuantity");
                            TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
                            TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
                            TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
                            TextBox txtAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
                            TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
                            TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
                            TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
                            TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
                            TextBox txtTermsCondition = (TextBox)dl.FindControl("txtTermsCondition");
                            HiddenField hdnsuggestedProductName = (HiddenField)dl.FindControl("hdnSuggestedProductName");
                            DataTable dtChildData = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, editid.Value);
                            if (dtChildData.Rows.Count > 0)
                            {
                                if (hdnProductId.Value == "0")
                                {
                                    dtChildData = new DataView(dtChildData, "Supplier_Id='" + hdnSupplierId.Value + "' and Product_Id='" + hdnProductId.Value + "' and SuggestedProductName='" + hdnsuggestedProductName.Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                else
                                {
                                    dtChildData = new DataView(dtChildData, "Supplier_Id='" + hdnSupplierId.Value + "' and Product_Id='" + hdnProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                                }
                                if (dtChildData.Rows.Count > 0)
                                {
                                    txtRefrencedPName.Text = dtChildData.Rows[0]["RefrencedProductName"].ToString();
                                    txtRefPartNo.Text = dtChildData.Rows[0]["RefrencedPartNo"].ToString();
                                    txtUnitPrice.Text = dtChildData.Rows[0]["UnitPrice"].ToString();
                                    txtTaxP.Text = dtChildData.Rows[0]["TaxPercentage"].ToString();
                                    txtTaxV.Text = dtChildData.Rows[0]["TaxValue"].ToString();
                                    txtAfterTax.Text = dtChildData.Rows[0]["PriceAfterTax"].ToString();
                                    txtDiscountP.Text = dtChildData.Rows[0]["DisPercentage"].ToString();
                                    txtDiscountV.Text = dtChildData.Rows[0]["DiscountValue"].ToString();
                                    txtTermsCondition.Text = dtChildData.Rows[0]["TermsCondition"].ToString();
                                    txtVendorQuotationNo.Text = dtChildData.Rows[0]["Field1"].ToString();
                                    ddlCurrency.SelectedValue = dtChildData.Rows[0]["Field4"].ToString();
                                    txtAmount.Text = dtChildData.Rows[0]["Amount"].ToString();
                                    txtNetAmountPICurrency.Text = dtChildData.Rows[0]["Field5"].ToString();
                                }
                            }
                            Get_Product_Details(dl);
                        }
                    }
                    else
                    {
                        //get currency from, purchase inquiry header table
                        DataTable dtPIEdit = objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPIId.Value);
                        if (dtPIEdit.Rows.Count > 0)
                        {
                            ddlCurrency.SelectedValue = dtPIEdit.Rows[0]["Field2"].ToString();
                        }
                    }
                }
            }
        }
        else
        {
            foreach (DataListItem dl in dlProductDetail.Items)
            {
                HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                TextBox txtRefrencedPName = (TextBox)dl.FindControl("txtRefProductName");
                TextBox txtRefPartNo = (TextBox)dl.FindControl("txtRefPartNo");
                HiddenField hdnUnitId = (HiddenField)dl.FindControl("hdngvUnitId");
                Label txtReqQty = (Label)dl.FindControl("txtRequiredQuantity");
                TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
                Label txtQtyPrice = (Label)dl.FindControl("txtQtyPrice");
                TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
                TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
                TextBox txtAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
                TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
                TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
                TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
                TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
                TextBox txtTermsCondition = (TextBox)dl.FindControl("txtTermsCondition");
                HiddenField hdnsuggestedProductName = (HiddenField)dl.FindControl("hdnSuggestedProductName");
                HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
                DataTable dtChildData = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, editid.Value);
                if (dtChildData.Rows.Count > 0)
                {
                    if (hdnProductId.Value == "0")
                    {
                        dtChildData = new DataView(dtChildData, "Supplier_Id='" + hdnSupplierId.Value + "' and Product_Id='" + hdnProductId.Value + "' and Field3='" + hdnSerialNo.Value + "' and SuggestedProductName='" + hdnsuggestedProductName.Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dtChildData = new DataView(dtChildData, "Supplier_Id='" + hdnSupplierId.Value + "' and Product_Id='" + hdnProductId.Value + "'  and Field3='" + hdnSerialNo.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    if (dtChildData.Rows.Count > 0)
                    {
                        txtRefrencedPName.Text = dtChildData.Rows[0]["RefrencedProductName"].ToString();
                        txtRefPartNo.Text = dtChildData.Rows[0]["RefrencedPartNo"].ToString();
                        txtUnitPrice.Text = dtChildData.Rows[0]["UnitPrice"].ToString();
                        if (ddlTransType.SelectedIndex != 0)
                        {
                            txtTaxP.Text = dtChildData.Rows[0]["TaxPercentage"].ToString();
                            txtTaxV.Text = dtChildData.Rows[0]["TaxValue"].ToString();
                        }
                        else
                        {
                            txtTaxP.Text = "0.00";
                            txtTaxV.Text = "0.00";
                        }
                        txtAfterTax.Text = dtChildData.Rows[0]["PriceAfterTax"].ToString();
                        txtDiscountP.Text = dtChildData.Rows[0]["DisPercentage"].ToString();
                        txtDiscountV.Text = dtChildData.Rows[0]["DiscountValue"].ToString();
                        txtTermsCondition.Text = dtChildData.Rows[0]["TermsCondition"].ToString();
                        txtQtyPrice.Text = (Convert.ToDouble(txtUnitPrice.Text) * Convert.ToDouble(txtReqQty.Text)).ToString();
                        txtVendorQuotationNo.Text = dtChildData.Rows[0]["Field1"].ToString();
                        ddlCurrency.SelectedValue = dtChildData.Rows[0]["Field4"].ToString();
                        txtAmount.Text = dtChildData.Rows[0]["Amount"].ToString();
                        txtNetAmountPICurrency.Text = dtChildData.Rows[0]["Field5"].ToString();
                    }
                }
                dtChildData = null;
            }
        }
        Get_Tax_From_DB();
        SetDecimalFormat();
        ddlTransType.Enabled = true;
        foreach (DataListItem dli in dlProductDetail.Items)
        {
            TextBox UnitPrice = (TextBox)dli.FindControl("txtUnitPrice");
            if (UnitPrice.Text == "")
            {
                UnitPrice.Text = "0.00";
            }
            if (Convert.ToDouble(UnitPrice.Text) != 0)
            {
                ddlTransType.Enabled = false;
            }
        }
    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        double Net_Amount = 0;
        foreach (DataListItem dl in dlProductDetail.Items)
        {
            TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
            Net_Amount = Net_Amount + Convert.ToDouble(txtNetAmountPICurrency.Text);
        }
        if (Net_Amount == 0)
        {
            ddlTransType.Enabled = true;
            DisplayMessage("Net Price should not be zero!!!");
            btnQuoteSave.Attributes.Add("onclick", "this.disabled=false;" + ClientScript.GetPostBackEventReference(btnQuoteSave, "").ToString());
        }
        ddlTransType.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (editid.Value != "")
            {
                objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PQ", editid.Value.ToString(), ref trns);
                b = objQuoteHeader.UpdateQuoteHeader(StrCompId, StrBrandId, StrLocationId, editid.Value, txtRPQNo.Text, ObjSysParam.getDateForInput(txtRPQDate.Text).ToString(), hdnPIId.Value, "0", ViewState["CurrencyId"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                if (b != 0)
                {
                    //Add QuoteDetail Record
                    ObjQuoteDetail.DeleteQuoteDetail(StrCompId, StrBrandId, StrLocationId, editid.Value, hdnSupplierId.Value, ref trns);
                    foreach (DataListItem dlDetail in dlProductDetail.Items)
                    {
                        HiddenField hdnProductId = (HiddenField)dlDetail.FindControl("hdngvProductId");
                        HiddenField hdnSerialNo = (HiddenField)dlDetail.FindControl("hdnSerialNo");
                        Label lblProductDescription = (Label)dlDetail.FindControl("txtProductDescription");
                        TextBox txtRefrencedPName = (TextBox)dlDetail.FindControl("txtRefProductName");
                        TextBox txtRefPartNo = (TextBox)dlDetail.FindControl("txtRefPartNo");
                        HiddenField hdnUnitId = (HiddenField)dlDetail.FindControl("hdngvUnitId");
                        Label txtReqQty = (Label)dlDetail.FindControl("txtRequiredQuantity");
                        TextBox txtUnitPrice = (TextBox)dlDetail.FindControl("txtUnitPrice");
                        TextBox txtTaxP = (TextBox)dlDetail.FindControl("txtTaxP");
                        TextBox txtTaxV = (TextBox)dlDetail.FindControl("txtTaxV");
                        TextBox txtAfterTax = (TextBox)dlDetail.FindControl("txtPriceAfterTax");
                        TextBox txtDiscountP = (TextBox)dlDetail.FindControl("txtDiscountP");
                        TextBox txtDiscountV = (TextBox)dlDetail.FindControl("txtDiscountV");
                        TextBox txtAmount = (TextBox)dlDetail.FindControl("txtAmount");
                        TextBox txtNetAmountPICurrency = (TextBox)dlDetail.FindControl("txtNetAmountPICurrency");
                        TextBox txtTermsCondition = (TextBox)dlDetail.FindControl("txtTermsCondition");
                        HiddenField hdnsuggestedProductName = (HiddenField)dlDetail.FindControl("hdnSuggestedProductName");
                        if (hdnProductId.Value != "0")
                        {
                            hdnsuggestedProductName.Value = "";
                        }
                        if (txtTaxP.Text == "")
                        {
                            txtTaxP.Text = "0";
                        }
                        if (txtDiscountP.Text == "")
                        {
                            txtDiscountP.Text = "0";
                        }
                        int Detail_ID = ObjQuoteDetail.InsertQuoteDetail(StrCompId, StrBrandId, StrLocationId, editid.Value, hdnSupplierId.Value, hdnProductId.Value, hdnsuggestedProductName.Value, lblProductDescription.Text, txtRefrencedPName.Text, txtRefPartNo.Text, hdnUnitId.Value, txtReqQty.Text, txtUnitPrice.Text, txtTaxP.Text, txtTaxV.Text, txtAfterTax.Text, txtDiscountP.Text, txtDiscountV.Text, txtAmount.Text, txtTermsCondition.Text, txtVendorQuotationNo.Text, "", hdnSerialNo.Value, ddlCurrency.SelectedValue, txtNetAmountPICurrency.Text, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax(editid.Value.ToString(), Detail_ID.ToString(), dlDetail, ref trns);
                    }
                    dlProductDetail.DataSource = null;
                    dlProductDetail.DataBind();
                    pnlProduct.Visible = false;
                    PnlNewEdit.Visible = true;
                    pnlSalesQuotation.Visible = false;
                }
                else
                {
                    DisplayMessage("Record  Not Updated");
                }
            }
            else
            {
                b = objQuoteHeader.InsertQuoteHeader(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text, ObjSysParam.getDateForInput(txtRPQDate.Text).ToString(), hdnPIId.Value, "0", ViewState["CurrencyId"].ToString(), "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString(), ref trns);
                if (b != 0)
                {
                    string strMaxId = string.Empty;
                    strMaxId = b.ToString();
                    editid.Value = strMaxId;
                    if (txtRPQNo.Text.Trim() == ViewState["DocNo"].ToString().Trim())
                    {
                        int Quotationcount = objQuoteHeader.GetQuotationCountByLocationId1(Session["LocId"].ToString(), ref trns);
                        if (Quotationcount == 0)
                        {
                            objQuoteHeader.Updatecode(b.ToString(), txtRPQNo.Text + "1", ref trns);
                            txtRPQNo.Text = txtRPQNo.Text + "1";
                        }
                        else
                        {
                            if (objDa.return_DataTable("select RPQ_no from Inv_PurchaseQuoteHeader where location_id=" + Session["LocId"].ToString() + " and RPQ_no='" + txtRPQNo.Text.Trim() + Quotationcount.ToString() + "'",ref trns).Rows.Count > 0)
                            {
                                bool bCodeFlag = true;
                                while (bCodeFlag)
                                {
                                    Quotationcount += 1;
                                    if (objDa.return_DataTable("select RPQ_no from Inv_PurchaseQuoteHeader where location_id=" + Session["LocId"].ToString() + " and RPQ_no='" + txtRPQNo.Text.Trim() + Quotationcount.ToString() + "'",ref trns).Rows.Count == 0)
                                    {
                                        bCodeFlag = false;
                                    }
                                }
                            }
                            objQuoteHeader.Updatecode(b.ToString(), txtRPQNo.Text + Quotationcount.ToString(), ref trns);
                            txtRPQNo.Text = txtRPQNo.Text + Quotationcount.ToString();
                        }
                    }
                    //Add QuoteDetail Record
                    foreach (DataListItem dlDetail in dlProductDetail.Items)
                    {
                        HiddenField hdnProductId = (HiddenField)dlDetail.FindControl("hdngvProductId");
                        HiddenField hdnSerialNo = (HiddenField)dlDetail.FindControl("hdnSerialNo");
                        Label lblProductDescription = (Label)dlDetail.FindControl("txtProductDescription");
                        TextBox txtRefrencedPName = (TextBox)dlDetail.FindControl("txtRefProductName");
                        TextBox txtRefPartNo = (TextBox)dlDetail.FindControl("txtRefPartNo");
                        HiddenField hdnUnitId = (HiddenField)dlDetail.FindControl("hdngvUnitId");
                        Label txtReqQty = (Label)dlDetail.FindControl("txtRequiredQuantity");
                        TextBox txtUnitPrice = (TextBox)dlDetail.FindControl("txtUnitPrice");
                        TextBox txtTaxP = (TextBox)dlDetail.FindControl("txtTaxP");
                        TextBox txtTaxV = (TextBox)dlDetail.FindControl("txtTaxV");
                        TextBox txtAfterTax = (TextBox)dlDetail.FindControl("txtPriceAfterTax");
                        TextBox txtDiscountP = (TextBox)dlDetail.FindControl("txtDiscountP");
                        TextBox txtDiscountV = (TextBox)dlDetail.FindControl("txtDiscountV");
                        TextBox txtAmount = (TextBox)dlDetail.FindControl("txtAmount");
                        TextBox txtNetAmountPICurrency = (TextBox)dlDetail.FindControl("txtNetAmountPICurrency");
                        TextBox txtTermsCondition = (TextBox)dlDetail.FindControl("txtTermsCondition");
                        HiddenField hdnsuggestedProductName = (HiddenField)dlDetail.FindControl("hdnSuggestedProductName");
                        if (hdnProductId.Value != "0")
                        {
                            hdnsuggestedProductName.Value = "";
                        }
                        if (txtTaxP.Text == "")
                        {
                            txtTaxP.Text = "0";
                        }
                        if (txtDiscountP.Text == "")
                        {
                            txtDiscountP.Text = "0";
                        }
                        int Detail_ID = ObjQuoteDetail.InsertQuoteDetail(StrCompId, StrBrandId, StrLocationId, strMaxId, hdnSupplierId.Value, hdnProductId.Value, hdnsuggestedProductName.Value, lblProductDescription.Text, txtRefrencedPName.Text, txtRefPartNo.Text, hdnUnitId.Value, txtReqQty.Text, txtUnitPrice.Text, txtTaxP.Text, txtTaxV.Text, txtAfterTax.Text, txtDiscountP.Text, txtDiscountV.Text, txtAmount.Text, txtTermsCondition.Text, txtVendorQuotationNo.Text, "", hdnSerialNo.Value, ddlCurrency.SelectedValue, txtNetAmountPICurrency.Text, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        Insert_Tax(b.ToString(), Detail_ID.ToString(), dlDetail, ref trns);
                    }
                    dlProductDetail.DataSource = null;
                    dlProductDetail.DataBind();
                    pnlProduct.Visible = false;
                    PnlNewEdit.Visible = true;
                    pnlSalesQuotation.Visible = false;
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
            using (DataTable dtSupplier = objPISupplier.GetAllPISupplierWithPI_NoandAmount(StrCompId, StrBrandId, Session["LocId"].ToString(), hdnPIId.Value))
            {
                if (dtSupplier.Rows.Count > 0)
                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)GvSupplier, dtSupplier, "", "");
                    ViewState["DefaultCurrency"] = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), Resources.Attendance.Amount, Session["DBConnection"].ToString());
                    if (GvSupplier != null && GvSupplier.Rows.Count > 0)
                    {
                        if (ViewState["DefaultCurrency"] != null)
                        {
                            GvSupplier.HeaderRow.Cells[3].Text = ViewState["DefaultCurrency"].ToString();
                        }
                    }
                }
            }
            txtVendorQuotationNo.Text = "";
            SetDecimalFormat();
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
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        dlProductDetail.DataSource = null;
        dlProductDetail.DataBind();
        pnlProduct.Visible = false;
        PnlNewEdit.Visible = true;
        pnlSalesQuotation.Visible = false;
        txtVendorQuotationNo.Text = "";
    }
    //#endregion
    //#region Calculations
    protected void txtUnitPrice_TextChanged(object sender, EventArgs e)
    {
        ddlTransType.Enabled = true;
        foreach (DataListItem dli in dlProductDetail.Items)
        {
            TextBox UnitPrice = (TextBox)dli.FindControl("txtUnitPrice");
            if (UnitPrice.Text == "")
            {
                UnitPrice.Text = "0.00";
            }
            if (Convert.ToDouble(UnitPrice.Text) != 0)
            {
                ddlTransType.Enabled = false;
            }
        }
        TextBox txt = (TextBox)sender;
        DataListItem dl = (DataListItem)(((TextBox)sender)).Parent;
        if (txt.Text == "")
        {
            txt.Text = "0";
        }
        Label Lbl_Product_ID = (Label)dl.FindControl("Lbl_Product_ID");
        Label txtproductCode = (Label)dl.FindControl("txtproductCode");
        TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
        TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
        TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
        TextBox txtPriceAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
        TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
        TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
        TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
        TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
        Label txtQtyPrice = (Label)dl.FindControl("txtQtyPrice");
        Label txtQty = (Label)dl.FindControl("txtRequiredQuantity");
        HiddenField hdnPINO = (HiddenField)dl.FindControl("hdnPINO");
        HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
        if (txtUnitPrice.Text != "")
        {
            string[] GetVal = Common.TaxDiscountCaluculation(txtUnitPrice.Text, txtQty.Text, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, true, ViewState["CurrencyId"].ToString(), false, Session["DBConnection"].ToString());
            txtQtyPrice.Text = GetVal[0].ToString();
            txtDiscountP.Text = GetVal[1].ToString();
            txtDiscountV.Text = GetVal[2].ToString();
            //txtDiscountV.Text = (Convert.ToDouble(GetVal[2].ToString()) * Convert.ToDouble(txtQty.Text)).ToString();
            //txtTaxP.Text = GetVal[3].ToString();
            //txtTaxV.Text = GetVal[4].ToString();
            double Amt = Convert.ToDouble(txt.Text) - Convert.ToDouble(txtDiscountV.Text);
            Add_Tax_In_Session(Amt.ToString(), Lbl_Product_ID.Text, hdnSerialNo.Value);
            txtTaxP.Text = Convert.ToString(Get_Tax_Percentage(Lbl_Product_ID.Text, hdnSerialNo.Value));
            txtTaxV.Text = Convert.ToString(Get_Tax_Amount(Amt.ToString(), Lbl_Product_ID.Text, hdnSerialNo.Value));
            txtAmount.Text = GetVal[5].ToString();
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            //here we convert price in purchase inquiry currency 
            try
            {
                txtAmount.Text = ObjSysParam.GetCurencyConversionForInv(objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), (Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString())) * Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text)).ToString());
                txtNetAmountPICurrency.Text = ObjSysParam.GetCurencyConversionForInv(objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), (Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString())) * Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text)).ToString());
                //txtNetAmountPICurrency.Text = ObjSysParam.GetCurencyConversionForInv(objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), (Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString())) * Convert.ToDouble(txtAmount.Text)).ToString());                
            }
            catch
            {
            }
        }
        else
        {
            txtUnitPrice.Text = "0";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
        }
        Update_Tax_On_Change();
        SetDecimalFormat();
        if (!Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Tax_Hide()", true);
        if (!Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Discount_Hide()", true);
    }
    protected void txtDiscountP_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        DataListItem dl = (DataListItem)(((TextBox)sender)).Parent;
        if (txt.Text == "")
        {
            txt.Text = "0";
        }
        TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
        TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
        TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
        TextBox txtPriceAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
        TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
        TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
        TextBox txtTermCondition = (TextBox)dl.FindControl("txtTermsCondition");
        Label txtQtyPrice = (Label)dl.FindControl("txtQtyPrice");
        Label txtQty = (Label)dl.FindControl("txtRequiredQuantity");
        TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
        TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
        Label Lbl_Product_ID = (Label)dl.FindControl("Lbl_Product_ID");
        HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
        if (txtUnitPrice.Text != "")
        {
            double flTemp = 0;
            if (double.TryParse(txtUnitPrice.Text, out flTemp))
            {
                if (txtDiscountP.Text != "")
                {
                    if (double.TryParse(txtDiscountP.Text, out flTemp))
                    {
                        txtDiscountV.Text = "";
                        string[] GetVal = Common.TaxDiscountCaluculation(txtUnitPrice.Text, txtQty.Text, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, true, ViewState["CurrencyId"].ToString(), false, Session["DBConnection"].ToString());
                        txtQtyPrice.Text = GetVal[0].ToString();
                        txtDiscountP.Text = GetVal[1].ToString();
                        txtDiscountV.Text = GetVal[2].ToString();
                        //txtDiscountV.Text = (Convert.ToDouble(GetVal[2].ToString()) * Convert.ToDouble(txtQty.Text)).ToString();
                        //txtTaxP.Text = GetVal[3].ToString();
                        //txtTaxV.Text = GetVal[4].ToString();
                        double Amt = Convert.ToDouble(txtUnitPrice.Text) - Convert.ToDouble(txtDiscountV.Text);
                        txtTaxP.Text = Convert.ToString(Get_Tax_Percentage(Lbl_Product_ID.Text, hdnSerialNo.Value));
                        txtTaxV.Text = Convert.ToString(Get_Tax_Amount(Amt.ToString(), Lbl_Product_ID.Text, hdnSerialNo.Value));
                        //txtAmount.Text = GetVal[5].ToString();
                        txtAmount.Text = Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text).ToString();
                        txtNetAmountPICurrency.Text = Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text).ToString();
                    }
                }
            }
        }
        else
        {
            txtUnitPrice.Text = "";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
        }
        Update_Tax_On_Change();
        SetDecimalFormat();
        if (!Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Tax_Hide()", true);
        if (!Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Discount_Hide()", true);
    }
    protected void txtDiscountV_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = (TextBox)sender;
        DataListItem dl = (DataListItem)(((TextBox)sender)).Parent;
        if (txt.Text == "")
        {
            txt.Text = "0";
        }
        TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
        TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
        TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
        TextBox txtPriceAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
        TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
        TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
        TextBox txtTermCondition = (TextBox)dl.FindControl("txtTermsCondition");
        Label txtQtyPrice = (Label)dl.FindControl("txtQtyPrice");
        Label txtQty = (Label)dl.FindControl("txtRequiredQuantity");
        TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
        TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
        Label txtRequiredQuantity = (Label)dl.FindControl("txtRequiredQuantity");
        Label Lbl_Product_ID = (Label)dl.FindControl("Lbl_Product_ID");
        HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
        if (txtUnitPrice.Text != "")
        {
            double flTemp = 0;
            if (double.TryParse(txtUnitPrice.Text, out flTemp))
            {
                if (txtDiscountV.Text != "")
                {
                    if (double.TryParse(txtDiscountV.Text, out flTemp))
                    {
                        txtDiscountP.Text = "";
                        string[] GetVal = Common.TaxDiscountCaluculation(txtUnitPrice.Text, txtQty.Text, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, true, ViewState["CurrencyId"].ToString(), false, Session["DBConnection"].ToString());
                        txtQtyPrice.Text = GetVal[0].ToString();
                        txtDiscountP.Text = GetVal[1].ToString();
                        txtDiscountV.Text = GetVal[2].ToString();
                        //txtDiscountV.Text = (Convert.ToDouble(GetVal[2].ToString()) * Convert.ToDouble(txtQty.Text)).ToString();
                        //txtTaxP.Text = GetVal[3].ToString();
                        //txtTaxV.Text = GetVal[4].ToString();
                        double Amt = Convert.ToDouble(txtUnitPrice.Text) - Convert.ToDouble(txtDiscountV.Text);
                        txtTaxP.Text = Convert.ToString(Get_Tax_Percentage(Lbl_Product_ID.Text, hdnSerialNo.Value));
                        txtTaxV.Text = Convert.ToString(Get_Tax_Amount(Amt.ToString(), Lbl_Product_ID.Text, hdnSerialNo.Value));
                        //txtAmount.Text = GetVal[5].ToString();
                        txtAmount.Text = Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text).ToString();
                        txtNetAmountPICurrency.Text = Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text).ToString();
                    }
                }
            }
            else
            {
                txtUnitPrice.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
            }
        }
        Update_Tax_On_Change();
        SetDecimalFormat();
        if (!Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Tax_Hide()", true);
        if (!Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DL_Discount_Hide()", true);
    }
    //#endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnQuoteSave.Visible = clsPagePermission.bAdd;
        btnProductSave.Visible = clsPagePermission.bAdd;
        try
        {
            hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
            hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
            hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
            hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
            hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        }
        catch
        {
        }
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #endregion
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "12", "51", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    public void fillDataList(string RPQNo, ref SqlTransaction trns)
    {
        string Id = string.Empty;
        Id = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, RPQNo.Trim(), ref trns).Rows[0]["Trans_Id"].ToString();
        DataTable dtPQDetail = GetRecord(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString(), ref trns));
        if (dtPQDetail.Rows.Count != 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)datalistProduct, dtPQDetail, "", "");
        }
        DataTable dt = ObjSalesInqdetail.GetSIDetailBySInquiryId(StrCompId, StrBrandId, StrLocationId, Session["SalesInquiryId"].ToString(), ref trns, Session["FinanceYearId"].ToString());
        foreach (DataListItem item in datalistProduct.Items)
        {
            Label lblpId = (Label)item.FindControl("lblProductId");
            TextBox txtSPrice = (TextBox)item.FindControl("txtSalesPrice");
            TextBox txtSdesc = (TextBox)item.FindControl("txtSalesDescription");
            try
            {
                DataTable dtTemp = new DataView(dt, "Product_Id='" + lblpId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                txtSPrice.Text = dtTemp.Rows[0]["PurchaseProductPrice"].ToString();
                txtSdesc.Text = dtTemp.Rows[0]["PurchaseProductDescription"].ToString();
            }
            catch
            {
            }
        }
    }
    public void fillDataList(string RPQNo)
    {
        string Id = string.Empty;
        Id = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, RPQNo.Trim()).Rows[0]["Trans_Id"].ToString();
        DataTable dtPQDetail = GetRecord(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()));
        if (dtPQDetail.Rows.Count != 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)datalistProduct, dtPQDetail, "", "");
        }
        DataTable dt = ObjSalesInqdetail.GetSIDetailBySInquiryId(StrCompId, StrBrandId, StrLocationId, Session["SalesInquiryId"].ToString(), Session["FinanceYearId"].ToString());
        foreach (DataListItem item in datalistProduct.Items)
        {
            Label lblpId = (Label)item.FindControl("lblProductId");
            TextBox txtSPrice = (TextBox)item.FindControl("txtSalesPrice");
            TextBox txtSdesc = (TextBox)item.FindControl("txtSalesDescription");
            try
            {
                DataTable dtTemp = new DataView(dt, "Product_Id='" + lblpId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                txtSPrice.Text = dtTemp.Rows[0]["PurchaseProductPrice"].ToString();
                txtSdesc.Text = dtTemp.Rows[0]["PurchaseProductDescription"].ToString();
            }
            catch
            {
            }
        }
    }
    public DataTable GetRecord(DataTable dt)
    {
        DataTable dtreturn = new DataTable();
        dtreturn.Columns.Add("Product_Id");
        dtreturn.Columns.Add("Product_Description");
        dtreturn.Rows.Add();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["Product_Id"].ToString() != "0")
            {
                DataTable dtTemp = new DataView(dtreturn, "Product_Id='" + dt.Rows[i]["Product_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count == 0)
                {
                    if (dtreturn.Rows.Count == 1)
                    {
                        dtreturn.Rows[0]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[0]["Product_Description"] = dt.Rows[i]["ProductDescription"].ToString();
                    }
                    else
                    {
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Description"] = dt.Rows[i]["ProductDescription"].ToString();
                    }
                    dtreturn.Rows.Add();
                }
            }
            else
            {
                DataTable dtTemp = new DataView(dtreturn, "Product_Id='" + dt.Rows[i]["Product_Id"].ToString() + "' and Product_Description='" + dt.Rows[i]["ProductDescription"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count == 0)
                {
                    if (dtreturn.Rows.Count == 1)
                    {
                        dtreturn.Rows[0]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[0]["Product_Description"] = dt.Rows[i]["ProductDescription"].ToString();
                    }
                    else
                    {
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Description"] = dt.Rows[i]["ProductDescription"].ToString();
                    }
                    dtreturn.Rows.Add();
                }
            }
        }
        dtreturn.Rows.RemoveAt(dtreturn.Rows.Count - 1);
        return dtreturn;
    }
    public string ProductName(string ProductId, string Description)
    {
        string ProductName = string.Empty;
        using (DataTable dt = ObjProductMaster.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()))
        {
            if (dt.Rows.Count != 0)
            {
                ProductName = dt.Rows[0]["EProductName"].ToString();
            }
            else
            {
                ProductName = Description.ToString();
            }
        }
        return ProductName;
    }
    protected void datalistProduct_DataBinding(object sender, EventArgs e)
    {
        SetDecimalFormat();
        //
    }
    protected void datalistProduct_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Label l1 = (Label)e.Item.FindControl("lblProductId");
        GridView gvSup = (GridView)e.Item.FindControl("gvSupplier");
        string Id = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, txtRPQNo.Text).Rows[0]["Trans_Id"].ToString();
        DataTable dtPQDetail = new DataTable();
        if (l1.Text.Trim() != "0")
        {
            dtPQDetail = new DataView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Product_Id='" + l1.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            Label lNm = (Label)e.Item.FindControl("txtProductName");
            dtPQDetail = new DataView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Product_Id='" + l1.Text.Trim() + "' and ProductDescription='" + lNm.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSup, dtPQDetail, "", "");
        dtPQDetail = null;
        SetDecimalFormat();
    }
    protected void btnSalesSave_Click(object sender, EventArgs e)
    {
        int counter = 0;
        foreach (DataListItem item in datalistProduct.Items)
        {
            TextBox txt = (TextBox)item.FindControl("txtSalesPrice");
            TextBox txtdesc = (TextBox)item.FindControl("txtSalesDescription");
            if (txt.Text == "" || txtdesc.Text == "")
            {
                DisplayMessage("All Sales price and Description should not be blank");
                txt.Focus();
                counter = 1;
                break;
            }
        }
        if (counter == 1)
        {
            DisplayMessage("Sales price and Description should not be blank");
            return;
        }
        ObjSalesInquiryHeader.UpdateSalesInquiryStatusbyPI(StrCompId, StrBrandId, StrLocationId, Session["SalesInquiryId"].ToString(), "Quotation Come From Purchase");
        foreach (DataListItem item in datalistProduct.Items)
        {
            Label lblpId = (Label)item.FindControl("lblProductId");
            TextBox txtSPrice = (TextBox)item.FindControl("txtSalesPrice");
            TextBox txtSdesc = (TextBox)item.FindControl("txtSalesDescription");
            ObjSalesInqdetail.UpdateSIDetailFromPurchase(StrCompId, StrBrandId, StrLocationId, Session["SalesInquiryId"].ToString(), lblpId.Text, txtSPrice.Text, txtSdesc.Text, Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        Session["TransFrom"] = null;
        Session["SalesInquiryId"] = null;
        datalistProduct.DataSource = null;
        datalistProduct.DataBind();
        btnList_Click(null, null);
        SetDecimalFormat();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        datalistProduct.DataSource = null;
        datalistProduct.DataBind();
        pnlProduct.Visible = false;
        PnlNewEdit.Visible = true;
        pnlSalesQuotation.Visible = false;
        SetDecimalFormat();
    }
    public string GetPurchaseinquiryNo(string PId)
    {
        string PInqNo = "";
        if (PId != "")
        {
            using (DataTable Dt = objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, PId))
            {
                if (Dt.Rows.Count > 0)
                {
                    PInqNo = Dt.Rows[0]["PI_No"].ToString();
                }
            }
        }
        return PInqNo;
    }
    public string GetPurchaseRequestNo(string PId)
    {
        string PInqNo = "";
        if (PId != "0")
        {
            using (DataTable Dt = ObjPRHeader.GetPurchaseRequestTrueByRequestId_For_quotation(StrCompId, StrBrandId, StrLocationId, PId))
            {
                if (Dt.Rows.Count > 0)
                {
                    PInqNo = Dt.Rows[0]["RequestNo"].ToString();
                }
            }
        }
        else
        {
            PInqNo = "0";
        }
        return PInqNo;
    }
    //#region View
    protected void BtnCancelView_Click(object sender, EventArgs e)
    {
        ViewReset();
    }
    void ViewReset()
    {
        btnList_Click(null, null);
        editid.Value = "";
        hdnSupplierId.Value = "";
        hdnPIId.Value = "";
        lblQuotationNoView.Text = "";
        lblRPQDateView.Text = "";
        lblTotalAmountView.Text = "";
        lblInquiryNoView.Text = "";
        lblInquiryDateView.Text = "";
        DataListView.DataSource = null;
        DataListView.DataBind();
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        using (DataTable dtQuoteEdit = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, editid.Value))
        {
            if (dtQuoteEdit.Rows.Count > 0)
            {
                ddlCurrency.SelectedValue = dtQuoteEdit.Rows[0]["Field1"].ToString();
                lblQuotationNoView.Text = dtQuoteEdit.Rows[0]["RPQ_No"].ToString();
                lblRPQDateView.Text = Convert.ToDateTime(dtQuoteEdit.Rows[0]["RPQ_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                lblTotalAmountView.Text = dtQuoteEdit.Rows[0]["TotalAmount"].ToString();
                ViewState["CurrencyId"] = dtQuoteEdit.Rows[0]["Field1"].ToString();
                //Add Inquiry Data
                string strPI_Id = dtQuoteEdit.Rows[0]["PI_No"].ToString();
                hdnPIId.Value = strPI_Id;
                if (strPI_Id != "" && strPI_Id != "0")
                {
                    using (DataTable dtInquiry = objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, strPI_Id))
                    {
                        if (dtInquiry.Rows.Count > 0)
                        {
                            lblInquiryNoView.Text = dtInquiry.Rows[0]["PI_No"].ToString();
                            lblInquiryDateView.Text = Convert.ToDateTime(dtInquiry.Rows[0]["PIDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        }
                    }
                }
                FillDatalistView();
                foreach (DataListItem item in DataListView.Items)
                {
                    GridView gvSupplierHeader = (GridView)item.FindControl("gvSupplierView");
                    foreach (GridViewRow gvRow in gvSupplierHeader.Rows)
                    {
                        GridView gvSupplierDetail = (GridView)gvRow.FindControl("gvSupplierDetailView");
                        foreach (GridViewRow gvrowDetail in gvSupplierDetail.Rows)
                        {
                            Label lblreqqtyView = (Label)gvrowDetail.FindControl("lblreqqtyView");
                            Label lblunitpriceView = (Label)gvrowDetail.FindControl("lblunitpriceView");
                            Label lblQtypriceView = (Label)gvrowDetail.FindControl("lblQtypriceView");
                            Label lbltaxpview = (Label)gvrowDetail.FindControl("lbltaxpview");
                            Label lbltaxvview = (Label)gvrowDetail.FindControl("lbltaxvview");
                            Label lbltaxafterpview = (Label)gvrowDetail.FindControl("lbltaxafterpview");
                            Label lblDispview = (Label)gvrowDetail.FindControl("lblDispview");
                            Label lblDisvview = (Label)gvrowDetail.FindControl("lblDisvview");
                            Label lblAmountview = (Label)gvrowDetail.FindControl("lblAmountview");
                            lblreqqtyView.Text = lblreqqtyView.Text.ToString();
                            lblunitpriceView.Text = GetAmountDecimal(lblunitpriceView.Text);
                            lblQtypriceView.Text = GetAmountDecimal(lblQtypriceView.Text);
                            lbltaxpview.Text = GetAmountDecimal(lbltaxpview.Text);
                            lbltaxvview.Text = GetAmountDecimal(lbltaxvview.Text);
                            lbltaxafterpview.Text = GetAmountDecimal(lbltaxafterpview.Text);
                            lblDispview.Text = GetAmountDecimal(lblDispview.Text);
                            lblDisvview.Text = GetAmountDecimal(lblDisvview.Text);
                            lblAmountview.Text = GetAmountDecimal(lblAmountview.Text);
                        }
                    }
                }
            }
        }
        SetDecimalFormat();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Purchase_Quotation_Popup()", true);
    }
    void FillDatalistView()
    {
        using (DataTable Dtdetail = GetRecordView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, editid.Value.Trim())))
        {
            if (Dtdetail.Rows.Count != 0)
            {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)DataListView, Dtdetail, "", "");
            }
        }
    }
    public DataTable GetRecordView(DataTable dt)
    {
        DataTable dtreturn = new DataTable();
        dtreturn.Columns.Add("Product_Id");
        dtreturn.Columns.Add("ProductDescription");
        dtreturn.Columns.Add("RefrencedProductName");
        dtreturn.Columns.Add("RefrencedPartNo");
        dtreturn.Columns.Add("TermsCondition");
        dtreturn.Columns.Add("Amount");
        dtreturn.Columns.Add("SuggestedProductName");
        dtreturn.Rows.Add();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["Product_Id"].ToString() != "0")
            {
                using (DataTable dtTemp = new DataView(dtreturn, "Product_Id='" + dt.Rows[i]["Product_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable())
                {
                    if (dtTemp.Rows.Count == 0)
                    {
                        if (dtreturn.Rows.Count == 1)
                        {
                            if (dt.Rows[i]["Product_Id"].ToString().Trim() == "0")
                            {
                                dtreturn.Rows[0]["SuggestedProductName"] = dt.Rows[i]["SuggestedProductName"].ToString();
                            }
                            else
                            {
                            }
                            dtreturn.Rows[0]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                            dtreturn.Rows[0]["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                            if (dt.Rows[i]["RefrencedProductName"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[0]["RefrencedProductName"] = dt.Rows[i]["RefrencedProductName"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[0]["RefrencedProductName"] = "No";
                            }
                            if (dt.Rows[i]["RefrencedPartNo"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[0]["RefrencedPartNo"] = dt.Rows[i]["RefrencedPartNo"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[0]["RefrencedPartNo"] = "No";
                            }
                            dtreturn.Rows[0]["TermsCondition"] = dt.Rows[i]["TermsCondition"].ToString();
                            dtreturn.Rows[0]["Amount"] = dt.Rows[i]["Amount"].ToString();
                        }
                        else
                        {
                            if (dt.Rows[i]["Product_Id"].ToString().Trim() == "0")
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["SuggestedProductName"] = dt.Rows[i]["SuggestedProductName"].ToString();
                            }
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                            if (dt.Rows[i]["RefrencedProductName"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedProductName"] = dt.Rows[i]["RefrencedProductName"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedProductName"] = "No";
                            }
                            if (dt.Rows[i]["RefrencedPartNo"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedPartNo"] = dt.Rows[i]["RefrencedPartNo"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedPartNo"] = "No";
                            }
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["TermsCondition"] = dt.Rows[i]["TermsCondition"].ToString();
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["Amount"] = dt.Rows[i]["Amount"].ToString();
                        }
                        dtreturn.Rows.Add();
                    }
                }
            }
            else
            {
                using (DataTable dtTemp = new DataView(dtreturn, "Product_Id='" + dt.Rows[i]["Product_Id"].ToString() + "' and ProductDescription='" + dt.Rows[i]["ProductDescription"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable())
                {
                    if (dtTemp.Rows.Count == 0)
                    {
                        if (dtreturn.Rows.Count == 1)
                        {
                            if (dt.Rows[i]["Product_Id"].ToString().Trim() == "0")
                            {
                                dtreturn.Rows[0]["SuggestedProductName"] = dt.Rows[i]["SuggestedProductName"].ToString();
                            }
                            dtreturn.Rows[0]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                            dtreturn.Rows[0]["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                            if (dt.Rows[i]["RefrencedProductName"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[0]["RefrencedProductName"] = dt.Rows[i]["RefrencedProductName"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[0]["RefrencedProductName"] = "No";
                            }
                            if (dt.Rows[i]["RefrencedPartNo"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[0]["RefrencedPartNo"] = dt.Rows[i]["RefrencedPartNo"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[0]["RefrencedPartNo"] = "No";
                            }
                            dtreturn.Rows[0]["TermsCondition"] = dt.Rows[i]["TermsCondition"].ToString();
                            dtreturn.Rows[0]["Amount"] = dt.Rows[i]["Amount"].ToString();
                        }
                        else
                        {
                            if (dt.Rows[i]["Product_Id"].ToString().Trim() == "0")
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["SuggestedProductName"] = dt.Rows[i]["SuggestedProductName"].ToString();
                            }
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["ProductDescription"] = dt.Rows[i]["ProductDescription"].ToString();
                            if (dt.Rows[i]["RefrencedProductName"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedProductName"] = dt.Rows[i]["RefrencedProductName"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedProductName"] = "No";
                            }
                            if (dt.Rows[i]["RefrencedPartNo"].ToString().Trim() != "")
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedPartNo"] = dt.Rows[i]["RefrencedPartNo"].ToString();
                            }
                            else
                            {
                                dtreturn.Rows[dtreturn.Rows.Count - 1]["RefrencedPartNo"] = "No";
                            }
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["TermsCondition"] = dt.Rows[i]["TermsCondition"].ToString();
                            dtreturn.Rows[dtreturn.Rows.Count - 1]["Amount"] = dt.Rows[i]["Amount"].ToString();
                        }
                        dtreturn.Rows.Add();
                    }
                }
            }
        }
        dtreturn.Rows.RemoveAt(dtreturn.Rows.Count - 1);
        return dtreturn;
    }
    public void FillViewDetail(GridView gvDetail, DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvDetail, dt, "", "");
        try
        {
            gvDetail.Columns[6].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvDetail.Columns[7].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvDetail.Columns[8].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvDetail.Columns[9].Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            gvDetail.Columns[10].Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
        }
        dt = null;
    }
    protected void gvSupplierView_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView gvchild = (GridView)e.Row.FindControl("gvSupplierDetailView");
            if (Session["dtView"] != null)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvchild, (DataTable)Session["dtView"], "", "");
                Session["dtView"] = null;
                try
                {
                    gvchild.Columns[6].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    gvchild.Columns[7].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    gvchild.Columns[8].Visible = Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    gvchild.Columns[9].Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    gvchild.Columns[10].Visible = Inventory_Common.IsPurchaseDiscountEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                }
                catch
                {
                }
            }
            else
            {
                gvchild.DataSource = null;
                gvchild.DataBind();
            }
        }
    }
    protected void datalistProductView_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HiddenField l1 = (HiddenField)e.Item.FindControl("hdngvProductId");
        GridView gvSup = (GridView)e.Item.FindControl("gvSupplierView");
        Label productDescription = (Label)e.Item.FindControl("txtProductDescription");
        HiddenField hdnsuggestedProductName = (HiddenField)e.Item.FindControl("hdnSuggestedProductName");
        string Id = editid.Value.Trim();
        DataTable dtPQDetail = new DataTable();
        if (l1.Value.Trim() != "0")
        {
            dtPQDetail = new DataView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Product_Id='" + l1.Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dtPQDetail = new DataView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Product_Id='" + l1.Value.Trim() + "' and ProductDescription='" + productDescription.Text.Trim() + "' and SuggestedProductName='" + hdnsuggestedProductName.Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        Session["dtView"] = dtPQDetail;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSup, dtPQDetail, "", "");
        foreach (GridViewRow GridRow in gvSup.Rows)
        {
            DataTable dt = new DataTable();
            dt = dtPQDetail.Copy();
            try
            {
                dt = new DataView(dt, "Supplier_Id=" + ((HiddenField)GridRow.FindControl("hdngvSupplierId")).Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            FillViewDetail((GridView)GridRow.FindControl("gvSupplierDetailView"), dt);
        }
        dtPQDetail = null;
    }
    //#endregion
    public string SuggestedProductName(string ProductId, string ProductDescription)
    {
        string ProductName = string.Empty;
        using (DataTable dtQuoteEdit = objQuoteHeader.GetQuoteHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, editid.Value))
        {
            if (dtQuoteEdit.Rows.Count > 0)
            {
                String Str_PINO = dtQuoteEdit.Rows[0]["PI_No"].ToString();
                DataTable dtProductDetail = ObjPIDetail.GetPIDetailByPI_No(StrCompId, StrBrandId, StrLocationId, Str_PINO);
                if (ProductId.Trim() != "0")
                {
                    DataTable dtPName = objProductM.GetProductMasterById(StrCompId, Session["BrandId"].ToString(), ProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
                    if (dtPName.Rows.Count > 0)
                    {
                        ProductName = dtPName.Rows[0]["EProductName"].ToString();
                    }
                }
                else
                {
                    try
                    {
                        dtProductDetail = new DataView(dtProductDetail, "ProductDescription='" + ProductDescription.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    ProductName = dtProductDetail.Rows[0]["SuggestedProductName"].ToString();
                    if (ProductName.Trim() == "")
                    {
                        ProductName = "0";
                    }
                }
                dtProductDetail = null;
            }
        }
        return ProductName;
    }
    public string GetAmountDecimal(string Amount)
    {
        return SystemParameter.GetAmountWithDecimal(Amount, Session["LoginLocDecimalCount"].ToString());
    }
    //#region Purchase Inquiry Search
    protected void btnbindRequest_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameRequest.SelectedItem.Value == "PIDate" || ddlFieldNameRequest.SelectedItem.Value == "OrderCompletionDate")
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
        FillRequestGrid();
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
        txtValueRequestDate.Text = "";
        txtValueRequestDate.Visible = false;
        txtValueRequest.Visible = true;
        txtValueRequest.Focus();
        FillRequestGrid();
    }
    //#endregion
    //#region QuotationComparision
    protected void IbtnQC_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/CompareQuatation.aspx?RPQId=" + HttpUtility.UrlEncode(Encrypt(e.CommandArgument.ToString())) + "&&C=F');", true);
    }
    public string Encrypt(string clearText)
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
    //#endregion
    //#region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "RPQ_Date")
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
        if (ddlFieldNameBin.SelectedItem.Value == "RPQ_Date")
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
    protected void ddlFieldNameRequest_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameRequest.SelectedItem.Value == "PIDate" || ddlFieldNameRequest.SelectedItem.Value == "OrderCompletionDate")
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
    //#endregion
    //#region PendingPurchaseQuotation
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    //#endregion
    //#region ProductHistory
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
    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        string CustomerName = string.Empty;
        try
        {
            CustomerName = lblsupplierName.Text;
        }
        catch
        {
        }
        string strCmd = "window.open('../Inventory/Magic_Stock_Analysis.aspx?ProductId=" + e.CommandArgument.ToString() + "&&Type=PURCHASE&&Contact=" + CustomerName + "')";
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
    }
    //#endregion
    //#region SupplierHistory
    protected void lnkSupplierdetail_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + e.CommandArgument.ToString() + "&&Page=PQ','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    //#endregion
    public void FillTransactionType()
    {
        ddlTransType.DataSource = Enum
        .GetValues(typeof(Common.TransactionType))
        .Cast<Common.TransactionType>()
        .Select(s => new KeyValuePair<int, string>((int)s, s.ToString()))
        .ToList();
        ddlTransType.DataValueField = "Key";
        ddlTransType.DataTextField = "Value";
        ddlTransType.DataBind();
        ddlTransType.Items.Insert(0, new ListItem("--Select--", "-1"));
    }
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = Session["LocCurrencyId"].ToString();
        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = GetAmountDecimal((Convert.ToDouble(strExchangeRate) * Convert.ToDouble(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    protected void Get_Tax_Parameter()
    {
        using (DataTable Dt_Parameter = ObjSysParam.GetSysParameterByParamName("Tax System"))
        {
            if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
            {
                if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Company")
                {
                    Hdn_Tax_By.Value = "Company";
                }
                else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Location")
                {
                    Hdn_Tax_By.Value = "Location";
                }
                else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "System")
                {
                    Hdn_Tax_By.Value = "System";
                }
                else
                {
                    Hdn_Tax_By.Value = "Select";
                }
            }
        }
    }
    public DataTable TemporaryProductWiseTaxes()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Product_Id", typeof(float));
        dt.Columns.Add("Tax_Id", typeof(float));
        dt.Columns.Add("Tax_Name", typeof(string));
        dt.Columns.Add("Tax_Value", typeof(float));
        dt.Columns.Add("TaxAmount", typeof(float));
        dt.Columns.Add("Amount", typeof(float));
        dt.Columns.Add("Serial_No", typeof(float));
        return dt;
    }
    public double Get_Net_Amount(string Gross_Price, string Discount_Value, string Tax_Value, string Qty)
    {
        double Net_Price = 0;
        double discount = Convert.ToDouble(Discount_Value) * Convert.ToDouble(Qty);
        double Tax = Convert.ToDouble(Tax_Value) * Convert.ToDouble(Qty);
        Net_Price = (Convert.ToDouble(Gross_Price) + Convert.ToDouble(Tax)) - Convert.ToDouble(discount);
        return Net_Price;
    }
    protected void Get_Product_Details(DataListItem DtLIst)
    {
        DataListItem dl = DtLIst;
        Label Lbl_Product_ID = (Label)dl.FindControl("Lbl_Product_ID");
        Label txtproductCode = (Label)dl.FindControl("txtproductCode");
        TextBox txtUnitPrice = (TextBox)dl.FindControl("txtUnitPrice");
        TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
        TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
        TextBox txtPriceAfterTax = (TextBox)dl.FindControl("txtPriceAfterTax");
        TextBox txtDiscountP = (TextBox)dl.FindControl("txtDiscountP");
        TextBox txtDiscountV = (TextBox)dl.FindControl("txtDiscountV");
        TextBox txtAmount = (TextBox)dl.FindControl("txtAmount");
        TextBox txtNetAmountPICurrency = (TextBox)dl.FindControl("txtNetAmountPICurrency");
        Label txtQtyPrice = (Label)dl.FindControl("txtQtyPrice");
        Label txtQty = (Label)dl.FindControl("txtRequiredQuantity");
        HiddenField hdnPINO = (HiddenField)dl.FindControl("hdnPINO");
        HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
        if (txtUnitPrice.Text != "")
        {
            string[] GetVal = Common.TaxDiscountCaluculation(txtUnitPrice.Text, txtQty.Text, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, true, ViewState["CurrencyId"].ToString(), false, Session["DBConnection"].ToString());
            txtQtyPrice.Text = GetVal[0].ToString();
            txtDiscountP.Text = GetVal[1].ToString();
            txtDiscountV.Text = GetVal[2].ToString();
            // txtDiscountV.Text = (Convert.ToDouble(GetVal[2].ToString()) * Convert.ToDouble(txtQty.Text)).ToString();
            //txtTaxP.Text = GetVal[3].ToString();
            //txtTaxV.Text = GetVal[4].ToString();
            double Amt = Convert.ToDouble(txtUnitPrice.Text) - Convert.ToDouble(txtDiscountV.Text);
            txtTaxP.Text = Convert.ToString(Get_Tax_Percentage(Lbl_Product_ID.Text, hdnSerialNo.Value));
            txtTaxV.Text = Convert.ToString(Get_Tax_Amount(Amt.ToString(), Lbl_Product_ID.Text, hdnSerialNo.Value));
            txtAmount.Text = GetVal[5].ToString();
            if (txtAmount.Text == "")
            {
                txtAmount.Text = "0";
            }
            //here we convert price in purchase inquiry currency 
            try
            {
                txtAmount.Text = ObjSysParam.GetCurencyConversionForInv(objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), (Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString())) * Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text)).ToString());
                txtNetAmountPICurrency.Text = ObjSysParam.GetCurencyConversionForInv(objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), (Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), Session["DBConnection"].ToString())) * Get_Net_Amount(txtQtyPrice.Text, txtDiscountV.Text, txtTaxV.Text, txtQty.Text)).ToString());
                //txtNetAmountPICurrency.Text = ObjSysParam.GetCurencyConversionForInv(objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString(), (Convert.ToDouble(SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, objPIHeader.GetPIHeaderAllDataByTransId(StrCompId, StrBrandId, StrLocationId, hdnPINO.Value).Rows[0]["Field2"].ToString())) * Convert.ToDouble(txtAmount.Text)).ToString());                
            }
            catch
            {
            }
        }
        else
        {
            txtUnitPrice.Text = "0";
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
        }
        SetDecimalFormat();
    }
    public void Update_Tax_On_Change()
    {
        foreach (DataListItem DLI in dlProductDetail.Items)
        {
            Get_Product_Details(DLI);
        }
    }
    public void Insert_Tax(string PQ_Header_ID, string Detail_ID, DataListItem dlDetail, ref SqlTransaction trns)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PQ", PQ_Header_ID.ToString(), Detail_ID.ToString(), ref trns);
        Label Lbl_Product_ID = (Label)dlDetail.FindControl("Lbl_Product_ID");
        TextBox txtUnitPrice = (TextBox)dlDetail.FindControl("txtUnitPrice");
        TextBox txtDiscountV = (TextBox)dlDetail.FindControl("txtDiscountV");
        HiddenField hdnSerialNo = (HiddenField)dlDetail.FindControl("hdnSerialNo");
        double Net_Amount = Convert.ToDouble(txtUnitPrice.Text) - Convert.ToDouble(txtDiscountV.Text);
        string Tax_Value = Convert.ToString(Get_Tax_Amount(Net_Amount.ToString(), Lbl_Product_ID.Text, hdnSerialNo.Value));
        DataTable ProductTax = new DataTable();
        if (Session["Temp_Product_Tax_PQ"] == null)
            ProductTax = TemporaryProductWiseTaxes();
        else
            ProductTax = (DataTable)Session["Temp_Product_Tax_PQ"];
        string ProductId = string.Empty;
        string TaxId = string.Empty;
        string TaxValue = string.Empty;
        string TaxAmount = string.Empty;
        string Amount = string.Empty;
        if (ProductTax != null && ProductTax.Rows.Count > 0)
        {
            foreach (DataRow dr in ProductTax.Rows)
            {
                //if (dr["Product_Id"].ToString() == Lbl_Product_ID.Text)
                if (dr["Product_Id"].ToString() == Lbl_Product_ID.Text && dr["Serial_No"].ToString() == hdnSerialNo.Value)
                {
                    //ProductId = dr["Product_Id"].ToString();
                    //TaxId = dr["Tax_Id"].ToString();
                    //TaxValue = GetAmountDecimal(dr["Tax_Value"].ToString());
                    //TaxAmount = dr["TaxAmount"].ToString();
                    //Amount = Net_Amount.ToString();
                    ProductId = dr["Product_Id"].ToString();
                    TaxId = dr["Tax_Id"].ToString();
                    TaxValue = GetAmountDecimal(dr["Tax_Value"].ToString());
                    TaxAmount = (Convert.ToDouble((Convert.ToDouble(Net_Amount) * Convert.ToDouble(TaxValue))) / 100).ToString();
                    Amount = Net_Amount.ToString();
                    //if (Convert.ToDouble(Amount) != 0)
                    objTaxRefDetail.InsertRecord("PQ", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
        }
    }
    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if (Session["Temp_Product_Tax_PQ"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PQ"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                DataListItem dl = (DataListItem)(((ImageButton)sender)).Parent;
                HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
                //Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "' and Serial_No='" + hdnSerialNo.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    gvTaxCalculation.DataSource = Dt_Cal;
                    gvTaxCalculation.DataBind();
                    int Row_Index = 0;
                    ImageButton img = (ImageButton)sender;
                    string id = img.ID;
                    DataListItem item = (DataListItem)img.NamingContainer;
                    if (item != null)
                    {
                        Row_Index = item.ItemIndex;
                    }
                    //int Row_Index = ((DataListItem)((ImageButton)sender).Parent.Parent).ItemIndex;
                    string Grid_Name = e.CommandName.ToString();
                    if (Grid_Name == "dlProductDetail")
                    {
                        TextBox Unit_Price = (TextBox)dlProductDetail.Items[Row_Index].FindControl("txtUnitPrice");
                        TextBox Discount_Price = (TextBox)dlProductDetail.Items[Row_Index].FindControl("txtDiscountV");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    Hdn_Serial_No_Tax.Value = hdnSerialNo.Value;
                    Hdn_Product_Id_Tax.Value = e.CommandArgument.ToString();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
                }
                else
                {
                    DisplayMessage("No Tax Details found");
                    return;
                }
            }
        }
        else
        {
            DisplayMessage("No Tax Details found");
            return;
        }
    }
    public void Get_Tax_From_DB()
    {
        try
        {
            string TaxQuery = string.Empty;
            if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    if (editid.Value != "")
                    {
                        using (DataTable DT_Db_Details = ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId, StrBrandId, StrLocationId, editid.Value))
                        {
                            if (DT_Db_Details.Rows.Count > 0)
                            {
                                TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Field3 as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_PurchaseQuoteDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + editid.Value + "' and TRD.Ref_Type='PQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Field3";
                                DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                                Session["Temp_Product_Tax_PQ"] = null;
                                DataTable Dt_Temp = new DataTable();
                                Dt_Temp = TemporaryProductWiseTaxes();
                                Dt_Temp = Dt_Inv_TaxRefDetail;
                                if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                                {
                                    Session["Temp_Product_Tax_PQ"] = Dt_Temp;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public double Get_Tax_Percentage(string ProductId, string Serial_No)
    {
        double TotalTax_Percentage = 0;
        if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true)
        {
            if (Session["Temp_Product_Tax_PQ"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_PQ"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow DRT in Dt_Session_Tax.Rows)
                    {
                        TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                    }
                }
            }
        }
        return TotalTax_Percentage;
    }
    public double Get_Tax_Amount(string Amount, string ProductId, string Serial_No)
    {
        double TotalTax_Amount = 0;
        if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true)
        {
            if (Session["Temp_Product_Tax_PQ"] != null)
            {
                double Tax_Value = Get_Tax_Percentage(ProductId, Serial_No);
                double Temp_Amount = Convert.ToDouble(Amount);
                TotalTax_Amount = Convert.ToDouble(GetAmountDecimal(((Tax_Value * Temp_Amount) / 100).ToString()));
            }
        }
        return TotalTax_Amount;
    }
    public void Add_Tax_In_Session(string Amount, string ProductId, string Serial_No)
    {
        string TaxQuery = string.Empty;
        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
        if (Inventory_Common.IsPurchaseTaxEnabled(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()) == true && double.Parse(Amount) > 0)
        {
            Get_Tax_Parameter();
            String Condition = string.Empty;
            if (Hdn_Tax_By.Value == Resources.Attendance.Company)
                Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Company_ID = " + Session["CompId"].ToString() + "";
            else if (Hdn_Tax_By.Value == Resources.Attendance.Location)
                Condition = "AND IPTM.Field1='" + Hdn_Tax_By.Value + "' AND IPTM.Location_ID = " + Session["LocId"].ToString() + "";
            if (ddlTransType.SelectedIndex > 0)
            {
                TaxQuery = @"Select STM.Tax_Name,IPTM.Tax_Percentage as Tax_Value,Tax_Id,IPTM.Product_Id from Inv_ProductTaxMaster IPTM inner join Sys_TaxMaster STM on IPTM.Tax_Id = STM.Trans_Id
                            where ((',' + RTRIM(STM.Field2) + ',') LIKE '%,' + CONVERT(nvarchar(100)," + ddlTransType.SelectedValue + ") + ',%') and IPTM.Product_Id = " + ProductId + "" + Condition + "";
                DataTable dtTax = objDa.return_DataTable(TaxQuery);
                double TotalPriceBeforeDiscount = double.Parse(Amount);
                DataTable dt = new DataTable();
                if (Session["Temp_Product_Tax_PQ"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_PQ"];
                if (dtTax.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTax.Rows)
                    {
                        double taxvalue = double.Parse(dr["Tax_Value"].ToString());
                        double taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                        DataRow Newdr = dt.NewRow();
                        Newdr["Product_Id"] = ProductId;
                        Newdr["Tax_Id"] = dr["Tax_Id"].ToString();
                        Newdr["Tax_Name"] = dr["Tax_Name"].ToString();
                        Newdr["Tax_Value"] = dr["Tax_Value"].ToString();
                        Newdr["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                        Newdr["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                        Newdr["Serial_No"] = Serial_No;
                        DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + " AND Serial_No = '" + Serial_No + "'");
                        //DataRow[] SRow = dt.Select("Product_id = " + ProductId + " AND Tax_Id = " + dr["Tax_Id"].ToString() + "");
                        if (SRow.Length == 0)
                        {
                            dt.Rows.Add(Newdr);
                        }
                        else
                        {
                            taxvalue = double.Parse(SRow[0]["Tax_Value"].ToString());
                            taxamount = (TotalPriceBeforeDiscount * taxvalue) / 100;
                            SRow[0]["TaxAmount"] = GetCurrency(Session["LocCurrencyId"].ToString(), taxamount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Amount"] = GetCurrency(Session["LocCurrencyId"].ToString(), TotalPriceBeforeDiscount.ToString()).Split('/')[0].ToString();
                            SRow[0]["Serial_No"] = Serial_No;
                        }
                    }
                    Session["Temp_Product_Tax_PQ"] = dt;
                }
            }
        }
    }
    protected void ddlTransType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Temp_Product_Tax_PQ"] = null;
        foreach (DataListItem dl in dlProductDetail.Items)
        {
            TextBox txtTaxP = (TextBox)dl.FindControl("txtTaxP");
            TextBox txtTaxV = (TextBox)dl.FindControl("txtTaxV");
            txtTaxP.Text = "0.00";
            txtTaxV.Text = "0.00";
        }
    }
    protected void Btn_Update_Tax_Click(object sender, EventArgs e)
    {
        string Serial_No = Hdn_Serial_No_Tax.Value;
        string Product_ID = Hdn_Product_Id_Tax.Value;
        string Unit_Price = Hdn_unit_Price_Tax.Value;
        string Unit_Discount = Hdn_Discount_Tax.Value;
        string Net_Unit_Price = (Convert.ToDouble(Unit_Price) - Convert.ToDouble(Unit_Discount)).ToString();
        if (Session["Temp_Product_Tax_PQ"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PQ"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                foreach (DataRow DR_Tax in Dt_Cal.Rows)
                {
                    foreach (GridViewRow GVR in gvTaxCalculation.Rows)
                    {
                        TextBox Tax_Percentage = (TextBox)GVR.FindControl("txtTaxValueInPer");
                        HiddenField TaxId = (HiddenField)GVR.FindControl("lblgvTaxId");
                        HiddenField ProductId = (HiddenField)GVR.FindControl("lblgvProductId");
                        if (Tax_Percentage.Text == "")
                            Tax_Percentage.Text = "0.00";
                        //if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value)
                        if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value && DR_Tax["Serial_No"].ToString() == Serial_No)
                        {
                            DR_Tax["Tax_Value"] = Tax_Percentage.Text;
                            DR_Tax["TaxAmount"] = (Convert.ToDouble(Net_Unit_Price) * Convert.ToDouble(Tax_Percentage.Text)) / 100;
                            DR_Tax["Amount"] = Net_Unit_Price;
                        }
                    }
                }
            }
            Session["Temp_Product_Tax_PQ"] = Dt_Cal;
            foreach (DataListItem dl in dlProductDetail.Items)
            {
                if (Dt_Cal.Rows.Count > 0)
                {
                    //DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
                    DataTable Dt_Cal_Temp = new DataView(Dt_Cal, "Product_Id='" + Product_ID + "' And Serial_No='" + Serial_No + "' ", "", DataViewRowState.CurrentRows).ToTable();
                    if (Dt_Cal_Temp.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Dt_Cal_Temp.Rows)
                        {
                            DataTable Dt_Cal_Temp2 = Dt_Cal_Temp.DefaultView.ToTable(true, "Tax_Id", "Tax_Value");
                            double Tax_Val = 0;
                            foreach (DataRow DRR in Dt_Cal_Temp2.Rows)
                            {
                                Tax_Val = Tax_Val + Convert.ToDouble(DRR["Tax_Value"].ToString());
                            }
                            TextBox Tax_Percent = (TextBox)dl.FindControl("txtTaxP");
                            HiddenField hdnProductId = (HiddenField)dl.FindControl("hdngvProductId");
                            HiddenField hdnSerialNo = (HiddenField)dl.FindControl("hdnSerialNo");
                            //if (Product_ID == hdnProductId.Value)
                            if (Product_ID == hdnProductId.Value && Serial_No == hdnSerialNo.Value)
                            {
                                Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                            }
                        }
                    }
                }
            }
            Dt_Cal = null;
            Update_Tax_On_Change();
            Hdn_Product_Id_Tax.Value = "";
            Hdn_Serial_No_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }

    protected void lbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        string id = e.CommandArgument.ToString().Split('/')[0];
        string LocId = e.CommandArgument.ToString().Split('/')[1];

        FUpload1.setID(id, Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + LocId + "/PQuotation", "Purchase", "PQuotation", id, e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
}