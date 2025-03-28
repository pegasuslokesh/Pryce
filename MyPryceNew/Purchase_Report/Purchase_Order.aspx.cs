using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Purchase_Report_Purchase_Order : BasePage
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

        if (!IsPostBack)
        {
         
            Session["FromDate"] = null;
            Session["ToDate"] = null;
            Session["DtFilter"] = null;

            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtFromDate.Focus();
            chkOrderNo.Visible = false;
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("168", (DataTable)Session["ModuleName"]);
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
            chkOrderNo.Visible = false;


        }


    }
    protected void rbtnShipping_CheckedChanged(object sender, EventArgs e)
    {

        rbtnheader_CheckedChanged(null, null);
    }
    protected void RbtnDetail_CheckedChanged(object sender, EventArgs e)
    {
        if (RbtnDetail.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = true;
            lblProductName.Visible = true;
           // lblcolon.Visible = true;
            chkOrderNo.Visible = true;
            chkOrderNo.Checked = false;
        }



    }
    protected void rbtnOrderStatus_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnOrderStatus.Checked == true)
        {
            txtFromDate.Focus();
            txtProductName.Visible = true;
            lblProductName.Visible = true;
            //lblcolon.Visible = true;
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
        if (rbtnheader.Checked == false && RbtnDetail.Checked == false && rbtnOrderStatus.Checked == false && rbtnShipping.Checked == false)
        {
            DisplayMessage("Select the Report Type(Header or Detail or Status Report)");
            rbtnheader.Focus();
            return;
        }

        DataTable DtFilter = new DataTable();
        PurchaseDataSet ObjPurchaseDataset = new PurchaseDataSet();
        ObjPurchaseDataset.EnforceConstraints = false;

        if (rbtnheader.Checked == true)
        {

            DataTable Dt = new DataTable();


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
            Dt.Columns.Add("PurchaseOrderId");
            Dt.Columns.Add("CurrencyID");



            DataTable dtPoheader = ObjPurchaseOrder.GetPurchaseOrderTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

            if (chkSupplier.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Order Header Report By Supplier";
                Session["ReportType"] = "0";

            }
            else
            {
                Session["ReportHeader"] = "Purchase Order Header Report By Order Date";
                Session["ReportType"] = "1";

            }

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    dtPoheader = new DataView(dtPoheader, "CreatedDate>='" + txtFromDate.Text + "' and CreatedDate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;

            }
            else
            {

                DataTable DtDate = dtPoheader.Copy();
                DtDate = new DataView(DtDate, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (DtDate.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(DtDate.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


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
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "FilterBy:PurchaseOrderNo.Wise";
                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , PurchaseOrderNo.";
                }

            }
            if (txtSupplierName.Text != "")
            {
                try
                {
                    dtPoheader = new DataView(dtPoheader, "SupplierId=" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }

                if (chkSupplier.Checked == false)
                {
                    if (Session["Parameter"] == null)
                    {
                        Session["Parameter"] = "FilterBy:SupplierWise";
                    }
                    else
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , Supplier";
                    }
                }
            }
            if (txtQuotationNo.Text != "")
            {
                try
                {
                    dtPoheader = new DataView(dtPoheader, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["ReportHeader"] == null)
                {
                    Session["ReportHeader"] = "FilterBy:QuotationNo.Wise";

                }
                else
                {
                    Session["ReportHeader"] = Session["ReportHeader"].ToString() + " , QuotationNo.";
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
                dr["CurrencyID"] = dtPoheader.Rows[i]["CurrencyID"].ToString();
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
                DataTable DtQuotation = objPurchaseQuotation.GetQuoteHeaderAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtPoheader.Rows[i]["ReferenceID"].ToString());
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

                DataTable DtPaymentMode = objPaymentMode.GetPaymentModeMasterById(Session["CompId"].ToString(), PaymentModeId,Session["BrandId"].ToString(),Session["LocId"].ToString());
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
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/Purchase_Order_Report.aspx','window','width=1024');", true);

                //Response.Redirect("../Purchase_Report/Purchase_Order_Report.aspx");
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
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report;
            if (chkOrderNo.Checked == true)
            {

                Session["ReportHeader"] = "Purchase Order Detail Report By Order No.";
                Session["ReportType"] = "0";

            }

            if (chkSupplier.Checked == true)
            {
                Session["ReportHeader"] = "Purchase Order Detail Report By Supplier";
                Session["ReportType"] = "1";

            }
            if (chkOrderNo.Checked == false && chkSupplier.Checked == false)
            {
                Session["ReportHeader"] = "Purchase Order Detail Report By Order Date";
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


                DtFilter = new DataView(DtFilter, "", "CreatedDate asc", DataViewRowState.CurrentRows).ToTable();
                if (DtFilter.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(DtFilter.Rows[0]["CreatedDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


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
                if (chkOrderNo.Checked == false)
                {
                    if (Session["Parameter"] == null)
                    {
                        Session["Parameter"] = "FilterBy:PurchaseOrderNo. Wise";
                    }
                    else
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , PurchaseOrderNo. Wise";

                    }
                }

            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SupplierId =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (chkSupplier.Checked == false)
                {
                    if (Session["Parameter"] == null)
                    {
                        Session["Parameter"] = "FilterBy:SupplierWise";
                    }
                    else
                    {
                        Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";

                    }
                }

            }
            if (txtQuotationNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy:QuotationNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";

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
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "FilterBy:ProductWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";

                }


            }

            if (DtFilter.Rows.Count > 0)
            {
                Session["DtFilter"] = DtFilter;

                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/POrderDetailReport.aspx','window','width=1024');", true);

                // Response.Redirect("../Purchase_Report/POrderDetailReport.aspx");


            }
            else
            {
                DisplayMessage("Record Not Found");
                RbtnDetail.Focus();
                return;
            }
        }


        if (rbtnOrderStatus.Checked == true)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter();

            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report;






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
            if (txtOrderNo.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "TransId=" + hdnOrderNo.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:PurchaseOrderNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , PurchaseOrderNo.Wise";
                }

            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SupplierId =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:SupplierWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";
                }

            }
            if (txtQuotationNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:QuotationNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";
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
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:ProductWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";
                }


            }

            if (DtFilter.Rows.Count > 0)
            {
                DtFilter = new DataView(DtFilter, "Pendingqty>0", "", DataViewRowState.CurrentRows).ToTable();
                Session["DtFilter"] = DtFilter;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/POrderStatusReport.aspx','window','width=1024');", true);

                //Response.Redirect("../Purchase_Report/POrderStatusReport.aspx");


            }
            else
            {
                DisplayMessage("Record Not Found");
                RbtnDetail.Focus();
                return;
            }
        }



        if (rbtnShipping.Checked == true)
        {
            PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter adp = new PurchaseDataSetTableAdapters.sp_Inv_PurchaseOrderDetail_SelectRow_ReportTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();

            adp.Fill(ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["BrandId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), 0);
            DtFilter = ObjPurchaseDataset.sp_Inv_PurchaseOrderDetail_SelectRow_Report;



            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                try
                {
                    DtFilter = new DataView(DtFilter, "PODate>='" + txtFromDate.Text + "' and PODate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


            }
            else
            {

                DataTable Dt = DtFilter.Copy();
                Dt = new DataView(Dt, "", "PODate asc", DataViewRowState.CurrentRows).ToTable();
                if (Dt.Rows.Count > 0)
                {

                    txtFromDate.Text = Convert.ToDateTime(Dt.Rows[0]["PODate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                }
                txtToDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());

                Session["Parameter"] = "From: " + txtFromDate.Text + "  To: " + txtToDate.Text;


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
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:PurchaseOrderNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , PurchaseOrderNo.Wise";
                }

            }
            if (txtSupplierName.Text != "")
            {

                try
                {
                    DtFilter = new DataView(DtFilter, "SupplierId =" + hdnSupplierId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:SupplierWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , SupplierWise";
                }

            }
            if (txtQuotationNo.Text != "")
            {
                try
                {
                    DtFilter = new DataView(DtFilter, "ReferenceVoucherType='PQ' and ReferenceID='" + hdnQuotationId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
                }

                catch
                {
                }
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:QuotationNo.Wise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , QuotationNo.Wise";
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
                if (Session["Parameter"] == null)
                {
                    Session["Parameter"] = "Filter By:ProductWise";
                }
                else
                {
                    Session["Parameter"] = Session["Parameter"].ToString() + " , ProductWise";
                }


            }

            if (DtFilter.Rows.Count > 0)
            {
                Inv_ShipExpDetail ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());

                DtFilter = DtFilter.DefaultView.ToTable(true, "TransID", "PoNO", "PODate", "Supplier_Name", "Airway_Bill_No", "DateShipping", "DateReceiving", "TotalWeight", "Volumetric_weight", "UnitRate", "Shippingline_Name");

                DtFilter.DefaultView.Sort = "PODate asc";

                DataTable dtShip = new DataTable();

                dtShip.Columns.Add("PONo");
                dtShip.Columns.Add("PODate");
                dtShip.Columns.Add("ShippingLine");
                dtShip.Columns.Add("SupplierName");
                dtShip.Columns.Add("AirwayBillNo");
                dtShip.Columns.Add("ShippingDate");
                dtShip.Columns.Add("ReceivingDate");
                dtShip.Columns.Add("ActualWeight");
                dtShip.Columns.Add("VolumetricWeight");
                dtShip.Columns.Add("Unit_Rate");
                dtShip.Columns.Add("Prepaid");

                foreach (DataRow dr in DtFilter.Rows)
                {
                    DataRow drnew = dtShip.NewRow();

                    drnew["PONo"] = dr["PoNO"].ToString();
                    drnew["PODate"] = dr["PODate"].ToString();
                    drnew["ShippingLine"] = dr["Shippingline_Name"].ToString();
                    drnew["SupplierName"] = dr["Supplier_Name"].ToString();
                    drnew["AirwayBillNo"] = dr["Airway_Bill_No"].ToString();
                    drnew["ShippingDate"] = dr["DateShipping"].ToString();
                    drnew["ReceivingDate"] = dr["DateReceiving"].ToString();

                    if (dr["TotalWeight"].ToString() == "")
                    {
                        dr["TotalWeight"] = "0.000";
                    }


                    if (float.Parse(dr["TotalWeight"].ToString()) > 0)
                    {
                        drnew["ActualWeight"] = dr["TotalWeight"].ToString();
                    }
                    else
                    {
                        drnew["ActualWeight"] = "0.000";
                    }



                    if (dr["Volumetric_weight"].ToString() == "")
                    {
                        dr["Volumetric_weight"] = "0.000";
                    }


                    if (float.Parse(dr["Volumetric_weight"].ToString()) > 0)
                    {
                        drnew["VolumetricWeight"] = dr["Volumetric_weight"].ToString();
                    }
                    else
                    {
                        drnew["VolumetricWeight"] = "0.000";
                    }


                    if (dr["UnitRate"].ToString() == "")
                    {
                        dr["UnitRate"] = "0.000";
                    }


                    if (float.Parse(dr["UnitRate"].ToString()) > 0)
                    {
                        drnew["Unit_Rate"] = dr["UnitRate"].ToString();
                    }
                    else
                    {
                        drnew["Unit_Rate"] = "0.000";
                    }

                    DataTable dtshipexpenses = ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["TransID"].ToString().Trim(), "PO");

                    if (dtshipexpenses.Rows.Count > 0)
                    {
                        if (float.Parse(dtshipexpenses.Rows[0]["Exp_Charges"].ToString()) > 0)
                        {
                            drnew["Prepaid"] = "Yes";
                        }
                        else
                        {
                            drnew["Prepaid"] = "No";
                        }
                    }
                    else
                    {
                        drnew["Prepaid"] = "No";
                    }
                    dtShip.Rows.Add(drnew);
                }

                Session["DtFilter"] = dtShip;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase_Report/PurchaseOrderShippingReport.aspx','window','width=1024');", true);

                //Response.Redirect("../Purchase_Report/POrderStatusReport.aspx");


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
        rbtnOrderStatus.Checked = false;
        txtFromDate.Focus();

        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductName.Text = "";
        txtQuotationNo.Text = "";
        txtSupplierName.Text = "";
        txtOrderNo.Text = "";
        chkOrderNo.Visible = false;
        chkOrderNo.Checked = false;




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




    protected void txtQuotationNo_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt = objPurchaseQuotation.GetQuoteHeaderAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        try
        {
            Dt = new DataView(Dt, "RPQ_No='" + txtQuotationNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
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
