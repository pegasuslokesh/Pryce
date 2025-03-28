using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PegasusDataAccess;
using System.Data.SqlClient;
public partial class Purchase_PurchaseGoodsRec : BasePage
{
    #region Defined Class object
    PurchaseInvoice ObjPurchaseInvoice = null;
    SystemParameter ObjSysParam = null;
    PurchaseInvoiceDetail ObjPurchaseInvoiceDetail = null;
    Inv_UnitMaster objUnit = null;
    Inv_ProductMaster ObjProductMaster = null;
    Ems_ContactMaster objContact = null;
    Set_Suppliers objSupplier = null;
    Common cmn = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductLadger = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseInquiryHeader ObjPIHeader = null;
    Inv_ParameterMaster objInvParam = null;
    Set_DocNumber objDocNo = null;
    LocationMaster objLocation = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass objDa = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string UserId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        ObjPurchaseInvoice = new PurchaseInvoice(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjPurchaseInvoiceDetail = new PurchaseInvoiceDetail(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        UserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        if (Common.DtPhysical != null)
        {
            DataTable dt = new DataTable();
            dt = Common.DtPhysical;
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "CompanyId='" + StrCompId + "' and BrandId='" + Session["BrandId"].ToString() + "' and LocationId='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["TransType"].ToString() == "1")
                    {
                        DisplayMessage("Don't transaction ,Stock Work is going On... ");
                        Response.Redirect("../MasterSetup/Home.aspx");
                    }
                }
            }
        }
        btnSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSave, "").ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Purchase/PurchaseGoodsRec.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocList);
            GetDocumentNumber();
            //Update By Akshay on 17/12/2013 Count Serial No.
            txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
            //End 
            FillGrid();
            txtSerialNo.Text = "";
            Session["dtSerial"] = null;
            Session["dtFinal"] = null;
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            LoadStores();
            FillShipUnitddl();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            CalendarExtender3.Format = Session["DateFormat"].ToString();
            Session["dtPackingList"] = null;
            txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            FillddlLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            if (Request.QueryString["Id"] != null)
            {
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                StrLocationId = Request.QueryString["LocId"].ToString();
                btnEdit_Command(imgeditbutton, new CommandEventArgs("commandName", Request.QueryString["Id"].ToString() + "," + StrLocationId));
                btnCancel.Visible = false;
                // ((Panel)Master.FindControl("pnlaccordian")).Visible = false;
            }
            else
            {
                btnCancel.Visible = true;
                //  ((Panel)Master.FindControl("pnlaccordian")).Visible = true;
            }
            CalendarExtendertxtQValue.Format = Session["DateFormat"].ToString();
        }

        setFooterVisibility();
    }
    public void setFooterVisibility()
    {
        if (gvShippingInformation.Rows.Count > 0)
        {
            Footer1.Visible = true;
            Footer2.Visible = true;
            Footer3.Visible = true;
        }
        else
        {
            Footer1.Visible = false;
            Footer2.Visible = false;
            Footer3.Visible = false;
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        PnlProductSearching.Visible = false;
        if (Lbl_Tab_New.Text == Resources.Attendance.New)
        {
            ViewState["hdnInvoiceId"] = null;
            Session["dtSerial"] = null;
            Session["dtFinal"] = null;
            gvInvoice.DataSource = null;
            gvInvoice.DataBind();
            PnlProductSearching.Visible = true;
            try
            {
                GvProduct.Columns[0].Visible = true;
                GvProduct.Columns[1].Visible = true;
            }
            catch
            {
            }
        }
    }
    #region System Defined Function
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldName.SelectedItem.Value == "InvoiceDate") || (ddlFieldName.SelectedItem.Value == "SupInvoiceDate"))
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
                txtValue.Text = "";
                return;
            }
        }

        FillGrid();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        FillGrid();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandArgument.ToString().Split(',')[1] != Session["LocId"].ToString())
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "redirectToHome('Due to change in Location, we cannot process your request, change Location to continue, do you want to continue ?');", true);
            return;
        }

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;
        if (objSenderID.ToString() != "lnkViewDetail")
        {
            if (e.CommandName.ToString().Trim() == "True")
            {
                DisplayMessage("Record posted ,you can not edit !");
                return;
            }
        }
        using (DataTable dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), e.CommandArgument.ToString().Split(',')[0]))
        {
            if (dt.Rows.Count != 0)
            {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvInvoice, dt, "", "");

                if (dt.Rows[0]["Field1"].ToString().Trim() == "PO")
                {
                    PnlProductSearching.Visible = true;
                    try
                    {
                        GvProduct.Columns[0].Visible = true;
                    }
                    catch
                    {
                    }
                }
                else
                {
                    PnlProductSearching.Visible = false;
                    try
                    {
                        GvProduct.Columns[0].Visible = false;
                    }
                    catch
                    {
                    }
                }

                DataTable dtContact = objContact.GetContactTrueById(dt.Rows[0]["SupplierId"].ToString());
                txtSupplierName.Text = dtContact.Rows[0]["Name"].ToString() + "/" + dtContact.Rows[0]["TRans_Id"].ToString();
                txtSupplierName_TextChanged(null, null);
                try
                {
                    ddlLocation.SelectedValue = dt.Rows[0]["Field3"].ToString();
                }
                catch
                {
                    ddlLocation.SelectedValue = Session["LocId"].ToString();
                }
                ViewState["hdnInvoiceId"] = e.CommandArgument.ToString().Split(',')[0];
                FillGridDetail(e.CommandArgument.ToString().Split(',')[0]);
                foreach (GridViewRow gvrow in GvProduct.Rows)
                {
                    if (((Label)gvrow.FindControl("lblPoID")).Text == "0")
                    {
                        GvProduct.Columns[1].Visible = false;
                        PanelTab.Enabled = true;
                        break;
                    }
                    else
                    {
                        GvProduct.Columns[1].Visible = true;
                        PanelTab.Enabled = false;
                        getshipinformation_BY_OrderId(((Label)gvrow.FindControl("lblPoID")).Text);
                    }
                }


                if (objSenderID.ToString() == "lnkViewDetail")
                {
                    Hdn_Edit.Value = "View";
                    btnSave.Enabled = false;
                    btnReset.Visible = false;
                    Lbl_Tab_New.Text = Resources.Attendance.View;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }
                else
                {
                    Hdn_Edit.Value = "Edit";
                    btnSave.Enabled = true;
                    btnReset.Visible = true;
                    Lbl_Tab_New.Text = Resources.Attendance.Edit;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                }
            }
        }
        DataTable dtTemp = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(StrCompId, StrBrandId, StrLocationId, "PG", e.CommandArgument.ToString().Split(',')[0]);

        if (dtTemp.Rows.Count > 0)
        {

            dtTemp = dtTemp.DefaultView.ToTable(false, "ProductId", "SerialNo", "Barcode", "BatchNo", "LotNo", "ExpiryDate", "Field1", "TransType", "TransTypeId", "ManufacturerDate", "Quantity", "Trans_Id", "Width", "Length", "Pallet_ID");
            dtTemp.Columns["Field1"].ColumnName = "POID";
            dtTemp.Columns["Barcode"].ColumnName = "BarcodeNo";
            Session["dtFinal"] = dtTemp;
            dtTemp = null;
        }

        using (DataTable dtshipInformation = objDa.return_DataTable("select * from Inv_InvoiceShippingHeader where ref_type='PINV' and ref_id=" + e.CommandArgument.ToString().Split(',')[0] + ""))
        {
            if (dtshipInformation.Rows.Count > 0)
            {
                try
                {
                    txtShippingLine.Text = objContact.GetContactTrueById(dtshipInformation.Rows[0]["Shipping_Line"].ToString()).Rows[0]["Name"].ToString().Trim() + "/" + dtshipInformation.Rows[0]["Shipping_Line"].ToString().Trim();
                }
                catch
                {
                }
                ddlShipBy.SelectedValue = dtshipInformation.Rows[0]["Ship_By"].ToString().Trim();
                ddlShipmentType.SelectedValue = dtshipInformation.Rows[0]["Shipment_Type"].ToString().Trim();
                ddlFreightStatus.SelectedValue = dtshipInformation.Rows[0]["Freight_Status"].ToString().Trim();
                try
                {
                    ddlShipUnit.SelectedValue = dtshipInformation.Rows[0]["Ship_Unit"].ToString().Trim();
                }
                catch
                {
                    ddlShipUnit.SelectedIndex = 0;
                }
                txtAirwaybillno.Text = dtshipInformation.Rows[0]["Airway_Bill_No"].ToString().Trim();
                txtvolumetricweight.Text = dtshipInformation.Rows[0]["Volumetric_weight"].ToString().Trim();
                txtTotalWeight.Text = dtshipInformation.Rows[0]["Actual_Weight"].ToString().Trim();
                txtUnitRate.Text = GetAmountDecimal(dtshipInformation.Rows[0]["UnitRate"].ToString().Trim());
                txtReceivingDate.Text = Convert.ToDateTime(dtshipInformation.Rows[0]["Receiving_date"].ToString().Trim()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtShippingDate.Text = Convert.ToDateTime(dtshipInformation.Rows[0]["Shipping_Date"].ToString().Trim()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                txtdivideby.Text = dtshipInformation.Rows[0]["Divide_By"].ToString().Trim();
            }
        }
        using (DataTable dtPackingList = objDa.return_DataTable("select Trans_Id,Length,Height,Width,Cartons,Total from Inv_InvoiceShippingDetail  where ref_type='PINV' and ref_id=" + e.CommandArgument.ToString().Split(',')[0] + ""))
        {
            Session["dtPackingList"] = dtPackingList;
            objPageCmn.FillData((object)gvShippingInformation, dtPackingList, "", "");
        }
        shippingCalculation();
        setFooterVisibility();
    }

    protected void GvPurchaseInvocie_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvPurchaseInvoice.Attributes["CurrentSortField"] != null &&
            GvPurchaseInvoice.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvPurchaseInvoice.Attributes["CurrentSortField"])
            {
                if (GvPurchaseInvoice.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvPurchaseInvoice.Attributes["CurrentSortField"] = sortField;
        GvPurchaseInvoice.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid(Int32.Parse(hdnGvPurchaseInvoiceCurrentPageIndex.Value));
    }
    public string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, StrCompId, true, "12", "48", "0",
 HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        ViewState["DocNo"] = s;
        return s;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        string sql = string.Empty;
        string StrInvoiceid = string.Empty;
        string InvoiceNo = string.Empty;
        string ShippingLine = string.Empty;
        string shipUnit = string.Empty;
        if (ddlShipUnit.SelectedIndex == 0)
        {
            shipUnit = "0";
        }
        else
        {
            shipUnit = ddlShipUnit.SelectedValue;
        }
        if (txtTotalWeight.Text == "")
        {
            txtTotalWeight.Text = "0";
        }
        if (txtUnitRate.Text == "")
        {
            txtUnitRate.Text = "0";
        }
        if (txtShippingLine.Text != "")
        {
            ShippingLine = txtShippingLine.Text.Split('/')[1].ToString();
        }
        if (txtShippingDate.Text.Trim() != "")
        {
            try
            {
                Convert.ToDateTime(txtShippingDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Shipping Date");
                txtShippingDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Shipping Date");
            txtShippingDate.Focus();
            return;
        }
        if (txtReceivingDate.Text.Trim() != "")
        {
            try
            {
                Convert.ToDateTime(txtReceivingDate.Text);
            }
            catch
            {
                DisplayMessage("Enter Valid Receiving Date");
                txtReceivingDate.Focus();
                return;
            }
        }
        else
        {
            DisplayMessage("Enter Receiving Date");
            txtReceivingDate.Focus();
            return;
        }
        //here we check validation
        if (ViewState["hdnInvoiceId"] == null)
        {
            if (txtSupplierName.Text == "")
            {
                DisplayMessage("Enter Supplier Name");
                txtSupplierName.Focus();
                return;
            }
            if (GvProduct.Rows.Count == 0)
            {
                DisplayMessage("Product Not Found");
                return;
            }
            foreach (GridViewRow Rows in GvProduct.Rows)
            {
                //for check invoice sty validation
                if (((TextBox)Rows.FindControl("QtyReceived")).Text == "")
                {
                    DisplayMessage("Invoice quantity should be greater then 0");
                    return;
                }
                else
                {
                    if (float.Parse(((TextBox)Rows.FindControl("QtyReceived")).Text) == 0)
                    {
                        DisplayMessage("Invoice quantity should be greater then 0");
                        return;
                    }
                }
                if (((TextBox)Rows.FindControl("txtRecQty")).Text == "")
                {
                    ((TextBox)Rows.FindControl("txtRecQty")).Text = "0";
                    DisplayMessage("Received quantity should be greater then 0");
                    return;
                }
                else
                {
                    if (float.Parse(((TextBox)Rows.FindControl("txtRecQty")).Text) == 0)
                    {
                        DisplayMessage("Received quantity should be greater then 0");
                        return;
                    }
                }
            }
        }

        int trans_type = -1;
        try
        {
            foreach (GridViewRow Rows in GvProduct.Rows)
            {
                Label lblPoId = (Label)Rows.FindControl("lblPoID");
                string strTrnsType = objDa.get_SingleValue("select trans_type from Inv_PurchaseOrderHeader where TransID=" + lblPoId.Text);
                int.TryParse(strTrnsType, out trans_type);
                //trans_type = trans_type == -1 ? 0 : trans_type;
            }
        }
        catch (Exception ex)
        {

        }

        int b = 0;
        //here we create internal invoice when we goods recive by order
        //code start
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (ViewState["hdnInvoiceId"] == null)
            {
                string strExchangeRate = "1";
                double f = 0;
                foreach (GridViewRow Rows in GvProduct.Rows)
                {
                    try
                    {
                        strExchangeRate = ((HiddenField)Rows.FindControl("hdnExchangeRate")).Value;
                        f += float.Parse(((HiddenField)Rows.FindControl("hdnUnitCost")).Value) * float.Parse(((TextBox)Rows.FindControl("QtyReceived")).Text);
                    }
                    catch
                    {
                        f += 0;
                    }
                }

                //b = ObjPurchaseInvoice.InsertPurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "", DateTime.Now.ToString(), "0", DateTime.Now.ToString(), "0", "Cash", txtSupplierName.Text.Split('/')[1].ToString(), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), "1", "0", "0", "0", "0", "0", "0", f.ToString(), "0", "0", f.ToString(), false.ToString(), "", "PO", "0", ddlLocation.SelectedValue, "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(),"-1", ref trns);
                b = ObjPurchaseInvoice.InsertPurchaseInvoiceHeader(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "", DateTime.Now.ToString(), "0", DateTime.Now.ToString(), "0", "Cash", txtSupplierName.Text.Split('/')[1].ToString(), Hdn_Invoice_Currency.Value.ToString(), strExchangeRate, "0", "0", "0", "0", "0", "0", f.ToString(), "0", "0", f.ToString(), false.ToString(), "", "PO", "0", ddlLocation.SelectedValue, "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), trans_type.ToString(), ref trns);
                //insert record in shipping header
                sql = "INSERT INTO [Inv_InvoiceShippingHeader]([Ref_Type],[Ref_Id] ,[Shipping_Line],[Ship_By] ,[Airway_Bill_No],[Ship_Unit],[Actual_Weight],[Volumetric_weight],[Shipping_Date],[Receiving_date],[Shipment_Type],[Freight_Status],[UnitRate],[Divide_By])VALUES('PINV'," + b.ToString() + ",'" + ShippingLine.Trim() + "','" + ddlShipBy.SelectedValue.Trim() + "','" + txtAirwaybillno.Text.Trim() + "','" + shipUnit.Trim() + "','" + txtTotalWeight.Text.Trim() + "','" + txtvolumetricweight.Text.Trim() + "','" + ObjSysParam.getDateForInput(txtShippingDate.Text).ToString().Trim() + "','" + ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString().Trim() + "','" + ddlShipmentType.SelectedValue.Trim() + "','" + ddlFreightStatus.SelectedValue.Trim() + "','" + txtUnitRate.Text + "','" + txtdivideby.Text.Trim() + "')";
                objDa.execute_Command(sql, ref trns);
                if (Session["dtPackingList"] != null)
                {
                    foreach (DataRow dr in ((DataTable)Session["dtPackingList"]).Rows)
                    {
                        sql = "insert into  dbo.Inv_InvoiceShippingDetail values('PINV'," + b.ToString() + ",'" + dr["Length"].ToString() + "','" + dr["Height"].ToString() + "','" + dr["Width"].ToString() + "','" + dr["Cartons"].ToString() + "','" + dr["Total"].ToString() + "')";
                        objDa.execute_Command(sql, ref trns);
                    }
                }
                if (ViewState["DocNo"].ToString() != "")
                {


                    int invoicecount = ObjPurchaseInvoice.GetInvoiceCountByLocationId(Session["LocId"].ToString(), ref trns);

                    if (invoicecount == 0)
                    {
                        ObjPurchaseInvoice.Updatecode(b.ToString(), ViewState["DocNo"].ToString() + "1", ref trns);
                    }
                    else
                    {
                        if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + ViewState["DocNo"].ToString() + invoicecount.ToString() + "'").Rows.Count > 0)
                        {

                            bool bCodeFlag = true;
                            while (bCodeFlag)
                            {
                                invoicecount += 1;

                                if (objDa.return_DataTable("select invoiceno from Inv_PurchaseInvoiceHeader where location_id=" + Session["LocId"].ToString() + " and invoiceno='" + ViewState["DocNo"].ToString() + invoicecount.ToString() + "'").Rows.Count == 0)
                                {
                                    bCodeFlag = false;
                                }

                            }

                        }
                        ObjPurchaseInvoice.Updatecode(b.ToString(), ViewState["DocNo"].ToString() + invoicecount.ToString(), ref trns);

                    }
                }
                else
                {
                    ObjPurchaseInvoice.Updatecode(b.ToString(), b.ToString(), ref trns);
                    InvoiceNo = b.ToString();
                }
                int SerialNo = 0;
                foreach (GridViewRow Rows in GvProduct.Rows)
                {
                    Label lblPoId = (Label)Rows.FindControl("lblPoID");
                    string ProductId = ((HiddenField)Rows.FindControl("hdnProductId")).Value.ToString();
                    string UnitId = ((HiddenField)Rows.FindControl("hdnUnitId")).Value.ToString();
                    string Discount_Per = ((TextBox)Rows.FindControl("lblDiscount")).Text.ToString();
                    string Discount_Value = ((TextBox)Rows.FindControl("lblDiscountValue")).Text.ToString();
                    string Tax_Per = ((TextBox)Rows.FindControl("lblTax")).Text.ToString();
                    string Tax_Value = ((TextBox)Rows.FindControl("lblTaxValue")).Text.ToString();
                    string strExpiryDate = ((TextBox)Rows.FindControl("txtgvExpiryDate")).Text.ToString();
                    string strBatchNo = ((TextBox)Rows.FindControl("txtgvBatchNo")).Text.ToString();

                    if (((TextBox)Rows.FindControl("txtRecQty")).Text == "")
                    {
                        ((TextBox)Rows.FindControl("txtRecQty")).Text = "0";
                    }
                    SerialNo++;
                    ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), b.ToString(), SerialNo.ToString(), ProductId, lblPoId.Text.Trim(), UnitId, ((HiddenField)Rows.FindControl("hdnUnitCost")).Value, ((Label)Rows.FindControl("lblorderqty")).Text, ((Label)Rows.FindControl("lblfreeqty")).Text, ((TextBox)Rows.FindControl("txtRecQty")).Text, ((TextBox)Rows.FindControl("txtRecQty")).Text, Discount_Per, Discount_Value, Tax_Per, Tax_Value, strExpiryDate, strBatchNo, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                }
            }

            //code end
            string InvoiceId = string.Empty;
            string TransId = string.Empty;
            string Qty = string.Empty;
            if (ViewState["hdnInvoiceId"] == null)
            {
                InvoiceId = b.ToString();
            }
            else
            {
                InvoiceId = ViewState["hdnInvoiceId"].ToString();
                //delete and reinsert shipping information by ref_rtype and ref_id 
                //for delete in shipping header and detail
                //code start
                sql = "delete from dbo.Inv_InvoiceShippingHeader where ref_type='PINV' and ref_id=" + InvoiceId + "";
                objDa.execute_Command(sql, ref trns);
                sql = "delete from dbo.Inv_InvoiceShippingDetail where ref_type='PINV' and ref_id=" + InvoiceId + "";
                objDa.execute_Command(sql, ref trns);
                //code end
                //for insert inshippiong herader and etail 
                //code start
                sql = "INSERT INTO [Inv_InvoiceShippingHeader]([Ref_Type],[Ref_Id] ,[Shipping_Line],[Ship_By] ,[Airway_Bill_No],[Ship_Unit],[Actual_Weight],[Volumetric_weight],[Shipping_Date],[Receiving_date],[Shipment_Type],[Freight_Status],[UnitRate],[Divide_By])VALUES('PINV'," + InvoiceId + ",'" + ShippingLine.Trim() + "','" + ddlShipBy.SelectedValue.Trim() + "','" + txtAirwaybillno.Text.Trim() + "','" + shipUnit.Trim() + "','" + txtTotalWeight.Text.Trim() + "','" + txtvolumetricweight.Text.Trim() + "','" + ObjSysParam.getDateForInput(txtShippingDate.Text).ToString().Trim() + "','" + ObjSysParam.getDateForInput(txtReceivingDate.Text).ToString().Trim() + "','" + ddlShipmentType.SelectedValue.Trim() + "','" + ddlFreightStatus.SelectedValue.Trim() + "','" + txtUnitRate.Text + "','" + txtdivideby.Text.Trim() + "')";
                objDa.execute_Command(sql, ref trns);
                if (Session["dtPackingList"] != null)
                {
                    foreach (DataRow dr in ((DataTable)Session["dtPackingList"]).Rows)
                    {
                        sql = "insert into  dbo.Inv_InvoiceShippingDetail values('PINV'," + InvoiceId + ",'" + dr["Length"].ToString() + "','" + dr["Height"].ToString() + "','" + dr["Width"].ToString() + "','" + dr["Cartons"].ToString() + "','" + dr["Total"].ToString() + "')";
                        objDa.execute_Command(sql, ref trns);
                    }
                }
                //code end 
            }

            double Serial_No = 0;
            DataTable dtInvoiceDetail = new DataTable();
            if (Session["dtPIDetail"] != null)
            {
                dtInvoiceDetail = (DataTable)Session["dtPIDetail"];

                Serial_No = Convert.ToDouble(new DataView(dtInvoiceDetail, "", "SerialNo Desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["SerialNo"].ToString());
            }

            foreach (GridViewRow Rows in GvProduct.Rows)
            {
                TransId = ((Label)Rows.FindControl("lblTransID")).Text.ToString();
                Label lblPoId = (Label)Rows.FindControl("lblPoID");
                string ProductId = ((HiddenField)Rows.FindControl("hdnProductId")).Value.ToString();
                string UnitId = ((HiddenField)Rows.FindControl("hdnUnitId")).Value.ToString();
                string Discount_Per = ((TextBox)Rows.FindControl("lblDiscount")).Text.ToString();
                string Discount_Value = ((TextBox)Rows.FindControl("lblDiscountValue")).Text.ToString();
                string Tax_Per = ((TextBox)Rows.FindControl("lblTax")).Text.ToString();
                string Tax_Value = ((TextBox)Rows.FindControl("lblTaxValue")).Text.ToString();

                string strExpiryDate = ((TextBox)Rows.FindControl("txtgvExpiryDate")).Text.ToString();
                string strBatchNo = ((TextBox)Rows.FindControl("txtgvBatchNo")).Text.ToString();

                Qty = ((TextBox)Rows.FindControl("txtRecQty")).Text.Trim().ToString(); //+ float.Parse(((Label)Rows.FindControl("QtyField1")).Text.ToString())).ToString();
                if (Qty != "0" && Qty != "")
                {
                    if (ViewState["hdnInvoiceId"] != null)
                    {

                        if (new DataView(dtInvoiceDetail, "ProductId=" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            Serial_No++;
                            ObjPurchaseInvoiceDetail.InsertPurchaseInvoiceDetail(Session["Compid"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), InvoiceId, Serial_No.ToString(), ProductId, lblPoId.Text.Trim(), UnitId, ((HiddenField)Rows.FindControl("hdnUnitCost")).Value, ((Label)Rows.FindControl("lblorderqty")).Text, ((Label)Rows.FindControl("lblfreeqty")).Text, ((TextBox)Rows.FindControl("txtRecQty")).Text, ((TextBox)Rows.FindControl("txtRecQty")).Text, Discount_Per, Discount_Value, Tax_Per, Tax_Value, strExpiryDate, strBatchNo, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            ObjPurchaseInvoiceDetail.UpdatePurchaseInvoiceDetailByGoods(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), TransId.ToString(), Qty, ref trns);
                            int i = objDa.execute_Command("Update Inv_PurchaseInvoiceDetail  Set Field1 ='" + strExpiryDate + "', Field2='" + strBatchNo + "'  Where Company_Id='" + StrCompId.ToString() + "' and Brand_Id='" + StrBrandId.ToString() + "' and Location_ID='" + StrLocationId.ToString() + "' and TransID='" + TransId + "'", ref trns);
                        }
                    }
                    if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                    {
                        if (Session["dtFinal"] != null)
                        {
                            DataTable dt = (DataTable)Session["dtFinal"];
                            ObjStockBatchMaster.DeleteStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), "PG", InvoiceId, ProductId, ref trns);
                            dt = new DataView(dt, "ProductId='" + ProductId + "' and POId='" + lblPoId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dt.Rows)
                            {
                                ObjStockBatchMaster.InsertStockBatchMaster(StrCompId.ToString(), StrBrandId.ToString(), ddlLocation.SelectedValue, "PG", InvoiceId, ProductId, UnitId, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), dr["BarcodeNo"].ToString(), dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), dr["POId"].ToString(), "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), UserId.ToString(), DateTime.Now.ToString(), UserId.ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            dt = null;
                        }
                    }
                }
            }
            //updated By jitendra upadhyay on 08-Jan-2014 to update the status in Purchase Inquiry


            DataTable DtInvoiceDetail = objDa.return_DataTable("select Inv_PurchaseInquiryHeader.trans_id from Inv_PurchaseInvoiceDetail inner join Inv_PurchaseOrderHeader on Inv_PurchaseOrderHeader.TransID = Inv_PurchaseInvoiceDetail.poId inner join Inv_PurchaseQuoteHeader on Inv_PurchaseOrderHeader.ReferenceID = Inv_PurchaseQuoteHeader.Trans_Id and Inv_PurchaseOrderHeader.ReferenceVoucherType='PQ' inner join Inv_PurchaseInquiryHeader on Inv_PurchaseQuoteHeader.PI_No = Inv_PurchaseInquiryHeader.Trans_id  where Inv_PurchaseInvoiceDetail.poId <>0 and Inv_PurchaseInvoiceDetail.InvoiceNo=" + InvoiceId + "", ref trns);

            if (DtInvoiceDetail.Rows.Count > 0)
            {
                ObjPIHeader.UpdatePIHeaderStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), DtInvoiceDetail.Rows[0]["trans_id"].ToString(), "Goods Receive", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            if (InvoiceNo != "")
            {
                DisplayMessage("Invoice Created and invoice no. is " + InvoiceNo);
                btnNew_Click(null, null);
                Reset();
            }
            else
            {
                DisplayMessage("Record Saved", "green");
                btnCancel_Click(null, null);
            }
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
        PnlProductSearching.Visible = false;
        btnReset.Visible = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        btnNew_Click(null, null);
    }
    public void Reset()
    {
        Hdn_Edit.Value = "";
        FillGrid();
        ViewState["hdnInvoiceId"] = null;
        Session["dtSerial"] = null;
        Session["dtFinal"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        gvInvoice.DataSource = null;
        gvInvoice.DataBind();
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        ResetInvoicePanel();
        txtdivideby.Text = "0";
        Session["dtPackingList"] = null;
        objPageCmn.FillData((object)gvShippingInformation, null, "", "");
        txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtShippingLine.Text = "";
        ddlShipBy.SelectedIndex = 0;
        ddlShipmentType.SelectedIndex = 0;
        ddlFreightStatus.SelectedIndex = 0;
        ddlShipUnit.SelectedIndex = 0;
        txtAirwaybillno.Text = "";
        txtvolumetricweight.Text = "";
        txtTotalWeight.Text = "";
        txtUnitRate.Text = "";
        txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txttotalvolumetricweight.Text = "0";
        PanelTab.Enabled = false;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        txtheight.Text = "";
        txtLength.Text = "";
        txtwidth.Text = "";
        txtcartons.Text = "";
        shippingCalculation();
        setFooterVisibility();
        btnSave.Enabled = true;
        btnReset.Visible = true;
    }
    public void ResetInvoicePanel()
    {
        txtSupplierName.Text = "";
        Session["DtOrderproduct"] = null;
        Session["dtProductSearch"] = null;
        Session["dtPo"] = null;
        gvSerachGrid.DataSource = null;
        gvSerachGrid.DataBind();
        txtProductSerachValue.Text = "";
    }
    #endregion
    #region User Defined Function
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    private void FillGrid(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName.SelectedValue + " Like '" + txtValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id in (" + ddlLocList.SelectedValue + ") and isActive='true'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            strWhereClause += " and Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            strWhereClause += " and Post='False'";
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = ObjPurchaseInvoice.getInvoiceList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), GvPurchaseInvoice.Attributes["CurrentSortField"], GvPurchaseInvoice.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvPurchaseInvoice, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvPurchaseInvoice.DataSource = null;
                GvPurchaseInvoice.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvPurchaseInvoiceCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void GvProduct_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((HiddenField)e.Row.FindControl("hdnProductId")).Value;
            using (DataTable dtProduct = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()))
            {

                if (dtProduct.Rows[0]["itemtype"].ToString().Trim() == "S")
                {
                    ((TextBox)e.Row.FindControl("txtRecQty")).Enabled = true;
                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;

                    ((TextBox)e.Row.FindControl("txtgvExpiryDate")).Visible = false;
                    ((TextBox)e.Row.FindControl("txtgvBatchNo")).Visible = false;

                    if (dtProduct.Rows[0]["MaintainStock"].ToString().Trim() == "SNO")
                    {
                        if (Convert.ToBoolean(objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Is Serial on Purchase Goods").Rows[0]["ParameterValue"].ToString()))
                        {
                            ((TextBox)e.Row.FindControl("txtRecQty")).Enabled = false;
                            ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                        }
                        else
                        {
                            ((TextBox)e.Row.FindControl("txtRecQty")).Enabled = true;
                            ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                        }
                    }
                    if (dtProduct.Rows[0]["MaintainStock"].ToString().Trim() == "Expiry")
                    {
                        ((TextBox)e.Row.FindControl("txtgvExpiryDate")).Visible = true;
                        ((TextBox)e.Row.FindControl("txtgvBatchNo")).Visible = true;
                    }
                }
                else
                {
                    ((TextBox)e.Row.FindControl("txtRecQty")).Enabled = true;
                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
                    ((TextBox)e.Row.FindControl("txtgvExpiryDate")).Visible = false;
                    ((TextBox)e.Row.FindControl("txtgvBatchNo")).Visible = false;
                }
            }
            //if (ViewState["hdnInvoiceId"] == null)
            //{
                ((TextBox)e.Row.FindControl("QtyReceived")).Enabled = true;
            if (((TextBox)e.Row.FindControl("QtyReceived")).Text == "")
            {
                ((TextBox)e.Row.FindControl("QtyReceived")).Text = "0";
            }
            if (float.Parse(((TextBox)e.Row.FindControl("QtyReceived")).Text) == 0)
            {
                ((TextBox)e.Row.FindControl("QtyReceived")).Text = ((Label)e.Row.FindControl("lblorderqty")).Text;
            }
            //}
            //else
            //{
                ((TextBox)e.Row.FindControl("QtyReceived")).Enabled = false;
            //}
        }
    }

    public void FillGridDetail(string InvoiceId)
    {
        DataTable dtTemp = new DataTable();
        DataTable dt = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), InvoiceId);

        if (dt.Rows.Count != 0)
        {
            dt.Columns["TaxP"].ColumnName = "TaxPercentage";
            dt.Columns["TaxV"].ColumnName = "TaxValue";
            dt.Columns["DiscountP"].ColumnName = "DisPercentage";
            dt.Columns["DiscountV"].ColumnName = "DiscountValue";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProduct, dt, "", "");
            Session["dtPo"] = dt;
            dtTemp = dt.Copy();
            Session["dtPIDetail"] = dtTemp;
        }


    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #endregion
    #region AllPageCode

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();

    }


    #endregion
    #region View
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    #endregion
    //this region is created by jitendra upadhyay on 03-Nov-2014
    //this region for print goods receieve
    #region PrintReport
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        PrintReport(e.CommandArgument.ToString());
    }
    void PrintReport(string InvoiceID)
    {
        string strCmd = string.Format("window.open('../Purchase/GoodsReceive_Print.aspx?Id=" + InvoiceID.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if ((ddlFieldName.SelectedItem.Value == "InvoiceDate") || (ddlFieldName.SelectedItem.Value == "SupInvoiceDate"))
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
    #endregion
    #region Serial Number
    public DataTable CreateStock_Datatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ProductId");
        dt.Columns.Add("SerialNo");
        dt.Columns.Add("BarcodeNo");
        dt.Columns.Add("BatchNo");
        dt.Columns.Add("LotNo");
        dt.Columns.Add("ExpiryDate");
        dt.Columns.Add("POID");
        dt.Columns.Add("TransType");
        dt.Columns.Add("TransTypeId");
        dt.Columns.Add("ManufacturerDate");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("Width");
        dt.Columns.Add("Length");
        dt.Columns.Add("Pallet_ID");
        return dt;
    }
    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        string SerialMissMatch = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        if (txtSerialNo.Text.Trim() != "")
        {
            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;
            if (Session["dtSerial"] == null)
            {
                dt = new DataTable();
                //for create table structure
                dt = CreateStock_Datatable();
                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        if ((new DataView(dt, "SerialNo='" + txt[i].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) == 0)
                        {
                            string[] result = isSerialNumberValid_GoodsReceive(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                            if (result[0] == "VALID")
                            {
                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dr[0] = ViewState["PID"].ToString();
                                dr[1] = txt[i].ToString().Trim();
                                dr[2] = "0";
                                dr[3] = "0";
                                dr[4] = "0";
                                dr[5] = DateTime.Now.ToString();
                                dr[6] = ViewState["POId"].ToString();
                                dr[7] = "PG";
                                dr[8] = "0";
                                dr[9] = DateTime.Now.ToString();
                                dr[10] = "1";
                                counter++;
                            }
                            else if (result[0] == "DUPLICATE")
                            {
                                DuplicateserialNo += txt[i].ToString() + ",";
                            }
                            else if (result[0].ToString() == "SERIAL_MISSMATCH")
                            {
                                SerialMissMatch += txt[i].ToString().Trim() + ",";
                            }
                        }
                        else
                        {
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                    }
                }
            }
            else
            {
                dt = (DataTable)Session["dtSerial"];
                dtTemp = dt.Copy();
                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        if ((new DataView(dt, "SerialNo='" + txt[i].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) == 0)
                        {
                            string[] result = isSerialNumberValid_GoodsReceive(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                            if (result[0] == "VALID")
                            {
                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dr[0] = ViewState["PID"].ToString();
                                dr[1] = txt[i].ToString().Trim();
                                dr[2] = "0";
                                dr[3] = "0";
                                dr[4] = "0";
                                dr[5] = DateTime.Now.ToString();
                                dr[6] = ViewState["POId"].ToString();
                                dr[7] = "PG";
                                dr[8] = "0";
                                dr[9] = DateTime.Now.ToString();
                                dr[10] = "1";
                                counter++;
                            }
                            else if (result[0] == "DUPLICATE")
                            {
                                DuplicateserialNo += txt[i].ToString() + ",";
                            }
                            else if (result[0].ToString() == "SERIAL_MISSMATCH")
                            {
                                SerialMissMatch += txt[i].ToString().Trim() + ",";
                            }
                        }
                        else
                        {
                            DuplicateserialNo += txt[i].ToString() + ",";
                        }
                    }
                }
            }
        }
        else
        {
            if (Session["dtSerial"] != null)
            {
                dt = (DataTable)Session["dtSerial"];
            }
        }
        string Message = string.Empty;
        if (DuplicateserialNo != "" || SerialMissMatch != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is Already Exists in stock=" + DuplicateserialNo;
            }
            if (SerialMissMatch != "")
            {
                Message += " Serial number already exist with another Product=" + SerialMissMatch;
            }
            DisplayMessage(Message);
        }
        Session["dtSerial"] = dt;
        if (Session["dtFinal"] == null)
        {
            if (Session["dtSerial"] != null)
            {
                Session["dtFinal"] = (DataTable)Session["dtSerial"];
            }
        }
        else
        {
            dt = new DataTable();
            DataTable Dtfinal = (DataTable)Session["dtFinal"];
            if (ViewState["POId"].ToString() == "0")
            {
                Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DataTable DtTemp = Dtfinal.Copy();
                try
                {
                    DtTemp = new DataView(DtTemp, "ProductId='" + ViewState["PID"].ToString() + "' and POID='" + ViewState["POId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (DtTemp.Rows.Count > 0)
                {
                    string s = "POID Not In('" + ViewState["POId"].ToString() + "') or ProductId Not In('" + ViewState["PID"].ToString() + "')";
                    Dtfinal = new DataView(Dtfinal, s.ToString(), "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            if (Session["dtSerial"] != null)
            {
                dt = (DataTable)Session["dtSerial"];
            }
            Dtfinal.Merge(dt);
            Session["dtFinal"] = Dtfinal;
        }
        float QtyCount = 0;
        if (Session["dtSerial"] != null)
        {
            if (pnlSerialNumber.Visible == true)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerialNumber, (DataTable)Session["dtSerial"], "", "");
                txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
                QtyCount = gvSerialNumber.Rows.Count;
            }
            else
            {
                DataTable dtcountqty = (DataTable)Session["dtSerial"];
                foreach (DataRow dr in dtcountqty.Rows)
                {
                    QtyCount += float.Parse(dr["Quantity"].ToString());
                }
            }
        }
        foreach (GridViewRow gvRow in GvProduct.Rows)
        {
            TextBox txtRecQty = (TextBox)gvRow.FindControl("txtRecQty");
            if (txtRecQty.Text == "")
            {
                txtRecQty.Text = "0";
            }
            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                if (Session["dtSerial"] != null)
                {
                    txtRecQty.Text = QtyCount.ToString();
                }
                else
                {
                    txtRecQty.Text = "0";
                }
                break;
            }
        }
        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();
    }
    public static string[] isSerialNumberValid_GoodsReceive(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber)
    {
        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
        SystemParameter ObjSysParam = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());
        string[] Result = new string[5];
        int counter = 0;
        foreach (GridViewRow gvrow in gvSerialNumber.Rows)
        {
            if (((Label)gvrow.FindControl("lblsrno")).Text.Trim() == serialNumber)
            {
                counter = 1;
                break;
            }
        }
        if (counter == 0)
        {
            if (ObjStockBatchMaster.getSerialBalanceByProductId_GoodsReceive(HttpContext.Current.Session["CompId"].ToString(), ProductId, serialNumber) > 0)
            {
                Result[0] = "DUPLICATE";
            }
            else if (ObjStockBatchMaster.getSerialCountWithAnotherProductId(HttpContext.Current.Session["CompId"].ToString(), ProductId, serialNumber) > 0)
            {
                Result[0] = "SERIAL_MISSMATCH";
            }
            else
            {
                Result[0] = "VALID";
            }
            //DataTable dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ProductId, serialNumber);
            //try
            //{
            //    dtserial = new DataView(dtserial, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            //}
            //catch
            //{
            //}
            //if (dtserial.Rows.Count == 0)
            //{
            //    //here we are checking that serial already exist or not with another product in current final table
            //    DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMaster_By_SerialNo(serialNumber);
            //    if (dtStockBatch.Rows.Count == 0)
            //    {
            //        if (HttpContext.Current.Session["dtFinal"] != null)
            //        {
            //            if (new DataView((DataTable)HttpContext.Current.Session["dtFinal"], "SerialNo='" + serialNumber + "' and ProductId<>" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            //            {
            //                Result[0] = "VALID";
            //            }
            //            else
            //            {
            //                Result[0] = "SERIAL_MISSMATCH";
            //            }
            //        }
            //        else
            //        {
            //            Result[0] = "VALID";
            //        }
            //    }
            //    else
            //    {
            //        Result[0] = "SERIAL_MISSMATCH";
            //    }
            //}
            //else
            //{
            //    Result[0] = "DUPLICATE";
            //}
        }
        else
        {
            Result[0] = "DUPLICATE";
        }
        return Result;
    }
    protected void lnkAddSerial_Command(object sender, CommandEventArgs e)
    {
        lblCount.Text = Resources.Attendance.Serial_No + ":-";
        txtCount.Text = "0";
        Session["RdoType"] = "SerialNo";
        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["PID"] = e.CommandArgument.ToString();
        ViewState["RowIndex"] = Row.RowIndex;
        ViewState["POId"] = ((Label)Row.FindControl("lblPoID")).Text;
        lblpnlInvoiceQty.Text = ((TextBox)Row.FindControl("QtyReceived")).Text;
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblproductcode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblProductId")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        gvStockwithManf_and_expiry.DataSource = null;
        gvStockwithManf_and_expiry.DataBind();
        txtSerialNo.Text = "";
        pnlSerialNumber.Visible = false;
        pnlexp_and_Manf.Visible = false;
        int Counter = 0;
        DataTable dt = new DataTable();
        if (Session["dtFinal"] == null)
        {

        }
        else
        {
            dt = (DataTable)Session["dtFinal"];
            dt = new DataView(dt, "ProductId='" + ViewState["PID"].ToString() + "' and POID='" + ViewState["POId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            Session["dtSerial"] = dt;
        }
        if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "SNO")
        {
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            pnlSerialNumber.Visible = true;
            pnlexp_and_Manf.Visible = false;
            Counter = 1;
        }
        else if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LE" || ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FE")
        {
            LoadStores();
            pnlSerialNumber.Visible = false;
            pnlexp_and_Manf.Visible = true;
            try
            {
                gvStockwithManf_and_expiry.Columns[1].Visible = true;
                gvStockwithManf_and_expiry.Columns[2].Visible = false;
            }
            catch
            {
            }
            Counter = 1;
        }
        else if (ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LM" || ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FM")
        {
            LoadStores();
            pnlSerialNumber.Visible = false;
            pnlexp_and_Manf.Visible = true;
            try
            {
                gvStockwithManf_and_expiry.Columns[1].Visible = false;
                gvStockwithManf_and_expiry.Columns[2].Visible = true;
            }
            catch
            {
            }
            Counter = 1;
        }
        if (Counter == 0)
        {
            DisplayMessage("First set Manage inventory option in product Master");
            return;
        }
        txtSerialNo.Focus();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "View_Modal_Popup()", true);
    }

    public string setRoundValue(string Value)
    {
        string strRoundValue = string.Empty;
        try
        {
            strRoundValue = Math.Round(float.Parse(Value), 0).ToString();
        }
        catch
        {
        }
        return strRoundValue;
    }
    protected void btnResetSerial_Click(object sender, EventArgs e)
    {
        Session["dtSerial"] = null;
        txtSerialNo.Text = "";
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        LoadStores();
        txtCount.Text = "0";
        txtselectedSerialNumber.Text = "0";
        if (Session["dtFinal"] != null)
        {
            DataTable Dtfinal = (DataTable)Session["dtFinal"];
            if (ViewState["POId"].ToString() == "0")
            {
                Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "' and  POID<>'" + ViewState["POId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                int POId = Convert.ToInt32(ViewState["POId"].ToString());
            }
            Session["dtFinal"] = Dtfinal;
        }
        foreach (GridViewRow gvRow in GvProduct.Rows)
        {
            TextBox txtgvSalesQuantity = (TextBox)gvRow.FindControl("txtRecQty");
            if (gvRow.RowIndex == (int)ViewState["RowIndex"])
            {
                if (Session["dtSerial"] != null)
                {
                    txtgvSalesQuantity.Text = ((DataTable)Session["dtSerial"]).Rows.Count.ToString();
                }
                else
                {
                    txtgvSalesQuantity.Text = "0";
                }
                break;
            }
        }
        txtSerialNo.Focus();
    }
    protected void Btnloadfile_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Session["dtFinal"] == null)
        {
            dt = CreateStock_Datatable();
        }
        else
        {
            dt = (DataTable)Session["dtFinal"];
        }
        int counter = 0;
        int Index = 0;
        float recqty = 0;
        txtSerialNo.Text = "";
        try
        {
            string Path = Server.MapPath("~/Temp/" + FULogoPath.FileName);
            string[] csvRows = System.IO.File.ReadAllLines(Path);
            string[] fields = null;
            int i = 0;
            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(',');
                if (fields.Length == 1)
                {
                    if (fields[0].ToString() != "")
                    {
                        if (txtSerialNo.Text == "")
                        {
                            txtSerialNo.Text = fields[0].ToString();
                        }
                        else
                        {
                            txtSerialNo.Text += Environment.NewLine + fields[0].ToString();
                        }
                        counter++;
                    }
                }
                else
                {
                    if (Index == 0 || Index == 1)
                    {
                        Index++;
                        continue;
                    }
                    if (fields[5].ToString().Trim() == "")
                    {
                        continue;
                    }
                    DataRow dr = dt.NewRow();
                    dr[0] = ViewState["PID"].ToString();
                    dr[1] = fields[5].ToString();
                    dr[2] = "0";
                    dr[3] = "0";
                    dr[4] = "0";
                    dr[5] = DateTime.Now.ToString();
                    dr[6] = ViewState["POId"].ToString();
                    dr[7] = "PG";
                    dr[8] = "0";
                    dr[9] = DateTime.Now.ToString();
                    dr[10] = fields[4].ToString();
                    dr[12] = fields[2].ToString();
                    dr[13] = fields[3].ToString();
                    dr[14] = fields[6].ToString();
                    dt.Rows.Add(dr);
                    try
                    {
                        recqty += float.Parse(fields[4].ToString());
                    }
                    catch
                    {
                        recqty += 0;
                    }
                    counter++;
                    Index++;
                }
            }
            if (Directory.Exists(Path))
            {
                try
                {
                    Directory.Delete(Path);
                }
                catch
                {
                }
            }
            txtCount.Text = counter.ToString();
            if (Index > 0)
            {
                Session["dtFinal"] = dt;
                dt = new DataView(dt, "ProductId='" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                objPageCmn.FillData((Object)gvSerialNumber, dt, "", "");
                ((TextBox)GvProduct.Rows[(int)ViewState["RowIndex"]].FindControl("txtRecQty")).Text = recqty.ToString();
                Update_New.Update();
            }
        }
        catch
        {
            txtSerialNo.Text = "";
            DisplayMessage("File Not Found ,Try Again");
        }
        txtCount.Text = counter.ToString();
        if (counter == 0)
        {
            DisplayMessage("Serial Number Not Found");
        }
    }
    protected void IbtnDeleteserialNumber_Command(object sender, CommandEventArgs e)
    {
        if (Session["dtSerial"] != null)
        {
            DataTable dt = (DataTable)Session["dtSerial"];
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "SerialNo<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            Session["dtSerial"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
            dt = null;
        }
    }
    protected void btnsearchserial_Click(object sender, EventArgs e)
    {
        if (txtserachserialnumber.Text != "")
        {
            DataTable dt = new DataTable();
            if (Session["dtSerial"] != null)
            {
                dt = (DataTable)Session["dtSerial"];
                if (dt.Rows.Count > 0)
                {
                    dt = new DataView(dt, "SerialNo='" + txtserachserialnumber.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Serial Number Not Found");
                txtserachserialnumber.Text = "";
                txtserachserialnumber.Focus();
                return;
            }
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            btnRefreshserial.Focus();
            dt = null;
        }
        else
        {
            DisplayMessage("Enter Serial Number");
            txtserachserialnumber.Text = "";
            txtserachserialnumber.Focus();
        }
    }
    protected void btnRefreshserial_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (Session["dtSerial"] != null)
        {
            dt = (DataTable)Session["dtSerial"];
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
        txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
        txtserachserialnumber.Text = "";
        txtserachserialnumber.Focus();
        dt = null;
    }
    protected void gvSerialNumber_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtSerial"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtSerial"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
        lblSelectedRecord.Text = "";
        dt = null;

    }
    #region Lifo_and_fifo
    public void LoadStores()
    {
        DataTable dt = new DataTable();
        if (Session["dtSerial"] != null)
        {
            dt = new DataTable();
            dt = (DataTable)Session["dtSerial"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvStockwithManf_and_expiry, dt, "", "");
            }
            else
            {
                dt = new DataTable();
                dt = CreateStock_Datatable();
                dt.Rows.Add(dt.NewRow());
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvStockwithManf_and_expiry, dt, "", "");
                int TotalColumns = gvStockwithManf_and_expiry.Rows[0].Cells.Count;
                gvStockwithManf_and_expiry.Rows[0].Cells.Clear();
                gvStockwithManf_and_expiry.Rows[0].Cells.Add(new TableCell());
                gvStockwithManf_and_expiry.Rows[0].Cells[0].ColumnSpan = TotalColumns;
                gvStockwithManf_and_expiry.Rows[0].Visible = false;
            }
        }
        else
        {
            dt = CreateStock_Datatable();
            dt.Rows.Add(dt.NewRow());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvStockwithManf_and_expiry, dt, "", "");
            int TotalColumns = gvStockwithManf_and_expiry.Rows[0].Cells.Count;
            gvStockwithManf_and_expiry.Rows[0].Cells.Clear();
            gvStockwithManf_and_expiry.Rows[0].Cells.Add(new TableCell());
            gvStockwithManf_and_expiry.Rows[0].Cells[0].ColumnSpan = TotalColumns;
            gvStockwithManf_and_expiry.Rows[0].Visible = false;
        }
    }
    protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        string TaxId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtQuantity")).Text == "")
            {
                DisplayMessage("Enter Quantity");
                ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtQuantity")).Focus();
                return;
            }
            if (gvStockwithManf_and_expiry.Columns[1].Visible == true)
            {
                if (((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtExpiryFooter")).Text == "")
                {
                    DisplayMessage("Enter Expiry Date");
                    return;
                }
                else
                {
                    try
                    {
                        Convert.ToDateTime(((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtExpiryFooter")).Text);
                    }
                    catch
                    {
                        DisplayMessage("Expiry Date should be in correct Format");
                        ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtExpiryFooter")).Focus();
                        return;
                    }
                }
            }
            if (gvStockwithManf_and_expiry.Columns[2].Visible == true)
            {
                if (((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtManfacturerFooter")).Text == "")
                {
                    DisplayMessage("Enter Manufacture Date");
                    return;
                }
                else
                {
                    try
                    {
                        Convert.ToDateTime(((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtManfacturerFooter")).Text);
                    }
                    catch
                    {
                        DisplayMessage("Manufacturing Date should be in correct Format");
                        ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtManfacturerFooter")).Focus();
                        return;
                    }
                }
            }
            if (Session["dtSerial"] == null)
            {
                dt = CreateStock_Datatable();
                DataRow dr = dt.NewRow();
                dr[0] = ViewState["PID"].ToString();
                dr[1] = "0";
                dr[2] = "0";
                dr[3] = "0";
                dr[4] = "0";
                if (gvStockwithManf_and_expiry.Columns[1].Visible == true)
                {
                    dr[5] = ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtExpiryFooter")).Text;
                }
                else
                {
                    dr[5] = DateTime.Now.ToString();
                }
                dr[6] = ViewState["POId"].ToString();
                dr[7] = "PG";
                dr[8] = "0";
                if (gvStockwithManf_and_expiry.Columns[2].Visible == true)
                {
                    dr[9] = ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtManfacturerFooter")).Text;
                }
                else
                {
                    dr[9] = DateTime.Now.ToString();
                }
                dr[10] = ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtQuantity")).Text;
                dr[11] = 1;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = (DataTable)Session["dtSerial"];
                DataColumnCollection columns = dt.Columns;
                if (!columns.Contains("Trans_Id"))
                {
                    dt.Columns.Add("Trans_Id");
                }
                DataRow dr = dt.NewRow();
                DataTable dtCopy = dt.Copy();
                dtCopy = new DataView(dtCopy, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
                dr[0] = ViewState["PID"].ToString();
                dr[1] = "0";
                dr[2] = "0";
                dr[3] = "0";
                dr[4] = "0";
                if (gvStockwithManf_and_expiry.Columns[1].Visible == true)
                {
                    dr[5] = ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtExpiryFooter")).Text;
                }
                else
                {
                    dr[5] = DateTime.Now.ToString();
                }
                dr[6] = ViewState["POId"].ToString();
                dr[7] = "PG";
                dr[8] = "0";
                if (gvStockwithManf_and_expiry.Columns[2].Visible == true)
                {
                    dr[9] = ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtManfacturerFooter")).Text;
                }
                else
                {
                    dr[9] = DateTime.Now.ToString();
                }
                dr[10] = ((TextBox)gvStockwithManf_and_expiry.FooterRow.FindControl("txtQuantity")).Text;
                try
                {
                    dr[11] = float.Parse(dtCopy.Rows[0]["Trans_Id"].ToString()) + 1;
                }
                catch
                {
                    dr[11] = 1;
                }
                dt.Rows.Add(dr);
            }
            Session["dtSerial"] = dt;
        }
        if (e.CommandName.Equals("Delete"))
        {
            if (Session["dtSerial"] != null)
            {
                dt = (DataTable)Session["dtSerial"];
                dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtSerial"] = dt;
            }
        }
        gvStockwithManf_and_expiry.EditIndex = -1;
        LoadStores();
        dt = null;
    }
    protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }
    #endregion
    #endregion
    #region GoodsreceiveBYorder
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            using (DataTable dtSupplier = ObjSupplier.GetSupplierAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText))
            {
                string[] filterlist = new string[dtSupplier.Rows.Count];
                if (dtSupplier.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSupplier.Rows.Count; i++)
                    {
                        filterlist[i] = dtSupplier.Rows[i]["Filtertext"].ToString();
                    }
                }
                return filterlist;
            }
        }
        catch
        {
            return null;
        }
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            try
            {
                txtSupplierName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Enter Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                Session["DtOrderproduct"] = null;
                Session["dtPo"] = null;
                return;
            }
            DataTable dt = objContact.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                Session["DtOrderproduct"] = null;
                Session["dtPo"] = null;
            }
            else
            {
                Session["DtOrderproduct"] = null;
                Session["dtPo"] = null;
                gvSerachGrid.DataSource = null;
                gvSerachGrid.DataBind();
                DataTable dtPurchaseOrder = fillPOSearhgrid();
                if (dtPurchaseOrder.Rows.Count != 0)
                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)gvSerachGrid, dtPurchaseOrder, "", "");
                    Session["DtOrderproduct"] = dtPurchaseOrder;

                }
                else
                {
                    gvSerachGrid.DataSource = null;
                    gvSerachGrid.DataBind();
                    Session["DtOrderproduct"] = null;

                }
                dtPurchaseOrder = null;
            }
            dt = null;
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierName.Focus();
        }
    }
    public DataTable fillPOSearhgrid()
    {
        DataTable dtPurchaseOrder = null;
        using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(StrCompId, StrBrandId, StrLocationId, "PurchaseOrderApproval"))
        {
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    dtPurchaseOrder = new DataView(ObjPurchaseOrder.GetProductFromPurchaseOrderForInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSupplierName.Text.Trim().Split('/')[1].ToString()), "Field41='Approved'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    if (txtSupplierName.Text != "")
                        dtPurchaseOrder = ObjPurchaseOrder.GetProductFromPurchaseOrderForInvoice(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSupplierName.Text.Trim().Split('/')[1].ToString());
                }
            }

            dtPurchaseOrder.Columns["PONO"].ColumnName = "POID";
            dtPurchaseOrder.Columns["PONO1"].ColumnName = "PurchaseOrderNo";
            dtPurchaseOrder.Columns["Product_Id"].ColumnName = "ProductId";
            return dtPurchaseOrder;
        }
    }

    //Added New Function on 30-07-2024 By Lokesh for Select All
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in gvSerachGrid.Rows)
        {
            Label lblTransId = (Label)gvr.FindControl("TransId");
            CheckBox chkTrand = (CheckBox)gvr.FindControl("chkTrandId");

            DataTable dt = new DataTable();            
            dt = (DataTable)Session["DtOrderproduct"];
            dt = new DataView((DataTable)Session["DtOrderproduct"], "Trans_Id='" + lblTransId.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            getshipinformation_BY_OrderId(dt.Rows[0]["POID"].ToString());
            if (Hdn_Currency_Match.Value == "Not Matched")
            {
                return;
            }
            if (Session["dtPo"] != null)
            {
                DataTable dtPO = (DataTable)Session["dtPo"];
                dtPO.ImportRow(dt.Rows[0]);
                dt = dtPO.Copy();
            }
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //here righ code fo rbind gooods receive hgrid
            objPageCmn.FillData((object)GvProduct, dt, "", "");
            Session["dtPo"] = dt;
            dt = new DataView((DataTable)Session["DtOrderproduct"], "Trans_Id<>'" + lblTransId.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            Session["DtOrderproduct"] = dt;
            if (Session["dtProductSearch"] == null)
            {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
            }
            else
            {
                dt = new DataView((DataTable)Session["dtProductSearch"], "Trans_Id<>'" + lblTransId.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtProductSearch"] = dt;
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
            }
            dt = null;

        }
    }
    protected void chkTrandId_CheckedChanged(object seder, EventArgs e)
    {
        DataTable dt = new DataTable();
        GridViewRow row = (GridViewRow)((CheckBox)seder).Parent.Parent;
        dt = (DataTable)Session["DtOrderproduct"];
        dt = new DataView((DataTable)Session["DtOrderproduct"], "Trans_Id='" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        getshipinformation_BY_OrderId(dt.Rows[0]["POID"].ToString());
        if (Hdn_Currency_Match.Value == "Not Matched")
        {
            return;
        }
        if (Session["dtPo"] != null)
        {
            DataTable dtPO = (DataTable)Session["dtPo"];
            dtPO.ImportRow(dt.Rows[0]);
            dt = dtPO.Copy();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        //here righ code fo rbind gooods receive hgrid
        objPageCmn.FillData((object)GvProduct, dt, "", "");
        Session["dtPo"] = dt;
        dt = new DataView((DataTable)Session["DtOrderproduct"], "Trans_Id<>'" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        Session["DtOrderproduct"] = dt;
        if (Session["dtProductSearch"] == null)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
        }
        else
        {
            dt = new DataView((DataTable)Session["dtProductSearch"], "Trans_Id<>'" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            Session["dtProductSearch"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
        }
        dt = null;
    }
    public void getshipinformation_BY_OrderId(string strOrderId)
    {
        DataTable dt = ObjPurchaseOrder.GetPurchaseOrderTrueAllByReqId(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), strOrderId);
        if (dt.Rows.Count != 0)
        {
            if (Hdn_Invoice_Currency.Value != "0")
            {
                if (dt.Rows[0]["CurrencyId"].ToString() != Hdn_Invoice_Currency.Value.ToString())
                {
                    DisplayMessage("Purchase Order and Purchase Invoice currency for this supplier dose not match");
                    foreach (GridViewRow GVR in gvSerachGrid.Rows)
                    {
                        CheckBox Chk = (CheckBox)GVR.FindControl("chkTrandId");
                        Chk.Checked = false;
                    }
                    Hdn_Currency_Match.Value = "Not Matched";
                    return;
                }
            }
            Hdn_Currency_Match.Value = "Matched";
            Hdn_Invoice_Currency.Value = dt.Rows[0]["CurrencyId"].ToString();
            try
            {
                txtShippingLine.Text = objContact.GetContactTrueById(dt.Rows[0]["ShippingLine"].ToString()).Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["ShippingLine"].ToString();
            }
            catch
            {
            }
            ddlShipBy.SelectedValue = dt.Rows[0]["ShipBy"].ToString();
            ddlShipmentType.SelectedValue = dt.Rows[0]["ShipmentType"].ToString();
            ddlFreightStatus.SelectedValue = dt.Rows[0]["Freight_Status"].ToString();
            try
            {
                ddlShipUnit.SelectedValue = dt.Rows[0]["ShipUnit"].ToString();
            }
            catch
            {
                ddlShipUnit.SelectedIndex = 0;
            }
            txtAirwaybillno.Text = dt.Rows[0]["Airway_Bill_No"].ToString();
            txtvolumetricweight.Text = dt.Rows[0]["Volumetric_weight"].ToString();
            txtTotalWeight.Text = dt.Rows[0]["TotalWeight"].ToString();
            txtUnitRate.Text = dt.Rows[0]["UnitRate"].ToString();
            txtReceivingDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtShippingDate.Text = Convert.ToDateTime(dt.Rows[0]["DateShipping"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        dt = null;
    }



    protected void imgbtnsearch_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (Session["DtOrderproduct"] != null)
        {
            if (txtProductSerachValue.Text != "")
            {
                string condition = string.Empty;
                condition = "convert(" + ddlProductSerach.SelectedValue + ",System.String) like '%" + txtProductSerachValue.Text.Trim() + "%'";
                DataTable dtProductSearch = (DataTable)Session["DtOrderproduct"];
                DataView view = new DataView(dtProductSearch, condition, "", DataViewRowState.CurrentRows);
                Session["dtProductSearch"] = view.ToTable();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvSerachGrid, view.ToTable(), "", "");
                if (view.ToTable().Rows.Count == 0)
                {
                    DisplayMessage("No Record found");
                }
                dtProductSearch = null;
            }
        }
        else
        {
            DisplayMessage("No Record found");
        }
    }
    protected void ImgbtnRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["dtProductSearch"] = null;
        txtProductSerachValue.Text = "";
        ddlProductSerach.SelectedIndex = 1;
        DataTable dtPurchaseOrder = new DataTable();
        if (Session["DtOrderproduct"] != null)
        {
            dtPurchaseOrder = (DataTable)Session["DtOrderproduct"];
        }
        else
        {
            dtPurchaseOrder = fillPOSearhgrid();
        }
        if (dtPurchaseOrder.Rows.Count != 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dtPurchaseOrder, "", "");
            Session["DtOrderproduct"] = dtPurchaseOrder;

        }
        dtPurchaseOrder = null;
    }
    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtPo"];
        dt = new DataView(dt, "TransId <>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProduct, dt, "", "");
        if (Session["dtPo"] != null)
        {
            DataTable dtStorePO = dt;
            dt = new DataView((DataTable)Session["dtPo"], "TransId=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            try
            {
                dt.Columns["TransId"].ColumnName = "Trans_Id";
            }
            catch
            {
            }
            DataTable dtPO = (DataTable)Session["DtOrderproduct"];
            dtPO.ImportRow(dt.Rows[0]);
            if (dtPO.Rows.Count != 0)
            {
                try
                {
                    dtPO.Rows[dtPO.Rows.Count - 1]["TransId"] = dtPO.Rows[dtPO.Rows.Count - 1]["Trans_Id"].ToString();
                }
                catch
                {
                }
            }
            dt = dtPO;
            Session["dtProductSearch"] = dtPO;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSerachGrid, dtPO, "", "");
            Session["dtPo"] = dtStorePO;
            if (GvProduct.Rows.Count == 0)
            {
                Hdn_Invoice_Currency.Value = "0";
                Hdn_Currency_Match.Value = "0";
            }
        }
        Session["DtOrderproduct"] = dt;
        dt = null;
    }
    #endregion
    public void FillShipUnitddl()
    {
        objPageCmn.FillData((object)ddlShipUnit, objUnit.GetUnitMaster(Session["CompId"].ToString()), "Unit_Name", "Unit_Id");
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListShippingLine(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            using (DataTable dtContact = ObjContactMaster.GetAllContactAsPerFilterText(prefixText))
            {
                string[] filterlist = new string[dtContact.Rows.Count];
                if (dtContact.Rows.Count > 0)
                {
                    for (int i = 0; i < dtContact.Rows.Count; i++)
                    {
                        filterlist[i] = dtContact.Rows[i]["Filtertext"].ToString();
                    }
                }
                return filterlist;
            }
        }
        catch
        {
            return null;
        }

    }
    protected void txtShippingLine_TextChanged(object sender, EventArgs e)
    {
        if (txtShippingLine.Text != "")
        {
            try
            {
                string ContactId = string.Empty;
                try
                {
                    ContactId = txtShippingLine.Text.Split('/')[1].ToString();
                }
                catch
                {
                    ContactId = "0";
                }
                if ((ContactId == "") || (ContactId == "0"))
                {
                    DisplayMessage("Please Select from Suggestions Only");
                    txtShippingLine.Text = "";
                    txtShippingLine.Focus();
                    return;
                }
            }
            catch
            {
            }
        }
    }
    protected void getTotalVolume(object sender, EventArgs e)
    {
        decimal length = 0;
        decimal height = 0;
        decimal width = 0;
        decimal carton = 0;
        try
        {
            length = decimal.Parse(txtLength.Text);
        }
        catch
        {
        }
        try
        {
            height = decimal.Parse(txtheight.Text);
        }
        catch
        {
        }
        try
        {
            width = decimal.Parse(txtwidth.Text);
        }
        catch
        {
        }
        try
        {
            carton = decimal.Parse(txtcartons.Text);
        }
        catch
        {
        }
        txttotal.Text = ((length * height * width) * carton).ToString();
    }
    public void shippingCalculation()
    {
        float totalsum = 0;
        foreach (GridViewRow gvRow in gvShippingInformation.Rows)
        {
            try
            {
                totalsum += float.Parse(((Label)gvRow.FindControl("lblTotal")).Text);
            }
            catch
            {
            }
        }
        txttotalVolume.Text = totalsum.ToString();
        if (txtdivideby.Text == "")
        {
            txtdivideby.Text = "0";
        }
        if (float.Parse(txtdivideby.Text) > 0)
        {
            txttotalvolumetricweight.Text = (float.Parse(txttotalVolume.Text) / float.Parse(txtdivideby.Text)).ToString();
        }
    }
    protected void txtdivideby_OnTextChanged(object sender, EventArgs e)
    {
        shippingCalculation();
    }
    protected void IbtnAddShippingInfo_Click(object sender, EventArgs e)
    {
        float serialNo = 0;
        if (txtLength.Text == "")
        {
            txtLength.Text = "0";
        }
        if (txtheight.Text == "")
        {
            txtheight.Text = "0";
        }
        if (txtwidth.Text == "")
        {
            txtwidth.Text = "0";
        }
        if (txtcartons.Text == "")
        {
            txtcartons.Text = "0";
        }
        try
        {
            txttotal.Text = ((Convert.ToDouble(txtLength.Text) * Convert.ToDouble(txtheight.Text) * Convert.ToDouble(txtwidth.Text)) * Convert.ToDouble(txtcartons.Text)).ToString();
        }
        catch
        {
        }
        DataTable dt = (DataTable)Session["dtPackingList"];
        if (dt == null)
        {
            dt = new DataTable();
            dt.Columns.Add("Trans_Id", typeof(float));
            dt.Columns.Add("Length", typeof(string));
            dt.Columns.Add("Height", typeof(string));
            dt.Columns.Add("Width", typeof(string));
            dt.Columns.Add("Cartons", typeof(int));
            dt.Columns.Add("Total", typeof(string));
        }
        else
        {
            if (dt.Rows.Count == 0)
            {
                if (!dt.Columns.Contains("Trans_Id"))
                {
                    dt.Columns.Add("Trans_Id", typeof(float));
                    dt.Columns.Add("Length", typeof(string));
                    dt.Columns.Add("Height", typeof(string));
                    dt.Columns.Add("Width", typeof(string));
                    dt.Columns.Add("Cartons", typeof(int));
                    dt.Columns.Add("Total", typeof(string));
                }
            }
            else
            {
                serialNo = float.Parse(new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1;
            }
        }
        dt.Rows.Add(serialNo, txtLength.Text, txtheight.Text, txtwidth.Text, txtcartons.Text, txttotal.Text);
        Session["dtPackingList"] = dt;
        objPageCmn.FillData((object)gvShippingInformation, dt, "", "");
        shippingCalculation();
        txtheight.Text = "";
        txtLength.Text = "";
        txtwidth.Text = "";
        txtcartons.Text = "";
        txttotal.Text = "";
        setFooterVisibility();
    }
    protected void IbtnDeleteShipping_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPackingList"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                var rows = dt.Select("Trans_Id ='" + e.CommandArgument.ToString() + "'");
                foreach (var row in rows)
                    row.Delete();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                Session["dtPackingList"] = dt;
                objPageCmn.FillData((object)gvShippingInformation, dt, "", "");
                shippingCalculation();
            }
        }
        setFooterVisibility();
        dt = null;
    }
    #region PendingOrder
    protected void ddlQSeleclField_SelectedIndexChanged(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "PODate" || ddlQSeleclField.SelectedItem.Value == "DeliveryDate")
        {
            txtQValueDate.Visible = true;
            txtQValue.Visible = false;
            txtQValue.Text = "";
            txtQValueDate.Text = "";
        }
        else
        {
            txtQValueDate.Visible = false;
            txtQValue.Visible = true;
            txtQValueDate.Text = "";
            txtQValue.Text = "";
        }
    }
    protected void ImgBtnQBind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "PODate" || ddlQSeleclField.SelectedItem.Value == "DeliveryDate")
        {
            if (txtQValueDate.Text != "")
            {
                try
                {
                    ObjSysParam.getDateForInput(txtQValueDate.Text);
                    txtQValue.Text = Convert.ToDateTime(txtQValueDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtQValueDate.Text = "";
                    txtQValue.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQValueDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtQValueDate.Focus();
                txtQValue.Text = "";
                return;
            }
            txtQValueDate.Focus();
        }
        FillGridPendingOrder();
    }
    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        ddlQSeleclField.SelectedIndex = 0;
        ddlQOption.SelectedIndex = 2;
        txtQValue.Text = "";
        txtQValueDate.Text = "";
        txtQValue.Visible = true;
        txtQValueDate.Visible = false;
        FillGridPendingOrder();
    }

    protected void gvPurchaseOrder_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (gvPurchaseOrder.Attributes["CurrentSortField"] != null &&
            gvPurchaseOrder.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == gvPurchaseOrder.Attributes["CurrentSortField"])
            {
                if (gvPurchaseOrder.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        gvPurchaseOrder.Attributes["CurrentSortField"] = sortField;
        gvPurchaseOrder.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGridPendingOrder(Int32.Parse(hdnGvPurchaseOrderCurrentPageIndex.Value));
    }
    public string GetDateFromat(string Date)
    {
        try
        {
            return Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
        }
        catch
        {
            return "";
        }
    }
    private void FillGridPendingOrder(int currentPageIndex = 1)
    {
        string strSearchCondition = string.Empty;
        if (ddlQOption.SelectedIndex != 0 && txtQValue.Text != string.Empty)
        {
            if (ddlQOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlQSeleclField.SelectedValue + "='" + txtQValue.Text.Trim() + "'";
            }
            else if (ddlQOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlQSeleclField.SelectedValue + " like '%" + txtQValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlQSeleclField.SelectedValue + " Like '" + txtQValue.Text.Trim() + "'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = ObjPurchaseInvoice.getPendingOrderList((currentPageIndex - 1).ToString(), PageControlCommon.GetPageSize().ToString(), gvPurchaseOrder.Attributes["CurrentSortField"], gvPurchaseOrder.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvPurchaseOrder, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                gvPurchaseOrder.DataSource = null;
                gvPurchaseOrder.DataBind();
                lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPagerPO, totalRows, currentPageIndex);
        }
    }
    protected void PagePO_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvPurchaseOrderCurrentPageIndex.Value = pageIndex.ToString();
        FillGridPendingOrder(pageIndex);
    }
    //for child grid
    protected void btnPendingOrder_Click(object sender, EventArgs e)
    {
        FillGridPendingOrder();
        //ddlCurrency_SelectedIndexChanged(null, null);
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(CurrencyId, ObjSysParam.GetCurencyConversionForInv(CurrencyId, Amount), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }
        return Amountwithsymbol;
    }
    #endregion
    #region Posted
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    #endregion
    #region GetLocation
    private void FillddlLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "Location_Name", DataViewRowState.CurrentRows).ToTable();
        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        //Common Function add By jitendra on 06-02-2016
        ddlLocation.DataSource = dtLoc;
        ddlLocation.DataTextField = "Location_Name";
        ddlLocation.DataValueField = "Location_Id";
        ddlLocation.DataBind();
        dtLoc = null;
    }
    #endregion
    public string GetAmountDecimal(string Amount)
    {
        return SystemParameter.GetAmountWithDecimal(Amount, Session["LoginLocDecimalCount"].ToString());
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }

    protected void lbtnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + e.CommandName.ToString() + "/PurGoodsRec", "Purchase", "PurGoodsRec", e.CommandArgument.ToString(), "");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    protected void btnGenerteSerial_Click(object sender, EventArgs e)
    {
        try
        {
            string strDefaultSno = ObjProductMaster.getProductAutoSerialInitials(lblProductIdvalue.Text, HttpContext.Current.Session["CompId"].ToString());
            if (strDefaultSno == string.Empty)
            {
                DisplayMessage("Configuration settions does not exist for auto serial system. please check parameters");
                return;
            }
            int reqQty = 0;
            if (lblpnlInvoiceQty.Text.Contains("."))
            {
                reqQty = Int32.Parse(lblpnlInvoiceQty.Text.Substring(0, lblpnlInvoiceQty.Text.IndexOf('.')));
            }
            else
            {
                reqQty = Int32.Parse(lblpnlInvoiceQty.Text);
            }
            using (DataTable _dt = ObjProductMaster.getAutoProductSerial(ViewState["PID"].ToString(), reqQty, strDefaultSno, HttpContext.Current.Session["CompId"].ToString()))
            {
                if (_dt == null || _dt.Rows.Count == 0)
                {
                    DisplayMessage("There is error to generate serial please contact technical person");
                    return;
                }
                txtSerialNo.Text = string.Empty;
                foreach (DataRow dr in _dt.Rows)
                {
                    if (txtSerialNo.Text == string.Empty)
                    {
                        txtSerialNo.Text = dr["sno"].ToString();
                    }
                    else
                    {
                        txtSerialNo.Text += Environment.NewLine + dr["sno"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}