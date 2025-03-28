using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.IO;

public partial class Inventory_ProductLadger : BasePage
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
    Inv_ModelMaster ObjInvModelMaster = null;
    PageControlCommon objPageCmn = null;
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
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = ObjSysParam.GetSysTitle();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ProductLadger.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            btnExportExcel.Visible = clsPagePermission.bDownload;

            FillddlLocation();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            txtToDate_CalendarExtender.Format = Session["DateFormat"].ToString();

            //here we modify code for show cost price according the login user permission
            //code created by jitendra on 13-08-2016
            try
            {
                gvProductLadger.Columns[3].Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission("24", "20", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));

                gvProductLadger.Columns[5].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            }
            catch
            {

            }
            FillRefTye();
        }
        //AllPageCode();
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region ModelFilter
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_No like '%" + prefixText.ToString() + "%'", "Model_No Asc", DataViewRowState.CurrentRows).ToTable();


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }


        return txt;
    }

    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        hdnModelId.Value = "0";
        if (txtModelNo.Text != "")
        {

            DataTable dt = new DataView(ObjInvModelMaster.GetModelMasterByModelNo(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), "True", txtModelNo.Text.ToString().Trim()), "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                txtProductcode.Text = "";
                txtProductcode.Visible = false;
                ddlModelProduct.Visible = true;
                FillProductList(dt.Rows[0]["Trans_Id"].ToString());
                hdnModelId.Value = dt.Rows[0]["Trans_Id"].ToString();
                hdnNewProductId.Value = "0";
            }
            else
            {
                FillProductList("0");
                txtProductcode.Text = "";
                txtProductcode.Visible = true;
                ddlModelProduct.Visible = false;
                txtModelNo.Text = "";
                txtModelNo.Focus();
            }
        }
        else
        {
            txtProductcode.Text = "";
            txtProductName.Text = "";
            txtProductcode.Visible = true;
            ddlModelProduct.Visible = false;
        }


    }
    public void FillProductList(string strModelId)
    {
        ddlModelProduct.Items.Clear();

        DataTable dt = objDa.return_DataTable("select ProductId, ProductCode, EProductName from inv_productmaster where modelno =" + strModelId + "");

        ddlModelProduct.DataSource = dt;
        ddlModelProduct.DataTextField = "ProductCode";
        ddlModelProduct.DataValueField = "ProductId";
        ddlModelProduct.DataBind();
        ddlModelProduct.Items.Insert(0, "--Select--");
    }
    protected void ddlModelProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtProductcode.Text = "";
        txtProductName.Text = "";
        if (ddlModelProduct.SelectedIndex != 0)
        {
            txtProductcode.Text = ddlModelProduct.SelectedItem.Text.ToString();

        }
        txtProductCode_TextChanged(null, null);
    }
    #endregion

    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

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
            lstLocation.DataSource = null;
            lstLocation.DataBind();

            lstLocation.DataSource = dtLoc;
            lstLocation.DataTextField = "Location_Name";
            lstLocation.DataValueField = "Location_Id";
            lstLocation.DataBind();
            lstLocation.SelectedValue = Session["LocId"].ToString();
            btnPushDept_Click(null, null);

        }
        else
        {
            lstLocation.Items.Clear();
            lstLocation.DataSource = null;
            lstLocation.DataBind();

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

                    string strCmd = string.Format("window.open('../Purchase/PurchaseReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','_blank','width=1024, ');");
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
                    string strCmd = string.Format("window.open('../Purchase/PurchaseGoodsRec.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','_blank','width=1024, ');");
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
                    string strCmd = string.Format("window.open('../Sales/SalesReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','_blank','width=1024, ');");
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
                    string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','_blank','width=1024, ');");
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
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TO&LocId=" + LocationId + "','_blank','width=1024');", true);
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
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TI&LocId=" + LocationId + "','_blank','width=1024');", true);
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
                    string strCmd = string.Format("window.open('../Inventory/StockAdjustment.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','_blank','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }

            }

            else if (RefType == "Delivery Voucher")
            {
                if (IsObjectPermission("144", "327"))
                {
                    string strCmd = string.Format("window.open('../Sales/DeliveryVoucher.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','_blank','width=1024, ');");
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

        //AllPageCode();

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
    public void FillRefTye()
    {
        //DataTable dt = ObjProductLadger.GetProductLedgerAll();

        //dt = new DataView(dt, "", "Ref_Type", DataViewRowState.CurrentRows).ToTable(true, "Ref_Type", "TransType");

        Ddlreftype.DataSource = getReflist();
        Ddlreftype.DataTextField = "Ref_Type";
        Ddlreftype.DataValueField = "TransType";
        Ddlreftype.DataBind();

        Ddlreftype.Items.Insert(0, "ALL");
    }

    public DataTable getReflist()
    {
        return objDa.return_DataTable("SELECT distinct Inv_ProductLedger.TransType,CASE WHEN Inv_ProductLedger.TransType = 'SI' THEN 'Sales Invoice' WHEN Inv_ProductLedger.TransType = 'SR' THEN 'Sales Return' WHEN Inv_ProductLedger.TransType = 'PR' THEN 'Purchase Return' WHEN Inv_ProductLedger.TransType = 'TI' THEN 'Transfer IN' WHEN Inv_ProductLedger.TransType = 'TO' THEN 'Transfer Out' WHEN Inv_ProductLedger.TransType = 'PG' THEN 'Purchase Goods' WHEN Inv_ProductLedger.TransType = 'SA' THEN 'Stock Adjustment' WHEN Inv_ProductLedger.TransType = 'OP' THEN 'Opening Stock' WHEN Inv_ProductLedger.TransType = 'FO' THEN 'Production Out' WHEN Inv_ProductLedger.TransType = 'FI' THEN 'Production In' WHEN Inv_ProductLedger.TransType = 'FR' THEN 'Production Return' WHEN Inv_ProductLedger.TransType = 'DV' THEN 'Delivery Voucher' WHEN Inv_ProductLedger.TransType = 'Ph' THEN 'Physical Inventory' WHEN Inv_ProductLedger.TransType = 'RM' THEN 'RMA Outward' WHEN Inv_ProductLedger.TransType = 'IW' THEN 'RMA Inward' END AS Ref_Type FROM Inv_ProductLedger");

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
        //AllPageCode();

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
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        fillGrid();
        GetOpeningandClosingBalance();
        //AllPageCode();
        txtValue.Focus();
    }
    public void fillGrid()
    {

        DataTable dtCheck = objDa.return_DataTable("Select * from Inv_ProductMaster where ProductCode='" + txtProductcode.Text.Trim() + "'");

        string strFirstFinanceYearId = string.Empty;
        string stropTransId = "0";

        if (txtProductName.Text.Trim() == "")
        {
            hdnNewProductId.Value = "0";
        }

        string strProductWhereClause = string.Empty;
        if (hdnNewProductId.Value == "0" && hdnModelId.Value != "0")
        {
            strProductWhereClause = " and Inv_ProductMaster.ModelNo='" + hdnModelId.Value + "' ";
        }
        else
        {
            if (dtCheck.Rows.Count > 0)
            {
                strProductWhereClause = " and Inv_ProductMaster.ProductId='" + hdnNewProductId.Value + "' ";
            }
            else
            {
                strProductWhereClause = " and Inv_ProductMaster.ProductCode LIKE '%" + txtProductcode.Text.Trim() + "%' ";
            }
        }

        if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "")
        {
            string strsqlfinance = "select top 1 Trans_Id  from Ac_Finance_Year_Info where IsActive='True' and company_id=" + Session["CompId"].ToString() + " ";
            strFirstFinanceYearId = objDa.return_DataTable(strsqlfinance).Rows[0][0].ToString();
        }
        else
        {
            string strsqlfinance = "select top 1 Trans_Id  from Ac_Finance_Year_Info where IsActive='True' and company_id=" + Session["CompId"].ToString() + " and '" + txtFromDate.Text + "' between From_date and To_Date";
            try
            {
                strFirstFinanceYearId = objDa.return_DataTable(strsqlfinance).Rows[0][0].ToString();
            }
            catch
            {
                strFirstFinanceYearId = "0";
            }
        }


        string strselectquery = "Select Inv_ProductLedger.*,case when Inv_ProductLedger.InOut='o' THEN 'Out'        ELSE 'In' end as Status,        case when Inv_ProductLedger.TransType='SI' THEN 'Sales Invoice'         when Inv_ProductLedger.TransType='SR' THEN 'Sales Return'   when Inv_ProductLedger.TransType='Ph' THEN 'Physical Inventory'         when Inv_ProductLedger.TransType='PR' THEN 'Purchase Return'        when Inv_ProductLedger.TransType='TI' THEN 'Transfer IN'        when Inv_ProductLedger.TransType='TO' THEN 'Transfer Out'        when Inv_ProductLedger.TransType='PG' THEN 'Purchase Goods'        when Inv_ProductLedger.TransType='SA' THEN 'Stock Adjustment'        when Inv_ProductLedger.TransType='OP' THEN 'Opening Stock'        when Inv_ProductLedger.TransType='FO' THEN 'Production Out'      when Inv_ProductLedger.TransType='FI' THEN 'Production In'      when Inv_ProductLedger.TransType='FR' THEN 'Production Return'      when Inv_ProductLedger.TransType='DV' THEN 'Delivery Voucher'   WHEN Inv_ProductLedger.TransType = 'RM' THEN 'RMA Outward'     WHEN Inv_ProductLedger.TransType = 'IW' THEN 'RMA Inward'      end  as Ref_Type ,         Case  when Inv_ProductLedger.TransType='Ph' Then  cast((select Inv_PhysicalHeader.VoucherNo from Inv_PhysicalHeader where Inv_PhysicalHeader.Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))                       When Inv_ProductLedger.TransType='SI' Then  cast((Select Invoice_No From Inv_SalesInvoiceHeader where Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))          When Inv_ProductLedger.TransType='SR' Then Cast((Select Return_No From Inv_SalesReturnHeader where Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))          When Inv_ProductLedger.TransType='PR' Then Cast((Select PReturn_No From Inv_PurchaseReturnHeader where Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))          When Inv_ProductLedger.TransType='TI' Then Cast((Select VoucherNo From Inv_TransferHeader where Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))          When Inv_ProductLedger.TransType='TO' Then Cast((Select VoucherNo From Inv_TransferHeader where Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))          When Inv_ProductLedger.TransType='SA' Then Cast((Select VoucherNo From Inv_AdjustHeader where TransId=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))         when Inv_ProductLedger.TransType='PG' THEN Cast((Select InvoiceNo From dbo.Inv_PurchaseInvoiceHeader where TransId=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))           when Inv_ProductLedger.TransType='FO' or Inv_ProductLedger.TransType='FI' or Inv_ProductLedger.TransType='FR'  THEN Cast((Select Inv_Production_Process.Job_No From dbo.Inv_Production_Process where Inv_Production_Process.Id=Inv_ProductLedger.TransTypeId      )as Nvarchar(Max))          when Inv_ProductLedger.TransType='DV'  THEN Cast((Select Inv_SalesDeliveryVoucher_Header.Voucher_No From dbo.Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.Trans_Id=Inv_ProductLedger.TransTypeId)as Nvarchar(Max))       when Inv_ProductLedger.TransType = 'RM' then (select SM_GetPass_Header.Get_Pass_No from SM_GetPass_Header where SM_GetPass_Header.Trans_Id=Inv_ProductLedger.TransTypeId  )   when Inv_ProductLedger.TransType = 'IW' then (select SM_Inward_header.Inward_Voucher_No from SM_Inward_header where SM_Inward_header.Trans_Id=Inv_ProductLedger.TransTypeId  )                end as Ref_Id        ,((Inv_ProductLedger.QuantityIn)+(Inv_ProductLedger.QuantityOut)) as Quantity,Inv_ProductMaster.EProductName,Inv_ProductMaster.ProductCode,Inv_ProductMaster.AlternateId1,Inv_ProductMaster.AlternateId2,Inv_ProductMaster.AlternateId3 ,        (select Set_LocationMaster.Location_Name from Set_LocationMaster where Set_LocationMaster.Location_Id=Inv_ProductLedger.Location_Id) as LocationName  ,        (select Set_LocationMaster.Field1 from Set_LocationMaster where Set_LocationMaster.Location_Id=Inv_ProductLedger.Location_Id) as CurrencyID  ,       (select Unit_Name from Inv_UnitMaster where  Unit_Id=Inv_ProductLedger.UnitId) as UnitName    ,   case when(TransType='Dv') then    (select Ems_ContactMaster.Name from Inv_SalesDeliveryVoucher_Header inner join Ems_ContactMaster on Inv_SalesDeliveryVoucher_Header.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_SalesDeliveryVoucher_Header.Trans_Id=Inv_ProductLedger.TransTypeId )       when(TransType='SI') then    (select Ems_ContactMaster.Name from Inv_SalesInvoiceHeader inner join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id=Ems_ContactMaster.Trans_Id where Inv_SalesInvoiceHeader.Trans_Id=Inv_ProductLedger.TransTypeId ) when(TransType='SR') then    (select Ems_ContactMaster.Name from Inv_SalesReturnHeader inner join Ems_ContactMaster on Inv_SalesReturnHeader.Customer_Id=Ems_ContactMaster.Trans_Id where Inv_SalesReturnHeader.Trans_Id=Inv_ProductLedger.TransTypeId )   when(TransType='PG') then    (select Ems_ContactMaster.Name from Inv_PurchaseInvoiceHeader inner join Ems_ContactMaster on Inv_PurchaseInvoiceHeader.SupplierId=Ems_ContactMaster.Trans_Id where Inv_PurchaseInvoiceHeader.TransId=Inv_ProductLedger.TransTypeId )      when(TransType='FI' or TransType='FO') then    (select Ems_ContactMaster.Name from Inv_Production_Process inner join Inv_ProductionRequestHeader on Inv_Production_Process.Ref_Production_Req_No=Inv_ProductionRequestHeader.Trans_Id  inner join Ems_ContactMaster on  Inv_ProductionRequestHeader.Customer_Id=Ems_ContactMaster.Trans_Id  where  Inv_Production_Process.Id=Inv_ProductLedger.TransTypeId  )      end as ContactName  ,  case when (select Set_UserMaster.Emp_Id from Set_UserMaster where  Set_UserMaster.Company_Id=Inv_ProductLedger.Company_Id and Set_UserMaster.User_Id=Inv_ProductLedger.ModifiedBy)=0 then 'SuperAdmin'     else (select set_employeemaster.Emp_Name from set_employeemaster where set_employeemaster.Emp_Id in (select Set_UserMaster.Emp_Id from Set_UserMaster where  Set_UserMaster.Company_Id=Inv_ProductLedger.Company_Id and Set_UserMaster.User_Id=Inv_ProductLedger.ModifiedBy)) end as ModifiedUser";
        string strbalqty = " (select SUM(PG.QuantityIn)-SUM(QuantityOut) from (select * from Inv_ProductLedger where TransType<>'op' and IsActive='True' union all select * from Inv_ProductLedger where Finance_Year_Id='" + strFirstFinanceYearId + "' and TransType='op' and IsActive='True')PG where PG.Trans_Id<= Inv_ProductLedger.Trans_Id   ";
        string strCondition = " From (select * from Inv_ProductLedger where TransType<>'op' and IsActive='True' union all select * from Inv_ProductLedger where Finance_Year_Id='" + strFirstFinanceYearId + "' and TransType='op' and IsActive='True')Inv_ProductLedger left Join        Inv_ProductMaster  On Inv_ProductLedger.ProductId=Inv_ProductMaster.ProductId    Where Inv_ProductLedger.IsActive='True' and  Inv_ProductLedger.Company_Id=" + Session["CompId"].ToString() + " and Inv_ProductLedger.Brand_Id=" + Session["BrandId"].ToString() + strProductWhereClause;


        DataTable dt = new DataTable();
        string LocationValue = "";


        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {

            if (LocationValue == "")
            {
                LocationValue += "'" + lstLocationSelect.Items[i].Value + "'";
            }
            else
            {
                LocationValue += ",'" + lstLocationSelect.Items[i].Value + "'";
            }
        }
        if (LocationValue != "")
        {
            strbalqty += " and PG.Location_Id in(" + LocationValue + ")";

            strCondition += " and Inv_ProductLedger.Location_Id in(" + LocationValue + ")";

        }

        //if (ddlLocation.SelectedIndex == 0)
        //{

        //    string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        //    if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        //    {
        //        if (LocIds != "")
        //        {

        //            strbalqty += " and PG.Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")";
        //            strCondition += " and Inv_ProductLedger.Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")";

        //        }
        //    }
        //}
        //else
        //{
        //    strbalqty += " and PG.Location_Id in(" + ddlLocation.SelectedValue + ")";

        //    strCondition += " and Inv_ProductLedger.Location_Id in(" + ddlLocation.SelectedValue + ")";

        //}



        //if from date and to date is blank then we will allow only first financial year openin gbalance otheriwse it will show duplicate 


        //if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "")
        //{
        //    string strsqlfinance = "select top 1 Trans_Id  from Ac_Finance_Year_Info where IsActive='True' and company_id=" + Session["CompId"].ToString() + " ";
        //    strFirstFinanceYearId = objDa.return_DataTable(strsqlfinance).Rows[0][0].ToString();

        //    string stropsql = "select Trans_Id from Inv_ProductLedger where ProductId=" + hdnNewProductId.Value + " and  TransType='OP'  and Finance_Year_Id<>" + strFirstFinanceYearId + "";
        //    DataTable dtop = objDa.return_DataTable(stropsql);


        //    foreach (DataRow dr in dtop.Rows)
        //    {
        //        stropTransId = stropTransId + "," + dr["Trans_Id"].ToString();

        //    }


        //    strbalqty += " and PG.Trans_Id  not  in (" + stropTransId + ")";
        //    strCondition += " and Inv_ProductLedger.Trans_Id not  in (" + stropTransId + ")";


        //}
        //else
        //{
        //    DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
        //    ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);

        //    string strsqlfinance = "select top 1 Trans_Id  from Ac_Finance_Year_Info where IsActive='True' and company_id=" + Session["CompId"].ToString() + " and From_date<='" + txtFromDate.Text + "' and To_Date>='" + txtFromDate.Text + "' ";
        //    try
        //    {
        //        strFirstFinanceYearId = objDa.return_DataTable(strsqlfinance).Rows[0][0].ToString();
        //    }
        //    catch
        //    {
        //        strFirstFinanceYearId = "0";
        //    }

        //    string stropsql = "select Trans_Id from Inv_ProductLedger where ProductId=" + hdnNewProductId.Value + " and  TransType='OP'  and Finance_Year_Id<>" + strFirstFinanceYearId + "";
        //    DataTable dtop = objDa.return_DataTable(stropsql);


        //    foreach (DataRow dr in dtop.Rows)
        //    {
        //        stropTransId = stropTransId + "," + dr["Trans_Id"].ToString();

        //    }


        //    strbalqty += " and PG.Trans_Id  not  in (" + stropTransId + ")";
        //    strCondition += " and Inv_ProductLedger.Trans_Id not  in (" + stropTransId + ")";

        //}

        //bellow line has been commented on 05-02-2020 as per discussion with thoms and neeraj sir
        //strbalqty += " and PG.Finance_Year_Id = " + Session["FinanceYearId"].ToString() + "";
        //strCondition += " and Inv_ProductLedger.Finance_Year_Id = " + Session["FinanceYearId"].ToString() + "";

        if (txtFromDate.Text != "" || txtToDate.Text != "")
        {
            try
            {
                DateTime ToDate = Convert.ToDateTime(txtToDate.Text);
                ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59);

                DateTime FromDate = Convert.ToDateTime(txtFromDate.Text);
                FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 00, 00, 0);



                //strbalqty += " and PG.ModifiedDate>='" + FromDate.ToString().Trim() + "' and PG.ModifiedDate<='" + ToDate.ToString().Trim() + "'";

                strCondition += " and Inv_ProductLedger.ModifiedDate>='" + FromDate.ToString().Trim() + "' and Inv_ProductLedger.ModifiedDate<='" + ToDate.ToString().Trim() + "'";


                //dt = new DataView(dt, "ModifiedDate>='" + FromDate.ToString().Trim() + "' and ModifiedDate<='" + ToDate.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {


            }
        }

        //FOR FILTER BY REF TYPE 
        if (Ddlreftype.SelectedIndex > 0)
        {
            strbalqty += " and PG.TransType='" + Ddlreftype.SelectedValue.Trim() + "'";
            strCondition += " and Inv_ProductLedger.TransType='" + Ddlreftype.SelectedValue.Trim() + "'";

            //dt = new DataView(dt, "TransType='"+Ddlreftype.SelectedValue.Trim()+"'", "", DataViewRowState.CurrentRows).ToTable();
        }

        string condition = string.Empty;



        if (txtValue.Text.Trim() != "")
        {

            if (ddlOption.SelectedIndex != 0)
            {

                if (ddlOption.SelectedIndex == 1)
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String)='" + txtValue.Text.Trim() + "'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String)='" + txtValue.Text.Trim() + "'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String)='" + txtValue.Text.Trim() + "'";

                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                    }
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";

                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
                else
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String) like '" + txtValue.Text.Trim() + "%'";

                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
            }
        }

        if (condition.Trim() != "")
        {
            //strbalqty += " and " + condition;

            //strCondition += " and " + condition + "  and Inv_ProductLedger.IsActive='True' and Inv_ProductLedger.Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + " Order By Inv_ProductLedger.Trans_Id Asc";


            strCondition += " and " + condition + "  and Inv_ProductLedger.IsActive='True'  Order By Inv_ProductLedger.Trans_Id Asc";

        }
        else
        {
            //strCondition += "  and Inv_ProductLedger.IsActive='True' and  Inv_ProductLedger.Finance_Year_Id=" + HttpContext.Current.Session["FinanceYearId"].ToString() + " Order By Inv_ProductLedger.Trans_Id Asc";

            strCondition += "  and Inv_ProductLedger.IsActive='True'  Order By Inv_ProductLedger.Trans_Id Asc";
        }


        if (hdnNewProductId.Value == "0" && hdnModelId.Value != "0")
        {
            strbalqty = strbalqty + "  and pg.ProductId in (select productid from Inv_ProductMaster where modelNo='" + hdnModelId.Value + "')) as BalQty";
        }
        else
        {
            strbalqty = strbalqty + "  and pg.ProductId=Inv_ProductLedger.ProductId) as BalQty";
        }

        string strsql = strselectquery + " , " + strbalqty + " " + strCondition;


        dt = objDa.return_DataTable(strsql);



        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductLadger, dt, "", "");

        float Purchase = 0;
        float Sales = 0;
        float AdjustmentIn = 0;
        float Deleviry = 0;
        float PhysicalIN = 0;
        float PhysicalOut = 0;
        float Adjustout = 0;
        float Return = 0;
        float PurchaseReturn = 0;
        float TransferIN = 0;
        float TransferOut = 0;
        float ProductionIn = 0;
        float ProductIonOUt = 0;
        float RmaOutWord = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["TransType"].ToString() == "PG")
            {
                Purchase = Purchase + float.Parse(dt.Rows[i]["QuantityIn"].ToString());

            }
            else if (dt.Rows[i]["TransType"].ToString() == "Ph" && dt.Rows[i]["InOut"].ToString() == "I")
            {
                PhysicalIN = PhysicalIN + float.Parse(dt.Rows[i]["QuantityIn"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "Ph" && dt.Rows[i]["InOut"].ToString() == "O")
            {
                PhysicalOut = PhysicalOut + float.Parse(dt.Rows[i]["QuantityOut"].ToString());

            }
            else if (dt.Rows[i]["TransType"].ToString() == "SA" && dt.Rows[i]["InOut"].ToString() == "I")
            {
                AdjustmentIn = AdjustmentIn + float.Parse(dt.Rows[i]["QuantityIn"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "SI")
            {
                Sales = Sales + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "SR")
            {
                Return = Return + float.Parse(dt.Rows[i]["QuantityIn"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "PR")
            {
                PurchaseReturn = PurchaseReturn + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "SA" && dt.Rows[i]["InOut"].ToString() == "O")
            {
                Adjustout = Adjustout + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "DV")
            {
                Deleviry = Deleviry + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "TI")
            {
                TransferIN = TransferIN + float.Parse(dt.Rows[i]["QuantityIn"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "TO")
            {
                TransferOut = TransferOut + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "FI")
            {
                ProductionIn = ProductionIn + float.Parse(dt.Rows[i]["QuantityIn"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "FO")
            {
                ProductIonOUt = ProductIonOUt + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }
            else if (dt.Rows[i]["TransType"].ToString() == "RM")
            {
                RmaOutWord = RmaOutWord + float.Parse(dt.Rows[i]["QuantityOut"].ToString());
            }

        }

        float INBalance = Purchase + AdjustmentIn + PhysicalIN + Return + TransferIN + ProductionIn;
        float OutBalamce = Sales + Deleviry + Adjustout + PurchaseReturn + TransferOut + ProductIonOUt + RmaOutWord + PhysicalOut;

        float NetBalance = INBalance - OutBalamce;

        tblPurchaseAmount.InnerText = Purchase.ToString();
        tblSalesAmount.InnerText = Sales.ToString();
        tblAdjustmentIn.InnerText = AdjustmentIn.ToString();
        tblDelivery.InnerText = Deleviry.ToString();
        tblPhysicalIn.InnerText = PhysicalIN.ToString();
        tblPhysicalOut.InnerText = PhysicalOut.ToString();
        tblAdjustmentOut.InnerText = Adjustout.ToString();
        tblReturn.InnerText = Return.ToString();
        tblPurchaseReturn.InnerText = PurchaseReturn.ToString();
        tblINTotalAmount.InnerText = INBalance.ToString();
        tblOutTotalAmount.InnerText = OutBalamce.ToString();
        tblRemaningBalance.InnerText = NetBalance.ToString();
        tblTransferIn.InnerText = TransferIN.ToString();
        tblTransferOut.InnerText = TransferOut.ToString();
        tblProductionIn.InnerText = ProductionIn.ToString();
        tblProductionOut.InnerText = ProductIonOUt.ToString();
        tblRmaOutword.InnerText = RmaOutWord.ToString();

        Session["DtPl"] = dt;
        Session["dtFilter_Pro_Ledg"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        if (txtValue.Text != "")
        {
            btnbind_Click(null, null);
        }
        GetOpeningandClosingBalance();
        //AllPageCode();
    }

    public void ExportTableData(DataTable dtdata)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ProductList.xls"));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtdata.Copy();
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }
    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();
    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        fillGrid();
        GetOpeningandClosingBalance();
        //AllPageCode();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtProductcode.Text = "";
        txtProductName.Text = "";
        Ddlreftype.SelectedIndex = 0;
        try
        {
            ddlModelProduct.SelectedIndex = 0;
        }
        catch (Exception ex)
        {

        }
        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();
        hdnModelId.Value = "0";
        // btnRefresh_Click(null, null);
        GetOpeningandClosingBalance();
        //AllPageCode();
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (txtProductcode.Text == "")
        {
            DisplayMessage("Enter Product Id");
            txtModelNo.Focus();
            return;

        }
        if (txtProductName.Text.Trim() == "" && string.IsNullOrEmpty(txtModelNo.Text))
        {
            //DisplayMessage("Enter Product Name or Model No");
            //txtModelNo.Focus();
            //return;
        }
        else
        {

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









        //if (txtFromDate.Text.Trim() == "")
        //{

        //    DisplayMessage("Select From Date");
        //    txtFromDate.Focus();
        //    return;
        //}
        //if (txtToDate.Text.Trim() == "")
        //{

        //    DisplayMessage("Select To Date");
        //    txtToDate.Focus();
        //    return;
        //}

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if (Convert.ToDateTime(txtFromDate.Text.Trim()) > Convert.ToDateTime(txtToDate.Text.Trim()))
            {

                DisplayMessage("From Date Must Be Less then To Date");
                txtToDate.Focus();
                return;
            }
        }
        fillGrid();

        GetOpeningandClosingBalance();
        //AllPageCode();
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
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        Session["DtProductLedger"] = (DataTable)Session["dtFilter_Pro_Ledg"];
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/ProductLedgerPrint.aspx','window','width=1024');", true);
    }
    #endregion
    public void GetOpeningandClosingBalance()
    {
        DataTable Dt = new DataTable();
        DataTable DtCopy = new DataTable();

        txtOpeningBalance.Text = "0";
        txtClosingBalance.Text = "0";

        if (Session["dtFilter_Pro_Ledg"] != null)
        {
            Dt = (DataTable)Session["dtFilter_Pro_Ledg"];


            if (Dt.Rows.Count > 0)
            {
                //if (txtFromDate.Text != "")
                //{
                //    txtOpeningBalance.Text = objDa.return_DataTable("select       sum(QuantityIn)-sum(QuantityOut)  from Inv_ProductLedger where ProductId = " + hdnNewProductId.Value + " and Finance_Year_Id = " + Session["FinanceYearId"].ToString() + " and Location_Id = " + Session["LocId"].ToString() + " and Inv_ProductLedger.ModifiedDate < '" + Convert.ToDateTime(txtFromDate.Text).ToString() + "' and Inv_ProductLedger.IsActive = 'True'").Rows[0][0].ToString();
                //}
                //else
                //{
                txtOpeningBalance.Text = SetDecimal((float.Parse(Dt.Rows[0]["BalQty"].ToString()) - float.Parse(Dt.Rows[0]["QuantityIn"].ToString()) + float.Parse(Dt.Rows[0]["QuantityOut"].ToString())).ToString(), Session["CurrencyId"].ToString());
                //}
                txtClosingBalance.Text = SetDecimal((float.Parse(Dt.Rows[Dt.Rows.Count - 1]["BalQty"].ToString())).ToString(), Session["CurrencyId"].ToString());
                tblOpeningBalance.InnerText = txtOpeningBalance.Text.ToString();


            }
            else
            {
                txtOpeningBalance.Text = "0";
                txtClosingBalance.Text = "0";
            }


        }
        //AllPageCode();

        //for get grid footer total
        float Inqty = 0;
        float Outqty = 0;
        string strCurrencyID = "0";

        foreach (GridViewRow gvrow in gvProductLadger.Rows)
        {
            Label lblQtyIn = (Label)gvrow.FindControl("lblQuantityIn");
            Label lblQtyout = (Label)gvrow.FindControl("lblQuantityOut");
            Label lblgvCurrency = (Label)gvrow.FindControl("lblgvCurrency");

            Inqty += float.Parse(lblQtyIn.Text);
            Outqty += float.Parse(lblQtyout.Text);
            strCurrencyID = lblgvCurrency.Text;
        }

        try
        {
            ((Label)gvProductLadger.FooterRow.FindControl("txttotQuantityIn")).Text = SetDecimal(Inqty.ToString(), strCurrencyID);
            ((Label)gvProductLadger.FooterRow.FindControl("txttotQuantityOut")).Text = SetDecimal(Outqty.ToString(), strCurrencyID);
            if (txtClosingBalance.Text == "" || txtClosingBalance.Text == "0")
            {
                ((Label)gvProductLadger.FooterRow.FindControl("txttotBalance")).Text = SetDecimal((Inqty - Outqty).ToString(), strCurrencyID);
            }
            else
            {
                ((Label)gvProductLadger.FooterRow.FindControl("txttotBalance")).Text = SetDecimal(txtClosingBalance.Text, strCurrencyID);
            }

        }
        catch
        {

        }

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

            dtProduct = objProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.Trim());


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

            //dtProduct = objProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductcode.Text.Trim());

            dtProduct = objDa.return_DataTable("select ProductId,EProductName,ProductCode from inv_productmaster where productcode='" + txtProductcode.Text.Trim() + "'");

            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                ////updated by jitendra upadhyay
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();



            }
            else
            {
                // DisplayMessage("Product Not Found !");
                txtProductName.Text = "";
                //txtProductcode.Text = "";
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



    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        if (Session["dtFilter_Pro_Ledg"] != null)
        {
            DataTable dt = Session["dtFilter_Pro_Ledg"] as DataTable;
            if (dt.Rows.Count > 0)
            {
                dt.Columns["ModifiedDate"].SetOrdinal(0);
                dt.Columns["ProductCode"].SetOrdinal(1);
                dt.Columns["ContactName"].SetOrdinal(2);
                dt.Columns["UnitName"].SetOrdinal(3);
                dt.Columns["UnitPrice"].SetOrdinal(4);
                dt.Columns["QuantityIn"].SetOrdinal(5);
                dt.Columns["QuantityOut"].SetOrdinal(6);
                dt.Columns["BalQty"].SetOrdinal(7);
                dt.Columns["Ref_Id"].SetOrdinal(8);
                dt.Columns["Ref_Type"].SetOrdinal(9);
                dt.Columns["ModifiedUser"].SetOrdinal(10);
                dt.Columns["LocationName"].SetOrdinal(11);


                dt.Columns["BalQty"].ColumnName = "BalanceQty";
                dt.Columns["Ref_Id"].ColumnName = "ReferenceId";
                dt.Columns["Ref_Type"].ColumnName = "ReferenceType";
                dt.Columns["ModifiedDate"].ColumnName = "Date";
                dt.Columns["ModifiedUser"].ColumnName = "UserName";


                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);
                dt.Columns.RemoveAt(12);

                ExportTableData(dt, "productLedger");
            }
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
