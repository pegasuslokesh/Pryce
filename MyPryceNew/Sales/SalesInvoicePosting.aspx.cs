using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using ClosedXML.Excel;

public partial class Sales_SalesInvoicePosting : System.Web.UI.Page
{
    Inv_TaxRefDetail objTaxRefDetail = null;
    Inv_SalesInvoiceHeader objSInvHeader = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    DataAccessClass objda = null;
    Inv_SalesDeliveryVoucher_Header objdelVoucherHeader = null;
    Inv_SalesInvoiceDetail objSInvDetail = null;
    Inv_ProductLedger ObjProductLedger = null;
    Inv_ParameterMaster objInvParam = null;
    Inv_ProductMaster objProductM = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Set_DocNumber objDocNo = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Inv_PaymentTrans ObjPaymentTrans = null;
    LocationMaster objLocation = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_StockDetail objStockDetail = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Ac_AccountMaster objAccMaster = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objTaxRefDetail = new Inv_TaxRefDetail(Session["DBConnection"].ToString());
        objSInvHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objdelVoucherHeader = new Inv_SalesDeliveryVoucher_Header(Session["DBConnection"].ToString());
        objSInvDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        ObjProductLedger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        ObjPaymentTrans = new Inv_PaymentTrans(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objAccMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //btnPost.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnPost, "").ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../sales/SalesInvoicePosting.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillUser();
            string Decimal_Count = string.Empty;
            Decimal_Count = Session["LoginLocDecimalCount"].ToString();
            Session["Decimal_Count_For_Tax"] = Convert.ToInt32(Decimal_Count);
            txtFrom_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtToDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            FillGrid(Session["LocId"].ToString());
            Session["CHECKED_ITEMS"] = null;
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnPost.Visible = clsPagePermission.bAdd;
    }
    public void FillGrid(string location1d)
    {
        GvSalesInvoice.Columns[0].Visible = false;
        bool IsAll = true;
        string PostType = string.Empty;
        string myval = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            //PostStatus = " Post='True'";
            myval = "1";
            IsAll = false;
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            GvSalesInvoice.Columns[0].Visible = true;
            //PostStatus = " Post='False'";
            myval = "0";
            IsAll = false;
        }

        if (IsAll == false)
            PostType = " and Post = " + myval + "";
        else
            PostType = DBNull.Value.ToString();


