using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_Purchase_Return : BasePage
{
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    Inv_PurchaseQuoteHeader objPurchaseQuotation = null;
    Set_Suppliers objSupplier = null;
    Inv_SalesOrderHeader objSalesOrder = null;
    PurchaseOrderDetail ObjPoDetail = null;
    Inv_PurchaseQuoteDetail objPqDetail = null;
    CurrencyMaster objCurrency = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_ProductMaster ObjProductMaster = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    Inv_UnitMaster objUnit = null;
    PurchaseInvoice ObjPinvoiceHeader = null;
    Inv_PurchaseReturnHeader ObjPReturnHeader = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objPurchaseQuotation = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objSalesOrder = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjPoDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        objPqDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjPinvoiceHeader = new PurchaseInvoice(Session["DBConnection"].ToString());
        ObjPReturnHeader = new Inv_PurchaseReturnHeader(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

        }
        AllPageCode();

    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("187", (DataTable)Session["ModuleName"]);
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



        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;



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
    protected void btngo_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;


        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;




        PurchaseDataSetTableAdapters.sp_Inv_PurchaseReturn_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseReturn_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseReturn_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
        DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseReturn_SelectRow_Report;


        if (chkReturnNo.Checked == true)
        {
            Session["ReportHeader"] = "Purchase Return Report By Return No";
            Session["ReportType"] = "1";

        }
        else
        {
            Session["ReportHeader"] = "Purchase Return Report By Return Date";
            Session["ReportType"] = "0";


        }


        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            try
            {
                DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
            Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;
        }
        else
        {
            DataTable Dt = DtFilter.Copy();
            Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
            if (Dt.Rows.Count > 0)
            {

                txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            }
            txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;

        }
        if (txtReturnNo.Text != "")
        {

            try
            {
                DtFilter = new DataView(DtFilter, "Trans_Id=" + hdnReturnId.Value + "", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
            if (chkReturnNo.Checked == false)
            {
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Return Report by Return No.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Return No.";
                }
            }

        }


        if (txtInvoiceNo.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Invoice_Id=" + hdnInvoiceNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            if (Session["ReportHeader"] == null)
            {
                Session["ReportHeader"] = "Purchase Return Report by Invoice No.";
            }
            else
            {
                Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice No.";
            }

        }



        if (txtSupplierName.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "SupplierId='" + hdnSupplierId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            if (Session["ReportHeader"] == null)
            {
                Session["ReportHeader"] = "Purchase Return Report by Supplier";
            }
            else
            {
                Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
            }

        }


        if (txtProductName.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            if (Session["ReportHeader"] == null)
            {
                Session["ReportHeader"] = "Purchase Return Report by Product";
            }
            else
            {
                Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Product";
            }

        }
        //if (txtQuotationNo.Text != "")
        //{
        //    try
        //    {
        //        dtPoheader = new DataView(dtPoheader, "ReferenceVoucherType='PQ' and ReferenceID='"+hdnQuotationId.Value+"'", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //    catch
        //    {
        //    }

        //}

        if (DtFilter.Rows.Count > 0)
        {





            Session["DtFilter"] = DtFilter;
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/Purchase_Return_Report.aspx','window','width=1024');", true);


            //Response.Redirect("../Purchase_Report/Purchase_Return_Report.aspx");
        }
        else
        {
            DisplayMessage("Record Not Found");
            txtFromDate.Focus();
            return;
        }












    }
    protected void btnReset_Click(object sender, EventArgs e)
    {


        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";

        txtInvoiceNo.Text = "";

        txtReturnNo.Text = "";
        txtProductName.Text = "";

        txtSupplierName.Text = "";





    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductName.Text.ToString());
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductName.Text = "";
                txtProductName.Focus();
                return;
            }
        }


    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnSupplierId.Value = dt.Rows[0]["Contact_Id"].ToString();

            }
            else
            {
                DisplayMessage("select supplier in suggestion only");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                return;
            }


        }


    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["locId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["EProductName"].ToString();
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_InvoiceNo(string prefixText, int count, string contextKey)
    {
        PurchaseInvoice objPInvoiceHeader = new PurchaseInvoice(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objPInvoiceHeader.GetPurchaseInvoiceTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["InvoiceNo"].ToString();
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objPInvoiceHeader.GetPurchaseInvoiceTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["InvoiceNo"].ToString();
                    }
                }
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_ReturnNo(string prefixText, int count, string contextKey)
    {
        Inv_PurchaseReturnHeader objPReturnHeader = new Inv_PurchaseReturnHeader(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objPReturnHeader.GetPRHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["PReturn_No"].ToString();
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = objPReturnHeader.GetPRHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["PReturn_No"].ToString();
                    }
                }
            }
        }
        return str;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());
        DataTable dtMain = new DataTable();
        dtMain = dtSupplier.Copy();


        string filtertext = "Name like '" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtSupplier;
        }
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }




    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = ObjPinvoiceHeader.GetPurchaseInvoiceTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            Dt = new DataView(Dt, "InvoiceNo='" + txtInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (Dt.Rows.Count > 0)
        {
            hdnInvoiceNo.Value = Dt.Rows[0]["TransID"].ToString();
        }
        else
        {
            DisplayMessage("Select Invoice No. in suggestion Only");
            txtInvoiceNo.Text = "";
            txtInvoiceNo.Focus();

            return;
        }

    }
    protected void txtReturnNo_TextChanged(object sender, EventArgs e)
    {


        DataTable Dt = ObjPReturnHeader.GetPRHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            Dt = new DataView(Dt, "PReturn_No='" + txtReturnNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (Dt.Rows.Count > 0)
        {
            hdnReturnId.Value = Dt.Rows[0]["Trans_ID"].ToString();
        }
        else
        {
            DisplayMessage("Select Return No. in suggestion Only");
            txtReturnNo.Text = "";
            txtReturnNo.Focus();

            return;
        }

    }

}
