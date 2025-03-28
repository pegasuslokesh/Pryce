using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;

public partial class Inventory_Report_StockTransferReport : System.Web.UI.Page
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
    TransferOutPrint objTransferOutPrint = null;
    TransferInPrint objTransferInPrint = null;
    TransferDetailReport objTransferdetailReport = null;
    PurchaseRequestHeader InvPr = null;
    InventoryDataSet ObjInvdataset = new InventoryDataSet();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;    
    CurrencyMaster objCurrency = null;
    PageControlCommon pageCmn = null;
    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;

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
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objTransferOutPrint = new TransferOutPrint(Session["DBConnection"].ToString());
        objTransferInPrint = new TransferInPrint(Session["DBConnection"].ToString());
        objTransferdetailReport = new TransferDetailReport(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        pageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = ObjSysParam.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "335", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("335", (DataTable)Session["ModuleName"]);
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
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId,HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
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
        ddlLocation.SelectedIndex = 0;

        AllPageCode();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        Session["DateCrediteria"] = "";

        string strsql = string.Empty;


        strsql = "select th.Trans_Id,th.VoucherNo,th.TDate as VoucherDate,th.Remark,LM.Location_Name as Location_To,lm.Field1 as Currency_Id,pm.ProductCode,pm.EProductName,um.Unit_Name,TD.RequestQty,TD.OutQty,TD.ReceivedQty,Inv_StockDetail.Field1 as UnitCost,(TD.ReceivedQty*cast(Inv_StockDetail.Field1 as numeric(18,6))) as LineTotal  from Inv_TransferHeader as TH  inner join Inv_TransferDetail as TD on TH.Trans_Id=TD.Transfer_Id inner join Inv_ProductMaster as PM on td.ProductId=PM.ProductId inner join Inv_UnitMaster as UM on td.Unit_Id=UM.Unit_Id left join Inv_StockDetail  on td.ProductId=Inv_StockDetail.ProductId and Inv_StockDetail.Location_Id=td.FromLocationID inner join Set_LocationMaster as LM on th.ToLocationID=LM.Location_Id where TH.IsActive='True' and TH.Post='Y' and TH.FromLocationID=" + Session["LocId"].ToString() + "";

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

            strsql = strsql + " and th.TDate>='" + txtFromDate.Text + "' and th.TDate<='" + ToDate.ToString() + "'";

            Session["DateCrediteria"] = "From : " + txtFromDate.Text + " To : " + txtToDate.Text;

        }
        if (ddlLocation.SelectedIndex != 0)
        {

            strsql = strsql + " and th.ToLocationID=" + ddlLocation.SelectedValue + " ";
        }
        if (txtProductcode.Text != "")
        {
            strsql = strsql + " and TD.ProductId=" + hdnNewProductId.Value + " ";
        }

        strsql = strsql + " order by th.TDate ";

        DataTable dt = objDa.return_DataTable(strsql);

        Session["dtTranferOut"] = dt;


        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=0&&Type=TO','window','width=1024');", true);        
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

        Session["DtProductLedger"] = (DataTable)Session["dtFilter_Supplier_TR"];

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
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());

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
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());

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