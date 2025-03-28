using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.IO;
using System.Data.SqlClient;

public partial class Inventory_SerialAdjustment : System.Web.UI.Page
{
    #region defined Class Object
    Common cmn = null;
    Inv_AdjustHeader objAdjustHeader = null;
    Inv_AdjustDetail objAdjustDetail = null;
    Inv_ProductMaster objProductM = null;
    Inv_UnitMaster UM = null;
    Inv_StockDetail objStockDetail = null;
    SystemParameter ObjSysParam = null;
    UserMaster ObjUser = null;
    Set_DocNumber objDocNo = null;
    LocationMaster LM = null;
    Inv_StockBatchMaster ObjStockBatchMaster = null;
    Inv_ProductLedger ObjProductledger = null;
    Inv_TransferRequestHeader ObjTrans = null;
    PageControlCommon objPageCmn = null;
    string strLocationId = string.Empty;
    #endregion
    protected string FuLogo_UploadFolderPath = "~/Temp/";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        if (Request.QueryString["Id"] != null)
        {

            strLocationId = Request.QueryString["LocId"].ToString();
        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }

        //AllPageCode();
        btnSInquirySave.Attributes.Add("onclick", "this.disabled=true;" + ClientScript.GetPostBackEventReference(btnSInquirySave, "").ToString());

        cmn = new Common(Session["DBConnection"].ToString());
        objAdjustHeader = new Inv_AdjustHeader(Session["DBConnection"].ToString());
        objAdjustDetail = new Inv_AdjustDetail(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        UM = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        LM = new LocationMaster(Session["DBConnection"].ToString());
        ObjStockBatchMaster = new Inv_StockBatchMaster(Session["DBConnection"].ToString());
        ObjProductledger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        ObjTrans = new Inv_TransferRequestHeader(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/SerialAdjustment.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            

            AllPageCode(clsPagePermission);

            ddlOption.SelectedIndex = 2;
            FillGrid();

            txtQuantity.Text = "1";
            txtVoucherNo.Text = GetDocumentNumber(); //updated by jitendra on 28-9-2013
            txtVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            Calender.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtValueDate.Format = Session["DateFormat"].ToString();
            //this code for when we redirect from the producte ledger page 
            //this code created on 22-07-2015
            //code start

            if (Request.QueryString["Id"] != null)
            {
                ImageButton imgeditbutton = new ImageButton();
                imgeditbutton.ID = "btnView";
                btnView_Command(imgeditbutton, new CommandEventArgs("commandName", Request.QueryString["Id"].ToString()));
                //btnList.Visible = false;
                BtnReset.Visible = false;
                btnSInquiryCancel.Visible = false;
                strLocationId = Request.QueryString["LocId"].ToString();
                //((Panel)Master.FindControl("pnlaccordian")).Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Hide()", true);
            }
            else
            {
                //btnList.Visible = true;
                BtnReset.Visible = true;
                btnSInquiryCancel.Visible = true;

                //((Panel)Master.FindControl("pnlaccordian")).Visible = true;
            }


            Session["dtSerial"] = null;

            Session["dtFinal"] = null;
            //code end
            //here we modify code for show cost price according the login user permission
            //code created by jitendra on 13-08-2016
            try
            {
                GvProduct.Columns[7].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
                GvProduct.Columns[8].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
                trcost.Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
                //trNettotal.Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            }
            catch
            {

            }

        }
        //AllPageCode();
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        if (Request.QueryString["Id"] == null)
        {
            btnPostSave.Visible = clsPagePermission.bAdd;
        }
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    public string GetDocumentNumber()
    {

        string s = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), true, "11", "131", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        return s;

    }
    #region System defined Function

    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        btnEdit_Command(sender, (CommandEventArgs)e);
        try
        {
            GvProduct.Columns[0].Visible = false;
            GvProduct.Columns[1].Visible = false;
        }
        catch
        {

        }



        btnSInquirySave.Visible = false;
        btnPostSave.Visible = false;
        btnAddNewProduct.Visible = false;
        Lbl_Tab_New.Text = Resources.Attendance.View;

    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {


        DataTable dtStockEdit = objAdjustHeader.GetAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());


        if (dtStockEdit.Rows.Count > 0)
        {

            if (((LinkButton)sender).ID == "btnEdit")
            {
                if (Convert.ToBoolean(dtStockEdit.Rows[0]["Post"].ToString()))
                {
                    DisplayMessage("Cannot Edit,It has Posted");
                    hdnStockTransId.Value = "";
                    return;
                }
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

            try
            {
                GvProduct.Columns[0].Visible = true;
                GvProduct.Columns[1].Visible = true;
            }
            catch
            {

            }
            hdnStockTransId.Value = e.CommandArgument.ToString();
            // txtToLocation.Text = GetLocationName(dtStockEdit.Rows[0]["ToLocationID"].ToString());
            // hdnToLocationId.Value = strLocationId.ToString();
            //dtStockEdit.Rows[0]["ToLocationID"].ToString();
            txtVoucherNo.Text = dtStockEdit.Rows[0]["VoucherNo"].ToString();
            txtVoucherNo.ReadOnly = true;
            txtVoucherDate.Text = Convert.ToDateTime(dtStockEdit.Rows[0]["Vdate"].ToString()).ToString(ObjSysParam.GetSysParameterByParamName("Date_Format").Rows[0]["Param_Value"].ToString());
            txtRemark.Text = dtStockEdit.Rows[0]["Remark"].ToString();
            txtNetAmount.Text = GetAmountDecimal(dtStockEdit.Rows[0]["NetAmount"].ToString());
            ChkPost.Checked = Convert.ToBoolean(dtStockEdit.Rows[0]["Post"].ToString());

            //for get record from stockbatch mastre table

            DataTable dtTemp = ObjStockBatchMaster.GetStockBatchMasterAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            if (dtTemp.Rows.Count > 0)
            {
                dtTemp = new DataView(dtTemp, "TransType='SA' and TransTypeId=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTemp.Rows.Count > 0)
                {
                    dtTemp = dtTemp.DefaultView.ToTable(false, "ProductId", "SerialNo", "Barcode", "BatchNo", "LotNo", "ExpiryDate", "Field1", "TransType", "TransTypeId", "ManufacturerDate", "Quantity", "Trans_Id", "Width", "Length", "Pallet_ID");

                    Session["dtFinal"] = dtTemp;

                }

            }
            else
            {
                Session["dtFinal"] = null;
            }

            //Add Child Concept
            DataTable dtDetail = objAdjustDetail.GetAllDataByHeaderTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnStockTransId.Value);
            if (dtDetail.Rows.Count > 0)
            {
                //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
                objPageCmn.FillData((object)GvProduct, dtDetail, "", "");


                foreach (GridViewRow gvr in GvProduct.Rows)
                {
                    TextBox lblgvQuantity = (TextBox)gvr.FindControl("txtQuantity");
                    Label lblgvUnitCost = (Label)gvr.FindControl("lblgvUnitCost");
                    Label lblgvTotal = (Label)gvr.FindControl("lblgvTotal");

                    if (lblgvQuantity.Text != "")
                    {
                        if (lblgvUnitCost.Text != "")
                        {
                            lblgvTotal.Text = GetAmountDecimal((float.Parse(lblgvQuantity.Text) * float.Parse(lblgvUnitCost.Text)).ToString());
                        }
                    }
                }
            }
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToLocation);
    }
    protected void GvStockAdjustment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvStockAdjustment.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvStockAdjustment, dt, "", "");

