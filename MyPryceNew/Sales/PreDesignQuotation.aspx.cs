using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using PegasusDataAccess;

public partial class Sales_PreDesignQuotation : BasePage
{

    #region defined Class Object
    Inv_TaxRefDetail objTaxRefDetail = null;
    Common cmn = null;
    IT_ObjectEntry objObjectEntry = null;
    Set_Approval_Employee objEmpApproval = null;
    Inv_SalesQuotationHeader objSQuoteHeader = null;
    Inv_SalesQuotationDetail ObjSQuoteDetail = null;
    Inv_SalesInquiryHeader objSIHeader = null;
    Inv_SalesInquiryDetail ObjSIDetail = null;
    Contact_PriceList objCustomerPriceList = null;
    Inv_ProductMaster objProductM = null;
    Ems_ContactMaster objContact = null;
    CurrencyMaster objCurrency = null;
    Inv_UnitMaster UM = null;
    EmployeeMaster objEmployee = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_ParameterMaster objInvParam = null;
    Inv_ReferenceMailContact objMailContact = null;
    Set_AddressMaster objAddMaster = null;
    Inv_Product_RelProduct objRelProduct = null;
    LocationMaster objLocation = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string StrUserId = string.Empty;
    string ProductIds = string.Empty;
    string Parameter_Id = string.Empty;
    string ParameterValue = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objSQuoteHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        ObjSQuoteDetail = new Inv_SalesQuotationDetail(Session["DBConnection"].ToString());
        objSIHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        ObjSIDetail = new Inv_SalesInquiryDetail(Session["DBConnection"].ToString());
        objCustomerPriceList = new Contact_PriceList(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objMailContact = new Inv_ReferenceMailContact(Session["DBConnection"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        objRelProduct = new Inv_Product_RelProduct(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        if (!IsPostBack)
        {
           
            Session["isSalesTaxEnabled"] = null;
            Session["IsSalesDiscountEnabled"] = null;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/PreDesignQuotation.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            allPageCode(clsPagePermission);
            ddlTransType.Enabled = true;
            Session["Temp_Product_Tax_PSQ"] = null;
            ddlOption.SelectedIndex = 2;
           
            FillGrid();
            FillProductCategory();
            ViewState["ExchangeRate"] = SystemParameter.GetExchageRate(Session["CurrencyId"].ToString(), Session["LocCurrencyId"].ToString(), Session["DBConnection"].ToString());
            ViewState["DocNo"] = GetDocumentNumber();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            rbtnFormView.Visible = false;
            rbtnAdvancesearchView.Visible = false;
            btnAddNewProduct.Visible = false;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
            //this code is created by jitendra upadhyay when we open direct quotation page throug inquiry
            Session["DtSearchProduct"] = null;
            Session["DtPreDesignItemList"] = null;
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnAdvancesearchView.Checked = false;
            rbtnFormView_OnCheckedChanged(null, null);
            TaxandDiscountParameter();
            FillProductCurrency();
            ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
            ddlPCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
            FillTransactionType();
        }
        if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            Trans_Div.Visible = false;
        Page.MaintainScrollPositionOnPostBack = true;
        if (IsPostBack && hdfCurrentRow.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "setScrollAndRow()", true);
        }
    }

    public void allPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSQuoteSave.Visible = clsPagePermission.bAdd;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }

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
    private void FillProductCategory()
    {
        using (DataTable dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString()))
        {
            if (dsCategory.Rows.Count > 0)
            {
                ddlProductcategory.Items.Clear();

                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                objPageCmn.FillData((object)ddlProductcategory, dsCategory, "Category_Name", "Category_Id");

            }
            else
            {
                ddlProductcategory.Items.Insert(0, "--Select--");
            }
        }
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void TaxandDiscountParameter()
    {
        //for tax visibility
        GvDetail.Columns[14].Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        GvDetail.Columns[15].Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        lblTaxP.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        txtTaxP.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        Label2.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        txtTaxV.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        lblPriceAfterTax.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        txtPriceAfterTax.Visible = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //for discount visibility
        GvDetail.Columns[11].Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        GvDetail.Columns[12].Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        lblDiscountP.Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()); 
        txtDiscountP.Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()); 
        Label3.Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()); 
        txtDiscountV.Visible = Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()); ;

    }
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "13", "57", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #region System defined Function
 
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;


        editid.Value = e.CommandArgument.ToString();
        edit();
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
        if (Lbl_Tab_New.Text == "View")
        {
            btnSQuoteSave.Visible = false;
            BtnReset.Visible = false;
        }
        else
        {
            btnSQuoteSave.Visible = true;
            BtnReset.Visible = true;
        }

       

        if (objSenderID == "lnkViewDetail")
        {
            btnSQuoteSave.Visible = false;
            BtnReset.Visible = false;
        }
        else
        {
            rbtnFormView.Visible = true;
            rbtnAdvancesearchView.Visible = true;
            rbtnFormView.Checked = true;
            rbtnFormView_OnCheckedChanged(null, null);
            btnSQuoteSave.Visible = true;
            BtnReset.Visible = true;
        }

    }
    public void edit()
    {
        string CustomerId = string.Empty;
        CustomerId = "0";
        string ContactId = string.Empty;
        ContactId = "0";
        using (DataTable dtQuoteEdit = objSQuoteHeader.GetQuotationHeaderAllBySQuotationId("0", "0", "0", editid.Value))
        {
            if (dtQuoteEdit.Rows.Count > 0)
            {
                if (dtQuoteEdit.Rows[0]["Trans_Type"].ToString() != "")
                    ddlTransType.SelectedValue = dtQuoteEdit.Rows[0]["Trans_Type"].ToString();
                else
                    ddlTransType.SelectedIndex = 0;
                Get_Tax_From_DB();

                txtTemplateName.Text = dtQuoteEdit.Rows[0]["Condition2"].ToString();
                txtTemplateNameLocal.Text = dtQuoteEdit.Rows[0]["Condition3"].ToString();
                try
                {
                    ddlCurrency.SelectedValue = dtQuoteEdit.Rows[0]["Currency_Id"].ToString();
                }
                catch
                {

                }

                try
                {
                    ddlProductcategory.SelectedValue = dtQuoteEdit.Rows[0]["Field5"].ToString();
                }
                catch
                {

                }
                //For Approval Status
                //Add Detail Grid For Edit


                using (DataTable dtDetail = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id("0", "0", "0", editid.Value,Session["FinanceYearId"].ToString()))
                {

                    if (dtDetail.Rows.Count > 0)
                    {
                        dtDetail.Columns["Field1"].ColumnName = "UnitId";
                        dtDetail.Columns["UnitPrice"].ColumnName = "SalesPrice";
                        dtDetail.Columns["Field2"].ColumnName = "EstimatedUnitPrice";
                        dtDetail.Columns["Field4"].ColumnName = "PurchaseProductPrice";
                        dtDetail.Columns["Field5"].ColumnName = "PurchaseProductDescription";

                        dtDetail.DefaultView.Sort = "Serial_No Asc";
                        Session["DtPreDesignItemList"] = dtDetail;
                        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                        objPageCmn.FillData((object)GvDetail, dtDetail, "", "");



                        GvDetail.HeaderRow.Cells[8].Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Estimated", Session["DBConnection"].ToString());
                        //GvDetail.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(ddlCurrency.SelectedValue.ToString(), "Quanti");

                        GvDetail.HeaderRow.Cells[9].Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Price", Session["DBConnection"].ToString());
                        GvDetail.HeaderRow.Cells[10].Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "", Session["DBConnection"].ToString());

                        GvDetail.HeaderRow.Cells[12].Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Value", Session["DBConnection"].ToString());
                        GvDetail.HeaderRow.Cells[15].Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Value", Session["DBConnection"].ToString());

                        GvDetail.HeaderRow.Cells[17].Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Net Amount", Session["DBConnection"].ToString());

                        foreach (GridViewRow Row in GvDetail.Rows)
                        {
                            try
                            {
                                DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                                {
                                    ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                                    ((Panel)Row.FindControl("PopupMenu")).Visible = false;

                                }
                            }
                            catch
                            {

                            }

                            Label lblgvEstimatedUnitPrice = (Label)Row.FindControl("lblgvEstimatedUnitPrice");
                            TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");
                            TextBox txtgvUnitPrice = (TextBox)Row.FindControl("txtgvUnitPrice");
                            TextBox txtgvQuantityPrice = (TextBox)Row.FindControl("txtgvQuantityPrice");
                            TextBox txtgvDiscountP = (TextBox)Row.FindControl("txtgvDiscountP");
                            TextBox txtgvDiscountV = (TextBox)Row.FindControl("txtgvDiscountV");
                            TextBox txtgvPriceAfterDiscount = (TextBox)Row.FindControl("txtgvPriceAfterDiscount");
                            TextBox txtgvTaxP = (TextBox)Row.FindControl("txtgvTaxP");
                            TextBox txtgvTaxV = (TextBox)Row.FindControl("txtgvTaxV");
                            TextBox txtgvPriceAfterTax = (TextBox)Row.FindControl("txtgvPriceAfterTax");
                            TextBox txtgvTotal = (TextBox)Row.FindControl("txtgvTotal");
                            if (lblgvQuantity.Text == "")
                            {
                                lblgvQuantity.Text = "0";
                            }
                            if (txtgvUnitPrice.Text == "")
                            {
                                txtgvUnitPrice.Text = "0";
                            }
                            txtgvQuantityPrice.Text = GetAmountDecimal((Convert.ToDouble(lblgvQuantity.Text) * Convert.ToDouble(txtgvUnitPrice.Text)).ToString());
                            lblgvEstimatedUnitPrice.Text = GetAmountDecimal(lblgvEstimatedUnitPrice.Text);
                            lblgvQuantity.Text = GetAmountDecimal(lblgvQuantity.Text);
                            txtgvUnitPrice.Text = GetAmountDecimal(txtgvUnitPrice.Text);
                            txtgvDiscountP.Text = GetAmountDecimal(txtgvDiscountP.Text);
                            txtgvDiscountV.Text = GetAmountDecimal(txtgvDiscountV.Text);
                            txtgvTaxP.Text = GetAmountDecimal(txtgvTaxP.Text);
                            txtgvTaxV.Text = GetAmountDecimal(txtgvTaxV.Text);
                            txtgvTotal.Text = GetAmountDecimal(txtgvTotal.Text);


                        }
                        txtAmount.Text =SystemParameter.GetScaleAmount(GetAmountDecimal(dtQuoteEdit.Rows[0]["Amount"].ToString()),"0");
                        lblAmount.Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Gross Amount", Session["DBConnection"].ToString());
                        lblTotalAmount.Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Net Amount", Session["DBConnection"].ToString());
                    }
                }

                txtTaxP.Text = GetAmountDecimal(dtQuoteEdit.Rows[0]["TaxPercent"].ToString());
                txtTaxV.Text = GetAmountDecimal(GetAmountDecimal(dtQuoteEdit.Rows[0]["TaxValue"].ToString()));
                Label2.Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Value", Session["DBConnection"].ToString());
                Label3.Text = SystemParameter.GetCurrencySmbol(Session["LocCurrencyId"].ToString(), "Value", Session["DBConnection"].ToString());
                //updatd by jitendra upadhyay on 16-dec-2013
                //because the tax perentage and value is not coming exactly
                try
                {
                    txtPriceAfterTax.Text = GetAmountDecimal((Decimal.Parse(txtAmount.Text) + Decimal.Parse(txtTaxV.Text)).ToString());
                }
                catch
                {
                    txtPriceAfterTax.Text = "0";
                }
                txtDiscountP.Text = GetAmountDecimal(dtQuoteEdit.Rows[0]["DiscountPercent"].ToString());
                txtDiscountV.Text = GetAmountDecimal(GetAmountDecimal(dtQuoteEdit.Rows[0]["DiscountValue"].ToString()));
                try
                {
                    txtTotalAmount.Text = GetAmountDecimal((Decimal.Parse(txtPriceAfterTax.Text) - Decimal.Parse(txtDiscountV.Text)).ToString());
                }
                catch
                {
                    txtTotalAmount.Text = "0";
                }


                txtHeader.Content = dtQuoteEdit.Rows[0]["Header"].ToString();
                txtFooter.Content = dtQuoteEdit.Rows[0]["Footer"].ToString();
                txtCondition1.Content = dtQuoteEdit.Rows[0]["Condition1"].ToString();
                Editor1.Content = dtQuoteEdit.Rows[0]["Condition5"].ToString();
                GvDetail.Columns[13].Visible = false;
                GvDetail.Columns[16].Visible = false;
            }
        }

    }

    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "Quotation_Date")
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
                return;
            }
        }
        FillGrid(1);
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvSalesQuote_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (GvSalesQuote.Attributes["CurrentSortField"] != null &&
            GvSalesQuote.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvSalesQuote.Attributes["CurrentSortField"])
            {
                if (GvSalesQuote.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvSalesQuote.Attributes["CurrentSortField"] = sortField;
        GvSalesQuote.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvSalesQuotationCurrentPageIndex.Value));

    }

    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvSalesQuotationCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objSQuoteHeader.DeleteQuotationHeader("0", "0", "0", e.CommandArgument.ToString(), "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            objEmpApproval.Delete_Approval_Transaction("8", "0", "0", "0", "0", e.CommandArgument.ToString());
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGrid(1);
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        FillGrid(1);
    }
    protected void btnSQuoteCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    protected void btnSQuoteSave_Click(object sender, EventArgs e)
    {

        //if (txtTemplateName.Text.Trim() == "")
        //{
        //    DisplayMessage("Enter Template Name");
        //    txtTemplateName.Focus();
        //    return;
        //}

        if (ddlCurrency.SelectedIndex == 0)
        {
            DisplayMessage("select currency");
            ddlCurrency.Focus();
            return;
        }
        if (ddlTransType.SelectedIndex == 0 && Trans_Div.Visible == true)
        {
            DisplayMessage("Please Select Transaction Type");
            return;
        }

        string Quotationid = string.Empty;
        string strContactId = string.Empty;
        string strAddressId = string.Empty;
        strAddressId = "0";

        if (GvDetail.Rows.Count > 0)
        {

        }
        else
        {
            DisplayMessage("You have no Product For Generate Quotation");
            return;
        }


        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        if (txtTaxP.Text == "")
        {
            txtTaxP.Text = "0";
        }
        if (txtTaxV.Text == "")
        {
            txtTaxV.Text = "0";
        }
        if (txtDiscountP.Text == "")
        {
            txtDiscountP.Text = "0";
        }
        if (txtDiscountV.Text == "")
        {
            txtDiscountV.Text = "0";
        }
        if (txtTotalAmount.Text == "")
        {
            txtTotalAmount.Text = "0";
        }

        string strcategory = string.Empty;

        strcategory = "0";
        if (ddlProductcategory.SelectedIndex > 0)
        {
            strcategory = ddlProductcategory.SelectedValue;
        }


        int b = 0;
        if (editid.Value != "")
        {
            Quotationid = editid.Value;



            b = objSQuoteHeader.UpdateQuotationHeader("0", "0", "0", editid.Value, editid.Value, DateTime.Now.ToString(), "0", ddlCurrency.SelectedValue, "0", txtAmount.Text, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, "Close", txtHeader.Content, txtFooter.Content, txtCondition1.Content, txtTemplateName.Text, txtTemplateNameLocal.Text, "", Editor1.Content, "False", "0", "", "0", "0", strAddressId, "Approved", strcategory, "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString());

            if (b != 0)
            {
                ObjSQuoteDetail.DeleteQuotationDetail("0", "0", "0", editid.Value);
                objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PSQ", editid.Value);
                foreach (GridViewRow gvr in GvDetail.Rows)
                {
                    Label lblgvSerialNo = (Label)gvr.FindControl("lblSerialNo");
                    HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
                    Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
                    Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");
                    Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");

                    HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
                    HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdnUnitId");
                    TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
                    TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                    TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                    TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
                    TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
                    TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                    TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
                    TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
                    AjaxControlToolkit.HTMLEditor.Editor hdnSuggestedProductdesc = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
                    AjaxControlToolkit.HTMLEditor.Editor EditorDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

                    if (txtgvUnitPrice.Text == "")
                    {
                        txtgvUnitPrice.Text = "0";
                    }
                    if (txtgvTaxP.Text == "")
                    {
                        txtgvTaxP.Text = "0";
                    }
                    if (txtgvTaxV.Text == "")
                    {
                        txtgvTaxV.Text = "0";
                    }
                    if (txtgvPriceAfterTax.Text == "")
                    {
                        txtgvPriceAfterTax.Text = "0";
                    }
                    if (txtgvDiscountP.Text == "")
                    {
                        txtgvDiscountP.Text = "0";
                    }
                    if (txtgvDiscountV.Text == "")
                    {
                        txtgvDiscountV.Text = "0";
                    }
                    if (txtgvPriceAfterDiscount.Text == "")
                    {
                        txtgvPriceAfterDiscount.Text = "0";
                    }

                    int Detail_Id = ObjSQuoteDetail.InsertQuotationDetail("0", "0", "0", editid.Value, lblgvSerialNo.Text, hdngvProductId.Value, EditorDescription.Content, ddlCurrency.SelectedValue, txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, txtgvTaxV.Text, txtgvPriceAfterTax.Text, txtgvDiscountP.Text, txtgvDiscountV.Text, txtgvPriceAfterDiscount.Text, "False", "0", hdngvUnitId.Value, lblgvEstimatedUnitPrice.Text, hdnSuggestedProductdesc.Content, lblProductPurchasePrice.Text.Trim(), lblProductPurchaseDiscription.Text.Trim(), "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    Insert_Tax("GvDetail", editid.Value, Detail_Id.ToString(), gvr);

                }

                DisplayMessage("Record Updated", "green");
                Reset();
                editid.Value = "";
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            string IsApproved = "Pending";





            b = objSQuoteHeader.InsertQuotationHeader("0", "0", "0", "", DateTime.Now.ToString(), "0", ddlCurrency.SelectedValue, "0", txtAmount.Text, txtTaxP.Text, txtTaxV.Text, txtDiscountP.Text, txtDiscountV.Text, "Close", txtHeader.Content, txtFooter.Content, txtCondition1.Content, txtTemplateName.Text, txtTemplateNameLocal.Text, "", Editor1.Content, "False", "0", "", "0", "0", strAddressId, IsApproved.Trim(), strcategory, "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ddlTransType.SelectedValue.ToString());
            if (b != 0)
            {
                string strMaxId = string.Empty;
                strMaxId = b.ToString();


                if (strMaxId != "" && strMaxId != "0")
                {

                    objSQuoteHeader.Updatecode(b.ToString(), b.ToString());

                    Quotationid = strMaxId;
                    objTaxRefDetail.DeleteRecord_By_RefType_and_RefId("PSQ", strMaxId);
                    foreach (GridViewRow gvr in GvDetail.Rows)
                    {
                        Label lblgvSerialNo = (Label)gvr.FindControl("lblSerialNo");
                        HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
                        HiddenField hdngvCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
                        HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdnUnitId");
                        TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
                        TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
                        TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
                        TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
                        TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
                        TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
                        TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
                        TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
                        Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
                        Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");
                        Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");


                        AjaxControlToolkit.HTMLEditor.Editor hdnSuggestedProductdesc = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
                        AjaxControlToolkit.HTMLEditor.Editor EditorDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

                        if (txtgvUnitPrice.Text == "")
                        {
                            txtgvUnitPrice.Text = "0";
                        }
                        if (txtgvTaxP.Text == "")
                        {
                            txtgvTaxP.Text = "0";
                        }
                        if (txtgvTaxV.Text == "")
                        {
                            txtgvTaxV.Text = "0";
                        }
                        if (txtgvPriceAfterTax.Text == "")
                        {
                            txtgvPriceAfterTax.Text = "0";
                        }
                        if (txtgvDiscountP.Text == "")
                        {
                            txtgvDiscountP.Text = "0";
                        }
                        if (txtgvDiscountV.Text == "")
                        {
                            txtgvDiscountV.Text = "0";
                        }
                        if (txtgvPriceAfterDiscount.Text == "")
                        {
                            txtgvPriceAfterDiscount.Text = "0";
                        }


                        int Detail_Id = ObjSQuoteDetail.InsertQuotationDetail("0", "0", "0", strMaxId, lblgvSerialNo.Text, hdngvProductId.Value, EditorDescription.Content, ddlCurrency.SelectedValue, txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, txtgvTaxV.Text, txtgvPriceAfterTax.Text, txtgvDiscountP.Text, txtgvDiscountV.Text, txtgvPriceAfterDiscount.Text, "False", "0", hdngvUnitId.Value, lblgvEstimatedUnitPrice.Text.Trim(), hdnSuggestedProductdesc.Content, lblProductPurchasePrice.Text.Trim(), lblProductPurchaseDiscription.Text.Trim(), "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        Insert_Tax("GvDetail", strMaxId, Detail_Id.ToString(), gvr);
                    }

                }
                DisplayMessage("Record Saved","green");

                FillGrid();
                Reset();

            }
            else
            {
                DisplayMessage("Record  Not Saved");
            }
        }


    }
    public string GetApprovalStatus(int TransID)
    {
        return ((DataTable)(objSQuoteHeader.GetQuotationHeaderAllBySQuotationId(StrCompId, StrBrandId, StrLocationId, TransID.ToString()))).Rows[0]["Field4"].ToString();
    }
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
    }

    protected void GvSalesQuoteBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvSalesQuoteBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtSQuotationBin"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesQuoteBin, dt, "", "");
        string temp = string.Empty;
        for (int i = 0; i < GvSalesQuoteBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvSalesQuoteBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvSalesQuoteBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        dt.Dispose();

    }
    protected void GvSalesQuoteBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";

        DataTable dt = objSQuoteHeader.GetQuotationHeaderAllFalse("0", "0", "0");
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtSQuotationBin"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesQuoteBin, dt, "", "");
        dt.Dispose();
    }
    public void FillGridBin()
    {
        using (DataTable dt = objSQuoteHeader.GetQuotationHeaderAllFalse("0", "0", "0"))
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuoteBin, dt, "", "");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuoteBin, dt, "", "");
            Session["dtSQuotationBin"] = dt;
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            lblSelectedRecord.Text = "";
            if (dt.Rows.Count == 0)
            {
             
                imgBtnRestore.Visible = false;
            }
            else
            {
             
                imgBtnRestore.Visible = true;
            }
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlFieldNameBin.SelectedItem.Value == "Quotation_Date")
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

            DataTable dtCust = (DataTable)Session["dtSQuotationBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtSQuotationBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuoteBin, view.ToTable(), "", "");
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
        CheckBox chkSelAll = ((CheckBox)GvSalesQuoteBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvSalesQuoteBin.Rows.Count; i++)
        {
            ((CheckBox)GvSalesQuoteBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvSalesQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvSalesQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvSalesQuoteBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
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
        HiddenField lb = (HiddenField)GvSalesQuoteBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvSalesQuoteBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueBinDate.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtSQuotationBin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["SQuotation_Id"]))
                {
                    lblSelectedRecord.Text += dr["SQuotation_Id"] + ",";
                }
            }
            for (int i = 0; i < GvSalesQuoteBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvSalesQuoteBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvSalesQuoteBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtSQuotationBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvSalesQuoteBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
        dtPbrand.Dispose();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        //in use
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objSQuoteHeader.DeleteQuotationHeader("0", "0", "0", lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


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
            foreach (GridViewRow Gvr in GvSalesQuoteBin.Rows)
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
        strWhereClause = "Location_id='0' and isActive='true'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        int totalRows = 0;
        using (DataTable dt = objSQuoteHeader.getQuotationList((currentPageIndex - 1).ToString(), Session["GridSize"].ToString(), GvSalesQuote.Attributes["CurrentSortField"], GvSalesQuote.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvSalesQuote, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

            }
            else
            {
                GvSalesQuote.DataSource = null;
                GvSalesQuote.DataBind();
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
        ddlTransType.Enabled = true;
        Session["Temp_Product_Tax_PSQ"] = null;
        txtAmount.Text = "";
        txtTaxP.Text = "";
        txtTaxV.Text = "";
        txtPriceAfterTax.Text = "";
        txtDiscountP.Text = "";
        txtDiscountV.Text = "";
        txtTotalAmount.Text = "";
        txtHeader.Content = "";
        txtFooter.Content = "";
        txtCondition1.Content = "";
        ddlProductcategory.SelectedIndex = 0;
        // btnClosePanel_Click(null, null);
        GvDetail.DataSource = null;
        GvDetail.DataBind();
        FillGrid();
        btnAddNewProduct.Visible = false;
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueBin.Text = "";
        txtValueBinDate.Text = "";
        lblSelectedRecord.Text = "";
        txtValueBinDate.Visible = false;
        txtValueBin.Visible = true;
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        // rbtnFormView.Visible = false;
        // rbtnAdvancesearchView.Visible = false;
        btnAddNewProduct.Visible = false;
        //  btnAddProductScreen.Visible = false;
        btnAddtoList.Visible = false;
        Session["DtSearchProduct"] = null;
        Session["DtPreDesignItemList"] = null;
        lblTotal.Visible = false;
        txtTemplateName.Text = "";
        txtTemplateNameLocal.Text = "";
        Editor1.Content = null;
        ddlCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
        ddlPCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
    }
    #endregion
    #region Add Product Concept
   
    //updated by varsha 
    public string SuggestedProductName(string ProductId, string SerialNo)
    {

        string Product_Name = string.Empty;

        if (editid.Value == "")
        {

            DataTable dtDetail = ((DataTable)Session["DtPreDesignItemList"]);


            try
            {
                dtDetail = new DataView(dtDetail, "Product_Id=" + ProductId + " and Serial_No=" + SerialNo + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {


            }


            if (!Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsProductNameShow").Rows[0]["ParameterValue"].ToString()))
            {
                Product_Name = dtDetail.Rows[0]["ProductDescription"].ToString();
            }
            else
            {
                Product_Name = dtDetail.Rows[0]["SuggestedProductName"].ToString();
            }

            dtDetail.Dispose();

        }
        else
        {
            DataTable dtSquotationDetail = ((DataTable)Session["DtPreDesignItemList"]);

            try
            {
                dtSquotationDetail = new DataView(dtSquotationDetail, "Product_Id=" + ProductId + "  and Serial_No=" + SerialNo + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {


            }


            if (!Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsProductNameShow").Rows[0]["ParameterValue"].ToString()))
            {
                Product_Name = dtSquotationDetail.Rows[0]["ProductDescription"].ToString();
            }
            else
            {
                Product_Name = dtSquotationDetail.Rows[0]["SuggestedProductName"].ToString();
            }
            dtSquotationDetail.Dispose();
        }

        return Product_Name;
    }
    
   
    protected string GetSalesInquiryNo(string strSalesInquiryId)
    {
        string strSalesInquiryNo = string.Empty;
        if (strSalesInquiryId != "0" && strSalesInquiryId != "")
        {
            using (DataTable dtSINo = objSIHeader.GetSIHeaderAllBySInquiryId(StrCompId, StrBrandId, StrLocationId, strSalesInquiryId))
            {
                if (dtSINo.Rows.Count > 0)
                {
                    strSalesInquiryNo = dtSINo.Rows[0]["SInquiryNo"].ToString();
                }
            }
        }
        else
        {
            strSalesInquiryNo = "";
        }
        return strSalesInquiryNo;
    }
   
   
    #endregion
    #region Add Request Section
    protected void GvDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {

        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.S_No_;
            cell.ColumnSpan = 2;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 5;
            cell.Text = Resources.Attendance.Product_Detail;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Unit;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Gross_Price;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Discount;

            if (Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
            }
            else
            {
                row.Controls.Add(cell);
            }


            cell = new TableHeaderCell();
            cell.ColumnSpan = 2;
            cell.Text = Resources.Attendance.Tax;
            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == false)
            {
                cell.ColumnSpan = 0;
            }
            else
            {
                row.Controls.Add(cell);
            }

            cell = new TableHeaderCell();
            cell.ColumnSpan = 1;
            cell.Text = Resources.Attendance.Line_total;
            row.Controls.Add(cell);
            gvProduct.Controls[0].Controls.Add(row);


        }
    }
    #endregion
    #region Calculations
    protected void txtTaxP_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();
        TaxDiscountFromHeader(false);
    }
    protected void txtTaxV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", "", txtTaxV.Text, "", txtDiscountV.Text, false, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
        txtTaxP.Text = str[3].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();
        TaxDiscountFromHeader(false);
    }
    protected void txtDiscountP_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", txtDiscountP.Text, "", false, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
        txtDiscountV.Text = str[2].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();
        TaxDiscountFromHeader(true);
    }
    protected void txtDiscountV_TextChanged(object sender, EventArgs e)
    {
        if (txtAmount.Text == "")
        {
            txtAmount.Text = "0";
        }
        string[] str = Common.TaxDiscountCaluculation(txtAmount.Text, "0", txtTaxP.Text, "", "", txtDiscountV.Text, false, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());
        txtTaxV.Text = str[4].ToString();
        txtDiscountP.Text = str[1].ToString();
        txtTotalAmount.Text = str[5].ToString();
        txtPriceAfterTax.Text = str[5].ToString();
        TaxDiscountFromHeader(true);
    }

    #endregion
    #region Grid Calculations
    public void TaxDiscountFromHeader(bool IsDiscount)
    {


        double netPrice = 0;
        double netTax = 0;
        double netDiscount = 0;


        foreach (GridViewRow Row in GvDetail.Rows)
        {
            TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");
            string[] str;
            if (IsDiscount)
            {
                str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("txtgvUnitPrice")).Text, lblgvQuantity.Text, ((TextBox)Row.FindControl("txtgvTaxP")).Text, "", txtDiscountP.Text, "", true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());

                ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
                ((TextBox)Row.FindControl("txtgvDiscountV")).Text = str[2].ToString();
                ((TextBox)Row.FindControl("txtgvDiscountP")).Text = str[1].ToString();
                ((TextBox)Row.FindControl("txtgvPriceAfterTax")).Text = str[5].ToString();
                ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();
            }
            else
            {
                str = Common.TaxDiscountCaluculation(((TextBox)Row.FindControl("txtgvUnitPrice")).Text, lblgvQuantity.Text, txtTaxP.Text, "", "", ((TextBox)Row.FindControl("txtgvDiscountV")).Text, true, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
                ((TextBox)Row.FindControl("txtgvTaxV")).Text = str[4].ToString();
                ((TextBox)Row.FindControl("txtgvTaxP")).Text = str[3].ToString();
                ((TextBox)Row.FindControl("txtgvPriceAfterTax")).Text = str[5].ToString();
                ((TextBox)Row.FindControl("txtgvTotal")).Text = str[5].ToString();
            }

        }
    }
    protected void lblgvQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)row.FindControl("hdnProductId");
        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
        TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
        TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
        TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");

        Label lblgvSerialNo = (Label)row.FindControl("lblgvSerialNo");

        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";

        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (txtgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Amount(txtgvUnitPrice.Text, hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }
        string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", txtgvDiscountP.Text, "", true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());

        txtgvQuantityPrice.Text = strtotal[0].ToString();
        txtgvDiscountP.Text = strtotal[1].ToString();
        txtgvDiscountV.Text = strtotal[2].ToString();
        txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxV.Text = strtotal[4].ToString();
        txtgvPriceAfterTax.Text = strtotal[5].ToString();
        txtgvTotal.Text = strtotal[5].ToString();
        HeadearCalculateGrid();
    }
    protected void txtgvUnitPrice_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)row.FindControl("hdnProductId");
        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
        TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
        TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
        TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");
        Label lblgvEstimatedUnitPrice = (Label)row.FindControl("lblgvEstimatedUnitPrice");

        Label lblgvSerialNo = (Label)row.FindControl("lblgvSerialNo");

        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (txtgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Amount(txtgvUnitPrice.Text, hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }
        string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", txtgvDiscountP.Text, "", true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());
        txtgvQuantityPrice.Text = strtotal[0].ToString();
        txtgvDiscountP.Text = strtotal[1].ToString();
        txtgvDiscountV.Text = strtotal[2].ToString();
        txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxV.Text = strtotal[4].ToString();
        txtgvPriceAfterTax.Text = strtotal[5].ToString();
        txtgvTotal.Text = strtotal[5].ToString();
        txtgvUnitPrice.Text = GetAmountDecimal(txtgvUnitPrice.Text);
        lblgvEstimatedUnitPrice.Text = GetAmountDecimal(lblgvEstimatedUnitPrice.Text);
        HeadearCalculateGrid();
    }
    protected void txtgvTaxP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
        TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
        TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
        TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");

        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }
        string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, txtgvTaxP.Text, "", "", txtgvDiscountV.Text, true, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
        txtgvTaxV.Text = strtotal[4].ToString();
        txtgvPriceAfterTax.Text = strtotal[5].ToString();
        txtgvTotal.Text = strtotal[5].ToString();
        HeadearCalculateGrid();
    }
    protected void txtgvTaxV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;

        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
        TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
        TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
        TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");

        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }
        string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, "", txtgvTaxV.Text, "", txtgvDiscountV.Text, true, Session["LocCurrencyId"].ToString(), true, Session["DBConnection"].ToString());
        txtgvTaxP.Text = strtotal[3].ToString();
        txtgvPriceAfterTax.Text = strtotal[5].ToString();
        txtgvTotal.Text = strtotal[5].ToString();
        HeadearCalculateGrid();
    }
    protected void txtgvDiscountP_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)row.FindControl("hdnProductId");
        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
        TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
        TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
        TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");
        Label lblgvSerialNo = (Label)row.FindControl("lblgvSerialNo");
        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }

        double TotalTax = 0;
        if (txtgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Amount(txtgvUnitPrice.Text, hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }
        string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", txtgvDiscountP.Text, "", true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());
        txtgvQuantityPrice.Text = strtotal[0].ToString();
        txtgvDiscountV.Text = strtotal[2].ToString();
        txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxV.Text = strtotal[4].ToString();
        txtgvPriceAfterTax.Text = strtotal[5].ToString();
        txtgvTotal.Text = strtotal[5].ToString();
        HeadearCalculateGrid();

    }
    protected void txtgvDiscountV_TextChanged(object sender, EventArgs e)
    {
        GridViewRow row = (GridViewRow)((TextBox)sender).Parent.Parent;
        HiddenField hdnProductId = (HiddenField)row.FindControl("hdnProductId");
        TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
        TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
        TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
        TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
        TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
        TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
        TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
        TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
        TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
        TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");

        Label lblgvSerialNo = (Label)row.FindControl("lblgvSerialNo");

        if (txtgvUnitPrice.Text == "")
        {
            txtgvUnitPrice.Text = "0";
        }
        if (lblgvQuantity.Text == "")
        {
            lblgvQuantity.Text = "0";
        }
        double TotalTax = 0;
        if (txtgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
        {
            TotalTax = Get_Tax_Amount(txtgvUnitPrice.Text, hdnProductId.Value, lblgvSerialNo.Text);
            double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
        }
        string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", "", txtgvDiscountV.Text, true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());


        txtgvQuantityPrice.Text = strtotal[0].ToString();
        txtgvDiscountP.Text = strtotal[1].ToString();
        txtgvTaxP.Text = strtotal[3].ToString();
        txtgvTaxV.Text = strtotal[4].ToString();
        txtgvPriceAfterTax.Text = strtotal[5].ToString();
        txtgvTotal.Text = strtotal[5].ToString();

        HeadearCalculateGrid();

    }
    public void SetDecimal()
    {
        foreach (GridViewRow gvr in GvDetail.Rows)
        {


            Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
            TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
            TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
            TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
            TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
            TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
            TextBox txtgvTotal = (TextBox)gvr.FindControl("txtgvTotal");
            if (lblgvQuantity.Text == "")
            {
                lblgvQuantity.Text = "0";
            }
            if (txtgvUnitPrice.Text == "")
            {
                txtgvUnitPrice.Text = "0";
            }
            txtgvQuantityPrice.Text = GetAmountDecimal((Convert.ToDouble(lblgvQuantity.Text) * Convert.ToDouble(txtgvUnitPrice.Text)).ToString());
            lblgvEstimatedUnitPrice.Text = GetAmountDecimal(lblgvEstimatedUnitPrice.Text);
            lblgvQuantity.Text = GetAmountDecimal(lblgvQuantity.Text);
            txtgvUnitPrice.Text = GetAmountDecimal(txtgvUnitPrice.Text);
            txtgvDiscountP.Text = GetAmountDecimal(txtgvDiscountP.Text);
            txtgvDiscountV.Text = GetAmountDecimal(txtgvDiscountV.Text);
            txtgvTaxP.Text = GetAmountDecimal(txtgvTaxP.Text);
            txtgvTaxV.Text = GetAmountDecimal(txtgvTaxV.Text);
            txtgvTotal.Text = GetAmountDecimal(txtgvTotal.Text);


        }
        txtAmount.Text = SystemParameter.GetScaleAmount(GetAmountDecimal(txtAmount.Text),"0");
        txtTaxP.Text = GetAmountDecimal(txtTaxP.Text);
        txtDiscountP.Text = GetAmountDecimal(txtDiscountP.Text);
        txtDiscountV.Text = SystemParameter.GetScaleAmount(GetAmountDecimal(txtDiscountV.Text),"0");
        txtTaxV.Text = SystemParameter.GetScaleAmount(GetAmountDecimal(txtTaxV.Text),"0");
        txtTotalAmount.Text = SystemParameter.GetScaleAmount(GetAmountDecimal(txtTotalAmount.Text),"0");
    }
    public string GetAmountDecimal(string Amount)
    {

        return SystemParameter.GetAmountWithDecimal(Amount, Session["LoginLocDecimalCount"].ToString());


    }
    #endregion
    public string getStatus(string Status)
    {
        if (Status == "FWInPur")
        {
            Status = "Forward In Purchase";
        }
        return Status;
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
        if (ddlFieldName.SelectedItem.Value == "Quotation_Date")
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
        if (ddlFieldNameBin.SelectedItem.Value == "Quotation_Date")
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
    #region Add Product Concept
    private void FillUnit()
    {
        DataTable dsUnit = null;
        dsUnit = UM.GetUnitMaster(StrCompId);
        if (dsUnit.Rows.Count > 0)
        {   //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlUnit, dsUnit, "Unit_Name", "Unit_Id");

        }
    }
    private void FillProductCurrency()
    {

        using (DataTable dsPCurrency = objCurrency.GetCurrencyMaster())
        {
            if (dsPCurrency.Rows.Count > 0)
            {   //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                objPageCmn.FillData((object)ddlPCurrency, dsPCurrency, "Currency_Name", "Currency_ID");
                objPageCmn.FillData((object)ddlCurrency, dsPCurrency, "Currency_Name", "Currency_ID");

            }
            else
            {
                ddlPCurrency.Items.Insert(0, "--Select--");
                ddlPCurrency.SelectedIndex = 0;
                ddlCurrency.Items.Insert(0, "--Select--");
                ddlCurrency.SelectedIndex = 0;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetProductName_PreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            dt = null;
            return txt;
        }
        catch
        {
            return null;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
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
        catch
        {
            return null;
        }
    }
    protected void btnAddNewProduct_Click(object sender, EventArgs e)
    {
        pnlProduct1.Visible = true;
        ResetProduct();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);

        Session["DtSearchProduct"] = null;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string ParameterValue = string.Empty;
        if (txtProductName.Text != "")
        {
            if (Session["DtPreDesignItemList"] != null)
            {

                if (new DataView(((DataTable)Session["DtPreDesignItemList"]), "SuggestedProductName='" + txtProductcode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
                {

                    DisplayMessage("Product is already exists!");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;

                }
            }


            DataTable dtProduct = new DataTable();
            try
            {
                dtProduct = objProductM.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductName.Text.ToString());

            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {


                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();

                string strUnitId = dtProduct.Rows[0]["UnitId"].ToString();
                if (strUnitId != "0" && strUnitId != "")
                {
                    ddlUnit.SelectedValue = strUnitId;
                }
                else
                {
                    FillUnit();
                }
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
                FillRelatedProduct(hdnNewProductId.Value);

            }
            else
            {
                GvRelatedProduct.DataSource = null;
                GvRelatedProduct.DataBind();
                hdnNewProductId.Value = "0";
                txtPDesc.Visible = true;
                pnlPDescription.Visible = false;

            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
            GvRelatedProduct.DataSource = null;
            GvRelatedProduct.DataBind();
        }

    }
    public void FillRelatedProduct(string ProductId)
    {
        DataTable dsUnit = UM.GetUnitMaster(Session["CompId"].ToString());
        DataTable dsPCurrency = objCurrency.GetCurrencyMaster();
        using (DataTable dtRelatedProduct = objRelProduct.GetRelatedProductByProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId))
        {
            if (dtRelatedProduct.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvRelatedProduct, dtRelatedProduct, "", "");

                foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
                {

                    DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
                    DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
                    HiddenField hdnUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
                    TextBox txtquantity = (TextBox)gvRow.FindControl("txtquantity");

                    txtquantity.Text = "1";

                    if (dsUnit.Rows.Count > 0)
                    {
                        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                        objPageCmn.FillData((object)ddlgvUnit, dsUnit, "Unit_Name", "Unit_Id");

                        try
                        {
                            ddlgvUnit.SelectedValue = hdnUnitId.Value;
                        }
                        catch
                        {
                        }
                    }
                    if (dsPCurrency.Rows.Count > 0)
                    {
                        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

                        objPageCmn.FillData((object)ddlgvCurrency, dsPCurrency, "Currency_Name", "Currency_ID");

                        try
                        {
                            ddlgvCurrency.SelectedValue = Session["LocCurrencyId"].ToString();
                        }
                        catch
                        {
                        }


                    }
                    else
                    {
                        ddlgvCurrency.Items.Insert(0, "--Select--");

                    }

                }
            }
        }
        dsPCurrency.Dispose();
        dsUnit.Dispose();
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        string ParameterValue = string.Empty;
        if (txtProductcode.Text != "")
        {
            DataTable dtProduct = new DataTable();
            try
            {
                dtProduct = objProductM.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductcode.Text.ToString());
                if (new DataView(((DataTable)Session["DtPreDesignItemList"]), "Product_Id='" + dtProduct.Rows[0]["ProductId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0)
                {

                    DisplayMessage("Product is already exists!");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;

                }
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                string strUnitId = dtProduct.Rows[0]["UnitId"].ToString();
                if (strUnitId != "0" && strUnitId != "")
                {
                    ddlUnit.SelectedValue = strUnitId;
                }
                else
                {
                    FillUnit();
                }
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtPDesc.Visible = false;
                pnlPDescription.Visible = true;
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
                FillRelatedProduct(hdnNewProductId.Value);
            }
            else
            {
                GvRelatedProduct.DataSource = null;
                GvRelatedProduct.DataBind();
                FillUnit();
                hdnNewProductId.Value = "0";
                txtPDesc.Visible = true;
                pnlPDescription.Visible = false;
            }
            dtProduct.Dispose();
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
            GvRelatedProduct.DataSource = null;
            GvRelatedProduct.DataBind();
        }

    }
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        string SuggestedProductName = string.Empty;


        if (txtProductName.Text != "")
        {
            hdnNewProductId.Value = objProductM.GetProductIdbyProductName(txtProductName.Text,Session["BrandId"].ToString());

            if (hdnNewProductId.Value.Trim() == "@NOTFOUND@")
            {
                hdnNewProductId.Value = "0";
                SuggestedProductName = txtProductName.Text;
                if (txtPDesc.Text.Trim() == "")
                {
                    DisplayMessage("Enter Product Description");
                    txtPDesc.Focus();
                    return;
                }
            }
        }

        if (ddlUnit.SelectedValue != "--Select--")
        {
            hdnUnitId.Value = ddlUnit.SelectedValue;
        }
        else
        {
            DisplayMessage("Select Unit Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlUnit);
            return;
        }


        if (txtRequiredQty.Text == "")
        {

            DisplayMessage("Enter Required Quantity");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRequiredQty);
            return;
        }

        if (ddlPCurrency.SelectedValue != "--Select--")
        {
            hdnCurrencyId.Value = ddlPCurrency.SelectedValue;
        }
        else
        {
            DisplayMessage("Currency Required On Company Level");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlPCurrency);
            return;
        }

        if (txtEstimatedUnitPrice.Text == "")
        {

            txtEstimatedUnitPrice.Text = "0";

        }



        foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
        {
            CheckBox chk = (CheckBox)gvRow.FindControl("chk");
            DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
            DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
            TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
            Label txtgvProductdesc = (Label)gvRow.FindControl("Description");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");
            if (chk.Checked == true)
            {


                foreach (GridViewRow gvDetailRow in GvDetail.Rows)
                {
                    HiddenField hdnCheckProductId = (HiddenField)gvDetailRow.FindControl("hdnProductId");
                    if (lblgvProductId.Text == hdnCheckProductId.Value)
                    {
                        DisplayMessage("Related Product(" + lblgvProductName.Text + ") is Already Exists");
                        chk.Checked = false;
                        return;

                    }


                }


                if (ddlgvUnit.SelectedIndex == 0)
                {
                    DisplayMessage("Select Unit In Related Product List for product name=" + lblgvProductName.Text);
                    ddlgvUnit.Focus();
                    return;
                }
                if (txtgvquantity.Text.Trim() == "")
                {
                    DisplayMessage("Enter Required Quantity In Related Product List for product name=" + lblgvProductName.Text);
                    ddlgvUnit.Focus();
                    return;

                }


                if (txtGvEstimatedUnitPrice.Text != "")
                {
                    float flTemp = 0;
                    if (float.TryParse(txtGvEstimatedUnitPrice.Text, out flTemp))
                    {
                    }
                    else
                    {
                        txtGvEstimatedUnitPrice.Text = "";
                        ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Estimated Unit Price Should be numeric In Related Product List for product name=" + lblgvProductName.Text + "');", true);
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtGvEstimatedUnitPrice);
                        return;
                    }
                }
                else
                {
                    txtGvEstimatedUnitPrice.Text = "0";
                }
            }
        }
        DataTable dtDetail = CreateProductDataTable();
        int i = 0;


        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
            HiddenField hdnCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");
            HiddenField hdngvUnitId = (HiddenField)gvr.FindControl("hdnUnitId");

            //

            AjaxControlToolkit.HTMLEditor.Editor lblgvProductDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
            AjaxControlToolkit.HTMLEditor.Editor lblSuggestedProductName = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

            Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
            TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
            TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
            TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
            TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
            TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
            Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");

            Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
            Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");

            if (Session["DtPreDesignItemList"] != null)
            {
                dtDetail.Rows.Add();

                i = dtDetail.Rows.Count - 1;
                dtDetail.Rows[i]["TaxPercent"] = txtgvTaxP.Text;
                dtDetail.Rows[i]["TaxValue"] = txtgvTaxV.Text;
                dtDetail.Rows[i]["PriceAfterTax"] = txtgvPriceAfterTax.Text.Trim();
                dtDetail.Rows[i]["DiscountPercent"] = txtgvDiscountP.Text.Trim();
                dtDetail.Rows[i]["DiscountValue"] = txtgvDiscountV.Text.Trim();
                dtDetail.Rows[i]["PriceAfterDiscount"] = txtgvPriceAfterDiscount.Text;
                dtDetail.Rows[i]["Serial_No"] = lblSerialNo.Text;
                dtDetail.Rows[i]["Quantity"] = lblgvQuantity.Text;
                dtDetail.Rows[i]["Product_Id"] = hdngvProductId.Value;
                dtDetail.Rows[i]["SuggestedProductName"] = lblSuggestedProductName.Content;
                dtDetail.Rows[i]["UnitId"] = hdngvUnitId.Value;
                dtDetail.Rows[i]["ProductDescription"] = lblgvProductDescription.Content;
                dtDetail.Rows[i]["Currency_Id"] = "0";
                dtDetail.Rows[i]["EstimatedUnitPrice"] = lblgvEstimatedUnitPrice.Text;
                dtDetail.Rows[i]["SalesPrice"] = txtgvUnitPrice.Text.Trim();
                dtDetail.Rows[i]["PurchaseProductDescription"] = lblProductPurchaseDiscription.Text.Trim();
                try
                {
                    dtDetail.Rows[i]["PurchaseProductPrice"] = lblProductPurchasePrice.Text.Trim();
                }
                catch
                {
                }
            }
        }
        dtDetail.Rows.Add();
        i = dtDetail.Rows.Count - 1;
        dtDetail.Rows[i]["TaxPercent"] = "0";
        dtDetail.Rows[i]["TaxValue"] = "0";
        dtDetail.Rows[i]["PriceAfterTax"] = "0";
        dtDetail.Rows[i]["DiscountPercent"] = "0";
        dtDetail.Rows[i]["DiscountValue"] = "0";
        dtDetail.Rows[i]["PriceAfterDiscount"] = "0";
        try
        {
            dtDetail.Rows[i]["Serial_No"] = float.Parse(new DataView(dtDetail, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;
        }
        catch
        {
            dtDetail.Rows[i]["Serial_No"] = 1;

        }
        dtDetail.Rows[i]["Quantity"] = txtRequiredQty.Text;

        if (hdnNewProductId.Value == "0")
        {
            dtDetail.Rows[i]["ProductDescription"] = txtPDesc.Text;
            dtDetail.Rows[i]["SuggestedProductName"] = txtProductName.Text;
        }
        else
        {
            dtDetail.Rows[i]["ProductDescription"] = txtPDescription.Text;
            dtDetail.Rows[i]["SuggestedProductName"] = txtProductName.Text;

        }
        dtDetail.Rows[i]["Product_Id"] = hdnNewProductId.Value;
        dtDetail.Rows[i]["UnitId"] = hdnUnitId.Value;
        dtDetail.Rows[i]["Currency_Id"] = "0";
        dtDetail.Rows[i]["EstimatedUnitPrice"] = txtEstimatedUnitPrice.Text;
        dtDetail.Rows[i]["PurchaseProductDescription"] = "";
        try
        {
            dtDetail.Rows[i]["PurchaseProductPrice"] = "";
        }
        catch
        {
        }

        //this code is created by jitendra upadhyay on 15-12-2014
        //this code for get the sales price according the inventory parameter

        //code start


        try
        {




            dtDetail.Rows[i]["SalesPrice"] = (Convert.ToDouble(objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["brandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", ViewState["Customer_Id"].ToString(), hdnNewProductId.Value).Rows[0]["Sales_Price"].ToString()) * Convert.ToDouble(ViewState["ExchangeRate"].ToString())).ToString();

            dtDetail.Rows[i]["SalesPrice"] = ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), dtDetail.Rows[i]["SalesPrice"].ToString());

        }
        catch
        {
            dtDetail.Rows[i]["SalesPrice"] = txtEstimatedUnitPrice.Text;
        }





        foreach (GridViewRow gvRow in GvRelatedProduct.Rows)
        {
            CheckBox chk = (CheckBox)gvRow.FindControl("chk");
            DropDownList ddlgvUnit = (DropDownList)gvRow.FindControl("ddlunit");
            DropDownList ddlgvCurrency = (DropDownList)gvRow.FindControl("ddlCurrency");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdnUnitId");
            TextBox txtgvquantity = (TextBox)gvRow.FindControl("txtquantity");
            Label txtgvProductdesc = (Label)gvRow.FindControl("lblgvProductDescription");
            Label lblgvProductId = (Label)gvRow.FindControl("lblgvProductId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            TextBox txtGvEstimatedUnitPrice = (TextBox)gvRow.FindControl("txtEstimatedUnitPrice");


            if (chk.Checked == true)
            {

                dtDetail.Rows.Add();
                i = dtDetail.Rows.Count - 1;
                dtDetail.Rows[i]["TaxPercent"] = "0";
                dtDetail.Rows[i]["TaxValue"] = "0";
                dtDetail.Rows[i]["PriceAfterTax"] = "0";
                dtDetail.Rows[i]["DiscountPercent"] = "0";
                dtDetail.Rows[i]["DiscountValue"] = "0";
                dtDetail.Rows[i]["PriceAfterDiscount"] = "0";
                try
                {
                    dtDetail.Rows[i]["Serial_No"] = float.Parse(new DataView(dtDetail, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;
                }
                catch
                {
                    dtDetail.Rows[i]["Serial_No"] = 1;

                }
                dtDetail.Rows[i]["Quantity"] = txtgvquantity.Text;


                dtDetail.Rows[i]["ProductDescription"] = txtgvProductdesc.Text;
                dtDetail.Rows[i]["SuggestedProductName"] = lblgvProductName.Text;


                dtDetail.Rows[i]["Product_Id"] = lblgvProductId.Text;
                dtDetail.Rows[i]["UnitId"] = ddlgvUnit.SelectedValue;
                dtDetail.Rows[i]["Currency_Id"] = "0";
                dtDetail.Rows[i]["EstimatedUnitPrice"] = txtGvEstimatedUnitPrice.Text;
                dtDetail.Rows[i]["PurchaseProductDescription"] = "";
                try
                {
                    dtDetail.Rows[i]["PurchaseProductPrice"] = "";
                }
                catch
                {
                }


                //this code is created by jitendra upadhyay on 15-12-2014
                //this code for get the sales price according the inventory parameter

                //code start
                try
                {
                    dtDetail.Rows[i]["SalesPrice"] = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), objProductM.GetSalesPrice_According_InventoryParameter(Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "C", ViewState["Customer_Id"].ToString(), lblgvProductId.Text).Rows[0]["Sales_Price"].ToString());
                }
                catch
                {
                    dtDetail.Rows[i]["SalesPrice"] = "0";
                }
                //code end




            }


        }


        Session["DtPreDesignItemList"] = dtDetail;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

        foreach (GridViewRow row in GvDetail.Rows)
        {
            try
            {
                DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                {
                    ((LinkButton)row.FindControl("lnkDeatil")).Visible = false;
                    ((Panel)row.FindControl("PopupMenu")).Visible = false;
                }
            }
            catch
            {
            }
            //for calculation
            HiddenField hdnProductId = (HiddenField)row.FindControl("hdnProductId");
            TextBox lblgvQuantity = (TextBox)row.FindControl("lblgvQuantity");
            TextBox txtgvUnitPrice = (TextBox)row.FindControl("txtgvUnitPrice");
            TextBox txtgvQuantityPrice = (TextBox)row.FindControl("txtgvQuantityPrice");
            TextBox txtgvDiscountP = (TextBox)row.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)row.FindControl("txtgvDiscountV");
            TextBox txtgvPriceAfterDiscount = (TextBox)row.FindControl("txtgvPriceAfterDiscount");
            TextBox txtgvTaxP = (TextBox)row.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)row.FindControl("txtgvTaxV");
            TextBox txtgvPriceAfterTax = (TextBox)row.FindControl("txtgvPriceAfterTax");
            TextBox txtgvTotal = (TextBox)row.FindControl("txtgvTotal");
            Label lblgvEstimatedUnitPrice = (Label)row.FindControl("lblgvEstimatedUnitPrice");

            Label lblgvSerialNo = (Label)row.FindControl("lblgvSerialNo");

            if (txtgvUnitPrice.Text == "")
            {
                txtgvUnitPrice.Text = "0";
            }
            if (lblgvQuantity.Text == "")
            {
                lblgvQuantity.Text = "0";
            }

            double TotalTax = 0;
            if (txtgvUnitPrice.Text != "0" && lblgvQuantity.Text != "0")
            {
                TotalTax = Get_Tax_Amount(txtgvUnitPrice.Text, hdnProductId.Value, lblgvSerialNo.Text);
                double.TryParse(GetCurrency(Session["LocCurrencyId"].ToString(), TotalTax.ToString()).Split('/')[0].ToString(), out TotalTax);
            }

            string[] strtotal = Common.TaxDiscountCaluculation(txtgvUnitPrice.Text, lblgvQuantity.Text, TotalTax.ToString(), "", txtgvDiscountP.Text, "", true, Session["LocCurrencyId"].ToString(), false, Session["DBConnection"].ToString());


            txtgvQuantityPrice.Text = strtotal[0].ToString();
            txtgvDiscountP.Text = strtotal[1].ToString();
            txtgvDiscountV.Text = strtotal[2].ToString();
            txtgvTaxP.Text = strtotal[3].ToString();
            txtgvTaxV.Text = strtotal[4].ToString();
            txtgvPriceAfterTax.Text = strtotal[5].ToString();
            txtgvTotal.Text = strtotal[5].ToString();
            txtgvUnitPrice.Text = GetAmountDecimal(txtgvUnitPrice.Text);
            lblgvEstimatedUnitPrice.Text = GetAmountDecimal(lblgvEstimatedUnitPrice.Text);
        }
        SetDecimal();
        HeadearCalculateGrid();
        ResetProduct();
        ddlTransType.Enabled = false;
        dtDetail.Dispose();
    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Serial_No", typeof(float));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("TaxPercent");
        dt.Columns.Add("TaxValue");
        dt.Columns.Add("PriceAfterTax");
        dt.Columns.Add("DiscountPercent");
        dt.Columns.Add("DiscountValue");
        dt.Columns.Add("PriceAfterDiscount");
        dt.Columns.Add("PurchaseProductDescription");
        dt.Columns.Add("PurchaseProductPrice");
        dt.Columns.Add("SalesPrice");
        return dt;
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetProduct();

    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {

        pnlProduct1.Visible = false;
        ResetProduct();

    }
    public void ResetProduct()
    {
        txtProductName.Text = "";
        FillUnit();
        txtPDescription.Text = "";
        txtRequiredQty.Text = "1";
        txtEstimatedUnitPrice.Text = "";
        hdnNewProductId.Value = "0";
        txtPDesc.Text = "";
        txtProductcode.Text = "";
        txtProductcode.Focus();
        GvRelatedProduct.DataSource = null;
        GvRelatedProduct.DataBind();

    }
    protected void IbtnDetailDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtDetail = new DataView((DataTable)Session["DtPreDesignItemList"], "Serial_No<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        Session["DtPreDesignItemList"] = dtDetail;

        if (dtDetail.Rows.Count != 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvDetail, dtDetail, "", "");

            foreach (GridViewRow Row in GvDetail.Rows)
            {

                TextBox lblgvQuantity = (TextBox)Row.FindControl("lblgvQuantity");

                TextBox txtgvQuantityPrice = (TextBox)Row.FindControl("txtgvQuantityPrice");

                TextBox txtgvUnitPrice = (TextBox)Row.FindControl("txtgvUnitPrice");

                if (lblgvQuantity.Text == "")
                {
                    lblgvQuantity.Text = "0";
                }
                if (txtgvUnitPrice.Text == "")
                {
                    txtgvUnitPrice.Text = "0";
                }
                txtgvQuantityPrice.Text = (Convert.ToDouble(lblgvQuantity.Text) * Convert.ToDouble(txtgvUnitPrice.Text)).ToString();

                try
                {
                    DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                    {
                        ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                        ((Panel)Row.FindControl("PopupMenu")).Visible = false;
                    }
                }
                catch
                {
                }

            }
            HeadearCalculateGrid();
            SetDecimal();
        }
        else
        {
            GvDetail.DataSource = null;
            GvDetail.DataBind();
            txtAmount.Text = "0";
            txtTaxP.Text = "0";
            txtTaxV.Text = "0";
            txtPriceAfterTax.Text = "0";
            txtDiscountP.Text = "0";
            txtDiscountV.Text = "0";
            txtTotalAmount.Text = "0";

        }
        dtDetail.Dispose();

    }
    #endregion
    #region Advancesearch
    public DataTable CreateProductDataTableForSalesQuotation()
    {
        DataTable dt = new DataTable();


        dt.Columns.Add("TaxPercent");
        dt.Columns.Add("TaxValue");
        dt.Columns.Add("PriceAfterTax");
        dt.Columns.Add("DiscountPercent");
        dt.Columns.Add("DiscountValue");

        dt.Columns.Add("PriceAfterDiscount");

        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Quantity");
        dt.Columns.Add("ProductDescription");
        dt.Columns.Add("SuggestedProductName");
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("UnitId");

        dt.Columns.Add("Currency_Id");
        dt.Columns.Add("EstimatedUnitPrice");
        dt.Columns.Add("PurchaseProductDescription");
        dt.Columns.Add("PurchaseProductPrice");
        dt.Columns.Add("SalesPrice");
        dt.Columns.Add("Sysqty");
        dt.Columns.Add("AgentCommission");
        return dt;
    }

    protected void btnAddProductScreen_Click(object sender, EventArgs e)
    {
        DataTable dtDetail = CreateProductDataTableForSalesQuotation();
        int i = 0;

        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            HiddenField hdngvProductId = (HiddenField)gvr.FindControl("hdnProductId");
            HiddenField hdnCurrencyId = (HiddenField)gvr.FindControl("hdnCurrencyId");

            //

            AjaxControlToolkit.HTMLEditor.Editor lblgvProductDescription = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("hdnSuggestedProductdesc");
            AjaxControlToolkit.HTMLEditor.Editor lblSuggestedProductName = (AjaxControlToolkit.HTMLEditor.Editor)gvr.FindControl("lblgvProductDescription");

            Label lblSerialNo = (Label)gvr.FindControl("lblSerialNo");
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("lblgvQuantity");
            HiddenField hdnUnitIdvalue = (HiddenField)gvr.FindControl("hdnUnitId");
            TextBox txtgvUnitPrice = (TextBox)gvr.FindControl("txtgvUnitPrice");
            TextBox txtgvQuantityPrice = (TextBox)gvr.FindControl("txtgvQuantityPrice");
            TextBox txtgvTaxP = (TextBox)gvr.FindControl("txtgvTaxP");
            TextBox txtgvTaxV = (TextBox)gvr.FindControl("txtgvTaxV");
            TextBox txtgvPriceAfterTax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
            TextBox txtgvDiscountP = (TextBox)gvr.FindControl("txtgvDiscountP");
            TextBox txtgvDiscountV = (TextBox)gvr.FindControl("txtgvDiscountV");
            TextBox txtgvPriceAfterDiscount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
            Label lblgvEstimatedUnitPrice = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");

            Literal lblProductPurchaseDiscription = (Literal)gvr.FindControl("txtdesc");
            Label lblProductPurchasePrice = (Label)gvr.FindControl("Label8");


            dtDetail.Rows.Add();

            i = dtDetail.Rows.Count - 1;
            dtDetail.Rows[i]["TaxPercent"] = txtgvTaxP.Text;
            dtDetail.Rows[i]["TaxValue"] = txtgvTaxV.Text;
            dtDetail.Rows[i]["PriceAfterTax"] = txtgvPriceAfterTax.Text.Trim();
            dtDetail.Rows[i]["DiscountPercent"] = txtgvDiscountP.Text.Trim();
            dtDetail.Rows[i]["DiscountValue"] = txtgvDiscountV.Text.Trim();
            dtDetail.Rows[i]["PriceAfterDiscount"] = txtgvPriceAfterDiscount.Text;
            dtDetail.Rows[i]["Serial_No"] = lblSerialNo.Text;
            dtDetail.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dtDetail.Rows[i]["Product_Id"] = hdngvProductId.Value;
            dtDetail.Rows[i]["SuggestedProductName"] = lblSuggestedProductName.Content;
            dtDetail.Rows[i]["UnitId"] = hdnUnitIdvalue.Value;
            dtDetail.Rows[i]["ProductDescription"] = lblgvProductDescription.Content;
            dtDetail.Rows[i]["Currency_Id"] = hdnCurrencyId.Value.Trim();
            dtDetail.Rows[i]["EstimatedUnitPrice"] = lblgvEstimatedUnitPrice.Text;
            dtDetail.Rows[i]["SalesPrice"] = txtgvUnitPrice.Text.Trim();
            dtDetail.Rows[i]["PurchaseProductDescription"] = lblProductPurchaseDiscription.Text.Trim();
            dtDetail.Rows[i]["PurchaseProductPrice"] = lblProductPurchasePrice.Text.Trim();
            dtDetail.Rows[i]["Sysqty"] = "0";
            dtDetail.Rows[i]["AgentCommission"] = "0";

        }

        Session["DtPreDesignItemList"] = dtDetail;

        Session["DtSearchProduct"] = Session["DtPreDesignItemList"];

        string strCmd = string.Format("window.open('../Inventory/AddItem.aspx?Page=SQ&&CustomerId=0','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    protected void btnAddtoList_Click(object sender, EventArgs e)
    {
        DataTable dtDetail = new DataTable();
        if (Session["DtSearchProduct"] != null)
        {
            Session["DtPreDesignItemList"] = Session["DtSearchProduct"];
            if (Session["DtPreDesignItemList"] != null)
            {
                dtDetail = (DataTable)Session["DtPreDesignItemList"];
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvDetail, (DataTable)Session["DtPreDesignItemList"], "", "");


            }
            foreach (GridViewRow Row in GvDetail.Rows)
            {
                try
                {
                    DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                    {
                        ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                        ((Panel)Row.FindControl("PopupMenu")).Visible = false;
                    }
                }
                catch
                {
                }
                txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

            }
            Session["DtSearchProduct"] = null;
        }
        else
        {

            if (Session["DtPreDesignItemList"] != null)
            {
                dtDetail = (DataTable)Session["DtPreDesignItemList"];
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)GvDetail, (DataTable)Session["DtPreDesignItemList"], "", "");


            }


            foreach (GridViewRow Row in GvDetail.Rows)
            {
                try
                {
                    DataTable dt = new DataView(dtDetail, "Product_Id='" + ((HiddenField)Row.FindControl("hdnProductId")).Value.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows[0]["PurchaseProductPrice"].ToString() == "" && dt.Rows[0]["PurchaseProductDescription"].ToString() == "")
                    {
                        ((LinkButton)Row.FindControl("lnkDeatil")).Visible = false;
                        ((Panel)Row.FindControl("PopupMenu")).Visible = false;
                    }
                }
                catch
                {
                }
                txtgvUnitPrice_TextChanged(((object)((TextBox)Row.FindControl("txtgvUnitPrice"))), e);

            }
            DisplayMessage("Product Not Found");
            return;

        }


    }
    protected void rbtnFormView_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFormView.Checked == true)
        {
            btnAddNewProduct.Visible = true;
            btnAddProductScreen.Visible = false;
            btnAddtoList.Visible = false;
            btnAddNewProduct_Click(null, null);
        }
        if (rbtnAdvancesearchView.Checked == true)
        {
            btnAddNewProduct.Visible = false;
            btnAddProductScreen.Visible = true;
            btnAddtoList.Visible = true;
            btnClosePanel_Click(null, null);
        }

    }
    #endregion
    #region UPloadTemplate


    protected void ImgLogoAdd_Click(object sender, ImageClickEventArgs e)
    {

        if (FileUploadImage.HasFile == false)
        {
            DisplayMessage("Upload The File");
            FileUploadImage.Focus();
            return;
        }

        string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/PreDesignContent/" + Session["UserId"].ToString());
        if (!Directory.Exists(path))
        {
            CheckDirectory(path);
        }
        string filepath = "../CompanyResource/" + Session["CompId"].ToString() + "/PreDesignContent/" + Session["UserId"].ToString() + "/" + Guid.NewGuid() + FileUploadImage.FileName;
        FileUploadImage.SaveAs(Server.MapPath(filepath));
        Editor1.Content = Editor1.Content + "<img src='" + filepath + "' />";
    }

    public void CheckDirectory(string path)
    {
        if (path != "")
        {
            Directory.CreateDirectory(path);
        }
    }

    #endregion
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FileUploadImage.HasFile)
        {
            string ext = FileUploadImage.FileName.Substring(FileUploadImage.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/PreDesignContent/" + Session["UserId"].ToString());
                if (!Directory.Exists(path))
                {
                    CheckDirectory(path);
                }
                string filepath = "../CompanyResource/" + Session["CompId"].ToString() + "/PreDesignContent/" + Session["UserId"].ToString() + "/" + Guid.NewGuid() + FileUploadImage.FileName;
                FileUploadImage.SaveAs(Server.MapPath(filepath));
            }
        }
    }
    protected void FUHtml_FileUploadComplete(object sender, EventArgs e)
    {
        if (UploadTemplate.HasFile)
        {
            string ext = UploadTemplate.FileName.Substring(UploadTemplate.FileName.Split('.')[0].Length);
            if ((ext != ".html"))
            {
                DisplayMessage("Invalid File Type, Select Only .html extension file");
                return;
            }
            else
            {
                string FilePath = string.Empty;
                string UploadFileName = UploadTemplate.PostedFile.FileName;
                string path = string.Empty;
                path = Server.MapPath("~/ArcaWing/Template");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                FilePath = "~/ArcaWing/Template/" + UploadFileName;
                UploadTemplate.SaveAs(Server.MapPath(FilePath));
            }
        }
    }
    protected void btnTemplate_Click(object sender, ImageClickEventArgs e)
    {
        if (UploadTemplate.HasFile == false)
        {
            DisplayMessage("Upload File");
            UploadTemplate.Focus();
            return;
        }


        if (UploadTemplate.PostedFile.ContentType.Trim() != "text/html")
        {
            DisplayMessage("File extension Should be in Html format");
            UploadTemplate.Focus();
            return;
        }

        string FilePath = string.Empty;
        string UploadFileName = UploadTemplate.PostedFile.FileName;
        //Session["FileName"] = UploadFileName.ToString();
        string path = string.Empty;
        path = Server.MapPath("~/ArcaWing/Template");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FilePath = "~/ArcaWing/Template/" + UploadFileName;
        UploadTemplate.SaveAs(Server.MapPath(FilePath));
        //Stream fs = UploadTemplate.PostedFile.InputStream;
        //BinaryReader br = new BinaryReader(fs);
        //Byte[] bytes = br.ReadBytes((Int32)fs.Length);
        //Session["FileData"] = bytes;
        string Html = File.ReadAllText(Server.MapPath(FilePath));
        Editor1.Content = Html.ToString();
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
    public string GetCurrency(string strToCurrency, string strLocalAmount)
    {
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        strExchangeRate = SystemParameter.GetExchageRate(Session["LocCurrencyId"].ToString(), strToCurrency, Session["DBConnection"].ToString());
        try
        {
            strForienAmount = ObjSysParam.GetCurencyConversionForInv(strToCurrency, (Convert.ToDouble(strExchangeRate) * Convert.ToDouble(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    
    protected void BtnAddTax_Command(object sender, CommandEventArgs e)
    {
        if (Session["Temp_Product_Tax_PSQ"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PSQ"] as DataTable;
            if (Dt_Cal.Rows.Count > 0)
            {
                string F_Serial_No = string.Empty;
                if (e.CommandName.ToString() == "GvDetail")
                {
                    GridViewRow Gv_Rows = (GridViewRow)(sender as Control).Parent.Parent;
                    Label Serial_No = (Label)Gv_Rows.FindControl("lblgvSerialNo");
                    F_Serial_No = Serial_No.Text;
                }
                Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "' and Serial_No='" + F_Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();

                //Dt_Cal = new DataView(Dt_Cal, "Product_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (Dt_Cal.Rows.Count > 0)
                {
                    gvTaxCalculation.DataSource = Dt_Cal;
                    gvTaxCalculation.DataBind();
                    int Row_Index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
                    string Grid_Name = e.CommandName.ToString();
                    if (Grid_Name == "GvDetail")
                    {
                        TextBox Unit_Price = (TextBox)GvDetail.Rows[Row_Index].FindControl("txtgvUnitPrice");
                        TextBox Discount_Price = (TextBox)GvDetail.Rows[Row_Index].FindControl("txtgvDiscountV");
                        Hdn_unit_Price_Tax.Value = Unit_Price.Text;
                        Hdn_Discount_Tax.Value = Discount_Price.Text;
                    }
                    Hdn_Serial_No_Tax.Value = F_Serial_No;
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

            bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (IsTax == true)
            {
                if (ddlTransType.SelectedIndex > 0)
                {
                    if (editid.Value != "")
                    {
                        using (DataTable DT_Db_Details = ObjSQuoteDetail.GetQuotationDetailBySQuotation_Id(StrCompId, StrBrandId, StrLocationId, editid.Value, Session["FinanceYearId"].ToString()))
                        {
                            if (DT_Db_Details.Rows.Count > 0)
                            {
                                TaxQuery = @"Select TRD.ProductId as Product_Id,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per as Tax_Value,TRD.Tax_value as TaxAmount,TRD.Field1 as Amount,IPID.Serial_No as Serial_No from Inv_TaxRefDetail TRD inner join Sys_TaxMaster STM on TRD.Tax_Id=STM.Trans_Id inner join Inv_SalesQuotationDetail IPID on IPID.Trans_Id=TRD.Field2 where TRD.Ref_Id='" + editid.Value + "' and TRD.Ref_Type='PSQ' Group by TRD.ProductId,TRD.Tax_Id,STM.Tax_Name,TRD.Tax_Per,TRD.Tax_value,TRD.Field1,IPID.Serial_No";
                                DataTable Dt_Inv_TaxRefDetail = objDa.return_DataTable(TaxQuery);
                                Session["Temp_Product_Tax_PSQ"] = null;
                                DataTable Dt_Temp = new DataTable();
                                Dt_Temp = TemporaryProductWiseTaxes();
                                Dt_Temp = Dt_Inv_TaxRefDetail;
                                if (Dt_Inv_TaxRefDetail.Rows.Count > 0)
                                {
                                    Session["Temp_Product_Tax_PSQ"] = Dt_Temp;
                                }
                                Dt_Inv_TaxRefDetail.Dispose();
                                Dt_Temp.Dispose();
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
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_PSQ"] != null)
            {
                DataTable Dt_Session_Tax = Session["Temp_Product_Tax_PSQ"] as DataTable;
                if (Dt_Session_Tax.Rows.Count > 0)
                {
                    Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "' and Serial_No='" + Serial_No + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //Dt_Session_Tax = new DataView(Dt_Session_Tax, "Product_Id='" + ProductId + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow DRT in Dt_Session_Tax.Rows)
                    {
                        TotalTax_Percentage = TotalTax_Percentage + Convert.ToDouble(DRT["Tax_Value"].ToString());
                    }
                }
                Dt_Session_Tax.Dispose();
            }
        }
        return TotalTax_Percentage;
    }
    public double Get_Tax_Amount(string Amount, string ProductId, string Serial_No)
    {
        double TotalTax_Amount = 0;
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (IsTax == true)
        {
            if (Session["Temp_Product_Tax_PSQ"] != null)
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
        bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        Amount = GetCurrency(Session["LocCurrencyId"].ToString(), Amount.ToString()).Split('/')[0].ToString();
        if (IsTax)
        {
            Hdn_Tax_By.Value = SystemParameter.Get_Tax_Parameter(Session["DBConnection"].ToString());
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
                if (Session["Temp_Product_Tax_PSQ"] == null)
                    dt = TemporaryProductWiseTaxes();
                else
                    dt = (DataTable)Session["Temp_Product_Tax_PSQ"];
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
                    Session["Temp_Product_Tax_PSQ"] = dt;
                }
            }
        }
    }
    protected void Btn_Update_Tax_Click(object sender, EventArgs e)
    {
        string Serial_No = Hdn_Serial_No_Tax.Value;
        string Product_ID = Hdn_Product_Id_Tax.Value;
        string Unit_Price = Hdn_unit_Price_Tax.Value;
        string Unit_Discount = Hdn_Discount_Tax.Value;
        string Net_Unit_Price = (Convert.ToDouble(Unit_Price) - Convert.ToDouble(Unit_Discount)).ToString();

        if (Session["Temp_Product_Tax_PSQ"] != null)
        {
            DataTable Dt_Cal = Session["Temp_Product_Tax_PSQ"] as DataTable;
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

                        if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value && DR_Tax["Serial_No"].ToString() == Serial_No)
                        //if (DR_Tax["Product_Id"].ToString() == ProductId.Value && DR_Tax["Tax_Id"].ToString() == TaxId.Value)
                        {
                            DR_Tax["Tax_Value"] = Tax_Percentage.Text;
                            DR_Tax["TaxAmount"] = (Convert.ToDouble(Net_Unit_Price) * Convert.ToDouble(Tax_Percentage.Text)) / 100;
                            DR_Tax["Amount"] = Net_Unit_Price;
                        }
                    }
                }
            }
            Session["Temp_Product_Tax_PSQ"] = Dt_Cal;
            foreach (GridViewRow dl in GvDetail.Rows)
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
                            TextBox Tax_Percent = (TextBox)dl.FindControl("txtgvTaxP");
                            HiddenField hdnProductId = (HiddenField)dl.FindControl("hdnProductId");
                            Label lblgvSerialNo = (Label)dl.FindControl("lblgvSerialNo");
                            if (Product_ID == hdnProductId.Value && Serial_No == lblgvSerialNo.Text)
                                if (Product_ID == hdnProductId.Value)
                                {
                                    Tax_Percent.Text = GetAmountDecimal(Tax_Val.ToString());
                                }
                        }
                    }
                }
            }
            HeadearCalculateGrid();
            Hdn_Product_Id_Tax.Value = "";
            Hdn_Serial_No_Tax.Value = "";
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
        }
    }
    public void Insert_Tax(string Grid_Name, string PQ_Header_ID, string Detail_ID, GridViewRow Gv_Row)
    {
        objTaxRefDetail.DeleteRecord_By_RefType_and_RefId_and_Detail_Id("PSQ", PQ_Header_ID.ToString(), Detail_ID.ToString());
        string R_Product_ID = string.Empty;
        string R_Order_Req_Qty = string.Empty;
        string R_Unit_Price = string.Empty;
        string R_Discount_Value = string.Empty;
        string Grid = string.Empty;
        string R_Serial_No = string.Empty;
        if (Grid_Name == "GvDetail")
        {
            Grid = "GvDetail";
            HiddenField Product_ID = (HiddenField)Gv_Row.FindControl("hdnProductId");
            TextBox Order_Req_Qty = (TextBox)Gv_Row.FindControl("lblgvQuantity");
            TextBox Unit_Price = (TextBox)Gv_Row.FindControl("txtgvUnitPrice");
            TextBox Discount_Value = (TextBox)Gv_Row.FindControl("txtgvDiscountV");
            Label lblSerialNO = (Label)Gv_Row.FindControl("lblgvSerialNo");
            R_Product_ID = Product_ID.Value;
            R_Order_Req_Qty = Order_Req_Qty.Text;
            R_Unit_Price = Unit_Price.Text;
            R_Discount_Value = Discount_Value.Text;
            R_Serial_No = lblSerialNO.Text;
        }
        if (Grid != "")
        {
            double A_Unit_Cost = Convert.ToDouble(R_Unit_Price) * Convert.ToDouble(R_Order_Req_Qty);
            double A_Unit_Discount = Convert.ToDouble(R_Discount_Value) * Convert.ToDouble(R_Order_Req_Qty);
            double Net_Amount = A_Unit_Cost - A_Unit_Discount;
            //Get_Tax_Insert(R_Product_ID);
            DataTable ProductTax = new DataTable();
            if (Session["Temp_Product_Tax_PSQ"] == null)
                ProductTax = TemporaryProductWiseTaxes();
            else
                ProductTax = (DataTable)Session["Temp_Product_Tax_PSQ"];
            string ProductId = string.Empty;
            string TaxId = string.Empty;
            string TaxValue = string.Empty;
            string TaxAmount = string.Empty;
            string Amount = string.Empty;
            if (ProductTax != null && ProductTax.Rows.Count > 0)
            {
                foreach (DataRow dr in ProductTax.Rows)
                {
                    //if (dr["Product_Id"].ToString() == R_Product_ID)
                    if (dr["Product_Id"].ToString() == R_Product_ID && dr["Serial_No"].ToString() == R_Serial_No)
                    {
                        ProductId = dr["Product_Id"].ToString();
                        TaxId = dr["Tax_Id"].ToString();
                        TaxValue = GetAmountDecimal(dr["Tax_Value"].ToString());
                        TaxAmount = dr["TaxAmount"].ToString();
                        Amount = Net_Amount.ToString();
                        //if (Convert.ToDouble(Amount) != 0)
                        objTaxRefDetail.InsertRecord("PSQ", PQ_Header_ID, "0", "0", ProductId, TaxId, TaxValue, TaxAmount, false.ToString(), Amount, Detail_ID.ToString(), ddlTransType.SelectedValue.ToString(), "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }
        }
    }
    public double Get_Discount_Percentage(string Unit_Price, string Discount_Amount)
    {
        try
        {
            double Discount_Percent = 0;
            if (Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == true)
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Discount_Amount = Convert.ToDouble(Discount_Amount);
                Discount_Percent = (D_Discount_Amount / D_Unit_Price) * 100;
            }
            return Discount_Percent;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Total_Tax_Percentage(string Unit_Price, string Tax_Amount)
    {
        try
        {
            double Tax_Percent = 0;
            if (Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == true)
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Tax_Amount = Convert.ToDouble(Tax_Amount);
                Tax_Percent = (D_Tax_Amount / D_Unit_Price) * 100;
            }
            return Tax_Percent;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Discount_Amount(string Unit_Price, string Discount_Percent)
    {
        try
        {
            double Discount_Amount = 0;
            if (Inventory_Common.IsSalesDiscountEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()) == true)
            {
                double D_Unit_Price = Convert.ToDouble(Unit_Price);
                double D_Discount_Percent = Convert.ToDouble(Discount_Percent);
                Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            }
            return Discount_Amount;
        }
        catch
        {
            return 0;
        }
    }
    public double Get_Net_Amount(string Unit_Price, string Discount_Percent, string Product_Id, string Serial_No)
    {
        try
        {
            double Net_Amount = 0;
            double D_Unit_Price = Convert.ToDouble(Unit_Price);
            double D_Discount_Percent = Convert.ToDouble(Discount_Percent);
            double Discount_Amount = (D_Unit_Price * D_Discount_Percent) / 100;
            double Tax_Amount = Get_Tax_Amount((D_Unit_Price - Discount_Amount).ToString(), Product_Id, Serial_No);
            Net_Amount = (D_Unit_Price - Discount_Amount) + Tax_Amount;
            return Net_Amount;
        }
        catch
        {
            return 0;
        }
    }
    public void HeadearCalculateGrid()
    {
        double F_Gross_Total = 0;
        double F_Discount_Per = 0;
        double F_Discount_Value = 0;
        double F_Tax_Per = 0;
        double F_Tax_Value = 0;
        double F_Net_Total = 0;


        double Gross_Unit_Price = 0;
        double Gross_Discount_Amount = 0;
        double Gross_Tax_Amount = 0;
        double Gross_Line_Total = 0;

        foreach (GridViewRow gvr in GvDetail.Rows)
        {
            Label Product_Code = (Label)gvr.FindControl("lblgvProductcode");
            HiddenField Product_Id = (HiddenField)gvr.FindControl("hdnProductId");
            HiddenField Unit_Id = (HiddenField)gvr.FindControl("hdnUnitId");
            Label Unit_Name = (Label)gvr.FindControl("lblgvUnit");
            HiddenField Currency_ID = (HiddenField)gvr.FindControl("hdnCurrencyId");
            Label Currency_Name = (Label)gvr.FindControl("lblgvCurrency");
            TextBox Quantity = (TextBox)gvr.FindControl("lblgvQuantity");
            Label Estimated_Price = (Label)gvr.FindControl("lblgvEstimatedUnitPrice");
            TextBox Unit_Price = (TextBox)gvr.FindControl("txtgvUnitPrice");
            TextBox Total_Quantity_Price = (TextBox)gvr.FindControl("txtgvQuantityPrice");
            TextBox Discount_Percentage = (TextBox)gvr.FindControl("txtgvDiscountP");
            TextBox Discount_Amount = (TextBox)gvr.FindControl("txtgvDiscountV");
            TextBox Price_After_Discount = (TextBox)gvr.FindControl("txtgvPriceAfterDiscount");
            TextBox Tax_Percentage = (TextBox)gvr.FindControl("txtgvTaxP");
            TextBox Tax_Amount = (TextBox)gvr.FindControl("txtgvTaxV");
            TextBox Amount_After_Tax = (TextBox)gvr.FindControl("txtgvPriceAfterTax");
            TextBox Line_Total = (TextBox)gvr.FindControl("txtgvTotal");
            TextBox Agent_Commission = (TextBox)gvr.FindControl("txtgvAgentCommission");
            Label lblgvSerialNo = (Label)gvr.FindControl("lblgvSerialNo");

            if (Unit_Price.Text == "")
                Unit_Price.Text = "0";

            if (Quantity.Text == "")
                Quantity.Text = "0";

            if (Discount_Percentage.Text == "")
                Discount_Percentage.Text = "0";

            double F_Unit_Price = double.Parse(Unit_Price.Text);
            double F_Order_Quantity = double.Parse(Quantity.Text);
            double F_Discount_Percentage = double.Parse(Discount_Percentage.Text);
            double F_Discount_Amount = Get_Discount_Amount(F_Unit_Price.ToString(), F_Discount_Percentage.ToString());
            //if (Hdn_Get_Inquity.Value != "")
            Add_Tax_In_Session((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
            double F_Tax_Percentage = Get_Tax_Percentage(Product_Id.Value, lblgvSerialNo.Text);
            double F_Tax_Amount = Get_Tax_Amount((F_Unit_Price - F_Discount_Amount).ToString(), Product_Id.Value, lblgvSerialNo.Text);
            double F_Total_Amount = (F_Unit_Price - F_Discount_Amount) + F_Tax_Amount;
            double F_Row_Total_Amount = F_Total_Amount * F_Order_Quantity;

            Discount_Percentage.Text = GetAmountDecimal(F_Discount_Percentage.ToString());
            Discount_Amount.Text = GetAmountDecimal(F_Discount_Amount.ToString());
            Tax_Percentage.Text = GetAmountDecimal(F_Tax_Percentage.ToString());
            Tax_Amount.Text = GetAmountDecimal(F_Tax_Amount.ToString());
            Line_Total.Text = GetAmountDecimal(F_Row_Total_Amount.ToString());
            Total_Quantity_Price.Text = (F_Unit_Price * F_Order_Quantity).ToString();

            Gross_Unit_Price = Gross_Unit_Price + (F_Unit_Price * F_Order_Quantity);
            Gross_Discount_Amount = Gross_Discount_Amount + (F_Discount_Amount * F_Order_Quantity);
            Gross_Tax_Amount = Gross_Tax_Amount + (F_Tax_Amount * F_Order_Quantity);
            Gross_Line_Total = Gross_Line_Total + F_Row_Total_Amount;
        }
        txtAmount.Text = GetAmountDecimal(Gross_Unit_Price.ToString());
        txtDiscountV.Text = GetAmountDecimal(Gross_Discount_Amount.ToString());
        txtTaxV.Text = GetAmountDecimal(Gross_Tax_Amount.ToString());
        txtPriceAfterTax.Text = GetAmountDecimal(Gross_Line_Total.ToString());
        txtTotalAmount.Text = GetAmountDecimal(Gross_Line_Total.ToString());
        txtDiscountP.Text = GetAmountDecimal(Get_Discount_Percentage(Gross_Unit_Price.ToString(), Gross_Discount_Amount.ToString()).ToString());
        txtTaxP.Text = GetAmountDecimal(Get_Total_Tax_Percentage((Gross_Unit_Price - Gross_Discount_Amount).ToString(), Gross_Tax_Amount.ToString()).ToString());
    }
}