        string strSql = "SELECT SH.location_id, SH.Trans_Id, SH.Invoice_No, SH.Invoice_Date, Set_EmployeeMaster.Emp_Name as EmployeeName, Ems_ContactMaster.Name as CustomerName, CREATEDBY.Emp_Name as InvoiceCreatedBy, SH.Post, sh.IsActive, SH.GrandTotal, SH.Currency_Id, CASE WHEN SH.SIFromTransType = 'S' THEN (SELECT STUFF((SELECT DISTINCT '','' + RTRIM(Inv_salesOrderHeader.SalesOrderNo) FROM Inv_salesOrderHeader WHERE Inv_salesOrderHeader.Trans_id IN (SELECT DISTINCT Inv_SalesInvoiceDetail.SIFromTransNo FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.Invoice_No = SH.Trans_Id) FOR xml PATH ('')), 1, 1, '')) WHEN SH.SIFromTransType ='J' THEN (SELECT SM_JobCards_Header.Job_No FROM SM_JobCards_Header WHERE SM_JobCards_Header.Trans_Id = SH.SIFromTransNo) WHEN SH.SIFromTransType = 'W' THEN (SELECT SM_WorkOrder.Work_Order_No FROM SM_WorkOrder WHERE SM_WorkOrder.Trans_Id = SH.SIFromTransNo) END AS OrderList, SH.Supplier_Id as Customer_Id, SH.Field4 FROM Inv_SalesInvoiceHeader as SH INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.emp_id = SH.SalesPerson_Id LEFT JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = SH.Supplier_Id left join (SELECT Set_UserMaster.User_Id,set_employeemaster.Emp_Name from set_employeemaster inner join Set_UserMaster on set_employeemaster.emp_id = Set_UserMaster.Emp_Id)CREATEDBY ON CREATEDBY.User_Id=SH.CreatedBy where SH.location_id in (" + location1d + ") and sH.IsActive='True' " + PostType + " ";
        if (ddlUser.SelectedItem.ToString() != "--Select User--")
        {
            strSql = strSql + " and set_employeemaster.emp_id ='" + ddlUser.SelectedValue + "'";
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            try
            {
                ObjSysParam.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                txtFromDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
                return;
            }
            try
            {
                ObjSysParam.getDateForInput(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                txtToDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                return;
            }
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            strSql += " and Invoice_Date>='" + txtFromDate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "' order by Invoice_Date desc";

        }
        else
        {
            strSql += " order by Invoice_Date desc";
        }
        using (DataTable dt = objda.return_DataTable(strSql))
        {
            objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");
            Session["dtunpostInvoice"] = dt;
            Session["DtFilterunpostInvoice"] = dt;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
            GvSalesInvoice.Dispose();
            dt.Dispose();
            Session["CHECKED_ITEMS"] = null;
            try
            {
                object CustTotal;
                CustTotal = dt.Compute("Sum(GrandTotal)", "");
                lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(CustTotal.ToString(), Session["LocCurrencyId"].ToString());
            }
            catch
            {
                lblTotalAmount_List.Text = "Total Amount = 0";
            }
        }
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
    protected void GvSalesInvoice_Sorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        DataTable dt = (DataTable)Session["DtFilterunpostInvoice"];
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

        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");
        PopulateCheckedValues();
        dt = null;
    }
    protected void GvSalesInvoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        GvSalesInvoice.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtFilterunpostInvoice"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvSalesInvoice, dt, "", "");
        PopulateCheckedValues();
        dt = null;
    }
    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    { return SystemParameter.GetCurrencySmbol(CurrencyId, ObjSysParam.GetCurencyConversionForInv(CurrencyId, Amount), Session["DBConnection"].ToString()); }
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
        dtres = null;
        return ArebicMessage;
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please select one location to continue");
            ddlLocation.Focus();
            return;
        }
        if (Session["LocId"].ToString() != ddlLocation.SelectedValue)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "redirectToHome('Login location and Invoice location are not same, please change location to continue, do you want to continue ?');", true);
            return;
        }

        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }

        string strCashflowPostedV = string.Empty;
        string strBankIdReconcile = string.Empty;
        strBankIdReconcile = objAccParameterLocation.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int Msg = 0;
            if (GvSalesInvoice.Rows.Count != 0)
            {
                SaveCheckedValues();
                if (Session["CHECKED_ITEMS"] != null)
                {
                    ArrayList userdetails = new ArrayList();
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                    if (userdetails.Count == 0)
                    {
                        DisplayMessage("Please Select Record");
                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;
                    }
                    else
                    {
                        //for get invoice TransId
                        //for get distinct product id and total invoice quantity for selected invoice
                        string strInvoiceId = string.Empty;
                        for (int i = 0; i < userdetails.Count; i++)
                        {
                            strInvoiceId += userdetails[i].ToString() + ",";
                        }

                        //for check stock of selected invoice
                        for (int i = 0; i < userdetails.Count; i++)
                        {
                            DataTable dtHeader = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), ref trns);

                            if (dtHeader.Rows.Count > 0)
                            {
                                if (!Common.IsFinancialyearAllow(Convert.ToDateTime(dtHeader.Rows[0]["Invoice_Date"].ToString()), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
                                {
                                    DisplayMessage("Log In Financial year not allowing to perform this action");

                                    trns.Rollback();
                                    if (con.State == System.Data.ConnectionState.Open)
                                    {
                                        con.Close();
                                    }
                                    trns.Dispose();
                                    con.Dispose();
                                    return;
                                }

                                //here we are checking that payment detail exist or not fo current invoice
                                //code start
                                if (ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString(), "SI", userdetails[i].ToString(), ref trns).Rows.Count == 0)
                                {
                                    DisplayMessage("Payment information not found for Invoice No. = " + dtHeader.Rows[0]["Invoice_No"].ToString());

                                    trns.Rollback();
                                    if (con.State == System.Data.ConnectionState.Open)
                                    {
                                        con.Close();
                                    }
                                    trns.Dispose();
                                    con.Dispose();
                                    return;

                                }



                                DataTable dtInvoiceDetail = new DataTable();
                                dtInvoiceDetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), ref trns, Session["FinanceYearId"].ToString());
                                dtInvoiceDetail.AcceptChanges();
                                dtInvoiceDetail.Columns["SysQty"].ReadOnly = false;
                                foreach (DataRow dr in dtInvoiceDetail.Rows)
                                {
                                    if (dr["SysQty"].ToString() == "" || dr["SysQty"].ToString() == null)
                                    {
                                        dr["SysQty"] = "0";
                                    }
                                    //here we check stock quantity availabe or not 
                                    if (dtHeader.Rows[0]["SIFromTransType"].ToString() == "D" || dtHeader.Rows[0]["SIFromTransType"].ToString() == "J" || dtHeader.Rows[0]["SIFromTransType"].ToString() == "W" || objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["SIFromTransNo"].ToString(), ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                                    {
                                        DataTable dtTempProduct = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), dr["Product_Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                                        if (dtTempProduct.Rows[0]["ItemType"].ToString() == "S")
                                        {
                                            string strsql = "select distinct Product_Id,(SUM(quantity)+sum(CAST(Field3 as numeric(18,6)))) as InvoiceQty from Inv_SalesInvoiceDetail where (SIFromTransType='D' or SIFromTransType='W' or SIFromTransType='J' or (SIFromTransType='S' and SIFromTransNo in (select Inv_SalesOrderHeader.Trans_Id from Inv_SalesOrderHeader where Inv_SalesOrderHeader.IsdeliveryVoucher='False') ))  and Invoice_No in (" + strInvoiceId.Substring(0, strInvoiceId.Length - 1).Trim() + ") group by Product_Id";
                                            DataTable dttotalinvoiceqty = objda.return_DataTable(strsql, ref trns);
                                            dttotalinvoiceqty = new DataView(dttotalinvoiceqty, "Product_Id=" + dr["Product_Id"].ToString().Trim() + "", "", DataViewRowState.CurrentRows).ToTable();

                                            if (float.Parse(dr["SysQty"].ToString()) < float.Parse(dttotalinvoiceqty.Rows[0]["InvoiceQty"].ToString()))
                                            {
                                                DisplayMessage("Stock is not available for Invoice No. = " + dtHeader.Rows[0]["Invoice_No"].ToString());
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
                                    }
                                    //here we check that serial number is entered or not for related product 
                                    if (dtHeader.Rows[0]["SIFromTransType"].ToString() == "D" || dtHeader.Rows[0]["SIFromTransType"].ToString() == "J" || dtHeader.Rows[0]["SIFromTransType"].ToString() == "W" || objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["SIFromTransNo"].ToString(), ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                                    {
                                        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), dr["Product_Id"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString() == "SNO")
                                        {
                                            using (DataTable dtStock = objda.return_DataTable("select isnull( count(*),0) from inv_stockbatchmaster where transtype='SI' and  TransTypeId=" + userdetails[i].ToString() + " and ProductId=" + dr["Product_Id"].ToString() + "", ref trns))
                                            {
                                                if (Convert.ToInt32(dtStock.Rows[0][0].ToString()) == 0)
                                                {
                                                    string strSessionLocation = Session["LocId"].ToString();
                                                    if (strSessionLocation == "11" || strSessionLocation == "14" || strSessionLocation == "15")
                                                    {

                                                    }
                                                    else
                                                    {
                                                        DisplayMessage("Serial Information is not available for Invoice No. = " + dtHeader.Rows[0]["Invoice_No"].ToString());
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
                                                //if (float.Parse(dtStock.Rows[0][0].ToString()) != float.Parse(dr["Quantity"].ToString()))
                                                //{
                                                //    DisplayMessage("Serial Information should be equal to Invoice quantity for Invoice No. = " + dtHeader.Rows[0]["Invoice_No"].ToString());
                                                //    trns.Rollback();
                                                //    if (con.State == System.Data.ConnectionState.Open)
                                                //    {
                                                //        con.Close();
                                                //    }
                                                //    trns.Dispose();
                                                //    con.Dispose();
                                                //    return;
                                                //}
                                            }
                                        }
                                    }

                                    //here we check validation that delivery voucher is posted or created for invoice and order number or not
                                    //validation added by jitendra upadhyay on 20-04-2016
                                    //according the disucssion with neeraj sir 
                                    if (dtHeader.Rows[0]["SIFromTransType"].ToString() == "S")
                                    {

                                        if (objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["SIFromTransNo"].ToString(), ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "True")
                                        {
                                            //string strsql = "select sum(Inv_SalesDeliveryVoucher_Detail.Delievered_Qty) from Inv_SalesDeliveryVoucher_Header inner join Inv_SalesDeliveryVoucher_Detail on Inv_SalesDeliveryVoucher_Header.Trans_Id=Inv_SalesDeliveryVoucher_Detail.Voucher_No where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id=" + dr["SIFromTransNo"].ToString() + " and Inv_SalesDeliveryVoucher_Header.post='True' and Inv_SalesDeliveryVoucher_Detail.Product_Id=" + dr["Product_Id"].ToString() + "";
                                            string strsql = "select sum(Inv_SalesDeliveryVoucher_Detail.Delievered_Qty) from Inv_SalesDeliveryVoucher_Header inner join Inv_SalesDeliveryVoucher_Detail on Inv_SalesDeliveryVoucher_Header.SalesOrder_Id=Inv_SalesDeliveryVoucher_Detail.Order_Id where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id=" + dr["SIFromTransNo"].ToString() + " and Inv_SalesDeliveryVoucher_Header.post='True' and Inv_SalesDeliveryVoucher_Detail.Product_Id=" + dr["Product_Id"].ToString() + "";

                                            if (objda.return_DataTable(strsql, ref trns).Rows[0][0].ToString() == null || objda.return_DataTable(strsql, ref trns).Rows[0][0].ToString() == "")
                                            {
                                                DisplayMessage("Delivery voucher should be Create and post for Invoice No. = " + dtHeader.Rows[0]["Invoice_No"].ToString() + " and Order No. = " + dr["SalesOrderNo"].ToString());
                                                return;
                                            }
                                            else
                                            {
                                                if (float.Parse(objda.return_DataTable(strsql, ref trns).Rows[0][0].ToString()) < float.Parse(dr["Quantity"].ToString()))
                                                {
                                                    DisplayMessage("Delivery qty and invoice qty should be same and post for Invoice No. = " + dtHeader.Rows[0]["Invoice_No"].ToString() + " and Order No. = " + dr["SalesOrderNo"].ToString());
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        string strCashAccount = string.Empty;
                        string strCreditAccount = string.Empty;
                        string strInventory = string.Empty;
                        string strCostOfSales = string.Empty;
                        string strReceiveVoucherAcc = string.Empty;
                        string strRoundoffAccount = string.Empty;
                        string strTaxAccount = string.Empty;
                        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString(), ref trns);
                        using (DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            if (dtCash.Rows.Count > 0)
                            {
                                strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                            }
                        }

                        if (new DataView(dtAcParameter, "Param_Name='RoundOffAccount'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                        {
                            strRoundoffAccount = new DataView(dtAcParameter, "Param_Name='RoundOffAccount'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
                        }

                        using (DataTable dtCredit = new DataView(dtAcParameter, "Param_Name='Sales Invoice'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            if (dtCredit.Rows.Count > 0)
                            {
                                strCreditAccount = dtCredit.Rows[0]["Param_Value"].ToString();
                            }
                        }

                        using (DataTable dtInventory = new DataView(dtAcParameter, "Param_Name='Purchase Invoice'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            if (dtInventory.Rows.Count > 0)
                            {
                                strInventory = dtInventory.Rows[0]["Param_Value"].ToString();
                            }
                        }

                        using (DataTable dtCostOfSales = new DataView(dtAcParameter, "Param_Name='Cost Of Sales'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            if (dtCostOfSales.Rows.Count > 0)
                            {
                                strCostOfSales = dtCostOfSales.Rows[0]["Param_Value"].ToString();
                            }
                        }

                        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtReceiveVoucher.Rows.Count > 0)
                        {
                            strReceiveVoucherAcc = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
                        }

                        string strAdvanceCreditAC = string.Empty;
                        using (DataTable dtAdvanceCreditAC = new DataView(dtAcParameter, "Param_Name='SO Advance Credit Account'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            if (dtAdvanceCreditAC.Rows.Count > 0)
                            {
                                strAdvanceCreditAC = dtAdvanceCreditAC.Rows[0]["Param_Value"].ToString();
                            }
                        }

                        string strLocationCode = Session["LoginLocCode"].ToString();
                        //Code Start by ghanshyam suthar on 12-04-2018
                        for (int i = 0; i < userdetails.Count; i++)
                        {
                            //first update post status 

                            //in header table

                            string strsql = "update Inv_SalesInvoiceHeader set Post='True',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id='" + userdetails[i].ToString() + "'";
                            objda.execute_Command(strsql, ref trns);


                            DataTable dtHeader = new DataTable();

                            dtHeader = objSInvHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), ref trns);

                            if (dtHeader.Rows.Count > 0)
                            {
                                string strInvCurrencyId = dtHeader.Rows[0]["currency_id"].ToString();
                                string strInvCurrencyName = dtHeader.Rows[0]["Currency_Name"].ToString(); ;
                                //For Finance Add                                

                                string strRefrenceNumber = string.Empty;
                                PegasusDataAccess.DataAccessClass ObjDa = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                                using (DataTable dtOrderNumber = ObjDa.return_DataTable("select distinct(SIFromTransNo) from Inv_SalesInvoiceDetail where Invoice_No='" + userdetails[i].ToString() + "' and SIFromTransType='S' ", ref trns))
                                {
                                    if (dtOrderNumber.Rows.Count > 0)
                                    {
                                        for (int r = 0; r < dtOrderNumber.Rows.Count; r++)
                                        {
                                            string strOrderNumber = string.Empty;
                                            string strOderId = dtOrderNumber.Rows[r]["SIFromTransNo"].ToString();
                                            if (strOderId != "")
                                            {
                                                using (DataTable dtOrderDetail = ObjDa.return_DataTable("select SalesOrderNo from Inv_SalesOrderHeader where Trans_Id='" + strOderId + "'"))
                                                {
                                                    if (dtOrderDetail.Rows.Count > 0)
                                                    {
                                                        strOrderNumber = dtOrderDetail.Rows[0]["SalesOrderNo"].ToString();
                                                        if (strRefrenceNumber == "")
                                                        {
                                                            strRefrenceNumber = strOrderNumber;
                                                        }
                                                        else if (strRefrenceNumber != "")
                                                        {
                                                            strRefrenceNumber = strRefrenceNumber + "," + strOrderNumber;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "303", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                                int b = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), userdetails[i].ToString(), "SINV", dtHeader.Rows[0]["Invoice_No"].ToString(), dtHeader.Rows[0]["Invoice_Date"].ToString(), dtHeader.Rows[0]["Invoice_No"].ToString(), dtHeader.Rows[0]["Invoice_Date"].ToString(), "SI", "1/1/1800", "1/1/1800", "", "", dtHeader.Rows[0]["Currency_Id"].ToString(), dtHeader.Rows[0]["Field5"].ToString(), "From SI On  '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", false.ToString(), false.ToString(), false.ToString(), "AV", "", "", "Invoice Refrence No. '" + dtHeader.Rows[0]["Invoice_Ref_No"].ToString() + "' Refrence Order No. '" + dtHeader.Rows[0]["Ref_Order_Number"].ToString() + "'", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                string strVMaxId = b.ToString();

                                // Code By Ghanshyam Suthar on 11-04-2018
                                double Exchange_Rate = 1;
                                if (dtHeader.Rows[0]["Field5"].ToString() != "")
                                    Exchange_Rate = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["Field5"].ToString()));
                                //---------------------------------------------------------------------------------------------------------------------
                                // For Location Currency
                                double L_Advance_Expenses_Amount = 0;

                                double L_Invoice_Without_Tax_Amount = 0;
                                double L_Invoice_Tax_Amount = 0;
                                double L_Invoice_With_Tax_Amount = 0;

                                double L_Total_Expenses_Without_Tax_Amount = 0;
                                double L_Total_Expenses_Tax_Amount = 0;
                                double L_Total_Expenses_With_Tax_Amount = 0;

                                double L_Separate_Expenses_Without_Tax_Amount = 0;
                                double L_Separate_Expenses_Tax_Amount = 0;
                                double L_Separate_Expenses_With_Tax_Amount = 0;
                                //---------------------------------------------------------------------------------------------------------------------
                                // For Foregin Currency
                                double F_Advance_Expenses_Amount = 0;

                                double F_Invoice_Without_Tax_Amount = 0;
                                double F_Invoice_Tax_Amount = 0;
                                double F_Invoice_With_Tax_Amount = 0;

                                double F_Total_Expenses_Without_Tax_Amount = 0;
                                double F_Total_Expenses_Tax_Amount = 0;
                                double F_Total_Expenses_With_Tax_Amount = 0;

                                double F_Separate_Expenses_Without_Tax_Amount = 0;
                                double F_Separate_Expenses_Tax_Amount = 0;
                                double F_Separate_Expenses_With_Tax_Amount = 0;
                                //---------------------------------------------------------------------------------------------------------------------
                                // For Company Currency
                                double C_Advance_Expenses_Amount = 0;

                                double C_Invoice_Without_Tax_Amount = 0;
                                double C_Invoice_Tax_Amount = 0;
                                double C_Invoice_With_Tax_Amount = 0;

                                double C_Total_Expenses_Without_Tax_Amount = 0;
                                double C_Total_Expenses_Tax_Amount = 0;
                                double C_Total_Expenses_With_Tax_Amount = 0;

                                double C_Separate_Expenses_Without_Tax_Amount = 0;
                                double C_Separate_Expenses_Tax_Amount = 0;
                                double C_Separate_Expenses_With_Tax_Amount = 0;

                                double Advance_Amount = 0;
                                double NetTaxVal = 0;
                                double GrandTotal = 0;

                                string strPayTotal = "0";
                                double Cash = 0;
                                double Credit = 0;

                                //-------------------------------------------------------------------------------------------------------------------------------

                                // Foregin Currency
                                F_Advance_Expenses_Amount = Convert.ToDouble(Convert_Into_DF((L_Advance_Expenses_Amount).ToString()));

                                F_Invoice_Tax_Amount = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["NetTaxV"].ToString()));
                                F_Invoice_With_Tax_Amount = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["GrandTotal"].ToString()));
                                F_Invoice_Without_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Invoice_With_Tax_Amount - F_Invoice_Tax_Amount).ToString()));

                                L_Invoice_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Invoice_Tax_Amount * Exchange_Rate).ToString()));
                                L_Invoice_With_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Invoice_With_Tax_Amount * Exchange_Rate).ToString()));
                                L_Invoice_Without_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Invoice_Without_Tax_Amount * Exchange_Rate).ToString()));

                                C_Invoice_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Invoice_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());
                                C_Invoice_With_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Invoice_With_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());
                                C_Invoice_Without_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Invoice_Without_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());


                                //Get Data for Expenses Detail
                                double Total_Expenses = 0;
                                double Total_Expenses_Tax = 0;
                                DataTable dtExpenseDetail = ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), "SI", ref trns);

                                if (dtExpenseDetail.Rows.Count > 0)
                                {
                                    for (int E = 0; E < dtExpenseDetail.Rows.Count; E++)
                                    {
                                        Total_Expenses += Convert.ToDouble(dtExpenseDetail.Rows[E]["FCExpAmount"].ToString());
                                    }

                                    bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                                    if (IsTax == true)
                                    {
                                        Session["Dt_Final_Save_Tax "] = null;
                                        DataTable Dt_Final_Save_Tax = new DataTable();
                                        Dt_Final_Save_Tax.Columns.AddRange(new DataColumn[10] { new DataColumn("Tax_Type_Id", typeof(int)), new DataColumn("Tax_Type_Name"), new DataColumn("Tax_Percentage"), new DataColumn("Tax_Value"), new DataColumn("Expenses_Id"), new DataColumn("Expenses_Name"), new DataColumn("Expenses_Amount"), new DataColumn("Page_Name"), new DataColumn("Tax_Entry_Type"), new DataColumn("Tax_Account_Id") });
                                        Dt_Final_Save_Tax = objTaxRefDetail.Get_Tax_Detail_For_Expenses("SINV", userdetails[i].ToString(), "Sales_Invoice", "Multiple", "1");
                                        Session["Dt_Final_Save_Tax "] = Dt_Final_Save_Tax;
                                        Dt_Final_Save_Tax = null;
                                    }
                                    Total_Expenses_Tax = Convert.ToDouble(Get_Expenses_Tax(Total_Expenses.ToString()));

                                    F_Total_Expenses_Tax_Amount = Convert.ToDouble(Convert_Into_DF((Total_Expenses_Tax).ToString()));
                                    F_Total_Expenses_With_Tax_Amount = Convert.ToDouble(Convert_Into_DF((Total_Expenses + F_Total_Expenses_Tax_Amount).ToString()));
                                    F_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(Convert_Into_DF((Total_Expenses - Total_Expenses_Tax).ToString()));

                                    // Location Currency
                                    L_Total_Expenses_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Total_Expenses_Tax_Amount * Exchange_Rate).ToString()));
                                    L_Total_Expenses_With_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Total_Expenses_With_Tax_Amount * Exchange_Rate).ToString()));
                                    L_Total_Expenses_Without_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Total_Expenses_Without_Tax_Amount * Exchange_Rate).ToString()));

                                    // Company Currency
                                    C_Total_Expenses_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Total_Expenses_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());
                                    C_Total_Expenses_With_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Total_Expenses_With_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());
                                    C_Total_Expenses_Without_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Total_Expenses_Without_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());
                                    //------------------------------------------------------------------------------------------------------------------------
                                }

                                DataTable dtPaymentTran = ObjPaymentTrans.GetPaymentTransTrue(Session["CompId"].ToString().ToString(), "SI", userdetails[i].ToString(), ref trns);
                                if (dtPaymentTran.Rows.Count > 0)
                                {
                                    int j = 0;
                                    string L_Debit_Amount = ((L_Invoice_Without_Tax_Amount)).ToString();
                                    string F_Debit_Amount = ((F_Invoice_Without_Tax_Amount)).ToString();
                                    string C_Debit_Amount = ((C_Invoice_Without_Tax_Amount)).ToString();

                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCreditAccount, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount, "From SI On  '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[0]["payExchangerate"].ToString(), F_Debit_Amount, "0.00", C_Debit_Amount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                    //------------------------------------------------------------------------------------------------------------------------
                                    // Entry for Taxes of Product
                                    // Start Code
                                    string TaxQuery = "Select * from Inv_TaxRefDetail where (Expenses_Id is null or Expenses_Id='') and Ref_Type='SINV' and Ref_Id = " + userdetails[i].ToString() + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0";
                                    DataTable dtTaxDetails = objda.return_DataTable(TaxQuery, ref trns);
                                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                                    {
                                        string Product_Id = string.Empty;
                                        string Tax_Id = string.Empty;
                                        string Tax_Name = string.Empty;
                                        string Tax_Value = string.Empty;
                                        string Tax_Amount = string.Empty;
                                        string TaxAccountNo = string.Empty;
                                        string TaxAccountDetails = "Select * from Sys_TaxMaster where IsActive = 'true'";
                                        DataTable dtTaxAccountDetails = objda.return_DataTable(TaxAccountDetails);
                                        if (dtTaxAccountDetails == null || dtTaxAccountDetails.Rows.Count == 0)
                                        {
                                            DisplayMessage("Please Configure Account for Tax in Tax Master");
                                            trns.Rollback();
                                            return;
                                        }
                                        string TaxGrouping = "Select Tax_Id,Tax_Name,STM.Field3,SUM(CONVERT(decimal(18,2),Tax_value)) as TaxAmount from Inv_TaxRefDetail inner join Sys_TaxMaster STM on STM.Trans_Id = Tax_Id where (Expenses_Id is null or Expenses_Id='') and Ref_Type='SINV' AND Ref_Id = " + userdetails[i].ToString() + " and CAST(Tax_Per as decimal(38,6)) <> 0 and CAST(Tax_value as decimal(38,6)) <> 0 group by Tax_Id,Tax_Name,STM.Field3";
                                        DataTable TaxTableGrouping = objda.return_DataTable(TaxGrouping, ref trns);
                                        string TaxIdInfo = string.Empty;
                                        string GroupTaxId = string.Empty;
                                        double GroupTaxAmount = 0;
                                        string S_Tax_Percentage = string.Empty;
                                        string GroupTaxName = string.Empty;
                                        string strTaxPer = string.Empty;
                                        foreach (DataRow grouprow in TaxTableGrouping.Rows)
                                        {
                                            GroupTaxId = grouprow["Tax_Id"].ToString();
                                            GroupTaxAmount = Convert.ToDouble(grouprow["TaxAmount"].ToString());
                                            GroupTaxName = grouprow["Tax_Name"].ToString();
                                            TaxAccountNo = grouprow["Field3"].ToString();

                                            if (String.IsNullOrEmpty(TaxAccountNo))
                                            {
                                                DisplayMessage("Please Configure Account for Tax in Tax Master");
                                                trns.Rollback();
                                                return;
                                            }
                                            F_Separate_Expenses_Tax_Amount = Convert.ToDouble(Convert_Into_DF((GroupTaxAmount).ToString()));
                                            L_Separate_Expenses_Tax_Amount = Convert.ToDouble(Convert_Into_DF((F_Separate_Expenses_Tax_Amount * Exchange_Rate).ToString()));
                                            C_Separate_Expenses_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Separate_Expenses_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());

                                            S_Tax_Percentage = string.Empty;
                                            foreach (DataRow row in dtTaxDetails.Rows)
                                            {
                                                if (!TaxIdInfo.Contains(GroupTaxId))
                                                {
                                                    DataView groupView = new DataView(dtTaxDetails);
                                                    groupView.RowFilter = "Tax_Id = " + GroupTaxId + "";
                                                    GroupTaxName = string.Empty;
                                                    double N_Tax_Per = 0;
                                                    foreach (DataRow newrow in groupView.ToTable().Rows)
                                                    {
                                                        N_Tax_Per = N_Tax_Per + Convert.ToDouble(newrow["Tax_Per"].ToString());
                                                        S_Tax_Percentage = Convert.ToString(N_Tax_Per / Convert.ToDouble(groupView.ToTable().Rows.Count));
                                                    }
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", TaxAccountNo, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Separate_Expenses_Tax_Amount.ToString(), S_Tax_Percentage + "% " + GroupTaxName + " On Invoice No.-'" + dtHeader.Rows[0]["Invoice_No"].ToString() + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[0]["payExchangerate"].ToString(), F_Separate_Expenses_Tax_Amount.ToString(), "0.00", C_Separate_Expenses_Tax_Amount.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                    if (String.IsNullOrEmpty(TaxIdInfo))
                                                        TaxIdInfo = GroupTaxId;
                                                    else
                                                        TaxIdInfo = TaxIdInfo + "," + GroupTaxId;
                                                    break;
                                                }
                                            }
                                        }
                                        dtTaxAccountDetails = null;
                                        TaxTableGrouping = null;
                                    }
                                    //End Code
                                    //------------------------------------------------------------------------------------------------------------------------



                                    //------------------------------------------------------------------------------------------------------------------------
                                    //Start Code
                                    // Insert Expenses Entry
                                    double Expenses_Tax = 0;
                                    string[,] Net_Expenses_Tax = new string[1, 5];
                                    double Exp = 0;
                                    double CustomerExp = 0;
                                    foreach (DataRow dEx in dtExpenseDetail.Rows)
                                    {
                                        string strExpensesName = GetExpName(dEx["Expense_Id"].ToString());
                                        string strForeignAmount = dEx["FCExpAmount"].ToString();
                                        string strExpensesId = dEx["Expense_Id"].ToString();
                                        string strExpAmount = dEx["Exp_Charges"].ToString();
                                        string strAccountNo = dEx["Account_No"].ToString();
                                        string strExpCurrencyId = dEx["ExpCurrencyID"].ToString();
                                        //string strExchangeRate = dEx["ExpExchangeRate"].ToString();
                                        Exp = Convert.ToDouble(Convert_Into_DF((Exp + Convert.ToDouble(dEx["Exp_Charges"].ToString())).ToString()));

                                        if (strExpensesName == "")
                                        {
                                            strExpensesName = GetExpName(strExpensesId);
                                        }

                                        Expenses_Tax = Convert.ToDouble(Get_Expenses_Tax(strExpAmount.ToString()));

                                        L_Separate_Expenses_Tax_Amount = Convert.ToDouble(Convert_Into_DF((Expenses_Tax).ToString()));
                                        F_Separate_Expenses_Tax_Amount = Convert.ToDouble(Convert_Into_DF((L_Separate_Expenses_Tax_Amount / Exchange_Rate).ToString()));
                                        C_Separate_Expenses_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Separate_Expenses_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());

                                        L_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(Convert_Into_DF((strExpAmount).ToString()));
                                        F_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble(Convert_Into_DF((L_Separate_Expenses_Without_Tax_Amount / Exchange_Rate).ToString()));
                                        C_Separate_Expenses_Without_Tax_Amount = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Separate_Expenses_Without_Tax_Amount).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());


                                        if (strAccountNo == strReceiveVoucherAcc)
                                        {
                                            //check currency account exist or not for customer - 22/08/2018(Neelkanth Purohit)
                                            string strCustomerAcc = objAccMaster.GetCustomerAccountByCurrency(dtHeader.Rows[0]["Supplier_Id"].ToString(), strInvCurrencyId).ToString();
                                            if (strCustomerAcc == "0")
                                            {
                                                throw new System.ArgumentException("Account No not exist in " + strInvCurrencyName + " , first create it");
                                            }


                                            string L_Debit_Amount_Exp = string.Empty;
                                            string F_Debit_Amount_Exp = string.Empty;
                                            string C_Debit_Amount_Exp = string.Empty;

                                            L_Debit_Amount_Exp = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                            F_Debit_Amount_Exp = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                            C_Debit_Amount_Exp = (C_Separate_Expenses_Without_Tax_Amount).ToString();


                                            CustomerExp += double.Parse(strExpAmount);
                                            //objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strReceiveVoucherAcc, dtHeader.Rows[0]["Supplier_Id"].ToString(), userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount_Exp, "'" + strExpensesName + "' On Sales Invoice No '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, Exchange_Rate.ToString(), F_Debit_Amount_Exp, "0.00", C_Debit_Amount_Exp, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strReceiveVoucherAcc, strCustomerAcc, userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount_Exp, "'" + strExpensesName + "' On Sales Invoice No '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, Exchange_Rate.ToString(), F_Debit_Amount_Exp, "0.00", C_Debit_Amount_Exp, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                            Tax_Insert_Into_Ac_Voucher_Detail_Debit(strVMaxId, "", strReceiveVoucherAcc, dtHeader.Rows[0]["Supplier_Id"].ToString(), userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Separate_Expenses_Without_Tax_Amount.ToString(), "'" + strExpensesName + "' On Sales Invoice No '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, Exchange_Rate.ToString(), F_Separate_Expenses_Without_Tax_Amount.ToString(), "0.00", C_Separate_Expenses_Without_Tax_Amount.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                        }
                                        else
                                        {
                                            string L_Debit_Amount_Exp = string.Empty;
                                            string F_Debit_Amount_Exp = string.Empty;
                                            string C_Debit_Amount_Exp = string.Empty;

                                            L_Debit_Amount_Exp = (L_Separate_Expenses_Without_Tax_Amount).ToString();
                                            F_Debit_Amount_Exp = (F_Separate_Expenses_Without_Tax_Amount).ToString();
                                            C_Debit_Amount_Exp = (C_Separate_Expenses_Without_Tax_Amount).ToString();

                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strAccountNo, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Debit_Amount_Exp, "'" + strExpensesName + "' On Sales Invoice No '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, Exchange_Rate.ToString(), F_Debit_Amount_Exp, "0.00", C_Debit_Amount_Exp, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                            Tax_Insert_Into_Ac_Voucher_Detail_Debit(strVMaxId, "", strAccountNo, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Separate_Expenses_Without_Tax_Amount.ToString(), "'" + strExpensesName + "' On Sales Invoice No '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "'", "", Session["EmpId"].ToString(), strExpCurrencyId, Exchange_Rate.ToString(), F_Separate_Expenses_Without_Tax_Amount.ToString(), "0.00", C_Separate_Expenses_Without_Tax_Amount.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strExpensesId, ref trns);
                                        }
                                    }

                                    //End Code
                                    // Insert Expenses Entry
                                    //------------------------------------------------------------------------------------------------------------------------


                                    string strPaymentType = string.Empty;
                                    for (int P = 0; P < dtPaymentTran.Rows.Count; P++)
                                    {
                                        strPaymentType = dtPaymentTran.Rows[P]["PaymentType"].ToString();

                                        //string L_Debit_Amount_Final = ((L_Invoice_With_Tax_Amount + L_Total_Expenses_With_Tax_Amount)).ToString();
                                        //string F_Debit_Amount_Final = ((F_Invoice_With_Tax_Amount + F_Total_Expenses_With_Tax_Amount)).ToString();
                                        //string C_Debit_Amount_Final = ((C_Invoice_With_Tax_Amount + C_Total_Expenses_With_Tax_Amount)).ToString();

                                        string L_Debit_Amount_Final = Convert_Into_DF(dtPaymentTran.Rows[P]["Pay_Charges"].ToString());
                                        string F_Debit_Amount_Final = Convert_Into_DF(dtPaymentTran.Rows[P]["FcPayAmount"].ToString());
                                        string C_Debit_Amount_Final = Convert_Into_DF(dtPaymentTran.Rows[P]["Pay_Charges"].ToString()); //need to convert this amount in company currency
                                        string strNarration = "From SI On  '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + Session["LoginLocCode"].ToString() + "'";
                                        int creditNoteVoucherId = 0;
                                        Int32.TryParse(dtPaymentTran.Rows[P]["field3"].ToString(), out creditNoteVoucherId);
                                        strNarration = creditNoteVoucherId > 0 ? strNarration + " Credit Note " + objda.get_SingleValue("select voucher_no from ac_voucher_header where trans_id=" + creditNoteVoucherId) + " has been adjusted" : strNarration;
                                        if (strPaymentType == "Cash")
                                        {
                                            if (dtPaymentTran.Rows[P]["AccountNo"].ToString() == strReceiveVoucherAcc)
                                            {
                                                if (strBankIdReconcile.Split(',').Contains(strReceiveVoucherAcc))
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), strReceiveVoucherAcc, dtHeader.Rows[0]["Supplier_Id"].ToString(), userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", true.ToString(), "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                                else
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), strReceiveVoucherAcc, dtHeader.Rows[0]["Supplier_Id"].ToString(), userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                            }
                                            else
                                            {
                                                if (strBankIdReconcile.Split(',').Contains(dtPaymentTran.Rows[P]["AccountNo"].ToString()))
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), dtPaymentTran.Rows[P]["AccountNo"].ToString(), "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", true.ToString(), "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                                else
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), dtPaymentTran.Rows[P]["AccountNo"].ToString(), "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                            }

                                            strPayTotal = (float.Parse(strPayTotal) + float.Parse(L_Debit_Amount_Final)).ToString();
                                            Cash = Cash + Convert.ToDouble(L_Debit_Amount_Final);
                                        }
                                        else if (strPaymentType == "Credit")
                                        {
                                            if (dtPaymentTran.Rows[P]["AccountNo"].ToString() == strReceiveVoucherAcc)
                                            {
                                                string strMerchantId = "0";
                                                if (!string.IsNullOrEmpty(dtHeader.Rows[0]["Invoice_Merchant_Id"].ToString()) && dtHeader.Rows[0]["Invoice_Merchant_Id"].ToString() != "0")
                                                {
                                                    using (DataTable _dt = new MerchantMaster(Session["DBConnection"].ToString()).GetMerchantMasterById(dtHeader.Rows[0]["Invoice_Merchant_Id"].ToString()))
                                                    {
                                                        if (_dt.Rows.Count > 0 && _dt.Rows[0]["Merchant_name"].ToString().ToLower() != "direct")
                                                        {
                                                            if (string.IsNullOrEmpty(_dt.Rows[0]["field1"].ToString()) || _dt.Rows[0]["field1"].ToString() == "0")
                                                            {
                                                                throw new System.ArgumentException("Contact is not linked with merchant - " + _dt.Rows[0]["Merchant_Name"].ToString());
                                                            }
                                                            else
                                                            {
                                                                strMerchantId = _dt.Rows[0]["field1"].ToString();
                                                            }

                                                        }
                                                    }
                                                }
                                                //check currency account exist or not for customer - 22/08/2018(Neelkanth Purohit)
                                                string strCustomerAcc = "0";
                                                if (strMerchantId != "0")
                                                {
                                                    strCustomerAcc = objAccMaster.GetCustomerAccountByCurrency(strMerchantId, strInvCurrencyId).ToString();
                                                    if (strCustomerAcc == "0")
                                                    {
                                                        throw new System.ArgumentException("Account No not exist in " + strInvCurrencyName + " of Merchant , first create it");
                                                    }
                                                }
                                                else
                                                {
                                                    strCustomerAcc = objAccMaster.GetCustomerAccountByCurrency(dtHeader.Rows[0]["Supplier_Id"].ToString(), strInvCurrencyId).ToString();
                                                    if (strCustomerAcc == "0")
                                                    {
                                                        throw new System.ArgumentException("Account No not exist in " + strInvCurrencyName + " , first create it");
                                                    }
                                                }



                                                if (strBankIdReconcile.Split(',').Contains(strReceiveVoucherAcc))
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), strReceiveVoucherAcc, strCustomerAcc, userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", true.ToString(), "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                                else
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), strReceiveVoucherAcc, strCustomerAcc, userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                            }
                                            else
                                            {
                                                if (strBankIdReconcile.Split(',').Contains(dtPaymentTran.Rows[P]["AccountNo"].ToString()))
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), dtPaymentTran.Rows[P]["AccountNo"].ToString(), "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", true.ToString(), "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                                else
                                                {
                                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, (j++).ToString(), dtPaymentTran.Rows[P]["AccountNo"].ToString(), "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Debit_Amount_Final, "0.00", strNarration, "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), dtPaymentTran.Rows[P]["payExchangerate"].ToString(), F_Debit_Amount_Final, C_Debit_Amount_Final, "0.00", "", "", "", "", "", "True", "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                                }
                                            }
                                            Credit = Credit + Convert.ToDouble(L_Debit_Amount_Final);
                                        }
                                    }
                                    dtTaxDetails = null;
                                }
                                //End
                                // End By Ghanshyam Suthar on 12-04-2018
                                DataTable dtDetail = new DataTable();
                                dtDetail = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), ref trns, Session["FinanceYearId"].ToString());
                                int delvouchercounter = 0;
                                string strTotalAllAmount = string.Empty;
                                double TotalQty = 0;
                                foreach (DataRow dr in dtDetail.Rows)
                                {

                                    TotalQty = Convert.ToDouble(dr["Quantity"].ToString()) + Convert.ToDouble(dr["FreeQty"].ToString());

                                    if (dr["SIFromTransType"].ToString() == "D" || dr["SIFromTransType"].ToString() == "W" || dr["SIFromTransType"].ToString() == "J")
                                    {
                                        string UnitPrice = ((Convert.ToDouble(dr["UnitPrice"].ToString()) - Convert.ToDouble(dr["DiscountV"].ToString()) + Convert.ToDouble(dr["TaxV"].ToString())) / Convert.ToDouble(dtHeader.Rows[0]["Field5"].ToString())).ToString();
                                        ObjProductLedger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", userdetails[i].ToString(), "0", dr["Product_Id"].ToString(), dr["Unit_Id"].ToString(), "O", "0", "0", "0", TotalQty.ToString(), "1/1/1800", UnitPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(dtHeader.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                                    }
                                    else
                                    {
                                        if (objSOrderHeader.GetSOHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["SIFromTransNo"].ToString(), ref trns).Rows[0]["IsdeliveryVoucher"].ToString() == "False")
                                        {
                                            string UnitPrice = ((Convert.ToDouble(dr["UnitPrice"].ToString()) - Convert.ToDouble(dr["DiscountV"].ToString()) + Convert.ToDouble(dr["TaxV"].ToString())) / Convert.ToDouble(dtHeader.Rows[0]["Field5"].ToString())).ToString();
                                            ObjProductLedger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SI", userdetails[i].ToString(), "0", dr["Product_Id"].ToString(), dr["Unit_Id"].ToString(), "O", "0", "0", "0", TotalQty.ToString(), "1/1/1800", UnitPrice, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(dtHeader.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                                        }
                                    }

                                    //update post status in detail table in field 1 and also update cost 
                                    string AvgCost = string.Empty;

                                    try
                                    {
                                        AvgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), dr["Product_Id"].ToString(), ref trns).Rows[0]["Field2"].ToString();
                                    }
                                    catch
                                    {
                                        AvgCost = "0";
                                    }
                                    strsql = "update Inv_SalesInvoiceDetail set Field1='True',Field2='" + AvgCost + "',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + dr["Trans_Id"].ToString() + "";
                                    objda.execute_Command(strsql, ref trns);
                                }

                                //Add Finance Code On 26-01-2016

                                strTotalAllAmount = (L_Total_Expenses_With_Tax_Amount + L_Invoice_With_Tax_Amount).ToString();
                                string strTotalAmount = (L_Total_Expenses_With_Tax_Amount + L_Invoice_With_Tax_Amount).ToString();


                                string strDebitAmount = (float.Parse(strTotalAmount) - float.Parse(strPayTotal.ToString())).ToString();

                                string strSalesFreign = (float.Parse(strDebitAmount) * float.Parse(dtHeader.Rows[0]["Field5"].ToString())).ToString();

                                if (dtDetail.Rows.Count > 0)
                                {
                                    strSalesFreign = (float.Parse(strDebitAmount) * float.Parse(dtHeader.Rows[0]["Field5"].ToString())).ToString();
                                }
                                string strPaidTotal = "0.00";
                                if (dtDetail.Rows.Count > 0)
                                {
                                }
                                double CostofSales = 0;
                                DataTable dtCOS = new DataTable();
                                dtCOS = objSInvDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetails[i].ToString(), ref trns, Session["FinanceYearId"].ToString());
                                if (dtCOS.Rows.Count > 0)
                                {
                                    for (int D = 0; D < dtCOS.Rows.Count; D++)
                                    {
                                        string strCost = "0";
                                        try
                                        {
                                            strCost = (float.Parse(dtCOS.Rows[D]["Quantity"].ToString()) * float.Parse(dtCOS.Rows[D]["Field2"].ToString())).ToString();

                                        }
                                        catch
                                        {

                                        }
                                        CostofSales += Convert.ToDouble(strCost);
                                    }
                                }
                                else
                                {
                                    CostofSales = 0;
                                }

                                bool IsCostingEntry = false;
                                IsCostingEntry = Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsCostingEntry").Rows[0]["ParameterValue"].ToString());
                                if (IsCostingEntry)
                                {
                                    if (CostofSales != 0)
                                    {
                                        double Exchange_Rate_Temp = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["Field5"].ToString()));
                                        double L_Cost = Convert.ToDouble(Convert_Into_DF((CostofSales.ToString()).ToString()));
                                        double F_Cost = Convert.ToDouble(Convert_Into_DF((L_Cost * Exchange_Rate).ToString()));
                                        double C_Cost = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Cost).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());

                                        if (strBankIdReconcile.Split(',').Contains(strCostOfSales))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Cost.ToString(), "0.00", "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), C_Cost.ToString(), "0.00", "", true.ToString(), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Cost.ToString(), "0.00", "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), C_Cost.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        if (strBankIdReconcile.Split(',').Contains(strInventory))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Cost.ToString(), "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), "0.00", C_Cost.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Cost.ToString(), "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), "0.00", C_Cost.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                    }
                                    else
                                    {
                                        double Exchange_Rate_Temp = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["Field5"].ToString()));
                                        double L_Cost = Convert.ToDouble(Convert_Into_DF((CostofSales.ToString()).ToString()));
                                        double F_Cost = Convert.ToDouble(Convert_Into_DF((L_Cost * Exchange_Rate).ToString()));
                                        double C_Cost = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Cost).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());

                                        if (strBankIdReconcile.Split(',').Contains(strCostOfSales))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Cost.ToString(), "0.00", "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), C_Cost.ToString(), "0.00", "", true.ToString(), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strCostOfSales, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Cost.ToString(), "0.00", "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), C_Cost.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }

                                        //for credit entry
                                        string str3CompanyCrrValueCr = SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), CostofSales.ToString(), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString());
                                        string Company3CurrCredit = str3CompanyCrrValueCr.Trim().Split('/')[0].ToString();
                                        if (strBankIdReconcile.Split(',').Contains(strInventory))
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Cost.ToString(), "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), "0.00", C_Cost.ToString(), "", true.ToString(), "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                        else
                                        {
                                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strInventory, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Cost.ToString(), "Sales Cost On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), F_Cost.ToString(), "0.00", C_Cost.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                        }
                                    }
                                }
                                DataTable newcheck = objVoucherDetail.GetSumRecordByVoucherNo(strVMaxId, ref trns);
                                if (newcheck.Rows.Count > 0)
                                {
                                    double DebitTotal = Convert.ToDouble(Convert_Into_DF(newcheck.Rows[0]["DebitTotal"].ToString()));
                                    double CreditTotal = Convert.ToDouble(Convert_Into_DF(newcheck.Rows[0]["CreditTotal"].ToString()));

                                    if (DebitTotal > CreditTotal)
                                    {
                                        double Exchange_Rate_Temp = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["Field5"].ToString()));
                                        double L_Cost = Convert.ToDouble(Convert_Into_DF((DebitTotal - CreditTotal).ToString()));
                                        double F_Cost = Convert.ToDouble(Convert_Into_DF((L_Cost * Exchange_Rate).ToString()));
                                        double C_Cost = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Cost).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());


                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strRoundoffAccount, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", "0.00", L_Cost.ToString(), "Round off difference On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), "0.00", "0.00", C_Cost.ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                        // objVoucherDetail.InsertVoucherDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strVMaxId, (j++).ToString(), strRoundoffAccount, "0", HdnEdit.Value, "SINV", "1/1/1800", "1/1/1800", "0", "0.00", diff.ToString(), "Payment RoundOff Diffrence On PI '" + txtInvoiceNo.Text + "'", "", Session["EmpId"].ToString(), strRoundCurrencyId, "0.00", "0.00", "0.00", CompanyCurrRoundCredit, "", "", "", "", "", "True", "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    else if (CreditTotal > DebitTotal)
                                    {
                                        double Exchange_Rate_Temp = Convert.ToDouble(Convert_Into_DF(dtHeader.Rows[0]["Field5"].ToString()));
                                        double L_Cost = Convert.ToDouble(Convert_Into_DF((CreditTotal - DebitTotal).ToString()));
                                        double F_Cost = Convert.ToDouble(Convert_Into_DF((L_Cost * Exchange_Rate).ToString()));
                                        double C_Cost = Convert.ToDouble((SystemParameter.GetCurrency(Session["CurrencyId"].ToString(), (Convert_Into_DF((L_Cost).ToString())), Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["LocId"].ToString())).Split('/')[0].ToString());

                                        objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "", strRoundoffAccount, "0", userdetails[i].ToString(), "SINV", "1/1/1800", "1/1/1800", "", L_Cost.ToString(), "0.00", "Round off difference On '" + dtHeader.Rows[0]["Invoice_No"].ToString() + "' On '" + strLocationCode + "'", "", Session["EmpId"].ToString(), dtHeader.Rows[0]["Currency_Id"].ToString(), Exchange_Rate_Temp.ToString(), "0.00", C_Cost.ToString(), "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                }
                                dtAcParameter = null;
                                dtExpenseDetail = null;
                                dtPaymentTran = null;
                                dtDetail = null;
                                dtCOS = null;
                                newcheck = null;
                            }
                            dtHeader = null;
                        }

                        DisplayMessage("Record Posted Successfully");
                        Session["CHECKED_ITEMS"] = null;
                    }
                }
                else
                {
                    DisplayMessage("Please Select Record");
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
            else
            {
                DisplayMessage("Record Not Found");
                trns.Rollback();
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
                trns.Dispose();
                con.Dispose();
                return;
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid(ddlLocation.SelectedValue);
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


    public string GetExpName(string ExpId)
    {
        return (ObjShipExp.GetShipExpMasterById(Session["CompId"].ToString(), ExpId)).Rows[0]["Exp_Name"].ToString();
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtAllowance = (DataTable)Session["DtFilterunpostInvoice"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtAllowance.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Trans_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Trans_Id"]));

            }
            foreach (GridViewRow gvrow in GvSalesInvoice.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;

            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;

        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtunpostInvoice"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvSalesInvoice, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
            dtAddressCategory1 = null;
        }
        dtAllowance = null;
    }
    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in GvSalesInvoice.Rows)
            {
                int index = (int)GvSalesInvoice.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvSalesInvoice.Rows)
        {
            index = (int)GvSalesInvoice.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSalesInvoice.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvSalesInvoice.Rows)
        {
            index = (int)GvSalesInvoice.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    #region FilterRecord
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
            DataTable dtAdd = (DataTable)Session["dtunpostInvoice"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            objPageCmn.FillData((object)GvSalesInvoice, view.ToTable(), "", "");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            Session["DtFilterunpostInvoice"] = view.ToTable();
            object CustTotal;
            CustTotal = view.ToTable().Compute("Sum(GrandTotal)", "");
            lblTotalAmount_List.Text = "Total Amount = " + GetCurrencySymbol(Convert_Into_DF(CustTotal.ToString()), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString());
            dtAdd = null;
            view = null;
        }
        if (txtValue.Text != "")
            txtValue.Focus();

    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid(ddlLocation.SelectedValue);
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Visible = true;
        Session["CHECKED_ITEMS"] = null;
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        btnRefreshReport_Click(null, null);
    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        btnRefreshReport_Click(null, null);
    }
    #endregion
    #region PrintReport


    protected void btnLoadSerial_Click(object sender, EventArgs e)
    {
        bool IsAll = true;
        string PostType = string.Empty;
        string myval = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            //PostStatus = " Post='True'";
            myval = "1";
            IsAll = false;
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            GvSalesInvoice.Columns[0].Visible = true;
            //PostStatus = " Post='False'";
            myval = "0";
            IsAll = false;
        }

        if (IsAll == false)
            PostType = " and Post = " + myval + "";
        else
            PostType = DBNull.Value.ToString();


        string strSql = "SELECT    SH.Invoice_Merchant_Id,SH.Ref_Order_Number,SH.location_id, SH.Trans_Id, SH.Invoice_No, SH.Invoice_Date, Set_EmployeeMaster.Emp_Name as EmployeeName, Ems_ContactMaster.Name as CustomerName, CREATEDBY.Emp_Name as InvoiceCreatedBy, SH.Post, sh.IsActive, SH.GrandTotal, SH.Currency_Id, CASE WHEN SH.SIFromTransType = 'S' THEN (SELECT STUFF((SELECT DISTINCT '','' + RTRIM(Inv_salesOrderHeader.SalesOrderNo) FROM Inv_salesOrderHeader WHERE Inv_salesOrderHeader.Trans_id IN (SELECT DISTINCT Inv_SalesInvoiceDetail.SIFromTransNo FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.Invoice_No = SH.Trans_Id) FOR xml PATH ('')), 1, 1, '')) WHEN SH.SIFromTransType ='J' THEN (SELECT SM_JobCards_Header.Job_No FROM SM_JobCards_Header WHERE SM_JobCards_Header.Trans_Id = SH.SIFromTransNo) WHEN SH.SIFromTransType = 'W' THEN (SELECT SM_WorkOrder.Work_Order_No FROM SM_WorkOrder WHERE SM_WorkOrder.Trans_Id = SH.SIFromTransNo) END AS OrderList, SH.Supplier_Id as Customer_Id, SH.Field4 FROM Inv_SalesInvoiceHeader as SH INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.emp_id = SH.SalesPerson_Id LEFT JOIN Ems_ContactMaster ON Ems_ContactMaster.Trans_Id = SH.Supplier_Id left join (SELECT Set_UserMaster.User_Id,set_employeemaster.Emp_Name from set_employeemaster inner join Set_UserMaster on set_employeemaster.emp_id = Set_UserMaster.Emp_Id)CREATEDBY ON CREATEDBY.User_Id=SH.CreatedBy where SH.location_id in ('" + Session["LocId"].ToString() + "') and sH.IsActive='True' " + PostType + " ";
        if (ddlUser.SelectedItem.ToString() != "--Select User--")
        {
            strSql = strSql + " and set_employeemaster.emp_id ='" + ddlUser.SelectedValue + "'";
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            try
            {
                ObjSysParam.getDateForInput(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                txtFromDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
                return;
            }
            try
            {
                ObjSysParam.getDateForInput(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                txtToDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                return;
            }
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
            strSql += " and Invoice_Date>='" + txtFromDate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "' order by Invoice_Date desc";

        }
        else
        {
            strSql += " order by Invoice_Date desc";
        }
        DataTable dtExport = new DataTable();
        dtExport.Columns.Add(new DataColumn("SalesInvoiceNo"));
        dtExport.Columns.Add(new DataColumn("OrderId"));
        dtExport.Columns.Add(new DataColumn("ProductCode"));
        dtExport.Columns.Add(new DataColumn("SerialNo"));
        dtExport.Columns.Add(new DataColumn("Remarks"));

        using (DataTable dt = objda.return_DataTable(strSql))
        {

            for (int count = 0; count < dt.Rows.Count; count++)
            {

                /* (dt.Rows[count]["Invoice_Merchant_Id"].ToString() != "0") &&*/

                //(dt.Rows[count]["Ref_Order_Number"].ToString().Length > 0)
                DataTable dtOrderDetail = objda.return_DataTable("Select Product_Id,Unit_Id ,Quantity, Inv_ProductMaster.MaintainStock,Inv_ProductMaster.ProductCode  From Inv_SalesInvoiceDetail INNER JOIN Inv_productMaster on Inv_ProductMaster.ProductId = Inv_SalesInvoiceDetail.Product_Id   where Location_Id ='" + dt.Rows[count]["Location_Id"].ToString() + "' and Invoice_No= " + dt.Rows[count]["Trans_Id"].ToString() + "  and Inv_ProductMaster.MaintainStock ='SNO'");

                for (int dCount = 0; dCount < dtOrderDetail.Rows.Count; dCount++)
                {
                    DataTable dtOrderSerial = objda.return_DataTable("    Select  * From Inv_StockBatchMaster where  TransType ='SI' and TransTypeId = '" + dt.Rows[count]["Trans_Id"].ToString() + "' ");



                    // we need to fetch detail and insert in table
                    DataTable dtTempSerial = new DataTable();
                    if (dt.Rows[count]["Ref_Order_Number"].ToString().Length > 0)
                    {
                        dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  order_id ='" + dt.Rows[count]["Ref_Order_Number"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");

                        if (dtTempSerial.Rows.Count == 0)
                        {
                            DataTable dtShipment = objda.return_DataTable("    Select * From Inv_SalesInvoiceHeader_Extra Where Invoice_No = '" + dt.Rows[count]["Trans_Id"].ToString() + "'  ");
                            if (dtShipment.Rows.Count > 0)
                            {
                                dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  alternate_scan ='" + dtShipment.Rows[0]["Shipment_Id"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");
                                if (dtTempSerial.Rows.Count == 0)
                                {
                                    dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  order_id ='" + dt.Rows[count]["Invoice_No"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");
                                }

                            }
                            else
                            {
                                dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  order_id ='" + dt.Rows[count]["Invoice_No"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");
                            }

                        }
                    }
                    else
                    {
                        DataTable dtShipment = objda.return_DataTable("    Select * From Inv_SalesInvoiceHeader_Extra Where Invoice_No = '" + dt.Rows[count]["Trans_Id"].ToString() + "'  ");
                        if (dtShipment.Rows.Count > 0)
                        {
                            dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  alternate_scan ='" + dtShipment.Rows[0]["Shipment_Id"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");
                            if (dtTempSerial.Rows.Count == 0)
                            {
                                dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  order_id ='" + dt.Rows[count]["Invoice_No"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");
                            }

                        }
                        else
                        {
                            dtTempSerial = objda.return_DataTable("    Select * From tns_merchant_product_serial where frmType='SI' and merchant_id='" + dt.Rows[count]["Invoice_Merchant_Id"].ToString() + "' and  order_id ='" + dt.Rows[count]["Invoice_No"].ToString() + "' and product_id ='" + dtOrderDetail.Rows[dCount][0].ToString() + "' ");
                        }

                    }

                    if (dtTempSerial.Rows.Count > 0)
                    {
                        if (dtOrderSerial.Rows.Count > 0)
                        {
                            objda.execute_Command("Delete From Inv_StockBatchMaster Where Location_Id ='" + dt.Rows[count]["Location_Id"].ToString() + "' AND TransType ='SI' and TransTypeId = '" + dt.Rows[count]["Trans_Id"].ToString() + "'  ");
                        }
                        for (int lSerial = 0; lSerial < dtTempSerial.Rows.Count; lSerial++)
                        {
                            // if (Convert.ToInt32(dtOrderDetail.Rows[dCount][2].ToString()) == Convert.ToInt32(dtTempSerial.Rows.Count.ToString()))
                            // {
                            bool b = false;
                            // Here we need to check serial no too it is valid or not 

                            DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMasterAll_By_ProductIdForSalesInvoice(dtOrderDetail.Rows[dCount][0].ToString(), dt.Rows[count]["location_id"].ToString());

                            DataTable dtCheckSerial = new DataView(dtStockBatch, "SerialNo = '" + dtTempSerial.Rows[lSerial]["serialno"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


                            if (dtCheckSerial.Rows.Count > 0)
                            {
                                int iResult = objda.execute_Command(" INSERT INTO Inv_StockBatchMaster VALUES (2,2,'" + dt.Rows[count]["location_id"].ToString() + "','" + dtOrderDetail.Rows[dCount][0].ToString() + "','SI','" + dt.Rows[count]["Trans_Id"].ToString() + "','" + dtOrderDetail.Rows[dCount][1].ToString() + "','O','1','0','0',GETDATE(),'" + dtTempSerial.Rows[lSerial]["serialno"].ToString() + "',GETDATE(),'','','','','0','','','','','1',getdate(),'1',7591,GETDATE(),759,GETDATE())");
                                if (iResult == 0)
                                {
                                    DataRow row = dtExport.NewRow();
                                    row[0] = dt.Rows[count]["Invoice_No"].ToString();
                                    row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                                    row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                                    row[3] = dtTempSerial.Rows[lSerial]["serialno"].ToString();
                                    row[4] = "Record Not Saved";
                                    dtExport.Rows.Add(row);
                                }
                            }
                            else
                            {

                                bool bLocationAutoAdjust = false;

                                if (dt.Rows[count]["location_id"].ToString() == "7")
                                {
                                    bLocationAutoAdjust = true;
                                }
                                // if Serial Not Found then  need to check last mode of serial and compare with product id if out then do one entry for in  and   then add for out
                                //Select TOP 1 * From Inv_StockBatchMaster where SerialNo ='"+ dtTempSerial.Rows[lSerial]["serialno"].ToString() +"'  and Location_Id = '"+ dt.Rows[count]["location_id"].ToString() +"' order by Trans_Id DESC
                                DataTable dtExistSerial = objda.return_DataTable("Select TOP 1 * From Inv_StockBatchMaster where SerialNo ='" + dtTempSerial.Rows[lSerial]["serialno"].ToString() + "'  and Location_Id = '" + dt.Rows[count]["location_id"].ToString() + "' order by Trans_Id DESC");
                                bool bCheck = false;
                                try
                                {
                                    if (dtExistSerial.Rows.Count > 0)
                                    {
                                        if (dtOrderDetail.Rows[dCount][0].ToString() == dtExistSerial.Rows[0]["ProductId"].ToString())
                                        {
                                            int iResult = 0;

                                            if (bLocationAutoAdjust)
                                            {
                                                iResult = objda.execute_Command(" INSERT INTO Inv_StockBatchMaster VALUES (2,2,'" + dt.Rows[count]["location_id"].ToString() + "','" + dtOrderDetail.Rows[dCount][0].ToString() + "','SA','0','" + dtOrderDetail.Rows[dCount][1].ToString() + "','I','1','0','0',GETDATE(),'" + dtTempSerial.Rows[lSerial]["serialno"].ToString() + "',GETDATE(),'','','','','0','','','','','1',getdate(),'1',7591,GETDATE(),759,GETDATE())");

                                                iResult = 0;
                                                iResult = objda.execute_Command(" INSERT INTO Inv_StockBatchMaster VALUES (2,2,'" + dt.Rows[count]["location_id"].ToString() + "','" + dtOrderDetail.Rows[dCount][0].ToString() + "','SI','" + dt.Rows[count]["Trans_Id"].ToString() + "','" + dtOrderDetail.Rows[dCount][1].ToString() + "','O','1','0','0',GETDATE(),'" + dtTempSerial.Rows[lSerial]["serialno"].ToString() + "',GETDATE(),'','','','','0','','','','','1',getdate(),'1',7591,GETDATE(),759,GETDATE())");

                                            }

                                            if (iResult == 0)
                                            {
                                                DataRow row = dtExport.NewRow();
                                                row[0] = dt.Rows[count]["Invoice_No"].ToString();
                                                row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                                                row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                                                row[3] = dtTempSerial.Rows[lSerial]["serialno"].ToString();
                                                row[4] = "Record Not Saved Or Location Parameter Issue!";
                                                dtExport.Rows.Add(row);
                                                bCheck = false;
                                            }
                                            else
                                            {
                                                bCheck = false;
                                            }
                                        }
                                        else
                                        {
                                            DataRow row = dtExport.NewRow();
                                            row[0] = dt.Rows[count]["Invoice_No"].ToString();
                                            row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                                            row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                                            row[3] = dtTempSerial.Rows[lSerial]["serialno"].ToString();
                                            row[4] = "Found with Different Product";
                                            dtExport.Rows.Add(row);
                                            bCheck = false;
                                        }
                                    }
                                    else
                                    {
                                        int iResult = 0;
                                        iResult = objda.execute_Command(" INSERT INTO Inv_StockBatchMaster VALUES (2,2,'" + dt.Rows[count]["location_id"].ToString() + "','" + dtOrderDetail.Rows[dCount][0].ToString() + "','SI','" + dt.Rows[count]["Trans_Id"].ToString() + "','" + dtOrderDetail.Rows[dCount][1].ToString() + "','O','1','0','0',GETDATE(),'" + dtTempSerial.Rows[lSerial]["serialno"].ToString() + "',GETDATE(),'','','','','0','','','','','1',getdate(),'1',7591,GETDATE(),759,GETDATE())");

                                        if (iResult == 0)
                                        {
                                            DataRow row = dtExport.NewRow();
                                            row[0] = dt.Rows[count]["Invoice_No"].ToString();
                                            row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                                            row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                                            row[3] = dtTempSerial.Rows[lSerial]["serialno"].ToString();
                                            row[4] = "Record Not Saved!During New Serial Add Query";
                                            dtExport.Rows.Add(row);
                                            bCheck = false;
                                        }
                                        else
                                        {

                                            DataRow row = dtExport.NewRow();
                                            row[0] = dt.Rows[count]["Invoice_No"].ToString();
                                            row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                                            row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                                            row[3] = dtTempSerial.Rows[lSerial]["serialno"].ToString();
                                            row[4] = "New Serial Added";
                                            dtExport.Rows.Add(row);
                                            bCheck = false;

                                        }
                                    }
                                }
                                catch
                                {
                                    bCheck = true;
                                }

                                if (bCheck)
                                {
                                    DataRow row = dtExport.NewRow();
                                    row[0] = dt.Rows[count]["Invoice_No"].ToString();
                                    row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                                    row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                                    row[3] = dtTempSerial.Rows[lSerial]["serialno"].ToString();
                                    row[4] = "Serial Issue";
                                    dtExport.Rows.Add(row);
                                }

                            }



                            //   }

                        }
                    }
                    else
                    {
                        DataRow row = dtExport.NewRow();
                        row[0] = dt.Rows[count]["Invoice_No"].ToString();
                        row[1] = dt.Rows[count]["Ref_Order_Number"].ToString();
                        row[2] = dtOrderDetail.Rows[dCount][4].ToString();
                        row[3] = "";
                        row[4] = "Dispatch Department Not Scanned";
                        dtExport.Rows.Add(row);
                    }


                }


            }
        }
        LogWrite(dtExport);
    }
    public void LogWrite(DataTable dt)
    {


        ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        wbook.Worksheets.Add(dt, "tab1");
        // Prepare the response
        HttpResponse httpResponse = Response;
        httpResponse.Clear();
        httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //Provide you file name here
        httpResponse.AddHeader("content-disposition", "attachment;filename=\"SalesInvoice.xlsx\"");

        // Flush the workbook to the Response.OutputStream
        using (MemoryStream memoryStream = new MemoryStream())
        {
            wbook.SaveAs(memoryStream);
            memoryStream.WriteTo(httpResponse.OutputStream);
            memoryStream.Close();
        }

        httpResponse.End();

        //if (!Directory.Exists(Server.MapPath("\\LogFile")))
        //{
        //    Directory.CreateDirectory(Server.MapPath("\\LogFile"));
        //}
        //string txtFilename = "SIP" + ".txt";
        //string txtFilePath = @"\LogFile\" + txtFilename;
        ////Validate log file exist or not
        //if (!File.Exists(Server.MapPath(txtFilePath)))
        //{
        //    File.Create(Server.MapPath(txtFilePath));
        //}


        //StreamWriter file = new StreamWriter(Server.MapPath(txtFilePath), true);
        //for(int i=0;i< lst.Count;i++)
        //{
        //    file.Write(lst[i] + "\r\n");
        //}


        // file.Close();
    }
    protected void btnunpostinvoice_Click(object sender, EventArgs e)
    {
        if (ddlLocation.SelectedItem.ToString() == "All")
        {
            DisplayMessage("Please select one Location to continue");
            ddlLocation.Focus();
            return;
        }
        string strunpostid = string.Empty;
        if (GvSalesInvoice.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please Select Record");
                    return;
                }
                else
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        strunpostid = strunpostid + userdetails[i].ToString() + ",";
                    }
                }
            }
            else
            {
                DisplayMessage("Please Select Record");
                return;
            }
        }
        else
        {
            DisplayMessage("Record Not Found");
            return;
        }
        string sql = "select * from (select Inv_SalesInvoiceHeader.Invoice_No,Inv_SalesInvoiceHeader.Invoice_Date,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName,Inv_UnitMaster.Unit_Name,Inv_SalesInvoiceDetail.Quantity,case when (select Inv_StockDetail.Quantity from Inv_StockDetail where Inv_StockDetail.ProductId=Inv_SalesInvoiceDetail.Product_Id and Inv_StockDetail.Location_Id=Inv_SalesInvoiceHeader.Location_Id And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "') IS null then 0 else (select Inv_StockDetail.Quantity from Inv_StockDetail where Inv_StockDetail.ProductId=Inv_SalesInvoiceDetail.Product_Id and Inv_StockDetail.Location_Id=Inv_SalesInvoiceHeader.Location_Id And Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "') end as Sysqty,Inv_SalesInvoiceHeader.Trans_Id from Inv_SalesInvoiceHeader  inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No   inner join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id=Inv_ProductMaster.ProductId  inner join Inv_UnitMaster on Inv_SalesInvoiceDetail.Unit_Id=Inv_UnitMaster.Unit_Id   where Inv_SalesInvoiceHeader.Location_Id='" + ddlLocation.SelectedValue + "' and Inv_SalesInvoiceHeader.Post='False' and Inv_ProductMaster.ItemType='S' and ((select Inv_SalesOrderHeader.IsdeliveryVoucher from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Trans_Id=Inv_SalesInvoiceDetail.SIFromTransNo) is null or (select Inv_SalesOrderHeader.IsdeliveryVoucher from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Trans_Id=Inv_SalesInvoiceDetail.SIFromTransNo)='False' or (select Inv_SalesOrderHeader.IsdeliveryVoucher from Inv_SalesOrderHeader where Inv_SalesOrderHeader.Trans_Id=Inv_SalesInvoiceDetail.SIFromTransNo)=' ') and Inv_SalesInvoiceHeader.Trans_Id in (" + strunpostid.Substring(0, strunpostid.Length - 1) + ") )invoicePrint where invoicePrint.Sysqty=0 or invoicePrint.Quantity>invoicePrint.Sysqty order by invoicePrint.Trans_Id";
        DataTable dt = objda.return_DataTable(sql);
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }
        Session["UnPostedStock"] = dt;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales_Report/UnPostedSalesInvoice.aspx?LID=" + ddlLocation.SelectedValue + "','window','width=1024');", true);
        dt = null;
    }
    #endregion
    protected void lblgvCustomerName_Command(object sender, CommandEventArgs e)
    {
        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + e.CommandArgument.ToString() + "&&Page=SINV','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    #region SalesinvoiceDetail
    protected void lblgvSInvNo_Command(object sender, CommandEventArgs e)
    {
        if (!isSalesInvoicePermission())
        {
            DisplayMessage("User have no permission to view sales invoice ");
            return;
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales/SalesInvoice.aspx?Id=" + e.CommandArgument.ToString() + "')", true);
    }
    public bool isSalesInvoicePermission()
    {
        bool isAllow = false;

        //here we checking user permission for view sales order info 

        if (Session["EmpId"].ToString() == "0")
        {
            isAllow = true;
        }
        else
        {
            using (DataTable dtAllpagecode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "144", "92", Session["CompId"].ToString()))
            {
                if (dtAllpagecode.Rows.Count > 0)
                {
                    isAllow = true;
                }
            }

        }
        return isAllow;
    }
    #endregion
    public string[,] Tax_Insert_Into_Ac_Voucher_Detail_Debit(string strVoucher_No, string strSerial_No, string strAccount_No, string strOther_Account_No, string strRef_Id, string strRef_Type, string strCheque_Issue_Date, string strCheque_Clear_Date, string strCheque_No, string strDebit_Amount, string strCredit_Amount, string strNarration, string strCostCenter_ID, string strEmp_Id, string strCurrency_Id, string strExchange_Rate, string strForeign_Amount, string strCompanyCurrDebit, string strCompanyCurrCredit, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strExpensesId, ref SqlTransaction trns)
    {
        string[,] Expenses_Tax_Amount = new string[1, 5];
        try
        {
            double Debit_Amount = 0;
            double CompanyCurrDebit = 0;
            double Foreign_Amount = 0;
            double Credit_Amount = 0;
            double CompanyCurrCredit = 0;

            bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (IsTax == true)
            {
                DataTable Dt_Final_Save_Tax = Session["Dt_Final_Save_Tax "] as DataTable;
                if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
                {
                    string strNarration_Temp = string.Empty;
                    string strDebit_Amount_Temp = string.Empty;
                    string strCompanyCurrDebit_Temp = string.Empty;
                    string strForeign_Amount_Temp = string.Empty;
                    string strCredit_Amount_Temp = string.Empty;
                    string strCompanyCurrCredit_Temp = string.Empty;

                    foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                    {
                        if (strExpensesId == Dt_Row["Expenses_Id"].ToString())
                        {
                            strNarration_Temp = strNarration;

                            strDebit_Amount_Temp = Convert_Into_DF(((Convert.ToDouble(strDebit_Amount) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strForeign_Amount_Temp = Convert_Into_DF(((Convert.ToDouble(strForeign_Amount) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strCompanyCurrDebit_Temp = Convert_Into_DF(((Convert.ToDouble(strCompanyCurrDebit) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());

                            strCredit_Amount_Temp = Convert_Into_DF(((Convert.ToDouble(strCredit_Amount) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                            strCompanyCurrCredit_Temp = Convert_Into_DF(((Convert.ToDouble(strCompanyCurrCredit) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());

                            Debit_Amount = Debit_Amount + Convert.ToDouble(strDebit_Amount_Temp);
                            CompanyCurrDebit = CompanyCurrDebit + Convert.ToDouble(strCompanyCurrDebit_Temp);
                            Foreign_Amount = Foreign_Amount + Convert.ToDouble(strForeign_Amount_Temp);
                            Credit_Amount = Credit_Amount + Convert.ToDouble(strCredit_Amount_Temp);
                            CompanyCurrCredit = CompanyCurrCredit + Convert.ToDouble(strCompanyCurrCredit_Temp);

                            Expenses_Tax_Amount[0, 0] = Debit_Amount.ToString();
                            Expenses_Tax_Amount[0, 1] = CompanyCurrDebit.ToString();
                            Expenses_Tax_Amount[0, 2] = Foreign_Amount.ToString();
                            Expenses_Tax_Amount[0, 3] = Credit_Amount.ToString();
                            Expenses_Tax_Amount[0, 4] = CompanyCurrCredit.ToString();

                            strNarration_Temp = Convert_Into_DF(Dt_Row["Tax_Percentage"].ToString()) + "% " + Dt_Row["Tax_Type_Name"].ToString() + " On " + strNarration;
                            objVoucherDetail.InsertVoucherDetail
                                (Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(),
                                strVoucher_No,
                                strSerial_No,
                                Dt_Row["Tax_Account_Id"].ToString(),
                                strOther_Account_No,
                                strRef_Id,
                                strRef_Type,
                                strCheque_Issue_Date,
                                strCheque_Clear_Date,
                                strCheque_No,
                                strDebit_Amount_Temp,
                                strCredit_Amount_Temp,
                                strNarration_Temp,
                                strCostCenter_ID,
                                strEmp_Id,
                                strCurrency_Id,
                                strExchange_Rate,
                                strForeign_Amount_Temp,
                                strCompanyCurrDebit_Temp,
                                strCompanyCurrCredit_Temp,
                                strField1,
                                strField2,
                                strField3,
                                strField4,
                                strField5,
                                strField6,
                                strField7,
                                strIsActive,
                                strCreatedBy,
                                strCreatedDate,
                                strModifiedBy,
                                strModifiedDate,
                                ref trns);
                        }
                    }
                }
                Dt_Final_Save_Tax = null;
            }
            return Expenses_Tax_Amount;
        }
        catch { return Expenses_Tax_Amount; }
    }
    public string Get_Expenses_Tax(string Debit_Amount)
    {
        string Expenses_Tax_Amount = "0";
        try
        {
            bool IsTax = Inventory_Common.IsSalesTaxEnabled(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (IsTax == true)
            {
                DataTable Dt_Final_Save_Tax = Session["Dt_Final_Save_Tax "] as DataTable;
                if (Dt_Final_Save_Tax != null && Dt_Final_Save_Tax.Rows.Count > 0)
                {
                    string strDebit_Amount_Temp = string.Empty;
                    foreach (DataRow Dt_Row in Dt_Final_Save_Tax.Rows)
                    {
                        strDebit_Amount_Temp = Convert_Into_DF(((Convert.ToDouble(Debit_Amount.ToString()) * Convert.ToDouble(Dt_Row["Tax_Percentage"].ToString())) / 100).ToString());
                        Expenses_Tax_Amount = (Convert.ToDouble(Expenses_Tax_Amount) + Convert.ToDouble(strDebit_Amount_Temp)).ToString();
                    }
                }
                Dt_Final_Save_Tax = null;
            }
            return Expenses_Tax_Amount;
        }
        catch (Exception ex) { return "0"; }
    }
    public string Convert_Into_DF(string Amount)
    {
        try
        {
            if (Amount == "")
                Amount = "0";

            if (Session["Decimal_Count_For_Tax"].ToString() == "")
                Session["Decimal_Count_For_Tax"] = 0;

            if (Convert.ToInt32(Session["Decimal_Count_For_Tax"].ToString()) == 0)
            {
                string Decimal_Count = string.Empty;
                Decimal_Count = Session["LoginLocDecimalCount"].ToString();
                Session["Decimal_Count_For_Tax"] = Convert.ToInt32(Decimal_Count);
            }

            decimal Amount_D = Convert.ToDecimal((Convert.ToDouble(Amount)).ToString("F7"));
            Amount = Amount_D.ToString();
            int index = Amount.ToString().LastIndexOf(".");
            if (index > 0)
                Amount = Amount.Substring(0, index + (Convert.ToInt32(Session["Decimal_Count_For_Tax"].ToString()) + 1));
            return Amount;
        }
        catch
        {
            return "0";
        }
    }
    public string SetDecimal(string amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(Session["LocCurrencyId"].ToString(), amount);
    }
    protected void lnkCustomerAccountMaster_Command(object sender, CommandEventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;
        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string CustomerId = arguments[0];
        //GridViewRow gr = (GridViewRow)((LinkButton)sender).Parent.Parent;
        UcAcMaster.setUcAcMasterValues(CustomerId, "Customer", myButton.Text);
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "Modal_AcMaster_Open()", true);
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "text", "$('#ModelAcMaster').modal('toggle');" ,true);
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static bool hasSalesInvoicePermission()
    {
        bool isAllow = false;
        if (HttpContext.Current.Session["EmpId"].ToString() == "0")
        {
            isAllow = true;
        }
        else
        {
            using (DataTable dtAllpagecode = new Common(HttpContext.Current.Session["DBConnection"].ToString()).GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), "144", "92", HttpContext.Current.Session["CompId"].ToString()))
            {
                if (dtAllpagecode.Rows.Count > 0)
                {
                    isAllow = true;
                }
            }
        }
        return isAllow;
    }
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid(ddlLocation.SelectedValue);
    }
    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new ListItem("All", Session["LocId"].ToString()));
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            if (dtLoc.Rows.Count > 1 && LocIds != "")
            {
                ddlLocation.Items.Insert(0, new ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        dtLoc = null;
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUser();
        FillGrid(ddlLocation.SelectedValue);
    }

    public void FillUser()
    {
        try
        {
            EmployeeMaster objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
            UserMaster ObjUser = new UserMaster(Session["DBConnection"].ToString());

            string strEmpId = string.Empty;
            string strLocationDept = string.Empty;
            string strLocId = Session["LocId"].ToString();

            strLocId = ddlLocation.SelectedValue;
            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept);


            DataTable dtEmp = new DataTable();

            string isSingle = ObjUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString());
            bool IsSingleUser = false;
            try
            {
                IsSingleUser = Convert.ToBoolean(isSingle);
            }
            catch
            {
                IsSingleUser = false;
            }

            // can see multiple employee data
            if (IsSingleUser == false)
            {
                //for normal user
                if (Session["EmpId"].ToString() != "0")
                {
                    dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), strEmpId);
                    //dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    //for super admin
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                        dtEmp = new DataView(dtEmp, "Location_Id=" + ddlLocation.SelectedValue.Trim() + "", "emp_name asc", DataViewRowState.CurrentRows).ToTable();
                    }
                }
            }
            else
            {
                dtEmp = objEmployee.GetEmployeeMasterOnRole(Session["CompId"].ToString(), Session["EmpId"].ToString());
            }
            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_name";
            ddlUser.DataValueField = "Emp_id";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("--Select User--", "--Select User--"));
        }
        catch
        {

        }
    }

    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";
        DataTable dtEmp = objda.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id in (" + strLocationId + ") and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");
        if (dtEmp.Rows[0][0] != null)
        {
            strEmpList = dtEmp.Rows[0][0].ToString();
        }
        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;

    }
    protected void ddlUser_Click(object sender, EventArgs e)
    {
        FillGrid(ddlLocation.SelectedValue);
    }
}