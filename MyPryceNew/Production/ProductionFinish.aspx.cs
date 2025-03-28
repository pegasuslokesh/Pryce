using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.IO;
using System.Data.SqlClient;

public partial class Production_ProductionFinish : System.Web.UI.Page
{
    #region defined Class Object
    Common cmn = null;
    Production_Process_Detail Objproductiondetail = null;
    Ac_ChartOfAccount objCOA = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Production_Process objProductionProcess = null;
    Production_BOM objProductionBom = null;
    Production_Employee objProductionEmployee = null;
    Ems_GroupMaster objGroupMaster = null;
    Inv_ProductionRequestDetail ObjProductionRequestDetail = null;
    Inv_ProductionRequestHeader ObjProductionReqestHeader = null;
    CountryMaster ObjCountry = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    SystemParameter ObjSysParam = null;
    Ems_ContactMaster objContact = null;
    LocationMaster objLocation = null;
    Inv_StockDetail objStockDetail = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    EmployeeMaster objEmployee = null;
    Inv_ParameterMaster objInvParameter = null;
    Inv_StockDetail objStock = null;
    DataAccessClass objDa = null;
    Inv_ShipExpMaster ObjShipExp = null;
    Inv_ShipExpDetail ObjShipExpDetail = null;
    Inv_ProductLedger ObjProductledger = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Ac_Finance_Year_Info objFYI = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    Ac_ParameterMaster objAcParameter = null;
    Inv_TransferRequestHeader ObjTrans = null;
    Inv_TransferRequestDetail OBjtransDetail = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        Objproductiondetail = new Production_Process_Detail(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objProductionProcess = new Production_Process(Session["DBConnection"].ToString());
        objProductionBom = new Production_BOM(Session["DBConnection"].ToString());
        objProductionEmployee = new Production_Employee(Session["DBConnection"].ToString());
        objGroupMaster = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjProductionRequestDetail = new Inv_ProductionRequestDetail(Session["DBConnection"].ToString());
        ObjProductionReqestHeader = new Inv_ProductionRequestHeader(Session["DBConnection"].ToString());
        ObjCountry = new CountryMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objInvParameter = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        objStock = new Inv_StockDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjShipExp = new Inv_ShipExpMaster(Session["DBConnection"].ToString());
        ObjShipExpDetail = new Inv_ShipExpDetail(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        objFYI = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        ObjTrans = new Inv_TransferRequestHeader(Session["DBConnection"].ToString());
        OBjtransDetail = new Inv_TransferRequestDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Production/ProductionFinish.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtSerialNo.Attributes["onkeydown"] = String.Format("count('{0}')", txtSerialNo.ClientID);
            Session["dtSerial"] = null;
            Session["dtFinal"] = null;
            Session["dtExpList"] = null;
            Session["dtIssueRoll"] = null;
            //LoadExpensesRecord();
            Session["DtBOM"] = null;
            Session["dtVisitTaskList"] = null;
            LoadVisitTask();
            ddlOption.SelectedIndex = 2;
            FillGrid();
            Session["DtDetail"] = null;
            txtPINo.Text = GetDocumentNum();
            ViewState["DocNo"] = txtPINo.Text;
            CalendarExtendertxtValueBinDate.Format = Session["DateFormat"].ToString();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtender1.Format = Session["DateFormat"].ToString();
            CalendarExtender2.Format = Session["DateFormat"].ToString();
            BtnReset_Click(null, null);
            Session["DtSearchProduct"] = null;
            ddlFieldName.Focus();
            FillRequestLocation();
            fillCurrency();
            Session["Expdt"] = null;
            fillExpenses();
        }

        if (hdnReportId.Value != "")
        {
            BarcodeReport();
        }


        //AllPageCode();
    }
    public void FillRequestLocation()
    {

        DataTable dtLocation = objLocation.GetLocationMaster(HttpContext.Current.Session["CompId"].ToString());
        dtLocation = new DataView(dtLocation, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();

        objPageCmn.FillData((object)ddlLocation, dtLocation, "Location_Name", "Location_Id");
        ddlLocation.Items.RemoveAt(0);
        ddlLocation.SelectedValue = Session["LocId"].ToString();
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region System defined Function

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        LinkButton b = (LinkButton)sender;
        string objSenderID = b.ID;


        if (objSenderID == "lnkViewDetail")
        {
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        else
        {
            if (e.CommandName.ToString() == "True")
            {
                DisplayMessage("Record posted , you can not edit !");
                return;
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }

        if (Lbl_Tab_New.Text == "View")
        {
            btnPISave.Visible = false;
            BtnReset.Visible = false;
        }
        else
        {

            BtnReset.Visible = true;
        }
        editid.Value = e.CommandArgument.ToString();

        DataTable dtProcess = objProductionProcess.GetRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        if (dtProcess.Rows.Count > 0)
        {
            txtPINo.Text = dtProcess.Rows[0]["Job_No"].ToString();
            hdnrequestid.Value = dtProcess.Rows[0]["Ref_Production_Req_No"].ToString();
            txtCustomer.Text = dtProcess.Rows[0]["Customername"].ToString();
            txtRequestNo.Text = dtProcess.Rows[0]["Request_No"].ToString();
            txtRequestDate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Request_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtSONo.Text = dtProcess.Rows[0]["Order_No"].ToString();
            if (dtProcess.Rows[0]["Order_Date"].ToString() != "1/1/1900 12:00:00 AM")
            {
                txtSODate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Order_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            hdnOrderId.Value = dtProcess.Rows[0]["SalesOrderId"].ToString();
            txtjobcreationdate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Job_Creation_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtjobstartdate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Job_Start"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtexpjobenddate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Exp_Job_End"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtDescription.Text = dtProcess.Rows[0]["Remarks"].ToString();
            chkCancel.Checked = Convert.ToBoolean(dtProcess.Rows[0]["Is_Cancel"].ToString());
            if (dtProcess.Rows[0]["Job_End"].ToString() != "1/1/1900 12:00:00 AM")
            {
                txtjobenddate.Text = Convert.ToDateTime(dtProcess.Rows[0]["Job_End"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            ddlLocation.SelectedValue = dtProcess.Rows[0]["Req_For_Material"].ToString();

            ChkisQualitycheck.Checked = Convert.ToBoolean(dtProcess.Rows[0]["Field6"].ToString());

            //get record from bom table
            DataTable dtBom = objProductionBom.GetRecord_By_RefJobNo(editid.Value, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


            //for return grdiview
            //objPageCmn.FillData((object)gvReturn, dtBom, "", "");

            try
            {
                dtBom = dtBom.DefaultView.ToTable(true, "Id", "Item_Id", "Unit_Id", "Sys_qty", "Unit_Price", "Req_Qty", "wst_Qty", "Rec_Qty", "Line_Total", "ShortDescription");
            }
            catch
            {
            }
            Session["DtBOM"] = dtBom;
            objPageCmn.FillData((object)gvBom, dtBom, "", "");


            string strProductId = string.Empty;
            foreach (GridViewRow gvbon in gvBom.Rows)
            {
                if (((HiddenField)gvbon.FindControl("hdngvProductId")).Value != "0")
                {
                    if (strProductId.Trim() == "")
                    {
                        strProductId = ((HiddenField)gvbon.FindControl("hdngvProductId")).Value;
                    }
                    else
                    {
                        strProductId = strProductId + "," + ((HiddenField)gvbon.FindControl("hdngvProductId")).Value;
                    }
                }
            }

            if (strProductId.Trim() != "")
            {

                Session["MaterialProductId"] = strProductId;
            }
            //get record from production employee

            DataTable dtEmployee = objProductionEmployee.GetRecord_By_RefJobNo(editid.Value);
            try
            {
                dtEmployee = dtEmployee.DefaultView.ToTable(true, "Id", "Emp_Name", "Start_Time", "End_Time", "Duration");
            }
            catch
            {
            }
            if (dtEmployee.Rows.Count > 0)
            {
                Session["dtVisitTaskList"] = dtEmployee;
                objPageCmn.FillData((object)gvVisitTask, dtEmployee, "", "");
            }
            //get request detail
            DataTable dtrequestDetail = Objproductiondetail.GetRecord_By_RefJobNo(e.CommandArgument.ToString());

            objPageCmn.FillData((object)GvProduct, dtrequestDetail, "", "");

            //this code for get Expenses information
            fillExpGrid(ObjShipExpDetail.Get_ShipExpDetailByInvoiceId(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), e.CommandArgument.ToString(), "PF"));


            TabContainer2.ActiveTabIndex = 0;

            //for get stockbatch master record

            DataTable dtTemp = ObjStockBatchMaster.GetStockBatchMaster_By_TRansType_and_TransId(Session["Compid"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "FO", e.CommandArgument.ToString());
            if (dtTemp.Rows.Count > 0)
            {
                dtTemp = new DataView(dtTemp, "TransType='FO' and TransTypeId=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTemp.Rows.Count > 0)
                {

                    dtTemp = dtTemp.DefaultView.ToTable(false, "Trans_Id", "ProductId", "UnitId", "SerialNo", "Quantity", "Width", "Length", "Pallet_ID", "Field4", "Field5", "TransType");
                    dtTemp.Columns["Field5"].ColumnName = "TotalQuantity";
                    dtTemp.Columns["Quantity"].ColumnName = "UsedQuantity";
                    dtTemp.Columns["Field4"].ColumnName = "WasteQuantity";

                    Session["dtIssueRoll"] = dtTemp;
                    objPageCmn.FillData((Object)gvReturn, dtTemp, "", "");
                }
            }
            getGridTotal();
        }

        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);

    }
    protected void GvProductionProcess_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvProductionProcess.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtPInquiry"];

        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcess, dt, "", "");
        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "Job_Creation_Date" || ddlFieldName.SelectedItem.Value == "Request_Date")
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

            DataTable dtPurchaseInquiry = (DataTable)Session["dtPInquiry"];
            DataView view = new DataView(dtPurchaseInquiry, condition, "", DataViewRowState.CurrentRows);

            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcess, view.ToTable(), "", "");

            Session["dtPInquiry"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            //AllPageCode();
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvProductionProcess_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtPInquiry"];
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
        Session["dtPInquiry"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcess, dt, "", "");
        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName.ToString() == "True")
        {
            DisplayMessage("Record posted , you can not delete !");
            return;
        }
        int b = 0;
        b = objProductionProcess.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillGridBin(); //Update grid view in bin tab
        FillGrid();
        Reset();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueDate.Text = "";
        txtValue.Focus();
    }
    protected void btnPICancel_Click(object sender, EventArgs e)
    {
        Reset();
        FillGridBin();
        FillGrid();
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        editid.Value = "";
        Reset();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtPINo);
    }
    protected void btnPost_Click(object sender, EventArgs e)
    {
        if (Inventory_Common.GetPhyscialInventoryStatus(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Physical Inventory work is under processing ,you can not post.");
            return;
        }


        foreach (GridViewRow gvrow in gvBom.Rows)
        {
            if (((TextBox)gvrow.FindControl("txtReceivedqty")).Text == "")
            {
                ((TextBox)gvrow.FindControl("txtReceivedqty")).Text = "0";
            }

            if (((Label)gvrow.FindControl("lblRequestquantity")).Text == "")
            {
                ((Label)gvrow.FindControl("lblRequestquantity")).Text = "0";
            }

            if (((TextBox)gvrow.FindControl("txtwasteqty")).Text == "")
            {
                ((TextBox)gvrow.FindControl("txtwasteqty")).Text = "0";
            }

            if (((HiddenField)gvrow.FindControl("hdngvProductId")).Value.Trim() != "0")
            {
                if ((float.Parse(((TextBox)gvrow.FindControl("txtwasteqty")).Text) + float.Parse(((TextBox)gvrow.FindControl("txtReceivedqty")).Text)) < float.Parse(((Label)gvrow.FindControl("lblRequestquantity")).Text))
                {
                    DisplayMessage("Received quantity and waste quantity sum should be equal or gerater then Request quantity");
                    TabContainer2.ActiveTabIndex = 1;
                    return;
                }
            }
           
        }

        btnPISave_Click(sender, e);
    }
    protected void btnPISave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());


        if (((Button)sender).ID.Trim() == "btnPost")
        {
            chkpost.Checked = true;
        }
        else
        {
            chkpost.Checked = false;
        }

        string strSql = string.Empty;

        if (txtRequestNo.Text == "")
        {
            DisplayMessage("Request no. not found");

            return;
        }
        if (txtPINo.Text == "")
        {
            DisplayMessage("Enter Voucher No.");
            txtPINo.Focus();

            return;
        }

        if (txtjobcreationdate.Text == "")
        {
            DisplayMessage("Enter Job creation date");
            txtjobcreationdate.Focus();

            return;
        }

        //code added by jitendra upadhyay on 09-12-2016
        //for insert record according the log in financial year

        if (!Common.IsFinancialyearAllow(Convert.ToDateTime(txtjobcreationdate.Text), "I", Session["DBConnection"].ToString(), HttpContext.Current.Session["FinanceTodate"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString()))
        {
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }




        if (txtexpjobenddate.Text == "")
        {
            DisplayMessage("Enter expected job end date");
            txtexpjobenddate.Focus();

            return;
        }


        if (GvProduct.Rows.Count == 0)
        {
            DisplayMessage("Product not found");

            return;
        }



        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            string strsql = string.Empty;

            //for update unit cost in request detail table
            foreach (GridViewRow gvRow in GvProduct.Rows)
            {
                strsql = "update dbo.Inv_ProductionRequestDetail set Field1='" + ((Label)gvRow.FindControl("lblunitCost")).Text + "' where Request_No=" + hdnrequestid.Value + " and ProductId=" + ((Label)gvRow.FindControl("lblPID")).Text + "";
                objDa.execute_Command(strsql, ref trns);

                //here we in stock of requested item on current location
                if (((Label)gvRow.FindControl("lblReqQty")).Text == "")
                {
                    ((Label)gvRow.FindControl("lblReqQty")).Text = "0";
                }

                if (((TextBox)gvRow.FindControl("txtExtraQty")).Text == "")
                {
                    ((TextBox)gvRow.FindControl("txtExtraQty")).Text = "0";
                }

                objDa.execute_Command("update Inv_Production_Process_Detail set Extra_Qty=" + ((TextBox)gvRow.FindControl("txtExtraQty")).Text + " where Ref_Job_No=" + editid.Value.ToString() + " and ProductId=" + ((Label)gvRow.FindControl("lblPID")).Text + "", ref trns);

                if (chkpost.Checked)
                {
                    //out stock for used item in bom
                    if ((float.Parse(((Label)gvRow.FindControl("lblReqQty")).Text.Trim()) + float.Parse(((TextBox)gvRow.FindControl("txtExtraQty")).Text.Trim())) > 0)
                    {
                        ObjProductledger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "FI", editid.Value.ToString(), "0", ((Label)gvRow.FindControl("lblPID")).Text, ((Label)gvRow.FindControl("lblUID")).Text.Trim(), "I", "0", "0", (float.Parse(((Label)gvRow.FindControl("lblReqQty")).Text.Trim()) + float.Parse(((TextBox)gvRow.FindControl("txtExtraQty")).Text.Trim())).ToString(), "0", "1/1/1800", ((Label)gvRow.FindControl("lblunitCost")).Text.Trim(), "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(txtjobcreationdate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);

                    }
                }

                if (Session["dtFinal"] != null)
                {
                    DataTable dt = (DataTable)Session["dtFinal"];
                    ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "FI", editid.Value, ((Label)gvRow.FindControl("lblPID")).Text, ref trns);
                    dt = new DataView(dt, "ProductId='" + ((Label)gvRow.FindControl("lblPID")).Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    foreach (DataRow dr in dt.Rows)
                    {

                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString(), "FI", editid.Value, ((Label)gvRow.FindControl("lblPID")).Text, ((Label)gvRow.FindControl("lblUID")).Text, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), dr["BarcodeNo"].ToString(), "", "", "", dr["POId"].ToString(), "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }

            }


            //update expected delivery date in sales order page from production 
            strsql = "update Inv_SalesOrderHeader set EstimateDeliveryDate ='" + ObjSysParam.getDateForInput(txtexpjobenddate.Text).ToString() + "' where Trans_Id=" + hdnOrderId.Value + "";
            objDa.execute_Command(strsql, ref trns);


            //for delete exsting record
            //for production out and return
            ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "FO", editid.Value, "0", ref trns);
            ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "FR", editid.Value, "0", ref trns);


            //for received stock


            foreach (GridViewRow gvrow in gvBom.Rows)
            {
                if (((TextBox)gvrow.FindControl("txtReceivedqty")).Text.Trim() == "")
                {
                    ((TextBox)gvrow.FindControl("txtReceivedqty")).Text = "0";
                }

                if (((TextBox)gvrow.FindControl("txtwasteqty")).Text.Trim() == "")
                {
                    ((TextBox)gvrow.FindControl("txtwasteqty")).Text = "0";
                }

                string sql = string.Empty;
                sql = "update Inv_Production_BOM set rec_qty=" + ((TextBox)gvrow.FindControl("txtReceivedqty")).Text.Trim() + ",Wst_qty=" + ((TextBox)gvrow.FindControl("txtwasteqty")).Text.Trim() + " where Id=" + ((Label)gvrow.FindControl("lblTransId")).Text.Trim() + "";
                objDa.execute_Command(sql, ref trns);
                if (((HiddenField)gvrow.FindControl("hdngvProductId")).Value.Trim() == "0")
                {
                    continue;
                }

                if (chkpost.Checked)
                {
                    if ((float.Parse(((TextBox)gvrow.FindControl("txtReceivedqty")).Text.Trim())+ float.Parse(((TextBox)gvrow.FindControl("txtwasteqty")).Text.Trim())) > 0)
                    {


                        ObjProductledger.InsertProductLedger(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "FO", editid.Value.ToString(), "0", ((HiddenField)gvrow.FindControl("hdngvProductId")).Value.Trim(), ((HiddenField)gvrow.FindControl("hdngvUnitId")).Value.Trim(), "O", "0", "0", "0", (float.Parse(((TextBox)gvrow.FindControl("txtReceivedqty")).Text.Trim()) + float.Parse(((TextBox)gvrow.FindControl("txtwasteqty")).Text.Trim())).ToString(), "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), ObjSysParam.getDateForInput(Convert.ToDateTime(txtjobcreationdate.Text).ToString(HttpContext.Current.Session["DateFormat"].ToString())).ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), ref trns);


                    }
                }
            }

            ////for update return quantity in bom table 
            foreach (GridViewRow gvRow in gvReturn.Rows)
            {
                TextBox txtUsedquantity = (TextBox)gvRow.FindControl("txtusedqty");
                HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
                if (((Label)gvRow.FindControl("lblqty")).Text.Trim() == "")
                {
                    ((Label)gvRow.FindControl("lblqty")).Text = "0";
                }
                if (((TextBox)gvRow.FindControl("txtusedqty")).Text == "")
                {
                    ((TextBox)gvRow.FindControl("txtusedqty")).Text = "0";
                }
                if (float.Parse(((TextBox)gvRow.FindControl("txtusedqty")).Text.Trim()) > 0)
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString(), "FO", editid.Value, hdngvProductId.Value, ((HiddenField)gvRow.FindControl("hdngvUnitId")).Value.Trim(), "O", "", "", (float.Parse(((TextBox)gvRow.FindControl("txtusedqty")).Text.Trim()) + float.Parse(((TextBox)gvRow.FindControl("txtwasteqty")).Text.Trim())).ToString(), DateTime.Now.ToString(), ((Label)gvRow.FindControl("lblserialNo")).Text, DateTime.Now.ToString(), "0", ((Label)gvRow.FindControl("lblwidth")).Text, ((Label)gvRow.FindControl("lblLength")).Text, ((Label)gvRow.FindControl("lblPalletiD")).Text, "0", "", "", ((TextBox)gvRow.FindControl("txtwasteqty")).Text.Trim(), ((Label)gvRow.FindControl("lblqty")).Text.Trim(), true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }

                if ((float.Parse(((Label)gvRow.FindControl("lblqty")).Text.Trim()) - (float.Parse(((TextBox)gvRow.FindControl("txtusedqty")).Text.Trim()) + float.Parse(((TextBox)gvRow.FindControl("txtwasteqty")).Text.Trim()))) > 0)
                {
                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString(), "FR", editid.Value, hdngvProductId.Value, ((HiddenField)gvRow.FindControl("hdngvUnitId")).Value.Trim(), "I", "", "", (float.Parse(((Label)gvRow.FindControl("lblqty")).Text.Trim()) - (float.Parse(((TextBox)gvRow.FindControl("txtusedqty")).Text.Trim()) + float.Parse(((TextBox)gvRow.FindControl("txtwasteqty")).Text.Trim()))).ToString(), DateTime.Now.ToString(), ((Label)gvRow.FindControl("lblserialNo")).Text, DateTime.Now.ToString(), "0", ((Label)gvRow.FindControl("lblwidth")).Text, ((Label)gvRow.FindControl("lblLength")).Text, ((Label)gvRow.FindControl("lblPalletiD")).Text, "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
            }
            //this code for sav expenes
            //code start

            DataTable dtExpense = new DataTable();
            dtExpense = (DataTable)Session["Expdt"];
            if (dtExpense != null)
            {
                ObjShipExpDetail.ShipExpDetail_Delete(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value.ToString(), "PF", ref trns);

                try
                {
                    foreach (DataRow dr in ((DataTable)Session["Expdt"]).Rows)
                    {
                        ObjShipExpDetail.ShipExpDetail_Insert(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, dr["Expense_Id"].ToString(), dr["Account_No"].ToString(), dr["Exp_Charges"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "PF", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                catch
                {

                }
            }
            //code end





            //update is quanlity check flag and expected jon end date 

            strsql = "update Inv_Production_Process set Field6='" + ChkisQualitycheck.Checked.ToString() + "', Exp_Job_End ='" + ObjSysParam.getDateForInput(txtexpjobenddate.Text).ToString() + "'  where Id=" + editid.Value + "";
            objDa.execute_Command(strsql, ref trns);

            //for update is production finish flag when we post process
            //code start
            if (chkpost.Checked)
            {
                //For Finance Entry

                //string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "160", "304", "0");
                //objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), strFinanceCode, Session["LocId"].ToString().ToString(), "0", strVoucherNo, DateTime.Now.ToString(), "PI", "1/1/1800", "1/1/1800", "", "", Session["Currency"].ToString(), "0.00", "Production Finish Detail on Finish Number is '" + txtPINo + "'", false.ToString(), false.ToString(), "AV", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //string strMaxId = string.Empty;
                //DataTable dtMaxId = objVoucherHeader.GetVoucherHeaderMaxId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                //if (dtMaxId.Rows.Count > 0)
                //{
                //    strMaxId = dtMaxId.Rows[0][0].ToString();

                //    if (strVoucherNo != "")
                //    {
                //        DataTable dtCount = objVoucherHeader.GetVoucherAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                //        if (dtCount.Rows.Count == 0)
                //        {
                //            objVoucherHeader.Updatecode(strMaxId, strVoucherNo + "1");
                //        }
                //        else
                //        {
                //            objVoucherHeader.Updatecode(strMaxId, strVoucherNo + dtCount.Rows.Count);
                //        }
                //    }
                //}

                //int j = 0;
                //string strPayTotal = "0";
                //string strCashAccount = string.Empty;

                //DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(Session["CompId"].ToString());
                //DataTable dtCash = new DataView(dtAcParameter, "Param_Name='Cash Transaction'", "", DataViewRowState.CurrentRows).ToTable();
                //if (dtCash.Rows.Count > 0)
                //{
                //    strCashAccount = dtCash.Rows[0]["Param_Value"].ToString();
                //}

                ////for voucher detail entries
                //DataTable dtVExpense = new DataTable();
                //dtVExpense = (DataTable)Session["Expdt"];
                //if (dtVExpense != null)
                //{
                //    try
                //    {
                //        foreach (DataRow dr in ((DataTable)Session["Expdt"]).Rows)
                //        {
                //            //For Debit
                //            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), strMaxId, (j++).ToString(), dr["Account_No"].ToString(), "0", editid.Value, "PF", "1/1/1800", "1/1/1800", "", dr["Exp_Charges"].ToString(), "0.00", "Expenses Name in Production Finish Detail Number is '" + txtPINo.Text + "'", "", Session["EmpId"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                //            //For Credit
                //            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), strMaxId, (j++).ToString(), strCashAccount, "0", editid.Value, "PF", "1/1/1800", "1/1/1800", "", "0.00", dr["Exp_Charges"].ToString(), "Expenses Name in Production Finish Detail Number is '" + txtPINo.Text + "'", "", Session["EmpId"].ToString(), dr["ExpCurrencyID"].ToString(), dr["ExpExchangeRate"].ToString(), dr["FCExpAmount"].ToString(), "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                //        }
                //    }
                //    catch
                //    {

                //    }
                //}


                //for update job end date in production process header table
                strsql = "update Inv_Production_Process set Job_End='" + DateTime.Now.ToString() + "',Field2='True',Field6='" + ChkisQualitycheck.Checked.ToString() + "' where Id=" + editid.Value + "";
                objDa.execute_Command(strsql, ref trns);

                strSql = "update dbo.Inv_ProductionRequestHeader set Is_Production_Finish='True',ModifiedBy='" + Session["UserId"].ToString() + "',ModifiedDate='" + DateTime.Now.ToString() + "' where Trans_Id=" + hdnrequestid.Value + "";
                objDa.execute_Command(strSql, ref trns);



                //this code is created by jitendra upadhyay on 24-02-2016
                //for insert Transfer request  record when request came from another location
                //this is showing as Transfer out for current location


                //code start
                if (ddlLocation.SelectedValue != Session["LocId"].ToString())
                {

                    //get requst number for Transfer voucher 

                    int TrasferRequestId = ObjTrans.InsertTransferRequestHeader(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), ddlLocation.SelectedValue, "", DateTime.Now.ToString(), "For Sales Order - " + txtSONo.Text + " and From - " + ddlLocation.SelectedItem.Text, "Y", "0", Session["LocId"].ToString(), false.ToString(), false.ToString(), "0", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);

                    DataTable dtCount = ObjTrans.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, "0", ref trns);
                    if (dtCount.Rows.Count == 0)
                    {
                        ObjTrans.Updatecode(TrasferRequestId.ToString(), GetDocumentNo(true, Session["CompId"].ToString(), ddlLocation.SelectedValue, true, "11", "93", "0", ref trns) + "1", ref trns);

                    }
                    else
                    {
                        ObjTrans.Updatecode(TrasferRequestId.ToString(), GetDocumentNo(true, Session["CompId"].ToString(), ddlLocation.SelectedValue, true, "11", "93", "0", ref trns) + dtCount.Rows.Count, ref trns);
                    }

                    float counter = 0;
                    foreach (GridViewRow gvr in GvProduct.Rows)
                    {
                        counter++;
                        if (((Label)gvr.FindControl("lblReqQty")).Text == "")
                        {
                            ((Label)gvr.FindControl("lblReqQty")).Text = "0";
                        }
                        OBjtransDetail.InsertTransferRequestDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, counter.ToString(), ((Label)gvr.FindControl("lblPID")).Text, TrasferRequestId.ToString(), ((Label)gvr.FindControl("lblUID")).Text, "0", ((Label)gvr.FindControl("lblReqQty")).Text, "0", "0", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                    }
                }
                //code end
                DisplayMessage("Record has been posted");
            }
            else
            {
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }



            //code end
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid();
            btnPICancel_Click(null, null);
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
    protected void txtReceivedqty_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        if (((TextBox)gvrow.FindControl("txtReceivedqty")).Text == "")
        {
            ((TextBox)gvrow.FindControl("txtReceivedqty")).Text = "0";
        }

        if (((Label)gvrow.FindControl("lblgvSystemQuantity")).Text == "")
        {
            ((Label)gvrow.FindControl("lblgvSystemQuantity")).Text = "0";
        }
        int product_id = 0;
        int.TryParse(((HiddenField)gvrow.FindControl("hdngvProductId")).Value, out product_id);
        if (product_id > 0)
        {
            if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((HiddenField)gvrow.FindControl("hdngvProductId")).Value, Session["FinanceYearId"].ToString()).Rows[0]["ItemType"].ToString() == "S")
            {
                if (float.Parse(((TextBox)gvrow.FindControl("txtReceivedqty")).Text) > float.Parse(((Label)gvrow.FindControl("lblgvSystemQuantity")).Text))
                {
                    DisplayMessage("Receive quantity should be equal or less then System quantity");
                    TabContainer2.ActiveTabIndex = 1;
                    ((TextBox)gvrow.FindControl("txtReceivedqty")).Text = "0";
                    return;
                }
            }
        }

    }
    public string GetDocumentNo(bool IsFormateOnly, string CompanyId, string LocationId, bool IsUseCompIdInWhere, string ModuleId, string ObjectId, string LastTransId, ref SqlTransaction trns)
    {
        string StrDocument = string.Empty;
        DataTable dt = new DataTable();

        dt = objDocNo.GetDocumentNumberAll(CompanyId, HttpContext.Current.Session["BrandId"].ToString(), LocationId, ModuleId, ObjectId, "2", ref trns);


        if (dt.Rows.Count != 0)
        {
            DataRow dr = dt.Rows[0];

            if (dr["Prefix"].ToString() != "")
            {
                StrDocument += dr["Prefix"].ToString();
            }
            if (Convert.ToBoolean(dr["CompId"].ToString()))
            {
                StrDocument += HttpContext.Current.Session["CompId"].ToString();
            }

            if (Convert.ToBoolean(dr["BrandId"].ToString()))
            {
                StrDocument += HttpContext.Current.Session["BrandId"].ToString();
            }

            if (Convert.ToBoolean(dr["LocationId"].ToString()))
            {
                StrDocument += LocationId;
            }

            if (Convert.ToBoolean(dr["DeptId"].ToString()))
            {
                StrDocument += HttpContext.Current.Session["DepartmentId"].ToString();
            }

            if (Convert.ToBoolean(dr["EmpId"].ToString()))
            {
                StrDocument += HttpContext.Current.Session["EmpId"].ToString();
            }

            if (Convert.ToBoolean(dr["Day"].ToString()))
            {
                StrDocument += DateTime.Now.Day.ToString();
            }

            if (Convert.ToBoolean(dr["Month"].ToString()))
            {
                StrDocument += DateTime.Now.Month.ToString();
            }
            if (Convert.ToBoolean(dr["Year"].ToString()))
            {
                StrDocument += DateTime.Now.Year.ToString();
            }

            if (dr["Suffix"].ToString() != "")
            {
                StrDocument += dr["Suffix"].ToString();
            }

            if (!IsFormateOnly)
            {
                if (StrDocument != "")
                {
                    StrDocument += (LastTransId + 1).ToString();
                }
                else
                {
                    StrDocument += (LastTransId + 1).ToString();

                }
            }
            else
            {
                StrDocument += "";
            }
        }
        return StrDocument.Trim();
    }
    protected void IbtnRestore_Command(object sender, CommandEventArgs e)
    {

        int b = 0;
        b = objProductionProcess.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Activated");
        }
        else
        {
            DisplayMessage("Record Not Activated");
        }
        FillGrid();
        FillGridBin();
        Reset();
    }
    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Text = "";
        txtValueBin.Focus();
    }
    protected void GvProductionProcessBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvProductionProcessBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtPInquiryBin"];
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcessBin, dt, "", "");


