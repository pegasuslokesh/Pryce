using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

public partial class VoucherEntries_TaxEntryAfterInvoice : System.Web.UI.Page
{
    Ems_ContactMaster ObjContactMaster = null;
    DataAccessClass da = null;
    CurrencyMaster objCurrency = null;
    Common cmn = null;
    LocationMaster ObjLocation = null;
    SystemParameter objsys = null;
    Set_BankMaster objBank = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ChartOfAccount objCOA = null;
    Ac_Finance_Year_Info objFYI = null;
    Set_Suppliers objSupplier = null;
    Ac_ParameterMaster objAccParameter = null;
    EmployeeMaster objEmployee = null;
    Set_CustomerMaster ObjCoustmer = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Prj_VehicleMaster objVehicleMaster = null;
    NotificationMaster Obj_Notifiacation = null;
    Inv_AfterInvoiceTaxDetail objAfterInvoiceTaxDetail = null;
    Inv_SalesInvoiceHeader objSalesInvoiceHeader = null;
    PurchaseInvoice objPurchaseInvoiceHeader = null;
    Inv_ParameterMaster objInvParameterMaster = null;
    TaxMaster objTaxMaster = null;
    PageControlCommon objPageCmn = null;
    PageControlsSetting objPageCtlSettting = null;
    public const int grdDefaultColCount = 6;
    private const string strPageName = "TaxEntryAfterInvoice";
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    string strLocationCurrencyId = string.Empty;
    static string SupplierID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        da = new DataAccessClass(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objBank = new Set_BankMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objAccParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objVehicleMaster = new Prj_VehicleMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objAfterInvoiceTaxDetail = new Inv_AfterInvoiceTaxDetail(Session["DBConnection"].ToString());
        objSalesInvoiceHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objPurchaseInvoiceHeader = new PurchaseInvoice(Session["DBConnection"].ToString());
        objInvParameterMaster = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objTaxMaster = new TaxMaster(Session["DBConnection"].ToString());
        objPageCtlSettting = new PageControlsSetting(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../VoucherEntries/TaxEntryAfterInvoice.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            

            try
            {
                strLocationCurrencyId = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

            }
            catch
            {

            }

            CalendarExtender_txtVoucherDate.Format = objsys.SetDateFormat();
            txtVoucherDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            //AllPageCode();
            //objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0");
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            btnList_Click(sender, e);
            FillGrid();
            Reset();
            FillCurrency();
            FillTax();
            getPageControlsVisibility();
        }

        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnControlsSetting.Visible = false;
        btnGvListSetting.Visible = clsPagePermission.bCanChangeControsAttribute;
    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlInvoiceCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlInvoiceCurrency.Items.Insert(0, "--Select--");
            ddlInvoiceCurrency.SelectedIndex = 0;
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    
   
    //#region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSupplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);

        string[] filterlist = new string[dtSupplier.Rows.Count];
        if (dtSupplier.Rows.Count > 0)
        {
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                filterlist[i] = dtSupplier.Rows[i]["Name"].ToString() + "/" + dtSupplier.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objCustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString()); 
        DataTable dtCustomer = objCustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();

        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListPurchaseInvoice(string prefixText, int count, string contextKey)
    {
        try
        {
            if(SupplierID=="")
            {
                Page page = (Page)HttpContext.Current.Handler;
                TextBox tb = (TextBox)page.FindControl("txtCompanyName");
                tb.Focus();
            }

            PurchaseInvoice objPurchaseInvoice = new PurchaseInvoice(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable _dt = objPurchaseInvoice.GetPurchaseInvoiceTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            string filtertext = "InvoiceNo like '%" + prefixText + "%' and SupplierId='" + SupplierID + "'";
            DataTable dtCon = new DataView(_dt, filtertext, "", DataViewRowState.CurrentRows).ToTable();

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["InvoiceNo"].ToString() + "/" + dtCon.Rows[i]["TransId"].ToString();
                }
            }
            return filterlist;
        }
        catch
        {
            string[] filterlist = { };
            return filterlist;
        }
        
    }

    //protected string GetEmployeeCode(string strEmployeeId)
    //{
    //    string strEmployeeName = string.Empty;
    //    if (strEmployeeId != "0" && strEmployeeId != "")
    //    {
    //        DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId);
    //        if (dtEName.Rows.Count > 0)
    //        {
    //            strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
    //        }
    //    }
    //    else
    //    {
    //        strEmployeeName = "";
    //    }
    //    return strEmployeeName;
    //}

