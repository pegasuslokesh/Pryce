using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_PurchaseReport : BasePage
{
    SystemParameter ObjSysParam = null;
    Inv_ProductMaster ObjProductMaster = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_PurchaseInquiryHeader objPInquiryHeader = null;
    Inv_PurchaseQuoteHeader objPQuotHeader = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    Set_Suppliers objSupplier = null;
    PurchaseOrderDetail ObjPoDetail = null;
    Inv_PurchaseQuoteDetail objPqDetail = null;
    CurrencyMaster objCurrency = null;
    Set_Payment_Mode_Master objPaymentMode = null;
    Inv_UnitMaster objUnit = null;
    Inv_SalesOrderHeader objSalesOrder = null;
    PurchaseInvoice ObjPinvoiceHeader = null;
    Inv_PurchaseReturnHeader ObjPReturnHeader = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPInquiryHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objPQuotHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjPoDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        objPqDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objPaymentMode = new Set_Payment_Mode_Master(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objSalesOrder = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        ObjPinvoiceHeader = new PurchaseInvoice(Session["DBConnection"].ToString());
        ObjPReturnHeader = new Inv_PurchaseReturnHeader(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            btnMenuRequest_Click(null, null);
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            CalendarExtender3.Format = Session["DateFormat"].ToString();
            CalendarExtender4.Format = Session["DateFormat"].ToString();
            CalendarExtender5.Format = Session["DateFormat"].ToString();
            CalendarExtender6.Format = Session["DateFormat"].ToString();
            CalendarExtender7.Format = Session["DateFormat"].ToString();
            CalendarExtender8.Format = Session["DateFormat"].ToString();
            CalendarExtender9.Format = Session["DateFormat"].ToString();
            CalendarExtender10.Format = Session["DateFormat"].ToString();
            CalendarExtender11.Format = Session["DateFormat"].ToString();
            CalendarExtender12.Format = Session["DateFormat"].ToString();
            CalendarExtender13.Format = Session["DateFormat"].ToString();

            btnMenuRequest_Click(null, null);

        }


        AllPageCode();
    }


    public void AllPageCode()
    {


        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "146";
        Session["HeaderText"] = "Purchase Report";
        string a = Resources.Attendance.Working;


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





    protected void btnMenuQuotation_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlRequest.Visible = false;
        PnlInquiry.Visible = false;
        PnlQuotation.Visible = true;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlGoodsReceive.Visible = false;
        PnlReturn.Visible = false;
        ddlQuotationReportType.SelectedIndex = 0;
        ddlQuotationReportType_SelectedIndexChanged(null, null);


    }
    protected void btnMenuOrder_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlRequest.Visible = false;
        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = true;
        PnlInvoice.Visible = false;
        PnlGoodsReceive.Visible = false;
        PnlReturn.Visible = false;
        ddlOrderReportType_SelectedIndexChanged(null, null);


    }
    protected void btnMenuInvoice_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlRequest.Visible = false;
        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = true;
        PnlGoodsReceive.Visible = false;
        PnlReturn.Visible = false;

        ddlInvoiceReportType_SelectedIndexChanged(null, null);
    }
    protected void btnMenuGoodReceive_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlRequest.Visible = false;
        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlGoodsReceive.Visible = true;
        PnlReturn.Visible = false;
        BindddlGroup_Goods();

    }
    protected void btnMenuReturn_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlRequest.Visible = false;
        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlGoodsReceive.Visible = false;
        PnlReturn.Visible = true;
        BindddlGroup_Return();

    }

    #region PurchaseRequest
    public void BindddlGroup()
    {
        try
        {
            ddlGroupBy.Items.Clear();
        }
        catch
        {
        }
        ddlGroupBy.Items.Insert(0, Resources.Attendance.Request_Date);

        if (ddlReportType.SelectedValue == "0")
        {
            ddlGroupBy.Items.Insert(1, Resources.Attendance.Department);
        }
        else
        {
            ddlGroupBy.Items.Insert(1, Resources.Attendance.Request_No);
        }



    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroup();
        if (ddlReportType.SelectedValue == "0")
        {
            lblcolon.Visible = false;
            lblProductName.Visible = false;
            txtProductName.Visible = false;
            lblStatus.Visible = true;
            lblStatusColon.Visible = true;
            ddlStatus.Visible = true;

        }
        else
        {
            lblcolon.Visible = true;
            lblProductName.Visible = true;
            txtProductName.Visible = true;
            lblStatus.Visible = false;
            lblStatusColon.Visible = false;
            ddlStatus.Visible = false;

        }
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
    protected void btnMenuRequest_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlRequest.Visible = true;
        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlGoodsReceive.Visible = false;
        PnlReturn.Visible = false;
        ddlReportType_SelectedIndexChanged(null, null);


    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        Session["ReportType"] = null;
        Session["ReportHeader"] = null;
        Session["Parameter"] = null;
        Session["DtFilter"] = null;


        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (ddlReportType.SelectedValue == "0")
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseRequestHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseRequestHeader_SelectRow_Report;


            if (ddlGroupBy.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Request_Header_Report_By_Department;
                Session["ReportType"] = "0";

                //here we calling the PRequestHeaderByDepartment.cs report object
            }
            if (ddlGroupBy.SelectedIndex == 0)
            {


                Session["ReportHeader"] = Resources.Attendance.Purchase_Request_Header_Report_By_Request_Date;
                Session["ReportType"] = "1";
                //here we calling the PRequestHeader.cs report object

            }
            string Date = "";

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
                Session["Parameter"] = "From: " + txtFromDate.Text + " To: " + txtToDate.Text;




            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "RequestDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["RequestDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
                Session["Parameter"] = "From: " + txtFromDate.Text + " To: " + txtToDate.Text;

                txtFromDate.Text = "";
                txtToDate.Text = "";


            }
            Date = Session["Parameter"].ToString();
            if (txtRequestNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "RequestNo='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }
            if (ddlStatus.SelectedIndex > 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Status='" + ddlStatus.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }







            try
            {
                DtFilter = new DataView(DtFilter, "Department_Name<>' '", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PRequestHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }

                //Response.Redirect("../Purchase_Report/PRequestHeaderReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");


                return;
            }
        }
        if (ddlReportType.SelectedValue == "1")
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestDetail_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseRequestDetail_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseRequestDetail_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseRequestDetail_Report;
            if (ddlGroupBy.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Request_Detail_Report_By_Request_Date;
                Session["ReportType"] = "0";
            }
            if (ddlGroupBy.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Request_Detail_Report_By_Request_Number;
                Session["ReportType"] = "1";
                //here we calling the PRequestHeaderByDepartment.cs report object
            }
            //if (ChkProduct.Checked == true)
            //{
            //    Session["ReportHeader"] = "Purchase Request Detail Report By Product";
            //    Session["ReportType"] = "2";
            //    //here we calling the PRequestHeader.cs report object

            //}

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
                Session["Parameter"] = "From: " + txtFromDate.Text + " To: " + txtToDate.Text;




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
                Session["Parameter"] = Resources.Attendance.From + ": " + txtFromDate.Text + ' ' + Resources.Attendance.To + ": " + txtToDate.Text;



                txtFromDate.Text = "";
                txtToDate.Text = "";

            }
            if (txtRequestNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "RequestNo='" + txtRequestNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }




            }
            if (ddlStatus.SelectedIndex > 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Status='" + ddlStatus.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
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



                //Response.Redirect("../Purchase_Report/PRequestdetailReportByProduct.aspx");



            }

            if (DtFilter.Rows.Count > 0)
            {

                Session["DtFilter"] = DtFilter;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PRequestdetailReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }



                //Response.Redirect("../Purchase_Report/PRequestdetailReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlReportType.SelectedIndex = 0;
        ddlReportType_SelectedIndexChanged(null, null);
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtRequestNo.Text = "";
        ddlStatus.SelectedIndex = 0;

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
    public static string[] GetCompletionListRequestNo(string prefixText, int count, string contextKey)
    {
        PurchaseRequestHeader objPRequestHeader = new PurchaseRequestHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objPRequestHeader.GetPurchaseRequestHeaderTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["RequestNo"].ToString();
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
                dt = objPRequestHeader.GetPurchaseRequestHeaderTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["RequestNo"].ToString();
                    }
                }
            }
        }
        return str;
    }
    #endregion
    #region PurchaseInquiry
    protected void btnMenuInquiry_Click(object sender, EventArgs e)
    {
        pnlMenuRequest.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuGoodReceive.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        pnlRequest.Visible = false;
        PnlInquiry.Visible = true;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlGoodsReceive.Visible = false;
        PnlReturn.Visible = false;
        ddlInquiryReportType_SelectedIndexChanged(null, null);


    }
    public void BindddlGroupInquiry()
    {
        try
        {
            DdlInquiryGroupBy.Items.Clear();
        }
        catch
        {
        }
        DdlInquiryGroupBy.Items.Insert(0, Resources.Attendance.Inquiry_Date);


        if (ddlInquiryReportType.SelectedValue == "0")
        {
            DdlInquiryGroupBy.Items.Insert(1, Resources.Attendance.Supplier);

        }
        else
        {
            DdlInquiryGroupBy.Items.Insert(1, Resources.Attendance.Inquiry_No_);
        }




    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListInquiryNo(string prefixText, int count, string contextKey)
    {
        Inv_PurchaseInquiryHeader objPInquiryHeader = new Inv_PurchaseInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objPInquiryHeader.GetPIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["PI_No"].ToString();
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
                dt = objPInquiryHeader.GetPIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["PI_No"].ToString();
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
    protected void txtInquiryNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInquiryNo.Text != "")
        {
            DataTable Dt = objPInquiryHeader.GetPIHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text);
            if (Dt.Rows.Count > 0)
            {
            }
            else
            {
                DisplayMessage("Select Inquiry No. in suggestion Only");
                txtInquiryNo.Text = "";
                txtInquiryNo.Focus();

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

                hdnSupplierId.Value = dt.Rows[0]["Trans_Id"].ToString();

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
    protected void txtInquiryProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtInquiryProductName.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtInquiryProductName.Text.ToString());
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtInquiryProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtInquiryProductName.Text = "";
                txtInquiryProductName.Focus();
                return;
            }
        }


    }
    protected void ddlInquiryReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroupInquiry();
        if (ddlInquiryReportType.SelectedValue == "0")
        {
            lblInquiryProductName.Visible = false;
            lblProductColonInquiry.Visible = false;
            txtInquiryProductName.Visible = false;
            lblStatusInquiry.Visible = true;
            lblStatusColonInquiry.Visible = true;
            ddlStatusInquiry.Visible = true;

        }
        else
        {
            lblInquiryProductName.Visible = true;
            lblProductColonInquiry.Visible = true;
            txtInquiryProductName.Visible = true;
            lblStatusInquiry.Visible = false;
            lblStatusColonInquiry.Visible = false;
            ddlStatusInquiry.Visible = false;

        }
    }
    protected void BtnInquiryReport_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        Session["ReportHeader"] = null;
        Session["SupplierGroup"] = null;

        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (ddlInquiryReportType.SelectedIndex == 0)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();


            if (DdlInquiryGroupBy.SelectedIndex == 0)
            {
                adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInquiryHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 2);

                Session["ReportHeader"] = Resources.Attendance.Purchase_Inquiry_Header_Report_By_Inquiry_Date;


                Session["SupplierGroup"] = "No";

            }
            else
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Inquiry_Header_Report_By_Supplier;
                adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInquiryHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 1);
                Session["SupplierGroup"] = "Yes";
            }
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInquiryHeader_SelectRow_Report;
            if (txtFromDateInquiry.Text != "" && txtTodateInquiry.Text != "")
            {
                DateTime ToDate = new DateTime();
                try
                {
                    ToDate = Convert.ToDateTime(txtTodateInquiry.Text);
                    DateTime FromDate = Convert.ToDateTime(txtFromDateInquiry.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date");
                    txtFromDateInquiry.Text = "";
                    txtTodateInquiry.Text = "";
                    txtFromDateInquiry.Focus();
                    return;
                }
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateInquiry.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
                Session["Parameter"] = "From: " + txtFromDateInquiry.Text + "  To: " + txtTodateInquiry.Text;

            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateInquiry.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtTodateInquiry.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateInquiry.Text + "  To: " + txtTodateInquiry.Text;

                txtFromDateInquiry.Text = "";
                txtTodateInquiry.Text = "";


            }
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "PI_No='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }


            if (ddlStatusInquiry.SelectedIndex == 1)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status ='" + ddlStatusInquiry.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }

            }
            if (ddlStatusInquiry.SelectedIndex == 2)
            {
                {
                    try
                    {
                        DtFilter = new DataView(DtFilter, "Status <>'Pending'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }

                }

            }







            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PInquiryHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }


            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }
        if (ddlInquiryReportType.SelectedIndex == 1)
        {

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInquiryDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInquiryDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInquiryDetail_SelectRow_Report;

            if (DdlInquiryGroupBy.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Inquiry_Detail_Report_By_Inquiry_Date;

                Session["ReportType"] = "0";
            }
            if (DdlInquiryGroupBy.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Inquiry_Detail_Report_By_Inquiry_No_;
                Session["ReportType"] = "2";
            }


            //if (chkSupplier.Checked == true)
            //{
            //    Session["ReportHeader"] = "Purchase Inquiry Detail Report By Supplier";
            //    Session["ReportType"] = "1";

            //}

            if (txtFromDateInquiry.Text != "" && txtTodateInquiry.Text != "")
            {
                DateTime ToDate = new DateTime();
                try
                {
                    ToDate = Convert.ToDateTime(txtTodateInquiry.Text);
                    DateTime FromDate = Convert.ToDateTime(txtFromDateInquiry.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date");
                    txtFromDateInquiry.Text = "";
                    txtTodateInquiry.Text = "";
                    txtFromDateInquiry.Focus();
                    return;
                }
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateInquiry.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
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

                    txtFromDateInquiry.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtTodateInquiry.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateInquiry.Text + "  To: " + txtTodateInquiry.Text;

                txtFromDateInquiry.Text = "";
                txtTodateInquiry.Text = "";


            }
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "PI_No='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
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




            }

            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PInquirydetailReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }


            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }






    }
    protected void BtnResetInquiryReport_Click(object sender, EventArgs e)
    {
        ddlInquiryReportType.SelectedIndex = 0;
        ddlInquiryReportType_SelectedIndexChanged(null, null);
        txtFromDateInquiry.Text = "";
        txtTodateInquiry.Text = "";
        txtInquiryProductName.Text = "";
        txtInquiryNo.Text = "";
        txtSupplierName.Text = "";
        ddlStatus.SelectedIndex = 0;

    }
    #endregion
    #region Purchase_Quotation
    public void BindddlGroupQuotation()
    {
        try
        {
            DdlQuotationGroupBy.Items.Clear();
        }
        catch
        {
        }
        DdlQuotationGroupBy.Items.Insert(0, Resources.Attendance.Quotation_Date);
        DdlQuotationGroupBy.Items.Insert(1, Resources.Attendance.Quotation_No_);
        DdlQuotationGroupBy.Items.Insert(2, Resources.Attendance.Supplier);
        if (ddlQuotationReportType.SelectedIndex == 0)
        {
            DdlQuotationGroupBy.Items.Insert(3, Resources.Attendance.Inquiry_No_);
        }







    }

    protected void ddlQuotationReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroupQuotation();
        if (ddlQuotationReportType.SelectedValue == "0")
        {
            lblProductNameQuotation.Visible = false;
            lblProductcolonQuotation.Visible = false;
            txtProductNameQuotation.Visible = false;


        }
        else
        {

            lblProductNameQuotation.Visible = true;
            lblProductcolonQuotation.Visible = true;
            txtProductNameQuotation.Visible = true;
        }
    }
    protected void txtInquiryInQuotation_TextChanged(object sender, EventArgs e)
    {
        if (txtInquiryInQuotation.Text != "")
        {
            DataTable Dt = objPInquiryHeader.GetPIHeaderAllDataByPI_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryInQuotation.Text);
            if (Dt.Rows.Count > 0)
            {
            }
            else
            {
                DisplayMessage("Select Inquiry No. in suggestion Only");
                txtInquiryInQuotation.Text = "";
                txtInquiryInQuotation.Focus();

                return;
            }
        }
    }
    protected void txtSupplierNameQuotation_TextChanged(object sender, EventArgs e)
    {

        if (txtSupplierNameQuotation.Text != "")
        {

            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierNameQuotation.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnSupplierId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select supplier in suggestion only");
                txtSupplierNameQuotation.Text = "";
                txtSupplierNameQuotation.Focus();
                return;
            }


        }


    }
    protected void txtProductNameQuotation_TextChanged(object sender, EventArgs e)
    {
        if (txtProductNameQuotation.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductNameQuotation.Text);
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductNameQuotation.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductNameQuotation.Text = "";
                txtProductNameQuotation.Focus();
                return;
            }
        }


    }

    protected void txtQuotationNo_TextChanged(object sender, EventArgs e)
    {
        if (txtQuotationNo.Text != "")
        {
            DataTable Dt = objPQuotHeader.GetQuoteHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                Dt = new DataView(Dt, "RPQ_No='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (Dt.Rows.Count > 0)
            {
            }
            else
            {
                DisplayMessage("Select Quotation No. in suggestion Only");
                txtQuotationNo.Text = "";
                txtQuotationNo.Focus();

                return;
            }
        }

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListQuotationNo(string prefixText, int count, string contextKey)
    {
        Inv_PurchaseQuoteHeader objPQuotHeader = new Inv_PurchaseQuoteHeader(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objPQuotHeader.GetQuoteHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["RPQ_No"].ToString();
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
                dt = objPQuotHeader.GetQuoteHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["RPQ_No"].ToString();
                    }
                }
            }
        }
        return str;
    }
    protected void BtnQuotationReport_Click(object sender, EventArgs e)
    {

        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        Session["ReportType"] = null;
        Session["ReportHeader"] = null;


        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (ddlQuotationReportType.SelectedIndex == 0)
        {
            if (DdlQuotationGroupBy.SelectedIndex == 0)
            {
                //showing the quotation Report By date
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Header_Report_By_Quotation_Date;
                Session["ReportType"] = "3";
            }
            if (DdlQuotationGroupBy.SelectedIndex == 1)
            {
                //showing the quotation Report By Quotation No
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Header_Report_By_Quotation_No_;
                Session["ReportType"] = "1";
            }
            if (DdlQuotationGroupBy.SelectedIndex == 2)
            {
                //showing the quotation Report By Supplier
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Header_Report_By_Supplier;
                Session["ReportType"] = "2";
            }
            if (DdlQuotationGroupBy.SelectedIndex == 3)
            {
                //showing the quotation Report By Inquiry No
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Header_Report_By_Inquiry_No_;
                Session["ReportType"] = "0";

            }
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseQuoteHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 2);




            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseQuoteHeader_SelectRow_Report;



            if (txtFromDateQuotation.Text != "" && txtToDateQuotation.Text != "")
            {
                DateTime ToDate = new DateTime();
                try
                {
                    ToDate = Convert.ToDateTime(txtToDateQuotation.Text);
                    DateTime FromDate = Convert.ToDateTime(txtFromDateQuotation.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date");
                    txtFromDateQuotation.Text = "";
                    txtToDateQuotation.Text = "";
                    txtFromDateQuotation.Focus();
                    return;
                }
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateQuotation.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
                Session["Parameter"] = "From: " + txtFromDateQuotation.Text + "  To: " + txtToDateQuotation.Text;

            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "RPQ_Date asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateQuotation.Text = Convert.ToDateTime(Dt.Rows[0]["RPQ_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateQuotation.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateQuotation.Text + "  To: " + txtToDateQuotation.Text;

                txtFromDateQuotation.Text = "";
                txtToDateQuotation.Text = "";


            }



            if (txtQuotationNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "RPQ_No ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }



            }
            if (txtSupplierNameQuotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy:SupplierWise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";
                //}

            }


            if (txtInquiryInQuotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Inquiry_No='" + txtInquiryInQuotation.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }







            if (DtFilter.Rows.Count > 0)
            {
                double TotalAmount = 0;
                //DataTable DtQuotation = DtFilter.DefaultView.ToTable(true, "TotalAmount", "Trans_Id");

                //for (int i = 0; i < DtQuotation.Rows.Count; i++)
                //{
                //    if (DtQuotation.Rows[i]["TotalAmount"].ToString() != "")
                //    {
                //        TotalAmount += float.Parse(DtQuotation.Rows[i]["TotalAmount"].ToString());
                //    }
                //}
                Session["TotalAmount"] = TotalAmount.ToString("0.0");
                Session["DtFilter"] = DtFilter;

                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PQuotationHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }

            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }
        if (ddlQuotationReportType.SelectedIndex == 1)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseQuoteDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseQuoteDetail_SelectRow_Report;

            if (DdlQuotationGroupBy.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Detail_Report_By_Quotation_Date;
                Session["ReportType"] = "1";
            }
            if (DdlQuotationGroupBy.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Detail_Report_By_Quotation_No;
                Session["ReportType"] = "0";
            }
            if (DdlQuotationGroupBy.SelectedIndex == 2)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Quotation_Detail_Report_By_Supplier;
                Session["ReportType"] = "2";
            }







            if (txtFromDateQuotation.Text != "" && txtToDateQuotation.Text != "")
            {
                DateTime ToDate = new DateTime();
                try
                {
                    ToDate = Convert.ToDateTime(txtToDateQuotation.Text);
                    DateTime FromDate = Convert.ToDateTime(txtFromDateQuotation.Text);
                }
                catch
                {
                    DisplayMessage("Invalid Date");
                    txtFromDateQuotation.Text = "";
                    txtToDateQuotation.Text = "";
                    txtFromDateQuotation.Focus();
                    return;
                }
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateQuotation.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
                Session["Parameter"] = Resources.Attendance.From + ": " + txtFromDateQuotation.Text + ' ' + Resources.Attendance.To + ": " + txtToDateQuotation.Text;

            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateQuotation.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateQuotation.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateQuotation.Text + "  To: " + txtToDateQuotation.Text;

                txtFromDateQuotation.Text = "";
                txtToDateQuotation.Text = "";


            }


            if (txtQuotationNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "RPQ_No ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }


            if (txtSupplierNameQuotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Supplier_Id =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }
            if (txtInquiryInQuotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Inquiry_No='" + txtInquiryInQuotation.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }









            if (txtProductNameQuotation.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }



            }

            if (DtFilter.Rows.Count > 0)
            {

                Session["DtFilter"] = DtFilter;
                double TotalAmount = 0;
                DataTable DtQuotation = DtFilter.DefaultView.ToTable(true, "TotalAmount", "Trans_Id");

                for (int i = 0; i < DtQuotation.Rows.Count; i++)
                {
                    if (DtQuotation.Rows[i]["TotalAmount"].ToString() != "")
                    {
                        TotalAmount += float.Parse(DtQuotation.Rows[i]["TotalAmount"].ToString());
                    }
                }

                Session["TotalAmount"] = TotalAmount.ToString("0.0");
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PQuotationDetailReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

                }
                catch
                {
                }


            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }





    }
    protected void BtnResetQuotationReport_Click(object sender, EventArgs e)
    {
        ddlQuotationReportType.SelectedIndex = 0;
        ddlQuotationReportType_SelectedIndexChanged(null, null);
        txtFromDateQuotation.Text = "";
        txtToDateQuotation.Text = "";
        txtProductNameQuotation.Text = "";
        txtInquiryInQuotation.Text = "";
        txtSupplierNameQuotation.Text = "";
        txtQuotationNo.Text = "";
        hdnSupplierId.Value = "";
        hdnProductId.Value = "";




    }
    #endregion
    #region Purchase_Order
    public void BindddlGroupOrder()
    {
        try
        {
            ddlGroupByOrder.Items.Clear();
        }
        catch
        {
        }
        ddlGroupByOrder.Items.Insert(0, Resources.Attendance.Order_Date);
        ddlGroupByOrder.Items.Insert(1, Resources.Attendance.Supplier);

        if (ddlOrderReportType.SelectedIndex == 1)
        {
            ddlGroupByOrder.Items.Insert(2, Resources.Attendance.Order_No_);
        }

    }

    protected void ddlOrderReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroupOrder();
        if (ddlOrderReportType.SelectedValue == "0")
        {
            lblProductNameOrder.Visible = false;
            lblColonOrder.Visible = false;
            txtProductNameOrder.Visible = false;


        }
        else
        {

            lblProductNameOrder.Visible = true;
            lblColonOrder.Visible = true;
            txtProductNameOrder.Visible = true;
        }
    }

    protected void txtOrderNo_TextChanged(object sender, EventArgs e)
    {
        if (txtOrderNo.Text != "")
        {
            DataTable Dt = ObjPurchaseOrder.GetPurchaseOrderTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (Dt.Rows.Count > 0)
            {
                try
                {
                    Dt = new DataView(Dt, "PoNO='" + txtOrderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Dt.Rows.Count > 0)
                {
                    hdnOrderNo.Value = Dt.Rows[0]["TransId"].ToString();
                }
                else
                {


                    DisplayMessage("Select Purchase Order No. in suggestion Only");
                    txtOrderNo.Text = "";
                    txtOrderNo.Focus();

                    return;

                }
            }
        }

    }


    protected void txtSupplierNameOrder_TextChanged(object sender, EventArgs e)
    {

        if (txtSupplierNameOrder.Text != "")
        {

            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierNameOrder.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnSupplierId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select supplier in suggestion only");
                txtSupplierNameOrder.Text = "";
                txtSupplierNameOrder.Focus();
                return;
            }


        }


    }
    protected void txtProductNameOrder_TextChanged(object sender, EventArgs e)
    {
        if (txtProductNameOrder.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductNameOrder.Text);
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductNameOrder.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductNameOrder.Text = "";
                txtProductNameOrder.Focus();
                return;
            }
        }


    }

    protected void txtQuotationNoOrder_TextChanged(object sender, EventArgs e)
    {
        if (txtQuotationNoOrder.Text != "")
        {
            DataTable Dt = objPQuotHeader.GetQuoteHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                Dt = new DataView(Dt, "RPQ_No='" + txtQuotationNoOrder.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (Dt.Rows.Count > 0)
            {
                hdnQuotationId.Value = Dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("Select Quotation No. in suggestion Only");
                txtQuotationNo.Text = "";
                txtQuotationNo.Focus();

                return;
            }
        }

    }




    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListOrderNo(string prefixText, int count, string contextKey)
    {
        PurchaseOrderHeader objPOHeader = new PurchaseOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = objPOHeader.GetPurchaseOrderTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["PoNo"].ToString();
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
                dt = objPOHeader.GetPurchaseOrderTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["PoNo"].ToString();
                    }
                }
            }
        }
        return str;
    }

    protected void BtnOrderReport_Click(object sender, EventArgs e)
    {

        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        Session["ReportType"] = null;


        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (ddlOrderReportType.SelectedIndex == 0)
        {

            DataTable Dt = new DataTable();

            Dt.Columns.Add("PurchaseOrderId");
            Dt.Columns.Add("PurchaseOrderNo");
            Dt.Columns.Add("SupplierName");
            Dt.Columns.Add("PurchaseOrderDate");
            Dt.Columns.Add("DelieveryDate");
            Dt.Columns.Add("Refference");
            Dt.Columns.Add("Total_Amount");
            Dt.Columns.Add("Currency");
            Dt.Columns.Add("ShippingDate");
            Dt.Columns.Add("PaymentMode");
            Dt.Columns.Add("TotalWeight");
            Dt.Columns.Add("Remark");
            Dt.Columns.Add("ShippingLine");
            Dt.Columns.Add("ShipBy");
            Dt.Columns.Add("ShipmentType");
            Dt.Columns.Add("Freight_Status");
            Dt.Columns.Add("ShipUnit");
            Dt.Columns.Add("UnitRate");
            Dt.Columns.Add("DateReceiving");
            Dt.Columns.Add("PartialShipment");
            Dt.Columns.Add("Condition_1");
            Dt.Columns.Add("Condition_2");
            Dt.Columns.Add("Condition_3");
            Dt.Columns.Add("Condition_4");
            Dt.Columns.Add("Condition_5");
            Dt.Columns.Add("RefType");



            DataTable dtPoheader = ObjPurchaseOrder.GetPurchaseOrderTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());



            if (ddlGroupByOrder.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Order_Header_Report_By_Order_Date;
                Session["ReportType"] = "1";
            }
            else
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Order_Header_Report_By_Supplier;
                Session["ReportType"] = "0";
            }



            if (txtFromDateOrder.Text != "" && txtTOdateOrder.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtTOdateOrder.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    dtPoheader = new DataView(dtPoheader, "CreatedDate>='" + txtFromDateOrder.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtTOdateOrder.Text;

            }
            else
            {

                DataTable DtDate = dtPoheader.Copy();
                DtDate = new DataView(DtDate, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (DtDate.Rows.Count > 0)
                {

                    txtFromDateOrder.Text = Convert.ToDateTime(DtDate.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtTOdateOrder.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtTOdateOrder.Text;
                txtFromDateOrder.Text = "";
                txtTOdateOrder.Text = "";

            }
            if (txtOrderNo.Text != "")
            {
                try
                {
                    dtPoheader = new DataView(dtPoheader, "TransID=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }


            }
            if (txtSupplierNameOrder.Text != "")
            {
                try
                {
                    dtPoheader = new DataView(dtPoheader, "SupplierId=" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }


            }
            if (txtQuotationNoOrder.Text != "")
            {
                try
                {
                    dtPoheader = new DataView(dtPoheader, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }


            }
            for (int i = 0; i < dtPoheader.Rows.Count; i++)
            {
                DataRow dr = Dt.NewRow();
                dr["PurchaseOrderId"] = dtPoheader.Rows[i]["TransId"].ToString();
                dr["PurchaseOrderNo"] = dtPoheader.Rows[i]["PoNo"].ToString();
                string SupplierId = dtPoheader.Rows[i]["SupplierId"].ToString();
                try
                {
                    DataTable Dtsupplier = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), SupplierId);
                    dr["SupplierName"] = Dtsupplier.Rows[0]["Name"].ToString();
                }
                catch
                {
                    dr["SupplierName"] = "";
                }
                dr["PurchaseOrderDate"] = dtPoheader.Rows[i]["PoDate"].ToString();
                dr["DelieveryDate"] = dtPoheader.Rows[i]["DeliveryDate"].ToString();
                dr["ShippingDate"] = dtPoheader.Rows[i]["DateShipping"].ToString();
                string RefferenceType = string.Empty;
                string QuotationNo = string.Empty;
                string SalesOrderNo = string.Empty;
                string CurrencyId = dtPoheader.Rows[i]["CurrencyID"].ToString();
                DataTable DtCurrency = objCurrency.GetCurrencyMasterById(CurrencyId);
                if (DtCurrency.Rows.Count > 0)
                {
                    dr["Currency"] = DtCurrency.Rows[0]["Currency_Code"].ToString();
                }

                Double TotalAmount = 0;

                RefferenceType = dtPoheader.Rows[i]["ReferenceVoucherType"].ToString();
                DataTable DtQuotation = objPQuotHeader.GetQuoteHeaderAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPoheader.Rows[i]["ReferenceID"].ToString());
                if (DtQuotation.Rows.Count > 0)
                {
                    QuotationNo = "PQ/" + DtQuotation.Rows[0]["RPQ_No"].ToString();
                }
                DataTable Podetail = new DataTable();
                Podetail = ObjPoDetail.GetPurchaseOrderDetailbyPOId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPoheader.Rows[i]["TransId"].ToString());

                for (int j = 0; j < Podetail.Rows.Count; j++)
                {
                    TotalAmount += (float.Parse(Podetail.Rows[j]["OrderQty"].ToString()) * float.Parse(Podetail.Rows[j]["UnitCost"].ToString()));
                    if (RefferenceType != "")
                    {
                        DataTable DtQuotationDetail = objPqDetail.GetQuoteDetailByRPQ_No(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPoheader.Rows[i]["ReferenceID"].ToString());
                        try
                        {
                            DtQuotationDetail = new DataView(DtQuotationDetail, "Product_Id=" + Podetail.Rows[j]["Product_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }

                        string TaxValue = string.Empty;
                        string DiscountValue = string.Empty;
                        try
                        {
                            TaxValue = DtQuotationDetail.Rows[0]["TaxValue"].ToString();
                            DiscountValue = DtQuotationDetail.Rows[0]["DiscountValue"].ToString();
                        }
                        catch
                        {
                        }
                        if (TaxValue == "")
                        {
                            TaxValue = "0";
                        }
                        if (DiscountValue == "")
                        {
                            DiscountValue = "0";
                        }

                        TotalAmount = TotalAmount + float.Parse(TaxValue) - float.Parse(DiscountValue);
                    }


                }
                dr["Total_Amount"] = Math.Round(TotalAmount, 3).ToString();

                if (RefferenceType == "PQ")
                {
                    dr["Refference"] = QuotationNo;


                }
                if (RefferenceType == "SO")
                {

                    DataTable DtsalesOrder = objSalesOrder.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPoheader.Rows[i]["SalesOrderID"].ToString());
                    if (DtsalesOrder.Rows.Count > 0)
                    {
                        SalesOrderNo = DtsalesOrder.Rows[0]["SalesOrderNo"].ToString();
                    }
                    if (SalesOrderNo == "")
                    {
                        dr["Refference"] = QuotationNo;
                    }
                    else
                    {
                        if (QuotationNo != "")
                        {
                            dr["Refference"] = QuotationNo + ", SO/" + SalesOrderNo;
                        }
                        else
                        {
                            dr["Refference"] = "SO/" + SalesOrderNo;
                        }
                    }


                }
                if (RefferenceType.Trim() == "")
                {
                    dr["Refference"] = "Direct";
                }


                string PaymentModeId = string.Empty;

                PaymentModeId = dtPoheader.Rows[i]["PaymentModeID"].ToString();

                DataTable DtPaymentMode = objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), PaymentModeId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                try
                {
                    dr["PaymentMode"] = DtPaymentMode.Rows[0]["Pay_Mod_Name"].ToString();
                }
                catch
                {
                    dr["PaymentMode"] = "";
                }
                dr["TotalWeight"] = dtPoheader.Rows[i]["TotalWeight"].ToString();
                dr["Remark"] = dtPoheader.Rows[i]["Remark"].ToString();
                string ShipingLine = "";
                ShipingLine = dtPoheader.Rows[i]["ShippingLine"].ToString();
                try
                {
                    DataTable Dtsupplier = objSupplier.GetSupplierAllDataBySupplierId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ShipingLine);
                    dr["ShippingLine"] = Dtsupplier.Rows[0]["Contact_Name"].ToString();
                }
                catch
                {
                    dr["ShippingLine"] = "";
                }
                dr["ShipBy"] = dtPoheader.Rows[i]["ShipBy"].ToString();
                dr["ShipmentType"] = dtPoheader.Rows[i]["ShipmentType"].ToString();
                dr["Freight_Status"] = dtPoheader.Rows[i]["Freight_Status"].ToString();
                string ShipUnit = dtPoheader.Rows[i]["ShipUnit"].ToString();
                DataTable DtUnit = objUnit.GetUnitMasterById(Session["CompId"].ToString(), ShipUnit);
                if (DtUnit.Rows.Count > 0)
                {
                    dr["ShipUnit"] = DtUnit.Rows[0]["Unit_Name"].ToString();
                }
                dr["UnitRate"] = dtPoheader.Rows[i]["UnitRate"].ToString();
                dr["DateReceiving"] = dtPoheader.Rows[i]["DateReceiving"].ToString();
                dr["PartialShipment"] = dtPoheader.Rows[i]["PartialShipment"].ToString();
                dr["Condition_1"] = dtPoheader.Rows[i]["Condition_1"].ToString();
                dr["Condition_2"] = dtPoheader.Rows[i]["Condition_2"].ToString();
                dr["Condition_3"] = dtPoheader.Rows[i]["Condition_3"].ToString();
                dr["Condition_4"] = dtPoheader.Rows[i]["Condition_4"].ToString();
                dr["Condition_5"] = dtPoheader.Rows[i]["Condition_5"].ToString();

                if (dtPoheader.Rows[i]["ReferenceVoucherType"].ToString().Trim() == "PQ")
                {
                    dr["RefType"] = "By Purchase Quotation";
                }

                if (dtPoheader.Rows[i]["ReferenceVoucherType"].ToString().Trim() == "SO")
                {
                    dr["RefType"] = "By Sales Order";
                }
                if (dtPoheader.Rows[i]["ReferenceVoucherType"].ToString().Trim() == "")
                {
                    dr["RefType"] = "By Direct";
                }

                Dt.Rows.Add(dr);


            }
            if (Dt.Rows.Count > 0)
            {
                Session["DtFilter"] = Dt;
                //if (txtOrderNo.Text != "")
                //{
                //    Response.Redirect("../Purchase_Report/PoHeaderReportByOrderNumber.aspx");


                //}
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/Purchase_Order_Report.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }



            }
            else
            {
                DisplayMessage("Record Not Found");
                txtFromDate.Focus();
                return;
            }
        }
        if (ddlOrderReportType.SelectedIndex == 1)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report;
            if (ddlGroupByOrder.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Order_Detail_Report_By_Order_Date;
                Session["ReportType"] = "2";
            }
            if (ddlGroupByOrder.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Order_Detail_Report_By_Supplier;
                Session["ReportType"] = "1";

            }
            if (ddlGroupByOrder.SelectedIndex == 2)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Order_Detail_Report_By_Order_No_;
                Session["ReportType"] = "0";
            }



            if (txtFromDateOrder.Text != "" && txtTOdateOrder.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtTOdateOrder.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateOrder.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtTOdateOrder.Text;



            }
            else
            {


                DtFilter = new DataView(DtFilter, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (DtFilter.Rows.Count > 0)
                {

                    txtFromDateOrder.Text = Convert.ToDateTime(DtFilter.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtTOdateOrder.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtTOdateOrder.Text;
                txtFromDateOrder.Text = "";
                txtTOdateOrder.Text = "";
            }
            if (txtOrderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "TransId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }
            if (txtSupplierNameOrder.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SupplierId =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (chkSupplier.Checked == false)
                //{
                //    if (Session["Parameter"] == null)
                //    {
                //        Session["Parameter"] = "FilterBy:SupplierWise";
                //    }
                //    else
                //    {
                //        Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";

                //    }
                //}

            }
            if (txtQuotationNoOrder.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy:QuotationNo.Wise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";

                //}

            }








            if (txtProductNameOrder.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy:ProductWise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";

                //}


            }

            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;


                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/POrderDetailReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

                }
                catch
                {
                }



            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }


        //if (rbtnOrderStatus.Checked == true)
        //{
        //    PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter();


        //    adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
        //    DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report;






        //    if (txtFromDate.Text != "" && txtToDate.Text != "")
        //    {
        //        DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
        //        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
        //        try
        //        {
        //            DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //        Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


        //    }
        //    else
        //    {

        //        DataTable Dt = DtFilter.Copy();
        //        Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
        //        if (Dt.Rows.Count > 0)
        //        {

        //            txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

        //        }
        //        txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

        //        Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


        //    }
        //    if (txtOrderNo.Text != "")
        //    {

        //        try
        //        {
        //            DtFilter = new DataView(DtFilter, "TransId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //        if (Session["Parameter"] == null)
        //        {
        //            Session["Parameter"] = "Filter By:PurchaseOrderNo.Wise";
        //        }
        //        else
        //        {
        //            Session["Parameter"] = Session["Parameter"].ToString() + " , PurchaseOrderNo.Wise";
        //        }

        //    }
        //    if (txtSupplierName.Text != "")
        //    {

        //        try
        //        {
        //            DtFilter = new DataView(DtFilter, "SupplierId =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //        if (Session["Parameter"] == null)
        //        {
        //            Session["Parameter"] = "Filter By:SupplierWise";
        //        }
        //        else
        //        {
        //            Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";
        //        }

        //    }
        //    if (txtQuotationNo.Text != "")
        //    {
        //        try
        //        {
        //            DtFilter = new DataView(DtFilter, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        //        }

        //        catch
        //        {
        //        }
        //        if (Session["Parameter"] == null)
        //        {
        //            Session["Parameter"] = "Filter By:QuotationNo.Wise";
        //        }
        //        else
        //        {
        //            Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";
        //        }

        //    }








        //    if (txtProductName.Text != "")
        //    {
        //        try
        //        {
        //            DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //        if (Session["Parameter"] == null)
        //        {
        //            Session["Parameter"] = "Filter By:ProductWise";
        //        }
        //        else
        //        {
        //            Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";
        //        }


        //    }

        //    if (DtFilter.Rows.Count > 0)
        //    {
        //        DtFilter = new DataView(DtFilter, "Pendingqty>0", "", DataViewRowState.CurrentRows).ToTable();
        //        Session["DtFilter"] = DtFilter;

        //        Response.Redirect("../Purchase_Report/POrderStatusReport.aspx");


        //    }
        //    else
        //    {
        //        DisplayMessage("Record Not Found");
        //        RbtnDetail.Focus();
        //        return;
        //    }
        //}





    }
    protected void BtnResetOrderReport_Click(object sender, EventArgs e)
    {
        ddlOrderReportType.SelectedIndex = 0;
        ddlOrderReportType_SelectedIndexChanged(null, null);
        txtFromDateOrder.Text = "";
        txtTOdateOrder.Text = "";
        txtProductNameOrder.Text = "";
        txtQuotationNoOrder.Text = "";
        txtSupplierNameOrder.Text = "";
        hdnSupplierId.Value = "";
        hdnProductId.Value = "";
    }


    #endregion

    #region Purchase_Invoice

    public void BindddlGroupInvoice()
    {
        try
        {
            ddlGroupByInvoice.Items.Clear();
        }
        catch
        {
        }
        ddlGroupByInvoice.Items.Insert(0, Resources.Attendance.Invoice_Date);
        ddlGroupByInvoice.Items.Insert(1, Resources.Attendance.Supplier);

        if (ddlInvoiceReportType.SelectedIndex == 1)
        {
            ddlGroupByInvoice.Items.Insert(2, Resources.Attendance.Invoice_No);
        }

    }

    protected void ddlInvoiceReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroupInvoice();
        if (ddlInvoiceReportType.SelectedValue == "0")
        {
            lblProductNameInvoice.Visible = false;
            lblColonInvoice.Visible = false;
            txtProductNameInvoice.Visible = false;


        }
        else
        {

            lblProductNameInvoice.Visible = true;
            lblColonInvoice.Visible = true;
            txtProductNameInvoice.Visible = true;
        }
    }



    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text != "")
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

    }
    protected void txtOrderNoInvoice_TextChanged(object sender, EventArgs e)
    {
        if (txtOrderNoInvoice.Text != "")
        {
            DataTable Dt = ObjPurchaseOrder.GetPurchaseOrderTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (Dt.Rows.Count > 0)
            {
                try
                {
                    Dt = new DataView(Dt, "PoNO='" + txtOrderNoInvoice.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Dt.Rows.Count > 0)
                {
                    hdnOrderNo.Value = Dt.Rows[0]["TransId"].ToString();
                }
                else
                {


                    DisplayMessage("Select Purchase Order No. in suggestion Only");
                    txtOrderNoInvoice.Text = "";
                    txtOrderNoInvoice.Focus();

                    return;

                }
            }
        }

    }


    protected void txtSupplierNameInvoice_TextChanged(object sender, EventArgs e)
    {

        if (txtSupplierNameInvoice.Text != "")
        {

            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierNameInvoice.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnSupplierId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select supplier in suggestion only");
                txtSupplierNameInvoice.Text = "";
                txtSupplierNameInvoice.Focus();
                return;
            }


        }


    }
    protected void txtProductNameInvoice_TextChanged(object sender, EventArgs e)
    {
        if (txtProductNameInvoice.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductNameInvoice.Text);
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductNameInvoice.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductNameInvoice.Text = "";
                txtProductNameInvoice.Focus();
                return;
            }
        }


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

    protected void BtnInvoiceReport_Click(object sender, EventArgs e)
    {

        Session["Parameter"] = null;
        Session["DtFilter"] = null;

        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (ddlInvoiceReportType.SelectedIndex == 0)
        {

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report;

            if (ddlGroupByInvoice.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Invoice_Header_Report_By_Invoice_Date;
                Session["ReportType"] = "2";
            }
            else
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Invoice_Header_Report_By_Supplier;
                Session["ReportType"] = "1";
            }



            if (txtFromDateInvoice.Text != "" && txtToDateInvoice.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateInvoice.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateInvoice.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;
            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateInvoice.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateInvoice.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;
                txtFromDateInvoice.Text = "";
                txtToDateInvoice.Text = "";

            }



            if (txtInvoiceNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "TransID=" + hdnInvoiceNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }

            }
            if (txtSupInvoiceNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SupInvoiceNo='" + txtSupInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //if (Session["ReportHeader"] == null)
                //{
                //    Session["ReportHeader"] = "Purchase Invoice Header Report By Supplier Invoice Number";
                //}
                //else
                //{
                //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier Invoice Number";
                //}

            }


            if (txtSupplierNameInvoice.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SupplierId='" + hdnSupplierId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //if (chkSupplier.Checked == false)
                //{
                //    if (Session["ReportHeader"] == null)
                //    {
                //        Session["ReportHeader"] = "Purchase Invoice Header Report By Supplier";
                //    }
                //    else
                //    {
                //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
                //    }
                //}

            }
            if (txtOrderNoInvoice.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "PoId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //if (Session["ReportHeader"] == null)
                //{
                //    Session["ReportHeader"] = "Purchase Invoice Header Report By Order Number";
                //}
                //else
                //{
                //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Order Number";
                //}
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

                DataTable Dt = new DataTable();

                Dt.Columns.Add("TransId");
                Dt.Columns.Add("InvoiceNo");
                Dt.Columns.Add("InvoiceDate");
                Dt.Columns.Add("SupInvoiceNo");
                Dt.Columns.Add("SupInvoiceDate");
                Dt.Columns.Add("RefType");
                Dt.Columns.Add("RefNo");
                Dt.Columns.Add("Currency_Name");
                Dt.Columns.Add("PaymentModeName");
                Dt.Columns.Add("Remark");
                Dt.Columns.Add("GrandTotal");
                Dt.Columns.Add("SupplierId");
                Dt.Columns.Add("Supplier_Name");
                Dt.Columns.Add("Field1");
                Dt.Columns.Add("Field2");

                DataTable DtFilter_Copy = DtFilter.Copy();
                DtFilter_Copy = DtFilter_Copy.DefaultView.ToTable(true, "TransId", "InvoiceNo", "InvoiceDate", "SupInvoiceNo", "SupInvoiceDate", "RefType", "Currency_Name", "PaymentModeName", "Remark", "GrandTotal", "SupplierId", "Supplier_Name");
                for (int i = 0; i < DtFilter_Copy.Rows.Count; i++)
                {
                    DataRow dr = Dt.NewRow();
                    dr["TransId"] = DtFilter_Copy.Rows[i]["TransId"].ToString();
                    dr["InvoiceNo"] = DtFilter_Copy.Rows[i]["InvoiceNo"].ToString();
                    dr["InvoiceDate"] = Convert.ToDateTime(DtFilter_Copy.Rows[i]["InvoiceDate"]).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    dr["SupInvoiceNo"] = DtFilter_Copy.Rows[i]["SupInvoiceNo"].ToString();
                    dr["SupInvoiceDate"] = Convert.ToDateTime(DtFilter_Copy.Rows[i]["SupInvoiceDate"]).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    dr["RefType"] = DtFilter_Copy.Rows[i]["RefType"].ToString();
                    dr["Currency_Name"] = DtFilter_Copy.Rows[i]["Currency_Name"].ToString();
                    dr["PaymentModeName"] = DtFilter_Copy.Rows[i]["PaymentModeName"].ToString();
                    dr["Remark"] = DtFilter_Copy.Rows[i]["Remark"].ToString();
                    dr["GrandTotal"] = DtFilter_Copy.Rows[i]["GrandTotal"].ToString();
                    dr["SupplierId"] = DtFilter_Copy.Rows[i]["SupplierId"].ToString();
                    dr["Supplier_Name"] = DtFilter_Copy.Rows[i]["Supplier_Name"].ToString();
                    dr["Field1"] = "";
                    dr["Field2"] = "";
                    string RefNo = string.Empty;
                    DataTable DtRefNo = new DataTable();
                    DtRefNo = DtFilter.Copy();
                    try
                    {
                        DtRefNo = new DataView(DtRefNo, "Field1='PO' and TransId=" + DtFilter_Copy.Rows[i]["TransId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable(true, "RefNo");
                    }

                    catch
                    {

                    }
                    for (int j = 0; j < DtRefNo.Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            RefNo = DtRefNo.Rows[j]["RefNo"].ToString();
                        }
                        else
                        {
                            RefNo = RefNo + " , " + DtRefNo.Rows[j]["RefNo"].ToString();
                        }
                    }
                    dr["RefNo"] = RefNo;
                    Dt.Rows.Add(dr);
                }



                Session["DtFilter"] = Dt;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PurchaseInvoiceHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

                }
                catch
                {
                }



            }
            else
            {
                DisplayMessage("Record Not Found");

                return;
            }
        }
        if (ddlInvoiceReportType.SelectedIndex == 1)
        {

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report;

            if (ddlGroupByInvoice.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Invoice_Detail_Report_By_Invoice_Date;
                Session["ReportType"] = "0";

            }
            if (ddlGroupByInvoice.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Invoice_Detail_Report_By_Supplier;
                Session["ReportType"] = "1";

            }
            if (ddlGroupByInvoice.SelectedIndex == 2)
            {
                Session["ReportHeader"] = Resources.Attendance.Purchase_Invoice_Detail_Report_By_Invoice_No;
                Session["ReportType"] = "2";
            }




            if (txtFromDateInvoice.Text != "" && txtToDateInvoice.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateInvoice.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateInvoice.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;
            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateInvoice.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateInvoice.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;
                txtFromDateInvoice.Text = "";
                txtToDateInvoice.Text = "";

            }



            if (txtInvoiceNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "TransID=" + hdnInvoiceNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //if (ChkInvoiceno.Checked == false)
                //{
                //    if (Session["ReportHeader"] == null)
                //    {
                //        Session["ReportHeader"] = "Purchase Invoice Detail Report By Invoice No.";
                //    }
                //    else
                //    {
                //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice No.";
                //    }
                //}

            }
            if (txtSupInvoiceNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SupInvoiceNo='" + txtSupInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }

                //if (Session["ReportHeader"] == null)
                //{
                //    Session["ReportHeader"] = "Purchase Invoice Detail Report By Supplier Invoice No.";
                //}
                //else
                //{
                //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier Invoice No.";
                //}
            }


            if (txtSupplierNameInvoice.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SupplierId='" + hdnSupplierId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }

            }
            if (txtOrderNoInvoice.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "PoId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //if (Session["ReportHeader"] == null)
                //{
                //    Session["ReportHeader"] = "Purchase Invoice Detail Report By Order No.";
                //}
                //else
                //{
                //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " ,Order No.";
                //}

            }
            //if (ddlPostStatus.SelectedIndex != 0)
            //{
            //    try
            //    {
            //        DtFilter = new DataView(DtFilter, "Post='" + ddlPostStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    }

            //    catch
            //    {
            //    }
            //    string Status = string.Empty;
            //    if (Session["ReportHeader"] == null)
            //    {
            //        Session["ReportHeader"] = "Purchase Invoice Detail Report By Post(" + ddlPostStatus.SelectedItem.Text + ")";
            //    }
            //    else
            //    {
            //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Post(" + ddlPostStatus.SelectedItem.Text + ")";
            //    }
            //}
            if (txtProductNameInvoice.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "ProductId=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                //    if (Session["ReportHeader"] == null)
                //    {
                //        Session["ReportHeader"] = "Purchase Invoice Detail Report By Product";
                //    }
                //    else
                //    {
                //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Product";
                //    }
            }
            //}
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

            //if (DtFilter.Rows.Count > 0)
            //{

            //    DataTable Dt = new DataTable();

            //    Dt.Columns.Add("TransId");
            //    Dt.Columns.Add("InvoiceNo");
            //    Dt.Columns.Add("InvoiceDate");
            //    Dt.Columns.Add("SupInvoiceNo");
            //    Dt.Columns.Add("SupInvoiceDate");
            //    Dt.Columns.Add("RefType");
            //    Dt.Columns.Add("RefNo");
            //    Dt.Columns.Add("Currency_Name");
            //    Dt.Columns.Add("PaymentModeName");
            //    Dt.Columns.Add("Remark");
            //    Dt.Columns.Add("GrandTotal");
            //    Dt.Columns.Add("SupplierId");
            //    Dt.Columns.Add("Supplier_Name");
            //    Dt.Columns.Add("Field1");
            //    Dt.Columns.Add("Field2");
            //    Dt.Columns.Add("ProductId");
            //    Dt.Columns.Add("Product_Name");
            //    Dt.Columns.Add("Unit_Name");
            //    Dt.Columns.Add("unitcost");
            //    Dt.Columns.Add("orderqty");
            //    Dt.Columns.Add("Invoiceqty");
            //    Dt.Columns.Add("Amount");
            //    Dt.Columns.Add("Tax");
            //    Dt.Columns.Add("Discount");
            //    Dt.Columns.Add("PriceAfterDiscount");
            //    Dt.Columns.Add("NetTaxValue");
            //    Dt.Columns.Add("NetDiscountValue");

            //    for (int i = 0; i < DtFilter.Rows.Count; i++)
            //    {
            //        DataRow dr = Dt.NewRow();
            //        dr["TransId"] = DtFilter.Rows[i]["TransId"].ToString();
            //        dr["InvoiceNo"] = DtFilter.Rows[i]["InvoiceNo"].ToString();
            //        dr["InvoiceDate"] = Convert.ToDateTime(DtFilter.Rows[i]["InvoiceDate"]).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            //        dr["SupInvoiceNo"] = DtFilter.Rows[i]["SupInvoiceNo"].ToString();
            //        dr["SupInvoiceDate"] = Convert.ToDateTime(DtFilter.Rows[i]["SupInvoiceDate"]).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            //        dr["RefType"] = DtFilter.Rows[i]["RefType"].ToString();
            //        dr["Currency_Name"] = DtFilter.Rows[i]["Currency_Name"].ToString();
            //        dr["PaymentModeName"] = DtFilter.Rows[i]["PaymentModeName"].ToString();
            //        dr["Remark"] = DtFilter.Rows[i]["Remark"].ToString();
            //        dr["GrandTotal"] = DtFilter.Rows[i]["GrandTotal"].ToString();
            //        dr["SupplierId"] = DtFilter.Rows[i]["SupplierId"].ToString();
            //        dr["Supplier_Name"] = DtFilter.Rows[i]["Supplier_Name"].ToString();
            //        dr["Field1"] = DtFilter.Rows[i]["RefNo"].ToString();
            //        dr["Field2"] = "";
            //        string RefNo = string.Empty;
            //        DataTable DtRefNo = new DataTable();
            //        DtRefNo = DtFilter.Copy();
            //        try
            //        {
            //            DtRefNo = new DataView(DtRefNo, "Field1='PO' and TransId=" + DtFilter.Rows[i]["TransId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable(true, "RefNo");
            //        }

            //        catch
            //        {

            //        }
            //        for (int j = 0; j < DtRefNo.Rows.Count; j++)
            //        {
            //            if (j == 0)
            //            {
            //                RefNo = DtRefNo.Rows[j]["RefNo"].ToString();
            //            }
            //            else
            //            {
            //                RefNo = RefNo + " , " + DtRefNo.Rows[j]["RefNo"].ToString();
            //            }
            //        }
            //        dr["RefNo"] = RefNo;
            //        dr["ProductId"] = DtFilter.Rows[i]["ProductId"].ToString();
            //        dr["Product_Name"] = DtFilter.Rows[i]["Product_Name"].ToString();
            //        dr["Unit_Name"] = DtFilter.Rows[i]["Unit_Name"].ToString();
            //        dr["unitcost"] = DtFilter.Rows[i]["unitcost"].ToString();
            //        dr["orderqty"] = DtFilter.Rows[i]["orderqty"].ToString();
            //        dr["Invoiceqty"] = DtFilter.Rows[i]["InvoiceQty"].ToString();
            //        dr["Amount"] = DtFilter.Rows[i]["Amount"].ToString();
            //        dr["Tax"] = DtFilter.Rows[i]["TaxV"].ToString();
            //        dr["Discount"] = DtFilter.Rows[i]["DiscountV"].ToString();
            //        dr["PriceAfterDiscount"] = DtFilter.Rows[i]["PriceAfterDiscount"].ToString();
            //        dr["NetTaxValue"] = DtFilter.Rows[i]["NetTaxValue"].ToString();
            //        dr["NetDiscountValue"] = DtFilter.Rows[i]["NetDiscountValue"].ToString();
            //        Dt.Rows.Add(dr);
            //    }//

            if (DtFilter.Rows.Count > 0)
            {

                Session["DtFilter"] = DtFilter;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PurchaseInvoiceDetailReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

                }
                catch
                {
                }


            }
            else
            {
                DisplayMessage("Record Not Found");
                txtFromDate.Focus();
                return;
            }

        }








    }
    protected void BtnResetInvoiceReport_Click(object sender, EventArgs e)
    {

        ddlInvoiceReportType.SelectedIndex = 0;
        ddlInvoiceReportType_SelectedIndexChanged(null, null);
        txtFromDateInvoice.Text = "";
        txtToDateInvoice.Text = "";
        txtSupplierNameInvoice.Text = "";
        txtSupInvoiceNo.Text = "";
        txtOrderNoInvoice.Text = "";
        txtProductNameInvoice.Text = "";
        txtInvoiceNo.Text = "";



    }

    #endregion
    #region PurchaseGoods
    public void BindddlGroup_Goods()
    {
        try
        {
            DddlGroupByGoods.Items.Clear();
        }
        catch
        {
        }
        DddlGroupByGoods.Items.Insert(0, Resources.Attendance.Invoice_Date);
        DddlGroupByGoods.Items.Insert(1, Resources.Attendance.Invoice_No);
        DddlGroupByGoods.Items.Insert(2, Resources.Attendance.Order_No_);





    }

    protected void txtOrderNoGoods_TextChanged(object sender, EventArgs e)
    {
        if (txtOrderNoGoods.Text != "")
        {
            DataTable Dt = ObjPurchaseOrder.GetPurchaseOrderTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (Dt.Rows.Count > 0)
            {
                try
                {
                    Dt = new DataView(Dt, "PoNO='" + txtOrderNoGoods.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Dt.Rows.Count > 0)
                {
                    hdnOrderNo.Value = Dt.Rows[0]["TransId"].ToString();
                }
                else
                {


                    DisplayMessage("Select Purchase Order No. in suggestion Only");
                    txtOrderNoGoods.Text = "";
                    txtOrderNoGoods.Focus();

                    return;

                }
            }
        }

    }
    protected void txtProductNamegoods_TextChanged(object sender, EventArgs e)
    {
        if (txtProductNamegoods.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductNamegoods.Text);
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductNamegoods.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductNamegoods.Text = "";
                txtProductNamegoods.Focus();
                return;
            }
        }


    }
    protected void txtInvoiceNoGoods_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNoGoods.Text != "")
        {
            DataTable Dt = ObjPinvoiceHeader.GetPurchaseInvoiceTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                Dt = new DataView(Dt, "InvoiceNo='" + txtInvoiceNoGoods.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                txtInvoiceNoGoods.Text = "";
                txtInvoiceNoGoods.Focus();

                return;
            }
        }

    }
    protected void BtnGoodsReport_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;


        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;




        PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
        DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report;

        if (DddlGroupByGoods.SelectedIndex == 0)
        {
            Session["ReportHeader"] = Resources.Attendance.Purchase_Goods_Receive_Report_By_Invoice_Date;
            Session["ReportType"] = "0";

        }
        if (DddlGroupByGoods.SelectedIndex == 1)
        {
            Session["ReportHeader"] = Resources.Attendance.Purchase_Goods_Receive_Report_By_Invoice_No;
            Session["ReportType"] = "1";
        }
        if (DddlGroupByGoods.SelectedIndex == 2)
        {
            Session["ReportHeader"] = Resources.Attendance.Purchase_Goods_Receive_Report_By_Order_No;
            Session["ReportType"] = "2";

            try
            {
                DtFilter = new DataView(DtFilter, "PoId<>0", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
        }




        if (txtFromdateGoods.Text != "" && txttOdateGoods.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txttOdateGoods.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            try
            {
                DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromdateGoods.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
            Session["Parameter"] = "From: " + txtFromdateGoods.Text + "  To: " + txttOdateGoods.Text;
        }
        else
        {
            DataTable Dt = DtFilter.Copy();
            Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
            if (Dt.Rows.Count > 0)
            {

                txtFromdateGoods.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            }
            txttOdateGoods.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            Session["Parameter"] = "From: " + txtFromdateGoods.Text + "  To: " + txttOdateGoods.Text;
            txtFromdateGoods.Text = "";
            txttOdateGoods.Text = "";


        }



        if (txtInvoiceNoGoods.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "TransID=" + hdnInvoiceNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            //if (rbtnInvoiceNo.Checked == false)
            //{
            //    if (Session["ReportHeader"] == null)
            //    {
            //        Session["ReportHeader"] = "Purchase Goods Receive Report";
            //    }
            //    else
            //    {
            //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice No.";
            //    }
            //}

        }
        //if (txtSupInvoiceNo.Text != "")
        //{
        //    try
        //    {
        //        DtFilter = new DataView(DtFilter, "SupInvoiceNo='" + txtSupInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //    catch
        //    {
        //    }
        //    if (Session["Parameter"] == null)
        //    {
        //        Session["Parameter"] = "Filter By: SupplierInvoice No. Wise";
        //    }
        //    else
        //    {
        //        Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierInvoice No. Wise";
        //    }

        //}


        //if (txtSupplierName.Text != "")
        //{
        //    try
        //    {
        //        DtFilter = new DataView(DtFilter, "SupplierId='" + hdnSupplierId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //    catch
        //    {
        //    }
        //    if (Session["Parameter"] == null)
        //    {
        //        Session["Parameter"] = "Filter By: Supplier Wise";
        //    }
        //    else
        //    {
        //        Session["Parameter"] = Session["Parameter"].ToString() + " , Supplier Wise";
        //    }

        //}
        if (txtOrderNoGoods.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "PoId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            //if (rbtnOrderNo.Checked == false)
            //{
            //    if (Session["ReportHeader"] == null)
            //    {
            //        Session["ReportHeader"] = "Purchase Goods Receive Report";
            //    }
            //    else
            //    {
            //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Order No.";
            //    }
            //}
        }
        //if (ddlPostStatus.SelectedIndex != 0)
        //{
        //    try
        //    {
        //        DtFilter = new DataView(DtFilter, "Post='" + ddlPostStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //    catch
        //    {
        //    }
        //    string Status = string.Empty;
        //    if (Session["Parameter"] == null)
        //    {
        //        Session["Parameter"] = "Filter By: " + ddlPostStatus.SelectedItem.Text+" Wise";
        //    }
        //    else
        //    {
        //        Session["Parameter"] = Session["Parameter"].ToString() + " , " + ddlPostStatus.SelectedItem.Text+" Wise";
        //    }
        //}
        if (txtProductNamegoods.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "ProductId=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            //if (Session["ReportHeader"] == null)
            //{
            //    Session["ReportHeader"] = "Purchase Goods Receive Report";
            //}
            //else
            //{
            //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " ,Product Name";
            //}
        }


        try
        {
            DtFilter = new DataView(DtFilter, "RecQty<>'0'", "", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {
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

            DataTable Dt = new DataTable();

            Dt.Columns.Add("TransId");
            Dt.Columns.Add("InvoiceNo");
            Dt.Columns.Add("InvoiceDate");
            Dt.Columns.Add("SupInvoiceNo");
            Dt.Columns.Add("SupInvoiceDate");
            Dt.Columns.Add("RefType");
            Dt.Columns.Add("RefNo");
            Dt.Columns.Add("Currency_Name");
            Dt.Columns.Add("PaymentModeName");
            Dt.Columns.Add("Remark");
            Dt.Columns.Add("GrandTotal");
            Dt.Columns.Add("SupplierId");
            Dt.Columns.Add("Supplier_Name");
            Dt.Columns.Add("Field1");
            Dt.Columns.Add("Field2");
            Dt.Columns.Add("ProductId");
            Dt.Columns.Add("Product_Name");
            Dt.Columns.Add("Unit_Name");
            Dt.Columns.Add("unitcost");
            Dt.Columns.Add("orderqty");
            Dt.Columns.Add("Invoiceqty");
            Dt.Columns.Add("Amount");
            Dt.Columns.Add("Tax");
            Dt.Columns.Add("Discount");
            Dt.Columns.Add("PriceAfterDiscount");

            for (int i = 0; i < DtFilter.Rows.Count; i++)
            {
                DataRow dr = Dt.NewRow();
                dr["TransId"] = DtFilter.Rows[i]["TransId"].ToString();
                dr["InvoiceNo"] = DtFilter.Rows[i]["InvoiceNo"].ToString();
                dr["InvoiceDate"] = Convert.ToDateTime(DtFilter.Rows[i]["InvoiceDate"]).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                dr["SupInvoiceNo"] = DtFilter.Rows[i]["SupInvoiceNo"].ToString();
                dr["SupInvoiceDate"] = Convert.ToDateTime(DtFilter.Rows[i]["SupInvoiceDate"]).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                dr["RefType"] = DtFilter.Rows[i]["RefType"].ToString();
                dr["Currency_Name"] = DtFilter.Rows[i]["Currency_Name"].ToString();
                dr["PaymentModeName"] = DtFilter.Rows[i]["PaymentModeName"].ToString();
                dr["Remark"] = DtFilter.Rows[i]["Remark"].ToString();
                dr["GrandTotal"] = DtFilter.Rows[i]["GrandTotal"].ToString();
                dr["SupplierId"] = DtFilter.Rows[i]["SupplierId"].ToString();
                dr["Supplier_Name"] = DtFilter.Rows[i]["Supplier_Name"].ToString();
                dr["Field1"] = DtFilter.Rows[i]["RefNo"].ToString();
                dr["Field2"] = Math.Round(Convert.ToDouble(DtFilter.Rows[i]["RecQty"].ToString()), 0);
                string RefNo = string.Empty;
                DataTable DtRefNo = new DataTable();
                DtRefNo = DtFilter.Copy();
                try
                {
                    DtRefNo = new DataView(DtRefNo, "Field1='PO' and TransId=" + DtFilter.Rows[i]["TransId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable(true, "RefNo");
                }

                catch
                {

                }
                for (int j = 0; j < DtRefNo.Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        RefNo = DtRefNo.Rows[j]["RefNo"].ToString();
                    }
                    else
                    {
                        RefNo = RefNo + " , " + DtRefNo.Rows[j]["RefNo"].ToString();
                    }
                }
                dr["RefNo"] = RefNo;
                dr["ProductId"] = DtFilter.Rows[i]["ProductCode"].ToString();
                dr["Product_Name"] = DtFilter.Rows[i]["Product_Name"].ToString();
                dr["Unit_Name"] = DtFilter.Rows[i]["Unit_Name"].ToString();
                dr["unitcost"] = DtFilter.Rows[i]["unitcost"].ToString();
                dr["orderqty"] = Math.Round(Convert.ToDouble(DtFilter.Rows[i]["orderqty"].ToString()), 0);
                dr["Invoiceqty"] = Math.Round(Convert.ToDouble(DtFilter.Rows[i]["InvoiceQty"].ToString()), 0);
                dr["Amount"] = DtFilter.Rows[i]["Amount"].ToString();
                dr["Tax"] = DtFilter.Rows[i]["TaxV"].ToString();
                dr["Discount"] = DtFilter.Rows[i]["DiscountV"].ToString();
                dr["PriceAfterDiscount"] = DtFilter.Rows[i]["PriceAfterDiscount"].ToString();
                Dt.Rows.Add(dr);
            }



            Session["DtFilter"] = Dt;
            try
            {

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PurchaseGoodRec_Report.aspx','','height=650,width=900,scrollbars=Yes')", true);

            }
            catch
            {
            }


        }
        else
        {
            DisplayMessage("Record Not Found");
            //txtFromDate.Focus();
            return;
        }













    }
    protected void BtnResetGoodsReport_Click(object sender, EventArgs e)
    {
        DddlGroupByGoods.SelectedIndex = 0;
        txtFromdateGoods.Text = "";
        txttOdateGoods.Text = "";
        txtInvoiceNoGoods.Text = "";
        txtOrderNoGoods.Text = "";
        txtProductNamegoods.Text = "";
    }


    #endregion
    #region Purchase_Return
    public void BindddlGroup_Return()
    {
        try
        {
            ddlGroupByReturn.Items.Clear();
        }
        catch
        {
        }
        ddlGroupByReturn.Items.Insert(0, Resources.Attendance.Return_Date);
        ddlGroupByReturn.Items.Insert(1, Resources.Attendance.Return_No_);

    }
    protected void txtProductnameReturn_TextChanged(object sender, EventArgs e)
    {
        if (txtProductnameReturn.Text != "")
        {
            //DataTable dtProduct = objProductM.GetProductMasterTrueAll(StrCompId);
            //dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            //if (dtProduct.Rows.Count > 0)
            //{
            DataTable dtProduct = new DataTable();

            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductnameReturn.Text);
            }
            catch
            {
            }

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                txtProductnameReturn.Text = dtProduct.Rows[0]["EProductName"].ToString();
                hdnProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();





            }
            else
            {
                DisplayMessage("Select Product in suggestion only");
                txtProductnameReturn.Text = "";
                txtProductnameReturn.Focus();
                return;
            }
        }


    }
    protected void txtInvoiceNoReturn_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNoReturn.Text != "")
        {
            DataTable Dt = ObjPinvoiceHeader.GetPurchaseInvoiceTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                Dt = new DataView(Dt, "InvoiceNo='" + txtInvoiceNoReturn.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                txtInvoiceNoReturn.Text = "";
                txtInvoiceNoReturn.Focus();

                return;
            }
        }

    }
    protected void txtReturnNo_TextChanged(object sender, EventArgs e)
    {
        if (txtReturnNo.Text != "")
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

    protected void btnReturnReport_Click(object sender, EventArgs e)
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



        if (ddlGroupByReturn.SelectedIndex == 0)
        {
            Session["ReportHeader"] = Resources.Attendance.Purchase_Return_Report_By_Return_Date;
            Session["ReportType"] = "0";

        }
        else
        {
            Session["ReportHeader"] = Resources.Attendance.Purchase_Return_Report_By_Return_No;
            Session["ReportType"] = "1";
        }



        if (txtFromDateReturn.Text != "" && txtToDateReturn.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDateReturn.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            try
            {
                DtFilter = new DataView(DtFilter, "CreatedDate>='" + txtFromDateReturn.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
            Session["Parameter"] = "From: " + txtFromDateReturn.Text + "  To: " + txtToDateReturn.Text;
        }
        else
        {
            DataTable Dt = DtFilter.Copy();
            Dt = new DataView(Dt, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
            if (Dt.Rows.Count > 0)
            {

                txtFromDateReturn.Text = Convert.ToDateTime(Dt.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            }
            txtToDateReturn.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            Session["Parameter"] = "From: " + txtFromDateReturn.Text + "  To: " + txtToDateReturn.Text;
            txtFromDateReturn.Text = "";
            txtToDateReturn.Text = "";

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
            //if (chkReturnNo.Checked == false)
            //{
            //    if (Session["ReportHeader"] == null)
            //    {
            //        Session["ReportHeader"] = "Purchase Return Report by Return No.";
            //    }
            //    else
            //    {
            //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Return No.";
            //    }
            //}

        }


        if (txtInvoiceNoReturn.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Invoice_Id=" + hdnInvoiceNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            //if (Session["ReportHeader"] == null)
            //{
            //    Session["ReportHeader"] = "Purchase Return Report by Invoice No.";
            //}
            //else
            //{
            //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice No.";
            //}

        }



        //if (txtSupplierName.Text != "")
        //{
        //    try
        //    {
        //        DtFilter = new DataView(DtFilter, "SupplierId='" + hdnSupplierId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }

        //    catch
        //    {
        //    }
        //    if (Session["ReportHeader"] == null)
        //    {
        //        Session["ReportHeader"] = "Purchase Return Report by Supplier";
        //    }
        //    else
        //    {
        //        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
        //    }

        //}


        if (txtProductnameReturn.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }
            //if (Session["ReportHeader"] == null)
            //{
            //    Session["ReportHeader"] = "Purchase Return Report by Product";
            //}
            //else
            //{
            //    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Product";
            //}

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

            try
            {

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/Purchase_Return_Report.aspx','','height=650,width=900,scrollbars=Yes')", true);

            }
            catch
            {
            }


        }
        else
        {
            DisplayMessage("Record Not Found");
            //txtFromDate.Focus();
            return;
        }











    }
    protected void btnResetReturnReport_Click(object sender, EventArgs e)
    {
        ddlGroupByReturn.SelectedIndex = 0;
        txtFromDateReturn.Text = "";
        txtToDateReturn.Text = "";
        txtReturnNo.Text = "";
        txtInvoiceNoReturn.Text = "";
        txtProductnameReturn.Text = "";
    }
    #endregion
}
