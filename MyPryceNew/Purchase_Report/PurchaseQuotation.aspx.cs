using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_PurchaseQuotation : BasePage
{
    Inv_ProductMaster ObjProductMaster = null;
    LocationMaster objLocation = null;
    SystemParameter ObjSysParam = null;
    PurchaseRequestHeader objPRequestHeader = null;
    Ems_ContactMaster ObjContactMaster = null;
    Inv_PurchaseInquiryHeader objPInquiryHeader = null;
    Inv_PurchaseInquiry_Supplier ObjPISupplier = null;
    Inv_PurchaseQuoteHeader objPQuotHeader = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objPRequestHeader = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objPInquiryHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        ObjPISupplier = new Inv_PurchaseInquiry_Supplier(Session["DBConnection"].ToString());
        objPQuotHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            rbtnQuotationdate.Checked = true;
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("178", (DataTable)Session["ModuleName"]);
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


    protected void rbtnheader_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnheader.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = false;
            lblProductName.Visible = false;
            //lblcolon.Visible = false;
            rbtnInquiryNo.Visible = true;
            rbtnInquiryNo.Checked = true;


        }


    }
    protected void RbtnDetail_CheckedChanged(object sender, EventArgs e)
    {
        if (RbtnDetail.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = true;
            lblProductName.Visible = true;
            //lblcolon.Visible = true;
            rbtnInquiryNo.Visible = false;
            rbtnQuotationdate.Checked = true;
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        Session["Parameter"] = null;
        Session["DtFilter"] = null;
        Session["ReportType"] = null;
        Session["ReportHeader"] = null;
        if (rbtnheader.Checked == false && RbtnDetail.Checked == false)
        {
            DisplayMessage("Select the Report Type(Header or Detail)");
            rbtnheader.Focus();
            return;
        }

        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (rbtnheader.Checked == true)
        {

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseQuoteHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 2);

            if (rbtnInquiryNo.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Quotation Header Report By Inquiry No.";
                Session["ReportType"] = "0";

                //here we calling the PquotationHeaderByInquiryNo report object



            }
            if (rbtnQuotationNo.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Quotation Header Report By Quotation No.";
                Session["ReportType"] = "1";

                //here we calling the PquotationHeaderByquotationNo report object



            }

            if (rbtnInquiryNo.Checked == false && rbtnQuotationNo.Checked == false && rbtnQuotationdate.Checked == false)

            {

                Session["ReportHeader"] = "Purchase Quotation Header Report By Supplier";
                Session["ReportType"] = "2";
                //here we calling the PquotationHeader report object

            }
            if (rbtnQuotationdate.Checked == true)
            {

                Session["ReportHeader"] = "Purchase Quotation Header Report By Quotation Date";
                Session["ReportType"] = "3";
                //here we calling the PquotationHeadernuQuotationDate report object

            }
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseQuoteHeader_SelectRow_Report;

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
                Session["Parameter"] = "From " + txtFromDate.Text + "  To " + txtToDate.Text;


            }
            else
            {

                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "RPQ_Date asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["RPQ_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


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
                if (rbtnQuotationNo.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Quotation Header Report By QuotationNo.";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , QuotationNo.";
                    }
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
                //if (Session["Parameter"] == null)
                //{
                //    Session["Parameter"] = "FilterBy:SupplierWise";
                //}
                //else
                //{
                //    Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";
                //}

            }


            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Inquiry_No='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (rbtnInquiryNo.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Quotation Header Report By InquiryNo.";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , InquiryNo.";
                    }
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
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PQuotationHeaderReport.aspx','window','width=1024');", true);

                // Response.Redirect("../Purchase_Report/PQuotationHeaderReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                rbtnheader.Focus();
                return;
            }
        }
        if (RbtnDetail.Checked == true)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseQuoteDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseQuoteDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()));
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseQuoteDetail_SelectRow_Report;
            if (rbtnQuotationNo.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Quotation Detail Report By Quotation No";
                Session["ReportType"] = "0";
            }
            if (rbtnQuotationdate.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Quotation Detail Report By Quotation Date";
                Session["ReportType"] = "1";

            }
            if (rbtnSupplier.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Quotation Detail Report By Supplier";
                Session["ReportType"] = "2";
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
            if (txtQuotationNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "RPQ_No ='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (rbtnQuotationNo.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Quotation Detail Report By Quotation No.";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , QuotationNo.";
                    }
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
                if (rbtnSupplier.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Quotation Detail Report By Supplier";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
                    }
                }

            }
            if (txtInquiryNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "Inquiry_No='" + txtInquiryNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Quotation Detail Report By Inquiry No.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Inquiry No.";
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
                    Session["ReportHeader"] = "Purchase Quotation Detail Report By Product";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Product";
                }
                if (DtFilter.Rows.Count > 0)
                {

                    Session["DtFilter"] = DtFilter;

                    //    Response.Redirect("../Purchase_Report/PInquirydetailReportByProduct.aspx");
                    //}
                }
                else
                {
                    DisplayMessage("Record Not Found");
                    RbtnDetail.Focus();
                    return;
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

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PQuotationDetailReport.aspx','window','width=1024');", true);

                //Response.Redirect("../Purchase_Report/PQuotationDetailReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                RbtnDetail.Focus();
                return;
            }
        }







    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        rbtnheader.Checked = true;
        RbtnDetail.Checked = false;
        rbtnInquiryNo.Visible = true;
        rbtnInquiryNo.Checked = false;
        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtQuotationNo.Text = "";
        txtSupplierName.Text = "";
        txtInquiryNo.Text = "";
        rbtnInquiryNo.Checked = false;
        rbtnQuotationNo.Checked = false;
        rbtnQuotationdate.Checked = false;
        rbtnQuotationdate.Checked = true;





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

    protected void txtQuotationNo_TextChanged(object sender, EventArgs e)
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
