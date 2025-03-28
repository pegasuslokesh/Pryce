using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.IO;

public partial class Purchase_BackOrder : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    LocationMaster objLocation = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../purchase/backorder.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillGrid();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSOrderSave.Visible = clsPagePermission.bAdd;
        GvBackOrder.Columns[0].Visible = clsPagePermission.bEdit;
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "PODate" || ddlFieldName.SelectedItem.Value == "DeliveryDate")
        {
            txtValueDate.Visible = true;
            txtValue.Visible = false;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
        else
        {
            txtValueDate.Visible = false;
            txtValue.Visible = true;
            txtValue.Text = "";
            txtValueDate.Text = "";

        }
    }
    protected void GvBackOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvBackOrder.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilterSorder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBackOrder, dt, "", "");


    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "PODate" || ddlFieldName.SelectedItem.Value == "DeliveryDate")
        {
            if (txtValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtValueDate.Text);
                    txtValue.Text = Convert.ToDateTime(txtValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueDate.Text = "";
                    txtValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueDate.Focus();
                return;
            }
        }
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
            DataTable dtAdd = (DataTable)Session["dtSorder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvBackOrder, view.ToTable(), "", "");

            Session["dtFilterSorder"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";


        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvBackOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterSorder"];
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
        Session["dtFilterSorder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBackOrder, dt, "", "");


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
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        chkSelectall.Checked = false;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;

        txtValue.Text = "";
        txtValueDate.Text = "";

        txtValueDate.Visible = false;
        txtValue.Visible = true;

    }
    private void FillGrid()
    {
        DataTable dt = objDa.return_DataTable("SELECT POD.Trans_Id,POD.field4,poh.TransID as POID,POD.orderqty, (SELECT case when SUM(Inv_PurchaseInvoiceDetail.InvoiceQty) is null then 0 else SUM(Inv_PurchaseInvoiceDetail.InvoiceQty) end FROM Inv_PurchaseInvoiceDetail WHERE Inv_PurchaseInvoiceDetail.POId = POD.PoNO AND Inv_PurchaseInvoiceDetail.ProductId = POD.Product_ID) as deliveredQty, poh.SupplierId, Ems_ContactMaster.Name AS SupplierName, POH.PoNO, POH.PODate, POH.DeliveryDate, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_SalesOrderHeader.Trans_Id as SOID, Inv_SalesOrderHeader.SalesOrderNo as SONO, Inv_UnitMaster.Unit_Name, CASE WHEN (POD.OrderQty - (SELECT SUM(Inv_PurchaseInvoiceDetail.InvoiceQty) FROM Inv_PurchaseInvoiceDetail inner join Inv_PurchaseInvoiceHeader on Inv_PurchaseInvoiceheader.TransID = Inv_PurchaseInvoiceDetail.InvoiceNo WHERE Inv_PurchaseInvoiceDetail.POId = POD.PoNO and Inv_PurchaseInvoiceHeader.IsActive='true'  AND Inv_PurchaseInvoiceDetail.ProductId = POD.Product_ID) ) IS NULL THEN POD.OrderQty ELSE (POD.OrderQty - (SELECT SUM(Inv_PurchaseInvoiceDetail.InvoiceQty) FROM Inv_PurchaseInvoiceDetail inner join Inv_PurchaseInvoiceHeader on Inv_PurchaseInvoiceheader.TransID = Inv_PurchaseInvoiceDetail.InvoiceNo WHERE Inv_PurchaseInvoiceDetail.POId = POD.PoNO and Inv_PurchaseInvoiceHeader.IsActive='true'  AND Inv_PurchaseInvoiceDetail.ProductId = POD.Product_ID) ) END AS BackOrderQty, (SELECT CASE WHEN Set_UserMaster.Emp_Id != '0' THEN Emp_Name ELSE 'Superadmin' END FROM Set_EmployeeMaster RIGHT JOIN Set_UserMaster ON Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE Set_UserMaster.User_Id = poh.CreatedBy) AS CreatedUser, (SELECT CASE WHEN Set_UserMaster.Emp_Id != '0' THEN Emp_Name ELSE 'Superadmin' END FROM Set_EmployeeMaster RIGHT JOIN Set_UserMaster ON Set_UserMaster.Emp_Id = Set_EmployeeMaster.Emp_Id WHERE Set_UserMaster.User_Id = poh.ModifiedBy) AS ModifiedUser,Inv_StockDetail.quantity,pod.product_id FROM Inv_PurchaseOrderHeader AS POH INNER JOIN Inv_PurchaseOrderDetail AS POD ON poh.TransID = pod.PoNO INNER JOIN Inv_ProductMaster ON POD.Product_Id = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON POD.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Ems_ContactMaster ON POH.SupplierId = Ems_ContactMaster.Trans_Id left join Inv_SalesOrderHeader on Inv_SalesOrderHeader.Trans_Id = POD.SalesOrderID left join Inv_StockDetail on Inv_StockDetail.ProductId =  POD.Product_ID and Inv_StockDetail.Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "' and Inv_StockDetail.Location_Id='" + Session["locId"].ToString() + "' where      POD.Product_ID not in                      (Select Inv_PurchaseInvoiceDetail.ProductId from Inv_PurchaseInvoiceDetail where Inv_PurchaseInvoiceDetail.InvoiceNo in(Select Inv_PurchaseInvoiceHeader.TransId From Inv_PurchaseInvoiceHeader ) and         (Inv_PurchaseInvoiceDetail.InvoiceQty)>0 and POH.PartialShipment='N'   and Inv_PurchaseInvoiceDetail.POId =POD.PoNO )      and POH.Company_Id=" + Session["CompId"].ToString() + "     and POH.Brand_Id=" + Session["BrandId"].ToString() + "     and POH.Location_Id in (" + ddlLocation.SelectedValue + ")    and POD.Field1<>'0' and POH.IsActive='True' order by POH.PODate desc ");

        try
        {
            dt = new DataView(dt, "BackOrderQty<>0", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        //DataTable dt=ObjPurchaseOrder.GetPendingPurchaseOrder(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (txtCustomer.Text != "")
        {
            try
            {
                dt = new DataView(dt, "SupplierId=" + txtCustomer.Text.Split('/')[1].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        Session["dtSorder"] = dt;
        Session["dtFilterSorder"] = dt;
        if (dt != null && dt.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvBackOrder, dt, "", "");
        }
        else
        {
            GvBackOrder.DataSource = null;
            GvBackOrder.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Choose Supplier in suggestion");
            txtCustomer.Focus();
            return;
        }
        FillGrid();

        if (GvBackOrder.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            txtCustomer.Text = "";
            return;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtCustomer.Text = "";
        btnRefreshReport_Click(null, null);
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        if (txtCustomer.Text != "")
        {
            try
            {
                string[] CustomerName = txtCustomer.Text.Split('/');

                DataTable DtCustomer = objContact.GetContactByContactName(CustomerName[0].ToString().Trim());

                if (DtCustomer.Rows.Count == 0)
                {

                    DisplayMessage("Enter Supplier Name in suggestion Only");
                    txtCustomer.Text = "";
                    txtCustomer.Focus();
                    return;
                }
            }
            catch
            {
                DisplayMessage("Enter Supplier Name in suggestion Only");
                txtCustomer.Text = "";
                txtCustomer.Focus();
                return;
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);

        //dtCustomer = ObjContactMaster.GetContactTrueAllData();

        //DataTable dtMain = new DataTable();
        //dtMain = dtCustomer.Copy();


        //string filtertext = "Filtertext like '%" + prefixText + "%'";
        //DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        txtsoNo.Text = ((Label)gvRow.FindControl("lblgvSONo")).Text;
        txtSODate.Text = Convert.ToDateTime(((Label)gvRow.FindControl("lblgvSODate")).Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtprouctname.Text = ((Label)gvRow.FindControl("lblgvproductname")).Text;
        txtUnitName.Text = ((Label)gvRow.FindControl("lblunitname")).Text;
        txtquantity.Text = ((Label)gvRow.FindControl("lblqty")).Text;
        hdnTransId.Value = e.CommandArgument.ToString();
        txtDeliveryDate.Text = ((Label)gvRow.FindControl("lblgvDeliveryDate")).Text;
        txtSalesOrderNo.Text = ((Label)gvRow.FindControl("lblgvSOrderNO")).Text;
        txtDescription.Text = ((Label)gvRow.FindControl("lblNotes")).Text;

        hdnQuantity.Value = txtquantity.Text;
        hdnOrderId.Value = ((Label)gvRow.FindControl("lblgvPOID")).Text;
        PnlNewEdit.Visible = true;
        PnlList.Visible = false;

    }
    protected void btnSOrderSave_Click(object sender, EventArgs e)
    {
        string sql = "";
        if (txtquantity.Text != "")
        {
            decimal number = 0;
            decimal.TryParse(hdnQuantity.Value.Trim(), out number);
            decimal number1 = 0;
            decimal.TryParse(txtquantity.Text.Trim(), out number1);

            if (number == number1 || number1 == 0)
            {
                sql = "update Inv_PurchaseOrderDetail set Field1='" + txtquantity.Text + "',Field4='" + txtDescription.Text + "' where Trans_Id=" + hdnTransId.Value + "";
                objDa.execute_Command(sql);
            }
            else
            {
                DisplayMessage("Quantity must be same or Zero to adjust");
                return;
            }
        }
        if (chkSelectall.Checked)
        {
            sql = "update Inv_PurchaseOrderDetail set Field4='" + txtDescription.Text + "' where PONO=" + hdnOrderId.Value + "";
            objDa.execute_Command(sql);
        }
        else
        {
            sql = "update Inv_PurchaseOrderDetail set Field4='" + txtDescription.Text + "' where Trans_Id=" + hdnTransId.Value + "";
            objDa.execute_Command(sql);
        }

        if (txtDeliveryDate.Text.Trim() != "")
        {
            sql = "update Inv_PurchaseOrderHeader set DeliveryDate='" + txtDeliveryDate.Text + "',ModifiedBy='" + Session["UserID"].ToString() + "',ModifiedDate=(select GETDATE()) where TransID= (select PoNO from Inv_PurchaseOrderDetail where Trans_Id='" + hdnTransId.Value + "')";
            objDa.execute_Command(sql);
        }

        DisplayMessage("Record Updated", "green");
        btnRefreshReport_Click(null, null);
        PnlNewEdit.Visible = false;
        PnlList.Visible = true;
    }
    protected void btnSOrderCancel_Click(object sender, EventArgs e)
    {
        btnRefreshReport_Click(null, null);
        PnlNewEdit.Visible = false;
        PnlList.Visible = true;
    }
    public string GetAmountDecimal(string Amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Amount);

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
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
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
        FillGrid();
    }
    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportTableData(Session["dtSorder"] as DataTable, "Purchse Back Order Data");
    }

    protected void lnkStockInfo_Command(object sender, CommandEventArgs e)
    {
        modelSA.getProductDetail(e.CommandArgument.ToString(), "", "");
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        Common cmn = new Common(HttpContext.Current.Session["DBConnection"].ToString());
        bool Result = false;

        if (HttpContext.Current.Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(HttpContext.Current.Session["UserId"].ToString(), ModuelId, ObjectId, HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }
}