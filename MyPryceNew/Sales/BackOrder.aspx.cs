using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.IO;

public partial class Sales_BackOrder : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
    LocationMaster objLocation = null;
    Set_CustomerMaster objCustomer = null;
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
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "278", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillGrid();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            AllPageCode();
        }
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("278", (DataTable)Session["ModuleName"]);
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


        if (Session["EmpId"].ToString() == "0")
        {
            btnSOrderSave.Visible = true;
            try
            {
                GvBackOrder.Columns[1].Visible = true;
            }
            catch
            {

            }
        }
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "278", Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnSOrderSave.Visible = true;
            }
            if (DtRow["Op_Id"].ToString() == "2")
            {
                try
                {
                    GvBackOrder.Columns[1].Visible = true;
                }
                catch
                {
                }
            }
        }
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "SalesOrderDate" || ddlFieldName.SelectedItem.Value == "EstimateDeliveryDate")
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
        DataTable dt = (DataTable)Session["dtFilterBackorder"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBackOrder, dt, "", "");


    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "SalesOrderDate" || ddlFieldName.SelectedItem.Value == "EstimateDeliveryDate")
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
            DataTable dtAdd = (DataTable)Session["dtBackorder"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvBackOrder, view.ToTable(), "", "");

            Session["dtFilterBackorder"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";


        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvBackOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtFilterBackorder"];
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
        Session["dtFilterBackorder"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvBackOrder, dt, "", "");


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
        //DataTable dt = objDa.return_DataTable("SELECT Inv_SalesOrderDetail.product_Id,Inv_SalesOrderDetail.field4, Inv_SalesOrderDetail.Trans_Id,Inv_SalesOrderDetail.SalesOrderNo as SOID, Inv_SalesOrderHeader.CustomerId,case when Inv_SalesOrderDetail.Field3='' then Inv_SalesOrderHeader.EstimateDeliveryDate else Inv_SalesOrderDetail.Field3 end as EstimateDeliveryDate, Ems_ContactMaster.Name AS CustomerName, Inv_SalesOrderHeader.SalesOrderNo, Inv_SalesOrderHeader.SalesOrderDate, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_UnitMaster.Unit_Name, Inv_PurchaseOrderHeader.PoNO, Inv_PurchaseOrderHeader.DeliveryDate,Inv_SalesOrderDetail.Quantity as OrderQty, CASE WHEN (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail inner join Inv_SalesInvoiceheader on Inv_SalesInvoiceheader.Trans_Id = Inv_SalesInvoiceDetail.Invoice_No WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo And Inv_SalesInvoiceheader.IsActive='true' AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) IS NULL THEN Inv_SalesOrderDetail.Quantity ELSE (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail       inner join Inv_SalesInvoiceheader on Inv_SalesInvoiceheader.Trans_Id = Inv_SalesInvoiceDetail.Invoice_No WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo  And Inv_SalesInvoiceheader.IsActive='true' AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) END AS BackOrderQty, case when (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) is null then 0 else (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) end as deliveredQty,Set_EmployeeMaster.Emp_Name AS ModifiedUser, Inv_StockDetail.Quantity  FROM inv_salesorderheader  INNER JOIN Inv_SalesOrderDetail ON Inv_SalesOrderHeader.Trans_Id = Inv_SalesOrderDetail.SalesOrderNo INNER JOIN Inv_ProductMaster ON Inv_SalesOrderDetail.Product_Id = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_SalesOrderDetail.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Ems_ContactMaster ON Inv_SalesOrderHeader.CustomerId = Ems_ContactMaster.Trans_Id left join Inv_PurchaseOrderDetail on Inv_PurchaseOrderDetail.SalesOrderID = Inv_SalesOrderHeader.Trans_Id and Inv_PurchaseOrderDetail.Product_ID = Inv_SalesOrderDetail.Product_Id left join Inv_PurchaseOrderHeader on Inv_PurchaseOrderHeader.TransID = Inv_PurchaseOrderDetail.PoNO  left join Set_EmployeeMaster on Set_EmployeeMaster.Company_Id = inv_salesorderheader.company_id  AND Set_EmployeeMaster.Emp_Code = inv_salesorderheader.ModifiedBy left join Inv_StockDetail on Inv_StockDetail.ProductId = Inv_SalesOrderDetail.Product_Id and Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString()+"' and Inv_StockDetail.Location_Id = '"+Session["LocId"].ToString()+"'  where Inv_SalesOrderDetail.Product_ID not in(Select Inv_SalesInvoiceDetail.Product_Id from Inv_SalesInvoiceDetail where  Inv_SalesInvoiceDetail.Invoice_No in(Select Inv_SalesInvoiceHeader.Trans_Id From Inv_SalesInvoiceHeader where Inv_SalesInvoiceHeader.IsActive='true' and Inv_SalesInvoiceHeader.Company_Id='" + Session["CompId"].ToString()+ "' and Inv_SalesInvoiceHeader.brand_id='" + Session["BrandId"].ToString() + "' and Inv_SalesInvoiceHeader.Location_Id='" + Session["LocId"].ToString() + "') and (Inv_SalesInvoiceDetail.Quantity)>0 and Inv_SalesOrderHeader.Field6='False'   and SIFromTransNo =Inv_SalesOrderDetail.SalesOrderNo ) and Inv_SalesOrderHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesOrderHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesOrderHeader.Location_Id in (" + ddlLocation.SelectedValue + ") and Inv_SalesOrderDetail.Field1<>'0' and Inv_SalesOrderHeader.IsActive='True' order by Inv_SalesOrderHeader.Trans_Id desc");
        //Query Updated on 11-03-2024 By Lokesh
        DataTable dt = objDa.return_DataTable("SELECT Inv_SalesOrderDetail.product_Id,Inv_SalesOrderDetail.field4, Inv_SalesOrderDetail.Trans_Id,Inv_SalesOrderDetail.SalesOrderNo as SOID, Inv_SalesOrderHeader.CustomerId,  case when Inv_SalesOrderDetail.Field3='' then Convert(Nvarchar, Inv_SalesOrderHeader.EstimateDeliveryDate) else Inv_SalesOrderDetail.Field3  end as EstimateDeliveryDate, Ems_ContactMaster.Name AS CustomerName, Inv_SalesOrderHeader.SalesOrderNo, Inv_SalesOrderHeader.SalesOrderDate, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_UnitMaster.Unit_Name, Inv_PurchaseOrderHeader.PoNO, Inv_PurchaseOrderHeader.DeliveryDate,Inv_SalesOrderDetail.Quantity as OrderQty, CASE WHEN (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail inner join Inv_SalesInvoiceheader on Inv_SalesInvoiceheader.Trans_Id = Inv_SalesInvoiceDetail.Invoice_No WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo And Inv_SalesInvoiceheader.IsActive='true' AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) IS NULL THEN Inv_SalesOrderDetail.Quantity ELSE (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail       inner join Inv_SalesInvoiceheader on Inv_SalesInvoiceheader.Trans_Id = Inv_SalesInvoiceDetail.Invoice_No WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo  And Inv_SalesInvoiceheader.IsActive='true' AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) END AS BackOrderQty, case when (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) is null then 0 else (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) end as deliveredQty,Set_EmployeeMaster.Emp_Name AS ModifiedUser, Inv_StockDetail.Quantity  FROM inv_salesorderheader  INNER JOIN Inv_SalesOrderDetail ON Inv_SalesOrderHeader.Trans_Id = Inv_SalesOrderDetail.SalesOrderNo INNER JOIN Inv_ProductMaster ON Inv_SalesOrderDetail.Product_Id = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_SalesOrderDetail.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Ems_ContactMaster ON Inv_SalesOrderHeader.CustomerId = Ems_ContactMaster.Trans_Id left join Inv_PurchaseOrderDetail on Inv_PurchaseOrderDetail.SalesOrderID = Inv_SalesOrderHeader.Trans_Id and Inv_PurchaseOrderDetail.Product_ID = Inv_SalesOrderDetail.Product_Id left join Inv_PurchaseOrderHeader on Inv_PurchaseOrderHeader.TransID = Inv_PurchaseOrderDetail.PoNO  left join Set_EmployeeMaster on Set_EmployeeMaster.Company_Id = inv_salesorderheader.company_id  AND Set_EmployeeMaster.Emp_Code = inv_salesorderheader.ModifiedBy left join Inv_StockDetail on Inv_StockDetail.ProductId = Inv_SalesOrderDetail.Product_Id and Inv_StockDetail.Finance_Year_Id = '" + Session["FinanceYearId"].ToString() + "' and Inv_StockDetail.Location_Id = '" + Session["LocId"].ToString() + "'  where Inv_SalesOrderDetail.Product_ID not in(Select Inv_SalesInvoiceDetail.Product_Id from Inv_SalesInvoiceDetail where  Inv_SalesInvoiceDetail.Invoice_No in(Select Inv_SalesInvoiceHeader.Trans_Id From Inv_SalesInvoiceHeader where Inv_SalesInvoiceHeader.IsActive='true' and Inv_SalesInvoiceHeader.Company_Id='" + Session["CompId"].ToString() + "' and Inv_SalesInvoiceHeader.brand_id='" + Session["BrandId"].ToString() + "' and Inv_SalesInvoiceHeader.Location_Id='" + Session["LocId"].ToString() + "') and (Inv_SalesInvoiceDetail.Quantity)>0 and Inv_SalesOrderHeader.Field6='False'   and SIFromTransNo =Inv_SalesOrderDetail.SalesOrderNo ) and Inv_SalesOrderHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesOrderHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesOrderHeader.Location_Id in (" + ddlLocation.SelectedValue + ") and Inv_SalesOrderDetail.Field1<>'0' and Inv_SalesOrderHeader.IsActive='True' order by Inv_SalesOrderHeader.Trans_Id desc");
        if (txtCustomer.Text != "")
        {
            try
            {
                dt = new DataView(dt, "CustomerId=" + txtCustomer.Text.Split('/')[3].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        try
        {
            dt = new DataView(dt, "BackOrderQty>0", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        Session["dtBackorder"] = dt;
        Session["dtFilterBackorder"] = dt;
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
            DisplayMessage("Choose customer in suggestion");
            txtCustomer.Focus();
            return;
        }
        FillGrid();
        string data = getEmpData(txtCustomer.Text.Split('/')[3]);
        divEmpDetail.Visible = false;
        if (data != "")
        {
            divEmpDetail.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "getEmpData('" + data + "');", true);
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

                string name = objContact.GetContactNameByContactiD(CustomerName[1].ToString().Trim());
                if (name.Trim() != CustomerName[0].ToString().Trim())
                {
                    DisplayMessage("Enter Customer Name in suggestion Only");
                    txtCustomer.Text = "";
                    Session["ContactID"] = "";
                    divEmpDetail.Visible = false;
                    txtCustomer.Focus();
                    return;
                }
                else
                {
                    Session["ContactID"] = CustomerName[1].ToString().Trim();
                }
            }
            catch
            {
                DisplayMessage("Enter Customer Name in suggestion Only");
                txtCustomer.Text = "";
                Session["ContactID"] = "";
                txtCustomer.Focus();
                divEmpDetail.Visible = false;
                return;
            }
        }
        else
        {
            divEmpDetail.Visible = false;
            Session["ContactID"] = "";
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
        try
        {
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
        catch (Exception ex)
        {
            return strNewDate;
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        txtsoNo.Text = ((Label)gvRow.FindControl("lblgvSONo")).Text;
        txtSODate.Text = Convert.ToDateTime(((Label)gvRow.FindControl("lblgvSODate")).Text).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtprouctname.Text = ((Label)gvRow.FindControl("lblgvproductname")).Text;
        txtUnitName.Text = ((Label)gvRow.FindControl("lblunitname")).Text;
        txtquantity.Text = ((Label)gvRow.FindControl("lblqty")).Text;
        hdnQuantity.Value = txtquantity.Text;
        txtProductCode.Text = ((Label)gvRow.FindControl("lblgvproductid")).Text;
        txtCustExpDeliveryDate.Text = ((Label)gvRow.FindControl("lblgvDeliveryDate")).Text;
        txtDescription.Text = ((Label)gvRow.FindControl("lblNotes")).Text;
        hdnTransId.Value = e.CommandArgument.ToString();
        hdnSoId.Value = ((Label)gvRow.FindControl("lblgvSOID")).Text;
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
                sql = "update Inv_SalesOrderDetail set Field1='" + txtquantity.Text + "' where Trans_Id=" + hdnTransId.Value + "";
                objDa.execute_Command(sql);
            }
            else
            {
                DisplayMessage("Quantity must be same or zero to adjust");
                return;
            }
        }

        if (txtCustExpDeliveryDate.Text != "")
        {
            if (chkSelectall.Checked)
            {
                sql = "update Inv_SalesOrderDetail set Field3='" + txtCustExpDeliveryDate.Text + "',Field4='" + txtDescription.Text + "', modifiedBy='" + Session["UserId"].ToString() + "',modifiedDate='" + System.DateTime.Now.ToString("dd-MMM-yyyy") + "' where Salesorderno=" + hdnSoId.Value + "";
                objDa.execute_Command(sql);
            }
            else
            {
                sql = "update Inv_SalesOrderDetail set Field3='" + txtCustExpDeliveryDate.Text + "',Field4='" + txtDescription.Text + "', modifiedBy='" + Session["UserId"].ToString() + "',modifiedDate='" + System.DateTime.Now.ToString("dd-MMM-yyyy") + "' where Trans_Id=" + hdnTransId.Value + "";
                objDa.execute_Command(sql);
            }
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


    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        if (hdnEmailIDS.Value.Length < 4)
        {
            DisplayMessage("Please select contact person to send the email");
            return;
        }
        if (hdnCheckedValues.Value == "")
        {
            DisplayMessage("Please select record to send the mail");
            return;
        }
        DataTable dt = objDa.return_DataTable("SELECT Inv_SalesOrderDetail.Trans_Id, Inv_SalesOrderDetail.SalesOrderNo AS SOID, Inv_SalesOrderHeader.CustomerId, CASE WHEN Inv_SalesOrderDetail.Field3 = '' THEN Inv_SalesOrderHeader.EstimateDeliveryDate ELSE Inv_SalesOrderDetail.Field3 END AS EstimateDeliveryDate, Ems_ContactMaster.Name AS CustomerName, Inv_SalesOrderHeader.SalesOrderNo   ,Inv_SalesInvoiceDetail.Quantity as deliveredQty, Inv_SalesOrderHeader.SalesOrderDate, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_UnitMaster.Unit_Name, Inv_PurchaseOrderHeader.PoNO, Inv_PurchaseOrderHeader.DeliveryDate, Inv_SalesOrderDetail.Quantity AS OrderQty, Inv_StockDetail.quantity as sysQty, CASE WHEN (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) IS NULL THEN Inv_SalesOrderDetail.Quantity ELSE (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) END AS BackOrderQty, (SELECT Set_EmployeeMaster.Emp_Name FROM Set_EmployeeMaster WHERE Set_EmployeeMaster.Company_Id = inv_salesorderheader.company_id AND Set_EmployeeMaster.Emp_Code = inv_salesorderheader.CreatedBy) AS CreatedUser, (SELECT Set_EmployeeMaster.Emp_Name FROM Set_EmployeeMaster WHERE Set_EmployeeMaster.Company_Id = inv_salesorderheader.company_id AND Set_EmployeeMaster.Emp_Code = inv_salesorderheader.ModifiedBy) AS ModifiedUser FROM inv_salesorderheader INNER JOIN Inv_SalesOrderDetail ON Inv_SalesOrderHeader.Trans_Id = Inv_SalesOrderDetail.SalesOrderNo INNER JOIN Inv_ProductMaster ON Inv_SalesOrderDetail.Product_Id = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_SalesOrderDetail.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Ems_ContactMaster ON Inv_SalesOrderHeader.CustomerId = Ems_ContactMaster.Trans_Id LEFT JOIN Inv_PurchaseOrderDetail ON Inv_PurchaseOrderDetail.SalesOrderID = Inv_SalesOrderHeader.Trans_Id AND Inv_PurchaseOrderDetail.Product_ID = Inv_SalesOrderDetail.Product_Id LEFT JOIN Inv_PurchaseOrderHeader ON Inv_PurchaseOrderHeader.TransID = Inv_PurchaseOrderDetail.PoNO left join Inv_StockDetail on Inv_StockDetail.ProductId = Inv_SalesOrderDetail.Product_Id and Inv_StockDetail.Finance_Year_Id='" + Session["FinanceYearId"].ToString() + "' and Inv_StockDetail.Location_Id='" + Session["LocID"].ToString() + "' left join Inv_SalesInvoiceDetail on Inv_SalesInvoiceDetail.SIFromTransType='S' and Inv_SalesInvoiceDetail.SIFromTransNo =Inv_SalesOrderDetail.SalesOrderNo and  Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId WHERE Inv_SalesOrderDetail.Product_ID NOT IN (SELECT Inv_SalesInvoiceDetail.Product_Id FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.Invoice_No IN (SELECT Inv_SalesInvoiceHeader.Trans_Id FROM Inv_SalesInvoiceHeader) AND (Inv_SalesInvoiceDetail.Quantity) > 0 AND Inv_SalesOrderHeader.Field6 = 'False' AND SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo) AND Inv_SalesOrderHeader.Company_Id = '" + Session["CompID"].ToString() + "' AND Inv_SalesOrderHeader.Brand_Id = '" + Session["BrandID"].ToString() + "' AND Inv_SalesOrderHeader.Location_Id = '" + Session["LocID"].ToString() + "' AND Inv_SalesOrderDetail.Field1 = ' ' AND Inv_SalesOrderHeader.IsActive = 'True' and Inv_SalesOrderDetail.Trans_Id in (" + hdnCheckedValues.Value.Trim().Substring(0, hdnCheckedValues.Value.Trim().ToString().Length - 1) + ") ORDER BY Inv_SalesOrderHeader.Trans_Id DESC");
        if (dt.Rows.Count > 0)
        {
            string data = " <html><head><link rel = 'stylesheet' type = 'text/css href = 'http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css' /></head><body> <p>Hi " + dt.Rows[0]["CustomerName"].ToString() + " </p><p> We are happy to inform you that your order has been out of expected delivery with following  details:</p><p>   <table border='1px' cellpadding='5' cellspacing='0' style='border: solid 1px Silver; font-size: x-small; width: 100%;'><thead><tr><td align='left'   style='font-weight:bold; font-size: 13px;'>Order No</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Order Date</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Ref.No.</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Product ID</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Product Name</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Order Qty</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Delivered Qty</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Current Stock</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Balanced Qty</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Expected Delivery Date</td></tr></thead><tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data += "<tr> <td>" + dt.Rows[i]["SalesOrderNo"].ToString() + "</td>  <td>" + Convert.ToDateTime(dt.Rows[i]["SalesOrderDate"].ToString()).ToString("dd-MMM-yyyy") + "</td>  <td>" + dt.Rows[i]["PONo"].ToString() + "</td>  <td>" + dt.Rows[i]["ProductCode"].ToString() + "</td>  <td>" + dt.Rows[i]["EProductName"].ToString() + "</td>  <td>" + dt.Rows[i]["OrderQty"].ToString() + "</td>   <td> " + dt.Rows[i]["deliveredQty"].ToString() + " </td>  <td>" + dt.Rows[i]["sysQty"].ToString() + "</td>  <td>" + dt.Rows[i]["BackOrderQty"].ToString() + "</td>  <td>" + Convert.ToDateTime(dt.Rows[i]["EstimateDeliveryDate"].ToString()).ToString("dd-MMM-yyyy") + "</td></tr>";
            }
            data += "</tbody></table></p></body></html>";

            Session["hdnBody"] = data;
            Session["hdnEmailIDS"] = hdnEmailIDS.Value;
            ScriptManager.RegisterStartupScript(this, GetType(), "", "window.open('../EmailSystem/SendMail.aspx?HD=1', '_blank', 'width=1024, ');", true);
            //string data = getEmpData(txtCustomer.Text.Split('/')[1]);
            //divEmpDetail.Visible = false;
            //if (data != "")
            //{
            //    divEmpDetail.Visible = true;
            //    ScriptManager.RegisterStartupScript(this, GetType(), "", "getEmpData('" + data + "');", true);
            //}
            //sendMail(data);
            //DisplayMessage("Email send Successfully");
        }
    }

    public void sendMail(string body)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        SendMailSms ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        string Master_Email_SMTP = "smtpout.secureserver.net";// objAppParam.GetApplicationParameterValueByParamName("Support_Email_SMTP", HttpContext.Current.Session["CompId"].ToString()).ToString();
        string Master_Email_Port = "25"; objAppParam.GetApplicationParameterValueByParamName("Support_Email_Port", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();
        string Master_Email = "jitendra@pegasustech.net";// objAppParam.GetApplicationParameterValueByParamName("Support_Email", Session["CompId"].ToString()).ToString();
        string Master_Email_Password = "Ji@12345";// Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Support_Email_Password", Session["CompId"].ToString()).ToString());
        string Email_Display_Name = objAppParam.GetApplicationParameterValueByParamName("Support_Display_Text", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();
        ObjSendMailSms.SendMail_TicketInfo("neeraj@pegasustech.net", "", "", "Purchase Order Details", body, Session["CompId"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string getEmployeeData(string CustID)
    {
        Ems_ContactMaster CM = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dt = CM.getContactEmailFromCustId(CustID))
        {
            if (dt.Rows.Count > 0)
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return "";
            }
        }
    }
    public string getEmpData(string CustID)
    {
        Ems_ContactMaster CM = new Ems_ContactMaster(Session["DBConnection"].ToString());
        using (DataTable dt = CM.getContactEmailFromCustId(CustID))
        {
            if (dt.Rows.Count > 0)
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return "";
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string btnSendEmail_Click(string CheckedValues, string emailIDS)
    {

        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objDa.return_DataTable("SELECT Inv_SalesOrderDetail.Trans_Id, Inv_SalesOrderDetail.SalesOrderNo AS SOID, Inv_SalesOrderHeader.CustomerId, CASE WHEN Inv_SalesOrderDetail.Field3 = '' THEN Inv_SalesOrderHeader.EstimateDeliveryDate ELSE Inv_SalesOrderDetail.Field3 END AS EstimateDeliveryDate, Ems_ContactMaster.Name AS CustomerName, Inv_SalesOrderHeader.SalesOrderNo   ,Inv_SalesInvoiceDetail.Quantity as deliveredQty, Inv_SalesOrderHeader.SalesOrderDate, Inv_ProductMaster.ProductCode, Inv_ProductMaster.EProductName, Inv_UnitMaster.Unit_Name, Inv_PurchaseOrderHeader.PoNO, Inv_PurchaseOrderHeader.DeliveryDate, Inv_SalesOrderDetail.Quantity AS OrderQty, Inv_StockDetail.quantity as sysQty, CASE WHEN (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) IS NULL THEN Inv_SalesOrderDetail.Quantity ELSE (Inv_SalesOrderDetail.Quantity - (SELECT SUM(Inv_SalesInvoiceDetail.Quantity) FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo AND Inv_SalesInvoiceDetail.Product_Id = Inv_SalesOrderDetail.Product_ID) ) END AS BackOrderQty, (SELECT Set_EmployeeMaster.Emp_Name FROM Set_EmployeeMaster WHERE Set_EmployeeMaster.Company_Id = inv_salesorderheader.company_id AND Set_EmployeeMaster.Emp_Code = inv_salesorderheader.CreatedBy) AS CreatedUser, (SELECT Set_EmployeeMaster.Emp_Name FROM Set_EmployeeMaster WHERE Set_EmployeeMaster.Company_Id = inv_salesorderheader.company_id AND Set_EmployeeMaster.Emp_Code = inv_salesorderheader.ModifiedBy) AS ModifiedUser FROM inv_salesorderheader INNER JOIN Inv_SalesOrderDetail ON Inv_SalesOrderHeader.Trans_Id = Inv_SalesOrderDetail.SalesOrderNo INNER JOIN Inv_ProductMaster ON Inv_SalesOrderDetail.Product_Id = Inv_ProductMaster.ProductId INNER JOIN Inv_UnitMaster ON Inv_SalesOrderDetail.UnitId = Inv_UnitMaster.Unit_Id INNER JOIN Ems_ContactMaster ON Inv_SalesOrderHeader.CustomerId = Ems_ContactMaster.Trans_Id LEFT JOIN Inv_PurchaseOrderDetail ON Inv_PurchaseOrderDetail.SalesOrderID = Inv_SalesOrderHeader.Trans_Id AND Inv_PurchaseOrderDetail.Product_ID = Inv_SalesOrderDetail.Product_Id LEFT JOIN Inv_PurchaseOrderHeader ON Inv_PurchaseOrderHeader.TransID = Inv_PurchaseOrderDetail.PoNO left join Inv_StockDetail on Inv_StockDetail.ProductId = Inv_SalesOrderDetail.Product_Id and Inv_StockDetail.Finance_Year_Id='" + HttpContext.Current.Session["FinanceYearId"].ToString() + "' and Inv_StockDetail.Location_Id='" + HttpContext.Current.Session["LocID"].ToString() + "' left join Inv_SalesInvoiceDetail on Inv_SalesInvoiceDetail.SIFromTransType='S' and Inv_SalesInvoiceDetail.SIFromTransNo =Inv_SalesOrderDetail.SalesOrderNo and  Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId WHERE Inv_SalesOrderDetail.Product_ID NOT IN (SELECT Inv_SalesInvoiceDetail.Product_Id FROM Inv_SalesInvoiceDetail WHERE Inv_SalesInvoiceDetail.Invoice_No IN (SELECT Inv_SalesInvoiceHeader.Trans_Id FROM Inv_SalesInvoiceHeader) AND (Inv_SalesInvoiceDetail.Quantity) > 0 AND Inv_SalesOrderHeader.Field6 = 'False' AND SIFromTransNo = Inv_SalesOrderDetail.SalesOrderNo) AND Inv_SalesOrderHeader.Company_Id = '" + HttpContext.Current.Session["CompID"].ToString() + "' AND Inv_SalesOrderHeader.Brand_Id = '" + HttpContext.Current.Session["BrandID"].ToString() + "' AND Inv_SalesOrderHeader.Location_Id = '" + HttpContext.Current.Session["LocID"].ToString() + "' AND Inv_SalesOrderDetail.Field1  <> '0' AND Inv_SalesOrderHeader.IsActive = 'True' and Inv_SalesOrderDetail.Trans_Id in (" + CheckedValues.Trim().Substring(0, CheckedValues.Trim().ToString().Length - 1) + ") ORDER BY Inv_SalesOrderHeader.Trans_Id DESC");
        string val = "";
        if (dt.Rows.Count > 0)
        {
            string data = " <html><head><link rel = 'stylesheet' type = 'text/css href = 'http://ajax.aspnetcdn.com/ajax/jquery.dataTables/1.9.4/css/jquery.dataTables.css' /></head><body> <p>Hi " + dt.Rows[0]["CustomerName"].ToString() + " </p><p> We are happy to inform you that your order has been out of expected delivery with the following  details:</p><p>   <table border='1px' cellpadding='5' cellspacing='0' style='border: solid 1px Silver; font-size: x-small; width: 100%;'><thead><tr><td align='left'   style='font-weight:bold; font-size: 13px;'>Order No</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Order Date</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Ref.No.</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Product ID</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Product Name</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Order Qty</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Delivered Qty</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Current Stock</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Balanced Qty</td><td align='left'   style='font-weight:bold; font-size: 13px;'>Expected Delivery Date</td></tr></thead><tbody>";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data += "<tr> <td>" + dt.Rows[i]["SalesOrderNo"].ToString() + "</td>  <td>" + Convert.ToDateTime(dt.Rows[i]["SalesOrderDate"].ToString()).ToString("dd-MMM-yyyy") + "</td>  <td>" + dt.Rows[i]["PONo"].ToString() + "</td>  <td>" + dt.Rows[i]["ProductCode"].ToString() + "</td>  <td>" + dt.Rows[i]["EProductName"].ToString() + "</td>  <td>" + Common.GetAmountDecimal(dt.Rows[i]["OrderQty"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + "</td>   <td> " + Common.GetAmountDecimal(dt.Rows[i]["deliveredQty"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + " </td>  <td>" + Common.GetAmountDecimal(dt.Rows[i]["sysQty"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + "</td>  <td>" + Common.GetAmountDecimal(dt.Rows[i]["BackOrderQty"].ToString(), HttpContext.Current.Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + "</td>  <td>" + Convert.ToDateTime(dt.Rows[i]["EstimateDeliveryDate"].ToString()).ToString("dd-MMM-yyyy") + "</td></tr>";
            }
            data += "</tbody></table></p></body></html>";

            HttpContext.Current.Session["hdnBody"] = data;
            HttpContext.Current.Session["hdnEmailIDS"] = emailIDS;
            val = "1";
        }
        return val;
    }
    protected void btnAddCustomer_Click(object sender, EventArgs e)
    {
        if (txtCustomer.Text == "")
        {
            DisplayMessage("Please Enter Customer Name");
            txtCustomer.Focus();
            return;
        }
        DataTable dt = objCustomer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["ContactID"].ToString());
        UcContactList.fillHeader(dt);
        UcContactList.fillFollowupList(Session["ContactID"].ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_CustomerInfo_Open()", true);
    }
    //add all these function on parent .cs page to access autocomplete list

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        //DataTable dt = new EmployeeMaster().GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
        }
        dt = null;
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetEmailIdPreFixText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        dt = null;
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDesignationMaster(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DesignationMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDesignationDataPreText(prefixText);
        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Designation"].ToString() + "/" + dt.Rows[i]["Designation_Id"].ToString();
        }
        dt = null;
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Phone_no"].ToString();
        }
        dt = null;
        return txt;
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
        ExportTableData(Session["dtBackorder"] as DataTable, "Sales Back Order Data");
    }
}