using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Collections;
using System.Data.SqlClient;

public partial class Inventory_Inventory_Closing : System.Web.UI.Page
{
    SystemParameter ObjSys = null;
    DataAccessClass objDa = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_FinancialYear_Detail objFinanceDetail = null;
    Ac_FinancialYear_Closing_Detail objClosingDetail = null;
    Inv_StockDetail objstockDetail = null;
    Inv_ProductLedger ObjProductLadger = null;
    LocationMaster objLocation = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objFinanceDetail = new Ac_FinancialYear_Detail(Session["DBConnection"].ToString());
        objClosingDetail = new Ac_FinancialYear_Closing_Detail(Session["DBConnection"].ToString());
        objstockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "344", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
           

            txtFinanceCode.Text = Session["FinanceCode"].ToString();
            txtFromdate.Text = Convert.ToDateTime(Session["FinanceFromdate"].ToString()).ToString(ObjSys.SetDateFormat());
            txTodate.Text = Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString(ObjSys.SetDateFormat());
        }
    }


    public string SetDecimal(string amount)
    {
        return ObjSys.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }


    protected void btnGet_Click(object sender, EventArgs e)
    {
        DataTable DtTemp = new DataTable();
        DataTable dTTempHeader = new DataTable();
        string strStatus = string.Empty;
        DataTable dtFinanceDetail = objFinanceDetail.GetAllDataByHeader_Id(HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtFinanceheader = objFYI.GetInfoAllTrue(Session["CompId"].ToString());
        try
        {
            strStatus = new DataView(dtFinanceheader, "Trans_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Status"].ToString();
        }
        catch
        {

        }
        if (strStatus.Trim().ToUpper() == "NEW" || strStatus.Trim().ToUpper() == "CLOSE")
        {
            DisplayMessage("you can not close current Financial year");
            return;
        }
        else if (strStatus.Trim().ToUpper() == "REOPEN")
        {

            if (new DataView(dtFinanceheader, "From_Date>='" + Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString() + "' and Status='Open'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            {
                DisplayMessage("Open Financial year Not Found");
                return;
            }
            else
            {
                Session["NextFinancialYearId"] = new DataView(dtFinanceheader, "From_Date>='" + Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString() + "' and Status='Open'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
            }



            DtTemp = new DataView(dtFinanceDetail, "Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Inv_Status='ReOpen'", "", System.Data.DataViewRowState.CurrentRows).ToTable();

            if (DtTemp.Rows.Count == 0)
            {
                DisplayMessage("Inventory already closed for this financial year and location");
                return;
            }
            else
            {
                Session["FinancialYearStatus"] = "REOPEN";
                hdnTransId.Value = DtTemp.Rows[0]["Trans_Id"].ToString();
            }
        }
        else if (strStatus.Trim().ToUpper() == "OPEN")
        {

            if (new DataView(dtFinanceheader, "Trans_Id<>" + HttpContext.Current.Session["FinanceYearId"].ToString() + " and Status='ReOpen'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("First Close Your ReOpen Year then you can close the Same");
                return;
            }

            if (new DataView(dtFinanceheader, "From_Date>='" + Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString() + "' and Status='New'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            {
                DisplayMessage("Next Financial Year Not Found");
                return;
            }
            else
            {
                Session["NextFinancialYearId"] = new DataView(dtFinanceheader, "From_Date>='" + Convert.ToDateTime(Session["FinanceTodate"].ToString()).ToString() + "' and Status='New'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
            }



            DtTemp = new DataView(dtFinanceDetail, "Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' and Inv_Status='Open'", "", System.Data.DataViewRowState.CurrentRows).ToTable();

            if (DtTemp.Rows.Count == 0)
            {
                DisplayMessage("Inventory already closed for this financial year and location");
                return;
            }
            else
            {

                Session["FinancialYearStatus"] = "OPEN";
                hdnTransId.Value = DtTemp.Rows[0]["Trans_Id"].ToString();
            }
        }




        //here we get stock closing quanity and stock value

        DataTable dtStock = objDa.return_DataTable("select  ISNULL( SUM(quantity),0) as Closing_stock_Qty,   ISNULL(   sum(Quantity* CAST(inv_stockdetail.Field2 as decimal(18,6))),0) as Closing_stock_Value   from Inv_StockDetail  inner join Inv_ProductMaster on Inv_StockDetail.ProductId=Inv_ProductMaster.ProductId   inner join Inv_UnitMaster on Inv_ProductMaster.UnitId=Inv_UnitMaster.Unit_Id   inner join Set_LocationMaster on Inv_StockDetail.Location_Id=Set_LocationMaster.Location_Id and Inv_StockDetail.Location_Id=" + Session["LocId"].ToString() + " and Inv_StockDetail.Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + " and  Inv_ProductMaster.ItemType='S' ");

        if (dtStock.Rows.Count > 0)
        {

            lblClosingqty.Text = SetDecimal(dtStock.Rows[0]["Closing_stock_Qty"].ToString());
            lblClosingValue.Text = SetDecimal(dtStock.Rows[0]["Closing_stock_Value"].ToString());

        }

        //or Inv_Status='ReOpen')
        pnlclosing.Visible = true;

        gvunPostedRecord.DataSource = GetUnpostedRecord();
        gvunPostedRecord.DataBind();


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

    public DataTable GetUnpostedRecord()
    {
        DateTime ToDate = Convert.ToDateTime(txTodate.Text);
        ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);


        DataTable dt = new DataTable();

        dt.Columns.Add("PageName");
        dt.Columns.Add("UnpostRecord", typeof(Int32));

        DataRow dr;
        //for transfer
        dr = dt.NewRow();

        dr[0] = "Transfer Voucher";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_TransferHeader where FromLocationID=" + Session["LocId"].ToString() + " and Post='N' and IsActive='True' and TDate>='" + txtFromdate.Text + "' and TDate<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);

        //for adjustment
        dr = dt.NewRow();

        dr[0] = "Stock Adjustment";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_AdjustHeader where FromLocationID=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False' and Vdate>='" + txtFromdate.Text + "' and Vdate<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //for adjustment
        dr = dt.NewRow();

        dr[0] = "Physical Inventory";
        dr[1] = objDa.return_DataTable("select COUNT( *) from Inv_PhysicalHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False' and Vdate>='" + txtFromdate.Text + "' and Vdate<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //purchase Invoice
        dr = dt.NewRow();

        dr[0] = "Purchase Invoice";
        dr[1] = objDa.return_DataTable("select COUNT ( *) from Inv_PurchaseInvoiceHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False' and InvoiceDate>='" + txtFromdate.Text + "' and InvoiceDate<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //purchase Return
        dr = dt.NewRow();

        dr[0] = "Purchase Return";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_PurchaseReturnHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Field1='False' and PRDate>='" + txtFromdate.Text + "' and PRDate<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);


        //Sales Invoice
        dr = dt.NewRow();

        dr[0] = "Sales Invoice";
        dr[1] = objDa.return_DataTable("select count(*) from Inv_SalesInvoiceHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False' and Invoice_Date>='" + txtFromdate.Text + "' and Invoice_Date<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //Delivery Voucher
        dr = dt.NewRow();

        dr[0] = "Delivery Voucher";
        dr[1] = objDa.return_DataTable("select  COUNT(*) from Inv_SalesDeliveryVoucher_Header where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='false' and Voucher_Date>='" + txtFromdate.Text + "' and Voucher_Date<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);



        //Sales Return
        dr = dt.NewRow();

        dr[0] = "Sales Return";
        dr[1] = objDa.return_DataTable("select COUNT(*) from Inv_SalesReturnHeader where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Post='False' and Return_Date>='" + txtFromdate.Text + "' and Return_Date<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);


        //Sales Return
        dr = dt.NewRow();

        dr[0] = "Production Voucher";
        dr[1] = objDa.return_DataTable("select COUNT( *) from Inv_Production_Process where Location_Id=" + Session["LocId"].ToString() + " and IsActive='True' and Is_Post='False' and Job_Creation_Date>='" + txtFromdate.Text + "' and Job_Creation_Date<='" + ToDate.ToString() + "'").Rows[0][0].ToString();

        dt.Rows.Add(dr);




        dt = new DataView(dt, "UnpostRecord<>0", "", DataViewRowState.CurrentRows).ToTable();


        return dt;

    }

    protected void btnClosing_Click(object sender, EventArgs e)
    {

        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        gvunPostedRecord.DataSource = GetUnpostedRecord();
        gvunPostedRecord.DataBind();

        if (gvunPostedRecord.Rows.Count > 0)
        {
            DisplayMessage("All Voucher should be post and confirm closing stock quantity and value before closing");
            return;
        }


        btnStockReport_Click(null, null);



        DataTable dtStock = (DataTable)((ArrayList)Session["DtProductStock"])[2];

        int b = 0;



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();


        try
        {

            //foreach (DataRow dr in dtStock.Rows)
            //{
            //    if (Session["FinancialYearStatus"].ToString() == "OPEN")
            //    {


            //        b = objstockDetail.InsertStockDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["ProductId"].ToString(), dr["Quantity"].ToString(), "0", "0", "0", "0", "0", "0", "0", "0", dr["LastPrice"].ToString(), dr["UnitCost"].ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["NextFinancialYearId"].ToString(), ref trns);


            //        b = ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", "0", dr["ProductId"].ToString(), dr["Unit_Id"].ToString(), "I", "0", "0", dr["Quantity"].ToString(), "0", "1/1/1800", dr["LastPrice"].ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["NextFinancialYearId"].ToString(), ref trns);
            //    }
            //    else if (Session["FinancialYearStatus"].ToString() == "REOPEN")
            //    {
            //        double NextFianceOpeningStock = 0;
            //        double CurrentStock = 0;

            //        double NextYearStock = 0;
            //        try
            //        {
            //            NextFianceOpeningStock = Convert.ToDouble(objstockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["NextFinancialYearId"].ToString(), dr["ProductId"].ToString(), ref trns).Rows[0]["OpeningBalance"].ToString());
            //        }
            //        catch
            //        {

            //        }


            //        try
            //        {
            //            NextYearStock = Convert.ToDouble(objstockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["NextFinancialYearId"].ToString(), dr["ProductId"].ToString(), ref trns).Rows[0]["Quantity"].ToString());
            //        }
            //        catch
            //        {

            //        }

            //        try
            //        {
            //            CurrentStock = Convert.ToDouble(dr["Quantity"].ToString());
            //        }
            //        catch
            //        {

            //        }

            //        if ((0 - NextFianceOpeningStock + CurrentStock) != 0)
            //        {

            //            objDa.execute_Command("delete from Inv_ProductLedger where TransType='OP' and Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);

            //            objDa.execute_Command("update Inv_StockDetail set Quantity=" + (NextYearStock - NextFianceOpeningStock).ToString() + " where Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);



            //            b = ObjProductLadger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "OP", "0", "0", dr["ProductId"].ToString(), dr["Unit_Id"].ToString(), "I", "0", "0", (CurrentStock).ToString(), "0", "1/1/1800", dr["LastPrice"].ToString(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["NextFinancialYearId"].ToString(), ref trns);

            //            objDa.execute_Command("update Inv_StockDetail set OpeningBalance=" + (CurrentStock).ToString() + " where Location_Id=" + Session["LocId"].ToString() + " and Finance_Year_Id=" + HttpContext.Current.Session["NextFinancialYearId"].ToString() + " and ProductId=" + dr["ProductId"].ToString() + "", ref trns);
            //        }
            //    }
            //}

            string strsql = "update Ac_FinancialYear_Detail set Inv_Status='Close',Inv_Closed_By=" + Session["EmpId"].ToString() + ",Inv_Closed_Date='" + DateTime.Now.ToString() + "' where Trans_Id=" + hdnTransId.Value + "";
            objDa.execute_Command(strsql, ref trns);


            objClosingDetail.InsertFinancialYearClosingDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnTransId.Value, "I", "0", lblClosingValue.Text, lblClosingqty.Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", ref trns);

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            trns.Dispose();
            con.Dispose();
            DisplayMessage("Inventory closed successfully");
            pnlclosing.Visible = false;
            return;
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

    protected void btnStockReport_Click(object sender, EventArgs e)
    {
        string GroupBy = string.Empty;
        DataTable dtFilter = new DataTable();

        InventoryDataSet rptdata = new InventoryDataSet();

        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_StockDetail_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_StockDetail_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_StockDetail_SelectRow_Report);

        dtFilter = rptdata.sp_Inv_StockDetail_SelectRow_Report;


        try
        {
            dtFilter = new DataView(dtFilter, "Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        ArrayList arr = new ArrayList();


        string strGroupBy = string.Empty;
        string strReporttItle = string.Empty;

        strGroupBy = "0";
        strReporttItle = "Stock Report for " + ((Label)Master.FindControl("Lbl_Location")).Text + " Location";

        arr.Add(strReporttItle);
        arr.Add(strGroupBy);
        arr.Add(dtFilter);

        Session["DtProductStock"] = arr;

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/ProductStockPrint.aspx','window','width=1024');", true);
    }
}