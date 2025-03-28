using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Collections;

public partial class Inventory_Report_StockAdjustmentreport : System.Web.UI.Page
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

    StockAdjustmentPrint objReport = null;
    CompanyMaster Objcompany = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

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

        Page.Title = ObjSysParam.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "336", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtFromDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            txtToDate_CalendarExtender.Format = Session["DateFormat"].ToString();
            FillddlLocation();
        }
        AllPageCode();
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("336", (DataTable)Session["ModuleName"]);
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
        ddlLocation.SelectedIndex = 0;

        AllPageCode();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {



        string strReporttitle = "Adjustment Report For All Location";
        string strdateCriteria = string.Empty;

        string GroupBy = string.Empty;
        DataTable dtFilter = new DataTable();

        InventoryDataSet rptdata = new InventoryDataSet();

        rptdata.EnforceConstraints = false;
        InventoryDataSetTableAdapters.sp_Inv_StockAdjustment_SelectRow_ReportTableAdapter adp = new InventoryDataSetTableAdapters.sp_Inv_StockAdjustment_SelectRow_ReportTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(rptdata.sp_Inv_StockAdjustment_SelectRow_Report);

        dtFilter = rptdata.sp_Inv_StockAdjustment_SelectRow_Report;



        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            strdateCriteria = "From : " + txtFromDate.Text + " To : " + txtToDate.Text;

            dtFilter = new DataView(dtFilter, "Vdate>='" + txtFromDate.Text + "' and Vdate<='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (ddlLocation.SelectedIndex != 0)
        {

            strReporttitle = "Adjustment Report For " + ddlLocation.SelectedItem.Text;
            dtFilter = new DataView(dtFilter, "Location_Name='" + ddlLocation.SelectedItem.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (txtProductcode.Text != "")
        {
            dtFilter = new DataView(dtFilter, "ProductId='" + hdnNewProductId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlPosted.SelectedIndex != 0)
        {
            dtFilter = new DataView(dtFilter, "Post='" + ddlPosted.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (ddlPosted.SelectedIndex == 1)
            {
                strReporttitle = "Posted " + strReporttitle;
            }
            else
            {
                strReporttitle = "UnPosted " + strReporttitle;
            }
        }

        ArrayList objarr = new ArrayList();

        objarr.Add(strReporttitle);
        objarr.Add(strdateCriteria);
        objarr.Add(dtFilter);

        Session["dtAdjustmentReport"] = objarr;


        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/StockAdjustment_Print.aspx','window','width=1024');", true);
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

        Session["DtProductLedger"] = (DataTable)Session["dtFilter_Stock_Adj"];

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
            txtProductcode.Text = "";

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
            txtProductName.Text = "";

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
}