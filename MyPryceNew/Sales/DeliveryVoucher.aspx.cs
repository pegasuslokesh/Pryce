using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Sales_DeliveryVoucher : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster ObjContactMaster = null;
    Common cmn = null;
    Inv_ParameterMaster objInvParam = null;
    Inv_SalesOrderHeader objSOrderHeader = null;
    Inv_ProductMaster objProductM = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_SalesDeliveryVoucher_Header objVoucherHeader = null;
    Inv_SalesDeliveryVoucher_Detail objVoucherDetail = null;
    EmployeeMaster objEmployee = null;
    Set_DocNumber objDocNo = null;
    Inv_ProductLedger ObjProductLedger = null;
    LocationMaster objLocation = null;
    PageControlCommon objPageCmn = null;

    string StrLocationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objSOrderHeader = new Inv_SalesOrderHeader(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Inv_SalesDeliveryVoucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Inv_SalesDeliveryVoucher_Detail(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjProductLedger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        btnSInvSave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSInvSave, "").ToString());
        if (!IsPostBack)
        {
           
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Sales/DeliveryVoucher.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            CalendertxtVoucherDate.Format = Session["DateFormat"].ToString();
            ResetDetailPanel();
            ViewState["DocNo"] = GetDocumentNumber();
            txtVoucherNo.Text = ViewState["DocNo"].ToString();
            txtVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Session["dtFinal"] = null;
            Session["dtSerial"] = null;

            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtQValue.Format = Session["DateFormat"].ToString();
            if (Request.QueryString["Id"] != null)
            {
                
                LinkButton imgeditbutton = new LinkButton();
                imgeditbutton.ID = "lnkViewDetail";
                btnEdit_Command(imgeditbutton, new CommandEventArgs("commandName", Request.QueryString["Id"].ToString()));
                try
                {
                    StrLocationId = Request.QueryString["LocId"].ToString();
                }
                catch
                {
                    StrLocationId = Session["LocId"].ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_upload_Active()", true);
            }
            else
            {
                StrLocationId = Session["LocId"].ToString();
            }
            pnlScanProduct.Visible = Inventory_Common.IsScanningsolutioninSales(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            FillGrid();
        }
    }
   
    #region allPagecode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSInvSave.Visible = clsPagePermission.bAdd;
        btnPost.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    #endregion
    #region DocumentNo
    protected string GetDocumentNumber()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "13", "327", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    #endregion
    #region Message
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
    #endregion

    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        string StrCustomerId = string.Empty;

        if (txtCustomer.Text != "")
        {
            try
            {
                StrCustomerId = txtCustomer.Text.Trim().Split('/')[3].ToString();

                using (DataTable dt = ObjContactMaster.GetContactByContactName(txtCustomer.Text.Trim().Split('/')[0].ToString()))
                {
                    if (dt.Rows.Count == 0)
                    {
                        DisplayMessage("Select Customer Name");
                        txtCustomer.Text = "";
                        txtCustomer.Focus();
                        ResetDetailPanel();
                        return;
                    }
                    else
                    {
                        ResetDetailPanel();
                        using (DataTable dtsalesorder = fillSOSearhgrid())
                        {
                            if (dtsalesorder != null)
                            {
                                if (dtsalesorder.Rows.Count != 0)
                                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                                    objPageCmn.FillData((object)gvSerachGrid, dtsalesorder, "", "");
                                    Session["Dtproduct"] = dtsalesorder;

                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                DisplayMessage("Select Customer Name in suggestion only");
                txtCustomer.Text = "";
                txtCustomer.Focus();
                ResetDetailPanel();
                return;
            }
        }
        else
        {
            DisplayMessage("Select Customer Name");
            txtCustomer.Focus();
            ResetDetailPanel();
        }
        txtscanProduct.Focus();
    }
    protected void txtSalesPerson_TextChanged(object sender, EventArgs e)
    {
        string strEmployeeId = string.Empty;
        if (txtSalesPerson.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            strEmployeeId = Emp_ID;
            if (strEmployeeId != "" && strEmployeeId != "0")
            {
            }
            else
            {
                DisplayMessage("Select Employee In Suggestions Only");
                txtSalesPerson.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSalesPerson);
            }
        }
    }
    
    public void ResetDetailPanel()
    {
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        gvSerachGrid.DataSource = null;
        gvSerachGrid.DataBind();
        Session["Dtproduct"] = null;
        Session["dtPo"] = null;
    }
    #region GridDetailCode
    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dtSerialDetail = (DataTable)Session["dtFinal"];
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtPo"];
        dt = new DataView(dt, "Trans_Id <>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        if (Session["dtPo"] != null)
        {
            DataTable dtStorePO = dt;
            dt = new DataView((DataTable)Session["dtPo"], "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtPO = (DataTable)Session["Dtproduct"];
            dtPO.ImportRow(dt.Rows[0]);

            dt = dtPO;
            Session["dtPo"] = dtStorePO;
        }
        Session["Dtproduct"] = dt;
        objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
        //here we deleting serial detail for this product
        if (dtSerialDetail != null)
        {
            dtSerialDetail = new DataView(dtSerialDetail, "Product_Id='" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "' and SOrderNo='" + ((Label)gvrow.FindControl("lblSOId")).Text + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dtSerialDetail.Rows.Count > 0)
            {
                string s = "SOrderNo Not In('" + ((Label)gvrow.FindControl("lblSOId")).Text + "') or Product_Id Not In('" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "')";
                dtSerialDetail = new DataView(dtSerialDetail, s.ToString(), "", DataViewRowState.CurrentRows).ToTable();
            }
            Session["dtFinal"] = dtSerialDetail;
        }
    }
    protected void chkTrandId_CheckedChanged(object seder, EventArgs e)
    {
        DataTable dt = new DataTable();
        GridViewRow row = (GridViewRow)((CheckBox)seder).Parent.Parent;
        dt = (DataTable)Session["Dtproduct"];
        dt = new DataView((DataTable)Session["Dtproduct"], "Trans_Id='" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        using (DataTable DtParameter = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDeliveryScanning"))
        {
            if (DtParameter.Rows.Count > 0)
            {
                if (Convert.ToBoolean(DtParameter.Rows[0]["ParameterValue"]) == true)
                {
                    try
                    {
                        dt.Rows[0]["Delievered_Qty"] = "1";
                    }
                    catch
                    {

                    }
                }
            }
        }
        if (Session["dtPo"] != null)
        {

            DataTable dtPO = (DataTable)Session["dtPo"];

            dtPO.ImportRow(dt.Rows[0]);
            dt = new DataView(dtPO, "", "Serial_No Asc", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        Session["dtPo"] = dt;
        dt = new DataView((DataTable)Session["Dtproduct"], "Trans_Id<>'" + ((Label)row.FindControl("TransId")).Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        Session["Dtproduct"] = dt;
        objPageCmn.FillData((object)gvSerachGrid, dt, "", "");
        if (txtscanProduct.Text.Trim() != "")
        {
            FillSerialForScanningsolution(((Label)row.FindControl("lblsoid")).Text.Trim());
            txtscanProduct.Text = "";
            txtscanProduct.Focus();
        }
    }
    public void FillSerialForScanningsolution(string strOrderId)
    {
        bool Isserial = false;
        using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDeliveryScanning"))
        {
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    DataTable dt = new DataTable();
                    using (dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtscanProduct.Text.ToString()))
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //heer we checking that scaned text is serial number or not
                            //code start
                            if (dt.Rows[0]["Type"].ToString() == "2")
                            {
                                Isserial = true;
                            }

                            if (Isserial)
                            {
                                addSerialfnc(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text.Trim(), strOrderId);
                            }
                        }
                    }
                }
            }
        }
    }
    public DataTable fillSOSearhgrid()
    {
        DataTable dtSalesOrder = null;
        using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SalesOrderApproval"))
        {
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    dtSalesOrder = new DataView(objVoucherHeader.GetProductFromSalesOrderForDeliveryVoucher(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtCustomer.Text.Trim().Split('/')[3].ToString(),Session["FinanceYearId"].ToString()), "Field4='Approved'", "", DataViewRowState.CurrentRows).ToTable();

                }
                else
                {
                    dtSalesOrder = objVoucherHeader.GetProductFromSalesOrderForDeliveryVoucher(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtCustomer.Text.Trim().Split('/')[3].ToString(), Session["FinanceYearId"].ToString());
                }
            }
        }
        return dtSalesOrder;
    }
    protected void GvProductDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((HiddenField)e.Row.FindControl("hdngvProductId")).Value;
            string So_Id = ((Label)e.Row.FindControl("lblSOId")).Text;
            GridView gvchildGrid = (GridView)e.Row.FindControl("gvchildGrid");
            if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
            {
                if (Lbl_Tab_New.Text == Resources.Attendance.New || Lbl_Tab_New.Text == Resources.Attendance.Edit)
                {
                    ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = true;
                }
            }
            else
            {
                ((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = true;
                ((LinkButton)e.Row.FindControl("lnkAddSO")).Visible = false;
            }
        }
    }
    
    
    protected void txtgvSalesQuantity_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        if (((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text == "")
        {
            ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text = "0";
        }
        if (((Label)gvrow.FindControl("lblgvSystemQuantity")).Text == "")
        {
            ((Label)gvrow.FindControl("lblgvSystemQuantity")).Text = "0";
        }
        if (((Label)gvrow.FindControl("lblgvRemaningQuantity")).Text == "")
        {
            ((Label)gvrow.FindControl("lblgvRemaningQuantity")).Text = "0";
        }
        if (Convert.ToDouble(((Label)gvrow.FindControl("lblgvRemaningQuantity")).Text) < Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text))
        {
            DisplayMessage("Request Quantity Should be less than or equal to the remaining quantity");
            ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text = "0";
            return;
        }
    }
    #endregion
    #region saveoperation
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }
        chkPost.Checked = true;
        btnSInvSave_Click(sender, e);
    }
    protected void btnSInvSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        Button btn = (Button)sender;

        if (btn.ID.Trim() == "btnPost")
        {
            chkPost.Checked = true;
        }
        else
        {
            chkPost.Checked = false;
        }

        if (txtVoucherNo.Text == "")
        {
            DisplayMessage("Enter Voucher No");
            txtVoucherNo.Focus();
            return;
        }

        if (txtVoucherDate.Text == "")
        {
            DisplayMessage("Enter Voucher date");
            txtVoucherNo.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtVoucherDate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid Voucher date");
                txtVoucherNo.Focus();

                return;
            }
        }


        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtVoucherDate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


        if (txtCustomer.Text == "")
        {
            DisplayMessage("Enter Customer Name");
            txtCustomer.Focus();

            return;
        }

        if (txtSalesPerson.Text == "")
        {
            DisplayMessage("Enter Sales person");
            txtSalesPerson.Focus();
            return;
        }


        if (gvProduct.Rows.Count == 0)
        {
            DisplayMessage("Product Not Found");

            return;
        }
        bool Isallow = false;
        string OrderId = string.Empty;


        foreach (GridViewRow gvrow in gvProduct.Rows)
        {
            if (((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text == "")
            {
                ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text = "0";

            }
            if (((Label)gvrow.FindControl("lblgvSystemQuantity")).Text == "")
            {
                ((Label)gvrow.FindControl("lblgvSystemQuantity")).Text = "0";
            }

            if (chkPost.Checked)
            {
                if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString() == "S")
                {

                    if (Convert.ToDouble(((Label)gvrow.FindControl("lblgvSystemQuantity")).Text) < Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text))
                    {
                        DisplayMessage("stock is not available !");

                        return;
                    }
                }
            }
            if (Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text) > 0)
            {
                Isallow = true;
                OrderId = ((Label)gvrow.FindControl("lblSOId")).Text;
            }

        }

        if (!Isallow)
        {

            DisplayMessage("Enter quantity !");

            return;
        }


        //here we checking serial validation when serial validation parameter is true in inventory pages
        //cpde created on 16-09-2016
        //code created by jitendra upadhyay 


        if (chkPost.Checked)
        {

            foreach (GridViewRow gvr in gvProduct.Rows)
            {
                double TotalSalesqty = 0;

                TotalSalesqty = Convert.ToDouble(((TextBox)gvr.FindControl("txtgvSalesQuantity")).Text);

                if (TotalSalesqty > 0)
                {
                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvr.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
                    {
                        DataTable dt = (DataTable)Session["dtFinal"];
                        dt = new DataView(dt, "Product_Id='" + ((HiddenField)gvr.FindControl("hdngvProductId")).Value + "' ", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt.Rows.Count == 0)
                        {
                            DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblgvProductCode")).Text);
                            return;
                        }

                        if (dt.Rows.Count != TotalSalesqty)
                        {
                            DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblgvProductCode")).Text);
                            return;
                        }
                    }
                }
            }
        }
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            int b = 0;
            if (editid.Value == "")
            {
                //here we check that invoice number is duplicate or bot which is generated according 

                //code start
                string strInvoiceNo = string.Empty;
                if (txtVoucherNo.Text == ViewState["DocNo"].ToString())
                {

                    DataTable Dttemp = new DataTable();
                    string count = (objVoucherHeader.GetRecordCount(Session["LocId"].ToString(), ref trns) + 1).ToString();
                    strInvoiceNo = txtVoucherNo.Text + count;
                    txtVoucherNo.Text = strInvoiceNo;
                    try
                    {
                        Dttemp = new DataView(Dttemp, "Voucher_No = '" + strInvoiceNo + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {

                    }
                    if (Dttemp.Rows.Count > 0)
                    {
                        DisplayMessage("Generated Voucher no. already exists");
                        txtVoucherNo.Focus();
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

                //code end
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                b = objVoucherHeader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), OrderId, txtCustomer.Text.Split('/')[3].ToString(), Emp_ID, chkPost.Checked.ToString(), txtRemarks.Text, "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["Userid"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                int i = 0;
                foreach (GridViewRow gvrow in gvProduct.Rows)
                {
                    if (((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text = "0";

                    }

                    i++;
                    objVoucherDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), ((Label)gvrow.FindControl("lblSOId")).Text, i.ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, ((Label)gvrow.FindControl("lblgvOrderqty")).Text, ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text, chkPost.Checked.ToString(), ref trns);
                    //code is modified by jitendra upadhyay on 09-08-2016
                    //code modifed for also insert row of non stockable item in ledger table for check complete cycle 
                    int LedgerId = 0;
                    if (chkPost.Checked)
                    {
                        LedgerId = ObjProductLedger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", b.ToString(), "0", ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, "O", "0", "0", "0", ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text, "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }
                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                    {
                        string strIventoryMethod = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MainTainStock"].ToString();

                        if (strIventoryMethod == "SNO")
                        {
                            ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", b.ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((Label)gvrow.FindControl("lblSOId")).Text, ref trns);

                            if (Session["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)Session["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "' and SOrderNo='" + ((Label)gvrow.FindControl("lblSOId")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                                foreach (DataRow dr in dt.Rows)
                                {
                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", b.ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", ((Label)gvrow.FindControl("lblSOId")).Text, "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }

                        }
                        //else if (strIventoryMethod == "FM" || strIventoryMethod == "FE" || strIventoryMethod == "LM" || strIventoryMethod == "LE")
                        //{
                        //    ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", b.ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((Label)gvrow.FindControl("lblSOId")).Text, ref trns);
                        //    UpdateRecordForStckableItem(((HiddenField)gvrow.FindControl("hdngvProductId")).Value, objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MaintainStock"].ToString(), Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text), b.ToString(), ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, ((Label)gvrow.FindControl("lblSOId")).Text, LedgerId.ToString(), ref trns);
                        //}
                    }


                }


                if (chkPost.Checked)
                {
                    DisplayMessage("Record Posted Successfully");
                    //    cmn.DisableControls(this.Page, true);
                    //}
                }
                else
                {
                    DisplayMessage("Record Saved","green");
                }

            }
            else
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtSalesPerson.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                objVoucherHeader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), OrderId, txtCustomer.Text.Split('/')[3].ToString(), Emp_ID, chkPost.Checked.ToString(), txtRemarks.Text, "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //first we delete old record in detail table using voucher id 

                objVoucherDetail.DeleteRecord_By_VoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ref trns);

                int i = 0;
                foreach (GridViewRow gvrow in gvProduct.Rows)
                {
                    if (((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text == "")
                    {
                        ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text = "0";

                    }

                    i++;
                    objVoucherDetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ((Label)gvrow.FindControl("lblSOId")).Text, i.ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, ((Label)gvrow.FindControl("lblgvOrderqty")).Text, ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text, chkPost.Checked.ToString(), ref trns);
                    //code is modified by jitendra upadhyay on 09-08-2016
                    //code modifed for also insert row of non stockable item in ledger table for check complete cycle 
                    int LedgerId = 0;
                    if (chkPost.Checked)
                    {
                        LedgerId = ObjProductLedger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", editid.Value, "0", ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, "O", "0", "0", "0", ((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text, "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);
                    }

                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["ItemType"].ToString() == "S")
                    {
                        string strIventoryMethod = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns).Rows[0]["MainTainStock"].ToString();

                        if (strIventoryMethod == "SNO")
                        {

                            ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", editid.Value, ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((Label)gvrow.FindControl("lblSOId")).Text, ref trns);

                            if (Session["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)Session["dtFinal"];
                                dt = new DataView(dt, "Product_Id='" + ((HiddenField)gvrow.FindControl("hdngvProductId")).Value + "' and SOrderNo='" + ((Label)gvrow.FindControl("lblSOId")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();

                                foreach (DataRow dr in dt.Rows)
                                {
                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", editid.Value, ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), "1", dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), "1/1/1800", dr["BarcodeNo"].ToString(), "", "", "", ((Label)gvrow.FindControl("lblSOId")).Text, "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }

                        }
                        //else if (strIventoryMethod == "FM" || strIventoryMethod == "FE" || strIventoryMethod == "LM" || strIventoryMethod == "LE")
                        //{
                        //    ObjStockBatchMaster.DeleteStockBatchMasterBySalesOrderNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", editid.Value, ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, ((Label)gvrow.FindControl("lblSOId")).Text, ref trns);
                        //    UpdateRecordForStckableItem(((HiddenField)gvrow.FindControl("hdngvProductId")).Value, objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString(), Convert.ToDouble(((TextBox)gvrow.FindControl("txtgvSalesQuantity")).Text), editid.Value, ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value, ((Label)gvrow.FindControl("lblSOId")).Text, LedgerId.ToString(), ref trns);
                        //}
                    }

                }

                if (chkPost.Checked)
                {
                    DisplayMessage("Record Posted Successfully");
                    //    cmn.DisableControls(this.Page, true);
                    //}
                }
                else
                {
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }

            }

            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
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
    public void Reset()
    {
        ViewState["DocNo"] = GetDocumentNumber();
        txtVoucherNo.Text = ViewState["DocNo"].ToString();
        txtVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtCustomer.Text = "";
        ResetDetailPanel();
        txtSalesPerson.Text = "";
        txtRemarks.Text = "";
        editid.Value = "";
        chkPost.Checked = false;
        txtVoucherNo.Focus();
        //FillGrid();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        txtVoucherNo.Enabled = true;
        txtCustomer.Enabled = true;
        Session["dtFinal"] = null;
        Session["dtSerial"] = null;
        btnPost.Enabled = true;
        btnSInvSave.Enabled = true;
        BtnReset.Visible = true;
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnSInvCancel_Click(object sender, EventArgs e)
    {
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void UpdateRecordForStckableItem(string ProductId, string ItemType, double Quantity, string InvoiceId, string UnitId, string SoId, string Ledgerid, ref SqlTransaction trns)
    {

        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        double Currencyquantity = 0;
        DataTable dt = new DataTable();
        dt = ObjStockBatchMaster.GetStockBatchMasterAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
        //for fifo expiry date
        if (ItemType == "FE")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ExpiryDate asc", DataViewRowState.CurrentRows).ToTable();
        }
        //for lifo expiry date
        else if (ItemType == "LE")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ExpiryDate desc", DataViewRowState.CurrentRows).ToTable();

        }
        //for fifo manufacturing date
        else if (ItemType == "FM")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ManufacturerDate asc", DataViewRowState.CurrentRows).ToTable();
        }
        //for lifo manufacturing date
        else
            if (ItemType == "LM")
        {
            dt = new DataView(dt, "ProductId=" + ProductId + " and InOut='I'", "ManufacturerDate desc", DataViewRowState.CurrentRows).ToTable();

        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Currencyquantity = Convert.ToDouble(dt.Rows[i]["Quantity"].ToString());

            if (Quantity == 0)
            {
                break;
            }
            string sql = "select SUM(quantity) from Inv_StockBatchMaster where Field3='" + dt.Rows[i]["Trans_Id"].ToString() + "' and InOut='O'";

            if (da.return_DataTable(sql, ref trns).Rows.Count > 0)
            {
                try
                {
                    if (Currencyquantity == Convert.ToDouble(da.return_DataTable(sql, ref trns).Rows[0][0].ToString()))
                    {
                        continue;
                    }
                    else
                    {
                        double Remqty = 0;

                        Remqty = Currencyquantity - Convert.ToDouble(da.return_DataTable(sql, ref trns).Rows[0][0].ToString());
                        if (Remqty > Quantity)
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", InvoiceId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = 0;
                        }
                        else
                        {
                            ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", InvoiceId, ProductId, UnitId, "O", "0", "0", Remqty.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                            Quantity = Quantity - Remqty;

                        }

                    }
                }
                catch
                {
                    if (Currencyquantity > Quantity)
                    {

                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", InvoiceId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                        Quantity = 0;
                    }
                    else
                    {
                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", InvoiceId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                        Quantity = Quantity - Currencyquantity;
                    }
                }

            }
            else
            {
                if (Currencyquantity > Quantity)
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", InvoiceId, ProductId, UnitId, "O", "0", "0", Quantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                    Quantity = 0;
                }
                else
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", InvoiceId, ProductId, UnitId, "O", "0", "0", Currencyquantity.ToString(), dt.Rows[i]["ExpiryDate"].ToString(), "0", dt.Rows[i]["ManufacturerDate"].ToString(), "0", "", "", "", SoId, "", dt.Rows[i]["Trans_Id"].ToString(), Ledgerid, "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                    Quantity = Quantity - Currencyquantity;
                }
            }
        }
    }
    #endregion
    #region GridandFilter
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
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and IsActive='True' and Post='True'";
        }
        else if (ddlPosted.SelectedItem.Value == "PostNotInvoice")
        {
            strWhereClause = "" + Session["LocId"].ToString() + "";
        }
        else if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and IsActive='True' and  Post='False'";
        }
        else if (ddlPosted.SelectedItem.Value == "Cancel")
        {
            strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and IsActive='False'";
        }
        else
        {
            strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and IsActive='True'";
        }

        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        try
        {
            if (ddlPosted.SelectedItem.Value == "PostNotInvoice")
            {
                using (DataTable dt = objVoucherHeader.getVouhcerListPostButSalesInvoiceNotCreate((currentPageIndex - 1).ToString(), int.Parse(Session["GridSize"].ToString()).ToString(), gvDeliveryVoucher.Attributes["CurrentSortField"], gvDeliveryVoucher.Attributes["CurrentSortDirection"], strWhereClause))
                {
                    if (dt.Rows.Count > 0)
                    {
                        objPageCmn.FillData((object)gvDeliveryVoucher, dt, "", "");
                        totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

                    }
                    else
                    {
                        gvDeliveryVoucher.DataSource = null;
                        gvDeliveryVoucher.DataBind();
                        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
                    }

                    PageControlCommon.PopulatePager((Repeater)rptPager, totalRows, currentPageIndex);
                }
            }
            else
            {
                using (DataTable dt = objVoucherHeader.getVouhcerList((currentPageIndex - 1).ToString(), int.Parse(Session["GridSize"].ToString()).ToString(), gvDeliveryVoucher.Attributes["CurrentSortField"], gvDeliveryVoucher.Attributes["CurrentSortDirection"], strWhereClause))
                {
                    if (dt.Rows.Count > 0)
                    {
                        objPageCmn.FillData((object)gvDeliveryVoucher, dt, "", "");
                        totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

                    }
                    else
                    {
                        gvDeliveryVoucher.DataSource = null;
                        gvDeliveryVoucher.DataBind();
                        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
                    }

                    PageControlCommon.PopulatePager((Repeater)rptPager, totalRows, currentPageIndex);
                }
            }
        }
        catch(Exception ex)
        {

        }

    }


    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvDeliveryVoucherCurrentPageIndex.Value = pageIndex.ToString();
        FillGrid(pageIndex);
    }
    protected void PageRequest_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnPendingorderpageindex.Value = pageIndex.ToString();
        FillGridPendingOrder(pageIndex);
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
    protected void SetCustomerTextBox(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.Text == "Voucher_Date")
        {
            txtValue.Visible = false;

            txtValueDate.Visible = true;
        }
        else
        {
            txtValue.Visible = true;

            txtValueDate.Visible = false;
        }
        txtValue.Text = "";
        txtValueDate.Text = "";
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Voucher_Date")
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
        FillGrid(1);
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void gvDeliveryVoucher_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;

        if (gvDeliveryVoucher.Attributes["CurrentSortField"] != null &&
            gvDeliveryVoucher.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == gvDeliveryVoucher.Attributes["CurrentSortField"])
            {
                if (gvDeliveryVoucher.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        gvDeliveryVoucher.Attributes["CurrentSortField"] = sortField;
        gvDeliveryVoucher.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGrid();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        FillGrid();
    }
    #endregion
    #region Gridoperation

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, e);
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Session["dtFinal"] = null;
        string objSenderID;
        if (sender is Button)
        {
            Button b = (Button)sender;
            objSenderID = b.ID;
        }
        else
        {
            LinkButton b = (LinkButton)sender;
            objSenderID = b.ID;
        }

        DataTable dtInvEdit = new DataTable();

        if (Request.QueryString["LocId"] != null)
        {
            dtInvEdit = objVoucherHeader.GetAllRecord_ByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Request.QueryString["LocId"].ToString(), e.CommandArgument.ToString());
        }
        else
        {
            dtInvEdit = objVoucherHeader.GetAllRecord_ByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        }

        if (dtInvEdit.Rows.Count > 0)
        {
            if (objSenderID != "lnkViewDetail")
            {
                if (Convert.ToBoolean(dtInvEdit.Rows[0]["Post"]))
                {
                    DisplayMessage("Delivery Voucher has posted , can not be Update");

                    return;
                }
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                btnPost.Enabled = true;
                btnSInvSave.Enabled = true;
                BtnReset.Visible = true;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else
            {
                btnPost.Enabled = false;
                btnSInvSave.Enabled = false;
                BtnReset.Visible = false;

                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            editid.Value = e.CommandArgument.ToString();

            txtVoucherNo.Text = dtInvEdit.Rows[0]["Voucher_No"].ToString();
            txtVoucherDate.Text = Convert.ToDateTime(dtInvEdit.Rows[0]["Voucher_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtVoucherNo.Enabled = false;
            txtCustomer.Enabled = false;

            string strCustomerId = dtInvEdit.Rows[0]["Customer_Id"].ToString();
            DataTable dtCustomerName = objDa.return_DataTable("select * from Ems_ContactMaster where Trans_Id='" + strCustomerId + "'");
            if (dtCustomerName != null)
            {
                string strCustomerEmail = dtCustomerName.Rows[0]["Field1"].ToString();
                string strCustomerNumber = dtCustomerName.Rows[0]["Field2"].ToString();
                txtCustomer.Text = dtInvEdit.Rows[0]["CustomerName"].ToString() + "/" + strCustomerNumber + "/" + strCustomerEmail + "/" + strCustomerId;
                //txtCustomer.Text = dtInvEdit.Rows[0]["CustomerName"].ToString() + "/" + dtInvEdit.Rows[0]["Customer_Id"].ToString();
            }
                        
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dtInvEdit.Rows[0]["Salesperson_Id"].ToString());
            txtSalesPerson.Text = dtInvEdit.Rows[0]["Emp_Name"].ToString() + "/" + Emp_Code;
            txtRemarks.Text = dtInvEdit.Rows[0]["Remarks"].ToString();

            DataTable dtVoucherDetail = new DataTable();

            if (Request.QueryString["LocId"] != null)
            {
                dtVoucherDetail = objVoucherDetail.GetAllRecord_ByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Request.QueryString["LocId"].ToString(), e.CommandArgument.ToString(),Session["FinanceYearId"].ToString());
            }
            else
            {
                dtVoucherDetail = objVoucherDetail.GetAllRecord_ByVoucherNo(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), Session["FinanceYearId"].ToString());
            }


            gvProduct.DataSource = dtVoucherDetail;
            gvProduct.DataBind();
            Session["dtPo"] = dtVoucherDetail;
            //for fill search  grid
            if (objSenderID != "lnkViewDetail")
            {
                DataTable dtsalesorder = fillSOSearhgrid();
                if (dtsalesorder.Rows.Count != 0)
                {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                    objPageCmn.FillData((object)gvSerachGrid, dtsalesorder, "", "");
                    Session["Dtproduct"] = dtsalesorder;

                }
            }

            //for get serial number

            if (Session["dtFinal"] == null)
            {
                ucProductSno.initialiseSerialTbl();
            }
            DataTable dtSerial = (DataTable)Session["dtFinal"];
            DataTable dtStock = new DataTable();
            if (Request.QueryString["LocId"] != null)
            {
                dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Request.QueryString["LocId"].ToString(), "DV", editid.Value);
            }
            else
            {
                dtStock = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "DV", editid.Value);
            }
            if (dtStock.Rows.Count > 0)
            {
                for (int i = 0; i < dtStock.Rows.Count; i++)
                {
                    DataRow dr = dtSerial.NewRow();
                    dr["Product_Id"] = dtStock.Rows[i]["ProductId"].ToString();
                    dr["SerialNo"] = dtStock.Rows[i]["SerialNo"].ToString();
                    dr["SOrderNo"] = dtStock.Rows[i]["Field1"].ToString();
                    dr["BarcodeNo"] = dtStock.Rows[i]["Barcode"].ToString(); ;
                    dr["BatchNo"] = dtStock.Rows[i]["BatchNo"].ToString(); ;
                    dr["LotNo"] = dtStock.Rows[i]["LotNo"].ToString(); ;
                    dr["ExpiryDate"] = Convert.ToDateTime(dtStock.Rows[i]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    dr["ManufacturerDate"] = Convert.ToDateTime(dtStock.Rows[i]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    dtSerial.Rows.Add(dr);
                }
            }
            Session["dtFinal"] = dtSerial;
            dtStock.Dispose();
        }
        dtInvEdit.Dispose();
        Update_New.Update();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        using (DataTable dtInvEdit = objVoucherHeader.GetAllRecord_ByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString()))
        {
            if (dtInvEdit.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dtInvEdit.Rows[0]["Post"]))
                {
                    DisplayMessage("Delivery Voucher has posted , can not be delete");

                    return;
                }
            }
        }

        int b = objVoucherHeader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            string strSql = string.Empty;

            strSql = "delete from Inv_StockBatchMaster where TransType='DV' and TransTypeId=" + e.CommandArgument.ToString() + "";
            objDa.execute_Command(strSql);
            DisplayMessage("Record deleted");
            FillGrid();
        }
    }
    #endregion
    #region Post
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
    #endregion
    #region stockable with serial number
    protected void lnkAddSO_Click(object sender, EventArgs e)
    {
        Session["RdoType"] = "SerialNo";
        GridViewRow Row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        ViewState["RowIndex"] = Row.RowIndex;
        ViewState["SOrderId"] = ((Label)Row.FindControl("lblSOId")).Text;
        HiddenField HdnProductId = (HiddenField)Row.FindControl("hdngvProductId");
        ucProductSno.setValues("DV", "gvProduct", Row.RowIndex);
        ViewState["PID"] = HdnProductId.Value;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Popup()", true);
    }
    #endregion
    #region AutoCompleteMethod
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListOrderNo(string prefixText, int count, string contextKey)
    {
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            using (DataTable dt = objDa.return_DataTable("select SalesOrderNo from Inv_SalesOrderHeader where Location_Id=" + HttpContext.Current.Session["LocId"].ToString() + " and IsActive='True' and salesorderno like '%" + prefixText + "%'"))
            {
                string[] str = new string[dt.Rows.Count];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["SalesOrderNo"].ToString();
                    }
                }

                return str;
            }
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            using (DataTable dtCustomer = objcustomer.GetCustomerAsPerFilterText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText))
            {
                string[] filterlist = new string[dtCustomer.Rows.Count];
                if (dtCustomer.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCustomer.Rows.Count; i++)
                    {
                        filterlist[i] = dtCustomer.Rows[i]["Filtertext"].ToString();
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            using (DataTable dt = ObjEmployeeMaster.GetEmployeeDTListByPrefixText(HttpContext.Current.Session["CompId"].ToString(), prefixText))
            {
                string[] txt = new string[dt.Rows.Count];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["Emp_Code"].ToString() + "";
                    }
                }
                return txt;
            }
        }
        catch
        {
            return null;
        }
    }
    #endregion
    #region PendingOrder
    protected void ddlQSeleclField_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "SalesOrderDate")
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
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlQSeleclField.SelectedItem.Value == "SalesOrderDate")
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
                txtQValueDate.Focus();
            }
            else
            {
                DisplayMessage("Enter Date");
                txtQValueDate.Focus();
                txtQValue.Text = "";
                return;
            }
        }

        FillGridPendingOrder();
    }
    protected void ImgBtnQRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ddlQSeleclField.SelectedIndex = 1;
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

        if (gvSalesOrder.Attributes["CurrentSortField"] != null &&
            gvSalesOrder.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == gvSalesOrder.Attributes["CurrentSortField"])
            {
                if (gvSalesOrder.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        gvSalesOrder.Attributes["CurrentSortField"] = sortField;
        gvSalesOrder.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        FillGridPendingOrder();

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

        strWhereClause = "Location_id='" + Session["LocId"].ToString() + "' and IsActive='True'";


        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }

        //strWhereClause = strWhereClause.Substring(0, 4) == " and" ? strWhereClause.Substring(4, strWhereClause.Length - 4) : strWhereClause;
        int totalRows = 0;
        using (DataTable dt = objVoucherHeader.getPendingDeliveryVoucherList((currentPageIndex - 1).ToString(), int.Parse(Session["GridSize"].ToString()).ToString(), gvSalesOrder.Attributes["CurrentSortField"], gvSalesOrder.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvSalesOrder, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";

            }
            else
            {
                gvSalesOrder.DataSource = null;
                gvSalesOrder.DataBind();
                lblQTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager((Repeater)rptRequestPager, totalRows, currentPageIndex);
        }

    }

    protected void btnPendingOrder_Click(object sender, EventArgs e)
    {
        FillGridPendingOrder();
    }
    public string SetDecimal(string amount)
    {
        return SystemParameter.GetAmountWithDecimal(amount, Session["LoginLocDecimalCount"].ToString());
    }
    //for child grid
    #endregion
    #region Scanningsolution
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_Id", typeof(int));
        dt.Columns.Add("SalesOrderNo");
        dt.Columns.Add("SoID");
        dt.Columns.Add("Serial_No", typeof(int));
        dt.Columns.Add("Product_Id");
        dt.Columns.Add("ProductName");
        dt.Columns.Add("UnitId");
        dt.Columns.Add("Unit_Name");
        dt.Columns.Add("SysQty");
        dt.Columns.Add("OrderQty");
        dt.Columns.Add("SoldQty");
        dt.Columns.Add("RemainQty");
        dt.Columns.Add("Delievered_Qty");

        return dt;
    }
    public DataTable SavedGridRecordindatatble()
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in gvProduct.Rows)
        {
            DataRow dr = dt.NewRow();
            Label lblgvSNo = (Label)gvRow.FindControl("lblgvSerialNo");
            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            Label lblgvProductName = (Label)gvRow.FindControl("lblgvProductName");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            Label lblgvUnitName = (Label)gvRow.FindControl("lblgvUnit");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");

            if (((TextBox)gvRow.FindControl("txtgvSalesQuantity")).Text == "")
            {
                ((TextBox)gvRow.FindControl("txtgvSalesQuantity")).Text = "0";
            }


            dr["Trans_Id"] = lblTransId.Text;
            dr["SalesOrderNo"] = ((Label)gvRow.FindControl("lblSONo")).Text;
            dr["SoID"] = ((Label)gvRow.FindControl("lblSOId")).Text;
            dr["Serial_No"] = lblgvSNo.Text;
            dr["Product_Id"] = hdngvProductId.Value;
            dr["ProductName"] = lblgvProductName.Text;

            dr["UnitId"] = hdngvUnitId.Value;
            dr["Unit_Name"] = lblgvUnitName.Text;
            dr["SysQty"] = SetDecimal(((Label)gvRow.FindControl("lblgvSystemQuantity")).Text);
            dr["OrderQty"] = SetDecimal(((Label)gvRow.FindControl("lblgvOrderqty")).Text);
            dr["SoldQty"] = SetDecimal(((Label)gvRow.FindControl("lblgvsoldQuantity")).Text);
            dr["RemainQty"] = SetDecimal(((Label)gvRow.FindControl("lblgvRemaningQuantity")).Text);
            dr["Delievered_Qty"] = SetDecimal(((TextBox)gvRow.FindControl("txtgvSalesQuantity")).Text);
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void btnscanserial_OnClick(object sender, EventArgs e)
    {
        DisableOrderList();
        bool Isserial = false;

        int counter = 0;
        if (gvSerachGrid.Rows.Count == 0)
        {
            DisplayMessage("Product Not Found");
            txtscanProduct.Text = "";
            txtscanProduct.Focus();
            return;
        }
        if (txtscanProduct.Text != "")
        {
            DataTable dt = new DataTable();
            dt = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtscanProduct.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {


                //heer we checking that scaned text is serial number or not
                //code start
                if (dt.Rows[0]["Type"].ToString() == "2")
                {

                    Isserial = true;
                }


                //here checking if serial based product then valid or not
                if (Isserial)
                {
                    if (!Inventory_Common.CheckValidSerialForSalesinvoice(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text))
                    {
                        DisplayMessage("Serial number is invalid");
                        txtscanProduct.Text = "";
                        txtscanProduct.Focus();
                        return;
                    }

                    if (Session["dtFinal"] != null)
                    {
                        if (new DataView((DataTable)Session["dtFinal"], "SerialNo='" + txtscanProduct.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                        {
                            DisplayMessage("Serial number is already exists");
                            txtscanProduct.Text = "";
                            txtscanProduct.Focus();
                            return;

                        }
                    }
                }

                //code end

                if (gvProduct.Rows.Count > 0)
                {
                    if (counter == 0)
                    {
                        DataTable DtProduct = SavedGridRecordindatatble();

                        for (int i = 0; i < DtProduct.Rows.Count; i++)
                        {
                            if (DtProduct.Rows[i]["Product_Id"].ToString().Trim() == dt.Rows[0]["ProductId"].ToString().Trim())
                            {
                                DtProduct.Rows[i]["Delievered_Qty"] = (float.Parse(DtProduct.Rows[i]["Delievered_Qty"].ToString()) + 1).ToString();
                                if (Isserial)
                                {
                                    addSerialfnc(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text.Trim(), DtProduct.Rows[i]["SoID"].ToString());
                                }
                                counter = 1;
                                break;
                            }
                        }

                        if (counter == 1)
                        {

                            Session["dtPo"] = DtProduct;
                            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                            objPageCmn.FillData((object)gvProduct, DtProduct, "", "");
                            txtscanProduct.Focus();
                            txtscanProduct.Text = "";

                        }

                    }
                }
                if (counter == 0)
                {
                    int itemcount = 0;
                    //here we checking that item exist or no tin order list iif we foudn multiple product then enable the check box and if found single then directly add in listt

                    foreach (GridViewRow gvrow in gvSerachGrid.Rows)
                    {
                        if (((Label)gvrow.FindControl("lblproductcode")).Text == dt.Rows[0]["ProductCode"].ToString())
                        {
                            itemcount++;
                        }
                    }


                    if (itemcount > 0)
                    {

                        foreach (GridViewRow gvrow in gvSerachGrid.Rows)
                        {
                            if (itemcount == 1)
                            {
                                if (((Label)gvrow.FindControl("lblproductcode")).Text == dt.Rows[0]["ProductCode"].ToString())
                                {
                                    chkTrandId_CheckedChanged(((object)((CheckBox)gvrow.FindControl("chkTrandId"))), e);
                                    if (Isserial)
                                    {
                                        addSerialfnc(dt.Rows[0]["ProductId"].ToString(), txtscanProduct.Text.Trim(), ((Label)gvrow.FindControl("lblsoid")).Text);
                                    }

                                    counter = 1;
                                    txtscanProduct.Focus();
                                    txtscanProduct.Text = "";
                                    break;
                                }
                            }
                            else
                            {
                                if (((Label)gvrow.FindControl("lblproductcode")).Text == dt.Rows[0]["ProductCode"].ToString())
                                {

                                    ((CheckBox)gvrow.FindControl("chkTrandId")).Enabled = true;
                                    counter = 1;
                                }
                            }
                        }

                    }



                }



            }


            if (counter == 0)
            {
                DisplayMessage("Product Not Found");
                txtscanProduct.Focus();
                txtscanProduct.Text = "";

            }
        }


    }
    public void DisableOrderList()
    {
        using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDeliveryScanning"))
        {
            if (Dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                {
                    foreach (GridViewRow gvrow in gvSerachGrid.Rows)
                    {
                        ((CheckBox)gvrow.FindControl("chkTrandId")).Enabled = false;
                    }
                }
            }
        }
    }
    public void addSerialfnc(string strProductId, string strSerialNumber, string strOrderId)
    {
        DataTable dt = new DataTable();


        if (Session["dtFinal"] == null)
        {
            dt.Columns.Add("Product_Id");
            dt.Columns.Add("SerialNo");
            dt.Columns.Add("SOrderNo");
            dt.Columns.Add("BarcodeNo");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("LotNo");
            dt.Columns.Add("ExpiryDate");
            dt.Columns.Add("ManufacturerDate");
        }
        else
        {
            dt = (DataTable)Session["dtFinal"];
        }

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);
        dr[0] = strProductId;
        dr[1] = strSerialNumber;
        dr[2] = strOrderId;
        //for batch number
        dr[4] = "";

        dr[5] = "0";
        //for expiry date
        dr[6] = DateTime.Now.ToString();
        //for Manufacturer date
        dr[7] = DateTime.Now.ToString();

        Session["dtFinal"] = dt;
    }
    protected void gvSerachGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            using (DataTable Dt = objInvParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "IsDeliveryScanning"))
            {
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(Dt.Rows[0]["ParameterValue"]) == true)
                    {
                        ((CheckBox)e.Row.FindControl("chkTrandId")).Enabled = false;
                    }
                }
            }
        }
    }
    #endregion
}

