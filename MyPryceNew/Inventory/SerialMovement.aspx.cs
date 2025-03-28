using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.IO;

public partial class Inventory_SerialMovement : System.Web.UI.Page
{
    Inv_StockBatchMaster objstockbatch = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    Inv_ProductMaster objProductM = null;
    IT_ObjectEntry objObjectEntry = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    Inv_StockDetail objStockDetail = null;
    DataAccessClass objda = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objstockbatch = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/SerialMovement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            btnExportExcel.Visible = clsPagePermission.bDownload;

            Session["DtSerial"] = null;
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtToDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            FillddlLocation();
            lblValueAvailableserialstock.Text = "0";
            lblValueAvailablestock.Text = "0";
            lblValueExceptionalserialstock.Text = "0";
        }
        //New Code created by jitendra on 09-12-2014
    }
    
    protected void gvserailMovement_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtSerial"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvserailMovement, dt, "", "");
    }
    #region Filter
    protected void btngo_Click(object sender, EventArgs e)
    {
        lblValueAvailableserialstock.Text = "0";
        lblValueAvailablestock.Text = "0";
        lblValueExceptionalserialstock.Text = "0";
        if (txtProductName.Text.Trim() == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Focus();
            return;
        }


        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Invalid Date Format");
                txtFromDate.Focus();
                return;
            }


            if (txtToDate.Text == "")
            {

                DisplayMessage("To Date should be necessary for filter the record");
                txtToDate.Focus();
                return;
            }
        }

        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Invalid Date Format");
                txtToDate.Focus();
                return;
            }


            if (txtFromDate.Text == "")
            {

                DisplayMessage("From Date should be necessary for filter the record");
                txtToDate.Focus();
                return;
            }
        }

        string strselectquery = string.Empty;


        string strsql = string.Empty;

        string strtotalInquery = string.Empty;

        string strtotalOutquery = string.Empty;

        string strwhere = string.Empty;

        DataTable dt = new DataTable();
        //DataTable dt = objstockbatch.GetStockBatchMasterAll_By_ProductId(hdnNewProductId.Value);



        strselectquery = "Select [Company_Id], [Brand_Id], [Location_Id], [Trans_Id], [TransType], [TransTypeId], [ProductId], [UnitId], [InOut], CAST( Quantity as nvarchar(max)) as Quantity , [LotNo], [BatchNo], [ExpiryDate], [SerialNo], [ManufacturerDate], [Barcode], [Width], [Length], [Pallet_ID], [Field1], [Field2], [Field3], [Field4], [Field5], [Field6], [Field7], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate] , CONVERT(VARCHAR(11),Inv_StockBatchMaster.CreatedDate,106) as VoucherDate, (select Inv_ProductMaster.ProductCode from Inv_ProductMaster where Inv_ProductMaster.ProductId=Inv_StockBatchMaster.ProductId) as ProductCode, (select Inv_ProductMaster.EProductName from Inv_ProductMaster where Inv_ProductMaster.ProductId=Inv_StockBatchMaster.ProductId) as ProductName, (select Inv_UnitMaster.Unit_Name from Inv_UnitMaster where Inv_UnitMaster.Unit_Id=Inv_StockBatchMaster.UnitId) as UnitName , case when Inv_StockBatchMaster.TransType='SI' THEN 'Sales Invoice' when Inv_StockBatchMaster.TransType='SR' THEN 'Sales Return' when Inv_StockBatchMaster.TransType='PR' THEN 'Purchase Return' when Inv_StockBatchMaster.TransType='TI' THEN 'Transfer IN' when Inv_StockBatchMaster.TransType='TO' THEN 'Transfer Out' when Inv_StockBatchMaster.TransType='PG' THEN 'Purchase Goods' when Inv_StockBatchMaster.TransType='SA' THEN 'Stock Adjustment' when Inv_StockBatchMaster.TransType='OP' THEN 'Opening Stock' when Inv_StockBatchMaster.TransType='FO' THEN 'Production Out' when Inv_StockBatchMaster.TransType='FI' THEN 'Production In' when Inv_StockBatchMaster.TransType='FR' THEN 'Production Return' when Inv_StockBatchMaster.TransType='DV' THEN 'Delivery Voucher' end as RefferenceType , Case When Inv_StockBatchMaster.TransType='SI' Then cast((Select Invoice_No From Inv_SalesInvoiceHeader where Trans_Id=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) When Inv_StockBatchMaster.TransType='SR' Then Cast((Select Return_No From Inv_SalesReturnHeader where Trans_Id=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) When Inv_StockBatchMaster.TransType='PR' Then Cast((Select PReturn_No From Inv_PurchaseReturnHeader where Trans_Id=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) When Inv_StockBatchMaster.TransType='TI' Then Cast((Select VoucherNo From Inv_TransferHeader where Trans_Id=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) When Inv_StockBatchMaster.TransType='TO' Then Cast((Select VoucherNo From Inv_TransferHeader where Trans_Id=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) When Inv_StockBatchMaster.TransType='SA' Then Cast((Select VoucherNo From Inv_AdjustHeader where TransId=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) when Inv_StockBatchMaster.TransType='PG' THEN Cast((Select InvoiceNo From dbo.Inv_PurchaseInvoiceHeader where TransId=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) when Inv_StockBatchMaster.TransType='FO' or Inv_StockBatchMaster.TransType='FI' or Inv_StockBatchMaster.TransType='FR' THEN Cast((Select Inv_Production_Process.Job_No From dbo.Inv_Production_Process where Inv_Production_Process.Id=Inv_StockBatchMaster. TransTypeId)as Nvarchar(Max)) when Inv_StockBatchMaster.TransType='DV' THEN Cast((Select Inv_SalesDeliveryVoucher_Header.Voucher_No From dbo.Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.Trans_Id=Inv_StockBatchMaster.TransTypeId)as Nvarchar(Max)) end as RefferenceId , case when Inv_StockBatchMaster.InOut='o' THEN 'Out' ELSE 'In' end as Status, case when Inv_StockBatchMaster.InOut='I' THEN inv_stockbatchmaster.Quantity else 0 end as InQty, case when Inv_StockBatchMaster.InOut='O' THEN inv_stockbatchmaster.Quantity else 0 end as OutQty,";


        strtotalInquery = " ((select ISNULL( SUM( isnull(sb.Quantity,0)),0) from Inv_StockBatchMaster as SB where sb.Company_Id=Inv_StockBatchMaster.Company_Id and sb.Brand_Id=Inv_StockBatchMaster.Brand_Id  and sb.ProductId=Inv_StockBatchMaster.ProductId and sb.InOut='I' and SB.Trans_Id<= Inv_StockBatchMaster.Trans_Id";

        strtotalOutquery = " - (select ISNULL( SUM(isnull(sb.Quantity,0)),0) from Inv_StockBatchMaster as SB where sb.Company_Id=Inv_StockBatchMaster.Company_Id and sb.Brand_Id=Inv_StockBatchMaster.Brand_Id and sb.ProductId=Inv_StockBatchMaster.ProductId and sb.InOut='O' and SB.Trans_Id<= Inv_StockBatchMaster.Trans_Id";


        strwhere = "  (select substring(Set_LocationMaster.Location_Name,0,15)+'..' from Set_LocationMaster where Set_LocationMaster.Location_Id=Inv_StockBatchMaster.Location_Id) as Location , case when(TransType='Dv') then (select Ems_ContactMaster.Name from Inv_SalesDeliveryVoucher_Header inner join Ems_ContactMaster on Inv_SalesDeliveryVoucher_Header.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_SalesDeliveryVoucher_Header.Trans_Id=Inv_StockBatchMaster.TransTypeId ) when(TransType='SI') then (select Ems_ContactMaster.Name from Inv_SalesInvoiceHeader inner join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id=Ems_ContactMaster.Trans_Id where Inv_SalesInvoiceHeader.Trans_Id=Inv_StockBatchMaster.TransTypeId ) when(TransType='PG') then (select Ems_ContactMaster.Name from Inv_PurchaseInvoiceHeader inner join Ems_ContactMaster on Inv_PurchaseInvoiceHeader.SupplierId=Ems_ContactMaster.Trans_Id where Inv_PurchaseInvoiceHeader.TransId=Inv_StockBatchMaster.TransTypeId )    when (TransType='PR') then (select Ems_ContactMaster.Name from Inv_PurchaseInvoiceHeader inner join Ems_ContactMaster on Inv_PurchaseInvoiceHeader.SupplierId=Ems_ContactMaster.Trans_Id where Inv_PurchaseInvoiceHeader.TransId=  (select Inv_PurchaseReturnHeader.Invoice_Id from Inv_PurchaseReturnHeader where Inv_PurchaseReturnHeader.Trans_Id=Inv_StockBatchMaster.TransTypeId ))     when(TransType='FI' or TransType='FO') then (select Ems_ContactMaster.Name from Inv_Production_Process inner join Inv_ProductionRequestHeader on Inv_Production_Process.Ref_Production_Req_No=Inv_ProductionRequestHeader.Trans_Id inner join Ems_ContactMaster on Inv_ProductionRequestHeader.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_Production_Process.Id=Inv_StockBatchMaster.TransTypeId ) when(TransType='SR') then (select Ems_ContactMaster.Name from Inv_SalesReturnHeader inner join Ems_ContactMaster on Inv_SalesReturnHeader.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_SalesReturnHeader.Trans_Id=Inv_StockBatchMaster.TransTypeId )  end as ContactName, case when(TransType='PG') then (select Inv_PurchaseInvoiceHeader.SupInvoiceNo from Inv_PurchaseInvoiceHeader where Inv_PurchaseInvoiceHeader.TransId=Inv_StockBatchMaster.TransTypeId ) else '0' end as Supplier_Invoice_No , case when(TransType='PG') then (select CONVERT(VARCHAR(11),Inv_PurchaseInvoiceHeader.SupInvoiceDate,106) from Inv_PurchaseInvoiceHeader where Inv_PurchaseInvoiceHeader.TransId=Inv_StockBatchMaster.TransTypeId ) else ' ' end as Supplier_Invoice_Date From Inv_StockBatchMaster where ProductId=" + hdnNewProductId.Value + "";
        //strbalancequery = " ((select ISNULL( SUM( isnull(sb.Quantity,0)),0) from Inv_StockBatchMaster as SB where sb.Company_Id=Inv_StockBatchMaster.Company_Id and sb.Brand_Id=Inv_StockBatchMaster.Brand_Id  and sb.ProductId=Inv_StockBatchMaster.ProductId and sb.InOut='I' and SB.Trans_Id<= Inv_StockBatchMaster.Trans_Id)-(select ISNULL( SUM(isnull(sb.Quantity,0)),0) from Inv_StockBatchMaster as SB where sb.Company_Id=Inv_StockBatchMaster.Company_Id and sb.Brand_Id=Inv_StockBatchMaster.Brand_Id and sb.ProductId=Inv_StockBatchMaster.ProductId and sb.InOut='O' and SB.Trans_Id<= Inv_StockBatchMaster.Trans_Id)) as BalanceQty,";




        if (ddlLocation.SelectedIndex != 0)
        {
            strtotalInquery = strtotalInquery + " and sb.Location_Id=" + ddlLocation.SelectedValue + "";

            strtotalOutquery = strtotalOutquery + " and sb.Location_Id=" + ddlLocation.SelectedValue + " ";

            strwhere = strwhere + " and Inv_StockBatchMaster.Location_Id=" + ddlLocation.SelectedValue + "";
        }

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            strtotalInquery = strtotalInquery + " and sb.CreatedDate>='" + txtFromDate.Text + "' and sb.CreatedDate<='" + ToDate.ToString() + "'";

            strtotalOutquery = strtotalOutquery + " and sb.CreatedDate>='" + txtFromDate.Text + "' and sb.CreatedDate<='" + ToDate.ToString() + "'";

            strwhere = strwhere + " and Inv_StockBatchMaster.CreatedDate>='" + txtFromDate.Text + "' and Inv_StockBatchMaster.CreatedDate<='" + ToDate.ToString() + "'";

        }

        if (txtSerialNo.Text != "")
        {

            strtotalInquery = strtotalInquery + " and sb.SerialNo='" + txtSerialNo.Text + "'";

            strtotalOutquery = strtotalOutquery + " and sb.SerialNo='" + txtSerialNo.Text + "'";

            strwhere = strwhere + " and Inv_StockBatchMaster.SerialNo='" + txtSerialNo.Text + "'";
        }

        strsql = strselectquery + " " + strtotalInquery + " ) " + strtotalOutquery + " )) as BalanceQty ," + strwhere + " order by Inv_StockBatchMaster.Trans_Id";
        
        dt = objda.return_DataTable(strsql);

        try
        {
            lblValueAvailableserialstock.Text = SetDecimal(dt.Rows[dt.Rows.Count - 1]["BalanceQty"].ToString());
        }
        catch
        {

        }

       

        try
        {
            dt.DefaultView.Sort = "CreatedDate";

            //dt = dt.DefaultView.ToTable(true, "ProductCode", "ContactName", "UnitName", "RefferenceType", "RefferenceId", "Supplier_Invoice_No", "Supplier_Invoice_Date", "Status", "InQty", "OutQty","BalanceQty", "VoucherDate", "SerialNo", "Location");
            dt = dt.DefaultView.ToTable(true, "ProductCode", "ContactName", "UnitName", "RefferenceType", "RefferenceId", "Status", "InQty", "OutQty", "BalanceQty", "VoucherDate", "SerialNo", "Location","ModifedDate");
            //dt.Columns["ContactName"].ColumnName = "Customer/Supplier";
        }

        catch
        {

        }

        Session["DtSerial"] = dt;
        objPageCmn.FillData((object)gvserailMovement, dt, "", "");
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        float Inqty = 0;
        float Outqty = 0;
        foreach (GridViewRow gvrow in gvserailMovement.Rows)
        {
            Label lblQtyIn = (Label)gvrow.FindControl("bllInQty");
            Label lblQtyout = (Label)gvrow.FindControl("bllOutQty");

            Inqty += float.Parse(lblQtyIn.Text);
            Outqty += float.Parse(lblQtyout.Text);
        }

        try
        {
            ((Label)gvserailMovement.FooterRow.FindControl("txttotQuantityIn")).Text = SetDecimal(Inqty.ToString());
            ((Label)gvserailMovement.FooterRow.FindControl("txttotQuantityOut")).Text = SetDecimal(Outqty.ToString());

            ((Label)gvserailMovement.FooterRow.FindControl("txttotQuantityBalance")).Text = SetDecimal((Inqty - Outqty).ToString());
        }
        catch
        {

        }

        //here we get footer information


        string strstockquery = string.Empty;
        try
        {
            if (ddlLocation.SelectedIndex == 0)
            {
                strstockquery = "select sum(Quantity) from inv_stockdetail where productid=" + hdnNewProductId.Value + " and Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";
            }
            else
            {
                strstockquery = "select sum(Quantity) from inv_stockdetail where productid=" + hdnNewProductId.Value + " and Location_Id=" + ddlLocation.SelectedValue + " and  Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "";
            }

            lblValueAvailablestock.Text = SetDecimal(objda.return_DataTable(strstockquery).Rows[0][0].ToString());
        }
        catch
        {
            lblValueAvailablestock.Text = "0";
        }


        try
        {

            lblValueExceptionalserialstock.Text = SetDecimal((float.Parse(lblValueAvailablestock.Text) - float.Parse(lblValueAvailableserialstock.Text)).ToString());
        }
        catch
        {

        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductcode.Text = "";
        txtProductName.Text = "";
        txtSerialNo.Text = "";
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        objPageCmn.FillData((object)gvserailMovement, null, "", "");
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
        Session["DtSerial"] = null;
        lblValueAvailableserialstock.Text = "0";
        lblValueAvailablestock.Text = "0";
        lblValueExceptionalserialstock.Text = "0";
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text.Trim() != "")
        {

            DataTable dtProduct = new DataTable();
          
            dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.ToString());

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {

                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
            }
            else
            {
                DisplayMessage("Product Not Found !");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
                return;
            }

        }
        else
        {

            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {


        TextBox txtSender = (TextBox)sender;

        if (txtSender.Text.Trim() != "")
        {

            DataTable dtProduct = new DataTable();


            //}
            dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSender.Text.Trim());



            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                ////updated by jitendra upadhyay
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();


            }
            else
            {
                DisplayMessage("Product Not Found !");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtSender.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
    }

    protected void txtSerialNo_TextChanged(object sender, EventArgs e)
    {
        if (txtSerialNo.Text != "")
        {

            DataTable dtProduct = new DataTable();


            string strsql = "select Inv_StockBatchMaster.ProductId,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName from Inv_StockBatchMaster inner join Inv_ProductMaster on Inv_StockBatchMaster.ProductId=Inv_ProductMaster.ProductId where  Inv_StockBatchMaster.SerialNo='" + txtSerialNo.Text.Trim() + "'";
            //}
            //dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSender.Text.Trim());

            dtProduct = objda.return_DataTable(strsql);

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                ////updated by jitendra upadhyay
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();


            }
            else
            {
                DisplayMessage("Product Not Found !");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                txtSerialNo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
                return;
            }


        }
        else
        {
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
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
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "Location_Name", DataViewRowState.CurrentRows).ToTable();



        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();

            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, "All");
            ddlLocation.SelectedValue = Session["LocId"].ToString();

        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        string[] str = new string[0];
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (dt != null)
        {
            str = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();

            }

        }

        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        string[] txt = new string[0];
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (dt != null)
        {
            txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
        }


        return txt;
    }

    #endregion
    public string SetDecimal(string Amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Amount);
    }

    public string GetProductStock(string strProductId)
    {
        string SysQty = string.Empty;
        try
        {
            SysQty = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Quantity"].ToString();
        }
        catch
        {
            SysQty = "0";
        }


        if (SysQty == "")
        {
            SysQty = "0.000";
        }

        return SetDecimal(SysQty);
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {        
        if(Session["DtSerial"]!=null)
        {
            DataTable dt = Session["DtSerial"] as DataTable;
         
            dt.Columns["ProductCode"].SetOrdinal(0);
            dt.Columns["ContactName"].SetOrdinal(1);
            dt.Columns["UnitName"].SetOrdinal(2);
            dt.Columns["RefferenceType"].SetOrdinal(3);
            dt.Columns["RefferenceId"].SetOrdinal(4);
            dt.Columns["VoucherDate"].SetOrdinal(5);
            dt.Columns["InQty"].SetOrdinal(6);
            dt.Columns["OutQty"].SetOrdinal(7);
            dt.Columns["BalanceQty"].SetOrdinal(8);
            dt.Columns["SerialNo"].SetOrdinal(9);
            dt.Columns["Location"].SetOrdinal(10);
            dt.Columns.RemoveAt(11);

            ExportTableData(dt, "SerialMovement");

        }
    }

    public void ExportTableData(DataTable dtdata, string filename)
    {
        string strFname = filename;
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

}