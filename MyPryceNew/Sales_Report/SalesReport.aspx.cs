using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Sales_Report_SalesReport : BasePage
{
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_SalesInquiryHeader objSInquiryHeader = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    Inv_SalesQuotationHeader objSQuotationHeader = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_SalesInvoiceHeader objSinvoiceheader = null;
    EmployeeMaster objEmployee = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objSInquiryHeader = new Inv_SalesInquiryHeader(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        objSQuotationHeader = new Inv_SalesQuotationHeader(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objSinvoiceheader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            CalendarExtender3.Format = Session["DateFormat"].ToString();
            CalendarExtender4.Format = Session["DateFormat"].ToString();
            CalendarExtender5.Format = Session["DateFormat"].ToString();
            CalendarExtender8.Format = Session["DateFormat"].ToString();
            CalendarExtender9.Format = Session["DateFormat"].ToString();
            CalendarExtenderReturnfrom.Format = Session["DateFormat"].ToString();
            CalendarExtenderReturnto.Format = Session["DateFormat"].ToString();
           
            btnMenuInquiry_Click(null, null);

        }
        AllPageCode();
    }

    public void AllPageCode()
    {

        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "30";
        Session["HeaderText"] = "Sales Report";


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


   
    protected void btnMenuInquiry_Click(object sender, EventArgs e)
    {
        
        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlInquiry.Visible = true;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlReturn.Visible = false;
        ddlReportType_SelectedIndexChanged(null, null);
      

    }

    protected void btnMenuQuotation_Click(object sender, EventArgs e)
    {
        
        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
         pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlInquiry.Visible = false;
        PnlQuotation.Visible = true;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlReturn.Visible = false;

        ddlReportTypeQuotation_SelectedIndexChanged(null, null);


    }
    protected void btnMenuOrder_Click(object sender, EventArgs e)
    {
       
        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = true;
        PnlInvoice.Visible = false;
        PnlReturn.Visible = false;

        ddlReportTypeOrder_SelectedIndexChanged(null, null);

    }
    protected void btnMenuInvoice_Click(object sender, EventArgs e)
    {
        
        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = true;
        PnlReturn.Visible = false;
        ddlInvoiceReportType_SelectedIndexChanged(null, null);
       
    }
    
    protected void btnMenuReturn_Click(object sender, EventArgs e)
    {
    
        pnlMenuInquiry.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuQuotation.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuOrder.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuInvoice.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuReturn.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        PnlInquiry.Visible = false;
        PnlQuotation.Visible = false;
        PnlOrder.Visible = false;
        PnlInvoice.Visible = false;
        PnlReturn.Visible = true;
        BindddlGroupReturn();

    }

    #region SalesInquiry

    public void BindddlGroup()
    {
        try
        {
            ddlGroupBy.Items.Clear();
        }
        catch
        {
        }
           ddlGroupBy.Items.Insert(0, Resources.Attendance.Inquiry_Date);
           if (ddlReportType.SelectedIndex == 0)
           {
               ddlGroupBy.Items.Insert(1, Resources.Attendance.Customer_Name);
               ddlGroupBy.Items.Insert(2, Resources.Attendance.Sales_Person);
           }
           else
           {
               ddlGroupBy.Items.Insert(1, Resources.Attendance.Inquiry_No_);
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
            lblTender.Visible = true;
            lblTendercolon.Visible = true;
            txtTenderNo.Visible = true;
           

        }
        else
        {
            lblcolon.Visible = true;
            lblProductName.Visible = true;
            txtProductName.Visible = true;
            lblTender.Visible = false;
            lblTendercolon.Visible = false;
            txtTenderNo.Visible = false;
           

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
    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerName.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnCustomerId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select Customer in suggestion only");
                txtCustomerName.Text = "";
                txtCustomerName.Focus();
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
                dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    public static string[] GetCompletionListInquiryNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesInquiryHeader ObjSinquiryHeader = new Inv_SalesInquiryHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjSinquiryHeader.GetSIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SInquiryNo"].ToString();
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
                dt = ObjSinquiryHeader.GetSIHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SInquiryNo"].ToString();
                    }
                }
            }
        }
        return str;
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Name like '" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        if (dtCon.Rows.Count == 0)
        {
            dtCon = dtCustomer;
        }
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




    protected void txtInquiryNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInquiryNo.Text != "")
        {
            DataTable Dt = objSInquiryHeader.GetSIHeaderAllBySInquiryNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNo.Text);
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        Session["ReportHeader"] = null;
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        

        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

       
        if(ddlReportType.SelectedIndex==0)
        {

            SalesDataSetTableAdapters.sp_Inv_SalesInvoiceHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceHeader_SelectRow_Report;

            if (ddlGroupBy.SelectedIndex == 0)
            {
                Session["ReportHeader"] = "Sales Invoice Header Report By Invoice Date";
                Session["ReportType"] = "0";
            }

            if (ddlGroupBy.SelectedIndex == 1)
            {
                Session["ReportHeader"] = "Sales Invoice Header Report By Customer";
                Session["ReportType"] = "1";
            }

            if (ddlGroupBy.SelectedIndex == 2)
            {
                Session["ReportHeader"] = "Sales Invoice Header Report By Sales Person";
                Session["ReportType"] = "2";
            }
            if (txtFromDateInvoice.Text != "" && txtToDateInvoice.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateInvoice.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "IDate>='" + txtFromDateInvoice.Text + "' and IDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "IDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateInvoice.Text = Convert.ToDateTime(Dt.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateInvoice.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;
                txtFromDate.Text = "";
                txtToDate.Text = "";


            }
            if (txtInvoiceNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_No='" + txtInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
             

            }
            if (txtCustomerNameInvoice.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Customer_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
               

            }

            if (txtSalesPerson.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SalesPerson_Id ='" +hdnsalespersonId.Value+ "'", "", DataViewRowState.CurrentRows).ToTable();
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInquiryHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

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
        if (ddlReportType.SelectedIndex==1)
        {
            ObjSalesDataset.EnforceConstraints = false;
            SalesDataSetTableAdapters.sp_Inv_SalesInqDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInqDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_SalesInqDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()),0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesInqDetail_SelectRow_Report;

            if (ddlGroupBy.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Inquiry_Detail_Report_By_Inquiry_Date;
                Session["ReportType"] = "0";
            }
            else
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Inquiry_Detail_Report_By_Inquiry_No;
                Session["ReportType"] = "1";
            }


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "IDate>='" + txtFromDate.Text + "' and IDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "FilterBy:From " + txtFromDate.Text + "  To " + txtToDate.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "IDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["IDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;
                txtFromDate.Text = "";
                txtToDate.Text = "";


            }
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SInquiryNo='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                

            }
            if (txtCustomerName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Customer_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInquiryDetailReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlReportType.SelectedIndex = 0;
        ddlReportType_SelectedIndexChanged(null, null);
        
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtTenderNo.Text = "";
        txtCustomerName.Text = "";
        txtInquiryNo.Text = "";
       


    }
    
    #endregion
    #region SalesQuotation
    public void BindddlGroupQuotation()
    {
        try
        {
            ddlGroupByQuotation.Items.Clear();
        }
        catch
        {
        }
        ddlGroupByQuotation.Items.Insert(0,Resources.Attendance.Quotation_Date);
        if (ddlReportTypeQuotation.SelectedIndex == 0)
        {
            ddlGroupByQuotation.Items.Insert(1, Resources.Attendance.Inquiry_No_);
            ddlGroupByQuotation.Items.Insert(2, Resources.Attendance.Customer_Name);
            ddlGroupByQuotation.Items.Insert(3, Resources.Attendance.Sales_Person);
        }
        else
        {
            ddlGroupByQuotation.Items.Insert(1, Resources.Attendance.Quotation_No_);
            ddlGroupByQuotation.Items.Insert(2, Resources.Attendance.Inquiry_No_);
            ddlGroupByQuotation.Items.Insert(3, Resources.Attendance.Customer_Name);
        }
        

    }
    protected void ddlReportTypeQuotation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroupQuotation();
        if (ddlReportTypeQuotation.SelectedValue == "0")
        {
            lblQuotationColon.Visible = false;
            lblProductNameQuotation.Visible = false;
            txtProductNameQuotation.Visible = false;
           


        }
        else
        {
            lblQuotationColon.Visible = true;
            lblProductNameQuotation.Visible = true;
            txtProductNameQuotation.Visible = true;
           

           

        }
    }
    protected void txtInquiryNoInquotation_TextChanged(object sender, EventArgs e)
    {
        if (txtInquiryNoInquotation.Text != "")
        {
            DataTable Dt = objSInquiryHeader.GetSIHeaderAllBySInquiryNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtInquiryNoInquotation.Text);
            if (Dt.Rows.Count > 0)
            {
            }
            else
            {
                DisplayMessage("Select Inquiry No. in suggestion Only");
                txtInquiryNoInquotation.Text = "";
                txtInquiryNoInquotation.Focus();

                return;
            }
        }
    }
    protected void txtQuotationNo_TextChanged(object sender, EventArgs e)
    {
        if (txtQuotationNo.Text != "")
        {
            DataTable Dt = objSQuotationHeader.GetQuotationHeaderAllBySQuotationNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtQuotationNo.Text);
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
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductNameQuotation.Text.ToString());
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
    protected void txtCustomerNamequotation_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerNamequotation.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerNamequotation.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnCustomerId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select Customer in suggestion only");
                txtCustomerNamequotation.Text = "";
                txtCustomerNamequotation.Focus();
                return;
            }


        }


    }

    protected void btnSaveQuotationReport_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
       
        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

        if (ddlReportTypeQuotation.SelectedIndex==0)
        {

            SalesDataSetTableAdapters.sp_Inv_SalesQuotationHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesQuotationHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_SalesQuotationHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjSalesDataset.sp_Inv_SalesQuotationHeader_SelectRow_Report;
            Session["ReportType"] = ddlGroupByQuotation.SelectedIndex;
            if (ddlGroupByQuotation.SelectedIndex == 0)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Header_Report_By_Quotation_Date;

                
            }
            if (ddlGroupByQuotation.SelectedIndex == 1)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Header_Report_By_Inquiry_No;

               
            }
            if (ddlGroupByQuotation.SelectedIndex == 2)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Header_Report_By_Customer;


            }
            if (ddlGroupByQuotation.SelectedIndex == 3)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Header_Report_By_Sales_Person;


            }


            if (txtFromDateQuotation.Text != "" && txtToDateQuotation.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateQuotation.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "Quotation_Date>='" + txtFromDateQuotation.Text + "' and Quotation_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateQuotation.Text + "  To: " + txtToDateQuotation.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "Quotation_Date asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateQuotation.Text = Convert.ToDateTime(Dt.Rows[0]["Quotation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

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
                    DtFilter = new DataView(DtFilter, "SQuotation_No ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
             

            }

            if (txtCustomerNamequotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerId =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
              

            }


            if (txtInquiryNoInquotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Inq_No='" + txtInquiryNoInquotation.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SQuotationHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

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
        if (ddlReportTypeQuotation.SelectedIndex==1)
        {
            SalesDataSetTableAdapters.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesQuotationDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesQuotationDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()),0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesQuotationDetail_SelectRow_Report;


           

            Session["ReportType"] = ddlGroupByQuotation.SelectedIndex;
            Session["ReportType"] = ddlGroupByQuotation.SelectedIndex;
            if (ddlGroupByQuotation.SelectedIndex == 0)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Detail_Report_By_Quotation_Date;


            }
            if (ddlGroupByQuotation.SelectedIndex == 1)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Detail_Report_By_Quotation_No;


            }
            if (ddlGroupByQuotation.SelectedIndex == 2)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Detail_Report_By_Inquiry_No;


            }
            if (ddlGroupByQuotation.SelectedIndex == 3)
            {

                Session["ReportHeader"] = Resources.Attendance.Sales_Quotation_Detail_Report_By_Customer;


            }


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "Quotation_Date>='" + txtFromDate.Text + "' and Quotation_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDate.Text + "  To " + txtToDate.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "Quotation_Date asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateQuotation.Text = Convert.ToDateTime(Dt.Rows[0]["Quotation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

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
                    DtFilter = new DataView(DtFilter, "SQuotation_No ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
              
            }

            if (txtCustomerName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerId =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
               

            }


            if (txtInquiryNoInquotation.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Inq_No='" + txtInquiryNoInquotation.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SQuotationDetailReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

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
    protected void btnResetQuotationReport_Click(object sender, EventArgs e)
    {
        ddlReportTypeQuotation.SelectedIndex = 0;
        ddlReportTypeQuotation_SelectedIndexChanged(null, null);

        txtFromDateQuotation.Text = "";
        txtToDateQuotation.Text = "";
        txtProductNameQuotation.Text = "";
        txtQuotationNo.Text = "";
        txtCustomerNamequotation.Text = "";
        txtInquiryNoInquotation.Text = "";



    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListQuotationNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesQuotationHeader ObjSquotationHeader = new Inv_SalesQuotationHeader(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjSquotationHeader.GetQuotationHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SQuotation_No"].ToString();
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
                dt = ObjSquotationHeader.GetQuotationHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SQuotation_No"].ToString();
                    }
                }
            }
        }
        return str;
    }
    #endregion
    #region SalesOrder
    public void BindddlGroupOrder()
    {   try
        {
            ddlGroupByOrder.Items.Clear();
      
        }
        catch
        {
        }
            ddlGroupByOrder.Items.Insert(0, Resources.Attendance.Order_Date);
            if (ddlReportTypeOrder.SelectedIndex == 0)
            {
                ddlGroupByOrder.Items.Insert(1, Resources.Attendance.Quotation_No_);
            }
            else
            {
                ddlGroupByOrder.Items.Insert(1, Resources.Attendance.Order_No_);
            }
            ddlGroupByOrder.Items.Insert(2, Resources.Attendance.Customer_Name);
    }
    protected void ddlReportTypeOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddlGroupOrder();
        if (ddlReportTypeOrder.SelectedIndex == 0)
        {
           
            lblProductNameOrder.Visible = false;
            lblProductNameColon.Visible = false;
            txtProductNameOrder.Visible = false;

        }
        else
        {

            lblProductNameOrder.Visible = true;
            lblProductNameColon.Visible = true;
            txtProductNameOrder.Visible = true;

        }
    }

    protected void txtOrderNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objSOrderHeader.GetSOHeaderAllBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtOrderNo.Text);
        if (Dt.Rows.Count > 0)
        {
        }
        else
        {
            DisplayMessage("Select Order No. in suggestion Only");
            txtOrderNo.Text = "";
            txtOrderNo.Focus();

            return;
        }
    }
    protected void txtQuotationNoInOrder_TextChanged(object sender, EventArgs e)
    {
        if (txtQuotationNoInOrder.Text != "")
        {
            DataTable Dt = objSQuotationHeader.GetQuotationHeaderAllBySQuotationNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtQuotationNoInOrder.Text);
            if (Dt.Rows.Count > 0)
            {
            }
            else
            {
                DisplayMessage("Select Quotation No. in suggestion Only");
                txtQuotationNoInOrder.Text = "";
                txtQuotationNoInOrder.Focus();

                return;
            }
        }
    }

    protected void txtCustomerNameInOrder_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerNameInOrder.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerNameInOrder.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnCustomerId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select Customer in suggestion only");
                txtCustomerNameInOrder.Text = "";
                txtCustomerNameInOrder.Focus();
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
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), txtProductNameOrder.Text.ToString());
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListOrderNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesOrderHeader objSOrderHeader = new Inv_SalesOrderHeader(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objSOrderHeader.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
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
                dt = objSOrderHeader.GetSOHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
                    }
                }
            }
        }
        return str;
    }
    protected void btnSaveOrderReport_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
      


        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;

        if (ddlReportTypeOrder.SelectedIndex==0)
        {

            Session["ReportType"] = ddlGroupByOrder.SelectedIndex.ToString();
            if (ddlGroupByOrder.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Order_Header_Report_By_Order_Date;
            }
            else
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Order_Header_Report_By_Quotation_No;
            }
           
            SalesDataSetTableAdapters.sp_Inv_SalesOrderHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesOrderHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjSalesDataset.sp_Inv_SalesOrderHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjSalesDataset.sp_Inv_SalesOrderHeader_SelectRow_Report;


            if (ddlGroupByOrder.SelectedIndex == 1)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType='Q' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }

            if (txtFromDateOrder.Text != "" && txtToDateOrder.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateOrder.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderDate>='" + txtFromDateOrder.Text + "' and SalesOrderDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtToDateOrder.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "SalesOrderDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateOrder.Text = Convert.ToDateTime(Dt.Rows[0]["SalesOrderDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateOrder.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtToDateOrder.Text;
                txtFromDateOrder.Text = "";
                txtToDateOrder.Text = "";
            }
            if (ddlOrderType.SelectedIndex > 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType ='" + ddlOrderType.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
               
            }
            if (txtOrderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderNo='" + txtOrderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                
            }


            if (txtQuotationNoInOrder.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType='Q' and  RefNo ='" + txtQuotationNoInOrder.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
              
            }

            if (txtCustomerNameInOrder.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerId =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SOrderHeaderReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

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
        else
        {

            
            Session["ReportType"] = ddlGroupByOrder.SelectedIndex.ToString();
            if (ddlGroupByOrder.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Order_Detail_Report_By_Order_Date;
            }
            if (ddlGroupByOrder.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Order_Detail_Report_By_Order_No;
            }
            if (ddlGroupByOrder.SelectedIndex == 2)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Order_Detail_Report_By_Customer;
            }

            SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesOrderDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()),0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesOrderDetail_SelectRow_Report;




            if (txtFromDateOrder.Text != "" && txtToDateOrder.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateOrder.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderDate>='" + txtFromDateOrder.Text + "' and SalesOrderDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtToDateOrder.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "SalesOrderDate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateOrder.Text = Convert.ToDateTime(Dt.Rows[0]["SalesOrderDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDateOrder.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDateOrder.Text + "  To: " + txtToDateOrder.Text;
                txtFromDateOrder.Text = "";
                txtToDateOrder.Text = "";
            }
            if (ddlOrderType.SelectedIndex > 0)
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType ='" + ddlOrderType.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                
            }
            if (txtOrderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SalesOrderNo='" + txtOrderNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
              

            }


            if (txtQuotationNoInOrder.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SOfromTransType='Q' and  RefNo ='" + txtQuotationNoInOrder.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                
            }

            if (txtCustomerNameInOrder.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "CustomerId =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
              

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

                




            }

            if (DtFilter.Rows.Count > 0)
            {



                Session["DtFilter"] = DtFilter;
                try
                {

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SOrderDetailReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

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
    protected void btnResetOrderReport_Click(object sender, EventArgs e)
    {
        ddlReportTypeOrder.SelectedIndex = 0;
        ddlReportTypeOrder_SelectedIndexChanged(null, null);

        txtFromDateOrder.Text = "";
        txtToDateOrder.Text = "";
        txtProductNameOrder.Text = "";
        ddlOrderType.SelectedIndex = 0;
        txtCustomerNameInOrder.Text = "";
        txtQuotationNoInOrder.Text = "";
        txtOrderNo.Text = "";



    }
   
    #endregion
    #region SalesInvoice

    public void BindddlGroupInvoice()
    {
        try
        {
            ddlGroupByInvoice.Items.Clear();
        }
        catch
        {
        }
        if (ddlInvoiceReportType.SelectedIndex == 0)
        {
            ddlGroupByInvoice.Items.Insert(0, Resources.Attendance.Invoice_Date);
            ddlGroupByInvoice.Items.Insert(1, Resources.Attendance.Customer_Name);
            ddlGroupByInvoice.Items.Insert(2, Resources.Attendance.Sales_Person);
        }
        else
        {
            ddlGroupByInvoice.Items.Insert(0, Resources.Attendance.Invoice_Date);
            ddlGroupByInvoice.Items.Insert(1, Resources.Attendance.Invoice_No);
            ddlGroupByInvoice.Items.Insert(2, Resources.Attendance.Customer_Name);
            
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
            lblsalesperson.Visible = true;
            lblsalespersoncolon.Visible = true;
            txtSalesPerson.Visible = true;


        }
        else
        {

            lblProductNameInvoice.Visible = true;
            lblColonInvoice.Visible = true;
            txtProductNameInvoice.Visible = true;
            lblsalesperson.Visible = false;
            lblsalespersoncolon.Visible = false;
            txtSalesPerson.Visible = false;

        }
    }



    protected void txtInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNo.Text != "")
        {
            DataTable Dt = objSinvoiceheader.GetSInvHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                Dt = new DataView(Dt, "Invoice_No='" + txtInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (Dt.Rows.Count > 0)
            {
                hdnInvoiceNo.Value = Dt.Rows[0]["Trans_ID"].ToString();
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

    protected void txtCustomerNameInvoice_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomerNameInvoice.Text != "")
        {
            DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomerNameInvoice.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count > 0)
            {

                hdnCustomerId.Value = dt.Rows[0]["Trans_Id"].ToString();

            }
            else
            {
                DisplayMessage("select Customer in suggestion only");
                txtCustomerNameInvoice.Text = "";
                txtCustomerNameInvoice.Focus();
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
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            strEmployeeId = GetEmployeeId(txtSalesPerson.Text);
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
                hdnsalespersonId.Value = strEmployeeId;

            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtSalesPerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);
            }
        }
    }

    private string GetEmployeeId(string strEmployeeName)
    {
        string retval = string.Empty;
        if (strEmployeeName != "")
        {
            DataTable dtEmployee = objEmployee.GetEmployeeMasterByEmpName(Session["CompId"].ToString(), strEmployeeName.Split('/')[0].ToString());

            try
            {
                dtEmployee = new DataView(dtEmployee, "Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
               
            }
            if (dtEmployee.Rows.Count > 0)
            {
                retval = (strEmployeeName.Split('/'))[strEmployeeName.Split('/').Length - 1];

                DataTable dtEmp = objEmployee.GetEmployeeMasterById(Session["CompId"].ToString(), retval);
                if (dtEmp.Rows.Count > 0)
                {

                }
                else
                {
                    retval = "";
                }
            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_InvoiceNo(string prefixText, int count, string contextKey)
    {
        Inv_SalesInvoiceHeader objsInvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objsInvoiceHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        try
        {
            dt = new DataView(dt, "Invoice_No like '" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
            string[] str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
           {
     
        
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Invoice_No"].ToString();
            }
        }
        else
        {
            
              dt = objsInvoiceHeader.GetSInvHeaderAllTrue(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Invoice_No"].ToString();
                    }
                }
            
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = ObjEmployeeMaster.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());
        dt1 = new DataView(dt1, "Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataView(dt1, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //txt[i] = dt.Rows[i]["Emp_Name"].ToString();
                txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    //txt[i] = dt1.Rows[i]["Emp_Name"].ToString();
                    txt[i] = dt1.Rows[i]["Emp_Name"].ToString() + "/" + dt1.Rows[i]["Emp_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }

    protected void BtnInvoiceReport_Click(object sender, EventArgs e)
    {
        Session["ReportHeader"] = null;
        Session["Parameter"] = null;
        Session["DtFilter"] = null;


        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;


        if (ddlInvoiceReportType.SelectedIndex == 0)
        {

            SalesDataSetTableAdapters.sp_Inv_SalesInvoiceHeader_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceHeader_SelectRow_Report;

            if (ddlGroupByInvoice.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Invoice_Header_Report_By_Invoice_Date;
                Session["ReportType"] = "0";
            }

            if (ddlGroupByInvoice.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Invoice_Header_Report_By_Customer;
                Session["ReportType"] = "1";
            }

            if (ddlGroupByInvoice.SelectedIndex == 2)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Invoice_Header_Report_By_Sales_Person;
                Session["ReportType"] = "2";
            }
            if (txtFromDateInvoice.Text != "" && txtToDateInvoice.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateInvoice.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_Date>='" + txtFromDateInvoice.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "Invoice_Date asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateInvoice.Text = Convert.ToDateTime(Dt.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

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
                    DtFilter = new DataView(DtFilter, "Invoice_No='" + txtInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }
            if (txtCustomerNameInvoice.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "supplier_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }

            if (txtSalesPerson.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SalesPerson_Id ='" + hdnsalespersonId.Value+ "'", "", DataViewRowState.CurrentRows).ToTable();
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInvoiceHeaderReport.aspx','','height=650,width=900,scrollbars=Yes')", true);

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
            ObjSalesDataset.EnforceConstraints = false;
            SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()),0);
            DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report;

            if (ddlGroupByInvoice.SelectedIndex == 0)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Invoice_Detail_Report_By_Invoice_Date;
                Session["ReportType"] = "0";
            }
            if (ddlGroupByInvoice.SelectedIndex == 1)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Invoice_Detail_Report_By_Invoice_Number;
                Session["ReportType"] = "1";
            }
            if (ddlGroupByInvoice.SelectedIndex == 2)
            {
                Session["ReportHeader"] = Resources.Attendance.Sales_Invoice_Detail_Report_By_Customer_Name;
                Session["ReportType"] = "2";
            }


            if (txtFromDateInvoice.Text != "" && txtToDateInvoice.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDateInvoice.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "Invoice_Date>='" + txtFromDateInvoice.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDateInvoice.Text + "  To: " + txtToDateInvoice.Text;


            }
            else
            {
                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "Invoice_Date asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDateInvoice.Text = Convert.ToDateTime(Dt.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

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
                    DtFilter = new DataView(DtFilter, "Invoice_No='" + txtInvoiceNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }
            if (txtCustomerNameInvoice.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Customer_Id =" + hdnCustomerId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }


            }






            if (txtProductNameInvoice.Text != "")
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

                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SInvoiceDetailReport.aspx','','height=650,width=1274,scrollbars=Yes')", true);

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
    protected void BtnResetInvoiceReport_Click(object sender, EventArgs e)
    {

        ddlInvoiceReportType.SelectedIndex = 0;
        ddlInvoiceReportType_SelectedIndexChanged(null, null);
        txtFromDateInvoice.Text = "";
        txtToDateInvoice.Text = "";
        txtSalesPerson.Text = "";
        txtCustomerNameInvoice.Text = "";
       txtProductNameInvoice.Text = "";
        txtInvoiceNo.Text = "";



    }

    #endregion

    #region SalesReturn
    public void BindddlGroupReturn()
    {
        try
        {
            ddlGroupByReturn.Items.Clear();
        }
        catch
        {
        }

        ddlGroupByReturn.Items.Insert(0, Resources.Attendance.Invoice_Date);
        ddlGroupByReturn.Items.Insert(1, Resources.Attendance.Invoice_No);
        ddlGroupByReturn.Items.Insert(2, Resources.Attendance.Order_Date);
        ddlGroupByReturn.Items.Insert(3, Resources.Attendance.Order_No_);
       



    }
    protected void txtOrderNoReturn_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objSOrderHeader.GetSOHeaderAllBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtOrderNoReturn.Text);
        if (Dt.Rows.Count > 0)
        {
        }
        else
        {
            DisplayMessage("Select Order No. in suggestion Only");
            txtOrderNoReturn.Text = "";
            txtOrderNoReturn.Focus();

            return;
        }
    }
    protected void txtInvoiceNoReturn_TextChanged(object sender, EventArgs e)
    {
        if (txtInvoiceNoReturn.Text != "")
        {
            DataTable Dt = objSinvoiceheader.GetSInvHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            try
            {
                Dt = new DataView(Dt, "Invoice_No='" + txtInvoiceNoReturn.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (Dt.Rows.Count > 0)
            {
                hdnInvoiceNo.Value = Dt.Rows[0]["Trans_ID"].ToString();
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
    protected void btnReturnReport_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;


        DataTable DtFilter = new DataTable();
        SalesDataSet ObjSalesDataset = new SalesDataSet();
        ObjSalesDataset.EnforceConstraints = false;




        SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter adp = new SalesDataSetTableAdapters.sp_Inv_SalesInvoiceDetail_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();

        adp.Fill(ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()),0);
        DtFilter = ObjSalesDataset.sp_Inv_SalesInvoiceDetail_SelectRow_Report;



        if (ddlGroupByReturn.SelectedIndex == 0)
        {
            Session["ReportHeader"] = Resources.Attendance.Sales_Return_Report_By_Invoice_Date;
            Session["ReportType"] = "0";

        }
        if (ddlGroupByReturn.SelectedIndex == 1)
        {
            Session["ReportHeader"] = Resources.Attendance.Sales_Return_Report_By_Invoice_Number;
            Session["ReportType"] = "1";
        }
        if (ddlGroupByReturn.SelectedIndex == 2)
        {
            Session["ReportHeader"] = Resources.Attendance.Sales_Return_Report_By_Order_Date;
            Session["ReportType"] = "2";
        }
        if (ddlGroupByReturn.SelectedIndex == 3)
        {
            Session["ReportHeader"] = Resources.Attendance.Sales_Return_Report_By_Order_Number;
            Session["ReportType"] = "3";
        }



        if (txtFromDateReturn.Text != "" && txtToDateReturn.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDateReturn.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            try
            {
                DtFilter = new DataView(DtFilter, "Invoice_Date>='" + txtFromDateReturn.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
            Session["Parameter"] = "From: " + txtFromDateReturn.Text + "  To: " + txtToDateReturn.Text;
        }
        else
        {
            DataTable Dt = DtFilter.Copy();
            Dt = new DataView(Dt, "", "Invoice_Date asc", DataViewRowState.CurrentRows).ToTable();
            if (Dt.Rows.Count > 0)
            {

                txtFromDateReturn.Text = Convert.ToDateTime(Dt.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            }
            txtToDateReturn.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

            Session["Parameter"] = "From: " + txtFromDateReturn.Text + "  To: " + txtToDateReturn.Text;
            txtFromDateReturn.Text = "";
            txtToDateReturn.Text = "";

        }
        if (txtInvoiceNoReturn.Text != "")
        {

            try
            {
                DtFilter = new DataView(DtFilter, "Invoice_No=" +txtInvoiceNoReturn.Text+ "", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {
            }
          

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
            

        }
        if (txtOrderNoReturn.Text != "")
        {
            try
            {
                DtFilter = new DataView(DtFilter, "SalesOrderNo=" + txtOrderNoReturn.Text + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }


        }


       

        if (txtProductnameReturn.Text != "")
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

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/SalesReturn_Report.aspx','','height=650,width=900,scrollbars=Yes')", true);

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
    protected void btnResetReturnReport_Click(object sender, EventArgs e)
    {
        BindddlGroupReturn();
        txtFromDateReturn.Text = "";
        txtToDateReturn.Text = "";
        txtInvoiceNoReturn.Text = "";
        txtOrderNoReturn.Text = "";
        txtProductnameReturn.Text = "";


    }
    #endregion

}