    //public string GetEmployeeName(string EmployeeId)
    //{
    //    string EmployeeName = string.Empty;
    //    DataTable Dt = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), EmployeeId);
    //    if (Dt.Rows.Count > 0)
    //    {
    //        EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
    //        Session["Emp_Img"] = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
    //    }
    //    else
    //    {
    //        Session["Emp_Img"] = "";
    //    }

    //    return EmployeeName;
    //}

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    public void Reset()
    {
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlRefType.SelectedIndex = 0;
        txtVoucherDate.Text = "";
        txtCompanyName.Text = "";
        txtInvoiceNo.Text = "";
        txtInvoiceAmount.Text = "";
        
        txtTaxableAmount.Text = "";
        ddlTax.SelectedIndex = 0;
        txtTaxPercentage.Text = "";
        txtTaxValue.Text = "";
        txtExchangeRate.Text = "";
        txtTaxValue.Text = "";
        hdnCompanyID.Value = "0";
        txtTaxValueLocal.Text = "";
        ddlRefType_SelectedIndexChanged(null, null);
        ddlInvoiceCurrency.SelectedIndex = 0;
        ddlTax.SelectedIndex = 0;
        hdnEditId.Value = "0";
        btnSave.Visible = true;
        txtCustomDeclarationNo.Text = "";
    }
   
   
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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

            DataTable dtVoucher = (DataTable)Session["dtAfterInvoiceTaxDetail"];
            DataView view = new DataView(dtVoucher, condition, "", DataViewRowState.CurrentRows);
            
            objPageCmn.FillData((object)GvList, view.ToTable(), "", "");
            string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            Session["dtFilter_dtAfterInvoiceTaxDetail"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //FillGridBin();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
       
    }
   
   protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        string strbtnStatus = string.Empty;
        if (sender is ImageButton)
        {
            ImageButton btnId = (ImageButton)sender;

            if (btnId.ID == "btnEdit")
            {
                strbtnStatus = "Edit";
                btnSave.Visible = true;
            }

            if (btnId.ID == "lnkViewDetail")
            {
                strbtnStatus = "View";
                btnSave.Visible = false;
            }
        }
        hdnEditId.Value= e.CommandArgument.ToString();