        //AllPageCode();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.SelectedItem.Value == "Vdate")
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
            DataTable dtAdd = (DataTable)Session["dtCurrency"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)GvStockAdjustment, view.ToTable(), "", "");

            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvStockAdjustment_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtFilter"];
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
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvStockAdjustment, dt, "", "");

        //AllPageCode();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        hdnStockTransId.Value = e.CommandArgument.ToString();

        DataTable dtStockEdit = objAdjustHeader.GetAllDataByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnStockTransId.Value);


        if (dtStockEdit.Rows.Count > 0)
        {

            if (Convert.ToBoolean(dtStockEdit.Rows[0]["Post"].ToString()))
            {
                DisplayMessage("Can not Delete,It has Posted");
                hdnStockTransId.Value = "";
                return;
            }
        }
        objAdjustHeader.DeleteAdjustHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), hdnStockTransId.Value, "False", Session["UserId"].ToString(), DateTime.Now.ToString());
        FillGrid();
        Reset();
        //AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValue.Visible = true;
        txtValueDate.Visible = false;
        txtValue.Focus();
        //AllPageCode();

    }
    protected void btnSInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();

        FillGrid();
        //AllPageCode();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        //AllPageCode();
        //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToLocation);
    }
    protected void btnPostSave_Click(object sender, EventArgs e)
    {
        ChkPost.Checked = true;
        btnSInquirySave_Click(sender, e);
        //AllPageCode();
    }
    protected void btnSInquirySave_Click(object sender, EventArgs e)
    {


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        Button btn = (Button)sender;

        if (btn.ID.Trim() == "btnPostSave")
        {
            ChkPost.Checked = true;
        }
        else
        {
            ChkPost.Checked = false;
        }
        if (txtVoucherNo.Text == "")
        {
            DisplayMessage("Enter Voucher No.");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherNo);

            return;
        }
        else
        {

            DataTable dtVoucherNo = objAdjustHeader.GetAllData(Session["CompId"].ToString(), Session["BrandId"].ToString());

            try
            {

                if (hdnStockTransId.Value != "0")
                {
                    dtVoucherNo = new DataView(dtVoucherNo, "VoucherNo='" + txtVoucherNo.Text + "' and TransID<>" + hdnStockTransId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {

                    dtVoucherNo = new DataView(dtVoucherNo, "VoucherNo='" + txtVoucherNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            catch
            {
            }

            dtVoucherNo = new DataView(dtVoucherNo, "VoucherNo='" + txtVoucherNo.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVoucherNo.Rows.Count > 0)
            {
                DisplayMessage("Voucher No. Already Exits");
                txtVoucherNo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherNo);

                return;
            }

        }

        if (txtVoucherDate.Text == "")
        {
            DisplayMessage("Select Date");
            txtVoucherDate.Focus();

            return;
        }
        else
        {
            try
            {
                ObjSysParam.getDateForInput(txtVoucherDate.Text);

            }
            catch
            {
                DisplayMessage("Enter Voucher Date in format " + Session["DateFormat"].ToString() + "");
                txtVoucherDate.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtVoucherDate);

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



        if (GvProduct.Rows.Count == 0)
        {
            DisplayMessage("Enter Product");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);

            return;
        }

        if (txtNetAmount.Text == "")
        {
            DisplayMessage("Get Net Amount");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtNetAmount);

            return;
        }

        string strPost = string.Empty;
        if (ChkPost.Checked == true)
        {
            strPost = "True";
        }
        else if (ChkPost.Checked == false)
        {
            strPost = "False";
        }



        //here we checking serial validation when serial validation parameter is true in inventory pages
        //cpde created on 16-09-2016
        //code created by jitendra upadhyay 


        if (ChkPost.Checked)
        {

            foreach (GridViewRow gvr in GvProduct.Rows)
            {
                double Totalqty = 0;

                Totalqty = Convert.ToDouble(((TextBox)gvr.FindControl("txtQuantity")).Text);

                if (Totalqty > 0)
                {
                    if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ((Label)gvr.FindControl("lblgvProductId")).Text, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
                    {
                        DataTable dt = (DataTable)Session["dtFinal"];
                        if (dt == null)
                        {
                            DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblGvProductCode")).Text);
                            return;
                        }

                        dt = new DataView(dt, "ProductId='" + ((Label)gvr.FindControl("lblgvProductId")).Text + "' ", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt.Rows.Count == 0)
                        {
                            DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblGvProductCode")).Text);
                            return;
                        }

                        if (dt.Rows.Count != Totalqty)
                        {
                            DisplayMessage("Serial Information is not available for Product code = " + ((Label)gvr.FindControl("lblGvProductCode")).Text);
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

            if (hdnStockTransId.Value != "0")
            {
                objAdjustHeader.UpdateAdjustHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strLocationId, hdnStockTransId.Value, txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), txtRemark.Text, txtNetAmount.Text, strPost, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                //Add Detail Section.
                objAdjustDetail.DeleteAdjustDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnStockTransId.Value, ref trns);

                foreach (GridViewRow gvr in GvProduct.Rows)
                {
                    Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                    Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                    Label lblgvUnitId = (Label)gvr.FindControl("lblgvUnitId");
                    TextBox lblgvQuantity = (TextBox)gvr.FindControl("txtQuantity");
                    Label lblgvUnitCost = (Label)gvr.FindControl("lblgvUnitCost");
                    HiddenField hdngvInOut = (HiddenField)gvr.FindControl("hdnInOutValue");


                    objAdjustDetail.InsertAdjustDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnStockTransId.Value, txtVoucherNo.Text, lblgvSerialNo.Text, lblgvProductId.Text, lblgvUnitId.Text, lblgvQuantity.Text, lblgvUnitCost.Text, hdngvInOut.Value, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    //here we insert record in stock batch amster table

                    //code start
                    if (ChkPost.Checked)
                    {
                        ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", hdnStockTransId.Value, lblgvProductId.Text, ref trns);

                        if (Session["dtFinal"] != null)
                        {
                            DataTable dt = (DataTable)Session["dtFinal"];
                            dt = new DataView(dt, "ProductId='" + lblgvProductId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (hdngvInOut.Value.Trim() == "I")
                                {
                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", hdnStockTransId.Value, lblgvProductId.Text, lblgvUnitId.Text, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), "0", dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                                if (hdngvInOut.Value.Trim() == "O")
                                {
                                    ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", hdnStockTransId.Value, lblgvProductId.Text, lblgvUnitId.Text, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), "0", dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                }

                            }
                        }

                        //code end
                        //if (hdngvInOut.Value.Trim() == "I")
                        //{
                        //    ObjProductledger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), strLocationId.ToString(), "SA", hdnStockTransId.Value.ToString(), strLocationId, lblgvProductId.Text, lblgvUnitId.Text, "I", "0", "0", lblgvQuantity.Text.Trim(), "0", "1/1/1800", lblgvUnitCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        //}
                        //else
                        //{
                        //    if (hdngvInOut.Value.Trim() == "O")
                        //    {
                        //        ObjProductledger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), strLocationId.ToString(), "SA", hdnStockTransId.Value.ToString(), strLocationId, lblgvProductId.Text, lblgvUnitId.Text, "O", "0", "0", "0", lblgvQuantity.Text.Trim(), "1/1/1800", lblgvUnitCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                        //    }
                        //}
                    }
                }
                //End 
                if (ChkPost.Checked)
                {
                    DisplayMessage("Record posted successfully");

                }
                else
                {
                    DisplayMessage("Record Updated", "green");
                }

                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {


                //DataTable dtMaxId = objAdjustHeader.GetAllDataMaxTransId(Session["CompId"].ToString(), Session["BrandId"].ToString());


                b = objAdjustHeader.InsertAdjustHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strLocationId, txtVoucherNo.Text, ObjSysParam.getDateForInput(txtVoucherDate.Text).ToString(), txtRemark.Text, txtNetAmount.Text, strPost, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                string strMaxId = string.Empty;
                if (b != 0)
                {
                    DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
                    string sql = "select * from Inv_AdjustHeader where CompanyId=" + Session["CompId"].ToString() + " and BrandId=" + Session["BrandId"].ToString() + " and FromLocationID=" + Session["LocId"].ToString() + "";
                    DataTable dtCount = da.return_DataTable(sql, ref trns);
                    if (dtCount.Rows.Count == 0)
                    {
                        objAdjustHeader.Updatecode(b.ToString(), txtVoucherNo.Text + "1", ref trns);
                        txtVoucherNo.Text = txtVoucherNo.Text + "1";
                    }
                    else
                    {
                        objAdjustHeader.Updatecode(b.ToString(), txtVoucherNo.Text + dtCount.Rows.Count, ref trns);
                        txtVoucherNo.Text = txtVoucherNo.Text + dtCount.Rows.Count;
                    }
                    strMaxId = b.ToString();
                    hdnStockTransId.Value = strMaxId;

                    //Add Detail Section.
                    objAdjustDetail.DeleteAdjustDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, hdnStockTransId.Value, ref trns);

                    foreach (GridViewRow gvr in GvProduct.Rows)
                    {
                        Label lblgvSerialNo = (Label)gvr.FindControl("lblSNo");
                        Label lblgvProductId = (Label)gvr.FindControl("lblgvProductId");
                        Label lblgvUnitId = (Label)gvr.FindControl("lblgvUnitId");
                        TextBox lblgvQuantity = (TextBox)gvr.FindControl("txtQuantity");
                        Label lblgvUnitCost = (Label)gvr.FindControl("lblgvUnitCost");
                        HiddenField hdngvInOut = (HiddenField)gvr.FindControl("hdnInOutValue");

                        objAdjustDetail.InsertAdjustDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strMaxId, txtVoucherNo.Text, lblgvSerialNo.Text, lblgvProductId.Text, lblgvUnitId.Text, lblgvQuantity.Text, lblgvUnitCost.Text, hdngvInOut.Value, "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                        //here we insert record in stock batch amster table

                        //code start
                        if (ChkPost.Checked)
                        {
                            ObjStockBatchMaster.DeleteStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", hdnStockTransId.Value, lblgvProductId.Text, ref trns);

                            if (Session["dtFinal"] != null)
                            {
                                DataTable dt = (DataTable)Session["dtFinal"];
                                dt = new DataView(dt, "ProductId='" + lblgvProductId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (hdngvInOut.Value.Trim() == "I")
                                    {
                                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", hdnStockTransId.Value, lblgvProductId.Text, lblgvUnitId.Text, "I", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), "0", dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                    }
                                    if (hdngvInOut.Value.Trim() == "O")
                                    {
                                        ObjStockBatchMaster.InsertStockBatchMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "SA", hdnStockTransId.Value, lblgvProductId.Text, lblgvUnitId.Text, "O", dr["LotNo"].ToString(), dr["BatchNo"].ToString(), dr["Quantity"].ToString(), dr["ExpiryDate"].ToString(), dr["SerialNo"].ToString().Trim(), dr["ManufacturerDate"].ToString(), "0", dr["Width"].ToString(), dr["Length"].ToString(), dr["Pallet_ID"].ToString(), "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                    }

                                }
                            }
                            //code end


                            //if (hdngvInOut.Value.Trim() == "I")
                            //{
                            //    ObjProductledger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), strLocationId.ToString(), "SA", hdnStockTransId.Value.ToString(), strLocationId.ToString(), lblgvProductId.Text, lblgvUnitId.Text, "I", "0", "0", lblgvQuantity.Text.Trim(), "0", "1/1/1800", lblgvUnitCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                            //    //ObjProductledger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), hdnToLocationId.Value.ToString(), "SA", hdnStockTransId.Value.ToString(), strLocationId.Trim(), lblgvProductId.Text, lblgvUnitId.Text, "O", "0", "0", "0", lblgvQuantity.Text.Trim(), "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                            //}
                            //else
                            //{
                            //    if (hdngvInOut.Value.Trim() == "O")
                            //    {
                            //        ObjProductledger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), strLocationId.ToString(), "SA", hdnStockTransId.Value.ToString(), strLocationId.ToString(), lblgvProductId.Text, lblgvUnitId.Text, "O", "0", "0", "0", lblgvQuantity.Text.Trim(), "1/1/1800", lblgvUnitCost.Text, "1/1/1800", "0", "1/1/1800", "0", "0", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), "1/1/1800", Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);
                            //        // ObjProductledger.InsertProductLedger(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), hdnToLocationId.Value.ToString(), "SA", hdnStockTransId.Value.ToString(), strLocationId.Trim(), lblgvProductId.Text, lblgvUnitId.Text, "I", "0", "0", lblgvQuantity.Text.Trim(), "0", "1/1/1800", "0", "1/1/1800", "0", "1/1/1800", "0", "", "", "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString().ToString(), "1/1/1800", Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

                            //    }
                            //}
                        }
                    }
                    //End  
                }

                if (ChkPost.Checked)
                {
                    DisplayMessage("Record posted successfully");

                }
                else
                {
                    DisplayMessage("Record Saved","green");
                }
            }
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid();
            Reset();
            //AllPageCode();
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


    #endregion

    #region User defined Function
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
    private void FillGrid()
    {

        string PostStatus = string.Empty;
        if (ddlPosted.SelectedItem.Value == "Posted")
        {
            PostStatus = " Post='True'";
        }
        if (ddlPosted.SelectedItem.Value == "UnPosted")
        {
            PostStatus = " Post='False'";
        }

        DataTable dtBrand = new DataView(objAdjustHeader.GetAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), PostStatus, "TransId desc", DataViewRowState.CurrentRows).ToTable();

        //DataTable dtBrand = objAdjustHeader.GetAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtCurrency"] = dtBrand;
        ViewState["dtFilter"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)GvStockAdjustment, dtBrand, "", "");
        }
        else
        {
            GvStockAdjustment.DataSource = null;
            GvStockAdjustment.DataBind();
        }
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count.ToString() + "";
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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
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
    public void Reset()
    {
        FillGrid();
        txtVoucherNo.ReadOnly = false;
        txtVoucherNo.Text = GetDocumentNumber();
        txtVoucherDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtNetAmount.Text = "";
        txtRemark.Text = "";
        ChkPost.Checked = false;

        GvProduct.DataSource = null;
        GvProduct.DataBind();

        hdnStockTransId.Value = "0";

        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        try
        {
            GvProduct.Columns[0].Visible = true;
            GvProduct.Columns[1].Visible = true;
        }
        catch
        {

        }
        Session["dtFinal"] = null;


        //if (Session["EmpId"].ToString() == "0")
        //{

        //    btnSInquirySave.Visible = true;
        //    btnPostSave.Visible = true;
        //    btnAddNewProduct.Visible = true;
        //}
        //else
        //{
            //DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), ViewState["ModuelId"].ToString(), "131", HttpContext.Current.Session["CompId"].ToString());
            //try
            //{
            //    dtAllPageCode = new DataView(dtAllPageCode, "Op_Id=1", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //catch
            //{
            //}
            //if (dtAllPageCode.Rows.Count > 0)
            //{
            //    btnSInquirySave.Visible = true;
            //    btnPostSave.Visible = true;
            //    btnAddNewProduct.Visible = true;
            //}
        //}
        ResetProduct();


    }
    #endregion

    #region Invoice Section
    protected string GetProductName(string strProductId)
    {
        string strProductName = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductName = dtPName.Rows[0]["EProductName"].ToString();
            }
        }
        else
        {
            strProductName = "";
        }
        return strProductName;
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
            ProductName = "0";


        }

        return ProductName;

    }
    protected string GetProductDescription(string strProductId)
    {
        string strProductDescription = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductDescription = dtPName.Rows[0]["Description"].ToString();
            }
        }
        else
        {
            strProductDescription = "";
        }
        return strProductDescription;
    }
    protected string GetLocationName(string strLocationId)
    {
        string strLocationName = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLName = LM.GetLocationMasterById(Session["CompId"].ToString(), strLocationId);
            if (dtLName.Rows.Count > 0)
            {
                strLocationName = dtLName.Rows[0]["Location_Name"].ToString();
            }
        }
        else
        {
            strLocationName = "";
        }
        return strLocationName;
    }
    protected string GetUnitName(string strUnitId)
    {
        string strUnitName = string.Empty;
        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = UM.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
            if (dtUName.Rows.Count > 0)
            {
                strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
            }
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }
    protected string GetType(string strTypeId)
    {
        string strTypeName = string.Empty;
        if (strTypeId != "")
        {
            if (strTypeId == "I")
            {
                strTypeName = "In";
            }
            else if (strTypeId == "O")
            {
                strTypeName = "Out";
            }
            else if (strTypeId == "In")
            {
                strTypeName = "In";
            }
            else if (strTypeId == "Out")
            {
                strTypeName = "Out";
            }
        }
        else
        {
            strTypeName = "";
        }
        return strTypeName;
    }
    #endregion

    #region Add Product Concept
    private void FillUnit(string ProductId)
    {

        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());

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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListLocation(string prefixText, int count, string contextKey)
    {
        LocationMaster LM = new LocationMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = LM.GetDistinctLocation("1", prefixText);
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Location_Name"].ToString();
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
                dt = LM.GetLocationMaster("1");
                if (dt.Rows.Count > 0)
                {
                    str = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        str[i] = dt.Rows[i]["Location_Name"].ToString();
                    }
                }
            }
        }
        return str;
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            DataTable dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, txtProductName.Text.ToString());

            if (dtProduct == null)
            {
                DisplayMessage("Product not found");
                txtProductName.Focus();
                return;
            }
            // dtProduct = new DataView(dtProduct, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtProduct.Rows.Count > 0)
            {
                if (dtProduct.Rows[0]["ItemType"].ToString() == "NS")
                {
                    DisplayMessage("Product type is Non Stockable");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductName.Focus();
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Product_Id");
                for (int i = 0; i < GvProduct.Rows.Count; i++)
                {
                    dt.Rows.Add(i);
                    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                    dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Product_Id"].ToString() == dtProduct.Rows[0]["ProductId"].ToString())
                    {

                        DisplayMessage("Product is already exists!");
                        txtProductName.Text = "";
                        txtProductcode.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
                        return;
                    }
                }
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                FillUnit(dtProduct.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                pnlPDescription.Visible = true;
                txtUnitCost.Text = GetAverageCost(dtProduct.Rows[0]["ProductId"].ToString());
                txtUnitCost_TextChanged(null, null);

            }
            else
            {
                txtProductName.Text = "";
                txtProductcode.Text = "";
                FillUnit("0");
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
                return;
            }
        }
        else
        {
            FillUnit("0");
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            DataTable dtProduct = objProductM.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, txtProductcode.Text.ToString());
            if (dtProduct == null)
            {
                DisplayMessage("Product not found");
                txtProductcode.Focus();
                return;
            }
            // dtProduct = new DataView(dtProduct, "ProductCode ='" + txtProductcode.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtProduct != null && dtProduct.Rows.Count > 0)
            {
                if (dtProduct.Rows[0]["ItemType"].ToString() == "NS")
                {
                    DisplayMessage("Product type is Non Stockable");
                    txtProductName.Text = "";
                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("Product_Id");
                for (int i = 0; i < GvProduct.Rows.Count; i++)
                {
                    dt.Rows.Add(i);
                    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                    dt.Rows[i]["Product_Id"] = lblgvProductId.Text;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["Product_Id"].ToString() == dtProduct.Rows[0]["ProductId"].ToString())
                    {

                        DisplayMessage("Product is already exists!");
                        txtProductName.Text = "";
                        txtProductcode.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
                        return;
                    }
                }
                txtProductName.Text = dtProduct.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dtProduct.Rows[0]["ProductCode"].ToString();
                FillUnit(dtProduct.Rows[0]["ProductId"].ToString());
                txtPDescription.Text = dtProduct.Rows[0]["Description"].ToString();
                hdnNewProductId.Value = dtProduct.Rows[0]["ProductId"].ToString();
                pnlPDescription.Visible = true;
                txtUnitCost.Text = GetAverageCost(dtProduct.Rows[0]["ProductId"].ToString());
                txtUnitCost_TextChanged(null, null);
            }
            else
            {
                FillUnit("0");
                txtProductName.Text = "";
                txtProductcode.Text = "";
                DisplayMessage("Choose In Suggestions Only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
                return;
            }
        }
        else
        {
            FillUnit("0");
            DisplayMessage("Enter Product Id");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
    }
    //add function for get average cost on

    public string GetAverageCost(string strProductId)
    {

        string avgCost = string.Empty;
        try
        {
            avgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString(), strProductId).Rows[0]["Field2"].ToString();
        }
        catch
        {
            avgCost = "0";
        }


        if (avgCost == "")
        {
            avgCost = "0.000";
        }

        return GetAmountDecimal(avgCost);
    }

    #region DecimlaFormat
    public string GetAmountDecimal(string Amount)
    {

        return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);

    }
    #endregion


    //Function to Check Whether there is Opening Stock for Particular Product or Not
    //created by Varsha Surana
    private bool CheckOpeningStockRow(string pid)
    {
        int st1;
        st1 = objStockDetail.GetProductOpeningStockStatus(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, pid);
        if (st1 > 0)
            return true;
        else
            return false;
    }
    //End Function
    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        string Description = string.Empty;
        if (txtProductName.Text != "")
        {
            if (hdnNewProductId.Value == "0")
            {
                if (txtProductName.Text != "")
                {
                    DataTable dt = objProductM.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                    dt = new DataView(dt, "EProductName ='" + txtProductName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        hdnNewProductId.Value = dt.Rows[0]["ProductId"].ToString();
                    }
                    else
                    {
                        hdnNewProductId.Value = "0";
                    }
                }
            }

            if (ddlUnit == null)
            {
                DisplayMessage("Select Unit Name");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlUnit);
                return;
            }
            else
            {
                hdnUnitId.Value = ddlUnit.SelectedValue;
            }


            if (txtQuantity.Text != "")
            {

            }
            else
            {
                DisplayMessage("Enter Quantity");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQuantity);
                return;
            }

            if (txtUnitCost.Text != "")
            {
                float flTemp = 0;
                if (float.TryParse(txtUnitCost.Text, out flTemp))
                {

                }
                else
                {
                    txtUnitCost.Text = "";
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtUnitCost);
                    return;
                }
            }
            else
            {
                txtUnitCost.Text = "0";
            }

            if (ddlTypeOfAdjustment.SelectedValue != "0")
            {

            }
            else
            {
                DisplayMessage("Select Type Of Adjustment");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlTypeOfAdjustment);
                return;
            }


            if (hdnProductId.Value == "")
            {
                FillProductChidGird("Save");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);

            }
            else
            {
                if (txtProductName.Text == hdnProductName.Value)
                {
                    FillProductChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);

                }
                else
                {
                    FillProductChidGird("Edit");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnAddNewProduct);

                }
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetProduct();
    }
    public void ResetProduct()
    {
        txtProductName.Text = "";
        ddlUnit.Items.Clear();
        txtPDescription.Text = "";
        txtQuantity.Text = "1";
        txtUnitCost.Text = "";
        txtTotal.Text = "";
        ddlTypeOfAdjustment.SelectedValue = "0";
        hdnProductId.Value = "";
        hdnProductName.Value = "";
        hdnNewProductId.Value = "0";
        txtProductcode.Text = "";
        txtProductcode.Focus();
    }
    public DataTable CreateProductDataTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("SerialNo");
        dt.Columns.Add("ProductId");
        dt.Columns.Add("UnitID");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("UnitCost");
        dt.Columns.Add("InOut");
        return dt;
    }
    public DataTable FillProductDataTabel()
    {
        string strNewSNo = string.Empty;
        DataTable dt = CreateProductDataTable();
        if (GvProduct.Rows.Count > 0)
        {
            for (int i = 0; i < GvProduct.Rows.Count + 1; i++)
            {
                if (dt.Rows.Count != GvProduct.Rows.Count)
                {
                    dt.Rows.Add(i);
                    Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
                    Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
                    Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
                    TextBox lblgvQuantity = (TextBox)GvProduct.Rows[i].FindControl("txtQuantity");
                    Label lblgvUnitCost = (Label)GvProduct.Rows[i].FindControl("lblgvUnitCost");
                    HiddenField hdngvInOut = (HiddenField)GvProduct.Rows[i].FindControl("hdnInOutValue");

                    dt.Rows[i]["SerialNo"] = lblgvSNo.Text;
                    strNewSNo = lblgvSNo.Text;
                    dt.Rows[i]["ProductId"] = lblgvProductId.Text;
                    dt.Rows[i]["UnitID"] = lblgvUnitId.Text;
                    dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
                    dt.Rows[i]["UnitCost"] = lblgvUnitCost.Text;
                    dt.Rows[i]["InOut"] = hdngvInOut.Value;
                }
                else
                {
                    dt.Rows.Add(i);
                    dt.Rows[i]["SerialNo"] = (float.Parse(strNewSNo) + 1).ToString();
                    dt.Rows[i]["ProductId"] = hdnNewProductId.Value;
                    dt.Rows[i]["UnitID"] = hdnUnitId.Value;
                    dt.Rows[i]["Quantity"] = txtQuantity.Text;
                    dt.Rows[i]["UnitCost"] = txtUnitCost.Text;
                    dt.Rows[i]["InOut"] = ddlTypeOfAdjustment.SelectedValue;
                }
            }
        }
        else
        {
            dt.Rows.Add(0);
            dt.Rows[0]["SerialNo"] = "1";
            dt.Rows[0]["ProductId"] = hdnNewProductId.Value;
            dt.Rows[0]["UnitID"] = hdnUnitId.Value;
            dt.Rows[0]["Quantity"] = txtQuantity.Text;
            dt.Rows[0]["UnitCost"] = txtUnitCost.Text;
            dt.Rows[0]["InOut"] = ddlTypeOfAdjustment.SelectedValue;
        }
        if (dt.Rows.Count > 0)
        {   //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)GvProduct, dt, "", "");

        }
        return dt;
    }
    public DataTable FillProductDataTabelDelete()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            TextBox lblgvQuantity = (TextBox)GvProduct.Rows[i].FindControl("txtQuantity");

            Label lblgvUnitCost = (Label)GvProduct.Rows[i].FindControl("lblgvUnitCost");
            HiddenField hdngvInOut = (HiddenField)GvProduct.Rows[i].FindControl("hdnInOutValue");

            dt.Rows[i]["SerialNo"] = lblgvSNo.Text;
            dt.Rows[i]["ProductId"] = lblgvProductId.Text;
            dt.Rows[i]["UnitID"] = lblgvUnitId.Text;
            dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dt.Rows[i]["UnitCost"] = lblgvUnitCost.Text;
            dt.Rows[i]["InOut"] = hdngvInOut.Value;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "SerialNo<>'" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        return dt;
    }
    protected void imgBtnProductEdit_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductDataTabelEdit();

        txtProductcode.Focus();
    }
    public DataTable FillProductDataTabelEdit()
    {
        DataTable dt = CreateProductDataTable();

        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblgvSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            TextBox lblgvQuantity = (TextBox)GvProduct.Rows[i].FindControl("txtQuantity");
            Label lblgvUnitCost = (Label)GvProduct.Rows[i].FindControl("lblgvUnitCost");
            HiddenField hdngvInOut = (HiddenField)GvProduct.Rows[i].FindControl("hdnInOutValue");

            dt.Rows[i]["SerialNo"] = lblgvSNo.Text;
            dt.Rows[i]["ProductId"] = lblgvProductId.Text;
            dt.Rows[i]["UnitID"] = lblgvUnitId.Text;
            dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dt.Rows[i]["UnitCost"] = lblgvUnitCost.Text;
            dt.Rows[i]["InOut"] = hdngvInOut.Value;
        }

        DataView dv = new DataView(dt);
        dv.RowFilter = "SerialNo='" + hdnProductId.Value + "'";
        dt = (DataTable)dv.ToTable();
        if (dt.Rows.Count != 0)
        {
            txtProductName.Text = GetProductName(dt.Rows[0]["ProductId"].ToString());
            txtProductcode.Text = ProductCode(dt.Rows[0]["ProductId"].ToString());

            FillUnit(dt.Rows[0]["ProductId"].ToString());
            txtPDescription.Text = GetProductDescription(dt.Rows[0]["ProductId"].ToString());
            ddlUnit.SelectedValue = dt.Rows[0]["UnitID"].ToString();
            txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
            txtUnitCost.Text = dt.Rows[0]["UnitCost"].ToString();
            if (txtQuantity.Text != "")
            {
                if (txtUnitCost.Text != "")
                {
                    txtTotal.Text = (float.Parse(txtQuantity.Text) * float.Parse(txtUnitCost.Text)).ToString();
                }
            }
            ddlTypeOfAdjustment.SelectedValue = dt.Rows[0]["InOut"].ToString();
            hdnProductName.Value = GetProductName(dt.Rows[0]["ProductId"].ToString());
        }
        return dt;
    }
    protected void imgBtnProductDelete_Command(object sender, CommandEventArgs e)
    {
        hdnProductId.Value = e.CommandArgument.ToString();
        FillProductChidGird("Del");
    }
    public void FillProductChidGird(string CommandName)
    {
        DataTable dt = new DataTable();
        if (CommandName.ToString() == "Del")
        {
            dt = FillProductDataTabelDelete();
        }
        else if (CommandName.ToString() == "Edit")
        {
            dt = FillProductDataTableUpdate();
        }
        else
        {
            dt = FillProductDataTabel();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)GvProduct, dt, "", "");


        float fGrossTotal = 0.00f;
        foreach (GridViewRow gvr in GvProduct.Rows)
        {
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("txtQuantity");
            Label lblgvUnitCost = (Label)gvr.FindControl("lblgvUnitCost");
            Label lblgvTotal = (Label)gvr.FindControl("lblgvTotal");

            if (lblgvQuantity.Text != "")
            {
                if (lblgvUnitCost.Text != "")
                {
                    lblgvTotal.Text = GetAmountDecimal((float.Parse(lblgvQuantity.Text) * float.Parse(lblgvUnitCost.Text)).ToString());
                }
            }
            if (lblgvTotal.Text != "")
            {
                fGrossTotal = fGrossTotal + float.Parse(lblgvTotal.Text);
            }
        }
        txtNetAmount.Text = GetAmountDecimal(fGrossTotal.ToString());
        ResetProduct();
    }
    public DataTable FillProductDataTableUpdate()
    {
        DataTable dt = CreateProductDataTable();
        for (int i = 0; i < GvProduct.Rows.Count; i++)
        {
            dt.Rows.Add(i);
            Label lblSNo = (Label)GvProduct.Rows[i].FindControl("lblSNo");
            Label lblgvProductId = (Label)GvProduct.Rows[i].FindControl("lblgvProductId");
            Label lblgvUnitId = (Label)GvProduct.Rows[i].FindControl("lblgvUnitId");
            TextBox lblgvQuantity = (TextBox)GvProduct.Rows[i].FindControl("txtQuantity");
            Label lblgvUnitCost = (Label)GvProduct.Rows[i].FindControl("lblgvUnitCost");
            HiddenField hdngvInOut = (HiddenField)GvProduct.Rows[i].FindControl("hdnInOutValue");

            dt.Rows[i]["SerialNo"] = lblSNo.Text;
            dt.Rows[i]["ProductId"] = lblgvProductId.Text;
            dt.Rows[i]["UnitID"] = lblgvUnitId.Text;
            dt.Rows[i]["Quantity"] = lblgvQuantity.Text;
            dt.Rows[i]["UnitCost"] = lblgvUnitCost.Text;
            dt.Rows[i]["InOut"] = hdngvInOut.Value;
        }
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (hdnProductId.Value == dt.Rows[i]["SerialNo"].ToString())
            {
                dt.Rows[i]["ProductId"] = hdnNewProductId.Value;
                dt.Rows[i]["UnitID"] = hdnUnitId.Value;
                dt.Rows[i]["Quantity"] = txtQuantity.Text;
                dt.Rows[i]["UnitCost"] = txtUnitCost.Text;
                dt.Rows[i]["InOut"] = ddlTypeOfAdjustment.SelectedValue;
            }
        }
        return dt;
    }
    protected void txtQuantity_TextChanged(object sender, EventArgs e)
    {
        if (txtQuantity.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtQuantity.Text, out flTemp))
            {
                if (txtUnitCost.Text != "")
                {
                    txtTotal.Text = (float.Parse(txtQuantity.Text) * float.Parse(txtUnitCost.Text)).ToString();
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlTypeOfAdjustment);
                }
                else
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtUnitCost);
                }
            }
            else
            {
                txtQuantity.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQuantity);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Quantity');", true);
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQuantity);
            return;
        }
    }
    protected void txtUnitCost_TextChanged(object sender, EventArgs e)
    {
        if (txtUnitCost.Text != "")
        {
            float flTemp = 0;
            if (float.TryParse(txtUnitCost.Text, out flTemp))
            {
                if (txtQuantity.Text != "")
                {
                    txtTotal.Text = (float.Parse(txtQuantity.Text) * float.Parse(txtUnitCost.Text)).ToString();

                    txtTotal.Text = GetAmountDecimal(txtTotal.Text);
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlTypeOfAdjustment);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Quantity');", true);
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtQuantity);
                    return;
                }
            }
            else
            {
                txtUnitCost.Text = "";
                ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter Numeric Value Only');", true);
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtUnitCost);
                return;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.alert('Enter UnitCost');", true);
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtUnitCost);
            return;
        }
    }
    protected void btnAddNewProduct_Click(object sender, EventArgs e)
    {
        ResetProduct();
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
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

    #region Serial Number


    protected void BtnSerialSave_Click(object sender, EventArgs e)
    {
        string TransId = string.Empty;
        string DuplicateserialNo = string.Empty;
        string serialNoExists = string.Empty;
        string alreadyout = string.Empty;
        string SerialExist = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        if (txtSerialNo.Text.Trim() != "")
        {
            string[] txt = txtSerialNo.Text.Split('\n');
            int counter = 0;
            if (Session["dtSerial"] == null)
            {
                dt = new DataTable();
                dt = CreateProductDatatable();
                for (int i = 0; i < txt.Length; i++)
                {
                    if (txt[i].ToString().Trim() != "")
                    {
                        if ((new DataView(dt, "SerialNo='" + txt[i].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count) == 0)
                        {
                            string[] result = isSerialNumberValid_Openingstock(txt[i].ToString().Trim(), Session["PID"].ToString(), hdnStockTransId.Value, gvSerialNumber);
                            if (result[0] == "VALID")
                            {
                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dr[0] = Session["PID"].ToString();
                                dr[1] = txt[i].ToString().Trim();
                                dr[2] = "0";
                                dr[3] = "0";
                                dr[4] = "0";
                                dr[5] = DateTime.Now.ToString();
                                dr[6] = "0";
                                dr[7] = "0";
                                dr[8] = "0";
                                dr[9] = DateTime.Now.ToString();
                                dr[10] = "1";
                                counter++;

                            }
                            else if (result[0].ToString() == "DUPLICATE")
                            {
                                DuplicateserialNo += txt[i].ToString().Trim() + ",";
                            }
                            else if (result[0].ToString() == "NOT EXISTS")
                            {
                                serialNoExists += txt[i].ToString().Trim() + ",";
                            }
                            else if (result[0].ToString() == "ALREADY OUT")
                            {
                                alreadyout += txt[i].ToString().Trim() + ",";
                            }
                            else if (result[0].ToString() == "SERIAL_MISSMATCH")
                            {
                                SerialExist += txt[i].ToString().Trim() + ",";
                            }
                        }
                        else
                        {
                            DuplicateserialNo += txt[i].ToString().Trim() + ",";

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
                            string[] result = isSerialNumberValid_Openingstock(txt[i].ToString().Trim(), Session["PID"].ToString(), hdnStockTransId.Value, gvSerialNumber);
                            if (result[0] == "VALID")
                            {

                                DataRow dr = dt.NewRow();
                                dt.Rows.Add(dr);
                                dr[0] = Session["PID"].ToString();
                                dr[1] = txt[i].ToString().Trim();
                                dr[2] = "0";
                                dr[3] = "0";
                                dr[4] = "0";
                                dr[5] = DateTime.Now.ToString();
                                dr[6] = "0";
                                dr[7] = "OP";
                                dr[8] = "0";
                                dr[9] = DateTime.Now.ToString();
                                dr[10] = "1";
                                counter++;

                            }
                            else if (result[0].ToString() == "DUPLICATE")
                            {
                                DuplicateserialNo += txt[i].ToString().Trim() + ",";
                            }
                            else if (result[0].ToString() == "NOT EXISTS")
                            {
                                serialNoExists += txt[i].ToString().Trim() + ",";
                            }
                            else if (result[0].ToString() == "ALREADY OUT")
                            {
                                alreadyout += txt[i].ToString().Trim() + ",";
                            }
                            else if (result[0].ToString() == "SERIAL_MISSMATCH")
                            {
                                SerialExist += txt[i].ToString().Trim() + ",";
                            }
                        }
                        else
                        {
                            DuplicateserialNo += txt[i].ToString().Trim() + ",";
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
            Message += "Following serial Number is Already Exists=" + DuplicateserialNo;

        }
        if (serialNoExists != "")
        {

            Message += " Following serial Number not Exists in stock=" + serialNoExists;
        }

        if (SerialExist != "")
        {
            Message += " Serial number already exist with another Product=" + SerialExist;
        }


        if (alreadyout != "")
        {

            Message += " Following serial Number already out from stock=" + alreadyout;
        }

        if (Message != "")
        {
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

            Dtfinal = new DataView(Dtfinal, "ProductId<>'" + Session["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


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

        if (Session["dtSerial"] != null)
        {
            ((TextBox)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = QtyCount.ToString();
        }
        else
        {
            ((TextBox)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = "0";

        }



        //for calculation in product detail gridview for get update net amount


        if (((Label)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("lblgvUnitCost")).Text == "")
        {
            ((Label)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("lblgvUnitCost")).Text = "0";
        }

        ((Label)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("lblgvTotal")).Text = GetAmountDecimal((Convert.ToDouble(((TextBox)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text) * Convert.ToDouble(((Label)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("lblgvUnitCost")).Text)).ToString());



        float fGrossTotal = 0.00f;
        foreach (GridViewRow gvr in GvProduct.Rows)
        {
            TextBox lblgvQuantity = (TextBox)gvr.FindControl("txtQuantity");
            Label lblgvUnitCost = (Label)gvr.FindControl("lblgvUnitCost");
            Label lblgvTotal = (Label)gvr.FindControl("lblgvTotal");

            if (lblgvQuantity.Text != "")
            {
                if (lblgvUnitCost.Text != "")
                {
                    lblgvTotal.Text = GetAmountDecimal((float.Parse(lblgvQuantity.Text) * float.Parse(lblgvUnitCost.Text)).ToString());
                }
            }
            if (lblgvTotal.Text != "")
            {
                fGrossTotal = fGrossTotal + float.Parse(lblgvTotal.Text);
            }
        }
        txtNetAmount.Text = GetAmountDecimal(fGrossTotal.ToString());


        txtSerialNo.Text = "";
        txtCount.Text = "0";
        txtSerialNo.Focus();

    }
    public static string[] isSerialNumberValid_Openingstock(string serialNumber, string ProductId, string TransId, GridView gvSerialNumber)
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

            DataTable dtserial = ObjStockBatchMaster.GetStockBatchMaster_By_ProductId_and_SerialNo(ProductId, serialNumber);

            if (HttpContext.Current.Session["InOut"].ToString() == "I")
            {
                if (dtserial.Rows.Count == 0)
                {
                    Result[0] = "VALID";
                }
                else if (dtserial.Rows[0]["InOut"].ToString().Trim() == "I")
                {
                    Result[0] = "DUPLICATE";
                }
                else if (dtserial.Rows[0]["InOut"].ToString().Trim() == "O")
                {
                    Result[0] = "VALID";
                }


                DataTable dtStockBatch = ObjStockBatchMaster.GetStockBatchMaster_By_SerialNo(serialNumber);

                if (dtStockBatch.Rows.Count > 0)
                {

                    dtStockBatch = new DataView(dtStockBatch, "ProductId<>" + ProductId + "", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtStockBatch.Rows.Count > 0)
                    {
                        Result[0] = "SERIAL_MISSMATCH";

                    }

                }
            }
            else
            {
                if (dtserial.Rows.Count == 0)
                {
                    Result[0] = "NOT EXISTS";
                }
                else if (dtserial.Rows[0]["InOut"].ToString().Trim() == "O")
                {
                    Result[0] = "ALREADY OUT";
                }
                else
                {
                    Result[0] = "VALID";
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
        Session["PID"] = e.CommandArgument.ToString();
        Session["InOut"] = e.CommandName.ToString();
        Session["RowIndex"] = Row.RowIndex;
        lblProductIdvalue.Text = ((Label)Row.FindControl("lblGvProductCode")).Text;
        lblProductNameValue.Text = ((Label)Row.FindControl("lblgvProductName")).Text;
        gvSerialNumber.DataSource = null;
        gvSerialNumber.DataBind();
        gvStockwithManf_and_expiry.DataSource = null;
        gvStockwithManf_and_expiry.DataBind();
        txtSerialNo.Text = "";
        DataTable dt = new DataTable();
        if (Session["dtFinal"] == null)
        {
        }
        else
        {
            dt = (DataTable)Session["dtFinal"];
            dt = new DataView(dt, "ProductId='" + Session["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            Session["dtSerial"] = dt;

        }
        if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "SNO")
        {
            objPageCmn.FillData((object)gvSerialNumber, dt, "", "");
            txtselectedSerialNumber.Text = gvSerialNumber.Rows.Count.ToString();
            pnlSerialNumber.Visible = true;
            pnlexp_and_Manf.Visible = false;
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LE" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FE")
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
        }
        else if (objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "LM" || objProductM.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["PID"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString().ToUpper().Trim() == "FM")
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
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Serial_Number_Popup()", true);
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

            Dtfinal = new DataView(Dtfinal, "ProductId<>'" + Session["PID"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();


            Session["dtFinal"] = Dtfinal;
        }

        ((TextBox)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = "0";

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
        DataTable dt = new DataTable();

        if (Session["dtFinal"] == null)
        {
            dt = CreateProductDatatable();
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

                    DataRow dr = dt.NewRow();

                    dr[0] = Session["PID"].ToString();
                    dr[1] = fields[5].ToString();
                    dr[2] = "0";
                    dr[3] = "0";
                    dr[4] = "0";
                    dr[5] = DateTime.Now.ToString();
                    dr[6] = "0";
                    dr[7] = "OP";
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
                objPageCmn.FillData((Object)gvSerialNumber, dt, "", "");
                ((TextBox)GvProduct.Rows[(int)Session["RowIndex"]].FindControl("txtQuantity")).Text = recqty.ToString();
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
        //int counter = 0;
        //txtSerialNo.Text = "";
        //try
        //{

        //    string Path = Server.MapPath("~/Temp/" + FULogoPath.FileName);
        //    string[] csvRows = System.IO.File.ReadAllLines(Path);
        //    string[] fields = null;

        //    foreach (string csvRow in csvRows)
        //    {
        //        fields = csvRow.Split(',');

        //        if (fields[0].ToString() != "")
        //        {

        //            if (txtSerialNo.Text == "")
        //            {
        //                txtSerialNo.Text = fields[0].ToString();

        //            }
        //            else
        //            {
        //                txtSerialNo.Text += Environment.NewLine + fields[0].ToString();
        //            }

        //            counter++;

        //        }

        //    }


        //    if (Directory.Exists(Path))
        //    {
        //        try
        //        {
        //            Directory.Delete(Path);
        //        }
        //        catch
        //        {
        //        }
        //    }
        //    txtCount.Text = counter.ToString();
        //}
        //catch
        //{
        //    txtSerialNo.Text = "";

        //    DisplayMessage("File Not Found ,Try Again");

        //}
        //txtCount.Text = counter.ToString();

        //if (counter == 0)
        //{
        //    DisplayMessage("Serial Number Not Found");
        //}

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
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtSerial"];

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtSerial"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvSerialNumber, dt, "", "");


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
                dt = CreateProductDatatable();
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
            dt = CreateProductDatatable();
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

    public DataTable CreateProductDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ProductId");
        dt.Columns.Add("SerialNo", typeof(string));
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
        dt.Columns.Add("Width");
        dt.Columns.Add("Length");
        dt.Columns.Add("Pallet_ID");
        return dt;

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
                dt = CreateProductDatatable();
                DataRow dr = dt.NewRow();

                dr[0] = Session["PID"].ToString();
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
                dr[7] = "OP";
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
                dr[0] = Session["PID"].ToString();
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
                dr[7] = "OP";
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
    protected void GvProductDetail_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            string ProductID = ((Label)e.Row.FindControl("lblgvProductId")).Text;
            if (objProductM.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductID, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["MaintainStock"].ToString() == "SNO")
            {
                //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = false;

                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = true;
            }
            else
            {
                //((TextBox)e.Row.FindControl("txtgvSalesQuantity")).Enabled = true;

                ((LinkButton)e.Row.FindControl("lnkAddSerial")).Visible = false;
            }
        }
    }

    #region Date Searching
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlFieldName.SelectedItem.Value == "Vdate")
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

    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            string ext = FULogoPath.FileName.Substring(FULogoPath.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                FULogoPath.SaveAs(Server.MapPath("~/Temp/" + FULogoPath.FileName));
            }
        }
    }
}