        string temp = string.Empty;

        for (int i = 0; i < GvProductionProcessBin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)GvProductionProcessBin.Rows[i].FindControl("hdnTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvProductionProcessBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }
    protected void GvProductionProcessBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objProductionProcess.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtPInquiryBin"] = dt;
        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcessBin, dt, "", "");

        lblSelectedRecord.Text = "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objProductionProcess.GetAllFalseRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvProductionProcessBin, dt, "", "");
        Session["dtPInquiryBin"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = false;
        }
        else
        {
            ImgbtnSelectAll.Visible = false;
            imgBtnRestore.Visible = true;
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedItem.Value == "Job_Creation_Date" || ddlFieldNameBin.SelectedItem.Value == "Request_Date")
        {
            if (txtValueBinDate.Text != "")
            {

                try
                {
                    ObjSysParam.getDateForInput(txtValueBinDate.Text);
                    txtValueBin.Text = Convert.ToDateTime(txtValueBinDate.Text).ToString();
                }
                catch
                {
                    DisplayMessage("Enter Date in format " + Session["DateFormat"].ToString() + "");
                    txtValueBinDate.Text = "";
                    txtValueBin.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBinDate);
                    return;
                }
            }
            else
            {
                DisplayMessage("Enter Date");
                txtValueBinDate.Focus();
                txtValueBin.Text = "";
                return;
            }
        }
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)Session["dtPInquiryBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtPInquiryBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcessBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillGridBin();
            }
            //AllPageCode();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
            //   btnRefreshBin.Focus();
        }
        if (txtValueBin.Text != "")
            txtValueBin.Focus();
        else if (txtValueBinDate.Text != "")
            txtValueBinDate.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvProductionProcessBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvProductionProcessBin.Rows.Count; i++)
        {
            ((CheckBox)GvProductionProcessBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(GvProductionProcessBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(GvProductionProcessBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(GvProductionProcessBin.Rows[i].FindControl("hdnTransId"))).Value.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        HiddenField lb = (HiddenField)GvProductionProcessBin.Rows[index].FindControl("hdnTransId");
        if (((CheckBox)GvProductionProcessBin.Rows[index].FindControl("chkSelect")).Checked)
        {
            empidlist += lb.Value.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Value.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;

        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        txtValueBinDate.Text = "";


        lblSelectedRecord.Text = "";
        //AllPageCode();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtPInquiryBin"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Id"]))
                {
                    lblSelectedRecord.Text += dr["Id"] + ",";
                }
            }
            for (int i = 0; i < GvProductionProcessBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)GvProductionProcessBin.Rows[i].FindControl("hdnTransId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvProductionProcessBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtPInquiryBin"];
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcessBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objProductionProcess.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvProductionProcessBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
                if (chk.Checked)
                {
                    fleg = 1;
                }
                else
                {
                    fleg = 0;
                }
            }
            if (fleg == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
    }
    #endregion
    #endregion
    #region User defined Function

    private void FillGrid()
    {
        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Field2='True'";
        }
        else
            if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Field2='False'";
        }

        if (PostStatus == "")
        {
            PostStatus = " Is_Post='True'";
        }
        else
        {
            PostStatus += " and Is_Post='True'";
        }

        DataTable dtPInquiry = new DataView(objProductionProcess.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()), PostStatus, "Id Desc", DataViewRowState.CurrentRows).ToTable();

        //DataTable dtPInquiry = objProductionProcess.GetAllTrueRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count + "";

        Session["dtPInquiry"] = dtPInquiry;
        if (dtPInquiry.Rows.Count > 0)
        {
            //this function for bind gridview by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvProductionProcess, dtPInquiry, "", "");

        }
        else
        {
            GvProductionProcess.DataSource = null;
            GvProductionProcess.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtPInquiry.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = Convert.ToDateTime(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
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
    public void Reset()
    {

        Session["DtBOM"] = null;
        objPageCmn.FillData((object)gvBom, null, "", "");
        btnPICancel.Visible = true;
        txtPINo.ReadOnly = false;
        txtDescription.Text = "";
        GvProduct.DataSource = null;
        GvProduct.DataBind();
        Session["DtDetail"] = null;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        txtValueDate.Text = "";
        txtValueBinDate.Text = "";
        Session["dtSerial"] = null;
        Session["dtFinal"] = null;
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValueBin.Visible = true;
        txtValueBinDate.Visible = false;
        Session["Expdt"] = null;
        objPageCmn.FillData((object)GridExpenses, null, "", "");
        lblSelectedRecord.Text = "";
        //txtPINo.Text = GetDocumentNumber();
        txtPINo.Text = GetDocumentNum();
        ViewState["DocNo"] = txtPINo.Text;
        Session["DtSearchProduct"] = null;
        ViewState["Dtproduct"] = null;
        Session["dtVisitTaskList"] = null;
        LoadVisitTask();
        hdnrequestid.Value = "";
        txtRequestNo.Text = "";
        txtRequestDate.Text = "";
        txtCustomer.Text = "";
        txtSONo.Text = "";
        txtSODate.Text = "";
        txtjobcreationdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtexpjobenddate.Text = "";
        txtjobstartdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtDescription.Text = "";
        chkpost.Checked = false;
        chkCancel.Checked = false;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        ChkisQualitycheck.Checked = false;
        //Session["dtExpList"] = null;
        //LoadExpensesRecord();
        objPageCmn.FillData((object)gvReturn, null, "", "");
        txtSerialNo.Text = "";
        Session["dtIssueRoll"] = null;
    }
    protected string GetDocumentNum()
    {
        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "12", "49", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;
    }
    #endregion
    #region ViewOption
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);

    }
    #endregion
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        btnPISave.Visible = true;
        btnPost.Visible = true;
        GridExpenses.Columns[0].Visible = true;

        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();

        GvProduct.Columns[0].Visible = true;
        GvProduct.Columns[1].Visible = true;
        imgBtnRestore.Visible = true;

    }
    #endregion
    protected void GvPurchaseInquiry_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public string ProductName(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        else
        {
            ProductName = "0";


        }

        return ProductName;

    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "";
        }
        return ProductName;
    }
    public string ProductDescription(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["Description"].ToString();
        }
        else
        {
            ProductName = "0";


        }
        return ProductName;
    }
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = UM.GetUnitMasterById(Session["CompId"].ToString().ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Job_Creation_Date" || ddlFieldName.SelectedItem.Value == "Request_Date")
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
        ddlFieldName.Focus();
    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldNameBin.SelectedItem.Value == "Job_Creation_Date" || ddlFieldNameBin.SelectedItem.Value == "Request_Date")
        {
            txtValueBinDate.Visible = true;
            txtValueBin.Visible = false;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        else
        {
            txtValueBinDate.Visible = false;
            txtValueBin.Visible = true;
            txtValueBin.Text = "";
            txtValueBinDate.Text = "";

        }
        ddlFieldNameBin.Focus();
    }


    #endregion
    #region Bom
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id", typeof(int));
        dt.Columns.Add("Item_Id");
        dt.Columns.Add("Unit_Id");
        dt.Columns.Add("Sys_qty");
        dt.Columns.Add("Unit_Price");
        dt.Columns.Add("Req_Qty");
        dt.Columns.Add("Line_Total");
        return dt;
    }
    public DataTable SavedGridRecordindatatble()
    {
        DataTable dt = CreateProductDataTable();
        foreach (GridViewRow gvRow in gvBom.Rows)
        {
            DataRow dr = dt.NewRow();

            Label lblTransId = (Label)gvRow.FindControl("lblTransId");
            HiddenField hdngvProductId = (HiddenField)gvRow.FindControl("hdngvProductId");
            HiddenField hdngvUnitId = (HiddenField)gvRow.FindControl("hdngvUnitId");
            Label lblgvSystemQuantity = (Label)gvRow.FindControl("lblgvSystemQuantity");
            Label txtgvUnitPrice = (Label)gvRow.FindControl("lblgvUnitPrice");
            Label txtRequestquantity = (Label)gvRow.FindControl("lblRequestquantity");
            Label lblgvtotal = (Label)gvRow.FindControl("lblgvtotal");

            dr["Id"] = lblTransId.Text;
            dr["Item_Id"] = hdngvProductId.Value;
            dr["Unit_Id"] = hdngvUnitId.Value;
            dr["Sys_qty"] = SetDecimal(lblgvSystemQuantity.Text);
            dr["Unit_Price"] = SetDecimal(txtgvUnitPrice.Text);
            dr["Req_Qty"] = SetDecimal(txtRequestquantity.Text);
            dr["Line_Total"] = SetDecimal(lblgvtotal.Text);
            dt.Rows.Add(dr);
        }
        return dt;
    }

    //protected void txtgvUnitPrice_TextChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;

    //    TextBox b = (TextBox)sender;
    //    string objSenderID = b.ID;


    //    if (((TextBox)gvRow.FindControl("txtRequestquantity")).Text == "")
    //    {
    //        ((TextBox)gvRow.FindControl("txtRequestquantity")).Text = "0";
    //    }

    //    if (((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text == "")
    //    {
    //        ((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text = "0";
    //    }
    //    ((TextBox)gvRow.FindControl("txtRequestquantity")).Text = SetDecimal(((TextBox)gvRow.FindControl("txtRequestquantity")).Text);
    //    ((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text = SetDecimal(((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text);

    //    try
    //    {
    //        ((Label)gvRow.FindControl("lblgvtotal")).Text = SetDecimal((float.Parse(((TextBox)gvRow.FindControl("txtgvUnitPrice")).Text) * float.Parse(((TextBox)gvRow.FindControl("txtRequestquantity")).Text)).ToString());
    //    }
    //    catch
    //    {
    //        ((Label)gvRow.FindControl("lblgvtotal")).Text = "0";
    //    }

    //    if (objSenderID == "txtRequestquantity")
    //    {
    //        ((TextBox)gvRow.FindControl("txtgvUnitPrice")).Focus();
    //    }

    //}

    public string SetDecimal(string amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster ObjInvProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvProductMaster.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["locId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        DataTable dtTemp = dt.Copy();
        dt = new DataView(dt, "ProductCode like '%" + prefixText.ToString() + "%'", "ProductCode Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();
            }
        }
        return str;
    }
    #endregion
    #region JobDetail



    public void LoadVisitTask()
    {
        DataTable dt = new DataTable();
        if (Session["dtVisitTaskList"] != null)
        {


            dt = new DataTable();
            dt = (DataTable)Session["dtVisitTaskList"];
            if (dt.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvVisitTask, dt, "", "");
            }
        }
    }

    protected void gvVisitTask_RowCommand(object sender, GridViewCommandEventArgs e)
    {



        DataTable dt = new DataTable();
        string EmpId = "";
        if (e.CommandName.Equals("AddNew"))
        {
            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Text == "")
            {
                DisplayMessage("Enter Employee Name");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Focus();
                return;
            }

            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Text == "")
            {
                DisplayMessage("Enter Start date");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Focus();
                return;
            }

            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtEnddate")).Text == "")
            {
                DisplayMessage("Enter End date");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtEnddate")).Focus();
                return;
            }


            if (((TextBox)gvVisitTask.FooterRow.FindControl("txtduration")).Text == "")
            {
                DisplayMessage("Enter Work duration");
                ((TextBox)gvVisitTask.FooterRow.FindControl("txtduration")).Focus();
                return;
            }







            if (Session["dtVisitTaskList"] == null)
            {
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("Emp_Name", typeof(string));
                dt.Columns.Add("Start_Time", typeof(DateTime));
                dt.Columns.Add("End_Time", typeof(DateTime));
                dt.Columns.Add("Duration", typeof(string));

                DataRow dr = dt.NewRow();
                dr[0] = "1";
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Text;
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Text;
                dr[3] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtEnddate")).Text;
                dr[4] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtduration")).Text;
                dt.Rows.Add(dr);
            }
            else
            {
                string strTransid = string.Empty;
                dt = (DataTable)Session["dtVisitTaskList"];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtCopy = dt.Copy();
                    dtCopy = new DataView(dtCopy, "", "Id desc", DataViewRowState.CurrentRows).ToTable();
                    strTransid = (float.Parse(dtCopy.Rows[0]["Id"].ToString()) + 1).ToString();
                }
                else
                {
                    strTransid = "1";
                }

                DataRow dr = dt.NewRow();
                dr[0] = strTransid;
                dr[1] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Text;
                dr[2] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtstartdate")).Text;
                dr[3] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtEnddate")).Text;
                dr[4] = ((TextBox)gvVisitTask.FooterRow.FindControl("txtduration")).Text;
                dt.Rows.Add(dr);
            }
            Session["dtVisitTaskList"] = dt;
            gvVisitTask.EditIndex = -1;
            LoadVisitTask();
        }
        if (e.CommandName.Equals("Delete"))
        {

            if (Session["dtVisitTaskList"] != null)
            {
                dt = (DataTable)Session["dtVisitTaskList"];
                dt = new DataView(dt, "Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                Session["dtVisitTaskList"] = dt;
            }
            gvVisitTask.EditIndex = -1;
            LoadVisitTask();
        }

        ((TextBox)gvVisitTask.FooterRow.FindControl("txtEmpFooter")).Focus();



    }
    protected void gvVisitTask_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvVisitTask.EditIndex = e.NewEditIndex;
        LoadVisitTask();
    }
    protected void gvVisitTask_OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvVisitTask.EditIndex = -1;
        LoadVisitTask();
    }
    protected void gvVisitTask_OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtVisitTaskList"];

        GridViewRow row = gvVisitTask.Rows[e.RowIndex];

        dt.Rows[row.DataItemIndex]["Emp_Name"] = ((TextBox)row.FindControl("txtempName")).Text;
        dt.Rows[row.DataItemIndex]["Start_Time"] = ((TextBox)row.FindControl("txteditstartdate")).Text;
        dt.Rows[row.DataItemIndex]["End_Time"] = ((TextBox)row.FindControl("txteditEnddate")).Text;
        dt.Rows[row.DataItemIndex]["Duration"] = ((TextBox)row.FindControl("txteditDuration")).Text;

        Session["dtVisitTaskList"] = dt;
        gvVisitTask.EditIndex = -1;
        LoadVisitTask();
    }
    protected void gvVisitTask_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjEmployeeMaster.GetEmployeeMasterAll(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        dt = new DataView(dt, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString();
        }
        return txt;
    }
    #endregion
    #region Post

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //AllPageCode();

    }
    #endregion
    #region Expenses
    public string CurrencyName(string CurrencyId)
    {
        string CurrencyName = string.Empty;
        DataTable dt = ObjCurrencyMaster.GetCurrencyMasterById(CurrencyId.ToString());
        if (dt.Rows.Count != 0)
        {
            CurrencyName = dt.Rows[0]["Currency_Name"].ToString();
        }
        else
        {
            CurrencyName = "0";
        }
        return CurrencyName;
    }
    public string GetExpName(string ExpId)
    {
        return (ObjShipExp.GetShipExpMasterById(Session["CompId"].ToString(), ExpId)).Rows[0]["Exp_Name"].ToString();
    }
    protected void txtFCExpAmount_TextChanged(object sender, EventArgs e)
    {

        if (txtFCExpAmount.Text != "")
        {
            if (txtExpExchangeRate.Text == "")
            {
                txtExpExchangeRate.Text = GetAmountDecimal("0");
            }
            txtExpCharges.Text = GetAmountDecimal((float.Parse(txtFCExpAmount.Text) * float.Parse(txtExpExchangeRate.Text)).ToString());
        }

    }
    protected void ddlExpCurrency_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpCurrency.SelectedIndex != 0)
        {
            //updated on 30-11-2015 for currency conversion by jitendra upadhyay
            txtExpExchangeRate.Text = SystemParameter.GetExchageRate(ddlExpCurrency.SelectedValue, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Session["DBConnection"].ToString());

            if (txtExpExchangeRate.Text != "")
            {
                txtExpCharges.Text = GetAmountDecimal((float.Parse(txtFCExpAmount.Text.Trim()) * float.Parse(txtExpExchangeRate.Text.Trim())).ToString());
            }
        }
    }

    public void fillCurrency()
    {
        DataTable dt = ObjCurrencyMaster.GetCurrencyMaster();


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        try
        {
            objPageCmn.FillData((object)ddlExpCurrency, dt, "Currency_Name", "Currency_Id");
        }
        catch
        {
        }

        try
        {
            ddlExpCurrency.SelectedValue = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
            txtExpExchangeRate.Text = "1";
        }
        catch
        {
        }

    }
    private string GetAccountId(string strAccountName)
    {
        string retval = string.Empty;
        if (strAccountName != "")
        {
            retval = (strAccountName.Split('/'))[strAccountName.Split('/').Length - 1];

            DataTable dtAccount = objCOA.GetCOAByTransId(Session["CompId"].ToString(), retval);
            if (dtAccount.Rows.Count > 0)
            {

            }
            else
            {
                retval = "";
            }
        }
        else
        {
            retval = "";
        }
        return retval;
    }

    public string GetAmountDecimal(string Amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LOcId"].ToString()).Rows[0]["Field1"].ToString(), Amount);

    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(CurrencyId, GetAmountDecimal(Amount.ToString()), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }

        return Amountwithsymbol;

    }

    public void fillExpGrid(DataTable dt)
    {
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GridExpenses, dt, "", "");
        Session["Expdt"] = dt;
        if (dt.Rows.Count != 0)
        {
            float f = 0;
            foreach (GridViewRow Row in GridExpenses.Rows)
            {
                f += float.Parse(((Label)Row.FindControl("lblgvExp_Charges")).Text.Trim());
                ((Label)Row.FindControl("lblgvExp_Charges")).Text = ObjSysParam.GetCurencyConversionForInv(((Label)Row.FindControl("hidExpCur")).Text, ((Label)Row.FindControl("lblgvExp_Charges")).Text.Trim());
                ((Label)Row.FindControl("lblgvExp_Charges")).Text = GetCurrencySymbol(((Label)Row.FindControl("lblgvExp_Charges")).Text, objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LOcId"].ToString()).Rows[0]["Field1"].ToString());
                ((Label)Row.FindControl("lblgvFCExchangeAmount")).Text = GetCurrencySymbol(((Label)Row.FindControl("lblgvFCExchangeAmount")).Text, ((Label)Row.FindControl("hidExpCur")).Text);
            }
            ((Label)GridExpenses.FooterRow.FindControl("txttotExp")).Text = GetAmountDecimal(f.ToString());
            ((Label)GridExpenses.FooterRow.FindControl("txttotExpShow")).Text = GetCurrencySymbol(GetAmountDecimal(f.ToString()), objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LOcId"].ToString()).Rows[0]["Field1"].ToString());

        }
        else
        {
            try
            {
                ((Label)GridExpenses.FooterRow.FindControl("txttotExp")).Text = "0";
                ((Label)GridExpenses.FooterRow.FindControl("txttotExpShow")).Text = "0";

            }
            catch
            {

            }
        }
        CostingRate();

        //AllPageCode();
    }
    public void fillExpenses()
    {
        DataTable dt = ObjShipExp.GetShipExpMaster(Session["CompId"].ToString().ToString());


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        objPageCmn.FillData((object)ddlExpense, dt, "Exp_Name", "Expense_Id");

    }
    protected void IbtnAddExpenses_Click(object sender, object e)
    {
        string ExpId = string.Empty;
        if (ddlExpense.SelectedIndex == 0)
        {
            DisplayMessage("Select Expenses");
            ddlExpense.Focus();
            return;
        }
        else
        {
            ExpId = ddlExpense.SelectedValue.ToString();
        }

        if (txtExpCharges.Text == "")
        {
            DisplayMessage("Enter Expenses Charges");
            txtExpCharges.Focus();
            return;
        }
        if (ddlExpCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Currency Required On Company Level");
            ddlExpCurrency.Focus();
            return;
        }

        DataTable dt = new DataTable();
        int i = 0;
        bool b = false;

        if (Session["Expdt"] != null)
        {
            dt = (DataTable)Session["Expdt"];
            foreach (DataRow Dr in dt.Rows)
            {
                if (Dr["Expense_Id"].ToString() == ddlExpense.SelectedValue.ToString())
                {
                    b = true;
                    i = dt.Rows.IndexOf(Dr);
                }
            }
            if (!b)
            {
                dt.Rows.Add();
                i = dt.Rows.Count - 1;
            }
        }
        else
        {
            dt.Columns.Add("Expense_Id");
            dt.Columns.Add("Account_No");
            dt.Columns.Add("Exp_Charges");
            dt.Columns.Add("ExpCurrencyID");
            dt.Columns.Add("ExpExchangeRate");
            dt.Columns.Add("FCExpAmount");

            dt.Rows.Add();
        }

        dt.Rows[i]["Expense_Id"] = ddlExpense.SelectedValue.ToString();
        dt.Rows[i]["Account_No"] = GetAccountId(txtExpensesAccount.Text);
        dt.Rows[i]["Exp_Charges"] = ObjSysParam.GetCurencyConversionForInv(ddlExpCurrency.SelectedValue.ToString(), txtExpCharges.Text.Trim());
        dt.Rows[i]["ExpCurrencyID"] = ddlExpCurrency.SelectedValue.ToString();
        dt.Rows[i]["ExpExchangeRate"] = txtExpExchangeRate.Text.ToString();
        dt.Rows[i]["FCExpAmount"] = ObjSysParam.GetCurencyConversionForInv(ddlExpCurrency.SelectedValue.ToString(), txtFCExpAmount.Text.ToString());

        txtExpCharges.Text = "0";
        ddlExpense.SelectedIndex = 0;
        txtExpensesAccount.Text = "";
        txtExpExchangeRate.Text = "0";
        txtFCExpAmount.Text = "0";
        fillCurrency();
        Session["Expdt"] = dt;
        fillExpGrid(dt);
    }
    protected void IbtnDeleteExp_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView(((DataTable)Session["Expdt"]), "Expense_Id<>'" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        Session["Expdt"] = dt;
        fillExpGrid(dt);
        // GetData();
    }
    protected void ddlExpense_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlExpense.SelectedValue == "--Select--")
        {
            txtExpensesAccount.Text = "";
        }
        else if (ddlExpense.SelectedValue != "--Select--")
        {
            DataTable dtExp = ObjShipExp.GetShipExpMasterById(Session["CompId"].ToString(), ddlExpense.SelectedValue);
            if (dtExp.Rows.Count > 0)
            {
                string strAccountId = dtExp.Rows[0]["Account_No"].ToString();
                DataTable dtAcc = objCOA.GetCOAByTransId(Session["CompId"].ToString(), strAccountId);
                if (dtAcc.Rows.Count > 0)
                {
                    string strAccountName = dtAcc.Rows[0]["AccountName"].ToString();
                    txtExpensesAccount.Text = strAccountName + "/" + strAccountId;
                }
            }
        }
        // GetData();
    }
    protected void txtExpensesAccount_TextChanged(object sender, EventArgs e)
    {

        if (txtExpensesAccount.Text != "")
        {
            DataTable dtAccount = objCOA.GetCOAAll(Session["CompId"].ToString());
            try
            {
                dtAccount = new DataView(dtAccount, "AccountName='" + txtExpensesAccount.Text.Split('/')[0].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

                if (dtAccount.Rows.Count == 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                    txtExpensesAccount.Text = "";
                    DisplayMessage("No Account Found");
                    txtExpensesAccount.Focus();
                    return;
                }

            }
            catch
            {
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtExpensesAccount);
                txtExpensesAccount.Text = "";
                DisplayMessage("No Account Found");
                txtExpensesAccount.Focus();
                return;
            }

        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountNo(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount COA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());

        dt = new DataView(dt, "AccountName Like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (prefixText.Length > 2)
            {
                str = null;
            }
            else
            {
                dt = COA.GetCOAAllTrue(HttpContext.Current.Session["CompId"].ToString());
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
                    }
                }
            }
        }
        return str;
    }
    #endregion
    #region CostingRate
    public void CostingRate()
    {
        float TotExp = 0;
        float totalAmt = 0;
        float costing_rate = 0;



        foreach (DataRow dr in ((DataTable)Session["Expdt"]).Rows)
        {
            try
            {
                TotExp += float.Parse(dr["Exp_Charges"].ToString());
            }
            catch
            {
            }
        }
        foreach (GridViewRow gvrow in gvBom.Rows)
        {
            try
            {
                totalAmt += float.Parse(((Label)gvrow.FindControl("lblgvtotal")).Text);
            }
            catch
            {
            }

        }

        try
        {

            float Cost = 0;
            Cost = (totalAmt + TotExp);
            if (Cost > 0 && totalAmt > 0)
            {
                costing_rate = Cost / totalAmt;
            }
            else
            {
                costing_rate = 0;
            }
        }
        catch
        {
            costing_rate = 0;
        }

        foreach (GridViewRow Row in GvProduct.Rows)
        {
            ((Label)Row.FindControl("lblunitCost")).Text = SetDecimal((float.Parse(((Label)Row.FindControl("lblunitPrice")).Text) * costing_rate).ToString());

        }
    }
    #endregion
    #region Serial Number

    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();

        if (txtSerialNo.Text.Trim() != "")
        {
            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;
            if (Session["dtSerial"] == null)
            {
                dt = new DataTable();
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

                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        if ((new DataView(dt, "SerialNo='" + txt[i].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) == 0)
                        {
                            //string[] result = isSerialNumberValid_purchasert(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                            //if (result[0] == "VALID")
                            //{
                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = "0";
                            dr[3] = "0";
                            dr[4] = "0";
                            dr[5] = DateTime.Now.ToString();
                            dr[6] = "0";
                            dr[7] = hdnTransType.Value;
                            dr[8] = "0";
                            dr[9] = DateTime.Now.ToString();
                            dr[10] = "1";
                            counter++;

                            //}
                            //else if (result[0].ToString() == "DUPLICATE")
                            //{
                            //    DuplicateserialNo += txt[i].ToString() + ",";
                            //}
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
                        if ((new DataView(dt, "SerialNo='" + txt[i].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) == 0)
                        {
                            //string[] result = isSerialNumberValid_GoodsReceive(txt[i].ToString().Trim(), ViewState["PID"].ToString(), TransId, gvSerialNumber);
                            //if (result[0] == "VALID")
                            //{

                            DataRow dr = dt.NewRow();
                            dt.Rows.Add(dr);
                            dr[0] = ViewState["PID"].ToString();
                            dr[1] = txt[i].ToString();
                            dr[2] = "0";
                            dr[3] = "0";
                            dr[4] = "0";
                            dr[5] = DateTime.Now.ToString();
                            dr[6] = "0";
                            dr[7] = hdnTransType.Value;
                            dr[8] = "0";
                            dr[9] = DateTime.Now.ToString();
                            dr[10] = "1";
                            counter++;

                            //}
                            //else if (result[0].ToString() == "DUPLICATE")
                            //{
                            //    DuplicateserialNo += txt[i].ToString() + ",";
                            //}
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
        if (DuplicateserialNo != "")
        {
            if (DuplicateserialNo != "")
            {
                Message += "Following serial Number is Already Exists in stock=" + DuplicateserialNo;
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

            Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


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

        //set return quanity in textbox
        if (ViewState["RowIndex"] != null)
        {
            foreach (GridViewRow gvRow in gvReturn.Rows)
            {

                TextBox txtRecQty = (TextBox)gvRow.FindControl("txtreturnquantity");
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



            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            try
            {
                dtserial = new DataView(dtserial, "SerialNo='" + serialNumber + "' and ProductId=" + ProductId + "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

            if (dtserial.Rows.Count == 0)
            {
                //if we not found in database with thsi product id that we are allow this serial number
                Result[0] = "VALID";
            }
            else
            {
                if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "I")
                {
                    Result[0] = "DUPLICATE";
                }
                else
                {

                    if (dtserial.Rows[0]["TransType"].ToString().ToUpper() == "PR")
                    {

                        Result[0] = "VALID";
                        Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                        Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                        Result[4] = dtserial.Rows[0]["BatchNo"].ToString();

                    }
                }
            }
        }
        else
        {
            Result[0] = "DUPLICATE";
        }

        return Result;
    }
    public static string[] isSerialNumberValid_PurchaseReturn(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber, string InvoiceId, string PoId)
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



            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMasterAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

            try
            {
                //ViewState["InvoiceId"] 

                dtserial = new DataView(dtserial, "SerialNo='" + serialNumber + "' and ProductId=" + ProductId + " and TransType='PG' and TransTypeId=" + InvoiceId + " and Field1='" + PoId + "'", "", DataViewRowState.CurrentRows).ToTable();

                //}
            }
            catch
            {
            }

            if (dtserial.Rows.Count == 0)
            {
                //if we not found in database with thsi product id that we are allow this serial number
                Result[0] = "Not Exist";
            }
            else
            {
                if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "O")
                {
                    Result[0] = "ALREADY OUT";
                }
                if (dtserial.Rows[0]["InOut"].ToString().ToUpper() == "I")
                {

                    Result[0] = "VALID";
                    Result[1] = dtserial.Rows[0]["SerialNo"].ToString();
                    Result[2] = Convert.ToDateTime(dtserial.Rows[0]["ManufacturerDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[3] = Convert.ToDateTime(dtserial.Rows[0]["ExpiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    Result[4] = dtserial.Rows[0]["BatchNo"].ToString();
                }
            }
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
        GridView gridView = (GridView)Row.NamingContainer;
        if (gridView.ID == "GvProduct")
        {
            hdnTransType.Value = "FI";
            Label6222.Text = Resources.Attendance.Request_Quantity;
            lblpnlInvoiceQty.Text = ((Label)Row.FindControl("lblReqQty")).Text;
            lblProductIdvalue.Text = ((Label)Row.FindControl("lblproductcode")).Text;
            lblProductNameValue.Text = ((Label)Row.FindControl("lblProductId")).Text;
            ViewState["RowIndex"] = null;
        }
        else
        {
            hdnTransType.Value = "FR";
            Label6222.Text = "";
            lblpnlInvoiceQty.Text = "";
            lblProductIdvalue.Text = ((Label)Row.FindControl("lblgvProductCode")).Text;
            lblProductNameValue.Text = ((Label)Row.FindControl("lblgvProductName")).Text;
            ViewState["RowIndex"] = Row.RowIndex;
        }

        ViewState["PID"] = e.CommandArgument.ToString();
        ViewState["RowIndex"] = Row.RowIndex;


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
            dt = new DataView(dt, "ProductId='" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ViewState["Tempdt"] = dt;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            Session["dtSerial"] = dt;
        }
        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "SNO")
        {
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            pnlSerialNumber.Visible = true;
            pnlexp_and_Manf.Visible = false;
            Counter = 1;
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LE" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FE")
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
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LM" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FM")
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
    }
    public string setDateTime(string Value)
    {
        string strDate = string.Empty;
        try
        {
            strDate = Convert.ToDateTime(Value).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        catch
        {
        }
        return strDate;
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

            Dtfinal = new DataView(Dtfinal, "ProductId<>'" + ViewState["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            Session["dtFinal"] = Dtfinal;
        }


        txtSerialNo.Focus();
    }
    protected void btncloseserial_Click(object sender, EventArgs e)
    {

        pnlSerialNumber.Visible = false;
        pnlexp_and_Manf.Visible = false;
        //lblDuplicateserialNo.Text = "";
        Session["dtSerial"] = null;
    }
    protected void Btnloadfile_Click(object sender, EventArgs e)
    {
        int counter = 0;
        txtSerialNo.Text = "";
        try
        {

            string Path = Server.MapPath("~/Temp/" + FULogoPath.FileName);
            string[] csvRows = System.IO.File.ReadAllLines(Path);
            string[] fields = null;

            foreach (string csvRow in csvRows)
            {
                fields = csvRow.Split(',');

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
        //AllPageCode();
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
                dt.Columns.Add("Trans_Id");
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
            dt.Columns.Add("Trans_Id");
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
                dr[6] = "0";
                dr[7] = hdnTransType.Value;
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
                dr[6] = "0";
                dr[7] = hdnTransType.Value;
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
    }

    protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }


    #endregion
    #endregion
    #region rowdatabound
    protected void GvProduct_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((Label)e.Row.FindControl("lblPID")).Text;
            if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["itemtype"].ToString() == "S")
            {
                if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "NM")
                {
                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
                }
                else
                {


                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                }
            }
            else
            {
                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
            }
        }
    }

    protected void gvReturn_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string ProductID = ((HiddenField)e.Row.FindControl("hdngvProductId")).Value;
            if (ProductID != "0")
            {
                if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["itemtype"].ToString() == "S")
                {
                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "NM")
                    {
                        ((TextBox)e.Row.FindControl("txtreturnquantity")).Enabled = true;

                        ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
                    }
                    else
                    {
                        ((TextBox)e.Row.FindControl("txtreturnquantity")).Enabled = false;

                        ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
                    }
                }
                else
                {
                    ((TextBox)e.Row.FindControl("txtreturnquantity")).Enabled = true;

                    ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
                }
            }
            else
            {
                ((TextBox)e.Row.FindControl("txtreturnquantity")).Enabled = true;

                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
            }
        }
    }
    #endregion
    #region ManageInventory
    protected void txtusedqty_OnTextChanged(object sender, EventArgs e)
    {
        double totalqty = 0;
        double usedqty = 0;
        double wasteqty = 0;

        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;

        if (((Label)gvRow.FindControl("lblqty")).Text == "")
        {
            ((Label)gvRow.FindControl("lblqty")).Text = "0";
        }
        if (((TextBox)gvRow.FindControl("txtusedqty")).Text.Trim() == "")
        {
            ((TextBox)gvRow.FindControl("txtusedqty")).Text = "0";
        }
        if (((TextBox)gvRow.FindControl("txtwasteqty")).Text.Trim() == "")
        {
            ((TextBox)gvRow.FindControl("txtwasteqty")).Text = "0";
        }

        if ((float.Parse(((TextBox)gvRow.FindControl("txtusedqty")).Text) + float.Parse(((TextBox)gvRow.FindControl("txtwasteqty")).Text)) > float.Parse(((Label)gvRow.FindControl("lblqty")).Text))
        {
            DisplayMessage("waste quantity and used quantity sum should be equal with total quantity");
            ((TextBox)gvRow.FindControl("txtusedqty")).Text = "0";
            return;
        }

        DataTable dt = (DataTable)Session["dtIssueRoll"];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["SerialNo"].ToString() == ((Label)gvRow.FindControl("lblserialNo")).Text)
            {
                dt.Rows[i]["UsedQuantity"] = ((TextBox)gvRow.FindControl("txtusedqty")).Text;
            }
        }
        Session["dtIssueRoll"] = dt;
        getGridTotal();
        setBomvalue();
    }

    protected void txtwasteqty_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;

        if (((Label)gvRow.FindControl("lblqty")).Text == "")
        {
            ((Label)gvRow.FindControl("lblqty")).Text = "0";
        }
        if (((TextBox)gvRow.FindControl("txtusedqty")).Text.Trim() == "")
        {
            ((TextBox)gvRow.FindControl("txtusedqty")).Text = "0";
        }
        if (((TextBox)gvRow.FindControl("txtwasteqty")).Text.Trim() == "")
        {
            ((TextBox)gvRow.FindControl("txtwasteqty")).Text = "0";
        }

        if ((float.Parse(((TextBox)gvRow.FindControl("txtusedqty")).Text) + float.Parse(((TextBox)gvRow.FindControl("txtwasteqty")).Text)) > float.Parse(((Label)gvRow.FindControl("lblqty")).Text))
        {
            DisplayMessage("waste quantity and used quantity sum should be equal with total quantity");
            ((TextBox)gvRow.FindControl("txtwasteqty")).Text = "0";
            return;
        }
        DataTable dt = (DataTable)Session["dtIssueRoll"];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["SerialNo"].ToString() == ((Label)gvRow.FindControl("lblserialNo")).Text)
            {
                dt.Rows[i]["WasteQuantity"] = ((TextBox)gvRow.FindControl("txtwasteqty")).Text;
            }
        }
        Session["dtIssueRoll"] = dt;
        getGridTotal();
        setBomvalue();
    }
    protected void btnadd_OnClick(object sender, EventArgs e)
    {
        if (txtSNo.Text != "")
        {
            //here we check if serial number is already added

            if (Session["dtIssueRoll"] != null)
            {
                if (new DataView((DataTable)Session["dtIssueRoll"], "SerialNo='" + txtSNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    DisplayMessage("Serial No. already exists !");
                    txtSNo.Focus();
                    return;
                }
            }
            DataTable dtstockBatchMaster = ObjStockBatchMaster.GetStockBatchMasterAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString());

            dtstockBatchMaster = new DataView(dtstockBatchMaster, "SerialNo='" + txtSNo.Text + "'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();

            dtstockBatchMaster = dtstockBatchMaster.DefaultView.ToTable(true, "Trans_Id", "ProductId", "UnitId", "SerialNo", "Quantity", "Width", "Length", "Pallet_ID", "Field4", "Field5", "TransType");

            dtstockBatchMaster.Columns["Quantity"].ColumnName = "TotalQuantity";
            dtstockBatchMaster.Columns["Field5"].ColumnName = "UsedQuantity";
            dtstockBatchMaster.Columns["Field4"].ColumnName = "WasteQuantity";
            int counter = 0;
            string strProductId = string.Empty;
            if (dtstockBatchMaster.Rows.Count > 0)
            {

                //here we check that product id is exist or not in bom

                if (HttpContext.Current.Session["MaterialProductId"] != null)
                {
                    if (!Session["MaterialProductId"].ToString().Split(',').Contains(dtstockBatchMaster.Rows[0]["ProductId"].ToString()))
                    {
                        DisplayMessage("Product not exist in bom");
                        return;
                    }
                }



                if (dtstockBatchMaster.Rows[0]["TransType"].ToString() == "PG" || dtstockBatchMaster.Rows[0]["TransType"].ToString() == "OP")
                {
                    dtstockBatchMaster = new DataView(dtstockBatchMaster, "TransType='PG' or TransType='OP'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();

                    counter = 1;
                }
                else
                    if (dtstockBatchMaster.Rows[0]["TransType"].ToString() == "FR")
                {
                    dtstockBatchMaster = new DataView(dtstockBatchMaster, "TransType='FR'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();

                    counter = 1;
                }
                else
                        if (dtstockBatchMaster.Rows[0]["TransType"].ToString() == "FO")
                {
                    DisplayMessage("Serial number already out from stock");
                    txtSNo.Focus();
                    return;
                }

                if (Session["dtIssueRoll"] != null)
                {
                    DataTable dt = new DataTable();
                    dt = (DataTable)Session["dtIssueRoll"];
                    dt.Merge(dtstockBatchMaster);

                    dt.Rows[dt.Rows.Count - 1]["UsedQuantity"] = dtstockBatchMaster.Rows[0]["UsedQuantity"].ToString();
                    dt.Rows[dt.Rows.Count - 1]["WasteQuantity"] = dtstockBatchMaster.Rows[0]["WasteQuantity"].ToString();
                    Session["dtIssueRoll"] = dt;
                    objPageCmn.FillData((Object)gvReturn, dt, "", "");

                    getGridTotal();
                    setBomvalue();
                }
                else
                {
                    dtstockBatchMaster.Rows[0]["UsedQuantity"] = dtstockBatchMaster.Rows[0]["UsedQuantity"].ToString();
                    dtstockBatchMaster.Rows[0]["WasteQuantity"] = dtstockBatchMaster.Rows[0]["WasteQuantity"].ToString();
                    Session["dtIssueRoll"] = dtstockBatchMaster;
                    objPageCmn.FillData((Object)gvReturn, dtstockBatchMaster, "", "");
                    getGridTotal();
                    setBomvalue();
                }

                txtSNo.Text = "";
                txtSNo.Focus();

            }
            else
            {
                DisplayMessage("Serial not found !");
                txtSNo.Focus();
                return;
            }
        }

    }

    public DataTable getSerialUsedQty()
    {
        DataTable dtSerialInfo = new DataTable();


        dtSerialInfo.Columns.Add("ProductId");
        dtSerialInfo.Columns.Add("Qty");
        dtSerialInfo.Columns.Add("WasteQty");


        foreach (GridViewRow gv in gvReturn.Rows)
        {
            if (((TextBox)gv.FindControl("txtusedqty")).Text == "")
            {
                ((TextBox)gv.FindControl("txtusedqty")).Text = "0";
            }

            if (((TextBox)gv.FindControl("txtwasteqty")).Text == "")
            {
                ((TextBox)gv.FindControl("txtwasteqty")).Text = "0";
            }


            if (new DataView(dtSerialInfo, "ProductId=" + ((HiddenField)gv.FindControl("hdngvProductId")).Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
            {
                DataRow dr = dtSerialInfo.NewRow();
                dr[0] = ((HiddenField)gv.FindControl("hdngvProductId")).Value;
                dr[1] = SetDecimal(((TextBox)gv.FindControl("txtusedqty")).Text);
                dr[2] = SetDecimal(((TextBox)gv.FindControl("txtwasteqty")).Text);
                dtSerialInfo.Rows.Add(dr);
            }
            else
            {
                for (int i = 0; i < dtSerialInfo.Rows.Count; i++)
                {
                    if (dtSerialInfo.Rows[i][0].ToString() == ((HiddenField)gv.FindControl("hdngvProductId")).Value)
                    {
                        dtSerialInfo.Rows[i][1] = SetDecimal((float.Parse(dtSerialInfo.Rows[i][1].ToString()) + float.Parse(((TextBox)gv.FindControl("txtusedqty")).Text)).ToString());
                        dtSerialInfo.Rows[i][2] = SetDecimal((float.Parse(dtSerialInfo.Rows[i][2].ToString()) + float.Parse(((TextBox)gv.FindControl("txtwasteqty")).Text)).ToString());

                    }
                }
            }

        }

        return dtSerialInfo;
    }
    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtIssueRoll"];

        try
        {
            dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        Session["dtIssueRoll"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvReturn, dt, "", "");
        //AllPageCode();
        getGridTotal();
        setBomvalue();
    }
    public void getGridTotal()
    {
        float usedqty = 0;
        float wasteqty = 0;
        string ProductId = string.Empty;
        foreach (GridViewRow gv in gvReturn.Rows)
        {
            ProductId = ((HiddenField)gv.FindControl("hdngvProductId")).Value;
            if (((TextBox)gv.FindControl("txtusedqty")).Text == "")
            {
                ((TextBox)gv.FindControl("txtusedqty")).Text = "0";
            }
            if (((TextBox)gv.FindControl("txtwasteqty")).Text == "")
            {
                ((TextBox)gv.FindControl("txtwasteqty")).Text = "0";
            }
            try
            {
                usedqty += float.Parse(((TextBox)gv.FindControl("txtusedqty")).Text);
                wasteqty += float.Parse(((TextBox)gv.FindControl("txtwasteqty")).Text);
            }
            catch
            {
            }
        }
        try
        {
            ((Label)gvReturn.FooterRow.FindControl("txttotUsedShow")).Text = SetDecimal(usedqty.ToString());
            ((Label)gvReturn.FooterRow.FindControl("txttotWasteShow")).Text = SetDecimal(wasteqty.ToString());
        }
        catch
        {

        }
        //for set total in bom at received qty

    }
    public void setBomvalue()
    {
        DataTable dt = getSerialUsedQty();



        string ProductId = string.Empty;
        foreach (GridViewRow gvbon in gvBom.Rows)
        {
            if (((HiddenField)gvbon.FindControl("hdngvProductId")).Value != "0")
            {

                if (ProductId.Trim() == "")
                {
                    ProductId = ((HiddenField)gvbon.FindControl("hdngvProductId")).Value;
                }
                else
                {
                    ProductId = ProductId + "," + ((HiddenField)gvbon.FindControl("hdngvProductId")).Value;
                }

                try
                {
                    ((TextBox)gvbon.FindControl("txtReceivedqty")).Text = new DataView(dt, "ProductId=" + ((HiddenField)gvbon.FindControl("hdngvProductId")).Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0][1].ToString();
                }
                catch
                {
                    ((TextBox)gvbon.FindControl("txtReceivedqty")).Text = "0";
                }

                try
                {
                    ((TextBox)gvbon.FindControl("txtwasteqty")).Text = new DataView(dt, "ProductId=" + ((HiddenField)gvbon.FindControl("hdngvProductId")).Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows[0][2].ToString();
                }
                catch
                {
                    ((TextBox)gvbon.FindControl("txtwasteqty")).Text = "0";
                }

                 ((Label)gvbon.FindControl("lblgvtotal")).Text = SetDecimal(((float.Parse(((TextBox)gvbon.FindControl("txtReceivedqty")).Text) + float.Parse(((TextBox)gvbon.FindControl("txtwasteqty")).Text)) * float.Parse(((Label)gvbon.FindControl("lblgvUnitPrice")).Text)).ToString());


            }
        }

        if (ProductId.Trim() == "")
        {
            Session["MaterialProductId"] = ProductId;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSerialNumber(string prefixText, int count, string contextKey)
    {
        string[] str = new string[0];

        Inv_StockBatchMaster ObjStockBatchMaster = new Inv_StockBatchMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtstockBatchMaster = new DataTable();


        if (HttpContext.Current.Session["MaterialProductId"] != null)
        {
            //dtstockBatchMaster = new DataView(dtstockBatchMaster, "ProductId in (" + HttpContext.Current.Session["MaterialProductId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            //dtstockBatchMaster = new DataView(dtstockBatchMaster, "ProductId in (" + HttpContext.Current.Session["MaterialProductId"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
            dtstockBatchMaster = ObjStockBatchMaster.GetStockBatchMasterAllForProduction(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LOcId"].ToString(), HttpContext.Current.Session["MaterialProductId"].ToString());
        }
        else
        {
            dtstockBatchMaster = ObjStockBatchMaster.GetStockBatchMasterAllForProduction(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LOcId"].ToString(), "");
        }
        DataTable dtTemp = dtstockBatchMaster.Copy();
        DataTable dt = new DataTable();
        dt = dtTemp;
        //foreach (DataRow dr in dtTemp.Rows)
        //{
        //    if (HttpContext.Current.Session["dtIssueRoll"] != null)
        //    {
        //        if (new DataView((DataTable)HttpContext.Current.Session["dtIssueRoll"], "SerialNo='" + dr["SerialNo"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
        //        {
        //            continue;
        //        }
        //    }
        //    DataTable dtcheck = new DataView(dtstockBatchMaster, "SerialNo='" + dr["SerialNo"].ToString() + "'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();


        //    if (dtcheck.Rows[0]["TransType"].ToString() == "PG" || dtcheck.Rows[0]["TransType"].ToString() == "OP" || dtcheck.Rows[0]["TransType"].ToString() == "FR")
        //    {

        //        dt.Merge(dtcheck);
        //    }
        //}

        dt = new DataView(dt, "SerialNo LIKE '" + prefixText  + "%'", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
        if (dt == null)
        {
            str = new string[0];
        }
        else
        {
            str = new string[dt.Rows.Count];
        }

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                str[i] = dt.Rows[i]["SerialNo"].ToString();
            }
        }
        return str;
    }
    #endregion
    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../Production_Report/ProductionFinishReport.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=ORDER','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    protected void IbtnPrintVoucher_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../Production_Report/ProductionFinishReport.aspx?Id=" + e.CommandArgument.ToString() + "&&Type=VOUCHER','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    protected void IbtnPrintbarcode_Command(object sender, CommandEventArgs e)
    {
        hdnReportId.Value = e.CommandArgument.ToString();
        BarcodeReport();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "openReport()", true);
    }

    public void BarcodeReport()
    {
        if (hdnReportId.Value.Trim() == "")
        {
            return;
        }

        double Totalqty = 0;
        string strProductcode = "0";
        BarcodePrint objbarcode_52_25 = new BarcodePrint(Session["DBConnection"].ToString());
        BarcodePrint_102mm_105mm objbarcode_102_105 = new BarcodePrint_102mm_105mm(Session["DBConnection"].ToString());
        BarcodePrint_60mm_60mm objbarcode_60_60 = new BarcodePrint_60mm_60mm(Session["DBConnection"].ToString());

        DataTable dt = objDa.return_DataTable("select Inv_ProductMaster.ProductCode,(Production_Qty+Extra_Qty) as TotalQty from Inv_Production_Process_Detail inner join Inv_ProductMaster on Inv_Production_Process_Detail.ProductId = Inv_ProductMaster.Productid where Ref_Job_No=" + hdnReportId.Value.Trim() + "");

        if (dt.Rows.Count > 0)
        {
            Totalqty = Convert.ToDouble(dt.Rows[0]["TotalQty"].ToString());
            Totalqty = 1;
            strProductcode = dt.Rows[0]["ProductCode"].ToString();
        }


        DataTable dtFilter = new DataTable();
        dtFilter.Columns.Add("Barcode");

        while (Totalqty > 0)
        {
            DataRow dr = dtFilter.NewRow();
            dr[0] = strProductcode;
            dtFilter.Rows.Add(dr);
            Totalqty = Totalqty - 1;
        }


        //if (hdnReportId.Value == "")
        //{
        //    return;
        //}
        //DataTable dtFilter = new DataTable();
        //AttendanceDataSet rptdata = new AttendanceDataSet();
        //rptdata.EnforceConstraints = false;
        //AttendanceDataSetTableAdapters.sp_Att_HalfDay_Request_ReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_HalfDay_Request_ReportTableAdapter();
        //adp.Fill(rptdata.sp_Att_HalfDay_Request_Report, Convert.ToInt32(hdnPrintTransId.Value), Convert.ToInt32(Session["LocId"].ToString()), Convert.ToInt32(Session["EmpId"].ToString()));
        //dtFilter = rptdata.sp_Att_HalfDay_Request_Report;
        //DataTable dtFilter = new DataTable();
        //dtFilter.Columns.Add("Barcode");
        //DataRow dr = dtFilter.NewRow();
        //dr[0] = "102030405060";
        //dtFilter.Rows.Add(dr);

        //DataRow dr1 = dtFilter.NewRow();
        //dr1[0] = "102030405061";
        //dtFilter.Rows.Add(dr1);
        //DevExpress.XtraReports.UI.XRBarCode Gh = (DevExpress.XtraReports.UI.XRBarCode)objbarcode.FindControl("xrBarCode1", true);
        //Gh.AutoModule = true;
        //Gh.ShowText = true;
        //if (ddlbarcodesize.SelectedIndex == 0)
        //{
        //    Gh.WidthF = 75;
        //    Gh.HeightF = 50;
        //    Gh.Font = new System.Drawing.Font("Times New Roman", 5F, System.Drawing.FontStyle.Bold);
        //}
        //else
        //{
        //    Gh.WidthF = 52;
        //    Gh.HeightF = 50;

        //}


        if (ddlbarcodesize.SelectedIndex == 0)
        {
            DevExpress.XtraReports.UI.XRBarCode Gh = (DevExpress.XtraReports.UI.XRBarCode)objbarcode_52_25.FindControl("xrBarCode1", true);
            Gh.AutoModule = true;
            Gh.ShowText = true;

            objbarcode_52_25.DataSource = dtFilter;
            objbarcode_52_25.DataMember = "dtBarcode";
            objbarcode_52_25.CreateDocument();
            ReportViewer1.OpenReport(objbarcode_52_25);
        }
        else if (ddlbarcodesize.SelectedIndex == 1)
        {
            objbarcode_102_105.DataSource = dtFilter;
            objbarcode_102_105.DataMember = "dtBarcode";
            objbarcode_102_105.CreateDocument();
            ReportViewer1.OpenReport(objbarcode_102_105);
        }
        else if (ddlbarcodesize.SelectedIndex == 2)
        {
            objbarcode_60_60.DataSource = dtFilter;
            objbarcode_60_60.DataMember = "dtBarcode";
            objbarcode_60_60.CreateDocument();
            ReportViewer1.OpenReport(objbarcode_60_60);
        }
        dtFilter.Dispose();
    }


    #endregion

    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
        }
    }

    protected void txtExtraQty_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((TextBox)sender).Parent.Parent;
        try
        {
            Convert.ToDouble(((TextBox)gvRow.FindControl("txtExtraQty")).Text);
        }
        catch
        {
            DisplayMessage("Invalid Quantity");
            ((TextBox)gvRow.FindControl("txtExtraQty")).Text = "0";
        }


        ((Label)gvRow.FindControl("lblLinetotal")).Text = SetDecimal(((Convert.ToDouble(((Label)gvRow.FindControl("lblReqQty")).Text) + Convert.ToDouble(((TextBox)gvRow.FindControl("txtExtraQty")).Text)) * Convert.ToDouble(((Label)gvRow.FindControl("lblunitPrice")).Text)).ToString());
    }
}