        try
        {
            DataTable _dt = objAfterInvoiceTaxDetail.GetAllActiveRowsByRefTypeAndTransID(Session["LocId"].ToString(), ddlRecType.SelectedValue, hdnEditId.Value);
            ddlRefType.SelectedValue = _dt.Rows[0]["Ref_Type"].ToString();
            ddlRefType_SelectedIndexChanged(null, null);
            txtVoucherDate.Text = Convert.ToDateTime(_dt.Rows[0]["trans_date"]).ToString(objsys.SetDateFormat());
            txtCompanyName.Text = _dt.Rows[0]["strPartyId"].ToString(); 
            txtInvoiceNo.Text = _dt.Rows[0]["strInvoiceNo"].ToString(); 
            txtInvoiceAmount.Text = Common.GetAmountDecimal(_dt.Rows[0]["invoice_amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtTaxableAmount.Text = Common.GetAmountDecimal(_dt.Rows[0]["taxable_amount"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            ddlTax.SelectedValue = _dt.Rows[0]["Tax_id"].ToString();
            txtTaxPercentage.Text = Common.GetAmountDecimal(_dt.Rows[0]["tax_percentage"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtTaxValue.Text = Common.GetAmountDecimal(_dt.Rows[0]["tax_value"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtExchangeRate.Text = Common.GetAmountDecimal(_dt.Rows[0]["exchange_rate"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            hdnCompanyID.Value = _dt.Rows[0]["party_id"].ToString();
            txtTaxValueLocal.Text = Common.GetAmountDecimal((double.Parse(txtTaxValue.Text) * double.Parse(txtExchangeRate.Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()).ToString();
            ddlInvoiceCurrency.SelectedValue = _dt.Rows[0]["currency_id"].ToString();
            txtCustomDeclarationNo.Text= _dt.Rows[0]["field1"].ToString();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtTaxableAmount);
        }
        catch
        {

        }
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        btnSave.Visible = true;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string _tranId = e.CommandArgument.ToString();
        try
        {
            if (objAfterInvoiceTaxDetail.deleteRecord(_tranId,Session["UserId"].ToString())!=0)
            {
                string _voucherHeaderId = "0";
                _voucherHeaderId = da.get_SingleValue("select trans_id from ac_voucher_header where ref_type='TaxEntryAfterInvoice' and ref_id='" + _tranId + "'");
                objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, _voucherHeaderId, "false", StrUserId, DateTime.Now.ToString());
                FillGrid();
                Reset();
                //AllPageCode();
                DisplayMessage("Record has been deleted");
            }
        }
        catch
        {
            DisplayMessage("Record Not deleted");
        }

      
    }
protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objsys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    private void FillGrid()
    {
        DataTable _dt = objAfterInvoiceTaxDetail.GetAllActiveRowsByRefType(strLocationId, ddlRecType.SelectedValue);
        if (_dt != null && _dt.Rows.Count > 0)
        {
            Session["dtAfterInvoiceTaxDetail"] = _dt;
            objPageCmn.FillData((object)GvList, _dt, "", "");
        }
        else
        {
            GvList.DataSource = null;
            GvList.DataBind();
        }


     
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + _dt.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    public string GetFinanceCode(string strFinanceId)
    {
        string strFinanceCode = string.Empty;
        DataTable dtFI = objFYI.GetInfoByTransId(StrCompId, strFinanceId);
        if (dtFI.Rows.Count > 0)
        {
            strFinanceCode = dtFI.Rows[0]["Finance_Code"].ToString();
        }
        else
        {
            strFinanceCode = "0";
        }
        return strFinanceCode;
    }
    protected string GetCurrencyName(string strCurrencyId)
    {
        string strCurrencyName = string.Empty;
        if (strCurrencyId != "0" && strCurrencyId != "")
        {
            DataTable dtCName = objCurrency.GetCurrencyMasterById(strCurrencyId);
            if (dtCName.Rows.Count > 0)
            {
                strCurrencyName = dtCName.Rows[0]["Currency_Name"].ToString();
            }
        }
        else
        {
            strCurrencyName = "";
        }
        return strCurrencyName;
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
        Lbl_Tab_New.Text = Resources.Attendance.View;
        btnSave.Visible = false;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }

    
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }
    protected void GvVoucherBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvVoucherBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtAfterInvoiceTaxDetailBin"];
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        string temp = string.Empty;

        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvVoucherBin.Rows[i].FindControl("lblgvTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        
        //AllPageCode();
    }
    protected void GvVoucherBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtAfterInvoiceTaxDetailBin"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtAfterInvoiceTaxDetailBin"] = dt;
        
        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");

        lblSelectedRecord.Text = "";
        //AllPageCode();
    }
        public void FillGridBin()
        {
        DataTable dt = new DataTable();
        dt = objAfterInvoiceTaxDetail.GetAllInActiveRowsByRefType(strLocationId, ddlRecType.SelectedValue);
        //dt = new DataView(dt, "Voucher_Type='PV'", "", DataViewRowState.CurrentRows).ToTable();

        objPageCmn.FillData((object)GvVoucherBin, dt, "", "");
        Session["dtAfterInvoiceTaxDetailBin"] = dt;
        //Session["dtAfterInvoiceTaxDetailInactive"] = dt;
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
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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


            DataTable dtCust = (DataTable)Session["dtAfterInvoiceTaxDetailBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            //Session["dtAfterInvoiceTaxDetailInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvVoucherBin, view.ToTable(), "", "");
            
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        txtValueBin.Focus();
    }
    //protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    //{
    //    int b = 0;
    //    DataTable dt = objVoucherHeader.GetVoucherHeaderAllFalse(StrCompId, StrBrandId, strLocationId, Session["FinanceYearId"].ToString());

    //    if (GvVoucherBin.Rows.Count != 0)
    //    {
    //        if (lblSelectedRecord.Text != "")
    //        {
    //            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
    //            {
    //                if (lblSelectedRecord.Text.Split(',')[j] != "")
    //                {
    //                    //Msg = objTax.DeleteTaxMaster(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                    b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //                }
    //            }
    //        }

    //        if (b != 0)
    //        {
    //            FillGrid();
    //            FillGridBin();

    //            lblSelectedRecord.Text = "";
    //            DisplayMessage("Record Activate");
    //        }
    //        else
    //        {
    //            int fleg = 0;
    //            foreach (GridViewRow Gvr in GvVoucherBin.Rows)
    //            {
    //                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
    //                if (chk.Checked)
    //                {
    //                    fleg = 1;
    //                }
    //                else
    //                {
    //                    fleg = 0;
    //                }
    //            }
    //            if (fleg == 0)
    //            {
    //                DisplayMessage("Please Select Record");
    //            }
    //            else
    //            {
    //                DisplayMessage("Record Not Activated");
    //            }
    //        }
    //    }
    //    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
//}
    
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
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
                    if (lblSelectedRecord.Text.Split(',')[j].Trim() != "" && lblSelectedRecord.Text.Split(',')[j].Trim() != "0")
                    {
                        string _transId= lblSelectedRecord.Text.Split(',')[j].Trim();
                        b=objAfterInvoiceTaxDetail.restoreRecord(_transId,Session["UserId"].ToString());

                        string _voucherHeaderId = "0";
                        _voucherHeaderId = da.get_SingleValue("select trans_id from ac_voucher_header where ref_type='TaxEntryAfterInvoice' and ref_id='" + _transId + "'");
                        b = objVoucherHeader.DeleteVoucherHeader(StrCompId, StrBrandId, strLocationId,_voucherHeaderId, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            int flag = 0;
            foreach (GridViewRow Gvr in GvVoucherBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvVoucherBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvVoucherBin.Rows.Count; i++)
        {
            ((CheckBox)GvVoucherBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvVoucherBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvVoucherBin.Rows[index].FindControl("lblgvTransId");
        if (((CheckBox)GvVoucherBin.Rows[index].FindControl("chkSelect")).Checked)
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
    protected string GetEmployeeNameByEmpCode(string strEmployeeCode)
    {
        string strEmpName = string.Empty;
        if (strEmployeeCode != "0" && strEmployeeCode != "")
        {
            if (strEmployeeCode == "superadmin")
            {
                strEmpName = "Admin";
            }
            else
            {
                DataTable dtEmp = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeCode);
                if (dtEmp.Rows.Count > 0)
                {
                    strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
                }
            }
        }
        else
        {
            strEmpName = "";
        }
        return strEmpName;
    }
   
    
    protected void ddlRecType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void txtCompanyName_TextChanged(object sender, EventArgs e)
    {
        if (txtCompanyName.Text!=string.Empty)
        {
            try
            {
                DataTable _dt = new DataTable();
                string strId=txtCompanyName.Text.Split('/')[1].ToString();

                if (ddlRefType.SelectedValue=="PINV")
                {
                    _dt = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strId);
                    if(_dt.Rows.Count>0)
                    {
                        if(_dt.Rows[0]["Name"].ToString().Trim() != txtCompanyName.Text.Split('/')[0].Trim())
                        {
                            DisplayMessage("Select from suggestions only");
                            txtCompanyName.Focus();
                            txtCompanyName.Text = "";
                            SupplierID = "";
                            return;
                        }
                        else
                        {
                            hdnCompanyID.Value = _dt.Rows[0]["Supplier_id"].ToString();
                            SupplierID= _dt.Rows[0]["Supplier_id"].ToString(); 
                        }
                    }                    
                }
                else if(ddlRefType.SelectedValue == "SINV")
                {
                    _dt = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strId);
                    if (_dt.Rows.Count > 0)
                    {
                        if (_dt.Rows[0]["Name"].ToString().Trim() != txtCompanyName.Text.Split('/')[0].Trim())
                        {
                            DisplayMessage("Select from suggestions only");
                            txtCompanyName.Focus();
                            txtCompanyName.Text = "";
                            SupplierID = "";
                            return;
                        }
                        else
                        {
                            hdnCompanyID.Value = _dt.Rows[0]["Trans_id"].ToString();
                            SupplierID = _dt.Rows[0]["Trans_id"].ToString();
                        }
                    }
                    
                }

            }
            catch 
            {
                
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtCompanyName);
                DisplayMessage("Please enter valid name");
                SupplierID = "";
                hdnCompanyID.Value = "0";
            }
        }
        else
        {
            SupplierID = "";
        }
    }

    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text != string.Empty)
        {
            try
            {
                DataTable _dt = new DataTable();
                string strId = txtInvoiceNo.Text.Split('/')[1].ToString();
                if (ddlRefType.SelectedValue == "PINV")
                {
                    _dt = objPurchaseInvoiceHeader.GetPurchaseInvoiceTrueAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strId);
                    txtInvoiceAmount.Text = Common.GetAmountDecimal(_dt.Rows[0]["GrandTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    txtTaxableAmount.Text = txtInvoiceAmount.Text;
                    ddlInvoiceCurrency.SelectedValue= _dt.Rows[0]["Currencyid"].ToString();
                    txtExchangeRate.Text= Common.GetAmountDecimal(_dt.Rows[0]["ExchangeRate"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    txtVoucherDate.Text = Convert.ToDateTime(_dt.Rows[0]["InvoiceDate"]).ToString(objsys.SetDateFormat()); 
                }
                else if (ddlRefType.SelectedValue == "SINV")
                {
                    _dt = objSalesInvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strId);
                    txtInvoiceAmount.Text = Common.GetAmountDecimal(_dt.Rows[0]["GrandTotal"].ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
                    txtTaxableAmount.Text = txtInvoiceAmount.Text;
                    ddlInvoiceCurrency.SelectedValue = _dt.Rows[0]["Currency_id"].ToString();
                    txtVoucherDate.Text = Convert.ToDateTime(_dt.Rows[0]["InvoiceDate"]).ToString(objsys.SetDateFormat());
                    txtExchangeRate.Text = "1";
                }

            }
            catch
            {

                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoiceNo);
                DisplayMessage("Please enter valid name");
                txtInvoiceNo.Text = "";
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtVoucherDate.Text == String.Empty)
        {
            DisplayMessage("Please enter valid date");
            txtVoucherDate.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtCompanyName.Text == String.Empty)
        {
            DisplayMessage("Please enter valid Name");
            txtCompanyName.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtInvoiceNo.Text == String.Empty)
        {
            DisplayMessage("Please enter valid Invoice No");
            txtInvoiceNo.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtTaxableAmount.Text == String.Empty || txtTaxableAmount.Text=="0")
        {
            DisplayMessage("Please enter valid taxable amount");
            txtInvoiceNo.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtTaxPercentage.Text == String.Empty || txtTaxPercentage.Text == "0")
        {
            DisplayMessage("Please enter valid tax percentage");
            txtInvoiceNo.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (txtTaxValue.Text == String.Empty || txtTaxValue.Text == "0")
        {
            DisplayMessage("Please enter valid tax value");
            txtTaxValue.Focus();
            btnSave.Enabled = true;
            return;
        }
        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "F", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            btnSave.Enabled = true;
            return;
        }

        //Expenses Account Parameter
        string strDebitAccountId = "0";
        string strTaxLedgerID = "0";
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        SqlTransaction trns;
        con.Open();
        trns = con.BeginTransaction();
        try
        {
            DataTable _dt = new DataTable();
            _dt = objInvParameterMaster.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Expenses Account Parameter");
            if (_dt.Rows.Count>0)
            {
                strDebitAccountId = _dt.Rows[0]["ParameterValue"].ToString();
            }
            else
            {
                DisplayMessage("Please Check expenses Account in Inventory Prameter");
                btnSave.Enabled = true;
                return;
            }
            _dt = objTaxMaster.GetTaxMasterById(ddlTax.SelectedValue);
            if (_dt.Rows.Count > 0)
            {
                strTaxLedgerID = _dt.Rows[0]["Field3"].ToString();
            }
            else
            {
                DisplayMessage("Please set chart of finance for tax");
                btnSave.Enabled = true;
                return;
            }
            

            string strInvoiceId = txtInvoiceNo.Text.Split('/')[1].ToString();
            
            

            string strNarration = string.Empty;
            strNarration = ddlTax.SelectedItem.Text + " - " + txtTaxPercentage.Text + "% on invoice No - " + txtInvoiceNo.Text;
            int _recId = 0;

            if (hdnEditId.Value == "0")
            {
                _recId = objAfterInvoiceTaxDetail.insertRecord(txtVoucherDate.Text, ddlRefType.SelectedValue,strInvoiceId, txtTaxableAmount.Text, txtExchangeRate.Text, ddlTax.SelectedValue, txtTaxPercentage.Text, txtTaxValue.Text, txtCustomDeclarationNo.Text, string.Empty, string.Empty, string.Empty, string.Empty, "False", "01-01-1900", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "302", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                if (strVoucherNumber != "")
                {
                    int counter = objAccParameter.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "JV", Session["FinanceYearId"].ToString(), ref trns);

                    if (counter == 0)
                    {
                        strVoucherNumber = strVoucherNumber + "1";
                    }
                    else
                    {
                        strVoucherNumber = strVoucherNumber + (counter + 1);
                    }
                }

                
                int _voucher_id = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), _recId.ToString(), "TaxEntryAfterInvoice", "0", DateTime.Now.ToString(), strVoucherNumber, objsys.getDateForInput(txtVoucherDate.Text).ToString(), "JV", "01-Jan-1900", "01-Jan-1900", "", "0", ddlInvoiceCurrency.SelectedValue, txtExchangeRate.Text, strNarration, false.ToString(), false.ToString(), true.ToString(), "JV", "", "Approved", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Debit Entry
                objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),_voucher_id.ToString(), "1", strDebitAccountId, "0", _recId.ToString(), "TaxEntryAfterInvoice", "01-Jan-1900", "01-Jan-1900", "", txtTaxValueLocal.Text, "0.00", strNarration, "", Session["EmpId"].ToString(), ddlInvoiceCurrency.SelectedValue, txtExchangeRate.Text, txtTaxValue.Text, txtTaxValue.Text, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                //Credit Entry
                objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _voucher_id.ToString(), "1", strTaxLedgerID, "0", _recId.ToString(), "TaxEntryAfterInvoice", "01-Jan-1900", "01-Jan-1900", "", "0.00", txtTaxValueLocal.Text, strNarration, "", Session["EmpId"].ToString(), ddlInvoiceCurrency.SelectedValue, txtExchangeRate.Text, txtTaxValue.Text, "0.00", txtTaxValue.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                DisplayMessage("Record Saved Successfully","green");
            }
            else
            {
                int _result = 0;
                _recId = int.Parse(hdnEditId.Value);
                _result = objAfterInvoiceTaxDetail.updateRecord(hdnEditId.Value,txtVoucherDate.Text, ddlRefType.SelectedValue, strInvoiceId, txtTaxableAmount.Text, txtExchangeRate.Text, ddlTax.SelectedValue, txtTaxPercentage.Text, txtTaxValue.Text, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "False", "01-Jan-1900", "True", Session["UserId"].ToString(), DateTime.Now.ToString(),ref trns);
                
                if (_result==1)
                {
                    string sql = "select trans_id,voucher_no from ac_voucher_header where location_id=" + Session["LocId"].ToString() + " and ref_type='TaxEntryAfterInvoice' and ref_id=" + hdnEditId.Value;
                    DataTable _dtVoucherHeader = da.return_DataTable(sql);
                    if (_dtVoucherHeader.Rows.Count==0)
                    {
                        DisplayMessage("Related voucher has been deleted, so can't edit it");
                        btnSave.Enabled = true;
                        return;
                    }

                    string _voucherId = _dtVoucherHeader.Rows[0]["trans_id"].ToString();
                    string _voucherNo = _dtVoucherHeader.Rows[0]["voucher_no"].ToString();

                    objVoucherHeader.UpdateVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _voucherId, Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), _recId.ToString(), "TaxEntryAfterInvoice", "0", DateTime.Now.ToString(), _voucherNo, objsys.getDateForInput(txtVoucherDate.Text).ToString(), "JV", "01-Jan-1900", "01-Jan-1900", "", "0", ddlInvoiceCurrency.SelectedValue, txtExchangeRate.Text, strNarration, false.ToString(), false.ToString(), true.ToString(), "JV", "", "Approved", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    objVoucherDetail.DeleteVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _voucherId);

                    //Debit Entry
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _voucherId.ToString(), "1", strDebitAccountId, "0", _recId.ToString(), "TaxEntryAfterInvoice", "01-Jan-1900", "01-Jan-1900", "", txtTaxValueLocal.Text, "0.00", strNarration, "", Session["EmpId"].ToString(), ddlInvoiceCurrency.SelectedValue, txtExchangeRate.Text, txtTaxValue.Text, txtTaxValue.Text, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    //Credit Entry
                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), _voucherId.ToString(), "1", strTaxLedgerID, "0", _recId.ToString(), "TaxEntryAfterInvoice", "01-Jan-1900", "01-Jan-1900", "", "0.00", txtTaxValueLocal.Text, strNarration, "", Session["EmpId"].ToString(), ddlInvoiceCurrency.SelectedValue, txtExchangeRate.Text, txtTaxValue.Text, "0.00", txtTaxValue.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                    DisplayMessage("Record Updated Successfully", "green");
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
            btnSave.Enabled = true;
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
            btnSave.Enabled = true;
            return;
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    protected void GvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvList.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtAfterInvoiceTaxDetail"];
        objPageCmn.FillData((object)GvList, dt, "", "");
        //AllPageCode();
    }

    protected void GvList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtAfterInvoiceTaxDetail"];
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
        Session["dtAfterInvoiceTaxDetail"] = dt;
        objPageCmn.FillData((object)GvList, dt, "", "");
        //AllPageCode();
    }

    protected void ddlRefType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRefType.SelectedValue=="PINV")
        {
            AutoCompleteExtenderSupplier.Enabled = true;
            AutoCompleteExtenderPINV.Enabled = true;
            AutoCompleteExtenderCustomer.Enabled = false;
            AutoCompleteExtenderSINV.Enabled = false;
        }
        else if (ddlRefType.SelectedValue=="SINV")
        {
            AutoCompleteExtenderCustomer.Enabled = true;
            AutoCompleteExtenderSupplier.Enabled = false;
            AutoCompleteExtenderPINV.Enabled = false;
            AutoCompleteExtenderSINV.Enabled = true;
        }
    }
    protected void FillTax()
    {
        string TaxQuery = "Select Trans_Id as Id, Tax_Name as Name from Sys_TaxMaster where isActive='true' order by tax_name";
        DataTable Taxdt = da.return_DataTable(TaxQuery);
        if (Taxdt != null && Taxdt.Rows.Count > 0)
        {
            ddlTax.DataTextField = "Name";
            ddlTax.DataValueField = "Id";
            ddlTax.DataSource = Taxdt;
            ddlTax.DataBind();
            ddlTax.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }
    protected void calcuateTaxValue()
    {
        string taxValue = "0";
        try
        {
            taxValue = ((double.Parse(txtTaxableAmount.Text) * double.Parse(txtTaxPercentage.Text)) / 100).ToString();
            txtTaxValue.Text = Common.GetAmountDecimal(taxValue, Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            txtTaxValueLocal.Text = Common.GetAmountDecimal((double.Parse(taxValue) * double.Parse(txtExchangeRate.Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()).ToString();
        }
        
        catch { }
        try
        {
            txtTaxValueLocal.Text = Common.GetAmountDecimal((double.Parse(taxValue) * double.Parse(txtExchangeRate.Text)).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString()).ToString();
        }
        catch
        {

        }
    }

    protected void txtTaxableAmount_TextChanged(object sender, EventArgs e)
    {
        //if (txtTaxableAmount.Text!=string.Empty && txtInvoiceAmount.Text!=string.Empty)
        //{
        //    if(double.Parse(txtTaxableAmount.Text) > double.Parse(txtInvoiceAmount.Text))
        //    {
        //        DisplayMessage("Taxable Amount can not greater than invoice amount");
        //        txtTaxableAmount.Text = txtInvoiceAmount.Text;
        //        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtInvoiceNo);
        //        return;
        //    }
        //}
        calcuateTaxValue();
    }

    protected void txtTaxPercentage_TextChanged(object sender, EventArgs e)
    {
        calcuateTaxValue();
    }

    protected void txtExchangeRate_TextChanged(object sender, EventArgs e)
    {
        calcuateTaxValue();
    }
    private void UcCtlSetting_refreshPageControl(object sender, EventArgs e)
    {
        Update_List.Update();
        Update_New.Update();
    }
    protected void btnGvListSetting_Click(object sender, EventArgs e)
    {
        lblUcSettingsTitle.Text = "Set Columns Visibility";
        ucCtlSetting.getGrdColumnsSettings(strPageName, GvList, grdDefaultColCount);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }
    protected void getPageControlsVisibility()
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        objPageCtlSettting.setControlsVisibility(lstCls, this.Page);
        objPageCtlSettting.setColumnsVisibility(GvList, lstCls);
    }
    protected void btnControlsSetting_Click(object sender, ImageClickEventArgs e)
    {
        List<PageControlsSetting.clsContorlsSetting> lstCls = objPageCtlSettting.getControlsSetting(strPageName);
        lblUcSettingsTitle.Text = "Set Controls attribute";
        ucCtlSetting.getPageControlsSetting(strPageName, lstCls);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "showUcControlsSettings()", true);
    }

}