using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using PegasusDataAccess;

public partial class Inventory_Report_ProductStockReport : System.Web.UI.Page
{
    Inv_ParameterMaster objParam = null;
    SystemParameter ObjSysParam = null;
    Inv_ProductLedger ObjProductLadger = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    Common cmn = null;
    Inv_ProductMaster objProductMaster = null;
    DataAccessClass objDa = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_ProductBrandMaster ObjProductBrandMaster = null;
    Set_Suppliers ObjSupplierMaster = null;
    EmployeeMaster objEmployee = null;
    ProductStockPrint objProductStockPrint = null;
    PurchaseRequestHeader InvPr = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    PageControlCommon pageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Session["AccordianId"] = "11";
        Session["HeaderText"] = "Inventory";
        objParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjProductBrandMaster = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());



        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objProductStockPrint = new ProductStockPrint(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());

        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());

        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());

        pageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = ObjSysParam.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "334", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlLocation();

            FillProductCategorySerch();
            FillddlBrandSearch();
        }
        else
        {
            GetReport();
        }
        AllPageCode();
    }
    protected void txtSuppliers_OnTextChanged(object sender, EventArgs e)
    {
        if (txtSuppliers.Text != "")
        {
            string strSupplierId = "";

            try
            {
                strSupplierId = txtSuppliers.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Invalid Supplier Name");

                txtSuppliers.Focus();
                txtSuppliers.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
            }
            DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
            try
            {
                dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt1.Rows.Count == 0)
            {
                DisplayMessage("Invalid Supplier Name");

                txtSuppliers.Focus();
                txtSuppliers.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
            }
        }
    }
    private void FillProductCategorySerch()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());
        if (dsCategory.Rows.Count > 0)
        {

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            pageCmn.FillData((object)ddlcategorysearch, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            ddlcategorysearch.Items.Insert(0, "--Select One--");
            ddlcategorysearch.SelectedIndex = 0;
        }
    }
    public void FillddlBrandSearch()
    {
        DataTable dt = ObjProductBrandMaster.GetProductBrandTrueAllData(Session["CompId"].ToString());
        try
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            pageCmn.FillData((object)ddlbrandsearch, dt, "Brand_Name", "PBrandId");

        }
        catch
        {
            ddlbrandsearch.Items.Insert(0, "--Select One--");
            ddlbrandsearch.SelectedIndex = 0;
        }
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("334", (DataTable)Session["ModuleName"]);
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

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
    }
    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "Location_Name", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }


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
    protected void lnkRef_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string RefId = arguments[0];
        string RefType = arguments[1];
        string RefTypeId = arguments[2].Trim();
        string LocationId = arguments[3].Trim();
        if (RefId != "" && RefId != "")
        {
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('ProductLedgerRefTypeView.aspx?RefType=" + RefType + "&RefId=" + RefId + "','s','height=650,width=900,scrollbars=Yes');", true);


            if (RefType == "Purchase Return")
            {
                if (IsObjectPermission("143", "53"))
                {

                    string strCmd = string.Format("window.open('../Purchase/PurchaseReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "Purchase Goods")
            {
                if (IsObjectPermission("143", "58"))
                {
                    string strCmd = string.Format("window.open('../Purchase/PurchaseGoodsRec.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "Sales Return")
            {
                if (IsObjectPermission("144", "120"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "Sales Invoice")
            {
                if (IsObjectPermission("144", "92"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
                //SalesInvoiceDetailnew(RefId);
            }
            else if (RefType == "Transfer Out")
            {
                if (IsObjectPermission("142", "94"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TO&LocId=" + LocationId + "','window','width=1024');", true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }

            }
            else if (RefType == "Transfer IN")
            {
                if (IsObjectPermission("142", "118"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TI&LocId=" + LocationId + "','window','width=1024');", true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }

            else if (RefType == "Stock Adjustment")
            {
                if (IsObjectPermission("142", "131"))
                {
                    string strCmd = string.Format("window.open('../Inventory/StockAdjustment.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }

            }
        }
        else
        {
            //myButton.Attributes.Add("
            DisplayMessage("No Data");
            return;
        }

        AllPageCode();

        //if (RefId != "" && RefId != "")
        //{
        //    if (RefType == "Purchase Return")
        //    {
        //        string strCmd = string.Format("window.open('../Purchase/PurchaseReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

        //    }
        //    else if (RefType == "Sales Return")
        //    {
        //        string strCmd = string.Format("window.open('../Sales/SalesReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true); 
        //    }
        //    else if (RefType == "Sales Invoice")
        //    {
        //        string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefTypeId + "&LocId=" +LocationId+ "','window','width=1024, ');");
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        //        //SalesInvoiceDetailnew(RefId);
        //    }
        //    else if (RefType == "Transfer Out")
        //    {
        //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TO&LocId=" + LocationId + "','window','width=1024');", true);

        //    }
        //    else if (RefType == "Transfer IN")
        //    {
        //        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TI&LocId=" + LocationId + "','window','width=1024');", true);
        //    }
        //    else if (RefType == "Purchase Goods")
        //    {
        //        string strCmd = string.Format("window.open('../Purchase/PurchaseGoodsRec.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        //    }
        //    else if (RefType == "Stock Adjustment")
        //    {
        //        string strCmd = string.Format("window.open('../Inventory/StockAdjustment.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

        //    }
        //}




    }
    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;

        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId, HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lnkref = (LinkButton)e.Row.FindControl("lnkRef");


            if (lnkref.Text == "Opening Stock" || lnkref.Text == "Physical Inventory")
            {
                lnkref.Style.Add("text-decoration", "none");
                lnkref.Style.Add("cursor", "none");
                lnkref.CssClass = "labelComman";
                //lnkref.Visible = false;
                //e.Row.Cells[8].Text = "Opening Stock";
                //e.Row.Attributes["onmouseover"] = "this.style.cursor='crosshair';this.style.background='#f2f2f2';";
            }
        }
        AllPageCode();

    }
    //public string getLink(string str)
    //{

    //    if (str == "Opening Stock")
    //    {
    //        ((LinkButton)gvProductLadger.Rows[index].FindControl("lblTransId")).Text.Trim()

    //    }
    //    else if (str == "False")
    //    {
    //        str = "Close";


    //    }
    //    return str;
    //}

    //public void OpenProductRef(object sender, EventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('ProductLedgerRefTypeView.aspx?RefType=" + e + "&RefId=" + RefId + "','','height=650,width=900,scrollbars=Yes');", true);

    //}
    protected void btnReset_Click(object sender, EventArgs e)
    {

        txtProductcode.Text = "";
        txtProductName.Text = "";
        ddlbrandsearch.SelectedIndex = 0;
        ddlcategorysearch.SelectedIndex = 0;
        txtSuppliers.Text = "";
        ddlLocation.SelectedValue = Session["LocId"].ToString();

        AllPageCode();
    }
    protected void btngo_Click(object sender, EventArgs e)
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
            dtFilter = new DataView(dtFilter, "Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (txtProductcode.Text != "" || txtProductName.Text != "")
        {
            dtFilter = new DataView(dtFilter, "ProductId=" + hdnNewProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlLocation.SelectedIndex != 0)
        {
            dtFilter = new DataView(dtFilter, "Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (txtSuppliers.Text != "")
        {
            dtFilter = new DataView(dtFilter, "Supplier='" + txtSuppliers.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlbrandsearch.SelectedIndex != 0)
        {
            dtFilter = new DataView(dtFilter, "Brand='" + ddlbrandsearch.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlcategorysearch.SelectedIndex != 0)
        {
            dtFilter = new DataView(dtFilter, "Category='" + ddlcategorysearch.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (chkNegativestock.Checked)
        {
            dtFilter = new DataView(dtFilter, "Quantity<0", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (chkZeroCostPrice.Checked)
        {
            dtFilter = new DataView(dtFilter, "UnitCost<=0", "", DataViewRowState.CurrentRows).ToTable();
        }

        ArrayList arr = new ArrayList();


        string strGroupBy = string.Empty;
        string strReporttItle = string.Empty;
        if (rbtnNone.Checked)
        {
            strGroupBy = "0";

            if (ddlLocation.SelectedIndex == 0)
            {
                strReporttItle = "Stock Report for All Location ";
            }
            else
            {
                strReporttItle = "Stock Report for " + ddlLocation.SelectedItem.Text + " Location";
            }
        }
        if (rbtnByBrand.Checked)
        {
            strGroupBy = "1";

            if (ddlLocation.SelectedIndex == 0)
            {
                strReporttItle = "Stock Report by Product Brand for All Location ";
            }
            else
            {
                strReporttItle = "Stock Report by Product Brand for " + ddlLocation.SelectedItem.Text + " Location";
            }
        }
        if (rbtngroupByCategoryName.Checked)
        {
            strGroupBy = "2";
            if (ddlLocation.SelectedIndex == 0)
            {
                strReporttItle = "Stock Report by Product Category for All Location ";
            }
            else
            {
                strReporttItle = "Stock Report by Product Category for " + ddlLocation.SelectedItem.Text + " Location";
            }
        }
        if (rbtnGroupByRackName.Checked)
        {
            strGroupBy = "3";

            if (ddlLocation.SelectedIndex == 0)
            {
                strReporttItle = "Stock Report by Rack No. for All Location ";
            }
            else
            {
                strReporttItle = "Stock Report by Rack No. for " + ddlLocation.SelectedItem.Text + " Location";
            }
        }
        if (rbtnbysupplier.Checked)
        {
            strGroupBy = "4";
            if (ddlLocation.SelectedIndex == 0)
            {
                strReporttItle = "Stock Report by Supplier for All Location ";
            }
            else
            {
                strReporttItle = "Stock Report by Supplier for " + ddlLocation.SelectedItem.Text + " Location";
            }
        }


        arr.Add(strReporttItle);
        arr.Add(strGroupBy);
        arr.Add(dtFilter);

        Session["DtProductStock"] = arr;

        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/ProductStockPrint.aspx','window','width=1024');", true);
        GetReport();
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
    #region PrintReport

    protected void IbtnPrint_Command(object sender, EventArgs e)
    {

        Session["DtProductLedger"] = (DataTable)Session["dtFilter_Stock_Rpt"];

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/ProductLedgerPrint.aspx','window','width=1024');", true);

    }
    #endregion
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
    public string SetDecimal(string amount, string CurrencyID)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(CurrencyID, amount);

    }
    #region ProducttextChanged
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text.Trim() != "")
        {

            DataTable dtProduct = new DataTable();

            dtProduct = objProductMaster.GetProductMasterAll(Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());


            try
            {
                dtProduct = new DataView(dtProduct, "EProductName='" + txtProductName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

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
        if (txtProductcode.Text.Trim() != "")
        {

            DataTable dtProduct = new DataTable();

            dtProduct = objProductMaster.GetProductMasterAll(Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());


            try
            {
                dtProduct = new DataView(dtProduct, "ProductCode='" + txtProductcode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

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
    #endregion
    #region serviceMethod

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtSupplier, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();

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

    public void GetReport()
    {
       string strCompId = Session["CompId"].ToString();
        string strBrandId = Session["BrandId"].ToString();
        string strLocationId = Session["LocId"].ToString();
        //ObjInvdataset.EnforceConstraints = false;
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        string LocationName = "";
        string CompanyName_L = string.Empty;
        string Companytelno = string.Empty;
        string CompanyFaxno = string.Empty;
        string CompanyWebsite = string.Empty;



        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            CompanyName_L = DtCompany.Rows[0]["Company_Name_L"].ToString();
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            if (DtAddress.Rows[0]["Address"].ToString() != "")
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
            if (DtAddress.Rows[0]["Street"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
                }
            }
            if (DtAddress.Rows[0]["Block"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
                }

            }
            if (DtAddress.Rows[0]["Avenue"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
                }
            }

            if (DtAddress.Rows[0]["CityId"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["StateId"].ToString() != "")
            {


                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
                }

            }
            if (DtAddress.Rows[0]["CountryId"].ToString() != "")
            {

                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + LocationName;
                }
                else
                {
                    CompanyAddress = LocationName;
                }

            }
            if (DtAddress.Rows[0]["PinCode"].ToString() != "")
            {
                if (CompanyAddress != "")
                {
                    CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
                }

            }
            Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();

            if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
            {

                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
                }
            }
            if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
                }
            }
            if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
            {
                if (Companytelno != "")
                {
                    Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
                }
                else
                {
                    Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
                }
            }
            if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
            {
                CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
            }
            if (DtAddress.Rows[0]["WebSite"].ToString() != "")
            {
                CompanyWebsite = DtAddress.Rows[0]["WebSite"].ToString();
            }



        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
        }
        DataTable dtemployee = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());

        try
        {
            dtemployee = new DataView(dtemployee, "Emp_Id=" + Session["EmpId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string signatureurl = string.Empty;
        if (dtemployee.Rows.Count > 0)
        {


            signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field2"].ToString();

        }

        //find group header by report object

        DevExpress.XtraReports.UI.GroupHeaderBand GhLocationName = (DevExpress.XtraReports.UI.GroupHeaderBand)objProductStockPrint.FindControl("GroupHeader1", true);
        DevExpress.XtraReports.UI.GroupHeaderBand GhSupplierName = (DevExpress.XtraReports.UI.GroupHeaderBand)objProductStockPrint.FindControl("GroupHeader2", true);
        DevExpress.XtraReports.UI.GroupHeaderBand Ghbrand = (DevExpress.XtraReports.UI.GroupHeaderBand)objProductStockPrint.FindControl("GroupHeader3", true);
        DevExpress.XtraReports.UI.GroupHeaderBand GhCategory = (DevExpress.XtraReports.UI.GroupHeaderBand)objProductStockPrint.FindControl("GroupHeader4", true);
        DevExpress.XtraReports.UI.GroupHeaderBand GhRackName = (DevExpress.XtraReports.UI.GroupHeaderBand)objProductStockPrint.FindControl("GroupHeader5", true);

        GhSupplierName.Visible = false;
        Ghbrand.Visible = false;
        GhCategory.Visible = false;
        GhRackName.Visible = false;


        try
        {

            ArrayList ObjArr = (ArrayList)Session["DtProductStock"];


            if (ObjArr[1].ToString() == "1")
            {
                Ghbrand.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Location_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Brand", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                Ghbrand.Visible = true;
            }
            if (ObjArr[1].ToString() == "2")
            {
                GhCategory.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Location_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Category", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                GhCategory.Visible = true;

            }
            if (ObjArr[1].ToString() == "3")
            {
                GhRackName.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Location_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("RackName", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                GhRackName.Visible = true;

            }
            if (ObjArr[1].ToString() == "4")
            {
                GhSupplierName.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Location_Name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("Supplier", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
                GhSupplierName.Visible = true;
            }
            DataTable dt = (DataTable)ObjArr[2];

            objProductStockPrint.DataSource = dt;
            objProductStockPrint.DataMember = "sp_Inv_StockDetail_SelectRow_Report";
            rptViewer.Report = objProductStockPrint;
            rptToolBar.ReportViewer = rptViewer;
            objProductStockPrint.setcompanyname(CompanyName);
            objProductStockPrint.setSignature(signatureurl);
            objProductStockPrint.setCompanyArebicName(CompanyName_L);
            objProductStockPrint.setCompanyTelNo(Companytelno);
            objProductStockPrint.setCompanyFaxNo(CompanyFaxno);
            objProductStockPrint.setCompanyWebsite(CompanyWebsite);
            objProductStockPrint.setReportTitle(ObjArr[0].ToString());
            objProductStockPrint.setcompanyAddress(CompanyAddress);
            objProductStockPrint.SetImage(Imageurl);

            objProductStockPrint.ExportToPdf(Server.MapPath("~/Temp/Product Stock -" + dt.Rows[0]["ProductId"].ToString() + ".pdf"));
            ViewState["Path"] = "Product Ledger -" + dt.Rows[0]["ProductId"].ToString() + ".pdf";
        }
        catch
        {

        }
    }
}