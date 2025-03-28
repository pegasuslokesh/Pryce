using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_Purchase_Invoice : BasePage
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("183", (DataTable)Session["ModuleName"]);
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
            ChkInvoiceno.Visible = false;


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
            ChkInvoiceno.Visible = true;
            ChkInvoiceno.Checked = false;
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

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report;

            if (chkSupplier.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Invoice Header Report By Supplier";
                Session["ReportType"] = "1";
            }
            else
            {

                Session["ReportHeader"] = "Purchase Invoice Header Report By Invoice Date";
                Session["ReportType"] = "2";
            }

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "InvoiceDate>='" + txtFromDate.Text + "' and InvoiceDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

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

            if (ddlInvoiceType.SelectedIndex != 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Field1='" + ddlInvoiceType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                string InvoiceType = string.Empty;

                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Header Report By InvoiceType(" + ddlInvoiceType.SelectedItem.Text + ")";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , InvoiceType( " + ddlInvoiceType.SelectedItem.Text + ")";
                }
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
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Header Report By Invoice Number";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice Number";
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
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Header Report By Supplier Invoice Number";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier Invoice Number";
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
                if (chkSupplier.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Invoice Header Report By Supplier";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
                    }
                }

            }
            if (txtOrderNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "PoId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Header Report By Order Number";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Order Number";
                }
            }
            if (ddlPostStatus.SelectedIndex != 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Post='" + ddlPostStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                string Status = string.Empty;
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Header Report By Status(" + ddlPostStatus.SelectedItem.Text + ")";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Status( " + ddlPostStatus.SelectedItem.Text + ")";
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

                DataTable Dt = new DataTable();

                Dt.Columns.Add("TransId");
                Dt.Columns.Add("InvoiceNo");
                Dt.Columns.Add("InvoiceDate", typeof(DateTime));
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
                Dt.Columns.Add("CurrencyID");

                DataTable DtFilter_Copy = DtFilter.Copy();
                DtFilter_Copy = DtFilter_Copy.DefaultView.ToTable(true, "TransId", "InvoiceNo", "InvoiceDate", "SupInvoiceNo", "SupInvoiceDate", "RefType", "Currency_Name", "PaymentModeName", "Remark", "GrandTotal", "SupplierId", "Supplier_Name", "CurrencyID");
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
                    dr["CurrencyID"] = DtFilter_Copy.Rows[i]["CurrencyID"].ToString();
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
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PurchaseInvoiceHeaderReport.aspx','window','width=1024');", true);



                //Response.Redirect("../Purchase_Report/PurchaseInvoiceHeaderReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                txtFromDate.Focus();
                return;
            }
        }
        if (RbtnDetail.Checked == true)
        {

            PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseInvoiceHeader_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseInvoiceHeader_SelectRow_Report;

            if (chkSupplier.Checked == false && ChkInvoiceno.Checked == false)
            {
                Session["ReportHeader"] = "Purchase Invoice Detail Report By Invoice Date";
                Session["ReportType"] = "0";
            }

            if (chkSupplier.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Invoice Detail Report By Supplier";
                Session["ReportType"] = "1";
            }

            if (ChkInvoiceno.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Invoice Detail Report By Invoice No";
                Session["ReportType"] = "2";
            }

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "InvoiceDate>='" + txtFromDate.Text + "' and InvoiceDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

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

            if (ddlInvoiceType.SelectedIndex != 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Field1='" + ddlInvoiceType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                string InvoiceType = string.Empty;

                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Detail Report By Invoice Type(" + ddlInvoiceType.SelectedItem.Text + ")";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice Type(" + ddlInvoiceType.SelectedItem.Text + ")";
                }
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
                if (ChkInvoiceno.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Invoice Detail Report By Invoice No.";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Invoice No.";
                    }
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

                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Detail Report By Supplier Invoice No.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier Invoice No.";
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
                if (chkSupplier.Checked == false)
                {
                    if (Session["ReportHeader"] == null)
                    {
                        Session["ReportHeader"] = "Purchase Invoice Detail Report By Supplier";
                    }
                    else
                    {
                        Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Supplier";
                    }
                }

            }
            if (txtOrderNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "PoId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Detail Report By Order No.";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " ,Order No.";
                }

            }
            if (ddlPostStatus.SelectedIndex != 0)
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "Post='" + ddlPostStatus.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                string Status = string.Empty;
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Detail Report By Post(" + ddlPostStatus.SelectedItem.Text + ")";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , Post(" + ddlPostStatus.SelectedItem.Text + ")";
                }
            }
            if (txtProductName.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "ProductId=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "Purchase Invoice Detail Report By Product";
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

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PurchaseInvoiceDetailReport.aspx','window','width=1024');", true);

                //Response.Redirect("../Purchase_Report/PurchaseInvoiceDetailReport.aspx");
            }
            else
            {
                DisplayMessage("Record Not Found");
                txtFromDate.Focus();
                return;
            }

        }










    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        rbtnheader.Checked = true;
        RbtnDetail.Checked = false;

        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlInvoiceType.SelectedIndex = 0;
        txtInvoiceNo.Text = "";
        txtSupInvoiceNo.Text = "";

        txtProductName.Text = "";
        ddlPostStatus.SelectedIndex = 0;
        txtSupplierName.Text = "";
        txtOrderNo.Text = "";
        ChkInvoiceno.Visible = false;




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
    protected void txtOrderNo_TextChanged(object sender, EventArgs e)
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
